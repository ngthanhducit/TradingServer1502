using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    public class ChatDelivery
    {
        internal static List<ClientBusiness.ChatItem> ChatQueue { get; set; }
        internal static List<ClientBusiness.Friends> ListFriends { get; set; }
        internal static List<ClientBusiness.RequestFriends> ListFriendsRequest { get; set; }
    }
}
