﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace AssemblyExecuteAndUnloadDemo
{
    class TestAssemblyLoadContext : AssemblyLoadContext
    {
        public TestAssemblyLoadContext() : base(true)
        {
        }
        protected override Assembly Load(AssemblyName name)
        {
            return null;
        }
    }

    class TestInfo
    {
        public TestInfo(MethodInfo mi)
        {
            entryPoint = mi;
        }
        MethodInfo entryPoint;
    }

    class Program
    {
        static TestInfo entryPoint;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int ExecuteAndUnload(string assemblyPath, out WeakReference testAlcWeakRef, out MethodInfo testEntryPoint)
        {
            var alc = new TestAssemblyLoadContext();
            testAlcWeakRef = new WeakReference(alc);

            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);
            if (a == null)
            {
                testEntryPoint = null;
                Console.WriteLine("Loading the test assembly failed");
                return -1;
            }

            var args = new object[1] { new string[] { "Hello" } };

            // Issue preventing unloading #1 - we keep MethodInfo of a method for an assembly loaded into the TestAssemblyLoadContext in a static variable
            entryPoint = new TestInfo(a.EntryPoint);
            testEntryPoint = a.EntryPoint;

            int result = (int)a.EntryPoint.Invoke(entryPoint, args);
            alc.Unload();

            return result;
        }
        static void Main(string[] args)
        {
            WeakReference testAlcWeakRef;
            // Issue preventing unloading #2 - we keep MethodInfo of a method for an assembly loaded into the TestAssemblyLoadContext in a local variable
            MethodInfo testEntryPoint;
            int result = ExecuteAndUnload(@"F:\Workspace\DotNet\Demos\src\AssemblyExecuteAndUnloadDemo\TestAssemblyLoadContextDemo\bin\Debug\netcoreapp3.0\TestAssemblyLoadContextDemo.dll", out testAlcWeakRef, out testEntryPoint);

            // 这里不管怎么GC还是一直被占用着 TODO 只能采用文件流的方式加载`var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open)`还需要自定义`MyAssemblyPart`
            for (int i = 0; testAlcWeakRef.IsAlive && (i < 10); i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            System.Diagnostics.Debugger.Break();

            // Test completed, result=1, entryPoint: Int32 Main(System.String[]) unload success: False
            Console.WriteLine($"Test completed, result={result}, entryPoint: {testEntryPoint} unload success: {!testAlcWeakRef.IsAlive}");
        }
    }
}
