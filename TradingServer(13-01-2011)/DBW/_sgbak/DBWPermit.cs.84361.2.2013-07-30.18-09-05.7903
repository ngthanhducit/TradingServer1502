using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWPermit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Permit> GetAllPermit()
        {
            List<Business.Permit> Result = new List<Business.Permit>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();
            DS.PermitDataTable tbPermit = new DS.PermitDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbPermit = adap.GetData();

                if (tbPermit != null)
                {
                    int count = tbPermit.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Permit newPermit = new Business.Permit();
                        newPermit.AgentGroupID = tbPermit[i].AgentGroupID;
                        newPermit.AgentID = tbPermit[i].AgentID;
                        newPermit.PermitID = tbPermit[i].PermitID;
                        newPermit.Role = new Business.Role();
                        newPermit.Role.RoleID = tbPermit[i].RoleID;

                        Result.Add(newPermit);
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
        /// <param name="PermitID"></param>
        /// <returns></returns>
        internal Business.Permit GetPermitByPermitID(int PermitID)
        {
            Business.Permit Result = new Business.Permit();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();
            DS.PermitDataTable tbPermit = new DS.PermitDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbPermit = adap.GetPermitByPermitID(PermitID);
                if (tbPermit != null)
                {
                    Result.AgentGroupID = tbPermit[0].AgentGroupID;
                    Result.AgentID = tbPermit[0].AgentID;
                    Result.PermitID = tbPermit[0].PermitID;
                    Result.Role = new Business.Role();
                    Result.Role.RoleID = tbPermit[0].RoleID;
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
        internal List<Business.Permit> GetPermitByAgentGroupID(int AgentGroupID)
        {
            List<Business.Permit> Result = new List<Business.Permit>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();
            DS.PermitDataTable tbPermit = new DS.PermitDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbPermit = adap.GetPermitByAgentGroupID(AgentGroupID);
                if (tbPermit != null)
                {
                    int count = tbPermit.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Permit newPermit = new Business.Permit();
                        newPermit.AgentGroupID = tbPermit[i].AgentGroupID;
                        newPermit.AgentID = tbPermit[i].AgentID;
                        newPermit.PermitID = tbPermit[i].PermitID;
                        newPermit.Role = new Business.Role();
                        newPermit.Role.RoleID = tbPermit[i].RoleID;

                        Result.Add(newPermit);
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
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal List<Business.Permit> GetPermitByAgentID(int AgentID)
        {
            List<Business.Permit> Result = new List<Business.Permit>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();
            DS.PermitDataTable tbPermit = new DS.PermitDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbPermit = adap.GetPermitByAgentID(AgentID);
                if (tbPermit != null)
                {
                    int count = tbPermit.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Permit newPermit = new Business.Permit();
                        newPermit.AgentGroupID = tbPermit[i].AgentGroupID;
                        newPermit.AgentID = tbPermit[i].AgentID;
                        newPermit.PermitID = tbPermit[i].PermitID;
                        newPermit.Role = new Business.Role();
                        newPermit.Role = Facade.FacadeGetRoleByRoleID(tbPermit[i].RoleID);

                        Result.Add(newPermit);
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
        /// <param name="RoleID"></param>
        /// <returns></returns>
        internal List<Business.Permit> GetPermitByRoleID(int RoleID)
        {
            List<Business.Permit> Result = new List<Business.Permit>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();
            DS.PermitDataTable tbPermit = new DS.PermitDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbPermit = adap.GetPermitByRoleID(RoleID);
                if (tbPermit != null)
                {
                    int count = tbPermit.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Permit newPermit = new Business.Permit();
                        newPermit.AgentGroupID = tbPermit[i].AgentGroupID;
                        newPermit.AgentID = tbPermit[i].AgentID;
                        newPermit.PermitID = tbPermit[i].PermitID;
                        newPermit.Role = new Business.Role();
                        newPermit.Role.RoleID = tbPermit[i].RoleID;

                        Result.Add(newPermit);
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
        /// <param name="AgentID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        internal int AddNewPermit(int AgentGroupID, int AgentID, int RoleID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();            

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewPermit(AgentGroupID, AgentID, RoleID).ToString());
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
        /// <param name="ListRoleID"></param>
        /// <returns></returns>
        internal int AddListPermit(int AgentID, List<int> ListRoleID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int count = ListRoleID.Count;
                for (int i = 0; i < count; i++)
                {
                    Result = int.Parse(adap.AddNewPermit(null, AgentID, ListRoleID[i]).ToString());
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
        /// 
        /// </summary>
        /// <param name="PermitID"></param>
        /// <param name="AgentGroupID"></param>
        /// <param name="AgentID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        internal bool UpdatePermit(int PermitID, int AgentGroupID, int AgentID, int RoleID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int Record = adap.UpdatePermit(AgentGroupID, AgentID, RoleID, PermitID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PermitID"></param>
        /// <returns></returns>
        internal bool DeletePermitByID(int PermitID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeletePermitByPermitID(PermitID);
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

        internal int DeletePermitByAgentID(int AgentID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.PermitTableAdapter adap = new DSTableAdapters.PermitTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeletePermitByAgentID(AgentID);
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
    }
}
