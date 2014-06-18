﻿using System;
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
        /// <param name="OpenTrade"></param>
        internal void UpdateCommand(Business.OpenTrade OpenTrade)
        {
            this.TaskWork = this.ProcessSetTassMT4;
            this.Comment = "UpdateCommand " + this.InvestorID;
            this.TaskName = "UpdateCommand " + this.InvestorID;
            //this.IsActive = false;

            if (this.IsActive == true)
            {
                if (this.isInTask)
                {
                    this.UpdateCommands.Add(OpenTrade);
                }
                else
                {
                    this.IsActive = false;
                    this.UpdateCommands.Add(OpenTrade);
                    Business.CalculatorFacade.SetTask(this);
                }
            }
            else
            {
                this.UpdateCommands.Add(OpenTrade);
                Business.CalculatorFacade.SetTask(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        private void ProcessSetTask()
        {            
            Investor.IsInProcess = true;
            this.isInTask = true;

            while (this.UpdateCommands.Count > 0)
            {
                #region WHILE
                try
                {
                    DateTime timeStart = DateTime.Now;

                    NumCheck++;
                    if (NumCheck == 100)
                        NumCheck = 0;

                    Business.OpenTrade Command = this.UpdateCommands[0];

                    this.UpdateCommands.RemoveAt(0);

                    if (Command == null)
                        continue;

                    bool IsBuy = false;
                    if (Command.Type.ID == 1 || Command.Type.ID == 7 || Command.Type.ID == 9 || Command.Type.ID == 11 || Command.Type.ID == 17 || Command.Type.ID == 19)
                        IsBuy = true;

                    #region PROCESS MARKET AREA
                    if (Command.Symbol.MarketAreaRef.IMarketAreaName.Trim() == "SpotCommand")
                    {
                        #region Spot Command
                        if (this.CommandList != null)
                        {
                            if (Command.IsClose && Command.IsMultiClose)
                            {
                                this.MultiCloseCommand(Command);
                            }
                            else
                            {
                                #region For Command List
                                bool FlagCommand = false;
                                for (int i = 0; i < this.CommandList.Count; i++)
                                {
                                    if (this.CommandList[i].ID == Command.ID)
                                    {
                                        if (Command.IsClose == true)
                                        {
                                            int commandRefID = this.CommandList[i].ID;
                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);

                                            //if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10)
                                            if(isPending)
                                            {
                                                #region CLOSE PENDING ORDER
                                                //ADD PENDING ORDER TO DATABASE
                                                //int addHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Command.Investor.InvestorID, Command.Type.ID, Command.CommandCode,
                                                //    Command.OpenTime, Command.OpenPrice, Command.CloseTime, Command.ClosePrice, 0, 0, 0, Command.ExpTime, Command.Size, Command.StopLoss,
                                                //    Command.TakeProfit, Command.ClientCode, Command.Symbol.SymbolID, Command.Taxes, Command.AgentCommission, Command.Comment, "3",
                                                //    Command.TotalSwap, Command.RefCommandID, Command.AgentRefConfig, Command.IsActivePending, Command.IsStopLossAndTakeProfit);

                                                //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                Business.OpenRemove newOpenRemove = new OpenRemove();
                                                newOpenRemove.InvestorID = this.InvestorID;
                                                newOpenRemove.OpenTradeID = this.CommandList[i].ID;
                                                newOpenRemove.SymbolName = this.CommandList[i].Symbol.Name;
                                                newOpenRemove.IsExecutor = true;
                                                newOpenRemove.IsSymbol = true;
                                                newOpenRemove.IsInvestor = false;
                                                Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                //Delete Command In Database                             
                                                //bool deleteDB = TradingServer.Facade.FacadeDeleteOpenTradeByID(Command.ID);

                                                //Close Command Complete Add Message To Client
                                                if (this.ClientCommandQueue == null)
                                                    this.ClientCommandQueue = new List<string>();

                                                #region MAP STRING SEND TO CLIENT
                                                string Message = string.Empty;
                                                if (Command.IsServer)
                                                {
                                                    Message = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.CloseTime;

                                                    string msg = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close";

                                                    string msgNotify = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.AgentRefConfig + "," + Command.SpreaDifferenceInOpenTrade;

                                                    //SEND COMMAND TO AGENT SERVER
                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = msgNotify;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Command.Investor.InvestorGroupInstance);
                                                }
                                                else
                                                {
                                                    Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.CloseTime;

                                                    string msg = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close";

                                                    string msgNotify = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                         Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                         Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                         1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                         ",Close," + Command.AgentRefConfig + "," + Command.SpreaDifferenceInOpenTrade;

                                                    //SEND COMMAND TO AGENT SERVER
                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = msgNotify;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Command.Investor.InvestorGroupInstance);
                                                }

                                                this.ClientCommandQueue.Add(Message);
                                                #endregion

                                                #region INSTER SYSTEM LOG(EVENT DELETE PENDING ORDER)
                                                string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(this.CommandList[i].Type.ID).ToLower();
                                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Size.ToString(), 2);
                                                string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);

                                                string content = "'" + this.CommandList[i].Investor.Code + "': delete order #" + this.CommandList[i].CommandCode + " " + mode + " " +
                                                    size + this.CommandList[i].Symbol.Name + " at " + openPrice;

                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[delete order]", Command.Investor.IpAddress, Command.Investor.Code);
                                                #endregion

                                                lock (Business.Market.syncObject)
                                                {
                                                    //Delete Command In Investor
                                                    bool deleteCommandInvestor = this.CommandList.Remove(this.CommandList[i]);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region CLOSE SPOT COMMAND
                                                #region GET STEP LOTS 
                                                double step = -1;
                                                int strStep = -1;
                                                if (Command.IGroupSecurity != null)
                                                {
                                                    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
                                                    {
                                                        int count = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                                                        for (int j = 0; j < count; j++)
                                                        {
                                                            if (Command.IGroupSecurity.IGroupSecurityConfig[j].Code == "B13")
                                                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[j].NumValue, out step);
                                                        }
                                                    }
                                                }

                                                if (step > 0)
                                                {
                                                    string[] temp = step.ToString().Split('.');
                                                    if (temp.Length > 1)
                                                    {
                                                        strStep = temp[1].Length;
                                                    }
                                                }
                                                #endregion

                                                double tempSize = -1;
                                                if (strStep > 0)
                                                    tempSize = Math.Round((this.CommandList[i].Size - Command.Size), strStep);
                                                else
                                                    tempSize = Math.Round((this.CommandList[i].Size - Command.Size), 2);

                                                if (tempSize > 0)
                                                    Command.Commission = Model.CalculationFormular.Instance.CalculationCommission(Command);

                                                #region CALCLUATION COMMISSION FOR AGENT(COMMENT 03-02-2012)
                                                //double commissionAgent = Model.CalculationFormular.Instance.CalculationAgentCommission(Command);

                                                ////set Agent Commission To Open Trade
                                                //Command.AgentCommission = commissionAgent;

                                                //Business.Investor newAgent = TradingServer.Facade.FacadeSelectInvestorByCode(Command.Investor.AgentID);

                                                //if (newAgent.InvestorID > 0)
                                                //{
                                                //    //Update balance of agent                
                                                //    double BalanceAgent = newAgent.Balance + commissionAgent;
                                                //    TradingServer.Facade.FacadeUpdateBalance(newAgent.InvestorID, BalanceAgent);

                                                //    //search in investor online if agent online then send message get balance of agent 
                                                //    if (Business.Market.InvestorList != null)
                                                //    {
                                                //        int countInvestorOnline = Business.Market.InvestorList.Count;
                                                //        for (int m = 0; m < countInvestorOnline; m++)
                                                //        {
                                                //            if (Business.Market.InvestorList[m].InvestorID == newAgent.InvestorID)
                                                //            {
                                                //                if (Business.Market.InvestorList[m].IsOnline)
                                                //                {
                                                //                    string Message = "GetNewBalance";
                                                //                    Business.Market.InvestorList[m].ClientCommandQueue.Add(Message);
                                                //                }

                                                //                Business.Market.InvestorList[m].Balance += commissionAgent;

                                                //                //SEND NOTIFY TO MANAGER with type =3 then balance and credit
                                                //                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[m]);

                                                //                break;
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    Command.AgentCommission = 0;
                                                //}
                                                #endregion

                                                #region CALCULATION COMMISSION FOR AGENT
                                                double commissionAgent = Model.CalculationFormular.Instance.CalculationAgentCommission(Command);

                                                //search in investor online if agent online then send message get balance of agent 
                                                if (Business.Market.InvestorList != null)
                                                {
                                                    int countInvestorOnline = Business.Market.InvestorList.Count;
                                                    for (int m = 0; m < countInvestorOnline; m++)
                                                    {
                                                        if (Business.Market.InvestorList[m].Code == Command.Investor.AgentID)
                                                        {
                                                            double BalanceAgent = Business.Market.InvestorList[m].Balance + commissionAgent;

                                                            Business.Market.InvestorList[m].Balance += commissionAgent;

                                                            //Update balance of agent                                                                            
                                                            TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[m].InvestorID, BalanceAgent);

                                                            //set Agent Commission To Open Trade
                                                            Command.AgentCommission = commissionAgent;

                                                            //SEND NOTIFY TO MANAGER with type =3 then balance and credit
                                                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[m]);

                                                            if (Business.Market.InvestorList[m].IsOnline)
                                                            {
                                                                string Message = "GetNewBalance";

                                                                Business.Market.InvestorList[m].ClientCommandQueue.Add(Message);
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion                                                

                                                int commandId = Command.ID;

                                                int ResultHistory = -1;
                                                //Add Command To Command History
                                                ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Command.Investor.InvestorID, Command.Type.ID, Command.CommandCode, Command.OpenTime,
                                                    Command.OpenPrice, Command.CloseTime, Command.ClosePrice, Command.Profit, this.CommandList[i].Swap, Command.Commission, Command.ExpTime,
                                                    Command.Size, Command.StopLoss, Command.TakeProfit, Command.ClientCode, Command.Symbol.SymbolID, Command.Taxes, Command.AgentCommission,
                                                    Command.Comment, "4", this.CommandList[i].TotalSwap, this.CommandList[i].RefCommandID,
                                                    this.CommandList[i].AgentRefConfig, this.CommandList[i].IsActivePending, this.CommandList[i].IsStopLossAndTakeProfit);

                                                //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                Business.OpenRemove newOpenRemove = new OpenRemove();
                                                newOpenRemove.InvestorID = this.InvestorID;
                                                newOpenRemove.OpenTradeID = commandId;
                                                newOpenRemove.SymbolName = this.CommandList[i].Symbol.Name;
                                                newOpenRemove.IsExecutor = true;
                                                newOpenRemove.IsSymbol = true;
                                                newOpenRemove.IsInvestor = false;
                                                Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                double totalProfit = Math.Round(Command.Profit + Command.Commission + Command.Swap, 2);
                                                //CHANGE FORMAT CALCULATION SWAP(20-02-2012)
                                                //Update Balance Of Investor Account
                                                double totalBalance = this.Balance + totalProfit;

                                                bool updateBalance = false;
                                                if (!Business.Market.IsConnectMT4)
                                                    updateBalance = this.UpdateBalance(Command.Investor.InvestorID, totalBalance);

                                                if (updateBalance)
                                                {
                                                    string strBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.Balance.ToString(), 2);
                                                    string strProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(totalProfit.ToString(), 2);
                                                    string strCommission = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Commission.ToString(), 2);
                                                    string strSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Swap.ToString(), 2);
                                                    string strTotalSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].TotalSwap.ToString(), 2);
                                                    string strTotalBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(totalBalance.ToString(), 2);

                                                    //'00001140': close command #00148535 balance : 123456 command profit : 10000
                                                    string content = "'" + this.Code + "': close command #" + this.CommandList[i].CommandCode + " balance : " + strBalance + " command profit: " + strProfit +
                                                        " Commission: " + strCommission + " Swap: " + strSwap + " Total Swap: " + strTotalSwap +
                                                        " -> total balance: " + strTotalBalance;

                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[Close Command]", "", this.Code);

                                                    if (!Business.Market.IsConnectMT4)
                                                    {
                                                        this.Balance += totalProfit;
                                                    }
                                                }

                                                //Update Command In Database                             
                                                bool deleteCommandDB = TradingServer.Facade.FacadeDeleteOpenTradeByID(Command.ID);

                                                //Close Command Complete Add Message To Client
                                                if (this.ClientCommandQueue == null)
                                                    this.ClientCommandQueue = new List<string>();

                                                string msg = string.Empty;

                                                #region Map Command Server To Client
                                                if (Command.IsServer)
                                                {
                                                    string Message = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.CloseTime;

                                                    //int countOnline = this.CountInvestorOnline(this.InvestorID);
                                                    //if (countOnline > 0)
                                                    this.ClientCommandQueue.Add(Message);

                                                    string msgNotify = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.AgentRefConfig + "," + Command.SpreaDifferenceInOpenTrade;

                                                    //SEND COMMAND TO AGENT SERVER
                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = msgNotify;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Command.Investor.InvestorGroupInstance);

                                                    msg = Message;
                                                }
                                                else
                                                {
                                                    string Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.CloseTime;

                                                    //int countOnline = this.CountInvestorOnline(this.InvestorID);
                                                    //if (countOnline > 0)
                                                    this.ClientCommandQueue.Add(Message);

                                                    string msgNotify = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                        ",Close," + Command.AgentRefConfig + "," + Command.SpreaDifferenceInOpenTrade;

                                                    //SEND COMMAND TO AGENT SERVER
                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = msgNotify;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Command.Investor.InvestorGroupInstance);

                                                    msg = Message;
                                                }
                                                #endregion

                                                #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                                string mode = "sell";
                                                if (this.CommandList[i].Type.ID == 1)
                                                    mode = "buy";

                                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                                string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                                                string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), this.CommandList[i].Symbol.Digit);
                                                string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), this.CommandList[i].Symbol.Digit);
                                                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), this.CommandList[i].Symbol.Digit);
                                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), this.CommandList[i].Symbol.Digit);
                                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), this.CommandList[i].Symbol.Digit);

                                                string contentServer = "'" + this.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                                    Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " completed";

                                                TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.Investor.IpAddress, Command.Investor.Code);
                                                #endregion

                                                lock (Business.Market.syncObject)
                                                {
                                                    //Remove Command In Investor List
                                                    //this.CommandList.Remove(this.CommandList[i]);
                                                    bool deleteCommandInvestor = this.CommandList.Remove(this.CommandList[i]);
                                                }

                                                #region RECALCULATION TOTAL MARGIN OF INVESTOR
                                                //RECACULATION TOTAL MARGIN OF INVESTOR
                                                if (this.CommandList.Count > 0)
                                                {
                                                    Business.Margin newMargin = new Margin();
                                                    newMargin = Command.Symbol.CalculationTotalMargin(this.CommandList);
                                                    this.Margin = newMargin.TotalMargin;
                                                    this.FreezeMargin = newMargin.TotalFreezeMargin;
                                                }
                                                else
                                                {
                                                    this.Margin = 0;
                                                    this.FreezeMargin = 0;
                                                }
                                                #endregion

                                                #region MAKE NEW COMMAND WITH DELTA SIZE
                                                if (!Business.Market.IsConnectMT4)
                                                {
                                                    if (tempSize > 0)
                                                    {
                                                        //SET MARGIN OF NEW COMMAND
                                                        Command.Margin = 0;

                                                        //SET NEW SIZE AND NEW PRICES
                                                        Command.Size = tempSize;
                                                        Command.Swap = 0;
                                                        Command.IsReNewOpen = true;
                                                        Command.RefCommandID = commandRefID;
                                                        switch (Command.Type.ID)
                                                        {
                                                            case 1:
                                                                {
                                                                    Command.ClosePrice = Command.Symbol.TickValue.Bid;
                                                                    Command.OpenPrice = Command.OpenPrice;
                                                                }
                                                                break;
                                                            case 2:
                                                                {
                                                                    Command.ClosePrice = Command.Symbol.TickValue.Ask;
                                                                    Command.OpenPrice = Command.OpenPrice;
                                                                }
                                                                break;
                                                        }

                                                        Command.IsServer = true;
                                                        Command.Symbol.MarketAreaRef.AddCommand(Command);
                                                    }
                                                }
                                                #endregion

                                                //SEND NOTIFY TO MANAGER THEN CLOSE COMMAND
                                                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, this);
                                                #endregion

                                                //CHECK NEW RULE CLOSE POSITION
                                                Command.CheckRuleCloseOpenPosition(this.CommandList, tempSize);
                                            }
                                        }
                                        else
                                        {
                                            #region Command Is Open
                                            //Update Command Of Investor
                                            //this.CommandList[i].ClientCode = Command.ClientCode;

                                            #region SET CLOSE PRICE FOR COMMAND
                                            if (Command.ClosePrice > 0)

                                            {
                                                this.CommandList[i].ClosePrice = Command.ClosePrice;
                                            }
                                            else
                                            {
                                                switch (this.CommandList[i].Type.ID)
                                                {
                                                    case 1:
                                                        if (this.CommandList[i].Symbol.TickValue != null && this.CommandList[i].Symbol.TickValue.Bid > 0)
                                                            this.CommandList[i].ClosePrice = this.CommandList[i].Symbol.TickValue.Bid;
                                                        break;

                                                    case 2:
                                                        if (this.CommandList[i].Symbol.TickValue != null && this.CommandList[i].Symbol.TickValue.Ask > 0)
                                                        {
                                                            double Ask = 0;
                                                            Ask = (Symbol.ConvertNumberPip(this.CommandList[i].Symbol.Digit, this.CommandList[i].SpreaDifferenceInOpenTrade) +
                                                                this.CommandList[i].Symbol.TickValue.Ask);
                                                            this.CommandList[i].ClosePrice = Ask;
                                                        }
                                                        break;
                                                }
                                            }
                                            #endregion                                            

                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(this.CommandList[i].Type.ID);
                                            if (!isPending)
                                            {
                                                this.CommandList[i].CalculatorProfitCommand(this.CommandList[i]);
                                                this.CommandList[i].Profit = this.CommandList[i].Symbol.ConvertCurrencyToUSD(this.CommandList[i].Symbol.Currency, this.CommandList[i].Profit,
                                                    false, this.CommandList[i].SpreaDifferenceInOpenTrade, this.CommandList[i].Symbol.Digit);

                                                #region UPDATE CLOSE PRICES IN COMMAND EXECUTOR
                                                //if (Business.Market.CommandExecutor != null)
                                                //{
                                                //    int countCommandExe = Business.Market.CommandExecutor.Count;
                                                //    for (int j = 0; j < countCommandExe; j++)
                                                //    {
                                                //        if (Business.Market.CommandExecutor[j].ID == Command.ID)
                                                //        {
                                                //            Business.Market.CommandExecutor[j].ClosePrice = Command.ClosePrice;
                                                //            Business.Market.CommandExecutor[j].Profit = this.CommandList[i].Profit;

                                                //            break;
                                                //        }
                                                //    }
                                                //}
                                                this.CommandList[i].InsExe.Profit = this.CommandList[i].Profit;
                                                #endregion

                                                #region UPDATE PROFIT IN SYMBOL LIST
                                                if (this.CommandList[i].Symbol != null && this.CommandList[i].Symbol.CommandList != null)
                                                {
                                                    int countCommand = this.CommandList[i].Symbol.CommandList.Count;
                                                    for (int j = 0; j < countCommand; j++)
                                                    {
                                                        if (this.CommandList[i].Symbol.CommandList[j].ID == Command.ID)
                                                        {
                                                            this.CommandList[i].Symbol.CommandList[j].Profit = this.CommandList[i].Profit;                                                            

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region UPDATE PENDING ORDER(PENDING ORDER ACTIVE)
                                                if (this.CommandList[i].Type.ID != Command.Type.ID)
                                                {
                                                    if (this.CommandList[i].Type.ID > Command.Type.ID)
                                                    {
                                                        //CHECK VALID ACCOUNT IF TRUE THEN UPDATE TYPE ELSE
                                                        //THEN CANCEL PENDING ORDER AND INSERT DATABASE
                                                        bool checkValidAccount = Command.CheckValidAccountInvestor(Command);

                                                        if (checkValidAccount)
                                                        {
                                                            //SET OPEN TIME FOR COMMAND
                                                            this.CommandList[i].OpenTime = DateTime.Now;
                                                            Command.OpenTime = DateTime.Now;
                                                            this.CommandList[i].OpenPrice = Command.OpenPrice;
                                                            this.CommandList[i].IsActivePending = false;
                                                            this.CommandList[i].IsStopLossAndTakeProfit = false;

                                                            //CALCULATION COMMISSION(COMMENT BECAUSE DON"T NEED, COMMISSION GET FROM MT4)
                                                            //double commission = Model.CalculationFormular.Instance.CalculationCommission(Command);
                                                            //Command.Commission = commission;

                                                            #region CALCULATION AGENT COMMISSION
                                                            //CALCULATION AGENT COMMISSION
                                                            double agentCommission = Model.CalculationFormular.Instance.CalculationAgentCommission(Command);

                                                            Business.Investor newAgent = TradingServer.Facade.FacadeSelectInvestorByCode(Command.Investor.AgentID);

                                                            //set commission and agent commission to command list in investor
                                                            this.CommandList[i].AgentCommission = agentCommission;
                                                            //this.CommandList[i].Commission = commission;
                                                            this.CommandList[i].Commission = Command.Commission;

                                                            #region FIND AGENT AND REQUEST BALANCE IF AGENT ONLINE
                                                            if (newAgent != null && newAgent.InvestorID > 0)
                                                            {
                                                                //Update balance of agent                
                                                                double BalanceAgent = newAgent.Balance + agentCommission;
                                                                TradingServer.Facade.FacadeUpdateBalance(newAgent.InvestorID, BalanceAgent);

                                                                //search in investor online if agent online then send message get balance of agent 
                                                                if (Business.Market.InvestorList != null)
                                                                {
                                                                    int countInvestorOnline = Business.Market.InvestorList.Count;
                                                                    for (int m = 0; m < countInvestorOnline; m++)
                                                                    {
                                                                        if (Business.Market.InvestorList[m].InvestorID == newAgent.InvestorID)
                                                                        {
                                                                            if (Business.Market.InvestorList[m].IsOnline)
                                                                            {
                                                                                Business.Market.InvestorList[m].Balance += agentCommission;
                                                                                string messageAgent = "GetNewBalance";

                                                                                Business.Market.InvestorList[m].ClientCommandQueue.Add(messageAgent);

                                                                                //SEND NOTIFY TO MANAGER
                                                                                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[m]);
                                                                            }

                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion
                                                            #endregion

                                                            this.CommandList[i].Type = Command.Type;

                                                            //CALCULATION MARGIN FOR COMMAND PENDING ORDER
                                                            this.CommandList[i].CalculatorMarginCommand(this.CommandList[i]);

                                                            //SET MARGIN FOR COMMAND
                                                            Command.Margin = this.CommandList[i].Margin;

                                                            #region FIND COMMAND IN COMMAND EXECUTOR AND UPDATE TYPE
                                                            if (Business.Market.CommandExecutor != null)
                                                            {
                                                                int countExe = Business.Market.CommandExecutor.Count;
                                                                for (int m = 0; m < countExe; m++)
                                                                {
                                                                    if (Business.Market.CommandExecutor[m].ID == Command.ID)
                                                                    {
                                                                        Business.Market.CommandExecutor[m].Type = Command.Type;
                                                                        Business.Market.CommandExecutor[m].Commission = Command.Commission;
                                                                        Business.Market.CommandExecutor[m].AgentCommission = agentCommission;
                                                                        Business.Market.CommandExecutor[m].Margin = Command.Margin;
                                                                        Business.Market.CommandExecutor[m].OpenTime = Command.OpenTime;
                                                                        Business.Market.CommandExecutor[m].OpenPrice = Command.OpenPrice;
                                                                        Business.Market.CommandExecutor[m].IsActivePending = false;
                                                                        Business.Market.CommandExecutor[m].IsStopLossAndTakeProfit = false; 

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            #region UPDATE COMMAND TYPE IN SYMBOL LIST
                                                            if (this.CommandList[i].Symbol != null && this.CommandList[i].Symbol.CommandList != null)
                                                            {
                                                                int countCommand = this.CommandList[i].Symbol.CommandList.Count;
                                                                for (int j = 0; j < countCommand; j++)
                                                                {
                                                                    if (this.CommandList[i].Symbol.CommandList[j].ID == Command.ID)
                                                                    {
                                                                        this.CommandList[i].Symbol.CommandList[j].Type = Command.Type;
                                                                        this.CommandList[i].Symbol.CommandList[j].Commission = Command.Commission;
                                                                        this.CommandList[i].Symbol.CommandList[j].AgentCommission = agentCommission;
                                                                        this.CommandList[i].Symbol.CommandList[j].Margin = Command.Margin;
                                                                        this.CommandList[i].Symbol.CommandList[j].OpenTime = Command.OpenTime;
                                                                        this.CommandList[i].Symbol.CommandList[j].OpenPrice = Command.OpenPrice;
                                                                        this.CommandList[i].Symbol.CommandList[j].IsActivePending = false;
                                                                        this.CommandList[i].Symbol.CommandList[j].IsStopLossAndTakeProfit = false;  

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            //Update Command Type In Database
                                                            TradingServer.Facade.FacadeUpdateOpenTrade(Command);

                                                            #region Map Command Server To Client
                                                            if (this.ClientCommandQueue == null)
                                                                this.ClientCommandQueue = new List<string>();

                                                            string Message = "UpdatePendingOrder$True,Update Pending Order Command To Spot Command," + this.CommandList[i].ID + "," +
                                                                this.CommandList[i].Investor.InvestorID + "," + this.CommandList[i].Symbol.Name + "," + this.CommandList[i].Size + "," +
                                                                IsBuy + "," + this.CommandList[i].OpenTime + "," + this.CommandList[i].OpenPrice + "," + this.CommandList[i].StopLoss + "," +
                                                                this.CommandList[i].TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                                                Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + "," + this.CommandList[i].ExpTime + "," +
                                                                this.CommandList[i].ClientCode + "," + this.CommandList[i].CommandCode + "," + this.CommandList[i].IsHedged + "," +
                                                                this.CommandList[i].Type.ID + "," + this.CommandList[i].Margin + ",UpdatePendingOrder";

                                                            //int countInvestor = this.CountInvestorOnline(this.InvestorID);
                                                            //if (countInvestor > 0)
                                                            this.ClientCommandQueue.Add(Message);

                                                            //SEND NOTIFY TO AGENT SERVER
                                                            Message += "," + this.CommandList[i].AgentRefConfig;
                                                            Business.AgentNotify newAgentNotify = new AgentNotify();
                                                            newAgentNotify.NotifyMessage = Message;
                                                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, this.CommandList[i].Investor.InvestorGroupInstance);
                                                            #endregion

                                                            #region CALCULATION TOTAL MARGIN
                                                            if (this.CommandList.Count > 0)
                                                            {
                                                                Business.Margin newMargin = new Margin();
                                                                newMargin = this.CommandList[0].Symbol.CalculationTotalMargin(this.CommandList);
                                                                this.Margin = newMargin.TotalMargin;
                                                                this.FreezeMargin = newMargin.TotalFreezeMargin;
                                                            }
                                                            else
                                                            {
                                                                this.Margin = 0;
                                                            }
                                                            #endregion

                                                            //SENT COMMAND REMOVE COMMAND TO MANAGER
                                                            TradingServer.Facade.FacadeSendNoticeManagerRequest(2, this.CommandList[i]);

                                                            //SENT NOTIFY ADD COMMAND TO MANAGER
                                                            TradingServer.Facade.FacadeSendNoticeManagerRequest(1, this.CommandList[i]);

                                                            //SEND NOTIFY TO MANAGE CHANGE ACCOUNT
                                                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, this);

                                                            //SEND COMMAND TO BROKER SERVER(CLOSE COMMAND)
                                                            //try
                                                            //{
                                                            //    string cmdBroker = Command.CommandCode + "¬" + Command.Size + "¬" + Command.Symbol.Name + "¬" +
                                                            //        Command.Investor.Code;

                                                            //    TradingServer.Business.Market.clientBroker.NotifyBroker(cmdBroker);
                                                            //}
                                                            //catch (Exception ex)
                                                            //{

                                                            //}

                                                            this.CommandList[i].IsProcess = false;

                                                            //UPDATE DATABASE 
                                                            this.CommandList[i].UpdateIsActivePending(false, false, this.CommandList[i].ID, this.InvestorID);

                                                            #region LOG ACTIVATE PENDING ORDER
                                                            if (this.CommandList[i].Symbol != null)
                                                            {
                                                                string content = string.Empty;
                                                                string mode = string.Empty;
                                                                string size = string.Empty;
                                                                string openPrice = string.Empty;
                                                                string bid = string.Empty;
                                                                string ask = string.Empty;

                                                                bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Symbol.TickValue.Bid.ToString(), this.CommandList[i].Symbol.Digit);
                                                                ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Symbol.TickValue.Ask.ToString(), this.CommandList[i].Symbol.Digit);

                                                                TradingServer.Facade.FacadeConvertTypeIDToName(this.CommandList[i].Type.ID);
                                                                size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Size.ToString(), 2);
                                                                openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                                                                content = "order #" + this.CommandList[i].CommandCode + " " + mode + " " + size + " " + this.CommandList[i].Symbol.Name + " is opened at " +
                                                                    openPrice + " (" + bid + "/" + ask + ")";

                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", this.Code);
                                                            }
                                                            else
                                                            {
                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, "pending order triggerd [symbol empty]", "[pending order triggered]", "", this.Code);
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region CLOSE PENDING ORDER IF CHECK ACCOUNT INVALID
                                                            //SET PROPERTY FOR CLOSE COMMAND
                                                            Command.CloseTime = DateTime.Now;
                                                            Command.Comment = "Cancel pending order #" + Command.CommandCode + " [not enough money]";
                                                            Command.Type = this.CommandList[i].Type;

                                                            //ADD PENDING ORDER TO DATABASE
                                                            TradingServer.Facade.FacadeAddNewCommandHistory(Command.Investor.InvestorID, Command.Type.ID, Command.CommandCode,
                                                                Command.OpenTime, Command.OpenPrice, Command.CloseTime, Command.ClosePrice, 0, 0, 0, Command.ExpTime, Command.Size, Command.StopLoss,
                                                                Command.TakeProfit, Command.ClientCode, Command.Symbol.SymbolID, Command.Taxes, Command.AgentCommission,
                                                                Command.Comment, "5", Command.TotalSwap,
                                                                Command.RefCommandID, Command.AgentRefConfig, Command.IsActivePending, Command.IsStopLossAndTakeProfit);

                                                            #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                                                            ////Remove Command In Symbol List
                                                            //bool resultRemoveCommandSymbol = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(Command.ID);

                                                            ////Remove Command In Command Executor
                                                            //bool deleteCommandExe = TradingServer.Facade.FacadeRemoveOpenTradeInCommandExecutor(Command.ID);
                                                            #endregion

                                                            //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                            //Business.OpenTrade newOpenTrade = this.CommandList[i];
                                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                                            newOpenRemove.InvestorID = this.InvestorID;
                                                            newOpenRemove.OpenTradeID = this.CommandList[i].ID;
                                                            newOpenRemove.SymbolName = this.CommandList[i].Symbol.Name;
                                                            newOpenRemove.IsExecutor = true;
                                                            newOpenRemove.IsSymbol = true;
                                                            newOpenRemove.IsInvestor = false;
                                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                            //Delete Command In Database                             
                                                            TradingServer.Facade.FacadeDeleteOpenTradeByID(Command.ID);

                                                            //Close Command Complete Add Message To Client
                                                            if (this.ClientCommandQueue == null)
                                                                this.ClientCommandQueue = new List<string>();

                                                            string Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                                Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                                Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                                1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                                                                DateTime.Now;

                                                            this.ClientCommandQueue.Add(Message);
                                                            #endregion

                                                            #region LOG ACTIVATE PENDING ORDER
                                                            if (this.CommandList[i].Symbol != null)
                                                            {
                                                                string content = string.Empty;
                                                                string mode = string.Empty;
                                                                string size = string.Empty;
                                                                string openPrice = string.Empty;
                                                                string bid = string.Empty;
                                                                string ask = string.Empty;

                                                                bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Symbol.TickValue.Bid.ToString(), this.CommandList[i].Symbol.Digit);
                                                                ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Symbol.TickValue.Ask.ToString(), this.CommandList[i].Symbol.Digit);

                                                                TradingServer.Facade.FacadeConvertTypeIDToName(this.CommandList[i].Type.ID);
                                                                size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Size.ToString(), 2);
                                                                openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                                                                content = "order #" + this.CommandList[i].CommandCode + " " + mode + " " + size + " " + this.CommandList[i].Symbol.Name + " is opened at " +
                                                                    openPrice + " (" + bid + "/" + ask + ")" + " [unsuccessful - not enough money]";

                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[unsuccessful - not enough money]", "", this.Code);
                                                            }
                                                            else
                                                            {
                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, "pending order triggered [symbol empty]", "[unsuccessful - not enough money]", "", this.Code);
                                                            }
                                                            #endregion

                                                            //SENT NOTIFY TO MANAGER
                                                            TradingServer.Facade.FacadeSendNoticeManagerRequest(2, this.CommandList[i]);

                                                            lock (Business.Market.syncObject)
                                                            {
                                                                this.CommandList.Remove(this.CommandList[i]);
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }

                                            #region UPDATE STOP LOSS AND TAKE PROFIT 
                                            //Update Database If Command Is Update StopLoss Or Update Take Profit
                                            if (this.CommandList[i].StopLoss != Command.StopLoss ||
                                                this.CommandList[i].TakeProfit != Command.TakeProfit ||
                                                this.CommandList[i].OpenPrice != Command.OpenPrice)
                                            {
                                                //TradingServer.Facade.FacadeUpdateOnlineCommandWithTakeProfit(Command.TakeProfit, Command.StopLoss, Command.ID);

                                                this.CommandList[i].TakeProfit = Command.TakeProfit;
                                                this.CommandList[i].StopLoss = Command.StopLoss;

                                                bool checkIsPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(this.CommandList[i].Type.ID);
                                                //if (this.CommandList[i].Type.ID == 7 || this.CommandList[i].Type.ID == 8 ||
                                                //    this.CommandList[i].Type.ID == 9 || this.CommandList[i].Type.ID == 10)
                                                if(checkIsPending)
                                                {
                                                    if (this.CommandList[i].OpenPrice != Command.OpenPrice)
                                                    {
                                                        this.CommandList[i].OpenPrice = Command.OpenPrice;
                                                    }
                                                }

                                                //If Client Update Command Then Add Message To Client Message
                                                if (this.ClientCommandQueue == null)
                                                    this.ClientCommandQueue = new List<string>();

                                                //SEND NOTIFY TO MANAGER
                                                TradingServer.Facade.FacadeSendNoticeManagerRequest(1, this.CommandList[i]);
                                            }
                                            #endregion
                                            
                                            #endregion
                                        }

                                        FlagCommand = true;
                                        break;
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            #region Init Command List Of Investor
                            this.CommandList = new List<OpenTrade>();
                            this.CommandList.Add(Command);
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    //Call Function ReCalculation Account Of Investor
                    this.ReCalculationAccount();

                    DateTime timeStop = DateTime.Now;
                    TimeSpan timeProcess = timeStop - timeStart;

                    if (this.IsMonitor)
                    {
                        StringBuilder monitor = new StringBuilder();
                        monitor.Append("Time Process " + timeProcess.TotalMilliseconds + " millisecond <=> Command Code: " + Command.CommandCode + "<=>");
                        monitor.Append("Balance: " + this.Balance + "<=> Margin Level: " + Math.Round(this.MarginLevel,2) + "<=> Margin: " + this.Margin);

                        if (this.ListMonitor == null)
                            this.ListMonitor = new List<string>();

                        this.ListMonitor.Insert(0, monitor.ToString());

                        if (this.ListMonitor.Count > 10)
                        {
                            this.ListMonitor.RemoveAt(10);
                        }
                    }

                    Command = null;
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            this.isInTask = false;
            Investor.IsInProcess = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        private void ProcessSetTassMT4()
        {
            Investor.IsInProcess = true;
            this.isInTask = true;

            while (this.UpdateCommands.Count > 0)
            {
                #region WHILE
                try
                {   
                    Business.OpenTrade Command = this.UpdateCommands[0];

                    this.UpdateCommands.RemoveAt(0);

                    if (Command == null)
                        continue;

                    bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

                    #region PROCESS MARKET AREA
                    if (Command.Symbol.MarketAreaRef.IMarketAreaName.Trim() == "SpotCommand")
                    {
                        #region Spot Command
                        if (this.CommandList != null)
                        {
                            #region For Command List
                            for (int i = 0; i < this.CommandList.Count; i++)
                            {
                                if (this.CommandList[i].ID == Command.ID)
                                {
                                    if (Command.IsClose == true)
                                    {
                                        #region COMMAND CLOSE
                                        int commandRefID = this.CommandList[i].ID;
                                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);

                                        if (isPending)
                                        {
                                            #region CLOSE PENDING ORDER
                                            //ADD PENDING ORDER TO DATABASE
                                            int addHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Command.Investor.InvestorID, Command.Type.ID, Command.CommandCode,
                                                Command.OpenTime, Command.OpenPrice, Command.CloseTime, Command.ClosePrice, 0, 0, 0, Command.ExpTime, Command.Size, Command.StopLoss,
                                                Command.TakeProfit, Command.ClientCode, Command.Symbol.SymbolID, Command.Taxes, Command.AgentCommission, Command.Comment, "3",
                                                Command.TotalSwap, Command.RefCommandID, Command.AgentRefConfig, Command.IsActivePending, Command.IsStopLossAndTakeProfit);

                                            //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                            newOpenRemove.InvestorID = this.InvestorID;
                                            newOpenRemove.OpenTradeID = this.CommandList[i].ID;
                                            newOpenRemove.SymbolName = this.CommandList[i].Symbol.Name;
                                            newOpenRemove.IsExecutor = true;
                                            newOpenRemove.IsSymbol = true;
                                            newOpenRemove.IsInvestor = false;
                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                            //Close Command Complete Add Message To Client
                                            if (this.ClientCommandQueue == null)
                                                this.ClientCommandQueue = new List<string>();
                                            #endregion

                                            #region MAP STRING SEND TO CLIENT
                                            string Message = string.Empty;
                                            if (Command.IsServer)
                                            {
                                                Message = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                    ",Close," + Command.CloseTime;
                                            }
                                            else
                                            {
                                                Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                    ",Close," + Command.CloseTime;
                                            }

                                            this.ClientCommandQueue.Add(Message);

                                            if (Business.Market.marketInstance.MQLCommands != null)
                                            {
                                                int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                for (int j = 0; j < countMQL; j++)
                                                {
                                                    if (Business.Market.marketInstance.MQLCommands[j].InvestorCode == Command.Investor.Code)
                                                    {
                                                        Command.IpAddress = Business.Market.marketInstance.MQLCommands[j].IpAddress;
                                                        Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[j]);
                                                        break;
                                                    }
                                                }
                                            }

                                            #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                            string mode = "sell";
                                            if (this.CommandList[i].Type.ID == 1)
                                                mode = "buy";

                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                                            string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), this.CommandList[i].Symbol.Digit);
                                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), this.CommandList[i].Symbol.Digit);
                                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), this.CommandList[i].Symbol.Digit);
                                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), this.CommandList[i].Symbol.Digit);
                                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), this.CommandList[i].Symbol.Digit);

                                            string contentServer = "'" + this.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                                Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " completed";

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.IpAddress, Command.Investor.Code);
                                            #endregion

                                            lock (Business.Market.syncObject)
                                            {
                                                bool deleteCommandInvestor = this.CommandList.Remove(this.CommandList[i]);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            int commandId = Command.ID;

                                            //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                            newOpenRemove.InvestorID = this.InvestorID;
                                            newOpenRemove.OpenTradeID = commandId;
                                            newOpenRemove.SymbolName = this.CommandList[i].Symbol.Name;
                                            newOpenRemove.IsExecutor = true;
                                            newOpenRemove.IsSymbol = true;
                                            newOpenRemove.IsInvestor = false;
                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                            //Close Command Complete Add Message To Client
                                            if (this.ClientCommandQueue == null)
                                                this.ClientCommandQueue = new List<string>();

                                            #region Map Command Server To Client
                                            if (Command.IsServer)
                                            {
                                                string Message = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                    ",Close," + Command.CloseTime;

                                                this.ClientCommandQueue.Add(Message);

                                                TradingServer.Facade.FacadeAddNewSystemLog(5, Message, "  ", Command.IpAddress, Command.Investor.Code);
                                            }
                                            else
                                            {
                                                string Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                    ",Close," + Command.CloseTime;

                                                this.ClientCommandQueue.Add(Message);

                                                TradingServer.Facade.FacadeAddNewSystemLog(5, Message, "  ", Command.IpAddress, Command.Investor.Code);
                                            }
                                            #endregion

                                            if (Business.Market.marketInstance.MQLCommands != null)
                                            {
                                                int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                for (int j = 0; j < countMQL; j++)
                                                {
                                                    if (Business.Market.marketInstance.MQLCommands[j].InvestorCode == Command.Investor.Code)
                                                    {
                                                        Command.IpAddress = Business.Market.marketInstance.MQLCommands[j].IpAddress;
                                                        Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[j]);
                                                        break;
                                                    }
                                                }
                                            }

                                            #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                            string mode = "sell";
                                            if (this.CommandList[i].Type.ID == 1)
                                                mode = "buy";

                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                                            string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), this.CommandList[i].Symbol.Digit);
                                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), this.CommandList[i].Symbol.Digit);
                                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), this.CommandList[i].Symbol.Digit);
                                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), this.CommandList[i].Symbol.Digit);
                                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), this.CommandList[i].Symbol.Digit);

                                            string contentServer = "'" + this.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                                Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " completed";

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.IpAddress, Command.Investor.Code);
                                            #endregion

                                            lock (Business.Market.syncObject)
                                            {
                                                bool deleteCommandInvestor = this.CommandList.Remove(this.CommandList[i]);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region UPDATE PENDING ORDER(PENDING ORDER ACTIVE)
                                        if (this.CommandList[i].Type.ID != Command.Type.ID)
                                        {
                                            if (this.CommandList[i].Type.ID > Command.Type.ID)
                                            {
                                                //SET OPEN TIME FOR COMMAND
                                                this.CommandList[i].OpenTime = DateTime.Now;
                                                Command.OpenTime = DateTime.Now;
                                                this.CommandList[i].OpenPrice = Command.OpenPrice;
                                                this.CommandList[i].IsActivePending = false;
                                                this.CommandList[i].IsStopLossAndTakeProfit = false;
                                                this.CommandList[i].Type = Command.Type;

                                                //CALCULATION MARGIN FOR COMMAND PENDING ORDER
                                                this.CommandList[i].CalculatorMarginCommand(this.CommandList[i]);

                                                //SET MARGIN FOR COMMAND
                                                Command.Margin = this.CommandList[i].Margin;

                                                #region FIND COMMAND IN COMMAND EXECUTOR AND UPDATE TYPE
                                                if (this.CommandList[i].InsExe != null && this.CommandList[i].InsExe.ID == Command.ID)
                                                {
                                                    this.CommandList[i].InsExe.Type = Command.Type;
                                                    this.CommandList[i].InsExe.Commission = Command.Commission;

                                                    this.CommandList[i].InsExe.Margin = Command.Margin;
                                                    this.CommandList[i].InsExe.OpenTime = Command.OpenTime;
                                                    this.CommandList[i].InsExe.OpenPrice = Command.OpenPrice;
                                                    this.CommandList[i].InsExe.IsActivePending = false;
                                                    this.CommandList[i].InsExe.IsStopLossAndTakeProfit = false;
                                                }

                                                //if (Business.Market.CommandExecutor != null)
                                                //{
                                                //    int countExe = Business.Market.CommandExecutor.Count;
                                                //    for (int m = 0; m < countExe; m++)
                                                //    {
                                                //        if (Business.Market.CommandExecutor[m].ID == Command.ID)
                                                //        {
                                                //            Business.Market.CommandExecutor[m].Type = Command.Type;
                                                //            Business.Market.CommandExecutor[m].Commission = Command.Commission;
                                                            
                                                //            Business.Market.CommandExecutor[m].Margin = Command.Margin;
                                                //            Business.Market.CommandExecutor[m].OpenTime = Command.OpenTime;
                                                //            Business.Market.CommandExecutor[m].OpenPrice = Command.OpenPrice;
                                                //            Business.Market.CommandExecutor[m].IsActivePending = false;
                                                //            Business.Market.CommandExecutor[m].IsStopLossAndTakeProfit = false;

                                                //            break;
                                                //        }
                                                //    }
                                                //}
                                                #endregion

                                                #region UPDATE COMMAND TYPE IN SYMBOL LIST
                                                if (this.CommandList[i].Symbol != null && this.CommandList[i].Symbol.CommandList != null)
                                                {
                                                    int countCommand = this.CommandList[i].Symbol.CommandList.Count;
                                                    for (int j = 0; j < countCommand; j++)
                                                    {
                                                        if (this.CommandList[i].Symbol.CommandList[j].ID == Command.ID)
                                                        {
                                                            this.CommandList[i].Symbol.CommandList[j].Type = Command.Type;
                                                            this.CommandList[i].Symbol.CommandList[j].Commission = Command.Commission;
                                                            
                                                            this.CommandList[i].Symbol.CommandList[j].Margin = Command.Margin;
                                                            this.CommandList[i].Symbol.CommandList[j].OpenTime = Command.OpenTime;
                                                            this.CommandList[i].Symbol.CommandList[j].OpenPrice = Command.OpenPrice;
                                                            this.CommandList[i].Symbol.CommandList[j].IsActivePending = false;
                                                            this.CommandList[i].Symbol.CommandList[j].IsStopLossAndTakeProfit = false;

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                //Update Command Type In Database
                                                //TradingServer.Facade.FacadeUpdateOpenTrade(Command);

                                                #region Map Command Server To Client
                                                if (this.ClientCommandQueue == null)
                                                    this.ClientCommandQueue = new List<string>();

                                                string Message = "UpdatePendingOrder$True,Update Pending Order Command To Spot Command," + this.CommandList[i].ID + "," +
                                                    this.CommandList[i].Investor.InvestorID + "," + this.CommandList[i].Symbol.Name + "," + this.CommandList[i].Size + "," +
                                                    IsBuy + "," + this.CommandList[i].OpenTime + "," + this.CommandList[i].OpenPrice + "," + this.CommandList[i].StopLoss + "," +
                                                    this.CommandList[i].TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                                    Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + "," + this.CommandList[i].ExpTime + "," +
                                                    this.CommandList[i].ClientCode + "," + this.CommandList[i].CommandCode + "," + this.CommandList[i].IsHedged + "," +
                                                    this.CommandList[i].Type.ID + "," + this.CommandList[i].Margin + ",UpdatePendingOrder";

                                                //int countInvestor = this.CountInvestorOnline(this.InvestorID);
                                                //if (countInvestor > 0)
                                                this.ClientCommandQueue.Add(Message);
                                                #endregion

                                                this.CommandList[i].IsProcess = false;
                                            }
                                        }
                                        #endregion

                                        #region UPDATE STOP LOSS AND TAKE PROFIT
                                        //Update Database If Command Is Update StopLoss Or Update Take Profit
                                        if (this.CommandList[i].StopLoss != Command.StopLoss ||
                                            this.CommandList[i].TakeProfit != Command.TakeProfit ||
                                            this.CommandList[i].OpenPrice != Command.OpenPrice)
                                        {
                                            this.CommandList[i].TakeProfit = Command.TakeProfit;
                                            this.CommandList[i].StopLoss = Command.StopLoss;

                                            bool checkIsPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(this.CommandList[i].Type.ID);

                                            if (checkIsPending)
                                            {
                                                if (this.CommandList[i].OpenPrice != Command.OpenPrice)
                                                {
                                                    this.CommandList[i].OpenPrice = Command.OpenPrice;
                                                }
                                            }

                                            //If Client Update Command Then Add Message To Client Message
                                            if (this.ClientCommandQueue == null)
                                                this.ClientCommandQueue = new List<string>();

                                            //SEND NOTIFY TO MANAGER
                                            TradingServer.Facade.FacadeSendNoticeManagerRequest(1, this.CommandList[i]);
                                        }
                                        #endregion
                                    }
                                    break;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Init Command List Of Investor
                            this.CommandList = new List<OpenTrade>();
                            this.CommandList.Add(Command);
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    //Call Function ReCalculation Account Of Investor
                    this.ReCalculationAccount();

                    Command = null;
                }
                catch (Exception ex)
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(1, ex.Message, "[Process Close Open Position]", "", "");
                }
                #endregion
            }

            this.isInTask = false;
            Investor.IsInProcess = false;
        }
    }
}
