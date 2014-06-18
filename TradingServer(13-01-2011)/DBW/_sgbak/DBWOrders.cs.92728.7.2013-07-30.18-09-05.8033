using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWOrders
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        internal List<Business.OrderData> GetOrderByInvestorID(int InvestorID, int Start, int Limit)
        {
            List<Business.OrderData> Result = new List<Business.OrderData>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adapInvestor = new DSTableAdapters.InvestorTableAdapter();
            DSTableAdapters.OnlineCommandTableAdapter adapOpenTrade = new DSTableAdapters.OnlineCommandTableAdapter();
            DSTableAdapters.CommandHistoryTableAdapter adapHistory = new DSTableAdapters.CommandHistoryTableAdapter();
            DSTableAdapters.InvestorAccountLogTableAdapter adapAccountLog = new DSTableAdapters.InvestorAccountLogTableAdapter();

            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();
            DS.InvestorAccountLogDataTable tbInvestorAccountLog = new DS.InvestorAccountLogDataTable();

            try
            {
                conn.Open();
                adapInvestor.Connection = conn;
                adapOpenTrade.Connection = conn;
                adapHistory.Connection = conn;
                adapAccountLog.Connection = conn;

                tbOnlineCommand = adapOpenTrade.GetOpenTradeByInvestorID(Start, InvestorID, Limit);
                tbCommandHistory = adapHistory.GetHistoryByInvestorID(Start, InvestorID, Limit);
                tbInvestorAccountLog = adapAccountLog.GetInvestorLogByInvestorID(Start, InvestorID, Limit);

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OrderData newOrderData = new Business.OrderData();
                        newOrderData.OrderCode = "OPT001";
                        newOrderData.Code = tbOnlineCommand[i].CommandCode;
                        newOrderData.ID = tbOnlineCommand[i].OnlineCommandID;

                        tbInvestor = adapInvestor.GetInvestorNameByInvestorID(tbOnlineCommand[i].InvestorID);
                        if (tbInvestor != null)
                            newOrderData.Login = tbInvestor[0].Code;

                        newOrderData.InvestorID = tbOnlineCommand[i].InvestorID;
                        newOrderData.AgentCommission = 0;
                        newOrderData.ClosePrice = tbOnlineCommand[i].ClosePrice;
                        newOrderData.CloseTime = tbOnlineCommand[i].CloseTime;                        
                        //newOrderData.Comment = tbOnlineCommand[i]
                        newOrderData.Commission = tbOnlineCommand[i].Commission;
                        newOrderData.ExpDate = tbOnlineCommand[i].ExpTime;
                        newOrderData.Lots = tbOnlineCommand[i].Size;
                        //newOrderData.MarginRate =
                        //newOrderData.OneConvRate
                        newOrderData.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOrderData.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOrderData.Profit = tbOnlineCommand[i].Profit;
                        newOrderData.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOrderData.Swaps = tbOnlineCommand[i].Swap;
                        newOrderData.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbOnlineCommand[i].SymbolID);
                        newOrderData.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        newOrderData.Taxes = tbOnlineCommand[i].Taxes;
                        //newOrderData.TwoConvRate
                        newOrderData.Type = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbOnlineCommand[i].CommandTypeID);
                        //newOrderData.ValueDate

                        Result.Add(newOrderData);
                    }
                }

                if (tbCommandHistory != null)
                {
                    int count = tbCommandHistory.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OrderData newOrderData = new Business.OrderData();
                        newOrderData.OrderCode = "CMH01";
                        
                        newOrderData.Code = tbCommandHistory[i].CommandCode;
                        newOrderData.ID = tbCommandHistory[i].CommandHistoryID;

                        tbInvestor = adapInvestor.GetInvestorNameByInvestorID(tbCommandHistory[i].InvestorID);
                        if (tbInvestor != null)
                            newOrderData.Login = tbInvestor[0].Code;

                        newOrderData.InvestorID = tbCommandHistory[i].InvestorID;
                        newOrderData.AgentCommission = 0;
                        newOrderData.ClosePrice = tbCommandHistory[i].ClosePrice;
                        newOrderData.CloseTime = tbCommandHistory[i].CloseTime;
                        //newOrderData.Code = tbOnlineCommand[i].inves
                        //newOrderData.Comment = tbOnlineCommand[i]
                        newOrderData.Commission = tbCommandHistory[i].Commission;
                        newOrderData.ExpDate = tbCommandHistory[i].ExpTime;
                        newOrderData.Lots = tbCommandHistory[i].Size;
                        //newOrderData.MarginRate =
                        //newOrderData.OneConvRate
                        newOrderData.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOrderData.OpenTime = tbCommandHistory[i].OpenTime;
                        newOrderData.Profit = tbCommandHistory[i].Profit;
                        newOrderData.StopLoss = tbCommandHistory[i].StopLoss;
                        newOrderData.Swaps = tbCommandHistory[i].Swap;
                        newOrderData.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbCommandHistory[i].SymbolID);
                        newOrderData.TakeProfit = tbCommandHistory[i].TakeProfit;
                        newOrderData.Taxes = tbCommandHistory[i].Taxes;
                        //newOrderData.TwoConvRate
                        switch (tbCommandHistory[i].CommandTypeID)
                        {
                            case 13:
                                {
                                    newOrderData.Type = "ADP01";
                                }
                                break;
                            case 14:
                                {
                                    newOrderData.Type = "WRD01";
                                }
                                break;
                            case 15:
                                {
                                    newOrderData.Type = "ACD01";
                                }
                                break;
                            case 16:
                                {
                                    newOrderData.Type = "CRD01";
                                }
                                break;
                            default:
                                {
                                    newOrderData.Type = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbCommandHistory[i].CommandTypeID);
                                }
                                break;
                        }
                        //newOrderData.Type = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbCommandHistory[i].CommandTypeID);
                        //newOrderData.ValueDate

                        Result.Add(newOrderData);
                    }
                }

                if (tbInvestorAccountLog != null)
                {
                    int count = tbInvestorAccountLog.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OrderData newOrderData = new Business.OrderData();
                        newOrderData.OrderCode = "IAL01";
                        newOrderData.Code = tbInvestorAccountLog[i].DealID;
                        newOrderData.ID = tbInvestorAccountLog[i].ID;

                        tbInvestor = adapInvestor.GetInvestorNameByInvestorID(tbInvestorAccountLog[i].InvestorID);
                        if (tbInvestor != null)
                            newOrderData.Login = tbInvestor[0].Code;

                        newOrderData.InvestorID = tbInvestorAccountLog[i].InvestorID;
                        //newOrderData.AgentCommission = 0;
                        //newOrderData.ClosePrice = tbCommandHistory[i].ClosePrice;
                        //newOrderData.CloseTime = tbCommandHistory[i].CloseTime;
                        //newOrderData.Code = tbOnlineCommand[i].inves
                        //newOrderData.Comment = tbOnlineCommand[i]
                        //newOrderData.Commission = tbCommandHistory[i].Commission;
                        //newOrderData.ExpDate = tbCommandHistory[i].ExpTime;
                        //newOrderData.Lots = tbCommandHistory[i].Size;
                        //newOrderData.MarginRate =
                        //newOrderData.OneConvRate
                        //newOrderData.OpenPrice = tbCommandHistory[i].OpenPrice;
                        newOrderData.OpenTime = tbInvestorAccountLog[i].Date;
                        //newOrderData.Profit = tbCommandHistory[i].Profit;
                        //newOrderData.StopLoss = tbCommandHistory[i].StopLoss;
                        //newOrderData.Swaps = tbCommandHistory[i].Swap;
                        //newOrderData.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbCommandHistory[i].SymbolID);
                        //newOrderData.TakeProfit = tbCommandHistory[i].TakeProfit;
                        //newOrderData.Taxes = tbCommandHistory[i].Taxes;
                        //newOrderData.TwoConvRate
                        newOrderData.Type = tbInvestorAccountLog[i].Code;
                        newOrderData.Comment = tbInvestorAccountLog[i].Comment;
                        newOrderData.Profit = tbInvestorAccountLog[i].Amount;
                        //newOrderData.ValueDate

                        Result.Add(newOrderData);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adapInvestor.Connection.Close();
                adapOpenTrade.Connection.Close();
                adapHistory.Connection.Close();
                adapAccountLog.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.OrderData GetOrderByCode(string Code)
        {
            Business.OrderData Result = new Business.OrderData();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adapOnlineCommand = new DSTableAdapters.OnlineCommandTableAdapter();
            DSTableAdapters.CommandHistoryTableAdapter adapCommandHistory = new DSTableAdapters.CommandHistoryTableAdapter();
            DSTableAdapters.InvestorAccountLogTableAdapter adapAccountLog = new DSTableAdapters.InvestorAccountLogTableAdapter();
            DSTableAdapters.InvestorTableAdapter adapInvestor = new DSTableAdapters.InvestorTableAdapter();

            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();
            DS.CommandHistoryDataTable tbCommandHistory = new DS.CommandHistoryDataTable();
            DS.InvestorAccountLogDataTable tbAccountLog = new DS.InvestorAccountLogDataTable();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();

            try
            {
                bool flag = false;
                conn.Open();
                adapAccountLog.Connection = conn;
                adapCommandHistory.Connection = conn;
                adapOnlineCommand.Connection = conn;
                adapInvestor.Connection = conn;

                tbOnlineCommand = adapOnlineCommand.GetOnlineCommandByCommandCode(Code);
                if (tbOnlineCommand != null && tbOnlineCommand.Count > 0)
                {
                    Result.OrderCode = "OPT001";
                    Result.Code = tbOnlineCommand[0].CommandCode;

                    tbInvestor = adapInvestor.GetInvestorNameByInvestorID(tbOnlineCommand[0].InvestorID);
                    if (tbInvestor != null)
                        Result.Login = tbInvestor[0].Code;

                    Result.InvestorID = tbOnlineCommand[0].InvestorID;
                    Result.AgentCommission = 0;
                    Result.ClosePrice = tbOnlineCommand[0].ClosePrice;
                    Result.CloseTime = tbOnlineCommand[0].CloseTime;
                    //newOrderData.Comment = tbOnlineCommand[i]
                    Result.Commission = tbOnlineCommand[0].Commission;
                    Result.ExpDate = tbOnlineCommand[0].ExpTime;
                    Result.Lots = tbOnlineCommand[0].Size;
                    //newOrderData.MarginRate =
                    //newOrderData.OneConvRate
                    Result.OpenPrice = tbOnlineCommand[0].OpenPrice;
                    Result.OpenTime = tbOnlineCommand[0].OpenTime;
                    Result.Profit = tbOnlineCommand[0].Profit;
                    Result.StopLoss = tbOnlineCommand[0].StopLoss;
                    Result.Swaps = tbOnlineCommand[0].Swap;
                    Result.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbOnlineCommand[0].SymbolID);
                    Result.TakeProfit = tbOnlineCommand[0].TakeProfit;
                    Result.Taxes = tbOnlineCommand[0].Taxes;
                    //newOrderData.TwoConvRate
                    Result.Type = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbOnlineCommand[0].CommandTypeID);
                    //newOrderData.ValueDate
                    flag = true;
                }

                if (!flag)
                {
                    tbCommandHistory = adapCommandHistory.GetCommandHistoryByCommandCode(Code);
                    if (tbCommandHistory != null && tbCommandHistory.Count > 0)
                    {
                        Result.OrderCode = "CMH01";
                        Result.Code = tbCommandHistory[0].CommandCode;

                        tbInvestor = adapInvestor.GetInvestorNameByInvestorID(tbCommandHistory[0].InvestorID);
                        if (tbInvestor != null)
                            Result.Login = tbInvestor[0].Code;

                        Result.InvestorID = tbCommandHistory[0].InvestorID;
                        Result.AgentCommission = 0;
                        Result.ClosePrice = tbCommandHistory[0].ClosePrice;
                        Result.CloseTime = tbCommandHistory[0].CloseTime;
                        //newOrderData.Code = tbOnlineCommand[i].inves
                        //newOrderData.Comment = tbOnlineCommand[i]
                        Result.Commission = tbCommandHistory[0].Commission;
                        Result.ExpDate = tbCommandHistory[0].ExpTime;
                        Result.Lots = tbCommandHistory[0].Size;
                        //newOrderData.MarginRate =
                        //newOrderData.OneConvRate
                        Result.OpenPrice = tbCommandHistory[0].OpenPrice;
                        Result.OpenTime = tbCommandHistory[0].OpenTime;
                        Result.Profit = tbCommandHistory[0].Profit;
                        Result.StopLoss = tbCommandHistory[0].StopLoss;
                        Result.Swaps = tbCommandHistory[0].Swap;
                        Result.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbCommandHistory[0].SymbolID);
                        Result.TakeProfit = tbCommandHistory[0].TakeProfit;
                        Result.Taxes = tbCommandHistory[0].Taxes;
                        //newOrderData.TwoConvRate
                        Result.Type = TradingServer.Facade.FacadeGetTypeNameByTypeID(tbCommandHistory[0].CommandTypeID);
                        //newOrderData.ValueDate

                        flag = true;
                    }
                }

                if (!flag)
                {
                    tbAccountLog = adapAccountLog.GetInvestorAccountLogByCode(Code);
                    if (tbAccountLog != null && tbAccountLog.Count > 0)
                    {
                        Result.OrderCode = "IAL01";
                        Result.Code = tbAccountLog[0].DealID;

                        tbInvestor = adapInvestor.GetInvestorNameByInvestorID(tbAccountLog[0].InvestorID);
                        if (tbInvestor != null)
                            Result.Login = tbInvestor[0].Code;

                        Result.InvestorID = tbAccountLog[0].InvestorID;
                        //newOrderData.AgentCommission = 0;
                        //newOrderData.ClosePrice = tbCommandHistory[i].ClosePrice;
                        //newOrderData.CloseTime = tbCommandHistory[i].CloseTime;
                        //newOrderData.Code = tbOnlineCommand[i].inves
                        //newOrderData.Comment = tbOnlineCommand[i]
                        //newOrderData.Commission = tbCommandHistory[i].Commission;
                        //newOrderData.ExpDate = tbCommandHistory[i].ExpTime;
                        //newOrderData.Lots = tbCommandHistory[i].Size;
                        //newOrderData.MarginRate =
                        //newOrderData.OneConvRate
                        //newOrderData.OpenPrice = tbCommandHistory[i].OpenPrice;
                        Result.OpenTime = tbAccountLog[0].Date;
                        //newOrderData.Profit = tbCommandHistory[i].Profit;
                        //newOrderData.StopLoss = tbCommandHistory[i].StopLoss;
                        //newOrderData.Swaps = tbCommandHistory[i].Swap;
                        //newOrderData.Symbol = TradingServer.Facade.FacadeGetSymbolNameBySymbolID(tbCommandHistory[i].SymbolID);
                        //newOrderData.TakeProfit = tbCommandHistory[i].TakeProfit;
                        //newOrderData.Taxes = tbCommandHistory[i].Taxes;
                        //newOrderData.TwoConvRate
                        Result.Type = tbAccountLog[0].Code;
                        Result.Comment = tbAccountLog[0].Comment;
                        Result.Profit = tbAccountLog[0].Amount;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adapAccountLog.Connection.Close();
                adapCommandHistory.Connection.Close();
                adapInvestor.Connection.Close();
                adapOnlineCommand.Connection.Close();
                conn.Close();
            }

            return Result;
        }
    }
}
