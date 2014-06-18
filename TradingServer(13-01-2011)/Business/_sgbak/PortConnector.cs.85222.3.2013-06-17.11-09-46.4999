using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TradingServer.Business
{
    internal class PortConnector
    {
        internal static Business.PortConnector portConnection = new PortConnector();
        
        /// <summary>
        /// MAKE COMMAND OF MARKETAREA SPOT COMMAND
        /// </summary>
        /// <param name="Cmd"></param>
        internal void MakeCommand(string Cmd)
        {
            Business.OpenTrade Command = new OpenTrade();
            Command = Model.CommandFramework.CommandFrameworkInstance.ExtractCommand(Cmd);
                       
            if (Command.Symbol.IsTrade == false)
            {
                string Message = "CMD12369$False,CM034";
                Command.Investor.ClientCommandQueue.Add(Message);
                return;
            }

            if (Business.Market.IsOpen == false)
            {
                string Message = "CMD12369$False,CM033";
                Command.Investor.ClientCommandQueue.Add(Message);
                return;
            }

            if (Command.Symbol.IsHoliday == 1)
            {
                string Message = "CMD12369$False,CM035";
                Command.Investor.ClientCommandQueue.Add(Message);
                return;
            }

            #region Get Setting IsTrade In Group
            bool IsTradeGroup = false;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G01")
                                {
                                    if (Business.Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                        IsTradeGroup = true;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion

            if (IsTradeGroup == true)
            {
                if (Command.Symbol == null || Command.Investor == null || Command.Type == null || Command.IGroupSecurity == null)
                {
                    #region Check Investor != null Then Return Error To Client
                    if (Command.Investor != null)
                    {
                        //Add Result To Client Command Queue Of Investor
                        string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM001", Command);
                                                
                        Command.Investor.ClientCommandQueue.Add(Message);
                    }
                    #endregion

                    return;                    
                }   //End If Check Instant Symbol,Investor,Type,IGroupSecurity
                else
                {
                    #region Check Lots Minimum And Maximum In IGroupSecurity And Check IsTrade In IGroupSecurity
                    bool IsTrade = false;
                    double Minimum = -1;
                    double Maximum = -1;
                    double Step = -1;
                    bool ResultCheckStepLots = false;

                    #region Get Config IGroupSecurity
                    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
                    {
                        int countIGroupSecurityConfig = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                        for (int i = 0; i < countIGroupSecurityConfig; i++)
                        {
                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B01")
                            {
                                if (Command.IGroupSecurity.IGroupSecurityConfig[i].BoolValue == 1)
                                    IsTrade = true;
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B11")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Minimum);
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B12")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Maximum);
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B13")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Step);
                            }
                        }
                    }
                    #endregion

                    if (IsTrade == true)
                    {
                        ResultCheckStepLots = Command.IGroupSecurity.CheckStepLots(Minimum, Maximum, Step, Command.Size);

                        #region If Check Step Lots False Return Client
                        if (ResultCheckStepLots == false)
                        {
                            string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM002", Command);
                                                        
                            Command.Investor.ClientCommandQueue.Add(Message);

                            return;
                        }
                        #endregion

                        #region Check Status Trade Of Symbol(Full Access,Close Only, No)
                        if (Command.Symbol.Trade != "Full access")
                        {
                            string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM003", Command);

                            Command.Investor.ClientCommandQueue.Add(Message);

                            return;
                        }
                        #endregion

                        #region Check Stop Loss And Take Profit
                        if (Command.StopLoss > 0 || Command.TakeProfit > 0)
                        {
                            if (Command.Type.ID == 1 || Command.Type.ID == 2)
                            {
                                #region Check Limit And Stop Of Open Trade
                                bool ResultCheckLimit = false;

                                ResultCheckLimit = Command.Symbol.CheckLimitAndStop(Command.Symbol.Name, Command.Type.ID, Command.StopLoss, Command.TakeProfit,
                                                                    Command.Symbol.LimitStopLevel, Command.Symbol.Digit);

                                if (ResultCheckLimit == false)
                                {
                                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM004", Command);

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    return;
                                }
                                #endregion
                            }
                            else if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10)
                            {
                                #region Check Limit And Stop Of Pending Order
                                bool ResultCheckLimit = false;
                                ResultCheckLimit = Command.Symbol.CheckLimitAndStopPendingOrder(Command.Symbol.Name, Command.Type.ID, Command.OpenPrice,
                                    Command.StopLoss, Command.TakeProfit, Command.Symbol.LimitStopLevel, Command.Symbol.Digit);

                                if (ResultCheckLimit == false)
                                {
                                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM005", Command);

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    return;
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region Check Setting IsLong
                        switch (Command.Type.ID)
                        {
                            #region Case Sell
                            case 2:
                                {
                                    if (Command.Symbol.LongOnly == true)
                                    {
                                        string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM006", Command);

                                        Command.Investor.ClientCommandQueue.Add(Message);

                                        return;
                                    }
                                }
                                break;
                            #endregion

                            #region Case Sell Limit
                            case 8:
                                {
                                    if (Command.Symbol.LongOnly == true)
                                    {
                                        string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM006", Command);

                                        Command.Investor.ClientCommandQueue.Add(Message);

                                        return;
                                    }
                                }
                                break;
                            #endregion

                            #region Case Sell Stop
                            case 10:
                                {
                                    if (Command.Symbol.LongOnly == true)
                                    {
                                        string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM006", Command);

                                        Command.Investor.ClientCommandQueue.Add(Message);

                                        return;
                                    }
                                }
                                break;
                            #endregion
                        }
                        #endregion

                        #region If Command Type == 1 Or 2 Then Call Dealer Else Then Call Function In MarketArea(SpotCommand, FutureCommand....)
                        if (Command.Type.ID == 1 || Command.Type.ID == 2)
                        {
                            //Call Dealer
                            Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                            newRequestDealer.InvestorID = Command.Investor.InvestorID;
                            newRequestDealer.MaxDev = Command.MaxDev;
                            newRequestDealer.Name = "Open";
                            newRequestDealer.Request = Command;
                            newRequestDealer.TimeClientRequest = DateTime.Now;

                            TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);
                        }
                        else
                        {
                            Command.Symbol.MarketAreaRef.AddCommand(Command);
                        }
                        #endregion                        
                    }   //End If Check IsTrade In IGroupSecurity
                    else
                    {
                        string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM007", Command);
                                                
                        Command.Investor.ClientCommandQueue.Add(Message);

                        return;
                    }   //End If Check Is Trade In IGroupSecurity
                    #endregion
                }   //End Else Check Instant Symbol,Investor,Type,IGroupSecurity
            }   //End If Check IsTrade In Group
            else
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(1, false, "CM008", Command);

                    Command.Investor.ClientCommandQueue.Add(Message);
                }

                return;
            }   //End Else Check IsTrade In Group
        }

        /// <summary>
        /// CLOSE COMMAND OF MARKETAREA SPOT COMMAND
        /// </summary>
        /// <param name="CommandID"></param>
        internal void CloseCommand(string Cmd)
        {
            Business.OpenTrade Command = new Business.OpenTrade();
            Command = Model.CommandFramework.CommandFrameworkInstance.ExtractCommand(Cmd);

            #region Find In Symbol List And Remove Command
            //Command = TradingServer.Facade.FacadeFindOpenTradeInCommandExecutor(CommandID);
            #endregion

            TradingServer.Facade.FacadeFillInstanceToCommand(Command);

            #region Check Status Trade Of Symbol(Full Access,Close Only, No)
            if (Command.Symbol.Trade == "No")
            {
                string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(2, false, "CM009", Command);
                                
                Command.Investor.ClientCommandQueue.Add(Message);

                return;
            }
            #endregion

            if (Command.Symbol != null && Command.Investor != null && Command.Type != null && Command.IGroupSecurity != null)
            {
                if (Command.Type.ID == 1 || Command.Type.ID == 2)
                {
                    //Call Dealer
                    Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                    newRequestDealer.InvestorID = Command.Investor.InvestorID;
                    newRequestDealer.MaxDev = Command.MaxDev;
                    newRequestDealer.Name = "Close";
                    newRequestDealer.Request = Command;
                    newRequestDealer.TimeClientRequest = DateTime.Now;

                    TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);
                }
                else
                {
                    Command.Symbol.MarketAreaRef.CloseCommand(Command);
                }
            }
            else
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(2, false, "CM001", Command);

                    Command.Investor.ClientCommandQueue.Add(Message);
                }
            }
        }

        /// <summary>
        /// UPDATE COMMAND OF MARKET AREA SPOT COMMAND
        /// </summary>
        /// <param name="Cmd"></param>
        internal void UpdateCommand(string Cmd)
        {
            Business.OpenTrade Result = new OpenTrade();
            Result = Model.CommandFramework.CommandFrameworkInstance.ExtractCommand(Cmd);

            if (Result.Symbol != null && Result.Investor != null && Result.Type != null && Result.IGroupSecurity != null)
            {
                #region Check Valid Take Profit And Stop Loss
                bool ResultTakeProfit = false;
                ResultTakeProfit = Result.Symbol.CheckLimitAndStop(Result.Symbol.Name, Result.Type.ID, Result.StopLoss, Result.TakeProfit,
                                                                    Result.Symbol.LimitStopLevel, Result.Symbol.Digit);

                if (ResultTakeProfit == false)
                {
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(3, false, "CM018", Result);

                    Result.Investor.ClientCommandQueue.Add(Message);
                    return;
                }
                #endregion

                #region Call Check FreezeLevel
                //Call Check FreezeLevel
                ResultTakeProfit = Result.Symbol.CheckLimitAndStop(Result.Symbol.Name, Result.Type.ID, Result.StopLoss, Result.TakeProfit,
                                                    Result.Symbol.FreezeLevel, Result.Symbol.Digit);

                if (ResultTakeProfit == false)
                {
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(3, false, "CM019", Result);

                    Result.Investor.ClientCommandQueue.Add(Message);
                    return;
                }
                #endregion            

                Result.Symbol.MarketAreaRef.UpdateCommand(Result);
            }
            else
            {
                string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(3, false, "CM001", Result);

                Result.Investor.ClientCommandQueue.Add(Message);

                return;
            }
        }

        /// <summary>
        /// MAKE BINARY COMMAND OF MARKETAREA BINARY
        /// </summary>
        /// <param name="Cmd"></param>
        internal void MakeBinaryCommand(string Cmd)
        {
            Business.OpenTrade Command = new OpenTrade();
            Command = Model.CommandFramework.CommandFrameworkInstance.ExtractCommand(Cmd);
            
            #region Get Setting IsTrade In Group
            bool IsTradeGroup = false;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G01")
                                {
                                    if (Business.Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                        IsTradeGroup = true;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion

            if (IsTradeGroup == true)
            {
                if (Command.Symbol == null || Command.Investor == null || Command.Type == null || Command.IGroupSecurity == null)
                {
                    #region Session Close
                    //Add String Command Server To Client
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(4, false, "CM001", Command);

                    Command.Investor.ClientCommandQueue.Add(Message);
                    #endregion

                    return;
                }   //End If Check Instance == Null
                else
                {
                    #region Check Lots Minimum And Maximum In IGroupSecurity And Check IsTrade In IGroupSecurity
                    bool IsTrade = false;
                    double Minimum = -1;
                    double Maximum = -1;
                    double Step = -1;
                    bool ResultCheckStepLots = false;

                    #region Get Config IGroupSecurity
                    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
                    {
                        int countIGroupSecurityConfig = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                        for (int i = 0; i < countIGroupSecurityConfig; i++)
                        {
                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B01")
                            {
                                if (Command.IGroupSecurity.IGroupSecurityConfig[i].BoolValue == 1)
                                    IsTrade = true;
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B11")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Minimum);
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B12")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Maximum);
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B13")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Step);
                            }
                        }
                    }
                    #endregion

                    if (IsTrade == true)
                    {
                        ResultCheckStepLots = Command.IGroupSecurity.CheckStepLots(Minimum, Maximum, Step, Command.Size);

                        #region If Check Step Lots False Return Client
                        if (ResultCheckStepLots == false)
                        {
                            string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(4, false, "CM013", Command);

                            Command.Investor.ClientCommandQueue.Add(Message);

                            return;
                        }
                        #endregion

                        //Call Function Make Binary Command In MarketArea BinaryCommand
                        Command.Symbol.MarketAreaRef.AddCommand(Command);
                    }   //End If Check IsTrade In IGroupSecurity
                    else
                    {
                        #region Check IsTrade  == False
                        string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(4, false, "CM014", Command);

                        Command.Investor.ClientCommandQueue.Add(Message);

                        return;
                        #endregion
                    }   //End Else Check IsTrade In IGroupSecurity
                    #endregion
                }   //End Else Check Instance == Null
            }   //End If Check IsTrade In Group
            else
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(4, false, "CM015", Command);

                    Command.Investor.ClientCommandQueue.Add(Message);
                }

                return;
            }   //End Else Check IsTrade In Group
        }

        /// <summary>
        /// CANCEL BINARY COMMAND OF MARKETAREA BINARY
        /// </summary>
        /// <param name="InvestorID"></param>
        internal void CancelBinaryCommand(int CommandID)
        {
            Business.OpenTrade Result = new OpenTrade();
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == CommandID)
                    {
                        #region Find Command In Command Executor
                        Result.ClientCode = Business.Market.CommandExecutor[i].ClientCode;
                        Result.ClosePrice = Business.Market.CommandExecutor[i].ClosePrice;
                        Result.CloseTime = Business.Market.CommandExecutor[i].CloseTime;
                        Result.CommandCode = Business.Market.CommandExecutor[i].CommandCode;
                        Result.Commission = Business.Market.CommandExecutor[i].Commission;
                        Result.ExpTime = Business.Market.CommandExecutor[i].ExpTime;
                        Result.ID = Business.Market.CommandExecutor[i].ID;
                        Result.IsClose = Business.Market.CommandExecutor[i].IsClose;
                        Result.Margin = Business.Market.CommandExecutor[i].Margin;
                        Result.OpenPrice = Business.Market.CommandExecutor[i].OpenPrice;
                        Result.OpenTime = Business.Market.CommandExecutor[i].OpenTime;
                        Result.Profit = Business.Market.CommandExecutor[i].Profit;
                        Result.Size = Business.Market.CommandExecutor[i].Size;
                        Result.StopLoss = Business.Market.CommandExecutor[i].StopLoss;
                        Result.Swap = Business.Market.CommandExecutor[i].Swap;
                        Result.Symbol = Business.Market.CommandExecutor[i].Symbol;
                        Result.TakeProfit = Business.Market.CommandExecutor[i].TakeProfit;
                        Result.Investor = Business.Market.CommandExecutor[i].Investor;
                        Result.Type = Business.Market.CommandExecutor[i].Type;

                        if (TradingServer.Business.BinaryCommand.isTrade == true)
                        {
                            //Call Function Remove In Investor List
                            Result.Symbol.MarketAreaRef.CloseCommand(Result);

                            return;
                        }
                        else
                        {
                            string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(5, false, "CM016", Business.Market.CommandExecutor[i]);

                            Result.Investor.ClientCommandQueue.Add(Message);

                            return;
                        }
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// GET ONLINE COMMAND BY INVESTOR ID OF MARKETAREA SPOT COMMAND
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<string> GetOnlineCommandByInvestorID(int InvestorID)
        {
            List<string> Result = new List<string>();
            List<Business.OpenTrade> OpenTrade = new List<OpenTrade>();
            OpenTrade = TradingServer.Facade.FacadeGetOnlineCommandByInvestorID(InvestorID);
            if (OpenTrade != null && OpenTrade.Count > 0)
            {
                int count = OpenTrade.Count;
                for (int i = 0; i < count; i++)
                {
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(OpenTrade[i]);
                    Result.Add(Message);
                }
            }

            return Result;
        }

        /// <summary>
        /// GET ONLINE COMMAND BY ONLINE COMMAND ID OF MARKETARE SPOT COMMAND
        /// </summary>
        /// <param name="OpenTradeID"></param>
        /// <returns></returns>
        internal string GetOpenTradeByOpenTradeID(int OpenTradeID)
        {
            string Result = string.Empty;
            Business.OpenTrade OpenTrade = new OpenTrade();
            OpenTrade = TradingServer.Facade.FacadeGetOpenTradeByOpenTradeID(OpenTradeID);
            if (OpenTrade != null)
            {
                Result = OpenTrade.ClientCode + "," + OpenTrade.ClosePrice + "," + OpenTrade.CloseTime + "," +
                            OpenTrade.CommandCode + "," + OpenTrade.Commission + "," + OpenTrade.ExpTime + "," +
                            OpenTrade.ID + "," + OpenTrade.Investor.InvestorID + "," + OpenTrade.IsClose + "," +
                            OpenTrade.IsHedged + "," + OpenTrade.Margin + "," + OpenTrade.MaxDev + "," + OpenTrade.OpenPrice + "," +
                            OpenTrade.OpenTime + "," + OpenTrade.Profit + "," + OpenTrade.Size + "," + OpenTrade.StopLoss + "," +
                            OpenTrade.Swap + "," + OpenTrade.Symbol.Name + "," + OpenTrade.TakeProfit + "," + OpenTrade.Taxes + "," +
                            OpenTrade.Type.Name + "," + OpenTrade.Type.ID + "," + OpenTrade.Symbol.ContractSize;
            }

            return Result;
        }

        /// <summary>
        /// GET PENDING ORDER BY INVESTOR ID OF MARKETAREA SPOT COMMAND
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<string> GetPendingOrderByInvestorID(int InvestorID)
        {
            List<string> Result = new List<string>();
            List<Business.OpenTrade> OpenTrade = new List<OpenTrade>();
            OpenTrade = TradingServer.Facade.FacadeGetPendingOrderByInvestorID(InvestorID);
            if (OpenTrade != null && OpenTrade.Count > 0)
            {
                int count=OpenTrade.Count;
                for (int i = 0; i < count; i++)
                { 
                  string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(OpenTrade[i]);  
                    Result.Add(Message);
                }
            }

            return Result;
        }

        /// <summary>
        /// GET BINARY COMMAND BY INVESTOR ID OF MARKETAREA BINARY COMMAND
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<string> GetBinaryCommandByInvestorID(int InvestorID)
        {
            List<string> Result = new List<string>();
            List<Business.OpenTrade> OpenTrade = new List<OpenTrade>();
            OpenTrade = TradingServer.Facade.FacadeGetBinaryCommandByInvestorID(InvestorID);
            if (OpenTrade != null && OpenTrade.Count > 0)
            {
                int count = OpenTrade.Count;
                for (int i = 0; i < count; i++)
                {
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(OpenTrade[i]);
                    Result.Add(Message);
                }
            }

            return Result;
        }

        /// <summary>
        /// GET COMMAND HISTORY BY INVESTOR IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<string> GetCommandHistoryByInvestorID(int InvestorID)
        {
            List<string> Result = new List<string>();
            List<Business.OpenTrade> OpenTrade = new List<Business.OpenTrade>();
            OpenTrade = TradingServer.Facade.FacadeGetCommandHistoryByInvestor(InvestorID);
            if (OpenTrade != null)
            {
                int count = OpenTrade.Count;
                for (int i = 0; i < count; i++)
                {
                    string Message = Model.CommandFramework.CommandFrameworkInstance.ExtractCommandToString(OpenTrade[i]);
                    Result.Add(Message);
                }
            }

            return Result;
        }

        /// <summary>
        /// CLIENT LOGIN TRADING SYSTEM
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal List<string> LoginSystem(string Code, string Pwd)
        {
            List<string> Result = new List<string>();
            Business.Investor InvestorLogin = new Investor();
            InvestorLogin = TradingServer.Facade.FacadeLoginServer(Code, Pwd);
            if (InvestorLogin != null)
            {
                string tempLogin = string.Empty;
                tempLogin = "L001$" + Model.CommandFramework.CommandFrameworkInstance.ExtractInvestorToString(InvestorLogin);
                Result.Add(tempLogin);

                #region GET LIST IGROUP SECURITY CONFIG
                if (InvestorLogin.ListIGroupSecurity != null && InvestorLogin.ListIGroupSecurity.Count > 0)
                {
                    int countIGroupSecurity = InvestorLogin.ListIGroupSecurity.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        string MessageIGroupSecurity = "L002$" + Model.CommandFramework.CommandFrameworkInstance.ExtractIGroupSecurityToString(InvestorLogin.ListIGroupSecurity[i]);
                        MessageIGroupSecurity = MessageIGroupSecurity.Remove(MessageIGroupSecurity.Length - 1);

                        if (Business.Market.SecurityList != null)
                        {
                            int countSecurity = Business.Market.SecurityList.Count;
                            for (int n = 0; n < countSecurity; n++)
                            {
                                if (Business.Market.SecurityList[n].SecurityID == InvestorLogin.ListIGroupSecurity[i].SecurityID)
                                {
                                    string MessageSecurity = "S01$" + Business.Market.SecurityList[n].SecurityID + "," + Business.Market.SecurityList[n].Name + "," +
                                        Business.Market.SecurityList[n].Description + "," + Business.Market.SecurityList[n].MarketAreaID;

                                    Result.Add(MessageSecurity);

                                    break;
                                }
                            }
                        }

                        Result.Add(MessageIGroupSecurity);
                    }
                }
                #endregion

                #region GET LIST IGROUP SYMBOL CONFIG
                if (InvestorLogin.ListIGroupSymbol != null && InvestorLogin.ListIGroupSymbol.Count > 0)
                {
                    int countIGroupSymbol = InvestorLogin.ListIGroupSymbol.Count;
                    for (int j = 0; j < countIGroupSymbol; j++)
                    {
                        string MessageIGroupSymbol = "L003$" + Model.CommandFramework.CommandFrameworkInstance.ExtractIGroupSymbolToString(InvestorLogin.ListIGroupSymbol[j]);
                        MessageIGroupSymbol = MessageIGroupSymbol.Remove(MessageIGroupSymbol.Length - 1);

                        Result.Add(MessageIGroupSymbol);
                    }
                }
                #endregion

                #region GET LIST SYMBOL BY INVESTOR
                if (InvestorLogin.ListSymbol != null && InvestorLogin.ListSymbol.Count > 0)
                {
                    int countSymbol = InvestorLogin.ListSymbol.Count;
                    for (int n = 0; n < countSymbol; n++)
                    {
                        string MessageSymbol = "L004$" + Model.CommandFramework.CommandFrameworkInstance.ExtractSymbolToString(InvestorLogin.ListSymbol[n]);

                        Result.Add(MessageSymbol);
                    }
                }
                #endregion
            }
            else
            {
                string tempLogin = "L001$-1";
                Result.Add(tempLogin);                
            }

            return Result;
        }

        /// <summary>
        /// LOGOUT TRADING SYSTEM
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal void LogoutSystem(int InvestorID)
        {
            TradingServer.Facade.FacadeLogoutSystem(InvestorID);
        }

        /// <summary>
        /// GET INVESTOR ACCOUNT BY INVESTOR ID IN MARKET
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal string GetInvestorByInvestorID(int InvestorID)
        {
            string Result = string.Empty;
            Business.Investor Investor = new Business.Investor();
            Investor = TradingServer.Facade.FacadeGetInvestorAccountByInvestorID(InvestorID);
            Result = Model.CommandFramework.CommandFrameworkInstance.ExtractInvestorToString(Investor);

            return Result;
        }

        /// <summary>
        /// CLOSE COMMAND BY MANAGER
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal bool CloseCommandByManager(int CommandID)
        {
            bool Result = false;
            Business.OpenTrade OpenTrade = new OpenTrade();
            OpenTrade = TradingServer.Facade.FacadeFindOpenTradeInCommandExecutor(CommandID);

            if (OpenTrade != null)
            {
                OpenTrade.Symbol.MarketAreaRef.CloseCommand(OpenTrade);

                Result = true;
            }

            return Result;
        }

        /// <summary>
        /// GET TICK ONLINE
        /// </summary>
        /// <returns></returns>
        internal List<string> GetTickOnline()
        {
            List<string> Result = new List<string>();
            if (Business.Market.SymbolList != null)
            {
                int countTick = Business.Market.SymbolList.Count;
                for (int j = 0; j < countTick; j++)
                {
                    if (Business.Market.SymbolList[j].TickValue != null)
                    {   
                        string Message = "T01$" + Business.Market.SymbolList[j].TickValue.Ask.ToString(CultureInfo.InvariantCulture) + "," + 
                            Business.Market.SymbolList[j].TickValue.Bid.ToString(CultureInfo.InvariantCulture) + "," +
                            Business.Market.SymbolList[j].TickValue.Status + "," + Business.Market.SymbolList[j].TickValue.SymbolID + "," +
                            Business.Market.SymbolList[j].TickValue.SymbolName + "," + Business.Market.SymbolList[j].TickValue.TickTime;

                        Result.Add(Message);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// GET COMMAND QUEUE OF CLIENT
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<string> GetCommandQueueOfClient(int InvestorID)
        {
            List<string> Result = new List<string>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        Result = Business.Market.InvestorList[i].ClientCommandQueue;
                        Business.Market.InvestorList[i].ClientCommandQueue.Clear();

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// GET DATA SERVER
        /// </summary>
        /// <returns></returns>
        internal List<string> GetDataServer(int InvestorID)
        {
            List<string> Result = new List<string>();

            #region GET ALL TICK ONLINE
            Result = this.GetTickOnline();
            #endregion

            #region GET COMMAND OF SERVER SEND TO CLIENT
            List<string> tempClientCommandQueue = new List<string>();
            tempClientCommandQueue = this.GetCommandQueueOfClient(InvestorID);
            if (tempClientCommandQueue != null && tempClientCommandQueue.Count > 0)
            {
                int count = tempClientCommandQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    Result.Add(tempClientCommandQueue[i]);
                }
            }
            #endregion

            return Result;
        }
    }
}
