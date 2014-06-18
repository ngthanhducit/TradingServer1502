using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Security
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Business.Security> GetAllSecurity()
        {
            List<Business.Security> result = new List<Business.Security>();
            result = Security.SecurityInstance.GetAllSecurity();

            //if (result != null)
            //{
            //    for (int i = 0; i < result.Count; i++)
            //    {
            //        result[i].ParameterItems = Security.SecurityConfigInstance.GetSecurityConfigBySecurityID(result[i].SecurityID);
            //        result[i].SymbolGroup = Facade.FacadeGetSymbolBySecurityID(result[i].SecurityID);
            //    }
            //}
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Business.ParameterItem> GetAllSecurityConfig()
        {
            List<Business.ParameterItem> result = new List<Business.ParameterItem>();
            result = Security.SecurityConfigInstance.GetAllSecurityConfig();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newParameterItem"></param>
        /// <returns></returns>
        public int AddSecurityConfig(Business.ParameterItem newParameterItem)
        {
            int result = Security.SecurityConfigInstance.AddSecurityConfig(newParameterItem.SecondParameterID, -1, newParameterItem.Name,
                                newParameterItem.Code, newParameterItem.NumValue, newParameterItem.StringValue,
                                newParameterItem.DateValue, newParameterItem.BoolValue);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalSecurity()
        {
            return Security.SecurityInstance.CountSecurity();
        }
    }
}
