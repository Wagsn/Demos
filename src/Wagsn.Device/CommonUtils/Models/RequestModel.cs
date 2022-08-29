using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{

    public class RequestModel
    {
        public string method { get; set; }
        public string version { get; set; }
        public string appid { get; set; }
        public string format { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string sign { get; set; }
        public string data { get; set; }
    }
}
