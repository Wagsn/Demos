using System.Collections.Generic;

namespace ApiServer.AspNetCore.Mvc
{
    /// <summary>
    /// 总插件加载上下文，用来统一管理每一个单独的插件加载上下文
    /// </summary>
    public static class PluginsLoadContext
    {
        private static readonly Dictionary<string, CollectibleAssemblyLoadContext> _pluginContexts = new Dictionary<string, CollectibleAssemblyLoadContext>();

        public static bool Any(string pluginName)
        {
            return _pluginContexts.ContainsKey(pluginName);
        }

        public static void Remove(string pluginName)
        {
            if (_pluginContexts.ContainsKey(pluginName))
            {
                _pluginContexts[pluginName].Unload();
                _pluginContexts.Remove(pluginName);
            }
        }

        public static CollectibleAssemblyLoadContext Get(string pluginName)
        {
            return _pluginContexts[pluginName];
        }

        public static void Add(string pluginName, CollectibleAssemblyLoadContext context)
        {
            _pluginContexts.Add(pluginName, context);
        }
    }
}
