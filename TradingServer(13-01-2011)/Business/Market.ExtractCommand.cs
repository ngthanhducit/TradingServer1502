﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        #region Extract String Command To Investor Group Object
        /// <summary>
        /// Extract String Command To Investor Group Object
        /// </summary>
        /// <param name="Cmd">String Cmd</param>
        /// <returns>Business.InvestorGroup</returns>
        internal Business.InvestorGroup ExtractInvestorGroup(string Cmd)
        {
            Business.InvestorGroup Result = new InvestorGroup();
            if (!string.IsNullOrEmpty(Cmd))
            {
                int InvestorGroupID = -1;
                double defaultDeposite = 0;
                string[] subParameter = Cmd.Split(',');
                int.TryParse(subParameter[0], out InvestorGroupID);
                Result.InvestorGroupID = InvestorGroupID;
                Result.Name = subParameter[1];
                Result.Owner = subParameter[2];
                double.TryParse(subParameter[3], out defaultDeposite);
                Result.DefautDeposite = defaultDeposite;
            }

            return Result;
        }
        #endregion

        #region Extract String Command To List Parameter Object
        /// <summary>
        /// Extract String Command To List Parameter Object
        /// </summary>
        /// <param name="Cmd">String Cmd</param>
        /// <returns>List<Business.ParameterItem></returns>
        internal List<Business.ParameterItem> ExtractParameterItem(string Cmd)
        {
            List<Business.ParameterItem> Result = new List<ParameterItem>();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split('|');
                if (subParameter.Length > 0)
                {
                    int count = subParameter.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string[] subList = subParameter[i].Split(',');
                        if (subList.Length > 0)
                        {
                            int secondParameterId = 0;
                            int collectionValue = 0;
                            int boolValue = 0;
                            DateTime dateValue = new DateTime();

                            Business.ParameterItem newParameterItem = new ParameterItem();
                            int.TryParse(subList[0], out secondParameterId);
                            int.TryParse(subList[1], out collectionValue);
                            int.TryParse(subList[4], out boolValue);
                            DateTime.TryParse(subList[7], out dateValue);

                            newParameterItem.ParameterItemID = secondParameterId;
                            newParameterItem.SecondParameterID = secondParameterId;
                            newParameterItem.CollectionValue = new List<ParameterItem>();
                            newParameterItem.Name = subList[2];
                            newParameterItem.Code = subList[3];
                            newParameterItem.BoolValue = boolValue;
                            newParameterItem.StringValue = subList[5];
                            newParameterItem.NumValue = subList[6];
                            newParameterItem.DateValue = dateValue;

                            Result.Add(newParameterItem);
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Extract String Command To List Parameter Object
        /// </summary>
        /// <param name="Cmd">String Cmd</param>
        /// <returns>List<Business.ParameterItem></returns>
        internal List<Business.ParameterItem> ExtractParameterItemOne(string Cmd)
        {
            List<Business.ParameterItem> Result = new List<ParameterItem>();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split('|');
                if (subParameter.Length > 0)
                {
                    int count = subParameter.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string[] subList = subParameter[i].Split(',');
                        if (subList.Length > 0)
                        {
                            int secondParameterId = 0;
                            int collectionValue = 0;
                            int boolValue = 0;
                            DateTime dateValue = new DateTime();

                            Business.ParameterItem newParameterItem = new ParameterItem();
                            int.TryParse(subList[0], out secondParameterId);
                            int.TryParse(subList[1], out collectionValue);
                            int.TryParse(subList[4], out boolValue);
                            DateTime.TryParse(subList[7], out dateValue);

                            newParameterItem.ParameterItemID = secondParameterId;
                            newParameterItem.SecondParameterID = secondParameterId;
                            newParameterItem.CollectionValue = new List<ParameterItem>();
                            newParameterItem.Name = subList[2];
                            newParameterItem.Code = subList[3];
                            newParameterItem.BoolValue = boolValue;
                            newParameterItem.StringValue = subList[5];
                            newParameterItem.NumValue = subList[6];
                            newParameterItem.DateValue = dateValue;

                            Result.Add(newParameterItem);
                        }
                    }
                }
            }

            return Result;
        }
        #endregion       
        
        #region Extract String Command To IGroupSymbol Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal List<Business.IGroupSymbol> ExtractIGroupSymbol(string Cmd)
        {
            List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split('|');                
                if (subParameter.Length > 0)
                {
                    int count = subParameter.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string[] subValue = subParameter[i].Split(',');

                        int IGroupSymbolID = 0;
                        int symbolID = 0;
                        int investorGroupID = 0;

                        int.TryParse(subValue[0], out IGroupSymbolID);
                        int.TryParse(subValue[1], out symbolID);
                        int.TryParse(subValue[2], out investorGroupID);

                        Business.IGroupSymbol newIGroupSymbol = new IGroupSymbol();
                        newIGroupSymbol.IGroupSymbolID = IGroupSymbolID;
                        newIGroupSymbol.SymbolID = symbolID;
                        newIGroupSymbol.InvestorGroupID = investorGroupID;

                        Result.Add(newIGroupSymbol);
                    }                   
                }
            }

            return Result;
        }
        #endregion

        #region Extract String Command To IGroupSecurity Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> ExtractIGroupSecurity(string Cmd)
        {
            List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split('|');
                if (subParameter.Length > 0)
                {
                    int count = subParameter.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string[] subValue = subParameter[i].Split(',');

                        int IGroupSecurityID = 0;
                        int InvestorGroupID = 0;
                        int SecurityID = 0;

                        int.TryParse(subValue[0], out IGroupSecurityID);
                        int.TryParse(subValue[1], out InvestorGroupID);
                        int.TryParse(subValue[2], out SecurityID);

                        Business.IGroupSecurity newIGroupSymbol = new IGroupSecurity();
                        newIGroupSymbol.IGroupSecurityID = IGroupSecurityID;
                        newIGroupSymbol.InvestorGroupID = InvestorGroupID;
                        newIGroupSymbol.SecurityID = SecurityID;

                        Result.Add(newIGroupSymbol);
                    }
                }
            }

            return Result;
        }
        #endregion

        #region Extract String Command To Investor Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.Investor ExtractionInvestor(string Cmd)
        {
            Business.Investor Result = new Investor();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split(',');
                if (subParameter.Length > 0)
                {
                    int investorStatusID = 5;
                    int investorGroupID = 0;
                    //int agentID = 0;

                    double balance = 0;
                    double credit = 0;
                    double taxRate = 0;
                    double leverage = 0;

                    bool isDisable = false;
                    bool AllowChangePwd = false;
                    bool ReadOnly = false;
                    bool SendReport = false;

                    //int.TryParse(subParameter[0], out investorStatusID);
                    int.TryParse(subParameter[0], out investorGroupID);
                    //int.TryParse(subParameter[1], out agentID);

                    double.TryParse(subParameter[2], out balance);
                    double.TryParse(subParameter[3], out credit);
                    double.TryParse(subParameter[9], out taxRate);
                    double.TryParse(subParameter[10], out leverage);

                    bool.TryParse(subParameter[8], out isDisable);
                    bool.TryParse(subParameter[21], out AllowChangePwd);
                    bool.TryParse(subParameter[22], out ReadOnly);
                    bool.TryParse(subParameter[23], out SendReport);

                    Result.InvestorStatusID = investorStatusID;
                    //Result.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(investorGroupID);

                    if (Business.Market.InvestorGroupList != null)
                    {
                        int countGroup = Business.Market.InvestorGroupList.Count;
                        for (int i = 0; i < countGroup; i++)
                        {
                            if (Business.Market.InvestorGroupList[i].InvestorGroupID == investorGroupID)
                            {
                                Result.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                                break;
                            }
                        }
                    }

                    if (subParameter[1] == "")
                    {
                        Result.AgentID = string.Empty;
                    }
                    else
                    {
                        Result.AgentID = subParameter[1];
                    }

                    if (balance > 0)
                    {
                        Result.Balance = balance;
                    }
                    else
                    {
                        if (Result.InvestorGroupInstance.ParameterItems != null)
                        {
                            int countConfig = Result.InvestorGroupInstance.ParameterItems.Count;
                            for (int i = 0; i < countConfig; i++)
                            {
                                if (Result.InvestorGroupInstance.ParameterItems[i].Code == "G24")
                                {
                                    double tempBalance = 0;
                                    double.TryParse(Result.InvestorGroupInstance.ParameterItems[i].StringValue, out tempBalance);
                                    Result.Balance = tempBalance;

                                    break;
                                }
                            }
                        }
                    }

                    Result.Credit = credit;

                    if (!string.IsNullOrEmpty(subParameter[4]))
                    {                        
                        Result.Code = subParameter[4];
                    }
                    else
                    {
                        //Random ran = new Random();
                        //int Code = ran.Next(0000000, 9999999);
                        //Result.Code = Code.ToString();
                        Result.Code = TradingServer.Model.TradingCalculate.Instance.GetNextRandomCode();
                    }
                    
                    Result.PrimaryPwd = subParameter[5];
                    Result.ReadOnlyPwd = subParameter[6];
                    Result.PhonePwd = subParameter[7];
                    Result.IsDisable = isDisable;
                    Result.TaxRate = taxRate;
                    Result.Leverage = leverage;

                    //Investor Profile                                        
                    DateTime registerDay = new DateTime();
                    DateTime.TryParse(subParameter[17], out registerDay);

                    Result.Address = subParameter[11];
                    Result.Phone = subParameter[12];
                    Result.City = subParameter[13];
                    Result.Country = subParameter[14];
                    Result.Email = subParameter[15];
                    Result.ZipCode = subParameter[16];
                    Result.RegisterDay = DateTime.Now;
                    Result.InvestorComment = subParameter[18];
                    Result.State = subParameter[19];
                    Result.NickName = subParameter[20];
                    Result.AllowChangePwd = AllowChangePwd;
                    Result.ReadOnly = ReadOnly;
                    Result.SendReport = SendReport;

                    if (subParameter.Length == 25)
                    {
                        Result.IDPassport = subParameter[24];
                    }
                    else
                    {
                        if (subParameter.Length > 25)
                        {
                            int investorID = -1;
                            int.TryParse(subParameter[24], out investorID);
                            Result.InvestorID = investorID;
                        }

                        if (subParameter.Length > 26)
                        {
                            int investorProfileID = -1;
                            int.TryParse(subParameter[25], out investorProfileID);
                            Result.InvestorProfileID = investorProfileID;

                            Result.IDPassport = subParameter[26];
                        }
                    }                    
                }
            }

            return Result;
        }
        #endregion        

        #region Extract String Command To Agent Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.Agent ExtractionAgent(string Cmd)
        {
            Business.Agent result = new Agent();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split('}');
                if (subParameter.Length > 0)
                {
                    int AgentID = 0;
                    int agentGroupID = 0;
                    int investorID = 0;
                    bool isDisable = false;
                    bool isIpFilter = false;

                    int.TryParse(subParameter[0], out agentGroupID);
                    result.Name = subParameter[1];
                    int.TryParse(subParameter[2], out investorID);
                    result.Comment = subParameter[3];
                    bool.TryParse(subParameter[4], out isDisable);
                    bool.TryParse(subParameter[5], out isIpFilter);

                    if (subParameter.Length > 8)
                    {
                        int.TryParse(subParameter[8], out AgentID);
                        result.AgentID = AgentID;
                    }
                    result.AgentGroupID = agentGroupID;
                    result.InvestorID = investorID;
                    result.IsDisable = isDisable;
                    result.IsIpFilter = isIpFilter;
                    result.IpForm = subParameter[6];
                    result.IpTo = subParameter[7];
                    result.Code = subParameter[9];
                    result.Pwd = subParameter[10];
                    result.GroupCondition = subParameter[11];
                }
            }
            return result;
        }
        #endregion  

        #region Extract String Command To IAgentSecurity Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal List<Business.IAgentSecurity> ExtractionIAgentSecurity(string Cmd)
        {
            List<Business.IAgentSecurity> Result = new List<IAgentSecurity>();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split('|');
                if (subParameter.Length > 0)
                {
                    int AgentID = 0;
                    int.TryParse(subParameter[0], out AgentID);
                    int count = subParameter.Length;
                    for (int i = 1; i < count; i++)
                    {
                        string[] subValue = subParameter[i].Split(',');

                        int SecurityID = 0;
                        bool IsUse = false;
                        double MinLots = 0;
                        double MaxLots = 0;
                        int.TryParse(subValue[0], out SecurityID);
                        bool.TryParse(subValue[1], out IsUse);
                        double.TryParse(subValue[2], out MinLots);
                        double.TryParse(subValue[3], out MaxLots);

                        Business.IAgentSecurity newIAgentSecurity = new IAgentSecurity();
                        newIAgentSecurity.AgentID = AgentID;
                        newIAgentSecurity.SecurityID = SecurityID;
                        newIAgentSecurity.Use = IsUse;
                        newIAgentSecurity.MinLots = MinLots;
                        newIAgentSecurity.MaxLots = MaxLots;
                        Result.Add(newIAgentSecurity);
                    }
                }
            }
            return Result;
        }
        #endregion 

        #region Extract String Command To RequestDealer Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.RequestDealer ExtractionRequestDealer(string Cmd)
        {
            Business.RequestDealer Result = new RequestDealer();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split(',');
                if (subParameter.Length > 0)
                {
                    int AgentID = 0;
                    bool FlagConfirm = false;
                    double MaxDev = 0;
                    DateTime TimeAgentReceive = new DateTime();
                    DateTime TimeClientRequest = new DateTime();
                    int InvestorID = 0;
                    double Size = 0;
                    DateTime OpenTime = new DateTime();
                    double OpenPrice = 0;
                    double StopLoss = 0;
                    double TakeProfit = 0;
                    double ClosePrice = 0;
                    double Commission = 0;
                    double Swap = 0;
                    double Profit = 0;
                    int ID = 0;
                    DateTime ExpTime = new DateTime();
                    bool IsHedged = false, IsMutiClose = false;
                    int TypeID = 0;
                    double Margin = 0;

                    int.TryParse(subParameter[0], out AgentID);
                    Result.AgentID = AgentID;
                    bool.TryParse(subParameter[1], out FlagConfirm);
                    Result.FlagConfirm = FlagConfirm;
                    double.TryParse(subParameter[2], out MaxDev);
                    Result.MaxDev = MaxDev;
                    Result.Name = subParameter[3];
                    Result.Notice = subParameter[4];
                    DateTime.TryParse(subParameter[5], out TimeAgentReceive);
                    Result.TimeAgentReceive = TimeAgentReceive;
                    DateTime.TryParse(subParameter[6], out TimeClientRequest);
                    Result.TimeClientRequest = TimeClientRequest;
                    int.TryParse(subParameter[7], out InvestorID);
                    Result.Request = new OpenTrade();
                    Result.Request.Investor = new Investor();
                    Result.Request.Investor.InvestorID = InvestorID;
                    Result.InvestorID = InvestorID;
                    Result.Request.Symbol = new Symbol();
                    Result.Request.Symbol.Name = subParameter[8];
                    double.TryParse(subParameter[9], out Size);
                    Result.Request.Size = Size;
                    DateTime.TryParse(subParameter[10], out OpenTime);
                    Result.Request.OpenTime = OpenTime;
                    double.TryParse(subParameter[11], out OpenPrice);
                    Result.Request.OpenPrice = OpenPrice;
                    double.TryParse(subParameter[12], out StopLoss);
                    Result.Request.StopLoss = StopLoss;
                    double.TryParse(subParameter[13], out TakeProfit);
                    Result.Request.TakeProfit = TakeProfit;
                    double.TryParse(subParameter[14], out ClosePrice);
                    Result.Request.ClosePrice = ClosePrice;
                    double.TryParse(subParameter[15], out Commission);
                    Result.Request.Commission = Commission;
                    double.TryParse(subParameter[16], out Swap);
                    Result.Request.Swap = Swap;
                    double.TryParse(subParameter[17], out Profit);
                    Result.Request.Profit = Profit;
                    int.TryParse(subParameter[18], out ID);
                    Result.Request.ID = ID;
                    DateTime.TryParse(subParameter[19], out ExpTime);
                    Result.Request.ExpTime = ExpTime;
                    Result.Request.ClientCode = subParameter[20];
                    bool.TryParse(subParameter[21],out IsHedged);
                    int.TryParse(subParameter[22], out TypeID);
                    Result.Request.Type = new TradeType();
                    Result.Request.Type.ID = TypeID;
                    double.TryParse(subParameter[23], out Margin);
                    Result.Request.Margin = Margin;
                    Result.Request.CommandCode = subParameter[28];
                    bool.TryParse(subParameter[29], out IsMutiClose);
                    Result.Request.IsMultiClose = IsMutiClose;
                    int.TryParse(subParameter[30], out ID);
                    Result.Request.RefCommandID = ID;
                }
            }

            return Result;
        }
        #endregion

        #region Extract String Command To AgentGroup Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.AgentGroup ExtractionAgentGroup(string Cmd)
        {
            Business.AgentGroup result = new AgentGroup();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split(',');
                if (subParameter.Length > 0)
                {
                    int AgentGroupID = 0;
                    result.Name = subParameter[0];
                    result.Comment = subParameter[1];
                    if (subParameter.Length > 2)
                    {
                        int.TryParse(subParameter[2], out AgentGroupID);
                        result.AgentGroupID = AgentGroupID;
                    }
                }
            }
            return result;
        }
        #endregion  

        #region Extract String Command To Permit Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.Permit ExtractionPermit(string Cmd)
        {
            Business.Permit result = new Permit();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split(',');
                if (subParameter.Length > 0)
                {
                    int PermitID = 0;
                    int AgentGroupID = 0;
                    int AgentID = 0;
                    int RoleID = 0;
                    int.TryParse(subParameter[0], out AgentGroupID);
                    int.TryParse(subParameter[1], out AgentID);
                    int.TryParse(subParameter[2], out RoleID);
                    if (subParameter.Length > 3)
                    {
                        int.TryParse(subParameter[3], out PermitID);
                        result.PermitID = PermitID;
                    }
                    result.AgentGroupID = AgentGroupID;
                    result.AgentID = AgentID;
                    result.Role = new Role();
                    result.Role.RoleID = RoleID;
                }
            }
            return result;
        }
        #endregion  

        #region Extract String Command To Role Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.Role ExtractionRole(string Cmd)
        {
            Business.Role result = new Role();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subParameter = Cmd.Split(',');
                if (subParameter.Length > 0)
                {
                    int RoleID = 0;
                    result.Code = subParameter[0];
                    result.Name = subParameter[1];
                    result.Comment = subParameter[2];
                    if (subParameter.Length > 3)
                    {
                        int.TryParse(subParameter[3], out RoleID);
                        result.RoleID = RoleID;
                    }
                }
            }
            return result;
        }
        #endregion  

        #region Extract String Command To TimeEvent Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal List<Business.TimeEvent> ExtractTimeEvent(string Cmd,string TargetPosition,string Code,int ID)
        {
            List<Business.TimeEvent> Result = new List<TimeEvent>();
            if (!string.IsNullOrEmpty(Cmd))
            {   
                int HourFrist = -1;
                int MinuteFirst = -1;
                int HourSecond = -1;
                int MinuteSecond = -1;
                string[] subParameter = Cmd.Split('~');
                if (subParameter.Length > 0)
                {
                    int count = subParameter.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string[] subEvent = subParameter[i].Split('#');
                        if (subEvent.Length > 1)
                        {
                            string[] subSession = subEvent[1].Split('+');
                            if (subSession.Length > 0)
                            {
                                int countSession = subSession.Length;
                                for (int j = 0; j < countSession; j++)
                                {
                                    string[] subTimeSession = subSession[j].Split('-');
                                    if (subTimeSession.Length > 1)
                                    {
                                        string[] subTimeStart = subTimeSession[0].Split(':');
                                        string[] subTimeEnd = subTimeSession[1].Split(':');

                                        switch (Code)
                                        {
                                            case "S042":
                                                {
                                                    #region Add Event Quote Config
                                                    int.TryParse(subTimeStart[0], out HourFrist);
                                                    int.TryParse(subTimeStart[1], out MinuteFirst);

                                                    Business.TimeEvent TimeEventStart = new TimeEvent();
                                                    Business.DateTimeEvent TimeEvent = new DateTimeEvent();
                                                    TimeEventStart.EventType = TimeEventType.EndCloseTradeSymbol;
                                                    TimeEvent.Day = -1;
                                                    TimeEvent.DayInWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), subEvent[0], true);
                                                    TimeEvent.Hour = HourFrist;
                                                    TimeEvent.Minute = MinuteFirst;
                                                    TimeEvent.Month = -1;
                                                    TimeEventStart.Time = TimeEvent;
                                                    TimeEventStart.TimeEventID = ID;
                                                    TimeEventStart.NumSession = j;

                                                    Business.TargetFunction newTargetFunctionStart = new TargetFunction();
                                                    newTargetFunctionStart.EventPosition = TargetPosition;
                                                    newTargetFunctionStart.Function = this.OpenTradeSymbol;
                                                    newTargetFunctionStart.NumSession = j;

                                                    if (TimeEventStart.TargetFunction == null)
                                                        TimeEventStart.TargetFunction = new List<TargetFunction>();

                                                    TimeEventStart.TargetFunction.Add(newTargetFunctionStart);

                                                    Result.Add(TimeEventStart);

                                                    //Add Event
                                                    Business.TimeEvent TimeEventEnd = new TimeEvent();
                                                    Business.DateTimeEvent DateTimeEnd = new DateTimeEvent();
                                                    TimeEventEnd.EventType = TimeEventType.BeginCloseQuoteSymbol;
                                                    int.TryParse(subTimeEnd[0], out HourSecond);
                                                    int.TryParse(subTimeEnd[1], out MinuteSecond);
                                                    DateTimeEnd.Day = -1;
                                                    DateTimeEnd.DayInWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), subEvent[0], true);
                                                    DateTimeEnd.Hour = HourSecond;
                                                    DateTimeEnd.Minute = MinuteSecond;
                                                    TimeEventEnd.Time = DateTimeEnd;
                                                    TimeEventEnd.TimeEventID = ID;
                                                    TimeEventEnd.NumSession = j;

                                                    Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                    newTargetFunctionEnd.EventPosition = TargetPosition;
                                                    newTargetFunctionEnd.Function = this.CloseTradeSymbol;
                                                    newTargetFunctionEnd.NumSession = j;

                                                    if (TimeEventEnd.TargetFunction == null)
                                                        TimeEventEnd.TargetFunction = new List<TargetFunction>();

                                                    TimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                    Result.Add(TimeEventEnd);
                                                    #endregion                                                    
                                                }
                                                break;

                                            case "S043":
                                                {
                                                    #region Add Trade Symbol Config
                                                    int.TryParse(subTimeStart[0], out HourFrist);
                                                    int.TryParse(subTimeStart[1], out MinuteFirst);
                                                    Business.TimeEvent TimeEventStart = new TimeEvent();
                                                    Business.DateTimeEvent TimeEvent = new DateTimeEvent();
                                                    TimeEventStart.EventType = TimeEventType.EndCloseQuoteSymbol;
                                                    TimeEvent.Day = -1;
                                                    TimeEvent.DayInWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), subEvent[0], true);
                                                    TimeEvent.Hour = HourFrist;
                                                    TimeEvent.Minute = MinuteFirst;
                                                    TimeEvent.Month = -1;
                                                    TimeEventStart.Time = TimeEvent;
                                                    TimeEventStart.TimeEventID = ID;
                                                    TimeEventStart.NumSession = j;

                                                    Business.TargetFunction newTargetFunctionStart = new TargetFunction();
                                                    newTargetFunctionStart.EventPosition = TargetPosition;
                                                    newTargetFunctionStart.Function = this.OpenQuoteSymbol;
                                                    newTargetFunctionStart.NumSession = j;

                                                    if (TimeEventStart.TargetFunction == null)
                                                        TimeEventStart.TargetFunction = new List<TargetFunction>();

                                                    TimeEventStart.TargetFunction.Add(newTargetFunctionStart);

                                                    Result.Add(TimeEventStart);

                                                    Business.TimeEvent TimeEventEnd = new TimeEvent();
                                                    Business.DateTimeEvent DateTimeEnd = new DateTimeEvent();
                                                    TimeEventEnd.EventType = TimeEventType.BeginCloseTradeSymbol;
                                                    int.TryParse(subTimeEnd[0], out HourSecond);
                                                    int.TryParse(subTimeEnd[1], out MinuteSecond);
                                                    DateTimeEnd.Day = -1;
                                                    DateTimeEnd.DayInWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), subEvent[0], true);
                                                    DateTimeEnd.Hour = HourSecond;
                                                    DateTimeEnd.Minute = MinuteSecond;
                                                    TimeEventEnd.Time = DateTimeEnd;
                                                    TimeEventEnd.TimeEventID = ID;
                                                    TimeEventEnd.NumSession = j;

                                                    Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                    newTargetFunctionEnd.EventPosition = TargetPosition;
                                                    newTargetFunctionEnd.Function = this.CloseQuoteSymbol;
                                                    newTargetFunctionEnd.NumSession = j;

                                                    if (TimeEventEnd.TargetFunction == null)
                                                        TimeEventEnd.TargetFunction = new List<TargetFunction>();

                                                    TimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                    Result.Add(TimeEventEnd);
                                                    #endregion                                                    
                                                }
                                                break;

                                            case "C28":
                                                {
                                                    #region Add Trade Symbol Config
                                                    int.TryParse(subTimeStart[0], out HourFrist);
                                                    int.TryParse(subTimeStart[1], out MinuteFirst);
                                                    Business.TimeEvent TimeEventStart = new TimeEvent();
                                                    Business.DateTimeEvent TimeEvent = new DateTimeEvent();
                                                    TimeEventStart.EventType = TimeEventType.EndCloseMarket;
                                                    TimeEvent.Day = -1;
                                                    TimeEvent.DayInWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), subEvent[0], true);
                                                    TimeEvent.Hour = HourFrist;
                                                    TimeEvent.Minute = MinuteFirst;
                                                    TimeEvent.Month = -1;
                                                    TimeEventStart.Time = TimeEvent;
                                                    TimeEventStart.TimeEventID = ID;
                                                    TimeEventStart.NumSession = j;

                                                    Business.TargetFunction newTargetFunctionStart = new TargetFunction();
                                                    newTargetFunctionStart.EventPosition = TargetPosition;
                                                    newTargetFunctionStart.Function = this.OpenMarket;
                                                    newTargetFunctionStart.NumSession = j;

                                                    if (TimeEventStart.TargetFunction == null)
                                                        TimeEventStart.TargetFunction = new List<TargetFunction>();

                                                    TimeEventStart.TargetFunction.Add(newTargetFunctionStart);

                                                    Result.Add(TimeEventStart);

                                                    Business.TimeEvent TimeEventEnd = new TimeEvent();
                                                    Business.DateTimeEvent DateTimeEnd = new DateTimeEvent();
                                                    TimeEventEnd.EventType = TimeEventType.BeginCloseMarket;
                                                    int.TryParse(subTimeEnd[0], out HourSecond);
                                                    int.TryParse(subTimeEnd[1], out MinuteSecond);
                                                    DateTimeEnd.Day = -1;
                                                    DateTimeEnd.DayInWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), subEvent[0], true);
                                                    DateTimeEnd.Hour = HourSecond;
                                                    DateTimeEnd.Minute = MinuteSecond;
                                                    TimeEventEnd.Time = DateTimeEnd;
                                                    TimeEventEnd.TimeEventID = ID;
                                                    TimeEventEnd.NumSession = j;

                                                    Business.TargetFunction newTargetFunctionEnd = new TargetFunction();
                                                    newTargetFunctionEnd.EventPosition = TargetPosition;
                                                    newTargetFunctionEnd.Function = this.CloseMarket;
                                                    newTargetFunctionEnd.NumSession = j;

                                                    if (TimeEventEnd.TargetFunction == null)
                                                        TimeEventEnd.TargetFunction = new List<TargetFunction>();

                                                    TimeEventEnd.TargetFunction.Add(newTargetFunctionEnd);

                                                    Result.Add(TimeEventEnd);
                                                    #endregion                                                    
                                                }
                                                break;
                                        }
                                    }                                        
                                }
                            }
                        }
                    }
                }

                return Result;                              
            }

            return Result;
        }
        #endregion

        #region Get All Symbol In List Symbol Of Class Market And Return String Command To Client
        /// <summary>
        /// Get All Symbol In List Symbol Of Class Market And Return String Command To Client
        /// </summary>
        /// <returns></returns>
        internal List<string> SelectSymbolInSymbolList()
        {
            string CommandName = "SelectSymbol$";
            List<string> Result = new List<string>();
            if (Market.SymbolList != null && Market.SymbolList.Count > 0)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    string temp;
                    temp = Market.SymbolList[i].SymbolID.ToString() + "," + Market.SymbolList[i].SecurityID.ToString() + "," + "-1" + "," +
                            Market.SymbolList[i].MarketAreaRef.IMarketAreaID + "," + Market.SymbolList[i].Name + "," + Market.SymbolList[i].TickValue.Bid + "," +
                            Market.SymbolList[i].TickValue.Ask;
                    temp = CommandName + temp;
                    Result.Add(temp);

                    if (Market.SymbolList[i].RefSymbol != null && Market.SymbolList[i].RefSymbol.Count > 0)
                    {
                        List<string> listTemp = new List<string>();
                        listTemp = this.SelectSymbolReferenceInSymbolList(Market.SymbolList[i].RefSymbol, Market.SymbolList[i].SymbolID.ToString());
                        if (listTemp != null)
                        {
                            int countListTemp = listTemp.Count;
                            for (int j = 0; j < countListTemp; j++)
                            {
                                Result.Add(listTemp[j]);
                            }
                        }
                    }
                }
            }
            else
            {
                Result.Add(CommandName);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="RefSymbolID"></param>
        /// <returns></returns>
        internal List<string> SelectSymbolReferenceInSymbolList(List<Business.Symbol> ListSymbol, string RefSymbolID)
        {
            string CommandName = "SelectSymbol$";
            List<string> Result = new List<string>();
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    string temp = string.Empty;
                    temp = ListSymbol[i].SymbolID.ToString() + "," + ListSymbol[i].SecurityID.ToString() + "," + RefSymbolID + "," +
                                ListSymbol[i].MarketAreaRef.IMarketAreaID.ToString() + "," + ListSymbol[i].Name;
                    temp = CommandName + temp;
                    Result.Add(temp);

                    if (ListSymbol[i].RefSymbol != null && ListSymbol[i].RefSymbol.Count > 0)
                    {
                        List<string> listTemp = new List<string>();
                        listTemp = this.SelectSymbolReferenceInSymbolList(ListSymbol[i].RefSymbol, ListSymbol[i].SymbolID.ToString());
                        if (listTemp != null)
                        {
                            int countListTemp = listTemp.Count;
                            for (int j = 0; j < countListTemp; j++)
                            {
                                Result.Add(listTemp[j]);
                            }
                        }
                    }
                }
            }

            return Result;
        }
        #endregion

        #region Get Symbol By SymbolID In List Symbol Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal string SelectSymbolByIDInSymbolList(int SymbolID)
        {
            string result = string.Empty;
            if (Market.SymbolList != null)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SymbolList[i].SymbolID == SymbolID)
                    {
                        result += Market.SymbolList[i].SymbolID.ToString() + "," + Market.SymbolList[i].SecurityID.ToString() + "," + "-1," +
                                Market.SymbolList[i].MarketAreaRef.IMarketAreaID.ToString() + "," + Market.SymbolList[i].Name + "," +
                                Business.Market.SymbolList[i].TickValue.Bid + "," + Business.Market.SymbolList[i].TickValue.Ask;

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSymbol"></param>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        public string SelectSymbolByIDReferenceInSymbolList(Business.Symbol objSymbol, int SymbolID)
        {
            string result = string.Empty;
            if (objSymbol != null)
            {
                int count = objSymbol.RefSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (objSymbol.RefSymbol[i].SymbolID == SymbolID)
                    {
                        result = objSymbol.RefSymbol[i].SymbolID.ToString() + "," + objSymbol.RefSymbol[i].SecurityID.ToString() + "," + objSymbol.SymbolID.ToString() + "," +
                                    objSymbol.RefSymbol[i].MarketAreaRef.IMarketAreaID.ToString() + "," + objSymbol.RefSymbol[i].Name;

                        break;
                    }

                    if (objSymbol.RefSymbol[i].RefSymbol != null)
                    {
                        result += this.SelectSymbolByIDReferenceInSymbolList(objSymbol.RefSymbol[i], SymbolID);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Get All MarketArea In List MarketArea Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string ExtractSelectMarketArea()
        {
            string Result = string.Empty;
            if (Market.MarketArea != null)
            {
                int count = Market.MarketArea.Count;
                for (int i = 0; i < count; i++)
                {
                    Result += Market.MarketArea[i].IMarketAreaID + "," + Market.MarketArea[i].IMarketAreaName + "|";
                }
            }

            return Result;
        }
        #endregion

        #region Get Trading Config Of Symbol In Symbol List Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        internal string SelectTradingConfigBySymbolIDInSymbolList(int SymbolID)
        {
            bool Flag = false;
            string result = string.Empty;
            if (Market.SymbolList != null)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SymbolList[i].SymbolID == SymbolID)
                    {
                        if (Market.SymbolList[i].ParameterItems != null)
                        {
                            int countParameterItem = Market.SymbolList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameterItem; j++)
                            {
                                result += Market.SymbolList[i].ParameterItems[j].ParameterItemID.ToString() + "," +
                                            Market.SymbolList[i].ParameterItems[j].SecondParameterID.ToString() + "," + "-1" + "," +
                                            Market.SymbolList[i].ParameterItems[j].Code + "," +
                                            Market.SymbolList[i].ParameterItems[j].Name + "," +
                                            Market.SymbolList[i].ParameterItems[j].BoolValue.ToString() + "," +
                                            Market.SymbolList[i].ParameterItems[j].StringValue + "," +
                                            Market.SymbolList[i].ParameterItems[j].NumValue + "," +
                                            Market.SymbolList[i].ParameterItems[j].DateValue.ToString() + "|";
                            }
                        }
                        Flag = true;
                        break;
                    }

                    if (Flag == false)
                    {
                        if (Market.SymbolList[i].RefSymbol != null)
                        {
                            result += this.SelectTradingConfigBySymbolIDReferenceInSymbolList(Market.SymbolList[i].RefSymbol, SymbolID);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="SymbolID"></param>
        /// <returns></returns>
        public string SelectTradingConfigBySymbolIDReferenceInSymbolList(List<Business.Symbol> ListSymbol, int SymbolID)
        {
            bool Flag = false;
            string result = string.Empty;
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].SymbolID == SymbolID)
                    {
                        if (ListSymbol[i].ParameterItems != null)
                        {
                            int countParameterItem = ListSymbol[i].ParameterItems.Count;
                            for (int j = 0; j < countParameterItem; j++)
                            {
                                result += ListSymbol[i].ParameterItems[j].ParameterItemID.ToString() + "," +
                                            ListSymbol[i].ParameterItems[j].SecondParameterID.ToString() + "," + "-1" +
                                            ListSymbol[i].ParameterItems[j].Code + "," +
                                            ListSymbol[i].ParameterItems[j].Name + "," +
                                            ListSymbol[i].ParameterItems[j].BoolValue.ToString() + "," +
                                            ListSymbol[i].ParameterItems[j].StringValue + "," +
                                            ListSymbol[i].ParameterItems[j].NumValue + "," +
                                            ListSymbol[i].ParameterItems[j].DateValue.ToString() + "|";
                            }
                        }
                        Flag = true;
                        break;
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            result += this.SelectTradingConfigBySymbolIDReferenceInSymbolList(ListSymbol[i].RefSymbol, SymbolID);
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region Get IGroupSecurity Config By IGroupSecurityID In IGroupSecurity List Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal string GetIGroupSecurityConfigByIGroupSecurityIDInIGroupSecurityList(int SecurityID)
        {            
            string Result = string.Empty;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].IGroupSecurityID == SecurityID)
                    {
                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                        {
                            int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                Result += Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].ParameterItemID.ToString() + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].SecondParameterID.ToString() + "," + "-1" + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Name + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].BoolValue.ToString() + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].StringValue + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue + "," +
                                            Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].DateValue.ToString() + "|";
                            }                            
                        }
                        break;
                    }
                }
            }

            return Result;
        }        
        #endregion

        #region Get IGroupSymbol Config By IGroupSymbolID In IGroupSymbol List Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbolID"></param>
        /// <returns></returns>
        internal string GetIGroupSymbolConfigByIGroupSymbolIDInIGroupSymbolList(int IGroupSymbolID)
        {
            string result = string.Empty;
            if (Business.Market.IGroupSymbolList != null)
            {
                int count = Business.Market.IGroupSymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSymbolList[i].IGroupSymbolID == IGroupSymbolID)
                    {
                        if (Business.Market.IGroupSymbolList[i].IGroupSymbolConfig != null)
                        {
                            int countParameterItem = Business.Market.IGroupSymbolList[i].IGroupSymbolConfig.Count;
                            for (int j = 0; j < countParameterItem; j++)
                            {
                                result += Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].ParameterItemID.ToString() + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].SecondParameterID.ToString() + "," + "-1" + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Code + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].Name + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].BoolValue.ToString() + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].StringValue + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].NumValue + "," +
                                                Business.Market.IGroupSymbolList[i].IGroupSymbolConfig[j].DateValue.ToString() + "|";
                            }                            
                        }
                        break;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Get All Security In Security List Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string SelectSecurityInSecurityList()
        {
            string result = string.Empty;
            if (Market.SecurityList != null)
            {
                int count = Market.SecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    result += Market.SecurityList[i].SecurityID.ToString() + "," + Market.SecurityList[i].Name + "," + Market.SecurityList[i].Description + "," + Market.SecurityList[i].MarketAreaID + "|";
                }
            }

            return result;
        }
        #endregion

        #region Get Security By ID In Security List Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal string SelectSecurityByIDInSecurityList(int SecurityID)
        {
            string result = string.Empty;
            if (Market.SecurityList != null)
            {
                int count = Market.SecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SecurityList[i].SecurityID == SecurityID)
                    {
                        result = Market.SecurityList[i].SecurityID.ToString() + "," + Market.SecurityList[i].Name + "," + Market.SecurityList[i].Description + "," + Market.SecurityList[i].MarketAreaID;
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Get Security Config By SecurityID In Security List Of Class Market And Return String Command To Client
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal string SelectSecurityConfigBySecurityIDInSecurityList(int SecurityID)
        {
            string result = string.Empty;
            if (Market.SecurityList != null)
            {
                int count = Market.SecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SecurityList[i].SecurityID == SecurityID)
                    {
                        if (Market.SecurityList[i].ParameterItems != null)
                        {
                            int countParameterItem = Market.SecurityList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameterItem; j++)
                            {
                                result += Market.SecurityList[i].ParameterItems[j].ParameterItemID.ToString() + "," +
                                        Market.SecurityList[i].ParameterItems[j].SecondParameterID.ToString() + "," +
                                        Market.SecurityList[i].ParameterItems[j].Name + "," +
                                        Market.SecurityList[i].ParameterItems[j].Code + "," +
                                        Market.SecurityList[i].ParameterItems[j].NumValue + "," +
                                        Market.SecurityList[i].ParameterItems[j].BoolValue.ToString() + "," +
                                        Market.SecurityList[i].ParameterItems[j].StringValue + "," +
                                        Market.SecurityList[i].ParameterItems[j].DateValue.ToString() + "|";
                            }
                        }
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        //=========================================================================

        #region Function Static
        #region Find OpenTrade In Spot Command Of Symbol List In Class Market And Remove It(It Will Call Then Close Spot Command)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal bool FindAndRemoveOpenTradeInCommandList(int CommandID)
        {
            bool tempResult = false;
            Business.OpenTrade Result = new Business.OpenTrade();
            //Find In Symbol List And Remove Command
            if (Business.Market.SymbolList != null)
            {
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                {
                    if (Flag == true)
                        break;

                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                        for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j] != null)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == CommandID)
                                {
                                    #region COMMENT
                                    //Result.ClientCode = Business.Market.SymbolList[i].CommandList[j].ClientCode;
                                    //Result.ClosePrice = Business.Market.SymbolList[i].CommandList[j].ClosePrice;
                                    //Result.CloseTime = Business.Market.SymbolList[i].CommandList[j].CloseTime;
                                    //Result.CommandCode = Business.Market.SymbolList[i].CommandList[j].CommandCode;
                                    //Result.Commission = Business.Market.SymbolList[i].CommandList[j].Commission;
                                    //Result.ExpTime = Business.Market.SymbolList[i].CommandList[j].ExpTime;
                                    //Result.ID = Business.Market.SymbolList[i].CommandList[j].ID;
                                    //Result.IsClose = Business.Market.SymbolList[i].CommandList[j].IsClose;
                                    //Result.Margin = Business.Market.SymbolList[i].CommandList[j].Margin;
                                    //Result.OpenPrice = Business.Market.SymbolList[i].CommandList[j].OpenPrice;
                                    //Result.OpenTime = Business.Market.SymbolList[i].CommandList[j].OpenTime;
                                    //Result.Profit = Business.Market.SymbolList[i].CommandList[j].Profit;
                                    //Result.Size = Business.Market.SymbolList[i].CommandList[j].Size;
                                    //Result.StopLoss = Business.Market.SymbolList[i].CommandList[j].StopLoss;
                                    //Result.Swap = Business.Market.SymbolList[i].CommandList[j].Swap;
                                    //Result.Symbol = Business.Market.SymbolList[i].CommandList[j].Symbol;
                                    //Result.TakeProfit = Business.Market.SymbolList[i].CommandList[j].TakeProfit;
                                    //Result.Investor = Business.Market.SymbolList[i].CommandList[j].Investor;
                                    //Result.Type = Business.Market.SymbolList[i].CommandList[j].Type;
                                    //Result.IGroupSecurity = Business.Market.SymbolList[i].CommandList[j].IGroupSecurity;   
                                    #endregion

                                    //Business.Market.SymbolList[i].CommandList.RemoveAt(j);
                                    tempResult = Business.Market.SymbolList[i].CommandList.Remove(Business.Market.SymbolList[i].CommandList[j]);

                                    Flag = true;

                                    break;
                                }
                            }
                        }
                    }

                    if (Flag == false)
                    {
                        //if (Business.Market.SymbolList[i].RefSymbol != null)
                        //{
                        //    Result = this.FindAndRemoveOpenTradeInCommandListReference(Business.Market.SymbolList[i].RefSymbol, CommandID);
                        //}
                        TradingServer.Facade.FacadeAddNewSystemLog(1, CommandID.ToString(), "find command id in symbol list", "", "");
                    }
                }
            }

            return tempResult;
            //return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="OpenTradeID"></param>
        /// <returns></returns>
        private Business.OpenTrade FindAndRemoveOpenTradeInCommandListReference(List<Business.Symbol> ListSymbol, int OpenTradeID)
        {
            Business.OpenTrade Command = new OpenTrade();
            if (ListSymbol != null)
            {
                bool Flag = false;
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag == true)
                        break;

                    if (ListSymbol[i].CommandList != null)
                    {
                        int countOpenTrade = ListSymbol[i].CommandList.Count;
                        for (int j = 0; j < ListSymbol[i].CommandList.Count; j++)
                        {
                            if (ListSymbol[i].CommandList[j].ID == OpenTradeID)
                            {
                                Command.ClientCode = ListSymbol[i].CommandList[j].ClientCode;
                                Command.ClosePrice = ListSymbol[i].CommandList[j].ClosePrice;
                                Command.CloseTime = ListSymbol[i].CommandList[j].CloseTime;
                                Command.CommandCode = ListSymbol[i].CommandList[j].CommandCode;
                                Command.Commission = ListSymbol[i].CommandList[j].Commission;
                                Command.ExpTime = ListSymbol[i].CommandList[j].ExpTime;
                                Command.ID = ListSymbol[i].CommandList[j].ID;
                                Command.IsClose = ListSymbol[i].CommandList[j].IsClose;
                                Command.Margin = ListSymbol[i].CommandList[j].Margin;                                
                                Command.OpenPrice = ListSymbol[i].CommandList[j].OpenPrice;
                                Command.OpenTime = ListSymbol[i].CommandList[j].OpenTime;
                                Command.Profit = ListSymbol[i].CommandList[j].Profit;
                                Command.Size = ListSymbol[i].CommandList[j].Size;
                                Command.StopLoss = ListSymbol[i].CommandList[j].StopLoss;
                                Command.Swap = ListSymbol[i].CommandList[j].Swap;
                                Command.Symbol = ListSymbol[i].CommandList[j].Symbol;
                                Command.TakeProfit = ListSymbol[i].CommandList[j].TakeProfit;
                                Command.Investor = ListSymbol[i].CommandList[j].Investor;
                                Command.Type = ListSymbol[i].CommandList[j].Type;
                                Command.IGroupSecurity = ListSymbol[i].CommandList[j].IGroupSecurity;

                                ListSymbol[i].CommandList.Remove(ListSymbol[i].CommandList[j]);
                                j--;
                                Flag = true;
                                break;
                            }
                        }
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            Command = this.FindAndRemoveOpenTradeInCommandListReference(ListSymbol[i].RefSymbol, OpenTradeID);
                        }
                    }
                }
            }

            return Command;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal bool FindAndRemoveOpenTradeInCommandExecutor(int CommandID)
        {
            bool result = false;
            if (Business.Market.CommandExecutor != null && Business.Market.CommandExecutor.Count > 0)
            {
                int count = Business.Market.CommandExecutor.Count;
                bool flag = false;
                for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                {                    
                    if (Business.Market.CommandExecutor[i] != null)
                    {
                        if (Business.Market.CommandExecutor[i].ID == CommandID)
                        {
                            result = Business.Market.CommandExecutor.Remove(Business.Market.CommandExecutor[i]);
                            flag = true;
                            break;
                        }
                    }
                }

                if(!flag)
                    TradingServer.Facade.FacadeAddNewSystemLog(1, CommandID.ToString(), "find command id", "", "");
            }

            return result;
        }

        #region Find OpenTrade In Binary Command Of BinaryCommand List In Class Market And Remove It(It Will Call Then Cancel BinaryCommand)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade FindAndRemoveOpenTradeInBinaryCommandList(int CommandID)
        {
            Business.OpenTrade Result = new Business.OpenTrade();
            //Find In Symbol List And Remove Command
            if (Business.Market.SymbolList != null)
            {
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag == false)
                    {
                        if (Business.Market.SymbolList[i].BinaryCommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].BinaryCommandList.Count;
                            for (int j = 0; j < Business.Market.SymbolList[i].BinaryCommandList.Count; j++)
                            {
                                if (Business.Market.SymbolList[i].BinaryCommandList[j].ID == CommandID)
                                {
                                    Result.ClientCode = Business.Market.SymbolList[i].BinaryCommandList[j].ClientCode;
                                    Result.ClosePrice = Business.Market.SymbolList[i].BinaryCommandList[j].ClosePrice;
                                    Result.CloseTime = Business.Market.SymbolList[i].BinaryCommandList[j].CloseTime;
                                    Result.CommandCode = Business.Market.SymbolList[i].BinaryCommandList[j].CommandCode;
                                    Result.Commission = Business.Market.SymbolList[i].BinaryCommandList[j].Commission;
                                    Result.ExpTime = Business.Market.SymbolList[i].BinaryCommandList[j].ExpTime;
                                    Result.ID = Business.Market.SymbolList[i].BinaryCommandList[j].ID;
                                    Result.IsClose = Business.Market.SymbolList[i].BinaryCommandList[j].IsClose;
                                    Result.Margin = Business.Market.SymbolList[i].BinaryCommandList[j].Margin;                                    
                                    Result.OpenPrice = Business.Market.SymbolList[i].BinaryCommandList[j].OpenPrice;
                                    Result.OpenTime = Business.Market.SymbolList[i].BinaryCommandList[j].OpenTime;
                                    Result.Profit = Business.Market.SymbolList[i].BinaryCommandList[j].Profit;
                                    Result.Size = Business.Market.SymbolList[i].BinaryCommandList[j].Size;
                                    Result.StopLoss = Business.Market.SymbolList[i].BinaryCommandList[j].StopLoss;
                                    Result.Swap = Business.Market.SymbolList[i].BinaryCommandList[j].Swap;
                                    Result.Symbol = Business.Market.SymbolList[i].BinaryCommandList[j].Symbol;
                                    Result.TakeProfit = Business.Market.SymbolList[i].BinaryCommandList[j].TakeProfit;
                                    Result.Investor = Business.Market.SymbolList[i].BinaryCommandList[j].Investor;
                                    Result.Type = Business.Market.SymbolList[i].BinaryCommandList[j].Type;

                                    if (Business.Market.SymbolList[i].BinaryCommandList.Count > 0)
                                    {
                                        Business.Market.SymbolList[i].BinaryCommandList.RemoveAt(j);
                                        j--;
                                    }

                                    Flag = true;
                                    break;
                                }
                            }
                        }

                        if (Flag == false)
                        {
                            if (Business.Market.SymbolList[i].RefSymbol != null && Business.Market.SymbolList[i].RefSymbol.Count > 0)
                            {
                                Result = this.FindAndRemoveOpenTradeInCommandListReference(Business.Market.SymbolList[i].RefSymbol, CommandID);
                            }
                            else
                            {
                                bool IsBuy = false;
                                //if (Result.Type.ID == 3)
                                //    IsBuy = true;

                                if (Result.Investor != null)
                                {
                                    if (Result.Investor.ClientBinaryQueue == null)
                                        Result.Investor.ClientBinaryQueue = new List<string>();

                                    string Message = "CancelBinary$False,Can't Find Symbol," + Result.ID + "," + Result.Investor.InvestorID + "," + Result.Symbol.Name + "," +
                                        Result.Size + "," + IsBuy + "," + Result.OpenTime + "," + Result.OpenPrice + "," + Result.StopLoss + "," + Result.TakeProfit + "," +
                                        Result.ClosePrice + "," + Result.Commission + "," + Result.Swap + "," + Result.Profit + "," + "Comment," + Result.ID + "," + "BinaryTrading" + "," +
                                        1 + "," + Result.ExpTime + "," + Result.ClientCode + "," + Result.CommandCode + "," + Result.IsHedged + "," + Result.Type.ID + "," + Result.Type.ID + ",Binary";

                                    Result.Investor.ClientBinaryQueue.Add(Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="OpenTradeID"></param>
        /// <returns></returns>
        private Business.OpenTrade FindAndRemoveOpenTradeInBinaryCommandListReference(List<Business.Symbol> ListSymbol, int OpenTradeID)
        {
            Business.OpenTrade Result = new OpenTrade();
            if (ListSymbol != null)
            {
                bool Flag = false;
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].BinaryCommandList != null)
                    {
                        int countOpenTrade = ListSymbol[i].BinaryCommandList.Count;
                        for (int j = 0; j < ListSymbol[i].BinaryCommandList.Count; j++)
                        {
                            if (ListSymbol[i].BinaryCommandList[j].ID == OpenTradeID)
                            {
                                Result.ClientCode = ListSymbol[i].BinaryCommandList[j].ClientCode;
                                Result.ClosePrice = ListSymbol[i].BinaryCommandList[j].ClosePrice;
                                Result.CloseTime = ListSymbol[i].BinaryCommandList[j].CloseTime;
                                Result.CommandCode = ListSymbol[i].BinaryCommandList[j].CommandCode;
                                Result.Commission = ListSymbol[i].BinaryCommandList[j].Commission;
                                Result.ExpTime = ListSymbol[i].BinaryCommandList[j].ExpTime;
                                Result.ID = ListSymbol[i].BinaryCommandList[j].ID;
                                Result.IsClose = ListSymbol[i].BinaryCommandList[j].IsClose;
                                Result.Margin = ListSymbol[i].BinaryCommandList[j].Margin;                                
                                Result.OpenPrice = ListSymbol[i].BinaryCommandList[j].OpenPrice;
                                Result.OpenTime = ListSymbol[i].BinaryCommandList[j].OpenTime;
                                Result.Profit = ListSymbol[i].BinaryCommandList[j].Profit;
                                Result.Size = ListSymbol[i].BinaryCommandList[j].Size;
                                Result.StopLoss = ListSymbol[i].BinaryCommandList[j].StopLoss;
                                Result.Swap = ListSymbol[i].BinaryCommandList[j].Swap;
                                Result.Symbol = ListSymbol[i].BinaryCommandList[j].Symbol;
                                Result.TakeProfit = ListSymbol[i].BinaryCommandList[j].TakeProfit;
                                Result.Investor = ListSymbol[i].BinaryCommandList[j].Investor;
                                Result.Type = ListSymbol[i].BinaryCommandList[j].Type;

                                if (ListSymbol[i].BinaryCommandList.Count > 0)
                                {
                                    ListSymbol[i].BinaryCommandList.RemoveAt(j);
                                }

                                Flag = true;
                                break;
                            }
                        }
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            Result = this.FindAndRemoveOpenTradeInCommandListReference(ListSymbol[i].RefSymbol, OpenTradeID);
                        }
                        else
                        {
                            bool IsBuy = false;
                            if (Result.Type.ID == 3)
                                IsBuy = true;

                            if (Result.Investor.ClientBinaryQueue == null)
                                Result.Investor.ClientBinaryQueue = new List<string>();

                            string Message = "CancelBinary$False,Can't Find Symbol," + Result.ID + "," + Result.Investor.InvestorID + "," + Result.Symbol.Name + "," +
                                Result.Size + "," + IsBuy + "," + Result.OpenTime + "," + Result.OpenPrice + "," + Result.StopLoss + "," + Result.TakeProfit + "," +
                                Result.ClosePrice + "," + Result.Commission + "," + Result.Swap + "," + Result.Profit + "," + "Comment," + Result.ID + "," + "BinaryTrading" + "," +
                                1 + "," + Result.ExpTime + "," + Result.ClientCode + "," + Result.CommandCode + "," + Result.IsHedged + "," + Result.Type.ID + "," + Result.Margin + ",Binary";

                            Result.Investor.ClientBinaryQueue.Add(Message);
                        }
                    }
                }
            }

            return Result;
        }
        #endregion
        #endregion

        #region Find Symbol In Symbol List Of Class Market And Fill Symbol ,Type,Investor To OpenTrade
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade FindOpenTradeInSymbolList(int CommandID)
        {
            Business.OpenTrade Result = new OpenTrade();
            if (Business.Market.SymbolList != null)
            {                
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;
                    
                    if (Business.Market.SymbolList[i].CommandList != null)
                    {                        
                        for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j] != null)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == CommandID)
                                {
                                    //Result = this.ExtractCommand(Business.Market.SymbolList[i].CommandList[j]);
                                    Result = Business.Market.SymbolList[i].CommandList[j];

                                    //Result.AgentCommission = Business.Market.SymbolList[i].CommandList[j].AgentCommission;
                                    //Result.ClientCode = Business.Market.SymbolList[i].CommandList[j].ClientCode;
                                    //Result.ClosePrice = Business.Market.SymbolList[i].CommandList[j].ClosePrice;
                                    //Result.CloseTime = Business.Market.SymbolList[i].CommandList[j].CloseTime;
                                    //Result.CommandCode = Business.Market.SymbolList[i].CommandList[j].CommandCode;
                                    //Result.Comment = Business.Market.SymbolList[i].CommandList[j].Comment;
                                    //Result.Commission = Business.Market.SymbolList[i].CommandList[j].Commission;
                                    //Result.ExpTime = Business.Market.SymbolList[i].CommandList[j].ExpTime;
                                    //Result.FreezeMargin = Business.Market.SymbolList[i].CommandList[j].FreezeMargin;
                                    //Result.ID = Business.Market.SymbolList[i].CommandList[j].ID;
                                    //Result.IGroupSecurity = Business.Market.SymbolList[i].CommandList[j].IGroupSecurity;
                                    //Result.InProcessClose = Business.Market.SymbolList[i].CommandList[j].InProcessClose;
                                    //Result.Investor = Business.Market.SymbolList[i].CommandList[j].Investor;
                                    //Result.IsClose = Business.Market.SymbolList[i].CommandList[j].IsClose;
                                    //Result.IsHedged = Business.Market.SymbolList[i].CommandList[j].IsHedged;
                                    //Result.IsMultiClose = Business.Market.SymbolList[i].CommandList[j].IsMultiClose;
                                    //Result.IsMultiUpdate = Business.Market.SymbolList[i].CommandList[j].IsMultiUpdate;
                                    //Result.IsProcess = Business.Market.SymbolList[i].CommandList[j].IsProcess;
                                    //Result.IsProcessStatus = Business.Market.SymbolList[i].CommandList[j].IsProcessStatus;
                                    //Result.IsReOpen = Business.Market.SymbolList[i].CommandList[j].IsReOpen;
                                    //Result.IsServer = Business.Market.SymbolList[i].CommandList[j].IsServer;
                                    //Result.ManagerID = Business.Market.SymbolList[i].CommandList[j].ManagerID;
                                    //Result.Margin = Business.Market.SymbolList[i].CommandList[j].Margin;
                                    //Result.MaxDev = Business.Market.SymbolList[i].CommandList[j].MaxDev;
                                    //Result.NumberUpdate = Business.Market.SymbolList[i].CommandList[j].NumberUpdate;
                                    //Result.OpenPrice = Business.Market.SymbolList[i].CommandList[j].OpenPrice;
                                    //Result.OpenTime = Business.Market.SymbolList[i].CommandList[j].OpenTime;
                                    //Result.Profit = Business.Market.SymbolList[i].CommandList[j].Profit;
                                    //Result.Size = Business.Market.SymbolList[i].CommandList[j].Size;
                                    //Result.SpreaDifferenceInOpenTrade = Business.Market.SymbolList[i].CommandList[j].SpreaDifferenceInOpenTrade;
                                    //Result.StopLoss = Business.Market.SymbolList[i].CommandList[j].StopLoss;
                                    //Result.Swap = Business.Market.SymbolList[i].CommandList[j].Swap;
                                    //Result.Symbol = Business.Market.SymbolList[i].CommandList[j].Symbol;
                                    //Result.TakeProfit = Business.Market.SymbolList[i].CommandList[j].TakeProfit;
                                    //Result.Taxes = Business.Market.SymbolList[i].CommandList[j].Taxes;
                                    //Result.TotalSwap = Business.Market.SymbolList[i].CommandList[j].TotalSwap;
                                    //Result.Type = Business.Market.SymbolList[i].CommandList[j].Type;
                                    //Result.RefCommandID = Business.Market.SymbolList[i].CommandList[j].RefCommandID;
                                    //Result.AgentRefConfig = Business.Market.SymbolList[i].CommandList[j].AgentRefConfig;
                                   
                                    Flag = true;
                                    break;
                                }
                            }                         
                        }
                    }
                }
            }

            if (Result == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(5, "Find command in symbol list: " + CommandID, "", "", "");
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade FindOpenTradeInSymbolListByRefID(int RefCommandID)
        {
            Business.OpenTrade Result = new OpenTrade();
            if (Business.Market.SymbolList != null)
            {
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;

                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j] != null)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].RefCommandID == RefCommandID)
                                {   
                                    Result.AgentCommission = Business.Market.SymbolList[i].CommandList[j].AgentCommission;
                                    Result.ClientCode = Business.Market.SymbolList[i].CommandList[j].ClientCode;
                                    Result.ClosePrice = Business.Market.SymbolList[i].CommandList[j].ClosePrice;
                                    Result.CloseTime = Business.Market.SymbolList[i].CommandList[j].CloseTime;
                                    Result.CommandCode = Business.Market.SymbolList[i].CommandList[j].CommandCode;
                                    Result.Comment = Business.Market.SymbolList[i].CommandList[j].Comment;
                                    Result.Commission = Business.Market.SymbolList[i].CommandList[j].Commission;
                                    Result.ExpTime = Business.Market.SymbolList[i].CommandList[j].ExpTime;
                                    Result.FreezeMargin = Business.Market.SymbolList[i].CommandList[j].FreezeMargin;
                                    Result.ID = Business.Market.SymbolList[i].CommandList[j].ID;
                                    Result.IGroupSecurity = Business.Market.SymbolList[i].CommandList[j].IGroupSecurity;
                                    Result.InProcessClose = Business.Market.SymbolList[i].CommandList[j].InProcessClose;
                                    Result.Investor = Business.Market.SymbolList[i].CommandList[j].Investor;
                                    Result.IsClose = Business.Market.SymbolList[i].CommandList[j].IsClose;
                                    Result.IsHedged = Business.Market.SymbolList[i].CommandList[j].IsHedged;
                                    Result.IsMultiClose = Business.Market.SymbolList[i].CommandList[j].IsMultiClose;
                                    Result.IsMultiUpdate = Business.Market.SymbolList[i].CommandList[j].IsMultiUpdate;
                                    Result.IsProcess = Business.Market.SymbolList[i].CommandList[j].IsProcess;
                                    Result.IsProcessStatus = Business.Market.SymbolList[i].CommandList[j].IsProcessStatus;
                                    Result.IsReOpen = Business.Market.SymbolList[i].CommandList[j].IsReOpen;
                                    Result.IsServer = Business.Market.SymbolList[i].CommandList[j].IsServer;
                                    Result.ManagerID = Business.Market.SymbolList[i].CommandList[j].ManagerID;
                                    Result.Margin = Business.Market.SymbolList[i].CommandList[j].Margin;
                                    Result.MaxDev = Business.Market.SymbolList[i].CommandList[j].MaxDev;
                                    Result.NumberUpdate = Business.Market.SymbolList[i].CommandList[j].NumberUpdate;
                                    Result.OpenPrice = Business.Market.SymbolList[i].CommandList[j].OpenPrice;
                                    Result.OpenTime = Business.Market.SymbolList[i].CommandList[j].OpenTime;
                                    Result.Profit = Business.Market.SymbolList[i].CommandList[j].Profit;
                                    Result.Size = Business.Market.SymbolList[i].CommandList[j].Size;
                                    Result.SpreaDifferenceInOpenTrade = Business.Market.SymbolList[i].CommandList[j].SpreaDifferenceInOpenTrade;
                                    Result.StopLoss = Business.Market.SymbolList[i].CommandList[j].StopLoss;
                                    Result.Swap = Business.Market.SymbolList[i].CommandList[j].Swap;
                                    Result.Symbol = Business.Market.SymbolList[i].CommandList[j].Symbol;
                                    Result.TakeProfit = Business.Market.SymbolList[i].CommandList[j].TakeProfit;
                                    Result.Taxes = Business.Market.SymbolList[i].CommandList[j].Taxes;
                                    Result.TotalSwap = Business.Market.SymbolList[i].CommandList[j].TotalSwap;
                                    Result.Type = Business.Market.SymbolList[i].CommandList[j].Type;
                                    Result.RefCommandID = Business.Market.SymbolList[i].CommandList[j].RefCommandID;
                                    Result.InsExe = Business.Market.SymbolList[i].CommandList[j].InsExe;

                                    Flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (Result == null)
                TradingServer.Facade.FacadeAddNewSystemLog(5, "Find command in symbol list: " + RefCommandID, "", "", "");

            return Result;
        }
        #endregion

        #region EXTRACT COMMAND WITH PROPERTY
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openPosition"></param>
        /// <returns></returns>
        internal Business.OpenTrade ExtractCommand(Business.OpenTrade openPosition)
        {
            Business.OpenTrade newOpenPosition = new OpenTrade();
            if (openPosition != null)
            {
                newOpenPosition.AgentCommission = openPosition.AgentCommission;
                newOpenPosition.ClientCode = openPosition.ClientCode;
                newOpenPosition.ClosePrice = openPosition.ClosePrice;
                newOpenPosition.CloseTime = openPosition.CloseTime;
                newOpenPosition.CommandCode = openPosition.CommandCode;
                newOpenPosition.Comment = openPosition.Comment;
                newOpenPosition.Commission = openPosition.Commission;
                newOpenPosition.ExpTime = openPosition.ExpTime;
                newOpenPosition.FreezeMargin = openPosition.FreezeMargin;
                newOpenPosition.ID = openPosition.ID;
                newOpenPosition.IGroupSecurity = openPosition.IGroupSecurity;
                newOpenPosition.Investor = openPosition.Investor;
                newOpenPosition.IsClose = openPosition.IsClose;
                newOpenPosition.IsHedged = openPosition.IsHedged;
                newOpenPosition.IsMultiClose = openPosition.IsMultiClose;
                newOpenPosition.IsMultiUpdate = openPosition.IsMultiUpdate;
                newOpenPosition.IsProcess = openPosition.IsProcess;
                newOpenPosition.IsServer = openPosition.IsServer;
                newOpenPosition.ManagerID = openPosition.ManagerID;
                newOpenPosition.Margin = openPosition.Margin;
                newOpenPosition.MaxDev = openPosition.MaxDev;
                newOpenPosition.NumberUpdate = openPosition.NumberUpdate;
                newOpenPosition.OpenPrice = openPosition.OpenPrice;
                newOpenPosition.OpenTime = openPosition.OpenTime;
                newOpenPosition.Profit = openPosition.Profit;
                newOpenPosition.Size = openPosition.Size;
                newOpenPosition.SpreaDifferenceInOpenTrade = openPosition.SpreaDifferenceInOpenTrade;
                newOpenPosition.StopLoss = openPosition.StopLoss;
                newOpenPosition.Swap = openPosition.Swap;
                newOpenPosition.Symbol = openPosition.Symbol;
                newOpenPosition.TakeProfit = openPosition.TakeProfit;
                newOpenPosition.Taxes = openPosition.Taxes;
                newOpenPosition.Type = openPosition.Type;
            }

            return newOpenPosition;
        }
        #endregion

        #region Find Symbol In Symbol List Of Class Market And Fill Symbol,Type,Investor To OpenTrade Reference
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        private Business.OpenTrade FindOpenTradeReferenceInSymbolList(List<Business.Symbol> ListSymbol, int CommandID)
        {
            Business.OpenTrade Result = new OpenTrade();
            if (ListSymbol != null)
            {
                bool Flag = false;
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;
                    
                    if (ListSymbol[i].CommandList != null && ListSymbol[i].CommandList.Count > 0)
                    {
                        int countCommand = ListSymbol[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (ListSymbol[i].CommandList[j].ID == CommandID)
                            {
                                Result = ListSymbol[i].CommandList[j];
                                Flag = true;
                                break;
                            }
                        }
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null && ListSymbol[i].RefSymbol.Count > 0)
                        {
                            Result = this.FindOpenTradeReferenceInSymbolList(ListSymbol[i].RefSymbol, CommandID);
                        }
                    }
                }
            }

            return Result;
        }
        #endregion

        #region FIND COMMAND IN COMMAND EXECUTOR
        internal Business.OpenTrade FindOpenTradeInCommandExe(int commandID)
        {
            Business.OpenTrade result = new OpenTrade();
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == commandID)
                    {
                        result = Business.Market.CommandExecutor[i];
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        #region GET MAIL CONFIG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="isEnable"></param>
        /// <param name="smtpServer"></param>
        /// <param name="smtpLogin"></param>
        /// <param name="smtpPassword"></param>
        internal Model.MailConfig GetMailConfig(Business.Investor objInvestor)
        {
            Model.MailConfig result = new Model.MailConfig();

            #region GET PARAMETER SEND MAIL IN GROUP OF INVESTOR
            bool isEnable = false;
            string smtpServer = string.Empty;
            string smtpLogin = string.Empty;
            string smtpPassword = string.Empty;
            string supportEmail = string.Empty;
            string signature = string.Empty;

            if (objInvestor.InvestorGroupInstance != null)
            {
                if (objInvestor.InvestorGroupInstance.ParameterItems != null)
                {
                    int countParameter = objInvestor.InvestorGroupInstance.ParameterItems.Count;
                    for (int j = 0; j < countParameter; j++)
                    {
                        if (objInvestor.InvestorGroupInstance.ParameterItems[j].Code == "G29")
                        {
                            if (objInvestor.InvestorGroupInstance.ParameterItems[j].BoolValue == 1)
                                isEnable = true;
                        }

                        if (objInvestor.InvestorGroupInstance.ParameterItems[j].Code == "G30")
                        {
                            smtpServer = objInvestor.InvestorGroupInstance.ParameterItems[j].StringValue;
                        }

                        if (objInvestor.InvestorGroupInstance.ParameterItems[j].Code == "G32")
                        {
                            smtpLogin = objInvestor.InvestorGroupInstance.ParameterItems[j].StringValue;
                        }

                        if (objInvestor.InvestorGroupInstance.ParameterItems[j].Code == "G33")
                        {
                            smtpPassword = objInvestor.InvestorGroupInstance.ParameterItems[j].StringValue;
                        }

                        if (objInvestor.InvestorGroupInstance.ParameterItems[j].Code == "G34")
                        {
                            supportEmail = objInvestor.InvestorGroupInstance.ParameterItems[j].StringValue;
                        }

                        if (objInvestor.InvestorGroupInstance.ParameterItems[j].Code == "G35")
                        {
                            signature = objInvestor.InvestorGroupInstance.ParameterItems[j].StringValue;
                        }
                    }
                }
            }
            #endregion

            #region SET MAIL CONFIG
            result.isEnable = isEnable;
            result.DisplayNameFrom = "Element 5";
            result.MessageFrom = smtpLogin;
            result.PasswordCredential = smtpPassword;
            result.SmtpHost = smtpServer;
            result.UserCredential = smtpLogin;
            result.Signature = signature;
            #endregion

            return result;
        }
        #endregion       

        #region Find Open Trade Investor List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        internal Business.OpenTrade FindOpenTradeInInvestorList(int CommandID)
        {
            Business.OpenTrade Result = new OpenTrade();
            if (Business.Market.InvestorList != null)
            {
                bool Flag = false;
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;

                    if (Business.Market.InvestorList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.InvestorList[i].CommandList[j].ID == CommandID)
                            {
                                //Business.Market.InvestorList[i].CommandList[j].InProcessClose = true;
                                Result = Business.Market.InvestorList[i].CommandList[j];
                                Flag = true;
                                break;
                            }
                        }
                    }
                }
            }

            return Result;
        }
        #endregion

        #region FIND COMMAND IN INVESTOR LIST AND UPDATE SWAP IN COMMAND
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Swaps"></param>
        /// <returns></returns>
        internal bool FindAndUpdateSwapInInvestorList(int CommandID, int InvestorID, double Swaps,double totalSwaps)
        {
            bool Result = false;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        if (Business.Market.InvestorList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].ID == CommandID)
                                {
                                    Business.Market.InvestorList[i].CommandList[j].Swap = Swaps;
                                    Business.Market.InvestorList[i].CommandList[j].TotalSwap = totalSwaps;
                                    Result = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            return Result;
        }
        #endregion

        #region FIND COMMAND IN SYMBOL LIST AND UPDATE SWAP IN COMMAND
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandID"></param>
        /// <param name="SymbolName"></param>
        /// <param name="Swaps"></param>
        /// <returns></returns>
        internal bool FindAndUpdateSwapInSymbolList(int CommandID, string SymbolName, double Swaps,double totalSwaps)
        {
            bool Result = false;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].Name == SymbolName)
                    {
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == CommandID)
                                {
                                    Business.Market.SymbolList[i].CommandList[j].Swap = Swaps;
                                    Business.Market.SymbolList[i].CommandList[j].TotalSwap = totalSwaps;
                                    Result = true;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            return Result;
        }
        #endregion
    }
}