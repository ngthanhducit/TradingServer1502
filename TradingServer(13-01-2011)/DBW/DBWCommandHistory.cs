using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWCommandHistory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetAllCommandHistory()
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetData();
                
                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;                        
                        //Investor

                        #region Find Trade Type In Market Area
                        //Find Trade Type
                        bool FlagType = false;
                        if (Business.Market.MarketArea != null)
                        {                            
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (FlagType == false)
                                {
                                    if (Business.Market.MarketArea[j].Type != null)
                                    {
                                        int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                        for (int n = 0; n < countTradeType; n++)
                                        {
                                            if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[i].CommandTypeID)
                                            {
                                                newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                                FlagType = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Find Investor In Investor List
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int m = 0; m < countInvestor; m++)
                            {
                                if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                    break;
                                }
                            }
                        }
                        #endregion
                        
                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;                        
                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];

                                    //if (Business.Market.SymbolList[k].CommandList == null)
                                    //    Business.Market.SymbolList[k].CommandList = new List<Business.OpenTrade>();

                                    //Business.Market.SymbolList[k].CommandList.Add(newOpenTrade);
                                    Flag = true;
                                    break;
                                }

                                if (Flag == false)
                                {
                                    if (Business.Market.SymbolList[k].RefSymbol != null)
                                    {
                                        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol,newOpenTrade,tbCommandHistory[i].SymbolID);

                                        if (newOpenTrade.Symbol != null)
                                            break;
                                    }
                                }                              
                            }
                        }
                        #endregion

                        Result.Add(newOpenTrade);
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
        /// <param name="numberGet"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetTopClosePosition(int numberGet)
        {
            List<Business.OpenTrade> result = new List<Business.OpenTrade>();

            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetTopOpenClosePosition(numberGet);
                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        //Investor

                        #region Find Trade Type In Market Area
                        //Find Trade Type
                        bool FlagType = false;
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (FlagType == false)
                                {
                                    if (Business.Market.MarketArea[j].Type != null)
                                    {
                                        int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                        for (int n = 0; n < countTradeType; n++)
                                        {
                                            if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[i].CommandTypeID)
                                            {
                                                newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                                FlagType = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Find Investor In Investor List
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int m = 0; m < countInvestor; m++)
                            {
                                if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                    break;
                                }
                            }
                        }
                        #endregion

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];
                                    Flag = true;
                                    break;
                                }

                                if (Flag == false)
                                {
                                    if (Business.Market.SymbolList[k].RefSymbol != null)
                                    {
                                        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol, newOpenTrade, tbCommandHistory[i].SymbolID);

                                        if (newOpenTrade.Symbol != null)
                                            break;
                                    }
                                }
                            }
                        }
                        #endregion

                        result.Add(newOpenTrade);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <returns></returns>
        internal Dictionary<int, double> GetDepositInvestor(List<int> listInvestorID)
        {
            Dictionary<int, double> result = new Dictionary<int, double>();

            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                if (listInvestorID != null)
                {
                    int count = listInvestorID.Count;
                    for (int i = 0; i < count; i++)
                    {
                        tbCommandHistory = adap.GetDepositByInvestorID(listInvestorID[i]);
                        if (tbCommandHistory != null)
                        {
                            int countHistory = tbCommandHistory.Count;
                            double deposit = 0;
                            for (int j = 0; j < countHistory; j++)
                            {
                                deposit += tbCommandHistory[j].Profit;
                            }

                            result.Add(listInvestorID[i], deposit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberGet"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetTopClosePosition(int numberGet, List<int> listInvestorID)
        {
            List<Business.OpenTrade> result = new List<Business.OpenTrade>();

            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            System.Data.SqlClient.SqlDataAdapter adap = new System.Data.SqlClient.SqlDataAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();

                string strInvestor = string.Empty;
                if (listInvestorID != null && listInvestorID.Count > 0)
                {
                    int count = listInvestorID.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (i == count - 1)
                            strInvestor += listInvestorID[i];
                        else
                            strInvestor += listInvestorID[i] + ",";
                    }
                }

                string sql = "SELECT TOP(" + numberGet + ") * FROM CommandHistory WHERE (InvestorID IN(" + strInvestor + ")) AND " +
                    " CommandTypeID NOT IN(13, 14, 15, 16, 21, 3, 4, 7, 8, 9, 10) AND (IsDelete = 0) ORDER BY CloseTime DESC";
                adap.SelectCommand = new System.Data.SqlClient.SqlCommand(sql, conn);
                tbCommandHistory = new DS.CommandHistoryDataTable();
                adap.Fill(tbCommandHistory);

                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        //Investor

                        #region Find Trade Type In Market Area
                        //Find Trade Type
                        bool FlagType = false;
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (FlagType == false)
                                {
                                    if (Business.Market.MarketArea[j].Type != null)
                                    {
                                        int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                        for (int n = 0; n < countTradeType; n++)
                                        {
                                            if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[i].CommandTypeID)
                                            {
                                                newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                                FlagType = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Find Investor In Investor List
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int m = 0; m < countInvestor; m++)
                            {
                                if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                    break;
                                }
                            }
                        }
                        #endregion

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;
                        newOpenTrade.IsClose = true;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];
                                    Flag = true;
                                    break;
                                }

                                if (Flag == false)
                                {
                                    if (Business.Market.SymbolList[k].RefSymbol != null)
                                    {
                                        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol, newOpenTrade, tbCommandHistory[i].SymbolID);

                                        if (newOpenTrade.Symbol != null && newOpenTrade.Symbol.SymbolID > 0)
                                            break;
                                    }
                                }
                            }
                        }
                        #endregion

                        result.Add(newOpenTrade);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                adap.SelectCommand.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetCommandHistoryByCommandID(int commandID)
        {
            Business.OpenTrade result = new Business.OpenTrade();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetCommandHistoryByID(commandID);
                if (tbCommandHistory != null)
                {
                    //CALL FUNCTION FILL INSTANCE OF COMMAND 
                    result = TradingServer.ClientFacade.FillInstanceOpenTrade(tbCommandHistory[0].SymbolID, tbCommandHistory[0].InvestorID, tbCommandHistory[0].CommandTypeID);

                    result.AgentCommission = tbCommandHistory[0].AgentCommission;
                    result.ClientCode = tbCommandHistory[0].ClientCode;
                    result.ClosePrice = tbCommandHistory[0].ClosePrice;
                    result.CloseTime = tbCommandHistory[0].CloseTime;
                    result.CommandCode = tbCommandHistory[0].CommandCode;
                    result.Comment = tbCommandHistory[0].Comment;
                    result.Commission = tbCommandHistory[0].Commission;
                    result.ExpTime = tbCommandHistory[0].ExpTime;
                    result.ID = tbCommandHistory[0].CommandHistoryID;
                    
                    result.IsClose = true;
                    result.OpenPrice = tbCommandHistory[0].OpenPrice;
                    result.OpenTime = tbCommandHistory[0].OpenTime;
                    result.Profit = tbCommandHistory[0].Profit;
                    result.Size = tbCommandHistory[0].Size;
                    result.StopLoss = tbCommandHistory[0].StopLoss;
                    result.Swap = tbCommandHistory[0].Swap;
                    result.TotalSwap = tbCommandHistory[0].TotalSwaps;
                    
                    result.TakeProfit = tbCommandHistory[0].TakeProfit;
                    result.Taxes = tbCommandHistory[0].Taxes;
                    result.AgentRefConfig = tbCommandHistory[0].AgentRefConfig;
                    result.IsActivePending = tbCommandHistory[0].IsActivePending;
                    result.IsStopLossAndTakeProfit = tbCommandHistory[0].IsStopLossTakeProfit;
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryWithStartLimit(int InvestorID, int ManagerID, DateTime TimeStart, DateTime TimeEnd)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            //List<Business.OpenTrade> tempResult = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                //Open Connection
                conn.Open();
                adap.Connection = conn;

                //Get Data
                tbCommandHistory = adap.GetCommandHistoryWithStartLimit(InvestorID, TimeStart, TimeEnd);

                //Close Connection
                adap.Connection.Close();
                conn.Close();

                #region FILL DATA
                if (tbCommandHistory != null && tbCommandHistory.Count > 0)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (tbCommandHistory[i].CommandTypeID == 21)
                            continue;

                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        newOpenTrade.Investor = new Business.Investor();
                        newOpenTrade.Investor.InvestorID = tbCommandHistory[i].InvestorID;
                        newOpenTrade.IsClose = true;
                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.ManagerID = ManagerID;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;

                        if (tbCommandHistory[i].SymbolID > 0)
                        {
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < countSymbol; j++)
                                {
                                    if (Business.Market.SymbolList[j].SymbolID == tbCommandHistory[i].SymbolID)
                                    {
                                        newOpenTrade.Symbol = Business.Market.SymbolList[j];
                                        break;
                                    }
                                }
                            }
                        }

                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Type = new Business.TradeType();
                        newOpenTrade.Type.ID = tbCommandHistory[i].CommandTypeID;
                        newOpenTrade.Type.Name = TradingServer.Facade.FacadeGetTypeNameByTypeID(newOpenTrade.Type.ID);
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;

                        Result.Add(newOpenTrade);
                    }
                }
                #endregion                
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
        /// <param name="listInvestorID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryWithListInvestorID(List<int> listInvestorID, DateTime start, DateTime end, int ManagerID)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            List<Business.OpenTrade> listLastCommand = new List<Business.OpenTrade>();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);     
       
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();
            System.Data.SqlClient.SqlDataAdapter adap = new System.Data.SqlClient.SqlDataAdapter();
            DSTableAdapters.CommandHistoryTableAdapter adapCommandHistory = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adapCommandHistory.Connection = conn;

                DateTime timeLastBalance = start.AddDays(-1);
                if (timeLastBalance.DayOfWeek == DayOfWeek.Sunday)
                {
                    timeLastBalance = timeLastBalance.AddDays(-2);
                }

                if (timeLastBalance.DayOfWeek == DayOfWeek.Saturday)
                {
                    timeLastBalance = timeLastBalance.AddDays(-1);
                }

                DateTime timeStartLastBalance = new DateTime(timeLastBalance.Year, timeLastBalance.Month, timeLastBalance.Day, 00, 00, 00);
                DateTime tempTimeEndLastBalance = timeStartLastBalance.AddDays(1);
                DateTime timeEndLastBalance = new DateTime(tempTimeEndLastBalance.Year, tempTimeEndLastBalance.Month, tempTimeEndLastBalance.Day, 00, 00, 00);

                #region FILL LIST INVESTOR SEARCH
                string listInvestor = string.Empty;
                if (listInvestorID != null)
                {
                    int countListInvestor = listInvestorID.Count;
                    for (int i = 0; i < countListInvestor; i++)
                    {
                        if (i == countListInvestor - 1)
                        {
                            listInvestor += listInvestorID[i];
                        }
                        else
                        {
                            listInvestor += listInvestorID[i] + ",";
                        }
                    }
                }
                #endregion               

                string sql = "SELECT * FROM CommandHistory WHERE (InvestorID IN(" + listInvestor + ")) AND (CloseTime >= '" + start + "') AND (CloseTime <= '" + end + "')" +
                    " AND CommandTypeID NOT IN(21) AND (IsDelete = 0)";
                adap.SelectCommand = new System.Data.SqlClient.SqlCommand(sql, conn);
                tbCommandHistory = new DS.CommandHistoryDataTable();
                adap.Fill(tbCommandHistory);

                //close connection
                conn.Close();

                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        bool isExistInvestor = false;
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;

                        if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == tbCommandHistory[i].InvestorID)
                                {   
                                    newOpenTrade.Investor = Business.Market.InvestorList[j];                                    
                                    isExistInvestor = true;
                                    break;
                                }
                            }
                        }

                        newOpenTrade.IsClose = true;
                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.ManagerID = ManagerID;

                        if (tbCommandHistory[i].SymbolID > 0)
                        {
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < countSymbol; j++)
                                {
                                    if (Business.Market.SymbolList[j].SymbolID == tbCommandHistory[i].SymbolID)
                                    {
                                        newOpenTrade.Symbol = Business.Market.SymbolList[j];
                                        break;
                                    }
                                }
                            }
                        }

                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.Type = new Business.TradeType();
                        newOpenTrade.Type.ID = tbCommandHistory[i].CommandTypeID;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.AgentRefConfig = tbCommandHistory[i].AgentRefConfig;
                        //newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        //newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;

                        Result.Add(newOpenTrade);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {   
                
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="SymbolID"></param>
        internal Business.Symbol FindSymbolReference(List<Business.Symbol> ListSymbol, Business.OpenTrade objOpenTrade, int SymbolID)
        {
            Business.Symbol Result = new Business.Symbol();
            bool Flag = false;
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].SymbolID == SymbolID)
                    {
                        Result = ListSymbol[i];
                        objOpenTrade.Symbol = ListSymbol[i];

                        Flag = true;
                        break;
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            this.FindSymbolReference(ListSymbol[i].RefSymbol, objOpenTrade, SymbolID);
                        }
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
        internal List<Business.OpenTrade> GetCommandHistoryByInvestor(int InvestorID)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetCommandHistoryByInvestor(InvestorID);

                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        //Investor

                        #region Find Trade Type In Market Area
                        //Find Trade Type
                        bool FlagType = false;
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (FlagType == false)
                                {
                                    if (Business.Market.MarketArea[j].Type != null)
                                    {
                                        int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                        for (int n = 0; n < countTradeType; n++)
                                        {
                                            if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[i].CommandTypeID)
                                            {
                                                newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                                FlagType = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Find Investor In Investor List
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int m = 0; m < countInvestor; m++)
                            {
                                if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                    break;
                                }
                            }
                        }
                        #endregion

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Flag == true)
                                    break;

                                if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[i].SymbolID)
                                {                                    
                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];

                                    Flag = true;
                                    break;
                                }

                                if (Flag == false)
                                {
                                    if (Business.Market.SymbolList[k].RefSymbol != null && Business.Market.SymbolList[k].RefSymbol.Count > 0)
                                    {
                                        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol, newOpenTrade, tbCommandHistory[i].SymbolID);
                                    }
                                }
                            }
                        }
                        #endregion

                        Result.Add(newOpenTrade);
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
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryByInvestorInMonth(int investorID)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetHistoryByInvestorInMonth(investorID);

                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        //Investor

                        #region Find Trade Type In Market Area
                        //Find Trade Type
                        bool FlagType = false;
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (FlagType == false)
                                {
                                    if (Business.Market.MarketArea[j].Type != null)
                                    {
                                        int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                        for (int n = 0; n < countTradeType; n++)
                                        {
                                            if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[i].CommandTypeID)
                                            {
                                                newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                                FlagType = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Find Investor In Investor List
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int m = 0; m < countInvestor; m++)
                            {
                                if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                    break;
                                }
                            }
                        }
                        #endregion

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Flag == true)
                                    break;

                                if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];

                                    Flag = true;
                                    break;
                                }

                                if (Flag == false)
                                {
                                    if (Business.Market.SymbolList[k].RefSymbol != null && Business.Market.SymbolList[k].RefSymbol.Count > 0)
                                    {
                                        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol, newOpenTrade, tbCommandHistory[i].SymbolID);
                                    }
                                }
                            }
                        }
                        #endregion

                        Result.Add(newOpenTrade);
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
        /// <param name="commandTypeID"></param>
        /// <param name="dateTime"></param>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetLastBalanceOfInvestor(int commandTypeID, DateTime dateTime, int investorID,DateTime timeEnd)
        {
            Business.OpenTrade result = new Business.OpenTrade();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetLastBalanceWithDateTime(commandTypeID, investorID, dateTime, timeEnd);

                if (tbCommandHistory != null)
                {
                    #region MAP LASS BALANCE OF INVESTOR
                    result.ClientCode = tbCommandHistory[0].ClientCode;
                    result.ClosePrice = tbCommandHistory[0].ClosePrice;
                    result.CloseTime = tbCommandHistory[0].CloseTime;
                    result.CommandCode = tbCommandHistory[0].CommandCode;
                    result.Commission = tbCommandHistory[0].Commission;
                    result.ExpTime = tbCommandHistory[0].ExpTime;
                    result.ID = tbCommandHistory[0].CommandHistoryID;
                    //Investor

                    result.Type = new Business.TradeType();

                    #region Find Investor In Investor List
                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int m = 0; m < countInvestor; m++)
                        {
                            if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[0].InvestorID)
                            {
                                result.Investor = Business.Market.InvestorList[m];
                                break;
                            }
                        }
                    }
                    #endregion

                    result.OpenPrice = tbCommandHistory[0].OpenPrice;
                    result.OpenTime = tbCommandHistory[0].OpenTime;
                    result.Profit = tbCommandHistory[0].Profit;
                    result.Size = tbCommandHistory[0].Size;
                    result.StopLoss = tbCommandHistory[0].StopLoss;
                    result.Swap = tbCommandHistory[0].Swap;
                    result.Comment = tbCommandHistory[0].Comment;
                    result.Taxes = tbCommandHistory[0].Taxes;
                    result.AgentCommission = tbCommandHistory[0].AgentCommission;
                    result.TakeProfit = tbCommandHistory[0].TakeProfit;
                    result.IsActivePending = tbCommandHistory[0].IsActivePending;
                    result.IsStopLossAndTakeProfit = tbCommandHistory[0].IsStopLossTakeProfit;

                    #region Find Symbol In Symbol List
                    if (Business.Market.SymbolList != null)
                    {
                        bool Flag = false;
                        int countSymbol = Business.Market.SymbolList.Count;
                        for (int k = 0; k < countSymbol; k++)
                        {
                            if (Flag == true)
                                break;

                            if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[0].SymbolID)
                            {
                                result.Symbol = Business.Market.SymbolList[k];

                                Flag = true;
                                break;
                            }
                        }
                    }
                    #endregion
                    #endregion                    
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetCommandHistoryWithTime(int InvestorID, DateTime TimeStart, DateTime TimeEnd)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetCommandHistoryWithTime(InvestorID, TimeStart, TimeEnd);
                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        //Investor

                        #region Find Trade Type In Market Area
                        //Find Trade Type
                        bool FlagType = false;
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (FlagType == false)
                                {
                                    if (Business.Market.MarketArea[j].Type != null)
                                    {
                                        int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                        for (int n = 0; n < countTradeType; n++)
                                        {
                                            if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[i].CommandTypeID)
                                            {
                                                newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                                FlagType = true;
                                                break;
                                            }
                                        }

                                        if (newOpenTrade.Type == null)
                                        {
                                            newOpenTrade.Type = new Business.TradeType();
                                            switch (tbCommandHistory[i].CommandTypeID)
                                            {
                                                case 13:
                                                    newOpenTrade.Type.Name = "AddDeposit";
                                                    break;  
                                                case 14:
                                                    newOpenTrade.Type.Name = "Withdrawals";
                                                    break;
                                                case 15:
                                                    newOpenTrade.Type.Name = "CreditIn";
                                                    break;
                                                case 16:
                                                    newOpenTrade.Type.Name = "CreditOut";
                                                    break;
                                            }
                                            newOpenTrade.Type.ID = tbCommandHistory[i].CommandTypeID;
                                            FlagType = true;
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Find Investor In Investor List
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int m = 0; m < countInvestor; m++)
                            {
                                if (Business.Market.InvestorList[m].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                    break;
                                }
                            }
                        }
                        #endregion

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Flag == true)
                                    break;

                                if (Business.Market.SymbolList[k].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];

                                    Flag = true;
                                    break;
                                }

                                if (Flag == false)
                                {
                                    if (Business.Market.SymbolList[k].RefSymbol != null && Business.Market.SymbolList[k].RefSymbol.Count > 0)
                                    {
                                        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol, newOpenTrade, tbCommandHistory[i].SymbolID);
                                    }
                                }
                            }
                        }
                        #endregion

                        Result.Add(newOpenTrade);
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
        /// <param name="rowNumber"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetAllHistoryStartEnd(int rowNumber, int from)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetAllHistoryWithStartStop(rowNumber, from);

                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        newOpenTrade.IsClose = true;

                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[j];
                                    break;
                                }
                            }
                        }

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;

                        if (Business.Market.SymbolList != null)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.SymbolList[j].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[j];
                                    break;
                                }
                            }
                        }

                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;

                        newOpenTrade.Type = new Business.TradeType();
                        newOpenTrade.Type.ID = tbCommandHistory[i].CommandTypeID;
                        newOpenTrade.Type.Name = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbCommandHistory[i].CommandTypeID);
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;

                        Result.Add(newOpenTrade);
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
        internal List<Business.OpenTrade> GetHistoryByInvestorID(int InvestorID, int RowNumber, int From)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetHistoryByInvestorID(RowNumber, InvestorID, From);
                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;
                        newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                        newOpenTrade.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOpenTrade.CloseTime = tbCommandHistory[i].CloseTime;
                        newOpenTrade.CommandCode = tbCommandHistory[i].CommandCode;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.Commission = tbCommandHistory[i].Commission;
                        newOpenTrade.ExpTime = tbCommandHistory[i].ExpTime;
                        newOpenTrade.ID = tbCommandHistory[i].CommandHistoryID;
                        newOpenTrade.IsClose = true;

                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == tbCommandHistory[i].InvestorID)
                                {
                                    newOpenTrade.Investor = Business.Market.InvestorList[j];
                                    break;
                                }
                            }
                        }

                        newOpenTrade.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOpenTrade.OpenTime = tbCommandHistory[i].OpenTime;
                        newOpenTrade.Profit = tbCommandHistory[i].Profit;
                        newOpenTrade.Size = tbCommandHistory[i].Size;
                        newOpenTrade.StopLoss = tbCommandHistory[i].StopLoss;
                        newOpenTrade.Swap = tbCommandHistory[i].Swap;
                        newOpenTrade.IsClose = true;

                        if (Business.Market.SymbolList != null)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++) 
                            {
                                if (Business.Market.SymbolList[j].SymbolID == tbCommandHistory[i].SymbolID)
                                {
                                    newOpenTrade.Symbol = Business.Market.SymbolList[j];
                                    break;
                                }
                            }
                        }

                        newOpenTrade.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOpenTrade.Taxes = tbCommandHistory[i].Taxes;
                        newOpenTrade.Comment = tbCommandHistory[i].Comment;
                        newOpenTrade.AgentCommission = tbCommandHistory[i].AgentCommission;

                        newOpenTrade.Type = new Business.TradeType();
                        newOpenTrade.Type.ID = tbCommandHistory[i].CommandTypeID;
                        newOpenTrade.Type.Name = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbCommandHistory[i].CommandTypeID);
                        newOpenTrade.IsActivePending = tbCommandHistory[i].IsActivePending;
                        newOpenTrade.IsStopLossAndTakeProfit = tbCommandHistory[i].IsStopLossTakeProfit;
                        
                        Result.Add(newOpenTrade);
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
        /// <param name="commandCode"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetHistoryByCommandCode(string commandCode)
        {
            Business.OpenTrade result = new Business.OpenTrade();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandHistory = adap.GetHistoryByCommandCode(commandCode);

                if (tbCommandHistory != null)
                {
                    result.AgentCommission = tbCommandHistory[0].AgentCommission;
                    result.ClientCode = tbCommandHistory[0].ClientCode;
                    result.ClosePrice = tbCommandHistory[0].ClosePrice;
                    result.CloseTime = tbCommandHistory[0].CloseTime;
                    result.CommandCode = tbCommandHistory[0].CommandCode;
                    result.Comment = tbCommandHistory[0].Comment;
                    result.Commission = tbCommandHistory[0].Commission;
                    result.ExpTime = tbCommandHistory[0].ExpTime;
                    result.ID = tbCommandHistory[0].CommandHistoryID;
                    result.IsClose = true;

                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].InvestorID == tbCommandHistory[0].InvestorID)
                            {
                                result.Investor = Business.Market.InvestorList[j];
                                break;
                            }
                        }
                    }

                    result.OpenPrice = tbCommandHistory[0].OpenPrice;
                    result.OpenTime = tbCommandHistory[0].OpenTime;
                    result.Profit = tbCommandHistory[0].Profit;
                    result.Size = tbCommandHistory[0].Size;
                    result.StopLoss = tbCommandHistory[0].StopLoss;
                    result.Swap = tbCommandHistory[0].Swap;

                    if (Business.Market.SymbolList != null)
                    {
                        int countSymbol = Business.Market.SymbolList.Count;
                        for (int j = 0; j < countSymbol; j++)
                        {
                            if (Business.Market.SymbolList[j].SymbolID == tbCommandHistory[0].SymbolID)
                            {
                                result.Symbol = Business.Market.SymbolList[j];
                                break;
                            }
                        }
                    }

                    result.TakeProfit = tbCommandHistory[0].TakeProfit;
                    result.Taxes = tbCommandHistory[0].Taxes;
                    result.Comment = tbCommandHistory[0].Comment;
                    result.AgentCommission = tbCommandHistory[0].AgentCommission;
                    result.IsActivePending = tbCommandHistory[0].IsActivePending;
                    result.IsStopLossAndTakeProfit = tbCommandHistory[0].IsStopLossTakeProfit;

                    if (Business.Market.MarketArea != null)
                    {
                        int countMarketArea = Business.Market.MarketArea.Count;
                        for (int j = 0; j < countMarketArea; j++)
                        {
                            if (Business.Market.MarketArea[j].Type != null)
                            {
                                int countType = Business.Market.MarketArea[j].Type.Count;
                                for (int n = 0; n < countType; n++)
                                {
                                    if (Business.Market.MarketArea[j].Type[n].ID == tbCommandHistory[0].CommandTypeID)
                                    {
                                        result.Type = Business.Market.MarketArea[j].Type[n];
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (result.Type == null)
                    {
                        result.Type = new Business.TradeType();
                        result.Type.ID = tbCommandHistory[0].CommandTypeID;
                        result.Type.Name = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbCommandHistory[0].CommandTypeID);
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal int AddNewCommandHistory(int InvestorID, int CommandTypeID, string CommandCode, DateTime OpenTime, double OpenPrice, DateTime CloseTime,
                                            double ClosePrice, double Profit, double Swap, double Commission, DateTime ExpTime, double Size, double StopLoss,
                                            double TakeProfit, string clientCode, int SymbolID, double Taxes, double AgentCommission,string Comment,double totalSwap, 
                                            int RefCommandID, string AgentRefConfig, bool isActive, bool isStopLossTakeProfit)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            SqlTransaction tran;
            conn.Open();
            adap.Connection = conn;

            tran = conn.BeginTransaction();
            adap.Transaction = tran;
            try
            {
                if (SymbolID > 0)
                {                    
                    //Swap += totalSwap;
                    //Swap = totalSwap;
                    //totalSwap = 0;

                    Result = int.Parse(adap.AddNewCommandHistory(InvestorID, CommandTypeID, OpenTime, OpenPrice, CloseTime, ClosePrice, Math.Round(Profit, 2), Swap,
                                                                    Commission, ExpTime, Size, StopLoss, TakeProfit, clientCode, CommandCode, SymbolID, Taxes,
                                                                    AgentCommission, Comment, totalSwap, false, RefCommandID, AgentRefConfig, isStopLossTakeProfit, isActive).ToString());
                    tran.Commit();
                }
                else
                {
                    Result = int.Parse(adap.AddNewCommandHistory(InvestorID, CommandTypeID, OpenTime, OpenPrice, CloseTime, ClosePrice, Math.Round(Profit, 2), Swap,
                                                                    Commission, ExpTime, Size, StopLoss, TakeProfit, clientCode, CommandCode, null, Taxes,
                                                                    AgentCommission, Comment, totalSwap, false, RefCommandID, AgentRefConfig, isStopLossTakeProfit, isActive).ToString());

                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();

                TradingServer.Facade.FacadeAddNewSystemLog(1, ex.ToString(), "[insert command history]", "", "");
                return -1;
            }
            finally
            {
                tran.Dispose();
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// FUNCTION USING INSERT LAST BALANCE
        /// </summary>
        /// <param name="listInvestor"></param>
        internal void AddCommandHistoryWithListCommand(List<Business.Investor> listInvestor, DateTime time)
        {
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();
            DateTime timeCurrent = new DateTime(time.Year, time.Month, time.Day, 23, 59, 00);
            try
            {
                conn.Open();
                adap.Connection = conn;

                if (listInvestor != null)
                {
                    int count = listInvestor.Count;
                    for (int i = 0; i < count; i++)
                    {
                        adap.AddNewCommandHistory(listInvestor[i].InvestorID, 21, timeCurrent, 0, timeCurrent, 0, listInvestor[i].Balance, 0, 0, timeCurrent,
                            0, 0, 0, "", "", -1, 0, 0, "[last balance account] " + listInvestor[i].Code, 0, false, -1, "", false, false);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandHistoryID"></param>
        /// <returns></returns>
        internal bool DeleteCommandHistory(int CommandHistoryID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteCommandHistory(CommandHistoryID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal bool DeleteCommandHistoryByInvestorID(int investorID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteCommandHistoryByInvestorID(investorID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objOpenTrade"></param>
        /// <returns></returns>
        internal bool UpdateCommandHistory(Business.OpenTrade objOpenTrade)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = 0;
                if (objOpenTrade.Symbol != null)
                {
                    resultUpdate = adap.UpdateCommandHistory(objOpenTrade.Type.ID, objOpenTrade.OpenTime,
                    objOpenTrade.OpenPrice, objOpenTrade.CloseTime, objOpenTrade.ClosePrice, Math.Round(objOpenTrade.Profit, 2), objOpenTrade.Swap, objOpenTrade.Commission,
                    objOpenTrade.ExpTime, objOpenTrade.Size, objOpenTrade.StopLoss, objOpenTrade.TakeProfit, objOpenTrade.Symbol.SymbolID,
                    objOpenTrade.Taxes, objOpenTrade.Comment, objOpenTrade.AgentCommission, objOpenTrade.TotalSwap, false, objOpenTrade.IsStopLossAndTakeProfit,
                    objOpenTrade.IsActivePending, objOpenTrade.ID);
                }
                else
                {
                    resultUpdate = adap.UpdateCommandHistory(objOpenTrade.Type.ID, objOpenTrade.OpenTime,
                    objOpenTrade.OpenPrice, objOpenTrade.OpenTime, objOpenTrade.ClosePrice, Math.Round(objOpenTrade.Profit, 2), objOpenTrade.Swap, objOpenTrade.Commission,
                    objOpenTrade.ExpTime, objOpenTrade.Size, objOpenTrade.StopLoss, objOpenTrade.TakeProfit, null,
                    objOpenTrade.Taxes, objOpenTrade.Comment, objOpenTrade.AgentCommission, objOpenTrade.TotalSwap, false,
                    objOpenTrade.IsStopLossAndTakeProfit, objOpenTrade.IsActivePending, objOpenTrade.ID);
                }                

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
        /// <param name="CommandCode"></param>
        /// <param name="CommandHistoryID"></param>
        /// <returns></returns>
        internal bool UpdateCommandCodeHistory(string CommandCode, int CommandHistoryID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateCommandCodeHistory(CommandCode, CommandHistoryID);

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
        /// <param name="commandHistoryID"></param>
        /// <param name="totalSwap"></param>
        /// <returns></returns>
        internal bool UpdateTotalSwapHistory(int commandHistoryID, double totalSwap)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateTotalSwap(totalSwap, commandHistoryID);
                if (resultUpdate > 0)
                    result = true;
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDelete"></param>
        /// <param name="commandHistoryID"></param>
        /// <returns></returns>
        internal bool UpdateIsDeleteHistory(bool isDelete, int commandHistoryID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandHistoryTableAdapter adap = new DSTableAdapters.CommandHistoryTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateIsDeleteCommand(isDelete, commandHistoryID);
                if (resultUpdate > 0)
                    result = true;
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

            return result;
        }
    }
}
