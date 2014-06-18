using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    public class DBWTradingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllParameterItem()
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;

                tbTradingConfig = adap.GetData();
                if (tbTradingConfig != null)
                {
                    int count = tbTradingConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbTradingConfig[i].TradingConfigID;
                        newParameterItem.BoolValue = tbTradingConfig[i].BoolValue;
                        newParameterItem.Code = tbTradingConfig[i].Code;
                        newParameterItem.DateValue = tbTradingConfig[i].DateValue;
                        newParameterItem.Name = tbTradingConfig[i].Name;
                        newParameterItem.NumValue = tbTradingConfig[i].NumValue;
                        newParameterItem.StringValue = tbTradingConfig[i].StringValue;
                        newParameterItem.SecondParameterID = tbTradingConfig[i].SymbolID;

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
        /// <param name="TradingConfigID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetTradingConfigByTradingConfigID(int TradingConfigID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbTradingConfig = adap.GetTradingConfigByTradingConfigID(TradingConfigID);

                if (tbTradingConfig != null)
                {
                    int count = tbTradingConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItems = new Business.ParameterItem();
                        newParameterItems.BoolValue = tbTradingConfig[i].BoolValue;
                        newParameterItems.Code = tbTradingConfig[i].Code;
                        newParameterItems.DateValue = tbTradingConfig[i].DateValue;
                        newParameterItems.Name = tbTradingConfig[i].Name;
                        newParameterItems.NumValue = tbTradingConfig[i].NumValue;
                        newParameterItems.ParameterItemID = tbTradingConfig[i].TradingConfigID;
                        newParameterItems.StringValue = tbTradingConfig[i].StringValue;                        
                        newParameterItems.SecondParameterID = tbTradingConfig[i].SymbolID;
                        newParameterItems.CollectionValue = this.GetTradingConfigByCollectionValue(tbTradingConfig[i].TradingConfigID);

                        Result.Add(newParameterItems);
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
        /// Get Parameter Item By Symbol ID
        /// </summary>
        /// <param name="SymbolID">int SymbolID</param>
        /// <returns>List<Business.ParameterItem></returns>
        internal List<Business.ParameterItem> GetParameterItemBySymbolID(int SymbolID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;

                tbTradingConfig = adap.GetTradingConfigBySymbolID(SymbolID);

                if (tbTradingConfig != null)
                {
                    int count = tbTradingConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbTradingConfig[i].TradingConfigID;
                        newParameterItem.BoolValue = tbTradingConfig[i].BoolValue;
                        newParameterItem.Code = tbTradingConfig[i].Code;
                        newParameterItem.DateValue = tbTradingConfig[i].DateValue;
                        newParameterItem.Name = tbTradingConfig[i].Name;
                        newParameterItem.NumValue = tbTradingConfig[i].NumValue;
                        newParameterItem.NumValue = tbTradingConfig[i].NumValue;
                        newParameterItem.StringValue = tbTradingConfig[i].StringValue;                        
                        newParameterItem.SecondParameterID = tbTradingConfig[i].SymbolID;

                        //newParameterItem.CollectionValue = this.GetTradingConfigByCollectionValue(newParameterItem.ParameterItemID);

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
        /// <param name="CollectionValue"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetTradingConfigByCollectionValue(int CollectionValue)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;

                tbTradingConfig = adap.GetTradingConfigByCollectionValue(CollectionValue);
                if (tbTradingConfig != null)
                {
                    int count = tbTradingConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbTradingConfig[i].TradingConfigID;
                        newParameterItem.BoolValue = tbTradingConfig[i].BoolValue;
                        newParameterItem.Code = tbTradingConfig[i].Code;
                        newParameterItem.DateValue= tbTradingConfig[i].DateValue;
                        newParameterItem.Name = tbTradingConfig[i].Name;
                        newParameterItem.NumValue = tbTradingConfig[i].NumValue;                        
                        newParameterItem.StringValue = tbTradingConfig[i].StringValue;                       

                        newParameterItem.CollectionValue = this.GetTradingConfigByCollectionValue(newParameterItem.ParameterItemID);                        
                        
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
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal int AddNewTradingConfig(int SymbolID,int CollectionValue,string Name,string Code,int BoolValue,
                                            string StringValue,string NumValue,DateTime DateValue)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                
                Result = int.Parse(adap.AddNewTradingConfig(SymbolID, null, Code, Name, BoolValue, StringValue, NumValue, DateValue).ToString());                            
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
        /// <param name="TradingConfigID"></param>
        /// <param name="SymbolID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        internal bool UpdateTradingConfig(int TradingConfigID, int SymbolID, int CollectionValue, string Name,
                                            string Code, int BoolValue, string StringValue, string NumValue, DateTime DateValue)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumUpdate = -1;
                NumUpdate = adap.UpdateTradingConfig(SymbolID, null, Code, Name, BoolValue, StringValue, NumValue, DateValue, TradingConfigID);
                if (NumUpdate > 0)
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
        /// <param name="SymbolID"></param>
        internal void DeleteTradingConfigBySymbolID(int SymbolID)
        {
            List<Business.ParameterItem> ParameterItemsList = new List<Business.ParameterItem>();
            ParameterItemsList = this.GetParameterItemBySymbolID(SymbolID);
            while (ParameterItemsList.Count > 0)
            {
                this.DeleteTradingConfigReference(ParameterItemsList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TradingConfigID"></param>
        internal void DeleteTradingConfigByTradingConfigID(int TradingConfigID)
        {
            List<Business.ParameterItem> ParameterItemsList = new List<Business.ParameterItem>();
            ParameterItemsList = this.GetTradingConfigByTradingConfigID(TradingConfigID);
            while (ParameterItemsList.Count > 0)
            {
                this.DeleteTradingConfigReference(ParameterItemsList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameterItem"></param>
        internal void DeleteTradingConfigReference(List<Business.ParameterItem> objParameterItem)
        {
            if (objParameterItem != null)
            {
                int count = objParameterItem.Count;
                for (int i = 0; i < count; i++)
                {
                    if (objParameterItem[i].CollectionValue == null || objParameterItem[i].CollectionValue.Count == 0)
                    {
                        this.DeleteTradingConfigByTradingConfigID(objParameterItem[i].ParameterItemID);
                        objParameterItem.Remove(objParameterItem[i]);
                        return;
                    }
                    else
                    {
                        this.DeleteTradingConfigReference(objParameterItem[i].CollectionValue);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TradingConfigID"></param>
        internal bool DeleteTradingConfigByID(int TradingConfigID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteTradingConfigByTradingConfigID(TradingConfigID);

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
        /// delete funcion
        /// Delete trading config by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connect"></param>
        /// <returns></returns>
        internal bool DFDeleteByID(int id,SqlConnection connect)
        {
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();
            adap.Connection = connect;
            int effectRow= adap.DeleteTradingConfigByTradingConfigID(id);
            if (effectRow == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// delete funcion
        /// Delete trading config by id
        /// </summary>
        /// <param name="id">TradingConfigID</param>
        /// <returns></returns>
        /// 
        internal bool DFDeleteByID(int id)
        {
            bool result=false;
            
            SqlConnection connection=new SqlConnection(DBConnection.DBConnection.Connection);
            try
            {
                connection.Open();
                result = this.DFDeleteByID(id, connection);
            }
            catch (Exception ex)
            { 
                
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        internal bool DFDeleteBySymbolID(int id,SqlConnection connection,SqlTransaction trans)
        {
            DSTableAdapters.TradingConfigTableAdapter adap = new DSTableAdapters.TradingConfigTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            adap.DeleteBySymbolID(id);
            
            return true;
        }
    }
}
