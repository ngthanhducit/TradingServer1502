using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class InvestorGroup
    {
        /// <summary>
        /// Find Investor Group In List Investor Group Of Class Market
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal Business.InvestorGroup FindInvestorGroupByInvestorGropuID(int InvestorGroupID)
        {
            Business.InvestorGroup Result = new InvestorGroup();
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == InvestorGroupID)
                    {
                        Result = Business.Market.InvestorGroupList[i];
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetReportConfigInInvestorGroup(int InvestorGroupID)
        {
            List<Business.ParameterItem> Result = new List<ParameterItem>();

            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            Result = Business.Market.InvestorGroupList[i].ParameterItems;
                        }
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorGroup"></param>
        /// <returns></returns>
        internal int AddNewInvestorGroup(Business.InvestorGroup objInvestorGroup)
        {
            int Result = -1;
            //Call Function In Class DBWInvestorGroup. Add InvestorGroup To Database
            Result = DBWInvestorGroupInstance.AddNewInvestorGroup(objInvestorGroup);

            if (Result > 0)
            {
                objInvestorGroup.InvestorGroupID = Result;
                if (Market.InvestorGroupList != null)
                {
                    Market.InvestorGroupList.Add(objInvestorGroup);
                }
                else
                {
                    Market.InvestorGroupList = new List<InvestorGroup>();
                    Market.InvestorGroupList.Add(objInvestorGroup);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IvnestorGroupID"></param>
        internal bool DeleteInvestorGroup(int InvestorGroupID)
        {
            bool Result = false;

            Result = InvestorGroup.DBWInvestorGroupInstance.DeleteInvestorGroup(InvestorGroupID);

            if (Result == true)
            {
                //Find In Queue InvestorGroupList Of Class Market
                if (Market.InvestorGroupList != null)
                {
                    int count = Market.InvestorGroupList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.InvestorGroupList[i].InvestorGroupID == InvestorGroupID)
                        {
                            Market.InvestorGroupList.Remove(Market.InvestorGroupList[i]);
                        }
                    }
                }
            }

            return Result;
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        internal bool UpdateInvestorGroup(Business.InvestorGroup objInvestorGroup)
        {
            bool Result = false;
            Result = DBWInvestorGroupInstance.UpdateInvestorGroup(objInvestorGroup);

            if (Result == true)
            {
                //Update Investor Group In Class Market
                //
                if (Market.InvestorGroupList != null)
                {
                    int count = Market.InvestorGroupList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.InvestorGroupList[i].InvestorGroupID == objInvestorGroup.InvestorGroupID)
                        {
                            Market.InvestorGroupList[i].Name = objInvestorGroup.Name;
                            Market.InvestorGroupList[i].Owner = objInvestorGroup.Owner;
                            Market.InvestorGroupList[i].DefautDeposite = objInvestorGroup.DefautDeposite;
                        }
                    }
                }
            }

            return Result;
        }
    }
}
