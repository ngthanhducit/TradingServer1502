using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public enum ClientCommand
    {
        BuyCommand,
        SellCommand,
        BuyStop,
        BuyLimit,
        SellStop,
        SellLimit,
        UpdateOnlineCommand,
        CloseCommand
    }
}
