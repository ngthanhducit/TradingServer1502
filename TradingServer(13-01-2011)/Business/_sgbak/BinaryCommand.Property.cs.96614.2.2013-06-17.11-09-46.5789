using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class BinaryCommand
    {
        //private static List<TradingServiceV3.QuotesWCF.Tick> ListTick { get; set; }

        //private static BinaryTrading newBinaryTrading;

        static double TotalSecondNowToPause { get; set; }
        public static double TotalSecondNowToEnd { get; set; }
        static double TotalSecondNowToNext { get; set; }
        static double TotalSecondNowToStart { get; set; }
        static double TotalSecondNowToDelegate { get; set; }
        static double TotalSecondStartToPause { get; set; }
        static double TotalSecondStartToEnd { get; set; }

        public static bool isStart = true;
        public static bool isTrade = true;
        public static bool isPause = false;

        bool isGetPriceStart = false;
        bool isNewSession = true;

        public static ClientBusiness.StatusBinaryTrading StatusBinary { get; set; }

        public static DateTime timeStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 04, 00, 00);
        public static DateTime TimeStartMarket
        {
            get
            {
                return timeStart;
            }
            set
            {
                BinaryCommand.timeStart = value;
            }
        }

        public static DateTime timeClose = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 00, 00);
        public static DateTime TimeCloseMarket
        {
            get
            {
                return BinaryCommand.timeClose;
            }
            set
            {
                BinaryCommand.timeClose = value;
            }
        }

        static DateTime TimeCurrent { get; set; }
        public static DateTime TimeStart { get; set; }
        static DateTime TimeDelegate { get; set; }
        public static DateTime TimePause { get; set; }
        public static DateTime TimeNext { get; set; }
        public static DateTime TimeEnd { get; set; }

        public static System.Threading.Thread refeshThread { get; set; }

        public static Dictionary<string, Business.Tick> PriceStartSession { get; set; }
        public static Dictionary<string, Business.Tick> PriceStopSession { get; set; }
    }
}
