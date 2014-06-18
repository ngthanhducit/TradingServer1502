using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWIAgentSecurity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IAgentSecurity> GetAllIAgentSecurity()
        {
            List<Business.IAgentSecurity> Result = new List<Business.IAgentSecurity>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();
            DS.IAgentSecurityDataTable tbIAgentSecurity = new DS.IAgentSecurityDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentSecurity = adap.GetData();
                if (tbIAgentSecurity != null)
                {
                    int count = tbIAgentSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IAgentSecurity newIAgentSecurity = new Business.IAgentSecurity();
                        newIAgentSecurity.IAgentSecurityID = tbIAgentSecurity[i].IAgentSecurityID;
                        newIAgentSecurity.AgentID = tbIAgentSecurity[i].AgentID;
                        newIAgentSecurity.SecurityID = tbIAgentSecurity[i].SecurityID;
                        newIAgentSecurity.Use = tbIAgentSecurity[i].IsUse;
                        double min = 0;
                        double max = 0;
                        double.TryParse(tbIAgentSecurity[i].MinLots,out min);
                        double.TryParse(tbIAgentSecurity[i].MaxLots, out max);
                        newIAgentSecurity.MinLots = min;
                        newIAgentSecurity.MaxLots = max;
                        Result.Add(newIAgentSecurity);
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
        /// <param name="IAgentSecurityID"></param>
        /// <returns></returns>
        internal Business.IAgentSecurity GetIAgentSecurityByIAgentSecurityID(int IAgentSecurityID)
        {
            Business.IAgentSecurity Result = new Business.IAgentSecurity();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();
            DS.IAgentSecurityDataTable tbIAgentGroup = new DS.IAgentSecurityDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentGroup = adap.GetIAgentSecurityByIAgentSecurityID(IAgentSecurityID);

                if (tbIAgentGroup != null)
                {
                    Result.IAgentSecurityID = tbIAgentGroup[0].IAgentSecurityID;
                    Result.SecurityID = tbIAgentGroup[0].SecurityID;
                    Result.AgentID = tbIAgentGroup[0].AgentID;
                    double min = 0;
                    double max = 0;
                    double.TryParse(tbIAgentGroup[0].MinLots, out min);
                    double.TryParse(tbIAgentGroup[0].MaxLots, out max);
                    Result.MinLots = min;
                    Result.MaxLots = max;
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
        internal List<Business.IAgentSecurity> GetIAgentSecurityByAgentID(int AgentID)
        {
            List<Business.IAgentSecurity> Result = new List<Business.IAgentSecurity>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();
            DS.IAgentSecurityDataTable tbIAgentSecurity = new DS.IAgentSecurityDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentSecurity = adap.GetIAgentSecurityByAgentID(AgentID);

                if (tbIAgentSecurity != null)
                {
                    int count = tbIAgentSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IAgentSecurity newIAgentSecurity = new Business.IAgentSecurity();
                        newIAgentSecurity.IAgentSecurityID = tbIAgentSecurity[i].IAgentSecurityID;
                        newIAgentSecurity.AgentID = tbIAgentSecurity[i].AgentID;
                        newIAgentSecurity.SecurityID = tbIAgentSecurity[i].SecurityID;
                        double min = 0;
                        double max = 0;
                        newIAgentSecurity.Use = tbIAgentSecurity[i].IsUse;
                        double.TryParse(tbIAgentSecurity[i].MinLots, out min);
                        double.TryParse(tbIAgentSecurity[i].MaxLots, out max);
                        newIAgentSecurity.MinLots = min;
                        newIAgentSecurity.MaxLots = max;
                        Result.Add(newIAgentSecurity);
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
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal List<Business.IAgentSecurity> GetIAgentSecurityBySecurityID(int SecurityID)
        {
            List<Business.IAgentSecurity> Result = new List<Business.IAgentSecurity>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();
            DS.IAgentSecurityDataTable tbIAgentSecurity = new DS.IAgentSecurityDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIAgentSecurity = adap.GetIAgentSecurityBySecurityID(SecurityID);

                if (tbIAgentSecurity != null)
                {
                    int count = tbIAgentSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IAgentSecurity newIAgentSecurity = new Business.IAgentSecurity();
                        newIAgentSecurity.IAgentSecurityID = tbIAgentSecurity[i].IAgentSecurityID;
                        newIAgentSecurity.AgentID = tbIAgentSecurity[i].AgentID;
                        newIAgentSecurity.SecurityID = tbIAgentSecurity[i].SecurityID;
                        double min = 0;
                        double max = 0;
                        double.TryParse(tbIAgentSecurity[i].MinLots, out min);
                        double.TryParse(tbIAgentSecurity[i].MaxLots, out max);
                        newIAgentSecurity.MinLots = min;
                        newIAgentSecurity.MaxLots = max;
                        Result.Add(newIAgentSecurity);
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
        /// <param name="IAgentSecurityID"></param>
        /// <param name="AgentID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal bool UpdateIAgentSecurity(int IAgentSecurityID, int AgentID, int SecurityID,bool IsUse,string MinLots,string MaxLots)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateIAgentSecurity(AgentID, SecurityID,IsUse,MinLots,MaxLots,IAgentSecurityID);
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
        /// <param name="AgentID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal int AddNewIAgentSecurity(int AgentID, int SecurityID,bool IsUse,string MinLots,string MaxLots)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddIAgentSecurity(AgentID, SecurityID,IsUse,MinLots,MaxLots).ToString());
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

        internal int AddNewIAgentSecurityByListIAgentSecurity(List<Business.IAgentSecurity> ListIAgentSecurity)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int count = ListIAgentSecurity.Count;
                for (int i = 0; i < count; i++)
                {
                    Result = int.Parse(adap.AddIAgentSecurity(ListIAgentSecurity[i].AgentID, ListIAgentSecurity[i].SecurityID, ListIAgentSecurity[i].Use, ListIAgentSecurity[i].MinLots.ToString(), ListIAgentSecurity[i].MaxLots.ToString()).ToString());
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
        /// <param name="IAgentSecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIAgentSecurityByIAgentSecurityID(int IAgentSecurityID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIAgentSecurityByIAgentSecurityID(IAgentSecurityID);
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
        internal int DeleteIAgentSecurityByAgentID(int AgentID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteIAgentSecurityByAgentID(AgentID);
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
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIAgentSecurityBySecurityID(int SecurityID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIAgentSecurityBySecurityID(SecurityID);
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
        /// delete IAgentSecurity by securityID
        /// </summary>
        /// <param name="securityID"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal bool DFDeleteBySecurityID(int securityID,SqlConnection connection,SqlTransaction trans)
        {
            DSTableAdapters.IAgentSecurityTableAdapter adap = new DSTableAdapters.IAgentSecurityTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            int affectRow= adap.DeleteIAgentSecurityBySecurityID(securityID);
            if (affectRow == 0)
                return false;
            else
                return true;
        }

    }
}
