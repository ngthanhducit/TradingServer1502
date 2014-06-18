using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWLastBalance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Business.LastBalance> MapLastBalance(DS.LastAccountDataTable tbLastAccount)
        {
            List<Business.LastBalance> result = new List<Business.LastBalance>();

            if (tbLastAccount != null)
            {
                int count = tbLastAccount.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.LastBalance newLastBalance = new Business.LastBalance();
                    newLastBalance.Balance = tbLastAccount[i].Balance;
                    newLastBalance.ClosePL = tbLastAccount[i].ClosePL;
                    newLastBalance.Credit = tbLastAccount[i].Credit;
                    newLastBalance.Deposit = tbLastAccount[i].Deposit;
                    newLastBalance.FloatingPL = tbLastAccount[i].FloatingPL;
                    newLastBalance.FreeMargin = tbLastAccount[i].FreeMargin;
                    newLastBalance.InvestorID = tbLastAccount[i].InvestorID;
                    newLastBalance.LastAccountID = tbLastAccount[i].LastAccountID;
                    newLastBalance.LastEquity = tbLastAccount[i].Equity;
                    newLastBalance.LastMargin = tbLastAccount[i].Margin;
                    newLastBalance.LogDate = tbLastAccount[i].LogDate;
                    newLastBalance.LoginCode = tbLastAccount[i].LoginCode;
                    newLastBalance.PLBalance = tbLastAccount[i].PLBalance;
                    newLastBalance.CreditOut = tbLastAccount[i].CreditOut;
                    newLastBalance.Withdrawal = tbLastAccount[i].Withdrawal;
                    newLastBalance.CreditAccount = tbLastAccount[i].CreditAccount;

                    result.Add(newLastBalance);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.LastBalance> GetData()
        {
            List<Business.LastBalance> result = new List<Business.LastBalance>();

            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            DS.LastAccountDataTable tbLastBalance = new DS.LastAccountDataTable();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbLastBalance = adap.GetData();

                result = this.MapLastBalance(tbLastBalance);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int InsertLastAccount(Business.LastBalance value)
        {
            int result = -1;
            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = int.Parse(adap.AddNewLastAccount(value.InvestorID, value.LoginCode, value.PLBalance, value.ClosePL,
                                    value.Deposit, value.Balance, value.FloatingPL, value.Credit, value.LastEquity, value.LastMargin,
                                    value.FreeMargin, value.LogDate, value.CreditOut, value.Withdrawal,value.CreditAccount,value.MonthSize).ToString());
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int InsertLastAccount(List<Business.LastBalance> values)
        {
            int result = -1;
            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                if (values != null && values.Count > 0)
                {
                    conn.Open();
                    adap.Connection = conn;
                    int count = values.Count;
                    for (int i = 0; i < count; i++)
                    {
                        result = int.Parse(adap.AddNewLastAccount(values[i].InvestorID, values[i].LoginCode, values[i].PLBalance, values[i].ClosePL,
                                        values[i].Deposit, values[i].Balance, values[i].FloatingPL, values[i].Credit, values[i].LastEquity, values[i].LastMargin,
                                        values[i].FreeMargin, values[i].LogDate, values[i].CreditOut, values[i].Withdrawal, values[i].CreditAccount, values[i].MonthSize).ToString());
                    }
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal Business.LastBalance GetLastAccountByTime(int investorID, DateTime timeStart, DateTime timeEnd)
        {
            Business.LastBalance result = new Business.LastBalance();
            DS.LastAccountDataTable tbLastAccount = new DS.LastAccountDataTable();
            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbLastAccount = adap.GetLastAccountByDate(investorID, timeStart, timeEnd);

                if (tbLastAccount != null)
                {
                    result.Balance = tbLastAccount[0].Balance;
                    result.ClosePL = tbLastAccount[0].ClosePL;
                    result.Credit = tbLastAccount[0].Credit;
                    result.Deposit = tbLastAccount[0].Deposit;
                    result.FloatingPL = tbLastAccount[0].FloatingPL;
                    result.FreeMargin = tbLastAccount[0].FreeMargin;
                    result.InvestorID = tbLastAccount[0].InvestorID;
                    result.LastAccountID = tbLastAccount[0].LastAccountID;
                    result.LastEquity = tbLastAccount[0].Equity;
                    result.LastMargin = tbLastAccount[0].Margin;
                    result.LogDate = tbLastAccount[0].LogDate;
                    result.LoginCode = tbLastAccount[0].LoginCode;
                    result.PLBalance = tbLastAccount[0].PLBalance;
                    result.CreditOut = tbLastAccount[0].CreditOut;
                    result.Withdrawal = tbLastAccount[0].Withdrawal;
                    result.CreditAccount = tbLastAccount[0].CreditAccount;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal List<Business.LastBalance> GetLastAccountByTimeListInvestorID(List<int> listInvestorID, DateTime timeStart, DateTime timeEnd)
        {
            List<Business.LastBalance> result = new List<Business.LastBalance>();
            DS.LastAccountDataTable tbLastAccount = new DS.LastAccountDataTable();
            DS.LastAccountDataTable tbPLBalance = new DS.LastAccountDataTable();

            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            System.Data.SqlClient.SqlDataAdapter sqlAdap = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (listInvestorID != null)
                {
                    int count = listInvestorID.Count;

                    if (timeStart.DayOfWeek == DayOfWeek.Saturday)
                        timeStart = timeStart.AddDays(-1);

                    if (timeStart.DayOfWeek == DayOfWeek.Sunday)
                        timeStart = timeStart.AddDays(-2);

                    DateTime tempTimeStart = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 00, 00, 00);
                    DateTime tempTimeEnd = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 23, 59, 59);

                    string strInvestor = string.Empty;
                    for (int i = 0; i < count; i++)
                    {
                        if (i == count - 1)
                            strInvestor += listInvestorID[i];
                        else
                            strInvestor += listInvestorID[i] + ",";
                    }

                    string sql = "SELECT Balance, ClosePL, Credit, CreditAccount, CreditOut, Deposit, Equity, FloatingPL, FreeMargin, InvestorID," +
                                    " LastAccountID, LogDate, LoginCode, Margin, MonthSize, PLBalance, Withdrawal " +
                                  "FROM LastAccount " +
                                  "WHERE (InvestorID IN(" + strInvestor + ")) AND (LogDate >= '" + timeStart + "') AND (LogDate <= '" + timeEnd + "')";

                    sqlAdap.SelectCommand = new System.Data.SqlClient.SqlCommand(sql, conn);
                    sqlAdap.Fill(tbLastAccount);

                    for (int i = 0; i < count; i++)
                    {
                        bool isExitst = false;
                        if (tbLastAccount != null)
                        {
                            int countLastAccount = tbLastAccount.Count;
                            for (int j = 0; j < countLastAccount; j++)
                            {
                                if (tbLastAccount[j].InvestorID == listInvestorID[i])
                                {
                                    #region MAP DATA
                                    Business.LastBalance newLastBalance = new Business.LastBalance();

                                    newLastBalance.Balance = tbLastAccount[j].Balance;
                                    newLastBalance.ClosePL = tbLastAccount[j].ClosePL;
                                    newLastBalance.Credit = tbLastAccount[j].Credit;
                                    newLastBalance.Deposit = tbLastAccount[j].Deposit;
                                    newLastBalance.FloatingPL = tbLastAccount[j].FloatingPL;
                                    newLastBalance.FreeMargin = tbLastAccount[j].FreeMargin;
                                    newLastBalance.InvestorID = tbLastAccount[j].InvestorID;
                                    newLastBalance.LastAccountID = tbLastAccount[j].LastAccountID;
                                    newLastBalance.LastEquity = tbLastAccount[j].Equity;
                                    newLastBalance.LastMargin = tbLastAccount[j].Margin;
                                    newLastBalance.LogDate = tbLastAccount[j].LogDate;
                                    newLastBalance.LoginCode = tbLastAccount[j].LoginCode;
                                    newLastBalance.PLBalance = tbLastAccount[j].PLBalance;
                                    newLastBalance.CreditOut = tbLastAccount[j].CreditOut;
                                    newLastBalance.Withdrawal = tbLastAccount[j].Withdrawal;
                                    newLastBalance.CreditAccount = tbLastAccount[j].CreditAccount;

                                    result.Add(newLastBalance);

                                    isExitst = true;
                                    break;
                                    #endregion
                                }
                            }
                        }

                        if (!isExitst)
                        {
                            #region DEFAULT DATA
                            Business.LastBalance newLastBalance = new Business.LastBalance();

                            newLastBalance.Balance = 0;
                            newLastBalance.ClosePL = 0;
                            newLastBalance.Credit = 0;
                            newLastBalance.Deposit = 0;
                            newLastBalance.FloatingPL = 0;
                            newLastBalance.FreeMargin = 0;
                            newLastBalance.InvestorID = listInvestorID[i];
                            newLastBalance.LastAccountID = 0;
                            newLastBalance.LastEquity = 0;
                            newLastBalance.LastMargin = 0;
                            newLastBalance.LogDate = DateTime.Now;
                            newLastBalance.LoginCode = "";
                            newLastBalance.PLBalance = 0;
                            newLastBalance.CreditOut = 0;
                            newLastBalance.Withdrawal = 0;
                            newLastBalance.CreditAccount = 0;

                            result.Add(newLastBalance); 
                            #endregion
                        }
                    }

                    #region COMMENT CODE BOI VI NEU SU DUNG ADAP CUA DATASET CALL LAY DATA THI SE BI TIMEOUT
                    //for (int i = 0; i < count; i++)
                    //{
                    //    tbLastAccount = adap.GetLastAccountByDate(listInvestorID[i], timeStart, timeEnd);

                    //    var temp = from n in tbLastAccount
                    //               orderby n.LastAccountID descending
                    //               select n;

                    //    //GET PL BALANCE WITH TIME START
                    //    //tbPLBalance = adap.GetLastAccountByDate(listInvestorID[i], tempTimeStart, tempTimeEnd);

                    //    //var tempPLBalance = from n in tbPLBalance
                    //    //                    orderby n.LastAccountID descending
                    //    //                    select n;

                    //    if (tbPLBalance == null || tbLastAccount == null || temp == null)
                    //        continue;

                    //    if (temp != null && temp.Count() > 0)
                    //    {
                    //        Business.LastBalance newLastBalance = new Business.LastBalance();

                    //        newLastBalance.Balance = temp.ElementAt(0).Balance;
                    //        newLastBalance.ClosePL = temp.ElementAt(0).ClosePL;
                    //        newLastBalance.Credit = temp.ElementAt(0).Credit;
                    //        newLastBalance.Deposit = temp.ElementAt(0).Deposit;
                    //        newLastBalance.FloatingPL = temp.ElementAt(0).FloatingPL;
                    //        newLastBalance.FreeMargin = temp.ElementAt(0).FreeMargin;
                    //        newLastBalance.InvestorID = temp.ElementAt(0).InvestorID;
                    //        newLastBalance.LastAccountID = temp.ElementAt(0).LastAccountID;
                    //        newLastBalance.LastEquity = temp.ElementAt(0).Equity;
                    //        newLastBalance.LastMargin = temp.ElementAt(0).Margin;
                    //        newLastBalance.LogDate = temp.ElementAt(0).LogDate;
                    //        newLastBalance.LoginCode = temp.ElementAt(0).LoginCode;

                    //        newLastBalance.PLBalance = temp.ElementAt(0).PLBalance;
                    //        //if (tbPLBalance != null && tbPLBalance.Count > 0)
                    //        //    newLastBalance.PLBalance = tbPLBalance[0].PLBalance;
                    //        //else
                    //        //    newLastBalance.PLBalance = 0;

                    //        //if (tbPLBalance != null && tbPLBalance.Count() > 0)
                    //        //    newLastBalance.PLBalance = tbPLBalance[0].PLBalance;
                    //        //else
                    //        //{
                    //            //DateTime tempTimeEndGetDeposit = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);
                    //            //Business.OpenTrade tempGetDepositAcc = TradingServer.Facade.FacadeGetLastBalanceByInvestor(listInvestorID[i], tempTimeStart, 13, tempTimeEndGetDeposit);
                    //            //if (tempGetDepositAcc != null && tempGetDepositAcc.Profit != 0)
                    //            //    newLastBalance.PLBalance = tempGetDepositAcc.Profit;
                    //            //else
                    //        //    newLastBalance.PLBalance = 0;
                    //        //}


                    //        newLastBalance.CreditOut = temp.ElementAt(0).CreditOut;
                    //        newLastBalance.Withdrawal = temp.ElementAt(0).Withdrawal;
                    //        newLastBalance.CreditAccount = temp.ElementAt(0).CreditAccount;

                    //        result.Add(newLastBalance);
                    //    }
                    //    else
                    //    {
                    //        Business.LastBalance newLastBalance = new Business.LastBalance();

                    //        newLastBalance.Balance = 0;
                    //        newLastBalance.ClosePL = 0;
                    //        newLastBalance.Credit = 0;
                    //        newLastBalance.Deposit = 0;
                    //        newLastBalance.FloatingPL = 0;
                    //        newLastBalance.FreeMargin = 0;
                    //        newLastBalance.InvestorID = listInvestorID[i];
                    //        newLastBalance.LastAccountID = 0;
                    //        newLastBalance.LastEquity = 0;
                    //        newLastBalance.LastMargin = 0;
                    //        newLastBalance.LogDate = DateTime.Now;
                    //        newLastBalance.LoginCode = "";
                    //        newLastBalance.PLBalance = 0;
                    //        newLastBalance.CreditOut = 0;
                    //        newLastBalance.Withdrawal = 0;
                    //        newLastBalance.CreditAccount = 0;

                    //        result.Add(newLastBalance);
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        internal List<Business.LastBalance> GetLastAccountByTimeListInvestorID(Dictionary<int, string> listInvestorID, DateTime timeStart, DateTime timeEnd)
        {
            List<Business.LastBalance> result = new List<Business.LastBalance>();
            DS.LastAccountDataTable tbLastAccount = new DS.LastAccountDataTable();
            DS.LastAccountDataTable tbPLBalance = new DS.LastAccountDataTable();

            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (listInvestorID != null)
                {
                    int count = listInvestorID.Count;
                    DateTime tempTimeStart = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 00, 00, 00);
                    DateTime tempTimeEnd = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 23, 59, 59);

                    foreach(KeyValuePair<int, string> pair in listInvestorID)
                    {
                        tbLastAccount = adap.GetLastAccountByDate(pair.Key, timeStart, timeEnd);

                        var temp = from n in tbLastAccount
                                   orderby n.LastAccountID descending
                                   select n;

                        //GET PL BALANCE WITH TIME START
                        tbPLBalance = adap.GetLastAccountByDate(pair.Key, tempTimeStart, tempTimeEnd);

                        if (tbPLBalance == null || tbLastAccount == null || temp == null)
                            continue;

                        if (temp != null && temp.Count() > 0)
                        {
                            Business.LastBalance newLastBalance = new Business.LastBalance();

                            newLastBalance.Balance = temp.ElementAt(0).Balance;
                            newLastBalance.ClosePL = temp.ElementAt(0).ClosePL;
                            newLastBalance.Credit = temp.ElementAt(0).Credit;
                            newLastBalance.Deposit = temp.ElementAt(0).Deposit;
                            newLastBalance.FloatingPL = temp.ElementAt(0).FloatingPL;
                            newLastBalance.FreeMargin = temp.ElementAt(0).FreeMargin;
                            newLastBalance.InvestorID = temp.ElementAt(0).InvestorID;
                            newLastBalance.LastAccountID = temp.ElementAt(0).LastAccountID;
                            newLastBalance.LastEquity = temp.ElementAt(0).Equity;
                            newLastBalance.LastMargin = temp.ElementAt(0).Margin;
                            newLastBalance.LogDate = temp.ElementAt(0).LogDate;
                            newLastBalance.LoginCode = temp.ElementAt(0).LoginCode;

                            if (tbPLBalance != null && tbPLBalance.Count() > 0)
                                newLastBalance.PLBalance = tbPLBalance[0].PLBalance;
                            else
                                newLastBalance.PLBalance = 0;

                            newLastBalance.CreditOut = temp.ElementAt(0).CreditOut;
                            newLastBalance.Withdrawal = temp.ElementAt(0).Withdrawal;
                            newLastBalance.CreditAccount = temp.ElementAt(0).CreditAccount;

                            result.Add(newLastBalance);
                        }
                        else
                        {
                            Business.LastBalance newLastBalance = new Business.LastBalance();

                            newLastBalance.Balance = 0;
                            newLastBalance.ClosePL = 0;
                            newLastBalance.Credit = 0;
                            newLastBalance.Deposit = 0;
                            newLastBalance.FloatingPL = 0;
                            newLastBalance.FreeMargin = 0;
                            newLastBalance.InvestorID = pair.Key;
                            newLastBalance.LastAccountID = 0;
                            newLastBalance.LastEquity = 0;
                            newLastBalance.LastMargin = 0;
                            newLastBalance.LogDate = DateTime.Now;
                            newLastBalance.LoginCode = pair.Value;
                            newLastBalance.PLBalance = 0;
                            newLastBalance.CreditOut = 0;
                            newLastBalance.Withdrawal = 0;
                            newLastBalance.CreditAccount = 0;

                            result.Add(newLastBalance);
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
                conn.Close();
            }

            return result;
        }

        internal List<Business.LastBalance> GetLastAccountByTimeInvestor(Dictionary<int, string> listInvestorID, DateTime timeStart, DateTime timeEnd)
        {
            List<Business.LastBalance> result = new List<Business.LastBalance>();
            DS.LastAccountDataTable tbLastAccount = new DS.LastAccountDataTable();
            DS.LastAccountDataTable tbPLBalance = new DS.LastAccountDataTable();

            DSTableAdapters.LastAccountTableAdapter adap = new DSTableAdapters.LastAccountTableAdapter();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);

            try
            {
                conn.Open();
                adap.Connection = conn;

                if (listInvestorID != null)
                {
                    int count = listInvestorID.Count;
                    DateTime tempTimeStart = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 00, 00, 00);
                    DateTime tempTimeEnd = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 23, 59, 59);

                    foreach (KeyValuePair<int, string> pair in listInvestorID)
                    {
                        tbLastAccount = adap.GetLastAccountByDate(pair.Key, timeStart, timeEnd);

                        var temp = from n in tbLastAccount
                                   orderby n.LastAccountID descending
                                   select n;

                        //GET PL BALANCE WITH TIME START
                        tbPLBalance = adap.GetLastAccountByDate(pair.Key, tempTimeStart, tempTimeEnd);

                        if (tbPLBalance == null || tbLastAccount == null || temp == null)
                            continue;

                        if (temp != null && temp.Count() > 0)
                        {
                            Business.LastBalance newLastBalance = new Business.LastBalance();

                            newLastBalance.Balance = temp.ElementAt(0).Balance;
                            newLastBalance.ClosePL = temp.ElementAt(0).ClosePL;
                            newLastBalance.Credit = temp.ElementAt(0).Credit;
                            newLastBalance.Deposit = temp.ElementAt(0).Deposit;
                            newLastBalance.FloatingPL = temp.ElementAt(0).FloatingPL;
                            newLastBalance.FreeMargin = temp.ElementAt(0).FreeMargin;
                            newLastBalance.InvestorID = temp.ElementAt(0).InvestorID;
                            newLastBalance.LastAccountID = temp.ElementAt(0).LastAccountID;
                            newLastBalance.LastEquity = temp.ElementAt(0).Equity;
                            newLastBalance.LastMargin = temp.ElementAt(0).Margin;
                            newLastBalance.LogDate = temp.ElementAt(0).LogDate;
                            newLastBalance.LoginCode = temp.ElementAt(0).LoginCode;
                            newLastBalance.EndMargin = temp.ElementAt(temp.Count() - 1).Margin;
                            newLastBalance.EndFreeMargin = temp.ElementAt(temp.Count() - 1).FreeMargin;
                            newLastBalance.EndLogDate = temp.ElementAt(temp.Count() - 1).LogDate;

                            if (tbPLBalance != null && tbPLBalance.Count() > 0)
                                newLastBalance.PLBalance = tbPLBalance[0].PLBalance;
                            else
                                newLastBalance.PLBalance = 0;

                            newLastBalance.CreditOut = temp.ElementAt(0).CreditOut;
                            newLastBalance.Withdrawal = temp.ElementAt(0).Withdrawal;
                            newLastBalance.CreditAccount = temp.ElementAt(0).CreditAccount;

                            result.Add(newLastBalance);
                        }
                        else
                        {
                            Business.LastBalance newLastBalance = new Business.LastBalance();

                            newLastBalance.Balance = 0;
                            newLastBalance.ClosePL = 0;
                            newLastBalance.Credit = 0;
                            newLastBalance.Deposit = 0;
                            newLastBalance.FloatingPL = 0;
                            newLastBalance.FreeMargin = 0;
                            newLastBalance.InvestorID = pair.Key;
                            newLastBalance.LastAccountID = -1;
                            newLastBalance.LastEquity = 0;
                            newLastBalance.LastMargin = 0;
                            newLastBalance.LogDate = DateTime.Now;
                            newLastBalance.LoginCode = pair.Value;
                            newLastBalance.PLBalance = 0;
                            newLastBalance.CreditOut = 0;
                            newLastBalance.Withdrawal = 0;
                            newLastBalance.CreditAccount = 0;

                            result.Add(newLastBalance);
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
                conn.Close();
            }

            return result;
        }
    }
}
