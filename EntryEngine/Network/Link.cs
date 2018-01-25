﻿#if SERVER

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using EntryEngine.Serialize;
using System.IO;
using System.Linq;

namespace EntryEngine.Network
{
    public interface IConnector
    {
        //void Connect(string host, ushort port);
        Async Connect(string host, ushort port);
    }
	public abstract class Link : IDisposable
	{
		public static ushort MaxBuffer = 8192;
		public const int MAX_BUFFER_SIZE = sizeof(int);

		public abstract bool IsConnected { get; }
		public abstract bool CanRead { get; }
		public virtual IPEndPoint EndPoint { get { throw new NotImplementedException(); } }

		public void Write(byte[] data)
		{
			Write(data, 0, data.Length);
		}
		public abstract void Write(byte[] buffer, int offset, int size);
		protected internal abstract void WriteBytes(byte[] buffer, int offset, int size);
		public abstract byte[] Read();
		public abstract byte[] Flush();
		public abstract void Close();
        void IDisposable.Dispose()
        {
            Close();
        }

        
        static Link()
        {
            
        }
        
    }
	public class LinkMultiple : Link
	{
		internal IEnumerable<Link> Links;
        private CorEnumerator<byte[]> read;

        internal LinkMultiple()
        {
        }
		public LinkMultiple(IEnumerable<Link> links)
		{
            if (links == null)
                throw new ArgumentNullException("links");
			this.Links = links;
		}

		public override bool IsConnected
		{
            get
            {
                foreach (var link in Links)
                    if (link.IsConnected)
                        return true;
                return false;
            }
		}
		public override bool CanRead
		{
            get
            {
                foreach (var link in Links)
                    if (link.CanRead)
                        return true;
                return false;
            }
		}
		public override void Write(byte[] buffer, int offset, int size)
		{
			foreach (Link link in Links)
			{
				link.Write(buffer, offset, size);
			}
		}
		protected internal override void WriteBytes(byte[] buffer, int offset, int size)
		{
			foreach (Link link in Links)
			{
				link.WriteBytes(buffer, offset, size);
			}
		}
		public override byte[] Read()
		{
            if (read == null)
                read = new CorEnumerator<byte[]>(Links.SelectMany(l => ReadAll(l)));

            if (read.IsEnd)
            {
                read = null;
                return null;
            }

            byte[] data;
            read.Update(out data);
            return data;
		}
        private IEnumerable<byte[]> ReadAll(Link link)
        {
            while (true)
            {
                if (link.CanRead)
                {
                    byte[] data = link.Read();
                    if (data != null)
                        yield return data;
                    else
                        yield break;
                }
                else
                    yield break;
            }
        }
		public override byte[] Flush()
		{
			foreach (Link link in Links)
			{
				link.Flush();
			}
			return null;
		}
		public override void Close()
		{
			foreach (Link link in Links)
			{
				link.Close();
			}
		}
	}
	public abstract class LinkLink : Link
	{
		public virtual Link BaseLink
		{
			get;
			set;
		}
		public override bool IsConnected
		{
			get { return BaseLink.IsConnected; }
		}
		public override bool CanRead
		{
			get { return BaseLink.CanRead; }
		}
		public override IPEndPoint EndPoint
		{
			get { return BaseLink.EndPoint; }
		}

		public LinkLink()
		{
		}
		public LinkLink(Link baseLink)
		{
			this.BaseLink = baseLink;
		}

		public override void Write(byte[] buffer, int offset, int size)
		{
			BaseLink.Write(buffer, offset, size);
		}
		protected internal override void WriteBytes(byte[] buffer, int offset, int size)
		{
			BaseLink.WriteBytes(buffer, offset, size);
		}
		public override byte[] Read()
		{
			return BaseLink.Read();
		}
		public override byte[] Flush()
		{
			return BaseLink.Flush();
		}
		public override void Close()
		{
			BaseLink.Close();
		}
	}
	public abstract class LinkBinary : Link
	{
		protected byte[] buffer = new byte[MaxBuffer];
		protected ByteWriter writer = new ByteWriter(MaxBuffer);
        private int bigData = -1;

		protected abstract int DataLength { get; }
        public bool HasBigData
        {
            get { return bigData >= 0; }
        }
        protected virtual bool CanFlush { get { return writer.Position > 0; } }

		public sealed override void Write(byte[] buffer, int offset, int size)
		{
			int count = size + MAX_BUFFER_SIZE;
			if (size != 0)
				count += 4;
            //if (count > MaxBuffer)
            //    throw new ArgumentOutOfRangeException("package size");
            // cache full, flush the cache
            if (writer.Position + count > MaxBuffer)
                Flush();
            // write package size
            writer.Write(count);
            // write package content
            writer.WriteBytes(buffer, offset, size);
            // write crc valid while package has length
            if (size != 0)
                writer.Write(_IO.Crc32(buffer, offset, size));
            if (writer.Position > MaxBuffer)
            {
                // big data
                Flush();
                writer = new ByteWriter(MaxBuffer);
            }
		}
		protected internal sealed override void WriteBytes(byte[] buffer, int offset, int size)
		{
			writer.WriteBytes(buffer, offset, size);
		}
		public sealed override byte[] Read()
		{
			int available = DataLength;
			if (available >= MAX_BUFFER_SIZE)
			{
            BIG_DATA:
                if (HasBigData)
                {
                    // 读取大数据
                    if (bigData < buffer.Length)
                    {
                        int canReceive = _MATH.Min(buffer.Length - bigData, available);
                        InternalRead(buffer, bigData, canReceive);
                        bigData += canReceive;
                        available -= canReceive;
                    }
                    // 读取包尾crc验证
                    if (bigData == buffer.Length && available >= 4)
                    {
                        byte[] crc = new byte[4];
                        InternalRead(crc, 0, 4);
                        uint getCrc = BitConverter.ToUInt32(crc, 0);
                        uint calcCrc = _IO.Crc32(buffer, 0, buffer.Length);
                        if (getCrc != calcCrc)
                            throw new ArgumentException(string.Format("crc invalid! get={0} calc={1}", getCrc, calcCrc));
                        bigData = -1;
                        byte[] bigPackage = buffer;
                        buffer = new byte[MaxBuffer];
                        return bigPackage;
                    }
                    // stream has not received all data.
                    return null;
                }

				int peek;
				int receive = PeekSize(buffer, out peek);
                if (receive == 0)
                {
                    // no package
                    return null;
                }
				int size = BitConverter.ToInt32(buffer, 0);
				if (size < MAX_BUFFER_SIZE)
				{
					throw new ArgumentOutOfRangeException("size");
				}
				if (size == MAX_BUFFER_SIZE)
				{
                    if (peek < receive)
                    {
                        InternalRead(buffer, peek, receive);
                    }
                    return AgentHeartbeat.HeartbeatProtocol;
				}
                if (size > buffer.Length)
                {
                    // buffer为最终数据包
                    buffer = new byte[size - MAX_BUFFER_SIZE - 4];
                    bigData = 0;
                    // 去掉包头
                    if (peek < receive)
                        InternalRead(buffer, peek, receive);
                    available -= receive;
                    goto BIG_DATA;
                }
                if (size <= available)
                {
                    int packageSize = size - peek;
                    receive = InternalRead(buffer, peek, packageSize);
                    //if (receive != packageSize)
                    //{
                    //    throw new ArgumentException(string.Format("receive size invalid! receive={0} size={1}", receive, packageSize));
                    //}
                    packageSize = size - MAX_BUFFER_SIZE - 4;
                    uint getCrc = BitConverter.ToUInt32(buffer, size - 4);
                    uint calcCrc = _IO.Crc32(buffer, MAX_BUFFER_SIZE, packageSize);
                    if (getCrc != calcCrc)
                    {
                        throw new ArgumentException(string.Format("crc invalid! get={0} calc={1}", getCrc, calcCrc));
                    }
                    else
                    {
                        byte[] package = new byte[packageSize];
                        Utility.Copy(buffer, MAX_BUFFER_SIZE, package, 0, packageSize);
                        return package;
                    }
                }
                // stream has not received all data.
			}
			return null;
		}
        public sealed override byte[] Flush()
        {
            if (!CanFlush)
                return null;
            byte[] buffer = writer.GetBuffer();
            InternalFlush(buffer);
            writer.Reset();
            return buffer;
        }
		protected virtual int PeekSize(byte[] buffer, out int peek)
		{
            int read = InternalRead(buffer, 0, MAX_BUFFER_SIZE);
			peek = read;
			return read;
		}
		protected abstract int InternalRead(byte[] buffer, int offset, int size);
		protected abstract void InternalFlush(byte[] buffer);
		public override void Close()
		{
            writer.Reset();
            bigData = -1;
		}
    }
    /// <summary>将本地写入的数据进行读取</summary>
    public class LinkBinaryLocal : LinkBinary
    {
        private int read;

        public override bool IsConnected
        {
            get { return true; }
        }
        public override bool CanRead
        {
            get { return read < writer.Position; }
        }
        protected override int DataLength
        {
            get { return writer.Position; }
        }

        protected override int InternalRead(byte[] buffer, int offset, int size)
        {
            read += size;
            Array.Copy(writer.Buffer, 0, buffer, offset, size);
            return size;
        }
        protected override void InternalFlush(byte[] buffer)
        {
            read = 0;
        }
    }
    /// <summary>保护短时间内不重复发相同的数据</summary>
    public sealed class LinkRepeatPreventor : LinkLink
    {
        private class Package : PoolItem
        {
            public byte[] Buffer;
            public TIMER Timer;
        }

        private Pool<Package> packages = new Pool<Package>();
        public TimeSpan PreventTime = TimeSpan.FromSeconds(1);

        public override void Write(byte[] buffer, int offset, int size)
        {
            byte[] target = new byte[size];
            buffer.Copy(0, target, offset, size);
            foreach (Package package in packages)
            {
                if (package.Timer.ElapsedNow >= PreventTime)
                {
                    packages.RemoveAt(package);
                }
                else
                {
                    if (Utility.Equals(package.Buffer, target))
                    {
                        return;
                    }
                }
            }
            Package newP = new Package();
            newP.Buffer = target;
            newP.Timer = TIMER.StartNew();
            packages.Add(newP);
            base.Write(buffer, offset, size);
        }
    }
    /// <summary>一段时间内若有其他包则一起写入后发送</summary>
    public sealed class LinkBatchDelay : LinkLink
    {
        private TIMER time = new TIMER();
        public TimeSpan Delay = TimeSpan.FromMilliseconds(250);

        private void ResetTime()
        {
            time.Start();
        }
        public override void Write(byte[] buffer, int offset, int size)
        {
            ResetTime();
            base.Write(buffer, offset, size);
        }
        protected internal override void WriteBytes(byte[] buffer, int offset, int size)
        {
            ResetTime();
            base.WriteBytes(buffer, offset, size);
        }
        public override byte[] Flush()
        {
            if (time.Running)
            {
                if (time.ElapsedNow < Delay)
                    return null;
                time.Stop();
                return base.Flush();
            }
            else
                return null;
        }
    }

    // socket
    public class LinkSocket : LinkBinary, IConnector
    {
        protected internal Socket Socket
        {
            get;
            private set;
        }
        public override bool IsConnected
        {
            get
            {
                if (Socket != null && Socket.Connected)
                {
                    if (Socket.Poll(0, SelectMode.SelectRead) && Socket.Available == 0)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }
        public override bool CanRead
        {
            get { return Socket.Available >= MAX_BUFFER_SIZE; }
        }
        protected override int DataLength
        {
            get { return Socket.Available; }
        }
        public override IPEndPoint EndPoint
        {
            get { return Socket == null ? null : (IPEndPoint)Socket.RemoteEndPoint; }
        }

        public LinkSocket()
        {
        }
        public LinkSocket(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");
            this.Socket = socket;
            socket.SendBufferSize = MaxBuffer;
            socket.ReceiveBufferSize = MaxBuffer;
        }

        protected override int PeekSize(byte[] buffer, out int peek)
        {
            peek = 0;
            return Socket.Receive(buffer, 0, MAX_BUFFER_SIZE, SocketFlags.Peek);
        }
        protected override int InternalRead(byte[] buffer, int offset, int size)
        {
            SocketError error;
            int read = Socket.Receive(buffer, offset, size, SocketFlags.None, out error);
            if (error != SocketError.Success)
                throw new SocketException((int)error);
            return read;
        }
        protected override void InternalFlush(byte[] buffer)
        {
            SocketError error;
            Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, out error, null, null);
            //Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, out error, SendCallback, buffer);
            if (error != SocketError.Success)
                throw new SocketException((int)error);
        }
        //private void SendCallback(IAsyncResult ar)
        //{
        //    SocketError error;
        //    int sent = Socket.EndSend(ar, out error);
        //}
        public override void Close()
        {
            base.Close();
            if (Socket != null)
            {
                if (Socket.Connected)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                }
                Socket.Close();
                Socket = null;
            }
        }
        protected virtual Socket BuildSocket()
        {
            throw new NotImplementedException();
        }
        protected void BuildConnection()
        {
            if (IsConnected)
                throw new InvalidOperationException("try connect while socket has been connected.");
            Close();
            Socket = BuildSocket();
            Socket.SendBufferSize = MaxBuffer;
            Socket.ReceiveBufferSize = MaxBuffer;
        }
        public void ConnectSync(string host, ushort port)
        {
            BuildConnection();
            Socket.Connect(host, port);
        }
        public Async Connect(string host, ushort port)
        {
            BuildConnection();
            AsyncData<Socket> async = new AsyncData<Socket>();
            Socket.BeginConnect(host, port, ar =>
            {
                Socket.EndConnect(ar);
                try
                {
                    if (Socket.Connected)
                        async.SetData(Socket);
                    else
                        async.Cancel();
                }
                catch (Exception ex)
                {
                    async.Error(ex);
                }
            }, Socket);
            async.Run();
            return async;
        }
    }
	public class LinkTcp : LinkSocket
	{
		public LinkTcp()
		{
		}
        public LinkTcp(Socket socket) : base(socket)
		{
		}

        protected override Socket BuildSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

		public static LinkTcp Connect(string host, ushort port, Action<Socket> onConnect)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			if (onConnect != null)
				onConnect(socket);
			socket.Connect(host, port);
			return new LinkTcp(socket);
		}
        public static AsyncData<LinkTcp> ConnectAsync(string host, ushort port, Action<Socket> onConnect)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (onConnect != null)
                onConnect(socket);
            AsyncData<LinkTcp> async = new AsyncData<LinkTcp>();
            socket.BeginConnect(host, port,
                e =>
                {
                    try
                    {
                        socket.EndConnect(e);
                        async.SetData(new LinkTcp(socket));
                    }
                    catch (Exception ex)
                    {
                        async.Error(ex);
                    }
                }, socket);
            return async;
        }
    }
    [Code(ECode.ToBeContinue | ECode.MayBeReform)]
    public class LinkUdp : LinkSocket
    {
        private IPEndPoint endPoint;

        public override IPEndPoint EndPoint
        {
            get { return endPoint == null ? base.EndPoint : endPoint; }
        }

        public LinkUdp()
		{
		}
        public LinkUdp(Socket socket) : base(socket)
		{
		}
        public LinkUdp(Socket socket, IPEndPoint endPoint) : base(socket)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");
            this.endPoint = endPoint;
            Connect(endPoint.Address.ToString(), (ushort)endPoint.Port);
        }

        public void Bind(string ip, ushort port)
        {
            Bind(new IPEndPoint(IPAddress.Parse(ip), port));
        }
        public void Bind(IPEndPoint ep)
        {
            if (ep == null)
                throw new ArgumentNullException("ep");
            if (Socket == null)
                BuildConnection();
        }
        protected override Socket BuildSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
    }

    // http
	public class LinkHttpRequest : LinkBinary
	{
        /// <summary>超时时间(ms)，小于0则不超时</summary>
		public int Timeout = 5000;
        /// <summary>重连次数，小于0则无限重连</summary>
		public int ReconnectCount = 3;
		private int reconnect;
		private int length;
        public HttpRequestPost Http
        {
            get;
            private set;
        }
		protected override int DataLength
		{
			get { return length; }
		}
		public override bool IsConnected
		{
            get { return Http.IsConnected; }
		}
		public override bool CanRead
		{
			get
			{
                //if (request == null && Ex != null)
                //{
                //    throw Ex;
                //}
				return Http.HasResponse && length > 0;
			}
		}
		public WebException Ex
		{
			get;
			private set;
		}

        public LinkHttpRequest()
        {
            Http = new HttpRequestPost();
            Http.OnConnect += Http_OnConnect;
            Http.OnError += Http_OnError;
            Http.OnReceived += Http_OnReceived;
        }

        void Http_OnReceived(HttpWebResponse obj)
        {
            length = (int)obj.ContentLength;
        }
        void Http_OnError(WebException ex, byte[] buffer)
        {
            Ex = ex;
            if (ex.Status == WebExceptionStatus.Timeout)
            {
                _LOG.Debug("请求超时:byte[{0}], Timeout:{1}ms", buffer.Length, Timeout);
                reconnect++;
                if (ReconnectCount < 0 || reconnect <= ReconnectCount)
                {
                    Http.Reconnect();
                    InternalFlush(buffer);
                }
            }
            else if (ex.Status == WebExceptionStatus.RequestCanceled)
            {
                _LOG.Debug("request canceled");
            }
            else if (ex.Status == WebExceptionStatus.NameResolutionFailure)
            {
                _LOG.Debug("服务器不存在");
            }
            else
            {
                _LOG.Debug("request error! msg={0} code={1}", ex.Message, ex.Status);
            }
        }
        void Http_OnConnect(HttpWebRequest obj)
        {
            obj.Timeout = Timeout;
        }

        protected override void InternalFlush(byte[] buffer)
        {
            Http.Send(buffer);
        }
		protected override int InternalRead(byte[] buffer, int offset, int size)
		{
			try
			{
				int read = Http.Response.GetResponseStream().Read(buffer, offset, size);
				length -= read;
				return read;
			}
			catch (WebException ex)
			{
				Ex = ex;
				_LOG.Error(ex, "Read error!");
				throw ex;
			}
		}
		public void Connect(string uri)
		{
            Http.Connect(uri);
		}
		public override void Close()
		{
			base.Close();
            Http.Dispose();
		}
	}
    public class HttpRequestPost : IDisposable
    {
        public event Action<HttpWebRequest> OnConnect;
        /// <summary>异步</summary>
        public event Action<HttpWebRequest, Stream> OnSend;
        /// <summary>异步</summary>
        public event Action<HttpWebRequest, Stream> OnSent;
        /// <summary>异步</summary>
        public event Action<HttpWebResponse> OnReceived;
        /// <summary>异步</summary>
        public event Action<WebException, byte[]> OnError;
        private Uri uri;

        public bool IsConnected
        {
            get { return Request != null; }
        }
        protected HttpWebRequest Request
        {
            get;
            private set;
        }
        public bool HasResponse
        {
            get { return Response != null; }
        }
        public HttpWebResponse Response
        {
            get;
            private set;
        }

        public void Reconnect()
        {
            Uri uri = Request == null ? this.uri : Request.RequestUri;
            Dispose();
            Connect(uri);
        }
        public void Connect(Uri uri)
        {
            if (Request != null)
                throw new InvalidOperationException();

            if (uri == null)
                throw new ArgumentNullException("uri");

            this.uri = uri;
            Request = (HttpWebRequest)HttpWebRequest.Create(uri);
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "POST";
            if (OnConnect != null)
                OnConnect(Request);
        }
        public void Connect(string uri)
        {
            Connect(new Uri(uri));
        }
        public void Send(byte[] buffer)
        {
            Request.BeginGetRequestStream(
                ar =>
                {
                    Stream connection = null;
                    try
                    {
                        // .net3.5: RequestStream不关闭，GetResponse不能发出请求
                        // .net4.0: RequestStream不关闭，GetResponse可以发出请求，此时KeepAlive=true
                        connection = Request.EndGetRequestStream(ar);
                        if (OnSend != null)
                            OnSend(Request, connection);
                        if (buffer != null && buffer.Length > 0)
                            connection.Write(buffer, 0, buffer.Length);
                        if (OnSent != null)
                            OnSent(Request, connection);
                    }
                    catch (WebException ex)
                    {
                        Dispose();
                        if (OnError != null)
                            OnError(ex, buffer);
                    }
                    catch (Exception ex)
                    {
                        Dispose();
                        _LOG.Error(ex, "HttpRequestPost write stream data error!");
                    }
                    finally
                    {
                        connection.Close();
                    }

                    Request.BeginGetResponse((ar2) =>
                    {
                        try
                        {
                            var response = (HttpWebResponse)Request.EndGetResponse(ar2);
                            this.Response = response;
                            if (OnReceived != null)
                                OnReceived(response);
                        }
                        catch (WebException ex)
                        {
                            Dispose();
                            if (OnError != null)
                                OnError(ex, buffer);
                        }
                        catch (Exception ex)
                        {
                            Dispose();
                            _LOG.Error(ex, "HttpRequestPost send error!");
                        }
                    }, Request);

                }, Request);
        }
        public void Dispose()
        {
            if (Request != null)
            {
                Request.Abort();
                Request = null;
            }
            if (Response != null)
            {
                Response.Close();
                Response = null;
            }
        }
    }
    public abstract class LinkQueue<T> : LinkLink where T : Link
    {
        protected LinkedList<T> queue
        {
            get;
            private set;
        }
        public sealed override bool CanRead
        {
            get
            {
                if (BaseLink != null)
                    return base.CanRead;
                if (queue.Count == 0)
                    return false;
                var link = queue.First.Value;
                if (!link.IsConnected)
                    //throw new WebException("Can't receive any data.", WebExceptionStatus.Timeout);
                    return true;
                return link.CanRead;
            }
        }
        public virtual bool CanFlush
        {
            get { return BaseLink != null && !BaseLink.CanRead; }
        }

        public LinkQueue()
        {
            queue = new LinkedList<T>();
        }

        protected T Dequeue()
        {
            var link = queue.First.Value;
            queue.RemoveFirst();
            return link;
        }
        public sealed override byte[] Flush()
        {
            if (!CanFlush)
                return null;
            byte[] flush = base.Flush();
            if (flush == null)
                return flush;
            FlushOver(flush);
            BaseLink = null;
            return flush;
        }
        protected abstract void FlushOver(byte[] flush);
        public override void Close()
        {
            if (BaseLink != null)
                base.Close();
            if (queue == null)
                return;
            while (queue.Count > 0)
                Dequeue().Close();
            queue = null;
        }
    }
    public class LinkLongHttpRequest : LinkQueue<LinkHttpRequest>, IConnector
    {
        private static byte[] REQUEST_ID;

        private byte[] idBuffer;
        public TimeSpan Timeout = TimeSpan.FromSeconds(5);
        public int ReconnectCount = 3;
        public TimeSpan KeepAlive = TimeSpan.FromMinutes(5);

        public ushort ID
        {
            get;
            private set;
        }
        public string Uri
        {
            get;
            private set;
        }
        public override bool IsConnected
        {
            get { return idBuffer != null; }
        }

        private LinkHttpRequest GetRequestLink(bool keepAlive)
        {
            LinkHttpRequest request = new LinkHttpRequest();
            if (keepAlive)
            {
                request.Http.OnSend += FlushWithID;
                request.Timeout = (int)KeepAlive.TotalMilliseconds;
                request.ReconnectCount = -1;
            }
            else
            {
                request.Http.OnSend += FlushWithPID;
                request.Timeout = (int)Timeout.TotalMilliseconds;
                request.ReconnectCount = ReconnectCount;
            }
            request.Connect(Uri);
            return request;
        }
        private void FlushWithID(HttpWebRequest request, Stream obj)
        {
            // 客户端ID
            obj.Write(idBuffer, 0, 2);
        }
        private void FlushWithPID(HttpWebRequest request, Stream obj)
        {
            // 客户端ID
            obj.Write(idBuffer, 0, 2);
            // 数据包ID
            idBuffer[2]++;
            obj.Write(idBuffer, 2, 1);
        }
        public Async Connect(string host, ushort port)
        {
            if (!string.IsNullOrEmpty(Uri))
                throw new InvalidOperationException("Http has connected.");

            AsyncData<LinkLongHttpRequest> async = new AsyncData<LinkLongHttpRequest>();

            HttpRequestPost request = new HttpRequestPost();
            request.OnSend += RequestID;
            request.OnReceived += (response) =>
            {
                byte[] buffer = new byte[3];
                using (var stream = response.GetResponseStream())
                    stream.Read(buffer, 0, 2);
                request.Dispose();

                this.Uri = host;
                this.ID = BitConverter.ToUInt16(buffer, 0);
                this.idBuffer = buffer;
                async.SetData(this);
            };
            request.Connect(host);
            request.Send(null);

            return async;
        }
        public override void Write(byte[] buffer, int offset, int size)
        {
            if (BaseLink == null)
                BaseLink = GetRequestLink(false);
            base.Write(buffer, offset, size);
        }
        protected override void FlushOver(byte[] flush)
        {
            queue.AddFirst((LinkHttpRequest)BaseLink);
        }
        public override byte[] Read()
        {
            byte[] data;
            var link = queue.First.Value;
            if (link.IsConnected)
            {
                data = link.Read();
                if (data == null)
                    return null;
                // 已经读取完所有数据
                if (!link.CanRead)
                    Dequeue().Close();
            }
            else
            {
                Dequeue();
                data = null;
            }
            // 保持一个长连接
            if (queue.Count == 0)
            {
                LinkHttpRequest _keep = GetRequestLink(true);
                _keep.Http.Send(null);
                queue.AddLast(_keep);
            }
            return data;
        }

        static LinkLongHttpRequest()
        {
            REQUEST_ID = BitConverter.GetBytes(ushort.MaxValue);
        }
        static void RequestID(HttpWebRequest request, Stream obj)
        {
            obj.Write(REQUEST_ID, 0, 2);
        }
    }
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode
        {
            get;
            private set;
        }
        public HttpException(HttpStatusCode code)
        {
            this.StatusCode = code;
        }
    }

    // agent and stub
    public abstract class Agent
    {
        public virtual Link Link { get; set; }
        public virtual IEnumerable<byte[]> Receive()
        {
            while (Link.CanRead)
            {
                byte[] package = Link.Read();
                if (package != null)
                    yield return package;
                else
                    yield break;
            }
        }
        public abstract void OnProtocol(byte[] package);
    }
	public abstract class Agent_Link : Agent
	{
        public virtual EntryEngine.Network.Agent Base { get; set; }
        public override EntryEngine.Network.Link Link
        {
            get { return Base.Link; }
            set { Base.Link = value; }
        }

        public Agent_Link() { }
        public Agent_Link(EntryEngine.Network.Agent Base) { this.Base = Base; }

        public override System.Collections.Generic.IEnumerable<byte[]> Receive()
        {
            return Base.Receive();
        }
        public override void OnProtocol(byte[] package)
        {
            Base.OnProtocol(package);
        }
	}
	public class AgentHeartbeat : Agent_Link
	{
		internal static byte[] HeartbeatProtocol = new byte[0];
        /// <summary>超出时间发出心跳包</summary>
		public TimeSpan Heartbeat = TimeSpan.FromSeconds(60);
        /// <summary>超出时间视为已断开</summary>
		public TimeSpan HeartbeatOvertime = TimeSpan.FromSeconds(90);

		private bool beat;
		private TIMER heartbeat;

        public AgentHeartbeat(Agent baseAgent) : base(baseAgent)
		{
		}
        public AgentHeartbeat(Agent baseAgent, bool ping) : base(baseAgent)
		{
			if (ping)
			{
				heartbeat.Start();
			}
		}
        public AgentHeartbeat(Agent baseAgent, TimeSpan heartbeat, TimeSpan overtime) : this(baseAgent, true)
        {
            this.Heartbeat = heartbeat;
            this.HeartbeatOvertime = overtime;
        }

		private void Beat()
		{
			Link.Write(HeartbeatProtocol);
		}
		public override IEnumerable<byte[]> Receive()
		{
			bool flag = false;
			foreach (byte[] item in base.Receive())
			{
				if (!flag)
				{
					heartbeat.StopAndStart();
					beat = false;
					flag = true;
				}
				if (item.Length == 0)
				{
					Beat();
					continue;
				}
				yield return item;
			}
			if (!flag)
			{
				if (heartbeat.Running)
				{
					TimeSpan elapsed = heartbeat.ElapsedNow;
					if (elapsed >= HeartbeatOvertime)
					{
						_LOG.Info("{0} heartbeat stop", Link.EndPoint);
						Link.Close();
					}
					else if (elapsed >= Heartbeat && !beat)
					{
						Beat();
						beat = true;
					}
				}
			}
		}
	}
	public class AgentProtocolStub : Agent
	{
		private Dictionary<byte, Stub> protocols = new Dictionary<byte, Stub>();

        public AgentProtocolStub()
        {
        }
        public AgentProtocolStub(Link link)
        {
            this.Link = link;
        }
        public AgentProtocolStub(params Stub[] stubs)
        {
            foreach (var stub in stubs)
                AddAgent(stub);
        }
        public AgentProtocolStub(Link link, params Stub[] stubs)
        {
            this.Link = link;
            foreach (var stub in stubs)
                AddAgent(stub);
        }

		public void AddAgent(Stub stub)
		{
			if (stub == null)
				throw new ArgumentNullException("agent");
            stub.ProtocolAgent = this;
			protocols.Add(stub.Protocol, stub);
		}
		public override void OnProtocol(byte[] data)
		{
			ByteReader reader = new ByteReader(data);
            while (true)
            {
                int position = reader.Position;
                byte protocol;
                ushort index;
                reader.Read(out protocol);
                reader.Read(out index);

                Stub agent;
                if (!protocols.TryGetValue(protocol, out agent))
                    throw new NotImplementedException("no procotol: " + protocol);

                agent[index](reader);
                if (position == reader.Position || reader.IsEnd)
                    break;
            }
		}
	}
    // be used to generate code
    public abstract class Stub
    {
        private Dictionary<ushort, Action<ByteReader>> stubs = new Dictionary<ushort, Action<ByteReader>>();
        protected internal byte Protocol
        {
            get;
            protected set;
        }
        protected internal AgentProtocolStub ProtocolAgent;
        public Link Link
        {
            get { return ProtocolAgent.Link; }
        }
        internal Action<ByteReader> this[ushort index]
        {
            get
            {
                Action<ByteReader> stub;
                if (!stubs.TryGetValue(index, out stub))
                    throw new ArgumentOutOfRangeException(string.Format("protocol: {0} method stub {1} not find!", Protocol, index));
                return stub;
            }
        }
        protected Stub()
        {
        }
        public Stub(byte protocol)
        {
            this.Protocol = protocol;
        }
        protected void AddMethod(Action<ByteReader> method)
        {
            AddMethod((ushort)stubs.Count, method);
        }
        protected void AddMethod(ushort id, Action<ByteReader> method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            stubs.Add(id, method);
        }
    }
    public abstract class StubClientAsync : Stub
    {
        public class AsyncWaitCallback : ICoroutine
        {
            private StubClientAsync stub;
            public event Action<sbyte, string> OnError;

            public bool IsEnd
            {
                get;
                internal set;
            }
            public byte ID
            {
                get;
                private set;
            }
            public sbyte Ret
            {
                get;
                private set;
            }
            public string Msg
            {
                get;
                private set;
            }
            public Delegate Function
            {
                get;
                private set;
            }

            internal AsyncWaitCallback(StubClientAsync stub, Delegate function, byte id)
            {
                if (stub == null)
                    throw new ArgumentNullException("stub");
                this.stub = stub;
                this.Function = function;
                this.ID = id;
            }

            internal void Error(sbyte ret, string msg)
            {
                this.Ret = ret;
                this.Msg = msg;
                if (OnError != null)
                    OnError(ret, msg);
                this.IsEnd = true;
            }
            void ICoroutine.Update(GameTime time)
            {
            }
        }

        private Dictionary<byte, AsyncWaitCallback> requests = new Dictionary<byte, AsyncWaitCallback>();
        private byte id;
        public event Action<ushort, sbyte, string> OnAsyncCallbackError;
        public event Action OnAsyncRequestFull;
        public event Action<Exception> OnAgentError;

        protected bool HasRequest
        {
            get { return requests.Count > 0; }
        }
        public bool IsAsyncRequestFull
        {
            get
            {
                byte next = (byte)(id + 1);
                return requests.ContainsKey(next);
            }
        }

        protected AsyncWaitCallback Push(Delegate method)
        {
            if (requests.ContainsKey(id))
            {
                if (OnAsyncRequestFull == null)
                {
                    throw new InsufficientMemoryException("stack full");
                }
                else
                {
                    OnAsyncRequestFull();
                    return null;
                }
            }
            var async = new AsyncWaitCallback(this, method, id);
            requests.Add(id, async);
            id++;
            return async;
        }
        protected AsyncWaitCallback Pop(byte id)
        {
            AsyncWaitCallback async;
            if (requests.TryGetValue(id, out async))
            {
                async.IsEnd = true;
                requests.Remove(id);
            }
            else
                throw new KeyNotFoundException(id.ToString());
            return async;
        }
        protected void Error(AsyncWaitCallback async, ushort key, sbyte ret, string msg)
        {
            async.Error(ret, msg);
            if (OnAsyncCallbackError != null)
                OnAsyncCallbackError(key, ret, msg);
        }
        public void Update(int packageCount)
        {
            Update(null, packageCount);
        }
        public void Update(Agent agent, int packageCount)
        {
            Link link = Link;
            if (agent == null)
            {
                agent = ProtocolAgent;
                link = Link;
            }
            if (agent != null && link != null && link.IsConnected)
            {
                link.Flush();
                foreach (var item in agent.Receive())
                {
                    try
                    {
                        agent.OnProtocol(item);
                        if (--packageCount == 0)
                            break;
                    }
                    catch (Exception ex)
                    {
                        if (OnAgentError == null)
                            throw ex;
                        else
                            OnAgentError(ex);
                    }
                }
            }
        }
    }

	[AttributeUsage(AttributeTargets.Interface)]
	public class ProtocolStubAttribute : Attribute
	{
		private byte protocol;
		private Type callback;
		private string agentType;
		public byte Protocol { get { return protocol; } }
		public Type Callback { get { return callback; } }
		public string AgentType { get { return agentType; } }
		public ProtocolStubAttribute(byte protocol, Type callback)
		{
			this.protocol = protocol;
			this.callback = callback;
		}
		public ProtocolStubAttribute(byte protocol, Type callback, string agentType)
		{
			this.protocol = protocol;
			this.callback = callback;
			this.agentType = agentType;
		}
	}
}

#endif