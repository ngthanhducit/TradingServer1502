using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWMarketAreaConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllMarketAreaConfig()
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();
            DS.MarketAreaConfigDataTable tbMarketAreaConfig = new DS.MarketAreaConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbMarketAreaConfig = adap.GetData();

                if (tbMarketAreaConfig != null)
                {
                    int count = tbMarketAreaConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbMarketAreaConfig[i].MarketAreaConfigID;
                        newParameterItem.SecondParameterID = tbMarketAreaConfig[i].MarketAreaID;
                        newParameterItem.Code = tbMarketAreaConfig[i].Code;
                        newParameterItem.Name = tbMarketAreaConfig[i].Name;
                        newParameterItem.BoolValue = tbMarketAreaConfig[i].BoolValue;
                        newParameterItem.StringValue = tbMarketAreaConfig[i].StringValue;
                        newParameterItem.NumValue = tbMarketAreaConfig[i].NumValue;
                        newParameterItem.DateValue = tbMarketAreaConfig[i].DateValue;

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
        /// <param name="MarketAreaConfigID"></param>
        /// <returns></returns>
        internal Business.ParameterItem GetMarketAreaConfigByID(int MarketAreaConfigID)
        {
            Business.ParameterItem Result = new Business.ParameterItem();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();
            DS.MarketAreaConfigDataTable tbMarketAreaConfig = new DS.MarketAreaConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbMarketAreaConfig = adap.GetMarketAreaConfigByID(MarketAreaConfigID);

                if (tbMarketAreaConfig != null)
                {
                    Result.ParameterItemID = tbMarketAreaConfig[0].MarketAreaConfigID;
                    Result.SecondParameterID = tbMarketAreaConfig[0].MarketAreaID;
                    Result.Code = tbMarketAreaConfig[0].Code;
                    Result.Name = tbMarketAreaConfig[0].Name;
                    Result.BoolValue = tbMarketAreaConfig[0].BoolValue;
                    Result.StringValue = tbMarketAreaConfig[0].StringValue;
                    Result.NumValue = tbMarketAreaConfig[0].NumValue;
                    Result.DateValue = tbMarketAreaConfig[0].DateValue;
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
        /// <param name="MarketAreaID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetMarketAreaConfigByMarketAreaID(int MarketAreaID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();
            DS.MarketAreaConfigDataTable tbMarketAreaConfig = new DS.MarketAreaConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbMarketAreaConfig = adap.GetMarketAreaConfigByMarketAreaID(MarketAreaID);

                if (tbMarketAreaConfig != null)
                {
                    int count = tbMarketAreaConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbMarketAreaConfig[i].MarketAreaConfigID;
                        newParameterItem.SecondParameterID = tbMarketAreaConfig[i].MarketAreaID;
                        newParameterItem.Code = tbMarketAreaConfig[i].Code;
                        newParameterItem.Name = tbMarketAreaConfig[i].Name;
                        newParameterItem.BoolValue = tbMarketAreaConfig[i].BoolValue;
                        newParameterItem.StringValue = tbMarketAreaConfig[i].StringValue;
                        newParameterItem.NumValue = tbMarketAreaConfig[i].NumValue;
                        newParameterItem.DateValue = tbMarketAreaConfig[i].DateValue;

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
        /// <param name="SymbolID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        internal int AddNewMarketArea(int MarketAreaID, int CollectionValue, string Name, string Code, int BoolValue,
                                            string StringValue, string NumValue, DateTime DateValue)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();
            DS.MarketAreaConfigDataTable tbMarketAreaConfig = new DS.MarketAreaConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewMarketAreaConfig(MarketAreaID, Code, CollectionValue, StringValue, NumValue, BoolValue, DateValue, Name).ToString());
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
        /// <param name="MarketAreaConfigID"></param>
        /// <returns></returns>
        internal bool DeleteMarketAreaConfig(int MarketAreaConfigID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteMarketAreaConfigByID(MarketAreaConfigID);
            }
            catch (Exception ex)
            {
                Result = true;
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
        /// <param name="MarketAreaID"></param>
        /// <returns></returns>
        internal bool DeleteMarketAreaConfigByMarketAreaID(int MarketAreaID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteMarketAreaConfigByMarketAreaID(MarketAreaID);
            }
            catch (Exception ex)
            {
                Result = true;
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
        internal void UpdateMarketAreaConfig(int MarketAreaConfigID,int MarketAreaID, int SymbolID, int CollectionValue, string Name,
                                            string Code, int BoolValue, string StringValue, string NumValue, DateTime DateValue)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaConfigTableAdapter adap = new DSTableAdapters.MarketAreaConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateMarketAreaConfig(MarketAreaID, Code, CollectionValue, StringValue, NumValue, BoolValue, DateValue, Name, MarketAreaConfigID);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
        }
    }
}
