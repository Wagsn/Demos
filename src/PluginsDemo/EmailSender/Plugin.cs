using PluginCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender
{
    /// <summary>
    /// 插件
    /// </summary>
    public class Plugin : PluginBase<PluginConfig>
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public override string PluginName { get; set; } = "EmailSender";
        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns>默认插件配置</returns>
        public override PluginConfig GetDefaultConfig()
        {
            return new PluginConfig() 
            {
                ConnectString = "Server=localhost;database=emailsender;uid=root;pwd=123456;Pooling=true;ConnectionReset=false;maxpoolsize=20",
                Email = "wagsn@foxmail.com",
                UserName = "Wagsn",
                Key = "Email-Key"
            };
        }
    }
}
