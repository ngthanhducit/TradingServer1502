﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Symbol
    {
        #region Create Instance Class DBWTradingConfig
        private static TradingServer.DBW.DBWTradingConfig dbwTradingConfig;
        private static TradingServer.DBW.DBWTradingConfig TradingConfigInstance
        {
            get
            {
                if (Symbol.dbwTradingConfig == null)
                {
                    Symbol.dbwTradingConfig = new DBW.DBWTradingConfig();
                }
                return Symbol.dbwTradingConfig;
            }
        }
        #endregion

        #region Create Instance Class DBWSymbol
        private static TradingServer.DBW.DBWSymbol dbwSymbol;
        private static TradingServer.DBW.DBWSymbol DBWSymbolInstance
        {
            get
            {
                if (Symbol.dbwSymbol == null)
                {
                    Symbol.dbwSymbol = new DBW.DBWSymbol();
                }

                return Symbol.dbwSymbol;
            }
        }
        #endregion

        #region Create Instance Class ProcessQuotes
        //static ProcessQuoteLibrary.Business.QuoteProcess newQuoteProcess = new ProcessQuoteLibrary.Business.QuoteProcess();

        private ProcessQuoteLibrary.Business.QuoteProcess quoteProcess;
        private ProcessQuoteLibrary.Business.QuoteProcess QuotesProcess
        {
            get
            {
                if (this.quoteProcess == null)
                {
                    this.quoteProcess = new ProcessQuoteLibrary.Business.QuoteProcess();
                }

                return this.quoteProcess;
            }        
        }
        #endregion

        /// <summary>
        /// Constructure Symbol
        /// </summary>
        public Symbol()
        {
            IniSymbol();
            if (this.AlertQueue == null)
                this.AlertQueue = new List<PriceAlert>();

            if (this.TickCurrent == null)
                this.TickCurrent = new TickLog();

            if (this.CommandList == null)
                this.CommandList = new List<OpenTrade>();

            //this.IsQuote = true;            
        }

        /// <summary>
        /// Function Init Class Symbol, Set Some Parameter Default
        /// </summary>
        internal void IniSymbol()
        {            
            //this.GetParameterItems();
            //this.IsProcess = false;
        }

        /// <summary>
        /// Send Tick To Process Quote Library
        /// </summary>
        internal void UpdateTick()
        {
            //TRY CATCH FIX ERROR OF FUNCTION ArchiveTick
            try
            {   
                if (!this.TickValue.IsManager)
                {
                    #region FILTER TICK FROM SDDE
                    //Call Function Filter Digit           
                    this.TickValue = this.FormatDigit(this.TickValue, this.Digit);

                    //Call Function Check Allow ReadTime Quotes From DataFeeds
                    if (this.AllowReadTime == false)
                    {
                        this.TickValue.TickTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 00);
                    }

                    if (this.ApplySpread)
                    {
                        //Call Function Spread by Default
                        this.TickValue = this.SpreadPrices(this.TickValue, this.SpreadByDefault, this.SpreadBalace, this.Digit, this.ApplySpread);
                    }

                    this.TickValue = this.CalculationTickSize(this.TickValue, this.SpreadByDefault, this.TickSize, this.Digit,this.ApplySpread);

                    bool ResultFilter = false;
                    //Call Function Automatic Limit
                    bool ResultParse = false;
                    double AutoLimit = 0;

                    ResultParse = double.TryParse(this.AutoLimit, out AutoLimit);
                    if (ResultParse == true)
                        ResultFilter = this.AutomaticLimit(this.TickValue, AutoLimit);
                    else
                        ResultFilter = true;

                    if (ResultFilter == false)
                    {
                        //TradingServer.Facade.FacadeAddNewSystemLog(1, "Data feed : " + TickValue.SymbolName + " error automatic limit", "[automatic limit]", "", "");
                        TradingServer.Facade.FacadeAddNewTickLog(1, "Data feed : " + TickValue.SymbolName + " error automatic limit", "[automatic limit]", "", "");
                        return;
                    }

                    //Call Function Filter Level
                    ResultFilter = this.FiltrationLevel(this.TickValue, this.FiltrationsLevel, this.Filter);
                    if (ResultFilter == false)
                    {
                        //TradingServer.Facade.FacadeAddNewSystemLog(1, "Data feed : " + TickValue.SymbolName + " error filtration level", "[filtration level]", "", "");
                        TradingServer.Facade.FacadeAddNewTickLog(1, "Data feed : " + TickValue.SymbolName + "error filtration level", "[filtration level]", "", "");
                        return; 
                    }
                    #endregion                    
                }

                DateTime timeStart = DateTime.Now;

                //Call Function Filter Digit           
                this.TickValue = this.FormatDigit(this.TickValue, this.Digit);

                //Call Function Process Tick Value
                ProcessQuoteLibrary.Business.QuoteProcess.ApplyQuote(1, this.TickValue.Status, this.TickValue.Bid.ToString(), this.TickValue.Ask.ToString(),
                                                                           this.TickValue.SymbolName, this.TickValue.TickTime);

                //Call Function Archive Tick            
                //this.ArchiveTick(this.TickValue);   //Change position ( 19/07/2011)

                //Call Function Update Tick
                //if (!Business.Market.IsConnectMT4)
                //    this.MarketAreaRef.SetTickValueNotify(this.TickValue, this);

                //DateTime Time = new DateTime(this.TickValue.TickTime.Ticks);

                #region CHECK HIGH LOW BID
                if (this.TickValue.TimeCurrent.Day != this.TickValue.TickTime.Day)
                {
                    this.TickValue.TimeCurrent = this.TickValue.TickTime;
                    this.TickValue.HighInDay = this.TickValue.Bid;
                    this.TickValue.LowInDay = this.TickValue.Bid;
                    this.TickValue.HighAsk = this.TickValue.Ask;
                    this.TickValue.LowAsk = this.TickValue.Ask;

                    int Result = ProcessQuoteLibrary.FacadeDataLog.AddNewDataLog(this.TickValue.TickTime, this.TickValue.Bid, this.TickValue.Bid, this.TickValue.Bid,
                        this.TickValue.Ask, this.TickValue.Ask, this.TickValue.Bid, this.TickValue.SymbolName);

                    this.TickValue.ID = Result;
                }
                else
                {
                    #region CHECK HIGH LOW BID
                    if (this.TickValue.HighInDay == 0 || this.TickValue.LowInDay == 0 ||
                        this.TickValue.HighAsk == 0 || this.TickValue.LowAsk == 0)
                    {
                        this.TickValue.HighInDay = this.TickValue.Bid;
                        this.TickValue.LowInDay = this.TickValue.Bid;
                        this.TickValue.HighAsk = this.TickValue.Ask;
                        this.TickValue.LowAsk = this.TickValue.Ask;
                        this.TickValue.TimeCurrent = this.TickValue.TickTime;

                        int Result = ProcessQuoteLibrary.FacadeDataLog.AddNewDataLog(this.TickValue.TickTime, this.TickValue.Bid, this.TickValue.Bid, this.TickValue.Bid,
                        this.TickValue.Ask, this.TickValue.Ask, this.TickValue.Bid, this.TickValue.SymbolName);

                        this.TickValue.ID = Result;
                    }
                    else
                    {
                        if (this.TickValue.HighInDay < this.TickValue.Bid)
                        {
                            this.TickValue.HighInDay = this.TickValue.Bid;
                            this.TickValue.TimeCurrent = this.TickValue.TickTime;

                            ProcessQuoteLibrary.FacadeDataLog.UpdateHighDataLog(this.TickValue.HighInDay, this.TickValue.ID);
                        }

                        if (this.TickValue.LowInDay > this.TickValue.Bid)
                        {
                            this.TickValue.LowInDay = this.TickValue.Bid;
                            this.TickValue.TimeCurrent = this.TickValue.TickTime;

                            ProcessQuoteLibrary.FacadeDataLog.UpdateLowDataLog(this.TickValue.LowInDay, this.TickValue.ID);
                        }

                        if (this.TickValue.HighAsk < this.TickValue.Ask)
                        {
                            this.TickValue.HighAsk = this.TickValue.Ask;
                            this.TickValue.TimeCurrent = this.TickValue.TickTime;

                            ProcessQuoteLibrary.FacadeDataLog.UpdateHighAskDataLog(this.TickValue.HighAsk, this.TickValue.ID);
                        }

                        if (this.TickValue.LowAsk > this.TickValue.Ask)
                        {
                            this.TickValue.LowAsk = this.TickValue.Ask;
                            this.TickValue.TimeCurrent = this.TickValue.TickTime;

                            ProcessQuoteLibrary.FacadeDataLog.UpdateLowAskDataLog(this.TickValue.HighAsk, this.TickValue.ID);
                        }
                    }
                    #endregion
                }
                #endregion                              

                //ADD TICK TO INVESTOR ONLINE(22/07/2011)
                this.AddTickToInvestorOnline(this.TickValue);

                this.AddTickToManager(this.TickValue);

                //Business.Market.strDetect = this.TickValue.SymbolName + " - " + this.TickValue.Bid + " - " + this.TickValue.Ask;

                //SEND TICK TO AGENT SERVER
                #region SEND TICK TO AGENT SERVER(COMMENT BECAUSE CLOSE AGENT)
                //string cmd = "SendTick$" + this.TickValue.Bid + "{" + this.TickValue.Ask + "{" + this.TickValue.HighAsk + "{" +
                //                    this.TickValue.HighInDay + "{" + this.TickValue.LowAsk + "{" + this.TickValue.LowInDay + "{" +
                //                    this.TickValue.Status + "{" + this.TickValue.SymbolName + "{" + this.TickValue.TickTime;

                //Business.Market.ListTickQueueAgent.Add(cmd);
                #endregion

                DateTime timeEnd = DateTime.Now;

                TimeSpan timeProcess = timeEnd - timeStart;
                if (this.isMonitorSymbol)
                {
                    StringBuilder monitor = new StringBuilder();
                    monitor.Append("Time Process " + timeProcess.TotalMilliseconds + " millisecond");

                    if (this.ListSymbolMonitor == null)
                        this.ListSymbolMonitor = new List<string>();

                    this.ListSymbolMonitor.Insert(0, monitor.ToString());

                    if (this.ListSymbolMonitor.Count > 10)
                    {
                        this.ListSymbolMonitor.RemoveAt(10);
                    }
                }
            }
            catch (Exception ex)
            {
                //string stringConnection = "Data Source=192.168.1.202;Initial Catalog=SyTrading;User ID=SyTrading;Password=-Dlog21P-";
                //System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(stringConnection);
                //conn.Open();
                //string Command = "insert into ApplicationError(Name,Description,[DateTime]) VALUES(" +
                //    "'Class Symbol'" + "," + "'" + ex.ToString() + "'" + "," + "'" + DateTime.Now + "'" + ")";

                //System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(Command, conn);
                //command.ExecuteNonQuery();
                //conn.Close();

                //TradingServer.Model.TradingCalculate.Instance.Element5Log(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void UpdateTickMT4()
        {
            //TRY CATCH FIX ERROR OF FUNCTION ArchiveTick
            try
            {   
                //Call Function Filter Digit           
                this.TickValue = this.FormatDigit(this.TickValue, this.Digit);

                //Call Function Process Tick Value
                ProcessQuoteLibrary.Business.QuoteProcess.ApplyQuote(1, this.TickValue.Status, this.TickValue.Bid.ToString(), this.TickValue.Ask.ToString(),
                                                                           this.TickValue.SymbolName, this.TickValue.TickTime);

                //ADD TICK TO INVESTOR ONLINE(22/07/2011)
                this.AddTickToInvestorOnline(this.TickValue);

                this.AddTickToManager(this.TickValue);
            }
            catch (Exception ex)
            {   
                //TradingServer.Model.TradingCalculate.Instance.Element5Log(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        internal void AddTickToInvestorOnline(Business.Tick value)
        {
            try
            {
                if (Business.Market.InvestorOnline != null)
                {
                    int count = Business.Market.InvestorOnline.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].IsOnline)
                            Business.Market.InvestorOnline[i].TickInvestor.Add(value);
                    }
                }
            }   
            catch (Exception ex)
            {
                //TradingServer.Model.TradingCalculate.Instance.Element5Log(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        internal void AddTickToManager(Business.Tick value)
        {
            TradingServer.Facade.FacadeSendManagerTick(value);
        }

        /// <summary>
        /// GET SYMBOL NAME BY SYMBOL ID IN CLASS MARKET
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal string GetSymbolNameBySymbolID(int SymbolID)
        {
            string Result = string.Empty;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].SymbolID == SymbolID)
                    {
                        Result = Business.Market.SymbolList[i].Name;
                        break;
                    }
                }
            }

            return Result;
        }

        #region Add New Symbol
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSymbol"></param>
        internal int AddNewSymbol(int SecurityID, int RefSymbolID, int MarketAreaID, string SymbolName)
        {
            int Result = -1;
            Result = Symbol.DBWSymbolInstance.AddNewSymbol(SecurityID, RefSymbolID, MarketAreaID, SymbolName);

            #region Add Symbol To Symbol List
            if (Result > 0)
            {
                if (RefSymbolID > 0)
                {
                    this.AddNewSymbolWithRef(Result, SymbolName, RefSymbolID, MarketAreaID, Market.SymbolList);
                    int count = Market.QuoteList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        this.AddNewSymbolWithRef(Result, SymbolName, RefSymbolID, MarketAreaID, Market.QuoteList[i].RefSymbol);
                    }
                }
                else
                {
                    //add to symbol list
                    Business.Symbol newSymbol = new Business.Symbol();                    
                    newSymbol.SymbolID = Result;
                    newSymbol.Name = SymbolName;

                    if (!newSymbol.IsQuote)
                        newSymbol.IsQuote = true;

                    if (!newSymbol.IsTrade)
                        newSymbol.IsTrade = true;

                    newSymbol.SecurityID = SecurityID;

                    newSymbol.TickValue = new Tick();
                    #region Find Market Area Of Symbol
                    //Find Market Area
                    if (Market.MarketArea != null)
                    {
                        int count = Market.MarketArea.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Market.MarketArea[i].IMarketAreaID == MarketAreaID)
                            {
                                newSymbol.MarketAreaRef = Market.MarketArea[i];

                                if (Business.Market.MarketArea[i].ListSymbol == null)
                                    Business.Market.MarketArea[i].ListSymbol = new List<Symbol>();

                                Business.Market.MarketArea[i].ListSymbol.Add(newSymbol);

                                break;
                            }
                        }
                    }                   
                    #endregion
                    
                    Business.QuoteSymbol newQuoteSymbol = new Business.QuoteSymbol();
                    newQuoteSymbol.Name = SymbolName;
                    newQuoteSymbol.RefSymbol = new List<Business.Symbol>();
                    newQuoteSymbol.RefSymbol.Add(newSymbol);

                    Market.SymbolList.Add(newSymbol);
                    Market.QuoteList.Add(newQuoteSymbol);
                }
            }

            return Result;
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="SymbolName"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="objSymbol"></param>
        internal void AddNewSymbolWithRef(int SymbolID, string SymbolName, int RefSymbolID,int MarketAreaID, List<Business.Symbol> objSymbol)
        {
            if (objSymbol != null)
            {
                int count = objSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (objSymbol[i].SymbolID == RefSymbolID)
                    {
                        Business.Symbol newSymbol = new Business.Symbol();
                        newSymbol.Name = SymbolName;
                        newSymbol.SymbolID = SymbolID;

                        #region Find Market Area Of Symbol
                        //Find Market Area
                        if (Market.MarketArea != null)
                        {
                            int countMarketArea = Market.MarketArea.Count;
                            for (int j = 0; j < countMarketArea; j++)
                            {
                                if (Market.MarketArea[j].IMarketAreaID == MarketAreaID)
                                {
                                    newSymbol.MarketAreaRef = Market.MarketArea[j];
                                }
                            }
                        }
                        #endregion                        

                        if (objSymbol[i].RefSymbol == null)
                            objSymbol[i].RefSymbol = new List<Business.Symbol>();

                        objSymbol[i].RefSymbol.Add(newSymbol);

                        break;
                    }
                    else
                    {
                        if (objSymbol[i].RefSymbol != null)
                        {
                            for (int j = 0; j < objSymbol[i].RefSymbol.Count; j++)
                            {
                                if (objSymbol[i].RefSymbol[j].SymbolID == RefSymbolID)
                                {
                                    Business.Symbol newSymbol = new Business.Symbol();
                                    newSymbol.Name = SymbolName;
                                    newSymbol.SymbolID = SymbolID;

                                    #region Find Market Of Symbol
                                    //Find Market Area
                                    if (Market.MarketArea != null)
                                    {
                                        int countMarketArea = Market.MarketArea.Count;
                                        for (int n = 0; n < countMarketArea; n++)
                                        {
                                            if (Market.MarketArea[n].IMarketAreaID == MarketAreaID)
                                            {
                                                newSymbol.MarketAreaRef = Market.MarketArea[n];
                                                break;
                                            }
                                        }
                                    }
                                    #endregion                                    

                                    if (objSymbol[i].RefSymbol[j].RefSymbol == null)
                                        objSymbol[i].RefSymbol[j].RefSymbol = new List<Business.Symbol>();

                                    objSymbol[i].RefSymbol[j].RefSymbol.Add(newSymbol);

                                    break;
                                }
                                else
                                {
                                    if (objSymbol[i].RefSymbol[j].RefSymbol != null)
                                    {
                                        AddNewSymbolWithRef(SymbolID, SymbolName, RefSymbolID, MarketAreaID, objSymbol[i].RefSymbol[j].RefSymbol);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion        

        #region Update Symbol In Symbol List
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
            bool Result = false;            
            
            #region Find Symbol In Market.SymbolList
            if (Market.SymbolList != null)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SymbolList[i].SymbolID == SymbolID)
                    {
                        //Update Data In Table Symbol Of Database 
                        Result = Symbol.DBWSymbolInstance.UpdateSymbol(SymbolID, SecurityID, RefSymbolID, MarketAreaID, Name);

                        if (Result == true)
                        {
                            Market.SymbolList[i].SecurityID = SecurityID;
                            //Market.SymbolList[i].MarketAreaRef.IMarketAreaID = MarketAreaID;
                            Market.SymbolList[i].Name = Name;

                            if (Business.Market.SymbolList[i].TickValue != null)
                            {
                                Business.Market.SymbolList[i].TickValue.SymbolName = Market.SymbolList[i].Name;
                            }
                        }

                        #region UPDATE QUOTE LIST SYMBOL
                        if (Business.Market.QuoteList != null)
                        {
                            int countQuote = Business.Market.QuoteList.Count;
                            for (int j = 0; j < countQuote; j++)
                            {
                                if (Business.Market.QuoteList[j].Name.Trim() == Name.Trim())
                                {
                                    Business.Market.QuoteList[j].Name = Name;

                                    break;
                                }
                            }
                        }
                        #endregion                        

                        break;

                        #region COMMENT CODE
                        //if (RefSymbolID > 0)
                        //{
                        //    Market.SymbolList.Remove(Market.SymbolList[i]);

                        //    //Call Function Add Symbol Update
                        //    this.AddSymbolUpdateToSymbolList(SymbolID, SecurityID, RefSymbolID, MarketAreaID, Name);
                        //}
                        //else
                        //{
                        //    //Update Value Symbol In SymbolList
                        //    int securityID = Business.Market.SymbolList[i].SecurityID;

                        //    if (SecurityID != securityID)
                        //    {
                        //        Business.Symbol tempSymbol = new Symbol();

                        //        #region FIND IN MARKET AREA OF SYMBOL AND COPY SYMBOL
                        //        if (Business.Market.SymbolList != null)
                        //        {
                        //            int countSecurity = Business.Market.SecurityList.Count;
                        //            for (int j = 0; j < countSecurity; j++)
                        //            {
                        //                #region REMOVE SYMBOL IN OLD MARKET ARE AND MOVE TO NEW MARKET AREA
                        //                if (Business.Market.SecurityList[j].SecurityID == securityID)
                        //                {
                        //                    int countMarketArea = Business.Market.MarketArea.Count;
                        //                    for (int n = 0; n < countMarketArea; n++)
                        //                    {
                        //                        if (Business.Market.MarketArea[n].IMarketAreaID == Business.Market.SecurityList[j].MarketAreaID)
                        //                        {
                        //                            if (Business.Market.MarketArea[n].ListSymbol != null)
                        //                            {
                        //                                int countSymbol = Business.Market.MarketArea[n].ListSymbol.Count;
                        //                                for (int m = 0; m < countSymbol; m++)
                        //                                {
                        //                                    if (Business.Market.MarketArea[n].ListSymbol[m].Name == Business.Market.SymbolList[i].Name)
                        //                                    {
                        //                                        tempSymbol = Business.Market.MarketArea[n].ListSymbol[m];
                        //                                        Business.Market.MarketArea[n].ListSymbol.RemoveAt(m);

                        //                                        break;
                        //                                    }
                        //                                }
                        //                            }

                        //                            break;
                        //                        }
                        //                    }

                        //                    break;
                        //                }
                        //                #endregion
                        //            }
                        //        }
                        //        #endregion

                        //        #region MOVE SYMBOL IN OLD MARKET ARE TO NEW MARKET AREA
                        //        if (Business.Market.SecurityList != null)
                        //        {
                        //            int countSecurity = Business.Market.SecurityList.Count;
                        //            for (int j = 0; j < countSecurity; j++)
                        //            {
                        //                if (Business.Market.SecurityList[j].SecurityID == SecurityID)
                        //                {
                        //                    int countMarketArea = Business.Market.MarketArea.Count;
                        //                    for (int n = 0; n < countMarketArea; n++)
                        //                    {
                        //                        if (Business.Market.MarketArea[n].IMarketAreaID == Business.Market.SecurityList[j].MarketAreaID)
                        //                        {
                        //                            if (Business.Market.MarketArea[n].ListSymbol == null)
                        //                                Business.Market.MarketArea[n].ListSymbol = new List<Symbol>();

                        //                            //SET NEW MARKET AREA
                        //                            tempSymbol.MarketAreaRef = Business.Market.MarketArea[n];

                        //                            Business.Market.MarketArea[n].ListSymbol.Add(tempSymbol);

                        //                            break;
                        //                        }
                        //                    }

                        //                    break;
                        //                }
                        //            }
                        //        }
                        //        #endregion

                        //        Business.Market.marketInstance.InitSymbolFuture();
                        //    }

                        //    Market.SymbolList[i].SecurityID = SecurityID;
                        //    //Market.SymbolList[i].MarketAreaRef.IMarketAreaID = MarketAreaID;
                        //    Market.SymbolList[i].Name = Name;
                        //}
                        #endregion
                        
                    }
                    //else
                    //{
                    //    //Call Function Find In Reference Symbol
                    //    this.UpdateSymbolReference(SymbolID, SecurityID, RefSymbolID, MarketAreaID, Name, Market.SymbolList[i]);
                    //}
                }
            }
            #endregion

            #region Find Symbol In Market.QuoteSymbol
            //if (Market.QuoteList != null)
            //{
            //    int count = Market.QuoteList.Count;
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (Market.QuoteList[i].RefSymbol != null)
            //        {
            //            int countRef = Market.QuoteList[i].RefSymbol.Count;
            //            for (int j = 0; j < countRef; j++)
            //            {
            //                if (Market.QuoteList[i].RefSymbol[j].SymbolID == SymbolID)
            //                {
            //                    if (RefSymbolID > 0)
            //                    {
            //                        Market.QuoteList[i].RefSymbol.Remove(Market.QuoteList[i].RefSymbol[j]);

            //                        //Call Function Add Symbol Update
            //                        this.AddSymbolUpdateToSymbolList(SymbolID, SecurityID, RefSymbolID, MarketAreaID, Name);
            //                    }
            //                    else
            //                    {
            //                        //Update Value Symbol In SymbolList
            //                        Market.QuoteList[i].RefSymbol[j].SecurityID = SecurityID;
            //                        Market.QuoteList[i].RefSymbol[j].MarketAreaRef.IMarketAreaID = MarketAreaID;
            //                        Market.QuoteList[i].RefSymbol[j].Name = Name;
            //                    }
            //                }
            //                else
            //                {
            //                    //Call Function Find In Reference Symbol
            //                    this.UpdateSymbolReference(SymbolID, SecurityID, RefSymbolID, MarketAreaID, Name, Market.QuoteList[i].RefSymbol[j]);
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion            

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="MarketAreaID"></param>
        /// <param name="SymbolName"></param>
        /// <param name="objSymbol"></param>
        internal void UpdateSymbolReference(int SymbolID, int SecurityID, int RefSymbolID, int MarketAreaID, string SymbolName, Business.Symbol objSymbol)
        {
            if (objSymbol != null)
            {
                if (objSymbol.RefSymbol != null)
                {
                    int count = objSymbol.RefSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (objSymbol.RefSymbol[i].SymbolID == SymbolID)
                        {
                            if (RefSymbolID > 0 && RefSymbolID != objSymbol.SymbolID)
                            {
                                objSymbol.RefSymbol.Remove(objSymbol.RefSymbol[i]);

                                //Call Function Add Symbol Update
                                this.AddSymbolUpdateToSymbolList(SymbolID, SecurityID, RefSymbolID, MarketAreaID, SymbolName);
                            }
                            else
                            {
                                //Update Value Symbol In SymbolList
                                objSymbol.RefSymbol[i].SecurityID = SecurityID;
                                objSymbol.RefSymbol[i].MarketAreaRef.IMarketAreaID = MarketAreaID;
                                objSymbol.RefSymbol[i].Name = SymbolName;
                            }
                        }
                        else
                        {
                            if (objSymbol.RefSymbol[i].RefSymbol != null)
                            {
                                this.UpdateSymbolReference(SymbolID, SecurityID, RefSymbolID, MarketAreaID, SymbolName, objSymbol.RefSymbol[i]);
                            }
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="SecurityID"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="MarketAreaID"></param>
        /// <param name="SymbolName"></param>
        internal void AddSymbolUpdateToSymbolList(int SymbolID, int SecurityID, int RefSymbolID, int MarketAreaID, string SymbolName)
        {
            #region Add Symbol Update To Symbol List In Class Market
            if (Market.SymbolList != null)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SymbolList[i].SymbolID == RefSymbolID)
                    {
                        Business.Symbol newSymbol = new Business.Symbol();
                        newSymbol.Name = SymbolName;
                        newSymbol.SymbolID = SymbolID;
                        if (Market.SymbolList[i].RefSymbol == null)
                            Market.SymbolList[i].RefSymbol = new List<Business.Symbol>();

                        Market.SymbolList[i].RefSymbol.Add(newSymbol);

                        break;
                    }
                    else
                    {
                        //Call Function Find In Reference Symbol
                        this.AddSymbolUpdateReference(SymbolID, RefSymbolID, SymbolName, Market.SymbolList[i].RefSymbol);
                    }
                }
            }
            #endregion

            #region Add Symbol Update To QuoteList In Class Market
            if (Market.QuoteList != null)
            {
                int count = Market.QuoteList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.QuoteList[i].RefSymbol!= null)
                    {
                        int countRef = Market.QuoteList[i].RefSymbol.Count;
                        for (int j = 0; j < countRef; j++)
                        {
                            if (Market.QuoteList[i].RefSymbol[j].SymbolID == RefSymbolID)
                            {
                                Business.Symbol newSymbol = new Business.Symbol();
                                newSymbol.Name = SymbolName;
                                newSymbol.SymbolID = SymbolID;
                                if (Market.QuoteList[i].RefSymbol[j].RefSymbol == null)
                                    Market.QuoteList[i].RefSymbol[j].RefSymbol = new List<Symbol>();

                                Market.QuoteList[i].RefSymbol[j].RefSymbol.Add(newSymbol);
                                break;
                            }
                            else
                            {
                                //Call Function Find In Reference Symbol
                                this.AddSymbolUpdateReference(SymbolID, RefSymbolID, SymbolName, Market.QuoteList[i].RefSymbol[j].RefSymbol);
                            }
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <param name="RefSymbolID"></param>
        /// <param name="SymbolName"></param>
        /// <param name="ListSymbol"></param>
        internal void AddSymbolUpdateReference(int SymbolID, int RefSymbolID, string SymbolName, List<Business.Symbol> ListSymbol)
        {
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].SymbolID == RefSymbolID)
                    {
                        Business.Symbol newSymbol = new Business.Symbol();
                        newSymbol.Name = SymbolName;
                        newSymbol.SymbolID = SymbolID;
                        if (ListSymbol[i].RefSymbol == null)
                            ListSymbol[i].RefSymbol = new List<Business.Symbol>();

                        ListSymbol[i].RefSymbol.Add(newSymbol);

                        break;
                    }
                    else
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            int countRef = ListSymbol[i].RefSymbol.Count;
                            for (int j = 0; j < countRef; j++)
                            {
                                if (ListSymbol[i].RefSymbol[j].SymbolID == RefSymbolID)
                                {
                                    Business.Symbol newSymbol = new Business.Symbol();
                                    newSymbol.Name = SymbolName;
                                    newSymbol.SymbolID = SymbolID;

                                    if (ListSymbol[i].RefSymbol[j].RefSymbol == null)
                                        ListSymbol[i].RefSymbol[j].RefSymbol = new List<Business.Symbol>();

                                    ListSymbol[i].RefSymbol[j].RefSymbol.Add(newSymbol);

                                    break;
                                }
                                else
                                {
                                    this.AddSymbolUpdateReference(SymbolID, RefSymbolID, SymbolName, ListSymbol[i].RefSymbol[j].RefSymbol);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal List<Business.Symbol> GetListSymbolBySecurityID(List<Business.IGroupSecurity> ListIGroupSecurity)
        {
            List<Business.Symbol> ListSymbol = new List<Symbol>();
            if (ListIGroupSecurity != null)
            {
                int count = ListIGroupSecurity.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList != null)
                    {
                        int countSymbol = Business.Market.SymbolList.Count;
                        for (int j = 0; j < countSymbol; j++)
                        {
                            if (Business.Market.SymbolList[j].SecurityID == ListIGroupSecurity[i].SecurityID)
                            {
                                ListSymbol.Add(Business.Market.SymbolList[j]);                                
                            }
                        }
                    }
                }
            }

            return ListSymbol;
        }

        /// <summary>   
        /// 
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        internal Business.Symbol GetSymbolConfig(string symbolName)
        {
            Business.Symbol result = new Symbol();
            int count = Business.Market.SymbolList.Count;
            for (int i = 0; i < count; i++)
            {
                if (Business.Market.SymbolList[i].Name == symbolName)
                {
                    result = Business.Market.SymbolList[i];

                    break;
                }
            }

            return result;
        }
    }
}
