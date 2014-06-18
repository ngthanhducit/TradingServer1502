using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class DateTimeEvent
    {
        public int Minute { get; set; }
        public int Hour { get; set; }
        public DayOfWeek DayInWeek { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
