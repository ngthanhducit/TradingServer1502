using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWRole
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Role> GetAllRole()
        {
            List<Business.Role> Result = new List<Business.Role>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.RoleTableAdapter adap = new DSTableAdapters.RoleTableAdapter();
            DS.RoleDataTable tbRole = new DS.RoleDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbRole = adap.GetData();

                if (tbRole != null)
                {
                    int count = tbRole.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Role newRole = new Business.Role();
                        newRole.Code = tbRole[i].Code;
                        newRole.Comment = tbRole[i].Comment;
                        newRole.Name = tbRole[i].Name;
                        newRole.RoleID = tbRole[i].RoleID;

                        Result.Add(newRole);
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
        internal Business.Role GetRoleByRoleID(int RoleID)
        {
            Business.Role Result = new Business.Role();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.RoleTableAdapter adap = new DSTableAdapters.RoleTableAdapter();
            DS.RoleDataTable tbRole = new DS.RoleDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbRole = adap.GetRoleByRoleID(RoleID);

                if (tbRole != null)
                {
                    Result.Code = tbRole[0].Code;
                    Result.Comment = tbRole[0].Comment;
                    Result.Name = tbRole[0].Name;
                    Result.RoleID = tbRole[0].RoleID;
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

        internal List<Business.Role> GetListRoleByListRoleID(List<int> ListRoleID)
        {
            List<Business.Role> Result = new List<Business.Role>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.RoleTableAdapter adap = new DSTableAdapters.RoleTableAdapter();
            DS.RoleDataTable tbRole = new DS.RoleDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int count = ListRoleID.Count;
                for (int i = 0; i < count; i++)
                {
                    tbRole = adap.GetRoleByRoleID(ListRoleID[i]);
                    if (tbRole != null)
                    {
                        Business.Role newRole = new Business.Role();
                        newRole.Code = tbRole[0].Code;
                        newRole.Comment = tbRole[0].Comment;
                        newRole.Name = tbRole[0].Name;
                        newRole.RoleID = tbRole[0].RoleID;
                        Result.Add(newRole);
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
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.Role GetRoleByCode(string Code)
        {
            Business.Role Result = new Business.Role();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.RoleTableAdapter adap = new DSTableAdapters.RoleTableAdapter();
            DS.RoleDataTable tbRole = new DS.RoleDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbRole = adap.GetRoleByCode(Code);

                if (tbRole != null)
                {
                    Result.Code = tbRole[0].Code;
                    Result.Comment = tbRole[0].Comment;
                    Result.Name = tbRole[0].Name;
                    Result.RoleID = tbRole[0].RoleID;
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
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        internal int AddNewRole(string Code, string Name, string Comment)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.RoleTableAdapter adap = new DSTableAdapters.RoleTableAdapter();

            try
            {
                Result = int.Parse(adap.AddNewRole(Code, Comment, Name).ToString());
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
        /// <param name="RoleID"></param>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        internal bool UpdateRole(int RoleID, string Code, string Name, string Comment)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.RoleTableAdapter adap = new DSTableAdapters.RoleTableAdapter();

            try
            {
                int Record = adap.UpdateRole(Code, Comment, Name, RoleID);
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
