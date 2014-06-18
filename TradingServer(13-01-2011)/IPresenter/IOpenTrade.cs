using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.IPresenter
{
    interface IOpenTrade
    {
        double ClosePrice { get; set; }
        DateTime CloseTime { get; set; }        
        DateTime ExpTime { get; set; }
        int ID { get; set; }
        Business.Investor Investor { get; set; }
        double OpenPrice { get; set; }
        DateTime OpenTime { get; set; }
        double Size { get; set; }
        double StopLoss { get; set; }
        Business.Symbol Symbol { get; set; }
        double TakeProfit { get; set; }
        Business.TradeType Type { get; set; }        
        double Commission { get; set; }
    }
}
