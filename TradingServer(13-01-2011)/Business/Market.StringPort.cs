using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// EXTRACT COMMAND AND CALL FUNCTION 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        public string ExtractCommandServer(string Cmd, string ipAddress, string code)
        {
            string StringResult = string.Empty;
            string Command = string.Empty;
            string Value = string.Empty;
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subValue = new string[2];

                int Position = -1;
                Position = Cmd.IndexOf('$');
                if (Position > 0)
                {
                    Command = Cmd.Substring(0, Position);
                    Value = Cmd.Substring(Position + 1);

                    subValue[0] = Command;
                    subValue[1] = Value;
                }
                else
                {
                    subValue[0] = Cmd;
                }

                if (subValue.Length > 0)
                {
                    switch (subValue[0])
                    {
                        //Command Add New
                        //
                        #region Add Investor Group(LOG)
                        case "AddInvestorGroup":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int ResultAddNew = -1;
                                    Business.InvestorGroup Result = new InvestorGroup();
                                    Result = this.ExtractInvestorGroup(subValue[1]);
                                    //Call Function Add New Investor Group
                                    ResultAddNew = TradingServer.Facade.FacadeAddNewInvestorGroup(Result);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': group config added/changed ['test-duc']
                                    string status = "[Failed]";

                                    if (ResultAddNew > 0)
                                    {
                                        status = "[successs]";
                                        TradingServer.Facade.FacadeCheckUpdateGroupVirtualDealerOnline();
                                    }

                                    string content = "'" + code + "': group config added/changed ['" + Result.Name + "'] " + status;
                                    string comment = "[add new group]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion

                                    if (ResultAddNew > 0) TradingServer.Facade.FacadeSendNoticeManagerChangeGroup(3, ResultAddNew);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add Investor Group Config
                        case "AddInvestorGroupConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int ResultAddNew = -1;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);
                                    //Call Function Add New Investor Group Config
                                    ResultAddNew = TradingServer.Facade.FacadeAddNewInvestorGroupConfig(Result);
                                    if (ResultAddNew > 0) Facade.FacadeSendNoticeManagerChangeGroup(2, Result[0].SecondParameterID);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();

                                    //SEND COMMAND TO AGENT SERVER
                                    string strAgent = "AddInvestorGroup$" + Result[0].SecondParameterID;
                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = strAgent;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add New Security(LOG)
                        case "AddSecurity":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        int ResultAddNew = -1;
                                        int MarketAreaID = -1;
                                        string[] subParameter = subValue[1].Split(',');
                                        int.TryParse(subParameter[2], out MarketAreaID);
                                        if (subParameter.Length > 0)
                                        {
                                            ResultAddNew = TradingServer.Facade.FacadeAddSecurity(subParameter[0], subParameter[1], MarketAreaID);
                                        }

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': security config added/changed ['test-duc']
                                        string status = "[Failed]";

                                        if (ResultAddNew > 0)
                                            status = "[Success]";

                                        string content = "'" + code + "': security config added/changed ['" + subParameter[0] + "'] " + status;
                                        string comment = "[add new security]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion

                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add New SecurityConfig
                        case "AddSecurityConfig":
                            {
                                int ResultAddNew = -1;
                                List<Business.ParameterItem> Result = new List<ParameterItem>();
                                Result = this.ExtractParameterItem(subValue[1]);
                                ResultAddNew = TradingServer.Facade.FacadeAddSecurityConfig(Result);
                                StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                            }
                            break;
                        #endregion

                        #region Add New Symbol(LOG)
                        case "AddNewSymbol":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        int ResultAddNew = -1;
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length > 0)
                                        {
                                            int SecurityID = 0;
                                            int RefSymbolID = 0;
                                            int MarketAreaID = 0;

                                            int.TryParse(subParameter[0], out SecurityID);
                                            int.TryParse(subParameter[1], out RefSymbolID);
                                            int.TryParse(subParameter[2], out MarketAreaID);

                                            //Call Function Add New Symbol
                                            ResultAddNew = TradingServer.Facade.FacadeAddNewSymbol(SecurityID, RefSymbolID, MarketAreaID, subParameter[3]);

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            //'2222': symbol config added/changed ['XAUUSD']
                                            string status = "[Failed]";

                                            if (ResultAddNew > 0)
                                            {
                                                status = "[Success]";
                                                TradingServer.Facade.FacadeCheckUpdateGroupVirtualDealerOnline();
                                            }

                                            string content = "'" + code + "': symbol config added/changed ['" + subParameter[3] + "'] " + status;
                                            string comment = "[add new security]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion

                                            if (ResultAddNew > 0) Facade.FacadeSendNoticeManagerChangeSymbol(3, ResultAddNew);
                                            StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add New TradingConfig(SymbolConfig)
                        case "AddTradingConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int ResultAddNew = -1;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);
                                    //Call Function Add New TradingConfig(SymbolConfig)
                                    ResultAddNew = TradingServer.Facade.FacadeAddNewTradingConfig(Result);
                                    if (ResultAddNew > 0) Facade.FacadeSendNoticeManagerChangeSymbol(2, Result[0].SecondParameterID);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    
                                    //SEND COMMAND TO AGENT SERVER
                                    string strAgent = "AddNewSymbol$" + Result[0].SecondParameterID;
                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                    newAgentNotify.NotifyMessage = strAgent;
                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add New Investor(LOG)
                        case "AddNewInvestor":
                            {
                                int ResultAddNew = -1;
                                Business.Investor Result = new Investor();
                                Result = this.ExtractionInvestor(subValue[1]);

                                //Check Email Add Send Mail Confirm Create Account Complete
                                bool checkEmail = Model.TradingCalculate.Instance.IsEmail(Result.Email);

                                if (checkEmail)
                                {
                                    bool CheckCode = false;
                                    CheckCode = TradingServer.Facade.FacadeGetInvestorByCode(Result.Code);
                                    if (CheckCode == true)
                                    {
                                        double balance = Result.Balance;
                                        Result.Balance = 0;
                                        bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                        if (checkip)
                                        {
                                            bool checkRule = Facade.FacadeCheckPermitAccountManagerAndAdmin(code);
                                            bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, Result.InvestorGroupInstance.InvestorGroupID);
                                            if (checkRule && checkGroup)
                                            {
                                                ResultAddNew = TradingServer.Facade.FacadeAddNewInvestor(Result);

                                                //Add Investor To Investor List
                                                if (ResultAddNew > 0)
                                                {
                                                    Result.InvestorID = ResultAddNew;
                                                    int resultProfileID = TradingServer.Facade.FacadeAddInvestorProfile(Result);
                                                    Result.InvestorProfileID = resultProfileID;
                                                    if (string.IsNullOrEmpty(Result.AgentID))
                                                        Result.AgentID = "0";

                                                    Business.Market.InvestorList.Add(Result);

                                                    //Deposit account
                                                    Result.Deposit(balance, ResultAddNew, "deposit");
                                                }

                                                StringResult = subValue[0] + "$" + ResultAddNew.ToString() + "," + Result.Code;

                                                #region INSERT SYSTEM LOG
                                                //INSERT SYSTEM LOG
                                                //'2222': new account '9942881' - ngthanhduc
                                                string status = "[Failed]";

                                                if (ResultAddNew > 0)
                                                    status = "[Success]";

                                                string content = "'" + code + "': new account '" + Result.Code + "' - " + Result.NickName + " " + status;
                                                string comment = "[add new account]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                                #endregion
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$MCM006";
                                                string content = "'" + code + "': new account '" + Result.Code + "' - " + Result.NickName + " failed(not enough rights)";
                                                string comment = "[add new account]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            }
                                            //SEND NOTIFY TO MANAGER
                                            //TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Result);

                                            //TradingServer.Facade.FacadeAutoSendMailRegistration(Result);
                                        }
                                        else
                                        {
                                            StringResult = subValue[0] + "$MCM005";
                                            string content = "'" + code + "': new account '" + Result.Code + "' - " + Result.NickName + " failed(invalid ip)";
                                            string comment = "[add new account]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }   
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$Account exist";
                                        string content = "'" + code + "': new account '" + Result.Code + "' - " + Result.NickName + " failed(account exist)";
                                        string comment = "[add new account]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$Invalid email";
                                    string content = "'" + code + "': new account '" + Result.Code + "' - " + Result.NickName + " failed(invalid email)";
                                    string comment = "[add new account]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region Add New Agent(LOG)
                        case "AddNewAgent":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = "";
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        Business.Agent result = new Agent();

                                        #region Create Agent
                                        string[] subParameter = subValue[1].Split('}');
                                        if (subParameter.Length > 0)
                                        {
                                            int agentGroupID = -1;
                                            bool isDisable = false;
                                            bool isIpFilter = false;

                                            int.TryParse(subParameter[0], out agentGroupID);
                                            result.Name = subParameter[1];
                                            result.Comment = subParameter[3];
                                            bool.TryParse(subParameter[4], out isDisable);
                                            bool.TryParse(subParameter[5], out isIpFilter);

                                            result.AgentGroupID = agentGroupID;
                                            result.IsDisable = isDisable;
                                            result.IsIpFilter = isIpFilter;
                                            result.IpForm = subParameter[6];
                                            result.IpTo = subParameter[7];
                                            result.Code = subParameter[8];
                                            result.Pwd = subParameter[9];
                                            result.GroupCondition = subParameter[10];
                                        }
                                        #endregion

                                        int isSpace = result.Code.IndexOf(' ');
                                        if (isSpace > 0)
                                        {
                                            temp = "invalid code login";

                                            string strContent = "'" + code + "': add new agent [Failed] (invalid code login)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, strContent, "[add agent]",ipAddress, code);
                                        }
                                        else
                                        {
                                            bool inves = Facade.FacadeGetInvestorByCode(result.Code);
                                            if (inves)
                                            {
                                                int ResultAddNew = TradingServer.Facade.FacadeAddNewAgent(result);

                                                #region INSERT SYSTEM LOG
                                                //INSERT SYSTEM LOG
                                                //'2222': manager config added/changed ['212121']
                                                string status = "[Failed]";

                                                if (ResultAddNew > 0)
                                                    status = "[Success]";

                                                string content = "'" + code + "': manager config added/changed ['" + subParameter[8] + "']" + status;
                                                string comment = "[add agent]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                                #endregion

                                                temp = ResultAddNew.ToString();
                                            }
                                            else temp = "Account is exist";
                                        }
                                    }
                                    StringResult = subValue[0] + "$" + temp;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add News
                        case "AddNews":
                            {
                                int ResultAddNew = -1;
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string content = subValue[1];
                                    if (subValue.Length > 2)
                                    {
                                        for (int i = 2; i < subValue.Length; i++)
                                        {
                                            content += subValue[i];
                                        }
                                    }
                                    Business.News result = new News();

                                    #region Create News
                                    string[] subParameter = content.Split('█');
                                    if (subParameter.Length > 2)
                                    {
                                        string title = subParameter[0];
                                        string body = subParameter[1];
                                        string category = subParameter[2];
                                        ResultAddNew = Facade.FacadeAddNews(title, body, DateTime.Now, category);
                                    }
                                    #endregion
                                }
                                StringResult = subValue[0] + "$" + ResultAddNew;
                            }
                            break;
                        #endregion

                        #region Add New Alert
                        case "AddNewAlert":
                            {
                                int ResultAddNew = -1;
                                string timeCreate = "";
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    Business.PriceAlert result = new PriceAlert();

                                    #region Create Alert
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        result.TickOnline = new Tick();
                                        result.Symbol = subParameter[0];
                                        result.Email = subParameter[1];
                                        result.PhoneNumber = subParameter[2];
                                        result.Value = double.Parse(subParameter[3]);
                                        #region ConditionAlert & ActionAlert
                                        switch (subParameter[4])
                                        {
                                            case "LargerBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerBid;
                                                    break;
                                                }
                                            case "LargerAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerAsk;
                                                    break;
                                                }
                                            case "LargerHighBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerHighBid;
                                                    break;
                                                }
                                            case "LargerHighAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerHighAsk;
                                                    break;
                                                }
                                            case "SmallerBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerBid;
                                                    break;
                                                }
                                            case "SmallerAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerAsk;
                                                    break;
                                                }
                                            case "SmallerLowBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerLowBid;
                                                    break;
                                                }
                                            case "SmallerLowAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                                    break;
                                                }
                                        }
                                        switch (subParameter[5])
                                        {
                                            case "Email":
                                                {
                                                    result.AlertAction = Business.ActionAlert.Email;
                                                    break;
                                                }
                                            case "SMS":
                                                {
                                                    result.AlertAction = Business.ActionAlert.SMS;
                                                    break;
                                                }
                                            case "Sound":
                                                {
                                                    result.AlertAction = Business.ActionAlert.Sound;
                                                    break;
                                                }
                                        }
                                        #endregion
                                        result.IsEnable = bool.Parse(subParameter[6]);
                                        result.InvestorID = int.Parse(subParameter[7]);
                                        result.Notification = subParameter[8];
                                        result.DateCreate = DateTime.Now;
                                        timeCreate = result.DateCreate.ToString();
                                        result.DateActive = result.DateCreate;
                                        ResultAddNew = TradingServer.Facade.FacadeAddNewAlert(result);
                                    }
                                    #endregion

                                }
                                StringResult = subValue[0] + "$" + ResultAddNew + "," + timeCreate;
                            }
                            break;

                        case "InsertNewAlert":
                            {
                                int ResultAddNew = -1;
                                string timeCreate = "";
                                Business.PriceAlert result = new PriceAlert();
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    #region Create Alert
                                    string[] subParameter = subValue[1].Split('{');
                                    if (subParameter.Length > 0)
                                    {
                                        result.TickOnline = new Tick();
                                        result.Symbol = subParameter[0];
                                        result.Email = subParameter[1];
                                        result.PhoneNumber = subParameter[2];
                                        result.Value = double.Parse(subParameter[3]);
                                        #region ConditionAlert & ActionAlert
                                        switch (subParameter[4])
                                        {
                                            case "LargerBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerBid;
                                                    break;
                                                }
                                            case "LargerAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerAsk;
                                                    break;
                                                }
                                            case "LargerHighBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerHighBid;
                                                    break;
                                                }
                                            case "LargerHighAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.LargerHighAsk;
                                                    break;
                                                }
                                            case "SmallerBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerBid;
                                                    break;
                                                }
                                            case "SmallerAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerAsk;
                                                    break;
                                                }
                                            case "SmallerLowBid":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerLowBid;
                                                    break;
                                                }
                                            case "SmallerLowAsk":
                                                {
                                                    result.AlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                                    break;
                                                }
                                        }
                                        switch (subParameter[5])
                                        {
                                            case "Email":
                                                {
                                                    result.AlertAction = Business.ActionAlert.Email;
                                                    break;
                                                }
                                            case "SMS":
                                                {
                                                    result.AlertAction = Business.ActionAlert.SMS;
                                                    break;
                                                }
                                            case "Sound":
                                                {
                                                    result.AlertAction = Business.ActionAlert.Sound;
                                                    break;
                                                }
                                        }
                                        #endregion
                                        result.IsEnable = bool.Parse(subParameter[6]);
                                        result.InvestorID = int.Parse(subParameter[7]);
                                        result.Notification = subParameter[8];
                                        result.DateCreate = DateTime.Now;
                                        timeCreate = result.DateCreate.ToString();
                                        result.DateActive = result.DateCreate;
                                        ResultAddNew = TradingServer.Facade.FacadeAddNewAlert(result);
                                    }
                                    #endregion
                                }
                                StringResult = subValue[0] + "$" + ResultAddNew + "{" + result.Symbol + "{" + result.Email + "{" + result.PhoneNumber + "{" +
                                    result.Value + "{" + result.AlertCondition + "{" + result.AlertAction + "{" + result.IsEnable + "{" +
                                    timeCreate + "{" + result.DateActive + "{" + result.InvestorID + "{" + result.Notification;
                            }
                            break;
                        #endregion

                        #region Add New Permit
                        case "AddPermit":
                            {
                                int ResultAddNew = -1;
                                Business.Permit Result = new Permit();
                                Result = this.ExtractionPermit(subValue[1]);
                                ResultAddNew = TradingServer.Facade.FacadeAddNewPermit(Result.AgentGroupID, Result.AgentID, Result.Role.RoleID);
                                StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                            }
                            break;
                        #endregion

                        #region Add New IAgentSecurity
                        case "AddNewIAgentSecurity":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    int ResultAddNew = -1;
                                    int AgentID = -1;
                                    int SecurityID = -1;
                                    bool IsUse = false;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out AgentID);
                                        int.TryParse(subParameter[1], out SecurityID);
                                        bool.TryParse(subParameter[2], out IsUse);
                                        ResultAddNew = TradingServer.Facade.FacadeAddNewIAgentSecurity(AgentID, SecurityID, IsUse, subParameter[3], subParameter[4]);
                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                            }
                            break;
                        case "AddListIAgentSecurity":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    int ResultAddNew = -1;
                                    List<Business.IAgentSecurity> Result = new List<IAgentSecurity>();
                                    Result = this.ExtractionIAgentSecurity(subValue[1]);
                                    ResultAddNew = TradingServer.Facade.FacadeAddListIAgentSecurity(Result);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                            }
                            break;
                        #endregion

                        #region Add New IAgentGroup
                        case "AddNewIAgentGroup":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    int ResultAddNew = -1;
                                    int AgentID = -1;
                                    int InvestorGroupID = -1;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out AgentID);
                                        int.TryParse(subParameter[1], out InvestorGroupID);
                                        ResultAddNew = TradingServer.Facade.FacadeAddNewIAgentGroup(AgentID, InvestorGroupID);
                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Add New Investor Profile
                        case "AddNewInvestorProfile":
                            {
                                int ResultAddNew = -1;
                                //Business.InvestorProfile Result = new InvestorProfile();
                                //Result = this.ExtractInvestorProfile(subValue[1]);
                                //ResultAddNew = TradingServer.Facade.FacadeAddNewInvestorProfile(Result);
                                StringResult = ResultAddNew.ToString();
                            }
                            break;
                        #endregion

                        #region Add New IGroupSecurityConfig
                        case "AddNewIGroupSecurityConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int ResultAddNew = -1;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);
                                    //Call Function Add New TradingConfig(SymbolConfig)
                                    ResultAddNew = TradingServer.Facade.FacadeAddIGroupSecurityConfig(Result);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add New IGroupSymbolConfig
                        case "AddNewIGroupSymbolConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int ResultAddNew = -1;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);
                                    //Call Function Add New TradingConfig(SymbolConfig)
                                    ResultAddNew = TradingServer.Facade.FacadeAddIGroupSymbolConfig(Result);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Add Deposit, Add Credit, Withrawals
                        case "AddDeposit":
                            {
                                string[] Parameter = subValue[1].Split(',');
                                int InvestorID = 0;
                                double Money = 0;
                                if (Parameter.Length == 3)
                                {
                                    bool ResultDeposit = false;
                                    int.TryParse(Parameter[0], out InvestorID);

                                    Business.Investor tempInvestor = new Investor();
                                    tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);

                                    bool parseMoney = double.TryParse(Parameter[1], out Money);
                                    bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                    bool checkRule = Facade.FacadeCheckPermitAddMoney(code);
                                    bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempInvestor.InvestorGroupInstance.InvestorGroupID);
                                    if (parseMoney && checkip && checkRule && checkGroup)
                                    {
                                        ResultDeposit = TradingServer.Facade.FacadeAddDeposit(InvestorID, Money, Parameter[2]);
                                        StringResult = subValue[0] + "$" + ResultDeposit + "," + InvestorID + "," + Money;
                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': account '9789300' withdrawal: 100000.00
                                        string status = "[Success]";
                                        if (!ResultDeposit)
                                        {
                                            status = "[Failed]";
                                        }

                                        string tempMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                                        string content = "'" + code + "': account '" + tempInvestor.Code + "' deposit: " + tempMoney + " " + status;
                                        string comment = "[deposit]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + InvestorID + "," + Money;
                                        string comment = "[deposit]";
                                        string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                                        if (!checkip)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' deposit: " + tempCredit + " failed (invalid ip)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!checkRule || !checkGroup)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' deposit: " + tempCredit + " failed (not enough rights)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!parseMoney)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' deposit: " + tempCredit + " failed (invalid amount)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                    }                                    
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false + "," + InvestorID + "," + Money;
                                }
                            }
                            break;

                        case "AddDepostAndCredit":
                            {
                                string[] Parameter = subValue[1].Split(',');
                                if (Parameter.Length == 3)
                                {
                                    bool ResultDeposit = false;
                                    int InvestorID = 0;
                                    int.TryParse(Parameter[0], out InvestorID);
                                    double Deposit = 0;
                                    bool parseDeposit = double.TryParse(Parameter[1], out Deposit);
                                    double Credit = 0;
                                    bool parseCredit = double.TryParse(Parameter[2], out Credit);

                                    if (parseDeposit && parseCredit)
                                    {
                                        ResultDeposit = TradingServer.Facade.FacadeUpdateBalanceAndCredit(InvestorID, Deposit, Credit);

                                        StringResult = subValue[0] + "$" + ResultDeposit + "," + InvestorID + "," + Deposit + "," + Credit;
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + InvestorID + "," + Deposit + "," + Credit;
                                    }
                                }
                            }
                            break;

                        case "AddCredit":
                            {
                                string[] Parameter = subValue[1].Split(',');
                                if (Parameter.Length == 3)
                                {
                                    bool ResultCredit = false;
                                    int InvestorID = 0;
                                    double Credit = 0;

                                    int.TryParse(Parameter[0], out InvestorID);
                                    bool parseCredit = double.TryParse(Parameter[1], out Credit);

                                    Business.Investor tempInvestor = new Investor();
                                    tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);
                                    bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                    bool checkRule = Facade.FacadeCheckPermitAddMoney(code);
                                    bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempInvestor.InvestorGroupInstance.InvestorGroupID);
                                    if (parseCredit && checkip && checkRule && checkGroup)
                                    {
                                        ResultCredit = TradingServer.Facade.FacadeAddCredit(InvestorID, Credit, Parameter[2]);

                                        StringResult = subValue[0] + "$" + ResultCredit + "," + InvestorID + "," + Credit;

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': account '9789300' withdrawal: 100000.00
                                        string status = "[Success]";
                                        if (!ResultCredit)
                                            status = "[Failed]";

                                        string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Credit.ToString(), 2);
                                        string content = "'" + code + "': account '" + tempInvestor.Code + "' credit in: " + tempCredit + " " + status;
                                        string comment = "[credit in]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + InvestorID + "," + Credit;
                                        string comment = "[credit in]";
                                        string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Credit.ToString(), 2);
                                        if (!checkip)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' credit in: " + tempCredit + " failed (invalid ip)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!checkRule || !checkGroup)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' credit in: " + tempCredit + " failed (not enough rights)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!parseCredit)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' credit in: " + tempCredit + " failed (invalid amount)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                    }
                                }
                            }
                            break;

                        case "SubCredit":
                            {
                                string[] Parameter = subValue[1].Split(',');
                                if (Parameter.Length == 3)
                                {
                                    bool ResultCredit = false;
                                    int InvestorID = 0;
                                    double Credit = 0;

                                    int.TryParse(Parameter[0], out InvestorID);
                                    bool parseCredit = double.TryParse(Parameter[1], out Credit);

                                    Business.Investor tempInvestor = new Investor();
                                    tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);

                                    bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                    bool checkRule = Facade.FacadeCheckPermitAddMoney(code);
                                    bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempInvestor.InvestorGroupInstance.InvestorGroupID);
                                    if (parseCredit && checkip && checkRule && checkGroup)
                                    {
                                        ResultCredit = TradingServer.Facade.FacadeSubCredit(InvestorID, Credit, Parameter[2]);
                                        StringResult = subValue[0] + "$" + ResultCredit + "," + InvestorID + "," + Credit;
                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': account '9789300' withdrawal: 100000.00
                                        string status = "[Success]";
                                        if (!ResultCredit)
                                            status = "[Failed]";

                                        string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Credit.ToString(), 2);
                                        string content = "'" + code + "': account '" + tempInvestor.Code + "' credit out: " + tempCredit + " " + status;
                                        string comment = "[credit out]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + InvestorID + "," + Credit;
                                        string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Credit.ToString(), 2);
                                        string comment = "[credit out]";
                                        if (!checkip)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' credit out: " + tempCredit + " failed (invalid ip)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!checkRule || !checkGroup)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' credit out: " + tempCredit + " failed (not enough rights)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!parseCredit)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' credit out: " + tempCredit + " failed (invalid amount)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                    }
                                }
                            }
                            break;

                        case "WithRawals":
                            {
                                string[] Parameter = subValue[1].Split(',');
                                if (Parameter.Length == 3)
                                {
                                    bool ResultWithRawals = false;
                                    int InvestorID = 0;
                                    double Money = 0;
                                    int.TryParse(Parameter[0], out InvestorID);
                                    bool parseMoney = double.TryParse(Parameter[1], out Money);

                                    Business.Investor tempInvestor = new Investor();
                                    tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);
                                    bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                    bool checkRule = Facade.FacadeCheckPermitAddMoney(code);
                                    bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempInvestor.InvestorGroupInstance.InvestorGroupID);
                                    if (parseMoney && checkip && checkRule && checkGroup)
                                    {
                                        ResultWithRawals = TradingServer.Facade.FacadeWithRawals(InvestorID, Money, Parameter[2]);
                                        StringResult = subValue[0] + "$" + ResultWithRawals + "," + InvestorID + "," + Money;
                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': account '9789300' withdrawal: 100000.00
                                        string status = "[Success]";
                                        if (!ResultWithRawals)
                                            status = "[Failed]";

                                        string formatMoney = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                                        string content = "'" + code + "': account '" + tempInvestor.Code + "' withdrawal: " + formatMoney + " " + status;
                                        string comment = "[withdrawal]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + InvestorID + "," + Money;
                                        string comment = "[withdrawal]";
                                        string tempCredit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Money.ToString(), 2);
                                        if (!checkip)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' withdrawal: " + tempCredit + " failed (invalid ip)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!checkRule || !checkGroup)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' withdrawal: " + tempCredit + " failed (not enough rights)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                        if (!parseMoney)
                                        {
                                            string content = "'" + code + "': account '" + tempInvestor.Code + "' withdrawal: " + tempCredit + " failed (invalid amount)";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                    }

                                   
                                }
                            }
                            break;
                        #endregion

                        #region MAKE COMMAND BY MANAGER
                        case "AddCommandByManager":
                            {
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    if (checkRule)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 9)
                                        {
                                            #region Map Value
                                            int InvestorID = 0;
                                            double Size = 0;
                                            double StopLoss = 0;
                                            double TakeProfit = 0;
                                            double OpenPrice = 0;
                                            int CommandType = 0;
                                            string Symbol = string.Empty;
                                            DateTime ExpTime = DateTime.Now;

                                            int.TryParse(subParameter[0], out InvestorID);
                                            double.TryParse(subParameter[1], out Size);
                                            Symbol = subParameter[2];
                                            double.TryParse(subParameter[3], out StopLoss);
                                            double.TryParse(subParameter[4], out TakeProfit);
                                            double.TryParse(subParameter[5], out OpenPrice);
                                            int.TryParse(subParameter[6], out CommandType);
                                            DateTime.TryParse(subParameter[7], out ExpTime);
                                            #endregion
                                            if (Size > 0)
                                            {
                                                Business.OpenTrade newCommand = new OpenTrade();
                                                newCommand = TradingServer.Facade.FacadeFillInstanceOpenTrade(InvestorID, subParameter[2], CommandType);

                                                newCommand.Size = Size;
                                                newCommand.StopLoss = StopLoss;
                                                newCommand.TakeProfit = TakeProfit;
                                                newCommand.OpenPrice = OpenPrice;
                                                newCommand.ExpTime = ExpTime;
                                                newCommand.IsServer = true;
                                                newCommand.Comment = subParameter[8];

                                                //CALCULATION MARGIN
                                                //newCommand.CalculatorMarginCommand(newCommand);
                                                bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, newCommand.Investor.InvestorGroupInstance.InvestorGroupID);
                                                if (checkGroup)
                                                {
                                                    bool checkAccount = newCommand.CheckValidAccountInvestor(newCommand);

                                                    if (checkAccount)
                                                    {
                                                        bool IsTrade = false;
                                                        double Minimum = -1;
                                                        double Maximum = -1;
                                                        double Step = -1;
                                                        bool ResultCheckStepLots = false;

                                                        //CHECK MIN MAX LOTS COMMAND
                                                        #region Get Config IGroupSecurity
                                                        if (newCommand.IGroupSecurity.IGroupSecurityConfig != null)
                                                        {
                                                            int countIGroupSecurityConfig = newCommand.IGroupSecurity.IGroupSecurityConfig.Count;
                                                            for (int i = 0; i < countIGroupSecurityConfig; i++)
                                                            {
                                                                if (newCommand.IGroupSecurity.IGroupSecurityConfig[i].Code == "B01")
                                                                {
                                                                    if (newCommand.IGroupSecurity.IGroupSecurityConfig[i].BoolValue == 1)
                                                                        IsTrade = true;
                                                                }

                                                                if (newCommand.IGroupSecurity.IGroupSecurityConfig[i].Code == "B11")
                                                                {
                                                                    double.TryParse(newCommand.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Minimum);
                                                                }

                                                                if (newCommand.IGroupSecurity.IGroupSecurityConfig[i].Code == "B12")
                                                                {
                                                                    double.TryParse(newCommand.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Maximum);
                                                                }

                                                                if (newCommand.IGroupSecurity.IGroupSecurityConfig[i].Code == "B13")
                                                                {
                                                                    double.TryParse(newCommand.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Step);
                                                                }
                                                            }
                                                        }
                                                        #endregion

                                                        ResultCheckStepLots = newCommand.IGroupSecurity.CheckStepLots(Minimum, Maximum, Step, newCommand.Size);

                                                        if (ResultCheckStepLots)
                                                        {
                                                            newCommand.DealerCode = code;
                                                            newCommand.Symbol.MarketAreaRef.AddCommand(newCommand);
                                                            StringResult = subValue[0] + "$" + true + "," + "MCM001";
                                                        }
                                                        else
                                                        {
                                                            StringResult = subValue[0] + "$" + false + "," + "MCM003";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        StringResult = subValue[0] + "$" + false + "," + "MCM002";
                                                    }
                                                }
                                                else
                                                {
                                                    StringResult = subValue[0] + "$" + false + "," + "MCM006";
                                                    string content = "'" + code + "': manager open command failed(not enough rights)";
                                                    string comment = "[manager open command]";
                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                }
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$" + false + "," + "MCM003";
                                            }
                                        }
                                        else
                                        {
                                            StringResult = subValue[0] + "$" + false + "," + "MCM004";
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + "MCM006";
                                        string content = "'" + code + "': manager open command failed(not enough rights)";
                                        string comment = "[manager open command]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false + "," + "MCM005";
                                    string content = "'" + code + "': manager open command failed(invalid ip)";
                                    string comment = "[manager open command]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region FUNCTION IN CANDLES(LOG)
                        case "AddNewCandles":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool Result = false;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length == 8)
                                    {
                                        DateTime Time;
                                        double High = 0;
                                        double Low = 0;
                                        double Open = 0;
                                        double Close = 0;
                                        double openAsk = 0;
                                        double highAsk = 0;
                                        double lowAsk = 0;
                                        double closeAsk = 0;
                                        int Volume = 0;
                                        int TimeFrame = 0;

                                        DateTime.TryParse(subParameter[1], out Time);
                                        double.TryParse(subParameter[2], out High);
                                        double.TryParse(subParameter[3], out Low);
                                        double.TryParse(subParameter[4], out Open);
                                        double.TryParse(subParameter[5], out Close);
                                        int.TryParse(subParameter[6], out Volume);
                                        int.TryParse(subParameter[7], out TimeFrame);
                                        //double.TryParse(subParameter[8], out openAsk);
                                        //double.TryParse(subParameter[9], out highAsk);
                                        //double.TryParse(subParameter[10], out lowAsk);
                                        //double.TryParse(subParameter[11], out closeAsk);

                                        ProcessQuoteLibrary.Business.Candles objCandles = new ProcessQuoteLibrary.Business.Candles();
                                        objCandles.Close = Close;
                                        objCandles.CloseAsk = closeAsk;
                                        objCandles.High = High;
                                        objCandles.HighAsk = highAsk;
                                        objCandles.Low = Low;
                                        objCandles.LowAsk = lowAsk;
                                        objCandles.Name = subParameter[0];
                                        objCandles.Open = Open;
                                        objCandles.OpenAsk = openAsk;
                                        objCandles.Time = Time;
                                        objCandles.TimeFrame = TimeFrame;
                                        objCandles.Volume = Volume;

                                        int ResultAdd = ProcessQuoteLibrary.Business.QuoteProcess.AddCandles(objCandles);
                                        if (ResultAdd > 0)
                                            Result = true;

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': XAUUSD 1 bars added
                                        string status = "[Failed]";

                                        if (ResultAdd > 0)
                                            status = "[Success]";

                                        string content = "'" + code + "': " + subParameter[0] + " 1 bars added";
                                        string comment = "[add new candles]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion

                                        StringResult = subValue[0] + "$" + Result + "," + ResultAdd;
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + -1;
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;

                        case "DeleteCandles":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool Result = false;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length == 2)
                                    {
                                        int CandlesID = 0;
                                        int TimeFrame = 0;

                                        int.TryParse(subParameter[0], out CandlesID);
                                        int.TryParse(subParameter[1], out TimeFrame);

                                        Result = ProcessQuoteLibrary.Business.QuoteProcess.DeleteCandlesByID(CandlesID, TimeFrame);

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': XAUUSD M1 1 bars deleted
                                        string status = "[Failed]";

                                        if (Result)
                                            status = "[Success]";

                                        string content = "'" + code + "': " + subParameter[0] + " 1 bars deleted";
                                        string comment = "[delete candles]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion

                                        StringResult = subValue[0] + "$" + Result;
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false;
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;

                        case "UpdateCandles":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool Result = false;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length == 8)
                                    {
                                        int CandlesID = 0;
                                        DateTime Time;
                                        int Volume = 0;
                                        double Open = 0;
                                        double Close = 0;
                                        double High = 0;
                                        double Low = 0;
                                        double openAsk = 0;
                                        double highAsk = 0;
                                        double lowAsk = 0;
                                        double closeAsk = 0;
                                        int TimeFrame = 0;

                                        int.TryParse(subParameter[0], out CandlesID);
                                        DateTime.TryParse(subParameter[1], out Time);
                                        int.TryParse(subParameter[2], out Volume);
                                        double.TryParse(subParameter[3], out Open);
                                        double.TryParse(subParameter[4], out Close);
                                        double.TryParse(subParameter[5], out High);
                                        double.TryParse(subParameter[6], out Low);
                                        int.TryParse(subParameter[7], out TimeFrame);
                                        //double.TryParse(subParameter[8], out openAsk);
                                        //double.TryParse(subParameter[9], out highAsk);
                                        //double.TryParse(subParameter[10], out lowAsk);
                                        //double.TryParse(subParameter[11], out closeAsk);

                                        ProcessQuoteLibrary.Business.Candles objCandles = new ProcessQuoteLibrary.Business.Candles();
                                        objCandles.Open = Open;
                                        objCandles.High = High;
                                        objCandles.Low = Low;
                                        objCandles.Close = Close;

                                        //objCandles.OpenAsk = openAsk;
                                        //objCandles.HighAsk = highAsk;
                                        //objCandles.LowAsk = lowAsk;
                                        //objCandles.CloseAsk = closeAsk;

                                        objCandles.ID = CandlesID;
                                        objCandles.Time = Time;
                                        objCandles.TimeFrame = TimeFrame;
                                        objCandles.Volume = Volume;

                                        Result = ProcessQuoteLibrary.Business.QuoteProcess.UpdateCandleOnline(objCandles);

                                        StringResult = subValue[0] + "$" + Result;
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false;
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region FUNCTION IN JOURNAL
                        case "AddJournal":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length == 4)
                                {
                                    int typeID = 0;
                                    int.TryParse(subParameter[0], out typeID);
                                    bool resultAdd = TradingServer.Facade.FacadeAddNewSystemLog(typeID, subParameter[1], subParameter[2], ipAddress, code);

                                    StringResult = subValue[0] + "$" + resultAdd;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false;
                                }
                            }
                            break;
                        #endregion

                        #region REOPEN COMMAND(LOG)
                        case "ReOpenComamnd":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    if (checkRule)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        bool resultRe = false;
                                        if (subParameter.Length == 16)
                                        {
                                            #region MAP VALUE
                                            Business.OpenTrade result = new OpenTrade();
                                            int commandHistoryID;
                                            int commandType;
                                            double lots;
                                            //string symbol;
                                            DateTime openTime;
                                            double openPrice;
                                            double stopLoss;
                                            DateTime closeTime;
                                            double closePrice;
                                            double takeProfit;
                                            double commission;
                                            double agentCommission;
                                            double swap;
                                            //string comment;
                                            double taxes;
                                            DateTime expTime;

                                            int.TryParse(subParameter[0], out commandHistoryID);
                                            int.TryParse(subParameter[1], out commandType);
                                            double.TryParse(subParameter[2], out lots);
                                            //Symbol subparameter[3]
                                            DateTime.TryParse(subParameter[4], out openTime);
                                            double.TryParse(subParameter[5], out openPrice);
                                            double.TryParse(subParameter[6], out stopLoss);
                                            DateTime.TryParse(subParameter[7], out closeTime);
                                            double.TryParse(subParameter[8], out closePrice);
                                            double.TryParse(subParameter[9], out takeProfit);
                                            double.TryParse(subParameter[10], out commission);
                                            double.TryParse(subParameter[11], out agentCommission);
                                            double.TryParse(subParameter[12], out swap);
                                            //subParameter[13], out comment
                                            double.TryParse(subParameter[14], out taxes);
                                            DateTime.TryParse(subParameter[15], out expTime);
                                            #endregion

                                            result = TradingServer.Facade.FacadeGetCommandHistoryByCommandID(commandHistoryID);

                                            if (result != null)
                                            {
                                                Business.OpenTrade tempOpenTrade = new OpenTrade();
                                                tempOpenTrade = TradingServer.ClientFacade.FillInstanceOpenTrade(subParameter[3], result.Investor.InvestorID, commandType);

                                                if (tempOpenTrade.Investor != null && tempOpenTrade.Symbol != null && tempOpenTrade.IGroupSecurity != null)
                                                {
                                                    #region VALID INVESTOR, SYMBOL, IGROUPSECURITY INSTANCE
                                                    //SET NEW DATA
                                                    tempOpenTrade.Size = lots;
                                                    tempOpenTrade.OpenTime = result.OpenTime;
                                                    tempOpenTrade.OpenPrice = openPrice;
                                                    tempOpenTrade.StopLoss = stopLoss;
                                                    tempOpenTrade.CloseTime = closeTime;
                                                    tempOpenTrade.ClosePrice = closePrice;
                                                    tempOpenTrade.TakeProfit = takeProfit;
                                                    tempOpenTrade.Commission = commission;
                                                    tempOpenTrade.AgentCommission = agentCommission;
                                                    tempOpenTrade.Swap = swap;
                                                    //tempOpenTrade.Swap = 0;
                                                    tempOpenTrade.Taxes = taxes;
                                                    tempOpenTrade.ExpTime = expTime;
                                                    tempOpenTrade.Comment = subParameter[13];
                                                    tempOpenTrade.CommandCode = result.CommandCode;
                                                    tempOpenTrade.IsServer = true;
                                                    tempOpenTrade.IsReOpen = true;
                                                    tempOpenTrade.DealerCode = code;
                                                    tempOpenTrade.IsActivePending = result.IsActivePending;
                                                    tempOpenTrade.IsStopLossAndTakeProfit = result.IsStopLossAndTakeProfit;

                                                    //CHECK IF PENDING ORDER THEN SET PROFIT = 0(BECAUSE, IF PENDING ORDER WITH PROFIT != 0 THEN WHEN STOP OUT
                                                    //SYSTEM WILL PLUS PROFIT TO BALANCE
                                                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(tempOpenTrade.Type.ID);
                                                    if (isPending)
                                                        tempOpenTrade.Profit = 0;

                                                    tempOpenTrade.Symbol.MarketAreaRef.AddCommand(tempOpenTrade);

                                                    //DELETE COMMAND HISTORY
                                                    resultRe = TradingServer.Facade.FacadeDeleteCommandHistory(commandHistoryID);

                                                    if (resultRe)
                                                    {
                                                        if (Business.Market.InvestorList != null)
                                                        {
                                                            int count = Business.Market.InvestorList.Count;
                                                            for (int j = 0; j < count; j++)
                                                            {
                                                                if (Business.Market.InvestorList[j].InvestorID == tempOpenTrade.Investor.InvestorID)
                                                                {
                                                                    #region UPDATE BALANCE ACCOUNT OF INVESTOR AFTER REOPEN COMMAND
                                                                    double totalProfit = result.Profit + result.Commission + result.Swap;
                                                                    //CHANGE FORMAT REOPEN COMMAND(DONT RETURN SWAP TO BALANCE)
                                                                    //double totalProfit = result.Profit + result.Commission;

                                                                    double totalBalance = Business.Market.InvestorList[j].Balance - totalProfit;
                                                                    #endregion

                                                                    //UPDATE BALANCE IN DATABASE
                                                                    bool updateBalance = TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[j].InvestorID, totalBalance);

                                                                    if (updateBalance)
                                                                    {
                                                                        Business.Market.InvestorList[j].Balance -= totalProfit;

                                                                        //RECALCULATION ACCOUNT OF INVESTOR
                                                                        Business.Market.InvestorList[j].ReCalculationAccountInit();

                                                                        if (Business.Market.InvestorList[j].ClientCommandQueue == null)
                                                                            Business.Market.InvestorList[j].ClientCommandQueue = new List<string>();

                                                                        string message = "ROC53709";
                                                                        Business.Market.InvestorList[j].ClientCommandQueue.Add(message);
                                                                    }
                                                                    else
                                                                    {
                                                                        resultRe = false;
                                                                    }

                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    #region INSERT SYSTEM LOG
                                                    //'2222': restore order #11235348
                                                    //'9720467': restore order #11235348, sell 5.00 XAUUSD 
                                                    //'2222': open order #11235248 for '9720467' modified - sell 5.00 XAUUSD at 1.44730 -> sell 5.00 XAUUSD at 1.44730

                                                    //INSERT SYSTEM LOG 1
                                                    string content = "'" + code + "': restore order #" + tempOpenTrade.CommandCode;
                                                    string comment = "[restore order]";
                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);

                                                    //INSERT SYSTEM LOG 2
                                                    string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Size.ToString(), 2);
                                                    string sizeBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(result.Size.ToString(), 2);

                                                    string strOpenPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.OpenPrice.ToString(), tempOpenTrade.Symbol.Digit);
                                                    string strOpenPriceBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(result.OpenPrice.ToString(), result.Symbol.Digit);
                                                    string mode = string.Empty;
                                                    string content1 = string.Empty;
                                                    string type = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(tempOpenTrade.Type.ID);
                                                    string typeBefore = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(result.Type.ID);
                                                    string strCommission = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(commission.ToString(), 2);
                                                    string strAgentCommission = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(agentCommission.ToString(), 2);
                                                    string strStopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(stopLoss.ToString(), tempOpenTrade.Symbol.Digit);
                                                    string strTakeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(takeProfit.ToString(), tempOpenTrade.Symbol.Digit);
                                                    string strSwap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(swap.ToString(), 2);

                                                    content = "'" + tempOpenTrade.Investor.Code + "': restore order #" + tempOpenTrade.CommandCode + ", [" + typeBefore + "] " + sizeBefore + " " + tempOpenTrade.Symbol.Name;
                                                    content1 = "'" + code + "': open order #" + tempOpenTrade.CommandCode + " for '" + tempOpenTrade.Investor.Code +
                                                        "' modified - [" + typeBefore + "] " + sizeBefore + " " + result.Symbol.Name + " at " + strOpenPriceBefore + " sl: " + strStopLoss + " tp: " + strTakeProfit +
                                                        " cm: " + strCommission + " agent commission: " + strAgentCommission + " swap: " + strSwap;

                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content1, comment, ipAddress, code);
                                                    #endregion
                                                }
                                                else
                                                {
                                                    if (tempOpenTrade.Investor == null)
                                                    {
                                                        StringResult = subValue[0] + "$MCM007";
                                                        string content = "'" + code + "': restore order #" + tempOpenTrade.CommandCode + " failed(investor account not found)";
                                                        string comment = "[restore order]";
                                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                    }

                                                    if (tempOpenTrade.Symbol == null)
                                                    {
                                                        StringResult = subValue[0] + "$MCM008";
                                                        string content = "'" + code + "': restore order #" + tempOpenTrade.CommandCode + " failed(symbol not found)";
                                                        string comment = "[restore order]";
                                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                    }

                                                    if (tempOpenTrade.IGroupSecurity == null)
                                                    {
                                                        StringResult = subValue[0] + "$MCM009";
                                                        string content = "'" + code + "': restore order #" + tempOpenTrade.CommandCode + " failed(symbol account not found)";
                                                        string comment = "[restore order]";
                                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                    }
                                                }
                                            }
                                        }

                                        StringResult = subValue[0] + "$" + resultRe;
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$MCM006";
                                        string content = "'" + code + "': restore order failed(not enough rights)";
                                        string comment = "[restore order]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                    string content = "'" + code + "': restore order failed(invalid ip)";
                                    string comment = "[restore order]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region ADD MARKET CONFIG(LOG)
                        case "AddMarketConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int ResultAddNew = -1;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);

                                    if (Result != null)
                                    {
                                        int countConfig = Result.Count;
                                        for (int m = 0; m < countConfig; m++)
                                        {   
                                            ResultAddNew = TradingServer.Facade.FacadeAddNewMarketConfig(-1, Result[m].Name, Result[m].Code,
                                                Result[m].BoolValue, Result[m].StringValue, Result[m].NumValue, Result[m].DateValue);

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            //'2222': symbol config added/changed ['XAUUSD']
                                            string status = "[Failed]";

                                            if (ResultAddNew > 0)
                                                status = "[Success]";

                                            string content = "'" + code + "': market config added/changed ['" + Result[m].Name + "'] " + status;
                                            string comment = "[add market]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }

                                    if (ResultAddNew > 0)
                                    {
                                        Facade.FacadeSendNoticeManagerChangeSymbol(2, Result[0].SecondParameterID);
                                    }
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region ADD TICK FROM MANAGER
                        case "AddTickByManager":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                bool result = false;
                                if (subParameter.Length == 3)
                                {
                                    double bid, ask;
                                    double.TryParse(subParameter[1], out bid);
                                    double.TryParse(subParameter[2], out ask);
                                    string symbol = subParameter[0];
                                    Symbol symbolInstance = null;
                                    for (int i = Market.SymbolList.Count - 1; i >= 0; i--)
                                    {
                                        if (symbol == Market.SymbolList[i].Name)
                                        {
                                            symbolInstance = Market.SymbolList[i];
                                            break;
                                        }
                                    }
                                    if (symbolInstance != null)
                                    {
                                        bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                        if (checkip)
                                        {
                                            bool checkRule = Facade.FacadeCheckPermitTickManager(code);
                                            checkRule = true;
                                            if (checkRule)
                                            {
                                                Business.Tick newTick = new Tick();
                                                newTick.Ask = ask;
                                                newTick.Bid = bid;
                                                newTick.SymbolName = symbol;
                                                newTick.IsManager = true;
                                                newTick.TickTime = DateTime.Now;
                                                this.UpdateTick(newTick);
                                                result = true;
                                                string content = "'" + code + "': add tick " + symbol + " " 
                                                    + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(bid.ToString(),symbolInstance.Digit) + " / " + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(ask.ToString(),symbolInstance.Digit) + " (" 
                                                    + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(symbolInstance.TickValue.Bid.ToString() ,symbolInstance.Digit) + " / " + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(symbolInstance.TickValue.Ask.ToString(),symbolInstance.Digit) + ")";
                                                string comment = "[add tick]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            }
                                            else
                                            {
                                                string content = "'" + code + "': add tick failed (not enough rights) " + symbol + " "
                                                    + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(bid.ToString(), symbolInstance.Digit) + " / " + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(ask.ToString(), symbolInstance.Digit) + " ("
                                                    + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(symbolInstance.TickValue.Bid.ToString(), symbolInstance.Digit) + " / " + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(symbolInstance.TickValue.Ask.ToString(), symbolInstance.Digit) + ")";
                                                string comment = "[add tick]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            }
                                        }
                                        else
                                        {
                                            string content = "'" + code + "': add tick failed (invalid ip) " + symbol + " "
                                                    + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(bid.ToString(), symbolInstance.Digit) + " / " + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(ask.ToString(), symbolInstance.Digit) + " ("
                                                    + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(symbolInstance.TickValue.Bid.ToString(), symbolInstance.Digit) + " / " + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(symbolInstance.TickValue.Ask.ToString(), symbolInstance.Digit) + ")";
                                            string comment = "[add tick]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + result;
                            }
                            break;
                        #endregion

                        #region SEND INTERNAL MAIL SERVER TO CLIENT
                        case "SendInternalMailToClient":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length > 0)
                                {
                                    string subject = TradingServer.Model.TradingCalculate.Instance.ConvertHexToString(subParameter[0]);
                                    string from = TradingServer.Model.TradingCalculate.Instance.ConvertHexToString(subParameter[1]);
                                    string fromName = TradingServer.Model.TradingCalculate.Instance.ConvertHexToString(subParameter[2]);
                                    string content = TradingServer.Model.TradingCalculate.Instance.ConvertHexToString(subParameter[3]);

                                    List<string> listInvestorCode = new List<string>();
                                    for (int j = 4; j < subParameter.Length; j++)
                                    {
                                        string login = subParameter[j];
                                        listInvestorCode.Add(login);
                                    }
                                    bool flag = false;
                                    if (listInvestorCode != null && listInvestorCode.Count > 0)
                                    {
                                        int count = listInvestorCode.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.InvestorList != null)
                                            {
                                                int countInvestor = Business.Market.InvestorList.Count;

                                                for (int j = 0; j < countInvestor; j++)
                                                {
                                                    if (Business.Market.InvestorList[j].Code.Trim() == listInvestorCode[i].Trim())
                                                    {
                                                        bool permit = Facade.FacadeCheckPermitAgentSendMail(from, Business.Market.InvestorList[j].InvestorGroupInstance.InvestorGroupID);
                                                        if (!permit)
                                                        {
                                                            flag = true;
                                                            break;
                                                        }

                                                        #region SEARCH INVESTOR IN INVESTOR LIST
                                                        if (Business.Market.InvestorList[j].IsOnline)
                                                        {
                                                            if (Business.Market.InvestorList[j].ClientCommandQueue == null)
                                                                Business.Market.InvestorList[j].ClientCommandQueue = new List<string>();

                                                            Business.InternalMail newInternalMailMessage = new InternalMail();
                                                            newInternalMailMessage.Content = content;
                                                            newInternalMailMessage.From = from;
                                                            newInternalMailMessage.FromName = fromName;
                                                            newInternalMailMessage.Subject = subject;
                                                            newInternalMailMessage.Time = DateTime.Now;
                                                            newInternalMailMessage.To = listInvestorCode[i];
                                                            newInternalMailMessage.ToName = Business.Market.InvestorList[j].NickName;
                                                            newInternalMailMessage.IsNew = true;



                                                            //ADD NEW MESSAGE TO DATABASE
                                                            int id = TradingServer.Facade.FacadeAddNewInternalMail(newInternalMailMessage);
                                                            string message = "ITM21358$" + id + "█" + subject + "█" + from + "█" + fromName + "█" +
                                                               newInternalMailMessage.Time.ToString() + "█" + newInternalMailMessage.IsNew.ToString() + "█" +
                                                               content;
                                                            //TradingServer.Facade.FacadeAddNewInternalMail(

                                                            int countInvestorOnline = Business.Market.InvestorList[j].CountInvestorOnline(Business.Market.InvestorList[j].InvestorID);
                                                            if (countInvestorOnline > 0)
                                                                Business.Market.InvestorList[j].ClientCommandQueue.Add(message);
                                                        }
                                                        else
                                                        {
                                                            Business.InternalMail newInternalMailMessage = new InternalMail();
                                                            newInternalMailMessage.Content = content;
                                                            newInternalMailMessage.From = from;
                                                            newInternalMailMessage.FromName = fromName;
                                                            newInternalMailMessage.Subject = subject;
                                                            newInternalMailMessage.Time = DateTime.Now;
                                                            newInternalMailMessage.To = listInvestorCode[i];
                                                            newInternalMailMessage.ToName = Business.Market.InvestorList[j].NickName;
                                                            newInternalMailMessage.IsNew = true;

                                                            //ADD NEW MESSAGE TO DATABASE
                                                            TradingServer.Facade.FacadeAddNewInternalMail(newInternalMailMessage);
                                                        }
                                                        #endregion

                                                        flag = true;
                                                    }
                                                }

                                                if (!flag)
                                                {
                                                    Business.InternalMail newInternalMailMessage = new InternalMail();
                                                    newInternalMailMessage.Content = content;
                                                    newInternalMailMessage.From = from;
                                                    newInternalMailMessage.FromName = fromName;
                                                    newInternalMailMessage.Subject = subject;
                                                    newInternalMailMessage.Time = DateTime.Now;
                                                    newInternalMailMessage.To = listInvestorCode[i];
                                                    newInternalMailMessage.IsNew = true;
                                                    int InvestorID = TradingServer.Facade.FacadeGetInvestorIDByCode(listInvestorCode[i]);
                                                    if (InvestorID != -1)
                                                    {
                                                        Business.Agent agent = new Business.Agent();
                                                        agent = TradingServer.Facade.FacadeGetAgentByInvestorID(InvestorID);
                                                        if (agent != null)
                                                        {
                                                            if (agent.Name == "")
                                                            {
                                                                newInternalMailMessage.ToName = newInternalMailMessage.To;
                                                            }
                                                            else
                                                            {
                                                                newInternalMailMessage.ToName = agent.Name;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            newInternalMailMessage.ToName = newInternalMailMessage.To;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newInternalMailMessage.ToName = newInternalMailMessage.To;
                                                    }

                                                    int id = TradingServer.Facade.FacadeAddNewInternalMail(newInternalMailMessage);
                                                    #region SEARCH AGENT IN AGENT LIST
                                                    for (int j = 0; j < Business.Market.AgentList.Count; j++)
                                                    {
                                                        if (Business.Market.AgentList[j].Code == listInvestorCode[i])
                                                        {
                                                            //add here
                                                            Business.Market.AgentList[j].AgentMail.Add(newInternalMailMessage);
                                                            Facade.FacadeSendNoticeManagerChangeMail(1, id, Business.Market.AgentList[j].AgentID);
                                                            break;
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                    StringResult = subValue[0] + "$" + flag.ToString();
                                }
                            }
                            break;
                        #endregion

                        #region SEND INTERMAIL CLIENT TO SERVER
                        case "ClientSendInternalMailToServer":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length == 4)
                                {
                                    string subject = subParameter[0];
                                    string from = subParameter[1];
                                    string content = subParameter[2];
                                    string to = subParameter[3];

                                    if (Business.Market.AgentList != null)
                                    {
                                        bool flag = false;
                                        int count = Business.Market.AgentList.Count;
                                        for (int j = 0; j < count; j++)
                                        {
                                            if (Business.Market.AgentList[j].Code == to)
                                            {
                                                if (Business.Market.AgentList[j].IsOnline)
                                                {

                                                }
                                                else
                                                {

                                                }

                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region SEND COMMAND LOGOUT CLIENT
                        case "LogOffClient":
                            {
                                bool result = false;
                                int investorID = 0;
                                if (int.TryParse(subValue[1], out investorID))
                                {
                                    Business.Investor tempInvestor = new Investor();
                                    tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(investorID);
                                    if (tempInvestor != null)
                                    {
                                        bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                        if (checkip)
                                        {
                                            bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempInvestor.InvestorGroupInstance.InvestorGroupID);
                                            if (checkGroup)
                                            {
                                                result = Business.Market.SendNotifyToClient("LOFF14790251", 3, investorID);

                                                if (result)
                                                {
                                                    string content = "'" + code + "': logoff '" + tempInvestor.Code + "' [Success]";
                                                    string comment = "[logoff client]";
                                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                                }
                                                else
                                                {
                                                    string content = "'" + code + "': logoff '" + tempInvestor.Code + "' [Failed]";
                                                    string comment = "[logoff client]";
                                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                                }
                                            }
                                            else
                                            {
                                                string content = "'" + code + "': logoff '" + tempInvestor.Code + "' failed(not enough rights)";
                                                string comment = "[logoff client]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                            }
                                        }
                                        else
                                        {
                                            string content = "'" + code + "': logoff '" + tempInvestor.Code + "' failed(invalid ip)";
                                            string comment = "[logoff client]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                        }
                                    }
                                }

                                StringResult = subValue[0] + "$" + result;
                            }
                            break;
                        #endregion

                        //Command Delete
                        //
                        #region Delete Investor Group(LOG)
                        case "DeleteInvestorGroup":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length == 1)
                                    {
                                        int InvestorGroupID = 0;
                                        int.TryParse(subParameter[0], out InvestorGroupID);
                                        StringResult = subValue[0] + "$" + this.DeleteGroup(InvestorGroupID);
                                    }
                                }
                            }
                            break;

                        case "DeleteInvestorGroupByName":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        string resultDelete = string.Empty;
                                        if (subParameter.Length == 1)
                                        {
                                            resultDelete = this.DeleteGroup(subParameter[0]);
                                            StringResult = subValue[0] + "$" + resultDelete;
                                        }

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        string status = "[Failed]";
                                        if (resultDelete == "DSyE010")
                                            status = "[Success]";

                                        string content = "'" + code + "': " + subParameter[0] + " has been delete " + status;
                                        string comment = "[delete group]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete Investor Group Config
                        case "DeleteInvestorGroupConfig":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int InvestorGroupConfigID = 0;
                                        int.TryParse(subParameter[0], out InvestorGroupConfigID);
                                        bool Result = false;
                                        Result = TradingServer.Facade.FacadeDeleteInvestorGroupConfig(InvestorGroupConfigID);
                                        StringResult = subValue[0] + "$" + Result.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete IGroupSymbol
                        case "DeleteIGroupSymbol":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length > 0)
                                        {
                                            int IGroupSymbolID = 0;
                                            int.TryParse(subParameter[0], out IGroupSymbolID);
                                            bool Result = false;
                                            Result = TradingServer.Facade.FacadeDeleteIGroupSymbol(IGroupSymbolID);
                                            StringResult = subValue[0] + "$" + Result.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete IGroupSecurityConfig
                        case "DeleteIGroupSymbolConfigByIGroupSymbolID":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int IGroupSymbolID = -1;
                                    int.TryParse(subValue[1], out IGroupSymbolID);
                                    StringResult = subValue[0] + "$" + TradingServer.Facade.FacadeDeleteIGroupSymbolConfigByIGroupSymbolID(IGroupSymbolID);
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete IGroupSecurity
                        case "DeleteIGroupSecurity":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length > 0)
                                        {
                                            int IGroupSecurityID = 0;
                                            int.TryParse(subParameter[0], out IGroupSecurityID);
                                            bool Result = false;
                                            Result = TradingServer.Facade.FacadeDeleteIGroupSecurityByIGroupSecurityID(IGroupSecurityID);
                                            StringResult = subValue[0] + "$" + Result.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete IGroupSecurityConfig
                        case "DeleteIGroupSecurityConfigByIGroupSecurityID":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int IGroupSecurityID = -1;
                                    int.TryParse(subValue[1], out IGroupSecurityID);
                                    StringResult = subValue[0] + "$" + TradingServer.Facade.FacadeDeleteIGroupSecurityConfigByIGroupSecurity(IGroupSecurityID);
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete Security(LOG)
                        case "DeleteSecurityByName":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        string resultDelete = string.Empty;
                                        if (subParameter.Length == 1)
                                        {
                                            resultDelete = this.DeleteSecurity(subParameter[0]);
                                            StringResult = subValue[0] + "$" + resultDelete;
                                        }

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        string status = "[Failed]";
                                        if (resultDelete == "DSyE006")
                                            status = "[Success]";

                                        string content = "'" + code + "': " + subParameter[0] + " has been delete " + status;
                                        string comment = "[delete security]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;

                        case "DeleteSecurityByID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length == 1)
                                    {
                                        int SecurityID = 0;
                                        int.TryParse(subParameter[0], out SecurityID);
                                        StringResult = subValue[0] + "$" + this.DeleteSecurity(SecurityID);
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete Symbol(LOG)
                        case "DeleteSymbol":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 1)
                                        {
                                            string result = this.DeleteSymbol(subParameter[0]);
                                            StringResult = subValue[0] + "$" + result;

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            string status = "[Failed]";
                                            if (result == "DSyE000")
                                                status = "[Success]";

                                            string content = "'" + code + "': " + subParameter[0] + " has been delete " + status;
                                            string comment = "[delete symbol]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete TradingConfig(SymbolConfig)
                        case "DeleteTradingConfig":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int TradingConfigID = 0;
                                        int.TryParse(subParameter[0], out TradingConfigID);
                                        bool Result = false;
                                        Result = TradingServer.Facade.FacadeDeleteTradingConfig(TradingConfigID);
                                        StringResult = subValue[0] + "$" + Result.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete SecurityConfig
                        case "DeleteSecurityConfig":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int SecurityConfigID = 0;
                                        int.TryParse(subParameter[0], out SecurityConfigID);
                                        bool Result = false;
                                        Result = TradingServer.Facade.FacadeDeleteSecurityConfigBySecurityConfigID(SecurityConfigID);
                                        StringResult = subValue[0] + "$" + Result.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete Investor
                        case "DeleteInvestor":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int Result = -1;
                                        int InvestorID = 0;
                                        int.TryParse(subParameter[0], out InvestorID);
                                        Result = TradingServer.Facade.FacadeDeleteInvestor(InvestorID);
                                        StringResult = subValue[0] + "$" + Result.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete Agent
                        case "DeleteAgentByID":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        if (subValue[1].Length > 0)
                                        {
                                            int Result = -1;
                                            int AgentID = 0;
                                            int.TryParse(subValue[1], out AgentID);
                                            TradingServer.Facade.FacadeManagerLogout(AgentID);
                                            TradingServer.Facade.FacadeAdminLogout(AgentID);
                                            Business.Agent agent = new Business.Agent();
                                            agent = TradingServer.Facade.FacadeGetAgentByAgentID(AgentID);
                                            Result = TradingServer.Facade.FacadeDeleteIAgentSecurityByAgentID(AgentID);
                                            if (Result != -1)
                                            {
                                                Result = TradingServer.Facade.FacadeDeleteIAgentGroupByAgentID(AgentID);
                                            }
                                            if (Result != -1)
                                            {
                                                Result = TradingServer.Facade.FacadeDeletePermitByAgentID(AgentID);
                                            }
                                            if (Result != -1)
                                            {
                                                Result = TradingServer.Facade.FacadeDeleteAgent(AgentID);
                                            }
                                            if (Result != -1)
                                            {
                                                Result = TradingServer.Facade.FacadeDeleteInvestor(agent.InvestorID);
                                            }
                                            StringResult = subValue[0] + "$" + Result.ToString();

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            string status = "[Failed]";
                                            if (Result > 0)
                                                status = "[Success]";

                                            string content = "'" + code + "': " + agent.Name + " has been delete " + status;
                                            string comment = "[delete agent]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Delete Alert
                        case "DeleteAlertByID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    if (subValue[1].Length > 0)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length > 0)
                                        {
                                            int Result = -1;
                                            int alertID = int.Parse(subParameter[0]);
                                            string symbol = subParameter[1];
                                            int investorID = int.Parse(subParameter[2]);
                                            Result = TradingServer.Facade.FacadeDeleteAlertByID(alertID, symbol, investorID);
                                            if (Result != -1)
                                            {
                                                StringResult = subValue[0] + "$" + alertID;
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$" + Result.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "DeleteAlert":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    if (subValue[1].Length > 0)
                                    {
                                        string[] subParameter = subValue[1].Split('{');
                                        if (subParameter.Length > 0)
                                        {
                                            int Result = -1;
                                            int alertID = int.Parse(subParameter[0]);
                                            string symbol = subParameter[1];
                                            int investorID = int.Parse(subParameter[2]);
                                            Result = TradingServer.Facade.FacadeDeleteAlertByID(alertID, symbol, investorID);
                                            if (Result != -1)
                                            {
                                                StringResult = subValue[0] + "$" + alertID;
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$" + Result.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete Mail
                        case "DeleteMailByID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    bool Result = false;
                                    if (subValue[1].Length > 0)
                                    {
                                        int mailID = int.Parse(subValue[1]);
                                        Result = TradingServer.Facade.DeleteInternalMail(mailID);

                                    }
                                    StringResult = subValue[0] + "$" + Result.ToString();
                                }
                            }
                            break;
                        #endregion

                        #region Delete Permit
                        case "DeletePermitByID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        bool Result = false;
                                        int PermitID = 0;
                                        int.TryParse(subParameter[0], out PermitID);
                                        Result = TradingServer.Facade.FacadeDeletePermitByID(PermitID);
                                        StringResult = subValue[0] + "$" + Result.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete IAgentSecurity
                        case "DeleteIAgentSecurityByID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    bool ResultAddNew = false;
                                    int IAgentSecurityID = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out IAgentSecurityID);
                                        ResultAddNew = TradingServer.Facade.FacadeDeleteIAgentSecurityByID(IAgentSecurityID);
                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                            }
                            break;

                        case "DeleteIAgentSecurityByAgentID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    int ResultAddNew = -1;
                                    int AgentID = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out AgentID);
                                        ResultAddNew = TradingServer.Facade.FacadeDeleteIAgentSecurityByAgentID(AgentID);
                                        StringResult = subValue[0] + "$" + ResultAddNew;
                                    }
                                }
                            }
                            break;
                        case "DeleteIAgentSecurityBySecurityID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    bool ResultAddNew = false;
                                    int SecurityID = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out SecurityID);
                                        ResultAddNew = TradingServer.Facade.FacadeDeleteIAgentSecurityBySecurityID(SecurityID);
                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Delete IAgentGroup
                        case "DeleteIAgentGroupByID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    bool ResultAddNew = false;
                                    int IAgentGroupID = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out IAgentGroupID);
                                        ResultAddNew = TradingServer.Facade.FacadeDeleteIAgentGroupByID(IAgentGroupID);
                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                            }
                            break;
                        case "DeleteIAgentGroupByAgentID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    int ResultAddNew = -1;
                                    int AgentID = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out AgentID);
                                        ResultAddNew = TradingServer.Facade.FacadeDeleteIAgentGroupByAgentID(AgentID);
                                        StringResult = subValue[0] + "$" + ResultAddNew;
                                    }
                                }
                            }
                            break;
                        case "DeleteIAgentGroupByInvestorGroupID":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    bool ResultAddNew = false;
                                    int InvestorGroupID = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    if (subParameter.Length > 0)
                                    {
                                        int.TryParse(subParameter[0], out InvestorGroupID);
                                        ResultAddNew = TradingServer.Facade.FacadeDeleteIAgentGroupByInvestorGroupID(InvestorGroupID);
                                        StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                    }
                                }
                            }
                            break;

                        #endregion

                        #region Delete Investor Profile
                        case "DeleteInvestorProfile":
                            {
                                //if (!string.IsNullOrEmpty(subValue[1]))
                                //{
                                //    string[] subParameter = subValue[1].Split(',');
                                //    if (subParameter.Length > 0)
                                //    {
                                //        bool Result = false;
                                //        int InvestorID = 0;
                                //        int.TryParse(subParameter[0], out InvestorID);
                                //        Result = TradingServer.Facade.FacadeDeleteInvestorProfileByInvestorID(InvestorID);
                                //        StringResult = Result.ToString();
                                //    }
                                //}
                            }
                            break;
                        #endregion

                        #region Close Command By Manager(LOG)
                        case "CloseCommandByManager":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    if (checkRule)
                                    {
                                        bool Result = false;
                                        int CommandID = -1;
                                        double Size = 0;
                                        double ClosePrices = 0;
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 3)
                                        {
                                            int.TryParse(subParameter[0], out CommandID);
                                            double.TryParse(subParameter[1], out Size);
                                            double.TryParse(subParameter[2], out ClosePrices);

                                            Business.OpenTrade tempOpenTrade = TradingServer.Facade.FacadeFindOpenTradeInCommandEx(CommandID);
                                            bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID);
                                            if (checkGroup)
                                            {
                                                Result = TradingServer.ClientFacade.FacadeCloseSpotCommandByManager(CommandID, Size, ClosePrices);

                                                #region INSERT SYSTEM LOG
                                                //INSERT SYSTEM LOG
                                                //'2222': account '9789300', close order #11349528 buy 2.00 EURUSD at 1.4624
                                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Size.ToString(), 2);
                                                string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(tempOpenTrade.Type.ID);
                                                string closePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(subParameter[2], tempOpenTrade.Symbol.Digit);
                                                string content = "'" + code + "': account '" + tempOpenTrade.Investor.Code + "' close order #" + tempOpenTrade.CommandCode +
                                                    " " + mode + " " + size + " " + tempOpenTrade.Symbol.Name + " at " + closePrice;
                                                string comment = "[manager close command]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                #endregion

                                                if (Result)
                                                    StringResult = subValue[0] + "$" + Result + ",MCM001";
                                                else StringResult = subValue[0] + "$" + Result + ",MCM004";
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$" + false + ",MCM006";
                                                string content = "'" + code + "': manager close command failed(not enough rights)";
                                                string comment = "[manager close command]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                            }
                                        }
                                        else
                                        {
                                            StringResult = subValue[0] + "$" + false + ",MCM004";
                                            string content = "'" + code + "': manager close command failed(invalid parameter)";
                                            string comment = "[manager close command]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + ",MCM006";
                                        string content = "'" + code + "': manager close command failed(not enough rights)";
                                        string comment = "[manager close command]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false + ",MCM005";
                                    string content = "'" + code + "': manager close command failed(invalid ip)";
                                    string comment = "[manager close command]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region Close List Command By Manager
                        case "CloseListCommandByManager":
                            {
                                bool Result = false;
                                string[] subParamter = subValue[1].Split(',');
                                if (subParamter.Length > 0)
                                {
                                    int countCommand = subParamter.Length;
                                    for (int j = 0; j < countCommand; j++)
                                    {
                                        int CommandID = -1;
                                        double Size = 0;
                                        double ClosePrices = 0;
                                        int.TryParse(subParamter[j], out CommandID);
                                        j++;
                                        double.TryParse(subParamter[j], out Size);
                                        j++;
                                        double.TryParse(subParamter[j], out ClosePrices);
                                        Result = TradingServer.ClientFacade.FacadeCloseSpotCommandByManager(CommandID, Size, ClosePrices);
                                    }
                                }

                                StringResult = subValue[0] + "$" + Result;
                            }
                            break;
                        #endregion

                        #region Delete Command By Manager(Complete)(LOG)
                        case "DeleteCommandByManager":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    if (checkRule)
                                    {
                                        bool Result = false;
                                        int CommandID = -1;
                                        int.TryParse(subValue[1], out CommandID);
                                        Business.OpenTrade tempOpenTrade = new OpenTrade();

                                        tempOpenTrade = TradingServer.Facade.FacadeFindOpenTradeInCommandEx(CommandID);
                                        bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID);
                                        if (checkGroup)
                                        {
                                            if (tempOpenTrade.Symbol == null || tempOpenTrade.Investor == null || tempOpenTrade.IGroupSecurity == null || tempOpenTrade.Type == null)
                                                return StringResult = subValue[0] + "$" + false;

                                            #region INSERT SYSTEM LOG
                                            //'2222': account '9720467', delete order #11232898 buy 1.00 EURUSD at 1.4474
                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Size.ToString(), 2);
                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.OpenPrice.ToString(), tempOpenTrade.Symbol.Digit);
                                            string comment = "[manager delete command]";
                                            string type = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(tempOpenTrade.Type.ID);

                                            string content = "'" + code + "': account '" + tempOpenTrade.Investor.Code + "' delete order #" + tempOpenTrade.CommandCode +
                                                " " + type + " " + size + " " + tempOpenTrade.Symbol.Name + " at " + openPrice;

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                            #endregion

                                            Result = TradingServer.Facade.FacadeDeleteOpenTradeByManager(CommandID);
                                            if (Result)
                                                StringResult = subValue[0] + "$" + Result + ",MCM001";
                                            else StringResult = subValue[0] + "$" + Result + ",MCM004";
                                        }
                                        else
                                        {
                                            StringResult = subValue[0] + "$" + false + ",MCM006";
                                            string content = "'" + code + "': manager delete command failed(not enough rights)";
                                            string comment = "[manager delete command]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + ",MCM006";
                                        string content = "'" + code + "': manager delete command failed(not enough rights)";
                                        string comment = "[manager delete command]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false + ",MCM005";
                                    string content = "'" + code + "': manager delete command failed(invalid ip)";
                                    string comment = "[manager delete command]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region DELETE ORDER IN DATABASE
                        case "DeleteOrder":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                bool Result = false;
                                if (subParameter.Length == 2)
                                {
                                    int DealID = int.Parse(subParameter[0]);
                                    int InvestorID = int.Parse(subParameter[1]);
                                    switch (subParameter[1])
                                    {
                                        case "OPT001":
                                            Result = TradingServer.Facade.FacadeDeleteOpenTradeByID(DealID);

                                            if (Result)
                                            {
                                                TradingServer.Facade.FacadeRemoveOpenTradeInInvestorList(DealID, InvestorID);
                                            }
                                            break;
                                        case "CMH01":
                                            Result = TradingServer.Facade.FacadeDeleteCommandHistory(DealID);
                                            break;
                                        case "IAL01":
                                            Result = TradingServer.Facade.FacadeDeleteInvestorAccountLog(DealID);
                                            break;
                                    }
                                }

                                StringResult = subValue[0] + "$" + Result;
                            }
                            break;
                        #endregion

                        #region DELETE MARKET CONFIG(DELETE HOLIDAY)
                        case "DeleteMarketConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool result = false;
                                    int marketConfigID = -1;
                                    int.TryParse(subValue[1], out marketConfigID);
                                    result = TradingServer.Facade.FacadeDeleteMarketConfig(marketConfigID);

                                    StringResult = subValue[0] + "$" + result;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        //Command Update
                        //
                        #region Update Investor Group(LOG)
                        case "UpdateInvestorGroup":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    Business.InvestorGroup Result = new InvestorGroup();
                                    Result = this.ExtractInvestorGroup(subValue[1]);
                                    resultUpdate = Facade.FacadeUpdateInvestorGroup(Result);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': group config added/changed ['test-duc']
                                    //string status = "[Failed]";

                                    if (resultUpdate)
                                    {
                                        TradingServer.Facade.FacadeCheckUpdateGroupVirtualDealerOnline();
                                        //status = "[Success]";
                                    }

                                    //string content = "'" + code + "': group config added/changed ['" + Result.Name + "'] " + status;
                                    //string comment = "[update group]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion

                                    if (resultUpdate)
                                    {
                                        //SEND NOTIFY TO MANAGER
                                        Facade.FacadeSendNoticeManagerChangeGroup(1, Result.InvestorGroupID);

                                        //SEND NOTIFY TO AGENT SERVER
                                        Business.AgentNotify newAgentNotify = new AgentNotify();
                                        newAgentNotify.NotifyMessage = "UpdateGroup$" + Result.InvestorGroupID;
                                        TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Result);
                                    }
                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update Investor Group Config
                        case "UpdateInvestorGroupConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool ResultUpdate = false;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);
                                    ResultUpdate = Facade.FacadeUpdateInvestorGroupConfig(Result, ipAddress, code);
                                    
                                    if (ResultUpdate)
                                    {
                                        //SEND NOTIFY TO MANAGER
                                        Facade.FacadeSendNoticeManagerChangeGroup(2, Result[0].SecondParameterID);

                                        //SEND NOTIFY TO AGENT SERVER
                                        if (Business.Market.InvestorGroupList != null)
                                        {
                                            int count = Business.Market.InvestorGroupList.Count;
                                            for (int i = 0; i < count; i++)
                                            {
                                                if (Business.Market.InvestorGroupList[i].InvestorGroupID == Result[0].SecondParameterID)
                                                {
                                                    Business.AgentNotify newAgentNotify = new AgentNotify();
                                                    newAgentNotify.NotifyMessage = "UpdateGroup$" + Business.Market.InvestorGroupList[i].InvestorGroupID;
                                                    TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Business.Market.InvestorGroupList[i]);
                                                }
                                            }
                                        }

                                        //SEND NOTIFY TO CLIENT
                                        TradingServer.Business.Market.SendNotifyToClient("UIG69345", 1, Result[0].SecondParameterID);
                                    }

                                    StringResult = subValue[0] + "$" + ResultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update IGroupSymbol
                        case "UpdateIGroupSymbol":
                            {
                                bool resultUpdate = false;
                                List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
                                Result = this.ExtractIGroupSymbol(subValue[1]);
                                if (Result != null)
                                {
                                    int countResult = Result.Count;
                                    for (int j = 0; j < countResult; j++)
                                    {
                                        resultUpdate = Facade.FacadeUpdateIGroupSymbol(Result[j].IGroupSymbolID, Result[j].SymbolID, Result[j].InvestorGroupID);
                                    }

                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                            }
                            break;
                        #endregion

                        #region Update IGroupSecurity
                        case "UpdateIGroupSecurity":
                            {
                                bool resultUpdate = false;
                                List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
                                Result = this.ExtractIGroupSecurity(subValue[1]);
                                if (Result != null)
                                {
                                    int countResult = Result.Count;
                                    for (int j = 0; j < countResult; j++)
                                    {
                                        resultUpdate = Facade.FacadeUpdateIGroupSecurity(Result[j].IGroupSecurityID, Result[j].SecurityID, Result[j].InvestorGroupID);
                                    }

                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                            }
                            break;
                        #endregion

                        #region Update Security(LOG)
                        case "UpdateSecurity":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool ResultUpdate = false;
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        int MarketAreaID = -1;
                                        int.TryParse(subParameter[3], out MarketAreaID);
                                        if (subParameter.Length > 0)
                                        {
                                            int SecurityID = 0;
                                            int.TryParse(subParameter[0], out SecurityID);
                                            ResultUpdate = TradingServer.Facade.FacadeUpdateSecurity(SecurityID, subParameter[1], subParameter[2], MarketAreaID);

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            //'2222': group config added/changed ['test-duc']
                                            string status = "[Failed]";

                                            if (ResultUpdate)
                                                status = "[Success]";

                                            string content = "'" + code + "': security config added/changed ['" + subParameter[1] + "'] " + status;
                                            string comment = "[update security]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }

                                    StringResult = subValue[0] + "$" + ResultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update Symbol(LOG)
                        case "UpdateSymbol":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length > 0)
                                        {
                                            int SymbolID = 0;
                                            int SecurityID = 0;
                                            int RefSymbolID = 0;
                                            int MarketAreaID = 0;

                                            int.TryParse(subParameter[0], out SymbolID);
                                            int.TryParse(subParameter[1], out SecurityID);
                                            int.TryParse(subParameter[2], out RefSymbolID);
                                            int.TryParse(subParameter[3], out MarketAreaID);

                                            //Call Function Update New Symbol
                                            resultUpdate = TradingServer.Facade.FacadeUpdateSymbol(SymbolID, SecurityID, RefSymbolID, MarketAreaID, subParameter[4]);

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            //'2222': group config added/changed ['test-duc']
                                            string status = "[Failed]";

                                            if (resultUpdate)
                                            {
                                                status = "[Success]";
                                                TradingServer.Facade.FacadeCheckUpdateGroupVirtualDealerOnline();
                                            }
                                            string content = "'" + code + "': symbol config added/changed ['" + subParameter[4] + "'] " + status;
                                            string comment = "[update symbol]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion

                                            if (resultUpdate)
                                            {
                                                Facade.FacadeSendNoticeManagerChangeSymbol(1, SymbolID);

                                                //SEND COMMAND TO AGENT SERVER
                                                Business.AgentNotify newAgentNotify = new AgentNotify();
                                                newAgentNotify.NotifyMessage = "UpdateSymbol$" + SymbolID;
                                                TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                                            }

                                            StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update TradingConfig(SymbolConfig)
                        case "UpdateTradingConfig":
                            {
                                bool resultUpdate = false;
                                List<Business.ParameterItem> Result = new List<ParameterItem>();
                                //CALL FUNCTION EXTRACT PARAMETER ITEM ONE 
                                Result = this.ExtractParameterItem(subValue[1]);
                                int countResult = Result.Count;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {                                     
                                    if (countResult > 0)
                                    {
                                        //for (int j = 0; j < countResult; j++)
                                        //{
                                        //    resultUpdate = Facade.FacadeUpdateTradingConfig(Result[j]);
                                        //}

                                        resultUpdate = Facade.FacadeUpdateTradingConfig(Result, code, ipAddress);
                                        //string status = "[Failed]";
                                        if (resultUpdate)
                                        {
                                            //status = "[Success]";
                                            //SEND NOTITY TO MANAGER
                                            Facade.FacadeSendNoticeManagerChangeSymbol(2, Result[0].SecondParameterID);

                                            //SEND NOTIFY TO CLIENT
                                            TradingServer.Business.Market.SendNotifyToClient("UTC534345", 2, Result[0].SecondParameterID);
                                        }

                                        #region LOG(Duc Comment because in file ParameterItem.TradingConfg have log details)
                                        //string symbol = "", spreadDefault = "", spreadBalanace = "", stops = "", execution = "";
                                        //for (int i = Market.SymbolList.Count - 1; i >= 0; i--)
                                        //{
                                        //    if (Result[0].SecondParameterID == Market.SymbolList[i].SymbolID)
                                        //    {
                                        //        symbol = Market.SymbolList[i].Name;
                                        //        break;
                                        //    }
                                        //}
                                        //for (int i = 0; i < Result.Count; i++)
                                        //{
                                        //    if (Result[i].Code == "S006")
                                        //    {
                                        //        execution = Result[i].StringValue;
                                        //    }
                                        //    if (Result[i].Code == "S015")
                                        //    {
                                        //        stops = Result[i].NumValue;
                                        //    }
                                        //    if (Result[i].Code == "S013")
                                        //    {
                                        //        spreadDefault = Result[i].NumValue;
                                        //    }
                                        //    if (Result[i].Code == "S016")
                                        //    {
                                        //        spreadDefault = Result[i].StringValue;
                                        //    }
                                        //}
                                        ////'2222': EURUSD5 - spread: 1 / 2, stops: 15, execution: Market
                                        //string content = "'" + code + "': " + symbol + " spread default: " + spreadDefault + ", spread balance: " + spreadBalanace
                                        //    + ", stops: " + stops + ", execution: " + execution + status;
                                        //string comment = "[update symbol]";
                                        //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                    if (countResult > 0)
                                    {
                                        #region LOG(Duc Comment because in file parameteritem.tradingconfig have log details)
                                        //string symbol = "", spreadDefault = "", spreadBalanace = "", stops = "", execution = "";
                                        //for (int i = Market.SymbolList.Count - 1; i >= 0; i--)
                                        //{
                                        //    if (Result[0].SecondParameterID == Market.SymbolList[i].SymbolID)
                                        //    {
                                        //        symbol = Market.SymbolList[i].Name;
                                        //        break;
                                        //    }
                                        //}
                                        //for (int i = 0; i < Result.Count; i++)
                                        //{
                                        //    if (Result[i].Code == "S006")
                                        //    {
                                        //        execution = Result[i].StringValue;
                                        //    }
                                        //    if (Result[i].Code == "S015")
                                        //    {
                                        //        stops = Result[i].NumValue;
                                        //    }
                                        //    if (Result[i].Code == "S013")
                                        //    {
                                        //        spreadDefault = Result[i].NumValue;
                                        //    }
                                        //    if (Result[i].Code == "S016")
                                        //    {
                                        //        spreadDefault = Result[i].StringValue;
                                        //    }
                                        //}
                                        ////'2222': EURUSD5 - spread: 1 / 2, stops: 15, execution: Market
                                        //string content = "'" + code + "': " + symbol + " spread default: " + spreadDefault + ", spread balance: " + spreadBalanace
                                        //    + ", stops: " + stops + ", execution: " + execution + " failed(invalid ip)";
                                        //string comment = "[update symbol]";
                                        //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region UPDATE MARKET CONFIG
                        case "UpdateMarketConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);
                                    resultUpdate = TradingServer.Facade.FacadeUpdateMarketConfig(Result, ipAddress, code);
                                    //int countResult = Result.Count;
                                    //for (int j = 0; j < countResult; j++)
                                    //{
                                    //    resultUpdate = TradingServer.Facade.FacadeUpdateMarketConfig(Result[j]);

                                    //    #region INSERT SYSTEM LOG
                                    //    //INSERT SYSTEM LOG
                                    //    //'2222': symbol config added/changed ['XAUUSD']                                
                                    //    string content = "'" + code + "': market config added/changed ['" + Result[j].Name + "'] ";
                                    //    string comment = "[update market]";
                                    //    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    //    #endregion
                                    //}

                                    StringResult = subValue[0] + "$" + resultUpdate;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update SecurityConfig
                        case "UpdateSecurityConfig":
                            {
                                bool ResultUpdate = false;
                                List<Business.ParameterItem> Result = new List<ParameterItem>();
                                Result = this.ExtractParameterItem(subValue[1]);

                                int countResult = Result.Count;
                                for (int j = 0; j < countResult; j++)
                                {
                                    ResultUpdate = Facade.FacadeUpdateSecurityConfig(Result[j]);
                                }

                                StringResult = subValue[0] + "$" + ResultUpdate.ToString();
                            }
                            break;
                        #endregion

                        #region Update Investor(LOG)
                        case "UpdateInvestor":
                            {
                                bool resultUpdate = false;
                                bool resultUpdateProfile = false;
                                Business.Investor Result = new Investor();
                                Result = this.ExtractionInvestor(subValue[1]);
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitAccountManagerAndAdmin(code);
                                    bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, Result.InvestorGroupInstance.InvestorGroupID);
                                    if (checkRule && checkGroup)
                                    {
                                        //resultUpdate = TradingServer.Facade.FacadeUpdateInvestor(Result);
                                        resultUpdate = TradingServer.Facade.FacadeUpdateInvestor(Result, ipAddress, code);
                                        //resultUpdateProfile = TradingServer.Facade.FacadeUpdateInvestorProfile(Result);
                                        if (resultUpdate)
                                        {
                                            resultUpdateProfile = TradingServer.Facade.FacadeUpdateInvestorProfile(Result, ipAddress, code);

                                            if (Result.PrimaryPwd != "") resultUpdate = TradingServer.Facade.FacadeUpdatePasswordByCode(Result.Code, Result.PrimaryPwd);
                                            if (Result.ReadOnlyPwd != "") resultUpdate = TradingServer.Facade.FacadeUpdateReadPwdByCode(Result.Code, Result.ReadOnlyPwd);
                                            if (Result.PhonePwd != "") resultUpdate = TradingServer.Facade.FacadeUpdatePhonePwdByCode(Result.Code, Result.PhonePwd);

                                            //SEND COMMAND TO AGENT SERVER
                                            Business.AgentNotify newAgentNotify = new AgentNotify();
                                            newAgentNotify.NotifyMessage = "UpdateInvestorNotify$" + Result.InvestorID;
                                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify, Result.InvestorGroupInstance);
                                        }

                                        StringResult = subValue[0] + "$" + resultUpdate.ToString();

                                        #region INSERT SYSTEM LOG
                                        ////INSERT SYSTEM LOG
                                        /////'2222': account '9720467' has been updated
                                        //string status = "[Failed]";

                                        //if (resultUpdate)
                                        //    status = "[Success]";

                                        //string content = "'" + code + "': account '" + Result.Code + "' has been updated " + status;
                                        //string comment = "[update account]";
                                        //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion      
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$MCM006";
                                        string content = "'" + code + "': account '" + Result.Code + "' has been updated failed(not enough rights)";
                                        string comment = "[update account]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + "MCM005";
                                    string content = "'" + code + "': account '" + Result.Code + "' has been updated failed(invalid ip)";
                                    string comment = "[update account]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                } 
                            }
                            break;
                        #endregion

                        #region UPDATE PASSWORD BY CODE(LOG)
                        case "UpdatePasswordByCode":
                            {
                                string[] subParameter = subValue[1].Split(',');

                                bool resultUpdate = TradingServer.Facade.FacadeUpdatePasswordByCode(subParameter[0], subParameter[1]);

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                ///'2222': account '9720467' change password failed
                                string status = "[Failed]";

                                if (resultUpdate)
                                    status = "[Success]";

                                string content = "'" + code + "': account '" + subParameter[0] + "' change master password " + status;
                                string comment = "[change password]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion

                                StringResult = subValue[0] + "$" + resultUpdate;
                            }
                            break;
                        #endregion

                        #region UPDATE READ PASSWORD BY CODE(LOG)
                        case "UpdateReadPwdByCode":
                            {
                                string[] subParameter = subValue[1].Split(',');

                                bool resultUpdate = TradingServer.Facade.FacadeUpdateReadPwdByCode(subParameter[0], subParameter[1]);

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                ///'2222': account '9720467' change password failed
                                string status = "[Failed]";

                                if (resultUpdate)
                                    status = "[Success]";

                                string content = "'" + code + "': account '" + subParameter[0] + "' change read password " + status;
                                string comment = "[change password]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion

                                StringResult = subValue[0] + "$" + resultUpdate;
                            }
                            break;
                        #endregion

                        #region UPDATE PHONE PASSWORD(LOG)
                        case "UpdatePhonePwdByCode":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                bool resultUpdate = TradingServer.Facade.FacadeUpdatePhonePwdByCode(subParameter[0], subParameter[1]);

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                ///'2222': account '9720467' change password failed
                                string status = "[Failed]";

                                if (resultUpdate)
                                    status = "[Success]";

                                string content = "'" + code + "': account '" + subParameter[0] + "' change phone password " + status;
                                string comment = "[change password]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion

                                StringResult = subValue[0] + "$" + resultUpdate;
                            }
                            break;
                        #endregion

                        #region Update Agent(LOG)
                        case "UpdateAgent":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    Business.Agent Result = new Agent();
                                    Result = this.ExtractionAgent(subValue[1]);
                                    resultUpdate = TradingServer.Facade.FacadeUpdateAgent(Result);
                                    if (Result.Pwd != "") resultUpdate = TradingServer.Facade.FacadeUpdatePasswordByCode(Result.Code, Result.Pwd);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': manager configuration has been changed 
                                    string status = "[Failed]";

                                    if (resultUpdate)
                                        status = "[Success]";

                                    string content = "'" + code + "': manager configuration has been changed " + status;
                                    string comment = "[update agent]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion

                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                                break;
                            }
                        case "UpdatePasswordAgent":
                            {
                                bool resultUpdate = false;
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {                                    
                                    if (subValue[1] != "")
                                    {
                                        string[] tempArr = subValue[1].Split('{');
                                        if (tempArr.Length == 3)
                                        {
                                            int investorID = 0;
                                            int.TryParse(tempArr[0], out investorID);
                                            if (investorID > 0)
                                            {
                                                resultUpdate = TradingServer.Facade.FacadeManagerChangePass(investorID,code, tempArr[1], tempArr[2]);
                                            }
                                        }
                                    }
                                    StringResult = subValue[0] + "$" + resultUpdate;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                string content = "";
                                if (resultUpdate)
                                {
                                    content = "': manager update password [Success]";
                                }
                                else
                                {
                                    if (checkip)
                                    {
                                        content = "': manager update password [Failed]";
                                    }
                                    else
                                    {
                                        content = "': manager update password (invalid ip)[Failed]";
                                    }
                                }
                                string comment = "[update password agent]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion
                                break;
                            }
                            
                        #endregion

                        #region Update Alert
                        case "UpdateAlert":
                            {
                                bool resultUpdate = false;
                                Business.PriceAlert result = new PriceAlert();
                                #region Update Alert
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length > 0)
                                {
                                    result.TickOnline = new Tick();
                                    result.Symbol = subParameter[0];
                                    result.Email = subParameter[1];
                                    result.PhoneNumber = subParameter[2];
                                    result.Value = double.Parse(subParameter[3]);
                                    #region ConditionAlert & ActionAlert
                                    switch (subParameter[4])
                                    {
                                        case "LargerBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerBid;
                                                break;
                                            }
                                        case "LargerAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerAsk;
                                                break;
                                            }
                                        case "LargerHighBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerHighBid;
                                                break;
                                            }
                                        case "LargerHighAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerHighAsk;
                                                break;
                                            }
                                        case "SmallerBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerBid;
                                                break;
                                            }
                                        case "SmallerAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerAsk;
                                                break;
                                            }
                                        case "SmallerLowBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerLowBid;
                                                break;
                                            }
                                        case "SmallerLowAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                                break;
                                            }
                                    }
                                    switch (subParameter[5])
                                    {
                                        case "Email":
                                            {
                                                result.AlertAction = Business.ActionAlert.Email;
                                                break;
                                            }
                                        case "SMS":
                                            {
                                                result.AlertAction = Business.ActionAlert.SMS;
                                                break;
                                            }
                                        case "Sound":
                                            {
                                                result.AlertAction = Business.ActionAlert.Sound;
                                                break;
                                            }
                                    }
                                    #endregion
                                    result.IsEnable = bool.Parse(subParameter[6]);
                                    result.InvestorID = int.Parse(subParameter[7]);
                                    result.DateCreate = DateTime.Parse(subParameter[8]);
                                    result.DateActive = result.DateCreate;
                                    result.Notification = subParameter[9];
                                    result.ID = int.Parse(subParameter[10]);
                                }
                                #endregion
                                resultUpdate = TradingServer.Facade.FacadeUpdateAlert(result);
                                StringResult = subValue[0] + "$" + resultUpdate.ToString();
                            }
                            break;
                        case "ConfigAlert":
                            {
                                bool resultUpdate = false;
                                Business.PriceAlert result = new PriceAlert();
                                #region Update Alert
                                string[] subParameter = subValue[1].Split('{');
                                if (subParameter.Length > 0)
                                {
                                    result.TickOnline = new Tick();
                                    result.Symbol = subParameter[0];
                                    result.Email = subParameter[1];
                                    result.PhoneNumber = subParameter[2];
                                    result.Value = double.Parse(subParameter[3]);
                                    #region ConditionAlert & ActionAlert
                                    switch (subParameter[4])
                                    {
                                        case "LargerBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerBid;
                                                break;
                                            }
                                        case "LargerAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerAsk;
                                                break;
                                            }
                                        case "LargerHighBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerHighBid;
                                                break;
                                            }
                                        case "LargerHighAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.LargerHighAsk;
                                                break;
                                            }
                                        case "SmallerBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerBid;
                                                break;
                                            }
                                        case "SmallerAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerAsk;
                                                break;
                                            }
                                        case "SmallerLowBid":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerLowBid;
                                                break;
                                            }
                                        case "SmallerLowAsk":
                                            {
                                                result.AlertCondition = Business.ConditionAlert.SmallerLowAsk;
                                                break;
                                            }
                                    }
                                    switch (subParameter[5])
                                    {
                                        case "Email":
                                            {
                                                result.AlertAction = Business.ActionAlert.Email;
                                                break;
                                            }
                                        case "SMS":
                                            {
                                                result.AlertAction = Business.ActionAlert.SMS;
                                                break;
                                            }
                                        case "Sound":
                                            {
                                                result.AlertAction = Business.ActionAlert.Sound;
                                                break;
                                            }
                                    }
                                    #endregion
                                    result.IsEnable = bool.Parse(subParameter[6]);
                                    result.InvestorID = int.Parse(subParameter[7]);
                                    result.DateCreate = DateTime.Parse(subParameter[8]);
                                    result.DateActive = result.DateCreate;
                                    result.Notification = subParameter[9];
                                    result.ID = int.Parse(subParameter[10]);
                                }
                                #endregion
                                resultUpdate = TradingServer.Facade.FacadeUpdateAlert(result);
                                if (resultUpdate)
                                {
                                    StringResult = subValue[0] + "$" + result.ID + "{" + result.Symbol + "{" + result.Email + "{" + result.PhoneNumber + "{" +
                                    result.Value + "{" + result.AlertCondition + "{" + result.AlertAction + "{" + result.IsEnable + "{" +
                                    result.DateCreate + "{" + result.DateActive + "{" + result.InvestorID + "{" + result.Notification;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$";
                                }

                            }
                            break;
                        #endregion

                        #region Update Mail
                        case "UpdateMailStatus":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length > 0)
                                {
                                    bool isNew = bool.Parse(subParameter[0]);
                                    int mailID = int.Parse(subParameter[1]);
                                    TradingServer.Facade.UpdateInternalMailStatus(isNew, mailID);
                                }
                                StringResult = subValue[0] + "$";
                            }
                            break;
                        #endregion

                        #region UPDATE(RESET) USERCONFIG IPHONE, IPAD, SIVERLIGHT
                        case "AdminResetUserConfig":
                            {
                                int investorID = int.Parse(subValue[1]);

                                bool isUpdate = Investor.DBWInvestorInstance.UpdateAllUserConfig("", "", "", investorID);

                                if (isUpdate)
                                {
                                    if (Business.Market.InvestorList != null)
                                    {
                                        int count = Business.Market.InvestorList.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.InvestorList[i].InvestorID == investorID)
                                            {
                                                Business.Market.InvestorList[i].UserConfig = "";
                                                Business.Market.InvestorList[i].UserConfigIpad = "";
                                                Business.Market.InvestorList[i].UserConfigIphone = "";

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Update Permit(LOG)
                        case "UpdatePermit":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    int AgentID = -1;
                                    int RoleID = -1;
                                    List<int> ListRoleID = new List<int>();
                                    string[] subParameter = subValue[1].Split(',');
                                    int count = subParameter.Length;
                                    if (count > 0)
                                    {
                                        int.TryParse(subParameter[0], out AgentID);
                                        for (int i = 1; i < count; i++)
                                        {
                                            int.TryParse(subParameter[i], out RoleID);
                                            ListRoleID.Add(RoleID);
                                        }
                                    }
                                    resultUpdate = TradingServer.Facade.FacadeUpdatePermit(AgentID, ListRoleID);

                                    #region INSERT SYSTEM LOG
                                    ////INSERT SYSTEM LOG
                                    ////'2222': manager configuration has been changed 
                                    //string status = "[uncomplete]";

                                    //if (resultUpdate)
                                    //    status = "[complete]";

                                    //string content = "'" + code + "': manager configuration has been changed " + status;
                                    //string comment = "[update agent]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion

                                    Facade.FacadeSendNoticeManagerChangeAgent(3, AgentID);
                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update IAgentSecurity
                        case "UpdateIAgentSecurity":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        bool resultUpdate = false;
                                        List<Business.IAgentSecurity> newListIAgentSecurity = new List<IAgentSecurity>();
                                        newListIAgentSecurity = this.ExtractionIAgentSecurity(subValue[1]);
                                        resultUpdate = TradingServer.Facade.FacadeUpdateIAgentSecurity(newListIAgentSecurity);
                                        StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update IAgentGroup
                        case "UpdateIAgentGroup":
                            {
                                if (!string.IsNullOrEmpty(subValue[1]))
                                {
                                    bool resultUpdate = false;
                                    int AgentID = -1;
                                    int InvestorGroupID = -1;
                                    string[] subParameter = subValue[1].Split(',');
                                    int count = subParameter.Length;
                                    List<int> ListInvestorID = new List<int>();
                                    if (count > 0)
                                    {
                                        int.TryParse(subParameter[0], out AgentID);
                                        for (int i = 1; i < count; i++)
                                        {
                                            int.TryParse(subParameter[i], out InvestorGroupID);
                                            ListInvestorID.Add(InvestorGroupID);
                                        }
                                    }
                                    resultUpdate = TradingServer.Facade.FacadeUpdateIAgentGroup(AgentID, ListInvestorID);
                                    Facade.FacadeSendNoticeManagerChangeAgent(2, AgentID);
                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                            }
                            break;
                        #endregion

                        #region Update Investor Profile
                        case "UpdateInvestorProfile":
                            {
                                //Business.InvestorProfile Result = new InvestorProfile();
                                //Result = this.ExtractInvestorProfile(subValue[1]);
                                //TradingServer.Facade.FacadeUpdateInvestorProfile(Result);
                                StringResult = "";
                            }
                            break;
                        #endregion

                        #region Update IGroupSecurityConfig
                        case "UpdateIGroupSecurityConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);

                                    int countResult = Result.Count;
                                    for (int j = 0; j < countResult; j++)
                                    {
                                        resultUpdate = Facade.FacadeUpdateIGroupSecurityConfig(Result[j]);
                                    }

                                    Business.Market.SendNotifyToClient("USTC0532434", 2, -1);

                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update IGroupSymbolConfig
                        case "UpdateIGroupSymbolConfig":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultUpdate = false;
                                    List<Business.ParameterItem> Result = new List<ParameterItem>();
                                    Result = this.ExtractParameterItem(subValue[1]);

                                    int countResult = Result.Count;
                                    for (int j = 0; j < countResult; j++)
                                    {
                                        resultUpdate = Facade.FacadeUpdateIGroupSymbolConfig(Result[j]);
                                    }
                                    StringResult = subValue[0] + "$" + resultUpdate.ToString();
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region Update Online Command(Open Trade)(LOG)
                        case "UpdateOnlineCommandByManager":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    if (checkRule)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 13)
                                        {
                                            #region MapValue
                                            int InvestorID = -1;
                                            int CommandID = -1;
                                            double Commission = -1;
                                            DateTime ExpTime;
                                            double OpenPrice = -1;
                                            DateTime OpenTime;
                                            double StopLoss = -1;
                                            double Swap = -1;
                                            double TakeProfit = -1;
                                            double taxes = 0;
                                            double agentCommission = 0;

                                            int.TryParse(subParameter[0], out InvestorID);
                                            int.TryParse(subParameter[1], out CommandID);
                                            double.TryParse(subParameter[2], out Commission);

                                            DateTime.TryParse(subParameter[3], out ExpTime);
                                            double.TryParse(subParameter[4], out OpenPrice);
                                            DateTime.TryParse(subParameter[5], out OpenTime);
                                            double.TryParse(subParameter[6], out StopLoss);
                                            double.TryParse(subParameter[7], out Swap);
                                            double.TryParse(subParameter[8], out TakeProfit);
                                            double.TryParse(subParameter[10], out taxes);
                                            //comment
                                            //double.TryParse(subParameter[12], out agentCommission);

                                            Business.OpenTrade tempOpenTrade = new OpenTrade();
                                            tempOpenTrade = TradingServer.Facade.FacadeFindOpenTradeInCommandEx(CommandID);

                                            string strOpenPriceBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.OpenPrice.ToString(), tempOpenTrade.Symbol.Digit);
                                            string strTakeProfitBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.TakeProfit.ToString(), 2);
                                            string strStopLossBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.StopLoss.ToString(), 2);
                                            string strCommissionBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Commission.ToString(), 2);
                                            string strAgentCommissionBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.AgentCommission.ToString(), 2);
                                            string strSwapBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Swap.ToString(), 2);
                                            #endregion
                                            bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempOpenTrade.Investor.InvestorGroupInstance.InvestorGroupID);
                                            if (checkGroup)
                                            {
                                                bool ResultUpdate = false;
                                                ResultUpdate = TradingServer.Facade.FacadeUpdateOnlineCommand(InvestorID, CommandID, Commission, ExpTime, OpenPrice, OpenTime,
                                                    StopLoss, Swap, TakeProfit, subParameter[9], taxes, subParameter[11], agentCommission, tempOpenTrade.Size);
                                                if (ResultUpdate)
                                                    StringResult = subValue[0] + "$" + ResultUpdate + "," + "MCM001";
                                                else StringResult = subValue[0] + "$" + ResultUpdate + "," + "MCM004";

                                                Business.Investor tempInvestor = new Investor();
                                                tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(InvestorID);

                                                #region INSERT SYSTEM LOG
                                                //'2222': account '9789300', modify order #11346382 sell 1.00 XAUUSD at 1504.85 sl: 1506.55 tp: 1503.85
                                                //INSERT SYSTEM LOG
                                                if (ResultUpdate)
                                                {
                                                    string comment = "[update order]";
                                                    string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(tempOpenTrade.Type.ID);
                                                    string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Size.ToString(), 2);
                                                    string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(OpenPrice.ToString(), tempOpenTrade.Symbol.Digit);
                                                    string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(StopLoss.ToString(), tempOpenTrade.Symbol.Digit);
                                                    string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(TakeProfit.ToString(), tempOpenTrade.Symbol.Digit);
                                                    string strCommissionAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Commission.ToString(), 2);
                                                    string strAgentCommissionAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(agentCommission.ToString(), 2);
                                                    string strSwapAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Swap.ToString(), 2);

                                                    //string content = "'" + code + "': account '" + tempInvestor.Code + "' modify order #" + tempOpenTrade.CommandCode +
                                                    //    " " + mode + " " + size + " " + tempOpenTrade.Symbol.Name + " at " + openPrice + " sl: " + stopLoss + " tp: " + takeProfit;

                                                    string content = "'" + code + "': open order #" + tempOpenTrade.CommandCode + " for '" + tempInvestor.Code + "' modified - " +
                                                        mode + " " + size + " " + tempOpenTrade.Symbol.Name + " at " + strOpenPriceBefore + " - tp: " + strTakeProfitBefore + " - sl: " + strStopLossBefore +
                                                            " - cm: " + strCommissionBefore + " - sw: " + strSwapBefore + " -> " +
                                                            mode + " " + size + " " + tempOpenTrade.Symbol.Name + " at " + openPrice + " - tp: " + takeProfit + " - sl: " + stopLoss + " - cm: " +
                                                            strCommissionAfter + " - sw: " + strSwapAfter;

                                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$" + false + "," + "MCM006";
                                                string content = "'" + code + "': update order failed(not enough rights)";
                                                string comment = "[update order]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                            }
                                        }
                                        else
                                        {
                                            StringResult = subValue[0] + "$" + false + "," + "MCM004";
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + "," + "MCM006";
                                        string content = "'" + code + "': update order failed(not enough rights)";
                                        string comment = "[update order]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false + "," + "MCM005";
                                    string content = "'" + code + "': update order failed(invalid ip)";
                                    string comment = "[update order]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region Update Candle Online
                        case "UpdateCandleOnline":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length > 0)
                                {
                                    int ID = -1;
                                    DateTime Time;
                                    int Volume = -1;
                                    double Open = -1;
                                    double Close = -1;
                                    double High = -1;
                                    double Low = -1;
                                    double openAsk = -1;
                                    double highAsk = -1;
                                    double lowAsk = -1;
                                    double closeAsk = -1;
                                    int TimeFrame = -1;

                                    int.TryParse(subParameter[0], out ID);
                                    DateTime.TryParse(subParameter[1], out Time);
                                    int.TryParse(subParameter[2], out Volume);
                                    double.TryParse(subParameter[3], out Open);
                                    double.TryParse(subParameter[4], out Close);
                                    double.TryParse(subParameter[5], out High);
                                    double.TryParse(subParameter[6], out Low);
                                    int.TryParse(subParameter[7], out TimeFrame);
                                    double.TryParse(subParameter[8], out openAsk);
                                    double.TryParse(subParameter[9], out highAsk);
                                    double.TryParse(subParameter[10], out lowAsk);
                                    double.TryParse(subParameter[11], out closeAsk);

                                    ProcessQuoteLibrary.Business.Candles objCandles = new ProcessQuoteLibrary.Business.Candles();
                                    objCandles.Close = Close;
                                    objCandles.CloseAsk = closeAsk;
                                    objCandles.High = High;
                                    objCandles.HighAsk = highAsk;
                                    objCandles.ID = ID;
                                    objCandles.Low = Low;
                                    objCandles.LowAsk = lowAsk;
                                    objCandles.Open = Open;
                                    objCandles.OpenAsk = openAsk;
                                    objCandles.Time = Time;
                                    objCandles.TimeFrame = TimeFrame;
                                    objCandles.Volume = Volume;

                                    bool ResultUpdate = false;
                                    ResultUpdate = ProcessQuoteLibrary.Business.QuoteProcess.UpdateCandleOnline(objCandles);

                                    //#region INSERT SYSTEM LOG
                                    ////INSERT SYSTEM LOG
                                    ////'2222': XAUUSD M1 1 bars updated
                                    //string status = "[uncomplete]";

                                    //if (ResultUpdate)
                                    //    status = "[complete]";

                                    //string content = "'" + code + "': " +  + status;
                                    //string comment = "[update agent]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    //#endregion

                                    StringResult = subValue[0] + "$" + ResultUpdate;
                                }
                            }
                            break;
                        #endregion

                        #region UPDATE ORDER IN DATABASE(COMMENT)
                        case "UpdateOrderData":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                bool Result = false;
                                Business.OpenTrade newOpenTrade = new OpenTrade();
                                if (subParameter.Length > 0)
                                {
                                    switch (subParameter[subParameter.Length - 1])
                                    {
                                        case "OPT001":
                                            {
                                                double closePrice = 0;
                                                double.TryParse(subParameter[0], out closePrice);
                                                bool resultUpdate = false;

                                                if (closePrice > 0)
                                                {
                                                    newOpenTrade = this.ConvertStringToOpenTrade(subParameter);
                                                    //resultUpdate = TradingServer.Facade.FacadeUpdateOnlineCommand(newOpenTrade.Investor.InvestorID, newOpenTrade.ID, newOpenTrade.Commission,
                                                    //    newOpenTrade.ExpTime, newOpenTrade.OpenPrice, newOpenTrade.OpenTime, newOpenTrade.StopLoss, newOpenTrade.Swap,
                                                    //    newOpenTrade.TakeProfit, newOpenTrade.Symbol.Name);
                                                }
                                                else
                                                {
                                                    newOpenTrade = this.ConvertStringToOpenTrade(subParameter);
                                                    if (newOpenTrade.Type.ID == 1 || newOpenTrade.Type.ID == 5 || newOpenTrade.Type.ID == 7 || newOpenTrade.Type.ID == 9 || newOpenTrade.Type.ID == 11)
                                                    {
                                                        newOpenTrade.ClosePrice = newOpenTrade.Symbol.TickValue.Bid;
                                                    }
                                                    else
                                                    {
                                                        newOpenTrade.ClosePrice = (Business.Symbol.ConvertNumberPip(newOpenTrade.Symbol.Digit, newOpenTrade.SpreaDifferenceInOpenTrade) + newOpenTrade.Symbol.TickValue.Ask);
                                                    }

                                                    //resultUpdate = TradingServer.Facade.FacadeUpdateOnlineCommand(newOpenTrade.Investor.InvestorID, newOpenTrade.ID, newOpenTrade.Commission,
                                                    //    newOpenTrade.ExpTime, newOpenTrade.OpenPrice, newOpenTrade.OpenTime, newOpenTrade.StopLoss, newOpenTrade.Swap,
                                                    //    newOpenTrade.TakeProfit, newOpenTrade.Symbol.Name);
                                                }

                                                if (resultUpdate)
                                                {
                                                    //MAP COMMAND UPDATE TO CLIENT
                                                    string Message = "UpdateCommand$True,UPDATE COMMAND COMPLETE," + newOpenTrade.ID + "," +
                                                                newOpenTrade.Investor.InvestorID + "," + newOpenTrade.Symbol.Name + "," +
                                                                newOpenTrade.Size + "," + false + "," + newOpenTrade.OpenTime + "," +
                                                                newOpenTrade.OpenPrice + "," + newOpenTrade.StopLoss + "," +
                                                                newOpenTrade.TakeProfit + "," + newOpenTrade.ClosePrice + "," +
                                                                newOpenTrade.Commission + "," + newOpenTrade.Swap + "," +
                                                                newOpenTrade.Profit + "," + "Comment," + newOpenTrade.ID + "," +
                                                                newOpenTrade.Type.Name + "," + 1 + "," + newOpenTrade.ExpTime + "," +
                                                                newOpenTrade.ClientCode + "," + newOpenTrade.CommandCode + "," +
                                                                newOpenTrade.IsHedged + "," + newOpenTrade.Type.ID + "," +
                                                                newOpenTrade.Margin + ",Update";
                                                }

                                                StringResult = subValue[0] + "$" + resultUpdate;
                                            }
                                            break;
                                        case "CMH01":
                                            {
                                                newOpenTrade = ConvertStringToOpenTrade(subParameter);
                                                bool resultUpdate = TradingServer.Facade.FacadeUpdateCommandHistory(newOpenTrade);

                                                StringResult = subValue[0] + "$" + resultUpdate;
                                            }
                                            break;
                                        case "IAL01":
                                            {
                                                Business.InvestorAccountLog newInvestorAccountLog = new InvestorAccountLog();
                                                int ID = 0;
                                                //deal ID
                                                int InvestorID = 0;
                                                //name
                                                DateTime Date;
                                                //comment
                                                double Amount = 0;
                                                //code
                                                int.TryParse(subParameter[0], out ID);
                                                int.TryParse(subParameter[2], out InvestorID);
                                                DateTime.TryParse(subParameter[4], out Date);
                                                double.TryParse(subParameter[6], out Amount);

                                                newInvestorAccountLog.ID = ID;
                                                newInvestorAccountLog.DealID = subParameter[1];
                                                newInvestorAccountLog.InvestorID = InvestorID;
                                                newInvestorAccountLog.Name = subParameter[3];
                                                newInvestorAccountLog.Date = Date;
                                                newInvestorAccountLog.Comment = subParameter[5];
                                                newInvestorAccountLog.Amount = Amount;
                                                newInvestorAccountLog.Code = subParameter[6];

                                                bool resultUpdate = TradingServer.Facade.FacadeUpdateInvestorAccountLog(newInvestorAccountLog);
                                                StringResult = subValue[0] + "$" + resultUpdate;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region UPDATE COMMAND HISTORY(UPDATE COMMAND HISTORY, UPDATE DEPOSIT....)
                        case "UpdateCommandHistory":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    if (checkRule)
                                    {
                                        bool resultUpdate = false;
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 18)
                                        {
                                            #region MAP STRING TO OBJECT
                                            Business.OpenTrade newOpenTrade = new OpenTrade();

                                            int commandTypeID, commandHistoryID;
                                            DateTime openTime, closeTime, expTime;
                                            double openPrice, closePrice, profit, swap, commission, size, stopLoss, takeProfit, taxes, agentCommission;

                                            int.TryParse(subParameter[0], out commandTypeID);
                                            DateTime.TryParse(subParameter[1], out openTime);
                                            double.TryParse(subParameter[2], out openPrice);
                                            DateTime.TryParse(subParameter[3], out closeTime);
                                            double.TryParse(subParameter[4], out closePrice);
                                            double.TryParse(subParameter[5], out profit);
                                            double.TryParse(subParameter[6], out swap);
                                            double.TryParse(subParameter[7], out commission);
                                            DateTime.TryParse(subParameter[8], out expTime);
                                            double.TryParse(subParameter[9], out size);
                                            double.TryParse(subParameter[10], out stopLoss);
                                            double.TryParse(subParameter[11], out takeProfit);

                                            //int.TryParse(subParameter[12], out symbolID);
                                            double.TryParse(subParameter[13], out taxes);
                                            //string comment  = subparmeter[14]
                                            double.TryParse(subParameter[15], out agentCommission);
                                            int.TryParse(subParameter[16], out commandHistoryID);
                                            //string InvestorCode == .....

                                            newOpenTrade.AgentCommission = agentCommission;
                                            newOpenTrade.ClosePrice = closePrice;
                                            newOpenTrade.CloseTime = closeTime;
                                            newOpenTrade.Comment = subParameter[14];
                                            newOpenTrade.Commission = commission;
                                            newOpenTrade.ExpTime = expTime;
                                            newOpenTrade.ID = commandHistoryID;
                                            newOpenTrade.OpenPrice = openPrice;
                                            newOpenTrade.OpenTime = openTime;
                                            newOpenTrade.Type = new TradeType();
                                            newOpenTrade.Type.ID = commandTypeID;
                                            newOpenTrade.Profit = profit;
                                            newOpenTrade.Size = size;
                                            newOpenTrade.StopLoss = stopLoss;
                                            newOpenTrade.Swap = swap;
                                            newOpenTrade.TakeProfit = takeProfit;
                                            newOpenTrade.Taxes = taxes;

                                            newOpenTrade.Investor = TradingServer.Facade.FacadeFindInvestor(subParameter[17]);
                                            #endregion

                                            if (commandTypeID == 1 || commandTypeID == 2 || commandTypeID == 11 || commandTypeID == 12)
                                            {
                                                #region FILL INSTANT SYMBOL
                                                if (Business.Market.SymbolList != null)
                                                {
                                                    int countSymbol = Business.Market.SymbolList.Count;
                                                    for (int j = 0; j < countSymbol; j++)
                                                    {
                                                        if (Business.Market.SymbolList[j].Name.Trim() == subParameter[12].Trim())
                                                        {
                                                            newOpenTrade.Symbol = Business.Market.SymbolList[j];
                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);
                                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);
                                                newOpenTrade.Profit = Math.Round(newOpenTrade.Profit, 2);
                                            }

                                            //resultUpdate = TradingServer.Facade.FacadeUpdateOnlineCommand(0, commandHistoryID, commission, expTime, openPrice, openTime, stopLoss,
                                            //    swap, takeProfit, subParameter[12], taxes, subParameter[14], agentCommission, size);

                                            Business.OpenTrade tempOpenTrade = TradingServer.Facade.FacadeGetCommandHistoryByCommandID(commandHistoryID);

                                            #region BUIL STRING CHANGER OF ORDER
                                            string change = string.Empty;
                                            string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(tempOpenTrade.Type.ID);
                                            string strOpenPriceBefore = string.Empty;
                                            string strOpenPriceAfter = string.Empty;
                                            string strClosePriceBefore = string.Empty;
                                            string strClosePriceAfter = string.Empty;

                                            if (tempOpenTrade.Symbol != null)
                                            {
                                                strOpenPriceBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.OpenPrice.ToString(), tempOpenTrade.Symbol.Digit);
                                                strOpenPriceAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openPrice.ToString(), tempOpenTrade.Symbol.Digit);
                                                strClosePriceBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.ClosePrice.ToString(), tempOpenTrade.Symbol.Digit);
                                                strClosePriceAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(closePrice.ToString(), tempOpenTrade.Symbol.Digit);
                                            }
                                            else
                                            {
                                                strOpenPriceBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.OpenPrice.ToString(), 5);
                                                strOpenPriceAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(openPrice.ToString(), 5);
                                                strClosePriceBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.ClosePrice.ToString(), 5);
                                                strClosePriceAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(closePrice.ToString(), 5);
                                            }

                                            string strStopLossBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.StopLoss.ToString(), 2);
                                            string strStopLossAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(stopLoss.ToString(), 2);
                                            string strTakeProfitBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.TakeProfit.ToString(), 2);
                                            string strTakeProfitAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(takeProfit.ToString(), 2);
                                            string strSwapBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Swap.ToString(), 2);
                                            string strSwapAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(swap.ToString(), 2);
                                            string strLotsBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Size.ToString(), 2);
                                            string strLotsAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(size.ToString(), 2);
                                            string strCommissionBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Commission.ToString(), 2);
                                            string strCommissionAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(commission.ToString(), 2);
                                            string strProfitBefore = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(tempOpenTrade.Profit.ToString(), 2);
                                            string strProfitAfter = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Profit.ToString(), 2);
                                            string strSize = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(size.ToString(), 2);
                                            #endregion

                                            #region UPDATE COMMAND IN DATABASE AND FIX BALANCE
                                            if (Business.Market.InvestorList != null)
                                            {
                                                int count = Business.Market.InvestorList.Count;
                                                for (int i = 0; i < count; i++)
                                                {
                                                    if (Business.Market.InvestorList[i].InvestorID == newOpenTrade.Investor.InvestorID)
                                                    {
                                                        double totalProfit = newOpenTrade.Profit + newOpenTrade.Commission + newOpenTrade.Swap;
                                                        double profitHistory = tempOpenTrade.Profit + tempOpenTrade.Commission + tempOpenTrade.Swap;
                                                        double deltaProfit = profitHistory - totalProfit;

                                                        bool isValid = false;
                                                        if (deltaProfit != 0)
                                                        {
                                                            #region SWITCH COMMAND TYPE
                                                            switch (tempOpenTrade.Type.ID)
                                                            {
                                                                case 13:
                                                                    {
                                                                        double tempBalance = Business.Market.InvestorList[i].Balance - deltaProfit;
                                                                        if (tempBalance > 0 && newOpenTrade.Profit > 0)
                                                                        {
                                                                            Business.Market.InvestorList[i].Balance -= deltaProfit;
                                                                            TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);
                                                                            isValid = true;
                                                                        }
                                                                    }
                                                                    break;

                                                                case 14:
                                                                    {
                                                                        double tempBalance = Business.Market.InvestorList[i].Balance + deltaProfit;
                                                                        if (tempBalance > 0 && newOpenTrade.Profit > 0)
                                                                        {
                                                                            Business.Market.InvestorList[i].Balance += deltaProfit;
                                                                            TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);
                                                                            isValid = true;
                                                                        }
                                                                    }
                                                                    break;

                                                                case 15:
                                                                    {
                                                                        double tempBalance = Business.Market.InvestorList[i].Credit - deltaProfit;
                                                                        if (tempBalance > 0 && newOpenTrade.Profit > 0)
                                                                        {
                                                                            Business.Market.InvestorList[i].Credit -= deltaProfit;
                                                                            TradingServer.Facade.FacadeUpdateCredit(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Credit);
                                                                            isValid = true;
                                                                        }
                                                                    }
                                                                    break;

                                                                case 16:
                                                                    {
                                                                        double tempBalance = Business.Market.InvestorList[i].Credit + deltaProfit;
                                                                        if (tempBalance > 0 && newOpenTrade.Profit > 0)
                                                                        {
                                                                            Business.Market.InvestorList[i].Credit += deltaProfit;
                                                                            TradingServer.Facade.FacadeUpdateCredit(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Credit);
                                                                            isValid = true;
                                                                        }
                                                                    }
                                                                    break;

                                                                default:
                                                                    {
                                                                        Business.Market.InvestorList[i].Balance -= deltaProfit;
                                                                        TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);
                                                                        isValid = true;
                                                                    }
                                                                    break;
                                                            }
                                                            #endregion

                                                            //SEND NOTIFY TO MANAGER with type =3 then balance and credit
                                                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[i]);
                                                        }

                                                        if (isValid)
                                                        {
                                                            #region CALCUALTION AGENT COMMISSION
                                                            double deltaAgentCommission = agentCommission - tempOpenTrade.AgentCommission;

                                                            if (deltaAgentCommission != 0)
                                                            {
                                                                if (Business.Market.InvestorList != null)
                                                                {
                                                                    int countInvestor = Business.Market.InvestorList.Count;
                                                                    for (int j = 0; j < countInvestor; j++)
                                                                    {
                                                                        if (Business.Market.InvestorList[j].Code == tempOpenTrade.Investor.AgentID)
                                                                        {
                                                                            Business.Market.InvestorList[j].Balance += deltaAgentCommission;
                                                                            TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[j].InvestorID, Business.Market.InvestorList[j].Balance);
                                                                            //SEND NOTIFY TO MANAGER with type =3 then balance and credit
                                                                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Business.Market.InvestorList[j]);

                                                                            if (Business.Market.InvestorList[j].IsOnline)
                                                                            {
                                                                                string Message = "GetNewBalance";

                                                                                int countInvestorOnline = Business.Market.InvestorList[j].CountInvestorOnline(Business.Market.InvestorList[j].InvestorID);
                                                                                if (countInvestorOnline > 0)
                                                                                    Business.Market.InvestorList[j].ClientCommandQueue.Add(Message);
                                                                            }
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            if (Business.Market.InvestorList[i].ClientCommandQueue == null)
                                                                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

                                                            int countOnline = Business.Market.InvestorList[i].CountInvestorOnline(Business.Market.InvestorList[i].InvestorID);
                                                            if (countOnline > 0)
                                                                Business.Market.InvestorList[i].ClientCommandQueue.Add("UCH1478963");
                                                        }

                                                        //UPDATE COMMAND HISTORY IN DATABASE
                                                        resultUpdate = TradingServer.Facade.FacadeUpdateCommandHistory(newOpenTrade);

                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            StringResult = subValue[0] + "$" + resultUpdate;
                                            #region LOG
                                            //'2222': the existent order has been overwritten
                                            string status = "[Failed]";
                                            if (resultUpdate)
                                                status = "[Success]";

                                            if (tempOpenTrade.Type.ID == 1 || tempOpenTrade.Type.ID == 2 || tempOpenTrade.Type.ID == 11 || tempOpenTrade.Type.ID == 12)
                                            {
                                                string content = "'" + code + "': close order #" + tempOpenTrade.CommandCode + " for " + tempOpenTrade.Investor.Code +
                                                                " modified: " + mode + " " + strSize + " " + tempOpenTrade.Symbol.Name + " at " + strOpenPriceBefore + " - cp: " +
                                                                strClosePriceBefore + " - tp: " + strTakeProfitBefore + " - sl: " + strStopLossBefore + " - cm: " + strCommissionBefore +
                                                                " - sw: " + strSwapBefore + " - profit: " + strProfitBefore + " -> " + mode + " " + strSize + " " + tempOpenTrade.Symbol.Name +
                                                                " at " + strOpenPriceAfter + " - cp: " + strClosePriceAfter + " - tp: " + strTakeProfitAfter + " - sl: " + strStopLossAfter +
                                                                " - cm: " + strCommissionAfter + " - sw: " + strSwapAfter + " - profit: " + strProfitAfter + status;
                                                string comment = "[update order]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                            }
                                            else
                                            {
                                                switch (tempOpenTrade.Type.ID)
                                                {
                                                    case 13:
                                                        {
                                                            string content = "'" + code + "': close order #" + tempOpenTrade.CommandCode + " for " + tempOpenTrade.Investor.Code +
                                                                " modified - deposit " + strSize + " at " + strOpenPriceBefore + " - cp: " + strClosePriceBefore +
                                                                " - profit: " + strProfitBefore + " -> deposit " + strSize + " at " + strOpenPriceAfter + " - profit: " +
                                                                strProfitAfter + status;
                                                            string comment = "[update order]";
                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                        }
                                                        break;
                                                    case 14:
                                                        {
                                                            string content = "'" + code + "': close order #" + tempOpenTrade.CommandCode + " for " + tempOpenTrade.Investor.Code +
                                                                " modified - withdrawal " + strSize + " at " + strOpenPriceBefore + " - cp: " + strClosePriceBefore +
                                                                " - profit: " + strProfitBefore + " -> withdrawal " + strSize + " at " + strOpenPriceAfter + " - profit: " +
                                                                strProfitAfter + status;
                                                            string comment = "[update order]";
                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                        }
                                                        break;
                                                    case 15:
                                                        {
                                                            string content = "'" + code + "': close order #" + tempOpenTrade.CommandCode + " for " + tempOpenTrade.Investor.Code +
                                                                " modified - credit in " + strSize + " at " + strOpenPriceBefore + " - cp: " + strClosePriceBefore +
                                                                " - profit: " + strProfitBefore + " -> credit in " + strSize + " at " + strOpenPriceAfter + " - profit: " +
                                                                strProfitAfter + status;
                                                            string comment = "[update order]";
                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                        }
                                                        break;
                                                    case 16:
                                                        {
                                                            string content = "'" + code + "': close order #" + tempOpenTrade.CommandCode + " for " + tempOpenTrade.Investor.Code +
                                                                " modified - credit out " + strSize + " at " + strOpenPriceBefore + " - cp: " + strClosePriceBefore +
                                                                " - profit: " + strProfitBefore + " -> credit out " + strSize + " at " + strOpenPriceAfter + " - profit: " +
                                                                strProfitAfter + status;
                                                            string comment = "[update order]";
                                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                                        }
                                                        break;
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$MCM006";
                                        string content = "'" + code + "': update order failed(not enough rights)";
                                        string comment = "[update order]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                    string content = "'" + code + "': update order failed(invalid ip)";
                                    string comment = "[update order]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        #region DELETE COMMAND IN HISTORY(DELETE COMMAND, DELETE DEPOSIT)
                        case "DeleteCommandHistory":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    bool resultDelete = false;
                                    int commandHistoryID = 0;

                                    string[] subParameter = subValue[1].Split(',');

                                    int.TryParse(subValue[1], out commandHistoryID);

                                    Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandHistoryByCommandID(commandHistoryID);

                                    //if (newOpenTrade == null)
                                    //    return StringResult = subValue[0] + "$" + false;
                                    bool isDelete = false;
                                    if (newOpenTrade != null)
                                    {
                                        if (newOpenTrade.ID > 0)
                                        {
                                            #region DELETE COMMAND HISTORY
                                            #region FIX BALANCE ACCOUNT
                                            if (newOpenTrade.Type != null)
                                            {
                                                double swap = newOpenTrade.Swap;

                                                switch (newOpenTrade.Type.ID)
                                                {
                                                    #region SPOT COMMAND
                                                    case 1:
                                                        this.FixBalanceAccount(newOpenTrade.Profit, swap, newOpenTrade.Commission, newOpenTrade.Investor.InvestorID, newOpenTrade.AgentCommission);
                                                        break;

                                                    case 2:
                                                        this.FixBalanceAccount(newOpenTrade.Profit, swap, newOpenTrade.Commission, newOpenTrade.Investor.InvestorID, newOpenTrade.AgentCommission);
                                                        break;
                                                    #endregion

                                                    #region FUTURE COMMAND
                                                    case 11:
                                                        this.FixBalanceAccount(newOpenTrade.Profit, swap, newOpenTrade.Commission, newOpenTrade.Investor.InvestorID, newOpenTrade.AgentCommission);
                                                        break;

                                                    case 12:
                                                        this.FixBalanceAccount(newOpenTrade.Profit, swap, newOpenTrade.Commission, newOpenTrade.Investor.InvestorID, newOpenTrade.AgentCommission);
                                                        break;
                                                    #endregion

                                                    #region DEPOSIT
                                                    case 13:
                                                        {
                                                            if (Business.Market.InvestorList != null)
                                                            {
                                                                int count = Business.Market.InvestorList.Count;
                                                                for (int i = 0; i < count; i++)
                                                                {
                                                                    if (Business.Market.InvestorList[i].InvestorID == newOpenTrade.Investor.InvestorID)
                                                                    {
                                                                        Business.Market.InvestorList[i].Balance -= newOpenTrade.Profit;

                                                                        TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    #endregion

                                                    #region WITHDRAWL
                                                    case 14:
                                                        {
                                                            if (Business.Market.InvestorList != null)
                                                            {
                                                                int count = Business.Market.InvestorList.Count;
                                                                for (int i = 0; i < count; i++)
                                                                {
                                                                    if (Business.Market.InvestorList[i].InvestorID == newOpenTrade.Investor.InvestorID)
                                                                    {
                                                                        Business.Market.InvestorList[i].Balance += newOpenTrade.Profit;

                                                                        TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    #endregion

                                                    #region CREDIT IN
                                                    case 15:
                                                        {
                                                            if (Business.Market.InvestorList != null)
                                                            {
                                                                int count = Business.Market.InvestorList.Count;
                                                                for (int i = 0; i < count; i++)
                                                                {
                                                                    if (Business.Market.InvestorList[i].InvestorID == newOpenTrade.Investor.InvestorID)
                                                                    {
                                                                        Business.Market.InvestorList[i].Credit -= newOpenTrade.Profit;

                                                                        TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    #endregion

                                                    #region CREDIT OUT
                                                    case 16:
                                                        {
                                                            if (Business.Market.InvestorList != null)
                                                            {
                                                                int count = Business.Market.InvestorList.Count;
                                                                for (int i = 0; i < count; i++)
                                                                {
                                                                    if (Business.Market.InvestorList[i].InvestorID == newOpenTrade.Investor.InvestorID)
                                                                    {
                                                                        Business.Market.InvestorList[i].Credit += newOpenTrade.Profit;

                                                                        TradingServer.Facade.FacadeUpdateBalance(Business.Market.InvestorList[i].InvestorID, Business.Market.InvestorList[i].Balance);

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    #endregion
                                                }
                                            }
                                            #endregion

                                            //UPDATE ISDELETE IN DATABASE
                                            isDelete = TradingServer.Facade.FacadeUpdateIsDeleteHistory(true, commandHistoryID);
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        //Delete Open Position
                                        newOpenTrade = TradingServer.Facade.FacadeFindOpenTradeInCommandEx(commandHistoryID);

                                        if (newOpenTrade.Symbol == null || newOpenTrade.Investor == null || newOpenTrade.IGroupSecurity == null || newOpenTrade.Type == null)
                                            return StringResult = subValue[0] + "$" + false;

                                        isDelete = TradingServer.Facade.FacadeDeleteOpenTradeByAdmin(commandHistoryID);
                                    }

                                    #region BUILD STRING INSERT LOG
                                    string message = string.Empty;

                                    if (newOpenTrade.Type.ID == 1 || newOpenTrade.Type.ID == 2 || newOpenTrade.Type.ID == 11 || newOpenTrade.Type.ID == 12)
                                    {
                                        #region BUILD STRING INSERT LOG
                                        //'2222': order #2495264 for '2365714' delete - buy 2.00 GBPUSD at 1.48840, profit: 20.00
                                        string mode = string.Empty;
                                        string size = string.Empty;
                                        string openPrice = string.Empty;
                                        string profit = string.Empty;
                                        string swap = string.Empty;
                                        string commission = string.Empty;
                                        size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Size.ToString(), 2);
                                        mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(newOpenTrade.Type.ID);
                                        openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.OpenPrice.ToString(), newOpenTrade.Symbol.Digit);
                                        profit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Profit.ToString(), 2);
                                        swap = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Swap.ToString(), 2);
                                        commission = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Commission.ToString(), 2);
                                        message = "'" + code + "': order #" + newOpenTrade.CommandCode + " for '" + newOpenTrade.Investor.Code + "' delete - " +
                                            mode + " " + size + " " + newOpenTrade.Symbol.Name + " at " + openPrice + ", profit: " + profit + " - swap: " +
                                            swap + "- commission: " + commission;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region BUILD STRING INSERT LOG
                                        //'2222': changed balance #14051372 - 100000.00 for ' 90244233' - ' Deposit'
                                        string mode = string.Empty;
                                        string profit = string.Empty;
                                        switch (newOpenTrade.Type.ID)
                                        {
                                            case 13:
                                                mode = "Deposit";
                                                break;

                                            case 14:
                                                mode = "Withdrawal";
                                                break;

                                            case 15:
                                                mode = "Credit In";
                                                break;

                                            case 16:
                                                mode = "Credit Out";
                                                break;
                                        }
                                        profit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(newOpenTrade.Profit.ToString(), 2);
                                        message = "'" + code + "': changed balance #" + newOpenTrade.CommandCode + " - " + profit + " for '" +
                                            newOpenTrade.Investor.Code + "'" + " - '" + mode + "'";
                                        #endregion
                                    }

                                    TradingServer.Facade.FacadeAddNewSystemLog(5, message, "[delete admin order]", ipAddress, code);
                                    #endregion

                                    #region SEND COMMAND PREFESH DATA CLIENT AND MANAGER
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, newOpenTrade.Investor);
                                    TradingServer.Business.Market.SendNotifyToClient("IAC04332451", 3, newOpenTrade.Investor.InvestorID);
                                    #endregion

                                    #region COMMENT CODE(2-2-2012)
                                    //Business.Investor tempInvestor = new Investor();
                                    //if (Business.Market.InvestorList != null)
                                    //{
                                    //    int count = Business.Market.InvestorList.Count;
                                    //    for (int i = 0; i < count; i++)
                                    //    {
                                    //        if (Business.Market.InvestorList[i].InvestorID == newOpenTrade.Investor.InvestorID)
                                    //        {
                                    //            tempInvestor = Business.Market.InvestorList[i];

                                    //            break;
                                    //        }
                                    //    }
                                    //}

                                    //resultDelete = TradingServer.Facade.FacadeDeleteCommandHistory(commandHistoryID);

                                    //if (!resultDelete)
                                    //{
                                    //    if (Business.Market.CommandExecutor != null)
                                    //    {
                                    //        int countCommand = Business.Market.CommandExecutor.Count;
                                    //        for (int j = 0; j < countCommand; j++)
                                    //        {
                                    //            if (Business.Market.CommandExecutor[j].ID == commandHistoryID)
                                    //            {
                                    //                resultDelete = TradingServer.Facade.FacadeDeleteOpenTradeByManager(commandHistoryID);

                                    //                if (Business.Market.CommandExecutor[j].Investor.ClientCommandQueue == null)
                                    //                    Business.Market.CommandExecutor[j].Investor.ClientCommandQueue = new List<string>();

                                    //                Business.Market.CommandExecutor[j].Investor.ClientCommandQueue.Add("COD24680");

                                    //                break;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    if (tempInvestor != null && tempInvestor.InvestorID > 0)
                                    //    {
                                    //        if (tempInvestor.ClientCommandQueue == null)
                                    //            tempInvestor.ClientCommandQueue = new List<string>();

                                    //        tempInvestor.ClientCommandQueue.Add("COD24680");
                                    //    }
                                    //}
                                    #endregion

                                    //StringResult = subValue[0] + "$" + resultDelete;
                                    StringResult = subValue[0] + "$" + isDelete;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$MCM005";
                                }
                            }
                            break;
                        #endregion

                        #region ACTIVE PENDING ORDER
                        case "ActivePendingOrder":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    #region Map Value
                                    bool resultActive = false;
                                    int commandID = 0;
                                    int.TryParse(subValue[1], out commandID);
                                    Business.OpenTrade result = new OpenTrade();
                                    result = TradingServer.Facade.FacadeFindOpenTradeInCommandEx(commandID);

                                    Business.OpenTrade newOpenTrade = new OpenTrade();
                                    newOpenTrade.ClientCode = result.ClientCode;
                                    newOpenTrade.CloseTime = result.CloseTime;
                                    newOpenTrade.ExpTime = result.ExpTime;
                                    newOpenTrade.ID = result.ID;
                                    newOpenTrade.Investor = result.Investor;
                                    newOpenTrade.IsClose = result.IsClose;
                                    newOpenTrade.OpenPrice = result.OpenPrice;
                                    newOpenTrade.OpenTime = result.OpenTime;
                                    newOpenTrade.Size = result.Size;
                                    newOpenTrade.StopLoss = result.StopLoss;
                                    newOpenTrade.Swap = result.Swap;
                                    newOpenTrade.Symbol = result.Symbol;
                                    newOpenTrade.TakeProfit = result.TakeProfit;
                                    newOpenTrade.Type = result.Type;
                                    newOpenTrade.ClosePrice = result.ClosePrice;
                                    newOpenTrade.Margin = result.Margin;
                                    newOpenTrade.CommandCode = result.CommandCode;
                                    newOpenTrade.Commission = result.Commission;
                                    newOpenTrade.IGroupSecurity = result.IGroupSecurity;
                                    #endregion

                                    bool checkRule = Facade.FacadeCheckPermitCommandManagerAndAdmin(code);
                                    bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, result.Investor.InvestorGroupInstance.InvestorGroupID);
                                    if (checkRule && checkGroup)
                                    {
                                        

                                        if (newOpenTrade != null && newOpenTrade.ID > 0)
                                        {
                                            //result.Symbol.MarketAreaRef.UpdateCommand(result);
                                            if (newOpenTrade.Type.ID == 7 || newOpenTrade.Type.ID == 9)
                                            {
                                                #region CASE BUY LIMIT AND BUY STOP
                                                if (newOpenTrade.Symbol.MarketAreaRef != null)
                                                {
                                                    if (newOpenTrade.Symbol.MarketAreaRef.Type != null)
                                                    {
                                                        int countType = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                                                        for (int m = 0; m < countType; m++)
                                                        {
                                                            if (newOpenTrade.Symbol.MarketAreaRef.Type[m].ID == 1)
                                                            {
                                                                newOpenTrade.Type = new TradeType();
                                                                newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[m].ID;
                                                                newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[m].Name;

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            else if (newOpenTrade.Type.ID == 8 || newOpenTrade.Type.ID == 10)
                                            {
                                                #region CASE SELL LIMIT AND SELL STOP
                                                if (newOpenTrade.Symbol.MarketAreaRef != null)
                                                {
                                                    if (newOpenTrade.Symbol.MarketAreaRef.Type != null)
                                                    {
                                                        int countType = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                                                        for (int m = 0; m < countType; m++)
                                                        {
                                                            if (newOpenTrade.Symbol.MarketAreaRef.Type[m].ID == 2)
                                                            {
                                                                newOpenTrade.Type = new TradeType();
                                                                newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[m].ID;
                                                                newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[m].Name;

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }

                                            if (newOpenTrade.Type.ID == 17 || newOpenTrade.Type.ID == 19)
                                            {
                                                if (newOpenTrade.Symbol.MarketAreaRef.Type != null)
                                                {
                                                    int countType = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                                                    for (int m = 0; m < countType; m++)
                                                    {
                                                        if (newOpenTrade.Symbol.MarketAreaRef.Type[m].ID == 11)
                                                        {
                                                            newOpenTrade.Type = new TradeType();
                                                            newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[m].ID;
                                                            newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[m].Name;

                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (newOpenTrade.Type.ID == 18 || newOpenTrade.Type.ID == 20)
                                            {
                                                if (newOpenTrade.Symbol.MarketAreaRef.Type != null)
                                                {
                                                    int countType = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                                                    for (int m = 0; m < countType; m++)
                                                    {
                                                        if (newOpenTrade.Symbol.MarketAreaRef.Type[m].ID == 12)
                                                        {
                                                            newOpenTrade.Type = new TradeType();
                                                            newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[m].ID;
                                                            newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[m].Name;

                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            newOpenTrade.IsProcess = true;
                                            newOpenTrade.Investor.UpdateCommand(newOpenTrade);
                                            resultActive = true;

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            //'2222': account '9789300', activate order #11349528 buy stop 2.00 EURUSD at 1.5625
                                            string comment = "[active pending order]";
                                            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(result.Size.ToString(), 2);
                                            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(result.OpenPrice.ToString(), result.Symbol.Digit);
                                            string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(result.Type.ID);
                                            string content = "'" + code + "': account '" + result.Investor.Code + "' activate order #" + result.CommandCode + " " + mode + " " + size +
                                                " " + result.Symbol.Name + " at " + openPrice;

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                        if(resultActive)
                                            StringResult = subValue[0] + "$" + resultActive + ",MCM001";
                                        else StringResult = subValue[0] + "$" + resultActive + ",MCM004";
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + false + ",MCM006";
                                        string content = "'" + code + "': manager active pending order failed(not enough rights)";
                                        string comment = "[active pending order]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$" + false + ",MCM005";
                                    string content = "'" + code + "': manager active pending order failed(invalid ip)";
                                    string comment = "[active pending order]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, ipAddress, code);
                                }
                            }
                            break;
                        #endregion

                        //Command Select 
                        //
                        #region Select Investor Group
                        case "SelectInvestorGroup":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {                                    
                                    List<Business.InvestorGroup> Result = new List<InvestorGroup>();
                                    //Result = TradingServer.Facade.FacadeGetAllInvestorGroup();
                                    Result = TradingServer.Business.Market.InvestorGroupList;
                                    if (Result != null)
                                    {
                                        int countInvestorGroup = Result.Count;
                                        for (int n = 0; n < countInvestorGroup; n++)
                                        {
                                            temp += Result[n].InvestorGroupID.ToString() + "," + Result[n].Name + "," + Result[n].Owner + "," + Result[n].DefautDeposite.ToString() + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectInvestorGroupByID":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int InvestorGroupID = -1;
                                    int.TryParse(subValue[1], out InvestorGroupID);

                                    Business.InvestorGroup Result = new InvestorGroup();
                                    if (Business.Market.InvestorGroupList != null)
                                    {
                                        int countInvestor = Business.Market.InvestorGroupList.Count;
                                        for (int j = 0; j < countInvestor; j++)
                                        {
                                            if (Business.Market.InvestorGroupList[j].InvestorGroupID == InvestorGroupID)
                                            {
                                                Result = Business.Market.InvestorGroupList[j];
                                                break;
                                            }
                                        }
                                    }
                                    //Result = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(InvestorGroupID);
                                    if (Result != null)
                                    {
                                        temp += Result.InvestorGroupID.ToString() + "," + Result.Name + "," + Result.Owner + "," + Result.DefautDeposite.ToString();
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Investor Group Config
                        case "SelectInvestorGroupConfigByInvestorGroupID":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int InvestorGroupID = -1;
                                    int.TryParse(subValue[1], out InvestorGroupID);

                                    if (Market.InvestorGroupList != null)
                                    {
                                        int countInvestorGroup = Market.InvestorGroupList.Count;
                                        for (int n = 0; n < countInvestorGroup; n++)
                                        {
                                            if (Market.InvestorGroupList[n].InvestorGroupID == InvestorGroupID)
                                            {
                                                if (Market.InvestorGroupList[n].ParameterItems != null)
                                                {
                                                    int countParameterItem = Market.InvestorGroupList[n].ParameterItems.Count;
                                                    for (int m = 0; m < countParameterItem; m++)
                                                    {
                                                        temp += Market.InvestorGroupList[n].ParameterItems[m].ParameterItemID.ToString() + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].SecondParameterID.ToString() + "," + "-1" + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].Name + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].Code + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].BoolValue.ToString() + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].StringValue + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].NumValue + "," +
                                                                Market.InvestorGroupList[n].ParameterItems[m].DateValue.ToString() + "|";
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select IGroup Symbol
                        case "SelectIGroupSymbol":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
                                    Result = Business.Market.IGroupSymbolList;
                                    if (Result != null)
                                    {
                                        int countIGroupSymbol = Result.Count;
                                        for (int n = 0; n < countIGroupSymbol; n++)
                                        {
                                            temp += Result[n].IGroupSymbolID.ToString() + "," + Result[n].SymbolID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "|";
                                        }
                                    }
                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;

                        case "SelectIGroupSymbolByID":
                            {
                                string temp = string.Empty;
                                int IGroupSymbolID = -1;
                                int.TryParse(subValue[1], out IGroupSymbolID);

                                Business.IGroupSymbol Result = new IGroupSymbol();
                                if (Business.Market.IGroupSymbolList != null)
                                {
                                    int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
                                    for (int j = 0; j < countIGroupSymbol; j++)
                                    {
                                        if (Business.Market.IGroupSymbolList[j].IGroupSymbolID == IGroupSymbolID)
                                        {
                                            Result = Business.Market.IGroupSymbolList[j];
                                            break;
                                        }
                                    }
                                }

                                //Result = TradingServer.Facade.FacadeGetIGroupSymbolByIGroupSymbolID(IGroupSymbolID);
                                if (Result != null)
                                {
                                    temp = Result.IGroupSymbolID.ToString() + "," + Result.SymbolID.ToString() + "," + Result.InvestorGroupID.ToString();
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIGroupSymbolBySymbolID":
                            {
                                string temp = string.Empty;
                                int SymbolID = -1;
                                int.TryParse(subValue[1], out SymbolID);

                                List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
                                if (Business.Market.IGroupSymbolList != null)
                                {
                                    int countIGroupSymbol = Business.Market.IGroupSymbolList.Count;
                                    for (int j = 0; j < countIGroupSymbol; j++)
                                    {
                                        if (Business.Market.IGroupSymbolList[j].SymbolID == SymbolID)
                                        {
                                            Result.Add(Business.Market.IGroupSymbolList[j]);
                                        }
                                    }
                                }

                                //Result = TradingServer.Facade.FacadeGetIGroupSymbolBySymbolID(SymbolID);
                                if (Result != null)
                                {
                                    int countIGroupSymbol = Result.Count;
                                    for (int n = 0; n < countIGroupSymbol; n++)
                                    {
                                        temp += Result[n].IGroupSymbolID.ToString() + "," + Result[n].SymbolID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select IGroupSymbolConfig
                        case "SelectIGroupSymbolConfigByIGroupSymbolID":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    int IGroupSymbolID = -1;
                                    int.TryParse(subValue[1], out IGroupSymbolID);
                                    temp = this.GetIGroupSymbolConfigByIGroupSymbolIDInIGroupSymbolList(IGroupSymbolID);
                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;
                        #endregion

                        #region Select IGroup Security
                        case "SelectIGroupSecurity":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
                                    Result = Business.Market.IGroupSecurityList;
                                    if (Result != null)
                                    {
                                        int countIGroupSecurity = Result.Count;
                                        for (int n = 0; n < countIGroupSecurity; n++)
                                        {
                                            temp += Result[n].IGroupSecurityID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "," +
                                                        Result[n].SecurityID.ToString() + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIGroupSecurityByID":
                            {
                                string temp = string.Empty;
                                int IGroupSecurityID = -1;
                                int.TryParse(subValue[1], out IGroupSecurityID);

                                Business.IGroupSecurity Result = new IGroupSecurity();
                                if (Business.Market.IGroupSecurityList != null)
                                {
                                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                    for (int j = 0; j < countIGroupSecurity; j++)
                                    {
                                        if (Business.Market.IGroupSecurityList[j].IGroupSecurityID == IGroupSecurityID)
                                        {
                                            Result = Business.Market.IGroupSecurityList[j];
                                            break;
                                        }
                                    }
                                }

                                if (Result != null)
                                {
                                    temp += Result.IGroupSecurityID.ToString() + "," + Result.InvestorGroupID.ToString() + "," +
                                                    Result.SecurityID.ToString();
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIGroupSecurityBySecurityID":
                            {
                                string temp = string.Empty;
                                int SecurityID = -1;
                                int.TryParse(subValue[1], out SecurityID);

                                List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
                                if (Business.Market.IGroupSecurityList != null)
                                {
                                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                    for (int j = 0; j < countIGroupSecurity; j++)
                                    {
                                        if (Business.Market.IGroupSecurityList[j].SecurityID == SecurityID)
                                        {
                                            Result.Add(Business.Market.IGroupSecurityList[j]);
                                        }
                                    }
                                }
                                //Result = TradingServer.Facade.FacadeGetIGroupSecurityBySecurityID(SecurityID);
                                if (Result != null)
                                {
                                    int countIGroupSecurity = Result.Count;
                                    for (int n = 0; n < countIGroupSecurity; n++)
                                    {
                                        temp += Result[n].IGroupSecurityID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "," +
                                                    Result[n].SecurityID.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIGroupSecurityByInvestorGroupID":
                            {
                                string temp = string.Empty;
                                int InvestorGroupID = -1;
                                int.TryParse(subValue[1], out InvestorGroupID);

                                List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
                                if (Business.Market.IGroupSecurityList != null)
                                {
                                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                    for (int j = 0; j < countIGroupSecurity; j++)
                                    {
                                        if (Business.Market.IGroupSecurityList[j].InvestorGroupID == InvestorGroupID)
                                        {
                                            Result.Add(Business.Market.IGroupSecurityList[j]);
                                        }
                                    }
                                }
                                //Result = TradingServer.Facade.FacadeGetIGroupSecurityByInvestorGroupID(InvestorGroupID);
                                if (Result != null)
                                {
                                    int countIGroupSecurity = Result.Count;
                                    for (int n = 0; n < countIGroupSecurity; n++)
                                    {
                                        temp += Result[n].IGroupSecurityID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "," +
                                                    Result[n].SecurityID.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select IGroupSecurityConfig
                        case "SelectIGroupSecurityConfigByIGroupSecurityID":
                            {
                                string temp = string.Empty;
                                int IGroupSecurityID = -1;
                                int.TryParse(subValue[1], out IGroupSecurityID);
                                temp = this.GetIGroupSecurityConfigByIGroupSecurityIDInIGroupSecurityList(IGroupSecurityID);
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Security
                        case "SelectSecurity":
                            {
                                string temp = string.Empty;
                                temp = this.SelectSecurityInSecurityList();

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectSecurityByID":
                            {
                                string temp = string.Empty;
                                int SecurityID = -1;
                                int.TryParse(subValue[1], out SecurityID);
                                temp = this.SelectSecurityByIDInSecurityList(SecurityID);
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Symbol
                        case "SelectSymbol":
                            {
                                //string commandName = "GetAllSymbol$";
                                //string temp = string.Empty;
                                //temp = this.ExtractSelectSymbol();
                                //StringResult = commandName + temp;
                            }
                            break;

                        case "SelectSymbolByID":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int SymbolID = -1;
                                    int.TryParse(subValue[1], out SymbolID);
                                    temp = this.SelectSymbolByIDInSymbolList(SymbolID);
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select TradingConfig(SymbolConfig)
                        case "SelectTradingConfigBySymbolID":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int SymbolID = -1;
                                    int.TryParse(subValue[1], out SymbolID);
                                    temp = this.SelectTradingConfigBySymbolIDInSymbolList(SymbolID);
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Security Config
                        case "SelectSecurityConfigBySecurityID":
                            {
                                string temp = string.Empty;
                                int SecurityID = -1;
                                int.TryParse(subValue[1], out SecurityID);
                                temp = this.SelectSecurityConfigBySecurityIDInSecurityList(SecurityID);
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Investor(LOG COMMENT)
                        case "SelectInvestor":
                            {
                                string temp = string.Empty;
                                List<Business.Investor> Result = new List<Investor>();
                                Result = TradingServer.Facade.FacadeGetAllInvestor();
                                Result = Business.Market.InvestorList;

                                if (Result != null)
                                {
                                    int countInvestor = Result.Count;
                                    for (int n = 0; n < countInvestor; n++)
                                    {
                                        temp += Result[n].InvestorID.ToString() + "," + Result[n].InvestorStatusID.ToString() + "," +
                                                Result[n].InvestorGroupInstance.InvestorGroupID + "," + Result[n].AgentID.ToString() + "," +
                                                Result[n].Balance.ToString() + "," + Result[n].Credit.ToString() + "," +
                                                Result[n].Code + "," + Result[n].IsDisable.ToString() + "," +
                                                Result[n].TaxRate.ToString() + "," + Result[n].Leverage.ToString() + "," +
                                                Result[n].ToString() + "," + Result[n].Address + "," + Result[n].Phone + "," +
                                                Result[n].City + "," + Result[n].Country + "," + Result[n].Email + "," + Result[n].ZipCode + "," +
                                                Result[n].RegisterDay.ToString() + "," + Result[n].InvestorComment + "," + Result[n].State + "," + Result[n].NickName + "|";
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': 22450 accounts have been requested
                                    //string content = "'" + code + "': " + countInvestor + " accounts have been requested";
                                    //string comment = "[account request]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "LoginServer":
                            {
                                string status = "[Failed]";
                                string temp = string.Empty;
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length == 4)
                                {
                                    Business.Investor Result = new Investor();
                                    int InvestorIndex = -1;
                                    int.TryParse(subParameter[2], out InvestorIndex);

                                    Result = TradingServer.Facade.FacadeLoginServer(subParameter[0], subParameter[1], InvestorIndex, subParameter[3]);

                                    if (Result != null)
                                    {
                                        temp += Result.InvestorID.ToString() + "," + Result.InvestorStatusID.ToString() + "," +
                                                    Result.InvestorGroupInstance.InvestorGroupID + "," + Result.AgentID.ToString() + "," +
                                                    Result.Balance.ToString() + "," + Result.Credit.ToString() + "," +
                                                    Result.Code + "," + Result.IsDisable.ToString() + "," +
                                                    Result.TaxRate.ToString() + "," + Result.Leverage.ToString() + "," +
                                                    Result.InvestorProfileID.ToString() + "," + Result.Address + "," + Result.Phone + "," +
                                                    Result.City + "," + Result.Country + "," + Result.Email + "," + Result.ZipCode + "," +
                                                    Result.RegisterDay.ToString() + "," + Result.InvestorComment + "," + Result.State + "," + Result.NickName;

                                        status = "[Success]";
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': login admin [success]
                                    string content = "'" + code + "': login admin " + status;
                                    string comment = "[login admin]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    #endregion

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;

                        case "SelectInvestorByID":
                            {
                                string temp = string.Empty;
                                int InvestorID = -1;
                                int.TryParse(subValue[1], out InvestorID);
                                Business.Investor Result = new Investor();

                                if (Business.Market.InvestorList != null)
                                {
                                    int countInvestor = Business.Market.InvestorList.Count;
                                    for (int j = 0; j < countInvestor; j++)
                                    {
                                        if (Business.Market.InvestorList[j].InvestorID == InvestorID)
                                        {
                                            Result = Business.Market.InvestorList[j];
                                            break;
                                        }
                                    }
                                }

                                if (Result != null)
                                {
                                    temp = Result.InvestorID + "," + Result.InvestorStatusID + "," + Result.InvestorGroupInstance.InvestorGroupID + "," +
                                                Result.AgentID + "," + Math.Round(Result.Balance, 2) + "," + Result.Credit + "," + Result.Code + "," +
                                                Result.IsDisable + "," + Result.TaxRate + "," + Result.Leverage + "," +
                                                Result.InvestorProfileID + "," + Result.Address + "," + Result.Phone + "," +
                                                Result.City + "," + Result.Country + "," + Result.Email + "," + Result.ZipCode + "," +
                                                Result.RegisterDay + "," + Result.Comment + "," + Result.State + "," + Result.NickName + "," +
                                                Result.AllowChangePwd + "," + Result.ReadOnly + "," + Result.SendReport + "," + Result.IsOnline;
                                }

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                //'2222': 22450 accounts have been requested
                                int num = 0;
                                if (!string.IsNullOrEmpty(temp))
                                    num = 1;

                                string content = "'" + code + "': " + num + " accounts have been requested";
                                string comment = "[account request]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion

                                if (!string.IsNullOrEmpty(temp))
                                {
                                    StringResult = subValue[0] + "$" + temp;
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$";
                                }
                            }
                            break;

                        case "FindInvestor":    //COMMENT LOG BECAUSE MANAGER AUTO REQUEST ACCOUNT THEN ACCOUNT CHANGE(22/07/2011)
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    Business.Investor Result = new Investor();
                                    Result = TradingServer.Facade.FacadeFindInvestor(subValue[1]);

                                    if (Result != null)
                                    {
                                        temp = Result.InvestorID + "," + Result.InvestorStatusID + "," + Result.InvestorGroupInstance.InvestorGroupID + "," +
                                                    Result.AgentID + "," + Result.Balance + "," + Result.Credit + "," + Result.Code + "," +
                                                    Result.IsDisable + "," + Result.TaxRate + "," + Result.Leverage + "," +
                                                    Result.InvestorProfileID + "," + Result.Address + "," + Result.Phone + "," +
                                                    Result.City + "," + Result.Country + "," + Result.Email + "," + Result.ZipCode + "," +
                                                    Result.RegisterDay + "," + Result.InvestorComment + "," + Result.State + "," + Result.NickName + "," +
                                                    Result.AllowChangePwd + "," + Result.ReadOnly + "," + Result.SendReport + "," + Result.IsOnline + "," +
                                                    Result.Margin + "," + Result.FreezeMargin + "," + Result.PhonePwd + "," + Result.IDPassport + "," +
                                                    Result.TotalDeposit;
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': 22450 accounts have been requested
                                    //int num = 0;
                                    //if (!string.IsNullOrEmpty(temp))
                                    //    num = 1;

                                    //string content = "'" + code + "': " + num + " accounts have been requested";
                                    //string comment = "[account request]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "FindInvestorWithOnlineCommand":
                            {
                                string temp = string.Empty;
                                Business.Investor Result = new Investor();
                                Result = TradingServer.Facade.FacadeFindInvestorWithOnlineCommand(subValue[1]);

                                if (Result != null)
                                {
                                    temp = Result.InvestorID + "," + Result.NickName + "," + Result.Code;
                                }

                                #region INSERT SYSTEM LOG
                                //INSERT SYSTEM LOG
                                //'2222': 22450 accounts have been requested
                                int num = 0;
                                if (!string.IsNullOrEmpty(temp))
                                    num = 1;

                                string content = "'" + code + "': " + num + " accounts have been requested";
                                string comment = "[account request]";
                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                #endregion

                                StringResult = subValue[0] + "$" + Result;
                            }
                            break;

                        case "SelectInvestorByInvestorGroupID":
                            {
                                int InvestorGroupID = -1;
                                int.TryParse(subValue[1], out InvestorGroupID);
                            }
                            break;
                        #endregion

                        #region Select Agent
                        case "SelectAgent":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    List<Business.Agent> Result = new List<Agent>();
                                    Result = TradingServer.Facade.FacadeGetAllAgent();

                                    if (Result != null)
                                    {
                                        int countAgent = Result.Count;
                                        for (int n = 0; n < countAgent; n++)
                                        {
                                            temp += Result[n].AgentID.ToString() + "}" + Result[n].AgentGroupID.ToString() + "}" + Result[n].Name + "}" + Result[n].InvestorID.ToString()
                                                    + "}" + Result[n].Comment + "}" + Result[n].IsDisable.ToString() + "}" + Result[n].IsIpFilter.ToString() + "}" + Result[n].IpForm
                                                    + "}" + Result[n].IpTo + "}" + Result[n].Code + "}" + Result[n].GroupCondition + "|";
                                        }
                                    }

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;
                        case "SelectAgentOnline":
                            {
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    List<Business.Agent> Result = new List<Agent>();

                                    for (int i = Market.AgentList.Count - 1; i >= 0; i--)
                                    {
                                        if (Market.AgentList[i].IsVirtualDealer == false && Market.AgentList[i].IsOnline == true)
                                        {
                                            Result.Add(Market.AgentList[i]);
                                        }
                                    }

                                    if (Result != null)
                                    {
                                        int countAgent = Result.Count;
                                        for (int n = 0; n < countAgent; n++)
                                        {
                                            temp += Result[n].AgentID + "}" + Result[n].Code + "}" + Result[n].IsOnline + "}" + Result[n].IsBusy + "|";
                                        }
                                    }

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;
                        case "SelectAgentByID":
                            {
                                string temp = string.Empty;
                                int AgentID = -1;
                                int.TryParse(subValue[1], out AgentID);
                                Business.Agent Result = new Agent();

                                Result = TradingServer.Facade.FacadeGetAgentByAgentID(AgentID);

                                if (Result != null)
                                {
                                    temp += Result.AgentID.ToString() + "," + Result.AgentGroupID.ToString() + "," + Result.Name
                                        + "," + Result.InvestorID.ToString() + "," + Result.Comment + "," + Result.IsDisable.ToString();
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "LoginAdmin":
                            {
                                string status = "[Failed]";
                                string temp = string.Empty;
                                string[] subParameter = subValue[1].Split(',');
                                int count = subParameter.Length;
                                Business.Agent Result = new Agent();
                                if (count == 2)
                                {
                                    Result = TradingServer.Facade.FacadeAdminLogin(subParameter[0], subParameter[1], ipAddress);
                                    if (Result == null) temp = "-1";
                                    else
                                    {
                                        status = "[Success]";
                                        temp += Result.AgentID.ToString() + "," + Result.AgentGroupID.ToString() + "," + Result.Name + "," + Result.InvestorID.ToString() + "," + Result.Comment + "," + Result.IsDisable.ToString() + "," + ipAddress;
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': login admin [success]

                                    string content = "'" + code + "': login admin " + status;
                                    string comment = "[login admin]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    #endregion

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;

                        case "AdminLogout":
                            {
                                bool Result = false;
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int AgentID = -1;
                                    int.TryParse(subValue[1], out AgentID);

                                    Result = TradingServer.Facade.FacadeAdminLogout(AgentID);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': login admin [success]
                                    string status = "[Failed]";
                                    if (Result)
                                        status = "[Success]";

                                    string content = "'" + code + "': logout admin " + status;
                                    string comment = "[logout admin]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    #endregion
                                }
                                StringResult = subValue[0] + "$" + Result.ToString();
                            }
                            break;

                        case "LoginManager":
                            {
                                string temp = string.Empty;
                                string status = "[Failed]";
                                string[] subParameter = subValue[1].Split(',');
                                int count = subParameter.Length;
                                Business.Agent Result = new Agent();
                                if (count == 3)
                                {
                                    Result = TradingServer.Facade.FacadeManagerLogin(subParameter[0], subParameter[1], subParameter[2], ipAddress);
                                    if (Result == null) temp = "-1";
                                    else
                                    {
                                        status = "[Success]";
                                        temp += Result.AgentID.ToString() + "," + Result.AgentGroupID.ToString() + "," + Result.Name + "," + Result.InvestorID.ToString() + "," + Result.Comment + "," + Result.IsDisable.ToString() + "," + ipAddress;
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': login admin [success]                                       

                                    string content = "'" + code + "': login manager " + status;
                                    string comment = "[login manager]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    #endregion

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;

                        case "ManagerLogout":
                            {
                                bool Result = false;
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {
                                    int AgentID = -1;
                                    int.TryParse(subValue[1], out AgentID);
                                    
                                    Result = TradingServer.Facade.FacadeManagerLogout(AgentID);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': login admin [success]
                                    string status = "[Failed]";
                                    if (Result)
                                        status = "[Success]";

                                    string content = "'" + code + "': logout manager " + status;
                                    string comment = "[logout manager]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    #endregion                                    
                                }
                                StringResult = subValue[0] + "$" + Result.ToString();
                            }
                            break;

                        case "LoginDealer"://COMMENT LOG BECAUSE MANAGER 1 MINUTE LOGIN DEALER THEN MANY LOG INSERT TO DATABASE(21/07/2011)
                            {
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    int count = subParameter.Length;
                                    int AgentID = -1;
                                    int.TryParse(subParameter[0], out AgentID);
                                    bool Result = false;
                                    if (count > 0)
                                    {
                                        Result = TradingServer.Facade.FacadeDealerLogin(AgentID, ipAddress);

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        //'2222': dealer dispatched connected
                                        string content = "'" + code + "': dealer dispatched connected ";
                                        string comment = "[login dealer]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                        #endregion


                                        StringResult = subValue[0] + "$" + Result.ToString();
                                    }
                                }
                            }
                            break;
                        case "UpdateTimeSyncDealer":
                            {
                                int AgentID = -1;
                                string[] splits = subValue[1].Split(',');
                                if (splits.Length == 2)
                                {
                                    int.TryParse(splits[0], out AgentID);
                                    string KeyActive = splits[1];
                                    if (AgentID > 0)
                                    {
                                        for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
                                        {
                                            if (Business.Market.AgentList[i].AgentID == AgentID)
                                            {
                                                Business.Market.AgentList[i].TimeSync = DateTime.Now;
                                                break;
                                            }
                                        }

                                    }
                                }
                                StringResult = subValue[0] + "$";
                                break;
                            }
                        case "DealerLogout": //COMMENT LOG BECAUSE MANAGER 1 MINUTE LOGIN DEALER THEN MANY LOG INSERT TO DATABASE(21/07/2011)
                            {
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {
                                    int AgentID = -1;
                                    int.TryParse(subValue[1], out AgentID);
                                    bool Result = false;
                                    Result = TradingServer.Facade.FacadeDealerLogout(AgentID);

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': dealer dispatched disconnected
                                    string content = "'" + code + "': dealer dispatched disconnected ";
                                    string comment = "[logout dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    #endregion

                                    StringResult = subValue[0] + "$" + Result.ToString();
                                }
                            }
                            break;

                        case "GetRequestToDealer":
                            {
                                string temp = string.Empty;
                                int AgentID = -1;
                                int.TryParse(subValue[1], out AgentID);
                                if (AgentID > 0)
                                {
                                    List<Business.RequestDealer> Result = new List<RequestDealer>();
                                    Result = TradingServer.Facade.FacadeGetRequestToDealer(AgentID);
                                    if (Result == null) temp = "-1";
                                    else
                                    {
                                        int count = Result.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            temp += Result[i].AgentID + "," + Result[i].FlagConfirm + "," + Result[i].MaxDev + "," + Result[i].Name + "," + Result[i].Notice
                                                + "," + Result[i].TimeAgentReceive + "," + Result[i].TimeClientRequest + "," + Result[i].Request.Investor.InvestorID
                                                + "," + Result[i].Request.Symbol.Name + "," + Result[i].Request.Size + "," + Result[i].Request.OpenTime
                                                + "," + Result[i].Request.OpenPrice + "," + Result[i].Request.StopLoss + "," + Result[i].Request.TakeProfit
                                                + "," + Result[i].Request.ClosePrice + "," + Result[i].Request.Commission + "," + Result[i].Request.Swap
                                                + "," + Result[i].Request.Profit + "," + Result[i].Request.ID + "," + Result[i].Request.ExpTime + "," + Result[i].Request.ClientCode
                                                + "," + Result[i].Request.IsHedged + "," + Result[i].Request.Type.ID + "," + Result[i].Request.Margin + "," + Result[i].Request.Investor.InvestorGroupInstance.Name
                                                + "," + Result[i].Request.Symbol.Digit + "," + Result[i].Request.Symbol.SpreadByDefault.ToString() + "," +
                                                /*Result[i].Request.Symbol.SpreadDifference*/ Result[i].Request.SpreaDifferenceInOpenTrade
                                                + "," + Result[i].Request.CommandCode + "," + Result[i].Request.IsMultiClose + "," + Result[i].Request.RefCommandID + "|";
                                        }
                                    }
                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;
                        case "GetAllRequestDealer":
                            {
                                string temp = string.Empty;

                                List<Business.RequestDealer> Result = new List<RequestDealer>();
                                Result = TradingServer.Facade.FacadeGetAllRequestDealer();
                                if (Result == null) temp = "-1";
                                else
                                {
                                    int count = Result.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        temp += Result[i].AgentID + "," + Result[i].FlagConfirm + "," + Result[i].MaxDev + "," + Result[i].Name + "," + Result[i].Notice
                                                + "," + Result[i].TimeAgentReceive + "," + Result[i].TimeClientRequest + "," + Result[i].Request.Investor.InvestorID
                                                + "," + Result[i].Request.Symbol.Name + "," + Result[i].Request.Size + "," + Result[i].Request.OpenTime
                                                + "," + Result[i].Request.OpenPrice + "," + Result[i].Request.StopLoss + "," + Result[i].Request.TakeProfit
                                                + "," + Result[i].Request.ClosePrice + "," + Result[i].Request.Commission + "," + Result[i].Request.Swap
                                                + "," + Result[i].Request.Profit + "," + Result[i].Request.ID + "," + Result[i].Request.ExpTime + "," + Result[i].Request.ClientCode
                                                + "," + Result[i].Request.IsHedged + "," + Result[i].Request.Type.ID + "," + Result[i].Request.Margin + "," + Result[i].Request.Investor.InvestorGroupInstance.Name
                                                + "," + Result[i].Request.Symbol.Digit + "," + Result[i].Request.Symbol.SpreadByDefault.ToString() + "," +
                                            /*Result[i].Request.Symbol.SpreadDifference*/ Result[i].Request.SpreaDifferenceInOpenTrade
                                                + "," + Result[i].Request.CommandCode + "|";
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        case "GetAllRequestCompare":
                            {
                                string temp = string.Empty;

                                List<Business.RequestDealer> Result = new List<RequestDealer>();
                                Result = TradingServer.Facade.FacadeGetAllRequestCompareDealer();
                                if (Result == null) temp = "-1";
                                else
                                {
                                    int count = Result.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        temp += Result[i].AgentID + "," + Result[i].FlagConfirm + "," + Result[i].MaxDev + "," + Result[i].Name + "," + Result[i].Notice
                                                + "," + Result[i].TimeAgentReceive + "," + Result[i].TimeClientRequest + "," + Result[i].Request.Investor.InvestorID
                                                + "," + Result[i].Request.Symbol.Name + "," + Result[i].Request.Size + "," + Result[i].Request.OpenTime
                                                + "," + Result[i].Request.OpenPrice + "," + Result[i].Request.StopLoss + "," + Result[i].Request.TakeProfit
                                                + "," + Result[i].Request.ClosePrice + "," + Result[i].Request.Commission + "," + Result[i].Request.Swap
                                                + "," + Result[i].Request.Profit + "," + Result[i].Request.ID + "," + Result[i].Request.ExpTime + "," + Result[i].Request.ClientCode
                                                + "," + Result[i].Request.IsHedged + "," + Result[i].Request.Type.ID + "," + Result[i].Request.Margin + "," + Result[i].Request.Investor.InvestorGroupInstance.Name
                                                + "," + Result[i].Request.Symbol.Digit + "," + Result[i].Request.Symbol.SpreadByDefault.ToString() + "," +
                                            /*Result[i].Request.Symbol.SpreadDifference*/ Result[i].Request.SpreaDifferenceInOpenTrade
                                                + "," + Result[i].Request.CommandCode + "|";
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "GetNoticeDealer":
                            {
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {
                                    int AgentID = -1;
                                    string[] splits = subValue[1].Split(',');
                                    if (splits.Length == 2)
                                    {
                                        int.TryParse(splits[0], out AgentID);
                                        string KeyActive = splits[1];
                                        if (AgentID > 0)
                                        {
                                            string Result = "";
                                            Result = TradingServer.Facade.FacadeGetNoticeDealer(AgentID, KeyActive);
                                            if (!string.IsNullOrEmpty(Result))
                                            {
                                                StringResult = subValue[0] + "$" + Result + ",NA28:" + DateTime.Now.Ticks;
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$" + ",NA28:" + DateTime.Now.Ticks;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    StringResult = "InvalidIP$NA27";
                                }
                            }
                            break;

                        case "GetAllDealerOnline":
                            {
                                string temp = Facade.FacadeGetAllDealerOnline();
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "GetAllArchiveCandlesOffline":
                            {
                                string temp = Facade.FacadeGetAllArchiveCandlesOffline();
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "DealerCommandConfirm":
                            {
                                bool ResultAddNew = false;
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                Business.RequestDealer Result = new RequestDealer();
                                Result = this.ExtractionRequestDealer(subValue[1]);
                                Facade.FillInstanceOpenTrade(Result, Result.Request);
                                if (checkip)
                                {                                    
                                    ResultAddNew = TradingServer.Facade.FacadeDealerCommandConfirm(Result);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = "InvalidIP";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + code + "': dealer confirm action not taken (invalid ip)'" + Result.Request.Investor.Code
                                                       + "' " + Result.Name.ToLower() + " " + Facade.FacadeGetTypeCommand(Result.Request.Type.ID) + " " + Result.Request.FormatDoubleToString(Result.Request.Size) + " symbol:"
                                                       + Result.Request.Symbol.Name + " price open:" + Result.Request.MapPriceForDigit(Result.Request.OpenPrice) + " price close:" + Result.Request.MapPriceForDigit(Result.Request.ClosePrice) + " sl:" + Result.Request.MapPriceForDigit(Result.Request.StopLoss)
                                                       + " tp:" + Result.Request.MapPriceForDigit(Result.Request.TakeProfit), "Invalid IP", ipAddress, code);
                                }

                            }
                            break;

                        case "DealerCommandReject":
                            {
                                bool ResultAddNew = false;
                                Business.RequestDealer Result = new RequestDealer();
                                Result = this.ExtractionRequestDealer(subValue[1]);
                                Facade.FillInstanceOpenTrade(Result, Result.Request);
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {                                    
                                    ResultAddNew = TradingServer.Facade.FacadeDealerCommandReject(Result);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = "InvalidIP";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + code + "': dealer reject action not taken (invalid ip)'" + Result.Request.Investor.Code
                                                      + "' " + Result.Name.ToLower() + " " + Facade.FacadeGetTypeCommand(Result.Request.Type.ID) + " " + Result.Request.FormatDoubleToString(Result.Request.Size) + " symbol:"
                                                      + Result.Request.Symbol.Name + " price open:" + Result.Request.MapPriceForDigit(Result.Request.OpenPrice) + " price close:" + Result.Request.MapPriceForDigit(Result.Request.ClosePrice) + " sl:" + Result.Request.MapPriceForDigit(Result.Request.StopLoss)
                                                      + " tp:" + Result.Request.MapPriceForDigit(Result.Request.TakeProfit), "Invalid IP", ipAddress, code);
                                }
                            }
                            break;

                        case "DealerCommandReturn":
                            {
                                bool ResultAddNew = false;
                                Business.RequestDealer Result = new RequestDealer();
                                Result = this.ExtractionRequestDealer(subValue[1]);
                                Facade.FillInstanceOpenTrade(Result, Result.Request);
                                bool checkip = Facade.FacadeCheckIpManager(code, ipAddress);
                                if (checkip)
                                {                                    
                                    ResultAddNew = TradingServer.Facade.FacadeDealerCommandReturn(Result);
                                    StringResult = subValue[0] + "$" + ResultAddNew.ToString();
                                }
                                else
                                {
                                    StringResult = "InvalidIP";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, "'" + code + "': dealer return action not taken (invalid ip)'" + Result.Request.Investor.Code
                                                      + "' " + Result.Name.ToLower() + " " + Facade.FacadeGetTypeCommand(Result.Request.Type.ID) + " " + Result.Request.FormatDoubleToString(Result.Request.Size) + " symbol:"
                                                      + Result.Request.Symbol.Name + " price open:" + Result.Request.MapPriceForDigit(Result.Request.OpenPrice) + " price close:" + Result.Request.MapPriceForDigit(Result.Request.ClosePrice) + " sl:" + Result.Request.MapPriceForDigit(Result.Request.StopLoss)
                                                      + " tp:" + Result.Request.MapPriceForDigit(Result.Request.TakeProfit), "Invalid IP", ipAddress, code);
                                }
                            }
                            break;

                        #endregion

                        #region Select Internal Mail
                        case "GetInternalMailToInvestorByID":
                            {
                                Business.InternalMail result = new InternalMail();
                                string temp = "";
                                int MailID = int.Parse(subValue[1]);
                                result = TradingServer.Facade.FacadeGetInternalMailToInvestorByID(MailID);
                                if (result != null)
                                {
                                    temp = result.InternalMailID + "," + TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.Subject) + "," +
                                        TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.From) + "," +
                                        TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.FromName) + "," +
                                        TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.Time.ToString()) + "," +
                                        result.IsNew.ToString() + "," +
                                        TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.Content);
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        case "GetFullInternalMailToInvestor":
                            {
                                string[] tem = subValue[1].Split('{');
                                string temp = "";
                                List<Business.InternalMail> result = new List<InternalMail>();
                                result = Facade.internalMailInstance.GetInternalMailToInvestor(tem[0]);
                                if (result != null)
                                {
                                    int countResult = result.Count;
                                    for (int j = 0; j < countResult; j++)
                                    {
                                        temp += result[j].InternalMailID + "█" + result[j].Subject + "█" +
                                            result[j].From + "█" + result[j].FromName + "█" + result[j].Time.ToString() + "█" +
                                            result[j].IsNew.ToString() + "█" + result[j].Content + "█";
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        case "GetInternalMailToAgentByID":
                            {
                                Business.InternalMail result = new InternalMail();
                                string temp = "";
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string[] spli = subValue[1].Split(',');
                                    int MailID = int.Parse(spli[0]);
                                    result = TradingServer.Facade.FacadeGetInternalMailToAgentByID(MailID, spli[1]);
                                    if (result != null)
                                    {
                                        temp = result.InternalMailID + "," + TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.Subject) + "," +
                                            TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.From) + "," +
                                            TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.FromName) + "," +
                                            TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.Time.ToString()) + "," +
                                            result.IsNew.ToString() + "," +
                                            TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result.Content);
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        #endregion

                        #region Select Alert
                        case "SelectAlertByInvestorID":
                            {
                                string temp = string.Empty;
                                List<Business.PriceAlert> Result = new List<PriceAlert>();
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length > 0)
                                {
                                    int Start = int.Parse(subParameter[0]);
                                    int End = int.Parse(subParameter[1]);
                                    int InvestorID = int.Parse(subParameter[2]);
                                    Result = TradingServer.Facade.FacadeGetAlertByInvestorID(InvestorID, Start, End);
                                    if (Result != null)
                                    {
                                        for (int n = 0; n < Result.Count; n++)
                                        {
                                            temp += Result[n].ID + "," + Result[n].Symbol + "," + Result[n].Email + "," + Result[n].PhoneNumber
                                                    + "," + Result[n].Value + "," + Result[n].AlertCondition.ToString() + "," + Result[n].AlertAction.ToString() + "," + Result[n].IsEnable
                                                    + "," + Result[n].DateCreate + "," + Result[n].DateActive + "," + Result[n].InvestorID + "," + Result[n].Notification + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        case "GetAlertByInvestorID":
                            {
                                string temp = string.Empty;
                                List<Business.PriceAlert> Result = new List<PriceAlert>();
                                if (subValue[1] != "")
                                {
                                    int InvestorID = int.Parse(subValue[1]);
                                    Result = TradingServer.Facade.FacadeGetAlertByInvestorID(InvestorID);
                                    if (Result != null)
                                    {
                                        for (int n = 0; n < Result.Count; n++)
                                        {
                                            temp += Result[n].ID + "{" + Result[n].Symbol + "{" + Result[n].Email + "{" + Result[n].PhoneNumber
                                                    + "{" + Result[n].Value + "{" + Result[n].AlertCondition.ToString() + "{" + Result[n].AlertAction.ToString() + "{" + Result[n].IsEnable
                                                    + "{" + Result[n].DateCreate + "{" + Result[n].DateActive + "{" + Result[n].InvestorID + "{" + Result[n].Notification + "`";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        case "SelectAlertByInvestorIDWithTime":
                            {
                                string temp = string.Empty;
                                List<Business.PriceAlert> Result = new List<PriceAlert>();
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length > 0)
                                {
                                    int InvestorID = int.Parse(subParameter[0]);
                                    DateTime Start = DateTime.Parse(subParameter[1]);
                                    DateTime End = DateTime.Parse(subParameter[2]);
                                    Result = TradingServer.Facade.FacadeGetAlertByInvestorIDWithTime(InvestorID, Start, End);
                                    if (Result != null)
                                    {
                                        for (int n = 0; n < Result.Count; n++)
                                        {
                                            temp += Result[n].ID + "," + Result[n].Symbol + "," + Result[n].Email + "," + Result[n].PhoneNumber
                                                    + "," + Result[n].Value + "," + Result[n].AlertCondition.ToString() + "," + Result[n].AlertAction.ToString() + "," + Result[n].IsEnable
                                                    + "," + Result[n].DateCreate + "," + Result[n].DateActive + "," + Result[n].InvestorID + "," + Result[n].Notification + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        case "SelectHistoryAlertWithTime":
                            {
                                string temp = string.Empty;
                                List<Business.PriceAlert> Result = new List<PriceAlert>();
                                string[] subParameter = subValue[1].Split('{');
                                if (subParameter.Length > 0)
                                {
                                    int InvestorID = int.Parse(subParameter[0]);
                                    DateTime Start = DateTime.Parse(subParameter[1]);
                                    DateTime End = DateTime.Parse(subParameter[2]);
                                    Result = TradingServer.Facade.FacadeGetAlertByInvestorIDWithTime(InvestorID, Start, End);
                                    if (Result != null)
                                    {
                                        for (int n = 0; n < Result.Count; n++)
                                        {
                                            temp += Result[n].ID + "{" + Result[n].Symbol + "{" + Result[n].Email + "{" + Result[n].PhoneNumber
                                                    + "{" + Result[n].Value + "{" + Result[n].AlertCondition.ToString() + "{" + Result[n].AlertAction.ToString() + "{" + Result[n].IsEnable
                                                    + "{" + Result[n].DateCreate + "{" + Result[n].DateActive + "{" + Result[n].InvestorID + "{" + Result[n].Notification + "`";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Permit
                        case "SelectPermit":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    List<Business.Permit> Result = new List<Permit>();
                                    Result = TradingServer.Facade.FacadeGetAllPermit();

                                    if (Result != null)
                                    {
                                        int countAgent = Result.Count;
                                        for (int n = 0; n < countAgent; n++)
                                        {
                                            temp += Result[n].PermitID.ToString() + "," + Result[n].AgentGroupID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].Role.RoleID.ToString() + "|";
                                        }
                                    }

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;

                        case "SelectPermitByID":
                            {
                                string temp = string.Empty;
                                int PermitID = -1;
                                int.TryParse(subValue[1], out PermitID);
                                Business.Permit Result = new Permit();

                                Result = TradingServer.Facade.FacadeGetPermitByPermitID(PermitID);

                                if (Result != null)
                                {
                                    temp += Result.PermitID.ToString() + "," + Result.AgentGroupID.ToString() + "," + Result.AgentID.ToString() + "," + Result.Role.RoleID.ToString();
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "GetCodeAgentSendMailByInvestorGroupID":
                            {
                                string temp = string.Empty;
                                int GroupID = -1;
                                int.TryParse(subValue[1], out GroupID);
                                List<Business.Agent> Result = new List<Agent>();
                                Result = TradingServer.Facade.FacadeGetCodeAgentMailByInvestorGroupID(GroupID);
                                if (Result != null)
                                {
                                    for (int i = 0; i < Result.Count; i++)
                                    {
                                        if (Result[i].Name != "" & Result[i].Code != "")
                                        {
                                            temp += Result[i].Name + "," + Result[i].Code + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectPermitByAgentID":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int AgentID = -1;
                                    int.TryParse(subValue[1], out AgentID);
                                    List<Business.Permit> Result = new List<Permit>();

                                    Result = TradingServer.Facade.FacadeGetPermitByAgentID(AgentID);
                                    if (Result != null)
                                    {
                                        int countAgent = Result.Count;
                                        for (int n = 0; n < countAgent; n++)
                                        {
                                            temp += Result[n].PermitID.ToString() + "," + Result[n].AgentGroupID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].Role.RoleID.ToString() + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region Select Role
                        case "SelectRole":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    List<Business.Role> Result = new List<Role>();
                                    Result = TradingServer.Facade.FacadeGetAllRole();

                                    if (Result != null)
                                    {
                                        int countAgent = Result.Count;
                                        for (int n = 0; n < countAgent; n++)
                                        {
                                            temp += Result[n].RoleID.ToString() + "," + Result[n].Code + "," + Result[n].Name + "," + Result[n].Comment + "|";
                                        }
                                    }

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;

                        case "SelectRoleByID":
                            {
                                string temp = string.Empty;
                                int RoleID = -1;
                                int.TryParse(subValue[1], out RoleID);
                                Business.Role Result = new Role();

                                Result = TradingServer.Facade.FacadeGetRoleByRoleID(RoleID);

                                if (Result != null)
                                {
                                    temp += Result.RoleID.ToString() + "," + Result.Code + "," + Result.Name + "," + Result.Comment;
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        #endregion

                        #region Select IAgentSecurity
                        case "SelectIAgentSecurity":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string temp = string.Empty;
                                    List<Business.IAgentSecurity> Result = new List<IAgentSecurity>();
                                    Result = TradingServer.Facade.FacadeGetIAgentSecurity();

                                    if (Result != null)
                                    {
                                        int countInvestor = Result.Count;
                                        for (int n = 0; n < countInvestor; n++)
                                        {
                                            temp += Result[n].IAgentSecurityID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].SecurityID.ToString()
                                                    + "," + Result[n].Use.ToString() + "," + Result[n].MinLots.ToString() + "," + Result[n].MaxLots.ToString() + "|";
                                        }
                                    }

                                    StringResult = subValue[0] + "$" + temp;
                                }
                            }
                            break;


                        case "SelectIAgentSecurityByID":
                            {
                                string temp = string.Empty;
                                int IAgentSecurityID = -1;
                                int.TryParse(subValue[1], out IAgentSecurityID);
                                Business.IAgentSecurity Result = new IAgentSecurity();

                                Result = TradingServer.Facade.FacadeGetIAgentSecurityByID(IAgentSecurityID);

                                if (Result != null)
                                {
                                    temp = Result.IAgentSecurityID.ToString() + "," + Result.AgentID.ToString() + "," + Result.SecurityID.ToString() + "," + Result.Use.ToString() + "," + Result.MinLots.ToString() + "," + Result.MaxLots.ToString();
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIAgentSecurityByAgentID":
                            {
                                string temp = string.Empty;
                                int AgentID = -1;
                                int.TryParse(subValue[1], out AgentID);
                                List<Business.IAgentSecurity> Result = new List<Business.IAgentSecurity>();

                                Result = TradingServer.Facade.FacadeGetIAgentSecurityByAgentID(AgentID);

                                if (Result != null)
                                {
                                    int countInvestor = Result.Count;
                                    for (int n = 0; n < countInvestor; n++)
                                    {
                                        temp += Result[n].IAgentSecurityID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].SecurityID.ToString()
                                                 + "," + Result[n].Use.ToString() + "," + Result[n].MinLots.ToString() + "," + Result[n].MaxLots.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIAgentSecurityBySecurityID":
                            {
                                string temp = string.Empty;
                                int SecurityID = -1;
                                int.TryParse(subValue[1], out SecurityID);
                                List<Business.IAgentSecurity> Result = new List<Business.IAgentSecurity>();

                                Result = TradingServer.Facade.FacadeGetIAgentSecurityBySecurityID(SecurityID);

                                if (Result != null)
                                {
                                    int countInvestor = Result.Count;
                                    for (int n = 0; n < countInvestor; n++)
                                    {
                                        temp += Result[n].IAgentSecurityID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].SecurityID.ToString()
                                             + "," + Result[n].Use.ToString() + "," + Result[n].MinLots.ToString() + "," + Result[n].MaxLots.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        #endregion

                        #region Select IAgentGroup
                        case "SelectIAgentGroup":
                            {
                                string temp = string.Empty;
                                List<Business.IAgentGroup> Result = new List<IAgentGroup>();
                                Result = TradingServer.Facade.FacadeGetAllIAgentGroup();

                                if (Result != null)
                                {
                                    int countInvestor = Result.Count;
                                    for (int n = 0; n < countInvestor; n++)
                                    {
                                        temp += Result[n].IAgentGroupID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIAgentGroupByID":
                            {
                                string temp = string.Empty;
                                int IAgentGroupID = -1;
                                int.TryParse(subValue[1], out IAgentGroupID);
                                Business.IAgentGroup Result = new IAgentGroup();

                                Result = TradingServer.Facade.FacadeGetIAgentGroupByIAgentGroupID(IAgentGroupID);

                                if (Result != null)
                                {
                                    temp = Result.IAgentGroupID.ToString() + "," + Result.AgentID.ToString() + "," + Result.InvestorGroupID.ToString();
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIAgentGroupByAgentID":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int AgentID = -1;
                                    int.TryParse(subValue[1], out AgentID);
                                    List<Business.IAgentGroup> Result = new List<Business.IAgentGroup>();

                                    Result = TradingServer.Facade.FacadeGetIAgentGroupByAgentID(AgentID);

                                    if (Result != null)
                                    {
                                        int countInvestor = Result.Count;
                                        for (int n = 0; n < countInvestor; n++)
                                        {
                                            temp += Result[n].IAgentGroupID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "|";
                                        }
                                    }
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        case "SelectIAgentSecurityByInvestorGroupID":
                            {
                                string temp = string.Empty;
                                int InvestorGroupID = -1;
                                int.TryParse(subValue[1], out InvestorGroupID);
                                List<Business.IAgentGroup> Result = new List<Business.IAgentGroup>();

                                Result = TradingServer.Facade.FacadeGetIAgentGroupByInvestorGroupID(InvestorGroupID);

                                if (Result != null)
                                {
                                    int countInvestor = Result.Count;
                                    for (int n = 0; n < countInvestor; n++)
                                    {
                                        temp += Result[n].IAgentGroupID.ToString() + "," + Result[n].AgentID.ToString() + "," + Result[n].InvestorGroupID.ToString() + "|";
                                    }
                                }

                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;

                        #endregion

                        #region Select Market Area
                        case "SelectMarketArea":
                            {
                                string temp = string.Empty;
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    temp = this.ExtractSelectMarketArea();
                                }
                                StringResult = subValue[0] + "$" + temp;
                            }
                            break;
                        #endregion

                        #region SELECT OPEN TRADE(LOG COMMENT)
                        case "GetOpenTradeByID":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    int OpenTradeID = -1;
                                    int.TryParse(subValue[1], out OpenTradeID);
                                    Business.OpenTrade Result = new OpenTrade();
                                    Result = TradingServer.Facade.FacadeGetOpenTradeByOpenTradeID(OpenTradeID);

                                    if (Result.Symbol != null && Result.Investor != null)
                                    {
                                        StringResult = subValue[0] + "$" + Result.ClientCode + "," + Result.ClosePrice + "," + Result.CloseTime + "," +
                                                        Result.CommandCode + "," + Result.Commission + "," + Result.ExpTime + "," +
                                                        Result.ID + "," + Result.Investor.InvestorID + "," + Result.IsClose + "," +
                                                        Result.IsHedged + "," + Result.Margin + "," + Result.MaxDev + "," + Result.OpenPrice + "," +
                                                        Result.OpenTime + "," + Result.Profit + "," + Result.Size + "," + Result.StopLoss + "," +
                                                        Result.Swap + "," + Result.Symbol.Name + "," + Result.TakeProfit + "," + Result.Taxes + "," +
                                                        Result.Type.Name + "," + Result.Type.ID + "," + Result.Symbol.ContractSize + "," +
                                            /*Result.Symbol.SpreadDifference*/ +Result.SpreaDifferenceInOpenTrade + "," + Result.Symbol.Currency + "," +
                                            Result.Comment + "," + Result.AgentCommission;
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': 22450 accounts have been requested
                                    //int num = 0;
                                    //if (Result.ID > 0)
                                    //    num = 1;                           

                                    //string content = "'" + code + "': " + num + " accounts have been requested";
                                    //string comment = "[account request]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion
                                }
                            }
                            break;
                        #endregion

                        #region SELECT ORDER DATA
                        case "GetListFiles":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                bool checkPermit = Facade.FacadeCheckPermitDownloadStatement(code);
                                if (checkip)
                                {
                                    if (checkPermit)
                                    {
                                        DateTime from = new DateTime();
                                        DateTime to = new DateTime();
                                        string[] times = subValue[1].Split('{');
                                        if (times.Length == 2)
                                        {
                                            from = DateTime.Parse(times[0]);
                                            to = DateTime.Parse(times[1]);
                                            StringResult = subValue[0] + "$" + Model.TradingCalculate.Instance.GetFileInfo(from, to);
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + "MCM006";
                                    }
                                }
                            }
                            break;
                        case "GetFileContents":
                            {
                                bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                bool checkPermit = Facade.FacadeCheckPermitDownloadStatement(code);
                                if (checkip)
                                {
                                    if (checkPermit)
                                    {
                                        if (subValue[1] != "")
                                        {
                                            StringResult = subValue[0] + "$" + Model.TradingCalculate.Instance.GetContenFile(subValue[1]);
                                        }
                                    }
                                    else
                                    {
                                        StringResult = subValue[0] + "$" + "MCM006";
                                    }
                                }
                            }
                            break;
                        case "GetOrderByCode":
                            {
                                Business.OrderData Result = new OrderData();
                                Result = TradingServer.Facade.FacadeGetOrderDataByCode(subValue[1]);
                                if (!string.IsNullOrEmpty(Result.Code))
                                {
                                    StringResult = subValue[0] + "$" + Result.Login + "," + Result.Type + "," +
                                                    Result.OpenTime + "," + Result.CloseTime + "," + Result.OneConvRate + "," +
                                                    Result.Commission + "," + Result.Comment + "," + Result.ExpDate + "," +
                                                    Result.Lots + "," + Result.OpenPrice + "," + Result.ClosePrice + "," +
                                                    Result.TwoConvRate + "," + Result.AgentCommission + "," + Result.ValueDate + "," +
                                                    Result.Symbol + "," + Result.StopLoss + "," + Result.TakeProfit + "," +
                                                    Result.MarginRate + "," + Result.Swaps + "," + Result.Taxes + "," + Result.Profit + "," +
                                                    Result.OrderCode + "," + Result.Code;
                                }
                            }
                            break;
                        #endregion

                        #region CHECK PASSWORD
                        case "VerifyMaster":
                            {
                                bool resultCheck = false;
                                string[] subParameter = subValue[1].Split(',');
                                if (subParameter.Length == 2)
                                {
                                    int investorID = 0;
                                    bool check = int.TryParse(subParameter[0], out investorID);
                                    if (check)
                                    {
                                        Business.Investor tempInvestor = new Investor();
                                        tempInvestor = TradingServer.Facade.FacadeGetInvestorByInvestorID(investorID);
                                        bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                        if (checkip)
                                        {
                                            bool checkRule = Facade.FacadeCheckPermitAccountManagerAndAdmin(code);
                                            bool checkGroup = Facade.FacadeCheckPermitAccessGroupManagerAndAdmin(code, tempInvestor.InvestorGroupInstance.InvestorGroupID);
                                            if (checkRule && checkGroup)
                                            {                                                
                                                resultCheck = TradingServer.Facade.FacadeCheckMasterPassword(investorID, subParameter[1]);
                                                if (resultCheck)
                                                {
                                                    string content = "'" + code + "': checking password of '" + tempInvestor.Code + "' [Success]";
                                                    string comment = "[verify password]";
                                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                                }
                                                else
                                                {
                                                    string content = "'" + code + "': checking password of '" + tempInvestor.Code + "' [Failed]";
                                                    string comment = "[verify password]";
                                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                                }
                                                StringResult = subValue[0] + "$" + resultCheck;
                                            }
                                            else
                                            {
                                                StringResult = subValue[0] + "$MCM006";
                                                string content = "'" + code + "': checking password of '" + tempInvestor.Code + "' failed(not enough rights)";
                                                string comment = "[verify password]";
                                                TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            }
                                        }
                                        else
                                        {
                                            StringResult = subValue[0] + "$" + "MCM005";
                                            string content = "'" + code + "': checking password of '" + tempInvestor.Code + "' failed(invalid ip)";
                                            string comment = "[verify password]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        }
                                    }
                                }                                

                            }
                            break;
                        #endregion

                        #region GET TIME SERVER
                        case "GetTimeServer":
                            {
                                long time = 0;
                                time = TradingServer.ClientFacade.FacadeGetTimeZoneServer();
                                StringResult = subValue[0] + "$" + time;
                            }
                            break;
                        #endregion

                        #region MANUAL SEND REPORT
                        case "ManualSendReport":
                            {
                                string[] subParameter = subValue[1].Split(',');
                                //string investorCode = subParameter[0];
                                //DateTime timeStart = DateTime.Parse(subParameter[1]);
                                //DateTime timeEnd = DateTime.Parse(subParameter[2]);

                                Business.TimeEvent newTimeEvent = new TimeEvent();
                                //Market.marketInstance.SendReportDayManaual(investorCode, timeStart, timeEnd);                                
                                marketInstance.SendReportDay("All,", newTimeEvent);
                            }
                            break;

                        case "ManualSendReportMonth":
                            {
                                string[] subParameter = subValue[1].Split(',');

                                Business.TimeEvent newTimeEvent = new TimeEvent();
                                marketInstance.SendReportMonth("All", newTimeEvent);
                            }
                            break;
                        #endregion

                        #region MANUAL CALCULATION SWAP
                        case "ManualCalculationSwap":
                            {
                                Business.TimeEvent newTimeEvent = new TimeEvent();
                                marketInstance.BeginCalculationSwap("All", newTimeEvent);
                            }
                            break;
                        #endregion

                        #region VIRTUAL DEALER
                        case "KD312":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string status = "[Failed]";
                                    VirtualDealer vd = new VirtualDealer();
                                    string result = vd.MapDealer(subValue[1]);
                                    int id;
                                    int.TryParse(result, out id);
                                    if (id == 1)
                                    {
                                        result = vd.AddDealer();
                                        status = "[Success]";
                                    }
                                    string content = "'" + code + "': robot dealer config added/changed ['" + vd.Name + "'] " + status;
                                    string comment = "[add new robot dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    StringResult = subValue[0] + "$" + result;
                                }
                            }
                            break;

                        case "KD341":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string status = "[Failed]",contentChange = "";
                                    VirtualDealer vd = new VirtualDealer();
                                    string result = vd.MapDealer(subValue[1]);
                                    int id;
                                    int.TryParse(result, out id);
                                    if (id == 1)
                                    {
                                        contentChange = vd.MapChangeContentConfigVD();
                                        result = vd.UpdateDealer();
                                        status = "[Success]";
                                    }
                                    string content = "'" + code + "': update config robot dealer '" + vd.Name + "': " + contentChange + " " + status;
                                    string comment = "[update config robot dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    StringResult = subValue[0] + "$" + result;
                                }
                            }
                            break;

                        case "KD485":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string status = "[Failed]";
                                    int id;
                                    int.TryParse(subValue[1], out id);
                                    string result = "0";
                                    VirtualDealer vd = new VirtualDealer();
                                    if (id > 0)
                                    {                                        
                                        vd.ID = id;
                                        result = vd.DeleteDealer(id);
                                        status = "[Success]";
                                    }
                                    string content = "'" + code + "': delete robot dealer ['" + vd.Name + "'] " + status;
                                    string comment = "[delete config robot dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    StringResult = subValue[0] + "$" + result;
                                }
                            }
                            break;

                        case "KD678":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string status = "[Failed]", contentChange = "";
                                    VirtualDealer vd = new VirtualDealer();
                                    string result = vd.MapDealer(subValue[1]);
                                    int id;
                                    int.TryParse(result, out id);
                                    if (id == 1)
                                    {
                                        contentChange = vd.MapChangeContentConfigVD();
                                        result = vd.UpdateDealerInfo();
                                        status = "[Success]";
                                    }
                                    string content = "'" + code + "': update config robot dealer '" + vd.Name + "': " + contentChange + " " + status;
                                    string comment = "[update config robot dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    StringResult = subValue[0] + "$" + result;
                                }
                            }
                            break;

                        case "KD346":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string result = "";
                                    VirtualDealer vd = new VirtualDealer();
                                    List<VirtualDealer> vds = new List<VirtualDealer>();
                                    vds = vd.GetAllVirtualDealer();
                                    for (int i = 0; i < vds.Count; i++)
                                    {
                                        result += vds[i].ExtractDealer() + "[";
                                    }
                                    StringResult = subValue[0] + "$" + result;
                                }
                            }
                            break;

                        case "KD394":
                            {
                                bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                if (checkip)
                                {
                                    string status = "[Failed]", contentChange = "";
                                    VirtualDealer vd = new VirtualDealer();
                                    string result = vd.MapDealer(subValue[1]);
                                    int id;
                                    int.TryParse(result, out id);
                                    if (id == 1)
                                    {
                                        contentChange = vd.MapChangeContentConfigVD();
                                        result = vd.UpdateDealerSymbol();
                                        status = "[Success]";
                                    }
                                    string content = "'" + code + "': update config robot dealer '" + vd.Name + "': " + contentChange + " " + status;
                                    string comment = "[update config robot dealer]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    StringResult = subValue[0] + "$" + result;
                                }
                            }
                            break;
                        #endregion

                        #region MONITOR STATUS SYMBOL
                        case "MonitorSymbol":
                            {
                                if (Business.Market.SymbolList != null)
                                {
                                    int count = Business.Market.SymbolList.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (Business.Market.SymbolList[j].Name == subValue[1])
                                        {
                                            StringResult = subValue[0] + "$Trade Status: " + Business.Market.SymbolList[j].IsTrade + "- Quote Status: " + Business.Market.SymbolList[j].IsQuote;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region COMMAND GET CLIENT LOG
                        case "GetClientLog":
                            {
                                //GetClientLog$LoginCode{StartTime{EndTime
                                string[] subCommand = subValue[1].Split('{');
                                if (subCommand.Length > 0)
                                {
                                    //int investorID = int.Parse(subCommand[0]);
                                    DateTime timeStart = DateTime.Parse(subCommand[1]);
                                    DateTime timeEnd = DateTime.Parse(subCommand[2]);

                                    if (Business.Market.InvestorList != null)
                                    {
                                        bool isExists = false;
                                        int count = Business.Market.InvestorList.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.InvestorList[i].Code == subCommand[0])
                                            {   
                                                #region PROCESS CHECK CLIENT IN MARKET
                                                if (Business.Market.ListClientLogs != null && Business.Market.ListClientLogs.Count > 0)
                                                {
                                                    int countClientLog = Business.Market.ListClientLogs.Count;

                                                    for (int j = 0; j < count; j++)
                                                    {
                                                        if (Business.Market.ListClientLogs[j].AdminCode == code)
                                                        {
                                                            Business.Market.ListClientLogs[j].InvestorID = Business.Market.InvestorList[i].InvestorID;
                                                            Business.Market.ListClientLogs[j].InvestorCode = subCommand[0];
                                                            Business.Market.ListClientLogs[j].IsComplete = false;
                                                            Business.Market.ListClientLogs[j].ClientLogs = new List<string>();

                                                            isExists = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!isExists)
                                                {
                                                    Business.ClientLog newClientLog = new ClientLog();
                                                    newClientLog.InvestorID = Business.Market.InvestorList[i].InvestorID;
                                                    newClientLog.InvestorCode = subCommand[0];
                                                    newClientLog.AdminCode = code;
                                                    newClientLog.IsComplete = false;
                                                    newClientLog.ClientLogs = new List<string>();

                                                    Business.Market.ListClientLogs.Add(newClientLog);
                                                }
                                                #endregion
                                                
                                                //SEND COMMAND TO CLIENT REQUEST LOG
                                                string strRequestActionLog = "ServerRequestLog$" + subCommand[0] + "{" + timeStart + "{" + timeEnd;
                                                Business.Market.InvestorList[i].ClientCommandQueue.Add(strRequestActionLog);

                                                System.Threading.Thread.Sleep(1000);
                                                StringResult = subValue[0] + "$True";
                                                isExists = true;
                                                
                                                break;
                                            }
                                        }

                                        if (!isExists)
                                            StringResult = subValue[0] + "$False";
                                    }
                                }
                                else
                                {
                                    StringResult = subValue[0] + "$False";
                                }
                            }
                            break;
                        #endregion
                    }
                }
            }

            return StringResult;
        }              
    }
}
