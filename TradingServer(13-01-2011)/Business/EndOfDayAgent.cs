using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class EndOfDayAgent
    {
        public int InvestorID { get; set; }
        public double MonthVolume { get; set; }
        public double FloatingPL { get; set; }
    }
}
