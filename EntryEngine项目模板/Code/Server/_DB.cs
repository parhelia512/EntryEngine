﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using EntryEngine;
using EntryEngine.Serialize;
using EntryEngine.Network;

namespace Server
{
    public partial class _DB
    {
        // 网络请求通用方法
        public static string HttpRequest(string url, Action<string> callback, string method = "GET", Dictionary<string, string> headers = null, string postData = null, int timeout = 5000, bool keepAlive = true, bool protocalVersion10 = false, string userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            // 基本设置
            request.Method = method;
            request.KeepAlive = keepAlive;
            request.ProtocolVersion = protocalVersion10 ? HttpVersion.Version10 : HttpVersion.Version11;
            request.UserAgent = userAgent;
            request.Timeout = timeout;
            // 头部设置
            if (headers == null)
                headers = new Dictionary<string, string>();
            if (headers.ContainsKey("Content-Type"))
            {
                request.ContentType = headers["Content-Type"];
                headers.Remove("Content-Type");
            }
            else
            {
                request.ContentType = "text/html;charset=UTF-8";
            }
            foreach (var header in headers)
                request.Headers[header.Key] = header.Value;
            // POST数据
            if (!string.IsNullOrEmpty(postData))
            {
                request.Method = "POST";
                using (var stream = request.GetRequestStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    stream.Write(data, 0, data.Length);
                }
            }
            // 发出请求
            if (callback == null)
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                    return reader.ReadToEnd();
            }
            else
            {
                request.BeginGetResponse((async) =>
                {
                    try
                    {
                        var response = request.EndGetResponse(async);
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            callback(reader.ReadToEnd());
                    }
                    catch (Exception ex)
                    {
                        _LOG.Warning("HttpRequestAsync异步请求异常：{0}\r\nStack: {1}", ex.Message, ex.StackTrace);
                    }
                }, request);
                return null;
            }
        }
        public static string HttpGet(string url, Dictionary<string, string> headers = null)
        {
            return HttpRequest(url, null, "GET", headers);
        }
        public static string HttpPost(string url, Dictionary<string, string> headers = null, string postData = null)
        {
            return HttpRequest(url, null, "POST", headers, postData);
        }
        public static IPAddress GetRemoteIP(HttpListenerContext context)
        {
            var xff = context.Request.Headers["X-Real-IP"];
            if (!string.IsNullOrWhiteSpace(xff)) return IPAddress.Parse(xff);
            else return context.Request.RemoteEndPoint.Address;
        }
        /// <summary>发送下载内容给前端</summary>
        public static void Download(HttpListenerContext __context, byte[] buffer, string filename = null, string contentType = "application/octet-stream")
        {
            __context.Response.ContentType = contentType;
            // 指定下载文件名
            __context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", filename));
            __context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            __context.Response.Close();
        }
        /// <summary>发送下载内容给前端，编码格式为Response.ContentEncoding</summary>
        public static void Download(HttpListenerContext __context, string buffer, string filename = null, string contentType = "application/octet-stream")
        {
            Download(__context, __context.Response.ContentEncoding.GetBytes(buffer), filename, contentType);
        }
        

        // 发送验证码
        public static void SendSMSCode(T_SMSCode sms)
        {
            HttpRequest(
                string.Format("https://api.smsbao.com/sms?u=yyhlm&p=24e7bd23c11c471091ff857773b46687&m={0}&c=【暗和科技】您的验证码是{1}，请不要随意告诉他人哦。",
                sms.Mobile, sms.Code), null);
        }

        public interface IPayResult
        {
            /// <summary>是否支付成功</summary>
            bool IsTradeSuccess { get; }
            /// <summary>支付时间</summary>
            DateTime PayTime { get; }
            /// <summary>订单号</summary>
            string Voucher { get; }
        }
        public class TestPayResult : IPayResult
        {
            public bool IsTradeSuccess { get; set; }
            public DateTime PayTime { get; set; }
            public string Voucher { get; set; }
            public TestPayResult(bool success)
            {
                IsTradeSuccess = success;
                PayTime = DateTime.Now;
                if (IsTradeSuccess)
                    Voucher = PayTime.Ticks.ToString();
            }
        }

        #region 支付宝支付
        // 需要更高的.net framework版本
        // 需要以下dll，已经放入Launch\Server\
        // AlipaySDKNet.dll
        // Aliyun.MNS.dll
        // Aliyun.OSS.dll
        // aliyun-net-sdk-core.dll
        // aliyun-net-sdk-dybaseapi.dll
        // aliyun-net-sdk-dysmsapi.dll
        // jose-jwt.dll
        // Newtonsoft.Json.dll
        /// <summary>支付宝支付</summary>
        //public static class _ZFB
        //{
        //    // 正式环境
        //    const string URL = "https://openapi.alipay.com/gateway.do";
        //    // 沙箱环境
        //    //const string URL = "https://openapi.alipaydev.com/gateway.do";

        //    public static string APP_ID = "20210031********";
        //    /// <summary>应用私钥和下面的应用公钥，可以在支付宝网页版加签工具生成https://miniu.alipay.com/keytool/create</summary>
        //    public static string APP_PRIVATE_KEY = "MIIEpQIBAAKCAQEAgDotn4+sxAUo29ayC5aXgSxeNuTYgPfqxUZbvrCuK2ZkQ0OUz4m+iKYLn6d5DgnzKwFJLCJJ4aSAi/2MB95H+/Hiq/iGGQj5YLSIwtzmlVTDisT9RRMO/Mgl4rNq0TkgETD8jZKNrY6zSZ5Hvy5/tgnNkcoH65F4nL3L+OgYqKks6pIrnlzXpqe4V7V0a50u5/Cort8ykLS6GcWX1or7w6TGmCEzriBLTxiC4imSo2Zu3g6FYLPofZ7wUy3YPEUWFF5lvCjKDuiBnFeTTgp3kWhUx1RH3tUDkbihO7C65EZOSfVDzyRrUcYgGH6LIAGouIaHX6vN/6vM4F6KwWwXSwIDAQABAoIBAEpQbWySOhCI5Psz3JA2wKuOaTPrQUbNZ/TZKAbGIsroVqddHXuCWzia8xWeW9w1DAcagavgW204h3+afHN68cEkmLgOGrmbp9vSBYjZuZFGROXB8P79YqxB2yMd1IRZVSphd50dGJtDnsjFwNMeQcnguJELw7dU4dAFd5dT/CaSvsp0fNzFcAPwZY/8O5jUuaVIFP2CdMEEt0nQusQZGL9ue6aDoz3HKuBdiDK/uLCzUZSEPXnmTL1uCITINT199WLJ9lvdvfCoNNpG+vM+v5V76vZVvSEkNgC89Haw3TlBGuIGaxgNZGfOeXZZUHkT5sA8bNC3oRbCwuhibtyTN8ECgYEA39iflFnXt4i0RYX4bT9fUoY6PFZtR1GER2wWFNp8VKwIjNRlTifqOONYGFJjSsZ8Jl1cxpp+hmkf4xq1i3QriP9zi4X0BEq21UoG2LpKR9Ltu7nnT7+QvjC1xoCxNpKL4XDMirQ+9Uy8KXmDC07wWOxv2Ne7lHuQbmfHx0Z8ZdsCgYEAkqVpNOUlgYqqOmcSbVTCtCg40Gkp/M0qN9WZltJZz9bDSJlzqJW15ifPL8XsrPzSivzOeKZlpqQ6pADOSgUu85e0ll4iKGks9Vpx6JHTOm6jyjndXqHTWq8+NmkqRrbrv3JPlBzSGb2TPF/1Nke+Xrf+0KC9HAvFdnipJ846p1ECgYEAvy7fLO+HBKRng7GmungTzAIEnyAZ+X2wAuhX+7uX0SGVs+J8G8KPk8LorO1BDM51nrbC1IWDZv1GVMutHsw7mqjDYPkprri5a3XsXxLM+oc2sM1YuI4e67HirwWfVcLYYdXbfOPxmcTOOIYl3HSxZuGZrZSkC291rjZJNPQIr10CgYEAjQMH3ng2D5HyTMSOQJmPvEMtFqL5YAE9BoGb9h6BhEzEbcw5HjQPvKgtH4gYJOPb5RBhzjxbZNlpFgk8VIsVceFAIpOUDv3L4IY/IF8RGZAIac4oovXDUeFPVmzb3THKEcbu5MKt+ViE+zpehfqJAXW2TpEyJ4TeNSSjrAYv+nECgYEAjF4MmCNibz5wbh468mRglayvFJ0UUCM3ltGedKcmnvB0c5XH5xkJAm3bkYNx4o593hloX18Emy5z6ua5jJ2FZygvOgYfn2ClWsvPN7IYWtaR+69OTUb0RGUl3Z4LkU1FhM6KkPQGjIx4NOi9TrQT+YXMsOA85BY1SpLWkuA7LSg=";
        //    // 应用公钥，在应用后台生成支付宝公钥
        //    //public static string APP_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgDotn4+sxAUo29ayC5aXgSxeNuTYgPfqxUZbvrCuK2ZkQ0OUz4m+iKYLn6d5DgnzKwFJLCJJ4aSAi/2MB95H+/Hiq/iGGQj5YLSIwtzmlVTDisT9RRMO/Mgl4rNq0TkgETD8jZKNrY6zSZ5Hvy5/tgnNkcoH65F4nL3L+OgYqKks6pIrnlzXpqe4V7V0a50u5/Cort8ykLS6GcWX1or7w6TGmCEzriBLTxiC4imSo2Zu3g6FYLPofZ7wUy3YPEUWFF5lvCjKDuiBnFeTTgp3kWhUx1RH3tUDkbihO7C65EZOSfVDzyRrUcYgGH6LIAGouIaHX6vN/6vM4F6KwWwXSwIDAQAB";
        //    /// <summary>支付宝公钥，可以用于最终的付款</summary>
        //    public static string APP_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlMJws27InXL7ZldgtZ3wcycucV4DQgt5yYN2XPALKdeOqo2L7GLfk8fQzXIrGsaMAMY3iCCcmh4vN8tjz9KRLFZzZglGBA4XkQyQk8WeSvUR5xN/SfEDgE9KtcVyyW23u+s5juMKkSi/NICJ+toLpIGVu6KpK0tJPok4TzUS3MHoz4qx7STvgW+m1CbHW+nl8l1F36A7EGlaDa1yUdfq2acN25XDFfZ/T+OZ94zrlD2wk1Ug8qjpuJChK8OzxHtpQcz4YVveWs56k+K9O0027vnuWNcblC0QFJT7vVxR1b+BgwGdgDWwYaA4AytstJxSys57wBNiOl6e2Y743CVsbwIDAQAB";

        //    /// <summary>支付完成后通知服务端的地址</summary>
        //    public static string NOTIFY_URL = "http://8.134.82.110:30005/Action/0/AlipayCallback";
        //    /// <summary>支付完成后前端的跳转地址</summary>
        //    public static string RETURN_URL = "https://8.134.82.110:31000/pages/oder/paySuccess";
        //    /// <summary>H5网页支付的网页html内容</summary>
        //    const string BODY = "<meta charset=\"utf-8\"><!-- {0} -->\r\n{1}";


        //    private static Dictionary<string, string> OrderPageCodes = new Dictionary<string, string>();
        //    public static string GetOrderPageCode(string out_trade_no)
        //    {
        //        string result;
        //        if (!OrderPageCodes.TryGetValue(out_trade_no, out result))
        //            result = "<h1 text-align:'center'>支付页已过期，请重新下单</h1>";
        //        return result;
        //    }

        //    static DefaultAopClient GetClient()
        //    {
        //        DefaultAopClient client = new DefaultAopClient(URL, APP_ID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", APP_PUBLIC_KEY, "utf-8", false);
        //        client.SetTimeout(3000);
        //        client.notify_url = NOTIFY_URL;
        //        return client;
        //    }

        //    class ReqOrderPage
        //    {
        //        public string out_trade_no;
        //        public string product_code = "FAST_INSTANT_TRADE_PAY";
        //        public double total_amount;
        //        /// <summary>订单标题</summary>
        //        public string subject;
        //        /// <summary>订单描述</summary>
        //        public string body;
        //        /// <summary>公用回传参数，如果请求时传递了该参数，则返回给商户时会回传该参数。支付宝会在异步通知时将该参数原样返回。本参数必须进行UrlEncode之后才可以发送给支付宝。</summary>
        //        //public string passback_params;
        //        /// <summary>该笔订单允许的最晚付款时间，逾期将关闭交易</summary>
        //        public string timeout_express = "60m";
        //    }
        //    public class RetAlipay
        //    {
        //        /// <summary>10000</summary>
        //        public string code;
        //        /// <summary>Success</summary>
        //        public string msg;

        //        public void Check()
        //        {
        //            if (code != "10000")
        //            {
        //                _LOG.Error("支付宝异常：{0}", msg);
        //                throw new Exception("支付宝异常");
        //            }
        //        }
        //    }
        //    public class RetOrderQueryData : RetAlipay, IPayResult
        //    {
        //        /// <summary>买家登录的账号：137******63</summary>
        //        public string buyer_logon_id;
        //        /// <summary>买家支付宝的账号：2088702599874210</summary>
        //        public string buyer_user_id;
        //        /// <summary>我们系统的订单号</summary>
        //        public string out_trade_no;
        //        /// <summary>实付款：0.01</summary>
        //        public string total_amount;
        //        /// <summary>支付宝订单号</summary>
        //        public string trade_no;
        //        /// <summary>交易结果：TRADE_SUCCESS</summary>
        //        public string trade_status;
        //        /// <summary>支付时间：2020-03-23 15:20:55</summary>
        //        public string send_pay_date;

        //        public bool IsTradeSuccess { get { return trade_status == "TRADE_SUCCESS"; } }
        //        public DateTime PayTime { get { return DateTime.Parse(send_pay_date); } }
        //        public string Voucher { get { return trade_no; } }
        //    }
        //    class RetOrderQuery
        //    {
        //        public RetOrderQueryData alipay_trade_query_response;
        //        public string sign;
        //    }
        //    static string PageExecute<T>(DefaultAopClient client, IAopRequest<T> request) where T : AopResponse
        //    {
        //        if (client == null)
        //            client = GetClient();
        //        request.SetNotifyUrl(client.notify_url);
        //        request.SetReturnUrl(client.return_url);
        //        //var response = client.SdkExecute(request);
        //        //return ImplHelper.HttpGet(URL + "?" + response.Body);
        //        var response = client.pageExecute(request);
        //        return response.Body;
        //    }
        //    static string SDKExecute<T>(DefaultAopClient client, IAopRequest<T> request) where T : AopResponse
        //    {
        //        if (client == null)
        //            client = GetClient();
        //        request.SetNotifyUrl(client.notify_url);
        //        request.SetReturnUrl(client.return_url);
        //        var response = client.SdkExecute(request);
        //        return HttpGet(URL + "?" + response.Body);
        //    }
        //    static string SDKExecute<T>(IAopRequest<T> request) where T : AopResponse
        //    {
        //        return SDKExecute(null, request);
        //    }
        //    /// <summary>支付宝订单</summary>
        //    /// <param name="out_trade_no">外部订单号，商户网站订单系统中唯一的订单号</param>
        //    /// <param name="price">单位分</param>
        //    public static string OrderPage(int id, string out_trade_no, string title, string desc, int price)
        //    {
        //        if (!T_SMSCode.IsValid)
        //            price = 1;

        //        DefaultAopClient client = GetClient();
        //        // 支付成功后的跳转页面
        //        client.return_url = string.Format("{0}{1}id={2}&oid={3}&price={4}",
        //            RETURN_URL, RETURN_URL.Contains('?') ? '&' : '?', id, out_trade_no, price);     

        //        var request = new AlipayTradeWapPayRequest();
        //        //var request = new AlipayTradePagePayRequest();
        //        double __price = price / 100.0;
        //        string __tempPrice = __price.ToString();
        //        int point = __tempPrice.IndexOf('.');
        //        if (point != -1)
        //        {
        //            if (__tempPrice.Length - point - 1 > 2)
        //            {
        //                _LOG.Info("支付宝支付金额超过2位小数 {0}", __tempPrice);
        //                __tempPrice = __tempPrice.Substring(0, point + 1 + 2);
        //                __price = double.Parse(__tempPrice);
        //            }
        //        }
        //        request.BizContent = JsonWriter.Serialize(new ReqOrderPage()
        //        {
        //            out_trade_no = out_trade_no,
        //            total_amount = __price,
        //            subject = title,
        //            body = desc,
        //        });

        //        _FILE.CheckFilePath("order");
        //        string get = string.Format(BODY, out_trade_no, PageExecute(client, request));
        //        //OrderPageCodes[out_trade_no] = get;
        //        // 写入文件，返回URL
        //        _FILE.CheckPath("order");
        //        string result = string.Format("order/{0}.html", out_trade_no);
        //        File.WriteAllText(result, get);
        //        //return string.Format("{0}ZFBURL?orderID={1}", OrderPageURL, out_trade_no);
        //        result.ResolveImage(out result);
        //        return result;
        //        // 写入文件，返回URL
        //        //string url = string.Format("orders/{0}.html", out_trade_no);
        //        //_RES.CheckFilePath(_IO.RootDirectory + url);
        //        //_IO.WriteText(url, get);

        //        //url = _RES.GetAccessURL(url, true);
        //        //return url;
        //    }
        //    public static RetOrderQueryData OrderQuery(string out_trade_no)
        //    {
        //        AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
        //        request.BizContent = string.Format("{{\"out_trade_no\":\"{0}\"}}", out_trade_no);
        //        string get = SDKExecute(request);
        //        var data = JsonReader.Deserialize<RetOrderQuery>(get);
        //        //data.alipay_trade_query_response.Check();
        //        return data.alipay_trade_query_response;
        //    }
        //}
        #endregion
        #region 微信支付
        // 需要引入.Net组件System.Xml.dll
        /// <summary>微信支付
        /// 微信支付API有v2和v3版本，现在最新网上找到的文档为v3版本，这里实现的是v2版本
        /// 微信支付申请流程：
        /// 1. APP到微信开放平台https://open.weixin.qq.com/
        ///    小程序，公众号到微信公众平台https://mp.weixin.qq.com/
        ///    注册账号后，都需要300元进行认证
        /// 2. 认证通过后首次对接支付需要到微信微信商户平台https://pay.weixin.qq.com/
        ///    注册需要AppID，对公账户汇款验证
        /// 3. 以认证通过的公众号为例，需要以下配置
        ///    公众号设置 -> 功能设置 多出网页授权域名，设置前端域名
        ///    安全中心 -> IP白名单 设置服务端IP
        /// 4. 商户后台需要以下配置
        ///    产品中心 -> 开发配置 -> JSAPI支付 添加支付授权前端域名
        ///    账户中心 -> API安全 -> 申请API，APIv2，APIv3证书
        /// </summary>
        public static class _WX
        {
            /// <summary>
            /// 微信支付协议接口数据类，所有的API接口通信都依赖这个数据结构，
            /// 在调用接口之前先填充各个字段的值，然后进行接口通信，
            /// 这样设计的好处是可扩展性强，用户可随意对协议进行更改而不用重新设计数据结构，
            /// 还可以随意组合出不同的协议数据包，不用为每个协议设计一个数据包结构
            /// </summary>
            public class WxPayData
            {
                public const string SIGN_TYPE_MD5 = "MD5";
                public const string SIGN_TYPE_HMAC_SHA256 = "HMAC-SHA256";

                //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
                private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

                public void SetValue(string key, object value)
                {
                    m_values[key] = value;
                }
                public string GetValue(string key)
                {
                    return GetValue<string>(key);
                }
                public T GetValue<T>(string key)
                {
                    object o = null;
                    m_values.TryGetValue(key, out o);
                    if (o == null)
                        return default(T);
                    return (T)o;
                }

                public string ToXml()
                {
                    //数据为空时不能转化为xml格式
                    if (0 == m_values.Count)
                        throw new InvalidOperationException("WxPayData数据为空!");

                    StringBuilder xml = new StringBuilder("<xml>");
                    foreach (KeyValuePair<string, object> pair in m_values)
                    {
                        // 字段值不能为null，会影响后续流程
                        if (pair.Value == null)
                            throw new InvalidOperationException("WxPayData内部含有值为null的字段" + pair.Key);

                        if (pair.Value.GetType() == typeof(int))
                            xml.AppendFormat("<{0}>{1}</{0}>", pair.Key, pair.Value);
                        else if (pair.Value.GetType() == typeof(string))
                            xml.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", pair.Key, pair.Value);
                        else
                            // 除了string和int类型不能含有其他数据类型
                            throw new InvalidOperationException("WxPayData字段数据类型错误!");
                    }
                    xml.Append("</xml>");
                    return xml.ToString();
                }
                public void FromXml(string xml)
                {
                    if (string.IsNullOrEmpty(xml))
                        throw new InvalidOperationException("将空的xml串转换为WxPayData不合法!");

                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(xml);
                    System.Xml.XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                    System.Xml.XmlNodeList nodes = xmlNode.ChildNodes;
                    foreach (System.Xml.XmlNode xn in nodes)
                    {
                        System.Xml.XmlElement xe = (System.Xml.XmlElement)xn;
                        m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
                    }

                    if (m_values["return_code"].ToString() == "SUCCESS" && !string.IsNullOrEmpty(GetValue("sign")))
                        if (!CheckSign())
                            throw new InvalidOperationException("签名验证失败");
                }

                /// <summary>Dictionary格式转化成url参数格式（不包含sign字段值）</summary>
                string ToUrl()
                {
                    StringBuilder buff = new StringBuilder();
                    foreach (KeyValuePair<string, object> pair in m_values)
                    {
                        if (pair.Value == null)
                            throw new InvalidOperationException("WxPayData内部含有值为null的字段" + pair.Key);

                        if (pair.Key != "sign" && pair.Value.ToString() != "")
                            buff.AppendFormat("{0}={1}&", pair.Key, pair.Value);
                    }
                    if (buff.Length > 0)
                        buff = buff.Remove(buff.Length - 1, 1);
                    return buff.ToString();
                }
                /// <summary>生成签名，详见签名生成算法，sign字段不参加签名</summary>
                public string MakeSign(string signType)
                {
                    //转url格式
                    string str = ToUrl();
                    //在string后加入API KEY
                    str += "&key=" + APP_KEY;
                    if (signType == SIGN_TYPE_MD5)
                    {
                        var md5 = System.Security.Cryptography.MD5.Create();
                        var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                        var sb = new StringBuilder();
                        foreach (byte b in bs)
                        {
                            sb.Append(b.ToString("x2"));
                        }
                        //所有字符转为大写
                        return sb.ToString().ToUpper();
                    }
                    else if (signType == SIGN_TYPE_HMAC_SHA256)
                    {
                        var enc = Encoding.Default;
                        byte[] baText2BeHashed = enc.GetBytes(str),
                        baSalt = enc.GetBytes(APP_KEY);
                        var hasher = new System.Security.Cryptography.HMACSHA256(baSalt);
                        byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
                        return string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
                    }
                    else
                        throw new InvalidOperationException("sign_type 不合法");
                }
                /// <summary>SIGN_TYPE_MD5生成签名</summary>
                public string MakeSign()
                {
                    return MakeSign(SIGN_TYPE_MD5);
                }
                /// <summary>检测签名是否正确</summary>
                public bool CheckSign(string signType)
                {
                    // 如果没有设置签名或设置了签名但是签名为空，则跳过检测
                    object sign;
                    if (!m_values.TryGetValue("sign", out sign) || sign == null)
                        return false;
                    return sign.ToString() == MakeSign(signType);
                }
                /// <summary>检测MD5签名是否正确</summary>
                public bool CheckSign()
                {
                    return CheckSign(SIGN_TYPE_MD5);
                }
            }
            /// <summary>查询订单付款状态</summary>
            public class RetQueryOrder : IPayResult
            {
                public enum ETradeState
                {
                    /// <summary>支付成功</summary>
                    SUCCESS,
                    /// <summary>转入退款</summary>
                    REFUND,
                    /// <summary>未支付</summary>
                    NOTPAY,
                    /// <summary>已关闭</summary>
                    CLOSED,
                    /// <summary>已撤销（刷卡支付）</summary>
                    REVOKED,
                    /// <summary>用户支付中</summary>
                    USERPAYING,
                    /// <summary>支付失败(其他原因，如银行返回失败)</summary>
                    PAYERROR,
                }

                public string device_info;
                public string openid;
                public string trade_type;
                public string trade_state;
                public ETradeState TradeState
                {
                    get { return (ETradeState)Enum.Parse(typeof(ETradeState), trade_state); }
                }
                public string bank_type;
                public int total_fee;
                public string fee_type;
                public string transaction_id;
                public string out_trade_no;
                public string time_end;
                public string trade_state_desc;

                public bool IsTradeSuccess { get { return trade_state == "SUCCESS"; } }
                public DateTime PayTime { get { return DateTime.ParseExact(time_end, "yyyyMMddHHmmss", null); } }
                public string Voucher { get { return transaction_id; } }

                public static RetQueryOrder FromWxPayData(WxPayData data)
                {
                    RetQueryOrder ret = new RetQueryOrder();
                    if (data.GetValue("device_info") != null) ret.device_info = data.GetValue("device_info");
                    ret.openid = data.GetValue("openid");
                    ret.trade_type = data.GetValue("trade_type");
                    ret.trade_state = data.GetValue("trade_state");
                    if (string.IsNullOrEmpty(ret.trade_state)) ret.trade_state = data.GetValue("result_code");
                    ret.bank_type = data.GetValue("bank_type");
                    ret.total_fee = Convert.ToInt32(data.GetValue("total_fee"));
                    if (data.GetValue("fee_type") != null) ret.fee_type = data.GetValue("fee_type");
                    ret.transaction_id = data.GetValue("transaction_id");
                    ret.out_trade_no = data.GetValue("out_trade_no");
                    ret.time_end = data.GetValue("time_end");
                    ret.trade_state_desc = data.GetValue("trade_state_desc");
                    return ret;
                }
            }
            public class RetTransfers
            {
                public string mch_appid;
                public string mchid;
                public string device_info;
                public string nonce_str;
                public string result_code;
                public string err_code;
                public string err_code_des;
                public string partner_trade_no;
                public string payment_no;
                public string payment_time;

                public static RetTransfers FromWxPayData(WxPayData data)
                {
                    RetTransfers ret = new RetTransfers();
                    ret.mch_appid = data.GetValue("mch_appid");
                    ret.mchid = data.GetValue("mchid");
                    ret.device_info = data.GetValue("device_info");
                    ret.nonce_str = data.GetValue("nonce_str");
                    ret.result_code = data.GetValue("result_code");
                    ret.err_code = data.GetValue("err_code");
                    ret.err_code_des = data.GetValue("err_code_des");
                    ret.partner_trade_no = data.GetValue("partner_trade_no");
                    ret.payment_no = data.GetValue("payment_no");
                    ret.payment_time = data.GetValue("payment_time");
                    return ret;
                }
            }

            public class WXOauth
            {
                public string access_token;
                public int expires_in;
                public string refresh_token;
                public string openid;
                /// <summary>snsapi_userinfo: 可以获取微信用户信息
                /// snsapi_base: 仅能
                /// </summary>
                public string scope;

                public bool CanGetUserInfo { get { return scope == "snsapi_userinfo"; } }
            }
            /// <summary>关注公众号的微信用户信息</summary>
            public class GZHUserInfo : UserInfo
            {
                public bool subscribe;
                public string language;
                /// <summary>用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间</summary>
                public long subscribe_time;
                public string remark;
                public int groupid;
                public int[] tagid_list;
                public string subscribe_scene;
            }
            /// <summary>微信用户基本信息</summary>
            public class UserInfo
            {
                public string openid;
                public string nickname;
                /// <summary>用户的性别，值为1时是男性，值为2时是女性，值为0时是未知</summary>
                public int sex;
                public string city;
                public string province;
                public string country;
                /// <summary>用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。</summary>
                public string headimgurl;
                /// <summary>只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。</summary>
                public string unionid;
            }

            /// <summary>
            /// 可以是微信开放平台：https://open.weixin.qq.com/
            /// 1. 开发者资质认证需要300元
            /// 2. 管理中心 -> 创建移动应用（创建网页应用虽然能创建也能通过，但是最终商户支付不支持绑定网页应用的AppID）
            /// 注意：创建移动应用需要有应用官网，官网网址可以是IP地址的不一定要域名，内容包含要创建的移动应用的名字和简介就行
            /// 
            /// 可以是微信公众平台：https://mp.weixin.qq.com/
            /// 1. 注册公众号，小程序都可，认证需要300元
            /// </summary>
            public static string APP_ID = "wx783c68aea3a66450";
            /// <summary>以微信开放平台移动应用为例，创建成功后 -> 查看应用 -> AppSecret -> 生成即可</summary>
            public static string APP_KEY = "caa792892fd6111f1e40f6e44a8c5f14";
            /// <summary>微信商户平台：https://pay.weixin.qq.com/
            /// 注册时需要填写已有支付场景，例如移动应用，公众号，小程序等，所以还是先去搞到AppID再注册商户
            /// </summary>
            public static string SHOP_ID = "1629524233";
            /// <summary>
            /// 商户平台管理员设置API安全：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/
            /// 1. API证书：按照平台要求，下载工具，按步骤操作，得到一个压缩包
            /// 2. API2 & API3证书：都是一个32位字符串
            /// </summary>
            public static string APP_SECRET = "caa792892fd6111f1e40f6e44a8c5f14";
            /// <summary>微信支付成功的回调</summary>
            public static string PAY_CALLBACK = string.Format("http//{0}:{1}/Action/1/WeChatPayCallback", _NETWORK.HostIP, _S<Service>.Value.Port);
            /// <summary>微信退款成功的回调</summary>
            public static string REFUND_CALLBACK = string.Format("http//{0}:{1}/Action/1/WeChatRefundCallback", _NETWORK.HostIP, _S<Service>.Value.Port);
            public static string RETURN_URL = "https://api.1996yx.com/pages/oder/paySuccess";


            /// <summary>获取一个用户的openid和access_token，以便获取用户信息，若仅需要openid，本步骤即可(openid可用于jssdk调用微信充值)
            /// 文档：https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html#0
            /// </summary>
            /// <param name="code">参见文档第一步，前端获取</param>
            public static WXOauth GetOpenID(string code)
            {
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", APP_ID, APP_SECRET, code);
                string result = HttpGet(url);
                if (result.StartsWith("{\"errcode\":"))
                {
                    _LOG.Warning("sns/oauth2/access_token错误：{0}", result);
                    "获得微信信息失败".Check(true);
                }
                return JsonReader.Deserialize<WXOauth>(result);
            }
            /// <summary>前端是公众号时，不需要用户授权也可获取用户的基本资料。获取不到时返回null</summary>
            public static GZHUserInfo GetGZHUserInfo(WXOauth auth)
            {
                // 有关注公众号信息，据网上说有每日2000次的限制
                // https://api.weixin.qq.com/cgi-bin/user/info?access_token=$access_token&openid=$openid&lang=zh_CN
                string result = HttpGet(
                    string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}", AccessTokenService.AccessToken, auth.openid));

                if (!result.StartsWith("{\"errcode\":"))
                {
                    var userinfo = JsonReader.Deserialize<GZHUserInfo>(result);
                    // 没有关注公众号，拉不出玩家信息
                    if (userinfo.subscribe)
                        return userinfo;
                }

                return null;
            }
            /// <summary>获取微信用户的基本资料</summary>
            public static UserInfo GetUserInfo(WXOauth auth)
            {
                "scope必须是snsapi_userinfo".Check(!auth.CanGetUserInfo);
                string result = HttpGet(
                    string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", auth.access_token, auth.openid));
                string.Format("获取微信用户信息错误：{0}", result).Check(result.StartsWith("{\"errcode\":"));
                return JsonReader.Deserialize<GZHUserInfo>(result);
            }


            private static string GenerateNonceStr()
            {
                return Guid.NewGuid().ToString("n");
            }


            private static WxPayData InternalOrder(string out_trade_no, string body, int total_fee, string ip, string trade_type, string openid)
            {
                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                //检测必填参数
                if (string.IsNullOrEmpty(out_trade_no))
                    throw new InvalidOperationException("缺少统一支付接口必填参数out_trade_no！");
                else if (string.IsNullOrEmpty(body))
                    throw new InvalidOperationException("缺少统一支付接口必填参数body！");
                else if (total_fee <= 0)
                    throw new InvalidOperationException("缺少统一支付接口必填参数total_fee！");

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("out_trade_no", out_trade_no);
                inputObj.SetValue("body", body);
                inputObj.SetValue("total_fee", total_fee);
                inputObj.SetValue("trade_type", trade_type);
                inputObj.SetValue("spbill_create_ip", ip);//终端ip
                inputObj.SetValue("notify_url", PAY_CALLBACK);//异步通知url

                inputObj.SetValue("appid", APP_ID);//公众账号ID
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                if (!string.IsNullOrEmpty(openid))
                    inputObj.SetValue("openid", openid);

                //签名
                inputObj.SetValue("sign", inputObj.MakeSign(WxPayData.SIGN_TYPE_MD5));
                string xml = inputObj.ToXml();

                var start = DateTime.Now;

                string response = Post(xml, url, false);

                var end = DateTime.Now;
                int timeCost = (int)((end - start).TotalMilliseconds);

                WxPayData result = new WxPayData();
                result.FromXml(response);
                if (result.GetValue("return_code") != "SUCCESS")
                {
                    _LOG.Warning("UnifiedOrder错误：{0}", result.GetValue("return_msg"));
                    throw new Exception(result.GetValue("return_msg"));
                }

                return result;
            }
            /// <summary>微信订单</summary>
            /// <param name="out_trade_no">外部订单号，商户网站订单系统中唯一的订单号</param>
            /// <param name="price">单位分</param>
            public static string OrderPage(string out_trade_no, string body, int total_fee, string ip)
            {
                var result = InternalOrder(out_trade_no, body, total_fee, ip, "MWEB", null);
                return result.GetValue("mweb_url") + "&redirect_url=" + _NETWORK.UrlEncode(RETURN_URL + "?oid=" + out_trade_no);
            }
            /// <summary>微信APP方式登录</summary>
            public static WXPay UnifiedOrder(string out_trade_no, string body, int total_fee, string ip)
            {
                var result = InternalOrder(out_trade_no, body, total_fee, ip, "APP", null);

                WxPayData resign = new WxPayData();
                resign.SetValue("appid", result.GetValue("appid"));
                resign.SetValue("partnerid", SHOP_ID);
                resign.SetValue("prepayid", result.GetValue("prepay_id"));
                resign.SetValue("package", "Sign=WXPay");
                resign.SetValue("noncestr", result.GetValue("nonce_str"));
                resign.SetValue("timestamp", Utility.UnixTimestamp.ToString());
                resign.SetValue("sign", resign.MakeSign());

                WXPay ret = new WXPay();
                ret.appId = result.GetValue("appid");
                ret.nonceStr = result.GetValue("nonce_str");
                ret.prepayid = result.GetValue("prepay_id");
                ret.timeStamp = resign.GetValue("timestamp");
                ret.paySign = resign.GetValue("sign");
                ret.partnerid = SHOP_ID;
                ret.package = "Sign=WXPay";
                return ret;
            }
            /// <summary>微信JSAPI方式登录</summary>
            public static WXPay UnifiedOrder(string out_trade_no, string body, int total_fee, string ip, string openid)
            {
                var result = InternalOrder(out_trade_no, body, total_fee, ip, "JSAPI", openid);

                WxPayData resign = new WxPayData();
                resign.SetValue("appId", result.GetValue("appid"));
                resign.SetValue("nonceStr", result.GetValue("nonce_str"));
                resign.SetValue("timeStamp", Utility.UnixTimestamp.ToString());
                resign.SetValue("package", "prepay_id=" + result.GetValue("prepay_id"));
                resign.SetValue("signType", "MD5");
                resign.SetValue("sign", resign.MakeSign());

                WXPay ret = new WXPay();
                ret.appId = result.GetValue("appid");
                ret.nonceStr = result.GetValue("nonce_str");
                ret.prepayid = result.GetValue("prepay_id");
                ret.timeStamp = resign.GetValue("timeStamp");
                ret.paySign = resign.GetValue("sign");
                ret.signType = resign.GetValue("signType");
                ret.partnerid = SHOP_ID;
                ret.package = "prepay_id=" + ret.prepayid;
                ret.out_trade_no = out_trade_no;
                return ret;
            }
            public static RetQueryOrder OrderQuery(string out_trade_no)
            {
                string url = "https://api.mch.weixin.qq.com/pay/orderquery";
                //检测必填参数
                //if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
                if (string.IsNullOrEmpty(out_trade_no))
                {
                    throw new InvalidOperationException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
                }

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("out_trade_no", out_trade_no);

                inputObj.SetValue("appid", APP_ID);//公众账号ID
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                inputObj.SetValue("sign", inputObj.MakeSign());//签名

                string xml = inputObj.ToXml();

                var start = DateTime.Now;

                string response = Post(xml, url, false);//调用HTTP通信接口提交数据

                var end = DateTime.Now;
                int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

                //将xml格式的数据转化为对象以返回
                WxPayData result = new WxPayData();
                result.FromXml(response);

                if (result.GetValue("return_code") != "SUCCESS")
                {
                    _LOG.Error("OrderQuery错误：{0}", result.GetValue("return_msg"));
                    return null;
                }

                return RetQueryOrder.FromWxPayData(result);
            }
            public static WxPayData CloseOrder(string out_trade_no)
            {
                string url = "https://api.mch.weixin.qq.com/pay/closeorder";
                //检测必填参数
                if (string.IsNullOrEmpty(out_trade_no))
                {
                    throw new InvalidOperationException("关闭订单接口中，out_trade_no必填！");
                }

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("out_trade_no", out_trade_no);

                inputObj.SetValue("appid", APP_ID);//公众账号ID
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                inputObj.SetValue("sign", inputObj.MakeSign());//签名
                string xml = inputObj.ToXml();

                var start = DateTime.Now;//请求开始时间

                string response = Post(xml, url, false);

                var end = DateTime.Now;
                int timeCost = (int)((end - start).TotalMilliseconds);

                WxPayData result = new WxPayData();
                result.FromXml(response);

                return result;
            }
            public static WxPayData DownloadBill(DateTime date)
            {
                string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
                //检测必填参数

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("bill_date", date.ToString("yyyyMMdd"));

                inputObj.SetValue("appid", APP_ID);//公众账号ID
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                inputObj.SetValue("sign", inputObj.MakeSign());//签名

                string xml = inputObj.ToXml();

                _LOG.Debug("WxPayApi", "DownloadBill request : " + xml);
                string response = Post(xml, url, false);//调用HTTP通信接口以提交数据到API
                _LOG.Debug("WxPayApi", "DownloadBill result : " + response);

                WxPayData result = new WxPayData();
                //若接口调用失败会返回xml格式的结果
                if (response.Substring(0, 5) == "<xml>")
                {
                    result.FromXml(response);
                }
                //接口调用成功则返回非xml格式的数据
                else
                    result.SetValue("result", response);

                return result;
            }
            /// <summary>退款</summary>
            /// <param name="out_trade_no">支付订单号：标识要退哪一单</param>
            /// <param name="out_refund_no">退款订单号：标识一个唯一退款订单，同一订单多个商品可以分为多次退款</param>
            /// <param name="refund_desc">退款理由</param>
            public static WxPayData Refund(string out_trade_no, string out_refund_no, int total_fee, int refund_fee, string refund_desc)
            {
                string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
                //检测必填参数
                if (string.IsNullOrEmpty("out_trade_no"))
                    throw new InvalidOperationException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
                else if (string.IsNullOrEmpty("out_refund_no"))
                    throw new InvalidOperationException("退款申请接口中，缺少必填参数out_refund_no！");
                else if (total_fee <= 0)
                    throw new InvalidOperationException("退款申请接口中，缺少必填参数total_fee！");
                else if (refund_fee <= 0 || refund_fee > total_fee)
                    throw new InvalidOperationException("退款申请接口中，缺少必填参数refund_fee！");

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("out_trade_no", out_trade_no);
                inputObj.SetValue("out_refund_no", out_refund_no);
                inputObj.SetValue("notify_url", REFUND_CALLBACK);//异步通知url
                if (!string.IsNullOrEmpty(refund_desc))
                    inputObj.SetValue("refund_desc", refund_desc);
                inputObj.SetValue("total_fee", total_fee);
                inputObj.SetValue("refund_fee", refund_fee);

                inputObj.SetValue("appid", APP_ID);//公众账号ID
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
                inputObj.SetValue("sign", inputObj.MakeSign());//签名

                string xml = inputObj.ToXml();
                var start = DateTime.Now;

                _LOG.Debug("WxPayApi", "Refund request : " + xml);
                string response = Post(xml, url, true);//调用HTTP通信接口提交数据到API
                _LOG.Debug("WxPayApi", "Refund response : " + response);

                var end = DateTime.Now;
                int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

                //将xml格式的结果转换为对象以返回
                WxPayData result = new WxPayData();
                result.FromXml(response);

                result.GetValue("err_code_des").Check(result.GetValue("result_code") != "SUCCESS");

                return result;
            }
            public static WxPayData RefundQuery(string out_trade_no)
            {
                string url = "https://api.mch.weixin.qq.com/pay/refundquery";
                //检测必填参数
                //if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                //    !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
                if (string.IsNullOrEmpty(out_trade_no))
                    throw new InvalidOperationException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("out_trade_no", out_trade_no);

                inputObj.SetValue("appid", APP_ID);//公众账号ID
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                inputObj.SetValue("sign", inputObj.MakeSign());//签名

                string xml = inputObj.ToXml();

                var start = DateTime.Now;//请求开始时间

                _LOG.Debug("WxPayApi", "RefundQuery request : " + xml);
                string response = Post(xml, url, false);//调用HTTP通信接口以提交数据到API
                _LOG.Debug("WxPayApi", "RefundQuery response : " + response);

                var end = DateTime.Now;
                int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

                //将xml格式的结果转换为对象以返回
                WxPayData result = new WxPayData();
                result.FromXml(response);

                return result;
            }
            /// <summary>企业付款到微信账号</summary>
            /// <param name="partner_trade_no">订单号</param>
            /// <param name="openid">到账人的微信OpenID</param>
            /// <param name="amount">付款金额，单位分</param>
            /// <param name="desc">付款描述</param>
            public static RetTransfers Transfers(string partner_trade_no, string openid, int amount, string desc)
            {
                //检测必填参数
                if (string.IsNullOrEmpty(partner_trade_no))
                    throw new InvalidOperationException("缺少统一支付接口必填参数partner_trade_no！");
                if (string.IsNullOrEmpty(openid))
                    throw new InvalidOperationException("缺少统一支付接口必填参数openid！");
                if (string.IsNullOrEmpty(desc))
                    throw new InvalidOperationException("缺少统一支付接口必填参数desc！");
                if (amount < 30)
                    throw new InvalidOperationException("付款金额最低0.3元");

                string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("mch_appid", APP_ID);
                // MARK: 文档上是mch_id，这里其实是mchid
                inputObj.SetValue("mchid", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                inputObj.SetValue("partner_trade_no", partner_trade_no);
                inputObj.SetValue("openid", openid);
                inputObj.SetValue("check_name", "NO_CHECK");
                inputObj.SetValue("amount", amount);
                inputObj.SetValue("desc", desc);

                //签名
                inputObj.SetValue("sign", inputObj.MakeSign(WxPayData.SIGN_TYPE_MD5));
                string xml = inputObj.ToXml();
                string response = Post(xml, url, true);

                WxPayData result = new WxPayData();
                result.FromXml(response);

                if (result.GetValue("result_code") != "SUCCESS")
                    throw new Exception(result.GetValue("err_code_des"));

                return RetTransfers.FromWxPayData(result);
            }
            public static WxPayData GetTransferInfo(string partner_trade_no)
            {
                //检测必填参数
                if (string.IsNullOrEmpty(partner_trade_no))
                    throw new InvalidOperationException("缺少统一支付接口必填参数partner_trade_no！");

                string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gettransferinfo";

                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("appid", APP_ID);
                // MARK: 文档上是mch_id，这里其实是mchid
                inputObj.SetValue("mch_id", SHOP_ID);//商户号
                inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
                inputObj.SetValue("partner_trade_no", partner_trade_no);

                //签名
                inputObj.SetValue("sign", inputObj.MakeSign(WxPayData.SIGN_TYPE_MD5));
                string xml = inputObj.ToXml();
                string response = Post(xml, url, true);

                WxPayData result = new WxPayData();
                result.FromXml(response);

                if (result.GetValue("result_code") != "SUCCESS")
                    throw new Exception(result.GetValue("err_code_des"));

                return result;
            }

            private static string USER_AGENT = string.Format("WXPaySDK/{3} ({0}) .net/{1} {2}", Environment.OSVersion, Environment.Version, SHOP_ID, typeof(_DB).Assembly.GetName().Version);
            public static bool CheckValidationResult(object sender, 
                System.Security.Cryptography.X509Certificates.X509Certificate certificate, 
                System.Security.Cryptography.X509Certificates.X509Chain chain, 
                System.Net.Security.SslPolicyErrors errors)
            {
                //直接确认，否则打不开    
                return true;
            }
            public static string Post(string xml, string url, bool isUseCert, int timeoutSecond = 6)
            {
                System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

                string result = "";//返回结果

                HttpWebRequest request = null;
                HttpWebResponse response = null;
                Stream reqStream = null;

                try
                {
                    //设置最大连接数
                    ServicePointManager.DefaultConnectionLimit = 200;
                    //设置https验证方式
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback =
                                new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                    }

                    /***************************************************************
                    * 下面设置HttpWebRequest的相关属性
                    * ************************************************************/
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.UserAgent = USER_AGENT;
                    request.Method = "POST";
                    request.Timeout = timeoutSecond * 1000;

                    //设置代理服务器
                    //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                    //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                    //request.Proxy = proxy;

                    //设置POST的数据类型和长度
                    request.ContentType = "text/xml";
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                    request.ContentLength = data.Length;

                    //是否使用证书
                    if (isUseCert)
                    {
                        var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2("apiclient_cert.p12", SHOP_ID);
                        request.ClientCertificates.Add(cert);
                    }

                    //往服务器写入数据
                    reqStream = request.GetRequestStream();
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();

                    //获取服务端返回
                    response = (HttpWebResponse)request.GetResponse();

                    //获取服务端返回数据
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    result = sr.ReadToEnd().Trim();
                    sr.Close();
                }
                catch (System.Threading.ThreadAbortException e)
                {
                    _LOG.Error(e, "HttpService");
                    System.Threading.Thread.ResetAbort();
                }
                catch (WebException e)
                {
                    _LOG.Error(e, "HttpService");
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        _LOG.Error("ProtocolError StatusCode : {0} StatusDescription : {1}", ((HttpWebResponse)e.Response).StatusCode, ((HttpWebResponse)e.Response).StatusDescription);
                    }
                    throw new InvalidOperationException(e.ToString());
                }
                catch (Exception e)
                {
                    _LOG.Error(e, "HttpService");
                    throw new InvalidOperationException(e.ToString());
                }
                finally
                {
                    //关闭连接和流
                    if (response != null)
                    {
                        response.Close();
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }
                }
                return result;
            }
            public static string Get(string url)
            {
                System.GC.Collect();
                string result = "";

                HttpWebRequest request = null;
                HttpWebResponse response = null;

                //请求url以获取数据
                try
                {
                    //设置最大连接数
                    ServicePointManager.DefaultConnectionLimit = 200;
                    //设置https验证方式
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback =
                                new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                    }

                    /***************************************************************
                    * 下面设置HttpWebRequest的相关属性
                    * ************************************************************/
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.UserAgent = USER_AGENT;
                    request.Method = "GET";

                    //获取服务器返回
                    response = (HttpWebResponse)request.GetResponse();

                    //获取HTTP返回数据
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    result = sr.ReadToEnd().Trim();
                    sr.Close();
                }
                catch (System.Threading.ThreadAbortException e)
                {
                    _LOG.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                    _LOG.Error("Exception message: {0}", e.Message);
                    System.Threading.Thread.ResetAbort();
                }
                catch (WebException e)
                {
                    _LOG.Error("HttpService", e.ToString());
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        _LOG.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                        _LOG.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                    }
                    throw new InvalidOperationException(e.ToString());
                }
                catch (Exception e)
                {
                    _LOG.Error("HttpService", e.ToString());
                    throw new InvalidOperationException(e.ToString());
                }
                finally
                {
                    //关闭连接和流
                    if (response != null)
                    {
                        response.Close();
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }
                }
                return result;
            }

            /// <summary>微信AccessToken服务</summary>
            public static class AccessTokenService
            {
                const string FILE = "wxtoken.bin";
                const string API_ACCESSTOKEN = @"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
                const string API_TICKET = @"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

                private static string __Token;
                private static DateTime __TokenExpire;
                private static string __Ticket;
                private static DateTime __TicketExpire;

                public static string AccessToken
                {
                    get
                    {
                        if (string.IsNullOrEmpty(__Token) || DateTime.Now >= __TokenExpire)
                        {
                            var ret = HttpRequest(string.Format(API_ACCESSTOKEN, _WX.APP_ID, _WX.APP_SECRET), null);
                            var obj = JsonReader.Deserialize(ret);
                            if (obj.ContainsKey("errcode"))
                            {
                                _LOG.Warning("刷新微信AccessToken异常 errcode: {0} errmsg: {1}", obj["errcode"], obj["errmsg"]);
                                throw new InvalidOperationException("刷新微信AccessToken异常");
                            }
                            __Token = obj["access_token"].GetString();
                            __TokenExpire = DateTime.Now.AddSeconds(obj["expires_in"].GetInt32() - 300);
                            Save();
                            _LOG.Debug("刷新微信AccessToken: {0} 过期时间: {1}", __Token, __TokenExpire);
                        }
                        return __Token;
                    }
                }
                public static string Ticket
                {
                    get
                    {
                        if (string.IsNullOrEmpty(__Ticket) || DateTime.Now >= __TicketExpire)
                        {
                            var ret = HttpRequest(string.Format(API_TICKET, AccessToken), null);
                            var obj = JsonReader.Deserialize(ret);
                            int err = obj["errcode"].GetInt32();
                            if (err != 0)
                            {
                                if (err == 40001)
                                {
                                    // 需要重新刷新AccessToken
                                    __Token = null;
                                    return Ticket;
                                }
                                _LOG.Warning("刷新微信Ticket异常 errcode: {0} errmsg: {1}", err, obj["errmsg"]);
                                throw new InvalidOperationException("刷新微信Ticket异常");
                            }
                            __Ticket = obj["ticket"].GetString();
                            __TicketExpire = DateTime.Now.AddSeconds(obj["expires_in"].GetInt32() - 300);
                            Save();
                            _LOG.Debug("刷新微信Ticket: {0} 过期时间: {1}", __Ticket, __TicketExpire);
                        }
                        return __Ticket;
                    }
                }

                static AccessTokenService()
                {
                    Load();
                }
                public static void Load()
                {
                    if (!File.Exists(FILE)) return;
                    ByteReader reader = new ByteReader(File.ReadAllBytes(FILE));
                    reader.Read(out __Token);
                    reader.Read(out __TokenExpire);
                    reader.Read(out __Ticket);
                    reader.Read(out __TicketExpire);
                }
                public static void Save()
                {
                    ByteWriter writer = new ByteWriter();
                    writer.Write(__Token);
                    writer.Write(__TokenExpire);
                    writer.Write(__Ticket);
                    writer.Write(__TicketExpire);
                    File.WriteAllBytes(FILE, writer.GetBuffer());
                }
            }
        }
        #endregion
    }
}
