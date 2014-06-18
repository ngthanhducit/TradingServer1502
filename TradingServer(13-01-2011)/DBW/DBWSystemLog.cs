using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWSystemLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private List<Business.SystemLog> MapSystemLog(DS.SystemLogDataTable tb)
        {
            if (tb == null || tb.Rows.Count == 0)
            { return null; }
            List<Business.SystemLog> systemLogList = new List<Business.SystemLog>();
            int count = tb.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                Business.SystemLog systemLog = new Business.SystemLog();
                systemLog.ID = tb[i].SystemLogID;
                systemLog.TypeID = tb[i].LogTypeID;
                systemLog.Comment = tb[i].Comment;
                systemLog.LogContent = tb[i].LogContent;
                systemLog.LogDay = tb[i].Day;
                systemLog.IPAddress = tb[i].IPAddress;
                systemLog.InvestorCode = tb[i].InvestorCode;

                systemLogList.Add(systemLog);
            }

            return systemLogList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetByTimeAndType(int typeID, DateTime begin, DateTime end)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            DS.SystemLogDataTable tb = null;
            try
            {
                DSTableAdapters.SystemLogTableAdapter adap = new DSTableAdapters.SystemLogTableAdapter();
                adap.Connection = connection;
                tb = adap.GetByTypeAndTime(begin, typeID, end);
            }
            catch (Exception ex)
            { 
            
            }
            finally
            {
                connection.Dispose();
            }
            return this.MapSystemLog(tb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetByTine(DateTime begin, DateTime end)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            DS.SystemLogDataTable tb = null;
            try
            {
                DSTableAdapters.SystemLogTableAdapter adap = new DSTableAdapters.SystemLogTableAdapter();
                adap.Connection = connection;
                tb = adap.GetByTime(begin, end);
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Dispose();
            }
            return this.MapSystemLog(tb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetByIPAddress(DateTime begin, DateTime end, string ipaddress)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            DS.SystemLogDataTable tb = null;
            DSTableAdapters.SystemLogTableAdapter adap  = new DSTableAdapters.SystemLogTableAdapter();

            try
            {
                connection.Open();
                adap.Connection = connection;
                tb = adap.GetByIPAddress(ipaddress.Trim(),begin, end);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                adap.Connection.Close();
                connection.Close();
            }

            return this.MapSystemLog(tb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="ipAddress"></param>
        /// <param name="typeID"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetByIPAddressAndType(DateTime begin, DateTime end, string ipAddress, int typeID)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            DS.SystemLogDataTable tb = null;
            DSTableAdapters.SystemLogTableAdapter adap = new DSTableAdapters.SystemLogTableAdapter();

            try
            {
                connection.Open();
                adap.Connection = connection;
                tb = adap.GetByIPAddressAndType(ipAddress.Trim(), typeID, begin, end);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                adap.Connection.Close();
                connection.Close();
            }

            return this.MapSystemLog(tb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="typeID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetByCodeAndTime(DateTime begin, DateTime end, int typeID, string code)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            DS.SystemLogDataTable tb = null;
            try
            {
                DSTableAdapters.SystemLogTableAdapter adap = new DSTableAdapters.SystemLogTableAdapter();
                adap.Connection = connection;
                tb = adap.GetByCodeAndTime(begin, typeID, end, code);                
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Dispose();
            }
            return this.MapSystemLog(tb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetLogLikeContent(DateTime begin, DateTime end, string code)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            
            DS.SystemLogDataTable tb = null;
            try
            {
                DSTableAdapters.SystemLogTableAdapter adap = new DSTableAdapters.SystemLogTableAdapter();
                adap.Connection = connection;
                
                tb = adap.GetLogLikeContent(code, begin, end);
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Dispose();
            }
            return this.MapSystemLog(tb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemLog"></param>
        /// <returns></returns>
        internal bool InsertLog(Business.SystemLog systemLog)
        {
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            int rowAffect=0;
            try
            {
                DSTableAdapters.SystemLogTableAdapter adap = new DSTableAdapters.SystemLogTableAdapter();
                adap.Connection = connection;
                connection.Open();
                rowAffect = adap.Insert(systemLog.TypeID, systemLog.LogContent, systemLog.LogDay, systemLog.Comment, systemLog.IPAddress, systemLog.InvestorCode); ;
                
            }
            catch (Exception ex)
            { 
            
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            if (rowAffect == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    //end class code
}
