using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.EnumMT4
{
    public enum IGroupSecurityExecution
    {
        MANUAL_ONLY_NO_AUTOMATIC,
        AUTOMATIC_ONLY,
        MANUAL_BUT_AUTOMATIC_IF_NO_DEALERS_ONLINE
    }
}
