using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWIGroupSecurityConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllIGroupSecurityConfig()
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();
            DS.IGroupSecurityConfigDataTable tbIGroupSecurityConfig = new DS.IGroupSecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSecurityConfig = adap.GetData();

                if (tbIGroupSecurityConfig != null)
                {
                    int count = tbIGroupSecurityConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameter = new Business.ParameterItem();
                        newParameter.ParameterItemID = tbIGroupSecurityConfig[i].IGroupSecurityConfigID;
                        newParameter.SecondParameterID = tbIGroupSecurityConfig[i].IGroupSecurityID;
                        newParameter.Name = tbIGroupSecurityConfig[i].Name;
                        newParameter.Code = tbIGroupSecurityConfig[i].Code;
                        newParameter.BoolValue = tbIGroupSecurityConfig[i].BoolValue;
                        newParameter.StringValue = tbIGroupSecurityConfig[i].StringValue;
                        newParameter.NumValue = tbIGroupSecurityConfig[i].NumValue;
                        newParameter.DateValue = tbIGroupSecurityConfig[i].DateValue;

                        Result.Add(newParameter);
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
        /// <param name="IGroupSecurityConfigID"></param>
        /// <returns></returns>
        internal Business.ParameterItem GetIGroupSecurityConfigByID(int IGroupSecurityConfigID)
        {
            Business.ParameterItem Result = new Business.ParameterItem();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();
            DS.IGroupSecurityConfigDataTable tbIGroupSecurityConfig = new DS.IGroupSecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSecurityConfig = adap.GetIGroupSecurityConfigByID(IGroupSecurityConfigID);

                if (tbIGroupSecurityConfig != null)
                {
                    Result.ParameterItemID = tbIGroupSecurityConfig[0].IGroupSecurityConfigID;
                    Result.SecondParameterID = tbIGroupSecurityConfig[0].IGroupSecurityID;
                    Result.Name = tbIGroupSecurityConfig[0].Name;
                    Result.Code = tbIGroupSecurityConfig[0].Code;
                    Result.BoolValue = tbIGroupSecurityConfig[0].BoolValue;
                    Result.StringValue = tbIGroupSecurityConfig[0].StringValue;
                    Result.NumValue = tbIGroupSecurityConfig[0].NumValue;
                    Result.DateValue = tbIGroupSecurityConfig[0].DateValue;
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
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetIGroupSecurityConfigByIGroupSecurity(int IGroupSecurityID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();
            DS.IGroupSecurityConfigDataTable tbIGroupSecurityConfig = new DS.IGroupSecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSecurityConfig = adap.GetIGroupSecurityConfigByIGroupSecurity(IGroupSecurityID);

                if (tbIGroupSecurityConfig != null)
                {
                    int count = tbIGroupSecurityConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbIGroupSecurityConfig[i].IGroupSecurityConfigID;
                        newParameterItem.SecondParameterID = tbIGroupSecurityConfig[i].IGroupSecurityID;
                        newParameterItem.Name = tbIGroupSecurityConfig[i].Name;
                        newParameterItem.Code = tbIGroupSecurityConfig[i].Code;
                        newParameterItem.BoolValue = tbIGroupSecurityConfig[i].BoolValue;
                        newParameterItem.StringValue = tbIGroupSecurityConfig[i].StringValue;
                        newParameterItem.NumValue = tbIGroupSecurityConfig[i].NumValue;
                        newParameterItem.DateValue = tbIGroupSecurityConfig[i].DateValue;

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
        /// <param name="IGroupSecurityConfigID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityConfigByID(int IGroupSecurityConfigID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIGroupSecurityConfigByID(IGroupSecurityConfigID);
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
        internal bool DeleteIGroupSecurityConfigByIGroupSecurityID(int IGroupSecurityID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int ResultDelete = adap.DeleteIGroupSecurityConfigByIGroupSecurity(IGroupSecurityID);
                if (ResultDelete > 0)
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
        /// <param name="objParameter"></param>
        /// <returns></returns>
        internal bool UpdateIGroupSecurityConfig(int IGroupSecurityConfigID, int IGroupSecurityID, int CollectionValue, string Name,
                                            string Code, int BoolValue, string StringValue, string NumValue, DateTime DateValue)
        {
            bool Result = false;
            int NumUpdate = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                NumUpdate = adap.UpdateIGroupSecurityConfig(IGroupSecurityID, null, Name, Code, BoolValue, StringValue, NumValue, DateValue, IGroupSecurityConfigID);
                if (NumUpdate > 0)
                {
                    Result = true;
                }
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
        /// <param name="SymbolID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal int AddIGroupSecurityConfig(int IGroupSecurityID, int CollectionValue, string Name, string Code, int BoolValue,
                                            string StringValue, string NumValue, DateTime DateValue)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (CollectionValue > 0)
                {
                    Result = int.Parse(adap.AddIGroupSecurityConfig(IGroupSecurityID, CollectionValue, Name, Code, BoolValue, StringValue, NumValue, DateValue).ToString());
                }
                else
                {
                    Result = int.Parse(adap.AddIGroupSecurityConfig(IGroupSecurityID, null, Name, Code, BoolValue, StringValue, NumValue, DateValue).ToString());
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
        /// <param name="iGroupSecurityID"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal bool DFDeleteByIGroupSecurityID(int iGroupSecurityID, SqlConnection connection,SqlTransaction trans)
        {
            DSTableAdapters.IGroupSecurityConfigTableAdapter adap = new DSTableAdapters.IGroupSecurityConfigTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            adap.DeleteIGroupSecurityConfigByIGroupSecurity(iGroupSecurityID);
            return true;
        }

    }
}
