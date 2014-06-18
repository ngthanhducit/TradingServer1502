using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class Investor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRollBack"></param>
        internal bool MultipleCloseCommand(Business.OpenTrade command)
        {
            bool result = false;
            if (this.CommandList != null && this.CommandList.Count > 0)
            {
                int count = this.CommandList.Count;
                for (int i = 0; i < this.CommandList.Count; i++)
                {
                    if (this.CommandList[i].ID == command.ID)
                    {
                        bool IsBuy = false;
                        if (this.CommandList[i].Type.ID == 1 || this.CommandList[i].Type.ID == 7 ||
                            this.CommandList[i].Type.ID == 9 || this.CommandList[i].Type.ID == 11 ||
                            this.CommandList[i].Type.ID == 17 || this.CommandList[i].Type.ID == 19)
                        {
                            IsBuy = true;
                        }

                        #region SET NEW DATA
                        this.CommandList[i].CloseTime = DateTime.Now;
                        this.CommandList[i].ClosePrice = command.ClosePrice;

                        //this.CommandList[i].CalculatorProfitCommand(this.CommandList[i]);
                        //this.CommandList[i].Profit = this.CommandList[i].Symbol.ConvertCurrencyToUSD(this.CommandList[i].Symbol.Currency, this.CommandList[i].Profit, false,
                        //    this.CommandList[i].SpreaDifferenceInOpenTrade, this.CommandList[i].Symbol.Digit);
                        #endregion

                        #region CLOSE COMMAND

                        #region COMMAND CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                        ////Remove Command In Symbol List
                        //bool temp = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(this.CommandList[i].ID);

                        ////Remove Command In Command Executor
                        //bool deleteCommandExe = TradingServer.Facade.FacadeRemoveOpenTradeInCommandExecutor(this.CommandList[i].ID);
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

                        //if (temp && deleteCommandExe)
                        //{
                            double totalProfit = Math.Round(this.CommandList[i].Profit + this.CommandList[i].Commission + this.CommandList[i].Swap, 2);

                            //Update Balance Of Investor Account
                            bool updateBalance = this.UpdateBalance(this.CommandList[i].Investor.InvestorID, this.Balance);

                            if (updateBalance)
                            {
                                //'00001140': close command #00148535 balance : 123456 command profit : 10000
                                string content = "'" + this.Code + "': multiple close command #" + this.CommandList[i].CommandCode + " balance : " + this.Balance + " command profit: " + totalProfit +
                                    " Commission: " + this.CommandList[i].Commission + " Swap: " + this.CommandList[i].Swap;

                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[Close Command]", "", this.Code);

                                this.Balance += totalProfit;

                                //Close Command Complete Add Message To Client
                                if (this.ClientCommandQueue == null)
                                    this.ClientCommandQueue = new List<string>();

                                #region Map Command Server To Client
                                if (command.IsServer)
                                {
                                    string Message = "CloseCommandByManager$True,Close Command Complete," + this.CommandList[i].ID + "," + this.CommandList[i].Investor.InvestorID + "," +
                                        this.CommandList[i].Symbol.Name + "," + this.CommandList[i].Size + "," + IsBuy + "," + this.CommandList[i].OpenTime + "," +
                                        this.CommandList[i].OpenPrice + "," + this.CommandList[i].StopLoss + "," + this.CommandList[i].TakeProfit + "," +
                                        this.CommandList[i].ClosePrice + "," + this.CommandList[i].Commission + "," + this.CommandList[i].Swap + "," + this.CommandList[i].Profit + "," +
                                        "Comment," + this.CommandList[i].ID + "," + this.CommandList[i].Type.Name + "," + 1 + "," + this.CommandList[i].ExpTime + "," +
                                        this.CommandList[i].ClientCode + "," + this.CommandList[i].CommandCode + "," + this.CommandList[i].IsHedged + "," + this.CommandList[i].Type.ID + "," +
                                        this.CommandList[i].Margin + ",Close," + this.CommandList[i].CloseTime;

                                    if (this.ClientCommandQueue == null)
                                        this.ClientCommandQueue = new List<string>();

                                    this.ClientCommandQueue.Add(Message);
                                }
                                else
                                {
                                    string Message = "CloseCommand$True,Close Command Complete," + this.CommandList[i].ID + "," + this.CommandList[i].Investor.InvestorID + "," +
                                        this.CommandList[i].Symbol.Name + "," + this.CommandList[i].Size + "," + IsBuy + "," + this.CommandList[i].OpenTime + "," +
                                        this.CommandList[i].OpenPrice + "," + this.CommandList[i].StopLoss + "," + this.CommandList[i].TakeProfit + "," +
                                        this.CommandList[i].ClosePrice + "," + this.CommandList[i].Commission + "," + this.CommandList[i].Swap + "," +
                                        this.CommandList[i].Profit + "," + "Comment," + this.CommandList[i].ID + "," + this.CommandList[i].Type.Name + "," +
                                        1 + "," + this.CommandList[i].ExpTime + "," + this.CommandList[i].ClientCode + "," + this.CommandList[i].CommandCode + "," +
                                        this.CommandList[i].IsHedged + "," + this.CommandList[i].Type.ID + "," + this.CommandList[i].Margin +
                                        ",Close," + this.CommandList[i].CloseTime;

                                    if (this.ClientCommandQueue == null)
                                        this.ClientCommandQueue = new List<string>();

                                    this.ClientCommandQueue.Add(Message);
                                }
                                #endregion

                                #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(this.CommandList[i].Type.ID).ToLower();
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Size.ToString(), 2);
                                string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                                string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].StopLoss.ToString(), this.CommandList[i].Symbol.Digit);
                                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].TakeProfit.ToString(), this.CommandList[i].Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Symbol.TickValue.Bid.ToString(), this.CommandList[i].Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Symbol.TickValue.Ask.ToString(), this.CommandList[i].Symbol.Digit);

                                string contentServer = "'" + this.Code + "': multiple close order #" + this.CommandList[i].CommandCode + " (" + mode + " " + size + " " +
                                    this.CommandList[i].Symbol.Name + " at " + this.CommandList[i].OpenPrice + ") at " + command.ClosePrice + " completed";

                                TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "[multiple close]", this.CommandList[i].Investor.IpAddress, this.CommandList[i].Investor.Code);
                                #endregion

                                bool removeOpenTrade = false;
                                lock (Business.Market.syncObject)
                                {
                                    //Remove Command In Investor List
                                    removeOpenTrade = this.CommandList.Remove(this.CommandList[i]);
                                }

                                if (removeOpenTrade)
                                {
                                    if (this.CommandList.Count > 0)
                                    {
                                        //RECACULATION TOTAL MARGIN OF INVESTOR
                                        Business.Margin newMargin = new Margin();
                                        newMargin = this.CommandList[0].Symbol.CalculationTotalMargin(this.CommandList);
                                        this.Margin = newMargin.TotalMargin;
                                        this.FreezeMargin = newMargin.TotalFreezeMargin;
                                    }
                                    else
                                    {
                                        this.Margin = 0;
                                    }

                                    //SEND NOTIFY TO MANAGER THEN CLOSE COMMAND
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, this);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        //}
                        //else
                        //{
                        //    return false;
                        //}

                        #endregion

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
        /// <param name="command"></param>
        private void MultiCloseCommand(Business.OpenTrade command)
        {
            List<Business.OpenTrade> rollbackData = new List<OpenTrade>();

            if (this.CommandList != null)
            {
                for (int i = 0; i < this.CommandList.Count; i++)
                {
                    if (this.CommandList[i].Symbol.Name.ToUpper().Trim() == command.Symbol.Name.ToUpper().Trim())
                    {
                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(this.CommandList[i].Type.ID);
                        if (!isPending)
                        {
                            #region BACKUP DATA
                            Business.OpenTrade newOpenTrade = new OpenTrade();
                            newOpenTrade.AgentCommission = this.CommandList[i].AgentCommission;
                            newOpenTrade.ClientCode = this.CommandList[i].ClientCode;
                            newOpenTrade.ClosePrice = this.CommandList[i].ClosePrice;
                            newOpenTrade.CommandCode = this.CommandList[i].CommandCode;
                            newOpenTrade.Comment = this.CommandList[i].Comment;
                            newOpenTrade.Commission = this.CommandList[i].Commission;
                            newOpenTrade.ExpTime = this.CommandList[i].ExpTime;
                            newOpenTrade.FreezeMargin = this.CommandList[i].FreezeMargin;
                            newOpenTrade.ID = this.CommandList[i].ID;
                            newOpenTrade.IGroupSecurity = this.CommandList[i].IGroupSecurity;
                            newOpenTrade.Investor = this.CommandList[i].Investor;
                            newOpenTrade.IsClose = this.CommandList[i].IsClose;
                            newOpenTrade.IsHedged = this.CommandList[i].IsHedged;
                            newOpenTrade.IsMultiClose = this.CommandList[i].IsMultiClose;
                            newOpenTrade.IsMultiUpdate = this.CommandList[i].IsMultiUpdate;
                            newOpenTrade.IsProcess = this.CommandList[i].IsProcess;
                            newOpenTrade.IsServer = this.CommandList[i].IsServer;
                            newOpenTrade.ManagerID = this.CommandList[i].ManagerID;
                            newOpenTrade.Margin = this.CommandList[i].Margin;
                            newOpenTrade.MaxDev = this.CommandList[i].MaxDev;
                            newOpenTrade.NumberUpdate = this.CommandList[i].NumberUpdate;
                            newOpenTrade.OpenPrice = this.CommandList[i].OpenPrice;
                            newOpenTrade.OpenTime = this.CommandList[i].OpenTime;
                            newOpenTrade.Size = this.CommandList[i].Size;
                            newOpenTrade.SpreaDifferenceInOpenTrade = this.CommandList[i].SpreaDifferenceInOpenTrade;
                            newOpenTrade.StopLoss = this.CommandList[i].StopLoss;
                            newOpenTrade.Swap = this.CommandList[i].Swap;
                            newOpenTrade.Symbol = this.CommandList[i].Symbol;
                            newOpenTrade.TakeProfit = this.CommandList[i].TakeProfit;
                            newOpenTrade.Taxes = this.CommandList[i].Taxes;
                            newOpenTrade.Type = this.CommandList[i].Type;
                            newOpenTrade.CloseTime = DateTime.Now;
                            newOpenTrade.Profit = this.CommandList[i].Profit;

                            if (this.CommandList[i].Type.ID == 2 || this.CommandList[i].Type.ID == 12)
                            {
                                //newOpenTrade.ClosePrice = this.CommandList[i].Symbol.CreateAskPrices(command.ClosePrice, this.CommandList[i].Symbol.SpreadByDefault,
                                //    this.CommandList[i].Symbol.Digit, int.Parse(this.CommandList[i].SpreaDifferenceInOpenTrade.ToString()));
                                newOpenTrade.ClosePrice = this.CommandList[i].Symbol.CreateAskPrices(this.CommandList[i].Symbol.Digit,
                                    int.Parse(this.CommandList[i].SpreaDifferenceInOpenTrade.ToString()), command.ClosePrice);
                            }

                            #region SET NEW DATA
                            newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                            newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false,
                                newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);
                            #endregion

                            rollbackData.Add(newOpenTrade);
                            #endregion
                        }
                    }
                }
            }

            if (rollbackData != null && rollbackData.Count > 0)
            {
                TradingServer.DBW.DBWOnlineCommand newDBWOnlineCommand = new DBW.DBWOnlineCommand();
                bool resultMultiClose = newDBWOnlineCommand.MultipleCloseOpenTrade(rollbackData);
                if (!resultMultiClose)
                {
                    //rollback data
                    this.RollBackData(rollbackData, 1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRollBack"></param>
        private void RollBackData(List<Business.OpenTrade> dataRollBack, int mode)
        {
            if (dataRollBack != null && dataRollBack.Count > 0)
            {
                int count = dataRollBack.Count;
                for (int i = 0; i < count; i++)
                {
                    dataRollBack[i].RollBackOpenTradeInCommandExe(dataRollBack[i], mode);
                    dataRollBack[i].RollBackOpenTradeInInvestor(dataRollBack[i], mode);
                    dataRollBack[i].RollBackOpenTradeInSymbolList(dataRollBack[i], mode);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void MultiUpdateCommand(Business.OpenTrade command)
        {
            List<Business.OpenTrade> listBackupData = new List<OpenTrade>();

            if (this.CommandList != null)
            {
                int count = this.CommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.CommandList[i].Symbol.Name.Trim().ToUpper() == command.Symbol.Name.Trim().ToUpper())
                    {
                        #region BACKUP DATA
                        Business.OpenTrade newOpenTrade = new OpenTrade();
                        newOpenTrade.AgentCommission = this.CommandList[i].AgentCommission;
                        newOpenTrade.ClientCode = this.CommandList[i].ClientCode;
                        newOpenTrade.ClosePrice = this.CommandList[i].ClosePrice;
                        newOpenTrade.CommandCode = this.CommandList[i].CommandCode;
                        newOpenTrade.Comment = this.CommandList[i].Comment;
                        newOpenTrade.Commission = this.CommandList[i].Commission;
                        newOpenTrade.ExpTime = this.CommandList[i].ExpTime;
                        newOpenTrade.FreezeMargin = this.CommandList[i].FreezeMargin;
                        newOpenTrade.ID = this.CommandList[i].ID;
                        newOpenTrade.IGroupSecurity = this.CommandList[i].IGroupSecurity;
                        newOpenTrade.Investor = this.CommandList[i].Investor;
                        newOpenTrade.IsClose = this.CommandList[i].IsClose;
                        newOpenTrade.IsHedged = this.CommandList[i].IsHedged;
                        newOpenTrade.IsMultiClose = this.CommandList[i].IsMultiClose;
                        newOpenTrade.IsMultiUpdate = this.CommandList[i].IsMultiUpdate;
                        newOpenTrade.IsProcess = this.CommandList[i].IsProcess;
                        newOpenTrade.IsServer = this.CommandList[i].IsServer;
                        newOpenTrade.ManagerID = this.CommandList[i].ManagerID;
                        newOpenTrade.Margin = this.CommandList[i].Margin;
                        newOpenTrade.MaxDev = this.CommandList[i].MaxDev;
                        newOpenTrade.NumberUpdate = this.CommandList[i].NumberUpdate;
                        newOpenTrade.OpenPrice = this.CommandList[i].OpenPrice;
                        newOpenTrade.OpenTime = this.CommandList[i].OpenTime;
                        newOpenTrade.Size = this.CommandList[i].Size;
                        newOpenTrade.SpreaDifferenceInOpenTrade = this.CommandList[i].SpreaDifferenceInOpenTrade;
                        newOpenTrade.StopLoss = this.CommandList[i].StopLoss;
                        newOpenTrade.Swap = this.CommandList[i].Swap;
                        newOpenTrade.Symbol = this.CommandList[i].Symbol;
                        newOpenTrade.TakeProfit = this.CommandList[i].TakeProfit;
                        newOpenTrade.Taxes = this.CommandList[i].Taxes;
                        newOpenTrade.Type = this.CommandList[i].Type;
                        newOpenTrade.ClosePrice = this.CommandList[i].ClosePrice;
                        newOpenTrade.Profit = this.CommandList[i].Profit;
                        newOpenTrade.CloseTime = this.CommandList[i].CloseTime;

                        listBackupData.Add(newOpenTrade);
                        #endregion
                    }
                }
            }

            if (listBackupData != null)
            {
                TradingServer.DBW.DBWOnlineCommand newDBWOnlineCommand = new DBW.DBWOnlineCommand();
                bool resultUpdate = newDBWOnlineCommand.MultipleUpdateOpenTrade(listBackupData, command.StopLoss, command.TakeProfit);
                if (!resultUpdate)
                {
                    //rollback data
                    RollBackData(listBackupData, 2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        internal bool MultipleUpdateCommand(Business.OpenTrade command, double valueStopLoss, double valueTakeProfit)
        {
            bool result = false;
            if (this.CommandList != null)
            {
                int count = this.CommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.CommandList[i].Symbol.Name == command.Symbol.Name && this.CommandList[i].ID == command.ID)
                    {
                        string content = string.Empty;
                        string comment = "[multiple modified symbol]";
                        string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].Size.ToString(), 2);
                        string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].OpenPrice.ToString(), this.CommandList[i].Symbol.Digit);
                        string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].StopLoss.ToString(), this.CommandList[i].Symbol.Digit);
                        string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(this.CommandList[i].TakeProfit.ToString(), this.CommandList[i].Symbol.Digit);
                        string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(this.CommandList[i].Type.ID).ToLower();

                        bool IsBuy = false;
                        string commandType = "SellSpotCommand";
                        if (command.Type.ID == 1 || command.Type.ID == 11)
                        {
                            IsBuy = true;
                            commandType = "BuySpotCommand";
                        }

                        //CHECK S/L AND T/P
                        if (valueStopLoss != 0 || valueTakeProfit != 0)
                        {
                            bool ResultCheckLimit = false;

                            ResultCheckLimit = this.CommandList[i].Symbol.CheckLimitAndStop(this.CommandList[i].Symbol.Name, this.CommandList[i].Type.ID,
                                valueStopLoss, valueTakeProfit, this.CommandList[i].Symbol.StopLossTakeProfitLevel,
                                this.CommandList[i].Symbol.Digit, int.Parse(command.SpreaDifferenceInOpenTrade.ToString()));

                            if (!ResultCheckLimit)
                                return false;
                        }

                        bool updateCommandExe = TradingServer.Facade.FacadeUpdateCommandExecutor(this.CommandList[i].ID, valueStopLoss, valueTakeProfit, command.OpenPrice);
                        bool updateCommandSymbolList = TradingServer.Facade.FacadeUpdateCommandSymbolList(this.CommandList[i].ID, valueStopLoss, valueTakeProfit, command.OpenPrice);

                        if (updateCommandExe && updateCommandSymbolList)
                        {
                            this.CommandList[i].StopLoss = valueStopLoss;
                            this.CommandList[i].TakeProfit = valueTakeProfit;

                            #region Map Command Server To Client
                            //if (this.ClientCommandQueue == null)
                            //    this.ClientCommandQueue = new List<string>();

                            //string Message = "UpdateCommand$True,Update Command Complete," + this.CommandList[i].ID + "," +
                            //    this.CommandList[i].Investor.InvestorID + "," + this.CommandList[i].Symbol.Name + "," + this.CommandList[i].Size + "," +
                            //    IsBuy + "," + this.CommandList[i].OpenTime + "," + this.CommandList[i].OpenPrice + "," + this.CommandList[i].StopLoss + "," +
                            //    this.CommandList[i].TakeProfit + "," + this.CommandList[i].ClosePrice + "," + this.CommandList[i].Commission + "," +
                            //    this.CommandList[i].Swap + "," + this.CommandList[i].Profit + "," + "Comment," + this.CommandList[i].ID + "," + commandType + "," + 1 + "," +
                            //    this.CommandList[i].ExpTime + "," + this.CommandList[i].ClientCode + "," + this.CommandList[i].CommandCode + "," +
                            //    this.CommandList[i].IsHedged + "," + this.CommandList[i].Type.ID + "," + this.CommandList[i].Margin + ",UpdatePendingOrder";

                            //this.ClientCommandQueue.Add(Message);
                            #endregion

                            #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                            string afterSL = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(valueStopLoss.ToString(), this.CommandList[i].Symbol.Digit);
                            string afterTP = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(valueTakeProfit.ToString(), this.CommandList[i].Symbol.Digit);
                            string tempContent = string.Empty;
                            tempContent = "'" + this.Code + "':multiple modified #" + this.CommandList[i].CommandCode + " " + mode + " " + size + " " +
                                this.CommandList[i].Symbol.Name + " at " + openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " -> sl " + afterSL + " tp " + afterTP + " successful";

                            TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, command.Investor.IpAddress, this.Code);
                            #endregion

                            result = true;
                        }
                        else
                        {
                            TradingServer.Facade.FacadeAddNewSystemLog(1, "command don't exists in command exe or symbol list", "[multiple command]", "", "");
                            result = false;
                            break;
                        }

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// REMOVE ONLINE COMMAND IN INVESTOR COMMAND LIST
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal bool RemoveOnlineCommand(int CommandID, int InvestorID)
        {
            bool Result = false;
            if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                        {
                            int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                            for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].ID == CommandID)
                                {
                                    Business.Market.InvestorList[i].CommandList.RemoveAt(j);
                                    Result = true;
                                    break;
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
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateUserConfig(int investorID, string userConfig)
        {
            bool result = false;
            result = Investor.DBWInvestorInstance.UpdateUserConfig(investorID, userConfig);
            if (result)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            Business.Market.InvestorList[i].UserConfig = userConfig;
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
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateUserConfigIpad(int investorID, string userConfig)
        {
            bool result = false;
            result = Investor.DBWInvestorInstance.UpdateUserConfigIpad(investorID, userConfig);
            if (result)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            Business.Market.InvestorList[i].UserConfigIpad = userConfig;
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
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateUserConfigIphone(int investorID, string userConfig)
        {
            bool result = false;
            result = Investor.DBWInvestorInstance.UpdateUserConfigIphone(investorID, userConfig);
            if (result)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            Business.Market.InvestorList[i].UserConfigIphone = userConfig;
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
        /// <param name="Money"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool Deposit(double Money, int InvestorID, string Comment)
        {
            bool Result = false;
            Money = Math.Round(Money, 2);

            if (Money <= 0)
                return false;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        //Calculation Balance
                        double Balance = 0;
                        double balanceBefore = Business.Market.InvestorList[i].Balance;
                        Balance = Business.Market.InvestorList[i].Balance + Money;

                        //Update Balance In Database
                        bool UpdateBalance = false;
                        UpdateBalance = TradingServer.Facade.FacadeUpdateBalance(InvestorID, Balance);

                        Business.Market.InvestorList[i].TotalDeposit += Money;

                        if (UpdateBalance == false)
                        {
                            return false;
                        }
                        else
                        {
                            //Update Balance In Market                        
                            Business.Market.InvestorList[i].Balance = Balance;

                            //Call Function Calculation All Account Of Investor
                            Business.Market.InvestorList[i].ReCalculationAccount();

                            //Send Command To Client, Clinet Will Get Account Again
                            if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                            string Message = "AddDeposit$True,Add Deposit Complete";

                            //int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                            //if (countOnline > 0)
                            Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);

                            //SEND COMMAND TO AGENT SERVER
                            string msg = Message + "," + Money + "," + InvestorID;
                            Business.AgentNotify newAgentNotify = new AgentNotify();
                            newAgentNotify.NotifyMessage = msg;
                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);

                            Result = true;

                            //SEND NOTIFY TO MANAGER THEN ADD DEPOSIT
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                        }

                        Business.InvestorAccountLog newInvestorAccountLog = new InvestorAccountLog();
                        newInvestorAccountLog.Name = Business.Market.InvestorList[i].NickName;
                        newInvestorAccountLog.InvestorID = Business.Market.InvestorList[i].InvestorID;
                        newInvestorAccountLog.Date = DateTime.Now;
                        newInvestorAccountLog.Comment = Comment;
                        newInvestorAccountLog.Amount = Money;
                        newInvestorAccountLog.Code = "ADP01";

                        //int ResultLog = TradingServer.Facade.FacadeAddInvestorAccountLog(newInvestorAccountLog);

                        //string CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(ResultLog.ToString());

                        //TradingServer.Facade.FacadeUpdateDealID(ResultLog, CommandCode);

                        //ADD HISTORY DEPOSIT TO TABLE COMMAND HISTORY
                        int resultAddHistory = TradingServer.Facade.FacadeAddNewCommandHistory(InvestorID, 13, "", DateTime.Now, 0, DateTime.Now, 0, Money, 0, 0, DateTime.Now, 0, 0, 0, "", -1, 0, 0, Comment, "9", 0, -1, "", false, false);

                        if (resultAddHistory > 0)
                        {
                            string commandHistory = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(resultAddHistory.ToString());

                            TradingServer.Facade.FacadeUpdateCommandCodeHistory(commandHistory, resultAddHistory);
                        }

                        string content = string.Empty;
                        string strBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(balanceBefore.ToString(), 2);
                        string strMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                        string strBalanceAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].Balance.ToString(), 2);
                        content = "'" + Business.Market.InvestorList[i].Code + "': balance " + strBalance + " deposit " + strMoney + " -> " + strBalanceAfter;
                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, "[deposit account]", "", Business.Market.InvestorList[i].Code);

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Money"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool AddCredit(double Money, int InvestorID, string Comment)
        {
            bool Result = false;

            Money = Math.Round(Money, 2);
            if (Money <= 0)
                return false;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        double Credit = 0;
                        double creditBefore = Business.Market.InvestorList[i].Credit;
                        Credit = Business.Market.InvestorList[i].Credit + Money;

                        //Call Function Update Credit In Database
                        bool UpdateCredit = false;
                        UpdateCredit = this.UpdateCredit(InvestorID, Credit);

                        if (UpdateCredit == true)
                        {
                            //Update Credit In Class Market
                            Business.Market.InvestorList[i].Credit = Credit;

                            //Call Function Calculation Account
                            Business.Market.InvestorList[i].ReCalculationAccount();

                            if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                            string Message = "AddCredit$True,Add Credit Complete";
                            //int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                            //if (countOnline > 0)
                            Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);

                            //SEND COMMAND TO AGENT SERVER
                            string msg = Message + "," + Money + "," + InvestorID;
                            Business.AgentNotify newAgentNotify = new AgentNotify();
                            newAgentNotify.NotifyMessage = msg;
                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);

                            Business.InvestorAccountLog newInvestorAccountLog = new InvestorAccountLog();
                            newInvestorAccountLog.Name = Business.Market.InvestorList[i].NickName;
                            newInvestorAccountLog.InvestorID = Business.Market.InvestorList[i].InvestorID;
                            newInvestorAccountLog.Date = DateTime.Now;
                            newInvestorAccountLog.Comment = Comment;
                            newInvestorAccountLog.Amount = Money;
                            newInvestorAccountLog.Code = "ACD01";

                            //int ResultLog = TradingServer.Facade.FacadeAddInvestorAccountLog(newInvestorAccountLog);

                            //string CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(ResultLog.ToString());

                            //TradingServer.Facade.FacadeUpdateDealID(ResultLog, CommandCode);

                            Result = true;

                            //SEND NOTIFY TO MANAGER THEN ADD CREDIT
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);

                            int resultAddCommandHistory = TradingServer.Facade.FacadeAddNewCommandHistory(InvestorID, 15, "", DateTime.Now, 0, DateTime.Now, 0, Money, 0, 0, DateTime.Now, 0, 0, 0, "", -1, 0, 0, Comment, "10", 0, -1, "", false, false);
                            if (resultAddCommandHistory > 0)
                            {
                                string commandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(resultAddCommandHistory.ToString());
                                TradingServer.Facade.FacadeUpdateCommandCodeHistory(commandCode, resultAddCommandHistory);
                            }
                        }
                        else
                        {
                            Result = false;
                        }

                        string content = string.Empty;
                        string strBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(creditBefore.ToString(), 2);
                        string strMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                        string strCreditAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].Credit.ToString(), 2);
                        content = "'" + Business.Market.InvestorList[i].Code + "': credit " + strBalance + " credit in " + strMoney + " -> " + strCreditAfter;
                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, "[credit in account]", "", Business.Market.InvestorList[i].Code);

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mont"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool SubCredit(double Money, int InvestorID, string Comment)
        {
            bool Result = false;

            Money = Math.Round(Money, 2);

            if (Money <= 0)
                return false;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        double Credit = 0;
                        double tempCredit = 0;
                        double creditBefore = Business.Market.InvestorList[i].Credit;
                        tempCredit = Math.Round(Business.Market.InvestorList[i].Credit, 2) - Money;

                        Credit = (Business.Market.InvestorList[i].Balance + tempCredit);

                        if (tempCredit >= 0)
                        {
                            if (this.InvestorGroupInstance != null)
                            {
                                //RECALCULATION ACCOUNT
                                this.ReCalculationAccount();
                            }
                            else
                            {
                                #region RECALCULATION INVESTOR ACCOUNT
                                if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                {
                                    //Call function Total Margin Of Investor
                                    Business.Margin newMargin = new Business.Margin();
                                    newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                    Business.Market.InvestorList[i].Margin = newMargin.TotalMargin;
                                    Business.Market.InvestorList[i].FreezeMargin = newMargin.TotalFreezeMargin;

                                    Business.Market.InvestorList[i].Profit = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalProfit(Business.Market.InvestorList[i].CommandList);
                                    Business.Market.InvestorList[i].Equity = Credit + Business.Market.InvestorList[i].Profit;
                                    double Loss = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalLoss(Business.Market.InvestorList[i].CommandList);
                                    double Profit = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalProfitPositive(Business.Market.InvestorList[i].CommandList);
                                    int Method = -1;
                                    switch (Business.Market.InvestorList[i].InvestorGroupInstance.FreeMargin)
                                    {
                                        case "do not use unrealized profit/loss":
                                            Method = 0;
                                            break;
                                        case "use unrealized profit/loss":
                                            Method = 1;
                                            break;
                                        case "use unrealized profit only":
                                            Method = 2;
                                            break;
                                        case "use unrealized loss only":
                                            Method = 3;
                                            break;
                                    }

                                    double totalMargin = Business.Market.InvestorList[i].Margin + Business.Market.InvestorList[i].FreezeMargin;
                                    Business.Market.InvestorList[i].FreeMargin = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalFreeMargin(totalMargin, Credit, Business.Market.InvestorList[i].Equity, Profit, Loss, Method);
                                    Business.Market.InvestorList[i].MarginLevel = (Business.Market.InvestorList[i].Equity * 100) / (Business.Market.InvestorList[i].Margin + Business.Market.InvestorList[i].FreezeMargin);
                                }
                                else
                                {
                                    Business.Market.InvestorList[i].Margin = 0;
                                    Business.Market.InvestorList[i].FreeMargin = 0;
                                    Business.Market.InvestorList[i].MarginLevel = 0;
                                    Business.Market.InvestorList[i].Profit = 0;
                                }

                                //Check Stop Out Level Of Account
                                if (Business.Market.InvestorList[i].MarginLevel <= Business.Market.InvestorList[i].InvestorGroupInstance.MarginStopOut) 
                                {
                                    string comment = "so: " + Math.Round(Business.Market.InvestorList[i].MarginLevel, 2) + "%/" + Business.Market.InvestorList[i].Equity + "/" + Business.Market.InvestorList[i].Margin;

                                    //Call Function Close Command
                                    if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                    {
                                        for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                        {
                                            bool isTrade = TradingServer.Facade.FacadeCheckStatusSymbol(Business.Market.InvestorList[i].CommandList[j].Symbol.Name.Trim());
                                            if (isTrade == true)
                                            {
                                                bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.InvestorList[i].CommandList[j].Type.ID);
                                                if (isPending)
                                                    Business.Market.InvestorList[i].CommandList[j].Profit = 0;

                                                #region Command Is Close
                                                int ResultHistory = -1;
                                                //Add Command To Command History
                                                Business.Market.InvestorList[i].CommandList[j].Comment = comment;

                                                if (isPending)
                                                {
                                                    ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID,
                                                    Business.Market.InvestorList[i].CommandList[j].Type.ID, Business.Market.InvestorList[i].CommandList[j].CommandCode,
                                                    Business.Market.InvestorList[i].CommandList[j].OpenTime, Business.Market.InvestorList[i].CommandList[j].OpenPrice,
                                                    Business.Market.InvestorList[i].CommandList[j].CloseTime, Business.Market.InvestorList[i].CommandList[j].ClosePrice,
                                                    0, Business.Market.InvestorList[i].CommandList[j].Swap,
                                                    Business.Market.InvestorList[i].CommandList[j].Commission, Business.Market.InvestorList[i].CommandList[j].ExpTime,
                                                    Business.Market.InvestorList[i].CommandList[j].Size, Business.Market.InvestorList[i].CommandList[j].StopLoss,
                                                    Business.Market.InvestorList[i].CommandList[j].TakeProfit, Business.Market.InvestorList[i].CommandList[j].ClientCode,
                                                    Business.Market.InvestorList[i].CommandList[j].Symbol.SymbolID,
                                                    Business.Market.InvestorList[i].CommandList[j].Taxes,
                                                    Business.Market.InvestorList[i].CommandList[j].AgentCommission, /*Business.Market.InvestorList[i].CommandList[j].Comment*/comment, "12",
                                                    Business.Market.InvestorList[i].CommandList[j].TotalSwap,
                                                    Business.Market.InvestorList[i].CommandList[j].RefCommandID,
                                                    Business.Market.InvestorList[i].CommandList[j].AgentRefConfig,
                                                    Business.Market.InvestorList[i].CommandList[j].IsActivePending,
                                                    Business.Market.InvestorList[i].CommandList[j].IsStopLossAndTakeProfit);
                                                }
                                                else
                                                {
                                                    ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID,
                                                    Business.Market.InvestorList[i].CommandList[j].Type.ID, Business.Market.InvestorList[i].CommandList[j].CommandCode,
                                                    Business.Market.InvestorList[i].CommandList[j].OpenTime, Business.Market.InvestorList[i].CommandList[j].OpenPrice,
                                                    Business.Market.InvestorList[i].CommandList[j].CloseTime, Business.Market.InvestorList[i].CommandList[j].ClosePrice,
                                                    Business.Market.InvestorList[i].CommandList[j].Profit, Business.Market.InvestorList[i].CommandList[j].Swap,
                                                    Business.Market.InvestorList[i].CommandList[j].Commission, Business.Market.InvestorList[i].CommandList[j].ExpTime,
                                                    Business.Market.InvestorList[i].CommandList[j].Size, Business.Market.InvestorList[i].CommandList[j].StopLoss,
                                                    Business.Market.InvestorList[i].CommandList[j].TakeProfit, Business.Market.InvestorList[i].CommandList[j].ClientCode,
                                                    Business.Market.InvestorList[i].CommandList[j].Symbol.SymbolID,
                                                    Business.Market.InvestorList[i].CommandList[j].Taxes,
                                                    Business.Market.InvestorList[i].CommandList[j].AgentCommission, /*Business.Market.InvestorList[i].CommandList[j].Comment*/comment, "12",
                                                    Business.Market.InvestorList[i].CommandList[j].TotalSwap,
                                                    Business.Market.InvestorList[i].CommandList[j].RefCommandID,
                                                    Business.Market.InvestorList[i].CommandList[j].AgentRefConfig,
                                                    Business.Market.InvestorList[i].CommandList[j].IsActivePending,
                                                    Business.Market.InvestorList[i].CommandList[j].IsStopLossAndTakeProfit);
                                                }


                                                if (ResultHistory > 0)
                                                {
                                                    //Log Stop Out
                                                    string content = string.Empty;
                                                    string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(Business.Market.InvestorList[i].CommandList[j].Type.ID).ToLower();
                                                    string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].CommandList[j].Size.ToString(), 2);
                                                    string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].CommandList[j].OpenPrice.ToString(),
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.Digit);

                                                    content = "'" + Business.Market.InvestorList[i].CommandList[j].Investor.Code + "': stop out #" +
                                                        Business.Market.InvestorList[i].CommandList[j].CommandCode + " " + mode + " " + size + " " +
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.Name + " " + openPrice + " (" +
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.TickValue.Bid + "/" +
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.TickValue.Ask + ")";

                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[stop out]", "", Business.Market.InvestorList[i].Code);

                                                    #region COMMAND CODE(REMOVE COMAMND IN COMMAND EXECUTOR AND SYMBOL LIST
                                                    ////Remove Command In Symbol List
                                                    //bool resultRemoveCommandSymbol = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(Business.Market.InvestorList[i].CommandList[j].ID);

                                                    ////Remove Command In Command Executor
                                                    //bool deleteCommandExe = TradingServer.Facade.FacadeRemoveOpenTradeInCommandExecutor(Business.Market.InvestorList[i].CommandList[j].ID);
                                                    #endregion

                                                    //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                    //Business.OpenTrade newOpenTrade = this.CommandList[i];
                                                    Business.OpenRemove newOpenRemove = new OpenRemove();
                                                    newOpenRemove.InvestorID = this.InvestorID;
                                                    newOpenRemove.OpenTradeID = Business.Market.InvestorList[i].CommandList[j].ID;
                                                    newOpenRemove.SymbolName = Business.Market.InvestorList[i].CommandList[j].Symbol.Name;
                                                    newOpenRemove.IsExecutor = true;
                                                    newOpenRemove.IsSymbol = true;
                                                    newOpenRemove.IsInvestor = false;
                                                    Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                    if (!isPending)
                                                    {
                                                        double totalProfit = Math.Round(Business.Market.InvestorList[i].CommandList[j].Profit + Business.Market.InvestorList[i].CommandList[j].Commission - Business.Market.InvestorList[i].CommandList[j].Swap, 2);
                                                        Business.Market.InvestorList[i].Balance += totalProfit;
                                                        //Update Balance Of Investor Account
                                                        this.UpdateBalance(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID, Business.Market.InvestorList[i].Balance);
                                                    }

                                                    //Update Command In Database
                                                    TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.InvestorList[i].CommandList[j].ID);

                                                    //Close Command Complete Add Message To Client
                                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                    #region Map Command Server To Client
                                                    string Message = "StopOut$True,Close Command Complete," + Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Size + "," + false + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Commission + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Swap + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Profit + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Comment + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Type.Name + "," +
                                                        1 + "," + Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Margin + ",Close";

                                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                    //int countInvestorOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                                                    //if (countInvestorOnline > 0)
                                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);


                                                    //SEND COMMAND TO AGENT SERVER

                                                    string msg = "StopOut$True,Close Command Complete," + Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Size + "," + false + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Commission + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Swap + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Profit + "," + "Comment," +
                                                            Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Type.Name + "," +
                                                            1 + "," + Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Margin +
                                                            ",Close," + Business.Market.InvestorList[i].CommandList[j].CloseTime;

                                                    msg += Business.Market.InvestorList[i].CommandList[j].AgentRefConfig;

                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = msg;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Business.Market.InvestorList[i].InvestorGroupInstance);

                                                    #endregion

                                                    //SEND NOTIFY TO MANAGER
                                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.InvestorList[i].CommandList[j]);

                                                    lock (Business.Market.syncObject)
                                                    {
                                                        //Remove Command In Investor List                        
                                                        Business.Market.InvestorList[i].CommandList.Remove(Business.Market.InvestorList[i].CommandList[j]);
                                                    }

                                                    if (Business.Market.InvestorList[i].CommandList.Count > 0 && Business.Market.InvestorList[i].CommandList != null)
                                                    {
                                                        //Call function Total Margin Of Investor                                                            
                                                        Business.Margin totalMargin = new Business.Margin();
                                                        totalMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                                        Business.Market.InvestorList[i].Margin = totalMargin.TotalMargin;
                                                        Business.Market.InvestorList[i].FreezeMargin = totalMargin.TotalFreezeMargin;

                                                        Business.Market.InvestorList[i].Profit = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalProfit(Business.Market.InvestorList[i].CommandList);
                                                        Business.Market.InvestorList[i].Equity = Business.Market.InvestorList[i].Balance +
                                                            Business.Market.InvestorList[i].Credit + Business.Market.InvestorList[i].Profit;
                                                    }
                                                    else
                                                    {
                                                        Business.Market.InvestorList[i].Margin = 0;
                                                        Business.Market.InvestorList[i].Profit = 0;
                                                        Business.Market.InvestorList[i].Equity = 0;
                                                    }

                                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                                                }
                                                else
                                                {
                                                    #region Map Command Server To Client
                                                    string Message = "StopOut$False,Can't Add Command To Database," +
                                                        Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Size + "," + false + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Commission + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Swap + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Profit + "," + "Comment," +
                                                        Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Type.Name + "," + 1 + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                                        Business.Market.InvestorList[i].CommandList[j].Margin + ",Close";

                                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);
                                                    #endregion
                                                }
                                                #endregion

                                                j--;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            //Call Function Update Credit In Database
                            bool UpdateCredit = false;
                            UpdateCredit = this.UpdateCredit(InvestorID, tempCredit);

                            if (UpdateCredit)
                            {
                                //Update Credit In Class Market
                                Business.Market.InvestorList[i].Credit = tempCredit;

                                if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                    Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                string Message = "SubCredit$True,Add Credit Complete";
                                //int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                                //if (countOnline > 0)
                                Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);

                                //SEND COMMAND TO AGENT SERVER
                                string msg = Message + "," + Money + "," + InvestorID;
                                Business.AgentNotify newAgentNotify = new AgentNotify();
                                newAgentNotify.NotifyMessage = msg;
                                TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);

                                Business.InvestorAccountLog newInvestorAccountLog = new InvestorAccountLog();
                                newInvestorAccountLog.Name = Business.Market.InvestorList[i].NickName;
                                newInvestorAccountLog.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                newInvestorAccountLog.Date = DateTime.Now;
                                newInvestorAccountLog.Comment = Comment;
                                newInvestorAccountLog.Amount = Money;
                                newInvestorAccountLog.Code = "CRD01";

                                //int ResultLog = TradingServer.Facade.FacadeAddInvestorAccountLog(newInvestorAccountLog);

                                //string CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(ResultLog.ToString());

                                //TradingServer.Facade.FacadeUpdateDealID(ResultLog, CommandCode);

                                //SEND NOTIFY TO MANAGER THEN SUB CREDIT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                            }

                            int resultAddCommandHistory = TradingServer.Facade.FacadeAddNewCommandHistory(InvestorID, 16, "", DateTime.Now, 0, DateTime.Now, 0, Money, 0, 0, DateTime.Now, 0, 0, 0, "", -1, 0, 0, Comment, "13", 0, -1, "", false, false);
                            if (resultAddCommandHistory > 0)
                            {
                                string commandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(resultAddCommandHistory.ToString());
                                TradingServer.Facade.FacadeUpdateCommandCodeHistory(commandCode, resultAddCommandHistory);
                            }

                            string strContent = string.Empty;
                            string strBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(creditBefore.ToString(), 2);
                            string strMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                            string strCreditAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].Credit.ToString(), 2);
                            strContent = "'" + Business.Market.InvestorList[i].Code + "': credit " + strBalance + " credit out " + strMoney + " -> " + strCreditAfter;
                            TradingServer.Facade.FacadeAddNewSystemLog(3, strContent, "[credit out account]", "", Business.Market.InvestorList[i].Code);

                            return true;
                        }
                        else
                        {
                            string strContent = string.Empty;
                            string strBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(creditBefore.ToString(), 2);
                            string strMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                            string strCreditAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].Credit.ToString(), 2);
                            strContent = "'" + Business.Market.InvestorList[i].Code + "': credit " + strBalance + " credit out " + strMoney + " -> " + strCreditAfter;
                            TradingServer.Facade.FacadeAddNewSystemLog(3, strContent, "[credit out account]", "", Business.Market.InvestorList[i].Code);

                            return false;
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        public bool WithDrawals(int InvestorID, double Money, string Comment)
        {
            bool Result = false;

            Money = Math.Round(Money, 2);

            if (Money <= 0)
                return false;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        double Balance = 0;
                        double balanceBefore = Business.Market.InvestorList[i].Balance;
                        double tempBalance = Math.Round(Business.Market.InvestorList[i].Balance, 2) - Money;
                        Balance = (tempBalance + Business.Market.InvestorList[i].Credit);

                        if (tempBalance >= 0)
                        {
                            #region WITHDRAWALS MONEY > BALANCE
                            if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                            {
                                #region RECALCULATION ACCOUNT
                                //ReCalculation Account If Account Valid Then WithRaw Complete Else Then WithRaw False
                                Business.Margin newMargin = new Business.Margin();
                                double Margin = 0;
                                newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                Margin = newMargin.TotalMargin;

                                double Profit = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalProfit(Business.Market.InvestorList[i].CommandList);
                                double Equity = Balance + Profit;
                                double Loss = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalLoss(Business.Market.InvestorList[i].CommandList);
                                double ProfitPositive = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalProfitPositive(Business.Market.InvestorList[i].CommandList);
                                int Method = -1;
                                switch (Business.Market.InvestorList[i].InvestorGroupInstance.FreeMargin)
                                {
                                    case "do not use unrealized profit/loss":
                                        Method = 0;
                                        break;
                                    case "use unrealized profit/loss":
                                        Method = 1;
                                        break;
                                    case "use unrealized profit only":
                                        Method = 2;
                                        break;
                                    case "use unrealized loss only":
                                        Method = 3;
                                        break;
                                }


                                double totalMargin = Business.Market.InvestorList[i].Margin + Business.Market.InvestorList[i].FreezeMargin;

                                double FreeMargin = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalFreeMargin(totalMargin, Balance, Equity, ProfitPositive, Loss, Method);
                                double MarginLevel = (Equity * 100) / (Margin + Business.Market.InvestorList[i].FreezeMargin);
                                #endregion

                                if (FreeMargin > 0)
                                {
                                    #region Valid Account Will WithRawals Complete
                                    bool UpdateBalance = false;
                                    //Update Balance In Database
                                    UpdateBalance = TradingServer.Facade.FacadeUpdateBalance(InvestorID, tempBalance);

                                    if (UpdateBalance == true)
                                    {
                                        //Set Balance Of Investor 
                                        Business.Market.InvestorList[i].Balance = tempBalance;

                                        if (this.InvestorGroupInstance != null)
                                        {
                                            //ReCalculation Account Of Investor
                                            Business.Market.InvestorList[i].ReCalculationAccount();
                                        }
                                        else
                                        {
                                            if (MarginLevel <= Business.Market.InvestorList[i].InvestorGroupInstance.MarginStopOut)
                                            {
                                                for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                                {
                                                    #region Command Is Close
                                                    int ResultHistory = -1;
                                                    //set time close
                                                    Business.Market.InvestorList[i].CommandList[j].CloseTime = DateTime.Now;

                                                    //Add Command To Command History

                                                    ResultHistory = TradingServer.Facade.FacadeAddNewCommandHistory(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID,
                                                        Business.Market.InvestorList[i].CommandList[j].Type.ID,
                                                        Business.Market.InvestorList[i].CommandList[j].CommandCode,
                                                        Business.Market.InvestorList[i].CommandList[j].OpenTime,
                                                        Business.Market.InvestorList[i].CommandList[j].OpenPrice,
                                                        Business.Market.InvestorList[i].CommandList[j].CloseTime,
                                                        Business.Market.InvestorList[i].CommandList[j].ClosePrice,
                                                        Business.Market.InvestorList[i].CommandList[j].Profit,
                                                        Business.Market.InvestorList[i].CommandList[j].Swap,
                                                        Business.Market.InvestorList[i].CommandList[j].Commission,
                                                        Business.Market.InvestorList[i].CommandList[j].ExpTime,
                                                        Business.Market.InvestorList[i].CommandList[j].Size,
                                                        Business.Market.InvestorList[i].CommandList[j].StopLoss,
                                                        Business.Market.InvestorList[i].CommandList[j].TakeProfit,
                                                        Business.Market.InvestorList[i].CommandList[j].ClientCode,
                                                        Business.Market.InvestorList[i].CommandList[j].Symbol.SymbolID,
                                                        Business.Market.InvestorList[i].CommandList[j].Taxes,
                                                        Business.Market.InvestorList[i].CommandList[j].AgentCommission,
                                                        Business.Market.InvestorList[i].CommandList[j].Comment, "16",
                                                        Business.Market.InvestorList[i].CommandList[j].TotalSwap,
                                                        Business.Market.InvestorList[i].CommandList[j].RefCommandID,
                                                        Business.Market.InvestorList[i].CommandList[j].AgentRefConfig,
                                                        Business.Market.InvestorList[i].CommandList[j].IsActivePending,
                                                        Business.Market.InvestorList[i].CommandList[j].IsStopLossAndTakeProfit);

                                                    if (ResultHistory > 0)
                                                    {
                                                        //Log Stop Out
                                                        string content = string.Empty;
                                                        string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(Business.Market.InvestorList[i].CommandList[j].Type.ID).ToLower();
                                                        string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].CommandList[j].Size.ToString(), 2);
                                                        string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].CommandList[j].OpenPrice.ToString(),
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Digit);
                                                        content = "'" + Business.Market.InvestorList[i].CommandList[j].Investor.Code + "': stop out #" +
                                                            Business.Market.InvestorList[i].CommandList[j].CommandCode + " " + mode + " " + size + " " +
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Name + " " + openPrice + " (" +
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.TickValue.Bid + "/" +
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.TickValue.Ask + ")";

                                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[stop out]", "", this.Code);

                                                        #region COMMENT CODE(REMOVE COMMAND IN COMMAND EXECUTOR AND SYMBOL LIST
                                                        ////Remove Command In Symbol List
                                                        //bool resultRemoveCommandSymbol = TradingServer.Facade.FacadeRemoveOpenTradeInCommandList(Business.Market.InvestorList[i].CommandList[j].ID);

                                                        ////Remove Command In Command Executor
                                                        //bool deleteCommandExe = TradingServer.Facade.FacadeRemoveOpenTradeInCommandExecutor(Business.Market.InvestorList[i].CommandList[j].ID);
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

                                                        double totalProfit = Math.Round(Business.Market.InvestorList[i].CommandList[j].Profit + Business.Market.InvestorList[i].CommandList[j].Commission - Business.Market.InvestorList[i].CommandList[j].Swap, 2);
                                                        Business.Market.InvestorList[i].Balance += totalProfit;

                                                        //Update Balance Of Investor Account
                                                        this.UpdateBalance(Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID, Business.Market.InvestorList[j].Balance);

                                                        //Update Command In Database                             
                                                        TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.InvestorList[i].CommandList[j].ID);

                                                        //Close Command Complete Add Message To Client
                                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                        #region Map Command Server To Client
                                                        string Message = "StopOut$True,Close Command Complete," +
                                                            Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Size + "," + false + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Commission + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Swap + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Profit + "," + "Comment," +
                                                            Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Type.Name + "," + 1 + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Margin + ",Close," +
                                                            Business.Market.InvestorList[i].CommandList[j].CloseTime;

                                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                        Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);
                                                        #endregion

                                                        //SEND NOTIFY TO MANAGER
                                                        TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.InvestorList[i].CommandList[j]);

                                                        lock (Business.Market.syncObject)
                                                        {
                                                            //Remove Command In Investor List                        
                                                            Business.Market.InvestorList[i].CommandList.Remove(Business.Market.InvestorList[i].CommandList[j]);
                                                        }

                                                        if (Business.Market.InvestorList[i].CommandList.Count > 0 && Business.Market.InvestorList[i].CommandList != null)
                                                        {
                                                            //Call function Total Margin Of Investor                                                            
                                                            Business.Margin totalMarginAccount = new Business.Margin();
                                                            totalMarginAccount = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                                            Business.Market.InvestorList[i].Margin = totalMarginAccount.TotalMargin;
                                                            Business.Market.InvestorList[i].FreezeMargin = totalMarginAccount.TotalFreezeMargin;

                                                            Business.Market.InvestorList[i].Profit = Business.Market.InvestorList[i].InvestorGroupInstance.CalculationTotalProfit(Business.Market.InvestorList[i].CommandList);
                                                            Business.Market.InvestorList[i].Equity = Business.Market.InvestorList[i].Balance +
                                                                Business.Market.InvestorList[i].Credit + Business.Market.InvestorList[i].Profit;
                                                        }
                                                        else
                                                        {
                                                            Business.Market.InvestorList[i].Margin = 0;
                                                            Business.Market.InvestorList[i].Profit = 0;
                                                            Business.Market.InvestorList[i].Equity = 0;
                                                        }

                                                        TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                                                    }
                                                    else
                                                    {
                                                        #region Map Command Server To Client
                                                        string Message = "CloseCommand$False,Can't Add Command To Database," + Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Investor.InvestorID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.Name + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Size + "," + false + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].OpenTime + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].OpenPrice + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].StopLoss + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].TakeProfit + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ClosePrice + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Commission + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Swap + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Profit + "," + "Comment," +
                                                            Business.Market.InvestorList[i].CommandList[j].ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Type.Name + "," + 1 + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ExpTime + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].ClientCode + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].CommandCode + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].IsHedged + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Type.ID + "," +
                                                            Business.Market.InvestorList[i].CommandList[j].Margin + ",Close," +
                                                            Business.Market.InvestorList[i].CommandList[j].CloseTime;

                                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                        Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);
                                                        #endregion
                                                    }
                                                    #endregion

                                                    j--;
                                                }
                                            }
                                        }

                                        //Send Command To Client
                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                        string SmS = "WithDrawals$True,WithDrawals Complete";

                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                        //int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                                        //if (countOnline > 0)
                                        Business.Market.InvestorList[i].ClientCommandQueue.Add(SmS);

                                        //SEND COMMAND TO AGENT SERVER
                                        string msg = SmS + "," + Money + "," + InvestorID;
                                        Business.AgentNotify newAgentNotify = new AgentNotify();
                                        newAgentNotify.NotifyMessage = msg;
                                        TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);

                                        Business.InvestorAccountLog newInvestorAccountLog = new InvestorAccountLog();
                                        newInvestorAccountLog.Name = Business.Market.InvestorList[i].NickName;
                                        newInvestorAccountLog.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                        newInvestorAccountLog.Date = DateTime.Now;
                                        newInvestorAccountLog.Comment = Comment;
                                        newInvestorAccountLog.Amount = Money;
                                        newInvestorAccountLog.Code = "WRD01";

                                        //int ResultLog = TradingServer.Facade.FacadeAddInvestorAccountLog(newInvestorAccountLog);

                                        //string CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(ResultLog.ToString());

                                        //TradingServer.Facade.FacadeUpdateDealID(ResultLog, CommandCode);

                                        Result = true;

                                        //SEND NOTIFY TO MANAGER THEN WITHDRAWALS
                                        TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                                    }
                                    else
                                    {
                                        Result = false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    Result = false;
                                }
                            }
                            else
                            {
                                #region Valid Account Will WithRawals Complete
                                bool UpdateBalance = false;
                                //Update Balance In Database
                                UpdateBalance = TradingServer.Facade.FacadeUpdateBalance(InvestorID, tempBalance);

                                if (UpdateBalance == true)
                                {
                                    //Set Balance Of Investor 
                                    Business.Market.InvestorList[i].Balance = tempBalance;

                                    //ReCalculation Account Of Investor
                                    Business.Market.InvestorList[i].ReCalculationAccount();

                                    //Send Command To Client
                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    string Message = "WithDrawals$True,WithDrawals Complete";

                                    //int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                                    //if (countOnline > 0)
                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);

                                    //SEND COMMAND TO AGENT SERVER
                                    string msg = Message + "," + Money + "," + InvestorID;
                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = msg;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);

                                    Business.InvestorAccountLog newInvestorAccountLog = new InvestorAccountLog();
                                    newInvestorAccountLog.Name = Business.Market.InvestorList[i].NickName;
                                    newInvestorAccountLog.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                    newInvestorAccountLog.Date = DateTime.Now;
                                    newInvestorAccountLog.Comment = Comment;
                                    newInvestorAccountLog.Amount = Money;
                                    newInvestorAccountLog.Code = "WRD01";

                                    //int ResultLog = TradingServer.Facade.FacadeAddInvestorAccountLog(newInvestorAccountLog);

                                    //string CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(ResultLog.ToString());

                                    //TradingServer.Facade.FacadeUpdateDealID(ResultLog, CommandCode);

                                    Result = true;

                                    //SEND NOTIFY TO MANAGER THEN WITHDRAWALS
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                                }
                                else
                                {
                                    Result = false;
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            Result = false;
                        }

                        //Check If Result = true Then Insert To Database In Table Command History
                        if (Result)
                        {
                            int resultAddCommandHistory = TradingServer.Facade.FacadeAddNewCommandHistory(InvestorID, 14, "", DateTime.Now, 0, DateTime.Now, 0, Money, 0, 0, DateTime.Now, 0, 0, 0, "", -1, 0, 0, Comment, "14", 0, -1, "", false, false);
                            if (resultAddCommandHistory > 0)
                            {
                                string commandCodeHistory = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(resultAddCommandHistory.ToString());
                                TradingServer.Facade.FacadeUpdateCommandCodeHistory(commandCodeHistory, resultAddCommandHistory);
                            }
                        }

                        string strContent = string.Empty;
                        string strBalance = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(balanceBefore.ToString(), 2);
                        string strMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                        string strCreditAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.InvestorList[i].Balance.ToString(), 2);
                        strContent = "'" + Business.Market.InvestorList[i].Code + "': balance " + strBalance + " withdrawals " + strMoney + " -> " + strCreditAfter;
                        TradingServer.Facade.FacadeAddNewSystemLog(3, strContent, "[credit in account]", "", Business.Market.InvestorList[i].Code);

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// AFTER LOGIN COMPLETE THEN CALL FUNCTION GET IGROUPSECURITY, IGROUPSYMBOL, LISTSYMBOL, CLIENTCONFIG, ALERT OF INVESTOR
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="index"></param>
        /// <param name="investorGroup"></param>
        private void GetSymbolOfInvestor(Business.Investor Result, int index, int investorGroup)
        {
            if (Result != null && Result.InvestorID > 0)
            {
                //GET SETTING OF INVESTOR
                Result.ListIGroupSecurity = this.GetIGroupSecurity(investorGroup);
                Result.ListIGroupSymbol = this.GetIGroupSymbol(investorGroup);
                Result.ListSymbol = this.GetSymbolOfInvestor(Result.ListIGroupSecurity);
                Result.ClientConfigInstance = this.GetClientConfig(investorGroup);
                Result.InvestorIndex = index;

                //Notify to Manager
                TradingServer.Facade.FacadeSendNotifyManagerRequest(1, Result);

                #region GET ALERT OF INVESTOR
                if (Result.AlertQueue == null)
                {
                    Result.AlertQueue = new List<PriceAlert>();
                    for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                    {
                        if (Business.Market.SymbolList[i].AlertQueue != null)
                        {
                            for (int j = 0; j < Business.Market.SymbolList[i].AlertQueue.Count; j++)
                            {
                                if (Result.InvestorID == Business.Market.SymbolList[i].AlertQueue[j].InvestorID)
                                {
                                    Result.AlertQueue.Add(Business.Market.SymbolList[i].AlertQueue[j]);
                                }
                            }
                        }
                        else
                        {
                            Business.Market.SymbolList[i].AlertQueue = new List<PriceAlert>();
                        }
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="typeLogin"></param>
        /// <returns></returns>
        internal bool CheckPrimaryInvestorOnline(int investorID, Business.TypeLogin typeLogin)
        {
            bool result = false;
            if (Business.Market.InvestorOnline != null)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginType == typeLogin)
                    {
                        if (Business.Market.InvestorOnline[i].IsOnline)
                        {
                            Business.Market.InvestorOnline[i].ClientCommandQueue.Add("OLOFF14790251");
                            result = true;
                        }
                        //comment break, can't remove
                        //break;
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
        internal int CountInvestorOnline(int investorID)
        {
            int result = 0;
            if (Business.Market.InvestorOnline != null)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].ConnectType == 1)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="typeLogin"></param>
        /// <returns></returns>
        public bool CheckPrimaryInvestorOnline(int investorID, Business.TypeLogin typeLogin, string key)
        {
            bool result = false;
            if (Business.Market.InvestorOnline != null)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginType == typeLogin &&
                        Business.Market.InvestorOnline[i].LoginKey == key)
                    {
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
        /// <param name="loginKey"></param>
        /// <returns></returns>
        internal bool CheckOnlineInvestor(int investorID, string loginKey)
        {
            bool result = false;
            bool isExits = false;
            Business.Investor InstanceInvestor = null;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == investorID)
                    {
                        //save instance investor
                        InstanceInvestor = Business.Market.InvestorList[i];

                        if (Business.Market.InvestorList[i].LoginKey == loginKey)
                        {
                            Business.Investor newInvestor = new Business.Investor();
                            newInvestor.InvestorID = investorID;
                            newInvestor.LastConnectTime = DateTime.Now;
                            newInvestor.numTimeOut = 30;
                            newInvestor.TickInvestor = new List<Business.Tick>();
                            newInvestor.Code = Business.Market.InvestorList[i].Code;
                            newInvestor.IsLogout = false;
                            newInvestor.LoginKey = loginKey;
                            newInvestor.LoginType = TypeLogin.Primary;
                            newInvestor.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;

                            //set islogout for investor list
                            Business.Market.InvestorList[i].IsLogout = false;

                            if (Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable && !Business.Market.InvestorList[i].IsDisable)
                            {
                                Business.Market.InvestorOnline.Add(newInvestor);
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(1, newInvestor);
                            }

                            isExits = true;
                            result = true;

                            break;
                        }
                    }
                }
            }

            if (!isExits)
            {
                if (InstanceInvestor != null && InstanceInvestor.InvestorID > 0)
                {
                    bool flag = false;
                    if (Business.Market.InvestorOnline != null)
                    {
                        int count = Business.Market.InvestorOnline.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == loginKey)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }

                    if (!flag)
                    {
                        Business.Investor newInvestor = new Business.Investor();
                        newInvestor.InvestorID = investorID;
                        newInvestor.LastConnectTime = DateTime.Now;
                        newInvestor.numTimeOut = 30;
                        newInvestor.TickInvestor = new List<Business.Tick>();
                        newInvestor.Code = InstanceInvestor.Code;
                        newInvestor.IsLogout = false;
                        newInvestor.LoginKey = loginKey;
                        newInvestor.LoginType = TypeLogin.ReadOnly;
                        newInvestor.InvestorGroupInstance = InstanceInvestor.InvestorGroupInstance;

                        ////set islogout for investor list
                        //Business.Market.InvestorList[i].IsLogout = false;

                        Business.Market.InvestorOnline.Add(newInvestor);
                        TradingServer.Facade.FacadeSendNotifyManagerRequest(1, newInvestor);

                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="typeLogin"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool SendCommandToInvestorOnline(int investorID, Business.TypeLogin typeLogin, string value)
        {
            bool result = false;
            if (Business.Market.InvestorOnline != null)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginType == typeLogin)
                    {
                        Business.Market.InvestorOnline[i].ClientCommandQueue.Add(value);
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="key"></param>
        /// <param name="tempResult"></param>
        internal StringBuilder AddCommandToInvestorOnline(int investorID, string key, List<string> messages)
        {
            StringBuilder result = new StringBuilder();
            if (Business.Market.InvestorOnline != null)
            {
                for (int n = 0; n < Business.Market.InvestorOnline.Count; n++)
                {
                    if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey != key)
                    {
                        if (messages != null)
                        {
                            int countMessage = messages.Count;
                            for (int m = 0; m < countMessage; m++)
                            {
                                Business.Market.InvestorOnline[n].ClientCommandQueue.Add(messages[m]);
                            }
                        }
                    }
                    else
                    {
                        if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey == key)
                        {
                            if (Business.Market.InvestorOnline[n].ClientCommandQueue != null)
                            {
                                for (int m = 0; m < Business.Market.InvestorOnline[n].ClientCommandQueue.Count; m++)
                                {
                                    try
                                    {
                                        if (Business.Market.InvestorOnline[n].ClientCommandQueue[0] != null)
                                        {
                                            string _temp = Business.Market.InvestorOnline[n].ClientCommandQueue[0];
                                            result.Append(_temp);
                                            result.Append("▼");

                                            Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                        }
                                        else
                                        {
                                            Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        
                                    }
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
        /// <param name="investorID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckInvestorOnline(int investorID, string key)
        {
            bool result = false;
            if (Business.Market.InvestorOnline != null && Business.Market.InvestorOnline.Count > 0)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                    {
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
        public int CountPrimaryInvestor(int investorID)
        {
            int result = 0;
            if (Business.Market.InvestorOnline != null)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        internal bool UpdateInvestor(Business.Investor objInvestor)
        {
            bool Result = false;
            StringBuilder content = new StringBuilder();
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == objInvestor.InvestorID)
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            Business.Market.InvestorList[i].InvestorStatusID = objInvestor.InvestorStatusID;

                            #region Find Investor Group
                            if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID != objInvestor.InvestorGroupInstance.InvestorGroupID)
                            {
                                #region IF COMMAND LIST IN INVESTOR > 0 THEN UPDATE INVESTOR GROUP FALSE(DMD)
                                if (Business.Market.InvestorList[i].CommandList.Count > 0)
                                {
                                    return false;
                                }
                                #endregion

                                beforeContent.Append("group: " + Business.Market.InvestorList[i].InvestorGroupInstance.Name + " - ");

                                if (Business.Market.InvestorGroupList != null)
                                {
                                    int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                                    for (int j = 0; j < countInvestorGroup; j++)
                                    {
                                        if (Business.Market.InvestorGroupList[j].InvestorGroupID == objInvestor.InvestorGroupInstance.InvestorGroupID)
                                        {
                                            Business.Market.InvestorList[i].InvestorGroupInstance = Business.Market.InvestorGroupList[j];
                                            beforeContent.Append("group: " + Business.Market.InvestorGroupList[j].Name + " - ");
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (!string.IsNullOrEmpty(objInvestor.PrimaryPwd))
                                Business.Market.InvestorList[i].PrimaryPwd = objInvestor.PrimaryPwd;

                            if (!string.IsNullOrEmpty(objInvestor.ReadOnlyPwd))
                                Business.Market.InvestorList[i].ReadOnlyPwd = objInvestor.ReadOnlyPwd;

                            if (!string.IsNullOrEmpty(objInvestor.PhonePwd))
                                Business.Market.InvestorList[i].PhonePwd = objInvestor.PhonePwd;

                            Business.Market.InvestorList[i].AgentID = objInvestor.AgentID;

                            if (Business.Market.InvestorList[i].IsDisable != objInvestor.IsDisable)
                            {
                                if (Business.Market.InvestorList[i].IsDisable)
                                    beforeContent.Append("disable account: on");
                                else
                                    beforeContent.Append("disable account: off");

                                if (objInvestor.IsDisable)
                                    afterContent.Append("disable account: on");
                                else
                                    afterContent.Append("disable account: off");

                                Business.Market.InvestorList[i].IsDisable = objInvestor.IsDisable;
                            }

                            if (Business.Market.InvestorList[i].AllowChangePwd != objInvestor.AllowChangePwd)
                            {
                                if (Business.Market.InvestorList[i].AllowChangePwd)
                                    beforeContent.Append("allow changed password: on - ");
                                else
                                    beforeContent.Append("allow changed password: off - ");

                                if (objInvestor.AllowChangePwd)
                                    afterContent.Append("allow changed password: on - ");
                                else
                                    afterContent.Append("allow changed password: off - ");

                                Business.Market.InvestorList[i].AllowChangePwd = objInvestor.AllowChangePwd;
                            }

                            if (Business.Market.InvestorList[i].SendReport != objInvestor.SendReport)
                            {
                                if (Business.Market.InvestorList[i].SendReport)
                                    beforeContent.Append("enable send report: on - ");
                                else
                                    beforeContent.Append("enable send report: off - ");

                                if (objInvestor.SendReport)
                                    afterContent.Append("enable send report: on - ");
                                else
                                    afterContent.Append("enable send report: off - ");

                                Business.Market.InvestorList[i].SendReport = objInvestor.SendReport;
                            }

                            if (Business.Market.InvestorList[i].ReadOnly != objInvestor.ReadOnly)
                            {
                                if (Business.Market.InvestorList[i].ReadOnly)
                                    beforeContent.Append("enable read only account: on - ");
                                else
                                    beforeContent.Append("enable read only account: off - ");

                                if (objInvestor.ReadOnly)
                                    afterContent.Append("enable read only account: on - ");
                                else
                                    afterContent.Append("enalbe read only account: off - ");

                                Business.Market.InvestorList[i].ReadOnly = objInvestor.ReadOnly;
                            }

                            if (Business.Market.InvestorList[i].InvestorComment != objInvestor.InvestorComment)
                            {
                                beforeContent.Append("investor command: " + Business.Market.InvestorList[i].InvestorComment + " - ");
                                afterContent.Append("investor command: " + objInvestor.InvestorComment + " - ");

                                Business.Market.InvestorList[i].InvestorComment = objInvestor.InvestorComment;
                            }
                            

                            if (Business.Market.InvestorList[i].Leverage != objInvestor.Leverage)
                            {
                                beforeContent.Append("leverage: " + Business.Market.InvestorList[i].Leverage + " - ");
                                afterContent.Append("leverage: " + objInvestor.Leverage + " - ");

                                Business.Market.InvestorList[i].Leverage = objInvestor.Leverage;

                                if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                {
                                    int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                                    for (int j = 0; j < countCommand; j++)
                                    {
                                        Business.Market.InvestorList[i].CommandList[j].CalculatorMarginCommand(Business.Market.InvestorList[i].CommandList[j]);
                                    }

                                    Business.Margin newMargin = new Business.Margin();
                                    newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                    Business.Market.InvestorList[i].Margin = newMargin.TotalMargin;
                                    Business.Market.InvestorList[i].FreezeMargin = newMargin.TotalFreezeMargin;

                                    string message = "CSW5789";
                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(message);
                                }
                            }

                            if (Business.Market.InvestorList[i].TaxRate != objInvestor.TaxRate)
                            {
                                beforeContent.Append("taxrate: " + Business.Market.InvestorList[i].TaxRate + " - ");
                                afterContent.Append("taxrate: " + objInvestor.TaxRate + " - ");

                                Business.Market.InvestorList[i].TaxRate = objInvestor.TaxRate;
                            }

                            if (Business.Market.InvestorList[i].IDPassport != objInvestor.IDPassport)
                            {
                                beforeContent.Append("ID passport: " + Business.Market.InvestorList[i].IDPassport + " - ");
                                afterContent.Append("ID passport: " + objInvestor.IDPassport + " - ");

                                Business.Market.InvestorList[i].IDPassport = objInvestor.IDPassport;
                            }

                            Investor.DBWInvestorInstance.UpdateInvestor(objInvestor);

                            ts.Complete();
                            Result = true;

                            //NOTIFY TO MANAGER THEN INVESTOR ACCOUNT UPDATE
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                        }

                        break;
                    }
                }
            }

            if (Result == false)
            {
                Investor.DBWInvestorInstance.UpdateInvestor(objInvestor);
                Result = true;
            }



            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        internal bool UpdateInvestor(Business.Investor objInvestor,string ipaddress,string code)
        {
            bool Result = false;
            StringBuilder content = new StringBuilder();
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();

            content.Append("'" + code + "': update investor ");

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == objInvestor.InvestorID)
                    {
                        content.Append("'" + Business.Market.InvestorList[i].Code + "': ");
                        using (TransactionScope ts = new TransactionScope())
                        {
                            Business.Market.InvestorList[i].InvestorStatusID = objInvestor.InvestorStatusID;

                            #region Find Investor Group
                            if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID != objInvestor.InvestorGroupInstance.InvestorGroupID)
                            {
                                beforeContent.Append("group: " + Business.Market.InvestorList[i].InvestorGroupInstance.Name + " - ");

                                if (Business.Market.InvestorGroupList != null)
                                {
                                    int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                                    for (int j = 0; j < countInvestorGroup; j++)
                                    {
                                        if (Business.Market.InvestorGroupList[j].InvestorGroupID == objInvestor.InvestorGroupInstance.InvestorGroupID)
                                        {
                                            Business.Market.InvestorList[i].InvestorGroupInstance = Business.Market.InvestorGroupList[j];
                                            afterContent.Append("group: " + Business.Market.InvestorGroupList[j].Name + " - ");
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region UPDATE PASSWORD
                            if (!string.IsNullOrEmpty(objInvestor.PrimaryPwd))
                                Business.Market.InvestorList[i].PrimaryPwd = objInvestor.PrimaryPwd;

                            if (!string.IsNullOrEmpty(objInvestor.ReadOnlyPwd))
                                Business.Market.InvestorList[i].ReadOnlyPwd = objInvestor.ReadOnlyPwd;

                            if (!string.IsNullOrEmpty(objInvestor.PhonePwd))
                                Business.Market.InvestorList[i].PhonePwd = objInvestor.PhonePwd;
                            #endregion

                            #region UPDATE PROPERTY AGENT ID
                            Business.Market.InvestorList[i].AgentID = objInvestor.AgentID;
                            #endregion

                            #region UPDATE PROPERTY ISDISABLE ACCOUNT
                            if (Business.Market.InvestorList[i].IsDisable != objInvestor.IsDisable)
                            {
                                if (Business.Market.InvestorList[i].IsDisable)
                                    beforeContent.Append("disable account: on - ");
                                else
                                    beforeContent.Append("disable account: off - ");

                                if (objInvestor.IsDisable)
                                    afterContent.Append("disable account: on - ");
                                else
                                    afterContent.Append("disable account: off - ");

                                Business.Market.InvestorList[i].IsDisable = objInvestor.IsDisable;
                            }
                            #endregion

                            #region UPDATE PROPERTY ALLOW CHANGE PASSWORD
                            if (Business.Market.InvestorList[i].AllowChangePwd != objInvestor.AllowChangePwd)
                            {
                                if (Business.Market.InvestorList[i].AllowChangePwd)
                                    beforeContent.Append("allow changed password: on - ");
                                else
                                    beforeContent.Append("allow changed password: off - ");

                                if (objInvestor.AllowChangePwd)
                                    afterContent.Append("allow changed password: on - ");
                                else
                                    afterContent.Append("allow changed password: off - ");

                                Business.Market.InvestorList[i].AllowChangePwd = objInvestor.AllowChangePwd;
                            }
                            #endregion

                            #region UPDATE PROPERTY SEND REPORT
                            if (Business.Market.InvestorList[i].SendReport != objInvestor.SendReport)
                            {
                                if (Business.Market.InvestorList[i].SendReport)
                                    beforeContent.Append("enable send report: on - ");
                                else
                                    beforeContent.Append("enable send report: off - ");

                                if (objInvestor.SendReport)
                                    afterContent.Append("enable send report: on - ");
                                else
                                    afterContent.Append("enable send report: off - ");

                                Business.Market.InvestorList[i].SendReport = objInvestor.SendReport;
                            }
                            #endregion

                            #region UPDATE PROPERTY READONLY
                            if (Business.Market.InvestorList[i].ReadOnly != objInvestor.ReadOnly)
                            {
                                if (Business.Market.InvestorList[i].ReadOnly)
                                    beforeContent.Append("enable read only account: on - ");
                                else
                                    beforeContent.Append("enable read only account: off - ");

                                if (objInvestor.ReadOnly)
                                    afterContent.Append("enable read only account: on - ");
                                else
                                    afterContent.Append("enalbe read only account: off - ");

                                Business.Market.InvestorList[i].ReadOnly = objInvestor.ReadOnly;
                            }
                            #endregion

                            #region UPDATE PROPERTY INVESTOR COMMENT
                            if (Business.Market.InvestorList[i].InvestorComment != objInvestor.InvestorComment)
                            {
                                beforeContent.Append("investor comment: " + Business.Market.InvestorList[i].InvestorComment + " - ");
                                afterContent.Append("investor comment: " + objInvestor.InvestorComment + " - ");

                                Business.Market.InvestorList[i].InvestorComment = objInvestor.InvestorComment;
                            }
                            #endregion

                            #region UPDATE PROPERTY LEVERAGE
                            if (Business.Market.InvestorList[i].Leverage != objInvestor.Leverage)
                            {
                                beforeContent.Append("leverage: " + Business.Market.InvestorList[i].Leverage + " - ");
                                afterContent.Append("leverage: " + objInvestor.Leverage + " - ");

                                Business.Market.InvestorList[i].Leverage = objInvestor.Leverage;

                                if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                {
                                    int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                                    for (int j = 0; j < countCommand; j++)
                                    {
                                        Business.Market.InvestorList[i].CommandList[j].CalculatorMarginCommand(Business.Market.InvestorList[i].CommandList[j]);
                                    }

                                    Business.Margin newMargin = new Business.Margin();
                                    newMargin = Business.Market.InvestorList[i].CommandList[0].Symbol.CalculationTotalMargin(Business.Market.InvestorList[i].CommandList);
                                    Business.Market.InvestorList[i].Margin = newMargin.TotalMargin;
                                    Business.Market.InvestorList[i].FreezeMargin = newMargin.TotalFreezeMargin;

                                    string message = "CSW5789";
                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(message);
                                }
                            }
                            #endregion

                            #region UPDATE PROPERTY TAXRATE
                            if (Business.Market.InvestorList[i].TaxRate != objInvestor.TaxRate)
                            {
                                beforeContent.Append("taxrate: " + Business.Market.InvestorList[i].TaxRate + " - ");
                                afterContent.Append("taxrate: " + objInvestor.TaxRate + " - ");

                                Business.Market.InvestorList[i].TaxRate = objInvestor.TaxRate;
                            }
                            #endregion

                            #region UPDATE PROPERTY IDPASSPORT
                            if (Business.Market.InvestorList[i].IDPassport != objInvestor.IDPassport)
                            {
                                beforeContent.Append("ID passport: " + Business.Market.InvestorList[i].IDPassport + " - ");
                                afterContent.Append("ID passport: " + objInvestor.IDPassport + " - ");

                                Business.Market.InvestorList[i].IDPassport = objInvestor.IDPassport;
                            }
                            #endregion

                            Investor.DBWInvestorInstance.UpdateInvestor(objInvestor);

                            ts.Complete();
                            Result = true;

                            //NOTIFY TO MANAGER THEN INVESTOR ACCOUNT UPDATE
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);

                            #region IF COMMAND LIST IN INVESTOR > 0 THEN UPDATE INVESTOR GROUP FALSE(DMD)
                            //if (Business.Market.InvestorList[i].CommandList.Count > 0)
                            //{
                            //    content.Append("[falied] exists open postion");
                            //    TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update investor]", ipaddress, code);

                            //    return false;
                            //}
                            //else
                            //{
                                
                            //}
                            #endregion
                        }

                        break;
                    }
                }
            }

            if (Result == false)
            {
                Investor.DBWInvestorInstance.UpdateInvestor(objInvestor);
                Result = true;
            }

            string tempBeforeContent = beforeContent.ToString();
            if (tempBeforeContent.EndsWith(" - "))
                tempBeforeContent = tempBeforeContent.Remove(tempBeforeContent.Length - 2, 2);

            string tempAftertConten = afterContent.ToString();
            if (tempAftertConten.EndsWith(" - "))
                tempAftertConten = tempAftertConten.Remove(tempAftertConten.Length - 2, 2);

            if (!string.IsNullOrEmpty(tempBeforeContent.Trim()) && !string.IsNullOrEmpty(tempAftertConten.Trim()))
            {
                content.Append(tempBeforeContent + " -> " + tempAftertConten);
                TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update investor]", ipaddress, code);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        /// <returns></returns>
        internal bool UpdateInvestorProfile(Business.Investor objInvestorProfile)
        {            
            bool Result = false;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorProfileID == objInvestorProfile.InvestorProfileID)
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            Business.Market.InvestorList[i].Address = objInvestorProfile.Address;
                            Business.Market.InvestorList[i].Phone = objInvestorProfile.Phone;
                            Business.Market.InvestorList[i].City = objInvestorProfile.City;
                            Business.Market.InvestorList[i].Country = objInvestorProfile.Country;
                            Business.Market.InvestorList[i].Email = objInvestorProfile.Email;
                            Business.Market.InvestorList[i].ZipCode = objInvestorProfile.ZipCode;

                            //COMMENT(DON'T UPDATE REGISTER DAY NO NEED)21-03-2011
                            //Business.Market.InvestorList[i].RegisterDay = objInvestorProfile.RegisterDay;
                            Business.Market.InvestorList[i].InvestorComment = objInvestorProfile.InvestorComment;
                            Business.Market.InvestorList[i].State = objInvestorProfile.State;
                            Business.Market.InvestorList[i].NickName = objInvestorProfile.NickName;
                            Business.Market.InvestorList[i].IDPassport = objInvestorProfile.IDPassport;

                            Investor.DBWInvestorProfile.UpdateInvestorProfile(objInvestorProfile);

                            ts.Complete();

                            Result = true;
                        }

                        break;
                    }
                }
            }

            if (Result == false)
            {
                Investor.DBWInvestorProfile.UpdateInvestorProfile(objInvestorProfile);

                Result = true;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        /// <returns></returns>
        internal bool UpdateInvestorProfile(Business.Investor objInvestorProfile,string ipAddress,string code)
        {
            StringBuilder content = new StringBuilder();
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();

            content.Append("'" + code + "': update investor profile ");

            bool Result = false;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorProfileID == objInvestorProfile.InvestorProfileID)
                    {
                        content.Append("'" + Business.Market.InvestorList[i].Code + "': ");
                        using (TransactionScope ts = new TransactionScope())
                        {
                            if (Business.Market.InvestorList[i].Address.Trim() != objInvestorProfile.Address.Trim())
                            {
                                beforeContent.Append("address: " + Business.Market.InvestorList[i].Address + " - ");
                                afterContent.Append("address: " + objInvestorProfile.Address + " - ");

                                Business.Market.InvestorList[i].Address = objInvestorProfile.Address;
                            }

                            if (Business.Market.InvestorList[i].Phone.Trim() != objInvestorProfile.Phone.Trim())
                            {
                                beforeContent.Append("phone: " + Business.Market.InvestorList[i].Phone + " - ");
                                afterContent.Append("phone: " + objInvestorProfile.Phone + " - ");
                                Business.Market.InvestorList[i].Phone = objInvestorProfile.Phone;
                            }

                            if (Business.Market.InvestorList[i].City.Trim() != objInvestorProfile.City.Trim())
                            {
                                beforeContent.Append("city: " + Business.Market.InvestorList[i].City + " - ");
                                afterContent.Append("city: " + objInvestorProfile.City + " - ");

                                Business.Market.InvestorList[i].City = objInvestorProfile.City;
                            }

                            if (Business.Market.InvestorList[i].Country.Trim() != objInvestorProfile.Country.Trim())
                            {
                                beforeContent.Append("country: " + Business.Market.InvestorList[i].Country + " - ");
                                afterContent.Append("country: " + objInvestorProfile.Country + " - ");
                                Business.Market.InvestorList[i].Country = objInvestorProfile.Country;
                            }

                            if (Business.Market.InvestorList[i].Email.Trim() != objInvestorProfile.Email.Trim())
                            {
                                beforeContent.Append("email: " + Business.Market.InvestorList[i].Email + " - ");
                                afterContent.Append("email: " + objInvestorProfile.Email + " - ");
                                Business.Market.InvestorList[i].Email = objInvestorProfile.Email;
                            }

                            if (Business.Market.InvestorList[i].ZipCode.Trim() != objInvestorProfile.ZipCode.Trim())
                            {
                                beforeContent.Append("zip code: " + Business.Market.InvestorList[i].ZipCode + " - ");
                                afterContent.Append("zip code: " + objInvestorProfile.ZipCode + " - ");
                                Business.Market.InvestorList[i].ZipCode = objInvestorProfile.ZipCode;
                            }

                            //COMMENT(DON'T UPDATE REGISTER DAY NO NEED)21-03-2011
                            //Business.Market.InvestorList[i].RegisterDay = objInvestorProfile.RegisterDay;
                            if (Business.Market.InvestorList[i].InvestorComment.Trim() != objInvestorProfile.InvestorComment.Trim())
                            {
                                beforeContent.Append("investor comment: " + Business.Market.InvestorList[i].InvestorComment + " - ");
                                afterContent.Append("investor comment: " + objInvestorProfile.InvestorComment + " - ");
                                Business.Market.InvestorList[i].InvestorComment = objInvestorProfile.InvestorComment;
                            }

                            if (Business.Market.InvestorList[i].State.Trim() != objInvestorProfile.State.Trim())
                            {
                                beforeContent.Append("state: " + Business.Market.InvestorList[i].State + " - ");
                                afterContent.Append("state: " + objInvestorProfile.State + " - ");
                                Business.Market.InvestorList[i].State = objInvestorProfile.State;
                            }

                            if (Business.Market.InvestorList[i].NickName.Trim() != objInvestorProfile.NickName.Trim())
                            {
                                beforeContent.Append("nick name: " + Business.Market.InvestorList[i].NickName + " - ");
                                afterContent.Append("nick name: " + objInvestorProfile.NickName + " - ");
                                Business.Market.InvestorList[i].NickName = objInvestorProfile.NickName;
                            }

                            if (Business.Market.InvestorList[i].IDPassport.ToUpper().Trim() != objInvestorProfile.IDPassport.ToUpper().Trim())
                            {
                                beforeContent.Append("ID passport: " + Business.Market.InvestorList[i].IDPassport + " - ");
                                afterContent.Append("ID passport: " + objInvestorProfile.IDPassport + " - ");
                                Business.Market.InvestorList[i].IDPassport = objInvestorProfile.IDPassport;
                            }

                            Investor.DBWInvestorProfile.UpdateInvestorProfile(objInvestorProfile);

                            ts.Complete();

                            Result = true;
                        }

                        break;
                    }
                }
            }

            if (Result == false)
            {
                Investor.DBWInvestorProfile.UpdateInvestorProfile(objInvestorProfile);

                Result = true;
            }

            string tempBeforeContent = beforeContent.ToString();
            if (tempBeforeContent.EndsWith(" - "))
                tempBeforeContent = tempBeforeContent.Remove(tempBeforeContent.Length - 2, 2);

            string tempAfterContent = afterContent.ToString();
            if (tempAfterContent.EndsWith(" - "))
                tempAfterContent = tempAfterContent.Remove(tempAfterContent.Length - 2, 2);

            if (!string.IsNullOrEmpty(tempBeforeContent.Trim()) && !string.IsNullOrEmpty(tempAfterContent.Trim()))
            {
                content.Append(tempBeforeContent + " -> " + tempAfterContent);
                TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update investor profile]", ipAddress, code);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool ChangePrimaryPassword(int investorID, string oldPwd, string password)
        {           
            bool result = false;
            string hash = TradingServer.Model.ValidateCheck.Encrypt(password);
            //VERIFY OLD PASSWORD
            string hashOldPwd = TradingServer.Model.ValidateCheck.Encrypt(oldPwd);
            bool checkMasterPwd = TradingServer.Facade.FacadeCheckMasterPassword(investorID, hashOldPwd);

            if (checkMasterPwd)
            {
                if (Business.Market.IsConnectMT4)
                {
                    #region CONNECT WITH MT4
                    //get investor code
                    Business.Investor newInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(investorID);
                    if (newInvestor != null && newInvestor.InvestorID > 0)
                    {
                        string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertResetPasswordToString(newInvestor.Code, oldPwd, password);

                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].InvestorID == investorID)
                                {
                                    if (Business.Market.InvestorList[i].AllowChangePwd)
                                    {
                                        //string resultMT4 = Business.Market.InstanceSocket.SendToMT4(Business.Market.DEFAULT_IPADDRESS, Business.Market.DEFAULT_PORT, cmd);
                                        string resultMT4 = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);
                                        //string resultMT4 = LibraryAPI.CPPOut.Command(cmd);
                                        //string resultMT4 = Business.Market.InstanceGlobalDelegate.SendCommand(cmd);

                                        if (!string.IsNullOrEmpty(resultMT4))
                                        {
                                            string[] subValue = resultMT4.Split('$');
                                            if (subValue.Length == 2)
                                            {
                                                string[] subParameter = subValue[1].Split('{');
                                                if (subParameter.Length > 0)
                                                {
                                                    if (int.Parse(subParameter[0]) == 1)
                                                    {
                                                        result = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (result)
                                        {
                                            Business.Market.InvestorList[i].PrimaryPwd = hash;

                                            //UPDATE PRIMARY PASSWORD IN DATABASE
                                            TradingServer.Facade.FacadeUpdatePasswordByCode(Business.Market.InvestorList[i].Code, hash);
                                            result = true;
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region NO CONNECT WITH MT4
                    if (Business.Market.InvestorList != null)
                    {
                        int count = Business.Market.InvestorList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorList[i].InvestorID == investorID)
                            {
                                if (Business.Market.InvestorList[i].AllowChangePwd)
                                {
                                    Business.Market.InvestorList[i].PrimaryPwd = hash;
                                    result = true;
                                    break;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (result)
                {
                    result = Investor.DBWInvestorInstance.UpdatePrimaryPasword(investorID, hash);

                    //SEND NOTIFY TO AGENT SYSTEM 
                    //SEND COMMAND TO AGENT SERVER
                    string Message = "UpdatePrimaryPassword$" + result + "{" + hash + "{" + investorID;
                    Business.AgentNotify newAgentNotify = new AgentNotify();
                    newAgentNotify.NotifyMessage = Message;
                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                }
            }
            else
            {
                return false;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="oldPwd"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool ChangePrimaryPasswordMT4(int investorID, string oldPwd, string password)
        {
            bool result = false;
            string hash = TradingServer.Model.ValidateCheck.Encrypt(password);
            //VERIFY OLD PASSWORD
            string hashOldPwd = TradingServer.Model.ValidateCheck.Encrypt(oldPwd);

            if (Business.Market.IsConnectMT4)
            {
                #region CONNECT WITH MT4
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            if (Business.Market.InvestorList[i].PrimaryPwd == hash)
                                return true;

                            string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertResetPasswordToString(Business.Market.InvestorList[i].Code, oldPwd, password);

                            string resultMT4 = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                            if (!string.IsNullOrEmpty(resultMT4))
                            {
                                string[] subValue = resultMT4.Split('$');
                                if (subValue.Length == 2)
                                {
                                    string[] subParameter = subValue[1].Split('{');
                                    if (subParameter.Length > 0)
                                    {
                                        if (int.Parse(subParameter[0]) == 1)
                                        {
                                            result = true;
                                        }
                                    }
                                }
                            }

                            if (result)
                            {
                                //send command reset password mt mt4client
                                //string cmdMT4 = NJ4XConnectSocket.MapNJ4X.Instance.MapResetPassword(Business.Market.InvestorList[i].Code, password);
                                //string resultMT4Client = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(cmdMT4);

                                //string[] subClientMT4 = resultMT4Client.Split('$');
                                //bool resultClientMT4 = bool.Parse(subClientMT4[1]);

                                Business.Market.InvestorList[i].PrimaryPwd = hash;

                                //UPDATE PRIMARY PASSWORD IN DATABASE
                                TradingServer.Facade.FacadeUpdatePasswordByCode(Business.Market.InvestorList[i].Code, hash);
                                result = true;
                            }

                            break;
                        }
                    }
                }
                #endregion
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        internal bool ChangeReadOnlyPassword(int investorID, string oldPwd, string newPwd)
        {
            bool result = false;
            string hash = TradingServer.Model.ValidateCheck.Encrypt(newPwd);
            //VERIFY OLD PASSWORD
            string hashOldPwd = TradingServer.Model.ValidateCheck.Encrypt(oldPwd);
            //bool checkMasterPwd = TradingServer.Facade.FacadeCheckMasterPassword(investorID, hashOldPwd);

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == investorID)
                    {
                        if (Business.Market.InvestorList[i].AllowChangePwd)
                        {
                            if (Business.Market.InvestorList[i].ReadOnlyPwd == hashOldPwd)
                            {
                                bool updatePwd = Investor.DBWInvestorInstance.UpdatePrimaryPasword(investorID, hash);
                                if (updatePwd == true)
                                {
                                    Business.Market.InvestorList[i].ReadOnlyPwd = hash;
                                    result = true;

                                    //SEND NOTIFY TO AGENT SYSTEM 
                                    //SEND COMMAND TO AGENT SERVER
                                    string Message = "UpdateReadOnlyPassword$" + result + "{" + hash + "{" + investorID;
                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = Message;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                                }
                            }
                        }
                        else
                        {
                            return false;
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
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal int GetListUpdateCommandOfInvestor(int InvestorID)
        {
            int Result = -1;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        Result = Business.Market.InvestorList[i].UpdateCommands.Count;
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal string GetTaskNameOfInvestor(int InvestorID)
        {
            string Result = string.Empty;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        Result = Business.Market.InvestorList[i].TaskName;
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
        /// <param name="loginKey"></param>
        public void UpdateLastConnect(int investorID, string loginKey)
        {            
            if (Business.Market.InvestorOnline != null && Business.Market.InvestorOnline.Count > 0)
            {
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == loginKey)
                    {
                        Business.Market.InvestorOnline[i].numTimeOut = 30;

                        break;
                    }
                }
            }
        }
    }
}
