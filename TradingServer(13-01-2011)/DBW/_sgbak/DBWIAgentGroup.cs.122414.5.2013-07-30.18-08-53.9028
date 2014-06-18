using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWIAgentGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IAgentGroup> GetAllIAgentGroup()
        {
            List<Business.IAgentGroup> Result = new List<Business.IAgentGroup>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();
            DS.IAgentGroupDataTable tbIAgentGroup = new DS.IAgentGroupDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentGroup = adap.GetData();
                if (tbIAgentGroup != null)
                {
                    int count = tbIAgentGroup.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IAgentGroup newIAgentGroup = new Business.IAgentGroup();
                        newIAgentGroup.IAgentGroupID = tbIAgentGroup[i].IAgentGroupID;
                        newIAgentGroup.AgentID = tbIAgentGroup[i].AgentID;
                        newIAgentGroup.InvestorGroupID = tbIAgentGroup[i].InvestorGroupID;

                        Result.Add(newIAgentGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
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
        /// <param name="IAgentGroupID"></param>
        /// <returns></returns>
        internal Business.IAgentGroup GetIAgentGroupByIAgentGroupID(int IAgentGroupID)
        {
            Business.IAgentGroup Result = new Business.IAgentGroup();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();
            DS.IAgentGroupDataTable tbIAgentGroup = new DS.IAgentGroupDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentGroup = adap.GetIAgentGroupByIAgentGroupID(IAgentGroupID);

                if (tbIAgentGroup != null)
                {
                    Result.IAgentGroupID = tbIAgentGroup[0].IAgentGroupID;
                    Result.InvestorGroupID = tbIAgentGroup[0].InvestorGroupID;
                    Result.AgentID = tbIAgentGroup[0].AgentID;
                }
            }
            catch (Exception ex)
            {
                return null;
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
        internal List<Business.IAgentGroup> GetIAgentGroupByAgentID(int AgentID)
        {
            List<Business.IAgentGroup> Result = new List<Business.IAgentGroup>();
            Business.Agent Agent = new Business.Agent();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adapIAgentGroup = new DSTableAdapters.IAgentGroupTableAdapter();
            DS.IAgentGroupDataTable tbIAgentGroup = new DS.IAgentGroupDataTable();

            DSTableAdapters.AgentTableAdapter adapAgent = new DSTableAdapters.AgentTableAdapter();
            DS.AgentDataTable tbAgent = new DS.AgentDataTable();
            try
            {
                conn.Open();
                adapIAgentGroup.Connection = conn;
                tbIAgentGroup = adapIAgentGroup.GetIAgentGroupByAgentID(AgentID);

                adapAgent.Connection = conn;
                tbAgent = adapAgent.GetAgentByAgentID(AgentID);

                if (tbIAgentGroup != null)
                {
                    int count = tbIAgentGroup.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IAgentGroup newIGroupSymbol = new Business.IAgentGroup();
                        newIGroupSymbol.IAgentGroupID = tbIAgentGroup[i].IAgentGroupID;
                        newIGroupSymbol.InvestorGroupID = tbIAgentGroup[i].InvestorGroupID;
                        newIGroupSymbol.AgentID = tbIAgentGroup[i].AgentID;

                        Result.Add(newIGroupSymbol);
                    }
                    if (tbAgent != null)
                    {
                        Agent.AgentID = tbAgent[0].AgentID;
                        Agent.AgentGroupID = tbAgent[0].AgentGroupID;
                        Agent.Name = tbAgent[0].Name;
                        Agent.InvestorID = tbAgent[0].InvestorID;
                        Agent.Comment = tbAgent[0].Comment;
                        Agent.IsDisable = tbAgent[0].Isdiable;
                        Agent.IsIpFilter = tbAgent[0].IsIpFilter;
                        Agent.IpForm = tbAgent[0].IpForm;
                        Agent.IpTo = tbAgent[0].IpTo;
                        Agent.GroupCondition = tbAgent[0].GroupCondition;
                        List<int> listInvestorGroupIDs = Agent.MakeListIAgentGroupManager(Agent.GroupCondition);
                        for (int i = 0; i < listInvestorGroupIDs.Count; i++)
                        {
                            bool check = true;
                            for (int j = 0; j < Result.Count; j++)
                            {
                                if (listInvestorGroupIDs[i] == Result[j].InvestorGroupID)
                                {
                                    check = false;
                                    break;
                                }
                            }
                            if (check == true)
                            {
                                int idIAgentGroup = int.Parse(adapIAgentGroup.AddIAgentGroup(Agent.AgentID, listInvestorGroupIDs[i]).ToString());
                                if (idIAgentGroup > 0)
                                {
                                    Business.IAgentGroup newIGroupSymbol = new Business.IAgentGroup();
                                    newIGroupSymbol.IAgentGroupID = idIAgentGroup;
                                    newIGroupSymbol.InvestorGroupID = listInvestorGroupIDs[i];
                                    newIGroupSymbol.AgentID = Agent.AgentID;
                                    Result.Add(newIGroupSymbol);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adapIAgentGroup.Connection.Close();
                adapAgent.Connection.Close();
                conn.Close();
            }

            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.IAgentGroup> GetIAgentGroupByInvestorGroupID(int InvestorGroupID)
        {
            List<Business.IAgentGroup> Result = new List<Business.IAgentGroup>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();
            DS.IAgentGroupDataTable tbIAgentGroup = new DS.IAgentGroupDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentGroup = adap.GetIAgentGroupByInvestorGroupID(InvestorGroupID);

                if (tbIAgentGroup != null)
                {
                    int count = tbIAgentGroup.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IAgentGroup newIGroupSymbol = new Business.IAgentGroup();
                        newIGroupSymbol.IAgentGroupID = tbIAgentGroup[i].IAgentGroupID;
                        newIGroupSymbol.InvestorGroupID = tbIAgentGroup[i].InvestorGroupID;
                        newIGroupSymbol.AgentID = tbIAgentGroup[i].AgentID;

                        Result.Add(newIGroupSymbol);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
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
        /// <param name="IAgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal bool UpdateIAgentGroup(int IAgentGroupID, int AgentID, int InvestorGroupID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateIAgentGroup(AgentID, InvestorGroupID,IAgentGroupID);
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

            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IAgentGroupID"></param>
        /// <returns></returns>
        internal bool DeleteIAgentGroupByIAgentGroupID(int IAgentGroupID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIAgentGroupByIAgentGroupID(IAgentGroupID);
                Result = true;
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
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal int DeleteIAgentGroupByAgentID(int AgentID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteIAgentGroupByAgentID(AgentID);
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
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal bool DeleteIAgentGroupByInvestorGroupID(int InvestorGroupID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIAgentGroupByInvestorGroupID(InvestorGroupID);
                Result = true;
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
        /// <param name="AgentID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal int AddNewIAgentGroup(int AgentID, int InvestorGroupID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddIAgentGroup(AgentID, InvestorGroupID).ToString());
            }
            catch (Exception ex)
            {
                return -1;
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
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal int AddNewIAgentGroups(int AgentID, List<int> ListInvestorGroupID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int count = ListInvestorGroupID.Count;
                for (int i = 0; i < count; i++)
                {
                    Result = int.Parse(adap.AddIAgentGroup(AgentID, ListInvestorGroupID[i]).ToString());
                }
                    
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }
        /// <summary>
        /// delete by investorgroupid
        /// </summary>
        /// <param name="id">investor group id</param>
        /// <param name="connection">opened sql connection</param>
        /// <returns></returns>
        internal bool DFDeleteByInvestorGroupID(int id, SqlConnection connection,SqlTransaction trans)
        {
            DSTableAdapters.IAgentGroupTableAdapter adap = new DSTableAdapters.IAgentGroupTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            adap.DeleteIAgentGroupByIAgentGroupID(id);
            return true;
        }

    }
}
