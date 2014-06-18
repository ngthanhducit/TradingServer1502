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
        /// <returns></returns>
        public static List<Business.InternalMail> FacadeGetAllInternalMail()
        {
            return Facade.internalMailInstance.GetAllInternalMail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        public static List<Business.InternalMail> FacadeGetInternalMailToInvestor(int Start, int End, string investorCode)
        {
            List<Business.InternalMail> tem = Facade.internalMailInstance.GetInternalMailToInvestor(investorCode);
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            if (tem != null)
            {
                int count = tem.Count;
                if (Start > count - 1)
                {
                    return null;
                }
                if (End > count - 1)
                {
                    End = count;
                }
                for (int i = Start; i < End; i++)
                {
                    result.Add(tem[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        public static List<Business.InternalMail> FacadeGetInternalMailFromInvestor(string investorCode)
        {
            return Facade.internalMailInstance.GetInternalMailFromInvestor(investorCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        public static List<Business.InternalMail> FacadeGetTopInternalMailToInvestor(string investorCode)
        {
            return Facade.internalMailInstance.GetTopInternalMailToInvestor(investorCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailID"></param>
        /// <returns></returns>
        public static Business.InternalMail FacadeGetInternalMailToInvestorByID(int mailID)
        {
            return Facade.internalMailInstance.GetInternalMailToInvestorByID(mailID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailID"></param>
        /// <param name="codeAgent"></param>
        /// <returns></returns>
        public static Business.InternalMail FacadeGetInternalMailToAgentByID(int mailID, string codeAgent)
        {
            return Facade.internalMailInstance.GetInternalMailToAgentByID(mailID, codeAgent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailIns"></param>
        /// <returns></returns>
        public static int FacadeAddNewInternalMail(Business.InternalMail internalMailIns)
        {
            return Facade.internalMailInstance.AddNewInternalMail(internalMailIns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investor"></param>
        /// <returns></returns>
        public static int FacadeAutoSendMailRegistration(Business.Investor investor)
        {
            return Facade.internalMailInstance.AutoSendMailRegistration(investor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailIns"></param>
        /// <returns></returns>
        public static bool UpdateInternalMail(Business.InternalMail internalMailIns)
        {
            return Facade.internalMailInstance.UpdateInternalMail(internalMailIns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="mailID"></param>
        public static void UpdateInternalMailStatus(bool isNew, int mailID)
        {
            Facade.internalMailInstance.UpdateInternalMailStatus(isNew, mailID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailID"></param>
        /// <returns></returns>
        public static bool DeleteInternalMail(int internalMailID)
        {
            return Facade.internalMailInstance.DeleteInternalMail(internalMailID);
        }        
    }
}
