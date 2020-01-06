using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer
{
    /// <summary>
    /// 总插件加载上下文，用来统一管理每一个单独的插件加载上下文
    /// </summary>
    public static class PluginsLoadContext
    {
        private static Dictionary<string, CollectibleAssemblyLoadContext> _pluginContexts { get; } = new Dictionary<string, CollectibleAssemblyLoadContext>();

        public static bool Any(string pluginName)
        {
            return _pluginContexts.ContainsKey(pluginName);
        }

        public static void RemovePluginContext(string pluginName)
        {
            if (_pluginContexts.ContainsKey(pluginName))
            {
                _pluginContexts[pluginName].Unload();
                _pluginContexts.Remove(pluginName);
            }
        }

        public static CollectibleAssemblyLoadContext GetContext(string pluginName)
        {
            return _pluginContexts[pluginName];
        }

        public static void AddPluginContext(string pluginNmae, CollectibleAssemblyLoadContext context)
        {
            _pluginContexts.Add(pluginNmae, context);
        }
    }
}
