using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class InvestorGroupConfig
    {
        public int InvestorGroupConfigID { get; set; }
        public int InvestorGroupID { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }
}
