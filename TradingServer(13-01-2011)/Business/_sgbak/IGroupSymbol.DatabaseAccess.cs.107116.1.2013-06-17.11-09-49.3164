using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class IGroupSymbol
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IGroupSymbol> GetAllIGroupSymbol()
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.GetAllIGroupSymbol();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        internal Business.IGroupSymbol GetIGroupSymbolByID(int IGroupSymbolID)
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.GetIGroupSymbolByIGroupSymbolID(IGroupSymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSymbol> GetIGroupSymbolBySymbolID(int SymbolID)
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.GetIGroupSymbolBySymbolID(SymbolID);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.IGroupSymbol> GetIGroupSymbolByInvestorGroupID(int InvestorGroupID)
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.GetIGroupSymbolByInvestorGroupID(InvestorGroupID);
        }

        /// <summary>
        /// Delete IGroupSymbol By Symbol ID
        /// </summary>
        /// <param name="SymbolID">int SymbolID</param>
        internal bool DeleteIGroupSymbolBySymbolID(int SymbolID)
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.DeleteIGroupSymbolByIGroupSymbolID(SymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        internal bool DeleteIGroupSymbolByInvestorGroupID(int InvestorGroupID)
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.DeleteIGroupSymbolByInvestorGroupID(InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <param name="SymbolID"></param>
        /// <param name="InvestorGroupID"></param>
        internal bool UpdateIGroupSymbol(int IGroupSymbolID, int SymbolID, int InvestorGroupID)
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.UpdateIGroupSymbol(IGroupSymbolID, SymbolID, InvestorGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalIGroupSymbol()
        {
            return IGroupSymbol.DBWIGroupSymbolInstance.CountIGroupSymbol();
        }
    }
}
