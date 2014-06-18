using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmde"></param>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        public void AgentPort(string cmd, string ipAddress, string code)
        {
            string result = string.Empty;
            string Command = string.Empty;
            string Value = string.Empty;

            if (!string.IsNullOrEmpty(cmd))
            {
                string[] subValue = cmd.Split('$');
                //TradingServer.Facade.FacadeAddNewSystemLog(1, subValue[0], "[SA]", ipAddress, code);
                switch (subValue[0])
                {
                    #region ADD ADMIN AGENT CONFIG
                    case "AddIAdminAgent":
                        {
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        Business.Market.ListAgentConfig[i].ListIAdminAgent = new List<TradingServer.Agent.IAdminAgent>();
                                        TradingServer.Agent.IAdminAgent newIAdminAgent = null;

                                        string[] subCommand = subValue[1].Split('[');
                                        if (subCommand.Length > 2)
                                        {
                                            int countCommand = subCommand.Length;
                                            for (int j = 0; j < countCommand; j++)
                                            {
                                                if (string.IsNullOrEmpty(subCommand[j]))
                                                    continue;

                                                string[] subParameter = subCommand[j].Split('{');
                                                if (subParameter.Length > 0)
                                                {
                                                    newIAdminAgent = new TradingServer.Agent.IAdminAgent();

                                                    newIAdminAgent.IAdminAgentID = int.Parse(subParameter[0]);
                                                    newIAdminAgent.AdminID = int.Parse(subParameter[1]);
                                                    newIAdminAgent.AgentID = int.Parse(subParameter[2]);
                                                    newIAdminAgent.SymbolID = int.Parse(subParameter[3]);
                                                    newIAdminAgent.DefaultPL = double.Parse(subParameter[4]);
                                                    newIAdminAgent.IsDelete = bool.Parse(subParameter[5]);
                                                    newIAdminAgent.PercentPL = double.Parse(subParameter[6]);
                                                    newIAdminAgent.IsSkipRisk = bool.Parse(subParameter[7]);

                                                    Business.Market.ListAgentConfig[i].ListIAdminAgent.Add(newIAdminAgent);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region INSERT IADMINAGENT FOR PL SYMBOL
                    case "InsertIAdminAgent":
                        {
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        //Business.Market.ListAgentConfig[i].ListIAdminAgent = new List<TradingServer.Agent.IAdminAgent>();
                                        TradingServer.Agent.IAdminAgent newIAdminAgent = null;

                                        string[] subCommand = subValue[1].Split('[');
                                        if (subCommand.Length > 2)
                                        {
                                            int countCommand = subCommand.Length;
                                            for (int j = 0; j < countCommand; j++)
                                            {
                                                if (string.IsNullOrEmpty(subCommand[j]))
                                                    continue;

                                                string[] subParameter = subCommand[j].Split('{');
                                                if (subParameter.Length > 0)
                                                {
                                                    newIAdminAgent = new TradingServer.Agent.IAdminAgent();

                                                    newIAdminAgent.IAdminAgentID = int.Parse(subParameter[0]);
                                                    newIAdminAgent.AdminID = int.Parse(subParameter[1]);
                                                    newIAdminAgent.AgentID = int.Parse(subParameter[2]);
                                                    newIAdminAgent.SymbolID = int.Parse(subParameter[3]);
                                                    newIAdminAgent.DefaultPL = double.Parse(subParameter[4]);
                                                    newIAdminAgent.IsDelete = bool.Parse(subParameter[5]);
                                                    newIAdminAgent.PercentPL = double.Parse(subParameter[6]);
                                                    newIAdminAgent.IsSkipRisk = bool.Parse(subParameter[7]);

                                                    Business.Market.ListAgentConfig[i].ListIAdminAgent.Add(newIAdminAgent);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region REMOVE IADMINAGENT PL IN SYMBOL
                    case "RemoveIAdminAgent":
                        {
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = TradingServer.Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (TradingServer.Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        string[] subCommand = subValue[1].Split('[');
                                        if (subCommand.Length > 0)
                                        {
                                            int countCommand = subCommand.Length;
                                            for (int j = 0; j < countCommand; j++)
                                            {
                                                if (string.IsNullOrEmpty(subCommand[j]))
                                                    continue;

                                                int IAdminAgentID = 0;
                                                bool isParse = int.TryParse(subCommand[j], out IAdminAgentID);

                                                if (isParse)
                                                {
                                                    if (TradingServer.Business.Market.ListAgentConfig[i].ListIAdminAgent != null)
                                                    {
                                                        int countIAdminAgent = TradingServer.Business.Market.ListAgentConfig[i].ListIAdminAgent.Count;
                                                        for (int n = 0; n < countIAdminAgent; n++)
                                                        {
                                                            if (TradingServer.Business.Market.ListAgentConfig[i].ListIAdminAgent[n].IAdminAgentID == IAdminAgentID)
                                                            {
                                                                TradingServer.Business.Market.ListAgentConfig[i].ListIAdminAgent.RemoveAt(n);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region ADD AGENT WITH AGENT
                    case "AddAgentWithAgent":
                        {
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        Business.Market.ListAgentConfig[i].ListIAgentWithAgent = new List<TradingServer.Agent.IAgentWithAgent>();

                                        TradingServer.Agent.IAgentWithAgent newIAgentWithAgent = null;

                                         string[] subParameter = subValue[1].Split('{');
                                                    if (subParameter.Length > 0)
                                                    {
                                                        newIAgentWithAgent = new TradingServer.Agent.IAgentWithAgent();
                                                        newIAgentWithAgent.IAgentWithAgentID = int.Parse(subParameter[0]);
                                                        newIAgentWithAgent.AgentID = int.Parse(subParameter[1]);
                                                        newIAgentWithAgent.AgentRefID = int.Parse(subParameter[2]);
                                                        newIAgentWithAgent.SymbolID = int.Parse(subParameter[3]);
                                                        newIAgentWithAgent.DefaultPL = double.Parse(subParameter[4]);
                                                        newIAgentWithAgent.DefaultPLParent = double.Parse(subParameter[5]);
                                                        newIAgentWithAgent.PercentPLParent = double.Parse(subParameter[6]);
                                                        newIAgentWithAgent.IsDelete = bool.Parse(subParameter[7]);
                                                        newIAgentWithAgent.PercentPL = double.Parse(subParameter[8]);
                                                        newIAgentWithAgent.IsSkipRisk = bool.Parse(subParameter[9]);
                                                        newIAgentWithAgent.PercentPLChild = double.Parse(subParameter[10]);
                                                        newIAgentWithAgent.IsSkipRiskChild = bool.Parse(subParameter[11]);

                                                        Business.Market.ListAgentConfig[i].ListIAgentWithAgent.Add(newIAgentWithAgent);
                                                    }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region REMOVE IAGENTWITHAGENT FOR PL SYMBOL
                    case "RemoveIAgentWithAgent":
                        {
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = TradingServer.Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (TradingServer.Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        string[] subCommand = subValue[1].Split('[');
                                        if (subCommand.Length > 0)
                                        {
                                            int countCommand = subCommand.Length;
                                            for (int j = 0; j < countCommand; j++)
                                            {
                                                int IAgentWithAgentID = 0;
                                                bool isParse = int.TryParse(subCommand[j], out IAgentWithAgentID);

                                                if (isParse)
                                                {
                                                    if (TradingServer.Business.Market.ListAgentConfig[i].ListIAgentWithAgent != null)
                                                    {
                                                        int countIAgentWithAgent = TradingServer.Business.Market.ListAgentConfig[i].ListIAgentWithAgent.Count;
                                                        for (int n = 0; n < countIAgentWithAgent; n++)
                                                        {
                                                            if (TradingServer.Business.Market.ListAgentConfig[i].ListIAgentWithAgent[n].IAgentWithAgentID == IAgentWithAgentID)
                                                            {
                                                                TradingServer.Business.Market.ListAgentConfig[i].ListIAgentWithAgent.RemoveAt(n);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region ADD AGENT INVESTOR CONFIG
                    case "AddAgentInvestorConfig":
                        {
                            //Check IP VALID
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        if (Business.Market.ListAgentConfig[i].ListIAgentInvestor == null)
                                            Business.Market.ListAgentConfig[i].ListIAgentInvestor = new List<TradingServer.Agent.IAgentInvestorSymbol>();

                                        TradingServer.Agent.IAgentInvestorSymbol newIAgentInvestor = null;

                                        string[] subParameter = subValue[1].Split(']');
                                        if (subParameter.Length == 2)
                                        {
                                            if (string.IsNullOrEmpty(subParameter[0]))
                                                continue;

                                            string[] subData = subParameter[0].Split('{');
                                            newIAgentInvestor = new TradingServer.Agent.IAgentInvestorSymbol();
                                            newIAgentInvestor.IAgentInvestorSymbolID = int.Parse(subData[0]);
                                            newIAgentInvestor.AgentID = int.Parse(subData[1]);
                                            newIAgentInvestor.InvestorID = int.Parse(subData[2]);
                                            newIAgentInvestor.SymbolID = int.Parse(subData[3]);
                                            newIAgentInvestor.PercentAgent = double.Parse(subData[4]);

                                            newIAgentInvestor.IAgentInvestorSymbolConfig = new List<ParameterItem>();
                                        }

                                        Business.Market.ListAgentConfig[i].ListIAgentInvestor.Add(newIAgentInvestor);

                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region UPDATE ADMIN AGENT CONFIG
                    case "UpdateIAdminAgent":
                        {
                            //IAdminMaster]IAdminMasterConfig|IAdminMasterConfig
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                int countIAgentWithAdmin = subParameter.Length;
                                for (int n = 0; n < countIAgentWithAdmin; n++)
                                {
                                    if (!string.IsNullOrEmpty(subParameter[n]))
                                    {
                                        string[] subIAdminMaster = subParameter[n].Split('{');
                                        TradingServer.Agent.IAdminAgent newIAdminAgent = new TradingServer.Agent.IAdminAgent();

                                        newIAdminAgent.IAdminAgentID = int.Parse(subIAdminMaster[0]);
                                        newIAdminAgent.AdminID = int.Parse(subIAdminMaster[1]);
                                        newIAdminAgent.AgentID = int.Parse(subIAdminMaster[2]);
                                        newIAdminAgent.SymbolID = int.Parse(subIAdminMaster[3]);
                                        newIAdminAgent.DefaultPL = double.Parse(subIAdminMaster[4]);
                                        newIAdminAgent.IsDelete = bool.Parse(subIAdminMaster[5]);
                                        newIAdminAgent.PercentPL = double.Parse(subIAdminMaster[6]);
                                        newIAdminAgent.IsSkipRisk = bool.Parse(subIAdminMaster[7]);

                                        newIAdminAgent.IAdminAgentConfig = new List<ParameterItem>();

                                        #region UPDATE IADMINMASTER
                                        bool isExits = false;
                                        if (Business.Market.ListAgentConfig != null)
                                        {
                                            int count = Business.Market.ListAgentConfig.Count;
                                            for (int i = 0; i < count; i++)
                                            {
                                                if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                {
                                                    if (Business.Market.ListAgentConfig[i].ListIAdminAgent != null)
                                                    {
                                                        int countIAdminMaster = Business.Market.ListAgentConfig[i].ListIAdminAgent.Count;
                                                        for (int j = 0; j < countIAdminMaster; j++)
                                                        {
                                                            if (Business.Market.ListAgentConfig[i].ListIAdminAgent[j].IAdminAgentID == newIAdminAgent.IAdminAgentID)
                                                            {
                                                                Business.Market.ListAgentConfig[i].ListIAdminAgent[j] = newIAdminAgent;
                                                                isExits = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (!isExits)
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAdminAgent == null)
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgent = new List<TradingServer.Agent.IAdminAgent>();

                                                        Business.Market.ListAgentConfig[i].ListIAdminAgent.Add(newIAdminAgent);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region UPDATE AGENT WITH AGENT CONFIG
                    case "UpdateIAgentWithAgent":
                        {
                            //TradingServer.Facade.FacadeAddNewSystemLog(1, subValue[1], "[UpdateIAgentWithAgent]", ipAddress, code);
                            //UpdateIAgentWithAgent$93{235{233{-1{30{20{30{False{0{False[
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                int countIAgentWithAgent = subParameter.Length;
                                for (int n = 0; n < countIAgentWithAgent; n++)
                                {
                                    if (!string.IsNullOrEmpty(subParameter[n]))
                                    {
                                        string[] subIMasterAgent = subParameter[n].Split('{');
                                        TradingServer.Agent.IAgentWithAgent newIAgentWithAgent = new TradingServer.Agent.IAgentWithAgent();

                                        newIAgentWithAgent.IAgentWithAgentID = int.Parse(subIMasterAgent[0]);
                                        newIAgentWithAgent.AgentID = int.Parse(subIMasterAgent[1]);
                                        newIAgentWithAgent.AgentRefID = int.Parse(subIMasterAgent[2]);
                                        newIAgentWithAgent.SymbolID = int.Parse(subIMasterAgent[3]);
                                        newIAgentWithAgent.DefaultPL = double.Parse(subIMasterAgent[4]);
                                        newIAgentWithAgent.DefaultPLParent = double.Parse(subIMasterAgent[5]);
                                        newIAgentWithAgent.PercentPLParent = double.Parse(subIMasterAgent[6]);
                                        newIAgentWithAgent.IsDelete = bool.Parse(subIMasterAgent[7]);
                                        newIAgentWithAgent.PercentPL = double.Parse(subIMasterAgent[8]);
                                        newIAgentWithAgent.IsSkipRisk = bool.Parse(subIMasterAgent[9]);
                                        newIAgentWithAgent.PercentPLChild = double.Parse(subIMasterAgent[10]);
                                        newIAgentWithAgent.IsSkipRiskChild = bool.Parse(subIMasterAgent[11]);

                                        newIAgentWithAgent.IAgentWithAgentConfig = new List<ParameterItem>();

                                        #region UPDATE IMASTERAGENT
                                        bool isExits = false;
                                        if (Business.Market.ListAgentConfig != null)
                                        {
                                            int count = Business.Market.ListAgentConfig.Count;
                                            for (int i = 0; i < count; i++)
                                            {
                                                if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                {
                                                    if (Business.Market.ListAgentConfig[i].ListIAgentWithAgent != null)
                                                    {
                                                        int countIMasterAgent = Business.Market.ListAgentConfig[i].ListIAgentWithAgent.Count;
                                                        for (int j = 0; j < countIMasterAgent; j++)
                                                        {
                                                            if (Business.Market.ListAgentConfig[i].ListIAgentWithAgent[j].IAgentWithAgentID == newIAgentWithAgent.IAgentWithAgentID)
                                                            {
                                                                //TradingServer.Facade.FacadeAddNewSystemLog(1, newIAgentWithAgent.IAgentWithAgentID + "-" + newIAgentWithAgent.AgentID + "-" + 
                                                                //                                            newIAgentWithAgent.AgentRefID + "-" + newIAgentWithAgent.SymbolID + "-" + 
                                                                //                                            newIAgentWithAgent.DefaultPL + "-" + newIAgentWithAgent.DefaultPLParent + "-" +
                                                                //                                            newIAgentWithAgent.PercentPLParent + "-" + newIAgentWithAgent.PercentPLParent + "-" + 
                                                                //                                            newIAgentWithAgent.IsDelete + "-" + newIAgentWithAgent.PercentPL + "-" + 
                                                                //                                            newIAgentWithAgent.IsSkipRisk + "-" + newIAgentWithAgent.PercentPLChild + "-" + 
                                                                //                                            newIAgentWithAgent.IsSkipRiskChild, "[UpdateIAgentWithAgent]", ipAddress, code);
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgent[j] = newIAgentWithAgent;
                                                                isExits = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (!isExits)
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAgentWithAgent == null)
                                                            Business.Market.ListAgentConfig[i].ListIAgentWithAgent = new List<TradingServer.Agent.IAgentWithAgent>();

                                                        Business.Market.ListAgentConfig[i].ListIAgentWithAgent.Add(newIAgentWithAgent);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region UPDATE AGENT INVESTOR CONFIG
                    case "UpdateIAgentInvestor":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                string[] subIAgentInvestor = subParameter[0].Split('{');
                                TradingServer.Agent.IAgentInvestorSymbol newIAgentInvestor = new TradingServer.Agent.IAgentInvestorSymbol();
                                newIAgentInvestor.IAgentInvestorSymbolID = int.Parse(subIAgentInvestor[0]);
                                newIAgentInvestor.AgentID = int.Parse(subIAgentInvestor[1]);
                                newIAgentInvestor.InvestorID = int.Parse(subIAgentInvestor[2]);
                                newIAgentInvestor.SymbolID = int.Parse(subIAgentInvestor[3]);
                                newIAgentInvestor.PercentAgent = double.Parse(subIAgentInvestor[4]);

                                newIAgentInvestor.IAgentInvestorSymbolConfig = new List<ParameterItem>();

                                #region MAP IAGENTINVESTOR CONFIG
                                if (!string.IsNullOrEmpty(subParameter[1]))
                                {
                                    string[] subIAgentInvestorConfig = subParameter[1].Split('|');
                                    if (subIAgentInvestorConfig.Length > 0)
                                    {
                                        int count = subIAgentInvestorConfig.Length;
                                        for (int i = 0; i < count; i++)
                                        {
                                            string[] subData = subIAgentInvestorConfig[i].Split('{');
                                            if (subData.Length > 0)
                                            {
                                                Business.ParameterItem newParameterItem = new ParameterItem();
                                                newParameterItem.ParameterItemID = int.Parse(subData[0]);
                                                newParameterItem.SecondParameterID = int.Parse(subData[1]);
                                                newParameterItem.Name = subData[2];
                                                newParameterItem.Code = subData[3];
                                                newParameterItem.BoolValue = int.Parse(subData[4]);
                                                newParameterItem.DateValue = DateTime.Parse(subData[5]);
                                                newParameterItem.NumValue = subData[6];
                                                newParameterItem.StringValue = subData[7];

                                                newIAgentInvestor.IAgentInvestorSymbolConfig.Add(newParameterItem);
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region UPDATE IAGENTINVESTOR
                                if (Business.Market.ListAgentConfig != null)
                                {
                                    int count = Business.Market.ListAgentConfig.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                        {
                                            if (Business.Market.ListAgentConfig[i].ListIAgentInvestor != null)
                                            {
                                                int countIAgentInvestor = Business.Market.ListAgentConfig[i].ListIAgentInvestor.Count;
                                                for (int j = 0; j < countIAgentInvestor; j++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].ListIAgentInvestor[j].IAgentInvestorSymbolID == newIAgentInvestor.IAgentInvestorSymbolID)
                                                    {
                                                        Business.Market.ListAgentConfig[i].ListIAgentInvestor[j] = newIAgentInvestor;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region INSERT IADMIN AGENT CONFIG
                    case "InsertIAdminAgentConfig":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(subParameter[0]))
                                {
                                    string[] subIAdminMaster = subParameter[0].Split('{');
                                    TradingServer.Agent.IAdminAgent newIAdminAgent = new TradingServer.Agent.IAdminAgent();
                                    newIAdminAgent.IAdminAgentID = int.Parse(subIAdminMaster[0]);
                                    newIAdminAgent.AdminID = int.Parse(subIAdminMaster[1]);
                                    newIAdminAgent.AgentID = int.Parse(subIAdminMaster[2]);
                                    newIAdminAgent.SymbolID = int.Parse(subIAdminMaster[3]);
                                    newIAdminAgent.DefaultPL = double.Parse(subIAdminMaster[4]);
                                    newIAdminAgent.IsDelete = bool.Parse(subIAdminMaster[5]);
                                    newIAdminAgent.PercentPL = double.Parse(subIAdminMaster[6]);
                                    newIAdminAgent.IsSkipRisk = bool.Parse(subIAdminMaster[7]);

                                    newIAdminAgent.IAdminAgentConfig = new List<ParameterItem>();

                                    if (Business.Market.ListAgentConfig != null)
                                    {
                                        int count = Business.Market.ListAgentConfig.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                            {
                                                bool flag = false;
                                                if (Business.Market.ListAgentConfig[i].ListIAdminAgent != null)
                                                {
                                                    int countIAdminMaster = Business.Market.ListAgentConfig[i].ListIAdminAgent.Count;
                                                    for (int j = 0; j < countIAdminMaster; j++)
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAdminAgent[j].AdminID == newIAdminAgent.AdminID &&
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgent[j].AgentID == newIAdminAgent.AgentID)
                                                        {
                                                            if (newIAdminAgent.IAdminAgentConfig != null)
                                                            {
                                                                int countConfig = newIAdminAgent.IAdminAgentConfig.Count;
                                                                //Business.Market.ListAgentConfig[i].ListIAdminAgent[j].IAdminAgentConfig = newIAdminAgent.IAdminAgentConfig;
                                                            }

                                                            flag = true;

                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!flag)
                                                {
                                                    Business.Market.ListAgentConfig[i].ListIAdminAgent.Add(newIAdminAgent);
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region INSERT IAGENT WITH AGENT CONFIG
                    case "InsertIAgentWithAgent":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(subParameter[0]))
                                {
                                    string[] subIMasterAgent = subParameter[0].Split('{');
                                    TradingServer.Agent.IAgentWithAgent newIAgentWithAgent = new TradingServer.Agent.IAgentWithAgent();
                                    newIAgentWithAgent.IAgentWithAgentID = int.Parse(subIMasterAgent[0]);
                                    newIAgentWithAgent.AgentID = int.Parse(subIMasterAgent[1]);
                                    newIAgentWithAgent.AgentRefID = int.Parse(subIMasterAgent[2]);
                                    newIAgentWithAgent.SymbolID = int.Parse(subIMasterAgent[3]);
                                    newIAgentWithAgent.DefaultPL = double.Parse(subIMasterAgent[4]);
                                    newIAgentWithAgent.DefaultPLParent = double.Parse(subIMasterAgent[5]);

                                    newIAgentWithAgent.IsDelete = bool.Parse(subIMasterAgent[7]);
                                    newIAgentWithAgent.PercentPL = double.Parse(subIMasterAgent[8]);
                                    newIAgentWithAgent.IsSkipRisk = bool.Parse(subIMasterAgent[9]);
                                    newIAgentWithAgent.PercentPLChild = double.Parse(subIMasterAgent[10]);
                                    newIAgentWithAgent.IsSkipRiskChild = bool.Parse(subIMasterAgent[11]);

                                    newIAgentWithAgent.IAgentWithAgentConfig = new List<ParameterItem>();

                                    if (Business.Market.ListAgentConfig != null)
                                    {
                                        int count = Business.Market.ListAgentConfig.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                            {
                                                bool flag = false;
                                                if (Business.Market.ListAgentConfig[i].ListIAgentWithAgent != null)
                                                {
                                                    int countIMasterAgent = Business.Market.ListAgentConfig[i].ListIAgentWithAgent.Count;
                                                    for (int j = 0; j < countIMasterAgent; j++)
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAgentWithAgent[j].AgentID == newIAgentWithAgent.AgentID &&
                                                            Business.Market.ListAgentConfig[i].ListIAgentWithAgent[j].AgentRefID == newIAgentWithAgent.AgentRefID &&
                                                            Business.Market.ListAgentConfig[i].ListIAgentWithAgent[j].SymbolID == newIAgentWithAgent.SymbolID)
                                                        {
                                                            if (newIAgentWithAgent.IAgentWithAgentConfig != null)
                                                            {
                                                                int countConfig = newIAgentWithAgent.IAgentWithAgentConfig.Count;
                                                                //Business.Market.ListAgentConfig[i].ListIAgentWithAgent[j].IAgentWithAgentConfig = newIAgentWithAgent.IAgentWithAgentConfig;
                                                            }

                                                            flag = true;

                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!flag)
                                                {
                                                    Business.Market.ListAgentConfig[i].ListIAgentWithAgent.Add(newIAgentWithAgent);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region INSERT IAGENT INVESTOR CONFIG
                    case "InsertAgentInvestor":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                string[] subIAgentInvestor = subParameter[0].Split('{');
                                TradingServer.Agent.IAgentInvestorSymbol newIAgentInvestor = new TradingServer.Agent.IAgentInvestorSymbol();
                                newIAgentInvestor.IAgentInvestorSymbolID = int.Parse(subIAgentInvestor[0]);
                                newIAgentInvestor.AgentID = int.Parse(subIAgentInvestor[1]);
                                newIAgentInvestor.InvestorID = int.Parse(subIAgentInvestor[2]);
                                newIAgentInvestor.SymbolID = int.Parse(subIAgentInvestor[3]);
                                newIAgentInvestor.PercentAgent = double.Parse(subIAgentInvestor[4]);

                                newIAgentInvestor.IAgentInvestorSymbolConfig = new List<ParameterItem>();

                                if (Business.Market.ListAgentConfig != null)
                                {
                                    int count = Business.Market.ListAgentConfig.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                        {
                                            bool flag = false;
                                            if (Business.Market.ListAgentConfig[i].ListIAgentInvestor != null)
                                            {
                                                int countIAgentInvestorConfig = Business.Market.ListAgentConfig[i].ListIAgentInvestor.Count;
                                                for (int j = 0; j < countIAgentInvestorConfig; j++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].ListIAgentInvestor[j] == null)
                                                    {
                                                        Business.Market.ListAgentConfig[i].ListIAgentInvestor.RemoveAt(j);
                                                        j--;
                                                        countIAgentInvestorConfig = Business.Market.ListAgentConfig[i].ListIAgentInvestor.Count;
                                                        continue;
                                                    }

                                                    if (Business.Market.ListAgentConfig[i].ListIAgentInvestor[j].AgentID == newIAgentInvestor.AgentID &&
                                                        Business.Market.ListAgentConfig[i].ListIAgentInvestor[j].InvestorID == newIAgentInvestor.InvestorID)
                                                    {  
                                                        flag = true;

                                                        break;
                                                    }
                                                }

                                                if (!flag)
                                                {
                                                    Business.Market.ListAgentConfig[i].ListIAgentInvestor.Add(newIAgentInvestor);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region INSERT LIST IAGENT INVESTOR
                    case "InsertListIAgentInvestor":
                        {
                            //InsertListIAgentInvestor$IAgentInvestorID{AgentID{InvestorID{SymbolID{PercentPL[ParameterID{SecondParameterID{Name{Code{
                            //                                            BoolValue{DateValue{NumValue{StringValue{|.....
                            string[] subRoot = subValue[1].Split('|');
                            if (subRoot.Length > 0)
                            {
                                int countRoot = subRoot.Length;
                                for (int n = 0; n < countRoot; n++)
                                {
                                    if (!string.IsNullOrEmpty(subRoot[n]))
                                    {
                                        string[] subParameter = subRoot[n].Split('[');
                                        if (subParameter.Length > 0)
                                        {
                                            string[] subIAgentInvestor = subParameter[0].Split('{');
                                            TradingServer.Agent.IAgentInvestorSymbol newIAgentInvestor = new TradingServer.Agent.IAgentInvestorSymbol();
                                            newIAgentInvestor.IAgentInvestorSymbolID = int.Parse(subIAgentInvestor[0]);
                                            newIAgentInvestor.AgentID = int.Parse(subIAgentInvestor[1]);
                                            newIAgentInvestor.InvestorID = int.Parse(subIAgentInvestor[2]);
                                            newIAgentInvestor.SymbolID = int.Parse(subIAgentInvestor[3]);
                                            newIAgentInvestor.PercentAgent = double.Parse(subIAgentInvestor[4]);

                                            newIAgentInvestor.IAgentInvestorSymbolConfig = new List<ParameterItem>();

                                            #region ADD TO LISTAGENTCONFIG
                                            if (Business.Market.ListAgentConfig != null)
                                            {
                                                int count = Business.Market.ListAgentConfig.Count;
                                                for (int i = 0; i < count; i++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                    {
                                                        bool flag = false;
                                                        if (Business.Market.ListAgentConfig[i].ListIAgentInvestor != null)
                                                        {
                                                            int countIAgentInvestorConfig = Business.Market.ListAgentConfig[i].ListIAgentInvestor.Count;
                                                            for (int j = 0; j < countIAgentInvestorConfig; j++)
                                                            {
                                                                if (Business.Market.ListAgentConfig[i].ListIAgentInvestor[j] == null)
                                                                {
                                                                    Business.Market.ListAgentConfig[i].ListIAgentInvestor.RemoveAt(j);
                                                                    j--;
                                                                    countIAgentInvestorConfig = Business.Market.ListAgentConfig[i].ListIAgentInvestor.Count;
                                                                    continue;
                                                                }

                                                                if (Business.Market.ListAgentConfig[i].ListIAgentInvestor[j].AgentID == newIAgentInvestor.AgentID &&
                                                                    Business.Market.ListAgentConfig[i].ListIAgentInvestor[j].InvestorID == newIAgentInvestor.InvestorID)
                                                                {
                                                                    flag = true;

                                                                    break;
                                                                }
                                                                //if (Business.Market.ListAgentConfig[i].ListIAgentInvestor[j].IAgentInvestorSymbolID == newIAgentInvestor.IAgentInvestorSymbolID)
                                                                //{
                                                                //    flag = true;
                                                                //    break;
                                                                //}
                                                            }

                                                            if (!flag)
                                                            {
                                                                Business.Market.ListAgentConfig[i].ListIAgentInvestor.Add(newIAgentInvestor);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region ADD ADMIN AGENT COMMISSION
                    case "AddAdminAgentCommission":
                        {
                            //TradingServer.Facade.FacadeAddNewSystemLog(1, "Begin AddAdminAgentComm" + code, "[AddAdminAgentComm]", "", "");
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                 int count = Business.Market.ListAgentConfig.Count;
                                 for (int i = 0; i < count; i++)
                                 {
                                     if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                     {
                                         string[] subRoot = subValue[1].Split('>');

                                         if (bool.Parse(subRoot[1]))
                                             Business.Market.ListAgentConfig[i].ListIAdminAgentCommission = new List<TradingServer.Agent.IParameterCommission>();
                                         
                                         TradingServer.Agent.IParameterCommission newAdminAgentCommission = null;

                                         string[] subCommand = subRoot[0].Split('[');
                                         if (subCommand.Length > 0)
                                         {
                                             int countCommand = subCommand.Length;
                                             for (int j = 0; j < countCommand; j++)
                                             {
                                                 if (!string.IsNullOrEmpty(subCommand[j]))
                                                 {
                                                     string[] subParameter = subCommand[j].Split('{');
                                                     if (subParameter.Length > 0)
                                                     {
                                                         newAdminAgentCommission = new TradingServer.Agent.IParameterCommission();

                                                         newAdminAgentCommission.IParameterID = int.Parse(subParameter[0]);
                                                         newAdminAgentCommission.FirstParameterID = int.Parse(subParameter[1]);
                                                         newAdminAgentCommission.SecondParameterID = int.Parse(subParameter[2]);
                                                         newAdminAgentCommission.GroupID = int.Parse(subParameter[3]);
                                                         newAdminAgentCommission.SymbolID = int.Parse(subParameter[4]);
                                                         newAdminAgentCommission.Comission = double.Parse(subParameter[5]);
                                                         newAdminAgentCommission.IsDelete = bool.Parse(subParameter[6]);
                                                         newAdminAgentCommission.ParentCommission = double.Parse(subParameter[7]);
                                                         newAdminAgentCommission.ChildCommission = double.Parse(subParameter[8]);
                                                         newAdminAgentCommission.ParentPipReBate = double.Parse(subParameter[9]);
                                                         newAdminAgentCommission.ChildPipReBate = double.Parse(subParameter[10]);

                                                         Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.Add(newAdminAgentCommission);
                                                     }
                                                 }
                                             }
                                         }

                                         //TradingServer.Facade.FacadeAddNewSystemLog(1, Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.Count.ToString(), "[TotalAdminAgentComm]", "", "");
                                     }
                                 }
                            }

                            //TradingServer.Facade.FacadeAddNewSystemLog(1, "End AddAdminAgentComm", "[AddAdminAgentComm]", "", "");
                        }
                        break;
                    #endregion

                    #region ADD AGENT WITH AGENT COMMISSION
                    case "AddAgentWithAgentCommission":
                        {
                            if (TradingServer.Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        string[] subRoot = subValue[1].Split('>');

                                        if (bool.Parse(subRoot[1]))
                                            Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission = new List<TradingServer.Agent.IParameterCommission>();

                                        TradingServer.Agent.IParameterCommission newAdminAgentCommission = null;

                                        string[] subCommand = subRoot[0].Split('[');
                                        if (subCommand.Length > 0)
                                        {
                                            int countCommand = subCommand.Length;
                                            for (int j = 0; j < countCommand; j++)
                                            {
                                                if (!string.IsNullOrEmpty(subCommand[j]))
                                                {
                                                    string[] subParameter = subCommand[j].Split('{');
                                                    if (subParameter.Length > 0)
                                                    {
                                                        newAdminAgentCommission = new TradingServer.Agent.IParameterCommission();

                                                        newAdminAgentCommission.IParameterID = int.Parse(subParameter[0]);
                                                        newAdminAgentCommission.FirstParameterID = int.Parse(subParameter[1]);
                                                        newAdminAgentCommission.SecondParameterID = int.Parse(subParameter[2]);
                                                        newAdminAgentCommission.GroupID = int.Parse(subParameter[3]);
                                                        newAdminAgentCommission.SymbolID = int.Parse(subParameter[4]);
                                                        newAdminAgentCommission.Comission = double.Parse(subParameter[5]);
                                                        newAdminAgentCommission.IsDelete = bool.Parse(subParameter[6]);
                                                        newAdminAgentCommission.ParentCommission = double.Parse(subParameter[7]);
                                                        newAdminAgentCommission.ChildCommission = double.Parse(subParameter[8]);
                                                        newAdminAgentCommission.ParentPipReBate = double.Parse(subParameter[9]);
                                                        newAdminAgentCommission.ChildPipReBate = double.Parse(subParameter[10]);

                                                        Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission.Add(newAdminAgentCommission);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region UPDATE ADMIN AGENT COMMISSION
                    case "UpdateIAdminAgentCommission":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                int countPara = subParameter.Length;
                                for (int n = 0; n < countPara; n++)
                                {
                                    if (!string.IsNullOrEmpty(subParameter[n]))
                                    {
                                        string[] subIAdminAgent = subParameter[n].Split('{');
                                        if (subIAdminAgent.Length > 0)
                                        {
                                            TradingServer.Agent.IParameterCommission newIAdminAgentCommission = new TradingServer.Agent.IParameterCommission();
                                            newIAdminAgentCommission.IParameterID = int.Parse(subIAdminAgent[0]);
                                            newIAdminAgentCommission.FirstParameterID = int.Parse(subIAdminAgent[1]);
                                            newIAdminAgentCommission.SecondParameterID = int.Parse(subIAdminAgent[2]);
                                            newIAdminAgentCommission.GroupID = int.Parse(subIAdminAgent[3]);
                                            newIAdminAgentCommission.SymbolID = int.Parse(subIAdminAgent[4]);
                                            newIAdminAgentCommission.Comission = double.Parse(subIAdminAgent[5]);
                                            newIAdminAgentCommission.IsDelete = bool.Parse(subIAdminAgent[6]);
                                            newIAdminAgentCommission.ParentCommission = double.Parse(subIAdminAgent[7]);
                                            newIAdminAgentCommission.ChildCommission = double.Parse(subIAdminAgent[8]);
                                            newIAdminAgentCommission.ParentPipReBate = double.Parse(subIAdminAgent[9]);
                                            newIAdminAgentCommission.ChildPipReBate = double.Parse(subIAdminAgent[10]);

                                            #region UPDATE IADMINAGENT COMMISSION
                                            if (Business.Market.ListAgentConfig != null)
                                            {
                                                int count = Business.Market.ListAgentConfig.Count;
                                                for (int i = 0; i < count; i++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission != null)
                                                        {
                                                            int countIAdminAgentComm = Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.Count;
                                                            for (int j = 0; j < countIAdminAgentComm; j++)
                                                            {
                                                                if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[j].IParameterID ==
                                                                    newIAdminAgentCommission.IParameterID)
                                                                {
                                                                    Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[j] = newIAdminAgentCommission;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region UPDATE AGENT WITH AGENT COMMISSION
                    case "UpdateIAgentWithAgentCommission":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                int countPara = subParameter.Length;
                                for (int n = 0; n < countPara; n++)
                                {
                                    if (!string.IsNullOrEmpty(subParameter[n]))
                                    {
                                        string[] subIAdminAgent = subParameter[n].Split('{');
                                        if (subIAdminAgent.Length > 0)
                                        {
                                            TradingServer.Agent.IParameterCommission newIAdminAgentCommission = new TradingServer.Agent.IParameterCommission();
                                            newIAdminAgentCommission.IParameterID = int.Parse(subIAdminAgent[0]);
                                            newIAdminAgentCommission.FirstParameterID = int.Parse(subIAdminAgent[1]);
                                            newIAdminAgentCommission.SecondParameterID = int.Parse(subIAdminAgent[2]);
                                            newIAdminAgentCommission.GroupID = int.Parse(subIAdminAgent[3]);
                                            newIAdminAgentCommission.SymbolID = int.Parse(subIAdminAgent[4]);
                                            newIAdminAgentCommission.Comission = double.Parse(subIAdminAgent[5]);
                                            newIAdminAgentCommission.IsDelete = bool.Parse(subIAdminAgent[6]);
                                            newIAdminAgentCommission.ParentCommission = double.Parse(subIAdminAgent[7]);
                                            newIAdminAgentCommission.ChildCommission = double.Parse(subIAdminAgent[8]);
                                            newIAdminAgentCommission.ParentPipReBate = double.Parse(subIAdminAgent[9]);
                                            newIAdminAgentCommission.ChildPipReBate = double.Parse(subIAdminAgent[10]);

                                            #region UPDATE IADMINAGENT COMMISSION
                                            if (Business.Market.ListAgentConfig != null)
                                            {
                                                int count = Business.Market.ListAgentConfig.Count;
                                                for (int i = 0; i < count; i++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission != null)
                                                        {
                                                            int countIAdminAgentComm = Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission.Count;
                                                            for (int j = 0; j < countIAdminAgentComm; j++)
                                                            {
                                                                if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[j].IParameterID ==
                                                                    newIAdminAgentCommission.IParameterID)
                                                                {
                                                                    Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[j] = newIAdminAgentCommission;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region INSERT ADMIN AGENT COMMISSION
                    case "InsertIAdminAgentCommission":
                        {
                            bool isUpdate = false;
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                int countPara = subParameter.Length;
                                for (int n = 0; n < countPara; n++)
                                {
                                    if (!string.IsNullOrEmpty(subParameter[n]))
                                    {
                                        string[] subIAdminAgentComm = subParameter[n].Split('{');
                                        if (subIAdminAgentComm.Length > 0)
                                        {
                                            TradingServer.Agent.IParameterCommission newIAdminAgentCommission = new TradingServer.Agent.IParameterCommission();
                                            newIAdminAgentCommission.IParameterID = int.Parse(subIAdminAgentComm[0]);
                                            newIAdminAgentCommission.FirstParameterID = int.Parse(subIAdminAgentComm[1]);
                                            newIAdminAgentCommission.SecondParameterID = int.Parse(subIAdminAgentComm[2]);
                                            newIAdminAgentCommission.GroupID = int.Parse(subIAdminAgentComm[3]);
                                            newIAdminAgentCommission.SymbolID = int.Parse(subIAdminAgentComm[4]);
                                            newIAdminAgentCommission.Comission = double.Parse(subIAdminAgentComm[5]);
                                            newIAdminAgentCommission.IsDelete = bool.Parse(subIAdminAgentComm[6]);
                                            newIAdminAgentCommission.ParentCommission = double.Parse(subIAdminAgentComm[7]);
                                            newIAdminAgentCommission.ChildCommission = double.Parse(subIAdminAgentComm[8]);
                                            newIAdminAgentCommission.ParentPipReBate = double.Parse(subIAdminAgentComm[9]);
                                            newIAdminAgentCommission.ChildPipReBate = double.Parse(subIAdminAgentComm[10]);

                                            #region UPDATE IADMINAGENT COMMISSION
                                            if (Business.Market.ListAgentConfig != null)
                                            {
                                                int count = Business.Market.ListAgentConfig.Count;
                                                for (int i = 0; i < count; i++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission != null)
                                                        {
                                                            int countIAdminAgentComm = Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.Count;
                                                            for (int j = 0; j < countIAdminAgentComm; j++)
                                                            {
                                                                if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[j].IParameterID ==
                                                                    newIAdminAgentCommission.IParameterID)
                                                                {
                                                                    Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[j] = newIAdminAgentCommission;
                                                                    isUpdate = true;
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                        if (!isUpdate)
                                                        {
                                                            if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission == null)
                                                                Business.Market.ListAgentConfig[i].ListIAdminAgentCommission = new List<TradingServer.Agent.IParameterCommission>();
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.Add(newIAdminAgentCommission);
                                                        }
                                                        

                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region INSERT AGENT WITH AGENT COMMISSION
                    case "InsertIAgentWithAgentCommission":
                        { 
                            bool isUpdate = false;
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                int countPara = subParameter.Length;
                                for (int n = 0; n < countPara; n++)
                                {
                                    if (!string.IsNullOrEmpty(subParameter[n]))
                                    {
                                        string[] subIAdminAgentComm = subParameter[n].Split('{');
                                        if (subIAdminAgentComm.Length > 1)
                                        {
                                            TradingServer.Agent.IParameterCommission newIAdminAgentCommission = new TradingServer.Agent.IParameterCommission();
                                            newIAdminAgentCommission.IParameterID = int.Parse(subIAdminAgentComm[0]);
                                            newIAdminAgentCommission.FirstParameterID = int.Parse(subIAdminAgentComm[1]);
                                            newIAdminAgentCommission.SecondParameterID = int.Parse(subIAdminAgentComm[2]);
                                            newIAdminAgentCommission.GroupID = int.Parse(subIAdminAgentComm[3]);
                                            newIAdminAgentCommission.SymbolID = int.Parse(subIAdminAgentComm[4]);
                                            newIAdminAgentCommission.Comission = double.Parse(subIAdminAgentComm[5]);
                                            newIAdminAgentCommission.IsDelete = bool.Parse(subIAdminAgentComm[6]);
                                            newIAdminAgentCommission.ParentCommission = double.Parse(subIAdminAgentComm[7]);
                                            newIAdminAgentCommission.ChildCommission = double.Parse(subIAdminAgentComm[8]);
                                            newIAdminAgentCommission.ParentPipReBate = double.Parse(subIAdminAgentComm[9]);
                                            newIAdminAgentCommission.ChildPipReBate = double.Parse(subIAdminAgentComm[10]);

                                            #region UPDATE IADMINAGENT COMMISSION
                                            if (Business.Market.ListAgentConfig != null)
                                            {
                                                int count = Business.Market.ListAgentConfig.Count;
                                                for (int i = 0; i < count; i++)
                                                {
                                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission != null)
                                                        {
                                                            int countIAdminAgentComm = Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission.Count;
                                                            for (int j = 0; j < countIAdminAgentComm; j++)
                                                            {
                                                                if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[j].IParameterID ==
                                                                    newIAdminAgentCommission.IParameterID)
                                                                {
                                                                    Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[j] = newIAdminAgentCommission;
                                                                    isUpdate = true;
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                        if (!isUpdate)
                                                        {
                                                            if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission == null)
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission = new List<TradingServer.Agent.IParameterCommission>();

                                                            Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission.Add(newIAdminAgentCommission);
                                                        }

                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region REMOVE IADMINAGENT COMMISSION
                    case "RemoveIAdminAgentCommission":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                if (Business.Market.ListAgentConfig != null)
                                {
                                    int count = Business.Market.ListAgentConfig.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                        {
                                            int countPara = subParameter.Length;
                                            for (int j = 0; j < countPara; j++)
                                            {
                                                if (string.IsNullOrEmpty(subParameter[j]))
                                                    continue;

                                                string[] subIAdminAgentComm = subParameter[j].Split('{');
                                                if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission != null)
                                                {
                                                    int countIAdminAgentComm = Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.Count;
                                                    for (int n = 0; n < countIAdminAgentComm; n++)
                                                    {
                                                        if (Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[n].IParameterID == int.Parse(subIAdminAgentComm[0]) &&
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[n].FirstParameterID == int.Parse(subIAdminAgentComm[1]) &&
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[n].SecondParameterID == int.Parse(subIAdminAgentComm[2]) &&
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[n].GroupID == int.Parse(subIAdminAgentComm[3]) &&
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgentCommission[n].SymbolID == int.Parse(subIAdminAgentComm[4]))
                                                        {
                                                            Business.Market.ListAgentConfig[i].ListIAdminAgentCommission.RemoveAt(n);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region REMOVE IAGENTWITHAGENT COMMISSION
                    case "RemoveIAgentWithAgentCommission":
                        {
                            string[] subParameter = subValue[1].Split('[');
                            if (subParameter.Length > 0)
                            {
                                if (Business.Market.ListAgentConfig != null)
                                {
                                    int count = Business.Market.ListAgentConfig.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                        {
                                            int countPara = subParameter.Length;
                                            for (int j = 0; j < countPara; j++)
                                            {
                                                if (!string.IsNullOrEmpty(subParameter[j]))
                                                {
                                                    string[] subIAgentWithAgentComm = subParameter[j].Split('{');

                                                    if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission != null)
                                                    {
                                                        int countIAdminAgentComm = Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission.Count;
                                                        for (int n = 0; n < countIAdminAgentComm; n++)
                                                        {
                                                            if (Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[n].IParameterID == int.Parse(subIAgentWithAgentComm[0]) &&
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[n].FirstParameterID == int.Parse(subIAgentWithAgentComm[1]) &&
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[n].SecondParameterID == int.Parse(subIAgentWithAgentComm[2]) &&
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[n].GroupID == int.Parse(subIAgentWithAgentComm[3]) &&
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission[n].SymbolID == int.Parse(subIAgentWithAgentComm[4]))
                                                            {
                                                                Business.Market.ListAgentConfig[i].ListIAgentWithAgentCommission.RemoveAt(n);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region SET ISCONNECT AGENT
                    case "SetIsConnectAgent":
                        {
                            bool isConnect = bool.Parse(subValue[1]);
                            if (Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.ListAgentConfig[i].AgentName.ToUpper().Trim() == code.ToUpper().Trim())
                                    {
                                        Business.Market.ListAgentConfig[i].IsConnect = isConnect;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    case "AgentStop":
                        {
                            Business.Market.IsConnectAgent = false;
                            TradingServer.Facade.FacadeAddNewSystemLog(6, "Agent Stop", "[AgentStop]", "", "");
                        }
                        break;

                    #region UPDATE CITY OF INVESTOR
                    case "UpdateCityInvestor":
                        {
                            string[] subParameter = subValue[1].Split('|');

                            if (subParameter.Length == 2)
                            {
                                string[] subInvestor = subParameter[1].Split('{');
                                int countInvestor = subInvestor.Length;
                                for (int j = 0; j < countInvestor; j++)
                                {
                                    int investorID = int.Parse(subInvestor[j]);
                                    if (investorID > 0)
                                    {
                                        if (Business.Market.InvestorList != null)
                                        {
                                            int countInvestorMarket = Business.Market.InvestorList.Count;

                                            for (int n = 0; n < countInvestorMarket; n++)
                                            {
                                                if (Business.Market.InvestorList[n].InvestorID == investorID)
                                                {
                                                    Business.Market.InvestorList[n].City = subParameter[0];

                                                    Investor.DBWInvestorInstance.UpdateInvestor(Business.Market.InvestorList[n]);

                                                    //NOTIFY TO MANAGER THEN INVESTOR ACCOUNT UPDATE
                                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[n]);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string AgentStringPort(string cmd, string ipAddress, string code)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(cmd))
            {
                string[] subValue = cmd.Split('$');
                switch (subValue[0])
                {
                    #region GET GROUP BY GROUP ID
                    case "GetGroupByGroupID":
                        {
                            int groupID = int.Parse(subValue[1]);
                            Business.InvestorGroup newGroup = new InvestorGroup();
                            if (Business.Market.InvestorGroupList != null)
                            {
                                int count = Business.Market.InvestorGroupList.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == groupID)
                                    {
                                        newGroup = Business.Market.InvestorGroupList[i];
                                        break;
                                    }
                                }
                            }

                            string msg = subValue[0] + "$" + newGroup.InvestorGroupID + "{" + newGroup.Name + "{" + newGroup.Owner + "{" + newGroup.DefautDeposite;
                            result = msg;
                        }
                        break;
                    #endregion

                    #region GET SYMBOL CONFIG BY SYMBOL ID
                    case "GetSymbolConfigBySymbolID":
                        {
                            int symbolID = -1;
                            int.TryParse(subValue[1], out symbolID);
                            string temp = this.SelectTradingConfigBySymbolIDInSymbolList(symbolID);

                            result = subValue[0] + "$" + temp;
                        }
                        break;
                    #endregion

                    #region GET SECURITY
                    case "GetSecurity":
                        {
                            string temp = string.Empty;
                            temp = this.SelectSecurityInSecurityList();
                            result = subValue[0] + "$" + temp;
                        }
                        break;
                    #endregion

                    #region GET SECURITY CONFIG BY SECURITY ID
                    case "GetSecurityConfigBySecurityID":
                        {
                            string temp = string.Empty;
                            int SecurityID = -1;
                            int.TryParse(subValue[1], out SecurityID);
                            temp = this.SelectSecurityConfigBySecurityIDInSecurityList(SecurityID);
                            result = subValue[0] + "$" + temp;
                        }
                        break;
                    #endregion

                    #region GET IGROUPSECURITY BY GROUP ID
                    case "GetIGroupSecurityByGroupID":
                        {
                            string temp = string.Empty;
                            int InvestorGroupID = -1;
                            int.TryParse(subValue[1], out InvestorGroupID);

                            List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
                            if (Business.Market.IGroupSecurityList != null)
                            {
                                int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                for (int j = 0; j < countIGroupSecurity; j++)
                                {
                                    if (Business.Market.IGroupSecurityList[j].InvestorGroupID == InvestorGroupID)
                                    {
                                        Result.Add(Business.Market.IGroupSecurityList[j]);
                                    }
                                }
                            }
                            //Result = TradingServer.Facade.FacadeGetIGroupSecurityByInvestorGroupID(InvestorGroupID);
                            if (Result != null)
                            {
                                int countIGroupSecurity = Result.Count;
                                for (int n = 0; n < countIGroupSecurity; n++)
                                {
                                    temp += Result[n].IGroupSecurityID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "," +
                                                Result[n].SecurityID.ToString() + "|";
                                }
                            }

                            result = subValue[0] + "$" + temp;
                        }
                        break;
                    #endregion

                    #region GET IGROUP SECURITY CONFIG BY IGROUP SECURITY ID
                    case "GetIGroupSecurityConfigByIGroupSecurityID":
                        {
                            string temp = string.Empty;
                            int IGroupSecurityID = -1;
                            int.TryParse(subValue[1], out IGroupSecurityID);
                            temp = this.GetIGroupSecurityConfigByIGroupSecurityIDInIGroupSecurityList(IGroupSecurityID);
                            result = subValue[0] + "}" + temp;
                        }
                        break;
                    #endregion

                    #region GET IGROUP SYMBOL BY GROUP ID
                    case "GetIGroupSymbolByGroupID":
                        {
                            string temp = string.Empty;
                            int groupID = -1;
                            int.TryParse(subValue[1], out groupID);

                            List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
                            if (Business.Market.IGroupSymbolList != null)
                            {
                                int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
                                for (int j = 0; j < countIGroupSymbol; j++)
                                {
                                    if (Business.Market.IGroupSymbolList[j].InvestorGroupID == groupID)
                                    {
                                        Result.Add(Business.Market.IGroupSymbolList[j]);
                                    }
                                }
                            }

                            //Result = TradingServer.Facade.FacadeGetIGroupSymbolBySymbolID(SymbolID);
                            if (Result != null)
                            {
                                int countIGroupSymbol = Result.Count;
                                for (int n = 0; n < countIGroupSymbol; n++)
                                {
                                    temp += Result[n].IGroupSymbolID.ToString() + "," + Result[n].SymbolID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "|";
                                }
                            }

                            result = subValue[0] + "$" + temp;
                        }
                        break;
                    #endregion

                    #region GET IGROUP SYMBOL CONFIG BY IGROUP SYMBOL ID
                    case "GetIGroupSymbolConfigByIGroupSymbolID":
                        {
                            bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                            if (checkip)
                            {
                                string temp = string.Empty;
                                int IGroupSymbolID = -1;
                                int.TryParse(subValue[1], out IGroupSymbolID);
                                temp = this.GetIGroupSymbolConfigByIGroupSymbolIDInIGroupSymbolList(IGroupSymbolID);
                                result = subValue[0] + "$" + temp;
                            }
                        }
                        break;
                    #endregion

                    #region ADD NEW INVESTOR
                    case "AddNewInvestor":
                        {
                            Business.Investor newInvestor = new Investor();

                            #region EXTRACT STRING TO OBJECT
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                int groupID = int.Parse(subParameter[0]);
                                newInvestor.AgentRefID = int.Parse(subParameter[1]);
                                newInvestor.Balance = 0;
                                newInvestor.Credit = 0;

                                if (!string.IsNullOrEmpty(subParameter[4]))
                                    newInvestor.Code = subParameter[4];
                                else
                                    newInvestor.Code = TradingServer.Model.TradingCalculate.Instance.GetNextRandomCode();

                                newInvestor.PrimaryPwd = subParameter[5];
                                newInvestor.ReadOnlyPwd = subParameter[6];
                                newInvestor.PhonePwd = subParameter[7];
                                newInvestor.IsDisable = bool.Parse(subParameter[8]);
                                newInvestor.TaxRate = double.Parse(subParameter[9]);
                                newInvestor.Leverage = double.Parse(subParameter[10]);
                                newInvestor.Address = subParameter[11];
                                newInvestor.Phone = subParameter[12];
                                newInvestor.City = subParameter[13];
                                newInvestor.Country = subParameter[14];
                                newInvestor.Email = subParameter[15];
                                newInvestor.ZipCode = subParameter[16];
                                newInvestor.RegisterDay = DateTime.Now;
                                newInvestor.InvestorComment = subParameter[18];
                                newInvestor.State = subParameter[19];
                                newInvestor.NickName = subParameter[20];
                                newInvestor.AllowChangePwd = bool.Parse(subParameter[21]);
                                newInvestor.ReadOnly = bool.Parse(subParameter[22]);
                                newInvestor.SendReport = bool.Parse(subParameter[23]);
                                newInvestor.IDPassport = subParameter[24];
                                newInvestor.InvestorStatusID = 5;
                                newInvestor.AgentID = "0";

                                if (Business.Market.InvestorGroupList != null)
                                {
                                    int countGroup = Business.Market.InvestorGroupList.Count;
                                    for (int i = 0; i < countGroup; i++)
                                    {
                                        if (Business.Market.InvestorGroupList[i].InvestorGroupID == groupID)
                                        {
                                            newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            //Check Email Add Send Mail Confirm Create Account Complete
                            bool checkEmail = Model.TradingCalculate.Instance.IsEmail(newInvestor.Email);

                            if (checkEmail)
                            {
                                bool CheckCode = false;
                                CheckCode = TradingServer.Facade.FacadeGetInvestorByCode(newInvestor.Code);
                                if (CheckCode == true)
                                {
                                    double balance = newInvestor.Balance;
                                    newInvestor.Balance = 0;

                                    int resultAddNew = TradingServer.Facade.FacadeAddNewInvestor(newInvestor);

                                    //Add Investor To Investor List
                                    if (resultAddNew > 0)
                                    {
                                        newInvestor.InvestorID = resultAddNew;
                                        int resultProfileID = TradingServer.Facade.FacadeAddInvestorProfile(newInvestor);
                                        newInvestor.InvestorProfileID = resultProfileID;
                                        if (string.IsNullOrEmpty(newInvestor.AgentID))
                                            newInvestor.AgentID = "0";

                                        Business.Market.InvestorList.Add(newInvestor);

                                        //Deposit account
                                        newInvestor.Deposit(balance, resultAddNew, "deposit");
                                    }

                                    result = subValue[0] + "$" + resultAddNew.ToString() + "," + newInvestor.Code;

                                    //SEND NOTIFY TO MANAGER
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, newInvestor);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': new account '9942881' - ngthanhduc
                                    string status = "[Failed]";

                                    if (resultAddNew > 0)
                                        status = "[Success]";

                                    string content = "'" + code + "': new account '" + newInvestor.Code + "' - " + newInvestor.NickName + " " + status;
                                    string comment = "[add new account]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion
                                }
                                else
                                {
                                    result = subValue[0] + "$Account exist";
                                    string content = "'" + code + "': new account '" + newInvestor.Code + "' - " + newInvestor.NickName + " failed(account exist)";
                                    string comment = "[add new account]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                }
                            }
                            else
                            {
                                result = subValue[0] + "$Invalid email";
                                string content = "'" + code + "': new account '" + newInvestor.Code + "' - " + newInvestor.NickName + " failed(invalid email)";
                                string comment = "[add new account]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                            }
                        }
                        break;
                    #endregion

                    #region CREDIT IN
                    case "CreditIn":
                        {
                            string[] Parameter = subValue[1].Split(',');
                            if (Parameter.Length == 3)
                            {
                                bool ResultCredit = false;
                                int InvestorID = 0;
                                double Credit = 0;

                                int.TryParse(Parameter[0], out InvestorID);
                                bool parseCredit = double.TryParse(Parameter[1], out Credit);

                                Business.Investor tempInvestor = new Investor();
                                tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);

                                ResultCredit = TradingServer.Facade.FacadeAddCredit(InvestorID, Credit, Parameter[2]);

                                result = subValue[0] + "$" + ResultCredit + "," + InvestorID + "," + Credit;

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                //'2222': account '9789300' withdrawal: 100000.00
                                string status = "[Success]";
                                if (!ResultCredit)
                                    status = "[Failed]";

                                string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Credit.ToString(), 2);
                                string content = "'Agent-" + code + "': account '" + tempInvestor.Code + "' credit in: " + tempCredit + " " + status;
                                string comment = "[credit in]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region CREDIT OUT
                    case "CreditOut":
                        {
                            string[] Parameter = subValue[1].Split(',');
                            if (Parameter.Length == 3)
                            {
                                bool ResultCredit = false;
                                int InvestorID = 0;
                                double Credit = 0;

                                int.TryParse(Parameter[0], out InvestorID);
                                bool parseCredit = double.TryParse(Parameter[1], out Credit);

                                Business.Investor tempInvestor = new Investor();
                                tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);

                                ResultCredit = TradingServer.Facade.FacadeSubCredit(InvestorID, Credit, Parameter[2]);
                                result = subValue[0] + "$" + ResultCredit + "," + InvestorID + "," + Credit;
                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                //'2222': account '9789300' withdrawal: 100000.00
                                string status = "[Success]";
                                if (!ResultCredit)
                                    status = "[Failed]";

                                string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Credit.ToString(), 2);
                                string content = "'Agent-" + code + "': account '" + tempInvestor.Code + "' credit out: " + tempCredit + " " + status;
                                string comment = "[credit out]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region DEPOSIT INVESTOR
                    case "AddDeposit":
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length == 4)
                            {
                                bool resultDeposit = false;
                                int investorID = -1;
                                double money = 0;

                                int.TryParse(subParameter[0], out investorID);
                                bool parseMoney = double.TryParse(subParameter[1], out money);
                                Business.Investor newInvestor = new Investor();
                                if (investorID > 0 && parseMoney)
                                {   
                                    newInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(investorID);

                                    resultDeposit = TradingServer.Facade.FacadeAddDeposit(investorID, money, subParameter[2]);
                                    result = subValue[0] + "$" + resultDeposit + "{" + investorID + "{" + money;
                                }
                                else
                                {
                                    result = subValue[0] + "$False{" + investorID + "{" + money;
                                }

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                //'2222': account '9789300' withdrawal: 100000.00
                                string status = "[Success]";
                                if (!resultDeposit)
                                    status = "[Failed]";

                                string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(money.ToString(), 2);
                                string content = "'Agent-" + subParameter[3] + "': account '" + newInvestor.Code + "' deposit: " + tempCredit + " " + status;
                                string comment = "[deposit]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region WITHDRAWAL INVESTOR
                    case "Withdrawal":
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length == 4)
                            {
                                bool resultDeposit = false;
                                int investorID = -1;
                                double money = 0;

                                int.TryParse(subParameter[0], out investorID);
                                bool parseMoney = double.TryParse(subParameter[1], out money);
                                Business.Investor newInvestor = new Investor();
                                if (investorID > 0 && parseMoney)
                                {
                                    newInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(investorID);

                                    resultDeposit = TradingServer.Facade.FacadeWithRawals(investorID, money, subParameter[2]);
                                    result = subValue[0] + "$" + resultDeposit + "{" + investorID + "{" + money;
                                }
                                else
                                {
                                    result = subValue[0] + "$False{" + investorID + "{" + money;
                                }

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                //'2222': account '9789300' withdrawal: 100000.00
                                string status = "[Success]";
                                if (!resultDeposit)
                                    status = "[Failed]";

                                string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(money.ToString(), 2);
                                string content = "'Agent-" + subParameter[3] + "': account '" + newInvestor.Code + "' withdrawal: " + tempCredit + " " + status;
                                string comment = "[withdrawal]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region UPDATE INVESTOR
                    case "UpdateInvestor":
                        {
                            //add GroupID
                            //UpdateInvestor$Password{ReadPassword{PhonePassword{Address{Phone{City{Country{Email{ZipCode{State{NickName{IsReadOnly{IsDisable{
                            //AllowChangePassword{SendReport{InvestorComment{Leverage{TaxRate{IDPassport
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                Business.Investor newInvestor = new Investor();
                                newInvestor.PrimaryPwd = subParameter[0];
                                newInvestor.ReadOnlyPwd = subParameter[1];
                                newInvestor.PhonePwd = subParameter[2];
                                newInvestor.Address = subParameter[3];
                                newInvestor.Phone = subParameter[4];
                                newInvestor.City = subParameter[5];
                                newInvestor.Country = subParameter[6];
                                newInvestor.Email = subParameter[7];
                                newInvestor.ZipCode = subParameter[8];
                                newInvestor.State = subParameter[9];
                                newInvestor.NickName = subParameter[10];
                                newInvestor.ReadOnly = bool.Parse(subParameter[11]);
                                newInvestor.IsDisable = bool.Parse(subParameter[12]);
                                newInvestor.AllowChangePwd = bool.Parse(subParameter[13]);
                                newInvestor.SendReport = bool.Parse(subParameter[14]);
                                //newInvestor.ReadOnly = bool.Parse(subParameter[15]);
                                newInvestor.InvestorComment = subParameter[15];
                                newInvestor.Leverage = double.Parse(subParameter[16]);
                                newInvestor.TaxRate = double.Parse(subParameter[17]);
                                newInvestor.IDPassport = subParameter[18];
                                newInvestor.InvestorID = int.Parse(subParameter[19]);
                                newInvestor.InvestorStatusID = 5;
                                newInvestor.AgentID = "0";

                                int groupID = int.Parse(subParameter[20]);
                                if (Business.Market.InvestorGroupList != null)
                                {
                                    int count = Business.Market.InvestorGroupList.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.InvestorGroupList[i].InvestorGroupID == groupID)
                                        {
                                            newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                                            break;
                                        }
                                    }
                                }

                                if (Business.Market.InvestorList != null)
                                {
                                    int count = Business.Market.InvestorList.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.InvestorList[i].InvestorID == newInvestor.InvestorID)
                                        {
                                            newInvestor.InvestorProfileID = Business.Market.InvestorList[i].InvestorProfileID;
                                            break;
                                        }
                                    }
                                }

                                newInvestor.Code = subParameter[21];

                                bool resultUpdate = TradingServer.Facade.FacadeUpdateInvestor(newInvestor, ipAddress, code);
                                bool resultUpdateProfile = TradingServer.Facade.FacadeUpdateInvestorProfile(newInvestor, ipAddress, code);

                                if (newInvestor.PrimaryPwd != "") resultUpdate = TradingServer.Facade.FacadeUpdatePasswordByCode(newInvestor.Code, newInvestor.PrimaryPwd);
                                if (newInvestor.ReadOnlyPwd != "") resultUpdate = TradingServer.Facade.FacadeUpdateReadPwdByCode(newInvestor.Code, newInvestor.ReadOnlyPwd);
                                if (newInvestor.PhonePwd != "") resultUpdate = TradingServer.Facade.FacadeUpdatePhonePwdByCode(newInvestor.Code, newInvestor.PhonePwd);

                                result = subValue[0] + "$" + resultUpdate.ToString();
                            }
                        }
                        break;
                    #endregion

                    #region GET INVESTOR ACCOUNT BY INVESTOR ID
                    case "GetInvestorAccountByInvestorID":
                        {
                            int investorID = int.Parse(subValue[1]);

                        }
                        break;
                    #endregion

                    #region GET SYMBOL BY SYMBOL ID
                    case "GetSymbolBySymbolID":
                        {
                            int symbolID = int.Parse(subValue[1]);
                            string temp = this.SelectSymbolByIDInSymbolList(symbolID);

                            result = subValue[0] + "$" + temp;
                        }
                        break;
                    #endregion

                    #region GET INVESTOR BY INVESTOR ID
                    case "GetInvestorByInvestorID":
                        {
                            int investorID = int.Parse(subValue[1]);
                            Business.Investor newInvestor = TradingServer.ClientFacade.FacadeGetInvestorByInvestor(investorID);

                            //GET CLOSE PL IN DAY, COMMISSION CLOSE IN DAY, SWAP CLOSE IN DAY
                            DateTime timeStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
                            DateTime timeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                            double plInDay = 0;
                            double commInDay = 0;
                            double swapInDay = 0;

                            List<Business.OpenTrade> listCommand = TradingServer.Facade.FacadeGetCommandHistoryWithTime(investorID, timeStart, timeEnd);
                            if (listCommand != null)
                            {
                                int count = listCommand.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (listCommand[i].Type.ID == 1 || listCommand[i].Type.ID == 2)
                                    {
                                        plInDay += listCommand[i].Profit;
                                        commInDay += listCommand[i].Commission;
                                        swapInDay += listCommand[i].Swap;
                                    }
                                }
                            }

                            result = subValue[0] + "$" + newInvestor.Address + "{" + newInvestor.AgentRefID + "{" + newInvestor.AllowChangePwd + "{" + newInvestor.Balance + "{" +
                                        newInvestor.City + "{" + newInvestor.Code + "{" + newInvestor.Comment + "{" + newInvestor.Country + "{" + newInvestor.Credit + "{" +
                                        newInvestor.Email + "{" + newInvestor.Equity + "{" + newInvestor.FirstName + "{" + newInvestor.FreeMargin + "{" + newInvestor.FreezeMargin + "{" +
                                        newInvestor.IDPassport + "{" + newInvestor.InvestorComment + "{" + newInvestor.IsDisable + "{" + newInvestor.IsReadOnly + "{" +
                                        newInvestor.Leverage + "{" + newInvestor.Margin + "{" + newInvestor.MarginLevel + "{" + newInvestor.NickName + "{" + newInvestor.Phone + "{" +
                                        newInvestor.PhonePwd + "{" + newInvestor.PrimaryPwd + "{" + newInvestor.Profit + "{" + newInvestor.ReadOnlyPwd + "{" + newInvestor.RegisterDay + "{" +
                                        newInvestor.SendReport + "{" + newInvestor.State + "{" + newInvestor.ZipCode + "{" + plInDay + "{" + commInDay + "{" + swapInDay;
                        }
                        break;
                    #endregion

                    #region ADD INVESTOR TO AGENT
                    case "AddInvestorToAgent":
                        {
                            //AddInvestorToAgent$InvestorCode{AgentID{ListGroupOfAgent,ListGroupOfAgent,....
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length >2)
                            {
                                List<int> listGroupAgent = new List<int>();
                                string[] subGroupAgent = subParameter[2].Split(',');

                                if (subGroupAgent.Length > 0)
                                {
                                    int countGroupAgent = subGroupAgent.Length;
                                    for (int i = 0; i < countGroupAgent; i++)
                                    {
                                        listGroupAgent.Add(int.Parse(subGroupAgent[i]));
                                    }
                                }

                                int agentID = int.Parse(subParameter[1]);
                                if (Business.Market.InvestorList != null)
                                {
                                    int count = Business.Market.InvestorList.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.InvestorList[i].Code.Trim() == subParameter[0].Trim())
                                        {
                                            if (Business.Market.InvestorList[i].AgentRefID < 0)
                                            {
                                                bool isExits = false;
                                                if (listGroupAgent != null)
                                                {   
                                                    int countGroupAgent = listGroupAgent.Count;
                                                    for (int j = 0; j < countGroupAgent; j++)
                                                    {
                                                        if (listGroupAgent[j] == Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID)
                                                        {
                                                            isExits = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!isExits)
                                                    result = subValue[0] + "$False}GroupInvalid";
                                                else
                                                {
                                                    //UPDATE AGENTREFID TO DATABASE AND RAM
                                                    bool updateAgentRef = TradingServer.Facade.FacadeUpdateAgentRefID(Business.Market.InvestorList[i].InvestorID, agentID);
                                                    if (updateAgentRef)
                                                    {
                                                        Business.Market.InvestorList[i].AgentRefID = agentID;

                                                        string msg = Business.Market.InvestorList[i].InvestorID + "{" +
                                                        Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID + "{" +
                                                        agentID + Business.Market.InvestorList[i].Balance + "{" + Business.Market.InvestorList[i].Credit + "{" +
                                                        Business.Market.InvestorList[i].Code + "{" + Business.Market.InvestorList[i].PrimaryPwd + "{" +
                                                        Business.Market.InvestorList[i].ReadOnlyPwd + "{" + Business.Market.InvestorList[i].PhonePwd + "{" +
                                                        Business.Market.InvestorList[i].IsDisable + "{" + Business.Market.InvestorList[i].TaxRate + "{" +
                                                        Business.Market.InvestorList[i].Leverage + "{" + Business.Market.InvestorList[i].AllowChangePwd + "{" +
                                                        Business.Market.InvestorList[i].ReadOnly + "{" + Business.Market.InvestorList[i].Address + "{" +
                                                        Business.Market.InvestorList[i].Phone + "{" + Business.Market.InvestorList[i].City + "{" +
                                                        Business.Market.InvestorList[i].Country + "{" + Business.Market.InvestorList[i].Email + "{" +
                                                        Business.Market.InvestorList[i].ZipCode + "{" + Business.Market.InvestorList[i].RegisterDay + "{" +
                                                        Business.Market.InvestorList[i].Comment + "{" + Business.Market.InvestorList[i].State + "{" +
                                                        Business.Market.InvestorList[i].NickName + "{" + Business.Market.InvestorList[i].IDPassport;

                                                        result = subValue[0] + "$True}" + msg;
                                                    }
                                                    else
                                                        result = subValue[0] + "$False}SomeError";
                                                }
                                            }
                                            else
                                            {
                                                result = subValue[0] + "$False}AgentExits";
                                            }

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET END OF DAY INFO INVESTOR ACCOUNT
                    case "GetEndOfDayAccount":
                        {
                            int investorID = int.Parse(subValue[1]);
                            if (investorID > 0)
                            {
                                //if (Business.Market.InvestorList != null)
                                //{
                                //    int count = Business.Market.InvestorList.Count;
                                //    for (int i = 0; i < count; i++)
                                //    {
                                //        if (Business.Market.InvestorList[i].InvestorID == investorID)
                                //        {
                                //            result = subValue[0] + "$" + Business.Market.InvestorList[i].FloatingPL + "{" + 
                                //                Business.Market.InvestorList[i].MonthVolume + "{" + Business.Market.InvestorList[i].TimeSave;

                                //            break;
                                //        }
                                //    }
                                //}
                                if (Business.Market.ListEODAgent != null)
                                {
                                    int count = Business.Market.ListEODAgent.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.ListEODAgent[i].InvestorID == investorID)
                                        {
                                            result = subValue[0] + "$" + Business.Market.ListEODAgent[i].FloatingPL + "{" +
                                                Business.Market.ListEODAgent[i].MonthVolume + "{" + DateTime.Now;

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET MONTH SIZE
                    case "GetMonthSize":
                        {
                            double size = 0;
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                int investorID = int.Parse(subParameter[0]);
                                DateTime timeStart = DateTime.Parse(subParameter[1]);
                                DateTime timeEnd = DateTime.Parse(subParameter[2]);

                                List<Business.OpenTrade> listHistory = TradingServer.Facade.FacadeGetCommandHistoryWithTime(investorID, timeStart, timeEnd);
                                if (listHistory != null)
                                {
                                    int count = listHistory.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(listHistory[i].Type.ID);
                                        if (!isPending && listHistory[i].Type.ID != 13 && listHistory[i].Type.ID != 14 &&
                                            listHistory[i].Type.ID != 15 && listHistory[i].Type.ID != 16 && listHistory[i].Type.ID != 21)
                                            size += listHistory[i].Size;
                                    }
                                }
                            }

                            result = subValue[0] + "$" + size;
                        }
                        break;
                    #endregion

                    #region GET FLOATING PL
                    case "GetFloatingPL":
                        {
                            double floatingPL = 0;
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                int investorID = int.Parse(subParameter[0]);
                                DateTime timeStart = DateTime.Parse(subParameter[1]);
                                DateTime timeEnd = DateTime.Parse(subParameter[2]);

                                List<Business.OpenTrade> listHistory = TradingServer.Facade.FacadeGetCommandHistoryWithTime(investorID, timeStart, timeEnd);
                                if (listHistory != null)
                                {
                                    int count = listHistory.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(listHistory[i].Type.ID);
                                        if (!isPending && listHistory[i].Type.ID != 13 && listHistory[i].Type.ID != 14 &&
                                            listHistory[i].Type.ID != 15 && listHistory[i].Type.ID != 16 && listHistory[i].Type.ID != 21)
                                            floatingPL += listHistory[i].Profit;
                                    }
                                }
                            }

                            result = subValue[0] + "$" + floatingPL;
                        }
                        break;
                    #endregion

                    #region UPDATE CITY OF INVESTOR
                    case "UpdateCityInvestor":
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                int count = subParameter.Length;
                                for (int i = 0; i < count; i++)
                                {
                                    bool isComplete = false;
                                    string[] subData = subParameter[i].Split('|');
                                    if (subData.Length == 2)
                                    {
                                        int investorID = int.Parse(subData[0]);
                                        if (Business.Market.InvestorList != null)
                                        {
                                            int countInvestor = Business.Market.InvestorList.Count;
                                            for (int j = 0; j < countInvestor; j++)
                                            {
                                                if (Business.Market.InvestorList[i].InvestorID == investorID)
                                                {
                                                    Business.Market.InvestorList[i].City = subData[1];
                                                    isComplete = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (!isComplete)
                                    {
                                        result = subValue[0] + "$" + false;
                                        return result;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET ALL TICK
                    case "GetAllTick":
                        {
                            StringBuilder tempResult = new StringBuilder();
                            List<Business.Tick> listTick = TradingServer.Facade.FacadeGetTickOnline();
                            if (listTick != null)
                            {
                                int count = listTick.Count;
                                tempResult.Append(subValue[0]);
                                tempResult.Append("$");

                                for (int i = 0; i < count; i++)
                                {
                                    tempResult.Append(listTick[i].Bid);
                                    tempResult.Append("{");
                                    tempResult.Append(listTick[i].Ask);
                                    tempResult.Append("{");
                                    tempResult.Append(listTick[i].HighInDay);
                                    tempResult.Append("{");
                                    tempResult.Append(listTick[i].LowInDay);
                                    tempResult.Append("{");
                                    tempResult.Append(listTick[i].Status);
                                    tempResult.Append("{");
                                    tempResult.Append(listTick[i].SymbolName);
                                    tempResult.Append("{");
                                    tempResult.Append(listTick[i].TickTime);
                                    tempResult.Append("[");
                                }

                                result = tempResult.ToString();

                                result.Remove(result.Length - 1);
                            }
                        }
                        break;
                    #endregion
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<string> AgentListStringPort(string cmd, string ipAddress, string code)
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(cmd))
            {
                string[] subValue = cmd.Split('$');
                switch (subValue[0])
                {
                    #region GET ALL GROUP
                    case "GetAllGroup":
                        {   
                            if (Business.Market.InvestorGroupList != null)
                            {
                                int count = Business.Market.InvestorGroupList.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    string msg = string.Empty;

                                    msg += subValue[0] + "$" + Business.Market.InvestorGroupList[i].InvestorGroupID + "{" +
                                                                Business.Market.InvestorGroupList[i].Name + "{" +
                                                                Business.Market.InvestorGroupList[i].Owner + "{" +
                                                                Business.Market.InvestorGroupList[i].DefautDeposite;

                                    result.Add(msg);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET GROUPCONFIG BY GROUPID
                    case "GetGroupConfigByGroupID":
                        {
                            int InvestorGroupID = -1;
                            int.TryParse(subValue[1], out InvestorGroupID);

                            if (Market.InvestorGroupList != null)
                            {
                                int countInvestorGroup = Market.InvestorGroupList.Count;
                                for (int n = 0; n < countInvestorGroup; n++)
                                {
                                    if (Market.InvestorGroupList[n].InvestorGroupID == InvestorGroupID)
                                    {
                                        if (Market.InvestorGroupList[n].ParameterItems != null)
                                        {
                                            int countParameterItem = Market.InvestorGroupList[n].ParameterItems.Count;
                                            for (int m = 0; m < countParameterItem; m++)
                                            {
                                                string temp = string.Empty;
                                                temp = Market.InvestorGroupList[n].ParameterItems[m].ParameterItemID.ToString() + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].SecondParameterID.ToString() + "," + "-1" + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].Name + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].Code + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].BoolValue.ToString() + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].StringValue + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].NumValue + "," +
                                                        Market.InvestorGroupList[n].ParameterItems[m].DateValue.ToString();

                                                result.Add(subValue[0] + "$" + temp);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET ALL SYMBOL
                    case "GetSymbol":
                        {
                            result = this.SelectSymbolInSymbolList();
                        }
                        break;
                    #endregion

                    #region GET OPEN TRADE BY LIST INVESTORID
                    case "GetOpenTradeByListInvestorID":
                        {
                            List<int> listInvestorID = new List<int>();
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter != null && subParameter.Length > 0)
                            {
                                int countCode = subParameter.Length;
                                for (int j = 0; j < countCode; j++)
                                {
                                    listInvestorID.Add(int.Parse(subParameter[j]));
                                }
                            }

                            List<Business.OpenTrade> resultOpenTrade = TradingServer.Facade.FacadeGetOpenTradeByListInvestorID(listInvestorID);
                            if (resultOpenTrade != null)
                            {
                                int countResult = resultOpenTrade.Count;
                                for (int j = 0; j < countResult; j++)
                                {
                                    string Message = subValue[0] + "$" + resultOpenTrade[j].ClientCode + "," + resultOpenTrade[j].ClosePrice + "," + resultOpenTrade[j].CloseTime + "," +
                                                    resultOpenTrade[j].CommandCode + "," + resultOpenTrade[j].Commission + "," + resultOpenTrade[j].ExpTime + "," + resultOpenTrade[j].ID + "," +
                                                    resultOpenTrade[j].Investor.InvestorID + "," + resultOpenTrade[j].IsClose + "," + resultOpenTrade[j].IsHedged + "," + resultOpenTrade[j].Margin + "," +
                                                    resultOpenTrade[j].MaxDev + "," + resultOpenTrade[j].OpenPrice + "," + resultOpenTrade[j].OpenTime + "," + resultOpenTrade[j].Profit + "," +
                                                    resultOpenTrade[j].Size + "," + resultOpenTrade[j].StopLoss + "," + resultOpenTrade[j].Swap + "," + resultOpenTrade[j].Symbol.Name + "," +
                                                    resultOpenTrade[j].TakeProfit + "," +
                                                    resultOpenTrade[j].Taxes + "," + resultOpenTrade[j].Type.Name + "," + resultOpenTrade[j].Type.ID + "," + resultOpenTrade[j].Symbol.ContractSize + "," +
                                                    resultOpenTrade[j].SpreaDifferenceInOpenTrade + "," + resultOpenTrade[j].Symbol.Currency + "," + resultOpenTrade[j].Comment + "," +
                                                    resultOpenTrade[j].AgentCommission + "," +
                                                    resultOpenTrade[j].Investor.Code + "," + resultOpenTrade[j].AgentRefConfig;

                                    result.Add(Message);
                                }
                            }
                            else
                            {
                                string message = subValue[0] + "$";
                                result.Add(message);
                            }
                        }
                        break;
                    #endregion

                    #region GET COMMAND HISTORY AGENT ONLY CLOSE COMMAND
                    case "GetHistoryAgent":
                        {
                            //GetHistoryAgent$timeStart{TimeEnd}investorID{InvestorID{InvestorID
                            string[] subParameter = subValue[1].Split('}');
                            string[] subTime = subParameter[0].Split('{');
                            DateTime timeStart, timeEnd;

                            bool isParse = DateTime.TryParse(subTime[0], out timeStart);

                            bool isParseDate = DateTime.TryParse(subTime[1], out timeEnd);

                            List<TradingServer.Business.OpenTrade> listCommand = new List<OpenTrade>();
                            if (isParse && isParseDate)
                            {
                                string[] subListInvestor = subParameter[1].Split('{');

                                if (subListInvestor != null && subListInvestor.Length > 0)
                                {
                                    int countListInvestor = subListInvestor.Length;
                                    for (int i = 0; i < countListInvestor; i++)
                                    {
                                        if (string.IsNullOrEmpty(subListInvestor[i]))
                                            continue;

                                        int investorID = int.Parse(subListInvestor[i]);

                                        if (investorID > 0)
                                        {
                                            List<TradingServer.Business.OpenTrade> listResult = TradingServer.Facade.FacadeGetCommandHistoryWithTime(investorID, timeStart, timeEnd);

                                            if (listResult != null && listResult.Count > 0)
                                            {
                                                int countCommand = listResult.Count;
                                                for (int j = 0; j < countCommand; j++)
                                                {
                                                    if (listResult[j].Type.ID == 1 || listResult[j].Type.ID == 2 ||
                                                        listResult[j].Type.ID == 3 || listResult[j].Type.ID == 4 ||
                                                        listResult[j].Type.ID == 7 || listResult[j].Type.ID == 8 ||
                                                        listResult[j].Type.ID == 9 || listResult[j].Type.ID == 10)
                                                        listCommand.Add(listResult[j]);
                                                }
                                            }
                                        }
                                    }
                                }

                                if (listCommand != null && listCommand.Count > 0)
                                {
                                    int count = listCommand.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        string Message = subValue[0] + "$" + listCommand[i].ClientCode + "," + listCommand[i].ClosePrice + "," +
                                                    listCommand[i].CloseTime + "," + listCommand[i].CommandCode + "," + listCommand[i].Commission + "," +
                                                    listCommand[i].ExpTime + "," + listCommand[i].ID + "," + listCommand[i].Investor.InvestorID + "," +
                                                    listCommand[i].IsClose + "," + listCommand[i].IsHedged + "," + listCommand[i].Margin + "," +
                                                    listCommand[i].MaxDev + "," + listCommand[i].OpenPrice + "," + listCommand[i].OpenTime + "," +
                                                    listCommand[i].Profit + "," + listCommand[i].Size + "," + listCommand[i].StopLoss + "," +
                                                    listCommand[i].Swap + "," + listCommand[i].Symbol.Name + "," +
                                                    listCommand[i].TakeProfit + "," + listCommand[i].Taxes + "," + listCommand[i].Type.Name + "," +
                                                    listCommand[i].Type.ID + "," + listCommand[i].Symbol.ContractSize + "," +
                                                    listCommand[i].SpreaDifferenceInOpenTrade + "," + listCommand[i].Symbol.Currency + "," +
                                                    listCommand[i].Comment + "," + listCommand[i].AgentCommission + "," +
                                                    listCommand[i].Investor.Code + "," + listCommand[i].AgentRefConfig;

                                        result.Add(Message);
                                    }
                                }
                            }
                            else
                            {
                                result.Add(subValue[0] + "$Invalid Time");
                            }
                        }
                        break;
                    #endregion

                    #region GET PAYMONT MONEY AGENT(DEPOSIT, WITHDRAW, CREDIT IN, CREDIT OUT)
                    case "GetMoneyAccount":
                        {
                            //"GetMoneyAccount$timeStart{timeEnd}InvestorID{InvestorID{InvestorID{......"
                            string[] subParameter = subValue[1].Split('}');
                            string[] subTime = subParameter[0].Split('{');
                            DateTime timeStart, timeEnd;

                            bool isParse = DateTime.TryParse(subTime[0], out timeStart);

                            bool isParseDate = DateTime.TryParse(subTime[1], out timeEnd);

                            List<TradingServer.Business.OpenTrade> listCommand = new List<OpenTrade>();
                            if (isParse && isParseDate)
                            {
                                string[] subListInvestor = subParameter[1].Split('{');
                                if (subListInvestor != null && subListInvestor.Length > 0)
                                {
                                    int countInvestor = subListInvestor.Length;
                                    for (int i = 0; i < countInvestor; i++)
                                    {
                                        int investorID = int.Parse(subListInvestor[i]);
                                        if (investorID > 0)
                                        {
                                            List<TradingServer.Business.OpenTrade> listResult = TradingServer.Facade.FacadeGetCommandHistoryWithTime(investorID, timeStart, timeEnd);

                                            if (listResult != null && listResult.Count > 0)
                                            {
                                                int countCommand = listResult.Count;
                                                for (int j = 0; j < countCommand; j++)
                                                {
                                                    if (listResult[j].Type.ID == 13 || listResult[j].Type.ID == 14 ||
                                                        listResult[j].Type.ID == 15 || listResult[j].Type.ID == 16)
                                                        listCommand.Add(listResult[j]);
                                                }
                                            }
                                        }
                                    }
                                }

                                if (listCommand != null && listCommand.Count > 0)
                                {
                                    int count = listCommand.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        string Message = subValue[0] + "$" + listCommand[i].ClientCode + "," + listCommand[i].ClosePrice + "," +
                                                    listCommand[i].CloseTime + "," + listCommand[i].CommandCode + "," + listCommand[i].Commission + "," +
                                                    listCommand[i].ExpTime + "," + listCommand[i].ID + "," + listCommand[i].Investor.InvestorID + "," +
                                                    listCommand[i].IsClose + "," + listCommand[i].IsHedged + "," + listCommand[i].Margin + "," +
                                                    listCommand[i].MaxDev + "," + listCommand[i].OpenPrice + "," + listCommand[i].OpenTime + "," +
                                                    listCommand[i].Profit + "," + listCommand[i].Size + "," + listCommand[i].StopLoss + "," +
                                                    listCommand[i].Swap + "," + listCommand[i].Symbol.Name + "," +
                                                    listCommand[i].TakeProfit + "," + listCommand[i].Taxes + "," + listCommand[i].Type.Name + "," +
                                                    listCommand[i].Type.ID + "," + listCommand[i].Symbol.ContractSize + "," +
                                                    listCommand[i].SpreaDifferenceInOpenTrade + "," + listCommand[i].Symbol.Currency + "," +
                                                    listCommand[i].Comment + "," + listCommand[i].AgentCommission + "," +
                                                    listCommand[i].Investor.Code + "," + listCommand[i].AgentRefConfig;

                                        result.Add(Message);
                                    }
                                }
                            }
                            else
                            {
                                result.Add(subValue[0] + "$Invalid Time");
                            }
                        }
                        break;
                    #endregion

                    #region GET REPORT FROM AGENT SYSTEM
                    case "GetReportAgent":
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                #region GET PARAMETER
                                //GetReportAgent$Start{End{AgentID{startTime{endTime{investorID{investorID{investorID.......
                                int start = 0;
                                int end = 0;
                                string managerName = string.Empty;
                                DateTime startTime;
                                DateTime endTime;
                                List<int> InvestorList = new List<int>();
                                Dictionary<int, string> listInvestor = new Dictionary<int, string>();
                                List<Business.OpenTrade> Result = new List<OpenTrade>();

                                int.TryParse(subParameter[0], out start);
                                int.TryParse(subParameter[1], out end);
                                //int.TryParse(subParameter[2], out managerID);
                                managerName = subParameter[2];
                                DateTime.TryParse(subParameter[3], out startTime);
                                DateTime.TryParse(subParameter[4], out endTime);

                                //set new time start,end
                                startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 00, 00, 00);
                                endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
                                #endregion

                                #region GET LIST INVESTOR
                                for (int j = 5; j < subParameter.Length; j++)
                                {
                                    int investorID = 0;
                                    int.TryParse(subParameter[j], out investorID);
                                    InvestorList.Add(investorID);

                                    if (Business.Market.InvestorList != null)
                                    {
                                        int countInvestor = Business.Market.InvestorList.Count;
                                        for (int n = 0; n < countInvestor; n++)
                                        {
                                            if (Business.Market.InvestorList[n].InvestorID == investorID)
                                            {
                                                listInvestor.Add(investorID, Business.Market.InvestorList[n].Code);
                                                break;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (Business.Market.ListAgentReport != null)
                                {
                                    int countAgent = Business.Market.ListAgentReport.Count;
                                    bool isExits = false;
                                    for (int j = 0; j < countAgent; j++)
                                    {
                                        if (Business.Market.ListAgentReport[j].AgentName.ToUpper().Trim() == managerName.ToUpper().Trim())
                                        {
                                            if (start == 0)
                                            {
                                                Business.Market.ListAgentReport[j].ListHistoryReport = new List<OpenTrade>();

                                                #region GET REPORT IN DATABASE AND ADD TO LIST HISTORY REPORT
                                                Result = TradingServer.Facade.FacadegetCommandHistoryWithInvestorList(InvestorList, 0, startTime, endTime);

                                                if (Result != null)
                                                {
                                                    int countResult = Result.Count;

                                                    if (Business.Market.ListAgentReport[j].ListHistoryReport == null)
                                                        Business.Market.ListAgentReport[j].ListHistoryReport = new List<OpenTrade>();

                                                    for (int n = 0; n < countResult; n++)
                                                    {
                                                        Business.Market.ListAgentReport[j].ListHistoryReport.Add(Result[n]);
                                                    }
                                                }
                                                #endregion

                                                #region GET LAST ACCOUNT
                                                List<Business.LastBalance> listLastAccount = 
                                                    TradingServer.Facade.FacadeGetLastBalanceByTimeInvestor(listInvestor, startTime, endTime);
                                                if (listLastAccount != null)
                                                {
                                                    int countLastAccount = listLastAccount.Count;
                                                    for (int n = 0; n < countLastAccount; n++)
                                                    {
                                                        if (listLastAccount[n].LastAccountID > 0)
                                                        {
                                                            Business.OpenTrade newOpenTrade = new OpenTrade();
                                                            newOpenTrade.AgentCommission = listLastAccount[n].Balance;
                                                            newOpenTrade.AskServer = listLastAccount[n].ClosePL;
                                                            newOpenTrade.BidServer = listLastAccount[n].Credit;
                                                            newOpenTrade.ClosePrice = listLastAccount[n].CreditAccount;
                                                            newOpenTrade.Commission = listLastAccount[n].CreditOut;
                                                            newOpenTrade.FreezeMargin = listLastAccount[n].Deposit;
                                                            newOpenTrade.Margin = listLastAccount[n].FloatingPL;
                                                            newOpenTrade.MaxDev = listLastAccount[n].FreeMargin;
                                                            newOpenTrade.NumberUpdate = listLastAccount[n].InvestorID;
                                                            newOpenTrade.OpenPrice = listLastAccount[n].LastEquity;
                                                            newOpenTrade.OpenTime = listLastAccount[n].LogDate;
                                                            newOpenTrade.Profit = listLastAccount[n].LastMargin;
                                                            newOpenTrade.Size = listLastAccount[n].PLBalance;
                                                            newOpenTrade.SpreaDifferenceInOpenTrade = listLastAccount[n].Withdrawal;
                                                            newOpenTrade.Comment = listLastAccount[n].LoginCode;
                                                            newOpenTrade.Type = new TradeType();
                                                            newOpenTrade.Type.ID = 21;
                                                            newOpenTrade.Type.Name = "LastBalance";
                                                            newOpenTrade.Investor = new Investor();

                                                            newOpenTrade.StopLoss = listLastAccount[n].EndMargin;
                                                            newOpenTrade.TakeProfit = listLastAccount[n].EndFreeMargin;
                                                            newOpenTrade.CloseTime = listLastAccount[n].EndLogDate;

                                                            Business.Market.ListAgentReport[j].ListHistoryReport.Add(newOpenTrade);
                                                        }
                                                        else
                                                        {
                                                            if (Business.Market.InvestorList != null)
                                                            {
                                                                int countInvestor = Business.Market.InvestorList.Count;
                                                                for (int m = 0; m < countInvestor; m++)
                                                                {
                                                                    if (Business.Market.InvestorList[m].Code == listLastAccount[n].LoginCode)
                                                                    {
                                                                        Business.OpenTrade newOpenTrade = new OpenTrade();
                                                                        newOpenTrade.AgentCommission = listLastAccount[n].Balance;
                                                                        newOpenTrade.AskServer = listLastAccount[n].ClosePL;
                                                                        newOpenTrade.BidServer = listLastAccount[n].Credit;
                                                                        newOpenTrade.ClosePrice = listLastAccount[n].CreditAccount;
                                                                        newOpenTrade.Commission = listLastAccount[n].CreditOut;
                                                                        newOpenTrade.FreezeMargin = listLastAccount[n].Deposit;
                                                                        newOpenTrade.Margin = listLastAccount[n].FloatingPL;
                                                                        newOpenTrade.MaxDev = listLastAccount[n].FreeMargin;
                                                                        newOpenTrade.NumberUpdate = listLastAccount[n].InvestorID;
                                                                        newOpenTrade.OpenPrice = listLastAccount[n].LastEquity;
                                                                        newOpenTrade.OpenTime = listLastAccount[n].LogDate;
                                                                        newOpenTrade.Profit = listLastAccount[n].LastMargin;
                                                                        newOpenTrade.Size = listLastAccount[n].PLBalance;
                                                                        newOpenTrade.SpreaDifferenceInOpenTrade = listLastAccount[n].Withdrawal;
                                                                        newOpenTrade.Comment = listLastAccount[n].LoginCode;
                                                                        newOpenTrade.Type = new TradeType();
                                                                        newOpenTrade.Type.ID = 21;
                                                                        newOpenTrade.Type.Name = "LastBalance";
                                                                        newOpenTrade.Investor = new Investor();

                                                                        newOpenTrade.StopLoss = Business.Market.InvestorList[m].Margin;
                                                                        newOpenTrade.TakeProfit = Business.Market.InvestorList[m].FreeMargin;
                                                                        newOpenTrade.CloseTime = DateTime.Now;

                                                                        Business.Market.ListAgentReport[j].ListHistoryReport.Add(newOpenTrade);

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }

                                            if (Business.Market.ListAgentReport[j].ListHistoryReport != null)
                                            {
                                                int rowNumber = end - start;
                                                if (Business.Market.ListAgentReport[j].ListHistoryReport.Count < rowNumber)
                                                    rowNumber = Business.Market.ListAgentReport[j].ListHistoryReport.Count;

                                                #region FOR GET ORDER WITH START ,END
                                                for (int n = 0; n < rowNumber; n++)
                                                {
                                                    string symbolName = string.Empty;
                                                    if (Business.Market.ListAgentReport[j].ListHistoryReport[0].Type == null ||
                                                        Business.Market.ListAgentReport[j].ListHistoryReport[0].Investor == null)
                                                        continue;

                                                    if (Business.Market.ListAgentReport[j].ListHistoryReport[0].Type.ID == 21)
                                                    {
                                                        string messageLastBalance = "LastBalance$" +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].NumberUpdate + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Size + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].AskServer + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].FreezeMargin + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Margin + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].BidServer + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenPrice + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Profit + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].MaxDev + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenTime + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Commission + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].SpreaDifferenceInOpenTrade + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].ClosePrice + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Comment + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].StopLoss + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].TakeProfit + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].CloseTime;

                                                        result.Add(messageLastBalance);

                                                        Business.Market.ListAgentReport[j].ListHistoryReport.RemoveAt(0);
                                                    }
                                                    else
                                                    {
                                                        if (Business.Market.ListAgentReport[j].ListHistoryReport[0].Symbol != null)
                                                            symbolName = Business.Market.ListAgentReport[j].ListHistoryReport[0].Symbol.Name;

                                                        string message = subValue[0] + "$" +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].ClientCode + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].ClosePrice + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].CloseTime + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].CommandCode + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Commission + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].ExpTime + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].ID + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Investor.InvestorID + "," +
                                                            true + "," + false + "," + "0,0" + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenPrice + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenTime + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Profit + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Size + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].StopLoss + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Swap + "," + symbolName + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].TakeProfit + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Taxes + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Type.Name + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Type.ID + ",-1,-1" + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].AgentCommission + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Investor.AgentID + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Comment + "," +
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].AgentRefConfig;

                                                        result.Add(message);

                                                        Business.Market.ListAgentReport[j].ListHistoryReport.RemoveAt(0);
                                                    }
                                                }
                                                #endregion
                                            }

                                            isExits = true;
                                            break;
                                        }
                                    }

                                    if (!isExits)
                                    {
                                        #region GET REPORT IN DATABASE AND ADD TO LIST HISTORY REPORT
                                        Result = TradingServer.Facade.FacadegetCommandHistoryWithInvestorList(InvestorList, 0, startTime, endTime);
                                        Business.AgentReport newAgentReport = new AgentReport();
                                        newAgentReport.AgentName = managerName;
                                        if (Result != null)
                                        {
                                            int countResult = Result.Count;

                                            if (newAgentReport.ListHistoryReport == null)
                                                newAgentReport.ListHistoryReport = new List<OpenTrade>();

                                            for (int n = 0; n < countResult; n++)
                                            {
                                                newAgentReport.ListHistoryReport.Add(Result[n]);
                                            }
                                        }
                                        #endregion

                                        #region GET LAST ACCOUNT
                                        List<Business.LastBalance> listLastAccount = 
                                            TradingServer.Facade.FacadeGetLastBalanceByTimeInvestor(listInvestor, startTime, endTime);
                                        if (listLastAccount != null)
                                        {
                                            int countLastAccount = listLastAccount.Count;
                                            for (int n = 0; n < countLastAccount; n++)
                                            {
                                                if (listLastAccount[n].LastAccountID > 0)
                                                {
                                                    Business.OpenTrade newOpenTrade = new OpenTrade();
                                                    newOpenTrade.AgentCommission = listLastAccount[n].Balance;
                                                    newOpenTrade.AskServer = listLastAccount[n].ClosePL;
                                                    newOpenTrade.BidServer = listLastAccount[n].Credit;
                                                    newOpenTrade.ClosePrice = listLastAccount[n].CreditAccount;
                                                    newOpenTrade.Commission = listLastAccount[n].CreditOut;
                                                    newOpenTrade.FreezeMargin = listLastAccount[n].Deposit;
                                                    newOpenTrade.Margin = listLastAccount[n].FloatingPL;
                                                    newOpenTrade.MaxDev = listLastAccount[n].FreeMargin;
                                                    newOpenTrade.NumberUpdate = listLastAccount[n].InvestorID;
                                                    newOpenTrade.OpenPrice = listLastAccount[n].LastEquity;
                                                    newOpenTrade.OpenTime = listLastAccount[n].LogDate;
                                                    newOpenTrade.Profit = listLastAccount[n].LastMargin;
                                                    newOpenTrade.Size = listLastAccount[n].PLBalance;
                                                    newOpenTrade.SpreaDifferenceInOpenTrade = listLastAccount[n].Withdrawal;
                                                    newOpenTrade.Comment = listLastAccount[n].LoginCode;
                                                    newOpenTrade.Type = new TradeType();
                                                    newOpenTrade.Type.ID = 21;
                                                    newOpenTrade.Type.Name = "LastBalance";
                                                    newOpenTrade.Investor = new Investor();

                                                    newOpenTrade.StopLoss = listLastAccount[n].EndMargin;
                                                    newOpenTrade.TakeProfit = listLastAccount[n].EndFreeMargin;
                                                    newOpenTrade.CloseTime = listLastAccount[n].EndLogDate;

                                                    newAgentReport.ListHistoryReport.Add(newOpenTrade);
                                                }
                                                else
                                                {
                                                    if (Business.Market.InvestorList != null)
                                                    {
                                                        int countInvestor = Business.Market.InvestorList.Count;
                                                        for (int m = 0; m < countInvestor; m++)
                                                        {
                                                            if (Business.Market.InvestorList[m].Code == listLastAccount[n].LoginCode)
                                                            {
                                                                Business.OpenTrade newOpenTrade = new OpenTrade();
                                                                newOpenTrade.AgentCommission = listLastAccount[n].Balance;
                                                                newOpenTrade.AskServer = listLastAccount[n].ClosePL;
                                                                newOpenTrade.BidServer = listLastAccount[n].Credit;
                                                                newOpenTrade.ClosePrice = listLastAccount[n].CreditAccount;
                                                                newOpenTrade.Commission = listLastAccount[n].CreditOut;
                                                                newOpenTrade.FreezeMargin = listLastAccount[n].Deposit;
                                                                newOpenTrade.Margin = listLastAccount[n].FloatingPL;
                                                                newOpenTrade.MaxDev = listLastAccount[n].FreeMargin;
                                                                newOpenTrade.NumberUpdate = listLastAccount[n].InvestorID;
                                                                newOpenTrade.OpenPrice = listLastAccount[n].LastEquity;
                                                                newOpenTrade.OpenTime = listLastAccount[n].LogDate;
                                                                newOpenTrade.Profit = listLastAccount[n].LastMargin;
                                                                newOpenTrade.Size = listLastAccount[n].PLBalance;
                                                                newOpenTrade.SpreaDifferenceInOpenTrade = listLastAccount[n].Withdrawal;
                                                                newOpenTrade.Comment = listLastAccount[n].LoginCode;
                                                                newOpenTrade.Type = new TradeType();
                                                                newOpenTrade.Type.ID = 21;
                                                                newOpenTrade.Type.Name = "LastBalance";
                                                                newOpenTrade.Investor = new Investor();

                                                                newOpenTrade.StopLoss = Business.Market.InvestorList[m].Margin;
                                                                newOpenTrade.TakeProfit = Business.Market.InvestorList[m].FreeMargin;
                                                                newOpenTrade.CloseTime = DateTime.Now;

                                                                newAgentReport.ListHistoryReport.Add(newOpenTrade);

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        Business.Market.ListAgentReport.Add(newAgentReport);

                                        int countAgentReport = Business.Market.ListAgentReport.Count;
                                        for (int j = 0; j < countAgentReport; j++)
                                        {
                                            if (Business.Market.ListAgentReport[j].AgentName == managerName)
                                            {
                                                if (Business.Market.ListAgentReport[j].ListHistoryReport != null)
                                                {
                                                    int rowNumber = end - start;
                                                    if (Business.Market.ListAgentReport[j].ListHistoryReport.Count < rowNumber)
                                                        rowNumber = Business.Market.ListAgentReport[j].ListHistoryReport.Count;

                                                    #region FOR GET ORDER WITH START ,END
                                                    for (int n = 0; n < rowNumber; n++)
                                                    {
                                                        string symbolName = string.Empty;
                                                        if (Business.Market.ListAgentReport[j].ListHistoryReport[0].Type == null ||
                                                            Business.Market.ListAgentReport[j].ListHistoryReport[0].Investor == null)
                                                            continue;

                                                        if (Business.Market.ListAgentReport[j].ListHistoryReport[0].Type.ID == 21)
                                                        {
                                                            string messageLastBalance = "LastBalance$" +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].NumberUpdate + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Size + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].AskServer + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].FreezeMargin + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Margin + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].BidServer + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenPrice + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Profit + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].MaxDev + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenTime + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Commission + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].SpreaDifferenceInOpenTrade + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].ClosePrice + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Comment + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].StopLoss + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].TakeProfit + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].CloseTime;

                                                            result.Add(messageLastBalance);

                                                            Business.Market.ListAgentReport[j].ListHistoryReport.RemoveAt(0);
                                                        }
                                                        else
                                                        {
                                                            if (Business.Market.ListAgentReport[j].ListHistoryReport[0].Symbol != null)
                                                                symbolName = Business.Market.ListAgentReport[j].ListHistoryReport[0].Symbol.Name;

                                                            string message = subValue[0] + "$" +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].ClientCode + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].ClosePrice + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].CloseTime + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].CommandCode + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Commission + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].ExpTime + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].ID + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Investor.InvestorID + "," +
                                                                true + "," + false + "," + "0,0" + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenPrice + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].OpenTime + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Profit + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Size + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].StopLoss + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Swap + "," + symbolName + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].TakeProfit + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Taxes + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Type.Name + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Type.ID + ",-1,-1" + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].AgentCommission + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Investor.AgentID + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].Comment + "," +
                                                                Business.Market.ListAgentReport[j].ListHistoryReport[0].AgentRefConfig;

                                                            result.Add(message);

                                                            Business.Market.ListAgentReport[j].ListHistoryReport.RemoveAt(0);
                                                        }
                                                    }
                                                    #endregion
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET BACKUP COMMAND
                    case "GetBKCommand":
                        {
                            List<TradingServer.Business.AgentNotify> listNotify = TradingServer.Facade.FacadeGetBKCommand();
                            if (listNotify != null && listNotify.Count > 0)
                            {
                                int count = listNotify.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    result.Add(listNotify[i].NotifyMessage);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GET ORDER DATA WITH TIME
                    //GET ORDER IN DATABASE WITH TABLE COMMAND HISTORY 
                    case "GetOrderDataWithTime":
                        {
                            string[] subParameter = subValue[1].Split('{');
                            if (subParameter.Length > 0)
                            {
                                #region GET PARAMETER
                                //GetOrderDataWithTime$Start{End{AgentName{Year|Mont|Day{endTime{investorID{investorID{investorID.......
                                int start = 0;
                                int end = 0;
                                string managerName = string.Empty;
                                DateTime startTime;
                                DateTime endTime;
                                List<int> InvestorList = new List<int>();
                                Dictionary<int, string> listInvestor = new Dictionary<int, string>();
                                List<Business.OpenTrade> Result = new List<OpenTrade>();

                                int.TryParse(subParameter[0], out start);
                                int.TryParse(subParameter[1], out end);
                                managerName = subParameter[2];

                                string[] subTimeStart = subParameter[3].Split('|');
                                string[] subTimeEnd = subParameter[4].Split('|');

                                //set new time start,end
                                startTime = new DateTime(int.Parse(subTimeStart[0]), int.Parse(subTimeStart[1]), int.Parse(subTimeStart[2]), 00, 00, 00);
                                endTime = new DateTime(int.Parse(subTimeEnd[0]), int.Parse(subTimeEnd[1]), int.Parse(subTimeEnd[2]), 23, 59, 59);

                                ////build new time end if time end > current time
                                DateTime timeCurrent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                                TimeSpan span = timeCurrent - endTime;
                                if (span.TotalSeconds > 0)
                                    endTime = timeCurrent;
                                #endregion

                                #region GET LIST INVESTOR
                                for (int j = 5; j < subParameter.Length; j++)
                                {
                                    int investorID = 0;
                                    int.TryParse(subParameter[j], out investorID);
                                    InvestorList.Add(investorID);

                                    if (Business.Market.InvestorList != null)
                                    {
                                        int countInvestor = Business.Market.InvestorList.Count;
                                        for (int n = 0; n < countInvestor; n++)
                                        {
                                            if (Business.Market.InvestorList[n].InvestorID == investorID)
                                            {
                                                listInvestor.Add(investorID, Business.Market.InvestorList[n].Code);
                                                break;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (Business.Market.ListHistoryAgent != null)
                                {
                                    int countAgent = Business.Market.ListHistoryAgent.Count;
                                    bool isExits = false;
                                    for (int j = 0; j < countAgent; j++)
                                    {
                                        #region FIND AND GET MANAGER
                                        if (Business.Market.ListHistoryAgent[j].AgentName.ToUpper().Trim() == managerName.ToUpper().Trim())
                                        {
                                            if (start == 0)
                                            {
                                                Business.Market.ListHistoryAgent[j].ListHistoryReport = new List<OpenTrade>();

                                                #region GET REPORT IN DATABASE AND ADD TO LIST HISTORY REPORT
                                                Result = TradingServer.Facade.FacadegetCommandHistoryWithInvestorList(InvestorList, 0, startTime, endTime);

                                                if (Result != null)
                                                {
                                                    int countResult = Result.Count;

                                                    if (Business.Market.ListHistoryAgent[j].ListHistoryReport == null)
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport = new List<OpenTrade>();

                                                    for (int n = 0; n < countResult; n++)
                                                    {
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport.Add(Result[n]);
                                                    }
                                                }
                                                #endregion
                                            }

                                            if (Business.Market.ListHistoryAgent[j].ListHistoryReport != null)
                                            {
                                                int rowNumber = end - start;
                                                if (Business.Market.ListHistoryAgent[j].ListHistoryReport.Count < rowNumber)
                                                    rowNumber = Business.Market.ListHistoryAgent[j].ListHistoryReport.Count;

                                                #region FOR GET ORDER WITH START ,END
                                                for (int n = 0; n < rowNumber; n++)
                                                {
                                                    string symbolName = string.Empty;
                                                    if (Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Type == null ||
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Investor == null)
                                                        continue;

                                                    if (Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Symbol != null)
                                                        symbolName = Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Symbol.Name;

                                                    string message = subValue[0] + "$" +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ClientCode + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ClosePrice + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].CloseTime + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].CommandCode + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Commission + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ExpTime + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ID + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Investor.InvestorID + "," +
                                                        true + "," + false + "," + "0.0" + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].OpenPrice + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].OpenTime + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Profit + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Size + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].StopLoss + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Swap + "," + symbolName + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].TakeProfit + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Taxes + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Type.Name + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Type.ID + ",-1,-1" + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].AgentCommission + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Investor.AgentID + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Comment + "," +
                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport[0].AgentRefConfig;

                                                    result.Add(message);

                                                    Business.Market.ListHistoryAgent[j].ListHistoryReport.RemoveAt(0);
                                                }
                                                #endregion
                                            }

                                            isExits = true;
                                            break;
                                        }
                                        #endregion
                                    }

                                    #region ISEXISTS AGENT MANAGER
                                    if (!isExits)
                                    {
                                        #region GET REPORT IN DATABASE AND ADD TO LIST HISTORY REPORT
                                        Result = TradingServer.Facade.FacadegetCommandHistoryWithInvestorList(InvestorList, 0, startTime, endTime);
                                        Business.AgentReport newAgentReport = new AgentReport();
                                        newAgentReport.AgentName = managerName;
                                        if (Result != null)
                                        {
                                            int countResult = Result.Count;

                                            if (newAgentReport.ListHistoryReport == null)
                                                newAgentReport.ListHistoryReport = new List<OpenTrade>();

                                            for (int n = 0; n < countResult; n++)
                                            {
                                                newAgentReport.ListHistoryReport.Add(Result[n]);
                                            }
                                        }
                                        #endregion

                                        Business.Market.ListHistoryAgent.Add(newAgentReport);

                                        int countAgentReport = Business.Market.ListHistoryAgent.Count;
                                        for (int j = 0; j < countAgentReport; j++)
                                        {
                                            if (Business.Market.ListHistoryAgent[j].AgentName == managerName)
                                            {
                                                if (Business.Market.ListHistoryAgent[j].ListHistoryReport != null)
                                                {
                                                    int rowNumber = end - start;
                                                    if (Business.Market.ListHistoryAgent[j].ListHistoryReport.Count < rowNumber)
                                                        rowNumber = Business.Market.ListHistoryAgent[j].ListHistoryReport.Count;

                                                    #region FOR GET ORDER WITH START ,END
                                                    for (int n = 0; n < rowNumber; n++)
                                                    {
                                                        string symbolName = string.Empty;
                                                        if (Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Type == null ||
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Investor == null)
                                                            continue;

                                                        if (Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Symbol != null)
                                                            symbolName = Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Symbol.Name;

                                                        string message = subValue[0] + "$" +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ClientCode + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ClosePrice + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].CloseTime + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].CommandCode + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Commission + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ExpTime + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].ID + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Investor.InvestorID + "," +
                                                            true + "," + false + "," + "0.0" + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].OpenPrice + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].OpenTime + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Profit + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Size + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].StopLoss + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Swap + "," + symbolName + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].TakeProfit + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Taxes + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Type.Name + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Type.ID + ",-1,-1" + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].AgentCommission + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Investor.AgentID + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].Comment + "," +
                                                            Business.Market.ListHistoryAgent[j].ListHistoryReport[0].AgentRefConfig;

                                                        result.Add(message);

                                                        Business.Market.ListHistoryAgent[j].ListHistoryReport.RemoveAt(0);
                                                    }
                                                    #endregion
                                                }

                                                break;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        break;
                    #endregion

                    case "GetLastAccountInvestor":
                        {
                            //TradingServer.Facade.FacadeGetLastBalanceByTimeListInvestor
                            //GetLastAccountInvestor$TimeStart{TimeEnd|InvestorID{InvestorID{InvestorID{....
                            string[] subParameter = subValue[1].Split('|');
                            if (subParameter.Length == 2)
                            {
                                List<int> listInvestor = new List<int>();
                                List<Business.OpenTrade> listOpenTrade = new List<OpenTrade>();

                                string[] subDateTime = subParameter[0].Split('{');                               
                                DateTime _tempTimeStart = DateTime.Parse(subDateTime[0]);
                                DateTime _tempTimeEnd = DateTime.Parse(subDateTime[1]);
                                DateTime timeStart = new DateTime(_tempTimeStart.Year, _tempTimeStart.Month, _tempTimeStart.Day, 00, 00, 00);
                                DateTime timeEnd = new DateTime(_tempTimeEnd.Year, _tempTimeEnd.Month, _tempTimeEnd.Day, 23, 59, 59);

                                string[] subInvestor = subParameter[1].Split('{');
                                for (int j = 0; j < subInvestor.Length; j++)
                                {
                                    listInvestor.Add(int.Parse(subInvestor[j]));
                                }

                                List<Business.LastBalance> listLastAccount = TradingServer.Facade.FacadeGetLastBalanceByTimeListInvestor(listInvestor, timeStart, timeEnd);

                                if (listLastAccount != null)
                                {
                                    int count = listLastAccount.Count;

                                    int countLastAccount = listLastAccount.Count;
                                    for (int n = 0; n < countLastAccount; n++)
                                    {
                                        //GetLastAccountInvestor$Balance,ClosePL,Credit,CreditAccount,CreditOut,Deposit,FloatingPL,FreeMargin,
                                        //InvestorID,LastEquity,LogDate,LastMargin,PLBalance,Withdrawal,LoginCode
                                        string message = "GetLastAccountInvestor$" + listLastAccount[n].Balance + "," +
                                                                   listLastAccount[n].ClosePL + "," +
                                                                   listLastAccount[n].Credit + "," +
                                                                   listLastAccount[n].CreditAccount + "," +
                                                                   listLastAccount[n].CreditOut + "," +
                                                                   listLastAccount[n].Deposit + "," +
                                                                   listLastAccount[n].FloatingPL + "," +
                                                                   listLastAccount[n].FreeMargin + "," +
                                                                   listLastAccount[n].InvestorID + "," +
                                                                   listLastAccount[n].LastEquity + "," +
                                                                   listLastAccount[n].LogDate + "," +
                                                                   listLastAccount[n].LastMargin + "," +
                                                                   listLastAccount[n].PLBalance + "," +
                                                                   listLastAccount[n].Withdrawal + "," +
                                                                   listLastAccount[n].LoginCode;

                                        result.Add(message);
                                    }
                                }
                            }
                            else
                            {
                                //INVALID PARAMETER
                                result.Add(subValue[0] + "IVP01923278");
                            }
                        }
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iAgentWithAgents"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal string GetAgentWithAgentRefConfig(List<TradingServer.Agent.IAgentWithAgent> iAgentWithAgents, 
                                                    List<TradingServer.Agent.IAdminAgent> iAdminAgents, int agentID)
        {
            string result = string.Empty;

            if (iAgentWithAgents != null)
            {
                int count = iAgentWithAgents.Count;
                bool isExits = false;
                int agentAdminID = -1;

                for (int i = 0; i < count; i++)
                {
                    if (iAgentWithAgents[i].AgentID == agentID)
                    {
                        result = iAgentWithAgents[i].IAgentWithAgentID + "{" + iAgentWithAgents[i].AgentID + "{" + iAgentWithAgents[i].AgentRefID + "{" +
                            iAgentWithAgents[i].SymbolID + "{" + iAgentWithAgents[i].DefaultPL + "{" + iAgentWithAgents[i].DefaultPLParent + "{" +
                            iAgentWithAgents[i].PercentPLParent + "{" + iAgentWithAgents[i].IsDelete + "{" + iAgentWithAgents[i].PercentPL + "{" +
                            iAgentWithAgents[i].IsSkipRisk + "{" + iAgentWithAgents[i].PercentPLChild + "{" + iAgentWithAgents[i].IsSkipRiskChild + "]";

                        result += this.GetAgentWithAgentRefConfig(iAgentWithAgents, iAdminAgents, iAgentWithAgents[i].AgentRefID);
                        isExits = true;

                        agentAdminID = iAgentWithAgents[i].AgentID;

                        break;
                    }
                }

                if (!isExits)
                {
                    if (iAdminAgents != null)
                    {
                        int countAdminAgent = iAdminAgents.Count;
                        for (int j = 0; j < countAdminAgent; j++)
                        {
                            if (iAdminAgents[j].AgentID == agentID)
                            {
                                result += "|" + iAdminAgents[j].IAdminAgentID + "{" + iAdminAgents[j].AdminID + "{" + iAdminAgents[j].AgentID + "{" +
                                    iAdminAgents[j].SymbolID + "{" + iAdminAgents[j].DefaultPL + "{" + iAdminAgents[j].IsDelete + "{" +
                                    iAdminAgents[j].PercentPL + "{" + iAdminAgents[j].IsSkipRisk;

                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iAgentWithAgents"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal string GetAgentWithAgentRefConfigSymbolByID(List<TradingServer.Agent.IAgentWithAgent> iAgentWithAgents,
                                                    List<TradingServer.Agent.IAdminAgent> iAdminAgents, int agentID, int symbolID)
        {
            string result = string.Empty;

            if (iAgentWithAgents != null)
            {
                int count = iAgentWithAgents.Count;
                bool isExits = false;
                int agentAdminID = -1;
                for (int i = 0; i < count; i++)
                {
                    if (iAgentWithAgents[i].SymbolID == symbolID)
                    {
                        if (iAgentWithAgents[i].AgentID == agentID)
                        {
                            result = iAgentWithAgents[i].IAgentWithAgentID + "{" + iAgentWithAgents[i].AgentID + "{" + iAgentWithAgents[i].AgentRefID + "{" +
                            iAgentWithAgents[i].SymbolID + "{" + iAgentWithAgents[i].DefaultPL + "{" + iAgentWithAgents[i].DefaultPLParent + "{" +
                            iAgentWithAgents[i].PercentPLParent + "{" + iAgentWithAgents[i].IsDelete + "{" + iAgentWithAgents[i].PercentPL + "{" +
                            iAgentWithAgents[i].IsSkipRisk + "{" + iAgentWithAgents[i].PercentPLChild + "{" + iAgentWithAgents[i].IsSkipRiskChild + "]";

                            result += this.GetAgentWithAgentRefConfig(iAgentWithAgents, iAdminAgents, iAgentWithAgents[i].AgentRefID);
                            isExits = true;

                            agentAdminID = iAgentWithAgents[i].AgentID;

                            isExits = true;

                            break;
                        }
                    }
                }

                if (!isExits)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (iAgentWithAgents[i].AgentID == agentID)
                        {
                            result = iAgentWithAgents[i].IAgentWithAgentID + "{" + iAgentWithAgents[i].AgentID + "{" + iAgentWithAgents[i].AgentRefID + "{" +
                                iAgentWithAgents[i].SymbolID + "{" + iAgentWithAgents[i].DefaultPL + "{" + iAgentWithAgents[i].DefaultPLParent + "{" +
                                iAgentWithAgents[i].PercentPLParent + "{" + iAgentWithAgents[i].IsDelete + "{" + iAgentWithAgents[i].PercentPL + "{" +
                                iAgentWithAgents[i].IsSkipRisk + "{" + iAgentWithAgents[i].PercentPLChild + "{" + iAgentWithAgents[i].IsSkipRiskChild + "]";

                            result += this.GetAgentWithAgentRefConfig(iAgentWithAgents, iAdminAgents, iAgentWithAgents[i].AgentRefID);
                            isExits = true;

                            agentAdminID = iAgentWithAgents[i].AgentID;

                            break;
                        }
                    }
                }

                if (!isExits)
                {
                    if (iAdminAgents != null)
                    {
                        int countAdminAgent = iAdminAgents.Count;
                        for (int j = 0; j < countAdminAgent; j++)
                        {
                            if (iAdminAgents[j].AgentID == agentID)
                            {
                                result += "|" + iAdminAgents[j].IAdminAgentID + "{" + iAdminAgents[j].AdminID + "{" + iAdminAgents[j].AgentID + "{" +
                                    iAdminAgents[j].SymbolID + "{" + iAdminAgents[j].DefaultPL + "{" + iAdminAgents[j].IsDelete + "{" +
                                    iAdminAgents[j].PercentPL + "{" + iAdminAgents[j].IsSkipRisk;

                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iAgentWithAgents"></param>
        /// <param name="iAdminAgents"></param>
        /// <param name="agentiD"></param>
        /// <returns></returns>
        internal string GetAgentWithAgentRefConfigBySymbolID(List<TradingServer.Agent.IAgentWithAgent> iAgentWithAgents, 
            List<TradingServer.Agent.IAdminAgent> iAdminAgents, int agentID, int symbolID)
        {
            string result = string.Empty;

            if (iAgentWithAgents != null)
            {
                int count = iAgentWithAgents.Count;
                bool isExits = false;
                int agentAdminID = -1;
                for (int i = 0; i < count; i++)
                {
                    if (iAgentWithAgents[i].SymbolID == symbolID)
                    {
                        if (iAgentWithAgents[i].AgentID == agentID)
                        {
                            result = iAgentWithAgents[i].IAgentWithAgentID + "{" + iAgentWithAgents[i].AgentID + "{" + iAgentWithAgents[i].AgentRefID + "{" +
                                iAgentWithAgents[i].SymbolID + "{" + iAgentWithAgents[i].DefaultPL + "{" + iAgentWithAgents[i].DefaultPLParent + "{" +
                                iAgentWithAgents[i].PercentPLParent + "{" + iAgentWithAgents[i].IsDelete + "{" + iAgentWithAgents[i].PercentPL + "{" +
                                iAgentWithAgents[i].IsSkipRisk + "{" + iAgentWithAgents[i].PercentPLChild + "{" + iAgentWithAgents[i].IsSkipRiskChild + "]";

                            result += this.GetAgentWithAgentRefConfig(iAgentWithAgents, iAdminAgents, iAgentWithAgents[i].AgentRefID);
                            isExits = true;

                            agentAdminID = iAgentWithAgents[i].AgentID;

                            break;
                        }
                    }
                }

                if (!isExits)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (iAgentWithAgents[i].AgentID == agentID)
                        {
                            result = iAgentWithAgents[i].IAgentWithAgentID + "{" + iAgentWithAgents[i].AgentID + "{" + iAgentWithAgents[i].AgentRefID + "{" +
                                    iAgentWithAgents[i].SymbolID + "{" + iAgentWithAgents[i].DefaultPL + "{" + iAgentWithAgents[i].DefaultPLParent + "{" +
                                    iAgentWithAgents[i].PercentPLParent + "{" + iAgentWithAgents[i].IsDelete + "{" + iAgentWithAgents[i].PercentPL + "{" +
                                    iAgentWithAgents[i].IsSkipRisk + "{" + iAgentWithAgents[i].PercentPLChild + "{" + iAgentWithAgents[i].IsSkipRiskChild + "]";

                            result += this.GetAgentWithAgentRefConfig(iAgentWithAgents, iAdminAgents, iAgentWithAgents[i].AgentRefID);
                            isExits = true;

                            agentAdminID = iAgentWithAgents[i].AgentID;

                            break;
                        }
                    }
                }

                if (!isExits)
                {
                    if (iAdminAgents != null)
                    {
                        int countAdminAgent = iAdminAgents.Count;
                        for (int j = 0; j < countAdminAgent; j++)
                        {
                            if (iAdminAgents[j].SymbolID == symbolID)
                            {
                                if (iAdminAgents[j].AgentID == agentID)
                                {
                                    result += "|" + iAdminAgents[j].IAdminAgentID + "{" + iAdminAgents[j].AdminID + "{" + iAdminAgents[j].AgentID + "{" +
                                        iAdminAgents[j].SymbolID + "{" + iAdminAgents[j].DefaultPL + "{" + iAdminAgents[j].IsDelete + "{" +
                                        iAdminAgents[j].PercentPL + "{" + iAdminAgents[j].IsSkipRisk;

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iAgentWithAgentComm"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal string GetIAgentWithAgentCommission(List<TradingServer.Agent.IParameterCommission> iAgentWithAgentComm, 
            List<TradingServer.Agent.IParameterCommission> iAdminAgentComm, int agentID, int groupID, int symbolID)
        {
            string result = string.Empty;

            if (iAgentWithAgentComm != null)
            {
                bool isExits = false;
                int adminID = -1;
                int agentRefID = -1;
                int count = iAgentWithAgentComm.Count;
                for (int i = 0; i < count; i++)
                {
                    if (iAgentWithAgentComm[i].SecondParameterID == agentID && iAgentWithAgentComm[i].GroupID == groupID &&
                        iAgentWithAgentComm[i].SymbolID == symbolID)
                    {
                        result = iAgentWithAgentComm[i].IParameterID + "{" + iAgentWithAgentComm[i].FirstParameterID + "{" + iAgentWithAgentComm[i].SecondParameterID + "{" +
                            iAgentWithAgentComm[i].GroupID + "{" + iAgentWithAgentComm[i].SymbolID + "{" + iAgentWithAgentComm[i].Comission + "{" + iAgentWithAgentComm[i].IsDelete + "{" +
                            iAgentWithAgentComm[i].ParentCommission + "{" + iAgentWithAgentComm[i].ChildCommission + "{" + iAgentWithAgentComm[i].ParentPipReBate + "{" +
                            iAgentWithAgentComm[i].ChildPipReBate + "]";

                        result += this.GetIAgentWithAgentCommission(iAgentWithAgentComm, iAdminAgentComm, iAgentWithAgentComm[i].FirstParameterID, groupID, symbolID);

                        isExits = true;
                        agentRefID = iAgentWithAgentComm[i].FirstParameterID;
                    }
                }

                if (iAdminAgentComm != null)
                {
                    int countIAdminAgentCom = iAdminAgentComm.Count;
                    for (int i = 0; i < countIAdminAgentCom; i++)
                    {
                        if (iAdminAgentComm[i].SecondParameterID == agentRefID &&
                            iAdminAgentComm[i].GroupID == groupID && iAdminAgentComm[i].SymbolID == symbolID)
                        {
                            result += "|" + iAdminAgentComm[i].IParameterID + "{" + iAdminAgentComm[i].FirstParameterID + "{" +
                                iAdminAgentComm[i].SecondParameterID + "{" + iAdminAgentComm[i].GroupID + "{" + iAdminAgentComm[i].SymbolID + "{" +
                                iAdminAgentComm[i].Comission + "{" + iAdminAgentComm[i].IsDelete + "{" + iAdminAgentComm[i].ParentCommission + "{" +
                                iAdminAgentComm[i].ChildCommission + "{" + iAdminAgentComm[i].ParentPipReBate + "{" + iAdminAgentComm[i].ChildPipReBate;

                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
