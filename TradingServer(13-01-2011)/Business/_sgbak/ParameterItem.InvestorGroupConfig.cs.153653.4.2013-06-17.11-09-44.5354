using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class ParameterItem
    {        
        /// <summary>
        /// GET ALL INVESTOR GROUP CONFIG IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllInvestorGroupConfig()
        {
            return ParameterItem.DBWInvestorGroupConfigInstance.GetAllInvestorGroupConfig();
        }

        /// <summary>
        /// ADD NEW INVESTOR GROUP CONFIG
        /// </summary>
        /// <param name="ListParameterItem"></param>
        internal int AddNewInvestorGroupConfig(List<Business.ParameterItem> ListParameterItem)
        {           
            try
            {
                int Result = -1;
                bool Flag = false;
                if (ListParameterItem != null && Market.InvestorGroupList != null)
                {
                    int count = Market.InvestorGroupList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.InvestorGroupList[i].InvestorGroupID == ListParameterItem[0].SecondParameterID)
                        {
                            if (Business.Market.InvestorGroupList[i].ParameterItems == null)
                                Business.Market.InvestorGroupList[i].ParameterItems = new List<ParameterItem>();

                            int countParameterItem = ListParameterItem.Count;
                            for (int j = 0; j < countParameterItem; j++)
                            {
                                //Add Database
                                Result = ParameterItem.DBWInvestorGroupConfigInstance.AddNewInvestorGroupConfig(ListParameterItem[j].SecondParameterID, -1,
                                    ListParameterItem[j].Name, ListParameterItem[j].Code, ListParameterItem[j].BoolValue, ListParameterItem[j].StringValue,
                                    ListParameterItem[j].NumValue, ListParameterItem[j].DateValue);

                                if (Result > 0)
                                {
                                    ListParameterItem[j].ParameterItemID = Result;
                                    Business.Market.InvestorGroupList[i].ParameterItems.Add(ListParameterItem[j]);

                                    if (ListParameterItem[j].Code == "G01")
                                    {
                                        bool isEnable = false;
                                        if (ListParameterItem[j].BoolValue == 1)
                                            isEnable = true;
                                        Business.Market.InvestorGroupList[i].IsEnable = isEnable;
                                    }

                                    if (ListParameterItem[j].Code == "G19")
                                    {
                                        Business.Market.InvestorGroupList[i].MarginCall = double.Parse(ListParameterItem[j].NumValue);
                                    }

                                    if (ListParameterItem[j].Code == "G20")
                                    {
                                        Business.Market.InvestorGroupList[i].MarginStopOut = double.Parse(ListParameterItem[j].NumValue);
                                    }

                                    if (ListParameterItem[j].Code == "G26")
                                    {
                                        Business.Market.InvestorGroupList[i].FreeMargin = ListParameterItem[j].StringValue;
                                    }
                                }
                            }

                            Flag = true;
                        }
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// UPDATE INVESTOR GROUP CONFIG
        /// </summary>
        /// <param name="objInvestorGroupConfig"></param>
        internal bool UpdateInvestorGroupConfig(Business.ParameterItem objInvestorGroupConfig)
        {
            bool Result = false;

            //Update Value In List Investor Group Of Class Market
            if (Market.InvestorGroupList != null)
            {
                int count = Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.InvestorGroupList[i].InvestorGroupID == objInvestorGroupConfig.SecondParameterID)
                    {
                        if (Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countConfig = Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countConfig; j++)
                            {
                                if (Market.InvestorGroupList[i].ParameterItems[j].Code == objInvestorGroupConfig.Code)
                                {
                                    if (objInvestorGroupConfig.Code == "G01")
                                    {
                                        bool isEnable = false;
                                        if (objInvestorGroupConfig.BoolValue == 1)
                                            isEnable = true;
                                        Business.Market.InvestorGroupList[i].IsEnable = isEnable;
                                    }

                                    if (objInvestorGroupConfig.Code == "G19")
                                    {
                                        Business.Market.InvestorGroupList[i].MarginCall = double.Parse(objInvestorGroupConfig.NumValue);
                                    }

                                    if (objInvestorGroupConfig.Code == "G20")
                                    {
                                        Business.Market.InvestorGroupList[i].MarginStopOut = double.Parse(objInvestorGroupConfig.NumValue);
                                    }

                                    if (objInvestorGroupConfig.Code == "G26")
                                    {
                                        Business.Market.InvestorGroupList[i].FreeMargin = objInvestorGroupConfig.StringValue;
                                    }

                                    if (objInvestorGroupConfig.Code == "G38")
                                    {
                                        if (objInvestorGroupConfig.BoolValue == 1)
                                            Business.Market.InvestorGroupList[i].IsManualStopOut = true;
                                        else
                                            Business.Market.InvestorGroupList[i].IsManualStopOut = false;
                                    }

                                    //Set ParameterId For Config
                                    objInvestorGroupConfig.ParameterItemID = Market.InvestorGroupList[i].ParameterItems[j].ParameterItemID;

                                    //Update Value Parameter Item
                                    Market.InvestorGroupList[i].ParameterItems[j].Name = objInvestorGroupConfig.Name;
                                    Market.InvestorGroupList[i].ParameterItems[j].Code = objInvestorGroupConfig.Code;
                                    Market.InvestorGroupList[i].ParameterItems[j].BoolValue = objInvestorGroupConfig.BoolValue;
                                    Market.InvestorGroupList[i].ParameterItems[j].StringValue = objInvestorGroupConfig.StringValue;
                                    Market.InvestorGroupList[i].ParameterItems[j].NumValue = objInvestorGroupConfig.NumValue;
                                    Market.InvestorGroupList[i].ParameterItems[j].DateValue = objInvestorGroupConfig.DateValue;

                                    //Set ParameterID 
                                    objInvestorGroupConfig.ParameterItemID = Market.InvestorGroupList[i].ParameterItems[j].ParameterItemID;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            Result = ParameterItem.DBWInvestorGroupConfigInstance.UpdateInvestorGroupConfig(objInvestorGroupConfig.ParameterItemID, objInvestorGroupConfig.SecondParameterID,
                -1, objInvestorGroupConfig.Name, objInvestorGroupConfig.Code, objInvestorGroupConfig.BoolValue, objInvestorGroupConfig.StringValue, objInvestorGroupConfig.NumValue,
                objInvestorGroupConfig.DateValue);

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListInvestorGroupConfig"></param>
        /// <param name="code"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal bool UpdateInvestorGroupConfig(List<Business.ParameterItem> ListInvestorGroupConfig, string code, string ipAddress)
        {
            bool Result = false;
            bool isExits = false;
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();
            StringBuilder content = new StringBuilder();
            content.Append("'" + code + "': update group config ");

            if (ListInvestorGroupConfig != null)
            {
                int countGroupConfig = ListInvestorGroupConfig.Count;
                for (int a = 0; a < countGroupConfig; a++)
                {
                    //Update Value In List Investor Group Of Class Market
                    if (Market.InvestorGroupList != null)
                    {
                        int count = Market.InvestorGroupList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Market.InvestorGroupList[i].InvestorGroupID == ListInvestorGroupConfig[a].SecondParameterID)
                            {
                                if (!isExits)
                                {
                                    content.Append("'" + Market.InvestorGroupList[i].Name + "': ");
                                    isExits = true;
                                }

                                if (Market.InvestorGroupList[i].ParameterItems != null)
                                {
                                    int countConfig = Market.InvestorGroupList[i].ParameterItems.Count;
                                    for (int j = 0; j < countConfig; j++)
                                    {
                                        if (Market.InvestorGroupList[i].ParameterItems[j].Code == ListInvestorGroupConfig[a].Code)
                                        {
                                            #region FILTER CODE GET NAME UPDATE BEFORE
                                            switch (Market.InvestorGroupList[i].ParameterItems[j].Code)
                                            {
                                                case "G01":
                                                    if (Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("enable: enable - ");
                                                    else
                                                        beforeContent.Append("enable: disable - ");
                                                    break;
                                                case "G02":
                                                    beforeContent.Append("leverage by default: " + Market.InvestorGroupList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "G03":
                                                    break;
                                                case "G04":
                                                    beforeContent.Append("timeout: " + Market.InvestorGroupList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "G05":
                                                    break;
                                                case "G06":
                                                    break;
                                                case "G07":
                                                    break;
                                                case "G08":
                                                    break;
                                                case "G09":
                                                    break;
                                                case "G10":
                                                    break;
                                                case "G11":
                                                    break;
                                                case "G12":
                                                    break;
                                                case "G13":
                                                    break;
                                                case "G14":
                                                    break;
                                                case "G15":
                                                    break;
                                                case "G16":
                                                    break;
                                                case "G17":
                                                    break;
                                                case "G18":
                                                    break;
                                                case "G19":
                                                    beforeContent.Append("margin call level: " + Market.InvestorGroupList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "G20":
                                                    beforeContent.Append("stop out level: " + Market.InvestorGroupList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "G21":
                                                    break;
                                                case "G22":
                                                    beforeContent.Append("owner: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G23":
                                                    beforeContent.Append("group name: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G24":
                                                    beforeContent.Append("deposit by default: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G25":
                                                    break;
                                                case "G26":
                                                    beforeContent.Append("free margin calculation: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G27":
                                                    beforeContent.Append("virtual credit: " + Market.InvestorGroupList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "G28":
                                                    if (Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("skip fully hedged accounts when checking for stop out: enabled - ");
                                                    else
                                                        beforeContent.Append("skip fully hedged accounts when checking for stop out: disabled - ");
                                                    break;
                                                case "G29":
                                                    if (Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("enable email: enabled - ");
                                                    else
                                                        beforeContent.Append("enable email: disabled - ");
                                                    break;
                                                case "G30":
                                                    beforeContent.Append("SMTP server: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G31":
                                                    break;
                                                case "G32":
                                                    beforeContent.Append("SMTP login: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G33": //BECAUSE PASSWORD SECURITY 
                                                    break;
                                                case "G34":
                                                    beforeContent.Append("sender email: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G35":
                                                    beforeContent.Append("signature: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "G36":
                                                    break;
                                                case "G37":
                                                    beforeContent.Append("display name: " + Market.InvestorGroupList[i].ParameterItems[j].StringValue + " -");
                                                    break;
                                            }
                                            #endregion

                                            #region FILTER CODE GET NAME UPDATE AFTER
                                            switch (ListInvestorGroupConfig[a].Code)
                                            {
                                                case "G01":
                                                    if (ListInvestorGroupConfig[a].BoolValue == 1)
                                                        afterContent.Append("enable: enable - ");
                                                    else
                                                        afterContent.Append("enable: disable - ");
                                                    break;
                                                case "G02":
                                                    afterContent.Append("leverage by default: " + ListInvestorGroupConfig[a].NumValue + " - ");
                                                    break;
                                                case "G03":
                                                    break;
                                                case "G04":
                                                    afterContent.Append("timeout: " + ListInvestorGroupConfig[a].NumValue + " - ");
                                                    break;
                                                case "G05":
                                                    break;
                                                case "G06":
                                                    break;
                                                case "G07":
                                                    break;
                                                case "G08":
                                                    break;
                                                case "G09":
                                                    break;
                                                case "G10":
                                                    break;
                                                case "G11":
                                                    break;
                                                case "G12":
                                                    break;
                                                case "G13":
                                                    break;
                                                case "G14":
                                                    break;
                                                case "G15":
                                                    break;
                                                case "G16":
                                                    break;
                                                case "G17":
                                                    break;
                                                case "G18":
                                                    break;
                                                case "G19":
                                                    afterContent.Append("margin call level: " + ListInvestorGroupConfig[a].NumValue + " - ");
                                                    break;
                                                case "G20":
                                                    afterContent.Append("stop out level: " + ListInvestorGroupConfig[a].NumValue + " - ");
                                                    break;
                                                case "G21":
                                                    break;
                                                case "G22":
                                                    afterContent.Append("owner: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G23":
                                                    afterContent.Append("group name: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G24":
                                                    afterContent.Append("deposit by default: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G25":
                                                    break;
                                                case "G26":
                                                    afterContent.Append("free margin calculation: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G27":
                                                    afterContent.Append("virtual credit: " + ListInvestorGroupConfig[a].NumValue + " - ");
                                                    break;
                                                case "G28":
                                                    if (ListInvestorGroupConfig[a].BoolValue == 1)
                                                        afterContent.Append("skip fully hedged accounts when checking for stop out: enabled - ");
                                                    else
                                                        afterContent.Append("skip fully hedged accounts when checking for stop out: disabled - ");
                                                    break;
                                                case "G29":
                                                    if (ListInvestorGroupConfig[a].BoolValue == 1)
                                                        afterContent.Append("enable email: enabled - ");
                                                    else
                                                        afterContent.Append("enable email: disabled - ");
                                                    break;
                                                case "G30":
                                                    afterContent.Append("SMTP server: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G31":
                                                    break;
                                                case "G32":
                                                    afterContent.Append("SMTP login: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G33": //BECAUSE PASSWORD SECURITY 
                                                    break;
                                                case "G34":
                                                    afterContent.Append("sender email: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G35":
                                                    afterContent.Append("signature: " + ListInvestorGroupConfig[a].StringValue + " - ");
                                                    break;
                                                case "G36":
                                                    break;
                                                case "G37":
                                                    afterContent.Append("display name: " + ListInvestorGroupConfig[a].StringValue + " -");
                                                    break;
                                            }
                                            #endregion

                                            if (ListInvestorGroupConfig[a].Code == "G01")
                                            {
                                                bool isEnable = false;
                                                if (ListInvestorGroupConfig[a].BoolValue == 1)
                                                    isEnable = true;
                                                Business.Market.InvestorGroupList[i].IsEnable = isEnable;
                                            }

                                            if (ListInvestorGroupConfig[a].Code == "G19")
                                            {
                                                Business.Market.InvestorGroupList[i].MarginCall = double.Parse(ListInvestorGroupConfig[a].NumValue);
                                            }

                                            if (ListInvestorGroupConfig[a].Code == "G20")
                                            {
                                                Business.Market.InvestorGroupList[i].MarginStopOut = double.Parse(ListInvestorGroupConfig[a].NumValue);
                                            }

                                            if (ListInvestorGroupConfig[a].Code == "G26")
                                            {
                                                Business.Market.InvestorGroupList[i].FreeMargin = ListInvestorGroupConfig[a].StringValue;
                                            }

                                            //Set ParameterId For Config
                                            ListInvestorGroupConfig[a].ParameterItemID = Market.InvestorGroupList[i].ParameterItems[j].ParameterItemID;

                                            //Update Value Parameter Item
                                            Market.InvestorGroupList[i].ParameterItems[j].Name = ListInvestorGroupConfig[a].Name;
                                            Market.InvestorGroupList[i].ParameterItems[j].Code = ListInvestorGroupConfig[a].Code;
                                            Market.InvestorGroupList[i].ParameterItems[j].BoolValue = ListInvestorGroupConfig[a].BoolValue;
                                            Market.InvestorGroupList[i].ParameterItems[j].StringValue = ListInvestorGroupConfig[a].StringValue;
                                            Market.InvestorGroupList[i].ParameterItems[j].NumValue = ListInvestorGroupConfig[a].NumValue;
                                            Market.InvestorGroupList[i].ParameterItems[j].DateValue = ListInvestorGroupConfig[a].DateValue;

                                            //Set ParameterID 
                                            ListInvestorGroupConfig[a].ParameterItemID = Market.InvestorGroupList[i].ParameterItems[j].ParameterItemID;
                                            break;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }

                    Result = ParameterItem.DBWInvestorGroupConfigInstance.UpdateInvestorGroupConfig(ListInvestorGroupConfig[a].ParameterItemID,
                        ListInvestorGroupConfig[a].SecondParameterID, -1, ListInvestorGroupConfig[a].Name, ListInvestorGroupConfig[a].Code,
                        ListInvestorGroupConfig[a].BoolValue, ListInvestorGroupConfig[a].StringValue, ListInvestorGroupConfig[a].NumValue,
                        ListInvestorGroupConfig[a].DateValue);
                }
            }

            string tempBeforeContent = beforeContent.ToString();
            if (tempBeforeContent.EndsWith(" - "))
                tempBeforeContent = tempBeforeContent.Remove(tempBeforeContent.Length - 2, 2);

            string tempAfterContent = afterContent.ToString();
            if (tempAfterContent.EndsWith(" - "))
                tempAfterContent = tempAfterContent.Remove(tempAfterContent.Length - 2, 2);

            content.Append(tempBeforeContent + " -> " + tempAfterContent);

            TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update group config]", ipAddress, code);

            return Result;
        }

        /// <summary>
        /// DELETE INVESTOR GROUP CONFIG
        /// </summary>
        /// <param name="InvestorGroupConfigID"></param>
        /// <returns></returns>
        internal bool DeleteInvestorGroupConfig(int InvestorGroupConfigID)
        {
            bool Result = false;
            Result = ParameterItem.DBWInvestorGroupConfigInstance.DeleteInvestorGroupConfig(InvestorGroupConfigID);

            if (Result == true)
            {
                if (Market.InvestorGroupList != null)
                {
                    int count = Market.InvestorGroupList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countRef = Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countRef; j++)
                            {
                                if (Market.InvestorGroupList[i].ParameterItems[j].ParameterItemID == InvestorGroupConfigID)
                                {
                                    Market.InvestorGroupList[i].ParameterItems.Remove(Market.InvestorGroupList[i].ParameterItems[j]);
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
