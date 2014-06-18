using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class BinaryCommand : IPresenter.IMarketArea
    {
        public IPresenter.AddCommandDelegate AddCommandNotify { get; set; }
        public int IMarketAreaID { get; set; }
        public Market MarketContainer { get; set; }
        List<TradeType> IPresenter.IMarketArea.Type { get; set; }
        public string IMarketAreaName { get; set; }
        public List<Symbol> ListSymbol { get; set; }
        public List<ParameterItem> MarketAreaConfig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BinaryCommand()
        {
            if (BinaryCommand.refeshThread == null)
            {
                BinaryCommand.refeshThread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadRefesh));
                BinaryCommand.refeshThread.Start();                
            }
        }

        #region Thread Refesh
        /// <summary>
        /// private function Thread Refesh
        /// </summary>
        private void ThreadRefesh()
        {
            while (BinaryCommand.isStart)
            {   
                bool result = CheckTimeSession();
                if (result == true)
                {
                    #region Start News Session
                    ///Start News Session
                    ///
                    if (this.isNewSession == true)
                    {
                        BinaryCommand.TimeCurrent = DateTime.Now;

                        BinaryCommand.TimeStart = CalNextTime(TimeCurrent);
                        BinaryCommand.TimeDelegate = BinaryCommand.TimeStart.AddMinutes(1);
                        BinaryCommand.TimePause = BinaryCommand.TimeDelegate.AddMinutes(1);
                        BinaryCommand.TimeEnd = BinaryCommand.TimePause.AddMinutes(2);
                        BinaryCommand.TimeNext = BinaryCommand.TimeEnd.AddMinutes(1);

                        this.isNewSession = false;
                    }
                    #endregion

                    #region Calculate Parameter Time In Session Binary Trading
                    ///Calculate Parameter Time In Session Binary Trading
                    ///
                    TimeSpan temp = new TimeSpan();
                    if (BinaryCommand.TimePause >= DateTime.Now)
                    {
                        ///Time Pause - Time Now
                        ///
                        temp = BinaryCommand.TimePause - DateTime.Now;
                    }

                    ///Time End - Time Now
                    ///
                    TimeSpan tempEnd = BinaryCommand.TimeEnd - DateTime.Now;

                    ///Time Delegate  - Time Now
                    ///
                    TimeSpan tempDelegate = BinaryCommand.TimeDelegate - DateTime.Now;

                    ///Get Total Second
                    ///
                    BinaryCommand.TotalSecondNowToPause = Math.Round(temp.TotalSeconds);

                    ///Get Total Second From Date.Now To End
                    ///
                    BinaryCommand.TotalSecondNowToEnd = Math.Round(tempEnd.TotalSeconds);
                    ///Get Total Second From Date.Now To Time Delegate
                    ///
                    BinaryCommand.TotalSecondNowToDelegate = Math.Round(tempDelegate.TotalSeconds);

                    if (DateTime.Now < BinaryCommand.TimeStart)
                    {
                        BinaryCommand.StatusBinary = ClientBusiness.StatusBinaryTrading.Wait;
                    }

                    if (DateTime.Now == BinaryCommand.TimeStart)
                    {
                        BinaryCommand.StatusBinary = ClientBusiness.StatusBinaryTrading.Start;
                    }
                    #endregion

                    if (BinaryCommand.TimeDelegate <= DateTime.Now)
                    {
                        if (this.isGetPriceStart == false)
                        {
                            #region Set Status Market == Delegate And Get Price And Fill To Price Reference

                            this.isGetPriceStart = true;

                            ///Set Status Market == Delegate
                            ///
                            BinaryCommand.StatusBinary = ClientBusiness.StatusBinaryTrading.Delegate;

                            ///Call Function Get Price Start Session
                            ///
                            this.GetPricesStart();

                            ///Call Function Fill Price Start Reference To List Online Command Queue
                            ///
                            this.FillPricesStart();
                            #endregion
                        }
                    }

                    if (BinaryCommand.TotalSecondNowToPause <= 0)
                    {
                        #region If BinaryTrading.TotalSecond < 0 and BinaryTrading.IsTrade==true Then Get Price Current,and Close Trade ==>IsTrade=false
                        if (BinaryCommand.isPause == false)
                        {
                            ///Set Status Market == Pause
                            ///
                            BinaryCommand.StatusBinary = ClientBusiness.StatusBinaryTrading.Pause;

                            ///Set Property IsTrade == false
                            ///
                            BinaryCommand.isTrade = false;

                            BinaryCommand.isPause = true;
                        }
                        #endregion
                    }

                    if (BinaryCommand.TotalSecondNowToEnd <= 0)
                    {
                        #region If BinaryTrading.TotalSecondEnd < 0 Then Calculate All Binary Command
                        ///Set Status Market == End
                        ///
                        BinaryCommand.StatusBinary = ClientBusiness.StatusBinaryTrading.End;

                        ///Get Price End Binary Trading
                        ///
                        this.GetPricesStop();

                        ///Call Function Calculate All Command In Queue
                        ///
                        this.CalQueueBinaryCommandClose();

                        ///Set Time Start = Time Next
                        ///
                        BinaryCommand.TimeStart = BinaryCommand.TimeNext;

                        ///Set Time Delegate
                        ///
                        BinaryCommand.TimeDelegate = BinaryCommand.TimeStart.AddMinutes(1);

                        ///Calculate Time Pause
                        ///
                        BinaryCommand.TimePause = BinaryCommand.TimeDelegate.AddMinutes(1);

                        ///Calculate Time End
                        ///
                        BinaryCommand.TimeEnd = BinaryCommand.TimePause.AddMinutes(2);

                        ///Calculate Time Next
                        ///
                        BinaryCommand.TimeNext = BinaryCommand.TimeEnd.AddMinutes(1);

                        ///Set IsTrade == True 
                        ///
                        BinaryCommand.isTrade = true;

                        ///Clean Price Start Trading And Price Stop Trading
                        ///
                        if (BinaryCommand.PriceStartSession != null)
                        {
                            BinaryCommand.PriceStartSession.Clear();
                        }

                        if (BinaryCommand.PriceStopSession != null)
                        {
                            BinaryCommand.PriceStopSession.Clear();
                        }
                        #endregion

                        ///Set Flag Get Price Start == false;
                        ///
                        this.isGetPriceStart = false;

                        ///Reset Flag IsPause == False;
                        ///
                        BinaryCommand.isPause = false;
                    }
                }

                ///Sleep 1 Second
                ///
                System.Threading.Thread.Sleep(1000);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void AddCommand(OpenTrade Command)
        {
            bool IsBuy = false;
            TradingServer.Business.Tick priceReference = new Tick();
            bool result = CheckTimeSession();            

            #region Check Result if result == true then Time Session == Start Else Time Session == Close
            ///Check Result if result == true then Time Session == Start Else Time Session == Close
            ///
            if (result == true)
            {
                #region If result == true Then Check BinaryTrading.isTrade
                ///If result == true Then Check BinaryTrading.isTrade
                ///
                if (Business.BinaryCommand.isTrade == true)
                {
                    #region If BinaryTrading.isTrade == True Then Check BinaryTrading.TotalSecond
                    ///If BinaryTrading.isTrade == True Then Check BinaryTrading.TotalSecond
                    ///
                    if (Business.BinaryCommand.TotalSecondNowToPause > 0)
                    {
                        #region If BinaryTrading.TotalSecond > 0 Then Check Balance Of Account
                        ///If BinaryTrading.TotalSecond > 0 Then Check Balance Of Account
                        /// 
                        bool checkAccount=false;
                        checkAccount = this.CheckValidAccount(Command);

                        if (checkAccount == true)
                        {
                            #region Make Command
                            ///Get Price Reference for command
                            ///
                            if (Business.BinaryCommand.PriceStartSession != null)
                            {
                                if (Business.BinaryCommand.PriceStartSession.ContainsKey(Command.Symbol.Name))
                                {
                                    Business.BinaryCommand.PriceStartSession.TryGetValue(Command.Symbol.Name, out priceReference);
                                }
                            }
                            
                            if (Command.Type.ID == 3)
                            {
                                if (priceReference.Ask < 0)
                                {
                                    ///Add Open Price To Command
                                    ///
                                    Command.OpenPrice = 0;
                                }
                                else
                                {
                                    ///Add Open Price To Command
                                    ///
                                    Command.OpenPrice = priceReference.Ask;
                                }

                                //Set IsBuy Is True
                                IsBuy = true;
                            }
                            else if (Command.Type.ID == 4)
                            {
                                if (priceReference.Bid < 0)
                                {
                                    ///Add Open Price To Command
                                    ///
                                    Command.OpenPrice = 0;
                                }
                                else
                                {
                                    ///Add Open Price To Command
                                    ///
                                    Command.OpenPrice = priceReference.Bid;
                                }

                                //Set IsBuy Is False
                                IsBuy = false;
                            }

                            ///Add Time To Command
                            ///
                            Command.OpenTime = DateTime.Now;
                            Command.CloseTime = Command.OpenTime;
                            Command.Taxes = 0;

                            if (string.IsNullOrEmpty(Command.Comment))
                                Command.Comment = "[binary command]";

                            Command.ExpTime= DateTime.Now;

                            if (Command.Investor.Balance < Command.Size)
                            {
                                if (Command.Investor.Balance > 0)
                                {
                                    double tempSize = Command.Size - Command.Investor.Balance;
                                    Command.Investor.Balance = 0;
                                    //UPDATE BALANCE OF INVESTOR ACCOUNT IN DATABASE
                                    TradingServer.Facade.FacadeUpdateBalance(Command.Investor.InvestorID, 0);

                                    Command.Investor.Credit -= tempSize;

                                    //UPDATE CREDIT OF INVESTOR ACCOUNT IN DATABASE
                                    TradingServer.Facade.FacadeUpdateCredit(Command.Investor.InvestorID, Command.Investor.Credit);
                                }
                                else
                                {
                                    Command.Investor.Balance -= Command.Size;

                                    //UPDATE BALANCE OF INVESTOR ACCOUNT IN DATABASE
                                    TradingServer.Facade.FacadeUpdateBalance(Command.Investor.InvestorID, 0);
                                }
                            }
                            else
                            {
                                //SUB BALANCE OF INVESTOR ACCOUNT
                                Command.Investor.Balance -= Command.Size;

                                //UPDATE BALANCE OF INVESTOR ACCOUNT IN DATABASE
                                TradingServer.Facade.FacadeUpdateBalance(Command.Investor.InvestorID, Command.Investor.Balance);
                            }                         

                            ///Insert Command To Database
                            ///
                            int temp = TradingServer.Facade.FacadeAddNewOpenTrade(Command);

                            //Build Comand Code 
                            string CommandCode = string.Empty;
                            CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(temp.ToString());
                            Command.CommandCode = CommandCode;

                            //Update Command Code
                            TradingServer.Facade.FacadeUpdateCommandCode(temp, CommandCode);

                            //Build Two Instance OpenTrade
                            //One Instance For Investor 
                            //One Instance For Symbol And MarketArea
                            #region Build Instance OpenTrade For Investor
                            Business.OpenTrade newOpenTradeInvestor = new OpenTrade();
                            newOpenTradeInvestor.ID = temp;
                            newOpenTradeInvestor.ClientCode = Command.ClientCode;
                            newOpenTradeInvestor.ClosePrice = Command.ClosePrice;
                            newOpenTradeInvestor.CloseTime = Command.CloseTime;
                            newOpenTradeInvestor.CommandCode = CommandCode;
                            newOpenTradeInvestor.Commission = Command.Commission;
                            newOpenTradeInvestor.ExpTime = Command.ExpTime;
                            newOpenTradeInvestor.ID = temp;
                            newOpenTradeInvestor.Investor = Command.Investor;
                            newOpenTradeInvestor.IsClose = Command.IsClose;                            
                            newOpenTradeInvestor.OpenPrice = Command.OpenPrice;
                            newOpenTradeInvestor.OpenTime = Command.OpenTime;
                            newOpenTradeInvestor.Profit = Command.Profit;
                            newOpenTradeInvestor.Size = Command.Size;
                            newOpenTradeInvestor.StopLoss = Command.StopLoss;
                            newOpenTradeInvestor.Swap = Command.Swap;
                            newOpenTradeInvestor.Symbol = Command.Symbol;
                            newOpenTradeInvestor.TakeProfit = Command.TakeProfit;
                            newOpenTradeInvestor.Type = Command.Type;
                            newOpenTradeInvestor.Margin = Command.Margin;
                            newOpenTradeInvestor.IGroupSecurity = Command.IGroupSecurity;
                            #endregion

                            #region Find Investor In Investor List And Add Command To Investor List
                            //Find Investor In Investor List And Add Command To Investor List
                            if (Business.Market.InvestorList != null)
                            {
                                int countInvestor = Business.Market.InvestorList.Count;
                                for (int n = 0; n < countInvestor; n++)
                                {
                                    if (Business.Market.InvestorList[n].InvestorID == newOpenTradeInvestor.Investor.InvestorID)
                                    {
                                        if (Business.Market.InvestorList[n].BinaryCommandList != null && Business.Market.InvestorList[n].BinaryCommandList.Count > 0)
                                        {
                                            Business.Market.InvestorList[n].BinaryCommandList.Add(newOpenTradeInvestor);
                                        }
                                        else
                                        {
                                            Business.Market.InvestorList[n].BinaryCommandList = new List<OpenTrade>();
                                            Business.Market.InvestorList[n].BinaryCommandList.Add(newOpenTradeInvestor);
                                        }

                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region Build Instance Open Trade For Symbol
                            Business.OpenTrade newOpenTradeSymbol = new OpenTrade();
                            newOpenTradeSymbol.ID = temp;
                            newOpenTradeSymbol.ClientCode = Command.ClientCode;
                            newOpenTradeSymbol.ClosePrice = Command.ClosePrice;
                            newOpenTradeSymbol.CloseTime = Command.CloseTime;
                            newOpenTradeSymbol.CommandCode = CommandCode;
                            newOpenTradeSymbol.Commission = Command.Commission;
                            newOpenTradeSymbol.ExpTime = Command.ExpTime;
                            newOpenTradeSymbol.ID = temp;
                            newOpenTradeSymbol.Investor = Command.Investor;
                            newOpenTradeSymbol.IsClose = false;                            
                            newOpenTradeSymbol.OpenPrice = Command.OpenPrice;
                            newOpenTradeSymbol.OpenTime = DateTime.Now;
                            newOpenTradeSymbol.Profit = Command.Profit;
                            newOpenTradeSymbol.Size = Command.Size;
                            newOpenTradeSymbol.StopLoss = Command.StopLoss;
                            newOpenTradeSymbol.Swap = Command.Swap;
                            newOpenTradeSymbol.Symbol = Command.Symbol;
                            newOpenTradeSymbol.TakeProfit = Command.TakeProfit;
                            newOpenTradeSymbol.Type = Command.Type;
                            newOpenTradeSymbol.Margin = Command.Margin;
                            newOpenTradeSymbol.IGroupSecurity = Command.IGroupSecurity;
                            #endregion

                            #region Find Symbol In Market And Add Command To Market Area And List Symbol
                            //Find Symbol In Market And Add Command To Market Area And List Symbol
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int i = 0; i < countSymbol; i++)
                                {
                                    if (Business.Market.SymbolList[i].SymbolID == newOpenTradeSymbol.Symbol.SymbolID)
                                    {
                                        if (Business.Market.SymbolList[i].BinaryCommandList != null)
                                        {
                                            Business.Market.SymbolList[i].BinaryCommandList.Add(newOpenTradeSymbol);
                                        }
                                        else
                                        {
                                            Business.Market.SymbolList[i].BinaryCommandList = new List<OpenTrade>();
                                            Business.Market.SymbolList[i].BinaryCommandList.Add(newOpenTradeSymbol);
                                        }
                                    }

                                    //Find In Symbol Reference

                                }
                            }
                            #endregion 

                            #region BUILD INSTANCE OPEN TRADE FOR COMMAND EXECUTORY
                            Business.OpenTrade newOpenTradeExe = new OpenTrade();
                            newOpenTradeExe.ID = temp;
                            newOpenTradeExe.ClientCode = Command.ClientCode;
                            newOpenTradeExe.ClosePrice = Command.ClosePrice;
                            newOpenTradeExe.CloseTime = Command.CloseTime;
                            newOpenTradeExe.CommandCode = CommandCode;
                            newOpenTradeExe.Commission = Command.Commission;
                            newOpenTradeExe.ExpTime = Command.ExpTime;
                            newOpenTradeExe.ID = temp;
                            newOpenTradeExe.Investor = Command.Investor;
                            newOpenTradeExe.IsClose = false;
                            newOpenTradeExe.OpenPrice = Command.OpenPrice;
                            newOpenTradeExe.OpenTime = DateTime.Now;
                            newOpenTradeExe.Profit = Command.Profit;
                            newOpenTradeExe.Size = Command.Size;
                            newOpenTradeExe.StopLoss = Command.StopLoss;
                            newOpenTradeExe.Swap = Command.Swap;
                            newOpenTradeExe.Symbol = Command.Symbol;
                            newOpenTradeExe.TakeProfit = Command.TakeProfit;
                            newOpenTradeExe.Type = Command.Type;
                            newOpenTradeExe.Margin = Command.Margin;
                            newOpenTradeExe.IGroupSecurity = Command.IGroupSecurity;
                            #endregion

                            Business.Market.CommandExecutor.Add(newOpenTradeExe);

                            ///Set Command ID
                            ///
                            Command.ID = temp;

                            //Command.Investor.UpdateCommand(Command);
                            if (Command.Investor != null)
                            {
                                if (Command.Investor.ClientBinaryQueue == null)
                                    Command.Investor.ClientBinaryQueue = new List<string>();

                                //Add String Command Server To Client
                                string Message = string.Empty;
                                Message = "AddBinary$True,Add Binary Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                            Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                            Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                            Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," +
                                            Command.Type.ID + "," + Command.Margin + ",Binary";

                                Command.Investor.ClientBinaryQueue.Add(Message);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Error Margin
                            if (Command.Investor != null)
                            {
                                ///Return false
                                ///
                                if (Command.Investor.ClientBinaryQueue == null)
                                    Command.Investor.ClientBinaryQueue = new List<string>();

                                //Add String Command Server To Client
                                string Message = string.Empty;
                                Message = "AddBinary$False,INSUFFICIENT FUND - TRADE NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                            Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                            Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                            Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "00" + "," + 
                                            false + "," + "-1" + "," + Command.Margin + "," + ",Binary";

                                Command.Investor.ClientBinaryQueue.Add(Message);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Time Out
                        if (Command.Investor != null)
                        {
                            ///Return false
                            ///
                            if (Command.Investor.ClientBinaryQueue == null)
                                Command.Investor.ClientBinaryQueue = new List<string>();

                            //Add String Command Server To Client
                            string Message = string.Empty;
                            Message = "AddBinary$False,BINARY TIME OUT," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                        Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                        Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                        Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "00" + "," + 
                                        false + "," + "-1" + "," + Command.Margin + "," + ",Binary";

                            Command.Investor.ClientBinaryQueue.Add(Message);
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region Market Close
                    if (Command.Investor != null)
                    {
                        ///return false
                        ///
                        if (Command.Investor.ClientBinaryQueue == null)
                            Command.Investor.ClientBinaryQueue = new List<string>();

                        //Add String Command Server To Client
                        string Message = string.Empty;
                        Message = "AddBinary$False,TRADING TIME IS CLOSED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                    Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                    Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "00" + "," + 
                                    false + "," + "-1" + "," + Command.Margin + "," + ",Binary";

                        Command.Investor.ClientBinaryQueue.Add(Message);
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region Session Close
                if (Command.Investor != null)
                {
                    ///Return false
                    ///
                    if (Command.Investor.ClientBinaryQueue == null)
                        Command.Investor.ClientBinaryQueue = new List<string>();

                    //Add String Command Server To Client
                    string Message = string.Empty;
                    Message = "AddBinary$False,BINARY TIME OUT," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "00" + "," + 
                                false + "," + "-1" + "," + Command.Margin + "," + ",Binary";

                    Command.Investor.ClientBinaryQueue.Add(Message);
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void CloseCommand(OpenTrade Command)
        {
            bool Flag = false;

            bool IsBuy = false;
            if (Command.Type.ID == 3)
                IsBuy = true;

            if (string.IsNullOrEmpty(Command.Comment))
                Command.Comment = "[close binary command]";

            if (Business.BinaryCommand.isTrade)
            {
                #region Binary Command List Is Null
                if (Command.Investor != null)
                {
                    //Binary Command List Is Null
                    if (Command.Investor.BinaryCommandList == null)
                    {
                        if (Command.Investor.ClientBinaryQueue == null)
                            Command.Investor.ClientBinaryQueue = new List<string>();

                        string Message = string.Empty;
                        Message = "CancelBinary$False,Binary Command Null," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                    Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                    Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "00" + "," +
                                    Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Binary";

                        Command.Investor.ClientBinaryQueue.Add(Message);
                        return;
                    }
                }
                #endregion               

                #region Remove Binary Command In Binary Command List Of InvestorList
                //Remove Binary Command In Binary Command List
                int count = Command.Investor.BinaryCommandList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Command.Investor.BinaryCommandList[i].ID == Command.ID)
                    {
                        //RETURN MONEY OF INVESTOR ACCOUNT
                        Command.Investor.Balance += Command.Investor.BinaryCommandList[i].Size;

                        //UPDATE BALANCE IN DATABASE
                        TradingServer.Facade.FacadeUpdateBalance(Command.Investor.InvestorID, Command.Investor.Balance);

                        ///Remove Command In Online Command Queue
                        ///                            
                        Command.Investor.BinaryCommandList.Remove(Command.Investor.BinaryCommandList[i]);

                        ///Delete Command In Database
                        ///
                        TradingServer.Facade.FacadeDeleteOpenTradeByID(Command.ID);

                        if (Command.Investor != null)
                        {
                            if (Command.Investor.ClientBinaryQueue == null)
                                Command.Investor.ClientBinaryQueue = new List<string>();

                            //Add String Command Server To Client
                            string Message = string.Empty;
                            Message = "CancelBinary$True,Close Command Complete," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                        Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                        Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                        Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," +
                                        Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Binary";

                            Command.Investor.ClientBinaryQueue.Add(Message);
                        }
                        Flag = true;
                        break;
                    }
                }
                #endregion

                #region Return False If Don't Find In Binary Command List
                //Return False If Don't Find In Binary Command List
                if (Flag == false)
                {
                    if (Command.Investor != null)
                    {
                        if (Command.Investor.ClientBinaryQueue == null)
                            Command.Investor.ClientBinaryQueue = new List<string>();

                        //Add String Command Server To Client
                        string Message = string.Empty;
                        Message = "CancelBinary$False,Can't Find Command," + "-1" + "," + "-1" + "," +
                                    "NaN" + "," + "-1" + "," + IsBuy + "," + "-1" + "," +
                                    "-1" + "," + "-1" + "," + "-1" + "," +
                                    "-1" + "," + "-1" + "," + "-1" + "," +
                                    "-1" + "," + "Comment," + "-1" + "," + "BinaryTrading" + "," +
                                    1 + "," + Command.ExpTime + "," + "-1" + "," + "-1" + "," + false + ",-1,0,Open";

                        Command.Investor.ClientBinaryQueue.Add(Message);
                    }
                }
                #endregion                
            }
            else
            {                
                #region Return False If Time Close Command Is Close
                if (Command.Investor != null)
                {
                    if (Command.Investor.ClientBinaryQueue == null)
                        Command.Investor.ClientBinaryQueue = new List<string>();

                    //Add String Command Server To Client
                    string Message = string.Empty;
                    Message = "CancelBinary$False,BINARY TIME OUT," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," +
                                Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                                Command.Profit + "," + "Comment," + Command.ID + "," + "BinaryTrading" + "," +
                                1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," +
                                Command.IsHedged + "," + Command.Type.ID + Command.Margin + "," + "Open";

                    Command.Investor.ClientBinaryQueue.Add(Message);
                }
                #endregion                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiCloseCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiUpdateCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public IPresenter.CloseCommandDelegate CloseCommandNotify(OpenTrade Command)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initStatus"></param>
        /// <returns></returns>
        public IPresenter.InitServerDelegate CheckStatusInitServer(InitStatus initStatus)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        public IPresenter.SendClientCmdDelegate SendClientCmdDelegate(string Cmd)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        public void SetTickValueNotify(Tick Tick, Symbol RefSymbol)
        {
            TradingServer.Facade.FacadeCalculationAlert(Tick, RefSymbol);        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public OpenTrade CalculateCommand(OpenTrade Command)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void UpdateCommand(OpenTrade Command)
        {
            return;
        }
    }
}
