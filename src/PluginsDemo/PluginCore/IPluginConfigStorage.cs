using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 插件存储
    /// </summary>
    public interface IPluginConfigStorage
    {
        /// <summary>
        /// 获取插件配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        Task<PluginResultMessage<TConfig>> GetConfig<TConfig>(string pluginName);

        /// <summary>
        /// 保存插件配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="pluginName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<PluginResultMessage> SaveConfig<TConfig>(string pluginName, TConfig config);

        /// <summary>
        /// 删除插件配置
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        Task<PluginResultMessage> DeleteConfig(string pluginName);
    }
}
