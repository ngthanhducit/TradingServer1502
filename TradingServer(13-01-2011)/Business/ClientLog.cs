using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class ClientLog
    {
        public int InvestorID { get; set; }
        public string AdminCode { get; set; }
        public int AdminID { get; set; }
        public int ManagerID { get; set; }
        public string InvestorCode { get; set; }
        public List<string> ClientLogs { get; set; }//Title{Message{Date
        public int ClientDevice { get; set; }
        public bool IsComplete { get; set; }
    }
}
