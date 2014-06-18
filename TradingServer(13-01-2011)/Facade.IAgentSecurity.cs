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
        public static List<Business.IAgentSecurity> FacadeGetIAgentSecurity()
        {
            return Facade.IAgentSecurityInstance.GetAllIAgentSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentSecurityID"></param>
        /// <returns></returns>
        public static Business.IAgentSecurity FacadeGetIAgentSecurityByID(int IAgentSecurityID)
        {
            return Facade.IAgentSecurityInstance.GetIAgentSecurityByID(IAgentSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static List<Business.IAgentSecurity> FacadeGetIAgentSecurityByAgentID(int AgentID)
        {
            return Facade.IAgentSecurityInstance.GetIAgentSecurityByAgentID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        public static List<Business.IAgentSecurity> FacadeGetIAgentSecurityBySecurityID(int SecurityID)
        {
            return Facade.IAgentSecurityInstance.GetIAgentSecurityBySecurityID(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="IsUse"></param>
        /// <param name="MinLots"></param>
        /// <param name="MaxLots"></param>
        /// <returns></returns>
        public static int FacadeAddNewIAgentSecurity(int AgentID, int SecurityID, bool IsUse, string MinLots, string MaxLots)
        {
            return Facade.IAgentSecurityInstance.AddNewIAgentSecurity(AgentID, SecurityID, IsUse, MinLots, MaxLots);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListIAgentSecurity"></param>
        /// <returns></returns>
        public static int FacadeAddListIAgentSecurity(List<Business.IAgentSecurity> ListIAgentSecurity)
        {
            return Facade.IAgentSecurityInstance.AddListIAgentSecurity(ListIAgentSecurity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListIAgentSecurity"></param>
        /// <returns></returns>
        public static bool FacadeUpdateIAgentSecurity(List<Business.IAgentSecurity> ListIAgentSecurity)
        {
            return Facade.IAgentSecurityInstance.UpdateIAgentSecurity(ListIAgentSecurity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentSecurityID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIAgentSecurityByID(int IAgentSecurityID)
        {
            return Facade.IAgentSecurityInstance.DeleteIAgentSecurityByID(IAgentSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static int FacadeDeleteIAgentSecurityByAgentID(int AgentID)
        {
            return Facade.IAgentSecurityInstance.DeleteIAgentSecurityByAgentID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIAgentSecurityBySecurityID(int SecurityID)
        {
            return Facade.IAgentSecurityInstance.DeleteIAgentSecurityBySecurityID(SecurityID);
        }        
    }
}
