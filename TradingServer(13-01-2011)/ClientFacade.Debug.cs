using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer
{
    public static partial class ClientFacade
    {
        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string FacadeGetInvestorAccount(string code)
        {
            string result = string.Empty;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        result = "code: " + Business.Market.InvestorList[i].Code + " Margin Level: " + Business.Market.InvestorList[i].MarginLevel + " " +
                            "Margin: " + Business.Market.InvestorList[i].Margin + " Freeze Margin: " + Business.Market.InvestorList[i].FreezeMargin + " " +
                            "Equity: " + Business.Market.InvestorList[i].Equity + " Free Margin: " + Business.Market.InvestorList[i].FreeMargin + " " +
                            "Profit: " + Business.Market.InvestorList[i].Profit + " Balance: " + Business.Market.InvestorList[i].Balance + " Margin Stop out: " +
                            Business.Market.InvestorList[i].InvestorGroupInstance.MarginStopOut + " numcheck :" + Business.Market.InvestorList[i].NumCheck;

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> FacadeGetMonitorInvestor(string code)
        {
            List<string> result = new List<string>();

            if (Business.Market.InvestorList != null)
            {
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        result = Business.Market.InvestorList[i].ListMonitor;

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="code"></param>
        public static void FacadeResetMonitor(string code)
        {
            if (Business.Market.InvestorList != null)
            {
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        if(Business.Market.InvestorList[i].IsMonitor)
                            Business.Market.InvestorList[i].IsMonitor = false;

                        if (Business.Market.InvestorList[i].ListMonitor != null)
                            Business.Market.InvestorList[i].ListMonitor = new List<string>();

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isMonitor"></param>
        public static bool FacadeIsMonitor(string code, bool isMonitor)
        {
            bool result = false;
            if (Business.Market.InvestorList != null)
            {
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        Business.Market.InvestorList[i].IsMonitor = isMonitor;
                        result = true;

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static List<string> FacadeGetMonitorSymbol(string symbol)
        {
            List<string> result = new List<string>();
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == symbol)
                    {
                        result = Business.Market.SymbolList[i].ListSymbolMonitor;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isMonitor"></param>
        /// <returns></returns>
        public static bool FacadeIsMonitorSymbol(string symbol, bool isMonitor)
        {
            bool result = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == symbol)
                    {
                        Business.Market.SymbolList[i].isMonitorSymbol = isMonitor;
                        result = true;

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        public static List<string> FacadeGetClientCommandQueue(int investorID)
        {
            List<string> result = new List<string>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == investorID)
                    {
                        result = Business.Market.InvestorList[i].ClientCommandQueue;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> FacadeGetClientCommandQueue(string code)
        {
            List<string> result = new List<string>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        result = Business.Market.InvestorList[i].ClientCommandQueue;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="investorID"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static string FacadeGetCommandInThreeQueue(int commandID, int investorID, string symbolName)
        {
            string result = string.Empty;

            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == commandID)
                    {
                        result += "List Command Executor: orderID: #" + commandID + " profit: " + Business.Market.CommandExecutor[i].Profit + "<=>";
                        break;
                    }
                }
            }

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == investorID)
                    {
                        if (Business.Market.InvestorList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].ID == commandID)
                                {
                                    result += "List Command Investor: orderID: #" + commandID + " profit: " + Business.Market.InvestorList[i].CommandList[j].Profit + "<=>";
                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == symbolName)
                    {
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == commandID)
                                {
                                    result += "List Command Symbol: orderID: #" + commandID + " profit: " + Business.Market.SymbolList[i].CommandList[j].Profit + "<=>";
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string FacadeGetNumCheckMarketAread(int mode)
        {
            string result = string.Empty;
            if (mode == 1)
            {
                result = TradingServer.Business.SpotCommand.NumCheck.ToString();
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="mode">1: investor, 2: symbol,3: command executor</param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeGetCommandByID(int commandID, int mode)
        {
            Business.OpenTrade result = new Business.OpenTrade();
            switch (mode)
            {
                #region SEARCH COMMAND IN INVESTOR LIST
                case 1:
                    {
                        if (TradingServer.Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            bool flag = false;
                            for (int i = 0; i < count; i++)
                            {
                                if (flag)
                                    break;

                                if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                {
                                    int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                                    for (int j = 0; j < countCommand; j++)
                                    {
                                        if (Business.Market.InvestorList[i].CommandList[j].ID == commandID)
                                        {
                                            result = Business.Market.InvestorList[i].CommandList[j];
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region SEARCH COMMAND IN SYMBOL LIST
                case 2:
                    {
                        if (Business.Market.SymbolList != null)
                        {
                            int count = Business.Market.SymbolList.Count;
                            bool flag = false;
                            for (int i = 0; i < count; i++)
                            {
                                if (flag)
                                    break;

                                if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                {
                                    int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                                    for (int j = 0; j < countCommand; j++)
                                    {
                                        if (Business.Market.SymbolList[i].CommandList[j].ID == commandID)
                                        {
                                            result = Business.Market.SymbolList[i].CommandList[j];
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region SEARCH COMMAND IN COMMAND EXECUTOR
                case 3:
                    {
                        if (Business.Market.CommandExecutor != null)
                        {
                            int count = Business.Market.CommandExecutor.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.CommandExecutor[i].ID == commandID)
                                {
                                    result = Business.Market.CommandExecutor[i];

                                    break;
                                }
                            }
                        }
                    }
                    break;
                #endregion
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static Business.Symbol FacadeGetSymbolConfing(string symbolName)
        {
            Business.Symbol result = null;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == symbolName)
                    {
                        result = Business.Market.SymbolList[i];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static List<string> FacadeGetMultipleQuotes()
        {
            List<string> result = new List<string>();
            if (Business.Market.MultiplePriceQuotes != null)
            {
                int count = Business.Market.MultiplePriceQuotes.Count;
                for (int i = 0; i < count; i++)
                {
                    string message = "IPAddress: " + Business.Market.MultiplePriceQuotes[i].IpServer + " IsPrimary: " +
                        Business.Market.MultiplePriceQuotes[i].IsPrimary + " IsRecive: " + Business.Market.MultiplePriceQuotes[i].IsRecive +
                        " Status: " + Business.Market.MultiplePriceQuotes[i].Status + " Time Connection: " + Business.Market.MultiplePriceQuotes[i].TimeConnect;

                    result.Add(message);
                }
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static int FacadeGetTimeCheckMultipleQuotes()
        {
            return Business.Market.TimeCheckMultiplePrice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountListRemoveCommand()
        {
            return Business.Market.RemoveCommandList.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FacadeGetSymbolConfig(string name)
        {
            string value = string.Empty;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name.ToUpper().Trim() == name.ToUpper().Trim())
                    {
                        value += "Spread Balance: " + Business.Market.SymbolList[i].SpreadBalace + " Spread By Default: " + Business.Market.SymbolList[i].SpreadByDefault;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int FacadeCountInvestorByCode(string code)
        {
            int result = 0;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                        result++;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCoundCandles()
        {
            return ProcessQuoteLibrary.FacadeDataLog.FacadeCoundCandles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountTickQueue()
        {
            return ProcessQuoteLibrary.FacadeDataLog.FacadeCountTickQueue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> FacadeGetMessage(string code)
        {
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        return Business.Market.InvestorList[i].ClientCommandQueue;
                    }
                }
            }

            return null;
        }
    }
}
