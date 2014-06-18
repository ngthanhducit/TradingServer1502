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
        public static List<Business.IAgentGroup> FacadeGetAllIAgentGroup()
        {
            return Facade.IAgentGroupInstance.GetAllIAgentGroup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentGroupID"></param>
        /// <returns></returns>
        public static Business.IAgentGroup FacadeGetIAgentGroupByIAgentGroupID(int IAgentGroupID)
        {
            return Facade.IAgentGroupInstance.GetIAgentGroupByIAgentGroupID(IAgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static List<Business.IAgentGroup> FacadeGetIAgentGroupByAgentID(int AgentID)
        {
            return Facade.IAgentGroupInstance.GetIAgentGroupByAgentID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static List<Business.IAgentGroup> FacadeGetIAgentGroupByInvestorGroupID(int InvestorGroupID)
        {
            return Facade.IAgentGroupInstance.GetIAgentGroupByInvestorGroupID(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static int FacadeAddNewIAgentGroup(int AgentID, int InvestorGroupID)
        {
            return Facade.IAgentGroupInstance.AddNewIAgentGroup(AgentID, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static bool FacadeUpdateIAgentGroup(int AgentID, List<int> ListInvestorGroupID)
        {
            return Facade.IAgentGroupInstance.UpdateIAgentGroup(AgentID, ListInvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentGroupID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIAgentGroupByID(int IAgentGroupID)
        {
            return Facade.IAgentGroupInstance.DeleteIAgentGroupByIAgentGroupID(IAgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static int FacadeDeleteIAgentGroupByAgentID(int AgentID)
        {
            return Facade.IAgentGroupInstance.DeleteIAgentGroupByAgentID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIAgentGroupByInvestorGroupID(int InvestorGroupID)
        {
            return Facade.IAgentGroupInstance.DeleteIAgentGroupByInvestorGroupID(InvestorGroupID);
        }        
    }
}
