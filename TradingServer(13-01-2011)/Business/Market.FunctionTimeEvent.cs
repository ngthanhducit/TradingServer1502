using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// EVENT WEEK CLOSE MARKET
        /// </summary>
        internal void CloseMarket(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.IsOpen)
            {
                Business.Market.IsOpen = false;

                //SEND NOTIFY TO CLIENT
                string content = "MKC53427640$" + TargetName + "{" + Business.Market.IsOpen;
                TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
            }            
        }

        /// <summary>
        /// EVENT WEEK OPEN MARKET
        /// </summary>
        internal void OpenMarket(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (!Business.Market.IsOpen)
            {
                Business.Market.IsOpen = true;

                //SEND NOTIFY TO CLIENT
                string content = "MKO53427640$" + TargetName + "{" + Business.Market.IsOpen;
                TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
            }   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetName"></param>
        internal void SetIsCloseOnlyFuture(string targetName, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name.Trim().ToUpper() == targetName.Trim().ToUpper())
                    {
                        if (!Business.Market.SymbolList[i].isCloseOnlyFuture)
                            Business.Market.SymbolList[i].isCloseOnlyFuture = true;

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetName"></param>
        internal void ProcessExpTimeFuture(string targetName, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].Symbol.MarketAreaRef.IMarketAreaName == "FutureCommand")
                    {
                        if (Business.Market.CommandExecutor[i].Symbol.Name.Trim() == targetName.Trim())
                        {
                            Business.Market.CommandExecutor[i].IsClose = true;
                            Business.Market.CommandExecutor[i].IsServer = true;
                            Business.Market.CommandExecutor[i].CloseTime = DateTime.Now;
                            Business.Market.CommandExecutor[i].ExpTime = DateTime.Now;

                            Business.Market.CommandExecutor[i].Symbol.MarketAreaRef.CloseCommand(Business.Market.CommandExecutor[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// EVENT WEEK CLOSE QUOTE SYMBOL
        /// </summary>
        internal void CloseQuoteSymbol(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        if (Business.Market.SymbolList[i].IsQuote)
                        {
                            Business.Market.SymbolList[i].IsQuote = false;

                            //SEND NOTIFY TO CLIENT
                            string content = "OQS53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsQuote;
                            TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// EVENT WEEK OPEN QUOTE SYMBOL
        /// </summary>
        internal void OpenQuoteSymbol(string NameTarget, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == NameTarget)
                    {
                        if (!Business.Market.SymbolList[i].IsQuote)
                        {
                            Business.Market.SymbolList[i].IsQuote = true;

                            //SEND NOTIFY TO CLIENT
                            string content = "OQS53427640$" + NameTarget + "{" + Business.Market.SymbolList[i].IsQuote;
                            TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                        }

                        break;
                    }
                }
            }            
        }

        /// <summary>
        /// EVENT WEEK CLOSE TRADE SYMBOL
        /// </summary>
        internal void CloseTradeSymbol(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        //=============CLOSE TRADE SESSION=========================
                        if (Business.Market.SymbolList[i].IsTrade)
                        {
                            Business.Market.SymbolList[i].IsTrade = false;

                            //TradingServer.Facade.FacadeAddNewSystemLog(6, "Close Trade Symbol " + TargetName + " is: " +
                            //                                            Business.Market.SymbolList[i].IsTrade,
                            //                                            "[Check Event Close Trade]", "", "");

                            //SEND NOTIFY TO CLIENT
                            string content = "OTS53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsTrade;
                            TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                        }

                        //==============FREEZE MARGIN============================
                        #region FIND SETTING ORDER OF SYMBOL
                        string MethodOrders = string.Empty;
                        if (Business.Market.SymbolList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.SymbolList[i].ParameterItems.Count;
                            for (int n = 0; n < countParameter; n++)
                            {
                                if (Business.Market.SymbolList[i].ParameterItems[n].Code == "S012")
                                {
                                    MethodOrders = Business.Market.SymbolList[i].ParameterItems[n].StringValue;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region PROCESS GOOD TILL CANCEL
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                            {
                                bool flagUpdateSLTP = false;
                                switch (MethodOrders)
                                {
                                    #region GOOD TILL TODAY INCLUDING SL/TP
                                    case "Good till today including SL/TP":
                                        {
                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.SymbolList[i].CommandList[j].Type.ID);
                                            if (!isPending)
                                            {
                                                #region RESET STOP LOSS AND TAKE PROFIT OF ONLINE COMMAND
                                                if (Business.Market.SymbolList[i].CommandList[j].StopLoss > 0 ||
                                                  Business.Market.SymbolList[i].CommandList[j].TakeProfit > 0)
                                                {
                                                    Business.Market.SymbolList[i].CommandList[j].StopLoss = 0;
                                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit = 0;
                                                    flagUpdateSLTP = true;
                                                }
                                                #endregion

                                                if (flagUpdateSLTP)
                                                {
                                                    #region UPDATE STOP LOST AND TAKE PROFIT OF COMMAND IN COMMAND EXECUTOR
                                                    //UPDATE STOP LOST AND TAKE PROFIT OF COMMAND IN COMMAND EXECUTOR
                                                    if (Business.Market.CommandExecutor != null && Business.Market.CommandExecutor.Count > 0)
                                                    {
                                                        for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                        {
                                                            if (Business.Market.CommandExecutor[n].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                                            {
                                                                if (Business.Market.CommandExecutor[n].StopLoss > 0 || Business.Market.CommandExecutor[n].TakeProfit > 0)
                                                                {
                                                                    Business.Market.CommandExecutor[n].StopLoss = 0;
                                                                    Business.Market.CommandExecutor[n].TakeProfit = 0;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    #region UPDATE STOP LOSS AND TAKE PROFIT OF COMMAND IN INVESTOR COMMAND
                                                    if (Business.Market.InvestorList != null)
                                                    {
                                                        for (int n = 0; n < Business.Market.InvestorList.Count; n++)
                                                        {
                                                            if (Business.Market.InvestorList[n].InvestorCommand == Business.Market.SymbolList[i].CommandList[j].Investor.InvestorCommand)
                                                            {
                                                                if (Business.Market.InvestorList[n].CommandList != null)
                                                                {
                                                                    for (int m = 0; m < Business.Market.InvestorList[n].CommandList.Count; m++)
                                                                    {
                                                                        if (Business.Market.InvestorList[n].CommandList[m].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                                                        {
                                                                            if (Business.Market.InvestorList[n].CommandList[m].StopLoss > 0 || Business.Market.InvestorList[n].CommandList[m].TakeProfit > 0)
                                                                            {
                                                                                Business.Market.InvestorList[n].CommandList[m].StopLoss = 0;
                                                                                Business.Market.InvestorList[n].CommandList[m].TakeProfit = 0;

                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    //SENT NOTIFY TO CLIENT
                                                    string message = "STO8546";
                                                    Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                                    //SEND NOTIFY TO CLIENT UPDATE STOP LOSS AND TAKE PROFIT
                                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Business.Market.SymbolList[i].CommandList[j]);

                                                    //UPDATE STOP LOST AND TAKE PROFIT OF COMMAND IN DATABASE
                                                    TradingServer.Facade.FacadeUpdateOnlineCommandWithTakeProfit(0, 0, Business.Market.SymbolList[i].CommandList[j].ID,
                                                        Business.Market.SymbolList[i].CommandList[j].Comment, Business.Market.SymbolList[i].CommandList[j].OpenPrice);
                                                }
                                            }
                                            else
                                            {
                                                #region REMOVE PENDING ORDER OF INVESTOR
                                                Business.OpenRemove newOpenRemove = new OpenRemove();
                                                newOpenRemove.InvestorID = Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID;
                                                newOpenRemove.IsExecutor = true;
                                                newOpenRemove.IsInvestor = true;
                                                newOpenRemove.IsSymbol = false;
                                                newOpenRemove.OpenTradeID = Business.Market.SymbolList[i].CommandList[j].ID;
                                                newOpenRemove.SymbolName = Business.Market.SymbolList[i].Name;
                                                Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                //DELETE COMMAND IN DATABASE
                                                bool deleteCommand = TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.SymbolList[i].CommandList[j].ID);

                                                //INSERT DATABASE THEN CANCEL PENDING ORDER
                                                TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                                    Business.Market.SymbolList[i].CommandList[j].Type.ID, Business.Market.SymbolList[i].CommandList[j].CommandCode,
                                                    Business.Market.SymbolList[i].CommandList[j].OpenTime, Business.Market.SymbolList[i].CommandList[j].OpenPrice,
                                                    DateTime.Now, 0, 0, 0, 0, Business.Market.SymbolList[i].CommandList[j].ExpTime, Business.Market.SymbolList[i].CommandList[j].Size,
                                                    Business.Market.SymbolList[i].CommandList[j].StopLoss, Business.Market.SymbolList[i].CommandList[j].TakeProfit,
                                                    Business.Market.SymbolList[i].CommandList[j].ClientCode, Business.Market.SymbolList[i].SymbolID,
                                                    Business.Market.SymbolList[i].CommandList[j].Taxes, 0, Business.Market.SymbolList[i].CommandList[j].Comment, "8",
                                                    Business.Market.SymbolList[i].CommandList[j].TotalSwap,
                                                    Business.Market.SymbolList[i].CommandList[j].RefCommandID,
                                                    Business.Market.SymbolList[i].CommandList[j].AgentRefConfig,
                                                    Business.Market.SymbolList[i].CommandList[j].IsActivePending,
                                                    Business.Market.SymbolList[i].CommandList[j].IsStopLossAndTakeProfit);

                                                //SENT NOTIFY TO CLIENT
                                                string message = "STO8546";
                                                if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                    Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                                //NOTIFY TO MANAGER DELETE PENDING ORDER COMMAND
                                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.SymbolList[i].CommandList[j]);

                                                //'1205': close sell stop order #00275799 sell stop 0.10 XAUUSD at 1401.30 sl: 0.00 tp: 0.00 (1410.40/1410.90) exp 6/7/2013 10:48:46 AM
                                                //'1205': delete order #00275799 sell stop 0.10XAUUSD at 1401.30
                                                string strLog = "'System': close " +
                                                    Business.Market.SymbolList[i].CommandList[j].Type.Name + " order #" + Business.Market.SymbolList[i].CommandList[j].CommandCode +
                                                    " " + Business.Market.SymbolList[i].CommandList[j].Type.Name + " " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].Size.ToString(), 2) + " " +
                                                    Business.Market.SymbolList[i].CommandList[j].Symbol.Name + " at " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(), Business.Market.SymbolList[i].CommandList[j].Symbol.Digit) +
                                                    " sl: " + Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].StopLoss.ToString(), 2) +
                                                    " tp: " + Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].TakeProfit.ToString(), 2) +
                                                    " (" + Business.Market.SymbolList[i].CommandList[j].Symbol.TickValue.Bid + "/" + Business.Market.SymbolList[i].CommandList[j].Symbol.TickValue.Ask + ")" +
                                                    " [Good till today including SL/TP]";

                                                TradingServer.Facade.FacadeAddNewSystemLog(3, strLog, "[Good till today including SL/TP]", "", "");

                                                string stContent = "'System': delete order #" + Business.Market.SymbolList[i].CommandList[j].CommandCode +
                                                    " " + Business.Market.SymbolList[i].CommandList[j].Type.Name + " " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].Size.ToString(), 2) +
                                                    " " + Business.Market.SymbolList[i].CommandList[j].Symbol.Name + " at " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(), Business.Market.SymbolList[i].CommandList[j].Symbol.Digit) +
                                                    " [Good till today including SL/TP]";

                                                TradingServer.Facade.FacadeAddNewSystemLog(3, stContent, "[Good till today including SL/TP]", "", "");

                                                //REMOVE COMMAND IN SYMBOL LIST
                                                Business.Market.SymbolList[i].CommandList.RemoveAt(j);

                                                j--;
                                                #endregion
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region GOOL TILL TODAY EXCLUDING SL/TP
                                    case "Good till today excluding SL/TP":
                                        {
                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.SymbolList[i].CommandList[j].Type.ID);
                                            if (isPending)
                                            {
                                                #region REMOVE PENDING ORDER OF INVESTOR
                                                Business.OpenRemove newOpenRemove = new OpenRemove();
                                                newOpenRemove.InvestorID = Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID;
                                                newOpenRemove.IsExecutor = true;
                                                newOpenRemove.IsInvestor = true;
                                                newOpenRemove.IsSymbol = false;
                                                newOpenRemove.OpenTradeID = Business.Market.SymbolList[i].CommandList[j].ID;
                                                newOpenRemove.SymbolName = Business.Market.SymbolList[i].Name;
                                                Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                //DELETE COMMAND IN DATABASE
                                                TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.SymbolList[i].CommandList[j].ID);

                                                //INSERT DATABASE THEN CANCEL PENDING ORDER
                                                TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                                    Business.Market.SymbolList[i].CommandList[j].Type.ID, Business.Market.SymbolList[i].CommandList[j].CommandCode,
                                                    Business.Market.SymbolList[i].CommandList[j].OpenTime, Business.Market.SymbolList[i].CommandList[j].OpenPrice,
                                                    DateTime.Now, 0, 0, 0, 0, Business.Market.SymbolList[i].CommandList[j].ExpTime, Business.Market.SymbolList[i].CommandList[j].Size,
                                                    Business.Market.SymbolList[i].CommandList[j].StopLoss, Business.Market.SymbolList[i].CommandList[j].TakeProfit,
                                                    Business.Market.SymbolList[i].CommandList[j].ClientCode, Business.Market.SymbolList[i].SymbolID,
                                                    Business.Market.SymbolList[i].CommandList[j].Taxes, 0, Business.Market.SymbolList[i].CommandList[j].Comment, "11",
                                                    Business.Market.SymbolList[i].CommandList[j].TotalSwap,
                                                    Business.Market.SymbolList[i].CommandList[j].RefCommandID,
                                                    Business.Market.SymbolList[i].CommandList[j].AgentRefConfig,
                                                    Business.Market.SymbolList[i].CommandList[j].IsActivePending,
                                                    Business.Market.SymbolList[i].CommandList[j].IsStopLossAndTakeProfit);

                                                //SENT NOTIFY TO CLIENT
                                                string message = "STO8546";
                                                if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                    Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                                //SEND NOTIFY DELETE PENDING ORDER TO MANAGER
                                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.SymbolList[i].CommandList[j]);

                                                //'1205': close sell stop order #00275799 sell stop 0.10 XAUUSD at 1401.30 sl: 0.00 tp: 0.00 (1410.40/1410.90) exp 6/7/2013 10:48:46 AM
                                                //'1205': delete order #00275799 sell stop 0.10XAUUSD at 1401.30
                                                string strLog = "'System': close " +
                                                    Business.Market.SymbolList[i].CommandList[j].Type.Name + " order #" + Business.Market.SymbolList[i].CommandList[j].CommandCode +
                                                    " " + Business.Market.SymbolList[i].CommandList[j].Type.Name + " " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].Size.ToString(), 2) + " " +
                                                    Business.Market.SymbolList[i].CommandList[j].Symbol.Name + " at " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(), Business.Market.SymbolList[i].CommandList[j].Symbol.Digit) +
                                                    " sl: " + Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].StopLoss.ToString(), 2) +
                                                    " tp: " + Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].TakeProfit.ToString(), 2) +
                                                    " (" + Business.Market.SymbolList[i].CommandList[j].Symbol.TickValue.Bid + "/" + Business.Market.SymbolList[i].CommandList[j].Symbol.TickValue.Ask + ")" +
                                                    " [Good till today including SL/TP]";

                                                TradingServer.Facade.FacadeAddNewSystemLog(3, strLog, "[Good till today including SL/TP]", "", "");

                                                string stContent = "'System': delete order #" + Business.Market.SymbolList[i].CommandList[j].CommandCode +
                                                    " " + Business.Market.SymbolList[i].CommandList[j].Type.Name + " " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].Size.ToString(), 2) +
                                                    " " + Business.Market.SymbolList[i].CommandList[j].Symbol.Name + " at " +
                                                    Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(), Business.Market.SymbolList[i].CommandList[j].Symbol.Digit) +
                                                    " [Good till today including SL/TP]";

                                                TradingServer.Facade.FacadeAddNewSystemLog(3, stContent, "[Good till today including SL/TP]", "", "");

                                                //REMOVE COMMAND IN SYMBOL LIST
                                                Business.Market.SymbolList[i].CommandList.RemoveAt(j);

                                                j--;

                                                #endregion
                                            }
                                        }
                                        break;
                                    #endregion
                                }
                            }
                        }
                        #endregion                        

                        if (!Business.Market.SymbolList[i].IsEnableFreezeMargin)
                            return;

                        List<Business.Investor> listTempInvestor = new List<Investor>();

                        #region CHANGE VALUE MARGIN AND SET FREEZE MARGIN
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {                            
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                bool flagInvestor = false;

                                Business.Market.SymbolList[i].CommandList[j].Symbol.UseFreezeMargin = true;
                                //Business.Market.SymbolList[i].CommandList[j].Margin = Business.Market.SymbolList[i].CommandList[j].CalculationFreezeMarginCommand(Business.Market.SymbolList[i].CommandList[j]);
                                Business.Market.SymbolList[i].CommandList[j].CalculatorMarginCommand(Business.Market.SymbolList[i].CommandList[j]);

                                if (listTempInvestor != null)
                                {
                                    int countInvestor = listTempInvestor.Count;
                                    for (int n = 0; n < countInvestor; n++)
                                    {
                                        if (listTempInvestor[n].Code == Business.Market.SymbolList[i].CommandList[j].Investor.Code)
                                        {
                                            flagInvestor = true;
                                            break;
                                        }
                                    }
                                }

                                if (!flagInvestor)
                                {
                                    listTempInvestor.Add(Business.Market.SymbolList[i].CommandList[j].Investor);
                                }
                            }
                        }
                        #endregion     

                        #region SEND COMMAND TO INVESTOR ONLINE
                        if (listTempInvestor != null)
                        {
                            int countInvestor = listTempInvestor.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (listTempInvestor[j].CommandList != null && listTempInvestor[j].CommandList.Count > 0)
                                {
                                    int countCommand = listTempInvestor[j].CommandList.Count;
                                    for (int n = 0; n < countCommand; n++)
                                    {
                                        if (listTempInvestor[j].CommandList[n].Symbol.Name.Trim().ToUpper() == TargetName.Trim().ToUpper())
                                        {
                                            listTempInvestor[j].CommandList[n].Symbol.UseFreezeMargin = true;
                                            //listTempInvestor[j].CommandList[n].Margin = listTempInvestor[j].CommandList[n].CalculationFreezeMarginCommand(listTempInvestor[j].CommandList[n]);
                                            listTempInvestor[j].CommandList[n].CalculatorMarginCommand(listTempInvestor[j].CommandList[n]);
                                        }
                                    }
                                }

                                #region RECALCULATION ACCOUNT OF INVESTOR
                                //Recalculation total margin of investor
                                if (listTempInvestor[j].CommandList.Count > 0)
                                {
                                    Business.Margin newMargin = new Margin();
                                    newMargin = listTempInvestor[j].CommandList[0].Symbol.CalculationTotalMargin(listTempInvestor[j].CommandList);
                                    listTempInvestor[j].Margin = newMargin.TotalMargin;
                                    listTempInvestor[j].FreezeMargin = newMargin.TotalFreezeMargin;
                                }
                                else
                                {
                                    listTempInvestor[j].Margin = 0;
                                    listTempInvestor[j].FreezeMargin = 0;
                                }
                                #endregion                                

                                //SEND COMMAND TO AGENT SERVER
                                Business.AgentNotify newAgentNotify = new AgentNotify();
                                newAgentNotify.NotifyMessage = "UpdateInvestorAccount$" + listTempInvestor[j].InvestorID;
                                TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, listTempInvestor[j].InvestorGroupInstance);

                                //SEND NOTIFY TO MANAGE RESET ACCOUNT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, listTempInvestor[j]);

                                #region SEND COMMAND TO INVESTOR IF INVESTOR ONLINE
                                if (listTempInvestor[j].IsOnline)
                                {
                                    string message = "FRZM14785213";
                                    if (listTempInvestor[j].ClientCommandQueue == null)
                                        listTempInvestor[j].ClientCommandQueue = new List<string>();

                                        listTempInvestor[j].ClientCommandQueue.Add(message);
                                }
                                #endregion                                
                            }
                        }
                        #endregion                        

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// EVENT WEEK OPEN TRADE SYMBOL
        /// </summary>
        internal void OpenTradeSymbol(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        if (!Business.Market.SymbolList[i].IsTrade)
                        {
                            Business.Market.SymbolList[i].IsTrade = true;

                            //SEND NOTIFY TO CLIENT
                            string content = "OTS53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsTrade;
                            TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                        }

                        if (Business.Market.SymbolList[i].IsEnableFreezeMargin)
                        {
                            Business.Market.SymbolList[i].UseFreezeMargin = false;
                            List<Business.Investor> listInvestor = new List<Investor>();

                            #region RESET FREEZE MARGIN AND CALCULATION MARGION OF COMMAND
                            if (Business.Market.SymbolList[i].CommandList != null)
                            {                                
                                int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                                for (int j = 0; j < countCommand; j++)
                                {
                                    bool flagInvestor = false;
                                    Business.Market.SymbolList[i].CommandList[j].CalculatorMarginCommand(Business.Market.SymbolList[i].CommandList[j]);
                                    Business.Market.SymbolList[i].CommandList[j].FreezeMargin = 0;

                                    if (listInvestor != null)
                                    {
                                        int countInvestor = listInvestor.Count;
                                        for (int n = 0; n < countInvestor; n++)
                                        {
                                            if (listInvestor[n].InvestorID == Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID)
                                            {
                                                flagInvestor = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!flagInvestor)
                                    {
                                        listInvestor.Add(Business.Market.SymbolList[i].CommandList[j].Investor);
                                    }
                                }
                            }
                            #endregion

                            #region COMMENT CODE(06/07/2011)
                            //if (timeEvent.NumSession == 0)    //First session
                            //{
                            //    #region RESET FREEZE MARGIN AND CALCULATION MARGION OF COMMAND
                            //    if (Business.Market.SymbolList[i].CommandList != null)
                            //    {
                            //        bool flagInvestor = false;
                            //        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            //        for (int j = 0; j < countCommand; j++)
                            //        {
                            //            Business.Market.SymbolList[i].CommandList[j].CalculatorMarginCommand(Business.Market.SymbolList[i].CommandList[j]);
                            //            Business.Market.SymbolList[i].CommandList[j].FreezeMargin = 0;

                            //            if (listInvestor != null)
                            //            {
                            //                int countInvestor = listInvestor.Count;
                            //                for (int n = 0; n < countInvestor; n++)
                            //                {
                            //                    if (listInvestor[n].InvestorID == Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID)
                            //                    {
                            //                        flagInvestor = true;
                            //                        break;
                            //                    }
                            //                }
                            //            }

                            //            if (!flagInvestor)
                            //            {
                            //                listInvestor.Add(Business.Market.SymbolList[i].CommandList[j].Investor);
                            //            }
                            //        }
                            //    }
                            //    #endregion
                            //}
                            //else    //Second Session
                            //{
                            //    #region RESET FREEZE MARGIN AND CALCULATION MARGION OF COMMAND
                            //    if (Business.Market.SymbolList[i].CommandList != null)
                            //    {
                            //        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            //        for (int j = 0; j < countCommand; j++)
                            //        {
                            //            bool flagInvestor = false;

                            //            DateTime timeSession = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timeEvent.Time.Hour, timeEvent.Time.Minute, 00);
                            //            TimeSpan checkSessionOpen = Business.Market.SymbolList[i].CommandList[j].OpenTime - timeSession;
                            //            if (checkSessionOpen.TotalSeconds > 0)  //command open in session
                            //            {
                            //                Business.Market.SymbolList[i].CommandList[j].FreezeMargin = 0;
                            //                Business.Market.SymbolList[i].CommandList[j].CalculatorMarginCommand(Business.Market.SymbolList[i].CommandList[j]);
                            //            }
                            //            else    //command open before session
                            //            {
                            //                Business.Market.SymbolList[i].CommandList[j].FreezeMargin = 0;
                            //                Business.Market.SymbolList[i].CommandList[j].Margin = Business.Market.SymbolList[i].CommandList[j].CalculationFreezeMarginCommand(Business.Market.SymbolList[i].CommandList[j]);
                            //            }

                            //            if (listInvestor != null)
                            //            {
                            //                int countInvestor = listInvestor.Count;
                            //                for (int n = 0; n < countInvestor; n++)
                            //                {
                            //                    if (listInvestor[n].InvestorID == Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID)
                            //                    {
                            //                        flagInvestor = true;
                            //                        break;
                            //                    }
                            //                }
                            //            }

                            //            if (!flagInvestor)
                            //            {
                            //                listInvestor.Add(Business.Market.SymbolList[i].CommandList[j].Investor);
                            //            }
                            //        }
                            //    }
                            //    #endregion
                            //}
                            #endregion                            

                            #region SEND COMMAND TO INVESTOR ONLINE
                            if (listInvestor != null)
                            {
                                int countInvestor = listInvestor.Count;
                                for (int j = 0; j < countInvestor; j++)
                                {
                                    if (listInvestor[j].CommandList != null && listInvestor[j].CommandList.Count > 0)
                                    {
                                        int countCommand = listInvestor[j].CommandList.Count;
                                        for (int n = 0; n < countCommand; n++)
                                        {
                                            if (listInvestor[j].CommandList[n].Symbol.Name.Trim().ToUpper() == TargetName.Trim().ToUpper())
                                            {
                                                listInvestor[j].CommandList[n].Symbol.UseFreezeMargin = false;
                                                listInvestor[j].CommandList[n].CalculatorMarginCommand(listInvestor[j].CommandList[n]);
                                            }
                                        }
                                    }

                                    if (listInvestor[j].CommandList != null && listInvestor[j].CommandList.Count > 0)
                                    {
                                        Business.Margin newMargin = new Margin();
                                        newMargin = listInvestor[j].CommandList[0].Symbol.CalculationTotalMargin(listInvestor[j].CommandList);
                                        listInvestor[j].Margin = newMargin.TotalMargin;
                                        listInvestor[j].FreezeMargin = newMargin.TotalFreezeMargin;
                                    }
                                    else
                                    {
                                        listInvestor[j].Margin = 0;
                                        listInvestor[j].FreezeMargin = 0;
                                    }

                                    //SEND COMMAND TO AGENT SERVER
                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = "UpdateInvestorAccount$" + listInvestor[j].InvestorID;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, listInvestor[j].InvestorGroupInstance);

                                    //SEND NOTIFY TO MANAGER THEN CLOSE COMMAND
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, listInvestor[j]);

                                    if (listInvestor[j].IsOnline)
                                    {
                                        string message = "FRZM14785213";

                                        if (listInvestor[j].ClientCommandQueue == null)
                                            listInvestor[j].ClientCommandQueue = new List<string>();

                                        listInvestor[j].ClientCommandQueue.Add(message);
                                    }
                                }
                            }
                            #endregion
                        }

                        break;
                    }
                }
            }         
        }

        /// <summary>
        /// EVENT YEAR BEGIN HOLIDAY
        /// </summary>
        internal void BeginHoliday(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            #region FIND SYMBOL IN SYMBOL LIST
            if (TargetName.ToUpper() == "ALL")
            {
                if (Business.Market.SymbolList != null)
                {
                    int count = Business.Market.SymbolList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Market.SymbolList[i].IsHoliday = true;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                    }
                }

                return;
            }

            bool FlagSymbol = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        Business.Market.SymbolList[i].IsHoliday = true;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);

                        FlagSymbol = true;
                        break;
                    }
                }
            }
            #endregion

            #region FIND SECURITY IN SECURITY LIST
            if (FlagSymbol == false)
            {
                if (Business.Market.SecurityList != null)
                {
                    int count = Business.Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.SecurityList[i].Name == TargetName)
                        {
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < countSymbol; j++)
                                {
                                    if (Business.Market.SymbolList[j].SecurityID == Business.Market.SecurityList[i].SecurityID)
                                    {
                                        Business.Market.SymbolList[j].IsHoliday = true;

                                        //SEND NOTIFY TO CLIENT
                                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
            #endregion            
        }

        /// <summary>
        /// EVENT YEAR END HOLIDAY
        /// </summary>
        internal void EndHoliday(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (TargetName.ToUpper() == "ALL")
            {
                if (Business.Market.SymbolList != null)
                {
                    int count = Business.Market.SymbolList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Market.SymbolList[i].IsHoliday = false;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                    }
                }

                return;
            }

            #region FIND SYMBOL IN LIST SYMBOL
            bool FlagSymbol = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        Business.Market.SymbolList[i].IsHoliday = false;


                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);

                        FlagSymbol = true;
                        break;
                    }
                }
            }
            #endregion

            #region FIND SECURITY IN SECURITY LIST
            if (FlagSymbol == false)
            {
                if (Business.Market.SecurityList != null)
                {
                    int count = Business.Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.SecurityList[i].Name == TargetName)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.SymbolList[j].SecurityID == Business.Market.SecurityList[i].SecurityID)
                                {
                                    Business.Market.SymbolList[j].IsHoliday = false;

                                    //SEND NOTIFY TO CLIENT
                                    string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                                    TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                                }
                            }

                            break;
                        }
                    }
                }
            }
            #endregion           
        }

        /// <summary>
        /// EVENT YEAR BEGIN WORK
        /// </summary>
        /// <param name="TargetName"></param>
        internal void BeginWork(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            if (TargetName.ToUpper() == "ALL")
            {
                if (Business.Market.SymbolList != null)
                {
                    int count = Business.Market.SymbolList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Market.SymbolList[i].IsHoliday = false;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                    }
                }

                return;
            }

            #region FIND SYMBOL IN SYMBOL LIST
            bool FlagSymbol = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        Business.Market.SymbolList[i].IsHoliday = false;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);

                        FlagSymbol = true;
                        break;
                    }
                }
            }
            #endregion

            #region FIND SECURITY IN SECURITY LIST
            if (FlagSymbol == false)
            {
                if (Business.Market.SecurityList != null)
                {
                    int count = Business.Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.SecurityList[i].Name == TargetName)
                        {
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < countSymbol; j++)
                                {
                                    if (Business.Market.SymbolList[j].SecurityID == Business.Market.SecurityList[i].SecurityID)
                                    {
                                        Business.Market.SymbolList[j].IsHoliday = false;

                                        //SEND NOTIFY TO CLIENT
                                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
            #endregion            
        }

        /// <summary>
        /// EVENT YEAR END WORK
        /// </summary>
        /// <param name="TargetName"></param>
        internal void EndWork(string TargetName, Business.TimeEvent timeEvent)
        {
            return;
            if (TargetName.ToUpper() == "ALL")
            {
                if (Business.Market.SymbolList != null)
                {
                    int count = Business.Market.SymbolList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Market.SymbolList[i].IsHoliday = true;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                    }
                }

                return;
            }

            #region FIND SYMBOL IN SYMBOL LIST
            bool FlagSymbol = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == TargetName)
                    {
                        Business.Market.SymbolList[i].IsHoliday = true;

                        //SEND NOTIFY TO CLIENT
                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);

                        FlagSymbol = true;
                        break;
                    }
                }
            }
            #endregion

            #region FIND SECURITY IN SECURITY LIST
            if (!FlagSymbol)
            {
                if (Business.Market.SecurityList != null)
                {
                    int count = Business.Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.SecurityList[i].Name == TargetName)
                        {
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < count; j++)
                                {
                                    if (Business.Market.SymbolList[j].SecurityID == Business.Market.SecurityList[i].SecurityID)
                                    {
                                        Business.Market.SymbolList[j].IsHoliday = true;

                                        //SEND NOTIFY TO CLIENT
                                        string content = "HLD53427640$" + TargetName + "{" + Business.Market.SymbolList[i].IsHoliday;
                                        TradingServer.Business.Market.SendNotifyToClient(content, 2, 0);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }            
            #endregion           
        }

        /// <summary>
        /// EVENT YEAR BEGIN MAINTAIN
        /// </summary>
        internal void BeginMaintain(string TargetName, Business.TimeEvent timeEvent)
        {
            return;
        }

        /// <summary>
        /// EVENT YEAR END MAINTAIN
        /// </summary>
        internal void EndMaintain(string TargetName, Business.TimeEvent timeEvent)
        {
            return;
        }

        /// <summary>
        /// BEGIN CALCULATION SWAP OF SYMBOL
        /// </summary>
        internal void BeginCalculationSwap(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            List<Business.Investor> listInvestor = new List<Investor>();

            #region BUILD TIME CURRENT
            DateTime timeCurrent;   //USING CHECK THREE DAY SWAP

            if (DateTime.Now.Hour < 10)
                timeCurrent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 00, 00, 00);
            else
                timeCurrent = DateTime.Now;
            #endregion

            //IF DAY OF WEEK == SUNDAY OR SATURDAY
            if (timeCurrent.DayOfWeek == DayOfWeek.Saturday || timeCurrent.DayOfWeek == DayOfWeek.Sunday)
                return;

            #region CALCULATION SWAP
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                {
                    bool isComplete = false;
                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.CommandExecutor[i].Type.ID);
                    if (!isPending)
                    {
                        //Begin log calculation swap
                        string strBeforeBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].Investor.Balance.ToString(), 2);
                        string strBeforeSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].Swap.ToString(), 2);
                        string strBeforeTotalSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].TotalSwap.ToString(), 2);
                        string strBid = string.Empty;
                        string strAsk = string.Empty;

                        #region CHECK IF COMMAND OPEN AFTER TIME CURRENT THEN DON'T CALCULATION SWAP
                        TimeSpan checkTime = timeCurrent - Business.Market.CommandExecutor[i].OpenTime;

                        if (checkTime.TotalSeconds < 0)
                            continue;
                        #endregion

                        bool IsEnable = false;
                        string Type = string.Empty;
                        double LongPosition = 0;
                        double ShortPosition = 0;
                        string ThreeDaySwaps = string.Empty;
                        bool UseOpenPrice = false;
                        int SpreadDifference = 0;
                        double CloseAsk = 0;
                        double closeAskCurrency = 0;
                        double SwapPrice = 0;

                        #region GET PARAMETER CALCULATION SWAP
                        if (Business.Market.CommandExecutor[i].Symbol.ParameterItems != null)
                        {
                            int countParameter = Business.Market.CommandExecutor[i].Symbol.ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].Code == "S035")
                                {
                                    if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].BoolValue == 1)
                                        IsEnable = true;
                                }

                                if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].Code == "S036")
                                {
                                    Type = Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].StringValue;
                                }

                                if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].Code == "S037")
                                {
                                    double.TryParse(Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].NumValue, out LongPosition);
                                }

                                if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].Code == "S038")
                                {
                                    double.TryParse(Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].NumValue, out ShortPosition);
                                }

                                if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].Code == "S039")
                                {
                                    ThreeDaySwaps = Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].StringValue;
                                }

                                if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].Code == "S040")
                                {
                                    if (Business.Market.CommandExecutor[i].Symbol.ParameterItems[j].BoolValue == 1)
                                        UseOpenPrice = true;
                                }
                            }
                        }
                        #endregion

                        #region CHECK ISENABLE CALCULATION SWAP
                        if (IsEnable)
                        {
                            DayOfWeek ThreeDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ThreeDaySwaps, true);
                            int numThreeday = 1;
                            if (ThreeDay == timeCurrent.DayOfWeek)
                                numThreeday = 3;

                            switch (Type)
                            {
                                #region BY POINTS[ LOTS * LONG_OR_SHORT_POINTS * POINTSIZE ]
                                case "by points[ lots * long_or_short_points * pointsize ]":
                                    {
                                        #region PROCESS GET SPREAD DIFFIRENCES
                                        if (Business.Market.CommandExecutor[i].IGroupSecurity != null)
                                        {
                                            if (Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig != null)
                                            {
                                                int countIGroupSecurityConfig = Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig.Count;
                                                for (int n = 0; n < countIGroupSecurityConfig; n++)
                                                {
                                                    if (Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig[n].Code == "B14")
                                                    {
                                                        int.TryParse(Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig[n].NumValue, out SpreadDifference);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region PROCESS GET PRICE OF DAY
                                        ProcessQuoteLibrary.Business.BarTick BarTick1M = new ProcessQuoteLibrary.Business.BarTick();
                                        ProcessQuoteLibrary.Business.BarTick barTick1MCurrency = new ProcessQuoteLibrary.Business.BarTick();
                                        //Get Price Bid and Ask Close Of Day
                                        if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes != null)
                                        {
                                            #region GET ONLINE CANDLE WITH SYMBOL
                                            int countQuote = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes.Count;
                                            for (int n = 0; n < countQuote; n++)
                                            {
                                                if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[n].Name == Business.Market.CommandExecutor[i].Symbol.Name)
                                                {
                                                    BarTick1M = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[n].BarTick1M;
                                                    //CloseAsk = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(BarTick1M.Close, Business.Market.CommandExecutor[i].Symbol.SpreadByDefault,
                                                    //    Business.Market.CommandExecutor[i].Symbol.Digit, int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()));
                                                    CloseAsk = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(Business.Market.CommandExecutor[i].Symbol.Digit,
                                                        int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()), BarTick1M.CloseAsk);
                                                    break;
                                                }
                                            }
                                            #endregion

                                            #region GET ONLINE CANDLE WITH CURRENCY
                                            for (int m = 0; m < countQuote; m++)
                                            {
                                                if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[m].Name.Trim() == Business.Market.CommandExecutor[i].Symbol.Currency.Trim())
                                                {
                                                    barTick1MCurrency = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[m].BarTick1M;

                                                    //GET DIGIT OF SYMBOL CURRENCY
                                                    if (Business.Market.SymbolList != null)
                                                    {
                                                        int countSymbol = Business.Market.SymbolList.Count;
                                                        for (int k = 0; k < countSymbol; k++)
                                                        {
                                                            if (Business.Market.SymbolList[k].Name.ToUpper().Trim() == Business.Market.CommandExecutor[i].Symbol.Currency.ToUpper().Trim())
                                                            {
                                                                //closeAskCurrency = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(barTick1MCurrency.Close, 0,
                                                                //   Business.Market.SymbolList[k].Digit, int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()));
                                                                closeAskCurrency = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(Business.Market.SymbolList[k].Digit,
                                                                    int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()), barTick1MCurrency.CloseAsk);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        if (BarTick1M.Open > 0 && BarTick1M.Close > 0)
                                        {
                                            SwapPrice = Math.Round(Business.Market.CommandExecutor[i].Symbol.Points(Business.Market.CommandExecutor[i].Size, LongPosition, ShortPosition,
                                                    BarTick1M.Close, CloseAsk, Business.Market.CommandExecutor[i].Symbol.ContractSize,
                                                    Business.Market.CommandExecutor[i].Symbol.Digit, numThreeday, Business.Market.CommandExecutor[i].Type.ID), 2);

                                            //BUILD STRING BID/ASK SYMBOL
                                            strBid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(BarTick1M.Close.ToString(), Business.Market.CommandExecutor[i].Symbol.Digit);
                                            strAsk = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(CloseAsk.ToString(), Business.Market.CommandExecutor[i].Symbol.Digit);

                                            //if (SwapPrice > 0)
                                            //    SwapPrice = -SwapPrice;

                                            Business.Market.CommandExecutor[i].TotalSwap += SwapPrice;

                                            TradingServer.Facade.FacadeUpdateTotalSwapOnlineCommand(Business.Market.CommandExecutor[i].ID, Business.Market.CommandExecutor[i].TotalSwap);

                                            double totalSwaps = Business.Market.CommandExecutor[i].Swap + SwapPrice;

                                            //Set new Swap To Command
                                            Business.Market.CommandExecutor[i].Swap = totalSwaps;

                                            //Call Fucntion Update Swap In Investor List
                                            bool resultUpdateInInvestorList = Market.marketInstance.FindAndUpdateSwapInInvestorList(Business.Market.CommandExecutor[i].ID,
                                                Business.Market.CommandExecutor[i].Investor.InvestorID, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                            //Call Function Update Swap In Symbol List
                                            bool resultUpdateInSymbolList = Market.marketInstance.FindAndUpdateSwapInSymbolList(Business.Market.CommandExecutor[i].ID,
                                                Business.Market.CommandExecutor[i].Symbol.Name, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                            //UPDATE SWAP ONLINE COMMAND
                                            TradingServer.Facade.FacadeUpdateSwapOnlineCommand(Business.Market.CommandExecutor[i].ID,
                                                Business.Market.CommandExecutor[i].Swap);

                                            isComplete = true;
                                        }
                                    }
                                    break;
                                #endregion

                                #region BY MONEY [ LOTS * LONG_OR-SHORT ]
                                case "by money [ lots * long_or-short ]":
                                    {
                                        SwapPrice = Math.Round(Business.Market.CommandExecutor[i].Symbol.Money(Business.Market.CommandExecutor[i].Size, LongPosition, ShortPosition, numThreeday,
                                            Business.Market.CommandExecutor[i].Type.ID), 2);

                                        //BUILD BID/ASK SYMBOL
                                        strBid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].Symbol.TickValue.Bid.ToString(),
                                            Business.Market.CommandExecutor[i].Symbol.Digit);
                                        strAsk = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].Symbol.TickValue.Ask.ToString(),
                                            Business.Market.CommandExecutor[i].Symbol.Digit);

                                        //if (SwapPrice > 0)
                                        //    SwapPrice = -SwapPrice;
                                                                                
                                        Business.Market.CommandExecutor[i].TotalSwap += SwapPrice;

                                        TradingServer.Facade.FacadeUpdateTotalSwapOnlineCommand(Business.Market.CommandExecutor[i].ID, Business.Market.CommandExecutor[i].TotalSwap);

                                        double totalSwaps = Business.Market.CommandExecutor[i].Swap + SwapPrice;

                                        Business.Market.CommandExecutor[i].Swap = totalSwaps;

                                        //Call Fucntion Update Swap In Investor List
                                        bool resultUpdateInInvestorList = Market.marketInstance.FindAndUpdateSwapInInvestorList(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Investor.InvestorID, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                        //Call Function Update Swap In Symbol List
                                        bool resultUpdateInSymbolList = Market.marketInstance.FindAndUpdateSwapInSymbolList(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Symbol.Name, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                        //UPDATE SWAP IN DATABASE
                                        TradingServer.Facade.FacadeUpdateSwapOnlineCommand(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Swap);

                                        isComplete = true;  
                                    }
                                    break;
                                #endregion

                                #region BY INTEREST [ LOTS * LONG_OR_SHORT / 100 / 360 ]
                                case "by interest [ lots * long_or_short / 100 / 360 ]":
                                    {
                                        #region PROCESS GET SPREAD DIFFIRENCES
                                        if (Business.Market.CommandExecutor[i].IGroupSecurity != null)
                                        {
                                            if (Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig != null)
                                            {
                                                int countIGroupSecurityConfig = Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig.Count;
                                                for (int n = 0; n < countIGroupSecurityConfig; n++)
                                                {
                                                    if (Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig[n].Code == "B14")
                                                    {
                                                        int.TryParse(Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig[n].NumValue, out SpreadDifference);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region PROCESS GET PRICE OF DAY
                                        ProcessQuoteLibrary.Business.BarTick BarTick1M = new ProcessQuoteLibrary.Business.BarTick();
                                        ProcessQuoteLibrary.Business.BarTick barTick1MCurrency = new ProcessQuoteLibrary.Business.BarTick();
                                        //Get Price Bid and Ask Close Of Day
                                        if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes != null)
                                        {
                                            #region GET ONLINE CANDLE WITH SYMBOL
                                            int countQuote = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes.Count;
                                            for (int n = 0; n < countQuote; n++)
                                            {
                                                if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[n].Name == Business.Market.CommandExecutor[i].Symbol.Name)
                                                {
                                                    BarTick1M = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[n].BarTick1M;
                                                    //CloseAsk = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(BarTick1M.Close, Business.Market.CommandExecutor[i].Symbol.SpreadByDefault,
                                                    //    Business.Market.CommandExecutor[i].Symbol.Digit, int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()));
                                                    CloseAsk = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(Business.Market.CommandExecutor[i].Symbol.Digit,
                                                        int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()), BarTick1M.CloseAsk);
                                                    break;
                                                }
                                            }
                                            #endregion

                                            #region GET ONLINE CANDLE WITH CURRENCY
                                            for (int m = 0; m < countQuote; m++)
                                            {
                                                if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[m].Name.Trim() == Business.Market.CommandExecutor[i].Symbol.Currency.Trim())
                                                {
                                                    barTick1MCurrency = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[m].BarTick1M;

                                                    //GET DIGIT OF SYMBOL CURRENCY
                                                    if (Business.Market.SymbolList != null)
                                                    {
                                                        int countSymbol = Business.Market.SymbolList.Count;
                                                        for (int k = 0; k < countSymbol; k++)
                                                        {
                                                            if (Business.Market.SymbolList[k].Name.ToUpper().Trim() == Business.Market.CommandExecutor[i].Symbol.Currency.ToUpper().Trim())
                                                            {
                                                                //closeAskCurrency = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(barTick1MCurrency.Close, 0,
                                                                //   Business.Market.SymbolList[k].Digit, int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()));
                                                                closeAskCurrency = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(Business.Market.SymbolList[k].Digit,
                                                                    int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()), barTick1MCurrency.CloseAsk);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        //Call Function Convert If Symbol Is XXXUSD    
                                        #region GET LONG AND SHORT IN IGROUPSYMBOL
                                        if (Business.Market.IGroupSymbolList != null)
                                        {
                                            int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
                                            for (int j = 0; j < countIGroupSymbol; j++)
                                            {
                                                if (Business.Market.IGroupSymbolList[j].SymbolID == Business.Market.CommandExecutor[i].Symbol.SymbolID &&
                                                    Business.Market.IGroupSymbolList[j].InvestorGroupID == Business.Market.CommandExecutor[i].Investor.InvestorGroupInstance.InvestorGroupID)
                                                {
                                                    if (Business.Market.IGroupSymbolList[j].IGroupSymbolConfig != null)
                                                    {
                                                        int countParameter = Business.Market.IGroupSymbolList[j].IGroupSymbolConfig.Count;
                                                        for (int n = 0; n < countParameter; n++)
                                                        {
                                                            if (Business.Market.IGroupSymbolList[j].IGroupSymbolConfig[n].Code == "GS01")
                                                            {
                                                                LongPosition = double.Parse(Business.Market.IGroupSymbolList[j].IGroupSymbolConfig[n].NumValue);
                                                            }

                                                            if (Business.Market.IGroupSymbolList[j].IGroupSymbolConfig[n].Code == "GS02")
                                                            {
                                                                ShortPosition = double.Parse(Business.Market.IGroupSymbolList[j].IGroupSymbolConfig[n].NumValue);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        //Calculation Swaps
                                        SwapPrice = Math.Round(Business.Market.CommandExecutor[i].Symbol.Interest(Business.Market.CommandExecutor[i].Size, LongPosition, ShortPosition, BarTick1M.Close,
                                            Business.Market.CommandExecutor[i].Symbol.ContractSize, numThreeday, Business.Market.CommandExecutor[i].Type.ID), 2);

                                        //BUILD BID/ASK SYMBOL
                                        strBid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(BarTick1M.Close.ToString(), Business.Market.CommandExecutor[i].Symbol.Digit);
                                        strAsk = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(CloseAsk.ToString(), Business.Market.CommandExecutor[i].Symbol.Digit);

                                        double closePrice = 0;

                                        if (Business.Market.CommandExecutor[i].Type.ID == 1)
                                            closePrice = barTick1MCurrency.Close;
                                        else
                                            closePrice = closeAskCurrency;

                                        SwapPrice = Business.Market.CommandExecutor[i].Symbol.ConvertCurrencyToUSD(Business.Market.CommandExecutor[i].Symbol.Currency, SwapPrice, closePrice, 2);

                                        //if (SwapPrice > 0)
                                        //    SwapPrice = -SwapPrice;
                                                                                
                                        Business.Market.CommandExecutor[i].TotalSwap += SwapPrice;

                                        TradingServer.Facade.FacadeUpdateTotalSwapOnlineCommand(Business.Market.CommandExecutor[i].ID, Business.Market.CommandExecutor[i].TotalSwap);

                                        double totalSwaps = Business.Market.CommandExecutor[i].Swap + SwapPrice;

                                        //Set new Swap To Command
                                        Business.Market.CommandExecutor[i].Swap = totalSwaps;

                                        //Call Fucntion Update Swap In Investor List
                                        bool resultUpdateInInvestorList = Market.marketInstance.FindAndUpdateSwapInInvestorList(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Investor.InvestorID, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                        //Call Function Update Swap In Symbol List
                                        bool resultUpdateInSymbolList = Market.marketInstance.FindAndUpdateSwapInSymbolList(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Symbol.Name, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                        //UPDATE SWAP IN DATABASE   
                                        TradingServer.Facade.FacadeUpdateSwapOnlineCommand(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Swap);

                                        isComplete = true;
                                    }
                                    break;
                                #endregion

                                #region BY MONEY IN MARGIN CURRENCY [ LOTS * LONG_OR_SHORT ]
                                case "by money in margin currency [ lots * long_or_short ]":
                                    {
                                        #region PROCESS GET SPREAD DIFFIRENCES
                                        if (Business.Market.CommandExecutor[i].IGroupSecurity != null)
                                        {
                                            if (Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig != null)
                                            {
                                                int countIGroupSecurityConfig = Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig.Count;
                                                for (int n = 0; n < countIGroupSecurityConfig; n++)
                                                {
                                                    if (Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig[n].Code == "B14")
                                                    {
                                                        int.TryParse(Business.Market.CommandExecutor[i].IGroupSecurity.IGroupSecurityConfig[n].NumValue, out SpreadDifference);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region PROCESS GET PRICE OF DAY
                                        ProcessQuoteLibrary.Business.BarTick BarTick1M = new ProcessQuoteLibrary.Business.BarTick();
                                        ProcessQuoteLibrary.Business.BarTick barTick1MCurrency = new ProcessQuoteLibrary.Business.BarTick();
                                        //Get Price Bid and Ask Close Of Day
                                        if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes != null)
                                        {
                                            #region GET ONLINE CANDLE WITH SYMBOL
                                            int countQuote = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes.Count;
                                            for (int n = 0; n < countQuote; n++)
                                            {
                                                if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[n].Name == Business.Market.CommandExecutor[i].Symbol.Name)
                                                {
                                                    BarTick1M = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[n].BarTick1M;
                                                    //CloseAsk = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(BarTick1M.Close, Business.Market.CommandExecutor[i].Symbol.SpreadByDefault,
                                                    //    Business.Market.CommandExecutor[i].Symbol.Digit, int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()));
                                                    CloseAsk = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(Business.Market.CommandExecutor[i].Symbol.Digit,
                                                        int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()), BarTick1M.CloseAsk);
                                                    break;
                                                }
                                            }
                                            #endregion

                                            #region GET ONLINE CANDLE WITH CURRENCY
                                            for (int m = 0; m < countQuote; m++)
                                            {
                                                if (ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[m].Name.Trim() == Business.Market.CommandExecutor[i].Symbol.Currency.Trim())
                                                {
                                                    barTick1MCurrency = ProcessQuoteLibrary.Business.QuoteProcess.listQuotes[m].BarTick1M;

                                                    //GET DIGIT OF SYMBOL CURRENCY
                                                    if (Business.Market.SymbolList != null)
                                                    {
                                                        int countSymbol = Business.Market.SymbolList.Count;
                                                        for (int k = 0; k < countSymbol; k++)
                                                        {
                                                            if (Business.Market.SymbolList[k].Name.ToUpper().Trim() == Business.Market.CommandExecutor[i].Symbol.Currency.ToUpper().Trim())
                                                            {
                                                                //closeAskCurrency = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(barTick1MCurrency.Close, 0,
                                                                //   Business.Market.SymbolList[k].Digit, int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()));
                                                                closeAskCurrency = Business.Market.CommandExecutor[i].Symbol.CreateAskPrices(Business.Market.SymbolList[k].Digit,
                                                                    int.Parse(Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade.ToString()), barTick1MCurrency.CloseAsk);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        //Calculation Swap
                                        SwapPrice = Math.Round(Business.Market.CommandExecutor[i].Symbol.MoneyInMarginCurrency(Business.Market.CommandExecutor[i].Size,
                                            LongPosition, ShortPosition, numThreeday, Business.Market.CommandExecutor[i].Type.ID), 2);

                                        //BUILD BID/ASK SYMBOL
                                        strBid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(BarTick1M.Close.ToString(), Business.Market.CommandExecutor[i].Symbol.Digit);
                                        strAsk = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(CloseAsk.ToString(), Business.Market.CommandExecutor[i].Symbol.Digit);

                                        double closePrice = 0;

                                        if (Business.Market.CommandExecutor[i].Type.ID == 1)
                                            closePrice = barTick1MCurrency.Close;
                                        else
                                            closePrice = closeAskCurrency;

                                        Business.Market.CommandExecutor[i].Symbol.ConvertCurrencyToUSD(Business.Market.CommandExecutor[i].Symbol.Currency, SwapPrice, closePrice, 2);

                                        //if (SwapPrice > 0)
                                        //    SwapPrice = -SwapPrice;

                                        Business.Market.CommandExecutor[i].TotalSwap += SwapPrice;

                                        TradingServer.Facade.FacadeUpdateTotalSwapOnlineCommand(Business.Market.CommandExecutor[i].ID, Business.Market.CommandExecutor[i].TotalSwap);

                                        double totalSwaps = Business.Market.CommandExecutor[i].Swap + SwapPrice;
                                        Business.Market.CommandExecutor[i].Swap = totalSwaps;

                                        //Call Fucntion Update Swap In Investor List
                                        bool resultUpdateInInvestorList = Market.marketInstance.FindAndUpdateSwapInInvestorList(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Investor.InvestorID, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                        //Call Function Update Swap In Symbol List
                                        bool resultUpdateInSymbolList = Market.marketInstance.FindAndUpdateSwapInSymbolList(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Symbol.Name, totalSwaps, Business.Market.CommandExecutor[i].TotalSwap);

                                        //UPDATE SWAP IN DATABASE
                                        TradingServer.Facade.FacadeUpdateSwapOnlineCommand(Business.Market.CommandExecutor[i].ID,
                                            Business.Market.CommandExecutor[i].Swap);

                                        isComplete = true;
                                    }
                                    break;
                                #endregion
                            }

                            if (isComplete)
                            {
                                //End login calculation swap
                                string strAfterBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].Investor.Balance.ToString(), 2);
                                string strAfterSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(SwapPrice.ToString(), 2);
                                string strAfterTotalSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].TotalSwap.ToString(), 2);
                                string strSize = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.CommandExecutor[i].Size.ToString(), 2);

                                //'00001140: calculation swap order #004432443 - current balance: 40000 - old swap: 5.00'
                                string content = "'" + Business.Market.CommandExecutor[i].Investor.Code + "': calculation swap order #" + Business.Market.CommandExecutor[i].CommandCode +
                                    " name: " + Business.Market.CommandExecutor[i].Symbol.Name + " size: " + strSize + " old balance: " + strBeforeBalance + " - old swap: " + strBeforeSwap +
                                    " - old total swap: " + strBeforeTotalSwap + " -> " + "current balance: " + strAfterBalance + " - current swap: " + strAfterSwap + " - current total swap: " + strAfterTotalSwap + "(" + strBid + "/" + strAsk + ")";

                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[calculation swap]", "", Business.Market.CommandExecutor[i].Investor.Code);

                                //SEND NOTIFY TO MANAGER CHANGE COMMAND
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Business.Market.CommandExecutor[i]);

                                //SEND NOTIFY TO MANAGER CHANGE ACCOUNT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.CommandExecutor[i].Investor);

                                #region SEND NOTIFY TO INVESTOR IF IT ONLINE
                                if (Business.Market.CommandExecutor[i].Investor.IsOnline)
                                {
                                    //Send Command To Client, Get Account And Online Command
                                    string MessageGetAccount = "CSW5789";

                                    if (Business.Market.CommandExecutor[i].Investor.ClientCommandQueue == null)
                                        Business.Market.CommandExecutor[i].Investor.ClientCommandQueue = new List<string>();

                                    Business.Market.CommandExecutor[i].Investor.ClientCommandQueue.Add(MessageGetAccount);
                                }
                                #endregion

                                #region SEND NOTIFY TO AGENT SYSTEM
                                string msg = "UpdateCommand$True,Update Command," + Business.Market.CommandExecutor[i].ID + "," +
                                        Business.Market.CommandExecutor[i].Investor.InvestorID + "," + Business.Market.CommandExecutor[i].Symbol.Name + "," +
                                        Business.Market.CommandExecutor[i].Size + "," + true + "," + Business.Market.CommandExecutor[i].OpenTime + "," +
                                        Business.Market.CommandExecutor[i].OpenPrice + "," + Business.Market.CommandExecutor[i].StopLoss + "," +
                                        Business.Market.CommandExecutor[i].TakeProfit + "," + Business.Market.CommandExecutor[i].ClosePrice + "," +
                                        Business.Market.CommandExecutor[i].Commission + "," + Business.Market.CommandExecutor[i].Swap + "," +
                                        Business.Market.CommandExecutor[i].Profit + "," + Business.Market.CommandExecutor[i].Comment + "," +
                                        Business.Market.CommandExecutor[i].ID + "," + Business.Market.CommandExecutor[i].Type.Name + "," + 1 + "," +
                                        Business.Market.CommandExecutor[i].ExpTime + "," + Business.Market.CommandExecutor[i].ClientCode + "," +
                                        Business.Market.CommandExecutor[i].CommandCode + "," + Business.Market.CommandExecutor[i].IsHedged + "," +
                                        Business.Market.CommandExecutor[i].Type.ID + "," + Business.Market.CommandExecutor[i].Margin + ",Update" + "," +
                                        Business.Market.CommandExecutor[i].AgentRefConfig + "," + Business.Market.CommandExecutor[i].SpreaDifferenceInOpenTrade;

                                //SEND NOTIFY UPDATE COMMAND TO AGENT
                                Business.AgentNotify newAgentNotify = new AgentNotify();
                                newAgentNotify.NotifyMessage = msg;
                                TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Business.Market.CommandExecutor[i].Investor.InvestorGroupInstance);
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            if (Business.Market.InvestorList != null)
            {
                TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.InvestorList, timeCurrent);
            }
        }

        /// <summary>
        /// SEND REPORT IN DAY
        /// </summary>
        /// <param name="TargetName"></param>
        internal void SendReportDay(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            string total = string.Empty;
            Business.Market.LogContentSendMail = string.Empty;

            #region GET END OF DAY
            DateTime timeCurrent = DateTime.Now;
            DateTime timeEndDay = new DateTime();
            DateTime timeStartDay = new DateTime();
            bool IsReportWeekends = false;
            string pathStatements = string.Empty;
            string serverName = string.Empty;

            #region GET MARKET CONFIG
            if (Business.Market.MarketConfig != null)
            {
                int count = Business.Market.MarketConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C02")
                    {
                        serverName = Business.Market.MarketConfig[i].StringValue;
                    }

                    if (Business.Market.MarketConfig[i].Code == "C26")
                    {
                        if (Business.Market.MarketConfig[i].BoolValue == 1)
                            IsReportWeekends = true;
                    }

                    if (Business.Market.MarketConfig[i].Code == "C35")
                    {
                        pathStatements = Business.Market.MarketConfig[i].StringValue;
                    }
                }
            }
            #endregion   
         
            if (string.IsNullOrEmpty(pathStatements))
                pathStatements = Environment.CurrentDirectory + DateTime.Now.Ticks + "_" + serverName;

            if (DateTime.Now.Hour < 10)
                timeCurrent = timeCurrent.AddDays(-1);

            timeStartDay = new DateTime(timeCurrent.Year, timeCurrent.Month, timeCurrent.Day , 00, 00, 00);
            timeEndDay = new DateTime(timeCurrent.Year, timeCurrent.Month, timeCurrent.Day, 23, 59, 59);            
            #endregion

            #region CHECK Generate statements at weekends
            if (timeStartDay.DayOfWeek == DayOfWeek.Saturday || timeStartDay.DayOfWeek == DayOfWeek.Sunday)
            {
                if (!IsReportWeekends)
                    return;
            }
            #endregion
                        
            //CLEAR LIST END OF DAY AGENT
            Business.Market.ListEODAgent.Clear();

            #region SEND MAIL TO INVESTOR
            if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
            {   
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    List<Business.OpenTrade> listOpenTrade = new List<OpenTrade>();
                    List<Business.OpenTrade> listPendingOrder = new List<OpenTrade>();
                    List<Business.OpenTrade> listCommandHistory = new List<OpenTrade>();

                    #region GET ONLINE COMMAND OF INVESTOR
                    if (Business.Market.InvestorList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.InvestorList[i].CommandList[j].Type.ID);
                            if (!isPending)
                            {
                                double tempProfit = Math.Round(Business.Market.InvestorList[i].CommandList[j].Profit, 2);
                                Business.Market.InvestorList[i].CommandList[j].Profit = tempProfit;
                                listOpenTrade.Add(Business.Market.InvestorList[i].CommandList[j]);
                            }
                            else
                            {
                                double tempProfit = Math.Round(Business.Market.InvestorList[i].CommandList[j].Profit, 2);
                                Business.Market.InvestorList[i].CommandList[j].Profit = tempProfit;
                                listPendingOrder.Add(Business.Market.InvestorList[i].CommandList[j]);
                            }
                        }
                    }
                    #endregion

                    #region GET COMMAND HISTORY OF INVESTOR
                    listCommandHistory = TradingServer.Facade.FacadeGetCommandHistoryWithTime(Business.Market.InvestorList[i].InvestorID, timeStartDay, timeEndDay);
                    #endregion

                    #region DATA > 0
                    if (listOpenTrade != null && listOpenTrade.Count > 0 ||
                      listPendingOrder != null && listPendingOrder.Count > 0 ||
                      listCommandHistory != null && listCommandHistory.Count > 0)
                    {
                        Model.MailConfig newMailConfig = new Model.MailConfig();
                        newMailConfig = this.GetMailConfig(Business.Market.InvestorList[i]);

                        double tempBalance = Business.Market.InvestorList[i].PreviousLedgerBalance;

                        StringBuilder content = new StringBuilder();
                        Business.ReportItem contentOpenPosition = new ReportItem();
                        Business.ReportItem contentClosePosition = new ReportItem();
                        StringBuilder contentPendingPosition = new StringBuilder();

                        Business.StatementTemplate newTemplate = new StatementTemplate();

                        content = newTemplate.GetStatementReport(listOpenTrade, listCommandHistory, listPendingOrder, Business.Market.InvestorList[i], timeEndDay);
                        content.Replace("[ReportDay]", timeEndDay.ToShortDateString());

                        string tempContent = content.ToString();

                        #region CHECK SCALPER INVESTOR
                        bool isScalper = this.CheckScalperInvestor(listCommandHistory);
                        #endregion

                        if (Business.Market.InvestorList[i].SendReport)
                        {
                            #region GET MAIL CONFIG AND SEND REPORT
                            if (newMailConfig != null)
                            {
                                if (newMailConfig.isEnable)
                                {
                                    if (!string.IsNullOrEmpty(newMailConfig.SmtpHost) && !string.IsNullOrEmpty(newMailConfig.MessageFrom) &&
                                        !string.IsNullOrEmpty(newMailConfig.PasswordCredential))
                                    {
                                        if (!string.IsNullOrEmpty(Business.Market.InvestorList[i].Email))
                                        {
                                            content = content.Replace("[ScalperName]", "");
                                            content = content.Replace("[Scalper]", "");

                                            if (timeStartDay.DayOfWeek == DayOfWeek.Sunday || timeStartDay.DayOfWeek == DayOfWeek.Saturday)
                                            {
                                                if (IsReportWeekends)
                                                {
                                                    Model.TradingCalculate.Instance.SendMailAsync(Business.Market.InvestorList[i].Email, "Daily Confirmation", content.ToString(), newMailConfig);
                                                }
                                            }
                                            else
                                            {
                                                Model.TradingCalculate.Instance.SendMailAsync(Business.Market.InvestorList[i].Email, "Daily Confirmation", content.ToString(), newMailConfig);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        if (!string.IsNullOrEmpty(pathStatements) && pathStatements != "NaN")
                        {   
                            //STREAM FILE SAVE CONTENT SEND MAIL REPORT
                            TradingServer.Model.TradingCalculate.Instance.StreamFile(tempContent.ToString(), Business.Market.InvestorList[i].Code, timeStartDay, 0, pathStatements, isScalper);
                        }

                        if (!string.IsNullOrEmpty(content.ToString()))
                        {
                            Business.Statement newStatement = new Statement();
                            newStatement.InvestorCode = Business.Market.InvestorList[i].Code;
                            newStatement.StatementType = 1;
                            newStatement.TimeStatement = timeStartDay;
                            newStatement.Email = Business.Market.InvestorList[i].Email;
                            newStatement.Content = tempContent.ToString();

                            Business.Market.ListStatement.Add(newStatement);
                        }
                    }
                    #endregion

                    #region UPDATE PREVIOUS LEDGER BALANCE
                    TradingServer.Facade.FacadeUpdatePreviousLedgerBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].PreviousLedgerBalance);
                    #endregion  
             
                    #region PROCESS LAST ACCOUNT
                    Business.SumLastAccount newSumLastAccount = new SumLastAccount();
                    newSumLastAccount.InvestorAccount = Business.Market.InvestorList[i];
                    newSumLastAccount.ListHistory = listCommandHistory;

                    if (newSumLastAccount.ListOpenTrade == null)
                        newSumLastAccount.ListOpenTrade = new List<OpenTrade>();
                     
                    if (listOpenTrade != null)
                    {
                        int countOpenTrade = listOpenTrade.Count;
                        for (int j = 0; j < countOpenTrade; j++)
                        {
                            Business.OpenTrade newOpenTrade = new OpenTrade();
                            newOpenTrade.AgentCommission = listOpenTrade[j].AgentCommission;
                            newOpenTrade.ClientCode = listOpenTrade[j].ClientCode;
                            newOpenTrade.ClosePrice = listOpenTrade[j].ClosePrice;
                            newOpenTrade.CloseTime = listOpenTrade[j].CloseTime;
                            newOpenTrade.CommandCode = listOpenTrade[j].CommandCode;
                            newOpenTrade.Comment = listOpenTrade[j].Comment;
                            newOpenTrade.Commission = listOpenTrade[j].Commission;
                            newOpenTrade.ExpTime = listOpenTrade[j].ExpTime;
                            newOpenTrade.FreezeMargin = listOpenTrade[j].FreezeMargin;
                            newOpenTrade.ID = listOpenTrade[j].ID;
                            newOpenTrade.IGroupSecurity = listOpenTrade[j].IGroupSecurity;
                            newOpenTrade.Investor = listOpenTrade[j].Investor;
                            newOpenTrade.IsClose = listOpenTrade[j].IsClose;
                            newOpenTrade.IsHedged = listOpenTrade[j].IsHedged;
                            newOpenTrade.Margin = listOpenTrade[j].Margin;
                            newOpenTrade.MaxDev = listOpenTrade[j].MaxDev;
                            newOpenTrade.NumberUpdate = listOpenTrade[j].NumberUpdate;
                            newOpenTrade.OpenPrice = listOpenTrade[j].OpenPrice;
                            newOpenTrade.OpenTime = listOpenTrade[j].OpenTime;

                            double tempProfit = listOpenTrade[j].Profit;
                            newOpenTrade.Profit = tempProfit;

                            newOpenTrade.Size = listOpenTrade[j].Size;
                            newOpenTrade.SpreaDifferenceInOpenTrade = listOpenTrade[j].SpreaDifferenceInOpenTrade;
                            newOpenTrade.StopLoss = listOpenTrade[j].StopLoss;
                            newOpenTrade.Swap = listOpenTrade[j].Swap;
                            newOpenTrade.Symbol = listOpenTrade[j].Symbol;
                            newOpenTrade.TakeProfit = listOpenTrade[j].TakeProfit;
                            newOpenTrade.Taxes = listOpenTrade[j].Taxes;
                            newOpenTrade.TotalSwap = listOpenTrade[j].TotalSwap;
                            newOpenTrade.Type = listOpenTrade[j].Type;

                            newSumLastAccount.ListOpenTrade.Add(newOpenTrade);
                        }
                    }
                    
                    newSumLastAccount.TimeEndDay = timeEndDay;

                    Business.Market.ListLastAccount.Add(newSumLastAccount);
                    #endregion
                }
            }
            #endregion

            #region SAVE FILE TO HARD DRIVE
            if (!string.IsNullOrEmpty(pathStatements) && pathStatements != "NaN")
            {
                TradingServer.Model.TradingCalculate.Instance.StreamFile(Business.Market.LogContentSendMail, "", timeStartDay, 1, pathStatements);
                string folder = timeStartDay.Year.ToString() + timeStartDay.Month.ToString() + timeStartDay.Day.ToString();
                TradingServer.Model.TradingCalculate.Instance.ZipFolder(pathStatements + @"\" + folder, folder + ".zip", pathStatements + @"\" + folder + ".zip");
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="timeStartDay"></param>
        /// <param name="timeEndDay"></param>
        internal void SendReportDayManaual(string targetName, DateTime timeStartDay, DateTime timeEndDay)
        {
            return;

            DateTime tempTimeStart = timeStartDay;
            DateTime tempTimeEnd = timeEndDay;

            timeStartDay = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 00, 00, 00);
            timeEndDay = new DateTime(tempTimeEnd.Year, tempTimeEnd.Month, tempTimeEnd.Day, 23, 59, 59);

            string pathStatement = string.Empty;
            if (Business.Market.MarketConfig != null)
            {
                int countMarketConfig = Business.Market.MarketConfig.Count;
                for (int i = 0; i < countMarketConfig; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C35")
                    {
                        pathStatement = Business.Market.MarketConfig[i].StringValue;
                        break;
                    }
                }
            }

            #region end html
            string content = string.Empty;
            string begin = string.Empty;
            string header = "<div align=center style='font: 20pt Times New Roman'><b>PT Millennium Penata Futures ET5 Demo Account</b></div></br>";            
            string endString = "</body></html>";
            #endregion  

            #region SEND MAIL TO INVESTOR
            if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {                    
                    if (Business.Market.InvestorList[i].SendReport)
                    {
                        List<Business.OpenTrade> listOpenTrade = new List<OpenTrade>();
                        List<Business.OpenTrade> listPendingOrder = new List<OpenTrade>();
                        List<Business.OpenTrade> listCommandHistory = new List<OpenTrade>();

                        #region GET ONLINE COMMAND OF INVESTOR
                        if (Business.Market.CommandExecutor != null)
                        {
                            int countCommand = Business.Market.CommandExecutor.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.CommandExecutor[j].Investor.InvestorID == Business.Market.InvestorList[i].InvestorID)
                                {
                                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.CommandExecutor[j].Type.ID);
                                    if (!isPending)
                                    {
                                        double tempProfit = Math.Round(Business.Market.CommandExecutor[j].Profit, 2);
                                        Business.Market.CommandExecutor[j].Profit = tempProfit;
                                        listOpenTrade.Add(Business.Market.CommandExecutor[j]);
                                    }
                                    else
                                    {
                                        double tempProfit = Math.Round(Business.Market.CommandExecutor[j].Profit, 2);
                                        Business.Market.CommandExecutor[j].Profit = tempProfit;

                                        listPendingOrder.Add(Business.Market.CommandExecutor[j]);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region GET COMMAND HISTORY OF INVESTOR
                        listCommandHistory = TradingServer.Facade.FacadeGetCommandHistoryWithTime(Business.Market.InvestorList[i].InvestorID, timeStartDay, timeEndDay);
                        #endregion

                        Model.MailConfig newMailConfig = new Model.MailConfig();
                        newMailConfig = this.GetMailConfig(Business.Market.InvestorList[i]);

                        #region DATA > 0
                        if (listOpenTrade != null && listOpenTrade.Count > 0 ||
                          listPendingOrder != null && listPendingOrder.Count > 0 ||
                          listCommandHistory != null && listCommandHistory.Count > 0)
                        {
                            #region REND ACCOUNT HTML
                            StringBuilder headerAccount = new StringBuilder();
                            headerAccount.Append("<tr>");
                            headerAccount.Append("<td align='right'>");
                            headerAccount.Append("<div class='statementHeader'>PT. Millennium Penata Futures ET5 Demo Account</div>");
                            headerAccount.Append("</td>");
                            headerAccount.Append("</tr>");

                            headerAccount.Append("<tr align='left'>");
                            headerAccount.Append("<td>");
                            headerAccount.Append("<table style='width: 100%;border-top:1px black solid;border-bottom:1px black solid;margin-top:20px;padding:0px'>");
                            headerAccount.Append("<tr style='background-color:#EEEEEE;'>");
                            headerAccount.Append("<td style='width: auto;font-weight:bolder;'>Account</td>");
                            headerAccount.Append("<td style='width: auto;font-weight:bolder;'>Name</td>");
                            headerAccount.Append("<td style='width: auto;font-weight:bolder;'>Currency</td>");
                            headerAccount.Append("<td style='width: auto;font-weight:bolder;'>Date</td>");

                            headerAccount.Append("<tr>");
                            headerAccount.Append("<td style='width: auto'>");
                            headerAccount.Append(Business.Market.InvestorList[i].Code + "</td>");
                            headerAccount.Append("<td style='width: auto'>");
                            headerAccount.Append(Business.Market.InvestorList[i].NickName + "</td>");
                            headerAccount.Append("<td style='width: auto'>USD</td>");
                            headerAccount.Append("<td style='width: auto'>");
                            headerAccount.Append(timeEndDay.ToShortDateString());
                            headerAccount.Append("</td>");
                            headerAccount.Append("</tr>");
                            headerAccount.Append("</table>");
                            headerAccount.Append("</td>");
                            headerAccount.Append("</tr>");
                            #endregion
                            
                            Business.ClientReport newClientReport = new ClientReport();
                            
                            string tempHeaderHTML = newClientReport.RendStyleStatement();
                            string tempResult = newClientReport.RendStatement(Business.Market.InvestorList[i], listCommandHistory, listOpenTrade, listPendingOrder);                            

                            string signature = string.Empty;
                            if (!string.IsNullOrEmpty(newMailConfig.Signature))
                            {
                                signature = "<tr><td align='right'>" + newMailConfig.Signature + "</td></tr>";
                                signature += "</table>";
                            }
                                                        
                            content = tempHeaderHTML + headerAccount + tempResult + signature + endString;

                            if (newMailConfig != null)
                            {
                                if (newMailConfig.isEnable)
                                {
                                    if (!string.IsNullOrEmpty(newMailConfig.SmtpHost) && !string.IsNullOrEmpty(newMailConfig.MessageFrom) &&
                                        !string.IsNullOrEmpty(newMailConfig.PasswordCredential))
                                    {
                                        if (!string.IsNullOrEmpty(Business.Market.InvestorList[i].Email))
                                        {
                                            Model.TradingCalculate.Instance.SendMail(Business.Market.InvestorList[i].Email, "Daily Confirmation", content, newMailConfig);

                                            if (!string.IsNullOrEmpty(pathStatement) && pathStatement != "NaN")
                                            {
                                                //STREAM FILE SAVE CONTENT SEND MAIL REPORT
                                                TradingServer.Model.TradingCalculate.Instance.StreamFile(content, Business.Market.InvestorList[i].Code, timeStartDay, 0, pathStatement);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    #region UPDATE PREVIOUS LEDGER BALANCE
                    //TradingServer.Facade.FacadeUpdatePreviousLedgerBalance(Business.Market.InvestorList[i].InvestorID, preBalance);
                    #endregion                     
                }
            }
            #endregion            
        }

        /// <summary>
        /// SEND REPORT IN MONTH
        /// </summary>
        /// <param name="TargetName"></param>
        internal void SendReportMonth(string TargetName, Business.TimeEvent timeEvent)
        {
            return;

            #region GET END OF DAY
            string endOfDay = string.Empty;
            DateTime timeCurrent = DateTime.Now;
            DateTime timeEndDay = new DateTime();
            DateTime timeStartDay = new DateTime();
            bool IsReportWeekend = false;
            string pathStatement = string.Empty;
            if (Business.Market.MarketConfig != null)
            {
                int count = Business.Market.MarketConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C12")
                    {
                        endOfDay = Business.Market.MarketConfig[i].StringValue;
                    }

                    if (Business.Market.MarketConfig[i].Code == "C26")
                    {
                        if (Business.Market.MarketConfig[i].BoolValue == 1)
                            IsReportWeekend = true;
                    }

                    if (Business.Market.MarketConfig[i].Code == "C35")
                    {
                        pathStatement = Business.Market.MarketConfig[i].StringValue;
                    }
                }
            }

            if (DateTime.Now.Hour < 10)
                timeCurrent = timeCurrent.AddDays(-1);

            timeEndDay = new DateTime(timeCurrent.Year, timeCurrent.Month, timeCurrent.Day, 23, 59, 59);
            timeStartDay = new DateTime(timeCurrent.Year, timeCurrent.Month, 1, 00, 00, 00);

            #region COMMEND CODE BECAUSE DON'T NEED CHECK END OF DAY ALLWAY LAST DAY OF MONTH(03/05/2013->DD/MM/YYYY)
            //switch (endOfDay)
            //{
            //    case "Last day of month":
            //        {
            //            if (DateTime.Now.Hour < 10)
            //                timeCurrent = timeCurrent.AddDays(-1);

            //            timeEndDay = new DateTime(timeCurrent.Year, timeCurrent.Month, timeCurrent.Day, 23, 59, 59);
            //            timeStartDay = new DateTime(timeCurrent.Year, timeCurrent.Month, 1, 00, 00, 00);
            //        }
            //        break;
            //    case "First day of month":
            //        {
            //            if (DateTime.Now.Hour > 10)
            //                timeCurrent = timeCurrent.AddDays(1);

            //            timeCurrent = timeCurrent.AddMonths(-1);

            //            timeEndDay = new DateTime(timeCurrent.Year, timeCurrent.Month, timeCurrent.Day, 0, 0, 0);
            //            timeStartDay = new DateTime(timeCurrent.Year, timeCurrent.Month, 1, 00, 00, 00);
            //        }
            //        break;
            //}
            #endregion
            
            #endregion

            #region CHECK Generate statements at weekends
            if (timeStartDay.DayOfWeek == DayOfWeek.Sunday || timeStartDay.DayOfWeek == DayOfWeek.Saturday)
            {
                if (!IsReportWeekend)
                    return;
            }
            #endregion

            #region SEND MAIL TO INVESTOR
            if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    List<Business.OpenTrade> listOpenTrade = new List<OpenTrade>();
                    List<Business.OpenTrade> listPendingOrder = new List<OpenTrade>();
                    List<Business.OpenTrade> listCommandHistory = new List<OpenTrade>();

                    #region GET ONLINE COMMAND OF INVESTOR
                    if (Business.Market.CommandExecutor != null)
                    {
                        int countCommand = Business.Market.CommandExecutor.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.CommandExecutor[j].Investor.InvestorID == Business.Market.InvestorList[i].InvestorID)
                            {
                                bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.CommandExecutor[j].Type.ID);
                                if (!isPending)
                                {
                                    double tempProfit = Math.Round(Business.Market.CommandExecutor[j].Profit, 2);
                                    Business.Market.CommandExecutor[j].Profit = tempProfit;
                                    listOpenTrade.Add(Business.Market.CommandExecutor[j]);
                                }
                                else
                                {
                                    double tempProfit = Math.Round(Business.Market.CommandExecutor[j].Profit, 2);
                                    Business.Market.CommandExecutor[j].Profit = tempProfit;

                                    listPendingOrder.Add(Business.Market.CommandExecutor[j]);
                                }
                            }
                        }
                    }
                    #endregion

                    #region GET COMMAND HISTORY OF INVESTOR
                    listCommandHistory = TradingServer.Facade.FacadeGetCommandHistoryWithTime(Business.Market.InvestorList[i].InvestorID, timeStartDay, timeEndDay);
                    #endregion

                    Model.MailConfig newMailConfig = new Model.MailConfig();
                    newMailConfig = this.GetMailConfig(Business.Market.InvestorList[i]);

                    #region SEND REPORT IF COMMAND > 0
                    if (listOpenTrade != null && listOpenTrade.Count > 0 ||
                       listPendingOrder != null && listPendingOrder.Count > 0 ||
                       listCommandHistory != null && listCommandHistory.Count > 0)
                    {
                        StringBuilder content = new StringBuilder();
                        Business.StatementTemplate newTemplate = new StatementTemplate();

                        content = newTemplate.GetStatementReport(listOpenTrade, listCommandHistory, listPendingOrder, Business.Market.InvestorList[i], timeEndDay);

                        content.Replace("[ReportDay]", timeStartDay.ToShortDateString());

                        string tempContent = content.ToString();

                        #region CHECK SCALPER INVESTOR
                        bool isScalper = this.CheckScalperInvestor(listCommandHistory);
                        #endregion

                        if (Business.Market.InvestorList[i].SendReport)
                        {
                            if (newMailConfig.isEnable)
                            {
                                if (!string.IsNullOrEmpty(newMailConfig.SmtpHost) && !string.IsNullOrEmpty(newMailConfig.MessageFrom) &&
                                        !string.IsNullOrEmpty(newMailConfig.PasswordCredential))
                                {
                                    if (!string.IsNullOrEmpty(Business.Market.InvestorList[i].Email))
                                    {
                                        content = content.Replace("[ScalperName]", "");
                                        content = content.Replace("[Scalper]", "");

                                        Model.TradingCalculate.Instance.SendMailMonthAsync(Business.Market.InvestorList[i].Email, "Monthly Statement", content.ToString(), newMailConfig);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(pathStatement) && pathStatement != "NaN")
                        {
                            //STREAM FILE SAVE CONTENT SEND MAIL
                            TradingServer.Model.TradingCalculate.Instance.StreamMonthFile(content.ToString(), Business.Market.InvestorList[i].Code, timeStartDay, 0, pathStatement, isScalper);
                        }

                        if (!string.IsNullOrEmpty(content.ToString()))
                        {
                            Business.Statement newStatement = new Statement();
                            newStatement.Content = content.ToString();
                            newStatement.Email = Business.Market.InvestorList[i].Email;
                            newStatement.InvestorCode = Business.Market.InvestorList[i].Code;
                            newStatement.StatementType = 2;
                            newStatement.TimeStatement = timeStartDay;

                            Business.Market.ListStatement.Add(newStatement);
                        }
                    }
                    #endregion
                }
            }
            #endregion      
     
            if (!string.IsNullOrEmpty(pathStatement) && pathStatement != "NaN")
            {
                TradingServer.Model.TradingCalculate.Instance.StreamMonthFile(Business.Market.LogContentSendMailMonth, "", timeStartDay, 1, pathStatement);
                string folder = timeStartDay.Year.ToString() + timeStartDay.Month.ToString();
                TradingServer.Model.TradingCalculate.Instance.ZipFolder(pathStatement + @"\" + folder, folder + ".zip", pathStatement + @"\" + folder + ".zip");
            }
        }

        /// <summary>
        /// EVENT DAY CHECK SETTING ORDER IN ADMIN
        /// </summary>
        /// <param name="TargetName"></param>
        internal void ProcessSettingOrder(string TargetName, Business.TimeEvent timeEvent)
        {
            return;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                {
                    #region Find Setting Order Of Symbol
                    string MethodOrders = string.Empty;
                    if (Business.Market.SymbolList[i].ParameterItems != null)
                    {
                        int countParameter = Business.Market.SymbolList[i].ParameterItems.Count;
                        for (int n = 0; n < countParameter; n++)
                        {
                            if (Business.Market.SymbolList[i].ParameterItems[n].Code == "S012")
                            {
                                MethodOrders = Business.Market.SymbolList[i].ParameterItems[n].StringValue;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region FIND COMMAND IN SYMBOL LIST
                    if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                    {                     
                        for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                        {
                            bool flagResetSLTP = false;
                            switch (MethodOrders)
                            {
                                #region GOOD TILL TODAY INCLUDING SL/TP
                                case "Good till today including SL/TP":
                                    {
                                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.SymbolList[i].CommandList[j].Type.ID);
                                        if (!isPending)
                                        {
                                            string content = string.Empty;
                                            string strBeforeStopLoss = string.Empty;
                                            string strBeforeTakeProfit = string.Empty;
                                            string strSize = string.Empty;

                                            #region RESET STOP LOSS AND TAKE PROFIT OF ONLINE COMMAND
                                            if (Business.Market.SymbolList[i].CommandList[j].StopLoss > 0 ||
                                              Business.Market.SymbolList[i].CommandList[j].TakeProfit > 0)
                                            {
                                                strBeforeStopLoss = Business.Market.SymbolList[i].CommandList[j].StopLoss.ToString();
                                                strBeforeTakeProfit = Business.Market.SymbolList[i].CommandList[j].TakeProfit.ToString();
                                                strSize = Business.Market.SymbolList[i].CommandList[j].Size.ToString();

                                                Business.Market.SymbolList[i].CommandList[j].StopLoss = 0;
                                                Business.Market.SymbolList[i].CommandList[j].TakeProfit = 0;
                                                flagResetSLTP = true;
                                            }
                                            #endregion

                                            if (flagResetSLTP)
                                            {
                                                #region UPDATE STOP LOST AND TAKE PROFIT OF COMMAND IN COMMAND EXECUTOR
                                                //UPDATE STOP LOST AND TAKE PROFIT OF COMMAND IN COMMAND EXECUTOR
                                                if (Business.Market.CommandExecutor != null && Business.Market.CommandExecutor.Count > 0)
                                                {
                                                    for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                    {
                                                        if (Business.Market.CommandExecutor[n].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                                        {
                                                            if (Business.Market.CommandExecutor[n].StopLoss > 0 || Business.Market.CommandExecutor[n].TakeProfit > 0)
                                                            {
                                                                Business.Market.CommandExecutor[n].StopLoss = 0;
                                                                Business.Market.CommandExecutor[n].TakeProfit = 0;
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region UPDATE STOP LOSS AND TAKE PROFIT OF COMMAND IN INVESTOR LIST
                                                //if (Business.Market.SymbolList[i].CommandList[j].Investor.CommandList != null)
                                                //{
                                                //    int countInvestorCommand = Business.Market.SymbolList[i].CommandList[j].Investor.CommandList.Count;
                                                //    for (int n = 0; n < countInvestorCommand; n++)
                                                //    {
                                                //        if (Business.Market.SymbolList[i].CommandList[j].Investor.CommandList[n].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                                //        {
                                                //            if (Business.Market.SymbolList[i].CommandList[j].Investor.CommandList[n].StopLoss > 0 ||
                                                //                Business.Market.SymbolList[i].CommandList[j].Investor.CommandList[n].TakeProfit > 0)
                                                //            {
                                                //                Business.Market.SymbolList[i].CommandList[j].Investor.CommandList[n].StopLoss = 0;
                                                //                Business.Market.SymbolList[i].CommandList[j].Investor.CommandList[n].TakeProfit = 0;
                                                //            }

                                                //            break;
                                                //        }
                                                //    }
                                                //}

                                                if (Business.Market.InvestorList != null)
                                                {
                                                    for (int n = 0; n < Business.Market.InvestorList.Count; n++)
                                                    {
                                                        if (Business.Market.InvestorList[n].InvestorID == Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID)
                                                        {
                                                            if (Business.Market.InvestorList[n].CommandList != null)
                                                            {
                                                                for (int m = 0; m < Business.Market.InvestorList[n].CommandList.Count; m++)
                                                                {
                                                                    if (Business.Market.InvestorList[n].CommandList[m].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                                                    {
                                                                        if (Business.Market.InvestorList[n].CommandList[m].StopLoss > 0 || Business.Market.InvestorList[n].CommandList[m].TakeProfit > 0)
                                                                        {
                                                                            Business.Market.InvestorList[n].CommandList[m].StopLoss = 0;
                                                                            Business.Market.InvestorList[n].CommandList[m].TakeProfit = 0;
                                                                        }

                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                //SENT NOTIFY TO CLIENT
                                                string message = "STO8546";

                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                                //SEND NOTIFY TO CLIENT UPDATE STOP LOSS AND TAKE PROFIT
                                                TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Business.Market.SymbolList[i].CommandList[j]);

                                                //UPDATE STOP LOST AND TAKE PROFIT OF COMMAND IN DATABASE
                                                TradingServer.Facade.FacadeUpdateOnlineCommandWithTakeProfit(0, 0, Business.Market.SymbolList[i].CommandList[j].ID,
                                                    Business.Market.SymbolList[i].CommandList[j].Comment, Business.Market.SymbolList[i].CommandList[j].OpenPrice);

                                                //INSERT SYSTEM LOG

                                                //'00001140: calculation swap order #004432443 - current balance: 40000 - old swap: 5.00'
                                                content = "'System': reset order #" +
                                                    Business.Market.SymbolList[i].CommandList[j].CommandCode + " name: " + Business.Market.CommandExecutor[i].Symbol.Name +
                                                    " size: " + strSize + " s/l: " + strBeforeStopLoss + " t/p: " + strBeforeTakeProfit + " -> s/l: 0.00 t/p: 0.00 [Good till today including SL/TP]";

                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[Good till today including SL/TP]", "", "");
                                            }
                                        }
                                        else
                                        {
                                            string content = string.Empty;
                                            #region REMOVE PENDING ORDER OF INVESTOR

                                            #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND INVESTOR LIST
                                            ////REMOVE COMMAND IN COMMAND EXECUTOR
                                            //if (Business.Market.CommandExecutor != null)
                                            //{
                                            //    int countCommand = Business.Market.CommandExecutor.Count;
                                            //    for (int n = 0; n < countCommand; n++)
                                            //    {
                                            //        if (Business.Market.CommandExecutor[n].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                            //        {
                                            //            Business.Market.CommandExecutor.Remove(Business.Market.CommandExecutor[n]);
                                            //            break;
                                            //        }
                                            //    }
                                            //}

                                            ////DELETE COMMAND IN INVESTOR COMMAND LIST
                                            //TradingServer.Facade.FacadeRemoveOpenTradeInInvestorList(Business.Market.SymbolList[i].CommandList[j].ID,
                                            //    Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID);
                                            #endregion                                            

                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                            newOpenRemove.InvestorID = Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID;
                                            newOpenRemove.IsExecutor = true;
                                            newOpenRemove.IsInvestor = true;
                                            newOpenRemove.IsSymbol = false;
                                            newOpenRemove.OpenTradeID = Business.Market.SymbolList[i].CommandList[j].ID;
                                            newOpenRemove.SymbolName = Business.Market.SymbolList[i].Name;
                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                            //DELETE COMMAND IN DATABASE
                                            TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.SymbolList[i].CommandList[j].ID);

                                            //INSERT DATABASE THEN CANCEL PENDING ORDER
                                            TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                                Business.Market.SymbolList[i].CommandList[j].Type.ID, Business.Market.SymbolList[i].CommandList[j].CommandCode,
                                                Business.Market.SymbolList[i].CommandList[j].OpenTime, Business.Market.SymbolList[i].CommandList[j].OpenPrice,
                                                DateTime.Now, 0, 0, 0, 0, Business.Market.SymbolList[i].CommandList[j].ExpTime, Business.Market.SymbolList[i].CommandList[j].Size,
                                                Business.Market.SymbolList[i].CommandList[j].StopLoss, Business.Market.SymbolList[i].CommandList[j].TakeProfit,
                                                Business.Market.SymbolList[i].CommandList[j].ClientCode, Business.Market.SymbolList[i].SymbolID,
                                                Business.Market.SymbolList[i].CommandList[j].Taxes, 0, Business.Market.SymbolList[i].CommandList[j].Comment, "8",
                                                Business.Market.SymbolList[i].CommandList[j].TotalSwap,
                                                Business.Market.SymbolList[i].CommandList[j].RefCommandID,
                                                Business.Market.SymbolList[i].CommandList[j].AgentRefConfig,
                                                Business.Market.SymbolList[i].CommandList[j].IsActivePending,
                                                Business.Market.SymbolList[i].CommandList[j].IsStopLossAndTakeProfit);

                                            //SENT NOTIFY TO CLIENT
                                            string message = "STO8546";
                                            if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                            Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                            //NOTIFY TO MANAGER DELETE PENDING ORDER COMMAND
                                            TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.SymbolList[i].CommandList[j]);

                                            //INSERT SYSTEM LOG
                                            //'1205': delete order #00275799 sell stop 0.10XAUUSD at 1401.30
                                            content = "'System': delete order #" + Business.Market.SymbolList[i].CommandList[j].CommandCode + " " +
                                                Business.Market.SymbolList[i].CommandList[j].Type.Name + " " + Model.TradingCalculate.Instance.BuildStringWithDigit(
                                                Business.Market.SymbolList[i].CommandList[j].Size.ToString(), 2) + " " + Business.Market.SymbolList[i].CommandList[j].Symbol.Name + " at " +
                                                Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(),
                                                Business.Market.SymbolList[i].CommandList[j].Symbol.Digit) + " [Good till today including SL/TP]";

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[Good till today including SL/TP]", "", "");

                                            //REMOVE COMMAND IN SYMBOL LIST
                                            Business.Market.SymbolList[i].CommandList.RemoveAt(j);

                                            j--;
                                            #endregion                                                                                    
                                        }
                                    }
                                    break;
                                #endregion

                                #region GOOL TILL TODAY EXCLUDING SL/TP
                                case "Good till today excluding SL/TP":
                                    {
                                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.SymbolList[i].CommandList[j].Type.ID);
                                        if (isPending)
                                        {
                                            string content = string.Empty;
                                            #region REMOVE PENDING ORDER OF INVESTOR

                                            #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND INVESTOR LIST
                                            ////REMOVE COMMAND IN COMMAND EXECUTOR
                                            //if (Business.Market.CommandExecutor != null)
                                            //{
                                            //    int countCommand = Business.Market.CommandExecutor.Count;
                                            //    for (int n = 0; n < countCommand; n++)
                                            //    {
                                            //        if (Business.Market.CommandExecutor[n].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                            //        {
                                            //            Business.Market.CommandExecutor.Remove(Business.Market.CommandExecutor[n]);
                                            //            break;
                                            //        }
                                            //    }
                                            //}

                                            ////bool deleteCommandExe = TradingServer.Facade.FacadeRemoveOpenTradeInCommandExecutor(Business.Market.SymbolList[i].CommandList[j].ID);

                                            ////DELETE COMMAND IN INVESTOR COMMAND LIST
                                            //TradingServer.Facade.FacadeRemoveOpenTradeInInvestorList(Business.Market.SymbolList[i].CommandList[j].ID,
                                            //    Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID);
                                            #endregion
                                            
                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                            newOpenRemove.InvestorID = Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID;
                                            newOpenRemove.IsExecutor = true;
                                            newOpenRemove.IsInvestor = true;
                                            newOpenRemove.IsSymbol = false;
                                            newOpenRemove.OpenTradeID = Business.Market.SymbolList[i].CommandList[j].ID;
                                            newOpenRemove.SymbolName = Business.Market.SymbolList[i].Name;
                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                            //DELETE COMMAND IN DATABASE
                                            TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.SymbolList[i].CommandList[j].ID);

                                            //INSERT DATABASE THEN CANCEL PENDING ORDER
                                            TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                                Business.Market.SymbolList[i].CommandList[j].Type.ID, Business.Market.SymbolList[i].CommandList[j].CommandCode,
                                                Business.Market.SymbolList[i].CommandList[j].OpenTime, Business.Market.SymbolList[i].CommandList[j].OpenPrice,
                                                DateTime.Now, 0, 0, 0, 0, Business.Market.SymbolList[i].CommandList[j].ExpTime, Business.Market.SymbolList[i].CommandList[j].Size,
                                                Business.Market.SymbolList[i].CommandList[j].StopLoss, Business.Market.SymbolList[i].CommandList[j].TakeProfit,
                                                Business.Market.SymbolList[i].CommandList[j].ClientCode, Business.Market.SymbolList[i].SymbolID,
                                                Business.Market.SymbolList[i].CommandList[j].Taxes, 0, Business.Market.SymbolList[i].CommandList[j].Comment, "11",
                                                Business.Market.SymbolList[i].CommandList[j].TotalSwap,
                                                Business.Market.SymbolList[i].CommandList[j].RefCommandID,
                                                Business.Market.SymbolList[i].CommandList[j].AgentRefConfig,
                                                Business.Market.SymbolList[i].CommandList[j].IsActivePending,
                                                Business.Market.SymbolList[i].CommandList[j].IsStopLossAndTakeProfit);

                                            //SENT NOTIFY TO CLIENT
                                            string message = "STO8546";
                                            if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                            Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                            //SEND NOTIFY DELETE PENDING ORDER TO MANAGER
                                            TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.SymbolList[i].CommandList[j]);

                                            //INSERT SYSTEM LOG
                                            //'1205': delete order #00275799 sell stop 0.10XAUUSD at 1401.30
                                            content = "'System': delete order #" + Business.Market.SymbolList[i].CommandList[j].CommandCode + " " +
                                                Business.Market.SymbolList[i].CommandList[j].Type.Name + " " + Model.TradingCalculate.Instance.BuildStringWithDigit(
                                                Business.Market.SymbolList[i].CommandList[j].Size.ToString(), 2) + " " + Business.Market.SymbolList[i].CommandList[j].Symbol.Name + " at " +
                                                Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(),
                                                Business.Market.SymbolList[i].CommandList[j].Symbol.Digit) + " [Good till today excluding SL/TP]";

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[Good till today excluding SL/TP]", "", "");

                                            //REMOVE COMMAND IN SYMBOL LIST
                                            Business.Market.SymbolList[i].CommandList.RemoveAt(j);

                                            j--;
                                            #endregion                                            
                                        }
                                    }
                                    break;
                                #endregion
                            }
                        }
                    }
                    #endregion    
                }
            }                      
        }

        /// <summary>
        /// 
        /// </summary>
        private void CleanDemoAccount(int dayDemo)
        {
            return;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    TimeSpan timeDemo = DateTime.Now - Business.Market.InvestorList[i].RegisterDay;
                    if (timeDemo.TotalDays >= 14)
                    {
                        if (Business.Market.InvestorList[i].InvestorGroupInstance.Name.IndexOf("Demo") != -1)
                        {
                            if (Business.Market.InvestorList[i].CommandList != null)
                            {
                                int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                                for (int j = countCommand - 1; j >= 0; j--)
                                {
                                    int commandID = Business.Market.InvestorList[i].CommandList[j].ID;

                                    #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                                    //TradingServer.Facade.FacadeRemoveOpenTradeInCommandExecutor(commandID);
                                    //TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(commandID);
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

                                    lock (Business.Market.syncObject)
                                    {
                                        Business.Market.InvestorList[i].CommandList.RemoveAt(j);
                                    }
                                }
                            }

                            Business.Market.InvestorList[i].IsDisable = true;
                            TradingServer.Facade.FacadeUpdateInvestor(Business.Market.InvestorList[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetName"></param>
        /// <param name="timeEvent"></param>
        private void EventGetCandlesByDate(string TargetName, Business.TimeEvent timeEvent)
        {
            string symbols = string.Empty;
            if (Business.Market.Symbols != null)
            {
                foreach (KeyValuePair<string, Business.Symbol> sm in Business.Market.Symbols)
                {
                    symbols += sm.Key + "{";
                }
            }

            if (symbols.EndsWith("{"))
                symbols = symbols.Remove(symbols.Length - 1, 1);

            DateTime timeCandles = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
            DateTime timeCandlesOne = timeCandles.AddDays(-1);
            DateTime timeCandlesFive = timeCandles.AddDays(-5);

            string cmd = "GetCandlesByDate$" + symbols + "|" + timeCandles;
            string resultAPI = Business.Market.InstanceSocket.SendSocket(cmd);
            Business.Market.CandlesByDate = Model.BuildCommand.Instance.MapStringToDicCandle(resultAPI);

            string cmdOneDay = "GetCandlesByDate$" + symbols + "|" + timeCandlesOne;
            string OneDay = Business.Market.InstanceSocket.SendSocket(cmdOneDay);
            Business.Market.CandlesByDateOneDay = Model.BuildCommand.Instance.MapStringToDicCandle(OneDay);

            string cmdFiveDay = "GetCandlesByDate$" + symbols + "|" + timeCandlesFive;
            string FiveDay = Business.Market.InstanceSocket.SendSocket(cmdFiveDay);
            Business.Market.CandlesByDateFiveDay = Model.BuildCommand.Instance.MapStringToDicCandle(FiveDay);
        }
    }
}
