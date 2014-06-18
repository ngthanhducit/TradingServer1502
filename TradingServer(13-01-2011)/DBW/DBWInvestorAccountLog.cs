using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWInvestorAccountLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.InvestorAccountLog> GetInvestorAccountLogByInvestorID(int InvestorID)
        {
            List<Business.InvestorAccountLog> Result = new List<Business.InvestorAccountLog>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();
            DS.InvestorAccountLogDataTable tbInvestorAccountLog = new DS.InvestorAccountLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorAccountLog = adap.GetInvestorAccountLogByInvestorID(InvestorID);
                if (tbInvestorAccountLog != null)
                {
                    int count = tbInvestorAccountLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorAccountLog newInvestorAccountLog = new Business.InvestorAccountLog();
                        newInvestorAccountLog.Amount = tbInvestorAccountLog[i].Amount;
                        newInvestorAccountLog.Comment = tbInvestorAccountLog[i].Comment;
                        newInvestorAccountLog.Date = tbInvestorAccountLog[i].Date;
                        newInvestorAccountLog.DealID = tbInvestorAccountLog[i].DealID;
                        newInvestorAccountLog.ID = tbInvestorAccountLog[i].ID;
                        newInvestorAccountLog.InvestorID = tbInvestorAccountLog[i].InvestorID;
                        newInvestorAccountLog.Name = tbInvestorAccountLog[i].Name;

                        Result.Add(newInvestorAccountLog);
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
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        /// <returns></returns>
        internal List<Business.InvestorAccountLog> GetInvestorAccountLogWithTime(int InvestorID, DateTime TimeStart, DateTime TimeEnd)
        {
            List<Business.InvestorAccountLog> Result = new List<Business.InvestorAccountLog>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();
            DS.InvestorAccountLogDataTable tbInvestorAccountLog = new DS.InvestorAccountLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorAccountLog = adap.GetInvestorAccountLogWithTime(TimeStart, TimeEnd, InvestorID);
                if (tbInvestorAccountLog != null)
                {
                    int count = tbInvestorAccountLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorAccountLog newInvestorAccountLog = new Business.InvestorAccountLog();
                        newInvestorAccountLog.Amount = tbInvestorAccountLog[i].Amount;
                        newInvestorAccountLog.Comment = tbInvestorAccountLog[i].Comment;
                        newInvestorAccountLog.Date = tbInvestorAccountLog[i].Date;
                        newInvestorAccountLog.DealID = tbInvestorAccountLog[i].DealID;
                        newInvestorAccountLog.ID = tbInvestorAccountLog[i].ID;
                        newInvestorAccountLog.InvestorID = tbInvestorAccountLog[i].InvestorID;
                        newInvestorAccountLog.Name = tbInvestorAccountLog[i].Name;

                        Result.Add(newInvestorAccountLog);
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
        /// <param name="RowNumber"></param>
        /// <param name="From"></param>
        /// <returns></returns>
        //internal List<Business.OrderData> GetInvestorLogByInvestorID(int InvestorID, int RowNumber, int From)
        //{
        //    List<Business.OrderData> Result = new List<Business.OrderData>();
        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
        //    DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();
        //    DS.InvestorAccountLogDataTable tbInvestorAccountLog = new DS.InvestorAccountLogDataTable();

        //    try
        //    {
        //        conn.Open();
        //        adap.Connection = conn;
        //        tbInvestorAccountLog = adap.GetInvestorLogByInvestorID(From, InvestorID, RowNumber);
        //        if (tbInvestorAccountLog != null)
        //        {
        //            int count = tbInvestorAccountLog.Count;
        //            for (int i = 0; i < count; i++)
        //            {
        //                Business.OrderData newOrderData = new Business.OrderData();
        //                newOrderData.OrderCode = "IAL01";
        //                newOrderData.Code = tbInvestorAccountLog[i].DealID;
        //                //newOrderData.Login
        //                newOrderData.AgentCommission = 0;
        //                //newOrderData.ClosePrice = tbCommandHistory[i].ClosePrice;
        //                //newOrderData.CloseTime = tbCommandHistory[i].CloseTime;
        //                //newOrderData.Code = tbOnlineCommand[i].inves
        //                //newOrderData.Comment = tbOnlineCommand[i]
        //                //newOrderData.Commission = tbCommandHistory[i].Commission;
        //                //newOrderData.ExpDate = tbCommandHistory[i].ExpTime;
        //                //newOrderData.Lots = tbCommandHistory[i].Size;
        //                //newOrderData.MarginRate =
        //                //newOrderData.OneConvRate
        //                //newOrderData.OpenPrice = tbCommandHistory[i].OpenPrice;
        //                //newOrderData.OpenTime = tbCommandHistory[i].OpenTime;
        //                //newOrderData.Profit = tbCommandHistory[i].Profit;
        //                //newOrderData.StopLoss = tbCommandHistory[i].StopLoss;
        //                //newOrderData.Swaps = tbCommandHistory[i].Swap;
        //                //newOrderData.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbCommandHistory[i].SymbolID);
        //                //newOrderData.TakeProfit = tbCommandHistory[i].TakeProfit;
        //                //newOrderData.Taxes = tbCommandHistory[i].Taxes;
        //                //newOrderData.TwoConvRate
        //                newOrderData.Type = tbInvestorAccountLog[i].Name;
        //                //newOrderData.ValueDate

        //                Result.Add(newOrderData);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }

        //    return Result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Comment"></param>
        /// <returns></returns>
        internal List<Business.InvestorAccountLog> GetInvestorAccountLogWithComment(string Comment)
        {
            List<Business.InvestorAccountLog> Result = new List<Business.InvestorAccountLog>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();
            DS.InvestorAccountLogDataTable tbInvestorAccountLog = new DS.InvestorAccountLogDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorAccountLog = adap.GetInvestorAccountLogWithComment(Comment);
                if (tbInvestorAccountLog != null)
                {
                    int count = tbInvestorAccountLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.InvestorAccountLog newInvestorAccountLog = new Business.InvestorAccountLog();
                        newInvestorAccountLog.Amount = tbInvestorAccountLog[i].Amount;
                        newInvestorAccountLog.Comment = tbInvestorAccountLog[i].Comment;
                        newInvestorAccountLog.Date = tbInvestorAccountLog[i].Date;
                        newInvestorAccountLog.DealID = tbInvestorAccountLog[i].DealID;
                        newInvestorAccountLog.ID = tbInvestorAccountLog[i].ID;
                        newInvestorAccountLog.InvestorID = tbInvestorAccountLog[i].InvestorID;
                        newInvestorAccountLog.Name = tbInvestorAccountLog[i].Name;

                        Result.Add(newInvestorAccountLog);
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
        /// <param name="objInvestorAccountLog"></param>
        /// <returns></returns>
        internal int AddNewInvestorAccountLog(Business.InvestorAccountLog objInvestorAccountLog)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewAccountLog(objInvestorAccountLog.DealID, objInvestorAccountLog.InvestorID, objInvestorAccountLog.Name, objInvestorAccountLog.Date, objInvestorAccountLog.Comment, objInvestorAccountLog.Amount, objInvestorAccountLog.Code).ToString());
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

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DealID"></param>
        /// <returns></returns>
        internal bool UpdateDealID(int ID, string DealID)
        {
            bool Result =false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int ResultUpdate = adap.UpdateDealID(DealID, ID);
                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
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
        /// <param name="objInvestorAccountLog"></param>
        /// <returns></returns>
        internal bool UpdateInvestorAccountLog(Business.InvestorAccountLog objInvestorAccountLog)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateInvestorAccountLog(objInvestorAccountLog.DealID, objInvestorAccountLog.InvestorID, objInvestorAccountLog.Name, objInvestorAccountLog.Date,
                    objInvestorAccountLog.Comment, objInvestorAccountLog.Amount, objInvestorAccountLog.Code, objInvestorAccountLog.ID);

                if (resultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
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
        /// <param name="InvestorAccountLogID"></param>
        /// <returns></returns>
        internal bool DeleteInvestorAccountLog(int InvestorAccountLogID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorAccountLogTableAdapter adap = new DSTableAdapters.InvestorAccountLogTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteInvestorAccountLog(InvestorAccountLogID);
                if (resultDelete > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }
    }
}
