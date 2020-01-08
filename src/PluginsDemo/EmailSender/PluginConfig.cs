using PluginCore;

namespace EmailSender
{
    /// <summary>
    /// 插件配置
    /// </summary>
    public class PluginConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectString { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }
    }
}
