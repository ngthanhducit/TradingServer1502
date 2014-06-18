using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class AgentReport
    {
        public string AgentName { get; set; }
        public List<Business.OpenTrade> ListHistoryReport { get; set; }
    }
}
