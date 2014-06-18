using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class QuoteSymbol : IPresenter.ICalculatorClient
    {
        public string Name { get; set; }
        public List<Business.Symbol> RefSymbol { get; set; }
        //public Business.Tick TickValue { get; set; }
        public List<Business.Tick> Ticks { get; set; }
        public bool IsUpdated { get; set; }

        
        //public Tick Tick { get; set; }


        public TaskComplete TaskWork
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public TaskDelegate TaskJob
        {
            get;
            set;
        }

        public string TaskName
        {
            get;
            set;
        }
        
        public bool IsActive
        {
            get;
            set;
        }
    }
}
