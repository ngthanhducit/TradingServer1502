using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class AgentGroup
    {
        public int AgentGroupID { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        #region Create Instance Class DBWAgentGroup
        private static DBW.DBWAgentGroup dbwAgentGroup;
        private static DBW.DBWAgentGroup DBWAgentGroupInstance
        {
            get
            {
                if (AgentGroup.dbwAgentGroup == null)
                {
                    AgentGroup.dbwAgentGroup = new DBW.DBWAgentGroup();
                }
                return AgentGroup.dbwAgentGroup;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.AgentGroup> GetAllAgentGroup()
        {
            return AgentGroup.DBWAgentGroupInstance.GetAllAgentGroup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal Business.AgentGroup GetAgentGroupByAgentGroupID(int AgentGroupID)
        {
            return AgentGroup.DBWAgentGroupInstance.GetAgentGroupByAgentGroupID(AgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int CreateNewAgentGroup(string Name,string Comment)
        {
            return AgentGroup.DBWAgentGroupInstance.AddNewAgentGroup(Name, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal bool DeleteAgentGroupByID(int AgentGroupID)
        {
            return AgentGroup.DBWAgentGroupInstance.DeleteAgentGroupByAgentGroupID(AgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal bool UpdateAgentGroup(int AgentGroupID, string Name,string Comment)
        {
            return AgentGroup.DBWAgentGroupInstance.UpdateAgentGroup(AgentGroupID, Name, Comment);
        }
    }
}
