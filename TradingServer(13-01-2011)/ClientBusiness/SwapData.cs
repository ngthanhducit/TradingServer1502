using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    public class SwapData
    {
        public bool IsTimer { get; set; }
        public TimeSpan TimeSpanSwap { get; set; }
        public DateTime TimeSwap { get; set; }
        public DateTime TimeSleepSwaps { get; set; }
        public DayOfWeek ThreeSwap { get; set; }
        public DateTime RefDateTime { get; set; }
    }
}
