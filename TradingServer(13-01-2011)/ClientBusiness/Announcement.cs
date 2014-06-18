using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    public class Announcement
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int NumUpdate { get; set; }
    }
}
