using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace TradingServer.Business
{
    public partial class Investor
    {
        /// <summary>
        /// Find Investor With Online Command, Search In Investor List Of Market ,If Code Or Command List False, 
        /// Will Search In Database Server
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.Investor FindInvestor(string Code)
        {
            Business.Investor Result = null;
            bool Flag = false;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == Code)
                    {
                        if (Business.Market.InvestorList[i].CommandList == null)
                            break;

                        //if (Business.Market.InvestorList[i].CommandList.Count > 0)
                        //{
                        Result = Business.Market.InvestorList[i];

                        Flag = true;
                        break;
                        //}
                    }
                }
            }

            if (Flag == false)
            {
                Result = Investor.DBWInvestorInstance.SelectInvestorByCode(Code);
            }

            return Result;
        }

        /// <summary>
        /// Find Investor With Online Command, Search In Investor List Of Market With Condtion Code And Command List > 0
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.Investor FindInvestorWithOnlineCommand(string Code)
        {
            Business.Investor Result = new Investor();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == Code)
                    {
                        if (Business.Market.InvestorList[i].CommandList.Count > 0)
                        {
                            Result = Business.Market.InvestorList[i];
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorWithMarginLevel()
        {
            List<Business.Investor> Result = new List<Investor>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    double MarginCall = 0;
                    if (Business.Market.InvestorList[i].InvestorGroupInstance.ParameterItems != null)
                    {
                        int countParameter = Business.Market.InvestorList[i].InvestorGroupInstance.ParameterItems.Count;
                        for (int j = 0; j < countParameter; j++)
                        {
                            if (Business.Market.InvestorList[i].InvestorGroupInstance.ParameterItems[j].Code == "G19")
                            {
                                double.TryParse(Business.Market.InvestorList[i].InvestorGroupInstance.ParameterItems[j].NumValue, out MarginCall);
                                break;
                            }
                        }
                    }

                    if (Business.Market.InvestorList[i].MarginLevel <= MarginCall)
                    {
                        Result.Add(Business.Market.InvestorList[i]);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorOnline(int From, int To)
        {
            List<Business.Investor> tempResult = new List<Investor>();
            List<Business.Investor> Result = new List<Investor>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;

                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].IsOnline == true)
                    {
                        tempResult.Add(Business.Market.InvestorList[i]);
                    }
                }

                if (tempResult != null)
                {
                    int countResult = tempResult.Count;
                    if (countResult < To)
                        To = countResult;
                    for (int i = From; i < To; i++)
                    {
                        Result.Add(tempResult[i]);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorOnlineByGroupID(int From, int To, int GroupID)
        {
            List<Business.Investor> Result = new List<Investor>();
            List<Business.Investor> tempResult = new List<Investor>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == GroupID &&
                        Business.Market.InvestorList[i].IsOnline == true)
                    {
                        #region MAP DATA
                        Business.Investor newInvestor = new Investor();
                        newInvestor.Address = Business.Market.InvestorList[i].Address;
                        newInvestor.AgentID = Business.Market.InvestorList[i].AgentID;
                        newInvestor.AllowChangePwd = Business.Market.InvestorList[i].AllowChangePwd;
                        newInvestor.Balance = Business.Market.InvestorList[i].Balance;
                        newInvestor.City = Business.Market.InvestorList[i].City;
                        newInvestor.Code = Business.Market.InvestorList[i].Code;
                        newInvestor.Comment = Business.Market.InvestorList[i].Comment;
                        newInvestor.InvestorComment = Business.Market.InvestorList[i].InvestorComment;
                        newInvestor.Country = Business.Market.InvestorList[i].Country;
                        newInvestor.Credit = Business.Market.InvestorList[i].Credit;
                        newInvestor.Email = Business.Market.InvestorList[i].Email;
                        newInvestor.Equity = Business.Market.InvestorList[i].Equity;
                        newInvestor.FirstName = Business.Market.InvestorList[i].FirstName;
                        newInvestor.FreeMargin = Business.Market.InvestorList[i].FreeMargin;
                        newInvestor.InvestorCommand = Business.Market.InvestorList[i].InvestorCommand;
                        newInvestor.InvestorID = Business.Market.InvestorList[i].InvestorID;
                        newInvestor.InvestorProfileID = Business.Market.InvestorList[i].InvestorProfileID;
                        newInvestor.InvestorStatusID = Business.Market.InvestorList[i].InvestorStatusID;
                        newInvestor.IsActive = Business.Market.InvestorList[i].IsActive;
                        newInvestor.IsCalculating = Business.Market.InvestorList[i].IsCalculating;
                        newInvestor.IsDisable = Business.Market.InvestorList[i].IsDisable;
                        newInvestor.IsOnline = Business.Market.InvestorList[i].IsOnline;
                        newInvestor.Leverage = Business.Market.InvestorList[i].Leverage;
                        newInvestor.LeverageInvestorGroup = Business.Market.InvestorList[i].LeverageInvestorGroup;
                        newInvestor.Margin = Business.Market.InvestorList[i].Margin;
                        newInvestor.MarginLevel = Business.Market.InvestorList[i].MarginLevel;
                        newInvestor.NickName = Business.Market.InvestorList[i].NickName;
                        newInvestor.Phone = Business.Market.InvestorList[i].Phone;
                        newInvestor.Profit = Business.Market.InvestorList[i].Profit;
                        newInvestor.ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                        newInvestor.RegisterDay = Business.Market.InvestorList[i].RegisterDay;
                        newInvestor.SecondAddress = Business.Market.InvestorList[i].SecondAddress;
                        newInvestor.SendReport = Business.Market.InvestorList[i].SendReport;
                        newInvestor.State = Business.Market.InvestorList[i].State;
                        newInvestor.StatusCode = Business.Market.InvestorList[i].StatusCode;
                        newInvestor.TaxRate = Business.Market.InvestorList[i].TaxRate;
                        newInvestor.ZipCode = Business.Market.InvestorList[i].ZipCode;
                        #endregion

                        tempResult.Add(newInvestor);
                    }
                }
            }

            if (tempResult != null)
            {
                int count = tempResult.Count;
                if (count < To)
                    To = count;

                for (int i = From; i < To; i++)
                {
                    Result.Add(tempResult[i]);
                }
            }

            return Result;
        }

        /// <summary>
        /// GET INVESTOR WITH COMMAND IN RAM(GET FROM TO)
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorWithCommand(int From, int To)
        {
            List<Business.Investor> tempResult = new List<Investor>();
            List<Business.Investor> Result = new List<Investor>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].CommandList.Count > 0)
                    {
                        tempResult.Add(Business.Market.InvestorList[i]);
                    }
                }

                if (tempResult != null)
                {
                    int countResult = tempResult.Count;
                    for (int i = From; i < To; i++)
                    {
                        Result.Add(tempResult[i]);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// GET INVESTOR BY INVESTOR GROUP ID
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByInvestorGroup(int InvestorGroupID, int From, int To)
        {
            List<Business.Investor> Result = new List<Investor>();
            List<Business.Investor> tempResult = new List<Investor>();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == InvestorGroupID)
                    {
                        #region MAP DATA
                        Business.Investor newInvestor = new Investor();
                        newInvestor.Address = Business.Market.InvestorList[i].Address;
                        newInvestor.AgentID = Business.Market.InvestorList[i].AgentID;
                        newInvestor.AllowChangePwd = Business.Market.InvestorList[i].AllowChangePwd;
                        newInvestor.Balance = Business.Market.InvestorList[i].Balance;
                        newInvestor.City = Business.Market.InvestorList[i].City;
                        newInvestor.Code = Business.Market.InvestorList[i].Code;
                        newInvestor.Comment = Business.Market.InvestorList[i].Comment;
                        newInvestor.InvestorComment = Business.Market.InvestorList[i].InvestorComment;
                        newInvestor.Country = Business.Market.InvestorList[i].Country;
                        newInvestor.Credit = Business.Market.InvestorList[i].Credit;
                        newInvestor.Email = Business.Market.InvestorList[i].Email;
                        newInvestor.Equity = Business.Market.InvestorList[i].Equity;
                        newInvestor.FirstName = Business.Market.InvestorList[i].FirstName;
                        newInvestor.FreeMargin = Business.Market.InvestorList[i].FreeMargin;
                        newInvestor.InvestorCommand = Business.Market.InvestorList[i].InvestorCommand;
                        newInvestor.InvestorID = Business.Market.InvestorList[i].InvestorID;
                        newInvestor.InvestorProfileID = Business.Market.InvestorList[i].InvestorProfileID;
                        newInvestor.InvestorStatusID = Business.Market.InvestorList[i].InvestorStatusID;
                        newInvestor.IsActive = Business.Market.InvestorList[i].IsActive;
                        newInvestor.IsCalculating = Business.Market.InvestorList[i].IsCalculating;
                        newInvestor.IsDisable = Business.Market.InvestorList[i].IsDisable;
                        newInvestor.IsOnline = Business.Market.InvestorList[i].IsOnline;
                        newInvestor.Leverage = Business.Market.InvestorList[i].Leverage;
                        newInvestor.LeverageInvestorGroup = Business.Market.InvestorList[i].LeverageInvestorGroup;
                        newInvestor.Margin = Business.Market.InvestorList[i].Margin;
                        newInvestor.MarginLevel = Business.Market.InvestorList[i].MarginLevel;
                        newInvestor.NickName = Business.Market.InvestorList[i].NickName;
                        newInvestor.Phone = Business.Market.InvestorList[i].Phone;
                        newInvestor.Profit = Business.Market.InvestorList[i].Profit;
                        newInvestor.ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                        newInvestor.RegisterDay = Business.Market.InvestorList[i].RegisterDay;
                        newInvestor.SecondAddress = Business.Market.InvestorList[i].SecondAddress;
                        newInvestor.SendReport = Business.Market.InvestorList[i].SendReport;
                        newInvestor.State = Business.Market.InvestorList[i].State;
                        newInvestor.StatusCode = Business.Market.InvestorList[i].StatusCode;
                        newInvestor.TaxRate = Business.Market.InvestorList[i].TaxRate;
                        newInvestor.ZipCode = Business.Market.InvestorList[i].ZipCode;
                        #endregion

                        tempResult.Add(newInvestor);
                    }
                }
            }

            if (tempResult != null)
            {
                int count = tempResult.Count;
                if (count < To)
                    To = count;

                for (int i = From; i < To; i++)
                {
                    Result.Add(tempResult[i]);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByGroupFromTo(int groupID, int from, int to)
        {
            List<Business.Investor> result = new List<Investor>();
            List<Business.Investor> tempResult = new List<Investor>();

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == groupID)
                    {
                        tempResult.Add(Business.Market.InvestorList[i]);
                    }
                }
            }

            int countResult = tempResult.Count;
            if (countResult < to)
                to = countResult;

            for (int i = from; i < to; i++)
            {
                result.Add(tempResult[i]);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="GroupListID"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByGroupList(int From, int To, List<int> GroupListID)
        {
            List<Business.Investor> Result = new List<Investor>();
            List<Business.Investor> tempResult = new List<Investor>();

            if (GroupListID != null)
            {
                int countGroupList = GroupListID.Count;
                for (int n = 0; n < countGroupList; n++)
                {
                    if (Business.Market.InvestorList != null)
                    {
                        int count = Business.Market.InvestorList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID == GroupListID[n])
                            {
                                #region MAP DATA
                                Business.Investor newInvestor = new Investor();
                                newInvestor.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;
                                newInvestor.Address = Business.Market.InvestorList[i].Address;
                                newInvestor.AgentID = Business.Market.InvestorList[i].AgentID;
                                newInvestor.AllowChangePwd = Business.Market.InvestorList[i].AllowChangePwd;
                                newInvestor.Balance = Business.Market.InvestorList[i].Balance;
                                newInvestor.City = Business.Market.InvestorList[i].City;
                                newInvestor.Code = Business.Market.InvestorList[i].Code;
                                newInvestor.Comment = Business.Market.InvestorList[i].Comment;
                                newInvestor.InvestorComment = Business.Market.InvestorList[i].InvestorComment;
                                newInvestor.Country = Business.Market.InvestorList[i].Country;
                                newInvestor.Credit = Business.Market.InvestorList[i].Credit;
                                newInvestor.Email = Business.Market.InvestorList[i].Email;
                                newInvestor.Equity = Business.Market.InvestorList[i].Equity;
                                newInvestor.FirstName = Business.Market.InvestorList[i].FirstName;
                                newInvestor.FreeMargin = Business.Market.InvestorList[i].FreeMargin;
                                newInvestor.InvestorCommand = Business.Market.InvestorList[i].InvestorCommand;
                                newInvestor.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                newInvestor.InvestorProfileID = Business.Market.InvestorList[i].InvestorProfileID;
                                newInvestor.InvestorStatusID = Business.Market.InvestorList[i].InvestorStatusID;
                                newInvestor.IsActive = Business.Market.InvestorList[i].IsActive;
                                newInvestor.IsCalculating = Business.Market.InvestorList[i].IsCalculating;
                                newInvestor.IsDisable = Business.Market.InvestorList[i].IsDisable;
                                newInvestor.IsOnline = Business.Market.InvestorList[i].IsOnline;
                                newInvestor.Leverage = Business.Market.InvestorList[i].Leverage;
                                newInvestor.LeverageInvestorGroup = Business.Market.InvestorList[i].LeverageInvestorGroup;
                                newInvestor.Margin = Business.Market.InvestorList[i].Margin;
                                newInvestor.MarginLevel = Business.Market.InvestorList[i].MarginLevel;
                                newInvestor.NickName = Business.Market.InvestorList[i].NickName;
                                newInvestor.Phone = Business.Market.InvestorList[i].Phone;
                                newInvestor.Profit = Business.Market.InvestorList[i].Profit;
                                newInvestor.ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                                newInvestor.RegisterDay = Business.Market.InvestorList[i].RegisterDay;
                                newInvestor.SecondAddress = Business.Market.InvestorList[i].SecondAddress;
                                newInvestor.SendReport = Business.Market.InvestorList[i].SendReport;
                                newInvestor.State = Business.Market.InvestorList[i].State;
                                newInvestor.StatusCode = Business.Market.InvestorList[i].StatusCode;
                                newInvestor.TaxRate = Business.Market.InvestorList[i].TaxRate;
                                newInvestor.ZipCode = Business.Market.InvestorList[i].ZipCode;
                                newInvestor.IDPassport = Business.Market.InvestorList[i].IDPassport;
                                newInvestor.PhonePwd = Business.Market.InvestorList[i].PhonePwd;
                                newInvestor.FreezeMargin = Business.Market.InvestorList[i].FreezeMargin;
                                #endregion

                                tempResult.Add(newInvestor);
                            }
                        }
                    }
                }
            }

            if (tempResult != null)
            {
                int count = tempResult.Count;
                if (count < To)
                    To = count;

                for (int i = From; i < To; i++)
                {
                    Result.Add(tempResult[i]);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByListCode(List<string> listCode)
        {
            List<Business.Investor> result = new List<Investor>();
            if (listCode != null)
            {
                int count = listCode.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList != null)
                    {   
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].Code.Trim().ToUpper() == listCode[i].Trim().ToUpper())
                            {
                                result.Add(Business.Market.InvestorList[j]);

                                break;
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
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal Business.Investor Login(string Code, string Pwd, int InvestorIndex, string IpAddress)
        {
            Business.Investor Result = new Investor();
            Result.ListIGroupSecurity = new List<IGroupSecurity>();
            Result.ListIGroupSymbol = new List<IGroupSymbol>();
            Result.ListSymbol = new List<Symbol>();
            bool flag = false;
            int index = 0;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code.Trim() == Code.Trim())
                    {
                        #region LOGIN WITH PRIMARY PASSWORD
                        if (Business.Market.InvestorList[i].PrimaryPwd.Trim() == Pwd.Trim())
                        {
                            if (Business.Market.InvestorList[i].IsOnline)
                            {
                                #region IF INVESTOR ONLINE IS TRUE
                                TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;
                                if (span.TotalSeconds > 10)
                                {
                                    Business.Market.InvestorList[i].IpAddress = IpAddress;
                                    Business.Market.InvestorList[i].IsOnline = true;
                                    Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                    Business.Market.InvestorList[i].ReadOnly = Result.ReadOnly;
                                    Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                    Business.Market.InvestorList[i].IsLogout = false;
                                    Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                                    Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                    index = i;
                                }
                                else
                                {
                                    //Business.Market.InvestorList[i].ClientCommandQueue.Add("LOFF14790251");
                                    //TimeSpan tempSpan = new TimeSpan();      

                                    //while (tempSpan.TotalSeconds < 10)
                                    //{
                                    //    if(Business.Market.InvestorList[i].IsLogout)
                                    //        break;

                                    //    tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                    //    System.Threading.Thread.Sleep(200);
                                    //}

                                    Result.InvestorStatusCode = "LIA002";
                                    return Result;
                                }
                                #endregion
                            }
                            else
                            {
                                #region IF INVESTOR IS ONLINE  = FALSE
                                Business.Market.InvestorList[i].IpAddress = IpAddress;
                                Business.Market.InvestorList[i].IsOnline = true;
                                Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                Business.Market.InvestorList[i].ReadOnly = Result.ReadOnly;
                                Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                Business.Market.InvestorList[i].IsLogout = false;
                                Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                                Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                index = i;
                                #endregion
                            }

                            Result = Business.Market.InvestorList[i];
                            flag = true;

                            Result.LoginKey = Model.ValidateCheck.RandomKeyLogin(8);

                            //ADD INVESTOR TO INVESTOR ONLINE LIST
                            this.AddInvestorToInvestorOnline(Result);


                            break;
                        }
                        #endregion

                        #region LOGIN WITH READ ONLY PASSWORD
                        if (!flag)
                        {
                            if (Business.Market.InvestorList[i].ReadOnlyPwd.Trim() == Pwd.Trim())
                            {
                                if (Business.Market.InvestorList[i].IsOnline)
                                {
                                    #region IF INVESTOR ONLINE IS TRUE
                                    TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;
                                    if (span.TotalSeconds > 10)
                                    {
                                        Business.Market.InvestorList[i].IpAddress = IpAddress;
                                        Business.Market.InvestorList[i].IsOnline = true;
                                        Business.Market.InvestorList[i].IsReadOnly = true;
                                        Business.Market.InvestorList[i].ReadOnly = Result.ReadOnly;
                                        Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                        Business.Market.InvestorList[i].IsLogout = false;
                                        Business.Market.InvestorList[i].LoginType = TypeLogin.ReadOnly;
                                        Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                        index = i;
                                    }
                                    else
                                    {
                                        Result.InvestorStatusCode = "LIA002";
                                        return Result;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region IF INVESTOR IS ONLINE  = FALSE
                                    Business.Market.InvestorList[i].IpAddress = IpAddress;
                                    Business.Market.InvestorList[i].IsOnline = true;
                                    Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                    Business.Market.InvestorList[i].ReadOnly = Result.ReadOnly;
                                    Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                    Business.Market.InvestorList[i].IsLogout = false;
                                    Business.Market.InvestorList[i].LoginType = TypeLogin.ReadOnly;
                                    Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                    index = i;
                                    #endregion
                                }

                                Result = Business.Market.InvestorList[i];
                                //Result.IsReadOnly = true;
                                Result.IsOnline = true;
                                Result.IsLogout = false;
                                Result.LoginType = TypeLogin.ReadOnly;
                                Result.LoginKey = Model.ValidateCheck.RandomKeyLogin(8);
                                //ADD INVESTOR TO INVESTOR ONLINE
                                this.AddInvestorToInvestorOnline(Result);

                                break;
                            }
                        }
                        #endregion
                    }
                }
            }

            if (Result != null && Result.InvestorID > 0)
            {
                //GET SETTING OF INVESTOR
                Result.ListIGroupSecurity = this.GetIGroupSecurity(Business.Market.InvestorList[index].InvestorGroupInstance.InvestorGroupID);
                Result.ListIGroupSymbol = this.GetIGroupSymbol(Business.Market.InvestorList[index].InvestorGroupInstance.InvestorGroupID);
                Result.ListSymbol = this.GetSymbolOfInvestor(Result.ListIGroupSecurity);
                Result.ClientConfigInstance = this.GetClientConfig(Business.Market.InvestorList[index].InvestorGroupInstance.InvestorGroupID);
                Result.InvestorIndex = index;

                //Notify to Manager
                TradingServer.Facade.FacadeSendNotifyManagerRequest(1, Result);

                #region GET ALERT OF INVESTOR
                if (Result.AlertQueue == null)
                {
                    Result.AlertQueue = new List<PriceAlert>();
                    for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                    {
                        if (Business.Market.SymbolList[i].AlertQueue != null)
                        {
                            for (int j = 0; j < Business.Market.SymbolList[i].AlertQueue.Count; j++)
                            {
                                if (Result.InvestorID == Business.Market.SymbolList[i].AlertQueue[j].InvestorID)
                                {
                                    Result.AlertQueue.Add(Business.Market.SymbolList[i].AlertQueue[j]);
                                }
                            }
                        }
                        else
                        {
                            Business.Market.SymbolList[i].AlertQueue = new List<PriceAlert>();
                        }
                    }
                }
                #endregion
            }

            return Result;
        }

        /// <summary>
        /// CLIENT CONNECT WITH NO CONNECT MT4
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal Business.Investor NewLogin(string Code, string Pwd, int InvestorIndex, string IpAddress, Socket InsSocket)
        {
            Business.Investor Result = new Investor();
            Result.ListIGroupSecurity = new List<IGroupSecurity>();
            Result.ListIGroupSymbol = new List<IGroupSymbol>();
            Result.ListSymbol = new List<Symbol>();
            bool flag = false;
            int index = 0;
            int timeOut = 0;
            int investorGroup = -1;
            int timeLogin = 0;
            bool isSend = false;

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code.Trim() == Code.Trim())
                    {
                        #region LOGIN WITH PRIMARY PASSWORD
                        if (Business.Market.InvestorList[i].PrimaryPwd.Trim() == Pwd.Trim())
                        {
                            while (Business.Market.InvestorList[i].InLogin)
                            {
                                if (timeOut < 60)
                                    timeOut++;
                                else
                                    Business.Market.InvestorList[i].InLogin = false;

                                System.Threading.Thread.Sleep(500);
                            }

                            Business.Market.InvestorList[i].InLogin = true;

                            if (Business.Market.InvestorList[i].IsOnline)
                            {
                                #region IF INVESTOR ONLINE IS TRUE
                                TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;
                                if (span.TotalSeconds > 10)
                                {
                                    #region LOGOUT ACCOUNT PRIMARY  
                                    bool isPrimary = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);
                                    if (isPrimary)
                                    {
                                        //Business.Market.InvestorList[i].ClientCommandQueue.Add("OLOFF14790251");
                                        TimeSpan tempSpan = new TimeSpan();
                                        while (!Business.Market.InvestorList[i].IsLogout)
                                        {
                                            tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                            if (tempSpan.TotalSeconds > 20)
                                                Business.Market.InvestorList[i].IsLogout = true;

                                            //if (!isSend)
                                            //{
                                            //    if (timeLogin < 200)
                                            //        timeLogin++;
                                            //    else
                                            //    {
                                            //        //Business.Market.InvestorList[i].ClientCommandQueue.Add("OLOFF14790251");
                                            //        this.SendCommandToInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary, "OLOFF14790251");
                                            //        isSend = true;
                                            //        timeLogin = 0;
                                            //    }
                                            //}

                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region LOGOUT ACCOUNT PRIMARY
                                    bool isPrimary = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);
                                    if (isPrimary)
                                    {
                                        //Business.Market.InvestorList[i].ClientCommandQueue.Add("OLOFF14790251");
                                        TimeSpan tempSpan = new TimeSpan();

                                        while (!Business.Market.InvestorList[i].IsLogout)
                                        {
                                            tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                            if (tempSpan.TotalSeconds > 20)
                                                Business.Market.InvestorList[i].IsLogout = true;

                                            //if (!isSend)
                                            //{
                                            //    if (timeLogin < 200)
                                            //        timeLogin++;
                                            //    else
                                            //    {
                                            //        //Business.Market.InvestorList[i].ClientCommandQueue.Add("OLOFF14790251");
                                            //        this.SendCommandToInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary, "OLOFF14790251");
                                            //        isSend = true;
                                            //        timeLogin = 0;
                                            //    }
                                            //}

                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region LOGOUT ACCOUNT PRIMARY
                                bool isOnline = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);

                                if (isOnline)
                                {
                                    //Business.Market.InvestorList[i].ClientCommandQueue.Add("OLOFF14790251");
                                    TimeSpan tempSpan = new TimeSpan();

                                    while (!Business.Market.InvestorList[i].IsLogout)
                                    {
                                        tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                        if (tempSpan.TotalSeconds > 60)
                                            Business.Market.InvestorList[i].IsLogout = true;

                                        System.Threading.Thread.Sleep(100);
                                    }
                                }
                                #endregion

                            }

                            //CLEAR COMMAND QUEUE
                            Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                            Business.Market.InvestorList[i].BackupQueue = new List<Business.BackupQueue>();

                            Business.Market.InvestorList[i].IpAddress = IpAddress;
                            Business.Market.InvestorList[i].IsOnline = true;
                            Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                            //Business.Market.InvestorList[i].ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                            Business.Market.InvestorList[i].IsLogout = false;
                            Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                            Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                            index = i;
                            Result = Business.Market.InvestorList[i];

                            Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;

                            flag = true;
                            string key = Model.ValidateCheck.RandomKeyLogin(8);
                            Result.LoginKey = key;
                            Business.Market.InvestorList[i].LoginKey = key;

                            investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;

                            ValidIPAddress.Instance.AddIpBlock(IpAddress, Business.Market.InvestorList[i].InvestorID);

                            GetSymbolOfInvestor(Result, index, investorGroup);

                            Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                            Result.LastConnectTime = DateTime.Now;

                            if (!Business.Market.InvestorList[i].IsDisable && Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable)
                            {
                                //ADD INVESTOR TO INVESTOR ONLINE LIST
                                this.AddInvestorToInvestorOnline(Result);
                            }                            

                            Business.Market.InvestorList[i].InLogin = false;

                            break;
                        }
                        #endregion

                        #region LOGIN WITH READ ONLY PASSWORD
                        if (!flag)
                        {
                            if (Business.Market.InvestorList[i].ReadOnlyPwd.Trim() == Pwd.Trim())
                            {
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                                Business.Market.InvestorList[i].BackupQueue = new List<Business.BackupQueue>();

                                Result.Code = Business.Market.InvestorList[i].Code;
                                Result.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                Result.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;
                                Result.LoginType = TypeLogin.ReadOnly;

                                #region ADD NEW INFO ACCOUNT
                                Result.Address = Business.Market.InvestorList[i].Address;
                                Result.Balance = Business.Market.InvestorList[i].Balance;
                                Result.City = Business.Market.InvestorList[i].City;
                                Result.Country = Business.Market.InvestorList[i].Country;
                                Result.Email = Business.Market.InvestorList[i].Email;
                                Result.Equity = Business.Market.InvestorList[i].Equity;
                                Result.FreeMargin = Business.Market.InvestorList[i].FreeMargin;
                                Result.NickName = Business.Market.InvestorList[i].NickName;
                                Result.Leverage = Business.Market.InvestorList[i].Leverage;
                                Result.Margin = Business.Market.InvestorList[i].Margin;
                                Result.MarginLevel = Business.Market.InvestorList[i].MarginLevel;
                                Result.Phone = Business.Market.InvestorList[i].Phone;
                                Result.Profit = Business.Market.InvestorList[i].Profit;
                                Result.State = Business.Market.InvestorList[i].State;
                                Result.ZipCode = Business.Market.InvestorList[i].ZipCode;
                                Result.Credit = Business.Market.InvestorList[i].Credit;
                                Result.IsDisable = Business.Market.InvestorList[i].IsDisable;
                                Result.IsReadOnly = true;
                                Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                                Result.UserConfig = Business.Market.InvestorList[i].UserConfig;
                                Result.UserConfigIpad = Business.Market.InvestorList[i].UserConfigIpad;
                                Result.UserConfigIphone = Business.Market.InvestorList[i].UserConfigIphone;
                                Result.FreezeMargin = Business.Market.InvestorList[i].FreezeMargin;
                                #endregion

                                Result.LoginKey = Model.ValidateCheck.RandomKeyLogin(8);

                                //ADD INVESTOR TO INVESTOR ONLINE
                                this.AddInvestorToInvestorOnline(Result);
                                
                                ValidIPAddress.Instance.AddIpBlock(IpAddress, Business.Market.InvestorList[i].InvestorID);
                                investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;
                                Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;

                                GetSymbolOfInvestor(Result, index, investorGroup);

                                break;
                            }
                        }
                        #endregion
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// CLIENT LOGIN WITH CONNECT TO MT4
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        internal Business.Investor NewLogin(string code, string Pwd, string ipAddress, Socket InsSocket)
        {
            Business.Investor Result = new Investor();
            Result.ListIGroupSecurity = new List<IGroupSecurity>();
            Result.ListIGroupSymbol = new List<IGroupSymbol>();
            Result.ListSymbol = new List<Symbol>();
            bool flag = false;
            int index = 0;
            int timeOut = 0;
            int investorGroup = -1;
            int oldInvestorID = 0;
            int newInvestorID = 0;

            string hashPwd = Model.ValidateCheck.Encrypt(Pwd);

            //IF STATUS CONNECT TO MT4 IS FALSE THEN NOTIFY ERROR TO CLIENT "MT4 DOWN"
            if (Business.Market.StatusConnect == false)
            {   
                Result.NotifyError = "MT4 down";
                return Result;
            }

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code.Trim() == code.Trim())
                    {
                        if (Business.Market.InvestorList[i].IsFirstLogin)
                            continue;

                        #region LOGIN MT4
                        bool isLoginSuccess = false;
                        bool isReadOnlyPassword = false;
                        string strError = string.Empty;

                        string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertClientLoginToString(code, Pwd);

                        //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Command Send] - " + cmd);

                        string result = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                        //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Receive Command] - " + code + " - " + result);

                        if (!string.IsNullOrEmpty(result))
                        {
                            string[] subValue = result.Split('$');
                            if (subValue.Length == 2)
                            {
                                string[] subParameter = subValue[1].Split('{');
                                if (subParameter.Length > 0)
                                {
                                    if (int.Parse(subParameter[0]) == 1)
                                        isLoginSuccess = true;

                                    if (isLoginSuccess)
                                    {
                                        if (int.Parse(subParameter[1]) == 1)
                                            isReadOnlyPassword = true;
                                    }
                                    else
                                    {
                                        if (int.Parse(subParameter[1]) == 1)
                                            isReadOnlyPassword = true;

                                        strError = subParameter[2];
                                    }
                                }
                            }
                        }
                        #endregion

                        if (isLoginSuccess)
                        {
                            #region LOGIN WITH PRIMARY PASSWORD
                            if (Business.Market.InvestorList[i].PrimaryPwd.Trim() == hashPwd.Trim())
                            {
                                #region CASE LOGIN PRIMARY PASSWORD FROM MT4 CORRECT

                                #region PROCESS WAITING LOGIN COMPLETE
                                while (Business.Market.InvestorList[i].InLogin)
                                {
                                    if (timeOut < 60)
                                        timeOut++;
                                    else
                                        Business.Market.InvestorList[i].InLogin = false;

                                    System.Threading.Thread.Sleep(500);
                                }
                                #endregion

                                //CALL CHECK PASSWORD FROM MT4

                                Business.Market.InvestorList[i].InLogin = true;

                                #region LOGOUT CLIENT
                                if (Business.Market.InvestorList[i].IsOnline)
                                {
                                    #region IF INVESTOR ONLINE IS TRUE THEN LOGOUT ACCOUNT PRIMARY
                                    TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                    bool isPrimary = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);
                                    if (isPrimary)
                                    {
                                        TimeSpan tempSpan = new TimeSpan();
                                        while (!Business.Market.InvestorList[i].IsLogout)
                                        {
                                            tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                            if (tempSpan.TotalSeconds > 30)
                                            {
                                                Business.Market.InvestorList[i].IsLogout = true;
                                            }

                                            System.Threading.Thread.Sleep(50);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                //CLEAR COMMAND QUEUE
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                Business.Market.InvestorList[i].IpAddress = IpAddress;
                                Business.Market.InvestorList[i].IsOnline = true;
                                Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                Business.Market.InvestorList[i].IsLogout = false;
                                Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                                Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                                index = i;
                                Result = Business.Market.InvestorList[i];

                                Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;

                                flag = true;
                                //string key = Model.ValidateCheck.RandomKeyLogin(8);
                                string key = DateTime.Now.Ticks.ToString();
                                Result.LoginKey = key;
                                Business.Market.InvestorList[i].LoginKey = key;

                                investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;

                                //ADD IPADDRESS TO BLOCK IP
                                //ValidIPAddress.Instance.AddIpBlock(ipAddress, Business.Market.InvestorList[i].InvestorID);

                                //GET INFO ACCOUNT(IGROUPSECURITY, IGROUPSYMBOL, LISTSYMBOL CLIENT)
                                GetSymbolOfInvestor(Result, index, investorGroup);

                                Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                Result.LastConnectTime = DateTime.Now;

                                if (!Business.Market.InvestorList[i].IsDisable && Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable)
                                {
                                    //ADD INVESTOR TO INVESTOR ONLINE LIST
                                    this.AddInvestorToInvestorOnline(Result);
                                }

                                Business.Market.InvestorList[i].InLogin = false;

                                #region CALL FUNCTION CONNECT NJ4X CONNECT
                                //CONNECT TO NJ4X
                                string nj4xCmd = NJ4XConnectSocket.MapNJ4X.Instance.MapConnect(code, Pwd);
                                //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Send NJ4X] - " + nj4xCmd);
                                string nj4xResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xCmd);
                                //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Receive NJ4X] - " + nj4xResult);
                                #endregion

                                #region CALL FUNCTION CONNECT NJ4X USING MQL
                                //MQLConnector.Facade.LoginMt4(int.Parse(code), Pwd);
                                //while (true)
                                //{
                                //    bool check = MQLConnector.Facade.CheckClientActive(int.Parse(code));
                                //    if (check)
                                //        break;

                                //    System.Threading.Thread.Sleep(100);
                                //}
                                #endregion

                                break;
                                #endregion
                            }
                            else
                            {
                                #region CASE IF LOGIN PASSWORD FROM MT4 CORRECT THEN UPDATE PASSWORD ET5
                                //hash password and update password to ram
                                Business.Market.InvestorList[i].PrimaryPwd = hashPwd;
                                //Business.Market.InvestorList[i].ReadOnlyPwd = hashPwd;

                                TradingServer.Business.Investor.DBWInvestorInstance.UpdatePrimaryPasword(Business.Market.InvestorList[i].InvestorID, hashPwd);

                                #region PROCESS WAITING LOGIN
                                while (Business.Market.InvestorList[i].InLogin)
                                {
                                    if (timeOut < 60)
                                        timeOut++;
                                    else
                                        Business.Market.InvestorList[i].InLogin = false;

                                    System.Threading.Thread.Sleep(500);
                                }
                                #endregion

                                //CALL CHECK PASSWORD FROM MT4

                                Business.Market.InvestorList[i].InLogin = true;

                                if (Business.Market.InvestorList[i].IsOnline)
                                {
                                    #region IF INVESTOR ONLINE IS TRUE THEN LOGOUT ACCOUNT PRIMARY
                                    TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;
                                    if (span.TotalSeconds > 10)
                                    {
                                        bool isPrimary = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);
                                        if (isPrimary)
                                        {
                                            TimeSpan tempSpan = new TimeSpan();
                                            while (!Business.Market.InvestorList[i].IsLogout)
                                            {
                                                tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                                if (tempSpan.TotalSeconds > 20)
                                                    Business.Market.InvestorList[i].IsLogout = true;

                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                //CLEAR COMMAND QUEUE
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                Business.Market.InvestorList[i].IpAddress = IpAddress;
                                Business.Market.InvestorList[i].IsOnline = true;
                                Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                Business.Market.InvestorList[i].IsLogout = false;
                                Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                                Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                                index = i;
                                Result = Business.Market.InvestorList[i];

                                Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                                
                                flag = true;
                                string key = Model.ValidateCheck.RandomKeyLogin(8);
                                Result.LoginKey = key;
                                Business.Market.InvestorList[i].LoginKey = key;

                                investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;

                                ValidIPAddress.Instance.AddIpBlock(ipAddress, Business.Market.InvestorList[i].InvestorID);

                                GetSymbolOfInvestor(Result, index, investorGroup);

                                Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                Result.LastConnectTime = DateTime.Now;

                                
                                if (!Business.Market.InvestorList[i].IsDisable && Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable)
                                {
                                    //ADD INVESTOR TO INVESTOR ONLINE LIST
                                    this.AddInvestorToInvestorOnline(Result);    
                                }

                                Business.Market.InvestorList[i].InLogin = false;

                                break;
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            return Result;
                        }
                    }
                }
            }

            #region ACCOUNT DON'T EXITS IN ET5
            if (!flag)
            {
                //CALL LOGIN MT4
                bool isLoginSuccess = false;
                bool isReadOnlyPassword = false;
                string strError = string.Empty;

                string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertClientLoginToString(code, Pwd);
                
                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Command Send] - " + cmd);

                //string result = Business.Market.InstanceSocket.SendToMT4(Business.Market.DEFAULT_IPADDRESS, Business.Market.DEFAULT_PORT, cmd);
                string result = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                //string result = LibraryAPI.CPPOut.Command(cmd);
                //string result = Business.Market.InstanceGlobalDelegate.SendCommand(cmd);

                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Receive Command] - " + code + " - " + result);

                #region PROCESS DATA RETURN FROM MT4
                if (!string.IsNullOrEmpty(result))
                {
                    string[] subValue = result.Split('$');
                    if (subValue.Length == 2)
                    {
                        string[] subParameter = subValue[1].Split('{');
                        if (subParameter.Length > 0)
                        {
                            if (int.Parse(subParameter[0]) == 1)
                                isLoginSuccess = true;

                            if (isLoginSuccess)
                            {
                                if (int.Parse(subParameter[1]) == 1)
                                    isReadOnlyPassword = true;
                                else
                                    isReadOnlyPassword = false;
                            }
                            else
                            {
                                if (int.Parse(subParameter[1]) == 1)
                                    isReadOnlyPassword = true;
                                else
                                    isReadOnlyPassword = false;

                                strError = subParameter[2];
                            }
                        }
                    }
                }
                #endregion

                if (!isLoginSuccess)
                    return Result;

                #region FIND AND REMOVE INVESTOR IF INVESTOR EXITS
                //FIND AND REMOVE INVESTOR IF INVESTOR EXITS
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int j = 0; j < count; j++)
                    {
                        if (Business.Market.InvestorList[j].Code.ToUpper().Trim() == code.ToUpper().Trim())
                        {
                            if (Business.Market.InvestorList[j].CommandList != null)
                            {
                                int countCommand = Business.Market.InvestorList[j].CommandList.Count;
                                for (int n = 0; n < Business.Market.InvestorList[j].CommandList.Count; n++)
                                {
                                    #region FIND AND REMOVE COMMAND IN COMMAND EXECUTOR
                                    if (Business.Market.CommandExecutor != null)
                                    {
                                        int countExe = Business.Market.CommandExecutor.Count;
                                        for (int m = 0; m < countExe; m++)
                                        {
                                            if (Business.Market.CommandExecutor[m].ID == Business.Market.InvestorList[j].CommandList[n].ID)
                                            {
                                                Business.Market.CommandExecutor.RemoveAt(m);
                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region FIND AND REMOVE COMMAND IN SYMBOL LIST
                                    if (Business.Market.SymbolList != null)
                                    {
                                        int countSymbol = Business.Market.SymbolList.Count;
                                        bool flagSymbol = false;
                                        for (int m = 0; m < countSymbol; m++)
                                        {
                                            if (flagSymbol)
                                                break;

                                            if (Business.Market.SymbolList[m].CommandList != null)
                                            {
                                                int countCmd = Business.Market.SymbolList[m].CommandList.Count;
                                                for (int k = 0; k < countCmd; k++)
                                                {
                                                    if (Business.Market.SymbolList[m].CommandList[k].ID == Business.Market.InvestorList[j].CommandList[n].ID)
                                                    {
                                                        Business.Market.SymbolList[m].CommandList.RemoveAt(k);
                                                        flagSymbol = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    TradingServer.Facade.FacadeSendNoticeManagerRequest(2, Business.Market.InvestorList[j].CommandList[n]);

                                    //REMOTE COMMAND IN INVESTOR COMMAND LIST
                                    Business.Market.InvestorList[j].CommandList.RemoveAt(n);

                                    n--;

                                    //COMMENT BECAUSE USING SERVER MT4(DON'T NEED ADD COMMAND TO DATABASE(08/01/2014))
                                    //TradingServer.Facade.FacadeDeleteOpenTradeByID(Business.Market.InvestorList[j].CommandList[n].ID);
                                }
                            }

                            #region COMMENT BECAUSE USING SERVER MT4(DON'T NEED ADD COMMAND TO DATABASE(08/01/2014))
                            //TradingServer.Facade.FacadeDeleteCommandHistoryByInvestorID(Business.Market.InvestorList[j].InvestorID);

                            //bool isSuccess = Business.Market.InvestorList[j].DeleteInvestorProfileByInvestorID(Business.Market.InvestorList[j].InvestorID);

                            //int investorDel = -1;

                            //if (isSuccess)
                            //    investorDel = Business.Market.InvestorList[j].DeleteInvestor(Business.Market.InvestorList[j].InvestorID);

                            //if (investorDel > 0)
                            //    Business.Market.InvestorList.RemoveAt(j);
                            //else
                            //    TradingServer.Model.TradingCalculate.Instance.StreamFile("[Command Error] - Can't delete order");
                            #endregion

                            break;
                        }
                    }
                }
                #endregion
               
                #region PRORESS GET ACCOUNT INFO FROM MT4
                //CALL GET ACCOUNT FROM MT4
                string cmdGetAccount = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetAccountInfoToString(code);

                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Command Send] - " + cmdGetAccount);

                //string resultGetAccount = Business.Market.InstanceSocket.SendToMT4(Business.Market.DEFAULT_IPADDRESS, Business.Market.DEFAULT_PORT, cmdGetAccount);
                string resultGetAccount = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmdGetAccount);

                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Receive Command] - " + code + " - " + resultGetAccount);

                BuildCommandElement5ConnectMT4.Business.InvestorAccount newInvestor = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertInvestorAccountToString(resultGetAccount);
                Business.InvestorGroup newGroup = new InvestorGroup();
                if (newInvestor != null)
                {
                    #region GET GROUP BY GROUP NAME
                    if (Business.Market.InvestorGroupList != null)
                    {
                        int count = Business.Market.InvestorGroupList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorGroupList[i].Name.ToUpper().Trim() == newInvestor.GroupName.ToUpper().Trim())
                            {
                                newGroup = Business.Market.InvestorGroupList[i];
                                break;
                            }
                        }
                    }
                    #endregion

                    if (newGroup == null || newGroup.InvestorGroupID < 0)
                        return Result;

                    if (newGroup != null && newGroup.InvestorGroupID >= 0)
                    {
                        investorGroup = newGroup.InvestorGroupID;
                        #region BUILD COMMAND INVESTOR
                        Business.Investor objInvestor = new Investor();
                        objInvestor.UnZipPwd = Pwd;
                        objInvestor.InvestorGroupInstance = newGroup;
                        objInvestor.InvestorGroupInstance.InvestorGroupID = -1;
                        objInvestor.IpAddress = ipAddress;
                        objInvestor.InvestorStatusID = 6;
                        objInvestor.Address = newInvestor.Address;
                        objInvestor.AgentID = "";
                        objInvestor.Balance = newInvestor.Balance;
                        objInvestor.City = newInvestor.City;
                        objInvestor.Code = code;
                        //objInvestor.Comment = newInvestor.Comment;
                        objInvestor.Country = newInvestor.Country;
                        objInvestor.Credit = newInvestor.Credit;
                        objInvestor.Email = newInvestor.Email;
                        objInvestor.FirstName = newInvestor.Name;
                        objInvestor.InvestorComment = newInvestor.Comment;
                        objInvestor.Leverage = newInvestor.Leverage;
                        objInvestor.NickName = newInvestor.Name;
                        objInvestor.Phone = newInvestor.Phone;
                        objInvestor.PrimaryPwd = hashPwd;
                        objInvestor.ReadOnlyPwd = hashPwd;
                        objInvestor.PhonePwd = hashPwd;
                        objInvestor.RegisterDay = DateTime.Now;
                        objInvestor.RefInvestorID = -1;
                        objInvestor.SecondAddress = newInvestor.Address;
                        objInvestor.SendReport = newInvestor.SendReport;
                        objInvestor.IsDisable = newInvestor.IdDisable;
                        objInvestor.AllowChangePwd = newInvestor.AllowChangePassword;
                        objInvestor.State = newInvestor.State;
                        objInvestor.TaxRate = newInvestor.TaxRate;
                        objInvestor.ZipCode = newInvestor.ZipCode;
                        objInvestor.Equity = newInvestor.Equity;
                        objInvestor.Margin = newInvestor.Margin;
                        objInvestor.MarginLevel = newInvestor.MarginLevel;
                        objInvestor.FreeMargin = newInvestor.FreeMargin;
                        objInvestor.InvestorStatusID = -1;
                        #endregion

                        int investorIDDB = TradingServer.Facade.FacadeGetInvestorIDByCode(code);
                        
                        if (investorIDDB <= 0)
                        {
                            objInvestor.InvestorID = TradingServer.Facade.FacadeAddNewInvestor(objInvestor);

                            if (objInvestor.InvestorID > 0)
                                objInvestor.InvestorProfileID = TradingServer.Facade.FacadeAddInvestorProfile(objInvestor);
                        }
                        else
                        {
                            objInvestor.InvestorID = investorIDDB;
                        }

                        objInvestor.IpAddress = IpAddress;
                        objInvestor.IsOnline = true;
                        objInvestor.IsReadOnly = Result.IsReadOnly;
                        objInvestor.IsLogout = false;
                        objInvestor.IsFirstLogin = false;
                        objInvestor.LoginType = TypeLogin.Primary;
                        objInvestor.SocketInstance = InsSocket;
                        objInvestor.InvestorGroupInstance.InvestorGroupID = investorGroup;
                        index = Business.Market.InvestorList.Count;

                        //set result return to client
                        Result = objInvestor;

                        flag = true;
                        string key = Model.ValidateCheck.RandomKeyLogin(8);
                        Result.LoginKey = key;
                        objInvestor.LoginKey = key;

                        //investorGroup = objInvestor.InvestorGroupInstance.InvestorGroupID;

                        ValidIPAddress.Instance.AddIpBlock(ipAddress, objInvestor.InvestorID);

                        GetSymbolOfInvestor(Result, index, investorGroup);

                        objInvestor.LastConnectTime = DateTime.Now;
                        Result.LastConnectTime = DateTime.Now;

                        if (!objInvestor.IsDisable && objInvestor.InvestorGroupInstance.IsEnable)
                        {
                            //ADD INVESTOR TO INVESTOR ONLINE LIST
                            this.AddInvestorToInvestorOnline(Result);
                        }

                        //SEND NOTIFY TO MANAGER. CHANGE INVESTOR ACCOUNT
                        TradingServer.Facade.FacadeSendNotifyManagerRequest(3, objInvestor);

                        //add investor to list investor
                        Business.Market.InvestorList.Add(objInvestor);
                    }
                }
                #endregion

                #region PRORESS GET ONLINE COMMAND FROM MT4
                //CALL GET ONLINE COMMAND FROM MT4
                string cmdGetOnline = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetOnlineCommandToString(code, newInvestor.GroupName);

                //TradingServer.Model.TradingCalculate.Instance.StreamFile("[Command Send] - " + cmdGetOnline);

                //string resultGetOnlineCommand = Business.Market.InstanceSocket.SendToMT4(Business.Market.DEFAULT_IPADDRESS, Business.Market.DEFAULT_PORT, cmdGetOnline);
                string resultGetOnlineCommand = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmdGetOnline);
                
                //string resultGetOnlineCommand = LibraryAPI.CPPOut.Command(cmdGetOnline);
                //string resultGetOnlineCommand = Business.Market.InstanceGlobalDelegate.SendCommand(cmdGetOnline);

                //TradingServer.Model.TradingCalculate.Instance.StreamFile("Receive Command] - " + code + " - " + resultGetOnlineCommand);

                List<BuildCommandElement5ConnectMT4.Business.OnlineTrade> listOnlineCommand = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertOnlineTradeToListString(resultGetOnlineCommand);
                if (listOnlineCommand != null && listOnlineCommand.Count > 0)
                {
                    int countCommand = listOnlineCommand.Count;
                    for (int i = 0; i < countCommand; i++)
                    {
                        Business.OpenTrade newOpenTradeExe = new OpenTrade();
                        Business.OpenTrade newOpenTradeSymbol = new OpenTrade();
                        Business.OpenTrade newOpenTradeInvestor = new OpenTrade();

                        #region SEARCH SYMBOL
                        if (Business.Market.SymbolList != null)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.SymbolList[j].Name.ToUpper().Trim() == listOnlineCommand[i].SymbolName.ToUpper().Trim())
                                {
                                    newOpenTradeExe.Symbol = Business.Market.SymbolList[j];
                                    newOpenTradeInvestor.Symbol = Business.Market.SymbolList[j];
                                    newOpenTradeSymbol.Symbol = Business.Market.SymbolList[j];
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region FILL COMMAND TYPE
                        switch (listOnlineCommand[i].CommandType)
                        {
                            case "0":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(1);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;

                                    //SET CLOSE PRICES
                                    newOpenTradeExe.ClosePrice = newOpenTradeExe.Symbol.TickValue.Bid;
                                    newOpenTradeInvestor.ClosePrice = newOpenTradeInvestor.Symbol.TickValue.Bid;
                                    newOpenTradeSymbol.ClosePrice = newOpenTradeSymbol.Symbol.TickValue.Bid;
                                }
                                break;

                            case "1":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(2);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;

                                    //SET CLOSE PRICES
                                    newOpenTradeExe.ClosePrice = newOpenTradeExe.Symbol.TickValue.Ask;
                                    newOpenTradeInvestor.ClosePrice = newOpenTradeInvestor.Symbol.TickValue.Ask;
                                    newOpenTradeSymbol.ClosePrice = newOpenTradeSymbol.Symbol.TickValue.Ask;
                                }
                                break;

                            case "2":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(7);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;

                            case "3":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(8);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;

                            case "4":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(9);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;

                            case "5":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(10);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;
                        }
                        #endregion

                        #region SEARCH INVESTOR
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].Code == listOnlineCommand[i].InvestorCode)
                                {
                                    newOpenTradeExe.Investor = Business.Market.InvestorList[j];
                                    newOpenTradeInvestor.Investor = Business.Market.InvestorList[j];
                                    newOpenTradeSymbol.Investor = Business.Market.InvestorList[j];

                                    #region GET IGROUP SECURITY
                                    if (Business.Market.IGroupSecurityList != null)
                                    {
                                        int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                        for (int n = 0; n < countIGroupSecurity; n++)
                                        {
                                            if (Business.Market.IGroupSecurityList[n].SecurityID == newOpenTradeExe.Symbol.SecurityID &&
                                                Business.Market.IGroupSecurityList[n].InvestorGroupID == newOpenTradeExe.Investor.InvestorGroupInstance.InvestorGroupID)
                                            {
                                                newOpenTradeExe.IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                newOpenTradeInvestor.IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                newOpenTradeSymbol.IGroupSecurity = Business.Market.IGroupSecurityList[n];

                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Fill IGroupSecurity
                        if (newOpenTradeExe.Investor != null)
                        {
                            if (Business.Market.IGroupSecurityList != null)
                            {
                                int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                for (int j = 0; j < countIGroupSecurity; j++)
                                {
                                    if (Business.Market.IGroupSecurityList[j].SecurityID == newOpenTradeExe.Symbol.SecurityID &&
                                        Business.Market.IGroupSecurityList[j].InvestorGroupID == newOpenTradeExe.Investor.InvestorGroupInstance.InvestorGroupID)
                                    {
                                        newOpenTradeExe.IGroupSecurity = Business.Market.IGroupSecurityList[j];
                                        newOpenTradeSymbol.IGroupSecurity = Business.Market.IGroupSecurityList[j];
                                        newOpenTradeInvestor.IGroupSecurity = Business.Market.IGroupSecurityList[j];

                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region GET SPREAD DIFFIRENCE FOR COMMAND
                        //GET SPREAD DIFFRENCE OF OPEN TRADE
                        double spreadDifference = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(newOpenTradeInvestor.Symbol.SecurityID,
                            newOpenTradeInvestor.Investor.InvestorGroupInstance.InvestorGroupID);

                        newOpenTradeExe.SpreaDifferenceInOpenTrade = spreadDifference;
                        newOpenTradeInvestor.SpreaDifferenceInOpenTrade = spreadDifference;
                        newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreadDifference;
                        #endregion

                        #region NEW INSTANCES FOR COMMAND EXECUTOR
                        newOpenTradeExe.AgentCommission = 0;
                        newOpenTradeExe.ClientCode = listOnlineCommand[i].ClientCode;
                        newOpenTradeExe.CloseTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeExe.CommandCode = listOnlineCommand[i].OrderCode;
                        newOpenTradeExe.Comment = listOnlineCommand[i].Comment;
                        newOpenTradeExe.Commission = listOnlineCommand[i].Commission;
                        newOpenTradeExe.ExpTime = listOnlineCommand[i].TimeExpire;
                        newOpenTradeExe.FreezeMargin = 0;
                        //newOpenTradeExe.ID = listOnlineCommand[i].OnlineCommandID;
                        newOpenTradeExe.IsClose = false;
                        newOpenTradeExe.OpenPrice = listOnlineCommand[i].OpenPrice;
                        newOpenTradeExe.OpenTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeExe.Profit = listOnlineCommand[i].Profit;
                        newOpenTradeExe.Size = listOnlineCommand[i].Size;
                        newOpenTradeExe.StopLoss = listOnlineCommand[i].StopLoss;
                        newOpenTradeExe.Swap = listOnlineCommand[i].Swap;
                        newOpenTradeExe.TakeProfit = listOnlineCommand[i].TakeProfit;
                        newOpenTradeExe.Taxes = 0;
                        newOpenTradeExe.TotalSwap = 0;
                        newOpenTradeExe.RefCommandID = listOnlineCommand[i].CommandID;
                        #endregion

                        #region NEW INSTANCE FOR SYMBOL LIST
                        newOpenTradeSymbol.AgentCommission = 0;
                        newOpenTradeSymbol.ClientCode = listOnlineCommand[i].ClientCode;
                        newOpenTradeSymbol.CloseTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeSymbol.CommandCode = listOnlineCommand[i].OrderCode;
                        newOpenTradeSymbol.Comment = listOnlineCommand[i].Comment;
                        newOpenTradeSymbol.Commission = listOnlineCommand[i].Commission;
                        newOpenTradeSymbol.ExpTime = listOnlineCommand[i].TimeExpire;
                        newOpenTradeSymbol.FreezeMargin = 0;
                        //newOpenTradeSymbol.ID = listOnlineCommand[i].OnlineCommandID;
                        newOpenTradeSymbol.IsClose = true;
                        newOpenTradeSymbol.OpenPrice = listOnlineCommand[i].OpenPrice;
                        newOpenTradeSymbol.OpenTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeSymbol.Profit = listOnlineCommand[i].Profit;
                        newOpenTradeSymbol.Size = listOnlineCommand[i].Size;
                        newOpenTradeSymbol.StopLoss = listOnlineCommand[i].StopLoss;
                        newOpenTradeSymbol.Swap = listOnlineCommand[i].Swap;
                        newOpenTradeSymbol.TakeProfit = listOnlineCommand[i].TakeProfit;
                        newOpenTradeSymbol.Taxes = 0;
                        newOpenTradeSymbol.TotalSwap = 0;
                        newOpenTradeSymbol.InsExe = newOpenTradeExe;
                        newOpenTradeSymbol.RefCommandID = listOnlineCommand[i].CommandID;
                        #endregion

                        #region NEW INSTANCE FOR INVESTOR LIST
                        newOpenTradeInvestor.AgentCommission = 0;
                        newOpenTradeInvestor.ClientCode = listOnlineCommand[i].ClientCode;
                        newOpenTradeInvestor.CloseTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeInvestor.CommandCode = listOnlineCommand[i].OrderCode;
                        newOpenTradeInvestor.Comment = listOnlineCommand[i].Comment;
                        newOpenTradeInvestor.Commission = listOnlineCommand[i].Commission;
                        newOpenTradeInvestor.ExpTime = listOnlineCommand[i].TimeExpire;
                        newOpenTradeInvestor.FreezeMargin = 0;
                        //newOpenTradeInvestor.ID = listOnlineCommand[i].OnlineCommandID;
                        newOpenTradeInvestor.IsClose = false;
                        newOpenTradeInvestor.OpenPrice = listOnlineCommand[i].OpenPrice;
                        newOpenTradeInvestor.OpenTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeInvestor.Profit = listOnlineCommand[i].Profit;
                        newOpenTradeInvestor.Size = listOnlineCommand[i].Size;
                        newOpenTradeInvestor.StopLoss = listOnlineCommand[i].StopLoss;
                        newOpenTradeInvestor.Swap = listOnlineCommand[i].Swap;
                        newOpenTradeInvestor.TakeProfit = listOnlineCommand[i].TakeProfit;
                        newOpenTradeInvestor.Taxes = 0;
                        newOpenTradeInvestor.TotalSwap = 0;
                        newOpenTradeInvestor.InsExe = newOpenTradeExe;
                        newOpenTradeInvestor.RefCommandID = listOnlineCommand[i].CommandID;
                        #endregion

                        #region SET DATA TO 3 INSTANCE
                        //random client code
                        string clientCode = newOpenTradeExe.Investor.Code + "_" + DateTime.Now.Ticks;

                        //set client code
                        newOpenTradeExe.ClientCode = clientCode;
                        newOpenTradeInvestor.ClientCode = clientCode;
                        newOpenTradeSymbol.ClientCode = clientCode;

                        //Add Command To Database(Comment 28/11/2013)==> New Flow don't need insert database
                        //                
                        //int commandID = TradingServer.Facade.FacadeAddNewOpenTrade(newOpenTradeExe);
                        int commandID = -1;

                        //string commandCode = string.Empty;
                        ////Call Function Update Command Code Of Command
                        //commandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(commandID.ToString());
                        //TradingServer.Facade.FacadeUpdateCommandCode(commandID, commandCode);

                        //set id and command code
                        newOpenTradeExe.ID = commandID;
                        newOpenTradeExe.CommandCode = listOnlineCommand[i].CommandCode;
                        newOpenTradeInvestor.ID = commandID;
                        newOpenTradeInvestor.CommandCode = listOnlineCommand[i].CommandCode;
                        newOpenTradeSymbol.ID = commandID;
                        newOpenTradeSymbol.CommandCode = listOnlineCommand[i].CommandCode;


                        //set command id in system et5 = command id in mt4
                        commandID = listOnlineCommand[i].CommandID;
                        newOpenTradeExe.ID = listOnlineCommand[i].CommandID;
                        //newOpenTradeExe.CommandCode = listOnlineCommand[i].CommandCode;
                        newOpenTradeInvestor.ID = listOnlineCommand[i].CommandID;
                        //newOpenTradeInvestor.CommandCode = listOnlineCommand[i].CommandCode;
                        newOpenTradeSymbol.ID = listOnlineCommand[i].CommandID;
                        //newOpenTradeSymbol.CommandCode = listOnlineCommand[i].CommandCode;

                        //calculation margin for command
                        newOpenTradeExe.CalculatorMarginCommand(newOpenTradeExe);
                        //set margin for instance symbol and investor
                        newOpenTradeInvestor.Margin = newOpenTradeExe.Margin;
                        newOpenTradeSymbol.Margin = newOpenTradeExe.Margin;
                        #endregion

                        if (commandID > 0)
                        {   
                            //======================================
                            #region ADD COMMAND TO INVESTOR LIST
                            if (Business.Market.InvestorList != null)
                            {
                                int countInvestor = Business.Market.InvestorList.Count;
                                for (int j = 0; j < countInvestor; j++)
                                {
                                    if (Business.Market.InvestorList[j].Code == listOnlineCommand[i].InvestorCode)
                                    {
                                        if (Business.Market.InvestorList[j].CommandList == null)
                                            Business.Market.InvestorList[j].CommandList = new List<Business.OpenTrade>();

                                        Business.Market.InvestorList[j].CommandList.Add(newOpenTradeInvestor);

                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region ADD COMMAND TO COMMAND EXECUTOR
                            if (Business.Market.CommandExecutor == null)
                                Business.Market.CommandExecutor = new List<Business.OpenTrade>();

                            Business.Market.CommandExecutor.Add(newOpenTradeExe);
                            #endregion

                            #region ADD COMMAND TO SYMBOL LIST
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < countSymbol; j++)
                                {
                                    if (Business.Market.SymbolList[j].Name.ToUpper().Trim() == listOnlineCommand[i].SymbolName.ToUpper().Trim())
                                    {
                                        if (Business.Market.SymbolList[j].CommandList == null)
                                            Business.Market.SymbolList[j].CommandList = new List<Business.OpenTrade>();

                                        Business.Market.SymbolList[j].CommandList.Add(newOpenTradeSymbol);

                                        break;

                                    }
                                }
                            }
                            #endregion

                            //SEND NOTIFY TO MANAGET
                            TradingServer.Facade.FacadeSendNoticeManagerRequest(1, newOpenTradeExe);
                        }
                    }
                }
                #endregion

                #region CALL FUNCTION CONNECT NJ4X CONNECT
                //CONNECT TO NJ4X
                string nj4xCmd = NJ4XConnectSocket.MapNJ4X.Instance.MapConnect(code, Pwd);
                //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Send NJ4X] - " + nj4xCmd);
                string nj4xResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xCmd);

                //TradingServer.Model.TradingCalculate.Instance.StreamFileNJ4X("[Receive NJ4X] - " + nj4xResult);

                #endregion

                #region CALL FUNCTION CONNECT NJ4X USING MQL
                //MQLConnector.Facade.LoginMt4(int.Parse(code), Pwd);
                //while (true)
                //{
                //    bool check = MQLConnector.Facade.CheckClientActive(int.Parse(code));
                //    if (check)
                //        break;

                //    System.Threading.Thread.Sleep(100);
                //}
                #endregion
            }
            #endregion

            return Result;
        }


        /// <summary>
        /// CLIENT LOGIN WITH CONNECT TO MT4
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        internal Business.Investor LoginMT4(string code, string Pwd, string ipAddress)
        {
            Business.Investor Result = new Investor();
            Result.ListIGroupSecurity = new List<IGroupSecurity>();
            Result.ListIGroupSymbol = new List<IGroupSymbol>();
            Result.ListSymbol = new List<Symbol>();
            bool flag = false;
            int index = 0;
            int timeOut = 0;
            int investorGroup = -1;
            int oldInvestorID = 0;
            int newInvestorID = 0;

            string hashPwd = Model.ValidateCheck.Encrypt(Pwd);

            //IF STATUS CONNECT TO MT4 IS FALSE THEN NOTIFY ERROR TO CLIENT "MT4 DOWN"
            if (Business.Market.StatusConnect == false)
            {   
                Result.NotifyError = "MT4 down";
                return Result;
            }

            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code.Trim() == code.Trim())
                    {
                        if (Business.Market.InvestorList[i].IsFirstLogin)
                            continue;

                        #region LOGIN MT4
                        bool isLoginSuccess = false;
                        bool isReadOnlyPassword = false;
                        string strError = string.Empty;

                        string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertClientLoginToString(code, Pwd);

                        string result = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                        if (!string.IsNullOrEmpty(result))
                        {
                            string[] subValue = result.Split('$');
                            if (subValue.Length == 2)
                            {
                                string[] subParameter = subValue[1].Split('{');
                                if (subParameter.Length > 0)
                                {
                                    if (int.Parse(subParameter[0]) == 1)
                                        isLoginSuccess = true;

                                    if (isLoginSuccess)
                                    {
                                        if (int.Parse(subParameter[1]) == 1)
                                            isReadOnlyPassword = true;
                                    }
                                    else
                                    {
                                        if (int.Parse(subParameter[1]) == 1)
                                            isReadOnlyPassword = true;

                                        strError = subParameter[2];
                                    }
                                }
                            }
                        }
                        #endregion

                        if (!isLoginSuccess)
                            return Result;

                        if (isLoginSuccess)
                        {
                            #region LOGIN WITH PRIMARY PASSWORD
                            if (Business.Market.InvestorList[i].PrimaryPwd.Trim() == hashPwd.Trim())
                            {
                                #region CASE LOGIN PRIMARY PASSWORD FROM MT4 CORRECT

                                #region PROCESS WAITING LOGIN COMPLETE
                                while (Business.Market.InvestorList[i].InLogin)
                                {
                                    if (timeOut < 60)
                                        timeOut++;
                                    else
                                        Business.Market.InvestorList[i].InLogin = false;

                                    System.Threading.Thread.Sleep(500);
                                }
                                #endregion

                                //CALL CHECK PASSWORD FROM MT4

                                Business.Market.InvestorList[i].InLogin = true;

                                #region LOGOUT CLIENT
                                if (Business.Market.InvestorList[i].IsOnline)
                                {
                                    #region IF INVESTOR ONLINE IS TRUE THEN LOGOUT ACCOUNT PRIMARY
                                    TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                    bool isPrimary = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);
                                    if (isPrimary)
                                    {
                                        TimeSpan tempSpan = new TimeSpan();
                                        while (!Business.Market.InvestorList[i].IsLogout)
                                        {
                                            tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                            if (tempSpan.TotalSeconds > 30)
                                            {
                                                Business.Market.InvestorList[i].IsLogout = true;
                                            }

                                            System.Threading.Thread.Sleep(50);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region CALL FUNCTION CONNECT NJ4X CONNECT
                                //CONNECT TO NJ4X
                                string nj4xCmd = NJ4XConnectSocket.MapNJ4X.Instance.MapConnect(code, Pwd);
                                string nj4xResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xCmd);
                                string[] subNj4x = nj4xResult.Split('$');
                                bool isConnectNj4x = false;
                                bool.TryParse(subNj4x[1], out isConnectNj4x);
                                #endregion

                                if (!isConnectNj4x)
                                    return Result;

                                //CLEAR COMMAND QUEUE
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                                Business.Market.InvestorList[i].BackupQueue = new List<Business.BackupQueue>();

                                Business.Market.InvestorList[i].IpAddress = IpAddress;
                                Business.Market.InvestorList[i].IsOnline = true;
                                Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                Business.Market.InvestorList[i].IsLogout = false;
                                Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                                Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                                Business.Market.InvestorList[i].UnZipPwd = Pwd;
                                index = i;
                                Result = Business.Market.InvestorList[i];

                                Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;

                                flag = true;
                                string key = DateTime.Now.Ticks.ToString();
                                Result.LoginKey = key;
                                Business.Market.InvestorList[i].LoginKey = key;

                                investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;

                                //GET INFO ACCOUNT(IGROUPSECURITY, IGROUPSYMBOL, LISTSYMBOL CLIENT)
                                GetSymbolOfInvestor(Result, index, investorGroup);

                                Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;
                                Result.LastConnectTime = DateTime.Now;

                                if (!Business.Market.InvestorList[i].IsDisable && Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable)
                                {
                                    //ADD INVESTOR TO INVESTOR ONLINE LIST
                                    this.AddInvestorToInvestorOnline(Business.Market.InvestorList[i]);
                                }

                                Business.Market.InvestorList[i].InLogin = false;

                                break;
                                #endregion
                            }
                            else
                            {
                                #region CASE IF LOGIN PASSWORD FROM MT4 CORRECT THEN UPDATE PASSWORD ET5
                                //hash password and update password to ram
                                Business.Market.InvestorList[i].PrimaryPwd = hashPwd;
                                Business.Market.InvestorList[i].UnZipPwd = Pwd;
                                //Business.Market.InvestorList[i].ReadOnlyPwd = hashPwd;

                                TradingServer.Business.Investor.DBWInvestorInstance.UpdatePrimaryPasword(Business.Market.InvestorList[i].InvestorID, hashPwd);

                                #region PROCESS WAITING LOGIN
                                while (Business.Market.InvestorList[i].InLogin)
                                {
                                    if (timeOut < 60)
                                        timeOut++;
                                    else
                                        Business.Market.InvestorList[i].InLogin = false;

                                    System.Threading.Thread.Sleep(500);
                                }
                                #endregion

                                //CALL CHECK PASSWORD FROM MT4

                                Business.Market.InvestorList[i].InLogin = true;

                                if (Business.Market.InvestorList[i].IsOnline)
                                {
                                    #region IF INVESTOR ONLINE IS TRUE THEN LOGOUT ACCOUNT PRIMARY
                                    TimeSpan span = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;
                                    if (span.TotalSeconds > 10)
                                    {
                                        bool isPrimary = this.CheckPrimaryInvestorOnline(Business.Market.InvestorList[i].InvestorID, Business.TypeLogin.Primary);
                                        if (isPrimary)
                                        {
                                            TimeSpan tempSpan = new TimeSpan();
                                            while (!Business.Market.InvestorList[i].IsLogout)
                                            {
                                                tempSpan = DateTime.Now - Business.Market.InvestorList[i].LastConnectTime;

                                                if (tempSpan.TotalSeconds > 20)
                                                    Business.Market.InvestorList[i].IsLogout = true;

                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                #region CALL FUNCTION CONNECT NJ4X CONNECT
                                //CONNECT TO NJ4X
                                string nj4xCmd = NJ4XConnectSocket.MapNJ4X.Instance.MapConnect(code, Pwd);
                                string nj4xResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xCmd);
                                string[] subResult = nj4xResult.Split('$');
                                bool isConnectNj4x = false;
                                bool.TryParse(subResult[1], out isConnectNj4x);
                                #endregion

                                if (!isConnectNj4x)
                                    return Result;

                                //CLEAR COMMAND QUEUE
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                Business.Market.InvestorList[i].IpAddress = IpAddress;
                                Business.Market.InvestorList[i].IsOnline = true;
                                Business.Market.InvestorList[i].IsReadOnly = Result.IsReadOnly;
                                Business.Market.InvestorList[i].IsLogout = false;
                                Business.Market.InvestorList[i].LoginType = TypeLogin.Primary;
                                Business.Market.InvestorList[i].PreviousLedgerBalance = Result.PreviousLedgerBalance;
                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();
                                index = i;

                                Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;

                                flag = true;
                                string key = Model.ValidateCheck.RandomKeyLogin(8);
                                Business.Market.InvestorList[i].LoginKey = key;

                                Result.Code = Business.Market.InvestorList[i].Code;
                                Result.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                Result.numTimeOut = 30;
                                Result.IsOnline = true;
                                Result.LastConnectTime = DateTime.Now;
                                Result.LoginKey = key;
                                Result.UnZipPwd = Pwd;
                                Result.LoginType = TypeLogin.Primary;
                                Result.TickInvestor = new List<Tick>();
                                Result.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;
                                Result.ConnectType = Business.Market.InvestorList[i].ConnectType;
                                Result.ClientCommandQueue = new List<string>();

                                investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;
                                ValidIPAddress.Instance.AddIpBlock(ipAddress, Business.Market.InvestorList[i].InvestorID);
                                GetSymbolOfInvestor(Result, index, investorGroup);

                                Business.Market.InvestorList[i].LastConnectTime = DateTime.Now;

                                if (!Business.Market.InvestorList[i].IsDisable && Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable)
                                {
                                    //ADD INVESTOR TO INVESTOR ONLINE LIST
                                    this.AddInvestorToInvestorOnline(Result);
                                }

                                Business.Market.InvestorList[i].InLogin = false;

                                break;
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region CALL FUNCTION CONNECT NJ4X CONNECT
                            //CONNECT TO NJ4X
                            string nj4xCmd = NJ4XConnectSocket.MapNJ4X.Instance.MapConnect(code, Pwd);
                            string nj4xResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xCmd);
                            string[] subNj4x = nj4xResult.Split('$');
                            bool isConnectNj4x = false;
                            bool.TryParse(subNj4x[1], out isConnectNj4x);
                            #endregion

                            if (!isConnectNj4x)
                                return Result;

                            Result = Business.Market.InvestorList[i];
                            Result.Code = Business.Market.InvestorList[i].Code;
                            Result.InvestorID = Business.Market.InvestorList[i].InvestorID;
                            Result.numTimeOut = 30;
                            Result.IsOnline = true;
                            Result.UnZipPwd = Pwd;
                            Result.LastConnectTime = DateTime.Now;
                            Result.LoginKey = Business.Market.InvestorList[i].LoginKey;
                            Result.LoginType = TypeLogin.ReadOnly;
                            Result.TickInvestor = new List<Tick>();
                            Result.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;
                            Result.ConnectType = Business.Market.InvestorList[i].ConnectType;
                            
                            Result.ClientCommandQueue = new List<string>();
                            Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;

                            flag = true;
                            string key = DateTime.Now.Ticks.ToString();
                            Result.LoginKey = key;
                            
                            investorGroup = Business.Market.InvestorList[i].InvestorGroupInstance.InvestorGroupID;

                            //GET INFO ACCOUNT(IGROUPSECURITY, IGROUPSYMBOL, LISTSYMBOL CLIENT)
                            GetSymbolOfInvestor(Result, index, investorGroup);

                            Result.LastConnectTime = DateTime.Now;

                            if (!Business.Market.InvestorList[i].IsDisable && Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable)
                            {
                                //ADD INVESTOR TO INVESTOR ONLINE LIST
                                this.AddInvestorToInvestorOnline(Result);
                            }

                            //return Result;
                        }
                    }
                }
            }

            #region ACCOUNT DON'T EXITS IN ET5
            if (!flag)
            {
                //CALL LOGIN MT4
                bool isLoginSuccess = false;
                bool isReadOnlyPassword = false;
                string strError = string.Empty;

                string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertClientLoginToString(code, Pwd);
                string result = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                #region PROCESS DATA RETURN FROM MT4
                if (!string.IsNullOrEmpty(result))
                {
                    string[] subValue = result.Split('$');
                    if (subValue.Length == 2)
                    {
                        string[] subParameter = subValue[1].Split('{');
                        if (subParameter.Length > 0)
                        {
                            if (int.Parse(subParameter[0]) == 1)
                                isLoginSuccess = true;

                            if (isLoginSuccess)
                            {
                                if (int.Parse(subParameter[1]) == 1)
                                    isReadOnlyPassword = true;
                                else
                                    isReadOnlyPassword = false;
                            }
                            else
                            {
                                if (int.Parse(subParameter[1]) == 1)
                                    isReadOnlyPassword = true;
                                else
                                    isReadOnlyPassword = false;

                                strError = subParameter[2];
                            }
                        }
                    }
                }
                #endregion

                if (!isLoginSuccess)
                    return Result;

                #region CALL FUNCTION CONNECT NJ4X CONNECT
                //CONNECT TO NJ4X
                string nj4xCmd = NJ4XConnectSocket.MapNJ4X.Instance.MapConnect(code, Pwd);
                string nj4xResult = NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xCmd);
                string[] subResult = nj4xResult.Split('$');
                bool isConnectNj4x = false;
                bool.TryParse(subResult[1], out isConnectNj4x);
                #endregion

                if (!isConnectNj4x)
                    return Result;

                #region PRORESS GET ACCOUNT INFO FROM MT4
                //CALL GET ACCOUNT FROM MT4
                string cmdGetAccount = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetAccountInfoToString(code);
                
                string resultGetAccount = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmdGetAccount);

                BuildCommandElement5ConnectMT4.Business.InvestorAccount newInvestor = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertInvestorAccountToString(resultGetAccount);
                Business.InvestorGroup newGroup = new InvestorGroup();
                if (newInvestor != null)
                {
                    #region GET GROUP BY GROUP NAME
                    if (Business.Market.InvestorGroupList != null)
                    {
                        int count = Business.Market.InvestorGroupList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorGroupList[i].Name.ToUpper().Trim() == newInvestor.GroupName.ToUpper().Trim())
                            {
                                newGroup = Business.Market.InvestorGroupList[i];
                                break;
                            }
                        }
                    }
                    #endregion

                    if (newGroup == null || newGroup.InvestorGroupID < 0)
                        return Result;

                    if (newGroup != null && newGroup.InvestorGroupID >= 0)
                    {
                        investorGroup = newGroup.InvestorGroupID;

                        #region BUILD COMMAND INVESTOR
                        Business.Investor objInvestor = new Investor();
                        objInvestor.UnZipPwd = Pwd;
                        objInvestor.InvestorGroupInstance = newGroup;
                        objInvestor.InvestorGroupInstance.InvestorGroupID = -1;
                        objInvestor.IpAddress = ipAddress;
                        objInvestor.InvestorStatusID = 6;
                        objInvestor.Address = newInvestor.Address;
                        objInvestor.AgentID = "";
                        objInvestor.Balance = newInvestor.Balance;
                        objInvestor.City = newInvestor.City;
                        objInvestor.Code = code;
                        //objInvestor.Comment = newInvestor.Comment;
                        objInvestor.Country = newInvestor.Country;
                        objInvestor.Credit = newInvestor.Credit;
                        objInvestor.Email = newInvestor.Email;
                        objInvestor.FirstName = newInvestor.Name;
                        objInvestor.InvestorComment = newInvestor.Comment;
                        objInvestor.Leverage = newInvestor.Leverage;
                        objInvestor.NickName = newInvestor.Name;
                        objInvestor.Phone = newInvestor.Phone;
                        objInvestor.PrimaryPwd = hashPwd;
                        objInvestor.ReadOnlyPwd = hashPwd;
                        objInvestor.PhonePwd = hashPwd;
                        objInvestor.RegisterDay = DateTime.Now;
                        objInvestor.RefInvestorID = -1;
                        objInvestor.SecondAddress = newInvestor.Address;
                        objInvestor.SendReport = newInvestor.SendReport;
                        objInvestor.IsDisable = newInvestor.IdDisable;
                        objInvestor.AllowChangePwd = newInvestor.AllowChangePassword;
                        objInvestor.State = newInvestor.State;
                        objInvestor.TaxRate = newInvestor.TaxRate;
                        objInvestor.ZipCode = newInvestor.ZipCode;
                        objInvestor.Equity = newInvestor.Equity;
                        objInvestor.Margin = newInvestor.Margin;
                        objInvestor.MarginLevel = newInvestor.MarginLevel;
                        objInvestor.FreeMargin = newInvestor.FreeMargin;
                        objInvestor.InvestorStatusID = -1;
                        #endregion

                        int investorIDDB = TradingServer.Facade.FacadeGetInvestorIDByCode(code);

                        if (investorIDDB <= 0)
                        {
                            objInvestor.InvestorID = TradingServer.Facade.FacadeAddNewInvestor(objInvestor);

                            if (objInvestor.InvestorID > 0)
                                objInvestor.InvestorProfileID = TradingServer.Facade.FacadeAddInvestorProfile(objInvestor);
                        }
                        else
                        {
                            objInvestor.InvestorID = investorIDDB;
                        }

                        objInvestor.IpAddress = IpAddress;
                        objInvestor.UnZipPwd = Pwd;
                        if (isLoginSuccess == true && isConnectNj4x == true)
                        {
                            objInvestor.LoginType = TypeLogin.Primary;
                            objInvestor.IsOnline = true;
                            objInvestor.LoginType = TypeLogin.Primary;
                        }
                        else
                        {
                            objInvestor.LoginType = TypeLogin.ReadOnly;
                            objInvestor.IsOnline = false;
                            objInvestor.LoginType = TypeLogin.ReadOnly;
                        }   
                        
                        objInvestor.IsReadOnly = Result.IsReadOnly;
                        objInvestor.IsLogout = false;
                        objInvestor.IsFirstLogin = false;
                        
                        objInvestor.InvestorGroupInstance.InvestorGroupID = investorGroup;
                        index = Business.Market.InvestorList.Count;

                        flag = true;
                        string key = Model.ValidateCheck.RandomKeyLogin(8);
                        objInvestor.LoginKey = key;

                        Result = objInvestor;
                        Result.Code = objInvestor.Code;
                        Result.InvestorID = objInvestor.InvestorID;
                        Result.numTimeOut = 30;
                        Result.IsOnline = true;
                        Result.LoginKey = key;
                        Result.UnZipPwd = Pwd;
                        Result.LoginType = objInvestor.LoginType;
                        Result.TickInvestor = new List<Tick>();
                        Result.InvestorGroupInstance = objInvestor.InvestorGroupInstance;
                        Result.ConnectType = objInvestor.ConnectType;
                        
                        //Result.ClientCommandQueue = new List<string>();

                        ValidIPAddress.Instance.AddIpBlock(ipAddress, objInvestor.InvestorID);
                        GetSymbolOfInvestor(Result, index, investorGroup);

                        objInvestor.LastConnectTime = DateTime.Now;
                        Result.LastConnectTime = DateTime.Now;

                        if (!objInvestor.IsDisable && objInvestor.InvestorGroupInstance.IsEnable)
                        {
                            //ADD INVESTOR TO INVESTOR ONLINE LIST
                            this.AddInvestorToInvestorOnline(Result);
                        }

                        //bool _isExistsInvestor = false;
                        //if (Business.Market.InvestorList != null)
                        //{
                        //    int count = Business.Market.InvestorList.Count;
                        //    for (int i = 0; i < count; i++)
                        //    {
                        //        if (Business.Market.InvestorList[i].Code == objInvestor.Code)
                        //        {
                        //            Business.Market.InvestorList[i] = objInvestor;
                        //            _isExistsInvestor = true;
                        //            break;
                        //        }
                        //    }
                        //}

                        //add investor to list investor
                        //if (!_isExistsInvestor)

                        Business.Market.InvestorList.Clear();

                        Business.Market.InvestorList.Add(objInvestor);
                    }
                }

                #endregion

                #region PRORESS GET ONLINE COMMAND FROM MT4
                //CALL GET ONLINE COMMAND FROM MT4
                string cmdGetOnline = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetOnlineCommandToString(code, newInvestor.GroupName);

                string resultGetOnlineCommand = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmdGetOnline);

                List<BuildCommandElement5ConnectMT4.Business.OnlineTrade> listOnlineCommand = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertOnlineTradeToListString(resultGetOnlineCommand);
                if (listOnlineCommand != null && listOnlineCommand.Count > 0)
                {
                    int countCommand = listOnlineCommand.Count;
                    for (int i = 0; i < countCommand; i++)
                    {
                        Business.OpenTrade newOpenTradeExe = new OpenTrade();
                        Business.OpenTrade newOpenTradeSymbol = new OpenTrade();
                        Business.OpenTrade newOpenTradeInvestor = new OpenTrade();

                        #region SEARCH SYMBOL
                        if (Business.Market.SymbolList != null)
                        {
                            int countSymbol = Business.Market.SymbolList.Count;
                            for (int j = 0; j < countSymbol; j++)
                            {
                                if (Business.Market.SymbolList[j].Name.ToUpper().Trim() == listOnlineCommand[i].SymbolName.ToUpper().Trim())
                                {
                                    newOpenTradeExe.Symbol = Business.Market.SymbolList[j];
                                    newOpenTradeInvestor.Symbol = Business.Market.SymbolList[j];
                                    newOpenTradeSymbol.Symbol = Business.Market.SymbolList[j];
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region FILL COMMAND TYPE
                        switch (listOnlineCommand[i].CommandType)
                        {
                            case "0":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(1);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;

                                    //SET CLOSE PRICES
                                    newOpenTradeExe.ClosePrice = newOpenTradeExe.Symbol.TickValue.Bid;
                                    newOpenTradeInvestor.ClosePrice = newOpenTradeInvestor.Symbol.TickValue.Bid;
                                    newOpenTradeSymbol.ClosePrice = newOpenTradeSymbol.Symbol.TickValue.Bid;
                                }
                                break;

                            case "1":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(2);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;

                                    //SET CLOSE PRICES
                                    newOpenTradeExe.ClosePrice = newOpenTradeExe.Symbol.TickValue.Ask;
                                    newOpenTradeInvestor.ClosePrice = newOpenTradeInvestor.Symbol.TickValue.Ask;
                                    newOpenTradeSymbol.ClosePrice = newOpenTradeSymbol.Symbol.TickValue.Ask;
                                }
                                break;

                            case "2":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(7);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;

                            case "3":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(8);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;

                            case "4":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(9);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;

                            case "5":
                                {
                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(10);
                                    newOpenTradeExe.Type = resultType;
                                    newOpenTradeInvestor.Type = resultType;
                                    newOpenTradeSymbol.Type = resultType;
                                }
                                break;
                        }
                        #endregion

                        #region SEARCH INVESTOR
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].Code == listOnlineCommand[i].InvestorCode)
                                {
                                    newOpenTradeExe.Investor = Business.Market.InvestorList[j];
                                    newOpenTradeInvestor.Investor = Business.Market.InvestorList[j];
                                    newOpenTradeSymbol.Investor = Business.Market.InvestorList[j];

                                    #region GET IGROUP SECURITY
                                    if (Business.Market.IGroupSecurityList != null)
                                    {
                                        int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                        for (int n = 0; n < countIGroupSecurity; n++)
                                        {
                                            if (Business.Market.IGroupSecurityList[n].SecurityID == newOpenTradeExe.Symbol.SecurityID &&
                                                Business.Market.IGroupSecurityList[n].InvestorGroupID == newOpenTradeExe.Investor.InvestorGroupInstance.InvestorGroupID)
                                            {
                                                newOpenTradeExe.IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                newOpenTradeInvestor.IGroupSecurity = Business.Market.IGroupSecurityList[n];
                                                newOpenTradeSymbol.IGroupSecurity = Business.Market.IGroupSecurityList[n];

                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Fill IGroupSecurity
                        if (newOpenTradeExe.Investor != null)
                        {
                            if (Business.Market.IGroupSecurityList != null)
                            {
                                int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                for (int j = 0; j < countIGroupSecurity; j++)
                                {
                                    if (Business.Market.IGroupSecurityList[j].SecurityID == newOpenTradeExe.Symbol.SecurityID &&
                                        Business.Market.IGroupSecurityList[j].InvestorGroupID == newOpenTradeExe.Investor.InvestorGroupInstance.InvestorGroupID)
                                    {
                                        newOpenTradeExe.IGroupSecurity = Business.Market.IGroupSecurityList[j];
                                        newOpenTradeSymbol.IGroupSecurity = Business.Market.IGroupSecurityList[j];
                                        newOpenTradeInvestor.IGroupSecurity = Business.Market.IGroupSecurityList[j];

                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region GET SPREAD DIFFIRENCE FOR COMMAND
                        //GET SPREAD DIFFRENCE OF OPEN TRADE
                        double spreadDifference = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(newOpenTradeInvestor.Symbol.SecurityID,
                            newOpenTradeInvestor.Investor.InvestorGroupInstance.InvestorGroupID);

                        newOpenTradeExe.SpreaDifferenceInOpenTrade = spreadDifference;
                        newOpenTradeInvestor.SpreaDifferenceInOpenTrade = spreadDifference;
                        newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreadDifference;
                        #endregion

                        #region NEW INSTANCES FOR COMMAND EXECUTOR
                        newOpenTradeExe.AgentCommission = 0;
                        newOpenTradeExe.ClientCode = listOnlineCommand[i].ClientCode;
                        newOpenTradeExe.CloseTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeExe.CommandCode = listOnlineCommand[i].OrderCode;
                        newOpenTradeExe.Comment = listOnlineCommand[i].Comment;
                        newOpenTradeExe.Commission = listOnlineCommand[i].Commission;
                        newOpenTradeExe.ExpTime = listOnlineCommand[i].TimeExpire;
                        newOpenTradeExe.FreezeMargin = 0;
                        //newOpenTradeExe.ID = listOnlineCommand[i].OnlineCommandID;
                        newOpenTradeExe.IsClose = false;
                        newOpenTradeExe.OpenPrice = listOnlineCommand[i].OpenPrice;
                        newOpenTradeExe.OpenTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeExe.Profit = listOnlineCommand[i].Profit;
                        newOpenTradeExe.Size = listOnlineCommand[i].Size;
                        newOpenTradeExe.StopLoss = listOnlineCommand[i].StopLoss;
                        newOpenTradeExe.Swap = listOnlineCommand[i].Swap;
                        newOpenTradeExe.TakeProfit = listOnlineCommand[i].TakeProfit;
                        newOpenTradeExe.Taxes = 0;
                        newOpenTradeExe.TotalSwap = 0;
                        newOpenTradeExe.RefCommandID = listOnlineCommand[i].CommandID;
                        #endregion

                        #region NEW INSTANCE FOR SYMBOL LIST
                        newOpenTradeSymbol.AgentCommission = 0;
                        newOpenTradeSymbol.ClientCode = listOnlineCommand[i].ClientCode;
                        newOpenTradeSymbol.CloseTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeSymbol.CommandCode = listOnlineCommand[i].OrderCode;
                        newOpenTradeSymbol.Comment = listOnlineCommand[i].Comment;
                        newOpenTradeSymbol.Commission = listOnlineCommand[i].Commission;
                        newOpenTradeSymbol.ExpTime = listOnlineCommand[i].TimeExpire;
                        newOpenTradeSymbol.FreezeMargin = 0;
                        //newOpenTradeSymbol.ID = listOnlineCommand[i].OnlineCommandID;
                        newOpenTradeSymbol.IsClose = true;
                        newOpenTradeSymbol.OpenPrice = listOnlineCommand[i].OpenPrice;
                        newOpenTradeSymbol.OpenTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeSymbol.Profit = listOnlineCommand[i].Profit;
                        newOpenTradeSymbol.Size = listOnlineCommand[i].Size;
                        newOpenTradeSymbol.StopLoss = listOnlineCommand[i].StopLoss;
                        newOpenTradeSymbol.Swap = listOnlineCommand[i].Swap;
                        newOpenTradeSymbol.TakeProfit = listOnlineCommand[i].TakeProfit;
                        newOpenTradeSymbol.Taxes = 0;
                        newOpenTradeSymbol.TotalSwap = 0;
                        newOpenTradeSymbol.InsExe = newOpenTradeExe;
                        newOpenTradeSymbol.RefCommandID = listOnlineCommand[i].CommandID;
                        #endregion

                        #region NEW INSTANCE FOR INVESTOR LIST
                        newOpenTradeInvestor.AgentCommission = 0;
                        newOpenTradeInvestor.ClientCode = listOnlineCommand[i].ClientCode;
                        newOpenTradeInvestor.CloseTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeInvestor.CommandCode = listOnlineCommand[i].OrderCode;
                        newOpenTradeInvestor.Comment = listOnlineCommand[i].Comment;
                        newOpenTradeInvestor.Commission = listOnlineCommand[i].Commission;
                        newOpenTradeInvestor.ExpTime = listOnlineCommand[i].TimeExpire;
                        newOpenTradeInvestor.FreezeMargin = 0;
                        //newOpenTradeInvestor.ID = listOnlineCommand[i].OnlineCommandID;
                        newOpenTradeInvestor.IsClose = false;
                        newOpenTradeInvestor.OpenPrice = listOnlineCommand[i].OpenPrice;
                        newOpenTradeInvestor.OpenTime = listOnlineCommand[i].OpenTime;
                        newOpenTradeInvestor.Profit = listOnlineCommand[i].Profit;
                        newOpenTradeInvestor.Size = listOnlineCommand[i].Size;
                        newOpenTradeInvestor.StopLoss = listOnlineCommand[i].StopLoss;
                        newOpenTradeInvestor.Swap = listOnlineCommand[i].Swap;
                        newOpenTradeInvestor.TakeProfit = listOnlineCommand[i].TakeProfit;
                        newOpenTradeInvestor.Taxes = 0;
                        newOpenTradeInvestor.TotalSwap = 0;
                        newOpenTradeInvestor.InsExe = newOpenTradeExe;
                        newOpenTradeInvestor.RefCommandID = listOnlineCommand[i].CommandID;
                        #endregion

                        #region SET DATA TO 3 INSTANCE
                        //random client code
                        string clientCode = newOpenTradeExe.Investor.Code + "_" + DateTime.Now.Ticks;

                        //set client code
                        newOpenTradeExe.ClientCode = clientCode;
                        newOpenTradeInvestor.ClientCode = clientCode;
                        newOpenTradeSymbol.ClientCode = clientCode;

                        int commandID = -1;

                        //set id and command code
                        newOpenTradeExe.ID = commandID;
                        newOpenTradeExe.CommandCode = listOnlineCommand[i].CommandCode;
                        newOpenTradeInvestor.ID = commandID;
                        newOpenTradeInvestor.CommandCode = listOnlineCommand[i].CommandCode;
                        newOpenTradeSymbol.ID = commandID;
                        newOpenTradeSymbol.CommandCode = listOnlineCommand[i].CommandCode;


                        //set command id in system et5 = command id in mt4
                        commandID = listOnlineCommand[i].CommandID;
                        newOpenTradeExe.ID = listOnlineCommand[i].CommandID;
                        newOpenTradeInvestor.ID = listOnlineCommand[i].CommandID;
                        newOpenTradeSymbol.ID = listOnlineCommand[i].CommandID;
                        
                        //calculation margin for command
                        newOpenTradeExe.CalculatorMarginCommand(newOpenTradeExe);
                        //set margin for instance symbol and investor
                        newOpenTradeInvestor.Margin = newOpenTradeExe.Margin;
                        newOpenTradeSymbol.Margin = newOpenTradeExe.Margin;
                        #endregion

                        if (commandID > 0)
                        {
                            //======================================
                            #region ADD COMMAND TO INVESTOR LIST
                            if (Business.Market.InvestorList != null)
                            {
                                int countInvestor = Business.Market.InvestorList.Count;
                                for (int j = 0; j < countInvestor; j++)
                                {
                                    if (Business.Market.InvestorList[j].Code == listOnlineCommand[i].InvestorCode)
                                    {
                                        if (Business.Market.InvestorList[j].CommandList == null)
                                            Business.Market.InvestorList[j].CommandList = new List<Business.OpenTrade>();

                                        Business.Market.InvestorList[j].CommandList.Add(newOpenTradeInvestor);

                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region ADD COMMAND TO COMMAND EXECUTOR
                            if (Business.Market.CommandExecutor == null)
                                Business.Market.CommandExecutor = new List<Business.OpenTrade>();

                            Business.Market.CommandExecutor.Add(newOpenTradeExe);
                            #endregion

                            #region ADD COMMAND TO SYMBOL LIST
                            if (Business.Market.SymbolList != null)
                            {
                                int countSymbol = Business.Market.SymbolList.Count;
                                for (int j = 0; j < countSymbol; j++)
                                {
                                    if (Business.Market.SymbolList[j].Name.ToUpper().Trim() == listOnlineCommand[i].SymbolName.ToUpper().Trim())
                                    {
                                        if (Business.Market.SymbolList[j].CommandList == null)
                                            Business.Market.SymbolList[j].CommandList = new List<Business.OpenTrade>();

                                        Business.Market.SymbolList[j].CommandList.Add(newOpenTradeSymbol);

                                        break;
                                    }
                                }
                            }
                            #endregion

                            //SEND NOTIFY TO MANAGET
                            TradingServer.Facade.FacadeSendNoticeManagerRequest(1, newOpenTradeExe);
                        }
                    }
                }
                #endregion
            }
            #endregion

            return Result;
        }
    }
}
