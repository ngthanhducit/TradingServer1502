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
        public static List<Business.ParameterItem> FacadeGetIGroupSymbolConfig()
        {
            return Facade.ParameterItemInstance.GetAllIGroupSymbolConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        public static List<Business.ParameterItem> FacadeGetIGroupSymbolConfigByIGroupSymbolID(int IGroupSymbolID)
        {
            return Facade.ParameterItemInstance.GetIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolConfigID"></param>
        /// <returns></returns>
        public static Business.ParameterItem FacadeGetIGroupSymbolConfigByID(int IGroupSymbolConfigID)
        {
            return Facade.ParameterItemInstance.GetIGroupSymbolConfigByID(IGroupSymbolConfigID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListParameter"></param>
        /// <returns></returns>
        public static int FacadeAddIGroupSymbolConfig(List<Business.ParameterItem> ListParameter)
        {
            return Facade.ParameterItemInstance.AddNewIGroupSymbolConfig(ListParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameter"></param>
        /// <returns></returns>
        public static bool FacadeUpdateIGroupSymbolConfig(Business.ParameterItem objParameter)
        {
            return Facade.ParameterItemInstance.UpdateIGroupSymbolConfig(objParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolConfigID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIGroupSymbolConfig(int IGroupSymbolConfigID)
        {
            return Facade.ParameterItemInstance.DeleteIGroupSymbolConfig(IGroupSymbolConfigID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIGroupSymbolConfigByIGroupSymbolID(int IGroupSymbolID)
        {
            return Facade.ParameterItemInstance.DeleteIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);
        }        
    }
}
