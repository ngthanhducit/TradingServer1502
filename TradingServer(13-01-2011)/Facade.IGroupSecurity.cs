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
        public static List<Business.IGroupSecurity> FacadeGetIGroupSecurity()
        {
            return Facade.IGroupSecurityInstance.GetAllIGroupSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        public static Business.IGroupSecurity FacadeGetIGroupSecurityByID(int IGroupSecurityID)
        {
            return Facade.IGroupSecurityInstance.GetIGroupSecurityByID(IGroupSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSecurity> FacadeGetIGroupSecurityBySecurityID(int SecurityID)
        {
            return Facade.IGroupSecurityInstance.GetIGroupSecurityBySecurityIDCommand(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSecurity> FacadeGetIGroupSecurityByInvestorGroupID(int InvestorGroupID)
        {
            return Facade.IGroupSecurityInstance.GetIGroupSecurityBySecurityIDCommand(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSecurity> FacadeGetIGroupSecurityByInvestorGroup(int InvestorGroupID)
        {
            return Facade.IGroupSecurityInstance.GetIGroupSecurityByInvestorGroup(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        public static int FacadeAddIGroupSecurity(int InvestorGroupID, int SecurityID)
        {
            return Facade.IGroupSecurityInstance.AddIGroupSecurityCommand(InvestorGroupID, SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorGroupID"></param>
        /// <param name="securityID"></param>
        /// <param name="iGroupSecurityID"></param>
        public static void FacadeResetIGroupSecurityInCommand(int investorGroupID, int securityID, int iGroupSecurityID)
        {
            Facade.IGroupSecurityInstance.ResetIGroupSecurityInCommand(investorGroupID, securityID, iGroupSecurityID);
        }

        /// <summary>
        /// AFTER ADD NEW IGROUPSECURITY THEN FILL AGAIN IGROUPSECURITY TO COMMAND
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="InvestorGroupID"></param>
        public static bool FacadeUpdateIGroupSecurity(int IGroupSecurityID, int SecurityID, int InvestorGroupID)
        {
            return Facade.IGroupSecurityInstance.UpdateIGroupSecurityCommand(IGroupSecurityID, SecurityID, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        public static bool FacadeDeleteIGroupSecurityByIGroupSecurityID(int IGroupSecurityID)
        {
            return Facade.IGroupSecurityInstance.DeleteIGroupSecurityByIGroupSecurityIDCommand(IGroupSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        public static bool FacadeDeleteIGroupSecurityByInvestorGroupID(int InvestorGroupID)
        {
            return Facade.IGroupSecurityInstance.DeleteIGroupSecurityByInvestorGroupIDCommand(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        public static bool FacadeDeleteIGroupSecurityBySecurityID(int SecurityID)
        {
            return Facade.IGroupSecurityInstance.DeleteIGroupSecurityBySecurityIDCommand(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountIGroupSecurity()
        {
            return Facade.IGroupSecurityInstance.CountTotalIGroupSecurity();
        }
    }
}
