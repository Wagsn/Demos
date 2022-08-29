using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.BoardLocker.Boshuo
{
    /// <summary>
    /// 门禁控制
    /// </summary>
    public class InMsg
    {
        /// <summary>
        /// 生产线编号 例如：生产线 1
        /// </summary>
        public string StatCode { get; set; }

        /// <summary>
        /// 通道编号 1 到 16
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 操作指令 0:关闭门禁, 非0:打开门禁
        /// </summary>
        public int Command { get; set; }
    }
}
