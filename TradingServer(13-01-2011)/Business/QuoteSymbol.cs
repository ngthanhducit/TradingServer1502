using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class QuoteSymbol
    {
        private static int count=0;

        #region Create Instance Classs DBWSymbol
        private DBW.DBWSymbol dbwSymbol;
        private DBW.DBWSymbol DBWSymbol
        {
            get
            {
                if (this.dbwSymbol == null)
                {
                    this.dbwSymbol = new DBW.DBWSymbol();
                }
                return this.dbwSymbol;
            }
        }
        #endregion

        #region Create Instance Class Symbol
        private Business.Symbol symbol;
        private Business.Symbol Symbol
        {
            get
            {
                if (this.symbol == null)
                {
                    this.symbol = new Symbol();
                }

                return this.symbol;
            }
        }
        #endregion

        internal double bid, ask;
        internal DateTime getTickTime;
        internal string status;

        /// <summary>
        /// Constructure Quote Symbol
        /// </summary>
        public QuoteSymbol()
        {
            this.IsUpdated = false;

            if (this.Ticks == null)
                this.Ticks = new List<Tick>();
        }
        
        #region Private Function
        /// <summary>
        /// Update Tick Value 
        /// </summary>
        /// <param name="objQuoteSymbol">Business.QuoteSymbol</param>
        public void Update(Business.Tick Tick)
        {
            if (!Tick.IsManager)
            {
                if (this.ask == Tick.Ask && this.bid == Tick.Bid)
                    return;
            }

            this.ask = Tick.Ask;
            this.bid = Tick.Bid;
            this.getTickTime = Tick.TickTime;
            this.status = Tick.Status;

            this.TaskWork = this.SetTick;
            this.Comment = "Command Add Tick " + Tick.SymbolName;
            this.TaskName = "Add tick " + Tick.SymbolName;

            if (this.IsActive == true)
            {
                if (!this.IsUpdated)
                {
                    this.IsActive = false;
                    this.Ticks.Add(Tick);
                    Business.CalculatorFacade.SetTask(this);
                }
                else
                {
                    this.Ticks.Add(Tick);
                }                
            }
            else
            {
                this.Ticks.Add(Tick);
                Business.CalculatorFacade.SetTask(this);
            }
            
            Business.Market.IsTickUpdate = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetTick()
        {
            this.IsUpdated = true;

            #region process Tick
            while (this.Ticks.Count > 0)
            {
                Business.Tick Tick = this.Ticks[0];

                int count = this.RefSymbol.Count;
                for (int i = 0; i < count; i++)
                {   
                    if (this.RefSymbol[i].IsHoliday)
                        break;

                    if (!this.RefSymbol[i].IsQuote)
                        break;

                    if (Tick == null)
                    {
                        //TradingServer.Facade.FacadeAddNewSystemLog(1, "Data feed : " + Tick.SymbolName + "error invalid tick", "[invalid tick]", "", "");
                        break;
                    }

                    //Update Value Tick
                    //
                    if (this.RefSymbol[i].TickValue == null)
                        this.RefSymbol[i].TickValue = new Business.Tick();

                    if (this.RefSymbol[i].TickCurrent == null)
                        this.RefSymbol[i].TickCurrent = new TickLog();

                    if (this.RefSymbol[i].TickValue.TimeCurrent == null)
                        this.RefSymbol[i].TickValue.TimeCurrent = Tick.TickTime;

                    if (Tick.IsManager)
                    {
                        #region ADD TICK FROM MANAGER(COMMENT 19/12/2013 BECAUSE USING TICK FROM MT4, DON'T NEED CHECK TIME AND STATUS TICK) -> VIP
                        //SET TIME IF TICK SHORT FROM MANAGER
                        //Tick.TickTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 00);

                        ////COMPARE TICK ONLINE
                        //if (this.RefSymbol[i].TickValue.Bid < Tick.Bid)
                        //{
                        //    Tick.Status = "up";
                        //}
                        //else
                        //{
                        //    Tick.Status = "down";
                        //}

                        ////UPDATE TICK IN SYMBOL CURRENT
                        //this.RefSymbol[i].TickValue.Bid = Tick.Bid;
                        //this.RefSymbol[i].TickValue.Ask = Tick.Ask;
                        //this.RefSymbol[i].TickValue.Status = Tick.Status;
                        //this.RefSymbol[i].TickValue.TickTime = Tick.TickTime;
                        //this.RefSymbol[i].TickValue.SymbolName = Tick.SymbolName;
                        //this.RefSymbol[i].TickValue.IsManager = true;

                        ////ProcessQuoteLibrary.Business.QuoteProcess.ApplyQuote(1, Tick.Status, Tick.Bid.ToString(), Tick.Ask.ToString(), Tick.SymbolName, Tick.TickTime);
                        //this.RefSymbol[i].UpdateTick();
                        #endregion  
                      
                        #region PROCESS TICK SEND FROM MT4
                        Tick.TickTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 00);

                        //UPDATE TICK IN SYMBOL CURRENT
                        this.RefSymbol[i].TickValue.Bid = Tick.Bid;
                        this.RefSymbol[i].TickValue.Ask = Tick.Ask;
                        this.RefSymbol[i].TickValue.Status = Tick.Status;
                        this.RefSymbol[i].TickValue.TickTime = Tick.TickTime;
                        this.RefSymbol[i].TickValue.HighInDay = Tick.HighInDay;
                        this.RefSymbol[i].TickValue.LowInDay = Tick.LowInDay;
                        this.RefSymbol[i].TickValue.SymbolName = Tick.SymbolName;
                        this.RefSymbol[i].TickValue.IsManager = true;

                        //ProcessQuoteLibrary.Business.QuoteProcess.ApplyQuote(1, Tick.Status, Tick.Bid.ToString(), Tick.Ask.ToString(), Tick.SymbolName, Tick.TickTime);
                        this.RefSymbol[i].UpdateTickMT4();
                        #endregion
                    }
                    else
                    {
                        #region TICK ADD FROM CLIENT
                        //FILTER BID ASK IF CHANGE THEN UPDATE
                        //if (this.RefSymbol[i].TickValue.Ask != Tick.Ask || this.RefSymbol[i].TickValue.Bid != Tick.Bid)
                        //{
                            this.RefSymbol[i].TickValue.Ask = Tick.Ask;
                            this.RefSymbol[i].TickValue.Bid = Tick.Bid;
                            this.RefSymbol[i].TickValue.Status = Tick.Status;
                            this.RefSymbol[i].TickValue.TickTime = Tick.TickTime;
                            this.RefSymbol[i].TickValue.SymbolName = Tick.SymbolName;

                            //Call Function Of Class Symbol Update Tick(Build Online Tick, Candles Tick Of All Time Frame)
                            this.RefSymbol[i].UpdateTick();

                            //Find And Update Tick In Reference Symbol
                            //if (this.RefSymbol[i].RefSymbol != null && this.RefSymbol[i].RefSymbol.Count > 0)
                            //{
                            //    this.RefSymbol[i].RefSymbol = ProcessRefSymbol(Tick, this.RefSymbol[i].RefSymbol);
                            //}
                        //}
                        #endregion                        
                    }
                }
                //End For         
                try
                {
                    this.Ticks.RemoveAt(0);              
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
                        
            this.IsUpdated = false;
        }        
        //End Function

        /// <summary>
        /// Process Update Value Of List Reference Symbol
        /// </summary>
        /// <param name="Tick">Business.Tick</param>
        /// <param name="objSymbol">List<Business.Symbol></param>
        /// <returns>List<Business.Symbol</returns>
        public List<Business.Symbol> ProcessRefSymbol(Business.Tick Tick, List<Business.Symbol> objSymbol)
        {
            if (objSymbol != null)
            {
                int count = objSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    //Check Null Tick Value
                    if (objSymbol[i].TickValue == null)
                        objSymbol[i].TickValue = new Business.Tick();

                    //Update Value Tick Symbol
                    objSymbol[i].TickValue.Ask = Tick.Ask;
                    objSymbol[i].TickValue.Bid = Tick.Bid;
                    objSymbol[i].TickValue.Status = Tick.Status;
                    objSymbol[i].TickValue.SymbolName = objSymbol[i].Name;
                    objSymbol[i].TickValue.TickTime = Tick.TickTime;

                    objSymbol[i].UpdateTick();

                    if (objSymbol[i].RefSymbol != null && objSymbol[i].RefSymbol.Count > 0)
                    {
                        objSymbol[i].RefSymbol = ProcessRefSymbol(Tick, objSymbol[i].RefSymbol);
                    }
                }   //End For
            }

            return objSymbol;
        }   //End Function   
        #endregion 
    }
}
