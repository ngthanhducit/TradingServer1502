using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWTickLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.SystemLog> GetData()
        {
            List<Business.SystemLog> result = new List<Business.SystemLog>();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TickLogTableAdapter adap = new DSTableAdapters.TickLogTableAdapter();
            DS.TickLogDataTable tbTickLog = new DS.TickLogDataTable();

            try
            {
                tbTickLog = adap.GetData();

                if (tbTickLog != null)
                {
                    int count = tbTickLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.SystemLog newSystemLog = new Business.SystemLog();
                        newSystemLog.ID = tbTickLog[i].TickLogID;
                        newSystemLog.LogContent = tbTickLog[i].LogContent;
                        newSystemLog.LogDay = tbTickLog[i].Day;
                        newSystemLog.InvestorCode = tbTickLog[i].InvestorCode;
                        newSystemLog.Comment = tbTickLog[i].Comment;
                        newSystemLog.IPAddress = tbTickLog[i].IPAddress;
                        newSystemLog.TypeID = tbTickLog[i].LogTypeID;

                        result.Add(newSystemLog);
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int AddTickLog(Business.SystemLog value)
        {
            int result = -1;

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.TickLogTableAdapter adap = new DSTableAdapters.TickLogTableAdapter();

            try
            {
                int.Parse(adap.AddTickLog(value.TypeID, value.LogContent, value.LogDay, value.Comment, value.IPAddress, value.InvestorCode).ToString());
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

            return result;
        }
    }
}
