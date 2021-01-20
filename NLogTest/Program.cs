using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace NLogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DumpNLogAssembly("Pre(all):", AppDomain.CurrentDomain.GetAssemblies());
            DumpNLogAssembly("Pre(default ctx):", AssemblyLoadContext.Default.Assemblies);
            DumpNLogAssembly("Pre(current ctx):", AssemblyLoadContext.GetLoadContext(typeof(Program).Assembly).Assemblies);
            //NLog.Common.InternalLogger.LogToConsole = true;
            //NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Debug;
            //var lf = new NLog.LogFactory();
            //lf.Setup().SetupExtensions(
            NLog.LogManager.LogFactory.Setup().SetupExtensions(ext => {
                //ext.RegisterAssemblyLoader(asmName => {
                //    var assemblyName = new AssemblyName() { Name = asmName };
                //    var assemblyContext = AssemblyLoadContext.GetLoadContext(typeof(NLog.LogFactory).Assembly);
                //    return assemblyContext.LoadFromAssemblyName(assemblyName);
                //});
                //ext.AutoLoadExtensions();
                //ext.AutoLoadAssemblies(false);
            });
            NLog.LogManager.GetCurrentClassLogger().Info("Start NLog");
            DumpNLogAssembly("Post(all):", AppDomain.CurrentDomain.GetAssemblies());
            DumpNLogAssembly("Post(default ctx):", AssemblyLoadContext.Default.Assemblies);
            DumpNLogAssembly("Post(current ctx):", AssemblyLoadContext.GetLoadContext(typeof(Program).Assembly).Assemblies);
        }

        private static void DumpNLogAssembly(string title, IEnumerable<Assembly> assemblies)
        {
            assemblies = assemblies.Where(a => a.GetName().Name.StartsWith("NLog"));
            Console.WriteLine();
            Console.WriteLine(title + assemblies.Count());
            foreach (var cur in assemblies)
            {
                Console.WriteLine(cur.FullName);
            }
        }
    }
}
