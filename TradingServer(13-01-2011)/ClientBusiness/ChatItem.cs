using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TradingServer.ClientBusiness
{
    [DataContract]
    public class ChatItem
    {
        [DataMember]
        public int InvestorID
        {
            get;
            set;
        }
        [DataMember]
        public string InvestorName
        {
            get;
            set;
        }
        [DataMember]
        public int ToInvestorID
        {
            get;
            set;
        }
        [DataMember]
        public string ToInvestorName
        {
            get;
            set;
        }
        [DataMember]
        public DateTime Time
        {
            get;
            set;
        }
        [DataMember]
        public string Message
        {
            get;
            set;
        }
        [DataMember]
        public string IPAddress
        {
            get;
            set;
        }
        [DataMember]
        public int NumberUpdate
        {
            get;
            set;
        }
    }
}
