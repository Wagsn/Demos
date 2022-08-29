using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.IcCard
{
    public class OperateResult
    {
        public bool IsSuccess { get; set; } = false;

        public string Message { get; set; }

        public object Model{ get; set; }
    }
}
