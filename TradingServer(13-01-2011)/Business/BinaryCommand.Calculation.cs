using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class BinaryCommand
    {        
        /// <summary>
        /// CHECK TIME OF SESSION
        /// </summary>
        /// <returns></returns>
        private bool CheckTimeSession()
        {
            TimeSpan temp = DateTime.Now.TimeOfDay;

            int tempStartMarket = temp.Hours - BinaryCommand.TimeStartMarket.Hour;
            int tempCloseMarket = BinaryCommand.TimeCloseMarket.Hour - DateTime.Now.Hour;

            if (tempStartMarket >= 0 && tempCloseMarket >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// CALCULATION NEXT TIME 5 MINUTE
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        private DateTime CalNextTime(DateTime Time)
        {
            DateTime TimeNext = DateTime.Now;
            try
            {
                int temp = (5 - (Time.Minute % 5)) + Time.Minute;
                if (temp >= 60)
                {
                    TimeNext = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, 00, 00);
                    TimeNext = TimeNext.AddHours(1);
                }
                else
                {
                    TimeNext = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, temp, 00);
                }
            }
            catch (Exception ex)
            {

            }
            return TimeNext;
        }

        /// <summary>
        /// GET PRICE START
        /// </summary>
        private void GetPricesStart()
        {
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].MarketAreaRef.IMarketAreaName == "BinaryCommand")
                    {
                        if (Business.BinaryCommand.PriceStartSession == null)
                            Business.BinaryCommand.PriceStartSession = new Dictionary<string, Tick>();

                        Business.BinaryCommand.PriceStartSession.Add(Business.Market.SymbolList[i].Name, Business.Market.SymbolList[i].TickValue);
                    }

                    if (Business.Market.SymbolList[i].RefSymbol != null)
                    {
                        List<Business.Tick> tempResult = new List<Tick>();
                        tempResult = this.GetPriceStopReference(Business.Market.SymbolList[i].RefSymbol);

                        if (tempResult != null)
                        {
                            int countResult = tempResult.Count;
                            for (int j = 0; j < countResult; j++)
                            {
                                if (Business.BinaryCommand.PriceStartSession == null)
                                    Business.BinaryCommand.PriceStartSession = new Dictionary<string, Tick>();

                                Business.BinaryCommand.PriceStartSession.Add(tempResult[j].SymbolName, tempResult[j]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GET PRICE STOP
        /// </summary>
        private void GetPricesStop()
        {
            if (Business.Market.SymbolList != null)
            {                
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].MarketAreaRef != null)
                    {
                        if (Business.Market.SymbolList[i].MarketAreaRef.IMarketAreaName == "BinaryCommand")
                        {
                            if (Business.BinaryCommand.PriceStopSession == null)
                                Business.BinaryCommand.PriceStopSession = new Dictionary<string, Tick>();

                            Business.BinaryCommand.PriceStopSession.Add(Business.Market.SymbolList[i].Name, Business.Market.SymbolList[i].TickValue);
                        }

                        if (Business.Market.SymbolList[i].RefSymbol != null)
                        {
                            List<Business.Tick> tempResult = new List<Tick>();
                            tempResult = this.GetPriceStopReference(Business.Market.SymbolList[i].RefSymbol);
                            if (tempResult != null)
                            {
                                int countResult = tempResult.Count;
                                for (int j = 0; j < countResult; j++)
                                {
                                    if (Business.BinaryCommand.PriceStopSession == null)
                                        Business.BinaryCommand.PriceStopSession = new Dictionary<string, Tick>();

                                    Business.BinaryCommand.PriceStopSession.Add(tempResult[j].SymbolName, tempResult[j]);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GET PRICE STOP REFERENCE
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <returns></returns>
        private List<Business.Tick> GetPriceStopReference(List<Business.Symbol> ListSymbol)
        {
            List<Business.Tick> Result = new List<Tick>();
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].MarketAreaRef.IMarketAreaName == "BinaryCommand")
                    {
                        Result.Add(ListSymbol[i].TickValue);
                    }

                    if (ListSymbol[i].RefSymbol != null)
                    {
                        List<Business.Tick> tempResult = new List<Tick>();
                        tempResult= this.GetPriceStopReference(ListSymbol[i].RefSymbol);
                        if (tempResult != null)
                        {
                            int countResult = tempResult.Count;
                            for (int j = 0; j < countResult; j++)
                            {
                                Result.Add(tempResult[j]);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// FILL PRICES START
        /// </summary>
        private void FillPricesStart()
        {
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].MarketAreaRef.IMarketAreaName == "BinaryCommand")
                    {
                        if (Business.Market.SymbolList[i].BinaryCommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].BinaryCommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 3)
                                {
                                    #region Fill Prices Start For UpBinary Command
                                    if (Business.BinaryCommand.PriceStartSession.ContainsKey(Business.Market.SymbolList[i].Name))
                                    {
                                        TradingServer.Business.Tick newTickOnline = new Tick();
                                        Business.BinaryCommand.PriceStartSession.TryGetValue(Business.Market.SymbolList[i].Name, out newTickOnline);

                                        Business.Market.SymbolList[i].BinaryCommandList[j].OpenPrice = newTickOnline.Ask;

                                        //Call Function Update Price Open In List Investor
                                        Business.Market.SymbolList[i].BinaryCommandList[j].Investor.UpdateCommand(Business.Market.SymbolList[i].BinaryCommandList[j]);

                                        if (Business.Market.SymbolList[i].BinaryCommandList[j].Investor.ClientBinaryQueue == null)
                                            Business.Market.SymbolList[i].BinaryCommandList[j].Investor.ClientBinaryQueue = new List<string>();

                                        bool IsBuy = false;
                                        //Set IsBuy Of Binary Command
                                        if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 3)
                                            IsBuy = true;

                                        //Add String Command Of Server To Client
                                        string Message = string.Empty;
                                        Message = "FillPrices$True,Fill Price Start Binary," + Business.Market.SymbolList[i].BinaryCommandList[j].ID + "," + 
                                            Business.Market.SymbolList[i].BinaryCommandList[j].Investor.InvestorID + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].Symbol.Name + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].Size + "," + IsBuy + "," + Business.Market.SymbolList[i].BinaryCommandList[j].OpenTime + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].OpenPrice + "," + Business.Market.SymbolList[i].BinaryCommandList[j].StopLoss + "," + Business.Market.SymbolList[i].BinaryCommandList[j].TakeProfit + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].ClosePrice + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Commission + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Swap + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].Profit + "," + "Comment," + Business.Market.SymbolList[i].BinaryCommandList[j].ID + "," + "BinaryTrading" + "," +
                                                    1 + "," + Business.Market.SymbolList[i].BinaryCommandList[j].ExpTime + "," + Business.Market.SymbolList[i].BinaryCommandList[j].ClientCode + "," + Business.Market.SymbolList[i].BinaryCommandList[j].CommandCode + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].IsHedged + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Margin + "," + ",Binary";

                                        Business.Market.SymbolList[i].BinaryCommandList[j].Investor.ClientBinaryQueue.Add(Message);
                                    }
                                    #endregion                                    
                                }   //End If
                                else if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 4)
                                {
                                    #region Fill Prices Start For
                                    if (Business.BinaryCommand.PriceStartSession.ContainsKey(Business.Market.SymbolList[i].Name))
                                    {
                                        TradingServer.Business.Tick newTickOnline = new Tick();
                                        Business.BinaryCommand.PriceStartSession.TryGetValue(Business.Market.SymbolList[i].Name, out newTickOnline);
                                        if (newTickOnline == null || newTickOnline.Bid < 0)
                                            return;

                                        Business.Market.SymbolList[i].BinaryCommandList[j].OpenPrice = newTickOnline.Bid;

                                        //Call Function Update Open Price In Investor List
                                        Business.Market.SymbolList[i].BinaryCommandList[j].Investor.UpdateCommand(Business.Market.SymbolList[i].BinaryCommandList[j]);

                                        if (Business.Market.SymbolList[i].BinaryCommandList[j].Investor.ClientBinaryQueue == null)
                                            Business.Market.SymbolList[i].BinaryCommandList[j].Investor.ClientBinaryQueue = new List<string>();

                                        bool IsBuy = false;
                                        if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 3)
                                            IsBuy = true;

                                        //Add String Command Of Server To Client
                                        string Message = string.Empty;
                                        Message = "FillPrices$True,Fill Price Start Binary," + Business.Market.SymbolList[i].BinaryCommandList[j].ID + "," + 
                                            Business.Market.SymbolList[i].BinaryCommandList[j].Investor.InvestorID + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].Symbol.Name + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].Size + "," + IsBuy + "," + 
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].OpenTime + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].OpenPrice + "," + Business.Market.SymbolList[i].BinaryCommandList[j].StopLoss + "," + Business.Market.SymbolList[i].BinaryCommandList[j].TakeProfit + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].ClosePrice + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Commission + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Swap + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].Profit + "," + "Comment," + Business.Market.SymbolList[i].BinaryCommandList[j].ID + "," + "BinaryTrading" + "," +
                                                    1 + "," + Business.Market.SymbolList[i].BinaryCommandList[j].ExpTime + "," + Business.Market.SymbolList[i].BinaryCommandList[j].ClientCode + "," + Business.Market.SymbolList[i].BinaryCommandList[j].CommandCode + "," +
                                                    Business.Market.SymbolList[i].BinaryCommandList[j].IsHedged + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID + "," + Business.Market.SymbolList[i].BinaryCommandList[j].Margin + ",Binary";

                                        Business.Market.SymbolList[i].BinaryCommandList[j].Investor.ClientBinaryQueue.Add(Message);
                                    }
                                    #endregion                                    
                                }   //End Else If

                                #region Fill Prices Start References
                                if (Business.Market.SymbolList[i].RefSymbol != null && Business.Market.SymbolList[i].RefSymbol.Count > 0)
                                {
                                    this.FillPricesStartReference(Business.Market.SymbolList[i].RefSymbol);
                                }
                                #endregion                                
                            }   //End For
                        }   //End Check Null Binary Command List
                    }   //End Check Binary Market Area
                }   //End For Symbol List
            }   //End Check Symbol != null
        }   //End Function

        /// <summary>
        /// FILL PRICES START REFERENCE
        /// </summary>
        /// <param name="ListSymbol"></param>
        private void FillPricesStartReference(List<Business.Symbol> ListSymbol)
        {
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].BinaryCommandList != null)
                    {
                        int countCommand = ListSymbol[i].BinaryCommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (ListSymbol[i].BinaryCommandList[j].Type.ID == 3)
                            {
                                if (Business.BinaryCommand.PriceStartSession.ContainsKey(ListSymbol[i].Name))
                                {
                                    TradingServer.Business.Tick newTickOnline = new Tick();
                                    Business.BinaryCommand.PriceStartSession.TryGetValue(ListSymbol[i].Name, out newTickOnline);
                                    ListSymbol[i].BinaryCommandList[j].OpenPrice = newTickOnline.Ask;

                                    //Call Function Update Open Price In Investor List
                                    ListSymbol[i].BinaryCommandList[j].Investor.UpdateCommand(ListSymbol[i].BinaryCommandList[j]);

                                    #region Add Message Alert To Client
                                    if (ListSymbol[i].BinaryCommandList[j].Investor.ClientBinaryQueue == null)
                                        ListSymbol[i].BinaryCommandList[j].Investor.ClientBinaryQueue = new List<string>();

                                    bool IsBuy =false;
                                    if (ListSymbol[i].BinaryCommandList[j].Type.ID == 3)
                                        IsBuy = true;

                                    //Add String Command Of Server To Client
                                    string Message = string.Empty;
                                    Message = "FillPrices$True,Fill Price Start Binary," + ListSymbol[i].BinaryCommandList[j].ID + "," + ListSymbol[i].BinaryCommandList[j].Investor.InvestorID + "," +
                                                ListSymbol[i].BinaryCommandList[j].Symbol.Name + "," + ListSymbol[i].BinaryCommandList[j].Size + "," + IsBuy + "," + ListSymbol[i].BinaryCommandList[j].OpenTime + "," +
                                                ListSymbol[i].BinaryCommandList[j].OpenPrice + "," + ListSymbol[i].BinaryCommandList[j].StopLoss + "," + ListSymbol[i].BinaryCommandList[j].TakeProfit + "," +
                                                ListSymbol[i].BinaryCommandList[j].ClosePrice + "," + ListSymbol[i].BinaryCommandList[j].Commission + "," + ListSymbol[i].BinaryCommandList[j].Swap + "," +
                                                ListSymbol[i].BinaryCommandList[j].Profit + "," + "Comment," + ListSymbol[i].BinaryCommandList[j].ID + "," + "BinaryTrading" + "," +
                                                1 + "," + ListSymbol[i].BinaryCommandList[j].ExpTime + "," + ListSymbol[i].BinaryCommandList[j].ClientCode + "," + ListSymbol[i].BinaryCommandList[j].CommandCode + "," +
                                                ListSymbol[i].BinaryCommandList[j].IsHedged + "," + ListSymbol[i].BinaryCommandList[j].Type.ID + "," + ListSymbol[i].BinaryCommandList[j].Margin + ",Binary";

                                    ListSymbol[i].BinaryCommandList[j].Investor.ClientBinaryQueue.Add(Message);
                                    #endregion                                    
                                }
                            }
                            else if (ListSymbol[i].BinaryCommandList[j].Type.ID == 4)
                            {
                                if (Business.BinaryCommand.PriceStartSession.ContainsKey(ListSymbol[i].Name))
                                {
                                    ListSymbol[i].BinaryCommandList[j].OpenPrice = ListSymbol[i].TickValue.Bid;

                                    //Call Function Update Open Price In Investor List
                                    ListSymbol[i].BinaryCommandList[j].Investor.UpdateCommand(ListSymbol[i].BinaryCommandList[j]);

                                    #region Add Message Alert To Client
                                    if (ListSymbol[i].BinaryCommandList[j].Investor.ClientBinaryQueue == null)
                                        ListSymbol[i].BinaryCommandList[j].Investor.ClientBinaryQueue = new List<string>();

                                    bool IsBuy = false;
                                    if (ListSymbol[i].BinaryCommandList[j].Type.ID == 3)
                                        IsBuy = true;

                                    //Add String Command Of Server To Client
                                    string Message = string.Empty;
                                    Message = "FillPrices$True,Fill Price Start Binary," + ListSymbol[i].BinaryCommandList[j].ID + "," + ListSymbol[i].BinaryCommandList[j].Investor.InvestorID + "," +
                                                ListSymbol[i].BinaryCommandList[j].Symbol.Name + "," + ListSymbol[i].BinaryCommandList[j].Size + "," + IsBuy + "," + ListSymbol[i].BinaryCommandList[j].OpenTime + "," +
                                                ListSymbol[i].BinaryCommandList[j].OpenPrice + "," + ListSymbol[i].BinaryCommandList[j].StopLoss + "," + ListSymbol[i].BinaryCommandList[j].TakeProfit + "," +
                                                ListSymbol[i].BinaryCommandList[j].ClosePrice + "," + ListSymbol[i].BinaryCommandList[j].Commission + "," + ListSymbol[i].BinaryCommandList[j].Swap + "," +
                                                ListSymbol[i].BinaryCommandList[j].Profit + "," + "Comment," + ListSymbol[i].BinaryCommandList[j].ID + "," + "BinaryTrading" + "," +
                                                1 + "," + ListSymbol[i].BinaryCommandList[j].ExpTime + "," + ListSymbol[i].BinaryCommandList[j].ClientCode + "," + ListSymbol[i].BinaryCommandList[j].CommandCode + "," +
                                                ListSymbol[i].BinaryCommandList[j].IsHedged + "," + ListSymbol[i].BinaryCommandList[j].Type.ID + "," + ListSymbol[i].BinaryCommandList[j].Margin + ",Binary";

                                    ListSymbol[i].BinaryCommandList[j].Investor.ClientBinaryQueue.Add(Message);
                                    #endregion                                    
                                }
                            }

                            if (ListSymbol[i].RefSymbol != null)
                            {
                                this.FillPricesStartReference(ListSymbol[i].RefSymbol);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// CALCULATION QUEUE BINARY COMMAND 
        /// </summary>
        private void CalQueueBinaryCommandClose()
        {
            if (Business.Market.SymbolList == null || Business.Market.SymbolList.Count < 0)
                return;

            int count = Business.Market.SymbolList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Business.Market.SymbolList[i].MarketAreaRef.IMarketAreaName == "BinaryCommand")
                {
                    if (Business.Market.SymbolList[i].BinaryCommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].BinaryCommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            Business.Tick PriceStop = new Tick();
                            if (Business.BinaryCommand.PriceStopSession.ContainsKey(Business.Market.SymbolList[i].Name))
                            {
                                Business.BinaryCommand.PriceStopSession.TryGetValue(Business.Market.SymbolList[i].Name, out PriceStop);
                            }

                            if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 3 || Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 4)
                            {
                                //Remove Command In Symbol List                            
                                if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 3)
                                    Business.Market.SymbolList[i].BinaryCommandList[j].ClosePrice = PriceStop.Bid;

                                if (Business.Market.SymbolList[i].BinaryCommandList[j].Type.ID == 4)
                                    Business.Market.SymbolList[i].BinaryCommandList[j].ClosePrice = PriceStop.Ask;

                                Business.Market.SymbolList[i].BinaryCommandList[j].IsClose = true;
                                Business.Market.SymbolList[i].BinaryCommandList[j].CloseTime = DateTime.Now;
                                //Business.Market.SymbolList[i].BinaryCommandList[j].CalculatorProfitCommand(Business.Market.SymbolList[i].BinaryCommandList[j]);

                                Business.Market.SymbolList[i].BinaryCommandList[j].Investor.UpdateCommand(Business.Market.SymbolList[i].BinaryCommandList[j]);
                            }
                        }
                    }                    
                }
            }
        }

        /// <summary>
        /// CHECK VALID ACCOUNT.
        /// LAY TAT CA SIZE CUA LENH BINARY + SIZE CUA LENH HIEN TAI RA TONG SO TIEN
        /// LAY TONG SO TIEN + TONG SO MARGIN CUA ACCOUNT
        /// NEU TONG SO TIEN + TONG SO MARGIN CUA ACCOUNT < BALANCE + CREDIT + PROFIT CUA ACCOUNT THI OK
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        private bool CheckValidAccount(Business.OpenTrade Command)
        {
            bool Result = false;
            double totalMoney = 0;

            if (Command.Investor.BinaryCommandList != null)
            {                
                int count = Command.Investor.BinaryCommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Command.Investor.BinaryCommandList[i].Symbol.MarketAreaRef.IMarketAreaName == "BinaryCommand")
                    {
                        totalMoney += Command.Investor.BinaryCommandList[i].Size;
                    }
                }
            }

            totalMoney += Command.Size;

            double tempTotalMargin = 0;
            
            tempTotalMargin = Command.Investor.Margin;

            double totalProfit = 0;
            if (Command.Investor.CommandList != null)
            {
                int count = Command.Investor.CommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    totalProfit += Command.Investor.CommandList[i].Profit;
                }
            }
            
            if (totalMoney + tempTotalMargin  < Command.Investor.Balance + Command.Investor.Credit + totalProfit)
            {
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }
}
