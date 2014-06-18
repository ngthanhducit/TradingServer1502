using System;
using System.Collections.Generic;


namespace TradingServer.Business
{
    public class SystemLog
    {
        internal int ID { get; set; }
        internal string LogContent { get; set; }
        internal string Comment { get; set; }
        internal string IPAddress { get; set; }
        internal DateTime LogDay { get; set; }
        internal int TypeID { get; set; }
        internal string InvestorCode { get; set; }
        internal int ManagerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="content"></param>
        /// <param name="comment"></param>
        /// <param name="IpAddress"></param>
        /// <returns></returns>
        internal bool Insert(int typeID, string content, string comment, string IpAddress,string investorCode)
        {
            this.TypeID = typeID;
            this.LogContent = content;
            this.Comment = comment;
            this.LogDay = DateTime.Now;
            this.IPAddress = IpAddress;
            this.InvestorCode = investorCode;
            
            DBW.DBWSystemLog dbw = new DBW.DBWSystemLog();
            return dbw.InsertLog(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        internal bool Insert(Business.SystemLog log)
        {
            if (Business.Market.ListSystemLog != null)
                Business.Market.ListSystemLog.Add(log);
            else
            {
                Business.Market.ListSystemLog = new List<SystemLog>();
                Business.Market.ListSystemLog.Add(log);
            }

            return true;
        }

        /// <summary>
        /// get all log by tine time span
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<SystemLog> GetByTime(DateTime begin, DateTime end)
        { 
            DBW.DBWSystemLog dbw=new DBW.DBWSystemLog();

            List<SystemLog> listLog = dbw.GetByTine(begin, end);
            return listLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<SystemLog> GetByTimeAndTye(int typeID, DateTime begin, DateTime end)
        {
            DBW.DBWSystemLog dbw = new DBW.DBWSystemLog();
            List<SystemLog> listLog = dbw.GetByTimeAndType(typeID, begin, end);
            return listLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetCodeAndTime(int typeID, DateTime begin, DateTime end, string code)
        {
            DBW.DBWSystemLog dbw = new DBW.DBWSystemLog();
            List<SystemLog> listLog = dbw.GetByCodeAndTime(begin, end, typeID, code);
            return listLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetLogLikeContent(string code, DateTime begin, DateTime end)
        {
            DBW.DBWSystemLog dbw = new DBW.DBWSystemLog();
            List<SystemLog> listLog = dbw.GetLogLikeContent(begin, end, code);
            return listLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetLogByIPAddress(string ipAddress, DateTime begin, DateTime end)
        {
            DBW.DBWSystemLog dbw = new DBW.DBWSystemLog();
            List<SystemLog> listLog = dbw.GetByIPAddress(begin, end, ipAddress);
            return listLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="typeID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal List<Business.SystemLog> GetLogByIPAddressAndType(DateTime begin, DateTime end, int typeID, string ipAddress)
        {
            DBW.DBWSystemLog dbw = new DBW.DBWSystemLog();
            List<SystemLog> listLog = dbw.GetByIPAddressAndType(begin, end, ipAddress, typeID);
            return listLog;
        }

        //=====================================TICK LOG==========================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int AddNewTickLog(Business.SystemLog value)
        {
            DBW.DBWTickLog dbwTickLog = new DBW.DBWTickLog();
            return dbwTickLog.AddTickLog(value);
        }
    }
}
