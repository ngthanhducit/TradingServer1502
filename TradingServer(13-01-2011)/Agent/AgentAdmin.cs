using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Agent
{
    public class AgentAdmin
    {
        public int AdminID { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string NickName { get; set; }
        public DateTime CreateTime { get; set; }
        //public List<Business.Master> ListMaster { get; set; }
    }
}
