
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class OpenRemove
    {
        public int OpenTradeID { get; set; }
        public int InvestorID { get; set; }
        public string SymbolName { get; set; }
        public bool IsExecutor { get; set; }    //SETTING REMOVE COMMAND IN COMMAND EXECUTOR
        public bool IsInvestor { get; set; }    //SETTING REMOVE COMMAND IN INVESTOR LIST
        public bool IsSymbol { get; set; }      //SETTING REMOVE COMMAND IN SYMBOL LIST
    }
}
