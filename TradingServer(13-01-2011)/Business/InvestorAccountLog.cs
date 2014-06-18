using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class InvestorAccountLog
    {
        public int ID { get; set; }
        public string DealID { get; set; }
        public int InvestorID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public double Amount { get; set; }
        public string Code { get; set; }

        #region Create Instance Class DBW Investor Account Log
        private static DBW.DBWInvestorAccountLog dbwInvestorAccountLog;
        private static DBW.DBWInvestorAccountLog DBWInvestorAccountLog
        {
            get
            {
                if (InvestorAccountLog.dbwInvestorAccountLog == null)
                {
                    InvestorAccountLog.dbwInvestorAccountLog = new DBW.DBWInvestorAccountLog();
                }

                return InvestorAccountLog.dbwInvestorAccountLog;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.InvestorAccountLog> GetInvestorAccountLogByInvestorID(int InvestorID)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.GetInvestorAccountLogByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.InvestorAccountLog> GetInvestorAccountLogWithTime(DateTime TimeStart, DateTime TimeEnd, int InvestorID)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.GetInvestorAccountLogWithTime(InvestorID, TimeStart, TimeEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Comment"></param>
        /// <returns></returns>
        internal List<Business.InvestorAccountLog> GetInvestorAccountLogWithComment(string Comment)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.GetInvestorAccountLogWithComment(Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorAccountLog"></param>
        /// <returns></returns>
        internal int AddNewInvestorAccountLog(Business.InvestorAccountLog objInvestorAccountLog)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.AddNewInvestorAccountLog(objInvestorAccountLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DealID"></param>
        /// <returns></returns>
        internal bool UpdateDealID(int ID, string DealID)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.UpdateDealID(ID, DealID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorAccountLog"></param>
        /// <returns></returns>
        internal bool UpdateInvestorAccountLog(Business.InvestorAccountLog objInvestorAccountLog)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.UpdateInvestorAccountLog(objInvestorAccountLog);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorAccountLogID"></param>
        /// <returns></returns>
        internal bool DeleteInvestorAccountLog(int InvestorAccountLogID)
        {
            return InvestorAccountLog.DBWInvestorAccountLog.DeleteInvestorAccountLog(InvestorAccountLogID);
        }
    }
}
