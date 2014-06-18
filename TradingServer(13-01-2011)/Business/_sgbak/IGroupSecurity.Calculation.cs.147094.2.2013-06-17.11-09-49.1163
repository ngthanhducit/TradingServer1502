using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class IGroupSecurity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="step"></param>
        /// <param name="valueCheck"></param>
        /// <returns></returns>
        internal bool CheckStepLots(double min, double max, double step, double valueCheck)
        {
            if (valueCheck > max) return false;
            if (valueCheck < min) return false;
            double tempa = CheckLengthAfterDigit(valueCheck);
            double tempb = CheckLengthAfterDigit(step);
            if (tempa > tempb) return false;
            tempa = ConvertNonDigit(valueCheck);
            tempb = ConvertNonDigit(step);
            double result = tempa % tempb;
            if (result > 0) return false;
            //if (valueCheck.ToString().Length > step.ToString().Length) return false;
            return true;
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
                value = Math.Round(value, kq.Length);
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
        /// <param name="value"></param>
        /// <returns></returns>
        internal double ConvertValueToPip(double value)
        {
            int digit = CheckLengthAfterDigit(value);
            double result = 1;
            for (int i = 0; i < digit; i++)
            {
                result *= 0.1;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="standard"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double CalculateCommissionByMoney(double standard, double lots)
        {
            return standard * lots;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="standard"></param>
        /// <param name="lots"></param>
        /// <param name="contract_size"></param>
        /// <param name="open_price"></param>
        /// <returns></returns>
        internal double CalculateCommissionByPoints(double standard, double lots, double contract_size, double open_price)
        {
            return standard * lots * ConvertValueToPip(open_price);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="standard"></param>
        /// <param name="lots"></param>
        /// <param name="contract_size"></param>
        /// <param name="open_price"></param>
        /// <returns></returns>
        internal double CalculateCommissionByPercentage(double standard, double lots, double contract_size, double open_price)
        {
            return open_price * lots * standard;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentPoints"></param>
        /// <param name="lots"></param>
        /// <param name="contract_size"></param>
        /// <param name="open_price"></param>
        /// <returns></returns>
        internal double CalculateAgentPointsByPoints(double agentPoints, double lots, double contract_size, double open_price)
        {
            return agentPoints * lots * ConvertValueToPip(open_price);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxes"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        internal double CalculateTaxes(double taxes, double commission)
        {
            if (taxes > 0) return -commission / taxes;
            else return 0;
        }
    }
}
