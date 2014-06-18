using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWInvestorGroupConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllInvestorGroupConfig()
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();
            DS.InvestorGroupConfigDataTable tbInvestorGroupConfig = new DS.InvestorGroupConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorGroupConfig = adap.GetData();

                if (tbInvestorGroupConfig != null)
                {
                    int count = tbInvestorGroupConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbInvestorGroupConfig[i].InvestorGroupConfigID;
                        newParameterItem.SecondParameterID = tbInvestorGroupConfig[i].InvestorGroupID;
                        newParameterItem.CollectionValue = new List<Business.ParameterItem>();
                        newParameterItem.Name = tbInvestorGroupConfig[i].Name;
                        newParameterItem.Code = tbInvestorGroupConfig[i].Code;
                        newParameterItem.BoolValue = tbInvestorGroupConfig[i].BoolValue;
                        newParameterItem.StringValue = tbInvestorGroupConfig[i].StringValue;
                        newParameterItem.NumValue = tbInvestorGroupConfig[i].NumValue;
                        newParameterItem.DateValue = tbInvestorGroupConfig[i].DateValue;

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
        /// Get Investor Group COnfig By Investor Group ID
        /// </summary>
        /// <param name="InvestorGroupID">int InvestorGroupID</param>
        /// <returns>List<Business.ParameterItem></returns>
        internal List<Business.ParameterItem> GetInvestorGroupConfigByInvestorGroupID(int InvestorGroupID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();
            DS.InvestorGroupConfigDataTable tbInvestorGroupConfig = new DS.InvestorGroupConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorGroupConfig = adap.GetInvestorGroupConfigByInvestorGroupID(InvestorGroupID);

                if (tbInvestorGroupConfig != null)
                {
                    int count=tbInvestorGroupConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItem = new Business.ParameterItem();
                        newParameterItem.ParameterItemID = tbInvestorGroupConfig[i].InvestorGroupConfigID;                        
                        newParameterItem.SecondParameterID = tbInvestorGroupConfig[i].InvestorGroupID;
                        newParameterItem.Code = tbInvestorGroupConfig[i].Code;
                        newParameterItem.Name = tbInvestorGroupConfig[i].Name;
                        newParameterItem.BoolValue = tbInvestorGroupConfig[i].BoolValue;
                        newParameterItem.StringValue = tbInvestorGroupConfig[i].StringValue;
                        newParameterItem.NumValue = tbInvestorGroupConfig[i].NumValue;
                        newParameterItem.DateValue = tbInvestorGroupConfig[i].DateValue;

                        newParameterItem.CollectionValue = this.GetInvestorGroupConfigByCollectionValue(tbInvestorGroupConfig[i].InvestorGroupConfigID);

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
        /// Get Investor Group Config By Investor Group Config ID
        /// Using List Result Call Function Delete In Table InvestorGroupConfig
        /// </summary>
        /// <param name="InvestorGroupConfigID">int InvestorGroupConfigID</param>
        /// <returns>List<Business.ParameterItem></returns>
        internal List<Business.ParameterItem> GetInvestorGroupConfigByID(int InvestorGroupConfigID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();
            DS.InvestorGroupConfigDataTable tbInvestorGroupConfig = new DS.InvestorGroupConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorGroupConfig = adap.GetInvestorGroupConfigByID(InvestorGroupConfigID);

                if (tbInvestorGroupConfig != null)
                {
                    Business.ParameterItem newParameterItem = new Business.ParameterItem();
                    newParameterItem.BoolValue = tbInvestorGroupConfig[0].BoolValue;
                    newParameterItem.Code = tbInvestorGroupConfig[0].Code;
                    newParameterItem.DateValue = tbInvestorGroupConfig[0].DateValue;                    
                    newParameterItem.SecondParameterID = tbInvestorGroupConfig[0].InvestorGroupID;
                    newParameterItem.Name = tbInvestorGroupConfig[0].Name;
                    newParameterItem.NumValue = tbInvestorGroupConfig[0].NumValue;
                    newParameterItem.ParameterItemID = tbInvestorGroupConfig[0].InvestorGroupConfigID;
                    newParameterItem.StringValue = tbInvestorGroupConfig[0].StringValue;
                    newParameterItem.CollectionValue = this.GetInvestorGroupConfigByCollectionValue(tbInvestorGroupConfig[0].InvestorGroupConfigID);

                    Result.Add(newParameterItem);
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
        /// Get Investor Group Config By Collection Value
        /// </summary>
        /// <param name="CollectionValueID">int CollectionValueID</param>
        /// <returns>List<Business.ParameterItem</returns>
        internal List<Business.ParameterItem> GetInvestorGroupConfigByCollectionValue(int CollectionValueID)
        {
            List<Business.ParameterItem> Result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();
            DS.InvestorGroupConfigDataTable tbInvestorGroupConfig = new DS.InvestorGroupConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorGroupConfig = adap.GetInvestorGroupConfigByCollectionValue(CollectionValueID);

                if (tbInvestorGroupConfig != null)
                {
                    int count = tbInvestorGroupConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ParameterItem newParameterItems = new Business.ParameterItem();
                        newParameterItems.BoolValue = tbInvestorGroupConfig[i].BoolValue;
                        newParameterItems.Code = tbInvestorGroupConfig[i].Code;
                        newParameterItems.DateValue = tbInvestorGroupConfig[i].DateValue;                        
                        newParameterItems.SecondParameterID = tbInvestorGroupConfig[i].InvestorGroupID;
                        newParameterItems.Name = tbInvestorGroupConfig[i].Name;
                        newParameterItems.NumValue = tbInvestorGroupConfig[i].NumValue;
                        newParameterItems.ParameterItemID = tbInvestorGroupConfig[i].InvestorGroupConfigID;
                        newParameterItems.StringValue = tbInvestorGroupConfig[i].StringValue;
                        newParameterItems.CollectionValue = this.GetInvestorGroupConfigByCollectionValue(tbInvestorGroupConfig[i].InvestorGroupConfigID);

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
        /// Add New Investor Group Config
        /// </summary>
        /// <param name="InvestorGroupID">int InvestorGroupID</param>
        /// <param name="CollectionValue">int CollectionValue</param>
        /// <param name="Name">string Name</param>
        /// <param name="Code">string Code</param>
        /// <param name="BoolValue">int BoolValue</param>
        /// <param name="StringValue">string StringValue</param>
        /// <param name="NumValue">string NumValue</param>
        /// <param name="DateValue">DateTime DateValue</param>
        /// <returns>int</returns>
        internal int AddNewInvestorGroupConfig(int InvestorGroupID, int CollectionValue, string Name, string Code, int BoolValue,
                                                string StringValue,string NumValue,DateTime DateValue)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                Result = int.Parse(adap.AddNewInvestorGroupConfig(InvestorGroupID, null, Name, Code, BoolValue, StringValue, NumValue, DateValue).ToString());
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
        /// Update Investor Group Config By Investor Group Config ID
        /// </summary>
        /// <param name="InvestorGroupConfigID">int InvestorGroupConfigID</param>
        /// <param name="InvestorGroupID">int InvestorGroupID</param>
        /// <param name="CollectionValue">int CollectionValue</param>
        /// <param name="Name">string Name</param>
        /// <param name="Code">string Code</param>
        /// <param name="BoolValue">int BoolValue</param>
        /// <param name="StringValue">string StringValue</param>
        /// <param name="NumValue">string NumValue</param>
        /// <param name="DateValue">DateTime DateValue</param>
        internal bool UpdateInvestorGroupConfig(int InvestorGroupConfigID, int InvestorGroupID, int CollectionValue, string Name,
                                                string Code, int BoolValue, string StringValue, string NumValue, DateTime DateValue)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.UpdateInvestorGroupConfig(InvestorGroupID, null, Name, Code, BoolValue, StringValue, NumValue, DateValue, InvestorGroupConfigID);
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
        /// Delete Invesor Group Config By Investor Group Config ID
        /// Using Delete In Talbe InvestorGroupConfig With ID
        /// </summary>
        /// <param name="InvestorGroupConfigID">int Investor GroupConfigID</param>
        internal bool DeleteInvestorGroupConfig(int InvestorGroupConfigID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.DeleteInvestorGroupConfig(InvestorGroupConfigID);
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
        /// Delete Investor Group Config By Investor Group Config ID
        /// Using Get All Data Reference To InvestorGroupConfigID
        /// And Call Funtion Delete Reference
        /// </summary>
        /// <param name="InvestorGroupConfigID">int InvestorGroupConfigID</param>
        internal void DeleteInvestorGroupConfigByInvestorGroupConfigID(int InvestorGroupConfigID) 
        {
            List<Business.ParameterItem> ParameterItemsList = new List<Business.ParameterItem>();
            ParameterItemsList = this.GetInvestorGroupConfigByID(InvestorGroupConfigID);

            while (ParameterItemsList.Count > 0)
            {
                this.DeleteInvestorGroupConfigReference(ParameterItemsList);
            }
        }

        /// <summary>
        /// Delete Investor Group Config By Investor Group ID
        /// Using Get All Data Reference To Investor Group ID
        /// And Call Function Delete Reference
        /// </summary>
        /// <param name="InvestorGroupID">int InvestorGroupID</param>
        internal void DeleteInvestorGroupConfigByInvestorGroupID(int InvestorGroupID)
        {
            List<Business.ParameterItem> ParameterItemsList = new List<Business.ParameterItem>();
            ParameterItemsList = this.GetInvestorGroupConfigByInvestorGroupID(InvestorGroupID);

            while (ParameterItemsList.Count > 0)
            {
                this.DeleteInvestorGroupConfigReference(ParameterItemsList);
            }
        }

        /// <summary>
        /// Delete Investor Group Config Reference
        /// </summary>
        /// <param name="objParameterItem">List<Business.ParameterItem></param>
        internal void DeleteInvestorGroupConfigReference(List<Business.ParameterItem> objParameterItem)
        {
            if (objParameterItem != null)
            {
                int count = objParameterItem.Count;
                for (int i = 0; i < count; i++)
                {
                    if (objParameterItem[i].CollectionValue.Count == 0 || objParameterItem[i].CollectionValue == null)
                    {
                        this.DeleteInvestorGroupConfig(objParameterItem[i].ParameterItemID);
                        objParameterItem.Remove(objParameterItem[i]);
                        return;
                    }
                    else
                    {
                        this.DeleteInvestorGroupConfigReference(objParameterItem[i].CollectionValue);
                    }
                }
            }
        }
        /// <summary>
        /// delete InvestorGroupConfig by investorGroupID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection">opened sqlconnection</param>
        /// <returns></returns>
        internal bool DFDeleteByGroupID(int id,SqlConnection connection,SqlTransaction trans)
        {
            DSTableAdapters.InvestorGroupConfigTableAdapter adap = new DSTableAdapters.InvestorGroupConfigTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            adap.DeleteInvestorGroupConfigByInvestorGroupID(id);

            return true;
        }
    }
}
