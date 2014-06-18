using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// 
        /// </summary>
        public void ReceiveNotify(string message)
        {
            if (Business.Market.NotifyMessageFromMT4 == null)
                Business.Market.NotifyMessageFromMT4 = new List<string>();

            Business.Market.NotifyMessageFromMT4.Add(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        internal void ReceiveTickNotify(string message)
        {
            if (Business.Market.NotifyTickFromMT4 == null)
                Business.Market.NotifyTickFromMT4 = new List<string>();

            Business.Market.NotifyTickFromMT4.Add(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        internal void ReceiveTickNotify(Business.ManagerAPITick tick)
        {
            if (Business.Market.NotifyTickFromManagerAPI == null)
                Business.Market.NotifyTickFromManagerAPI = new List<ManagerAPITick>();

            Business.Market.NotifyTickFromManagerAPI.Add(tick);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ReceiveGroupNotify(List<string> value)
        {
            if (value != null)
            {
                Business.Market.InvestorGroupList = new List<InvestorGroup>();
                Business.Market.IGroupSecurityList = new List<IGroupSecurity>();
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(value[i]))
                    {
                        string[] subValue = value[i].Split('{');
                        if (subValue.Length > 0)
                        {
                            Business.InvestorGroup newGroup = new InvestorGroup();
                            newGroup.InvestorGroupID = i;
                            newGroup.Name = subValue[0];//Name
                            //company
                            newGroup.DefautDeposite = double.Parse(subValue[2]);//deposit
                            int _isEnable = int.Parse(subValue[3]);
                            if (_isEnable == 1)
                                newGroup.IsEnable = true;
                            else
                                newGroup.IsEnable = false;

                            // = bool.Parse(subValue[3]);//enable
                            newGroup.MarginCall = double.Parse(subValue[4]);//margincall
                            newGroup.MarginStopOut = double.Parse(subValue[5]);//margin_stopout
                            //default_leverage-G02
                            //interestrate-none
                            //timeout
                            //maxsecurities;
                            //maxpositions
                            //news
                            //support_email-G08
                            //use_swap
                            //hedge_prohibited
                            //close_fifo
                            //close_reopen
                            //archive_period
                            //archive_max_balance
                            //archive_pending_period
                            //margin_mode
                            //credit
                            //stoput_skip_hedged

                            newGroup.ParameterItems = new List<ParameterItem>();

                            ParameterItem newGroupConfig = new ParameterItem();
                            newGroupConfig.Code = "G01";
                            newGroupConfig.BoolValue = 1;
                            newGroupConfig.SecondParameterID = i;
                            newGroupConfig.Name = "Enable";
                            newGroupConfig.StringValue = "NaN";
                            newGroupConfig.NumValue = "NaN";
                            newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                            newGroup.ParameterItems.Add(newGroupConfig);

                            newGroupConfig = new ParameterItem();
                            newGroupConfig.Code = "G02";
                            newGroupConfig.BoolValue = -1;
                            newGroupConfig.SecondParameterID = i;
                            newGroupConfig.Name = "Default Leverag";
                            newGroupConfig.StringValue = "NaN";
                            newGroupConfig.NumValue = subValue[6];
                            newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                            newGroup.ParameterItems.Add(newGroupConfig);

                            newGroupConfig = new ParameterItem();
                            newGroupConfig.Code = "G04";
                            newGroupConfig.BoolValue = -1;
                            newGroupConfig.SecondParameterID = i;
                            newGroupConfig.Name = "TimeOut";
                            newGroupConfig.StringValue = "NaN";
                            newGroupConfig.NumValue = subValue[8];
                            newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                            newGroup.ParameterItems.Add(newGroupConfig);

                            newGroupConfig = new ParameterItem();
                            newGroupConfig.Code = "G19";
                            newGroupConfig.BoolValue = -1;
                            newGroupConfig.SecondParameterID = i;
                            newGroupConfig.Name = "Margin Call";
                            newGroupConfig.StringValue = "NaN";
                            newGroupConfig.NumValue = subValue[4];
                            newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                            newGroup.ParameterItems.Add(newGroupConfig);

                            newGroupConfig = new ParameterItem();
                            newGroupConfig.Code = "G20";
                            newGroupConfig.BoolValue = -1;
                            newGroupConfig.SecondParameterID = i;
                            newGroupConfig.Name = "StopOut";
                            newGroupConfig.StringValue = "NaN";
                            newGroupConfig.NumValue = subValue[5];
                            newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                            newGroup.ParameterItems.Add(newGroupConfig);

                            newGroupConfig = new ParameterItem();
                            newGroupConfig.Code = "G26";
                            newGroupConfig.BoolValue = -1;
                            newGroupConfig.SecondParameterID = i;
                            newGroupConfig.Name = "Margin Mode";
                            EnumMT4.MarginGroupCal enuFreeMargin = (EnumMT4.MarginGroupCal)Enum.Parse(typeof(EnumMT4.MarginGroupCal), subValue[20]);
                            switch (enuFreeMargin)
                            {
                                case EnumMT4.MarginGroupCal.DO_NOT_USE_UNREALIZED_PROFIT_LOSS:
                                    newGroupConfig.StringValue = "do not use unrealized profit/loss";
                                    break;

                                case EnumMT4.MarginGroupCal.USE_UNREALIZED_PROFIT_LOSS:
                                    newGroupConfig.StringValue = "use unrealized profit/loss";
                                    break;

                                case EnumMT4.MarginGroupCal.USE_UNREALIZED_LOSS_ONLY:
                                    newGroupConfig.StringValue = "use unrealized profit only";
                                    break;
                                
                                case EnumMT4.MarginGroupCal.USE_UNREALIZED_PROFIT_ONLY:
                                    newGroupConfig.StringValue = "use unrealized loss only";
                                    break;
                            }

                            newGroupConfig.NumValue = "NaN";
                            newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                            newGroup.ParameterItems.Add(newGroupConfig);

                            newGroup.FreeMargin = newGroupConfig.StringValue;

                            Business.Market.InvestorGroupList.Add(newGroup);

                            #region IGROUPSECURITY
                            //IGROUPSECURITY
                            string strGroupSecurity = subValue[subValue.Length - 1];
                            string[] subGroupSecurity = strGroupSecurity.Split(']');
                            if (subGroupSecurity.Length > 0)
                            {
                                int countGroupSecurity = subGroupSecurity.Length;
                                for (int j = 0; j < countGroupSecurity; j++)
                                {
                                    if (!string.IsNullOrEmpty(subGroupSecurity[j])) 
                                    {
                                        string[] subGroupSec = subGroupSecurity[j].Split('[');

                                        if (int.Parse(subGroupSec[0]) == 1)
                                        {
                                            Business.IGroupSecurity newIGroupSecurity = new IGroupSecurity();
                                            newIGroupSecurity.IGroupSecurityID = j;
                                            newIGroupSecurity.InvestorGroupID = newGroup.InvestorGroupID;
                                            newIGroupSecurity.SecurityID = j;
                                            newIGroupSecurity.IGroupSecurityConfig = new List<ParameterItem>();

                                            Business.ParameterItem newParameterItem = new ParameterItem();
                                            newParameterItem.Code = "B01";
                                            newParameterItem.BoolValue = int.Parse(subGroupSec[2]);
                                            newParameterItem.SecondParameterID = j;
                                            newParameterItem.Name = "Trade";
                                            newParameterItem.StringValue = "NaN";
                                            newParameterItem.NumValue = "NaN";
                                            newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                            newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                            newParameterItem = new ParameterItem();
                                            newParameterItem.Code = "B03";
                                            newParameterItem.BoolValue = -1;
                                            newParameterItem.SecondParameterID = j;
                                            newParameterItem.Name = "Execution";

                                            EnumMT4.IGroupSecurityExecution enuExecution = (EnumMT4.IGroupSecurityExecution)Enum.Parse(typeof(EnumMT4.IGroupSecurityExecution), subGroupSec[4]);
                                            switch (enuExecution)
                                            {
                                                case EnumMT4.IGroupSecurityExecution.AUTOMATIC_ONLY:
                                                    newParameterItem.StringValue = "automatic only";
                                                    break;

                                                case EnumMT4.IGroupSecurityExecution.MANUAL_BUT_AUTOMATIC_IF_NO_DEALERS_ONLINE:
                                                    newParameterItem.StringValue = "manual- but automatic if no dealer online";
                                                    break;

                                                case EnumMT4.IGroupSecurityExecution.MANUAL_ONLY_NO_AUTOMATIC:
                                                    newParameterItem.StringValue = "manual only- no automation";
                                                    break;
                                            }

                                            newParameterItem.NumValue = "NaN";
                                            newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                            newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                            newParameterItem = new ParameterItem();
                                            newParameterItem.Code = "B04";
                                            newParameterItem.BoolValue = -1;
                                            newParameterItem.SecondParameterID = j;
                                            newParameterItem.Name = "Spared Diffirence";
                                            newParameterItem.StringValue = "NaN";
                                            newParameterItem.NumValue = subGroupSec[5];
                                            newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                            newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                            newParameterItem = new ParameterItem();
                                            newParameterItem.Code = "B11";
                                            newParameterItem.BoolValue = -1;
                                            newParameterItem.SecondParameterID = j;
                                            newParameterItem.Name = "Lot Min";
                                            newParameterItem.StringValue = "NaN";
                                            double tempMin = double.Parse(subGroupSec[8]);
                                            tempMin = tempMin / 100;
                                            newParameterItem.NumValue = tempMin.ToString();
                                            newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                            newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                            newParameterItem = new ParameterItem();
                                            newParameterItem.Code = "B12";
                                            newParameterItem.BoolValue = -1;
                                            newParameterItem.SecondParameterID = j;
                                            newParameterItem.Name = "Lot Max";
                                            newParameterItem.StringValue = "NaN";
                                            double tempMax = double.Parse(subGroupSec[9]);
                                            tempMax = tempMax / 100;
                                            newParameterItem.NumValue = tempMax.ToString();
                                            newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                            newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                            newParameterItem = new ParameterItem();
                                            newParameterItem.Code = "B13";
                                            newParameterItem.BoolValue = -1;
                                            newParameterItem.SecondParameterID = j;
                                            newParameterItem.Name = "Step";
                                            newParameterItem.StringValue = "NaN";
                                            double tempStep = double.Parse(subGroupSec[10]);
                                            tempStep = tempStep / 100;
                                            newParameterItem.NumValue = tempStep.ToString();
                                            newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                            newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                            Business.Market.IGroupSecurityList.Add(newIGroupSecurity);
                                            //trade_rights
                                            //trade
                                            //confirmation
                                            //execution
                                            //spread_diff;
                                            //freemargin_mode
                                            //ie_deviation
                                            //lot_min;
                                            //lot_max
                                            //lot_step
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public bool MapGroup(string value)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('{');
                if (subValue.Length > 0)
                {
                    if (Business.Market.InvestorGroupList != null)
                    {
                        int countGroup = Business.Market.InvestorGroupList.Count;
                        for (int j = 0; j < countGroup; j++)
                        {
                            if (Business.Market.InvestorGroupList[j].Name == subValue[0])
                            {
                                #region update group
                                Business.InvestorGroup newGroup = Business.Market.InvestorGroupList[j];
                                newGroup.DefautDeposite = double.Parse(subValue[2]);//deposit
                                int _isEnable = int.Parse(subValue[3]);
                                if (_isEnable == 1)
                                    newGroup.IsEnable = true;
                                else
                                    newGroup.IsEnable = false;

                                // = bool.Parse(subValue[3]);//enable
                                newGroup.MarginCall = double.Parse(subValue[4]);//margincall
                                newGroup.MarginStopOut = double.Parse(subValue[5]);//margin_stopout
                                //default_leverage-G02
                                //interestrate-none
                                //timeout
                                //maxsecurities;
                                //maxpositions
                                //news
                                //support_email-G08
                                //use_swap
                                //hedge_prohibited
                                //close_fifo
                                //close_reopen
                                //archive_period
                                //archive_max_balance
                                //archive_pending_period
                                //margin_mode
                                //credit
                                //stoput_skip_hedged

                                newGroup.ParameterItems = new List<ParameterItem>();

                                ParameterItem newGroupConfig = new ParameterItem();
                                newGroupConfig.Code = "G01";
                                newGroupConfig.SecondParameterID = newGroup.InvestorGroupID;
                                newGroupConfig.BoolValue = 1;
                                newGroupConfig.Name = "Enable";
                                newGroupConfig.StringValue = "NaN";
                                newGroupConfig.NumValue = "NaN";
                                newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newGroup.ParameterItems.Add(newGroupConfig);

                                newGroupConfig = new ParameterItem();
                                newGroupConfig.Code = "G02";
                                newGroupConfig.SecondParameterID = newGroup.InvestorGroupID;
                                newGroupConfig.BoolValue = -1;
                                newGroupConfig.Name = "Default Leverag";
                                newGroupConfig.StringValue = "NaN";
                                newGroupConfig.NumValue = subValue[6];
                                newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newGroup.ParameterItems.Add(newGroupConfig);

                                newGroupConfig = new ParameterItem();
                                newGroupConfig.Code = "G04";
                                newGroupConfig.SecondParameterID = newGroup.InvestorGroupID;
                                newGroupConfig.BoolValue = -1;
                                newGroupConfig.Name = "TimeOut";
                                newGroupConfig.StringValue = "NaN";
                                newGroupConfig.NumValue = subValue[8];
                                newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newGroup.ParameterItems.Add(newGroupConfig);

                                newGroupConfig = new ParameterItem();
                                newGroupConfig.Code = "G19";
                                newGroupConfig.SecondParameterID = newGroup.InvestorGroupID;
                                newGroupConfig.BoolValue = -1;
                                newGroupConfig.Name = "Margin Call";
                                newGroupConfig.StringValue = "NaN";
                                newGroupConfig.NumValue = subValue[4];
                                newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newGroup.ParameterItems.Add(newGroupConfig);

                                newGroupConfig = new ParameterItem();
                                newGroupConfig.Code = "G20";
                                newGroupConfig.SecondParameterID = newGroup.InvestorGroupID;
                                newGroupConfig.BoolValue = -1;
                                newGroupConfig.Name = "StopOut";
                                newGroupConfig.StringValue = "NaN";
                                newGroupConfig.NumValue = subValue[5];
                                newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newGroup.ParameterItems.Add(newGroupConfig);

                                newGroupConfig = new ParameterItem();
                                newGroupConfig.Code = "G26";
                                newGroupConfig.SecondParameterID = newGroup.InvestorGroupID;
                                newGroupConfig.BoolValue = -1;
                                newGroupConfig.Name = "Margin Mode";
                                EnumMT4.MarginGroupCal enuFreeMargin = (EnumMT4.MarginGroupCal)Enum.Parse(typeof(EnumMT4.MarginGroupCal), subValue[20]);
                                switch (enuFreeMargin)
                                {
                                    case EnumMT4.MarginGroupCal.DO_NOT_USE_UNREALIZED_PROFIT_LOSS:
                                        newGroupConfig.StringValue = "do not use unrealized profit/loss";
                                        break;

                                    case EnumMT4.MarginGroupCal.USE_UNREALIZED_PROFIT_LOSS:
                                        newGroupConfig.StringValue = "use unrealized profit/loss";
                                        break;

                                    case EnumMT4.MarginGroupCal.USE_UNREALIZED_LOSS_ONLY:
                                        newGroupConfig.StringValue = "use unrealized profit only";
                                        break;

                                    case EnumMT4.MarginGroupCal.USE_UNREALIZED_PROFIT_ONLY:
                                        newGroupConfig.StringValue = "use unrealized loss only";
                                        break;
                                }

                                newGroupConfig.NumValue = "NaN";
                                newGroupConfig.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newGroup.ParameterItems.Add(newGroupConfig);

                                newGroup.FreeMargin = newGroupConfig.StringValue;
                                #endregion

                                //remove igroupsecurity
                                if (Business.Market.IGroupSecurityList != null)
                                {
                                    for (int n = 0; n < Business.Market.IGroupSecurityList.Count; n++)
                                    {
                                        if (Business.Market.IGroupSecurityList[n].InvestorGroupID == newGroup.InvestorGroupID)
                                        {
                                            Business.Market.IGroupSecurityList.RemoveAt(n);
                                            n--;
                                        }
                                    }
                                }

                                #region IGROUPSECURITY
                                //IGROUPSECURITY
                                string strGroupSecurity = subValue[subValue.Length - 1];
                                string[] subGroupSecurity = strGroupSecurity.Split(']');
                                if (subGroupSecurity.Length > 0)
                                {
                                    int countGroupSecurity = subGroupSecurity.Length;
                                    for (int n = 0; n < countGroupSecurity; n++)
                                    {
                                        if (!string.IsNullOrEmpty(subGroupSecurity[n]))
                                        {
                                            string[] subGroupSec = subGroupSecurity[n].Split('[');

                                            if (subGroupSec.Length <= 1)
                                                continue;

                                            if (int.Parse(subGroupSec[0]) == 1)
                                            {
                                                Business.IGroupSecurity newIGroupSecurity = new IGroupSecurity();
                                                newIGroupSecurity.IGroupSecurityID = n;
                                                newIGroupSecurity.InvestorGroupID = newGroup.InvestorGroupID;
                                                newIGroupSecurity.SecurityID = n;
                                                newIGroupSecurity.IGroupSecurityConfig = new List<ParameterItem>();

                                                Business.ParameterItem newParameterItem = new ParameterItem();
                                                newParameterItem.Code = "B01";
                                                newParameterItem.BoolValue = int.Parse(subGroupSec[2]);
                                                newParameterItem.SecondParameterID = n;
                                                newParameterItem.Name = "Trade";
                                                newParameterItem.StringValue = "NaN";
                                                newParameterItem.NumValue = "NaN";
                                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                                newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                                newParameterItem = new ParameterItem();
                                                newParameterItem.Code = "B03";
                                                newParameterItem.BoolValue = -1;
                                                newParameterItem.SecondParameterID = n;
                                                newParameterItem.Name = "Execution";

                                                EnumMT4.IGroupSecurityExecution enuExecution = (EnumMT4.IGroupSecurityExecution)Enum.Parse(typeof(EnumMT4.IGroupSecurityExecution), subGroupSec[4]);
                                                switch (enuExecution)
                                                {
                                                    case EnumMT4.IGroupSecurityExecution.AUTOMATIC_ONLY:
                                                        newParameterItem.StringValue = "automatic only";
                                                        break;

                                                    case EnumMT4.IGroupSecurityExecution.MANUAL_BUT_AUTOMATIC_IF_NO_DEALERS_ONLINE:
                                                        newParameterItem.StringValue = "manual- but automatic if no dealer online";
                                                        break;

                                                    case EnumMT4.IGroupSecurityExecution.MANUAL_ONLY_NO_AUTOMATIC:
                                                        newParameterItem.StringValue = "manual only- no automation";
                                                        break;
                                                }

                                                newParameterItem.NumValue = "NaN";
                                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                                newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                                newParameterItem = new ParameterItem();
                                                newParameterItem.Code = "B04";
                                                newParameterItem.BoolValue = -1;
                                                newParameterItem.SecondParameterID = n;
                                                newParameterItem.Name = "Spared Diffirence";
                                                newParameterItem.StringValue = "NaN";
                                                newParameterItem.NumValue = subGroupSec[5];
                                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                                newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                                newParameterItem = new ParameterItem();
                                                newParameterItem.Code = "B11";
                                                newParameterItem.BoolValue = -1;
                                                newParameterItem.SecondParameterID = n;
                                                newParameterItem.Name = "Lot Min";
                                                newParameterItem.StringValue = "NaN";
                                                double tempMin = double.Parse(subGroupSec[8]);
                                                tempMin = tempMin / 100;
                                                newParameterItem.NumValue = tempMin.ToString();
                                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                                newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                                newParameterItem = new ParameterItem();
                                                newParameterItem.Code = "B12";
                                                newParameterItem.BoolValue = -1;
                                                newParameterItem.SecondParameterID = n;
                                                newParameterItem.Name = "Lot Max";
                                                newParameterItem.StringValue = "NaN";
                                                double tempMax = double.Parse(subGroupSec[9]);
                                                tempMax = tempMax / 100;
                                                newParameterItem.NumValue = tempMax.ToString();
                                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                                newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                                newParameterItem = new ParameterItem();
                                                newParameterItem.Code = "B13";
                                                newParameterItem.BoolValue = -1;
                                                newParameterItem.SecondParameterID = n;
                                                newParameterItem.Name = "Step";
                                                newParameterItem.StringValue = "NaN";
                                                double tempStep = double.Parse(subGroupSec[10]);
                                                tempStep = tempStep / 100;
                                                newParameterItem.NumValue = tempStep.ToString();
                                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                                newIGroupSecurity.IGroupSecurityConfig.Add(newParameterItem);

                                                Business.Market.IGroupSecurityList.Add(newIGroupSecurity);
                                                //trade_rights
                                                //trade
                                                //confirmation
                                                //execution
                                                //spread_diff;
                                                //freemargin_mode
                                                //ie_deviation
                                                //lot_min;
                                                //lot_max
                                                //lot_step
                                            }
                                        }
                                    }
                                }
                                #endregion

                                result = true;
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
        /// <param name="value"></param>
        public void ReceiveSecurityNotify(List<string> value)
        {
            if (value != null)
            {
                Business.Market.SecurityList = new List<Security>();
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(value[i]))
                    {
                        string[] subValue = value[i].Split('{');
                        Business.Security newSecurity = new Security();
                        newSecurity.SecurityID = int.Parse(subValue[0]);
                        newSecurity.Name = subValue[1];

                        if (string.IsNullOrEmpty(subValue[2]))
                            newSecurity.Description = "MT4 Security: " + newSecurity.Name;
                        else
                            newSecurity.Description = subValue[2];

                        Business.Market.SecurityList.Add(newSecurity);
                    }
                }
            }
        }

        /// <summary>
        /// GetAllSymbol$symbol{source{digits{description{typeSecurity{tradeMode{margin_currency{instant_max_volume{
        /// gtc_pendings{spread{long_only{stops_level{spread_balance{freeze_level{contract_size{margin_initial{
        /// margin_maintenance{margin_hedged{tick_size{tick_value{margin_mode{profit_mode{margin_hedged_strong{realtime{
        /// starting{expiration{open_hour[open_min[close_hour[close_min]|open_hour[open_min[close_hour[close_min]^
        /// open_hour[open_min[close_hour[close_min]|open_hour[open_min[close_hour[close_min]^
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ReceiveSymbolNotify(List<string> value)
        {
            if (value != null)
            {
                Business.Market.SymbolList = new List<Symbol>();
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(value[i]))
                    {
                        string[] subValue = value[i].Split('{');
                        Business.Symbol newSymbol = new Symbol();
                        newSymbol.MarketAreaRef = Business.Market.MarketArea[0];
                        if (Business.Market.SecurityList != null)
                        {
                            int countSecurity = Business.Market.SecurityList.Count;
                            for (int j = 0; j < countSecurity; j++)
                            {
                                if (Business.Market.SecurityList[j].SecurityID == int.Parse(subValue[4]))
                                {
                                    newSymbol.SecurityID = Business.Market.SecurityList[j].SecurityID;
                                    break;
                                }
                            }
                        }

                        newSymbol.Name = subValue[0];
                        newSymbol.IsQuote = true;
                        newSymbol.IsTrade = true;
                        newSymbol.TimeOnHold = 500000;
                        //source
                        newSymbol.Digit = int.Parse(subValue[2]);
                        //description
                        //type
                        //execution_mode
                        EnumMT4.Execution enuExecutionTrade = (EnumMT4.Execution)(Enum.Parse(typeof(EnumMT4.Execution), subValue[5]));
                        newSymbol.ExecutionTrade = enuExecutionTrade;

                        newSymbol.Currency = subValue[6];

                        EnumMT4.Trade enuTrade = (EnumMT4.Trade)(Enum.Parse(typeof(EnumMT4.Trade), subValue[7]));
                        switch (enuTrade)
                        {
                            case EnumMT4.Trade.CLOSE_ONLY:
                                newSymbol.Trade = "Close Only";
                                break;

                            case EnumMT4.Trade.FULL_ACCESS:
                                newSymbol.Trade = "Full Access";
                                break;

                            case EnumMT4.Trade.NO:
                                newSymbol.Trade = "No";
                                break;
                        }

                        //newSymbol.Trade = subValue[7];
                        //margin_currency
                        //instant_max_volume
                        //gtc_pendings
                        newSymbol.SpreadByDefault = double.Parse(subValue[11]);
                        int _isLongOnly = int.Parse(subValue[12]);
                        if (_isLongOnly == 1)
                            newSymbol.LongOnly = true;
                        else
                            newSymbol.LongOnly = false;

                        newSymbol.StopLevel = int.Parse(subValue[13]);
                        newSymbol.SpreadBalace = double.Parse(subValue[14]) - 1;
                        newSymbol.FreezeLevel = int.Parse(subValue[15]);
                        newSymbol.ContractSize = double.Parse(subValue[16]);
                        newSymbol.InitialMargin = double.Parse(subValue[17]);
                        //margin_maintenance
                        newSymbol.MarginHedged = double.Parse(subValue[19]);
                        newSymbol.TickSize = double.Parse(subValue[20]);
                        newSymbol.TickPrice = double.Parse(subValue[21]);
                        //tick_value
                        //margin_mode

                        EnumMT4.ProfitCal enuProfitCal = (EnumMT4.ProfitCal)(Enum.Parse(typeof(EnumMT4.ProfitCal), subValue[23]));
                        switch (enuProfitCal)
                        {
                            case EnumMT4.ProfitCal.CFD:
                                newSymbol.ProfitCalculation = "CFD [ (close_price - open_price) * contract_size * lots ]";
                                break;

                            case EnumMT4.ProfitCal.FOREX:
                                newSymbol.ProfitCalculation = "Forex [ (close_price - open_price) * contract_size * lots ]";
                                break;

                            case EnumMT4.ProfitCal.FUTURES:
                                newSymbol.ProfitCalculation = "Futures [ (close_price - open_price) * tick_price / tick_size * lots ]";
                                break;
                        }
                        //margin_hedged_strong
                        //realtime
                        //starting
                        //expiration

                        newSymbol.TickValue = new Tick();
                        newSymbol.TickValue.Bid = 0;
                        newSymbol.TickValue.Ask = 0;
                        newSymbol.TickValue.SymbolName = newSymbol.Name;
                        newSymbol.TickValue.Status = "down";

                        newSymbol.ParameterItems = new List<ParameterItem>();

                        Business.ParameterItem newParameterItem = new ParameterItem();
                        newParameterItem.Code = "S004";
                        newParameterItem.BoolValue = -1;
                        newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                        newParameterItem.Name = "Description";
                        newParameterItem.NumValue = "-1";
                        newParameterItem.SecondParameterID = i;
                        newParameterItem.StringValue = subValue[3];
                        newSymbol.ParameterItems.Add(newParameterItem);

                        newParameterItem = new ParameterItem();
                        newParameterItem.Code = "S006";
                        newParameterItem.BoolValue = -1;
                        newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                        newParameterItem.Name = "Trade Execution";
                        newParameterItem.NumValue = subValue[5];
                        newParameterItem.SecondParameterID = i;
                        newParameterItem.StringValue = "NaN";
                        newSymbol.ParameterItems.Add(newParameterItem);

                        newParameterItem = new ParameterItem();
                        newParameterItem.Code = "S048";
                        newParameterItem.BoolValue = -1;
                        newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                        newParameterItem.Name = "Start Digit";
                        newParameterItem.NumValue = "0";
                        newParameterItem.SecondParameterID = i;
                        newParameterItem.StringValue = "NaN";
                        newSymbol.ParameterItems.Add(newParameterItem);

                        newParameterItem = new ParameterItem();
                        newParameterItem.Code = "S049";
                        newParameterItem.BoolValue = -1;
                        newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                        newParameterItem.Name = "End Digit";
                        newParameterItem.NumValue = "0";
                        newParameterItem.SecondParameterID = i;
                        newParameterItem.StringValue = "NaN";
                        newSymbol.ParameterItems.Add(newParameterItem);

                        newParameterItem = new ParameterItem();
                        newParameterItem.Code = "S054";
                        newParameterItem.BoolValue = -1;
                        newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                        newParameterItem.Name = "Description";
                        newParameterItem.NumValue = "500000";
                        newParameterItem.SecondParameterID = i;
                        newParameterItem.StringValue = "NaN";
                        newSymbol.ParameterItems.Add(newParameterItem);

                        Business.Market.SymbolList.Add(newSymbol);

                        if (Business.Market.Symbols.ContainsKey(newSymbol.Name))
                            Business.Market.Symbols[newSymbol.Name] = newSymbol;
                        else
                            Business.Market.Symbols.Add(newSymbol.Name, newSymbol);

                        #region SESSION
                        string strSession = subValue[subValue.Length - 1];
                        string[] subSession = strSession.Split('^');
                        if (subSession.Length > 0)
                        {
                            int countSession = subSession.Length;
                            for (int j = 0; j < countSession; j++)
                            {
                                if (!string.IsNullOrEmpty(subSession[j]))
                                {
                                    string[] subQuoteTrade = subSession[j].Split('|');
                                    if (subQuoteTrade.Length > 0)
                                    {
                                        string[] subQuote = subQuoteTrade[0].Split(']');
                                        if (subQuote.Length > 0)
                                        {
                                            int countSubQuote = subQuote.Length;
                                            for (int n = 0; n < countSubQuote; n++)
                                            {
                                                if (!string.IsNullOrEmpty(subQuote[n]))
                                                {
                                                    string[] subQuotePara = subQuote[n].Split('[');
                                                    //OPEN_HOUR
                                                    //OPEN_MIN
                                                    //CLOSE_HOUR
                                                    //CLOSE_MIN
                                                }
                                            }
                                        }

                                        string[] subTrade = subQuoteTrade[1].Split(']');
                                        if (subTrade.Length > 0)
                                        {
                                            int countSubTrade = subTrade.Length;
                                            for (int n = 0; n < countSubTrade; n++)
                                            {
                                                if (!string.IsNullOrEmpty(subTrade[n]))
                                                {
                                                    string[] subTradePara = subTrade[n].Split('[');
                                                    //OPEN_HOUR
                                                    //OPEN_MIN
                                                    //CLOSE_HOUR
                                                    //CLOSE_MIN
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            if (Business.Market.IsFirstStart)
            {
                Business.Market.IsProcessNotifyTick = true;
                Business.Market.ThreadProcessNotifyTickMT4 = new System.Threading.Thread(new System.Threading.ThreadStart(ProcessNotifyTickMT4));
                Business.Market.ThreadProcessNotifyTickMT4.Start();

                Business.Market.IsFirstStart = false;
            }
            
            //this.LoopGetCandlesManagerAPI();

            Business.Market.IsOpen = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool MapSymbol(string value)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('{');
                if (subValue.Length > 0)
                {
                    if (Business.Market.SymbolList != null)
                    {
                        int count = Business.Market.SymbolList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.SymbolList[i].Name == subValue[0])
                            {
                                #region update symbol
                                Business.Symbol newSymbol = Business.Market.SymbolList[i];
                                newSymbol.MarketAreaRef = Business.Market.MarketArea[0];
                                if (Business.Market.SecurityList != null)
                                {
                                    int countSecurity = Business.Market.SecurityList.Count;
                                    for (int j = 0; j < countSecurity; j++)
                                    {
                                        if (Business.Market.SecurityList[j].SecurityID == int.Parse(subValue[4]))
                                        {
                                            newSymbol.SecurityID = Business.Market.SecurityList[j].SecurityID;
                                            break;
                                        }
                                    }
                                }

                                newSymbol.Name = subValue[0];
                                newSymbol.IsQuote = true;
                                newSymbol.IsTrade = true;
                                newSymbol.TimeOnHold = 500000;
                                //source
                                newSymbol.Digit = int.Parse(subValue[2]);
                                //description
                                //type
                                //execution_mode
                                EnumMT4.Execution enuExecutionTrade = (EnumMT4.Execution)(Enum.Parse(typeof(EnumMT4.Execution), subValue[5]));
                                newSymbol.ExecutionTrade = enuExecutionTrade;

                                newSymbol.Currency = subValue[6];

                                EnumMT4.Trade enuTrade = (EnumMT4.Trade)(Enum.Parse(typeof(EnumMT4.Trade), subValue[7]));
                                switch (enuTrade)
                                {
                                    case EnumMT4.Trade.CLOSE_ONLY:
                                        newSymbol.Trade = "Close Only";
                                        break;

                                    case EnumMT4.Trade.FULL_ACCESS:
                                        newSymbol.Trade = "Full Access";
                                        break;

                                    case EnumMT4.Trade.NO:
                                        newSymbol.Trade = "No";
                                        break;
                                }

                                //newSymbol.Trade = subValue[7];
                                //margin_currency
                                //instant_max_volume
                                //gtc_pendings
                                newSymbol.SpreadByDefault = double.Parse(subValue[11]);
                                int _isLongOnly = int.Parse(subValue[12]);
                                if (_isLongOnly == 1)
                                    newSymbol.LongOnly = true;
                                else
                                    newSymbol.LongOnly = false;

                                newSymbol.StopLevel = int.Parse(subValue[13]);
                                newSymbol.SpreadBalace = double.Parse(subValue[14]) - 1;
                                newSymbol.FreezeLevel = int.Parse(subValue[15]);
                                newSymbol.ContractSize = double.Parse(subValue[16]);
                                newSymbol.InitialMargin = double.Parse(subValue[17]);
                                //margin_maintenance
                                newSymbol.MarginHedged = double.Parse(subValue[19]);
                                newSymbol.TickSize = double.Parse(subValue[20]);
                                newSymbol.TickPrice = double.Parse(subValue[21]);
                                //tick_value
                                //margin_mode

                                EnumMT4.ProfitCal enuProfitCal = (EnumMT4.ProfitCal)(Enum.Parse(typeof(EnumMT4.ProfitCal), subValue[23]));
                                switch (enuProfitCal)
                                {
                                    case EnumMT4.ProfitCal.CFD:
                                        newSymbol.ProfitCalculation = "CFD [ (close_price - open_price) * contract_size * lots ]";
                                        break;

                                    case EnumMT4.ProfitCal.FOREX:
                                        newSymbol.ProfitCalculation = "Forex [ (close_price - open_price) * contract_size * lots ]";
                                        break;

                                    case EnumMT4.ProfitCal.FUTURES:
                                        newSymbol.ProfitCalculation = "Futures [ (close_price - open_price) * tick_price / tick_size * lots ]";
                                        break;
                                }
                                //margin_hedged_strong
                                //realtime
                                //starting
                                //expiration

                                newSymbol.TickValue = new Tick();
                                newSymbol.TickValue.Bid = 0;
                                newSymbol.TickValue.Ask = 0;
                                newSymbol.TickValue.SymbolName = newSymbol.Name;
                                newSymbol.TickValue.Status = "down";

                                newSymbol.ParameterItems = new List<ParameterItem>();

                                Business.ParameterItem newParameterItem = new ParameterItem();
                                newParameterItem.Code = "S004";
                                newParameterItem.BoolValue = -1;
                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newParameterItem.Name = "Description";
                                newParameterItem.NumValue = "-1";
                                newParameterItem.SecondParameterID = newSymbol.SymbolID;
                                newParameterItem.StringValue = subValue[3];
                                newSymbol.ParameterItems.Add(newParameterItem);

                                newParameterItem = new ParameterItem();
                                newParameterItem.Code = "S006";
                                newParameterItem.BoolValue = -1;
                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newParameterItem.Name = "Trade Execution";
                                newParameterItem.NumValue = subValue[5];
                                newParameterItem.SecondParameterID = newSymbol.SymbolID;
                                newParameterItem.StringValue = "NaN";
                                newSymbol.ParameterItems.Add(newParameterItem);

                                newParameterItem = new ParameterItem();
                                newParameterItem.Code = "S048";
                                newParameterItem.BoolValue = -1;
                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newParameterItem.Name = "Start Digit";
                                newParameterItem.NumValue = "0";
                                newParameterItem.SecondParameterID = newSymbol.SymbolID;
                                newParameterItem.StringValue = "NaN";
                                newSymbol.ParameterItems.Add(newParameterItem);

                                newParameterItem = new ParameterItem();
                                newParameterItem.Code = "S049";
                                newParameterItem.BoolValue = -1;
                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newParameterItem.Name = "End Digit";
                                newParameterItem.NumValue = "0";
                                newParameterItem.SecondParameterID = newSymbol.SymbolID;
                                newParameterItem.StringValue = "NaN";
                                newSymbol.ParameterItems.Add(newParameterItem);

                                newParameterItem = new ParameterItem();
                                newParameterItem.Code = "S054";
                                newParameterItem.BoolValue = -1;
                                newParameterItem.DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                                newParameterItem.Name = "Description";
                                newParameterItem.NumValue = "500000";
                                newParameterItem.SecondParameterID = newSymbol.SymbolID;
                                newParameterItem.StringValue = "NaN";
                                newSymbol.ParameterItems.Add(newParameterItem);

                                if (Business.Market.Symbols.ContainsKey(newSymbol.Name))
                                    Business.Market.Symbols[newSymbol.Name] = newSymbol;
                                else
                                    Business.Market.Symbols.Add(newSymbol.Name, newSymbol);

                                result = true;
                                break;
                                #endregion
                            }
                        }
                    }
                }
            }

            return result;
        }

        //MakeCommandNotify$CommandID{InvestorCode{OpenPrice{Size{StopLoss{TakeProfit{Symbol{CommandType{Commission{Swap{OpenTime{TimeExpire{Comment
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static TradingServer.Business.OpenTrade MapNotifyMakeCommand(string value)
        {
            TradingServer.Business.OpenTrade result = new TradingServer.Business.OpenTrade();
            if (value != null)
            {
                string[] subValue = value.Split('$');
                if (subValue.Count() == 2)
                {
                    string[] subParameter = subValue[1].Split('{');
                    if (subParameter.Count() > 0)
                    {
                        result.RefCommandID = int.Parse(subParameter[0]);
                        result.CommandCode = subParameter[0];

                        #region FILL SYMBOL
                        if (Business.Market.Symbols != null)
                        {
                            bool isExits = Business.Market.Symbols.ContainsKey(subParameter[6]);
                            if (isExits)
                                result.Symbol = Business.Market.Symbols[subParameter[6]];
                        }
                        #endregion

                        #region FILL INVESTOR
                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].Code.ToUpper().Trim() == subParameter[1].ToUpper().Trim())
                                {
                                    result.Investor = Business.Market.InvestorList[i];

                                    #region GET IGROUP SECURITY
                                    if (Business.Market.IGroupSecurityList != null)
                                    {
                                        int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                        for (int n = 0; n < countIGroupSecurity; n++)
                                        {
                                            if (Business.Market.IGroupSecurityList[n].SecurityID == result.Symbol.SecurityID &&
                                                Business.Market.IGroupSecurityList[n].InvestorGroupID == result.Investor.InvestorGroupInstance.InvestorGroupID)
                                            {
                                                result.IGroupSecurity = Business.Market.IGroupSecurityList[n];

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

                        result.OpenPrice = double.Parse(subParameter[2]);
                        result.Size = double.Parse(subParameter[3]) / 100;
                        result.StopLoss = double.Parse(subParameter[4]);
                        result.TakeProfit = double.Parse(subParameter[5]);

                        #region FILL COMMAND TYPE
                        switch (int.Parse(subParameter[7]))
                        {
                            case 0: 
                                {   
                                    #region SEARCH COMMAND TYPE
                                    bool flag = false;
                                    result.Type = new TradeType();
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int count = Business.Market.MarketArea.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.MarketArea[i].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[i].Type.Count;
                                                for (int j = 0; j < countType; j++)
                                                {
                                                    if (Business.Market.MarketArea[i].Type[j].ID == 1)
                                                    {
                                                        result.Type = Business.Market.MarketArea[i].Type[j];
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (flag)
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                                break;

                            case 1:
                                {
                                    #region SEARCH COMMAND TYPE
                                    bool flag = false;
                                    result.Type = new TradeType();
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int count = Business.Market.MarketArea.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.MarketArea[i].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[i].Type.Count;
                                                for (int j = 0; j < countType; j++)
                                                {
                                                    if (Business.Market.MarketArea[i].Type[j].ID == 2)
                                                    {
                                                        result.Type = Business.Market.MarketArea[i].Type[j];
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (flag)
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                                break;

                            case 2:
                                {
                                    #region SEARCH COMMAND TYPE
                                    bool flag = false;
                                    result.Type = new TradeType();
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int count = Business.Market.MarketArea.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.MarketArea[i].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[i].Type.Count;
                                                for (int j = 0; j < countType; j++)
                                                {
                                                    if (Business.Market.MarketArea[i].Type[j].ID == 7)
                                                    {
                                                        result.Type = Business.Market.MarketArea[i].Type[j];
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (flag)
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                                break;

                            case 3:
                                {
                                    #region SEARCH COMMAND TYPE
                                    bool flag = false;
                                    result.Type = new TradeType();
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int count = Business.Market.MarketArea.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.MarketArea[i].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[i].Type.Count;
                                                for (int j = 0; j < countType; j++)
                                                {
                                                    if (Business.Market.MarketArea[i].Type[j].ID == 8)
                                                    {
                                                        result.Type = Business.Market.MarketArea[i].Type[j];
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (flag)
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                                break;

                            case 4:
                                {
                                    #region SEARCH COMMAND TYPE
                                    bool flag = false;
                                    result.Type = new TradeType();
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int count = Business.Market.MarketArea.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.MarketArea[i].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[i].Type.Count;
                                                for (int j = 0; j < countType; j++)
                                                {
                                                    if (Business.Market.MarketArea[i].Type[j].ID == 9)
                                                    {
                                                        result.Type = Business.Market.MarketArea[i].Type[j];
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (flag)
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                                break;

                            case 5:
                                {
                                    #region SEARCH COMMAND TYPE
                                    bool flag = false;
                                    result.Type = new TradeType();
                                    if (Business.Market.MarketArea != null)
                                    {
                                        int count = Business.Market.MarketArea.Count;
                                        for (int i = 0; i < count; i++)
                                        {
                                            if (Business.Market.MarketArea[i].Type != null)
                                            {
                                                int countType = Business.Market.MarketArea[i].Type.Count;
                                                for (int j = 0; j < countType; j++)
                                                {
                                                    if (Business.Market.MarketArea[i].Type[j].ID == 10)
                                                    {
                                                        result.Type = Business.Market.MarketArea[i].Type[j];
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (flag)
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                                break;
                        }
                        #endregion

                        result.Commission = double.Parse(subParameter[8]);
                        result.Swap = double.Parse(subParameter[9]);
                        result.Profit = double.Parse(subParameter[10]);

                        result.OpenTime = DateTime.Parse(subParameter[11]);
                        result.ExpTime = DateTime.Parse(subParameter[12]);

                        result.Comment = subParameter[13];
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        internal Business.TradeType GetTradeType(int typeID)
        {
            Business.TradeType result = new TradeType();
            if (Business.Market.MarketArea != null)
            {
                bool flag = false;
                int count = Business.Market.MarketArea.Count;
                for (int i = 0; i < count; i++)
                {
                    if (flag)
                        break;

                    if (Business.Market.MarketArea[i].Type != null)
                    {
                        int countType = Business.Market.MarketArea[i].Type.Count;
                        for (int j = 0; j < countType; j++)
                        {
                            if (Business.Market.MarketArea[i].Type[j].ID == typeID)
                            {
                                result = Business.Market.MarketArea[i].Type[j];
                                flag = true;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// NotifyUpdateCommand$CommandID{InvestorCode{OpenPrice{StopLoss{TakeProfit{Swap{Comment
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static Business.OpenTrade MapNotifyUpdateCommand(string value)
        {
            Business.OpenTrade result = new OpenTrade();
            
            if (value != null)
            {
                string[] subValue = value.Split('$');
                if (subValue.Count() == 2)
                {
                    string[] subParameter = subValue[1].Split('{');
                    if (subParameter.Count() == 13)
                    {
                        result.RefCommandID = int.Parse(subParameter[0]);

                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if(Business.Market.InvestorList[i].Code.ToUpper().Trim() == subParameter[1].ToUpper().Trim())
                                {
                                    result.Investor = Business.Market.InvestorList[i];
                                    break;
                                }
                            }
                        }
                        
                        result.OpenPrice = double.Parse(subParameter[2]);
                        result.StopLoss = double.Parse(subParameter[3]);
                        result.TakeProfit = double.Parse(subParameter[4]);

                        #region GET COMMAND TYPE
                        switch (int.Parse(subParameter[5]))
                        {
                            case 0:
                                {
                                    Business.TradeType typeCommand = Business.Market.marketInstance.GetTradeType(1);
                                    result.Type = typeCommand;
                                }
                                break;

                            case 1:
                                {
                                    Business.TradeType typeCommand = Business.Market.marketInstance.GetTradeType(2);
                                    result.Type = typeCommand;
                                }
                                break;

                            case 2:
                                {
                                    Business.TradeType typeCommand = Business.Market.marketInstance.GetTradeType(7);
                                    result.Type = typeCommand;
                                }
                                break;
                            case 3:
                                {
                                    Business.TradeType typeCommand = Business.Market.marketInstance.GetTradeType(8);
                                    result.Type = typeCommand;
                                }
                                break;

                            case 4:
                                {
                                    Business.TradeType typeCommand = Business.Market.marketInstance.GetTradeType(9);
                                    result.Type = typeCommand;
                                }
                                break;

                            case 5:
                                {
                                    Business.TradeType typeCommand = Business.Market.marketInstance.GetTradeType(10);
                                    result.Type = typeCommand;
                                }
                                break;
                        }
                        #endregion

                        result.Swap = double.Parse(subParameter[5]);
                        
                        result.Commission = double.Parse(subParameter[6]);
                        result.OpenTime = DateTime.Parse(subParameter[7]);
                        result.ExpTime = DateTime.Parse(subParameter[8]);
                        result.Swap = double.Parse(subParameter[9]);
                        
                        if (Business.Market.Symbols.ContainsKey(subParameter[10]))
                        {
                            result.Symbol = Business.Market.Symbols[subParameter[10]];
                        }

                        result.Size = double.Parse(subParameter[11]) / 100;

                        result.Comment = subParameter[12];
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
        internal static Business.OpenTrade MapNotifyCloseCommand(string value)
        {
            Business.OpenTrade result = new OpenTrade();
            //NotifyCloseCommand$CommandID{ClosePrice{Size{Profit{Swap{Commission
            if (value != null)
            {
                string[] subValue = value.Split('$');
                if (subValue.Count() == 2)
                {
                    string[] subParameter = subValue[1].Split('{');
                    if (subParameter.Count() == 7)
                    {
                        result.RefCommandID = int.Parse(subParameter[0]);
                        result.ClosePrice = double.Parse(subParameter[1]);
                        result.Size = double.Parse(subParameter[2]) / 100;
                        result.Profit = double.Parse(subParameter[3]);
                        result.Swap = double.Parse(subParameter[4]);
                        result.Commission = double.Parse(subParameter[5]);
                        result.CloseTime = DateTime.Parse(subParameter[6]);
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
        internal static int MapNotifyDeleteCommand(string value)
        {
            int result = -1;
            //NotifyDeleteCommand$CommandID
            if (value != null)
            {
                string[] subValue = value.Split('$');
                if (subValue.Count() == 2)
                {
                    result = int.Parse(subValue[1]);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static TradingServer.Business.Investor MapNotifyUpdateAccount(string value)
        {
            //NotifyUpdateAccount$InvestorCode{GroupName{AgentAccount{Balance{Credit{IsDisable{TaxRate{Leverage{Address{Phone{City{Country{Email{ZipCode{RegisterDay{Comment{State{Name(NickName){AllowChangePassword{ReadOnly{SendReport
            TradingServer.Business.Investor result = new TradingServer.Business.Investor();

            if (value != null)
            {
                string[] subValue = value.Split('$');
                if (subValue.Count() == 2)
                {
                    string[] subParameter = subValue[1].Split('{');
                    if (subParameter.Count() == 21)
                    {
                        result.Code = subParameter[0];

                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].Code.ToUpper().Trim() == subParameter[0].ToUpper().Trim())
                                {
                                    result = Business.Market.InvestorList[i];
                                    break;
                                }
                            }
                        }

                        result.AgentID = subParameter[2];
                        
                        result.Balance = double.Parse(subParameter[3]);
                        result.Credit = double.Parse(subParameter[4]);

                        if (int.Parse(subParameter[5]) == 1)
                            result.IsDisable = false;
                        else 
                            result.IsDisable = true;

                        result.TaxRate = double.Parse(subParameter[6]);
                        result.Leverage = int.Parse(subParameter[7]);
                        result.Address = subParameter[8];
                        result.Phone = subParameter[9];
                        result.City = subParameter[10];
                        result.Country = subParameter[11];
                        result.Email = subParameter[12];
                        result.ZipCode = subParameter[13];
                        result.RegisterDay = DateTime.Parse(subParameter[14]);
                        result.Comment = subParameter[15];
                        result.State = subParameter[16];
                        result.NickName = subParameter[17];

                        if (int.Parse(subParameter[18]) == 1)
                            result.AllowChangePwd = true;
                        else
                            result.AllowChangePwd = false;

                        if (int.Parse(subParameter[19]) == 1)
                            result.ReadOnly = true;
                        else 
                            result.ReadOnly = false;

                        if (!string.IsNullOrEmpty(subParameter[20]))
                        {
                            if (int.Parse(subParameter[20]) == 1)
                                result.SendReport = true;
                            else
                                result.SendReport = false;
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
        internal static Business.Investor MapNotifyUpdateInfoAccount(string value)
        {
            Business.Investor result = new Investor();

            if (!string.IsNullOrEmpty(value))
            {
                string[] subValue = value.Split('$');
                if (subValue.Length == 2)
                {
                    string[] subParameter = subValue[1].Split('{');
                    if (subParameter.Length == 7)
                    {
                        if (Business.Market.InvestorList != null)
                        {
                            int count = Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Business.Market.InvestorList[i].Code.ToUpper().Trim() == subParameter[0].ToUpper().Trim())
                                {
                                    result = Business.Market.InvestorList[i];
                                    break;
                                }
                            }
                        }

                        result.Credit = double.Parse(subParameter[6]);
                        result.Balance = double.Parse(subParameter[1]) - result.Credit;
                        result.Equity = double.Parse(subParameter[2]);
                        result.Margin = double.Parse(subParameter[3]);
                        result.FreeMargin = double.Parse(subParameter[4]);
                        result.MarginLevel = double.Parse(subParameter[5]);
                    }
                }
            }

            return result;
        }
    }
}
