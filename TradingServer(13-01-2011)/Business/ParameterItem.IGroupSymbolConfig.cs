using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class ParameterItem
    {        
        /// <summary>
        /// GET ALL IGROUP SYMBOL CONFIG IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllIGroupSymbolConfig()
        {
            return ParameterItem.DBWIGroupSymbolConfig.GetAllIGroupSymbolConfig();
        }

        /// <summary>
        /// GET IGROUP SYMBOL CONFIG BY IGROUP SYMBOL ID
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetIGroupSymbolConfigByIGroupSymbolID(int IGroupSymbolID)
        {
            return ParameterItem.DBWIGroupSymbolConfig.GetIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);
        }

        /// <summary>
        /// GET IGROUP SYMBOL CONFIG BY IGROUP SYMBOL CONFIG ID
        /// </summary>
        /// <param name="IGroupSymbolConfigID"></param>
        /// <returns></returns>
        internal Business.ParameterItem GetIGroupSymbolConfigByID(int IGroupSymbolConfigID)
        {
            return ParameterItem.DBWIGroupSymbolConfig.GetIGroupSymbolConfigByID(IGroupSymbolConfigID);
        }

        /// <summary>
        /// ADD NEW IGROUP SYMBOL CONFIG
        /// </summary>
        /// <param name="ListParameterItem"></param>
        /// <returns></returns>
        internal int AddNewIGroupSymbolConfig(List<Business.ParameterItem> ListParameterItem)
        {
            int Result = -1;

            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].IGroupSymbolID == ListParameterItem[0].SecondParameterID)
                    {
                        int countParameter = ListParameterItem.Count;
                        for (int j = 0; j < countParameter; j++)
                        {
                            Result = ParameterItem.DBWIGroupSymbolConfig.AddIGroupSymbolConfig(ListParameterItem[j].SecondParameterID, -1, ListParameterItem[j].Name,
                                ListParameterItem[j].Code, ListParameterItem[j].BoolValue, ListParameterItem[j].StringValue, ListParameterItem[j].NumValue, ListParameterItem[j].DateValue);

                            ListParameterItem[j].ParameterItemID = Result;

                            if (Result > 0)
                            {
                                if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig == null)
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig = new List<ParameterItem>();

                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Add(ListParameterItem[j]);
                            }
                        }
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// UPDATE IGROUP SYMBOL CONFIG
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal bool UpdateIGroupSymbolConfig(Business.ParameterItem objParameterItem)
        {
            bool Result = false;
            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].IGroupSymbolID == objParameterItem.SecondParameterID)
                    {
                        if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig != null)
                        {
                            int countParameterItem = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count;
                            for (int j = 0; j < countParameterItem; j++)
                            {
                                if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Code == objParameterItem.Code)
                                {
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Name = objParameterItem.Name;
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Code = objParameterItem.Code;
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].BoolValue = objParameterItem.BoolValue;
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].StringValue = objParameterItem.StringValue;
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].NumValue = objParameterItem.NumValue;
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].DateValue = objParameterItem.DateValue;

                                    //Set Parameter Item ID
                                    objParameterItem.ParameterItemID = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].ParameterItemID;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }

                Result = ParameterItem.DBWIGroupSymbolConfig.UpdateIGroupSymbolConfig(objParameterItem.ParameterItemID, objParameterItem.SecondParameterID, -1,
                    objParameterItem.Name, objParameterItem.Code, objParameterItem.BoolValue, objParameterItem.StringValue, objParameterItem.NumValue, objParameterItem.DateValue);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolConfigID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSymbolConfig(int IGroupSymbolConfigID)
        {
            return ParameterItem.DBWIGroupSymbolConfig.DeleteIGroupSymbolConfig(IGroupSymbolConfigID);
        }

        /// <summary>
        /// DELETE IGROUPSECURITYCONFIG BY IGROUPSECURITY ID IN RAM AND DATABASE
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSymbolConfigByIGroupSymbolID(int IGroupSymbolID)
        {
            bool Result = false;
            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].IGroupSymbolID == IGroupSymbolID)
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig != null &&
                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count > 0)
                            {
                                //bool ResultDelete = TradingServer.Facade.FacadeDeleteIGroupSecurityConfigByIGroupSecurityID(IGroupSecurityID);
                                bool resultDelete = ParameterItem.dbwIGroupSymbolConfig.DeleteIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);

                                if (resultDelete == true)
                                {
                                    int countConfig = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count;
                                    for (int j = 0; j < Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count; j++)
                                    {
                                        Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.RemoveAt(j);
                                    }

                                    Result = true;
                                }
                            }

                            ts.Complete();
                        }

                        break;
                    }
                }
            }

            return Result;
        }        
    }
}
