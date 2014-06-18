using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Symbol
    {
        internal static Dictionary<string, double> ListBid = new Dictionary<string, double>();
        internal static Dictionary<string, double> ListAsk = new Dictionary<string, double>();
        internal static Dictionary<string, int> ListLoop = new Dictionary<string, int>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTick"></param>
        /// <returns></returns>
        internal Business.Tick TickFilterSession(Business.Tick objTick)
        {
            throw new NotImplementedException();
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTick"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        internal Business.Tick FormatDigit(Business.Tick objTick, int digits)
        {
            string bid,ask = "";
            string formular = "{0:0.0000}";
            switch (digits)
            {
                case 0:
                    formular = "{0:0}";
                    break;
                case 1:
                    formular = "{0:0.0}";
                    break;
                case 2:
                    formular = "{0:0.00}";                    
                    break;
                case 3:
                    formular = "{0:0.000}"; 
                    break;
                case 4:
                    formular = "{0:0.0000}"; 
                    break;
                case 5:
                    formular = "{0:0.00000}"; 
                    break;
            }
            bid = String.Format(formular, objTick.Bid);
            ask = String.Format(formular, objTick.Ask);
            objTick.Bid = double.Parse(bid);
            objTick.Ask = double.Parse(ask);
            return objTick;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTick"></param>
        internal void ArchiveTick(Business.Tick newTick)
        {            
            string symbol = newTick.SymbolName;
            if (ListBid.ContainsKey(symbol)) ListBid[symbol] = newTick.Bid;
            else ListBid.Add(symbol, newTick.Bid);
            if (ListAsk.ContainsKey(symbol)) ListAsk[symbol] = newTick.Ask;
            else ListAsk.Add(symbol, newTick.Ask);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTick"></param>
        /// <param name="filter"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        internal bool FiltrationLevel(Business.Tick newTick, int filter, int loop)
        {
            if(filter==0) return true;
            double ex1, ex2, ex3 = 0;
            string symbol = newTick.SymbolName;
            if (ListBid.ContainsKey(symbol) && ListAsk.ContainsKey(symbol))
            {
                ex1 = newTick.Ask - newTick.Bid;
                ex2 = newTick.Ask - ListAsk[symbol];
                ex3 = newTick.Bid - ListBid[symbol];
                //ListBid[symbol] = newTick.Bid;
                //ListAsk[symbol] = newTick.Ask;
                if (ListLoop.ContainsKey(symbol))
                {
                    if (ex1 > filter | ex2 > filter | ex3 > filter)
                    {
                        if (ListLoop[symbol] > loop)
                        {
                            ListLoop[symbol] = 0;
                            return true;
                        }
                        else
                        {
                            ListLoop[symbol] += 1;
                            return false;
                        }
                    }
                    else
                    {
                        ListLoop[symbol] = 0;
                        return true;
                    }
                }
                else
                {
                    ListLoop.Add(symbol, 0);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTick"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        internal bool AutomaticLimit(Business.Tick newTick, double percent)
        {
            double ex1, ex2 = 0;
            string symbol = newTick.SymbolName;
            if (ListBid.ContainsKey(symbol) && ListAsk.ContainsKey(symbol))
            {
                ex1 = Math.Abs(newTick.Ask - ListAsk[symbol])/ListAsk[symbol];
                ex2 = Math.Abs(newTick.Bid - ListBid[symbol])/ListBid[symbol];
                if (ex1 > percent | ex2 > percent)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTick"></param>
        /// <param name="spreadDefault"></param>
        /// <param name="spreadBid"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        internal Business.Tick SpreadPrices(Business.Tick newTick, double spreadDefault, double spreadBid, int digits,bool applySpread)
        {
            if (applySpread == true)
            {
                if (spreadDefault >= 0)
                {
                    double bid = (newTick.Ask + newTick.Bid) / 2;
                    bid = (double)Math.Round((decimal)bid, digits, MidpointRounding.AwayFromZero);
                    double temp = ConvertNumberPip(digits, spreadBid);
                    bid += temp;
                    newTick.Bid = bid;
                    newTick.Ask = bid + ConvertNumberPip(digits, spreadDefault);
                }
            }
            return newTick;
        }

        internal Business.Tick CalculationTickSize(Business.Tick newTick, double spreadDefault, double tickSize, int digits,bool applySpread)
        {
            if (tickSize > 0)
            {
                double tempa, tempb;
                int tempe = CheckLengthAfterDigit(tickSize);

                if (tempe > digits)
                {
                    tempa = tickSize * Math.Pow(10, tempe);
                    tempb = newTick.Bid * Math.Pow(10, tempe);
                }
                else
                {
                    tempa = tickSize * Math.Pow(10, digits);
                    tempb = newTick.Bid * Math.Pow(10, digits);
                }
                // Convert 0.0000000000001
                tempa = Math.Round(tempa, 0);
                tempb = Math.Round(tempb, 0);

                double num = tempb / tempa;
                num = Math.Round(num, 0);
                tempb = tempa * num;

                if (tempe > digits)
                {
                    tempb = ConvertNumberPip(tempe, tempb);
                }
                else
                {
                    tempb = ConvertNumberPip(digits, tempb);
                }
                newTick.Bid = Math.Round(tempb, digits);
                if (applySpread == true)
                {
                    newTick.Ask = newTick.Bid + ConvertNumberPip(digits, spreadDefault);
                }
                else
                {
                    if (tempe > digits)
                    {
                        tempb = newTick.Ask * Math.Pow(10, tempe);
                    }
                    else
                    {
                        tempb = newTick.Ask * Math.Pow(10, digits);
                    }
                    // Convert 0.0000000000001
                    tempb = Math.Round(tempb, 0);

                    num = tempb / tempa;
                    num = Math.Round(num, 0);
                    tempb = tempa * num;

                    if (tempe > digits)
                    {
                        tempb = ConvertNumberPip(tempe, tempb);
                    }
                    else
                    {
                        tempb = ConvertNumberPip(digits, tempb);
                    }
                    newTick.Ask = Math.Round(tempb, digits);
                }
            }
            return newTick;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal double ConvertNonDigit(double value)
        {
            string temp = value.ToString();
            string[] kq = temp.Split('.');
            if (kq.Length > 1)
            {
                for (int i = 0; i < kq[1].Length; i++)
                {
                    value *= 10;
                }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int CheckLengthAfterDigit(double value)
        {
            string temp = value.ToString();
            string[] kq;
            if (temp.IndexOf("E") > 0)
            {
                temp = value.ToString("F10");
                kq = temp.Split('.');
                int i = kq[1].Length;
                string letter = "";
                while (i > 0)
                {
                    letter = kq[1].Substring(i - 1);
                    if (letter == "0")
                    {
                        kq[1] = kq[1].Substring(0, i - 1);
                        i = kq[1].Length;
                    }
                    else
                    {
                        i = 0;
                    }
                }
                return kq[1].Length;
            }
            else
            {
                kq = temp.Split('.');
                if (kq.Length <= 1) return 0;
                else return kq[1].Length;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="spreadDefault"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        internal double CreateAskPrices(int digits, int spreadDefferent,double ask)
        {
            return ask + ConvertNumberPip(digits, spreadDefferent);
        }

        /// <summary>
        /// CheckLimitAndStop and CheckFreeze (lsLevel)
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="command"></param>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <param name="lsLevel">SL/TP Level</param>
        /// <param name="digits"></param>
        /// <returns></returns>
        internal bool CheckLimitAndStop(string symbol, int command, double stopLoss, double takeProfit, int lsLevel, int digits,int spreadDiff)
        {
            //1 is buy, 2 is sell
            int valueCheck = ConvertCommandIsBuySell(command);
            if (valueCheck == 1)
            {
                if (takeProfit < stopLoss)
                {
                    if (takeProfit != 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (((stopLoss + ConvertNumberPip(digits, lsLevel)) <= ListBid[symbol]))
                        {
                            return true;
                        }
                        else return false;
                    }
                }
                else
                {
                    if (ListBid.ContainsKey(symbol))
                    {
                        if (((takeProfit - ConvertNumberPip(digits, lsLevel)) >= ListBid[symbol]) & ((stopLoss + ConvertNumberPip(digits, lsLevel)) <= ListBid[symbol]))
                        {
                            return true;
                        }
                        else return false;
                    }
                }
            }
            else if (valueCheck == 2 )
            {
                if (takeProfit > stopLoss)
                {
                    if (stopLoss != 0)
                    {
                        return false;
                    }
                    else 
                    {
                        if ((takeProfit + ConvertNumberPip(digits, lsLevel)) <= (ListAsk[symbol] + ConvertNumberPip(digits, spreadDiff)))
                        {
                            return true;
                        }
                        else return false;
                    }
                }
                else
                {
                    if (ListAsk.ContainsKey(symbol))
                    {
                        double askPrice = ListAsk[symbol] + ConvertNumberPip(digits, spreadDiff);
                        if (((stopLoss - ConvertNumberPip(digits, lsLevel)) >= askPrice) & ((takeProfit + ConvertNumberPip(digits, lsLevel)) <= askPrice))
                        {
                            return true;
                        }
                        else return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueCheck"></param>
        /// <param name="digits"></param>
        /// <param name="tickSize"></param>
        /// <returns></returns>
        internal bool CheckTickSizeAtOpenCommand(double valueCheck, int digits, double tickSize)
        {
            if (tickSize != 0)
            {
                double tempa, tempb;
                int tempe = CheckLengthAfterDigit(tickSize);
                if (tempe > digits)
                {
                    tempa = tickSize * Math.Pow(10, tempe);
                    tempb = valueCheck * Math.Pow(10, tempe);
                }
                else
                {
                    tempa = tickSize * Math.Pow(10, digits);
                    tempb = valueCheck * Math.Pow(10, digits);
                }
                // Convert 0.0000000000001
                tempa = Math.Round(tempa, 0);
                tempb = Math.Round(tempb, 0);
                double num = tempb % tempa;
                if (num == 0)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return true;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="Command"></param>
        /// <param name="OpenPrice"></param>
        /// <param name="LimitStopLevel"></param>
        /// <param name="Digit"></param>
        /// <returns></returns>
        internal bool CheckOpenPricePendingOrder(string Symbol, int Command, double OpenPrice, int limitLevel,int stopLevel, int Digit,int spreadDiff)
        {
            bool Result = false;
            if (OpenPrice == 0) return false;
            double LStopLevel = 0;
            switch (Command)
            {
                case 7:
                    {                        
                        LStopLevel = ConvertNumberPip(Digit, limitLevel);
                        double priceOpen = (ListAsk[Symbol] + ConvertNumberPip(Digit, spreadDiff)) - LStopLevel;
                        if (priceOpen > OpenPrice)
                            Result = true;
                    }
                    break;

                case 8:
                    {
                        LStopLevel = ConvertNumberPip(Digit, limitLevel);
                        double priceOpen = ListBid[Symbol] + LStopLevel;
                        if (priceOpen < OpenPrice)
                            Result = true;
                    }
                    break;

                case 9:
                    {
                        LStopLevel = ConvertNumberPip(Digit, stopLevel);
                        double priceOpen = (ListAsk[Symbol] + ConvertNumberPip(Digit, spreadDiff)) + LStopLevel;
                        if (priceOpen < OpenPrice)
                            Result = true;
                    }
                    break;

                case 10:
                    {
                        LStopLevel = ConvertNumberPip(Digit, stopLevel);
                        double priceOpen = ListBid[Symbol] - LStopLevel;
                        if (priceOpen > OpenPrice)
                            Result = true;
                    }
                    break;                

                case 17:
                    {
                        LStopLevel = ConvertNumberPip(Digit, stopLevel);
                        double priceOpen = (ListAsk[Symbol] + ConvertNumberPip(Digit, spreadDiff)) + LStopLevel;
                        if (priceOpen < OpenPrice)
                            Result = true;
                    }
                    break;

                case 18:
                    {
                        LStopLevel = ConvertNumberPip(Digit, stopLevel);
                        double priceOpen = ListBid[Symbol] - LStopLevel;
                        if (priceOpen > OpenPrice)
                            Result = true;
                    }
                    break;

                case 19:
                    {
                        LStopLevel = ConvertNumberPip(Digit, limitLevel);
                        double priceOpen = (ListAsk[Symbol] + ConvertNumberPip(Digit, spreadDiff)) - LStopLevel;
                        if (priceOpen > OpenPrice)
                            Result = true;
                    }
                    break;

                case 20:
                    {
                        LStopLevel = ConvertNumberPip(Digit, limitLevel);
                        double priceOpen = ListBid[Symbol] + LStopLevel;
                        if (priceOpen < OpenPrice)
                            Result = true;
                    }
                    break;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="command"></param>
        /// <param name="valueCheck"></param>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <param name="lsLevel"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        internal bool CheckLimitAndStopPendingOrder(string symbol, int command, double valueCheck, double stopLoss, double takeProfit, int lsLevel, int digits)
        {
            if (valueCheck == 0) return false;
            //1 is buy, 2 is sell
            int valueBuySell = Business.Symbol.ConvertCommandIsBuySell(command);
            if (valueBuySell == 1 )
            {
                if (takeProfit < stopLoss)
                {
                    if (takeProfit != 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (((stopLoss + ConvertNumberPip(digits, lsLevel)) <= valueCheck))
                        {
                            return true;
                        }
                        else return false;
                    }
                }
                else
                {
                    double temp1 = takeProfit - ConvertNumberPip(digits, lsLevel);
                    double temp2 = stopLoss + ConvertNumberPip(digits, lsLevel);
                    if (((takeProfit - ConvertNumberPip(digits, lsLevel)) >= valueCheck) & ((stopLoss + ConvertNumberPip(digits, lsLevel)) <= valueCheck))
                    {
                        return true;
                    }
                    else return false;
                }
            }
            else if (valueBuySell == 2 )
            {
                if (takeProfit > stopLoss)
                {
                    if (stopLoss != 0)
                    {
                        return false;
                    }
                    else
                    {
                        if ((takeProfit + ConvertNumberPip(digits, lsLevel)) <= valueCheck)
                        {
                            return true;
                        }
                        else return false;
                    }
                }
                else
                {
                    if (((stopLoss - ConvertNumberPip(digits, lsLevel)) >= valueCheck) & ((takeProfit + ConvertNumberPip(digits, lsLevel)) <= valueCheck))
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal double GetValueBid(string symbol)
        {
            double result=0;
            if (ListBid.ContainsKey(symbol))
            {
                result = ListBid[symbol];
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal double GetValueAsk(string symbol)
        {
            double result=0;
            if (ListAsk.ContainsKey(symbol))
            {
                result = ListAsk[symbol];
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="margin"></param>
        /// <param name="isBid"></param>
        /// <returns></returns>
        internal double ConvertCurrencyMarginToUSD(string symbol, double margin, bool isBid)
        {
            double result = 0;
            if (symbol.IndexOf("USD") > 0)
            {
                if (isBid) result = margin * GetValueBid(symbol);
                else result = margin * GetValueAsk(symbol);
            }
            else
            {
                if (isBid) result = margin / GetValueBid(symbol);
                else result = margin / GetValueAsk(symbol);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="value"></param>
        /// <param name="isBid"></param>
        /// <returns></returns>
        internal double ConvertCurrencyToUSD(string symbol, double value, bool isBid,double spreadDifferent,int digit)
        {
            double result = value;
            double temp = 0;
            double spread = 0;
            if (symbol == "") return result;
            else
            {
                spread = ConvertNumberPip(digit, spreadDifferent);
                if (symbol.IndexOf("USD") == 0)
                {
                    if (isBid) temp = GetValueBid(symbol);
                    else temp = GetValueAsk(symbol);                    
                    if (temp != 0) result = value / (temp + spread);
                    else result = 0;
                }
                else
                {
                    if (isBid) temp = GetValueBid(symbol);
                    else temp = GetValueAsk(symbol);
                    if (temp != 0) result = value * (temp + spread);
                    else result = 0;
                }
            }
            //if (symbol.IndexOf("USD") > 0) return value;

            ////Not complete
            //if (symbol.IndexOf("USD") == 0)
            //{
            //    if (isBid) result = value / GetValueBid(symbol);
            //    else result = value / GetValueAsk(symbol);                
            //}
            //else
            //{
            //    double currencyValue = FindSymbolUSDCurrency(symbol, isBid);
            //    if (currencyValue == 0)
            //    {
            //        currencyValue = FindSymbolCurrencyUSD(symbol, isBid);
            //        if (currencyValue == 0) return value;
            //        else result = value * currencyValue;
            //    }
            //    result = value / currencyValue;
            //}
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isBid"></param>
        /// <returns></returns>
        internal double FindSymbolUSDCurrency(string symbol, bool isBid)
        {
            string currency = symbol.Substring(3);
            double result = 0;
            if (currency == "USD")
            {
                if (isBid) result = GetValueBid(symbol);
                else result = GetValueAsk(symbol);
            }
            else
            {
                string temp = "USD" + currency;
                if (isBid) result = GetValueBid(temp);
                else result = GetValueAsk(temp);                
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isBid"></param>
        /// <returns></returns>
        internal double FindSymbolCurrencyUSD(string symbol, bool isBid)
        {
            string currency = symbol.Substring(3);
            double result = 0;
            if (currency == "USD")
            {
                if (isBid) result = GetValueBid(symbol);
                else result = GetValueAsk(symbol);
            }
            else
            {
                string temp = currency + "USD";
                if (isBid) result = GetValueBid(temp);
                else result = GetValueAsk(temp);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="pip"></param>
        /// <returns></returns>
        public static double ConvertNumberPip(int digits, double pip)
        {
            double result;
            result = pip / Math.Pow(10, digits);
            result = Math.Round(result, digits);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckTimeTick()
        {
            bool result = false;
            TimeSpan span = DateTime.Now - this.TickValue.TickTime;
            if (span.TotalSeconds < Business.Market.TickTimeOut)
                result = true;

            return result;
        }
    }
}
