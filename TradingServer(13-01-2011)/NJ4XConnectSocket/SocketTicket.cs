using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.NJ4XConnectSocket
{
    public class SocketTicket
    {
        public int Ticket { get; set; }
        public bool IsDisable { get; set; }
        public bool IsSuccess { get; set; }
        public string Cmd { get; set; }
        public string CmdResult { get; set; }
        public int TimeOut { get; set; }
        public Business.OpenTrade Command { get; set; }
    }
}
