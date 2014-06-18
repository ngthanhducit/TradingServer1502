using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolName"></param>
        /// <returns></returns>
        private List<Business.Symbol> GetReferenceSymbol(string SymbolName)
        {
            List<Business.Symbol> Result = new List<Symbol>();
            if (Market.SymbolList != null)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SymbolList[i].Name == SymbolName)
                    {
                        Result.Add(Market.SymbolList[i]);
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        private List<Business.Symbol> GetSymbol(string symbolName)
        {
            List<Business.Symbol> Result = new List<Symbol>();
            if (Business.Market.Symbols.ContainsKey(symbolName))
            {
                Result.Add(Business.Market.Symbols[symbolName]);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        internal bool IsExistsSymbolInSecurity(Business.OpenTrade Command)
        {
            bool result = false;
            //Command.Investor.InvestorGroupInstance.InvestorGroupID
            if (Business.Market.IGroupSecurityList != null)
            {
                for (int i = 0; i < Business.Market.IGroupSecurityList.Count; i++)
                {
                    if (Command.Investor.InvestorGroupInstance != null)
                    {
                        if (Business.Market.IGroupSecurityList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            if (Command.Symbol.SecurityID == Business.Market.IGroupSecurityList[i].SecurityID)
                            {
                                result = true;
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
        internal void InitTimeEventInSymbol()
        {
            List<Business.TimeEvent> ListTimeEvent = new List<TimeEvent>();
            Business.Market.WeekEvent = new List<TimeEvent>();
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {                    
                    if (Business.Market.SymbolList[i].ParameterItems != null)
                    {
                        #region For Check TimeEvent Of Symbol
                        int countParameter = Business.Market.SymbolList[i].ParameterItems.Count;
                        for (int j = 0; j < countParameter; j++)
                        {
                            if (Business.Market.SymbolList[i].ParameterItems[j].Code == "S042" ||
                                Business.Market.SymbolList[i].ParameterItems[j].Code == "S043")
                            {
                                ListTimeEvent = this.ExtractTimeEvent(Business.Market.SymbolList[i].ParameterItems[j].StringValue, Business.Market.SymbolList[i].Name,
                                    Business.Market.SymbolList[i].ParameterItems[j].Code, Business.Market.SymbolList[i].ParameterItems[j].ParameterItemID);

                                if (ListTimeEvent != null)
                                {
                                    int countTimeEvent = ListTimeEvent.Count;
                                    for (int n = 0; n < countTimeEvent; n++)
                                    {                                        
                                        #region Execute Events Later In Week
                                        if (ListTimeEvent[n].Time.DayInWeek == DateTime.Now.DayOfWeek)
                                        {
                                            int Hour = ListTimeEvent[n].Time.Hour - DateTime.Now.Hour;
                                            int Minute = ListTimeEvent[n].Time.Minute - DateTime.Now.Minute;

                                            int temp = (Hour * 60) + Minute;
                                            if (temp < 0)
                                            {
                                                if (ListTimeEvent[n].TargetFunction != null)
                                                {
                                                    int countTargetFunction = ListTimeEvent[n].TargetFunction.Count;
                                                    for (int k = 0; k < countTargetFunction; k++)
                                                    {
                                                        ListTimeEvent[n].TargetFunction[k].Function(ListTimeEvent[n].TargetFunction[k].EventPosition, ListTimeEvent[n]);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                                                                
                                        Business.Market.WeekEvent.Add(ListTimeEvent[n]);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            if (Business.Market.WeekEvent != null && Business.Market.WeekEvent.Count > 0)
            {
                //Call Function Sort Week Event
                this.ProcessSortTimeEventWeek();
            }
        }

        /// <summary>
        /// INIT TIME EVENT SERVER, HOLIDAY, EVENT END DAY
        /// </summary>
        internal void InitTimeEventServer()
        {
            string[] subEndofDay = new string[] { };
            Business.Market.YearEvent = new List<TimeEvent>();
            Business.Market.DayEvent = new List<TimeEvent>();
            if (Market.MarketConfig != null)
            {
                int count = Market.MarketConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    #region GET CONFIG END OF DAY OF MARKET CONFIG
                    if (Market.MarketConfig[i].Code == "C09")
                    {
                        subEndofDay = Market.MarketConfig[i].StringValue.Split(':');

                        if (subEndofDay.Length > 1)
                        {
                            //Set End Of Day To Class Market
                            this.EndOfDay.Hour = int.Parse(subEndofDay[0]);
                            this.EndOfDay.Minute = int.Parse(subEndofDay[1]);

                            #region BUILD TIME EVENT CALCULATOR SWAP
                            Business.TimeEvent newTimeEvent = new Business.TimeEvent();
                            newTimeEvent.EventType = Business.TimeEventType.BeginSwap;
                            newTimeEvent.TimeEventID = Market.MarketConfig[i].ParameterItemID;

                            Business.TargetFunction newTargetFunction = new Business.TargetFunction();
                            newTargetFunction.EventPosition = "All";
                            newTargetFunction.Function = this.BeginCalculationSwap;

                            newTimeEvent.TargetFunction = new List<Business.TargetFunction>();
                            newTimeEvent.TargetFunction.Add(newTargetFunction);

                            Business.DateTimeEvent newDataTimeEvent = new Business.DateTimeEvent();
                            newDataTimeEvent.Day = -1;
                            newDataTimeEvent.Month = -1;
                            newDataTimeEvent.DayInWeek = DateTime.Now.DayOfWeek;
                            newDataTimeEvent.Hour = int.Parse(subEndofDay[0]);
                            newDataTimeEvent.Minute = int.Parse(subEndofDay[1]);

                            newTimeEvent.Time = newDataTimeEvent;
                            DateTime newDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, newDataTimeEvent.Hour, newDataTimeEvent.Minute, 00);

                            newTimeEvent.IsEnable = true;

                            newTimeEvent.TimeExecution = newDateTime;

                            Business.Market.DayEvent.Add(newTimeEvent);

                            #endregion

                            #region BUILD TIME EVENT MAINTAIN
                            Business.TimeEvent newTimeEventMaintain = new Business.TimeEvent();
                            newTimeEventMaintain.EventType = Business.TimeEventType.BeginMaintain;
                            newTimeEventMaintain.TimeEventID = Market.MarketConfig[i].ParameterItemID;

                            Business.TargetFunction newTargetFunctionMaintain = new Business.TargetFunction();
                            newTargetFunctionMaintain.EventPosition = "All";
                            newTargetFunctionMaintain.Function = this.BeginMaintain;

                            newTimeEventMaintain.TargetFunction = new List<Business.TargetFunction>();
                            newTimeEventMaintain.TargetFunction.Add(newTargetFunctionMaintain);

                            Business.DateTimeEvent newDataTimeEventMaintain = new Business.DateTimeEvent();
                            newDataTimeEventMaintain.Day = -1;
                            newDataTimeEventMaintain.Month = -1;
                            newDataTimeEventMaintain.DayInWeek = DateTime.Now.DayOfWeek;
                            newDataTimeEventMaintain.Hour = int.Parse(subEndofDay[0]);
                            newDataTimeEventMaintain.Minute = int.Parse(subEndofDay[1]);

                            newTimeEventMaintain.Time = newDataTimeEventMaintain;
                            DateTime newDateTimeExe = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, newDataTimeEventMaintain.Hour, newDataTimeEventMaintain.Minute, 00);
                            newTimeEventMaintain.TimeExecution = newDateTimeExe;

                            newTimeEventMaintain.IsEnable = true;

                            Business.Market.DayEvent.Add(newTimeEventMaintain);
                            #endregion

                            #region BUILD TIME EVENT EXECUTOR SETTING ORDER IN ADMIN
                            Business.TimeEvent newTimeEventOrder = new Business.TimeEvent();
                            newTimeEventOrder.EventType = Business.TimeEventType.SettingOrder;
                            newTimeEventOrder.TimeEventID = Market.MarketConfig[i].ParameterItemID;

                            Business.TargetFunction newTargetFunctionOrder = new Business.TargetFunction();
                            newTargetFunctionOrder.EventPosition = "All";
                            newTargetFunctionOrder.Function = this.ProcessSettingOrder;

                            newTimeEventOrder.TargetFunction = new List<Business.TargetFunction>();
                            newTimeEventOrder.TargetFunction.Add(newTargetFunctionOrder);

                            Business.DateTimeEvent newDataTimeEventOrder = new Business.DateTimeEvent();
                            newDataTimeEventOrder.Day = -1;
                            newDataTimeEventOrder.Month = -1;
                            newDataTimeEventOrder.DayInWeek = DateTime.Now.DayOfWeek;
                            newDataTimeEventOrder.Hour = int.Parse(subEndofDay[0]);
                            newDataTimeEventOrder.Minute = int.Parse(subEndofDay[1]);

                            newTimeEventOrder.Time = newDataTimeEventMaintain;
                            DateTime newDateTimeExeOrder = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, newDataTimeEventMaintain.Hour, newDataTimeEventMaintain.Minute, 00);
                            newTimeEventOrder.TimeExecution = newDateTimeExeOrder;

                            newTimeEventOrder.IsEnable = true;

                            Business.Market.DayEvent.Add(newTimeEventOrder);
                            #endregion
                        }

                        break;
                    }
                    #endregion
                }

                for (int j = 0; j < count; j++)
                {
                    #region GET CONFIG STATEMENTS MODE OF MARKET CONFIG
                    if (Market.MarketConfig[j].Code == "C11")
                    {
                        switch (Market.MarketConfig[j].StringValue)
                        {
                            #region BUILD EVENT STATEMENTS WITH END OF DAY
                            case "End of day":
                                {
                                    Business.TimeEvent newTimeEvent = new Business.TimeEvent();
                                    newTimeEvent.EventType = Business.TimeEventType.Statements;
                                    newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                    Business.TargetFunction newTargetFunction = new Business.TargetFunction();
                                    newTargetFunction.EventPosition = "All";
                                    newTargetFunction.Function = this.SendReportDay;

                                    newTimeEvent.TargetFunction = new List<Business.TargetFunction>();
                                    newTimeEvent.TargetFunction.Add(newTargetFunction);

                                    Business.DateTimeEvent newDataTimeEvent = new Business.DateTimeEvent();
                                    newDataTimeEvent.Day = -1;
                                    newDataTimeEvent.Month = -1;
                                    newDataTimeEvent.DayInWeek = DateTime.Now.DayOfWeek;
                                    newDataTimeEvent.Hour = int.Parse(subEndofDay[0]);
                                    newDataTimeEvent.Minute = int.Parse(subEndofDay[1]);

                                    newTimeEvent.Time = newDataTimeEvent;
                                    DateTime newDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, newDataTimeEvent.Hour, newDataTimeEvent.Minute, 00);
                                    newTimeEvent.TimeExecution = newDateTime;

                                    newTimeEvent.IsEnable = true;

                                    Business.Market.DayEvent.Add(newTimeEvent);

                                    Market.EndDayTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 00);
                                }
                                break;
                            #endregion

                            #region BUILD EVENT STATEMENTS WITH START OF DAY
                            case "Start of day":
                                {
                                    Business.TimeEvent newTimeEvent = new Business.TimeEvent();
                                    newTimeEvent.EventType = Business.TimeEventType.Statements;
                                    newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                    Business.TargetFunction newTargetFunction = new Business.TargetFunction();
                                    newTargetFunction.EventPosition = "All";
                                    newTargetFunction.Function = this.SendReportDay;

                                    newTimeEvent.TargetFunction = new List<Business.TargetFunction>();
                                    newTimeEvent.TargetFunction.Add(newTargetFunction);

                                    Business.DateTimeEvent newDataTimeEvent = new Business.DateTimeEvent();
                                    newDataTimeEvent.Day = -1;
                                    newDataTimeEvent.Month = -1;
                                    newDataTimeEvent.DayInWeek = DateTime.Now.AddDays(1).DayOfWeek;
                                    newDataTimeEvent.Hour = 00;
                                    newDataTimeEvent.Minute = 00;

                                    newTimeEvent.Time = newDataTimeEvent;

                                    DateTime newDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, newDataTimeEvent.Hour, newDataTimeEvent.Minute, 00);
                                    newTimeEvent.TimeExecution = newDateTime.AddDays(1);

                                    newTimeEvent.IsEnable = true;

                                    Business.Market.DayEvent.Add(newTimeEvent);

                                    Market.EndDayTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                                }
                                break;
                            #endregion
                        }
                    }
                    #endregion

                    #region GET CONFIG MONTHLY STATEMENTS MODE
                    if (Market.MarketConfig[j].Code == "C12")
                    {
                        switch (Market.MarketConfig[j].StringValue)
                        {
                            #region LAST DAY OF MONTH
                            case "Last day of month":
                                {
                                    Business.TimeEvent newTimeEvent = new Business.TimeEvent();
                                    newTimeEvent.EventType = Business.TimeEventType.StatementsMonth;
                                    newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                    Business.TargetFunction newTargetFunction = new Business.TargetFunction();
                                    newTargetFunction.EventPosition = "All";
                                    newTargetFunction.Function = this.SendReportMonth;

                                    newTimeEvent.TargetFunction = new List<Business.TargetFunction>();
                                    newTimeEvent.TargetFunction.Add(newTargetFunction);

                                    //GET LAST DAY OF MONTH
                                    DateTime LastofMonth = Model.CommandFramework.CommandFrameworkInstance.GetLastDayOfMonth(DateTime.Now);

                                    Business.DateTimeEvent newDataTimeEvent = new Business.DateTimeEvent();
                                    newDataTimeEvent.Day = LastofMonth.Day;
                                    newDataTimeEvent.Month = LastofMonth.Month;
                                    newDataTimeEvent.DayInWeek = LastofMonth.DayOfWeek;
                                    newDataTimeEvent.Hour = int.Parse(subEndofDay[0]);
                                    newDataTimeEvent.Minute = int.Parse(subEndofDay[1]);

                                    newTimeEvent.Time = newDataTimeEvent;

                                    DateTime newDateTime = new DateTime(DateTime.Now.Year, newDataTimeEvent.Month, newDataTimeEvent.Day, newDataTimeEvent.Hour, newDataTimeEvent.Minute, 00);
                                    newTimeEvent.TimeExecution = newDateTime;

                                    TimeSpan temp = newTimeEvent.TimeExecution - DateTime.Now;
                                    if (temp.TotalSeconds < 0)
                                    {
                                        newTimeEvent.TimeExecution = newTimeEvent.TimeExecution.AddMonths(1);
                                    }

                                    newTimeEvent.IsEnable = true;

                                    Business.Market.YearEvent.Add(newTimeEvent);
                                }
                                break;
                            #endregion

                            #region FIRST DAY OF MONTH
                            case "First day of month":
                                {
                                    Business.TimeEvent newTimeEvent = new Business.TimeEvent();
                                    newTimeEvent.EventType = Business.TimeEventType.StatementsMonth;
                                    newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                    Business.TargetFunction newTargetFunction = new Business.TargetFunction();
                                    newTargetFunction.EventPosition = "All";
                                    newTargetFunction.Function = this.SendReportMonth;

                                    newTimeEvent.TargetFunction = new List<Business.TargetFunction>();
                                    newTimeEvent.TargetFunction.Add(newTargetFunction);

                                    //GET LAST DAY OF MONTH
                                    DateTime FirstofMonth = Model.CommandFramework.CommandFrameworkInstance.GetFirstDayOfMonth(DateTime.Now);

                                    Business.DateTimeEvent newDataTimeEvent = new Business.DateTimeEvent();
                                    newDataTimeEvent.Day = FirstofMonth.Day;
                                    newDataTimeEvent.Month = FirstofMonth.Month;
                                    newDataTimeEvent.DayInWeek = FirstofMonth.DayOfWeek;
                                    newDataTimeEvent.Hour = 00;
                                    newDataTimeEvent.Minute = 00;

                                    newTimeEvent.Time = newDataTimeEvent;

                                    DateTime newDateTime = new DateTime(DateTime.Now.Year, newDataTimeEvent.Month, newDataTimeEvent.Day, newDataTimeEvent.Hour, newDataTimeEvent.Minute, 00);
                                    newTimeEvent.TimeExecution = newDateTime;

                                    TimeSpan temp = newTimeEvent.TimeExecution - DateTime.Now;
                                    if (temp.TotalSeconds < 0)
                                    {
                                        newTimeEvent.TimeExecution = newTimeEvent.TimeExecution.AddMonths(1);
                                    }

                                    newTimeEvent.IsEnable = true;

                                    Business.Market.YearEvent.Add(newTimeEvent);
                                }
                                break;
                            #endregion
                        }
                    }
                    #endregion

                    #region GET CONFIG HOLIDAY OF MARKET CONFIG
                    if (Market.MarketConfig[j].Code == "C27")
                    {
                        string[] subValue = Market.MarketConfig[j].StringValue.Split('~');
                        if (subValue.Length == 5)
                        {
                            bool IsEnable = false;
                            bool Every = false;
                            int Year = -1;
                            int Month = -1;
                            int Day = -1;
                            int HourStart = -1;
                            int MinuteStart = -1;
                            int HourEnd = -1;
                            int MinuteEnd = -1;

                            bool.TryParse(subValue[0], out IsEnable);
                            bool.TryParse(subValue[4], out Every);

                            #region SUB PARAMETER DAY AND TIME
                            string[] subTime = subValue[1].Split('#');
                            if (subTime.Length == 2)
                            {
                                string[] subDay = subTime[0].Split(':');
                                if (subDay.Length == 3)
                                {
                                    bool IsParse = int.TryParse(subDay[0], out Year);
                                    if (!IsParse)
                                        Year = -1;

                                    int.TryParse(subDay[1], out Month);
                                    int.TryParse(subDay[2], out Day);

                                    #region SUB PARAMETER START WORK AND END WORK
                                    string[] subTimeWork = subTime[1].Split('-');
                                    if (subTimeWork.Length == 2)
                                    {
                                        string[] subTimeWorkStart = subTimeWork[0].Split(':');
                                        string[] subTimeWorkEnd = subTimeWork[1].Split(':');
                                        if (subTimeWorkStart.Length == 2 && subTimeWorkEnd.Length == 2)
                                        {
                                            int.TryParse(subTimeWorkStart[0], out HourStart);
                                            int.TryParse(subTimeWorkStart[1], out MinuteStart);
                                            int.TryParse(subTimeWorkEnd[0], out HourEnd);
                                            int.TryParse(subTimeWorkEnd[1], out MinuteEnd);

                                            #region BUILD EVENT HOLIDAY
                                            //NO START WORLK AND END WORLK
                                            if (HourStart == 0 && MinuteStart == 0 && HourEnd == 0 && MinuteEnd == 0)
                                            {
                                                #region BUILD EVENT BEGIN HOLIDAY
                                                Business.TimeEvent newTimeEvent = new TimeEvent();
                                                newTimeEvent.EventType = TimeEventType.BeginHoliday;

                                                newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                newTimeEvent.TargetFunction = new List<TargetFunction>();
                                                Business.TargetFunction newTargetFunction = new TargetFunction();
                                                newTargetFunction.EventPosition = subValue[2];
                                                newTargetFunction.Function = this.BeginHoliday;

                                                newTimeEvent.TargetFunction.Add(newTargetFunction);

                                                Business.DateTimeEvent newDateTimeEvent = new DateTimeEvent();
                                                newDateTimeEvent.Year = Year;
                                                newDateTimeEvent.Month = Month;
                                                newDateTimeEvent.Day = Day;
                                                newDateTimeEvent.Hour = 00;
                                                newDateTimeEvent.Minute = 00;

                                                newTimeEvent.Time = newDateTimeEvent;
                                                newTimeEvent.Every = Every;

                                                newTimeEvent.IsEnable = IsEnable;

                                                Business.Market.YearEvent.Add(newTimeEvent);
                                                #endregion

                                                #region EXECUTOR EVENT LATER
                                                if (newTimeEvent.IsEnable)
                                                {
                                                    if (newTimeEvent.Time.Day == DateTime.Now.Day && newTimeEvent.Time.Month == DateTime.Now.Month)
                                                    {
                                                        int tempHour = DateTime.Now.Hour - newTimeEvent.Time.Hour;
                                                        int tempMinute = DateTime.Now.Minute - newTimeEvent.Time.Minute;
                                                        int tempTotal = (tempHour * 60) + tempMinute;

                                                        if (tempTotal > 0)
                                                        {
                                                            if (newTimeEvent.TargetFunction != null)
                                                            {
                                                                int countTarget = newTimeEvent.TargetFunction.Count;
                                                                for (int m = 0; m < countTarget; m++)
                                                                {
                                                                    newTimeEvent.TargetFunction[m].Function(newTimeEvent.TargetFunction[m].EventPosition, newTimeEvent);
                                                                }
                                                            }
                                                        }
                                                    }   
                                                }                                                
                                                #endregion                                                                                                 

                                                #region BUILD EVENT END HOLIDAY
                                                Business.TimeEvent newTimeEventEnd = new TimeEvent();
                                                newTimeEventEnd.EventType = TimeEventType.EndHoliday;

                                                newTimeEventEnd.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                newTimeEventEnd.TargetFunction = new List<TargetFunction>();
                                                Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                newTargetFunctionEnd.EventPosition = subValue[2];
                                                newTargetFunctionEnd.Function = this.EndHoliday;

                                                newTimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                Business.DateTimeEvent newDateTimeEventEnd = new DateTimeEvent();
                                                newDateTimeEventEnd.Year = Year;
                                                newDateTimeEventEnd.Month = Month;
                                                newDateTimeEventEnd.Day = Day;
                                                newDateTimeEventEnd.Hour = int.Parse(subEndofDay[0]);
                                                newDateTimeEventEnd.Minute = int.Parse(subEndofDay[1]);

                                                newTimeEventEnd.Time = newDateTimeEventEnd;
                                                newTimeEventEnd.Every = Every;

                                                newTimeEventEnd.IsEnable = IsEnable;

                                                Business.Market.YearEvent.Add(newTimeEventEnd);
                                                #endregion
                                            }
                                            else
                                            {
                                                //IF START WORLK = BEGIN HOLIDAY
                                                if (HourStart == 0 && MinuteStart == 0)
                                                {
                                                    #region BUILD EVENT TIME WORK START
                                                    Business.TimeEvent newTimeEventWorkStart = new TimeEvent();
                                                    newTimeEventWorkStart.EventType = TimeEventType.BeginWork;

                                                    newTimeEventWorkStart.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                    newTimeEventWorkStart.TargetFunction = new List<TargetFunction>();
                                                    Business.TargetFunction newTargetFunctionWorkStart = new TargetFunction();
                                                    newTargetFunctionWorkStart.EventPosition = subValue[2];
                                                    newTargetFunctionWorkStart.Function = this.BeginWork;

                                                    newTimeEventWorkStart.TargetFunction.Add(newTargetFunctionWorkStart);

                                                    Business.DateTimeEvent newDateTimeEventWorkStart = new DateTimeEvent();
                                                    newDateTimeEventWorkStart.Year = Year;
                                                    newDateTimeEventWorkStart.Month = Month;
                                                    newDateTimeEventWorkStart.Day = Day;
                                                    newDateTimeEventWorkStart.Hour = HourStart;
                                                    newDateTimeEventWorkStart.Minute = MinuteStart;

                                                    newTimeEventWorkStart.Time = newDateTimeEventWorkStart;
                                                    newTimeEventWorkStart.Every = Every;

                                                    newTimeEventWorkStart.IsEnable = IsEnable;

                                                    Business.Market.YearEvent.Add(newTimeEventWorkStart);
                                                    #endregion

                                                    #region EXECUTOR EVENT LATER WORK START
                                                    if (newTimeEventWorkStart.IsEnable)
                                                    {
                                                        if (newTimeEventWorkStart.Time.Day == DateTime.Now.Day && newTimeEventWorkStart.Time.Month == DateTime.Now.Month)
                                                        {
                                                            int tempHour = DateTime.Now.Hour - newTimeEventWorkStart.Time.Hour;
                                                            int tempMinute = DateTime.Now.Minute - newTimeEventWorkStart.Time.Minute;
                                                            int tempTotal = (tempHour * 60) + tempMinute;

                                                            if (tempTotal > 0)
                                                            {
                                                                if (newTimeEventWorkStart.TargetFunction != null)
                                                                {
                                                                    int countTarget = newTimeEventWorkStart.TargetFunction.Count;
                                                                    for (int m = 0; m < countTarget; m++)
                                                                    {
                                                                        newTimeEventWorkStart.TargetFunction[m].Function(newTimeEventWorkStart.TargetFunction[m].EventPosition, newTimeEventWorkStart);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }                                                    
                                                    #endregion                                                                                                 

                                                    #region BUILD EVENT TIME WORK END
                                                    Business.TimeEvent newTimeEventWorkEnd = new TimeEvent();
                                                    newTimeEventWorkEnd.EventType = TimeEventType.EndWork;

                                                    newTimeEventWorkEnd.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                    newTimeEventWorkEnd.TargetFunction = new List<TargetFunction>();
                                                    Business.TargetFunction newTargetFunctionWorkEnd = new TargetFunction();
                                                    newTargetFunctionWorkEnd.EventPosition = subValue[2];
                                                    newTargetFunctionWorkEnd.Function = this.EndWork;

                                                    newTimeEventWorkEnd.TargetFunction.Add(newTargetFunctionWorkEnd);

                                                    Business.DateTimeEvent newDateTimeEventWorkEnd = new DateTimeEvent();
                                                    newDateTimeEventWorkEnd.Year = Year;
                                                    newDateTimeEventWorkEnd.Month = Month;
                                                    newDateTimeEventWorkEnd.Day = Day;
                                                    newDateTimeEventWorkEnd.Hour = HourEnd;
                                                    newDateTimeEventWorkEnd.Minute = MinuteEnd;

                                                    newTimeEventWorkEnd.Time = newDateTimeEventWorkEnd;
                                                    newTimeEventWorkEnd.Every = Every;

                                                    newTimeEventWorkEnd.IsEnable = IsEnable;

                                                    Business.Market.YearEvent.Add(newTimeEventWorkEnd);
                                                    #endregion

                                                    #region EXECUTOR EVENT LATER WORK END
                                                    if (newTimeEventWorkEnd.IsEnable)
                                                    {
                                                        if (newTimeEventWorkEnd.Time.Day == DateTime.Now.Day && newTimeEventWorkEnd.Time.Month == DateTime.Now.Month)
                                                        {
                                                            int tempHour = DateTime.Now.Hour - newTimeEventWorkEnd.Time.Hour;
                                                            int tempMinute = DateTime.Now.Minute - newTimeEventWorkEnd.Time.Minute;
                                                            int tempTotal = (tempHour * 60) + tempMinute;

                                                            if (tempTotal > 0)
                                                            {
                                                                if (newTimeEventWorkEnd.TargetFunction != null)
                                                                {
                                                                    int countTarget = newTimeEventWorkEnd.TargetFunction.Count;
                                                                    for (int m = 0; m < countTarget; m++)
                                                                    {
                                                                        newTimeEventWorkEnd.TargetFunction[m].Function(newTimeEventWorkEnd.TargetFunction[m].EventPosition, newTimeEventWorkEnd);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }                                                    
                                                    #endregion    

                                                    #region BUILD EVENT END HOLIDAY
                                                    Business.TimeEvent newTimeEventEnd = new TimeEvent();
                                                    newTimeEventEnd.EventType = TimeEventType.EndHoliday;

                                                    newTimeEventEnd.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                    newTimeEventEnd.TargetFunction = new List<TargetFunction>();
                                                    Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                    newTargetFunctionEnd.EventPosition = subValue[2];
                                                    newTargetFunctionEnd.Function = this.EndHoliday;

                                                    newTimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                    Business.DateTimeEvent newDateTimeEventEnd = new DateTimeEvent();
                                                    newDateTimeEventEnd.Year = Year;
                                                    newDateTimeEventEnd.Month = Month;
                                                    newDateTimeEventEnd.Day = Day;
                                                    newDateTimeEventEnd.Hour = int.Parse(subEndofDay[0]);
                                                    newDateTimeEventEnd.Minute = int.Parse(subEndofDay[1]);

                                                    newTimeEventEnd.Time = newDateTimeEventEnd;
                                                    newTimeEventEnd.Every = Every;

                                                    newTimeEventEnd.IsEnable = IsEnable;

                                                    Business.Market.YearEvent.Add(newTimeEventEnd);
                                                    #endregion
                                                }
                                                else
                                                {
                                                    //IF END WORLK = END HOLIDAY
                                                    if (HourEnd == 24)
                                                    {
                                                        #region BUILD EVENT BEGIN HOLIDAY
                                                        Business.TimeEvent newTimeEvent = new TimeEvent();
                                                        newTimeEvent.EventType = TimeEventType.BeginHoliday;

                                                        newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                        newTimeEvent.TargetFunction = new List<TargetFunction>();
                                                        Business.TargetFunction newTargetFunction = new TargetFunction();
                                                        newTargetFunction.EventPosition = subValue[2];
                                                        newTargetFunction.Function = this.BeginHoliday;

                                                        newTimeEvent.TargetFunction.Add(newTargetFunction);

                                                        Business.DateTimeEvent newDateTimeEvent = new DateTimeEvent();
                                                        newDateTimeEvent.Year = Year;
                                                        newDateTimeEvent.Month = Month;
                                                        newDateTimeEvent.Day = Day;
                                                        newDateTimeEvent.Hour = 00;
                                                        newDateTimeEvent.Minute = 00;

                                                        newTimeEvent.Time = newDateTimeEvent;
                                                        newTimeEvent.Every = Every;

                                                        newTimeEvent.IsEnable = IsEnable;

                                                        Business.Market.YearEvent.Add(newTimeEvent);
                                                        #endregion

                                                        #region EXECUTOR EVENT LATER BEGIN HOLIDAY
                                                        if (newTimeEvent.IsEnable)
                                                        {
                                                            if (newTimeEvent.Time.Day == DateTime.Now.Day && newTimeEvent.Time.Month == DateTime.Now.Month)
                                                            {
                                                                int tempHour = DateTime.Now.Hour - newTimeEvent.Time.Hour;
                                                                int tempMinute = DateTime.Now.Minute - newTimeEvent.Time.Minute;
                                                                int tempTotal = (tempHour * 60) + tempMinute;

                                                                if (tempTotal > 0)
                                                                {
                                                                    if (newTimeEvent.TargetFunction != null)
                                                                    {
                                                                        int countTarget = newTimeEvent.TargetFunction.Count;
                                                                        for (int m = 0; m < countTarget; m++)
                                                                        {
                                                                            newTimeEvent.TargetFunction[m].Function(newTimeEvent.TargetFunction[m].EventPosition, newTimeEvent);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }                                                        
                                                        #endregion                                                                                                 

                                                        #region BUILD EVENT TIME WORK START
                                                        Business.TimeEvent newTimeEventWorkStart = new TimeEvent();
                                                        newTimeEventWorkStart.EventType = TimeEventType.BeginWork;

                                                        newTimeEventWorkStart.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                        newTimeEventWorkStart.TargetFunction = new List<TargetFunction>();
                                                        Business.TargetFunction newTargetFunctionWorkStart = new TargetFunction();
                                                        newTargetFunctionWorkStart.EventPosition = subValue[2];
                                                        newTargetFunctionWorkStart.Function = this.BeginWork;

                                                        newTimeEventWorkStart.TargetFunction.Add(newTargetFunctionWorkStart);

                                                        Business.DateTimeEvent newDateTimeEventWorkStart = new DateTimeEvent();
                                                        newDateTimeEventWorkStart.Year = Year;
                                                        newDateTimeEventWorkStart.Month = Month;
                                                        newDateTimeEventWorkStart.Day = Day;
                                                        newDateTimeEventWorkStart.Hour = HourStart;
                                                        newDateTimeEventWorkStart.Minute = MinuteStart;

                                                        newTimeEventWorkStart.Time = newDateTimeEventWorkStart;
                                                        newTimeEventWorkStart.Every = Every;

                                                        newTimeEventWorkStart.IsEnable = IsEnable;

                                                        Business.Market.YearEvent.Add(newTimeEventWorkStart);
                                                        #endregion

                                                        #region EXECUTOR EVENT LATER WORK START
                                                        if (newTimeEventWorkStart.IsEnable)
                                                        {
                                                            if (newTimeEventWorkStart.Time.Day == DateTime.Now.Day && newTimeEventWorkStart.Time.Month == DateTime.Now.Month)
                                                            {
                                                                int tempHour = DateTime.Now.Hour - newTimeEventWorkStart.Time.Hour;
                                                                int tempMinute = DateTime.Now.Minute - newTimeEventWorkStart.Time.Minute;
                                                                int tempTotal = (tempHour * 60) + tempMinute;

                                                                if (tempTotal > 0)
                                                                {
                                                                    if (newTimeEventWorkStart.TargetFunction != null)
                                                                    {
                                                                        int countTarget = newTimeEventWorkStart.TargetFunction.Count;
                                                                        for (int m = 0; m < countTarget; m++)
                                                                        {
                                                                            newTimeEventWorkStart.TargetFunction[m].Function(newTimeEventWorkStart.TargetFunction[m].EventPosition, newTimeEventWorkStart);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }                                                        
                                                        #endregion                                                                                                 

                                                        #region BUILD EVENT END HOLIDAY
                                                        Business.TimeEvent newTimeEventEnd = new TimeEvent();
                                                        newTimeEventEnd.EventType = TimeEventType.EndHoliday;

                                                        newTimeEventEnd.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                        newTimeEventEnd.TargetFunction = new List<TargetFunction>();
                                                        Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                        newTargetFunctionEnd.EventPosition = subValue[2];
                                                        newTargetFunctionEnd.Function = this.EndHoliday;

                                                        newTimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                        Business.DateTimeEvent newDateTimeEventEnd = new DateTimeEvent();
                                                        newDateTimeEventEnd.Year = Year;
                                                        newDateTimeEventEnd.Month = Month;
                                                        newDateTimeEventEnd.Day = Day;
                                                        newDateTimeEventEnd.Hour = int.Parse(subEndofDay[0]);
                                                        newDateTimeEventEnd.Minute = int.Parse(subEndofDay[1]);

                                                        newTimeEventEnd.Time = newDateTimeEventEnd;
                                                        newTimeEventEnd.Every = Every;

                                                        newTimeEventEnd.IsEnable = IsEnable;

                                                        Business.Market.YearEvent.Add(newTimeEventEnd);
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        //FULL HOLIDAY
                                                        #region BUILD EVENT BEGIN HOLIDAY
                                                        Business.TimeEvent newTimeEvent = new TimeEvent();
                                                        newTimeEvent.EventType = TimeEventType.BeginHoliday;

                                                        newTimeEvent.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                        newTimeEvent.TargetFunction = new List<TargetFunction>();
                                                        Business.TargetFunction newTargetFunction = new TargetFunction();
                                                        newTargetFunction.EventPosition = subValue[2];
                                                        newTargetFunction.Function = this.BeginHoliday;

                                                        newTimeEvent.TargetFunction.Add(newTargetFunction);

                                                        Business.DateTimeEvent newDateTimeEvent = new DateTimeEvent();
                                                        newDateTimeEvent.Year = Year;
                                                        newDateTimeEvent.Month = Month;
                                                        newDateTimeEvent.Day = Day;
                                                        newDateTimeEvent.Hour = 00;
                                                        newDateTimeEvent.Minute = 00;

                                                        newTimeEvent.Time = newDateTimeEvent;
                                                        newTimeEvent.Every = Every;

                                                        newTimeEvent.IsEnable = IsEnable;

                                                        Business.Market.YearEvent.Add(newTimeEvent);
                                                        #endregion

                                                        #region EXECUTOR EVENT LATER BEGIN HOLIDAY
                                                        if (newTimeEvent.IsEnable)
                                                        {
                                                            if (newTimeEvent.Time.Day == DateTime.Now.Day && newTimeEvent.Time.Month == DateTime.Now.Month)
                                                            {
                                                                int tempHour = DateTime.Now.Hour - newTimeEvent.Time.Hour;
                                                                int tempMinute = DateTime.Now.Minute - newTimeEvent.Time.Minute;
                                                                int tempTotal = (tempHour * 60) + tempMinute;

                                                                if (tempTotal > 0)
                                                                {
                                                                    if (newTimeEvent.TargetFunction != null)
                                                                    {
                                                                        int countTarget = newTimeEvent.TargetFunction.Count;
                                                                        for (int m = 0; m < countTarget; m++)
                                                                        {
                                                                            newTimeEvent.TargetFunction[m].Function(newTimeEvent.TargetFunction[m].EventPosition, newTimeEvent);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }                                                        
                                                        #endregion                                                                                                 

                                                        if (HourStart != HourEnd || MinuteStart != MinuteEnd)
                                                        {
                                                            #region BUILD EVENT TIME WORK START
                                                            Business.TimeEvent newTimeEventWorkStart = new TimeEvent();
                                                            newTimeEventWorkStart.EventType = TimeEventType.BeginWork;

                                                            newTimeEventWorkStart.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                            newTimeEventWorkStart.TargetFunction = new List<TargetFunction>();
                                                            Business.TargetFunction newTargetFunctionWorkStart = new TargetFunction();
                                                            newTargetFunctionWorkStart.EventPosition = subValue[2];
                                                            newTargetFunctionWorkStart.Function = this.BeginWork;

                                                            newTimeEventWorkStart.TargetFunction.Add(newTargetFunctionWorkStart);

                                                            Business.DateTimeEvent newDateTimeEventWorkStart = new DateTimeEvent();
                                                            newDateTimeEventWorkStart.Year = Year;
                                                            newDateTimeEventWorkStart.Month = Month;
                                                            newDateTimeEventWorkStart.Day = Day;
                                                            newDateTimeEventWorkStart.Hour = HourStart;
                                                            newDateTimeEventWorkStart.Minute = MinuteStart;

                                                            newTimeEventWorkStart.Time = newDateTimeEventWorkStart;
                                                            newTimeEventWorkStart.Every = Every;

                                                            newTimeEventWorkStart.IsEnable = IsEnable;

                                                            Business.Market.YearEvent.Add(newTimeEventWorkStart);
                                                            #endregion

                                                            #region EXECUTOR EVENT LATER WORK START
                                                            if (newTimeEventWorkStart.IsEnable)
                                                            {
                                                                if (newTimeEventWorkStart.Time.Day == DateTime.Now.Day && newTimeEventWorkStart.Time.Month == DateTime.Now.Month)
                                                                {
                                                                    int tempHour = DateTime.Now.Hour - newTimeEventWorkStart.Time.Hour;
                                                                    int tempMinute = DateTime.Now.Minute - newTimeEventWorkStart.Time.Minute;
                                                                    int tempTotal = (tempHour * 60) + tempMinute;

                                                                    if (tempTotal > 0)
                                                                    {
                                                                        if (newTimeEventWorkStart.TargetFunction != null)
                                                                        {
                                                                            int countTarget = newTimeEventWorkStart.TargetFunction.Count;
                                                                            for (int m = 0; m < countTarget; m++)
                                                                            {
                                                                                newTimeEventWorkStart.TargetFunction[m].Function(newTimeEventWorkStart.TargetFunction[m].EventPosition, newTimeEventWorkStart);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }                                                            
                                                            #endregion                                                                                                 

                                                            #region BUILD EVENT TIME WORK END
                                                            Business.TimeEvent newTimeEventWorkEnd = new TimeEvent();
                                                            newTimeEventWorkEnd.EventType = TimeEventType.EndWork;

                                                            newTimeEventWorkEnd.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                            newTimeEventWorkEnd.TargetFunction = new List<TargetFunction>();
                                                            Business.TargetFunction newTargetFunctionWorkEnd = new TargetFunction();
                                                            newTargetFunctionWorkEnd.EventPosition = subValue[2];
                                                            newTargetFunctionWorkEnd.Function = this.EndWork;

                                                            newTimeEventWorkEnd.TargetFunction.Add(newTargetFunctionWorkEnd);

                                                            Business.DateTimeEvent newDateTimeEventWorkEnd = new DateTimeEvent();
                                                            newDateTimeEventWorkEnd.Year = Year;
                                                            newDateTimeEventWorkEnd.Month = Month;
                                                            newDateTimeEventWorkEnd.Day = Day;
                                                            newDateTimeEventWorkEnd.Hour = HourEnd;
                                                            newDateTimeEventWorkEnd.Minute = MinuteEnd;

                                                            newTimeEventWorkEnd.Time = newDateTimeEventWorkEnd;
                                                            newTimeEventWorkEnd.Every = Every;

                                                            newTimeEventWorkEnd.IsEnable = IsEnable;

                                                            Business.Market.YearEvent.Add(newTimeEventWorkEnd);
                                                            #endregion

                                                            #region EXECUTOR EVENT LATER WORK START
                                                            if (newTimeEventWorkEnd.IsEnable)
                                                            {
                                                                if (newTimeEventWorkEnd.Time.Day == DateTime.Now.Day && newTimeEventWorkEnd.Time.Month == DateTime.Now.Month)
                                                                {
                                                                    int tempHour = DateTime.Now.Hour - newTimeEventWorkEnd.Time.Hour;
                                                                    int tempMinute = DateTime.Now.Minute - newTimeEventWorkEnd.Time.Minute;
                                                                    int tempTotal = (tempHour * 60) + tempMinute;

                                                                    if (tempTotal > 0)
                                                                    {
                                                                        if (newTimeEventWorkEnd.TargetFunction != null)
                                                                        {
                                                                            int countTarget = newTimeEventWorkEnd.TargetFunction.Count;
                                                                            for (int m = 0; m < countTarget; m++)
                                                                            {
                                                                                newTimeEventWorkEnd.TargetFunction[m].Function(newTimeEventWorkEnd.TargetFunction[m].EventPosition, newTimeEventWorkEnd);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }                                                            
                                                            #endregion                                                                                                 
                                                        }

                                                        #region BUILD EVENT END HOLIDAY
                                                        Business.TimeEvent newTimeEventEnd = new TimeEvent();
                                                        newTimeEventEnd.EventType = TimeEventType.EndHoliday;

                                                        newTimeEventEnd.TimeEventID = Market.MarketConfig[j].ParameterItemID;

                                                        newTimeEventEnd.TargetFunction = new List<TargetFunction>();
                                                        Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                        newTargetFunctionEnd.EventPosition = subValue[2];
                                                        newTargetFunctionEnd.Function = this.EndHoliday;

                                                        newTimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                        Business.DateTimeEvent newDateTimeEventEnd = new DateTimeEvent();
                                                        newDateTimeEventEnd.Year = Year;
                                                        newDateTimeEventEnd.Month = Month;
                                                        newDateTimeEventEnd.Day = Day;
                                                        newDateTimeEventEnd.Hour = int.Parse(subEndofDay[0]);
                                                        newDateTimeEventEnd.Minute = int.Parse(subEndofDay[1]);

                                                        newTimeEventEnd.Time = newDateTimeEventEnd;
                                                        newTimeEventEnd.Every = Every;

                                                        newTimeEventEnd.IsEnable = IsEnable;

                                                        Business.Market.YearEvent.Add(newTimeEventEnd);
                                                        #endregion
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region GET DEFAULT GROUP BROKER
                    if (Business.Market.MarketConfig[j].Code == "C34")
                    {
                        string[] subValue = Business.Market.MarketConfig[j].StringValue.Split('`');
                        if (subValue.Length == 2)
                        {
                            Business.GroupDefault newGroupDefault = new GroupDefault();
                            newGroupDefault.GroupDefaultID = Business.Market.MarketConfig[j].ParameterItemID;
                            newGroupDefault.DomainName = subValue[0];
                            newGroupDefault.GroupDefaultName = subValue[1];

                            Business.Market.ListGroupDefault.Add(newGroupDefault);
                        }
                    }
                    #endregion

                    #region GET CONFIG TIME MARKET OF MARKET CONFIG
                    if (Market.MarketConfig[j].Code == "C28")
                    {
                        List<Business.TimeEvent> ResultTimeEvent = new List<TimeEvent>();
                        ResultTimeEvent = this.ExtractTimeEvent(Market.MarketConfig[j].StringValue, "Market", Market.MarketConfig[j].Code, Market.MarketConfig[j].ParameterItemID);
                        if (ResultTimeEvent != null && ResultTimeEvent.Count > 0)
                        {
                            int countTimeEvent = ResultTimeEvent.Count;
                            for (int n = 0; n < countTimeEvent; n++)
                            {
                                #region Execute Events Later In Week
                                if (ResultTimeEvent[n].Time.DayInWeek == DateTime.Now.DayOfWeek)
                                {
                                    int Hour = ResultTimeEvent[n].Time.Hour - DateTime.Now.Hour;
                                    int Minute = ResultTimeEvent[n].Time.Minute - DateTime.Now.Minute;

                                    int temp = (Hour * 60) + Minute;
                                    if (temp < 0)
                                    {
                                        if (ResultTimeEvent[n].TargetFunction != null)
                                        {
                                            int countTargetFunction = ResultTimeEvent[n].TargetFunction.Count;
                                            for (int k = 0; k < countTargetFunction; k++)
                                            {
                                                ResultTimeEvent[n].TargetFunction[k].Function(ResultTimeEvent[n].TargetFunction[k].EventPosition, ResultTimeEvent[n]);
                                            }
                                        }
                                    }
                                }
                                #endregion

                                Business.Market.WeekEvent.Add(ResultTimeEvent[n]);
                            }
                        }
                    }
                    #endregion

                    #region GET CONFIG MULTIPLE QUOTES
                    if (Business.Market.MarketConfig[j].Code == "C30")                    
                        Business.PriceServer.Instance.ConfigMultipleQuotes(Business.Market.MarketConfig[j].StringValue);                    
                    #endregion

                    #region GET CONFIG TIME MULTIPLE QUOTES
                    if (Business.Market.MarketConfig[j].Code == "C31")
                        Business.PriceServer.Instance.UpdateTimeCheckMultipleQuotes(Business.Market.MarketConfig[j].NumValue);
                    #endregion
                }
            }

            if (Business.Market.DayEvent != null && Business.Market.DayEvent.Count > 0)
                this.ProcessSortTimeEventDay();

            if (Business.Market.YearEvent != null && Business.Market.YearEvent.Count > 0)
                this.ProcessSortTimeEventYear();

            if (Business.Market.WeekEvent != null && Business.Market.WeekEvent.Count > 0)
                this.ProcessSortTimeEventWeek();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InitSymbolFuture()
        {
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.MarketArea.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.MarketArea[i].IMarketAreaName.Trim() == "FutureCommand")
                    {
                        Business.Market.FutureEvent = new List<TimeEvent>();
                        if (Business.Market.MarketArea[i].ListSymbol != null)
                        {
                            int countSymbol = Business.Market.MarketArea[i].ListSymbol.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.MarketArea[i].ListSymbol[j].ParameterItems != null)
                                {
                                    int countParameter = Business.Market.MarketArea[i].ListSymbol[j].ParameterItems.Count;
                                    for (int n = 0; n < countParameter; n++)
                                    {
                                        DateTime TimeExecutore = new DateTime();
                                        Business.TimeEvent newTimeEvent = new TimeEvent();

                                        if (Business.Market.MarketArea[i].ListSymbol[j].ParameterItems[n].Code == "S044")
                                        {
                                            #region BUILD EVENT PROCESS COMMAND OF SYMBOL FUTURE
                                            TimeExecutore = Business.Market.MarketArea[i].ListSymbol[j].ParameterItems[n].DateValue;

                                            newTimeEvent.EventType = TimeEventType.IsCloseFutureEvent;
                                            newTimeEvent.Every = false;                                            

                                            Business.TargetFunction newTargetFunction = new TargetFunction();
                                            newTargetFunction.EventID = Business.Market.MarketArea[i].ListSymbol[j].SymbolID;
                                            newTargetFunction.EventPosition = Business.Market.MarketArea[i].ListSymbol[j].Name;
                                            newTargetFunction.Function = this.SetIsCloseOnlyFuture;

                                            newTimeEvent.TargetFunction = new List<TargetFunction>();
                                            newTimeEvent.TargetFunction.Add(newTargetFunction);

                                            newTimeEvent.TimeEventID = Business.Market.MarketArea[i].ListSymbol[j].SymbolID;
                                            newTimeEvent.TimeExecution = TimeExecutore;
                                            #endregion

                                            if (newTimeEvent.TimeExecution < DateTime.Now)
                                            {
                                                Business.Market.MarketArea[i].ListSymbol[j].isCloseOnlyFuture = true;
                                                newTimeEvent.IsEnable = false;
                                            }
                                            else
                                            {
                                                Business.Market.MarketArea[i].ListSymbol[j].isCloseOnlyFuture = false;
                                                newTimeEvent.IsEnable = true;
                                            }

                                            #region SEARCH IN FUTURE EVENT IF TIME EXECUTOR == TIME EXECUTOR
                                            //Search in future event if time executor == time executor
                                            if (Business.Market.FutureEvent != null)
                                            {
                                                bool flag = false;
                                                int countEvent = Business.Market.FutureEvent.Count;
                                                for (int m = 0; m < countEvent; m++)
                                                {
                                                    if (Business.Market.FutureEvent[m].TimeExecution == TimeExecutore)
                                                    {
                                                        if (Business.Market.FutureEvent[m].TargetFunction != null)
                                                        {
                                                            Business.Market.FutureEvent[m].TargetFunction.Add(newTimeEvent.TargetFunction[0]);
                                                        }

                                                        flag = true;

                                                        break;
                                                    }
                                                }

                                                if (!flag)
                                                {
                                                    Business.Market.FutureEvent.Add(newTimeEvent);
                                                }
                                            }
                                            #endregion
                                        }

                                        if (Business.Market.MarketArea[i].ListSymbol[j].ParameterItems[n].Code == "S045")
                                        {
                                            #region BUILD EVENT PROCESS COMMAND OF SYMBOL FUTURE
                                            TimeExecutore = Business.Market.MarketArea[i].ListSymbol[j].ParameterItems[n].DateValue;

                                            newTimeEvent.EventType = TimeEventType.ProcessFutureEvent;
                                            newTimeEvent.Every = false;
                                            newTimeEvent.IsEnable = false;

                                            Business.TargetFunction newTargetFunction = new TargetFunction();
                                            newTargetFunction.EventID = Business.Market.MarketArea[i].ListSymbol[j].SymbolID;
                                            newTargetFunction.EventPosition = Business.Market.MarketArea[i].ListSymbol[j].Name;
                                            newTargetFunction.Function = this.ProcessExpTimeFuture;

                                            newTimeEvent.TargetFunction = new List<TargetFunction>();
                                            newTimeEvent.TargetFunction.Add(newTargetFunction);
                                            newTimeEvent.TimeEventID = Business.Market.MarketArea[i].ListSymbol[j].SymbolID;
                                            newTimeEvent.TimeExecution = TimeExecutore;
                                            #endregion

                                            #region CLOSE COMMAND IF TIME EXECUTOR < TIME CURRENT
                                            //Check Time Executor If Time < Time Current Then Close All Command Of Symbol
                                            if (newTimeEvent.TimeExecution < DateTime.Now)
                                            {
                                                if (Business.Market.MarketArea[i].ListSymbol[j].CommandList != null)
                                                {
                                                    int countCommand = Business.Market.MarketArea[i].ListSymbol[j].CommandList.Count;
                                                    for (int m = 0; m < Business.Market.MarketArea[i].ListSymbol[j].CommandList.Count; m++)
                                                    {
                                                        Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].IsClose = true;
                                                        Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].IsServer = true;
                                                        Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].CloseTime = DateTime.Now;
                                                        Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].CalculatorProfitCommand(Business.Market.MarketArea[i].ListSymbol[j].CommandList[m]);
                                                        Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].Profit = Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].Symbol.ConvertCurrencyToUSD(Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].Symbol.Currency,
                                                                Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].Profit, false, Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].SpreaDifferenceInOpenTrade, Business.Market.MarketArea[i].ListSymbol[j].Digit);

                                                        Business.Market.MarketArea[i].ListSymbol[j].CommandList[m].Investor.UpdateCommand(Business.Market.MarketArea[i].ListSymbol[j].CommandList[m]);
                                                    }
                                                }

                                                newTimeEvent.IsEnable = false;
                                            }
                                            else
                                            {
                                                newTimeEvent.IsEnable = true;
                                            }
                                            #endregion

                                            #region SEARCH IN FUTURE EVENT IF TIME EXECUTOR == TIME EXECUTOR
                                            //Search in future event if time executor == time executor
                                            if (Business.Market.FutureEvent != null)
                                            {
                                                bool flag = false;
                                                int countEvent = Business.Market.FutureEvent.Count;
                                                for (int m = 0; m < countEvent; m++)
                                                {
                                                    if (Business.Market.FutureEvent[m].TimeExecution == TimeExecutore)
                                                    {
                                                        if (Business.Market.FutureEvent[m].TargetFunction != null)
                                                        {
                                                            Business.Market.FutureEvent[m].TargetFunction.Add(newTimeEvent.TargetFunction[0]);
                                                        }

                                                        flag = true;

                                                        break;
                                                    }
                                                }

                                                if (!flag)
                                                {
                                                    Business.Market.FutureEvent.Add(newTimeEvent);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                }
            }

            //CALL FUNCTION PROCESS TIME EVENT EXECUTOR OF LIST FUTURE
            this.ProcessSortTimeFutureEvent();
        }

        /// <summary>
        /// Get Queue Tick Of Symbol, Using Test Error Tick
        /// </summary>
        /// <param name="SymbolName"></param>
        /// <returns></returns>
        internal List<Business.Tick> GetTickQueueBySymbolName(string SymbolName)
        {
            List<Business.Tick> Result = new List<Tick>();
            if (Business.Market.QuoteList != null)
            {
                int count = Business.Market.QuoteList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.QuoteList[i].Name == SymbolName)
                    {
                        Tick tick = new Tick();
                        tick.Ask = Business.Market.QuoteList[i].ask;
                        tick.Bid = Business.Market.QuoteList[i].bid;
                        tick.Status = Business.Market.QuoteList[i].status;
                        tick.TickTime = Business.Market.QuoteList[i].getTickTime;


                        for (int j = 0; j < Business.Market.QuoteList[i].Ticks.Count; j++)
                        {
                            Tick tem = new Tick();
                            tem.Ask = Business.Market.QuoteList[i].Ticks[i].Ask;
                            tem.Bid = Business.Market.QuoteList[i].Ticks[i].Bid;
                            tem.TickTime = Business.Market.QuoteList[i].Ticks[i].TickTime;
                            tem.SymbolName = SymbolName;
                            tem.Status = Business.Market.QuoteList[i].Ticks[i].Status;
                            Result.Add(tem);
                        }

                        Result.Insert(0, tick);
                        Result.Insert(1, Business.Market.QuoteList[i].RefSymbol[0].TickValue);

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.QuoteSymbol> GetQuoteList()
        {
            return Business.Market.QuoteList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Symbol> GetSymbolList()
        {
            return Business.Market.SymbolList;
        }

        /// <summary>
        /// FILL INSTANCE OPEN TRADE(CALL BY MANAGER)
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal Business.OpenTrade FillInstanceToCommand(int InvestorID,string Symbol,int Type)
        {
            Business.OpenTrade Result = new OpenTrade();

            #region FILL INSTANCE INVESTOR
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        Result.Investor = Business.Market.InvestorList[i];
                        break;
                    }
                }
            }
            #endregion            

            #region FILL INSTANCE SYMBOL AND TYPE
            if (Business.Market.SymbolList != null)
            {
                bool FlagSymbol = false;
                int countSymbol = Business.Market.SymbolList.Count;
                for (int j = 0; j < countSymbol; j++)
                {
                    if (Business.Market.SymbolList[j].Name == Symbol)
                    {
                        if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                        {
                            int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                            for (int n = 0; n < countType; n++)
                            {
                                if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == Type)
                                {
                                    Result.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                    break;
                                }
                            }
                        }

                        Result.Symbol = Business.Market.SymbolList[j];
                        FlagSymbol = true;
                        break;
                    }

                    if (FlagSymbol == false)
                    {
                        if (Business.Market.SymbolList[j].RefSymbol != null && Business.Market.SymbolList[j].RefSymbol.Count > 0)
                        {
                            Result.Symbol = ClientFacade.ClientFindSymbolReference(Business.Market.SymbolList[j].RefSymbol, Symbol);

                            if (Result.Symbol != null)
                            {
                                int countType = Result.Symbol.MarketAreaRef.Type.Count;
                                for (int k = 0; k < countType; k++)
                                {
                                    if (Result.Symbol.MarketAreaRef.Type[k].ID == Type)
                                    {
                                        Result.Type = Result.Symbol.MarketAreaRef.Type[k];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Fill IGroupSecurity
            if (Result.Investor != null)
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].SecurityID == Result.Symbol.SecurityID &&
                            Business.Market.IGroupSecurityList[i].InvestorGroupID == Result.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            Result.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Find Spread Difference And Add To Property SpreadDifference In Symbol
            double spreadDifference = 0;
            spreadDifference = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(Result.Symbol.SecurityID, Result.Investor.InvestorGroupInstance.InvestorGroupID);
            Result.SpreaDifferenceInOpenTrade = spreadDifference;
            //if (Result.IGroupSecurity != null)
            //{
            //    if (Result.IGroupSecurity.IGroupSecurityConfig != null)
            //    {
            //        int count = Result.IGroupSecurity.IGroupSecurityConfig.Count;
            //        for (int i = 0; i < count; i++)
            //        {
            //            if (Result.IGroupSecurity.IGroupSecurityConfig[i].Code == "B04")
            //            {
            //                double.TryParse(Result.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out spreadDifference);
            //                Result.Symbol.SpreadDifference = spreadDifference;
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion            

            return Result;
        }

        /// <summary>
        /// CONVERT STRING TO OPEN TRADE , USING DATA FROM ORDER ADMIN
        /// </summary>
        /// <param name="subParameter"></param>
        private Business.OpenTrade ConvertStringToOpenTrade(string[] subParameter)
        {
            Business.OpenTrade newOpenTrade = new OpenTrade();
            double closePrice = 0;
            DateTime closeTime;
            DateTime timeExp;
            int ID = 0;
            int investorID = 0;
            double OpenPrice = 0;
            DateTime openTime;
            double size = 0;
            double stopLoss = 0;
            //Symbol
            double takeProfit = 0;
            int TypeID = 0;
            double profit = 0;
            double swap = 0;
            double commission = 0;            
            double margin = 0;
            double taxes = 0;

            double.TryParse(subParameter[0], out closePrice);
            DateTime.TryParse(subParameter[1], out closeTime);
            DateTime.TryParse(subParameter[2], out timeExp);
            int.TryParse(subParameter[3], out ID);
            int.TryParse(subParameter[4], out investorID);
            double.TryParse(subParameter[5], out OpenPrice);
            DateTime.TryParse(subParameter[6], out openTime);
            double.TryParse(subParameter[7], out size);
            double.TryParse(subParameter[8], out stopLoss);
            //Symbol
            double.TryParse(subParameter[10], out takeProfit);
            int.TryParse(subParameter[11], out TypeID);
            double.TryParse(subParameter[12], out profit);
            double.TryParse(subParameter[13], out swap);
            double.TryParse(subParameter[14], out commission);            
            double.TryParse(subParameter[15], out margin);
            double.TryParse(subParameter[16], out taxes);

            newOpenTrade = TradingServer.Facade.FacadeFillInstanceOpenTrade(investorID, subParameter[9], TypeID);            
            newOpenTrade.ClosePrice = closePrice;
            newOpenTrade.CloseTime = closeTime;            
            newOpenTrade.Commission = commission;
            newOpenTrade.ExpTime = timeExp;
            newOpenTrade.ID = ID;
            newOpenTrade.Margin = margin;
            newOpenTrade.OpenPrice = OpenPrice;
            newOpenTrade.OpenTime = openTime;
            newOpenTrade.Profit = profit;
            newOpenTrade.Size = size;
            newOpenTrade.StopLoss = stopLoss;
            newOpenTrade.Swap = swap;
            newOpenTrade.TakeProfit = takeProfit;
            newOpenTrade.Taxes = taxes;
            newOpenTrade.SpreaDifferenceInOpenTrade = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(newOpenTrade.Symbol.SecurityID, newOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID);

            newOpenTrade.CalculatorMarginCommand(newOpenTrade);
            newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

            return newOpenTrade;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">command send</param>
        /// <param name="mode">1: group, 2: send all, 3: send with investor id</param>
        /// <param name="target">group , investorid,....</param>
        internal static bool SendNotifyToClient(string code,int mode,int target)
        {
            bool result = false;
            switch (mode)
            {
                #region SEND NOTIFY TO INVESTOR OF GROUP		 
                case 1:
                    {
                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == target)
                                {
                                    if (Business.Market.InvestorList[i].IsOnline)
                                    {
                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                        //int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                                        //if (countOnline > 0)
                                            Business.Market.InvestorList[i].ClientCommandQueue.Add(code);
                                    }

                                    result = true;
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region SEND NOTIFY TO ALL INVESTOR ONLINE		 
                case 2:
                    {
                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].IsOnline)
                                {
                                    if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                        Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                    Business.Market.InvestorList[i].ClientCommandQueue.Add(code);
                                }
                            }

                            result = true;
                        }
                    }
                    break;
                #endregion

                #region SEND NOTIFY TO INVESTOR		 
                case 3:
                    {
                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].InvestorID == target)
                                {
                                    if (Business.Market.InvestorList[i].IsOnline)
                                    {
                                        if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                        Business.Market.InvestorList[i].ClientCommandQueue.Add(code);
                                    }
                                    result = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                #endregion               
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listSecurityID"></param>
        /// <returns></returns>
        internal List<Business.Security> GetSecurityByListSecurity(List<int> listSecurityID)
        {
            List<Business.Security> result = new List<Security>();

            if (listSecurityID != null)
            {
                int count = listSecurityID.Count;
                for (int i = 0; i < count; i++)
                {
                    int countSecurity = Business.Market.SecurityList.Count;
                    for (int j = 0; j < countSecurity; j++)
                    {
                        if (Business.Market.SecurityList[j].SecurityID == listSecurityID[i])
                        {
                            result.Add(Business.Market.SecurityList[j]);

                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// FIX BALANCE ACCOUNT USING SPOT COMMAND OR FUTURE COMMAND
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="investorID"></param>
        private void FixBalanceAccount(double balance, double swap, double commission, int investorID, double agentCommission)
        {
            string agentCode = string.Empty;
            
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {                    
                    if (Business.Market.InvestorList[i].InvestorID == investorID)
                    {
                        Business.Market.InvestorList[i].Balance -= (balance + swap + commission);

                        TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);

                        agentCode = Business.Market.InvestorList[i].AgentID;

                        break;
                    }
                }
            }

            //SEARCH AGENT CODE AND UPDATE IF AGENT CODE != 0
            #region SEARCH AGENT CODE AND IF BALANCE ACCOUNT
            
            if (agentCommission > 0)
            {
                Business.Investor newAgent = TradingServer.Facade.FacadeSelectInvestorByCode(agentCode);

                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].Code == agentCode)
                        {
                            double balanceAgent = Business.Market.InvestorList[i].Balance - agentCommission;

                            Business.Market.InvestorList[i].Balance -= agentCommission;

                            TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, balanceAgent);

                            //SEND NOTIFY TO MANAGER with type =3 then balance and credit
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);

                            if (Business.Market.InvestorList[i].IsOnline)
                            {
                                string Message = "GetNewBalance";
                                Business.Market.InvestorList[i].ClientCommandQueue.Add(Message);
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
        private void ProcessExpirePending()
        {
            DateTime timeCurrenct = DateTime.Now;
            if (Business.Market.PendingOrders != null)
            {                
                for (int i = 0; i < Business.Market.PendingOrders.Count; i++)
                {
                    for (int j = i + 1; j < Business.Market.PendingOrders.Count; j++)
                    {
                        TimeSpan timeSpan = Business.Market.PendingOrders[j].ExpTime - Business.Market.PendingOrders[i].ExpTime;
                        if (timeSpan.TotalSeconds < 0)
                        {
                            Business.OpenTrade tempOpenTrade = new OpenTrade();
                            tempOpenTrade = Business.Market.PendingOrders[i];
                            Business.Market.PendingOrders[i] = Business.Market.PendingOrders[j];
                            Business.Market.PendingOrders[j] = tempOpenTrade;
                        }
                    }
                }
            }

            TimeSpan timeSpanExe = Business.Market.PendingOrders[0].ExpTime - timeCurrenct;
            if (Business.Market.TimerExpirePending != null && Business.Market.TimerExpirePending.Enabled)
            {
                Business.Market.TimerExpirePending.Stop();
                Business.Market.TimerExpirePending.Enabled = false;
                Business.Market.TimerExpirePending.Close();
                Business.Market.TimerExpirePending.Dispose();

                this.TimeEventExpirePending(timeSpanExe.TotalMilliseconds);
            }
            else
            {
                this.TimeEventExpirePending(timeSpanExe.TotalMilliseconds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TimeEventExpirePending(double timeSleep)
        {
            Business.Market.TimerExpirePending = new System.Timers.Timer(1000);
            Business.Market.TimerExpirePending.Interval = timeSleep;
            Business.Market.TimerExpirePending.Elapsed += new System.Timers.ElapsedEventHandler(ExecutorExpirePending);
            Business.Market.TimerExpirePending.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExecutorExpirePending(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime timeCurrent = DateTime.Now;
            if (Business.Market.PendingOrders != null && Business.Market.PendingOrders[0] != null)
            {
                Business.Market.PendingOrders[0].Symbol.MarketAreaRef.CloseCommand(Business.Market.PendingOrders[0]);
                Business.Market.PendingOrders.RemoveAt(0);
            }
                        
            if (Business.Market.PendingOrders.Count > 0)
            {
                 TimeSpan tempTimeSpan = Business.Market.PendingOrders[0].ExpTime - timeCurrent;
                 while (tempTimeSpan.TotalMilliseconds < 0)
                 {
                     if (Business.Market.PendingOrders[0] != null)
                     {                         
                         Business.Market.PendingOrders[0].Symbol.MarketAreaRef.CloseCommand(Business.Market.PendingOrders[0]);
                         Business.Market.PendingOrders.RemoveAt(0);

                         if (Business.Market.PendingOrders.Count > 0)
                         {
                             tempTimeSpan = Business.Market.PendingOrders[0].ExpTime - timeCurrent;
                         }
                         else
                         {
                             break;
                         }
                     }
                     else
                     {
                         Business.Market.PendingOrders.RemoveAt(0);
                         if (Business.Market.PendingOrders.Count > 0)
                         {
                             tempTimeSpan = Business.Market.PendingOrders[0].ExpTime - timeCurrent;
                         }
                         else
                         {
                             break;
                         }
                     }
                 }

                 if (Business.Market.TimerExpirePending != null && Business.Market.TimerExpirePending.Enabled)
                 {
                     Business.Market.TimerExpirePending.Stop();
                     Business.Market.TimerExpirePending.Enabled = false;
                     Business.Market.TimerExpirePending.Close();
                     Business.Market.TimerExpirePending.Dispose();

                     this.TimeEventExpirePending(tempTimeSpan.TotalMilliseconds);
                 }
                 else
                 {
                     this.TimeEventExpirePending(tempTimeSpan.TotalMilliseconds);
                 }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool CheckStatusSymbol(string name)
        {
            bool result = false;

            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == name)
                    {
                        result = Business.Market.SymbolList[i].IsTrade;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        internal bool CheckManualStopOut(int groupID)
        {
            bool result = false;

            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == groupID)
                    {
                        result = Business.Market.InvestorGroupList[i].IsManualStopOut;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listHistory"></param>
        /// <returns></returns>
        internal bool CheckScalperInvestor(List<Business.OpenTrade> listHistory)
        {
            bool result = false;

            int countTimeScalper = 0;
            int countPipScalper = 0;
            int countTotal = 0;

            if (listHistory != null && listHistory.Count > 0)
            {
                int count = listHistory.Count;
                for (int i = 0; i < count; i++)
                {
                    if (listHistory[i].Type.ID == 1 || listHistory[i].Type.ID == 2)
                    {
                        TimeSpan time = listHistory[i].CloseTime - listHistory[i].OpenTime;
                        if (time.TotalSeconds < Business.Market.ScalperTimeValue)
                            countTimeScalper++;

                        double pip = 0;
                        if (listHistory[i].Type.ID == 1)
                            pip = Math.Round((listHistory[i].ClosePrice - listHistory[i].OpenPrice), listHistory[i].Symbol.Digit);
                        else if (listHistory[i].Type.ID == 2)
                            pip = Math.Round((listHistory[i].OpenPrice - listHistory[i].ClosePrice), listHistory[i].Symbol.Digit);

                        pip = pip * Math.Pow(10, listHistory[i].Symbol.Digit);
                        pip = Math.Abs(pip);

                        if (pip < Business.Market.ScalperPipValue)
                            countPipScalper++;

                        countTotal++;
                    }
                }

                if (countTotal > 0)
                {
                    double percentTime = ((countTimeScalper * 100) / countTotal);
                    percentTime = Math.Round(percentTime, 2);

                    double percentPip = ((countPipScalper * 100) / countTotal);
                    percentPip = Math.Round(percentPip, 2);

                    if (percentTime > 50 || percentPip > 50)
                        result = true;
                }
            }

            return result;
        }
    }
}
