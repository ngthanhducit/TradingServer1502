using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWCommandLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.CommandLog> GetAllCommandLog()
        {
            List<Business.CommandLog> Result = new List<Business.CommandLog>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandLogTableAdapter adap = new DSTableAdapters.CommandLogTableAdapter();
            DS.CommandLogDataTable tbCommandLog = new DS.CommandLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandLog = adap.GetData();
                if (tbCommandLog != null)
                {
                    int count = tbCommandLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.CommandLog newCommandLog = new Business.CommandLog();
                        newCommandLog.CommandActionID = tbCommandLog[i].CommandActionID;
                        newCommandLog.CommandHistoryID = tbCommandLog[i].CommandHistoryID;
                        newCommandLog.CommandLogID = tbCommandLog[i].CommandLogID;
                        newCommandLog.InvestorID = tbCommandLog[i].InvestorID;
                        newCommandLog.LogContent = tbCommandLog[i].LogContent;
                        newCommandLog.LogDate = tbCommandLog[i].LogDate;
                        newCommandLog.OnlineCommandID = tbCommandLog[i].OnlineCommandID;

                        Result.Add(newCommandLog);
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
    }
}
