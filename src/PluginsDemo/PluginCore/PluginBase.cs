using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 插件基类
    /// 传入PluginCoreContext用来依赖注入
    /// </summary>
    public abstract class PluginBase<TConfig> : IPlugin, IPluginConfig<TConfig> where TConfig : class
    {
        public abstract string PluginName { get; set; }

        public abstract TConfig GetDefaultConfig();

        public PluginCoreContext PluginCoreCtext { get; set; }

        /// <summary>
        /// 配置更改
        /// </summary>
        /// <param name="newConfig"></param>
        /// <returns></returns>
        public virtual Task<PluginResultMessage> ConfigChanged(TConfig newConfig)
        {
            return Task.FromResult(new PluginResultMessage());
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public Task<TConfig> GetConfig()
        {
            TConfig cfg = PluginCoreCtext.PluginConfigStorage.GetConfig<TConfig>(PluginName).Result.Data;
            if (cfg == null)
            {
                cfg = GetDefaultConfig();
                PluginCoreCtext.PluginConfigStorage.SaveConfig(PluginName, cfg);
            }
            return Task.FromResult(cfg);
        }

        public virtual Task<PluginResultMessage> Init(PluginCoreContext context)
        {
            this.PluginCoreCtext = context;
            return Task.FromResult(new PluginResultMessage());
        }

        public virtual Task<bool> SaveConfig(TConfig cfg)
        {
            return Task.FromResult(true);
        }

        public virtual Task<PluginResultMessage> Start(PluginCoreContext context)
        {
            return Task.FromResult(new PluginResultMessage());
        }

        public virtual Task<PluginResultMessage> Stop(PluginCoreContext context)
        {
            return Task.FromResult(new PluginResultMessage());
        }
    }
}
