using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Agent
{
    public class IAgentInvestorSymbol
    {
        public int IAgentInvestorSymbolID { get; set; }
        public int AgentID { get; set; }
        public int InvestorID { get; set; }
        public int SymbolID { get; set; }
        public double PercentAgent { get; set; }

        public List<Business.ParameterItem> IAgentInvestorSymbolConfig { get; set; }
    }
}
