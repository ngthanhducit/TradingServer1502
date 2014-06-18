using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class InvestorStatus
    {
        public int InvestorStatusID { get; set; }
        public string Name { get; set; }

        #region Create Instance Class DBWInvestorStatus
        private static DBW.DBWInvestorStatus dbwInvestorStatus;
        private static DBW.DBWInvestorStatus DBWInvestorStatusInstance
        {
            get
            {
                if (InvestorStatus.dbwInvestorStatus == null)
                {
                    InvestorStatus.dbwInvestorStatus = new DBW.DBWInvestorStatus();
                }
                return dbwInvestorStatus;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.InvestorStatus> GetAllInvestorStatus()
        {
            return InvestorStatus.DBWInvestorStatusInstance.GetAllInvestorStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorStatusID"></param>
        /// <returns></returns>
        internal Business.InvestorStatus GetInvestorStatusByInvestorStatusID(int InvestorStatusID)
        {
            return InvestorStatus.DBWInvestorStatusInstance.GetInvestorStatusByID(InvestorStatusID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewInvestorStatus(string Name)
        {
            return InvestorStatus.DBWInvestorStatusInstance.AddNewInvestorStatus(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorStatusID"></param>
        internal bool DeleteInvestorStatusByID(int InvestorStatusID)
        {
            return InvestorStatus.DBWInvestorStatusInstance.DeleteInvestorStatus(InvestorStatusID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="InvestorStausID"></param>
        internal void UpdateInvestorStatus(string Name, int InvestorStausID)
        {
            InvestorStatus.DBWInvestorStatusInstance.UpdateInvestorStatus(Name, InvestorStausID);
        }
    }
}
