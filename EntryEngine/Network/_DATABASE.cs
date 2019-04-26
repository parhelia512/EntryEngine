﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EntryEngine.Serialize;

namespace EntryEngine.Network
{
#if SERVER
    public static partial class _DATABASE
    {
        public abstract class Database : IDisposable
        {
            private string connectionString;
            public Action<IDbConnection, Database> OnTestConnection;
            public Action<IDbConnection, Database> OnCreateConnection;
            public ushort Timeout = 60;

            public bool Available { get; private set; }
            public virtual string ConnectionString
            {
                get { return connectionString; }
                set
                {
                    if (Available)
                        throw new InvalidOperationException("Can't change tested connection string.");
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentNullException("connectionString");
                    connectionString = value;
                }
            }
            public virtual string DatabaseName { get; internal set; }
            public virtual string DataSource { get; internal set; }
            public virtual string ServerVersion { get; internal set; }

            protected Database()
            {
            }
            public Database(string connectionString)
            {
                this.ConnectionString = connectionString;
            }

            public void TestConnection()
            {
                if (Available)
                    throw new InvalidOperationException("Connection has been tested.");

                if (string.IsNullOrEmpty(ConnectionString))
                    throw new ArgumentNullException("ConnectionString");

                IDbConnection connection = null;
                try
                {
                    connection = CreateAvailableConnection();
                    var conn = connection as System.Data.Common.DbConnection;
                    if (conn != null)
                    {
                        DatabaseName = conn.Database;
                        DataSource = conn.DataSource;
                        ServerVersion = conn.ServerVersion;
                    }
                    // 允许在TestConnection时使用DB操作函数
                    Available = true;
                    if (OnTestConnection != null)
                        OnTestConnection(connection, this);
                }
                catch (Exception ex)
                {
                    Available = false;
                    _LOG.Error(ex, "TestConnection Error!");
                    throw;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        Executed(connection);
                    }
                }
                
                _LOG.Info("Connection test succuss.");
            }
            private IDbConnection CreateAvailableConnection()
            {
                IDbConnection connection = CreateConnection();
                if (OnCreateConnection != null)
                    OnCreateConnection(connection, this);
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new ArgumentException("Connection must not be null and opened.");
                return connection;
            }
            /// <summary>已打开可使用的数据库连接</summary>
            protected internal abstract IDbConnection CreateConnection();
            private IDbCommand CreateCommand(string sql, params object[] parameters)
            {
                if (!Available)
                    throw new InvalidOperationException("Connection must be tested.");
                IDbConnection conn = CreateAvailableConnection();
                IDbCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                for (int i = 0; i < parameters.Length; ++i)
                {
                    IDbDataParameter param = cmd.CreateParameter();
                    param.ParameterName = string.Format("p{0}", i);
                    param.Value = parameters[i];
                    cmd.Parameters.Add(param);
                }
                return cmd;
            }
            private void ExecuteError(Exception ex, string sql, object[] parameters)
            {
                _LOG.Error(ex, string.Format("sql: {0} {{0}}", sql), "param: " + JsonWriter.Serialize(parameters));
            }
            public int ExecuteNonQuery(string sql, params object[] parameters)
            {
                var command = CreateCommand(sql, parameters);
                try
                {
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ExecuteError(ex, sql, parameters);
                    throw;
                }
                finally
                {
                    if (command.Connection != null)
                    {
                        Executed(command.Connection);
                    }
                }
            }
            public object ExecuteScalar(string sql, params object[] parameters)
            {
                var command = CreateCommand(sql, parameters);
                try
                {
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    ExecuteError(ex, sql, parameters);
                    throw;
                }
                finally
                {
                    if (command.Connection != null)
                    {
                        Executed(command.Connection);
                    }
                }
            }
            public T ExecuteScalar<T>(string sql, params object[] parameters)
            {
                object value = ExecuteScalar(sql, parameters);
                if (value == null || value == DBNull.Value)
                    return default(T);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            public T SelectValue<T>(string sql, params object[] parameters)
            {
                object value = ExecuteScalar(sql, parameters);
                if (value == null || value == DBNull.Value)
                    return default(T);
                else
                    //return (T)value;
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            [Obsolete("instead of public void ExecuteReader")]
            protected IDataReader ExecuteReader(string sql, params object[] parameters)
            {
                var command = CreateCommand(sql, parameters);
                try
                {
                    return command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    ExecuteError(ex, sql, parameters);
                    throw;
                }
                finally
                {
                    if (command.Connection != null)
                    {
                        Executed(command.Connection);
                    }
                }
            }
            public void ExecuteReader(Action<IDataReader> read, string sql, params object[] parameters)
            {
                if (read == null)
                    throw new ArgumentNullException("read");

                IDataReader reader = null;
                var command = CreateCommand(sql, parameters);
                try
                {
                    reader = command.ExecuteReader();
                    read(reader);
                }
                catch (Exception ex)
                {
                    ExecuteError(ex, sql, parameters);
                    throw;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (command.Connection != null)
                    {
                        Executed(command.Connection);
                    }
                }
            }
            public List<T> SelectValues<T>(string sql, params object[] parameters)
            {
                List<T> list = new List<T>();
                ExecuteReader(
                    (reader) =>
                    {
                        Type type = typeof(T);
                        while (reader.Read())
                        {
                            object value = reader[0];
                            if (value == null || value is DBNull)
                                list.Add(default(T));
                            else
                                list.Add((T)Convert.ChangeType(reader[0], type));
                        }
                    }, sql, parameters);
                return list;
            }
            public T SelectObject<T>(string sql, params object[] parameters)
            {
                T instance = default(T);
                ExecuteReader(
                    (reader) =>
                    {
                        Type type = typeof(T);
                        int count = reader.FieldCount;

                        if (reader.Read())
                        {
                            instance = Activator.CreateInstance<T>();
                            Read(reader, type, 0, count, instance);
                        }
                    }, sql, parameters);
                return instance;
            }
            public List<T> SelectObjects<T>(string sql, params object[] parameters)
            {
                List<T> list = new List<T>();
                ExecuteReader(
                    (reader) =>
                    {
                        Type type = typeof(T);
                        int count = reader.FieldCount;

                        while (reader.Read())
                        {
                            T instance = Activator.CreateInstance<T>();
                            Read(reader, type, 0, count, instance);
                            list.Add(instance);
                        }
                    }, sql, parameters);
                return list;
            }
            protected virtual void Executed(IDbConnection connection)
            {
                connection.Close();
            }
            public Async ExecuteAsync(Action<Database> action)
            {
                var async = new AsyncThread();
                async.Queue(() =>
                    {
                        action(this);
                        return 1;
                    });
                return async;
            }
            public virtual void Dispose()
            {
            }
        }

        public static IEnumerable<T> ReadMultiple<T>(IDataReader reader) where T : new()
        {
            while (reader.Read())
            {
                T instance = new T();
                Read(reader, typeof(T), 0, reader.FieldCount, instance);
                yield return instance;
            }
        }
        public static T Read<T>(IDataReader reader) where T : new()
        {
            if (reader.Read())
            {
                T instance = new T();
                Read(reader, typeof(T), 0, reader.FieldCount, instance);
                return instance;
            }
            return default(T);
        }
        public static T ReadObject<T>(IDataReader reader, int offset) where T : new()
        {
            Type type = typeof(T);
            T instance = new T();
            Read(reader, type, offset, type.GetFields().Length, instance);
            return instance;
        }
        public static T ReadObject<T>(IDataReader reader, int offset, int fieldCount) where T : new()
        {
            T instance = new T();
            Read(reader, typeof(T), offset, fieldCount, instance);
            return instance;
        }
        public static void ReadObject(IDataReader reader, object instance)
        {
            Read(reader, instance.GetType(), 0, reader.FieldCount, instance);
        }
        public static void ReadObject(IDataReader reader, object instance, int offset)
        {
            Type type = instance.GetType();
            Read(reader, type, offset, type.GetFields().Length, instance);
        }
        public static void ReadObject(IDataReader reader, object instance, int offset, int fieldCount)
        {
            Read(reader, instance.GetType(), offset, fieldCount, instance);
        }
        private static void Read(IDataReader reader, Type type, int offset, int count, object instance)
        {
            SerializeSetting setting = SerializeSetting.DefaultSerializeAll;
            var properties = setting.GetProperties(type);
            var fields = setting.GetFields(type);
            for (int i = offset, e = offset + count; i < e; i++)
            {
                object value = reader[i];

                string name = reader.GetName(i);

                var property = properties.FirstOrDefault(p => p.Name == name);
                if (property != null)
                {
                    if (!(value is DBNull))
                        try
                        {
                            property.SetValue(instance, value, null);
                        }
                        catch
                        {
                            property.SetValue(instance, Convert.ChangeType(value, property.PropertyType), null);
                        }
                    continue;
                }

                var field = fields.FirstOrDefault(f => f.Name == name);
                if (field == null)
                    continue;
                if (!(value is DBNull))
                    try
                    {
                        field.SetValue(instance, value);
                    }
                    catch
                    {
                        field.SetValue(instance, Convert.ChangeType(value, field.FieldType));
                    }
            }
        }
        //private static object ChangeType(object value, Type type)
        //{
        //    if (type.IsEnum)
        //    {
        //        var underlying = type.UnderlyingSystemType;

        //    }
        //}
    }
    public abstract class Database_Link : EntryEngine.Network._DATABASE.Database
    {
        public virtual EntryEngine.Network._DATABASE.Database Base { get; set; }
        public override string ConnectionString
        {
            get
            {
                return Base.ConnectionString;
            }
            set
            {
                Base.ConnectionString = value;
            }
        }
        public override string DatabaseName
        {
            get { return Base.DatabaseName; }
            internal set { Base.DatabaseName = value; }
        }
        public override string DataSource
        {
            get { return Base.DataSource; }
            internal set { Base.DataSource = value; }
        }
        public override string ServerVersion
        {
            get { return Base.ServerVersion; }
            internal set { Base.ServerVersion = value; }
        }

        public Database_Link() { }
        public Database_Link(EntryEngine.Network._DATABASE.Database Base) { this.Base = Base; }

        protected internal override System.Data.IDbConnection CreateConnection()
        {
            return Base.CreateConnection();
        }
        public override void Dispose()
        {
            Base.Dispose();
        }
    }
    public sealed class ConnectionPool : Database_Link
    {
        class CONNECTION : PoolItem
        {
            public IDbConnection Connection;
            public DateTime LastUseTime;
            public bool Idle;
        }

        private COROUTINE clearPool;
        private Pool<CONNECTION> pools = new Pool<CONNECTION>();
        public TimeSpan ClearTime = TimeSpan.FromSeconds(15);

        public int WorkingCount
        {
            get { lock (pools) { return pools.Count(c => !c.Idle && c.Connection != null && c.Connection.State != ConnectionState.Closed); } }
        }
        public int IdleCount
        {
            get { lock (pools) { return pools.Count(c => c.Idle && (c.Connection == null || c.Connection.State == ConnectionState.Closed)); } }
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               

        public ConnectionPool()
        {
            this.OnTestConnection += TestConnecttion_UpdatePool;
        }
        public ConnectionPool(_DATABASE.Database _base) : this()
        {
            this.Base = _base;
        }

        private void TestConnecttion_UpdatePool(IDbConnection connection, _DATABASE.Database database)
        {
            if (EntryService.Instance != null)
                clearPool = EntryService.Instance.SetTimer(1000, ClearPool);
        }
        public void ClearPool()
        {
            var now = DateTime.Now;
            CONNECTION[] array;
            lock (pools)
                array = pools.ToArray();
            foreach (var item in array)
            {
                if (item.Connection == null ||
                    item.Connection.State == ConnectionState.Closed ||
                    (ClearTime.Ticks > 0 && (now - item.LastUseTime) >= ClearTime))
                {
                    ReleaseConnection(item);
                }
            }
        }
        private void ReleaseConnection(CONNECTION connection)
        {
            if (connection.Connection != null)
            {
                connection.Connection.Close();
                connection.Connection.Dispose();
                connection.Connection = null;
            }
            connection.Idle = true;
            lock (pools)
                pools.Remove(connection);
        }

        protected internal override IDbConnection CreateConnection()
        {
            lock (pools)
            {
                var conn = pools.FirstOrDefault(p => p.Idle);
                if (conn == null)
                {
                    conn = pools.Allot();
                    if (conn == null)
                    {
                        conn = new CONNECTION();
                        pools.Add(conn);
                    }
                    conn.Connection = base.CreateConnection();
                }
                conn.Idle = false;
                conn.LastUseTime = DateTime.Now;
                return conn.Connection;
            }
        }
        protected override void Executed(IDbConnection connection)
        {
            lock (pools)
            {
                CONNECTION conn = pools.FirstOrDefault(p => p.Connection == connection);
                if (conn != null)
                {
                    if (conn.Connection == null || conn.Connection.State == ConnectionState.Closed)
                        ReleaseConnection(conn);
                    conn.Idle = true;
                }
                else
                    base.Executed(connection);
            }
        }
        public override void Dispose()
        {
            lock (pools)
            {
                foreach (var connection in pools)
                {
                    connection.Connection.Close();
                    connection.Connection.Dispose();
                }
                pools.Clear();
            }
            if (clearPool != null)
                clearPool.Dispose();
        }
    }

    [Code(ECode.MayBeReform)]
    public abstract class MemoryDatabase : IDisposable
    {
        private string name = string.Empty;
        private string dir = string.Empty;
        public string Dir
        {
            get { return dir; }
            set { dir = _IO.DirectoryWithEnding(value); }
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("name");
                name = value;
            }
        }

        public MemoryDatabase(string name)
        {
            this.Name = name;
            this.Dir = name;
        }

        public void BackUp(string dir)
        {
            if (string.IsNullOrEmpty(dir))
                throw new ArgumentNullException("BackUp directory name can't be null.");
            _LOG.Info("BackUp {0} to {1}...", name, dir);
            string temp = this.dir;
            this.Dir = Name + dir;

            Save();

            this.Dir = temp;
            _LOG.Info("BackUp {0} to {1} completed.", name, dir);
        }
        public abstract void Save();
        public abstract void Load();
        private string BuildTableFile(string tableName)
        {
            return string.Format("{0}{1}.csv", dir, tableName);
        }
        protected void InternalSave<T>(T[] table, string tablename)
        {
            tablename = BuildTableFile(tablename);
            var dir = Path.GetDirectoryName(tablename);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            Type type = typeof(T);
            _LOG.Info("Save {0} [{1}]...", type.Name, table.Length);

            TIMER timer = TIMER.StartNew();

            CSVWriter writer = new CSVWriter();
            writer.Setting.Static = false;
            //writer.Setting.AutoType = false;
            writer.Setting.Property = false;
            writer.WriteObject(table);
            File.WriteAllText(tablename, writer.Result);

            _LOG.Info("Save {0} [{1}] completed! Time elapsed: {2}", type.Name, table.Length, timer.Stop());
        }
        protected List<T> InternalLoad<T>(string tablename)
        {
            tablename = BuildTableFile(tablename);
            if (!File.Exists(tablename))
            {
                _LOG.Info("Initialize memory table {0}", tablename);
                return new List<T>();
            }

            Type type = typeof(T);
            _LOG.Info("Load {0}...", type.Name);

            TIMER timer = TIMER.StartNew();

            CSVReader reader = new CSVReader(File.ReadAllText(tablename));
            reader.Setting.Static = false;
            //reader.Setting.AutoType = false;
            reader.Setting.Property = false;
            List<T> load = new List<T>(reader.ReadObject<T[]>());

            _LOG.Info("Load {0} [{1}] completed! Time elapsed: {2}", type.Name, load.Count, timer.Stop());
            return load;
        }
        public abstract void Dispose();
    }

    public class MYSQL_TABLE_COLUMN
    {
        public string COLUMN_NAME;
        public string COLUMN_KEY;
        public string EXTRA;
        public bool IsPrimary { get { return COLUMN_KEY == "PRI"; } }
        public bool IsIndex { get { return COLUMN_KEY == "MUL"; } }
        public bool IsUnique { get { return COLUMN_KEY == "UNI"; } }
        public bool IsIdentity { get { return EXTRA == "auto_increment"; } }
    }

    public class MergeDatabase
    {
        public string ConnectionStringWithDB;
        public MergeTable[] Tables;

        public MergeDatabase() { }
        public MergeDatabase(string connectionString)
        {
            this.ConnectionStringWithDB = connectionString;
        }
    }
    public class MergeTable
    {
        public string TableName;
        /// <summary>
        /// <para>SELECT * FROM TABLE 后的筛选条件，符合条件的数据将被留下，如下</para>
        /// <para>WHERE registTime > '2016/7/20'</para>
        /// <para>JOIN ON playerinfo ON (TABLE.uid = playerinfo.uid) WHERE level > 10</para>
        /// </summary>
        public string Where;
        public bool AutoMergeIdentity = true;
        //public Action<_DATABASE.Database> Update;

        public MergeTable() { }
        public MergeTable(string tableName)
        {
            this.TableName = tableName;
        }
        public MergeTable(string tableName, string where)
        {
            this.TableName = tableName;
            this.Where = where;
        }
    }
    public class AsyncThread : Async
    {
        private Func<float> func;

        public void Queue(Func<float> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");
            this.func = func;
            Run();
        }
        protected override void InternalRun()
        {
            if (!System.Threading.ThreadPool.QueueUserWorkItem((state) =>
            {
                while (true)
                {
                    lock (this)
                        if (State != EAsyncState.Running)
                            break;
                    try
                    {
                        float progress = func();
                        lock (this)
                            ProgressFloat = progress;
                    }
                    catch (Exception ex)
                    {
                        lock (this)
                            Error(ex);
                        break;
                    }
                }
            }))
            {
                throw new OutOfMemoryException();
            }
        }

        public static AsyncThread QueueUserWorkItem(Func<float> func)
        {
            AsyncThread thread = new AsyncThread();
            thread.Queue(func);
            return thread;
        }
        public static AsyncThread QueueUserWorkItem(Func<bool> func)
        {
            return QueueUserWorkItem(() => func() ? 1 : 0);
        }
        public static AsyncThread QueueUserWorkItem(Action action)
        {
            return QueueUserWorkItem(() =>
            {
                action();
                return 1;
            });
        }
    }
#endif

    public enum EIndex : byte
    {
        /// <summary>
        /// 索引：用于查询
        /// </summary>
        Index,
        /// <summary>
        /// 主键：添加时要测重
        /// <para>多主键时，每个主键生成Group</para>
        /// <para>所有主键生成一个复合主键类型，由此类型生成</para>
        /// </summary>
        Primary,
        /// <summary>
        /// 自增：视为主键
        /// </summary>
        Identity,
        /// <summary>
        /// 分组：例如同一个玩家的操作记录
        /// </summary>
        Group,
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class IndexAttribute : Attribute
    {
        public EIndex Index
        {
            get;
            private set;
        }

        public IndexAttribute()
        {
        }
        public IndexAttribute(EIndex index)
        {
            this.Index = index;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class ForeignAttribute : Attribute
    {
        public Type ForeignTable
        {
            get;
            private set;
        }
        public string ForeignField
        {
            get;
            private set;
        }

        public ForeignAttribute(Type type, string field)
        {
            this.ForeignTable = type;
            this.ForeignField = field;
        }
        public ForeignAttribute(Type type)
        {
            this.ForeignTable = type;
        }
    }
    /// <summary>数据库不生成标记此特性的字段</summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Code(ECode.ToBeContinue)]
    public class IgnoreAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class MemoryTableAttribute : Attribute
    {
        public bool TempTable { get; private set; }
        public MemoryTableAttribute()
        {
        }
        public MemoryTableAttribute(bool temp)
        {
            TempTable = temp;
        }
    }
}