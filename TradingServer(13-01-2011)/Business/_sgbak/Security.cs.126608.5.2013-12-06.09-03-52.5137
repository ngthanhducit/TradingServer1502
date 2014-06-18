using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Security
    {
        public int SecurityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MarketAreaID { get; set; }
        public List<Business.Symbol> SymbolGroup { get; set; }
        public List<Business.ParameterItem> ParameterItems { get; set; }

        //=========================================================================

        #region Create Instance Class DBWSecurity
        private static TradingServer.DBW.DBWSecurity dbwSecurity;
        private static TradingServer.DBW.DBWSecurity SecurityInstance
        {
            get
            {
                if (Security.dbwSecurity == null)
                {
                    Security.dbwSecurity = new DBW.DBWSecurity();
                }
                return Security.dbwSecurity;
            }
        }
        #endregion

        #region Create Instance Class DBWSecurityConfig
        private static TradingServer.DBW.DBWSecurityConfig dbwSecurityConfig;
        private static TradingServer.DBW.DBWSecurityConfig SecurityConfigInstance
        {
            get
            {
                if (Security.dbwSecurityConfig == null)
                {
                    Security.dbwSecurityConfig = new DBW.DBWSecurityConfig();
                }
                return Security.dbwSecurityConfig;
            }
        }
        #endregion

        //=========================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public int AddSecurity(string Name, string Description,int MarketAreaID)
        {
            int result = -1;
            result = Security.SecurityInstance.AddNewSecurity(Name, Description,MarketAreaID);
            if (result > 0)
            {
                Business.Security security = new Business.Security();                
                security.SecurityID = result;
                security.Name = Name;
                security.Description = Description;
                security.MarketAreaID = MarketAreaID;
                Market.SecurityList.Add(security);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        public bool DeleteSecurity(int SecurityID)
        {
            bool Result = true;
            Result = Security.SecurityConfigInstance.DeleteSecurityConfigBySecurityID(SecurityID);
            Result = Security.SecurityInstance.DeleteSecurity(SecurityID);
            if (Result)
            {
                if (Market.SecurityList != null)
                {
                    int count = Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SecurityList[i].SecurityID == SecurityID)
                        {
                            Market.SecurityList.Remove(Market.SecurityList[i]);
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// delete security by id
        /// </summary>
        /// <param name="securityID">security id</param>
        /// <returns></returns>
        public bool DFDeleteByID(int securityID)
        { 
            DBW.DBWSecurity security=new DBW.DBWSecurity();
            return security.DFDeleteByID(securityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        public bool UpdateSecurity(int SecurityID, string Name, string Description,int MarketAreaID)
        {            
            bool Result = false;
            Result = Security.SecurityInstance.UpdateSecurity(SecurityID, Name, Description,MarketAreaID);

            if (Result == true)
            {
                if (Market.SecurityList != null)
                {
                    int count = Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SecurityList[i].SecurityID == SecurityID)
                        {
                            Market.SecurityList[i].Name = Name;
                            Market.SecurityList[i].Description = Description;
                            Market.SecurityList[i].MarketAreaID = MarketAreaID;
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        public bool UpdateSecurity(int SecurityID, string Name, string Description, int MarketAreaID,string ipAddress,string code)
        {
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();
            StringBuilder content = new StringBuilder();

            content.Append("'" + code + "': update security: ");

            bool Result = false;
            Result = Security.SecurityInstance.UpdateSecurity(SecurityID, Name, Description, MarketAreaID);

            if (Result == true)
            {
                if (Market.SecurityList != null)
                {
                    int count = Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SecurityList[i].SecurityID == SecurityID)
                        {
                            if (Business.Market.SecurityList[i].Name.ToUpper().Trim() == Name.ToUpper().Trim())
                            {
                                beforeContent.Append("name: " + Business.Market.SecurityList[i].Name + " - ");
                                afterContent.Append("name: " + Name + " - ");
                                Market.SecurityList[i].Name = Name;
                            }

                            if (Business.Market.SecurityList[i].Description.ToUpper().Trim() != Description.ToUpper().Trim())
                            {
                                beforeContent.Append("description: " + Business.Market.SecurityList[i].Description + " - ");
                                afterContent.Append("description: " + Description + " - ");

                                Market.SecurityList[i].Description = Description;
                            }

                            if (Business.Market.SecurityList[i].MarketAreaID != MarketAreaID)
                            {
                                string nameBefore = string.Empty;
                                string nameAfter = string.Empty;
                                if (Business.Market.MarketArea != null)
                                {
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == Business.Market.SecurityList[i].MarketAreaID)
                                        {
                                            nameBefore = Business.Market.MarketArea[j].IMarketAreaName;                                            
                                        }

                                        if (Business.Market.MarketArea[j].IMarketAreaID == MarketAreaID)
                                        {
                                            nameAfter = Business.Market.MarketArea[j].IMarketAreaName;
                                        }
                                    }
                                }

                                beforeContent.Append("market area: " + nameBefore + " - ");
                                afterContent.Append("market area: " + nameAfter + " - ");
                                Market.SecurityList[i].MarketAreaID = MarketAreaID;
                            }
                        }
                    }
                }
            }

            string tempBeforeContent = beforeContent.ToString();
            if (tempBeforeContent.EndsWith(" - "))
                tempBeforeContent = tempBeforeContent.Remove(tempBeforeContent.Length - 2, 2);

            string tempAfterContent = afterContent.ToString();
            if (tempAfterContent.EndsWith(" - "))
                tempAfterContent.Remove(tempAfterContent.Length - 2, 2);

            if (!string.IsNullOrEmpty(tempBeforeContent.Trim()) && !string.IsNullOrEmpty(tempAfterContent.Trim()))
            {
                content.Append(tempBeforeContent + " -> " + tempAfterContent);
                TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update security]", ipAddress, code);
            }
            
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListParameterItem"></param>
        /// <returns></returns>
        public int AddListSecurityConfig(List<Business.ParameterItem> ListParameterItem)
        {
            int Result = -1;
            int countParameterItem = ListParameterItem.Count;
            for (int i = 0; i < countParameterItem; i++)
            {
                Result = this.AddSecurityConfig(ListParameterItem[i]);
            }
            if (Result > 0)
            {
                ListParameterItem = Security.SecurityConfigInstance.GetSecurityConfigBySecurityID(ListParameterItem[0].SecondParameterID);
                if (Market.SecurityList != null)
                {
                    int count = Market.SecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SecurityList[i].SecurityID == ListParameterItem[0].SecondParameterID)
                        {
                            Market.SecurityList[i].ParameterItems = ListParameterItem;
                            break;
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityConfigID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="NumValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="DateValue"></param>
        /// <param name="BoolValue"></param>
        public bool UpdateSecurityConfig(int SecurityConfigID, int SecurityID, int CollectionValue, string Name, string Code, string NumValue, string StringValue, DateTime DateValue, int BoolValue)
        {
            bool Result = false;
            Business.ParameterItem oldSecurityConfig = new Business.ParameterItem();      
            oldSecurityConfig = SecurityConfigInstance.GetSecurityConfigBySecurityConfigID(SecurityConfigID);
            Result = SecurityConfigInstance.UpdateSecurityConfig(SecurityConfigID, SecurityID, CollectionValue, Name, Code, NumValue, StringValue, DateValue, BoolValue);
            List<Business.ParameterItem> newLSecurityConfig = new List<ParameterItem>();
            newLSecurityConfig = Security.SecurityConfigInstance.GetSecurityConfigBySecurityID(SecurityID);
            if (Market.SecurityList != null)
            {
                int count = Market.SecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SecurityList[i].SecurityID == SecurityID)
                    {
                        Market.SecurityList[i].ParameterItems = newLSecurityConfig;
                        break;
                    }
                }
                if (SecurityID != oldSecurityConfig.SecondParameterID)
                {
                    List<Business.ParameterItem> oldLSecurityConfig = new List<ParameterItem>();
                    oldLSecurityConfig = Security.SecurityConfigInstance.GetSecurityConfigBySecurityID(oldSecurityConfig.SecondParameterID);
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SecurityList[i].SecurityID == oldSecurityConfig.SecondParameterID)
                        {
                            Market.SecurityList[i].ParameterItems = oldLSecurityConfig;
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityConfigID"></param>
        public bool DeleteSecurityConfig(int SecurityConfigID)
        {
            bool Result = true;
            Result = Security.SecurityConfigInstance.DeleteSecurityConfigBySecurityConfigID(SecurityConfigID);
            if (Result)
            {
                if (Market.SecurityList != null)
                {
                    int count1 = Market.SecurityList.Count;
                    int count2 = 0;
                    for (int i1 = 0; i1 < count1; i1++)
                    {
                        if (Market.SecurityList[i1].ParameterItems != null)
                        {
                            count2 = Market.SecurityList[i1].ParameterItems.Count;
                            for (int i2 = 0; i2 < count2; i2++)
                            {
                                if (Market.SecurityList[i1].ParameterItems[i2].ParameterItemID == SecurityConfigID)
                                {
                                    Market.SecurityList[i1].ParameterItems.Remove(Market.SecurityList[i1].ParameterItems[i2]);
                                }
                            }
                        }
                    }
                }
            }
            return Result;
        }
    }
}
