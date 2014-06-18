using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWAlert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.PriceAlert> GetAllAlert()
        {
            List<Business.PriceAlert> Result = new List<Business.PriceAlert>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();
            DS.AlertDataTable tbAlert = new DS.AlertDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbAlert = adap.GetData();

                if (tbAlert != null)
                {
                    int count = tbAlert.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.PriceAlert newAlert = new Business.PriceAlert();
                        newAlert.ID = tbAlert[i].ID;
                        newAlert.Email = tbAlert[i].Email;
                        newAlert.PhoneNumber = tbAlert[i].PhoneNumber;
                        newAlert.Value = tbAlert[i].Value;
                        newAlert.IsEnable = tbAlert[i].IsEnable;
                        newAlert.DateActive = tbAlert[i].DateActive;
                        newAlert.DateCreate = tbAlert[i].DateCreate;
                        newAlert.Iterations = tbAlert[i].Iterations;
                        newAlert.InvestorID = tbAlert[i].InvestorID;
                        newAlert.Notification = tbAlert[i].Notification;
                        newAlert.TickOnline = new Business.Tick();
                        newAlert.TickOnline.Ask = tbAlert[i].Ask;
                        newAlert.TickOnline.Bid = tbAlert[i].Bid;
                        newAlert.TickOnline.SymbolName = tbAlert[i].Symbol;
                        newAlert.Symbol = tbAlert[i].Symbol;
                        #region Condition & Action
                        Business.ActionAlert newAlertAction = new Business.ActionAlert();
                        switch (tbAlert[i].Action)
                        {
                            case "Email":
                                newAlertAction = Business.ActionAlert.Email;
                                break;
                            case "SMS":
                                newAlertAction = Business.ActionAlert.SMS;
                                break;
                            case "Sound":
                                newAlertAction = Business.ActionAlert.Sound;
                                break;
                        }
                        newAlert.AlertAction = newAlertAction;
                        Business.ConditionAlert newAlertCondition = new Business.ConditionAlert();
                        switch (tbAlert[i].Condtion)
                        {
                            case "LargerAsk":
                                newAlertCondition = Business.ConditionAlert.LargerAsk;
                                break;
                            case "LargerBid":
                                newAlertCondition = Business.ConditionAlert.LargerBid;
                                break;
                            case "LargerHighAsk":
                                newAlertCondition = Business.ConditionAlert.LargerHighAsk;
                                break;
                            case "LargerHighBid":
                                newAlertCondition = Business.ConditionAlert.LargerHighBid;
                                break;
                            case "SmallerAsk":
                                newAlertCondition = Business.ConditionAlert.SmallerAsk;
                                break;
                            case "SmallerBid":
                                newAlertCondition = Business.ConditionAlert.SmallerBid;
                                break;
                            case "SmallerLowAsk":
                                newAlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                break;
                            case "SmallerLowBid":
                                newAlertCondition = Business.ConditionAlert.SmallerLowBid;
                                break;
                        }
                        newAlert.AlertCondition = newAlertCondition;
                        #endregion
                       
                        for (int j = 0; j < Business.Market.SymbolList.Count; j++)
                        {
                            if (Business.Market.SymbolList[j].AlertQueue == null)
                            {
                                Business.Market.SymbolList[j].AlertQueue = new List<Business.PriceAlert>();
                            }
                            if (Business.Market.SymbolList[j].Name == newAlert.Symbol)
                            {
                                Business.Market.SymbolList[j].AlertQueue.Add(newAlert);
                                break;
                            }
                        }
                        for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                        {
                            if (Business.Market.InvestorList[j].AlertQueue == null)
                            {
                                Business.Market.InvestorList[j].AlertQueue = new List<Business.PriceAlert>();
                            }
                            if (Business.Market.InvestorList[j].InvestorID == newAlert.InvestorID)
                            {
                                Business.Market.InvestorList[j].AlertQueue.Add(newAlert);
                                break;
                            }
                        }
                        
                        Result.Add(newAlert);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlertID"></param>
        /// <returns></returns>
        internal Business.PriceAlert GetAlertByAlertID(int AlertID)
        {
            Business.PriceAlert newAlert = new Business.PriceAlert();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();
            DS.AlertDataTable tbAlert = new DS.AlertDataTable();
            try
            {
                conn.Open();
                adap.Connection = conn;
                tbAlert = adap.GetAlertByID(AlertID);
                if (tbAlert != null)
                {
                    newAlert.ID = tbAlert[0].ID;
                    newAlert.Email = tbAlert[0].Email;
                    newAlert.PhoneNumber = tbAlert[0].PhoneNumber;
                    newAlert.Value = tbAlert[0].Value;
                    newAlert.IsEnable = tbAlert[0].IsEnable;
                    newAlert.DateActive = tbAlert[0].DateActive;
                    newAlert.DateCreate = tbAlert[0].DateCreate;
                    newAlert.Iterations = tbAlert[0].Iterations;
                    newAlert.InvestorID = tbAlert[0].InvestorID;
                    newAlert.Notification = tbAlert[0].Notification;
                    newAlert.TickOnline = new Business.Tick();
                    newAlert.TickOnline.Ask = tbAlert[0].Ask;
                    newAlert.TickOnline.Bid = tbAlert[0].Bid;
                    newAlert.TickOnline.SymbolName = tbAlert[0].Symbol;
                    newAlert.Symbol = tbAlert[0].Symbol;
                    Business.ActionAlert newAlertAction = new Business.ActionAlert();
                    switch (tbAlert[0].Action)
                    {
                        case "Email":
                            newAlertAction = Business.ActionAlert.Email;
                            break;
                        case "SMS":
                            newAlertAction = Business.ActionAlert.SMS;
                            break;
                        case "Sound":
                            newAlertAction = Business.ActionAlert.Sound;
                            break;
                    }
                    newAlert.AlertAction = newAlertAction;
                    Business.ConditionAlert newAlertCondition = new Business.ConditionAlert();
                    switch (tbAlert[0].Condtion)
                    {
                        case "LargerAsk":
                            newAlertCondition = Business.ConditionAlert.LargerAsk;
                            break;
                        case "LargerBid":
                            newAlertCondition = Business.ConditionAlert.LargerBid;
                            break;
                        case "LargerHighAsk":
                            newAlertCondition = Business.ConditionAlert.LargerHighAsk;
                            break;
                        case "LargerHighBid":
                            newAlertCondition = Business.ConditionAlert.LargerHighBid;
                            break;
                        case "SmallerAsk":
                            newAlertCondition = Business.ConditionAlert.SmallerAsk;
                            break;
                        case "SmallerBid":
                            newAlertCondition = Business.ConditionAlert.SmallerBid;
                            break;
                        case "SmallerLowAsk":
                            newAlertCondition = Business.ConditionAlert.SmallerLowAsk;
                            break;
                        case "SmallerLowBid":
                            newAlertCondition = Business.ConditionAlert.SmallerLowBid;
                            break;
                    }

                    newAlert.AlertCondition = newAlertCondition;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
            return newAlert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.PriceAlert> GetAlertByInvestorIDWithTime(int InvestorID,DateTime StartDate, DateTime EndDate)
        {
            List<Business.PriceAlert> Result = new List<Business.PriceAlert>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();
            DS.AlertDataTable tbAlert = new DS.AlertDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbAlert = adap.GetAlertByInvestorWithTime(InvestorID, StartDate, EndDate);

                if (tbAlert != null)
                {
                    int count = tbAlert.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.PriceAlert newAlert = new Business.PriceAlert();
                        newAlert.ID = tbAlert[i].ID;
                        newAlert.Email = tbAlert[i].Email;
                        newAlert.PhoneNumber = tbAlert[i].PhoneNumber;
                        newAlert.Value = tbAlert[i].Value;
                        newAlert.IsEnable = tbAlert[i].IsEnable;
                        newAlert.DateActive = tbAlert[i].DateActive;
                        newAlert.DateCreate = tbAlert[i].DateCreate;
                        newAlert.Iterations = tbAlert[i].Iterations;
                        newAlert.InvestorID = tbAlert[i].InvestorID;
                        newAlert.Notification = tbAlert[i].Notification;
                        newAlert.TickOnline = new Business.Tick();
                        newAlert.TickOnline.Ask = tbAlert[i].Ask;
                        newAlert.TickOnline.Bid = tbAlert[i].Bid;
                        newAlert.TickOnline.SymbolName = tbAlert[i].Symbol;
                        newAlert.Symbol = tbAlert[i].Symbol;
                        Business.ActionAlert newAlertAction = new Business.ActionAlert();
                        switch (tbAlert[i].Action)
                        {
                            case "Email":
                                newAlertAction = Business.ActionAlert.Email;
                                break;
                            case "SMS":
                                newAlertAction = Business.ActionAlert.SMS;
                                break;
                            case "Sound":
                                newAlertAction = Business.ActionAlert.Sound;
                                break;
                        }
                        newAlert.AlertAction = newAlertAction;
                        Business.ConditionAlert newAlertCondition = new Business.ConditionAlert();
                        switch (tbAlert[i].Condtion)
                        {
                            case "LargerAsk":
                                newAlertCondition = Business.ConditionAlert.LargerAsk;
                                break;
                            case "LargerBid":
                                newAlertCondition = Business.ConditionAlert.LargerBid;
                                break;
                            case "LargerHighAsk":
                                newAlertCondition = Business.ConditionAlert.LargerHighAsk;
                                break;
                            case "LargerHighBid":
                                newAlertCondition = Business.ConditionAlert.LargerHighBid;
                                break;
                            case "SmallerAsk":
                                newAlertCondition = Business.ConditionAlert.SmallerAsk;
                                break;
                            case "SmallerBid":
                                newAlertCondition = Business.ConditionAlert.SmallerBid;
                                break;
                            case "SmallerLowAsk":
                                newAlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                break;
                            case "SmallerLowBid":
                                newAlertCondition = Business.ConditionAlert.SmallerLowBid;
                                break;
                        }

                        newAlert.AlertCondition = newAlertCondition;
                        Result.Add(newAlert);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.PriceAlert> GetAlertByInvestorID(int InvestorID)
        {
            List<Business.PriceAlert> Result = new List<Business.PriceAlert>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();
            DS.AlertDataTable tbAlert = new DS.AlertDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbAlert = adap.GetAlertByInvestorID(InvestorID);

                if (tbAlert != null)
                {
                    int count = tbAlert.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.PriceAlert newAlert = new Business.PriceAlert();
                        newAlert.ID = tbAlert[i].ID;
                        newAlert.Email = tbAlert[i].Email;
                        newAlert.PhoneNumber = tbAlert[i].PhoneNumber;
                        newAlert.Value = tbAlert[i].Value;
                        newAlert.IsEnable = tbAlert[i].IsEnable;
                        newAlert.DateActive = tbAlert[i].DateActive;
                        newAlert.DateCreate = tbAlert[i].DateCreate;
                        newAlert.Iterations = tbAlert[i].Iterations;
                        newAlert.InvestorID = tbAlert[i].InvestorID;
                        newAlert.Notification = tbAlert[i].Notification;
                        newAlert.TickOnline = new Business.Tick();
                        newAlert.TickOnline.Ask = tbAlert[i].Ask;
                        newAlert.TickOnline.Bid = tbAlert[i].Bid;
                        newAlert.TickOnline.SymbolName = tbAlert[i].Symbol;
                        newAlert.Symbol = tbAlert[i].Symbol;
                        Business.ActionAlert newAlertAction = new Business.ActionAlert();
                        switch (tbAlert[i].Action)
                        {
                            case "Email":
                                newAlertAction = Business.ActionAlert.Email;
                                break;
                            case "SMS":
                                newAlertAction = Business.ActionAlert.SMS;
                                break;
                            case "Sound":
                                newAlertAction = Business.ActionAlert.Sound;
                                break;
                        }
                        newAlert.AlertAction = newAlertAction;
                        Business.ConditionAlert newAlertCondition = new Business.ConditionAlert();
                        switch (tbAlert[i].Condtion)
                        {
                            case "LargerAsk":
                                newAlertCondition = Business.ConditionAlert.LargerAsk;
                                break;
                            case "LargerBid":
                                newAlertCondition = Business.ConditionAlert.LargerBid;
                                break;
                            case "LargerHighAsk":
                                newAlertCondition = Business.ConditionAlert.LargerHighAsk;
                                break;
                            case "LargerHighBid":
                                newAlertCondition = Business.ConditionAlert.LargerHighBid;
                                break;
                            case "SmallerAsk":
                                newAlertCondition = Business.ConditionAlert.SmallerAsk;
                                break;
                            case "SmallerBid":
                                newAlertCondition = Business.ConditionAlert.SmallerBid;
                                break;
                            case "SmallerLowAsk":
                                newAlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                break;
                            case "SmallerLowBid":
                                newAlertCondition = Business.ConditionAlert.SmallerLowBid;
                                break;
                        }

                        newAlert.AlertCondition = newAlertCondition;
                        Result.Add(newAlert);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewAlert(Business.PriceAlert alert)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.InsertAlert(alert.Symbol, alert.TickOnline.Bid, alert.TickOnline.Ask, alert.AlertCondition.ToString(), alert.Email, alert.Value, alert.AlertAction.ToString(), alert.PhoneNumber, alert.IsEnable, alert.DateCreate, alert.DateActive, alert.Iterations, alert.InvestorID, alert.Notification).ToString());    
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
            if (Result != -1)
            {
                alert.ID = Result;
                for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == alert.Symbol)
                    {
                        Business.Market.SymbolList[i].AlertQueue.Add(alert);
                        break;
                    }
                }
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == alert.InvestorID)
                    {
                        Business.Market.InvestorList[i].AlertQueue.Add(alert);
                        break;
                    }
                }
                for (int i = 0; i < Business.Market.AgentList.Count; i++)
                {
                    if (Business.Market.AgentList[i].InvestorID == alert.InvestorID)
                    {
                        Business.Market.AgentList[i].AlertQueue.Add(alert);
                        break;
                    }
                }
            }            
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        internal bool UpdateAlert(Business.PriceAlert alert)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int Record = adap.UpdateAlert(alert.Symbol, alert.TickOnline.Bid, alert.TickOnline.Ask, alert.AlertCondition.ToString(), alert.Email, alert.Value, alert.AlertAction.ToString(), alert.PhoneNumber, alert.IsEnable, alert.DateCreate, alert.DateActive, alert.Iterations, alert.InvestorID,alert.Notification,alert.ID);
                if (Record > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
            if (Result)
            {
                string symbolFirst = alert.Symbol;
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == alert.InvestorID)
                    {
                        for (int j = 0; j < Business.Market.InvestorList[i].AlertQueue.Count; j++)
                        {
                            if (Business.Market.InvestorList[i].AlertQueue[j].ID == alert.ID)
                            {
                                if (symbolFirst != Business.Market.InvestorList[i].AlertQueue[j].Symbol)
                                {
                                    symbolFirst = Business.Market.InvestorList[i].AlertQueue[j].Symbol;
                                }
                                Business.Market.InvestorList[i].AlertQueue[j] = alert;
                                break;
                            }
                        }
                        break;
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
                                if (symbolFirst != Business.Market.AgentList[i].AlertQueue[j].Symbol)
                                {
                                    symbolFirst = Business.Market.AgentList[i].AlertQueue[j].Symbol;
                                }
                                Business.Market.AgentList[i].AlertQueue[j] = alert;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (symbolFirst == alert.Symbol)
                {
                    for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                    {
                        if (Business.Market.SymbolList[i].Name == alert.Symbol)
                        {
                            for (int j = 0; j < Business.Market.SymbolList[i].AlertQueue.Count; j++)
                            {
                                if (Business.Market.SymbolList[i].AlertQueue[j].ID == alert.ID)
                                {
                                    Business.Market.SymbolList[i].AlertQueue[j] = alert;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    bool top1 = false;
                    bool top2 = false;
                    for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                    {
                        if (Business.Market.SymbolList[i].Name == symbolFirst)
                        {
                            for (int j = 0; j < Business.Market.SymbolList[i].AlertQueue.Count; j++)
                            {
                                if (Business.Market.SymbolList[i].AlertQueue[j].ID == alert.ID)
                                {
                                    Business.Market.SymbolList[i].AlertQueue.RemoveAt(j);
                                    break;
                                }
                            }
                            top1 = true;
                        }
                        if (Business.Market.SymbolList[i].Name == alert.Symbol)
                        {
                            if (Business.Market.SymbolList[i].AlertQueue == null)
                            {
                                Business.Market.SymbolList[i].AlertQueue = new List<Business.PriceAlert>();
                            }
                            Business.Market.SymbolList[i].AlertQueue.Add(alert);
                            top2 = true;
                        }
                        if (top1 && top2)
                        {
                            break;
                        }
                    }                    
                }
                
                
            }            

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlertID"></param>
        /// <returns></returns>
        internal int DeleteAlertByID(int AlertID,string Symbol,int InvestorID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteAlertByID(AlertID);
            }
            catch (Exception ex)
            {
                Result = -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
            if (Result != -1)
            {
                for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == Symbol)
                    {
                        for (int j = 0; j < Business.Market.SymbolList[i].AlertQueue.Count; j++)
                        {
                            if (Business.Market.SymbolList[i].AlertQueue[j].ID == AlertID)
                            {
                                Business.Market.SymbolList[i].AlertQueue.RemoveAt(j);
                                break;
                            }
                        }                            
                        break;
                    }
                }
                for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        for (int j = 0; j < Business.Market.InvestorList[i].AlertQueue.Count; j++)
                        {
                            if (Business.Market.InvestorList[i].AlertQueue[j].ID == AlertID)
                            {
                                Business.Market.InvestorList[i].AlertQueue.RemoveAt(j);
                                break;
                            }
                        }    
                        break;
                    }
                }
                for (int i = 0; i < Business.Market.AgentList.Count; i++)
                {
                    if (Business.Market.AgentList[i].InvestorID == InvestorID)
                    {
                        for (int j = 0; j < Business.Market.AgentList[i].AlertQueue.Count; j++)
                        {
                            if (Business.Market.AgentList[i].AlertQueue[j].ID == AlertID)
                            {
                                Business.Market.AgentList[i].AlertQueue.RemoveAt(j);
                                break;
                            }
                        }   
                        break;
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal int DeleteAlertByInvestorID(int InvestorID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteAlertByInvestorID(InvestorID);
            }
            catch (Exception ex)
            {
                Result = -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        internal int DeleteAlertWithTime(DateTime StartDate, DateTime EndDate)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteAlertWithTime(StartDate,EndDate);
            }
            catch (Exception ex)
            {
                Result = -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountAlert()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.AlertTableAdapter adap = new DSTableAdapters.AlertTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountAlert();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result.Value;
        }
    }
}
