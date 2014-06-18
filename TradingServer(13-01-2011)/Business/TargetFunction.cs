using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public delegate void DelegateTargetFunction(string TargetName,Business.TimeEvent timeEvent);
    public class TargetFunction
    {
        public string EventPosition { get; set; }
        public int EventID { get; set; }
        public int NumSession { get; set; }
        public DelegateTargetFunction Function { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TargetFunction()
        {
            this.EventID = 0;
        }
    }
}
