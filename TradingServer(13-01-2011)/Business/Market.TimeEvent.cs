using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// TIME EVENT EXECUTE DAY
        /// </summary>
        /// <param name="TimeSleep"></param>
        internal void TimeEventExecuteDay(double TimeSleep)
        {
            return;
            Business.Market.TimerEventDay = new System.Timers.Timer(1000);
            Business.Market.TimerEventDay.Interval = TimeSleep;
            Business.Market.TimerEventDay.Elapsed += new System.Timers.ElapsedEventHandler(ExecutionEventDay);            
            Business.Market.TimerEventDay.Enabled = true;
        }

        /// <summary>
        /// EXECUTION TIME EVENT DAY
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExecutionEventDay(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
            if (Business.Market.DayEvent.Count < 0)
            {
                Business.Market.TimerEventDay.Stop();
                Business.Market.TimerEventDay.Enabled = false;
                Business.Market.TimerEventDay.Close();
                Business.Market.TimerEventDay.Dispose();

                return;
            }

            Business.TimeEvent newTimeEventExe = new TimeEvent();
            newTimeEventExe.EventType = Business.Market.DayEvent[0].EventType;
            newTimeEventExe.Every = Business.Market.DayEvent[0].Every;
            newTimeEventExe.IsEnable = Business.Market.DayEvent[0].IsEnable;
            newTimeEventExe.TargetFunction = Business.Market.DayEvent[0].TargetFunction;
            newTimeEventExe.Time = Business.Market.DayEvent[0].Time;
            newTimeEventExe.TimeEventID = Business.Market.DayEvent[0].TimeEventID;
            newTimeEventExe.TimeExecution = Business.Market.DayEvent[0].TimeExecution;

            Business.Market.TimeEndDay = newTimeEventExe.TimeExecution;
            
            Business.Market.DayEvent[0].TimeExecution = Business.Market.DayEvent[0].TimeExecution.AddDays(1);

            Business.TimeEvent tempnewTimeEventExe = new TimeEvent();
            tempnewTimeEventExe.EventType = Business.Market.DayEvent[0].EventType;
            tempnewTimeEventExe.Every = Business.Market.DayEvent[0].Every;
            tempnewTimeEventExe.IsEnable = Business.Market.DayEvent[0].IsEnable;
            tempnewTimeEventExe.TargetFunction = Business.Market.DayEvent[0].TargetFunction;
            tempnewTimeEventExe.Time = Business.Market.DayEvent[0].Time;
            tempnewTimeEventExe.TimeEventID = Business.Market.DayEvent[0].TimeEventID;
            tempnewTimeEventExe.TimeExecution = Business.Market.DayEvent[0].TimeExecution;
            tempnewTimeEventExe.NumSession = Business.Market.DayEvent[0].NumSession;

            //REMOVE DAY EVENT 0
            Business.Market.DayEvent.RemoveAt(0);

            Business.Market.DayEvent.Add(tempnewTimeEventExe);

            //Calculation Time Sleep Next
            DateTime TimeCurrent = DateTime.Now;

            //Call Thread Process Event
            System.Threading.Thread newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.ThreadExecutionEventDay));
            newThread.Start(newTimeEventExe);

            TimeSpan TimeExe = Business.Market.DayEvent[0].TimeExecution - TimeCurrent;

            if (Business.Market.TimerEventDay != null && Business.Market.TimerEventDay.Enabled)
            {
                Business.Market.TimerEventDay.Stop();
                Business.Market.TimerEventDay.Enabled = false;
                Business.Market.TimerEventDay.Close();
                Business.Market.TimerEventDay.Dispose();

                this.TimeEventExecuteDay(TimeExe.TotalMilliseconds);
            }
            else
            {
                this.TimeEventExecuteDay(TimeExe.TotalMilliseconds);
            }
        }

        /// <summary>
        /// THREAD EXECUTION EVENT DAY 
        /// </summary>
        private void ThreadExecutionEventDay(object TimeEvent)
        {
            return;
            while (Business.Market.IsExecutorDay)
            {
                System.Threading.Thread.Sleep(500);
            }

            Business.Market.IsExecutorDay = true;

            Business.TimeEvent TimeEventExe = TimeEvent as Business.TimeEvent;

            if (TimeEventExe != null)
            {
                if (TimeEventExe.TargetFunction != null && TimeEventExe.TargetFunction.Count > 0)
                {
                    int count = TimeEventExe.TargetFunction.Count;
                    for (int i = 0; i < count; i++)
                    {
                        TimeEventExe.TargetFunction[i].Function(TimeEventExe.TargetFunction[i].EventPosition, TimeEventExe);
                    }
                }
            }

            Business.Market.IsExecutorDay = false;
        }

        /// <summary>
        /// PROCESS SORT TIME EVENT DAY
        /// </summary>
        private void ProcessSortTimeEventDay()
        {
            return;
            DateTime TimeCurrent = DateTime.Now;

            #region Process Sort List Day Event
            if (Business.Market.DayEvent != null)
            {
                //int count = Business.Market.DayEvent.Count;
                for (int i = 0; i < Business.Market.DayEvent.Count; i++)
                {
                    TimeSpan TimeOne = Business.Market.DayEvent[i].TimeExecution - TimeCurrent;

                    if (TimeOne.TotalMilliseconds <= 0)
                        Business.Market.DayEvent[i].TimeExecution = Business.Market.DayEvent[i].TimeExecution.AddDays(1);
                    
                    for (int j = i + 1; j < Business.Market.DayEvent.Count; j++)
                    {  
                        TimeSpan TimeTwo = Business.Market.DayEvent[j].TimeExecution - TimeCurrent;

                        if (TimeTwo.TotalMilliseconds <= 0)
                            Business.Market.DayEvent[j].TimeExecution = Business.Market.DayEvent[j].TimeExecution.AddDays(1);

                        //if (totalMinuteFirst > totalMinuteSecond)
                        if(TimeOne.TotalMilliseconds > TimeTwo.TotalMilliseconds)
                        {
                            Business.TimeEvent newTimeEvent = new TimeEvent();
                            newTimeEvent = Business.Market.DayEvent[i];
                            Business.Market.DayEvent[i] = Business.Market.DayEvent[j];
                            Business.Market.DayEvent[j] = newTimeEvent;
                        }
                        //else if (totalMinuteFirst == totalMinuteSecond)
                        else if (TimeTwo.TotalMilliseconds == TimeTwo.TotalMilliseconds)
                        {
                            if (Business.Market.DayEvent[i].Time.Hour == Business.Market.DayEvent[j].Time.Hour &&
                                Business.Market.DayEvent[i].Time.Minute == Business.Market.DayEvent[j].Time.Minute)
                            {
                                if (Business.Market.DayEvent[j].TargetFunction != null)
                                {
                                    int countTargetFunction = Business.Market.DayEvent[j].TargetFunction.Count;
                                    for (int n = 0; n < countTargetFunction; n++)
                                    {
                                        Business.Market.DayEvent[i].TargetFunction.Add(Business.Market.DayEvent[j].TargetFunction[n]);
                                    }
                                }

                                //Remove Position j
                                Business.Market.DayEvent.RemoveAt(j);

                                j--;
                            }
                        }                       
                    }                    
                }
            }
            #endregion
                        
            TimeSpan TimeExe = Business.Market.DayEvent[0].TimeExecution - TimeCurrent;

            if (Business.Market.TimerEventDay != null && Business.Market.TimerEventDay.Enabled)
            {
                Business.Market.TimerEventDay.Stop();
                Business.Market.TimerEventDay.Enabled = false;
                Business.Market.TimerEventDay.Close();
                Business.Market.TimerEventDay.Dispose();

                this.TimeEventExecuteDay(TimeExe.TotalMilliseconds);
            }
            else
            {
                this.TimeEventExecuteDay(TimeExe.TotalMilliseconds);
            }
        }
        
        //=====================================================

        /// <summary>
        /// TIMER EVENT EXECUTOR WEEK
        /// </summary>
        /// <param name="TimeSleep"></param>
        internal void TimeEventExecuteWeek(double TimeSleep)
        {
            return;
            Business.Market.TimerEventWeek = new System.Timers.Timer(1000);            
            Double temp = TimeSleep * 1000;

            if (temp > Int32.MaxValue)
            {
                double tempTime = Int32.MaxValue;
                this.TimeLeftWeek = temp - tempTime;

                Business.Market.TimerEventWeek.Interval = tempTime;                
            }
            else
            {
                Business.Market.TimerEventWeek.Interval = temp;                
            }

            Business.Market.TimerEventWeek.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteEventWeek);
            Business.Market.TimerEventWeek.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExecuteEventWeek(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
            if (this.TimeLeftWeek > 0)
            {
                if (this.TimeLeftWeek > Int32.MaxValue)
                {
                    double temp = Int32.MaxValue;
                    this.TimeLeft -= Int32.MaxValue;

                    if (Business.Market.TimerEventWeek != null && Business.Market.TimerEventWeek.Enabled)
                    {
                        Business.Market.TimerEventWeek.Stop();
                        Business.Market.TimerEventWeek.Enabled = false;
                        Business.Market.TimerEventWeek.Close();
                        Business.Market.TimerEventWeek.Dispose();

                        this.TimeEventExecuteWeek(temp);
                    }
                    else
                    {
                        this.TimeEventExecuteWeek(temp);
                    }                    

                    return;
                }
                else
                {
                    if (Business.Market.TimerEventWeek != null && Business.Market.TimerEventWeek.Enabled)
                    {
                        Business.Market.TimerEventWeek.Stop();
                        Business.Market.TimerEventWeek.Enabled = false;
                        Business.Market.TimerEventWeek.Close();
                        Business.Market.TimerEventWeek.Dispose();

                        this.TimeEventExecuteWeek(this.TimeLeftWeek);
                    }
                    else
                    {
                        this.TimeEventExecuteWeek(this.TimeLeftWeek);
                    }

                    this.TimeLeftWeek = 0;
                }
            }
            else
            {
                if (Business.Market.WeekEvent.Count > 0)
                {
                    this.TimeLeftWeek = 0;

                    Business.TimeEvent newTimeEventExe = new TimeEvent();
                    newTimeEventExe.EventType = Business.Market.WeekEvent[0].EventType;
                    newTimeEventExe.Every = Business.Market.WeekEvent[0].Every;
                    newTimeEventExe.IsEnable = Business.Market.WeekEvent[0].IsEnable;
                    newTimeEventExe.TargetFunction = Business.Market.WeekEvent[0].TargetFunction;
                    newTimeEventExe.Time = Business.Market.WeekEvent[0].Time;
                    newTimeEventExe.TimeEventID = Business.Market.WeekEvent[0].TimeEventID;
                    newTimeEventExe.TimeExecution = Business.Market.WeekEvent[0].TimeExecution;
                    newTimeEventExe.NumSession = Business.Market.WeekEvent[0].NumSession;

                    Business.Market.WeekEvent[0].TimeExecution = Business.Market.WeekEvent[0].TimeExecution.AddDays(7);

                    Business.TimeEvent tempnewTimeEventExe = new TimeEvent();
                    tempnewTimeEventExe.EventType = Business.Market.WeekEvent[0].EventType;
                    tempnewTimeEventExe.Every = Business.Market.WeekEvent[0].Every;
                    tempnewTimeEventExe.IsEnable = Business.Market.WeekEvent[0].IsEnable;
                    tempnewTimeEventExe.TargetFunction = Business.Market.WeekEvent[0].TargetFunction;
                    tempnewTimeEventExe.Time = Business.Market.WeekEvent[0].Time;
                    tempnewTimeEventExe.TimeEventID = Business.Market.WeekEvent[0].TimeEventID;
                    tempnewTimeEventExe.TimeExecution = Business.Market.WeekEvent[0].TimeExecution;
                    tempnewTimeEventExe.NumSession = Business.Market.WeekEvent[0].NumSession;
                    
                    Business.Market.WeekEvent.RemoveAt(0);

                    Business.Market.WeekEvent.Add(tempnewTimeEventExe);

                    //Calculation Time Sleep Of Event Week
                    DateTime TimeCurrent = DateTime.Now;
                    TimeSpan timeSpan = Business.Market.WeekEvent[0].TimeExecution - TimeCurrent;

                    System.Threading.Thread newThread = new Thread(new ParameterizedThreadStart(this.ThreadExecutionEventWeek));
                    newThread.Start(newTimeEventExe);

                    if (Business.Market.TimerEventWeek != null && Business.Market.TimerEventWeek.Enabled)
                    {
                        Business.Market.TimerEventWeek.Stop();
                        Business.Market.TimerEventWeek.Enabled = false;
                        Business.Market.TimerEventWeek.Close();
                        Business.Market.TimerEventWeek.Dispose();

                        this.TimeEventExecuteWeek(timeSpan.TotalSeconds);
                    }
                    else
                    {
                        this.TimeEventExecuteWeek(timeSpan.TotalSeconds);
                    }
                }
            }
        }

        /// <summary>
        /// THREAD EXECUTION EVENT WEEK
        /// </summary>
        private void ThreadExecutionEventWeek(object TimeEvent)
        {
            return;
            while (Business.Market.IsExecutorWeek)
            {
                System.Threading.Thread.Sleep(500);
            }

            Business.Market.IsExecutorWeek = true;

            Business.TimeEvent TimeEventExe = TimeEvent as Business.TimeEvent;
            if (TimeEventExe != null)
            {
                if (TimeEventExe.TargetFunction != null)
                {
                    int count = TimeEventExe.TargetFunction.Count;
                    for (int i = 0; i < count; i++)
                    {
                        TimeEventExe.TargetFunction[i].Function(TimeEventExe.TargetFunction[i].EventPosition, TimeEventExe);
                    }
                }
            }

            Business.Market.IsExecutorWeek = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessSortTimeEventWeek()
        {
            return;
            DateTime TimeCurrent = DateTime.Now;

            try
            {
                for (int i = 0; i < Business.Market.WeekEvent.Count; i++)
                {
                    #region CALCULATION TIME POSITION ONE
                    int DayOfWeekFirst = Business.Market.WeekEvent[i].Time.DayInWeek - TimeCurrent.DayOfWeek;
                    if (DayOfWeekFirst < 0)
                    {
                        DayOfWeekFirst += 7;
                    }

                    if (Business.Market.WeekEvent[i].Time.Hour == 24)
                    {
                        Business.Market.WeekEvent[i].TimeExecution = new DateTime(TimeCurrent.Year, TimeCurrent.Month, TimeCurrent.Day,
                            23, 59, 59);
                    }
                    else
                    {
                        if (Business.Market.WeekEvent[i].Time.Minute >= 60)
                        {
                            Business.Market.WeekEvent[i].TimeExecution = new DateTime(TimeCurrent.Year, TimeCurrent.Month, TimeCurrent.Day,
                                Business.Market.WeekEvent[i].Time.Hour, 59, 59);
                        }
                        else
                        {
                            Business.Market.WeekEvent[i].TimeExecution = new DateTime(TimeCurrent.Year, TimeCurrent.Month, TimeCurrent.Day,
                                Business.Market.WeekEvent[i].Time.Hour, Business.Market.WeekEvent[i].Time.Minute, 00);
                        }
                    }

                    Business.Market.WeekEvent[i].TimeExecution = Business.Market.WeekEvent[i].TimeExecution.AddDays(DayOfWeekFirst);

                    TimeSpan tsFirst = Business.Market.WeekEvent[i].TimeExecution - TimeCurrent;
                    if (tsFirst.TotalSeconds < 0)
                    {
                        Business.Market.WeekEvent[i].TimeExecution = Business.Market.WeekEvent[i].TimeExecution.AddDays(7);
                        tsFirst = new TimeSpan();
                        tsFirst = Business.Market.WeekEvent[i].TimeExecution - TimeCurrent;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }

            #region PROCESS REMOVE TIME EVENT WITH START EVENT = END EVENT
            for (int i = 0; i < Business.Market.WeekEvent.Count; i++)
            {
                for (int j = i + 1; j < Business.Market.WeekEvent.Count; j++)
                {
                    if (Business.Market.WeekEvent[i].TimeEventID == Business.Market.WeekEvent[j].TimeEventID)
                    {
                        DateTime newDateTimeOne = DateTime.Now;
                        DateTime newDateTimeTwo = DateTime.Now;

                        if (Business.Market.WeekEvent[i].Time.Hour == 24)
                        {                            
                            newDateTimeOne = Business.Market.WeekEvent[i].TimeExecution.AddSeconds(1);
                        }
                        else
                        {
                            newDateTimeOne = Business.Market.WeekEvent[i].TimeExecution;
                        }

                        if (Business.Market.WeekEvent[j].Time.Hour == 24)
                        {                            
                            newDateTimeTwo = Business.Market.WeekEvent[j].TimeExecution.AddSeconds(1);
                        }
                        else
                        {
                            newDateTimeTwo = Business.Market.WeekEvent[j].TimeExecution;
                        }

                        if (newDateTimeOne == newDateTimeTwo)
                        {
                            Business.Market.WeekEvent.Remove(Business.Market.WeekEvent[j]);
                            Business.Market.WeekEvent.Remove(Business.Market.WeekEvent[i]);
                            j = 0;
                            i = 0;                            
                        }
                    }
                }
            }
            #endregion

            #region GROUP EVENT WITH TIME EVENT = TIME EVENT
            for (int i = 0; i < Business.Market.WeekEvent.Count; i++)
            {
                for (int j = i + 1; j < Business.Market.WeekEvent.Count; j++)
                {
                    if (Business.Market.WeekEvent[i].Time.DayInWeek == Business.Market.WeekEvent[j].Time.DayInWeek &&
                        Business.Market.WeekEvent[i].Time.Hour == Business.Market.WeekEvent[j].Time.Hour &&
                        Business.Market.WeekEvent[i].Time.Minute == Business.Market.WeekEvent[j].Time.Minute)
                    {
                        if (Business.Market.WeekEvent[i].TargetFunction != null)
                        {
                            bool isExist = false;
                            int countWeekEventOne = Business.Market.WeekEvent[i].TargetFunction.Count;
                            for (int n = 0; n < countWeekEventOne; n++)
                            {
                                if (Business.Market.WeekEvent[j].TargetFunction[0].EventPosition == Business.Market.WeekEvent[i].TargetFunction[n].EventPosition &&
                                    Business.Market.WeekEvent[j].EventType == Business.Market.WeekEvent[i].EventType)
                                {
                                    isExist = true;
                                    break;
                                }
                            }

                            if (!isExist)
                            {
                                if (Business.Market.WeekEvent[j].TargetFunction != null)
                                {
                                    int count = Business.Market.WeekEvent[j].TargetFunction.Count;
                                    for (int n = 0; n < count; n++)
                                    {
                                        Business.Market.WeekEvent[i].TargetFunction.Add(Business.Market.WeekEvent[j].TargetFunction[n]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Business.Market.WeekEvent[i].TargetFunction = new List<TargetFunction>();
                            int count = Business.Market.WeekEvent[j].TargetFunction.Count;
                            for (int n = 0; n < count; n++)
                            {
                                Business.Market.WeekEvent[i].TargetFunction.Add(Business.Market.WeekEvent[j].TargetFunction[n]);
                            }
                        }

                        Business.Market.WeekEvent.RemoveAt(j);
                        j = i;
                    }
                }
            }
            #endregion

            #region Process Sort List Week Event
            if (Business.Market.WeekEvent != null)
            {
                int count = Business.Market.WeekEvent.Count;
                for (int i = 0; i < Business.Market.WeekEvent.Count; i++)
                {
                    for (int j = i + 1; j < Business.Market.WeekEvent.Count; j++)
                    {                       
                        TimeSpan tsFirst = Business.Market.WeekEvent[i].TimeExecution - TimeCurrent;
                        
                        TimeSpan tsSecond = Business.Market.WeekEvent[j].TimeExecution - TimeCurrent;
                        
                        if (tsFirst.TotalSeconds > tsSecond.TotalSeconds)
                        {
                            Business.TimeEvent newTimeEvent = new TimeEvent();
                            newTimeEvent = Business.Market.WeekEvent[i];
                            Business.Market.WeekEvent[i] = Business.Market.WeekEvent[j];
                            Business.Market.WeekEvent[j] = newTimeEvent;
                        }                                                        
                    }
                }
            }
            #endregion

            TimeSpan tsExecutor = Business.Market.WeekEvent[0].TimeExecution - TimeCurrent;             

            if (Business.Market.TimerEventWeek != null && Business.Market.TimerEventWeek.Enabled)
            {
                Business.Market.TimerEventWeek.Stop();
                Business.Market.TimerEventWeek.Enabled = false;
                Business.Market.TimerEventWeek.Close();
                Business.Market.TimerEventWeek.Dispose();

                this.TimeEventExecuteWeek(tsExecutor.TotalSeconds);
            }
            else
            {                
                this.TimeEventExecuteWeek(tsExecutor.TotalSeconds);
            }
        }

        //=====================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeSleep"></param>
        internal void TimeEventExecuteYear(double TimeSleep)
        {
            return;
            Business.Market.TimerEventYear = new System.Timers.Timer(1000);            
            Double TimeTemp = TimeSleep * 1000;

            if (TimeTemp > 0)
            {
                if (TimeTemp > Int32.MaxValue)
                {
                    double temp = Int32.MaxValue;
                    this.TimeLeft = TimeTemp - temp;

                    Business.Market.TimerEventYear.Interval = temp;                    
                }
                else
                {
                    Business.Market.TimerEventYear.Interval = TimeTemp;                    
                }  
            }

            Business.Market.TimerEventYear.Elapsed += new System.Timers.ElapsedEventHandler(ExecutionEventYear);
            Business.Market.TimerEventYear.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExecutionEventYear(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
            if (this.TimeLeft > 0)
            {
                if (this.TimeLeft > Int32.MaxValue)
                {
                    double temp = Int32.MaxValue;
                    this.TimeLeft -= Int32.MaxValue;
                    this.TimeEventExecuteYear(temp);

                    return;
                }
            }
            else
            {
                this.TimeLeft = 0;

                if (Business.Market.YearEvent[0].Every == true)
                {
                    if (Business.Market.YearEvent[0].Time.Year < 0)
                    {                        
                        Business.Market.YearEvent[0].TimeExecution = Business.Market.YearEvent[0].TimeExecution.AddYears(1);
                    }
                }
                else if (Business.Market.YearEvent[0].Time.Year == 0)
                {
                    if (Business.Market.YearEvent[0].Time.Day > 1)
                    {
                        DateTime temp = new DateTime(DateTime.Now.Year, Business.Market.YearEvent[0].Time.Month, 1, 00, 00, 00);
                        DateTime tempDate = temp.AddMonths(1);
                        DateTime EndofMonnt = Model.CommandFramework.CommandFrameworkInstance.GetLastDayOfMonth(tempDate);

                        Business.Market.YearEvent[0].Time.Month = EndofMonnt.Month;
                        Business.Market.YearEvent[0].Time.Day = EndofMonnt.Day;
                        Business.Market.YearEvent[0].Time.Hour = this.EndOfDay.Hour;
                        Business.Market.YearEvent[0].Time.Minute = this.EndOfDay.Minute;

                        DateTime TimeExe = new DateTime(DateTime.Now.Year, Business.Market.YearEvent[0].Time.Month,
                            Business.Market.YearEvent[0].Time.Day, Business.Market.YearEvent[0].Time.Hour,
                            Business.Market.YearEvent[0].Time.Minute, 00, 00);

                        Business.Market.YearEvent[0].TimeExecution = TimeExe;
                    }
                    else
                    {
                        DateTime temp = new DateTime(DateTime.Now.Year, Business.Market.YearEvent[0].Time.Month, 1, 00, 00, 00);
                        DateTime tempDate = temp.AddMonths(1);
                        DateTime FirstOfDay = Model.CommandFramework.CommandFrameworkInstance.GetFirstDayOfMonth(tempDate);

                        Business.Market.YearEvent[0].Time.Month = FirstOfDay.Month;
                        Business.Market.YearEvent[0].Time.Day = FirstOfDay.Day;
                        Business.Market.YearEvent[0].Time.Hour = 00;
                        Business.Market.YearEvent[0].Time.Minute = 00;

                        DateTime TimeExe = new DateTime(DateTime.Now.Year, Business.Market.YearEvent[0].Time.Month,
                            Business.Market.YearEvent[0].Time.Day, Business.Market.YearEvent[0].Time.Hour,
                            Business.Market.YearEvent[0].Time.Minute, 00, 00);

                        Business.Market.YearEvent[0].TimeExecution = TimeExe;
                    }
                }

                Business.TimeEvent newTimeEvent = new TimeEvent();
                newTimeEvent.EventType = Business.Market.YearEvent[0].EventType;
                newTimeEvent.Every = Business.Market.YearEvent[0].Every;
                newTimeEvent.IsEnable = Business.Market.YearEvent[0].IsEnable;
                newTimeEvent.TargetFunction = Business.Market.YearEvent[0].TargetFunction;
                newTimeEvent.Time = Business.Market.YearEvent[0].Time;
                newTimeEvent.TimeEventID = Business.Market.YearEvent[0].TimeEventID;
                newTimeEvent.TimeExecution = Business.Market.YearEvent[0].TimeExecution;

                this.ProcessSortTimeEventYear();

                System.Threading.Thread newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.ThreadExecutionEventYear));
                newThread.Start(newTimeEvent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadExecutionEventYear(object TimeEvent)
        {
            return;
            while (Business.Market.IsExecutorYear)
            {
                System.Threading.Thread.Sleep(500);
            }

            Business.Market.IsExecutorYear = true;

            Business.TimeEvent TimeEventExe = TimeEvent as Business.TimeEvent;

            if (TimeEventExe != null)
            {
                if (TimeEventExe.TargetFunction != null)
                {
                    int count = TimeEventExe.TargetFunction.Count;
                    for (int i = 0; i < count; i++)
                    {
                        TimeEventExe.TargetFunction[i].Function(TimeEventExe.TargetFunction[i].EventPosition, TimeEventExe);
                    }
                }
            }

            Business.Market.IsExecutorYear = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessSortTimeEventYear()
        {
            return;
            DateTime TimeCurrent = DateTime.Now;

            #region Process Sort List Year Event
            if (Business.Market.YearEvent != null)
            {
                int count = Business.Market.YearEvent.Count;
                for (int i = 0; i < Business.Market.YearEvent.Count; i++)
                {
                    for (int j = i + 1; j < Business.Market.YearEvent.Count; j++)
                    {
                        DateTime TimeFirst = new DateTime();
                        TimeSpan totalTimeFirst = new TimeSpan();

                        if (Business.Market.YearEvent[i].Time.Year > 0)
                        {
                            #region CHECK IF YEAR > 0 BUT YEAR < YEAR CURRENT
                            if (Business.Market.YearEvent[i].Time.Hour == 24)
                            {
                                TimeFirst = new DateTime(Business.Market.YearEvent[i].Time.Year, Business.Market.YearEvent[i].Time.Month,
                                                                Business.Market.YearEvent[i].Time.Day, 23, 59, 59);
                            }
                            else
                            {
                                TimeFirst = new DateTime(TimeCurrent.Year, Business.Market.YearEvent[i].Time.Month, Business.Market.YearEvent[i].Time.Day,
                                                       Business.Market.YearEvent[i].Time.Hour, Business.Market.YearEvent[i].Time.Minute, 00);
                            }

                            totalTimeFirst = TimeFirst - TimeCurrent;
                            Business.Market.YearEvent[i].TimeExecution = TimeFirst;                           
                            #endregion
                        }
                        else
                        {
                            #region CHECK YEAR <= 0 BUT IF TIME SPAND TIME SPAND CURRENT THEN ADD YEAR
                            if (Business.Market.YearEvent[i].Time.Hour == 24)
                            {
                                TimeFirst = new DateTime(TimeCurrent.Year, Business.Market.YearEvent[i].Time.Month, Business.Market.YearEvent[i].Time.Day,
                                                      23, 59, 59);
                            }
                            else
                            {
                                TimeFirst = new DateTime(TimeCurrent.Year, Business.Market.YearEvent[i].Time.Month, Business.Market.YearEvent[i].Time.Day,
                                                      Business.Market.YearEvent[i].Time.Hour, Business.Market.YearEvent[i].Time.Minute, 00);
                            }                           

                            totalTimeFirst = TimeFirst - TimeCurrent;

                            if (totalTimeFirst.TotalMilliseconds < 0)
                            {
                                if (Business.Market.YearEvent[i].Time.Year < 0)
                                {
                                    TimeFirst = TimeFirst.AddYears(1);                                    
                                }
                                else
                                {
                                    TimeFirst = TimeFirst.AddMonths(1);                                    
                                }

                                totalTimeFirst = TimeFirst - TimeCurrent;
                            }

                            Business.Market.YearEvent[i].TimeExecution = TimeFirst;                            
                            #endregion
                        } 

                        //===================================
                        DateTime TimeSecond = new DateTime();
                        TimeSpan totalTimeSecond = new TimeSpan();
                        if (Business.Market.YearEvent[j].Time.Year > 0)
                        {
                            #region CHECK YEAR > 0 BUT YEAR < YEAR CURRENT
                            if (Business.Market.YearEvent[j].Time.Hour == 24)
                            {
                                TimeSecond = new DateTime(Business.Market.YearEvent[j].Time.Year, Business.Market.YearEvent[j].Time.Month,
                                                        Business.Market.YearEvent[j].Time.Day, 23, 59, 59);
                            }
                            else
                            {
                                TimeSecond = new DateTime(Business.Market.YearEvent[j].Time.Year, Business.Market.YearEvent[j].Time.Month,
                                                        Business.Market.YearEvent[j].Time.Day, Business.Market.YearEvent[j].Time.Hour,
                                                        Business.Market.YearEvent[j].Time.Minute, 00);
                            }                            

                            totalTimeSecond = TimeSecond - TimeCurrent;
                            Business.Market.YearEvent[j].TimeExecution = TimeSecond;                                                     
                            #endregion                            
                        }
                        else
                        {
                            #region ELSE TIME YEAR < 0
                            if (Business.Market.YearEvent[j].Time.Hour == 24)
                            {
                                TimeSecond = new DateTime(TimeCurrent.Year, Business.Market.YearEvent[j].Time.Month,
                                                        Business.Market.YearEvent[j].Time.Day, 23, 59, 59);
                            }
                            else
                            {
                                TimeSecond = new DateTime(TimeCurrent.Year, Business.Market.YearEvent[j].Time.Month,
                                                           Business.Market.YearEvent[j].Time.Day, Business.Market.YearEvent[j].Time.Hour,
                                                           Business.Market.YearEvent[j].Time.Minute, 00);
                            }                            

                            totalTimeSecond = TimeSecond - TimeCurrent;

                            if (totalTimeSecond.TotalMilliseconds < 0)
                            {
                                if (Business.Market.YearEvent[j].Time.Year < 0)
                                {
                                    TimeSecond = TimeSecond.AddYears(1);
                                }
                                else
                                {
                                    TimeSecond = TimeSecond.AddMonths(1);
                                }

                                totalTimeSecond = TimeSecond - TimeCurrent;
                            }

                            Business.Market.YearEvent[j].TimeExecution = TimeSecond;                            
                            #endregion                            
                        }

                        if (totalTimeFirst.TotalMilliseconds > 0 && totalTimeSecond.TotalMilliseconds > 0)
                        {
                            if (Business.Market.YearEvent[i].IsEnable == true && Business.Market.YearEvent[j].IsEnable == true)
                            {
                                if (totalTimeFirst > totalTimeSecond)
                                {
                                    #region IF TIME FIRST > TIME SECOND
                                    Business.TimeEvent newTimeEvent = new TimeEvent();
                                    newTimeEvent = Business.Market.YearEvent[i];
                                    Business.Market.YearEvent[i] = Business.Market.YearEvent[j];
                                    Business.Market.YearEvent[j] = newTimeEvent;
                                    #endregion
                                }
                                else if (totalTimeFirst == totalTimeSecond)
                                {
                                    #region CHECK TIME FIRST == TIME SECOND
                                    if (Business.Market.YearEvent[i].Time.Hour > Business.Market.YearEvent[j].Time.Hour)
                                    {
                                        Business.TimeEvent newTimeEvent = new TimeEvent();
                                        newTimeEvent = Business.Market.YearEvent[i];
                                        Business.Market.YearEvent[i] = Business.Market.YearEvent[j];
                                        Business.Market.YearEvent[j] = newTimeEvent;
                                    }
                                    else
                                    {
                                        #region GROUP EVENT WITH TIME EQUAL
                                        if (Business.Market.YearEvent[i].Time.Year == Business.Market.YearEvent[j].Time.Year &&
                                            Business.Market.YearEvent[i].Time.Month == Business.Market.YearEvent[j].Time.Month &&
                                            Business.Market.YearEvent[i].Time.Day == Business.Market.YearEvent[j].Time.Day &&
                                            Business.Market.YearEvent[i].Time.Hour == Business.Market.YearEvent[j].Time.Hour &&
                                            Business.Market.YearEvent[i].Time.Minute == Business.Market.YearEvent[j].Time.Minute)
                                        {
                                            if (Business.Market.YearEvent[j].TargetFunction != null)
                                            {
                                                int countTarget = Business.Market.YearEvent[j].TargetFunction.Count;
                                                for (int n = 0; n < countTarget; n++)
                                                {
                                                    Business.Market.YearEvent[i].TargetFunction.Add(Business.Market.YearEvent[j].TargetFunction[n]);
                                                }
                                            }

                                            Business.Market.YearEvent.RemoveAt(j);

                                            j--;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (Business.Market.YearEvent[i].IsEnable == false)
                                {
                                    Business.TimeEvent newTimeEvent = new TimeEvent();
                                    newTimeEvent = Business.Market.YearEvent[i];
                                    Business.Market.YearEvent[i] = Business.Market.YearEvent[j];
                                    Business.Market.YearEvent[j] = newTimeEvent;
                                }
                            }                            
                        }
                        else
                        {
                            #region ELSE TIME FIRST < 0
                            if (totalTimeFirst.TotalMilliseconds < 0)
                            {
                                Business.TimeEvent newTimeEvent = new TimeEvent();
                                newTimeEvent = Business.Market.YearEvent[i];
                                Business.Market.YearEvent[i] = Business.Market.YearEvent[j];
                                Business.Market.YearEvent[j] = newTimeEvent;
                            }
                            #endregion                           
                        }                        
                    }
                }
            }
            #endregion

            //Calculation Time Sleep            
            DateTime TimeSleep = new DateTime(Business.Market.YearEvent[0].TimeExecution.Year, Business.Market.YearEvent[0].TimeExecution.Month, 
                Business.Market.YearEvent[0].TimeExecution.Day, Business.Market.YearEvent[0].TimeExecution.Hour, 
                Business.Market.YearEvent[0].TimeExecution.Minute, 00);

            TimeSpan temp = TimeSleep - TimeCurrent;

            if (temp.TotalSeconds < 0)
                TimeSleep = TimeSleep.AddMonths(1);

            TimeSpan TimeSpanSleep = TimeSleep - TimeCurrent;

            if (Business.Market.TimerEventYear != null && Business.Market.TimerEventYear.Enabled)
            {
                Business.Market.TimerEventYear.Stop();
                Business.Market.TimerEventYear.Enabled = false;
                Business.Market.TimerEventYear.Close();
                Business.Market.TimerEventYear.Dispose();

                this.TimeEventExecuteYear(TimeSpanSleep.TotalSeconds);
            }
            else
            {
                this.TimeEventExecuteYear(TimeSpanSleep.TotalSeconds);
            }
        }
        
        //=============================================================

        /// <summary>
        /// 
        /// </summary>
        private void ProcessSortTimeFutureEvent()
        {
            return;
            if (Business.Market.FutureEvent != null && Business.Market.FutureEvent.Count > 0)
            {
                for (int i = 0; i < Business.Market.FutureEvent.Count; i++)
                {
                    for (int j = i + 1; j < Business.Market.FutureEvent.Count; j++)
                    {
                        TimeSpan timeSpan = Business.Market.FutureEvent[j].TimeExecution - Business.Market.FutureEvent[i].TimeExecution;
                        if (timeSpan.TotalMilliseconds < 0)
                        {
                            if (Business.Market.FutureEvent[i].IsEnable && Business.Market.FutureEvent[j].IsEnable)
                            {
                                Business.TimeEvent newTimeEvent = new TimeEvent();
                                newTimeEvent = Business.Market.FutureEvent[i];
                                Business.Market.FutureEvent[i] = Business.Market.FutureEvent[j];
                                Business.Market.FutureEvent[j] = newTimeEvent;
                            }
                            else
                            {
                                if (Business.Market.FutureEvent[j].IsEnable)
                                {
                                    Business.TimeEvent newTimeEvent = new TimeEvent();
                                    newTimeEvent = Business.Market.FutureEvent[i];
                                    Business.Market.FutureEvent[i] = Business.Market.FutureEvent[j];
                                    Business.Market.FutureEvent[j] = newTimeEvent;
                                }
                            }
                        }
                        else
                        {
                            if (!Business.Market.FutureEvent[i].IsEnable)
                            {                                
                                if (!Business.Market.FutureEvent[i].IsEnable)
                                {
                                    if (Business.Market.FutureEvent[j].IsEnable)
                                    {
                                        Business.TimeEvent newTimeEvent = new TimeEvent();
                                        newTimeEvent = Business.Market.FutureEvent[i];
                                        Business.Market.FutureEvent[i] = Business.Market.FutureEvent[j];
                                        Business.Market.FutureEvent[j] = newTimeEvent;
                                    }
                                }
                            }
                        }
                    }
                }

                //Calculation Time Sleep            
                DateTime TimeCurrent = DateTime.Now;

                TimeSpan TimeSpanSleep = Business.Market.FutureEvent[0].TimeExecution - TimeCurrent;

                if (TimeSpanSleep.TotalSeconds > 0)
                {
                    if (Business.Market.FutureEvent[0].IsEnable)
                    {
                        if (Business.Market.TimerEventFuture != null && Business.Market.TimerEventFuture.Enabled)
                        {
                            Business.Market.TimerEventFuture.Stop();
                            Business.Market.TimerEventFuture.Enabled = false;
                            Business.Market.TimerEventFuture.Close();
                            Business.Market.TimerEventFuture.Dispose();

                            this.TimerEventExecuteFuture(TimeSpanSleep.TotalSeconds);
                        }
                        else
                        {
                            this.TimerEventExecuteFuture(TimeSpanSleep.TotalSeconds);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSleep"></param>
        internal void TimerEventExecuteFuture(double timeSleep)
        {
            return;
            Business.Market.TimerEventFuture = new System.Timers.Timer(1000);            
            Double TimeTemp = timeSleep * 1000;

            if (TimeTemp > Int32.MaxValue)
            {
                double temp = Int32.MaxValue;
                this.TimeLeftFuture = TimeTemp - temp;

                Business.Market.TimerEventFuture.Interval = temp;                
            }
            else
            {
                Business.Market.TimerEventFuture.Interval = TimeTemp;                
            }

            Business.Market.TimerEventFuture.Elapsed += new System.Timers.ElapsedEventHandler(TimerEventFuture_Elapsed);
            Business.Market.TimerEventFuture.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerEventFuture_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
            if (this.TimeLeftFuture > 0)
            {
                if (this.TimeLeftFuture > Int32.MaxValue)
                {
                    double temp = Int32.MaxValue;
                    this.TimeLeftFuture -= Int32.MaxValue;
                    this.TimerEventExecuteFuture(temp);

                    return;
                }
            }
            else
            {
                Business.TimeEvent newTimeEventExe = new TimeEvent();
                newTimeEventExe.EventType = Business.Market.FutureEvent[0].EventType;
                newTimeEventExe.Every = Business.Market.FutureEvent[0].Every;
                newTimeEventExe.IsEnable = Business.Market.FutureEvent[0].IsEnable;
                newTimeEventExe.TargetFunction = Business.Market.FutureEvent[0].TargetFunction;
                newTimeEventExe.Time = Business.Market.FutureEvent[0].Time;
                newTimeEventExe.TimeEventID = Business.Market.FutureEvent[0].TimeEventID;
                newTimeEventExe.TimeExecution = Business.Market.FutureEvent[0].TimeExecution;
                Business.Market.FutureEvent[0].IsEnable = false;

                this.ProcessSortTimeFutureEvent();

                System.Threading.Thread newThread = new Thread(new ParameterizedThreadStart(this.ThreadExeFutureEvent));
                newThread.Start(newTimeEventExe);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeEvent"></param>
        private void ThreadExeFutureEvent(object timeEvent)
        {
            return;
            Business.TimeEvent TimeEventExe = timeEvent as Business.TimeEvent;
            if (TimeEventExe != null)
            {
                if (TimeEventExe.TargetFunction != null)
                {
                    int count = TimeEventExe.TargetFunction.Count;
                    for (int i = 0; i < count; i++)
                    {
                        TimeEventExe.TargetFunction[i].Function(TimeEventExe.TargetFunction[i].EventPosition, TimeEventExe);
                    }
                }
            }
        }

        //===================================================================================
        //===================================================================================

        /// <summary>
        /// TIME EVENT EXECUTE DAY
        /// </summary>
        /// <param name="TimeSleep"></param>
        internal void TimeEventExecuteCandles(double TimeSleep)
        {
            Business.Market.TimerEventCandles = new System.Timers.Timer(1000);
            Business.Market.TimerEventCandles.Interval = TimeSleep;
            Business.Market.TimerEventCandles.Elapsed += new System.Timers.ElapsedEventHandler(ExecutionEventCandles);
            Business.Market.TimerEventCandles.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecutionEventCandles(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Call Thread Process Event
            System.Threading.Thread newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.ThreadExecutionEventCandles));
            newThread.Start(Business.Market.EventGetCandles);

            Business.Market.EventGetCandles.TimeExecution.AddDays(1);

            TimeSpan TimeExe = Business.Market.EventGetCandles.TimeExecution - DateTime.Now;

            if (Business.Market.TimerEventCandles != null && Business.Market.TimerEventCandles.Enabled)
            {
                Business.Market.TimerEventCandles.Stop();
                Business.Market.TimerEventCandles.Enabled = false;
                Business.Market.TimerEventCandles.Close();
                Business.Market.TimerEventCandles.Dispose();

                this.TimeEventExecuteCandles(TimeExe.TotalMilliseconds);
            }
            else
            {
                this.TimeEventExecuteCandles(TimeExe.TotalMilliseconds);
            }
        }

        /// <summary>
        /// THREAD EXECUTION EVENT DAY 
        /// </summary>
        private void ThreadExecutionEventCandles(object TimeEvent)
        {
            Business.TimeEvent TimeEventExe = TimeEvent as Business.TimeEvent;

            if (TimeEventExe != null)
            {
                if (TimeEventExe.TargetFunction != null && TimeEventExe.TargetFunction.Count > 0)
                {
                    int count = TimeEventExe.TargetFunction.Count;
                    for (int i = 0; i < count; i++)
                    {
                        TimeEventExe.TargetFunction[i].Function(TimeEventExe.TargetFunction[i].EventPosition, TimeEventExe);
                    }
                }
            }

            Business.Market.IsExecutorDay = false;
        }
    }
}
