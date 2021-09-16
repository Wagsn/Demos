using System;
using System.Collections.Generic;
using System.Text;

namespace PluginBasic
{
    /// <summary>
    /// 插件描述对象
    /// </summary>
    public class PluginDescriptor : /*IComparable<PluginDescriptor>,*/ IEquatable<PluginDescriptor>
    {
        /// <summary>
        /// 插件dll文件名称
        /// </summary>
        public virtual string PhysicalPath { get; set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        public virtual string PluginName { get; set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public virtual string Version { get; set; }
        ///// <summary>
        ///// 比较优先级大小
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //public int CompareTo(PluginDescriptor other) => throw new NotImplementedException();
        /// <summary>
        /// 比较相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PluginDescriptor other) => PluginName == other.PluginName;
    }
}
