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
        public static List<Business.Symbol> FacadeGetAllSymbol()
        {
            return Facade.SymbolInstance.GetAllSymbol();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        public static List<Business.Symbol> FacadeGetSymbolBySecurityID(int SecurityID)
        {
            return Facade.SymbolInstance.GetSymbolBySecurityID(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSymbol"></param>
        /// <returns></returns>
        public static int FacadeAddNewSymbol(int SecurityID, int RefSymbolID, int MarketAreaID, string SymbolName)
        {
            return Facade.SymbolInstance.AddNewSymbol(SecurityID, RefSymbolID, MarketAreaID, SymbolName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="MarketAreaID"></param>
        /// <param name="SymbolName"></param>
        public static bool FacadeUpdateSymbol(int SymbolID, int SecurityID, int RefSymbolID, int MarketAreaID, string SymbolName)
        {
            return Facade.SymbolInstance.UpdateSymbol(SymbolID, SecurityID, RefSymbolID, MarketAreaID, SymbolName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteSymbol(int SymbolID)
        {
            return Facade.SymbolInstance.DeleteSymbol(SymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListIGroupSecurit"></param>
        /// <returns></returns>
        public static List<Business.Symbol> FacadeGetSymbolByIGroupSecurity(List<Business.IGroupSecurity> ListIGroupSecurit)
        {
            return Facade.SymbolInstance.GetListSymbolBySecurityID(ListIGroupSecurit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static Business.Symbol FacadeGetSymbolConfig(string symbolName)
        {
            return Facade.SymbolInstance.GetSymbolConfig(symbolName);
        }

        /// <summary>
        /// FACADE GET SYMBOL NAME BY SYMBOL ID IN CLASS MARET
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        public static string FacadeGetSymbolNameBySymbolID(int SymbolID)
        {
            return Facade.SymbolInstance.GetSymbolNameBySymbolID(SymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountSymbol()
        {
            return Facade.SymbolInstance.CountTotalSymbol();
        }
    }
}
