using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWAgent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Agent> GetAllAgent()
        {
            List<Business.Agent> Result              = new List<Business.Agent>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap   = new DSTableAdapters.AgentTableAdapter();
            DS.AgentDataTable tbAgent                = new DS.AgentDataTable();

            try
            {
                conn.Open();
                adap.Connection                      = conn;
                tbAgent                              = adap.GetData();

                if (tbAgent != null)
                {
                    int count                        = tbAgent.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Agent newAgent      = new Business.Agent();
                        newAgent.AgentID             = tbAgent[i].AgentID;
                        newAgent.AgentGroupID        = tbAgent[i].AgentGroupID;
                        newAgent.Name                = tbAgent[i].Name;
                        newAgent.InvestorID          = tbAgent[i].InvestorID;
                        newAgent.Comment             = tbAgent[i].Comment;
                        newAgent.IsDisable           = tbAgent[i].Isdiable;
                        newAgent.IsOnline            = false;
                        newAgent.Code                = tbAgent[i].Code;
                        newAgent.IsIpFilter          = tbAgent[i].IsIpFilter;
                        newAgent.IpForm              = tbAgent[i].IpForm;
                        newAgent.IpTo                = tbAgent[i].IpTo;
                        newAgent.GroupCondition      = tbAgent[i].GroupCondition;
                        Result.Add(newAgent);
                    }
                }
            }
            catch (Exception ex)
            {
                Result                               = null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal Business.Agent GetAgentByAgentID(int AgentID)
        {
            Business.Agent Result                    = new Business.Agent();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap   = new DSTableAdapters.AgentTableAdapter();
            DS.AgentDataTable tbAgent                = new DS.AgentDataTable();

            try
            {
                conn.Open();
                adap.Connection                      = conn;
                tbAgent                              = adap.GetAgentByAgentID(AgentID);

                if (tbAgent != null)
                {
                    Result.AgentID                   = tbAgent[0].AgentID;
                    Result.AgentGroupID              = tbAgent[0].AgentGroupID;
                    Result.Name                      = tbAgent[0].Name;
                    Result.InvestorID                = tbAgent[0].InvestorID;
                    Result.Comment                   = tbAgent[0].Comment;
                    Result.IsDisable                 = tbAgent[0].Isdiable;
                    Result.IsIpFilter                = tbAgent[0].IsIpFilter;
                    Result.IpForm                    = tbAgent[0].IpForm;
                    Result.IpTo                      = tbAgent[0].IpTo;
                    Result.GroupCondition            = tbAgent[0].GroupCondition;
                }
            }
            catch (Exception ex)
            {
                Result                               = null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal Business.Agent GetAgentByInvestorID(int InvestorID)
        {
            Business.Agent Result                    = new Business.Agent();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap   = new DSTableAdapters.AgentTableAdapter();
            DS.AgentDataTable tbAgent                = new DS.AgentDataTable();

            try
            {
                conn.Open();
                adap.Connection                      = conn;
                tbAgent                              = adap.GetAgentByInvestorID(InvestorID);

                if (tbAgent != null)
                {
                    Result.AgentID                   = tbAgent[0].AgentID;
                    Result.AgentGroupID              = tbAgent[0].AgentGroupID;
                    Result.Name                      = tbAgent[0].Name;
                    Result.InvestorID                = tbAgent[0].InvestorID;
                    Result.Comment                   = tbAgent[0].Comment;
                    Result.IsDisable                 = tbAgent[0].Isdiable;
                    Result.IsIpFilter                = tbAgent[0].IsIpFilter;
                    Result.IpForm                    = tbAgent[0].IpForm;
                    Result.IpTo                      = tbAgent[0].IpTo;
                    Result.GroupCondition            = tbAgent[0].GroupCondition;
                }
            }
            catch (Exception ex)
            {
                Result                               = null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal List<Business.Agent> GetAgentByAgentGroupID(int AgentGroupID)
        {
            List<Business.Agent> Result              = new List<Business.Agent>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap   = new DSTableAdapters.AgentTableAdapter();
            DS.AgentDataTable tbAgent                = new DS.AgentDataTable();

            try
            {
                conn.Open();
                adap.Connection                      = conn;
                tbAgent                              = adap.GetAgentByAgentGroupID(AgentGroupID);

                if (tbAgent != null)
                {
                    int count                        = tbAgent.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Agent newAgent      = new Business.Agent();
                        newAgent.AgentID             = tbAgent[i].AgentID;
                        newAgent.AgentGroupID        = tbAgent[i].AgentGroupID;
                        newAgent.Name                = tbAgent[i].Name;
                        newAgent.InvestorID          = tbAgent[i].InvestorID;
                        newAgent.Comment             = tbAgent[i].Comment;
                        newAgent.IsDisable           = tbAgent[i].Isdiable;
                        newAgent.IsIpFilter          = tbAgent[i].IsIpFilter;
                        newAgent.IpForm              = tbAgent[i].IpForm;
                        newAgent.IpTo                = tbAgent[i].IpTo;
                        newAgent.GroupCondition      = tbAgent[i].GroupCondition;
                        Result.Add(newAgent);
                    }
                }
            }
            catch (Exception ex)
            {
                Result                               = null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewAgent(Business.Agent agent)
        {
            int idAgent = -1;
            System.Data.SqlClient.SqlTransaction trans = null;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adapAgent = new DSTableAdapters.AgentTableAdapter();
            DSTableAdapters.IAgentGroupTableAdapter adapIAgentGroup = new DSTableAdapters.IAgentGroupTableAdapter();
            try
            {
                conn.Open();
                trans                                          = conn.BeginTransaction();
                adapAgent.Connection                           = conn;
                adapAgent.Transaction                          = trans;

                adapIAgentGroup.Connection                     = conn;
                adapIAgentGroup.Transaction                    = trans;

                #region Create Investor
                Business.Investor investor                     = new Business.Investor();
                investor.InvestorStatusID                      = -1;
                investor.InvestorGroupInstance                 = new Business.InvestorGroup();
                investor.InvestorGroupInstance.InvestorGroupID = -1;
                investor.AgentID                               = string.Empty;
                investor.Balance                               = 0;
                investor.Credit                                = 0;
                investor.Code                                  = agent.Code;
                investor.PrimaryPwd                            = agent.Pwd;
                investor.ReadOnlyPwd                           = "";
                investor.PhonePwd                              = "";
                investor.IsDisable                             = true;
                investor.TaxRate                               = 0;
                investor.Leverage                              = 0;
                //Investor Profile                                        
                DateTime registerDay                           = new DateTime();
                investor.Address                               = "";
                investor.Phone                                 = "";
                investor.City                                  = "";
                investor.Country                               = "";
                investor.Email                                 = "";
                investor.ZipCode                               = "";
                investor.RegisterDay                           = registerDay;
                investor.Comment                               = "";
                investor.State                                 = "";
                investor.NickName                              = "";
                investor.AllowChangePwd                        = false;
                investor.ReadOnly                              = false;
                investor.SendReport                            = false;
                #endregion

                int idInvestor = TradingServer.Facade.FacadeAddNewInvestor(investor);
                if (idInvestor < 1)
                {
                    throw new Exception("Data error");
                }

                agent.InvestorID = idInvestor;
                if (agent.AgentGroupID == -1)
                {
                    idAgent = int.Parse(adapAgent.AddNewAgent(null, agent.Name, agent.InvestorID, agent.Comment, agent.IsDisable, agent.IsIpFilter, agent.IpForm, agent.IpTo, agent.GroupCondition).ToString());
                }
                else idAgent = int.Parse(adapAgent.AddNewAgent(agent.AgentGroupID, agent.Name, agent.InvestorID, agent.Comment, agent.IsDisable, agent.IsIpFilter, agent.IpForm, agent.IpTo, agent.GroupCondition).ToString());
                if (idAgent < 1)
                {
                    throw new Exception("Data error");
                }

                List<int> listInvestorGroupIDs = new List<int>();
                listInvestorGroupIDs = agent.MakeListIAgentGroupManager(agent.GroupCondition);
                int count = listInvestorGroupIDs.Count;
                for (int i = 0; i < count; i++)
                {
                    int idIAgentGroup = int.Parse(adapIAgentGroup.AddIAgentGroup(idAgent, listInvestorGroupIDs[i]).ToString());
                    if (idIAgentGroup < 1)
                    {
                        throw new Exception("Data error");
                    }
                }


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                idAgent = -1;
            }
            finally
            {
                adapAgent.Connection.Close();
                adapIAgentGroup.Connection.Close();
                conn.Close();
            }

            return idAgent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        internal bool UpdateAgent(Business.Agent agent)
        {
            bool Result = false;
            System.Data.SqlClient.SqlTransaction trans = null;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adapAgent = new DSTableAdapters.AgentTableAdapter();
            DSTableAdapters.IAgentGroupTableAdapter adapIAgentGroup = new DSTableAdapters.IAgentGroupTableAdapter();
            try
            {
                conn.Open();
                trans                          = conn.BeginTransaction();
                adapAgent.Connection           = conn;
                adapAgent.Transaction          = trans;

                adapIAgentGroup.Connection     = conn;
                adapIAgentGroup.Transaction    = trans;

                int Record                     = adapAgent.UpdateAgent(agent.AgentGroupID, agent.Name, agent.Comment, agent.IsDisable, agent.IsIpFilter, agent.IpForm, agent.IpTo, agent.GroupCondition, agent.AgentID);
                if (Record > 0)
                {
                    Result                     = true;
                }
                else
                {
                    throw new Exception("Data error");
                }
                adapIAgentGroup.DeleteIAgentGroupByAgentID(agent.AgentID);
                List<int> listInvestorGroupIDs = new List<int>();
                listInvestorGroupIDs           = agent.MakeListIAgentGroupManager(agent.GroupCondition);
                int count                      = listInvestorGroupIDs.Count;
                for (int i = 0; i < count; i++)
                {

                    int idIAgentGroup = int.Parse(adapIAgentGroup.AddIAgentGroup(agent.AgentID, listInvestorGroupIDs[i]).ToString());
                    if (idIAgentGroup < 1)
                    {
                        throw new Exception("Data error");
                    }
                }


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Result = false;
            }
            finally
            {
                adapAgent.Connection.Close();
                adapIAgentGroup.Connection.Close();
                conn.Close();
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal int DeleteAgentByID(int AgentID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap = new DSTableAdapters.AgentTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteAgentByAgentID(AgentID);
            }
            catch (Exception ex)
            {
                Result = -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal bool DeleteAgentByAgentGroupID(int AgentGroupID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap = new DSTableAdapters.AgentTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteAgentByAgentGroupID(AgentGroupID);
            }
            catch (Exception ex)
            {
                Result = false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal bool CheckAgentExist(int agentID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentTableAdapter adap = new DSTableAdapters.AgentTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultCheck = int.Parse(adap.CheckAgentExist(agentID).ToString());

                if(resultCheck>0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }
    }
}
