using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Net.WebSockets.DataCheck
{
    /// <summary>
    /// 数据检查
    /// </summary>
    public interface IDataCheck
    {
        bool TryCheck(byte[] bytes, out byte check);
    }
}
