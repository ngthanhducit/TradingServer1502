using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class LastBalance
    {
        public int LastAccountID { get; set; }
        public int InvestorID { get; set; }
        public string LoginCode { get; set; }
        public double PLBalance { get; set; }
        public double ClosePL { get; set; }
        public double Deposit { get; set; }
        public double Balance { get; set; }
        public double FloatingPL { get; set; }
        public double Credit { get; set; }
        public double LastEquity { get; set; }
        public double LastMargin { get; set; }
        public double EndMargin { get; set; }
        public double FreeMargin { get; set; }
        public double EndFreeMargin { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime EndLogDate { get; set; }
        public double CreditOut { get; set; }
        public double Withdrawal { get; set; }
        public double CreditAccount { get; set; }
        public double MonthSize { get; set; }

        #region CREATE INSTALCE DBWLASTBALANCE
        private static DBW.DBWLastBalance _instance;
        internal static DBW.DBWLastBalance Instance
        {
            get
            {
                if (LastBalance._instance == null)
                    Business.LastBalance._instance = new DBW.DBWLastBalance();

                return Business.LastBalance._instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.LastBalance> GetAllData()
        {
            return Business.LastBalance.Instance.GetData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int AddNewLastAcount(Business.LastBalance value)
        {
            return Business.LastBalance.Instance.InsertLastAccount(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int AddNewLastAcount(List<Business.LastBalance> values)
        {
            return Business.LastBalance.Instance.InsertLastAccount(values);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal Business.LastBalance GetLastAccountByDateTime(int investorID, DateTime timeStart, DateTime timeEnd)
        {
            return Business.LastBalance.Instance.GetLastAccountByTime(investorID, timeStart, timeEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal List<Business.LastBalance> GetLastAccountByTimeInvestorList(List<int> listInvestorID, DateTime timeStart, DateTime timeEnd)
        {
            return Business.LastBalance.Instance.GetLastAccountByTimeListInvestorID(listInvestorID, timeStart, timeEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal List<Business.LastBalance> GetLastAccountByTimeInvestorList(Dictionary<int, string> listInvestorID, DateTime timeStart, DateTime timeEnd)
        {
            return Business.LastBalance.Instance.GetLastAccountByTimeListInvestorID(listInvestorID, timeStart, timeEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal List<Business.LastBalance> GetLastAccountByTimeInvestor(Dictionary<int, string> listInvestorID, DateTime timeStart, DateTime timeEnd)
        {
            return Business.LastBalance.Instance.GetLastAccountByTimeInvestor(listInvestorID, timeStart, timeEnd);
        }
    }
}
