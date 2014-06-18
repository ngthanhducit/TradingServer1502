using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class LogContent
    {
        public int TypeID { get; set; }
        public string IPAddress { get; set; }
        public Business.ActionLog ActionLog { get; set; }
        public string Target { get; set; }
        public bool IsFrist { get; set; }
        public bool StatusLog { get; set; }
        public string Code { get; set; }
    }
}
