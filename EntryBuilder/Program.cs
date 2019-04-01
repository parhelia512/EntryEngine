﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using EntryEngine;
using EntryEngine.Network;
using EntryEngine.Serialize;
using PhotoshopFile;
using System.Drawing.Imaging;
using EntryBuilder.CodeAnalysis.Syntax;
using EntryBuilder.CodeAnalysis.Semantics;
using EntryBuilder.CodeAnalysis.Refactoring;
using System.Threading;

namespace EntryBuilder
{
    partial class Program
	{
        class Logger : _LOG.Logger
        {
            private const byte LOG = (byte)ELog.Debug;
            private byte last;

            public ConsoleColor[] Colors
            {
                get;
                private set;
            }

            public Logger()
            {
                Colors = new ConsoleColor[]
                {
                    ConsoleColor.Gray,
                    ConsoleColor.White,
                    ConsoleColor.DarkYellow,
                    ConsoleColor.Red,
                };
            }

            public override void Log(ref Record record)
            {
                byte level = record.Level;
                if (level > LOG)
                    return;

                if (level != last)
                {
                    last = level;
                    Console.ForegroundColor = Colors[level];
                }
                Console.WriteLine("[{0}] {1}", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ToString());
            }
        }
		private static MethodInfo[] Methods;

        class TestResolve
        {
            public string Name;
            public string[] Files;
            public string Symbols;
            public TestResolve(string name, string symbols, string[] files)
            {
                this.Name = name;
                this.Symbols = symbols;
                this.Files = files;
            }
        }

        [STAThread]
		static void Main(string[] args)
        {
            //BuildStaticInterface("EntryEngine.dll\\EntryEngine._LOG", "a.cs", true, "");

            //TexAnimationFromExcel("Outputs\\a.xlsx", "Outputs", "Outputs");
            //Environment.CurrentDirectory = @"D:\Projects\AIDemo\Design\Pack\";
            //TexPiece("打包图.xlsx", @"..\..\Launch\Client\Content\", @"..\..\Launch\Client\Content\");
            //TestText();
            //Console.ReadKey();
            //return;

            _LOG._Logger = new Logger();

            //BuildDll(@"..\..\..\Xna\", "Output\\Dummy\\Xna.dll", "3.5", "", true, "");

            //BuildEntryEngine(@"..\..\..\EntryEngine\");

            //BuildLinkShell(@"Outputs\EntryEditor\", "3.5", 1, "Outputs\\EntryEditor.exe", "", "2017-12-30");

            //BuildCSVFromExcel(@"D:\Project\FaerieChronicle\Design\Tables_Build\战斗相关.xlsx", "Outputs\\Unity\\", null, "12.0", "Outputs\\Unity\\TABLE.cs", false);

            //PsdFile psd = new PsdFile("Test/远征港（坐标）_JP.t.psd");
            //Layer li = psd.Layers.FirstOrDefault(l => l.Name == "确认");

            //BuildDatabaseMysql(@"..\..\..\EntryBuilderTest\bin\Debug\EntryBuilderTest.dll", "EntryBuilderTest.DB", @"..\..\..\EntryBuilderTest\DB.cs", "RELEASE", "");
            //return;

            //BuildPSDTranslate("Test", "LANGUAGE.csv");
            //return;

            //BuildTranslatePSD("Test", "LANGUAGE.csv", "JP");
            //Console.ReadLine();
            //return;

            #region Test Code Analysis

            //TestResolve[] testResolves = new TestResolve[]
            //{
            //    new TestResolve(".net", "CLIENT;SERVER", Directory.GetFiles(@"..\..\..\CSharp\.net\", "*.cs", SearchOption.AllDirectories)
            //        .Concat(Directory.GetFiles(@"..\..\..\JavaScript\", "*.cs", SearchOption.TopDirectoryOnly)).ToArray()),
            //    new TestResolve("EntryEngine", "CLIENT;HTML5", Directory.GetFiles(@"..\..\..\EntryEngine\", "*.cs", SearchOption.AllDirectories)),
            //    new TestResolve("HTML5", "", Directory.GetFiles(@"..\..\..\HTML5\", "*.cs", SearchOption.AllDirectories)),
            //    //new TestResolve("Chamber", "", Directory.GetFiles(@"D:\Project\ChamberH5\Code\Client\", "*.cs", SearchOption.AllDirectories)),
            //    //new TestResolve("Entry", "HTML5", Directory.GetFiles(@"D:\Project\ChamberH5\Code\Chamber\", "*.cs", SearchOption.AllDirectories)),
            //    new TestResolve("Project", "", Directory.GetFiles(@"D:\Project\TestWebgl\Code\Client\Client\", "*.cs", SearchOption.AllDirectories)),
            //    new TestResolve("Entry", "HTML5", Directory.GetFiles(@"D:\Project\TestWebgl\Code\Client\PCRun", "*.cs", SearchOption.AllDirectories)),
            //};

            //Stopwatch watch = Stopwatch.StartNew();
            //List<DefineFile> defines = new List<DefineFile>();
            //foreach (var item in testResolves)
            //{
            //    Project project = new Project();
            //    project.AddSymbols(item.Symbols);
            //    project.ParseFromFile(item.Files);
            //    Console.WriteLine("Parse [{0}]: {1}", item.Name, watch.ElapsedMilliseconds.ToString());
            //    Refactor.Resolve(project, true);
            //    Console.WriteLine("Resolve [{0}]: {1}", item.Name, watch.ElapsedMilliseconds.ToString());
            //    defines.AddRange(project.Files);
            //}
            //Refactor.Optimize();

            //string code = Refactor.RebuildCode(ECodeLanguage.JavaScript, defines.ToArray());
            //string code2 = _RH.Indent(code);
            //StringBuilder builder = new StringBuilder();
            //builder.AppendLine("<head><meta charset=\"utf-8\"></head>");
            //builder.AppendLine("<body>");
            ////builder.AppendLine("<canvas id=\"WEBGL\"></canvas>");
            ////builder.AppendLine("<canvas id=\"CANVAS\"></canvas>");
            //builder.AppendLine("</body>");
            //builder.AppendLine("<script>");
            //builder.AppendLine(code2);
            //builder.AppendLine("console.log(\"LOAD COMPLETED\");");
            //builder.AppendLine("console.log(\"RUNNING\");");
            //builder.AppendLine("Program.Main(null);");
            //builder.AppendLine("console.log(\"EXITED\");");
            //builder.AppendLine("</script>");
            //watch.Stop();
            ////File.WriteAllText(@"D:\Project\ChamberH5\PublishH5\test.html", builder.ToString());
            //File.WriteAllText(@"D:\Project\TestWebgl\Publish\Webgl\test.html", builder.ToString());
            //Console.WriteLine("Write code: {0}", watch.ElapsedMilliseconds.ToString());
            //Console.ReadKey();

            //return;
            #endregion

			Methods = typeof(Program).GetMethods(BindingFlags.Static | BindingFlags.Public);

            try
            {
                if (args.Length > 0)
                {
                    Invoke(args);
                }
                else
                {
                    while (true)
                    {
                        for (int i = 0; i < Methods.Length; i++)
                        {
                            MethodInfo method = Methods[i];

                            var obsolete = method.GetAttribute<ObsoleteAttribute>();
                            if (obsolete != null)
                            {
                                Console.Write("[Obsolete!", i, method.Name);
                                if (!string.IsNullOrEmpty(obsolete.Message))
                                    Console.Write(" {0}", obsolete.Message);
                                Console.Write("] ");
                            }

                            string param = "";
                            ParameterInfo[] parameters = method.GetParameters();
                            foreach (ParameterInfo parameter in parameters)
                            {
                                param += string.Format(" {0}({1})", parameter.Name, parameter.ParameterType.Name);
                            }
                            Console.WriteLine("{0}. {1} {2}", i, method.Name, param);
                        }
                        string input = Console.ReadLine();
                        args = input.Split(new char[] { ' ' }, StringSplitOptions.None);
                        if (args.Length == 0)
                            break;
                        Invoke(args);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ex = ex.InnerException;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
		}

		private static void Invoke(string[] args)
		{
			string symbol = args[0];
			MethodInfo method = Methods.FirstOrDefault(m => m.Name == symbol);
			if (method == null)
				method = Methods[int.Parse(symbol)];
			ParameterInfo[] parameters = method.GetParameters();

			int paramCount = parameters.Length;
			object[] param = new object[paramCount];
			for (int i = 0; i < paramCount; i++)
				param[i] = Convert.ChangeType(args[i + 1], parameters[i].ParameterType);
			if (args.Length > paramCount + 1)
				Environment.CurrentDirectory = Path.GetFullPath(args.Last());
			method.Invoke(null, param);

			Console.WriteLine("invoke {0} completed!", method.Name);
			Console.WriteLine();
		}
		private static Type[] GetDllTypes(string dllAndNamespace, SearchOption option)
		{
			// path\Dll.dll\Namespace
			int index = dllAndNamespace.LastIndexOf('\\');
			string lib = dllAndNamespace.Substring(0, index);
			Assembly assembly = Assembly.LoadFrom(lib);
			if (assembly == null)
			{
				Console.WriteLine("no library: {0}", lib);
				return null;
			}
			// Namespace
			dllAndNamespace = dllAndNamespace.Substring(index + 1);
			if (option == SearchOption.TopDirectoryOnly)
				return assembly.GetTypes().Where(t => t.Namespace == dllAndNamespace).ToArray();
			else
				return assembly.GetTypes().Where(t => t.Namespace.StartsWith(dllAndNamespace)).ToArray();
		}
		private static Type GetDllType(string dllAndType)
		{
			// path\Dll.dll\Namespace.Class
			int index = dllAndType.LastIndexOf('\\');
			string lib = Path.GetFullPath(dllAndType.Substring(0, index));
            string libName = Path.GetFileNameWithoutExtension(lib);
            var name = AppDomain.CurrentDomain.GetAssemblies()[0].GetName();
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == libName);
            if (assembly == null)
			    assembly = Assembly.LoadFrom(lib);
			if (assembly == null)
			{
				Console.WriteLine("no library: {0}", lib);
				return null;
			}
			// Class
			string type = dllAndType.Substring(index + 1);
			Type result = assembly.GetType(type);
			if (result == null)
				Console.WriteLine("no type: {0}", type);
			return result;
		}
		private static T GetDllTypeInstance<T>(string dllAndType) where T : class
		{
			Type type = GetDllType(dllAndType);
			if (type == null)
				return null;
			T instance = Activator.CreateInstance(type) as T;
			if (instance == null)
				Console.WriteLine("can not create instance of {0}", type.FullName);
			return instance;
		}
		private static string[] GetFiles(string dirOrFile, string pattern = null, SearchOption option = SearchOption.TopDirectoryOnly)
		{
			if (pattern == null || Path.GetFileName(dirOrFile).Contains('.'))
			{
				return new string[] { dirOrFile };
			}
			else
			{
				return Directory.GetFiles(dirOrFile, pattern, option);
			}
		}
		private static IEnumerable<string> GetFiles(string dirOrFile, SearchOption option = SearchOption.TopDirectoryOnly, params string[] patterns)
		{
			foreach (var pattern in patterns)
			{
				foreach (var file in GetFiles(dirOrFile, pattern, option))
				{
					yield return file;
				}
			}
		}
		private static void BuildDir(ref string dir, bool full = false)
		{
			if (Path.GetFileName(dir).Contains('.'))
				return;
			if (string.IsNullOrEmpty(dir))
				dir = Environment.CurrentDirectory;
            if (full)
                dir = Path.GetFullPath(dir);
            dir = _IO.DirectoryWithEnding(dir);
		}
		private static void ForeachDirectory(string inputDir, Action<DirectoryInfo> call)
		{
			DirectoryInfo directory = new DirectoryInfo(inputDir);
			ForeachDirectory(directory, call);
		}
		private static void ForeachDirectory(DirectoryInfo directory, Action<DirectoryInfo> call)
		{
			DirectoryInfo[] directories = directory.GetDirectories();
			foreach (var dir in directories)
			{
				call(dir);
				ForeachDirectory(dir, call);
			}
		}
		private static string GetFullPath(string path)
		{
			BuildDir(ref path);
			return Path.GetFullPath(path);
		}
		private static void ToFullPath(ref string path)
		{
			path = GetFullPath(path);
		}

		private static string[] DEFAULT_NAMESPACE =
		{
			"System",
			"System.Collections.Generic",
			"System.Linq",
			"System.Text",
		};
		private static void SaveCode(string file, StringBuilder code)
		{
			SaveCode(file, code.ToString());
		}
		private static void SaveCode(string file, string code)
		{
            File.WriteAllText(file, _RH.Indent(code), Encoding.UTF8);
		}
		private static void AppendDefaultNamespace(HashSet<string> set, StringBuilder builder)
		{
			foreach (var item in DEFAULT_NAMESPACE)
			{
				if (set.Add(item))
				{
					builder.AppendLine("using {0};", item);
				}
			}
		}
		private static HashSet<string> AppendDefaultNamespace(StringBuilder builder)
		{
			HashSet<string> set = new HashSet<string>();
			AppendDefaultNamespace(set, builder);
			return set;
		}
		private static void BuildDepthInvoke(StringBuilder builder, Type type, string[] instances, byte depth)
		{
			if (!type.IsCustomType())
				return;

			string instance = string.Join(".", instances);
			string insname = string.Join("", instances.Skip(1).ToArray());

			BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
			var fields = type.GetFields(flag);
			var properties = type.GetProperties(flag).WithoutIndex();
			var methods = type.GetAllMethods(flag).MethodOnly().DeclareTypeNotObject();

			foreach (var field in fields)
			{
				builder.AppendLine("public static {0} {1}{2}", field.FieldType.CodeName(), insname, field.Name);
				builder.AppendBlock(() =>
				{
					builder.AppendLine("get {{ return {0}.{1}; }}", instance, field.Name);
                    if (!field.IsInitOnly)
					    builder.AppendLine("set {{ {0}.{1} = value; }}", instance, field.Name);
				});
				if (depth != 0)
					if (!field.FieldType.IsValueType)
						BuildDepthInvoke(builder, field.FieldType, instances.Add(field.Name), (byte)(depth - 1));
			}
			foreach (var property in properties)
			{
				builder.AppendLine("public static {0} {1}{2}", property.PropertyType.CodeName(), insname, property.Name);
				builder.AppendBlock(() =>
				{
					if (property.CanRead && property.GetGetMethod(false) != null)
						builder.AppendLine("get {{ return {0}.{1}; }}", instance, property.Name);
					if (property.CanWrite && property.GetSetMethod(false) != null)
						builder.AppendLine("set {{ {0}.{1} = value; }}", instance, property.Name);
				});
				if (depth != 0)
					if (!property.PropertyType.IsValueType)
						BuildDepthInvoke(builder, property.PropertyType, instances.Add(property.Name), (byte)(depth - 1));
			}
			foreach (var method in methods)
			{
				builder.Append("public static {0} {1}{2}", method.ReturnType.CodeName(), insname, method.Name);
				builder.AppendMethodParametersWithBracket(method);
				builder.AppendBlock(() =>
				{
					builder.AppendMethodInvoke(method, instance);
				});
			}
		}
        private static string GetMySqlType(Type type)
        {
            bool special;
            return GetMySqlType(type, out special);
        }
        private static string GetMySqlType(Type type, out bool special)
        {
            special = false;
            if (type == typeof(bool))
                return "BIT";
            else if (type == typeof(sbyte))
                return "TINYINT";
            else if (type == typeof(byte))
                return "TINYINT UNSIGNED";
            else if (type == typeof(short))
                return "SMALLINT";
            else if (type == typeof(ushort))
                return "SMALLINT UNSIGNED";
            else if (type == typeof(char))
                return "CHAR";
            else if (type == typeof(int))
                return "INT";
            else if (type == typeof(uint))
                return "INT UNSIGNED";
            else if (type == typeof(float))
                return "FLOAT";
            else if (type == typeof(long))
                return "BIGINT";
            else if (type == typeof(ulong))
                return "BIGINT UNSIGNED";
            else if (type == typeof(double))
                return "DOUBLE";
            else if (type == typeof(TimeSpan))
                return "TIME";
            else if (type == typeof(DateTime))
                return "DATETIME";
            else if (type == typeof(string))
                return "TEXT";
            else if (type == typeof(byte[]))
                return "LONGBLOB";
            else if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
                return GetMySqlType(type, out special);
            }
            else
            {
                special = true;
                //throw new ArgumentException("type");
                return "TEXT";
            }
        }
        class TreeField : Tree<TreeField>
        {
            public FieldInfo Field
            {
                get;
                private set;
            }
            public Type Table
            {
                get { return Field.DeclaringType; }
            }
            public TreeField()
            {
            }
            public TreeField(FieldInfo field)
            {
                this.Field = field;
            }
        }
        class ProtocolDefault
        {
            public static Type DELEGATE_TYPE = typeof(Delegate);

            public StringBuilder builder
            {
                get;
                private set;
            }
            public StringBuilder server
            {
                get;
                private set;
            }
            protected Type type
            {
                get;
                private set;
            }
            protected ProtocolStubAttribute agent
            {
                get;
                private set;
            }

            public void Write(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException("type");
                this.type = type;

                this.agent = type.GetAttribute<ProtocolStubAttribute>();
                if (agent == null)
                    throw new ArgumentNullException("agent", string.Format("{0} don't have a {1}", type.FullName, typeof(ProtocolStubAttribute).Name));

                MethodInfo[] call = type.GetInterfaceMethods().ToArray();
                MethodInfo[] callback = agent.Callback == null ? new MethodInfo[0] : agent.Callback.GetInterfaceMethods().ToArray();
                Dictionary<int, Type> asyncCB = new Dictionary<int, Type>();
                for (int i = 0; i < call.Length; i++)
                {
                    if (call[i].ReturnType != typeof(void))
                        throw new NotImplementedException("Call function can't have a return value.");
                    var list = call[i].GetParameters().Where(p => p.ParameterType.Is(DELEGATE_TYPE)).ToList();
                    if (list.Count > 1)
                        throw new NotSupportedException("Delegate parameter can have only one.");
                    ParameterInfo d = list.FirstOrDefault();
                    if (d != null)
                        asyncCB.Add(i, d.ParameterType);
                }

                Write(call, callback, asyncCB);
            }
            protected virtual void Write(MethodInfo[] call, MethodInfo[] callback, Dictionary<int, Type> asyncCB)
            {
                builder = new StringBuilder();
                WNamespace(builder);
                server = new StringBuilder(builder.ToString());

                WSInterface(server, call, asyncCB);
                WSCallbackType(server, call, asyncCB);
                WSCallbackProxy(server, call, callback, asyncCB);
                WSAgentStub(server, call, asyncCB);

                WCCallProxy(builder, call, callback, asyncCB);
                // 生成默认的代理人
                if (agent.Callback != null)
                    WCDefaultAgentStub(builder, callback);
            }
            protected virtual void WNamespace(StringBuilder builder)
            {
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using EntryEngine;");
                builder.AppendLine("using EntryEngine.Network;");
                builder.AppendLine("using EntryEngine.Serialize;");
                if (!string.IsNullOrEmpty(type.Namespace))
                    builder.AppendLine("using {0};", type.Namespace);
                builder.AppendLine();
            }
            protected virtual void WSInterface(StringBuilder builder, MethodInfo[] call, Dictionary<int, Type> asyncCB)
            {
                bool hasAgent = !string.IsNullOrEmpty(agent.AgentType);

                builder.AppendLine("interface _{0}", type.Name);
                builder.AppendBlock(() =>
                {
                    for (int i = 0, n = call.Length; i < n; i++)
                    {
                        MethodInfo method = call[i];
                        builder.Append("void {0}(", method.Name);
                        bool hasDelegate = asyncCB.ContainsKey(i);
                        bool notFirst = false;
                        // 有指代理类型时，第一个参数为代理类型参数
                        if (hasAgent)
                        {
                            builder.Append("{0} __client", agent.AgentType);
                            notFirst = true;
                        }
                        var parameters = method.GetParameters();
                        foreach (ParameterInfo param in parameters)
                        {
                            // 不是第一个参数需要逗号分隔
                            if (notFirst)
                                builder.Append(", ");
                            // 参数类型，委托类型参数是生成的类型
                            if (hasDelegate && param.ParameterType.Is(typeof(Delegate)))
                                builder.Append("CB{0}_{1}", type.Name, method.Name);
                            else
                                builder.Append(param.ParameterType.CodeName());
                            // 参数名
                            builder.Append(" {0}", param.Name);
                            notFirst = true;
                        }
                        builder.AppendLine(");");
                    }
                });
                builder.AppendLine();
            }
            protected virtual void WSCallbackType(StringBuilder builder, MethodInfo[] call, Dictionary<int, Type> asyncCB)
            {
                foreach (var item in asyncCB)
                {
                    string name = string.Format("CB{0}_{1}", type.Name, call[item.Key].Name);
                    builder.AppendLine("class {0} : IDisposable", name);
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("private byte __id;");
                        builder.AppendLine("private Link __link;");
                        //builder.AppendLine("\tinternal int? Delay;");
                        builder.AppendLine("internal bool IsCallback { get; private set; }");
                        builder.AppendLine("public {0}(byte id, Link link)", name);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("this.__id = id;");
                            builder.AppendLine("this.__link = link;");
                        });

                        // 回调方法头
                        builder.Append("\tpublic void Callback(");
                        var parameters = item.Value.GetMethod("Invoke").GetParameters();
                        for (int i = 0, n = parameters.Length; i < n; i++)
                        {
                            if (i > 0)
                                builder.Append(", ");
                            var param = parameters[i];
                            builder.AppendFormat("{0} {1}", param.ParameterType.CodeName(), param.Name);
                        }
                        builder.AppendLine(") // INDEX = {0}", item.Key);
                        // 具体实现
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("if (IsCallback) return;");
                            // 参数
                            builder.AppendLine("ByteWriter __writer = new ByteWriter();");
                            builder.AppendLine("__writer.Write(__id);");
                            builder.AppendLine("__writer.Write((sbyte)0);");
                            foreach (ParameterInfo param in parameters)
                                builder.AppendLine("__writer.Write({0});", param.Name);
                            // 记录日志
                            builder.AppendLine("#if DEBUG");
                            builder.Append("_LOG.Debug(\"{0} Callback({{0}} bytes)", name);
                            for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                            {
                                ParameterInfo param = parameters[j];
                                builder.Append(" {0}: {{{1}}}", param.Name, j + 1);
                                if (j != n)
                                    builder.Append(",");
                            }
                            builder.Append("\", __writer.Position");
                            foreach (ParameterInfo param in parameters)
                                builder.Append(", JsonWriter.Serialize({0})", param.Name);
                            builder.AppendLine(");");
                            builder.AppendLine("#endif");
                            // 回调
                            builder.AppendLine("{0}Proxy.{1}_{2}(__link, __writer.Buffer, __writer.Position);", agent.Callback == null ? type.Name + "Callback" : agent.Callback.Name, call[item.Key].Name, item.Key);
                            builder.AppendLine("IsCallback = true;");
                        });

                        // 请求错误
                        builder.AppendLine("public void Error(sbyte ret, string msg)");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("if (IsCallback) return;");
                            builder.AppendLine("ByteWriter __writer = new ByteWriter();");
                            // 协议及参数信息
                            builder.AppendLine("__writer.Write((byte){0});", agent.Protocol);
                            builder.AppendLine("__writer.Write((ushort){0});", item.Key);
                            builder.AppendLine("__writer.Write(__id);");
                            builder.AppendLine("__writer.Write(ret);");
                            builder.AppendLine("__writer.Write(msg);");
                            // 日志
                            builder.AppendLine("#if DEBUG");
                            builder.AppendLine("_LOG.Debug(\"{0} Error({{0}} bytes) ret={{1}} msg={{2}}\", __writer.Position, ret, msg);", name);
                            builder.AppendLine("#endif");
                            // 回调
                            builder.AppendLine("__link.Write(__writer.Buffer, 0, __writer.Position);");
                            builder.AppendLine("IsCallback = true;");
                        });

                        // async callback
                        // 接口的处理方法涉及异步（例如登录，需要查询数据库）
                        // 不能立刻进行回调
                        // 需要延迟回调，当延迟时间过后仍未回调
                        // 再自动调用callback.Error
                        //builder.AppendLine("\tpublic void DelayCallback(int ms)");
                        //builder.AppendLine("\t{");
                        //builder.AppendLine("\t\tif (IsCallback) return;");
                        //builder.AppendLine("\t\tDelay = ms;");
                        //builder.AppendLine("\t}");
                        builder.AppendLine("public void Dispose()");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("if (!IsCallback) Error(-2, \"no callback\");");
                        });
                    });
                }
                builder.AppendLine();
            }
            protected virtual void WSCallbackProxy(StringBuilder builder, MethodInfo[] call, MethodInfo[] callback, Dictionary<int, Type> asyncCB)
            {
                if (agent.Callback == null)
                    builder.AppendLine("static class {0}CallbackProxy", type.Name);
                else
                    builder.AppendLine("static class {0}Proxy", agent.Callback.Name);
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("public static Action<ByteWriter> __WriteAgent;");
                    for (int i = 0, offset = 0,
                        len = callback.Length, n = call.Length + len;
                        i < n; i++)
                    {
                        // 调用中含有委托参数的方法和回调接口都需要生成回调函数
                        bool isDelegate = asyncCB.ContainsKey(i);
                        MethodInfo method;
                        if (isDelegate)
                        {
                            method = call[i];
                            offset++;
                        }
                        else
                        {
                            if (i - offset >= len)
                                continue;
                            method = callback[i - offset];
                        }

                        // 方法头
                        ParameterInfo[] parameters = null;
                        if (isDelegate)
                            builder.AppendLine("internal static void {0}_{1}(Link __link, byte[] data, int position)", method.Name, i);
                        else
                        {
                            builder.Append("public static {0} {1}(Link __link", method.ReturnType.CodeName(), method.Name);
                            parameters = method.GetParameters();
                            foreach (ParameterInfo param in parameters)
                                builder.AppendFormat(", {0} {1}", param.ParameterType.CodeName(), param.Name);
                            builder.AppendLine(")");
                        }

                        // 方法体
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("if (__link == null) return;");
                            builder.AppendLine("ByteWriter __writer = new ByteWriter();");
                            builder.AppendLine("if (__WriteAgent != null) __WriteAgent(__writer);");
                            // 协议
                            builder.AppendLine("__writer.Write((byte){0});", agent.Protocol);
                            builder.AppendLine("__writer.Write((ushort){0});", i);
                            // 回调参数内容
                            if (isDelegate)
                            {
                                builder.AppendLine("__writer.WriteBytes(data, 0, position);", i);
                                //builder.AppendLine("#if !OUTPUT");
                                //builder.AppendFormatLine("\t\tLog.Debug(\"async {0}.{1}({{0}} bytes)\", writer.Position);", type.Name, method.Name);
                                //builder.AppendLine("#endif");
                            }
                            else
                            {
                                foreach (ParameterInfo param in parameters)
                                    builder.AppendLine("__writer.Write({0});", param.Name);
                                // 日志
                                builder.AppendLine("#if DEBUG");
                                builder.AppendFormat("_LOG.Debug(\"{0}({{0}} bytes)", method.Name);
                                for (int j = 0, m = parameters.Length - 1; j <= m; j++)
                                {
                                    ParameterInfo param = parameters[j];
                                    builder.Append(" {0}: {{{1}}}", param.Name, j + 1);
                                    if (j != m)
                                        builder.Append(",");
                                }
                                builder.Append("\", __writer.Position");
                                for (int j = 0, m = parameters.Length - 1; j <= m; j++)
                                {
                                    ParameterInfo param = parameters[j];
                                    if (!param.ParameterType.IsCustomType())
                                        builder.AppendFormat(", {0}", param.Name);
                                    else
                                        builder.AppendFormat(", JsonWriter.Serialize({0})", param.Name);
                                }
                                builder.AppendLine(");");
                                builder.AppendLine("#endif");
                            }
                            builder.AppendLine("__link.Write(__writer.Buffer, 0, __writer.Position);");
                        });
                    }
                });
                builder.AppendLine();
            }
            protected virtual void WSAgentStub(StringBuilder builder, MethodInfo[] call, Dictionary<int, Type> asyncCB)
            {
                bool hasAgent = !string.IsNullOrEmpty(agent.AgentType);

                string name = type.Name + "Stub";
                builder.AppendLine("class {0} : {1}", name, typeof(Stub).Name);
                builder.AppendBlock(() =>
                {
                    string name2 = hasAgent ? agent.AgentType : "_" + type.Name;

                    builder.AppendLine("public _{0} __Agent;", type.Name);
                    builder.AppendLine("public Func<_{0}> __GetAgent;", type.Name);
                    builder.AppendLine("public Func<ByteReader, {0}> __ReadAgent;", name2);
                    builder.AppendLine("public {0}(_{1} agent) : base({2})", name, type.Name, agent.Protocol);
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("this.__Agent = agent;");
                        foreach (var method in call)
                            builder.AppendLine("AddMethod({0});", method.Name);
                    });
                    builder.AppendLine("public {0}(Func<_{1}> agent) : this((_{1})null)", name, type.Name, agent.Protocol);
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("this.__GetAgent = agent;");
                    });
                    builder.AppendLine("public {0}(Func<ByteReader, {1}> agent) : this((_{2})null)", name, name2, type.Name);
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("this.__ReadAgent = agent;");
                    });
                    // 根据方法参数生成对应的处理方法存根
                    for (int i = 0; i < call.Length; i++)
                    {
                        MethodInfo method = call[i];
                        bool isDelegate = asyncCB.ContainsKey(i);
                        // 方法头
                        builder.AppendLine("{0} {1}(ByteReader __stream)", method.ReturnType.CodeName(), method.Name);

                        ParameterInfo[] parameters = method.GetParameters();
                        builder.AppendBlock(() =>
                        {
                            //builder.AppendLine("Timer timer = Timer.StartNew();");    // 统计方法调用次数，可用于分析用户操作
                            builder.AppendLine("var agent = __Agent;");
                            builder.AppendLine("if (__GetAgent != null) { var temp = __GetAgent(); if (temp != null) agent = temp; }");
                            if (hasAgent)
                            {
                                // 指定了代理人类型时，处理方法首个参数将可由读取委托来获得
                                builder.AppendLine("var __client = default({0});", agent.AgentType);
                                builder.AppendLine("if (__ReadAgent != null) { var temp = __ReadAgent(__stream);if (temp != null) __client = temp; }");
                            }
                            else
                            {
                                // 没有指定代理人类型时，将采用默认代理人
                                builder.AppendLine("if (__ReadAgent != null) { var temp = __ReadAgent(__stream); if (temp != null) agent = temp; }");
                            }
                            // 参数声明
                            foreach (ParameterInfo param in parameters)
                            {
                                if (isDelegate && param.ParameterType.Is(typeof(Delegate)))
                                {
                                    builder.AppendLine("byte {0};", param.Name);
                                    name = param.Name;
                                }
                                else
                                    builder.AppendLine("{0} {1};", param.ParameterType.CodeName(), param.Name);
                            }
                            // 参数读取赋值
                            foreach (ParameterInfo param in parameters)
                                builder.AppendLine("__stream.Read(out {0});", param.Name);
                            // 日志
                            builder.AppendLine("#if DEBUG");
                            builder.AppendFormat("_LOG.Debug(\"{0}", method.Name);
                            for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                            {
                                ParameterInfo param = parameters[j];
                                builder.AppendFormat(" {0}: {{{1}}}", param.Name, j);
                                if (j != n)
                                    builder.Append(",");
                            }
                            builder.Append("\"");
                            for (int j = 0; j < parameters.Length; j++)
                            {
                                ParameterInfo param = parameters[j];
                                if (param.ParameterType.Is(typeof(Delegate)))
                                    builder.AppendFormat(", \"{0}\"", param.ParameterType.CodeName());
                                else if (!param.ParameterType.IsCustomType())
                                    builder.AppendFormat(", {0}", param.Name);
                                else
                                    builder.AppendFormat(", JsonWriter.Serialize({0})", param.Name);
                            }
                            builder.AppendLine(");");
                            builder.AppendLine("#endif");
                            // 让代理人根据参数调用代理函数
                            if (isDelegate)
                            {
                                const string CB = "__callback";
                                builder.AppendLine("var {1} = new CB{2}_{3}({0}, Link);", name, CB, type.Name, method.Name);
                                builder.AppendLine("try");
                                builder.AppendBlock(() =>
                                {
                                    builder.AppendFormat("agent.{0}({1}", method.Name, hasAgent ? "__client, " : "");
                                    for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                                    {
                                        ParameterInfo param = parameters[j];
                                        if (j == n)
                                            builder.AppendFormat(CB);
                                        else
                                        {
                                            builder.AppendFormat("{0}", param.Name);
                                            builder.Append(", ");
                                        }
                                    }
                                    builder.AppendLine(");");
                                });
                                builder.AppendLine("catch (Exception ex)");
                                builder.AppendBlock(() =>
                                {
                                    builder.AppendLine("_LOG.Error(\"Callback_{0} error! msg={{0}} stack={{1}}\", ex.Message, ex.StackTrace);", method.Name);
                                    builder.AppendLine("if (!{0}.IsCallback) {0}.Error(-1, ex.Message);", CB);
                                });
                                // 没有回调不管，可能是进行了异步操作，等待其IDisposable.Dispose调用自然回调no callback消息
                                //builder.AppendLine("finally");
                                //builder.AppendBlock(() =>
                                //{
                                //    // 1. if (!__callback.IsCallback) __callback.Error(-2, "no callback");
                                //    // 2. if (__callback.Delay.HasValue) EntryService.Instance.Delay(__callback.Delay.Value, () => { if (!__callback.IsCallback) __callback.Error(-2, "no callback"); });
                                //    builder.AppendLine("if (!{0}.IsCallback)", CB);
                                //    builder.AppendLine("if ({0}.Delay.HasValue && {0}.Delay.Value > 0) EntryService.Instance.Delay(__callback.Delay.Value, () => {{ if (!{0}.IsCallback) {0}.Error(-2, \"no callback\"); }});", CB);
                                //    builder.AppendLine("else {0}.Error(-2, \"no callback\");", CB);
                                //});
                            }
                            else
                            {
                                builder.AppendFormat("agent.{0}({1}", method.Name, hasAgent ? (parameters.Length == 0 ? "__client" : "__client, ") : "");
                                for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                                {
                                    ParameterInfo param = parameters[j];
                                    builder.Append("{0}", param.Name);
                                    if (j != n)
                                        builder.Append(", ");
                                }
                                builder.AppendLine(");");
                            }
                            //builder.AppendLine("\t\tStatistic.Add(\"{0}.{1}\", timer.Stop());", type.Name, method.Name);
                        });
                    }
                });
            }
            protected virtual void WCCallProxy(StringBuilder builder, MethodInfo[] call, MethodInfo[] callback, Dictionary<int, Type> asyncCB)
            {
                string name = type.Name + "Proxy";
                string cbname = agent.Callback != null ? agent.Callback.CodeName() : null;

                //builder.AppendLine("class {0} : {1}, {2}", name, typeof(StubClientAsync).Name, type.Name);
                builder.AppendLine("class {0} : {1}, {2}", name, typeof(StubClientAsync).Name, type.Name);
                builder.AppendBlock(() =>
                {
                    if (agent.Callback != null)
                    {
                        builder.AppendLine("public {0} __Agent;", cbname);
                        builder.AppendLine("public Func<{0}> __GetAgent;", cbname);
                        builder.AppendLine("public Func<ByteReader, {0}> __ReadAgent;", cbname);
                    }
                    builder.AppendLine("public Action<ByteWriter> __WriteAgent;");
                    builder.AppendLine();

                    // 构造函数，添加服务器回调处理方法存根
                    builder.AppendLine("public {0}()", name);
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("this.Protocol = {0};", agent.Protocol);
                        // add method
                        for (int i = 0, offset = 0,
                             len = callback.Length, n = call.Length + len;
                             i < n; i++)
                        {
                            bool isDelegate = asyncCB.ContainsKey(i);
                            MethodInfo method;
                            if (isDelegate)
                            {
                                method = call[i];
                                offset++;
                            }
                            else
                            {
                                if (i - offset >= len)
                                    continue;
                                method = callback[i - offset];
                            }
                            builder.AppendLine("AddMethod({1}, {0}_{1});", method.Name, i);
                        }
                    });
                    if (agent.Callback != null)
                    {
                        builder.AppendLine("public {0}({1} agent) : this()", name, cbname);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("this.__Agent = agent;");
                        });
                        builder.AppendLine("public {0}(Func<{1}> agent) : this()", name, cbname);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("this.__GetAgent = agent;");
                        });
                        builder.AppendLine("public {0}(Func<ByteReader, {1}> agent) : this()", name, cbname);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("this.__ReadAgent = agent;");
                        });
                    }
                    builder.AppendLine();

                    // 通过代理调用接口方法
                    WCCallProxy(builder, call, asyncCB);

                    // 通过代理人处理回调方法
                    WCAgentStub(builder, call, callback, asyncCB);
                });
            }
            protected virtual void WCCallProxy(StringBuilder builder, MethodInfo[] call, Dictionary<int, Type> asyncCB)
            {
                for (int i = 0; i < call.Length; i++)
                {
                    MethodInfo method = call[i];
                    ParameterInfo[] parameters = method.GetParameters();

                    // 方法头
                    //bool hasAsync = parameters.Contains(p => p.ParameterType.Is(typeof(Delegate)));
                    bool hasAsync = asyncCB.ContainsKey(i);
                    if (hasAsync)
                        builder.Append("public StubClientAsync.AsyncWaitCallback {0}(", method.Name);
                    else
                        builder.Append("public void {0}(", method.Name);
                    for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                    {
                        if (j != 0)
                            builder.Append(", ");
                        var param = parameters[j];
                        builder.Append("{0} {1}", param.ParameterType.CodeName(), param.Name);
                    }
                    builder.AppendLine(")");
                    builder.AppendBlock(() =>
                    {
                        if (hasAsync)
                            builder.AppendLine("if (Link == null) return null;");
                        else
                            builder.AppendLine("if (Link == null) return;");
                        builder.AppendLine("ByteWriter __writer = new ByteWriter();");
                        builder.AppendLine("if (__WriteAgent != null) __WriteAgent(__writer);");
                        builder.AppendLine("__writer.Write((byte){0});", agent.Protocol);
                        builder.AppendLine("__writer.Write((ushort){0});", i);
                        foreach (ParameterInfo param in parameters)
                        {
                            if (hasAsync && param.ParameterType.Is(typeof(Delegate)))
                            {
                                builder.AppendLine("var __async = Push({0});", param.Name);
                                builder.AppendLine("if (__async == null) return null;");
                                builder.AppendLine("__writer.Write(__async.ID);", param.Name);
                            }
                            else
                            {
                                builder.AppendLine("__writer.Write({0});", param.Name);
                            }
                        }
                        builder.AppendLine("#if DEBUG");
                        builder.AppendFormat("_LOG.Debug(\"{0}({{0}} bytes)", method.Name);
                        for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                        {
                            ParameterInfo param = parameters[j];
                            builder.AppendFormat(" {0}: {{{1}}}", param.Name, j + 1);
                            if (j != n)
                                builder.Append(",");
                        }
                        builder.Append("\", __writer.Position");
                        for (int j = 0; j < parameters.Length; j++)
                        {
                            ParameterInfo param = parameters[j];
                            if (hasAsync && param.ParameterType.Is(typeof(Delegate)))
                                builder.Append(", \"{0}\"", param.ParameterType.CodeName());
                            else if (!param.ParameterType.IsCustomType())
                                builder.Append(", {0}", param.Name);
                            else
                                builder.Append(", JsonWriter.Serialize({0})", param.Name);
                        }
                        builder.AppendLine(");");
                        builder.AppendLine("#endif");
                        builder.AppendLine("Link.Write(__writer.Buffer, 0, __writer.Position);");

                        if (hasAsync)
                            builder.AppendLine("return __async;");
                    });
                }
                builder.AppendLine();
            }
            protected virtual void WCAgentStub(StringBuilder builder, MethodInfo[] call, MethodInfo[] callback, Dictionary<int, Type> asyncCB)
            {
                for (int i = 0, offset = 0,
                     len = callback.Length, t = call.Length + len;
                     i < t; i++)
                {
                    bool idDelegate = asyncCB.ContainsKey(i);
                    MethodInfo method;
                    if (idDelegate)
                    {
                        method = call[i];
                        offset++;
                    }
                    else
                    {
                        if (i - offset >= len)
                            continue;
                        method = callback[i - offset];
                    }
                    builder.AppendLine("void {0}_{1}(ByteReader __stream)", method.Name, i);
                    builder.AppendBlock(() =>
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        //builder.AppendLine("Timer timer = Timer.StartNew();");    // 统计方法调用次数，可用于分析用户操作
                        if (!idDelegate)
                        {
                            builder.AppendLine("var __callback = __Agent;");
                            builder.AppendLine("if (__GetAgent != null) { var temp = __GetAgent(); if (temp != null) __callback = temp; }");
                            builder.AppendLine("if (__ReadAgent != null) { var temp = __ReadAgent(__stream); if (temp != null) __callback = temp; }");
                        }

                        if (idDelegate)
                        {
                            builder.AppendLine("byte __id;");
                            builder.AppendLine("sbyte __ret;");
                            builder.AppendLine("__stream.Read(out __id);");
                            builder.AppendLine("__stream.Read(out __ret);");
                            builder.AppendLine("var __callback = Pop(__id);");
                            //builder.AppendLine("if (__callback == null)");
                            //builder.AppendLine("throw new InvalidOperationException(\"no async request id=\" + id);");
                            // 请求正确回调
                            builder.AppendLine("if (__ret == 0)");
                            builder.AppendBlock(() =>
                            {
                                parameters = asyncCB[i].GetMethod("Invoke").GetParameters();
                                // 参数声明
                                foreach (ParameterInfo param in parameters)
                                    builder.AppendLine("{0} {1};", param.ParameterType.CodeName(), param.Name);
                                // 参数读取赋值
                                foreach (ParameterInfo param in parameters)
                                    builder.AppendLine("__stream.Read(out {0});", param.Name);
                                // 日志
                                builder.AppendLine("#if DEBUG");
                                builder.AppendFormat("_LOG.Debug(\"{0}", method.Name);
                                for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                                {
                                    ParameterInfo param = parameters[j];
                                    builder.AppendFormat(" {0}: {{{1}}}", param.Name, j);
                                    if (j != n)
                                        builder.Append(",");
                                }
                                builder.Append("\"");
                                for (int j = 0; j < parameters.Length; j++)
                                {
                                    ParameterInfo param = parameters[j];
                                    builder.AppendFormat(", JsonWriter.Serialize({0})", param.Name);
                                }
                                builder.AppendLine(");");
                                builder.AppendLine("#endif");
                                // 调用回调方法
                                builder.AppendLine("var __invoke = ({0})__callback.Function;", asyncCB[i].CodeName());
                                builder.AppendFormat("if (__invoke != null) __invoke(", method.Name);
                                for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                                {
                                    ParameterInfo param = parameters[j];
                                    builder.AppendFormat("{0}", param.Name);
                                    if (j != n)
                                        builder.Append(", ");
                                }
                                builder.AppendLine(");");
                            });
                            // 请求出错回调的错误信息
                            builder.AppendLine("else");
                            builder.AppendBlock(() =>
                            {
                                builder.AppendLine("string __msg;");
                                builder.AppendLine("__stream.Read(out __msg);");
                                builder.AppendLine("_LOG.Error(\"{0}_{1} error! id={{0}} ret={{1}} msg={{2}}\", __id, __ret, __msg);", method.Name, i);
                                //builder.AppendLine("Error(__callback, __id, __ret, __msg);");
                                builder.AppendLine("Error(__callback, {0}, __ret, __msg);", i);
                            });
                        }
                        else
                        {
                            // 参数声明
                            foreach (ParameterInfo param in parameters)
                                //if (!idDelegate || !param.ParameterType.Is(typeof(Delegate)))
                                builder.AppendLine("{0} {1};", param.ParameterType.CodeName(), param.Name);
                            // 参数读取赋值
                            foreach (ParameterInfo param in parameters)
                                //if (!idDelegate || !param.ParameterType.Is(typeof(Delegate)))
                                builder.AppendLine("__stream.Read(out {0});", param.Name);
                            // 日志
                            builder.AppendLine("#if DEBUG");
                            builder.AppendFormat("_LOG.Debug(\"{0}", method.Name);
                            for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                            {
                                ParameterInfo param = parameters[j];
                                builder.AppendFormat(" {0}: {{{1}}}", param.Name, j);
                                if (j != n)
                                    builder.Append(",");
                            }
                            builder.Append("\"");
                            foreach (ParameterInfo param in parameters)
                                builder.AppendFormat(", JsonWriter.Serialize({0})", param.Name);
                            builder.AppendLine(");");
                            builder.AppendLine("#endif");
                            // 调用回调方法
                            builder.AppendFormat("__callback.{0}(", method.Name);
                            for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                            {
                                ParameterInfo param = parameters[j];
                                builder.AppendFormat("{0}", param.Name);
                                if (j != n)
                                    builder.Append(", ");
                            }
                            builder.AppendLine(");");
                        }
                        //builder.AppendLine("\t\tStatistic.Add(\"{0}.{1}\", timer.Stop());", flag ? type.Name : agent.Callback.Name, method.Name);
                    });
                }
            }
            protected virtual void WCDefaultAgentStub(StringBuilder builder, MethodInfo[] callback)
            {
                string cbName = agent.Callback.CodeName();
                builder.AppendLine();
                builder.AppendLine("class {0}Agent : {1}", type.Name, cbName);
                builder.AppendBlock(() =>
                {
                    // build method static class
                    foreach (var method in callback)
                    {
                        builder.AppendLine("public static class _{0}", method.Name);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("public static Action GlobalCallback;");
                            builder.AppendLine("public static Action Callback;");
                            var parameters = method.GetParameters();
                            foreach (var item in parameters)
                                builder.AppendLine("public static {0} {1};", item.ParameterType.CodeName(), item.Name);
                        });
                    }
                    builder.AppendLine();

                    // build implement
                    foreach (var method in callback)
                    {
                        builder.Append("void {0}.{1}", cbName, method.Name);
                        builder.AppendMethodParametersWithBracket(method);
                        builder.AppendBlock(() =>
                        {
                            var parameters = method.GetParameters();
                            foreach (var item in parameters)
                                builder.AppendLine("_{0}.{1} = {1};", method.Name, item.Name);
                            builder.AppendMethodInvoke(method, null, "__" + method.Name, null, null);
                            builder.AppendLine("if (_{0}.GlobalCallback != null) _{0}.GlobalCallback();", method.Name);
                            builder.AppendLine("if (_{0}.Callback != null) _{0}.Callback();", method.Name);
                        });
                    }
                    foreach (var method in callback)
                    {
                        builder.Append("protected virtual void __{0}(", method.Name);
                        builder.AppendMethodParametersOnly(method);
                        builder.AppendLine("){ }");
                    }

                    // reset all local event
                    builder.AppendLine("public static void ResetAllCallback()");
                    builder.AppendBlock(() =>
                    {
                        foreach (var method in callback)
                        {
                            builder.AppendLine("_{0}.Callback = null;", method.Name);
                        }
                    });
                });
            }
        }

		private static void Sleep(int ms)
		{
			System.Threading.Thread.Sleep(ms);
		}
        // 鼠标
		[DllImport("user32")]
		private static extern IntPtr WindowFromPoint(Point Point);
		[DllImport("user32")]
		private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
		[DllImport("user32")]
		private static extern int GetWindowRect(IntPtr hwnd, ref Rectangle lpRect);
		private static void LeftClick(int x, int y)
		{
			Cursor.Position = new Point(x, y);
			mouse_event(0x0002 | 0x0004, x, y, 0, 0);
		}
        private static void LeftTap(int x, int y)
        {
            Cursor.Position = new Point(x, y);
            mouse_event(0x0002 | 0x0004, x, y, 0, 0);
            Thread.Sleep(100);
            mouse_event(0x0002 | 0x0004, x, y, 0, 0);
        }
        // 键盘
        [DllImport("user32.dll")]
        private static extern short GetKeyState(Keys nVirtKey);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private static void DestructSelf(string overdueTime, StringBuilder builder)
        {
            if (!string.IsNullOrEmpty(overdueTime))
            {
                builder.AppendLine("if (DateTime.Now.Ticks >= {0}L)", DateTime.Parse(overdueTime).Ticks);
                builder.AppendBlock(() =>
                {
                    // 自毁程序
                    builder.AppendLine("const string DESTRUCT = \"destruct.bat\";");
                    builder.AppendLine("System.IO.File.WriteAllText(DESTRUCT,");
                    builder.AppendLine("string.Format(\"taskkill /PID {0}\\r\\ndel {1}\\r\\ndel {2}\", System.Diagnostics.Process.GetCurrentProcess().Id, System.Reflection.Assembly.GetExecutingAssembly().Location, DESTRUCT));");
                    builder.AppendLine("System.Diagnostics.Process.Start(DESTRUCT);");
                    builder.AppendLine("return;");
                });
            }
        }

        
        private const string EXCEL_REVERISION = "12.0";
		private static void WriteCSVTable(string file, StringTable table)
		{
			CSVWriter.WriteTable(table, file);
		}
        class NamedStringTable : StringTable
        {
            public string Name;
        }
		private static StringTable ReadCSVTable(string file, int rows = -1)
		{
			return new CSVReader(File.ReadAllText(file, CSVWriter.CSVEncoding)).ReadTable(rows);
		}
        private static Microsoft.Office.Interop.Excel.Application excel;
        private static Process[] statupProcess;
        private static void QuitExcel()
        {
            if (excel == null)
                return;
            var _excel = excel;
            excel = null;
            _excel.Workbooks.Close();
            _excel.Quit();
            if (statupProcess != null)
            {
                var processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    if (statupProcess.Any(p => p.Id == process.Id))
                        continue;
                    // 多出来的Excel进程就杀掉
                    if (process.ProcessName == "EXCEL")
                        process.Kill();
                }
                statupProcess = null;
            }
        }
        private static List<NamedStringTable> LoadTablesFromExcel(string file)
        {
            if (excel == null)
            {
                statupProcess = Process.GetProcesses();
                excel = new Microsoft.Office.Interop.Excel.Application();
                //excel.Visible = false;
                //excel.DisplayAlerts = false;
            }
            List<NamedStringTable> results = new List<NamedStringTable>();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            try
            {
                workbook = excel.Workbooks.Open(file);
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in workbook.Sheets)
                {
                    string tableName = sheet.Name;
                    if (tableName.StartsWith("_"))
                        continue;

                    object[,] cells = (object[,])sheet.UsedRange.Value;
                    if (cells == null)
                        continue;
                    int columns = cells.GetLength(1);
                    if (columns == 0)
                        continue;

                    NamedStringTable table = new NamedStringTable();
                    if (tableName.EndsWith("$"))
                        tableName = tableName.Substring(0, tableName.Length - 1);
                    table.Name = tableName;

                    for (int i = 1; i <= columns; i++)
                        table.AddColumn(cells[1, i].ToString());
                    for (int j = 2, rows = cells.GetLength(0); j <= rows; j++)
                    {
                        bool isEmptyRow = true;
                        for (int i = 1; i <= columns; i++)
                        {
                            if (cells[j, i] != null)
                            {
                                isEmptyRow = false;
                                break;
                            }
                        }
                        if (isEmptyRow)
                            continue;
                        for (int i = 1; i <= columns; i++)
                        {
                            if (cells[j, i] == null)
                                table.AddValue(string.Empty);
                            else
                                table.AddValue(cells[j, i].ToString());
                        }
                    }

                    results.Add(table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
                //excel.Workbooks.Close();
            }
            return results;
        }
        private static List<NamedStringTable> LoadTablesFromExcelOleDb(string file, string version = "15.0")
        {
            List<NamedStringTable> results = new List<NamedStringTable>();
            string connString = string.Format("Provider=Microsoft.Ace.OleDb.{1};data source={0};Extended Properties='Excel 12.0; HDR=NO; IMEX=1'", file, version);
            OleDbConnection conn = new OleDbConnection(connString);
            try
            {
                conn.Open();

                var tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                int tableCount = tables.Rows.Count;
                for (int j = 0; j < tableCount; j++)
                {
                    string tableName = tables.Rows[j].ItemArray[2].ToString();
                    if (tableName.StartsWith("_") || tableName.StartsWith("#"))
                        continue;
                    NamedStringTable table = new NamedStringTable();
                    OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tableName), conn);
                    if (tableName.EndsWith("$"))
                        tableName = tableName.Substring(0, tableName.Length - 1);
                    table.Name = tableName;
                    OleDbDataReader reader = cmd.ExecuteReader();
                    int count = reader.FieldCount;
                    //if (readTitle)
                    //{
                    if (reader.Read())
                    {
                        for (int i = 0; i < count; i++)
                        {
                            table.AddColumn(reader[i].ToString());
                        }
                    }
                    else
                    {
                        reader.Close();
                        //results.Add(table);
                        continue;
                    }
                    //}
                    //else
                    //{
                    //    for (int i = 1; i <= count; i++)
                    //    {
                    //        table.AddColumn("Column" + i);
                    //    }
                    //}
                    //reader.Read();
                    //reader.Read();
                    while (reader.Read())
                    {
                        bool dbnull = true;
                        for (int i = 0; i < count; i++)
                        {
                            if (reader[i] != DBNull.Value)
                            {
                                dbnull = false;
                                break;
                            }
                        }
                        if (dbnull)
                            continue;

                        for (int i = 0; i < count; i++)
                        {
                            if (reader[i] == DBNull.Value)
                            {
                                table.AddValue(string.Empty);
                            }
                            else
                            {
                                // 字符数超过255个会被截掉
                                table.AddValue(reader[i].ToString());
                            }
                        }
                    }

                    results.Add(table);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return results;
        }
		private static StringTable LoadTableFromExcel(string file, string version = "15.0")
		{
            return LoadTablesFromExcelOleDb(file, version)[0];
		}
		private static List<int> Translate(StringTable language, StringTable table, string tableSource)
		{
			List<int> newRowIndex = new List<int>();
            //string[][] sources = language.GetColumns(1).Select(s => JsonReader.Deserialize<string[]>(s)).ToArray();
            string[][] sources = language.GetColumns(1).Select(s => s.Split('|')).ToArray();
			int lastID = 1 + (language.RowCount == 0 ? 0 : Convert.ToInt32(language[0, language.RowCount - 1]));

            if (table.RowCount > 0)
            {
                for (int i = 0; i < table.ColumnCount; i++)
                {
                    if (table[i, 0] == "String")
                    {
                        for (int j = 2; j < table.RowCount; j++)
                        {
                            string key = table[i, j];
                            if (string.IsNullOrEmpty(key))
                                continue;
                            int index = language.GetRowIndex(2, key);
                            if (index == -1)
                            {
                                // 新增翻译
                                index = language.RowCount;
                                //language.AddRow(lastID.ToString(), JsonWriter.Serialize(new string[] { tableSource }), key);
                                language.AddRow(lastID.ToString(), tableSource, key);
                                newRowIndex.Add(index);
                                lastID++;
                            }
                            else
                            {
                                if (!newRowIndex.Contains(index))
                                {
                                    // 新增/不变引用
                                    if (sources[index] != null && !sources[index].Contains(tableSource))
                                        //language[1, index] = JsonWriter.Serialize(sources[index].Add(tableSource));
                                        language[1, index] += string.Format("|{0}", tableSource);
                                    sources[index] = null;
                                }
                            }
                            // 表里需要翻译的内容替换成ID
                            table[i, j] = language[0, index];
                        }
                    }
                }
            }

			// 删除没有了的引用
            //for (int i = 0; i < sources.Length; i++)
            for (int i = sources.Length - 1; i >= 0; i--)
			{
				if (sources[i] != null)
				{
					int index = sources[i].IndexOf(tableSource);
					if (index != -1)
					{
						sources[i] = sources[i].Remove(index);
						if (sources[i].Length == 0)
						{
							// 删除已经没有引用的翻译
							language.RemoveRow(i);
						}
						else
						{
                            //language[1, i] = JsonWriter.Serialize(sources[i]);
                            language[1, i] = string.Join("|", sources[i]);
						}
					}
				}
			}

			return newRowIndex;
		}
		private static StringTable ReadTranslateTable(string translate)
		{
			StringTable result;
			if (File.Exists(translate))
			{
				result = ReadCSVTable(translate);
			}
			else
			{
				result = StringTable.DefaultLanguageTable();
			}
			return result;
		}
		private static string[] GetCSVTables(string dir, SearchOption option = SearchOption.AllDirectories)
		{
			const string Language = "language.csv";
			var files = GetFiles(dir, "*.csv", option);
			files = files.Where(f => !f.ToLower().EndsWith(Language)).ToArray();
			return files;
		}
        private class SpecialField
        {
            public string Name;
            public string Type;
            public char Seperator;

            public override string ToString()
            {
                return string.Format("{0} {1}", Type, Name);
            }
        }
        private class SpecialType
        {
            public string EnumUnderlyingType;
            public bool NeedBuildType;
            public string TypeName;
            public byte BuildDictionary;
            public char FieldSeperator;
            public List<SpecialField> Fields;
            public char ArraySeperator1;
            public char ArraySeperator2;

            public bool IsEnum
            {
                get { return EnumUnderlyingType != null; }
            }
            public bool IsArray1
            {
                get { return ArraySeperator1 != default(char); }
            }
            public bool IsArray2
            {
                get { return ArraySeperator2 != default(char); }
            }
            public bool IsDictionary
            {
                get { return BuildDictionary != 0; }
            }
            public bool IsDictionaryGroup
            {
                get { return BuildDictionary == 2; }
            }
        }
        private static SpecialType ParseSpecialType(string type)
        {
            if (!type.Contains('['))
                return null;

            /*
             * ushort[] 生成Dictionary<ushort, 类型>的字典
             * string#Item[ID(ushort),Count(uint)][|] '#'后需要生成Item类型
             * string:COLOR[R(byte),G(byte),B(byte),A(byte)] ':'后不需要生成类型
             * string:Range<float>[Min(float)~Max(float)] 可用于泛型
             * ushort[,] 解析成用','分割的ushort[]
             * string[Min(float)~Max(float)] 生成字段名的类型
             * ushort[,][#] 生成ushort[][]
             * string:COLOR[R(byte),G(byte),B(byte),A(byte)][#] 生成特殊类型的数组
             * string#Item[ID(ushort),Count(uint)][|][#] 特殊类型的数组最多支持二维数组，且不会重复生成Item类型
             * enum#EType:byte[A=0,B=1,C,D...] 生成类型的内部枚举
             * 以上均可复合使用
             */
            SpecialType special = new SpecialType();

            StringStreamReader reader = new StringStreamReader(type);
            string temp = reader.PeekNext("[");

            if (temp.Contains("#"))
            {
                special.NeedBuildType = true;
                reader.Next("#");
                reader.Read();
                special.TypeName = reader.Next("[");
                if (temp.StartsWith("enum"))
                {
                    special.EnumUnderlyingType = string.Empty;
                    int underlyingIndex = special.TypeName.IndexOf(':');
                    if (underlyingIndex != -1)
                    {
                        special.EnumUnderlyingType = special.TypeName.Substring(underlyingIndex + 1);
                        special.TypeName = special.TypeName.Substring(0, underlyingIndex);
                    }
                    special.Fields = new List<SpecialField>();
                    // eat the '['
                    reader.Read();
                    while (!reader.IsEnd)
                    {
                        string read = reader.Next(",]").Trim();
                        char next = reader.Read();
                        if (string.IsNullOrEmpty(read))
                            if (next == ']')
                                break;
                            else
                                continue;
                        SpecialField specialField = new SpecialField();
                        specialField.Name = read;
                        special.Fields.Add(specialField);
                        if (next == ']')
                            break;
                    }
                    if (reader.IsEnd)
                        return special;
                }
            }
            else if (temp.Contains(':'))
            {
                special.NeedBuildType = false;
                reader.Next(":");
                reader.Read();
                special.TypeName = reader.Next("[");
            }
            else
            {
                reader.Next("[");

                if (temp != "string")
                {
                    special.NeedBuildType = false;
                    special.TypeName = temp;
                }
                else
                {
                    special.NeedBuildType = true;
                }
            }
            
            temp = reader.PeekNext("]");
            if (temp.Length == 1)
            {
                // string数组
                if (special.NeedBuildType)
                {
                    special.TypeName = "string";
                    special.NeedBuildType = false;
                }
                special.BuildDictionary = 1;
                return special;
            }
            else if (temp.Length == 2)
            {
                // [[]]代表分组的字典
                if (reader.Tail == "[[]]")
                {
                    special.BuildDictionary = 2;
                    reader.ReadLine();
                }
            }
            else
                //if (temp.Length != 2)
            {
                // eat the '['
                reader.Read();
                special.Fields = new List<SpecialField>();
                while (true)
                {
                    SpecialField specialField = new SpecialField();
                    specialField.Name = reader.Next("(");
                    reader.Read();
                    specialField.Type = reader.Next(")");
                    reader.Read();
                    specialField.Seperator = reader.Read();
                    special.Fields.Add(specialField);
                    if (specialField.Seperator == ']')
                        break;
                    else
                        special.FieldSeperator = specialField.Seperator;
                }
            }

            // 最多二维数组
            if (!reader.IsEnd)
            {
                reader.Read();
                special.ArraySeperator1 = reader.Read();
                reader.Read();
            }
            if (!reader.IsEnd)
            {
                reader.Read();
                special.ArraySeperator2 = reader.Read();
                reader.Read();
            }

            return special;
        }

		private const string IMAGE_FORMAT = "*.png";
		private static string[] IMAGE_FORMATS =
        {
            "*.png",
            "*.jpg",
            "*.jpeg",
            "*.bmp",
        };
		private static Bitmap OpenBitmap(string file)
		{
			return (Bitmap)Bitmap.FromFile(file);
		}
		private static BitmapData LockBits(Bitmap texture)
		{
			return LockBits(texture, ImageLockMode.ReadOnly);
		}
		private static BitmapData LockBits(Bitmap texture, ImageLockMode mode)
		{
			return LockBits(texture, new Rectangle(0, 0, texture.Width, texture.Height), mode);
		}
		private static BitmapData LockBits(Bitmap texture, Rectangle rect, ImageLockMode mode)
		{
			return texture.LockBits(rect, mode, PixelFormat.Format32bppArgb);
		}
		private static byte[] GetData(Bitmap texture)
		{
			return GetData(texture, new Rectangle(0, 0, texture.Width, texture.Height));
		}
        private delegate bool DTextureColor(ref byte b, ref byte g, ref byte r, ref byte a);
        private static void LockBits(Bitmap texture, DTextureColor action)
        {
            if (action == null)
                return;

            var data = LockBits(texture, ImageLockMode.ReadWrite);

            int length = texture.Width * texture.Height * 4;
            byte[] buffer = new byte[length];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            bool changed = false;
            for (int i = 0; i < length; i += 4)
                if (action(ref buffer[i], ref buffer[i + 1], ref buffer[i + 2], ref buffer[i + 3]))
                    changed = true;

            if (changed)
                Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

            texture.UnlockBits(data);
        }
        private static void LockBits(Bitmap texture, Action<byte[]> action)
        {
            if (action == null)
                return;

            var data = LockBits(texture, ImageLockMode.ReadWrite);

            byte[] buffer = new byte[texture.Width * texture.Height * 4];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            action(buffer);

            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

            texture.UnlockBits(data);
        }
		private static byte[] GetData(Bitmap texture, Rectangle rect)
		{
			byte[] buffer = new byte[texture.Width * texture.Height * 4];
			var data = LockBits(texture);
			Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
			texture.UnlockBits(data);

			if (rect.X == 0 && rect.Y == 0 && rect.Width == texture.Width && rect.Height == texture.Height)
				return buffer;
			else
				return buffer.GetArray(rect.X * 4, rect.Y, rect.Width * 4, rect.Height, texture.Width * 4);
		}
        private static void BitsAlignCenter(byte[] data1, int width1, int height1, byte[] data2, int width2, int height2, Action<int, int> action)
        {
            // 两张图片像素中心对齐
            int cw1 = width1 / 2;
            int ch1 = height1 / 2;
            int cw2 = width2 / 2;
            int ch2 = height2 / 2;

            int width = Math.Min(width1, width2);
            int height = Math.Min(height1, height2);
            int minW = Math.Min(cw1, cw2);
            int minH = Math.Min(ch1, ch2);
            int offset1 = (ch1 - minH) * width1 * 4;
            int offset2 = (ch2 - minH) * width2 * 4;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0,
                    i1 = offset1 + (cw1 - minW) * 4,
                    i2 = offset2 + (cw2 - minW) * 4;

                    i < width;

                    i++, i1 += 4, i2 += 4)
                {
                    action(i1, i2);
                }
                offset1 += width1 * 4;
                offset2 += width2 * 4;
            }
        }
        private static void ImageDraw(Image texture, Action<Graphics> action)
		{
			using (Graphics graphics = Graphics.FromImage(texture))
				action(graphics);
		}
        private static void Cut(IList<Bitmap> textures, Point normalizeSize)
        {
            int minX, minY, maxX, maxY;
            Cut(textures, normalizeSize, out minX, out minY, out maxX, out maxY);
        }
        private static void Cut(IList<Bitmap> textures, Point normalizeSize, out int minX, out int minY, out int maxX, out int maxY)
        {
            // 附加效果
            // 如果所有图片的尺寸有不一样
            // 先将图片的尺寸统一到一个最大尺寸
            // 然后再进行裁切
            // 此时有些原本小的图片也被同一化到新的尺寸
            if (textures.Count > 1)
            {
                bool exists = false;
                int width = textures[0].Width;
                int height = textures[0].Height;
                for (int i = 1; i < textures.Count; i++)
                {
                    Bitmap texture = textures[i];

                    if (texture.Width != width ||
                        texture.Height != height)
                    {
                        if (texture.Width > width)
                            width = texture.Width;

                        if (texture.Height > height)
                            height = texture.Height;

                        exists = true;
                    }
                }

                if (exists)
                {
                    // 尺寸标准化
                    for (int i = 0; i < textures.Count; i++)
                    {
                        Bitmap source = textures[i];
                        if (source.Width != width ||
                            source.Height != height)
                        {
                            using (source)
                            {
                                Bitmap texture = new Bitmap(width, height, Graphics.FromImage(source));
                                ImageDraw(texture, graphics =>
                                {
                                    graphics.DrawImage(source, new Rectangle(
                                        (width - source.Width) / 2,
                                        (height - source.Height) / 2,
                                        source.Width, source.Height));
                                });
                                textures[i] = texture;
                            }
                        }
                    }
                }
            }

            // 循环所有图片的所有像素点
            // 分别找出4个边界的临界值
            // 根据这个临界值裁切图像
            List<COLOR[]> colors = new List<COLOR[]>(textures.Count);

            for (int i = 0; i < textures.Count; i++)
            {
                var buffer = GetData(textures[i]);
                int count = buffer.Length;
                COLOR[] color = new COLOR[count / 4];
                for (int j = 0; j < count; j += 4)
                {
                    COLOR temp;
                    temp.R = buffer[j];
                    temp.G = buffer[j + 1];
                    temp.B = buffer[j + 2];
                    temp.A = buffer[j + 3];
                    color[j / 4] = temp;
                }
                colors.Add(color);
            }

            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = 0;
            maxY = 0;
            for (int i = 0; i < colors.Count; i++)
            {
                COLOR[] data = colors[i];
                int width = textures[i].Width;
                int height = textures[i].Height;

                // 最左像素
                for (int x = 0; x < width; x++)
                {
                    if (x >= minX) break;

                    for (int y = 0; y < height; y++)
                    {
                        int index = y * width + x;
                        if (data[index].A != 0)
                        {
                            minX = x;
                            x = width;
                            break;
                        }
                    }
                }

                // 最上像素
                for (int y = 0; y < height; y++)
                {
                    if (y >= minY) break;

                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        if (data[index].A != 0)
                        {
                            minY = y;
                            y = height;
                            break;
                        }
                    }
                }

                // 最右像素
                for (int x = width - 1; x >= 0; x--)
                {
                    if (x <= maxX) break;

                    for (int y = 0; y < height; y++)
                    {
                        int index = y * width + x;
                        if (data[index].A != 0)
                        {
                            maxX = x;
                            x = 0;
                            break;
                        }
                    }
                }

                // 最下像素
                for (int y = height - 1; y >= 0; y--)
                {
                    if (y <= maxY) break;

                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        if (data[index].A != 0)
                        {
                            maxY = y;
                            y = 0;
                            break;
                        }
                    }
                }
            }

            Rectangle areaOfPixel = new Rectangle(
                minX, minY,
                maxX - minX,
                maxY - minY);

            Rectangle desc = new Rectangle(
                0, 0,
                areaOfPixel.Width,
                areaOfPixel.Height);

            if (normalizeSize.X > 0 && normalizeSize.Y > 0)
            {
                // 标准化到同一尺寸
                //border.X = _MATH.Nature(normalizeSize.X - areaOfPixel.Width);
                //border.Y = _MATH.Nature(normalizeSize.Y - areaOfPixel.Height);
                int borderX = normalizeSize.X - areaOfPixel.Width;
                int borderY = normalizeSize.Y - areaOfPixel.Height;
                desc.Width += borderX;
                desc.Height += borderY;
            }

            for (int i = 0; i < textures.Count; i++)
            {
                using (Bitmap source = textures[i])
                {
                    Bitmap texture = new Bitmap(desc.Width, desc.Height, Graphics.FromImage(source));
                    ImageDraw(texture, graphics =>
                    {
                        graphics.DrawImage(source,
                                new RectangleF(desc.Width * 0.5f - (areaOfPixel.X + areaOfPixel.Width * 0.5f),
                                    desc.Height * 0.5f - (areaOfPixel.Y + areaOfPixel.Height * 0.5f),
                                    source.Width, source.Height));
                    });
                    textures[i] = texture;
                }
            }
        }
		private static Rectangle[] Put(Point[] sizes, Point size, bool widthPriority)
		{
			Rectangle[] results;
			if (!widthPriority)
			{
				size = new Point(size.Y, size.X);
				results = sizes.Select(s => new Rectangle(0, 0, s.Y, s.X)).ToArray();
			}
			else
				results = sizes.Select(s => new Rectangle(0, 0, s.X, s.Y)).ToArray();

			Stack<Rectangle> remaind = new Stack<Rectangle>();
			remaind.Push(new Rectangle(0, 0, size.X, size.Y));

			HashSet<int> set = new HashSet<int>();

			int max;
			int index;
			Rectangle current;
			int count = results.Length;
			while (count > 0)
			{
				// has no space
				if (remaind.Count == 0)
					return null;

				max = 0;
				index = -1;
				current = remaind.Pop();

				for (int i = 0; i < results.Length; i++)
				{
					var item = results[i];

					// has set
					if (set.Contains(i))
						continue;

					// 放得下
					if (item.Width <= current.Width && item.Height <= current.Height)
					{
						// 最合适
						if (widthPriority)
						{
							if (item.Width > max)
							{
								index = i;
								max = item.Width;
							}
						}
						else
						{
							if (item.Height > max)
							{
								index = i;
								max = item.Height;
							}
						}
					}
				}

				// current没有能放下的方块
				if (index == -1)
					continue;

				// 放置
				results[index].X = current.X;
				results[index].Y = current.Y;

				// 切割剩余矩形
				Rectangle temp = results[index];
				Rectangle split;

				split = new Rectangle(temp.Right, current.Y, current.Width - temp.Width, current.Height);
				if (split.Width > 0 && split.Height > 0)
					remaind.Push(split);

				split = new Rectangle(current.X, temp.Bottom, temp.Width, current.Height - temp.Height);
				if (split.Width > 0 && split.Height > 0)
					remaind.Push(split);

				count--;
				set.Add(index);
			}
			return results;
		}
		private static string SavePng(Image texture, string dir, string file, bool disposed = true)
		{
			return SaveTexture(texture, dir, Path.ChangeExtension(file, "png"), disposed);
		}
		private static string SaveTexture(Image texture, string dir, string file, bool disposed)
		{
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			string output = string.Format("{0}{1}", dir, Path.GetFileName(file));
			//texture.Save(output, texture.RawFormat);
			string suffix = Path.GetExtension(file).ToLower();
			ImageFormat format;
			switch (suffix)
			{
				case "jpg":
				case "jpeg":
					format = ImageFormat.Jpeg;
					break;

				case "bmp":
					format = ImageFormat.Bmp;
					break;

				default:
					format = ImageFormat.Png;
					break;
			}
			texture.Save(output, format);
			if (disposed)
				texture.Dispose();
			return output;
		}
		private static bool ClearMemory()
		{
			// 512M清理内存
			return ClearMemory(1024 * 1024 * 256);
		}
		private static bool ClearMemory(long bytes)
		{
			long memory = GC.GetTotalMemory(false);
			if (memory >= bytes)
			{
				Console.WriteLine("Memory[{0}] too much, will clear.", memory);
				GC.Collect();
				var status = GC.WaitForFullGCComplete(-1);
				Console.WriteLine("GC completed. Status: {0} Memory: {1}", status, GC.GetTotalMemory(false));
				return true;
			}
			return false;
		}
        private class Piece
        {
            [ASummary("游戏内加载内容的根目录，使用原始根目录时不填，下面的Directories和OutputFile将自动包含此根目录路径")]
            public string Root;
            [ASummary("要组合的原始图所在目录，多个目录用','隔开")]
            public string Directories;
            [ASummary("是否包含目录内的所有子目录的文件")]
            public bool All;
            [ASummary("是否裁剪图片的空白区域（裁剪后Texture.Size不会变）")]
            public bool Cut;
            [ASummary("合并的大图尺寸是否采用2的次方")]
            public bool Power2;
            [ASummary("输出后的大图路径，输出文件可带后缀，不带后缀默认为t2d，不填则在图片目录并于首个目录同名；'/'结尾则视为目录，不组合大图（Power2将失效），主要作为仅裁切图片时使用")]
            public string Output;
            [ASummary("图片组合后的信息文件名，不带后缀，可以重复")]
            public string Metadata;

            public bool Combine
            {
                get { return !Output.EndsWith("/") && !Output.EndsWith("\\"); }
            }
        }
		private class Patch
		{
			[ASummary("九宫格左锚点")]
			public ushort Left;
			[ASummary("九宫格上锚点")]
			public ushort Top;
			[ASummary("九宫格右锚点")]
			public ushort Right;
			[ASummary("九宫格下锚点")]
			public ushort Bottom;
			[ASummary("原图片路径，不带后缀则使用8x8纯白图")]
			public string Source;
			[ASummary("中间部分颜色，格式为：R,G,B,A，不填由程序指定")]
			public string ColorBody;
			[ASummary("边框部分颜色，格式为：R,G,B,A，不填由程序指定")]
			public string ColorBorder;

			public static COLOR GetColor(string str)
			{
                if (string.IsNullOrEmpty(str))
                    return PATCH.NullColor;

				string[] rgba = str.Split(',');
				if (rgba.Length != 4)
					throw new ArgumentException("错误的颜色格式");

				return new COLOR(
					byte.Parse(rgba[0]),
					byte.Parse(rgba[1]),
					byte.Parse(rgba[2]),
					byte.Parse(rgba[3]));
			}
		}
		private class Animation
		{
			[ASummary("序列动画名字")]
			public string Name;
			[ASummary("循环次数")]
			public short Loop;
			[ASummary("跳转目标动画名字，若有循环次数，则在循环次数达到后跳转，否则永不跳转")]
			public string Next;
			[ASummary("帧持续秒")]
			public float Interval;
			[ASummary("序列帧图片所在目录")]
			public string Directory;
			[ASummary("输出目标文件名(不带后缀)，多个同样输出的动画将组合在一起，若不填则在图片目录并与目录同名")]
			public string Output;
            [ASummary("动画尺寸一样时可以预先设定支点的X坐标（单位尺寸百分比）")]
            public float PivotX;
            [ASummary("动画尺寸一样时可以预先设定支点的Y坐标（单位尺寸百分比）")]
            public float PivotY;
		}
        private class Pixel : Graph<Pixel>, IEquatable<Pixel>
        {
            public byte Alpha;
            public ushort X;
            public ushort Y;
            public override int GetHashCode()
            {
                return (X << 16) | Y;
            }
            public bool Equals(Pixel other)
            {
                return this.X == other.X && this.Y == other.Y;
            }
        }
        private class ScanLine
        {
            public int Min;
            public int Max;
            public override string ToString()
            {
                return string.Format("{0} - {1}", Min, Max);
            }
        }
        private class SplitArea
        {
            public Rectangle Area;
            public Dictionary<int, ScanLine> Lines = new Dictionary<int, ScanLine>();
            //public bool Tested(int x, int y)
            //{
            //    ScanLine line;
            //    if (Lines.TryGetValue(y, out line))
            //    {
            //        return x >= line.Min && x <= line.Max;
            //    }
            //    return false;
            //}
            public void SetPos(int x, int y)
            {
                ScanLine line;
                if (!Lines.TryGetValue(y, out line))
                {
                    line = new ScanLine();
                    Lines.Add(y, line);
                    line.Min = x;
                    line.Max = x;
                }
                else
                {
                    line.Min = Math.Min(x, line.Min);
                    line.Max = Math.Max(x, line.Max);
                }
            }
            public void Resolve()
            {
                int minY = -1;
                int maxY = -1;
                int minX = -1;
                int maxX = -1;
                Resolve(out minX, out minY, out maxX, out maxY);
            }
            public void Resolve(out int minX, out int minY, out int maxX, out int maxY)
            {
                minY = -1;
                maxY = -1;
                minX = -1;
                maxX = -1;
                bool flag = true;
                foreach (var item in Lines)
                {
                    if (flag)
                    {
                        flag = false;
                        minY = item.Key;
                        maxY = item.Key;
                        minX = item.Value.Min;
                        maxX = item.Value.Max;
                    }
                    else
                    {
                        minY = Math.Min(item.Key, minY);
                        maxY = Math.Max(item.Key, maxY);
                        minX = Math.Min(item.Value.Min, minX);
                        maxX = Math.Max(item.Value.Max, maxX);
                    }
                }
                Area.X = minX;
                Area.Y = minY;
                Area.Width = maxX - minX + 1;
                Area.Height = maxY - minY + 1;
            }
        }
        private class CutArea
        {
            public Bitmap Texture;
            public RECT Padding;
        }

		public static void BuildEntryEngine(string outputDir)
		{
			var entryType = typeof(Entry);
			var devices = entryType.Assembly.GetTypesWithAttribute<ADevice>(false).ToArray();
            devices.SortOrder((p1, p2) => p1.Name.CompareTo(p2.Name) > 0);

			StringBuilder builder = new StringBuilder();

			// using
			HashSet<string> ns = new HashSet<string>();
			builder.AppendLine("#if CLIENT");
			builder.AppendLine();
			AppendDefaultNamespace(ns, builder);
			ns.Add(entryType.Namespace);
			foreach (var device in devices)
				if (ns.Add(device.Namespace))
					builder.AppendLine("using {0};", device.Namespace);
			builder.AppendLine();

			// namespace
			builder.AppendLine("namespace EntryEngine");
			builder.AppendBlock(() =>
			{
				builder.AppendLine("partial class {0}", entryType.Name);
				builder.AppendBlock(() =>
				{
					foreach (var device in devices)
					{
						string deviceType = device.CodeName();
						string deviceName = device.Name;

						var da = device.GetAttribute<ADevice>();
						var dva = device.GetAttribute<ADefaultValue>();
						var constructors = device.GetConstructors().Where(c => c.HasAttribute<ADeviceNew>()).ToArray();
						bool unique = constructors.Length == 0;
						bool foreignKey = !string.IsNullOrEmpty(da.StaticInstance);

						builder.AppendLine("public static {0} _{1}", deviceType, deviceName);
						if (foreignKey)
						{
							builder.AppendBlock(() =>
							{
								builder.AppendLine("get {{ return {0}; }}", da.StaticInstance);
								builder.AppendLine("set {{ {0} = value; }}", da.StaticInstance);
							});
						}
						else
						{
							builder.AppendBlock(() =>
							{
								builder.AppendLine("get;");
								builder.AppendLine("set;");
							});
						}
						builder.AppendLine("public {0} {1} {{ get {{ return _{1}; }} }}", deviceType, deviceName);

                        if (constructors.Length == 0)
                            continue;
                        builder.AppendLine("public event Action<{0}> OnNew{1};", deviceType, deviceName);
						for (int i = 0; i < constructors.Length; i++)
						{
							var constructor = constructors[i];
							var att = constructor.GetAttribute<ADeviceNew>();
							var parameters = constructor.GetParameters();

                            builder.Append("public {0} New{1}", deviceType, deviceName);
                            builder.AppendMethodParametersWithBracket(constructor);
                            builder.AppendBlock(() =>
                            {
                                builder.Append("var __device = ");
                                builder.AppendMethodInvoke(constructor, "", "InternalNew" + deviceName, "", "");
                                builder.AppendLine("if (OnNew{0} != null) OnNew{0}(__device);", deviceName);
                                builder.AppendLine("return __device;");
                            });
							builder.Append("protected virtual {0} InternalNew{1}", deviceType, deviceName);
							builder.AppendMethodParametersWithBracket(constructor);
							builder.AppendBlock(() =>
							{
								builder.AppendSharpIfThrow(() =>
								{
									bool flag = false;
									if (att.DefaultValue != null)
									{
										if (!att.DefaultValue.Is(device))
											throw new ArgumentNullException("DefaultValue", string.Format("Type \"{0}\" is not the correct device type.", att.DefaultValue.Name));
										var defaultConstructor = att.DefaultValue.GetConstructor(parameters.Select(p => p.ParameterType).ToArray());
										if (defaultConstructor == null)
											throw new ArgumentNullException("DefaultValue", string.Format("Type \"{0}\" don't have matched constructor.", att.DefaultValue.Name));
										builder.Append("return new {0}", att.DefaultValue.CodeName());
										builder.AppendMethodInvoke(constructor);
										flag = true;
									}
									else if (dva != null)
									{
										var defaultConstructor = dva.DefaultValue.GetConstructor(parameters.Select(p => p.ParameterType).ToArray());
										if (defaultConstructor != null)
										{
											builder.Append("return new {0}", dva.DefaultValue.CodeName());
											builder.AppendMethodInvoke(constructor);
											flag = true;
										}
									}
									if (!flag)
										builder.AppendLine("throw new System.NotImplementedException();");
								});
							});
						}
					}

					// build initialize
					builder.AppendLine();
					builder.AppendLine("public void Initialize()");
					builder.AppendBlock(() =>
					{
						foreach (var device in devices)
							builder.AppendLine("{0} {1};", device.CodeName(), device.Name);
						builder.Append("Initialize(");
						Utility.ForeachExceptLast(devices,
							device => builder.Append("out {0}", device.Name),
							device => builder.Append(", "));
						builder.AppendLine(");");
						foreach (var device in devices)
							builder.AppendLine("if ({0} != null) _{0} = {0};", device.Name);
                        builder.AppendLine("OnInitialized();");
					});

					builder.AppendLine("protected abstract void Initialize(");
					Utility.ForeachExceptLast(devices,
							device => builder.Append("out {0} {1}", device.CodeName(), device.Name),
							device => builder.AppendLine(","));
					builder.AppendLine(");");
				});
			});
			builder.AppendLine();
			builder.AppendLine("#endif");

			SaveCode(Path.Combine(outputDir, "Entry.Implement.cs"), builder);
			Console.WriteLine("build entry device completed!");

			BuildStaticInterface("EntryEngine.dll\\EntryEngine.Utility", Path.Combine(outputDir, "Utility.Implement.cs"), true, "");
			Console.WriteLine("build entry utility completed!");
		}
        public static void BuildDll(string solutionDir, string output, string version, string refferenceDllsSplitBySep, bool dummy, string symbols)
        {
            Project project = new Project();
            project.AddSymbols(symbols);

            string[] codes = Directory.GetFiles(solutionDir, "*.cs", SearchOption.AllDirectories);
            project.ParseFromFile(codes);
            if (string.IsNullOrEmpty(project.Company))
                project.Company = "Yamiwa Studio";
            if (string.IsNullOrEmpty(project.Description))
                project.Description = "Powered by EntryEngine";

            for (int i = 0; i < codes.Length; i++)
            {
                CSharpCodeBuilder writer;
                if (dummy)
                    writer = new CSharpDummyCodeBuilder();
                else
                    writer = new CSharpCodeBuilder();
                writer.Visit(project.Files[i]);
                //File.WriteAllText(Path.Combine(@"D:\Projects\EntryEngine\CompileTest", Path.GetFileName(codes[i])), writer.Result);
                codes[i] = writer.Result;
            }

            CSharpCodeProvider.SetCompileVersion(version);
            CompilerParameters param = new CompilerParameters();
            param.CompilerOptions = "/unsafe /optimize";
            param.ReferencedAssemblies.Add("System.dll");
            param.ReferencedAssemblies.Add("System.Data.dll");
            param.ReferencedAssemblies.Add("System.Core.dll");
            var dlls = Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string dll in dlls)
            {
                if (project.Title != Path.GetFileNameWithoutExtension(dll))
                    param.ReferencedAssemblies.Add(Path.GetFileName(dll));
            }
            if (!string.IsNullOrEmpty(refferenceDllsSplitBySep))
                foreach (var dll in refferenceDllsSplitBySep.Split('|'))
                    param.ReferencedAssemblies.Add(dll);
            param.GenerateExecutable = false;
            param.OutputAssembly = output;
            param.GenerateInMemory = false;
            param.TreatWarningsAsErrors = false;

            string outputAssembly = Path.GetFileName(Path.GetDirectoryName(Path.GetFullPath(param.OutputAssembly)));

            CSharpCodeProvider compiler = new CSharpCodeProvider();
            CompilerResults result = compiler.CompileAssemblyFromSource(param, codes);
            if (result.Errors.Count > 0)
                for (int i = 0; i < result.Errors.Count; i++)
                    Console.WriteLine(result.Errors[i].ErrorText);
            else
                Console.WriteLine("编译{0}成功 output={1}", solutionDir, outputAssembly);
        }
        public static void BuildProtocolAgentBinary(string outputClientPath, string outputServerPath, string dllOrWithType)
        {
            Action<Type> build = (type) =>
            {
                ProtocolDefault writer = new ProtocolDefault();
                writer.Write(type);

                string clientFile = Path.Combine(outputClientPath, type.Name + "Proxy.cs");
                string serverFile = Path.Combine(outputServerPath, type.Name + "Stub.cs");
                SaveCode(clientFile, writer.builder);
                SaveCode(serverFile, writer.server);
                Console.WriteLine("生成协议类型{0}完成", type.FullName);
            };

            // DLL文件将生成所有带
            if (File.Exists(dllOrWithType))
            {
                var assembly = Assembly.LoadFile(dllOrWithType);
                foreach (var type in assembly.GetTypesWithAttribute<ProtocolStubAttribute>())
                    build(type);
            }
            else
            {
                Type type = GetDllType(dllOrWithType);
                if (type == null)
                    throw new ArgumentNullException("type");
                build(type);
            }
        }
        public static void BuildProtocolAgentHttp(string outputServerPathOrCSFile, string dllAndType)
        {
            Type type = GetDllType(dllAndType);
            if (type == null)
                return;

            ProtocolStubAttribute protocol = type.GetAttribute<ProtocolStubAttribute>();

            StringBuilder builder = new StringBuilder();
            AppendDefaultNamespace(builder);
            builder.AppendLine("using System.Net;");
            builder.AppendLine("using EntryEngine;");
            builder.AppendLine("using EntryEngine.Network;");
            builder.AppendLine("using EntryEngine.Serialize;");
            if (!string.IsNullOrEmpty(type.Namespace))
                builder.AppendLine("using {0};", type.Namespace);
            builder.AppendLine();

            MethodInfo[] call = type.GetInterfaceMethods().ToArray();

            #region interface

            builder.AppendLine("interface _{0}", type.Name);
            builder.AppendBlock(() =>
            {
                foreach (var item in call)
                {
                    bool output = item.HasReturnType();
                    if (output)
                    {
                        // 有返回类型则在Stub里自动回调
                        builder.Append("{0} {1}(", item.ReturnType.CodeName(false), item.Name);
                        builder.AppendMethodParametersOnly(item);
                        builder.AppendLine(");");
                    }
                    else
                    {
                        // 没有返回类型追加HttpListenerContext参数手动回调
                        if (item.GetParameters().Length > 0)
                        {
                            builder.Append("void {0}(HttpListenerContext __context, ", item.Name);
                            builder.AppendMethodParametersOnly(item);
                            builder.AppendLine(");");
                        }
                        else
                            builder.AppendLine("void {0}(HttpListenerContext __context);", item.Name);
                    }
                }
            });

            #endregion

            #region stub

            builder.AppendLine("class {0}Stub : {1}", type.Name, typeof(StubHttp).Name);
            builder.AppendBlock(() =>
            {
                builder.AppendLine("public Action<HttpListenerContext, object> __AutoCallback;");
                builder.AppendLine("public _{0} __Agent;", type.Name);
                builder.AppendLine("\tpublic Func<_{0}> __GetAgent;", type.Name);
                builder.AppendLine("public Func<HttpListenerContext, _{0}> __ReadAgent;", type.Name);
                builder.AppendLine("public {0}Stub(_{0} agent)", type.Name);
                if (protocol != null && !string.IsNullOrEmpty(protocol.AgentType))
                    builder.AppendLine(" : base(\"{0}\")", protocol.AgentType);
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("this.Agent = agent;");
                    foreach (var method in call)
                        builder.AppendLine("AddMethod(\"{0}\", {0});", method.Name);
                });

                foreach (var method in call)
                {
                    builder.AppendLine("void {0}(HttpListenerContext __context)", method.Name);
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("var agent = __Agent;");
                        builder.AppendLine("if (__GetAgent != null) agent = __GetAgent();");
                        builder.AppendLine("if (__ReadAgent != null) agent = __ReadAgent(__context);");

                        // 参数
                        var parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            // 解析参数
                            builder.AppendLine("string __temp;");
                            //builder.AppendLine("var __query = __context.Request.QueryString;");
                            foreach (var param in parameters)
                            {
                                //builder.AppendLine("__temp = __query[\"{0}\"];", param.Name);
                                builder.AppendLine("__temp = GetParam(\"{0}\");", param.Name);
                                //if (param.ParameterType == typeof(string))
                                //    builder.AppendLine("{0} {1} = __temp;", param.ParameterType.CodeName(), param.Name);
                                //else
                                builder.AppendLine("{0} {1} = __temp == null ? default({0}) : ({0})Convert.ChangeType(__temp, typeof({0}));", param.ParameterType.CodeName(), param.Name);
                            }
                            // 日志
                            builder.AppendLine("#if DEBUG");
                            builder.Append("_LOG.Debug(\"{0}", method.Name);
                            for (int j = 0, n = parameters.Length - 1; j <= n; j++)
                            {
                                ParameterInfo param = parameters[j];
                                builder.Append(" {0}: {{{1}}}", param.Name, j);
                                if (j != n)
                                    builder.Append(",");
                            }
                            builder.Append("\"");
                            for (int j = 0; j < parameters.Length; j++)
                            {
                                ParameterInfo param = parameters[j];
                                //if (param.ParameterType.Is(dt))
                                //    builder.AppendFormat(", \"{0}\"", param.ParameterType.CodeName());
                                if (!param.ParameterType.IsCustomType())
                                    builder.AppendFormat(", {0}", param.Name);
                                else
                                    builder.AppendFormat(", JsonWriter.Serialize({0})", param.Name);
                            }
                            builder.AppendLine(");");
                            builder.AppendLine("#endif");
                        }
                        //builder.AppendLine("__context.Response.ContentEncoding = CallbackEncoding;");

                        if (method.HasReturnType())
                        {
                            builder.Append("var __result = agent.");
                            builder.AppendMethodInvoke((MethodBase)method);
                            // 自动回调
                            builder.AppendLine("if (__AutoCallback != null) __AutoCallback(__context, __result);");
                            builder.AppendLine("else");
                            builder.AppendBlock(() =>
                            {
                                if (method.ReturnType.IsCustomType())
                                    builder.AppendLine("Response(__result);");
                                else
                                    builder.AppendLine("Response(__result.ToString());");
                            });
                        }
                        else
                        {
                            builder.AppendMethodInvoke(method, "agent", "__context", null);
                        }
                    });
                }
            });

            #endregion

            if (string.IsNullOrEmpty(Path.GetExtension(outputServerPathOrCSFile)))
                outputServerPathOrCSFile = Path.Combine(outputServerPathOrCSFile, type.Name + "Stub.cs");
            File.WriteAllText(outputServerPathOrCSFile, _RH.Indent(builder.ToString()), Encoding.UTF8);
        }
        public static void BuildEncrypt(string dirOrFile, string outputDir)
        {
            if (Path.GetFullPath(dirOrFile) == Path.GetFullPath(outputDir))
            {
                Console.WriteLine("输入目录不能和输出目录一样");
                return;
            }
            BuildDir(ref outputDir);

            IEncrypt[] encrypts = new IEncrypt[]
            {
                new EncryptShuffle(),
                new EncryptInvert(),
            };
            foreach (var file in GetFiles(dirOrFile, SearchOption.AllDirectories, "*.*"))
            {
                Console.WriteLine("加密文件:{0}", file);
                byte[] data = File.ReadAllBytes(file);
                for (int i = 0; i < encrypts.Length; i++)
                    encrypts[i].Encrypt(ref data);
                File.WriteAllBytes(Path.Combine(outputDir, Path.GetFileName(file)), data);
            }
        }
		public static void BuildPSDTranslate(string dirOrPsdFile, string languageTableCSV)
		{
			HashSet<string> newWord = new HashSet<string>();
			StringTable table;
			string[] files = GetFiles(dirOrPsdFile, "*.psd");
			foreach (string file in files)
			{
                string languageTable = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".csv");
                //if (File.Exists(languageTable))
                //{
                //    Console.WriteLine("跳过翻译已存在的LanguageTable: {0}", languageTable);
                //    continue;
                //}
                // 目标文件为翻译时保存出来的文件
                if (file.EndsWith("t.psd"))
                    continue;

				// open psd
				PsdFile psd = new PsdFile(file);

				// build psd text into csv
				table = new StringTable();
				table.AddColumn("图层");
				table.AddColumn("中文");
				table.AddColumn("对齐");

				foreach (Layer layer in psd.Layers)
				{
					foreach (LayerInfo info in layer.AdditionalInfo)
					{
						LayerText textInfo = info as LayerText;
						if (textInfo == null)
							continue;

						string value = textInfo.Text;
						if (value.EndsWith("\0"))
							value = value.Remove(value.Length - 1);

                        if (string.IsNullOrEmpty(value.Trim()))
                            continue;

                        double number;
                        if (double.TryParse(value, out number))
                            continue;

                        value = CSVWriter.Encode(value);
						newWord.Add(value);

						table.AddRow(layer.Name, value, string.Empty);
					}
				}

				WriteCSVTable(languageTable, table);
			}
			// build new text into languag table
			if (File.Exists(languageTableCSV))
			{
				table = ReadCSVTable(languageTableCSV);
			}
			else
			{
				File.Create(languageTableCSV).Close();
				table = new StringTable();
				table.AddColumn("标识列");
				table.AddColumn("中文");
			}

			foreach (string word in newWord)
			{
				if (table.GetRowIndex(1, word) == -1)
				{
					Console.WriteLine("新添文字到语言表：{0}", word);
					table.AddRow(table.RowCount.ToString(), word);
				}
			}

			WriteCSVTable(languageTableCSV, table);
		}
		public static void BuildTranslatePSD(string dirOrPsdFile, string languageTableCSV, string language)
		{
			TableList<string, string> table1 = ReadCSVTable(languageTableCSV);
			int index1 = table1.GetColumnIndex(language);
			if (index1 == -1)
			{
				Console.WriteLine("语言总表没有翻译{0}", language);
				return;
			}
			string primary = table1.GetColumn(1);

            /*
             * PSD修改文字步骤
             * . 打开一个PSD文件
             * --. 按键Ctrl + Alt + 0，实际像素显示图像。图像过大可能已66.67%之类的过小尺寸显示。
             * . 按键Ctrl + 0，按屏幕大小缩放
             * . 按键T，选用文字工具
             * . 循环每个文字图层
             *    1. 鼠标点击图层左下角选中图层并进入编辑（超出屏幕的可能要拖动画布）
             *    P.S. 文字内可能有尺寸不一样的情况，翻译时可能需要逐个替换文字
             *    2. 按键Ctrl + A全选文字，Ctrl + V粘贴文字，Ctrl + Enter确认修改
             * . 按键Ctrl + Shift + S另存PSD源文件
             * . 按键Ctrl + W关闭文件
             * 
             * . 对齐暂未实现：PS中文字图层有对齐选项，选好再进行翻译变长变短也都是对齐的
             */

			string[] files = GetFiles(dirOrPsdFile, "*.psd");
			foreach (string file in files)
			{
                if (file.EndsWith("t.psd"))
                    continue;

				// translated psd file not work
				string fileName = Path.GetFileNameWithoutExtension(file);
				int trans = fileName.IndexOf('_');
				if (trans != -1 && table1.GetColumnIndex(fileName.Substring(trans + 1)) != -1)
					continue;

				Process process = Process.Start(file);
				Sleep(1000);

				languageTableCSV = Path.Combine(Path.ChangeExtension(file, ".csv"));
                // 没有语言表的psd不翻译
                if (!File.Exists(languageTableCSV))
                    continue;
				TableList<string, string> table2 = ReadCSVTable(languageTableCSV);
				int index2 = table2.GetColumnIndex(language);

				//SendKeys.SendWait("^%{0}");
                SendKeys.SendWait("^{0}");

				Rectangle area = Screen.PrimaryScreen.WorkingArea;
				Point center = new Point(area.X + area.Width / 2, area.Y + area.Height / 2);
				// get phothshop window handle and work area
				IntPtr windowIntptr = WindowFromPoint(center);
				int result = GetWindowRect(windowIntptr, ref area);
				// Width & Height is real the Right & Bottom
				area = new Rectangle(area.X, area.Y, area.Width - area.X, area.Height - area.Y);
				PsdFile psd = new PsdFile(file);
                // 0.04大概是滚动条所占视口的比例
                const float SCROLL_BAR = 1.04f;
                float scale = Math.Min(area.Width / SCROLL_BAR / psd.ColumnCount, area.Height / SCROLL_BAR / psd.RowCount);

                SendKeys.SendWait("T");

                Stack<bool> groups = new Stack<bool>();
                for (int i = psd.Layers.Count - 1; i >= 0; i--)
				{
                    var layer = psd.Layers[i];
                    // 文件夹组
                    if (layer.Rect == Rectangle.Empty)
                    {
                        if (layer.Name == "</Layer group>")
                        {
                            // 组结束
                            groups.Pop();
                        }
                        else
                        {
                            // 新组
                            bool visible = layer.Visible && layer.Opacity > 0;
                            if (groups.Count > 0)
                                visible &= groups.Peek();
                            groups.Push(visible);
                        }
                        continue;
                    }
                    // 组不可见或者图层不可见都不翻译
                    if ((groups.Count > 0 && !groups.Peek()) || !layer.Visible || layer.Opacity == 0)
                        continue;

					string text = null;
					foreach (var addition in layer.AdditionalInfo)
					{
						if (addition.Key == "TySh")
						{
							text = (addition as LayerText).Text;
							break;
						}
					}
					if (text != null)
					{
						if (text.EndsWith("\0"))
							text = text.Remove(text.Length - 1);
                        text = CSVWriter.Encode(text);

						string translate = string.Empty;
						if (index2 != -1)
						{
							int row = table2.GetRowIndex(primary, text);
							if (row == -1)
							{
								Console.WriteLine("{0}缺少主键{1}", languageTableCSV, text);
							}
							else
							{
								if (!string.IsNullOrEmpty(table2[index2, row]))
								{
									translate = table2[index2, row];
								}
							}
						}
						if (string.IsNullOrEmpty(translate))
						{
							int row = table1.GetRowIndex(primary, text);
							if (row == -1)
							{
								Console.WriteLine("语言总表缺少主键{0}", text);
							}
							else
							{
								translate = table1[index1, row];
							}
						}

						if (text == translate || string.IsNullOrEmpty(translate))
							continue;

                        // 粘贴翻译到文本图层
                        Point leftTop = new Point((int)((area.Width - psd.ColumnCount * scale) / 2 + area.X), (int)((area.Height - psd.RowCount * scale) / 2 + area.Y));
                        //LeftClick(leftTop.X + layer.Rect.Left, leftTop.Y + layer.Rect.Top + layer.Rect.Height / 2);
                        leftTop.X += _MATH.Ceiling(layer.Rect.Left * scale);
                        leftTop.Y += _MATH.Ceiling((layer.Rect.Top + layer.Rect.Height / 2) * scale);
                        LeftClick(leftTop.X, leftTop.Y);
						Clipboard.SetText(translate);
						Sleep(50);
						SendKeys.SendWait("^a");
						Sleep(50);
						SendKeys.SendWait("^v");
						Sleep(50);
						SendKeys.SendWait("^{ENTER}");
					}

                    if (GetKeyState(Keys.Escape) < 0)
                    {
                        // 激活此窗体提示暂停或退出
                        SetForegroundWindow(Process.GetCurrentProcess().Handle);
                        Console.WriteLine("ESC，ENTER，SPACE键退出，其它任意键继续");
                        var info = Console.ReadKey();
                        if (info.Key == ConsoleKey.Escape
                            || info.Key == ConsoleKey.Spacebar
                            || info.Key == ConsoleKey.Enter)
                            Console.WriteLine("退出");
                    }
				}

                // 保存新PSD文件
				SendKeys.SendWait("^+{s}");
				string newFile = string.Format("{0}_{1}.t.psd", fileName, language);
				Clipboard.SetText(newFile);
				Sleep(50);
				SendKeys.SendWait("^v");
				Sleep(50);
				SendKeys.SendWait("{ENTER}");
				if (File.Exists(Path.Combine(Path.GetDirectoryName(languageTableCSV), newFile)))
				{
					Sleep(500);
					SendKeys.SendWait("{ENTER}");
				}
				Sleep(250);
                SendKeys.SendWait("{ENTER}");
                Sleep(100);
				SendKeys.SendWait("^w");
			}
		}
        /// <summary>UI编辑器翻译</summary>
		public static void BuildTableTranslate(string languageTableCSV, string inputDirOrFile)
		{
			StringTable translate = ReadTranslateTable(languageTableCSV);

			string[] tables = GetCSVTables(inputDirOrFile);
            if (tables.Length == 1 && !File.Exists(tables[0]))
            {
                string csv = _IO.RelativePath(tables[0], languageTableCSV);
                Console.WriteLine("去除引用翻译{0}", csv);
                Translate(translate, StringTable.DefaultLanguageTable(), csv);
            }
            else
            {
                foreach (string file in tables)
                {
                    var table = ReadCSVTable(file);
                    Translate(translate, table, _IO.RelativePath(file, languageTableCSV));
                    WriteCSVTable(file, table);
                }
            }

			WriteCSVTable(languageTableCSV, translate);
		}
        //[Obsolete("生成的csv已可以正常编辑，不需要经过xlsx的文件进行转换")]
        public static void BuildTranslate(string languageTableCSV, string translateTableXlsx, string languagesSplitByComma)
        {
            StringTable lang = ReadTranslateTable(languageTableCSV);
            StringTable table = LoadTableFromExcel(translateTableXlsx, EXCEL_REVERISION);
            StringTable append = new StringTable("CHS");
            string[] languages = languagesSplitByComma.Split(',');

            foreach (var language in languages)
            {
                int index = lang.GetColumnIndex(language);
                if (index == -1)
                    index = lang.AddColumn(language);

                int index2 = table.GetColumnIndex(language);
                if (index2 == -1)
                {
                    Console.WriteLine("没有翻译{0}", language);
                    continue;
                }

                try
                {
                    string[] chs = lang.GetColumns(2);
                    int count = chs.Length;
                    for (int i = 0; i < count; i++)
                    {
                        int line = table.GetRowIndex(0, chs[i]);
                        if (line != -1)
                            lang[index, i] = table[index2, line];
                        else
                            append.AddRow(lang[2, i]);
                    }

                    WriteCSVTable(languageTableCSV, lang);

                    Console.WriteLine("翻译{0}完成！", language);
                }
                catch (Exception)
                {
                    Console.WriteLine("翻译表格式不正确！第一列中文，其它列为相应的翻译");
                    return;
                }
            }
            
            if (append.RowCount > 0)
            {
                Console.WriteLine("需要追加翻译{0}.csv", Path.GetFileNameWithoutExtension(translateTableXlsx));
                WriteCSVTable(Path.ChangeExtension(translateTableXlsx, "csv"), append);
            }
        }
        /// <summary>策划表翻译</summary>
		public static void BuildCSVFromExcel(string inputDirOrFile, string outputDir, string translateTable, string excelVersion, string outputCS, bool isOutputResult)
		{
			BuildDir(ref inputDirOrFile);
			BuildDir(ref outputDir);

			if (!Directory.Exists(outputDir))
				Directory.CreateDirectory(outputDir);

			StringTable tt = null;
			string root = null;
			if (!string.IsNullOrEmpty(translateTable))
			{
				tt = ReadTranslateTable(translateTable);
				root = Path.GetDirectoryName(translateTable);
                if (!string.IsNullOrEmpty(root))
                    root += "\\";
			}

			string[] files = GetFiles(inputDirOrFile, "*.xlsx", SearchOption.AllDirectories);
			foreach (string file in files)
			{
                string tempName = Path.GetFileName(file);
                if (tempName.StartsWith("_") || tempName.StartsWith("#"))
                {
                    Console.WriteLine("skip xlsx: {0}", file);
                    continue;
                }
                try
                {
                    Console.WriteLine("xlsx -> csv: {0}", file);
                    //StringTable table = LoadTableFromExcel(file, excelVersion);
                    //// 去除没有类型的注释列
                    //for (int i = table.ColumnCount - 1; i >= 0; i--)
                    //    if (string.IsNullOrEmpty(table[i, 0]))
                    //        table.RemoveColumn(i);
                    //string output = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file) + ".csv");
                    //if (tt != null)
                    //{
                    //    string relative = _IO.RelativePath(output, root);
                    //    Translate(tt, table, relative);
                    //}
                    //WriteCSVTable(output, table);

                    List<NamedStringTable> tables;
                    if (string.IsNullOrEmpty(excelVersion))
                        tables = LoadTablesFromExcel(file);
                    else
                        tables = LoadTablesFromExcelOleDb(file, excelVersion);
                    if (tables.Count == 1)
                        tables[0].Name = Path.GetFileNameWithoutExtension(file);
                    foreach (var table in tables)
                    {
                        // 去除没有类型的注释列
                        for (int i = table.ColumnCount - 1; i >= 0; i--)
                            if (string.IsNullOrEmpty(table[i, 0]))
                                table.RemoveColumn(i);
                        string output = Path.Combine(outputDir, table.Name + ".csv");
                        if (tt != null)
                        {
                            string relative = _IO.RelativePath(output, root);
                            Translate(tt, table, relative);
                        }
                        WriteCSVTable(output, table);
                    }
                }
                catch (Exception ex)
                {
                    QuitExcel();
                    Console.WriteLine("Error: {0}", ex.Message);
                    return;
                }
			}
            QuitExcel();

            if (tt != null)
            {
                Console.WriteLine("去除翻译表失效引用");
                // 检测被移动过的文件引用
                //if (!string.IsNullOrEmpty(root))
                {
                    Dictionary<string, bool> refferences = new Dictionary<string, bool>();
                    string[] columns = tt.GetColumns(1);
                    int end = columns.Length - 1;
                    for (int i = end; i >= 0; i--)
                    {
                        bool removed = false;
                        files = columns[i].Split('|');
                        //files = JsonReader.Deserialize<string[]>(columns[i]);
                        for (int j = 0; j < files.Length; j++)
                        {
                            string target = root + files[j];
                            bool exists;
                            if (!refferences.TryGetValue(target, out exists))
                            {
                                exists = File.Exists(target);
                                refferences.Add(target, exists);
                            }

                            if (exists)
                                continue;
                            else
                            {
                                removed = true;
                                files[j] = null;
                            }
                        }

                        if (removed)
                        {
                            files = files.Where(f => f != null).ToArray();
                            if (files.Length == 0)
                                tt.RemoveRow(i);
                            else
                                //tt[1, i] = JsonWriter.Serialize(files);
                                tt[1, i] = string.Join("|", files);
                        }
                    }
                }

                WriteCSVTable(translateTable, tt);
            }

            if (!string.IsNullOrEmpty(outputCS))
            {
                // 删除翻译表，否则翻译表也被读取用于生成代码了
                if (tt != null)
                    File.Delete(Path.Combine(outputDir, Path.GetFileName(translateTable)));
                Console.WriteLine("从csv数据表构建读表代码");
                BuildTableFromCSV(outputDir, outputCS);
            }

			if (isOutputResult)
			{
				if (tt != null)
				{
					if (Path.GetDirectoryName(translateTable) == Path.GetFullPath(outputDir))
					{
						Console.WriteLine("翻译表和输出目录在同一目录下不能导出结果");
						return;
					}
					string target = Path.Combine(outputDir, Path.GetFileName(translateTable));
					File.Copy(translateTable, target, true);
					translateTable = target;
				}
                Console.WriteLine("输出csv数据表");
				BuildOutputCSV(translateTable, outputDir);
			}
		}
		public static void BuildTableFromCSV(string inputDirOrFile, string outputTable)
		{
			const string PATH = "_path";
			string className = Path.GetFileNameWithoutExtension(outputTable);
			int index = className.IndexOf('.');
			if (index != -1)
				className = className.Substring(0, index);

			StringBuilder writer = new StringBuilder();
			writer.AppendLine("using System;");
			writer.AppendLine("using System.Collections.Generic;");
			writer.AppendLine("using System.Linq;");
			writer.AppendLine("using EntryEngine;");
			writer.AppendLine("using EntryEngine.Serialize;");
			writer.AppendLine();

            string[] files = GetCSVTables(inputDirOrFile, SearchOption.AllDirectories);

            // 特殊类型：Table<Field, Special>
            Dictionary<string, Dictionary<string, SpecialType>> specials = new Dictionary<string, Dictionary<string, SpecialType>>();
            // 生成数据类型
            foreach (var file in files)
            {
                Console.WriteLine("生成读表代码：{0}", file);
                string name = Path.GetFileNameWithoutExtension(file);
                writer.AppendLine("[AReflexible]public partial class {0}", name);
                writer.AppendLine("{");
                writer.AppendLine("public static bool __Load = true;");

                // Unity导出IOS代码时，反射调用而没有显示调用的例如构造函数和属性的Set方法将被优化掉
                StringBuilder nonOptimize = new StringBuilder();
                // todo: 不排除编程人员需要自己扩展类型静态函数的情况，出现此需求后再修改生成的代码结构
                nonOptimize.AppendLine("static {0}()", name);
                nonOptimize.AppendLine("{");
                nonOptimize.AppendLine("{0} {0} = new {0}();", name);

                //var table = ReadCSVTable(file, 2);
                //if (table.RowCount < CSVReader.TITLE_ROW_COUNT - 1)
                //{
                //    Console.WriteLine("{0}行首数据不正确！", file);
                //}
                //else
                var table = ReadCSVTable(file);
                {
                    string desc;
                    for (int i = 0; i < table.ColumnCount; i++)
                    {
                        desc = table[i, 1];
                        if (!string.IsNullOrEmpty(desc))
                            writer.AppendSummary(desc);
                        string column = table.GetColumn(i);
                        string type = table[i, 0];
                        if (type == "String")
                        {
                            writer.AppendLine("public string {0}", column);
                            writer.AppendLine("{");
                            writer.AppendLine("get {{ return string.IsNullOrEmpty(__{0}) ? null : _LANGUAGE.GetString(__{0}); }}", column);
                            writer.AppendLine("set {{ __{0} = value; }}", column);
                            writer.AppendLine("}");
                            writer.AppendLine("private string __{0};", column);

                            nonOptimize.AppendLine("{0}.{1} = null;", name, column);
                        }
                        else if(type.StartsWith("enum:"))
                        {
                            // 生成枚举类型enum[#UnderlyingType]
                            string enumName = string.Format("E{0}{1}", name, column);
                            writer.Append("public enum {0} : {1}", enumName, type.Substring(5));
                            writer.AppendLine();
                            bool duplicateFlag = false;
                            writer.AppendBlock(() =>
                            {
                                string[] all = table.GetColumns(i);
                                HashSet<string> set = new HashSet<string>();
                                for (int j = 2; j < all.Length; j++)
                                    if (set.Add(all[j]))
                                        writer.AppendLine("{0},", all[j]);
                                    else
                                        duplicateFlag = true;
                            });
                            writer.AppendLine("public {0} {1};", enumName, column);

                            nonOptimize.AppendLine("{0}.{1} = default({2});", name, column, enumName);

                            if (!duplicateFlag)
                            {
                                // 构建字典
                                SpecialType special = new SpecialType();
                                special.TypeName = name + "." + enumName;
                                special.BuildDictionary = 1;

                                Dictionary<string, SpecialType> s;
                                if (!specials.TryGetValue(name, out s))
                                {
                                    s = new Dictionary<string, SpecialType>();
                                    specials.Add(name, s);
                                }
                                s.Add(table.GetColumn(i), special);
                            }
                        }
                        else
                        {
                            SpecialType special = ParseSpecialType(type);
                            if (special == null)
                            {
                                writer.AppendLine("public {0} {1};", type, table.GetColumn(i));

                                nonOptimize.AppendLine("{0}.{1} = default({2});", name, column, type);
                            }
                            else
                            {
                                Dictionary<string, SpecialType> s;
                                if (!specials.TryGetValue(name, out s))
                                {
                                    s = new Dictionary<string, SpecialType>();
                                    specials.Add(name, s);
                                }
                                s.Add(table.GetColumn(i), special);

                                if (!special.IsEnum && !special.IsDictionary)
                                {
                                    // 生成特殊类型代码
                                    string typeName = special.TypeName == null ? table.GetColumn(i) : special.TypeName;
                                    if (special.IsArray1)
                                        typeName += "[]";
                                    if (special.IsArray2)
                                        typeName += "[]";
                                    writer.AppendLine("public string {0};", table.GetColumn(i));
                                    // 特殊字段生成属性，避免序列化读取时的错误，不过 实例.结构属性.字段 = 解析赋值 会出错
                                    // 增加不序列化标记解决问题
                                    if (!string.IsNullOrEmpty(desc))
                                        writer.AppendSummary(desc);
                                    writer.AppendLine("[NonSerialized]");
                                    writer.AppendLine("public {0} _{1};", typeName, table.GetColumn(i));

                                    nonOptimize.AppendLine("{0}.{1} = null;", name, column);
                                }
                                else
                                {
                                    writer.AppendLine("public {0} {1};", special.TypeName, table.GetColumn(i));

                                    nonOptimize.AppendLine("{0}.{1} = default({2});", name, column, special.TypeName);
                                }
                            }
                        }
                    }
                }

                
                nonOptimize.AppendLine("}");
                writer.AppendLine(nonOptimize.ToString());

                writer.AppendLine("}");
            }

            // 生成特殊类型
            HashSet<string> buildTypes = new HashSet<string>();
            foreach (var table in specials)
            {
                foreach (var field in table.Value)
                {
                    if (!field.Value.NeedBuildType)
                        continue;

                    string typeName = field.Value.TypeName == null ? string.Format("{0}_{1}", table.Key, field.Key) : field.Value.TypeName;
                    // #指定生成类型时，不重复生成类型
                    if (!buildTypes.Add(typeName))
                        continue;

                    if (field.Value.IsEnum)
                    {
                        writer.Append("public enum {0}", typeName);
                        if (!string.IsNullOrEmpty(field.Value.EnumUnderlyingType))
                            writer.Append(" : {0}", field.Value.EnumUnderlyingType);
                        writer.AppendLine();
                        writer.AppendBlock(() =>
                        {
                            foreach (var f in field.Value.Fields)
                                writer.AppendLine("{0},", f.Name);
                        });
                    }
                    else
                    {
                        writer.AppendLine("public struct {0}", typeName);
                        writer.AppendBlock(() =>
                        {
                            foreach (var f in field.Value.Fields)
                                writer.AppendLine("public {0} {1};", f.Type, f.Name);
                        });
                    }
                }
            }

			writer.AppendLine("public static partial class {0}", className);
			writer.AppendLine("{");
			writer.AppendLine("public static string {0} {{ get; private set; }}", PATH);

			foreach (var file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);
				writer.AppendLine("public static {0}[] _{0};", name);
                // 按字段生成字典
                if (specials.ContainsKey(name))
                    foreach (var item in specials[name].Where(i => i.Value.IsDictionary))
                        if (item.Value.IsDictionaryGroup)
                            writer.AppendLine("public static Dictionary<{0}, {1}[]> _{1}By{2};", item.Value.TypeName, name, item.Key);
                        else
                            writer.AppendLine("public static Dictionary<{0}, {1}> _{1}By{2};", item.Value.TypeName, name, item.Key);
			}
			writer.AppendLine();
            //writer.AppendLine("public static event Func<string, string> OnLoad;");
			foreach (var file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);
				writer.AppendLine("public static event Action<{0}[]> OnLoad{0};", name);
			}
			writer.AppendLine();
			writer.AppendLine("public static void SetPath(string path)");
			writer.AppendLine("{");
			writer.AppendLine("if ({0} != path)", PATH);
			writer.AppendLine("{");
			writer.AppendLine("{0} = _IO.DirectoryWithEnding(path);", PATH);
			writer.AppendLine("_LOG.Info(\"Set table path: {{0}}\", {0});", PATH);
			writer.AppendLine("}");
			writer.AppendLine("}");
			writer.AppendLine("public static void Load(string path)");
			writer.AppendLine("{");
			writer.AppendLine("SetPath(path);");
			writer.AppendLine("_LOG.Info(\"Load all tables\", path);");
			foreach (var file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);
				writer.AppendLine("Load{0}();", name);
			}
			writer.AppendLine("}");

            writer.AppendLine("public static IEnumerable<AsyncReadFile> LoadAsync(string path)");
            writer.AppendLine("{");
            writer.AppendLine("SetPath(path);");
            writer.AppendLine("_LOG.Info(\"LoadAsync all tables\", path);");
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                writer.AppendLine("foreach (var item in Load{0}Async()) yield return item;", name);
            }
            writer.AppendLine("yield break;");
            writer.AppendLine("}");

			writer.AppendLine("public static void Reload()");
			writer.AppendLine("{");
			writer.AppendLine("_LOG.Info(\"Reload all tables\");");
			writer.AppendLine("Load({0});", PATH);
			writer.AppendLine("}");

            writer.AppendLine("public static IEnumerable<AsyncReadFile> ReloadAsync()");
            writer.AppendLine("{");
            writer.AppendLine("_LOG.Info(\"ReloadAsync all tables\");");
            writer.AppendLine("foreach (var item in LoadAsync({0})) yield return item;", PATH);
            writer.AppendLine("}");

            //writer.AppendLine("private static string LoadTable(string table)");
            //writer.AppendLine("{");
            //writer.AppendLine("table = {0} + table;", PATH);
            //writer.AppendLine("if (OnLoad != null) return OnLoad(table);");
            //writer.AppendLine("else return _IO.ReadText(table);");
            //writer.AppendLine("}");

			foreach (var file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);

                // special
                Dictionary<string, SpecialType> s;
                if (specials.TryGetValue(name, out s))
                {
                    // 解析特殊类型
                    writer.AppendLine("private static void __Parse{0}({0}[] array)", name);
                    writer.AppendBlock(() =>
                    {
                        // 生成字典
                        foreach (var item in s.Where(i => i.Value.IsDictionary))
                        {
                            if (item.Value.IsDictionaryGroup)
                            {
                                writer.AppendLine("Dictionary<{0}, List<{1}>> __group = new Dictionary<{0}, List<{1}>>();", item.Value.TypeName, name);
                                writer.AppendLine("List<{0}> __list;", name);
                                writer.AppendLine("foreach (var item in array)");
                                writer.AppendBlock(() =>
                                {
                                    writer.AppendLine("if (!__group.TryGetValue(item.{0}, out __list))", item.Key);
                                    writer.AppendBlock(() =>
                                    {
                                        writer.AppendLine("__list = new List<{0}>();", name);
                                        writer.AppendLine("__group.Add(item.{0}, __list);", item.Key);
                                    });
                                    writer.AppendLine("__list.Add(item);");
                                });
                                writer.AppendLine("_{0}By{1} = new Dictionary<{2}, {0}[]>();", name, item.Key, item.Value.TypeName);
                                writer.AppendLine("foreach (var item in __group) _{0}By{1}.Add(item.Key, item.Value.ToArray());", name, item.Key);
                            }
                            else
                                writer.AppendLine("_{0}By{1} = array.ToDictionary(__a => __a.{1});", name, item.Key);
                        }

                        // 生成特殊字段
                        if (s.Any(i => !i.Value.IsDictionary && !i.Value.IsEnum))
                        {
                            writer.AppendLine("int length = array.Length;");
                            writer.AppendLine("for (int i = 0; i < length; i++)");
                            writer.AppendBlock(() =>
                            {
                                writer.AppendLine("var value = array[i];");
                                foreach (var item in s.Where(i => !i.Value.IsDictionary))
                                {
                                    writer.AppendLine("if (!string.IsNullOrEmpty(value.{0}))", item.Key);
                                    writer.AppendBlock(() =>
                                    {
                                        string typeName = item.Value.TypeName == null ? string.Format("{0}_{1}", name, item.Key) : item.Value.TypeName;
                                        string fullType = typeName;
                                        Action<StringBuilder, string, string> buildParse =
                                            (builder, instance, r) =>
                                            {
                                                if (item.Value.Fields == null)
                                                {
                                                    if (typeName == "string")
                                                        builder.AppendLine("{0} = {1};", instance, r);
                                                    else
                                                        builder.AppendLine("{0} = {1}.Parse({2});", instance, typeName, r);
                                                }
                                                else
                                                {
                                                    writer.AppendLine("string[] _array = {0}.Split('{1}');", r, item.Value.FieldSeperator);
                                                    for (int i = 0; i < item.Value.Fields.Count; i++)
                                                        if (item.Value.Fields[i].Type == "string")
                                                            writer.AppendLine("{0}.{1} = _array[{2}];", instance, item.Value.Fields[i].Name, i);
                                                        else
                                                            writer.AppendLine("{0}.{1} = {2}.Parse(_array[{3}]);", instance, item.Value.Fields[i].Name, item.Value.Fields[i].Type, i);
                                                }
                                            };
                                        if (item.Value.IsArray2)
                                        {
                                            writer.AppendLine("string[] _array2 = value.{0}.Split('{1}');", item.Key, item.Value.ArraySeperator2);
                                            writer.AppendLine("int length2 = _array2.Length;");
                                            writer.AppendLine("var result1 = new {0}[length2][];", typeName);
                                            writer.AppendLine("for (int k = 0; k < length2; k++)");
                                            writer.AppendBlock(() =>
                                            {
                                                writer.AppendLine("string[] _array1 = _array2[k].Split('{0}');", item.Value.ArraySeperator1);
                                                writer.AppendLine("int length1 = _array1.Length;");
                                                writer.AppendLine("var result = new {0}[length1];", typeName);
                                                writer.AppendLine("for (int j = 0; j < length1; j++)");
                                                writer.AppendBlock(() =>
                                                {
                                                    buildParse(writer, "result[j]", "_array1[j]");
                                                });
                                                writer.AppendLine("result1[k] = result;");
                                            });
                                            writer.AppendLine("value._{0} = result1;", item.Key);
                                        }
                                        else if (item.Value.IsArray1)
                                        {
                                            writer.AppendLine("string[] _array1 = value.{0}.Split('{1}');", item.Key, item.Value.ArraySeperator1);
                                            writer.AppendLine("int length1 = _array1.Length;");
                                            writer.AppendLine("var result = new {0}[length1];", typeName);
                                            writer.AppendLine("for (int j = 0; j < length1; j++)");
                                            writer.AppendBlock(() =>
                                            {
                                                buildParse(writer, "result[j]", "_array1[j]");
                                            });
                                            writer.AppendLine("value._{0} = result;", item.Key);
                                        }
                                        else
                                        {
                                            buildParse(writer, "value._" + item.Key, "value." + item.Key);
                                        }
                                    });
                                }
                            });
                        }
                    });
                }

				writer.AppendLine("public static void Load{0}()", name);
				writer.AppendLine("{");
				writer.AppendLine("if (!{0}.__Load) return;", name);
				writer.AppendLine("_LOG.Debug(\"loading table {0}\");", name);
                //writer.AppendLine("CSVReader _reader = new CSVReader(LoadTable(\"{0}.csv\"));", name);
                writer.AppendLine("CSVReader _reader = new CSVReader(_IO.ReadText({1} + \"{0}.csv\"));", name, PATH);
				writer.AppendLine("{0}[] temp = _reader.ReadObject<{0}[]>();", name);
                if (s != null)
                    writer.AppendLine("__Parse{0}(temp);", name);
				writer.AppendLine("if (OnLoad{0} != null) OnLoad{0}(temp);", name);
				writer.AppendLine("_{0} = temp;", name);
				writer.AppendLine("}");

                writer.AppendLine("public static IEnumerable<AsyncReadFile> Load{0}Async()", name);
                writer.AppendLine("{");
                writer.AppendLine("if (!{0}.__Load) yield break;", name);
                writer.AppendLine("_LOG.Debug(\"loading async table {0}\");", name);
                //writer.AppendLine("CSVReader _reader = new CSVReader(LoadTable(\"{0}.csv\"));", name);
                writer.AppendLine("var async = _IO.ReadAsync({1} + \"{0}.csv\");", name, PATH);
                writer.AppendLine("if (!async.IsEnd) yield return async;");
                writer.AppendLine("CSVReader _reader = new CSVReader(_IO.ReadPreambleText(async.Data));", name, PATH);
                writer.AppendLine("{0}[] temp = _reader.ReadObject<{0}[]>();", name);
                if (s != null)
                    writer.AppendLine("__Parse{0}(temp);", name);
                writer.AppendLine("if (OnLoad{0} != null) OnLoad{0}(temp);", name);
                writer.AppendLine("_{0} = temp;", name);
                writer.AppendLine("}");
			}
			writer.AppendLine("}");

			string result = writer.ToString();
            result = _RH.Indent(result);

			File.WriteAllText(outputTable, result, Encoding.UTF8);
		}
		public static void BuildOutputCSV(string languageTableCSV, string csvDirOrFile)
		{
			// 去掉Source
			if (!string.IsNullOrEmpty(languageTableCSV) &&
				File.Exists(languageTableCSV))
			{
				var table = ReadTranslateTable(languageTableCSV);
				if (table.GetColumn(1) == "Source")
					table.RemoveColumn(1);
				WriteCSVTable(languageTableCSV, table);
			}

			// 去掉表头
			if (!string.IsNullOrEmpty(csvDirOrFile))
			{
				string[] tables = GetCSVTables(csvDirOrFile, SearchOption.AllDirectories);
				foreach (var file in tables)
				{
                    // 跳过语言表
                    if (file == languageTableCSV)
                        continue;
					var table = ReadCSVTable(file, -1);
                    // 去除enum类型等号前面的名字
                    for (int i = 0; i < table.ColumnCount; i++)
                        if (table[i, 0].StartsWith("enum"))
                            for (int j = 2; j < table.RowCount; j++)
                            {
                                int index = table[i, j].IndexOf('=');
                                if (index != -1)
                                    table[i, j] = table[i, j].Substring(index + 1);
                            }
					int title = CSVReader.TITLE_ROW_COUNT - 1;
					for (int i = 0; i < table.ColumnCount; i++)
					{
						for (int j = 0; j < title; j++)
						{
							table[i, j] = string.Empty;
						}
					}
					WriteCSVTable(file, table);
				}
			}
		}
        public static void BuildConstantTable(string inputXLSX, string outputXml, string outputCS, bool overrideValue, string excelVersion)
        {
            StringTable table = LoadTableFromExcel(inputXLSX, excelVersion);

            StringBuilder builder = new StringBuilder();
            int count = table.RowCount;
            string className = Path.GetFileNameWithoutExtension(outputCS);
            int tempIndex = className.IndexOf(".");
            if (tempIndex != -1)
                className = className.Substring(0, tempIndex);

            AppendDefaultNamespace(builder);
            builder.AppendLine("using EntryEngine;");
            builder.AppendLine("using EntryEngine.Serialize;");
            builder.AppendLine();
            builder.AppendLine("[AReflexible]public static partial class {0}", className);
            builder.AppendBlock(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    string summary = table[2, i];
                    if (!string.IsNullOrEmpty(summary))
                        builder.AppendSummary(summary);
                    builder.AppendLine("public static {0} {1};", table[1, i], table[0, i]);
                }
                builder.AppendLine();

                builder.AppendLine("public static Action OnSave;");
                builder.AppendLine("public static Action OnLoad;");
                builder.AppendLine("public static void Save(string file)");
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("if (OnSave != null) OnSave();");
                    builder.AppendLine("_IO.WriteText(file, new XmlWriter().WriteStatic(typeof({0})));", className);
                });

                builder.AppendLine("public static void Load(string content)");
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("new XmlReader(content).ReadStatic(typeof({0}));", className);
                    builder.AppendLine("if (OnLoad != null) OnLoad();");
                });
            });

            SaveCode(outputCS, builder);

            XmlNode root;
            if (!overrideValue && File.Exists(outputXml))
            {
                XmlReader reader = new XmlReader(File.ReadAllText(outputXml, Encoding.UTF8));
                root = reader.ReadToNode();
                for (int i = 0; i < count; i++)
                {
                    var node = root[table[0, i]];
                    if (node == null)
                        root.Add(table[0, i], table[3, i]);
                }
            }
            else
            {
                XmlWriter writer = new XmlWriter();
                writer.WriteNode("root");
                for (int i = 0; i < count; i++)
                    writer.WriteNode(table[0, i], table[3, i]);
                writer.WriteNodeClose("root");

                XmlReader reader = new XmlReader(writer.Result);
                root = reader.ReadToNode();
            }
            File.WriteAllText(outputXml, root.OutterXml, Encoding.UTF8);
        }
        public static void BuildUnityEditorScript(string dllAndNamespace, string outputDir)
		{
			Type[] types = GetDllTypes(dllAndNamespace, SearchOption.TopDirectoryOnly);
			foreach (var type in types.Where(t => t.IsClass))
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("using System;");
				builder.AppendLine("using System.Collections.Generic;");
				builder.AppendLine("using UnityEngine;");
				builder.AppendLine("using {0};", type.Namespace);
				builder.AppendLine();
				string className = "Unity_" + type.Name;
				builder.AppendLine("public class Unity_{0} : MonoBehaviour", type.Name);
				builder.AppendLine("{");
				var fields = type.GetFields();
				string targetName = "__" + type.Name.ToLowerInvariant();
				builder.AppendLine("private {0} {1} = new {0}();", type.Name, targetName);
				builder.AppendLine("public {0} {0} {{ get {{ return {1}; }} set {{ {1} = value; Awake(); }} }}", type.Name, targetName);
				builder.AppendLine();
				// fields
				foreach (var field in fields)
					builder.AppendLine("public {0} {1};", field.FieldType.CodeName(), field.Name);
				builder.AppendLine();
				// set script default value
				builder.AppendLine("void Awake()");
				builder.AppendLine("{");
				foreach (var field in fields)
					builder.AppendLine("this.{1} = {0}.{1};", type.Name, field.Name);
				builder.AppendLine("}");
				// update every fields value
				builder.AppendLine("public void Update()");
				builder.AppendLine("{");
				foreach (var field in fields)
					builder.AppendLine("{0}.{1} = this.{1};", type.Name, field.Name);
				builder.AppendLine("}");
				builder.AppendLine("}");

				string output = Path.Combine(outputDir, className) + ".cs";
				Console.WriteLine("write script {0} completed.", output);
                File.WriteAllText(output, _RH.Indent(builder.ToString()), Encoding.UTF8);
			}
		}
		public static void BuildDatabase(string dll, string ns_dbname, string outputCS, string outputDll)
		{
			Assembly assembly = Assembly.LoadFrom(Path.GetFullPath(dll));
			var types = assembly.GetTypesWithAttribute<MemoryTableAttribute>();
			// foreign relationship
			TreeField tree = new TreeField();
			foreach (var table in types)
			{
				var fields = table.GetFields();
				foreach (var field in fields)
				{
					var foreign = field.GetAttribute<ForeignAttribute>();
					if (foreign != null)
					{
						var foreignKey = string.IsNullOrEmpty(foreign.ForeignField) ? field.Name : foreign.ForeignField;
						var parentField = foreign.ForeignTable.GetField(foreignKey);
						if (parentField == null)
							throw new ArgumentNullException(string.Format("{0}.{1} required foreign key {2}.{3} is not exists.", table.Name, field.Name, foreign.ForeignTable.Name, foreignKey));
						var parentIndex = parentField.GetAttribute<IndexAttribute>();
						if (parentIndex == null || parentIndex.Index == EIndex.Group)
							throw new NotSupportedException("Be refferenced foreign field must be a unique key.");
						var myIndex = field.GetAttribute<IndexAttribute>();
						if (myIndex == null)
							throw new NotSupportedException(string.Format("Foreign key {0}.{1} must have a IndexAttribute.", table.Name, field.Name));
						var parent = tree.Find(f => f.Field == parentField);
						if (parent == null)
						{
							parent = new TreeField(parentField);
							tree.Add(parent);
						}
						else if (parent.Parent != null && parent.Parent.Field == field)
							throw new NotSupportedException("Foreign key can't be circular.");

						// added as other foreign parent
						var me = tree.Find(f => f.Field == field);
						if (me == null)
							me = new TreeField(field);

						parent.Add(me);
					}
				}
			}

			StringBuilder builder = new StringBuilder();
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Collections;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine("using System.Linq;");
			builder.AppendLine("using EntryEngine;");
			builder.AppendLine("using EntryEngine.Serialize;");
			builder.AppendLine("using EntryEngine.Network.Database;");
			foreach (var ns in types.Select(t => t.Namespace).Distinct())
				builder.AppendLine("using {0};", ns);
			builder.AppendLine();

			int dot = ns_dbname.LastIndexOf('.');
			if (dot == -1)
			{
				builder.AppendLine("namespace {0}", ns_dbname);
			}
			else
			{
				builder.AppendLine("namespace {0}", ns_dbname.Substring(0, dot));
				ns_dbname = ns_dbname.Substring(dot + 1);
			}
			builder.AppendLine("{");

			// build memory table
			foreach (var table in types)
			{
				var obj = Activator.CreateInstance(table);
				var fields = table.GetFields();
				var primary = fields.Where(f =>
				{
					var index = f.GetAttribute<IndexAttribute>();
					return index != null && index.Index == EIndex.Primary;
				}).ToArray();

				if (primary.Length > 1)
				{
					// NotSupportedException: multiple primary key can't be refferenced by foreign key
					if (tree.Find(f => primary.Contains(f.Field)) != null)
						throw new NotSupportedException("Multiple primary key can't be refferenced by foreign key.");
					// multiple primary key type
					builder.AppendLine("public struct Primary{0} : IEquatable<Primary{0}>", table.Name);
					builder.AppendLine("{");
					foreach (var field in primary)
						builder.AppendLine("public {0} {1};", field.FieldType.CodeName(), field.Name);
					// ToString
                    builder.AppendLine("public override string ToString() { return JsonWriter.Serialize(this); }");
					// GetHashCode
					builder.AppendLine("public override int GetHashCode()");
					builder.AppendLine("{");
					builder.AppendLine("int hash = 0;");
					foreach (var field in primary)
						builder.AppendLine("hash += {0}.GetHashCode();", field.Name);
					builder.AppendLine("return hash;");
					builder.AppendLine("}");
					// Equals
					builder.AppendLine("public bool Equals(Primary{0} obj)", table.Name);
					builder.AppendLine("{");
					builder.AppendLine("return {0};", string.Join(" && ", primary.Select(p => string.Format("obj.{0} == this.{0}", p.Name)).ToArray()));
					builder.AppendLine("}");
					builder.AppendLine("public override bool Equals(object obj)");
					builder.AppendLine("{");
					builder.AppendLine("bool result = false;");
					builder.AppendLine("if (obj is Primary{0}) return Equals((Primary{0})obj);", table.Name);
					builder.AppendLine("return result;");
					builder.AppendLine("}");
					builder.AppendLine("}");
				}
				// table class
				builder.AppendLine("public class _{0} : {0}", table.Name);
				builder.AppendLine("{");
				builder.AppendLine("public Memory{0} MemoryTable {{ get; internal set; }}", table.Name);
				//builder.AppendLine("internal _{0}() {{ }}", table.Name);
				builder.AppendLine("internal _{0}(Memory{0} memory, {0} table)", table.Name);
				builder.AppendLine("{");
				builder.AppendLine("MemoryTable = memory;");
				foreach (var field in fields)
					builder.AppendLine("base.{0} = table.{0};", field.Name);
				builder.AppendLine("}");
				// update
				builder.AppendLine("public void Update({0} table)", table.Name);
				builder.AppendLine("{");
				foreach (var field in fields)
					builder.AppendLine("this.{0} = table.{0};", field.Name);
				builder.AppendLine("}");
				if (primary.Length > 1)
				{
					// multiple primary key
					builder.AppendLine("public Primary{0} Primary", table.Name);
					builder.AppendLine("{");
					builder.AppendLine("get");
					builder.AppendLine("{");
					builder.AppendLine("Primary{0} primary = new Primary{0}();", table.Name);
					foreach (var field in primary)
						builder.AppendLine("primary.{0} = {0};", field.Name);
					builder.AppendLine("return primary;");
					builder.AppendLine("}");
					builder.AppendLine("}");
				}
				foreach (var field in fields)
				{
					var foreign = tree.Find(t => t.Field == field);
					if (foreign != null)
					{
						if (foreign.Parent != null && foreign.Parent.Field != null)
						{
							builder.AppendLine("public _{0} Foreign{1}{0}", foreign.Parent.Table.Name, foreign.Parent.Field.Name);
							builder.AppendLine("{");
							//builder.AppendLine("get;");
							//builder.AppendLine("internal set;");
							builder.AppendLine("get {{ return MemoryTable.MemoryDatabase.{0}.FindBy{1}({2}); }}", foreign.Parent.Table.Name, foreign.Parent.Field.Name, field.Name);
							builder.AppendLine("}");
						}
						foreach (var item in foreign)
						{
							var childIndex = item.Field.GetAttribute<IndexAttribute>();
							// null index has been checked by check foreign key
							if (childIndex == null ||
								childIndex.Index == EIndex.Group ||
								item.Table.GetFields().Count(f =>
								{
									var index = f.GetAttribute<IndexAttribute>();
									return index != null && index.Index == EIndex.Primary;
								}) > 1)
								builder.AppendLine("public IEnumerable<_{0}> Foreign{1}{0}", item.Table.Name, item.Field.Name);
							else
								builder.AppendLine("public _{0} Foreign{1}{0}", item.Table.Name, item.Field.Name);
							builder.AppendLine("{");
							//builder.AppendLine("get;");
							//builder.AppendLine("internal set;");
							builder.AppendLine("get {{ return MemoryTable.MemoryDatabase.{0}.FindBy{1}({2}); }}", item.Table.Name, item.Field.Name, field.Name);
							builder.AppendLine("}");
						}
					}
				}
				foreach (var field in fields)
				{
					var foreign = tree.Find(t => t.Field == field);
					var index = field.GetAttribute<IndexAttribute>();
					var check = new StringBuilder();
					check.Append("if (MemoryTable.FindBy{0}(value) != null) throw new InvalidOperationException(string.Format(\"Modify duplicate {0} = {{0}}.\", value));", field.Name);
					var temp = new StringBuilder();
					var fp = foreign;
					while (index == null)
					{
						if (fp == null || fp.Field == null)
							break;
						index = fp.Field.GetAttribute<IndexAttribute>();
						fp = fp.Parent;
					}
					if (index != null)
					{
						switch (index.Index)
						{
							case EIndex.Index:
								temp = null;
								break;

							case EIndex.Primary:
								// check multiple primary key
								if (primary.Length > 1)
								{
									check = new StringBuilder();
									check.AppendLine("var _primary = Primary;");
									check.AppendLine("_primary.{0} = value;", field.Name);
									check.Append("if (MemoryTable.FindByPrimary(_primary) != null) throw new InvalidOperationException(string.Format(\"Modify Primary{0} duplicate {{0}}.\", _primary));", table.Name);
								}
								temp = null;
								break;

							case EIndex.Identity:
								//builder.AppendLine("internal static {0} Identity{1} = {2};", field.FieldType.CodeName(), field.Name, field.GetValue(obj));
								//builder.AppendLine("internal static {0} GetIdentity{1}()", field.FieldType.CodeName(), field.Name);
								//builder.AppendLine("{");
								//builder.AppendLine("return Identity{0}++;", field.Name);
								//builder.AppendLine("}");
								temp.Append("if (value > MemoryTable.Identity{0}) MemoryTable.Identity{0} = value;", field.Name);
								break;

							case EIndex.Group:
								temp.Append("MemoryTable.UpdateGroup{0}(this, value);", field.Name);
								check = null;
								break;

							default:
								throw new ArgumentException(string.Format("error index type: {0}", index.Index));
						}
						builder.AppendSummary(field);
						builder.AppendLine("public new {0} {1}", field.FieldType.CodeName(), field.Name);
						builder.AppendLine("{");
						builder.AppendLine("get {{ return base.{0}; }}", field.Name);
						builder.AppendLine("set");
						builder.AppendLine("{");
						if (foreign != null && foreign.Parent != null && foreign.Parent.Field != null)
						{
							builder.AppendLine("if (value == {0}) return;", field.Name);
							// send message to foreign parent
							builder.AppendLine("Foreign{0}{1}.{0} = value;", foreign.Parent.Field.Name, foreign.Parent.Table.Name);
							builder.AppendLine("}");
							builder.AppendLine("}");
							builder.AppendLine("internal void SetForeign{0}({1} value)", field.Name, field.FieldType.CodeName());
							builder.AppendLine("{");
						}
						else
						{
							builder.AppendLine("if (value == {0}) return;", field.Name);
							if (check != null)
								builder.AppendLine(check.ToString());
						}
						builder.AppendLine("_LOG.Debug(\"Update {0}.{1} {{0}} => {{1}}\", base.{1}, value);", field.DeclaringType.Name, field.Name);
						builder.AppendLine("base.{0} = value;", field.Name);
						if (temp != null)
							builder.AppendLine(temp.ToString());
						// child foreign field will be changede
						if (foreign != null && foreign.ChildCount > 0)
						{
							foreach (var child in foreign)
							{
								var childIndex = child.Field.GetAttribute<IndexAttribute>();
								// null index has been checked by check foreign key
								if (childIndex == null ||
									childIndex.Index == EIndex.Group ||
									child.Table.GetFields().Count(f =>
									{
										var tempIndex = f.GetAttribute<IndexAttribute>();
										return tempIndex != null && tempIndex.Index == EIndex.Primary;
									}) > 1)
									builder.AppendLine("foreach (var table in Foreign{0}{1}) table.SetForeign{0}(value);", child.Field.Name, child.Table.Name);
								else
									builder.AppendLine("Foreign{0}{1}.SetForeign{0}(value);", child.Field.Name, child.Table.Name);
							}
						}
						builder.AppendLine("}");
						if (!(foreign != null && foreign.Parent != null && foreign.Parent.Field != null))
							builder.AppendLine("}");
					}
				}
				builder.AppendLine("}");

				// memory table
				string tableName = "_" + table.Name;
				builder.AppendLine("public class Memory{0} : IEnumerable<{1}>", table.Name, tableName);
				builder.AppendLine("{");
                builder.AppendLine("public static bool IsSaveLoad = {0};", _RH.CodeValue(!table.GetAttribute<MemoryTableAttribute>().TempTable));
				builder.AppendLine("public event Action<{0}> OnAdd;", tableName);
				builder.AppendLine("public event Action<{0}> OnDelete;", tableName);
				builder.AppendLine("public event Action<IEnumerable<{0}>> OnTruncate;", tableName);
				builder.AppendLine("public int Count { get { return " + table.Name + ".Count; } }");
				builder.AppendLine("public {0} MemoryDatabase {{ get; private set; }}", ns_dbname);
				builder.AppendLine();
				// dictionary
				builder.AppendLine("private List<{1}> {0} = new List<{1}>();", table.Name, tableName);
				if (primary.Length > 1)
					builder.AppendLine("private Dictionary<Primary{0}, {1}> {0}Primary = new Dictionary<Primary{0}, {1}>();", table.Name, tableName);
				foreach (var field in fields)
				{
					var index = field.GetAttribute<IndexAttribute>();
					if (index != null)
					{
						switch (index.Index)
						{
							case EIndex.Primary:
								if (primary.Length > 1)
									goto case EIndex.Group;
								else
									goto case EIndex.Index;

							case EIndex.Index:
							case EIndex.Identity:
								builder.AppendLine("private Dictionary<{0}, {3}> {1}{2} = new Dictionary<{0}, {3}>();", field.FieldType.CodeName(), table.Name, field.Name, tableName);
								if (index.Index == EIndex.Identity)
									builder.AppendLine("internal {0} Identity{1} = {2};", field.FieldType.CodeName(), field.Name, field.GetValue(obj));
								break;

							case EIndex.Group:
								builder.AppendLine("private Dictionary<{0}, List<{3}>> {1}{2} = new Dictionary<{0}, List<{3}>>();", field.FieldType.CodeName(), table.Name, field.Name, tableName);
								break;
						}
					}
				}
				builder.AppendLine();
				// constructor
				builder.AppendLine("internal Memory{0}({1} mdb, IEnumerable<{0}> _table)", table.Name, ns_dbname);
				builder.AppendLine("{");
				builder.AppendLine("this.MemoryDatabase = mdb;");
				builder.AppendLine("var table = _table.Select(t => new _{0}(this, t));", table.Name);
				// constrct all index key
				builder.AppendLine("{0}.AddRange(table);", table.Name);
				if (primary.Length > 1)
					builder.AppendLine("{0}Primary = table.ToDictionary(t => t.Primary);", table.Name);
				foreach (var field in fields)
				{
					var index = field.GetAttribute<IndexAttribute>();
					if (index != null)
					{
						switch (index.Index)
						{
							case EIndex.Primary:
								if (primary.Length > 1)
									goto case EIndex.Group;
								else
									goto case EIndex.Index;

							case EIndex.Index:
							case EIndex.Identity:
								builder.AppendLine("{0}{1} = table.ToDictionary(t => t.{1});", table.Name, field.Name);
								// reset identity
								if (index.Index == EIndex.Identity)
									builder.AppendLine("if ({1}.Count > 0) Identity{0} = {1}.Max(t => t.{0}) + 1;", field.Name, table.Name);
								break;

							case EIndex.Group:
								builder.AppendLine("{0}{1} = table.Group(t => t.{1});", table.Name, field.Name);
								break;
						}
					}
				}
				builder.AppendLine("}");
				builder.AppendLine();
				// find by index
				if (primary.Length > 1)
				{
					builder.AppendLine("public {1} FindByPrimary(Primary{0} primary)", table.Name, tableName);
					builder.AppendLine("{");
					builder.AppendLine("{0} result;", tableName);
					builder.AppendLine("{0}Primary.TryGetValue(primary, out result);", table.Name);
					builder.AppendLine("return result;");
					builder.AppendLine("}");
				}
				foreach (var field in fields)
				{
					var index = field.GetAttribute<IndexAttribute>();
					var foreign = tree.Find(f => f.Field == field);
					bool enumerable = false;
					EIndex key = EIndex.Index;
					if (index == null)
					{
						if (foreign != null)
						{
							key = EIndex.Group;
							enumerable = true;
						}
					}
					else
					{
						key = index.Index;
					}
					if (index != null)
					{
						switch (key)
						{
							case EIndex.Primary:
								if (primary.Length > 1)
									goto case EIndex.Group;
								else
									goto case EIndex.Index;

							case EIndex.Index:
							case EIndex.Identity:
								builder.AppendLine("public {0} FindBy{1}({2} value)", tableName, field.Name, field.FieldType.CodeName());
								break;

							case EIndex.Group:
								// change group
								builder.AppendLine("internal void UpdateGroup{0}({1} table, {2} value)", field.Name, tableName, field.FieldType.CodeName());
								builder.AppendLine("{");
                                builder.AppendLine("_LOG.Info(\"Update Group {0}.{1} {{0}} -> {{1}}\", table.{1}, value);", table.Name, field.Name);
								builder.AppendLine("{0}{1}[table.{1}].Remove(table);", table.Name, field.Name);
								builder.AppendLine("List<{0}> temp;", tableName);
								builder.AppendLine("if ({0}{1}.TryGetValue(table.{1}, out temp)) temp.Add(table);", table.Name, field.Name);
								builder.AppendLine("else {0}{1}.Add(table.{1}, new List<{2}>() {{ table }});", table.Name, field.Name, tableName);
								builder.AppendLine("}");
								builder.AppendLine("public IEnumerable<{0}> FindBy{1}({2} value)", tableName, field.Name, field.FieldType.CodeName());
								enumerable = true;
								break;
						}
						builder.AppendLine("{");
						if (enumerable)
							builder.AppendLine("List<{0}> result;", tableName);
						else
							builder.AppendLine("{0} result;", tableName);
						builder.AppendLine("{0}{1}.TryGetValue(value, out result);", table.Name, field.Name);
						if (enumerable)
							builder.AppendLine("return result.Enumerable();");
						else
							builder.AppendLine("return result;", tableName);
						builder.AppendLine("}");
					}
				}
				// Add Delete Truncate UpdateGroup
				builder.AppendLine("public _{0} Add({0} _table)", table.Name);
				builder.AppendLine("{");
                builder.AppendLine("_LOG.Info(\"Memory {0} Add {{0}}\", JsonWriter.Serialize(_table));", table.Name);
				builder.AppendLine("{0} table = new {0}(this, _table);", tableName);
				// check index key
				builder.AppendLine("bool duplicate = true;");
				if (primary.Length != 0)
				{
					foreach (var item in primary)
						builder.AppendLine("duplicate &= {0}{1}.ContainsKey(table.{1});", table.Name, item.Name);
					builder.Append("if (duplicate) ");
					if (primary.Length > 1)
						builder.AppendLine("throw new InvalidOperationException(string.Format(\"Add duplicate Primary = {0}\", table.Primary));");
					else
						builder.AppendLine("throw new InvalidOperationException(string.Format(\"Add duplicate Primary = {{0}}\", table.{0}));", primary[0].Name);
				}
				foreach (var field in fields)
				{
					var index = field.GetAttribute<IndexAttribute>();
					if (index != null)
					{
						switch (index.Index)
						{
							case EIndex.Index:
								builder.AppendLine("duplicate = {0}{1}.ContainsKey(table.{1});", table.Name, field.Name);
								builder.AppendLine("if (duplicate) throw new InvalidOperationException(string.Format(\"Add duplicate {0} = {{0}}\", table.{0}));", field.Name);
								break;
						}
					}
				}
				// check foreign key
				foreach (var field in fields)
				{
					var foreign = tree.Find(f => f.Field == field);
					if (foreign != null && foreign.Parent != null && foreign.Parent.Field != null)
					{
						builder.AppendLine("if (table.Foreign{1}{0} == null) throw new InvalidOperationException(string.Format(\"{0} must have required foreign key {1} = {{0}}.\", table.{2}));", foreign.Parent.Table.Name, foreign.Parent.Field.Name, field.Name);
					}
				}
				builder.AppendLine("{0}.Add(table);", table.Name);
				builder.AppendLine("List<{0}> temp;", tableName);
				foreach (var field in fields)
				{
					var index = field.GetAttribute<IndexAttribute>();
					if (index != null)
					{
						switch (index.Index)
						{
							case EIndex.Index:
								builder.AppendLine("{0}{1}.Add(table.{1}, table);", table.Name, field.Name);
								break;

							case EIndex.Primary:
								if (primary.Length == 1)
									builder.AppendLine("{0}{1}.Add(table.{1}, table);", table.Name, field.Name);
								else
									goto case EIndex.Group;
								break;

							case EIndex.Identity:
								builder.AppendLine("(({0})table).{1} = Identity{1}++;", table.Name, field.Name);
								builder.AppendLine("{0}{1}.Add(table.{1}, table);", table.Name, field.Name);
								break;

							case EIndex.Group:
								builder.AppendLine("if ({0}{1}.TryGetValue(table.{1}, out temp)) temp.Add(table);", table.Name, field.Name);
								builder.AppendLine("else {0}{1}.Add(table.{1}, new List<{2}>() {{ table }});", table.Name, field.Name, tableName);
								break;
						}
					}
				}
				builder.AppendLine("if (OnAdd != null) OnAdd(table);");
				builder.AppendLine("return table;");
				builder.AppendLine("}");
				// delete
				builder.AppendLine("public int Delete(Func<{0}, bool> where)", tableName);
				builder.AppendLine("{");
				builder.AppendLine("int count;");
				builder.AppendLine("if (where == null)");
				builder.AppendLine("{");
				builder.AppendLine("count = Count;");
				builder.AppendLine("Truncate();");
				builder.AppendLine("}");
				builder.AppendLine("else");
				builder.AppendLine("{");
				builder.AppendLine("List<{0}> removed;", tableName);
				builder.AppendLine("{0} = {0}.WhereAndRemove(where, out removed);", table.Name);
				builder.AppendLine("count = removed.Count;");
				builder.AppendLine("foreach (var remove in removed)");
				builder.AppendLine("{");
				foreach (var field in fields)
				{
					// delete foreign child
					var foreign = tree.Find(f => f.Field == field);
					if (foreign != null)
					{
						foreach (var child in foreign)
						{
							var childIndex = child.Field.GetAttribute<IndexAttribute>();
							// null index has been checked by check foreign key
							builder.AppendLine("var foreign{0}{1} = remove.Foreign{0}{1};", child.Field.Name, child.Table.Name);
							builder.Append("if (foreign{0}{1} != null) ", child.Field.Name, child.Table.Name);
							if (childIndex == null ||
								childIndex.Index == EIndex.Group ||
								child.Table.GetFields().Count(f =>
								{
									var tempIndex = f.GetAttribute<IndexAttribute>();
									return tempIndex != null && tempIndex.Index == EIndex.Primary;
								}) > 1)
								builder.AppendLine("foreach (var child in foreign{1}{0}) MemoryDatabase.{0}.Delete(t => t.{1} == child.{1});", child.Table.Name, child.Field.Name);
							else
								builder.AppendLine("MemoryDatabase.{0}.Delete(t => t.{1} == remove.{2});", child.Table.Name, child.Field.Name, field.Name);
						}
					}
					var index = field.GetAttribute<IndexAttribute>();
					if (index != null)
					{
						switch (index.Index)
						{
							case EIndex.Index:
							case EIndex.Identity:
								builder.AppendLine("{0}{1}.Remove(remove.{1});", table.Name, field.Name);
								break;

							case EIndex.Primary:
								if (primary.Length == 1)
									builder.AppendLine("{0}{1}.Remove(remove.{1});", table.Name, field.Name);
								else
									goto case EIndex.Group;
								break;

							case EIndex.Group:
								builder.AppendLine("{0}{1}[remove.{1}].Remove(remove);", table.Name, field.Name);
								break;
						}
					}
				}
				builder.AppendLine("if (OnDelete != null) OnDelete(remove);");
                builder.AppendLine("_LOG.Info(\"Memory {0} Delete {{0}}\", JsonWriter.Serialize(remove));", table.Name);
				builder.AppendLine("}");// end of foreach
				builder.AppendLine("}");// end of else
                builder.AppendLine("_LOG.Info(\"Delete {0} {{0}} rows affected.\", count);", table.Name);
				builder.AppendLine("return count;");
				builder.AppendLine("}");
				// truncate
				builder.AppendLine("public void Truncate()");
				builder.AppendLine("{");
				builder.AppendLine("if (Count == 0) return;");
				builder.AppendLine("_LOG.Info(\"Truncate {0} [{{0}}]...\", Count);", table.Name);
				builder.AppendLine("if (OnTruncate != null) OnTruncate(this);");
				builder.AppendLine("{0}.Clear();", table.Name);
				if (primary.Length > 1)
					builder.AppendLine("{0}Primary.Clear();", table.Name);
				foreach (var field in fields)
				{
					// delete foreign key
					var foreign = tree.Find(t => t.Field == field);
					if (foreign != null)
						foreach (var child in foreign)
							builder.AppendLine("MemoryDatabase.{0}.Truncate();", child.Table.Name);
					var index = field.GetAttribute<IndexAttribute>();
					if (index != null)
					{
						builder.AppendLine("{0}{1}.Clear();", table.Name, field.Name);
						if (index.Index == EIndex.Identity)
						{
							builder.AppendLine("Identity{0} = {1};", field.Name, field.GetValue(obj));
						}
					}
				}
                builder.AppendLine("_LOG.Info(\"Memory {0} truncated!\");", table.Name);
				builder.AppendLine("}");
				// IEnumerable
				builder.AppendLine("public IEnumerator<{1}> GetEnumerator() {{ return {0}.GetEnumerator(); }}", table.Name, tableName);
				builder.AppendLine("IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }");
				// end
				builder.AppendLine("}");
			}

			// build database
			builder.AppendLine("public class {0} : {1}", ns_dbname, typeof(MemoryDatabase).Name);
			builder.AppendLine("{");
			builder.AppendLine("public bool Loaded { get; private set; }");
			// memory tables
			foreach (var table in types)
				builder.AppendLine("public Memory{0} {0} {{ get; private set; }}", table.Name);
			builder.AppendLine();
			// constructor
			builder.AppendLine("public {0}(string name) : base(name) {{ }}", ns_dbname);
			builder.AppendLine();
			// save
			builder.AppendLine("public override void Save()");
			builder.AppendLine("{");
            builder.AppendLine("_LOG.Info(\"Save {0}...\");", ns_dbname);
			builder.AppendLine("if (!Loaded) throw new InvalidOperationException(\"Save operation requires {0} must have been loaded.\");", ns_dbname);
			builder.AppendLine("Timer timer = Timer.StartNew();");
			foreach (var table in types)
				builder.AppendLine("if (Memory{0}.IsSaveLoad) InternalSave({0}.ToArray(), \"{0}\");", table.Name);
            builder.AppendLine("_LOG.Info(\"Save {0}.{{0}} completed! Time elapsed: {{1}}\", Name, timer.Stop());", ns_dbname);
			builder.AppendLine("}");
			// load
			builder.AppendLine("public override void Load()");
			builder.AppendLine("{");
            builder.AppendLine("_LOG.Info(\"Load {0}...\");", ns_dbname);
			builder.AppendLine("if (Loaded) throw new InvalidOperationException(\"{0} has loaded memory tables.\");", ns_dbname);
			builder.AppendLine("Timer timer = Timer.StartNew();");
			foreach (var table in types)
				builder.AppendLine("{0} = new Memory{0}(this, Memory{0}.IsSaveLoad ? InternalLoad<{0}>(\"{0}\") : new List<{0}>());", table.Name);
			builder.AppendLine("Loaded = true;");
            builder.AppendLine("_LOG.Info(\"Load {0}.{{0}} completed! Time elapsed: {{1}}\", Name, timer.Stop());", ns_dbname);
			builder.AppendLine("}");
			// merge
			foreach (var table in types)
			{
				builder.AppendLine("public void Merge{0}(IEnumerable<_{0}> merge)", table.Name);
				builder.AppendLine("{");
                builder.AppendLine("_LOG.Info(\"Merge {0}...\");");
				builder.AppendLine("if (!Loaded) throw new InvalidOperationException(\"Merge {1} operation requires {0} must have been loaded.\");", ns_dbname, table.Name);
				builder.AppendLine("int count = {0}.Count;", table.Name);
				builder.AppendLine("{0} = new Memory{0}(this, (IEnumerable<{0}>){0}.Concat(merge));", table.Name);
                builder.AppendLine("_LOG.Info(\"Merge {0}.{{0}} [{{1}}] completed.\", Name, {0}.Count - count);", table.Name);
				builder.AppendLine("}");
			}
			// IDisposable
			builder.AppendLine("public override void Dispose()");
			builder.AppendLine("{");
            builder.AppendLine("_LOG.Info(\"Dispose {0}...\");", ns_dbname);
			builder.AppendLine("if (!Loaded) return;", ns_dbname);
			foreach (var table in types)
			{
				builder.AppendLine("{0}.Truncate();", table.Name);
				builder.AppendLine("{0} = null;", table.Name);
			}
			builder.AppendLine("Loaded = false;");
            builder.AppendLine("_LOG.Info(\"Dispose {0} completed!\");", ns_dbname);
			builder.AppendLine("}");
			// end
			builder.AppendLine("}");// end of dbname class
			builder.AppendLine("}");// end of namespace

			if (!string.IsNullOrEmpty(outputCS))
			{
                File.WriteAllText(outputCS, _RH.Indent(builder.ToString()), Encoding.UTF8);
				Console.WriteLine("Compile {0} to MemoryDatabase {1} succuss!", dll, outputCS);
			}
			if (!string.IsNullOrEmpty(outputDll))
			{
				var result = UtilityCode.Compile(outputDll, "3.5", "", Environment.CurrentDirectory, builder.ToString());
				if (result.Errors.Count > 0)
					for (int i = 0; i < result.Errors.Count; i++)
						Console.WriteLine(result.Errors[i].ErrorText);
				else
					Console.WriteLine("Compile {0} to MemoryDatabase {1} succuss!", dll, outputDll);
			}
		}
        /// <param name="isStatic">非静态的使用场景：需要连接多个数据结构完全一样的数据库时，例如GM工具为每一个需要操作的数据库构建一个操作实例</param>
        public static void BuildDatabaseMysql(string dll, string nsOrEmptyDotDBnameOrEmpty, string outputCS, string sharpIfBuild, string mysqlClassOrEmpty, bool isStatic)
        {
            /*
             * 优化
             * 1. 大数据表添加列时，在MySQL5.6将不重建表结构
             * 2. 主键在移除后，没有任何改变的CHANGE COLUMN也将影响所有数据，速度较慢
             * 3. 移除索引时，将影响所有数据，当数据量很大时，速度极慢
             * 4. 添加索引时，虽然影响行数为0，但当数据量很大时，任然很慢
             */
            Func<FieldInfo, bool> FieldCanUpdate = (f) =>
                {
                    var attribute = f.GetAttribute<IndexAttribute>();
                    return attribute == null || (attribute.Index != EIndex.Primary && attribute.Index != EIndex.Identity);
                };

            Assembly assembly = Assembly.LoadFrom(Path.GetFullPath(dll));
            var types = assembly.GetTypesWithAttribute<MemoryTableAttribute>(false).ToArray();

            // 外键：tree的第一层为主键表，主键表下有所有引用它的外键表，外键表可能有主键表
            TreeField tree = new TreeField();
            Dictionary<Type, List<FieldInfo>> identities = new Dictionary<Type, List<FieldInfo>>();
            foreach (var table in types)
            {
                Console.WriteLine("dbtable: {0}", table.Name);
                var fields = table.GetFields();
                foreach (var field in fields)
                {
                    // 外键
                    var foreign = field.GetAttribute<ForeignAttribute>();
                    if (foreign != null)
                    {
                        var foreignKey = string.IsNullOrEmpty(foreign.ForeignField) ? field.Name : foreign.ForeignField;
                        var parentField = foreign.ForeignTable.GetField(foreignKey);
                        if (parentField == null)
                            throw new ArgumentNullException(string.Format("{0}.{1} required foreign key {2}.{3} is not exists.", table.Name, field.Name, foreign.ForeignTable.Name, foreignKey));
                        var parentIndex = parentField.GetAttribute<IndexAttribute>();
                        if (parentIndex == null || parentIndex.Index == EIndex.Group)
                            throw new NotSupportedException("Be refferenced foreign field must be a unique key.");
                        //var myIndex = field.GetAttribute<IndexAttribute>();
                        //if (myIndex == null)
                        //    throw new NotSupportedException(string.Format("Foreign key {0}.{1} must have a IndexAttribute.", table.Name, field.Name));
                        var parent = tree.Find(f => f.Field == parentField);
                        if (parent == null)
                        {
                            parent = new TreeField(parentField);
                            tree.Add(parent);
                        }
                        else if (parent.Parent != null && parent.Parent.Field == field)
                            throw new NotSupportedException("Foreign key can't be circular.");

                        // added as other foreign parent
                        var me = tree.Find(f => f.Field == field);
                        if (me == null)
                            me = new TreeField(field);

                        parent.Add(me);
                    }

                    // 自增键
                    var index = field.GetAttribute<IndexAttribute>();
                    if (index != null && index.Index == EIndex.Identity)
                    {
                        List<FieldInfo> list;
                        if (!identities.TryGetValue(table, out list))
                        {
                            list = new List<FieldInfo>();
                            identities[table] = list;
                        }
                        list.Add(field);
                    }
                }
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Data;");
            builder.AppendLine("using System.Collections;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Linq;");
            builder.AppendLine("using System.Reflection;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine("using EntryEngine;");
            builder.AppendLine("using EntryEngine.Serialize;");
            builder.AppendLine("using EntryEngine.Network;");
            foreach (var ns in types.Select(t => t.Namespace).Distinct())
                if (!string.IsNullOrEmpty(ns))
                    builder.AppendLine("using {0};", ns);
            builder.AppendLine();

            if (string.IsNullOrEmpty(nsOrEmptyDotDBnameOrEmpty))
            {
                // 默认文件名作为类型名字
                nsOrEmptyDotDBnameOrEmpty = Path.GetFileNameWithoutExtension(outputCS);
            }
            else
            {
                int dot = nsOrEmptyDotDBnameOrEmpty.LastIndexOf('.');
                if (dot == -1)
                {
                    // 空命名空间
                    //builder.AppendLine("namespace {0}", nsOrEmptyDotDBnameOrEmpty);
                }
                else
                {
                    builder.AppendLine("namespace {0}", nsOrEmptyDotDBnameOrEmpty.Substring(0, dot));
                    nsOrEmptyDotDBnameOrEmpty = nsOrEmptyDotDBnameOrEmpty.Substring(dot + 1);
                }
            }
            builder.AppendLine("{");

            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
            // build enum of table fields
            foreach (var table in types)
            {
                builder.AppendLine("public enum E{0}", table.Name);
                builder.AppendBlock(() =>
                {
                    var fields = table.GetFields(flag);
                    foreach (var field in fields)
                    {
                        builder.AppendSummary(field);
                        builder.AppendLine("{0},", field.Name);
                    }
                });
            }

            // build mysql structure
            if (string.IsNullOrEmpty(mysqlClassOrEmpty))
            {
                mysqlClassOrEmpty = "MYSQL_DATABASE";
                builder.AppendLine("public class {0} : _DATABASE.Database", mysqlClassOrEmpty);
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("protected override System.Data.IDbConnection CreateConnection()");
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("var conn = new MySql.Data.MySqlClient.MySqlConnection();");
                        builder.AppendLine("conn.ConnectionString = ConnectionString;");
                        builder.AppendLine("conn.Open();");
                        builder.AppendLine("return conn;");
                    });
                });
            }

            string _static = isStatic ? "static " : string.Empty;
            // build database operation
            builder.AppendLine("public {1}partial class {0}", nsOrEmptyDotDBnameOrEmpty, _static);
            builder.AppendBlock(() =>
            {
                // build mysql structure
                //builder.AppendLine("class MYSQL_TABLE_COLUMN");
                //builder.AppendBlock(() =>
                //{
                //    builder.AppendLine("public string COLUMN_NAME;");
                //    //builder.AppendLine("public string COLUMN_TYPE;");
                //    builder.AppendLine("public string COLUMN_KEY;");
                //    builder.AppendLine("public string EXTRA;");

                //    builder.AppendLine("public bool IsPrimary { get { return COLUMN_KEY == \"PRI\"; } }");
                //    builder.AppendLine("public bool IsIndex { get { return COLUMN_KEY == \"MUL\"; } }");
                //    builder.AppendLine("public bool IsUnique { get { return COLUMN_KEY == \"UNI\"; } }");
                //    builder.AppendLine("public bool IsIdentity { get { return EXTRA == \"auto_increment\"; } }");
                //});

                // database instance
                builder.AppendLine("public {0}string DatabaseName;", _static);
                builder.AppendLine("public {0}Action<_DATABASE.Database> OnConstructDatabase;", _static);
                builder.AppendLine("public static List<MergeTable> AllMergeTable = new List<MergeTable>()");
                builder.AppendLine("{");
                foreach (var item in types)
                    builder.AppendLine("new MergeTable(\"{0}\"),", item.Name);
                builder.AppendLine("};");
                builder.AppendLine("private {0}_DATABASE.Database _dao;", _static);
                builder.AppendSummary("Set this will set the event 'OnCreateConnection' and 'OnTestConnection'");
                builder.AppendLine("public {0}_DATABASE.Database _DAO", _static);
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("get { if (_dao == null) return _DATABASE._Database; else return _dao; }");
                    builder.AppendLine("set");
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("if (_dao == value) return;");
                        //builder.AppendLine("if (_dao != null)");
                        //builder.AppendBlock(() =>
                        //{
                        //    builder.AppendLine("value.OnCreateConnection = null;");
                        //    builder.AppendLine("value.OnTestConnection = null;");
                        //});
                        builder.AppendLine("_dao = value;");
                        builder.AppendLine("if (value != null)");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("value.OnCreateConnection = CREATE_CONNECTION;");
                            builder.AppendLine("value.OnTestConnection = UPDATE_DATABASE_STRUCTURE;");
                        });
                    });
                });

                // create database
                builder.AppendSummary("Set this to the _DATABASE.Database.OnCreateConnection event");
                builder.AppendLine("public {0}void CREATE_CONNECTION(System.Data.IDbConnection conn, _DATABASE.Database database)", _static);
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("if (string.IsNullOrEmpty(conn.Database) && !string.IsNullOrEmpty(DatabaseName) && !_DAO.Available)");
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("var cmd = conn.CreateCommand();");
                        builder.AppendLine("cmd.CommandText = string.Format(\"CREATE DATABASE IF NOT EXISTS `{0}`;\", DatabaseName);");
                        builder.AppendLine("int create = cmd.ExecuteNonQuery();");
                        builder.AppendLine("conn.ChangeDatabase(DatabaseName);");
                        builder.AppendLine("_DAO.ConnectionString += string.Format(\"Database={0};\", DatabaseName);");
                        builder.AppendLine("_DAO.OnCreateConnection -= CREATE_CONNECTION;");
                        builder.AppendLine("if (create > 0)");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("_LOG.Info(\"Create database[`{0}`].\", DatabaseName);");
                        });
                        builder.AppendLine("_LOG.Info(\"Set database[`{0}`].\", DatabaseName);");
                    });
                });
                // update database structure
                builder.AppendSummary("Set this to the _DATABASE.Database.OnTestConnection event");
                builder.AppendLine("public static void UPDATE_DATABASE_STRUCTURE(System.Data.IDbConnection conn, _DATABASE.Database database)");
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("var cmd = conn.CreateCommand();");
                    builder.AppendLine("cmd.CommandTimeout = database.Timeout;");
                    builder.AppendLine("cmd.CommandText = string.Format(\"SELECT EXISTS (SELECT 1 FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = '{0}');\", conn.Database);");
                    builder.AppendLine("bool __exists = Convert.ToBoolean(cmd.ExecuteScalar());");

                    #region 创建数据表结构

                    builder.AppendLine("#region Create table");
                    builder.AppendLine("cmd.CommandText =");
                    builder.AppendLine("@\"");
                    // 创建数据表
                    foreach (var table in types)
                    {
                        builder.AppendLine("CREATE TABLE IF NOT EXISTS `{0}`", table.Name);
                        builder.AppendLine("(");
                        // fields
                        var fields = table.GetFields(flag);
                        var primary = fields.Where(f =>
                        {
                            var index = f.GetAttribute<IndexAttribute>();
                            return index != null && index.Index == EIndex.Primary;
                        }).ToArray();

                        if (primary.Length > 1)
                        {
                            var p2 = primary.Where(p => p.GetAttribute<ForeignAttribute>() == null);
                            // NotSupportedException: multiple primary key can't be refferenced by foreign key
                            if (tree.Find(f => p2.Contains(f.Field)) != null)
                                throw new NotSupportedException("Multiple primary key can't be refferenced by foreign key.");
                        }

                        fields.ForeachExceptLast(
                        (field) =>
                        {
                            builder.Append("`{0}` {1}", field.Name, GetMySqlType(field.FieldType));
                            IndexAttribute index = field.GetAttribute<IndexAttribute>();
                            if (index != null && index.Index == EIndex.Identity)
                            {
                                builder.Append(" PRIMARY KEY AUTO_INCREMENT");
                                if (primary.Length > 0)
                                    throw new NotSupportedException("Can't has other primary key where table column has AUTO_INCREMENT as a primary key.");
                            }
                        },
                        (field) =>
                        {
                            builder.AppendLine(",");
                        });
                        builder.AppendLine();
                        builder.AppendLine(");");
                    }
                    builder.AppendLine("\";");
                    builder.AppendLine("_LOG.Info(\"Begin create table.\");");
                    builder.AppendLine("cmd.ExecuteNonQuery();");
                    builder.AppendLine("_LOG.Info(\"Create table completed.\");");
                    builder.AppendLine("#endregion");

                    #endregion

                    #region 修改表结构

                    builder.AppendLine();
                    builder.AppendLine("Dictionary<string, MYSQL_TABLE_COLUMN> __columns = new Dictionary<string, MYSQL_TABLE_COLUMN>();");
                    builder.AppendLine("MYSQL_TABLE_COLUMN __value;");
                    builder.AppendLine("bool __noneChangePrimary;");
                    builder.AppendLine("bool __hasPrimary;");
                    builder.AppendLine("IDataReader reader;");
                    builder.AppendLine("StringBuilder builder = new StringBuilder();");

                    foreach (var table in types)
                    {
                        builder.AppendLine();
                        builder.AppendLine("#region Table structure \"{0}\"", table.Name);
                        builder.AppendLine("_LOG.Info(\"Begin update table[`{0}`] structure.\");", table.Name);
                        builder.AppendLine("__columns.Clear();");
                        builder.AppendLine("builder.Remove(0, builder.Length);");
                        //builder.AppendLine("builder = new StringBuilder();");
                        //builder.AppendLine("cmd = conn.CreateCommand();");
                        builder.AppendLine("cmd.CommandText = \"SELECT COLUMN_NAME, COLUMN_KEY, EXTRA FROM information_schema.COLUMNS WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '\" + conn.Database + \"';\";", table.Name, nsOrEmptyDotDBnameOrEmpty);
                        builder.AppendLine("reader = cmd.ExecuteReader();");
                        builder.AppendLine("__hasPrimary = false;");
                        builder.AppendLine("foreach (var __column in _DATABASE.ReadMultiple<MYSQL_TABLE_COLUMN>(reader))");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("if (__column.IsPrimary) __hasPrimary = true;");
                            builder.AppendLine("__columns.Add(__column.COLUMN_NAME, __column);");
                        });
                        builder.AppendLine("reader.Close();");

                        // 不支持：字段改名；
                        // 支持：增删字段，修改字段类型；增删索引
                        var fields = table.GetFields(flag);
                        var primary = fields.Where(f =>
                        {
                            var index = f.GetAttribute<IndexAttribute>();
                            return index != null && index.Index == EIndex.Primary;
                        }).ToArray();

                        // 更换主键
                        builder.AppendLine("__noneChangePrimary = true;");
                        for (int i = 0; i < primary.Length; i++)
                            builder.AppendLine("__noneChangePrimary &= (__columns.TryGetValue(\"{0}\", out __value) && __value.IsPrimary);", primary[i].Name);
                        builder.AppendLine("if (!__noneChangePrimary && __hasPrimary)");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("builder.AppendLine(\"ALTER TABLE `{0}` DROP PRIMARY KEY;\");", table.Name);
                            builder.AppendLine("_LOG.Debug(\"Drop primary key.\");");
                        });

                        // 删除旧索引
                        //builder.AppendLine("__hasPrimary = false;");
                        //builder.AppendLine("foreach (var __column in __columns.Values)");
                        //builder.AppendBlock(() =>
                        //{
                        //    builder.AppendLine("if (!__hasPrimary && __column.IsPrimary)");
                        //    builder.AppendBlock(() =>
                        //    {
                        //        builder.AppendLine("__hasPrimary = true;");
                        //        builder.AppendLine("if (!__column.IsIdentity)");
                        //        builder.AppendBlock(() =>
                        //        {
                        //            builder.AppendLine("builder.AppendLine(\"ALTER TABLE `{0}` DROP PRIMARY KEY;\");", table.Name);
                        //            builder.AppendLine("_LOG.Debug(\"Drop primary key.\");");
                        //        });
                        //    });
                        //    builder.AppendLine("if (__column.IsIndex || __column.IsUnique)");
                        //    builder.AppendBlock(() =>
                        //    {
                        //        builder.AppendLine("builder.AppendLine(\"ALTER TABLE {0} DROP INDEX `{{0}}`;\", __column.COLUMN_NAME);", table.Name);
                        //        builder.AppendLine("_LOG.Debug(\"Drop index[`{0}`].\", __column.COLUMN_NAME);");
                        //    });
                        //});
                        //builder.AppendLine();

                        //builder.AppendLine();
                        // 添加修改字段
                        for (int i = 0; i < fields.Length; i++)
                        {
                            StringBuilder temp = new StringBuilder();
                            var field = fields[i];
                            // 修改表顺序将重构表结构，跟增加删除表字段一样，需要较多时间
                            //temp.Append("builder.AppendLine(\"ALTER TABLE `{1}` {{0}} `{0}` {2}{{1}} {3}", field.Name, table.Name, GetMySqlType(field.FieldType), (i == 0 ? "FIRST" : "AFTER `" + fields[i - 1].Name + "`"));
                            temp.Append("builder.AppendLine(\"ALTER TABLE `{1}` {{0}} `{0}` {2}{{1}}", field.Name, table.Name, GetMySqlType(field.FieldType));
                            IndexAttribute index = field.GetAttribute<IndexAttribute>();
                            if (index != null && index.Index == EIndex.Identity)
                                temp.Append(" AUTO_INCREMENT");
                            temp.Append(";\");");

                            //string result = temp.ToString();
                            //builder.Append("if (__columns.Remove(\"{0}\")) ", field.Name);
                            //builder.AppendLine(result, "CHANGE COLUMN `" + field.Name + "`", "");
                            //builder.Append("else ");
                            //builder.AppendLine(result, "ADD COLUMN", (index != null && index.Index == EIndex.Identity ? " PRIMARY KEY" : ""));

                            string result = temp.ToString();
                            builder.AppendLine("if (__columns.TryGetValue(\"{0}\", out __value))", field.Name);
                            builder.AppendBlock(() =>
                            {
                                builder.AppendLine(result, "CHANGE COLUMN `" + field.Name + "`", "");
                                if (index != null && (index.Index == EIndex.Index || index.Index == EIndex.Group))
                                {
                                    // ADD_INDEX: 当前是索引，以前不是索引时添加索引
                                    builder.AppendLine("if (!__value.IsIndex && !__value.IsUnique)");
                                    builder.AppendBlock(() =>
                                    {
                                        builder.AppendLine("builder.AppendLine(\"ALTER TABLE `{0}` ADD INDEX(`{1}`{2});\");", table.Name, field.Name, field.FieldType == typeof(string) ? "(10)" : string.Empty);
                                        builder.AppendLine("_LOG.Debug(\"Add index[`{0}`].\", __value.COLUMN_NAME);");
                                    });
                                }
                                else
                                {
                                    // 当前不是索引，以前是索引时删除索引
                                    builder.AppendLine("if (__value.IsIndex || __value.IsUnique)");
                                    builder.AppendBlock(() =>
                                    {
                                        builder.AppendLine("builder.AppendLine(\"ALTER TABLE {0} DROP INDEX `{{0}}`;\", __value.COLUMN_NAME);", table.Name);
                                        builder.AppendLine("_LOG.Debug(\"Drop index[`{0}`].\", __value.COLUMN_NAME);");
                                    });
                                }
                                builder.AppendLine("__columns.Remove(__value.COLUMN_NAME);");
                            });
                            builder.AppendLine("else");
                            builder.AppendBlock(() =>
                            {
                                builder.AppendLine(result, "ADD COLUMN", (index != null && index.Index == EIndex.Identity ? " PRIMARY KEY" : ""));
                                if (index != null && (index.Index == EIndex.Index || index.Index == EIndex.Group))
                                {
                                    // ADD_INDEX: 添加索引
                                    builder.AppendLine("builder.AppendLine(\"ALTER TABLE `{0}` ADD INDEX(`{1}`{2});\");", table.Name, field.Name, field.FieldType == typeof(string) ? "(10)" : string.Empty);
                                    builder.AppendLine("_LOG.Debug(\"Add index[`{{0}}`].\", \"{0}\");", field.Name);
                                }
                                builder.AppendLine("_LOG.Debug(\"Add column[`{{0}}`].\", \"{0}\");", field.Name);
                            });
                        }
                        // 删除字段
                        builder.AppendLine("foreach (var __column in __columns.Keys)");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("builder.AppendLine(\"ALTER TABLE `{0}` DROP COLUMN `{{0}}`;\", __column);", table.Name);
                            builder.AppendLine("_LOG.Debug(\"Drop column[`{0}`].\", __column);");
                        });

                        // 添加主键 & 索引
                        if (primary.Length > 0)
                        {
                            builder.AppendLine("if (!__noneChangePrimary)");
                            builder.AppendBlock(() =>
                            {
                                builder.Append("builder.AppendLine(\"ALTER TABLE `{0}` ADD PRIMARY KEY (", table.Name);
                                primary.ForeachExceptLast(
                                    (field) => builder.Append("`{0}`{1}", field.Name,
                                        // 变长的类型建立索引需要指定长度 BLOB/TEXT column 'key' used in key specification without a key length
                                        // `key` TEXT, PRIMARY KEY (`key`(10))
                                        field.FieldType == typeof(string) ? "(10)" : string.Empty),
                                    (field) => builder.Append(","));
                                builder.AppendLine(");\");");
                                builder.AppendLine("_LOG.Debug(\"Add primary key[{{0}}].\", \"{0}\");", string.Join(",", primary.Select(p => p.Name).ToArray()));
                            });
                        }
                        //foreach (var field in fields)
                        //{
                        //    IndexAttribute index = field.GetAttribute<IndexAttribute>();
                        //    if (index != null && (index.Index == EIndex.Index || index.Index == EIndex.Group))
                        //    {
                        //        builder.AppendLine("builder.AppendLine(\"ALTER TABLE `{0}` ADD INDEX(`{1}`{2});\");", table.Name, field.Name, field.FieldType == typeof(string) ? "(10)" : string.Empty);
                        //        builder.AppendLine("_LOG.Debug(\"Add index[{0}].\");", field.Name);
                        //    }
                        //}

                        builder.AppendLine();
                        builder.AppendLine("cmd.CommandText = builder.ToString();");
                        builder.AppendLine("_LOG.Info(\"Building table[`{0}`] structure.\");", table.Name);
                        builder.AppendLine("cmd.ExecuteNonQuery();");
                        builder.AppendLine("#endregion");
                    }

                    #endregion

                    builder.AppendLine("if (!__exists) ");
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("_LOG.Info(\"The first time to construct database.\");");
                        builder.AppendLine("if (OnConstructDatabase != null) OnConstructDatabase(database);");
                    });
                });

                #region 合并数据库
                builder.AppendSummary(string.Format("/* Phase说明 */ BuildTemp: 原库，可用于延长Timeout，{0}操作原库 / ChangeTemp: 临时库，可用于修改主键可能重复的数据，{0}操作临时库 / Merge: 临时库，可用于修改需要参考其它合服数据的数据，{0}操作目标库", nsOrEmptyDotDBnameOrEmpty));
                builder.AppendLine("public {0}void MERGE(MergeDatabase[] dbs, Action<_DATABASE.Database> phaseBuildTemp, Action<_DATABASE.Database> phaseChangeTemp, Action<_DATABASE.Database[]> phaseMerge)", _static);
                builder.AppendBlock(() =>
                {
                    builder.AppendLine("_DATABASE.Database __target = _DAO;");
                    builder.AppendLine("if (__target == null) throw new ArgumentNullException(\"_DAO\");");
                    builder.AppendLine("if (__target.Available) throw new InvalidOperationException(\"_DAO can't be available.\");");
                    builder.AppendLine("_DATABASE.Database[] sources = new _DATABASE.Database[dbs.Length];");

                    // 自动合并自增列用
                    foreach (var item in identities)
                        foreach (var field in item.Value)
                            builder.AppendLine("{0} __{1}_{2} = 0;", field.FieldType.CodeName(), item.Key.Name, field.Name);

                    builder.AppendLine();
                    builder.AppendLine("#region create temp database");
                    #region 创建临时数据库

                    builder.AppendLine("for (int i = 0; i < sources.Length; i++)");
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("_DATABASE.Database db = new ConnectionPool() {{ Base = new {0}() }};", mysqlClassOrEmpty);
                        builder.AppendLine("db.ConnectionString = dbs[i].ConnectionStringWithDB;");
                        // 创建数据库 & 数据表
                        builder.AppendLine("db.OnTestConnection = (__conn, __db) =>");
                        builder.AppendBlockWithEnd(() =>
                        {
                            builder.AppendLine("string __temp = \"TEMP_\" + db.DatabaseName;");
                            builder.AppendLine("db.ExecuteNonQuery(string.Format(\"DROP DATABASE IF EXISTS `{0}`; CREATE DATABASE `{0}`;\", __temp));");
                            builder.AppendLine("__conn.ChangeDatabase(__temp);");
                        });
                        builder.AppendLine("db.OnTestConnection += UPDATE_DATABASE_STRUCTURE;");
                        builder.AppendLine("db.OnTestConnection += (__conn, __db) => __conn.ChangeDatabase(db.DatabaseName);");
                        builder.AppendLine("db.TestConnection();");
                        builder.AppendLine("_DAO = db;");
                        builder.AppendLine("if (phaseBuildTemp != null) phaseBuildTemp(db);");
                        builder.AppendLine("sources[i] = db;");
                        builder.AppendLine("string dbName = db.DatabaseName;");
                        builder.AppendLine("string tempName = \"TEMP_\" + dbName;");
                        builder.AppendLine("_LOG.Info(\"Begin build temp database[{0}].\", dbName);");
                        builder.AppendLine();

                        // 若不指定数据表默认合并全部
                        builder.AppendLine("if (dbs[i].Tables == null) dbs[i].Tables = AllMergeTable.ToArray();");
                        builder.AppendLine();

                        // 筛选数据到临时数据库
                        builder.AppendLine("StringBuilder builder = new StringBuilder();");
                        builder.AppendLine("string result;");
                        builder.AppendLine("MergeTable table;");

                        Action<Type, Action, Action> _mergeTable = (type, a1, a2) =>
                        {
                            builder.AppendLine("table = dbs[i].Tables.FirstOrDefault(t => t.TableName == \"{0}\");", type.Name);
                            builder.AppendLine("if (table != null)");
                            builder.AppendBlock(() =>
                            {
                                if (a1 != null) a1();
                                builder.Append("builder.Append(\"INSERT INTO {0}.{1} SELECT ");
                                var fields = type.GetFields();
                                for (int i = 0, n = fields.Length - 1; i <= n; i++)
                                {
                                    builder.Append("{{1}}.`{0}`", fields[i].Name);
                                    if (i != n)
                                        builder.Append(",");
                                }
                                builder.AppendLine(" FROM {2}.{1}\", tempName, table.TableName, dbName);");
                                builder.AppendLine("if (!string.IsNullOrEmpty(table.Where)) builder.Append(\" \" + table.Where);");
                                if (a2 != null) a2();
                                builder.AppendLine("builder.AppendLine(\";\");");
                                builder.AppendLine("result = builder.ToString();");
                                builder.AppendLine("builder.Remove(0, builder.Length);");
                                builder.AppendLine("_LOG.Info(\"Merge table[`{0}`] data.\", table.TableName);");
                                builder.AppendLine("_LOG.Debug(\"SQL: {0}\", result);");
                                builder.AppendLine("db.ExecuteNonQuery(result);");
                            });
                        };
                        // 主键表优先插入
                        foreach (var item in tree)
                        {
                            _mergeTable(item.Table, null, null);
                        }
                        // 插入其它表
                        foreach (var type in types)
                        {
                            // 跳过已经插入的主键表
                            if (tree.Any(t => t.Table == type))
                                continue;
                            var foreign = type.GetFields().Where(f => f.HasAttribute<ForeignAttribute>()).ToArray();
                            if (foreign.Length > 0)
                            {
                                // 外键表需要插入主键表剩余的数据
                                _mergeTable(type, () =>
                                {
                                    builder.AppendLine("bool __flag = false;");
                                },
                                () =>
                                {
                                    for (int i = 0; i < foreign.Length; i++)
                                    {
                                        var field = foreign[i];
                                        var att = field.GetAttribute<ForeignAttribute>();
                                        builder.AppendLine("if (dbs[i].Tables.Any(t => t.TableName == \"{0}\"))", att.ForeignTable.Name);
                                        builder.AppendBlock(() =>
                                        {
                                            builder.AppendLine("if (!__flag)");
                                            builder.AppendBlock(() =>
                                            {
                                                builder.AppendLine("__flag = true;");
                                                builder.AppendLine("builder.Append(\" WHERE EXISTS \");");
                                                // :INSERT2
                                                builder.AppendLine("builder.Append(\"(SELECT {{0}}.{0}.{1} FROM {{0}}.{0} WHERE {2}.{3} = {{0}}.{0}.{1})\", tempName);", att.ForeignTable.Name, string.IsNullOrEmpty(att.ForeignField) ? field.Name : att.ForeignField, type.Name, field.Name);
                                            });
                                            if (i != 0)
                                            {
                                                builder.AppendLine("else");
                                                builder.AppendBlock(() =>
                                                {
                                                    builder.AppendLine("builder.AppendLine(\" AND \");");
                                                    // :INSERT2
                                                    //builder.AppendLine("builder.Append(\"(SELECT __{0}.{1} FROM __{0} WHERE {2}.{3} = __{0}.{1})\");", att.ForeignTable.Name, string.IsNullOrEmpty(att.ForeignField) ? field.Name : att.ForeignField, type.Name, field.Name);
                                                    builder.AppendLine("builder.Append(\"(SELECT {{0}}.{0}.{1} FROM {{0}}.{0} WHERE {2}.{3} = {{0}}.{0}.{1})\", tempName);", att.ForeignTable.Name, string.IsNullOrEmpty(att.ForeignField) ? field.Name : att.ForeignField, type.Name, field.Name);
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                            else
                                _mergeTable(type, null, null);
                        }

                        builder.AppendLine("_LOG.Info(\"Build temp database[{0}] completed.\", dbName);");
                        builder.AppendLine("db.OnCreateConnection = (conn, __db) => conn.ChangeDatabase(tempName);");
                        builder.AppendLine("// 对临时数据库的表自动修改自增列");
                        // 自动合并自增键
                        foreach (var item in identities)
                        {
                            builder.AppendLine("table = dbs[i].Tables.FirstOrDefault(t => t.TableName == \"{0}\");", item.Key.Name);
                            builder.AppendLine("if (table.AutoMergeIdentity)");
                            builder.AppendBlock(() =>
                            {
                                foreach (var field in item.Value)
                                {
                                    builder.AppendLine("UpdateIdentityKey_{0}_{1}(ref __{0}_{1});", item.Key.Name, field.Name);
                                    builder.AppendLine("_LOG.Info(\"自动修改自增列`{0}`.{1}\");", item.Key.Name, field.Name);
                                }
                            });
                        }
                        builder.AppendLine("if (phaseChangeTemp != null) phaseChangeTemp(db);");
                    });

                    #endregion
                    builder.AppendLine("#endregion");
                    
                    builder.AppendLine();
                    builder.AppendLine("_LOG.Info(\"Build all temp database completed.\");");
                    builder.AppendLine("#region import data in temp database to merge target");
                    #region 将临时数据库数据导入目标库

                    builder.AppendLine();
                    // 修改可能重复的主键
                    builder.AppendLine("if (phaseMerge != null) phaseMerge(sources);");
                    // 新建目标数据库
                    builder.AppendLine("_DAO = __target;");
                    builder.AppendLine("_DAO.OnCreateConnection = (conn, __db) =>");
                    builder.AppendBlockWithEnd(() =>
                    {
                        builder.AppendLine("if (string.IsNullOrEmpty(conn.Database) && !string.IsNullOrEmpty(DatabaseName))");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("var cmd = conn.CreateCommand();");
                            builder.AppendLine("cmd.CommandText = string.Format(\"DROP DATABASE IF EXISTS `{0}`; CREATE DATABASE `{0}`;\", DatabaseName);");
                            builder.AppendLine("cmd.ExecuteNonQuery();");
                            builder.AppendLine("conn.ChangeDatabase(DatabaseName);");
                            builder.AppendLine("_DAO.ConnectionString += string.Format(\"Database={0};\", DatabaseName);");
                        });
                    });
                    builder.AppendLine("_DAO.TestConnection();");
                    builder.AppendLine("for (int i = 0; i < sources.Length; i++)");
                    builder.AppendBlock(() =>
                    {
                        builder.AppendLine("var db = sources[i];");
                        builder.AppendLine("var tables = dbs[i].Tables;");
                        builder.AppendLine("string tempName = \"TEMP_\" + db.DatabaseName;");
                        builder.AppendLine("_LOG.Info(\"Begin merge from temp database[`{0}`].\", db.DatabaseName);");
                        builder.AppendLine("if (db.DataSource == _DAO.DataSource)");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("for (int j = 0; j < tables.Length; j++)");
                            builder.AppendBlock(() =>
                            {
                                builder.AppendLine("_LOG.Debug(\"Merge table[`{0}`].\", tables[j].TableName);");
                                builder.AppendLine("_DAO.ExecuteNonQuery(string.Format(\"INSERT INTO {1} SELECT * FROM {0}.{1};\", tempName, tables[j].TableName));");
                            });
                        });
                        builder.AppendLine("else");
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("StringBuilder builder = new StringBuilder();");
                            builder.AppendLine("MergeTable table;");
                            foreach (var type in types)
                            {
                                builder.AppendLine("table = dbs[i].Tables.FirstOrDefault(t => t.TableName == \"{0}\");", type.Name);
                                builder.AppendLine("if (table != null)");
                                builder.AppendBlock(() =>
                                {
                                    builder.AppendLine("_LOG.Debug(\"Merge table[`{0}`].\", table.TableName);");
                                    builder.AppendLine("var list = db.SelectObjects<{0}>(\"SELECT * FROM {0};\");", type.Name);
                                    builder.AppendLine("if (list.Count > 0)");
                                    builder.AppendBlock(() =>
                                    {
                                        builder.AppendLine("foreach (var item in list)");
                                        builder.AppendBlock(() =>
                                        {
                                            builder.AppendLine("_{0}.Insert(item);", type.Name);
                                        });
                                    });
                                });
                            };
                        });
                        builder.AppendLine("_LOG.Info(\"Merge database[`{0}`] completed!\", db.DatabaseName);");
                        builder.AppendLine("db.ExecuteNonQuery(\"DROP DATABASE \" + tempName);");
                        builder.AppendLine("db.Dispose();");
                    });

                    #endregion
                    builder.AppendLine("#endregion");
                });
                #endregion

                #region 修改键
                tree.ForeachParentPriority(
                    field => field.ChildCount == 0,
                    field =>
                    {
                        if (field == tree)
                            return;
                        // 删除
                        builder.AppendLine("public {3}void DeleteForeignKey_{0}_{1}({2} target)", field.Table.Name, field.Field.Name, field.Field.FieldType.CodeName(), _static);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("StringBuilder builder = new StringBuilder();");
                            TreeField.ForParentPriority(field, null,
                                foreign =>
                                {
                                    builder.AppendLine("builder.AppendLine(\"DELETE FROM `{0}` WHERE `{1}` = @p0;\");", foreign.Table.Name, foreign.Field.Name);
                                });
                            builder.AppendLine("_DAO.ExecuteNonQuery(builder.ToString(), target);");
                        });
                        // 修改
                        builder.AppendLine("public {3}void UpdateForeignKey_{0}_{1}({2} origin, {2} target)", field.Table.Name, field.Field.Name, field.Field.FieldType.CodeName(), _static);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("StringBuilder builder = new StringBuilder();");
                            TreeField.ForParentPriority(field, null,
                                foreign =>
                                {
                                    builder.AppendLine("builder.AppendLine(\"UPDATE `{0}` SET `{1}` = @p0 WHERE `{1}` = @p1;\");", foreign.Table.Name, foreign.Field.Name);
                                });
                            builder.AppendLine("_DAO.ExecuteNonQuery(builder.ToString(), target, origin);");
                        });
                    });
                foreach (var item in identities)
                {
                    foreach (var field in item.Value)
                    {
                        string keyType = field.FieldType.CodeName();
                        // 自增键
                        builder.AppendLine("public {3}void UpdateIdentityKey_{0}_{1}(ref {2} start)", item.Key.Name, field.Name, keyType, _static);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("{0} min = _DAO.ExecuteScalar<{0}>(\"SELECT MIN(`{1}`) FROM `{2}`;\");", keyType, field.Name, item.Key.Name);
                            builder.AppendLine("{0} max = _DAO.ExecuteScalar<{0}>(\"SELECT MAX(`{1}`) FROM `{2}`;\");", keyType, field.Name, item.Key.Name);
                            builder.AppendLine("if (start > 0)");
                            builder.AppendBlock(() =>
                            {
                                builder.AppendLine("if (min > start) min = start - min;");
                                builder.AppendLine("else min = Math.Max(start, max + 1) - min;");
                                builder.AppendLine("start = max + min + 1;");
                            });
                            builder.AppendLine("else");
                            builder.AppendBlock(() =>
                            {
                                builder.AppendLine("start = max + 1;");
                                builder.AppendLine("return;");
                            });
                            builder.AppendLine("StringBuilder builder = new StringBuilder();");
                            builder.AppendLine("builder.AppendLine(\"UPDATE `{0}` SET `{1}` = `{1}` + @p0;\");", item.Key.Name, field.Name);
                            // 外键引用自增列的也需要改
                            TreeField tf = tree.Find(t => t.Field == field);
                            if (tf != null)
                                foreach (var foreign in tf)
                                    builder.AppendLine("builder.AppendLine(\"UPDATE `{0}` SET `{1}` = `{1}` + @p0;\");", foreign.Table.Name, foreign.Field.Name);
                            builder.AppendLine("_DAO.ExecuteNonQuery(builder.ToString(), min);");
                        });
                    }
                }
                #endregion

                #region 针对每张表的其它操作

                foreach (var table in types)
                {
                    if (!isStatic)
                        builder.AppendLine();
                    var fields = table.GetFields(flag);

                    string tableMapperName = "_" + table.Name;

                    #region 自动解析特殊类型

                    bool hasSpecial = false;
                    foreach (var field in fields)
                    {
                        bool special;
                        string type = GetMySqlType(field.FieldType, out special);
                        if (!special)
                            continue;
                        if (!hasSpecial)
                        {
                            if (!isStatic)
                            {
                                tableMapperName = "_" + tableMapperName;
                            }
                            // 创建类型
                            builder.AppendLine("public class {0} : {1}", tableMapperName, table.Name);
                            builder.AppendLine("{");
                        }
                        builder.AppendLine("public new string {0}", field.Name);
                        builder.AppendBlock(() =>
                        {
                            builder.AppendLine("get {{ return JsonWriter.Serialize(base.{0}); }}", field.Name);
                            builder.AppendLine("set {{ base.{0} = JsonReader.Deserialize<{1}>(value); }}", field.Name, field.FieldType.CodeName());
                        });
                        hasSpecial = true;
                    }

                    if (isStatic && !hasSpecial)
                    {
                        // 创建类型
                        builder.AppendLine("public class {0} : {1}", tableMapperName, table.Name);
                        builder.AppendLine("{");
                    }

                    if (hasSpecial)
                    {
                        builder.AppendLine();
                        builder.AppendLine("public {0}() {{ }}", tableMapperName);
                        builder.AppendLine("public {0}({1} __clone)", tableMapperName, table.Name);
                        builder.AppendBlock(() =>
                        {
                            foreach (var field in fields)
                                builder.AppendLine("base.{0} = __clone.{0};", field.Name);
                        });
                    }
                    else
                    {
                        //if (!isStatic)
                            tableMapperName = table.Name;
                    }

                    #endregion

                    #region 数据表类型操作

                    StringBuilder builderOP = new StringBuilder();
                    // 静态就将操作方法直接写入类型中，否则将生成新实例类型来存放操作方法
                    // 以保持静态和非静态的操作代码相同，在静态和非静态改动时可以比较方便
                    // 例如：静态_DB._TUser.Insert => 非静态db._TUser.Insert（仅需要替换静态类型=>实例）
                    if (!isStatic)
                    {
                        // 创建实例类型来操作表
                        builderOP.AppendLine("private ___{0} __ins{0};", table.Name);
                        builderOP.AppendLine("public ___{0} _{0}", table.Name);
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("get");
                            builderOP.AppendBlock(() =>
                            {
                                builderOP.AppendLine("if (__ins{0} == null) __ins{0} = new ___{0}(this);", table.Name);
                                builderOP.AppendLine("return __ins{0};", table.Name);
                            });
                        });
                        builderOP.AppendLine("public class ___{0}", table.Name);
                        builderOP.AppendLine("{");
                        builderOP.AppendLine("private {0} _{0};", nsOrEmptyDotDBnameOrEmpty);
                        builderOP.AppendLine("public _DATABASE.Database _DAO {{ get {{ return _{0}._DAO; }} }}", nsOrEmptyDotDBnameOrEmpty);
                        builderOP.AppendLine("internal ___{0}({1} ___db) {{ this._{1} = ___db; }}", table.Name, nsOrEmptyDotDBnameOrEmpty);
                    }

                    // 字段对应枚举
                    builderOP.AppendLine("private static E{0}[] FIELD_ALL = {{ {1} }};", table.Name,
                        string.Join(", ", fields.Select(f => string.Format("E{0}.{1}", table.Name, f.Name)).ToArray()));
                    //builderOP.AppendLine("private static E{0}[] FIELD_UPDATE = {{ {1} }};", table.Name,
                    //    string.Join(", ", fields.Where(field => !field.HasAttribute<IndexAttribute>() && !field.HasAttribute<ForeignAttribute>()).Select(f => string.Format("E{0}.{1}", table.Name, f.Name)).ToArray()));
                    builderOP.AppendLine("private static E{0}[] FIELD_UPDATE = {{ {1} }};", table.Name,
                        string.Join(", ", fields.Where(FieldCanUpdate).Select(f => string.Format("E{0}.{1}", table.Name, f.Name)).ToArray()));
                    builderOP.AppendLine("public {0}E{1}[] NoNeedField(params E{1}[] noNeed)", _static, table.Name);
                    builderOP.AppendBlock(() =>
                    {
                        builderOP.AppendLine("if (noNeed.Length == 0) return FIELD_ALL;");
                        builderOP.AppendLine("List<E{0}> list = new List<E{0}>(FIELD_ALL.Length);", table.Name);
                        builderOP.AppendLine("for (int i = 0; i < FIELD_ALL.Length; i++)");
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("if (!noNeed.Contains(FIELD_ALL[i])) list.Add(FIELD_ALL[i]);");
                        });
                        builderOP.AppendLine("return list.ToArray();");
                    });
                    builderOP.AppendLine();

                    // 键
                    FieldInfo identity = null;
                    List<FieldInfo> primaryFields = new List<FieldInfo>();
                    List<FieldInfo> groupFields = new List<FieldInfo>();
                    foreach (var f in fields)
                    {
                        var index = f.GetAttribute<IndexAttribute>();
                        if (index != null)
                        {
                            if (index.Index == EIndex.Group)
                            {
                                groupFields.Add(f);
                            }
                            else if (index.Index == EIndex.Primary)
                            {
                                primaryFields.Add(f);
                            }
                            else if (index.Index == EIndex.Identity)
                            {
                                if (identity == null)
                                {
                                    identity = f;
                                }
                                else
                                {
                                    Console.WriteLine("{0}自增键有多个，设计可能不合理", table.Name);
                                }
                                primaryFields.Add(f);
                            }
                        }
                    }

                    // 增
                    builderOP.AppendLine("public {1}void GetInsertSQL({0} target, StringBuilder builder, List<object> values)", table.Name, _static);
                    builderOP.AppendBlock(() =>
                    {
                        builderOP.AppendLine("int index = values.Count;");
                        string target = "target";
                        if (!string.IsNullOrEmpty(sharpIfBuild))
                            builderOP.AppendLine("#if !{0}", sharpIfBuild);
                        if (hasSpecial)
                        {
                            builderOP.AppendLine("{0} _{1} = new {0}({1});", tableMapperName, target);
                            target = "_" + target;
                        }
                        builderOP.Append("builder.Append(\"INSERT `{0}`(", table.Name);
                        StringBuilder temp1 = new StringBuilder();
                        int last = fields.Length - 1;
                        int offset = 0;
                        for (int i = 0; i <= last; i++)
                        {
                            var field = fields[i];

                            var index = field.GetAttribute<IndexAttribute>();
                            if (index != null && index.Index == EIndex.Identity)
                            {
                                offset++;
                                continue;
                            }

                            if (i != offset)
                                builderOP.Append(", ", field.Name);    
                            builderOP.Append("`{0}`", field.Name);
                            temp1.AppendLine("values.Add({0}.{1});", target, field.Name);
                        }
                        builderOP.AppendLine(") VALUES(\");");
                        builderOP.AppendLine("for (int i = 0, n = {0}; i <= n; i++)", last - offset);
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("builder.AppendFormat(\"@p{0}\", index++);");
                            builderOP.AppendLine("if (i != n) builder.Append(\", \");");
                        });
                        builderOP.AppendLine("builder.AppendLine(\");\");");
                        builderOP.Append(temp1.ToString());
                        if (!string.IsNullOrEmpty(sharpIfBuild))
                            builderOP.AppendLine("#endif");
                        //builderOP.AppendLine("return builder.ToString();");
                    });

                    builderOP.AppendLine("public {1}{2} Insert({0} target)", table.Name, _static, identity == null ? "void" : identity.FieldType.CodeName());
                    builderOP.AppendBlock(() =>
                    {
                        builderOP.AppendLine("StringBuilder builder = new StringBuilder();");
                        builderOP.AppendLine("List<object> values = new List<object>({0});", fields.Length);
                        builderOP.AppendLine("GetInsertSQL(target, builder, values);");
                        if (identity != null)
                        {
                            builderOP.AppendLine("builder.Append(\"SELECT LAST_INSERT_ID();\");");
                            builderOP.AppendLine("return _DAO.SelectValue<{0}>(builder.ToString(), values.ToArray());", identity.FieldType.CodeName());
                        }
                        else
                        {
                            builderOP.AppendLine("_DAO.ExecuteNonQuery(builder.ToString(), values.ToArray());");
                        }
                    });

                    // 删
                    if (primaryFields.Count > 0)
                    {
                        // 删除语句
                        builderOP.Append("public {0}void GetDeleteSQL(", _static);
                        for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                        {
                            var field = primaryFields[i];
                            builderOP.Append("{0} {1}", field.FieldType.CodeName(), field.Name);
                            //if (i != n)
                                builderOP.Append(", ");
                        }
                        builderOP.AppendLine("StringBuilder builder, List<object> values)");
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("int index = values.Count;");
                            builderOP.Append("builder.Append(\"DELETE FROM `{0}` WHERE ", table.Name);
                            for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                            {
                                var field = primaryFields[i];
                                builderOP.Append("`{0}` = @p{{0}}", field.Name);
                                if (i != n)
                                    builderOP.Append(" AND ");
                            }
                            builderOP.Append("\"");
                            foreach (var field in primaryFields)
                            {
                                builderOP.Append(", index++", field.Name);
                            }
                            builderOP.AppendLine(");");
                            foreach (var field in primaryFields)
                            {
                                builderOP.AppendLine("values.Add({0});", field.Name);
                            }
                        });

                        // 删除执行
                        builderOP.Append("public {0}int Delete(", _static);
                        for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                        {
                            var field = primaryFields[i];
                            builderOP.Append("{0} {1}", field.FieldType.CodeName(), field.Name);
                            if (i != n)
                                builderOP.Append(", ");
                        }
                        builderOP.AppendLine(")");
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.Append("return _DAO.ExecuteNonQuery(\"DELETE FROM `{0}` WHERE ", table.Name);
                            for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                            {
                                var field = primaryFields[i];
                                builderOP.Append("`{0}` = @p{{{1}}}", field.Name, i);
                                if (i != n)
                                    builderOP.Append(" AND ");
                            }
                            builderOP.Append("\", ");
                            for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                            {
                                var field = primaryFields[i];
                                builderOP.Append("{0}", field.Name);
                                if (i != n)
                                    builderOP.Append(", ");
                            }
                            builderOP.AppendLine(");");
                        });

                        // 单个主键可以作为Group删除
                        if (primaryFields.Count > 1)
                        {
                            foreach (var field in primaryFields)
                            {
                                builderOP.AppendLine("public {0}int DeleteBy{1}({2} {1})", _static, field.Name, field.FieldType.CodeName());
                                builderOP.AppendBlock(() =>
                                {
                                    builderOP.AppendLine("return _DAO.ExecuteNonQuery(\"DELETE FROM `{0}` WHERE `{1}` = @p0;\", {1});", table.Name, field.Name);
                                });
                            }
                        }
                    }
                    if (groupFields.Count > 0)
                    {
                        foreach (var field in groupFields)
                        {
                            builderOP.AppendLine("public {0}int DeleteBy{1}({2} {1})", _static, field.Name, field.FieldType.CodeName());
                            builderOP.AppendBlock(() =>
                            {
                                builderOP.AppendLine("return _DAO.ExecuteNonQuery(\"DELETE FROM `{0}` WHERE `{1}` = @p0;\", {1});", table.Name, field.Name);
                            });
                        }
                    }

                    // 改
                    var canUpdate = fields.Where(FieldCanUpdate).ToArray();
                    if (canUpdate.Length > 0)
                    {
                        builderOP.AppendLine("public {0}void GetUpdateSQL({1} target, string condition, StringBuilder builder, List<object> values, params E{1}[] fields)", _static, table.Name);
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("int index = values.Count;");
                            string target = "target";
                            if (!string.IsNullOrEmpty(sharpIfBuild))
                                builderOP.AppendLine("#if !{0}", sharpIfBuild);
                            if (hasSpecial)
                            {
                                builderOP.AppendLine("{0} _{1} = new {0}({1});", tableMapperName, target);
                                target = "_" + target;
                            }
                            builderOP.AppendLine("if (fields.Length == 0) fields = FIELD_UPDATE;");
                            builderOP.AppendLine("builder.Append(\"UPDATE `{0}` SET\");", table.Name);

                            foreach (var field in canUpdate)
                            {
                                builderOP.AppendLine("if (fields.Contains(E{0}.{1}))", table.Name, field.Name);
                                builderOP.AppendBlock(() =>
                                {
                                    builderOP.AppendLine("builder.Append(\" `{0}` = @p{{0}},\", index++);", field.Name);
                                    builderOP.AppendLine("values.Add({1}.{0});", field.Name, target);
                                });
                            }

                            builderOP.AppendLine("if (index == 0) return;");
                            builderOP.AppendLine("builder[builder.Length - 1] = ' ';");
                            //builderOP.AppendLine("builder.Remove(builder.Length - 1, 1);");
                            builderOP.AppendLine("if (!string.IsNullOrEmpty(condition)) builder.Append(condition);");
                            if (primaryFields.Count > 0)
                            {
                                // 应默认只更新实例的主键对应的实例
                                builderOP.AppendLine("else");
                                builderOP.AppendBlock(() =>
                                {
                                    builderOP.Append("builder.Append(\"WHERE ");
                                    for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                                    {
                                        var field = primaryFields[i];
                                        builderOP.Append("`{0}` = @p{{{1}}}", field.Name, i);
                                        if (i != n)
                                            builderOP.Append(" AND ");
                                    }
                                    builderOP.Append("\"");
                                    for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                                    {
                                        builderOP.Append(", index++");
                                        //if (i != n)
                                        //    builderOP.Append("++, ");
                                    }
                                    builderOP.AppendLine(");");
                                    for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                                    {
                                        builderOP.AppendLine("values.Add(target.{0});", primaryFields[i].Name);
                                    }
                                });
                            }
                            builderOP.AppendLine("builder.AppendLine(\";\");");
                            if (!string.IsNullOrEmpty(sharpIfBuild))
                                builderOP.AppendLine("#endif");
                            //builderOP.AppendLine("return builder.ToString();");
                        });

                        builderOP.AppendSummary("condition that 'where' or 'join' without ';'");
                        builderOP.AppendLine("public {1}void Update({0} target, string condition, params E{0}[] fields)", table.Name, _static);
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("StringBuilder builder = new StringBuilder();");
                            builderOP.AppendLine("List<object> values = new List<object>(fields.Length + {0});", primaryFields.Count);
                            builderOP.AppendLine("GetUpdateSQL(target, condition, builder, values, fields);");
                            builderOP.AppendLine("_DAO.ExecuteNonQuery(builder.ToString(), values.ToArray());");
                        });
                    }

                    // 查
                    builderOP.AppendLine("public {0}StringBuilder GetSelectSQL(params E{1}[] fields)", _static, table.Name);
                    builderOP.AppendBlock(() =>
                    {
                        builderOP.AppendLine("int count = fields.Length;");
                        builderOP.AppendLine("if (count == 0) return new StringBuilder(\"SELECT * FROM `{0}`\");", table.Name);
                        builderOP.AppendLine("StringBuilder builder = new StringBuilder();");
                        builderOP.AppendLine("builder.Append(\"SELECT\");");
                        builderOP.AppendLine("count--;");
                        builderOP.AppendLine("for (int i = 0; i <= count; i++)");
                        builderOP.AppendBlock(() =>
                        {
                            builderOP.AppendLine("builder.Append(\" `{0}`\", fields[i].ToString());");
                            builderOP.AppendLine("if (i != count) builder.Append(\",\");");
                        });
                        builderOP.AppendLine("builder.AppendLine(\" FROM `{0}`\");", table.Name);
                        builderOP.AppendLine("return builder;");
                    });

                    builderOP.Append("public {0}{1} Select(string condition, ", _static, tableMapperName);
                    foreach (var field in primaryFields)
                        builderOP.Append("{0} __{1}, ", field.FieldType.CodeName(), field.Name);
                    builderOP.AppendLine("params E{0}[] fields)", table.Name);
                    builderOP.AppendBlock(() =>
                    {
                        builderOP.AppendLine("StringBuilder builder = GetSelectSQL(fields);");
                        if (primaryFields.Count > 0)
                        {
                            builderOP.Append("bool whereFlag = ");
                            builderOP.Append("string.IsNullOrEmpty(condition)");
                            foreach (var field in primaryFields)
                                builderOP.Append(" || __{0} != default({1})", field.Name, field.FieldType.CodeName());
                            //builderOP.Append(" || __p{0} != {1}", field.Name, _RH.CodeValue(field.FieldType.DefaultValue()));
                            builderOP.AppendLine(";");
                            builderOP.Append("if (whereFlag) builder.Append(\" WHERE ");
                            for (int i = 0, n = primaryFields.Count - 1; i <= n; i++)
                            {
                                var field = primaryFields[i];
                                builderOP.Append("`{0}` = @p{1}", field.Name, i);
                                if (i != n)
                                    builderOP.Append(" AND ");
                            }
                            builderOP.AppendLine("\");");
                        }
                        builderOP.AppendLine("if (!string.IsNullOrEmpty(condition)) builder.Append(\" {0}\", condition);");
                        builderOP.AppendLine("builder.Append(\';\');");
                        //builderOP.AppendLine("if (count == 0) return _DAO.SelectObject<{1}>(string.Format(\"SELECT * FROM {0} {{0}};\", condition));", table.Name, tableMapperName);
                        //builderOP.AppendLine("StringBuilder builder = new StringBuilder();");
                        //builderOP.AppendLine("builder.Append(\"SELECT\");");
                        //builderOP.AppendLine("count--;");
                        //builderOP.AppendLine("for (int i = 0; i <= count; i++)");
                        //builderOP.AppendBlock(() =>
                        //{
                        //    builderOP.AppendLine("builder.Append(\" `{0}`\", fields[i].ToString());");
                        //    builderOP.AppendLine("if (i != count) builder.Append(\",\");");
                        //});
                        //builderOP.AppendLine("builder.AppendLine(\" FROM {0} {{0}};\", condition);", table.Name);
                        if (primaryFields.Count > 0)
                        {
                            builderOP.Append("if (whereFlag) return _DAO.SelectObject<{0}>(builder.ToString()", tableMapperName);
                            foreach (var field in primaryFields)
                                builderOP.Append(", __{0}", field.Name);
                            builderOP.AppendLine(");");
                            builderOP.AppendLine("else return _DAO.SelectObject<{0}>(builder.ToString());", tableMapperName);
                        }
                        else
                            builderOP.AppendLine("return _DAO.SelectObject<{0}>(builder.ToString());", tableMapperName);
                    });

                    builderOP.AppendLine("public {1}List<{0}> SelectMultiple(string condition, params E{0}[] fields)", table.Name, _static);
                    builderOP.AppendBlock(() =>
                    {
                        if (table.Name == tableMapperName)
                            builderOP.AppendLine("if (fields.Length == 0) return _DAO.SelectObjects<{1}>(string.Format(\"SELECT * FROM {0} {{0}};\", condition));", table.Name, tableMapperName);
                        else
                            builderOP.AppendLine("if (fields.Length == 0) return new List<{0}>(_DAO.SelectObjects<{1}>(string.Format(\"SELECT * FROM {0} {{0}};\", condition)).ToArray());", table.Name, tableMapperName);
                        //builderOP.AppendLine("StringBuilder builder = new StringBuilder();");
                        //builderOP.AppendLine("builder.Append(\"SELECT\");");
                        //builderOP.AppendLine("count--;");
                        //builderOP.AppendLine("for (int i = 0; i <= count; i++)");
                        //builderOP.AppendBlock(() =>
                        //{
                        //    builderOP.AppendLine("builder.Append(\" `{0}`\", fields[i].ToString());");
                        //    builderOP.AppendLine("if (i != count) builder.Append(\",\");");
                        //});
                        //builderOP.AppendLine("builder.Append(\" FROM {0} {{0}};\", condition);", table.Name);
                        builderOP.AppendLine("StringBuilder builder = GetSelectSQL(fields);");
                        builderOP.AppendLine("if (!string.IsNullOrEmpty(condition)) builder.Append(\" {0}\", condition);");
                        builderOP.AppendLine("builder.Append(\';\');");
                        if (table.Name == tableMapperName)
                            builderOP.AppendLine("return _DAO.SelectObjects<{0}>(builder.ToString());", tableMapperName);
                        else
                        {
                            builderOP.AppendLine("List<{0}> __temp = _DAO.SelectObjects<{0}>(builder.ToString());", tableMapperName);
                            builderOP.AppendLine("return new List<{0}>(__temp.ToArray());", table.Name);
                        }
                    });
                    if (groupFields.Count > 0 || primaryFields.Count > 1)
                    {
                        IEnumerable<FieldInfo> group = null;
                        if (groupFields.Count > 0 && primaryFields.Count > 1)
                            group = groupFields.Concat(primaryFields);
                        else if (groupFields.Count > 0)
                            group = groupFields;
                        else
                            group = primaryFields;
                        foreach (var field in group)
                        {
                            builderOP.AppendLine("public {1}List<{0}> SelectMultipleBy{2}({3} {2}, params E{0}[] fields)", table.Name, _static, field.Name, field.FieldType.CodeName());
                            builderOP.AppendBlock(() =>
                            {
                                builderOP.AppendLine("StringBuilder builder = GetSelectSQL(fields);");
                                builderOP.AppendLine("builder.Append(\"WHERE `{0}` = @p0;\");", field.Name);
                                if (table.Name == tableMapperName)
                                    builderOP.AppendLine("return _DAO.SelectObjects<{0}>(builder.ToString(), {1});", tableMapperName, field.Name);
                                else
                                {
                                    builderOP.AppendLine("List<{0}> __temp = _DAO.SelectObjects<{0}>(builder.ToString(), {1});", tableMapperName, field.Name);
                                    builderOP.AppendLine("return new List<{0}>(__temp.ToArray());", table.Name);
                                }
                            });
                        }
                    }

                    // ___类型结束
                    if (!isStatic)
                        builderOP.AppendLine("}");

                    #endregion

                    if (hasSpecial && !isStatic)
                        // 拥有特殊类型字段的数据库映射类型结束
                        builder.AppendLine("}");
                    // 数据库操作方法
                    builder.Append(builderOP);
                    if (isStatic)
                        // 静态时操作方法肯定在映射类中
                        builder.AppendLine("}");
                }
                #endregion
            });
            builder.AppendLine("}");

            SaveCode(outputCS, builder);
        }
        public static void BuildLinkStruct(string dllAndType, string className, string outputCS, byte modifierLevel)
		{
			Type type = GetDllType(dllAndType);

			StringBuilder builder = new StringBuilder();
			// using ;
			builder.AppendLine();
			// namespace
			if (!string.IsNullOrEmpty(type.Namespace))
				builder.AppendLine("namespace {0}", type.Namespace);
			builder.AppendBlock(() =>
			{
				BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
				IEnumerable<PropertyInfo> properties = type.GetAllProperties(flag).WithoutIndex().Where(p => p.CanOverride());
                IEnumerable<MethodInfo> methods = type.GetAllMethods(flag).MethodOnly().DeclareTypeNotObject().Where(m => m.CanOverride());

				#region base

				if (string.IsNullOrEmpty(className))
				{
					className = type.Name;
					if (type.IsInterface && className.StartsWith("I"))
						className = className.Substring(1);
					className = className + "_Link";
				}

				builder.AppendLine("{0} abstract class {1} : {2}", type.GetModifier(), className, type.CodeName());
				builder.AppendBlock(() =>
				{
					// property
					builder.AppendLine("public virtual {0} Base {{ get; set; }}", type.CodeName());
					foreach (var item in properties)
					{
						string modifier = item.GetModifier();
                        if (_RH.Modifier.IndexOf(modifier) > 1)
                            continue;
                        string implement;
                        if (item.DeclaringType.IsInterface)
                            implement = "virtual";
                        else
                            implement = "override";
                        builder.AppendLine("{0} {3} {1} {2}", modifier, item.PropertyType.CodeName(), item.Name, implement);
						builder.AppendBlock(() =>
						{
							if (item.CanRead)
							{
								string temp = item.GetGetMethod(true).GetModifier();
								builder.AppendLine("{0} get {{ return Base.{1}; }}", (temp == modifier ? "" : temp), item.Name);
							}
							if (item.CanWrite)
							{
								string temp = item.GetSetMethod(true).GetModifier();
								builder.AppendLine("{0} set {{ Base.{1} = value; }}", (temp == modifier ? "" : temp), item.Name);
							}
						});
					}
					builder.AppendLine();

					// constructor
					builder.AppendLine("public {0}() {{ }}", className);
					builder.AppendLine("public {0}({1} Base) {{ this.Base = Base; }}", className, type.CodeName());
					builder.AppendLine();

					// methods
                    if (modifierLevel > 2)
                        modifierLevel = 2;
					foreach (var item in methods)
					{
                        if (_RH.Modifier.IndexOf(item.GetModifier()) > modifierLevel)
                            continue;
                        string implement;
                        if (item.DeclaringType.IsInterface)
                            implement = "virtual";
                        else
                            implement = "override";
						builder.AppendMethodDefine(item, implement);
						builder.AppendBlock(() =>
						{
							builder.AppendMethodInvoke(item, "Base");
						});
					}
				});

				#endregion
			});

            File.WriteAllText(outputCS, _RH.Indent(builder.ToString()), Encoding.UTF8);
		}
        /// <summary>1. 静态类里有多个接口，且接口方法属性等有重名时，需要生成类似于深度调用的实例名</summary>
        public static void BuildStaticInterface(string dllAndType, string outputCS, bool preCompile, string preCondition)
		{
			Type type = GetDllType(dllAndType);
			//if (!type.IsStatic())
			//    throw new InvalidOperationException("Type must be a static class.");

			StringBuilder builder = new StringBuilder();
            builder.AppendSharpIfCompile(preCondition, () =>
            {
                // using
                AppendDefaultNamespace(builder);
                builder.AppendLine();

                // namespace
                builder.AppendLine("namespace {0}", type.Namespace);
                builder.AppendBlock(() =>
                {
                    if (type.IsStatic())
                        builder.AppendLine("static partial class {0}", type.Name);
                    else
                        builder.AppendLine("partial class {0}", type.Name);
                    //var interfaces = type.GetInnerTypes().Where(t => t.IsInterface && !t.IsStatic());
                    var interfaces = type.GetInnerTypes().Where(t => !t.IsStatic());
                    builder.AppendBlock(() =>
                    {
                        foreach (var item in interfaces)
                        {
                            string name = item.Name;
                            if (item.IsInterface && name.StartsWith("I"))
                                name = name.Substring(1);
                            name = "_" + name;
                            builder.Append("{0} static {1} {2}", item.GetModifier(), item.Name, name);
                            bool defaultFlag = false;
                            //object[] attributes = item.GetCustomAttributes(false);
                            var dv = item.GetAttribute<ADefaultValue>(false);
                            if (dv != null && dv.DefaultValue != null)
                            {
                                var constructor = dv.DefaultValue.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                                if (constructor != null)
                                {
                                    builder.Append(" = new {0}", dv.DefaultValue.CodeName());
                                    builder.AppendMethodInvoke(constructor);
                                    defaultFlag = true;
                                }
                            }
                            if (!defaultFlag)
                                builder.AppendLine(";");
                        }

                        foreach (var member in interfaces)
                        {
                            builder.AppendLine();

                            string name = member.Name;
                            if (member.IsInterface && name.StartsWith("I"))
                                name = name.Substring(1);
                            name = "_" + name;

                            var properties = member.GetProperties().WithoutIndex();
                            var methods = member.GetAllMethods(BindingFlags.Instance | BindingFlags.Public).DeclareTypeNotObject().MethodOnly();
                            //var methods = member.GetAllMethods().MethodOnly();

                            foreach (var item in properties)
                            {
                                builder.AppendLine("public static {0} {1}", item.PropertyType.CodeName(), item.Name);
                                builder.AppendBlock(() =>
                                {
                                    if (item.CanRead && item.GetGetMethod(false) != null)
                                    {
                                        builder.AppendLine("get");
                                        builder.AppendBlock(() =>
                                        {
                                            if (preCompile)
                                            {
                                                builder.AppendSharpIfThrow(() =>
                                                {
                                                    builder.AppendLine("return {0}.{1};", name, item.Name);
                                                });
                                            }
                                            else
                                            {
                                                builder.AppendLine("return {0}.{1};", name, item.Name);
                                            }
                                        });
                                    }
                                    if (item.CanWrite && item.GetSetMethod(false) != null)
                                    {
                                        builder.AppendLine("set");
                                        builder.AppendBlock(() =>
                                        {
                                            if (preCompile)
                                            {
                                                builder.AppendSharpIfThrow(() =>
                                                {
                                                    builder.AppendLine("{0}.{1} = value;", name, item.Name);
                                                });
                                            }
                                            else
                                            {
                                                builder.AppendLine("{0}.{1} = value;", name, item.Name);
                                            }
                                        });
                                    }
                                });
                            }
                            foreach (var item in methods)
                            {
                                builder.AppendMethodDefine(item, "static");
                                builder.AppendBlock(() =>
                                {
                                    if (preCompile)
                                    {
                                        builder.AppendSharpIfThrow(() =>
                                        {
                                            builder.AppendMethodInvoke(item, name);
                                        });
                                    }
                                    else
                                    {
                                        builder.AppendMethodInvoke(item, name);
                                    }
                                });
                            }
                        }
                    });
                });
            });

			SaveCode(outputCS, builder);
		}
		public static void BuildLinkShell(string inputDirOrFile, string dotnetCompilerVersion, byte depth, string outputFile, string x86, string overdueTime)
		{
			object[] exe = null;
			byte[] result = null;
			byte[] bytes;
			// shell off
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine("using System.Reflection;");
			builder.AppendLine("using System.Linq;");
			builder.AppendLine();
			builder.AppendLine("public class Shell");
			builder.AppendBlock(() =>
			{
				builder.AppendLine("public static void Next(byte[] bytes)");
				builder.AppendBlock(() =>
				{
                    DestructSelf(overdueTime, builder);

					string[] files = GetFiles(inputDirOrFile, "*.dll", SearchOption.TopDirectoryOnly).Concat(
						GetFiles(inputDirOrFile, "*.exe", SearchOption.TopDirectoryOnly)).ToArray();
					builder.AppendLine("byte[] raw, symbol;");
					builder.AppendLine("Assembly[] asses = new Assembly[{0}];", files.Length);
					int position = 0;
					for (int i = 0; i < files.Length; i++)
					{
						string file = files[i];
						bytes = File.ReadAllBytes(file);
						builder.AppendLine("raw = new byte[{0}];", bytes.Length);
						builder.AppendLine("Array.Copy(bytes, {0}, raw, 0, {1});", position, bytes.Length);
						position += bytes.Length;
						result = result.Add(bytes);

						string pdb = Path.ChangeExtension(file, "pdb");
						if (File.Exists(pdb))
						{
							byte[] pdbs = File.ReadAllBytes(pdb);
							builder.AppendLine("symbol = new byte[{0}];", pdbs.Length);
							builder.AppendLine("Array.Copy(bytes, {0}, symbol, 0, {1});", position, pdbs.Length);
							position += pdbs.Length;
							result = result.Add(pdbs);
						}
						else
							builder.AppendLine("symbol = null;");

						builder.AppendLine("asses[{0}] = AppDomain.CurrentDomain.Load(raw, symbol);", i);

						try
						{
							var load = Assembly.Load(bytes);
							if (load.EntryPoint != null)
							{
								exe = load.EntryPoint.GetCustomAttributes(true);
							}
						}
						catch (Exception)
						{
                            Console.WriteLine("加载入口Dll异常，编译选项采用x86");
							x86 = "x86";
						}
					}
					builder.AppendLine("AppDomain.CurrentDomain.AssemblyResolve += (sender, e) => asses.FirstOrDefault(a => a.FullName == e.Name);");
					// if has EntryPoint will run the application
					builder.AppendLine("foreach (var assembly in asses)");
					builder.AppendBlock(() =>
					{
						builder.AppendLine("if (assembly.EntryPoint != null)");
                        // WinForm的默认Main方法不带参数
                        builder.AppendLine("if (assembly.EntryPoint.GetParameters().Length == 0) assembly.EntryPoint.Invoke(null, null);");
                        builder.AppendLine("else assembly.EntryPoint.Invoke(null, new object[1] { Environment.GetCommandLineArgs().Skip(1).ToArray() });");
					});
					/*
					 * 增加使用验证
					 * 1. 通过网络将此程序的启动通知服务器
					 * 2. 若运行程序未被授权，服务器将通知程序自毁
					 * 3. 自毁程序自定义
					 */
				});
			});
			Shell[] shells = Assembly.GetExecutingAssembly().GetTypes(typeof(Shell)).
				Where(t => !t.IsAbstract).Select(t => (Shell)(t.GetConstructor(new Type[0]).Invoke(new object[0]))).ToArray();
			// shell on
			ushort d = 0;
			bytes = result;
			while (true)
			{
                // 若加载同名dll，在mono里将使用最开始加载的dll
				string temp = "Temp" + d + ".dll";
				// build Entry
				if (d == depth)
				{
					builder.AppendLine("public class Program");
					builder.AppendBlock(() =>
					{
						if (exe != null)
						{
							foreach (Attribute item in exe)
							{
								Type type = item.GetType();
								builder.AppendLine("[{0}]", type.FullName);
							}
							temp = Path.ChangeExtension(temp, "exe");
						}
						builder.AppendLine("public static void Main(string[] args)");
						builder.AppendBlock(() =>
						{
                            DestructSelf(overdueTime, builder);

							builder.AppendLine("byte[] bytes = {{{0}}};", string.Join(",", result.Select(r => r.ToString()).ToArray()));
							result = null;
							builder.AppendLine("Shell.Next(bytes);");
							// exe == null: 加密的dll没有入口
							// ext == .exe: 能运行和加载dll
							// 此时需要加载额外的exe来运行
							// exe != null: 已有入口
							// ext != .exe: 单纯dll，由外部自己动态添加，例如
							// Unity: 动态解析DLL，反射MonoBehaviour入口添加到Object上
							if (exe == null && Path.GetExtension(outputFile) == ".exe")
							{
								builder.AppendLine("var self = System.Reflection.Assembly.GetEntryAssembly().Location;");
								builder.AppendLine("var files = System.IO.Directory.GetFiles(Environment.CurrentDirectory, \"*.exe\", System.IO.SearchOption.TopDirectoryOnly);");
                                //builder.AppendLine("foreach (var file in files)");
                                builder.AppendLine("for (int i = 0, n = files.Length - 1; i <= n; i++)");
								builder.AppendBlock(() =>
								{
                                    builder.AppendLine("string file = files[i];");
									builder.AppendLine("if (file == self) continue;");
                                    builder.AppendLine("string arg = null;");
                                    // 同一文件夹下有多个exe文件时，可以通过参数指定要运行的exe文件
                                    builder.AppendLine("if (n > 2 && args.Length > 0)");
                                    builder.AppendBlock(() =>
                                    {
                                        builder.AppendLine("if (System.IO.Path.GetFileNameWithoutExtension(file) == args[0])");
                                        builder.AppendBlock(() =>
                                        {
                                            builder.AppendLine("string[] args2 = new string[args.Length - 1];");
                                            builder.AppendLine("System.Array.Copy(args, 1, args2, 0, args2.Length);");
                                            builder.AppendLine("args = args2;");
                                        });
                                        builder.AppendLine("else continue;");
                                    });
									builder.AppendLine("byte[] raw = System.IO.File.ReadAllBytes(file);");
									builder.AppendLine("byte[] symbol = null;");
									builder.AppendLine("string pdb = System.IO.Path.ChangeExtension(file, \"pdb\");");
									builder.AppendLine("if (System.IO.File.Exists(pdb))");
									builder.AppendBlock(() =>
									{
										builder.AppendLine("symbol = System.IO.File.ReadAllBytes(pdb);");
									});
									builder.AppendLine("var assembly = AppDomain.CurrentDomain.Load(raw, symbol);");
                                    builder.AppendLine("if (assembly.EntryPoint != null)");
                                    // WinForm的默认Main方法不带参数
                                    builder.AppendLine("if (assembly.EntryPoint.GetParameters().Length == 0) assembly.EntryPoint.Invoke(null, null);");
                                    builder.AppendLine("else assembly.EntryPoint.Invoke(null, new object[1] { args });");
									builder.AppendLine("return;");
								});
                                builder.AppendLine("Console.WriteLine(\"运行目录下有多个exe文件，但没有找到\" + args[0]);");
								temp = Path.ChangeExtension(temp, "exe");
							}
						});
					});
				}
                CompilerResults compileResult = UtilityCode.Compile(temp, dotnetCompilerVersion, d == depth && !string.IsNullOrEmpty(x86) ? x86 : "", "", builder.ToString());
                //CompilerResults compileResult = UtilityCode.Compile(temp, "", d == depth && !string.IsNullOrEmpty(x86) ? x86 : "", "", builder.ToString());
				if (UtilityCode.CheckCompileError(compileResult))
				{
					File.Delete(temp);
					return;
				}
				bytes = File.ReadAllBytes(temp);
				File.Delete(temp);
				result = result.Insert(0, bytes);

				if (d++ == depth)
					break;

				// Protect之后result长度可能发生变化
				int length = result.Length;
				Shell shell = shells[_RANDOM.Next(shells.Length)];
				shell.Protect(ref result);
				//File.WriteAllText(string.Format("Temp{0}.txt", d), Utility.Indent(builder.ToString()));

				builder = new StringBuilder();
				builder.AppendLine("using System;");
				builder.AppendLine("using System.Collections.Generic;");
				builder.AppendLine("using System.Reflection;");
				builder.AppendLine("using System.Linq;");
				builder.AppendLine();
				builder.AppendLine("public class Shell");
				builder.AppendBlock(() =>
				{
					builder.AppendLine("public static void Next(byte[] bytes)");
					builder.AppendBlock(() =>
					{
                        if (d % 50 == 0)
                        {
                            builder.AppendLine("GC.Collect();");
                            // unity默认的程序集GC没有此方法
                            //builder.AppendLine("GC.WaitForFullGCComplete(-1);");
                        }
                        shell.BuildLoadCode(builder);
                        builder.AppendLine("byte[] next = new byte[{0}];", bytes.Length);
                        builder.AppendLine("Array.Copy(bytes, 0, next, 0, {0});", bytes.Length);
                        builder.AppendLine("byte[] _continue = new byte[{0}];", length - bytes.Length);
                        builder.AppendLine("Array.Copy(bytes, {0}, _continue, 0, {1});", bytes.Length, length - bytes.Length);
                        builder.AppendLine("AppDomain.CurrentDomain.Load(next).GetType(\"Shell\").GetMethod(\"Next\").Invoke(null, new object[1] { _continue });");
					});
				});
			}
			if (exe != null)
				outputFile = Path.ChangeExtension(outputFile, "exe");
			File.WriteAllBytes(outputFile, result);
		}
        public static void BuildDepthInvoke(string dllAndType, string instance, byte depth, string outputCS, string preCondition)
		{
			Type type = GetDllType(dllAndType);

			StringBuilder builder = new StringBuilder();
            builder.AppendSharpIfCompile(preCondition, () =>
            {
                AppendDefaultNamespace(builder);
                builder.AppendLine();
                builder.AppendLine("namespace {0}", type.Namespace);
                builder.AppendBlock(() =>
                {
                    string name = "__" + type.Name;
                    builder.AppendLine("public static partial class {0}", name);
                    builder.AppendBlock(() =>
                    {
                        const string INSTANCE = "__instance";
                        if (string.IsNullOrEmpty(instance))
                            builder.AppendLine("public static {0} {1};", type.CodeName(true), INSTANCE);
                        else
                            builder.AppendLine("private static {0} {1} {{ get {{ return {2}; }} }}", type.CodeName(true), INSTANCE, instance);

                        BuildDepthInvoke(builder, type, new string[] { INSTANCE }, depth);
                    });
                });
            });

			SaveCode(outputCS, builder);
		}
		public static void BuildSameDirectory(string inputDir, string outputDir, bool deleteFiles)
		{
			inputDir = GetFullPath(inputDir);
			outputDir = GetFullPath(outputDir);

			DirectoryInfo target = new DirectoryInfo(outputDir);
			if (!target.Exists)
				target.Create();

			ForeachDirectory(inputDir,
				directory =>
				{
					string temp = outputDir + directory.FullName.Substring(inputDir.Length);
					target = new DirectoryInfo(temp);
					if (!target.Exists)
					{
						target.Create();
					}
					else
					{
						if (deleteFiles)
						{
							foreach (var file in target.GetFiles())
							{
								file.Delete();
							}
						}
					}
				});
		}
        public static void BuildMemberInvokeAvoidOptimize(string inputDirOrFile, string theNamespace, string outputCS)
        {
            string className = Path.GetFileNameWithoutExtension(outputCS);

            // 创建显示调用的代码防止被优化掉
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("public static class {0}", className);
            builder.AppendLine("{");
            builder.AppendLine("public static void AvoidOptimize()");
            builder.AppendLine("{");

            foreach (var file in GetFiles(inputDirOrFile, SearchOption.TopDirectoryOnly, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(Path.GetFullPath(file));
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass
                        || type.IsSpecialName
                        || type.IsAbstract
                        || type.IsGenericType || type.ContainsGenericParameters
                        || type.IsNotPublic
                        || !type.IsVisible)
                        continue;

                    if (string.IsNullOrEmpty(theNamespace) || type.Namespace.StartsWith(theNamespace))
                    {
                        var constructors = type.GetConstructors();
                        if (constructors.Length == 1 && constructors[0].GetParameters().Length == 0)
                        {
                            builder.AppendLine("new {0}();", type.FullName);
                        }
                    }
                }
            }

            builder.AppendLine("}");
            builder.AppendLine("}");

            File.WriteAllText(outputCS, builder.ToString());
            Console.WriteLine("生成防优化完毕");
        }
		public static void TexClearColor(string inputDirOrFile, byte clearR, byte clearG, byte clearB, byte clearA,
			byte coverR, byte coverG, byte coverB, byte coverA, string outputDir)
		{
			BuildDir(ref outputDir);

			COLOR clear = new COLOR(clearR, clearG, clearB, clearA);
			COLOR cover = new COLOR(coverR, coverG, coverB, coverA);

			string[] files = GetFiles(inputDirOrFile, IMAGE_FORMAT, SearchOption.AllDirectories);
			foreach (var file in files)
			{
				Bitmap texture = OpenBitmap(file);
				unsafe
				{
					int width = texture.Width;
					int height = texture.Height;

					var data = texture.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
					byte* scan = (byte*)data.Scan0.ToPointer();
					COLOR color;

					for (int i = 0; i < height; i++)
					{
						for (int j = 0; j < width; j++)
						{
							int index = i * data.Stride + j * 4;
							color.R = scan[index];
							color.G = scan[index + 1];
							color.B = scan[index + 2];
							color.A = scan[index + 3];

							byte alpha = color.A;

							if (clearA != 0)
								color = COLOR.ClearColor(color, clear);

                            if (coverA != 0)
								color = new COLOR(cover, (byte)(alpha * coverA / 255));

							scan[index] = color.R;
							scan[index + 1] = color.G;
							scan[index + 2] = color.B;
							scan[index + 3] = color.A;
						}
					}

					texture.UnlockBits(data);
					SavePng(texture, outputDir + _IO.RelativeDirectory(file, inputDirOrFile), file);
				}
				ClearMemory();
			}
		}
        public static void TexLightness(string inputDirOrFile, byte vStep, string outputDir)
        {
            if (vStep == 0)
                vStep = 1;

            BuildDir(ref outputDir);

            var files = GetFiles(inputDirOrFile, SearchOption.TopDirectoryOnly, IMAGE_FORMATS);
            foreach (var file in files)
            {
                var bitmap = OpenBitmap(file);

                LockBits(bitmap, (bytes) =>
                {
                    int len = bytes.Length;
                    for (int i = 0; i < len; i += 4)
                    {
                        byte r = bytes[i];
                        byte g = bytes[i + 1];
                        byte b = bytes[i + 2];

                        // HSV: H无所谓，S=0，V=明度=rgb中的最大值
                        byte value = r > g ? (r > b ? r : b) : (g > b ? g : b);
                        value = (byte)(value / vStep * vStep);
                        bytes[i] = value;
                        bytes[i + 1] = value;
                        bytes[i + 2] = value;
                    }
                });

                string filename = Path.GetFileName(file);
                bitmap.Save(outputDir + filename);
                Console.WriteLine("{0}处理完毕", filename);
            }
        }
        public static void TexMultiColor(string inputDirOrFile, string multiPng, string outputDir)
        {
            BuildDir(ref outputDir);

            var multiTex = (Bitmap)Bitmap.FromFile(multiPng);
            var multi = GetData(multiTex);
           
            float btf = COLOR.BYTE_TO_FLOAT;
            var files = GetFiles(inputDirOrFile, SearchOption.AllDirectories, IMAGE_FORMATS);
            foreach (var file in files)
            {
                var bitmap = OpenBitmap(file);

                // 图像像素
                LockBits(bitmap, (bytes) =>
                {
                    // 两张图片中心对齐
                    BitsAlignCenter(multi, multiTex.Width, multiTex.Height, bytes, bitmap.Width, bitmap.Height,
                    (i1, i2) =>
                    {
                        if (multi[i1 + 3] == 0)
                        {
                            bytes[i2 + 3] = 0;
                        }
                        else
                        {
                            bytes[i2] = (byte)(bytes[i2] * multi[i1] * btf);
                            bytes[i2 + 1] = (byte)(bytes[i2 + 1] * multi[i1 + 1] * btf);
                            bytes[i2 + 2] = (byte)(bytes[i2 + 2] * multi[i1 + 2] * btf);
                            bytes[i2 + 3] = (byte)(bytes[i2 + 3] * multi[i1 + 3] * btf);
                        }
                    });
                });

                string filename = Path.GetFileName(file);
                bitmap.Save(outputDir + filename);
                bitmap.Dispose();
                //SavePng(bitmap, outputDir, file, true);
                Console.WriteLine("{0}处理完毕", filename);
            }
        }
        public static void TexOverlying(string inputDirOrFile, string overlyingPng, bool overlyingTop, string outputDir)
        {
            BuildDir(ref outputDir);

            var overlyingTex = (Bitmap)Bitmap.FromFile(overlyingPng);
            var overlying = GetData(overlyingTex);

            var files = GetFiles(inputDirOrFile, SearchOption.AllDirectories, IMAGE_FORMATS);
            foreach (var file in files)
            {
                var bitmap = OpenBitmap(file);

                // 图像像素
                LockBits(bitmap, (bytes) =>
                {
                    if (overlyingTop)
                    {
                        BitsAlignCenter(overlying, overlyingTex.Width, overlyingTex.Height, bytes, bitmap.Width, bitmap.Height,
                        (i1, i2) =>
                        {
                            if (overlying[i1 + 3] != 0)
                            {
                                bytes[i2] = overlying[i1];
                                bytes[i2 + 1] = overlying[i1 + 1];
                                bytes[i2 + 2] = overlying[i1 + 2];
                                bytes[i2 + 3] = overlying[i1 + 3];
                            }
                        });
                    }
                    else
                    {
                        BitsAlignCenter(overlying, overlyingTex.Width, overlyingTex.Height, bytes, bitmap.Width, bitmap.Height,
                        (i1, i2) =>
                        {
                            if (overlying[i1 + 3] != 0 && bytes[i2 + 3] == 0)
                            {
                                bytes[i2] = overlying[i1];
                                bytes[i2 + 1] = overlying[i1 + 1];
                                bytes[i2 + 2] = overlying[i1 + 2];
                                bytes[i2 + 3] = overlying[i1 + 3];
                            }
                        });
                    }
                });

                string filename = Path.GetFileName(file);
                bitmap.Save(outputDir + filename);
                bitmap.Dispose();
                //SavePng(bitmap, outputDir, file, true);
                Console.WriteLine("{0}处理完毕", filename);
            }
        }
		public static void TexCut(string inputDirOrFile, bool normalize, ushort width, ushort height, string outputDir, bool isAll)
		{
			bool sizeAuto = width == 0 || height == 0;
			if (width > _MATH.MaxSize || height > _MATH.MaxSize)
			{
				Console.WriteLine("Texture size should not larger than {0}.", _MATH.MaxSize);
				return;
			}

			if (!sizeAuto && !normalize)
			{
				Console.WriteLine("Set texture size will require normalize is true.");
				return;
			}

			BuildDir(ref outputDir);

			bool allOrSplit = !normalize || !sizeAuto;
            string[] files = GetFiles(inputDirOrFile, IMAGE_FORMAT, isAll ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
			Console.WriteLine("Cut target mode: {0} count: {1}", allOrSplit ? "ALL" : "SPLIT", files.Length);

			Point size = new Point(width, height);

			if (!normalize)
			{
				string output;
				Bitmap[] textures = new Bitmap[1];
				foreach (var file in files)
				{
					textures[0] = OpenBitmap(file);
					Cut(textures, size);
					output = SavePng(textures[0], outputDir + _IO.RelativeDirectory(file, inputDirOrFile), file);
					Console.WriteLine("Save {0} completed.", output);
					ClearMemory();
				}
				return;
			}

			Dictionary<string, List<string>> group = files.Group(file => _IO.RelativeDirectory(file, inputDirOrFile));

			if (allOrSplit)
			{
				List<Bitmap> all = new List<Bitmap>();
				foreach (var item in group)
					all.AddRange(item.Value.Select(file => OpenBitmap(file)));

				Cut(all, size);

				string output;
				int index = 0;
				foreach (var item in group)
				{
					for (int i = 0; i < item.Value.Count; i++)
					{
						using (all[index])
						{
							output = SavePng(all[index], outputDir + item.Key, item.Value[i]);
							Console.WriteLine("Save {0} completed.", output);
						}
						all[index] = null;
						index++;
					}
				}
			}
			else
			{
				foreach (var item in group)
				{
					int count = item.Value.Count;
					Bitmap[] textures = new Bitmap[count];
					for (int i = 0; i < count; i++)
						textures[i] = OpenBitmap(item.Value[i]);

					Cut(textures, size);

					string output;
					for (int i = 0; i < count; i++)
					{
						using (textures[i])
						{
							output = SavePng(textures[i], outputDir + item.Key, item.Value[i]);
							Console.WriteLine("Save {0} completed.", output);
						}

						textures[i] = null;
					}
					textures = null;

					ClearMemory();
				}
			}
		}
        public static void TexPiece(string inputXLSX, string inputDir, string outputDir)
		{
            CSVWriter writer = new CSVWriter();
            if (!File.Exists(inputXLSX))
            {
                Piece test = new Piece();
                test.Directories = "Graphics\\TestDirectory";
                test.All = false;
                test.Metadata = "Graphics\\metadata";
                test.Output = "Graphics\\Test.png";
                writer.WriteObject(test);
                File.WriteAllText(Path.ChangeExtension(inputXLSX, "csv"), writer.Result, CSVWriter.CSVEncoding);
                Console.WriteLine("已生成CSV文档，请新建Excel文档按照CSV表头格式填写数据后再试");
                return;
            }

            BuildDir(ref inputDir, true);
            BuildDir(ref outputDir, true);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var table = LoadTableFromExcel(inputXLSX);
            writer.WriteTable(table);
            CSVReader reader = new CSVReader(writer.Result);
            var rows = reader.ReadObject<Piece[]>();
            var target = new PipelinePiece.Piece();
            //var root = Path.GetDirectoryName(Path.GetFullPath(inputXLSX));

            // 所有结果尺寸及尺寸组合
            Dictionary<int, List<Point>> all;
            #region

            all = new Dictionary<int, List<Point>>();
            int[] SIZE = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };
            int size = SIZE.Length;
            List<Point> sizes;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Point s = new Point(SIZE[i], SIZE[j]);
                    int c = SIZE[i] * SIZE[j];

                    if (all.TryGetValue(c, out sizes))
                    {
                        if (!sizes.Contains(s))
                            sizes.Add(s);
                    }
                    else
                    {
                        sizes = new List<Point>();
                        sizes.Add(s);
                        all.Add(c, sizes);
                    }
                }
            }

            #endregion

            Dictionary<string, Dictionary<string, PipelinePiece.Piece>> metadatas = new Dictionary<string, Dictionary<string, PipelinePiece.Piece>>();
            string metadata = null;
            foreach (var onePiece in rows)
            {
                //if (string.IsNullOrEmpty(onePiece.Directories))
                //{
                //    Console.WriteLine("Directories can't be null.");
                //    continue;
                //}

                bool isCombine = onePiece.Combine;
                if (!onePiece.Cut && !isCombine)
                {
                    Console.WriteLine("Must do 'Cut' or 'Combine'.");
                    continue;
                }

                if (string.IsNullOrEmpty(onePiece.Metadata))
                {
                    if (string.IsNullOrEmpty(metadata))
                    {
                        Console.WriteLine("Metadata must not be null.");
                        return;
                    }
                }
                else
                {
                    metadata = onePiece.Metadata;
                }

                Dictionary<string, PipelinePiece.Piece> pieces;
                if (!metadatas.TryGetValue(metadata, out pieces))
                {
                    pieces = new Dictionary<string, PipelinePiece.Piece>();
                    metadatas.Add(metadata, pieces);

                    //if (File.Exists(metadata))
                    //{
                    //    PipelinePiece.Piece[] temp = null;
                    //    reader = new CSVReader(File.ReadAllText(metadata));
                    //    reader.Read(out temp);
                    //    pieces.AddRange(temp);
                    //}
                }

                string[] directories = onePiece.Directories.Split(',');
                if (string.IsNullOrEmpty(onePiece.Output))
                {
                    onePiece.Output = directories[0];
                    var last = onePiece.Output[onePiece.Output.Length - 1];
                    if (last == '/' || last == '\\')
                        onePiece.Output = onePiece.Output.Substring(0, onePiece.Output.Length - 1);
                }

                // Map为已拥有的File
                if (pieces.ContainsKey(onePiece.Root + onePiece.Output))
                {
                    Console.WriteLine("目标图不能与原图路径重复");
                    return;
                }

                if (!string.IsNullOrEmpty(onePiece.Root))
                    BuildDir(ref onePiece.Root);
                
                var files = directories.SelectMany(d => GetFiles(inputDir + onePiece.Root + d, onePiece.All ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly, IMAGE_FORMATS)).ToArray();
                int count = files.Length;
                if (count == 0)
                {
                    Console.WriteLine("{0}没有任何可以打包的元素", onePiece.Directories);
                    continue;
                }
                Bitmap[] textures = new Bitmap[count];
                for (int i = 0; i < count; i++)
                    textures[i] = OpenBitmap(files[i]);

                string root = inputDir + onePiece.Root;
                CutArea[] cuts = null;
                if (onePiece.Cut)
                {
                    Directory.CreateDirectory("___TEMP");
                    cuts = new CutArea[count];
                    Bitmap[] temp = new Bitmap[1];
                    for (int i = 0; i < count; i++)
                    {
                        Bitmap texture = textures[i];

                        CutArea area = new CutArea();
                        area.Padding.Width = texture.Width;
                        area.Padding.Height = texture.Height;

                        // 裁切
                        temp[0] = texture;
                        int minX, minY, maxX, maxY;
                        Cut(temp, new Point(0, 0), out minX, out minY, out maxX, out maxY);

                        // 裁切后的图片
                        texture.Dispose();
                        texture = temp[0];
                        textures[i] = texture;

                        area.Texture = texture;
                        area.Padding.X = minX;
                        area.Padding.Y = minY;
                        area.Padding.Width -= maxX;
                        area.Padding.Height -= maxY;

                        cuts[i] = area;

                        if (!isCombine)
                        {
                            string fileName = Path.GetFileName(files[i]);

                            // :HINT
                            PipelinePiece.Piece piece = new PipelinePiece.Piece();
                            piece.File = ContentManager.FilePathUnify(onePiece.Root + _IO.RelativePath(files[i], root));
                            piece.Map = ContentManager.FilePathUnify(onePiece.Root + onePiece.Output + fileName);
                            piece.Source = new RECT(0, 0, texture.Width, texture.Height);
                            piece.Padding = area.Padding;
                            // 新增Piece
                            pieces.Add(piece.File, piece);

                            string saveFile = Path.GetFullPath(outputDir + piece.Map);
                            string saveDir = Path.GetDirectoryName(saveFile);
                            if (!Directory.Exists(saveDir))
                                Directory.CreateDirectory(saveDir);
                            texture.Save(saveFile);
                            texture.Dispose();
                            Console.WriteLine("裁切Piece: {0}", fileName);
                        }
                    }
                    Directory.Delete("___TEMP");

                    if (!isCombine)
                        continue;
                }

                // 矩形组的总面积
                int totalC = 0;
                for (int i = 0; i < count; i++)
                    totalC += textures[i].Width * textures[i].Height;

                // 满足面积的所有尺寸
                sizes = new List<Point>();
                foreach (var item in all)
                {
                    // 总面积足够放下所有
                    if (totalC <= item.Key)
                    {
                        Point max = new Point(
                            textures.Max(r => r.Width),
                            textures.Max(r => r.Height));
                        // 宽高足够放下单块最长和最宽
                        var temp = item.Value.Where(v => v.X >= max.X && v.Y >= max.Y).ToList();
                        if (temp.Count > 0)
                            sizes.AddRange(temp);
                    }
                }

                // 开始排布
                bool flag = false;
                Point[] normal = textures.Select(t => new Point(t.Width, t.Height)).ToArray();
                for (int i = 0; i < sizes.Count; i++)
                {
                    Rectangle[] result = Put(normal, sizes[i], true);
                    if (result == null)
                    {
                        result = Put(normal, sizes[i], false);
                        if (result != null)
                            result = result.Select(r => new Rectangle(r.Y, r.X, r.Height, r.Width)).ToArray();
                    }

                    if (result != null)
                    {
                        int width = sizes[i].X;
                        int height = sizes[i].Y;
                        // 不采用2的次方尺寸，裁切为最小尺寸
                        if (!onePiece.Power2)
                        {
                            int minX = width,
                                minY = height,
                                maxX = 0,
                                maxY = 0;
                            for (int j = 0; j < count; j++)
                            {
                                if (result[j].X < minX)
                                    minX = result[j].X;
                                if (result[j].Y < minY)
                                    minY = result[j].Y;
                                if (result[j].Right > maxX)
                                    maxX = result[j].Right;
                                if (result[j].Bottom > maxY)
                                    maxY = result[j].Bottom;
                            }

                            if (maxX - minX != width || maxY - minY != height)
                            {
                                width -= (width - maxX) + minX;
                                height -= (height - maxY) + minY;

                                if (minX != 0)
                                    for (int j = 0; j < count; j++)
                                        result[j].X -= minX;

                                if (minY != 0)
                                    for (int j = 0; j < count; j++)
                                        result[j].Y -= minY;
                            }
                        }

                        // draw tiled map
                        Bitmap map = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                        ImageDraw(map, graphics =>
                        {
                            for (int j = 0; j < count; j++)
                            {
                                using (Image source = textures[j])
                                {
                                    graphics.DrawImage(source, result[j]);
                                }
                            }
                        });

                        string suffix = Path.GetExtension(onePiece.Output);
                        if (string.IsNullOrEmpty(suffix))
                            onePiece.Output += "." + TEXTURE.SPECIAL_TEXTURE_TYPE;
                        root = inputDir + onePiece.Root;
                        for (int j = 0; j < count; j++)
                        {
                            // :HINT
                            PipelinePiece.Piece piece = new PipelinePiece.Piece();
                            //piece.File = _IO.RelativePath(files[j], inputDir);
                            piece.File = ContentManager.FilePathUnify(onePiece.Root + _IO.RelativePath(files[j], root));
                            piece.Map = ContentManager.FilePathUnify(onePiece.Root + onePiece.Output);
                            piece.Source = new RECT(
                                result[j].X,
                                result[j].Y,
                                result[j].Width,
                                result[j].Height);
                            if (onePiece.Cut)
                            {
                                piece.Padding = cuts[j].Padding;
                            }
                            // 新增Piece
                            pieces.Add(piece.File, piece);
                        }

                        string saveFile = outputDir + onePiece.Root + onePiece.Output;
                        string saveDir = Path.GetDirectoryName(saveFile);
                        if (!Directory.Exists(saveDir))
                            Directory.CreateDirectory(saveDir);
                        map.Save(saveFile);
                        Console.WriteLine("完成Piece: {0}", onePiece.Output);
                        flag = true;
                        break;
                    }
                } // end of combine

                if (!flag)
                {
                    Console.WriteLine("没有足够的尺寸组合Piece: {0}", onePiece.Output);
                }
            } // end of all pieces
            // 统一保存metadata
            foreach (var item in metadatas)
            {
                writer = new CSVWriter();
                writer.WriteObject(item.Value.Values.ToArray());
                string saveFile = Path.Combine(outputDir, item.Key);
                string saveDir = Path.GetDirectoryName(saveFile);
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);
                // 特殊的后缀.pcsv让PublishToUnity可以方便找到此文件，拷贝时过滤掉已经被打包的图片
                File.WriteAllText(saveFile + ".pcsv", writer.Result);
                Console.WriteLine("保存metadata:{0}完毕", item.Key);
            }
		}
		public static void TexPatchFromExcel(string inputXLSX, string outputDir)
		{
            CSVWriter writer = new CSVWriter();
			if (!File.Exists(inputXLSX))
			{
				Patch test = new Patch();
				test.Left = 1;
				test.Top = 1;
				test.Right = 2;
				test.Bottom = 2;
				test.Source = "Source/Test.png";
				test.ColorBody = "255,255,255,255";
				test.ColorBorder = "0,0,0,0";
				writer.WriteObject(test);
				File.WriteAllText(Path.ChangeExtension(inputXLSX, "csv"), writer.Result, CSVWriter.CSVEncoding);
                Console.WriteLine("已生成CSV文档，请新建Excel文档按照CSV表头格式填写数据后再试");
				return;
			}

            BuildDir(ref outputDir);

            var table = LoadTableFromExcel(inputXLSX);
            writer.WriteTable(table);
            CSVReader reader = new CSVReader(writer.Result);
			var patches = reader.ReadObject<Patch[]>();
			var target = new PipelinePatch.Patch();
            //var root = Path.GetDirectoryName(Path.GetFullPath(inputXLSX));
			var type = new PipelinePatch().FileType;
			foreach (var item in patches)
			{
				target.Anchor.X = item.Left;
				target.Anchor.Y = item.Top;
				target.Anchor.Width = item.Right - item.Left;
				target.Anchor.Height = item.Bottom - item.Top;
                if (item.Source.Contains('.'))
				    target.Source = item.Source;

				target.ColorBody = Patch.GetColor(item.ColorBody);
				target.ColorBorder = Patch.GetColor(item.ColorBorder);

                string result = Path.Combine(outputDir, item.Source);
                //if (!File.Exists(result))
                //{
                //    Console.WriteLine("没有目标图片{0}", result);
                //}

                var xml = new XmlWriter();
				xml.WriteObject(target);
				File.WriteAllText(Path.ChangeExtension(result, type), xml.Result);
                Console.WriteLine("生成九宫格{0}", item.Source);
			}
			Console.WriteLine("生成九宫格完毕");
		}
		public static void TexAnimationFromExcel(string inputXLSX, string inputDir, string outputDir)
		{
            CSVWriter writer = new CSVWriter();
			if (!File.Exists(inputXLSX))
			{
				Animation test = new Animation();
				test.Interval = 32;
				writer.WriteObject(test);
				File.WriteAllText(Path.ChangeExtension(inputXLSX, "csv"), writer.Result, CSVWriter.CSVEncoding);
				Console.WriteLine("已生成CSV文档，请新建Excel文档按照CSV表头格式填写数据后再试");
				return;
			}

            BuildDir(ref outputDir);

            var table = LoadTableFromExcel(inputXLSX);
            writer.WriteTable(table);
			CSVReader reader = new CSVReader(writer.Result);
			var animations = reader.ReadObject<Animation[]>();
			var type = new PipelineAnimation().FileType;
            foreach (var item in animations)
                if (string.IsNullOrEmpty(item.Output))
                    item.Output = item.Directory;
			foreach (var item in animations.Group(a => a.Output))
			{
				if (string.IsNullOrEmpty(item.Key))
				{
					Console.WriteLine("输出文件不能为空");
					continue;
				}
				try
				{
					item.Value.ToDictionary(a => a.Name);
				}
				catch (Exception)
				{
					Console.WriteLine("{0}拥有重复的序列动画名", item.Key);
					continue;
				}

				int count = item.Value.Count;
				var sequence = new Sequence[count];
				for (int i = 0; i < count; i++)
				{
					var anime = item.Value[i];
					var target = new Sequence();
					target.Name = anime.Name;
					target.Next = anime.Next;
					target.Loop = anime.Loop;
					// frames
                    string dir = Path.Combine(inputDir, anime.Directory);
					var frames = GetFiles(dir, SearchOption.TopDirectoryOnly, IMAGE_FORMATS).ToArray();
					target.Frames = new Frame[frames.Length];
					for (int j = 0; j < frames.Length; j++)
					{
						Frame frame = new Frame();
						frame.Texture = Path.Combine(anime.Directory, Path.GetFileName(frames[j]));
						frame.Interval = anime.Interval;
                        frame.PivotX = anime.PivotX;
                        frame.PivotY = anime.PivotY;
						target.Frames[j] = frame;
					}
					sequence[i] = target;
				}

                var xml = new XmlWriter();
				xml.WriteObject(sequence);
				File.WriteAllText(string.Format("{0}{1}.{2}", outputDir, item.Key, type), xml.Result);
                Console.WriteLine("生成动画{0}", item.Key);
			}
			Console.WriteLine("生成动画完毕");
		}
        public static void TexFontFromText(string inputText, string fontName, byte fontSize, byte fontStyle, string outputFileName)
        {
            Font font;
            if (string.IsNullOrEmpty(fontName) || fontSize <= 0)
            {
                FontDialog selector = new FontDialog();
                if (selector.ShowDialog() != DialogResult.OK)
                    return;
                font = selector.Font;
            }
            else
                font = new Font(fontName, fontSize, (FontStyle)fontStyle, GraphicsUnit.Pixel);

            char[] text = File.ReadAllText(inputText).Distinct().ToArray();
            Array.Sort(text);

            ByteWriter writer = new ByteWriter();
            writer.Write(font.Name);
            writer.Write(font.Size);
            float height = font.Height;
            writer.Write(height);
            int count = text.Length;
            writer.Write(count);

            using (Bitmap bitmap = new Bitmap(256, 256))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    Brush brush = Brushes.White;
                    StringFormat format = StringFormat.GenericTypographic;
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputFileName));
                    float x = 0;
                    float y = 0;
                    string c;
                    byte index = 0;
                    for (int i = 0; i < count; i++)
                    {
                        c = text[i].ToString();
                        //SizeF size = graphics.MeasureString(c, font, bitmap.Width, format);
                        //SizeF size = graphics.MeasureString(c, font);
                        //size.Width = _MATH.Ceiling(size.Width);
                        //size.Height = _MATH.Ceiling(size.Height);
                        float w;
                        if (FONT.IsHalfWidthChar(text[i]))
                            w = _MATH.Ceiling(font.Size * 0.5f);
                        else
                            w = font.Size;
                        float h = height;

                        if (x + w > bitmap.Width)
                        {
                            if (y + h > bitmap.Height)
                            {
                                // save
                                string file = string.Format("{0}\\{1}_{2}_{3}.png", outputDir, font.Name, font.Size, index);
                                bitmap.Save(file, ImageFormat.Png);

                                index++;
                                if (index == 0)
                                {
                                    Console.WriteLine("字数太多！缩小字体，减少字数，修改工具程序");
                                    return;
                                }
                                graphics.Clear(Color.Transparent);
                            }
                            else
                            {
                                x = 0;
                            }
                        }

                        RectangleF rect = new RectangleF(x, y, w, h);
                        graphics.DrawString(c, font, brush, rect, format);

                        writer.Write(text[i]);
                        writer.Write(index);
                        writer.Write((ushort)x);
                        writer.Write((ushort)y);
                        writer.Write((byte)w);
                        writer.Write((byte)h);

                        x += w;
                    }
                    bitmap.Save(string.Format("{0}\\{1}_{2}_{3}.png", outputDir, font.Name, font.Size, index), ImageFormat.Png);
                }
            }

            File.WriteAllBytes(string.Format("{0}.{1}", outputFileName, new PipelineFontStatic().FileType), writer.GetBuffer());
            Console.WriteLine("字体生成完毕");
        }
        public static void TexFontFromTexture(string inputDir, byte fontSize, string outputFileName)
        {
            string[] files = Directory.GetFiles(inputDir, IMAGE_FORMAT, SearchOption.TopDirectoryOnly);
            char[] text = files.Select(f => Path.GetFileNameWithoutExtension(f)[0]).ToArray();
            //Array.Sort(text);

            string fontName = Path.GetFileNameWithoutExtension(outputFileName);

            ByteWriter writer = new ByteWriter();
            writer.Write(fontName);
            writer.Write((float)fontSize);
            writer.Write((float)fontSize * 1.5f);
            int count = text.Length;
            writer.Write(count);

            using (Bitmap bitmap = new Bitmap(256, 256))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    Brush brush = Brushes.White;
                    StringFormat format = StringFormat.GenericTypographic;
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputFileName));
                    int x = 0;
                    int y = 0;
                    string c;
                    byte index = 0;
                    for (int i = 0; i < count; i++)
                    {
                        c = text[i].ToString();
                        //SizeF size = graphics.MeasureString(c, font, bitmap.Width, format);
                        //SizeF size = graphics.MeasureString(c, font);
                        //size.Width = _MATH.Ceiling(size.Width);
                        //size.Height = _MATH.Ceiling(size.Height);
                        var map = OpenBitmap(files[i]);
                        int w = map.Width;
                        int h = map.Height;

                        if (x + w > bitmap.Width)
                        {
                            if (y + h > bitmap.Height)
                            {
                                // save
                                string file = string.Format("{0}\\{1}_{2}_{3}.png", outputDir, fontName, fontSize, index);
                                bitmap.Save(file, ImageFormat.Png);

                                index++;
                                if (index == 0)
                                {
                                    Console.WriteLine("字数太多！缩小字体，减少字数，修改工具程序");
                                    return;
                                }
                                graphics.Clear(Color.Transparent);
                            }
                            else
                            {
                                x = 0;
                            }
                        }

                        Rectangle rect = new Rectangle(x, y, w, h);
                        graphics.DrawImage(map, rect, 0, 0, map.Width, map.Height, GraphicsUnit.Pixel);

                        writer.Write(text[i]);
                        writer.Write(index);
                        writer.Write((ushort)x);
                        writer.Write((ushort)y);
                        writer.Write((byte)w);
                        writer.Write((byte)h);

                        x += w;
                    }
                    bitmap.Save(string.Format("{0}\\{1}_{2}_{3}.png", outputDir, fontName, fontSize, index), ImageFormat.Png);
                }
            }

            File.WriteAllBytes(string.Format("{0}.{1}", outputFileName, new PipelineFontStatic().FileType), writer.GetBuffer());
            Console.WriteLine("字体生成完毕");
        }
        [Obsolete("To use TexPiece")]
		public static void TexTiledMap(string inputDirOrFile, ushort frameWidth, ushort frameHeight, byte mapRow, byte mapCol, string outputFile)
		{
			string[] files = GetFiles(inputDirOrFile, IMAGE_FORMAT);
			int count = files.Length;
			if (frameWidth == 0 || frameHeight == 0)
			{
				using (Bitmap sample = OpenBitmap(files[0]))
				{
					if (frameWidth == 0)
						frameWidth = (ushort)sample.Width;
					if (frameHeight == 0)
						frameHeight = (ushort)sample.Height;
				}
			}

			if (mapRow == 0 || mapCol == 0)
			{
				bool repeat = false;
				mapCol = (byte)(count < 8 ? count : 8);
			COL:
				mapRow = (byte)((count - 1) / mapCol + 1);

				if (repeat && mapRow * frameHeight > _MATH.MaxSize)
				{
					Console.WriteLine("图片尺寸已经超过了最大值" + _MATH.MaxSize);
					return;
				}
				else if (mapCol * frameWidth > _MATH.MaxSize)
				{
					mapCol = (byte)(_MATH.MaxSize / frameWidth);
					repeat = true;
					goto COL;
				}
			}

			PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
			using (Bitmap map = new Bitmap(frameWidth * mapCol, frameHeight * mapRow, pixelFormat))
			{
				ImageDraw(map, graphics =>
				{
					for (int i = 0; i < count; i++)
					{
						using (Image image = Image.FromFile(files[i]))
						{
							graphics.DrawImage(image,
								new Rectangle(
									(i % mapCol) * frameWidth,
									(i / mapCol) * frameHeight,
									frameWidth,
									frameHeight));
						}
					}
				});

				map.Save(outputFile, ImageFormat.Png);
			}
		}
		[Obsolete]
		public static void TexSplitTiledMap(string inputDirOrFile, ushort frameWidth, ushort frameHeight, string outputDir, string nameFormat)
		{
			if (!string.IsNullOrEmpty(nameFormat))
			{
				try
				{
					string format = string.Format(nameFormat, "name", 0);
					_LOG.Debug("NameFormat test: {0}", format);
				}
				catch (Exception)
				{
					Console.WriteLine("Name format: {0} is name, {1} is index. Index like {1:000} to limit the digit.");
					return;
				}
			}

			BuildDir(ref outputDir);

			string[] files = GetFiles(inputDirOrFile, IMAGE_FORMAT, SearchOption.AllDirectories);
			foreach (var file in files)
			{
				int width = frameWidth;
				int height = frameHeight;

				#region auto calc frame size
				if (files.Length > 1 || frameWidth == 0 || frameHeight == 0)
				{
					using (Bitmap sample = OpenBitmap(file))
					{
						byte[] buffer = GetData(sample);
						Rectangle rect = new Rectangle();
						int i;
						for (i = 0; i < sample.Height; i++)
						{
							for (int j = 0; j < sample.Width; j++)
							{
								int index = i * sample.Width * 4 + j * 4 + 3;
								if (buffer[index] != 0)
								{
									rect.Y = i;
									goto Left;
								}
							}
						}
					Left:
						for (i = 0; i < sample.Width; i++)
						{
							for (int j = 0; j < sample.Height; j++)
							{
								int index = j * sample.Width * 4 + i * 4 + 3;
								if (buffer[index] != 0)
								{
									rect.X = i;
									goto Bottom;
								}
							}
						}
					Bottom:
						bool has = true;
						for (i = rect.Y; i < sample.Height; i++)
						{
							bool temp = has;
							for (int j = 0; j < sample.Width; j++)
							{
								int index = i * sample.Width * 4 + j * 4 + 3;
								if (buffer[index] != 0)
								{
									temp = !has;
									break;
								}
							}
							if (has == temp == has)
							{
								has = !has;
								if (has)
								{
									rect.Width = i;
									break;
								}
							}
						}
						if (i == sample.Height)
						{
							rect.Width = sample.Height;
						}
						has = true;
						for (i = rect.X; i < sample.Width; i++)
						{
							bool temp = has;
							for (int j = 0; j < sample.Height; j++)
							{
								int index = j * sample.Width * 4 + i * 4 + 3;
								if (buffer[index] != 0)
								{
									temp = !has;
									break;
								}
							}
							if (has == temp == has)
							{
								has = !has;
								if (has)
								{
									rect.Height = i;
									break;
								}
							}
						}
						if (i == sample.Width)
						{
							rect.Height = i;
						}
						// try get bound
						for (i = rect.Right; i >= rect.Width - rect.X; i--)
							if (sample.Width % i == 0)
							{
								width = i;
								break;
							}
						for (i = rect.Bottom; i >= rect.Height - rect.Y; i--)
							if (sample.Height % i == 0)
							{
								height = i;
								break;
							}
					}

					if (width == 0 || height == 0)
						continue;
				}
				#endregion

				using (Image sourceMap = Image.FromFile(file))
				{
					string dir = outputDir + _IO.RelativeDirectory(file, inputDirOrFile);
					if (!Directory.Exists(dir))
						Directory.CreateDirectory(dir);

					string name = Path.GetFileNameWithoutExtension(file);
					int row = sourceMap.Height / height;
					int col = sourceMap.Width / width;
					Rectangle r = new Rectangle(0, 0, width, height);

					string format = nameFormat;
					if (string.IsNullOrEmpty(format))
					{
						int len = (row * col).ToString().Length;
						if (len > 1)
						{
							StringBuilder builder = new StringBuilder("{0}_{1:");
							for (int i = 0; i < len; i++)
								builder.Append("0");
							builder.Append("}");
							format = builder.ToString();
						}
						else
						{
							format = "{0}_{1}";
						}
					}

					for (int i = 0; i < row; i++)
					{
						for (int j = 0; j < col; j++)
						{
							using (Bitmap frame = new Bitmap(width, height))
							{
								ImageDraw(frame, g =>
								{
									g.DrawImage(
										sourceMap,
										r,
										new Rectangle(
											j * width,
											i * height,
											width,
											height),
										GraphicsUnit.Pixel);
								});

								int order = i * col + j;
								string output = string.Format("{0}{1}.png", dir, string.Format(format, name, order));
								frame.Save(output, ImageFormat.Png);
								Console.WriteLine("Save {0} completed.", output);
							}
						}
					}
				}

				ClearMemory();
			}
		}
        public static void TexSplit(string inputDirOrFile, string outputDir)
        {
            int STEP = 2;
            int STEP2 = STEP * 2 + 1;

            BuildDir(ref outputDir);

            bool isFile = inputDirOrFile.Contains('.');
            string[] files = GetFiles(inputDirOrFile, IMAGE_FORMAT, SearchOption.AllDirectories);
            inputDirOrFile = _IO.DirectoryWithEnding(Path.GetFullPath(Path.GetDirectoryName(inputDirOrFile)));
            Stopwatch watch = Stopwatch.StartNew();
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                //List<Image> splits = new List<Image>();
                using (Bitmap sourceMap = (Bitmap)Bitmap.FromFile(file))
                {
                    string dir;
                    if (isFile)
                        // 单个文件直接生成在[目标目录]内
                        dir = outputDir;
                    else
                        // 多个文件生成在[目标目录/文件名/]内
                        dir = string.Format("{0}{1}/{2}/", outputDir, _IO.RelativeDirectory(file, inputDirOrFile), name);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    int width = sourceMap.Width;
                    int height = sourceMap.Height;
                    int stride = width * 4;
                    int count = 0;

                    byte[] buffer = GetData(sourceMap);
                    Func<int, int, bool> HasAlpha = (x, y) =>
                    {
                        return buffer[(y * stride) + (x << 2) + 3] > 0;
                    };

                    Pixel[] pixeles = new Pixel[width * height];
                    for (int i = 0; i < pixeles.Length; i++)
                    {
                        pixeles[i] = new Pixel()
                        {
                            Alpha = buffer[(i << 2) + 3],
                            X = (ushort)(i % width),
                            Y = (ushort)(i / width),
                        };
                    }
                    GraphMap4444<Pixel> map = new GraphMap4444<Pixel>();
                    map.Build(pixeles, width);

                    while (true)
                    {
                        int x = 0, y;
                        for (y = 0; y < height; y+=3)
                        {
                            for (x = 0; x < width; x+=3)
                                // 刷全图，从刷到的像素开始扫描对象
                                if (HasAlpha(x, y))
                                    break;
                            if (x < width)
                                break;
                        }
                        if (y >= height && x >= width)
                            // 已经全部拆分完成，没有残留像素在图上
                            break;

                        SplitArea split = new SplitArea();
                        while (true)
                        {
                            // 找到边缘位置
                            while (y > 0 && HasAlpha(x, y - 1))
                                y--;
                            // 设定初始位置
                            split.SetPos(x, y);

                            Pixel.GPS(map[x, y], -1,
                                p =>
                                {
                                    // 本身有像素
                                    if (p.Alpha > 0)
                                    {
                                        // 周围都需要没有像素(边缘像素)
                                        for (int sx = p.X - STEP, a = 0; a < STEP2; a++, sx++)
                                        {
                                            if (sx < 0 || sx >= width)
                                                return true;
                                            for (int sy = p.Y - 2, b = 0; b < STEP2; b++, sy++)
                                            {
                                                if (sy < 0 || sy >= height)
                                                    return true;
                                                if (map[sx, sy].Alpha == 0)
                                                    return true;
                                            }
                                        }
                                    }
                                    return false;
                                },
                                p =>
                                {
                                    split.SetPos(p.Node.X, p.Node.Y);
                                    return false;
                                });

                            int minX, minY, maxX, maxY;
                            split.Resolve(out minX, out minY, out maxX, out maxY);

                            // 扫描矩形范围内未被扫描到的部分
                            // 如果有其它颜色，则那个颜色使用同样的扫描方式得出矩形范围
                            // 两个矩形范围取并集得出新的矩形范围
                            bool flag = false;
                            for (y = minY; y <= maxY; y++)
                            {
                                ScanLine line = split.Lines[y];
                                for (x = minX; x < line.Min; x++)
                                {
                                    if (HasAlpha(x, y))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag) break;
                                for (x = line.Max + 1; x <= maxX; x++)
                                {
                                    if (HasAlpha(x, y))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag) break;
                            }
                            if (!flag)
                            {
                                // 过小的分离像素不进行拆分
                                if (split.Area.Width * split.Area.Height >= 20)
                                {
                                    using (Bitmap frame = new Bitmap(split.Area.Width, split.Area.Height))
                                    {
                                        ImageDraw(frame, g =>
                                        {
                                            g.DrawImage(
                                                sourceMap,
                                                new Rectangle(0, 0, frame.Width, frame.Height),
                                                split.Area,
                                                GraphicsUnit.Pixel);
                                        });

                                        string output = string.Format("{0}{1:000}.png", dir, count++);
                                        frame.Save(output, ImageFormat.Png);
                                        //Console.WriteLine("Save {0} completed.", output);
                                    }
                                }

                                // 删除掉像素，让下次刷全图找像素时不会找到重复的图案
                                for (int i = minY; i <= maxY; i++)
                                    for (int j = minX; j <= maxX; j++)
                                        buffer[(i * stride) + (j << 2) + 3] = 0;
                                break;
                            }
                        }// end of split of one piece
                    }// end of split of a hole map
                }// end of using map
                Console.WriteLine("拆分图片[{0}]完成", file);

                ClearMemory();
            }
            watch.Stop();
            Console.WriteLine("拆分图片{0}张，总耗时:{1}ms", files.Length, watch.ElapsedMilliseconds);
        }
		public static void TexThumbnail(string inputDirOrFile, ushort frameWidth, ushort frameHeight, string outputDir)
		{
			if (frameWidth > _MATH.MaxSize || frameHeight > _MATH.MaxSize)
			{
				Console.WriteLine("Texture size should not larger than {0}.", _MATH.MaxSize);
				return;
			}

			BuildDir(ref outputDir);

			var textures = GetFiles(inputDirOrFile, SearchOption.AllDirectories, IMAGE_FORMATS);

            if (frameWidth == 0 || frameHeight == 0)
            {
                var first = textures.FirstOrDefault();
                if (first != null)
                {
                    using (var bitmap = OpenBitmap(first))
                    {
                        if (frameWidth == 0)
                        {
                            // 宽自动
                            frameWidth = (ushort)_MATH.Ceiling(bitmap.Width * frameHeight * 1.0f / bitmap.Height);
                        }
                        else
                        {
                            // 高自动
                            frameHeight = (ushort)_MATH.Ceiling(bitmap.Height * frameWidth * 1.0f / bitmap.Width);
                        }
                    }
                }
            }

			VECTOR2 offset;
			float scale;
			using (Bitmap graphics = new Bitmap(frameWidth, frameHeight))
			{
				using (Graphics g = Graphics.FromImage(graphics))
				{
					VECTOR2 gSize = new VECTOR2(frameWidth, frameHeight);
					foreach (var image in textures)
					{
						g.Clear(Color.Transparent);

						using (var bitmap = OpenBitmap(image))
						{
							__GRAPHICS.ViewAdapt(new VECTOR2(bitmap.Width, bitmap.Height), gSize, out scale, out offset);
							g.DrawImage(bitmap,
								new Rectangle((int)offset.X,
									(int)offset.Y,
									(int)(bitmap.Width * scale),
									(int)(bitmap.Height * scale)));
						}

						SavePng(graphics, outputDir, image, false);
					}
				}
			}
		}
        //public static void Pack(string inputDir, string suffix, string outputFile)
        //{

        //}
        public static void PublishToUnity(string xnaDir, string unityAssetsDir)
        {
            BuildDir(ref xnaDir);
            BuildDir(ref unityAssetsDir);

            // 将dll转为.bytes 文件放入StreamingAssets文件夹，并生成列表供Unity加载
            string dir = _IO.PathCombine(Path.GetFullPath(unityAssetsDir), "StreamingAssets\\");
            StringBuilder builder = new StringBuilder();

            FileInfo runtime = new FileInfo(Path.Combine(dir, "UnityRuntime.bytes"));
            // IOS平台由于不能热更新，将不会采用动态加载DLL
            if (runtime.Exists)
            {
                builder.AppendLine("UnityRuntime.bytes\t{0}\t{1}", runtime.LastWriteTime.Ticks, runtime.Length);
            }

            HashSet<string> filter = new HashSet<string>();

            foreach (var dll in GetFiles(xnaDir, "*.dll"))
            {
                string name = Path.GetFileNameWithoutExtension(dll);
                if (name.EndsWith("fmodex"))
                    continue;
                FileInfo info = new FileInfo(dll);
                builder.AppendLine("{0}.bytes\t{1}\t{2}", name, info.LastWriteTime.Ticks, info.Length);
                File.Copy(dll, dir + name + ".bytes", true);
            }
            Console.WriteLine("复制dll列表完成");
            string fileList = dir + "__filelist.txt";
            string fileListVersion = dir + "__version.txt";

            // 复制资源文件到StreamingAssets文件夹
            xnaDir = _IO.PathCombine(Path.GetFullPath(xnaDir), "Content\\");
            DirectoryInfo target = new DirectoryInfo(dir);
            if (!target.Exists)
                target.Create();
            // 筛选去除TexPiece,Pack命令已打包的图片源文件
            foreach (var meta in Directory.GetFiles(xnaDir, "*.pcsv", SearchOption.AllDirectories))
            {
                PipelinePiece.Piece[] pieces = new CSVReader(File.ReadAllText(meta)).ReadObject<PipelinePiece.Piece[]>();
                foreach (var piece in pieces)
                    filter.Add(piece.File);
            }
            Action<DirectoryInfo> copy = null;
            copy = (directory) =>
                {
                    string targetDirName = directory.FullName.Substring(xnaDir.Length);
                    string temp = dir + targetDirName;
                    Console.WriteLine(targetDirName);
                    target = new DirectoryInfo(temp);
                    if (target.Exists && !string.IsNullOrEmpty(targetDirName))
                        target.Delete(true);
                    bool createFlag = true;
                    foreach (var file in directory.GetFiles())
                    {
                        string fileName = ContentManager.FilePathUnify(file.FullName.Substring(xnaDir.Length));
                        if (filter.Contains(fileName))
                            continue;
                        if (createFlag)
                        {
                            // 没有文件的文件夹不创建
                            target.Create();
                            createFlag = false;
                        }
                        // 一些批处理之类的文件
                        if (file.Name.StartsWith("#"))
                            continue;
                        // 文件名\t最后修改时间\t文件大小(字节)
                        builder.AppendLine("{0}\t{1}\t{2}", fileName, file.LastWriteTime.Ticks, file.Length);
                        file.CopyTo(Path.Combine(temp, file.Name), true);
                    }
                    foreach (var dirc in directory.GetDirectories())
                        if (!dirc.Name.StartsWith("#"))
                            //ForeachDirectory(dirc, copy);
                            copy(dirc);
                };
            copy(new DirectoryInfo(xnaDir));
            //ForeachDirectory(xnaDir, copy);
            File.WriteAllText(fileList, builder.ToString(), Encoding.UTF8);
            File.WriteAllBytes(fileListVersion, BitConverter.GetBytes(new FileInfo(fileList).LastWriteTime.Ticks));
            Console.WriteLine("复制资源完成");
        }
        public static void PublishToWebGL(string xnaCodeDir, string xnaContentDir, string outputFile, bool min, byte encrypt)
        {
            throw new NotImplementedException();
        }
        public static void CreateFileToEmptyDirectory(string inputDir, string defaultFile)
        {
            BuildDir(ref inputDir);
            string fileName = null;
            if (!string.IsNullOrEmpty(defaultFile) && File.Exists(defaultFile))
                fileName = Path.GetFileName(defaultFile);
            ForeachDirectory(inputDir, dir =>
            {
                if (dir.GetFiles().Length == 0)
                {
                    if (fileName == null)
                        File.WriteAllText(Path.Combine(dir.FullName, ".empty"), "");
                    else
                        File.Copy(defaultFile, Path.Combine(dir.FullName, fileName));
                }
            });
        }


        interface ITaskSegment
        {
            /// <summary>任务片段ID，用于区分相同任务中的不同片段</summary>
            int SegmentID { get; }
            /// <summary>任务片段唯一ID，小于等于0则忽略，大于0时可用于统计固定任务片段的耗时</summary>
            int SegmentTimeID { get; }
        }
        struct TaskSegment : ITaskSegment
        {
            private int segmentID;
            private int segmentTimeID;
            public TaskSegment(int segmentID, int segmentTimeID)
            {
                this.segmentID = segmentID;
                this.segmentTimeID = segmentTimeID;
            }
            public int SegmentID
            {
                get { return segmentID; }
            }
            public int SegmentTimeID
            {
                get { return segmentTimeID; }
            }
        }
        class TaskSegmentArray : ITaskSegment
        {
            private int segmentID;
            private int segmentTimeID;
            public int Start;
            public int Index;
            /// <summary>不包含</summary>
            public int End;

            private TaskSegmentArray() { }

            public int SegmentID
            {
                get { return segmentID; }
            }
            public int SegmentTimeID
            {
                get { return segmentTimeID; }
            }
            public float Progress { get { return (Index - Start) * 1.0f / (End - Start); } }

            public static void BuildTaskSegmentArray(ITaskSegment[] segments, int segmentCount, int arrayLength, int segmentTimeID)
            {
                if (segmentCount > segments.Length)
                    throw new IndexOutOfRangeException();
                // 计算一个任务的数量
                // 10,101 前面是10，最后是11
                // 10, 109 前面是11，最后是10
                int pageCount = arrayLength / segmentCount;
                int over = arrayLength % segmentCount;
                if (over > pageCount * 0.5)
                {
                    pageCount++;
                }
                for (int i = 0; i < segmentCount; i++)
                {
                    TaskSegmentArray segment = new TaskSegmentArray();
                    if (segmentTimeID > 0)
                    {
                        segment.segmentTimeID = segmentTimeID + i;
                    }
                    segment.segmentID = i;
                    segment.Start = i * pageCount;
                    segment.Index = segment.Start;
                    segment.End = segment.Start + pageCount;
                    // 最后一个有可能超过
                    if (segment.End > arrayLength)
                        segment.End = arrayLength;
                    segments[i] = segment;
                }
            }
            public static void BuildTaskSegmentArray(ITaskSegment[] segments, int arrayLength, int segmentTimeID)
            {
                BuildTaskSegmentArray(segments, segments.Length, arrayLength, segmentTimeID);
            }
            public static void BuildTaskSegmentArray(ITaskSegment[] segments, int arrayLength)
            {
                BuildTaskSegmentArray(segments, arrayLength, -1);
            }
        }
        abstract class ParallelTask
        {
            public float GetTaskProgress()
            {
                return _MATH.Clamp(InternalGetTaskProgress(), 0, 1);
            }
            protected abstract float InternalGetTaskProgress();
            protected internal abstract int Split(ITaskSegment[] segments);
            protected internal abstract void Process(ITaskSegment task);
        }
        class ParallelPointCloudCompare : ParallelTask
        {
            const int TASK_TIME_ID = 1000;

            public ModelPointCloud[] Models;
            private ITaskSegment[] segments;
            private int scount;
            public ModelPointCloud Target;

            protected override float InternalGetTaskProgress()
            {
                if (segments == null)
                    return 0;

                int count = 0;
                for (int i = 0; i < scount; i++)
                {
                    var s = ((TaskSegmentArray)segments[i]);
                    count += s.Index - s.Start;
                }
                return count * 1.0f / Models.Length;
            }
            protected internal override int Split(ITaskSegment[] segments)
            {
                this.segments = segments;
                this.scount = segments.Length;
                if (scount > 2)
                {
                    scount = 2;
                }
                TaskSegmentArray.BuildTaskSegmentArray(segments, scount, Models.Length, TASK_TIME_ID);
                return scount;
            }
            protected internal override void Process(ITaskSegment task)
            {
                TaskSegmentArray s = (TaskSegmentArray)task;
                float rotate;
                float similar;
                for (; s.Index < s.End; s.Index++)
                {
                    //if (已经结束)
                    //    break;
                    similar = Models[s.Index].CalcModel(Target, -180, out rotate);
                    // 比较结果
                    throw new NotImplementedException();
                }
            }
        }
        class ParallelTextAnalysis : ParallelTask
        {
            protected override float InternalGetTaskProgress()
            {
                throw new NotImplementedException();
            }
            protected internal override int Split(ITaskSegment[] segments)
            {
                throw new NotImplementedException();
            }
            protected internal override void Process(ITaskSegment task)
            {
                throw new NotImplementedException();
            }
        }
        
        static class _PARALLEL
        {
            abstract class AsyncThreadBase
            {
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                Thread thread;
                bool working;
                public bool Working { get { return working; } }
                void Process()
                {
                    while (true)
                    {
                        // wait work
                        handle.WaitOne();
                        // do work
                        InternalWork();
                        working = false;
                    }
                }
                protected abstract void InternalWork();
                protected internal void Work()
                {
                    if (working)
                    {
                        _LOG.Warning("已经在工作 Track:{0}", Environment.StackTrace);
                        return;
                    }
                    if (thread == null)
                    {
                        thread = new Thread(Process);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    // let thread to do wrok
                    handle.Set();
                    working = true;
                }
            }
            class AsyncParallel : AsyncThreadBase
            {
                Stopwatch watch = new Stopwatch();
                ParallelTask task;
                ITaskSegment segment;
                public void Work(ParallelTask task, ITaskSegment segment)
                {
                    this.task = task;
                    this.segment = segment;
                    Work();
                }
                protected override void InternalWork()
                {
                    int sid = segment.SegmentTimeID;
                    if (sid > 0)
                    {
                        watch.Start();
                    }

                    task.Process(segment);

                    if (sid > 0)
                    {
                        watch.Stop();
                        // 对唯一任务片段进行计时统计
                    }
                }
            }
            /// <summary>处理线程空出来时的任务分配</summary>
            class AsyncParallelAllot : AsyncThreadBase
            {
                protected override void InternalWork()
                {
                    while (tasks.Count > 0)
                    {
                        if (InternalDo(tasks.Peek(), true))
                        {
                            tasks.Dequeue();
                        }
                        else
                        {
                            // 没有足够的空闲线程
                            break;
                        }
                    }
                }
            }

            // 首次工作时开启任务分配线程
            static AsyncParallelAllot allot = new AsyncParallelAllot();
            static AsyncParallel[] threads;
            static Queue<ParallelTask> tasks = new Queue<ParallelTask>();

            static _PARALLEL()
            {
                // todo:应根据硬件设备相关参数开启线程数
                int count = 8;
                threads = new AsyncParallel[count];
                for (int i = 0; i < count; i++)
                    threads[i] = new AsyncParallel();
            }

            public static bool HasIdleThread
            {
                get
                {
                    for (int i = 0; i < threads.Length; i++)
                    {
                        if (!threads[i].Working)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            public static int IdleThreadCount
            {
                get
                {
                    int count = 0;
                    for (int i = 0; i < threads.Length; i++)
                    {
                        if (!threads[i].Working)
                        {
                            count++;
                        }
                    }
                    return count;
                }
            }

            /* 在单机器，单进程中，可分配的线程数应该是固定的
             * 1. 不同的并行任务（可以考虑使用多进程）应该使用相同的线程池，需要考虑好分配
             * 2. 相同的任务采用并行完成，仍然需要保持这个任务的完整性。例如并行查找，查找到目标后应该结束整个并行任务
             */
            private static bool InternalDo(ParallelTask task, bool inQueue)
            {
                int tcount = IdleThreadCount;
                if (tcount == 0)
                {
                    if (inQueue)
                    {
                        //_LOG.Warning("队列任务分配失败");
                        return false;
                    }
                    else
                    {
                        tasks.Enqueue(task);
                    }
                }
                else
                {
                    if (inQueue)
                    {
                        ITaskSegment[] segments = new ITaskSegment[tcount];
                        int scount = task.Split(segments);
                        if (scount == 0)
                        {
                            throw new InvalidOperationException("错误的拆分任务 类型=" + task.GetType().FullName);
                        }
                        if (scount > tcount)
                        {
                            throw new IndexOutOfRangeException();
                        }
                        int idle = 0;
                        for (int i = 0; i < scount; i++)
                        {
                            //if (segments[i] == null)
                            //{
                            //    // 至少有一个任务
                            //    if (i == 0)
                            //    {
                            //        throw new InvalidOperationException("错误的拆分任务 类型=" + task.GetType().FullName);
                            //    }
                            //    break;
                            //}
                            // 让空闲线程工作
                            while (true)
                            {
                                if (!threads[idle].Working)
                                {
                                    threads[idle].Work(task, segments[i]);
                                    idle++;
                                    break;
                                }
                                idle++;
                            }
                        }
                    }
                    else
                    {
                        // 统一到分配线程上执行
                        tasks.Enqueue(task);
                        allot.Work();
                    }
                }
                return true;
            }
            public static void Do(ParallelTask task)
            {
                InternalDo(task, false);
            }
        }


        private static void TestText()
        {
            #region 测试点云相似

            //VECTOR2[] points = new VECTOR2[]
            //{
            //    new VECTOR2(1, -1),
            //    new VECTOR2(0.5f, -0.5f),
            //    new VECTOR2(-1, 1),
            //    new VECTOR2(-1, -1),
            //    new VECTOR2(1, 1),
            //    new VECTOR2(-0.5f, 0.5f),
            //    new VECTOR2(-0.6f, -0.5f),
            //    new VECTOR2(0.5f, 0.5f),
            //};
            //ModelPointCloud cloud = new ModelPointCloud();
            //cloud.SetPoints(points);
            //float rotate;
            //MATRIX2x3 rotateMatrix = MATRIX2x3.CreateRotation(_MATH.PI_OVER_4, 0, 0);
            //VECTOR2.Transform(points, ref rotateMatrix, points);
            //float similarity = cloud.CalcModel(points, -180, out rotate);

            // 读入所有的字模
            ModelPointCloud[] models;
            {
                Stopwatch time = Stopwatch.StartNew();

                string[] files = Directory.GetFiles("CharacterModel", "*.m", SearchOption.TopDirectoryOnly);
                models = new ModelPointCloud[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    byte[] content = File.ReadAllBytes(files[i]);
                    models[i].Load(content);
                }

                time.Stop();
                Console.WriteLine("读入{0}个字模文件，耗时{1}ms", models.Length, (int)time.ElapsedMilliseconds);
            }

            // 绘制一个文字图片进行比较
            {
                Font font = new Font("黑体", 72, FontStyle.Regular, GraphicsUnit.Pixel);
                int width = _MATH.Ceiling(font.Size * 1.5f);
                int height = _MATH.Ceiling(font.GetHeight() * 1.5f);
                byte[] buffer = new byte[width * height * 4];
                bool[] visited = new bool[width * height];
                VECTOR2[] points = new VECTOR2[width * height];
                int vcount = 0;
                VECTOR2[] result = new VECTOR2[width * height];
                int col4 = width * 4;
                Action<int, int> linked = null;
                linked = (row, col) =>
                {
                    int indexb = row * col4 + col * 4;
                    int indexv = row * width + col;
                    if (buffer[indexb + 3] != 0 && !visited[indexv])
                    {
                        byte alpha = buffer[indexb + 3];
                        visited[indexv] = true;
                        // 判断自己是否是边界点
                        if (row == 0 || row == height - 1 || col == 0 || col == width - 1
                            || buffer[indexb - 1] == 0// 左
                            || buffer[indexb + 7] == 0// 右
                            || buffer[indexb + 3 - col4] == 0// 上
                            || buffer[indexb + 3 + col4] == 0// 下
                            || buffer[indexb - 1 - col4] == 0// 左上
                            || buffer[indexb - 1 + col4] == 0// 左下
                            || buffer[indexb + 7 - col4] == 0// 右上
                            || buffer[indexb + 7 + col4] == 0// 右上
                            )
                        {
                            // 记录边界点
                            points[vcount].X = col;
                            points[vcount].Y = row;
                            vcount++;

                            // 往各个方向递归检测连续的像素
                            if (col > 0) linked(row, col - 1);// 左
                            if (col < width - 1) linked(row, col + 1);// 右
                            if (row > 0) linked(row - 1, col);// 上
                            if (row < height - 1) linked(row + 1, col);// 下
                            if (col > 0)
                            {
                                if (row > 0) linked(row - 1, col - 1);// 左上
                                if (row < height - 1) linked(row + 1, col - 1);// 左下
                            }
                            if (col < width - 1)
                            {
                                if (row > 0) linked(row - 1, col + 1);// 右上
                                if (row < height - 1) linked(row + 1, col + 1);// 右下
                            }
                        }
                    }
                };

                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        Brush brush = Brushes.White;
                        StringFormat format = StringFormat.GenericTypographic;

                        var matrix = graphics.Transform;
                        matrix.RotateAt(180, new PointF(width >> 1, height >> 1));
                        graphics.Transform = matrix;

                        int _width = _MATH.Ceiling(font.Size);
                        int _height = _MATH.Ceiling(font.GetHeight());
                        Rectangle rect = new Rectangle(0, 0, _width, _height);

                        graphics.Clear(System.Drawing.Color.Transparent);
                        char c = '自';
                        graphics.DrawString(c.ToString(), font, brush, rect, format);

                        bitmap.Save("test.png");

                        var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                        Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                        #region 从文字图形生成点云模型
                        for (int row = 0; row < height; row++)
                        {
                            for (int col = 0; col < width; col++)
                            {
                                linked(row, col);
                            }
                        }
                        #endregion
                        bitmap.UnlockBits(data);

                        #region 通过轮廓点的连接关系，拆分出渲染用的顶点

                        int vcount2 = 0;
                        int startIndex = 0;
                        int lineStartIndex = 0;
                        var p1 = points[0];
                        VECTOR2 p2;
                        byte currentDirection = 8;
                        byte direction;
                        int turnedIndex = -1;
                        Action<VECTOR2> newPoint = (p) =>
                        {
                            result[vcount2].X = p.X;
                            result[vcount2].Y = p.Y;
                            vcount2++;
                        };

                        for (int i = 1, e = vcount - 1; i <= e; i++)
                        {
                            p2 = points[i];
                            if (currentDirection == 8)
                            {
                                currentDirection = GetDirection(p1, p2);
                                //p1.Direction = currentDirection;
                                // 一条线段的起点
                                newPoint(p1);
                                startIndex = i - 1;
                            }
                            else
                            {
                                if (_MATH.Abs(p2.X - p1.X) > 1 || _MATH.Abs(p2.Y - p1.Y) > 1)
                                {
                                    // 不连续的笔画，例如“二”的上面一横和下面一横是分开的
                                    turnedIndex = -1;
                                    currentDirection = 8;
                                    // 前一条线段的终点
                                    // 可能是连接起点的闭合图形，此时，不添加最后一个点
                                    if (_MATH.Abs(p1.X - points[lineStartIndex].X) > 1 || _MATH.Abs(p1.Y - points[lineStartIndex].Y) > 1)
                                    {
                                        //p1.Direction = currentDirection;
                                        newPoint(p1);
                                    }
                                    lineStartIndex = i + 1;
                                }
                                else
                                {
                                    direction = GetDirection(p1, p2);
                                    //p1.Direction = direction;
                                    if (turnedIndex == -1)
                                    {
                                        // 转弯
                                        if (direction != currentDirection)
                                        {
                                            // 首次转弯，可能弯度不大，阈值内都不新建点
                                            turnedIndex = i - 1;
                                        }
                                    }
                                    else
                                    {
                                        // 取样点数内，n%的取样点都进行了转弯则视为转弯，否则视为没转弯
                                        if (i - turnedIndex >= 3)
                                        {
                                            // 连续转弯多次则需要新建点
                                            // 直线的起始点与转弯点的角度 和 转弯点和当前点的角度 差值不大时认为是同一条直线
                                            float a1 = _MATH.ToDegree((float)Math.Atan2(points[startIndex].Y - points[turnedIndex].Y, points[startIndex].X - points[turnedIndex].X));
                                            float a2 = _MATH.ToDegree((float)Math.Atan2(points[turnedIndex].Y - p2.Y, points[turnedIndex].X - p2.X));
                                            if (AngleDifference(a1, a2) > 30)
                                            {
                                                // 转弯角度过大，视为转弯
                                                p1 = points[turnedIndex];
                                                //points2[turnedIndex].Direction = currentDirection;
                                                //newPoint(points2[turnedIndex]);
                                                // 回到转弯点以新直线重现做一遍处理
                                                i = turnedIndex;
                                                turnedIndex = -1;
                                                currentDirection = 8;
                                                continue;
                                            }
                                            else
                                            {
                                                // 视为没有转弯
                                                turnedIndex = -1;
                                            }
                                        }// end of turnedIndex != -1
                                    }// end of turnedIndex != -1
                                }// end of 连续连接的点
                            }// end of 线段非起点
                            p1 = p2;
                            // 生成最后一个点
                            if (i == e)
                            {
                                if (_MATH.Abs(p1.X - points[lineStartIndex].X) > 1 || _MATH.Abs(p1.Y - points[lineStartIndex].Y) > 1)
                                {
                                    //p1.Direction = currentDirection;
                                    newPoint(p1);
                                }
                            }
                        }

                        #endregion


                        TimeSpan maxTime = new TimeSpan();
                        char maxCharacter = default(char);
                        TimeSpan minTime = new TimeSpan();
                        char minCharacter = default(char);
                        Stopwatch totalTime;

                        ModelPointCloud model = new ModelPointCloud();
                        model.SetPoints(result, vcount2);
                        // 与字模进行比较
                        int mcount = models.Length;
                        float rotation;

                        totalTime = Stopwatch.StartNew();
                        for (int i = 0; i < mcount; i++)
                        {
                            var aTime = Stopwatch.StartNew();
                            if (models[i].Charater == c)
                            {
                            }
                            float value = models[i].CalcModel(model, -180, out rotation);
                            if (value > 0.8f)
                            {
                                ModelPointCloud m = models[i];
                                char cc = m.Charater;
                            }
                            aTime.Stop();
                            var elapsed = aTime.Elapsed;
                            if (elapsed > maxTime)
                            {
                                maxCharacter = c;
                                maxTime = elapsed;
                            }
                            else if (elapsed < minTime)
                            {
                                minCharacter = c;
                                minTime = elapsed;
                            }
                        }
                        totalTime.Stop();
                    }
                }
            }

            #endregion

            return;

            #region 学习文字模型
            {
                Font font = new Font("黑体", 72, FontStyle.Regular, GraphicsUnit.Pixel);
                int width = _MATH.Ceiling(font.Size);
                int height = _MATH.Ceiling(font.GetHeight());
                byte[] buffer = new byte[width * height * 4];
                bool[] visited = new bool[width * height];
                VECTOR2[] points = new VECTOR2[width * height];
                int vcount = 0;
                VECTOR2[] result = new VECTOR2[width * height];
                int col4 = width * 4;
                Action<int, int> linked = null;
                linked = (row, col) =>
                {
                    int indexb = row * col4 + col * 4;
                    int indexv = row * width + col;
                    if (buffer[indexb + 3] != 0 && !visited[indexv])
                    {
                        byte alpha = buffer[indexb + 3];
                        visited[indexv] = true;
                        // 判断自己是否是边界点
                        if (row == 0 || row == height - 1 || col == 0 || col == width - 1
                            || buffer[indexb - 1] == 0// 左
                            || buffer[indexb + 7] == 0// 右
                            || buffer[indexb + 3 - col4] == 0// 上
                            || buffer[indexb + 3 + col4] == 0// 下
                            || buffer[indexb - 1 - col4] == 0// 左上
                            || buffer[indexb - 1 + col4] == 0// 左下
                            || buffer[indexb + 7 - col4] == 0// 右上
                            || buffer[indexb + 7 + col4] == 0// 右上
                            )
                        {
                            // 记录边界点
                            points[vcount].X = col;
                            points[vcount].Y = row;
                            vcount++;

                            // 往各个方向递归检测连续的像素
                            if (col > 0) linked(row, col - 1);// 左
                            if (col < width - 1) linked(row, col + 1);// 右
                            if (row > 0) linked(row - 1, col);// 上
                            if (row < height - 1) linked(row + 1, col);// 下
                            if (col > 0)
                            {
                                if (row > 0) linked(row - 1, col - 1);// 左上
                                if (row < height - 1) linked(row + 1, col - 1);// 左下
                            }
                            if (col < width - 1)
                            {
                                if (row > 0) linked(row - 1, col + 1);// 右上
                                if (row < height - 1) linked(row + 1, col + 1);// 右下
                            }
                        }
                    }
                };

                TimeSpan maxTime = new TimeSpan();
                char maxCharacter = default(char);
                TimeSpan minTime = new TimeSpan();
                char minCharacter = default(char);
                Stopwatch totalTime;
                int ccount = 0;

                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        Brush brush = Brushes.White;
                        StringFormat format = StringFormat.GenericTypographic;
                        Rectangle rect = new Rectangle(0, 0, width, height);

                        totalTime = Stopwatch.StartNew();

                        for (int z = 0x4E00; z <= 0x9FBF; z++)
                        //for (int z = 97; z < 123; z++)
                        {
                            Stopwatch aTime = Stopwatch.StartNew();

                            graphics.Clear(System.Drawing.Color.Transparent);
                            char c = (char)z;
                            graphics.DrawString(c.ToString(), font, brush, rect, format);

                            var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                            #region 从文字图形生成点云模型
                            for (int row = 0; row < height; row++)
                            {
                                for (int col = 0; col < width; col++)
                                {
                                    linked(row, col);
                                }
                            }
                            #endregion
                            bitmap.UnlockBits(data);

                            // 空白文字
                            if (vcount == 0)
                                continue;

                            #region 通过轮廓点的连接关系，拆分出渲染用的顶点

                            int vcount2 = 0;
                            int startIndex = 0;
                            int lineStartIndex = 0;
                            var p1 = points[0];
                            VECTOR2 p2;
                            byte currentDirection = 8;
                            byte direction;
                            int turnedIndex = -1;
                            Action<VECTOR2> newPoint = (p) =>
                            {
                                result[vcount2].X = p.X;
                                result[vcount2].Y = p.Y;
                                vcount2++;
                            };

                            for (int i = 1, e = vcount - 1; i <= e; i++)
                            {
                                p2 = points[i];
                                if (currentDirection == 8)
                                {
                                    currentDirection = GetDirection(p1, p2);
                                    //p1.Direction = currentDirection;
                                    // 一条线段的起点
                                    newPoint(p1);
                                    startIndex = i - 1;
                                }
                                else
                                {
                                    if (_MATH.Abs(p2.X - p1.X) > 1 || _MATH.Abs(p2.Y - p1.Y) > 1)
                                    {
                                        // 不连续的笔画，例如“二”的上面一横和下面一横是分开的
                                        turnedIndex = -1;
                                        currentDirection = 8;
                                        // 前一条线段的终点
                                        // 可能是连接起点的闭合图形，此时，不添加最后一个点
                                        if (_MATH.Abs(p1.X - points[lineStartIndex].X) > 1 || _MATH.Abs(p1.Y - points[lineStartIndex].Y) > 1)
                                        {
                                            //p1.Direction = currentDirection;
                                            newPoint(p1);
                                        }
                                        lineStartIndex = i + 1;
                                    }
                                    else
                                    {
                                        direction = GetDirection(p1, p2);
                                        //p1.Direction = direction;
                                        if (turnedIndex == -1)
                                        {
                                            // 转弯
                                            if (direction != currentDirection)
                                            {
                                                // 首次转弯，可能弯度不大，阈值内都不新建点
                                                turnedIndex = i - 1;
                                            }
                                        }
                                        else
                                        {
                                            // 取样点数内，n%的取样点都进行了转弯则视为转弯，否则视为没转弯
                                            if (i - turnedIndex >= 3)
                                            {
                                                // 连续转弯多次则需要新建点
                                                // 直线的起始点与转弯点的角度 和 转弯点和当前点的角度 差值不大时认为是同一条直线
                                                float a1 = _MATH.ToDegree((float)Math.Atan2(points[startIndex].Y - points[turnedIndex].Y, points[startIndex].X - points[turnedIndex].X));
                                                float a2 = _MATH.ToDegree((float)Math.Atan2(points[turnedIndex].Y - p2.Y, points[turnedIndex].X - p2.X));
                                                if (AngleDifference(a1, a2) > 30)
                                                {
                                                    // 转弯角度过大，视为转弯
                                                    p1 = points[turnedIndex];
                                                    //points2[turnedIndex].Direction = currentDirection;
                                                    //newPoint(points2[turnedIndex]);
                                                    // 回到转弯点以新直线重现做一遍处理
                                                    i = turnedIndex;
                                                    turnedIndex = -1;
                                                    currentDirection = 8;
                                                    continue;
                                                }
                                                else
                                                {
                                                    // 视为没有转弯
                                                    turnedIndex = -1;
                                                }
                                            }// end of turnedIndex != -1
                                        }// end of turnedIndex != -1
                                    }// end of 连续连接的点
                                }// end of 线段非起点
                                p1 = p2;
                                // 生成最后一个点
                                if (i == e)
                                {
                                    if (_MATH.Abs(p1.X - points[lineStartIndex].X) > 1 || _MATH.Abs(p1.Y - points[lineStartIndex].Y) > 1)
                                    {
                                        //p1.Direction = currentDirection;
                                        newPoint(p1);
                                    }
                                }
                            }

                            #endregion

                            #region 生成保存字模

                            ModelPointCloud model = new ModelPointCloud();
                            model.Charater = c;
                            model.Angle = 0;
                            model.SetPoints(result, vcount2);

                            byte[] content = model.Save();
                            File.WriteAllBytes("CharacterModel/" + c.ToString() + ".m", content);

                            #endregion

                            aTime.Stop();
                            var elapsed = aTime.Elapsed;
                            if (elapsed > maxTime)
                            {
                                maxCharacter = c;
                                maxTime = elapsed;
                            }
                            else if (elapsed < minTime)
                            {
                                minCharacter = c;
                                minTime = elapsed;
                            }
                            ccount++;

                            Console.WriteLine("{0}字模学习完成", c);

                            Array.Clear(visited, 0, visited.Length);
                            vcount = 0;
                            vcount2 = 0;
                        }

                        totalTime.Stop();
                    }
                }

                Console.WriteLine("学习文字数量:{0} 总用时:{1}ms 平均用时:{2}ms 最高用时:{3} {4}ms 最低用时:{5} {6}ms",
                    ccount, (int)totalTime.Elapsed.TotalMilliseconds, ((int)totalTime.Elapsed.TotalMilliseconds) / ccount,
                    maxCharacter, (int)maxTime.TotalMilliseconds, minCharacter, (int)minTime.TotalMilliseconds);
            }
            #endregion
        }
        private static byte GetDirection(VECTOR2 start, VECTOR2 next)
        {
            if (start.X == next.X)
            {
                return (byte)(next.Y > start.Y ? 2 : 6);
            }
            else if (start.X < next.X)
            {
                if (next.Y == start.Y) return 0;
                else if (next.Y > start.Y) return 1;
                else return 7;
            }
            else
            {
                if (next.Y == start.Y) return 4;
                else if (next.Y > start.Y) return 3;
                else return 5;
            }
        }
        private static int DirectionDifference(byte d1, byte d2)
        {
            int diff = _MATH.Abs(d1 - d2);
            if (diff > 4)
                return 8 - diff;
            return diff;
        }
        private static float AngleDifference(float a1, float a2)
        {
            float diff = a2 - a1;
            if (diff > 180)
            {
                return 360 + a1 - a2;
            }
            else if (diff < -180)
            {
                return 360 + a2 - a1;
            }
            else
            {
                return diff < 0 ? -diff : diff;
            }
        }
        private static float Angle1To2(float a1, float a2)
        {
            float diff = a2 - a1;
            if (diff > 180)
            {
                return 360 + a1 - a2;
            }
            else if (diff < -180)
            {
                return a1 - 360 + a2;
            }
            else
            {
                return diff > 0 ? -diff : diff;
            }
        }
        struct ModelPointCloud
        {
            private static ModelPointCloud compareModel;
            public char Charater;
            internal float Angle;
            internal ModelPoint[] Points;
            internal int Length;
            internal int GetPointCount()
            {

                if (Points == null) return -1;
                if (Length < 0) return Points.Length;
                return Length;
            }
            internal ModelPointCloud(char c, float angle)
            {
                this.Charater = c;
                this.Angle = angle;
                this.Points = null;
                this.Length = -1;
            }
            internal ModelPointCloud(char c, float angle, ModelPoint[] points, int length)
            {
                this.Charater = c;
                this.Angle = angle;
                this.Points = points;
                this.Length = length;
            }
            public void SetPoints(VECTOR2[] points, int len)
            {
                Points = new ModelPoint[len];
                #region 计算基本参数

                float l = points[0].X, t = points[0].Y, r = points[0].X, b = points[0].Y;
                // 将所有点归入以0,0为中心的坐标系中
                for (int i = 1; i < len; i++)
                {
                    VECTOR2 v = points[i];
                    if (v.Y < t)
                        t = v.Y;
                    else if (v.Y > b)
                        b = v.Y;
                    if (v.X < l)
                        l = v.X;
                    else if (v.X > r)
                        r = v.X;
                }
                VECTOR2 center = new VECTOR2((l + r) * 0.5f, (t + b) * 0.5f);
                for (int i = 0; i < len; i++)
                {
                    VECTOR2 point = points[i] - center;
                    ModelPoint mpoint = new ModelPoint();
                    mpoint.Distance = (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
                    mpoint.Angle = _MATH.ToDegree((float)Math.Atan2(point.Y, point.X));
                    //mpoint.Position = point;
                    mpoint.X = (sbyte)point.X;
                    mpoint.Y = (sbyte)point.Y;
                    Points[i] = mpoint;
                }

                #endregion
                
                // 按照角度顺时针排序
                Array.Sort(Points, 0, len, ModelPointComparer.Comparer);

                #region 连接最外层的点形成多边形轮廓，轮廓要求点数尽量少并且连接后的多边形要包含其它所有点
                if (len == 3)
                {
                    Points[0].IsOuter = true;
                    Points[1].IsOuter = true;
                    Points[2].IsOuter = true;
                }
                else
                {
                    // 最高且最左的点作为第一个边界点
                    int first = 0;
                    l = Points[0].X;
                    t = Points[1].Y;
                    for (int i = 1; i < len; i++)
                    {
                        if (Points[i].Y < t)
                        {
                            t = Points[i].Y;
                            first = i;
                        }
                        else if (Points[i].Y == t && Points[i].X < l)
                        {
                            l = Points[i].X;
                            first = i;
                        }
                    }
                    Points[first].IsOuter = true;
                    int start = first;
                    float prev = -180;
                    while (true)
                    {
                        // 顺时针查找与前一个点夹角最小的点
                        int index = start;
                        float max = 360;
                        int i = start + 1;
                        while (true)
                        {
                            if (i == len) i = 0;
                            if (i == start) break;
                            float dangle = (float)Math.Atan2(Points[i].Y - Points[start].Y, Points[i].X - Points[start].X) * _MATH.R2D;
                            float rotate = _MATH.RangeAngle(dangle - prev);
                            if (rotate <= max)
                            {
                                max = rotate;
                                index = i;
                            }
                            if (Points[i].IsOuter)
                                break;
                            i++;
                        }
                        Points[index].IsOuter = true;
                        prev += max;
                        if (index == first) break;
                        start = index;
                    }
                }
                #endregion

                this.Length = len;
            }
            public void SetPoints(VECTOR2[] points)
            {
                SetPoints(points, points.Length);
                this.Length = -1;
            }
            public float CalcModel(VECTOR2[] points, float defaultAngle, out float rotateAngle)
            {
                ModelPointCloud cloud = new ModelPointCloud();
                cloud.SetPoints(points);
                return CalcModel(cloud, defaultAngle, out rotateAngle);
            }
            public float CalcModel(ModelPointCloud cloud, float defaultAngle, out float rotateAngle)
            {
                var points1 = Points;
                var points2 = cloud.Points;

                int len1 = GetPointCount();
                int len2 = cloud.GetPointCount();

                // 学习的字模固定第一个外边界点作为比较点
                int first1 = -1;
                for (int i = 0; i < len1; i++)
                {
                    if (points1[i].IsOuter)
                    {
                        first1 = i;
                        break;
                    }
                }
                // 目标文字将所有边界点和字模的首个边界点对齐进行比较
                int first2 = -1;
                for (int i = 0; i < len2; i++)
                {
                    if (points2[i].IsOuter && (first2 == -1 || points2[i].Angle >= defaultAngle))
                    {
                        first2 = i;
                        if (points2[i].Angle >= defaultAngle)
                            break;
                    }
                }
                int start2 = first2;
                float maxScore = 0;
                rotateAngle = 0;
                while (true)
                {
                    int index1 = first1;
                    var p1 = points1[index1];
                    int index2 = start2;
                    var p2 = points2[index2];
                    // 对齐两个字模的首个边界点导致p2旋转的度数，有方向
                    float rotate = Angle1To2(p2.Angle, p1.Angle);
                    // 相似度比较的分数，满分为1，完全不匹配为0
                    float score = 1f;
                    for (int i = 1; i < len2; i++)
                    {
                        // 开始角度比较
                        float diff = AngleDifference(p1.Angle, 
                            // 所有p2的点都需要旋转对齐角度再与源字模比较
                            rotate + p2.Angle);
                        float __score = AngleDifferenceScore(diff);
                        score *= __score;
                        // 不匹配
                        if (score <= 0)
                            break;
                        // 两个字模的点应该做到大致对应
                        // todo: 对于手写体文字生成的点的数量是不确定的
                        index1++;
                        if (index1 == len1) index1 = 0;
                        p1 = points1[index1];
                        index2++;
                        if (index2 == len2) index2 = 0;
                        p2 = points2[index2];
                    }
                    // 记录最高相似度
                    if (score > 0 && score > maxScore)
                    {
                        maxScore = score;
                        rotateAngle = -rotate;
                        if (maxScore == 1)
                            break;
                    }
                    // 旋转到下一个边界点再次进行比较
                    bool flag = false;
                    for (int i = start2 + 1; i < len2; i++)
                    {
                        if (points2[i].IsOuter)
                        {
                            flag = true;
                            start2 = i;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        if (start2 >= first2) start2 = 0;
                        for (int i = start2; i < first2; i++)
                        {
                            if (points2[i].IsOuter)
                            {
                                flag = true;
                                start2 = i;
                                break;
                            }
                        }
                        // 所有边界点都比较完毕
                        if (!flag)
                            break;
                    }
                }
                return maxScore;
            }
            const float ADS = 1.0f / 8100f;
            private static float AngleDifferenceScore(float angle)
            {
                // 角度差越小，分数越高，否则分数越低
                // 分数直接乘算到总分上，所以此分数应呈抛物线状
                // 1 - ax^2
                return 1 - ADS * angle * angle;
            }
            public override string ToString()
            {
                return string.Format("C:{0} A:{1} L:{2}", Charater, Angle, GetPointCount());
            }

            public byte[] Save()
            {
                ByteWriter writer = new ByteWriter(10 + Points.Length * ModelPoint.SIZE);
                writer.Write(Charater);
                writer.Write(Angle);
                if (Points == null) writer.Write(-1);
                else
                {
                    int len = Points.Length;
                    if (Length >= 0)
                        len = Length;
                    writer.Write(len);
                    for (int i = 0; i < len; i++)
                        Points[i].Save(writer);
                }
                return writer.Buffer;
            }
            public void Load(byte[] data)
            {
                ByteReader reader = new ByteReader(data);
                reader.Read(out Charater);
                reader.Read(out Angle);
                reader.Read(out Length);
                if (Length == -1)
                {
                    Points = null;
                }
                else
                {
                    Points = new ModelPoint[Length];
                    for (int i = 0; i < Length; i++)
                    {
                        Points[i] = new ModelPoint();
                        Points[i].Load(reader);
                    }
                }
            }
            public static ModelPointCloud LoadModel(byte[] data)
            {
                ModelPointCloud model = new ModelPointCloud();
                model.Load(data);
                return model;
            }
        }
        class ModelPointComparer : IComparer<ModelPoint>
        {
            public static readonly ModelPointComparer Comparer = new ModelPointComparer();
            public int Compare(ModelPoint x, ModelPoint y)
            {
                int a = _MATH.Sign(x.Angle - y.Angle);
                if (a == 0)
                    return _MATH.Sign(y.Distance - x.Distance);
                return a;
            }
        }
        class ModelPoint
        {
            public const int SIZE = 12;
            public sbyte X;
            public sbyte Y;
            public byte Direction;
            public float Angle;
            public float Distance;
            public bool IsOuter;
            public void Save(ByteWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Direction);
                writer.Write(Angle);
                writer.Write(Distance);
                writer.Write(IsOuter);
            }
            public void Load(ByteReader reader)
            {
                reader.Read(out X);
                reader.Read(out Y);
                reader.Read(out Direction);
                reader.Read(out Angle);
                reader.Read(out Distance);
                reader.Read(out IsOuter);
            }
            public override string ToString()
            {
                return string.Format("{0},{1} T:{2} A:{3} D:{4}{6}", X, Y, Direction, Angle.ToString("0.00"), Distance,
                    IsOuter ? " Outter" : null);
            }
        }
	}
}
