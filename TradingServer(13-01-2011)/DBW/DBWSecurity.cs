using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWSecurity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Security> GetAllSecurity()
        {
            List<Business.Security> Result = new List<Business.Security>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();
            DSTableAdapters.SecurityConfigTableAdapter adapSecurityConfig = new DSTableAdapters.SecurityConfigTableAdapter();

            DSTableAdapters.SymbolTableAdapter adapSymbol = new DSTableAdapters.SymbolTableAdapter();
            DSTableAdapters.TradingConfigTableAdapter adapTradingConfig = new DSTableAdapters.TradingConfigTableAdapter();

            DS.SymbolDataTable tbSymbol = new DS.SymbolDataTable();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();
            DS.SecurityDataTable tbSecurity = new DS.SecurityDataTable();
            DS.SecurityConfigDataTable tbSecurityConfig = new DS.SecurityConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapSecurityConfig.Connection = conn;
                adapSymbol.Connection = conn;
                adapTradingConfig.Connection = conn;

                tbSecurity = adap.GetData();

                if (tbSecurity != null)
                {
                    int count = tbSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Security newSecurity = new Business.Security();
                        newSecurity.ParameterItems = new List<Business.ParameterItem>();
                        newSecurity.SymbolGroup = new List<Business.Symbol>();

                        newSecurity.SecurityID = tbSecurity[i].SecurityID;
                        newSecurity.Name = tbSecurity[i].Name;
                        newSecurity.Description = tbSecurity[i].Description;
                        newSecurity.MarketAreaID = tbSecurity[i].MarketAreaID;

                        #region GET SECURITY CONFIG BY SECURITY ID
                        //GET SECURITY CONFIG BY SECURITY ID
                        tbSecurityConfig = adapSecurityConfig.GetSecurityConfigBySecurityID(newSecurity.SecurityID);

                        if (tbSecurityConfig != null)
                        {
                            int countSecurityConfig = tbSecurityConfig.Count;
                            for (int j = 0; j < countSecurityConfig; j++)
                            {
                                Business.ParameterItem newParameterItem = new Business.ParameterItem();
                                newParameterItem.ParameterItemID = tbSecurityConfig[j].SecurityConfigID;
                                newParameterItem.SecondParameterID = tbSecurityConfig[j].SecurityID;
                                newParameterItem.Name = tbSecurityConfig[j].Name;
                                newParameterItem.Code = tbSecurityConfig[j].Code;
                                newParameterItem.NumValue = tbSecurityConfig[j].NumValue;
                                newParameterItem.StringValue = tbSecurityConfig[j].StringValue;
                                newParameterItem.DateValue = tbSecurityConfig[j].DateValue;
                                newParameterItem.BoolValue = tbSecurityConfig[j].BoolValue;

                                Result[i].ParameterItems.Add(newParameterItem);
                            }
                        }
                        #endregion

                        #region GET SYMBOL BY SECURITY ID
                        tbSymbol = adapSymbol.GetSymbolBySecurityID(newSecurity.SecurityID);
                        if (tbSymbol != null)
                        {
                            int countSymbol = tbSymbol.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                Business.Symbol newSymbol = new Business.Symbol();
                                newSymbol.ParameterItems = new List<Business.ParameterItem>();

                                if (Business.Market.MarketArea != null)
                                {
                                    int countMarket = Business.Market.MarketArea.Count;
                                    for (int n = 0; n < countMarket; n++)
                                    {
                                        if (Business.Market.MarketArea[n].IMarketAreaID == tbSymbol[j].MarketAreaID)
                                        {
                                            newSymbol.MarketAreaRef = Business.Market.MarketArea[n];
                                            break;
                                        }
                                    }
                                }

                                newSymbol.Name = tbSymbol[j].Name;
                                newSymbol.SecurityID = tbSymbol[j].SecurityID;
                                newSymbol.SymbolID = tbSymbol[j].SymbolID;                                

                                tbTradingConfig = adapTradingConfig.GetTradingConfigBySymbolID(tbSymbol[j].SymbolID);
                                if (tbTradingConfig != null)
                                {
                                    int countTradingConfig = tbTradingConfig.Count;
                                    for (int n = 0; n < countTradingConfig; n++)
                                    {
                                        Business.ParameterItem newParameter = new Business.ParameterItem();
                                        newParameter.BoolValue = tbTradingConfig[n].BoolValue;
                                        newParameter.Code = tbTradingConfig[n].Code;
                                        newParameter.DateValue = tbTradingConfig[n].DateValue;
                                        newParameter.NumValue = tbTradingConfig[n].NumValue;
                                        newParameter.ParameterItemID = tbTradingConfig[n].TradingConfigID;
                                        newParameter.SecondParameterID = tbTradingConfig[n].SymbolID;

                                        newSymbol.ParameterItems.Add(newParameter);
                                    }
                                }

                                newSecurity.SymbolGroup.Add(newSymbol);
                            }
                        }
                        #endregion

                        Result.Add(newSecurity);
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
                adapSecurityConfig.Connection.Close();
                adapSymbol.Connection.Close();
                adapTradingConfig.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewSecurity(string Name, string Description,int MarketAreaID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewSecurity(Name, Description,MarketAreaID).ToString());
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
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal Business.Security GetSecurityBySecurityID(int SecurityID)
        {
            Business.Security Result = new Business.Security();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();
            DS.SecurityDataTable tbSecurity = new DS.SecurityDataTable();
            try
            {
                conn.Open();
                adap.Connection = conn;
                tbSecurity = adap.GetSecurityBySecurityID(SecurityID);
                if (tbSecurity != null)
                {
                    Result.SecurityID = tbSecurity[0].SecurityID;
                    Result.Name = tbSecurity[0].Name;
                    Result.Description = tbSecurity[0].Description;
                    Result.MarketAreaID = tbSecurity[0].MarketAreaID;
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
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        internal bool UpdateSecurity(int SecurityID, string Name, string Description,int MarketAreaID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.UpdateSecurity(Name, Description, MarketAreaID, SecurityID);
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
        /// <param name="SecurityID"></param>
        internal bool DeleteSecurity(int SecurityID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteSecurity(SecurityID);
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
        /// delete security by id
        /// delete all relation sercurity config
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        internal bool DFDeleteByID(int ID)
        {
            bool result = false;
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            SqlTransaction tran;
            connection.Open();
            tran = connection.BeginTransaction();
            try
            {

                result= this.DFDeleteByID(ID, connection,tran);
                if (result)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                tran.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        /// <summary>
        /// delete security by id
        /// delete all relation sercurity config
        /// delete all relation IAgentSecurity
        /// </summary>
        /// <param name="id">opened sql connection</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal bool DFDeleteByID(int id, SqlConnection connection,SqlTransaction trans)
        {
            DBWSecurityConfig dbwSecurityConfig = new DBWSecurityConfig();
            dbwSecurityConfig.DFDeleteBySecurityID(id, connection,trans);
            DBWIAgentSecurity dbwIAgentSecurity = new DBWIAgentSecurity();
            dbwIAgentSecurity.DFDeleteBySecurityID(id, connection,trans);

            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            int affectRow= adap.DeleteSecurity(id);
            if (affectRow == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountSecurity()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SecurityTableAdapter adap = new DSTableAdapters.SecurityTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountSecurity();
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
