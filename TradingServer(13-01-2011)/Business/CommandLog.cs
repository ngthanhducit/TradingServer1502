using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class CommandLog
    {
        public int CommandLogID { get; set; }
        public int InvestorID { get; set; }
        public int CommandActionID { get; set; }
        public int CommandHistoryID { get; set; }
        public int OnlineCommandID { get; set; }
        public string LogContent { get; set; }
        public DateTime LogDate { get; set; }
    }
}
