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
        public static List<Business.ParameterItem> FacadeGetAllMarketConfig()
        {
            return Facade.ParameterItemInstance.GetAllMarketConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketConfigID"></param>
        /// <returns></returns>
        public static Business.ParameterItem FacadeGetAllMarketConfigByID(int MarketConfigID)
        {
            return Facade.ParameterItemInstance.GetMarketConfigByID(MarketConfigID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static int FacadeAddNewMarketConfig(int CollectionValue, string Name, string Code, int BoolValue,
                                            string StringValue, string NumValue, DateTime DateValue)
        {
            return Facade.ParameterItemInstance.AddNewMarketConfig(CollectionValue, Name, Code, BoolValue, StringValue, NumValue, DateValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketConfigID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="BoolValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="NumValue"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static bool FacadeUpdateMarketConfig(Business.ParameterItem objParameterItem)
        {
            return Facade.ParameterItemInstance.UpdateMarketConfig(objParameterItem.ParameterItemID, -1, objParameterItem.Name,
                objParameterItem.Code, objParameterItem.BoolValue, objParameterItem.StringValue, objParameterItem.NumValue, objParameterItem.DateValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMarketConfig"></param>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool FacadeUpdateMarketConfig(List<Business.ParameterItem> listMarketConfig, string ipAddress, string code)
        {
            return Facade.ParameterItemInstance.UpdateMarketConfig(listMarketConfig, ipAddress, code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        public static bool FacadeDeleteMarketConfig(int marketConfigID)
        {
            return Facade.ParameterItemInstance.DeleteMarketConfig(marketConfigID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountMarketConfig()
        {
            return Facade.ParameterItemInstance.CountTotalMarketConfig();
        }
    }
}
