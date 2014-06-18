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
        public static List<Business.Security> FacadeGetAllSecurity()
        {
            return Facade.SecurityInstance.GetAllSecurity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public static int FacadeAddSecurity(string Name, string Description, int MarketAreaID)
        {
            return Facade.SecurityInstance.AddSecurity(Name, Description, MarketAreaID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        public static bool FacadeUpdateSecurity(int SecurityID, string Name, string Description, int MarketAreaID)
        {
            return Facade.SecurityInstance.UpdateSecurity(SecurityID, Name, Description, MarketAreaID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        public static bool FacadeUpdateSecurity(int SecurityID, string Name, string Description, int MarketAreaID, string ipAddress, string code)
        {
            return Facade.SecurityInstance.UpdateSecurity(SecurityID, Name, Description, MarketAreaID, ipAddress, code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        public static bool FacadeDeleteSecurity(int SecurityID)
        {
            return Facade.SecurityInstance.DeleteSecurity(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountSecurity()
        {
            return Facade.SecurityInstance.CountTotalSecurity();
        }
    }
}
