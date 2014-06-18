using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class IAgentGroup
    {
        public int IAgentGroupID { get; set; }
        public int AgentID { get; set; }
        public int InvestorGroupID { get; set; }

        #region Create Instance Class DBWIGroupSymbol
        private static DBW.DBWIAgentGroup dbwIAgentGroup;
        private static DBW.DBWIAgentGroup DBWIAgentGroupInstance
        {
            get
            {
                if (IAgentGroup.dbwIAgentGroup == null)
                {
                    IAgentGroup.dbwIAgentGroup = new DBW.DBWIAgentGroup();
                }
                return IAgentGroup.dbwIAgentGroup;
            }
        }
        #endregion

        internal List<Business.IAgentGroup> GetAllIAgentGroup()
        {
            return IAgentGroup.DBWIAgentGroupInstance.GetAllIAgentGroup();
        }

        internal Business.IAgentGroup GetIAgentGroupByIAgentGroupID(int IAgentGroupID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.GetIAgentGroupByIAgentGroupID(IAgentGroupID);
        }

        internal List<Business.IAgentGroup> GetIAgentGroupByInvestorGroupID(int InvestorGroupID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.GetIAgentGroupByInvestorGroupID(InvestorGroupID);
        }

        internal List<Business.IAgentGroup> GetIAgentGroupByAgentID(int AgentID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.GetIAgentGroupByAgentID(AgentID);
        }

        internal int DeleteIAgentGroupByAgentID(int AgentID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.DeleteIAgentGroupByAgentID(AgentID);
        }

        internal bool DeleteIAgentGroupByIAgentGroupID(int IAgentGroupID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.DeleteIAgentGroupByIAgentGroupID(IAgentGroupID);
        }

        internal bool DeleteIAgentGroupByInvestorGroupID(int InvestorGroupID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.DeleteIAgentGroupByInvestorGroupID(InvestorGroupID);
        }

        internal int AddNewIAgentGroup(int AgentID, int InvestorGroupID)
        {
            return IAgentGroup.DBWIAgentGroupInstance.AddNewIAgentGroup(AgentID, InvestorGroupID);
        }

        internal bool UpdateIAgentGroup(int AgentID, List<int> ListInvestorGroupID)
        {
            int resultDel = -1;
            resultDel = this.DeleteIAgentGroupByAgentID(AgentID);              
            int resultAdd = -1;
            if(resultDel != -1)
            {
                resultAdd = IAgentGroup.DBWIAgentGroupInstance.AddNewIAgentGroups(AgentID, ListInvestorGroupID);
            }
            if (resultDel != -1 && resultAdd != -1)
            {
                int count = Business.Market.AgentList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.AgentList[i].AgentID == AgentID)
                    {
                        Business.Market.AgentList[i].IAgentGroup = this.GetIAgentGroupByAgentID(AgentID);
                        return true;
                    }
                }
                return true;
            }
            else return false;
        }
    }
}
