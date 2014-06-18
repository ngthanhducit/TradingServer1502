using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Symbol
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="lots"></param>
       /// <param name="longPosition"></param>
       /// <param name="shortPosition"></param>
       /// <param name="closePriceBid"></param>
       /// <param name="closePriceAsk"></param>
       /// <param name="contractSize"></param>
       /// <param name="digits"></param>
       /// <param name="day"></param>
       /// <param name="command"></param>
       /// <returns></returns>
        internal double Points(double lots, double longPosition, double shortPosition, double closePriceBid, double closePriceAsk, double contractSize, int digits, int day, int command)
        {
            double result = 0;
            double pointsize = ConvertDigitToPip(digits) * contractSize;
            if (command == 1)
            {
                result = lots * longPosition * (pointsize / closePriceBid);
            }
            else
            {
                result = lots * shortPosition * (pointsize/closePriceAsk);
            }
            return result * day;
        }

        /// <summary>
        /// 0 not declair, 1 buy, 2 sell
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static int ConvertCommandIsBuySell(int command)
        {
            int result = 0;
            if (command == 1 | command == 5 | command == 7 | command == 9 | command == 11 | command == 17 | command == 19)
            {
                result = 1;
            }
            else if (command == 2 | command == 6 | command == 8 | command == 10 | command == 12 | command == 18 | command == 20)
            {
                result = 2;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        internal double ConvertDigitToPip(int digits)
        {
            return 1 / Math.Pow(10, digits);
        }
        /// <summary>
        /// Note Use Function MoneyInMarginCurrency Convert If Symbol != XXXUSD
        /// </summary>
        /// <param name="lots"></param>
        /// <param name="longPosition"></param>
        /// <param name="shortPosition"></param>
        /// <param name="closePriceBid"></param>
        /// <param name="contractSize"></param>
        /// <param name="day"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        internal double Interest(double lots, double longPosition, double shortPosition, double closePriceBid, double contractSize, int day, int command)
        {
            double result = 0;
            if(command ==1)
            {
                result = lots * (longPosition / 100) / 360;
            }
            else
            {
                result = lots * (shortPosition / 100) / 360;
            }
            return result * closePriceBid * day * contractSize;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lots"></param>
        /// <param name="longPosition"></param>
        /// <param name="shortPosition"></param>
        /// <param name="day"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        internal double Money(double lots, double longPosition, double shortPosition, int day, int command)
        {
            double result = 0;
            if (command == 1)
            {
                result = lots * longPosition;
            }
            else
            {
                result = lots * shortPosition;
            }
            return result * day;
        }
        /// <summary>
        /// Need Convert To USD
        /// </summary>
        /// <param name="lots"></param>
        /// <param name="longPosition"></param>
        /// <param name="shortPosition"></param>
        /// <param name="day"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        internal double MoneyInMarginCurrency(double lots, double longPosition, double shortPosition, int day, int command)
        {
            double result = 0;
            if (command == 1)
            {
                result = lots * longPosition;
            }
            else
            {
                result = lots * shortPosition;
            }
            return result * day;
        }

        /// <summary>
        /// Use for swap
        /// </summary>
        /// <param name="valueNeedConvert"></param>
        /// <param name="priceCurrency"></param>
        /// <returns></returns>
        internal double ConvertCurrencyToUSD(string symbolCurrency, double valueNeedConvert, double priceCurrency, int digitSwap)
        {
            double result = valueNeedConvert;
            if (symbolCurrency == "" | priceCurrency == 0) return result;
            else
            {
                if (symbolCurrency.IndexOf("USD") == 0)
                {
                    result = valueNeedConvert / priceCurrency;
                }
                else
                {
                    result = valueNeedConvert * priceCurrency;
                }
            }
            result = Math.Round(result, digitSwap);
            return result;
        }
    }
}
