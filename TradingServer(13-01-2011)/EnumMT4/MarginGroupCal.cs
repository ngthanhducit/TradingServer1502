using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.EnumMT4
{
    public enum MarginGroupCal
    {
        DO_NOT_USE_UNREALIZED_PROFIT_LOSS,
        USE_UNREALIZED_PROFIT_LOSS,
        USE_UNREALIZED_PROFIT_ONLY,
        USE_UNREALIZED_LOSS_ONLY
    }
}
