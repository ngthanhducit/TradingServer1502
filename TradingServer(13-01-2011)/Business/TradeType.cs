using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class TradeType
    {
        #region Create Instance Class DBWTradeCommand
        private static DBW.DBWCommandType dbwCommandType;
        private static DBW.DBWCommandType DBWCommandTypeInstance
        {
            get
            {
                if (TradeType.dbwCommandType == null)
                {
                    TradeType.dbwCommandType = new DBW.DBWCommandType();
                }
                return TradeType.dbwCommandType;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public TradeType()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.TradeType> GetAllTradeType()
        {
            return TradeType.DBWCommandTypeInstance.GetAllTradeType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        internal string GetTypeNameByTypeID(int TypeID)
        {
            string Result = string.Empty;
            switch (TypeID)
            { 
                case 1:
                    Result = "BUY";
                    break;
                case 2:
                    Result = "SELL";
                    break;
                case 3:
                    Result = "UP BINARY";
                    break;
                case 4:
                    Result = "DOWN BINARY";
                    break;
                case 5:
                    Result = "BUY OPTION";
                    break;
                case 6:
                    Result = "SELL OPTION";
                    break;
                case 7:
                    Result = "BUY LIMIT";
                    break;  
                case 8:
                    Result = "SELL LIMIT";
                    break;
                case 9:
                    Result = "BUY STOP";
                    break;
                case 10:
                    Result = "SELL STOP";
                    break;
                case 11:
                    Result = "BUY FUTURE";
                    break;
                case 12:
                    Result = "SELL FUTURE";
                    break;
                case 13:
                    Result = "ADD DEPOSIT";
                    break;
                case 14:
                    Result = "WITHDRAWALS";
                    break;
                case 15:
                    Result = "CREDIT IN";
                    break;
                case 16:
                    Result = "CREADIT OUT";
                    break;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        internal string ConvertTypeIDToName(int typeID)
        {
            string result = string.Empty;
            switch (typeID)
            {
                case 1:
                    result = "Buy";
                    break;
                case 2:
                    result = "Sell";
                    break;
                case 7:
                    result = "Buy limit";
                    break;
                case 8:
                    result = "Sell limit";
                    break;
                case 9:
                    result = "Buy stop";
                    break;
                case 10:
                    result = "Sell stop";
                    break;
                case 11:
                    result = "Buy future";
                    break;
                case 12:
                    result = "Sell future";
                    break;
                case 13:
                    result = "Deposit";
                    break;
                case 14:
                    result = "Withdrawals";
                    break;
                case 15:
                    result = "Credit in";
                    break;
                case 16:
                    result = "Credit out";
                    break;
                case 17:
                    result = "Buy stop";
                    break;
                case 18:
                    result = "Sell stop";
                    break;
                case 19:
                    result = "Buy limit";
                    break;
                case 20:
                    result = "Sell limit";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        internal bool GetIsBuyByTypeID(int typeID)
        {
            bool result = false;
            if (typeID == 1 || typeID == 3 || typeID == 5 || typeID == 7 || typeID == 9 || typeID == 11)
            {
                result = true;
            }
            else if (typeID == 2 || typeID == 4 || typeID == 6 || typeID == 8 || typeID == 10 || typeID == 12)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalCommandType()
        {
            return TradeType.DBWCommandTypeInstance.CountCommandType();
        }
    }
}
