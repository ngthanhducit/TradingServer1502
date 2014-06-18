using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.ClientBusiness
{
    internal class PChatDelivery
    {
        ///// <summary>
        ///// Constructure
        ///// </summary>
        //internal PChatDelivery()
        //{
        //    TradingServer.ClientBusiness.ChatDelivery.ChatQueue = new List<ChatItem>();
        //    TradingServer.ClientBusiness.ChatDelivery.ListFriends = new List<Friends>();
        //}

        ///// <summary>
        ///// Get All Investor Online
        ///// </summary>
        ///// <returns>List<TradingServiceV3.Business.Investor></returns>
        ////internal List<TradingServiceV3.Business.Investor> GetAllInvestor()
        ////{
        ////    List<TradingServiceV3.Business.Investor> listInvestorOnlineChat = new List<Business.Investor>();
        ////    if (TradingServiceV3.Business.Market.OnlineInvestor == null)
        ////        return null;

        ////    int count = TradingServiceV3.Business.Market.OnlineInvestor.Count;
        ////    for (int i = 0; i < count; i++)
        ////    {
        ////        if (TradingServiceV3.Business.Market.OnlineInvestor[i].IsOnlineChat == true)
        ////        {
        ////            TradingServiceV3.Business.Investor newInvestor = new Business.Investor();
        ////            newInvestor.Balance = TradingServiceV3.Business.Market.OnlineInvestor[i].Balance;
        ////            newInvestor.Code = TradingServiceV3.Business.Market.OnlineInvestor[i].Code;
        ////            newInvestor.Equity = TradingServiceV3.Business.Market.OnlineInvestor[i].Equity;
        ////            newInvestor.FreeMargin = TradingServiceV3.Business.Market.OnlineInvestor[i].FreeMargin;
        ////            newInvestor.FullName = TradingServiceV3.Business.Market.OnlineInvestor[i].FullName;
        ////            newInvestor.InvestorID = TradingServiceV3.Business.Market.OnlineInvestor[i].InvestorID;
        ////            newInvestor.IsOnlineChat = TradingServiceV3.Business.Market.OnlineInvestor[i].IsOnlineChat;

        ////            listInvestorOnlineChat.Add(newInvestor);
        ////        }
        ////    }
        ////    return listInvestorOnlineChat;
        ////}

        ///// <summary>
        ///// Send Message To Investor
        ///// </summary>
        ///// <param name="objMessage">TradingServiceV3.Business.ChatItem objMessage</param>
        ///// <returns>bool</returns>
        //internal bool SendMessageToInvestor(TradingServer.ClientBusiness.ChatItem objMessage)
        //{
        //    if (TradingServer.ClientBusiness.ChatDelivery.ChatQueue == null)
        //        return false;

        //    objMessage.NumberUpdate = 1;
        //    TradingServer.ClientBusiness.ChatDelivery.ChatQueue.Add(objMessage);

        //    ///Call Function Calculate Number Update Queue Chat
        //    ///
        //    CalUpdateQueueChat(objMessage.ToInvestorName);
        //    return true;
        //}

        ///// <summary>
        ///// Get Message From Investor ID
        ///// </summary>
        ///// <param name="InvestorID">int InvestorID</param>
        ///// <returns>TradingServiceV3.Business.ChatItem[]</returns>
        //internal TradingServer.ClientBusiness.ChatItem[] GetMessageFromInvestor(int InvestorID)
        //{
        //    if (TradingServer.ClientBusiness.ChatDelivery.ChatQueue == null)
        //        return null;

        //    var item = from row in TradingServer.ClientBusiness.ChatDelivery.ChatQueue
        //               where row.ToInvestorID == InvestorID
        //               select row;

        //    if (item == null)
        //        return null;

        //    int count = item.Count();
        //    if (count == 0)
        //        return null;

        //    ClientBusiness.ChatItem[] listChat = new ClientBusiness.ChatItem[count];
        //    for (int i = 0; i < count; i++)
        //    {
        //        listChat[i] = item.ElementAt(0);
        //        TradingServer.ClientBusiness.ChatDelivery.ChatQueue.Remove(item.ElementAt(0));
        //    }
        //    return listChat;
        //}

        ///// <summary>
        ///// Get Message By Investor Name
        ///// </summary>
        ///// <param name="InvestorName">string InvestorName</param>
        ///// <returns>TradingServiceV3.Business.ChatItem[]</returns>
        //internal TradingServiceV3.Business.ChatItem[] GetMessageByInvestorName(string InvestorName)
        //{
        //    if (TradingServiceV3.Business.ChatDelivery.ChatQueue == null)
        //        return null;

        //    var item = from row in TradingServiceV3.Business.ChatDelivery.ChatQueue
        //               where row.ToInvestorName.ToUpper().Trim() == InvestorName.ToUpper().Trim()
        //               select row;

        //    if (item == null)
        //        return null;

        //    int count = item.Count();
        //    if (count == 0)
        //        return null;

        //    Business.ChatItem[] listChat = new Business.ChatItem[count];
        //    for (int i = 0; i < count; i++)
        //    {
        //        listChat[i] = item.ElementAt(0);
        //        TradingServiceV3.Business.ChatDelivery.ChatQueue.Remove(item.ElementAt(0));
        //    }
        //    ///Call Function Calculate Message
        //    ///
        //    CalUpdateQueueChat(InvestorName);

        //    return listChat;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="InvestorID"></param>
        ///// <param name="InvestorName"></param>
        ///// <returns></returns>
        //internal bool AddFriends(string MyName, string RequestName)
        //{
        //    bool result = false;
        //    if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest != null)
        //    {
        //        bool flag = false;
        //        int count = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest.Count;
        //        for (int i = 0; i < count; i++)
        //        {
        //            if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].InvestorNameBeRequest == RequestName)
        //            {
        //                flag = true;
        //            }
        //        }

        //        if (flag == false)
        //        {
        //            TradingServiceV3.Business.RequestFriends newRequestFriends = new Business.RequestFriends();
        //            newRequestFriends.InvestorNameRequest = MyName;
        //            newRequestFriends.InvestorNameBeRequest = RequestName;
        //            newRequestFriends.TimeRequest = DateTime.Now;
        //            newRequestFriends.IsAccept = false;
        //            TradingServiceV3.Business.ChatDelivery.ListFriendsRequest.Add(newRequestFriends);

        //            result = true;
        //        }
        //    }
        //    else
        //    {
        //        TradingServiceV3.Business.ChatDelivery.ListFriendsRequest = new List<Business.RequestFriends>();
        //        TradingServiceV3.Business.RequestFriends newRequestFriends = new Business.RequestFriends();
        //        newRequestFriends.InvestorNameRequest = MyName;
        //        newRequestFriends.InvestorNameBeRequest = RequestName;
        //        newRequestFriends.TimeRequest = DateTime.Now;
        //        newRequestFriends.IsAccept = false;

        //        TradingServiceV3.Business.ChatDelivery.ListFriendsRequest.Add(newRequestFriends);

        //        result = true;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="MyName"></param>
        ///// <returns></returns>
        //internal void CheckRequestFriends(int InvestorID, string MyName)
        //{
        //    if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest == null)
        //        return;

        //    int count = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].InvestorNameRequest == MyName)
        //        {
        //            if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].IsAccept == true)
        //            {
        //                TradingServiceV3.Business.Friends newFriends = new Business.Friends();
        //                newFriends.InvestorID = InvestorID;
        //                newFriends.InvestorName = MyName;
        //                newFriends.FriendsName.Add(TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].InvestorNameBeRequest);

        //                TradingServiceV3.Business.ChatDelivery.ListFriends.Add(newFriends);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="InvestorID"></param>
        ///// <param name="BeRequestName"></param>
        ///// <returns></returns>
        //internal List<TradingServiceV3.Business.RequestFriends> CheckBeRequestFriends(int InvestorID, string BeRequestName)
        //{
        //    List<TradingServiceV3.Business.RequestFriends> listRequestFriends = new List<Business.RequestFriends>();
        //    if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest == null)
        //        return null;

        //    int count = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].InvestorNameBeRequest == BeRequestName)
        //        {
        //            TradingServiceV3.Business.RequestFriends newRequestFriends = new Business.RequestFriends();
        //            newRequestFriends.InvestorNameRequest = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].InvestorNameRequest;
        //            newRequestFriends.InvestorNameBeRequest = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].InvestorNameBeRequest;
        //            newRequestFriends.TimeRequest = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].TimeRequest;
        //            newRequestFriends.IsAccept = TradingServiceV3.Business.ChatDelivery.ListFriendsRequest[i].IsAccept;

        //            listRequestFriends.Add(newRequestFriends);
        //        }
        //    }

        //    return listRequestFriends;
        //}

        //#region Remove Friends From List Friends
        ///// <summary>
        ///// Remove Friends From List Friends
        ///// </summary>
        ///// <param name="InvestorID">int InvestorID</param>
        ///// <param name="InvestorNameBeRemove">string InvestorName</param>
        ///// <returns>bool</returns>
        //internal bool RemoveFriends(int InvestorID, string InvestorNameBeRemove)
        //{
        //    bool result = false;
        //    if (TradingServiceV3.Business.ChatDelivery.ListFriends == null)
        //        return false;

        //    int count = TradingServiceV3.Business.ChatDelivery.ListFriends.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (TradingServiceV3.Business.ChatDelivery.ListFriends[i].InvestorID == InvestorID)
        //        {
        //            int countFriends = TradingServiceV3.Business.ChatDelivery.ListFriends[i].FriendsName.Count;
        //            for (int j = 0; j < countFriends; j++)
        //            {
        //                if (TradingServiceV3.Business.ChatDelivery.ListFriends[i].FriendsName[j] == InvestorNameBeRemove)
        //                {
        //                    TradingServiceV3.Business.ChatDelivery.ListFriends[i].FriendsName.RemoveAt(j);
        //                    result = true;
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
        //#endregion

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="InvestorID"></param>
        //internal void CalUpdateQueueChat(string InvestorName)
        //{
        //    Business.ChangeCommandQueue newChangeCommandQueue = new Business.ChangeCommandQueue();
        //    if (Business.ChatDelivery.ChatQueue == null)
        //        return;

        //    int count = Business.ChatDelivery.ChatQueue.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (Business.ChatDelivery.ChatQueue[i].ToInvestorName.ToUpper().Trim() == InvestorName.ToUpper().Trim())
        //        {
        //            newChangeCommandQueue.Change += Business.ChatDelivery.ChatQueue[i].NumberUpdate;
        //        }
        //    }

        //    if (Business.Market.UpdateQueueChat != null)
        //    {
        //        if (Business.Market.UpdateQueueChat.ContainsKey(InvestorName.ToUpper().Trim()))
        //        {
        //            Business.Market.UpdateQueueChat[InvestorName.ToUpper().Trim()] = newChangeCommandQueue;
        //        }
        //        else
        //        {
        //            Business.Market.UpdateQueueChat.Add(InvestorName.ToUpper().Trim(), newChangeCommandQueue);
        //        }
        //    }
        //    else
        //    {
        //        Business.Market.UpdateQueueChat = new Dictionary<string, Business.ChangeCommandQueue>();
        //        Business.Market.UpdateQueueChat.Add(InvestorName.ToUpper().Trim(), newChangeCommandQueue);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="InvestorName"></param>
        ///// <returns></returns>
        //internal static Business.ChangeCommandQueue GetNumberUpdateChatQueue(string InvestorName)
        //{
        //    Business.ChangeCommandQueue newChangeChatUpdateQueue = new Business.ChangeCommandQueue();

        //    if (Business.Market.UpdateQueueChat != null)
        //    {
        //        if (Business.Market.UpdateQueueChat.ContainsKey(InvestorName.ToUpper().Trim()))
        //        {
        //            Business.Market.UpdateQueueChat.TryGetValue(InvestorName.ToUpper().Trim(), out newChangeChatUpdateQueue);
        //        }
        //    }
        //    return newChangeChatUpdateQueue;
        //}
    }
}
