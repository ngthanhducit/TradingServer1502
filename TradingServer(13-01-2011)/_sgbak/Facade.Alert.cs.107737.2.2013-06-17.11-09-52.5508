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
        public static List<Business.PriceAlert> FacadeGetAllAlert()
        {
            return Facade.AlertInstance.GetAllAlert();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlertID"></param>
        /// <returns></returns>
        public static Business.PriceAlert FacadeGetAlertByAlertID(int AlertID)
        {
            return Facade.AlertInstance.GetAlertByAlertID(AlertID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static List<Business.PriceAlert> FacadeGetAlertByInvestorIDWithTime(int InvestorID, DateTime StartDate, DateTime EndDate)
        {
            return Facade.AlertInstance.GetAlertByInvestorIDWithTime(InvestorID, StartDate, EndDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public static List<Business.PriceAlert> FacadeGetAlertByInvestorID(int InvestorID, int Start, int End)
        {
            List<Business.PriceAlert> tem = Facade.AlertInstance.GetAlertByInvestorID(InvestorID);
            List<Business.PriceAlert> result = new List<Business.PriceAlert>();
            if (tem != null)
            {
                int count = tem.Count;
                if (Start > count - 1)
                {
                    return null;
                }
                if (End > count - 1)
                {
                    End = count;
                }
                for (int i = Start; i < End; i++)
                {
                    result.Add(tem[i]);
                }
            }
            return result;
        }

        public static List<Business.PriceAlert> FacadeGetAlertByInvestorID(int InvestorID)
        {
            return Facade.AlertInstance.GetAlertByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static int FacadeAddNewAlert(Business.PriceAlert alert)
        {
            return Facade.AlertInstance.AddNewAlert(alert);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static bool FacadeUpdateAlert(Business.PriceAlert alert)
        {
            return Facade.AlertInstance.UpdateAlert(alert);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlertID"></param>
        /// <param name="Symbol"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static int FacadeDeleteAlertByID(int AlertID, string Symbol, int InvestorID)
        {
            return Facade.AlertInstance.DeleteAlertByID(AlertID, Symbol, InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static int FacadeDeleteAlertByInvestorID(int InvestorID)
        {
            return Facade.AlertInstance.DeleteAlertByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static int FacadeDeleteAlertWithTime(DateTime StartDate, DateTime EndDate)
        {
            return Facade.AlertInstance.DeleteAlertWithTime(StartDate, EndDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        public static void FacadeCalculationAlert(Business.Tick Tick, Business.Symbol RefSymbol)
        {
            Facade.AlertInstance.CalculationAlert(Tick, RefSymbol);
        }        
    }
}
