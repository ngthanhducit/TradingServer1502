using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.EnumMT4
{
    public enum Orders
    {
        GOOD_TILL_TODAY_INCLUDING_SL_TP,
        GOOD_TILL_CANCELLED,
        GOOD_TILL_TODAY_EXCLUDING_SL_TP
    }
}
