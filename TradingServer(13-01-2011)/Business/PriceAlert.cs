using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class PriceAlert
    {
        public int ID { get; set; }
        public string Symbol { get; set; }
        public Business.Tick TickOnline { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Value { get; set; }
        public Business.ConditionAlert AlertCondition { get; set; }
        public Business.ActionAlert AlertAction { get; set; }
        public bool IsEnable { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateActive { get; set; }
        public int Iterations { get; set; }
        public int InvestorID { get; set; }
        public double ValueApply { get; set; }
        public string Notification { get; set; }
        #region Create Instance Class DBWAlert
        private static DBW.DBWAlert dbwAlert;
        private static DBW.DBWAlert DBWAlertInstance
        {
            get
            {
                if (PriceAlert.dbwAlert == null)
                {
                    PriceAlert.dbwAlert = new DBW.DBWAlert();
                }
                return PriceAlert.dbwAlert;
            }
        }
        #endregion       

        internal List<Business.PriceAlert> GetAllAlert()
        {
            return PriceAlert.DBWAlertInstance.GetAllAlert();
        }

        internal Business.PriceAlert GetAlertByAlertID(int AlertID)
        {
            return PriceAlert.DBWAlertInstance.GetAlertByAlertID(AlertID);
        }

        internal List<Business.PriceAlert> GetAlertByInvestorIDWithTime(int InvestorID,DateTime StartDate, DateTime EndDate)
        {
            return PriceAlert.DBWAlertInstance.GetAlertByInvestorIDWithTime(InvestorID, StartDate, EndDate);
        }

        internal List<Business.PriceAlert> GetAlertByInvestorID(int InvestorID)
        {
            for (int i = 0; i < Business.Market.InvestorList.Count; i++)
            {
                if (InvestorID == Business.Market.InvestorList[i].InvestorID)
                {
                    if (Business.Market.InvestorList[i].AlertQueue == null)
                    {
                        Business.Market.InvestorList[i].AlertQueue = new List<PriceAlert>();
                    }
                    return Business.Market.InvestorList[i].AlertQueue;
                }
            }
            for (int i = 0; i < Business.Market.AgentList.Count; i++)
            {
                if (InvestorID == Business.Market.AgentList[i].InvestorID)
                {
                    if (Business.Market.AgentList[i].AlertQueue == null)
                    {
                        Business.Market.AgentList[i].AlertQueue = new List<PriceAlert>();
                    }
                    return Business.Market.AgentList[i].AlertQueue;
                }
            }
            return null;
            //return PriceAlert.DBWAlertInstance.GetAlertByInvestorID(InvestorID);
        }

        internal int AddNewAlert(Business.PriceAlert alert)
        {
            return PriceAlert.DBWAlertInstance.AddNewAlert(alert);
        }

        internal bool UpdateAlert(Business.PriceAlert alert)
        {
            return PriceAlert.DBWAlertInstance.UpdateAlert(alert);
        }

        internal int DeleteAlertByID(int AlertID,string Symbol,int InvestorID)
        {
            return PriceAlert.DBWAlertInstance.DeleteAlertByID(AlertID,Symbol,InvestorID);
        }

        internal int DeleteAlertByInvestorID(int InvestorID)
        {
            return PriceAlert.DBWAlertInstance.DeleteAlertByInvestorID(InvestorID);
        }

        internal int DeleteAlertWithTime(DateTime StartDate, DateTime EndDate)
        {
            return PriceAlert.DBWAlertInstance.DeleteAlertWithTime(StartDate, EndDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        internal void CalculationAlert(Tick Tick, Symbol RefSymbol)
        {
            if (RefSymbol.AlertQueue == null)
            {
                RefSymbol.AlertQueue = new List<PriceAlert>();
            }
            for (int j = RefSymbol.AlertQueue.Count - 1; j >= 0; j--)
            {
                string condition = RefSymbol.AlertQueue[j].AlertCondition.ToString();
                double value = RefSymbol.AlertQueue[j].Value;
                int securityID = -1;
                int digit = RefSymbol.Digit;
                bool checkCon = false;
                RefSymbol.AlertQueue[j].TickOnline = Tick;
                switch (condition)
                {
                    case "LargerBid":
                        {
                            if (Tick.Bid < value)
                            {
                                RefSymbol.AlertQueue[j].ValueApply = Tick.Bid;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "LargerAsk":
                        {
                            if (Tick.Ask < value)
                            {
                                securityID = RefSymbol.SecurityID;
                                RefSymbol.AlertQueue[j].ValueApply = Tick.Ask;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "LargerHighBid":
                        {
                            if (Tick.HighInDay < value)
                            {
                                RefSymbol.AlertQueue[j].ValueApply = Tick.HighInDay;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "LargerHighAsk":
                        {
                            if (Tick.HighInDay < value)
                            {
                                securityID = RefSymbol.SecurityID;
                                RefSymbol.AlertQueue[j].ValueApply = Tick.HighInDay;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "SmallerBid":
                        {
                            if (Tick.Bid > value)
                            {
                                RefSymbol.AlertQueue[j].ValueApply = Tick.Bid;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "SmallerAsk":
                        {
                            if (Tick.Ask > value)
                            {
                                securityID = RefSymbol.SecurityID;
                                RefSymbol.AlertQueue[j].ValueApply = Tick.Ask;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "SmallerLowBid":
                        {
                            if (Tick.LowInDay > value)
                            {
                                RefSymbol.AlertQueue[j].ValueApply = Tick.LowInDay;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                    case "SmallerLowAsk":
                        {
                            if (Tick.LowInDay > value)
                            {
                                securityID = RefSymbol.SecurityID;
                                RefSymbol.AlertQueue[j].ValueApply = Tick.LowInDay;
                                checkCon = this.SendAlert(RefSymbol.AlertQueue[j], securityID, digit);
                            }
                            break;
                        }
                }
                if (checkCon)
                {
                    RefSymbol.AlertQueue.RemoveAt(j);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        internal bool SendAlert(Business.PriceAlert alert, int sercurityID, int digit)
        {
            string action = alert.AlertAction.ToString();
            string condition = alert.AlertCondition.ToString();
            switch (action)
            {
                case "Email":
                    {
                        for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                        {
                            if (Business.Market.InvestorList[i].InvestorID == alert.InvestorID)
                            {
                                double spreadDiff = 0;
                                double askPrice = 0;
                                double highAsk = 0;
                                double lowAsk = 0;
                                if (sercurityID != -1)
                                {
                                    spreadDiff = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(sercurityID, Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID);
                                    spreadDiff = spreadDiff / Math.Pow(10, digit);
                                    spreadDiff = Math.Round(spreadDiff, digit);
                                    askPrice = alert.TickOnline.Ask + spreadDiff;
                                    highAsk = alert.TickOnline.HighInDay + spreadDiff;
                                    lowAsk = alert.TickOnline.HighInDay + spreadDiff;
                                    switch (condition)
                                    {
                                        case "LargerAsk":
                                            {
                                                if (askPrice > alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                        case "SmallerAsk":
                                            {
                                                if (askPrice < alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                        case "LargerHighAsk":
                                            {
                                                if (highAsk > alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                        case "SmallerLowAsk":
                                            {
                                                if (lowAsk < alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                    }                                   
                                    alert.ValueApply = askPrice;
                                }
                                Model.MailConfig mail = Facade.FacadeGetMailConfig(Business.Market.InvestorList[i]);
                                string subject = "ALERT \"" + alert.Symbol + "\"";
                                string content = alert.Symbol + " @" + alert.ValueApply + " at " + alert.DateActive;
                                Model.TradingCalculate.Instance.SendMail(alert.Email, subject, content, mail);
                                for (int j = 0; j < Business.Market.InvestorList[i].AlertQueue.Count; j++)
                                {
                                    if (Business.Market.InvestorList[i].AlertQueue[j].ID == alert.ID)
                                    {
                                        Business.Market.InvestorList[i].AlertQueue.RemoveAt(j);
                                        alert.IsEnable = false;
                                        alert.DateActive = DateTime.Now;
                                        this.UpdateAlert(alert);
                                        string noticeInvestor = "NA21$" + alert.ID;

                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                        Business.Market.InvestorList[i].ClientCommandQueue.Add(noticeInvestor);
                                        return true;
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < Business.Market.AgentList.Count; i++)
                        {
                            if (Business.Market.AgentList[i].InvestorID == alert.InvestorID)
                            {
                                string subject = "ALERT \"" + alert.Symbol + "\"";
                                string content = alert.Symbol + " @" + alert.ValueApply + " at " + alert.DateActive;
                                Model.TradingCalculate.Instance.SendMailAgent(alert.Email, subject, content);
                                for (int j = 0; j < Business.Market.AgentList[i].AlertQueue.Count; j++)
                                {
                                    if (Business.Market.AgentList[i].AlertQueue[j].ID == alert.ID)
                                    {
                                        Business.Market.AgentList[i].AlertQueue.RemoveAt(j);
                                        alert.IsEnable = false;
                                        alert.DateActive = DateTime.Now;
                                        this.UpdateAlert(alert);
                                        Facade.FacadeSendNoticeManagerChangeAlert(1, alert);
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case "SMS":
                    {
                        break;
                    }
                case "Sound":
                    {
                        for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                        {
                            if (Business.Market.InvestorList[i].InvestorID == alert.InvestorID)
                            {
                                double spreadDiff = 0;
                                double askPrice = 0;
                                double highAsk = 0;
                                double lowAsk = 0;
                                if (sercurityID != -1)
                                {
                                    spreadDiff = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(sercurityID, Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID);
                                    spreadDiff = spreadDiff / Math.Pow(10, digit);
                                    spreadDiff = Math.Round(spreadDiff, digit);
                                    askPrice = alert.TickOnline.Ask + spreadDiff;
                                    highAsk = alert.TickOnline.HighInDay + spreadDiff;
                                    lowAsk = alert.TickOnline.HighInDay + spreadDiff;
                                    switch (condition)
                                    {
                                        case "LargerAsk":
                                            {
                                                if (askPrice > alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                        case "SmallerAsk":
                                            {
                                                if (askPrice < alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                        case "LargerHighAsk":
                                            {
                                                if (highAsk > alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                        case "SmallerLowAsk":
                                            {
                                                if (lowAsk < alert.Value)
                                                {
                                                    return false;
                                                }
                                                break;
                                            }
                                    }
                                    alert.ValueApply = askPrice;
                                }
                                for (int j = 0; j < Business.Market.InvestorList[i].AlertQueue.Count; j++)
                                {
                                    if (Business.Market.InvestorList[i].AlertQueue[j].ID == alert.ID)
                                    {
                                        Business.Market.InvestorList[i].AlertQueue.RemoveAt(j);
                                        alert.IsEnable = false;
                                        alert.DateActive = DateTime.Now;                                        
                                        this.UpdateAlert(alert);
                                        string noticeInvestor = "NA21$" + alert.ID;

                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                        Business.Market.InvestorList[i].ClientCommandQueue.Add(noticeInvestor);
                                        return true;
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < Business.Market.AgentList.Count; i++)
                        {
                            if (Business.Market.AgentList[i].InvestorID == alert.InvestorID)
                            {
                                for (int j = 0; j < Business.Market.AgentList[i].AlertQueue.Count; j++)
                                {
                                    if (Business.Market.AgentList[i].AlertQueue[j].ID == alert.ID)
                                    {
                                        Business.Market.AgentList[i].AlertQueue.RemoveAt(j);
                                        alert.IsEnable = false;
                                        alert.DateActive = DateTime.Now;
                                        this.UpdateAlert(alert);
                                        Facade.FacadeSendNoticeManagerChangeAlert(1, alert);
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
            return false;
        }
    }
}
