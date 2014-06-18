using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TradingServer.Business
{
    public partial class Market : IPresenter.IMarketArea, IPresenter.ICommand
    {
        #region PROPERTY WCF    
        public static Trading.TradingWCFClient client;
        public static PriceQuotes.PriceQuotesClient clientQuote;
        public static AgentWCF.DefaultWCFClient clientDefault;
        #endregion

        #region Property Implement From Class Market
        internal static int SecurityTickClient = 0;        
        internal static List<Business.ParameterItem> MarketConfig { get; set; }        
        internal static List<Business.OpenTrade> CommandExecutor { get; set; }
        internal static List<Business.OpenTrade> PendingOrders { get; set; }
        internal static List<Business.Investor> InvestorList { get; set; }
        internal static List<Business.Agent> AgentList { get; set; }
        internal static List<Business.Agent> AdminList { get; set; }
        internal static List<Business.News> NewsList { get; set; }
        internal static List<Business.RequestDealer> ListRequestDealer { get; set; }
        internal static List<Business.RequestDealer> ListQueueCompare { get; set; }
        public static bool IsOpen { get; set; }
        internal static bool IsTickUpdate { get; set; }
        internal static List<Business.QuoteSymbol> QuoteList { get; set; }
        internal static Dictionary<string, Business.QuoteSymbol> QuoteSymbols { get; set; }
        internal static List<Business.InvestorGroup> InvestorGroupList { get; set; }
        internal static List<Business.Security> SecurityList { get; set; }
        internal static List<Business.Symbol> SymbolList { get; set; }
        internal static Dictionary<string, Business.Symbol> Symbols { get; set; }
        internal static List<IPresenter.IMarketArea> MarketArea { get; set; }
        internal static List<Business.TradeType> TradeTypeList { get; set; }
        internal static List<Business.IGroupSecurity> IGroupSecurityList { get; set; }
        internal static List<Business.IGroupSymbol> IGroupSymbolList { get; set; }
        internal Double TimeLeftWeek { get; set; }
        internal static List<Business.OpenTrade> listTempOrder { get; set; }
        internal static List<Business.OpenTrade> listTempReport { get; set; }
        public static int TickTimeOut { get { return 3000; } }
        public static List<Business.GroupDefault> ListGroupDefault { get; set; }
        #endregion        

        #region Property Time Event
        internal static List<Business.TimeEvent> DayEvent { get; set; }
        internal static List<Business.TimeEvent> WeekEvent { get; set; }
        internal static List<Business.TimeEvent> YearEvent { get; set; }
        internal static List<Business.TimeEvent> FutureEvent { get; set; }
        
        internal static Timer TimerEventDay { get; set; }
        internal static Timer TimerEventWeek { get; set; }
        internal static Timer TimerEventYear { get; set; }
        internal static Timer TimerEventFuture { get; set; }
        
        private static System.Threading.Thread ThreadMultiplePrice { get; set; }
        internal static System.Threading.Thread ThreadAgentRecycle { get; set; }
   
        internal Double TimeLeft { get; set; }
        internal Double TimeLeftFuture { get; set; }
        internal Business.DateTimeEvent EndOfDay { get; set; }
        internal static bool IsExecutorDay { get; set; }
        internal static bool IsExecutorWeek { get; set; }
        internal static bool IsExecutorYear { get; set; }
        internal static bool IsFirstStart { get; set; }
        internal static DateTime TimeEndDay { get; set; }
        
        public static List<Business.Investor> InvestorOnline { get; set; }

        public static DateTime EndDayTime { get; set; } //time setting event end day
        
        public static bool IsRealSearver { get; set; }
        #endregion

        #region Property Implement From Interface IMarketArea
        public int IMarketAreaID { get; set; }
        public Market MarketContainer { get; set; }
        List<TradeType> IPresenter.IMarketArea.Type { get; set; }
        public IPresenter.AddCommandDelegate AddCommandNotify { get; set; }
        public string IMarketAreaName { get; set; }
        public List<Symbol> ListSymbol { get; set; }
        public List<ParameterItem> MarketAreaConfig { get; set; }
        #endregion 

        #region PROPERTY ISBUSY OF TEMPORDER
        //it using then manager get order, then isbusy = true
        public static bool IsBusy { get; set; }
        #endregion

        #region property templist order
        internal static List<Business.OrderInvestor> TempListOrder { get; set; }
        #endregion

        #region string send mail
        public static string LogContentSendMail { get; set; }
        public static string LogContentSendMailMonth { get; set; }
        #endregion

        #region property check valid ip address
        internal static List<List<List<List<List<bool>>>>> BlockIpAddress { get; set; }
        #endregion

        #region property check multiple price quotes
        internal static System.Threading.Thread ThreadInvestorOnline { get; set; }        
        internal static int TimeCheckMultiplePrice { get; set; }
        internal static List<Business.PriceServer> MultiplePriceQuotes { get; set; }
        internal static bool IsMultipleQuote { get; set; }
        internal static bool isBlock { get; set; }
        #endregion        

        #region property thread close command
        internal static System.Threading.Thread ThreadRemoveCommand { get; set; }
        internal static List<Business.OpenRemove> RemoveCommandList { get; set; }
        public static bool IsRemoveCommand { get; set; }
        #endregion   
     
        #region property thread delete pending order
        internal static Timer TimerExpirePending { get; set; }
        #endregion

        #region property lock object
        internal static object syncObject = new object();
        #endregion

        #region property lock nj4x ticket
        internal static object nj4xObject = new object();
        #endregion

        internal List<NJ4XConnectSocket.MQLCommand> MQLCommands { get; set; }

        #region property process add system log
        internal static List<Business.SystemLog> ListSystemLog { get; set; }
        internal static System.Threading.Thread ThreadSystemLog { get; set; }
        internal static bool IsProcessLog { get; set; }
        #endregion

        #region property thread insert statement
        private static bool IsProcessAddStatement { get; set; }
        private static List<Business.StatementInvestor> ListStatementEOD { get; set; }
        private static System.Threading.Thread ThreadStatement { get; set; }
        #endregion

        #region property thread process last account
        private static bool IsProcessLastAccount { get; set; }
        private static List<Business.SumLastAccount> ListLastAccount { get; set; }
        private static System.Threading.Thread ThreadLastAccount { get; set; }
        #endregion

        #region process save statement to database
        private static bool IsProcessSaveStatement { get; set; }
        private static List<Business.Statement> ListStatement { get; set; }
        private static System.Threading.Thread ThreadSaveStatement { get; set; }
        #endregion

        #region property connect to mt4 using socket
        internal static Element5SocketConnectMT4.Business.SocketConnect InstanceSocket { get; set; }
        internal static Element5SocketConnectMT4.Business.SocketConnectAsync InstanceSocketAsync { get; set; }
        internal static Element5SocketConnectMT4.Business.SocketConnectTickAsync InstanceSocketTickAsync { get; set; }
        public static List<string> NotifyMessageFromMT4 { get; set; }
        public static List<string> NotifyTickFromMT4 { get; set; }
        public static List<Business.ManagerAPITick> NotifyTickFromManagerAPI { get; set; }
        private static System.Threading.Thread ThreadProcessNotifyMessage { get; set; }
        private static System.Threading.Thread ThreadProcessNotifyTickMT4 { get; set; }
        private static bool IsProcessNotifyMessage { get; set; }
        private static bool IsProcessNotifyTick { get; set; }
        public static bool IsConnectMT4 { get; set; } 
        internal static bool StatusConnect { get; set; }

        internal const string DEFAULT_IPADDRESS_LOCAL = "127.0.0.1";
        internal const int DEFAULT_NJ4X = 9999;

        internal const string DEFAULT_IPADDRESS = "127.0.0.1";
        //internal const string DEFAULT_IPADDRESS = "118.69.62.203";
        //internal const string DEFAULT_IPADDRESS = "202.150.222.239";
        //internal const string DEFAULT_IPADDRESS = "127.0.0.1";
        
        internal const int DEFAULT_PORT = 2000;
        internal const int DEFAULT_PORTASYNC = 3000;
        //internal const int DEFAULT_PORTASYNC_CLIENT = 44000;
        internal const int DEFAULT_PORTASYNC_TICK = 5000;

        //internal const string DEFAULT_IPADDRESS = "202.150.222.196";
        //internal const int DEFAULT_PORT = 2000;
        //internal const int DEFAULT_PORTASYNC = 3000;

        //internal const string DEFAULT_IPADDRESS = "202.150.222.239";
        //internal const int DEFAULT_PORT = 2000;
        //internal const int DEFAULT_PORTASYNC = 3000;

        //internal const string DEFAULT_IPADDRESS = "202.150.222.238";
        //internal const int DEFAULT_PORT = 4000;
        //internal const int DEFAULT_PORTASYNC = 5000;

        //internal const string DEFAULT_IPADDRESS = "172.16.7.201";
        //internal const int DEFAULT_PORT = 22000;
        //internal const int DEFAULT_PORTASYNC = 33000;

        //internal const string DEFAULT_IPADDRESS = "192.168.1.204";
        //internal const int DEFAULT_PORT = 2000;
        //internal const int DEFAULT_PORTASYNC = 3000;
        
        //internal static Business.GlobalDelegate InstanceGlobalDelegate { get; set; }
        #endregion

        #region property connect nj4xconnect
        public static List<NJ4XConnectSocket.NJ4XTicket> NJ4XTickets { get; set; }
        #endregion

        #region property agent wcf
        internal static List<string> ListTickQueueAgent { get; set; }
        internal static System.Threading.Thread threadProcessTickAgent { get; set; }
        internal static bool IsProcessTickAgent { get; set; }
        internal static List<Business.AgentNotify> ListNotifyQueueAgent { get; set; }
        internal static List<Business.AgentNotify> BKListNotifyQueueAgent { get; set; }
        internal static System.Threading.Thread threadProcessNotifyAgent { get; set; }
        internal static bool IsProcessNotifyAgent { get; set; }
        internal static List<TradingServer.Agent.AgentConfig> ListAgentConfig { get; set; }
        internal static bool IsConnectAgent { get; set; }
        #endregion

        #region property delay send mail warning margin call
        public static int TimeDelaySendWarningMarginCall { get; set; }
        #endregion

        #region property get report agent
        public static List<Business.AgentReport> ListAgentReport { get; set; }
        public static List<Business.AgentReport> ListHistoryAgent { get; set; }
        #endregion

        #region property end of day agent
        public static List<Business.EndOfDayAgent> ListEODAgent { get; set; }
        #endregion

        public static bool IsCheckScalper { get; set; }
        public static double ScalperPipValue { get; set; }
        public static double ScalperTimeValue { get; set; }

        #region property config mail template
        public static string CompanyWebsite { get; set; }
        public static string ServiceName { get; set; }
        public static string MailSupport { get; set; }
        public static string MailContact { get; set; }
        public static string CompanyName { get; set; }
        public static string About { get; set; }
        public static string CompanyCopyright { get; set; }
        public static string AccessLink { get; set; }
        #endregion

        #region property admin and manager request client log
        internal static bool InRequestClientLog { get; set; }
        public static List<Business.ClientLog> ListClientLogs { get; set; }
        #endregion

        #region init manager api using dll(20/02/2014)
        [DllImport(@"C:\Users\Nguyen\Source\Workspaces\MT4ManagerAPI\CPlusToCSharp\MFCLibrary1\Release\MFCLibrary1.dll")]
        public static extern void StartAPI(CallbackDelegate notifyInfo, CallbackDelegateTick notifyTick, CallbackDelegate notifyLog);
        [DllImport(@"C:\Users\Nguyen\Source\Workspaces\MT4ManagerAPI\CPlusToCSharp\MFCLibrary1\Release\MFCLibrary1.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]

        //[DllImport("C:\\Users\\Duong\\Desktop\\MT4ManagerAPI\\CPlusToCSharp\\MFCLibrary1\\Release\\MFCLibrary1.dll")]
        //public static extern void StartAPI(CallbackDelegate notifyInfo, CallbackDelegateTick notifyTick, CallbackDelegate notifyLog);
        //[DllImport("C:\\Users\\Duong\\Desktop\\MT4ManagerAPI\\CPlusToCSharp\\MFCLibrary1\\Release\\MFCLibrary1.dll",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr CommandExecute(string code);

        public delegate void CallbackDelegate(string lCode);
        public delegate void CallbackDelegateTick(string symbol, int ipDown, string time, double bid, double ask, double high, double low, double spread);
        public static CallbackDelegate NotifyInfo = new CallbackDelegate(onCallbackInfo);
        public static CallbackDelegateTick NotifyTick = new CallbackDelegateTick(onCallbackTick);
        public static CallbackDelegate NotifyLog = new CallbackDelegate(onCallbackLog);
        #endregion

        internal static Timer TimerEventCandles { get; set; }

        public static Business.TimeEvent EventGetCandles { get; set; }

        internal static Dictionary<string, string> CandlesByDate { get; set; }
        internal static Dictionary<string, string> CandlesByDateOneDay { get; set; }
        internal static Dictionary<string, string> CandlesByDateFiveDay { get; set; }

    }
}
