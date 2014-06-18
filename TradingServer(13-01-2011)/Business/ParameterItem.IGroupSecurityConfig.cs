using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class ParameterItem
    {        
        /// <summary>
        /// GET ALL IGROUP SECURITY CONFIG IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllIGroupSecurityConfig()
        {
            return ParameterItem.DBWIGroupSecurityConfig.GetAllIGroupSecurityConfig();
        }

        /// <summary>
        /// ADD NEW IGROUP SECURITY CONFIG
        /// </summary>
        /// <param name="ListParameterItem"></param>
        /// <returns></returns>
        internal int AddNewIGroupSecurityConfig(List<Business.ParameterItem> ListParameterItem)
        {
            int Result = -1;
            if (Business.Market.IGroupSecurityList != null)
            {
                //Find In List IgroupSecurity If IGroupSecurity Exists Then Add IGroupSecurityConfig To Database
                //
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].IGroupSecurityID == ListParameterItem[0].SecondParameterID)
                    {
                        Business.Market.IGroupSecurityList[i].IGroupSecurityConfig = new List<ParameterItem>();
                        int countParameter = ListParameterItem.Count;
                        for (int j = 0; j < countParameter; j++)
                        {
                            Result = ParameterItem.DBWIGroupSecurityConfig.AddIGroupSecurityConfig(ListParameterItem[j].SecondParameterID, -1, ListParameterItem[j].Name,
                                ListParameterItem[j].Code, ListParameterItem[j].BoolValue, ListParameterItem[j].StringValue, ListParameterItem[j].NumValue, ListParameterItem[j].DateValue);

                            ListParameterItem[j].ParameterItemID = Result;

                            if (Result > 0)
                            {
                                if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig == null)
                                    Business.Market.IGroupSecurityList[i].IGroupSecurityConfig = new List<ParameterItem>();

                                Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Add(ListParameterItem[j]);
                            }
                        }

                        DateTime startTime = DateTime.Now;
                        TradingServer.Facade.FacadeResetIGroupSecurityInCommand(Business.Market.IGroupSecurityList[i].InvestorGroupID, Business.Market.IGroupSecurityList[i].SecurityID,
                                                                                Business.Market.IGroupSecurityList[i].IGroupSecurityID);

                        DateTime stopTime = DateTime.Now;
                        TimeSpan timeProcess = stopTime - startTime;
                        double second = timeProcess.TotalSeconds;

                        break;
                    }
                }
            }
            
            return Result;
        }

        /// <summary>
        /// UPDATE IGROUP SECURITY CONFIG 
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal bool UpdateIGroupSecurityConfig(Business.ParameterItem objParameterItem)
        {
            bool Result = false;

            try
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int count = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityID == objParameterItem.SecondParameterID)
                        {
                            if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                            {
                                bool isValid = true;
                                bool isValidPipRebate = true;
                                double spreadByDefault = 0;
                                double sparedDiffirence = 0;
                                int symbolID = 0;
                                double totalPip = 0;

                                int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;

                                #region GET SPREAD BY DEFAULT IN GROUP
                                //if (Business.Market.InvestorGroupList != null)
                                //{
                                //    int countGroup = Business.Market.InvestorGroupList.Count;
                                //    for (int j = 0; j < countGroup; j++)
                                //    {
                                //        if (Business.Market.InvestorGroupList[j].InvestorGroupID == Business.Market.IGroupSecurityList[i].InvestorGroupID)
                                //        {
                                //            if (Business.Market.InvestorGroupList[j].ParameterItems != null)
                                //            {
                                //                int countGroupParameter = Business.Market.InvestorGroupList[j].ParameterItems.Count;
                                //                for (int n = 0; n < countGroupParameter; n++)
                                //                {
                                //                    if (Business.Market.InvestorGroupList[j].ParameterItems[n].Code == "S013")
                                //                    {
                                //                        spreadByDefault = double.Parse(Business.Market.InvestorGroupList[j].ParameterItems[n].NumValue);

                                //                        break;
                                //                    }
                                //                }
                                //            }
                                //            break;
                                //        }
                                //    }
                                //}
                                #endregion

                                if (objParameterItem.Code == "B04")
                                {
                                    sparedDiffirence = double.Parse(objParameterItem.NumValue);

                                    if (Business.Market.SymbolList != null)
                                    {
                                        int countSymbol = Business.Market.SymbolList.Count;
                                        for (int n = 0; n < countSymbol; n++)
                                        {
                                            if (Business.Market.SymbolList[n].SecurityID == Business.Market.IGroupSecurityList[i].SecurityID)
                                            {
                                                if (Business.Market.SymbolList[n].ParameterItems != null)
                                                {
                                                    int countPara = Business.Market.SymbolList[n].ParameterItems.Count;
                                                    for (int m = 0; m < countPara; m++)
                                                    {
                                                        if (Business.Market.SymbolList[n].ParameterItems[m].Code == "S013")
                                                        {
                                                            spreadByDefault = double.Parse(Business.Market.SymbolList[n].ParameterItems[m].NumValue);
                                                            break;
                                                        }
                                                    }
                                                }

                                                symbolID = Business.Market.SymbolList[n].SymbolID;

                                                #region SEND COMMAND REQUEST TO AGENT CHECK VALID UPDATE VALUE IN IGROUPSECURITY CONFIG
                                                if (Business.Market.IsConnectAgent)
                                                {
                                                    totalPip = spreadByDefault + sparedDiffirence;
                                                    if (Business.Market.ListAgentConfig != null)
                                                    {
                                                        int countAgent = Business.Market.ListAgentConfig.Count;
                                                        for (int k = 0; k < countAgent; k++)
                                                        {
                                                            string strCmd = "UpdateGroupPipRebate$" + totalPip + "{" + symbolID;
                                                            string agentResult = Business.Market.ListAgentConfig[k].clientAgent.StringDefaultPort(strCmd, "");
                                                            string[] subValue = agentResult.Split('$');
                                                            if (subValue[0] == "UpdateGroupPipRByET5")
                                                                isValidPipRebate = bool.Parse(subValue[1]);
                                                        }
                                                    }
                                                }
                                                #endregion

                                                break;
                                            }
                                        }
                                    }
                                }
                                

                                #region GET SPARED DIFFIRENCE IN IGROUPSECURITY AND GET SYMBOL ID
                                for (int j = 0; j < countParameter; j++)
                                {
                                    if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == "B04")
                                    {
                                        sparedDiffirence = double.Parse(Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue);

                                       

                                        break;
                                    }
                                }
                                #endregion

                                #region SEND COMMAND REQUEST TO AGENT CHECK VALID UPDATE COMMISSION IN IGROUPSECUITY CONFIG
                                if (Business.Market.IsConnectAgent)
                                {
                                    if (objParameterItem.Code == "B14")
                                    {
                                        ///UpdateGroupCommByET5$commission{groupID
                                        ///
                                        string strCmd = "UpdateGroupCommByET5$" + objParameterItem.NumValue + "{" + Business.Market.IGroupSecurityList[i].InvestorGroupID;
                                        if (Business.Market.ListAgentConfig != null)
                                        {
                                            int countAgent = Business.Market.ListAgentConfig.Count;
                                            for (int j = 0; j < countAgent; j++)
                                            {
                                                string agentResult = Business.Market.ListAgentConfig[j].clientAgent.StringDefaultPort(strCmd, "");
                                                string[] subValue = agentResult.Split('$');
                                                if (subValue[0] == "UpdateGroupCommByET5")
                                                    isValid = bool.Parse(subValue[1]);
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (isValid && isValidPipRebate)
                                {
                                    for (int j = 0; j < countParameter; j++)
                                    {
                                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == objParameterItem.Code)
                                        {
                                            #region UPDATE VALUE DEFAULT SPREAD DIFFIRENCE IN SYMBOL LIST
                                            if (objParameterItem.Code == "B04")
                                            {
                                                if (Business.Market.SymbolList != null)
                                                {
                                                    int countSymbol = Business.Market.SymbolList.Count;
                                                    for (int n = 0; n < countSymbol; n++)
                                                    {
                                                        if (Business.Market.SymbolList[n].SecurityID == Business.Market.IGroupSecurityList[i].SecurityID)
                                                        {
                                                            Business.Market.SymbolList[n].SpreadDifference = double.Parse(objParameterItem.NumValue);
                                                            if (Business.Market.SymbolList[n].CommandList != null)
                                                            {
                                                                int countCommand = Business.Market.SymbolList[n].CommandList.Count;
                                                                for (int m = 0; m < countCommand; m++)
                                                                {
                                                                    Business.Market.SymbolList[n].CommandList[m].SpreaDifferenceInOpenTrade = double.Parse(objParameterItem.NumValue);
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region UPDATE COMMISSION AND NOTIFY TO AGENT
                                            if (Business.Market.IsConnectAgent)
                                            {
                                                if (objParameterItem.Code == "B14")
                                                {
                                                    //SEND COMMAND TO AGENT SERVER
                                                    //UpdateGroupCommission$Commisson{GroupID
                                                    string message = "UpdateGroupCommission$" + objParameterItem.NumValue + "{" + Business.Market.IGroupSecurityList[i].InvestorGroupID;
                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = message;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                                                }
                                            }
                                            #endregion

                                            #region UPDATE VALUE
                                            //Update Value Parameter Item
                                            Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Name = objParameterItem.Name;
                                            Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code = objParameterItem.Code;
                                            Market.IGroupSecurityList[i].IGroupSecurityConfig[j].BoolValue = objParameterItem.BoolValue;
                                            Market.IGroupSecurityList[i].IGroupSecurityConfig[j].StringValue = objParameterItem.StringValue;
                                            Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue = objParameterItem.NumValue;
                                            Market.IGroupSecurityList[i].IGroupSecurityConfig[j].DateValue = objParameterItem.DateValue;

                                            //Set ParameterID 
                                            objParameterItem.ParameterItemID = Market.IGroupSecurityList[i].IGroupSecurityConfig[j].ParameterItemID;

                                            Result = ParameterItem.DBWIGroupSecurityConfig.UpdateIGroupSecurityConfig(objParameterItem.ParameterItemID, objParameterItem.SecondParameterID, -1,
                                                        objParameterItem.Name, objParameterItem.Code, objParameterItem.BoolValue, objParameterItem.StringValue, objParameterItem.NumValue, objParameterItem.DateValue);
                                            #endregion

                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Result = false;

                                    //LOG SYSTEM CAN'T UPDATE BECAUSE AGENT CAN'T UPDATE
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, "can't update igroupsecurity because agent can't update", "[Update IGroupSecurity]", "", "");
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(5, ex.ToString(), "[UpdateIGroupSecurity]", "", "");
            }

            return Result;
        }

        /// <summary>
        /// DELETE IGROUPSECURITY CONFIG BY IGROUPSECURITYID IN DATABASE
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityConfig(int IGroupSecurityID)
        {
            return ParameterItem.DBWIGroupSecurityConfig.DeleteIGroupSecurityConfigByIGroupSecurityID(IGroupSecurityID);
        }

        /// <summary>
        /// DELETE IGROUPSECURITYCONFIG BY IGROUPSECURITY ID IN RAM AND DATABASE
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityConfigByIGroupSecurityID(int IGroupSecurityID)
        {
            bool Result = false;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].IGroupSecurityID == IGroupSecurityID)
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null &&
                                Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count > 0)
                            {
                                bool ResultDelete = TradingServer.Facade.FacadeDeleteIGroupSecurityConfigByIGroupSecurityID(IGroupSecurityID);

                                if (ResultDelete == true)
                                {
                                    int countConfig = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                                    for (int j = 0; j < Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count; j++)
                                    {
                                        Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.RemoveAt(j);
                                    }

                                    Result = true;
                                }
                            }

                            ts.Complete();
                        }

                        break;
                    }
                }
            }

            return Result;
        }        
    }
}
