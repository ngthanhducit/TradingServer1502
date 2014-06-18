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
        public static List<Business.Permit> FacadeGetAllPermit()
        {
            return Facade.PermitInstance.GetAllPermit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PermitID"></param>
        /// <returns></returns>
        public static Business.Permit FacadeGetPermitByPermitID(int PermitID)
        {
            return Facade.PermitInstance.GetPermitByID(PermitID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        public static List<Business.Permit> FacadeGetPermitByAgentGroupID(int AgentGroupID)
        {
            return Facade.PermitInstance.GetPermitByAgentGroupID(AgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static List<Business.Permit> FacadeGetPermitByAgentID(int AgentID)
        {
            return Facade.PermitInstance.GetPermitByAgentID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public static List<Business.Permit> FacadeGetPermitByRoleID(int RoleID)
        {
            return Facade.PermitInstance.GetPermitByRoleID(RoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public static int FacadeAddNewPermit(int AgentGroupID, int AgentID, int RoleID)
        {
            return Facade.PermitInstance.AddNewPermit(AgentGroupID, AgentID, RoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <param name="ListRoleID"></param>
        /// <returns></returns>
        public static bool FacadeUpdatePermit(int AgentID, List<int> ListRoleID)
        {
            return Facade.PermitInstance.UpdatePermit(AgentID, ListRoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PermitID"></param>
        /// <returns></returns>
        public static bool FacadeDeletePermitByID(int PermitID)
        {
            return Facade.PermitInstance.DeletePermitByID(PermitID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static int FacadeDeletePermitByAgentID(int AgentID)
        {
            return Facade.PermitInstance.DeletePermitByAgentID(AgentID);
        }        
    }
}
