using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class InvestorGroup
    {
        /// <summary>
        /// True is Call
        /// </summary>
        /// <param name="marginLevel"></param>
        /// <param name="marginCallLevel"></param>
        /// <returns></returns>
        internal bool CheckMarginCallLevel(double marginLevel, double marginCallLevel)
        {
            if (marginLevel <= marginCallLevel) return true;
            else return false;
        }
        /// <summary>
        /// method = 0: use %
        /// method = 1: use money
        /// method other non Stop Out
        /// True is Stop Out
        /// </summary>
        /// <param name="marginLevel"></param>
        /// <param name="balance"></param>
        /// <param name="stopOutLevel"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal bool CheckStopOutLevel(double marginLevel, double balance, double stopOutLevel, int method)
        {
            switch (method)
            {
                case 0:
                    if (marginLevel <= stopOutLevel) return true;
                    else return false;
                case 1:
                    if (balance <= stopOutLevel) return true;
                    else return false;
                default:
                    return false;
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalMargin"></param>
        /// <param name="balance"></param>
        /// <param name="equity"></param>
        /// <param name="profit"></param>
        /// <param name="loss"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal double CalculationTotalFreeMargin(double totalMargin, double balance, double equity, double profit, double loss, int method)
        {
            switch (method)
            {
                case 0:
                    {
                        return balance - totalMargin;
                    }
                case 1:
                    {
                        return equity - totalMargin;
                    }
                case 2:
                    {
                        return balance - totalMargin + profit;
                    }
                case 3:
                    {
                        return balance - totalMargin + loss;
                    }
                default:
                    {
                        return balance - totalMargin;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCommand"></param>
        /// <returns></returns>
        internal double CalculationTotalProfitPositive(List<Business.OpenTrade> listCommand)
        {
            double result = 0;
            for (int i = 0; i < listCommand.Count; i++)
            {
                int typeID = listCommand[i].Type.ID;
                if (!listCommand[i].IsClose & typeID != 3 & typeID != 4 & typeID != 7 & typeID != 8 & typeID != 9 & typeID != 10 & typeID != 17 & typeID != 18 & typeID != 19 & typeID != 20)
                {
                    double totalProfit = listCommand[i].Profit + listCommand[i].Commission + listCommand[i].Swap;
                    if (totalProfit > 0)
                    {
                        result += totalProfit;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCommand"></param>
        /// <returns></returns>
        internal double CalculationTotalLoss(List<Business.OpenTrade> listCommand)
        {
            double result = 0;
            for (int i = 0; i < listCommand.Count; i++)
            {
                int typeID = listCommand[i].Type.ID;
                if (!listCommand[i].IsClose & typeID != 3 & typeID != 4 & typeID != 7 & typeID != 8 & typeID != 9 & typeID != 10 & typeID != 17 & typeID != 18 & typeID != 19 & typeID != 20)
                {
                    double totalProfit = listCommand[i].Profit + listCommand[i].Commission + listCommand[i].Swap;
                    if (listCommand[i].Profit < 0)
                    {
                        result += totalProfit;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCommand"></param>
        /// <returns></returns>
        internal double CalculationTotalProfit(List<Business.OpenTrade> listCommand)
        {
            double result = 0;
            for (int i = 0; i < listCommand.Count; i++)
            {
                int typeID = listCommand[i].Type.ID;
                if (!listCommand[i].IsClose & typeID != 3 & typeID != 4 & typeID != 7 & typeID != 8 & typeID != 9 & typeID != 10 & typeID != 17 & typeID != 18 & typeID != 19 & typeID != 20)
                {
                    //result += (listCommand[i].Profit+ listCommand[i].Commission + listCommand[i].Swap);
                    //7-03-2012 Edit By Duc: Because Swap sub to balance then calculation total profit = PL + Commission
                    result += (listCommand[i].Profit + listCommand[i].Commission);
                }
            }
            return result;
        }

    }
}
