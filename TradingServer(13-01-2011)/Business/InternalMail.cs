using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class InternalMail
    {
        public int InternalMailID { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string Content { get; set; }
        public bool IsNew { get; set; }
        public DateTime Time { get; set; }

        #region CREATE INSTANCE DBWINTERNALMAIL
        private DBW.DBWInternalMail dbwInternalMail;
        internal DBW.DBWInternalMail InternalMailInstance
        {
            get
            {
                if (this.dbwInternalMail == null)
                    this.dbwInternalMail = new DBW.DBWInternalMail();

                return this.dbwInternalMail;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<InternalMail> GetAllInternalMail()
        {
            return this.InternalMailInstance.GetAllInternalMail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        internal List<InternalMail> GetInternalMailToInvestor(string investorCode)
        {
            return this.InternalMailInstance.GetInternalMailToInvestor(investorCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        internal List<InternalMail> GetInternalMailFromInvestor(string investorCode)
        {
            return this.InternalMailInstance.GetInternalMailFromInvestor(investorCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        internal List<InternalMail> GetTopInternalMailToInvestor(string investorCode)
        {
            return this.InternalMailInstance.GetTopInternalMailToInvestor(investorCode);
        }

        internal InternalMail GetInternalMailToInvestorByID(int mailID)
        {
            return this.InternalMailInstance.GetInternalMailToInvestorByID(mailID);
        }

        internal InternalMail GetInternalMailToAgentByID(int mailID,string codeAgent)
        {
            for (int i = 0; i < Market.AgentList.Count; i++)
            {
                if (codeAgent == Market.AgentList[i].Code)
                {
                    for(int j = 0; j < Market.AgentList[i].AgentMail.Count;j++)
                    {
                        if(mailID == Market.AgentList[i].AgentMail[j].InternalMailID)
                        {
                            return Market.AgentList[i].AgentMail[j];
                        }
                    }                    
                }
            }
            return this.GetInternalMailToInvestorByID(mailID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailInstance"></param>
        /// <returns></returns>
        internal int AddNewInternalMail(Business.InternalMail internalMailInstance)
        {
            return this.InternalMailInstance.AddNewInternalMail(internalMailInstance);
        }

        internal int AutoSendMailRegistration(Investor investor)
        {
            string content ="<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">"
            + "<Paragraph><Run FontWeight=\"Bold\">Dear " + investor.NickName + ",Your demo account has been activated !</Run></Paragraph><Paragraph><Run>Please, keep your login credentials for future access :</Run>"
            + "</Paragraph><Paragraph><Run>Username : </Run><Run FontWeight=\"Bold\">"+ investor.Code + "</Run></Paragraph><Paragraph><Run>Password : </Run><Run FontWeight=\"Bold\">" + investor.PrimaryPwd + "</Run></Paragraph>"
            + "<Paragraph><Run>Access : http://et5.mpf.co.id</Run></Paragraph><Paragraph><Run FontWeight=\"Bold\">Best Regards,</Run></Paragraph><Paragraph><Run>Millennium Penata Futures Customer Service Customer Service</Run>"
            + "</Paragraph><Paragraph><Run>Contact us anytime: support@mpf.co.id</Run></Paragraph></Section>";
            Business.InternalMail newInternalMailMessage = new InternalMail();
            newInternalMailMessage.Content = content;
            newInternalMailMessage.From = "admin";
            newInternalMailMessage.FromName = "PT Millennium Penata Futures";
            newInternalMailMessage.Subject = "Registratation";
            newInternalMailMessage.Time = DateTime.Now;
            newInternalMailMessage.To = investor.Code;
            newInternalMailMessage.ToName = investor.NickName;
            newInternalMailMessage.IsNew = true;
            return this.AddNewInternalMail(newInternalMailMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailInstance"></param>
        /// <returns></returns>
        internal bool UpdateInternalMail(Business.InternalMail internalMailInstance)
        {
            return this.InternalMailInstance.UpdateInternalMail(internalMailInstance);
        }

        internal void UpdateInternalMailStatus(bool isNew,int mailID)
        {
            this.InternalMailInstance.UpdateInternalMailStatus(isNew,mailID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailID"></param>
        /// <returns></returns>
        internal bool DeleteInternalMail(int internalMailID)
        {
            return this.InternalMailInstance.DeleteInternalMail(internalMailID);
        }
    }
}
