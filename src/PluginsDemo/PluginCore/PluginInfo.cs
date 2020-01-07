using System;
using System.Collections.Generic;
using System.Text;

namespace PluginCore
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// 唯一插件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 插件DLL物理地址
        /// </summary>
        public string PhysicalPath { get; set; }
    }
}
