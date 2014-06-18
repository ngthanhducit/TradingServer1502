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
        public static int FacadeAddNewInvestorGroupConfig(List<Business.ParameterItem> objParameterItem)
        {
            return Facade.ParameterItemInstance.AddNewInvestorGroupConfig(objParameterItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorGroupConfig"></param>
        public static bool FacadeUpdateInvestorGroupConfig(Business.ParameterItem objInvestorGroupConfig)
        {
            return Facade.ParameterItemInstance.UpdateInvestorGroupConfig(objInvestorGroupConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listParamterItem"></param>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool FacadeUpdateInvestorGroupConfig(List<Business.ParameterItem> listParamterItem, string ipAddress, string code)
        {
            return Facade.ParameterItemInstance.UpdateInvestorGroupConfig(listParamterItem, code, ipAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupConfigID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteInvestorGroupConfig(int InvestorGroupConfigID)
        {
            return Facade.ParameterItemInstance.DeleteInvestorGroupConfig(InvestorGroupConfigID);
        }        
    }
}
