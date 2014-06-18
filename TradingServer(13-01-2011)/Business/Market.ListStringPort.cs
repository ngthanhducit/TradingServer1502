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
        public List<string> ExtractServerCommand(string Cmd, string ipAddress, string code)
        {
            List<string> StringResult = new List<string>();
            if (!string.IsNullOrEmpty(Cmd))
            {
                string[] subCommand = Cmd.Split('#');
                if (subCommand.Length > 0)
                {
                    int count = subCommand.Length;  
                    for (int i = 0; i < count; i++)
                    {
                        string[] subValue = subCommand[i].Split('$');
                        switch (subValue[0])
                        {
                            #region Function Class Symbol(LOG)
                            case "SelectSymbol":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        StringResult = this.SelectSymbolInSymbolList();
                                    }
                                    //#region INSERT SYSTEM LOG
                                    ////INSERT SYSTEM LOG
                                    //string content = "'" + code + "': " + StringResult.Count + " symbol has been request";
                                    //string comment = "[request symbol]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                                    //#endregion                                   
                                }
                                break;
                            #endregion

                            #region Function Class TradingConfig(SymbolConfig)
                            case "SelectTradingConfigBySymbolID":
                                {
                                    string[] listParameter = subValue[1].Split(',');
                                    string temp = string.Empty;
                                    if (listParameter.Length > 0)
                                    {
                                        int countParameter = listParameter.Length;
                                        string result = string.Empty;
                                        for (int j = 0; j < countParameter; j++)
                                        {
                                            temp = this.ExtractCommandServer(subValue[0] + "$" + listParameter[j], "", code);
                                            StringResult.Add(temp);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GET TRADING CONFIG BY LIST SYMBOL ID
                            case "GetTradingConfigByListSymbolID":
                                {
                                    string[] listSymbolID = subValue[1].Split(',');
                                    int countListSymbolID = listSymbolID.Length;
                                    for (int j = 0; j < count; j++)
                                    {
                                        string temp = string.Empty;
                                        temp = this.SelectTradingConfigBySymbolIDInSymbolList(int.Parse(listSymbolID[j]));
                                        StringResult.Add(subValue[0] + "$" + temp);
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class Investor Group
                            case "SelectInvestorGroup":
                                {
                                    string temp = string.Empty;
                                    temp = this.ExtractCommandServer(subValue[0], "", code);
                                    StringResult.Add(temp);
                                }
                                break;
                            #endregion

                            #region Function Class Investor Group Config
                            case "SelectInvestorGroupConfigByInvestorGroupID":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] listParameter = subValue[1].Split(',');
                                        string temp = string.Empty;
                                        if (listParameter.Length > 0)
                                        {
                                            int countParameter = listParameter.Length;
                                            string result = string.Empty;
                                            for (int j = 0; j < countParameter; j++)
                                            {
                                                temp = this.ExtractCommandServer(subValue[0] + "$" + listParameter[j], "", code);
                                                StringResult.Add(temp);
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class Security
                            case "SelectSecurity":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string temp = string.Empty;
                                        temp = this.ExtractCommandServer(subValue[0], "", code);
                                        StringResult.Add(temp);
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class Security Config
                            case "SelectSecurityConfigBySecurityID":
                                {
                                    string[] listParameter = subValue[1].Split(',');
                                    string temp = string.Empty;
                                    if (listParameter.Length > 0)
                                    {
                                        int countParameter = listParameter.Length;
                                        string result = string.Empty;
                                        for (int j = 0; j < countParameter; j++)
                                        {
                                            temp = this.ExtractCommandServer(subValue[0] + "$" + listParameter[j], "", code);
                                            result = listParameter[j] + "*" + temp + "@";
                                        }
                                        StringResult.Add(result);
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class Investor
                            case "SelectInvestor":
                                {
                                    string temp = string.Empty;
                                    temp = this.ExtractCommandServer(subValue[0], "", code);
                                    StringResult.Add(temp);
                                }
                                break;

                            #region GET ALL INVESTOR IN DATABASE(GET FROM TO)(LOG COMMENT)
                            case "SelectInvestorWithRowNumber":
                                {
                                    List<Business.Investor> Result = new List<Investor>();
                                    if (subValue.Length == 2)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 2)
                                        {
                                            int From = -1;
                                            int To = -1;
                                            int.TryParse(subParameter[0], out From);
                                            int.TryParse(subParameter[1], out To);
                                            int RowNumber = To - From;
                                            //Result = TradingServer.Facade.FacadeGetInvestorWithRowNumber(RowNumber, From);
                                            Result = TradingServer.Facade.FacadeGetInvestorFormTo(From, To);
                                            if (Result != null && Result.Count > 0)
                                            {
                                                int countInvestor = Result.Count;
                                                for (int j = 0; j < countInvestor; j++)
                                                {
                                                    string Message = string.Empty;
                                                    Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                        Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                        Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                        Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                        Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                        Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                        Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                                    StringResult.Add(Message);
                                                }
                                            }
                                            //else
                                            //{
                                            //if (From == 0)
                                            //{
                                            //    Result = TradingServer.Facade.FacadeGetAllInvestor();
                                            //    if (Result != null && Result.Count > 0)
                                            //    {
                                            //        int countInvestor = Result.Count;
                                            //        for (int j = 0; j < countInvestor; j++)
                                            //        {
                                            //            string Message = string.Empty;
                                            //            Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                            //                Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                            //                Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                            //                Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                            //                Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                            //                Result[j].RegisterDay + "," + Result[j].Comment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                            //                Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                            //            StringResult.Add(Message);
                                            //        }
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    string Message = subValue[0] + "$";
                                            //    StringResult.Add(Message);
                                            //}
                                            //}

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            //'2222': 22450 accounts have been requested                                            
                                            //string content = "'" + code + "': " + StringResult.Count + " accounts have been requested";
                                            //string comment = "[account request]";
                                            //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GET INVESTOR ONLINE IN RAM(GET FROM TO)
                            case "SelectInvestorOnline":
                                {
                                    List<Business.Investor> Result = new List<Investor>();
                                    if (subValue.Length == 2)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 2)
                                        {
                                            int From = -1;
                                            int To = -1;
                                            int.TryParse(subParameter[0], out From);
                                            int.TryParse(subParameter[1], out To);
                                            Result = TradingServer.Facade.FacadeGetInvestorOnline(From, To);
                                            if (Result != null && Result.Count > 0)
                                            {
                                                int countInvestor = Result.Count;
                                                for (int j = 0; j < countInvestor; j++)
                                                {
                                                    string Message = string.Empty;
                                                    Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                        Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                        Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                        Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                        Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                        Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                        Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                                    StringResult.Add(Message);
                                                }
                                            }
                                            else
                                            {
                                                string Message = subValue[0] + "$";
                                                StringResult.Add(Message);
                                            }
                                        }
                                    }

                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': 22450 accounts have been requested                                            
                                    string content = "'" + code + "': " + StringResult.Count + " account have been requested";
                                    string comment = "[account request]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion
                                }
                                break;
                            #endregion

                            #region GET INVESTOR ONLINE IN RAM BY LIST GROUP(GET FROM TO)
                            case "GetInvestorOnlineByListGroup":
                                {
                                    List<int> ListGroupID = new List<int>();
                                    int From = -1;
                                    int To = -1;
                                    if (subValue[1].Length > 0)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        int.TryParse(subParameter[0], out From);
                                        int.TryParse(subParameter[1], out To);
                                        int countParameter = subParameter.Length;
                                        for (int j = 2; j < countParameter; j++)
                                        {
                                            int InvestorGroupID = -1;
                                            int.TryParse(subParameter[j], out InvestorGroupID);
                                            ListGroupID.Add(InvestorGroupID);
                                        }

                                        List<Business.Investor> Result = new List<Investor>();

                                        if (ListGroupID != null && ListGroupID.Count > 0)
                                        {
                                            int countListGroup = ListGroupID.Count;
                                            for (int n = 0; n < countListGroup; n++)
                                            {
                                                Result = TradingServer.Facade.FacadeGetInvestorOnlineByGroupID(From, To, ListGroupID[n]);

                                                if (Result != null && Result.Count > 0)
                                                {
                                                    int countResult = Result.Count;
                                                    for (int j = 0; j < countResult; j++)
                                                    {
                                                        string Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," +
                                                            Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                            Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                            Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                            Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                            Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                            Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                            Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                                        StringResult.Add(Message);
                                                    }
                                                }
                                            }
                                        }

                                        if (StringResult.Count == 0)
                                        {
                                            string Message = subValue[0] + "$";
                                            StringResult.Add(Message);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GET INVESTOR WITH COMMAND IN RAM(GET FROM TO)
                            case "SelectInvestorWithCommand":
                                {
                                    List<Business.Investor> Result = new List<Investor>();
                                    if (subValue.Length == 2)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 2)
                                        {
                                            int From = -1;
                                            int To = -1;
                                            int.TryParse(subParameter[0], out From);
                                            int.TryParse(subParameter[1], out To);

                                            Result = TradingServer.Facade.FacadeGetInvestorWithCommand(From, To);
                                            if (Result != null && Result.Count > 0)
                                            {
                                                int countResult = Result.Count;
                                                for (int j = 0; j < countResult; j++)
                                                {
                                                    string Message = string.Empty;
                                                    Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                        Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                        Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                        Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                        Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                        Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                        Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].Credit;

                                                    StringResult.Add(Message);
                                                }
                                            }
                                            else
                                            {
                                                string Message = subValue[0] + "$";
                                                StringResult.Add(Message);
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GET INVESTOR WITH MARGIN LEVEL IN RAM
                            case "SelectInvestorWithMarginLevel":
                                {
                                    List<Business.Investor> Result = new List<Investor>();
                                    Result = TradingServer.Facade.FacadeGetInvestorWithMarginLevel();
                                    if (Result != null && Result.Count > 0)
                                    {
                                        int countInvestor = Result.Count;
                                        for (int j = 0; j < countInvestor; j++)
                                        {
                                            string Message = string.Empty;
                                            Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;
                            #endregion

                            #region GET INVESTOR BY INVESTOR GROUP ID(GET FROM TO) IN RAM
                            case "GetInvestorByInvestorGroupID":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        if (subValue.Length == 2)
                                        {
                                            int InvestorGroupID = -1;
                                            int From = -1;
                                            int To = -1;

                                            string[] subParameter = subValue[1].Split(',');

                                            if (subParameter.Length > 3)
                                            {
                                                int.TryParse(subParameter[0], out From);
                                                int.TryParse(subParameter[1], out To);
                                                int.TryParse(subParameter[2], out InvestorGroupID);

                                                List<Business.Investor> Result = new List<Investor>();

                                                Result = TradingServer.Facade.FacadeGetInvestorByInvestorGroupID(InvestorGroupID, From, To);

                                                if (Result != null && Result.Count > 0)
                                                {
                                                    int countResult = Result.Count;
                                                    for (int j = 0; j < countResult; j++)
                                                    {
                                                        string Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + InvestorGroupID + "," +
                                                            Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                            Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                            Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                            Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                            Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                            Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                                        StringResult.Add(Message);
                                                    }
                                                }
                                                else
                                                {
                                                    string Message = subValue[0] + "$";
                                                    StringResult.Add(Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GET INVESTOR BY INVESTOR GROUP ID GET FROM TO IN DATABASE(LOG COMMENT)
                            case "GetInvestorByInvestorGroupIDWithDB":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        List<Business.Investor> Result = new List<Investor>();
                                        if (subValue.Length == 2)
                                        {
                                            int From = -1;
                                            int To = -1;
                                            int InvestorGroupID = -1;

                                            string[] subParameter = subValue[1].Split(',');
                                            if (subParameter.Length == 3)
                                            {
                                                int.TryParse(subParameter[0], out From);
                                                int.TryParse(subParameter[1], out To);
                                                int.TryParse(subParameter[2], out InvestorGroupID);
                                                int RowNumber = To - From;
                                                //Result = TradingServer.Facade.FacadeGetInvestorByInvestorGroupDB(InvestorGroupID, RowNumber, From);
                                                Result = TradingServer.Facade.FacadeGetInvestorByGroupFromTo(InvestorGroupID, From, To);
                                                if (Result != null && Result.Count > 0)
                                                {
                                                    int countInvestor = Result.Count;
                                                    for (int j = 0; j < countInvestor; j++)
                                                    {
                                                        string Message = string.Empty;
                                                        Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                            Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                            Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                            Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                            Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                            Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                            Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline + "," +
                                                            Result[j].Margin + "," + Result[j].FreezeMargin + "," + Result[j].PhonePwd + "," + Result[j].IDPassport + "," +
                                                            Result[j].TotalDeposit;

                                                        StringResult.Add(Message);
                                                    }
                                                }
                                                //else
                                                //{
                                                //    if (From == 0)
                                                //    {
                                                //        Result = TradingServer.Facade.FacadeGetInvestorByInvestorGroup(InvestorGroupID);
                                                //        if (Result != null && Result.Count > 0)
                                                //        {
                                                //            int countInvestor = Result.Count;
                                                //            for (int j = 0; j < countInvestor; j++)
                                                //            {
                                                //                string Message = string.Empty;
                                                //                Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," + Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                //                    Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                //                    Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                //                    Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                //                    Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                //                    Result[j].RegisterDay + "," + Result[j].Comment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                //                    Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline;

                                                //                StringResult.Add(Message);
                                                //            }
                                                //        }
                                                //    }                                                
                                                //}
                                            }
                                        }

                                        if (StringResult.Count == 0)
                                        {
                                            string Message = subValue[0] + "$";
                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                    #region INSERT SYSTEM LOG
                                    //INSERT SYSTEM LOG
                                    //'2222': 22450 accounts have been requested                                                                                
                                    //string content = "'" + code + "': " + StringResult.Count + " account have been requested";
                                    //string comment = "[account request]";
                                    //TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                    #endregion
                                }
                                break;
                            #endregion

                            #region GET INVESTOR BY INVESTOR GROUP LIST(GET FROM TO) IN RAM
                            case "GetInvestorByListGroup":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        List<int> ListGroupID = new List<int>();
                                        int From = -1;
                                        int To = -1;
                                        if (subValue[1].Length > 0)
                                        {
                                            string[] subParameter = subValue[1].Split(',');
                                            int.TryParse(subParameter[0], out From);
                                            int.TryParse(subParameter[1], out To);
                                            int countParameter = subParameter.Length;
                                            for (int j = 2; j < countParameter; j++)
                                            {
                                                int InvestorGroupID = -1;
                                                int.TryParse(subParameter[j], out InvestorGroupID);
                                                ListGroupID.Add(InvestorGroupID);
                                            }

                                            List<Business.Investor> Result = new List<Investor>();

                                            Result = TradingServer.Facade.FacadeGetInvestorByGroupList(From, To, ListGroupID);

                                            if (Result != null && Result.Count > 0)
                                            {
                                                int countResult = Result.Count;
                                                for (int j = 0; j < countResult; j++)
                                                {
                                                    string Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].InvestorStatusID + "," +
                                                        Result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                        Result[j].AgentID + "," + Result[j].Balance + "," + Result[j].Credit + "," + Result[j].Code + "," +
                                                        Result[j].IsDisable + "," + Result[j].TaxRate + "," + Result[j].Leverage + "," +
                                                        Result[j].InvestorProfileID + "," + Result[j].Address + "," + Result[j].Phone + "," +
                                                        Result[j].City + "," + Result[j].Country + "," + Result[j].Email + "," + Result[j].ZipCode + "," +
                                                        Result[j].RegisterDay + "," + Result[j].InvestorComment + "," + Result[j].State + "," + Result[j].NickName + "," +
                                                        Result[j].AllowChangePwd + "," + Result[j].ReadOnly + "," + Result[j].SendReport + "," + Result[j].IsOnline + "," +
                                                        Result[j].Margin + "," + Result[j].FreezeMargin + "," + Result[j].PhonePwd + "," + Result[j].IDPassport;

                                                    StringResult.Add(Message);
                                                }
                                            }
                                        }
                                    }
                                    if (StringResult.Count == 0)
                                    {
                                        string Message = subValue[0] + "$";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;
                            #endregion

                            case "GetInvestorAccountByListCode":
                                {
                                    string[] subParameter = subValue[1].Split(',');
                                    List<string> listCode = new List<string>();
                                    if (subParameter != null && subParameter.Length > 0)
                                    {
                                        int countCode = subParameter.Length;
                                        for (int j = 0; j < countCode; j++)
                                        {
                                            listCode.Add(subParameter[j]);
                                        }
                                    }

                                    List<Business.Investor> result = TradingServer.Facade.FacadeGetInvestorByListCode(listCode);

                                    if (result != null && result.Count > 0)
                                    {
                                        int countResult = result.Count;
                                        for (int j = 0; j < countResult; j++)
                                        {
                                            string Message = subValue[0] + "$" + result[j].InvestorID + "," + result[j].InvestorStatusID + "," +
                                                        result[j].InvestorGroupInstance.InvestorGroupID + "," +
                                                        result[j].AgentID + "," + result[j].Balance + "," + result[j].Credit + "," + result[j].Code + "," +
                                                        result[j].IsDisable + "," + result[j].TaxRate + "," + result[j].Leverage + "," +
                                                        result[j].InvestorProfileID + "," + result[j].Address + "," + result[j].Phone + "," +
                                                        result[j].City + "," + result[j].Country + "," + result[j].Email + "," + result[j].ZipCode + "," +
                                                        result[j].RegisterDay + "," + result[j].InvestorComment + "," + result[j].State + "," + result[j].NickName + "," +
                                                        result[j].AllowChangePwd + "," + result[j].ReadOnly + "," + result[j].SendReport + "," + result[j].IsOnline + "," +
                                                        result[j].Margin + "," + result[j].FreezeMargin + "," + result[j].PhonePwd + "," + result[j].IDPassport;

                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string message = subValue[0] + "$";
                                        StringResult.Add(message);
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class Open Trade
                            //GET ALL ONLINE COMMAND IN INVESTOR LIST OF CLASS MARKET(SERVER CALL)
                            case "SelectOnlineCommand":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        List<Business.OpenTrade> Result = new List<OpenTrade>();
                                        if (subValue.Length == 2)
                                        {
                                            string[] subParameter = subValue[1].Split(',');
                                            int From = -1;
                                            int To = -1;
                                            int.TryParse(subParameter[0], out From);
                                            int.TryParse(subParameter[1], out To);

                                            Result = TradingServer.Facade.FacadeGetOnlineCommand(From, To);
                                            if (Result != null && Result.Count > 0)
                                            {
                                                int countCommand = Result.Count;
                                                for (int j = 0; j < countCommand; j++)
                                                {
                                                    string Message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                        Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," +
                                                        Result[j].ID + "," + Result[j].Investor.InvestorID + "," + Result[j].IsClose + "," +
                                                        Result[j].IsHedged + "," + Result[j].Margin + "," + Result[j].MaxDev + "," + Result[j].OpenPrice + "," +
                                                        Result[j].OpenTime + "," + Result[j].Profit + "," + Result[j].Size + "," + Result[j].StopLoss + "," +
                                                        Result[j].Swap + "," + Result[j].Symbol.Name + "," + Result[j].TakeProfit + "," + Result[j].Taxes + "," +
                                                        Result[j].Type.Name + "," + Result[j].Type.ID + "," + Result[j].Symbol.ContractSize + "," +
                                                        /*Result[j].Symbol.SpreadDifference*/ Result[j].SpreaDifferenceInOpenTrade + "," + Result[j].Symbol.Currency + "," +
                                                        Result[j].Comment + "," + Result[j].AgentCommission;

                                                    StringResult.Add(Message);
                                                }
                                            }
                                            else
                                            {
                                                string Message = subValue[0] + "$";
                                                StringResult.Add(Message);
                                            }
                                        }
                                    }
                                }
                                break;

                            //SELECT ONLINE COMMAND BY INVESTOR ID IN INVESTOR LIST OF CLASS MARKET(SERVER CALL)
                            case "SelectOnlineCommandByInvestorID":
                                {
                                    List<Business.OpenTrade> Result = new List<OpenTrade>();
                                    int InvestorID = -1;
                                    int.TryParse(subValue[1], out InvestorID);
                                    Result = TradingServer.Facade.FacadeGetOnlineCommandByInvestorID(InvestorID);

                                    if (Result != null && Result.Count > 0)
                                    {
                                        int countCommand = Result.Count;
                                        for (int j = 0; j < countCommand; j++)
                                        {
                                            string Message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                Result[j].Investor.InvestorID + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + Result[j].Symbol.Name + "," + Result[j].TakeProfit + "," +
                                                Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + Result[j].Symbol.ContractSize + "," +
                                                Result[j].Symbol.Currency + "," + Result[j].Comment + "," + Result[j].AgentCommission;

                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "SelectOnlineCommandByInvestorWithStartEnd":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        List<Business.OpenTrade> Result = new List<OpenTrade>();
                                        int InvestorID = -1;
                                        int start = 0;
                                        int end = 0;

                                        string[] subParameter = subValue[1].Split(',');
                                        int.TryParse(subParameter[0], out InvestorID);
                                        int.TryParse(subParameter[1], out start);
                                        int.TryParse(subParameter[2], out end);

                                        Result = TradingServer.Facade.FacadeGetOpenTradeByInvestorWithStartEnd(InvestorID, start, end);

                                        if (Result != null && Result.Count > 0)
                                        {
                                            int countCommand = Result.Count;
                                            for (int j = 0; j < countCommand; j++)
                                            {
                                                string Message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                    Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                    Result[j].Investor.InvestorID + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                    Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                    Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + Result[j].Symbol.Name + "," + Result[j].TakeProfit + "," +
                                                    Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + Result[j].Symbol.ContractSize + "," + Result[j].SpreaDifferenceInOpenTrade + "," +
                                                    Result[j].Symbol.Currency + "," + Result[j].Comment + "," + Result[j].AgentCommission;

                                                StringResult.Add(Message);
                                            }
                                        }
                                        else
                                        {
                                            string Message = subValue[0] + "$";
                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "SelectOnlineCommandByGroupList":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    { 
                                        List<int> ListGroupID = new List<int>();
                                        int From = -1;
                                        int To = -1;
                                        if (subValue[1].Length > 0)
                                        {
                                            string[] subParameter = subValue[1].Split(',');
                                            if (subParameter.Length > 0)
                                            {
                                                int.TryParse(subParameter[0], out From);
                                                int.TryParse(subParameter[1], out To);

                                                int countParameter = subParameter.Length;
                                                for (int j = 2; j < countParameter; j++)
                                                {
                                                    int InvestorGroupID = -1;
                                                    int.TryParse(subParameter[j], out InvestorGroupID);
                                                    ListGroupID.Add(InvestorGroupID);
                                                }

                                                List<Business.OpenTrade> Result = new List<OpenTrade>();
                                                Result = TradingServer.Facade.FacadeGetOpenTradeByGroupList(From, To, ListGroupID);

                                                if (Result != null && Result.Count > 0)
                                                {
                                                    int countCommand = Result.Count;
                                                    for (int j = 0; j < countCommand; j++)
                                                    {
                                                        string Message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                            Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                            Result[j].Investor.InvestorID + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                            Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                            Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + Result[j].Symbol.Name + "," + Result[j].TakeProfit + "," +
                                                            Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + Result[j].Symbol.ContractSize + "," +
                                                            Result[j].SpreaDifferenceInOpenTrade + "," + Result[j].Symbol.Currency + "," + Result[j].Comment + "," + Result[j].AgentCommission + "," +
                                                            Result[j].IsActivePending + "," + Result[j].IsStopLossAndTakeProfit;

                                                        StringResult.Add(Message);
                                                    }
                                                }
                                                else
                                                {
                                                    string Message = subValue[0] + "$";
                                                    StringResult.Add(Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case "GetOnlineCommandByListInvestorCode":
                                {
                                    List<string> listCode = new List<string>();
                                    string[] subParameter = subValue[1].Split(',');
                                    if(subParameter!=null && subParameter.Length>0)
                                    {
                                        int countCode = subParameter.Length;
                                        for (int j = 0; j < countCode; j++)
                                        {
                                            listCode.Add(subParameter[j]);
                                        }
                                    }

                                    List<Business.OpenTrade> result = TradingServer.Facade.FacadeGetOpenTradeByListInvestorCode(listCode);
                                    if (result != null)
                                    {
                                        int countResult = result.Count;
                                        for (int j = 0; j < countResult; j++)
                                        {
                                            string Message = subValue[0] + "$" + result[j].ClientCode + "," + result[j].ClosePrice + "," + result[j].CloseTime + "," +
                                                            result[j].CommandCode + "," + result[j].Commission + "," + result[j].ExpTime + "," + result[j].ID + "," +
                                                            result[j].Investor.InvestorID + "," + result[j].IsClose + "," + result[j].IsHedged + "," + result[j].Margin + "," +
                                                            result[j].MaxDev + "," + result[j].OpenPrice + "," + result[j].OpenTime + "," + result[j].Profit + "," +
                                                            result[j].Size + "," + result[j].StopLoss + "," + result[j].Swap + "," + result[j].Symbol.Name + "," + result[j].TakeProfit + "," +
                                                            result[j].Taxes + "," + result[j].Type.Name + "," + result[j].Type.ID + "," + result[j].Symbol.ContractSize + "," +
                                                            result[j].SpreaDifferenceInOpenTrade + "," + result[j].Symbol.Currency + "," + result[j].Comment + "," + result[j].AgentCommission + "," +
                                                            result[j].Investor.Code;

                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string message = subValue[0] + "$";
                                        StringResult.Add(message);
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class IGroupSymbol
                            case "AddIGroupSymbol":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        int ResultAddNew = -1;
                                        List<Business.IGroupSymbol> Result = new List<IGroupSymbol>();
                                        Result = this.ExtractIGroupSymbol(subValue[1]);
                                        //Call Function Add New IGroup Symbol
                                        if (Result != null)
                                        {
                                            int countResult = Result.Count;
                                            for (int j = 0; j < countResult; j++)
                                            {
                                                ResultAddNew = TradingServer.Facade.FacadeAddNewIGroupSymbol(Result[j].SymbolID, Result[j].InvestorGroupID);

                                                string Message = subValue[0] + "$" + ResultAddNew.ToString() + "," + Result[j].SymbolID + "," + Result[j].InvestorGroupID;
                                                StringResult.Add(Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "SelectIGroupSymbolConfigByIGroupSymbolID":
                                {
                                    if (!string.IsNullOrEmpty(subValue[1]))
                                    {
                                        string[] listParameter = subValue[1].Split(',');
                                        string temp = string.Empty;
                                        if (listParameter.Length > 0)
                                        {
                                            int countParameter = listParameter.Length;
                                            string result = string.Empty;
                                            for (int j = 0; j < countParameter; j++)
                                            {
                                                temp = this.ExtractCommandServer(subValue[0] + "$" + listParameter[j], "", code);
                                                StringResult.Add(temp);
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class IGroupSecurity
                            case "AddIGroupSecurity":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        if (!string.IsNullOrEmpty(subValue[1]))
                                        {
                                            int ResultAddNew = -1;
                                            List<Business.IGroupSecurity> Result = new List<IGroupSecurity>();
                                            Result = this.ExtractIGroupSecurity(subValue[1]);
                                            if (Result != null)
                                            {
                                                int countResult = Result.Count;
                                                for (int j = 0; j < countResult; j++)
                                                {
                                                    ResultAddNew = TradingServer.Facade.FacadeAddIGroupSecurity(Result[j].InvestorGroupID, Result[j].SecurityID);

                                                    string Message = subValue[0] + "$" + ResultAddNew.ToString() + "," + Result[j].InvestorGroupID + "," + Result[j].SecurityID;
                                                    StringResult.Add(Message);
                                                }
                                            }

                                            //SEND COMMAND TO AGENT SERVER
                                            string strAgent = string.Empty;
                                            if (StringResult != null)
                                            {
                                                int countResult = StringResult.Count;
                                                for (int j = 0; j < countResult; j++)
                                                {
                                                    strAgent += StringResult[j] + "|";
                                                }
                                            }

                                            #region SEND NOTIFY TO AGENT
                                            if (strAgent.EndsWith("|"))
                                                strAgent = strAgent.Remove(strAgent.Length - 1, 1);

                                            Business.AgentNotify newAgentNotify = new AgentNotify();
                                            newAgentNotify.NotifyMessage = strAgent;
                                            TradingServer.Agent.AgentConfig.Instance.AddNotifyToAgent(newAgentNotify);
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "SelectIGroupSecurityConfigByIGroupSecurityID":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        if (!string.IsNullOrEmpty(subValue[1]))
                                        {
                                            string[] listParameter = subValue[1].Split(',');
                                            string temp = string.Empty;
                                            if (listParameter.Length > 0)
                                            {
                                                int countParameter = listParameter.Length;
                                                string result = string.Empty;
                                                for (int j = 0; j < countParameter; j++)
                                                {
                                                    temp = this.ExtractCommandServer(subValue[0] + "$" + listParameter[j], "", code);
                                                    StringResult.Add(temp);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region Function Class Investor Status
                            case "SelectInvestorStatus":
                                {
                                    List<Business.InvestorStatus> Result = new List<InvestorStatus>();
                                    Result = TradingServer.Facade.FacadeGetAllInvestorStatus();

                                    if (Result != null)
                                    {
                                        int countInvestorStatus = Result.Count;
                                        for (int j = 0; j < countInvestorStatus; j++)
                                        {
                                            string Message = subValue[0] + "$" + Result[j].InvestorStatusID + "," + Result[j].Name;

                                            StringResult.Add(Message);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION CLASS MARKET
                            case "TickOnline":
                                {
                                    if (Business.Market.SymbolList != null)
                                    {
                                        int countSymbol = Business.Market.SymbolList.Count;
                                        for (int j = 0; j < countSymbol; j++)
                                        {
                                            if (Business.Market.SymbolList[j].TickValue != null && Business.Market.SymbolList[j].TickValue.Ask > 0)
                                            {
                                                string Message = subValue[0] + "$" + Business.Market.SymbolList[j].TickValue.Ask + "," +
                                                    Business.Market.SymbolList[j].TickValue.Bid + "," + Business.Market.SymbolList[j].TickValue.Status + "," +
                                                    Business.Market.SymbolList[j].TickValue.SymbolID + "," + Business.Market.SymbolList[j].TickValue.SymbolName + "," +
                                                    Business.Market.SymbolList[j].TickValue.TickTime;

                                                StringResult.Add(Message);
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION CLASS INVESTOR ACCOUNT LOG
                            case "GetInvestorAccountLogByInvestor":
                                {
                                    List<Business.InvestorAccountLog> Result = new List<InvestorAccountLog>();
                                    int InvestorID = -1;
                                    int.TryParse(subValue[1], out InvestorID);
                                    Result = TradingServer.Facade.FacadeGetInvestorAccountLogByInvestorID(InvestorID);
                                    if (Result != null)
                                    {
                                        int countResult = Result.Count;
                                        for (int j = 0; j < countResult; j++)
                                        {
                                            string Message = subValue[0] + "$" + Result[j].InvestorID + "," + Result[j].Name + "," + Result[j].ID + "," +
                                                Result[j].DealID + "," + Result[j].Date + "," + Result[j].Comment + "," + Result[j].Amount;

                                            StringResult.Add(Message);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION CANDLES
                            case "GetCandlesToDay":
                                {
                                    int TimeFrame = 0;
                                    string[] subParameter = subValue[1].Split(',');
                                    List<string> result = new List<string>();
                                    if (subParameter.Length == 2)
                                    {
                                        int.TryParse(subParameter[1], out TimeFrame);
                                        result = ProcessQuoteLibrary.FacadeDataLog.FacadeGetPriceToDay(subParameter[0], TimeFrame);
                                        if (result != null)
                                        {
                                            int countResult = result.Count;
                                            for (int j = 0; j < countResult; j++)
                                            {
                                                string Message = subValue[0] + "$" + result[j];
                                                StringResult.Add(Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "GetCandlesInTime":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        int TimeFrame = 0;
                                        DateTime StartTime = new DateTime();
                                        DateTime endTime = new DateTime();
                                        string[] subParameter = subValue[1].Split(',');
                                        List<string> Result = new List<string>();
                                        if (subParameter.Length == 4)
                                        {
                                            int.TryParse(subParameter[1], out TimeFrame);
                                            DateTime.TryParse(subParameter[2], out StartTime);
                                            DateTime.TryParse(subParameter[3], out endTime);
                                            //Result = ProcessQuoteLibrary.FacadeDataLog.FacadeGetPriceInTime(subParameter[0], TimeFrame, StartTime, endTime);
                                            Result = ProcessQuoteLibrary.FacadeDataLog.FacadeGetCandlesStartEndTime(subParameter[0], TimeFrame, StartTime, endTime);
                                            if (Result != null && Result.Count > 0)
                                            {
                                                int countResult = Result.Count;
                                                for (int j = 0; j < countResult; j++)
                                                {
                                                    string Message = subValue[0] + "$" + Result[j];
                                                    StringResult.Add(Message);
                                                }
                                            }
                                            else
                                            {
                                                string Message = subValue[0] + "$";
                                                StringResult.Add(Message);
                                            }
                                        }
                                        else
                                        {
                                            string Message = subValue[0] + "$";
                                            StringResult.Add(Message);
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION MARKET CONFIG
                            case "GetMarketConfig":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        if (Business.Market.MarketConfig != null)
                                        {
                                            int countMarketConfig = Business.Market.MarketConfig.Count;
                                            for (int j = 0; j < countMarketConfig; j++)
                                            {
                                                string Message = subValue[0] + "$" + Business.Market.MarketConfig[j].ParameterItemID + "," +
                                                    Business.Market.MarketConfig[j].SecondParameterID + "," +
                                                    Business.Market.MarketConfig[j].Code + "," +
                                                    Business.Market.MarketConfig[j].Name + "," +
                                                    Business.Market.MarketConfig[j].BoolValue + "," +
                                                    Business.Market.MarketConfig[j].StringValue + "," +
                                                    Business.Market.MarketConfig[j].NumValue + "," +
                                                    Business.Market.MarketConfig[j].DateValue;

                                                StringResult.Add(Message);
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION ALERT
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
                                        if (Result != null && Result.Count > 0)
                                        {
                                            for (int n = 0; n < Result.Count; n++)
                                            {
                                                temp = subValue[0] + "$" + Result[n].ID + "," + Result[n].Symbol + "," + Result[n].Email + "," + Result[n].PhoneNumber
                                                        + "," + Result[n].Value + "," + Result[n].AlertCondition.ToString() + "," + Result[n].AlertAction.ToString() + "," + Result[n].IsEnable
                                                        + "," + Result[n].DateCreate + "," + Result[n].DateActive + "," + Result[n].InvestorID + "," + Result[n].Notification;
                                                StringResult.Add(temp);
                                            }
                                        }
                                        else
                                        {
                                            string Message = subValue[0] + "$";
                                            StringResult.Add(Message);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION NEWS
                            case "SelectTopNews":
                                {
                                    string temp = string.Empty;
                                    List<Business.News> Result = new List<News>();
                                    Result = Market.NewsList;
                                    if (Result != null && Result.Count > 0)
                                    {
                                        for (int n = 0; n < Result.Count; n++)
                                        {
                                            temp = subValue[0] + "$" + Result[n].ID + "█" + Result[n].Title + "█" + Result[n].Catetory + "█" + Result[n].DateCreated
                                                    + "█" + Result[n].Body;
                                            StringResult.Add(temp);
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION ORDER DATA
                            case "GetOrderWithDeal":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 3)
                                        {
                                            List<Business.OpenTrade> Result = new List<OpenTrade>();

                                            Business.OpenTrade newOpenTrade = new OpenTrade();
                                            newOpenTrade = TradingServer.Facade.FacadeGetHistoryByCommandCode(subParameter[0]);

                                            if (newOpenTrade == null || newOpenTrade.ID <= 0)
                                            {
                                                if (Business.Market.CommandExecutor != null)
                                                {
                                                    int countCommand = Business.Market.CommandExecutor.Count;
                                                    for (int j = 0; j < countCommand; j++)
                                                    {
                                                        if (Business.Market.CommandExecutor[j].CommandCode == subParameter[0])
                                                        {
                                                            newOpenTrade = Business.Market.CommandExecutor[j];
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (newOpenTrade != null && newOpenTrade.ID > 0)
                                            {
                                                string symbolName = "";
                                                double contractSize = 0;
                                                string currency = "";
                                                if (newOpenTrade.Symbol != null)
                                                {
                                                    symbolName = newOpenTrade.Symbol.Name;
                                                    contractSize = newOpenTrade.Symbol.ContractSize;
                                                    currency = newOpenTrade.Symbol.Currency;
                                                }

                                                string message = subValue[0] + "$" + newOpenTrade.ClientCode + "," + newOpenTrade.ClosePrice + "," + newOpenTrade.CloseTime + "," +
                                                   newOpenTrade.CommandCode + "," + newOpenTrade.Commission + "," + newOpenTrade.ExpTime + "," + newOpenTrade.ID + "," +
                                                   newOpenTrade.Investor.Code + "," + newOpenTrade.IsClose + "," + newOpenTrade.IsHedged + "," + newOpenTrade.Margin + "," +
                                                   newOpenTrade.MaxDev + "," + newOpenTrade.OpenPrice + "," + newOpenTrade.OpenTime + "," + newOpenTrade.Profit + "," +
                                                   newOpenTrade.Size + "," + newOpenTrade.StopLoss + "," + newOpenTrade.Swap + "," + symbolName + "," + newOpenTrade.TakeProfit + "," +
                                                   newOpenTrade.Taxes + "," + newOpenTrade.Type.Name + "," + newOpenTrade.Type.ID + "," + contractSize + "," +
                                                   currency + "," + newOpenTrade.Taxes + "," + newOpenTrade.Comment + "," + newOpenTrade.AgentCommission + "," + newOpenTrade.Investor.InvestorID;

                                                StringResult.Add(message);
                                            }

                                            if (StringResult.Count == 0)
                                            {
                                                string message = subValue[0] + "$end";
                                                StringResult.Add(message);
                                            }

                                            if (StringResult.Count == 0)
                                            {
                                                string message = subValue[0] + "$end";
                                                StringResult.Add(message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "GetAllOrderData":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 3)
                                        {
                                            List<Business.OpenTrade> Result = new List<OpenTrade>();
                                            int InvestorID = 0;
                                            int Start = 0;
                                            int End = 0;

                                            int.TryParse(subParameter[1], out Start);
                                            int.TryParse(subParameter[2], out End);

                                            int RowNumber = End - Start;

                                            bool isWildcards = this.IsWildCards(subParameter[0]);

                                            if (isWildcards)
                                            {
                                                #region GET ORDER WITH WILDCARDS
                                                //GET ALL ORDER WITH WILDCARDS
                                                Result = TradingServer.Facade.FacadeGetAllHistoryWithStartEnd(RowNumber, Start);

                                                #region START GET ONLINE ORDER
                                                if (Start == 0)
                                                {
                                                    bool isExistsListOrder = false;
                                                    if (Business.Market.TempListOrder != null)
                                                    {
                                                        int countTempListOrder = Business.Market.TempListOrder.Count;
                                                        for (int j = 0; j < countTempListOrder; j++)
                                                        {
                                                            if (Business.Market.TempListOrder[j].AdminCode == code)
                                                            {
                                                                if (Business.Market.CommandExecutor != null)
                                                                {
                                                                    for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                                    {
                                                                        if (Business.Market.TempListOrder[j].ListOrder == null)
                                                                            Business.Market.TempListOrder[j].ListOrder = new List<OpenTrade>();

                                                                        Business.Market.TempListOrder[j].ListOrder.Add(Business.Market.CommandExecutor[n]);
                                                                    }
                                                                }

                                                                isExistsListOrder = true;

                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (!isExistsListOrder)
                                                    {
                                                        List<Business.OpenTrade> tempOrder = new List<OpenTrade>();
                                                        if (Business.Market.CommandExecutor != null)
                                                        {
                                                            for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                            {
                                                                tempOrder.Add(Business.Market.CommandExecutor[n]);
                                                            }
                                                        }

                                                        if (tempOrder != null && tempOrder.Count > 0)
                                                        {
                                                            Business.OrderInvestor newOrderInvestor = new OrderInvestor();
                                                            newOrderInvestor.AdminCode = code;
                                                            newOrderInvestor.ListOrder = tempOrder;

                                                            Business.Market.TempListOrder.Add(newOrderInvestor);
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region GET ORDER ONLINE
                                                if (Result == null || Result.Count <= 0)
                                                {
                                                    if (Business.Market.TempListOrder != null)
                                                    {
                                                        int countTempOpenTrade = Business.Market.TempListOrder.Count;
                                                        for (int m = 0; m < countTempOpenTrade; m++)
                                                        {
                                                            if (Business.Market.TempListOrder[m].AdminCode == code)
                                                            {
                                                                if (End > Business.Market.TempListOrder[m].ListOrder.Count)
                                                                    End = Business.Market.TempListOrder[m].ListOrder.Count;

                                                                if (Start > End)
                                                                    Start = 0;

                                                                for (int n = Start; n < End; n++)
                                                                {
                                                                    Result.Add(Business.Market.TempListOrder[m].ListOrder[n]);
                                                                    Business.Market.TempListOrder[m].ListOrder.RemoveAt(n);
                                                                    n--;

                                                                    if (End > Business.Market.TempListOrder[m].ListOrder.Count)
                                                                        End = Business.Market.TempListOrder[m].ListOrder.Count;

                                                                    if (Business.Market.TempListOrder[m].ListOrder.Count == 0 ||
                                                                        Business.Market.TempListOrder[m].ListOrder == null)
                                                                    {
                                                                        Business.Market.TempListOrder.RemoveAt(m);
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region GET HISTORY BY INVESTOR ID
                                                if (Result != null && Result.Count > 0)
                                                {
                                                    int countResult = Result.Count;
                                                    for (int j = 0; j < countResult; j++)
                                                    {
                                                        string symbolName = "";
                                                        double contractSize = 0;
                                                        string currency = "";
                                                        if (Result[j].Symbol != null)
                                                        {
                                                            symbolName = Result[j].Symbol.Name;
                                                            contractSize = Result[j].Symbol.ContractSize;
                                                            currency = Result[j].Symbol.Currency;
                                                        }

                                                        string message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                           Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                           Result[j].Investor.Code + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                           Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                           Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + symbolName + "," + Result[j].TakeProfit + "," +
                                                           Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + contractSize + "," +
                                                           currency + "," + Result[j].Taxes + "," + Result[j].Comment + "," + Result[j].AgentCommission + "," + Result[j].Investor.InvestorID;

                                                        StringResult.Add(message);
                                                    }
                                                }
                                                #endregion
                                                #endregion
                                            }
                                            else
                                            {
                                                #region SELECT NORMAL
                                                //SELECT NORMAL
                                                InvestorID = TradingServer.Facade.FacadeGetInvestorIDByCode(subParameter[0]);

                                                if (InvestorID != -1)
                                                {
                                                    #region GET HISTORY BY INVESTOR ID
                                                    Result = TradingServer.Facade.FacadeGetHistoryByInvestor(InvestorID, RowNumber, Start);

                                                    #region START GET ONLINE ORDER
                                                    if (Start == 0)
                                                    {
                                                        bool isExists = false;
                                                        if (Business.Market.TempListOrder != null)
                                                        {
                                                            int countTempListOrder = Business.Market.TempListOrder.Count;
                                                            for (int j = 0; j < countTempListOrder; j++)
                                                            {
                                                                if (Business.Market.TempListOrder[j].AdminCode == code)
                                                                {
                                                                    if (Business.Market.CommandExecutor != null)
                                                                    {
                                                                        for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                                        {
                                                                            if (Business.Market.CommandExecutor[n].Investor.InvestorID == InvestorID)
                                                                            {
                                                                                if (Business.Market.TempListOrder[j].ListOrder == null)
                                                                                    Business.Market.TempListOrder[j].ListOrder = new List<OpenTrade>();

                                                                                Business.Market.TempListOrder[j].ListOrder.Add(Business.Market.CommandExecutor[n]);
                                                                            }
                                                                        }
                                                                    }

                                                                    isExists = true;

                                                                    break;
                                                                }
                                                            }
                                                        }

                                                        if (!isExists)
                                                        {
                                                            List<Business.OpenTrade> tempOrder = new List<OpenTrade>();
                                                            if (Business.Market.CommandExecutor != null)
                                                            {
                                                                for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                                {
                                                                    if (Business.Market.CommandExecutor[n].Investor.InvestorID == InvestorID)
                                                                    {
                                                                        tempOrder.Add(Business.Market.CommandExecutor[n]);
                                                                    }
                                                                }
                                                            }

                                                            if (tempOrder != null && tempOrder.Count > 0)
                                                            {
                                                                Business.OrderInvestor newOrderInvestor = new OrderInvestor();
                                                                newOrderInvestor.AdminCode = code;
                                                                newOrderInvestor.ListOrder = tempOrder;

                                                                Business.Market.TempListOrder.Add(newOrderInvestor);
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    #region GET ORDER ONLINE
                                                    if (Result == null || Result.Count <= 0)
                                                    {
                                                        if (Business.Market.TempListOrder != null)
                                                        {
                                                            int countTempOpenTrade = Business.Market.TempListOrder.Count;
                                                            for (int m = 0; m < countTempOpenTrade; m++)
                                                            {
                                                                if (Business.Market.TempListOrder[m].AdminCode == code)
                                                                {
                                                                    if (End > Business.Market.TempListOrder[m].ListOrder.Count)
                                                                        End = Business.Market.TempListOrder[m].ListOrder.Count;

                                                                    if (Start > End)
                                                                        Start = 0;

                                                                    for (int n = Start; n < End; n++)
                                                                    {
                                                                        Result.Add(Business.Market.TempListOrder[m].ListOrder[n]);
                                                                        Business.Market.TempListOrder[m].ListOrder.RemoveAt(n);
                                                                        n--;

                                                                        if (End > Business.Market.TempListOrder[m].ListOrder.Count)
                                                                            End = Business.Market.TempListOrder[m].ListOrder.Count;

                                                                        if (Business.Market.TempListOrder[m].ListOrder.Count == 0 ||
                                                                            Business.Market.TempListOrder[m].ListOrder == null)
                                                                        {
                                                                            Business.Market.TempListOrder.RemoveAt(m);
                                                                        }
                                                                    }

                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    #region MAP COMMAND TO STRING
                                                    if (Result != null && Result.Count > 0)
                                                    {
                                                        int countResult = Result.Count;
                                                        for (int j = 0; j < countResult; j++)
                                                        {
                                                            string symbolName = "";
                                                            double contractSize = 0;
                                                            string currency = "";
                                                            if (Result[j].Symbol != null)
                                                            {
                                                                symbolName = Result[j].Symbol.Name;
                                                                contractSize = Result[j].Symbol.ContractSize;
                                                                currency = Result[j].Symbol.Currency;
                                                            }

                                                            string message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                               Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                               Result[j].Investor.Code + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                               Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                               Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + symbolName + "," + Result[j].TakeProfit + "," +
                                                               Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + contractSize + "," +
                                                               currency + "," + Result[j].Taxes + "," + Result[j].Comment + "," + Result[j].AgentCommission + "," + Result[j].Investor.InvestorID;

                                                            StringResult.Add(message);
                                                        }
                                                    }
                                                    #endregion

                                                    #endregion

                                                    #region GET HISTORY BY CODE(COMMENT)
                                                    //if (StringResult.Count == 0)
                                                    //{
                                                    //    if (Start == 0)
                                                    //    {
                                                    //        Business.OpenTrade newOpenTrade = new OpenTrade();
                                                    //        newOpenTrade = TradingServer.Facade.FacadeGetHistoryByCommandCode(subParameter[0]);

                                                    //        #region GET ORDER ONLINE
                                                    //        if (newOpenTrade == null || newOpenTrade.ID <= 0)
                                                    //        {
                                                    //            if (Business.Market.CommandExecutor != null)
                                                    //            {
                                                    //                for (int j = 0; j < Business.Market.CommandExecutor.Count; j++)
                                                    //                {
                                                    //                    if (Business.Market.CommandExecutor[j].CommandCode == subParameter[0])
                                                    //                    {
                                                    //                        newOpenTrade = Business.Market.CommandExecutor[j];
                                                    //                        break;
                                                    //                    }
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //        #endregion

                                                    //        #region MAP COMMAND TO STRING
                                                    //        if (newOpenTrade != null && newOpenTrade.ID > 0)
                                                    //        {
                                                    //            //string message = subValue[0] + "$" + Result[0].ClientCode + "," + Result[0].ClosePrice + "," + Result[0].CloseTime + "," +
                                                    //            //   Result[0].CommandCode + "," + Result[0].Commission + "," + Result[0].ExpTime + "," + Result[0].ID + "," +
                                                    //            //   Result[0].Investor.Code + "," + Result[0].IsClose + "," + Result[0].IsHedged + "," + Result[0].Margin + "," +
                                                    //            //   Result[0].MaxDev + "," + Result[0].OpenPrice + "," + Result[0].OpenTime + "," + Result[0].Profit + "," +
                                                    //            //   Result[0].Size + "," + Result[0].StopLoss + "," + Result[0].Swap + "," + Result[0].Symbol.Name + "," + Result[0].TakeProfit + "," +
                                                    //            //   Result[0].Taxes + "," + Result[0].Type.Name + "," + Result[0].Type.ID + "," + Result[0].Symbol.ContractSize + "," +
                                                    //            //   Result[0].Symbol.Currency + "," + Result[0].Taxes + "," + Result[0].Comment + "," + Result[0].AgentCommission;

                                                    //            string message = subValue[0] + "$" + newOpenTrade.ClientCode + "," + newOpenTrade.ClosePrice + "," + newOpenTrade.CloseTime + "," +
                                                    //               newOpenTrade.CommandCode + "," + newOpenTrade.Commission + "," + newOpenTrade.ExpTime + "," + newOpenTrade.ID + "," +
                                                    //               newOpenTrade.Investor.Code + "," + newOpenTrade.IsClose + "," + newOpenTrade.IsHedged + "," + newOpenTrade.Margin + "," +
                                                    //               newOpenTrade.MaxDev + "," + newOpenTrade.OpenPrice + "," + newOpenTrade.OpenTime + "," + newOpenTrade.Profit + "," +
                                                    //               newOpenTrade.Size + "," + newOpenTrade.StopLoss + "," + newOpenTrade.Swap + "," + symbolName + "," + newOpenTrade.TakeProfit + "," +
                                                    //               newOpenTrade.Taxes + "," + newOpenTrade.Type.Name + "," + newOpenTrade.Type.ID + "," + contractSize + "," +
                                                    //               currency + "," + newOpenTrade.Taxes + "," + newOpenTrade.Comment + "," + newOpenTrade.AgentCommission;

                                                    //            StringResult.Add(message);
                                                    //        }
                                                    //        #endregion
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        List<Business.OpenTrade> listOpenTrade = new List<OpenTrade>();
                                                    //        listOpenTrade = TradingServer.Facade.FacadeGetOnlineCommandByInvestorID(InvestorID);
                                                    //        if (listOpenTrade != null && listOpenTrade.Count > 0)
                                                    //        {
                                                    //            int countOpenTrade = listOpenTrade.Count;
                                                    //            if (End > countOpenTrade)
                                                    //                End = countOpenTrade;

                                                    //            if (Start > End)
                                                    //                Start = End;

                                                    //            for (int j = Start; j < End; j++)
                                                    //            {
                                                    //                string message = subValue[0] + "$" + listOpenTrade[j].ClientCode + "," + listOpenTrade[j].ClosePrice + "," +
                                                    //                    listOpenTrade[j].CloseTime + "," + listOpenTrade[j].CommandCode + "," + listOpenTrade[j].Commission + "," +
                                                    //                    listOpenTrade[j].ExpTime + "," + listOpenTrade[j].ID + "," + listOpenTrade[j].Investor.Code + "," +
                                                    //                    listOpenTrade[j].IsClose + "," + listOpenTrade[j].IsHedged + "," + listOpenTrade[j].Margin + "," +
                                                    //                    listOpenTrade[j].MaxDev + "," + listOpenTrade[j].OpenPrice + "," + listOpenTrade[j].OpenTime + "," +
                                                    //                    listOpenTrade[j].Profit + "," + listOpenTrade[j].Size + "," + listOpenTrade[j].StopLoss + "," +
                                                    //                    listOpenTrade[j].Swap + "," + listOpenTrade[j].Symbol.Name + "," + listOpenTrade[j].TakeProfit + "," +
                                                    //                    listOpenTrade[j].Taxes + "," + listOpenTrade[j].Type.Name + "," + listOpenTrade[j].Type.ID + "," +
                                                    //                    listOpenTrade[j].Symbol.ContractSize + "," + listOpenTrade[j].Symbol.Currency + "," +
                                                    //                    listOpenTrade[j].Taxes + "," + listOpenTrade[j].Comment + "," + listOpenTrade[j].AgentCommission;

                                                    //                StringResult.Add(message);
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    #endregion

                                                    if (StringResult.Count == 0)
                                                    {
                                                        string message = subValue[0] + "$end";
                                                        StringResult.Add(message);
                                                    }
                                                }
                                                #endregion
                                            }

                                            if (StringResult.Count == 0)
                                            {
                                                string message = subValue[0] + "$end";
                                                StringResult.Add(message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            case "GetOrderData":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 3)
                                        {
                                            List<Business.OpenTrade> Result = new List<OpenTrade>();
                                            int InvestorID = 0;
                                            int Start = 0;
                                            int End = 0;

                                            bool isWildcards = this.IsWildCards(subParameter[0]);

                                            if (isWildcards)
                                            {
                                                #region GET ORDER WITH WILDCARDS
                                                int.TryParse(subParameter[1], out Start);
                                                int.TryParse(subParameter[2], out End);

                                                int RowNumber = End - Start;

                                                //GET ALL ORDER WITH WILDCARDS
                                                Result = TradingServer.Facade.FacadeGetAllHistoryWithStartEnd(RowNumber, Start);

                                                #region GET HISTORY BY INVESTOR ID
                                                if (Result != null && Result.Count > 0)
                                                {
                                                    int countResult = Result.Count;
                                                    for (int j = 0; j < countResult; j++)
                                                    {
                                                        string symbolName = "";
                                                        double contractSize = 0;
                                                        string currency = "";
                                                        if (Result[j].Symbol != null)
                                                        {
                                                            symbolName = Result[j].Symbol.Name;
                                                            contractSize = Result[j].Symbol.ContractSize;
                                                            currency = Result[j].Symbol.Currency;
                                                        }

                                                        string message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                           Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                           Result[j].Investor.Code + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                           Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                           Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + symbolName + "," + Result[j].TakeProfit + "," +
                                                           Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + contractSize + "," +
                                                           currency + "," + Result[j].Taxes + "," + Result[j].Comment + "," + Result[j].AgentCommission + "," + Result[j].Investor.InvestorID;

                                                        StringResult.Add(message);
                                                    }
                                                }
                                                #endregion
                                                #endregion
                                            }
                                            else
                                            {
                                                //SELECT NORMAL
                                                InvestorID = TradingServer.Facade.FacadeGetInvestorIDByCode(subParameter[0]);

                                                if (InvestorID != -1)
                                                {
                                                    int.TryParse(subParameter[1], out Start);
                                                    int.TryParse(subParameter[2], out End);

                                                    int RowNumber = End - Start;

                                                    #region GET HISTORY BY INVESTOR ID
                                                    Result = TradingServer.Facade.FacadeGetHistoryByInvestor(InvestorID, RowNumber, Start);
                                                    if (Result != null && Result.Count > 0)
                                                    {
                                                        int countResult = Result.Count;
                                                        for (int j = 0; j < countResult; j++)
                                                        {
                                                            string symbolName = "";
                                                            double contractSize = 0;
                                                            string currency = "";
                                                            if (Result[j].Symbol != null)
                                                            {
                                                                symbolName = Result[j].Symbol.Name;
                                                                contractSize = Result[j].Symbol.ContractSize;
                                                                currency = Result[j].Symbol.Currency;
                                                            }

                                                            string message = subValue[0] + "$" + Result[j].ClientCode + "," + Result[j].ClosePrice + "," + Result[j].CloseTime + "," +
                                                               Result[j].CommandCode + "," + Result[j].Commission + "," + Result[j].ExpTime + "," + Result[j].ID + "," +
                                                               Result[j].Investor.Code + "," + Result[j].IsClose + "," + Result[j].IsHedged + "," + Result[j].Margin + "," +
                                                               Result[j].MaxDev + "," + Result[j].OpenPrice + "," + Result[j].OpenTime + "," + Result[j].Profit + "," +
                                                               Result[j].Size + "," + Result[j].StopLoss + "," + Result[j].Swap + "," + symbolName + "," + Result[j].TakeProfit + "," +
                                                               Result[j].Taxes + "," + Result[j].Type.Name + "," + Result[j].Type.ID + "," + contractSize + "," +
                                                               currency + "," + Result[j].Taxes + "," + Result[j].Comment + "," + Result[j].AgentCommission + "," + Result[j].Investor.InvestorID;

                                                            StringResult.Add(message);
                                                        }
                                                    }
                                                    #endregion

                                                    #region GET HISTORY BY CODE(COMMENT CODE)
                                                    //if (StringResult.Count == 0)
                                                    //{
                                                    //    if (Start == 0)
                                                    //    {
                                                    //        Business.OpenTrade newOpenTrade = new OpenTrade();
                                                    //        newOpenTrade = TradingServer.Facade.FacadeGetHistoryByCommandCode(subParameter[0]);
                                                    //        if (newOpenTrade != null && newOpenTrade.ID > 0)
                                                    //        {
                                                    //            string message = subValue[0] + "$" + Result[0].ClientCode + "," + Result[0].ClosePrice + "," + Result[0].CloseTime + "," +
                                                    //               Result[0].CommandCode + "," + Result[0].Commission + "," + Result[0].ExpTime + "," + Result[0].ID + "," +
                                                    //               Result[0].Investor.Code + "," + Result[0].IsClose + "," + Result[0].IsHedged + "," + Result[0].Margin + "," +
                                                    //               Result[0].MaxDev + "," + Result[0].OpenPrice + "," + Result[0].OpenTime + "," + Result[0].Profit + "," +
                                                    //               Result[0].Size + "," + Result[0].StopLoss + "," + Result[0].Swap + "," + Result[0].Symbol.Name + "," + Result[0].TakeProfit + "," +
                                                    //               Result[0].Taxes + "," + Result[0].Type.Name + "," + Result[0].Type.ID + "," + Result[0].Symbol.ContractSize + "," +
                                                    //               Result[0].Symbol.Currency + "," + Result[0].Taxes + "," + Result[0].Comment + "," + Result[0].AgentCommission;

                                                    //            StringResult.Add(message);
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        List<Business.OpenTrade> listOpenTrade = new List<OpenTrade>();
                                                    //        listOpenTrade = TradingServer.Facade.FacadeGetOnlineCommandByInvestorID(InvestorID);
                                                    //        if (listOpenTrade != null && listOpenTrade.Count > 0)
                                                    //        {
                                                    //            int countOpenTrade = listOpenTrade.Count;
                                                    //            if (End > countOpenTrade)
                                                    //                End = countOpenTrade;

                                                    //            for (int j = Start; j < End; j++)
                                                    //            {
                                                    //                string message = subValue[0] + "$" + listOpenTrade[j].ClientCode + "," + listOpenTrade[j].ClosePrice + "," +
                                                    //                    listOpenTrade[j].CloseTime + "," + listOpenTrade[j].CommandCode + "," + listOpenTrade[j].Commission + "," +
                                                    //                    listOpenTrade[j].ExpTime + "," + listOpenTrade[j].ID + "," + listOpenTrade[j].Investor.Code + "," +
                                                    //                    listOpenTrade[j].IsClose + "," + listOpenTrade[j].IsHedged + "," + listOpenTrade[j].Margin + "," +
                                                    //                    listOpenTrade[j].MaxDev + "," + listOpenTrade[j].OpenPrice + "," + listOpenTrade[j].OpenTime + "," +
                                                    //                    listOpenTrade[j].Profit + "," + listOpenTrade[j].Size + "," + listOpenTrade[j].StopLoss + "," +
                                                    //                    listOpenTrade[j].Swap + "," + listOpenTrade[j].Symbol.Name + "," + listOpenTrade[j].TakeProfit + "," +
                                                    //                    listOpenTrade[j].Taxes + "," + listOpenTrade[j].Type.Name + "," + listOpenTrade[j].Type.ID + "," +
                                                    //                    listOpenTrade[j].Symbol.ContractSize + "," + listOpenTrade[j].Symbol.Currency + "," +
                                                    //                    listOpenTrade[j].Taxes + "," + listOpenTrade[j].Comment + "," + listOpenTrade[j].AgentCommission;

                                                    //                StringResult.Add(message);
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    #endregion

                                                    if (StringResult.Count == 0)
                                                    {
                                                        string message = subValue[0] + "$end";
                                                        StringResult.Add(message);
                                                    }
                                                }
                                            }


                                            if (StringResult.Count == 0)
                                            {
                                                string message = subValue[0] + "$end";
                                                StringResult.Add(message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string message = subValue[0] + "$MCM005";
                                        StringResult.Add(message);
                                    }
                                }
                                break;

                            case "GetOrderOpenOnly":
                                {
                                    bool checkip = Facade.FacadeCheckIpAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 3)
                                        {
                                            int start = 0;
                                            int end = 0;

                                            int.TryParse(subParameter[1], out start);
                                            int.TryParse(subParameter[2], out end);

                                            List<Business.OpenTrade> tempResult = new List<OpenTrade>();

                                            bool isWildCards = this.IsWildCards(subParameter[0]);

                                            if (isWildCards)
                                            {
                                                #region SEARCH WITH WILDCARDS
                                                if (Business.Market.CommandExecutor != null)
                                                {
                                                    int countCommand = Business.Market.CommandExecutor.Count;
                                                    for (int j = 0; j < countCommand; j++)
                                                    {
                                                        tempResult.Add(Business.Market.CommandExecutor[j]);
                                                    }
                                                }

                                                if (tempResult.Count > 0 && tempResult != null)
                                                {
                                                    int countTempResult = tempResult.Count;
                                                    if (countTempResult < end)
                                                        end = countTempResult;
                                                    for (int j = start; j < end; j++)
                                                    {
                                                        string message = subValue[0] + "$" + tempResult[j].ClientCode + "," + tempResult[j].ClosePrice + "," + tempResult[j].CloseTime + "," +
                                                               tempResult[j].CommandCode + "," + tempResult[j].Commission + "," + tempResult[j].ExpTime + "," + tempResult[j].ID + "," +
                                                               tempResult[j].Investor.Code + "," + tempResult[j].IsClose + "," + tempResult[j].IsHedged + "," + tempResult[j].Margin + "," +
                                                               tempResult[j].MaxDev + "," + tempResult[j].OpenPrice + "," + tempResult[j].OpenTime + "," + tempResult[j].Profit + "," +
                                                               tempResult[j].Size + "," + tempResult[j].StopLoss + "," + tempResult[j].Swap + "," + tempResult[j].Symbol.Name + "," + tempResult[j].TakeProfit + "," +
                                                               tempResult[j].Taxes + "," + tempResult[j].Type.Name + "," + tempResult[j].Type.ID + "," + tempResult[j].Symbol.ContractSize + "," +
                                                               tempResult[j].Symbol.Currency + "," + tempResult[j].Taxes + "," + tempResult[j].Comment + "," + tempResult[j].AgentCommission + "," +
                                                               tempResult[j].Investor.InvestorID;

                                                        StringResult.Add(message);
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region SEARCH IN COMMAND EXECUTOR WITH SEARCH VALUE IS INVESTOR ID
                                                if (Business.Market.CommandExecutor != null)
                                                {
                                                    int countCommand = Business.Market.CommandExecutor.Count;
                                                    for (int j = 0; j < countCommand; j++)
                                                    {
                                                        if (Business.Market.CommandExecutor[j].Investor.Code.ToUpper() == subParameter[0].ToUpper())
                                                        {
                                                            tempResult.Add(Business.Market.CommandExecutor[j]);
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region IF TEMP COUNT RESULT <=0 THEN FIND WITH SEARCH VALUE IS COMMAND CODE
                                                if (tempResult.Count <= 0)
                                                {
                                                    if (Business.Market.CommandExecutor != null)
                                                    {
                                                        int countCommand = Business.Market.CommandExecutor.Count;
                                                        for (int j = 0; j < countCommand; j++)
                                                        {
                                                            if (Business.Market.CommandExecutor[j].CommandCode == subParameter[0])
                                                            {
                                                                tempResult.Add(Business.Market.CommandExecutor[j]);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion

                                                if (tempResult.Count > 0 && tempResult != null)
                                                {
                                                    int countTempResult = tempResult.Count;
                                                    if (countTempResult < end)
                                                        end = countTempResult;
                                                    for (int j = start; j < end; j++)
                                                    {
                                                        string message = subValue[0] + "$" + tempResult[j].ClientCode + "," + tempResult[j].ClosePrice + "," + tempResult[j].CloseTime + "," +
                                                               tempResult[j].CommandCode + "," + tempResult[j].Commission + "," + tempResult[j].ExpTime + "," + tempResult[j].ID + "," +
                                                               tempResult[j].Investor.Code + "," + tempResult[j].IsClose + "," + tempResult[j].IsHedged + "," + tempResult[j].Margin + "," +
                                                               tempResult[j].MaxDev + "," + tempResult[j].OpenPrice + "," + tempResult[j].OpenTime + "," + tempResult[j].Profit + "," +
                                                               tempResult[j].Size + "," + tempResult[j].StopLoss + "," + tempResult[j].Swap + "," + tempResult[j].Symbol.Name + "," + tempResult[j].TakeProfit + "," +
                                                               tempResult[j].Taxes + "," + tempResult[j].Type.Name + "," + tempResult[j].Type.ID + "," + tempResult[j].Symbol.ContractSize + "," +
                                                               tempResult[j].Symbol.Currency + "," + tempResult[j].Taxes + "," + tempResult[j].Comment + "," + tempResult[j].AgentCommission + "," +
                                                               tempResult[j].Investor.InvestorID;

                                                        StringResult.Add(message);
                                                    }
                                                }
                                            }

                                            if (StringResult.Count == 0)
                                            {
                                                string message = subValue[0] + "$end";
                                                StringResult.Add(message);
                                            }

                                            #region INSERT SYSTEM LOG
                                            //INSERT SYSTEM LOG
                                            string content = "'" + code + "': " + StringResult.Count + " orders have been request";
                                            string comment = "[request order open only]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        string Message = subValue[0] + "$MCM005";
                                        StringResult.Add(Message);
                                    }
                                }
                                break;

                            //GET ORDER IN DATABASE WITH TABLE COMMAND HISTORY 
                            case "GetOrderDataWithTime":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 6)
                                        {
                                            int from = 0;
                                            int to = 0;
                                            int investorID = 0;
                                            int ManagerID = 0;
                                            DateTime timeStart;
                                            DateTime timeEnd;

                                            int.TryParse(subParameter[0], out from);
                                            int.TryParse(subParameter[1], out to);
                                            int.TryParse(subParameter[2], out investorID);
                                            int.TryParse(subParameter[3], out ManagerID);
                                            DateTime.TryParse(subParameter[4], out timeStart);
                                            DateTime.TryParse(subParameter[5], out timeEnd);

                                            List<Business.OpenTrade> Result = new List<OpenTrade>();

                                            #region FIRST GET DATA HISTORY OF INVESTOR
                                            if (Business.Market.AgentList != null)
                                            {
                                                int countAgent = Business.Market.AgentList.Count;
                                                bool isExists = false;
                                                for (int j = 0; j < countAgent; j++)
                                                {   
                                                    if (Business.Market.AgentList[j].AgentID == ManagerID)
                                                    {
                                                        if (from == 0)
                                                        {
                                                            #region REMOVE LIST HISTORY OF AGENT
                                                            if (Business.Market.AgentList[j].ListHistoryInvestor != null &&
                                                                Business.Market.AgentList[j].ListHistoryInvestor.Count > 0)
                                                            {
                                                                int countHistory = Business.Market.AgentList[j].ListHistoryInvestor.Count;
                                                                for (int n = 0; n < countHistory; n++)
                                                                {
                                                                    Business.Market.AgentList[j].ListHistoryInvestor.RemoveAt(n);
                                                                }
                                                            }
                                                            #endregion

                                                            #region GET HISTORY IN DATABASE AND ADD TO LIST HISTORY OF AGENT
                                                            Result = TradingServer.Facade.FacadeGetCommandHistoryWithStarLimit(investorID, ManagerID, timeStart, timeEnd);

                                                            if (Result != null && Result.Count > 0)
                                                            {
                                                                int countResult = Result.Count;

                                                                if (Business.Market.AgentList[j].ListHistoryInvestor == null)
                                                                    Business.Market.AgentList[j].ListHistoryInvestor = new List<OpenTrade>();

                                                                for (int m = 0; m < countResult; m++)
                                                                {
                                                                    Business.Market.AgentList[j].ListHistoryInvestor.Add(Result[m]);
                                                                }
                                                            }
                                                            #endregion
                                                        }

                                                        int rowNumber = to - from;
                                                        if (Business.Market.AgentList[j].ListHistoryInvestor == null)
                                                            Business.Market.AgentList[j].ListHistoryInvestor = new List<OpenTrade>();

                                                        if (Business.Market.AgentList[j].ListHistoryInvestor.Count < rowNumber)
                                                            rowNumber = Business.Market.AgentList[j].ListHistoryInvestor.Count;

                                                        for (int n = 0; n < rowNumber; n++)
                                                        {
                                                            string symbolName = string.Empty;
                                                            string currency = string.Empty;
                                                            string typeName = string.Empty;

                                                            if (Business.Market.AgentList[j].ListHistoryInvestor[0].Symbol != null)
                                                            {
                                                                symbolName = Business.Market.AgentList[j].ListHistoryInvestor[0].Symbol.Name;
                                                                currency = Business.Market.AgentList[j].ListHistoryInvestor[0].Symbol.Currency;
                                                            }

                                                            if (Business.Market.AgentList[j].ListHistoryInvestor[0].Type.ID == 13 ||
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Type.ID == 14 ||
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Type.ID == 15 ||
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Type.ID == 16)
                                                            {
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].CloseTime = DateTime.MinValue;
                                                            }

                                                            #region MAP STRING
                                                            string message = subValue[0] + "$" + Business.Market.AgentList[j].ListHistoryInvestor[0].ClientCode + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].ClosePrice + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].CloseTime + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].CommandCode + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Commission + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].ExpTime + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].ID + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Investor.InvestorID + "," +
                                                                true + "," + false + "," + "0,0" + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].OpenPrice + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].OpenTime + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Profit + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Size + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].StopLoss + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Swap + "," + symbolName + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].TakeProfit + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Taxes + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Type.Name + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Type.ID + ",-1,-1" + "," + currency + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Comment + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].AgentCommission + "," +
                                                                Business.Market.AgentList[j].ListHistoryInvestor[0].Investor.InvestorID;


                                                            StringResult.Add(message);
                                                            #endregion

                                                            Business.Market.AgentList[j].ListHistoryInvestor.RemoveAt(0);
                                                        }

                                                        isExists = true;
                                                        break;
                                                    }
                                                }

                                                if (!isExists)
                                                {
                                                    int countAdminList = Business.Market.AdminList.Count;
                                                    for (int j = 0; j < countAdminList; j++)
                                                    {
                                                        if (Business.Market.AdminList[j].AgentID == ManagerID)
                                                        {
                                                            if (from == 0)
                                                            {
                                                                #region REMOVE LIST HISTORY OF AGENT
                                                                if (Business.Market.AdminList[j].ListHistoryInvestor != null &&
                                                                    Business.Market.AdminList[j].ListHistoryInvestor.Count > 0)
                                                                    Business.Market.AdminList[j].ListHistoryInvestor.Clear();
                                                                
                                                                #endregion

                                                                #region GET HISTORY IN DATABASE AND ADD TO LIST HISTORY OF AGENT
                                                                Result = TradingServer.Facade.FacadeGetCommandHistoryWithStarLimit(investorID, ManagerID, timeStart, timeEnd);

                                                                if (Result != null && Result.Count > 0)
                                                                {
                                                                    int countResult = Result.Count;

                                                                    if (Business.Market.AdminList[j].ListHistoryInvestor == null)
                                                                        Business.Market.AdminList[j].ListHistoryInvestor = new List<OpenTrade>();

                                                                    for (int m = 0; m < countResult; m++)
                                                                    {
                                                                        Business.Market.AdminList[j].ListHistoryInvestor.Add(Result[m]);
                                                                    }
                                                                }
                                                                #endregion
                                                            }

                                                            int rowNumber = to - from;
                                                            if (Business.Market.AdminList[j].ListHistoryInvestor == null)
                                                                Business.Market.AdminList[j].ListHistoryInvestor = new List<OpenTrade>();

                                                            if (Business.Market.AdminList[j].ListHistoryInvestor.Count < rowNumber)
                                                                rowNumber = Business.Market.AdminList[j].ListHistoryInvestor.Count;

                                                            for (int n = 0; n < rowNumber; n++)
                                                            {
                                                                string symbolName = string.Empty;
                                                                string currency = string.Empty;
                                                                string typeName = string.Empty;

                                                                if (Business.Market.AdminList[j].ListHistoryInvestor[0].Symbol != null)
                                                                {
                                                                    symbolName = Business.Market.AdminList[j].ListHistoryInvestor[0].Symbol.Name;
                                                                    currency = Business.Market.AdminList[j].ListHistoryInvestor[0].Symbol.Currency;
                                                                }

                                                                if (Business.Market.AdminList[j].ListHistoryInvestor[0].Type.ID == 13 ||
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Type.ID == 14 ||
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Type.ID == 15 ||
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Type.ID == 16)
                                                                {
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].CloseTime = DateTime.MinValue;
                                                                }

                                                                #region MAP STRING
                                                                string message = subValue[0] + "$" + Business.Market.AdminList[j].ListHistoryInvestor[0].ClientCode + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].ClosePrice + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].CloseTime + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].CommandCode + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Commission + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].ExpTime + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].ID + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Investor.InvestorID + "," +
                                                                    true + "," + false + "," + "0,0" + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].OpenPrice + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].OpenTime + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Profit + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Size + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].StopLoss + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Swap + "," + symbolName + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].TakeProfit + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Taxes + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Type.Name + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Type.ID + ",-1,-1" + "," + currency + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Comment + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].AgentCommission + "," +
                                                                    Business.Market.AdminList[j].ListHistoryInvestor[0].Investor.InvestorID;

                                                                StringResult.Add(message);
                                                                #endregion

                                                                Business.Market.AdminList[j].ListHistoryInvestor.RemoveAt(0);
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        if (StringResult.Count == 0)
                                        {
                                            string message = subValue[0] + "$";
                                            StringResult.Add(message);
                                        }

                                        #region INSERT SYSTEM LOG
                                        //INSERT SYSTEM LOG
                                        string content = "'" + code + "': " + StringResult.Count + " orders have been request";
                                        string comment = "[request order]";
                                        TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                        #endregion
                                    }
                                    else
                                    {
                                        string message = subValue[0] + "$MCM005";
                                        StringResult.Add(message);
                                    }
                                }
                                break;

                            case "GetDepositInvestor":
                                {
                                    string[] subParameter = subValue[1].Split('{');
                                    List<int> listInvestor = new List<int>();
                                    int countInvestor = subParameter.Length;
                                    for (int j = 0; j < countInvestor; j++)
                                    {
                                        listInvestor.Add(int.Parse(subParameter[j]));
                                    }

                                    Dictionary<int, double> result = Facade.FacadeGetDepositByListInvestorID(listInvestor);

                                    if (result != null)
                                    {
                                        foreach (KeyValuePair<int, double> d in result)
                                        {
                                            string temp = subValue[0] + "$" + d.Key + "{" + d.Value;
                                            StringResult.Add(temp);
                                        }
                                    }
                                }
                                break;

                            case "GetTopOpenClosePosition":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split('|');
                                        if (subParameter.Length == 2)
                                        {
                                            List<Business.OpenTrade> listOpenPosition = new List<OpenTrade>();
                                            List<Business.OpenTrade> listClosePosition = new List<OpenTrade>();
                                            List<Business.OpenTrade> listAllPosition = new List<OpenTrade>();
                                            List<int> listInvestorID = new List<int>();
                                            int numberGet = int.Parse(subParameter[0]);
                                            string[] subListGroup = subParameter[1].Split(',');

                                            #region GET OPEN POSITION INVESTOR AND GET INVESTOR ID
                                            if (subListGroup != null && subListGroup.Length > 0)
                                            {
                                                int countSubGroupt = subListGroup.Length;
                                                for (int j = 0; j < countSubGroupt; j++)
                                                {
                                                    int grouptID = int.Parse(subListGroup[j]);

                                                    if (Business.Market.InvestorList != null)
                                                    {
                                                        int countGroupt = Business.Market.InvestorList.Count;
                                                        for (int k = 0; k < countGroupt; k++)
                                                        {
                                                            if (Business.Market.InvestorList[k].InvestorGroupInstance.InvestorGroupID == grouptID)
                                                            {
                                                                if (Business.Market.InvestorList[k].CommandList != null)
                                                                {
                                                                    int countCommand = Business.Market.InvestorList[k].CommandList.Count;
                                                                    for (int n = 0; n < countCommand; n++)
                                                                    {
                                                                        bool isPending = Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.InvestorList[k].CommandList[n].Type.ID);
                                                                        if (!isPending)
                                                                            listOpenPosition.Add(Business.Market.InvestorList[k].CommandList[n]);
                                                                    }
                                                                }

                                                                listInvestorID.Add(Business.Market.InvestorList[k].InvestorID);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                StringResult.Add(subValue[0] + "$IVC0002");
                                            }
                                            #endregion

                                            #region GET CLOSE POSITION AND MAP COMMAND TO CLIENT
                                            listClosePosition = TradingServer.Facade.FacadeGetTopClosePosition(numberGet, listInvestorID);

                                            if (listClosePosition != null)
                                                listAllPosition = listClosePosition;
                                            
                                            if (listOpenPosition != null && listOpenPosition.Count > 0)
                                            {
                                                listOpenPosition = listOpenPosition.OrderByDescending(o => o.OpenTime).ToList();

                                                int countGet = 0;
                                                if (numberGet > listOpenPosition.Count)
                                                    countGet = listOpenPosition.Count;
                                                else
                                                    countGet = numberGet;

                                                for (int j = 0; j < countGet; j++)
                                                {
                                                    listOpenPosition[j].IsClose = false;

                                                    listAllPosition.Add(listOpenPosition[j]);   
                                                }
                                            }
                                            #endregion

                                            #region SORT POSITION
                                            if (listAllPosition != null)
                                            {
                                                int countPosition = listAllPosition.Count;
                                                for (int j = 0; j < countPosition; j++)
                                                {
                                                    for (int n = j + 1; n < countPosition - 1; n++)
                                                    {
                                                        if (listAllPosition[j].IsClose)
                                                        {
                                                            if (listAllPosition[n].IsClose)
                                                            {
                                                                TimeSpan span = listAllPosition[j].CloseTime - listAllPosition[n].CloseTime;
                                                                if (span.TotalSeconds > 0)
                                                                {
                                                                    Business.OpenTrade temp = new OpenTrade();
                                                                    temp = listAllPosition[n];
                                                                    listAllPosition[n] = listAllPosition[j];
                                                                    listAllPosition[j] = temp;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                TimeSpan span = listAllPosition[j].CloseTime - listAllPosition[n].OpenTime;
                                                                if (span.TotalSeconds > 0)
                                                                {
                                                                    Business.OpenTrade temp = new OpenTrade();
                                                                    temp = listAllPosition[n];
                                                                    listAllPosition[n] = listAllPosition[j];
                                                                    listAllPosition[j] = temp;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (listAllPosition[n].IsClose)
                                                            {
                                                                TimeSpan span = listAllPosition[j].OpenTime - listAllPosition[n].CloseTime;
                                                                if (span.TotalSeconds > 0)
                                                                {
                                                                    Business.OpenTrade temp = new OpenTrade();
                                                                    temp = listAllPosition[n];
                                                                    listAllPosition[n] = listAllPosition[j];
                                                                    listAllPosition[j] = temp;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                TimeSpan span = listAllPosition[j].OpenTime - listAllPosition[n].OpenTime;
                                                                if (span.TotalSeconds > 0)
                                                                {
                                                                    Business.OpenTrade temp = new OpenTrade();
                                                                    temp = listAllPosition[n];
                                                                    listAllPosition[n] = listAllPosition[j];
                                                                    listAllPosition[j] = temp;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            if (listAllPosition != null && listAllPosition.Count > 0)
                                            {
                                                int countGet = int.Parse(subParameter[0]);
                                                if (countGet > listAllPosition.Count)
                                                    countGet = listAllPosition.Count;

                                                for (int j = 0; j < countGet; j++)
                                                {
                                                    string message = subValue[0] + "$" + listAllPosition[j].ClientCode + "," +
                                                            listAllPosition[j].ClosePrice + "," + listAllPosition[j].CloseTime + "," +
                                                            listAllPosition[j].CommandCode + "," + listAllPosition[j].Commission + "," +
                                                            listAllPosition[j].ExpTime + "," + listAllPosition[j].ID + "," +
                                                            listAllPosition[j].Investor.InvestorID + "," + listAllPosition[j].IsClose + "," +
                                                            listAllPosition[j].IsHedged + "," + listAllPosition[j].Margin + "," +
                                                            listAllPosition[j].MaxDev + "," + listAllPosition[j].OpenPrice + "," +
                                                            listAllPosition[j].OpenTime + "," + listAllPosition[j].Profit + "," +
                                                            listAllPosition[j].Size + "," + listAllPosition[j].StopLoss + "," +
                                                            listAllPosition[j].Swap + "," + listAllPosition[j].Symbol.Name + "," +
                                                            listAllPosition[j].TakeProfit + "," + listAllPosition[j].Taxes + "," +
                                                            listAllPosition[j].Type.Name + "," + listAllPosition[j].Type.ID + "," +
                                                            listAllPosition[j].Symbol.ContractSize + "," + listAllPosition[j].SpreaDifferenceInOpenTrade + "," +
                                                            listAllPosition[j].Symbol.Currency + "," + listAllPosition[j].Comment + "," +
                                                            listAllPosition[j].AgentCommission + "," + listAllPosition[j].IsActivePending + "," +
                                                            listAllPosition[j].IsStopLossAndTakeProfit;

                                                    StringResult.Add(message);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            StringResult.Add(subValue[0] + "$IVC0001");
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION REPORT
                            case "GetCommandHistoryWithGroupList":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length > 0)
                                        {
                                            int start = 0;
                                            int end = 0;
                                            int managerID = 0;
                                            DateTime startTime;
                                            DateTime endTime;
                                            List<int> InvestorList = new List<int>();
                                            List<Business.OpenTrade> Result = new List<OpenTrade>();

                                            int.TryParse(subParameter[0], out start);
                                            int.TryParse(subParameter[1], out end);
                                            int.TryParse(subParameter[2], out managerID);
                                            DateTime.TryParse(subParameter[3], out startTime);
                                            DateTime.TryParse(subParameter[4], out endTime);

                                            //set new time start,end
                                            startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 00, 00, 00);
                                            endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);

                                            for (int j = 5; j < subParameter.Length; j++)
                                            {
                                                int investorID = 0;
                                                int.TryParse(subParameter[j], out investorID);
                                                InvestorList.Add(investorID);
                                            }

                                            if (Business.Market.AgentList != null)
                                            {
                                                int countAgent = Business.Market.AgentList.Count;
                                                for (int j = 0; j < countAgent; j++)
                                                {
                                                    if (Business.Market.AgentList[j].AgentID == managerID)
                                                    {
                                                        if (start == 0)
                                                        {
                                                            #region REMOVE ALL DATA HISTORY REPORT OF INVESTOR
                                                            Business.Market.AgentList[j].ListHistoryReport = new List<OpenTrade>();
                                                            #endregion

                                                            #region GET REPORT IN DATABASE AND ADD TO LIST HISTORY REPORT
                                                            DateTime timeLastAccount = startTime.AddDays(-1);
                                                            Result = TradingServer.Facade.FacadegetCommandHistoryWithInvestorList(InvestorList, managerID, startTime, endTime);

                                                            if (Result != null)
                                                            {
                                                                int countResult = Result.Count;

                                                                if (Business.Market.AgentList[j].ListHistoryReport == null)
                                                                    Business.Market.AgentList[j].ListHistoryReport = new List<OpenTrade>();

                                                                for (int n = 0; n < countResult; n++)
                                                                {
                                                                    Business.Market.AgentList[j].ListHistoryReport.Add(Result[n]);
                                                                }
                                                            }
                                                            #endregion

                                                            #region GET LAST ACCOUNT
                                                            DateTime timeEndLastAccount = new DateTime(timeLastAccount.Year, timeLastAccount.Month, timeLastAccount.Day, 23, 59, 59);
                                                            List<Business.LastBalance> listLastAccount = TradingServer.Facade.FacadeGetLastBalanceByTimeListInvestor(InvestorList, timeLastAccount, timeEndLastAccount);
                                                            if (listLastAccount != null)
                                                            {
                                                                int countLastAccount = listLastAccount.Count;
                                                                for (int n = 0; n < countLastAccount; n++)
                                                                {
                                                                    Business.OpenTrade newOpenTrade = new OpenTrade();
                                                                    newOpenTrade.AgentCommission = listLastAccount[n].Balance;
                                                                    newOpenTrade.AskServer = listLastAccount[n].ClosePL;
                                                                    newOpenTrade.BidServer = listLastAccount[n].Credit;
                                                                    newOpenTrade.ClosePrice = listLastAccount[n].CreditAccount;
                                                                    newOpenTrade.Commission = listLastAccount[n].CreditOut;
                                                                    newOpenTrade.FreezeMargin = listLastAccount[n].Deposit;
                                                                    newOpenTrade.Margin = listLastAccount[n].FloatingPL;
                                                                    newOpenTrade.MaxDev = listLastAccount[n].FreeMargin;
                                                                    newOpenTrade.NumberUpdate = listLastAccount[n].InvestorID;
                                                                    newOpenTrade.OpenPrice = listLastAccount[n].LastEquity;
                                                                    newOpenTrade.OpenTime = listLastAccount[n].LogDate;
                                                                    newOpenTrade.Profit = listLastAccount[n].LastMargin;
                                                                    newOpenTrade.Size = listLastAccount[n].PLBalance;
                                                                    newOpenTrade.SpreaDifferenceInOpenTrade = listLastAccount[n].Withdrawal;
                                                                    newOpenTrade.Comment = listLastAccount[n].LoginCode;
                                                                    newOpenTrade.Type = new TradeType();
                                                                    newOpenTrade.Type.ID = 21;
                                                                    newOpenTrade.Type.Name = "LastBalance";
                                                                    newOpenTrade.Investor = new Investor();

                                                                    Business.Market.AgentList[j].ListHistoryReport.Add(newOpenTrade);
                                                                }
                                                            }
                                                            #endregion

                                                            #region GET END LAST ACCOUNT
                                                            DateTime tempStartTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 00, 00, 00);
                                                            DateTime tempEndTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
                                                            List<Business.LastBalance> listEndLastAccount = TradingServer.Facade.FacadeGetLastBalanceByTimeListInvestor(InvestorList, tempStartTime, tempEndTime);
                                                            if (listEndLastAccount != null)
                                                            {
                                                                int countLastAccount = listEndLastAccount.Count;
                                                                for (int n = 0; n < countLastAccount; n++)
                                                                {
                                                                    Business.OpenTrade newOpenTrade = new OpenTrade();
                                                                    newOpenTrade.AgentCommission = listEndLastAccount[n].Balance;
                                                                    newOpenTrade.AskServer = listEndLastAccount[n].ClosePL;
                                                                    newOpenTrade.BidServer = listEndLastAccount[n].Credit;
                                                                    newOpenTrade.ClosePrice = listEndLastAccount[n].CreditAccount;
                                                                    newOpenTrade.Commission = listEndLastAccount[n].CreditOut;
                                                                    newOpenTrade.FreezeMargin = listEndLastAccount[n].Deposit;
                                                                    newOpenTrade.Margin = listEndLastAccount[n].FloatingPL;
                                                                    newOpenTrade.MaxDev = listEndLastAccount[n].FreeMargin;
                                                                    newOpenTrade.NumberUpdate = listEndLastAccount[n].InvestorID;
                                                                    newOpenTrade.OpenPrice = listEndLastAccount[n].LastEquity;
                                                                    newOpenTrade.OpenTime = listEndLastAccount[n].LogDate;
                                                                    newOpenTrade.Profit = listEndLastAccount[n].LastMargin;
                                                                    newOpenTrade.Size = listEndLastAccount[n].PLBalance;
                                                                    newOpenTrade.SpreaDifferenceInOpenTrade = listEndLastAccount[n].Withdrawal;
                                                                    newOpenTrade.Comment = listEndLastAccount[n].LoginCode;
                                                                    newOpenTrade.Type = new TradeType();
                                                                    newOpenTrade.Type.ID = 22;
                                                                    newOpenTrade.Type.Name = "EndLastBalance";
                                                                    newOpenTrade.Investor = new Investor();

                                                                    Business.Market.AgentList[j].ListHistoryReport.Add(newOpenTrade);
                                                                }
                                                            }
                                                            #endregion
                                                        }

                                                        if (Business.Market.AgentList[j].ListHistoryReport != null)
                                                        {
                                                            int rowNumber = end - start;
                                                            if (Business.Market.AgentList[j].ListHistoryReport.Count < rowNumber)
                                                                rowNumber = Business.Market.AgentList[j].ListHistoryReport.Count;

                                                            #region FOR GET ORDER WITH START ,END
                                                            for (int n = 0; n < rowNumber; n++)
                                                            {
                                                                string symbolName = string.Empty;
                                                                if (Business.Market.AgentList[j].ListHistoryReport[0].Type == null ||
                                                                    Business.Market.AgentList[j].ListHistoryReport[0].Investor == null)
                                                                    continue;

                                                                if (Business.Market.AgentList[j].ListHistoryReport[0].Type.ID == 21)
                                                                {
                                                                    //string messageLastBalance = "LastBalance$" + Business.Market.AgentList[j].ListHistoryReport[0].Investor.InvestorID + "," +
                                                                    //    Business.Market.AgentList[j].ListHistoryReport[0].Profit;

                                                                    string messageLastBalance = "LastBalance$" + Business.Market.AgentList[j].ListHistoryReport[0].NumberUpdate + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].AgentCommission + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].AskServer + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].FreezeMargin + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Margin + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].BidServer + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].OpenPrice + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Profit + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].MaxDev + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].OpenTime + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Commission + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].SpreaDifferenceInOpenTrade + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].ClosePrice + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Comment;

                                                                    StringResult.Add(messageLastBalance);

                                                                    Business.Market.AgentList[j].ListHistoryReport.RemoveAt(0);

                                                                    //n--;

                                                                    //if (Business.Market.AgentList[j].ListHistoryReport.Count < rowNumber)
                                                                    //    rowNumber = Business.Market.AgentList[j].ListHistoryReport.Count;
                                                                }
                                                                else if (Business.Market.AgentList[j].ListHistoryReport[0].Type.ID == 22)
                                                                {
                                                                    string messageLastBalance = "EndLastBalance$" + Business.Market.AgentList[j].ListHistoryReport[0].NumberUpdate + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].AgentCommission + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].AskServer + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].FreezeMargin + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].Margin + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].BidServer + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].OpenPrice + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].Profit + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].MaxDev + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].OpenTime + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].Commission + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].SpreaDifferenceInOpenTrade + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].ClosePrice + "," +
                                                                       Business.Market.AgentList[j].ListHistoryReport[0].Comment;

                                                                    StringResult.Add(messageLastBalance);

                                                                    Business.Market.AgentList[j].ListHistoryReport.RemoveAt(0);
                                                                }
                                                                else
                                                                {
                                                                    if (Business.Market.AgentList[j].ListHistoryReport[0].Symbol != null)
                                                                        symbolName = Business.Market.AgentList[j].ListHistoryReport[0].Symbol.Name;

                                                                    string message = subValue[0] + "$" + Business.Market.AgentList[j].ListHistoryReport[0].ClientCode + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].ClosePrice + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].CloseTime + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].CommandCode + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Commission + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].ExpTime + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].ID + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Investor.InvestorID + "," +
                                                                        true + "," + false + "," + "0,0" + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].OpenPrice + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].OpenTime + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Profit + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Size + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].StopLoss + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Swap + "," + symbolName + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].TakeProfit + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Taxes + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Type.Name + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Type.ID + ",-1,-1" + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].AgentCommission + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Investor.AgentID + "," +
                                                                        Business.Market.AgentList[j].ListHistoryReport[0].Comment;

                                                                    StringResult.Add(message);

                                                                    Business.Market.AgentList[j].ListHistoryReport.RemoveAt(0);

                                                                    //n--;

                                                                    //if (Business.Market.AgentList[j].ListHistoryReport.Count < rowNumber)
                                                                    //    rowNumber = Business.Market.AgentList[j].ListHistoryReport.Count;
                                                                }
                                                            }
                                                            #endregion
                                                        }

                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (StringResult.Count == 0)
                                    {
                                        string message = subValue[0] + "$";
                                        StringResult.Add(message);
                                    }
                                }
                                break;
                            #endregion

                            #region FUNCTION SYSTEM LOG
                            case "GetLogByTime":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        string[] subParameter = subValue[1].Split(',');
                                        if (subParameter.Length == 7)
                                        {
                                            #region GET PARAMETER
                                            List<Business.SystemLog> result = new List<SystemLog>();
                                            DateTime begin;
                                            DateTime end;
                                            int typeID = 0;
                                            int positionStart = 0;
                                            int positionEnd = 0;

                                            int managerID = 0;
                                            bool isExists = false;
                                            //GetLogByTime$00000093,-1,05/01/2012 00:00:00,12/13/2012 15:01:13,0,10000,37
                                            int.TryParse(subParameter[1], out typeID);
                                            DateTime.TryParse(subParameter[2], out begin);
                                            DateTime.TryParse(subParameter[3], out end);
                                            int.TryParse(subParameter[4], out positionStart);
                                            int.TryParse(subParameter[5], out positionEnd);
                                            int.TryParse(subParameter[6], out managerID);
                                            #endregion

                                            #region GET LOG IF POSITION START = 0
                                            if (positionStart == 0)
                                            {
                                                if (string.IsNullOrEmpty(subParameter[0]))
                                                {
                                                    if (typeID == -1)
                                                    {
                                                        result = TradingServer.Facade.FacadeGetSystemLogByTime(begin, end);
                                                    }
                                                    else
                                                    {
                                                        result = TradingServer.Facade.FacadeGetSystemLogByTimeAndType(typeID, begin, end);
                                                    }
                                                }
                                                else
                                                {
                                                    if (typeID == -1)
                                                    {
                                                        string contentSearch = "%" + subParameter[0] + "%";
                                                        
                                                        //IPAddress IP = null;
                                                        bool isParseIP = true;//IPAddress.TryParse(subParameter[0], out IP);
                                                        bool isNumber = false;
                                                        int valueAddress = -1;
                                                        int dotIndex = subParameter[0].IndexOf('.');
                                                        if (dotIndex > 0)
                                                        {
                                                            string[] subAddress = subParameter[0].Split('.');
                                                            if (subAddress.Length == 4)
                                                            {
                                                                for (int n = 0; n < subAddress.Length; n++)
                                                                {
                                                                    isNumber = int.TryParse(subAddress[n], out valueAddress);
                                                                    if (!isNumber)
                                                                    {
                                                                        isParseIP = false;
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (valueAddress > 254)
                                                                        {
                                                                            isParseIP = false;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                isParseIP = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            isParseIP = false;
                                                        }

                                                        if (isParseIP)
                                                            result = TradingServer.Facade.FacadeGetLogByIPAddress(begin, end, subParameter[0]);
                                                        else
                                                            result = TradingServer.Facade.FacadeGetLogLikeCode(begin, end, contentSearch);
                                                    }
                                                    else
                                                    {
                                                        string contentSearch = "%" + subParameter[0] + "%";
                                                        //IPAddress IP = null;
                                                        bool isParseIP = true;//IPAddress.TryParse(subParameter[0], out IP);
                                                        bool isNumber = false;
                                                        int valueAddress = -1;
                                                        int dotIndex = subParameter[0].IndexOf('.');
                                                        if (dotIndex > 0)
                                                        {
                                                            string[] subAddress = subParameter[0].Split('.');
                                                            if (subAddress.Length == 4)
                                                            {
                                                                for (int n = 0; n < subAddress.Length; n++)
                                                                {
                                                                    isNumber = int.TryParse(subAddress[n], out valueAddress);
                                                                    if (!isNumber)
                                                                    {
                                                                        isParseIP = false;
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (valueAddress > 254)
                                                                        {
                                                                            isParseIP = false;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                isParseIP = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            isParseIP = false;
                                                        }

                                                        if (isParseIP)
                                                            result = TradingServer.Facade.FacadeGetLogByIPAddressAndType(begin, end, subParameter[0], typeID);
                                                        else
                                                            result = TradingServer.Facade.FacadeGetSystemLogByCodeAndTime(begin, end, typeID, contentSearch);
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region MAP RESULT TO AGENT LIST SYSTEM LOG
                                            if (Business.Market.AdminList != null && Business.Market.AdminList.Count > 0)
                                            {
                                                int countAgent = Business.Market.AdminList.Count;
                                                for (int j = 0; j < countAgent; j++)
                                                {
                                                    if (Business.Market.AdminList[j].AgentID == managerID)
                                                    {
                                                        if (positionStart == 0)
                                                        {
                                                            Business.Market.AdminList[j].ListSystemLog = new List<SystemLog>();

                                                            if (result != null)
                                                            {
                                                                int countResult = result.Count;
                                                                for (int n = 0; n < countResult; n++)
                                                                {
                                                                    Business.Market.AdminList[j].ListSystemLog.Add(result[n]);
                                                                }
                                                            }
                                                        }

                                                        int rowNumber = positionEnd - positionStart;
                                                        if (Business.Market.AdminList[j].ListSystemLog.Count < rowNumber)
                                                            rowNumber = Business.Market.AdminList[j].ListSystemLog.Count;

                                                        for (int m = 0; m < rowNumber; m++)
                                                        {
                                                            string message = subValue[0] + "$" + Business.Market.AdminList[j].ListSystemLog[0].ID + "," +
                                                                Business.Market.AdminList[j].ListSystemLog[0].LogDay + "," +
                                                                Business.Market.AdminList[j].ListSystemLog[0].IPAddress + "," +
                                                                Business.Market.AdminList[j].ListSystemLog[0].LogContent + "," +
                                                                Business.Market.AdminList[j].ListSystemLog[0].Comment;

                                                            StringResult.Add(message);

                                                            Business.Market.AdminList[j].ListSystemLog.RemoveAt(0);
                                                        }

                                                        isExists = true;

                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region MAP RESULT TO AGENT LIST SYSTEM LOG
                                            if (!isExists)
                                            {
                                                if (Business.Market.AgentList != null && Business.Market.AgentList.Count > 0)
                                                {
                                                    int countAgent = Business.Market.AgentList.Count;
                                                    for (int j = 0; j < countAgent; j++)
                                                    {
                                                        if (Business.Market.AgentList[j].AgentID == managerID)
                                                        {
                                                            if (positionStart == 0)
                                                            {
                                                                Business.Market.AgentList[j].ListSystemLog = new List<SystemLog>();

                                                                if (result != null)
                                                                {
                                                                    int countResult = result.Count;
                                                                    for (int n = 0; n < countResult; n++)
                                                                    {
                                                                        Business.Market.AgentList[j].ListSystemLog.Add(result[n]);
                                                                    }
                                                                }
                                                            }

                                                            int rowNumber = positionEnd - positionStart;
                                                            if (Business.Market.AgentList[j].ListSystemLog.Count < rowNumber)
                                                                rowNumber = Business.Market.AgentList[j].ListSystemLog.Count;

                                                            for (int m = 0; m < rowNumber; m++)
                                                            {
                                                                string message = subValue[0] + "$" + Business.Market.AgentList[j].ListSystemLog[0].ID + "," +
                                                                    Business.Market.AgentList[j].ListSystemLog[0].LogDay + "," +
                                                                    Business.Market.AgentList[j].ListSystemLog[0].IPAddress + "," +
                                                                    Business.Market.AgentList[j].ListSystemLog[0].LogContent + "," +
                                                                    Business.Market.AgentList[j].ListSystemLog[0].Comment;

                                                                StringResult.Add(message);

                                                                Business.Market.AgentList[j].ListSystemLog.RemoveAt(0);
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region INSERT SYSTEM LOG
                                            string content = "'" + code + "': Requested '" + subParameter[0] + "' ";

                                            string contentResult = string.Empty;
                                            if (result != null)
                                            {
                                                if (result.Count > 0)
                                                    contentResult = result.Count + " rows returned";
                                                else
                                                    contentResult = result.Count + " row returned";
                                            }
                                            else
                                            {
                                                contentResult = "0 row returned";
                                            }

                                            content = content + contentResult;
                                            string comment = "[Requested Log]";
                                            TradingServer.Facade.FacadeAddNewSystemLog(3, content, comment, ipAddress, code);
                                            #endregion
                                        }
                                    }

                                    #region RETURN -1 IF DON'T FIND LOG
                                    if (StringResult.Count == 0)
                                    {
                                        string message = subValue[0] + "$-1";
                                        StringResult.Add(message);
                                    }
                                    #endregion
                                }
                                break;
                            #endregion

                            #region GET TOP INTERNAL MAIL TO INVESTOR
                            case "GetTopInternalMailToInvestor":
                                {
                                    List<Business.InternalMail> result = new List<InternalMail>();
                                    result = TradingServer.Facade.FacadeGetTopInternalMailToInvestor(subValue[1]);

                                    if (result != null)
                                    {
                                        int countResult = result.Count;
                                        for (int j = 0; j < countResult; j++)
                                        {
                                            string message = subValue[j] + "$" + result[j].InternalMailID + "," + TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].Subject) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].From) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].FromName) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].Time.ToString()) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].Content);
                                            StringResult.Add(message);
                                        }
                                    }

                                    if (StringResult.Count == 0)
                                    {
                                        StringResult.Add(subValue[1] + "$-1");
                                    }
                                }
                                break;

                            case "GetInternalMailToInvestor":
                                {
                                    int start = 0, end = 0;
                                    string[] tem = subValue[1].Split(',');
                                    int.TryParse(tem[0], out start);
                                    int.TryParse(tem[1], out end);
                                    List<Business.InternalMail> result = new List<InternalMail>();
                                    result = TradingServer.Facade.FacadeGetInternalMailToInvestor(start, end, tem[2]);

                                    if (result != null)
                                    {
                                        int countResult = result.Count;
                                        for (int j = 0; j < countResult; j++)
                                        {
                                            string message = subValue[0] + "$" + result[j].InternalMailID + "," + TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].Subject) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].From) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].FromName) + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].Time.ToString()) + "," +
                                                result[j].IsNew.ToString() + "," +
                                                TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(result[j].Content);
                                            StringResult.Add(message);
                                        }
                                    }

                                    if (StringResult.Count == 0)
                                    {
                                        StringResult.Add(subValue[0] + "$");
                                    }
                                }
                                break;

                            #endregion

                            #region CHECK TIME EVENT DEBUG
                            case "DayEvent":
                                {
                                    if (Business.Market.DayEvent != null)
                                    {
                                        int countDayEvent = Business.Market.DayEvent.Count;
                                        for (int j = 0; j < countDayEvent; j++)
                                        {
                                            if (Business.Market.DayEvent[j].TargetFunction != null)
                                            {
                                                int countTarget = Business.Market.DayEvent[j].TargetFunction.Count;
                                                for (int n = 0; n < countTarget; n++)
                                                {
                                                    string message = subValue[0] + "$Event Type: " + Business.Market.DayEvent[j].EventType + "-Every: " + Business.Market.DayEvent[j].Every + "-Enable: " +
                                                        Business.Market.DayEvent[j].IsEnable + "-Event Position: " + Business.Market.DayEvent[j].TargetFunction[n].EventPosition + "-Day EventID: " +
                                                        Business.Market.DayEvent[j].TimeEventID + "-Time Execution: " + Business.Market.DayEvent[j].TimeExecution + " " + Business.Market.TimerEventDay.Interval;

                                                    StringResult.Add(message);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case "WeekEvent":
                                {
                                    if (Business.Market.WeekEvent != null)
                                    {
                                        int countWeekEvent = Business.Market.WeekEvent.Count;
                                        for (int j = 0; j < countWeekEvent; j++)
                                        {
                                            if (Business.Market.WeekEvent[j].TargetFunction != null)
                                            {
                                                int countTarget = Business.Market.WeekEvent[j].TargetFunction.Count;
                                                for (int n = 0; n < countTarget; n++)
                                                {
                                                    string message = subValue[0] + "$Event Type: " + Business.Market.WeekEvent[j].EventType + "-Every: " + Business.Market.WeekEvent[j].Every + "-Enable: " +
                                                        Business.Market.WeekEvent[j].IsEnable + "-Event Position: " + Business.Market.WeekEvent[j].TargetFunction[n].EventPosition + "-EventID: " +
                                                        Business.Market.WeekEvent[j].TimeEventID + "-TimeExecution: " + Business.Market.WeekEvent[j].TimeExecution + " " + Business.Market.TimerEventWeek.Interval;

                                                    StringResult.Add(message);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }

                            case "YearEvent":
                                {
                                    if (Business.Market.YearEvent != null)
                                    {
                                        if (Business.Market.YearEvent != null)
                                        {
                                            int countYearEvent = Business.Market.YearEvent.Count;
                                            for (int j = 0; j < countYearEvent; j++)
                                            {
                                                if (Business.Market.YearEvent[j].TargetFunction != null)
                                                {
                                                    int countTarget = Business.Market.YearEvent[j].TargetFunction.Count;
                                                    for (int n = 0; n < countTarget; n++)
                                                    {
                                                        string message = subValue[0] + "$Event Type: " + Business.Market.YearEvent[j].EventType + "-Every: " + Business.Market.YearEvent[j].Every + "-Enable: " +
                                                            Business.Market.YearEvent[j].IsEnable + "-EventPosition: " + Business.Market.YearEvent[j].TargetFunction[n].EventPosition + "-EventID: " +
                                                            Business.Market.YearEvent[j].TimeEventID + "-Time Execution: " + Business.Market.YearEvent[j].TimeExecution + " " + Business.Market.TimerEventYear.Interval;

                                                        StringResult.Add(message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GET TICK ONLINE INIT MARKET
                            case "GetTickOnline":
                                {
                                    if (Business.Market.QuoteList != null)
                                    {
                                        int countQuote = Business.Market.QuoteList.Count;
                                        for (int j = 0; j < countQuote; j++)
                                        {
                                            if (Business.Market.QuoteList[i].RefSymbol != null)
                                            {
                                                int countRefSymbol = Business.Market.QuoteList[i].RefSymbol.Count;
                                                for (int n = 0; n < countRefSymbol; n++)
                                                {
                                                    if (Business.Market.QuoteList[j].RefSymbol[n].TickValue != null &&
                                                        !string.IsNullOrEmpty(Business.Market.QuoteList[j].RefSymbol[n].TickValue.SymbolName))
                                                    {
                                                        string message = Business.Market.QuoteList[j].RefSymbol[n].TickValue.Bid + "?" +
                                                            //Business.Market.QuoteList[j].RefSymbol[n].TickValue.Ask + "?" +
                                                            Business.Market.QuoteList[j].RefSymbol[n].TickValue.TickTime + "?" +
                                                            Business.Market.QuoteList[j].RefSymbol[n].TickValue.SymbolName + "?" +
                                                            Business.Market.QuoteList[j].RefSymbol[n].TickValue.HighInDay + "?" +
                                                            Business.Market.QuoteList[j].RefSymbol[n].TickValue.LowInDay + "?" +
                                                            //Business.Market.QuoteList[j].RefSymbol[n].TickValue.HighAsk + "?" +
                                                            //Business.Market.QuoteList[j].RefSymbol[n].TickValue.LowAsk + "?" +
                                                            Business.Market.QuoteList[j].RefSymbol[n].TickValue.Status;

                                                        StringResult.Add(message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case "GetTickOnlineManager":
                                {
                                    bool checkip = Facade.FacadeCheckIpManagerAndAdmin(code, ipAddress);
                                    if (checkip)
                                    {
                                        if (Business.Market.SymbolList != null)
                                        {
                                            for (int j = Business.Market.SymbolList.Count - 1; j >= 0; j--)
                                            {
                                                if (Business.Market.SymbolList[j] != null)
                                                {
                                                    if (Business.Market.SymbolList[j].TickValue != null)
                                                    {
                                                        string message = "GetTickOnlineManager$" + Business.Market.SymbolList[j].TickValue.Bid + "}" +
                                                            Business.Market.SymbolList[j].TickValue.Ask + "}" +
                                                            Business.Market.SymbolList[j].TickValue.SymbolName + "}" +
                                                            Business.Market.SymbolList[j].TickValue.TickTime.Ticks + "}" +
                                                            Business.Market.SymbolList[j].TickValue.Status + "}" +
                                                            Business.Market.SymbolList[j].TickValue.HighInDay + "}" +
                                                            Business.Market.SymbolList[j].TickValue.LowInDay;
                                                        StringResult.Add(message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            #endregion

                            #region GET CANDLES DAY(GET OPEN OPEN PRICE IN DAY)
                            case "GetCandlesByDate":
                                {
                                    string[] subParameter = subValue[1].Split('|');
                                    int port = -1;
                                    int investorID = -1;
                                    DateTime time;
                                    List<string> listSymbol = new List<string>();
                                    if (subParameter.Length == 7)
                                    {
                                        int month, day, year;

                                        int.TryParse(subParameter[1], out month);
                                        int.TryParse(subParameter[2], out day);
                                        int.TryParse(subParameter[3], out year);

                                        time = new DateTime(year, month, day, 00, 00, 00);

                                        int.TryParse(subParameter[4], out port);
                                        int.TryParse(subParameter[5], out investorID);

                                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[6]);

                                        string[] subListSymbol = subParameter[0].Split('{');
                                        int countSymbol = subListSymbol.Length;
                                        for (int j = 0; j < countSymbol; j++)
                                        {
                                            if (!string.IsNullOrEmpty(subListSymbol[j]))
                                            {
                                                listSymbol.Add(subListSymbol[j]);
                                            }
                                        }

                                        List<string> temp = ProcessQuoteLibrary.Business.QuoteProcess.GetCandlesByDate(listSymbol, time, port);

                                        if (temp != null && temp.Count > 0)
                                        {
                                            int countTemp = temp.Count;
                                            for (int j = 0; j < countTemp; j++)
                                            {
                                                if (!string.IsNullOrEmpty(temp[j]))
                                                {
                                                    string message = subValue[0] + "$" + temp[j];
                                                    StringResult.Add(message);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string message = subValue[0] + "$";
                                            StringResult.Add(message);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region ADMIN OR MANGER REQUEST CLIENT LOG
                            case "RequestClientLog":
                                {
                                    if (Business.Market.ListClientLogs != null)
                                    {
                                        int countClientLog = Business.Market.ListClientLogs.Count;
                                        for (int j = 0; j < count; j++)
                                        {
                                            if (Business.Market.ListClientLogs[j].AdminCode == code)
                                            {
                                                int countWhile = 0;
                                                while (!Business.Market.ListClientLogs[j].IsComplete)
                                                {   
                                                    System.Threading.Thread.Sleep(1000);
                                                    countWhile++;
                                                    if (countWhile == 5)
                                                        Business.Market.ListClientLogs[j].IsComplete = true;
                                                }

                                                if (Business.Market.ListClientLogs[j].ClientLogs.Count > 0)
                                                    StringResult = Business.Market.ListClientLogs[j].ClientLogs;
                                                else
                                                    StringResult.Add(subValue[0] + "$");

                                                Business.Market.ListClientLogs[j].ClientLogs.Clear();

                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            #endregion
                        }
                    }
                }
            }

            return StringResult;
        }
    }
}
