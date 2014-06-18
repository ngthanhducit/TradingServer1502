using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    public class BinaryTradingData
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimePause { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime TimeNext { get; set; }

        public double TimeNowToEnd { get; set; }

        public int NumberChange { get; set; }
        public List<string> ClientCommand { get; set; }

        public Dictionary<string, TradingServer.Business.Tick> priceStart { get; set; }
        public Dictionary<string, TradingServer.Business.Tick> priceStop { get; set; }

        public ClientBusiness.StatusBinaryTrading StatusBinary { get; set; }
    }
}
