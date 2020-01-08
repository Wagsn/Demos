using System.Collections.Generic;

namespace ApiServer.AspNetCore.Mvc
{
    /// <summary>
    /// 总插件加载上下文，用来统一管理每一个单独的插件加载上下文
    /// </summary>
    public static class PluginsLoadContext
    {
        private static readonly Dictionary<string, CollectibleAssemblyLoadContext> _pluginContexts = new Dictionary<string, CollectibleAssemblyLoadContext>();

        /// <summary>
        /// 是否存在该插件上下文
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public static bool Any(string pluginName)
        {
            return _pluginContexts.ContainsKey(pluginName);
        }

        /// <summary>
        /// 卸载并移出插件加载上下文
        /// </summary>
        /// <param name="pluginName"></param>
        public static void Remove(string pluginName)
        {
            if (_pluginContexts.ContainsKey(pluginName))
            {
                _pluginContexts[pluginName].Unload();
                _pluginContexts.Remove(pluginName);
            }
        }

        /// <summary>
        /// 获取指定插件的加载上下文
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public static CollectibleAssemblyLoadContext Get(string pluginName)
        {
            return _pluginContexts[pluginName];
        }

        /// <summary>
        /// 添加插件加载上下文
        /// 添加之前程序集已被添加到 PartManager.ApplicationParts 中
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="context"></param>
        public static void Add(string pluginName, CollectibleAssemblyLoadContext context)
        {
            _pluginContexts.Add(pluginName, context);
        }

        /// <summary>
        /// 清空插件加载上下文
        /// </summary>
        public static void Clear()
        {
            _pluginContexts.Clear();
        }
    }
}
