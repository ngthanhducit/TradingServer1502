﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.NJ4XConnectSocket
{
    public class NJ4XTicket
    {
        public int Ticket { get; set; }
        public string Code { get; set; }
        public string ClientCode { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double OpenPrice { get; set; }
        public string Symbol { get; set; }
        public bool IsClose { get; set; }
        public bool IsReQuote { get; set; }
        public bool IsDisable { get; set; }
        public bool IsRequest { get; set; }
        public bool IsUpdate { get; set; }
        public int TimeOut { get; set; }
        public double OpenPrices { get; set; }
        public double ClosePrices { get; set; }
        public int Digit { get; set; }
        public EnumMT4.Execution Execution { get; set; }
        public Business.OpenTrade Command { get; set; }
    }
}
