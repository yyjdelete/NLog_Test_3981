using System;
using System.IO;
using System.Reflection;

namespace Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            var aaa = Path.GetFullPath(Path.Combine(typeof(Program).Assembly.Location, @"../../../../../NLogTest/bin/Debug/netcoreapp3.1/NLogTest.dll"));
            var loader = new PluginLoadContext(aaa);
            var ass = loader.LoadFromAssemblyPath(aaa);
            var pg2 = ass.GetType("NLogTest.Program");
            pg2.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { Array.Empty<string>() });
        }
        private class PluginLoadContext : System.Runtime.Loader.AssemblyLoadContext
        {
            private readonly System.Runtime.Loader.AssemblyDependencyResolver _resolver;

            public PluginLoadContext(string pluginPath)
            {
                _resolver = new System.Runtime.Loader.AssemblyDependencyResolver(pluginPath);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
                if (assemblyPath != null)
                {
                    return LoadFromAssemblyPath(assemblyPath);
                }

                return null;
            }

            protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
            {
                string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
                if (libraryPath != null)
                {
                    return LoadUnmanagedDllFromPath(libraryPath);
                }

                return IntPtr.Zero;
            }
        }
    }
}
