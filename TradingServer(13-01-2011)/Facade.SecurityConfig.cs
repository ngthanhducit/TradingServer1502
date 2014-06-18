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
        public static List<Business.ParameterItem> FacadeGetAllSecurityConfig()
        {
            return Facade.SecurityInstance.GetAllSecurityConfig();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="NumValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="DateValue"></param>
        /// <param name="BoolValue"></param>
        /// <returns></returns>
        public static int FacadeAddSecurityConfig(List<Business.ParameterItem> ListParameterItem)
        {
            return Facade.SecurityInstance.AddListSecurityConfig(ListParameterItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityConfigID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="CollectionValue"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="NumValue"></param>
        /// <param name="StringValue"></param>
        /// <param name="DateValue"></param>
        /// <param name="BoolValue"></param>
        public static bool FacadeUpdateSecurityConfig(Business.ParameterItem objParameterItem)
        {
            return Facade.SecurityInstance.UpdateSecurityConfig(objParameterItem.ParameterItemID, objParameterItem.SecondParameterID, -1, objParameterItem.Name, objParameterItem.Code, objParameterItem.NumValue, objParameterItem.StringValue, objParameterItem.DateValue, objParameterItem.BoolValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityConfigID"></param>
        public static bool FacadeDeleteSecurityConfigBySecurityConfigID(int SecurityConfigID)
        {
            return Facade.SecurityInstance.DeleteSecurityConfig(SecurityConfigID);
        }
    }
}
