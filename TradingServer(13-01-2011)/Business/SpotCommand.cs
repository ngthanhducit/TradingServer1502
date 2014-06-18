using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;

namespace TradingServer.Business
{
    public class SpotCommand : IPresenter.IMarketArea
    {
        public IPresenter.AddCommandDelegate AddCommandNotify { get; set; }
        public int IMarketAreaID { get; set; }
        public Market MarketContainer { get; set; }
        List<TradeType> IPresenter.IMarketArea.Type { get; set; }
        public string IMarketAreaName { get; set; }
        public List<Symbol> ListSymbol { get; set; }
        public List<ParameterItem> MarketAreaConfig { get; set; }
        public delegate void SendCommandID(int ID);
        public static int NumCheck { get; set; }
        //===================================================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void AddCommand(OpenTrade Command)
        {   
            #region Set Property IsBuy And CommandType Send To Client
            bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

            string CommandType = string.Empty;
            CommandType = Model.Helper.Instance.convertCommandTypeIDToString(Command.Type.ID);
            #endregion                    

            int Result = -1;

            Command.IsClose = false;

            if (!Command.IsReOpen && !Command.IsReNewOpen)
                Command.OpenTime = DateTime.Now;

            Command.CloseTime = DateTime.Now;
            
            Command.ExpTime = DateTime.Now;
            
            Command.Taxes = 0;

            #region Find Price Close Of Symbol
            switch (Command.Type.ID)
            {
                case 1:
                    Command.ClosePrice = Command.Symbol.TickValue.Bid;
                    break;
                case 2:
                    Command.ClosePrice = Command.Symbol.TickValue.Ask;
                    break;
                case 7: //BUY LIMIT
                    Command.ClosePrice = Command.Symbol.TickValue.Bid;                    
                    break;
                case 8: //SELL LIMIT
                    Command.ClosePrice = Command.Symbol.TickValue.Ask;                    
                    break;
                case 9: //BUY STOP
                    Command.ClosePrice = Command.Symbol.TickValue.Bid;                    
                    break;
                case 10:    //SELL STOP
                    Command.ClosePrice = Command.Symbol.TickValue.Ask;
                    break;
            }
            #endregion

            string CommandCode = string.Empty;

            #region INSERT SYSTEM LOG EVENT MAKE COMMAND
            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

            string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);
            string content = string.Empty;
            string comment = string.Empty;
            #endregion

            //====================MT4 CONNECT MAKE COMMAND ==============================
            if (Business.Market.IsConnectMT4)
            {
                if (!Business.Market.StatusConnect)
                {
                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.AddCommandByManager, EnumET5.ET5Message.DISCONNECT_FROM_MT4, false, -1, 0);

                    //string Message = "AddCommandByManager$False,DISCONNECT FROM MT4," + Result + "," + Command.Investor.InvestorID + "," +
                    //      Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                    //          Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                    //          CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    //if (Command.Investor.ClientCommandQueue == null)
                    //    Command.Investor.ClientCommandQueue = new List<string>();

                    //Command.Investor.ClientCommandQueue.Add(Message);
                }

                string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");

                if (executionType == "manual only- no automation" ||
                    executionType == "manual- but automatic if no dealer online" ||
                    executionType == "automatic only")
                {
                    #region CONNECT MT4 USING NJ4X
                    //CONNECT TO NJ4X
                    switch (Command.Symbol.ExecutionTrade)
                    {
                        case EnumMT4.Execution.REQUEST:
                            {
                                bool isFlag = false;
                                if (Business.Market.NJ4XTickets != null)
                                {
                                    int count = Business.Market.NJ4XTickets.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.NJ4XTickets[i].IsRequest == true &&
                                            Business.Market.NJ4XTickets[i].IsDisable == true &&
                                            Business.Market.NJ4XTickets[i].Code == Command.Investor.Code)
                                        {
                                            #region CALL MAKE COMMAND MT4
                                            string cmd = string.Empty;

                                            #region BUILD COMMAND
                                            switch (Command.Type.ID)
                                            {
                                                case 1:
                                                    cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertBuyToString(Command.Investor.Code, Command.OpenPrice, Command.Size,
                                                                                    Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                                                    break;

                                                case 2:
                                                    cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertSellToString(Command.Investor.Code, Command.OpenPrice,
                                                        Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                                                    break;

                                                case 7:
                                                    cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertBuyLimitToString(Command.Investor.Code, Command.OpenPrice,
                                                        Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                                                    break;

                                                case 8:
                                                    cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertSellLimitToString(Command.Investor.Code, Command.OpenPrice,
                                                        Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                                                    break;

                                                case 9:
                                                    cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertBuyStopToString(Command.Investor.Code, Command.OpenPrice,
                                                        Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                                                    break;

                                                case 10:
                                                    cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertSellStopToString(Command.Investor.Code, Command.OpenPrice,
                                                        Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                                                    break;
                                            }
                                            #endregion

                                            string resultMT4 = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                                            bool isMakeCommandSuccess = false;
                                            string strError = string.Empty;
                                            if (!string.IsNullOrEmpty(resultMT4))
                                            {
                                                string[] subValue = resultMT4.Split('$');
                                                if (subValue.Length == 2)
                                                {
                                                    string[] subParameter = subValue[1].Split('{');

                                                    if (int.Parse(subParameter[0]) == 1)
                                                        isMakeCommandSuccess = true;

                                                    if (!isMakeCommandSuccess)
                                                        strError = subParameter[1];
                                                }
                                            }
                                            #endregion

                                            #region NOTIFY COMMAND TO CLIENT IF MAKE COMMAND IN MT4 FALSE
                                            if (!isMakeCommandSuccess)
                                            {
                                                if (Command.IsServer)
                                                {
                                                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.AddCommandByManager, EnumET5.ET5Message.MT4_CAN_NOT_MAKE_COMMAND, false, -1, 0);

                                                    //string Message = "AddCommandByManager$False,MT4 CAN'T MAKE COMMAND," + Result + "," + Command.Investor.InvestorID + "," +
                                                    //   Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                                    //       Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                                    //       CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                                    //if (Command.Investor.ClientCommandQueue == null)
                                                    //    Command.Investor.ClientCommandQueue = new List<string>();

                                                    //Command.Investor.ClientCommandQueue.Add(Message);
                                                }
                                                else
                                                {
                                                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.MT4_CAN_NOT_MAKE_COMMAND, false, -1, 0);

                                                    //string Message = "AddCommand$False,MT4 CAN'T MAKE COMMAND," + Result + "," + Command.Investor.InvestorID + "," +
                                                    //    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                                    //        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                                    //        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                                    //if (Command.Investor.ClientCommandQueue == null)
                                                    //    Command.Investor.ClientCommandQueue = new List<string>();

                                                    //Command.Investor.ClientCommandQueue.Add(Message);
                                                }

                                                return;
                                            }
                                            #endregion

                                            isFlag = true;

                                            lock (Business.Market.nj4xObject)
                                                Business.Market.NJ4XTickets.RemoveAt(i);

                                            break;
                                        }
                                    }
                                }

                                if (!isFlag)
                                {
                                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
                                    if (!isPending)
                                    {
                                        Command.OpenPrice = 0;
                                    }   

                                    //make command using nj4x
                                    this.OrderSendNj4x(Command);
                                }
                            }
                            break;

                        case EnumMT4.Execution.INSTANT:
                            {
                                this.OrderSendNj4x(Command);
                            }
                            break;

                        case EnumMT4.Execution.MARKET:
                            this.OrderSendNj4x(Command);
                            break;
                    }
                    #endregion
                }
                else
                {
                    #region CALL MAKE COMMAND MT4
                    string cmd = string.Empty;

                    #region BUILD COMMAND
                    switch (Command.Type.ID)
                    {
                        case 1:
                            cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertBuyToString(Command.Investor.Code, Command.OpenPrice, Command.Size,
                                                            Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                            break;

                        case 2:
                            cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertSellToString(Command.Investor.Code, Command.OpenPrice,
                                Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                            break;

                        case 7:
                            cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertBuyLimitToString(Command.Investor.Code, Command.OpenPrice,
                                Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                            break;

                        case 8:
                            cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertSellLimitToString(Command.Investor.Code, Command.OpenPrice,
                                Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                            break;

                        case 9:
                            cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertBuyStopToString(Command.Investor.Code, Command.OpenPrice,
                                Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                            break;

                        case 10:
                            cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertSellStopToString(Command.Investor.Code, Command.OpenPrice,
                                Command.Size, Command.StopLoss, Command.TakeProfit, Command.Symbol.Name, Command.Comment);
                            break;
                    }
                    #endregion

                    string resultMT4 = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                    bool isMakeCommandSuccess = false;
                    string strError = string.Empty;
                    if (!string.IsNullOrEmpty(resultMT4))
                    {
                        string[] subValue = resultMT4.Split('$');
                        if (subValue.Length == 2)
                        {
                            string[] subParameter = subValue[1].Split('{');

                            if (int.Parse(subParameter[0]) == 1)
                                isMakeCommandSuccess = true;

                            if (!isMakeCommandSuccess)
                                strError = subParameter[1];
                        }
                    }
                    #endregion

                    #region NOTIFY COMMAND TO CLIENT IF MAKE COMMAND IN MT4 FALSE
                    if (!isMakeCommandSuccess)
                    {
                        if (Command.IsServer)
                        {
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.AddCommandByManager, EnumET5.ET5Message.MT4_CAN_NOT_MAKE_COMMAND, false, -1, 0);

                            //string Message = "AddCommandByManager$False,MT4 CAN'T MAKE COMMAND," + Result + "," + Command.Investor.InvestorID + "," +
                            //   Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            //       Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            //       CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                            //if (Command.Investor.ClientCommandQueue == null)
                            //    Command.Investor.ClientCommandQueue = new List<string>();

                            //Command.Investor.ClientCommandQueue.Add(Message);
                        }
                        else
                        {
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.MT4_CAN_NOT_MAKE_COMMAND, false, -1, 0);

                            //string Message = "AddCommand$False,MT4 CAN'T MAKE COMMAND," + Result + "," + Command.Investor.InvestorID + "," +
                            //    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            //        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            //        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                            //if (Command.Investor.ClientCommandQueue == null)
                            //    Command.Investor.ClientCommandQueue = new List<string>();

                            //Command.Investor.ClientCommandQueue.Add(Message);
                        }

                        return;
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        private void OrderSendNj4x(OpenTrade Command)
        {
            #region GET EXECUTION TYPE
            string executionType = string.Empty;
            //CHECK TRADE TYPE MANUAL OR AUTOMATICK
            executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");
            #endregion

            #region Set Property IsBuy And CommandType Send To Client
            bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

            string CommandType = Model.Helper.Instance.convertCommandTypeIDToString(Command.Type.ID);
            #endregion                    
            
            string CommandCode = string.Empty;

            Command.Comment = "Element5 OrderSend";
            string cmd = string.Empty;
            if (Command.Symbol.ExecutionTrade == EnumMT4.Execution.REQUEST)
            {
                cmd = NJ4XConnectSocket.MapNJ4X.Instance.MapOrderSend(Command.Investor.Code, Command.Symbol.Name, 1,
                                                Command.Size, Command.OpenPrice, 0, Command.StopLoss, Command.TakeProfit, Command.Comment);
            }
            else
            {
                cmd = NJ4XConnectSocket.MapNJ4X.Instance.MapOrderSend(Command.Investor.Code, Command.Symbol.Name, Command.Type.ID,
                                                Command.Size, Command.OpenPrice, 0, Command.StopLoss, Command.TakeProfit, Command.Comment);
            }

            NJ4XConnectSocket.NJ4XTicket newNJ4XTicket = new NJ4XConnectSocket.NJ4XTicket();
            newNJ4XTicket.Ask = 0;
            newNJ4XTicket.Bid = 0;
            newNJ4XTicket.Code = Command.Investor.Code;
            newNJ4XTicket.IsDisable = false;
            newNJ4XTicket.IsReQuote = false;
            newNJ4XTicket.IsUpdate = false;
            newNJ4XTicket.OpenPrice = Command.OpenPrice;
            newNJ4XTicket.Symbol = Command.Symbol.Name;
            newNJ4XTicket.Ticket = -1;
            newNJ4XTicket.Execution = Command.Symbol.ExecutionTrade;
            newNJ4XTicket.IsRequest = false;
            newNJ4XTicket.OpenPrice = Command.OpenPrice;
            newNJ4XTicket.ClosePrices = Command.ClosePrice;
            newNJ4XTicket.Digit = Command.Symbol.Digit;
            newNJ4XTicket.Command = Command;
            newNJ4XTicket.ClientCode = Command.ClientCode;

            Business.Market.NJ4XTickets.Add(newNJ4XTicket);

            string result = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(cmd);

            if (!string.IsNullOrEmpty(result))
            {
                string[] subResult = result.Split('$');
                if (subResult[0] == "OrderSend")
                {
                    int ticket = int.Parse(subResult[1]);
                    if (Model.Helper.Instance.IsMT4ErrorCode(ticket))
                    {
                        if (ticket != 135 && ticket != 138)
                        {
                            lock (Business.Market.nj4xObject)
                                Business.Market.NJ4XTickets.Remove(newNJ4XTicket);

                            this.CheckTicket(ticket, Command);

                            #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);
                            string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);

                            string content = string.Empty;
                            string comment = string.Empty;

                            content = "'" + Command.Investor.Code + "': " + mode + " " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                            " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";
                            comment = "[" + mode + " instance]";
                            string strError = this.GetErrorWithCode(ticket);
                            content += " - failed [" + strError + "]";

                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.IpAddress, Command.Investor.Code);
                            #endregion
                        }
                    }
                    else
                    {
                        if (executionType == "manual- but automatic if no dealer online" ||
                            executionType == "automatic only" ||
                            executionType == "manual only- no automation")
                        {
                            if (Business.Market.NJ4XTickets != null && Business.Market.NJ4XTickets.Count > 0)
                            {
                                lock (Business.Market.nj4xObject)
                                {
                                    //if (Business.Market.NJ4XTickets != null)
                                    //{
                                    //    int countNJ4X = Business.Market.NJ4XTickets.Count;
                                    //    for (int n = 0; n < countNJ4X; n++)
                                    //    {
                                    //        if (Business.Market.NJ4XTickets[n].Code == newNJ4XTicket.Code)
                                    //        {
                                    //            Business.Market.NJ4XTickets.RemoveAt(n);

                                    //            break;
                                    //        }
                                    //    }
                                    //}

                                    #region comment code
                                    int countNJ4X = Business.Market.NJ4XTickets.Count;
                                    for (int n = 0; n < countNJ4X; n++)
                                    {
                                        if (Business.Market.NJ4XTickets[n].Code == newNJ4XTicket.Code)
                                        {
                                            lock (Business.Market.nj4xObject)
                                                Business.Market.NJ4XTickets.RemoveAt(n);

                                            newNJ4XTicket.Ticket = ticket;

                                            string cmdOpenTrade = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetCommandHistoryInfo(ticket);
                                            string cmdResult = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmdOpenTrade);

                                            BuildCommandElement5ConnectMT4.Business.OnlineTrade _newOpenTrade = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertHistoryInfo(cmdResult);

                                            Business.OpenTrade resultOpenTrade = Command;

                                            resultOpenTrade.Commission = _newOpenTrade.Commission;
                                            resultOpenTrade.Swap = _newOpenTrade.Swap;
                                            resultOpenTrade.OpenPrice = _newOpenTrade.OpenPrice;
                                            resultOpenTrade.StopLoss = _newOpenTrade.StopLoss;
                                            resultOpenTrade.TakeProfit = _newOpenTrade.TakeProfit;

                                            resultOpenTrade.RefCommandID = ticket;
                                            resultOpenTrade.CommandCode = ticket.ToString();

                                            #region process add new command
                                            if (resultOpenTrade.RefCommandID > 0)
                                            {
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
                                                    Message.Append(CommandType);
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
                                                    Message.Append(CommandType);
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

                                            break;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            
                        }
                    }
                }
                else
                {
                    lock (Business.Market.nj4xObject)
                        Business.Market.NJ4XTickets.Remove(newNJ4XTicket);
                }
            }
            else
            {
                #region TIME OUT
                Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.TIME_OUT, false, -1, 0);
                
                lock (Business.Market.nj4xObject)
                    Business.Market.NJ4XTickets.Remove(newNJ4XTicket);
                #endregion

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);
                string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);

                string content = string.Empty;
                string comment = string.Empty;

                content = "'" + Command.Investor.Code + "': " + mode + " " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";
                comment = mode;
                content += " failed [time out]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.IpAddress, Command.Investor.Code);
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int MapTradeOperation(int cmd)
        {
            int result = -1;
            switch (cmd)
            {
                case 1:
                    result = 0;
                    break;

                case 2:
                    result = 1;
                    break;

                case 9:
                    result = 4;
                    break;

                case 7:
                    result = 2;
                    break;

                case 10:
                    result = 5;
                    break;

                case 8:
                    result = 3;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="command"></param>
        private void CheckTicket(int ticket, Business.OpenTrade command)
        {
            #region MAP ISBUY AND COMMANDTYPE
            bool IsBuy = Model.Helper.Instance.IsBuy(command.Type.ID);
            string CommandType = Model.Helper.Instance.convertCommandTypeIDToString(command.Type.ID);
            #endregion
           
            switch (ticket)
            {
                case 1:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.NO_RESULT, false, ticket, 0);

                        //string Message = "AddCommand$False,no result.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                           command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 2:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.COMMON_ERROR, false, ticket, 0);

                        //string Message = "AddCommand$False,Common error.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 3:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_TRADE_PARAMETERS, false, ticket, 0);

                        //string Message = "AddCommand$False,Invalid trade parameters.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.TRADE_SERVER_IS_BUSY, false, ticket, 0);

                        //string Message = "AddCommand$False,Trade server is busy.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 5:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.OLD_VERSION_OF_THE_CLIENT_TERMINAL, false, ticket, 0);

                        //string Message = "AddCommand$False,Old version of the client terminal.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                  command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                  command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                  command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                  command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                  command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                  command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 6:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.NO_CONNECTION_WITH_TRADE_SERVER, false, ticket, 0);

                        //string Message = "AddCommand$False,No connection with trade server.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                          command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                          command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                          command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                          command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                          command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                          command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 8:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.TOO_FREQUENT_REQUESTS, false, ticket, 0);

                        //string Message = "AddCommand$False,Too frequent requests.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                          command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                          command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                          command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                          command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                          command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                          command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 64:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.ACCOUNT_DISABLED, false, ticket, 0);

                        //string Message = "AddCommand$False,Account disabled.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                          command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                          command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                          command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                          command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                          command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                          command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 65:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_ACCOUNT, false, ticket, 0);

                        //string Message = "AddCommand$False,Invalid account.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                         command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                         command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                         command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                         command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                         command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                         command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 128:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.TRADE_TIMEOUT, false, ticket, 0);

                        //string Message = "AddCommand$False,Trade timeout.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                         command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                         command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                         command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                         command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                         command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                         command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 129:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_PRICE, false, ticket, 0);

                        //string Message = "AddCommand$False,Invalid price.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                        command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                        command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                        command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                        command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                        command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                        command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 131:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_TRADE_VOLUME, false, ticket, 0);

                        //string Message = "AddCommand$False,Invalid trade volume.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                       command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                       command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                       command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                       command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                       command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                       command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 134:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.NOT_ENOUGH_MONEY, false, ticket, 0);

                        //string Message = "AddCommand$False,Not enough money.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                       command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                       command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                       command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                       command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                       command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                       command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 136:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.OFF_QUOTES, false, ticket, 0);

                        //string Message = "AddCommand$False,Off quotes.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                                       command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                                       command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                                       command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                                       command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                                       command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                                       command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4051:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_FUNCTION_PARAMETER_VALUE, false, ticket, 0);

                        //string Message = "AddCommand$False,Invalid function parameter value.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," + 
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4055:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.CUSTOM_INDICATOR_ERROR, false, ticket, 0);

                        //string Message = "AddCommand$False,Custom indicator error.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4062:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.STRING_PARAMETER_EXPECTED, false, ticket, 0);

                        //string Message = "AddCommand$False,String parameter expected.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4063:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INTEGER_PARAMETER_EXPECTED, false, ticket, 0);

                        //string Message = "AddCommand$False,Integer parameter expected.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4106:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.UNKNOWN_SYMBOL, false, ticket, 0);

                        //string Message = "AddCommand$False,Unknown symbol.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4107:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_PRICE, false, ticket, 0);

                        //string Message = "AddCommand$False,Invalid price.," + ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4109:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.TRADE_IS_NOT_ALLOWED_ENABLE_CHECKBOX_ALLOW_LIVE_TRADING_IN_THE_EXPERT_PROPERTIES, false, ticket, 0);

                        //string Message = "AddCommand$False,Trade is not allowed. Enable checkbox \"Allow live trading\" in the expert properties.," + 
                        //                    ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4110:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.LONGS_ARE_NOT_ALLOWED_CHECK_THE_EXPERT_PROPERTIES, false, ticket, 0);

                        //string Message = "AddCommand$False,Longs are not allowed. Check the expert properties.," +
                        //                    ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 4111:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.SHORTS_ARE_NOT_ALLOWED_CHECK_THE_EXPERT_PROPERTIES, false, ticket, 0);

                        //string Message = "AddCommand$False,Shorts are not allowed. Check the expert properties.," +
                        //                    ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 130:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.INVALID_STOPS, false, ticket, 0);

                        //string Message = "AddCommand$False,invalid stops.," +
                        //                    ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 133:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.TRADE_IS_DISABLED, false, ticket, 0);

                        //string Message = "AddCommand$False,trade is disabled.," +
                        //                    ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;

                case 9999:
                    {
                        Model.Helper.Instance.SendCommandToClient(command, EnumET5.CommandName.AddCommand, EnumET5.ET5Message.UNKNOWN_ERROR, false, ticket, 0);

                        //string Message = "AddCommand$False,unknown error.," +
                        //                    ticket + "," + command.Investor.InvestorID + "," +
                        //                   command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                        //                   command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                        //                   command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                        //                   command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                        //                   command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                        //                   command.Type.ID + "," + command.Margin + ",Open";

                        //if (command.Investor.ClientCommandQueue == null)
                        //    command.Investor.ClientCommandQueue = new List<string>();

                        //command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tikcet"></param>
        /// <param name="command"></param>
        private void CheckTicketClose(int ticket, Business.OpenTrade command)
        {
            #region MAP ISBUY AND COMMANDTYPE
            bool IsBuy = Model.Helper.Instance.IsBuy(command.Type.ID);

            string CommandType = Model.Helper.Instance.convertCommandTypeIDToString(command.Type.ID);
            #endregion

            switch (ticket)
            {
                #region COMMON ERROR
                case 2:
                    {
                        string Message = "CloseCommand$False,Common error.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region INVALID TRADE PARAMETERS
                case 3:
                    {
                        string Message = "CloseCommand$False,Invalid trade parameters.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region TRADE SERVER IS BUSY
                case 4:
                    {
                        string Message = "CloseCommand$False,Trade server is busy.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region OLD VERSION OF THE CLIENT TERMINAL
                case 5:
                    {
                        string Message = "CloseCommand$False,Old version of the client terminal.," + ticket + "," + command.Investor.InvestorID + "," +
                                          command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                          command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                          command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                          command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                          command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                          command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region NO CONNECTION WITH TRADE SERVER
                case 6:
                    {
                        string Message = "CloseCommand$False,No connection with trade server.," + ticket + "," + command.Investor.InvestorID + "," +
                                                                  command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                                  command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                                  command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                                  command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                                  command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                                  command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region TOO FREQUENT REQUEST
                case 8:
                    {
                        string Message = "CloseCommand$False,Too frequent requests.," + ticket + "," + command.Investor.InvestorID + "," +
                                                                  command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                                  command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                                  command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                                  command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                                  command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                                  command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region ACCOUNT DISABLED
                case 64:
                    {
                        string Message = "CloseCommand$False,Account disabled.," + ticket + "," + command.Investor.InvestorID + "," +
                                                                  command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                                  command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                                  command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                                  command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                                  command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                                  command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region INVALID ACCOUNT
                case 65:
                    {
                        string Message = "CloseCommand$False,Invalid account.," + ticket + "," + command.Investor.InvestorID + "," +
                                                                 command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                                 command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                                 command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                                 command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                                 command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                                 command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region TRADE TIMEOUT
                case 128:
                    {
                        string Message = "CloseCommand$False,Trade timeout.," + ticket + "," + command.Investor.InvestorID + "," +
                                                                 command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                                 command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                                 command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                                 command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                                 command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                                 command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region INVALID PRICE
                case 129:
                    {
                        string Message = "CloseCommand$False,Invalid price.," + ticket + "," + command.Investor.InvestorID + "," +
                                                                command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                                command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                                command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                                command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                                command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                                command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region NOT ENOUGH MONEY
                case 134:
                    {
                        string Message = "CloseCommand$False,Not enough money.," + ticket + "," + command.Investor.InvestorID + "," +
                                                               command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                               command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                               command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                               command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                               command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                               command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region OFF QUOTES
                case 136:
                    {
                        string Message = "CloseCommand$False,Off quotes.," + ticket + "," + command.Investor.InvestorID + "," +
                                                               command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                                               command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                                               command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                                               command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                                               command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                                               command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region INVALID FUNCTION PARAMETER VALUE
                case 4051:
                    {
                        string Message = "CloseCommand$False,Invalid function parameter value.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region CUSTOM INDICATOR ERROR
                case 4055:
                    {
                        string Message = "CloseCommand$False,Custom indicator error.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region STRING PARAMETER EXPECTED
                case 4062:
                    {
                        string Message = "CloseCommand$False,String parameter expected.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region INTEGER PARAMETER EXPECTED
                case 4063:
                    {
                        string Message = "CloseCommand$False,Integer parameter expected.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region UNKNOWN SYMBOLE
                case 4106:
                    {
                        string Message = "CloseCommand$False,Unknown symbol.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region INVALID PRICE
                case 4107:
                    {
                        string Message = "CloseCommand$False,Invalid price.," + ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region TRADE IS NOT ALLOWED, ENABLE CHECKBOX ALLOW LIVE TRADING IN THE EXPERT PROPERTIES
                case 4109:
                    {
                        string Message = "CloseCommand$False,Trade is not allowed. Enable checkbox \"Allow live trading\" in the expert properties.," +
                                            ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break;
                #endregion

                #region LONGS ARE NOT ALLOWED. CHECK THE EXPERT PROPERTIES
                case 4110:
                    {
                        string Message = "CloseCommand$False,Longs are not allowed. Check the expert properties.," +
                                            ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region SHORTS ARE NOT ALLOWED, CHECK THE EXPERT PROPERTIES
                case 4111:
                    {
                        string Message = "CloseCommand$False,Shorts are not allowed. Check the expert properties.," +
                                            ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);
                    }
                    break; 
                #endregion

                #region UNKNOWN ERROR
                case 9999:
                    {
                        string Message = "CloseCommand$False,unknown error," +
                                            ticket + "," + command.Investor.InvestorID + "," +
                                           command.Symbol.Name + "," + command.Size + "," + IsBuy + "," + command.OpenTime + "," +
                                           command.OpenPrice + "," + command.StopLoss + "," + command.TakeProfit + "," +
                                           command.ClosePrice + "," + command.Commission + "," + command.Swap + "," +
                                           command.Profit + "," + "Comment," + command.ID + "," + CommandType + "," + 1 + "," +
                                           command.ExpTime + "," + command.ClientCode + "," + -1 + "," + command.IsHedged + "," +
                                           command.Type.ID + "," + command.Margin + ",Open";

                        if (command.Investor.ClientCommandQueue == null)
                            command.Investor.ClientCommandQueue = new List<string>();

                        command.Investor.ClientCommandQueue.Add(Message);

                    }
                    break;
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private string GetErrorWithCode(int ticket)
        {
            string result = string.Empty;

            switch (ticket)
            {
                #region NO RESULT
                case 1:
                    result = "no result.";
                    break;
                #endregion

                #region COMMON ERROR
                case 2:
                    result = "Common error.";
                    break;
                #endregion

                #region INVALID TRADE PARAMETERS
                case 3:
                    result = "Invalid trade parameters.";
                    break;
                #endregion

                #region TRADE SERVER IS BUSY
                case 4:
                    result = "Trade server is busy.";
                    break;
                #endregion

                #region OLD VERSION OF THE CLIENT TERMINAL
                case 5:
                    result = "Old version of the client terminal.";
                    break;
                #endregion

                #region NO CONNECTION WITH TRADE SERVER
                case 6:
                    result = "No connection with trade server.";
                    break;
                #endregion

                #region TOO FREQUENT REQUEST
                case 8:
                    result = "Too frequent requests.";
                    break;
                #endregion

                #region ACCOUNT DISABLED
                case 64:
                    result = "Account disabled.";
                    break;
                #endregion

                #region INVALID ACCOUNT
                case 65:
                    result = "Invalid account.";
                    break;
                #endregion

                #region TRADE TIMEOUT
                case 128:
                    result = "Trade timeout.";
                    break;
                #endregion

                #region INVALID PRICE
                case 129:
                    result = "Invalid price.";
                    break;
                #endregion

                #region INVALID TRADE VOLUME
                case 131:
                    result = "Invalid trade volume.";
                    break;
                #endregion

                #region TRADE IS DISABLED
                case 133:
                    result = "trade is disabled.";
                    break;
                #endregion

                #region NOT ENOUGH MONEY
                case 134:
                    result = "Not enough money.";
                    break;
                #endregion

                #region OFF QUOTES
                case 136:
                    result = "Off quotes.";
                    break;
                #endregion

                #region INVALID FUNCTION PARAMETER VALUE
                case 4051:
                    result = "Invalid function parameter value.";
                    break;
                #endregion

                #region CUSTOM INDICATOR ERROR
                case 4055:
                    result = "Custom indicator error.";
                    break;
                #endregion

                #region STRING PARAMETER EXPECTED
                case 4062:
                    result = "String parameter expected.";
                    break;
                #endregion

                #region INTEGER PARAMETER EXPECTED
                case 4063:
                    result = "Integer parameter expected.";
                    break;
                #endregion

                #region UNKNOWN SYMBOLE
                case 4106:
                    result = "Unknown symbol.";
                    break;
                #endregion

                #region INVALID PRICE
                case 4107:
                    result = "Invalid price.";
                    break;
                #endregion

                #region INVALID TICKET 
                case 4108:
                    result = "Invalid ticket.";
                    break;
                #endregion

                #region TRADE IS NOT ALLOWED, ENABLE CHECKBOX ALLOW LIVE TRADING IN THE EXPERT PROPERTIES
                case 4109:
                    result = "Trade is not allowed. Enable checkbox \"Allow live trading\" in the expert properties.";
                    break;
                #endregion

                #region LONGS ARE NOT ALLOWED. CHECK THE EXPERT PROPERTIES
                case 4110:
                    result = "Longs are not allowed. Check the expert properties.";
                    break;
                #endregion

                #region SHORTS ARE NOT ALLOWED, CHECK THE EXPERT PROPERTIES
                case 4111:
                    result = "Shorts are not allowed. Check the expert properties.";
                    break;
                #endregion

                #region INVALID STOPLOSS
                case 130:
                    result = "Invalid stops.";
                    break;
                #endregion

                #region UNKNOWN ERROR
                case 9999:
                    result = "unknown error.";
                    break;
                #endregion
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private EnumET5.ET5Message GetEnumWithTicket(int ticket)
        {
            EnumET5.ET5Message result = new EnumET5.ET5Message();

            switch (ticket)
            {
                #region NO RESULT
                case 1:
                    result = EnumET5.ET5Message.NO_RESULT;
                    break;
                #endregion

                #region COMMON ERROR
                case 2:
                    result = EnumET5.ET5Message.COMMON_ERROR;
                    break;
                #endregion

                #region INVALID TRADE PARAMETERS
                case 3:
                    result = EnumET5.ET5Message.INVALID_TRADE_PARAMETERS;
                    break;
                #endregion

                #region TRADE SERVER IS BUSY
                case 4:
                    result = EnumET5.ET5Message.TRADE_SERVER_IS_BUSY;
                    break;
                #endregion

                #region OLD VERSION OF THE CLIENT TERMINAL
                case 5:
                    result = EnumET5.ET5Message.OLD_VERSION_OF_THE_CLIENT_TERMINAL;
                    break;
                #endregion

                #region NO CONNECTION WITH TRADE SERVER
                case 6:
                    result = EnumET5.ET5Message.NO_CONNECTION_WITH_TRADE_SERVER;
                    break;
                #endregion

                #region TOO FREQUENT REQUEST
                case 8:
                    result = EnumET5.ET5Message.TOO_FREQUENT_REQUESTS;
                    break;
                #endregion

                #region ACCOUNT DISABLED
                case 64:
                    result = EnumET5.ET5Message.ACCOUNT_DISABLED;
                    break;
                #endregion

                #region INVALID ACCOUNT
                case 65:
                    result = EnumET5.ET5Message.INVALID_ACCOUNT;
                    break;
                #endregion

                #region TRADE TIMEOUT
                case 128:
                    result = EnumET5.ET5Message.TRADE_TIMEOUT;
                    break;
                #endregion

                #region INVALID PRICE
                case 129:
                    result = EnumET5.ET5Message.INVALID_PRICE;
                    break;
                #endregion

                #region INVALID TRADE VOLUME
                case 131:
                    result = EnumET5.ET5Message.INVALID_TRADE_VOLUME;
                    break;
                #endregion

                #region TRADE IS DISABLED
                case 133:
                    result = EnumET5.ET5Message.TRADE_IS_DISABLED;
                    break;
                #endregion

                #region NOT ENOUGH MONEY
                case 134:
                    result = EnumET5.ET5Message.NOT_ENOUGH_MONEY;
                    break;
                #endregion

                #region OFF QUOTES
                case 136:
                    result = EnumET5.ET5Message.OFF_QUOTES;
                    break;
                #endregion

                #region INVALID FUNCTION PARAMETER VALUE
                case 4051:
                    result = EnumET5.ET5Message.INVALID_FUNCTION_PARAMETER_VALUE;
                    break;
                #endregion

                #region CUSTOM INDICATOR ERROR
                case 4055:
                    result = EnumET5.ET5Message.CUSTOM_INDICATOR_ERROR;
                    break;
                #endregion

                #region STRING PARAMETER EXPECTED
                case 4062:
                    result = EnumET5.ET5Message.STRING_PARAMETER_EXPECTED;
                    break;
                #endregion

                #region INTEGER PARAMETER EXPECTED
                case 4063:
                    result = EnumET5.ET5Message.INTEGER_PARAMETER_EXPECTED;
                    break;
                #endregion

                #region UNKNOWN SYMBOLE
                case 4106:
                    result = EnumET5.ET5Message.UNKNOWN_SYMBOL;
                    break;
                #endregion

                #region INVALID PRICE
                case 4107:
                    result = EnumET5.ET5Message.INVALID_PRICE;
                    break;
                #endregion

                #region INVALID TICKET
                case 4108:
                    result = EnumET5.ET5Message.INVALID_TICKET;
                    break;
                #endregion

                #region TRADE IS NOT ALLOWED, ENABLE CHECKBOX ALLOW LIVE TRADING IN THE EXPERT PROPERTIES
                case 4109:
                    result = EnumET5.ET5Message.TRADE_IS_NOT_ALLOWED_ENABLE_CHECKBOX_ALLOW_LIVE_TRADING_IN_THE_EXPERT_PROPERTIES;
                    break;
                #endregion

                #region LONGS ARE NOT ALLOWED. CHECK THE EXPERT PROPERTIES
                case 4110:
                    result = EnumET5.ET5Message.LONGS_ARE_NOT_ALLOWED_CHECK_THE_EXPERT_PROPERTIES;
                    break;
                #endregion

                #region SHORTS ARE NOT ALLOWED, CHECK THE EXPERT PROPERTIES
                case 4111:
                    result = EnumET5.ET5Message.SHORTS_ARE_NOT_ALLOWED_CHECK_THE_EXPERT_PROPERTIES;
                    break;
                #endregion

                #region INVALID STOPLOSS
                case 130:
                    result = EnumET5.ET5Message.INVALID_STOPS;
                    break;
                #endregion

                #region UNKNOWN ERROR
                case 9999:
                    result = EnumET5.ET5Message.UNKNOWN_ERROR;
                    break;
                #endregion
            }

            return result;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void CloseCommand(Business.OpenTrade Command)
        {
            //Check Condition Close Command
            Business.OpenTrade newOpenTrade = new OpenTrade();            

            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.CloseTime = DateTime.Now;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.IsClose = true;
            newOpenTrade.IsHedged = Command.IsHedged;
            newOpenTrade.Margin = Command.Margin;            
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.OpenTime = Command.OpenTime;
            newOpenTrade.Profit = Command.Profit;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.Swap = Command.Swap;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Taxes = Command.Taxes;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.TotalSwap = Command.TotalSwap;
            newOpenTrade.IsServer = Command.IsServer;
            newOpenTrade.AgentRefConfig = Command.AgentRefConfig;

            if (string.IsNullOrEmpty(Command.Comment))
            {
                newOpenTrade.Comment = "[close spot command]";
            }
            else
            {
                newOpenTrade.Comment = Command.Comment;
            }

            newOpenTrade.AgentCommission = Command.AgentCommission;

            //Call Function Calculator Profit Command Close            
            //newOpenTrade.CalculatorProfitCommand(newOpenTrade);            
            //newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

            #region CONNECT MT4 AND CLOSE COMMAND
            if (Business.Market.IsConnectMT4)
            {
                if (!Business.Market.StatusConnect)
                {
                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommandByManager, EnumET5.ET5Message.DISCONNECT_FROM_MT4, false, Command.ID, 1);
                    return;
                }

                #region CHECK MANUAL DEALERS OR AUTOMATIC
                string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");
                #endregion

                if (executionType == "manual only- no automation" ||
                    executionType == "manual- but automatic if no dealer online" ||
                    executionType == "automatic only")
                {
                    #region CONNECT MT4 USING NJ4X
                    switch (Command.Symbol.ExecutionTrade)
                    {
                        case EnumMT4.Execution.MARKET:
                            {
                                this.OrderCloseNj4x(Command);
                            }
                            break;

                        case EnumMT4.Execution.INSTANT:
                            {
                                this.OrderCloseNj4x(Command);
                            }
                            break;

                        case EnumMT4.Execution.REQUEST:
                            {
                                bool isFlag = false;
                                if (Business.Market.NJ4XTickets != null)
                                {
                                    int count = Business.Market.NJ4XTickets.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.NJ4XTickets[i].IsRequest == true &&
                                            Business.Market.NJ4XTickets[i].IsDisable == true &&
                                            Business.Market.NJ4XTickets[i].Code == Command.Investor.Code)
                                        {
                                            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
                                            string cmd = string.Empty;
                                            if (isPending)
                                            {
                                                string typeMT4 = Model.Helper.Instance.ConvertTypeToMT4Type(Command.Type.ID).ToString();

                                                cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertDeletePendingOrderToString(Command.RefCommandID, typeMT4);
                                            }
                                            else
                                                cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertCloseCommandToString(Command.RefCommandID, newOpenTrade.ClosePrice, Command.Size);

                                            string resultClose = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                                            bool isCloseSuccess = false;
                                            string strError = string.Empty;

                                            if (!string.IsNullOrEmpty(resultClose))
                                            {
                                                string[] subValue = resultClose.Split('$');
                                                if (subValue.Length == 2)
                                                {
                                                    string[] subParameter = subValue[1].Split('{');
                                                    if (int.Parse(subParameter[0]) == 1)
                                                        isCloseSuccess = true;
                                                    else
                                                        isCloseSuccess = false;

                                                    if (!isCloseSuccess)
                                                        strError = subParameter[1];
                                                }

                                                if (!isCloseSuccess)
                                                {
                                                    #region Map Command Server To Client
                                                    if (Command.IsServer)
                                                    {
                                                        Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommandByManager, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);
                                                    }
                                                    else
                                                    {
                                                        Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                #region Map Command Server To Client
                                                if (Command.IsServer)
                                                {
                                                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommandByManager, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);
                                                }
                                                else
                                                {
                                                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);
                                                }
                                                #endregion
                                            }

                                            isFlag = true;

                                            lock (Business.Market.nj4xObject)
                                                Business.Market.NJ4XTickets.RemoveAt(i);

                                            break;
                                        }
                                    }
                                }

                                if (!isFlag)
                                {
                                    Command.ClosePrice = 0;
                                    //make command using nj4x
                                    this.OrderCloseNj4x(Command);
                                }
                            }
                            break;
                    }
                    #endregion
                }
                else
                {
                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
                    string cmd = string.Empty;
                    if (isPending)
                    {
                        string typeMT4 = Model.Helper.Instance.ConvertTypeToMT4Type(Command.Type.ID).ToString();

                        cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertDeletePendingOrderToString(Command.RefCommandID, typeMT4);
                    }
                    else
                        cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertCloseCommandToString(Command.RefCommandID, newOpenTrade.ClosePrice, Command.Size);

                    string resultClose = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                    bool isCloseSuccess = false;
                    string strError = string.Empty;

                    if (!string.IsNullOrEmpty(resultClose))
                    {
                        string[] subValue = resultClose.Split('$');
                        if (subValue.Length == 2)
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (int.Parse(subParameter[0]) == 1)
                                isCloseSuccess = true;
                            else
                                isCloseSuccess = false;

                            if (!isCloseSuccess)
                                strError = subParameter[1];
                        }

                        if (!isCloseSuccess)
                        {
                            #region Map Command Server To Client
                            if (Command.IsServer)
                            {
                                Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommandByManager, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);

                                //string Message = "CloseCommandByManager$False,CAN'T CLOSE COMMAND FROM MT4," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                //    Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                //    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                //    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                //    ",Close," + Command.CloseTime;

                                //if (Command.Investor.ClientCommandQueue == null)
                                //    Command.Investor.ClientCommandQueue = new List<string>();

                                //Command.Investor.ClientCommandQueue.Add(Message);
                            }
                            else
                            {
                                Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);

                                //string Message = "CloseCommand$False,CAN'T CLOSE COMMAND FROM MT4," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                //    Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                //    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                //    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                //    ",Close," + Command.CloseTime;

                                //if (Command.Investor.ClientCommandQueue == null)
                                //    Command.Investor.ClientCommandQueue = new List<string>();

                                //Command.Investor.ClientCommandQueue.Add(Message);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region Map Command Server To Client
                        if (Command.IsServer)
                        {
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommandByManager, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);

                            //string Message = "CloseCommandByManager$False,CAN'T CLOSE COMMAND FROM MT4," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            //    Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            //    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            //    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                            //    ",Close," + Command.CloseTime;

                            //if (Command.Investor.ClientCommandQueue == null)
                            //    Command.Investor.ClientCommandQueue = new List<string>();

                            //Command.Investor.ClientCommandQueue.Add(Message);
                        }
                        else
                        {
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, EnumET5.ET5Message.CAN_NOT_CLOSE_COMMAND_FROM_MT4, false, Command.ID, 1);

                            //string Message = "CloseCommand$False,CAN'T CLOSE COMMAND FROM MT4," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            //    Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            //    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            //    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                            //    ",Close," + Command.CloseTime;

                            //if (Command.Investor.ClientCommandQueue == null)
                            //    Command.Investor.ClientCommandQueue = new List<string>();

                            //Command.Investor.ClientCommandQueue.Add(Message);
                        }
                        #endregion
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="executionType"></param>
        private void OrderCloseNj4x(Business.OpenTrade Command)
        {
            #region CHECK MANUAL DEALERS OR AUTOMATIC
            string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");
            #endregion

            //CONNECT TO NJ4X
            bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
            string nj4xOrderClose = string.Empty;

            if (isPending)
                nj4xOrderClose = NJ4XConnectSocket.MapNJ4X.Instance.MapOrderDelete(Command.ID, Command.Investor.Code, Command.Investor.UnZipPwd);
            else
            {
                if (Command.Symbol.ExecutionTrade == EnumMT4.Execution.REQUEST)
                {
                    nj4xOrderClose = NJ4XConnectSocket.MapNJ4X.Instance.MapOrderClose(Command.ID, Command.Size, Command.OpenPrice, Command.Investor.Code,
                                                                                    Command.Symbol.Name);
                }
                else
                {
                    nj4xOrderClose = NJ4XConnectSocket.MapNJ4X.Instance.MapOrderClose(Command.ID, Command.Size, Command.ClosePrice, Command.Investor.Code,
                                                                                    Command.Symbol.Name);
                }
            }   

            NJ4XConnectSocket.NJ4XTicket newNJ4XTicket = new NJ4XConnectSocket.NJ4XTicket();
            newNJ4XTicket.Ticket = Command.ID;
            newNJ4XTicket.IsClose = true;
            newNJ4XTicket.Ask = 0;
            newNJ4XTicket.Bid = 0;
            newNJ4XTicket.Code = Command.Investor.Code;
            newNJ4XTicket.IsDisable = false;
            newNJ4XTicket.IsReQuote = false;
            newNJ4XTicket.IsRequest = false;
            newNJ4XTicket.IsUpdate = false;
            newNJ4XTicket.OpenPrice = Command.OpenPrice;
            newNJ4XTicket.Symbol = Command.Symbol.Name;
            newNJ4XTicket.Execution = Command.Symbol.ExecutionTrade;
            newNJ4XTicket.Command = Command;

            Business.Market.NJ4XTickets.Add(newNJ4XTicket);

            string nj4xOrderCloseResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xOrderClose);

            if (!string.IsNullOrEmpty(nj4xOrderCloseResult))
            {
                #region ORDER CLOSE
                string[] subnj4xResult = nj4xOrderCloseResult.Split('$');
                if (subnj4xResult[0] == "OrderClose")
                {
                    string[] subnj4x = subnj4xResult[1].Split('{');
                    bool _isClose = bool.Parse(subnj4x[0]);
                    int ticket = int.Parse(subnj4x[1]);
                    if (ticket == 138 || ticket == 4051 || ticket == 4055 || ticket == 4062 || ticket == 4063 ||
                        ticket == 4106 || ticket == 4107 || ticket == 4109 || ticket == 4110 || ticket == 4111 ||
                        ticket == 2 || ticket == 3 || ticket == 4 || ticket == 5 || ticket == 6 || ticket == 8 ||
                        ticket == 64 || ticket == 65 || ticket == 128 || ticket == 129 || ticket == 134 || ticket == 136 ||
                        ticket == 135 || ticket == 2004 || ticket == 9999 || ticket == 130 || ticket == 131 || ticket == 4108 ||
                        ticket == 133)
                    {
                        if (ticket != 135 && ticket != 138)
                        {
                            EnumET5.ET5Message eMessage = this.GetEnumWithTicket(ticket);
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, eMessage, false, Command.ID, 1);

                            string strError = this.GetErrorWithCode(ticket);
                            //if (!string.IsNullOrEmpty(strError))
                            //{
                            //    string Message = "CloseCommand$False," + strError + "," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            //       Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            //       Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            //       1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                            //       ",Close," + Command.CloseTime;

                            //    if (Command.Investor.ClientCommandQueue == null)
                            //        Command.Investor.ClientCommandQueue = new List<string>();

                            //    Command.Investor.ClientCommandQueue.Add(Message);
                            //}

                            lock (Business.Market.nj4xObject)
                                Business.Market.NJ4XTickets.Remove(newNJ4XTicket);

                            #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                            string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);
                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
                            string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), Command.Symbol.Digit);
                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                            string contentServer = "'" + Command.Investor.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " [Failed - " + Model.Helper.Instance.GetMessage(eMessage) + "]"; 

                            TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "[Close position]", Command.Investor.IpAddress, Command.Investor.Code);
                            #endregion
                        }
                    }
                    else
                    {
                        if (executionType == "manual- but automatic if no dealer online" ||
                            executionType == "automatic only" ||
                            executionType == "manual only- no automation")
                        {
                            if (Business.Market.NJ4XTickets != null)
                            {
                                lock (Business.Market.nj4xObject)
                                {
                                    //if (Business.Market.NJ4XTickets != null)
                                    //{
                                    //    int countNJ4x = Business.Market.NJ4XTickets.Count;
                                    //    for (int n = 0; n < countNJ4x; n++)
                                    //    {
                                    //        if (Business.Market.NJ4XTickets[n].Code == newNJ4XTicket.Code)
                                    //        {
                                    //            Business.Market.NJ4XTickets.RemoveAt(n);
                                    //            break;
                                    //        }
                                    //    }
                                    //}

                                    int countNJ4X = Business.Market.NJ4XTickets.Count;
                                    for (int m = 0; m < countNJ4X; m++)
                                    {
                                        if (Business.Market.NJ4XTickets[m].Ticket == Command.RefCommandID)
                                        {
                                            lock (Business.Market.nj4xObject)
                                                Business.Market.NJ4XTickets.RemoveAt(m);

                                            List<Business.OpenTrade> commands = Command.Investor.CommandList;
                                            bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

                                            string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetCommandHistoryInfo(Command.RefCommandID);
                                            string cmdResult = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                                            BuildCommandElement5ConnectMT4.Business.OnlineTrade listHistory = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertHistoryInfo(cmdResult);

                                            #region convert online trade to open trade
                                            Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                                            if (listHistory != null && listHistory.CommandID > 0)
                                            {
                                                //newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                                                newOpenTrade.ClosePrice = listHistory.ClosePrice;
                                                newOpenTrade.CloseTime = listHistory.CloseTime;
                                                newOpenTrade.CommandCode = listHistory.CommandCode;
                                                newOpenTrade.Commission = listHistory.Commission;
                                                newOpenTrade.ExpTime = listHistory.TimeExpire;
                                                newOpenTrade.ID = listHistory.CommandID;
                                                //Investor

                                                #region FILL COMMAND TYPE
                                                switch (listHistory.CommandType)
                                                {
                                                    case "0":
                                                        {
                                                            Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(1);
                                                            newOpenTrade.Type = resultType;
                                                        }
                                                        break;

                                                    case "1":
                                                        {
                                                            Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(2);
                                                            newOpenTrade.Type = resultType;
                                                        }
                                                        break;

                                                    case "2":
                                                        {
                                                            Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(7);
                                                            newOpenTrade.Type = resultType;
                                                        }
                                                        break;

                                                    case "3":
                                                        {
                                                            Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(8);
                                                            newOpenTrade.Type = resultType;
                                                        }
                                                        break;

                                                    case "4":
                                                        {
                                                            Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(9);
                                                            newOpenTrade.Type = resultType;
                                                        }
                                                        break;

                                                    case "5":
                                                        {
                                                            Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(10);
                                                            newOpenTrade.Type = resultType;
                                                        }
                                                        break;

                                                    case "6":
                                                        {
                                                            if (listHistory.Profit >= 0)
                                                            {
                                                                Business.TradeType resultType = new Business.TradeType();
                                                                resultType.ID = 13;
                                                                resultType.Name = "Deposit";
                                                                newOpenTrade.Type = resultType;
                                                            }
                                                            else
                                                            {
                                                                Business.TradeType resultType = new Business.TradeType();
                                                                resultType.ID = 14;

                                                                resultType.Name = "Withdraw";
                                                                newOpenTrade.Type = resultType;
                                                            }
                                                        }
                                                        break;

                                                    case "7":
                                                        {
                                                            if (listHistory.Profit >= 0)
                                                            {
                                                                Business.TradeType resultType = new Business.TradeType();
                                                                resultType.ID = 15;
                                                                resultType.Name = "CreditIn";
                                                                newOpenTrade.Type = resultType;
                                                            }
                                                            else
                                                            {
                                                                Business.TradeType resultType = new Business.TradeType();
                                                                resultType.ID = 16;
                                                                resultType.Name = "CreditOut";
                                                                newOpenTrade.Type = resultType;
                                                            }
                                                        }
                                                        break;
                                                }
                                                #endregion

                                                #region Find Investor In Investor List
                                                if (Business.Market.InvestorList != null)
                                                {
                                                    int countInvestor = Business.Market.InvestorList.Count;
                                                    for (int k = 0; k < countInvestor; k++)
                                                    {
                                                        if (Business.Market.InvestorList[k].Code.ToUpper().Trim() == listHistory.InvestorCode)
                                                        {
                                                            newOpenTrade.Investor = Business.Market.InvestorList[k];
                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                newOpenTrade.OpenPrice = listHistory.OpenPrice;
                                                newOpenTrade.OpenTime = listHistory.OpenTime;

                                                if (newOpenTrade.Type.ID == 13 || newOpenTrade.Type.ID == 14 ||
                                                    newOpenTrade.Type.ID == 15 || newOpenTrade.Type.ID == 16)
                                                {
                                                    newOpenTrade.CloseTime = newOpenTrade.OpenTime;
                                                }

                                                newOpenTrade.Profit = listHistory.Profit;
                                                newOpenTrade.Size = listHistory.Size;
                                                newOpenTrade.StopLoss = listHistory.StopLoss;
                                                newOpenTrade.Swap = listHistory.Swap;
                                                //newOpenTrade.Taxes = listHistory[j].Taxes;
                                                newOpenTrade.Comment = listHistory.Comment;
                                                //newOpenTrade.AgentCommission = listHistory[j].AgentCommission;
                                                newOpenTrade.TakeProfit = listHistory.TakeProfit;

                                                #region Find Symbol In Symbol List
                                                if (Business.Market.SymbolList != null)
                                                {
                                                    bool Flag = false;
                                                    int countSymbol = Business.Market.SymbolList.Count;
                                                    for (int k = 0; k < countSymbol; k++)
                                                    {
                                                        if (Flag == true)
                                                            break;

                                                        if (Business.Market.SymbolList[k].Name.ToUpper().Trim() == listHistory.SymbolName.ToUpper().Trim())
                                                        {
                                                            newOpenTrade.Symbol = Business.Market.SymbolList[k];

                                                            Flag = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion

                                            Command = newOpenTrade;

                                            #region For Command List
                                            for (int j = 0; j < commands.Count; j++)
                                            {
                                                if (commands[j].ID == Command.ID)
                                                {
                                                    int commandId = Command.ID;
                                                    Command.CommandCode = commands[j].CommandCode;

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

                                                    break;
                                                }
                                            }
                                            #endregion

                                            break;
                                        }
                                    }
                                        
                                }
                            }
                            
                        }
                    }
                }
                #endregion

                #region ORDER DELETE
                if (subnj4xResult[0] == "OrderDelete")
                {
                    string[] subnj4x = subnj4xResult[1].Split('{');
                    bool _isClose = bool.Parse(subnj4x[0]);
                    int ticket = int.Parse(subnj4x[1]);

                    if (ticket == 138 || ticket == 4051 || ticket == 4055 || ticket == 4062 || ticket == 4063 ||
                        ticket == 4106 || ticket == 4107 || ticket == 4109 || ticket == 4110 || ticket == 4111 ||
                        ticket == 2 || ticket == 3 || ticket == 4 || ticket == 5 || ticket == 6 || ticket == 8 ||
                        ticket == 64 || ticket == 65 || ticket == 128 || ticket == 129 || ticket == 134 || ticket == 136 ||
                        ticket == 135 || ticket == 2004 || ticket == 9999 || ticket == 130 || ticket == 4108 ||
                        ticket == 133)
                    {
                        if (ticket != 135 && ticket != 138)
                        {
                            EnumET5.ET5Message eMessage = this.GetEnumWithTicket(ticket);
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, eMessage, false, Command.ID, 1);

                            //string strError = this.GetErrorWithCode(ticket);
                            //if (!string.IsNullOrEmpty(strError))
                            //{
                            //    string Message = "CloseCommand$False," + strError + "," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            //       Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            //       Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            //       1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                            //       ",Close," + Command.CloseTime;

                            //    if (Command.Investor.ClientCommandQueue == null)
                            //        Command.Investor.ClientCommandQueue = new List<string>();

                            //    Command.Investor.ClientCommandQueue.Add(Message);
                            //}

                            lock (Business.Market.nj4xObject)
                                Business.Market.NJ4XTickets.Remove(newNJ4XTicket);

                            #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                            string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);
                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
                            string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), Command.Symbol.Digit);
                            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                            string contentServer = "'" + Command.Investor.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                                Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " [Failed - " + Model.Helper.Instance.GetMessage(eMessage) + "]";

                            TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.Investor.IpAddress, Command.Investor.Code);
                            #endregion
                        }
                    }
                    else
                    {
                        if (executionType == "manual- but automatic if no dealer online" ||
                            executionType == "automatic only" ||
                            executionType == "manual only- no automation")
                        {
                            lock (Business.Market.nj4xObject)
                                Business.Market.NJ4XTickets.Remove(newNJ4XTicket);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region TIME OUT
                Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.CloseCommand, EnumET5.ET5Message.TRADE_TIMEOUT, false, Command.ID, 1);

                //unknown error
                //string Message = "CloseCommand$False,timeout," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                //                   Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                //                   Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                //                   1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                //                   ",Close," + Command.CloseTime;

                //if (Command.Investor.ClientCommandQueue == null)
                //    Command.Investor.ClientCommandQueue = new List<string>();

                //Command.Investor.ClientCommandQueue.Add(Message);

                lock (Business.Market.nj4xObject)
                    Business.Market.NJ4XTickets.Remove(newNJ4XTicket);
                #endregion

                #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);
                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
                string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), Command.Symbol.Digit);
                string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                string contentServer = "'" + Command.Investor.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                    Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " [Failed - time out]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.Investor.IpAddress, Command.Investor.Code);
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiCloseCommand(OpenTrade Command)
        {
            //Check Condition Close Command
            Business.OpenTrade newOpenTrade = new OpenTrade();

            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.CloseTime = DateTime.Now;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.IsClose = true;
            newOpenTrade.IsMultiClose = Command.IsMultiClose;
            newOpenTrade.IsHedged = Command.IsHedged;
            newOpenTrade.Margin = Command.Margin;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.OpenTime = Command.OpenTime;
            newOpenTrade.Profit = Command.Profit;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.Swap = Command.Swap;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Taxes = Command.Taxes;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.IsServer = Command.IsServer;
            newOpenTrade.AgentCommission = Command.AgentCommission;

            if (string.IsNullOrEmpty(Command.Comment))
            {
                newOpenTrade.Comment = "[multi close spot command]";
            }
            else
            {
                newOpenTrade.Comment = Command.Comment;
            }
            
            //Call Function Update Command Of Inveestor
            newOpenTrade.Investor.UpdateCommand(newOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiUpdateCommand(OpenTrade Command)
        {
            #region NEW INSTANCE OPEN TRADE
            Business.OpenTrade newOpenTrade = new OpenTrade();
            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.CloseTime = Command.CloseTime;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.AgentCommission = Command.AgentCommission;
            newOpenTrade.Taxes = Command.Taxes;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.IsMultiUpdate = true;

            if (string.IsNullOrEmpty(Command.Comment))
            {
                newOpenTrade.Comment = "[update spot command]";
            }
            else
            {
                newOpenTrade.Comment = Command.Comment;
            }
            #endregion

            newOpenTrade.Investor.UpdateCommand(newOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void UpdateCommand(OpenTrade Command)
        {
            string content = string.Empty;
            string comment = "[modify order]";
            string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(Command.Type.ID).ToLower();

            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

            #region NEW INSTANCE OPEN TRADE
            Business.OpenTrade newOpenTrade = new OpenTrade();
            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.CloseTime = Command.CloseTime;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.AgentCommission = Command.AgentCommission;
            newOpenTrade.Taxes = Command.Taxes;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.Comment = Command.Comment;
            #endregion

            double TakeProfit = 0;
            double StopLoss = 0;
            TakeProfit = Command.TakeProfit;
            StopLoss = Command.StopLoss;

            #region CONNECT MT4 UPDATE COMMAND
            if (Business.Market.IsConnectMT4)
            {
                if (!Business.Market.StatusConnect)
                {
                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.UpdateCommand, EnumET5.ET5Message.DISCONNECT_FROM_MT4, false, Command.ID, 2);

                    //string Message = "UpdateCommand$False,DISCONNECT FROM MT4," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                    //                        Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                    //                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                    //                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Update";

                    //if (Command.Investor.ClientCommandQueue == null)
                    //    Command.Investor.ClientCommandQueue = new List<string>();

                    //Command.Investor.ClientCommandQueue.Add(Message);

                    return;
                }

                string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");

                if (executionType == "manual only- no automation" ||
                    executionType == "manual- but automatic if no dealer online" ||
                    executionType == "automatic only")
                {
                    #region Set Property IsBuy And CommandType Send To Client
                    bool IsBuy = Model.Helper.Instance.IsBuy(Command.Type.ID);

                    string CommandType = Model.Helper.Instance.convertCommandTypeIDToString(Command.Type.ID);
                    #endregion                    

                    #region CONNECT MT4 USING NJ4X
                    string cmd = NJ4XConnectSocket.MapNJ4X.Instance.MapOrderModify(Command.ID, Command.OpenPrice, Command.StopLoss, Command.TakeProfit,
                                                                                    Command.Investor.Code, Command.Investor.UnZipPwd);

                    NJ4XConnectSocket.NJ4XTicket newNJ4XTicket = new NJ4XConnectSocket.NJ4XTicket();
                    newNJ4XTicket.Ask = 0;
                    newNJ4XTicket.Bid = 0;
                    newNJ4XTicket.Code = Command.Investor.Code;
                    newNJ4XTicket.IsDisable = false;
                    newNJ4XTicket.IsReQuote = false;
                    newNJ4XTicket.IsUpdate = true;
                    newNJ4XTicket.IsRequest = false;
                    newNJ4XTicket.OpenPrice = Command.OpenPrice;
                    newNJ4XTicket.Symbol = Command.Symbol.Name;
                    newNJ4XTicket.Ticket = -1;
                    newNJ4XTicket.Execution = Command.Symbol.ExecutionTrade;
                    newNJ4XTicket.OpenPrice = Command.OpenPrice;
                    newNJ4XTicket.ClosePrices = Command.ClosePrice;
                    newNJ4XTicket.Digit = Command.Symbol.Digit;
                    newNJ4XTicket.Command = Command;

                    Business.Market.NJ4XTickets.Add(newNJ4XTicket);

                    string result = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(cmd);

                    if (!string.IsNullOrEmpty(result))
                    {
                        string[] subResult = result.Split('$');
                        if (subResult[0] == "OrderModify")
                        {
                            string[] subnj4x = subResult[1].Split('{');
                            bool _isClose = bool.Parse(subnj4x[0]);
                            int ticket = int.Parse(subnj4x[1]);

                            if (!_isClose)
                            {
                                if (ticket != 135 && ticket != 138)
                                {
                                    EnumET5.ET5Message eMessage = this.GetEnumWithTicket(ticket);
                                    Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.UpdateCommand, eMessage, false, Command.ID, 2);

                                    //string strError = this.GetErrorWithCode(ticket);
                                    //string Message = "UpdateCommand$False," + strError + "," + Command.ID + "," +
                                    //                                            Command.Investor.InvestorID + "," +
                                    //                                            Command.Symbol.Name + "," +
                                    //                                            Command.Size + "," + IsBuy + "," +
                                    //                                            Command.OpenTime + "," +
                                    //                                            Command.OpenPrice + "," +
                                    //                                            Command.StopLoss + "," +
                                    //                                            Command.TakeProfit + "," +
                                    //                                            Command.ClosePrice + "," +
                                    //                                            Command.Commission + "," +
                                    //                                            Command.Swap + "," +
                                    //                                            Command.Profit + "," +
                                    //                                            Command.Comment + "," +
                                    //                                            Command.ID + "," +
                                    //                                            Command.Type.Name + "," +
                                    //                                            1 + "," + Command.ExpTime + "," +
                                    //                                            Command.ClientCode + "," +
                                    //                                            Command.CommandCode + "," +
                                    //                                            Command.IsHedged + "," +
                                    //                                            Command.Type.ID + "," +
                                    //                                            Command.Margin + ",Update";

                                    //if (Command.Investor.ClientCommandQueue == null)
                                    //    Command.Investor.ClientCommandQueue = new List<string>();

                                    //Command.Investor.ClientCommandQueue.Add(Message);

                                    lock (Business.Market.nj4xObject)
                                        Business.Market.NJ4XTickets.Remove(newNJ4XTicket);

                                    string Content = string.Empty;
                                    Content = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                        openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ") - [Failed - " + Model.Helper.Instance.GetMessage(eMessage) + "]";

                                    TradingServer.Facade.FacadeAddNewSystemLog(5, Content, comment, Command.Investor.IpAddress, Command.Investor.Code);

                                }
                            }
                            else
                            {
                                if (executionType == "manual only- no automation" ||
                                    executionType == "manual- but automatic if no dealer online" ||
                                    executionType == "automatic only")
                                {
                                    Business.Market.NJ4XTickets.Remove(newNJ4XTicket);
                                }   
                            }
                        }
                    }
                    else
                    {
                        Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.UpdateCommand, EnumET5.ET5Message.TIME_OUT, false, -1, 2);

                        ////unknown error
                        //string Message = "UpdateCommand$False,timeout," + -1 + "," + Command.Investor.InvestorID + "," +
                        //                       Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                        //                           Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                        //                           CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                        //if (Command.Investor.ClientCommandQueue == null)
                        //    Command.Investor.ClientCommandQueue = new List<string>();

                        //Command.Investor.ClientCommandQueue.Add(Message);

                        lock (Business.Market.nj4xObject)
                            Business.Market.NJ4XTickets.Remove(newNJ4XTicket);

                        string Content = string.Empty;
                        Content = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ") - [Failed - time out]";

                        TradingServer.Facade.FacadeAddNewSystemLog(5, Content, comment, Command.Investor.IpAddress, Command.Investor.Code);
                    }
                    #endregion
                }
                else
                {
                    #region MODE AUTOMATIC
                    string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertUpdateOnlineCommandToString(Command.RefCommandID, Command.OpenPrice,
                        Command.StopLoss, Command.TakeProfit, Command.Comment);

                    string resultUpdate = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                    bool isUpdateSuccess = false;
                    string strError = string.Empty;

                    bool IsBuy = false;
                    if (Command.Type.ID == 1 || Command.Type.ID == 7 || Command.Type.ID == 9)
                        IsBuy = true;

                    if (!string.IsNullOrEmpty(resultUpdate))
                    {
                        string[] subValue = resultUpdate.Split('$');
                        if (subValue.Length == 2)
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (int.Parse(subParameter[0]) == 1)
                                isUpdateSuccess = true;

                            if (!isUpdateSuccess)
                                strError = subParameter[1];
                        }

                        if (!isUpdateSuccess)
                        {
                            Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.UpdateCommand, EnumET5.ET5Message.CAN_NOT_UPDATE_COMMAND_FROM_MT4, false, Command.ID, 2);

                            //string Message = "UpdateCommand$False,CAN'T UPDATE COMMAND FROM MT4," + Command.ID + "," +
                            //                Command.Investor.InvestorID + "," +
                            //                Command.Symbol.Name + "," +
                            //                Command.Size + "," + false + "," +
                            //                Command.OpenTime + "," +
                            //                Command.OpenPrice + "," +
                            //                Command.StopLoss + "," +
                            //                Command.TakeProfit + "," +
                            //                Command.ClosePrice + "," +
                            //                Command.Commission + "," +
                            //                Command.Swap + "," +
                            //                Command.Profit + "," + "Comment," +
                            //                Command.ID + "," +
                            //                Command.Type.Name + "," +
                            //                1 + "," + Command.ExpTime + "," +
                            //                Command.ClientCode + "," +
                            //                Command.CommandCode + "," +
                            //                Command.IsHedged + "," +
                            //                Command.Type.ID + "," +
                            //                Command.Margin + ",Update";

                            //if (Command.Investor.ClientCommandQueue == null)
                            //    Command.Investor.ClientCommandQueue = new List<string>();

                            //Command.Investor.ClientCommandQueue.Add(Message);
                        }
                    }
                    else
                    {
                        Model.Helper.Instance.SendCommandToClient(Command, EnumET5.CommandName.UpdateCommand, EnumET5.ET5Message.CAN_NOT_UPDATE_COMMAND_FROM_MT4, false, Command.ID, 2);

                        //string Message = "UpdateCommand$False,CAN'T UPDATE COMMAND FROM MT4," + Command.ID + "," +
                        //                    Command.Investor.InvestorID + "," +
                        //                    Command.Symbol.Name + "," +
                        //                    Command.Size + "," + false + "," +
                        //                    Command.OpenTime + "," +
                        //                    Command.OpenPrice + "," +
                        //                    Command.StopLoss + "," +
                        //                    Command.TakeProfit + "," +
                        //                    Command.ClosePrice + "," +
                        //                    Command.Commission + "," +
                        //                    Command.Swap + "," +
                        //                    Command.Profit + "," + "Comment," +
                        //                    Command.ID + "," +
                        //                    Command.Type.Name + "," +
                        //                    1 + "," + Command.ExpTime + "," +
                        //                    Command.ClientCode + "," +
                        //                    Command.CommandCode + "," +
                        //                    Command.IsHedged + "," +
                        //                    Command.Type.ID + "," +
                        //                    Command.Margin + ",Update";

                        //if (Command.Investor.ClientCommandQueue == null)
                        //    Command.Investor.ClientCommandQueue = new List<string>();

                        //Command.Investor.ClientCommandQueue.Add(Message);
                    }
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public IPresenter.CloseCommandDelegate CloseCommandNotify(OpenTrade Command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        public IPresenter.SendClientCmdDelegate SendClientCmdDelegate(string Cmd)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        public void SetTickValueNotify(Tick Tick, Symbol RefSymbol)
        {
            NumCheck++;
            if (NumCheck == 100)
                NumCheck = 0;

            #region Process Command List
            //Call Function Calculate Command 
            if (RefSymbol.CommandList != null && RefSymbol.CommandList.Count > 0)
            {
                int count = RefSymbol.CommandList.Count;
                //for (int i = 0; i < RefSymbol.CommandList.Count; i++)
                for (int i = count - 1; i >= 0; i--)
                {
                    if (RefSymbol.CommandList[i].IsClose == true)
                        continue;

                    //if (RefSymbol.CommandList[i].IsProcess)
                    //    continue;

                    #region Switch Condition Type
                    //Set Close Price For Command     
                    switch (RefSymbol.CommandList[i].Type.ID)
                    {
                        case 1: //Spot Buy Command                    
                            RefSymbol.CommandList[i].ClosePrice = Tick.Bid;
                            break;

                        case 2: //Spot Sell Command                     
                            double Ask = 0;
                            Ask = (Symbol.ConvertNumberPip(RefSymbol.CommandList[i].Symbol.Digit, RefSymbol.CommandList[i].SpreaDifferenceInOpenTrade) + Tick.Ask);
                            RefSymbol.CommandList[i].ClosePrice = Ask;
                            break;

                        case 7: //Buy Limit Command
                            double AskBuyLimit = 0;
                            AskBuyLimit = (Symbol.ConvertNumberPip(RefSymbol.CommandList[i].Symbol.Digit, RefSymbol.CommandList[i].SpreaDifferenceInOpenTrade) + Tick.Ask);
                            RefSymbol.CommandList[i].ClosePrice = AskBuyLimit;
                            break;

                        case 8: //Sell Limit Command                            
                            RefSymbol.CommandList[i].ClosePrice = Tick.Bid;
                            break;

                        case 9: //Buy Stop Command
                            double AskBuyStop = 0;
                            AskBuyStop = (Symbol.ConvertNumberPip(RefSymbol.CommandList[i].Symbol.Digit, RefSymbol.CommandList[i].SpreaDifferenceInOpenTrade) + Tick.Ask);
                            RefSymbol.CommandList[i].ClosePrice = AskBuyStop;
                            break;

                        case 10:    //Sell Stop Command                            
                            RefSymbol.CommandList[i].ClosePrice = Tick.Bid;
                            break;
                    }
                    #endregion

                    //Call Function Calculator Command
                    Business.OpenTrade newOpenTrade = new OpenTrade();
                    newOpenTrade = this.CalculateCommand(RefSymbol.CommandList[i]);

                    #region COMMENT CODE BECAUSE PENDING DON'T ACTIVE(15/07/2011)
                    //if (RefSymbol.CommandList[i].Type.ID != newOpenTrade.Type.ID)
                    //{
                    //    if (newOpenTrade.Type != null)
                    //    {
                    //        RefSymbol.CommandList[i].Type = new TradeType();
                    //        RefSymbol.CommandList[i].Type.ID = newOpenTrade.Type.ID;
                    //        RefSymbol.CommandList[i].Type.Name = newOpenTrade.Type.Name;

                    //        //RefSymbol.CommandList[i].Type = newOpenTrade.Type;
                    //    }
                    //}
                    #endregion

                    if (newOpenTrade.IsClose == true)
                        RefSymbol.CommandList[i].IsClose = true;

                    if (RefSymbol.CommandList[i].Investor.CommandList.Count > 0 && RefSymbol.CommandList[i].Investor.CommandList != null)
                    {
                        RefSymbol.CommandList[i].Investor.UpdateCommand(newOpenTrade);
                    }
                }
            } 
            #endregion    

            Facade.FacadeCalculationAlert(Tick, RefSymbol);
        }       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public OpenTrade CalculateCommand(OpenTrade Command)
        {
            Business.OpenTrade newOpenTrade = new OpenTrade();
            newOpenTrade.ClientCode = Command.ClientCode;            
            newOpenTrade.CloseTime = Command.CloseTime;            
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.IsClose = Command.IsClose;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.OpenTime = Command.OpenTime;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.Swap = Command.Swap;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Type = new TradeType();
            newOpenTrade.Type.ID = Command.Type.ID;
            newOpenTrade.Type.Name = Command.Type.Name;
            //newOpenTrade.Type = Command.Type;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.Margin = Command.Margin;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.AgentRefConfig = Command.AgentRefConfig;

            switch (newOpenTrade.Type.ID)
            {
                #region Case Buy Spot Command
                case 1:
                    {
                        #region COMMENT CODE
                        if (newOpenTrade.TakeProfit != 0)
                        {
                            if (Command.ClosePrice >= newOpenTrade.TakeProfit)
                            {
                                //call function close command         
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.TakeProfit;

                                //Calculator Profit Of Command                        
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false,
                                                        newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                newOpenTrade.IsActivePending = false;
                                newOpenTrade.IsStopLossAndTakeProfit = true;

                                newOpenTrade.UpdateIsActivePending(false, true, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT TAKE PROFIT)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'server-" + Command.Investor.Code + "': take profit #" + Command.CommandCode + " at " + takeProfit + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[take profit]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        if (newOpenTrade.StopLoss != 0)
                        {
                            if (Command.ClosePrice <= newOpenTrade.StopLoss)
                            {
                                //call function close command
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.StopLoss;

                                //Calculator Profit Of Command                        
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                newOpenTrade.IsActivePending = false;
                                newOpenTrade.IsStopLossAndTakeProfit = true;
                                newOpenTrade.UpdateIsActivePending(false, true, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT STOP LOSS)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string stoploss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'server-" + Command.Investor.Code + "': stop loss #" + Command.CommandCode + " at " + stoploss + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[stop loss]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        //=================================

                        //Calculator Profit Of Command                        
                        //newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                        //newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, 
                        //    newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);
                        #endregion
                    }
                    break;
                #endregion                

                #region Case Sell Spot Command
                case 2:
                    {
                        #region COMMENT CODE
                        if (newOpenTrade.TakeProfit != 0)
                        {
                            if (Command.ClosePrice <= newOpenTrade.TakeProfit)
                            {
                                //call Function Close Command
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.TakeProfit;

                                //Calculator Profit Of Command
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                newOpenTrade.IsActivePending = false;
                                newOpenTrade.IsStopLossAndTakeProfit = true;
                                newOpenTrade.UpdateIsActivePending(false, true, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                //INSERT SYSTEM LOG
                                #region INSERT SYSTEM LOG (EVENT TAKE PROFIT)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'server-" + Command.Investor.Code + "': take profit #" + Command.CommandCode + " at " + takeProfit + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[take profit]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        if (newOpenTrade.StopLoss != 0)
                        {
                            if (Command.ClosePrice >= newOpenTrade.StopLoss)
                            {
                                //call function close command
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.StopLoss;

                                //Calculator Profit Of Command
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                newOpenTrade.IsActivePending = false;
                                newOpenTrade.IsStopLossAndTakeProfit = true;
                                newOpenTrade.UpdateIsActivePending(false, true, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT TAKE PROFIT)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'server-" + Command.Investor.Code + "': stop loss #" + Command.CommandCode + " at " + stopLoss + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[stop loss]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }


                        //========================
                        //Calculator Profit Of Command
                        //newOpenTrade.CalculatorProfitCommand(newOpenTrade);                        
                        //newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, 
                        //    newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);
                        #endregion
                    }
                    break;
                #endregion                

                #region Case Buy Limit Command
                case 7:
                    {
                        if (Command.ClosePrice <= newOpenTrade.OpenPrice)
                        {
                            newOpenTrade.IsActivePending = true;
                            newOpenTrade.IsStopLossAndTakeProfit = false;   
                            TradingServer.Facade.FacadeFindDealerActivePendingRequest(newOpenTrade);
                            newOpenTrade.UpdateIsActivePending(true, false, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);

                            #region COMMENT CODE(BECAUSE PENDING ORDER ACTIVE FROM DEALER) 11/05/2012
                            //newOpenTrade.OpenTime = DateTime.Now;(COMMENT BECAUSE FIX ERROR ACTIVE PENDING ORDER BUT ENOUGH MONEY)

                            //newOpenTrade.OpenPrice = Command.ClosePrice;

                            //int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 1)
                            //    {
                            //        newOpenTrade.Type = new TradeType();
                            //        newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                            //        newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                            //        //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                            //        //SYSTEM LOG ACTIVATE PENDING ORDER 
                            //        string content = string.Empty;
                            //        content = "'server: '" + "pending buy limit order #" + newOpenTrade.CommandCode + " triggered";
                            //        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                            //        break;
                            //    }
                            //}
                            #endregion
                        }
                    }
                    break;
                #endregion

                #region Case Sell Limit Command
                case 8:
                    {   
                        if (newOpenTrade.ClosePrice >= newOpenTrade.OpenPrice)
                        {
                            newOpenTrade.IsActivePending = true;
                            newOpenTrade.IsStopLossAndTakeProfit = false;
                            TradingServer.Facade.FacadeFindDealerActivePendingRequest(newOpenTrade);
                            newOpenTrade.UpdateIsActivePending(true, false, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);

                            #region COMMENT CODE(BECAUSE PENDING ORDER ACTIVE FROM DEALER) 11/05/2012
                            //Make sell command
                            //newOpenTrade.OpenTime = DateTime.Now;

                            //newOpenTrade.OpenPrice = Command.ClosePrice;

                            //int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 2)
                            //    {
                            //        newOpenTrade.Type = new TradeType();
                            //        newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                            //        newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                            //        //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                            //        //SYSTEM LOG ACTIVATE PENDING ORDER 
                            //        string content = string.Empty;
                            //        content = "'server: '" + "pending sell limit order #" + newOpenTrade.CommandCode + " triggered";
                            //        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                            //        break;
                            //    }
                            //}
                            #endregion  
                        }                        
                    }
                    break;
                #endregion

                #region Case Buy Stop Command
                case 9:
                    {   
                        if (newOpenTrade.ClosePrice >= newOpenTrade.OpenPrice)
                        {
                            newOpenTrade.IsActivePending = true;
                            newOpenTrade.IsStopLossAndTakeProfit = false;
                            TradingServer.Facade.FacadeFindDealerActivePendingRequest(newOpenTrade);
                            newOpenTrade.UpdateIsActivePending(true, false, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);
                            #region COMMENT CODE(BECAUSE PENDING ORDER ACTIVE FROM DEALER) 11/05/2012
                            //Make sell command
                            //newOpenTrade.OpenTime = DateTime.Now;                            

                            //int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 1)
                            //    {
                            //        newOpenTrade.Type = new TradeType();
                            //        newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                            //        newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                            //        //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                            //        //SYSTEM LOG ACTIVATE PENDING ORDER 
                            //        string content = string.Empty;
                            //        content = "'server: '" + "pending buy stop order #" + newOpenTrade.CommandCode + " triggered";
                            //        TradingServer.Facade.FacadeAddNewSystemLog(1, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                            //        break;
                            //    }
                            //}
                            #endregion
                        }                        
                    }
                    break;
                #endregion

                #region Case Sell Stop Command
                case 10:
                    {   
                        if (newOpenTrade.ClosePrice <= newOpenTrade.OpenPrice)
                        {
                            newOpenTrade.IsActivePending = true;
                            newOpenTrade.IsStopLossAndTakeProfit = false;
                            TradingServer.Facade.FacadeFindDealerActivePendingRequest(newOpenTrade);

                            newOpenTrade.UpdateIsActivePending(true, false, newOpenTrade.ID, newOpenTrade.Investor.InvestorID);
                            #region COMMENT CODE(BECAUSE PENDING ORDER ACTIVE FROM DEALER) 11/05/2012
                            //Make sell command
                            //newOpenTrade.OpenTime = DateTime.Now;

                            //newOpenTrade.OpenPrice = Command.ClosePrice;

                            //int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 2)
                            //    {
                            //        newOpenTrade.Type = new TradeType();
                            //        newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                            //        newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                            //        //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                            //        //SYSTEM LOG ACTIVATE PENDING ORDER 
                            //        string content = string.Empty;
                            //        content = "'server: '" + "pending sell stop order #" + newOpenTrade.CommandCode + " triggered";
                            //        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                            //        break;
                            //    }
                            //}
                            #endregion
                        }
                    }
                    break;
                #endregion

                #region DEFAULT
                default:
                    {
                        if (newOpenTrade.Type == null)
                            TradingServer.Facade.FacadeAddNewSystemLog(1, "[type command is empty]", "[check type command]", "", newOpenTrade.CommandCode);
                        else
                            TradingServer.Facade.FacadeAddNewSystemLog(1, "[type command incorect] " + newOpenTrade.Type.ID, "[check type command]", "", newOpenTrade.CommandCode);
                    }
                    break;
                #endregion
            }

            return newOpenTrade;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public List<Business.OpenTrade> GetOpenTradeByInvestor(int InvestorID)
        {
            List<Business.OpenTrade> Result = new List<OpenTrade>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        Result = Business.Market.InvestorList[i].CommandList;

                        break;
                    }
                }
            }

            return Result;
        }
    }
}
