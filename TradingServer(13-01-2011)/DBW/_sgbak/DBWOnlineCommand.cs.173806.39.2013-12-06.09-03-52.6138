using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWOnlineCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetAllOnlineCommand()
        {            
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetData();

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ID = tbOnlineCommand[i].OnlineCommandID;

                        #region Find Symbol In Symbol List
                        if (Business.Market.SymbolList != null)
                        {
                            bool Flag = false;
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int k = 0; k < countSymbol; k++)
                            {
                                if (Business.Market.SymbolList[k].SymbolID == tbOnlineCommand[i].SymbolID)
                                {
                                    #region SEARCH IN MARKET OF SYMBOL AND GET TYPE OF COMMAND
                                    bool tempFlag = false;
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int countMarketArea = Business.Market.MarketArea.Count;
                                        for (int n = 0; n < countMarketArea; n++)
                                        {
                                            if (tempFlag)
                                                break;

                                            if (Business.Market.MarketArea[n].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[n].Type.Count;
                                                for (int m = 0; m < countType; m++)
                                                {
                                                    if (Business.Market.MarketArea[n].Type[m].ID == tbOnlineCommand[i].CommandTypeID)
                                                    {
                                                        newOpenTrade.Type = Business.Market.MarketArea[n].Type[m];
                                                        tempFlag = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion                                    

                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];
                                    newOpenTrade.ClientCode = tbOnlineCommand[i].ClientCode;
                                    
                                    switch (newOpenTrade.Type.ID)
                                    { 
                                        case 1:
                                            newOpenTrade.ClosePrice = newOpenTrade.Symbol.TickValue.Bid;
                                            break;
                                        case 2:
                                            newOpenTrade.ClosePrice = newOpenTrade.Symbol.TickValue.Ask;
                                            break;
                                        case 11:
                                            newOpenTrade.ClosePrice = newOpenTrade.Symbol.TickValue.Bid;
                                            break;
                                        case 12:
                                            newOpenTrade.ClosePrice = newOpenTrade.Symbol.TickValue.Ask;
                                            break;
                                    }
                                    
                                    newOpenTrade.CloseTime = tbOnlineCommand[i].CloseTime;
                                    newOpenTrade.ExpTime = tbOnlineCommand[i].ExpTime;
                                    newOpenTrade.IsClose = tbOnlineCommand[i].IsClose;
                                    newOpenTrade.OpenPrice = tbOnlineCommand[i].OpenPrice;
                                    newOpenTrade.OpenTime = tbOnlineCommand[i].OpenTime;
                                    newOpenTrade.Size = tbOnlineCommand[i].Size;
                                    newOpenTrade.StopLoss = tbOnlineCommand[i].StopLoss;
                                    newOpenTrade.TakeProfit = tbOnlineCommand[i].TakeProfit;                                    
                                    newOpenTrade.Commission = tbOnlineCommand[i].Commission;
                                    newOpenTrade.Swap = Math.Round(tbOnlineCommand[i].Swap, 2);
                                    newOpenTrade.Profit = tbOnlineCommand[i].Profit;
                                    newOpenTrade.CommandCode = tbOnlineCommand[i].CommandCode;
                                    newOpenTrade.Taxes = tbOnlineCommand[i].Taxes;
                                    newOpenTrade.Comment = tbOnlineCommand[i].Comment;
                                    newOpenTrade.AgentCommission = tbOnlineCommand[i].AgentCommission;
                                    newOpenTrade.TotalSwap = tbOnlineCommand[i].TotalSwaps;
                                    newOpenTrade.RefCommandID = tbOnlineCommand[i].RefCommandID;

                                    #region Find Investor In Investor List
                                    if (Business.Market.InvestorList != null)
                                    {
                                        int countInvestor = Business.Market.InvestorList.Count;
                                        for (int m = 0; m < countInvestor; m++)
                                        {
                                            if (Business.Market.InvestorList[m].InvestorID == tbOnlineCommand[i].InvestorID)
                                            {
                                                newOpenTrade.Investor = Business.Market.InvestorList[m];

                                                #region Find IGroupSecurity In IGroupSecurity List
                                                if (Business.Market.IGroupSecurityList != null)
                                                {
                                                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                                    for (int p = 0; p < countIGroupSecurity; p++)
                                                    {
                                                        if (Business.Market.IGroupSecurityList[p].SecurityID == newOpenTrade.Symbol.SecurityID &&
                                                            Business.Market.IGroupSecurityList[p].InvestorGroupID == newOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID)
                                                        {
                                                            newOpenTrade.IGroupSecurity = Business.Market.IGroupSecurityList[p];
                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                if (Business.Market.InvestorList[m].CommandList == null)
                                                    Business.Market.InvestorList[m].CommandList = new List<Business.OpenTrade>();

                                                Business.Market.InvestorList[m].CommandList.Add(newOpenTrade);
                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    //GET SPREAD DIFFRENCE OF OPEN TRADE
                                    double spreadDifference = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(newOpenTrade.Symbol.SecurityID, newOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID);
                                    newOpenTrade.SpreaDifferenceInOpenTrade = spreadDifference;

                                    if (Business.Market.SymbolList[k].CommandList == null)
                                        Business.Market.SymbolList[k].CommandList = new List<Business.OpenTrade>();

                                    Business.Market.SymbolList[k].CommandList.Add(newOpenTrade);

                                    #region ADD COMMAND TO COMMAND EXECUTORY
                                    if (Business.Market.CommandExecutor == null)
                                        Business.Market.CommandExecutor = new List<Business.OpenTrade>();

                                    Business.Market.CommandExecutor.Add(newOpenTrade);
                                                                        
                                    #endregion

                                    Flag = true;
                                    break;
                                }

                                //if (Flag == false)
                                //{
                                //    if (Business.Market.SymbolList[k].RefSymbol != null && Business.Market.SymbolList[k].RefSymbol.Count > 0)
                                //    {
                                //        newOpenTrade.Symbol = this.FindSymbolReference(Business.Market.SymbolList[k].RefSymbol, newOpenTrade, tbOnlineCommand[i].SymbolID);
                                //    }
                                //}
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
        /// <returns></returns>
        internal List<Business.OpenTrade> InitOnlineCommand()
        {
            List<Business.OpenTrade> result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetData();

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        //Business.OpenTrade newOpenTradeSymbol = new Business.OpenTrade();
                        //Business.OpenTrade newOpenTradeInvestor = new Business.OpenTrade();
                        //Business.OpenTrade newOpenTradeExe = new Business.OpenTrade();

                        Business.OpenTrade newOpenTradeSymbol = new Business.OpenTrade(tbOnlineCommand[i].IsActivePendig, tbOnlineCommand[i].IsStopLossTakeProfit);
                        Business.OpenTrade newOpenTradeInvestor = new Business.OpenTrade(tbOnlineCommand[i].IsActivePendig, tbOnlineCommand[i].IsStopLossTakeProfit);
                        Business.OpenTrade newOpenTradeExe = new Business.OpenTrade(tbOnlineCommand[i].IsActivePendig, tbOnlineCommand[i].IsStopLossTakeProfit);

                        #region FIND SYMBOL AND TYPE INSTANCE FOR COMMAND(COMMAND IN SYMBOL, COMMAND IN INVESTOR, COMMAND IN COMMAND EXECUTOR
                        if (Business.Market.SymbolList != null && Business.Market.SymbolList.Count > 0)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.SymbolList[j].SymbolID == tbOnlineCommand[i].SymbolID)
                                {
                                    newOpenTradeExe.Symbol = Business.Market.SymbolList[j];
                                    newOpenTradeInvestor.Symbol = Business.Market.SymbolList[j];
                                    newOpenTradeSymbol.Symbol = Business.Market.SymbolList[j];

                                    break;
                                }
                            }
                        }
                        #endregion

                        #region SEARCH MARKET AND GET TYPE COMMAND
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            bool flagMarketArea = false;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (flagMarketArea)
                                    break;

                                if (Business.Market.MarketArea[j].Type != null)
                                {
                                    int countType = Business.Market.MarketArea[j].Type.Count;
                                    for (int n = 0; n < countType; n++)
                                    {
                                        if (Business.Market.MarketArea[j].Type[n].ID == tbOnlineCommand[i].CommandTypeID)
                                        {
                                            newOpenTradeExe.Type = Business.Market.MarketArea[j].Type[n];
                                            newOpenTradeInvestor.Type = Business.Market.MarketArea[j].Type[n];
                                            newOpenTradeSymbol.Type = Business.Market.MarketArea[j].Type[n];

                                            flagMarketArea = true;

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region FIND INVESTOR AND IGROUPSECURITY INSTANCE OF COMMAND(COMMAND IN SYMBOL, COMMAND IN INVESTOR, COMMAND IN COMMAND EXECUTOR
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == tbOnlineCommand[i].InvestorID)
                                {
                                    newOpenTradeSymbol.Investor = Business.Market.InvestorList[j];
                                    newOpenTradeInvestor.Investor = Business.Market.InvestorList[j];
                                    newOpenTradeExe.Investor = Business.Market.InvestorList[j];

                                    #region GET IGROUP SECURITY
                                    if (Business.Market.IGroupSecurityList != null)
                                    {
                                        int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                        for (int n = 0; n < countIGroupSecurity; n++)
                                        {
                                            if (Business.Market.IGroupSecurityList[n].SecurityID == newOpenTradeSymbol.Symbol.SecurityID &&
                                                Business.Market.IGroupSecurityList[n].InvestorGroupID == newOpenTradeSymbol.Investor.InvestorGroupInstance.InvestorGroupID)
                                            {
                                                newOpenTradeSymbol.IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                newOpenTradeInvestor.IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                newOpenTradeExe.IGroupSecurity = Business.Market.IGroupSecurityList[n];

                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    break;
                                }
                            }
                        }
                        #endregion

                        #region GET SPREAD DIFFIRENCE FOR COMMAND
                        //GET SPREAD DIFFRENCE OF OPEN TRADE
                        double spreadDifference = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(newOpenTradeInvestor.Symbol.SecurityID,
                            newOpenTradeInvestor.Investor.InvestorGroupInstance.InvestorGroupID);

                        newOpenTradeExe.SpreaDifferenceInOpenTrade = spreadDifference;
                        newOpenTradeInvestor.SpreaDifferenceInOpenTrade = spreadDifference;
                        newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreadDifference;
                        #endregion

                        //===============================
                        #region SET CLOSE PRICES COMMAND
                        if (tbOnlineCommand[i].CommandTypeID == 1 || tbOnlineCommand[i].CommandTypeID == 11)
                        {
                            newOpenTradeExe.ClosePrice = newOpenTradeExe.Symbol.TickValue.Bid;
                            newOpenTradeInvestor.ClosePrice = newOpenTradeInvestor.Symbol.TickValue.Bid;
                            newOpenTradeSymbol.ClosePrice = newOpenTradeSymbol.Symbol.TickValue.Bid;
                        }

                        if (tbOnlineCommand[i].CommandTypeID == 2 || tbOnlineCommand[i].CommandTypeID == 12)
                        {
                            newOpenTradeExe.ClosePrice = (newOpenTradeExe.Symbol.TickValue.Ask +
                                Business.Symbol.ConvertNumberPip(newOpenTradeExe.Symbol.Digit, newOpenTradeExe.SpreaDifferenceInOpenTrade));

                            newOpenTradeInvestor.ClosePrice = (newOpenTradeInvestor.Symbol.TickValue.Ask +
                                Business.Symbol.ConvertNumberPip(newOpenTradeInvestor.Symbol.Digit, newOpenTradeInvestor.SpreaDifferenceInOpenTrade));

                            newOpenTradeSymbol.ClosePrice = (newOpenTradeSymbol.Symbol.TickValue.Ask +
                                Business.Symbol.ConvertNumberPip(newOpenTradeSymbol.Symbol.Digit, newOpenTradeSymbol.SpreaDifferenceInOpenTrade));
                        }
                        #endregion                                         

                        //=================================

                        #region NEW INSTANCES FOR COMMAND EXECUTOR
                        newOpenTradeExe.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        newOpenTradeExe.ClientCode = tbOnlineCommand[i].ClientCode;
                        newOpenTradeExe.CloseTime = tbOnlineCommand[i].CloseTime;
                        newOpenTradeExe.CommandCode = tbOnlineCommand[i].CommandCode;
                        newOpenTradeExe.Comment = tbOnlineCommand[i].Comment;
                        newOpenTradeExe.Commission = tbOnlineCommand[i].Commission;
                        newOpenTradeExe.ExpTime = tbOnlineCommand[i].ExpTime;
                        newOpenTradeExe.FreezeMargin = 0;
                        newOpenTradeExe.ID = tbOnlineCommand[i].OnlineCommandID;
                        newOpenTradeExe.IsClose = tbOnlineCommand[i].IsClose;
                        newOpenTradeExe.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOpenTradeExe.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOpenTradeExe.Profit = tbOnlineCommand[i].Profit;
                        newOpenTradeExe.Size = tbOnlineCommand[i].Size;
                        newOpenTradeExe.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOpenTradeExe.Swap = tbOnlineCommand[i].Swap;
                        newOpenTradeExe.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        newOpenTradeExe.Taxes = tbOnlineCommand[i].Taxes;
                        newOpenTradeExe.TotalSwap = tbOnlineCommand[i].TotalSwaps;
                        newOpenTradeExe.RefCommandID = tbOnlineCommand[i].RefCommandID;
                        newOpenTradeExe.AgentRefConfig = tbOnlineCommand[i].AgentRefConfig;
                        #endregion

                        #region NEW INSTANCE FOR SYMBOL LIST
                        newOpenTradeSymbol.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        newOpenTradeSymbol.ClientCode = tbOnlineCommand[i].ClientCode;
                        newOpenTradeSymbol.CloseTime = tbOnlineCommand[i].CloseTime;
                        newOpenTradeSymbol.CommandCode = tbOnlineCommand[i].CommandCode;
                        newOpenTradeSymbol.Comment = tbOnlineCommand[i].Comment;
                        newOpenTradeSymbol.Commission = tbOnlineCommand[i].Commission;
                        newOpenTradeSymbol.ExpTime = tbOnlineCommand[i].ExpTime;
                        newOpenTradeSymbol.FreezeMargin = 0;
                        newOpenTradeSymbol.ID = tbOnlineCommand[i].OnlineCommandID;
                        newOpenTradeSymbol.IsClose = tbOnlineCommand[i].IsClose;                                                
                        newOpenTradeSymbol.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOpenTradeSymbol.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOpenTradeSymbol.Profit = tbOnlineCommand[i].Profit;
                        newOpenTradeSymbol.Size = tbOnlineCommand[i].Size;
                        newOpenTradeSymbol.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOpenTradeSymbol.Swap = tbOnlineCommand[i].Swap;
                        newOpenTradeSymbol.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        newOpenTradeSymbol.Taxes = tbOnlineCommand[i].Taxes;
                        newOpenTradeSymbol.TotalSwap = tbOnlineCommand[i].TotalSwaps;
                        newOpenTradeSymbol.InsExe = newOpenTradeExe;
                        newOpenTradeSymbol.RefCommandID = tbOnlineCommand[i].RefCommandID;
                        newOpenTradeSymbol.AgentRefConfig = tbOnlineCommand[i].AgentRefConfig;
                        #endregion

                        #region NEW INSTANCE FOR INVESTOR LIST
                        newOpenTradeInvestor.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        newOpenTradeInvestor.ClientCode = tbOnlineCommand[i].ClientCode;                        
                        newOpenTradeInvestor.CloseTime = tbOnlineCommand[i].CloseTime;
                        newOpenTradeInvestor.CommandCode = tbOnlineCommand[i].CommandCode;
                        newOpenTradeInvestor.Comment = tbOnlineCommand[i].Comment;
                        newOpenTradeInvestor.Commission = tbOnlineCommand[i].Commission;
                        newOpenTradeInvestor.ExpTime = tbOnlineCommand[i].ExpTime;
                        newOpenTradeInvestor.FreezeMargin = 0;
                        newOpenTradeInvestor.ID = tbOnlineCommand[i].OnlineCommandID;
                        newOpenTradeInvestor.IsClose = tbOnlineCommand[i].IsClose;
                        newOpenTradeInvestor.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOpenTradeInvestor.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOpenTradeInvestor.Profit = tbOnlineCommand[i].Profit;
                        newOpenTradeInvestor.Size = tbOnlineCommand[i].Size;
                        newOpenTradeInvestor.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOpenTradeInvestor.Swap = tbOnlineCommand[i].Swap;
                        newOpenTradeInvestor.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        newOpenTradeInvestor.Taxes = tbOnlineCommand[i].Taxes;
                        newOpenTradeInvestor.TotalSwap = tbOnlineCommand[i].TotalSwaps;
                        newOpenTradeInvestor.InsExe = newOpenTradeExe;
                        newOpenTradeInvestor.RefCommandID = tbOnlineCommand[i].RefCommandID;
                        newOpenTradeInvestor.AgentRefConfig = tbOnlineCommand[i].AgentRefConfig;
                        #endregion

                        //======================================
                        #region ADD COMMAND TO COMMAND EXECUTOR
                        if (Business.Market.CommandExecutor == null)
                            Business.Market.CommandExecutor = new List<Business.OpenTrade>();

                        Business.Market.CommandExecutor.Add(newOpenTradeExe);
                        #endregion

                        #region ADD COMMAND TO INVESTOR LIST
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == tbOnlineCommand[i].InvestorID)
                                {
                                    if (Business.Market.InvestorList[j].CommandList == null)
                                        Business.Market.InvestorList[j].CommandList = new List<Business.OpenTrade>();

                                    Business.Market.InvestorList[j].CommandList.Add(newOpenTradeInvestor);

                                    break;
                                }
                            }
                        }
                        #endregion

                        #region ADD COMMAND TO SYMBOL LIST
                        if (Business.Market.SymbolList != null)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.SymbolList[j].SymbolID == tbOnlineCommand[i].SymbolID)
                                {
                                    if (Business.Market.SymbolList[j].CommandList == null)
                                        Business.Market.SymbolList[j].CommandList = new List<Business.OpenTrade>();

                                    Business.Market.SymbolList[j].CommandList.Add(newOpenTradeSymbol);

                                    break;
                                }
                            }
                        }
                        #endregion                        
                        
                        result.Add(newOpenTradeInvestor);
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
        /// <param name="ListSymbol"></param>
        /// <param name="SymbolID"></param>
        internal Business.Symbol FindSymbolReference(List<Business.Symbol> ListSymbol, Business.OpenTrade objOpenTrade,int SymbolID)
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

                        if (ListSymbol[i].CommandList == null)
                            ListSymbol[i].CommandList = new List<Business.OpenTrade>();

                        ListSymbol[i].CommandList.Add(objOpenTrade);
                        Flag = true;
                        break;
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            this.FindSymbolReference(ListSymbol[i].RefSymbol, objOpenTrade,SymbolID);
                        }
                    }                    
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandBySymbolID(int SymbolID)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetOnlineCommandBySymbolID(SymbolID);

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ID = tbOnlineCommand[i].OnlineCommandID;
                        
                        //Find Trade Type
                        if (Business.Market.MarketArea != null)
                        {
                            bool Flag = false;
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (Flag == true)
                                    break;

                                if (Business.Market.MarketArea[j].Type != null)
                                {
                                    int countTradeType = Business.Market.MarketArea[j].Type.Count;
                                    for (int n = 0; n < countTradeType; n++)
                                    {
                                        if (Business.Market.MarketArea[j].Type[n].ID == tbOnlineCommand[i].CommandTypeID)
                                        {
                                            newOpenTrade.Type = Business.Market.MarketArea[j].Type[n];
                                            Flag = true;
                                            break;
                                        }
                                    }
                                }                               
                            }
                        }
                        
                        newOpenTrade.Investor = new Business.Investor();
                        newOpenTrade.Investor.InvestorID = tbOnlineCommand[i].InvestorID;
                        newOpenTrade.Symbol = new Business.Symbol();
                        newOpenTrade.Symbol.SymbolID = tbOnlineCommand[i].SymbolID;
                        newOpenTrade.ClientCode = tbOnlineCommand[i].ClientCode;
                        newOpenTrade.ClosePrice = tbOnlineCommand[i].ClosePrice;
                        newOpenTrade.CloseTime = tbOnlineCommand[i].CloseTime;
                        newOpenTrade.ExpTime = tbOnlineCommand[i].ExpTime;
                        newOpenTrade.IsClose = tbOnlineCommand[i].IsClose;
                        newOpenTrade.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOpenTrade.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOpenTrade.Size = tbOnlineCommand[i].Size;
                        newOpenTrade.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOpenTrade.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        //newOpenTrade.NumberUpdate = tbOnlineCommand[i].NumberUpdate;
                        newOpenTrade.Commission = tbOnlineCommand[i].Commission;
                        newOpenTrade.Swap = tbOnlineCommand[i].Swap;
                        newOpenTrade.Profit = tbOnlineCommand[i].Profit;
                        newOpenTrade.CommandCode = tbOnlineCommand[i].CommandCode;
                        newOpenTrade.Commission = tbOnlineCommand[i].Commission;
                        newOpenTrade.Taxes = tbOnlineCommand[i].Taxes;
                        newOpenTrade.Comment = tbOnlineCommand[i].Comment;
                        newOpenTrade.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        newOpenTrade.RefCommandID = tbOnlineCommand[i].RefCommandID;

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
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByInvestorID(int InvestorID)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetOnlineCommandByInvestorID(InvestorID);

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ID = tbOnlineCommand[i].OnlineCommandID;
                        //CommandType
                        newOpenTrade.Investor = new Business.Investor();
                        newOpenTrade.Investor.InvestorID = tbOnlineCommand[i].InvestorID;
                        newOpenTrade.Symbol = new Business.Symbol();
                        newOpenTrade.Symbol.SymbolID = tbOnlineCommand[i].SymbolID;
                        newOpenTrade.ClientCode = tbOnlineCommand[i].ClientCode;
                        newOpenTrade.ClosePrice = tbOnlineCommand[i].ClosePrice;
                        newOpenTrade.CloseTime = tbOnlineCommand[i].CloseTime;
                        newOpenTrade.ExpTime = tbOnlineCommand[i].ExpTime;
                        newOpenTrade.IsClose = tbOnlineCommand[i].IsClose;
                        newOpenTrade.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOpenTrade.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOpenTrade.Size = tbOnlineCommand[i].Size;
                        newOpenTrade.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOpenTrade.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        //newOpenTrade.NumberUpdate = tbOnlineCommand[i].NumberUpdate;
                        newOpenTrade.Commission = tbOnlineCommand[i].Commission;
                        newOpenTrade.Swap = tbOnlineCommand[i].Swap;
                        newOpenTrade.Profit = tbOnlineCommand[i].Profit;
                        newOpenTrade.CommandCode = tbOnlineCommand[i].CommandCode;
                        newOpenTrade.Commission = tbOnlineCommand[i].Commission;
                        newOpenTrade.Taxes = tbOnlineCommand[i].Taxes;
                        newOpenTrade.Comment = tbOnlineCommand[i].Comment;
                        newOpenTrade.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        newOpenTrade.RefCommandID = tbOnlineCommand[i].RefCommandID;

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
        /// <param name="CommandTypeID"></param>
        /// <returns></returns>
        internal List<Business.OpenTrade> GetOnlineCommandByCommandTypeID(int CommandTypeID)
        {
            List<Business.OpenTrade> Result = new List<Business.OpenTrade>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetOnlineCommandByCommandType(CommandTypeID);

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                        newOpenTrade.ID = tbOnlineCommand[i].OnlineCommandID;
                        //CommandType
                        newOpenTrade.Investor = new Business.Investor();
                        newOpenTrade.Investor.InvestorID = tbOnlineCommand[i].InvestorID;
                        newOpenTrade.Symbol = new Business.Symbol();
                        newOpenTrade.Symbol.SymbolID = tbOnlineCommand[i].SymbolID;
                        newOpenTrade.ClientCode = tbOnlineCommand[i].ClientCode;
                        newOpenTrade.ClosePrice = tbOnlineCommand[i].ClosePrice;
                        newOpenTrade.CloseTime = tbOnlineCommand[i].CloseTime;
                        newOpenTrade.ExpTime = tbOnlineCommand[i].ExpTime;
                        newOpenTrade.IsClose = tbOnlineCommand[i].IsClose;
                        newOpenTrade.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        newOpenTrade.OpenTime = tbOnlineCommand[i].OpenTime;
                        newOpenTrade.Size = tbOnlineCommand[i].Size;
                        newOpenTrade.StopLoss = tbOnlineCommand[i].StopLoss;
                        newOpenTrade.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        //newOpenTrade.NumberUpdate = tbOnlineCommand[i].NumberUpdate;
                        newOpenTrade.Commission = tbOnlineCommand[i].Commission;
                        newOpenTrade.Swap = tbOnlineCommand[i].Swap;
                        newOpenTrade.Profit = tbOnlineCommand[i].Profit;
                        newOpenTrade.CommandCode = tbOnlineCommand[i].CommandCode;
                        newOpenTrade.ClientCode = tbOnlineCommand[i].ClientCode;
                        newOpenTrade.Taxes = tbOnlineCommand[i].Taxes;
                        newOpenTrade.Comment = tbOnlineCommand[i].Comment;
                        newOpenTrade.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        newOpenTrade.RefCommandID = tbOnlineCommand[i].RefCommandID;

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
        /// <param name="OnlineCommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetOnlineCommandByID(int OnlineCommandID)
        {
            Business.OpenTrade Result = new Business.OpenTrade();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetOnlineCommandByID(OnlineCommandID);

                if (tbOnlineCommand != null)
                {
                    int count = tbOnlineCommand.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Result.ID = tbOnlineCommand[i].OnlineCommandID;
                        //CommandType
                        Result.Investor = new Business.Investor();
                        Result.Investor.InvestorID = tbOnlineCommand[i].InvestorID;
                        Result.Symbol = new Business.Symbol();
                        Result.Symbol.SymbolID = tbOnlineCommand[i].SymbolID;
                        Result.ClientCode = tbOnlineCommand[i].ClientCode;
                        Result.ClosePrice = tbOnlineCommand[i].ClosePrice;
                        Result.CloseTime = tbOnlineCommand[i].CloseTime;
                        Result.ExpTime = tbOnlineCommand[i].ExpTime;
                        Result.IsClose = tbOnlineCommand[i].IsClose;
                        Result.OpenPrice = tbOnlineCommand[i].OpenPrice;
                        Result.OpenTime = tbOnlineCommand[i].OpenTime;
                        Result.Size = tbOnlineCommand[i].Size;
                        Result.StopLoss = tbOnlineCommand[i].StopLoss;
                        Result.TakeProfit = tbOnlineCommand[i].TakeProfit;
                        //Result.NumberUpdate = tbOnlineCommand[i].NumberUpdate;
                        Result.Commission = tbOnlineCommand[i].Commission;
                        Result.Swap = tbOnlineCommand[i].Swap;
                        Result.Profit = tbOnlineCommand[i].Profit;
                        Result.CommandCode = tbOnlineCommand[i].CommandCode;
                        Result.Taxes = tbOnlineCommand[i].Taxes;
                        Result.Comment = tbOnlineCommand[i].Comment;
                        Result.AgentCommission = tbOnlineCommand[i].AgentCommission;
                        Result.RefCommandID = tbOnlineCommand[i].RefCommandID;
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
        /// <param name="OnlineCommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade GetOnlineCommandByCommandID(int OnlineCommandID)
        {
            Business.OpenTrade Result = new Business.OpenTrade();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            DS.OnlineCommandDataTable tbOnlineCommand = new DS.OnlineCommandDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbOnlineCommand = adap.GetOnlineCommandByID(OnlineCommandID);

                if (tbOnlineCommand != null)
                {
                    Business.OpenTrade newOpenTradeSymbol = new Business.OpenTrade();                    

                    #region FIND SYMBOL AND TYPE INSTANCE FOR COMMAND(COMMAND IN SYMBOL, COMMAND IN INVESTOR, COMMAND IN COMMAND EXECUTOR
                    if (Business.Market.SymbolList != null && Business.Market.SymbolList.Count > 0)
                    {
                        int countSymbol = Business.Market.SymbolList.Count;
                        for (int j = 0; j < countSymbol; j++)
                        {
                            if (Business.Market.SymbolList[j].SymbolID == tbOnlineCommand[0].SymbolID)
                            {                                
                                newOpenTradeSymbol.Symbol = Business.Market.SymbolList[j];

                                break;
                            }
                        }
                    }
                    #endregion

                    #region SEARCH MARKET AND GET TYPE COMMAND
                    if (Business.Market.MarketArea != null)
                    {
                        int countMarketArea = Business.Market.MarketArea.Count;
                        bool flagMarketArea = false;
                        for (int j = 0; j < countMarketArea; j++)
                        {
                            if (flagMarketArea)
                                break;

                            if (Business.Market.MarketArea[j].Type != null)
                            {
                                int countType = Business.Market.MarketArea[j].Type.Count;
                                for (int n = 0; n < countType; n++)
                                {
                                    if (Business.Market.MarketArea[j].Type[n].ID == tbOnlineCommand[0].CommandTypeID)
                                    {                                        
                                        newOpenTradeSymbol.Type = Business.Market.MarketArea[j].Type[n];

                                        flagMarketArea = true;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region FIND INVESTOR AND IGROUPSECURITY INSTANCE OF COMMAND(COMMAND IN SYMBOL, COMMAND IN INVESTOR, COMMAND IN COMMAND EXECUTOR
                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].InvestorID == tbOnlineCommand[0].InvestorID)
                            {
                                newOpenTradeSymbol.Investor = Business.Market.InvestorList[j];

                                #region GET IGROUP SECURITY
                                if (Business.Market.IGroupSecurityList != null)
                                {
                                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                    for (int n = 0; n < countIGroupSecurity; n++)
                                    {
                                        if (Business.Market.IGroupSecurityList[n].SecurityID == newOpenTradeSymbol.Symbol.SecurityID &&
                                            Business.Market.IGroupSecurityList[n].InvestorGroupID == newOpenTradeSymbol.Investor.InvestorGroupInstance.InvestorGroupID)
                                        {
                                            newOpenTradeSymbol.IGroupSecurity = Business.Market.IGroupSecurityList[n];

                                            break;
                                        }
                                    }
                                }
                                #endregion

                                break;
                            }
                        }
                    }
                    #endregion

                    #region GET SPREAD DIFFIRENCE FOR COMMAND
                    //GET SPREAD DIFFRENCE OF OPEN TRADE
                    double spreadDifference = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(newOpenTradeSymbol.Symbol.SecurityID,
                        newOpenTradeSymbol.Investor.InvestorGroupInstance.InvestorGroupID);

                    newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreadDifference;
                    #endregion

                    //===============================
                    #region SET CLOSE PRICES COMMAND
                    if (tbOnlineCommand[0].CommandTypeID == 1 || tbOnlineCommand[0].CommandTypeID == 11)
                    {                        
                        newOpenTradeSymbol.ClosePrice = newOpenTradeSymbol.Symbol.TickValue.Bid;
                    }

                    if (tbOnlineCommand[0].CommandTypeID == 2 || tbOnlineCommand[0].CommandTypeID == 12)
                    {                        
                        newOpenTradeSymbol.ClosePrice = (newOpenTradeSymbol.Symbol.TickValue.Ask +
                            Business.Symbol.ConvertNumberPip(newOpenTradeSymbol.Symbol.Digit, newOpenTradeSymbol.SpreaDifferenceInOpenTrade));
                    }
                    #endregion

                    //=================================

                    #region NEW INSTANCE FOR SYMBOL LIST
                    newOpenTradeSymbol.AgentCommission = tbOnlineCommand[0].AgentCommission;
                    newOpenTradeSymbol.ClientCode = tbOnlineCommand[0].ClientCode;
                    newOpenTradeSymbol.CloseTime = tbOnlineCommand[0].CloseTime;
                    newOpenTradeSymbol.CommandCode = tbOnlineCommand[0].CommandCode;
                    newOpenTradeSymbol.Comment = tbOnlineCommand[0].Comment;
                    newOpenTradeSymbol.Commission = tbOnlineCommand[0].Commission;
                    newOpenTradeSymbol.ExpTime = tbOnlineCommand[0].ExpTime;
                    newOpenTradeSymbol.FreezeMargin = 0;
                    newOpenTradeSymbol.ID = tbOnlineCommand[0].OnlineCommandID;
                    newOpenTradeSymbol.IsClose = tbOnlineCommand[0].IsClose;
                    newOpenTradeSymbol.OpenPrice = tbOnlineCommand[0].OpenPrice;
                    newOpenTradeSymbol.OpenTime = tbOnlineCommand[0].OpenTime;
                    newOpenTradeSymbol.Profit = tbOnlineCommand[0].Profit;
                    newOpenTradeSymbol.Size = tbOnlineCommand[0].Size;
                    newOpenTradeSymbol.StopLoss = tbOnlineCommand[0].StopLoss;
                    newOpenTradeSymbol.Swap = tbOnlineCommand[0].Swap;
                    newOpenTradeSymbol.TakeProfit = tbOnlineCommand[0].TakeProfit;
                    newOpenTradeSymbol.Taxes = tbOnlineCommand[0].Taxes;
                    newOpenTradeSymbol.TotalSwap = tbOnlineCommand[0].TotalSwaps;
                    newOpenTradeSymbol.RefCommandID = tbOnlineCommand[0].RefCommandID;
                    #endregion

                    Result = newOpenTradeSymbol;
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
        /// <param name="objOpenTrade"></param>
        /// <returns></returns>
        internal int AddNewOnlineCommand(Business.OpenTrade objOpenTrade)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                if (objOpenTrade.TotalSwap == -1)
                    objOpenTrade.TotalSwap = 0;

                Result = int.Parse(adap.AddNewOnlineCommand(objOpenTrade.Type.ID, objOpenTrade.Investor.InvestorID, objOpenTrade.ClientCode, objOpenTrade.ClosePrice,
                                        objOpenTrade.CloseTime, objOpenTrade.ExpTime, objOpenTrade.IsClose, objOpenTrade.OpenPrice, objOpenTrade.OpenTime,
                                        objOpenTrade.Size, objOpenTrade.StopLoss, objOpenTrade.TakeProfit, 1, objOpenTrade.Commission,
                                        objOpenTrade.Swap, objOpenTrade.Profit, objOpenTrade.CommandCode, objOpenTrade.Symbol.SymbolID, objOpenTrade.Taxes,
                                        objOpenTrade.Comment, objOpenTrade.AgentCommission, objOpenTrade.TotalSwap,
                                        objOpenTrade.RefCommandID, objOpenTrade.AgentRefConfig, objOpenTrade.IsActivePending, objOpenTrade.IsStopLossAndTakeProfit).ToString());
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
        /// <param name="objOpenTrade"></param>
        internal bool UpdateOnlineCommand(Business.OpenTrade objOpenTrade)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();
            SqlTransaction tran;
            conn.Open();
            adap.Connection = conn;
            tran = conn.BeginTransaction();
            adap.Transaction = tran;

            try
            {
                int NumberUpdate = adap.UpdateOnlineCommand(objOpenTrade.Type.ID, objOpenTrade.Investor.InvestorID, objOpenTrade.ClosePrice,
                    objOpenTrade.CloseTime, objOpenTrade.ExpTime, objOpenTrade.OpenPrice, objOpenTrade.OpenTime, objOpenTrade.Size,
                    objOpenTrade.StopLoss, objOpenTrade.TakeProfit, objOpenTrade.Commission, objOpenTrade.Swap, objOpenTrade.Profit,
                    objOpenTrade.Symbol.SymbolID, objOpenTrade.Taxes, objOpenTrade.Comment, objOpenTrade.AgentCommission, 
                    objOpenTrade.TotalSwap, objOpenTrade.RefCommandID, objOpenTrade.AgentRefConfig, objOpenTrade.IsStopLossAndTakeProfit, 
                    objOpenTrade.IsActivePending, objOpenTrade.ID);

                if (NumberUpdate > 0)
                {
                    Result = true;
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
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
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isStopLoss"></param>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal bool UpdateIsActivePending(bool isActive, bool isStopLoss, int commandID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                TradingServer.Facade.FacadeAddNewSystemLog(1, "Isactive: " + isActive + " isStopLoss: " + isStopLoss, "[ActivePending]", "", "");
                int ResultUpdate = adap.UpdateIsActivePending(isActive, isStopLoss, commandID);
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
        /// <param name="InvestorID"></param>
        /// <param name="Swap"></param>
        /// <returns></returns>
        internal bool UpdateSwapOnlineCommand(int OnlineCommandID, double Swap)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int ResultUpdate = adap.UpdateSwapOnlineCommand(Swap, OnlineCommandID);
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
        /// <param name="OnlineCommandID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommand(int OnlineCommandID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumberDelete = adap.DeleteOnlineCommandByID(OnlineCommandID);
                if (NumberDelete > 0)
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
        /// <param name="listOpenTrade"></param>
        /// <returns></returns>
        internal bool MultipleCloseOpenTrade(List<TradingServer.Business.OpenTrade> listOpenTrade)
        {
            bool result = false;

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adapOnline = new DSTableAdapters.OnlineCommandTableAdapter();
            DSTableAdapters.CommandHistoryTableAdapter adapHistory = new DSTableAdapters.CommandHistoryTableAdapter();
            SqlTransaction tran;
            conn.Open();

            adapOnline.Connection = conn;
            adapHistory.Connection = conn;
            tran = conn.BeginTransaction();
            adapOnline.Transaction = tran;
            adapHistory.Transaction = tran;

            try
            {
                if (listOpenTrade != null)
                {
                    int count = listOpenTrade.Count;
                    for (int i = 0; i < count; i++)
                    {  
                        int ResultHistory = -1;
                        //Add Command To Command History
                        ResultHistory = int.Parse(adapHistory.AddNewCommandHistory(listOpenTrade[i].Investor.InvestorID, listOpenTrade[i].Type.ID, listOpenTrade[i].OpenTime,
                            listOpenTrade[i].OpenPrice, listOpenTrade[i].CloseTime, listOpenTrade[i].ClosePrice, listOpenTrade[i].Profit, listOpenTrade[i].Swap,
                            listOpenTrade[i].Commission, listOpenTrade[i].ExpTime, listOpenTrade[i].Size, listOpenTrade[i].StopLoss, listOpenTrade[i].TakeProfit,
                            listOpenTrade[i].ClientCode, listOpenTrade[i].CommandCode, listOpenTrade[i].Symbol.SymbolID, listOpenTrade[i].Taxes,
                            listOpenTrade[i].AgentCommission, listOpenTrade[i].Comment, listOpenTrade[i].TotalSwap, false, listOpenTrade[i].RefCommandID, 
                            listOpenTrade[i].AgentRefConfig, false, false).ToString());
                        
                        if (ResultHistory > 0)
                        {
                            //Update Command In Database                             
                            int deleteOpenTrade = adapOnline.DeleteOnlineCommandByID(listOpenTrade[i].ID);

                            if (deleteOpenTrade < 1)
                            {
                                tran.Rollback();

                                return false;
                            }
                        }
                        else
                        {
                            tran.Rollback();

                            return false;
                        }

                        bool removeDataOnline = listOpenTrade[0].Investor.MultipleCloseCommand(listOpenTrade[i]);
                        if (!removeDataOnline)
                        {
                            tran.Rollback();

                            return false;
                        }                        
                    }
                }

                if (listOpenTrade != null)
                {
                    int countCommand = listOpenTrade.Count;
                    for (int i = 0; i < countCommand; i++)
                    {
                        //Send Notify to Manager
                        TradingServer.Facade.FacadeSendNoticeManagerRequest(2, listOpenTrade[i]);
                    }
                }

                tran.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {   
                tran.Dispose();
                adapOnline.Connection.Close();
                adapHistory.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOpenTrade"></param>
        /// <returns></returns>
        internal bool MultipleUpdateOpenTrade(List<TradingServer.Business.OpenTrade> listOpenTrade,double stopLoss,double takeProfit)
        {
            List<string> listMessage = new List<string>();
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adapOnline = new DSTableAdapters.OnlineCommandTableAdapter();
            SqlTransaction tran;
            conn.Open();
            adapOnline.Connection = conn;
            tran = conn.BeginTransaction();
            adapOnline.Transaction = tran;

            try
            {
                if (listOpenTrade != null)
                {
                    int count = listOpenTrade.Count;
                    for (int i = 0; i < count; i++)
                    {
                        bool isBuy = false;
                        string commandType = "SellSpotCommand";
                        if (listOpenTrade[i].Type.ID == 1 || listOpenTrade[i].Type.ID == 11)
                        {
                            isBuy = true;
                            commandType = "BuySpotCommand";
                        }

                        double tempStopLoss = 0;
                        double tempTakeProfit = 0;
                        if (listOpenTrade[i].Type.ID == 2 || listOpenTrade[i].Type.ID == 12)
                        {
                            tempStopLoss = takeProfit;
                            tempTakeProfit = stopLoss;
                        }
                        else
                        {
                            tempStopLoss = stopLoss;
                            tempTakeProfit = takeProfit;
                        }

                        int resultUpdate = adapOnline.UpdateTakeProfit(tempStopLoss, tempTakeProfit, "[update s/l and t/p]", listOpenTrade[i].OpenPrice, listOpenTrade[i].ID);

                        if (resultUpdate > 0)
                        {
                            bool updateOnline = listOpenTrade[i].Investor.MultipleUpdateCommand(listOpenTrade[i], tempStopLoss, tempTakeProfit);

                            if (!updateOnline)
                            {
                                #region Map Command Server To Client
                                string Message = "UpdateCommand$False,Update Command UnComplete," + listOpenTrade[i].ID + "," +
                                    listOpenTrade[i].Investor.InvestorID + "," + listOpenTrade[i].Symbol.Name + "," + listOpenTrade[i].Size + "," +
                                    isBuy + "," + listOpenTrade[i].OpenTime + "," + listOpenTrade[i].OpenPrice + "," + tempStopLoss + "," +
                                    tempTakeProfit + "," + listOpenTrade[i].ClosePrice + "," + listOpenTrade[i].Commission + "," +
                                    listOpenTrade[i].Swap + "," + listOpenTrade[i].Profit + "," + "Comment," + listOpenTrade[i].ID + "," + commandType + "," + 1 + "," +
                                    listOpenTrade[i].ExpTime + "," + listOpenTrade[i].ClientCode + "," + listOpenTrade[i].CommandCode + "," +
                                    listOpenTrade[i].IsHedged + "," + listOpenTrade[i].Type.ID + "," + listOpenTrade[i].Margin + ",UpdatePendingOrder";

                                listMessage = new List<string>();
                                listMessage.Add(Message);
                                #endregion

                                tran.Rollback();
                                return false;
                            }
                            else
                            {
                                listOpenTrade[i].StopLoss = tempStopLoss;
                                listOpenTrade[i].TakeProfit = tempTakeProfit;

                                #region Map Command Server To Client
                                string Message = "UpdateCommand$True,Update Command Complete," + listOpenTrade[i].ID + "," +
                                    listOpenTrade[i].Investor.InvestorID + "," + listOpenTrade[i].Symbol.Name + "," + listOpenTrade[i].Size + "," +
                                    isBuy + "," + listOpenTrade[i].OpenTime + "," + listOpenTrade[i].OpenPrice + "," + tempStopLoss + "," +
                                    tempTakeProfit + "," + listOpenTrade[i].ClosePrice + "," + listOpenTrade[i].Commission + "," +
                                    listOpenTrade[i].Swap + "," + listOpenTrade[i].Profit + "," + "Comment," + listOpenTrade[i].ID + "," + commandType + "," + 1 + "," +
                                    listOpenTrade[i].ExpTime + "," + listOpenTrade[i].ClientCode + "," + listOpenTrade[i].CommandCode + "," +
                                    listOpenTrade[i].IsHedged + "," + listOpenTrade[i].Type.ID + "," + listOpenTrade[i].Margin + ",UpdatePendingOrder";

                                listMessage.Add(Message);
                                #endregion
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            return false;
                        }
                    }
                }

                if (listMessage != null)
                {
                    int count = listMessage.Count;
                    for (int i = 0; i < count; i++)
                    {
                        listOpenTrade[0].Investor.ClientCommandQueue.Add(listMessage[i]);
                    }
                }

                if (listOpenTrade != null)
                {
                    int countCommand = listOpenTrade.Count;
                    for (int i = 0; i < countCommand; i++)
                    {
                        //SEND NOTIFY TO MANAGER
                        TradingServer.Facade.FacadeSendNoticeManagerRequest(1, listOpenTrade[i]);
                    }
                }

                tran.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                tran.Dispose();
                adapOnline.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandTypeID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommandByCommandTypeID(int CommandTypeID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumberDelete = adap.DeleteOnlineCommandByCommandType(CommandTypeID);
                if (NumberDelete > 0)
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
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommandByInvestorID(int InvestorID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumberDelete = adap.DeleteOnlineCommandByInvestorID(InvestorID);
                if (NumberDelete > 0)
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
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal bool DeleteOnlineCommandBySymbolID(int SymbolID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumberDelete = adap.DeleteOnlineCommandBySymbolID(SymbolID);
                if (NumberDelete > 0)
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
        /// <param name="TakeProfit"></param>
        /// <param name="StopLoss"></param>
        /// <param name="CommandOnlineID"></param>
        /// <returns></returns>
        internal bool UpdateTakeProfit(double TakeProfit, double StopLoss, int OnlineCommandID,string comment,double openPrice)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumberUpdate = adap.UpdateTakeProfit(StopLoss, TakeProfit, comment, openPrice, OnlineCommandID);
                if (NumberUpdate > 0)
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
        /// <param name="OpenTradeID"></param>
        /// <param name="CommandCode"></param>
        /// <returns></returns>
        internal bool UpdateCommandCode(int OpenTradeID, string CommandCode)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int NumberUpdate = adap.UpdateCommandCode(CommandCode, OpenTradeID);

                if (NumberUpdate > 0)
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
        /// <param name="openPositionID"></param>
        /// <param name="totalSwap"></param>
        /// <returns></returns>
        internal bool UpdateTotalSwap(int openPositionID, double totalSwap)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateTotalSwap(totalSwap, openPositionID);
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
        /// <returns></returns>
        internal int CountOnlineCommand()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.OnlineCommandTableAdapter adap = new DSTableAdapters.OnlineCommandTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountOnlineCommand();
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
