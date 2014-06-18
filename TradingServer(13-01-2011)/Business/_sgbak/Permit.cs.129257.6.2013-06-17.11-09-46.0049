using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class Permit
    {
        public int PermitID { get; set; }
        public int AgentGroupID { get; set; }
        public int AgentID { get; set; }
        public Business.Role Role { get; set; }

        #region Create Instance Class DBWPermit
        private static DBW.DBWPermit dbwPermit;
        private static DBW.DBWPermit DBWPermitInstance
        {
            get
            {
                if (Permit.dbwPermit == null)
                {
                    Permit.dbwPermit = new DBW.DBWPermit();
                }

                return Permit.dbwPermit;
            }
        }
        #endregion

        /// <summary>
        /// Constructure
        /// </summary>
        public Permit()
        { 
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Permit> GetAllPermit()
        {
            return Permit.DBWPermitInstance.GetAllPermit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PermitID"></param>
        /// <returns></returns>
        internal Business.Permit GetPermitByID(int PermitID)
        {
            return Permit.DBWPermitInstance.GetPermitByPermitID(PermitID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal List<Business.Permit> GetPermitByAgentGroupID(int AgentGroupID)
        {
            return Permit.DBWPermitInstance.GetPermitByAgentGroupID(AgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal List<Business.Permit> GetPermitByAgentID(int AgentID)
        {
            return Permit.DBWPermitInstance.GetPermitByAgentID(AgentID);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        internal List<Business.Permit> GetPermitByRoleID(int RoleID)
        {
            return Permit.DBWPermitInstance.GetPermitByRoleID(RoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        internal int AddNewPermit(int AgentGroupID, int AgentID, int RoleID)
        {
            int result = -1;
            result = Permit.DBWPermitInstance.AddNewPermit(AgentGroupID, AgentID, RoleID);
            if (result == 1)
            {
                int count = Business.Market.AgentList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.AgentList[i].AgentID == AgentID)
                    {
                        Business.Market.AgentList[i].AgentGroupID = AgentGroupID;
                        Business.Role role = new Role();
                        role = Facade.FacadeGetRoleByRoleID(RoleID);                        
                        #region Enable Role
                        switch (role.Code)
                        {
                            case "R01":
                                Business.Market.AgentList[i].IsManager = true;
                                break;
                            case "R02":
                                Business.Market.AgentList[i].IsSuperviseTrades = true;
                                break;
                            case "R03":
                                Business.Market.AgentList[i].IsAdmin = true;
                                break;
                            case "R04":
                                Business.Market.AgentList[i].IsAccountant = true;
                                break;
                            case "R05":
                                Business.Market.AgentList[i].IsReports = true;
                                break;
                            case "R06":
                                Business.Market.AgentList[i].IsRiskManager = true;
                                break;
                            case "R07":
                                Business.Market.AgentList[i].IsInternalMail = true;
                                break;
                            case "R08":
                                Business.Market.AgentList[i].IsJournals = true;
                                break;
                            case "R09":
                                Business.Market.AgentList[i].IsSendNews = true;
                                break;
                            case "R10":
                                Business.Market.AgentList[i].IsMarketWatch = true;
                                break;
                            case "R11":
                                Business.Market.AgentList[i].IsConections = true;
                                break;
                            case "R12":
                                Business.Market.AgentList[i].IsPersonalDetails = true;
                                break;
                            case "R13":
                                Business.Market.AgentList[i].IsConfigServerPlugins = true;
                                break;
                            case "R14":
                                Business.Market.AgentList[i].IsAutomaticServerReports = true;
                                break;
                            case "R15":
                                Business.Market.AgentList[i].IsDealer = true;
                                break;
                            case "R16":
                                Business.Market.AgentList[i].IsEditDealer = true;
                                break;
                            case "R17":
                                Business.Market.AgentList[i].IsDownloadStatements = true;
                                break;
                        }

                        #endregion
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        //internal int AddNewPermitHasCode(int AgentGroupID, int AgentID, string Code)
        //{
        //    Business.Role role = new Business.Role();
        //    role = Facade.FacadeGetRoleByCode(Code);
        //    if (role == null)
        //    {
        //        return -1;
        //    }
        //    else return Permit.DBWPermitInstance.AddNewPermit(AgentGroupID, AgentID, role.RoleID);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PermitID"></param>
        /// <param name="AgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        //internal bool UpdatePermitHasCode(int PermitID, int AgentGroupID, int AgentID, string Code)
        //{
        //    Business.Role role = new Business.Role();
        //    role = Facade.FacadeGetRoleByCode(Code);
        //    if (role == null)
        //    {
        //        return false;
        //    }
        //    else return Permit.DBWPermitInstance.UpdatePermit(PermitID, AgentGroupID, AgentID, role.RoleID);
        //}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <param name="ListRoleID"></param>
        /// <returns></returns>
        internal bool UpdatePermit(int AgentID, List<int> ListRoleID)
        {
            int resultDel = -1;
            int resultAdd = 0;            
            resultDel = this.DeletePermitByAgentID(AgentID);
            if (resultDel != -1)
            {
               resultAdd = Permit.DBWPermitInstance.AddListPermit(AgentID, ListRoleID);
            }
            if (resultAdd != -1 && resultDel != -1)
            {

                for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
                {
                    if (Business.Market.AgentList[i].AgentID == AgentID)
                    {
                        List<Business.Role> listRole = new List<Role>();
                        listRole = Facade.FacadeGetListRoleByListRoleID(ListRoleID);
                        Agent agent = new Agent();
                        agent = Business.Market.AgentList[i];
                        #region Disable All Role
                        agent.IsManager = false;
                        agent.IsSuperviseTrades = false;
                        agent.IsAdmin = false;
                        agent.IsAccountant = false;
                        agent.IsReports = false;
                        agent.IsDownloadStatements = false;
                        agent.IsRiskManager = false;
                        agent.IsInternalMail = false;
                        agent.IsJournals = false;
                        agent.IsSendNews = false;
                        agent.IsMarketWatch = false;
                        agent.IsConections = false;
                        agent.IsPersonalDetails = false;
                        agent.IsConfigServerPlugins = false;
                        agent.IsAutomaticServerReports = false;
                        agent.IsDealer = false;
                        agent.IsEditDealer = false;
                        #endregion
                        int countRole = listRole.Count;
                        for (int r = 0; r < countRole; r++)
                        {
                            #region Enable Role
                            switch (listRole[r].Code)
                            {
                                case "R01":
                                    agent.IsManager = true;
                                    break;
                                case "R02":
                                    agent.IsSuperviseTrades = true;
                                    break;
                                case "R03":
                                    agent.IsAdmin = true;
                                    break;
                                case "R04":
                                    agent.IsAccountant = true;
                                    break;
                                case "R05":
                                    agent.IsReports = true;
                                    break;
                                case "R06":
                                    agent.IsRiskManager = true;
                                    break;
                                case "R07":
                                    agent.IsInternalMail = true;
                                    break;
                                case "R08":
                                    agent.IsJournals = true;
                                    break;
                                case "R09":
                                    agent.IsSendNews = true;
                                    break;
                                case "R10":
                                    agent.IsMarketWatch = true;
                                    break;
                                case "R11":
                                    agent.IsConections = true;
                                    break;
                                case "R12":
                                    agent.IsPersonalDetails = true;
                                    break;
                                case "R13":
                                    agent.IsConfigServerPlugins = true;
                                    break;
                                case "R14":
                                    agent.IsAutomaticServerReports = true;
                                    break;
                                case "R15":
                                    agent.IsDealer = true;
                                    break;
                                case "R16":
                                    agent.IsEditDealer = true;
                                    break;
                                case "R17":
                                    agent.IsDownloadStatements = true;
                                    break;
                            }

                            #endregion
                        }
                        if (agent.IsDealer == false) agent.IsBusy = true;
                        break;
                    }
                }

                for (int i = Business.Market.AdminList.Count - 1; i >= 0; i--)
                {
                    if (Business.Market.AdminList[i].AgentID == AgentID)
                    {
                        List<Business.Role> listRole = new List<Role>();
                        listRole = Facade.FacadeGetListRoleByListRoleID(ListRoleID);
                        Business.Agent agent = new Agent();
                        agent = Business.Market.AdminList[i];
                        #region Disable All Role
                        agent.IsManager = false;
                        agent.IsSuperviseTrades = false;
                        agent.IsAdmin = false;
                        agent.IsAccountant = false;
                        agent.IsReports = false;
                        agent.IsDownloadStatements = false;
                        agent.IsRiskManager = false;
                        agent.IsInternalMail = false;
                        agent.IsJournals = false;
                        agent.IsSendNews = false;
                        agent.IsMarketWatch = false;
                        agent.IsConections = false;
                        agent.IsPersonalDetails = false;
                        agent.IsConfigServerPlugins = false;
                        agent.IsAutomaticServerReports = false;
                        agent.IsDealer = false;
                        agent.IsEditDealer = false;
                        #endregion
                        int countRole = listRole.Count;
                        for (int r = 0; r < countRole; r++)
                        {
                            #region Enable Role
                            switch (listRole[r].Code)
                            {
                                case "R01":
                                    agent.IsManager = true;
                                    break;
                                case "R02":
                                    agent.IsSuperviseTrades = true;
                                    break;
                                case "R03":
                                    agent.IsAdmin = true;
                                    break;
                                case "R04":
                                    agent.IsAccountant = true;
                                    break;
                                case "R05":
                                    agent.IsReports = true;
                                    break;
                                case "R06":
                                    agent.IsRiskManager = true;
                                    break;
                                case "R07":
                                    agent.IsInternalMail = true;
                                    break;
                                case "R08":
                                    agent.IsJournals = true;
                                    break;
                                case "R09":
                                    agent.IsSendNews = true;
                                    break;
                                case "R10":
                                    agent.IsMarketWatch = true;
                                    break;
                                case "R11":
                                    agent.IsConections = true;
                                    break;
                                case "R12":
                                    agent.IsPersonalDetails = true;
                                    break;
                                case "R13":
                                    agent.IsConfigServerPlugins = true;
                                    break;
                                case "R14":
                                    agent.IsAutomaticServerReports = true;
                                    break;
                                case "R15":
                                    agent.IsDealer = true;
                                    break;
                                case "R16":
                                    agent.IsEditDealer = true;
                                    break;
                                case "R17":
                                    agent.IsDownloadStatements = true;
                                    break;
                            }

                            #endregion
                        }
                        if (agent.IsDealer == false) agent.IsBusy = true;
                        break;
                    }
                }
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal bool DeletePermitByID(int PermitID)
        {
            bool result = false;
            result = Permit.DBWPermitInstance.DeletePermitByID(PermitID);
            if (result)
            {
                Business.Permit permit = new Permit();
                permit = Facade.FacadeGetPermitByPermitID(PermitID);
                int count = Business.Market.AgentList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.AgentList[i].AgentID == permit.AgentID)
                    {
                        Business.Role role = new Role();
                        role = Facade.FacadeGetRoleByRoleID(permit.Role.RoleID);
                        #region Disable Role
                        switch (role.Code)
                        {
                            case "R01":
                                Business.Market.AgentList[i].IsManager = false;
                                break;
                            case "R02":
                                Business.Market.AgentList[i].IsSuperviseTrades = false;
                                break;
                            case "R03":
                                Business.Market.AgentList[i].IsAdmin = false;
                                break;
                            case "R04":
                                Business.Market.AgentList[i].IsAccountant = false;
                                break;
                            case "R05":
                                Business.Market.AgentList[i].IsReports = false;
                                break;
                            case "R06":
                                Business.Market.AgentList[i].IsRiskManager = false;
                                break;
                            case "R07":
                                Business.Market.AgentList[i].IsInternalMail = false;
                                break;
                            case "R08":
                                Business.Market.AgentList[i].IsJournals = false;
                                break;
                            case "R09":
                                Business.Market.AgentList[i].IsSendNews = false;
                                break;
                            case "R10":
                                Business.Market.AgentList[i].IsMarketWatch = false;
                                break;
                            case "R11":
                                Business.Market.AgentList[i].IsConections = false;
                                break;
                            case "R12":
                                Business.Market.AgentList[i].IsPersonalDetails = false;
                                break;
                            case "R13":
                                Business.Market.AgentList[i].IsConfigServerPlugins = false;
                                break;
                            case "R14":
                                Business.Market.AgentList[i].IsAutomaticServerReports = false;
                                break;
                            case "R15":
                                Business.Market.AgentList[i].IsDealer = false;
                                break;
                            case "R16":
                                Business.Market.AgentList[i].IsEditDealer = false;
                                break;
                            case "R17":
                                Business.Market.AgentList[i].IsDownloadStatements = false;
                                break;
                        }

                        #endregion
                        return result;
                    }
                }
            }
            return result;
        }

        internal int DeletePermitByAgentID(int AgentID)
        {
            return Permit.DBWPermitInstance.DeletePermitByAgentID(AgentID);     
        }

        internal int AddNewPermit(int AgentID, List<int> ListRoleID)
        {
            int result = -1;
            result = Permit.DBWPermitInstance.AddListPermit(AgentID, ListRoleID);            
            return result;
        }

    }
}
