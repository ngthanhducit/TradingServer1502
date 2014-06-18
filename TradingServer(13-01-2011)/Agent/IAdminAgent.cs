using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Agent
{
    public class IAdminAgent
    {
        public int IAdminAgentID { get; set; }
        public int AdminID { get; set; }
        public int AgentID { get; set; }
        public int SymbolID { get; set; }
        public double DefaultPL { get; set; }
        public bool IsDelete { get; set; }
        public double PercentPL { get; set; }
        public bool IsSkipRisk { get; set; }

        public List<Business.ParameterItem> IAdminAgentConfig { get; set; }
    }
}
