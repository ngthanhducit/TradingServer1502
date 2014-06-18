using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Symbol
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leverage"></param>
        /// <param name="percentage"></param>
        /// <param name="contract_size"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginForex(double leverage, double percentage, double contract_size, double initial_margin, double lots)
        {
            if (leverage == 0 | percentage == 0)
            {
                return 0;
            }
            if (initial_margin != 0)
            {
                contract_size = initial_margin;
            }
            return (lots * contract_size / leverage * percentage / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leverage"></param>
        /// <param name="percentage"></param>
        /// <param name="freezeMargin"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginFreezeForex(double leverage, double percentage, double contract_size, double initial_margin, double lots, double freezeMargin)
        {
            if (leverage == 0 | percentage == 0)
            {
                return 0;
            }
            if (initial_margin != 0)
            {
                contract_size = initial_margin;
            }
            return ((lots * contract_size / leverage * percentage / 100) * freezeMargin) / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="markert_price"></param>
        /// <param name="percentage"></param>
        /// <param name="contract_size"></param>
        /// <param name="lots"></param>
        /// <param name="symbolBid"></param>
        /// <returns></returns>
        internal double MarginCFD(double markert_price, double percentage, double contract_size, double lots)
        {
            return (lots * contract_size * markert_price * percentage / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="markert_price"></param>
        /// <param name="percentage"></param>
        /// <param name="freezeMargin"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginFreezeCFD(double markert_price, double percentage, double contract_size, double lots, double freezeMargin)
        {
            return ((lots * contract_size * markert_price * percentage / 100) * freezeMargin) / 100;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initial_margin"></param>
        /// <param name="percentage"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginFutures(double initial_margin, double percentage, double lots)
        {
            return (lots * initial_margin * percentage / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freezeMargin"></param>
        /// <param name="percentage"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginFreezeFutures(double initial_margin, double percentage, double lots, double freezeMargin)
        {
            return ((lots * initial_margin * percentage / 100) * freezeMargin) / 100;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="markert_price"></param>
        /// <param name="tick_size"></param>
        /// <param name="price"></param>
        /// <param name="percentage"></param>
        /// <param name="contract_size"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginCFD_Index(double markert_price, double tick_size, double price, double percentage, double contract_size, double lots)
        {
            if (tick_size == 0 | percentage == 0 | price == 0)
            {
                return 0;
            }
            return (lots * contract_size * markert_price / tick_size * price * percentage / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="markert_price"></param>
        /// <param name="tick_size"></param>
        /// <param name="price"></param>
        /// <param name="percentage"></param>
        /// <param name="freezeMargin"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginCFDFreeze_Index(double markert_price, double tick_size, double price, double percentage, double contract_size, double lots, double freezeMargin)
        {
            if (tick_size == 0 | percentage == 0 | price == 0)
            {
                return 0;
            }
            return ((lots * contract_size * markert_price / tick_size * price * percentage / 100) * freezeMargin) / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="markert_price"></param>
        /// <param name="leverage"></param>
        /// <param name="percentage"></param>
        /// <param name="contract_size"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginCFD_Leverage(double markert_price, double leverage, double percentage, double contract_size, double lots)
        {
            if (leverage == 0 | percentage == 0)
            {
                return 0;
            }
            return (lots * contract_size * markert_price / leverage * percentage / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="markert_price"></param>
        /// <param name="leverage"></param>
        /// <param name="percentage"></param>
        /// <param name="freezeMargin"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double MarginFreezeCFD_Leverage(double markert_price, double leverage, double percentage, double contract_size, double lots, double freezeMargin)
        {
            if (leverage == 0 | percentage == 0)
            {
                return 0;
            }
            return ((lots * contract_size * markert_price / leverage * percentage / 100) * freezeMargin) / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="close_price"></param>
        /// <param name="open_price"></param>
        /// <param name="contract_size"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double ProfitForex(double close_price, double open_price, double contract_size, double lots)
        {
            return ((close_price - open_price) * contract_size * lots);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="close_price"></param>
        /// <param name="open_price"></param>
        /// <param name="contract_size"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double ProfitCFD(double close_price, double open_price, double contract_size, double lots)
        {
            return ((close_price - open_price) * contract_size * lots);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="close_price"></param>
        /// <param name="open_price"></param>
        /// <param name="tick_price"></param>
        /// <param name="tick_size"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double ProfitFutures(double close_price, double open_price, double tick_price, double tick_size, double lots)
        {
            if (tick_size == 0 | lots == 0)
            {
                return 0;
            }
            return ((close_price - open_price) * tick_price / tick_size * lots);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCommand"></param>
        /// <returns></returns>
        internal Margin CalculationTotalMargin(List<Business.OpenTrade> nlistCommand)
        {
            Margin result = new Margin();
            result.TotalMargin = 0;
            result.TotalFreezeMargin = 0;
            if (nlistCommand == null || nlistCommand.Count == 0)
            {
                return result;
            }

            List<Business.OpenTrade> listCommand = new List<Business.OpenTrade>();

            #region for 1
            for (int i = 0; i < nlistCommand.Count; i++)
            {
                if (nlistCommand[i].Type.ID != 3 & nlistCommand[i].Type.ID != 4 & nlistCommand[i].Type.ID != 7 & nlistCommand[i].Type.ID != 8 &
                    nlistCommand[i].Type.ID != 9 & nlistCommand[i].Type.ID != 10 && nlistCommand[i].Type.ID != 17 && nlistCommand[i].Type.ID != 18 &&
                    nlistCommand[i].Type.ID != 19 && nlistCommand[i].Type.ID != 20 && !nlistCommand[i].IsClose)
                {
                    Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                    newOpenTrade.ClosePrice = nlistCommand[i].ClosePrice;
                    //newOpenTrade.CloseTime = nlistCommand[i].CloseTime;
                    //newOpenTrade.ExpTime = nlistCommand[i].ExpTime;
                    newOpenTrade.ID = nlistCommand[i].ID;
                    newOpenTrade.Investor = nlistCommand[i].Investor;
                    //newOpenTrade.OpenPrice = nlistCommand[i].OpenPrice;
                    //newOpenTrade.OpenTime = nlistCommand[i].OpenTime;
                    newOpenTrade.Size = nlistCommand[i].Size;
                    //newOpenTrade.StopLoss = nlistCommand[i].StopLoss;
                    newOpenTrade.Symbol = new Symbol();
                    newOpenTrade.Symbol = nlistCommand[i].Symbol;
                    //newOpenTrade.TakeProfit = nlistCommand[i].TakeProfit;
                    //newOpenTrade.NumberUpdate = nlistCommand[i].NumberUpdate;
                    newOpenTrade.Type = new TradeType();
                    newOpenTrade.Type = nlistCommand[i].Type;
                    //newOpenTrade.ClientCode = nlistCommand[i].ClientCode;
                    //newOpenTrade.IsClose = nlistCommand[i].IsClose;
                    //newOpenTrade.Profit = nlistCommand[i].Profit;
                    //newOpenTrade.Swap = nlistCommand[i].Swap;
                    //newOpenTrade.Commission = nlistCommand[i].Commission;
                    //newOpenTrade.CommandCode = nlistCommand[i].CommandCode;
                    newOpenTrade.Margin = nlistCommand[i].Margin;
                    newOpenTrade.IsHedged = nlistCommand[i].Symbol.IsHedged;
                    listCommand.Add(newOpenTrade);
                }
            }
            #endregion
            double margin, lots, change;
            if (listCommand.Count > 0)
            {
                double leverage = GetLeverageSymbol(listCommand[0]);
                if (leverage >= 0) leverage = listCommand[0].Investor.Leverage;

                for (int i = 0; i < listCommand.Count; i++)
                {
                    if ((listCommand[i].Type.ID % 2 != 0) && listCommand[i].Size != 0 && listCommand[i].IsHedged)
                    {
                        for (int f = 0; f < listCommand.Count; f++)
                        {
                            #region Non FreezeMargin
                            if ((listCommand[f].Symbol.Name == listCommand[i].Symbol.Name) &&
                                (listCommand[f].Type.ID == listCommand[i].Type.ID + 1) && (listCommand[f].Size != 0) && listCommand[i].IsHedged && !listCommand[i].Symbol.UseFreezeMargin)
                            {
                                //result += listCommand[i].Margin;
                                change = Math.Abs(listCommand[i].Size - listCommand[f].Size);
                                if (change == 0)
                                {
                                    lots = listCommand[i].Size + listCommand[f].Size;
                                    result.TotalMargin += CalculationMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol);
                                    listCommand[f].Size = 0;
                                    listCommand[f].Margin = 0;
                                    listCommand[i].Size = 0;
                                    listCommand[i].Margin = 0;
                                    break;
                                }
                                else
                                {
                                    if (listCommand[i].Size > listCommand[f].Size)
                                    {
                                        margin = (listCommand[i].Margin / listCommand[i].Size) * change;
                                        listCommand[i].Margin = margin;
                                        listCommand[i].Size = change;
                                        lots = listCommand[f].Size * 2;
                                        result.TotalMargin += CalculationMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol);
                                        listCommand[f].Size = 0;
                                    }
                                    else
                                    {
                                        margin = (listCommand[f].Margin / listCommand[f].Size) * change;
                                        listCommand[f].Margin = margin;
                                        listCommand[f].Size = change;
                                        lots = listCommand[i].Size * 2;
                                        result.TotalMargin += CalculationMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol);
                                        listCommand[i].Size = 0;
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region FreezeMargin
                            if ((listCommand[f].Symbol.Name == listCommand[i].Symbol.Name) &&
                               (listCommand[f].Type.ID == listCommand[i].Type.ID + 1) && (listCommand[f].Size != 0) && listCommand[i].IsHedged && listCommand[i].Symbol.UseFreezeMargin)
                            {
                                //result += listCommand[i].Margin;
                                change = Math.Abs(listCommand[i].Size - listCommand[f].Size);
                                if (change == 0)
                                {
                                    lots = listCommand[i].Size + listCommand[f].Size;
                                    result.TotalFreezeMargin += this.CalculationFreezeMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol.FreezeMarginHedged, listCommand[i].Symbol);
                                    listCommand[f].Size = 0;
                                    listCommand[f].Margin = 0;
                                    listCommand[i].Size = 0;
                                    listCommand[i].Margin = 0;
                                    break;
                                }
                                else
                                {
                                    if (listCommand[i].Size > listCommand[f].Size)
                                    {
                                        margin = (listCommand[i].Margin / listCommand[i].Size) * change;
                                        listCommand[i].Margin = margin;
                                        listCommand[i].Size = change;
                                        lots = listCommand[f].Size * 2;
                                        result.TotalFreezeMargin += CalculationFreezeMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol.FreezeMarginHedged, listCommand[i].Symbol);
                                        listCommand[f].Size = 0;
                                    }
                                    else
                                    {
                                        margin = (listCommand[f].Margin / listCommand[f].Size) * change;
                                        listCommand[f].Margin = margin;
                                        listCommand[f].Size = change;
                                        lots = listCommand[i].Size * 2;
                                        result.TotalFreezeMargin += CalculationFreezeMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol.FreezeMarginHedged, listCommand[i].Symbol);
                                        listCommand[i].Size = 0;
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }

                for (int i = 0; i < listCommand.Count; i++)
                {
                    if (listCommand[i].Size != 0)
                    {
                        if (!listCommand[i].Symbol.UseFreezeMargin)
                        {
                            result.TotalMargin += listCommand[i].Margin;
                        }else
                        {
                            result.TotalFreezeMargin += listCommand[i].Margin;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nlistCommand"></param>
        /// <returns></returns>
        internal Margin CalculationTotalMarginPending(List<Business.OpenTrade> nlistCommand, List<Business.OpenTrade> nlistSpotCommand)
        {
            Margin result = new Margin();
            result.TotalMargin = 0;
            result.TotalFreezeMargin = 0;
            if (nlistCommand.Count == 0 && nlistSpotCommand.Count == 0)
            {
                return result;
            }

            List<Business.OpenTrade> listCommand = new List<Business.OpenTrade>();

            #region for 1
            for (int i = 0; i < nlistCommand.Count; i++)
            {
                if (nlistCommand[i].Type.ID != 3 & nlistCommand[i].Type.ID != 4 && nlistCommand[i].Type.ID != 1 && nlistCommand[i].Type.ID != 2
                    && nlistCommand[i].Type.ID != 17 && nlistCommand[i].Type.ID != 18 &&
                    nlistCommand[i].Type.ID != 19 && nlistCommand[i].Type.ID != 20 && !nlistCommand[i].IsClose)
                {
                    Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                    newOpenTrade.ClosePrice = nlistCommand[i].ClosePrice;
                    newOpenTrade.ID = nlistCommand[i].ID;
                    newOpenTrade.Investor = nlistCommand[i].Investor;
                    newOpenTrade.Size = nlistCommand[i].Size;
                    newOpenTrade.Symbol = new Symbol();
                    newOpenTrade.Symbol = nlistCommand[i].Symbol;
                    newOpenTrade.Type = new TradeType();
                    newOpenTrade.Type.ID = Business.Symbol.ConvertCommandIsBuySell(nlistCommand[i].Type.ID);
                    newOpenTrade.Margin = nlistCommand[i].Margin;
                    newOpenTrade.IsHedged = nlistCommand[i].Symbol.IsHedged;
                    listCommand.Add(newOpenTrade);
                }
            }
            for (int i = 0; i < nlistSpotCommand.Count; i++)
            {
                if (nlistSpotCommand[i].Type.ID != 3 & nlistSpotCommand[i].Type.ID != 4
                    && nlistSpotCommand[i].Type.ID != 17 && nlistSpotCommand[i].Type.ID != 18 &&
                    nlistSpotCommand[i].Type.ID != 19 && nlistSpotCommand[i].Type.ID != 20 && !nlistSpotCommand[i].IsClose)
                {
                    Business.OpenTrade newOpenTrade = new Business.OpenTrade();
                    newOpenTrade.ClosePrice = nlistSpotCommand[i].ClosePrice;
                    newOpenTrade.ID = nlistSpotCommand[i].ID;
                    newOpenTrade.Investor = nlistSpotCommand[i].Investor;
                    newOpenTrade.Size = nlistSpotCommand[i].Size;
                    newOpenTrade.Symbol = new Symbol();
                    newOpenTrade.Symbol = nlistSpotCommand[i].Symbol;
                    newOpenTrade.Type = new TradeType();
                    newOpenTrade.Type.ID = Business.Symbol.ConvertCommandIsBuySell(nlistSpotCommand[i].Type.ID);
                    newOpenTrade.Margin = nlistSpotCommand[i].Margin;
                    newOpenTrade.IsHedged = nlistSpotCommand[i].Symbol.IsHedged;
                    listCommand.Add(newOpenTrade);
                }
            }
            #endregion
            double margin, lots, change;
            if (listCommand.Count > 0)
            {
                double leverage = GetLeverageSymbol(listCommand[0]);
                if (leverage >= 0) leverage = listCommand[0].Investor.Leverage;

                for (int i = 0; i < listCommand.Count; i++)
                {
                    if ((listCommand[i].Type.ID % 2 != 0) && listCommand[i].Size != 0 && listCommand[i].IsHedged)
                    {
                        for (int f = 0; f < listCommand.Count; f++)
                        {
                            #region Non FreezeMargin
                            if ((listCommand[f].Symbol.Name == listCommand[i].Symbol.Name) &&
                                (listCommand[f].Type.ID == listCommand[i].Type.ID + 1) && (listCommand[f].Size != 0) && listCommand[i].IsHedged && !listCommand[i].Symbol.UseFreezeMargin)
                            {
                                change = Math.Abs(listCommand[i].Size - listCommand[f].Size);
                                if (change == 0)
                                {
                                    lots = listCommand[i].Size + listCommand[f].Size;
                                    result.TotalMargin += CalculationMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol);
                                    listCommand[f].Size = 0;
                                    listCommand[f].Margin = 0;
                                    listCommand[i].Size = 0;
                                    listCommand[i].Margin = 0;
                                    break;
                                }
                                else
                                {
                                    if (listCommand[i].Size > listCommand[f].Size)
                                    {
                                        margin = (listCommand[i].Margin / listCommand[i].Size) * change;
                                        listCommand[i].Margin = margin;
                                        listCommand[i].Size = change;
                                        lots = listCommand[f].Size * 2;
                                        result.TotalMargin += CalculationMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol);
                                        listCommand[f].Size = 0;
                                    }
                                    else
                                    {
                                        margin = (listCommand[f].Margin / listCommand[f].Size) * change;
                                        listCommand[f].Margin = margin;
                                        listCommand[f].Size = change;
                                        lots = listCommand[i].Size * 2;
                                        result.TotalMargin += CalculationMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol);
                                        listCommand[i].Size = 0;
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region FreezeMargin
                            if ((listCommand[f].Symbol.Name == listCommand[i].Symbol.Name) && (listCommand[f].Type.ID != 7) && (listCommand[f].Type.ID != 8) &&
                                 (listCommand[f].Type.ID != 9) && (listCommand[f].Type.ID != 10) && (listCommand[f].Type.ID == listCommand[i].Type.ID + 1) && (listCommand[f].Size != 0) &&
                                 listCommand[i].IsHedged && listCommand[i].Symbol.UseFreezeMargin)
                            {
                                //result += listCommand[i].Margin;
                                change = Math.Abs(listCommand[i].Size - listCommand[f].Size);
                                if (change == 0)
                                {
                                    lots = listCommand[i].Size + listCommand[f].Size;
                                    result.TotalFreezeMargin += this.CalculationFreezeMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol.FreezeMarginHedged, listCommand[i].Symbol);
                                    listCommand[f].Size = 0;
                                    listCommand[f].Margin = 0;
                                    listCommand[i].Size = 0;
                                    listCommand[i].Margin = 0;
                                    break;
                                }
                                else
                                {
                                    if (listCommand[i].Size > listCommand[f].Size)
                                    {
                                        margin = (listCommand[i].Margin / listCommand[i].Size) * change;
                                        listCommand[i].Margin = margin;
                                        listCommand[i].Size = change;
                                        lots = listCommand[f].Size * 2;
                                        result.TotalFreezeMargin += CalculationFreezeMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol.FreezeMarginHedged, listCommand[i].Symbol);
                                        listCommand[f].Size = 0;
                                    }
                                    else
                                    {
                                        margin = (listCommand[f].Margin / listCommand[f].Size) * change;
                                        listCommand[f].Margin = margin;
                                        listCommand[f].Size = change;
                                        lots = listCommand[i].Size * 2;
                                        result.TotalFreezeMargin += CalculationFreezeMarginHedged(leverage, listCommand[i].Symbol.MarginHedged, lots, listCommand[i].Symbol.FreezeMarginHedged, listCommand[i].Symbol);
                                        listCommand[i].Size = 0;
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }

                for (int i = 0; i < listCommand.Count; i++)
                {
                    if (listCommand[i].Size != 0)
                    {
                        if (!listCommand[i].Symbol.UseFreezeMargin)
                        {
                            if (listCommand[i].Type.ID != 7 && listCommand[i].Type.ID != 8
                                && listCommand[i].Type.ID != 9 && listCommand[i].Type.ID != 10)
                            {
                                result.TotalMargin += listCommand[i].Margin;
                            }
                            if (listCommand[i].Type.ID == 7 || listCommand[i].Type.ID == 8 || 
                                listCommand[i].Type.ID == 9 || listCommand[i].Type.ID == 10)
                            {
                                result.TotalMargin += listCommand[i].Margin;
                            }
                        }
                        else
                        {
                            if ( listCommand[i].Type.ID != 7 && listCommand[i].Type.ID != 8 
                                && listCommand[i].Type.ID != 9 && listCommand[i].Type.ID != 10)
                            {
                                result.TotalFreezeMargin += listCommand[i].Margin;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leverage"></param>
        /// <param name="hedged"></param>
        /// <param name="lots"></param>
        /// <returns></returns>
        internal double CalculationMarginHedged(double leverage, double hedged, double lots, Symbol symbol)
        {
            string marginCalculation = "";
            double percentage = 0;
            int countConfig = symbol.ParameterItems.Count;
            for (int i = 0; i < countConfig; i++)
            {
                if (symbol.ParameterItems[i].Code == "S032")
                {
                    marginCalculation = symbol.ParameterItems[i].StringValue;
                }
                if (symbol.ParameterItems[i].Code == "S031")
                {
                    double.TryParse(symbol.ParameterItems[i].NumValue, out percentage);
                }
            }
            if (marginCalculation == "Futures [ lots * initial_margin * percentage / 100 ]")
            {
                return (lots * hedged * percentage / 100);
            }
            else
            {
                return lots * (hedged / leverage);
            }
        }

        internal double CalculationFreezeMarginHedged(double leverage, double hedged, double lots, double percentFreeze, Symbol symbol)
        {
            string marginCalculation = "";
            double percentage = 0;
            int countConfig = symbol.ParameterItems.Count;
            for (int i = 0; i < countConfig; i++)
            {
                if (symbol.ParameterItems[i].Code == "S032")
                {
                    marginCalculation = symbol.ParameterItems[i].StringValue;
                }
                if (symbol.ParameterItems[i].Code == "S031")
                {
                    double.TryParse(symbol.ParameterItems[i].NumValue, out percentage);
                }
            }
            if (marginCalculation == "Futures [ lots * initial_margin * percentage / 100 ]")
            {
                return ((lots * hedged * percentage / 100) * percentFreeze) / 100;
            }
            else
            {
                return ((lots * (hedged / leverage)) * percentFreeze) / 100;
            }
        }

        #region GetLeverage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Leverage"></param>
        /// <returns></returns>
        internal double GetLeverageSymbol(Business.OpenTrade Command)
        {
            double Leverage = 0;
            List<Business.IGroupSymbol> ListIGroupSymbol = new List<IGroupSymbol>();
            ListIGroupSymbol = TradingServer.Facade.FacadeGetIGroupSymbolBySymbolID(Command.Symbol.SymbolID);

            if (ListIGroupSymbol != null && ListIGroupSymbol.Count > 0)
            {
                if (Business.Market.InvestorGroupList != null)
                {
                    int countInvestor = Business.Market.InvestorGroupList.Count;
                    for (int i = 0; i < countInvestor; i++)
                    {
                        if (Business.Market.InvestorGroupList[i].InvestorGroupID == ListIGroupSymbol[0].InvestorGroupID)
                        {
                            if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                            {
                                int countConfig = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                                for (int j = 0; j < countConfig; j++)
                                {
                                    if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G02")
                                    {
                                        double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].NumValue, out Leverage);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Leverage;
        }
        #endregion

        #region Get Hedged Of Symbol
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal double GetHedgedOfSymbol(Business.OpenTrade Command)
        {
            double Hedged = 0;
            if (Command.Symbol.ParameterItems != null)
            {
                int count = Command.Symbol.ParameterItems.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Command.Symbol.ParameterItems[i].Code == "S028")
                    {
                        double.TryParse(Command.Symbol.ParameterItems[i].NumValue, out Hedged);
                    }
                }
            }

            return Hedged;
        }
        #endregion
    }
}
