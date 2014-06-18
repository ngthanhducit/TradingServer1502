using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace TradingServer.Business
{
    public partial class Agent
    {
        internal static Dictionary<int, string> NoticeDealer = new Dictionary<int, string>();
        internal static Dictionary<int, Agent> NoticeManager = new Dictionary<int, Agent>();
        internal static Dictionary<string, ProcessQuoteLibrary.Business.BarTick> CandlesOffline = new Dictionary<string, ProcessQuoteLibrary.Business.BarTick>();
        internal static bool IsBusyListRequest = false;
        /// <summary>
        /// Start at App Start
        /// </summary>
        //public void CleanRecycleRequest()
        //{
        //    while (true)
        //    {
        //        System.Threading.Thread.Sleep(1000);
        //        this.RemoveDealerExpire();
        //        this.DeleteRequestExpire();
        //    }   
        //}

        /// <summary>
        /// 
        /// </summary>
        internal void WaitAccessListRequest()
        {
            while (IsBusyListRequest)
            {
                System.Threading.Thread.Sleep(100);
            }
            IsBusyListRequest = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp"></param>
        internal void ArchiveCandlesOffline(ProcessQuoteLibrary.Business.BarTick temp)
        {
            ProcessQuoteLibrary.Business.BarTick bar = new ProcessQuoteLibrary.Business.BarTick();
            bar.Symbol = temp.Symbol;
            bar.High = temp.High;
            bar.Low = temp.Low;
            bar.HighAsk = temp.HighAsk;
            bar.LowAsk = temp.LowAsk;
            if (CandlesOffline.ContainsKey(bar.Symbol))
            {
                CandlesOffline[bar.Symbol] = bar;
            }
            else
            {
                CandlesOffline.Add(bar.Symbol, bar);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal ProcessQuoteLibrary.Business.BarTick GetCandlesTwoM(string symbol)
        {
            ProcessQuoteLibrary.Business.BarTick bar = new ProcessQuoteLibrary.Business.BarTick();
            if (CandlesOffline.ContainsKey(symbol))
            {
                bar = CandlesOffline[symbol];
            }
            if (bar.Low == 0 | bar.High == 0)
            {
                bar = ProcessQuoteLibrary.Business.QuoteProcess.GetCandles(1, symbol);
            }
            else
            {
                ProcessQuoteLibrary.Business.BarTick temp = new ProcessQuoteLibrary.Business.BarTick();
                temp = ProcessQuoteLibrary.Business.QuoteProcess.GetCandles(1, symbol);
                if (temp.High > bar.High)
                {
                    bar.High = temp.High;
                }
                if (temp.HighAsk > bar.HighAsk)
                {
                    bar.HighAsk = temp.HighAsk;
                }
                if (temp.Low < bar.Low)
                {
                    bar.Low = temp.Low;
                }
                if (temp.LowAsk < bar.LowAsk)
                {
                    bar.LowAsk = temp.LowAsk;
                }
            }
            return bar;
        }

        /// <summary>
        /// Check Auto or Dealer
        /// </summary>
        /// <param name="request"></param>
        internal void CalculationOrderExecutionMode(RequestDealer command)
        {
            #region Fill Valiable
            string modeExOfSymbol = "";
            int pipMaximumDeviation = -1;
            string modeExOfGroup = "";
            int digits = command.Request.Symbol.Digit;
            bool isDealingPending = false, isDealingUpdate = false;
            for (int i = command.Request.IGroupSecurity.IGroupSecurityConfig.Count - 1; i >= 0; i--)
            {
                if (command.Request.IGroupSecurity.IGroupSecurityConfig[i].Code == "B03")
                {
                    modeExOfGroup = command.Request.IGroupSecurity.IGroupSecurityConfig[i].StringValue;
                }

                if (command.Request.IGroupSecurity.IGroupSecurityConfig[i].Code == "B06")
                {
                    int.TryParse(command.Request.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out pipMaximumDeviation);
                }

                if (command.Request.IGroupSecurity.IGroupSecurityConfig[i].Code == "B21")
                {
                    if (command.Request.IGroupSecurity.IGroupSecurityConfig[i].BoolValue == 1)
                    {
                        isDealingPending = true;
                    }
                    else
                    {
                        isDealingPending = false;
                    }
                }

                if (command.Request.IGroupSecurity.IGroupSecurityConfig[i].Code == "B22")
                {
                    if (command.Request.IGroupSecurity.IGroupSecurityConfig[i].BoolValue == 1)
                    {
                        isDealingUpdate = true;
                    }
                    else
                    {
                        isDealingUpdate = false;
                    }
                }
            }

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S006")
                {
                    modeExOfSymbol = command.Request.Symbol.ParameterItems[i].StringValue;
                    break;
                }
            }

            Business.Tick onlineTick = new Tick();
            double Pip = command.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
            onlineTick.Ask = Math.Round((command.Request.Symbol.TickValue.Ask + Pip), digits);
            onlineTick.Bid = command.Request.Symbol.TickValue.Bid;
            onlineTick.Status = command.Request.Symbol.TickValue.Status;
            onlineTick.SymbolID = command.Request.Symbol.SymbolID;
            onlineTick.SymbolName = command.Request.Symbol.Name;
            onlineTick.TickTime = command.Request.Symbol.TickValue.TickTime;
            command.Request.ClosePrice = Math.Round(command.Request.ClosePrice, digits);

            #endregion

            switch (modeExOfGroup)
            {
                #region super auto
                case "super auto":
                    try
                    {
                        switch (command.Name.ToLower())
                        {
                            case "update":
                                {
                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                    break;
                                }
                            case "updatepending":
                                {
                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                    break;
                                }
                            case "closepending":
                                {
                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                    break;
                                }
                            case "openpending":
                                {
                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                    break;
                                }
                            case "open":
                                {
                                    AutoSuperDealerOpen(command, digits);
                                    break;
                                }
                            case "close":
                                {
                                    AutoSuperDealerClose(command, digits);
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        TradingServer.Facade.FacadeAddNewSystemLog(1, "super auto Invetor_" + command.InvestorID, ex.Message, "123", "");
                    }
                    break;
                #endregion

                #region manual only- no automation
                case "manual only- no automation":
                    this.WaitAccessListRequest();
                    try
                    {
                        bool check = true;
                        switch (command.Name.ToLower())
                        {
                            case "update":
                                {
                                    if (!isDealingUpdate)
                                    {
                                        command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                        check = false;
                                    }
                                    break;
                                }
                            case "updatepending":
                                {
                                    if (!isDealingPending)
                                    {
                                        command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                        check = false;
                                    }
                                    break;
                                }
                            case "closepending":
                                {
                                    if (!isDealingPending)
                                    {
                                        command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                        check = false;
                                    }
                                    break;
                                }
                            case "openpending":
                                {
                                    if (!isDealingPending)
                                    {
                                        command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                        check = false;
                                    }
                                    break;
                                }
                        }
                        if (check)
                        {
                            if (modeExOfSymbol == "Request")
                            {
                                bool flag = ProcessModeExecutionRequest(command);
                                if (!flag)
                                {
                                    //this.CheckPriceAllowDealer(command, digits);
                                    this.DistributionRequestToDealer(command);
                                }
                            }
                            else
                            {
                                //this.CheckPriceAllowDealer(command, digits);
                                this.DistributionRequestToDealer(command);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TradingServer.Facade.FacadeAddNewSystemLog(1, "manual only- no automation Invetor_" + command.InvestorID + "Agent_" + command.AgentID, ex.Message, "123", "");
                    }
                    IsBusyListRequest = false;
                    break;
                #endregion
                #region automatic only
                case "automatic only":
                    switch (command.Name.ToLower())
                    {
                        case "update":
                            {
                                command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                break;
                            }
                        case "updatepending":
                            {
                                command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                break;
                            }
                        case "closepending":
                            {
                                command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                break;
                            }
                        case "openpending":
                            {
                                command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                break;
                            }
                        default:
                            {
                                if (modeExOfSymbol == "Request")
                                {
                                    this.AutoDealerRequest(command, onlineTick);
                                }
                                else
                                {
                                    bool checkPrice = false;
                                    if (command.Name.ToLower() == "open")
                                    {
                                        checkPrice = this.CheckPriceOpenAuto(command, digits);
                                        if (checkPrice)
                                        {
                                            AutoDealerOpen(command, onlineTick, modeExOfSymbol, pipMaximumDeviation, digits);
                                        }
                                    }
                                    if (command.Name.ToLower() == "close")
                                    {
                                        checkPrice = this.CheckPriceCloseAuto(command, digits);
                                        if (checkPrice)
                                        {
                                            AutoDealerClose(command, onlineTick, modeExOfSymbol, pipMaximumDeviation, digits);
                                        }
                                    }
                                }
                                break;
                            }
                    }

                    break;
                #endregion
                #region manual- but automatic if no dealer online
                case "manual- but automatic if no dealer online":
                    Business.Agent agent = new Agent();
                    agent = FindDealerProcessRequest(command);
                    bool checkAgent = true;
                    if (agent != null)
                    {
                        if (agent.AgentID > 0 || agent.IsVirtualDealer)
                        {
                            this.WaitAccessListRequest();
                            try
                            {
                                bool check = true;
                                switch (command.Name.ToLower())
                                {
                                    case "update":
                                        {
                                            if (!isDealingUpdate)
                                            {
                                                command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                check = false;
                                            }
                                            break;
                                        }
                                    case "updatepending":
                                        {
                                            if (!isDealingPending)
                                            {
                                                command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                check = false;
                                            }
                                            break;
                                        }
                                    case "closepending":
                                        {
                                            if (!isDealingPending)
                                            {
                                                command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                                check = false;
                                            }
                                            break;
                                        }
                                    case "openpending":
                                        {
                                            if (!isDealingPending)
                                            {
                                                command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                                check = false;
                                            }
                                            break;
                                        }
                                }
                                if (check)
                                {
                                    if (modeExOfSymbol == "Request")
                                    {
                                        bool flag = ProcessModeExecutionRequest(command);
                                        if (!flag)
                                        {
                                            //this.CheckPriceAllowDealer(command, digits);
                                            this.DistributionRequestToDealer(command);
                                        }
                                    }
                                    else
                                    {
                                        //this.CheckPriceAllowDealer(command, digits);
                                        this.DistributionRequestToDealer(command);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TradingServer.Facade.FacadeAddNewSystemLog(1, "manual- but automatic if no dealer online Invetor_" + command.InvestorID + "Agent_" + command.AgentID, ex.Message, "123", "");
                            }
                            checkAgent = false;
                            IsBusyListRequest = false;
                        }
                    }
                    if (checkAgent)
                    {
                        switch (command.Name.ToLower())
                        {
                            case "update":
                                {
                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                    break;
                                }
                            case "updatepending":
                                {
                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                    break;
                                }
                            case "closepending":
                                {
                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                    break;
                                }
                            case "openpending":
                                {
                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                    break;
                                }
                            default:
                                {
                                    if (modeExOfSymbol == "Request")
                                    {
                                        this.AutoDealerRequest(command, onlineTick);
                                    }
                                    else
                                    {
                                        bool checkPrice = false;
                                        if (command.Name.ToLower() == "open")
                                        {
                                            checkPrice = this.CheckPriceOpenAuto(command, digits);
                                            if (checkPrice)
                                            {
                                                AutoDealerOpen(command, onlineTick, modeExOfSymbol, pipMaximumDeviation, digits);
                                            }
                                        }
                                        if (command.Name.ToLower() == "close")
                                        {
                                            checkPrice = this.CheckPriceCloseAuto(command, digits);
                                            if (checkPrice)
                                            {
                                                AutoDealerClose(command, onlineTick, modeExOfSymbol, pipMaximumDeviation, digits);
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                #endregion
            }
        }

        /// <summary>
        /// Only use Mode Request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal bool ProcessModeExecutionRequest(Business.RequestDealer command)
        {
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                {
                    switch (command.Name.ToLower())
                    {
                        case "open":
                            if (Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode)
                            {
                                Market.ListRequestDealer[i].Notice = "RD29";
                                Market.ListRequestDealer[i].FlagConfirm = true;
                                //this.AddRequestDealerToInvestor(command);
                                Market.ListRequestDealer[i].Request.Symbol.MarketAreaRef.AddCommand(Market.ListRequestDealer[i].Request);
                                Market.ListRequestDealer.RemoveAt(i);
                                command.AgentCode = "system";
                                command.Answer = "Confirm";
                                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                return true;
                            }
                            break;

                        case "close":
                            if (Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode)
                            {
                                Market.ListRequestDealer[i].Notice = "RD30";
                                Market.ListRequestDealer[i].FlagConfirm = true;
                                //this.AddRequestDealerToInvestor(command);
                                Market.ListRequestDealer[i].Request.Symbol.MarketAreaRef.CloseCommand(Market.ListRequestDealer[i].Request);
                                Market.ListRequestDealer.RemoveAt(i);
                                command.AgentCode = "system";
                                command.Answer = "Confirm";
                                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                return true;
                            }
                            break;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Only use Mode Request
        /// </summary>
        /// <param name="command"></param>
        /// <param name="onlineTick"></param>
        internal void AutoDealerRequest(RequestDealer command, Business.Tick onlineTick)
        {
            bool flag = ProcessModeExecutionRequest(command);
            if (!flag)
            {
                int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
                if (type == 1)
                {
                    if (command.Name.ToLower() == "open")
                    {
                        command.Request.OpenPrice = onlineTick.Ask;
                        command.Request.OpenTime = onlineTick.TickTime;
                    }
                    else
                    {
                        command.Request.ClosePrice = onlineTick.Bid;
                    }
                    command.FlagConfirm = true;
                    command.TimeAgentReceive = DateTime.Now;
                    command.Notice = "RD02";
                    this.AddRequestDealerToInvestor(command);
                    Market.ListRequestDealer.Add(command);
                    command.AgentCode = "system";
                    command.Answer = "Quotes";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system quotes " + command.LogRequest, "Quotes", "", "system");
                    return;
                }
                else if (type == 2)
                {
                    if (command.Name.ToLower() == "open")
                    {
                        command.Request.OpenPrice = onlineTick.Bid;
                        command.Request.OpenTime = onlineTick.TickTime;
                    }
                    else
                    {
                        command.Request.ClosePrice = onlineTick.Ask;
                    }
                    command.FlagConfirm = true;
                    command.TimeAgentReceive = DateTime.Now;
                    command.Notice = "RD02";
                    this.AddRequestDealerToInvestor(command);
                    Market.ListRequestDealer.Add(command);
                    command.AgentCode = "system";
                    command.Answer = "Quotes";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system quotes " + command.LogRequest, "Quotes", "", "system");
                    return;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onlineTick"></param>
        /// <returns></returns>
        internal void AutoDealerOpen(Business.RequestDealer command, Business.Tick onlineTick, string modeExOfSymbol, int pipMaximumDeviation, int digits)
        {
            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                if (command.Request.OpenPrice == onlineTick.Ask)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD02";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }

                if (command.Request.OpenPrice > onlineTick.Ask)
                {
                    //Need Check Again in autoDealer has work Instant or no? 
                    if (modeExOfSymbol == "Instant")
                    {
                        double pipdiff = command.Request.OpenPrice - onlineTick.Ask;
                        double pipDV = Business.Symbol.ConvertNumberPip(digits, pipMaximumDeviation);
                        if (pipdiff > pipDV)
                        {
                            command.FlagConfirm = false;
                            command.Notice = "RD01";
                            this.AddRequestDealerToInvestor(command);
                            command.AgentCode = "system";
                            command.Answer = "Requote";
                            TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                            TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                            return;
                        }
                    }

                    command.FlagConfirm = true;
                    command.Notice = "RD03";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    //this.SendNoticeManagerRequest(1,command.Request);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD04";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                    return;
                }
            }
            else if (type == 2)
            {
                if (command.Request.OpenPrice == onlineTick.Bid)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD02";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                if (command.Request.OpenPrice > onlineTick.Bid)
                {
                    //Need Check Again in autoDealer has work Instant or no? 
                    if (modeExOfSymbol == "Instant")
                    {
                        double pipdiff = command.Request.OpenPrice - onlineTick.Bid;
                        double pipDV = Business.Symbol.ConvertNumberPip(digits, pipMaximumDeviation);
                        if (pipdiff > pipDV)
                        {
                            command.FlagConfirm = false;
                            command.Notice = "RD05";
                            this.AddRequestDealerToInvestor(command);
                            command.AgentCode = "system";
                            command.Answer = "Requote";
                            TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                            TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                            return;
                        }
                    }
                    command.FlagConfirm = true;
                    command.Notice = "RD06";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD07";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onlineTick"></param>
        /// <returns></returns>
        internal void AutoDealerClose(Business.RequestDealer command, Business.Tick onlineTick, string modeExOfSymbol, int pipMaximumDeviation, int digits)
        {
            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                if (command.Request.ClosePrice == onlineTick.Bid)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD10";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                if (command.Request.ClosePrice < onlineTick.Bid)
                {
                    //Need Check Again in autoDealer has work or no? 
                    if (modeExOfSymbol == "Instant")
                    {
                        double pipdiff = onlineTick.Bid - command.Request.ClosePrice;
                        double pipDV = Business.Symbol.ConvertNumberPip(digits, pipMaximumDeviation);
                        if (pipdiff > pipDV)
                        {
                            command.FlagConfirm = false;
                            command.Notice = "RD09";
                            this.AddRequestDealerToInvestor(command);
                            command.AgentCode = "system";
                            command.Answer = "Requote";
                            TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                            TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                            return;
                        }
                    }
                    command.FlagConfirm = true;
                    command.Notice = "RD11";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD12";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                    return;
                }
            }
            else if (type == 2)
            {
                if (command.Request.ClosePrice == onlineTick.Ask)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD10";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                if (command.Request.ClosePrice > onlineTick.Ask)
                {
                    //Need Check Again in autoDealer has work or no? 
                    if (modeExOfSymbol == "Instant")
                    {
                        double pipdiff = command.Request.OpenPrice - onlineTick.Ask;
                        double pipDV = Business.Symbol.ConvertNumberPip(digits, pipMaximumDeviation);
                        if (pipdiff > pipDV)
                        {
                            command.FlagConfirm = false;
                            command.Notice = "RD13";
                            this.AddRequestDealerToInvestor(command);
                            command.AgentCode = "system";
                            command.Answer = "Requote";
                            TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                            TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                            return;
                        }
                    }
                    command.FlagConfirm = true;
                    command.Notice = "RD14";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD15";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequest, "Requote", "", "system");
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="digits"></param>
        internal void AutoSuperDealerClose(Business.RequestDealer command, int digits)
        {
            ProcessQuoteLibrary.Business.BarTick barBid = new ProcessQuoteLibrary.Business.BarTick();
            barBid = this.GetCandlesTwoM(command.Request.Symbol.Name);
            barBid.High = Math.Round(barBid.High, digits);
            barBid.Low = Math.Round(barBid.Low, digits);
            barBid.HighAsk = Math.Round(barBid.HighAsk, digits);
            barBid.LowAsk = Math.Round(barBid.LowAsk, digits);

            //double spreadDifference = command.Request.Symbol.SpreadDifference;
            double spreadDifference = command.Request.SpreaDifferenceInOpenTrade;
            double spreadDefault = 0;

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S013")
                {
                    double.TryParse(command.Request.Symbol.ParameterItems[i].NumValue, out spreadDefault);
                    break;
                }
            }

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                if (barBid.Low <= command.Request.ClosePrice && command.Request.ClosePrice <= barBid.High)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD11";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD12";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequestSuper(barBid.Low, barBid.High), "Requote", "", "system");
                    return;
                }
            }
            else if (type == 2)
            {
                ProcessQuoteLibrary.Business.BarTick barAsk = new ProcessQuoteLibrary.Business.BarTick();
                barAsk = barBid;
                double spreadDiff = Business.Symbol.ConvertNumberPip(digits, spreadDifference);
                double spreadDef = Business.Symbol.ConvertNumberPip(digits, spreadDefault);
                if (command.Request.Symbol.ApplySpread)
                {
                    double high = (double)barBid.High + (double)spreadDiff + (double)spreadDef;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.Low + (double)spreadDiff + (double)spreadDef;
                    barAsk.Low = Math.Round(low, digits);
                }
                else
                {
                    double high = (double)barBid.HighAsk + (double)spreadDiff;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.LowAsk + (double)spreadDiff;
                    barAsk.Low = Math.Round(low, digits);
                }

                if (barAsk.High >= command.Request.ClosePrice && command.Request.ClosePrice >= barAsk.Low)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD14";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD15";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequestSuper(barAsk.Low, barAsk.High), "Requote", "", "system");
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="digits"></param>
        internal void AutoSuperDealerOpen(Business.RequestDealer command, int digits)
        {
            ProcessQuoteLibrary.Business.BarTick barBid = new ProcessQuoteLibrary.Business.BarTick();
            barBid = this.GetCandlesTwoM(command.Request.Symbol.Name);
            barBid.High = Math.Round(barBid.High, digits);
            barBid.Low = Math.Round(barBid.Low, digits);
            barBid.HighAsk = Math.Round(barBid.HighAsk, digits);
            barBid.LowAsk = Math.Round(barBid.LowAsk, digits);

            //double spreadDifference = command.Request.Symbol.SpreadDifference;
            double spreadDifference = command.Request.SpreaDifferenceInOpenTrade;
            double spreadDefault = 0;

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S013")
                {
                    double.TryParse(command.Request.Symbol.ParameterItems[i].NumValue, out spreadDefault);
                    break;
                }
            }

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                ProcessQuoteLibrary.Business.BarTick barAsk = new ProcessQuoteLibrary.Business.BarTick();
                barAsk = barBid;
                double spreadDiff = Business.Symbol.ConvertNumberPip(digits, spreadDifference);
                double spreadDef = Business.Symbol.ConvertNumberPip(digits, spreadDefault);
                if (command.Request.Symbol.ApplySpread)
                {
                    double high = (double)barBid.High + (double)spreadDiff + (double)spreadDef;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.Low + (double)spreadDiff + (double)spreadDef;
                    barAsk.Low = Math.Round(low, digits);
                }
                else
                {
                    double high = (double)barBid.HighAsk + (double)spreadDiff;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.LowAsk + (double)spreadDiff;
                    barAsk.Low = Math.Round(low, digits);
                }

                if (barAsk.High >= command.Request.OpenPrice && command.Request.OpenPrice >= barAsk.Low)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD03";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD04";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequestSuper(barAsk.Low, barAsk.High), "Requote", "", "system");
                    return;
                }
            }
            else if (type == 2)
            {
                if (barBid.Low <= command.Request.OpenPrice && command.Request.OpenPrice <= barBid.High)
                {
                    command.FlagConfirm = true;
                    command.Notice = "RD06";
                    //this.AddRequestDealerToInvestor(command);
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    command.AgentCode = "system";
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD07";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system requote " + command.LogRequestSuper(barBid.Low, barBid.High), "Requote", "", "system");
                    return;
                }
            }
        }

        internal void CheckPriceAllowDealer(Business.RequestDealer command, int digits)
        {
            if (command.Name.ToLower() == "close")
            {
                CheckPriceClose(command, digits);
            }
            else
            {
                CheckPriceOpen(command, digits);
            }
        }

        internal void CheckPriceOpen(Business.RequestDealer command, int digits)
        {
            ProcessQuoteLibrary.Business.BarTick barBid = new ProcessQuoteLibrary.Business.BarTick();
            barBid = this.GetCandlesTwoM(command.Request.Symbol.Name);
            barBid.High = Math.Round(barBid.High, digits);
            barBid.Low = Math.Round(barBid.Low, digits);
            barBid.HighAsk = Math.Round(barBid.HighAsk, digits);
            barBid.LowAsk = Math.Round(barBid.LowAsk, digits);

            //double spreadDifference = command.Request.Symbol.SpreadDifference;
            double spreadDifference = command.Request.SpreaDifferenceInOpenTrade;
            double spreadDefault = 0;

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S013")
                {
                    double.TryParse(command.Request.Symbol.ParameterItems[i].NumValue, out spreadDefault);
                    break;
                }
            }

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                ProcessQuoteLibrary.Business.BarTick barAsk = new ProcessQuoteLibrary.Business.BarTick();
                barAsk = barBid;
                double spreadDiff = Business.Symbol.ConvertNumberPip(digits, spreadDifference);
                double spreadDef = Business.Symbol.ConvertNumberPip(digits, spreadDefault);
                if (command.Request.Symbol.ApplySpread)
                {
                    double high = (double)barBid.High + (double)spreadDiff + (double)spreadDef;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.Low + (double)spreadDiff + (double)spreadDef;
                    barAsk.Low = Math.Round(low, digits);
                }
                else
                {
                    double high = (double)barBid.HighAsk + (double)spreadDiff;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.LowAsk + (double)spreadDiff;
                    barAsk.Low = Math.Round(low, digits);
                }

                if (barAsk.High >= command.Request.OpenPrice && command.Request.OpenPrice >= barAsk.Low)
                {
                    // Send to dealer
                    this.DistributionRequestToDealer(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD04";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barAsk.Low, barAsk.High), "Requote", "", "system");
                    return;
                }
            }
            else if (type == 2)
            {
                if (barBid.Low <= command.Request.OpenPrice && command.Request.OpenPrice <= barBid.High)
                {
                    //Send to dealer
                    this.DistributionRequestToDealer(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD07";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barBid.Low, barBid.High), "Requote", "", "system");
                    return;
                }
            }
        }

        internal bool CheckPriceOpenAuto(Business.RequestDealer command, int digits)
        {
            ProcessQuoteLibrary.Business.BarTick barBid = new ProcessQuoteLibrary.Business.BarTick();
            barBid = this.GetCandlesTwoM(command.Request.Symbol.Name);
            barBid.High = Math.Round(barBid.High, digits);
            barBid.Low = Math.Round(barBid.Low, digits);
            barBid.HighAsk = Math.Round(barBid.HighAsk, digits);
            barBid.LowAsk = Math.Round(barBid.LowAsk, digits);

            //double spreadDifference = command.Request.Symbol.SpreadDifference;
            double spreadDifference = command.Request.SpreaDifferenceInOpenTrade;
            double spreadDefault = 0;

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S013")
                {
                    double.TryParse(command.Request.Symbol.ParameterItems[i].NumValue, out spreadDefault);
                    break;
                }
            }

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                ProcessQuoteLibrary.Business.BarTick barAsk = new ProcessQuoteLibrary.Business.BarTick();
                barAsk = barBid;
                double spreadDiff = Business.Symbol.ConvertNumberPip(digits, spreadDifference);
                double spreadDef = Business.Symbol.ConvertNumberPip(digits, spreadDefault);
                if (command.Request.Symbol.ApplySpread)
                {
                    double high = (double)barBid.High + (double)spreadDiff + (double)spreadDef;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.Low + (double)spreadDiff + (double)spreadDef;
                    barAsk.Low = Math.Round(low, digits);
                }
                else
                {
                    double high = (double)barBid.HighAsk + (double)spreadDiff;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.LowAsk + (double)spreadDiff;
                    barAsk.Low = Math.Round(low, digits);
                }

                if (barAsk.High >= command.Request.OpenPrice && command.Request.OpenPrice >= barAsk.Low)
                {
                    return true;
                }
                command.FlagConfirm = false;
                command.Notice = "RD04";
                this.AddRequestDealerToInvestor(command);
                command.AgentCode = "system";
                command.Answer = "Requote";
                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barAsk.Low, barAsk.High), "Requote", "", "system");
                return false;
            }
            else
            {
                if (barBid.Low <= command.Request.OpenPrice && command.Request.OpenPrice <= barBid.High)
                {
                    return true;
                }
                command.FlagConfirm = false;
                command.Notice = "RD07";
                this.AddRequestDealerToInvestor(command);
                command.AgentCode = "system";
                command.Answer = "Requote";
                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barBid.Low, barBid.High), "Requote", "", "system");
                return false;
            }
        }

        internal void CheckPriceClose(Business.RequestDealer command, int digits)
        {
            ProcessQuoteLibrary.Business.BarTick barBid = new ProcessQuoteLibrary.Business.BarTick();
            barBid = this.GetCandlesTwoM(command.Request.Symbol.Name);
            barBid.High = Math.Round(barBid.High, digits);
            barBid.Low = Math.Round(barBid.Low, digits);
            barBid.HighAsk = Math.Round(barBid.HighAsk, digits);
            barBid.LowAsk = Math.Round(barBid.LowAsk, digits);

            //double spreadDifference = command.Request.Symbol.SpreadDifference;
            double spreadDifference = command.Request.SpreaDifferenceInOpenTrade;
            double spreadDefault = 0;

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S013")
                {
                    double.TryParse(command.Request.Symbol.ParameterItems[i].NumValue, out spreadDefault);
                    break;
                }
            }

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                if (barBid.Low <= command.Request.ClosePrice && command.Request.ClosePrice <= barBid.High)
                {
                    //Send to dealer
                    this.DistributionRequestToDealer(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD12";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barBid.Low, barBid.High), "Requote", "", "system");
                    return;
                }
            }
            else if (type == 2)
            {
                ProcessQuoteLibrary.Business.BarTick barAsk = new ProcessQuoteLibrary.Business.BarTick();
                barAsk = barBid;
                double spreadDiff = Business.Symbol.ConvertNumberPip(digits, spreadDifference);
                double spreadDef = Business.Symbol.ConvertNumberPip(digits, spreadDefault);
                if (command.Request.Symbol.ApplySpread)
                {
                    double high = (double)barBid.High + (double)spreadDiff + (double)spreadDef;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.Low + (double)spreadDiff + (double)spreadDef;
                    barAsk.Low = Math.Round(low, digits);
                }
                else
                {
                    double high = (double)barBid.HighAsk + (double)spreadDiff;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.LowAsk + (double)spreadDiff;
                    barAsk.Low = Math.Round(low, digits);
                }

                if (barAsk.High >= command.Request.ClosePrice && command.Request.ClosePrice >= barAsk.Low)
                {
                    //Send to dealer
                    this.DistributionRequestToDealer(command);
                    return;
                }
                else
                {
                    command.FlagConfirm = false;
                    command.Notice = "RD15";
                    this.AddRequestDealerToInvestor(command);
                    command.AgentCode = "system";
                    command.Answer = "Requote";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barAsk.Low, barAsk.High), "Requote", "", "system");
                    return;
                }
            }
        }

        internal bool CheckPriceCloseAuto(Business.RequestDealer command, int digits)
        {
            ProcessQuoteLibrary.Business.BarTick barBid = new ProcessQuoteLibrary.Business.BarTick();
            barBid = this.GetCandlesTwoM(command.Request.Symbol.Name);
            barBid.High = Math.Round(barBid.High, digits);
            barBid.Low = Math.Round(barBid.Low, digits);
            barBid.HighAsk = Math.Round(barBid.HighAsk, digits);
            barBid.LowAsk = Math.Round(barBid.LowAsk, digits);

            //double spreadDifference = command.Request.Symbol.SpreadDifference;
            double spreadDifference = command.Request.SpreaDifferenceInOpenTrade;
            double spreadDefault = 0;

            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S013")
                {
                    double.TryParse(command.Request.Symbol.ParameterItems[i].NumValue, out spreadDefault);
                    break;
                }
            }

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            if (type == 1)
            {
                if (barBid.Low <= command.Request.ClosePrice && command.Request.ClosePrice <= barBid.High)
                {
                    return true;
                }
                command.FlagConfirm = false;
                command.Notice = "RD12";
                this.AddRequestDealerToInvestor(command);
                command.AgentCode = "system";
                command.Answer = "Requote";
                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barBid.Low, barBid.High), "Requote", "", "system");
                return false;
            }
            else 
            {
                ProcessQuoteLibrary.Business.BarTick barAsk = new ProcessQuoteLibrary.Business.BarTick();
                barAsk = barBid;
                double spreadDiff = Business.Symbol.ConvertNumberPip(digits, spreadDifference);
                double spreadDef = Business.Symbol.ConvertNumberPip(digits, spreadDefault);
                if (command.Request.Symbol.ApplySpread)
                {
                    double high = (double)barBid.High + (double)spreadDiff + (double)spreadDef;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.Low + (double)spreadDiff + (double)spreadDef;
                    barAsk.Low = Math.Round(low, digits);
                }
                else
                {
                    double high = (double)barBid.HighAsk + (double)spreadDiff;
                    barAsk.High = Math.Round(high, digits);
                    double low = (double)barBid.LowAsk + (double)spreadDiff;
                    barAsk.Low = Math.Round(low, digits);
                }

                if (barAsk.High >= command.Request.ClosePrice && command.Request.ClosePrice >= barAsk.Low)
                {
                    return true;
                }
                command.FlagConfirm = false;
                command.Notice = "RD15";
                this.AddRequestDealerToInvestor(command);
                command.AgentCode = "system";
                command.Answer = "Requote";
                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': system check price requote " + command.LogRequestSuper(barAsk.Low, barAsk.High), "Requote", "", "system");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDealer"></param>
        internal void AddRequestDealerToInvestor(Business.RequestDealer requestDealer)
        {
            StringBuilder result = new StringBuilder();
            int type = Business.Symbol.ConvertCommandIsBuySell(requestDealer.Request.Type.ID);
            switch (requestDealer.Name.ToLower())
            {
                case "open":
                    {
                        result.Append("AddCommand$");
                        break;
                    }
                case "close":
                    {
                        result.Append("CloseCommand$");
                        break;
                    }
                case "update":
                    {
                        result.Append("UpdateCommand$");
                        break;
                    }
                case "updatepending":
                    {
                        result.Append("UpdateCommand$");
                        break;
                    }
                case "closepending":
                    {
                        result.Append("CloseCommand$");
                        break;
                    }
                case "openpending":
                    {
                        result.Append("AddCommand$");
                        break;
                    }
            }

            #region OpenTrade
            result.Append(requestDealer.FlagConfirm);
            result.Append(",");
            result.Append(requestDealer.Notice);
            result.Append(",");
            result.Append(requestDealer.Request.ID);
            result.Append(",");
            result.Append(requestDealer.Request.Investor.InvestorID);
            result.Append(",");
            result.Append(requestDealer.Request.Symbol.Name);
            result.Append(",");
            result.Append(requestDealer.Request.Size);
            if (type == 1) result.Append(",true,");
            else result.Append(",false,");
            result.Append(requestDealer.Request.OpenTime);
            result.Append(",");
            result.Append(requestDealer.Request.OpenPrice);
            result.Append(",");
            result.Append(requestDealer.Request.StopLoss);
            result.Append(",");
            result.Append(requestDealer.Request.TakeProfit);
            result.Append(",");
            result.Append(requestDealer.Request.ClosePrice);
            result.Append(",");
            result.Append(requestDealer.Request.Commission);
            result.Append(",");
            result.Append(requestDealer.Request.Swap);
            result.Append(",");
            result.Append(requestDealer.Request.Profit);
            result.Append(",Comment,");
            result.Append(requestDealer.Request.CommandCode);
            result.Append(",");
            result.Append(requestDealer.Request.Type.Name);
            result.Append(",1,");
            result.Append(requestDealer.Request.ExpTime);
            result.Append(",");
            result.Append(requestDealer.Request.ClientCode);
            result.Append(",");
            result.Append(requestDealer.Request.CommandCode);
            result.Append(",");
            result.Append(requestDealer.Request.IsHedged);
            result.Append(",");
            result.Append(requestDealer.Request.Type.ID);
            result.Append(",");
            result.Append(requestDealer.Request.Margin);
            result.Append(",");
            result.Append(requestDealer.Name);
            result.Append(",");
            result.Append(requestDealer.Request.IsMultiClose);

            #endregion

            if (requestDealer.Request.Investor.ClientCommandQueue == null) requestDealer.Request.Investor.ClientCommandQueue = new List<string>();
            requestDealer.Request.Investor.ClientCommandQueue.Add(result.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="lotRequest"></param>
        /// <returns></returns>
        internal bool CheckDealerLots(double min, double max, double lotRequest)
        {
            if (min == max)
            {
                return false;
            }
            if (min <= lotRequest && max >= lotRequest)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// modeExOfGroup != "manual- but automatic if no dealer online" IsBusy set true
        /// </summary>
        /// <param name="command"></param>
        /// <param name="modeExOfGroup"></param>
        /// <returns></returns>
        internal Agent FindDealerProcessRequest(Business.RequestDealer command)
        {
            if (Market.AgentList.Count == 0)
            {
                return null;
            }
            string modeSymbol = "";
            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S006")
                {
                    modeSymbol = command.Request.Symbol.ParameterItems[i].StringValue;
                    break;
                }
            }

            int securityID = command.Request.Symbol.SecurityID;
            List<Business.Agent> result = new List<Agent>();
            List<Business.Agent> result1 = new List<Agent>();
            bool sameGroup = false;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer)
                {
                    #region VD
                    if (Market.AgentList[i].VirtualDealer.IsEnable)
                    {
                        //Check Agent had manager Group
                        sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].VirtualDealer.IVirtualDealer, command.Request.Investor.InvestorGroupInstance.InvestorGroupID);
                        if (sameGroup)
                        {
                            for (int isb = Market.AgentList[i].VirtualDealer.IVirtualDealer.Count - 1; isb >= 0; isb--)
                            {
                                if (Market.AgentList[i].VirtualDealer.IVirtualDealer[isb].SymbolID != -1)
                                {
                                    if (Market.AgentList[i].VirtualDealer.IVirtualDealer[isb].SymbolID == command.Request.Symbol.SymbolID)
                                    {
                                        //Check Range Lost Manager
                                        if (CheckDealerLots(Market.AgentList[i].VirtualDealer.StartVolume, Market.AgentList[i].VirtualDealer.EndVolume, command.Request.Size))
                                        {
                                            if (modeSymbol == "Instant" && Market.AgentList[i].VirtualDealer.Mode == 0)
                                            {
                                                result1.Add(Market.AgentList[i]);
                                                break;
                                            }
                                            if ((modeSymbol == "Request" || modeSymbol == "Market") && Market.AgentList[i].VirtualDealer.Mode == 1)
                                            {
                                                result1.Add(Market.AgentList[i]);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Dealer
                    for (int ia = Market.AgentList[i].IAgentSecurity.Count - 1; ia >= 0; ia--)
                    {
                        if (Market.AgentList[i].IsBusy == false & Market.AgentList[i].IsDealer == true)
                        {
                            //Check Agent had manager Group
                            sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].IAgentGroup, command.Request.Investor.InvestorGroupInstance.InvestorGroupID);
                            if (sameGroup)
                            {
                                // Same Security and have Use
                                if (Market.AgentList[i].IAgentSecurity[ia].SecurityID == securityID && Market.AgentList[i].IAgentSecurity[ia].Use)
                                {
                                    //Check Range Lost Manager
                                    if (CheckDealerLots(Market.AgentList[i].IAgentSecurity[ia].MinLots, Market.AgentList[i].IAgentSecurity[ia].MaxLots, command.Request.Size))
                                    {
                                        result.Add(Market.AgentList[i]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

            }
            if (result.Count == 0 && result1.Count == 0)
            {
                return null;
            }

            if (result1.Count > 0)
            {
                if (result1.Count == 1)
                {
                    return result1[0];
                }
                Random number = new Random();
                int randnum = number.Next(result1.Count);
                return result1[randnum];
            }
            else
            {
                if (result.Count == 1)
                {
                    return result[0];
                }

                Random number = new Random();
                int randnum = number.Next(result.Count);
                return result[randnum];
            }
        }

        internal void FindDealerActivePendingRequest(Business.OpenTrade command)
        {
            //string content = string.Empty;
            //content = "'" + "system" + ": '" + " active #" + command.CommandCode + " trigging..";
            //TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order trigging]", "", command.Investor.Code);
            if (Business.Market.AgentList == null)
                return;

            if (Market.AgentList.Count == 0)
                return;

            List<Business.Agent> result1 = new List<Agent>();
            bool sameGroup = false;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer)
                {
                    #region VD
                    if (Market.AgentList[i].VirtualDealer.IsEnable)
                    {
                        //Check Agent had manager Group
                        sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].VirtualDealer.IVirtualDealer, command.Investor.InvestorGroupInstance.InvestorGroupID);
                        if (sameGroup)
                        {
                            for (int isb = Market.AgentList[i].VirtualDealer.IVirtualDealer.Count - 1; isb >= 0; isb--)
                            {
                                if (Market.AgentList[i].VirtualDealer.IVirtualDealer[isb].SymbolID != -1)
                                {
                                    if (Market.AgentList[i].VirtualDealer.IVirtualDealer[isb].SymbolID == command.Symbol.SymbolID)
                                    {
                                        //Check Range Lost Manager
                                        if (CheckDealerLots(Market.AgentList[i].VirtualDealer.StartVolume, Market.AgentList[i].VirtualDealer.EndVolume, command.Size))
                                        {
                                            if (Market.AgentList[i].VirtualDealer.Mode == 2)
                                            {
                                                result1.Add(Market.AgentList[i]);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            if (result1.Count == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < result1.Count; i++)
                {
                    switch (command.Type.ID)
                    {
                        #region Case Buy Limit Command
                        case 7:
                            {
                                if (result1[i].VirtualDealer.IsLimitAuto)
                                {
                                    int count = command.Symbol.MarketAreaRef.Type.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (command.Symbol.MarketAreaRef.Type[j].ID == 1)
                                        {
                                            command.Type = new TradeType();
                                            command.Type.ID = command.Symbol.MarketAreaRef.Type[j].ID;
                                            command.Type.Name = command.Symbol.MarketAreaRef.Type[j].Name;

                                            command.OpenTime = DateTime.Now;
                                            //SYSTEM LOG ACTIVATE PENDING ORDER 
                                            string content = string.Empty;
                                            content = "'" + result1[i].Code + ": '" + "pending buy limit order #" + command.CommandCode + " triggered";
                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", command.Investor.Code);
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Case Sell Limit Command
                        case 8:
                            {
                                if (result1[i].VirtualDealer.IsLimitAuto)
                                {
                                    int count = command.Symbol.MarketAreaRef.Type.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (command.Symbol.MarketAreaRef.Type[j].ID == 2)
                                        {
                                            command.Type = new TradeType();
                                            command.Type.ID = command.Symbol.MarketAreaRef.Type[j].ID;
                                            command.Type.Name = command.Symbol.MarketAreaRef.Type[j].Name;

                                            command.OpenTime = DateTime.Now;
                                            //SYSTEM LOG ACTIVATE PENDING ORDER 
                                            string content = string.Empty;
                                            content = "'" + result1[i].Code + ": '" + "pending sell limit order #" + command.CommandCode + " triggered";
                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", command.Investor.Code);
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Case Buy Stop Command
                        case 9:
                            {
                                if (result1[i].VirtualDealer.IsStopAuto)
                                {
                                    int count = command.Symbol.MarketAreaRef.Type.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (command.Symbol.MarketAreaRef.Type[j].ID == 1)
                                        {
                                            command.Type = new TradeType();
                                            command.Type.ID = command.Symbol.MarketAreaRef.Type[j].ID;
                                            command.Type.Name = command.Symbol.MarketAreaRef.Type[j].Name;

                                            command.OpenTime = DateTime.Now;
                                            if (result1[i].VirtualDealer.IsStopSlippage)
                                            {
                                                command.OpenPrice = command.ClosePrice;
                                            }
                                            //SYSTEM LOG ACTIVATE PENDING ORDER 
                                            string content = string.Empty;
                                            content = "'" + result1[i].Code + ": '" + "pending buy stop order #" + command.CommandCode + " triggered";
                                            TradingServer.Facade.FacadeAddNewSystemLog(1, content, "[pending order triggered]", "", command.Investor.Code);
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Case Sell Stop Command
                        case 10:
                            {
                                if (result1[i].VirtualDealer.IsStopAuto)
                                {
                                    int count = command.Symbol.MarketAreaRef.Type.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (command.Symbol.MarketAreaRef.Type[j].ID == 2)
                                        {
                                            command.Type = new TradeType();
                                            command.Type.ID = command.Symbol.MarketAreaRef.Type[j].ID;
                                            command.Type.Name = command.Symbol.MarketAreaRef.Type[j].Name;

                                            command.OpenTime = DateTime.Now;
                                            if (result1[i].VirtualDealer.IsStopSlippage)
                                            {
                                                command.OpenPrice = command.ClosePrice;
                                            }
                                            //SYSTEM LOG ACTIVATE PENDING ORDER 
                                            string content = string.Empty;
                                            content = "'" + result1[i].Code + ": '" + "pending sell stop order #" + command.CommandCode + " triggered";
                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", command.Investor.Code);

                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                    }
                }
            }
        }
        #region Notice Manager
        /// <summary>
        /// mode = 1 is Open or Update , mode = 2 is Close
        /// </summary>
        /// <param name="command"></param>
        internal void SendNoticeManagerRequest(int mode, Business.OpenTrade command)
        {
            if (Business.Market.AgentList == null)
                return;

            if (Market.AgentList.Count == 0)
                return;

            Business.Agent result = new Agent();
            bool sameGroup = false;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false)
                {
                    //Check Agent had manager Group
                    sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].IAgentGroup, command.Investor.InvestorGroupInstance.InvestorGroupID);
                    if (sameGroup)
                    {
                        if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                        {
                            if (mode == 1)
                            {
                                NoticeDealer[Market.AgentList[i].AgentID] += "NA05:" + command.ID + ",";
                            }
                            else
                            {
                                NoticeDealer[Market.AgentList[i].AgentID] += "NA06:" + command.ID + "{" + command.CloseTime.Ticks + "{" + command.ClosePrice + "{" + command.Profit + ",";
                            }
                        }
                        else
                        {
                            if (mode == 1)
                            {
                                NoticeDealer.Add(Market.AgentList[i].AgentID, "NA05:" + command.ID + ",");
                            }
                            else
                            {
                                NoticeDealer.Add(Market.AgentList[i].AgentID, "NA06:" + command.ID + "{"+ command.CloseTime.Ticks + "{" + command.ClosePrice + "{" + command.Profit + ",");
                            }
                        }
                    }
                }
            }
        }

        internal void SendNoticeManagerDealerRequest(Business.RequestDealer requestDealer)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            Business.Agent result = new Agent();
            bool sameGroup = false;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false && Market.AgentList[i].IsBusy == false)
                {
                    //Check Agent had manager Group
                    sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].IAgentGroup, requestDealer.Request.Investor.InvestorGroupInstance.InvestorGroupID);
                    if (sameGroup)
                    {
                        if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                        {
                            NoticeDealer[Market.AgentList[i].AgentID] += "NA25{" + requestDealer.RequestString + ",";
                        }
                        else
                        {
                            NoticeDealer.Add(Market.AgentList[i].AgentID, "NA25{" + requestDealer.RequestString + ",");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// mode = 1 is Login, mode = 2 is Logout,  mode = 3 is Balance or Credit
        /// </summary>
        /// <param name="command"></param>
        internal void SendNoticeManagerRequest(int mode, Business.Investor investor)
        {
            if (Market.AgentList == null)
                return;

            if (Market.AgentList.Count == 0)
                return;

            Business.Agent result = new Agent();
            bool sameGroup = false;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false)
                {
                    //Check Agent had manager Group
                    sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].IAgentGroup, investor.InvestorGroupInstance.InvestorGroupID);
                    if (sameGroup)
                    {
                        if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA07:" + investor.Code + ",";
                                    break;
                                case 2:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA08:" + investor.Code + ",";
                                    break;
                                case 3:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA09:" + investor.Code + ":" + investor.InvestorID + ",";
                                    break;
                            }

                        }
                        else
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA07:" + investor.Code + ",");
                                    break;
                                case 2:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA08:" + investor.Code + ",");
                                    break;
                                case 3:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA09:" + investor.Code + ":" + investor.InvestorID + ",");
                                    break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// mode = 1 is update group , mode = 2 is update group config
        /// mode = 3 is add group, mode = 4 is delete group
        /// </summary>
        /// <param name="command"></param>
        internal void SendNoticeManagerChangeGroup(int mode, int groupID)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            Business.Agent result = new Agent();
            bool sameGroup = false;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false)
                {
                    //Check Agent had manager Group
                    sameGroup = CheckInvestorBelongGroup(Market.AgentList[i].IAgentGroup, groupID);
                    if (sameGroup)
                    {
                        if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA10:" + groupID + ",";
                                    break;
                                case 2:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA11:" + groupID + ",";
                                    break;
                                case 3:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA12:" + groupID + ",";
                                    break;
                                case 4:
                                    NoticeDealer[Market.AgentList[i].AgentID] += "NA13:" + groupID + ",";
                                    break;
                            }
                        }
                        else
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA10:" + groupID + ",");
                                    break;
                                case 2:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA11:" + groupID + ",");
                                    break;
                                case 3:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA12:" + groupID + ",");
                                    break;
                                case 4:
                                    NoticeDealer.Add(Market.AgentList[i].AgentID, "NA13:" + groupID + ",");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// mode = 1 is update symbol , mode = 2 is update symbol config
        /// mode = 3 is add symbol, mode = 4 is delete symbol
        /// </summary>
        /// <param name="command"></param>
        internal void SendNoticeManagerChangeSymbol(int mode, int symbolID)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            Business.Agent result = new Agent();
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false)
                {
                    if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                    {
                        switch (mode)
                        {
                            case 1:
                                NoticeDealer[Market.AgentList[i].AgentID] += "NA14:" + symbolID + ",";
                                break;
                            case 2:
                                NoticeDealer[Market.AgentList[i].AgentID] += "NA15:" + symbolID + ",";
                                break;
                            case 3:
                                NoticeDealer[Market.AgentList[i].AgentID] += "NA16:" + symbolID + ",";
                                break;
                            case 4:
                                NoticeDealer[Market.AgentList[i].AgentID] += "NA17:" + symbolID + ",";
                                break;
                        }
                    }
                    else
                    {
                        switch (mode)
                        {
                            case 1:
                                NoticeDealer.Add(Market.AgentList[i].AgentID, "NA14:" + symbolID + ",");
                                break;
                            case 2:
                                NoticeDealer.Add(Market.AgentList[i].AgentID, "NA15:" + symbolID + ",");
                                break;
                            case 3:
                                NoticeDealer.Add(Market.AgentList[i].AgentID, "NA16:" + symbolID + ",");
                                break;
                            case 4:
                                NoticeDealer.Add(Market.AgentList[i].AgentID, "NA17:" + symbolID + ",");
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="mailID"></param>
        /// <param name="agentID"></param>
        internal void SendNoticeManagerChangeMail(int mode, int mailID, int agentID)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            if (NoticeDealer.ContainsKey(agentID))
            {
                switch (mode)
                {
                    case 1:
                        NoticeDealer[agentID] += "NA23:" + mailID + ",";
                        break;
                }
            }
            else
            {
                switch (mode)
                {
                    case 1:
                        NoticeDealer.Add(agentID, "NA23:" + mailID + ",");
                        break;

                }
            }
        }

        /// <summary>
        /// mode = 1 is update Agent, mode = 2 update IAgentGroup, mode = 3 update Permit
        /// </summary>
        /// <param name="command"></param>
        internal void SendNoticeManagerChangeAgent(int mode, int agentID)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }

            Business.Agent result = new Agent();
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false)
                {
                    if (Market.AgentList[i].AgentID == agentID)
                    {
                        if (NoticeDealer.ContainsKey(agentID))
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer[agentID] += "NA18,";
                                    break;
                                case 2:
                                    NoticeDealer[agentID] += "NA19,";
                                    break;
                                case 3:
                                    NoticeDealer[agentID] += "NA20,";
                                    break;
                            }
                        }
                        else
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer.Add(agentID, "NA18,");
                                    break;
                                case 2:
                                    NoticeDealer.Add(agentID, "NA19,");
                                    break;
                                case 3:
                                    NoticeDealer.Add(agentID, "NA20,");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// mode = 1 is delete alert
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="alert"></param>
        internal void SendNoticeManagerChangeAlert(int mode, Business.PriceAlert alert)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            Business.Agent result = new Agent();
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false)
                {
                    if (Market.AgentList[i].InvestorID == alert.InvestorID)
                    {
                        int agentID = Market.AgentList[i].AgentID;
                        if (NoticeDealer.ContainsKey(agentID))
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer[agentID] += "NA21:" + alert.ID + ",";
                                    break;
                            }
                        }
                        else
                        {
                            switch (mode)
                            {
                                case 1:
                                    NoticeDealer.Add(agentID, "NA21:" + alert.ID + ",");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        internal void SendNoticeManagerTick(Business.Tick tick)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            string tickString = tick.Bid + "}" + tick.Ask + "}" + tick.SymbolName + "}" + tick.TickTime.Ticks + "}"
                + tick.Status + "}" + tick.HighInDay + "}" + tick.LowInDay;

            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false && Market.AgentList[i].IsOnline == true)
                {
                    if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                    {
                        NoticeDealer[Market.AgentList[i].AgentID] += "NA26{" + tickString + ",";
                    }
                    else
                    {
                        NoticeDealer.Add(Market.AgentList[i].AgentID, "NA26{" + tickString + ",");
                    }

                }
            }
        }

        internal void SendNoticeManagerOnline(Business.Agent agent)
        {
            if (Market.AgentList.Count == 0)
            {
                return;
            }
            string agentString = agent.AgentID + "}" + agent.Code + "}" + agent.IsOnline + "}" + agent.IsBusy;

            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer == false && Market.AgentList[i].IsOnline == true)
                {
                    if (NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                    {
                        NoticeDealer[Market.AgentList[i].AgentID] += "NA30{" + agentString + ",";
                    }
                    else
                    {
                        NoticeDealer.Add(Market.AgentList[i].AgentID, "NA30{" + agentString + ",");
                    }

                }
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iAgentGroup"></param>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        internal bool CheckInvestorBelongGroup(List<IAgentGroup> iAgentGroup, int investorGroupID)
        {
            if (iAgentGroup == null | iAgentGroup.Count == 0) return false;
            for (int i = iAgentGroup.Count - 1; i >= 0; i--)
            {
                if (iAgentGroup[i].InvestorGroupID == investorGroupID) return true;
            }
            return false;
        }

        internal bool CheckInvestorBelongGroup(List<Business.IVirtualDealer> iVirtualDealers, int investorGroupID)
        {
            for (int i = iVirtualDealers.Count - 1; i >= 0; i--)
            {
                if (iVirtualDealers[i].InvestorGroupID != -1)
                {
                    if (iVirtualDealers[i].InvestorGroupID == investorGroupID) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listAgentOnline"></param>
        /// <param name="request"></param>
        internal void DistributionRequestToDealer(Business.RequestDealer command)
        {
            Agent AgentProcess = FindDealerProcessRequest(command);
            if (AgentProcess == null)
            {
                command.Notice = "RD23";
                command.FlagConfirm = false;
                this.AddRequestDealerToInvestor(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': trade context is busy " + command.LogRequest, "Context Busy", "", "system");
                return;
            }
            if (AgentProcess.AgentID == 0)
            {
                command.Notice = "RD23";
                command.FlagConfirm = false;
                this.AddRequestDealerToInvestor(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code + "': trade context is busy " + command.LogRequest, "Context Busy", "", "system");
                return;
            }
            else
            {
                command.AgentID = AgentProcess.AgentID;
                command.TimeAgentReceive = DateTime.Now;
                command.FlagConfirm = false;
                Market.ListRequestDealer.Add(command);
                TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': request from '" + command.Request.Investor.Code
                           + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);

                if (!AgentProcess.IsVirtualDealer)
                {
                    if (NoticeDealer.ContainsKey(AgentProcess.AgentID))
                    {
                        NoticeDealer[AgentProcess.AgentID] += "NA01,";
                    }
                    else
                    {
                        NoticeDealer.Add(AgentProcess.AgentID, "NA01,");
                    }
                }
                else
                {
                    AgentProcess.VirtualRequestDealers.Add(command);
                }
                command.AgentCode = AgentProcess.Code;
                command.Answer = "Received";
                TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
            }
        }

        /// <summary>
        /// Get List Request
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal List<RequestDealer> GetRequestToDealer(int agentID)
        {
            if (Market.ListRequestDealer == null | Market.ListRequestDealer.Count == 0) return null;
            List<RequestDealer> result = new List<RequestDealer>();
            string codeInvestor = "";
            this.WaitAccessListRequest();
            try
            {
                for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                {
                    // Need add FlagComfirm
                    if (Market.ListRequestDealer[i].AgentID == agentID && Market.ListRequestDealer[i].FlagConfirm == false)
                    {
                        Market.ListRequestDealer[i].TimeAgentReceive = DateTime.Now;
                        codeInvestor += "R_" + Market.ListRequestDealer[i].InvestorID + "_";
                        result.Add(Market.ListRequestDealer[i]);
                    }
                }
                this.SetStatusDealer(agentID);
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Get request to Dealer Invetor_" + codeInvestor + "Agent_" + agentID, ex.Message, "123", "");
            }
            IsBusyListRequest = false;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        internal void AddListQueueCompare(RequestDealer command)
        {
            switch (command.Name)
            {
                case "Open":
                    for (int i = Market.ListQueueCompare.Count - 1; i >= 0; i--)
                    {
                        if (Market.ListQueueCompare[i].Request.ClientCode == command.Request.ClientCode && Market.ListQueueCompare[i].InvestorID == command.InvestorID)
                        {
                            if (Market.ListQueueCompare[i].Request.OpenPrice == command.Request.OpenPrice)
                            {
                                command.Notice = "RD17";
                                command.FlagConfirm = true;
                                //this.AddRequestDealerToInvestor(command);
                                command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                Market.ListQueueCompare.RemoveAt(i);
                                return;
                            }
                            else
                            {
                                // Check Again
                                double change = Math.Abs(Market.ListQueueCompare[i].Request.OpenPrice - command.Request.OpenPrice);
                                double pipMaxDevClient = Business.Symbol.ConvertNumberPip(command.Request.Symbol.Digit, command.MaxDev);
                                if (change > pipMaxDevClient)
                                {
                                    command.Notice = "RD18";
                                }
                                else
                                {
                                    command.Notice = "RD25";
                                }
                                command.FlagConfirm = false;
                                this.AddRequestDealerToInvestor(command);
                                Market.ListQueueCompare[i].Request.OpenPrice = command.Request.OpenPrice;
                                return;
                            }
                        }
                    }
                    break;
                case "Close":
                    for (int i = Market.ListQueueCompare.Count - 1; i >= 0; i--)
                    {
                        if (Market.ListQueueCompare[i].Request.ClientCode == command.Request.ClientCode && Market.ListQueueCompare[i].InvestorID == command.InvestorID)
                        {
                            if (Market.ListQueueCompare[i].Request.ClosePrice == command.Request.ClosePrice)
                            {
                                command.Notice = "RD19";
                                command.FlagConfirm = true;
                                //this.AddRequestDealerToInvestor(command);
                                command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                Market.ListQueueCompare.RemoveAt(i);
                                return;
                            }
                            else
                            {
                                //Check Again
                                double change = Math.Abs(Market.ListQueueCompare[i].Request.ClosePrice - command.Request.ClosePrice);
                                double pipMaxDevClient = Business.Symbol.ConvertNumberPip(command.Request.Symbol.Digit, command.MaxDev);
                                if (change > pipMaxDevClient)
                                {
                                    command.Notice = "RD20";
                                }
                                else
                                {
                                    command.Notice = "RD26";
                                }
                                command.FlagConfirm = false;
                                this.AddRequestDealerToInvestor(command);
                                Market.ListQueueCompare[i].Request.ClosePrice = command.Request.ClosePrice;
                                return;
                            }
                        }
                    }
                    break;
            }
            command.Notice = "RD32";
            command.FlagConfirm = false;
            this.AddRequestDealerToInvestor(command);
            Market.ListQueueCompare.Add(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        internal void RemoveListQueueCompare(RequestDealer command)
        {
            for (int i = Market.ListQueueCompare.Count - 1; i >= 0; i--)
            {
                if (Market.ListQueueCompare[i].Request.ClientCode == command.Request.ClientCode && Market.ListQueueCompare[i].InvestorID == command.InvestorID)
                {
                    Market.ListQueueCompare.RemoveAt(i);
                    return;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listAgentOnline"></param>
        /// <param name="agent"></param>
        /// <param name="isBusy"></param>
        internal void SetStatusDealer(int agent)
        {
            if (Market.AgentList.Count == 0) return;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].AgentID == agent)
                {
                    Market.AgentList[i].TimeSync = DateTime.Now;
                    return;
                }
            }
        }

        internal bool CheckPermitAccessRequest(Agent agent, RequestDealer command)
        {
            int securityID = command.Request.Symbol.SecurityID;
            for (int ia = agent.IAgentSecurity.Count - 1; ia >= 0; ia--)
            {
                if (agent.IsBusy == false & agent.IsDealer == true)
                {
                    bool sameGroup = CheckInvestorBelongGroup(agent.IAgentGroup, command.Request.Investor.InvestorGroupInstance.InvestorGroupID);
                    if (sameGroup)
                    {
                        // Same Security and have Use
                        if (agent.IAgentSecurity[ia].SecurityID == securityID && agent.IAgentSecurity[ia].Use)
                        {
                            //Check Range Lost Manager
                            if (CheckDealerLots(agent.IAgentSecurity[ia].MinLots, agent.IAgentSecurity[ia].MaxLots, command.Request.Size))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        internal bool CheckPermitAccessGroupManagerAndAdmin(string agentCode, int investorGroupID)
        {
            Agent agent = null;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (agentCode == Market.AgentList[i].Code)
                {
                    agent = Market.AgentList[i];
                    break;
                }
            }
            if (agent == null)
            {
                for (int i = Market.AdminList.Count - 1; i >= 0; i--)
                {
                    if (agentCode == Market.AdminList[i].Code)
                    {
                        agent = Market.AdminList[i];
                        break;
                    }
                }
            }
            if (agent != null)
            {
                bool sameGroup = CheckInvestorBelongGroup(agent.IAgentGroup, investorGroupID);
                if (sameGroup)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool CheckPermitDownloadStatements(string agentCode)
        {
            Agent agent = null;
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (agentCode == Market.AgentList[i].Code)
                {
                    agent = Market.AgentList[i];
                    break;
                }
            }
            if (agent == null)
            {
                for (int i = Market.AdminList.Count - 1; i >= 0; i--)
                {
                    if (agentCode == Market.AdminList[i].Code)
                    {
                        agent = Market.AdminList[i];
                        break;
                    }
                }
            }
            if (agent != null)
            {
                if (agent.IsDownloadStatements)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        internal void DealerCommandConfirm(RequestDealer command)
        {

            #region Open & Close Order
            if (Market.ListRequestDealer == null | Market.ListRequestDealer.Count == 0) return;
            this.WaitAccessListRequest();
            try
            {
                Agent AgentProcess = this.FindAgentOnlineByID(command.AgentID);
                if (AgentProcess != null)
                {
                    bool check = this.CheckPermitAccessRequest(AgentProcess, command);
                    if (check)
                    {
                        Business.Tick onlineTick = new Tick();
                        int digits = command.Request.Symbol.Digit;
                        double Pip = command.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
                        onlineTick.Ask = Math.Round((command.Request.Symbol.TickValue.Ask + Pip), digits);
                        onlineTick.Bid = command.Request.Symbol.TickValue.Bid;

                        string modeExOfSymbol = "";
                        for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
                        {
                            if (command.Request.Symbol.ParameterItems[i].Code == "S006")
                            {
                                modeExOfSymbol = command.Request.Symbol.ParameterItems[i].StringValue;
                                break;
                            }
                        }

                        switch (modeExOfSymbol)
                        {
                            #region Request
                            case "Request":
                                for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                                {
                                    if (Market.ListRequestDealer[i].AgentID == command.AgentID && Market.ListRequestDealer[i].Name == command.Name &&
                                        Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                    {
                                        switch (command.Name.ToLower())
                                        {
                                            case "update":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                    Market.ListRequestDealer.RemoveAt(i);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "updatepending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                    Market.ListRequestDealer.RemoveAt(i);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "closepending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                                    Market.ListRequestDealer.RemoveAt(i);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "openpending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                                    Market.ListRequestDealer.RemoveAt(i);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "open":
                                                {
                                                    command.Notice = "RD28";
                                                    command.FlagConfirm = false;
                                                    this.AddRequestDealerToInvestor(command);
                                                    Market.ListRequestDealer[i].Request.OpenPrice = command.Request.OpenPrice;
                                                    // Set FlagConfirm
                                                    Market.ListRequestDealer[i].FlagConfirm = true;
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': quotes '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Quotes";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }

                                                    break;
                                                }
                                            case "close":
                                                {
                                                    command.Notice = "RD28";
                                                    command.FlagConfirm = false;
                                                    this.AddRequestDealerToInvestor(command);
                                                    Market.ListRequestDealer[i].Request.ClosePrice = command.Request.ClosePrice;
                                                    // Set FlagConfirm
                                                    Market.ListRequestDealer[i].FlagConfirm = true;
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': quotes '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Quotes";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                }
                                break;
                            #endregion

                            #region Market
                            case "Market":
                                for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                                {
                                    if (Market.ListRequestDealer[i].AgentID == command.AgentID && Market.ListRequestDealer[i].Name == command.Name &&
                                        Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                    {
                                        switch (command.Name.ToLower())
                                        {
                                            case "update":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                    }
                                                    break;
                                                }
                                            case "updatepending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                    }
                                                    break;
                                                }
                                            case "closepending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                    }
                                                    break;
                                                }
                                            case "openpending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                    }
                                                    break;
                                                }
                                            case "open":
                                                command.Notice = "RD27";
                                                command.FlagConfirm = true;
                                                command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                                if (AgentProcess != null)
                                                {
                                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                  + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                }
                                                break;
                                            case "close":
                                                command.Notice = "RD31";
                                                command.FlagConfirm = true;
                                                command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                                if (AgentProcess != null)
                                                {
                                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                  + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                }
                                                break;
                                        }
                                        command.AgentCode = AgentProcess.Code;
                                        command.Answer = "Confirm";
                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                        Market.ListRequestDealer.RemoveAt(i);
                                        break;
                                    }
                                }
                                break;
                            #endregion

                            #region Instant
                            case "Instant":
                                for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                                {
                                    if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].AgentID == command.AgentID &&
                                        Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                    {
                                        switch (command.Name.ToLower())
                                        {
                                            case "update":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "updatepending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "closepending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "openpending":
                                                {
                                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                        + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                    break;
                                                }
                                            case "open":
                                                if (Market.ListRequestDealer[i].Request.OpenPrice == command.Request.OpenPrice)
                                                {
                                                    command.Notice = "RD21";
                                                    command.FlagConfirm = true;
                                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                                    //this.AddRequestDealerToInvestor(command);
                                                    this.RemoveListQueueCompare(command);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                       + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                }
                                                else
                                                {
                                                    this.AddListQueueCompare(command);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': quotes '" + command.Request.Investor.Code
                                                      + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Quotes";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                }
                                                break;

                                            case "close":
                                                if (Market.ListRequestDealer[i].Request.ClosePrice == command.Request.ClosePrice)
                                                {
                                                    command.Notice = "RD22";
                                                    command.FlagConfirm = true;
                                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                                    //this.AddRequestDealerToInvestor(command);
                                                    this.RemoveListQueueCompare(command);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': confirm '" + command.Request.Investor.Code
                                                       + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Confirm";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                }
                                                else
                                                {
                                                    this.AddListQueueCompare(command);
                                                    if (AgentProcess != null)
                                                    {
                                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': quotes '" + command.Request.Investor.Code
                                                      + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);
                                                        command.AgentCode = AgentProcess.Code;
                                                        command.Answer = "Quotes";
                                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                                    }
                                                }
                                                break;
                                        }
                                        //Remove ListRequestDealer
                                        Market.ListRequestDealer.RemoveAt(i);
                                        break;
                                    }
                                }
                                break;
                            #endregion

                        }
                    }
                    else
                    {
                        this.ReturnRequest(command);
                    }
                }
                else
                {
                    this.ReturnRequest(command);
                    TradingServer.Facade.FacadeAddNewSystemLog(1, "'" + code + "': dealer confirm action not taken (not enough rights)'" + command.Request.Investor.Code
                                                      + "' " + command.Name.ToLower() + " " + Facade.FacadeGetTypeCommand(command.Request.Type.ID) + " " + command.Request.FormatDoubleToString(command.Request.Size) + " symbol:"
                                                      + command.Request.Symbol.Name + " price open:" + command.Request.MapPriceForDigit(command.Request.OpenPrice) + " price close:" + command.Request.MapPriceForDigit(command.Request.ClosePrice) + " sl:" + command.Request.MapPriceForDigit(command.Request.StopLoss)
                                                      + " tp:" + command.Request.MapPriceForDigit(command.Request.TakeProfit), "Invalid IP", AgentProcess.Ip, AgentProcess.Code);
                }
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Dealer command confirm Invetor_" + command.InvestorID + " Agent_" + command.AgentID, ex.Message, "123", "");
            }

            IsBusyListRequest = false;
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        internal void DealerCommandReject(RequestDealer command)
        {
            if (Market.ListRequestDealer == null | Market.ListRequestDealer.Count == 0) return;
            this.WaitAccessListRequest();
            try
            {
                Agent AgentProcess = this.FindAgentOnlineByID(command.AgentID);
                if (AgentProcess != null)
                {
                    bool check = this.CheckPermitAccessRequest(AgentProcess, command);
                    if (check)
                    {
                        Business.Tick onlineTick = new Tick();
                        int digits = command.Request.Symbol.Digit;
                        double Pip = command.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
                        onlineTick.Ask = Math.Round((command.Request.Symbol.TickValue.Ask + Pip), digits);
                        onlineTick.Bid = command.Request.Symbol.TickValue.Bid;

                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': reject '" + command.Request.Investor.Code
                       + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);

                        command.AgentCode = AgentProcess.Code;
                        command.Answer = "Reject";
                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);

                        for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                        {
                            if (Market.ListRequestDealer[i].AgentID == command.AgentID && Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode)
                            {
                                Market.ListRequestDealer.RemoveAt(i);
                                this.RemoveListQueueCompare(command);
                                command.Notice = "RD24";
                                command.FlagConfirm = false;
                                this.AddRequestDealerToInvestor(command);
                                this.SetStatusDealer(command.AgentID);
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.ReturnRequest(command);
                        TradingServer.Facade.FacadeAddNewSystemLog(1, "'" + code + "': dealer reject action not taken (not enough rights)'" + command.Request.Investor.Code
                                                     + "' " + command.Name.ToLower() + " " + Facade.FacadeGetTypeCommand(command.Request.Type.ID) + " " + command.Request.FormatDoubleToString(command.Request.Size) + " symbol:"
                                                     + command.Request.Symbol.Name + " price open:" + command.Request.MapPriceForDigit(command.Request.OpenPrice) + " price close:" + command.Request.MapPriceForDigit(command.Request.ClosePrice) + " sl:" + command.Request.MapPriceForDigit(command.Request.StopLoss)
                                                     + " tp:" + command.Request.MapPriceForDigit(command.Request.TakeProfit), "Invalid IP", AgentProcess.Ip, AgentProcess.Code);
                    }
                }
                else
                {
                    this.ReturnRequest(command);
                }
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Dealer command reject Invetor_" + command.InvestorID + "Agent_" + command.AgentID, ex.Message, "123", "");
            }
            IsBusyListRequest = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        internal void DealerCommandReturn(RequestDealer command)
        {
            if (Market.ListRequestDealer == null | Market.ListRequestDealer.Count == 0) return;
            this.WaitAccessListRequest();
            try
            {
                Agent AgentProcess = this.FindAgentOnlineByID(command.AgentID);
                if (AgentProcess != null)
                {
                    Business.Tick onlineTick = new Tick();
                    int digits = command.Request.Symbol.Digit;
                    double Pip = command.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
                    onlineTick.Ask = Math.Round((command.Request.Symbol.TickValue.Ask + Pip), digits);
                    onlineTick.Bid = command.Request.Symbol.TickValue.Bid;

                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + AgentProcess.Code + "': return '" + command.Request.Investor.Code
                      + "' " + command.LogRequest, "", AgentProcess.Ip, AgentProcess.Code);

                    command.AgentCode = AgentProcess.Code;
                    command.Answer = "Return";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                }

                this.ReturnRequest(command);
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Dealer command return Invetor_" + command.InvestorID + "Agent_" + command.AgentID, ex.Message, "123", "");
            }
            IsBusyListRequest = false;
        }

        internal void ReturnRequest(RequestDealer command)
        {
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].AgentID == command.AgentID && Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode)
                {
                    Market.ListRequestDealer.RemoveAt(i);
                    DistributionRequestToDealer(command);
                    this.SetStatusDealer(command.AgentID);
                    break;
                }
            }
        }

        /// <summary>
        /// Not Use
        /// </summary>
        /// <param name="inverter"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal Business.RequestDealer InvestorGetRequest(int inverterID, out int method)
        {
            if (Market.ListRequestDealer == null | Market.ListRequestDealer.Count == 0)
            {
                method = -1;
                return null;
            }
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].InvestorID == inverterID)
                {
                    if (Market.ListRequestDealer[i].FlagConfirm)
                    {
                        RequestDealer newOpenTrade = new RequestDealer();
                        newOpenTrade = Market.ListRequestDealer[i];
                        Market.ListRequestDealer.RemoveAt(i);
                        method = 1;
                        return newOpenTrade;
                    }
                    else
                    {
                        method = -1;
                        return null;
                    }
                }
            }
            method = -1;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal string GetNoticeDealer(int agentID, string keyActive)
        {
            string result = "";
            bool check = false;
            if (NoticeManager.ContainsKey(agentID))
            {
                Agent agent = NoticeManager[agentID];
                if (agent != null)
                {
                    if (agent.IsOnline == false) check = true;
                    agent.IsOnline = true;
                    agent.TimeSync = DateTime.Now;
                    if (keyActive != agent.KeyActive)
                    {
                        return "NA22";
                    }
                }
            }
            else
            {
                return "NA22";
            }

            if (NoticeDealer.ContainsKey(agentID))
            {
                result = NoticeDealer[agentID];
                NoticeDealer[agentID] = "";
            }
            if (check) result += ",NA29";
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal int GetNumRequest(int agentID)
        {
            int result = 0;
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].AgentID == agentID)
                {
                    result += 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        internal void ClientCancelRequest(RequestDealer command)
        {
            this.WaitAccessListRequest();
            try
            {
                this.RemoveListQueueCompare(command);
                for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                {
                    if (Market.ListRequestDealer[i].Request.Symbol.Name == command.Request.Symbol.Name && Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode
                        && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                    {
                        Market.ListRequestDealer.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Client cancel request Invetor_" + command.InvestorID + "Agent_" + command.AgentID, ex.Message, "123", "");
            }
            IsBusyListRequest = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        internal void ChangeDealerProcessRequest(int agentID)
        {
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].AgentID == agentID && Market.ListRequestDealer[i].FlagConfirm == false)
                {
                    Business.RequestDealer request = new RequestDealer();
                    request = Market.ListRequestDealer[i];
                    Market.ListRequestDealer.RemoveAt(i);
                    if (NoticeDealer.ContainsKey(agentID))
                    {
                        NoticeDealer[agentID] += "NA03,";
                    }
                    this.CalculationOrderExecutionMode(request);
                }
            }

        }

        public string GetTypeCommand(int id)
        {
            string result = id.ToString();
            switch (id.ToString())
            {
                case "1":
                    result = "buy spot";
                    break;
                case "2":
                    result = "sell spot";
                    break;
                case "3":
                    result = "up binary";
                    break;
                case "4":
                    result = "down binary";
                    break;
                case "5":
                    result = "buy option";
                    break;
                case "6":
                    result = "sell option";
                    break;
                case "7":
                    result = "buy limit";
                    break;
                case "8":
                    result = "sell limit";
                    break;
                case "9":
                    result = "buy stop";
                    break;
                case "10":
                    result = "sell stop";
                    break;
                case "11":
                    result = "buy future";
                    break;
                case "12":
                    result = "sell future";
                    break;
                case "13":
                    result = "deposit";
                    break;
                case "14":
                    result = "withdrawal";
                    break;
                case "15":
                    result = "credit in";
                    break;
                case "16":
                    result = "credit out";
                    break;
                case "17":
                    result = "buy stop";
                    break;
                case "18":
                    result = "sell stop";
                    break;
                case "19":
                    result = "buy limit";
                    break;
                case "20":
                    result = "sell limit";
                    break;
            }
            return result;
        }

        #region Check system

        /// <summary>
        /// Get All Dealer Online
        /// </summary>
        /// <returns></returns>
        internal string GetAllDealerOnline()
        {
            string temp = "";
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                temp += "AgentID = " + Market.AgentList[i].AgentID.ToString() + ", Code = " + Market.AgentList[i].Code + Market.AgentList[i].Comment
                    + ", IsDealer = " + Market.AgentList[i].IsDealer.ToString() + ", IsManager = " + Market.AgentList[i].IsManager.ToString()
                    + "NumRequest = " + Market.AgentList[i].NumRequest.ToString() + ", TimeSync = " + Market.AgentList[i].TimeSync.ToString()
                    + ", IsBusy = " + Market.AgentList[i].IsBusy.ToString() + "|";
            }
            return temp;
        }

        /// <summary>
        /// Get All Request On Ram
        /// </summary>
        /// <returns></returns>
        internal List<Business.RequestDealer> GetAllRequestDealer()
        {
            return Market.ListRequestDealer;
        }

        /// <summary>
        /// Get All List Request Compare On Ram
        /// </summary>
        /// <returns></returns>
        internal List<Business.RequestDealer> GetAllRequestCompareDealer()
        {
            return Market.ListQueueCompare;
        }
        /// <summary>
        /// Get All List CandlesOffline On Ram
        /// </summary>
        /// <returns></returns>
        internal string GetAllArchiveCandlesOffline()
        {
            string result = "";
            foreach (KeyValuePair<string, ProcessQuoteLibrary.Business.BarTick> kvp in CandlesOffline)
            {
                result += kvp.Value.Symbol + "," + kvp.Value.High.ToString() + "," + kvp.Value.Low.ToString() + "|";
            }
            return result;
        }
        #endregion

        #region Virtual Dealer

        internal void VirtualDealerAutoOpen(Business.RequestDealer command)
        {
            Thread.Sleep(this.VirtualDealer.Delay * 1000);
            #region Tick
            int digits = command.Request.Symbol.Digit;
            Business.Tick onlineTick = new Tick();
            double Pip = command.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
            onlineTick.Ask = Math.Round((command.Request.Symbol.TickValue.Ask + Pip), digits);
            onlineTick.Bid = command.Request.Symbol.TickValue.Bid;
            onlineTick.Status = command.Request.Symbol.TickValue.Status;
            onlineTick.SymbolID = command.Request.Symbol.SymbolID;
            onlineTick.SymbolName = command.Request.Symbol.Name;
            onlineTick.TickTime = command.Request.Symbol.TickValue.TickTime;
            #endregion

            #region Mode Symbol
            string modeExOfSymbol = "";
            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S006")
                {
                    modeExOfSymbol = command.Request.Symbol.ParameterItems[i].StringValue;
                    break;
                }
            }
            #endregion

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            double maxPro = Business.Symbol.ConvertNumberPip(digits, this.VirtualDealer.ProfitMaxPip);
            double maxLoss = Business.Symbol.ConvertNumberPip(digits, this.VirtualDealer.LossMaxPip);
            double add = Business.Symbol.ConvertNumberPip(digits, this.VirtualDealer.AdditionalPip);
            double openPrice = command.Request.OpenPrice;
            double pipdiff = 0;

            #region Check Buy & Sell
            if (type == 1)
            {
                pipdiff = onlineTick.Ask - openPrice;
                if (pipdiff >= 0)
                {
                    if (pipdiff > maxPro)
                    {
                        openPrice = onlineTick.Ask;
                    }
                }
                else
                {
                    pipdiff = Math.Abs(pipdiff);
                    if (pipdiff > maxLoss)
                    {
                        openPrice = onlineTick.Ask;
                    }
                }
            }
            else
            {
                pipdiff = onlineTick.Bid - openPrice;
                if (pipdiff >= 0)
                {
                    if (pipdiff > maxLoss)
                    {
                        openPrice = onlineTick.Bid;
                    }
                }
                else
                {
                    pipdiff = Math.Abs(pipdiff);
                    if (pipdiff > maxPro)
                    {
                        openPrice = onlineTick.Bid;
                    }
                }

            }
            #endregion
            try
            {
                switch (modeExOfSymbol)
                {
                    #region Market
                    case "Market":
                        {
                            if (type == 1)
                            {
                                openPrice += add;
                            }
                            else
                            {
                                openPrice -= add;
                            }
                            command.Request.OpenPrice = openPrice;
                            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                            {
                                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode
                                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                {
                                    command.Notice = "RD27";
                                    command.FlagConfirm = true;
                                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                   + "' " + command.LogRequest, "", this.Ip, this.Code);
                                    command.AgentCode = this.Code;
                                    command.Answer = "Confirm";
                                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    Market.ListRequestDealer.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Request
                    case "Request":
                        {
                            if (type == 1)
                            {
                                openPrice += add;
                            }
                            else
                            {
                                openPrice -= add;
                            }
                            command.Request.OpenPrice = openPrice;
                            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                            {
                                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode
                                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                {
                                    command.Notice = "RD28";
                                    command.FlagConfirm = false;
                                    this.AddRequestDealerToInvestor(command);
                                    Market.ListRequestDealer[i].Request.OpenPrice = command.Request.OpenPrice;
                                    // Set FlagConfirm
                                    Market.ListRequestDealer[i].FlagConfirm = true;
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': quotes '" + command.Request.Investor.Code
                                  + "' " + command.LogRequest, "", this.Ip, this.Code);
                                    command.AgentCode = this.Code;
                                    command.Answer = "Quotes";
                                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);

                                    break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Instant
                    case "Instant":
                        {

                            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                            {
                                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.ClientCode == command.Request.ClientCode
                                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                {
                                    if (Market.ListRequestDealer[i].Request.OpenPrice == openPrice)
                                    {
                                        command.Notice = "RD21";
                                        command.FlagConfirm = true;
                                        command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                                        //this.AddRequestDealerToInvestor(command);
                                        this.RemoveListQueueCompare(command);
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                       + "' " + command.LogRequest, "", this.Ip, this.Code);
                                        command.AgentCode = this.Code;
                                        command.Answer = "Confirm";
                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    }
                                    else
                                    {
                                        command.Request.OpenPrice = openPrice;
                                        this.AddListQueueCompare(command);
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': quotes '" + command.Request.Investor.Code
                                      + "' " + command.LogRequest, "", this.Ip, this.Code);
                                        command.AgentCode = this.Code;
                                        command.Answer = "Quotes";
                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    }

                                    Market.ListRequestDealer.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Robot Dealer command confirm Invetor_" + command.InvestorID + " Robot Dealer" + this.Name, ex.Message, "123", "");
            }
        }

        internal void VirtualDealerAutoClose(Business.RequestDealer command)
        {
            Thread.Sleep(this.VirtualDealer.Delay * 1000);
            #region Tick
            int digits = command.Request.Symbol.Digit;
            Business.Tick onlineTick = new Tick();
            double Pip = command.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
            onlineTick.Ask = Math.Round((command.Request.Symbol.TickValue.Ask + Pip), digits);
            onlineTick.Bid = command.Request.Symbol.TickValue.Bid;
            onlineTick.Status = command.Request.Symbol.TickValue.Status;
            onlineTick.SymbolID = command.Request.Symbol.SymbolID;
            onlineTick.SymbolName = command.Request.Symbol.Name;
            onlineTick.TickTime = command.Request.Symbol.TickValue.TickTime;
            #endregion

            #region Mode Symbol
            string modeExOfSymbol = "";
            for (int i = command.Request.Symbol.ParameterItems.Count - 1; i >= 0; i--)
            {
                if (command.Request.Symbol.ParameterItems[i].Code == "S006")
                {
                    modeExOfSymbol = command.Request.Symbol.ParameterItems[i].StringValue;
                    break;
                }
            }
            #endregion

            int type = Business.Symbol.ConvertCommandIsBuySell(command.Request.Type.ID);
            double maxPro = Business.Symbol.ConvertNumberPip(digits, this.VirtualDealer.ProfitMaxPip);
            double maxLoss = Business.Symbol.ConvertNumberPip(digits, this.VirtualDealer.LossMaxPip);
            double add = Business.Symbol.ConvertNumberPip(digits, this.VirtualDealer.AdditionalPip);
            double closePrice = command.Request.ClosePrice;
            double pipdiff = 0;

            #region Check CloseBuy & CloseSell
            if (type == 1)
            {
                pipdiff = onlineTick.Bid - closePrice;
                if (pipdiff >= 0)
                {
                    if (pipdiff > maxLoss)
                    {
                        closePrice = onlineTick.Bid;
                    }
                }
                else
                {
                    pipdiff = Math.Abs(pipdiff);
                    if (pipdiff > maxPro)
                    {
                        closePrice = onlineTick.Bid;
                    }
                }
            }
            else
            {
                pipdiff = onlineTick.Ask - closePrice;
                if (pipdiff >= 0)
                {
                    if (pipdiff > maxPro)
                    {
                        closePrice = onlineTick.Ask;
                    }
                }
                else
                {
                    pipdiff = Math.Abs(pipdiff);
                    if (pipdiff > maxLoss)
                    {
                        closePrice = onlineTick.Ask;
                    }
                }
            }
            #endregion
            try
            {
                switch (modeExOfSymbol)
                {
                    #region Market
                    case "Market":
                        {
                            if (type == 1)
                            {
                                closePrice -= add;
                            }
                            else
                            {
                                closePrice += add;
                            }
                            command.Request.ClosePrice = closePrice;
                            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                            {
                                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.CommandCode == command.Request.CommandCode
                                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                {
                                    command.Notice = "RD31";
                                    command.FlagConfirm = true;
                                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                  + "' " + command.LogRequest, "", this.Ip, this.Code);
                                    command.AgentCode = this.Code;
                                    command.Answer = "Confirm";
                                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    Market.ListRequestDealer.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Request
                    case "Request":
                        {
                            if (type == 1)
                            {
                                closePrice -= add;
                            }
                            else
                            {
                                closePrice += add;
                            }
                            command.Request.ClosePrice = closePrice;
                            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                            {
                                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.CommandCode == command.Request.CommandCode
                                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                {
                                    command.Notice = "RD28";
                                    command.FlagConfirm = false;
                                    this.AddRequestDealerToInvestor(command);
                                    Market.ListRequestDealer[i].Request.ClosePrice = command.Request.ClosePrice;
                                    // Set FlagConfirm
                                    Market.ListRequestDealer[i].FlagConfirm = true;
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': quotes '" + command.Request.Investor.Code
                                           + "' " + command.LogRequest, "", this.Ip, this.Code);
                                    command.AgentCode = this.Code;
                                    command.Answer = "Quotes";
                                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Instant
                    case "Instant":
                        {
                            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                            {
                                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.CommandCode == command.Request.CommandCode
                                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                                {
                                    if (Market.ListRequestDealer[i].Request.ClosePrice == closePrice)
                                    {
                                        command.Notice = "RD22";
                                        command.FlagConfirm = true;
                                        command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                                        this.RemoveListQueueCompare(command);
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                       + "' " + command.LogRequest, "", this.Ip, this.Code);
                                        command.AgentCode = this.Code;
                                        command.Answer = "Confirm";
                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    }
                                    else
                                    {
                                        command.Request.ClosePrice = closePrice;
                                        this.AddListQueueCompare(command);
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': quotes '" + command.Request.Investor.Code
                                       + "' " + command.LogRequest, "", this.Ip, this.Code);
                                        command.AgentCode = this.Code;
                                        command.Answer = "Quotes";
                                        TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                                    }
                                    Market.ListRequestDealer.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Robot Dealer command confirm Invetor_" + command.InvestorID + " Robot Dealer" + this.Name, ex.Message, "123", "");
            }
        }

        internal void VirtualDealerAutoUpdate(Business.RequestDealer command)
        {
            Thread.Sleep(this.VirtualDealer.Delay * 1000);
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.CommandCode == command.Request.CommandCode
                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                {
                    command.Request.Symbol.MarketAreaRef.UpdateCommand(command.Request);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                          + "' " + command.LogRequest, "", this.Ip, this.Code);
                    command.AgentCode = this.Code;
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    Market.ListRequestDealer.RemoveAt(i);
                    break;
                }
            }
        }

        internal void VirtualDealerAutoDelete(Business.RequestDealer command)
        {
            Thread.Sleep(this.VirtualDealer.Delay * 1000);
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.CommandCode == command.Request.CommandCode
                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                {
                    command.Request.Symbol.MarketAreaRef.CloseCommand(command.Request);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                          + "' " + command.LogRequest, "", this.Ip, this.Code);
                    command.AgentCode = this.Code;
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    Market.ListRequestDealer.RemoveAt(i);
                    break;
                }
            }
        }

        internal void VirtualDealerAutoOpenPending(Business.RequestDealer command)
        {
            Thread.Sleep(this.VirtualDealer.Delay * 1000);
            for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
            {
                if (Market.ListRequestDealer[i].Name == command.Name && Market.ListRequestDealer[i].Request.CommandCode == command.Request.CommandCode
                    && Market.ListRequestDealer[i].InvestorID == command.InvestorID)
                {
                    command.Request.Symbol.MarketAreaRef.AddCommand(command.Request);
                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + this.Code + "': confirm '" + command.Request.Investor.Code
                                          + "' " + command.LogRequest, "", this.Ip, this.Code);
                    command.AgentCode = this.Code;
                    command.Answer = "Confirm";
                    TradingServer.Facade.FacadeSendNoticeManagerDealerRequest(command);
                    Market.ListRequestDealer.RemoveAt(i);
                    break;
                }
            }
        }
        #endregion
    }
}
