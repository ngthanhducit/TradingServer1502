using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class DealMessage
    {
        public bool isDeal { get; set; }
        public Business.OpenTrade Command { get; set; }
        public string Error { get; set; }
    }
}
