using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Investor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">0: Command Executor | 1: Command In Symbol List | 2: Command In Investor List</param>
        /// <param name="commandID">CommandID Need Search</param>
        public static bool ProcessCheckCommand(int mode, int commandID,string symbolName,int investorID)
        {
            bool result = false;
            switch (mode)
            {
                case 0:
                    {
                        #region PROCESS IN COMMAND EXECUTOR
                        Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetOpenTradeByID(commandID);
                        Business.Market.CommandExecutor.Add(newOpenTrade);
                        result = true;
                        #endregion                        
                    }
                    break;

                case 1:
                    {
                        #region PROCESS IN SYMBOL LIST
                        Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetOpenTradeByID(commandID);                        
                        if (Business.Market.SymbolList != null && Business.Market.SymbolList.Count > 0)
                        {
                            for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                            {
                                if (Business.Market.SymbolList[i].Name == symbolName)
                                {
                                    if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                    {
                                        Business.Market.SymbolList[i].CommandList.Add(newOpenTrade);
                                        result = true;
                                    }

                                    break;
                                }
                            }
                        }
                        #endregion                        
                    }
                    break;

                case 2:
                    {
                        #region PROCESS INVESTOR LIST
                        Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetOpenTradeByID(commandID);
                        if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                        {
                            for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                            {
                                if (Business.Market.InvestorList[i].InvestorID == investorID)
                                {
                                    if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                    {
                                        Business.Market.InvestorList[i].CommandList.Add(newOpenTrade);
                                        result = true;
                                    }

                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    break;
            }

            return result;
        }
    }
}
