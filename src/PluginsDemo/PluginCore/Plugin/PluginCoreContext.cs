using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 公共插件上下文
    /// </summary>
    public class PluginCoreContext
    {
        /// <summary>
        /// DI容器
        /// </summary>
        public IServiceCollection Services { get; internal set; }
        /// <summary>
        /// 插件根路径
        /// </summary>
        public string PluginBasePath { get; internal set; }
        public IPluginFactory PluginFactory { get; internal set; }
        /// <summary>
        /// 加载的程序集
        /// </summary>
        public List<Assembly> AdditionalAssembly { get; internal set; } = new List<Assembly>();

        //public static PluginCoreContext Current { get; private set; }

        public PluginCoreContext()
        {
            string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
            DirectoryLoader dl = new DirectoryLoader();
            var ass = dl.LoadFromDirectory(pluginPath);
            this.AdditionalAssembly.Clear();
            this.AdditionalAssembly.AddRange(ass);
        }

        public Task<bool> Init()
        {
            string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
            PluginFactory.Load(pluginPath);
            return PluginFactory.Init(this);
        }

        public Task<bool> Start()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Stop()
        {
            return Task.FromResult(true);
        }
    }
}
