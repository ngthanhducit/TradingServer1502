using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingServer.Business
{
    //Begin class code
    //
    class Calculator : IDisposable
    {        
        private IPresenter.ICalculatorClient iClient;
        private TaskComplete taskWork;
        private TaskDelegate taskJob;
        private Thread task;
        private bool isStart = true;
        private bool isPause = false;
        private CalculatorFacade calculatorFacade;

        /// <summary>
        /// Quick task run one time and pause. It can use againt for other task.
        /// </summary>
        public bool IsQuickTask { get; set; }

        /// <summary>
        /// Running status show task is running.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Long task is running repeatly. Quick task stop when it finish its job and dispose itseft
        /// </summary>
        public bool IsLongTask { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskDelegate TaskJob
        {
            set
            {
                taskJob = value;
            }
        }

        public string TaskName { get; set; }
        public string Comment { get; set; }
        public Business.Tick tickValue { get; set; }
                

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CalculatorContaint"></param>
        public Calculator(CalculatorFacade CalculatorContaint)
        {
            this.calculatorFacade = CalculatorContaint;
            this.IsLongTask = false;
            this.IsQuickTask = false;
            this.IsRunning = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iClient"></param>
        public void Start(IPresenter.ICalculatorClient iClient)
        {
            this.iClient = iClient;

            this.IsRunning = true;
            this.TaskName = this.iClient.TaskName;
            this.Comment = this.iClient.Comment;
            this.taskWork = this.iClient.TaskWork;            

            if (this.task == null)
            {
                this.task = new Thread(new ThreadStart(this.Run));
                this.task.Start();
            }

            if (this.isPause)
            {
                this.isPause = false;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="TaskName"></param>
        ///// <param name="Comment"></param>
        ///// <param name="TaskJob"></param>
        //public void Start(string TaskName, string Comment, TaskDelegate TaskJob,Business.Tick objTick)
        //{            
        //    this.IsRunning = true;
        //    this.TaskName = TaskName;
        //    this.Comment = Comment;
        //    this.TaskJob = TaskJob;
        //    this.tickValue = objTick;
                        
        //    if (task == null)
        //    {
        //        task = new Thread(new ThreadStart(this.Run));
        //        task.Start();
        //    }

        //    if (this.isPause)
        //    {
        //        this.isPause = false;
        //    }
        //}

        //public void Start(string TaskName, string Comment, TaskComplete TaskJob)
        //{
        //    this.IsRunning = true;
        //    this.TaskName = TaskName;
        //    this.Comment = Comment;
        //    this.taskWork = TaskJob;            

        //    if (task == null)
        //    {
        //        task = new Thread(new ThreadStart(this.Run));
        //        task.Start();
        //    }

        //    if (this.isPause)
        //    {
        //        this.isPause = false;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            this.isPause = false;
            this.isStart = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Run()
        {
            //begin while loop
            //
            while (this.isStart)
            {
                while (this.isPause)
                {
                    if (this.IsRunning)
                    {
                        this.TaskName = "None";
                        this.IsRunning = false;
                    }

                    Thread.Sleep(1);
                }

                if (!this.isStart)
                {
                    break;
                }

                //run tak job
                //                
                if (this.taskWork != null)
                {                    
                    this.taskWork();
                    this.iClient.IsActive = false;
                    this.TaskName = "None";                    
                }

                this.iClient.IsActive = false;

                //check status for continoun running
                //
                if (this.IsQuickTask)
                    break;

                if (!this.IsLongTask)
                {
                    this.isPause = true;
                    this.IsRunning = false;
                }

                calculatorFacade.isPauseControlTask = false;
            }
            //
            //end while loop
        }

        void IDisposable.Dispose()
        {
            this.calculatorFacade.Calculators.Remove(this);
            this.task.Abort();
        }
    }
    //
    //End class code
}
