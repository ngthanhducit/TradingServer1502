using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        bool isSymbolUpdate =false;
        bool isSecurityUpdate =false ;
        bool isInvestorGroup = false;

        /// <summary>
        /// get group id by group name
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <returns></returns>
        internal int GetGroupIDByName(string groupName)
        {
            int count = Market.InvestorGroupList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Market.InvestorGroupList[i].Name == groupName)
                {
                    return Market.InvestorGroupList[i].InvestorGroupID;
                }
            }
            return -1;

        }

        /// <summary>
        /// get security id by name
        /// </summary>
        /// <param name="securityName">security name</param>
        /// <returns></returns>
        internal int GetSecurityIDByName(string securityName)
        { 
            int countSecurity = Market.SecurityList.Count;
            for (int i = 0; i < countSecurity; i++)
            {
                if (Market.SecurityList[i].Name == securityName)
                {
                    return Market.SecurityList[i].SecurityID;
                }
            }
            return -1;
        }

        /// <summary>
        /// get symbol id by name
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <returns></returns>
        internal int GetSymbolIDByName(string symbolName)
        {
            int count=Market.SymbolList.Count;
            for(int i=0;i<count;i++)
            {
                if (Market.SymbolList[i].Name == symbolName)
                {
                    return Market.SymbolList[i].SymbolID;
                }
            }
            return -1;
        }

        /// <summary>
        /// delete symbol 
        /// </summary>
        /// <param name="symbol">symbol name</param>
        /// <returns></returns>
        internal string DeleteSymbol(string symbol)
        {
            if(isSymbolUpdate)
            {
                return "DSyE011";
            }

            #region check and remove symbol from market area
            int countMarketArea = Market.MarketArea.Count;
            for (int i = 0; i < countMarketArea; i++)
            {
                if (Market.MarketArea[i].ListSymbol == null || Market.MarketArea[i].ListSymbol.Count == 0)
                {
                    continue;
                }
                int countSymbol = Market.MarketArea[i].ListSymbol.Count;
                for (int j = 0; j < countSymbol; j++)
                {                    
                    if (Market.MarketArea[i].ListSymbol[j].Name == symbol)
                    {

                        #region delete activity
                        if (Market.MarketArea[i].ListSymbol[j].CommandList.Count != 0)
                        {
                            return "DSyE001";
                        }
                        else
                        { 
                            //call DB delete function
                            //
                            int symbolID=this.GetSymbolIDByName(symbol);
                            bool isValid = false;
                            #region BUILD COMMAND REQUEST AGENT CHECK VALID DELETE SYMBOL
                            //RemoveSymbol$SymbolID
                            if (Business.Market.ListAgentConfig != null)
                            {
                                string strCmd = "RemoveSymbol$" + symbolID;
                                int countAgent = Business.Market.ListAgentConfig.Count;
                                for (int n = 0; n < countAgent; n++)
                                {
                                    if (Business.Market.IsConnectAgent)
                                    {
                                        string resultAgent = Business.Market.ListAgentConfig[n].clientAgent.StringDefaultPort(strCmd, "");
                                        if (!string.IsNullOrEmpty(resultAgent))
                                        {
                                            string[] subResult = resultAgent.Split('$');
                                            if (subResult[0] == "RemoveSymbol")
                                                isValid = bool.Parse(subResult[1]);
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (isValid)
                            {
                                Symbol deleteSymbol = new Symbol();
                                bool result = deleteSymbol.DFDeleteSymbol(symbolID);
                                if (result)
                                {
                                    //delete business
                                    //
                                    this.RemoveSymbolFromMarket(symbolID);
                                    this.RemoveSymbolFromQuoteSymbol(symbol);
                                    Market.MarketArea[i].ListSymbol.RemoveAt(j);
                                    Facade.FacadeSendNoticeManagerChangeSymbol(4, symbolID);
                                    return "DSyE000";
                                }
                                else
                                {
                                    return "DSyE002";
                                }
                            }
                        }
                        #endregion
                        break;
                    }
                }
            }
            #endregion

            return "DSyE003";
        }
        /// <summary>
        /// remove delete symbol from market
        /// </summary>
        /// <param name="symbol"></param>
        private void RemoveSymbolFromMarket(int id)
        {
            int count = Market.SymbolList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Market.SymbolList[i].SymbolID == id)
                {
                    Market.SymbolList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolName"></param>
        private void RemoveSymbolFromQuoteSymbol(string symbolName)
        {
            int count = Business.Market.QuoteList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Business.Market.QuoteList[i].Name.Trim() == symbolName.Trim())
                {
                    Business.Market.QuoteList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// delete security
        /// </summary>
        /// <param name="security">security name</param>
        /// <returns></returns>
        internal string DeleteSecurity(string security)
        {
            
            this.isSecurityUpdate=true;

            int id = this.GetSecurityIDByName(security);
            string result= this.DeleteSecurity(id);

            return result;

        }

        /// <summary>
        /// delete security 
        /// </summary>
        /// <param name="id">security id</param>
        /// <returns></returns>
        internal string DeleteSecurity(int id)
        {
            #region find security in IGroupSecurity
            int count= Market.IGroupSecurityList.Count;
            for(int i=0;i<count;i++)
            {
                if(Market.IGroupSecurityList[i].SecurityID==id)
                {
                    return "DSyE004";
                }
            }
            count = Market.SymbolList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Market.SymbolList[i].SecurityID == id)
                {
                    return "DSyE011";
                }
            }
            #endregion

            #region delete function
            Business.Security security=new Security();
            bool result = security.DFDeleteByID(id);
            if (result)
            {
                //delete function
                //
                count = Market.SecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SecurityList[i].SecurityID == id)
                    {
                        Market.SecurityList.RemoveAt(i);
                        break;
                    }
                }
                return "DSyE006";
            }
            else
            { 
                //delete db unsuccess
                //
                return "DSyE005";
            }
            #endregion

        }

        /// <summary>
        /// delete group by name
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <returns></returns>
        internal string DeleteGroup(string groupName)
        {
            int id = this.GetGroupIDByName(groupName);
            if (id == -1)
            {
                return "DSyE007";
            }
            else
            {
                return this.DeleteGroup(id);
            }
        }

        /// <summary>
        /// delete group by group id
        /// </summary>
        /// <param name="id">group id</param>
        /// <returns></returns>
        internal string DeleteGroup(int id)
        {
            #region check investor 
            int count = Market.InvestorList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == id)
                {
                    //exist investor
                    //cannot delete
                    return "DSyE008";
                }
            }
            #endregion

            InvestorGroup group = new InvestorGroup();
            bool isValid = true;
   
            #region BUILD COMMAND REQUEST AGENT CHECK VALID DELETE GROUP
            if (Business.Market.IsConnectAgent)
            {
                if (Business.Market.ListAgentConfig != null)
                {
                    ///RemoveGroup$groupID
                    ///
                    string strCmd = "RemoveGroup$" + id;
                    int countAgent = Business.Market.ListAgentConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string resultAgent = Business.Market.ListAgentConfig[i].clientAgent.StringDefaultPort(strCmd, "");
                        if (!string.IsNullOrEmpty(resultAgent))
                        {
                            string[] subResult = resultAgent.Split('$');
                            if (subResult[0] == "RemoveGroup")
                                isValid = bool.Parse(subResult[1]);
                        }
                    }
                }
            }
            #endregion

            if (isValid)
            {
                bool result = group.DFDeleteByID(id);
                if (result)
                {
                    #region remove in business layer
                    count = Market.InvestorGroupList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.InvestorGroupList[i].InvestorGroupID == id)
                        {
                            Market.InvestorGroupList.RemoveAt(i);
                            Facade.FacadeSendNoticeManagerChangeGroup(4, id);
                            break;
                        }
                    }
                    return "DSyE010";
                    #endregion
                }
                else
                {
                    //cannot delete in database
                    return "DSyE009";
                }
            }
            else
            {
                TradingServer.Facade.FacadeAddNewSystemLog(5, "can't delete because agent can't delete", "[Delete Group]", "", "");
                return "DSyE009";
            }
        }       
    }
    // end class code 
    //
}
	
