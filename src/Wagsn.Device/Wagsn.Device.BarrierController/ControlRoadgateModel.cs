using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Core.Domain.Response
{
    public class ControlRoadgateModel
    {
        /// <summary>
        /// 控制指令
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 控制指令
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 控制指令
        /// </summary>
        public string Cmd { get; set; }

        /// <summary>
        /// 从站地址
        /// </summary>
        public string SlaveId { get; set; }
        /// <summary>
        /// 起始地址
        /// </summary>
        public string StartAddress { get; set; }
        /// <summary>
        /// 写入数据/读取寄存器数量
        /// </summary>
        public string NumInputs { get; set; }

    }

}
