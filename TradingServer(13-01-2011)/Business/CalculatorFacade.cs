﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingServer.Business
{   
    public delegate void TaskDelegate(Business.Tick objTick);
    public delegate void TaskComplete();
    //begin class code
    //
    class CalculatorFacade
    {
        public static bool IsBlock { get; set; }

        #region static     
        private static CalculatorFacade calculatorFacade;   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static bool SetTask(IPresenter.ICalculatorClient Client)
        {            
            return CalculatorFacade.calculatorFacade.SetCalculatorTask(Client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCalculatorInfo()
        {
            List<string> Result = new List<string>();
            if (calculatorFacade.Calculators != null)
            {
                int count = calculatorFacade.Calculators.Count;
                for (int i = 0; i < count; i++)
                {
                    string Message = calculatorFacade.Calculators[i].TaskName + " "  + calculatorFacade.Calculators[i].IsRunning.ToString() + " " +
                        calculatorFacade.Calculators[i].TaskName + " " + calculatorFacade.QueueTask.Count ;

                    Result.Add(Message);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal static List<string> GetTopQueueTask(int from, int to)
        {
            List<string> result = new List<string>();
            List<IPresenter.ICalculatorClient> tempCalculator = calculatorFacade.QueueTask;
            if (tempCalculator != null)
            {
                int count = tempCalculator.Count;
                if (count < to)
                    to = count;

                for (int i = from; i < to; i++)
                {
                    string Message = tempCalculator[i].TaskName + " " + tempCalculator[i].IsActive.ToString() + " " +
                        tempCalculator[i].TaskName + " " + tempCalculator.Count;

                    result.Add(Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetQueueNum()
        {
            return calculatorFacade.QueueTask.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FreeCalculator()
        {
            int count = calculatorFacade.Calculators.Count;
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (!calculatorFacade.Calculators[i].IsRunning)
                {
                    num++;
                }
            }
            return num;
        }
        #endregion      

        #region variable
        private Thread controlThread;
        private int defaultCalculatorNumber = 100;
        private List<IPresenter.ICalculatorClient> QueueTask = new List<IPresenter.ICalculatorClient>();

        /// <summary>
        /// default is true, if set false, exist control task
        /// </summary>
        private bool isStartControlTask = true;

        /// <summary>
        /// 
        /// </summary>
        internal bool isPauseControlTask = false;

        private bool isSettingCalculatorTask = true;
        private bool is1 = true;
        public List<Calculator> Calculators = new List<Calculator>();
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private CalculatorFacade()
        {
            this.IniMyClass();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Client"></param>
        public bool SetCalculatorTask(IPresenter.ICalculatorClient Client)
        {
            //Client.IsActive = true;

            int count = this.Calculators.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.Calculators[i].TaskName == Client.TaskName && this.Calculators[i].IsRunning)
                    return false;
            }

            this.QueueTask.Add(Client);
                        
            if (this.isPauseControlTask)
            {
                //wake up control task
                //
                this.isPauseControlTask = false;
            }
            this.isSettingCalculatorTask = false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void IniCalculatorFacade()
        {
            CalculatorFacade.calculatorFacade = new CalculatorFacade();
        }

        /// <summary>
        /// all calculator call this function when it finish its job.
        /// </summary>
        /// <param name="CalculatorObj">calculator object done job</param>
        public void CalculatorFreeNotify(Calculator CalculatorObj)
        {
            if (this.isPauseControlTask)
            {
                //wake up control task
                //
                this.isPauseControlTask = false;
            }
        }

        #region private function
        /// <summary>
        /// 
        /// </summary>
        private void IniMyClass()
        {
            Console.WriteLine("ini calculator thread");
            for (int i = 0; i < this.defaultCalculatorNumber; i++)
            {
                Calculator cal = new Calculator(this);
                this.Calculators.Add(cal);
            }

            if (this.controlThread == null)
            {
                this.controlThread = new Thread(new ThreadStart(this.ControlTask));
                this.controlThread.Start();
            }

            this.isSettingCalculatorTask = false;
            Console.WriteLine("ini control thread");
        }

        /// <summary>
        /// 
        /// </summary>
        private void ControlTask()
        {
            //System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            //conn.Open();
            //string Cmd = "insert into ApplicationError(Name,Description,DateTime) VALUES(" + "'Control Task'" + "," + "'Check Multithread in Control Task'" + "," + "'" + DateTime.Now.ToString() + "'" + ")";
            //System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(Cmd, conn);
            //command.ExecuteNonQuery();
            //conn.Close();

            //begin while code
            //
            //string content = "Init Control Task";
            //TradingServer.Facade.FacadeAddNewSystemLog(5, content, "", "", "");
            while (this.isStartControlTask)
            {
                while (this.isPauseControlTask)
                {
                    Thread.Sleep(1);
                }                

                int countQueueTask = this.QueueTask.Count;
                if (countQueueTask == 0)
                {
                    this.isPauseControlTask = true;
                    continue;
                }

                int count = this.Calculators.Count;
                while (0 < this.QueueTask.Count)
                {
                    if (count > 0)
                    {
                        //check task name
                        //
                        if (this.Calculators != null)
                        {
                            if (this.QueueTask[0] != null)
                            {
                                #region CHECK NAME OF QUEUE TASK		 
                                bool CheckName = false;
                                int countCa = this.Calculators.Count;
                                for (int i = 0; i < countCa; i++)
                                {
                                    string QueueTaskName = this.QueueTask[0].TaskName;
                                    string CalTaskName = this.Calculators[i].TaskName;
                                    if (CalTaskName == QueueTaskName)
                                    {
                                        try
                                        {
                                            this.QueueTask.RemoveAt(0);
                                        }
                                        catch (Exception ex)
                                        {
                                            //string content1 = "Task Name: " + this.QueueTask[0].TaskName + " Total Queue Task: " + this.QueueTask.Count + " Is Active: " + this.QueueTask[0].IsActive;
                                            //TradingServer.Facade.FacadeAddNewSystemLog(5, content1, ex.ToString(), "0", "");
                                        }

                                        CheckName = true;
                                        break;
                                    }
                                }

                                if (CheckName == true)
                                {
                                    continue;
                               } 
                                #endregion                                
                            }
                            else
                            {
                                #region REMOVE QUEUE TASK IS NULL		 
                                try
                                {
                                    this.QueueTask.RemoveAt(0);
                                }
                                catch (Exception ex)
                                {
                                    //string content1 = "Task Name: " + this.QueueTask[0].TaskName + " Total Queue Task: " + this.QueueTask.Count + " Is Active: " + this.QueueTask[0].IsActive;
                                    //TradingServer.Facade.FacadeAddNewSystemLog(5, content1, ex.ToString(), "1", "");
                                }
                                continue;
                                #endregion                                
                            }
                        }
                    }

                    #region process
                    bool check = false;
                    for (int i = 0; i < this.Calculators.Count; i++)
                    {
                        if (!this.Calculators[i].IsRunning)
                        {
                            if (this.QueueTask[0] != null)
                            {                                
                                this.QueueTask[0].IsActive = true;
                                IPresenter.ICalculatorClient tempTask = this.QueueTask[0];
                                //this.Calculators[i].Start(this.QueueTask[0]);
                                try
                                {
                                    this.QueueTask.RemoveAt(0);
                                    this.Calculators[i].Start(tempTask);
                                }
                                catch (Exception ex)
                                {
                                    //string content1 = "Task Name: " + this.QueueTask[0].TaskName + " Total Queue Task: " + this.QueueTask.Count + " Is Active: " + this.QueueTask[0].IsActive;
                                    //TradingServer.Facade.FacadeAddNewSystemLog(5, content1, ex.ToString(), "2", "");
                                }

                                check = true;
                                //try
                                //{
                                //    this.QueueTask.RemoveAt(0);
                                //}
                                //catch (Exception ex)
                                //{

                                //}
                                break;
                            }
                            else
                            {
                                check = true;
                                //try
                                //{
                                //    this.QueueTask.RemoveAt(0);
                                //}
                                //catch (Exception ex)
                                //{

                                //}
                                break;
                            }
                        }
                    }

                    //all calculator is running.
                    //exist and waiting for calculator done job
                    //
                    if (!check)
                    {
                        this.isPauseControlTask = true;
                        break;
                    }
                    #endregion
                }
            }
            //
            //end while code
        }
        #endregion
    }
    //
    //end class code
}