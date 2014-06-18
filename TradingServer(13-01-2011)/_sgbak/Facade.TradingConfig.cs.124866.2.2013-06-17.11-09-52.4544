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
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        public static int FacadeAddNewTradingConfig(List<Business.ParameterItem> objParameterItem)
        {
            return Facade.ParameterItemInstance.AddNewSymbolConfig(objParameterItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TradingConfigID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteTradingConfig(int TradingConfigID)
        {
            return Facade.ParameterItemInstance.DeleteTradingConfig(TradingConfigID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameterItem"></param>
        public static bool FacadeUpdateTradingConfig(Business.ParameterItem objParameterItem)
        {
            return Facade.ParameterItemInstance.UpdateTradingConfig(objParameterItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listParameterItem"></param>
        /// <returns></returns>
        public static bool FacadeUpdateTradingConfig(List<Business.ParameterItem> listParameterItem,string code,string ipAddress)
        {
            return Facade.parameterItem.UpdateTradingConfig(listParameterItem, code, ipAddress);
        }
    }
}
