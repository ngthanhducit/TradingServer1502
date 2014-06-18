using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Diagnostics;
//using ForexSignal;

namespace TradingServer.Business
{
    public partial class Market 
    {
        public static Business.Market marketInstance = new Market();
        
        #region Create Instance Class DBWMarketArea
        private static DBW.DBWMarketArea dbwMarketArea;
        private static DBW.DBWMarketArea DBWMarketArea
        {
            get
            {
                if (Market.dbwMarketArea == null)
                {
                    Market.dbwMarketArea = new DBW.DBWMarketArea();
                }

                return Market.dbwMarketArea;
            }
        }
        #endregion

        /// <summary>
        /// Constructure
        /// </summary>
        public Market()
        {
            this.IniMarket();
            //this.Init();
            //this.testSocket();
        }

        #region Implement Function Interface IPresenter.ICommand        
        /// <summary>
        /// EXTRACT COMMAND AND CALL FUNCTION
        /// </summary>
        /// <param name="Cmd"></param>
        public void ExtractCommand(string Cmd, string ipAddress)
        {
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subCommand = Cmd.Split('#');
                if (subCommand.Length > 0)
                {
                    int count = subCommand.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string[] subValue = subCommand[i].Split('$');
                        if (subValue.Length > 0)
                        {
                            switch (subValue[0])
                            {
                                #region CASE SDDE SEND TICK TO SERVER
                                case "Tick":
                                    {
                                        //bool isQuotes = Business.Market.CheckPriceQuotes(ipAddress);
                                        //if (!isQuotes)
                                        //    return;

                                        string[] subParameter = subValue[1].Split(',');

                                        int numCheck = 0;
                                        double priceAsk, priceBid;
                                        DateTime tickTime;

                                        double.TryParse(subParameter[0], out priceAsk);
                                        double.TryParse(subParameter[1], out priceBid);
                                        DateTime.TryParse(subParameter[4], out tickTime);

                                        Business.Tick newTick = new Tick();
                                        newTick.Ask = priceAsk;
                                        newTick.Bid = priceBid;
                                        newTick.Status = subParameter[2];
                                        newTick.SymbolName = subParameter[3];
                                        newTick.TickTime = DateTime.Parse(subParameter[4]);

                                        int.TryParse(subParameter[5], out numCheck);

                                        this.UpdateTick(newTick);
                                    }
                                    break;
                                #endregion

                                case "ListTick":
                                    {
                                        //bool isQuotes = Business.Market.CheckPriceQuotes(ipAddress);
                                        //if (!isQuotes)
                                        //    return;

                                        List<Business.Tick> result = TradingServer.Model.BuildCommand.Instance.ConvertStringToListTick(subValue[1]);
                                        if (result != null && result.Count > 0)
                                        {
                                            int countResult = result.Count;
                                            for (int j = 0; j < countResult; j++)
                                            {
                                                this.UpdateTick(result[j]);
                                            }
                                        }
                                    }
                                    break;

                                case "MultipleListTick":
                                    {
                                        //bool isQuotes = Business.Market.CheckMultiPriceQuotes(ipAddress);
                                        //if (!isQuotes)
                                        //    return;
                                        //@"MultipleListTick$0.81991000000000003{0.81960999999999995{up{NZDUSD_ECN{12/05/2013 15:44:57|
                                        //0.83150000000000002{0.83126{up{EURGBP_ECN{12/05/2013 15:44:57|
                                        //1.5061800000000001{1.50587{up{EURAUD_ECN{12/05/2013 15:44:57|
                                        //1.4750000000000001{1.4745300000000001{up{GBPCHF_ECN{12/05/2013 15:44:57|
                                        //166.87100000000001{166.827{down{GBPJPY_ECN{12/05/2013 15:44:57|
                                        //1.74458{1.7441499999999999{down{GBPCAD_ECN{12/05/2013 15:44:57|
                                        //1.8117399999999999{1.81124{up{GBPAUD_ECN{12/05/2013 15:44:57|
                                        //92.117999999999995{92.097999999999999{up{AUDJPY_ECN{12/05/2013 15:44:57|
                                        //0.81435000000000002{0.81398000000000004{up{AUDCHF_ECN{12/05/2013 15:44:57|
                                        //6503.5{6503.{down{FTSE100{12/05/2013 15:44:57|
                                        //0.81991999999999998{0.81962000000000002{up{NZDUSDx{12/05/2013 15:44:57|
                                        //0.83160999999999996{0.83121{up{EURGBPx{12/05/2013 15:44:57|
                                        //92.131{92.090999999999994{up{AUDJPYx{12/05/2013 15:44:57|
                                        //1.4751699999999999{1.4744699999999999{up{GBPCHFx{12/05/2013 15:44:57|
                                        //1.5064299999999999{1.50563{up{EURAUDx{12/05/2013 15:44:57|
                                        //1.81189{1.8110900000000001{up{GBPAUDx{12/05/2013 15:44:57|
                                        //1233.75{1233.25{down{SLLG{12/05/2013 15:44:57|
                                        //9144.5{9144.{down{DAX30{12/05/2013 15:44:57|
                                        //1233.75{1233.25{down{MSLLG{12/05/2013 15:44:57"

                                        //List<Business.Tick> result = TradingServer.Model.BuildCommand.Instance.ConvertStringToListTick(subValue[1]);
                                        List<Business.Tick> result = TradingServer.Model.BuildCommand.Instance.MapStringToTick(subValue[1]);
                                        if (result != null && result.Count > 0)
                                        {
                                            int countResult = result.Count;
                                            for (int j = 0; j < countResult; j++)
                                            {
                                                result[j].IsManager = true;
                                                this.UpdateTickMT4(result[j]);
                                            }
                                        }
                                    }
                                    break;

                                #region CASE CLIENT MAKE COMMAND
                                case "MakeComamnd":
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        Business.OpenTrade newOpenTrade = new OpenTrade();
                                        int CommandTypeID = 0;
                                        int InvestorID = 0;
                                        int SymbolID = 0;

                                        double OpenPrice = 0;
                                        double Size = 0;
                                        double StopLoss = 0;
                                        double TakeProfit = 0;

                                        int.TryParse(subParameter[0], out CommandTypeID);
                                        int.TryParse(subParameter[1], out InvestorID);
                                        int.TryParse(subParameter[2], out SymbolID);

                                        double.TryParse(subParameter[4], out OpenPrice);
                                        double.TryParse(subParameter[5], out Size);
                                        double.TryParse(subParameter[6], out StopLoss);
                                        double.TryParse(subParameter[7], out TakeProfit);

                                        #region Find Investor List
                                        //Find Investor List
                                        if (Business.Market.InvestorList != null)
                                        {
                                            int countInvestor = Business.Market.InvestorList.Count;
                                            for (int n = 0; n < countInvestor; n++)
                                            {
                                                if (Business.Market.InvestorList[n].InvestorID == InvestorID)
                                                {
                                                    newOpenTrade.Investor = Business.Market.InvestorList[n];
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion

                                        newOpenTrade.ClientCode = subParameter[3];
                                        newOpenTrade.OpenPrice = OpenPrice;
                                        newOpenTrade.Size = Size;
                                        newOpenTrade.StopLoss = StopLoss;
                                        newOpenTrade.TakeProfit = TakeProfit;

                                        newOpenTrade.Symbol.MarketAreaRef.AddCommand(newOpenTrade);
                                    }
                                    break;
                                #endregion
                            }
                        }
                    }
                }
            }
        }               
        #endregion        

        #region Implement Function Interface IPresenter.IMarketArea
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void AddCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void CloseCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiCloseCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiUpdateCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public OpenTrade CalculateCommand(OpenTrade Command)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public IPresenter.CloseCommandDelegate CloseCommandNotify(OpenTrade Command)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        public IPresenter.SendClientCmdDelegate SendClientCmdDelegate(string Cmd)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        public void SetTickValueNotify(Tick Tick, Symbol RefSymbol)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void UpdateCommand(OpenTrade Command)
        {
            return;
        }

        #endregion

        #region Implement Function Class Market        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void AddUpdateComment(Business.Symbol Value)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Ip"></param>
        /// <param name="InvestorCode"></param>
        /// <returns></returns>
        public string ClientMakeCommand(string Command, string Ip, string InvestorCode)
        {
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Index"></param>
        /// <param name="InvestorCode"></param>
        /// <returns></returns>
        public string ClientPrefesh(string Ip, int Index, string InvestorCode)
        {
            return string.Empty;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="SString"></param>
        /// <returns></returns>
        public string SClientPrefesh(string Ip, string SString)
        {
            return string.Empty;
        }

        /// <summary>
        /// UPDATE TICK ONLINE 
        /// </summary>
        /// <param name="Tick"></param>
        public void UpdateTick(Business.Tick Tick)
        {
            //Recive Tick From DDE
            if (Market.IsOpen)
            {
                //Find Symbol In List Market.QuoteList
                if (Market.QuoteList != null)
                {
                    //MarketQuoteList !=null
                    //
                    bool Flag = false;
                    int count = Market.QuoteList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.QuoteList[i] == null)
                        {
                            Business.Market.QuoteList.RemoveAt(i);
                            i--;
                            count = Business.Market.QuoteList.Count;
                            continue;
                        }

                        if (Market.QuoteList[i].Name == Tick.SymbolName)
                        {   
                            if (Market.QuoteList[i].RefSymbol.Count > 0)
                            {
                                Market.QuoteList[i].Update(Tick);                                
                            }
                            Flag = true;
                            break;                                                        
                        }
                    }

                    if (Flag == false)
                    {
                        Business.QuoteSymbol newQuoteSymbol = new QuoteSymbol();
                        newQuoteSymbol.Name = Tick.SymbolName;
                        //newQuoteSymbol.TickValue = Tick;                        
                        newQuoteSymbol.RefSymbol = this.GetReferenceSymbol(Tick.SymbolName);
                        //newQuoteSymbol.RefSymbol = this.GetSymbol(Tick.SymbolName);
                        if (newQuoteSymbol.RefSymbol.Count > 0)
                        {
                            Market.QuoteList.Add(newQuoteSymbol);           
                            newQuoteSymbol.Update(Tick);                            
                        }
                    }
                }   //End If Check Maraket.QuoteList != Null
                else
                {
                    //quote list is null 
                    //
                    Market.QuoteList = new List<QuoteSymbol>();
                    Business.QuoteSymbol newQuoteSymbol = new QuoteSymbol();
                    newQuoteSymbol.Name = Tick.SymbolName;
                    //newQuoteSymbol.TickValue = Tick;
                    newQuoteSymbol.RefSymbol = this.GetReferenceSymbol(Tick.SymbolName);
                    //newQuoteSymbol.RefSymbol = this.GetSymbol(Tick.SymbolName);
                    if (newQuoteSymbol.RefSymbol.Count > 0)
                    {
                        Market.QuoteList.Add(newQuoteSymbol);                  
                        newQuoteSymbol.Update(Tick);                        
                    }
                }   
                //End Else Check Market.QuoteList != Null
            }   
            //End If Check Market.IsOpen                
        }  //End Function Add Tick

        /// <summary>
        /// UPDATE TICK ONLINE 
        /// </summary>
        /// <param name="Tick"></param>
        public void UpdateTickMT4(Business.Tick Tick)
        {
            //Recive Tick From DDE
            if (Market.IsOpen)
            {
                //Find Symbol In List Market.QuoteList
                if (Market.QuoteList != null)
                {
                    //MarketQuoteList !=null
                    //
                    bool Flag = Business.Market.QuoteSymbols.ContainsKey(Tick.SymbolName);

                    if (Flag)
                    {
                        Business.QuoteSymbol newQuoteSymbol = new QuoteSymbol();
                        newQuoteSymbol = Business.Market.QuoteSymbols[Tick.SymbolName];
                        newQuoteSymbol.Update(Tick);
                    }
                    else
                    {
                        Business.QuoteSymbol newQuoteSymbol = new QuoteSymbol();
                        newQuoteSymbol.Name = Tick.SymbolName;
                        
                        newQuoteSymbol.RefSymbol = this.GetSymbol(Tick.SymbolName);
                        if (newQuoteSymbol.RefSymbol.Count > 0)
                        {
                            Market.QuoteList.Add(newQuoteSymbol);
                            Business.Market.QuoteSymbols.Add(Tick.SymbolName, newQuoteSymbol);
                            newQuoteSymbol.Update(Tick);
                        }
                    }
                }   //End If Check Maraket.QuoteList != Null
                else
                {
                    //quote list is null 
                    //
                    Market.QuoteList = new List<QuoteSymbol>();
                    Business.QuoteSymbol newQuoteSymbol = new QuoteSymbol();
                    newQuoteSymbol.Name = Tick.SymbolName;
                    newQuoteSymbol.RefSymbol = this.GetSymbol(Tick.SymbolName);

                    if (newQuoteSymbol.RefSymbol.Count > 0)
                    {
                        Market.QuoteList.Add(newQuoteSymbol);
                        Business.Market.QuoteSymbols.Add(Tick.SymbolName, newQuoteSymbol);
                        newQuoteSymbol.Update(Tick);
                    }
                }
                //End Else Check Market.QuoteList != Null
            }
            //End If Check Market.IsOpen                
        }  //End Function Add Tick

        /// <summary>
        /// INITIAL MARKET
        /// </summary>
        internal void IniMarket()
        {   
            try
            {
                Market.IsOpen = false;
                Business.Market.IsFirstStart = true;

                Business.Market.TempListOrder = new List<OrderInvestor>();

                //Init IpAddressBlock
                Business.Market.BlockIpAddress = new List<List<List<List<List<bool>>>>>();

                this.MQLCommands = new List<NJ4XConnectSocket.MQLCommand>();

                //Init Calculator Facade
                Business.CalculatorFacade.IniCalculatorFacade();

                //Init Alert  
                TradingServer.Facade.FacadeGetAllAlert();

                //init thread process system log
                this.InitThreadProcessLog();

                //init thread process remove open trade
                this.InitThreadRemoveTrade();

                //init thread check investor online
                this.InitThreadCheckOnlineInvestor();

                //init market area
                this.InitMarketArea();

                //init market config
                this.InitMarketConfig();

                //init command type
                this.InitCommandType();

                //init news
                this.InitNews();

                //init symbol list
                this.InitSymbolList();

                //init investor list
                this.InitInvestorList();

                //Init Process Quotes
                ProcessQuoteLibrary.Business.QuoteProcess.QuoteInstance.InitThread();

                Business.Market.TimeDelaySendWarningMarginCall = 4;

                Business.Market.IsRealSearver = true;

                Business.Market.IsConnectAgent = false;

                Business.Market.IsCheckScalper = false;

                Business.Market.ScalperPipValue = 5;
                Business.Market.ScalperTimeValue = 60;

                this.InitConnectMT4();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Init()
        {
            TradingServer.Facade.FacadeAddNewSystemLog(1, "Init Market", "[check init market]", "", "");

            Business.Market.TempListOrder = new List<OrderInvestor>();

            //Init WCF
            //this.InitWCF();

            //Init IpAddressBlock
            Business.Market.BlockIpAddress = new List<List<List<List<List<bool>>>>>();

            //Init Value Multiple Quotes
            Business.Market.MultiplePriceQuotes = new List<PriceServer>();

            Business.Market.DayEvent = new List<TimeEvent>();
            Business.Market.WeekEvent = new List<TimeEvent>();
            Business.Market.YearEvent = new List<TimeEvent>();
            
            this.EndOfDay = new DateTimeEvent();
            Business.Market.InvestorOnline = new List<Investor>();

            //Init Command Excutor
            Business.Market.CommandExecutor = new List<OpenTrade>();

            //Init Calculator Facade
            Business.CalculatorFacade.IniCalculatorFacade();

            //Market.IsOpen = false;
            Market.QuoteList = new List<Business.QuoteSymbol>();

            //Init Market Config
            Market.MarketConfig = TradingServer.Facade.FacadeGetAllMarketConfig();

            //CHECK COUNT MARKET CONFIG
            if (Market.MarketConfig.Count != TradingServer.Facade.FacadeCountMarketConfig())
                return;

            ////Init MarketArea
            Market.MarketArea = new List<IPresenter.IMarketArea>();
            Market.MarketArea = Market.DBWMarketArea.GetAllMarketArea();

            //Get All Investor Gorup
            //Init Investor Group List
            Market.InvestorGroupList = new List<InvestorGroup>();

            //Get All Security
            //Init List Security
            Market.SecurityList = new List<Business.Security>();

            //Init Command Type
            TradingServer.Facade.FacadeGetAllCommandType();

            //Init Investor
            Market.InvestorList = new List<Investor>();
            
            //Init IGroupSecurity
            Market.IGroupSecurityList = new List<IGroupSecurity>();
            
            //Get All Symbol
            //Init List Symbol
            Market.SymbolList = new List<Symbol>();
            
            //Init IGroupSymbol
            Market.IGroupSymbolList = new List<IGroupSymbol>();
            Market.IGroupSymbolList = TradingServer.Facade.FacadeGetAllIGroupSymbol();

            if (Business.Market.IGroupSymbolList.Count != TradingServer.Facade.FacadeCountIGroupSymbol())
                return;

            //Calculation margin All Open Trade
            //TradingServer.Facade.FacadeReCalculationAccount();

            //Init Alert  
            TradingServer.Facade.FacadeGetAllAlert();

            //Init Agent
            Market.AgentList = new List<Agent>();
            Market.AdminList = new List<Agent>();
            //Init News
            Market.NewsList = new List<News>();
            TradingServer.Facade.FacadeGetTopNews();

            ////Init Event
            Business.Market.DayEvent = new List<TimeEvent>();

            ////Init List Day Event
            Business.Market.WeekEvent = new List<TimeEvent>();

            ////Init List Year Event
            Business.Market.YearEvent = new List<TimeEvent>();

            ////Init List Future Event
            Business.Market.FutureEvent = new List<TimeEvent>();

            ////Init List Group Default
            Business.Market.ListGroupDefault = new List<GroupDefault>();

            //Init Event
            //this.InitTimeEventInSymbol();
            //this.InitTimeEventServer();
            //this.InitSymbolFuture();

            //Init Request Dealer
            Business.Market.ListRequestDealer = new List<RequestDealer>();
            Business.Market.ListQueueCompare = new List<RequestDealer>();

            //Init Process Agent
            //Business.Market.ThreadAgentRecycle = new System.Threading.Thread(new System.Threading.ThreadStart(CleanRecycleRequest));
            //COMMENT BECAUSE CONNECT MT4
            //Business.Market.ThreadAgentRecycle.Start();
            //this.IniVirtualDealer();

            //Init Process Quote
            ProcessQuoteLibrary.Business.QuoteProcess.QuoteInstance.InitThread();

            Business.Market.ThreadInvestorOnline = new System.Threading.Thread(new System.Threading.ThreadStart(TimeEventCheckInvestorOnline));
            Business.Market.ThreadInvestorOnline.Start();

            //Business.Market.ThreadMultiplePrice = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadCheckMultiplePrice));
            //COMMENT BECAUSE CONNECT MT4
            //Business.Market.ThreadMultiplePrice.Start();

            Business.Market.ThreadRemoveCommand = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ThreadRemoveOpenTrade));
            //REMOTE BECAUSE CONNECT MT4
            Business.Market.ThreadRemoveCommand.Start();

            #region INIT PROCESS SYSTEM LOG
            Business.Market.ListSystemLog = new List<SystemLog>();
            Business.Market.IsProcessLog = true;

            Business.Market.ThreadSystemLog = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessSystemLog));
            Business.Market.ThreadSystemLog.Start();
            #endregion

            Business.Market.IsRemoveCommand = true;
            Business.Market.RemoveCommandList = new List<OpenRemove>();

            #region INIT PROCESS STATEMENT EOD
            Business.Market.IsProcessAddStatement = true;
            Business.Market.ListStatementEOD = new List<StatementInvestor>();

            //Business.Market.ThreadStatement = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessStatementEOD));
            //COMMENT BECAUSE CONNECT MT4
            //Business.Market.ThreadStatement.Start();
            #endregion

            #region INIT PROCESS LAST ACCOUNT
            Business.Market.IsProcessLastAccount = true;
            Business.Market.ListLastAccount = new List<SumLastAccount>();
            Business.Market.ThreadLastAccount = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessLastBalance));
            //COMMENT BECAUSE CONNECT MT4
            //Business.Market.ThreadLastAccount.Start();
            #endregion

            #region INIT CONNECT MT4
            Business.Market.IsConnectMT4 = true;
            Business.Market.StatusConnect = true;
            Business.Market.NotifyMessageFromMT4 = new List<string>();

            //DELEGATE NOTIFY FROM MT4
            Business.Market.InstanceSocket = new Element5SocketConnectMT4.Business.SocketConnect();
            Business.Market.InstanceSocket.NotifyMT4 = this.ReceiveNotify;
            Business.Market.InstanceSocket.DelGroups = this.ReceiveGroupNotify;
            Business.Market.InstanceSocket.DelSecuritys = this.ReceiveSecurityNotify;
            Business.Market.InstanceSocket.DelSymbols = this.ReceiveSymbolNotify;

            //THREAD PROCESS NOTIFY MESSAGE FROM MT4
            Business.Market.IsProcessNotifyMessage = true;
            Business.Market.ThreadProcessNotifyMessage = new System.Threading.Thread(new System.Threading.ThreadStart(ProcessNotifyMessage));
            Business.Market.ThreadProcessNotifyMessage.Start();

            //INIT THREAD SOCKET SERVER SEND COMMAND FROM ET5 TO MT4
            Business.Market.InstanceSocketAsync = new Element5SocketConnectMT4.Business.SocketConnectAsync();
            Business.Market.InstanceSocketAsync.NotifyMT4 = this.ReceiveNotify;
            Business.Market.InstanceSocketAsync.StartListener(DEFAULT_PORTASYNC, DEFAULT_IPADDRESS);

            //INIT THREAD SOCKET SERVER RECEIVE TICK FROM MT4
            //Business.Market.InstanceSocketTickAsync = new Element5SocketConnectMT4.Business.SocketConnectTickAsync();
            //Business.Market.InstanceSocketTickAsync.NotifyTickMT4 = this.ReceiveTickNotify;
            //Business.Market.InstanceSocketTickAsync.StartListener(DEFAULT_PORTASYNC_TICK, DEFAULT_IPADDRESS);

            //Business.Market.InstanceSocketTick = new Element5SocketConnectMT4.Business.SocketConnectTick();
            //Business.Market.InstanceSocketTick.StartListening(DEFAULT_IPADDRESS, DEFAULT_PORTASYNC_TICK);


            //START SOCKET SYNC SERVER
            Business.Market.InstanceSocket.StartListening(DEFAULT_IPADDRESS, DEFAULT_PORT);

            #endregion

            #region INIT CONNECT MT4 USING MQL
            //Business.Market.MQLTrades = new List<MQLConnector.Model.TradeResult>();
            //MQLConnector.Facade.StartServerSocket();
            //MQLConnector.Facade.DelegateTrade = this.ReceiveNotifyMQL;
            #endregion

            #region INIT NJ4XConnect
            Business.Market.NJ4XTickets = new List<NJ4XConnectSocket.NJ4XTicket>();
            #endregion

            #region INIT THREAD SAVE STATEMENT
            Business.Market.IsProcessSaveStatement = true;
            Business.Market.ListStatement = new List<Statement>();
            Business.Market.ThreadSaveStatement = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessSaveStatement));
            //COMMENT BECAUSE CONNECT MT4
            //Business.Market.ThreadSaveStatement.Start();
            #endregion

            Business.Market.TimeDelaySendWarningMarginCall = 4;

            string path = Environment.CurrentDirectory + DateTime.Now.Ticks;

            Business.Market.ListAgentReport = new List<AgentReport>();

            Business.Market.ListEODAgent = new List<EndOfDayAgent>();

            Business.Market.IsRealSearver = true;

            Business.Market.IsConnectAgent = false;

            Business.Market.IsCheckScalper = false;

            Business.Market.ScalperPipValue = 5;
            Business.Market.ScalperTimeValue = 60;

            //#region INIT DEFAULT DATA ADMIN OR MANAGER REQUEST CLIENT LOG
            Business.Market.InRequestClientLog = false;
            Business.Market.ListClientLogs = new List<ClientLog>();
            //#endregion

            TradingServer.Facade.FacadeAddNewSystemLog(1, "End Init Market", "[check init market]", "", "");
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitWCF()
        {
            EndpointAddress address = new EndpointAddress("http://demotradingwcf.et5.co/tradingwcf.svc");
            //EndpointAddress address = new EndpointAddress("http://tradingwcf.sycomore.co.uk/tradingwcf.svc");
            BasicHttpBinding n = new BasicHttpBinding();
            n.MaxBufferSize = 2147483647;
            n.MaxReceivedMessageSize = 2147483647;
            n.Security.Mode = BasicHttpSecurityMode.None;
            System.ServiceModel.Channels.Binding bin = n;
            bin.Name = "BasicHttpBinding_ITradingWCFV3";
            Market.client = new Trading.TradingWCFClient(bin, address);

            EndpointAddress addressQuote = new EndpointAddress("http://demotradingwcf.et5.co/tradingwcf.svc");
            //EndpointAddress addressQuote = new EndpointAddress("http://tradingwcf.sycomore.co.uk/tradingwcf.svc");
            BasicHttpBinding basicBinding = new BasicHttpBinding();
            basicBinding.MaxBufferSize = 2147483647;
            basicBinding.MaxReceivedMessageSize = 2147483647;
            basicBinding.Security.Mode = BasicHttpSecurityMode.None;
            System.ServiceModel.Channels.Binding binQuotes = basicBinding;
            basicBinding.Name = "BasicHttpBinding_IPriceQuotes";
            Market.clientQuote = new PriceQuotes.PriceQuotesClient(bin, addressQuote);

            //EndpointAddress addressBroker = new EndpointAddress("http://localhost:23165/BrokerWCF.svc");
            //BasicHttpBinding binBroker = new BasicHttpBinding();
            //binBroker.MaxBufferSize = 2147483647;
            //binBroker.MaxReceivedMessageSize = 2147483647;
            //binBroker.Security.Mode = BasicHttpSecurityMode.None;
            //System.ServiceModel.Channels.Binding bindingBroker = binBroker;
            //bindingBroker.Name = "BasicHttpBinding_IBroker";
            //Market.clientBroker = new BrokerWCF.BrokerWCFClient(bindingBroker, addressBroker);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitAgent()
        {
            TradingServer.Agent.AgentConfig newAgentConfig = new TradingServer.Agent.AgentConfig();
            newAgentConfig.AgentName = "EFXGMAGENT";
            //newAgentConfig.DomainAccess = "http://localhost:1732/Default.aspx";
            newAgentConfig.DomainAccess = "http://agentwcf.sycomore.vn/Default.aspx";
            //newAgentConfig.DomainAccess = "http://agentlocalwcf.sycomore.vn/Default.aspx";
            //newAgentConfig.DomainAccess = "http://agenttradingwcf.sycomore.vn/Default.aspx";
            //newAgentConfig.DomainAccess = "http://agentefxgmwcf.et5.co/Default.aspx";
            newAgentConfig.GroupDefault = "Element";
            newAgentConfig.GroupID = 40;
            newAgentConfig.IpAddress = "111.91.237.197";
            newAgentConfig.IsConnect = false;
            newAgentConfig.NotifyQueue = new List<string>();
            newAgentConfig.TickQueue = new List<string>();

            //EndpointAddress address = new EndpointAddress("http://localhost:1732/DefaultWCF.svc");
            EndpointAddress address = new EndpointAddress("http://agentwcf.sycomore.vn/DefaultWCF.svc");
            //EndpointAddress address = new EndpointAddress("http://agentlocalwcf.sycomore.vn/DefaultWCF.svc");
            ////EndpointAddress address = new EndpointAddress("http://agenttradingwcf.sycomore.vn/DefaultWCF.svc");
            //EndpointAddress address = new EndpointAddress("http://agentefxgmwcf.et5.co/DefaultWCF.svc");
            BasicHttpBinding n = new BasicHttpBinding();
            n.MaxBufferSize = 2147483647;
            n.MaxReceivedMessageSize = 2147483647;
            n.Security.Mode = BasicHttpSecurityMode.None;
            System.ServiceModel.Channels.Binding bin = n;
            bin.Name = "BasicHttpBinding_ITradingWCFV3";
            newAgentConfig.clientAgent = new AgentWCF.DefaultWCFClient(bin, address);

            Business.Market.ListAgentConfig.Add(newAgentConfig);
        }
        #endregion

        #region CleanRecycle
        public void CleanRecycleRequest()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                this.RemoveDealerExpire();
                this.DeleteRequestExpire();
            }
        }

        /// <summary>
        /// 5 minutes Expired
        /// </summary>
        internal void DeleteRequestExpire()
        {                       
            string codeDealer = "";
            string codeInvestor = "";
            try
            {
                if (Market.ListRequestDealer != null)
                {
                    for (int i = Market.ListRequestDealer.Count - 1; i >= 0; i--)
                    {
                        RequestDealer command = new RequestDealer();
                        command = Market.ListRequestDealer[i];
                        TimeSpan executionTime = DateTime.Now - command.TimeClientRequest;
                        if (executionTime.Minutes >= 1.6)
                        {
                            command.FlagConfirm = false;
                            command.Notice = "RD24";
                            this.AddRequestDealerToInvestor(command);
                            codeDealer += "R_" + command.AgentID.ToString() + "_";
                            codeInvestor += "R_" + command.InvestorID.ToString() + "_";

                            TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + command.Request.Investor.Code
                                                         + "': No Response from dealer '" + command.AgentCode + "' " + command.LogRequest, "Time Out Trade", "", "system");

                            Market.ListRequestDealer.RemoveAt(i);
                        }
                    }
                }
                if (Market.ListQueueCompare != null)
                {
                    for (int i = Market.ListQueueCompare.Count - 1; i >= 0; i--)
                    {
                        TimeSpan executionTime = DateTime.Now - Market.ListQueueCompare[i].TimeClientRequest;
                        if (executionTime.Minutes >= 1.6)
                        {
                            codeDealer += "C_" + Market.ListQueueCompare[i].AgentID.ToString() + "_";
                            codeInvestor += "C_" + Market.ListQueueCompare[i].InvestorID.ToString() + "_";

                            Market.ListQueueCompare.RemoveAt(i);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Delete request expire Invetor_" + codeInvestor + "Agent_" + codeDealer, ex.Message, "123", "");
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
            if (requestDealer.Name == "Open")
            {
                result.Append("AddCommand$");
            }
            else
            {
                result.Append("CloseCommand$");
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

            #endregion
            if (requestDealer.Request.Investor.ClientCommandQueue == null) requestDealer.Request.Investor.ClientCommandQueue = new List<string>();
            requestDealer.Request.Investor.ClientCommandQueue.Add(result.ToString());
        }

        /// <summary>
        /// 2 minutes Expired
        /// </summary>
        internal void RemoveDealerExpire()
        {
            if (Market.AgentList.Count == 0) return;
            string notiAgent = "";
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (!Market.AgentList[i].IsVirtualDealer)
                {

                    TimeSpan executionTime = DateTime.Now - Market.AgentList[i].TimeSync;
                    if (!Market.AgentList[i].IsBusy)
                    {
                        if (executionTime.Minutes >= 1)
                        {
                            Market.AgentList[i].IsBusy = true;
                            if (Business.Agent.NoticeDealer.ContainsKey(Market.AgentList[i].AgentID))
                            {
                                notiAgent = Business.Agent.NoticeDealer[Market.AgentList[i].AgentID];
                                if (notiAgent.IndexOf("NA02") < 0)
                                {
                                    Business.Agent.NoticeDealer[Market.AgentList[i].AgentID] += "NA02,";
                                    string content = "'" + Market.AgentList[i].Code + "': dealer dispatched disconnected ";
                                    string comment = "[logout dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, "", Market.AgentList[i].Code);
                                }
                            }
                        }
                    }
                    if (executionTime.Minutes >= 1)
                    {
                        Market.AgentList[i].IsOnline = false;
                        Facade.FacadeSendNoticeManagerOnline(Market.AgentList[i]);
                    }

                }
            }
        }
        #endregion       

        #region VirtualDealer
        /// <summary>
        /// 
        /// </summary>
        internal void IniVirtualDealer()
        {
            List<Business.VirtualDealer> result = TradingServer.Facade.FacadeGetVirtualDealer();
            for (int i = 0; i < result.Count; i++)
            {
                Business.Agent agent = new Agent();
                agent.LoginVirtualDealer(result[i]);
            }
        }

        #endregion

        /// <summary>
        /// GET TICK ONLINE IN CLASS MARKET OF SYMBOL LIST
        /// </summary>
        /// <returns></returns>
        internal List<Business.Tick> GetTickOnline()
        {
            List<Business.Tick> Result = new List<Tick>();
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].TickValue != null)
                    {
                        Result.Add(Business.Market.SymbolList[i].TickValue);
                    }
                    else
                    {                        
                        Business.Tick newTick = new Business.Tick();
                        newTick.Ask = 0;
                        newTick.Bid = 0;
                        newTick.HighAsk = 0;
                        newTick.HighInDay = 0;
                        newTick.LowAsk = 0;
                        newTick.LowInDay = 0;
                        newTick.Status = "down";
                        newTick.SymbolName = Business.Market.SymbolList[i].Name;
                        newTick.TickTime = DateTime.Now;
                        newTick.SymbolID = Business.Market.SymbolList[i].SymbolID;

                        Result.Add(newTick);
                    }
                }
            }

            return Result;
        }
    }   //End Class Market
}   //End NameSpace
