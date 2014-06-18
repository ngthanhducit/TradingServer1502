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
        public static List<Business.AgentGroup> FacadeGetAllAgentGroup()
        {
            return Facade.AgentGroupInstance.GetAllAgentGroup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        public static Business.AgentGroup FacadeGetAgentGroupByAgentGroupID(int AgentGroupID)
        {
            return Facade.AgentGroupInstance.GetAgentGroupByAgentGroupID(AgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static int FacadeAddNewAgentGroup(string Name, string Comment)
        {
            return Facade.AgentGroupInstance.CreateNewAgentGroup(Name, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static bool FacadeUpdateAgentGroup(int AgentGroupID, string Name, string Comment)
        {
            return Facade.AgentGroupInstance.UpdateAgentGroup(AgentGroupID, Name, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteAgentGroup(int AgentGroupID)
        {
            return Facade.AgentGroupInstance.DeleteAgentGroupByID(AgentGroupID);
        }        
    }
}
