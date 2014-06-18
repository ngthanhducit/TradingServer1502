using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.NJ4XConnectSocket
{
    public class MapNJ4X
    {
        #region CREATE INSTANCE MAPNJ4X
        private static NJ4XConnectSocket.MapNJ4X _instance;
        public static NJ4XConnectSocket.MapNJ4X Instance
        {
            get
            {
                if (NJ4XConnectSocket.MapNJ4X._instance == null)
                    NJ4XConnectSocket.MapNJ4X._instance = new MapNJ4X();

                return NJ4XConnectSocket.MapNJ4X._instance;
            }
        }
        #endregion

        /// <summary>
        /// NJ4X
        /// </summary>
        /// <param name="code"></param>
        /// <param name="symbol"></param>
        /// <param name="cmd"></param>
        /// <param name="volume"></param>
        /// <param name="price"></param>
        /// <param name="slippage"></param>
        /// <param name="sl"></param>
        /// <param name="tp"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public string MapOrderSend(string code, string symbol, int cmd, double volume, double price, int slippage, double sl,
                                    double tp, string comment)
        {
            return "OrderSend$" + code + "{" + symbol + "{" + cmd + "{" + volume + "{" + price + "{" + slippage + "{" + sl + "{" +
                        tp + "{" + comment;
        }

        /// <summary>
        /// MQL4
        /// </summary>
        /// <param name="code"></param>
        /// <param name="symbol"></param>
        /// <param name="cmd"></param>
        /// <param name="volume"></param>
        /// <param name="price"></param>
        /// <param name="slippage"></param>
        /// <param name="sl"></param>
        /// <param name="tp"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        //public string MapOrderSend(string code, string symbol, int cmd, double volume, double price, int slippage, double sl,
        //                            double tp, string comment, string password)
        //{
        //    //0: Buy - 1: Sell - 2: BuyLimit - 3: SellLimit - 4: BuyStop - 5: SellStop
        //    return "OrderSend$" + code + "{" + symbol + "{" + this.MapTradeOperation(cmd) + "{" + volume + "{" + price + "{" + slippage + "{" + sl + "{" +
        //                tp + "{" + comment + "{" + password; 
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string MapResetPassword(string code, string password)
        {
            return "ResetPassword$" + code + "{" + password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int MapTradeOperation(int cmd)
        {
            int result = -1;

            switch (cmd)
            {
                case 1:
                    result = 0;
                    break;

                case 2:
                    result = 1;
                    break;

                case 9:
                    result = 4;
                    break;

                case 7:
                    result = 2;
                    break;

                case 10:
                    result = 5;
                    break;

                case 8:
                    result = 3;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="price"></param>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <returns></returns>
        public string MapOrderModify(int ticket, double price, double stopLoss, double takeProfit, string code, string password)
        {
            return "OrderModify$" + ticket + "{" + price + "{" + stopLoss + "{" + takeProfit + "{" + code + "{" + password;
        }

        /// <summary>
        /// 
        /// </summary>exp
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string MapConnect(string userName, string password)
        {
            return "Connect$" + userName + "{" + password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>7v
        /// <returns></returns>
        public string MapOrderClose(int ticket, double lots, double price, string code, string symbol, string password)
        {
            return "OrderClose$" + ticket + "{" + lots + "{" + price + "{" + code + "{" + symbol + "{" + password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>7v
        /// <returns></returns>
        public string MapOrderClose(int ticket, double lots, double price, string code, string symbol)
        {
            return "OrderClose$" + ticket + "{" + lots + "{" + price + "{" + code + "{" + symbol;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string MapOrderDelete(int ticket, string code, string password)
        {
            return "OrderDelete$" + ticket + "{" + code + "{" + password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string MapOrderDelete(int ticket, string coded)
        {
            return "OrderDelete$" + ticket + "{" + coded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string MapDisconnectNJ4X(string userName, string pass)
        {
            return "DisConnect$" + userName + "{" + pass;
        }
    }
}
