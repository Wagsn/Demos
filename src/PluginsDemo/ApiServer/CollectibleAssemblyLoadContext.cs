using System.Reflection;
using System.Runtime.Loader;

namespace ApiServer
{
    /// <summary>
    /// 可回收的程序集加载上下文
    /// 注：每个插件都使用一个单独的CollectibleAssemblyLoadContext来加载，所有插件的CollectibleAssemblyLoadContext都放在一个PluginsLoadContext对象中。
    /// </summary>
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        // 可回收的
        public CollectibleAssemblyLoadContext() : base(isCollectible: true) { }
        protected override Assembly Load(AssemblyName name)
        {
            return null;
        }
    }
}
