using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Net.WebSockets
{
    public class WebsocketMessage
    {
        public string MessageID { get; set; }

        public string SendID { get; set; }

        public string AcceptID { get; set; }

        public string Action { get; set; }

        public string Msg { get; set; }
    }
}
