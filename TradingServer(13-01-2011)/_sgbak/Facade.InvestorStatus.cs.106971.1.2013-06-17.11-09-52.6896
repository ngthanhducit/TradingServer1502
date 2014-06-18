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
        public static List<Business.InvestorStatus> FacadeGetAllInvestorStatus()
        {
            return Facade.InvestorStatusInstance.GetAllInvestorStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorStatusID"></param>
        /// <returns></returns>
        public static Business.InvestorStatus FacadeGetInvestorStatusByID(int InvestorStatusID)
        {
            return Facade.InvestorStatusInstance.GetInvestorStatusByInvestorStatusID(InvestorStatusID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static int FacadeAddNewInvestorStatus(string Name)
        {
            return Facade.InvestorStatusInstance.AddNewInvestorStatus(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorStatusID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteInvestorStatus(int InvestorStatusID)
        {
            return Facade.InvestorStatusInstance.DeleteInvestorStatusByID(InvestorStatusID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="InvestorStatusID"></param>
        public static void FacadeUpdateInvestorStatus(string Name, int InvestorStatusID)
        {
            Facade.InvestorStatusInstance.UpdateInvestorStatus(Name, InvestorStatusID);
        }        
    }
}
