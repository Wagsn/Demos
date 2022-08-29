using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Core.Domain.Response
{
    public class ControlLockModel
    {
        /// <summary>
        /// 仓位锁设备IP
        /// </summary>
        public string lockIP { get; set; }

        /// <summary>
        /// 仓位锁设备端口号
        /// </summary>
        public int lockPort { get; set; }

        /// <summary>
        /// 仓位锁设备控制板号
        /// </summary>
        public int lockBoardID { get; set; }

        /// <summary>
        /// 仓位锁设备号
        /// </summary>
        public int lockID { get; set; }

        /// <summary>
        /// 仓位锁状态
        /// </summary>
        public string lockCMD { get; set; }
    }

}
