using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Agent
{
    public class AgentConfig
    {
        public string AgentName { get; set; }
        public string GroupDefault { get; set; }
        public int GroupID { get; set; }
        public string DomainAccess { get; set; }
        public string IpAddress { get; set; }
        public bool IsConnect { get; set; }
        //public List<TradingServer.Agent.IAdminMaster> ListIAdminMaster { get; set; }
        //public List<TradingServer.Agent.IMasterAgentSymbol> ListIMasterAgent { get; set; }
        public List<TradingServer.Agent.IAdminAgent> ListIAdminAgent { get; set; }
        public List<TradingServer.Agent.IAgentWithAgent> ListIAgentWithAgent { get; set; }
        public List<TradingServer.Agent.IAgentInvestorSymbol> ListIAgentInvestor { get; set; }

        public List<TradingServer.Agent.IParameterCommission> ListIAdminAgentCommission { get; set; }
        public List<TradingServer.Agent.IParameterCommission> ListIAgentWithAgentCommission { get; set; }
        public AgentWCF.DefaultWCFClient clientAgent { get; set; }

        public List<string> TickQueue { get; set; }
        public List<string> NotifyQueue { get; set; }


        #region CREATE INSTANCE AGENT CONFIG
        private static TradingServer.Agent.AgentConfig _instance;
        public static TradingServer.Agent.AgentConfig Instance
        {
            get
            {
                if (TradingServer.Agent.AgentConfig._instance == null)
                    TradingServer.Agent.AgentConfig._instance = new AgentConfig();

                return TradingServer.Agent.AgentConfig._instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="investorInstance"></param>
        internal void AddNotifyToAgent(Business.AgentNotify value, Business.InvestorGroup investorGroup)
        {
            if (Business.Market.ListAgentConfig != null)
            {
                int countAgentConfig = Business.Market.ListAgentConfig.Count;
                for (int j = 0; j < countAgentConfig; j++)
                {
                    //if (Business.Market.ListAgentConfig[j].GroupDefault.ToUpper().Trim() == investorGroup.Name.ToUpper().Trim())
                    //{
                        value.InstanceAgent = Business.Market.ListAgentConfig[j];
                        Business.Market.ListNotifyQueueAgent.Add(value);
                        break;
                    //}
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="investorInstance"></param>
        internal void AddNotifyToAgent(Business.AgentNotify value)
        {
            if (Business.Market.ListAgentConfig != null)
            {
                int countAgentConfig = Business.Market.ListAgentConfig.Count;
                for (int j = 0; j < countAgentConfig; j++)
                {
                    value.InstanceAgent = Business.Market.ListAgentConfig[j];
                    Business.Market.ListNotifyQueueAgent.Add(value);
                    break;
                }
            }
        }
    }
}
