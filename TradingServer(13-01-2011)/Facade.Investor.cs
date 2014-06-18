using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace TradingServer
{
    public static partial class Facade
    {        
        /// <summary>
        /// FACADE GET ALL INVESTOR IN DATABASE
        /// </summary>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetAllInvestor()
        {
            return Facade.InvestorInstance.GetAllInvestor();
        }

        /// <summary>
        /// FACADE GET ALL INVESTOR ONLINE IN MARKET
        /// </summary>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorOnline(int From, int To)
        {
            return Facade.InvestorInstance.GetInvestorOnline(From, To);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorOnlineByGroupID(int From, int To, int InvestorGroupID)
        {
            return Facade.InvestorInstance.GetInvestorOnlineByGroupID(From, To, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static string FacadeGetLoginCodeByInvestorID(int InvestorID)
        {
            return Facade.InvestorInstance.GetCodeLoginByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RowNumber"></param>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorWithRowNumber(int RowNumber, int Limit)
        {
            return Facade.InvestorInstance.GetInvestorWithRowNumber(RowNumber, Limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorFormTo(int from, int to)
        {
            return Facade.InvestorInstance.GetInvestorFromTo(from, to);
        }

        /// <summary>
        /// FACADE GET INVESTOR WITH COMMAND IN RAM(GET FROM TO)
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorWithCommand(int From, int To)
        {
            return Facade.InvestorInstance.GetInvestorWithCommand(From, To);
        }

        /// <summary>
        /// GET ALL INVESTOR IN MARKET WITH MARGIN LEVEL SMALLER MARGIN CALL
        /// </summary>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorWithMarginLevel()
        {
            return Facade.InvestorInstance.GetInvestorWithMarginLevel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static Business.Investor FacadeFindInvestorWithOnlineCommand(string Code)
        {
            return Facade.InvestorInstance.FindInvestorWithOnlineCommand(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static Business.Investor FacadeFindInvestor(string Code)
        {
            return Facade.InvestorInstance.FindInvestor(Code);
        }

        /// <summary>
        /// FACADE GET INVESTOR BY INVESTOR ID IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static Business.Investor FacadeGetInvestorByInvestorID(int InvestorID)
        {
            return Facade.InvestorInstance.GetInvestorByInvestorID(InvestorID);
        }

        /// <summary>
        /// FACADE GET INVESTOR BY INVESTOR GROUP ID IN RAM With From To
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal static List<Business.Investor> FacadeGetInvestorByInvestorGroupID(int InvestorGroupID, int From, int To)
        {
            return Facade.InvestorInstance.GetInvestorByInvestorGroup(InvestorGroupID, From, To);
        }

        /// <summary>
        /// FACADE GET INVESTOR BY INVESTOR GROUP ID IN DATABASE
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal static List<Business.Investor> FacadeGetInvestorByInvestorGroup(int InvestorGroupID)
        {
            return Facade.InvestorInstance.GetInvestorByInvestorGroupIDDB(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        internal static List<Business.Investor> FacadeGetInvestorByInvestorGroupDB(int InvestorGroupID, int Start, int Limit)
        {
            return Facade.InvestorInstance.GetInvestorByInvestorGroupDB(InvestorGroupID, Start, Limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorGroupID"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal static List<Business.Investor> FacadeGetInvestorByGroupFromTo(int investorGroupID, int from, int to)
        {
            return Facade.InvestorInstance.GetInvestorByGroupFromTo(investorGroupID, from, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <returns></returns>
        internal static Dictionary<int, string> FacadeGetCodeByInvestorListID(List<int> listInvestorID)
        {
            return Facade.InvestorInstance.GetInvestorCodeByInvestorListID(listInvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        internal static double FacadeGetPreviousLedgerBalance(int investorID)
        {
            return Facade.InvestorInstance.GetPreviousLedgerBalanceByInvestorID(investorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="GroupListID"></param>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorByGroupList(int From, int To, List<int> GroupListID)
        {
            return Facade.InvestorInstance.GetInvestorByGroupList(From, To, GroupListID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        public static List<Business.Investor> FacadeGetInvestorByListCode(List<string> listCode)
        {
            return Facade.InvestorInstance.GetInvestorByListCode(listCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal static bool FacadeRemoveOpenTradeInInvestorList(int CommandID, int InvestorID)
        {
            return Facade.InvestorInstance.RemoveOnlineCommand(CommandID, InvestorID);
        }

        /// <summary>
        /// FACADE LOGIN TRADING SYSTEM OLD
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeLoginServer(string Code, string Pwd, int InvestorIndex, string IpAddress)
        {
            return Facade.InvestorInstance.Login(Code, Pwd, InvestorIndex, IpAddress);
        }

        /// <summary>
        /// FACADE LOGIN TRADING SYSTEM NEW
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeNewLoginServer(string Code, string Pwd, int InvestorIndex, string IpAddress, Socket InsSocket)
        {
            return Facade.InvestorInstance.NewLogin(Code, Pwd, InvestorIndex, IpAddress, InsSocket);
        }

        /// <summary>
        /// FACADE CLIENT LOGIN CONNECT WITH MT4
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeNewLoginServer(string code, string pwd, string ipAddress, Socket InsSocket)
        {
            return Facade.InvestorInstance.NewLogin(code, pwd, ipAddress, InsSocket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pwd"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static Business.Investor FacadeLoginMT4Server(string code, string pwd, string ipAddress)
        {
            return Facade.InvestorInstance.LoginMT4(code, pwd, ipAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool FacadeCheckMasterPassword(int investorID, string password)
        {
            return Facade.InvestorInstance.VerifyMasterPassword(investorID, password);
        }

        /// <summary>
        /// FACADE LOGIN AGENT IN DATABASE
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeLoginAgent(string Code, string Pwd)
        {
            return Facade.InvestorInstance.LoginAgent(Code, Pwd);
        }

        /// <summary>
        /// FACADE ADD NEW INVESTOR TO DATABASE
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        public static int FacadeAddNewInvestor(Business.Investor objInvestor)
        {
            return Facade.InvestorInstance.CreateNewInvestor(objInvestor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        public static int FacadeAddInvestorProfile(Business.Investor objInvestor)
        {
            return Facade.InvestorInstance.AddInvestorProfile(objInvestor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        public static int FacadeDeleteInvestor(int InvestorID)
        {
            return Facade.InvestorInstance.DeleteInvestor(InvestorID);
        }

        /// <summary>
        /// FACADE UPDATE INVESTOR IN RAM AND DATABASE
        /// </summary>
        /// <param name="objInvestor"></param>
        public static bool FacadeUpdateInvestor(Business.Investor objInvestor)
        {
            return Facade.InvestorInstance.UpdateInvestor(objInvestor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool FacadeUpdateInvestor(Business.Investor objInvestor, string ipAddress, string code)
        {
            return Facade.InvestorInstance.UpdateInvestor(objInvestor, ipAddress, code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool FacadeChangePassword(int investorID, string oldPwd, string pwd)
        {
            return Facade.InvestorInstance.ChangePrimaryPassword(investorID, oldPwd, pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="oldPwd"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool FacadeChangePasswordMT4(int investorID, string oldPwd, string pwd)
        {
            return Facade.InvestorInstance.ChangePrimaryPasswordMT4(investorID, oldPwd, pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="oldPwd"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool FacadeChangeReadOnlyPassword(int investorID, string oldPwd, string pwd)
        {
            return Facade.InvestorInstance.ChangeReadOnlyPassword(investorID, oldPwd, pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        public static bool FacadeUpdateBalance(int InvestorID, double Balance)
        {
            return Facade.InvestorInstance.UpdateBalance(InvestorID, Balance);
        }

        /// <summary>
        /// FACADE UPDATE CREDIT OF INVESTOR ACCOUNT IN DATABASE
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        public static bool FacadeUpdateCredit(int InvestorID, double Credit)
        {
            return Facade.InvestorInstance.UpdateCredit(InvestorID, Credit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        public static bool FacadeUpdateBalanceAndCredit(int InvestorID, double Balance, double Credit)
        {
            return Facade.InvestorInstance.UpdateBalanceAndCredit(InvestorID, Balance, Credit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public static bool FacadeUpdateAgentRefID(int investorID, int agentID)
        {
            return Facade.InvestorInstance.UpdateAgentRefIDByInvestor(investorID, agentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        public static bool FacadeUpdateUserConfig(int investorID, string userConfig)
        {
            return Facade.InvestorInstance.UpdateUserConfig(investorID, userConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        public static bool FacadeUpdateUserConfigIpad(int investorID, string userConfig)
        {
            return Facade.InvestorInstance.UpdateUserConfigIpad(investorID, userConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        public static bool FacadeUpdateUserConfigIphone(int investorID, string userConfig)
        {
            return Facade.InvestorInstance.UpdateUserConfigIphone(investorID, userConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static int FacadeGetUpdateCommandOfInvestor(int InvestorID)
        {
            return Facade.InvestorInstance.GetListUpdateCommandOfInvestor(InvestorID);
        }

        /// <summary>
        /// FACADE UPDATE PASSWORD BY CODE
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static bool FacadeUpdatePasswordByCode(string Code, string Pwd)
        {
            return Facade.InvestorInstance.ChangePasswordByCode(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static bool FacadeUpdateReadPwdByCode(string Code, string Pwd)
        {
            return Facade.InvestorInstance.ChangeReadPwdByCode(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static bool FacadeUpdatePhonePwdByCode(string Code, string Pwd)
        {
            return Facade.InvestorInstance.ChangePhonePwdByCode(Code, Pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="previousLedgerBalance"></param>
        /// <returns></returns>
        public static bool FacadeUpdatePreviousLedgerBalance(int investorID, double previousLedgerBalance)
        {
            return Facade.InvestorInstance.UpdatePreviousLedgerBalance(investorID, previousLedgerBalance);
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static string FacadeGetTaskNameOfInvestor(int InvestorID)
        {
            return Facade.InvestorInstance.GetTaskNameOfInvestor(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static List<string> FacadeGetTopQueueTask(int from, int to)
        {
            return TradingServer.Business.CalculatorFacade.GetTopQueueTask(from, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static bool FacadeAddDeposit(int InvestorID, double Money, string Comment)
        {
            return Facade.InvestorInstance.Deposit(Money, InvestorID, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static bool FacadeWithRawals(int InvestorID, double Money, string Comment)
        {
            return Facade.InvestorInstance.WithDrawals(InvestorID, Money, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        public static bool FacadeAddCredit(int InvestorID, double Credit, string Comment)
        {
            return Facade.InvestorInstance.AddCredit(Credit, InvestorID, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        public static bool FacadeSubCredit(int InvestorID, double Credit, string Comment)
        {
            return Facade.InvestorInstance.SubCredit(Credit, InvestorID, Comment);
        }

        /// <summary>
        /// Check Code In Database If Return True Then OK Else False
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static bool FacadeGetInvestorByCode(string Code)
        {
            return Facade.InvestorInstance.GetInvestorByCode(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static int FacadeGetInvestorIDByCode(string Code)
        {
            return Facade.InvestorInstance.GetInvestorIDByCode(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static Business.Investor FacadeSelectInvestorByCode(string Code)
        {
            return Facade.InvestorInstance.SelectInvestorByCode(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSecurity> FacadeGetIGroupSecurityByGroup(int investorGroupID)
        {
            return Facade.InvestorInstance.GetIGroupSecurity(investorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSymbol> FacadeGetIGroupSymbolByGroup(int investorGroupID)
        {
            return Facade.InvestorInstance.GetIGroupSymbol(investorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public static Business.ClientConfig FacadeGetClientConfigByGroup(int investorGroupID)
        {
            return Facade.InvestorInstance.GetClientConfig(investorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountInvestor()
        {
            return Facade.InvestorInstance.CountTotalInvestor();
        }
                
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="objInvestor"></param>
        ///// <returns></returns>
        //public static int FacadeAddNewInvestorProfile(Business.InvestorProfile objInvestorProfile)
        //{
        //    return Facade.InvestorProfileInstance.AddNewInvestorProfile(objInvestorProfile);            
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="InvestorProfileID"></param>
        ///// <returns></returns>
        //public static bool FacadeDeleteInvestorProfileByInvestorID(int InvestorID)
        //{

        //    return Facade.InvestorProfileInstance.DeleteInvestorProfileByInvestorID(InvestorID);
        //}

        /// <summary>
        /// FACADE UPDATE INVESTOR PROFILE IN RAM AND DATABASE
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        public static bool FacadeUpdateInvestorProfile(Business.Investor objInvestorProfile)
        {
            return Facade.InvestorInstance.UpdateInvestorProfile(objInvestorProfile);
        }

        /// <summary>
        /// FACADE UPDATE INVESTOR PROFILE IN RAM AND DATABASE
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        public static bool FacadeUpdateInvestorProfile(Business.Investor objInvestorProfile, string ipAddress, string code)
        {
            return Facade.InvestorInstance.UpdateInvestorProfile(objInvestorProfile, ipAddress, code);
        }
    }
}
