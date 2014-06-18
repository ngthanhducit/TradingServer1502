using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWIGroupSymbolConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllIGroupSymbolConfig()
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();
            DS.IGroupSymbolConfigDataTable tbIGroupSymbolConfig = new DS.IGroupSymbolConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSymbolConfig = adap.GetData();

                if (tbIGroupSymbolConfig != null)
                {
                    int count = tbIGroupSymbolConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbIGroupSymbolConfig[i].IGroupSymbolConfigID;
                        newParameterItem.SecondParameterID = tbIGroupSymbolConfig[i].IGroupSymbolID;
                        newParameterItem.CollectionValue = new List<Business.ParameterItem>();
                        newParameterItem.Name = tbIGroupSymbolConfig[i].Name;
                        newParameterItem.Code = tbIGroupSymbolConfig[i].Code;
                        newParameterItem.BoolValue = tbIGroupSymbolConfig[i].BoolValue;
                        newParameterItem.StringValue = tbIGroupSymbolConfig[i].StringValue;
                        newParameterItem.NumValue = tbIGroupSymbolConfig[i].NumValue;
                        newParameterItem.DateValue = tbIGroupSymbolConfig[i].DateValue;

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
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetIGroupSymbolConfigByIGroupSymbolID(int IGroupSymbolID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();
            DS.IGroupSymbolConfigDataTable tbIGroupSymbolConfig = new DS.IGroupSymbolConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSymbolConfig = adap.GetIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);

                if (tbIGroupSymbolConfig != null)
                {
                    int count = tbIGroupSymbolConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbIGroupSymbolConfig[i].IGroupSymbolConfigID;
                        newParameterItem.SecondParameterID = tbIGroupSymbolConfig[i].IGroupSymbolID;
                        newParameterItem.CollectionValue = new List<Business.ParameterItem>();
                        newParameterItem.Name = tbIGroupSymbolConfig[i].Name;
                        newParameterItem.Code = tbIGroupSymbolConfig[i].Code;
                        newParameterItem.BoolValue = tbIGroupSymbolConfig[i].BoolValue;
                        newParameterItem.StringValue = tbIGroupSymbolConfig[i].StringValue;
                        newParameterItem.NumValue = tbIGroupSymbolConfig[i].NumValue;
                        newParameterItem.DateValue = tbIGroupSymbolConfig[i].DateValue;

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
        /// <param name="IGroupSymbolConfigID"></param>
        /// <returns></returns>
        internal Business.ParameterItem GetIGroupSymbolConfigByID(int IGroupSymbolConfigID)
        {
            Business.ParameterItem Result = new Business.ParameterItem();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();
            DS.IGroupSymbolConfigDataTable tbIGroupSymbolConfig = new DS.IGroupSymbolConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSymbolConfig = adap.GetIGroupSymbolConfigByID(IGroupSymbolConfigID);

                if (tbIGroupSymbolConfig != null)
                {
                    Result.ParameterItemID = tbIGroupSymbolConfig[0].IGroupSymbolConfigID;
                    Result.SecondParameterID = tbIGroupSymbolConfig[0].IGroupSymbolID;
                    Result.CollectionValue = new List<Business.ParameterItem>();
                    Result.Name = tbIGroupSymbolConfig[0].Name;
                    Result.Code = tbIGroupSymbolConfig[0].Code;
                    Result.BoolValue = tbIGroupSymbolConfig[0].BoolValue;
                    Result.StringValue = tbIGroupSymbolConfig[0].StringValue;
                    Result.NumValue = tbIGroupSymbolConfig[0].NumValue;
                    Result.DateValue = tbIGroupSymbolConfig[0].DateValue;
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
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal int AddIGroupSymbolConfig(int IGroupSymbolID, int CollectionValue, string Name, string Code, int BoolValue,
                                            string StringValue, string NumValue, DateTime DateValue)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (CollectionValue > 0)
                {
                    Result = int.Parse(adap.AddIGroupSymbolConfig(IGroupSymbolID, CollectionValue, Name, Code, BoolValue, StringValue, NumValue, DateValue).ToString());
                }
                else
                {
                    Result = int.Parse(adap.AddIGroupSymbolConfig(IGroupSymbolID, null, Name, Code, BoolValue, StringValue, NumValue, DateValue).ToString());
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
        /// <param name="IGroupSecurityConfigID"></param>
        /// <param name="IGroupSecurityID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal bool UpdateIGroupSymbolConfig(int IGroupSymbolConfigID, int IGroupSymbolID, int CollectionValue, string Name,
                                            string Code, int BoolValue, string StringValue, string NumValue, DateTime DateValue)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateIGroupSymbolConfig(IGroupSymbolID, null, Name, Code, BoolValue, StringValue, NumValue, DateValue, IGroupSymbolConfigID);
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
        /// <param name="IGroupSymbolConfigID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSymbolConfig(int IGroupSymbolConfigID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteIGroupSymbolConfig(IGroupSymbolConfigID);
                if (resultDelete > 0)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSymbolConfigByIGroupSymbolID(int IGroupSymbolID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);
                if (resultDelete > 0)
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

        internal bool DFDeleteByIGroupSymbolID(SqlConnection connection, SqlTransaction trans, int IGroupSymbolID)
        {
            DSTableAdapters.IGroupSymbolConfigTableAdapter adap = new DSTableAdapters.IGroupSymbolConfigTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            adap.DeleteIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);

            return true;
        }

    }//end class code
}
