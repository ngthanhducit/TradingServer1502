using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class ClientConfig
    {
        public double TickSize { get; set; }
        public double LeverageGroup { get; set; }
        public double MarginCall { get; set; }
        public double StopOut { get; set; }
        public int InvestorIndex { get; set; }
        public int TimeOut { get; set; }
        public string FreeMarginFormular { get; set; }
    }
}
