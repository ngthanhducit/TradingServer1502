using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    public class DealMessage
    {
        public bool isDeal { get; set; }
        public string Error { get; set; }
        public Command Command { get; set; }
    }
}
