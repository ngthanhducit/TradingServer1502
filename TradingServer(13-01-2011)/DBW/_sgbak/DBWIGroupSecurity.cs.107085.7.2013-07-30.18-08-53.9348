using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWIGroupSecurity
    {
        #region Create Instance Class DBWIGroupSecurityConfig
        private static DBW.DBWIGroupSecurityConfig dbwIGroupSecurityConfig;
        private static DBW.DBWIGroupSecurityConfig DBWIGroupSecurityConfigInstance
        {
            get
            {
                if (DBWIGroupSecurity.dbwIGroupSecurityConfig == null)
                {
                    DBWIGroupSecurity.dbwIGroupSecurityConfig = new DBWIGroupSecurityConfig();
                }

                return DBWIGroupSecurity.dbwIGroupSecurityConfig;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> GetAllIGroupSecurity()
        {
            List<Business.IGroupSecurity> Result = new List<Business.IGroupSecurity>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            DSTableAdapters.IGroupSecurityConfigTableAdapter adapIGroupSecurityConfig = new DSTableAdapters.IGroupSecurityConfigTableAdapter();
            DS.IGroupSecurityDataTable tbIGroupSecurity = new DS.IGroupSecurityDataTable();
            DS.IGroupSecurityConfigDataTable tbIGroupSecurityConfig = new DS.IGroupSecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapIGroupSecurityConfig.Connection = conn;
                tbIGroupSecurity= adap.GetData();


                if (tbIGroupSecurity != null)
                {
                    int count = tbIGroupSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IGroupSecurity newIGroupSymbol = new Business.IGroupSecurity();
                        newIGroupSymbol.IGroupSecurityID = tbIGroupSecurity[i].IGroupSecurityID;
                        newIGroupSymbol.InvestorGroupID = tbIGroupSecurity[i].InvestorGroupID;
                        newIGroupSymbol.SecurityID = tbIGroupSecurity[i].SecurityID;

                        tbIGroupSecurityConfig = adapIGroupSecurityConfig.GetIGroupSecurityConfigByIGroupSecurity(tbIGroupSecurity[i].IGroupSecurityID);
                        //newIGroupSymbol.IGroupSecurityConfig = DBWIGroupSecurity.DBWIGroupSecurityConfigInstance.GetIGroupSecurityConfigByIGroupSecurity(tbIGroupSecurity[i].IGroupSecurityID);

                        if (tbIGroupSecurityConfig != null)
                        {
                            int countIGroupSecurityConfig = tbIGroupSecurityConfig.Count;
                            for (int j = 0; j < countIGroupSecurityConfig; j++)
                            {
                                Business.ParameterItem newParameter = new Business.ParameterItem();
                                newParameter.BoolValue = tbIGroupSecurityConfig[j].BoolValue;
                                newParameter.Code = tbIGroupSecurityConfig[j].Code;
                                newParameter.DateValue = tbIGroupSecurityConfig[j].DateValue;
                                newParameter.NumValue = tbIGroupSecurityConfig[j].NumValue;
                                newParameter.ParameterItemID = tbIGroupSecurityConfig[j].IGroupSecurityConfigID;
                                newParameter.SecondParameterID = tbIGroupSecurityConfig[j].IGroupSecurityID;
                                newParameter.StringValue = tbIGroupSecurityConfig[j].StringValue;

                                if (newIGroupSymbol.IGroupSecurityConfig == null)
                                    newIGroupSymbol.IGroupSecurityConfig = new List<Business.ParameterItem>();

                                newIGroupSymbol.IGroupSecurityConfig.Add(newParameter);
                            }
                        }

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
                adapIGroupSecurityConfig.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal Business.IGroupSecurity GetIGroupSecurityByID(int IGroupSecurityID)
        {
            Business.IGroupSecurity Result = new Business.IGroupSecurity();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            DS.IGroupSecurityDataTable tbIGroupSecurity = new DS.IGroupSecurityDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSecurity = adap.GetIGroupSecurityByIGroupSecurityID(IGroupSecurityID);

                if (tbIGroupSecurity != null)
                {
                    Result.IGroupSecurityID = tbIGroupSecurity[0].IGroupSecurityID;
                    Result.InvestorGroupID = tbIGroupSecurity[0].InvestorGroupID;
                    Result.SecurityID = tbIGroupSecurity[0].SecurityID;
                    Result.IGroupSecurityConfig = DBWIGroupSecurity.DBWIGroupSecurityConfigInstance.GetIGroupSecurityConfigByIGroupSecurity(tbIGroupSecurity[0].IGroupSecurityID);
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
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> GetIGroupSecurityByInvestorGroupID(int InvestorGroupID)
        {
            List<Business.IGroupSecurity> Result = new List<Business.IGroupSecurity>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            DS.IGroupSecurityDataTable tbIGroupSecurity = new DS.IGroupSecurityDataTable();
            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSecurity = adap.GetIGroupSecurityByInvestorGroupID(InvestorGroupID);

                if (tbIGroupSecurity != null)
                {
                    int count=tbIGroupSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IGroupSecurity newParameterItem = new Business.IGroupSecurity();
                        newParameterItem.IGroupSecurityID = tbIGroupSecurity[i].IGroupSecurityID;
                        newParameterItem.SecurityID = tbIGroupSecurity[i].SecurityID;
                        newParameterItem.InvestorGroupID = tbIGroupSecurity[i].InvestorGroupID;
                        newParameterItem.IGroupSecurityConfig = DBWIGroupSecurity.DBWIGroupSecurityConfigInstance.GetIGroupSecurityConfigByIGroupSecurity(tbIGroupSecurity[i].IGroupSecurityID);

                        Result.Add(newParameterItem);
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
        internal List<Business.IGroupSecurity> GetIGroupSecurityBySecurityID(int SecurityID)
        {
            List<Business.IGroupSecurity> Result = new List<Business.IGroupSecurity>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            DS.IGroupSecurityDataTable tbIGroupSecurity = new DS.IGroupSecurityDataTable();
            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSecurity = adap.GetIGroupSecurityByInvestorGroupID(SecurityID);
                if (tbIGroupSecurity != null)
                {
                    int count = tbIGroupSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IGroupSecurity newParameterItem = new Business.IGroupSecurity();
                        newParameterItem.IGroupSecurityID = tbIGroupSecurity[i].IGroupSecurityID;
                        newParameterItem.SecurityID = tbIGroupSecurity[i].SecurityID;
                        newParameterItem.InvestorGroupID = tbIGroupSecurity[i].InvestorGroupID;
                        newParameterItem.IGroupSecurityConfig = DBWIGroupSecurity.DBWIGroupSecurityConfigInstance.GetIGroupSecurityConfigByIGroupSecurity(tbIGroupSecurity[i].IGroupSecurityID);

                        Result.Add(newParameterItem);
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
        /// <param name="InvestorGroupID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal int AddIGroupSecurity(int InvestorGroupID, int SecurityID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddIGroupSecurity(InvestorGroupID, SecurityID).ToString());
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
        /// <param name="IGroupSecurityID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="InvestorGroupID"></param>
        internal bool UpdateIGroupSecurity(int IGroupSecurityID, int SecurityID, int InvestorGroupID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateIGroupSecurity(SecurityID, InvestorGroupID, IGroupSecurityID);
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
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityByIGroupSecurityID(int IGroupSecurityID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;

                int ResultDelete = adap.DeleteIGroupSecurityByIGroupSecurityID(IGroupSecurityID);
                if (ResultDelete > 0)
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
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityByInvestorGroupID(int InvestorGroupID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.DeleteIGroupSecurityByInvestorGroupID(InvestorGroupID);
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
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityBySecurityID(int SecurityID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIGroupSecurityBySecurityID(SecurityID);
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
        /// Delete by investor group id
        /// </summary>
        /// <param name="id">group id</param>
        /// <param name="connection">opened sqlconnection</param>
        /// <returns></returns>
        internal bool DFDeleteByInvestorGroupID(int id, SqlConnection connection,SqlTransaction trans)
        {
            List<int> igroupSecurityIDs = this.DFGetByInvestorGroupID(id, connection,trans);

            int count = igroupSecurityIDs.Count;
            for (int i = 0; i < count; i++)
            {
                DBWIGroupSecurityConfig dbwIGroupSecurityConfig = new DBWIGroupSecurityConfig();
                dbwIGroupSecurityConfig.DFDeleteByIGroupSecurityID(igroupSecurityIDs[i], connection,trans);
            }

            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            adap.DeleteIGroupSecurityByInvestorGroupID(id);
            return true;
        }

        /// <summary>
        /// get all by group id
        /// </summary>
        /// <param name="id">group id</param>
        /// <param name="connection">opended sql connection</param>
        /// <returns></returns>
        internal List<int> DFGetByInvestorGroupID(int id, SqlConnection connection,SqlTransaction trans)
        {
            List<int> ids = new List<int>();
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            DS.IGroupSecurityDataTable tb= adap.GetIGroupSecurityByInvestorGroupID(id);
            int count = tb.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                ids.Add(tb[i].IGroupSecurityID);
            }

            return ids;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountIGroupSecurity()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityTableAdapter adap = new DSTableAdapters.IGroupSecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountIGroupSecurity();
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

            return result.Value;
        }
    }
}
