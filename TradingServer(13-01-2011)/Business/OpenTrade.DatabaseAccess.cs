﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class OpenTrade
    {
        /// <summary>
        /// GET ALL ONLINE COMMAND IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetAllOnlineCommand()
        {
            return OpenTrade.DBWOnlineCommandInstance.GetAllOnlineCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.OpenTrade> InitOnlineCommand()
        {
            return OpenTrade.DBWOnlineCommandInstance.InitOnlineCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ReCalculationAccount()
        {
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].CommandList.Count > 0 && Business.Market.InvestorList[i].CommandList != null)
                    {
                        Business.Market.InvestorList[i].ReCalculationAccountInit();

                        //Business.Market.InvestorList[i].Margin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                    }
                }
            }
        }

        /// <summary>
        /// GET ONLINE COMMAND BY SYMBOL ID IN DATABASE
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandBySymbolID(int SymbolID)
        {
            return OpenTrade.DBWOnlineCommandInstance.GetOnlineCommandBySymbolID(SymbolID);
        }

        /// <summary>
        /// GET ONLINE COMMAND BY INVESTOR ID IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByInvestorID(int InvestorID)
        {
            return OpenTrade.DBWOnlineCommandInstance.GetOnlineCommandByInvestorID(InvestorID);
        }

        /// <summary>
        /// GET ONLINE COMMAND BY COMMAND TYPE ID IN DATABASE
        /// </summary>
        /// <param name="CommandTypeID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByCommandTypeID(int CommandTypeID)
        {
            return OpenTrade.DBWOnlineCommandInstance.GetOnlineCommandByCommandTypeID(CommandTypeID);
        }

        /// <summary>
        /// GET ONLINE COMMAND BY ID
        /// </summary>
        /// <param name="OnlineCommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetOnlineCommandByID(int OnlineCommandID)
        {
            return OpenTrade.DBWOnlineCommandInstance.GetOnlineCommandByID(OnlineCommandID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="symbolName"></param>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetOnlineCommandByCommandID(int commandID)
        {
            return OpenTrade.DBWOnlineCommandInstance.GetOnlineCommandByCommandID(commandID);
        }

        /// <summary>
        /// ADD NEW ONLINE COMMAND ON DATABASE
        /// </summary>
        /// <param name="objOpenTrade"></param>
        /// <returns></returns>
        internal int AddNewOnlineCommand(Business.OpenTrade objOpenTrade)
        {
            return OpenTrade.DBWOnlineCommandInstance.AddNewOnlineCommand(objOpenTrade);
        }

        /// <summary>
        /// UPDATE ONLINE COMMAND IN DATABASE
        /// </summary>
        /// <param name="objOpenTrade"></param>
        /// <returns></returns>
        internal bool UpdateOnlineCommand(Business.OpenTrade objOpenTrade)
        {
            return OpenTrade.DBWOnlineCommandInstance.UpdateOnlineCommand(objOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isStopLoss"></param>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal bool UpdateIsActivePending(bool isActive, bool isStopLoss, int commandID)
        {
            TradingServer.Facade.FacadeAddNewSystemLog(1, "Isactive: " + isActive + " isStopLoss: " + isStopLoss, "[ActivePending]", "", "");
            return OpenTrade.dbwOnlineCommand.UpdateIsActivePending(isActive, isStopLoss, commandID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openPositionID"></param>
        /// <param name="totalSwap"></param>
        /// <returns></returns>
        internal bool UpdateTotalSwapOpenTrade(int openPositionID, double totalSwap)
        {
            return OpenTrade.DBWOnlineCommandInstance.UpdateTotalSwap(openPositionID, totalSwap);
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY ID IN DATABASE
        /// </summary>
        /// <param name="OnlineCommandID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommandByID(int OnlineCommandID)
        {
            return OpenTrade.DBWOnlineCommandInstance.DeleteOnlineCommand(OnlineCommandID);
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY COMMAND TYPE ID IN DATABASE
        /// </summary>
        /// <param name="CommandTypeID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommandByCommandTypeID(int CommandTypeID)
        {
            return OpenTrade.DBWOnlineCommandInstance.DeleteOnlineCommandByCommandTypeID(CommandTypeID);
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY SYMBOL ID IN DATABASE
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommandBySymbolID(int SymbolID)
        {
            return OpenTrade.DBWOnlineCommandInstance.DeleteOnlineCommandBySymbolID(SymbolID);
        }

        /// <summary>
        /// GET ALL COMMAND HISTORY IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetAllCommandHistory()
        {
            return OpenTrade.DBWCommandHistoryInstance.GetAllCommandHistory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberGet"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetTopClosePosition(int numberGet)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetTopClosePosition(numberGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <returns></returns>
        internal Dictionary<int, double> GetDepositByListInvestor(List<int> listInvestorID)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetDepositInvestor(listInvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberGet"></param>
        /// <param name="listInvestorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetTopClosePosition(int numberGet, List<int> listInvestorID)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetTopClosePosition(numberGet, listInvestorID);
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY INVESTOR ID IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineComamndByInvestorID(int InvestorID)
        {
            return OpenTrade.DBWOnlineCommandInstance.DeleteOnlineCommandByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetCommandHistoryByCommandID(int commandID)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetCommandHistoryByCommandID(commandID);
        }

        /// <summary>
        /// GET COMMAND HISTORY BY INVESTOR ID IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryByInvestor(int InvestorID)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetCommandHistoryByInvestor(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryByInvestorInMonth(int investorID)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetCommandHistoryByInvestorInMonth(investorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetAllHistoryWithStartEnd(int rowNumber, int from)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetAllHistoryStartEnd(rowNumber, from);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="rowNumber"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetHistoryByInvestor(int investorID, int rowNumber, int from)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetHistoryByInvestorID(investorID, rowNumber, from);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandCode"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetHistoryByCommandCode(string commandCode)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetHistoryByCommandCode(commandCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryWithTime(int investorID, DateTime startTime, DateTime endTime)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetCommandHistoryWithTime(investorID, startTime, endTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <param name="InvestorID"></param>
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryWithStartLimit(int InvestorID, int ManagerID, DateTime TimeStart, DateTime TimeEnd)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetCommandHistoryWithStartLimit(InvestorID, ManagerID, TimeStart, TimeEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="dateTime"></param>
        /// <param name="commandTypeID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetLastBalanceWithDateTime(int investorID, DateTime dateTime, int commandTypeID, DateTime timeEnd)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetLastBalanceOfInvestor(commandTypeID, dateTime, investorID, timeEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorList"></param>
        /// <param name="ManagerID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryWithInvestorList(List<int> InvestorList, int ManagerID, DateTime start, DateTime end)
        {
            return OpenTrade.DBWCommandHistoryInstance.GetCommandHistoryWithListInvestorID(InvestorList, start, end, ManagerID);
        }

        /// <summary>
        /// ADD NEW COMMAND HISTORY IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="CommandTypeID"></param>
        /// <param name="CommandCode"></param>
        /// <param name="OpenTime"></param>
        /// <param name="OpenPrice"></param>
        /// <param name="CloseTime"></param>
        /// <param name="ClosePrice"></param>
        /// <param name="Profit"></param>
        /// <param name="Swap"></param>
        /// <param name="Commission"></param>
        /// <param name="ExpTime"></param>
        /// <param name="Size"></param>
        /// <param name="StopLoss"></param>
        /// <param name="TakeProfit"></param>
        /// <param name="clientCode"></param>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal int AddNewCommandHistory(int InvestorID, int CommandTypeID, string CommandCode, DateTime OpenTime, double OpenPrice, DateTime CloseTime,
                                            double ClosePrice, double Profit, double Swap, double Commission, DateTime ExpTime, double Size, double StopLoss,
                                            double TakeProfit, string clientCode, int SymbolID, double Taxes, double AgentCommission, string Comment, string checkCode, 
                                            double totalSwap, int RefCommandID, string agentRefConfig, bool isActive, bool isStopLostTakeProfit)
        {
            return OpenTrade.DBWCommandHistoryInstance.AddNewCommandHistory(InvestorID, CommandTypeID, CommandCode, OpenTime, OpenPrice, CloseTime, ClosePrice, Profit,
                                                                        Swap, Commission, ExpTime, Size, StopLoss, TakeProfit, clientCode, SymbolID, Taxes, AgentCommission,
                                                                        Comment, totalSwap, RefCommandID, agentRefConfig, isActive, isStopLostTakeProfit);
        }

        /// <summary>
        /// add new command history using add last balance
        /// </summary>
        /// <param name="listInvestor"></param>
        internal void AddNewCommandHistory(List<Business.Investor> listInvestor, DateTime time)
        {
            OpenTrade.DBWCommandHistoryInstance.AddCommandHistoryWithListCommand(listInvestor, time);
        }

        /// <summary>
        /// UPDATE COMMAND WITH TAKE PROFIT , STOP LOSS IN DATABASE
        /// </summary>
        /// <param name="TakeProfit"></param>
        /// <param name="StopLoss"></param>
        /// <param name="OnlineCommandID"></param>
        /// <returns></returns>
        internal bool UpdateCommandWithTakeProfit(double TakeProfit, double StopLoss, int OnlineCommandID, string comment, double openPrices)
        {
            return OpenTrade.DBWOnlineCommandInstance.UpdateTakeProfit(TakeProfit, StopLoss, OnlineCommandID, comment, openPrices);
        }

        /// <summary>
        /// UPDATE SWAP ONLINE COMMAND IN DATABASE
        /// </summary>
        /// <param name="OnlineCommandID"></param>
        /// <param name="Swap"></param>
        /// <returns></returns>
        internal bool UpdateSwapOnlineCommand(int OnlineCommandID, double Swap)
        {
            return OpenTrade.DBWOnlineCommandInstance.UpdateSwapOnlineCommand(OnlineCommandID, Swap);
        }

        /// <summary>
        /// UPDATE COMMAND CODE IN DATABASE
        /// </summary>
        /// <param name="OpenTradeID"></param>
        /// <param name="CommandCode"></param>
        /// <returns></returns>
        internal bool UpdateCommandCode(int OpenTradeID, string CommandCode)
        {
            return OpenTrade.DBWOnlineCommandInstance.UpdateCommandCode(OpenTradeID, CommandCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandHistoryID"></param>
        /// <returns></returns>
        internal bool DeleteHistoryCommand(int CommandHistoryID)
        {
            return OpenTrade.DBWCommandHistoryInstance.DeleteCommandHistory(CommandHistoryID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal bool DeleteCommandHistoryByInvestorID(int investorID)
        {
            return OpenTrade.DBWCommandHistoryInstance.DeleteCommandHistoryByInvestorID(investorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objOpenTrade"></param>
        /// <returns></returns>
        internal bool UpdateCommandHistory(Business.OpenTrade objOpenTrade)
        {
            return OpenTrade.DBWCommandHistoryInstance.UpdateCommandHistory(objOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandCode"></param>
        /// <param name="CommandHistoryID"></param>
        /// <returns></returns>
        internal bool UpdateCommandCodeHistory(string CommandCode, int CommandHistoryID)
        {
            return OpenTrade.DBWCommandHistoryInstance.UpdateCommandCodeHistory(CommandCode, CommandHistoryID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandHistoryID"></param>
        /// <param name="totalSwap"></param>
        /// <returns></returns>
        internal bool UpdateTotalSwapCommandHistory(int commandHistoryID, double totalSwap)
        {
            return OpenTrade.DBWCommandHistoryInstance.UpdateTotalSwapHistory(commandHistoryID, totalSwap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDelete"></param>
        /// <param name="commandHistoryID"></param>
        /// <returns></returns>
        internal bool UpdateIsDeleteHistory(bool isDelete, int commandHistoryID)
        {
            return OpenTrade.DBWCommandHistoryInstance.UpdateIsDeleteHistory(isDelete, commandHistoryID);
        }

        /// <summary>
        /// UPDATE ONLINE COMMAND IN SYMBOL LIST AND INVESTOR LIST OF CLASS MAKRET
        /// AND UPDATE OPEN TRADE ON DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="CommandID"></param>
        /// <param name="Commission"></param>
        /// <param name="ExpTime"></param>
        /// <param name="OpenPrice"></param>
        /// <param name="OpenTime"></param>
        /// <param name="StopLoss"></param>
        /// <param name="Swap"></param>
        /// <param name="TakeProfit"></param>
        /// <param name="SymbolName"></param>
        internal bool UpdateOpenTrade(int InvestorID, int CommandID, double Commission, DateTime ExpTime, double OpenPrice, DateTime OpenTime,
            double StopLoss, double Swap, double TakeProfit, string SymbolName, double taxes, string comment, double agentCommission, double size)
        {
            bool Result = false;
            double profit = 0;

            #region UPDATE ONLINE COMMAND IN INVESTOR LIST
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
                                if (Business.Market.InvestorList[i].CommandList[j].ID == CommandID)
                                {   
                                    //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                                    Business.Market.InvestorList[i].CommandList[j].Commission = Commission;
                                    Business.Market.InvestorList[i].CommandList[j].ExpTime = ExpTime;
                                    Business.Market.InvestorList[i].CommandList[j].OpenPrice = OpenPrice;
                                    Business.Market.InvestorList[i].CommandList[j].OpenTime = OpenTime;
                                    Business.Market.InvestorList[i].CommandList[j].StopLoss = StopLoss;
                                    Business.Market.InvestorList[i].CommandList[j].Swap = Swap;
                                    Business.Market.InvestorList[i].CommandList[j].TakeProfit = TakeProfit;
                                    Business.Market.InvestorList[i].CommandList[j].Comment = comment;
                                    Business.Market.InvestorList[i].CommandList[j].Size = size;
                                    Business.Market.InvestorList[i].CommandList[j].Profit = profit;

                                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID);

                                    if (!isPending)
                                    {
                                        Business.Market.InvestorList[i].CommandList[j].CalculatorProfitCommand(Business.Market.InvestorList[i].CommandList[j]);
                                        profit = Business.Market.InvestorList[i].CommandList[j].Symbol.ConvertCurrencyToUSD(
                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Currency,
                                            Business.Market.InvestorList[i].CommandList[j].Profit, false,
                                            Business.Market.InvestorList[i].CommandList[j].SpreaDifferenceInOpenTrade,
                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Digit);

                                        Business.Market.InvestorList[i].CommandList[j].Profit = profit;
                                    }

                                    //CALL FUNCTION UPDATE ONLINE COMMAND IN DATABASE     
                                    bool ResultUpdate = false;
                                    ResultUpdate = this.UpdateOnlineCommand(Business.Market.InvestorList[i].CommandList[j]);

                                    bool IsBuy = false;
                                    if (Business.Market.InvestorList[i].CommandList[j].Type.ID == 1 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 7 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 9 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 11)
                                        IsBuy = true;

                                    #region MAP COMMAND TO CLIENT
                                    //SEND NOTIFY TO CLIENT
                                    string Message = "UpdateCommandByManager$True,UPDATE COMMAND BY MANAGER COMPLETE," +
                                                        Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Size + "," + IsBuy + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Commission + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Swap + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Profit + "," + comment + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Type.Name + "," +
                                                        1 + "," + Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Margin + ",Update";

                                    if (Business.Market.InvestorList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].CommandList[j].Investor.ClientCommandQueue.Add(Message);
                                    #endregion

                                    //SEND NOTIFY TO AGENT SERVER
                                    Message += "," + Business.Market.InvestorList[i].CommandList[j].AgentRefConfig + "," +
                                        Business.Market.InvestorList[i].CommandList[j].SpreaDifferenceInOpenTrade;

                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = Message;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify,
                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorGroupInstance);

                                    //SEND COMMAND TO MANAGER
                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Business.Market.InvestorList[i].CommandList[j]);

                                    Result = true;
                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }
            #endregion

            #region UPDATE ONLINE COMMAND IN COMMAND EXECUTOR
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == CommandID)
                    {
                        #region UPDATE COMMAND OF INVESTOR IN COMMAND EXECUTOR
                        //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                        Business.Market.CommandExecutor[i].Commission = Commission;
                        Business.Market.CommandExecutor[i].ExpTime = ExpTime;
                        Business.Market.CommandExecutor[i].OpenPrice = OpenPrice;
                        Business.Market.CommandExecutor[i].OpenTime = OpenTime;
                        Business.Market.CommandExecutor[i].StopLoss = StopLoss;
                        Business.Market.CommandExecutor[i].Swap = Swap;
                        Business.Market.CommandExecutor[i].TakeProfit = TakeProfit;
                        Business.Market.CommandExecutor[i].Comment = comment;
                        Business.Market.CommandExecutor[i].Size = size;

                        break;
                        #endregion
                        
                    }
                }
            }
            #endregion

            #region UPDATE ONLINE COMMAND IN SYMBOL LIST
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == SymbolName)
                    {
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == CommandID)
                                {
                                    //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                                    Business.Market.SymbolList[i].CommandList[j].Commission = Commission;
                                    Business.Market.SymbolList[i].CommandList[j].ExpTime = ExpTime;
                                    Business.Market.SymbolList[i].CommandList[j].OpenPrice = OpenPrice;
                                    Business.Market.SymbolList[i].CommandList[j].OpenTime = OpenTime;
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss = StopLoss;
                                    Business.Market.SymbolList[i].CommandList[j].Swap = Swap;
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit = TakeProfit;
                                    Business.Market.SymbolList[i].CommandList[j].Comment = comment;
                                    Business.Market.SymbolList[i].CommandList[j].Size = size;
                                    Business.Market.SymbolList[i].CommandList[j].Profit = profit;

                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// UPDATE ONLINE COMMAND IN SYMBOL LIST AND INVESTOR LIST OF CLASS MAKRET
        /// AND UPDATE OPEN TRADE ON DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="CommandID"></param>
        /// <param name="Commission"></param>
        /// <param name="ExpTime"></param>
        /// <param name="OpenPrice"></param>
        /// <param name="OpenTime"></param>
        /// <param name="StopLoss"></param>
        /// <param name="Swap"></param>
        /// <param name="TakeProfit"></param>
        /// <param name="SymbolName"></param>
        internal bool UpdateOpenTrade(int InvestorID, int refCommandID, double Commission, DateTime ExpTime, double OpenPrice, DateTime OpenTime,
            double StopLoss, double Swap, double TakeProfit, string SymbolName, string comment, double size)
        {
            bool Result = false;
            double profit = 0;

            #region UPDATE ONLINE COMMAND IN COMMAND EXECUTOR
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].RefCommandID == refCommandID)
                    {
                        //InvestorID = 0 ==> set new investor id
                        if (Business.Market.CommandExecutor[i].Investor != null)
                            InvestorID = Business.Market.CommandExecutor[i].Investor.InvestorID;

                        #region UPDATE COMMAND OF INVESTOR IN COMMAND EXECUTOR
                        //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                        Business.Market.CommandExecutor[i].Commission = Commission;
                        Business.Market.CommandExecutor[i].ExpTime = ExpTime;
                        Business.Market.CommandExecutor[i].OpenPrice = OpenPrice;
                        Business.Market.CommandExecutor[i].OpenTime = OpenTime;
                        Business.Market.CommandExecutor[i].StopLoss = StopLoss;
                        Business.Market.CommandExecutor[i].Swap = Swap;
                        Business.Market.CommandExecutor[i].TakeProfit = TakeProfit;
                        Business.Market.CommandExecutor[i].Comment = comment;
                        Business.Market.CommandExecutor[i].Size = size;

                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.CommandExecutor[i].Type.ID);

                        if (!isPending)
                        {
                            Business.Market.CommandExecutor[i].CalculatorProfitCommand(Business.Market.CommandExecutor[i]);
                            profit = Business.Market.CommandExecutor[i].Symbol.ConvertCurrencyToUSD(Business.Market.CommandExecutor[i].Symbol.Currency,
                                Business.Market.CommandExecutor[i].Profit, false, Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade,
                                Business.Market.CommandExecutor[i].Symbol.Digit);
                            Business.Market.CommandExecutor[i].Profit = profit;
                        }

                        //CALL FUNCTION UPDATE ONLINE COMMAND IN DATABASE     
                        bool ResultUpdate = false;
                        ResultUpdate = this.UpdateOnlineCommand(Business.Market.CommandExecutor[i]);

                        bool IsBuy = false;
                        if (Business.Market.CommandExecutor[i].Type.ID == 1 ||
                            Business.Market.CommandExecutor[i].Type.ID == 7 ||
                            Business.Market.CommandExecutor[i].Type.ID == 9 ||
                            Business.Market.CommandExecutor[i].Type.ID == 11)
                            IsBuy = true;

                        #region MAP COMMAND TO CLIENT
                        //SEND NOTIFY TO CLIENT
                        string Message = "UpdateCommandByManager$True,UPDATE COMMAND BY MANAGER COMPLETE," + Business.Market.CommandExecutor[i].ID + "," +
                                            Business.Market.CommandExecutor[i].Investor.InvestorID + "," +
                                            Business.Market.CommandExecutor[i].Symbol.Name + "," +
                                            Business.Market.CommandExecutor[i].Size + "," + IsBuy + "," +
                                            Business.Market.CommandExecutor[i].OpenTime + "," +
                                            Business.Market.CommandExecutor[i].OpenPrice + "," +
                                            Business.Market.CommandExecutor[i].StopLoss + "," +
                                            Business.Market.CommandExecutor[i].TakeProfit + "," +
                                            Business.Market.CommandExecutor[i].ClosePrice + "," +
                                            Business.Market.CommandExecutor[i].Commission + "," +
                                            Business.Market.CommandExecutor[i].Swap + "," +
                                            Business.Market.CommandExecutor[i].Profit + "," + comment + "," +
                                            Business.Market.CommandExecutor[i].ID + "," +
                                            Business.Market.CommandExecutor[i].Type.Name + "," +
                                            1 + "," + Business.Market.CommandExecutor[i].ExpTime + "," +
                                            Business.Market.CommandExecutor[i].ClientCode + "," +
                                            Business.Market.CommandExecutor[i].CommandCode + "," +
                                            Business.Market.CommandExecutor[i].IsHedged + "," +
                                            Business.Market.CommandExecutor[i].Type.ID + "," +
                                            Business.Market.CommandExecutor[i].Margin + ",Update";

                        if (Business.Market.CommandExecutor[i].Investor.ClientCommandQueue == null)
                            Business.Market.CommandExecutor[i].Investor.ClientCommandQueue = new List<string>();

                        Business.Market.CommandExecutor[i].Investor.ClientCommandQueue.Add(Message);
                        #endregion

                        //SEND COMMAND TO MANAGER
                        TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Business.Market.CommandExecutor[i]);

                        Result = true;
                        break;
                        #endregion
                    }
                }
            }
            #endregion

            #region UPDATE ONLINE COMMAND IN INVESTOR LIST
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
                                if (Business.Market.InvestorList[i].CommandList[j].RefCommandID == refCommandID)
                                {
                                    //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                                    Business.Market.InvestorList[i].CommandList[j].Commission = Commission;
                                    Business.Market.InvestorList[i].CommandList[j].ExpTime = ExpTime;
                                    Business.Market.InvestorList[i].CommandList[j].OpenPrice = OpenPrice;
                                    Business.Market.InvestorList[i].CommandList[j].OpenTime = OpenTime;
                                    Business.Market.InvestorList[i].CommandList[j].StopLoss = StopLoss;
                                    Business.Market.InvestorList[i].CommandList[j].Swap = Swap;
                                    Business.Market.InvestorList[i].CommandList[j].TakeProfit = TakeProfit;
                                    Business.Market.InvestorList[i].CommandList[j].Comment = comment;
                                    Business.Market.InvestorList[i].CommandList[j].Size = size;
                                    Business.Market.InvestorList[i].CommandList[j].Profit = profit;

                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }
            #endregion

            #region UPDATE ONLINE COMMAND IN SYMBOL LIST
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == SymbolName)
                    {
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].RefCommandID == refCommandID)
                                {
                                    //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                                    Business.Market.SymbolList[i].CommandList[j].Commission = Commission;
                                    Business.Market.SymbolList[i].CommandList[j].ExpTime = ExpTime;
                                    Business.Market.SymbolList[i].CommandList[j].OpenPrice = OpenPrice;
                                    Business.Market.SymbolList[i].CommandList[j].OpenTime = OpenTime;
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss = StopLoss;
                                    Business.Market.SymbolList[i].CommandList[j].Swap = Swap;
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit = TakeProfit;
                                    Business.Market.SymbolList[i].CommandList[j].Comment = comment;
                                    Business.Market.SymbolList[i].CommandList[j].Size = size;
                                    //Business.Market.SymbolList[i].CommandList[j].Profit = profit;

                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY COMMAND ID IN CLASS MARKET AND DATABASE. IT CALL BY MANAGER
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal bool DeleteOpenTradeByCommandID(int CommandID)
        {   
            bool Result = false;

            #region FIND COMMAND IN COMMAND EXECUTOR
            //if (Business.Market.CommandExecutor != null)
            //{
            //    int count = Business.Market.CommandExecutor.Count;
            //    for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
            //    {
            //        if (Business.Market.CommandExecutor[i].ID == CommandID)
            //        {
            //            Business.Market.CommandExecutor.RemoveAt(i);
            //            break;
            //        }
            //    }
            //}
            #endregion

            #region Find Command In Investor List Of Class Market And Delete Command
            if (Business.Market.InvestorList != null)
            {
                bool Flag = false;
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Flag == true)
                        break;

                    if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                    {
                        int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                        for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.InvestorList[i].CommandList[j].ID == CommandID)
                            {
                                bool isBuy = TradingServer.Facade.FacadeGetIsBuyByTypeID(Business.Market.InvestorList[i].CommandList[j].Type.ID);
                                int ResultHistory = -1;
                                Business.Market.InvestorList[i].CommandList[j].CloseTime = DateTime.Now;
                                Business.Market.InvestorList[i].CommandList[j].Profit = 0;

                                #region ADD COMMAND TO HISTORY
                                //CALL FUNCTION INSERT COMMAND TO HISTORY COMMAND WITH PROFIT = 0
                                ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID,
                                    Business.Market.InvestorList[i].CommandList[j].Type.ID, Business.Market.InvestorList[i].CommandList[j].CommandCode,
                                    Business.Market.InvestorList[i].CommandList[j].OpenTime, Business.Market.InvestorList[i].CommandList[j].OpenPrice,
                                    Business.Market.InvestorList[i].CommandList[j].CloseTime, Business.Market.InvestorList[i].CommandList[j].ClosePrice,
                                    0/*Profit*/, 0/*SWAP*/,
                                    0/*COMMISSION*/, Business.Market.InvestorList[i].CommandList[j].ExpTime,
                                    Business.Market.InvestorList[i].CommandList[j].Size, Business.Market.InvestorList[i].CommandList[j].StopLoss,
                                    Business.Market.InvestorList[i].CommandList[j].TakeProfit, Business.Market.InvestorList[i].CommandList[j].ClientCode,
                                    Business.Market.InvestorList[i].CommandList[j].Symbol.SymbolID, Business.Market.InvestorList[i].CommandList[j].Taxes, 0,
                                    Business.Market.InvestorList[i].CommandList[j].Comment, "6", 0/*TOTAL SWAP*/,
                                    Business.Market.InvestorList[i].CommandList[j].RefCommandID,
                                    Business.Market.InvestorList[i].CommandList[j].AgentRefConfig,
                                    Business.Market.InvestorList[i].CommandList[j].IsActivePending,
                                    Business.Market.InvestorList[i].CommandList[j].IsStopLossAndTakeProfit);
                                #endregion

                                //CALL FUNCTION DELETE COMMAND IN ONLINE COMMAND
                                TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.InvestorList[i].CommandList[j].ID);

                                if (ResultHistory > 0)
                                {
                                    #region MAP COMMAND SEND TO CLIENT
                                    string Message = "CloseCommandByManager$True,CLOSE COMMAND COMMPLETE," + Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Size + "," + isBuy + "," +
                                         Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                         Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                         Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                         Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                         "0," +
                                         "0," +
                                         "0," + "Comment," +
                                         Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Type.Name + "," + 1 + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                         Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                         Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Margin + ",Close," +
                                         Business.Market.InvestorList[i].CommandList[j].CloseTime;
                                    #endregion

                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);

                                    string notifyAgent = "DeleteOrderByManager$True,CLOSE COMMAND COMMPLETE," + Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Size + "," + isBuy + "," +
                                         Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                         Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                         Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                         Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                         "0," +
                                         "0," +
                                         "0," + "Comment," +
                                         Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Type.Name + "," + 1 + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                         Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                         Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Margin + ",Close," +
                                         Business.Market.InvestorList[i].CommandList[j].CloseTime;

                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = notifyAgent;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Business.Market.InvestorList[i].InvestorGroupInstance);

                                    #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                                    ////CALL FUNCTION REMOVE COMMAND IN SYMBOL LIST OF CLASS MAKRET
                                    //bool resultRemoveCommnadSymbol = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(CommandID);
                                    #endregion                                    

                                    //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                    //Business.OpenTrade newOpenTrade = Business.Market.InvestorList[i].CommandList[j];
                                    Business.OpenRemove newOpenRemove = new OpenRemove();
                                    newOpenRemove.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                    newOpenRemove.OpenTradeID = Business.Market.InvestorList[i].CommandList[j].ID;
                                    newOpenRemove.SymbolName = Business.Market.InvestorList[i].CommandList[j].Symbol.Name;
                                    newOpenRemove.IsExecutor = true;
                                    newOpenRemove.IsSymbol = true;
                                    newOpenRemove.IsInvestor = false;
                                    Business.Market.AddCommandToRemoveList(newOpenRemove);

                                    //SEND NOTIFY TO MANAGER
                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.InvestorList[i].CommandList[j]);

                                    //SEND NOTIFY CHANGE ACCOUNT
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);

                                    #region BLOCK OBJECT COMMAND IN INVESTOR LIST
                                    lock (Business.Market.syncObject)
                                    {
                                        //REMOVE COMMAND IN INVESTOR COMMAND LIST OF CLASS MARKET
                                        Business.Market.InvestorList[i].CommandList.RemoveAt(j);
                                    }  
                                    #endregion                                                                      

                                    if (Business.Market.InvestorList[i].CommandList.Count > 0)
                                    {
                                        //RECALCULATION TOTAL MARGIN OF ACCOUNT
                                        //Business.Market.InvestorList[i].ReCalculationTotalMargin();
                                        Business.Margin newMargin = new Margin();
                                        newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                        Business.Market.InvestorList[i].Margin = newMargin.TotalMargin;
                                        Business.Market.InvestorList[i].FreezeMargin = newMargin.TotalFreezeMargin;
                                    }
                                    else
                                    {
                                        Business.Market.InvestorList[i].Margin = 0;
                                        Business.Market.InvestorList[i].FreezeMargin = 0;
                                        Business.Market.InvestorList[i].Profit = 0;
                                        Business.Market.InvestorList[i].FreeMargin = 0;
                                    }

                                    Result = true;
                                }
                                Flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region FIND COMMAND IN SYMBOL LIST AND DELETE
            //if (Business.Market.SymbolList != null)
            //{
            //    bool flag = false;
            //    int count = Business.Market.SymbolList.Count;
            //    for (int i = 0; i < Business.Market.SymbolList.Count; i++)
            //    {
            //        if (flag)
            //            break;

            //        if (Business.Market.SymbolList[i].CommandList != null)
            //        {
            //            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
            //            for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
            //            {
            //                if (Business.Market.SymbolList[i].CommandList[j].ID == CommandID)
            //                {
            //                    Business.Market.SymbolList[i].CommandList.RemoveAt(j);
            //                    flag = true;

            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            return Result;
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY COMMAND ID IN CLASS MARKET AND DATABASE. IT CALL BY MANAGER
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal bool DeleteOpenTradeByRefCommandID(int refCommandID)
        {
            bool Result = false;

            #region Find Command In Investor List Of Class Market And Delete Command
            if (Business.Market.InvestorList != null)
            {
                bool Flag = false;
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Flag == true)
                        break;

                    if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                    {   
                        for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.InvestorList[i].CommandList[j].RefCommandID == refCommandID)
                            {
                                Business.OpenTrade temp = Business.Market.InvestorList[i].CommandList[j];

                                bool isBuy = TradingServer.Facade.FacadeGetIsBuyByTypeID(temp.Type.ID);
                                int ResultHistory = -1;
                                temp.CloseTime = DateTime.Now;
                                temp.Profit = 0;

                                #region ADD COMMAND TO HISTORY
                                //CALL FUNCTION INSERT COMMAND TO HISTORY COMMAND WITH PROFIT = 0
                                ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(temp.Investor.InvestorID,
                                    temp.Type.ID, temp.CommandCode,
                                    temp.OpenTime, temp.OpenPrice,
                                    temp.CloseTime, temp.ClosePrice,
                                    0/*Profit*/, 0/*SWAP*/,
                                    0/*COMMISSION*/, temp.ExpTime,
                                    temp.Size, temp.StopLoss,
                                    temp.TakeProfit, temp.ClientCode,
                                    temp.Symbol.SymbolID, temp.Taxes, 0,
                                    temp.Comment, "6", 0/*TOTAL SWAP*/,
                                    temp.RefCommandID,
                                    temp.AgentRefConfig,
                                    temp.IsActivePending,
                                    temp.IsStopLossAndTakeProfit);
                                #endregion

                                //CALL FUNCTION DELETE COMMAND IN ONLINE COMMAND
                                TradingServer.Facade.FacadeDeleteOpenTradeByID(temp.ID);

                                if (ResultHistory > 0)
                                {
                                    #region MAP COMMAND SEND TO CLIENT
                                    StringBuilder Message = new StringBuilder();
                                    Message.Append("CloseCommandByManager$True,CLOSE COMMAND COMMPLETE,");
                                    Message.Append(temp.ID);
                                    Message.Append(",");
                                    Message.Append(temp.Investor.InvestorID);
                                    Message.Append(",");
                                    Message.Append(temp.Symbol.Name);
                                    Message.Append(",");
                                    Message.Append(temp.Size);
                                    Message.Append(",");
                                    Message.Append(isBuy);
                                    Message.Append(",");
                                    Message.Append(temp.OpenTime);
                                    Message.Append(",");
                                    Message.Append(temp.OpenPrice);
                                    Message.Append(",");
                                    Message.Append(temp.StopLoss);
                                    Message.Append(",");
                                    Message.Append(temp.TakeProfit);
                                    Message.Append(",");
                                    Message.Append(temp.ClosePrice);
                                    Message.Append(",");
                                    Message.Append("0,0,0,Comment,");
                                    Message.Append(temp.ID);
                                    Message.Append(",");
                                    Message.Append(temp.Type.Name);
                                    Message.Append(",");
                                    Message.Append(1);
                                    Message.Append(",");
                                    Message.Append(temp.ExpTime);
                                    Message.Append(",");
                                    Message.Append(temp.ClientCode);
                                    Message.Append(",");
                                    Message.Append(temp.CommandCode);
                                    Message.Append(",");
                                    Message.Append(temp.IsHedged);
                                    Message.Append(",");
                                    Message.Append(temp.Type.ID);
                                    Message.Append(",");
                                    Message.Append(temp.Margin);
                                    Message.Append(",Close,");
                                    Message.Append(temp.CloseTime);
                                    #endregion

                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(Message.ToString());

                                    #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                                    ////CALL FUNCTION REMOVE COMMAND IN SYMBOL LIST OF CLASS MAKRET
                                    //bool resultRemoveCommnadSymbol = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(CommandID);
                                    #endregion

                                    //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                    //Business.OpenTrade newOpenTrade = Business.Market.InvestorList[i].CommandList[j];
                                    Business.OpenRemove newOpenRemove = new OpenRemove();
                                    newOpenRemove.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                    newOpenRemove.OpenTradeID = Business.Market.InvestorList[i].CommandList[j].ID;
                                    newOpenRemove.SymbolName = temp.Symbol.Name;
                                    newOpenRemove.IsExecutor = true;
                                    newOpenRemove.IsSymbol = true;
                                    newOpenRemove.IsInvestor = false;
                                    Business.Market.AddCommandToRemoveList(newOpenRemove);

                                    //SEND NOTIFY TO MANAGER
                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.InvestorList[i].CommandList[j]);

                                    //SEND NOTIFY CHANGE ACCOUNT
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);

                                    #region BLOCK OBJECT COMMAND IN INVESTOR LIST
                                    lock (Business.Market.syncObject)
                                    {
                                        //REMOVE COMMAND IN INVESTOR COMMAND LIST OF CLASS MARKET
                                        Business.Market.InvestorList[i].CommandList.RemoveAt(j);
                                    }
                                    #endregion

                                    if (Business.Market.InvestorList[i].CommandList.Count > 0)
                                    {
                                        //RECALCULATION TOTAL MARGIN OF ACCOUNT
                                        //Business.Market.InvestorList[i].ReCalculationTotalMargin();
                                        Business.Margin newMargin = new Margin();
                                        newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                        Business.Market.InvestorList[i].Margin = newMargin.TotalMargin;
                                        Business.Market.InvestorList[i].FreezeMargin = newMargin.TotalFreezeMargin;
                                    }
                                    else
                                    {
                                        Business.Market.InvestorList[i].Margin = 0;
                                        Business.Market.InvestorList[i].FreezeMargin = 0;
                                        Business.Market.InvestorList[i].Profit = 0;
                                        Business.Market.InvestorList[i].FreeMargin = 0;
                                    }

                                    Result = true;
                                }

                                Flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// DELETE ONLINE COMMAND BY COMMAND ID IN CLASS MARKET AND DATABASE. IT CALL BY MANAGER
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal bool DeleteOpenTradeByMT4(int refCommandID)
        {
            bool Result = false;

            #region Find Command In Investor List Of Class Market And Delete Command
            if (Business.Market.InvestorList != null)
            {
                bool Flag = false;
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Flag == true)
                        break;

                    if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                    {
                        for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.InvestorList[i].CommandList[j].RefCommandID == refCommandID)
                            {
                                Business.OpenTrade temp = Business.Market.InvestorList[i].CommandList[j];

                                bool isBuy = TradingServer.Facade.FacadeGetIsBuyByTypeID(temp.Type.ID);
                                
                                temp.CloseTime = DateTime.Now;
                                temp.Profit = 0;

                                #region MAP COMMAND SEND TO CLIENT
                                StringBuilder Message = new StringBuilder();
                                Message.Append("CloseCommandByManager$True,CLOSE COMMAND COMMPLETE,");
                                Message.Append(temp.ID);
                                Message.Append(",");
                                Message.Append(temp.Investor.InvestorID);
                                Message.Append(",");
                                Message.Append(temp.Symbol.Name);
                                Message.Append(",");
                                Message.Append(temp.Size);
                                Message.Append(",");
                                Message.Append(isBuy);
                                Message.Append(",");
                                Message.Append(temp.OpenTime);
                                Message.Append(",");
                                Message.Append(temp.OpenPrice);
                                Message.Append(",");
                                Message.Append(temp.StopLoss);
                                Message.Append(",");
                                Message.Append(temp.TakeProfit);
                                Message.Append(",");
                                Message.Append(temp.ClosePrice);
                                Message.Append(",");
                                Message.Append("0,0,0,Comment,");
                                Message.Append(temp.ID);
                                Message.Append(",");
                                Message.Append(temp.Type.Name);
                                Message.Append(",");
                                Message.Append(1);
                                Message.Append(",");
                                Message.Append(temp.ExpTime);
                                Message.Append(",");
                                Message.Append(temp.ClientCode);
                                Message.Append(",");
                                Message.Append(temp.CommandCode);
                                Message.Append(",");
                                Message.Append(temp.IsHedged);
                                Message.Append(",");
                                Message.Append(temp.Type.ID);
                                Message.Append(",");
                                Message.Append(temp.Margin);
                                Message.Append(",Close,");
                                Message.Append(temp.CloseTime);
                                #endregion

                                if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                    Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                Business.Market.InvestorList[i].ClientCommandQueue.Add(Message.ToString());

                                //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                //Business.OpenTrade newOpenTrade = Business.Market.InvestorList[i].CommandList[j];
                                Business.OpenRemove newOpenRemove = new OpenRemove();
                                newOpenRemove.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                newOpenRemove.OpenTradeID = Business.Market.InvestorList[i].CommandList[j].ID;
                                newOpenRemove.SymbolName = temp.Symbol.Name;
                                newOpenRemove.IsExecutor = true;
                                newOpenRemove.IsSymbol = true;
                                newOpenRemove.IsInvestor = false;
                                Business.Market.AddCommandToRemoveList(newOpenRemove);

                                #region BLOCK OBJECT COMMAND IN INVESTOR LIST
                                lock (Business.Market.syncObject)
                                {
                                    //REMOVE COMMAND IN INVESTOR COMMAND LIST OF CLASS MARKET
                                    Business.Market.InvestorList[i].CommandList.RemoveAt(j);
                                }
                                #endregion

                                Result = true;
                                Flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// DELETE COMMAND IN ADMIN, DELETE ADMIN ORDER
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal bool DeleteOpenTradeByAdmin(int commandID)
        {
            bool Result = false;

            #region Find Command In Investor List Of Class Market And Delete Command
            if (Business.Market.InvestorList != null)
            {
                bool Flag = false;
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Flag == true)
                        break;

                    if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                    {
                        int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                        for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.InvestorList[i].CommandList[j].ID == commandID)
                            {
                                bool isBuy = TradingServer.Facade.FacadeGetIsBuyByTypeID(Business.Market.InvestorList[i].CommandList[j].Type.ID);
                                int ResultHistory = -1;
                                Business.Market.InvestorList[i].CommandList[j].CloseTime = DateTime.Now;
                                Business.Market.InvestorList[i].CommandList[j].Profit = 0;

                                #region ADD COMMAND TO HISTORY
                                //CALL FUNCTION INSERT COMMAND TO HISTORY COMMAND WITH PROFIT = 0
                                ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID,
                                    Business.Market.InvestorList[i].CommandList[j].Type.ID, Business.Market.InvestorList[i].CommandList[j].CommandCode,
                                    Business.Market.InvestorList[i].CommandList[j].OpenTime, Business.Market.InvestorList[i].CommandList[j].OpenPrice,
                                    Business.Market.InvestorList[i].CommandList[j].CloseTime, Business.Market.InvestorList[i].CommandList[j].ClosePrice,
                                    0/*Profit*/, 0/*SWAP*/,
                                    0/*COMMISSION*/, Business.Market.InvestorList[i].CommandList[j].ExpTime,
                                    Business.Market.InvestorList[i].CommandList[j].Size, Business.Market.InvestorList[i].CommandList[j].StopLoss,
                                    Business.Market.InvestorList[i].CommandList[j].TakeProfit, Business.Market.InvestorList[i].CommandList[j].ClientCode,
                                    Business.Market.InvestorList[i].CommandList[j].Symbol.SymbolID, Business.Market.InvestorList[i].CommandList[j].Taxes, 0,
                                    Business.Market.InvestorList[i].CommandList[j].Comment, "6", 0/*TOTAL SWAP*/,
                                    Business.Market.InvestorList[i].CommandList[j].RefCommandID,
                                    Business.Market.InvestorList[i].CommandList[j].AgentRefConfig,
                                    Business.Market.InvestorList[i].CommandList[j].IsActivePending,
                                    Business.Market.InvestorList[i].CommandList[j].IsStopLossAndTakeProfit);
                                #endregion

                                //UPDATE STATUS ISDELETE OF COMMAND HISTORY
                                //TradingServer.Facade.FacadeUpdateIsDeleteHistory(true, ResultHistory);

                                //CALL FUNCTION DELETE COMMAND IN ONLINE COMMAND
                                TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.InvestorList[i].CommandList[j].ID);

                                if (ResultHistory > 0)
                                {
                                    #region MAP COMMAND SEND TO CLIENT
                                    string Message = "CloseCommandByManager$True,CLOSE COMMAND COMMPLETE," + Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Size + "," + isBuy + "," +
                                         Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                         Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                         Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                         Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                         "0," +
                                         "0," +
                                         "0," + "Comment," +
                                         Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Type.Name + "," + 1 + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                         Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                         Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                         Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                         Business.Market.InvestorList[i].CommandList[j].Margin + ",Close," +
                                         Business.Market.InvestorList[i].CommandList[j].CloseTime;
                                    #endregion

                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);

                                    #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                                    ////CALL FUNCTION REMOVE COMMAND IN SYMBOL LIST OF CLASS MAKRET
                                    //bool resultRemoveCommnadSymbol = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(CommandID);
                                    #endregion

                                    //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                    //Business.OpenTrade newOpenTrade = Business.Market.InvestorList[i].CommandList[j];
                                    Business.OpenRemove newOpenRemove = new OpenRemove();
                                    newOpenRemove.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                    newOpenRemove.OpenTradeID = Business.Market.InvestorList[i].CommandList[j].ID;
                                    newOpenRemove.SymbolName = Business.Market.InvestorList[i].CommandList[j].Symbol.Name;
                                    newOpenRemove.IsExecutor = true;
                                    newOpenRemove.IsSymbol = true;
                                    newOpenRemove.IsInvestor = false;
                                    Business.Market.AddCommandToRemoveList(newOpenRemove);

                                    //SEND NOTIFY TO MANAGER
                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.InvestorList[i].CommandList[j]);

                                    //SEND NOTIFY CHANGE ACCOUNT
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);

                                    #region BLOCK OBJECT COMMAND IN INVESTOR LIST
                                    lock (Business.Market.syncObject)
                                    {
                                        //REMOVE COMMAND IN INVESTOR COMMAND LIST OF CLASS MARKET
                                        Business.Market.InvestorList[i].CommandList.RemoveAt(j);
                                    }
                                    #endregion

                                    if (Business.Market.InvestorList[i].CommandList.Count > 0)
                                    {
                                        //RECALCULATION TOTAL MARGIN OF ACCOUNT
                                        //Business.Market.InvestorList[i].ReCalculationTotalMargin();
                                        Business.Margin newMargin = new Margin();
                                        newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                        Business.Market.InvestorList[i].Margin = newMargin.TotalMargin;
                                        Business.Market.InvestorList[i].FreezeMargin = newMargin.TotalFreezeMargin;
                                    }
                                    else
                                    {
                                        Business.Market.InvestorList[i].Margin = 0;
                                        Business.Market.InvestorList[i].FreezeMargin = 0;
                                        Business.Market.InvestorList[i].Profit = 0;
                                        Business.Market.InvestorList[i].FreeMargin = 0;
                                    }

                                    Result = true;
                                }
                                Flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalCommand()
        {
            return OpenTrade.DBWOnlineCommandInstance.CountOnlineCommand();
        }
    }
}
