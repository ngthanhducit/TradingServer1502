using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// PROCESS NOTIFY FROM MT4(CASE CONNECT MT4)
        /// </summary>
        internal static void ProcessNotifyMessage()
        {
            string result = string.Empty;
            while (Business.Market.IsProcessNotifyMessage)
            {
                result = Business.Market.GetNotifyMessage();

                while (!string.IsNullOrEmpty(result))
                {
                    //Process Notify Message
                    string[] subCommand = result.Split('¬');

                    if (subCommand.Length > 0)
                    {
                        int count = subCommand.Length;
                        for (int i = 0; i < count; i++)
                        {
                            string[] subValue = subCommand[i].Split('$');

                            if (subValue.Length == 2)
                            {
                                switch (subValue[0])
                                {
                                    #region NOTIFY MAKE COMMAND
                                    case "MakeCommandNotify":
                                        {
                                            try
                                            {
                                                string[] subParameter = subValue[1].Split('{');
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[MakeCommandNotify]", "", subParameter[4]);

                                                string commandType = string.Empty;
                                                Business.OpenTrade resultOpenTrade = Business.Market.MapNotifyMakeCommand(subCommand[i]);

                                                if (resultOpenTrade.Investor == null || resultOpenTrade.Symbol == null || resultOpenTrade.Type == null)
                                                    continue;

                                                #region comment code
                                                if (Business.Market.NJ4XTickets != null)
                                                {
                                                    lock (Business.Market.nj4xObject)
                                                    {
                                                        bool isExitst = false;
                                                        int countNJ4XTicket = Business.Market.NJ4XTickets.Count;
                                                        for (int n = 0; n < countNJ4XTicket; n++)
                                                        {
                                                            if (Business.Market.NJ4XTickets[n].Code == resultOpenTrade.Investor.Code)
                                                            {
                                                                lock (Business.Market.nj4xObject)
                                                                    Business.Market.NJ4XTickets.RemoveAt(n);

                                                                #region get mql commands (log ipaddress)
                                                                if (Business.Market.marketInstance.MQLCommands != null)
                                                                {
                                                                    int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                                    for (int j = 0; j < countMQL; j++)
                                                                    {
                                                                        if (Business.Market.marketInstance.MQLCommands[j] != null)
                                                                        {
                                                                            if (Business.Market.marketInstance.MQLCommands[j].InvestorCode == resultOpenTrade.Investor.Code)
                                                                            {
                                                                                resultOpenTrade.IpAddress = Business.Market.marketInstance.MQLCommands[j].IpAddress;
                                                                                Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[j]);
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region process add new command
                                                                if (resultOpenTrade.RefCommandID > 0)
                                                                {
                                                                    bool IsBuy = Model.Helper.Instance.IsBuy(resultOpenTrade.Type.ID);
                                                                    commandType = Model.Helper.Instance.convertCommandTypeIDToString(resultOpenTrade.Type.ID);

                                                                    Business.OpenTrade newOpenTradeExe = new OpenTrade();
                                                                    Business.OpenTrade newOpenTradeSymbol = new OpenTrade();
                                                                    Business.OpenTrade newOpenTradeInvestor = new OpenTrade();

                                                                    #region SET INSTANCE IGROUPSECURITY, SYMBOL, TYPE, INVESTOR
                                                                    //set igroupsecurity
                                                                    newOpenTradeExe.IGroupSecurity = resultOpenTrade.IGroupSecurity;
                                                                    newOpenTradeInvestor.IGroupSecurity = resultOpenTrade.IGroupSecurity;
                                                                    newOpenTradeSymbol.IGroupSecurity = resultOpenTrade.IGroupSecurity;

                                                                    //set symbol
                                                                    newOpenTradeExe.Symbol = resultOpenTrade.Symbol;
                                                                    newOpenTradeInvestor.Symbol = resultOpenTrade.Symbol;
                                                                    newOpenTradeSymbol.Symbol = resultOpenTrade.Symbol;

                                                                    //set type
                                                                    newOpenTradeExe.Type = resultOpenTrade.Type;
                                                                    newOpenTradeInvestor.Type = resultOpenTrade.Type;
                                                                    newOpenTradeSymbol.Type = resultOpenTrade.Type;

                                                                    //set investor
                                                                    newOpenTradeExe.Investor = resultOpenTrade.Investor;
                                                                    newOpenTradeInvestor.Investor = resultOpenTrade.Investor;
                                                                    newOpenTradeSymbol.Investor = resultOpenTrade.Investor;
                                                                    #endregion

                                                                    #region GET SPREAD DIFFIRENCE FOR COMMAND
                                                                    //GET SPREAD DIFFRENCE OF OPEN TRADE
                                                                    double spreadDifference = 0;
                                                                    if (resultOpenTrade.IGroupSecurity != null)
                                                                    {
                                                                        if (resultOpenTrade.IGroupSecurity.IGroupSecurityConfig != null)
                                                                        {
                                                                            int countIGroupSecurity = resultOpenTrade.IGroupSecurity.IGroupSecurityConfig.Count;
                                                                            for (int j = 0; j < countIGroupSecurity; j++)
                                                                            {
                                                                                if (resultOpenTrade.IGroupSecurity.IGroupSecurityConfig[j].Code == "B11")
                                                                                {
                                                                                    double.TryParse(resultOpenTrade.IGroupSecurity.IGroupSecurityConfig[j].NumValue, out spreadDifference);
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    newOpenTradeExe.SpreaDifferenceInOpenTrade = spreadDifference;
                                                                    newOpenTradeInvestor.SpreaDifferenceInOpenTrade = spreadDifference;
                                                                    newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreadDifference;
                                                                    #endregion

                                                                    double tempClosePrice = 0;
                                                                    if (IsBuy)
                                                                        tempClosePrice = resultOpenTrade.Symbol.TickValue.Bid;
                                                                    else
                                                                        tempClosePrice = resultOpenTrade.Symbol.TickValue.Ask;

                                                                    resultOpenTrade.ClosePrice = tempClosePrice;

                                                                    #region NEW INSTANCES FOR COMMAND EXECUTOR
                                                                    newOpenTradeExe.AgentCommission = 0;
                                                                    newOpenTradeExe.ClientCode = resultOpenTrade.ClientCode;
                                                                    newOpenTradeExe.CloseTime = resultOpenTrade.OpenTime;
                                                                    //newOpenTradeExe.CommandCode = resultOpenTrade.CommandCode;
                                                                    newOpenTradeExe.Comment = resultOpenTrade.Comment;
                                                                    newOpenTradeExe.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeExe.Commission = resultOpenTrade.Commission;
                                                                    newOpenTradeExe.ExpTime = resultOpenTrade.ExpTime;
                                                                    newOpenTradeExe.FreezeMargin = 0;
                                                                    //newOpenTradeExe.ID = listOnlineCommand[i].OnlineCommandID;
                                                                    newOpenTradeExe.IsClose = false;
                                                                    newOpenTradeExe.OpenPrice = resultOpenTrade.OpenPrice;
                                                                    newOpenTradeExe.OpenTime = resultOpenTrade.OpenTime;
                                                                    newOpenTradeExe.Profit = resultOpenTrade.Profit;
                                                                    newOpenTradeExe.Size = resultOpenTrade.Size;
                                                                    newOpenTradeExe.StopLoss = resultOpenTrade.StopLoss;
                                                                    newOpenTradeExe.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeExe.TakeProfit = resultOpenTrade.TakeProfit;
                                                                    newOpenTradeExe.Taxes = 0;
                                                                    newOpenTradeExe.TotalSwap = 0;
                                                                    newOpenTradeExe.RefCommandID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeExe.ClosePrice = tempClosePrice;
                                                                    #endregion

                                                                    #region NEW INSTANCE FOR SYMBOL LIST
                                                                    newOpenTradeSymbol.AgentCommission = 0;
                                                                    newOpenTradeSymbol.ClientCode = resultOpenTrade.ClientCode;
                                                                    newOpenTradeSymbol.CloseTime = resultOpenTrade.OpenTime;
                                                                    //newOpenTradeSymbol.CommandCode = listOnlineCommand[i].CommandCode;
                                                                    newOpenTradeSymbol.Comment = resultOpenTrade.Comment;
                                                                    newOpenTradeInvestor.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeSymbol.Commission = resultOpenTrade.Commission;
                                                                    newOpenTradeSymbol.ExpTime = resultOpenTrade.ExpTime;
                                                                    newOpenTradeSymbol.FreezeMargin = 0;
                                                                    //newOpenTradeSymbol.ID = listOnlineCommand[i].OnlineCommandID;
                                                                    newOpenTradeSymbol.IsClose = true;
                                                                    newOpenTradeSymbol.OpenPrice = resultOpenTrade.OpenPrice;
                                                                    newOpenTradeSymbol.OpenTime = resultOpenTrade.OpenTime;
                                                                    newOpenTradeSymbol.Profit = resultOpenTrade.Profit;
                                                                    newOpenTradeSymbol.Size = resultOpenTrade.Size;
                                                                    newOpenTradeSymbol.StopLoss = resultOpenTrade.StopLoss;
                                                                    newOpenTradeSymbol.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeSymbol.TakeProfit = resultOpenTrade.TakeProfit;
                                                                    newOpenTradeSymbol.Taxes = 0;
                                                                    newOpenTradeSymbol.TotalSwap = 0;
                                                                    newOpenTradeSymbol.InsExe = newOpenTradeExe;
                                                                    newOpenTradeSymbol.RefCommandID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeSymbol.ClosePrice = tempClosePrice;
                                                                    #endregion

                                                                    #region NEW INSTANCE FOR INVESTOR LIST
                                                                    newOpenTradeInvestor.AgentCommission = 0;
                                                                    newOpenTradeInvestor.ClientCode = resultOpenTrade.ClientCode;
                                                                    newOpenTradeInvestor.CloseTime = resultOpenTrade.OpenTime;
                                                                    //newOpenTradeInvestor.CommandCode = listOnlineCommand[i].CommandCode;
                                                                    newOpenTradeInvestor.Comment = resultOpenTrade.Comment;
                                                                    newOpenTradeInvestor.Commission = resultOpenTrade.Commission;
                                                                    newOpenTradeInvestor.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeInvestor.ExpTime = resultOpenTrade.ExpTime;
                                                                    newOpenTradeInvestor.FreezeMargin = 0;
                                                                    //newOpenTradeInvestor.ID = listOnlineCommand[i].OnlineCommandID;
                                                                    newOpenTradeInvestor.IsClose = false;
                                                                    newOpenTradeInvestor.OpenPrice = resultOpenTrade.OpenPrice;
                                                                    newOpenTradeInvestor.OpenTime = resultOpenTrade.OpenTime;
                                                                    newOpenTradeInvestor.Profit = resultOpenTrade.Profit;
                                                                    newOpenTradeInvestor.Size = resultOpenTrade.Size;
                                                                    newOpenTradeInvestor.StopLoss = resultOpenTrade.StopLoss;
                                                                    newOpenTradeInvestor.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeInvestor.TakeProfit = resultOpenTrade.TakeProfit;
                                                                    newOpenTradeInvestor.Taxes = 0;
                                                                    newOpenTradeInvestor.TotalSwap = 0;
                                                                    newOpenTradeInvestor.InsExe = newOpenTradeExe;
                                                                    newOpenTradeInvestor.RefCommandID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeInvestor.ClosePrice = tempClosePrice;
                                                                    #endregion

                                                                    #region BUILD CLIENT CODE
                                                                    string clientCode = string.Empty;
                                                                    clientCode = resultOpenTrade.Investor.Code + "_" + DateTime.Now.Ticks;
                                                                    #endregion

                                                                    newOpenTradeExe.ClientCode = clientCode;
                                                                    newOpenTradeInvestor.ClientCode = clientCode;
                                                                    newOpenTradeSymbol.ClientCode = clientCode;

                                                                    newOpenTradeExe.CommandCode = resultOpenTrade.CommandCode;
                                                                    newOpenTradeInvestor.CommandCode = resultOpenTrade.CommandCode;
                                                                    newOpenTradeSymbol.CommandCode = resultOpenTrade.CommandCode;

                                                                    string commandCode = string.Empty;

                                                                    //set id and command code
                                                                    newOpenTradeExe.ID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeInvestor.ID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeSymbol.ID = resultOpenTrade.RefCommandID;

                                                                    commandCode = resultOpenTrade.CommandCode;

                                                                    //======================================
                                                                    #region ADD COMMAND TO COMMAND EXECUTOR
                                                                    if (Business.Market.CommandExecutor == null)
                                                                        Business.Market.CommandExecutor = new List<Business.OpenTrade>();

                                                                    Business.Market.CommandExecutor.Add(newOpenTradeExe);
                                                                    #endregion

                                                                    #region ADD COMMAND TO SYMBOL LIST
                                                                    resultOpenTrade.Symbol.CommandList.Add(newOpenTradeSymbol);
                                                                    #endregion

                                                                    #region ADD COMMAND TO INVESTOR LIST
                                                                    resultOpenTrade.Investor.CommandList.Add(newOpenTradeInvestor);
                                                                    #endregion

                                                                    string msg = string.Empty;

                                                                    #region Map Command Server To Client
                                                                    if (resultOpenTrade.IsServer)
                                                                    {
                                                                        #region BUILD COMMAND SEND TO CLIENT
                                                                        StringBuilder Message = new StringBuilder();
                                                                        Message.Append("AddCommandByManager$True,Add New Command Complete,");
                                                                        Message.Append(resultOpenTrade.RefCommandID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Investor.InvestorID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Symbol.Name);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Size);
                                                                        Message.Append(",");
                                                                        Message.Append(IsBuy);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenPrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.StopLoss);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.TakeProfit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClosePrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Commission);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Swap);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Profit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Comment);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(commandType);
                                                                        Message.Append(",");
                                                                        Message.Append(1);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ExpTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClientCode);
                                                                        Message.Append(",");
                                                                        Message.Append(commandCode);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.IsHedged);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Type.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Margin);
                                                                        Message.Append(",Open");

                                                                        if (resultOpenTrade.Investor.ClientCommandQueue == null)
                                                                            resultOpenTrade.Investor.ClientCommandQueue = new List<string>();

                                                                        resultOpenTrade.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                        msg = Message.ToString();
                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        #region BUILD COMMAND SEND TO CLIENT
                                                                        StringBuilder Message = new StringBuilder();
                                                                        Message.Append("AddCommand$True,Add New Command Complete,");
                                                                        Message.Append(resultOpenTrade.RefCommandID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Investor.InvestorID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Symbol.Name);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Size);
                                                                        Message.Append(",");
                                                                        Message.Append(IsBuy);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenPrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.StopLoss);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.TakeProfit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClosePrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Commission);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Swap);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Profit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Comment);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(commandType);
                                                                        Message.Append(",");
                                                                        Message.Append(1);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ExpTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClientCode);
                                                                        Message.Append(",");
                                                                        Message.Append(commandCode);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.IsHedged);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Type.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Margin);
                                                                        Message.Append(",Open");

                                                                        if (resultOpenTrade.Investor.ClientCommandQueue == null)
                                                                            resultOpenTrade.Investor.ClientCommandQueue = new List<string>();

                                                                        resultOpenTrade.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                        msg = Message.ToString();
                                                                        #endregion
                                                                    }
                                                                    #endregion

                                                                    #region LOG MAKE COMMAND SUCCESS
                                                                    string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(resultOpenTrade.Type.ID);
                                                                    string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.Size.ToString(), 2);
                                                                    string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.OpenPrice.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.TakeProfit.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.StopLoss.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.Symbol.TickValue.Bid.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.Symbol.TickValue.Ask.ToString(), resultOpenTrade.Symbol.Digit);

                                                                    string content = "'" + resultOpenTrade.Investor.Code + "': order #" + resultOpenTrade.RefCommandID + " " + mode + " " + size + " " +
                                                                                                resultOpenTrade.Symbol.Name + " at " + openPrice + " commission: " + resultOpenTrade.Commission + " [Success]";

                                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, mode, resultOpenTrade.IpAddress, resultOpenTrade.Investor.Code);
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[MakeCommandNotify]", "", "");
                                                                }
                                                                #endregion

                                                                isExitst = true;
                                                                break;
                                                            }
                                                        }
                                                         
                                                        if (!isExitst)
                                                        {
                                                            bool isExitsCommand = false;
                                                            if (resultOpenTrade.Investor.CommandList != null)
                                                            {

                                                                int countCommand = resultOpenTrade.Investor.CommandList.Count;
                                                                for (int n = 0; n < countCommand; n++)
                                                                {
                                                                    if (resultOpenTrade.Investor.CommandList[n].CommandCode == resultOpenTrade.CommandCode)
                                                                    {
                                                                        isExitsCommand = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            #region add command when make command from mt4
                                                            if (!isExitsCommand)
                                                            {
                                                                #region get mql commands (log ipaddress)
                                                                if (Business.Market.marketInstance.MQLCommands != null)
                                                                {
                                                                    int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                                    for (int j = 0; j < countMQL; j++)
                                                                    {
                                                                        if (Business.Market.marketInstance.MQLCommands[j] != null)
                                                                        {
                                                                            if (Business.Market.marketInstance.MQLCommands[j].InvestorCode == resultOpenTrade.Investor.Code)
                                                                            {
                                                                                resultOpenTrade.IpAddress = Business.Market.marketInstance.MQLCommands[j].IpAddress;
                                                                                Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[j]);
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region process add new command
                                                                if (resultOpenTrade.RefCommandID > 0)
                                                                {
                                                                    bool IsBuy = Model.Helper.Instance.IsBuy(resultOpenTrade.Type.ID);
                                                                    commandType = Model.Helper.Instance.convertCommandTypeIDToString(resultOpenTrade.Type.ID);

                                                                    Business.OpenTrade newOpenTradeExe = new OpenTrade();
                                                                    Business.OpenTrade newOpenTradeSymbol = new OpenTrade();
                                                                    Business.OpenTrade newOpenTradeInvestor = new OpenTrade();

                                                                    #region SET INSTANCE IGROUPSECURITY, SYMBOL, TYPE, INVESTOR
                                                                    //set igroupsecurity
                                                                    newOpenTradeExe.IGroupSecurity = resultOpenTrade.IGroupSecurity;
                                                                    newOpenTradeInvestor.IGroupSecurity = resultOpenTrade.IGroupSecurity;
                                                                    newOpenTradeSymbol.IGroupSecurity = resultOpenTrade.IGroupSecurity;

                                                                    //set symbol
                                                                    newOpenTradeExe.Symbol = resultOpenTrade.Symbol;
                                                                    newOpenTradeInvestor.Symbol = resultOpenTrade.Symbol;
                                                                    newOpenTradeSymbol.Symbol = resultOpenTrade.Symbol;

                                                                    //set type
                                                                    newOpenTradeExe.Type = resultOpenTrade.Type;
                                                                    newOpenTradeInvestor.Type = resultOpenTrade.Type;
                                                                    newOpenTradeSymbol.Type = resultOpenTrade.Type;

                                                                    //set investor
                                                                    newOpenTradeExe.Investor = resultOpenTrade.Investor;
                                                                    newOpenTradeInvestor.Investor = resultOpenTrade.Investor;
                                                                    newOpenTradeSymbol.Investor = resultOpenTrade.Investor;
                                                                    #endregion

                                                                    #region GET SPREAD DIFFIRENCE FOR COMMAND
                                                                    //GET SPREAD DIFFRENCE OF OPEN TRADE
                                                                    double spreadDifference = 0;
                                                                    if (resultOpenTrade.IGroupSecurity != null)
                                                                    {
                                                                        if (resultOpenTrade.IGroupSecurity.IGroupSecurityConfig != null)
                                                                        {
                                                                            int countIGroupSecurity = resultOpenTrade.IGroupSecurity.IGroupSecurityConfig.Count;
                                                                            for (int j = 0; j < countIGroupSecurity; j++)
                                                                            {
                                                                                if (resultOpenTrade.IGroupSecurity.IGroupSecurityConfig[j].Code == "B11")
                                                                                {
                                                                                    double.TryParse(resultOpenTrade.IGroupSecurity.IGroupSecurityConfig[j].NumValue, out spreadDifference);
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    newOpenTradeExe.SpreaDifferenceInOpenTrade = spreadDifference;
                                                                    newOpenTradeInvestor.SpreaDifferenceInOpenTrade = spreadDifference;
                                                                    newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreadDifference;
                                                                    #endregion

                                                                    double tempClosePrice = 0;
                                                                    if (IsBuy)
                                                                        tempClosePrice = resultOpenTrade.Symbol.TickValue.Bid;
                                                                    else
                                                                        tempClosePrice = resultOpenTrade.Symbol.TickValue.Ask;

                                                                    resultOpenTrade.ClosePrice = tempClosePrice;

                                                                    #region NEW INSTANCES FOR COMMAND EXECUTOR
                                                                    newOpenTradeExe.AgentCommission = 0;
                                                                    newOpenTradeExe.ClientCode = resultOpenTrade.ClientCode;
                                                                    newOpenTradeExe.CloseTime = resultOpenTrade.OpenTime;
                                                                    //newOpenTradeExe.CommandCode = resultOpenTrade.CommandCode;
                                                                    newOpenTradeExe.Comment = resultOpenTrade.Comment;
                                                                    newOpenTradeExe.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeExe.Commission = resultOpenTrade.Commission;
                                                                    newOpenTradeExe.ExpTime = resultOpenTrade.ExpTime;
                                                                    newOpenTradeExe.FreezeMargin = 0;
                                                                    //newOpenTradeExe.ID = listOnlineCommand[i].OnlineCommandID;
                                                                    newOpenTradeExe.IsClose = false;
                                                                    newOpenTradeExe.OpenPrice = resultOpenTrade.OpenPrice;
                                                                    newOpenTradeExe.OpenTime = resultOpenTrade.OpenTime;
                                                                    newOpenTradeExe.Profit = resultOpenTrade.Profit;
                                                                    newOpenTradeExe.Size = resultOpenTrade.Size;
                                                                    newOpenTradeExe.StopLoss = resultOpenTrade.StopLoss;
                                                                    newOpenTradeExe.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeExe.TakeProfit = resultOpenTrade.TakeProfit;
                                                                    newOpenTradeExe.Taxes = 0;
                                                                    newOpenTradeExe.TotalSwap = 0;
                                                                    newOpenTradeExe.RefCommandID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeExe.ClosePrice = tempClosePrice;
                                                                    #endregion

                                                                    #region NEW INSTANCE FOR SYMBOL LIST
                                                                    newOpenTradeSymbol.AgentCommission = 0;
                                                                    newOpenTradeSymbol.ClientCode = resultOpenTrade.ClientCode;
                                                                    newOpenTradeSymbol.CloseTime = resultOpenTrade.OpenTime;
                                                                    //newOpenTradeSymbol.CommandCode = listOnlineCommand[i].CommandCode;
                                                                    newOpenTradeSymbol.Comment = resultOpenTrade.Comment;
                                                                    newOpenTradeInvestor.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeSymbol.Commission = resultOpenTrade.Commission;
                                                                    newOpenTradeSymbol.ExpTime = resultOpenTrade.ExpTime;
                                                                    newOpenTradeSymbol.FreezeMargin = 0;
                                                                    //newOpenTradeSymbol.ID = listOnlineCommand[i].OnlineCommandID;
                                                                    newOpenTradeSymbol.IsClose = true;
                                                                    newOpenTradeSymbol.OpenPrice = resultOpenTrade.OpenPrice;
                                                                    newOpenTradeSymbol.OpenTime = resultOpenTrade.OpenTime;
                                                                    newOpenTradeSymbol.Profit = resultOpenTrade.Profit;
                                                                    newOpenTradeSymbol.Size = resultOpenTrade.Size;
                                                                    newOpenTradeSymbol.StopLoss = resultOpenTrade.StopLoss;
                                                                    newOpenTradeSymbol.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeSymbol.TakeProfit = resultOpenTrade.TakeProfit;
                                                                    newOpenTradeSymbol.Taxes = 0;
                                                                    newOpenTradeSymbol.TotalSwap = 0;
                                                                    newOpenTradeSymbol.InsExe = newOpenTradeExe;
                                                                    newOpenTradeSymbol.RefCommandID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeSymbol.ClosePrice = tempClosePrice;
                                                                    #endregion

                                                                    #region NEW INSTANCE FOR INVESTOR LIST
                                                                    newOpenTradeInvestor.AgentCommission = 0;
                                                                    newOpenTradeInvestor.ClientCode = resultOpenTrade.ClientCode;
                                                                    newOpenTradeInvestor.CloseTime = resultOpenTrade.OpenTime;
                                                                    //newOpenTradeInvestor.CommandCode = listOnlineCommand[i].CommandCode;
                                                                    newOpenTradeInvestor.Comment = resultOpenTrade.Comment;
                                                                    newOpenTradeInvestor.Commission = resultOpenTrade.Commission;
                                                                    newOpenTradeInvestor.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeInvestor.ExpTime = resultOpenTrade.ExpTime;
                                                                    newOpenTradeInvestor.FreezeMargin = 0;
                                                                    //newOpenTradeInvestor.ID = listOnlineCommand[i].OnlineCommandID;
                                                                    newOpenTradeInvestor.IsClose = false;
                                                                    newOpenTradeInvestor.OpenPrice = resultOpenTrade.OpenPrice;
                                                                    newOpenTradeInvestor.OpenTime = resultOpenTrade.OpenTime;
                                                                    newOpenTradeInvestor.Profit = resultOpenTrade.Profit;
                                                                    newOpenTradeInvestor.Size = resultOpenTrade.Size;
                                                                    newOpenTradeInvestor.StopLoss = resultOpenTrade.StopLoss;
                                                                    newOpenTradeInvestor.Swap = resultOpenTrade.Swap;
                                                                    newOpenTradeInvestor.TakeProfit = resultOpenTrade.TakeProfit;
                                                                    newOpenTradeInvestor.Taxes = 0;
                                                                    newOpenTradeInvestor.TotalSwap = 0;
                                                                    newOpenTradeInvestor.InsExe = newOpenTradeExe;
                                                                    newOpenTradeInvestor.RefCommandID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeInvestor.ClosePrice = tempClosePrice;
                                                                    #endregion

                                                                    #region BUILD CLIENT CODE
                                                                    string clientCode = string.Empty;
                                                                    clientCode = resultOpenTrade.Investor.Code + "_" + DateTime.Now.Ticks;
                                                                    #endregion

                                                                    newOpenTradeExe.ClientCode = clientCode;
                                                                    newOpenTradeInvestor.ClientCode = clientCode;
                                                                    newOpenTradeSymbol.ClientCode = clientCode;

                                                                    newOpenTradeExe.CommandCode = resultOpenTrade.CommandCode;
                                                                    newOpenTradeInvestor.CommandCode = resultOpenTrade.CommandCode;
                                                                    newOpenTradeSymbol.CommandCode = resultOpenTrade.CommandCode;

                                                                    string commandCode = string.Empty;

                                                                    //set id and command code
                                                                    newOpenTradeExe.ID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeInvestor.ID = resultOpenTrade.RefCommandID;
                                                                    newOpenTradeSymbol.ID = resultOpenTrade.RefCommandID;

                                                                    commandCode = resultOpenTrade.CommandCode;

                                                                    //======================================
                                                                    #region ADD COMMAND TO COMMAND EXECUTOR
                                                                    if (Business.Market.CommandExecutor == null)
                                                                        Business.Market.CommandExecutor = new List<Business.OpenTrade>();

                                                                    Business.Market.CommandExecutor.Add(newOpenTradeExe);
                                                                    #endregion

                                                                    #region ADD COMMAND TO SYMBOL LIST
                                                                    resultOpenTrade.Symbol.CommandList.Add(newOpenTradeSymbol);
                                                                    #endregion

                                                                    #region ADD COMMAND TO INVESTOR LIST
                                                                    resultOpenTrade.Investor.CommandList.Add(newOpenTradeInvestor);
                                                                    #endregion

                                                                    string msg = string.Empty;

                                                                    #region Map Command Server To Client
                                                                    if (resultOpenTrade.IsServer)
                                                                    {
                                                                        #region BUILD COMMAND SEND TO CLIENT
                                                                        StringBuilder Message = new StringBuilder();
                                                                        Message.Append("AddCommandByManager$True,Add New Command Complete,");
                                                                        Message.Append(resultOpenTrade.RefCommandID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Investor.InvestorID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Symbol.Name);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Size);
                                                                        Message.Append(",");
                                                                        Message.Append(IsBuy);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenPrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.StopLoss);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.TakeProfit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClosePrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Commission);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Swap);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Profit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Comment);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(commandType);
                                                                        Message.Append(",");
                                                                        Message.Append(1);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ExpTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClientCode);
                                                                        Message.Append(",");
                                                                        Message.Append(commandCode);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.IsHedged);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Type.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Margin);
                                                                        Message.Append(",Open");

                                                                        if (resultOpenTrade.Investor.ClientCommandQueue == null)
                                                                            resultOpenTrade.Investor.ClientCommandQueue = new List<string>();

                                                                        resultOpenTrade.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                        msg = Message.ToString();
                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        #region BUILD COMMAND SEND TO CLIENT
                                                                        StringBuilder Message = new StringBuilder();
                                                                        Message.Append("AddCommand$True,Add New Command Complete,");
                                                                        Message.Append(resultOpenTrade.RefCommandID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Investor.InvestorID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Symbol.Name);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Size);
                                                                        Message.Append(",");
                                                                        Message.Append(IsBuy);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.OpenPrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.StopLoss);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.TakeProfit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClosePrice);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Commission);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Swap);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Profit);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Comment);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(commandType);
                                                                        Message.Append(",");
                                                                        Message.Append(1);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ExpTime);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.ClientCode);
                                                                        Message.Append(",");
                                                                        Message.Append(commandCode);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.IsHedged);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Type.ID);
                                                                        Message.Append(",");
                                                                        Message.Append(resultOpenTrade.Margin);
                                                                        Message.Append(",Open");

                                                                        if (resultOpenTrade.Investor.ClientCommandQueue == null)
                                                                            resultOpenTrade.Investor.ClientCommandQueue = new List<string>();

                                                                        resultOpenTrade.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                        msg = Message.ToString();
                                                                        #endregion
                                                                    }
                                                                    #endregion

                                                                    #region LOG MAKE COMMAND SUCCESS
                                                                    string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(resultOpenTrade.Type.ID);
                                                                    string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.Size.ToString(), 2);
                                                                    string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.OpenPrice.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.TakeProfit.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.StopLoss.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.Symbol.TickValue.Bid.ToString(), resultOpenTrade.Symbol.Digit);
                                                                    string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(resultOpenTrade.Symbol.TickValue.Ask.ToString(), resultOpenTrade.Symbol.Digit);

                                                                    string content = "'" + resultOpenTrade.Investor.Code + "': order #" + resultOpenTrade.RefCommandID + " " + mode + " " + size + " " +
                                                                                                resultOpenTrade.Symbol.Name + " at " + openPrice + " commission: " + resultOpenTrade.Commission + " [Success]";

                                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, mode, resultOpenTrade.IpAddress, resultOpenTrade.Investor.Code);
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[MakeCommandNotify]", "", "");
                                                                }
                                                                #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY CLOSE COMMAND
                                    case "NotifyCloseCommand":
                                        {
                                            try
                                            {   
                                                string[] subParameter = subValue[1].Split('{');

                                                TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[NotifyCloseCommand]", "", subParameter[4]);

                                                Business.OpenTrade resultNotify = Business.Market.MapNotifyCloseCommand(subCommand[i]);

                                                Business.OpenTrade Command = TradingServer.Facade.FacadeFindOpenTradeInSymbolListByRefID(resultNotify.RefCommandID);

                                                if (Command.Investor == null || Command.Symbol == null || Command.Type == null)
                                                {   
                                                    continue;
                                                }

                                                #region comment code
                                                if (Business.Market.NJ4XTickets != null)
                                                {
                                                    lock (Business.Market.nj4xObject)
                                                    {
                                                        bool isExits = false;
                                                        int countNJ4X = Business.Market.NJ4XTickets.Count;
                                                        for (int m = 0; m < countNJ4X; m++)
                                                        {
                                                            if (Business.Market.NJ4XTickets[m].Code == Command.CommandCode)
                                                            {
                                                                lock (Business.Market.nj4xObject)
                                                                    Business.Market.NJ4XTickets.RemoveAt(m);

                                                                if (Command.ID > 0)
                                                                {
                                                                    Command.ClosePrice = resultNotify.ClosePrice;
                                                                    Command.Size = resultNotify.Size;
                                                                    Command.Profit = resultNotify.Profit;
                                                                    Command.Swap = resultNotify.Swap;
                                                                    Command.Commission = resultNotify.Commission;
                                                                    Command.CloseTime = resultNotify.CloseTime;
                                                                    Command.IsClose = true;

                                                                    //Command.Investor.UpdateCommand(Command);

                                                                    List<Business.OpenTrade> commands = Command.Investor.CommandList;
                                                                    bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

                                                                    #region For Command List
                                                                    for (int j = 0; j < commands.Count; j++)
                                                                    {
                                                                        if (commands[j].ID == Command.ID)
                                                                        {
                                                                            int commandId = Command.ID;

                                                                            //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                                                            newOpenRemove.InvestorID = Command.Investor.InvestorID;
                                                                            newOpenRemove.OpenTradeID = commandId;
                                                                            newOpenRemove.SymbolName = commands[j].Symbol.Name;
                                                                            newOpenRemove.IsExecutor = true;
                                                                            newOpenRemove.IsSymbol = true;
                                                                            newOpenRemove.IsInvestor = false;
                                                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                                            //Close Command Complete Add Message To Client
                                                                            if (Command.Investor.ClientCommandQueue == null)
                                                                                Command.Investor.ClientCommandQueue = new List<string>();

                                                                            #region Map Command Server To Client
                                                                            if (Command.IsServer)
                                                                            {
                                                                                string Message = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                                                    ",Close," + Command.CloseTime;

                                                                                Command.Investor.ClientCommandQueue.Add(Message);

                                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, Message, "  ", Command.IpAddress, Command.Investor.Code);
                                                                            }
                                                                            else
                                                                            {
                                                                                string Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                                                    ",Close," + Command.CloseTime;

                                                                                commands[j].Investor.ClientCommandQueue.Add(Message);

                                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, Message, "  ", Command.IpAddress, Command.Investor.Code);
                                                                            }
                                                                            #endregion

                                                                            if (Business.Market.marketInstance.MQLCommands != null)
                                                                            {
                                                                                int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                                                for (int n = 0; n < countMQL; n++)
                                                                                {
                                                                                    if (Business.Market.marketInstance.MQLCommands[n].InvestorCode == Command.Investor.Code)
                                                                                    {
                                                                                        Command.IpAddress = Business.Market.marketInstance.MQLCommands[n].IpAddress;
                                                                                        Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[n]);
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }

                                                                            #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                                                            string mode = "sell";
                                                                            if (commands[j].Type.ID == 1)
                                                                                mode = "buy";

                                                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), commands[j].Symbol.Digit);
                                                                            string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), commands[j].Symbol.Digit);
                                                                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), commands[j].Symbol.Digit);
                                                                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), commands[j].Symbol.Digit);
                                                                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), commands[j].Symbol.Digit);
                                                                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), commands[j].Symbol.Digit);

                                                                            string contentServer = "'" + commands[j].Investor.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                                                                Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " completed";

                                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.IpAddress, Command.Investor.Code);
                                                                            #endregion

                                                                            lock (Business.Market.syncObject)
                                                                            {
                                                                                bool deleteCommandInvestor = commands.Remove(commands[j]);
                                                                            }

                                                                            #region COMMAND CLOSE
                                                                            int commandRefID = commands[i].ID;
                                                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
                                                                            #endregion
                                                                            break;
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    //insert system log
                                                                    TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[NotifyCloseCommandFalse]", "", "");
                                                                }

                                                                isExits = true;

                                                                break;
                                                            }
                                                        }

                                                        if (!isExits)
                                                        {
                                                            bool isExitsCommand = false;
                                                            if (Command.Investor.CommandList != null)
                                                            {
                                                                int countCommand = Command.Investor.CommandList.Count;
                                                                for (int n = 0; n < countCommand; n++)
                                                                {
                                                                    if (Command.Investor.CommandList[n].CommandCode == Command.CommandCode)
                                                                    {
                                                                        isExitsCommand = true;

                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            #region remove command when close 
                                                            if (isExitsCommand)
                                                            {
                                                                if (Command.ID > 0)
                                                                {
                                                                    Command.ClosePrice = resultNotify.ClosePrice;
                                                                    Command.Size = resultNotify.Size;
                                                                    Command.Profit = resultNotify.Profit;
                                                                    Command.Swap = resultNotify.Swap;
                                                                    Command.Commission = resultNotify.Commission;
                                                                    Command.CloseTime = resultNotify.CloseTime;
                                                                    Command.IsClose = true;

                                                                    //Command.Investor.UpdateCommand(Command);

                                                                    List<Business.OpenTrade> commands = Command.Investor.CommandList;
                                                                    bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

                                                                    #region For Command List
                                                                    for (int j = 0; j < commands.Count; j++)
                                                                    {
                                                                        if (commands[j].ID == Command.ID)
                                                                        {
                                                                            int commandId = Command.ID;

                                                                            //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                                            Business.OpenRemove newOpenRemove = new OpenRemove();
                                                                            newOpenRemove.InvestorID = Command.Investor.InvestorID;
                                                                            newOpenRemove.OpenTradeID = commandId;
                                                                            newOpenRemove.SymbolName = commands[j].Symbol.Name;
                                                                            newOpenRemove.IsExecutor = true;
                                                                            newOpenRemove.IsSymbol = true;
                                                                            newOpenRemove.IsInvestor = false;
                                                                            Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                                            //Close Command Complete Add Message To Client
                                                                            if (Command.Investor.ClientCommandQueue == null)
                                                                                Command.Investor.ClientCommandQueue = new List<string>();

                                                                            #region Map Command Server To Client
                                                                            if (Command.IsServer)
                                                                            {
                                                                                string Message = "CloseCommandByManager$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                                                    ",Close," + Command.CloseTime;

                                                                                Command.Investor.ClientCommandQueue.Add(Message);

                                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, Message, "  ", Command.IpAddress, Command.Investor.Code);
                                                                            }
                                                                            else
                                                                            {
                                                                                string Message = "CloseCommand$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                                                                    Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                                                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                                                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                                                                    ",Close," + Command.CloseTime;

                                                                                commands[j].Investor.ClientCommandQueue.Add(Message);

                                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, Message, "  ", Command.IpAddress, Command.Investor.Code);
                                                                            }
                                                                            #endregion

                                                                            if (Business.Market.marketInstance.MQLCommands != null)
                                                                            {
                                                                                int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                                                for (int n = 0; n < countMQL; n++)
                                                                                {
                                                                                    if (Business.Market.marketInstance.MQLCommands[n].InvestorCode == Command.Investor.Code)
                                                                                    {
                                                                                        Command.IpAddress = Business.Market.marketInstance.MQLCommands[n].IpAddress;
                                                                                        Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[n]);
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }

                                                                            #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                                                            string mode = "sell";
                                                                            if (commands[j].Type.ID == 1)
                                                                                mode = "buy";

                                                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), commands[j].Symbol.Digit);
                                                                            string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), commands[j].Symbol.Digit);
                                                                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), commands[j].Symbol.Digit);
                                                                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), commands[j].Symbol.Digit);
                                                                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), commands[j].Symbol.Digit);
                                                                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), commands[j].Symbol.Digit);

                                                                            string contentServer = "'" + commands[j].Investor.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                                                                Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " completed";

                                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.IpAddress, Command.Investor.Code);
                                                                            #endregion

                                                                            lock (Business.Market.syncObject)
                                                                            {
                                                                                bool deleteCommandInvestor = commands.Remove(commands[j]);
                                                                            }

                                                                            #region COMMAND CLOSE
                                                                            int commandRefID = commands[i].ID;
                                                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
                                                                            #endregion

                                                                            break;
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    //insert system log
                                                                    TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[NotifyCloseCommandFalse]", "", "");
                                                                }
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY UPDATE ACCOUNT
                                    case "NotifyUpdateAccount":
                                        {
                                            try
                                            {
                                                Model.TradingCalculate.Instance.StreamManagerNotify(result + " (" + DateTime.Now + ")");

                                                Business.Investor resultNotify = Business.Market.MapNotifyUpdateAccount(subCommand[i]);

                                                if (resultNotify.InvestorID <= 0)
                                                    continue;

                                                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Receive Notify] - " + subCommand[i]);

                                                string message = "IAC04332451";
                                                resultNotify.ClientCommandQueue.Add(message);
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY UPDATE INFO ACCOUNT
                                    case "NotifyInfoAccount":
                                        {
                                            try
                                            {
                                                Model.TradingCalculate.Instance.StreamManagerNotify(result + " (" + DateTime.Now + ")");

                                                Business.Investor resultNotify = Business.Market.MapNotifyUpdateInfoAccount(subCommand[i]);

                                                if (resultNotify.InvestorID <= 0)
                                                    continue;

                                                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Receive Notify] - " + subCommand[i]);

                                                string message = "IAC04332451";

                                                resultNotify.ClientCommandQueue.Add(message);
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY DELETE COMMAND
                                    case "NotifyDeleteCommand":
                                        {
                                            try
                                            {
                                                Model.TradingCalculate.Instance.StreamManagerNotify(result + " (" + DateTime.Now + ")");

                                                int resultNotify = Business.Market.MapNotifyDeleteCommand(subCommand[i]);
                                                if (resultNotify > 0)
                                                {
                                                    Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeFindOpenTradeInSymbolListByRefID(resultNotify);

                                                    if (newOpenTrade.Investor == null || newOpenTrade.Symbol == null || newOpenTrade.Type == null)
                                                        continue;

                                                    //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Receive Notify] - " + subCommand[i]);

                                                    if (newOpenTrade.RefCommandID > 0)
                                                    {
                                                        List<Business.OpenTrade> commands = newOpenTrade.Investor.CommandList;
                                                        bool IsBuy = Model.Helper.Instance.IsBuy(newOpenTrade.Type.ID);

                                                        for (int j = 0; j < commands.Count; j++)
                                                        {
                                                            if (commands[j].ID == newOpenTrade.ID)
                                                            {
                                                                #region CLOSE PENDING ORDER
                                                                //ADD PENDING ORDER TO DATABASE
                                                                int addHistory = TradingServer.Facade.FacadeAddNewCommandHistory(newOpenTrade.Investor.InvestorID, newOpenTrade.Type.ID, newOpenTrade.CommandCode,
                                                                    newOpenTrade.OpenTime, newOpenTrade.OpenPrice, newOpenTrade.CloseTime, newOpenTrade.ClosePrice, 0, 0, 0, newOpenTrade.ExpTime, newOpenTrade.Size, newOpenTrade.StopLoss,
                                                                    newOpenTrade.TakeProfit, newOpenTrade.ClientCode, newOpenTrade.Symbol.SymbolID, newOpenTrade.Taxes, newOpenTrade.AgentCommission, newOpenTrade.Comment, "3",
                                                                    newOpenTrade.TotalSwap, newOpenTrade.RefCommandID, newOpenTrade.AgentRefConfig, newOpenTrade.IsActivePending, newOpenTrade.IsStopLossAndTakeProfit);

                                                                //NEW SOLUTION ADD COMMAND TO REMOVE LIST
                                                                Business.OpenRemove newOpenRemove = new OpenRemove();
                                                                newOpenRemove.InvestorID = commands[j].Investor.InvestorID;
                                                                newOpenRemove.OpenTradeID = commands[j].ID;
                                                                newOpenRemove.SymbolName = commands[j].Symbol.Name;
                                                                newOpenRemove.IsExecutor = true;
                                                                newOpenRemove.IsSymbol = true;
                                                                newOpenRemove.IsInvestor = false;
                                                                Business.Market.AddCommandToRemoveList(newOpenRemove);

                                                                //Close Command Complete Add Message To Client
                                                                if (newOpenTrade.Investor.ClientCommandQueue == null)
                                                                    newOpenTrade.Investor.ClientCommandQueue = new List<string>();
                                                                #endregion

                                                                #region MAP STRING SEND TO CLIENT
                                                                string Message = string.Empty;
                                                                if (newOpenTrade.IsServer)
                                                                {
                                                                    Message = "CloseCommandByManager$True,Close Command Complete," + newOpenTrade.ID + "," + newOpenTrade.Investor.InvestorID + "," + newOpenTrade.Symbol.Name + "," +
                                                                        newOpenTrade.Size + "," + IsBuy + "," + newOpenTrade.OpenTime + "," + newOpenTrade.OpenPrice + "," + newOpenTrade.StopLoss + "," + newOpenTrade.TakeProfit + "," +
                                                                        newOpenTrade.ClosePrice + "," + newOpenTrade.Commission + "," + newOpenTrade.Swap + "," + newOpenTrade.Profit + "," + "Comment," + newOpenTrade.ID + "," + newOpenTrade.Type.Name + "," +
                                                                        1 + "," + newOpenTrade.ExpTime + "," + newOpenTrade.ClientCode + "," + newOpenTrade.CommandCode + "," + newOpenTrade.IsHedged + "," + newOpenTrade.Type.ID + "," + newOpenTrade.Margin +
                                                                        ",Close," + newOpenTrade.CloseTime;
                                                                }
                                                                else
                                                                {
                                                                    Message = "CloseCommand$True,Close Command Complete," + newOpenTrade.ID + "," + newOpenTrade.Investor.InvestorID + "," + newOpenTrade.Symbol.Name + "," +
                                                                        newOpenTrade.Size + "," + IsBuy + "," + newOpenTrade.OpenTime + "," + newOpenTrade.OpenPrice + "," + newOpenTrade.StopLoss + "," + newOpenTrade.TakeProfit + "," +
                                                                        newOpenTrade.ClosePrice + "," + newOpenTrade.Commission + "," + newOpenTrade.Swap + "," + newOpenTrade.Profit + "," + "Comment," + newOpenTrade.ID + "," + newOpenTrade.Type.Name + "," +
                                                                        1 + "," + newOpenTrade.ExpTime + "," + newOpenTrade.ClientCode + "," + newOpenTrade.CommandCode + "," + newOpenTrade.IsHedged + "," + newOpenTrade.Type.ID + "," + newOpenTrade.Margin +
                                                                        ",Close," + newOpenTrade.CloseTime;
                                                                }

                                                                newOpenTrade.Investor.ClientCommandQueue.Add(Message);

                                                                if (Business.Market.marketInstance.MQLCommands != null)
                                                                {
                                                                    int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                                    for (int n = 0; n < countMQL; n++)
                                                                    {
                                                                        if (Business.Market.marketInstance.MQLCommands[n].InvestorCode == newOpenTrade.Investor.Code)
                                                                        {
                                                                            newOpenTrade.IpAddress = Business.Market.marketInstance.MQLCommands[n].IpAddress;
                                                                            Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[n]);
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                                                                string mode = "sell";
                                                                if (commands[i].Type.ID == 1)
                                                                    mode = "buy";

                                                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Size.ToString(), 2);
                                                                string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.OpenPrice.ToString(), commands[j].Symbol.Digit);
                                                                string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.ClosePrice.ToString(), commands[j].Symbol.Digit);
                                                                string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.StopLoss.ToString(), commands[j].Symbol.Digit);
                                                                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.TakeProfit.ToString(), commands[j].Symbol.Digit);
                                                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Symbol.TickValue.Bid.ToString(), commands[j].Symbol.Digit);
                                                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Symbol.TickValue.Ask.ToString(), commands[j].Symbol.Digit);

                                                                string contentServer = "'" + commands[j].Investor.Code + "': close order #" + newOpenTrade.CommandCode + " (" + mode + " " + size + " " +
                                                                    newOpenTrade.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " completed";

                                                                TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", newOpenTrade.IpAddress, newOpenTrade.Investor.Code);
                                                                #endregion

                                                                lock (Business.Market.syncObject)
                                                                {
                                                                    bool deleteCommandInvestor = commands.Remove(commands[j]);
                                                                }
                                                                #endregion

                                                                break;
                                                            }
                                                        }


                                                        //bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(newOpenTrade.Type.ID);

                                                        //if (isPending)
                                                        //{
                                                        //    newOpenTrade.IsClose = true;

                                                        //    newOpenTrade.Investor.UpdateCommand(newOpenTrade);
                                                        //}
                                                        //else
                                                        //{
                                                        //    TradingServer.Facade.FacadeDeleteOpenTradeByManagerWithRefID(newOpenTrade.RefCommandID);
                                                        //}
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY UPDATE COMMAND
                                    case "NotifyUpdateCommand":
                                        {
                                            try
                                            {
                                                Model.TradingCalculate.Instance.StreamManagerNotify(result + " (" + DateTime.Now + ")");

                                                Business.OpenTrade resultNotify = Business.Market.MapNotifyUpdateCommand(subCommand[i]);
                                                resultNotify.ID = resultNotify.RefCommandID;
                                                if (resultNotify.Investor == null || resultNotify.Symbol == null || resultNotify.Type == null)
                                                    continue;

                                                double profit = 0;

                                                #region UPDATE ONLINE COMMAND IN INVESTOR LIST
                                                if (resultNotify.Investor.CommandList != null)
                                                {
                                                    int countCommand = resultNotify.Investor.CommandList.Count;
                                                    for (int k = 0; k < countCommand; k++)
                                                    {
                                                        if (resultNotify.Investor.CommandList[k].ID == resultNotify.RefCommandID)
                                                        {
                                                            Business.OpenTrade temp = resultNotify.Investor.CommandList[k];

                                                            if (Business.Market.marketInstance.MQLCommands != null)
                                                            {
                                                                int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                                                for (int j = 0; j < countMQL; j++)
                                                                {
                                                                    if (Business.Market.marketInstance.MQLCommands[j].InvestorCode == temp.Investor.Code)
                                                                    {
                                                                        temp.IpAddress = Business.Market.marketInstance.MQLCommands[j].IpAddress;
                                                                        Business.Market.marketInstance.MQLCommands.Remove(Business.Market.marketInstance.MQLCommands[j]);
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            bool isManager = false;
                                                            if (temp.OpenPrice != resultNotify.OpenPrice || temp.Size != resultNotify.Size)
                                                                isManager = true;

                                                            //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                                                            temp.Commission = resultNotify.Commission;
                                                            temp.ExpTime = resultNotify.ExpTime;
                                                            temp.OpenPrice = resultNotify.OpenPrice;
                                                            temp.OpenTime = resultNotify.OpenTime;
                                                            temp.StopLoss = resultNotify.StopLoss;
                                                            temp.Swap = resultNotify.Swap;
                                                            temp.TakeProfit = resultNotify.TakeProfit;
                                                            temp.Comment = resultNotify.Comment;
                                                            temp.Size = resultNotify.Size;
                                                            temp.Profit = profit;

                                                            //Update Command In Command Execute
                                                            temp.InsExe.Commission = resultNotify.Commission;
                                                            temp.InsExe.ExpTime = resultNotify.ExpTime;
                                                            temp.InsExe.OpenPrice = resultNotify.OpenPrice;
                                                            temp.InsExe.OpenTime = resultNotify.OpenTime;
                                                            temp.InsExe.StopLoss = resultNotify.StopLoss;
                                                            temp.InsExe.Swap = resultNotify.Swap;
                                                            temp.InsExe.TakeProfit = resultNotify.TakeProfit;
                                                            temp.InsExe.Comment = resultNotify.Comment;
                                                            temp.InsExe.Size = resultNotify.Size;
                                                            temp.InsExe.Profit = profit;

                                                            bool IsBuy = false;
                                                            if (temp.Type.ID == 1 || temp.Type.ID == 7 || temp.Type.ID == 9 || temp.Type.ID == 11)
                                                                IsBuy = true;

                                                            #region MAP COMMAND TO CLIENT
                                                            #region BUILD COMMAND SEND TO CLIENT
                                                            StringBuilder Message = new StringBuilder();
                                                            Message.Append("UpdateCommand$True,UPDATE COMMAND BY MANAGER COMPLETE,");
                                                            Message.Append(temp.ID);
                                                            Message.Append(",");
                                                            Message.Append(temp.Investor.InvestorID);
                                                            Message.Append(",");
                                                            Message.Append(temp.Symbol.Name);
                                                            Message.Append(",");
                                                            Message.Append(temp.Size);
                                                            Message.Append(",");
                                                            Message.Append(IsBuy);
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
                                                            Message.Append(temp.Commission);
                                                            Message.Append(",");
                                                            Message.Append(temp.Swap);
                                                            Message.Append(",");
                                                            Message.Append(temp.Profit);
                                                            Message.Append(",");
                                                            Message.Append(temp.Comment);
                                                            Message.Append(",");
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
                                                            Message.Append(",Update");

                                                            if (temp.Investor.ClientCommandQueue == null)
                                                                temp.Investor.ClientCommandQueue = new List<string>();

                                                            temp.Investor.ClientCommandQueue.Add(Message.ToString());
                                                            #endregion

                                                            #region insert system log
                                                            string content = string.Empty;
                                                            string comment = "[modify order]";
                                                            string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(temp.Type.ID).ToLower();

                                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(temp.Size.ToString(), 2);
                                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(temp.OpenPrice.ToString(), temp.Symbol.Digit);
                                                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(temp.StopLoss.ToString(), temp.Symbol.Digit);
                                                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(temp.TakeProfit.ToString(), temp.Symbol.Digit);
                                                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(temp.Symbol.TickValue.Bid.ToString(), temp.Symbol.Digit);
                                                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(temp.Symbol.TickValue.Ask.ToString(), temp.Symbol.Digit);

                                                            string Content = string.Empty;
                                                            Content = "'" + temp.Investor.Code + "': modified #" + temp.CommandCode + " " + mode + " " + size + " " + temp.Symbol.Name + " at " +
                                                                openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ") - [Success]";

                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, Content, comment, temp.IpAddress, temp.Investor.Code);
                                                            #endregion

                                                            //if (!isManager)
                                                            //{

                                                            //}
                                                            //else
                                                            //{
                                                            //    #region BUILD COMMAND SEND TO CLIENT
                                                            //    StringBuilder Message = new StringBuilder();
                                                            //    Message.Append("UpdateCommandByManager$True,UPDATE COMMAND BY MANAGER COMPLETE,");
                                                            //    Message.Append(temp.ID);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Investor.InvestorID);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Symbol.Name);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Size);
                                                            //    Message.Append(",");
                                                            //    Message.Append(IsBuy);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.OpenTime);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.OpenPrice);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.StopLoss);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.TakeProfit);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.ClosePrice);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Commission);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Swap);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Profit);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Comment);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.ID);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Type.Name);
                                                            //    Message.Append(",");
                                                            //    Message.Append(1);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.ExpTime);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.ClientCode);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.CommandCode);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.IsHedged);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Type.ID);
                                                            //    Message.Append(",");
                                                            //    Message.Append(temp.Margin);
                                                            //    Message.Append(",Update");

                                                            //    if (temp.Investor.ClientCommandQueue == null)
                                                            //        temp.Investor.ClientCommandQueue = new List<string>();

                                                            //    temp.Investor.ClientCommandQueue.Add(Message.ToString());
                                                            //    #endregion
                                                            //}

                                                            #endregion

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region UPDATE ONLINE COMMAND IN SYMBOL LIST
                                                if (resultNotify.Symbol.CommandList != null)
                                                {
                                                    int countCommand = resultNotify.Symbol.CommandList.Count;
                                                    for (int k = 0; k < countCommand; k++)
                                                    {
                                                        if (resultNotify.Symbol.CommandList[k].ID == resultNotify.RefCommandID)
                                                        {
                                                            if (resultNotify.Symbol.CommandList[k].Type.ID == resultNotify.Type.ID)
                                                            {
                                                                //SET NEW VALUE FOR ONLINE COMMAND CURRENT
                                                                resultNotify.Symbol.CommandList[k].Commission = resultNotify.Commission;
                                                                resultNotify.Symbol.CommandList[k].ExpTime = resultNotify.ExpTime;
                                                                resultNotify.Symbol.CommandList[k].OpenPrice = resultNotify.OpenPrice;
                                                                resultNotify.Symbol.CommandList[k].OpenTime = resultNotify.OpenTime;
                                                                resultNotify.Symbol.CommandList[k].StopLoss = resultNotify.StopLoss;
                                                                resultNotify.Symbol.CommandList[k].Swap = resultNotify.Swap;
                                                                resultNotify.Symbol.CommandList[k].TakeProfit = resultNotify.TakeProfit;
                                                                resultNotify.Symbol.CommandList[k].Comment = resultNotify.Comment;
                                                                resultNotify.Symbol.CommandList[k].Size = resultNotify.Size;
                                                                //Business.Market.SymbolList[i].CommandList[j].Profit = profit;
                                                            }
                                                            else
                                                            {
                                                                resultNotify.Symbol.CommandList[k].Type = resultNotify.Type;
                                                                resultNotify.Symbol.CommandList[k].IsClose = false;
                                                                resultNotify.Symbol.CommandList[k].Investor.UpdateCommand(resultNotify);
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY CONNECT MT4
                                    case "NotifyConnectMT4":
                                        {
                                            try
                                            {
                                                Business.Market.StatusConnect = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY DISCONNECT MT4
                                    case "NotifyDisconnectMT4":
                                        {
                                            try
                                            {
                                                Model.TradingCalculate.Instance.StreamManagerNotify(result + " (" + DateTime.Now + ")");

                                                Business.Market.StatusConnect = false;

                                                if (Business.Market.InvestorOnline != null)
                                                {
                                                    int countInvestor = Business.Market.InvestorOnline.Count;
                                                    for (int j = 0; j < countInvestor; j++)
                                                    {
                                                        if (Business.Market.InvestorOnline[j].IsOnline)
                                                        {
                                                            string message = "OLOFF14790251";

                                                            Business.Market.InvestorOnline[j].ClientCommandQueue.Add(message);
                                                        }

                                                        Business.Market.InvestorOnline[j].IsFirstLogin = true;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY INFO CLIENT SEND REQUEST
                                    //NotifyInfoClientSendRequest$status{idSession{timeRequest{managerCode{clientCode{group{priceBid{
                                    //priceAsk{tradeType{commandID{reserved{commandCode{symbol{volume{price{stoploss{takeprofit{ie_deviation{expiration
                                    //"NotifyInfoClientSendRequest$1{353{03/08/1996 22:01:00{0{91122838{test-vietnam{1.3669{1.3672{@{1{{0{EURUSD{100{1.3669{0{0{0{01/01/1970 02:00:00¬"
                                    case "NotifyInfoClientSendRequest":
                                        {
                                            string[] subParameter = subValue[1].Split('{');

                                            TradingServer.Facade.FacadeAddNewSystemLog(6, result, "[NotifyInfoClientSendRequest]", subParameter[8], subParameter[4]);

                                            if (subParameter[0] == "3")
                                            {
                                                try
                                                {
                                                    if (subParameter[8] == "@")
                                                        break;

                                                    double bid = double.Parse(subParameter[6]);
                                                    double ask = double.Parse(subParameter[7]);

                                                    bool isFlag = false;
                                                    if (bid > 0 && ask > 0)
                                                    {
                                                        if (Business.Market.NJ4XTickets != null)
                                                        {
                                                            //int countTicket = Business.Market.NJ4XTickets.Count;
                                                            for (int j = 0; j < Business.Market.NJ4XTickets.Count; j++)
                                                            {
                                                                if (Business.Market.NJ4XTickets[j].IsClose)
                                                                {
                                                                    NJ4XConnectSocket.NJ4XTicket temp;
                                                                    temp = Business.Market.NJ4XTickets[j];

                                                                    #region NOTIFY CLOSE
                                                                    if (temp.Code.Trim() == subParameter[4].Trim() && temp.IsDisable == false &&
                                                                        temp.IsReQuote == false)
                                                                    {
                                                                        if (temp.Execution == EnumMT4.Execution.REQUEST)
                                                                        {
                                                                            bool IsBuy = false;
                                                                            if (temp.Command.Type.ID == 1 || temp.Command.Type.ID == 7 || temp.Command.Type.ID == 9)
                                                                                IsBuy = true;

                                                                            #region CLOSE COMMAND WITH MODE REQUEST
                                                                            temp.Ask = double.Parse(subParameter[7]);
                                                                            temp.Bid = double.Parse(subParameter[6]);
                                                                            temp.IsRequest = true;
                                                                            temp.IsDisable = true;
                                                                            temp.IsReQuote = true;

                                                                            if (temp.Command.Type.ID == 1)
                                                                                temp.Command.ClosePrice = temp.Bid;
                                                                            else
                                                                                temp.Command.ClosePrice = temp.Ask;

                                                                            StringBuilder Message = new StringBuilder();

                                                                            Message.Append("CloseCommand$False,RD28,");
                                                                            Message.Append(temp.Command.ID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Investor.InvestorID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Symbol.Name);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Size);
                                                                            Message.Append(",");
                                                                            Message.Append(IsBuy);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.OpenTime);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.OpenPrice);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.StopLoss);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.TakeProfit);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.ClosePrice);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Commission);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Swap);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Profit);
                                                                            Message.Append(",");
                                                                            Message.Append("Comment,");
                                                                            Message.Append(temp.Command.ID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Type.Name);
                                                                            Message.Append(",");
                                                                            Message.Append(1);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.ExpTime);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.ClientCode);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.CommandCode);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.IsHedged);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Type.ID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Margin);
                                                                            Message.Append(",Close,");
                                                                            Message.Append(temp.Command.CloseTime);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Bid);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Ask);

                                                                            if (temp.Command.Investor.ClientCommandQueue == null)
                                                                                temp.Command.Investor.ClientCommandQueue = new List<string>();

                                                                            temp.Command.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                            //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Receive Notify] - " + Message);
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            if (temp.Ticket == int.Parse(subParameter[11]))
                                                                            {
                                                                                bool IsBuy = false;
                                                                                if (temp.Command.Type.ID == 1 || temp.Command.Type.ID == 7 || temp.Command.Type.ID == 9)
                                                                                    IsBuy = true;

                                                                                #region CLOSE COMMAND WITH MODE INSTANCE
                                                                                temp.Ask = double.Parse(subParameter[7]);
                                                                                temp.Bid = double.Parse(subParameter[6]);
                                                                                temp.Ticket = int.Parse(subParameter[9]);
                                                                                temp.IsDisable = true;
                                                                                temp.IsReQuote = true;

                                                                                if (temp.Command.Type.ID == 1)
                                                                                    temp.Command.ClosePrice = temp.Bid;
                                                                                else
                                                                                    temp.Command.ClosePrice = temp.Ask;

                                                                                StringBuilder Message = new StringBuilder();
                                                                                Message.Append("CloseCommand$False,RD02,");
                                                                                Message.Append(temp.Command.ID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Investor.InvestorID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Symbol.Name);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Size);
                                                                                Message.Append(",");
                                                                                Message.Append(IsBuy);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.OpenTime);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.OpenPrice);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.StopLoss);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.TakeProfit);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.ClosePrice);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Commission);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Swap);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Profit);
                                                                                Message.Append(",");
                                                                                Message.Append("Comment,");
                                                                                Message.Append(temp.Command.ID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Type.Name);
                                                                                Message.Append(",");
                                                                                Message.Append(1);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.ExpTime);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.ClientCode);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.CommandCode);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.IsHedged);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Type.ID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Margin);
                                                                                Message.Append(",Close,");
                                                                                Message.Append(temp.Command.CloseTime);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Bid);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Ask);

                                                                                if (temp.Command.Investor.ClientCommandQueue == null)
                                                                                    temp.Command.Investor.ClientCommandQueue = new List<string>();

                                                                                temp.Command.Investor.ClientCommandQueue.Add(Message.ToString());
                                                                                #endregion
                                                                            }

                                                                            isFlag = true;
                                                                            lock (Business.Market.nj4xObject)
                                                                                Business.Market.NJ4XTickets.RemoveAt(j);
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    NJ4XConnectSocket.NJ4XTicket temp;
                                                                    temp = Business.Market.NJ4XTickets[j];

                                                                    #region NOTIFY OPEN
                                                                    if (temp.Code.Trim() == subParameter[4].Trim() &&
                                                                        temp.IsDisable == false &&
                                                                        temp.IsReQuote == false)
                                                                    {
                                                                        if (temp.Execution == EnumMT4.Execution.REQUEST)
                                                                        {
                                                                            #region OPEN WITH MODE REQUEST
                                                                            bool IsBuy = false;
                                                                            if (temp.Command.Type.ID == 1 ||
                                                                                temp.Command.Type.ID == 7 ||
                                                                                temp.Command.Type.ID == 9)
                                                                                IsBuy = true;

                                                                            string CommandType = string.Empty;
                                                                            if (temp.Command.Type.ID == 1 ||
                                                                                temp.Command.Type.ID == 2)
                                                                            {
                                                                                CommandType = "Open";
                                                                            }
                                                                            else
                                                                            {
                                                                                if (temp.Command.Type.ID == 7)
                                                                                {
                                                                                    CommandType = "BuyLimit";
                                                                                }

                                                                                if (temp.Command.Type.ID == 8)
                                                                                {
                                                                                    CommandType = "SellLimit";
                                                                                }

                                                                                if (temp.Command.Type.ID == 9)
                                                                                {
                                                                                    CommandType = "BuyStop";
                                                                                }

                                                                                if (temp.Command.Type.ID == 10)
                                                                                {
                                                                                    CommandType = "SellStop";
                                                                                }
                                                                            }

                                                                            temp.Ask = double.Parse(subParameter[7]);
                                                                            temp.Bid = double.Parse(subParameter[6]);
                                                                            temp.IsDisable = true;
                                                                            temp.IsRequest = true;
                                                                            temp.IsReQuote = true;

                                                                            if (temp.Command.Type.ID == 1)
                                                                                temp.Command.OpenPrice = temp.Ask;
                                                                            else
                                                                                temp.Command.OpenPrice = temp.Bid;

                                                                            StringBuilder Message = new StringBuilder();
                                                                            Message.Append("AddCommand$False,RD28,");
                                                                            Message.Append(temp.Command.ID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Investor.InvestorID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Symbol.Name);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Size);
                                                                            Message.Append(",");
                                                                            Message.Append(IsBuy);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.OpenTime);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.OpenPrice);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.StopLoss);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.TakeProfit);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.ClosePrice);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Commission);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Swap);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Profit);
                                                                            Message.Append(",");
                                                                            Message.Append("Comment,");
                                                                            Message.Append(temp.Command.ID);
                                                                            Message.Append(",");
                                                                            Message.Append(CommandType);
                                                                            Message.Append(",");
                                                                            Message.Append(1);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.ExpTime);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.ClientCode);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.CommandCode);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.IsHedged);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Type.ID);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.Margin);
                                                                            Message.Append(",Open");
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Command.CloseTime);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Bid);
                                                                            Message.Append(",");
                                                                            Message.Append(temp.Ask);

                                                                            if (temp.Command.Investor.ClientCommandQueue == null)
                                                                                temp.Command.Investor.ClientCommandQueue = new List<string>();

                                                                            temp.Command.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                            //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Receive Notify] - " + Message);
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            #region OPEN WITH MODE INSTANCE
                                                                            if (temp.Symbol.Trim().ToUpper() == subParameter[12].Trim().ToUpper())
                                                                            {
                                                                                temp.Ask = double.Parse(subParameter[7]);
                                                                                temp.Bid = double.Parse(subParameter[6]);
                                                                                temp.Ticket = int.Parse(subParameter[9]);
                                                                                temp.IsDisable = true;
                                                                                temp.IsReQuote = true;

                                                                                bool IsBuy = false;
                                                                                if (temp.Command.Type.ID == 1 ||
                                                                                    temp.Command.Type.ID == 7 ||
                                                                                    temp.Command.Type.ID == 9)
                                                                                    IsBuy = true;

                                                                                if (temp.Command.Type.ID == 1)
                                                                                    temp.Command.OpenPrice = temp.Ask;
                                                                                else
                                                                                    temp.Command.OpenPrice = temp.Bid;

                                                                                StringBuilder Message = new StringBuilder();
                                                                                Message.Append("AddCommand$False,RD28,");
                                                                                Message.Append(temp.Command.ID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Investor.InvestorID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Symbol.Name);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Size);
                                                                                Message.Append(",");
                                                                                Message.Append(IsBuy);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.OpenTime);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.OpenPrice);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.StopLoss);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.TakeProfit);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.ClosePrice);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Commission);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Swap);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Profit);
                                                                                Message.Append(",");
                                                                                Message.Append("Comment,");
                                                                                Message.Append(temp.Command.ID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Type.Name);
                                                                                Message.Append(",");
                                                                                Message.Append(1);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.ExpTime);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.ClientCode);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.CommandCode);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.IsHedged);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Type.ID);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Command.Margin);
                                                                                Message.Append(",Close,");
                                                                                Message.Append(temp.Command.CloseTime);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Bid);
                                                                                Message.Append(",");
                                                                                Message.Append(temp.Ask);

                                                                                if (temp.Command.Investor.ClientCommandQueue == null)
                                                                                    temp.Command.Investor.ClientCommandQueue = new List<string>();

                                                                                temp.Command.Investor.ClientCommandQueue.Add(Message.ToString());

                                                                                //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Receive Notify] - " + Message);
                                                                            }
                                                                            #endregion

                                                                            isFlag = true;
                                                                            lock (Business.Market.nj4xObject)
                                                                                Business.Market.NJ4XTickets.RemoveAt(0);
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    string _temp = ex.Message + " - " + subValue[1];
                                                    TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                                }
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY CHANGE GROUP
                                    case "NotifyChangeGroup":
                                        {
                                            #region COMMENT CODE BECAUSE NEED TEST
                                            try
                                            {
                                                Business.Market.IsOpen = false;

                                                if (Business.Market.InvestorList != null)
                                                {
                                                    int countInvestor = Business.Market.InvestorList.Count;
                                                    for (int j = 0; j < countInvestor; j++)
                                                    {
                                                        if (Business.Market.InvestorList[j].IsOnline)
                                                        {
                                                            string message = "OLOFF14790251";
                                                            //int countInvestorOnline = Business.Market.InvestorList[j].CountInvestorOnline(Business.Market.InvestorList[j].InvestorID);
                                                            //if (countInvestorOnline > 0)
                                                            Business.Market.InvestorList[j].ClientCommandQueue.Add(message);
                                                        }

                                                        Business.Market.InvestorList[j].IsFirstLogin = true;
                                                    }
                                                }

                                                int countSymbol = Business.Market.SymbolList.Count;
                                                for (int j = 0; j < countSymbol;j++ )
                                                {
                                                    Business.Market.SymbolList[j].CommandList.Clear();
                                                }

                                                Business.Market.CommandExecutor.Clear();

                                                Business.Market.marketInstance.MapGroup(subValue[1]);

                                                Business.Market.IsOpen = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                            #endregion
                                        }
                                        break;
                                    #endregion

                                    #region NOTIFY CHANGE SYMBOL
                                    case "NotifyChangeSymbol":
                                        {
                                            #region COMMENT CODE BECAUSE NEED TEST
                                            try
                                            {
                                                Business.Market.IsOpen = false;

                                                if (Business.Market.InvestorList != null)
                                                {
                                                    int countInvestor = Business.Market.InvestorList.Count;
                                                    for (int j = 0; j < countInvestor; j++)
                                                    {
                                                        if (Business.Market.InvestorList[j].IsOnline)
                                                        {
                                                            string message = "OLOFF14790251";
                                                            //int countInvestorOnline = Business.Market.InvestorList[j].CountInvestorOnline(Business.Market.InvestorList[j].InvestorID);
                                                            //if (countInvestorOnline > 0)
                                                            Business.Market.InvestorList[j].ClientCommandQueue.Add(message);
                                                        }

                                                        Business.Market.InvestorList[j].IsFirstLogin = true;
                                                    }
                                                }

                                                //List<string> symbols = Element5SocketConnectMT4.MT4Connect.MT4ConnectPort.Instance.InitSymbolMT4();
                                                //Business.Market.marketInstance.ReceiveSymbolNotify(symbols);

                                                int countSymbol = Business.Market.SymbolList.Count;
                                                for (int j = 0; j < countSymbol; j++)
                                                {
                                                    Business.Market.SymbolList[j].CommandList.Clear();
                                                }

                                                Business.Market.CommandExecutor.Clear();

                                                Business.Market.marketInstance.MapSymbol(subValue[1]);

                                                Business.Market.IsOpen = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                string _temp = ex.Message + " - " + subValue[1];
                                                TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", subValue[0]);
                                            }
                                            #endregion
                                        }
                                        break;
                                    #endregion
                                }
                            }
                        }
                    }

                    result = Business.Market.GetNotifyMessage();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetNotifyMessage()
        {
            string result = string.Empty;
            try
            {
                if (Business.Market.NotifyMessageFromMT4.Count > 0)
                {
                    if (Business.Market.NotifyMessageFromMT4[0] != null)
                    {
                        result = Business.Market.NotifyMessageFromMT4[0];
                        Business.Market.NotifyMessageFromMT4.Remove(Business.Market.NotifyMessageFromMT4[0]);
                    }
                    else
                    {
                        Business.Market.NotifyMessageFromMT4.Remove(Business.Market.NotifyMessageFromMT4[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return result;
        }

        /// <summary>
        /// PROCESS NOTIFY FROM MT4(CASE CONNECT MT4)
        /// </summary>
        internal static void ProcessNotifyTickManagerAPI()
        {
            Business.ManagerAPITick result = null;
            while (Business.Market.IsProcessNotifyTick)
            {
                result = Business.Market.GetNotifyTickManagerAPI();

                while (result != null)
                {
                    bool isExits = Business.Market.Symbols.ContainsKey(result.Symbol);
                    if (isExits)
                    {
                        Business.Symbol newSymbol = Business.Market.Symbols[result.Symbol];

                        //newSymbol.TickValue.SymbolName = subTick[1];
                        newSymbol.TickValue.Bid = result.Bid;
                        newSymbol.TickValue.Ask = result.Ask;
                        newSymbol.TickValue.HighInDay = result.High;
                        newSymbol.TickValue.LowInDay = result.Low;
                        newSymbol.TickValue.StrTickTime = result.Time;
                        newSymbol.TickValue.Status = result.IsUp;

                        //Call Function Process Tick Value
                        ProcessQuoteLibrary.Business.QuoteProcess.ApplyQuote(1, newSymbol.TickValue.Status,
                                                                newSymbol.TickValue.Bid.ToString(),
                                                                newSymbol.TickValue.Ask.ToString(),
                                                                newSymbol.TickValue.SymbolName,
                                                                newSymbol.TickValue.TickTime);

                        //ADD TICK TO INVESTOR ONLINE(22/07/2011)
                        newSymbol.AddTickToInvestorOnline(newSymbol.TickValue);
                    }

                    result = Business.Market.GetNotifyTickManagerAPI();
                    
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Business.ManagerAPITick GetNotifyTickManagerAPI()
        {
            Business.ManagerAPITick result = null;
            try
            {
                if (Business.Market.NotifyTickFromManagerAPI.Count > 0)
                {
                    if (Business.Market.NotifyTickFromManagerAPI[0] != null)
                    {
                        result = Business.Market.NotifyTickFromManagerAPI[0];
                        Business.Market.NotifyTickFromManagerAPI.Remove(Business.Market.NotifyTickFromManagerAPI[0]);
                    }
                    else
                        Business.Market.NotifyTickFromManagerAPI.Remove(Business.Market.NotifyTickFromManagerAPI[0]);
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        /// <summary>
        /// PROCESS NOTIFY FROM MT4(CASE CONNECT MT4)
        /// </summary>
        internal static void ProcessNotifyTickMT4()
        {
            string result = string.Empty;
            while (Business.Market.IsProcessNotifyTick)
            {
                result = Business.Market.GetNotifyTickMT4();

                while (!string.IsNullOrEmpty(result))
                {
                    //Process Notify Message
                    string[] subCommand = result.Split('¬');

                    #region progress data
                    if (subCommand.Length > 0)
                    {
                        int count = subCommand.Length;
                        for (int i = 0; i < count; i++)
                        {
                            if (string.IsNullOrEmpty(subCommand[i]))
                                continue;

                            string[] subValue = subCommand[i].Split('$');
                            if (subValue.Length == 2)
                            {
                                string[] subParameter = subValue[1].Split('}');
                                if (subParameter.Length > 0)
                                {
                                    int countPara = subParameter.Length;
                                    for (int j = 0; j < countPara; j++)
                                    {
                                        if (!string.IsNullOrEmpty(subParameter[j]))
                                        {
                                            string[] subTick = subParameter[j].Split('{');

                                            bool isExits = Business.Market.Symbols.ContainsKey(subTick[1]);

                                            #region exists symbol
                                            if (isExits)
                                            {
                                                Business.Symbol newSymbol = Business.Market.Symbols[subTick[1]];

                                                //newSymbol.TickValue.SymbolName = subTick[1];
                                                newSymbol.TickValue.StrBid = subTick[2];
                                                newSymbol.TickValue.StrAsk = subTick[3];
                                                newSymbol.TickValue.StrHighInDay = subTick[4];
                                                newSymbol.TickValue.StrLowInDay = subTick[5];
                                                newSymbol.TickValue.StrTickTime = subTick[7];

                                                string _status = "down";
                                                if (int.Parse(subTick[0]) == 1)
                                                    _status = "up";

                                                newSymbol.TickValue.Status = _status;

                                                //Call Function Process Tick Value
                                                ProcessQuoteLibrary.Business.QuoteProcess.ApplyQuote(1, newSymbol.TickValue.Status,
                                                                                        newSymbol.TickValue.Bid.ToString(),
                                                                                        newSymbol.TickValue.Ask.ToString(),
                                                                                        newSymbol.TickValue.SymbolName,
                                                                                        newSymbol.TickValue.TickTime);

                                                //ADD TICK TO INVESTOR ONLINE(22/07/2011)
                                                newSymbol.AddTickToInvestorOnline(newSymbol.TickValue);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    System.Threading.Thread.Sleep(10);
                    result = Business.Market.GetNotifyTickMT4();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetNotifyTickMT4()
        {
            string result = string.Empty;
            try
            {
                if (Business.Market.NotifyTickFromMT4.Count > 0)
                {
                    if (Business.Market.NotifyTickFromMT4[0] != null)
                    {
                        result = Business.Market.NotifyTickFromMT4[0];
                        Business.Market.NotifyTickFromMT4.Remove(Business.Market.NotifyTickFromMT4[0]);
                    }
                    else
                        Business.Market.NotifyTickFromMT4.Remove(Business.Market.NotifyTickFromMT4[0]);
                }
            }
            catch (Exception ex)
            {
                
            }

            return result;
        }
    }
}
