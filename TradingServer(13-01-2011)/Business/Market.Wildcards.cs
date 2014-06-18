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
        /// <returns></returns>
        private bool IsWildCards(string value)
        {
            bool result = false;

            switch (value)
            {
                case "*":
                    result = true;
                    break;
            }

            return result;
        }
    }
}
