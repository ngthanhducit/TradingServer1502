using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWSymbol
    {
        #region Create Instance Class DBWTradingConfig
        private static DBW.DBWTradingConfig dbwTradingConfig;
        private static DBW.DBWTradingConfig TradingConfigInstance
        {
            get
            {
                if (DBWSymbol.dbwTradingConfig == null)
                {
                    DBWSymbol.dbwTradingConfig = new DBWTradingConfig();
                }
                return DBWSymbol.dbwTradingConfig;
            }
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Symbol> GetAllSymbol()
        {
            List<Business.Symbol> Result = new List<Business.Symbol>();
            List<Business.Tick> listTickTemp = new List<Business.Tick>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();
            DSTableAdapters.TradingConfigTableAdapter adapTradingConfig = new DSTableAdapters.TradingConfigTableAdapter();
            DS.SymbolDataTable tbSymbol = new DS.SymbolDataTable();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapTradingConfig.Connection = conn;
                tbSymbol = adap.GetData();

                #region GET TICK FROM SERVER ANOTHER
                try
                {
                    string[] tickData = new string[] { };

                    tickData = Business.Market.client.ServerCommandList("GetTickOnline", "");

                    if (tickData != null)
                    {
                        int countTick = tickData.Count();

                        for (int j = 0; j < countTick; j++)
                        {
                            string message = tickData[j];
                            string[] splipTick = message.Split('▼');
                            if (splipTick.Length == 6)
                            {
                                Business.Tick newTick = new Business.Tick();
                                newTick.Bid = double.Parse(splipTick[0]);
                                newTick.TickTime = DateTime.Parse(splipTick[1]);
                                newTick.SymbolName = splipTick[2];
                                newTick.HighInDay = double.Parse(splipTick[3]);
                                newTick.LowInDay = double.Parse(splipTick[4]);
                                newTick.Status = splipTick[5];

                                listTickTemp.Add(newTick);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion                            

                if (tbSymbol != null)
                {
                    int count = tbSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (tbSymbol[i].RefSymbolID == -1)
                        {
                            Business.Symbol newSymbol = new Business.Symbol();
                            newSymbol.Name = tbSymbol[i].Name;
                            newSymbol.SymbolID = tbSymbol[i].SymbolID;
                            newSymbol.SecurityID = tbSymbol[i].SecurityID;

                            #region GET SPREAD DIFFERENCE SET TO SPREAD DIFFERENCE OF SYMBOL
                            if (Business.Market.IGroupSecurityList != null)
                            {
                                int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                for (int n = 0; n < countIGroupSecurity; n++)
                                {
                                    if (Business.Market.IGroupSecurityList[n].SecurityID == newSymbol.SecurityID)
                                    {
                                        if (Business.Market.IGroupSecurityList[n].IGroupSecurityConfig != null)
                                        {
                                            int countIGroupSecurityConfig = Business.Market.IGroupSecurityList[n].IGroupSecurityConfig.Count;
                                            for (int m = 0; m < countIGroupSecurityConfig; m++)
                                            {
                                                if (Business.Market.IGroupSecurityList[n].IGroupSecurityConfig[m].Code == "B04")
                                                {
                                                    newSymbol.SpreadDifference = double.Parse(Business.Market.IGroupSecurityList[n].IGroupSecurityConfig[m].NumValue);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            #endregion
                            
                            newSymbol.RefSymbol = this.GetSymbolReference(newSymbol.SymbolID, tbSymbol);
                            //newSymbol.ParameterItems = DBWSymbol.TradingConfigInstance.GetParameterItemBySymbolID(tbSymbol[i].SymbolID);

                            tbTradingConfig = adapTradingConfig.GetTradingConfigBySymbolID(tbSymbol[i].SymbolID);

                            if (tbTradingConfig != null)
                            {
                                int countTradingConfig = tbTradingConfig.Count;
                                for (int j = 0; j < countTradingConfig; j++)
                                {
                                    Business.ParameterItem newParameter = new Business.ParameterItem();
                                    newParameter.ParameterItemID = tbTradingConfig[j].TradingConfigID;
                                    newParameter.BoolValue = tbTradingConfig[j].BoolValue;
                                    newParameter.Code = tbTradingConfig[j].Code;
                                    newParameter.DateValue = tbTradingConfig[j].DateValue;
                                    newParameter.NumValue = tbTradingConfig[j].NumValue;
                                    newParameter.SecondParameterID = tbTradingConfig[j].SymbolID;
                                    newParameter.StringValue = tbTradingConfig[j].StringValue;
                                    newParameter.Name = tbTradingConfig[j].Name;

                                    if (newSymbol.ParameterItems == null)
                                        newSymbol.ParameterItems = new List<Business.ParameterItem>();

                                    newSymbol.ParameterItems.Add(newParameter);

                                    #region GET DIGIT SET TO DIGIT OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S003")
                                    {
                                        int Digit = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out Digit);
                                        newSymbol.Digit = Digit;
                                    }
                                    #endregion

                                    #region GET CURRENCY SET TO CURRENCY OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S007")
                                    {
                                        newSymbol.Currency = tbTradingConfig[j].StringValue;
                                    }
                                    #endregion

                                    #region GET ISTRADE SET TO TRADE OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S008")
                                    {
                                        newSymbol.Trade = tbTradingConfig[j].StringValue;
                                    }
                                    #endregion

                                    #region GET SPREAD BY DEFAULT SET TO SPREAD BY DEFAULT OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S013")
                                    {
                                        double SpreadByDefault = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out SpreadByDefault);
                                        newSymbol.SpreadByDefault = SpreadByDefault;
                                    }
                                    #endregion

                                    #region GET LONG ONLY SET TO LONG ONLY OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S014")
                                    {
                                        if (tbTradingConfig[j].BoolValue == 0)
                                        {
                                            newSymbol.LongOnly = false;
                                        }
                                        else
                                        {
                                            newSymbol.LongOnly = true;
                                        }
                                    }
                                    #endregion

                                    #region GET LIMIT STOP LEVEL SET TO LIMIT STOP LEVEL OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S015")
                                    {
                                        int LimitLevel = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out LimitLevel);
                                        newSymbol.LimitLevel = LimitLevel;
                                    }
                                    if (tbTradingConfig[j].Code == "S046")
                                    {
                                        int StopLevel = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out StopLevel);
                                        newSymbol.StopLevel = StopLevel;
                                    }
                                    if (tbTradingConfig[j].Code == "S047")
                                    {
                                        int SLTP = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out SLTP);
                                        newSymbol.StopLossTakeProfitLevel = SLTP;
                                    }
                                    #endregion

                                    #region GET SPREAD BALANCE SET TO SPREAD BALANCE OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S016")
                                    {
                                        double SpreadBalace = 0;
                                        if (tbTradingConfig[j].NumValue != "NaN")
                                        {
                                            double.TryParse(tbTradingConfig[j].NumValue, out SpreadBalace);
                                        }

                                        newSymbol.SpreadBalace = SpreadBalace;
                                    }
                                    #endregion

                                    #region GET FREEZE LEVEL SET TO FREEZE LEVEL TO SYMBOL
                                    if (tbTradingConfig[j].Code == "S017")
                                    {
                                        int FreezeLevel = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out FreezeLevel);
                                        newSymbol.FreezeLevel = FreezeLevel;
                                    }
                                    #endregion

                                    #region GET ALLOW READ TIME SET TO ALLOW READ TIME OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S018")
                                    {
                                        if (tbTradingConfig[j].BoolValue == 0)
                                        {
                                            newSymbol.AllowReadTime = false;
                                        }
                                        else
                                        {
                                            newSymbol.AllowReadTime = true;
                                        }
                                    }
                                    #endregion

                                    #region GET FILTTRATIONS LEVEL SET TO FILTRATIONS OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S020")
                                    {
                                        int FiltrationLevel = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out FiltrationLevel);
                                        newSymbol.FiltrationsLevel = FiltrationLevel;
                                    }
                                    #endregion

                                    #region GET AUTO LIMIT SET TO AUTO LIMIT OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S021")
                                    {
                                        newSymbol.AutoLimit = tbTradingConfig[j].StringValue;
                                    }
                                    #endregion

                                    #region GET FILTER SET TO FILTER OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S022")
                                    {
                                        int Filter = 0;
                                        int.TryParse(tbTradingConfig[j].NumValue, out Filter);
                                        newSymbol.Filter = Filter;
                                    }
                                    #endregion

                                    #region GET CONTRACT SIZE SET TO CONTRACT SIZE OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S025")
                                    {
                                        double ContractSize = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out ContractSize);
                                        newSymbol.ContractSize = ContractSize;
                                    }
                                    #endregion

                                    #region GET TICK SIZE SET TO TICK SIZE OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S029")
                                    {
                                        double TickSize = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out TickSize);
                                        newSymbol.TickSize = TickSize;
                                    }
                                    #endregion

                                    #region GET TICK PRICE SET TO TICK PRICE OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S030")
                                    {
                                        double TickPrice = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out TickPrice);
                                        newSymbol.TickPrice = TickPrice;
                                    }
                                    #endregion

                                    #region GET PROFIT CALCULATION SET TO PROFIT CALCULATION OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S033")
                                    {
                                        newSymbol.ProfitCalculation = tbTradingConfig[j].StringValue;
                                    }
                                    #endregion

                                    #region GET ISHEDGED SET TO ISHEDGED OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S034")
                                    {
                                        if (tbTradingConfig[j].BoolValue == 0)
                                        {
                                            newSymbol.IsHedged = false;
                                        }
                                        else
                                        {
                                            newSymbol.IsHedged = true;
                                        }
                                    }
                                    #endregion     
                              
                                    #region GET TIME CLOSE ONLY OF SYMBOL FUTURE
                                    if (tbTradingConfig[j].Code == "S044")
                                    {
                                        newSymbol.TimeCloseOnly = tbTradingConfig[j].DateValue;
                                    }
                                    #endregion

                                    #region GET TIME EXP OF SYMBOL FUTURE
                                    if (tbTradingConfig[j].Code == "S045")
                                    {
                                        newSymbol.TimeExp = tbTradingConfig[j].DateValue;
                                    }
                                    #endregion

                                    #region GET APPLY SPREAD OF SYMBOL
                                    if (tbTradingConfig[j].Code == "S050")
                                    {
                                        if (tbTradingConfig[j].BoolValue == 1)
                                        {
                                            newSymbol.ApplySpread = true;
                                        }
                                    }
                                    #endregion

                                    #region GET FREEZE MARGIN
                                    if (tbTradingConfig[j].Code == "S051")
                                    {
                                        bool useFreezeLevel = false;
                                        if (tbTradingConfig[j].BoolValue == 1)
                                            useFreezeLevel = true;

                                        newSymbol.IsEnableFreezeMargin = useFreezeLevel;
                                    }
                                    #endregion

                                    #region GET USE FREEZE MARGIN
                                    if (tbTradingConfig[j].Code == "S052")
                                    {
                                        double freezeMargin = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue.ToString(), out freezeMargin);
                                        newSymbol.FreezeMargin = freezeMargin;
                                    }
                                    #endregion

                                    #region GET FREEZE MARGIN HEDGED
                                    if (tbTradingConfig[j].Code == "S053")
                                    {
                                        double freezeMarginH = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out freezeMarginH);
                                        newSymbol.FreezeMarginHedged = freezeMarginH;
                                    }
                                    #endregion

                                    #region GET MARGIN HEDGED
                                    if (tbTradingConfig[j].Code == "S028")
                                    {
                                        double marginH = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out marginH);
                                        newSymbol.MarginHedged = marginH;
                                    }
                                    #endregion

                                    #region GET INIT MARGIN
                                    if (tbTradingConfig[j].Code == "S026")
                                    {
                                        double initMargin = 0;
                                        double.TryParse(tbTradingConfig[j].NumValue, out initMargin);
                                        newSymbol.InitialMargin = initMargin;
                                    }
                                    #endregion
                                }
                            }

                            bool flagTick = false;
                            if (listTickTemp != null)
                            {   
                                int countTick = listTickTemp.Count;
                                for (int j = 0; j < countTick; j++)
                                {
                                    if (listTickTemp[j].SymbolName == tbSymbol[i].Name)
                                    {
                                        if (newSymbol.TickValue == null)
                                            newSymbol.TickValue = new Business.Tick();

                                        //newSymbol.TickValue.Ask = Math.Round(newSymbol.CreateAskPrices(listTickTemp[j].Bid, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                        
                                        //Apply Spread Change Function CreateAskPrices
                                        newSymbol.TickValue.Ask = Math.Round(newSymbol.CreateAskPrices(newSymbol.Digit,int.Parse(newSymbol.SpreadDifference.ToString()),listTickTemp[j].Ask),newSymbol.Digit);

                                        newSymbol.TickValue.Bid = listTickTemp[j].Bid;                                        
                                        newSymbol.TickValue.Status = listTickTemp[j].Status;
                                        newSymbol.TickValue.SymbolID = newSymbol.SymbolID;
                                        newSymbol.TickValue.SymbolName = newSymbol.Name;
                                        newSymbol.TickValue.TickTime = listTickTemp[j].TickTime;
                                        newSymbol.TickValue.TimeCurrent = listTickTemp[j].TickTime;
                                        newSymbol.TickValue.HighInDay = listTickTemp[j].HighInDay;
                                        newSymbol.TickValue.LowInDay = listTickTemp[j].LowInDay;

                                        newSymbol.ArchiveTick(newSymbol.TickValue);

                                        #region BUILD HIGH LOW
                                        if (newSymbol.TickCurrent == null)
                                            newSymbol.TickCurrent = new Business.TickLog();

                                        newSymbol.TickCurrent.Close = listTickTemp[j].Bid;
                                        newSymbol.TickCurrent.Date = listTickTemp[j].TickTime;
                                        newSymbol.TickCurrent.HighBid = listTickTemp[j].HighInDay;
                                                                                
                                        newSymbol.TickCurrent.LowBid = listTickTemp[j].LowInDay;
                                        newSymbol.TickCurrent.Name = listTickTemp[j].SymbolName;
                                        newSymbol.TickCurrent.Open = listTickTemp[j].Bid;

                                        //newSymbol.TickValue.HighAsk = Math.Round(newSymbol.CreateAskPrices(listTickTemp[j].HighInDay, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                        newSymbol.TickValue.HighAsk = Math.Round(newSymbol.CreateAskPrices(newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString()), listTickTemp[j].HighAsk), newSymbol.Digit);
                                        
                                        //newSymbol.TickValue.HighAsk = ResultDataLog.HighAsk;
                                        //newSymbol.TickValue.HighInDay = ResultDataLog.High;
                                        //newSymbol.TickValue.LowAsk = Math.Round(newSymbol.CreateAskPrices(listTickTemp[j].LowInDay, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                        newSymbol.TickValue.LowAsk = Math.Round(newSymbol.CreateAskPrices(newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString()), listTickTemp[j].LowAsk), newSymbol.Digit);
                                        //newSymbol.TickValue.LowAsk = ResultDataLog.LowAsk;
                                        //newSymbol.TickValue.LowInDay = ResultDataLog.Low;

                                        newSymbol.TickCurrent.HighAsk = newSymbol.TickValue.HighAsk;
                                        newSymbol.TickCurrent.LowAsk = newSymbol.TickValue.LowAsk;

                                        int tickID = ProcessQuoteLibrary.FacadeDataLog.AddNewDataLog(listTickTemp[j].TickTime, listTickTemp[j].Bid, listTickTemp[j].HighInDay,
                                            listTickTemp[j].LowInDay, listTickTemp[j].HighAsk, listTickTemp[j].LowAsk, listTickTemp[j].Bid, listTickTemp[j].SymbolName);

                                        newSymbol.TickCurrent.ID = tickID;
                                        newSymbol.TickValue.ID = tickID;
                                        #endregion

                                        flagTick = true;
                                        break;
                                    }
                                }
                            }

                            if (!flagTick)
                            {
                                #region GET TICK OF SYMBOL
                                ProcessQuoteLibrary.Business.BarTick resultTick = ProcessQuoteLibrary.FacadeDataLog.FacadeGetClosePriceBySymbol(newSymbol.Name);
                                if (resultTick != null && resultTick.ID > 0)
                                {
                                    if (newSymbol.TickValue == null)
                                        newSymbol.TickValue = new Business.Tick();

                                    //newSymbol.TickValue.Ask = Math.Round(newSymbol.CreateAskPrices(resultTick.Close, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                    newSymbol.TickValue.Ask = Math.Round(newSymbol.CreateAskPrices(newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString()), resultTick.Close), newSymbol.Digit);
                                    newSymbol.TickValue.Bid = resultTick.Close;
                                    newSymbol.TickValue.Status = "up";
                                    newSymbol.TickValue.SymbolID = newSymbol.SymbolID;
                                    newSymbol.TickValue.SymbolName = newSymbol.Name;
                                    newSymbol.TickValue.TickTime = resultTick.Time;
                                    newSymbol.TickValue.TimeCurrent = resultTick.Time;

                                    newSymbol.ArchiveTick(newSymbol.TickValue);
                                }
                                else
                                {
                                    if (newSymbol.TickValue == null)
                                        newSymbol.TickValue = new Business.Tick();

                                    //newSymbol.TickValue.Ask = Math.Round(newSymbol.CreateAskPrices(resultTick.Close, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                    newSymbol.TickValue.Ask = 0;
                                    newSymbol.TickValue.Bid = 0;
                                    newSymbol.TickValue.Status = "down";
                                    newSymbol.TickValue.SymbolID = newSymbol.SymbolID;
                                    newSymbol.TickValue.SymbolName = newSymbol.Name;
                                    newSymbol.TickValue.TickTime = DateTime.Now;
                                    newSymbol.TickValue.TimeCurrent = DateTime.Now;
                                }
                                #endregion

                                #region GET DATA HIGH LOW IN DAY SET TO DATA HIGH LOW OF SYMBOL
                                ProcessQuoteLibrary.Business.BarTick ResultDataLog = new ProcessQuoteLibrary.Business.BarTick();
                                ResultDataLog = ProcessQuoteLibrary.FacadeDataLog.FacadeGetDataLogByName(newSymbol.Name);
                                if (ResultDataLog.Time.Day == DateTime.Now.Day)
                                {
                                    if (ResultDataLog != null && ResultDataLog.High != 0)
                                    {
                                        if (newSymbol.TickValue == null)
                                            newSymbol.TickValue = new Business.Tick();

                                        if (newSymbol.TickCurrent == null)
                                            newSymbol.TickCurrent = new Business.TickLog();

                                        newSymbol.TickCurrent.Close = ResultDataLog.Close;
                                        newSymbol.TickCurrent.Date = ResultDataLog.Time;
                                        newSymbol.TickCurrent.HighBid = ResultDataLog.High;

                                        newSymbol.TickCurrent.ID = ResultDataLog.ID;
                                        newSymbol.TickValue.ID = ResultDataLog.ID;
                                        newSymbol.TickCurrent.LowBid = ResultDataLog.Low;
                                        newSymbol.TickCurrent.Name = ResultDataLog.Symbol;
                                        newSymbol.TickCurrent.Open = ResultDataLog.Open;

                                        //newSymbol.TickValue.HighAsk = Math.Round(newSymbol.CreateAskPrices(resultTick.High, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                        newSymbol.TickValue.HighAsk = Math.Round(newSymbol.CreateAskPrices(newSymbol.Digit,int.Parse(newSymbol.SpreadDifference.ToString()),resultTick.High));
                                        //newSymbol.TickValue.HighAsk = ResultDataLog.HighAsk;
                                        newSymbol.TickValue.HighInDay = ResultDataLog.High;
                                        //newSymbol.TickValue.LowAsk = Math.Round(newSymbol.CreateAskPrices(resultTick.Low, newSymbol.SpreadByDefault, newSymbol.Digit, int.Parse(newSymbol.SpreadDifference.ToString())), newSymbol.Digit);
                                        newSymbol.TickValue.LowAsk = Math.Round(newSymbol.CreateAskPrices(newSymbol.Digit,int.Parse(newSymbol.SpreadDifference.ToString()),resultTick.Low),newSymbol.Digit);
                                        //newSymbol.TickValue.LowAsk = ResultDataLog.LowAsk;
                                        newSymbol.TickValue.LowInDay = ResultDataLog.Low;

                                        newSymbol.TickCurrent.HighAsk = ResultDataLog.HighAsk;
                                        newSymbol.TickCurrent.LowAsk = ResultDataLog.LowAsk;
                                    }
                                }
                                #endregion
                            }

                            #region FIND IMARKET AREA AND ADD TO SYMBOL
                            //Find IMarketArea 
                            if (Business.Market.MarketArea != null)
                            {
                                int countMarketArea = Business.Market.MarketArea.Count;
                                for (int j = 0; j < countMarketArea; j++)
                                {
                                    if (Business.Market.MarketArea[j].IMarketAreaID == tbSymbol[i].MarketAreaID)
                                    {
                                        newSymbol.MarketAreaRef = Business.Market.MarketArea[j];

                                        //Add Symbol To MarketArea
                                        if (Business.Market.MarketArea[j].ListSymbol == null)
                                            Business.Market.MarketArea[j].ListSymbol = new List<Business.Symbol>();

                                        Business.Market.MarketArea[j].ListSymbol.Add(newSymbol);

                                        break;
                                    }
                                }
                            }
                            #endregion                            

                            Result.Add(newSymbol);
                        }
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
                adapTradingConfig.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        internal List<Business.Symbol> GetSymbolReference(int SymbolID,DS.SymbolDataTable objSymbol)
        {
            List<Business.Symbol> Result = new List<Business.Symbol>();
            int count = objSymbol.Count;
            for (int i = 0; i < count; i++)
            {
                if (objSymbol[i].RefSymbolID > -1)
                {
                    if (objSymbol[i].RefSymbolID == SymbolID)
                    {
                        Business.Symbol newSymbol = new Business.Symbol();
                        newSymbol.Name = objSymbol[i].Name;
                        newSymbol.SymbolID = objSymbol[i].SymbolID;
                        newSymbol.SecurityID = objSymbol[i].SecurityID;
                        newSymbol.RefSymbol = this.GetSymbolReference(newSymbol.SymbolID, objSymbol);
                        newSymbol.ParameterItems = DBWSymbol.TradingConfigInstance.GetParameterItemBySymbolID(objSymbol[i].SymbolID);

                        //Find IMarketArea 
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarketArea = Business.Market.MarketArea.Count;
                            for (int j = 0; j < count; j++)
                            {
                                if (Business.Market.MarketArea[j].IMarketAreaID == objSymbol[i].MarketAreaID)
                                {
                                    newSymbol.MarketAreaRef = Business.Market.MarketArea[j];

                                    //Add Symbol To MarketArea
                                    if (Business.Market.MarketArea[j].ListSymbol == null)
                                        Business.Market.MarketArea[j].ListSymbol = new List<Business.Symbol>();

                                    Business.Market.MarketArea[j].ListSymbol.Add(newSymbol);

                                    break;
                                }
                            }
                        }

                        Result.Add(newSymbol);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Get Symbol By Name
        /// </summary>
        /// <param name="Name">string Name</param>
        /// <returns>List<Business.Symbol></returns>
        internal Business.Symbol GetSymbolByName(string Name)
        {
            Business.Symbol Result = new Business.Symbol();
            System.Data.SqlClient.SqlConnection SqlConnection = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();
            DS.SymbolDataTable tbSymbol = new DS.SymbolDataTable();            

            try
            {
                if (SqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    SqlConnection.Open();
                }
                adap.Connection = SqlConnection;

                tbSymbol= adap.GetSymbolByName(Name);
                if (tbSymbol != null)
                {   
                    Result.SymbolID = tbSymbol[0].SymbolID;
                    Result.Name = tbSymbol[0].Name;
                    Result.RefSymbol = this.GetSymbolByRefSymbolID(tbSymbol[0].SymbolID);
                    Result.ParameterItems = DBWSymbol.TradingConfigInstance.GetParameterItemBySymbolID(tbSymbol[0].SymbolID);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                SqlConnection.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal List<Business.Symbol> GetSymbolBySecurityID(int SecurityID)
        {
            List<Business.Symbol> Result = new List<Business.Symbol>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();
            DSTableAdapters.TradingConfigTableAdapter adapTradingConfig = new DSTableAdapters.TradingConfigTableAdapter();
            DS.SymbolDataTable tbSymbol = new DS.SymbolDataTable();
            DS.TradingConfigDataTable tbTradingConfig = new DS.TradingConfigDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapTradingConfig.Connection = conn;

                tbSymbol= adap.GetSymbolBySecurityID(SecurityID);

                if (tbSymbol != null)
                {
                    int count = tbSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Symbol newSymbol = new Business.Symbol();
                        if (Business.Market.MarketArea != null)
                        {
                            int countMarket = Business.Market.MarketArea.Count;
                            for (int j = 0; j < countMarket; j++)
                            {
                                if (Business.Market.MarketArea[j].IMarketAreaID == tbSymbol[i].MarketAreaID)
                                {
                                    newSymbol.MarketAreaRef = Business.Market.MarketArea[j];
                                    break;
                                }
                            }
                        }
                        
                        newSymbol.Name = tbSymbol[i].Name;
                        newSymbol.SecurityID = tbSymbol[i].SecurityID;
                        newSymbol.SymbolID = tbSymbol[i].SymbolID;
                        newSymbol.RefSymbol = this.GetSymbolByRefSymbolID(tbSymbol[i].SymbolID);

                        tbTradingConfig = adapTradingConfig.GetTradingConfigBySymbolID(tbSymbol[i].SymbolID);
                        if (tbTradingConfig != null)
                        {
                            int countTradingConfig = tbTradingConfig.Count;
                            for (int j = 0; j < countTradingConfig; j++)
                            {
                                Business.ParameterItem newParameter = new Business.ParameterItem();
                                newParameter.BoolValue = tbTradingConfig[j].BoolValue;
                                newParameter.Code = tbTradingConfig[j].Code;
                                newParameter.DateValue = tbTradingConfig[j].DateValue;
                                newParameter.NumValue = tbTradingConfig[j].NumValue;
                                newParameter.ParameterItemID = tbTradingConfig[j].TradingConfigID;
                                newParameter.SecondParameterID = tbTradingConfig[j].SymbolID;

                                if (newSymbol.ParameterItems == null)
                                    newSymbol.ParameterItems = new List<Business.ParameterItem>();

                                newSymbol.ParameterItems.Add(newParameter);
                            }
                        }

                        //newSymbol.ParameterItems = DBWSymbol.TradingConfigInstance.GetParameterItemBySymbolID(tbSymbol[i].SymbolID);

                        Result.Add(newSymbol);
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
                adapTradingConfig.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// Get Symbol By Ref Symbol ID
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>List<Business.Symbol></returns>
        internal List<Business.Symbol> GetSymbolByRefSymbolID(int ID)
        {            
            List<Business.Symbol> listSymbol = new List<Business.Symbol>();
            
            System.Data.SqlClient.SqlConnection SqlConnection = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();
            DS.SymbolDataTable tbSymbol = new DS.SymbolDataTable();

            try
            {
                if (SqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    SqlConnection.Open();
                }
                adap.Connection = SqlConnection;
                tbSymbol = adap.GetSymbolByRefSymbolID(ID);

                if (tbSymbol != null)
                {
                    int count = tbSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Symbol newSymbol = new Business.Symbol();
                        newSymbol.Name = tbSymbol[i].Name;
                        newSymbol.SymbolID = tbSymbol[i].SymbolID;
                        newSymbol.RefSymbol = this.GetSymbolByRefSymbolID(newSymbol.SymbolID);

                        listSymbol.Add(newSymbol);
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
                SqlConnection.Close();
            }

            return listSymbol;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="MarketAreaID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal int AddNewSymbol(int SecurityID, int RefSymbolID, int MarketAreaID, string Name)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (RefSymbolID != -1)
                {
                    Result = int.Parse((adap.AddNewSymbol(SecurityID, RefSymbolID, MarketAreaID, Name)).ToString());
                }
                else
                {
                    Result = int.Parse((adap.AddNewSymbol(SecurityID, null, MarketAreaID, Name)).ToString());
                }
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
        /// <param name="SymbolID"></param>
        internal bool DeleteSymbol(int SymbolID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();
            try
            {
                conn.Open();
                adap.Connection = conn;

                adap.DeleteSymbol(SymbolID);
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

            return Result;
        }

        /// <summary>
        /// delete function
        /// delete symbol by symbol id
        /// if exist TradingConfig, delete trading config
        /// </summary>
        /// <param name="symbolID">symbol id</param>
        /// <returns></returns>
        internal bool DFDeleteSymbol(int symbolID)
        {
            bool result = false;
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            SqlTransaction tran;
            connection.Open();
            tran = connection.BeginTransaction();
            try {
                
                
                result = this.DFDeleteSymbol(symbolID, connection,tran);
                if (result)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                tran.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        /// <summary>
        /// delete function
        /// delete symbol by symbol id
        /// if exist TradingConfig, delete trading config
        /// </summary>
        /// <param name="symbolID">symbol id</param>
        /// <param name="sqlConnect">sql connection</param>
        /// <returns></returns>
        internal bool DFDeleteSymbol(int symbolID, SqlConnection sqlConnect,SqlTransaction trans)
        {
            //DBWTradingConfig tradingConfig = new DBWTradingConfig();
            //tradingConfig.DFDeleteBySymbolID(symbolID, sqlConnect,trans);
            DBWIGroupSymbol iGroupSymbol = new DBWIGroupSymbol();
            iGroupSymbol.DFDeleteBySymbolID(sqlConnect, trans, symbolID);

            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();
            adap.Connection = sqlConnect;
            adap.Transaction = trans;
            int effectRow= adap.UpdateIsDeleteSymbol(symbolID);
            
            if (effectRow == 0)
            { return false; }
            else
            { return true; }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="MarketAreaID"></param>
        /// <param name="Name"></param>
        internal bool UpdateSymbol(int SymbolID, int SecurityID, int RefSymbolID, int MarketAreaID, string Name)
        {
            bool Result = true;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (RefSymbolID <= -1)
                {
                    adap.UpdateSymbol(SecurityID, null, MarketAreaID, Name, SymbolID);
                }
                else
                {
                    adap.UpdateSymbol(SecurityID, RefSymbolID, MarketAreaID, Name, SymbolID);
                }
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
        /// <returns></returns>
        internal int CountSymbol()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.SymbolTableAdapter adap = new DSTableAdapters.SymbolTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountSymbolList();
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
