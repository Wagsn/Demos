using System;
using System.Collections.Generic;

namespace ApiServer.AspNetCore.Mvc
{
    /// <summary>
    /// 自定义`AssemblyPart`，手动返回空引用路径数组
    /// </summary>
    public class MyAssemblyPart : Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart, Microsoft.AspNetCore.Mvc.ApplicationParts.ICompilationReferencesProvider
    {
        public MyAssemblyPart(System.Reflection.Assembly assembly) : base(assembly) { }

        public IEnumerable<string> GetReferencePaths() => Array.Empty<string>();
    }
}
