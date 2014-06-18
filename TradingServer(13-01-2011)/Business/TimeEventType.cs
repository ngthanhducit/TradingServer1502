using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public enum TimeEventType
    {
        BeginCloseMarket,
        BeginHoliday,
        BeginMaintain,
        BeginCloseQuoteSymbol,
        BeginCloseTradeSymbol,
        EndCloseQuoteSymbol,
        EndCloseTradeSymbol,
        EndCloseMarket,
        EndHoliday,
        EndMaintain,
        BeginSwap,
        EndSwap,
        BeginWork,
        EndWork,
        Statements,
        StatementsMonth,
        SettingOrder,
        TimeSymbolExp,
        IsCloseFutureEvent,
        ProcessFutureEvent,
    }
}
