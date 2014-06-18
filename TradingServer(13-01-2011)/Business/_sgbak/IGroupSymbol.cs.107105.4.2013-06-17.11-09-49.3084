using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class IGroupSymbol
    {
        public int IGroupSymbolID { get; set; }
        public int SymbolID { get; set; }
        public int InvestorGroupID { get; set; }
        public List<Business.ParameterItem> IGroupSymbolConfig { get; set; }

        #region Create Instance Class DBWIGroupSymbol
        private static DBW.DBWIGroupSymbol dbwIGroupSymbol;
        private static DBW.DBWIGroupSymbol DBWIGroupSymbolInstance
        {
            get
            {
                if (IGroupSymbol.dbwIGroupSymbol == null)
                {
                    IGroupSymbol.dbwIGroupSymbol = new DBW.DBWIGroupSymbol();
                }
                return IGroupSymbol.dbwIGroupSymbol;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal int AddNewIGroupSymbol(int SymbolID, int InvestorGroupID)
        {
            int Result = -1;
            Result = IGroupSymbol.DBWIGroupSymbolInstance.AddNewIGroupSymbol(SymbolID, InvestorGroupID);
            if (Result > 0)
            {
                Business.IGroupSymbol newIGroupSymbol = new IGroupSymbol();
                newIGroupSymbol.IGroupSymbolID = Result;
                newIGroupSymbol.SymbolID = SymbolID;
                newIGroupSymbol.InvestorGroupID = InvestorGroupID;

                if (Business.Market.IGroupSymbolList == null)
                    Business.Market.IGroupSymbolList = new List<IGroupSymbol>();

                Business.Market.IGroupSymbolList.Add(newIGroupSymbol);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        internal bool DeleteIGroupSymbolByIGroupSymbolID(int IGroupSymbolID)
        {
            bool Result = false;
            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].IGroupSymbolID == IGroupSymbolID)
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            //DELETE IGROUPSYMBOL CONFIG
                            if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig != null)
                            {
                                int countConfig = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count;
                                for (int j = 0; j < countConfig; j++)
                                {
                                    TradingServer.Facade.FacadeDeleteIGroupSymbolConfig(Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].SecondParameterID);
                                    Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.RemoveAt(j);
                                }
                            }

                            bool ResultDelete = IGroupSymbol.DBWIGroupSymbolInstance.DeleteIGroupSymbolByIGroupSymbolID(IGroupSymbolID);
                            Business.Market.IGroupSymbolList.RemoveAt(i);

                            ts.Complete();

                            Result = true;

                            break;
                        }
                    }
                }
            }
            Result = IGroupSymbol.DBWIGroupSymbolInstance.DeleteIGroupSymbolByIGroupSymbolID(IGroupSymbolID);
            return Result;
        }

        #region Process Data In Class Market With IGroupSymbol List
        /// <summary>
        /// Get IGroupSymbol By Investor GroupID , Get Data In List IGroupSymbol In Of Class Market
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public List<Business.IGroupSymbol> GetIGroupSymbolByInvestorGroup(int InvestorGroupID)
        {
            List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();            

            #region Process Get IGroupSymbol In IGroupSymbol List With InvestorGroupID
            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].InvestorGroupID == InvestorGroupID)
                    {
                        Result.Add(Business.Market.IGroupSymbolList[i]);
                    }
                }
            }
            #endregion

            return Result;
        }
        #endregion
    }
}
