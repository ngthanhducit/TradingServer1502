﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Model
{
    internal class BuildCommand
    {
        private static BuildCommand _buildCommand;
        internal static BuildCommand Instance
        {
            get
            {
                if (BuildCommand._buildCommand == null)
                    BuildCommand._buildCommand = new BuildCommand();

                return BuildCommand._buildCommand;
            }
        }

        /// <summary>
        /// CONVERT STRING TO COMMAND
        /// </summary>
        /// <param name="value">clientcode,investorid,openprice, size,sl, tp, symbol,comment, login key</param>
        /// <returns></returns>
        internal TradingServer.ClientBusiness.Command ConvertStringToCommand(string value)
        {
            TradingServer.ClientBusiness.Command result = new ClientBusiness.Command();
            string[] subValue = value.Split('{');
            if (subValue.Length > 0)
            {
                result.ClientCode = subValue[0];
                result.InvestorID = int.Parse(subValue[1]);
                result.OpenPrice = subValue[2];
                if (subValue[3] == "")
                    result.Size = 0;
                else
                    result.Size = double.Parse(subValue[3]);

                if (subValue[4] == "")
                    result.StopLoss = 0;
                else
                    result.StopLoss = double.Parse(subValue[4]);

                if (subValue[5] == "")
                    result.TakeProfit = 0;
                else
                    result.TakeProfit = double.Parse(subValue[5]);

                result.Symbol = subValue[6];
                result.Comment = subValue[7];
                result.LoginKey = subValue[8];
                //DayƱMonthƱYear
                //TAM THOI DEP BO THOI GIAN HET HAN VI LOI KHI KHONG THE UNESCAPE DUOC QUERYSTRING
                //string[] subDateTime = subValue[9].Split('Ʊ');
                //result.TimeExpiry = new DateTime(int.Parse(subDateTime[2]), int.Parse(subDateTime[1]), 
                //                                    int.Parse(subDateTime[0]), 0, 0, 0);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal TradingServer.ClientBusiness.Command ConvertStringToCommands(string value)
        {
            TradingServer.ClientBusiness.Command result = new ClientBusiness.Command();
            string[] subValue = value.Split('{');
            if (subValue.Length > 0)
            {
                result.CommandID = int.Parse(subValue[0]);
                result.OpenPrice = subValue[1];
                result.StopLoss = double.Parse(subValue[2]);
                result.TakeProfit = double.Parse(subValue[3]);
                result.Comment = subValue[4];                
                result.LoginKey = subValue[5];
                result.InvestorID = int.Parse(subValue[6]);
            }

            return result;
        }

        /// <summary>
        /// CONVERT STRING TO INVESTOR USING LOGIN ACCOUNT
        /// </summary>
        /// <param name="objInvestor">investor account, igroupsecurity,igroupsymbol</param>
        /// <returns></returns>
        internal List<string> ConvertInvestorToString(Business.Investor objInvestor)
        {
            string description = string.Empty;
            string StartDigit = string.Empty;
            string EndDigit = string.Empty;
            List<string> result = new List<string>();
            string command ="InvestorAccount$";
            string commandOne = "ListIGroupSecurity$";
            //string commandTwo = "ListIGroupSymbol$";
            //string commandThree = "IGroupSecurityConfig$";
            //string commandFour = "IGroupSymbolConfig$";
            string commandFive= "ListSymbol$";        
    
            if (objInvestor.InvestorID > 0 && objInvestor != null)
            {
                string message = command + objInvestor.Address + "~" + objInvestor.Balance + "~" + objInvestor.City + "~" + objInvestor.Code + "~" + objInvestor.Country + "~" +
                    objInvestor.Email + "~" + objInvestor.FreeMargin + "~" + objInvestor.NickName + "~" + objInvestor.InvestorID + "~" + objInvestor.IsOnline + "~" +
                    objInvestor.Leverage + "~" + objInvestor.Margin + "~" + objInvestor.MarginLevel + "~" + objInvestor.Phone + "~" + objInvestor.Profit + "~" +
                    objInvestor.State + "~" + DateTime.Now + "~" + objInvestor.ZipCode + "~" + objInvestor.Credit + "~" + objInvestor.IsDisable + "~" + objInvestor.IsReadOnly + "~" +
                    /*objInvestor.UserConfig*/ "" + "~" + objInvestor.FreezeMargin + "~" + objInvestor.InvestorGroupInstance.IsEnable + "~" +
                    objInvestor.InvestorGroupInstance.InvestorGroupID + "~" + objInvestor.InvestorGroupInstance.FreeMargin + "~" + objInvestor.LeverageInvestorGroup + "~" +
                    objInvestor.InvestorGroupInstance.MarginCall + "~" + +objInvestor.InvestorIndex + "~" + objInvestor.ClientConfigInstance.TimeOut + "~" + objInvestor.LoginKey + "~" +
                    objInvestor.ReadOnly + "~" + objInvestor.UserConfigIpad + "~" + objInvestor.UserConfigIphone;

                result.Add(message);

                #region MAP IGROUP SECURITY
                if (objInvestor.ListIGroupSecurity != null)
                {
                    int count = objInvestor.ListIGroupSecurity.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string messageIGroupSecurity = commandOne + objInvestor.ListIGroupSecurity[i].ExecutorMode + "~" + objInvestor.ListIGroupSecurity[i].IGroupSecurityID + "~" +
                            objInvestor.ListIGroupSecurity[i].InvestorGroupID + "~" + objInvestor.ListIGroupSecurity[i].SecurityID;

                        result.Add(messageIGroupSecurity);
                        
                        //if (objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig != null)
                        //{
                        //    int countConfig = objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig.Count;
                        //    for (int j = 0; j < countConfig; j++)
                        //    {
                        //        result.Add(commandThree + objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].ParameterItemID + "{" +
                        //            objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].SecondParameterID + "{" +
                        //            objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].BoolValue + "{" +
                        //            objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].Code + "{" +
                        //            "-1{" + objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].DateValue +
                        //            objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].Name + "{" +
                        //            objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].NumValue + "{" +
                        //            objInvestor.ListIGroupSecurity[i].IGroupSecurityConfig[j].StringValue);
                        //    }
                        //}
                    }
                }
                #endregion

                #region MAP IGROUP SYMBOL
                //if (objInvestor.ListIGroupSymbol != null)
                //{
                //    int count = objInvestor.ListIGroupSymbol.Count;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string messageIGroupSymbol = commandTwo + objInvestor.ListIGroupSymbol[i].IGroupSymbolID + "{" + objInvestor.ListIGroupSymbol[i].InvestorGroupID + "{" +
                //            objInvestor.ListIGroupSymbol[i].SymbolID;

                //        result.Add(messageIGroupSymbol);

                //        if (objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig != null)
                //        {
                //            int countConfig = objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig.Count;
                //            for (int j = 0; j < countConfig; j++)
                //            {
                //                result.Add(commandFour + objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].ParameterItemID + "{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].SecondParameterID + "{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].BoolValue + "{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].Code + "{-1{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].DateValue + "{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].Name + "{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].NumValue + "{" +
                //                    objInvestor.ListIGroupSymbol[i].IGroupSymbolConfig[j].StringValue);
                //            }
                //        }
                //    }
                //}
                #endregion

                #region MAP SYMBOL CLIENT
                if (objInvestor.ListSymbol != null)
                {
                    int count = objInvestor.ListSymbol.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string messageSymbol = commandFive + objInvestor.ListSymbol[i].ContractSize + "~" + objInvestor.ListSymbol[i].Digit + "~" + objInvestor.ListSymbol[i].Name + "~" +
                            objInvestor.ListSymbol[i].SpreadDifference + "~" + objInvestor.ListSymbol[i].IsQuote + "~" + objInvestor.ListSymbol[i].IsTrade + "~" +
                            objInvestor.ListSymbol[i].Currency;
                        
                        if (objInvestor.ListSymbol[i].ParameterItems != null)
                        {
                            int countConfig = objInvestor.ListSymbol[i].ParameterItems.Count;
                            for (int j = 0; j < countConfig; j++)
                            {
                                if (objInvestor.ListSymbol[i].ParameterItems[j].Code == "S004") //Description
                                    //messageSymbol += "~" + objInvestor.ListSymbol[i].ParameterItems[j].StringValue;
                                    description = objInvestor.ListSymbol[i].ParameterItems[j].StringValue;

                                if (objInvestor.ListSymbol[i].ParameterItems[j].Code == "S048") //Forcus Digit
                                    //messageSymbol += "~" + objInvestor.ListSymbol[i].ParameterItems[j].NumValue;
                                    StartDigit = objInvestor.ListSymbol[i].ParameterItems[j].NumValue;

                                if (objInvestor.ListSymbol[i].ParameterItems[j].Code == "S049") //Forcus Digit End
                                    //messageSymbol += "~" + objInvestor.ListSymbol[i].ParameterItems[j].NumValue;
                                    EndDigit = objInvestor.ListSymbol[i].ParameterItems[j].NumValue;
                            }
                        }

                        messageSymbol += "~" + description + "~" + StartDigit + "~" + EndDigit;

                        result.Add(messageSymbol);
                    }
                }
                #endregion
            }

            return result;
        }

        /// <summary>
        /// MAP CLIENTBUSINESS.COMMAND TO STRING
        /// </summary>
        /// <param name="listCommand"></param>
        /// <param name="type">1: Get Online Command | 2: Get Command History | 3: Get Pending Order</param>
        /// <returns></returns>
        internal List<string> ConvertCommandToString(List<TradingServer.ClientBusiness.Command> listCommand,int type)
        {
            List<string> result = new List<string>();
            string commandOne = string.Empty;
            switch (type)
            { 
                case 1:
                    commandOne = "GetOnlineCommandByInvestor$";
                    break;
                case 2:
                    commandOne = "GetCommandHistoryWithTime$";
                    break;
                case 3:
                    commandOne = "GetPendingOrderByInvestorID$";
                    break;
                case 4:
                    commandOne = "GetOpenTradeMultiAccount$";
                    break;
                case 5:
                    commandOne = "GetHistoryWithTimeMultiAccount$";
                    break;
            }

            if (listCommand != null && listCommand.Count > 0)
            {
                int count = listCommand.Count;
                for (int i = 0; i < count; i++)
                {
                    if (type == 4 || type == 5)
                    {
                        int digit = 0;
                        if (type == 5)
                        {
                            TradingServer.Business.Symbol tempSymbol = TradingServer.ClientFacade.FacadeGetSymbolConfing(listCommand[i].Symbol);
                            if (tempSymbol != null)
                            {
                                listCommand[i].Digit = tempSymbol.Digit;
                            }
                        }

                        string message = commandOne + listCommand[i].ClosePrice + "{" + listCommand[i].CommandID + "{" + listCommand[i].CommandType + "{" +
                                            listCommand[i].Commission + "{" + listCommand[i].InvestorID + "{" + listCommand[i].OpenPrice + "{" + listCommand[i].Size + "{" +
                                            listCommand[i].StopLoss + "{" + listCommand[i].Swap + "{" + listCommand[i].Symbol + "{" + listCommand[i].TakeProfit + "{" +
                                            listCommand[i].Time + "{" + listCommand[i].TimeExpiry + "{" + listCommand[i].ClientCode + "{" + listCommand[i].CommandCode + "{" +
                                            listCommand[i].TypeID + "{" + listCommand[i].Margin + "{" + listCommand[i].FreezeMargin + "{" + listCommand[i].IsHedged + "{" +
                                            listCommand[i].Profit + "{" + listCommand[i].Comment + "{" + listCommand[i].IsBuy + "{" + listCommand[i].CloseTime + "{" +
                                            listCommand[i].Digit;

                        result.Add(message);
                    }
                    else
                    {
                        string message = commandOne + listCommand[i].ClosePrice + "{" + listCommand[i].CommandID + "{" + listCommand[i].CommandType + "{" +
                                            listCommand[i].Commission + "{" + listCommand[i].InvestorID + "{" + listCommand[i].OpenPrice + "{" + listCommand[i].Size + "{" +
                                            listCommand[i].StopLoss + "{" + listCommand[i].Swap + "{" + listCommand[i].Symbol + "{" + listCommand[i].TakeProfit + "{" +
                                            listCommand[i].Time + "{" + listCommand[i].TimeExpiry + "{" + listCommand[i].ClientCode + "{" + listCommand[i].CommandCode + "{" +
                                            listCommand[i].TypeID + "{" + listCommand[i].Margin + "{" + listCommand[i].FreezeMargin + "{" + listCommand[i].IsHedged + "{" +
                                            listCommand[i].Profit + "{" + listCommand[i].Comment + "{" + listCommand[i].IsBuy + "{" + listCommand[i].CloseTime;

                        result.Add(message);
                    }
                }
            }
            else
            {
                result.Add(commandOne);
            }

            return result;
        }

        /// <summary>
        /// CONVERT INTERNAL MAIL TO STRING
        /// </summary>
        /// <param name="listInternalMail"></param>
        /// <returns></returns>
        internal List<string> ConvertInternalMailToString(List<TradingServer.Business.InternalMail> listInternalMail)
        {
            List<string> result = new List<string>();
            string command = "GetTopInternalMailToInvestor$";
            if (listInternalMail != null && listInternalMail.Count > 0)
            {
                int count = listInternalMail.Count;
                for (int i = 0; i < count; i++)
                {
                    string message = command + listInternalMail[i].Content + "{" + listInternalMail[i].From + "{" + listInternalMail[i].FromName + "{" +
                        listInternalMail[i].InternalMailID + "{" + listInternalMail[i].IsNew + "{" + listInternalMail[i].Subject + "{" +
                        listInternalMail[i].Time + "{" + listInternalMail[i].To + "{" + listInternalMail[i].ToName;

                    result.Add(message);
                }
            }
            else
            {
                result.Add(command);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMarketConfig"></param>
        /// <returns></returns>
        internal List<string> ConvertMarketConfigToString(List<Business.ParameterItem> listMarketConfig)
        {
            List<string> result = new List<string>();
            string command = "GetMarketConfig$";
            if (listMarketConfig != null && listMarketConfig.Count > 0)
            {
                int count = listMarketConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    if (listMarketConfig[i].Code == "C29")
                    {
                        string message = command + listMarketConfig[i].StringValue;
                        result.Add(message);
                    }
                }
            }
            else
            {
                result.Add(command);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIGroupSecurity"></param>
        /// <returns></returns>
        internal List<string> ConvertIGroupSecurityToString(List<Business.IGroupSecurity> listIGroupSecurity)
        {
            List<string> result = new List<string>();
            string command = "GetIGroupSecurity$";
            if (listIGroupSecurity != null)
            {
                int count = listIGroupSecurity.Count;
                for (int i = 0; i < count; i++)
                {
                    string message = command + listIGroupSecurity[i].ExecutorMode + "{" + listIGroupSecurity[i].IGroupSecurityID + "{" +
                        listIGroupSecurity[i].InvestorGroupID + "{" + listIGroupSecurity[i].SecurityID;

                    result.Add(message);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIGroupSymbol"></param>
        /// <returns></returns>
        internal List<string> ConvertIGroupSymbolToString(List<Business.IGroupSymbol> listIGroupSymbol)
        {
            List<string> result = new List<string>();
            string command = "GetIGroupSymbol$";
            if (listIGroupSymbol != null)
            {
                int count = listIGroupSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    string message = command + listIGroupSymbol[i].IGroupSymbolID + "{" + listIGroupSymbol[i].InvestorGroupID + "{" + listIGroupSymbol[i].SymbolID;

                    result.Add(message);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal List<Business.IGroupSecurity> ConvertStringToIGroupSecurity(string value)
        {
            List<Business.IGroupSecurity> result = new List<Business.IGroupSecurity>();
            string[] subValue = value.Split('|');
            int count = subValue.Length;
            for (int i = 0; i < count; i++)
            {
                int iGroupSecurityID, investorGroupID, securityID;
                string[] subParameter = subValue[i].Split('{');
                int.TryParse(subParameter[0], out iGroupSecurityID);
                int.TryParse(subParameter[1], out investorGroupID);
                int.TryParse(subParameter[2], out securityID);
                Business.IGroupSecurity newIGroupSecurity = new Business.IGroupSecurity();
                newIGroupSecurity.IGroupSecurityID = iGroupSecurityID;
                newIGroupSecurity.InvestorGroupID = investorGroupID;
                newIGroupSecurity.SecurityID = securityID;

                result.Add(newIGroupSecurity);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listSymbol"></param>
        /// <returns></returns>
        internal List<string> ConvertSymbolToString(List<Business.Symbol> listSymbol)
        {
            List<string> result = new List<string>();
            string Description = string.Empty;
            string StartDigit = string.Empty;
            string EndDigit = string.Empty;
            string Hedged = string.Empty;
            string MarginCalculation = string.Empty;
            string StrongHedgedMagrinMode = string.Empty;
            string Excution = string.Empty;
            string Percentage = string.Empty;
            string command = "GetListSymbol$";
            if (listSymbol != null)
            {
                int count = listSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    string messageSymbol = command + listSymbol[i].ContractSize + "{" + listSymbol[i].Digit + "{" + listSymbol[i].FreezeLevel + "{" +
                        listSymbol[i].IsHedged + "{" + listSymbol[i].StopLossTakeProfitLevel + "{" + listSymbol[i].LimitLevel + "{" + listSymbol[i].StopLevel + "{" +
                        listSymbol[i].Name + "{" + listSymbol[i].ProfitCalculation + "{" + listSymbol[i].SpreadDifference + "{" + listSymbol[i].MinLots + "{" +
                        listSymbol[i].MaxLots + "{" + listSymbol[i].StepLots + "{" + listSymbol[i].IsQuote + "{" + listSymbol[i].IsTrade + "{" + listSymbol[i].SecurityID + "{" +
                        listSymbol[i].SpreadBalace + "{" + listSymbol[i].SpreadByDefault + "{" + listSymbol[i].SymbolID + "{" + listSymbol[i].TickPrice + "{" +
                        listSymbol[i].TickSize + "{" + listSymbol[i].Trade + "{" + listSymbol[i].Currency + "{" + listSymbol[i].InitialMargin;                    
                    
                    if (listSymbol[i].ParameterItems != null)
                    {
                        int countConfig = listSymbol[i].ParameterItems.Count;
                        for (int j = 0; j < countConfig; j++)
                        {
                            if (listSymbol[i].ParameterItems[j].Code == "S004") //Description
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].StringValue;
                                Description = listSymbol[i].ParameterItems[j].StringValue;

                            if (listSymbol[i].ParameterItems[j].Code == "S048") //Forcus Digit
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].NumValue;
                                StartDigit = listSymbol[i].ParameterItems[j].NumValue;

                            if (listSymbol[i].ParameterItems[j].Code == "S049") //Forcus Digit End
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].NumValue;
                                EndDigit = listSymbol[i].ParameterItems[j].NumValue;

                            if (listSymbol[i].ParameterItems[j].Code == "S028") //Hedged Value
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].NumValue;
                                Hedged = listSymbol[i].ParameterItems[j].NumValue;

                            if (listSymbol[i].ParameterItems[j].Code == "S032") //Margin Calculation
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].StringValue;
                                MarginCalculation = listSymbol[i].ParameterItems[j].StringValue;

                            if (listSymbol[i].ParameterItems[j].Code == "S034") //Strong Hedged Margin Mode
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].BoolValue;
                                StrongHedgedMagrinMode = listSymbol[i].ParameterItems[j].BoolValue.ToString();

                            if (listSymbol[i].ParameterItems[j].Code == "S006") //Excution
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].StringValue;
                                Excution = listSymbol[i].ParameterItems[j].StringValue;

                            if (listSymbol[i].ParameterItems[j].Code == "S031") //Percentage
                                //messageSymbol += "{" + listSymbol[i].ParameterItems[j].NumValue;
                                Percentage = listSymbol[i].ParameterItems[j].NumValue;
                        }
                    }


                    messageSymbol += "{" + Description + "{" + StartDigit + "{" + EndDigit + "{" + Hedged + "{" + MarginCalculation + "{" + 
                        StrongHedgedMagrinMode + "{" + Excution + "{" + Percentage + "{";

                    messageSymbol += listSymbol[i].IsHoliday;

                    result.Add(messageSymbol);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal string ConvertSymbolToString(Business.Symbol symbol)
        {
            StringBuilder result = new StringBuilder();
            result.Append("GetSymbolConfig$");
            result.Append(symbol.ContractSize + "{");
            result.Append(symbol.Digit + "{");
            result.Append(symbol.FreezeLevel + "{");
            result.Append(symbol.IsHedged + "{");
            result.Append(symbol.StopLossTakeProfitLevel + "{");
            result.Append(symbol.LimitLevel + "{");
            result.Append(symbol.StopLevel + "{");
            result.Append(symbol.Name + "{");
            result.Append(symbol.ProfitCalculation + "{");
            result.Append(symbol.SpreadDifference + "{");
            result.Append(symbol.IsQuote + "{");
            result.Append(symbol.IsTrade + "{");

            result.Append(symbol.SecurityID + "{");
            result.Append(symbol.SpreadBalace + "{");
            result.Append(symbol.SpreadByDefault + "{");
            result.Append(symbol.SymbolID + "{");
            result.Append(symbol.TickPrice + "{");
            result.Append(symbol.TickSize + "{");
            result.Append(symbol.Trade + "{");
            result.Append(symbol.Currency + "{");
            result.Append(symbol.InitialMargin + "{");
            result.Append(symbol.IsEnableFreezeMargin + "{");
            result.Append(symbol.FreezeMarginHedged + "{");
            result.Append(symbol.FreezeMargin + "{");

            if (symbol.ParameterItems != null)
            {
                int count = symbol.ParameterItems.Count;
                for (int i = 0; i < count; i++)
                {
                    #region GET CONFIG DEFAULT OF SYMBOL FOR CLIENT                   
                    if (symbol.ParameterItems[i].Code == "S004")
                    {
                        //SYMBOL DESCRIPTION
                        result.Append(symbol.ParameterItems[i].StringValue + "{");
                        continue;
                    }

                    if (symbol.ParameterItems[i].Code == "S028")
                    {
                        //SYMBOL HEDGED
                        result.Append(symbol.ParameterItems[i].NumValue + "{");
                        continue;
                    }

                    if (symbol.ParameterItems[i].Code == "S031")
                    {
                        //SYMBOL PERCENTAGE
                        result.Append(symbol.ParameterItems[i].NumValue + "{");
                        continue;
                    }

                    if (symbol.ParameterItems[i].Code == "S032")
                    {
                        //SYMBOL MARGIN CALCULATION
                        result.Append(symbol.ParameterItems[i].StringValue + "{");
                        continue;
                    }

                    if (symbol.ParameterItems[i].Code == "S006")
                    {
                        //SYMBOL EXECUTION
                        result.Append(symbol.ParameterItems[i].StringValue + "{");
                        continue;
                    }
                    #endregion
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal List<Business.Tick> ConvertStringToListTick(string value)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";
            //1233.75{1233.25{down{MSLLG{12/05/2013 15:44:57"
            List<Business.Tick> result = new List<Business.Tick>();
            string[] subValue = value.Split('|');
            if (subValue.Length > 0)
            {
                int count = subValue.Length;
                for (int i = 0; i < count; i++)
                {
                    string[] subParameter = subValue[i].Split('{');
                    if (subParameter.Length == 5)
                    {
                        try
                        {
                            Business.Tick newTick = new Business.Tick();
                            newTick.Ask = double.Parse(subParameter[0]);
                            newTick.Bid = double.Parse(subParameter[1]);
                            newTick.Status = subParameter[2];
                            newTick.SymbolName = subParameter[3];
                            newTick.TickTime = DateTime.Parse(subParameter[4]);

                            result.Add(newTick);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal List<Business.Tick> MapStringToTick(string value)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";
            //1233.75{1233.25{down{MSLLG{12/05/2013 15:44:57{High{Low"

            List<Business.Tick> result = new List<Business.Tick>();
            string[] subValue = value.Split('|');
            if (subValue.Length > 0)
            {
                int count = subValue.Length;
                for (int i = 0; i < count; i++)
                {
                    string[] subParameter = subValue[i].Split('{');
                    if (subParameter.Length == 7)
                    {
                        try
                        {
                            Business.Tick newTick = new Business.Tick();
                            newTick.Ask = double.Parse(subParameter[0]);
                            newTick.Bid = double.Parse(subParameter[1]);
                            newTick.Status = subParameter[2];
                            newTick.SymbolName = subParameter[3];
                            newTick.TickTime = DateTime.Parse(subParameter[4]);
                            newTick.HighInDay = double.Parse(subParameter[5]);
                            newTick.LowInDay = double.Parse(subParameter[6]);

                            result.Add(newTick);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investor"></param>
        /// <returns></returns>
        internal string MapInvestorToString(Business.Investor investor)
        {
            StringBuilder result = new StringBuilder();
            
            if (investor != null)
            {
                result.Append("GetInvestorByInvestorID$");
                result.Append(investor.Address);
                result.Append("{");
                result.Append(investor.Balance);
                result.Append("{");
                result.Append(investor.City);
                result.Append("{");
                result.Append(investor.Code);
                result.Append("{");
                result.Append(investor.Country);
                result.Append("{");
                result.Append("USD");
                result.Append("{");
                result.Append(investor.Email);
                result.Append("{");
                result.Append(investor.Equity);
                result.Append("{");
                result.Append(investor.FreeMargin);
                result.Append("{");
                result.Append(investor.NickName);
                result.Append("{");
                result.Append(investor.InvestorID);
                result.Append("{");
                result.Append(investor.IsOnline);
                result.Append("{");
                result.Append(investor.Leverage);
                result.Append("{");
                result.Append(investor.Margin);
                result.Append("{");
                result.Append(investor.MarginLevel);
                result.Append("{");
                result.Append(investor.Phone);
                result.Append("{");
                result.Append(investor.Profit);
                result.Append("{");
                result.Append(investor.State);
                result.Append("{");
                result.Append(DateTime.Now);
                result.Append("{");
                result.Append(investor.ZipCode);
                result.Append("{");
                result.Append(investor.Credit);
                result.Append("{");
                result.Append(investor.FreezeMargin);
                result.Append("{");
                result.Append(investor.ReadOnly);
            }

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investor"></param>
        /// <returns></returns>
        internal string MapInvestorToStringByCode(Business.Investor investor)
        {
            StringBuilder result = new StringBuilder();

            if (investor != null)
            {
                result.Append("GetInvestorByCode$");
                result.Append(investor.Address);
                result.Append("{");
                result.Append(investor.Balance);
                result.Append("{");
                result.Append(investor.City);
                result.Append("{");
                result.Append(investor.Code);
                result.Append("{");
                result.Append(investor.Country);
                result.Append("{");
                result.Append("USD");
                result.Append("{");
                result.Append(investor.Email);
                result.Append("{");
                result.Append(investor.Equity);
                result.Append("{");
                result.Append(investor.FreeMargin);
                result.Append("{");
                result.Append(investor.NickName);
                result.Append("{");
                result.Append(investor.InvestorID);
                result.Append("{");
                result.Append(investor.IsOnline);
                result.Append("{");
                result.Append(investor.Leverage);
                result.Append("{");
                result.Append(investor.Margin);
                result.Append("{");
                result.Append(investor.MarginLevel);
                result.Append("{");
                result.Append(investor.Phone);
                result.Append("{");
                result.Append(investor.Profit);
                result.Append("{");
                result.Append(investor.State);
                result.Append("{");
                result.Append(DateTime.Now);
                result.Append("{");
                result.Append(investor.ZipCode);
                result.Append("{");
                result.Append(investor.Credit);
                result.Append("{");
                result.Append(investor.FreezeMargin);
            }

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listSecurity"></param>
        /// <returns></returns>
        internal string MapListSecurityToString(List<Business.Security> listSecurity)
        {
            string result = "GetSecurity$";
            if (listSecurity != null)
            {
                int count = listSecurity.Count;
                for (int i = 0; i < count; i++)
                {
                    result += listSecurity[i].SecurityID + "{" + listSecurity[i].Name + "{" + listSecurity[i].Description + "[";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal List<string> MapStringToCandles(string value)
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('$');

                if (subValue.Length == 2)
                {
                    string[] subCommand = subValue[1].Split('|');
                    if (subCommand.Length > 0)
                    {
                        int count = subCommand.Length;
                        for (int i = 0; i < count; i++)
                        {
                            string[] subParameter = subCommand[i].Split('}');
                            if (subParameter.Length == 2)
                            {
                                string[] subCandle = subParameter[0].Split('{');
                                StringBuilder candle = new StringBuilder();
                                candle.Append(subCandle[1]);
                                candle.Append("¬");
                                candle.Append(subCandle[3]);
                                candle.Append('¬');
                                candle.Append(subCandle[4]);
                                candle.Append('¬');
                                candle.Append(subCandle[2]);
                                candle.Append('¬');
                                candle.Append(subCandle[6]);
                                candle.Append('¬');
                                candle.Append(subCandle[5]);
                                candle.Append('¬');
                                candle.Append(subCandle[0]);

                                result.Add(candle.ToString());
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
        /// <param name="value"></param>
        /// <returns></returns>
        internal Dictionary<string, string> MapStringToDicCandle(string value)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('$');

                if (subValue.Length == 2)
                {
                    string[] subCommand = subValue[1].Split('|');
                    if (subCommand.Length > 0)
                    {
                        int count = subCommand.Length;
                        for (int i = 0; i < count; i++)
                        {
                            string[] subParameter = subCommand[i].Split('}');
                            if (subParameter.Length == 2)
                            {
                                string[] subCandle = subParameter[0].Split('{');
                                StringBuilder candle = new StringBuilder();
                                candle.Append(subCandle[1]);
                                candle.Append("¬");
                                candle.Append(subCandle[3]);
                                candle.Append('¬');
                                candle.Append(subCandle[4]);
                                candle.Append('¬');
                                candle.Append(subCandle[2]);
                                candle.Append('¬');
                                candle.Append(subCandle[6]);
                                candle.Append('¬');
                                candle.Append(subCandle[5]);
                                candle.Append('¬');
                                candle.Append(subCandle[0]);

                                result.Add(subCandle[0], candle.ToString());
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// GetOnlineCandles, GetLastCandles
        /// EURUSD{1.3962000000000001{1.3952{1.3965000000000001{1.3952{100.{03/13/2014 13:00}
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string MapStringToCandle(string value)
        {
            string result = "";

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('$');

                if (subValue.Length == 2)
                {
                    string[] subParameter = subValue[1].Split('}');
                    string[] subCommand = subParameter[0].Split('{');
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append(subValue[0]);
                    strBuilder.Append('$');
                    strBuilder.Append(subCommand[1]);
                    strBuilder.Append('-');
                    strBuilder.Append(subCommand[3]);
                    strBuilder.Append('-');
                    strBuilder.Append(subCommand[4]);
                    strBuilder.Append('-');
                    strBuilder.Append(subCommand[2]);
                    strBuilder.Append('-');
                    strBuilder.Append(subCommand[6]);
                    strBuilder.Append('-');
                    strBuilder.Append(subCommand[5]);
                    strBuilder.Append('\n');

                    result = strBuilder.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// GetTopCandlesByTimeFrame, GetCandlesByTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string MapStringToStrCandle(string value)
        {
            string result = "";

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('$');

                if (subValue.Length == 2)
                {
                    string[] subParameter = subValue[1].Split('}');
                    if (subParameter.Length > 0)
                    {
                        int count = subParameter.Length;
                        for (int i = 0; i < count; i++)
                        {
                            if (!string.IsNullOrEmpty(subParameter[i]))
                            {
                                string[] subCommand = subParameter[i].Split('{');

                                StringBuilder strBuilder = new StringBuilder();
                                strBuilder.Append(subCommand[1]);
                                strBuilder.Append('-');
                                strBuilder.Append(subCommand[3]);
                                strBuilder.Append('-');
                                strBuilder.Append(subCommand[4]);
                                strBuilder.Append('-');
                                strBuilder.Append(subCommand[2]);
                                strBuilder.Append('-');
                                strBuilder.Append(subCommand[6]);
                                strBuilder.Append('-');
                                strBuilder.Append(subCommand[5]);
                                strBuilder.Append('[');

                                result += strBuilder.ToString();
                            }
                        }
                    }
                }
            }

            

            return result;
        }

        /// <summary>
        /// GetOpenPriceByTimeFrame
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string MapStringToOpenPrice(string value)
        {
            string result = "";

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('$');

                if (subValue.Length == 2)
                {
                    if (!string.IsNullOrEmpty(subValue[1]))
                    {
                        string[] subParameter = subValue[1].Split('}');
                        string[] subCommand = subParameter[0].Split('{');

                        result = subCommand[1];
                    }
                }
            }

            return result;
        }
    }
}