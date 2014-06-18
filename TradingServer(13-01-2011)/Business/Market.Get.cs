using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="igroupSecurity"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetExecutionType(Business.IGroupSecurity igroupSecurity, string code)
        {
            string result = string.Empty;
            if (igroupSecurity != null)
            {
                if (igroupSecurity.IGroupSecurityConfig != null)
                {
                    int count = igroupSecurity.IGroupSecurityConfig.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (igroupSecurity.IGroupSecurityConfig[i].Code == code)
                        {
                            result = igroupSecurity.IGroupSecurityConfig[i].StringValue;
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
