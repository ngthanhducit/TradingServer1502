using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{    
    public class TimeEvent 
    {
        public int TimeEventID { get; set; }
        public TimeEventType EventType { get; set; }
        public Business.DateTimeEvent Time { get; set; }
        public List<TargetFunction> TargetFunction { get; set; }
        public DateTime TimeExecution { get; set; }
        public bool Every { get; set; }
        public bool IsEnable { get; set; }
        public int NumSession { get; set; }

        public TimeEvent()
        {
            this.NumSession = 0;
        }
    }
}
