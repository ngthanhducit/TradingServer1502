using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class InvestorGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.InvestorGroup> GetAllInvestorGroup()
        {
            return InvestorGroup.DBWInvestorGroupInstance.GetAllInvestorGroup();
        }

        /// <summary>
        /// Find Investor Group In Database
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal Business.InvestorGroup GetInvestorGroupByInvestorGroupID(int InvestorGroupID)
        {
            return InvestorGroup.DBWInvestorGroupInstance.GetInvestorGroupByInvestorGroupID(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal bool DFDeleteByID(int id)
        {
            DBW.DBWInvestorGroup group = new DBW.DBWInvestorGroup();
            return group.DFDeleteInvestorGroup(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalInvestorGroup()
        {
            return InvestorGroup.DBWInvestorGroupInstance.CountInvestorGroup();
        }
    }
}
