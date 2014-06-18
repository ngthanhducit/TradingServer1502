using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer
{
    public static partial class Facade
    {        
        /// <summary>
        /// FACADE ADD IGROUPSECURITY CONFIG
        /// </summary>
        /// <param name="ListPameterItem"></param>
        /// <returns></returns>
        public static int FacadeAddIGroupSecurityConfig(List<Business.ParameterItem> ListPameterItem)
        {
            return Facade.ParameterItemInstance.AddNewIGroupSecurityConfig(ListPameterItem);
        }

        /// <summary>
        /// FACADE UPDATE IGROUPSECURITY CONFIG
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        public static bool FacadeUpdateIGroupSecurityConfig(Business.ParameterItem objParameterItem)
        {
            return Facade.ParameterItemInstance.UpdateIGroupSecurityConfig(objParameterItem);
        }

        /// <summary>
        /// DELETE IGROUP SECURITY CONFIG BY IGROUP SECURITY ID IN DATABASE
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIGroupSecurityConfigByIGroupSecurityID(int IGroupSecurityID)
        {
            return Facade.ParameterItemInstance.DeleteIGroupSecurityConfig(IGroupSecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurityID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteIGroupSecurityConfigByIGroupSecurity(int IGroupSecurityID)
        {
            return Facade.ParameterItemInstance.DeleteIGroupSecurityConfigByIGroupSecurityID(IGroupSecurityID);
        }        
    }
}
