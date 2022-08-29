using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.BoardLocker.Boshuo
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public class JsonResult
    {
        /// <summary>
        /// 执行结果 必填；0 表示操作成功，其他表示错 误，具体原因参见 Message 说明
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 说明信息
        /// </summary>
        public string Message { get; set; }
    }
}
