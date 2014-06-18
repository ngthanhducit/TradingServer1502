using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWIGroupSymbol
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IGroupSymbol> GetAllIGroupSymbol()
        {
            List<Business.IGroupSymbol> Result = new List<Business.IGroupSymbol>();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            DSTableAdapters.IGroupSymbolConfigTableAdapter adapIGroupSymbolConfig = new DSTableAdapters.IGroupSymbolConfigTableAdapter();
            DS.IGroupSymbolDataTable tbIGroupSymbol = new DS.IGroupSymbolDataTable();
            DS.IGroupSymbolConfigDataTable tbIGroupSymbolConfig = new DS.IGroupSymbolConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapIGroupSymbolConfig.Connection = conn;
                tbIGroupSymbol = adap.GetData();

                if (tbIGroupSymbol != null)
                {
                    int count = tbIGroupSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IGroupSymbol newIGroupSymbol = new Business.IGroupSymbol();
                        newIGroupSymbol.IGroupSymbolID = tbIGroupSymbol[i].IGroupSymbolID;                        
                        newIGroupSymbol.SymbolID = tbIGroupSymbol[i].SymbolID;                        
                        newIGroupSymbol.InvestorGroupID = tbIGroupSymbol[i].InvestorGroupID;

                        tbIGroupSymbolConfig = adapIGroupSymbolConfig.GetIGroupSymbolConfigByIGroupSymbolID(tbIGroupSymbol[i].IGroupSymbolID);

                        if (tbIGroupSymbolConfig != null)
                        {
                            int countIGroupSymbolConfig = tbIGroupSymbolConfig.Count;
                            for (int j = 0; j < countIGroupSymbolConfig; j++)
                            {
                                Business.ParameterItem newParameterItem = new Business.ParameterItem();
                                newParameterItem.ParameterItemID = tbIGroupSymbolConfig[j].IGroupSymbolConfigID;
                                newParameterItem.SecondParameterID = tbIGroupSymbolConfig[j].IGroupSymbolID;
                                newParameterItem.CollectionValue = new List<Business.ParameterItem>();
                                newParameterItem.Name = tbIGroupSymbolConfig[j].Name;
                                newParameterItem.Code = tbIGroupSymbolConfig[j].Code;
                                newParameterItem.BoolValue = tbIGroupSymbolConfig[j].BoolValue;
                                newParameterItem.StringValue = tbIGroupSymbolConfig[j].StringValue;
                                newParameterItem.NumValue = tbIGroupSymbolConfig[j].NumValue;
                                newParameterItem.DateValue = tbIGroupSymbolConfig[j].DateValue;

                                if (newIGroupSymbol.IGroupSymbolConfig == null)
                                    newIGroupSymbol.IGroupSymbolConfig = new List<Business.ParameterItem>();
                                                                
                                newIGroupSymbol.IGroupSymbolConfig.Add(newParameterItem);
                            }
                        }

                        //newIGroupSymbol.IGroupSymbolConfig = TradingServer.Facade.FacadeGetIGroupSymbolConfigByIGroupSymbolID(tbIGroupSymbol[i].IGroupSymbolID);

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
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        internal Business.IGroupSymbol GetIGroupSymbolByIGroupSymbolID(int IGroupSymbolID)
        {
            Business.IGroupSymbol Result = new Business.IGroupSymbol();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();
            DS.IGroupSymbolDataTable tbIGroupSymbol = new DS.IGroupSymbolDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSymbol = adap.GetIGroupSymbolByIGroupSymbolID(IGroupSymbolID);

                if (tbIGroupSymbol != null)
                {
                    Result.IGroupSymbolID = tbIGroupSymbol[0].IGroupSymbolID;                    
                    Result.InvestorGroupID = tbIGroupSymbol[0].InvestorGroupID;                    
                    Result.SymbolID = tbIGroupSymbol[0].SymbolID;
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
        /// <returns></returns>
        internal List<Business.IGroupSymbol> GetIGroupSymbolBySymbolID(int SymbolID)
        {
            List<Business.IGroupSymbol> Result = new List<Business.IGroupSymbol>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();
            DS.IGroupSymbolDataTable tbIGroupSymbol = new DS.IGroupSymbolDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSymbol = adap.GetIGroupSymbolByIGroupSymbolID(SymbolID);

                if (tbIGroupSymbol != null)
                {
                    int count = tbIGroupSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IGroupSymbol newIGroupSymbol = new Business.IGroupSymbol();
                        newIGroupSymbol.IGroupSymbolID = tbIGroupSymbol[i].IGroupSymbolID;                        
                        newIGroupSymbol.InvestorGroupID = tbIGroupSymbol[i].InvestorGroupID;                        
                        newIGroupSymbol.SymbolID = tbIGroupSymbol[i].SymbolID;

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
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSymbol> GetIGroupSymbolByInvestorGroupID(int InvestorGroupID)
        {
            List<Business.IGroupSymbol> Result = new List<Business.IGroupSymbol>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();
            DS.IGroupSymbolDataTable tbIGroupSymbol = new DS.IGroupSymbolDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbIGroupSymbol = adap.GetIGroupSymbolByInvestorGroupID(InvestorGroupID);

                if (tbIGroupSymbol != null)
                {
                    int count = tbIGroupSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.IGroupSymbol newIGroupSymbol = new Business.IGroupSymbol();
                        newIGroupSymbol.IGroupSymbolID = tbIGroupSymbol[i].IGroupSymbolID;                        
                        newIGroupSymbol.InvestorGroupID = tbIGroupSymbol[i].InvestorGroupID;                        
                        newIGroupSymbol.SymbolID = tbIGroupSymbol[i].SymbolID;

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
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <param name="SymbolID"></param>
        /// <param name="InvestorGroupID"></param>
        internal bool UpdateIGroupSymbol(int IGroupSymbolID, int SymbolID, int InvestorGroupID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateIGroupSymbol(SymbolID, InvestorGroupID, IGroupSymbolID);
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
        /// <param name="IGroupSymbolID"></param>
        internal bool DeleteIGroupSymbolByIGroupSymbolID(int IGroupSymbolID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteIGroupSymbolByIGroupSymbolID(IGroupSymbolID);
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
        /// <param name="SymbolID"></param>
        internal bool DeleteIGroupSymbolBySymbolID(int SymbolID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteIGroupSymbolBySymbolID(SymbolID);
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
        /// <param name="InvestorGroupID"></param>
        internal bool DeleteIGroupSymbolByInvestorGroupID(int InvestorGroupID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteIGroupSymbolByInvestorGroupID(InvestorGroupID);
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
        /// <param name="SymbolID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal int AddNewIGroupSymbol(int SymbolID, int InvestorGroupID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewIGroupSymbol(SymbolID, InvestorGroupID).ToString());
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
        /// delete IGroudpSymbol
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="trans"></param>
        /// <param name="SymnbolID"></param>
        /// <returns></returns>
        internal bool DFDeleteBySymbolID(SqlConnection connection,SqlTransaction trans,int symnbolID)
        {
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            int? id = symnbolID;
            DS.IGroupSymbolDataTable tb = adap.GetIGroupSymbolBySymbolID(id);
            int count = tb.Rows.Count;
            DBWIGroupSymbolConfig iGroupSymbolConfig = new DBWIGroupSymbolConfig();
            for (int i = 0; i < count; i++)
            {
                iGroupSymbolConfig.DFDeleteByIGroupSymbolID(connection, trans, tb[i].IGroupSymbolID);
            }

            adap.DeleteIGroupSymbolBySymbolID(symnbolID);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountIGroupSymbol()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IGroupSymbolTableAdapter adap = new DSTableAdapters.IGroupSymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountIGroupSymbol();
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
    }//end class code
}
