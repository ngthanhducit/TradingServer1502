using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWInvestorLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.InvestorLog> GetAllInvestorLog()
        {
            List<Business.InvestorLog> Result = new List<Business.InvestorLog>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorLogTableAdapter adap = new DSTableAdapters.InvestorLogTableAdapter();
            DS.InvestorLogDataTable tbInvestorLog = new DS.InvestorLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorLog = adap.GetData();

                if (tbInvestorLog != null)
                {
                    int count = tbInvestorLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorLog newInvestorLog = new Business.InvestorLog();
                        newInvestorLog.InvestorLogID = tbInvestorLog[i].InvestorLogID;
                        newInvestorLog.InvestorInstance.InvestorID = tbInvestorLog[i].InvestorID;
                        newInvestorLog.IP = tbInvestorLog[i].IP;
                        newInvestorLog.Message = tbInvestorLog[i].Message;
                        newInvestorLog.Status = tbInvestorLog[i].Status;
                        newInvestorLog.Time = tbInvestorLog[i].Time;

                        Result.Add(newInvestorLog);
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
        /// <param name="InvestorLogID"></param>
        /// <returns></returns>
        internal Business.InvestorLog GetInvestorLogByID(int InvestorLogID)
        {
            Business.InvestorLog Result = new Business.InvestorLog();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorLogTableAdapter adap = new DSTableAdapters.InvestorLogTableAdapter();
            DS.InvestorLogDataTable tbInvestorLog = new DS.InvestorLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorLog = adap.GetInvestorLogByID(InvestorLogID);

                if (tbInvestorLog != null)
                {
                    Result.InvestorLogID = tbInvestorLog[0].InvestorLogID;
                    Result.InvestorInstance.InvestorID = tbInvestorLog[0].InvestorID;
                    Result.IP = tbInvestorLog[0].IP;
                    Result.Message = tbInvestorLog[0].Message;
                    Result.Status = tbInvestorLog[0].Status;
                    Result.Time = tbInvestorLog[0].Time;
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
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.InvestorLog> GetInvestorLogByInvestorID(int InvestorID)
        {
            List<Business.InvestorLog> Result = new List<Business.InvestorLog>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorLogTableAdapter adap = new DSTableAdapters.InvestorLogTableAdapter();
            DS.InvestorLogDataTable tbInvestorLog = new DS.InvestorLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorLog = adap.GetInvestorLogByInvestorID(InvestorID);

                if (tbInvestorLog != null)
                {
                    int count = tbInvestorLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorLog newInvestorLog = new Business.InvestorLog();
                        newInvestorLog.InvestorLogID = tbInvestorLog[i].InvestorLogID;
                        newInvestorLog.InvestorInstance.InvestorID = tbInvestorLog[i].InvestorID;
                        newInvestorLog.IP = tbInvestorLog[i].IP;
                        newInvestorLog.Message = tbInvestorLog[i].Message;
                        newInvestorLog.Status = tbInvestorLog[i].Status;
                        newInvestorLog.Time = tbInvestorLog[i].Time;

                        Result.Add(newInvestorLog);
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
        /// <param name="objInvestorLog"></param>
        /// <returns></returns>
        internal int AddNewInvestorLog(Business.InvestorLog objInvestorLog)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorLogTableAdapter adap = new DSTableAdapters.InvestorLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewInvestorLog(objInvestorLog.InvestorInstance.InvestorID, objInvestorLog.Time, objInvestorLog.IP, objInvestorLog.Message, 
                                    objInvestorLog.Status).ToString());
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
        /// <param name="InvestorLogID"></param>
        internal void DeleteInvestorLog(int InvestorLogID)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorLogTableAdapter adap = new DSTableAdapters.InvestorLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.DeleteInvestorLog(InvestorLogID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorLog"></param>
        internal void UpdateInvestorLog(Business.InvestorLog objInvestorLog)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorLogTableAdapter adap = new DSTableAdapters.InvestorLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.UpdateInvestorLog(objInvestorLog.InvestorInstance.InvestorID, objInvestorLog.Time, objInvestorLog.IP, 
                                        objInvestorLog.Message, objInvestorLog.Status, objInvestorLog.InvestorLogID);
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
