using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace TradingServer
{
    public class SocketConnectPort
    {
        #region CREATE INSTANCE
        public static SocketConnectPort _instance;
        public static SocketConnectPort Instance
        {
            get
            {
                if (SocketConnectPort._instance == null)
                    SocketConnectPort._instance = new SocketConnectPort();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ClientPort(string value, string ipAddress, Socket InsSocket)
        {
            string[] subValue = value.Split('$');
            switch (subValue[0])
            {
                #region BUYSPOT COMMAND
                case "BuyCommand":
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommand(subValue[1]);
                        objCommand.IsBuy = true;
                        objCommand.CommandType = "1";
                        objCommand.IpAddress = ipAddress;

                        TradingServer.ClientFacade.FacadeMakeCommand(objCommand);
                    }
                    break;
                #endregion

                #region SELLSPOT COMMAND
                case "SellCommand":
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommand(subValue[1]);
                        objCommand.IsBuy = false;
                        objCommand.CommandType = "2";
                        objCommand.IpAddress = ipAddress;

                        TradingServer.ClientFacade.FacadeMakeCommand(objCommand);
                    }
                    break;
                #endregion

                #region BUYSTOP COMMAND
                case "BuyStop":
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommand(subValue[1]);
                        objCommand.IsBuy = true;
                        objCommand.CommandType = "9";
                        objCommand.IpAddress = ipAddress;

                        //CHECK VALID IPADDRESS
                        bool checkIp = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIp)
                            checkOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
                        else
                            return;

                        if (!checkOnline)
                            return;

                        TradingServer.ClientFacade.FacadeMakeCommand(objCommand);
                    }
                    break;
                #endregion

                #region BUYLIMIT COMMAND
                case "BuyLimit":
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommand(subValue[1]);
                        objCommand.IsBuy = true;
                        objCommand.CommandType = "7";
                        objCommand.IpAddress = ipAddress;

                        //CHECK VALID IPADDRESS
                        bool checkIp = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIp)
                            checkOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
                        else
                            return;

                        if (!checkOnline)
                            return;

                        TradingServer.ClientFacade.FacadeMakeCommand(objCommand);
                    }
                    break;
                #endregion

                #region SELLSTOP COMMAND
                case "SellStop":
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommand(subValue[1]);
                        objCommand.IsBuy = false;
                        objCommand.CommandType = "10";
                        objCommand.IpAddress = ipAddress;

                        //CHECK VALID IPADDRESS
                        bool checkIp = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIp)
                            checkOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
                        else
                            return;

                        if (!checkOnline)
                            return;

                        TradingServer.ClientFacade.FacadeMakeCommand(objCommand);
                    }
                    break;
                #endregion

                #region SELLLIMIT COMMAND
                case "SellLimit":
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommand(subValue[1]);
                        objCommand.IsBuy = false;
                        objCommand.CommandType = "8";
                        objCommand.IpAddress = ipAddress;

                        //CHECK VALID IPADDRESS
                        bool checkIp = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIp)
                            checkOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
                        else
                            return;

                        if (!checkOnline)
                            return;

                        TradingServer.ClientFacade.FacadeMakeCommand(objCommand);
                    }
                    break;
                #endregion

                #region UPDATE ONLINE COMMAND
                case "UpdateOnlineCommand": //Update Online Command And Pending Order Command
                    {
                        TradingServer.ClientBusiness.Command objCommand = TradingServer.Model.BuildCommand.Instance.ConvertStringToCommands(subValue[1]);
                        objCommand.IpAddress = ipAddress;

                        //CHECK VALID IPADDRESS
                        bool checkIp = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIp)
                            checkOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
                        else
                            return;

                        if (!checkOnline)
                            return;

                        TradingServer.ClientFacade.FacadeUpdateOnlineCommand(objCommand);
                    }
                    break;
                #endregion

                #region CLOSE COMMAND
                case "CloseCommand":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int commandID = -1;
                        double size = -1;
                        double closePrice = -1;
                        int investorID = -1;

                        int.TryParse(subParameter[0], out commandID);
                        double.TryParse(subParameter[1], out closePrice);
                        double.TryParse(subParameter[2], out size);
                        int.TryParse(subParameter[5], out investorID);

                        TradingServer.ClientBusiness.Command objCommand = new ClientBusiness.Command();
                        objCommand.CommandID = commandID;
                        objCommand.ClosePrice = closePrice;
                        objCommand.Size = size;
                        objCommand.Symbol = subParameter[3];
                        objCommand.LoginKey = subParameter[4];
                        objCommand.IpAddress = ipAddress;
                        objCommand.InvestorID = investorID;

                        TradingServer.ClientFacade.FacadeCloseSpotCommand(objCommand);
                    }
                    break;
                #endregion
            }
        }

        /// <summary>
        /// //GetTopCandlesByTimeFrame$60{500{6/13/2013 1:59:00 PM{EURUSD{0{10666{mt7jqtpp
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string StringClientPort(string value, string ipAddress, Socket InsSocket)
        {
            string result = string.Empty;
            string[] subValue = value.Split('$');
            switch (subValue[0])
            {
                #region CREATE NEW INVESTOR ACCOUNT
                case "CreateNewInvestor":
                    {
                        //CreateNewInvestor$10000{100{gdjffti{244566444{hcm{Vietnam{abc123@gmail.com{23232{{duyen demo
                        string[] subParameter = subValue[1].Split('{');
                        double balance, Leverage;
                        double.TryParse(subParameter[0], out balance);
                        double.TryParse(subParameter[1], out Leverage);

                        string pwd = Model.ValidateCheck.RandomString(8);
                        TradingServer.Business.Investor investor = null;

                        if (Business.Market.IsConnectMT4)
                        {
                            //Connect MT4
                            investor = TradingServer.ClientFacade.FacadeAddNewInvestorWithMT4(5, 40, "0", balance, 0, "", pwd, 0, Leverage, subParameter[2], subParameter[3],
                                subParameter[4], subParameter[5], subParameter[6], subParameter[7], subParameter[8], subParameter[9]);
                        }
                        else
                        {
                            investor = TradingServer.ClientFacade.FacadeAddNewInvestor(5, 40, "0", balance, 0, "", pwd, 0, Leverage,
                                subParameter[2], subParameter[3], subParameter[4], subParameter[5], subParameter[6], subParameter[7], subParameter[8],
                                subParameter[9], subParameter[10]);
                        }

                        if (investor != null)
                        {
                            result = subValue[0] + "$" + investor.InvestorID + "{" + investor.Code + "{" + investor.PrimaryPwd;
                        }
                        else
                        {
                            result = subValue[0];
                        }
                    }
                    break;
                #endregion

                #region LOGOUT CLIENT
                case "Logout":
                    {
                        int investorID, investorIndex;
                        string[] subParameter = subValue[1].Split('{');
                        int.TryParse(subParameter[0], out investorID);
                        int.TryParse(subParameter[1], out investorIndex);

                        //bool logOut = TradingServer.ClientFacade.FacadeLogout(investorID, investorIndex);
                        bool logOut = TradingServer.ClientFacade.FacadeManualLogout(investorID, investorIndex, subParameter[2], ipAddress);

                        result = subValue[0] + "$" + logOut;
                    }
                    break;
                #endregion

                #region DIFFIRENCE LOGOUT CLIENT
                case "DiffLogout":
                    {
                        int investorID = -1;
                        int index = -1;
                        int logoutResult = -1;
                        string[] subParameter = subValue[1].Split('{');
                        int.TryParse(subParameter[0], out investorID);
                        int.TryParse(subParameter[1], out index);

                        bool logout = TradingServer.ClientFacade.FacadeLogout(investorID, index, subParameter[2], ipAddress);

                        if (logout)
                            logoutResult = 1;
                        else
                            logoutResult = 0;

                        result = subValue[0] + "$" + logoutResult;
                    }
                    break;
                #endregion

                #region CLIENT CANCEL REQUEST
                case "ClientCancelRequest":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[2], out investorID);

                        TradingServer.Business.RequestDealer newRequestDealer = new TradingServer.Business.RequestDealer();
                        newRequestDealer.Request = new TradingServer.Business.OpenTrade();
                        newRequestDealer.Request.Symbol = new TradingServer.Business.Symbol();
                        newRequestDealer.Request.Symbol.Name = subParameter[0];
                        newRequestDealer.Request.ClientCode = subParameter[1];
                        newRequestDealer.InvestorID = investorID;

                        bool cancelRequest = TradingServer.ClientFacade.FacadeClientCancelRequest(newRequestDealer);

                        result = subValue[0] + "$" + cancelRequest;
                    }
                    break;
                #endregion

                #region CHANGE PRIMARY PASSWORD
                case "ChangePrimaryPassword":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);

                        bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        if (!checkIP)
                            return result = subValue[0] + "$" + checkIP;

                        bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(investorID,
                            TradingServer.Business.TypeLogin.Primary, subParameter[3]);
                        if (!checkOnline)
                            return result = subValue[0] + "$" + checkOnline;

                        bool changePwd = TradingServer.Facade.FacadeChangePassword(investorID, subParameter[1], subParameter[2]);

                        result = subValue[0] + "$" + changePwd;
                    }
                    break;
                #endregion

                #region UPDATE USER CONFIG
                case "UpdateUserConfig":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);

                        bool updateUserConfig = TradingServer.Facade.FacadeUpdateUserConfig(investorID, subParameter[1]);

                        result = subValue[0] + "$" + updateUserConfig;
                    }
                    break;
                #endregion

                #region UPDATE USER CONFIG IPAD
                case "UpdateUserConfigIpad":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);

                        bool updateUserConfig = TradingServer.Facade.FacadeUpdateUserConfigIpad(investorID, subParameter[1]);

                        result = subValue[0] + "$" + updateUserConfig;
                    }
                    break;
                #endregion

                #region UPDATE USER CONFIG IPHONE
                case "UpdateUserConfigIphone":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);

                        bool updateUserConfig = TradingServer.Facade.FacadeUpdateUserConfigIphone(investorID, subParameter[1]);

                        result = subValue[0] + "$" + updateUserConfig;
                    }
                    break;
                #endregion

                #region GET DATA CLIENT(TICK, NOTIFICATION)
                case "GetData":
                    {
                        string[] subKey = subValue[1].Split('>');

                        string[] subParameter = subKey[0].Split('{');
                        int investorID, investorIndex;
                        int.TryParse(subParameter[0], out investorID);
                        int.TryParse(subParameter[2], out investorIndex);

                        result = TradingServer.ClientFacade.FacadeGetData(investorID, investorIndex, subParameter[3], 0, subKey[1]);
                        
                    }
                    break;
                #endregion

                #region GET INVESTOR BY INVESTOR ID
                case "GetInvestorByInvestorID":
                    {
                        int investorID = 0;
                        string[] subParameter = subValue[1].Split('{');

                        int.TryParse(subParameter[0], out investorID);
                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[1]);

                        Business.Investor investor = TradingServer.ClientFacade.FacadeGetInvestorByInvestor(investorID);

                        result = TradingServer.Model.BuildCommand.Instance.MapInvestorToString(investor);
                    }
                    break;
                #endregion

                #region GET INVESTOR BY CODE
                case "GetInvestorByCode":
                    {
                        Business.Investor investor = TradingServer.ClientFacade.FacadeGetInvestorByCode(subValue[1]);
                        result = TradingServer.Model.BuildCommand.Instance.MapInvestorToStringByCode(investor);
                    }
                    break;
                #endregion

                #region GET CLIENT CONFIG
                case "GetClientConfig":
                    {
                        int investorGroup = -1;
                        int.TryParse(subValue[1], out investorGroup);
                        TradingServer.Business.ClientConfig resultConfig = TradingServer.Facade.FacadeGetClientConfigByGroup(investorGroup);
                        if (resultConfig != null)
                        {
                            result = subValue[0] + "$" + resultConfig.FreeMarginFormular + "{" + resultConfig.LeverageGroup + "{" +
                                resultConfig.MarginCall + "{" + resultConfig.StopOut + "{" + resultConfig.TickSize + "{" +
                                resultConfig.TimeOut;
                        }
                        else
                        {
                            result = subValue[0] + "$";
                        }
                    }
                    break;
                #endregion

                #region GET TICK ONLINE
                case "GetTickOnline":
                    {
                        StringBuilder tempResult = new StringBuilder();
                        List<Business.Tick> listTick = TradingServer.Facade.FacadeGetTickOnline();
                        if (listTick != null)
                        {
                            int count = listTick.Count;
                            tempResult.Append(subValue[0]);
                            tempResult.Append("$");

                            for (int i = 0; i < count; i++)
                            {
                                tempResult.Append(listTick[i].Bid);
                                tempResult.Append("{");
                                tempResult.Append(listTick[i].Ask);
                                tempResult.Append("{");
                                tempResult.Append(listTick[i].HighInDay);
                                tempResult.Append("{");
                                tempResult.Append(listTick[i].LowInDay);
                                tempResult.Append("{");
                                tempResult.Append(listTick[i].Status);
                                tempResult.Append("{");
                                tempResult.Append(listTick[i].SymbolName);
                                tempResult.Append("{");
                                tempResult.Append(listTick[i].TickTime);
                                tempResult.Append("[");
                            }

                            result = tempResult.ToString();

                            result.Remove(result.Length - 1);
                        }
                    }
                    break;
                #endregion

                #region Mail
                case "GetFullInternalMailToInvestor":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[1], out investorID);

                        bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        if (!checkIP)
                        {
                            result = subValue[0] + "$";
                        }
                        else
                        {
                            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(investorID,
                            TradingServer.Business.TypeLogin.Primary, subParameter[2]);

                            if (!checkOnline)
                            {
                                result = subValue[0] + "$";
                            }
                            else
                            {
                                result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                            }
                        }
                    }
                    break;

                case "DeleteMailByID":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int mailID, investorID = -1;
                        int.TryParse(subParameter[0], out mailID);
                        int.TryParse(subParameter[1], out investorID);

                        bool checkIP = Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIP)
                        {
                            checkOnline = Business.Investor.investorInstance.CheckInvestorOnline(investorID, subParameter[2]);
                        }
                        else
                        {
                            return result = subValue[0] + "$";
                        }

                        if (!checkOnline)
                            return result + subValue[0] + "$";


                        string tempValue = subValue[0] + "$" + mailID;
                        TradingServer.Business.Market.marketInstance.ExtractCommandServer(tempValue, ipAddress, "");
                    }
                    break;

                case "GetCodeAgentSendMailByInvestorGroupID":
                    {
                        result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                    }
                    break;

                case "UpdateMailStatus":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[2], out investorID);
                        value = subValue[0] + "$" + subParameter[0] + "," + subParameter[1] + "," + subParameter[2] + "," + subParameter[3];
                        bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        if (!checkIP)
                        {
                            result = subValue[0] + "$";
                        }
                        else
                        {
                            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(investorID,
                            TradingServer.Business.TypeLogin.Primary, subParameter[3]);

                            checkOnline = true;
                            if (!checkOnline)
                            {
                                result = subValue[0] + "$";
                            }
                            else
                            {
                                result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                            }
                        }
                    }
                    break;

                case "SendInternalMailToClient":
                    {
                        string[] subParameter = subValue[1].Split('█');
                        int investorID = -1;
                        string subject = TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(subParameter[0]);
                        string from = TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(subParameter[1]);
                        string fromName = TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(subParameter[2]);
                        string content = TradingServer.Model.TradingCalculate.Instance.ConvertStringToHex(subParameter[3]);
                        value = subValue[0] + "$" + subject + "," + from + "," + fromName + "," + content + "," + subParameter[4] + "," + subParameter[5] + "," + subParameter[6];
                        int.TryParse(subParameter[5], out investorID);

                        bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        if (!checkIP)
                        {
                            result = subValue[0] + "$";
                        }
                        else
                        {
                            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(investorID,
                            TradingServer.Business.TypeLogin.Primary, subParameter[6]);

                            if (!checkOnline)
                            {
                                result = subValue[0] + "$";
                            }
                            else
                            {
                                result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                            }
                        }
                    }
                    break;
                #endregion

                #region Alert
                case "GetAlertByInvestorID":
                    {
                        result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                    }
                    break;

                case "SelectHistoryAlertWithTime":
                    {
                        result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                    }
                    break;
                case "InsertNewAlert":
                    {
                        result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                    }
                    break;

                case "ConfigAlert":
                    {
                        result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                    }
                    break;
                case "DeleteAlert":
                    {
                        result = TradingServer.Business.Market.marketInstance.ExtractCommandServer(value, ipAddress, "");
                    }
                    break;
                #endregion

                #region GET TOP CANDLES BY TIME FRAME
                case "GetTopCandlesByTimeFrame":    //Port 0: web | 1: wcf
                    {
                        //GetTopCandlesByTimeFrame
                        result = subValue[0] + "$";
                        int timeFrame = -1;
                        int numGet = -1;
                        int port = -1;
                        int investorID = -1;
                        DateTime time = DateTime.Now;

                        string[] subParameter = subValue[1].Split('{');

                        if (subParameter.Length != 7)
                            return result;

                        int.TryParse(subParameter[0], out timeFrame);
                        int.TryParse(subParameter[1], out numGet);
                        //DateTime.TryParse(subParameter[2], out time);

                        bool isParseDate = DateTime.TryParse(subParameter[2], out time);
                        if (!isParseDate)
                        {
                            if (subParameter[2].IndexOf('+') > 0)
                            {
                                subParameter[2] = subParameter[2].Replace('+', ' ');
                                DateTime.TryParse(subParameter[2], out time);
                            }
                        }

                        int.TryParse(subParameter[4], out port);
                        int.TryParse(subParameter[5], out investorID);

                        bool checkIP = Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIP)
                            checkOnline = Business.Investor.investorInstance.CheckInvestorOnline(investorID, subParameter[6]);
                        else
                            return result;

                        if (!checkOnline)
                            return result;

                        result += QuotesServer.QuotesFacade.FacadeGetCandlesByTimeFrame(timeFrame, numGet, subParameter[3], time, port);
                    }
                    break;
                #endregion

                #region GET CANDLES BY TIME
                case "GetCandlesByTime":
                    {
                        result = subValue[0] + "$";
                        int timeFrame = -1;
                        int port = -1;
                        DateTime time;
                        int investorID = -1;
                        string[] subParameter = subValue[1].Split('{');

                        if (subParameter.Length != 6)
                            return result;

                        int.TryParse(subParameter[0], out timeFrame);
                        DateTime.TryParse(subParameter[1], out time);
                        int.TryParse(subParameter[3], out port);
                        int.TryParse(subParameter[4], out investorID);

                        bool checkIP = Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIP)
                            checkOnline = Business.Investor.investorInstance.CheckInvestorOnline(investorID, subParameter[5]);
                        else
                            return result;

                        if (!checkOnline)
                            return result;

                        result += QuotesServer.QuotesFacade.FacadeGetPriceByTimeFrame(timeFrame, time, subParameter[2], port);
                    }
                    break;
                #endregion

                #region GET OPEN PRICE BY TIME FRAME
                case "GetOpenPriceByTimeFrame":
                    {
                        result = subValue[0] + "$";
                        int timeFrame = -1;
                        int numGet = -1;
                        int port = -1;
                        int investorID = -1;
                        DateTime time = DateTime.Now;


                        string[] subParameter = subValue[1].Split('{');

                        if (subParameter.Length != 7)
                            return result;

                        int.TryParse(subParameter[0], out timeFrame);
                        int.TryParse(subParameter[1], out numGet);

                        bool isParseDate = DateTime.TryParse(subParameter[2], out time);
                        if (!isParseDate)
                        {
                            if (subParameter[2].IndexOf('+') > 0)
                            {
                                subParameter[2] = subParameter[2].Replace('+', ' ');
                                DateTime.TryParse(subParameter[2], out time);
                            }
                        }

                        int.TryParse(subParameter[3], out port);
                        int.TryParse(subParameter[4], out investorID);

                        List<string> listSymbol = new List<string>();
                        string[] subSymbolList = subParameter[6].Split('|');
                        for (int i = 0; i < subSymbolList.Length; i++)
                        {
                            listSymbol.Add(subSymbolList[i]);
                        }

                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[5]);

                        bool checkIP = Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIP)
                            checkOnline = Business.Investor.investorInstance.CheckInvestorOnline(investorID, subParameter[5]);
                        else
                            return result;

                        if (!checkOnline)
                            return result;

                        if (listSymbol != null && listSymbol.Count > 0)
                        {
                            for (int i = 0; i < listSymbol.Count; i++)
                            {
                                string tempResult = string.Empty;
                                tempResult = QuotesServer.QuotesFacade.FacadeGetOpenPriceByTimeFrame(timeFrame, numGet, listSymbol[i], time, port);
                                if (tempResult.Length > 0)
                                {
                                    result += listSymbol[i] + '♦';
                                    result += tempResult;
                                    result += '▼';
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region GET ONLINE CANDLES
                case "GetOnlineCandles":
                    {
                        result = subValue[0] + "$";
                        int timeFrame = -1;
                        int port = -1;
                        int investorID = -1;
                        string[] subParameter = subValue[1].Split('{');

                        if (subParameter.Length != 5)
                            return result;

                        int.TryParse(subParameter[0], out timeFrame);
                        int.TryParse(subParameter[2], out port);
                        int.TryParse(subParameter[3], out investorID);

                        bool checkIP = Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIP)
                            checkOnline = Business.Investor.investorInstance.CheckInvestorOnline(investorID, subParameter[4]);
                        else
                            return result;

                        if (!checkOnline)
                            return result;

                        result += TradingServer.ClientFacade.FacadeGetCandleOnline(timeFrame, subParameter[1], port);
                    }
                    break;
                #endregion

                #region GET LAST CANDLES
                case "GetLastCandles":
                    {
                        result = subValue[0] + "$";
                        int timeFrame = -1;
                        int port = -1;
                        int investorID = -1;
                        string[] subParameter = subValue[1].Split('{');

                        if (subParameter.Length != 5)
                            return result;

                        int.TryParse(subParameter[0], out timeFrame);
                        int.TryParse(subParameter[2], out port);
                        int.TryParse(subParameter[3], out investorID);

                        bool checkIP = Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        bool checkOnline = false;
                        if (checkIP)
                            checkOnline = Business.Investor.investorInstance.CheckInvestorOnline(investorID, subParameter[4]);
                        else
                            return result;

                        if (!checkOnline)
                            return result;

                        result += TradingServer.Facade.FacadeGetLastCandles(subParameter[1], timeFrame, port);
                    }
                    break;
                #endregion

                #region GET SYMBOL CONFIG
                case "GetSymbolConfig":
                    {
                        Business.Symbol newSymbol = TradingServer.Facade.FacadeGetSymbolConfig(subValue[1]);
                        if (newSymbol != null)
                        {
                            result = Model.BuildCommand.Instance.ConvertSymbolToString(newSymbol);
                        }
                    }
                    break;
                #endregion

                #region GET SECURITY
                case "GetSecurity":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        List<int> listSecurity = new List<int>();
                        for (int i = 0; i < subParameter.Length; i++)
                        {
                            listSecurity.Add(int.Parse(subParameter[i]));
                        }

                        List<Business.Security> resultSecurity = Business.Market.marketInstance.GetSecurityByListSecurity(listSecurity);

                        result = Model.BuildCommand.Instance.MapListSecurityToString(resultSecurity);
                    }
                    break;
                #endregion

                #region CLIENT SEND LOG TO SERVER
                case "ClientActionLog":
                    {
                        //Device: 0 = silverlight Client | 1 = Ipad Client | 2 = Iphone Client | 3 = Android Client
                        //ClientActionLog$InvestorID{ClientDevice~Title{Message{Date|Title{Message{Date|Title{Message{Date
                        string[] subCommand = subValue[1].Split('~');
                        if (subCommand.Length == 2)
                        {
                            string[] subParameter = subCommand[0].Split('{');
                            int investorID = int.Parse(subParameter[0]);
                            int clientDevice = int.Parse(subParameter[1]);
                            string[] subActionLog = subCommand[1].Split('|');
                            if (subActionLog.Length > 0)
                            {
                                if (Business.Market.ListClientLogs != null)
                                {
                                    int count = Business.Market.ListClientLogs.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (Business.Market.ListClientLogs[i].InvestorID == investorID)
                                        {
                                            if (subActionLog != null && subActionLog.Length > 0)
                                            {
                                                int countLog = subActionLog.Length;
                                                for (int j = 0; j < countLog; j++)
                                                {
                                                    Business.Market.ListClientLogs[i].ClientLogs.Add(subActionLog[j]);
                                                }
                                            }

                                            Business.Market.ListClientLogs[i].ClientDevice = clientDevice;
                                            Business.Market.ListClientLogs[i].IsComplete = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            result = subValue[0] + "$True";
                        }
                        else
                        {
                            result = subValue[0] + "$False";
                        }
                    }
                    break;
                #endregion
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<string> ClientPorts(string value, string ipAddress, Socket InsSocket)
        {
            List<string> result = new List<string>();
            string[] subValue = value.Split('$');
            switch (subValue[0])
            {
                #region CLIENT LOGIN
                case "ClientLogin":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int index = int.Parse(subParameter[2]);
                        string pwd = Model.ValidateCheck.Encrypt(subParameter[1]);
                        Business.Investor objInvestor;

                        if (Business.Market.IsConnectMT4)
                            objInvestor = TradingServer.ClientFacade.FacadeLoginMT4(subParameter[0], subParameter[1], ipAddress);
                        else
                            objInvestor = TradingServer.ClientFacade.FacadeNewLogin(subParameter[0], pwd, index, ipAddress, InsSocket);

                        if (objInvestor != null && objInvestor.InvestorID > 0)
                        {
                            //CHECK ACCOUNT DISABLE
                            if (objInvestor.IsDisable)
                            {
                                string message = subValue[0] + "$IAD1478235";
                                result.Add(message);

                                return result;
                            }

                            if (objInvestor.InvestorGroupInstance != null)
                            {
                                if (!objInvestor.InvestorGroupInstance.IsEnable)
                                {
                                    string message = subValue[0] + "$IGA0368469";
                                    result.Add(message);

                                    return result;
                                }
                            }

                            result = Model.BuildCommand.Instance.ConvertInvestorToString(objInvestor);
                        }
                        else
                        {
                            if (objInvestor == null)
                                result.Add(subValue[0] + "$LIA003");
                            else
                            {
                                if (!string.IsNullOrEmpty(objInvestor.InvestorStatusCode))
                                {
                                    result.Add(objInvestor.InvestorStatusCode);
                                }
                                else
                                {
                                    result.Add(subValue[0] + "$LIA003");
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region GET ONLINE COMMAND BY INVESTOR
                case "GetOnlineCommandByInvestor":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);
                        List<TradingServer.ClientBusiness.Command> listOnlineCommand = TradingServer.ClientFacade.FacadeGetOnlineCommandByInvestor(investorID, 1);

                        result = Model.BuildCommand.Instance.ConvertCommandToString(listOnlineCommand, 1);
                    }
                    break;
                #endregion

                #region GET ONLINE COMMAND BY INVESTOR CODE
                case "GetOnlineCommandByCode":
                    {
                        List<TradingServer.ClientBusiness.Command> listOnlineCommand = TradingServer.ClientFacade.FacadeGetOnlineCommandByInvestor(subValue[1], 1);

                        result = Model.BuildCommand.Instance.ConvertCommandToString(listOnlineCommand, 4);
                    }
                    break;
                #endregion

                #region GET COMMAND HISTORY WITH TIME
                case "GetCommandHistoryWithTime":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);
                        DateTime timeStart, timeEnd;
                        string start = this.ReplaceString(subParameter[1]);
                        string end = this.ReplaceString(subParameter[2]);
                        bool isParseTimeStart = DateTime.TryParse(start, out timeStart);

                        #region PARSE TIME FROM COMMAND OF ANDROID
                        if (!isParseTimeStart)
                        {
                            if (start.IndexOf('+') > 0)
                            {
                                start = start.Replace('+', ' ');
                                DateTime.TryParse(start, out timeStart);
                            }
                        }
                        #endregion

                        isParseTimeStart = false;

                        isParseTimeStart = DateTime.TryParse(end, out timeEnd);

                        #region PARSE TIME FROM COMMAND OF ANDROID
                        if (!isParseTimeStart)
                        {
                            if (end.IndexOf('+') > 0)
                            {
                                end = end.Replace('+', ' ');
                                DateTime.TryParse(end, out timeEnd);
                            }
                        }
                        #endregion

                        List<TradingServer.ClientBusiness.Command> listCommandHistory = TradingServer.ClientFacade.FacadeGetCommandHistoryWithDateTime(investorID, timeStart, timeEnd);

                        result = Model.BuildCommand.Instance.ConvertCommandToString(listCommandHistory, 2);
                    }
                    break;
                #endregion

                #region GET HISTORY WITH TIME MULTI ACCOUNT(FOR BROKER)
                //For Broker
                case "GetHistoryWithTimeMultiAccount":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);
                        DateTime timeStart, timeEnd;
                        string start = this.ReplaceString(subParameter[1]);
                        string end = this.ReplaceString(subParameter[2]);
                        DateTime.TryParse(start, out timeStart);
                        DateTime.TryParse(end, out timeEnd);

                        List<TradingServer.ClientBusiness.Command> listCommandHistory = TradingServer.ClientFacade.FacadeGetCommandHistoryWithDateTime(investorID, timeStart, timeEnd);

                        result = Model.BuildCommand.Instance.ConvertCommandToString(listCommandHistory, 5);
                    }
                    break;
                #endregion

                #region GET PENDING ORDER BY INVESTOR ID
                case "GetPendingOrderByInvestorID":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);

                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[1]);

                        List<TradingServer.ClientBusiness.Command> listPendingOrder = TradingServer.ClientFacade.FacadeGetPendingOrderByInvestorID(investorID);

                        result = Model.BuildCommand.Instance.ConvertCommandToString(listPendingOrder, 3);
                    }
                    break;
                #endregion

                #region GET TOP INTERNAL MAIL TO INVESTOR
                case "GetTopInternalMailToInvestor":
                    {
                        List<Business.InternalMail> listInternalMail = TradingServer.ClientFacade.FacadeGetTopInternalMailToInvestor(subValue[1]);

                        result = TradingServer.Model.BuildCommand.Instance.ConvertInternalMailToString(listInternalMail);
                    }
                    break;
                #endregion

                #region GET MARKET CONFIG
                case "GetMarketConfig":
                    {
                        List<Business.ParameterItem> listMarketConfig = TradingServer.ClientFacade.FacadeGetMarketConfig();

                        result = Model.BuildCommand.Instance.ConvertMarketConfigToString(listMarketConfig);
                    }
                    break;
                #endregion

                #region GET IGROUP SECURITY
                case "GetIGroupSecurity":
                    {
                        int investorGroupID = 0;
                        int.TryParse(subValue[1], out investorGroupID);
                        List<Business.IGroupSecurity> listIGroupSecurity = TradingServer.Facade.FacadeGetIGroupSecurityByGroup(investorGroupID);

                        if (listIGroupSecurity != null && listIGroupSecurity.Count > 0)
                            result = Model.BuildCommand.Instance.ConvertIGroupSecurityToString(listIGroupSecurity);
                        else
                            result.Add(subValue[0] + "$");
                    }
                    break;
                #endregion

                #region GET IGROUP SYMBOL
                case "GetIGroupSymbol":
                    {
                        int investorGroupID = 0;
                        int investorID = -1;
                        string[] subParameter = subValue[1].Split('{');

                        int.TryParse(subParameter[0], out investorGroupID);
                        int.TryParse(subParameter[1], out investorID);

                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[2]);

                        List<Business.IGroupSymbol> listIGroupSymbol = TradingServer.Facade.FacadeGetIGroupSymbolByGroup(investorGroupID);

                        if (listIGroupSymbol != null && listIGroupSymbol.Count > 0)
                        {
                            result = Model.BuildCommand.Instance.ConvertIGroupSymbolToString(listIGroupSymbol);
                        }
                        else
                            result.Add(subValue[0] + "$");
                    }
                    break;
                #endregion

                #region GET LIST SYMBOL
                case "GetListSymbol":
                    {
                        string[] subParameter = subValue[1].Split('[');
                        int investorID = -1;
                        int.TryParse(subParameter[1], out investorID);

                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[2]);

                        List<Business.IGroupSecurity> listIGroupSecurity = Model.BuildCommand.Instance.ConvertStringToIGroupSecurity(subParameter[0]);

                        List<Business.Symbol> listSymbol = TradingServer.Facade.FacadeGetSymbolByIGroupSecurity(listIGroupSecurity);

                        if (listSymbol != null)
                        {
                            int count = listSymbol.Count;
                            for (int i = 0; i < count; i++)
                            {
                                double spreadDiffirence = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(listSymbol[i].SecurityID, listIGroupSecurity[0].InvestorGroupID);
                                listSymbol[i].SpreadDifference = spreadDiffirence;
                                double minLots = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetMinLots(listSymbol[i].SecurityID, listIGroupSecurity[0].InvestorGroupID);
                                listSymbol[i].MinLots = minLots;
                                double maxLots = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetMaxLots(listSymbol[i].SecurityID, listIGroupSecurity[0].InvestorGroupID);
                                listSymbol[i].MaxLots = maxLots;
                                double stepLots = TradingServer.Model.CommandFramework.CommandFrameworkInstance.GetStepLots(listSymbol[i].SecurityID, listIGroupSecurity[0].InvestorGroupID);
                                listSymbol[i].StepLots = stepLots;
                            }
                        }

                        result = Model.BuildCommand.Instance.ConvertSymbolToString(listSymbol);
                    }
                    break;
                #endregion

                #region SELECT TOP NEWS
                case "SelectTopNews":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[0], out investorID);
                        Business.Investor.investorInstance.UpdateLastConnect(investorID, subParameter[1]);
                        result = Business.Market.marketInstance.ExtractServerCommand(value, ipAddress, "");
                    }
                    break;
                #endregion

                #region DELETE MAIL BY ID
                case "DeleteMailByID":
                    {
                        string[] subParameter = subValue[1].Split('{');
                        int investorID = -1;
                        int.TryParse(subParameter[1], out investorID);

                        bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(investorID, ipAddress);
                        if (!checkIP)
                        {
                            result.Add(subValue[1] + "$");
                        }
                        else
                        {
                            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(investorID,
                            TradingServer.Business.TypeLogin.Primary, subParameter[2]);

                            if (!checkOnline)
                            {
                                result.Add(subValue[1] + "$");
                            }
                            else
                            {
                                result = TradingServer.Business.Market.marketInstance.ExtractServerCommand(value, ipAddress, "");
                            }
                        }
                    }
                    break;
                #endregion

                #region GET CANDLE BY DATE
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
                            int count = subListSymbol.Length;
                            for (int i = 0; i < count; i++)
                            {
                                if (!string.IsNullOrEmpty(subListSymbol[i]))
                                {
                                    listSymbol.Add(subListSymbol[i]);
                                }
                            }

                            List<string> temp = ProcessQuoteLibrary.Business.QuoteProcess.GetCandlesByDate(listSymbol, time, port);

                            if (temp != null && temp.Count > 0)
                            {
                                int countTemp = temp.Count;
                                for (int i = 0; i < countTemp; i++)
                                {
                                    if (!string.IsNullOrEmpty(temp[i]))
                                    {
                                        string message = subValue[0] + "$" + temp[i];
                                        result.Add(message);
                                    }
                                }
                            }
                            else
                            {
                                string message = subValue[0] + "$";
                                result.Add(message);
                            }
                        }
                    }
                    break;
                #endregion

                #region IPAD REQUEST TICK
                case "IpadRequestTick":
                    {
                        if (Business.Market.SymbolList != null)
                        {
                            int count = Business.Market.SymbolList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.SymbolList[i].TickValue != null)
                                {
                                    string message = subValue[0] + "$" + Business.Market.SymbolList[i].TickValue.SymbolName + "{" +
                                        Business.Market.SymbolList[i].TickValue.Bid + "{" + Business.Market.SymbolList[i].TickValue.Ask +
                                        "{" + Business.Market.SymbolList[i].TickValue.TickTime.ToString("MM/dd/yyyy HH:mm:ss") + "{" + Business.Market.SymbolList[i].TickValue.HighInDay + "{" +
                                         Business.Market.SymbolList[i].TickValue.LowInDay;

                                    result.Add(message);
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region IPAD REQUEST SYMBOL
                case "IpadRequestSymbol":
                    {
                        if (Business.Market.SymbolList != null)
                        {
                            int count = Business.Market.SymbolList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                string message = subValue[0] + "$" + Business.Market.SymbolList[i].Name + "{" + Business.Market.SymbolList[i].Digit + "{" +
                                    Business.Market.SymbolList[i].TickValue.Bid + "{" + Business.Market.SymbolList[i].TickValue.Ask + "{" +
                                    Business.Market.SymbolList[i].TickValue.TickTime.ToString("MM/dd/yyyy HH:mm:ss") + "{" + Business.Market.SymbolList[i].TickValue.HighInDay + "{" +
                                    Business.Market.SymbolList[i].TickValue.LowInDay;

                                if (Business.Market.SymbolList[i].ParameterItems != null)
                                {
                                    int countConfig = Business.Market.SymbolList[i].ParameterItems.Count;
                                    for (int j = 0; j < countConfig; j++)
                                    {
                                        if (Business.Market.SymbolList[i].ParameterItems[j].Code == "S048" ||
                                            Business.Market.SymbolList[i].ParameterItems[j].Code == "S049")
                                        {
                                            message += "{" + Business.Market.SymbolList[i].ParameterItems[j].NumValue;
                                        }
                                    }
                                }

                                result.Add(message);
                            }
                        }
                    }
                    break;
                #endregion

                #region IPAD REQUEST CANDLES BY DATE
                case "IpadRequestCandleByDate":
                    {
                        //IpadRequestCandleByDate$XAUUSD{EURUSD{GPBUSD{.....|month|day|year
                        string[] subParameter = subValue[1].Split('|');
                        DateTime time;
                        List<string> listSymbol = new List<string>();
                        if (subParameter.Length == 4)
                        {
                            int month, day, year;

                            int.TryParse(subParameter[1], out month);
                            int.TryParse(subParameter[2], out day);
                            int.TryParse(subParameter[3], out year);

                            time = new DateTime(year, month, day, 00, 00, 00);

                            string[] subListSymbol = subParameter[0].Split('{');
                            int count = subListSymbol.Length;
                            for (int i = 0; i < count; i++)
                            {
                                if (!string.IsNullOrEmpty(subListSymbol[i]))
                                {
                                    listSymbol.Add(subListSymbol[i]);
                                }
                            }

                            List<string> temp = ProcessQuoteLibrary.Business.QuoteProcess.GetCandlesByDate(listSymbol, time, 0);

                            if (temp != null && temp.Count > 0)
                            {
                                int countTemp = temp.Count;
                                for (int i = 0; i < countTemp; i++)
                                {
                                    if (!string.IsNullOrEmpty(temp[i]))
                                    {
                                        string message = subValue[0] + "$" + temp[i];
                                        result.Add(message);
                                    }
                                }
                            }
                            else
                            {
                                string message = subValue[0] + "$";
                                result.Add(message);
                            }
                        }
                    }
                    break;
                #endregion

                #region GET TIME SERVER STANDART FORMAT
                case "GetTimeServerStandartFormat":
                    {
                        DateTime currentTime = DateTime.Now;
                        string msg = subValue[0] + "$" + currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                        result.Add(msg);
                    }
                    break;
                #endregion
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ReplaceString(string value)
        {
            return value.Replace('_', ' ');
        }
    }
}
