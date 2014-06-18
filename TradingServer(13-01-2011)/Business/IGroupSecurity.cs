using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class IGroupSecurity
    {
        public int IGroupSecurityID { get; set; }
        public int SecurityID { get; set; }
        public int InvestorGroupID { get; set; }
        public List<Business.ParameterItem> IGroupSecurityConfig { get; set; }
        public string ExecutorMode { get; set; }

        #region Create Instance Class DBWIGroupSecurity
        private static DBW.DBWIGroupSecurity dbwIGroupSecurity;
        private static DBW.DBWIGroupSecurity DBWIGroupSecurityInstance
        {
            get
            {
                if (IGroupSecurity.dbwIGroupSecurity == null)
                {
                    IGroupSecurity.dbwIGroupSecurity = new DBW.DBWIGroupSecurity();
                }
                return IGroupSecurity.dbwIGroupSecurity;
            }
        }
        #endregion

        #region Select Insert Update Delete IGroupSecurity
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<IGroupSecurity> GetAllIGroupSecurity()
        {
            return IGroupSecurity.DBWIGroupSecurityInstance.GetAllIGroupSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal Business.IGroupSecurity GetIGroupSecurityByID(int IGroupSecurityID)
        {
            return IGroupSecurity.DBWIGroupSecurityInstance.GetIGroupSecurityByID(IGroupSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> GetIGroupSecurityBySecurityIDCommand(int SecurityID)
        {
            List<Business.IGroupSecurity> result = new List<Business.IGroupSecurity>();
            result = IGroupSecurity.dbwIGroupSecurity.GetIGroupSecurityBySecurityID(SecurityID);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> GetIGroupSecurityByInvestorGroupIDCommand(int InvestorGroupID)
        {
            List<Business.IGroupSecurity> result = new List<Business.IGroupSecurity>();
            result = IGroupSecurity.dbwIGroupSecurity.GetIGroupSecurityByInvestorGroupID(InvestorGroupID);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal int AddIGroupSecurityCommand(int InvestorGroupID, int SecurityID)
        {
            int result;
            result = IGroupSecurity.dbwIGroupSecurity.AddIGroupSecurity(InvestorGroupID, SecurityID);
            if (result > 0)
            {
                Business.IGroupSecurity newIGroupSecurity=new IGroupSecurity();
                newIGroupSecurity.IGroupSecurityID = result;
                newIGroupSecurity.InvestorGroupID = InvestorGroupID;
                newIGroupSecurity.SecurityID = SecurityID;

                if (Business.Market.IGroupSecurityList == null)
                    Business.Market.IGroupSecurityList = new List<IGroupSecurity>();

                Business.Market.IGroupSecurityList.Add(newIGroupSecurity);
            }
            return result;
        }

        /// <summary>
        /// AFTER ADD NEW IGROUPSECURITY THEN FILL AGAIN IGROUPSECURITY TO COMMAND
        /// </summary>
        /// <param name="investorGroupID"></param>
        /// <param name="securityID"></param>
        internal void ResetIGroupSecurityInCommand(int investorGroupID, int securityID, int iGroupSecurityID)
        {
            if (Business.Market.InvestorList != null)
            {
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == investorGroupID)
                    {
                        if (Business.Market.InvestorList[i].CommandList != null)
                        {
                            int count = Business.Market.InvestorList[i].CommandList.Count;
                            for (int j = 0; j < count; j++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].Symbol != null)
                                {
                                    if (Business.Market.InvestorList[i].CommandList[j].Symbol.SecurityID == securityID)
                                    {
                                        if (Business.Market.IGroupSecurityList != null)
                                        {
                                            int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                            for (int n = 0; n < countIGroupSecurity; n++)
                                            {
                                                if (Business.Market.IGroupSecurityList[n].InvestorGroupID == investorGroupID && 
                                                    Business.Market.IGroupSecurityList[n].SecurityID == securityID)
                                                {
                                                    Business.Market.InvestorList[i].CommandList[j].IGroupSecurity = Business.Market.IGroupSecurityList[n];

                                                    for (int m = 0; m < Business.Market.InvestorList[i].CommandList[j].Symbol.CommandList.Count; m++)
                                                    {
                                                        if (Business.Market.InvestorList[i].CommandList[j].Symbol.CommandList[m].ID == 
                                                            Business.Market.InvestorList[i].CommandList[j].ID)
                                                        {
                                                            Business.Market.InvestorList[i].CommandList[j].Symbol.CommandList[m].IGroupSecurity = Business.Market.IGroupSecurityList[n];

                                                            break;
                                                        }
                                                    }

                                                    if (Business.Market.CommandExecutor != null)
                                                    {
                                                        for (int k = 0; k < Business.Market.CommandExecutor.Count; k++)
                                                        {
                                                            if (Business.Market.CommandExecutor[k].ID == Business.Market.InvestorList[i].CommandList[j].ID)
                                                            {
                                                                Business.Market.CommandExecutor[k].IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="InvestorGroupID"></param>
        internal bool UpdateIGroupSecurityCommand(int IGroupSecurityID, int SecurityID, int InvestorGroupID)
        {
            return IGroupSecurity.dbwIGroupSecurity.UpdateIGroupSecurity(IGroupSecurityID, SecurityID, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityByIGroupSecurityIDCommand(int IGroupSecurityID)
        {
            bool Result = false;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].IGroupSecurityID == IGroupSecurityID)
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            //DELETE IGROUPSECURITY CONFIG IN RAM
                            if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                            {                                
                                for (int j = 0; j < Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count; j++)
                                {
                                    //CALL FUNCTION DELETE IGROUP SECURITY CONFIG IN DATABASE
                                    TradingServer.Facade.FacadeDeleteIGroupSecurityConfigByIGroupSecurityID(Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].SecondParameterID);

                                    Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.RemoveAt(j);
                                }
                            }

                            bool ResultDelete = IGroupSecurity.DBWIGroupSecurityInstance.DeleteIGroupSecurityByIGroupSecurityID(IGroupSecurityID);
                                                        
                            Business.Market.IGroupSecurityList.RemoveAt(i);

                            ts.Complete();

                            Result = true;

                            break;
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityByInvestorGroupIDCommand(int InvestorGroupID)
        {
            return IGroupSecurity.dbwIGroupSecurity.DeleteIGroupSecurityByInvestorGroupID(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal bool DeleteIGroupSecurityBySecurityIDCommand(int SecurityID)
        {
            return IGroupSecurity.dbwIGroupSecurity.DeleteIGroupSecurityBySecurityID(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalIGroupSecurity()
        {
            return IGroupSecurity.DBWIGroupSecurityInstance.CountIGroupSecurity();
        }
        #endregion

        #region Process Data In Class Market
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> GetIGroupSecurityByInvestorGroup(int InvestorGroupID)
        {
            List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();            
            
            #region Find IGroupSecurity Of Investor Group With Investor ID
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].InvestorGroupID == InvestorGroupID)
                    {
                        Result.Add(Business.Market.IGroupSecurityList[i]);
                    }
                }
            }
            #endregion

            return Result;
        }
        #endregion
    }
}
