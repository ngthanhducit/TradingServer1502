using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Investor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> GetIGroupSecurity(int InvestorGroupID)
        {
            List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].InvestorGroupID == InvestorGroupID)
                    {
                        Business.IGroupSecurity newIGroupSecurity = new IGroupSecurity();
                        newIGroupSecurity.IGroupSecurityID = Business.Market.IGroupSecurityList[i].IGroupSecurityID;
                        newIGroupSecurity.SecurityID = Business.Market.IGroupSecurityList[i].SecurityID;
                        newIGroupSecurity.InvestorGroupID = Business.Market.IGroupSecurityList[i].InvestorGroupID;

                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                        {
                            int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == "B03")
                                {
                                    newIGroupSecurity.ExecutorMode = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].StringValue;
                                    break;
                                }
                            }
                        }

                        //newIGroupSecurity.IGroupSecurityConfig = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig;
                        //Result.Add(Business.Market.IGroupSecurityList[i]);
                        Result.Add(newIGroupSecurity);
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
        internal List<Business.IGroupSymbol> GetIGroupSymbol(int InvestorGroupID)
        {
            List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].InvestorGroupID == InvestorGroupID)
                    {
                        Business.IGroupSymbol newIGroupSymbol = new IGroupSymbol();
                        newIGroupSymbol.IGroupSymbolID = Business.Market.IGroupSymbolList[i].IGroupSymbolID;
                        newIGroupSymbol.InvestorGroupID = Business.Market.IGroupSecurityList[i].InvestorGroupID;
                        newIGroupSymbol.SymbolID = Business.Market.IGroupSymbolList[i].SymbolID;                        
                        //Result.Add(Business.Market.IGroupSymbolList[i]);
                        Result.Add(newIGroupSymbol);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListIGroupSecurity"></param>
        /// <returns></returns>
        internal List<Business.Symbol> GetSymbolOfInvestor(List<Business.IGroupSecurity> ListIGroupSecurity)
        {
            List<Business.Symbol> Result = new List<Symbol>();

            #region Init Symbol List
            if (ListIGroupSecurity != null)
            {
                int count = ListIGroupSecurity.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList != null)
                    {                        
                        int countSymbol = Business.Market.SymbolList.Count;
                        for (int j = 0; j < countSymbol; j++)
                        {
                            if (Business.Market.SymbolList[j].SecurityID == ListIGroupSecurity[i].SecurityID)
                            {           
                                Result.Add(Business.Market.SymbolList[j]);
                            }
                        }
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal Business.ClientConfig GetClientConfig(int InvestorGroupID)
        {
            Business.ClientConfig Result = new ClientConfig();

            #region Get Leverage In Investor List
            double LeverageGroup = 0;
            double MarginCall = 0;
            double StopOut = 0;
            int timeOut = 0;
            string freeMarginFormular = string.Empty;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G02")
                                {
                                    double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].NumValue, out LeverageGroup);
                                }

                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G04")
                                {
                                    int.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].NumValue, out timeOut);
                                }

                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G19")
                                {
                                    double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].NumValue, out MarginCall);
                                }

                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G20")
                                {
                                    double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].NumValue, out StopOut);
                                }

                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G26")
                                {
                                    freeMarginFormular = Business.Market.InvestorGroupList[i].ParameterItems[j].StringValue;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion

            //SET VALUE CLIENT CONFIG TO CLIENT
            Result.LeverageGroup = LeverageGroup;
            Result.MarginCall = MarginCall;
            Result.StopOut = StopOut;
            Result.TimeOut = timeOut;
            Result.FreeMarginFormular = freeMarginFormular;

            return Result;
        }
    }
}
