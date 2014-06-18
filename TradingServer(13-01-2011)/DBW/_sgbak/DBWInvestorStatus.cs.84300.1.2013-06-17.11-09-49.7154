using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWInvestorStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.InvestorStatus> GetAllInvestorStatus()
        {
            List<Business.InvestorStatus> Result = new List<Business.InvestorStatus>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorStatusTableAdapter adap = new DSTableAdapters.InvestorStatusTableAdapter();
            DS.InvestorStatusDataTable tbInvestorStatus = new DS.InvestorStatusDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorStatus = adap.GetData();

                if (tbInvestorStatus != null)
                {
                    int count = tbInvestorStatus.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorStatus newInvestorStatus = new Business.InvestorStatus();
                        newInvestorStatus.InvestorStatusID = tbInvestorStatus[i].InvestorStatusID;
                        newInvestorStatus.Name = tbInvestorStatus[i].Name;

                        Result.Add(newInvestorStatus);
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
        /// <param name="InvestorStatusID"></param>
        /// <returns></returns>
        internal Business.InvestorStatus GetInvestorStatusByID(int InvestorStatusID)
        {
            Business.InvestorStatus Result = new Business.InvestorStatus();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorStatusTableAdapter adap = new DSTableAdapters.InvestorStatusTableAdapter();
            DS.InvestorStatusDataTable tbInvestorStatus = new DS.InvestorStatusDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorStatus = adap.GetInvestorStatusByInvestorStatusID(InvestorStatusID);

                if (tbInvestorStatus != null)
                {
                    Result.InvestorStatusID = tbInvestorStatus[0].InvestorStatusID;
                    Result.Name = tbInvestorStatus[0].Name;
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
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewInvestorStatus(string Name)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorStatusTableAdapter adap = new DSTableAdapters.InvestorStatusTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                Result = int.Parse(adap.AddNewInvestorStatus(Name).ToString());
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
        /// <param name="InvestorStatusID"></param>
        internal bool DeleteInvestorStatus(int InvestorStatusID)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorStatusTableAdapter adap = new DSTableAdapters.InvestorStatusTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.DeleteInvestorStatus(InvestorStatusID);
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
        /// <param name="Name"></param>
        /// <param name="InvestorStatusID"></param>
        internal void UpdateInvestorStatus(string Name, int InvestorStatusID)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorStatusTableAdapter adap = new DSTableAdapters.InvestorStatusTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.UpdateInvestorStatus(Name, InvestorStatusID);
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
