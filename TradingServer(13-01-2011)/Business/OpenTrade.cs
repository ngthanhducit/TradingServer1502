using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class OpenTrade : IPresenter.IOpenTrade, IDisposable
    {
        #region Create Instance Class DBWOnlineCommand
        private static DBW.DBWOnlineCommand dbwOnlineCommand;
        private static DBW.DBWOnlineCommand DBWOnlineCommandInstance
        {
            get
            {
                if (OpenTrade.dbwOnlineCommand == null)
                {
                    OpenTrade.dbwOnlineCommand = new DBW.DBWOnlineCommand();
                }
                return OpenTrade.dbwOnlineCommand;
            }
        }
        #endregion

        #region Create Instance Class DBWCommandHistory
        private static DBW.DBWCommandHistory dbwCommandHistory;
        private static DBW.DBWCommandHistory DBWCommandHistoryInstance
        {
            get
            {
                if (OpenTrade.dbwCommandHistory == null)
                {
                    OpenTrade.dbwCommandHistory = new DBW.DBWCommandHistory();
                }

                return OpenTrade.dbwCommandHistory;
            }
        }
        #endregion

        //============================================================================
        /// <summary>
        /// 
        /// </summary>
        public OpenTrade()
        {
            this.RefCommandID = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isStopLostTakeProfit"></param>
        public OpenTrade(bool isActive, bool isStopLostTakeProfit)
        {
            this.IsActivePending = isActive;
            this.IsStopLossAndTakeProfit = isStopLostTakeProfit;
        }
        
        #region Function Process On Ram
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommand(int From, int To)
        {
            List<Business.OpenTrade> Result = new List<OpenTrade>();
            List<Business.OpenTrade> tempResult = new List<OpenTrade>();
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                if (count < To)
                    To = count; 

                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].Type.ID == 1 || Business.Market.CommandExecutor[i].Type.ID == 2 ||
                        Business.Market.CommandExecutor[i].Type.ID == 7 || Business.Market.CommandExecutor[i].Type.ID == 8 ||
                        Business.Market.CommandExecutor[i].Type.ID == 9 || Business.Market.CommandExecutor[i].Type.ID == 10 ||
                        Business.Market.CommandExecutor[i].Type.ID == 11 || Business.Market.CommandExecutor[i].Type.ID == 12 ||
                        Business.Market.CommandExecutor[i].Type.ID == 17 || Business.Market.CommandExecutor[i].Type.ID == 18 ||
                        Business.Market.CommandExecutor[i].Type.ID == 19 || Business.Market.CommandExecutor[i].Type.ID == 20)
                    {
                        tempResult.Add(Business.Market.CommandExecutor[i]);
                    }
                }

                if (tempResult != null)
                {
                    int countResult = tempResult.Count;
                    if (countResult < To)
                        To = countResult;
                    for (int i = From; i < To; i++)
                    {
                        Result.Add(tempResult[i]);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="GroupList"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByGroupList(int From, int To, List<int> GroupList)
        {
            List<Business.OpenTrade> Result = new List<OpenTrade>();
            List<Business.OpenTrade> tempResult = new List<OpenTrade>();

            if (GroupList != null)
            {
                int count = GroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor != null)
                    {
                        int countCommand = Business.Market.CommandExecutor.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (GroupList[i] == Business.Market.CommandExecutor[j].Investor.InvestorGroupInstance.InvestorGroupID)
                            {
                                if (Business.Market.CommandExecutor[j].Type.ID == 1 || Business.Market.CommandExecutor[j].Type.ID == 2 ||
                                    Business.Market.CommandExecutor[j].Type.ID == 7 || Business.Market.CommandExecutor[j].Type.ID == 8 ||
                                    Business.Market.CommandExecutor[j].Type.ID == 9 || Business.Market.CommandExecutor[j].Type.ID == 10 ||
                                    Business.Market.CommandExecutor[j].Type.ID == 11 || Business.Market.CommandExecutor[j].Type.ID == 12 ||
                                    Business.Market.CommandExecutor[j].Type.ID == 17 || Business.Market.CommandExecutor[j].Type.ID == 18 ||
                                    Business.Market.CommandExecutor[j].Type.ID == 19 || Business.Market.CommandExecutor[j].Type.ID == 20)
                                {
                                    #region MAP DATA
                                    Business.OpenTrade newOpenTrade = new OpenTrade();
                                    newOpenTrade.ClientCode = Business.Market.CommandExecutor[j].ClientCode;
                                    newOpenTrade.ClosePrice = Business.Market.CommandExecutor[j].ClosePrice;
                                    newOpenTrade.CloseTime = Business.Market.CommandExecutor[j].CloseTime;
                                    newOpenTrade.CommandCode = Business.Market.CommandExecutor[j].CommandCode;
                                    newOpenTrade.Commission = Business.Market.CommandExecutor[j].Commission;
                                    newOpenTrade.ExpTime = Business.Market.CommandExecutor[j].ExpTime;
                                    newOpenTrade.ID = Business.Market.CommandExecutor[j].ID;
                                    newOpenTrade.IGroupSecurity = Business.Market.CommandExecutor[j].IGroupSecurity;
                                    newOpenTrade.Investor = Business.Market.CommandExecutor[j].Investor;
                                    newOpenTrade.IsClose = Business.Market.CommandExecutor[j].IsClose;
                                    newOpenTrade.Margin = Business.Market.CommandExecutor[j].Margin;
                                    newOpenTrade.MaxDev = Business.Market.CommandExecutor[j].MaxDev;
                                    newOpenTrade.OpenPrice = Business.Market.CommandExecutor[j].OpenPrice;
                                    newOpenTrade.OpenTime = Business.Market.CommandExecutor[j].OpenTime;
                                    newOpenTrade.Profit = Business.Market.CommandExecutor[j].Profit;
                                    newOpenTrade.Size = Business.Market.CommandExecutor[j].Size;
                                    newOpenTrade.SpreaDifferenceInOpenTrade = Business.Market.CommandExecutor[j].SpreaDifferenceInOpenTrade;
                                    newOpenTrade.StopLoss = Business.Market.CommandExecutor[j].StopLoss;
                                    newOpenTrade.Swap = Business.Market.CommandExecutor[j].Swap;
                                    newOpenTrade.Symbol = Business.Market.CommandExecutor[j].Symbol;
                                    newOpenTrade.TakeProfit = Business.Market.CommandExecutor[j].TakeProfit;
                                    newOpenTrade.Taxes = Business.Market.CommandExecutor[j].Taxes;
                                    newOpenTrade.Type = Business.Market.CommandExecutor[j].Type;
                                    newOpenTrade.Comment = Business.Market.CommandExecutor[j].Comment;
                                    newOpenTrade.AgentCommission = Business.Market.CommandExecutor[j].AgentCommission;
                                    newOpenTrade.IsActivePending = Business.Market.CommandExecutor[j].IsActivePending;
                                    newOpenTrade.IsStopLossAndTakeProfit = Business.Market.CommandExecutor[j].IsStopLossAndTakeProfit;
                                    #endregion

                                    tempResult.Add(newOpenTrade);
                                }
                            }
                        }
                    }
                }
            }

            if (tempResult != null && tempResult.Count > 0)
            {
                int count = tempResult.Count;
                if (count < To)
                    To = count;

                for (int i = From; i < To; i++)
                {
                    Result.Add(tempResult[i]);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByListInvestorCode(List<string> listCode)
        {
            List<Business.OpenTrade> result = new List<OpenTrade>();
            if (listCode != null)
            {
                int count = listCode.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].CommandList != null)
                            {
                                if (Business.Market.InvestorList[j].Code.Trim().ToUpper() == listCode[i].Trim().ToUpper())
                                {
                                    int countCommand = Business.Market.InvestorList[j].CommandList.Count;
                                    for (int n = 0; n < countCommand; n++)
                                    {
                                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.InvestorList[j].CommandList[n].Type.ID);
                                        if (!isPending)
                                            result.Add(Business.Market.InvestorList[j].CommandList[n]);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByListInvestorID(List<int> listInvestorID)
        {
            List<Business.OpenTrade> result = new List<OpenTrade>();
            if (listInvestorID != null)
            {
                int count = listInvestorID.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].CommandList != null)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == listInvestorID[i])
                                {
                                    int countCommand = Business.Market.InvestorList[j].CommandList.Count;
                                    for (int n = 0; n < countCommand; n++)
                                    {
                                        //bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.InvestorList[j].CommandList[n].Type.ID);
                                        //if (!isPending)
                                        result.Add(Business.Market.InvestorList[j].CommandList[n]);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    
        /// <summary>
        /// GET ONLINE COMMAND BY COMMAND ID IN CLASS MARKET OF COMMAND EXECUTOR LIST
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetOnlineCommandByOnlineCommandID(int CommandID)
        {
            Business.OpenTrade result = new OpenTrade();
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j].ID == CommandID)
                            {
                                return Business.Market.SymbolList[i].CommandList[j];
                            }                            
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOpenTradeByInvestorID(int InvestorID)
        {
            List<Business.OpenTrade> Result = new List<OpenTrade>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        if (Business.Market.InvestorList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].Type.ID == 1 ||
                                    Business.Market.InvestorList[i].CommandList[j].Type.ID == 2 ||
                                    Business.Market.InvestorList[i].CommandList[j].Type.ID == 11 ||
                                    Business.Market.InvestorList[i].CommandList[j].Type.ID == 12)
                                {
                                    Business.OpenTrade newOpenTrade = new OpenTrade();
                                    newOpenTrade.ClientCode = Business.Market.InvestorList[i].CommandList[j].ClientCode;
                                    newOpenTrade.ClosePrice = Business.Market.InvestorList[i].CommandList[j].ClosePrice;
                                    newOpenTrade.CloseTime = Business.Market.InvestorList[i].CommandList[j].CloseTime;
                                    newOpenTrade.CommandCode = Business.Market.InvestorList[i].CommandList[j].CommandCode;
                                    newOpenTrade.Commission = Business.Market.InvestorList[i].CommandList[j].Commission;
                                    newOpenTrade.ExpTime = Business.Market.InvestorList[i].CommandList[j].ExpTime;
                                    newOpenTrade.ID = Business.Market.InvestorList[i].CommandList[j].ID;
                                    newOpenTrade.IGroupSecurity = Business.Market.InvestorList[i].CommandList[j].IGroupSecurity;
                                    newOpenTrade.Investor = Business.Market.InvestorList[i].CommandList[j].Investor;
                                    newOpenTrade.IsClose = Business.Market.InvestorList[i].CommandList[j].IsClose;
                                    newOpenTrade.IsHedged = Business.Market.InvestorList[i].CommandList[j].IsHedged;
                                    newOpenTrade.Margin = Business.Market.InvestorList[i].CommandList[j].Margin;
                                    newOpenTrade.MaxDev = Business.Market.InvestorList[i].CommandList[j].MaxDev;
                                    newOpenTrade.OpenPrice = Business.Market.InvestorList[i].CommandList[j].OpenPrice;
                                    newOpenTrade.OpenTime = Business.Market.InvestorList[i].CommandList[j].OpenTime;
                                    newOpenTrade.Profit = Business.Market.InvestorList[i].CommandList[j].Profit;
                                    newOpenTrade.Size = Business.Market.InvestorList[i].CommandList[j].Size;
                                    newOpenTrade.StopLoss = Business.Market.InvestorList[i].CommandList[j].StopLoss;
                                    newOpenTrade.Swap = Business.Market.InvestorList[i].CommandList[j].Swap;
                                    newOpenTrade.Symbol = Business.Market.InvestorList[i].CommandList[j].Symbol;
                                    newOpenTrade.TakeProfit = Business.Market.InvestorList[i].CommandList[j].TakeProfit;
                                    newOpenTrade.Taxes = Business.Market.InvestorList[i].CommandList[j].Taxes;
                                    newOpenTrade.Type = Business.Market.InvestorList[i].CommandList[j].Type;
                                    newOpenTrade.Comment = Business.Market.InvestorList[i].CommandList[j].Comment;
                                    newOpenTrade.AgentCommission = Business.Market.InvestorList[i].CommandList[j].AgentCommission;

                                    Result.Add(newOpenTrade);
                                }
                            }
                        }
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOpenTradeByInvestorWithStartEnd(int investorID, int start, int end)
        {
            List<Business.OpenTrade> Result = new List<OpenTrade>();
            List<Business.OpenTrade> tempResult = new List<OpenTrade>();

            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].Investor.InvestorID == investorID)
                    {
                        #region MAP DATA
                        Business.OpenTrade newOpenTrade = new OpenTrade();
                        newOpenTrade.ClientCode = Business.Market.CommandExecutor[i].ClientCode;
                        newOpenTrade.ClosePrice = Business.Market.CommandExecutor[i].ClosePrice;
                        newOpenTrade.CloseTime = Business.Market.CommandExecutor[i].CloseTime;
                        newOpenTrade.CommandCode = Business.Market.CommandExecutor[i].CommandCode;
                        newOpenTrade.Commission = Business.Market.CommandExecutor[i].Commission;
                        newOpenTrade.ExpTime = Business.Market.CommandExecutor[i].ExpTime;
                        newOpenTrade.ID = Business.Market.CommandExecutor[i].ID;
                        newOpenTrade.IGroupSecurity = Business.Market.CommandExecutor[i].IGroupSecurity;
                        newOpenTrade.Investor = Business.Market.CommandExecutor[i].Investor;
                        newOpenTrade.IsClose = Business.Market.CommandExecutor[i].IsClose;
                        newOpenTrade.Margin = Business.Market.CommandExecutor[i].Margin;
                        newOpenTrade.MaxDev = Business.Market.CommandExecutor[i].MaxDev;
                        newOpenTrade.OpenPrice = Business.Market.CommandExecutor[i].OpenPrice;
                        newOpenTrade.OpenTime = Business.Market.CommandExecutor[i].OpenTime;
                        newOpenTrade.Profit = Business.Market.CommandExecutor[i].Profit;
                        newOpenTrade.Size = Business.Market.CommandExecutor[i].Size;
                        newOpenTrade.SpreaDifferenceInOpenTrade = Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade;
                        newOpenTrade.StopLoss = Business.Market.CommandExecutor[i].StopLoss;
                        newOpenTrade.Swap = Business.Market.CommandExecutor[i].Swap;
                        newOpenTrade.Symbol = Business.Market.CommandExecutor[i].Symbol;
                        newOpenTrade.TakeProfit = Business.Market.CommandExecutor[i].TakeProfit;
                        newOpenTrade.Taxes = Business.Market.CommandExecutor[i].Taxes;
                        newOpenTrade.Type = Business.Market.CommandExecutor[i].Type;
                        newOpenTrade.Comment = Business.Market.CommandExecutor[i].Comment;
                        newOpenTrade.AgentCommission = Business.Market.CommandExecutor[i].AgentCommission;
                        #endregion

                        tempResult.Add(newOpenTrade);
                    }
                }
            }

            if (tempResult != null)
            {
                if (tempResult.Count < end)
                    end = tempResult.Count;

                for (int i = start; i < end; i++)
                {
                    Result.Add(tempResult[i]);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <returns></returns>
        internal bool MultiUpdateCommandExecutor(double stopLoss, double takeProfit, double openPrices, int commandID)
        {
            bool result = false;

            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == commandID)
                    {
                        if (Business.Market.CommandExecutor[i].Type.ID == 1 || Business.Market.CommandExecutor[i].Type.ID == 11)
                        {
                            Business.Market.CommandExecutor[i].StopLoss = stopLoss;
                            Business.Market.CommandExecutor[i].TakeProfit = takeProfit;
                            result = true;

                            break;
                        }
                        else if (Business.Market.CommandExecutor[i].Type.ID == 2 || Business.Market.CommandExecutor[i].Type.ID == 12)
                        {
                            Business.Market.CommandExecutor[i].StopLoss = takeProfit;
                            Business.Market.CommandExecutor[i].TakeProfit = stopLoss;
                            result = true;

                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <param name="openPrice"></param>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal bool MultiUpdateCommandSymbolList(double stopLoss, double takeProfit, double openPrice, int commandID)
        {
            bool result = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (result)
                        break;

                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j].ID == commandID)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].Type.ID == 1 || Business.Market.SymbolList[i].CommandList[j].Type.ID == 11)
                                {
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss = stopLoss;
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit = takeProfit;
                                    result = true;

                                    break;
                                }
                                else if (Business.Market.SymbolList[i].CommandList[j].Type.ID == 2 || Business.Market.SymbolList[i].CommandList[j].Type.ID == 12)
                                {
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss = takeProfit;
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit = stopLoss;
                                    result = true;

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRollBack"></param>
        /// <param name="mode">1: remove and add new | 2: update s/l and t/p</param>
        /// <returns></returns>
        internal bool RollBackOpenTradeInCommandExe(Business.OpenTrade dataRollBack,int mode)
        {
            bool result = false;
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == dataRollBack.ID)
                    {
                        if (mode == 1)
                        {
                            Business.Market.CommandExecutor.Remove(Business.Market.CommandExecutor[i]);
                            Business.Market.CommandExecutor.Add(dataRollBack);
                            result = true;
                        }
                        else if (mode == 2)
                        {
                            Business.Market.CommandExecutor[i].StopLoss = dataRollBack.StopLoss;
                            Business.Market.CommandExecutor[i].TakeProfit = dataRollBack.TakeProfit;
                        }

                        break;
                    }
                }
            }

            if (!result)
            {
                if (mode == 1)
                {
                    if (Business.Market.CommandExecutor == null)
                        Business.Market.CommandExecutor = new List<OpenTrade>();

                    Business.Market.CommandExecutor.Add(dataRollBack);
                }

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRollBack"></param>
        /// <param name="mode">1: remove and add new | 2: update s/l and t/p</param>
        /// <returns></returns>
        internal bool RollBackOpenTradeInSymbolList(Business.OpenTrade dataRollBack,int mode)
        {
            bool result = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].SymbolID == dataRollBack.Symbol.SymbolID &&
                        Business.Market.SymbolList[i].Name == dataRollBack.Symbol.Name)
                    {
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == dataRollBack.ID)
                                {
                                    if (mode == 1)
                                    {
                                        Business.Market.SymbolList[i].CommandList.Remove(Business.Market.SymbolList[i].CommandList[j]);
                                        Business.Market.SymbolList[i].CommandList.Add(dataRollBack);
                                        result = true;
                                    }
                                    else if (mode == 2)
                                    {
                                        Business.Market.SymbolList[i].CommandList[j].StopLoss = dataRollBack.StopLoss;
                                        Business.Market.SymbolList[i].CommandList[j].TakeProfit = dataRollBack.TakeProfit;
                                        result = true;  
                                    }

                                    break;
                                }
                            }
                        }

                        if (!result)
                        {
                            if (mode == 1)
                            {
                                if (Business.Market.SymbolList[i].CommandList == null)
                                    Business.Market.SymbolList[i].CommandList = new List<OpenTrade>();

                                Business.Market.SymbolList[i].CommandList.Add(dataRollBack);
                            }

                            result = true;
                        }

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRollBack"></param>
        /// <param name="mode">1: remove and add new | 2: update s/l and t/p</param>
        /// <returns></returns>
        internal bool RollBackOpenTradeInInvestor(Business.OpenTrade dataRollBack,int mode)
        {
            bool result = false;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == dataRollBack.Investor.InvestorID)
                    {
                        if (Business.Market.InvestorList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].ID == dataRollBack.ID)
                                {
                                    if (mode == 1)
                                    {
                                        Business.Market.InvestorList[i].CommandList.Remove(Business.Market.InvestorList[i].CommandList[j]);
                                        Business.Market.InvestorList[i].CommandList.Add(dataRollBack);
                                        result = true;
                                    }
                                    else if(mode == 2)
                                    {
                                        Business.Market.InvestorList[i].CommandList[j].StopLoss = dataRollBack.StopLoss;
                                        Business.Market.InvestorList[i].CommandList[j].TakeProfit = dataRollBack.TakeProfit;
                                        result = true;
                                    }

                                    break;
                                }
                            }
                        }

                        if (!result)
                        {
                            if (mode == 1)
                            {
                                if (Business.Market.InvestorList[i].CommandList == null)
                                    Business.Market.InvestorList[i].CommandList = new List<OpenTrade>();

                                Business.Market.InvestorList[i].CommandList.Add(dataRollBack);
                            }

                            result = true;
                        }

                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isStopLoss"></param>
        public void UpdateIsActivePending(bool isActive, bool isStopLoss, int commandID, int investorID)
        {
            #region Find In List Symbol And Update Take Profit, Stop Loss
            if (Business.Market.SymbolList != null)
            {
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;

                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j].ID == commandID)
                            {
                                Business.Market.SymbolList[i].CommandList[j].IsActivePending = isActive;
                                Business.Market.SymbolList[i].CommandList[j].IsStopLossAndTakeProfit = isStopLoss;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region FIND IN LIST COMMAND EXECUTOR AND UPDATE TAKE PROFIT , STOP LOSS
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == commandID)
                    {
                        Business.Market.CommandExecutor[i].IsActivePending = isActive;
                        Business.Market.CommandExecutor[i].IsStopLossAndTakeProfit = isStopLoss;

                        break;
                    }
                }
            }
            #endregion

            #region FIND IN LIST INVESTOR AND UPDATE ISACTIVEPENDING
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
                                    Business.Market.InvestorList[i].CommandList[j].IsActivePending = isActive;
                                    Business.Market.InvestorList[i].CommandList[j].IsStopLossAndTakeProfit = isStopLoss;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion

            this.UpdateIsActivePending(isActive, isStopLoss, commandID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openTrade"></param>
        public bool CheckRuleTrader(Business.OpenTrade openTrade, double size)
        {
            bool result = true;

            if (openTrade.Investor != null)
            {
                List<Business.OpenTrade> commands = new List<OpenTrade>();
                int count = openTrade.Investor.CommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    bool _isPending = Model.TradingCalculate.Instance.CheckIsPendingPosition(openTrade.Investor.CommandList[i].Type.ID);
                    if (!_isPending)
                    {
                        commands.Add(openTrade.Investor.CommandList[i]);
                    }
                }

                if (commands != null && commands.Count > 0)
                {
                    bool isPending = Model.TradingCalculate.Instance.CheckIsPendingPosition(openTrade.Type.ID);
                    if (isPending)
                    {
                        result = this.IsRuleTrader(commands, openTrade, size);
                    }
                    else
                        result = true;
                }
                else
                {
                    #region NO COMMAND COMPARE WITH PENDING ORDER
                    //7 BUYLIMIT | 8 SELLLIMIT | 9 BUYSTOP | 10 SELLSTOP
                    //ACCOUT WITH NO COMMAND THEN BUYSTOP NOT OK | BUYLIMIT OK | SELLSTOP NOT OK | SELLLIMIT OK
                    bool isPending = Model.TradingCalculate.Instance.CheckIsPendingPosition(openTrade.Type.ID);
                    if (isPending)
                    {
                        if (openTrade.Type.ID == 9 || openTrade.Type.ID == 10)
                            result = false;

                        if (openTrade.Type.ID == 7 || openTrade.Type.ID == 8)
                            result = true;
                    }
                    #endregion
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="openTrade"></param>
        /// <returns></returns>
        private bool IsRuleTrader(List<Business.OpenTrade> commands, Business.OpenTrade openTrade, double size)
        {
            bool result = true;

            if (commands != null && openTrade != null)
            {
                //1 BUYSPOT | 2 SELLSPOT | 7 BUYLIMIT | 8 SELLLIMIT | 9 BUYSTOP | 10 SELLSTOP
                int count = commands.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!result)
                        break;

                    //OPEN TRADE WITH TYPE IS BUY SPOT
                    if (commands[i].Type.ID == 1)
                    {
                        switch (openTrade.Type.ID)
                        {
                            case 7:
                                //ALL CASE IT OK
                                break;

                            case 8:
                                //ALL CASE IT OK
                                break;

                            case 9:
                                result = false;
                                break;

                            case 10:
                                TradingServer.Facade.FacadeAddNewSystemLog(5, openTrade.Size + " - " + commands[i].Size, "[trade is disabled]", "", "");
                                if (size > commands[i].Size)
                                    result = false;
                                break;
                        }
                    }

                    //OPEN TRADE WITH TYPE IS SELL SPOT
                    if (commands[i].Type.ID == 2)
                    {
                        switch (openTrade.Type.ID)
                        {
                            case 7:
                                break;

                            case 8:
                                break;

                            case 9:
                                TradingServer.Facade.FacadeAddNewSystemLog(5, openTrade.Size + " - " + commands[i].Size, "[trade is disabled]", "", "");
                                if (size > commands[i].Size)
                                    result = false;
                                break;

                            case 10:
                                result = false;
                                break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openTrade"></param>
        /// <returns></returns>
        public bool IsEnableCheckRuleTrade(Business.OpenTrade openTrade)
        {
            bool isEnable = false;

            if (openTrade != null)
            {
                if (openTrade.Investor != null)
                {
                    if (openTrade.Investor.InvestorGroupInstance != null)
                    {
                        if (openTrade.Investor.InvestorGroupInstance.ParameterItems != null)
                        {
                            int count = openTrade.Investor.InvestorGroupInstance.ParameterItems.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (openTrade.Investor.InvestorGroupInstance.ParameterItems[i].Code == "G39")
                                {
                                    if (openTrade.Investor.InvestorGroupInstance.ParameterItems[i].BoolValue == 1)
                                        isEnable = true;
                                    else
                                        isEnable = false;

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            
            return isEnable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openTrade"></param>
        public void CheckRuleCloseOpenPosition(List<Business.OpenTrade> openTrades, double sizes)
        {
            if (openTrades != null)
            {
                List<Business.OpenTrade> listOpenPosition = new List<OpenTrade>();
                List<Business.OpenTrade> listPendingPosition = new List<OpenTrade>();

                Business.OpenTrade openTrade = openTrades[0];
                bool isEnable = false;
                if (openTrade != null)
                {
                    isEnable = this.IsEnableCheckRuleTrade(openTrade);
                }

                if (!isEnable)
                    return;

                #region GET OPEN POSITION AND PENDING POSITION
                if (openTrades != null && openTrades.Count > 0)
                {
                    int count = openTrades.Count;
                    for (int i = 0; i < count; i++)
                    {
                        bool isPending = Model.TradingCalculate.Instance.CheckIsPendingPosition(
                                                                        openTrades[i].Type.ID);

                        if (isPending)
                            listPendingPosition.Add(openTrades[i]);
                        else
                            listOpenPosition.Add(openTrades[i]);
                    }
                }
                #endregion
               
                if (listPendingPosition != null)
                {
                    int count = listPendingPosition.Count;
                    for (int i = 0; i < count; i++)
                    {
                        //1 BUYSPOT | 2 SELLSPOT | 7 BUYLIMIT | 8 SELLLIMIT | 9 BUYSTOP | 10 SELLSTOP
                        switch (listPendingPosition[i].Type.ID)
                        {
                            case 7:
                                //ALL CASE IT OK
                                break;

                            case 8:
                                //ALL CASE IT OK
                                break;

                            case 9:
                                //CASE BUY STOP. WE WILL CHECK OPEN POSITION SELL
                                if (listOpenPosition != null)
                                {
                                    double totalSize = 0;
                                    bool isExists = false;
                                    int countOpenPosition = listOpenPosition.Count;
                                    for (int j = 0; j < countOpenPosition; j++)
                                    {
                                        if (listOpenPosition[j].Type.ID == 2)
                                        {
                                            totalSize += listOpenPosition[j].Size;
                                            isExists = true;
                                        }
                                    }

                                    if (isExists)
                                    {
                                        if (totalSize < listPendingPosition[i].Size)
                                        {
                                            //UPDATE PENDING ORDER WITH NEW SIZE
                                            listPendingPosition[i].Size = sizes;
                                            listPendingPosition[i].IsClose = false;
                                            this.UpdateRuleClosePending(listPendingPosition[i], sizes);
                                        }
                                    }
                                    else
                                    {
                                        //CLOSE PENDING ORDER POSITION WITH TYPE IS BUY STOP
                                        listPendingPosition[i].IsClose = true;
                                        listPendingPosition[i].Symbol.MarketAreaRef.CloseCommand(listPendingPosition[i]);
                                    }
                                }
                                else
                                {
                                    //CLOSE PENDING ORDER POSITION WITH TYPE IS BUY STOP
                                    listPendingPosition[i].IsClose = true;
                                    listPendingPosition[i].Symbol.MarketAreaRef.CloseCommand(listPendingPosition[i]);
                                }
                                break;

                            case 10:
                                //CASE SELL STOP. WE WILL CHECK OPEN POSITION BUY
                                if (listOpenPosition != null)
                                {
                                    double totalSize = 0;
                                    bool isExists = false;
                                    int countOpenPosition = listOpenPosition.Count;
                                    for (int j = 0; j < countOpenPosition; j++)
                                    {
                                        if (listOpenPosition[j].Type.ID == 1)
                                        {
                                            totalSize += listOpenPosition[j].Size;
                                            isExists = true;
                                        }
                                    }

                                    if (isExists)
                                    {
                                        if (totalSize < listPendingPosition[i].Size)
                                        {
                                            //UPDATE PENDING ORDER WITH NEW SIZE
                                            listPendingPosition[i].Size = sizes;
                                            listPendingPosition[i].IsClose = false;
                                            this.UpdateRuleClosePending(listPendingPosition[i], sizes);
                                        }
                                    }
                                    else
                                    {
                                        //CLOSE PENDING ORDER POSITION WITH TYPE IS SELL STOP
                                        listPendingPosition[i].IsClose = true;
                                        listPendingPosition[i].Symbol.MarketAreaRef.CloseCommand(listPendingPosition[i]);
                                    }
                                }
                                else
                                {
                                    //CLOSE PENDING ORDER POSITION WITH TYPE IS SELL STOP
                                    listPendingPosition[i].IsClose = true;
                                    listPendingPosition[i].Symbol.MarketAreaRef.CloseCommand(listPendingPosition[i]);
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openTrade"></param>
        private void UpdateRuleClosePending(Business.OpenTrade openTrade, double Size)
        {
            string content = string.Empty;
            string comment = "[system modify order]";
            string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(openTrade.Type.ID).ToLower();

            string strSize = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Size.ToString(), 2);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openTrade.OpenPrice.ToString(), openTrade.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openTrade.StopLoss.ToString(), openTrade.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openTrade.TakeProfit.ToString(), openTrade.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openTrade.Symbol.TickValue.Bid.ToString(), openTrade.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openTrade.Symbol.TickValue.Ask.ToString(), openTrade.Symbol.Digit);

            #region Find In List Symbol And Update SIZE
            if (Business.Market.SymbolList != null)
            {
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;

                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j].ID == openTrade.ID)
                            {  
                                string Message = string.Empty;

                                Business.Market.SymbolList[i].CommandList[j].Size = Size;

                                #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                                string Content = string.Empty;
                                Content = "'system': modified #" + openTrade.CommandCode + " " + mode + " " + strSize + " " +
                                    openTrade.Symbol.Name + " at " + openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";

                                TradingServer.Facade.FacadeAddNewSystemLog(5, Content, comment, openTrade.Investor.IpAddress, openTrade.Investor.Code);
                                #endregion

                                TradingServer.Facade.FacadeUpdateOnlineCommandWithTakeProfit(TakeProfit, StopLoss, openTrade.ID, openTrade.Comment,
                                                                                                openTrade.OpenPrice);

                                #region MAP COMMAND TO CLIENT
                                Message = "UpdateCommand$True,UPDATE COMMAND COMPLETE," + openTrade.ID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Symbol.Name + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Size + "," + false + "," +
                                    Business.Market.SymbolList[i].CommandList[j].OpenTime + "," +
                                    Business.Market.SymbolList[i].CommandList[j].OpenPrice + "," +
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss + "," +
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit + "," +
                                    Business.Market.SymbolList[i].CommandList[j].ClosePrice + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Commission + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Swap + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Profit + "," + "Comment," +
                                    Business.Market.SymbolList[i].CommandList[j].ID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Type.Name + "," +
                                    1 + "," + Business.Market.SymbolList[i].CommandList[j].ExpTime + "," +
                                    Business.Market.SymbolList[i].CommandList[j].ClientCode + "," +
                                    Business.Market.SymbolList[i].CommandList[j].CommandCode + "," +
                                    Business.Market.SymbolList[i].CommandList[j].IsHedged + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Type.ID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Margin + ",Update";

                                //If Client Update Command Then Add Message To Client Message
                                if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                    Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(Message);
                                #endregion

                                //SEND NOTIFY TO AGENT SERVER
                                Message += "," + Business.Market.SymbolList[i].CommandList[j].AgentRefConfig + "," +
                                    Business.Market.SymbolList[i].CommandList[j].SpreaDifferenceInOpenTrade;
                                Business.AgentNotify newAgentNotify = new AgentNotify();
                                newAgentNotify.NotifyMessage = Message;
                                TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, 
                                                            Business.Market.SymbolList[i].CommandList[j].Investor.InvestorGroupInstance);

                                Flag = true;

                                //SENT COMMAND REMOVE COMMAND TO MANAGER
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.SymbolList[i].CommandList[j]);

                                //SENT NOTIFY ADD COMMAND TO MANAGER
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Business.Market.SymbolList[i].CommandList[j]);

                                //SEND NOTIFY TO MANAGE CHANGE ACCOUNT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.SymbolList[i].CommandList[j].Investor);

                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region FIND IN LIST COMMAND EXECUTOR AND UPDATE SIZE
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == openTrade.ID)
                    {
                        Business.Market.CommandExecutor[i].Size = Size;

                        break;
                    }
                }
            }
            #endregion

            #region FIND IN INVESTOR COMMAND LIST AND UPDATE SIZE
            if (openTrade.Investor.CommandList != null)
            {
                int count = openTrade.Investor.CommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (openTrade.Investor.CommandList[i].ID == openTrade.ID)
                    {
                        openTrade.Investor.CommandList[i].Size = Size;
                        break;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {            
            this.Investor = null;
            this.IGroupSecurity = null;
            this.Symbol = null;
            this.Type = null;

            GC.SuppressFinalize(this);
        }
    }
}
