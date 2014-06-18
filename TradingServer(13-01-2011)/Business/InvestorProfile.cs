using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class InvestorProfile
    {
        public int InvestorProfileID { get; set; }
        public Business.Investor InvestorInstance { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public DateTime RegisterDay { get; set; }
        public string Comment { get; set; }           
        public string NickName { get; set; }

        #region Create Instance Class DBWInvestorProfile
        private static DBW.DBWInvestorProfile dbwInvestorProfile;
        private static DBW.DBWInvestorProfile DBWInvestorProfileInstance
        {
            get
            {
                if (InvestorProfile.dbwInvestorProfile == null)
                {
                    InvestorProfile.dbwInvestorProfile = new DBW.DBWInvestorProfile();
                }
                return InvestorProfile.dbwInvestorProfile;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        /// <returns></returns>
        internal int AddNewInvestorProfile(Business.InvestorProfile objInvestorProfile)
        {
            return InvestorProfile.DBWInvestorProfileInstance.AddNewInvestorProfile(objInvestorProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        internal bool DeleteInvestorProfileByInvestorID(int InvestorID)
        {
            return InvestorProfile.DBWInvestorProfileInstance.DeleteInvestorProfileByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        internal void UpdateInvestorProfile(Business.InvestorProfile objInvestorProfile)
        {
            InvestorProfile.DBWInvestorProfileInstance.UpdateInvestorPofile(objInvestorProfile);
        }
    }
}
