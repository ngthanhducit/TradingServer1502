using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// THREAD CHECK INVESTOR ONLINE
        /// </summary>
        internal static void TimeEventCheckInvestorOnline()
        {
            //TradingServer.Facade.FacadeAddNewSystemLog(5, "Check Time Event Check Investor Online", "Error Check Investor Online", "", "");
            while (true)
            {
                if (Business.Market.InvestorOnline != null)
                {   
                    int count = Business.Market.InvestorOnline.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {  
                        if (Business.Market.InvestorOnline[i].IsLogout)
                        {
                            Business.Market.InvestorOnline.RemoveAt(i);
                            continue;
                        }

                        //int timeOut = Business.Market.InvestorOnline[i].numTimeOut;
                        //int temp = timeOut - 5;
                        Business.Market.InvestorOnline[i].numTimeOut = Business.Market.InvestorOnline[i].numTimeOut - 5;
                        if (Business.Market.InvestorOnline[i].numTimeOut <= 0)
                        //if (temp < 1)
                        {
                            //string content = "Remove investor online " + " " + Business.Market.InvestorOnline[i].Code + " " + Business.Market.InvestorOnline[i].numTimeOut;
                            //TradingServer.Facade.FacadeAddNewSystemLog(5, content, "Remove investor online", "", "");

                            //send notify to manager
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(2, Business.Market.InvestorOnline[i]);

                            if (Business.Market.InvestorList != null)
                            {
                                int countInvestor = Business.Market.InvestorList.Count;
                                for (int j = 0; j < countInvestor; j++)
                                {   
                                    if (Business.Market.InvestorList[j].InvestorID == Business.Market.InvestorOnline[i].InvestorID)
                                    {
                                        Business.Market.InvestorList[j].IsOnline = false;
                                        //Business.Market.InvestorList[j].IsLogout = true;

                                        break;
                                    }
                                }
                            }

                            Business.Market.InvestorOnline.RemoveAt(i);
                        }
                        //else
                        //{
                        //    Business.Market.InvestorOnline[i].numTimeOut = temp;
                        //}
                    }
                }

                System.Threading.Thread.Sleep(5000);
            }
            //Business.Market.TimerInvestorOnline = new System.Timers.Timer();
            //TradingServer.Facade.FacadeAddNewSystemLog(5, "New Timer", "", "", "");
            //Business.Market.TimerInvestorOnline.Interval = 5000;
            //Business.Market.TimerInvestorOnline.Elapsed += new System.Timers.ElapsedEventHandler(TimerInvestorOnline_Elapsed);            
            //Business.Market.TimerInvestorOnline.Enabled = true;            
        }

        //numCheck: 
        //==> 0-> Normal(IsPrimary = true and IsRecive = true Status = Connect)
        //==> 1-> Change (IsPrimary = true And IsRecive = True Status = Disconnect) ==> Change IsRecive = False
        //==> 2-> Return (IsPrimary = True , IsRecive = false Status = Connection) ==> Change IsRevice = True
        //==> 3-> ChangeSecond (IsPrimary = False, IsRecive = False Status = Disconnected) => Change IsRecive = True
        /// <summary>
        /// THREAD CHECK MUTILPLE PRICE QUOTES
        /// </summary>
        /// <returns></returns>
        internal static void ThreadCheckMultiplePrice()
        {
            while (Business.Market.IsMultipleQuote)
            {
                Business.Market.isBlock = true;

                int isNumCheck = 0;
                if (Business.Market.MultiplePriceQuotes != null)
                {
                    int count = Business.Market.MultiplePriceQuotes.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.MultiplePriceQuotes[i].IsPrimary)
                        {
                            if (Business.Market.MultiplePriceQuotes[i].IsRecive)
                            {
                                TimeSpan timeSpan = DateTime.Now - Business.Market.MultiplePriceQuotes[i].TimeConnect;
                                if (timeSpan.TotalSeconds > Business.Market.TimeCheckMultiplePrice)
                                {
                                    isNumCheck = 1;
                                    break;
                                }
                            }
                            else
                            {
                                TimeSpan timeSpan = DateTime.Now - Business.Market.MultiplePriceQuotes[i].TimeConnect;
                                if (timeSpan.TotalSeconds < Business.Market.TimeCheckMultiplePrice)
                                {
                                    isNumCheck = 2;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (Business.Market.MultiplePriceQuotes[i].IsRecive)
                            {
                                TimeSpan timeSpan = DateTime.Now - Business.Market.MultiplePriceQuotes[i].TimeConnect;
                                if (timeSpan.TotalSeconds > Business.Market.TimeCheckMultiplePrice)
                                {
                                    isNumCheck = 3;
                                    break;
                                }
                            }
                        }
                    }

                    switch (isNumCheck)
                    {
                        case 1:
                            {
                                int nCount = Business.Market.MultiplePriceQuotes.Count;
                                int indexMin = 0;
                                double min = 0;
                                for (int i = 0; i < nCount; i++)
                                {
                                    if (!Business.Market.MultiplePriceQuotes[i].IsPrimary)
                                    {                                        
                                        TimeSpan timeSpan = DateTime.Now - Business.Market.MultiplePriceQuotes[i].TimeConnect;
                                        if (min == 0)
                                        {
                                            min = timeSpan.TotalSeconds;
                                            indexMin = i;
                                        }

                                        if (min > timeSpan.TotalSeconds)
                                        {
                                            min = timeSpan.TotalSeconds;
                                            indexMin = i;
                                        }

                                        if (Business.Market.MultiplePriceQuotes[i].IsRecive == true)
                                        {
                                            Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Disconnected;
                                            Business.Market.MultiplePriceQuotes[i].IsRecive = false;
                                        }
                                    }
                                    else
                                    {
                                        Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Disconnected;
                                        Business.Market.MultiplePriceQuotes[i].IsRecive = false;
                                    }
                                }

                                if (indexMin < nCount)
                                {
                                    Business.Market.MultiplePriceQuotes[indexMin].Status = StatusPriceServer.Connected;
                                    Business.Market.MultiplePriceQuotes[indexMin].IsRecive = true;
                                }
                            }
                            break;

                        case 2:
                            {
                                int nCount = Business.Market.MultiplePriceQuotes.Count;
                                for (int i = 0; i < nCount; i++)
                                {
                                    if (Business.Market.MultiplePriceQuotes[i].IsPrimary)
                                    {
                                        Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Connected;
                                        Business.Market.MultiplePriceQuotes[i].IsRecive = true;
                                    }
                                    else
                                    {
                                        if (Business.Market.MultiplePriceQuotes[i].IsRecive == true)
                                        {
                                            Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Disconnected;
                                            Business.Market.MultiplePriceQuotes[i].IsRecive = false;
                                        }
                                        else
                                        {
                                            Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Disconnected;
                                        }
                                    }
                                }
                            }
                            break;

                        case 3:
                            {
                                int nCount = Business.Market.MultiplePriceQuotes.Count;
                                int indexMin = 0;
                                double min = 0;
                                for (int i = 0; i < nCount; i++)
                                {
                                    if (!Business.Market.MultiplePriceQuotes[i].IsPrimary)
                                    {
                                        TimeSpan timeSpan = DateTime.Now - Business.Market.MultiplePriceQuotes[i].TimeConnect;

                                        if (min == 0)
                                        {
                                            min = timeSpan.TotalSeconds;
                                            indexMin = i;
                                        }

                                        if (min > timeSpan.TotalSeconds)
                                        {
                                            min = timeSpan.TotalSeconds;
                                            indexMin = i;
                                        }

                                        if (Business.Market.MultiplePriceQuotes[i].IsRecive)
                                        {
                                            Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Disconnected;
                                            Business.Market.MultiplePriceQuotes[i].IsRecive = false;
                                        }
                                    }
                                    else
                                    {
                                        TimeSpan timeSpan = DateTime.Now - Business.Market.MultiplePriceQuotes[i].TimeConnect;
                                        if (timeSpan.TotalSeconds < Business.Market.TimeCheckMultiplePrice)
                                        {
                                            Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Connected;
                                            Business.Market.MultiplePriceQuotes[i].IsRecive = true;
                                            break;
                                        }
                                        else
                                        {
                                            Business.Market.MultiplePriceQuotes[i].Status = StatusPriceServer.Disconnected;
                                            Business.Market.MultiplePriceQuotes[i].IsRecive = false;
                                        }
                                    }
                                }

                                if (indexMin < nCount)
                                {
                                    Business.Market.MultiplePriceQuotes[indexMin].Status = StatusPriceServer.Connected;
                                    Business.Market.MultiplePriceQuotes[indexMin].IsRecive = true;
                                }
                            }
                            break;
                    }
                }

                Business.Market.isBlock = false;

                System.Threading.Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ThreadRemoveOpenTrade()
        {
            try
            {
                Business.OpenRemove result = null;
                while (Business.Market.IsRemoveCommand)
                {
                    result = Business.Market.GetOpenTradeRemove();

                    while (result != null && result.OpenTradeID > 0)
                    {
                        if (result != null && result.OpenTradeID > 0)
                        {
                            bool resultRemove = Business.Market.ProcessRemoveOpenTrade(result);
                        }
                        else
                        {
                            string message = "remove command null in list open trade uncomplete";
                            TradingServer.Facade.FacadeAddNewSystemLog(1, message, "[remove command]", "", "");
                        }

                        result = Business.Market.GetOpenTradeRemove();
                    }

                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {                
                TradingServer.Facade.FacadeAddNewSystemLog(1, ex.Message, "[remove command]", "", "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Business.OpenRemove GetOpenTradeRemove()
        {
            Business.OpenRemove result = new OpenRemove();
            if (Business.Market.RemoveCommandList != null && Business.Market.RemoveCommandList.Count > 0)
            {
                if (Business.Market.RemoveCommandList[0] != null)
                {
                    result = Business.Market.RemoveCommandList[0];
                    Business.Market.RemoveCommandList.Remove(Business.Market.RemoveCommandList[0]);
                }
                else
                {
                    Business.Market.RemoveCommandList.Remove(Business.Market.RemoveCommandList[0]);
                }                
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool ProcessRemoveOpenTrade(Business.OpenRemove value)
        {
            bool result = false;
            bool resultRemoveExe = false;
            bool resultRemoveComSym = false;
            bool resultRemoveComInv = false;
            Business.OpenTrade cacheOpenExecutor = null;            

            #region REMOVE COMMAND IN COMMAND EXECUTOR
            if (value.IsExecutor)
            {
                //REMOVE COMMAND IN COMMAND EXECUTOR
                if (Business.Market.CommandExecutor != null && Business.Market.CommandExecutor.Count > 0)
                {
                    for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                    {
                        if (Business.Market.CommandExecutor[i] != null)
                        {
                            if (Business.Market.CommandExecutor[i].ID == value.OpenTradeID)
                            {
                                cacheOpenExecutor = Business.Market.CommandExecutor[i];
                                Business.Market.CommandExecutor.RemoveAt(i);
                                resultRemoveExe = true;
                                break;
                            }
                        }
                        else
                        {
                            Business.Market.CommandExecutor.Remove(Business.Market.CommandExecutor[i]);
                            i--;
                        }
                    }
                }
            }            
            #endregion

            #region REMOVE COMMAND IN SYMBOL LIST
            if (value.IsSymbol)
            {
                if (resultRemoveExe)
                {
                    //REMOVE COMMAND IN SYMBOL LIST
                    if (Business.Market.SymbolList != null && Business.Market.SymbolList.Count > 0)
                    {
                        for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                        {
                            if (Business.Market.SymbolList[i].Name == value.SymbolName)
                            {
                                if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                {
                                    for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                                    {
                                        if (Business.Market.SymbolList[i].CommandList[j] != null)
                                        {
                                            if (Business.Market.SymbolList[i].CommandList[j].ID == value.OpenTradeID)
                                            {
                                                Business.Market.SymbolList[i].CommandList.RemoveAt(j);
                                                resultRemoveComSym = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Business.Market.SymbolList[i].CommandList.Remove(Business.Market.SymbolList[i].CommandList[j]);
                                            j--;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }            
            #endregion

            #region REMOVE COMMAND IN INVESTOR LIST
            if (value.IsInvestor)
            {
                if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                {
                    for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == value.InvestorID)
                        {
                            if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                            {
                                for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                {
                                    if (Business.Market.InvestorList[i].CommandList[j] != null)
                                    {
                                        if (Business.Market.InvestorList[i].CommandList[j].ID == value.OpenTradeID)
                                        {
                                            Business.Market.InvestorList[i].CommandList.Remove(Business.Market.InvestorList[i].CommandList[j]);
                                            resultRemoveComInv = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Business.Market.InvestorList[i].CommandList.Remove(Business.Market.InvestorList[i].CommandList[j]);
                                        j--;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openTrade"></param>
        /// <returns></returns>
        internal static void AddCommandToRemoveList(Business.OpenRemove openTrade)
        {            
            if (Business.Market.RemoveCommandList != null)
            {
                Business.Market.RemoveCommandList.Add(openTrade);
            }
            else
            {
                Business.Market.RemoveCommandList = new List<OpenRemove>();
                Business.Market.RemoveCommandList.Add(openTrade);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal static bool CheckPriceQuotes(string ipAddress)
        {            
            if (Business.Market.MultiplePriceQuotes != null)
            {
                int count = Business.Market.MultiplePriceQuotes.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.MultiplePriceQuotes[i].IpServer.Trim() == ipAddress.Trim())
                    {
                        if (Business.Market.MultiplePriceQuotes[i].IsRecive)
                        {
                            Business.Market.MultiplePriceQuotes[i].TimeConnect = DateTime.Now;
                            return true;
                        }
                        else
                        {
                            Business.Market.MultiplePriceQuotes[i].TimeConnect = DateTime.Now;
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal static bool CheckMultiPriceQuotes(string ipAddress)
        {
            bool result = false;
            if (Business.Market.MultiplePriceQuotes != null)
            {
                int count = Business.Market.MultiplePriceQuotes.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.MultiplePriceQuotes[i].IpServer.Trim() == ipAddress.Trim())
                    {
                        Business.Market.MultiplePriceQuotes[i].TimeConnect = DateTime.Now;
                        result = true;

                        break;
                    }                        
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void ProcessSystemLog()
        {
            Business.SystemLog newSystemLog;
            while (Business.Market.IsProcessLog)
            {                
                newSystemLog = Business.Market.GetSystemLog();
                while (newSystemLog != null)
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(newSystemLog);
                                        
                    newSystemLog = Business.Market.GetSystemLog();

                    System.Threading.Thread.Sleep(50);
                }

                System.Threading.Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Business.SystemLog GetSystemLog()
        {
            Business.SystemLog result = null;
            if (Business.Market.ListSystemLog != null && Business.Market.ListSystemLog.Count > 0)
            {
                result = Business.Market.ListSystemLog[0];

                Business.Market.ListSystemLog.RemoveAt(0);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void ProcessStatementEOD()
        {
            //Business.StatementInvestor newStatementInvestor;
            //while (Business.Market.IsProcessAddStatement)
            //{
            //    newStatementInvestor = Business.Market.GetStatementInvestor();
            //    while (newStatementInvestor != null)
            //    {
            //        //insert database
            //        if (newStatementInvestor != null)
            //        {
            //            double creditIn = 0;
            //            double creditOut = 0;
            //            double Deposit = 0;
            //            double Withdrawal = 0;
            //            double totalSwapClose = 0;
            //            double totalSwapOpen = 0;
            //            double totalCommssionClose = 0;
            //            double totalCommssionOpen = 0;
            //            double realizedPL = 0;
            //            double unRealizedPL = 0;
            //            double balance = 0;
            //            int investorID = -1;

            //            DateTime time = DateTime.Parse(newStatementInvestor.Date);
            //            string dbName = string.Empty;
            //            if (time.DayOfWeek == DayOfWeek.Monday)
            //            {                            
            //                //CREATE NEW DATABASE AND CREATE TABLE
            //                dbName = time.Day.ToString() + time.Month.ToString() + time.Year.ToString();

            //                bool isExists = ServerBackupLibrary.FacadeDB.FacadeCheckExitsDatabase(dbName);

            //                if (!isExists)
            //                {
            //                    ServerBackupLibrary.FacadeDB.CreateDatabase(dbName);
            //                    ServerBackupLibrary.FacadeDB.FacadeCreateInvestorAccount(dbName);
            //                    ServerBackupLibrary.FacadeDB.FacadeCreateTableCommandHistory(dbName);
            //                    ServerBackupLibrary.FacadeDB.FacadeCreateTableOpenPosition(dbName);
            //                }
            //            }
            //            else
            //            {
            //                int days = time.DayOfWeek - DayOfWeek.Monday;
            //                DateTime tempTime = time.AddDays(-days);
            //                dbName = tempTime.Day.ToString() + tempTime.Month.ToString() + tempTime.Year.ToString();

            //                bool isExits = ServerBackupLibrary.FacadeDB.FacadeCheckExitsDatabase(dbName);
            //                if (!isExits)
            //                {
            //                    ServerBackupLibrary.FacadeDB.CreateDatabase(dbName);
            //                    ServerBackupLibrary.FacadeDB.FacadeCreateInvestorAccount(dbName);
            //                    ServerBackupLibrary.FacadeDB.FacadeCreateTableCommandHistory(dbName);
            //                    ServerBackupLibrary.FacadeDB.FacadeCreateTableOpenPosition(dbName);
            //                }
            //            }

            //            #region PROCESS CALCULATION TOTAL COMMISSION, TOTAL SWAP, UNREALIZEDPL
            //            if (newStatementInvestor.ListOpenPosition != null)
            //            {
            //                int countOpenPosition = newStatementInvestor.ListOpenPosition.Count;
            //                for (int i = 0; i < countOpenPosition; i++)
            //                {
            //                    totalCommssionOpen += newStatementInvestor.ListOpenPosition[i].Commission;
            //                    totalSwapOpen += newStatementInvestor.ListOpenPosition[i].Swap;
            //                    unRealizedPL += newStatementInvestor.ListOpenPosition[i].Profit;
            //                }
            //            }
            //            #endregion

            //            #region PROCESS CALCULATION DEPOSIT, WITHDRAWAL, CREDIT IN, CREDIT OUT, TOTAL COMMISSION, TOTAL SWAP, REALIZEDPL
            //            if (newStatementInvestor.ListHistory != null)
            //            {
            //                int countHistory = newStatementInvestor.ListHistory.Count;
            //                for (int i = 0; i < countHistory; i++)
            //                {
            //                    ServerBackupLibrary.Business.OpenTrade newCommandHistory = new ServerBackupLibrary.Business.OpenTrade();

            //                    if (newStatementInvestor.ListHistory[i].Type.ID == 21)
            //                        continue;

            //                    if (newStatementInvestor.ListHistory[i].Type.ID == 13 || newStatementInvestor.ListHistory[i].Type.ID == 14 ||
            //                        newStatementInvestor.ListHistory[i].Type.ID == 15 || newStatementInvestor.ListHistory[i].Type.ID == 16)
            //                    {
            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 13)
            //                            Deposit += newStatementInvestor.ListHistory[i].Profit;

            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 14)
            //                            Withdrawal += newStatementInvestor.ListHistory[i].Profit;

            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 15)
            //                            creditIn += newStatementInvestor.ListHistory[i].Profit;

            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 16)
            //                            creditOut += newStatementInvestor.ListHistory[i].Profit;
            //                    }
            //                    else
            //                    {
            //                        totalCommssionClose += newStatementInvestor.ListHistory[i].Commission;
            //                        totalSwapClose += newStatementInvestor.ListHistory[i].Swap;
            //                        realizedPL += newStatementInvestor.ListHistory[i].Profit;
            //                    }
            //                }
            //            }
            //            #endregion

            //            #region PROCESS INSERT ACCOUNT STATEMENT
            //            if (newStatementInvestor.InvestorAccount != null && newStatementInvestor.InvestorAccount.InvestorID > 0)
            //            {
            //                ServerBackupLibrary.Business.InvestorAccount newInvestorAccount = new ServerBackupLibrary.Business.InvestorAccount();
            //                ServerBackupLibrary.Business.AccountSummary newAccountSummary = new ServerBackupLibrary.Business.AccountSummary();

            //                newInvestorAccount.Code = newStatementInvestor.InvestorAccount.Code;
            //                newInvestorAccount.Currency = "USD";
            //                newInvestorAccount.InvestorID = newStatementInvestor.InvestorAccount.InvestorID;
            //                newInvestorAccount.LogDate = DateTime.Parse(newStatementInvestor.Date);
            //                newInvestorAccount.Name = newStatementInvestor.InvestorAccount.NickName;

            //                double previousLedgerBalance = TradingServer.Facade.FacadeGetPreviousLedgerBalance(newStatementInvestor.InvestorAccount.InvestorID);
            //                balance = Math.Round(previousLedgerBalance + realizedPL + totalSwapClose + totalSwapOpen + Deposit - Withdrawal, 2);

            //                newInvestorAccount.Balance = balance;
            //                newInvestorAccount.CreditIn = creditIn;
            //                newInvestorAccount.CreditOut = creditOut;
            //                newInvestorAccount.Deposit = Deposit;
            //                newInvestorAccount.Equity = Math.Round(balance + unRealizedPL + newStatementInvestor.InvestorAccount.Credit, 2);
            //                //newInvestorAccount.FreeMargin = newAccountSummary.Equity - (newStatementInvestor.InvestorAccount.Margin + newStatementInvestor.InvestorAccount.FreezeMargin);
            //                newInvestorAccount.InvestorID = newStatementInvestor.InvestorAccount.InvestorID;
            //                newInvestorAccount.Margin = newStatementInvestor.InvestorAccount.Margin + newStatementInvestor.InvestorAccount.FreezeMargin;

            //                double marginLevel = 0;
            //                //if ((newStatementInvestor.InvestorAccount.Margin + newStatementInvestor.InvestorAccount.FreezeMargin) > 0)
            //                //    marginLevel = (newAccountSummary.Equity * 100) / (newStatementInvestor.InvestorAccount.Margin + newStatementInvestor.InvestorAccount.FreezeMargin);

            //                newInvestorAccount.MarginLevel = marginLevel;
            //                newInvestorAccount.PreviousLedger = previousLedgerBalance;
            //                newInvestorAccount.RealizePL = realizedPL;
            //                newInvestorAccount.Swap = totalSwapOpen;
            //                newInvestorAccount.TotalCommissionClose = totalCommssionClose;
            //                newInvestorAccount.TotalCommissionOpen = totalCommssionOpen;
            //                newInvestorAccount.TotalSwapClose = totalSwapClose;
            //                newInvestorAccount.TotalSwapOpen = totalSwapOpen;
            //                newInvestorAccount.UnrealizedPL = unRealizedPL;
            //                newInvestorAccount.Withdrawal = Withdrawal;

            //                investorID = ServerBackupLibrary.FacadeDB.FacadeInsertInvestorAccount(dbName, newInvestorAccount);
            //            }
            //            #endregion

            //            #region PROCESS INSERT DATABASE HISTORY COMMAND
            //            if (newStatementInvestor.ListHistory != null)
            //            {
            //                int countHistory = newStatementInvestor.ListHistory.Count;
            //                for (int i = 0; i < countHistory; i++)
            //                {
            //                    ServerBackupLibrary.Business.OpenTrade newCommandHistory = new ServerBackupLibrary.Business.OpenTrade();

            //                    if (newStatementInvestor.ListHistory[i].Type.ID == 21)
            //                        continue;

            //                    if (newStatementInvestor.ListHistory[i].Type.ID == 13 || newStatementInvestor.ListHistory[i].Type.ID == 14 ||
            //                        newStatementInvestor.ListHistory[i].Type.ID == 15 || newStatementInvestor.ListHistory[i].Type.ID == 16)
            //                    {
            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 13)
            //                            Deposit += newStatementInvestor.ListHistory[i].Profit;

            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 14)
            //                            Withdrawal += newStatementInvestor.ListHistory[i].Profit;

            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 15)
            //                            creditIn += newStatementInvestor.ListHistory[i].Profit;

            //                        if (newStatementInvestor.ListHistory[i].Type.ID == 16)
            //                            creditOut += newStatementInvestor.ListHistory[i].Profit;
            //                    }
            //                    else
            //                    {                                    
            //                        newCommandHistory.AgentCommission = newStatementInvestor.ListHistory[i].AgentCommission;
            //                        newCommandHistory.ClientCode = newStatementInvestor.ListHistory[i].ClientCode;
            //                        newCommandHistory.ClosePrice = newStatementInvestor.ListHistory[i].ClosePrice;
            //                        newCommandHistory.CloseTime = newStatementInvestor.ListHistory[i].CloseTime;
            //                        newCommandHistory.CommandCode = newStatementInvestor.ListHistory[i].CommandCode;
            //                        newCommandHistory.CommandTypeID = newStatementInvestor.ListHistory[i].Type.ID;
            //                        newCommandHistory.Comment = newStatementInvestor.ListHistory[i].Comment;
            //                        newCommandHistory.Commission = newStatementInvestor.ListHistory[i].Commission;
            //                        newCommandHistory.ExpTime = newStatementInvestor.ListHistory[i].ExpTime;
            //                        newCommandHistory.InvestorID = investorID;
            //                        newCommandHistory.IsClose = newStatementInvestor.ListHistory[i].IsClose;
            //                        newCommandHistory.IsDelete = false;
            //                        newCommandHistory.OpenPrice = newStatementInvestor.ListHistory[i].OpenPrice;
            //                        newCommandHistory.OpenTime = newStatementInvestor.ListHistory[i].OpenTime;
            //                        newCommandHistory.OpenTradeID = newStatementInvestor.ListHistory[i].ID;
            //                        newCommandHistory.Profit = newStatementInvestor.ListHistory[i].Profit;
            //                        newCommandHistory.Size = newStatementInvestor.ListHistory[i].Size;
            //                        newCommandHistory.StopLoss = newStatementInvestor.ListHistory[i].StopLoss;
            //                        newCommandHistory.Swap = newStatementInvestor.ListHistory[i].Swap;
            //                        newCommandHistory.SymbolName = newStatementInvestor.ListHistory[i].Symbol.Name;
            //                        newCommandHistory.TakeProfit = newStatementInvestor.ListHistory[i].TakeProfit;
            //                        newCommandHistory.Taxes = newStatementInvestor.ListHistory[i].Taxes;
            //                        newCommandHistory.TotalSwaps = newStatementInvestor.ListHistory[i].TotalSwap;
            //                        newCommandHistory.Pips = newStatementInvestor.ListHistory[i].ClosePrice - newStatementInvestor.ListHistory[i].OpenPrice;

            //                        totalCommssionClose += newStatementInvestor.ListHistory[i].Commission;
            //                        totalSwapClose += newStatementInvestor.ListHistory[i].Swap;
            //                        unRealizedPL += newStatementInvestor.ListHistory[i].Profit;
            //                    }

            //                    ServerBackupLibrary.FacadeDB.FacadeInsertOpenTrade(dbName, newCommandHistory, false);
            //                }
            //            }
            //            #endregion

            //            #region PROCESS INSERT DATABASE OPEN POSITION
            //            if (newStatementInvestor.ListOpenPosition != null)
            //            {
            //                int countOpenPosition = newStatementInvestor.ListOpenPosition.Count;
            //                for (int i = 0; i < countOpenPosition; i++)
            //                {
            //                    ServerBackupLibrary.Business.OpenTrade newOpenPosition = new ServerBackupLibrary.Business.OpenTrade();
            //                    newOpenPosition.AgentCommission = newStatementInvestor.ListOpenPosition[i].AgentCommission;
            //                    newOpenPosition.ClientCode = newStatementInvestor.ListOpenPosition[i].ClientCode;
            //                    newOpenPosition.ClosePrice = newStatementInvestor.ListOpenPosition[i].ClosePrice;
            //                    newOpenPosition.CloseTime = newStatementInvestor.ListOpenPosition[i].CloseTime;
            //                    newOpenPosition.CommandCode = newStatementInvestor.ListOpenPosition[i].CommandCode;
            //                    newOpenPosition.CommandTypeID = newStatementInvestor.ListOpenPosition[i].Type.ID;
            //                    newOpenPosition.Comment = newStatementInvestor.ListOpenPosition[i].Comment;
            //                    newOpenPosition.Commission = newStatementInvestor.ListOpenPosition[i].Commission;
            //                    newOpenPosition.ExpTime = newStatementInvestor.ListOpenPosition[i].ExpTime;
            //                    newOpenPosition.InvestorID =investorID;
            //                    newOpenPosition.IsClose = newStatementInvestor.ListOpenPosition[i].IsClose;
            //                    newOpenPosition.IsDelete = false;
            //                    newOpenPosition.OpenPrice = newStatementInvestor.ListOpenPosition[i].OpenPrice;
            //                    newOpenPosition.OpenTime = newStatementInvestor.ListOpenPosition[i].OpenTime;
            //                    newOpenPosition.OpenTradeID = newStatementInvestor.ListOpenPosition[i].ID;
            //                    newOpenPosition.Pips = newStatementInvestor.ListOpenPosition[i].ClosePrice - newStatementInvestor.ListOpenPosition[i].OpenPrice;
            //                    newOpenPosition.Profit = newStatementInvestor.ListOpenPosition[i].Profit;
            //                    newOpenPosition.Size = newStatementInvestor.ListOpenPosition[i].Size;
            //                    newOpenPosition.StopLoss = newStatementInvestor.ListOpenPosition[i].StopLoss;
            //                    newOpenPosition.Swap = newStatementInvestor.ListOpenPosition[i].Swap;
            //                    newOpenPosition.SymbolName = newStatementInvestor.ListOpenPosition[i].Symbol.Name;
            //                    newOpenPosition.TakeProfit = newStatementInvestor.ListOpenPosition[i].TakeProfit;
            //                    newOpenPosition.Taxes = newStatementInvestor.ListOpenPosition[i].Taxes;
            //                    newOpenPosition.TotalSwaps = newStatementInvestor.ListOpenPosition[i].TotalSwap;

            //                    totalCommssionOpen += newStatementInvestor.ListOpenPosition[i].Commission;
            //                    totalSwapOpen += newStatementInvestor.ListOpenPosition[i].Swap;
            //                    unRealizedPL += newStatementInvestor.ListOpenPosition[i].Profit;

            //                    ServerBackupLibrary.FacadeDB.FacadeInsertOpenTrade(dbName, newOpenPosition, true);
            //                }
            //            }
            //            #endregion
            //        }   
 
            //        newStatementInvestor = Business.Market.GetStatementInvestor();

            //        System.Threading.Thread.Sleep(50);
            //    }

            //    System.Threading.Thread.Sleep(60000);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Business.StatementInvestor GetStatementInvestor()
        {
            Business.StatementInvestor result = null;
            if (Business.Market.ListStatementEOD != null && Business.Market.ListStatementEOD.Count > 0)
            {
                result = Business.Market.ListStatementEOD[0];
                Business.Market.ListStatementEOD.RemoveAt(0);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void ProcessLastBalance()
        {   
            Business.SumLastAccount newLastAccount;
            while (Business.Market.IsProcessLastAccount)
            {
                newLastAccount = Business.Market.GetLastAccount();
                List<Business.LastBalance> listLastBalance = new List<LastBalance>();
                while (newLastAccount != null)
                {
                    double PLBalance = 0;
                    double closePL = 0;
                    double deposit = 0;
                    double balance = 0;
                    double floatingPL = 0;
                    double creditIn = 0;
                    double creditOut = 0;
                    double equity = 0;                    
                    double withdrawal = 0;
                    double freeMargin = 0;
                    double swap = 0;
                    double commission = 0;
                    double volume = 0;
                        
                    //PROCESS EXECUTOR LAST ACCOUNT
                    if (newLastAccount != null)
                    {
                        #region PROCESS EXECUTOR HISTORY
                        if (newLastAccount.ListHistory != null)
                        {
                            int count = newLastAccount.ListHistory.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (newLastAccount.ListHistory[i].Type.ID == 21)
                                    continue;

                                if (newLastAccount.ListHistory[i].Type.ID == 13 || newLastAccount.ListHistory[i].Type.ID == 14 ||
                                    newLastAccount.ListHistory[i].Type.ID == 15 || newLastAccount.ListHistory[i].Type.ID == 16)
                                {
                                    if (newLastAccount.ListHistory[i].Type.ID == 13)
                                        deposit += newLastAccount.ListHistory[i].Profit;

                                    if (newLastAccount.ListHistory[i].Type.ID == 14)
                                        withdrawal += newLastAccount.ListHistory[i].Profit;

                                    if (newLastAccount.ListHistory[i].Type.ID == 15)
                                        creditIn += newLastAccount.ListHistory[i].Profit;

                                    if (newLastAccount.ListHistory[i].Type.ID == 16)
                                        creditOut += newLastAccount.ListHistory[i].Profit;
                                }
                                else
                                {
                                    closePL += newLastAccount.ListHistory[i].Profit;
                                    swap += newLastAccount.ListHistory[i].Swap;
                                    commission += newLastAccount.ListHistory[i].Commission;
                                    volume += newLastAccount.ListHistory[i].Size;
                                }
                            }
                        }
                        #endregion

                        #region PROCESS EXECUTOR OPEN TRADE
                        if (newLastAccount.ListOpenTrade != null)
                        {
                            int count = newLastAccount.ListOpenTrade.Count;
                            for (int i = 0; i < count; i++)
                            {
                                floatingPL += newLastAccount.ListOpenTrade[i].Profit + newLastAccount.ListOpenTrade[i].Swap + 
                                    newLastAccount.ListOpenTrade[i].Commission;
                            }
                        }
                        #endregion
                        
                        #region SAVE FLOATINGPL AND MONTH VOLUME FOR AGENT SERVER
                        newLastAccount.InvestorAccount.FloatingPL = floatingPL;
                        newLastAccount.InvestorAccount.MonthVolume = volume;
                        newLastAccount.InvestorAccount.TimeSave = TimeEndDay;

                        Business.EndOfDayAgent newEODAgent = new EndOfDayAgent();
                        newEODAgent.InvestorID = newLastAccount.InvestorAccount.InvestorID;
                        newEODAgent.FloatingPL = floatingPL;
                        newEODAgent.MonthVolume = volume;

                        Business.Market.ListEODAgent.Add(newEODAgent);
                        bool isEndCalculation = false;
                        if (Business.Market.ListEODAgent.Count == Business.Market.InvestorList.Count)
                        {
                            #region SEND COMMAND ALERT END OF DAY TO AGENT SYSTEM
                            try
                            {   
                                //SEND COMMAND TO AGENT SERVER
                                string msg = "NotifyEndOfDay";
                                Business.AgentNotify newAgentNotify = new AgentNotify();
                                newAgentNotify.NotifyMessage = msg;
                                TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion

                            isEndCalculation = true;
                        }
                        #endregion

                        #region GET PREVIOUSLEDGERBALANCE = LAST BALANCE IN COMMAND HISTORY WITH TYPE = 21
                        DateTime tempTimeStart = newLastAccount.TimeEndDay.AddDays(-1);

                        if (tempTimeStart.DayOfWeek == DayOfWeek.Sunday)
                            tempTimeStart = tempTimeStart.AddDays(-2);

                        if (tempTimeStart.DayOfWeek == DayOfWeek.Saturday)
                            tempTimeStart = tempTimeStart.AddDays(-1);

                        DateTime timeStartLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 00, 00, 00);
                        DateTime timeEndLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 23, 59, 59);
                            
                        TradingServer.Business.OpenTrade LastBalance = TradingServer.Facade.FacadeGetLastBalanceByInvestor(newLastAccount.InvestorAccount.InvestorID, timeStartLastBalance, 21, timeEndLastBalance);

                        if (LastBalance != null)
                            PLBalance = LastBalance.Profit;
                        #endregion

                        #region CALCULATION ACCOUNT
                        balance = PLBalance + closePL + commission + swap + deposit - withdrawal;
                        equity = balance + floatingPL + newLastAccount.InvestorAccount.Credit;
                        #endregion

                        #region CALCULATION FREE MARGIN
                        freeMargin = equity - newLastAccount.InvestorAccount.Margin;
                        #endregion

                        List<Business.OpenTrade> listOpenTradeMonth = 
                                            TradingServer.Facade.FacadeGetHistoryByInvestorInMonth(newLastAccount.InvestorAccount.InvestorID);

                        double monthSize = 0;
                        if (listOpenTradeMonth != null)
                        {
                            int count = listOpenTradeMonth.Count;
                            for (int j = 0; j < count; j++)
                            {
                                monthSize += listOpenTradeMonth[j].Size;
                            }
                        }

                        #region MAP LAST ACCOUNT
                        Business.LastBalance newLastAccountEOD = new LastBalance();
                        newLastAccountEOD.InvestorID = newLastAccount.InvestorAccount.InvestorID;
                        newLastAccountEOD.LoginCode = newLastAccount.InvestorAccount.Code;
                        newLastAccountEOD.PLBalance = PLBalance;
                        newLastAccountEOD.ClosePL = closePL;
                        newLastAccountEOD.Deposit = deposit;
                        newLastAccountEOD.Balance = balance;
                        newLastAccountEOD.FloatingPL = floatingPL;
                        newLastAccountEOD.Credit = creditIn;
                        newLastAccountEOD.CreditOut = creditOut;
                        newLastAccountEOD.LastEquity = equity;
                        newLastAccountEOD.Withdrawal = withdrawal;
                        newLastAccountEOD.LogDate = TimeEndDay;
                        newLastAccountEOD.CreditAccount = newLastAccount.InvestorAccount.Credit;
                        newLastAccountEOD.LastMargin = newLastAccount.InvestorAccount.Margin;
                        newLastAccountEOD.FreeMargin = freeMargin;
                        newLastAccountEOD.MonthSize = monthSize;
                        #endregion

                        #region SEND NOTIFY LIST LAST ACCOUNT TO AGENT
                        //SEND NOTIFY LIST LAST ACCOUNT TO AGENT
                        if (isEndCalculation)
                        {
                            string message = "NotifyLastAccount$";
                            //NotifyLastAccount$InvestorID{LoginCode~CommandID{CommandCode{Profit{Commission{Swap|CommandID{CommandCode{Profit{Commission{Swap|....
                            message += newLastAccountEOD.InvestorID + "{" + newLastAccountEOD.LoginCode + "~";
                            if (newLastAccount.ListOpenTrade != null)
                            {
                                int count = newLastAccount.ListOpenTrade.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    message += newLastAccount.ListOpenTrade[i].ID + "{" + newLastAccount.ListOpenTrade[i].CommandCode + "{" +
                                        newLastAccount.ListOpenTrade[i].Profit + "{" + newLastAccount.ListOpenTrade[i].Commission + "{" +
                                        newLastAccount.ListOpenTrade[i].Swap + "|";
                                }
                            }

                            if (message.EndsWith("|"))
                                message.Remove(message.Length - 1, 1);

                            Business.AgentNotify newAgentNotify = new AgentNotify();
                            newAgentNotify.NotifyMessage = message;
                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);

                            //SEND NOTIFY END CALCULATION LAST ACCOUNT
                            string msgEndCalculation = "NotifyEndCalculation$";
                            Business.AgentNotify notifyEndCalculation = new AgentNotify();
                            notifyEndCalculation.NotifyMessage = msgEndCalculation;
                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(notifyEndCalculation);
                        }
                        else
                        {
                            string message = "NotifyLastAccount$";
                            //NotifyLastAccount$InvestorID{LoginCode~CommandID{CommandCode{Profit{Commission{Swap|CommandID{CommandCode{Profit{Commission{Swap|....
                            message += newLastAccountEOD.InvestorID + "{" + newLastAccountEOD.LoginCode + "~";
                            if (newLastAccount.ListOpenTrade != null)
                            {
                                int count = newLastAccount.ListOpenTrade.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    message += newLastAccount.ListOpenTrade[i].ID + "{" + newLastAccount.ListOpenTrade[i].CommandCode + "{" +
                                        newLastAccount.ListOpenTrade[i].Profit + "{" + newLastAccount.ListOpenTrade[i].Commission + "{" +
                                        newLastAccount.ListOpenTrade[i].Swap + "|";
                                }
                            }

                            if (message.EndsWith("|"))
                                message.Remove(message.Length - 1, 1);

                            Business.AgentNotify newAgentNotify = new AgentNotify();
                            newAgentNotify.NotifyMessage = message;
                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                        }
                        #endregion
                        
                        listLastBalance.Add(newLastAccountEOD);
                    }
                    newLastAccount = Business.Market.GetLastAccount();
                }

                if (listLastBalance != null && listLastBalance.Count > 0)
                    TradingServer.Facade.FacadeAddNewLastAccount(listLastBalance);

                System.Threading.Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Business.SumLastAccount GetLastAccount()
        {
            Business.SumLastAccount result = null;
            if (Business.Market.ListLastAccount != null && Business.Market.ListLastAccount.Count > 0)
            {
                result = Business.Market.ListLastAccount[0];
                Business.Market.ListLastAccount.RemoveAt(0);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void ProcessTickQueueAgent()
        {
            string result = string.Empty;
            while (Business.Market.IsProcessTickAgent)
            {
                result = Business.Market.marketInstance.GetTickQueueAgent();
                while (!string.IsNullOrEmpty(result))
                {
                    if (Business.Market.ListAgentConfig != null)
                    {
                        int count = Business.Market.ListAgentConfig.Count;
                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                ASCIIEncoding encoding = new ASCIIEncoding();
                                string postData = "js=";

                                postData += result;

                                byte[] data = encoding.GetBytes(postData);

                                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Business.Market.ListAgentConfig[i].DomainAccess);
                                myRequest.Method = "POST";
                                myRequest.ContentType = "application/x-www-form-urlencoded";
                                myRequest.ContentLength = data.Length;

                                Stream newStream = myRequest.GetRequestStream();

                                //Send the data
                                newStream.Write(data, 0, data.Length);
                                newStream.Close();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    result = Business.Market.marketInstance.GetTickQueueAgent();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetTickQueueAgent()
        {
            string result = string.Empty;

            if (Business.Market.ListTickQueueAgent != null && Business.Market.ListTickQueueAgent.Count > 0)
            {
                int count = Business.Market.ListTickQueueAgent.Count;
                for (int i = 0; i < Business.Market.ListTickQueueAgent.Count; i++)
                {
                    if (Business.Market.ListTickQueueAgent[0] != null)
                    {
                        result += Business.Market.ListTickQueueAgent[0] + "|";
                        Business.Market.ListTickQueueAgent.RemoveAt(0);
                    }
                    else
                    {
                        Business.Market.ListTickQueueAgent.RemoveAt(0);
                    }
                }
            }

            if (result.EndsWith("|"))
                result = result.Remove(result.Length - 1, 1);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void ProcessNotifyQueueAgent()
        {
            Business.AgentNotify result = null;
            while (Business.Market.IsProcessNotifyAgent)
            {
                result = Business.Market.marketInstance.GetNotifyQueueAgent();
                while (result != null)
                {
                    if (result.InstanceAgent != null)
                    {
                        try
                        {
                            if (Business.Market.ListAgentConfig != null)
                            {
                                int count = Business.Market.ListAgentConfig.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (Business.Market.IsConnectAgent)
                                    {
                                        ASCIIEncoding encoding = new ASCIIEncoding();
                                        string postData = "js=";

                                        postData += result.NotifyMessage;

                                        byte[] data = encoding.GetBytes(postData);

                                        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Business.Market.ListAgentConfig[i].DomainAccess);
                                        myRequest.Timeout = 60000;
                                        myRequest.Method = "POST";
                                        myRequest.ContentType = "application/x-www-form-urlencoded";
                                        myRequest.ContentLength = data.Length;

                                        Stream newStream = myRequest.GetRequestStream();

                                        //Send the data
                                        newStream.Write(data, 0, data.Length);
                                        newStream.Close();
                                    }
                                    else
                                    {
                                        string[] subValue = result.NotifyMessage.Split('$');
                                        if (subValue[0] == "CloseCommandByManager" || subValue[0] == "CloseCommand")
                                        {
                                            Business.Market.BKListNotifyQueueAgent.Add(result);
                                            TradingServer.Facade.FacadeAddNewSystemLog(6, result.NotifyMessage, "[Debug BKCommand]", "", "");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }

                    result = Business.Market.marketInstance.GetNotifyQueueAgent();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Business.AgentNotify GetNotifyQueueAgent()
        {
            Business.AgentNotify result = null;
            if (Business.Market.ListNotifyQueueAgent != null && Business.Market.ListNotifyQueueAgent.Count > 0)
            {
                if (Business.Market.ListNotifyQueueAgent[0] != null)
                {
                    result = Business.Market.ListNotifyQueueAgent[0];
                    Business.Market.ListNotifyQueueAgent.RemoveAt(0);
                }
                else
                {
                    Business.Market.ListNotifyQueueAgent.RemoveAt(0);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void ProcessSaveStatement()
        {
            Business.Statement result = null;

            while (Business.Market.IsProcessSaveStatement)
            {
                result = Business.Market.marketInstance.GetStatement();

                List<Business.Statement> listStatement = new List<Statement>();
                while (result != null)
                {
                    listStatement.Add(result);
                    result = Business.Market.marketInstance.GetStatement();
                }

                //PROCESS SAVE TO DATABASE
                if (listStatement != null && listStatement.Count > 0)
                    Business.Statement.Instance.AddNewStatement(listStatement);

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Business.Statement GetStatement()
        {
            Business.Statement result = null;

            if (Business.Market.ListStatement != null && Business.Market.ListStatement.Count > 0)
            {
                if (Business.Market.ListStatement[0] != null)
                {
                    result = Business.Market.ListStatement[0];
                    Business.Market.ListStatement.RemoveAt(0);
                }
                else
                    Business.Market.ListStatement.RemoveAt(0);
            }

            return result;
        }
    }
}
