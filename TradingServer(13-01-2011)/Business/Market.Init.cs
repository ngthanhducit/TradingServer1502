using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// 
        /// </summary>
        private void InitConnectMT4()
        {
            Business.Market.IsConnectMT4 = true;
            Business.Market.StatusConnect = true;
            Business.Market.NotifyMessageFromMT4 = new List<string>();
            Business.Market.NotifyTickFromMT4 = new List<string>();
            Business.Market.NotifyTickFromManagerAPI = new List<ManagerAPITick>();

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

            //INIT THREAD SOCKET SERVER RECEIVE COMMAND FROM MT4(port 3000)
            Element5SocketConnectMT4.Business.SocketConnectAsync instanceSocketAsync = new Element5SocketConnectMT4.Business.SocketConnectAsync();
            instanceSocketAsync.NotifyMT4 = this.ReceiveNotify;
            instanceSocketAsync.StartListener(DEFAULT_PORTASYNC, DEFAULT_IPADDRESS);

            //INIT THREAD SOCKET SERVER RECEIVE TICK FROM MT4(port 5000)
            Element5SocketConnectMT4.Business.SocketConnectTickAsync instanceSocketTickAsync = new Element5SocketConnectMT4.Business.SocketConnectTickAsync();
            instanceSocketTickAsync.NotifyTickMT4 = this.ReceiveTickNotify;
            instanceSocketTickAsync.StartListener(DEFAULT_PORTASYNC_TICK, DEFAULT_IPADDRESS);

            //START SOCKET SYNC SERVER(send command from et5 to manager api)(port 2000)
            Business.Market.InstanceSocket.StartListening(DEFAULT_IPADDRESS, DEFAULT_PORT);

            //list store command send to nj4x client
            Business.Market.NJ4XTickets = new List<NJ4XConnectSocket.NJ4XTicket>();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadProcessLastAccount()
        {
            Business.Market.IsProcessLastAccount = true;
            Business.Market.ListLastAccount = new List<SumLastAccount>();
            Business.Market.ThreadLastAccount = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessLastBalance));
            Business.Market.ThreadLastAccount.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadProcessStatementEOD()
        {
            Business.Market.IsProcessAddStatement = true;
            Business.Market.ListStatementEOD = new List<StatementInvestor>();

            Business.Market.ThreadStatement = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessStatementEOD));
            Business.Market.ThreadStatement.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadSaveStatement()
        {
            Business.Market.IsProcessSaveStatement = true;
            Business.Market.ListStatement = new List<Statement>();
            Business.Market.ThreadSaveStatement = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessSaveStatement));
            Business.Market.ThreadSaveStatement.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitAgentSystem()
        {
            Business.Market.ListAgentReport = new List<AgentReport>();

            Business.Market.ListEODAgent = new List<EndOfDayAgent>();

            //INIT WCF

            //Business.Market.ListConfigAdminMaster = new List<TradingServer.Agent.IAdminMaster>();
            //Business.Market.ListConfigMasterAgent = new List<TradingServer.Agent.IMasterAgentSymbol>();
            //Business.Market.ListConfigAgentInvestor = new List<TradingServer.Agent.IAgentInvestorSymbol>();
            Business.Market.ListTickQueueAgent = new List<string>();
            Business.Market.ListNotifyQueueAgent = new List<AgentNotify>();

            Business.Market.IsProcessTickAgent = true;
            Business.Market.IsProcessNotifyAgent = true;

            Business.Market.threadProcessTickAgent = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessTickQueueAgent));
            Business.Market.threadProcessTickAgent.Start();

            Business.Market.threadProcessNotifyAgent = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessNotifyQueueAgent));
            Business.Market.threadProcessNotifyAgent.Start();

            Business.Market.ListAgentConfig = new List<TradingServer.Agent.AgentConfig>();

            //init agent wcf address
            this.InitAgent();

            Business.Market.BKListNotifyQueueAgent = new List<AgentNotify>();
            Business.Market.ListHistoryAgent = new List<AgentReport>();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadProcessLog()
        {
            Business.Market.ListSystemLog = new List<SystemLog>();
            Business.Market.IsProcessLog = true;

            Business.Market.ThreadSystemLog = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ProcessSystemLog));
            Business.Market.ThreadSystemLog.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadRemoveTrade()
        {
            Business.Market.IsRemoveCommand = true;
            Business.Market.RemoveCommandList = new List<OpenRemove>();

            Business.Market.ThreadRemoveCommand = new System.Threading.Thread(new System.Threading.ThreadStart(Business.Market.ThreadRemoveOpenTrade));
            ThreadRemoveCommand.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadCheckMultiPrice()
        {
            //Init Value Multiple Quotes
            Business.Market.MultiplePriceQuotes = new List<PriceServer>();

            Business.Market.ThreadMultiplePrice = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadCheckMultiplePrice));
            Business.Market.ThreadMultiplePrice.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitThreadCheckOnlineInvestor()
        {
            Business.Market.ThreadInvestorOnline = new System.Threading.Thread(new System.Threading.ThreadStart(TimeEventCheckInvestorOnline));
            Business.Market.ThreadInvestorOnline.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitManagerAdmin()
        {
            //Init Agent
            Market.AgentList = new List<Agent>();
            Market.AdminList = new List<Agent>();

            //Init Request Dealer
            Business.Market.ListRequestDealer = new List<RequestDealer>();
            Business.Market.ListQueueCompare = new List<RequestDealer>();

            //Init Process Agent
            Business.Market.ThreadAgentRecycle = new System.Threading.Thread(new System.Threading.ThreadStart(CleanRecycleRequest));
            Business.Market.ThreadAgentRecycle.Start();
            this.IniVirtualDealer();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitEvent()
        {   
            Business.Market.DayEvent = new List<TimeEvent>();
            Business.Market.WeekEvent = new List<TimeEvent>();
            Business.Market.YearEvent = new List<TimeEvent>();
            Business.Market.FutureEvent = new List<TimeEvent>();

            this.EndOfDay = new DateTimeEvent();

            //Init Event
            this.InitTimeEventInSymbol();
            this.InitTimeEventServer();
            this.InitSymbolFuture();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitSymbolList()
        {
            Market.QuoteList = new List<Business.QuoteSymbol>();

            Market.SymbolList = new List<Symbol>();
            //Market.SymbolList = TradingServer.Facade.FacadeGetAllSymbol();

            Business.Market.Symbols = new Dictionary<string, Symbol>();
            Business.Market.QuoteSymbols = new Dictionary<string, QuoteSymbol>();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitIGroupSymbol()
        {
            //Init IGroupSymbol
            Market.IGroupSymbolList = new List<IGroupSymbol>();
            Market.IGroupSymbolList = TradingServer.Facade.FacadeGetAllIGroupSymbol();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitOpenTrade()
        {   
            Business.Market.CommandExecutor = new List<OpenTrade>();

            //Init Online Command            
            TradingServer.Facade.FacadeGetAllOpenTrade();
            TradingServer.Facade.FacadeInitOpenTrade();

            //Calculation margin All Open Trade
            TradingServer.Facade.FacadeReCalculationAccount();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitIGroupSecurity()
        {
            //Init IGroupSecurity
            Market.IGroupSecurityList = new List<IGroupSecurity>();
            Market.IGroupSecurityList = TradingServer.Facade.FacadeGetIGroupSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitInvestorList()
        {
            Business.Market.InvestorOnline = new List<Investor>();

            //Init Investor
            Market.InvestorList = new List<Investor>();
            //Market.InvestorList = TradingServer.Facade.FacadeGetAllInvestor();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitSecurityList()
        {   
            //Init List Security
            Market.SecurityList = new List<Business.Security>();
            Market.SecurityList = TradingServer.Facade.FacadeGetAllSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGroupList()
        {
            //Init List Group Default
            Business.Market.ListGroupDefault = new List<GroupDefault>();

            //Init Investor Group List
            Market.InvestorGroupList = new List<InvestorGroup>();
            Market.InvestorGroupList = TradingServer.Facade.FacadeGetAllInvestorGroup();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMarketArea()
        {
            //Init MarketArea
            Market.MarketArea = new List<IPresenter.IMarketArea>();
            Market.MarketArea = Market.DBWMarketArea.GetAllMarketArea();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMarketConfig()
        {
            //Init Market Config
            Market.MarketConfig = TradingServer.Facade.FacadeGetAllMarketConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitCommandType()
        {
            //Init Command Type
            TradingServer.Facade.FacadeGetAllCommandType();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitNews()
        {
            //Init News
            Market.NewsList = new List<News>();
            TradingServer.Facade.FacadeGetTopNews();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoopGetCandlesManagerAPI()
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
            if (timeCandlesFive.DayOfWeek == DayOfWeek.Saturday)
                timeCandlesFive = timeCandlesFive.AddDays(-1);

            if (timeCandlesFive.DayOfWeek == DayOfWeek.Sunday)
                timeCandlesFive = timeCandlesFive.AddDays(-2);

            string cmd = "GetCandlesByDate$" + symbols + "|" + timeCandles;
            string resultAPI = Business.Market.InstanceSocket.SendSocket(cmd);
            Business.Market.CandlesByDate = Model.BuildCommand.Instance.MapStringToDicCandle(resultAPI);

            string cmdOneDay = "GetCandlesByDate$" + symbols + "|" + timeCandlesOne;
            string OneDay = Business.Market.InstanceSocket.SendSocket(cmdOneDay);
            Business.Market.CandlesByDateOneDay = Model.BuildCommand.Instance.MapStringToDicCandle(OneDay);

            string cmdFiveDay = "GetCandlesByDate$" + symbols + "|" + timeCandlesFive;
            string FiveDay = Business.Market.InstanceSocket.SendSocket(cmdFiveDay);
            Business.Market.CandlesByDateFiveDay = Model.BuildCommand.Instance.MapStringToDicCandle(FiveDay);

            //==============================================================================================

            Business.Market.EventGetCandles = new TimeEvent();
            EventGetCandles.EventType = TimeEventType.BeginSwap;
            EventGetCandles.TimeEventID = 0;

            Business.TargetFunction newTargetFunction = new TargetFunction();
            newTargetFunction.EventPosition = "All";
            newTargetFunction.Function = this.EventGetCandlesByDate;

            EventGetCandles.TargetFunction = new List<TargetFunction>();
            EventGetCandles.TargetFunction.Add(newTargetFunction);

            Business.DateTimeEvent newDateTimeEvent = new DateTimeEvent();
            newDateTimeEvent.Day = -1;
            newDateTimeEvent.Month = -1;
            newDateTimeEvent.DayInWeek = DateTime.Now.DayOfWeek;
            newDateTimeEvent.Hour = 0;
            newDateTimeEvent.Minute = 5;

            EventGetCandles.Time = newDateTimeEvent;
            EventGetCandles.IsEnable = true;
            DateTime _tempTime = DateTime.Now.AddDays(1);
            DateTime newDateTime = new DateTime(_tempTime.Year, _tempTime.Month, _tempTime.Day, newDateTimeEvent.Hour, newDateTimeEvent.Minute, 00);

            EventGetCandles.TimeExecution = newDateTime;

            TimeSpan _timeSpand = EventGetCandles.TimeExecution - DateTime.Now;
            this.TimeEventExecuteCandles(_timeSpand.TotalMilliseconds);
        }
    }
}
