using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Model
{
    class CalculationFormular
    {
        internal static Model.CalculationFormular Instance = new CalculationFormular();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        internal double CalculationCommission(Business.OpenTrade Command)
        {
            double resultCommission = 0;

            #region Set Commission To Open Trade
            double CommissionStandar = 0;
            string CommissionType = string.Empty;
            string CommissionLot = string.Empty;

            #region Get Setting Commission
            double AgentPoints = 0;
            string AgentType = string.Empty;
            string AgentLot = string.Empty;
            //Set Commission To Open Trade
            if (Command.IGroupSecurity.IGroupSecurityConfig != null)
            {
                int countIGroupSecurityConfig = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                for (int i = 0; i < countIGroupSecurityConfig; i++)
                {
                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B14")
                    {
                        double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out CommissionStandar);
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B17")
                    {
                        CommissionType = Command.IGroupSecurity.IGroupSecurityConfig[i].StringValue;
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B18")
                    {
                        CommissionLot = Command.IGroupSecurity.IGroupSecurityConfig[i].StringValue;
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B15")
                    {
                        double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out AgentPoints);
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B19")
                    {
                        AgentType = Command.IGroupSecurity.IGroupSecurityConfig[i].StringValue;
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B20")
                    {
                        AgentLot = Command.IGroupSecurity.IGroupSecurityConfig[i].StringValue;
                    }
                }
            }
            #endregion

            #region Switch Commission Formular
            switch (CommissionType)
            {
                case "$- money":
                    {
                        resultCommission = -(Command.IGroupSecurity.CalculateCommissionByMoney(CommissionStandar, Command.Size));
                    }
                    break;

                case "pt- point":
                    {
                        resultCommission = -(Command.IGroupSecurity.CalculateCommissionByPoints(CommissionStandar, Command.Size, Command.Symbol.ContractSize, Command.OpenPrice));
                    }
                    break;

                case "%- percentage":
                    {
                        resultCommission = -(Command.IGroupSecurity.CalculateCommissionByPercentage(CommissionStandar, Command.Size, Command.Symbol.ContractSize, Command.OpenPrice));
                    }
                    break;
            }
            #endregion

            #endregion

            return resultCommission;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal double CalculationAgentCommission(Business.OpenTrade Command)
        {
            double agentPoints = 0;
            string agentType = string.Empty;
            string agentLots = string.Empty;
            double CommissionAgent = 0;
            if (Command.IGroupSecurity != null)
            {
                int countIGroupSecurit = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                for (int n = 0; n < countIGroupSecurit; n++)
                {
                    if (Command.IGroupSecurity.IGroupSecurityConfig[n].Code == "B15")
                    {
                        double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[n].NumValue, out agentPoints);
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[n].Code == "B19")
                    {
                        agentType = Command.IGroupSecurity.IGroupSecurityConfig[n].StringValue;
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[n].Code == "B20")
                    {
                        agentLots = Command.IGroupSecurity.IGroupSecurityConfig[n].StringValue;
                    }
                }
            }

            #region SWITCH AGENT TYPE
            switch (agentType)
            {
                case "$- money":
                    {
                        if (agentLots == "per lot")
                        {
                            CommissionAgent = Command.IGroupSecurity.CalculateCommissionByMoney(agentPoints, Command.Size);
                        }
                        else if (agentLots == "per deal")
                        {
                            CommissionAgent = Command.IGroupSecurity.CalculateCommissionByMoney(agentPoints, 1);
                        }
                    }
                    break;

                case "pt- point":
                    {
                        if (agentLots == "per lot")
                        {
                            CommissionAgent = Command.IGroupSecurity.CalculateCommissionByPoints(agentPoints, Command.Size, Command.Symbol.ContractSize, Command.OpenPrice);
                        }
                        else if (agentLots == "per deal")
                        {
                            CommissionAgent = Command.IGroupSecurity.CalculateCommissionByPoints(agentPoints, Command.Size, Command.Symbol.ContractSize, Command.OpenPrice);
                        }
                    }
                    break;
            }
            #endregion

            return CommissionAgent;
        }        
    }
}
