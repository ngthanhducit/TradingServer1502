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
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool FacadeCheckStatusSymbol(string name)
        {
            return Facade.MarketInstance.CheckStatusSymbol(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static bool FacadeCheckManaulStopOut(int groupID)
        {
            return Facade.MarketInstance.CheckManualStopOut(groupID);
        }
    }
}
