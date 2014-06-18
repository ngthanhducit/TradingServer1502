using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class ParameterItem
    {        
        /// <summary>
        /// GET ALL MARKET CONFIG IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllMarketConfig()
        {
            return ParameterItem.DBWMarketConfig.GetAllMarketConfig();
        }

        /// <summary>
        /// GET MARKET CONFIG BY MARKET CONFIG ID
        /// </summary>
        /// <param name="MarketConfigID"></param>
        /// <returns></returns>
        internal Business.ParameterItem GetMarketConfigByID(int MarketConfigID)
        {
            return ParameterItem.DBWMarketConfig.GetMarketConfigByMarketConfigID(MarketConfigID);
        }

        /// <summary>
        /// ADD NEW MARKET CONFIG
        /// </summary>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal int AddNewMarketConfig(int CollectionValue, string Name, string Code, int BoolValue,
                                            string StringValue, string NumValue, DateTime DateValue)
        {
            int Result = -1;

            Business.ParameterItem newParameter = new ParameterItem();
            newParameter.BoolValue = BoolValue;
            newParameter.Code = Code;
            newParameter.CollectionValue = new List<ParameterItem>();
            newParameter.DateValue = DateValue;
            newParameter.Name = Name;
            newParameter.NumValue = NumValue;
            newParameter.StringValue = StringValue;

            Result = ParameterItem.DBWMarketConfig.AddNewMarketConfig(CollectionValue, Name, Code, BoolValue, StringValue, NumValue, DateValue);

            newParameter.ParameterItemID = Result;

            if (Result > 0)
            {
                if (Business.Market.MarketConfig != null)
                {
                    Business.Market.MarketConfig.Add(newParameter);
                }
                else
                {
                    Business.Market.MarketConfig = new List<ParameterItem>();
                    Business.Market.MarketConfig.Add(newParameter);
                }

                if (Code == "C34")
                {
                    string[] subValue = StringValue.Split('`');
                    if (subValue.Length == 2)
                    {
                        Business.GroupDefault newGroupDefault = new GroupDefault();
                        newGroupDefault.DomainName = subValue[0];
                        newGroupDefault.GroupDefaultName = subValue[1];
                        newGroupDefault.GroupDefaultID = Result;

                        Business.Market.ListGroupDefault.Add(newGroupDefault);
                    }

                    return Result;
                }
            }

            Business.Market.marketInstance.InitTimeEventInSymbol();

            Business.Market.marketInstance.InitTimeEventServer();

            return Result;
        }

        /// <summary>
        /// UPDATE MARKET CONFIG
        /// </summary>
        /// <param name="MarketConfigID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal bool UpdateMarketConfig(int MarketConfigID, int CollectionValue, string Name,
                                            string Code, int BoolValue, string StringValue, string NumValue, DateTime DateValue)
        {
            bool Result = false;

            int count = Business.Market.MarketConfig.Count;
            for (int i = 0; i < count; i++)
            {
                if (Business.Market.MarketConfig[i].ParameterItemID == MarketConfigID)
                {
                    #region UPDATE HOLIDAY
                    if (Code == "C27")
                    {
                        if (Business.Market.YearEvent != null)
                        {
                            int countYearEvent = Business.Market.YearEvent.Count;
                            for (int j = 0; j < countYearEvent; j++)
                            {
                                if (Business.Market.YearEvent[j].TimeEventID == MarketConfigID)
                                {
                                    if (Business.Market.YearEvent[j].TargetFunction != null)
                                    {
                                        int countTarget = Business.Market.YearEvent[j].TargetFunction.Count;
                                        for (int n = 0; n < countTarget; n++)
                                        {
                                            string target = Business.Market.YearEvent[j].TargetFunction[n].EventPosition;

                                            #region RESTORE STATUS HOLIDAY ALL SYMBOL
                                            if (target.ToUpper() == "ALL")
                                            {
                                                if (Business.Market.SymbolList != null)
                                                {
                                                    int countSymbol = Business.Market.SymbolList.Count;
                                                    for (int m = 0; m < countSymbol; m++)
                                                    {
                                                        Business.Market.SymbolList[m].IsHoliday = false;
                                                    }
                                                }

                                                continue;
                                            }
                                            #endregion

                                            bool flag = false;

                                            #region RESTORE STATUS HOLIDAY OF SYMBOL
                                            if (Business.Market.SymbolList != null)
                                            {
                                                int countSymbol = Business.Market.SymbolList.Count;
                                                for (int m = 0; m < countSymbol; m++)
                                                {
                                                    if (Business.Market.SymbolList[m].Name.Trim() == target)
                                                    {
                                                        Business.Market.SymbolList[m].IsHoliday = false;
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region RESTORE STATUS HOLIDAY OF SECURITY
                                            if (!flag)
                                            {
                                                if (Business.Market.SecurityList != null)
                                                {
                                                    int countSecurity = Business.Market.SecurityList.Count;
                                                    for (int m = 0; m < countSecurity; m++)
                                                    {
                                                        if (Business.Market.SecurityList[m].Name == target)
                                                        {
                                                            if (Business.Market.SymbolList != null)
                                                            {
                                                                int countSymbol = Business.Market.SymbolList.Count;
                                                                for (int k = 0; k < countSymbol; k++)
                                                                {
                                                                    if (Business.Market.SymbolList[k].SecurityID == Business.Market.SecurityList[m].SecurityID)
                                                                    {
                                                                        Business.Market.SymbolList[k].IsHoliday = false;
                                                                    }
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region UPDATE DEFAULT GROUP
                    if (Code == "C34")
                    {
                        if (Business.Market.ListGroupDefault != null)
                        {
                            int countGroupDefault = Business.Market.ListGroupDefault.Count;
                            for (int j = 0; j < countGroupDefault; j++)
                            {
                                if (Business.Market.ListGroupDefault[j].GroupDefaultID == MarketConfigID)
                                {
                                    string[] subValue = StringValue.Split('`');
                                    if (subValue.Length == 2)
                                    {
                                        Business.Market.ListGroupDefault[j].DomainName = subValue[0];
                                        Business.Market.ListGroupDefault[j].GroupDefaultName = subValue[1];
                                    }

                                    break;
                                }
                            }
                        }

                        Business.Market.MarketConfig[i].BoolValue = BoolValue;
                        Business.Market.MarketConfig[i].Code = Code;
                        Business.Market.MarketConfig[i].DateValue = DateValue;
                        //Business.Market.MarketConfig[i].Name = Name;
                        Business.Market.MarketConfig[i].NumValue = NumValue;
                        Business.Market.MarketConfig[i].ParameterItemID = MarketConfigID;
                        Business.Market.MarketConfig[i].StringValue = StringValue;

                        Result = ParameterItem.DBWMarketConfig.UpdateMarketConfig(MarketConfigID, -1, Business.Market.MarketConfig[i].Name, Code, BoolValue,
                            StringValue, NumValue, DateValue);
                        Result = true;
                        return Result;
                    }
                    #endregion                    

                    if (Code == "C30")
                    {
                        bool result = Business.PriceServer.Instance.ConfigMultipleQuotes(StringValue);
                        if (!result)
                            break;
                    }

                    if (Code == "C31")
                    {
                        Business.PriceServer.Instance.UpdateTimeCheckMultipleQuotes(NumValue);
                    }

                    Business.Market.MarketConfig[i].BoolValue = BoolValue;
                    Business.Market.MarketConfig[i].Code = Code;
                    Business.Market.MarketConfig[i].DateValue = DateValue;
                    //Business.Market.MarketConfig[i].Name = Name;
                    Business.Market.MarketConfig[i].NumValue = NumValue;
                    Business.Market.MarketConfig[i].ParameterItemID = MarketConfigID;
                    Business.Market.MarketConfig[i].StringValue = StringValue;

                    Result = ParameterItem.DBWMarketConfig.UpdateMarketConfig(MarketConfigID, -1, Business.Market.MarketConfig[i].Name, Code, BoolValue,
                        StringValue, NumValue, DateValue);

                    break;
                }
            }

            Business.Market.marketInstance.InitTimeEventInSymbol();

            Business.Market.marketInstance.InitTimeEventServer();

            return Result;
        }

        /// <summary>
        /// UPDATE MARKET CONFIG
        /// </summary>
        /// <param name="MarketConfigID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal bool UpdateMarketConfig(List<Business.ParameterItem> listMarketConfig,string ipAddress,string code)
        {
            bool Result = false;
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();
            StringBuilder content = new StringBuilder();

            content.Append("'" + code + "': update common server: ");

            if (listMarketConfig != null)
            {
                int countMarketConfig = listMarketConfig.Count;
                for (int a = 0; a < countMarketConfig; a++)
                {
                    int count = Business.Market.MarketConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.MarketConfig[i].ParameterItemID == listMarketConfig[a].ParameterItemID)
                        {
                            #region BEFORE
		                    switch (Business.Market.MarketConfig[i].Code)
                            {
                                case "C01":
                                    beforeContent.Append("server: " + Business.Market.MarketConfig[i].StringValue + " - ");
                                    break;                                    
                                case "C02":
                                    beforeContent.Append("name: " + Business.Market.MarketConfig[i].StringValue + " - ");
                                    break;
                                case "C03":
                                    break;
                                case "C04":
                                    break;
                                case"C05":
                                    break;
                                case "C06":
                                    beforeContent.Append("time of demo: " + Business.Market.MarketConfig[i].NumValue + " - ");
                                    break;
                                case "C07":
                                    break;
                                case "C08":
                                    break;
                                case "C09":
                                    beforeContent.Append("end of day time: " + Business.Market.MarketConfig[i].StringValue + " - ");
                                    break;
                                case "C10":
                                    break;
                                case "C11":
                                    beforeContent.Append("statements mode: " + Business.Market.MarketConfig[i].StringValue + " - ");
                                    break;
                                case "C12":
                                    beforeContent.Append("monthly statements mode: " + Business.Market.MarketConfig[i].StringValue + " - ");
                                    break;
                                case "C13":
                                    break;
                                case "C14":
                                    break;
                                case "C15":
                                    break;
                                case "C16":
                                    break;
                                case "C17":
                                    break;
                                case "C18":
                                    break;
                                case "C19":
                                    break;
                                case "C20":
                                    break;
                                case "C21":
                                    break;
                                case "C22":
                                    break;
                                case "C23":
                                    break;
                                case "C24":
                                    break;
                                case "C25":
                                    break;
                                case "C26":
                                    if(Business.Market.MarketConfig[i].BoolValue == 1)
                                        beforeContent.Append("generate statements at weekends: on - ");
                                    else
                                        beforeContent.Append("generate statements at weekends: off - ");
                                    break;
                                case "C27":
                                    break;
                                case "C28":
                                    break;
                                case "C29":
                                    break;
                                case "C30":
                                    beforeContent.Append("multiple price quotes: " + Business.Market.MarketConfig[i].StringValue.Replace(',','-') + " - ");
                                    break;
                                case "C31":
                                    beforeContent.Append("time check price quotes: " + Business.Market.MarketConfig[i].StringValue +  " - ");
                                    break;
                                case "C33":
                                    beforeContent.Append("group default: " + Business.Market.MarketConfig[i].StringValue + " - ");
                                    break;
                                case "C35":
                                    beforeContent.Append("path statement: " + Business.Market.MarketConfig[i].StringValue + "-");
                                    break;
                            }
	                        #endregion                            

                            #region AFTER
		                    switch (listMarketConfig[a].Code)
                            {
                                case "C01":
                                    afterContent.Append("server: " + listMarketConfig[a].StringValue + " - ");
                                    break;                                    
                                case "C02":
                                    afterContent.Append("name: " + listMarketConfig[a].StringValue + " - ");
                                    break;
                                case "C03":
                                    break;
                                case "C04":
                                    break;
                                case"C05":
                                    break;
                                case "C06":
                                    afterContent.Append("time of demo: " + listMarketConfig[a].NumValue + " - ");
                                    break;
                                case "C07":
                                    break;
                                case "C08":
                                    break;
                                case "C09":
                                    afterContent.Append("end of day time: " + listMarketConfig[a].StringValue + " - ");
                                    break;
                                case "C10":
                                    break;
                                case "C11":
                                    afterContent.Append("statements mode: " + listMarketConfig[a].StringValue + " - ");
                                    break;
                                case "C12":
                                    afterContent.Append("monthly statements mode: " + listMarketConfig[a].StringValue + " - ");
                                    break;
                                case "C13":
                                    break;
                                case "C14":
                                    break;
                                case "C15":
                                    break;
                                case "C16":
                                    break;
                                case "C17":
                                    break;
                                case "C18":
                                    break;
                                case "C19":
                                    break;
                                case "C20":
                                    break;
                                case "C21":
                                    break;
                                case "C22":
                                    break;
                                case "C23":
                                    break;
                                case "C24":
                                    break;
                                case "C25":
                                    break;
                                case "C26":
                                    if(listMarketConfig[a].BoolValue == 1)
                                        afterContent.Append("generate statements at weekends: on - ");
                                    else
                                        afterContent.Append("generate statements at weekends: off - ");
                                    break;
                                case "C27":
                                    break;
                                case "C28":
                                    break;
                                case "C29":
                                    break;
                                case "C30":
                                    afterContent.Append("multiple price quotes: " + listMarketConfig[a].StringValue.Replace(',', '-') + " - ");
                                    break;
                                case "C31":
                                    afterContent.Append("time check price quotes: " + listMarketConfig[a].StringValue + " - ");
                                    break;
                                case "C33":
                                    afterContent.Append("group default: " + listMarketConfig[a].StringValue + " - ");
                                    break;
                                case "C35":
                                    afterContent.Append("path statement: " + listMarketConfig[a].StringValue + "-");
                                    break;
                            }
	                        #endregion                            

                            #region UPDATE HOLIDAY
                            if (listMarketConfig[a].Code == "C27")
                            {
                                if (Business.Market.YearEvent != null)
                                {
                                    int countYearEvent = Business.Market.YearEvent.Count;
                                    for (int j = 0; j < countYearEvent; j++)
                                    {
                                        if (Business.Market.YearEvent[j].TimeEventID == listMarketConfig[a].ParameterItemID)
                                        {
                                            if (Business.Market.YearEvent[j].TargetFunction != null)
                                            {
                                                int countTarget = Business.Market.YearEvent[j].TargetFunction.Count;
                                                for (int n = 0; n < countTarget; n++)
                                                {
                                                    string target = Business.Market.YearEvent[j].TargetFunction[n].EventPosition;

                                                    #region RESTORE STATUS HOLIDAY ALL SYMBOL
                                                    if (target.ToUpper() == "ALL")
                                                    {
                                                        if (Business.Market.SymbolList != null)
                                                        {
                                                            int countSymbol = Business.Market.SymbolList.Count;
                                                            for (int m = 0; m < countSymbol; m++)
                                                            {
                                                                Business.Market.SymbolList[m].IsHoliday = false;
                                                            }
                                                        }

                                                        continue;
                                                    }
                                                    #endregion

                                                    bool flag = false;

                                                    #region RESTORE STATUS HOLIDAY OF SYMBOL
                                                    if (Business.Market.SymbolList != null)
                                                    {
                                                        int countSymbol = Business.Market.SymbolList.Count;
                                                        for (int m = 0; m < countSymbol; m++)
                                                        {
                                                            if (Business.Market.SymbolList[m].Name.Trim() == target)
                                                            {
                                                                Business.Market.SymbolList[m].IsHoliday = false;
                                                                flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    #region RESTORE STATUS HOLIDAY OF SECURITY
                                                    if (!flag)
                                                    {
                                                        if (Business.Market.SecurityList != null)
                                                        {
                                                            int countSecurity = Business.Market.SecurityList.Count;
                                                            for (int m = 0; m < countSecurity; m++)
                                                            {
                                                                if (Business.Market.SecurityList[m].Name == target)
                                                                {
                                                                    if (Business.Market.SymbolList != null)
                                                                    {
                                                                        int countSymbol = Business.Market.SymbolList.Count;
                                                                        for (int k = 0; k < countSymbol; k++)
                                                                        {
                                                                            if (Business.Market.SymbolList[k].SecurityID == Business.Market.SecurityList[m].SecurityID)
                                                                            {
                                                                                Business.Market.SymbolList[k].IsHoliday = false;
                                                                            }
                                                                        }
                                                                    }

                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }

                                            break;
                                        }
                                    }
                                }

                                //Log Update Holiday
                                string contentUpdateHoliday = "'" + code + "': holidays configuration update";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, contentUpdateHoliday, "[update holiday]", ipAddress, code);
                            }
                            #endregion

                            #region UPDATE DEFAULT GROUP
                            if (listMarketConfig[a].Code == "C34")
                            {
                                if (Business.Market.ListGroupDefault != null)
                                {
                                    int countGroupDefault = Business.Market.ListGroupDefault.Count;
                                    for (int j = 0; j < countGroupDefault; j++)
                                    {
                                        if (Business.Market.ListGroupDefault[j].GroupDefaultID == listMarketConfig[a].ParameterItemID)
                                        {
                                            string[] subValue = listMarketConfig[a].StringValue.Split('`');
                                            if (subValue.Length == 2)
                                            {
                                                Business.Market.ListGroupDefault[j].DomainName = subValue[0];
                                                Business.Market.ListGroupDefault[j].GroupDefaultName = subValue[1];
                                            }

                                            break;
                                        }
                                    }
                                }

                                Business.Market.MarketConfig[i].BoolValue = listMarketConfig[a].BoolValue;
                                Business.Market.MarketConfig[i].Code = listMarketConfig[a].Code;
                                Business.Market.MarketConfig[i].DateValue = listMarketConfig[a].DateValue;
                                //Business.Market.MarketConfig[i].Name = Name;
                                Business.Market.MarketConfig[i].NumValue = listMarketConfig[a].NumValue;
                                Business.Market.MarketConfig[i].ParameterItemID = listMarketConfig[a].ParameterItemID;
                                Business.Market.MarketConfig[i].StringValue = listMarketConfig[a].StringValue;

                                Result = ParameterItem.DBWMarketConfig.UpdateMarketConfig(listMarketConfig[a].ParameterItemID, -1, Business.Market.MarketConfig[i].Name,
                                    listMarketConfig[a].Code, listMarketConfig[a].BoolValue, listMarketConfig[a].StringValue, listMarketConfig[a].NumValue,
                                    listMarketConfig[a].DateValue);
                                Result = true;

                                //Log Update Group Config
                                string tempContentGroupConfig = "'" + code + "': group config configurations update";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, tempContentGroupConfig, "[update group config]", ipAddress, code);

                                return Result;
                            }
                            #endregion

                            if (Code == "C30")
                            {
                                bool result = Business.PriceServer.Instance.ConfigMultipleQuotes(listMarketConfig[a].StringValue);
                                if (!result)
                                    break;
                            }

                            if (Code == "C31")
                            {
                                Business.PriceServer.Instance.UpdateTimeCheckMultipleQuotes(listMarketConfig[a].NumValue);
                            }

                            if (code == "C35")
                            {
                                if (string.IsNullOrEmpty(listMarketConfig[a].StringValue))
                                    listMarketConfig[a].StringValue = Business.Market.MarketConfig[i].StringValue;
                            }

                            Business.Market.MarketConfig[i].BoolValue = listMarketConfig[a].BoolValue;
                            Business.Market.MarketConfig[i].Code = listMarketConfig[a].Code;
                            Business.Market.MarketConfig[i].DateValue = listMarketConfig[a].DateValue;
                            //Business.Market.MarketConfig[i].Name = Name;
                            Business.Market.MarketConfig[i].NumValue = listMarketConfig[a].NumValue;
                            Business.Market.MarketConfig[i].ParameterItemID = listMarketConfig[a].ParameterItemID;
                            Business.Market.MarketConfig[i].StringValue = listMarketConfig[a].StringValue;

                            Result = ParameterItem.DBWMarketConfig.UpdateMarketConfig(listMarketConfig[a].ParameterItemID, -1, Business.Market.MarketConfig[i].Name,
                                listMarketConfig[a].Code, listMarketConfig[a].BoolValue, listMarketConfig[a].StringValue, listMarketConfig[a].NumValue, listMarketConfig[a].DateValue);

                            break;
                        }
                    }
                }
            }

            Business.Market.marketInstance.InitTimeEventInSymbol();

            Business.Market.marketInstance.InitTimeEventServer();

            string tempBeforeContent = beforeContent.ToString();
            if(tempBeforeContent.EndsWith(" - "))
                tempBeforeContent = tempBeforeContent.Remove(tempBeforeContent.Length-2,2);

            string tempAfterContent = afterContent.ToString();
            if (tempAfterContent.EndsWith(" - "))
                tempAfterContent = tempAfterContent.Remove(tempAfterContent.Length - 2, 2);

            if (!string.IsNullOrEmpty(tempAfterContent.Trim()) && !string.IsNullOrEmpty(tempBeforeContent.Trim()))
            {
                content.Append(tempBeforeContent + " -> " + tempAfterContent);
                TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update common]", ipAddress, code);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketConfigID"></param>
        /// <returns></returns>
        internal bool DeleteMarketConfig(int marketConfigID)
        {
            bool result = false;
            if (Business.Market.MarketConfig != null)
            {
                int count = Business.Market.MarketConfig.Count;
                for (int i = 0; i < Business.Market.MarketConfig.Count; i++)
                {
                    if (Business.Market.MarketConfig[i].ParameterItemID == marketConfigID)
                    {
                        #region DELETE HOLIDAY
                        if (Business.Market.MarketConfig[i].Code == "C27")
                        {
                            //RESTORE HOLIDAY TARGET
                            if (Business.Market.YearEvent != null)
                            {
                                int countYearEvent = Business.Market.YearEvent.Count;
                                for (int j = 0; j < countYearEvent; j++)
                                {
                                    if (Business.Market.YearEvent[j].TimeEventID == marketConfigID)
                                    {
                                        if (Business.Market.YearEvent[j].TargetFunction != null)
                                        {
                                            int countTarget = Business.Market.YearEvent[j].TargetFunction.Count;
                                            for (int n = 0; n < countTarget; n++)
                                            {
                                                bool flag = false;
                                                string target = Business.Market.YearEvent[j].TargetFunction[n].EventPosition;

                                                #region RESTORE STATUS HOLIDAY ALL SYMBOL
                                                if (target.ToUpper() == "ALL")
                                                {
                                                    if (Business.Market.SymbolList != null)
                                                    {
                                                        int countSymbol = Business.Market.SymbolList.Count;
                                                        for (int m = 0; m < countSymbol; m++)
                                                        {
                                                            Business.Market.SymbolList[m].IsHoliday = false;
                                                        }
                                                    }

                                                    continue;
                                                }
                                                #endregion

                                                #region RESTORE STATUS HOLIDAY OF SYMBOL
                                                if (Business.Market.SymbolList != null)
                                                {
                                                    int countSymbol = Business.Market.SymbolList.Count;
                                                    for (int m = 0; m < countSymbol; m++)
                                                    {
                                                        if (Business.Market.SymbolList[m].Name.Trim() == target)
                                                        {
                                                            Business.Market.SymbolList[m].IsHoliday = false;
                                                            flag = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region RESTORE STATUS HOLIDAY OF SECURITY
                                                if (!flag)
                                                {
                                                    if (Business.Market.SecurityList != null)
                                                    {
                                                        int countSecurity = Business.Market.SecurityList.Count;
                                                        for (int m = 0; m < countSecurity; m++)
                                                        {
                                                            if (Business.Market.SecurityList[m].Name == target)
                                                            {
                                                                if (Business.Market.SymbolList != null)
                                                                {
                                                                    int countSymbol = Business.Market.SymbolList.Count;
                                                                    for (int k = 0; k < countSymbol; k++)
                                                                    {
                                                                        if (Business.Market.SymbolList[k].SecurityID == Business.Market.SecurityList[m].SecurityID)
                                                                        {
                                                                            Business.Market.SymbolList[k].IsHoliday = false;
                                                                        }
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                            //REMOVE MARKET IN CLASS MARKET
                            Business.Market.MarketConfig.RemoveAt(i);
                            i--;
                            //REMOVE MARKET IN DATABASE
                            dbwMarketConfig.DeleteMarketConfig(marketConfigID);
                            result = true;
                        }
                        #endregion                        
                     
                        if (Business.Market.MarketConfig[i].Code == "C34")
                        {
                            if (Business.Market.ListGroupDefault != null)
                            {
                                int countGroupDefault = Business.Market.ListGroupDefault.Count;
                                for (int j = 0; j < countGroupDefault; j++)
                                {
                                    if (Business.Market.ListGroupDefault[j].GroupDefaultID == marketConfigID)
                                    {
                                        Business.Market.ListGroupDefault.RemoveAt(j);
                                        //REMOVE MARKET IN CLASS MARKET
                                        Business.Market.MarketConfig.RemoveAt(i);
                                        i--;
                                        //REMOVE MARKET IN DATABASE
                                        dbwMarketConfig.DeleteMarketConfig(marketConfigID);
                                        result = true;

                                        return result;
                                        break;
                                    }
                                }
                            }

                            //REMOVE MARKET IN CLASS MARKET
                            Business.Market.MarketConfig.RemoveAt(i);

                            //REMOVE MARKET IN DATABASE
                            dbwMarketConfig.DeleteMarketConfig(marketConfigID);
                            result = true;
                        }

                        break;
                    }
                }
            }

            Business.Market.marketInstance.InitTimeEventInSymbol();

            Business.Market.marketInstance.InitTimeEventServer();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalMarketConfig()
        {
            return ParameterItem.DBWMarketConfig.CountMarketConfig();
        }
    }
}
