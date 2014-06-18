using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWSecurityConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllSecurityConfig()
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();
            DS.SecurityConfigDataTable tbSecurityConfig = new DS.SecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbSecurityConfig = adap.GetData();

                if (tbSecurityConfig != null)
                {
                    int count = tbSecurityConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newSecurityConfig = new Business.ParameterItem();
                        newSecurityConfig.ParameterItemID = tbSecurityConfig[i].SecurityConfigID;
                        newSecurityConfig.SecondParameterID = tbSecurityConfig[i].SecurityID;
                        newSecurityConfig.Name = tbSecurityConfig[i].Name;
                        newSecurityConfig.Code = tbSecurityConfig[i].Code;
                        newSecurityConfig.NumValue = tbSecurityConfig[i].NumValue;
                        newSecurityConfig.StringValue = tbSecurityConfig[i].StringValue;
                        newSecurityConfig.DateValue = tbSecurityConfig[i].DateValue;
                        newSecurityConfig.BoolValue = tbSecurityConfig[i].BoolValue;
                        newSecurityConfig.CollectionValue = this.GetSecurityConfigByCollectionValue(tbSecurityConfig[i].SecurityConfigID, adap);
                        Result.Add(newSecurityConfig);
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
        internal List<Business.ParameterItem> GetSecurityConfigBySecurityID(int SecurityID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();
            DS.SecurityConfigDataTable tbSecurityConfig = new DS.SecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbSecurityConfig = adap.GetSecurityConfigBySecurityID(SecurityID);

                if (tbSecurityConfig != null)
                {
                    int count = tbSecurityConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbSecurityConfig[i].SecurityConfigID;
                        newParameterItem.SecondParameterID = tbSecurityConfig[i].SecurityID;
                        newParameterItem.Name = tbSecurityConfig[i].Name;
                        newParameterItem.Code = tbSecurityConfig[i].Code;
                        newParameterItem.NumValue = tbSecurityConfig[i].NumValue;
                        newParameterItem.StringValue = tbSecurityConfig[i].StringValue;
                        newParameterItem.DateValue = tbSecurityConfig[i].DateValue;
                        newParameterItem.BoolValue = tbSecurityConfig[i].BoolValue;
                        //newParameterItem.CollectionValue = this.GetSecurityConfigByCollectionValue(tbSecurityConfig[i].SecurityConfigID, adap);
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
        /// <param name="SecurityConfigID"></param>
        /// <returns></returns>
        internal Business.ParameterItem GetSecurityConfigBySecurityConfigID(int SecurityConfigID)
        {
            Business.ParameterItem result = new Business.ParameterItem();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();
            DS.SecurityConfigDataTable tbSecurityConfig = new DS.SecurityConfigDataTable();
            try
            {
                conn.Open();
                adap.Connection = conn;
                tbSecurityConfig = adap.GetSecurityConfigBySecurityConfigID(SecurityConfigID);
                if (tbSecurityConfig != null)
                {
                    result.ParameterItemID = tbSecurityConfig[0].SecurityConfigID;
                    result.SecondParameterID = tbSecurityConfig[0].SecurityID;
                    result.Name = tbSecurityConfig[0].Name;
                    result.Code = tbSecurityConfig[0].Code;
                    result.NumValue = tbSecurityConfig[0].NumValue;
                    result.StringValue = tbSecurityConfig[0].StringValue;
                    result.DateValue = tbSecurityConfig[0].DateValue;
                    result.BoolValue = tbSecurityConfig[0].BoolValue;
                    result.CollectionValue = this.GetSecurityConfigByCollectionValue(tbSecurityConfig[0].SecurityConfigID, adap);
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CollectionValue"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetSecurityConfigByCollectionValue(int CollectionValue, DSTableAdapters.SecurityConfigTableAdapter adap)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            DS.SecurityConfigDataTable tbSecurityConfig = new DS.SecurityConfigDataTable();
            try
            {
                tbSecurityConfig = adap.GetSecurityConfigByCollectionValue(CollectionValue);
                if (tbSecurityConfig != null)
                {
                    int count = tbSecurityConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbSecurityConfig[i].SecurityConfigID;
                        newParameterItem.SecondParameterID = tbSecurityConfig[i].SecurityID;
                        newParameterItem.Name = tbSecurityConfig[i].Name;
                        newParameterItem.Code = tbSecurityConfig[i].Code;
                        newParameterItem.NumValue = tbSecurityConfig[i].NumValue;
                        newParameterItem.StringValue = tbSecurityConfig[i].StringValue;
                        newParameterItem.DateValue = tbSecurityConfig[i].DateValue;
                        newParameterItem.BoolValue = tbSecurityConfig[i].BoolValue;
                        newParameterItem.CollectionValue = this.GetSecurityConfigByCollectionValue(tbSecurityConfig[i].SecurityConfigID, adap);
                        Result.Add(newParameterItem);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID">int SecurityID</param>
        /// <param name="CollectionValue">int CollectionValue</param>
        /// <param name="Name">string Name</param>
        /// <param name="Code">string Code</param>
        /// <param name="NumValue">string NumValue</param>
        /// <param name="StringValue">string StringValue</param>
        /// <param name="DateValue">DateTime DateValue</param>
        /// <param name="BoolValue">int BoolValue</param>
        /// <returns></returns>
        internal int AddSecurityConfig(int SecurityID, int CollectionValue, string Name, string Code, string NumValue, string StringValue, DateTime DateValue,int BoolValue)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                if (CollectionValue <= 0)
                {
                    Result = int.Parse(adap.AddSecurityConfig(SecurityID, null, Name, Code, NumValue, StringValue, DateValue, BoolValue).ToString());
                }else
                Result = int.Parse(adap.AddSecurityConfig(SecurityID, CollectionValue, Name, Code, NumValue, StringValue, DateValue, BoolValue).ToString());
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
        /// <param name="SecurityConfigID">int SecurityConfigID</param>
        /// <param name="SecurityID">int SecurityID</param>
        /// <param name="CollectionValue">int CollectionValue</param>
        /// <param name="Name">string Name</param>
        /// <param name="Code">string Code</param>
        /// <param name="NumValue">string NumValue</param>
        /// <param name="StringValue">string StringValue</param>
        /// <param name="DateValue">DateTime DateValue</param>
        /// <param name="BoolValue">int BoolValue</param>
        internal bool UpdateSecurityConfig(int SecurityConfigID, int SecurityID, int CollectionValue, string Name, string Code, string NumValue, string StringValue, DateTime DateValue, int BoolValue)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                if (CollectionValue <= 0) adap.UpdateSecurityConfig(SecurityID, null, Name, Code, NumValue, StringValue, DateValue, BoolValue, SecurityConfigID);
                else adap.UpdateSecurityConfig(SecurityID, CollectionValue, Name, Code, NumValue, StringValue, DateValue, BoolValue, SecurityConfigID);
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
        /// <param name="SecurityConfigID"></param>
        internal bool DeleteSecurityConfigBySecurityConfigID(int SecurityConfigID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteSecurityConfigBySecurityConfigID(SecurityConfigID);
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
        internal bool DeleteSecurityConfigBySecurityID(int SecurityID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteSecurityConfigBySecurityID(SecurityID);
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
        /// delete Security config by security id
        /// </summary>
        /// <param name="securityID">security id</param>
        /// <param name="connection">sql connection</param>
        /// <returns></returns>
        internal bool DFDeleteBySecurityID(int securityID, SqlConnection connection,SqlTransaction trans)
        {
            DSTableAdapters.SecurityConfigTableAdapter adap = new DSTableAdapters.SecurityConfigTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            int affectRow= adap.DeleteSecurityConfigBySecurityID(securityID);
            if (affectRow == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
