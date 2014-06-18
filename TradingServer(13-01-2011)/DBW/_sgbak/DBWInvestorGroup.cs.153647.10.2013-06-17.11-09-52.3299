using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWInvestorGroup
    {
        #region Create Instance Class DBWInvestorGroupConfig
        private static DBW.DBWInvestorGroupConfig dbwInvestorGroupConfig;
        private static DBW.DBWInvestorGroupConfig DBWInvestorGroupConfigInstance
        {
            get
            {
                if (DBWInvestorGroup.dbwInvestorGroupConfig == null)
                {
                    DBWInvestorGroup.dbwInvestorGroupConfig = new DBWInvestorGroupConfig();
                }
                return DBWInvestorGroup.dbwInvestorGroupConfig;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.InvestorGroup> GetAllInvestorGroup()
        {
            List<Business.InvestorGroup> Result = new List<Business.InvestorGroup>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();
            DSTableAdapters.InvestorGroupConfigTableAdapter adapInvestorGroupConfig = new DSTableAdapters.InvestorGroupConfigTableAdapter();

            DS.InvestorGroupDataTable tbInvestorGroup = new DS.InvestorGroupDataTable();
            DS.InvestorGroupConfigDataTable tbInvestorGroupConfig = new DS.InvestorGroupConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapInvestorGroupConfig.Connection = conn;

                tbInvestorGroup = adap.GetData();
                if (tbInvestorGroup != null)
                {
                    int count = tbInvestorGroup.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorGroup newInvestorGroup = new Business.InvestorGroup();
                        newInvestorGroup.DefautDeposite = tbInvestorGroup[i].DefautDeposite;
                        newInvestorGroup.InvestorGroupID = tbInvestorGroup[i].InvestorGroupID;
                        newInvestorGroup.Name = tbInvestorGroup[i].Name;
                        newInvestorGroup.Owner = tbInvestorGroup[i].Owner;

                        //newInvestorGroup.ParameterItems = DBWInvestorGroup.DBWInvestorGroupConfigInstance.GetInvestorGroupConfigByInvestorGroupID(tbInvestorGroup[i].InvestorGroupID);
                        tbInvestorGroupConfig = adapInvestorGroupConfig.GetInvestorGroupConfigByInvestorGroupID(tbInvestorGroup[i].InvestorGroupID);

                        if (tbInvestorGroupConfig != null)
                        {
                            int countInvestorGroupConfig = tbInvestorGroupConfig.Count;
                            for (int j = 0; j < countInvestorGroupConfig; j++)
                            {
                                Business.ParameterItem newParameterItem = new Business.ParameterItem();
                                newParameterItem.BoolValue = tbInvestorGroupConfig[j].BoolValue;
                                newParameterItem.Code = tbInvestorGroupConfig[j].Code;
                                newParameterItem.DateValue = tbInvestorGroupConfig[j].DateValue;
                                newParameterItem.NumValue = tbInvestorGroupConfig[j].NumValue;
                                newParameterItem.ParameterItemID = tbInvestorGroupConfig[j].InvestorGroupConfigID;
                                newParameterItem.SecondParameterID = tbInvestorGroupConfig[j].InvestorGroupID;
                                newParameterItem.Name = tbInvestorGroupConfig[j].Name;
                                newParameterItem.StringValue = tbInvestorGroupConfig[j].StringValue;

                                if (tbInvestorGroupConfig[j].Code == "G01")
                                {
                                    bool isEnable = false;
                                    if (tbInvestorGroupConfig[j].BoolValue == 1)
                                        isEnable = true;
                                    newInvestorGroup.IsEnable = isEnable;
                                }

                                if (tbInvestorGroupConfig[j].Code == "G26")
                                {
                                    newInvestorGroup.FreeMargin = tbInvestorGroupConfig[j].StringValue;
                                }

                                if (tbInvestorGroupConfig[j].Code == "G19")
                                {
                                    double MarginCallLevel = 0;
                                    double.TryParse(tbInvestorGroupConfig[j].NumValue, out MarginCallLevel);
                                    newInvestorGroup.MarginCall = MarginCallLevel;
                                }

                                if (tbInvestorGroupConfig[j].Code == "G20")
                                {
                                    double MarginStopOutLevel = 0;
                                    double.TryParse(tbInvestorGroupConfig[j].NumValue, out MarginStopOutLevel);
                                    newInvestorGroup.MarginStopOut = MarginStopOutLevel;
                                }

                                if (tbInvestorGroupConfig[j].Code == "G38")
                                {
                                    if (tbInvestorGroupConfig[j].BoolValue == 1)
                                        newInvestorGroup.IsManualStopOut = true;
                                }

                                if (newInvestorGroup.ParameterItems == null)
                                    newInvestorGroup.ParameterItems = new List<Business.ParameterItem>();

                                newInvestorGroup.ParameterItems.Add(newParameterItem);
                            }
                        }

                        //if (newInvestorGroup.ParameterItems != null)
                        //{
                        //    int countParameter = newInvestorGroup.ParameterItems.Count;
                        //    for (int j = 0; j < countParameter; j++)
                        //    {
                        //        if (newInvestorGroup.ParameterItems[j].Code == "G26")
                        //        {
                        //            newInvestorGroup.FreeMargin = newInvestorGroup.ParameterItems[j].StringValue;
                        //        }

                        //        if (newInvestorGroup.ParameterItems[j].Code == "G19")
                        //        {
                        //            double MarginCallLevel=0;
                        //            double.TryParse(newInvestorGroup.ParameterItems[j].NumValue, out MarginCallLevel);
                        //            newInvestorGroup.MarginCall = MarginCallLevel;
                        //        }

                        //        if (newInvestorGroup.ParameterItems[j].Code == "G20")
                        //        {
                        //            double MarginStopOutLevel = 0;
                        //            double.TryParse(newInvestorGroup.ParameterItems[j].NumValue, out MarginStopOutLevel);
                        //            newInvestorGroup.MarginStopOut = MarginStopOutLevel;
                        //        }
                        //    }
                        //}

                        Result.Add(newInvestorGroup);
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
                adapInvestorGroupConfig.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal Business.InvestorGroup GetInvestorGroupByInvestorGroupID(int InvestorGroupID)
        {
            Business.InvestorGroup Result = new Business.InvestorGroup();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();
            DS.InvestorGroupDataTable tbInvestorGroup=new DS.InvestorGroupDataTable();            

            try
            {
                conn.Open();
                adap.Connection = conn;

                tbInvestorGroup = adap.GetInvestorGroupIDByInvestorGroupID(InvestorGroupID);
                if (tbInvestorGroup != null)
                {
                    Result.DefautDeposite = tbInvestorGroup[0].DefautDeposite;
                    Result.InvestorGroupID = tbInvestorGroup[0].InvestorGroupID;
                    Result.Name = tbInvestorGroup[0].Name;
                    Result.Owner = tbInvestorGroup[0].Owner;
                    Result.ParameterItems = DBWInvestorGroup.DBWInvestorGroupConfigInstance.GetInvestorGroupConfigByInvestorGroupID(tbInvestorGroup[0].InvestorGroupID);
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
        /// <param name="objInvestorGroup"></param>
        /// <returns></returns>
        internal int AddNewInvestorGroup(Business.InvestorGroup objInvestorGroup)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                Result = int.Parse(adap.AddNewInvestorGroup(objInvestorGroup.Name, objInvestorGroup.Owner, objInvestorGroup.DefautDeposite).ToString());
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
        /// <param name="objInvestorGroup"></param>
        internal bool UpdateInvestorGroup(Business.InvestorGroup objInvestorGroup)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.UpdateInvestorGroup(objInvestorGroup.Name, objInvestorGroup.Owner, objInvestorGroup.DefautDeposite, objInvestorGroup.InvestorGroupID);
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
        internal bool DeleteInvestorGroup(int InvestorGroupID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteInvestorGroup(InvestorGroupID);

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
        /// delete InvestorGroup 
        /// delete relation InvestorGroupConfig
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        internal bool DFDeleteInvestorGroup(int groupID)
        {
            bool result = false;
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            SqlTransaction trans;
            connection.Open();
            trans = connection.BeginTransaction();
            
            try
            {
                
                result= this.DFDeleteInvestorGroup(groupID, connection,trans);
                if (result)
                { trans.Commit(); }
                else
                { trans.Rollback(); }
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
            finally
            {
                trans.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        /// <summary>
        /// delete InvestorGroup 
        /// delete relation InvestorGroupConfig
        /// delete relation IGroupSecurity
        /// delete relation IAgentGroup
        /// </summary>
        /// <param name="groupID">group id</param>
        /// <param name="connection">opened sql connection</param>
        /// <returns></returns>
        internal bool DFDeleteInvestorGroup(int groupID,SqlConnection connection,SqlTransaction trans)
        {
            DBWInvestorGroupConfig dbwGroupConfig = new DBWInvestorGroupConfig();
            dbwGroupConfig.DFDeleteByGroupID(groupID, connection,trans);
            DBWIGroupSecurity dbwIGroupSecurity = new DBWIGroupSecurity();
            dbwIGroupSecurity.DFDeleteByInvestorGroupID(groupID, connection,trans);
            DBWIAgentGroup dbwIAgentGroup = new DBWIAgentGroup();
            dbwIAgentGroup.DFDeleteByInvestorGroupID(groupID, connection,trans);

            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();
            adap.Connection = connection;
            adap.Transaction = trans;
            int affectRow= adap.DeleteInvestorGroup(groupID);
            if (affectRow == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountInvestorGroup()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorGroupTableAdapter adap = new DSTableAdapters.InvestorGroupTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountInvestorGroup();
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
