using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class OpenTrade
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newOpenTrade"></param>
        public void CalculatorMarginCommand(Business.OpenTrade newOpenTrade)
        {
            #region Get Parameters Config In Tab Calculation
            double Percentage = 0;
            double InitMarket = 0;
            double Leverage = 0;
            double Margin = 0;
            string MarginCalculation = string.Empty;
            string Currency = newOpenTrade.Symbol.Currency;
            #endregion

            #region Select Config Symbol
            if (newOpenTrade.Symbol.ParameterItems != null)
            {
                int countConfig = newOpenTrade.Symbol.ParameterItems.Count;
                for (int i = 0; i < countConfig; i++)
                {
                    //if (newOpenTrade.Symbol.ParameterItems[i].Code == "S007")
                    //{                        
                    //    //Currency = newOpenTrade.Symbol.ParameterItems[i].StringValue;
                        
                    //}

                    if (newOpenTrade.Symbol.ParameterItems[i].Code == "S026")
                    {                        
                        double.TryParse(newOpenTrade.Symbol.ParameterItems[i].NumValue, out InitMarket);
                    }

                    if (newOpenTrade.Symbol.ParameterItems[i].Code == "S031")
                    {
                        double.TryParse(newOpenTrade.Symbol.ParameterItems[i].NumValue, out Percentage);
                        
                    }

                    if (newOpenTrade.Symbol.ParameterItems[i].Code == "S032")
                    {                        
                        MarginCalculation = newOpenTrade.Symbol.ParameterItems[i].StringValue;
                    }
                }
            }
            #endregion

            #region COMMENT CODE(17-04-2012) BECAUSE LEVERAGE IN GROUP AVAREBLE IN CREATE ACCOUNT
            //if (newOpenTrade.Investor.InvestorGroupInstance.ParameterItems != null)
            //{
            //    int countGroupConfig = newOpenTrade.Investor.InvestorGroupInstance.ParameterItems.Count;
            //    for (int j = 0; j < countGroupConfig; j++)
            //    {
            //        if (newOpenTrade.Investor.InvestorGroupInstance.ParameterItems[j].Code == "G02")
            //        {
            //            double.TryParse(newOpenTrade.Investor.InvestorGroupInstance.ParameterItems[j].NumValue, out Leverage);

            //            break;
            //        }
            //    }
            //}
            #endregion            

            #region Get Parameter Leverage In List Security
            List<Business.IGroupSymbol> ListIGroupSymbol = new List<IGroupSymbol>();

            //if (Business.Market.IGroupSymbolList != null)
            //{
            //    int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
            //    for (int j = 0; j < countIGroupSymbol; j++)
            //    {
            //        if (Business.Market.IGroupSymbolList[j].SymbolID == newOpenTrade.Symbol.SymbolID)
            //        {
            //            ListIGroupSymbol.Add(Business.Market.IGroupSymbolList[j]);                        
            //        }
            //    }
            //}
            
            //ListIGroupSymbol = TradingServer.Facade.FacadeGetIGroupSymbolBySymbolID(newOpenTrade.Symbol.SymbolID);

            //if (ListIGroupSymbol != null && ListIGroupSymbol.Count > 0)
            //{
            //    if (Business.Market.InvestorGroupList != null)
            //    {
            //        int countInvestor = Business.Market.InvestorGroupList.Count;
            //        for (int i = 0; i < countInvestor; i++)
            //        {
            //            if (Business.Market.InvestorGroupList[i].InvestorGroupID == ListIGroupSymbol[0].InvestorGroupID)
            //            {
            //                if (Business.Market.InvestorGroupList[i].ParameterItems != null)
            //                {
            //                    int countConfig = Business.Market.InvestorGroupList[i].ParameterItems.Count;
            //                    for (int j = 0; j < countConfig; j++)
            //                    {
            //                        if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G02")
            //                        {
            //                            double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].NumValue, out Leverage);

            //                            break;
            //                        }
            //                    }
            //                }

            //                break;
            //            }
            //        }
            //    }
            //}

            //if (Leverage <= 0)
            //{
            //    Leverage = newOpenTrade.Investor.Leverage;
            //}
            #endregion

            #region GET PERCENTAGE IN IGROUP SYMBOL(COMMENT) BECAUSE IGROUP SECURITY AND IGROUP SYMBOL NOT AVAREBLE
            //if (Business.Market.IGroupSymbolList != null)
            //{
            //    int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
            //    for (int i = 0; i < countIGroupSymbol; i++)
            //    {
            //        if (Business.Market.IGroupSymbolList[i].SymbolID == newOpenTrade.Symbol.SymbolID &&
            //            Business.Market.IGroupSymbolList[i].InvestorGroupID == newOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID)
            //        {
            //            if (newOpenTrade.Investor.InvestorGroupInstance.ParameterItems != null)
            //            {
            //                int countGroupConfig = newOpenTrade.Investor.InvestorGroupInstance.ParameterItems.Count;
            //                for (int j = 0; j < countGroupConfig; j++)
            //                {
            //                    if (newOpenTrade.Investor.InvestorGroupInstance.ParameterItems[j].Code == "GS02")
            //                    {
            //                        double.TryParse(newOpenTrade.Investor.InvestorGroupInstance.ParameterItems[j].NumValue, out Leverage);

            //                        break;
            //                    }
            //                }
            //            }

            //            if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig != null)
            //            {
            //                int countParameter = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count;
            //                for (int j = 0; j < countParameter; j++)
            //                {
            //                    if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Code == "GS03")
            //                    {
            //                        double tempPercentage = double.Parse(Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].NumValue);
            //                        Percentage = (Percentage * tempPercentage) / 100;
            //                        break;
            //                    }
            //                }
            //            }

            //            break;
            //        }
            //    }
            //}
            #endregion

            //if (Leverage <= 0)
            //{
                Leverage = newOpenTrade.Investor.Leverage;
            //}

            #region Calculation Margin Of Investor
            switch (MarginCalculation)
            {
                case "Forex [ lots * contract_size / leverage * percentage / 100 ]":
                    if (newOpenTrade.Symbol.UseFreezeMargin)
                    {
                        Margin = newOpenTrade.Symbol.MarginFreezeForex(Leverage, Percentage, newOpenTrade.Symbol.ContractSize, InitMarket, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
                    }
                    else
                    {
                        Margin = newOpenTrade.Symbol.MarginForex(Leverage, Percentage, newOpenTrade.Symbol.ContractSize, InitMarket, newOpenTrade.Size);
                    }                    
                    break;

                case "CFD [ lots * contract_size / market_price * percentage / 100 ]":
                    if (newOpenTrade.Symbol.UseFreezeMargin)
                    {
                        Margin = newOpenTrade.Symbol.MarginFreezeCFD(newOpenTrade.OpenPrice, Percentage, newOpenTrade.Symbol.FreezeMargin, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
                    }
                    else
                    {
                        Margin = newOpenTrade.Symbol.MarginCFD(newOpenTrade.OpenPrice, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }                    
                    break;  

                case "Futures [ lots * initial_margin * percentage / 100 ]":
                    if (newOpenTrade.Symbol.UseFreezeMargin)
                    {
                        Margin = newOpenTrade.Symbol.MarginFreezeFutures(InitMarket, Percentage, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
                    }
                    else
                    {
                        Margin = newOpenTrade.Symbol.MarginFutures(InitMarket, Percentage, newOpenTrade.Size);
                    }                    
                    break;

                case "CFD-index [ lots * contract_size / market_price / tick_size * price * percentage / 100 ]":
                    if (newOpenTrade.Symbol.UseFreezeMargin)
                    {
                        Margin = newOpenTrade.Symbol.MarginCFDFreeze_Index(newOpenTrade.OpenPrice, newOpenTrade.Symbol.TickSize, newOpenTrade.OpenPrice, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
                    }
                    else
                    {
                        Margin = newOpenTrade.Symbol.MarginCFD_Index(newOpenTrade.OpenPrice, newOpenTrade.Symbol.TickSize, newOpenTrade.Symbol.TickPrice, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }                    
                    break;

                case "CFD-leverage [ lots * contract_size / market_price / leverage * percentage / 100 ]":
                    if (newOpenTrade.Symbol.UseFreezeMargin)
                    {
                        Margin = newOpenTrade.Symbol.MarginFreezeCFD_Leverage(newOpenTrade.OpenPrice, Leverage, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
                    }
                    else
                    {
                        Margin = newOpenTrade.Symbol.MarginCFD_Leverage(newOpenTrade.OpenPrice, Leverage, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }                    
                    break;
            }
            #endregion

            //Set Margin For Command
            newOpenTrade.Margin = Margin;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="newOpenTrade"></param>
        //public double CalculationFreezeMarginCommand(Business.OpenTrade newOpenTrade)
        //{
        //    #region Get Parameters Config In Tab Calculation
        //    double Percentage = 0;            
        //    double Leverage = 0;
        //    double FreezeMargin = 0;
        //    string MarginCalculation = string.Empty;
        //    string Currency = newOpenTrade.Symbol.Currency;
        //    #endregion

        //    #region Select Config Symbol
        //    if (newOpenTrade.Symbol.ParameterItems != null)
        //    {
        //        int countConfig = newOpenTrade.Symbol.ParameterItems.Count;
        //        for (int i = 0; i < countConfig; i++)
        //        {                    
        //            if (newOpenTrade.Symbol.ParameterItems[i].Code == "S031")
        //            {
        //                double.TryParse(newOpenTrade.Symbol.ParameterItems[i].NumValue, out Percentage);

        //            }

        //            if (newOpenTrade.Symbol.ParameterItems[i].Code == "S032")
        //            {
        //                MarginCalculation = newOpenTrade.Symbol.ParameterItems[i].StringValue;
        //            }
        //        }
        //    }
        //    #endregion

        //    #region Get Parameter Leverage In List Security
        //    List<Business.IGroupSymbol> ListIGroupSymbol = new List<IGroupSymbol>();
        //    #endregion

        //    #region GET PERCENTAGE IN IGROUP SYMBOL
        //    if (Business.Market.IGroupSymbolList != null)
        //    {
        //        int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
        //        for (int i = 0; i < countIGroupSymbol; i++)
        //        {
        //            if (Business.Market.IGroupSymbolList[i].SymbolID == newOpenTrade.Symbol.SymbolID &&
        //                Business.Market.IGroupSymbolList[i].InvestorGroupID == newOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID)
        //            {
        //                if (newOpenTrade.Investor.InvestorGroupInstance.ParameterItems != null)
        //                {
        //                    int countGroupConfig = newOpenTrade.Investor.InvestorGroupInstance.ParameterItems.Count;
        //                    for (int j = 0; j < countGroupConfig; j++)
        //                    {
        //                        if (newOpenTrade.Investor.InvestorGroupInstance.ParameterItems[j].Code == "GS02")
        //                        {
        //                            double.TryParse(newOpenTrade.Investor.InvestorGroupInstance.ParameterItems[j].NumValue, out Leverage);

        //                            break;
        //                        }
        //                    }
        //                }

        //                if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig != null)
        //                {
        //                    int countParameter = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count;
        //                    for (int j = 0; j < countParameter; j++)
        //                    {
        //                        if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Code == "GS03")
        //                        {
        //                            double tempPercentage = double.Parse(Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].NumValue);
        //                            Percentage = (Percentage * tempPercentage) / 100;
        //                            break;
        //                        }
        //                    }
        //                }

        //                break;
        //            }
        //        }
        //    }
        //    #endregion

        //    if (Leverage <= 0)
        //    {
        //        Leverage = newOpenTrade.Investor.Leverage;
        //    }

        //    #region Calculation Margin Of Investor
        //    switch (MarginCalculation)
        //    {
        //        case "Forex [ lots * contract_size / leverage * percentage / 100 ]":
        //            FreezeMargin = newOpenTrade.Symbol.MarginFreezeForex(Leverage, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Symbol.InitialMargin, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
        //            break;

        //        case "CFD [ lots * contract_size / market_price * percentage / 100 ]":
        //            FreezeMargin = newOpenTrade.Symbol.MarginFreezeCFD(newOpenTrade.OpenPrice, Percentage, newOpenTrade.Symbol.FreezeMargin, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
        //            break;

        //        case "Futures [ lots * initial_margin * percentage / 100 ]":
        //            FreezeMargin = newOpenTrade.Symbol.MarginFreezeFutures(newOpenTrade.Symbol.InitialMargin, Percentage, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
        //            break;

        //        case "CFD-index [ lots * contract_size / market_price / tick_size * price * percentage / 100 ]":
        //            FreezeMargin = newOpenTrade.Symbol.MarginCFDFreeze_Index(newOpenTrade.OpenPrice, newOpenTrade.Symbol.TickSize, newOpenTrade.OpenPrice, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
        //            break;

        //        case "CFD-leverage [ lots * contract_size / market_price / leverage * percentage / 100 ]":
        //            FreezeMargin = newOpenTrade.Symbol.MarginFreezeCFD_Leverage(newOpenTrade.OpenPrice, Leverage, Percentage, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size, newOpenTrade.Symbol.FreezeMargin);
        //            break;
        //    }
        //    #endregion

        //    //Set Margin For Command
        //    newOpenTrade.FreezeMargin = FreezeMargin;

        //    return FreezeMargin;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newOpenTrade"></param>
        internal void CalculatorProfitCommand(Business.OpenTrade newOpenTrade)
        {
            #region Call Function Calculation Profit
            int valueCheck = Business.Symbol.ConvertCommandIsBuySell(newOpenTrade.Type.ID);
            switch (newOpenTrade.Symbol.ProfitCalculation)
            {
                case "Forex [ (close_price - open_price) * contract_size * lots ]":
                    if (valueCheck == 2)
                    {
                        newOpenTrade.Profit = newOpenTrade.Symbol.ProfitForex(newOpenTrade.OpenPrice, newOpenTrade.ClosePrice, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }
                    else
                    {
                        newOpenTrade.Profit = newOpenTrade.Symbol.ProfitForex(newOpenTrade.ClosePrice, newOpenTrade.OpenPrice, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }
                    break;

                case "CFD [ (close_price - open_price) * contract_size * lots ]":
                    if (valueCheck == 2)
                    {
                        newOpenTrade.Profit = newOpenTrade.Symbol.ProfitCFD(newOpenTrade.OpenPrice, newOpenTrade.ClosePrice, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }
                    else
                    {
                        newOpenTrade.Profit = newOpenTrade.Symbol.ProfitCFD(newOpenTrade.ClosePrice, newOpenTrade.OpenPrice, newOpenTrade.Symbol.ContractSize, newOpenTrade.Size);
                    }
                    break;

                case "Futures [ (close_price - open_price) * tick_price / tick_size * lots ]":
                    if (valueCheck == 2)
                    {
                        newOpenTrade.Profit = newOpenTrade.Symbol.ProfitFutures(newOpenTrade.OpenPrice, newOpenTrade.ClosePrice, newOpenTrade.Symbol.TickPrice, newOpenTrade.Symbol.TickSize, newOpenTrade.Size);
                    }
                    else
                    {
                        newOpenTrade.Profit = newOpenTrade.Symbol.ProfitFutures(newOpenTrade.ClosePrice, newOpenTrade.OpenPrice, newOpenTrade.Symbol.TickPrice, newOpenTrade.Symbol.TickSize, newOpenTrade.Size);
                    }
                    break;
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public bool CheckValidAccountInvestor(Business.OpenTrade Command)
        {
            bool Result = false;
            if (Command != null)
            {
                //Calculator Margin Level
                double tempMarginLevel = 0;
                double tempProfit = 0;
                double tempTotalMargin = 0;
                double tempEquity = 0;
                double tempFreeMargin = 0;
                double marginPending = 0;
                double marginCommand = 0;
                bool isExits = false;

                List<Business.OpenTrade> tempOpenTrade = new List<OpenTrade>();
                List<Business.OpenTrade> listPending = new List<OpenTrade>();
                if (Command.Investor.CommandList != null)
                {
                    int count = Command.Investor.CommandList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Command.Investor.CommandList[i].ID == Command.ID)
                            isExits = true;

                        bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Investor.CommandList[i].Type.ID);
                        if (isPending)
                        {
                            Command.CalculatorMarginCommand(Command.Investor.CommandList[i]);
                            listPending.Add(Command.Investor.CommandList[i]);
                        }
                        else
                        {
                            Command.CalculatorMarginCommand(Command.Investor.CommandList[i]);
                            tempOpenTrade.Add(Command.Investor.CommandList[i]);
                        }

                        //if (Command.Investor.CommandList[i].Type.ID == 7 ||
                        //    Command.Investor.CommandList[i].Type.ID == 8 ||
                        //    Command.Investor.CommandList[i].Type.ID == 9 ||
                        //    Command.Investor.CommandList[i].Type.ID == 10 ||
                        //    Command.Investor.CommandList[i].Type.ID == 17 ||
                        //    Command.Investor.CommandList[i].Type.ID == 18 ||
                        //    Command.Investor.CommandList[i].Type.ID == 19 ||
                        //    Command.Investor.CommandList[i].Type.ID == 20)
                        //{
                        //    if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10 ||
                        //        Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20)
                        //    {
                        //        Command.CalculatorMarginCommand(Command.Investor.CommandList[i]);
                        //        listPending.Add(Command.Investor.CommandList[i]);
                        //        marginPending += Command.Investor.CommandList[i].Margin;
                        //    }
                        //}
                        //else
                        //{
                        //tempOpenTrade.Add(Command.Investor.CommandList[i]);
                        //}
                    }
                }

                Command.CalculatorMarginCommand(Command);

                if (!isExits)
                {
                    bool isPendingCommand = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Command.Type.ID);
                    if (isPendingCommand)
                        listPending.Add(Command);
                    else
                        tempOpenTrade.Add(Command);
                }

                //Business.Margin newMargin = new Business.Margin();
                //newMargin = Command.Symbol.CalculationTotalMargin(tempOpenTrade);
                ////marginCommand = newMargin.TotalMargin + newMargin.TotalFreezeMargin;
                //marginCommand = newMargin.TotalFreezeMargin;

                Business.Margin newMargin = new Business.Margin();
                newMargin = Command.Symbol.CalculationTotalMarginPending(listPending, tempOpenTrade);
                //tempTotalMargin = marginCommand + marginPending;
                tempTotalMargin = newMargin.TotalMargin + newMargin.TotalFreezeMargin;

                //if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10 ||
                //    Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20)
                //{
                //    tempTotalMargin = marginCommand + Command.Margin + marginPending;
                //}
                //else
                //{
                //    tempTotalMargin = marginCommand;
                //}

                tempProfit = Command.Investor.Profit + Command.Profit;
                tempEquity = Command.Investor.Balance + Command.Investor.Credit + tempProfit;
                tempFreeMargin = tempEquity - tempTotalMargin;
                tempMarginLevel = (tempEquity * 100) / tempTotalMargin;

                if (tempMarginLevel >= 100)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        }

        public string MapPriceForDigit(double price)
        {
            string tem1 = price.ToString();
            string[] split = tem1.Split('.');
            int digit = this.Symbol.Digit;
            if (split.Length > 1)
            {
                for (int i = split[1].Length; i < digit; i++)
                {
                    tem1 += "0";
                }
            }
            else
            {
                if (digit != 0)
                {
                    tem1 = tem1 + ".";
                    for (int i = 0; i < digit; i++)
                    {
                        tem1 += "0";
                    }
                }
            }
            return tem1;
        }

        public string FormatDoubleToString(double value)
        {
            string result = value.ToString("### ### ### ### ### ### ##0.00");
            if (value < 0)
            {
                for (int i = 1; i < result.Length; i++)
                {
                    if (result[i].ToString() != " ")
                    {
                        break;
                    }
                    if (result[i].ToString() == " ")
                    {
                        result = result.Remove(i, 1);
                        i = i - 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].ToString() != " ")
                    {
                        break;
                    }
                    if (result[i].ToString() == " ")
                    {
                        result = result.Remove(i, 1);
                        i = i - 1;
                    }
                }
            }
            return result;
        }
    }
}
