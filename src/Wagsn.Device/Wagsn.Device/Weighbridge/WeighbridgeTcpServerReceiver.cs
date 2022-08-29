using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Weighbridge
{
    public class WeighbridgeTcpServerReceiver : DeviceTcpServerReceiver<decimal?>
    {
        public override decimal? ReceiveConvert(object frame)
        {
            throw new NotImplementedException();
        }
    }
}
