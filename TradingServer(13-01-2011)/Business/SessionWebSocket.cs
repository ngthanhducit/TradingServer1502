﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class SessionWebSocket
    {
        public int InvestorID { get; set; }
        public string InvestorCode { get; set; }
        public string Key { get; set; }
        public Business.TypeLogin LoginType { get; set; }
    }
}
