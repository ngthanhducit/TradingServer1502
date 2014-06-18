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
        public static List<Business.IGroupSymbol> FacadeGetAllIGroupSymbol()
        {
            return Facade.IGroupSymbolInstance.GetAllIGroupSymbol();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        public static Business.IGroupSymbol FacadeGetIGroupSymbolByIGroupSymbolID(int IGroupSymbolID)
        {
            return Facade.IGroupSymbolInstance.GetIGroupSymbolByID(IGroupSymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSymbol> FacadeGetIGroupSymbolBySymbolID(int SymbolID)
        {
            return Facade.IGroupSymbolInstance.GetIGroupSymbolBySymbolID(SymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public static int FacadeAddNewIGroupSymbol(int SymbolID, int InvestorGroupID)
        {
            return Facade.IGroupSymbolInstance.AddNewIGroupSymbol(SymbolID, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <param name="SymbolID"></param>
        /// <param name="InvestorGroupID"></param>
        public static bool FacadeUpdateIGroupSymbol(int IGroupSymbolID, int SymbolID, int InvestorGroupID)
        {
            return Facade.IGroupSymbolInstance.UpdateIGroupSymbol(IGroupSymbolID, SymbolID, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIGroupSymbol(int IGroupSymbolID)
        {
            return Facade.IGroupSymbolInstance.DeleteIGroupSymbolByIGroupSymbolID(IGroupSymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<Business.IGroupSymbol> FacadeGetIGroupSymbolByInvestorGroup(int InvestorGroupID)
        {
            return Facade.IGroupSymbolInstance.GetIGroupSymbolByInvestorGroup(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountIGroupSymbol()
        {
            return Facade.IGroupSymbolInstance.CountTotalIGroupSymbol();
        }
    }
}
