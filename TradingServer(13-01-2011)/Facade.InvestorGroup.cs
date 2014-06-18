using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer
{
    public static partial class Facade
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.InvestorGroup> FacadeGetAllInvestorGroup()
        {
            return Facade.InvestorGroupInstance.GetAllInvestorGroup();
        }

        /// <summary>
        /// Find Investor Group In Database
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        //public static Business.InvestorGroup FacadeGetInvestorGroupByInvestorGroupID(int InvestorGroupID)
        //{
        //    return Facade.InvestorGroupInstance.GetInvestorGroupByInvestorGroupID(InvestorGroupID);
        //}

        /// <summary>
        /// Find Investor Group In List Investor Group Of Class Market
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static Business.InvestorGroup FacadeFindInvestorGroupByInvestorGroupID(int InvestorGroupID)
        {
            return Facade.InvestorGroupInstance.FindInvestorGroupByInvestorGropuID(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorGroup"></param>
        /// <returns></returns>
        public static int FacadeAddNewInvestorGroup(Business.InvestorGroup objInvestorGroup)
        {
            return Facade.InvestorGroupInstance.AddNewInvestorGroup(objInvestorGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorGroup"></param>
        public static bool FacadeUpdateInvestorGroup(Business.InvestorGroup objInvestorGroup)
        {
            return Facade.InvestorGroupInstance.UpdateInvestorGroup(objInvestorGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteInvestorGroup(int InvestorGroupID)
        {
            return Facade.InvestorGroupInstance.DeleteInvestorGroup(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountInvestorGroup()
        {
            return Facade.InvestorGroupInstance.CountTotalInvestorGroup();
        }
    }
}
