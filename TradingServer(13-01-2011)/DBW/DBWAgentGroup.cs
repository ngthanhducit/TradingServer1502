using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWAgentGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.AgentGroup> GetAllAgentGroup()
        {
            List<Business.AgentGroup> Result = new List<Business.AgentGroup>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentGroupTableAdapter adap = new DSTableAdapters.AgentGroupTableAdapter();
            DS.AgentGroupDataTable tbAgentGroup = new DS.AgentGroupDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbAgentGroup = adap.GetData();

                if (tbAgentGroup != null)
                {
                    int count = tbAgentGroup.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.AgentGroup newAgentGroup = new Business.AgentGroup();
                        newAgentGroup.AgentGroupID = tbAgentGroup[i].AgentGroupID;
                        newAgentGroup.Name = tbAgentGroup[i].Name;
                        newAgentGroup.Comment = tbAgentGroup[i].Comment;

                        Result.Add(newAgentGroup);
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
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal Business.AgentGroup GetAgentGroupByAgentGroupID(int AgentGroupID)
        {
            Business.AgentGroup Result = new Business.AgentGroup();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentGroupTableAdapter adap = new DSTableAdapters.AgentGroupTableAdapter();
            DS.AgentGroupDataTable tbAgentGroup = new DS.AgentGroupDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbAgentGroup = adap.GetAgentGroupByAgentGroupID(AgentGroupID);

                if (tbAgentGroup != null)
                {
                    Result.AgentGroupID = tbAgentGroup[0].AgentGroupID;
                    Result.Name = tbAgentGroup[0].Name;
                    Result.Comment = tbAgentGroup[0].Comment;
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
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewAgentGroup(string Name,string Comment)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentGroupTableAdapter adap = new DSTableAdapters.AgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewAgentGroup(Name,Comment).ToString());
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
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal bool DeleteAgentGroupByAgentGroupID(int AgentGroupID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentGroupTableAdapter adap = new DSTableAdapters.AgentGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteAgentGroupByID(AgentGroupID);
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
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        internal bool UpdateAgentGroup(int AgentGroupID, string Name,string Comment)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AgentGroupTableAdapter adap = new DSTableAdapters.AgentGroupTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;
                int Record = adap.UpdateAgentGroup(Name, Comment, AgentGroupID);
                if (Record > 0)
                    Result = true;
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
    }
}
