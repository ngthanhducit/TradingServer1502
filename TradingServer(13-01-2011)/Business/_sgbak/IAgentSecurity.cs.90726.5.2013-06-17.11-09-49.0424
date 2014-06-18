using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class IAgentSecurity
    {
        public int IAgentSecurityID { get; set; }
        public int AgentID { get; set; }
        public int SecurityID { get; set; }
        public bool Use { get; set; }
        public double MinLots { get; set; }
        public double MaxLots { get; set; }

        #region Create Instance Class DBWIAgentSecurity
        private static DBW.DBWIAgentSecurity dbwIAgentSecurity;
        private static DBW.DBWIAgentSecurity DBWIAgentSecurityInstance
        {
            get
            {
                if (IAgentSecurity.dbwIAgentSecurity == null)
                {
                    IAgentSecurity.dbwIAgentSecurity = new DBW.DBWIAgentSecurity();
                }
                return IAgentSecurity.dbwIAgentSecurity;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<IAgentSecurity> GetAllIAgentSecurity()
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.GetAllIAgentSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentSecurityID"></param>
        /// <returns></returns>
        internal Business.IAgentSecurity GetIAgentSecurityByID(int IAgentSecurityID)
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.GetIAgentSecurityByIAgentSecurityID(IAgentSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal List<IAgentSecurity> GetIAgentSecurityBySecurityID(int SecurityID)
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.GetIAgentSecurityBySecurityID(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal List<IAgentSecurity> GetIAgentSecurityByAgentID(int AgentID)
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.GetIAgentSecurityByAgentID(AgentID);  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal int AddNewIAgentSecurity(int IAgentID, int SecurityID,bool IsUse,string MinLots, string MaxLots)
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.AddNewIAgentSecurity(IAgentID,SecurityID,IsUse,MinLots,MaxLots);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListIAgentSecurity"></param>
        /// <returns></returns>
        internal int AddListIAgentSecurity(List<Business.IAgentSecurity> ListIAgentSecurity)
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.AddNewIAgentSecurityByListIAgentSecurity(ListIAgentSecurity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentSecurityID"></param>
        /// <param name="AgentID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal bool UpdateIAgentSecurity(List<Business.IAgentSecurity> ListIAgentSecurity)
        {
            int resultDel = -1;
            int resultAdd = -1;            
            resultDel = IAgentSecurity.DBWIAgentSecurityInstance.DeleteIAgentSecurityByAgentID(ListIAgentSecurity[0].AgentID);         
            if (resultDel != -1)
            {
                resultAdd = IAgentSecurity.DBWIAgentSecurityInstance.AddNewIAgentSecurityByListIAgentSecurity(ListIAgentSecurity);
            }
            if (resultAdd != -1 && resultDel != -1)
            {
                for (int i = 0; i < Business.Market.AgentList.Count; i++)
                {
                    if (Business.Market.AgentList[i].AgentID == ListIAgentSecurity[0].AgentID)
                    {
                        Business.Market.AgentList[i].IAgentSecurity = ListIAgentSecurity;
                        return true;
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
        internal int DeleteIAgentSecurityByAgentID(int AgentID)
        {
            int resultDel = -1;
            resultDel = IAgentSecurity.DBWIAgentSecurityInstance.DeleteIAgentSecurityByAgentID(AgentID);
            if (resultDel != -1)
            {
                for (int i = 0; i < Business.Market.AgentList.Count; i++)
                {
                    if (Business.Market.AgentList[i].AgentID == AgentID)
                    {
                        for (int j = 0; j < Business.Market.AgentList[i].IAgentSecurity.Count; j++)
                        {
                            Business.Market.AgentList[i].IAgentSecurity.RemoveAt(j);
                        }
                        return resultDel;
                    }
                }
            }
            return resultDel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentSecurityID"></param>
        internal bool DeleteIAgentSecurityByID(int IAgentSecurityID)
        {
           return IAgentSecurity.DBWIAgentSecurityInstance.DeleteIAgentSecurityByIAgentSecurityID(IAgentSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        internal bool DeleteIAgentSecurityBySecurityID(int SecurityID)
        {
            return IAgentSecurity.DBWIAgentSecurityInstance.DeleteIAgentSecurityBySecurityID(SecurityID);
        }
    }
}
