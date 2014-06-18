using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer
{
    public static partial class Facade
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public static bool FacadeCheckAgentExist(int agentID)
        {
            return Facade.AgentInstance.CheckAgentExist(agentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.Agent> FacadeGetAllAgent()
        {
            return Facade.AgentInstance.GetAllAgent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static Business.Agent FacadeGetAgentByAgentID(int AgentID)
        {
            return Facade.AgentInstance.GetAgentByAgentID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static List<Business.Agent> FacadeGetCodeAgentMailByInvestorGroupID(int InvestorGroupID)
        {
            return Facade.AgentInstance.CheckCodeAgentSendMail(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public static bool FacadeCheckPermitAgentSendMail(string code, int investorGroupID)
        {
            return Facade.AgentInstance.CheckPermitSendMail(code, investorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.RequestDealer> FacadeGetAllRequestDealer()
        {
            return Facade.AgentInstance.GetAllRequestDealer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.RequestDealer> FacadeGetAllRequestCompareDealer()
        {
            return Facade.AgentInstance.GetAllRequestCompareDealer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static Business.Agent FacadeGetAgentByInvestorID(int InvestorID)
        {
            return Facade.AgentInstance.GetAgentByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static int FacadeAddNewAgent(Business.Agent agent)
        {
            return Facade.AgentInstance.CreateNewAgent(agent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static int FacadeDeleteAgent(int AgentID)
        {
            return Facade.AgentInstance.DeleteAgentByID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Agent"></param>
        /// <returns></returns>
        public static bool FacadeUpdateAgent(Business.Agent Agent)
        {
            return Facade.AgentInstance.UpdateAgent(Agent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static bool FacadeSendRequestToDealer(Business.RequestDealer Request)
        {
            Facade.AgentInstance.CalculationOrderExecutionMode(Request);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Agent FacadeAdminLogin(string Code, string Pwd, string ipAddress)
        {
            return Facade.AgentInstance.AdminLogin(Code, Pwd, ipAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static bool FacadeAdminLogout(int AgentID)
        {
            return Facade.AgentInstance.AdminLogout(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Agent FacadeManagerLogin(string Code, string Pwd, string KeyActive, string ipAddress)
        {
            return Facade.AgentInstance.ManagerLogin(Code, Pwd, KeyActive, ipAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static bool FacadeManagerLogout(int AgentID)
        {
            return Facade.AgentInstance.ManagerLogout(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static bool FacadeDealerLogin(int AgentID, string ipAddress)
        {
            return Facade.AgentInstance.DealerLogin(AgentID, ipAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static bool FacadeDealerLogout(int AgentID)
        {
            return Facade.AgentInstance.DealerLogout(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static List<Business.RequestDealer> FacadeGetRequestToDealer(int AgentID)
        {
            return Facade.AgentInstance.GetRequestToDealer(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool FacadeDealerCommandConfirm(Business.RequestDealer command)
        {
            Facade.AgentInstance.DealerCommandConfirm(command);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool FacadeDealerCommandReject(Business.RequestDealer command)
        {
            Facade.AgentInstance.DealerCommandReject(command);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool FacadeDealerCommandReturn(Business.RequestDealer command)
        {
            Facade.AgentInstance.DealerCommandReturn(command);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool FacadeClientCancelRequest(Business.RequestDealer command)
        {
            Facade.AgentInstance.ClientCancelRequest(command);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public static string FacadeGetNoticeDealer(int agentID, string KeyActive)
        {
            return Facade.AgentInstance.GetNoticeDealer(agentID, KeyActive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FacadeGetAllDealerOnline()
        {
            return Facade.AgentInstance.GetAllDealerOnline();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FacadeGetAllArchiveCandlesOffline()
        {
            return Facade.AgentInstance.GetAllArchiveCandlesOffline();
        }

        /// <summary>
        /// Mode = 1 is Open Or Update, mode = 2 Is Close
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Command"></param>
        public static void FacadeSendNoticeManagerRequest(int Mode, Business.OpenTrade Command)
        {
            Facade.AgentInstance.SendNoticeManagerRequest(Mode, Command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public static void FacadeSendNoticeManagerDealerRequest(Business.RequestDealer Request)
        {
            Facade.AgentInstance.SendNoticeManagerDealerRequest(Request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="objInvestor"></param>
        public static void FacadeSendNotifyManagerRequest(int Mode, Business.Investor objInvestor)
        {            
            Facade.AgentInstance.SendNoticeManagerRequest(Mode, objInvestor);
        }

        /// <summary>
        /// mode = 1 is update group , mode = 2 is update group config
        /// mode = 3 is add group, mode = 4 is delete group
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="groupID"></param>
        public static void FacadeSendNoticeManagerChangeGroup(int mode, int groupID)
        {
            Facade.AgentInstance.SendNoticeManagerChangeGroup(mode, groupID);
        }

        /// <summary>
        /// mode = 1 is update symbol , mode = 2 is update symbol config
        /// mode = 3 is add symbol, mode = 4 is delete symbol
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="symbolID"></param>
        public static void FacadeSendNoticeManagerChangeSymbol(int mode, int symbolID)
        {
            Facade.AgentInstance.SendNoticeManagerChangeSymbol(mode, symbolID);
        }

        /// <summary>
        /// mode = 1 is update Agent, mode = 2 update IAgentGroup, mode = 3 update Permit , mode = 4 new mail
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="agentID"></param>
        public static void FacadeSendNoticeManagerChangeAgent(int mode, int agentID)
        {
            Facade.AgentInstance.SendNoticeManagerChangeAgent(mode, agentID);
        }

        /// <summary>
        /// mode = 1 is delete alert
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="alert"></param>
        public static void FacadeSendNoticeManagerChangeAlert(int mode, Business.PriceAlert alert)
        {
            Facade.AgentInstance.SendNoticeManagerChangeAlert(mode, alert);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="mailID"></param>
        /// <param name="agentID"></param>
        public static void FacadeSendNoticeManagerChangeMail(int mode, int mailID, int agentID)
        {
            Facade.AgentInstance.SendNoticeManagerChangeMail(mode, mailID, agentID);
        }

        public static void FacadeSendNoticeManagerOnline(Business.Agent agent)
        {
            Facade.AgentInstance.SendNoticeManagerOnline(agent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        public static List<int> FacadeMakeListIAgentGroupManager(string condition)
        {
            return Facade.AgentInstance.MakeListIAgentGroupManager(condition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        public static List<int> FacadeMakeListSymbolIDManager(string condition)
        {
            return Facade.AgentInstance.MakeListSymbolIDManager(condition);
        }

        public static void FacadeSendManagerTick(Business.Tick tick)
        {
            Facade.AgentInstance.SendNoticeManagerTick(tick);
        }

        public static bool FacadeCheckIpManager(string code, string ip)
        {
            return Facade.AgentInstance.CheckIpManager(code,ip);
        }

        public static bool FacadeCheckIpManagerAndAdmin(string code, string ip)
        {
            return Facade.AgentInstance.CheckIpManagerAndAdmin(code, ip);
        }

        public static bool FacadeCheckIpAdmin(string code, string ip)
        {
            return Facade.AgentInstance.CheckIpAdmin(code, ip);
        }

        public static bool FacadeCheckPermitAddMoney(string code)
        {
            return Facade.AgentInstance.CheckPermitAddMoney(code);
        }

        public static bool FacadeCheckPermitAccountManagerAndAdmin(string code)
        {
            return Facade.AgentInstance.CheckPermitAccountManagerAndAdmin(code);
        }

        public static bool FacadeCheckPermitTickManager(string code)
        {
            return Facade.AgentInstance.CheckPermitTickManager(code);
        }

        public static bool FacadeCheckPermitCommandManagerAndAdmin(string code)
        {
            return Facade.AgentInstance.CheckPermitCommandManagerAndAdmin(code);
        }

        public static bool FacadeCheckPermitAccessGroupManagerAndAdmin(string code, int investorGroupID)
        {
            return Facade.AgentInstance.CheckPermitAccessGroupManagerAndAdmin(code,investorGroupID);
        }

        public static bool FacadeCheckPermitDownloadStatement(string code)
        {
            return Facade.AgentInstance.CheckPermitDownloadStatements(code);
        }

        public static string FacadeGetTypeCommand(int idType)
        {
            return Facade.AgentInstance.GetTypeCommand(idType);
        }

        public static void FacadeFindDealerActivePendingRequest(Business.OpenTrade command)
        {
            Facade.AgentInstance.FindDealerActivePendingRequest(command);
        }

        public static bool FacadeManagerChangePass(int investorID, string code, string oldPass, string newPass)
        {
            return Facade.AgentInstance.ManagerChangePass(investorID, code, oldPass, newPass);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.AgentNotify> FacadeGetBKCommand()
        {
            return Business.Market.BKListNotifyQueueAgent;
        } 
    }
}
