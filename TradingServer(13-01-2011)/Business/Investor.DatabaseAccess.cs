using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class Investor
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Investor> GetAllInvestor()
        {
            return Investor.DBWInvestorInstance.GetAllInvestor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal Business.Investor GetInvestorByInvestorID(int InvestorID)
        {
            return Investor.DBWInvestorInstance.GetInvestorByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RowNumber"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorWithRowNumber(int RowNumber, int Limit)
        {
            return Investor.DBWInvestorInstance.GetInvestorByStartEnd(RowNumber, Limit);
        }

        /// <summary>
        /// GET CODE LOGIN BY INVESTOR ID
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal string GetCodeLoginByInvestorID(int InvestorID)
        {
            return Investor.DBWInvestorInstance.GetInvestorCodeByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool VerifyMasterPassword(int investorID, string password)
        {
            bool resultCheck = false;
            Business.Investor result = Investor.DBWInvestorInstance.CheckMasterPassword(investorID, password);
            if (result != null && result.InvestorID > 0)
                resultCheck = true;

            return resultCheck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorFromTo(int from, int to)
        {
            List<Business.Investor> result = new List<Business.Investor>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                if (to > count)
                    to = count;
                for (int i = from; i < to; i++)
                {
                    result.Add(Business.Market.InvestorList[i]);
                }
            }

            return result;
        }

        internal Dictionary<int, double> GetTotalDepositInvestor(List<int> listInvestor)
        {
            return null;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal Business.Investor LoginAgent(string Code, string Pwd)
        {
            return Investor.DBWInvestorInstance.LoginAgent(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal Business.Investor GetInvestor(string Code, string Pwd)
        {
            return Investor.DBWInvestorInstance.LoginSystem(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        /// <returns></returns>
        internal int CreateNewInvestor(Business.Investor objInvestorProfile)
        {
            return Investor.DBWInvestorInstance.AddNewInvestor(objInvestorProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        public int AddInvestorProfile(Business.Investor objInvestor)
        {
            return TradingServer.Business.Investor.DBWInvestorInstance.CreateNewInvestorProfile(objInvestor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        internal int DeleteInvestor(int InvestorID)
        {
            return Investor.DBWInvestorInstance.DeleteInvestorByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal bool DeleteInvestorProfileByInvestorID(int investorID)
        {
            return Investor.DBWInvestorInstance.DeleteInvestorProfileByInvestorID(investorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        internal bool UpdateBalance(int InvestorID, double Balance)
        {
            return Investor.DBWInvestorInstance.UpdateBalanceAccount(InvestorID, Balance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        internal bool UpdateBalanceAndCredit(int InvestorID, double Balance, double Credit)
        {
            return Investor.DBWInvestorInstance.UpdateBalanceAndCredit(InvestorID, Balance, Credit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="agentRefID"></param>
        /// <returns></returns>
        internal bool UpdateAgentRefIDByInvestor(int investorID, int agentRefID)
        {
            return Investor.DBWInvestorInstance.UpdateAgentRefID(agentRefID, investorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="previousLedgerBalance"></param>
        /// <returns></returns>
        internal bool UpdatePreviousLedgerBalance(int investorID, double previousLedgerBalance)
        {
            return Investor.DBWInvestorInstance.UpdatePreviousLedgerBalance(investorID, previousLedgerBalance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal bool GetInvestorByCode(string Code)
        {
            return Investor.DBWInvestorInstance.GetCodeInvestor(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.Investor SelectInvestorByCode(string Code)
        {
            return Investor.DBWInvestorInstance.SelectInvestorByCode(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal bool ChangePasswordByCode(string Code, string Pwd)
        {
            return Investor.DBWInvestorInstance.UpdatePasswordByCode(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByInvestorGroupIDDB(int InvestorID)
        {
            return Investor.DBWInvestorInstance.GetInvestorByInvestorGroupID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <param name="From"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByInvestorGroupDB(int InvestorGroupID, int From, int Limit)
        {
            return Investor.DBWInvestorInstance.GetInvestorByInvestorGroup(InvestorGroupID, From, Limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal bool ChangePhonePwdByCode(string Code, string Pwd)
        {
            return Investor.DBWInvestorInstance.UpdatePhonePasswordByCode(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal bool ChangeReadPwdByCode(string Code, string Pwd)
        {
            return Investor.DBWInvestorInstance.UpdateReadPasswordByCode(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal int GetInvestorIDByCode(string Code)
        {
            return Investor.DBWInvestorInstance.GetInvestorIDByCode(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalInvestor()
        {
            return Investor.DBWInvestorInstance.CountInvestor();
        }

        /// <summary>
        /// UPDATE CREDIT OF INVESTOR ACCOUNT IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        internal bool UpdateCredit(int InvestorID, double Credit)
        {
            return Investor.DBWInvestorInstance.UpdateCreditAccount(InvestorID, Credit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <returns></returns>
        internal Dictionary<int, string> GetInvestorCodeByInvestorListID(List<int> listInvestorID)
        {
            return Investor.DBWInvestorInstance.GetCodeByListInvestor(listInvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        internal double GetPreviousLedgerBalanceByInvestorID(int investorID)
        {
            return Investor.DBWInvestorInstance.GetPreviousLedgerBalance(investorID);
        }
    }
}
