using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    public class DataServer
    {
        public ClientBusiness.ChangeCommandQueue NumUpdate { get; set; }
        public ClientBusiness.ChangeCommandQueue NumUpdatePending { get; set; }
        public ClientBusiness.ChangeCommandQueue NumUpdateOption { get; set; }
        public ClientBusiness.ChangeCommandQueue NumUpdateChat { get; set; }
        public int NumAllInvestorOnline { get; set; }
        public int NumUpdateNews { get; set; }
        public int NumUpdateAnnouncement { get; set; }
        public List<Business.Tick> Tick { get; set; }
        public DateTime TimeServer { get; set; }
        public List<string> ClientMessage { get; set; }
        public int InvestorIndex { get; set; }
    }
}
