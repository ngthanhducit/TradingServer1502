using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Agent
{
    public class IMasterAgentSymbol
    {
        public int IMasterAgentSymbolID { get; set; }
        public int MasterID { get; set; }
        public int AgentID { get; set; }
        public int SymbolID { get; set; }

        public List<Business.ParameterItem> IMasterAgentSymbolConfig { get; set; }
    }
}
