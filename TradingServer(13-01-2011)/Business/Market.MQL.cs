using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// 
        /// </summary>
        public static void StartManagerAPI()
        {   
            Business.Market.StartAPI(Business.Market.NotifyInfo, Business.Market.NotifyTick, Business.Market.NotifyLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        internal static void onCallbackInfo(string code)
        {   
            if (code == "NotifyConnectMT4¬")
            {
                //List<string> securities = Business.Market.marketInstance.InitSecurityMT4();
                //if (securities != null)
                //    Business.Market.marketInstance.ReceiveSecurityNotify(securities);

                //List<string> groups = Business.Market.marketInstance.InitGroupMT4();
                //if (groups != null)
                //    Business.Market.marketInstance.ReceiveGroupNotify(groups);

                //List<string> symbols = Business.Market.marketInstance.InitSymbolMT4();
                //if (symbols != null)
                //    Business.Market.marketInstance.ReceiveSymbolNotify(symbols);
            }

            Business.Market.marketInstance.ReceiveNotify(code);
            Debug.WriteLine(code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="upDown"></param>
        /// <param name="time"></param>
        /// <param name="bid"></param>
        /// <param name="ask"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="spread"></param>
        internal static void onCallbackTick(string symbol, int upDown, string time, double bid, double ask, double high, double low, double spread)
        {
            Business.ManagerAPITick _newTick = new ManagerAPITick();
            _newTick.Symbol = symbol;
            _newTick.Ask = ask;
            _newTick.Bid = bid;
            _newTick.High = high;
            _newTick.Low = low;
            _newTick.Spread = spread;
            _newTick.Time = time;
            _newTick.UpDown = upDown;

            Business.Market.marketInstance.ReceiveTickNotify(_newTick);
            Debug.WriteLine(bid);
        }

        internal static void onCallbackLog(string code)
        {
            Debug.WriteLine(code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool RemoveNJ4XTicket(int investorID, string symbol)
        {
            bool result = false;

            string code = string.Empty;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == investorID)
                    {
                        code = Business.Market.InvestorList[i].Code;
                        break;
                    }
                }
            }

            if (Business.Market.NJ4XTickets != null)
            {
                int count = Business.Market.NJ4XTickets.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.NJ4XTickets[i].Code == code && 
                        Business.Market.NJ4XTickets[i].Symbol.Trim().ToUpper() == symbol.Trim().ToUpper())
                    {
                        Business.OpenTrade Command = Business.Market.NJ4XTickets[i].Command;

                        #region INSERT SYSTEM LOG EVENT CLOSE SPOT COMMAND ORDER
                        string mode = TradingServer.Model.TradingCalculate.Instance.ConvertTypeIDToString(Command.Type.ID);
                        string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                        string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
                        string strClosePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), Command.Symbol.Digit);
                        string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                        string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                        string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                        string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                        string contentServer = "'" + Command.Investor.Code + "': close order #" + Command.CommandCode + " (" + mode + " " + size + " " +
                            Command.Symbol.Name + " at " + openPrice + ") at " + strClosePrice + " [Failed - cancel request]";

                        TradingServer.Facade.FacadeAddNewSystemLog(5, contentServer, "  ", Command.Investor.IpAddress, Command.Investor.Code);
                        #endregion

                        lock (Business.Market.nj4xObject)
                        {
                            Business.Market.NJ4XTickets.RemoveAt(i);
                        }
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
        public List<string> InitSecurityMT4()
        {
            List<string> result = new List<string>();
            string cmd = "GetAllSecurity$";
            string strResult = Marshal.PtrToStringAnsi(Business.Market.CommandExecute(cmd));

            if (!string.IsNullOrEmpty(strResult))
            {
                string[] subValue = strResult.Split('$');
                if (subValue.Length == 2)
                {
                    string[] subParameter = subValue[1].Split('}');
                    if (subParameter.Length > 0)
                    {
                        int count = subParameter.Length;
                        for (int i = 0; i < count; i++)
                        {
                            result.Add(subParameter[i]);
                        }
                    }
                }

                if (subValue.Length > 2)
                {
                    string temp = string.Empty;
                    for (int i = 1; i < subValue.Length; i++)
                    {
                        temp += subValue[i];
                    }

                    string[] subParameter = temp.Split('}');
                    if (subParameter.Length > 0)
                    {
                        int count = subParameter.Length;
                        for (int i = 0; i < count; i++)
                        {
                            result.Add(subParameter[i]);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> InitGroupMT4()
        {
            List<string> result = new List<string>();
            string cmd = "GetAllGroup$";
            string strResult = Marshal.PtrToStringAnsi(Business.Market.CommandExecute(cmd));
            if (!string.IsNullOrEmpty(strResult))
            {
                string[] subValue = strResult.Split('$');
                if (subValue.Length > 0)
                {
                    string[] subParameter = subValue[1].Split('}');
                    if (subParameter.Length > 0)
                    {
                        int count = subParameter.Length;
                        for (int i = 0; i < count; i++)
                        {
                            result.Add(subParameter[i]);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> InitSymbolMT4()
        {
            List<string> result = new List<string>();
            string cmd = "GetAllSymbol$";
            string strResult = Marshal.PtrToStringAnsi(Business.Market.CommandExecute(cmd));
            if (!string.IsNullOrEmpty(strResult))
            {
                string[] subValue = strResult.Split('$');
                if (subValue.Length > 0)
                {
                    string strJoin = string.Empty;
                    int count = subValue.Length;
                    for (int i = 1; i < count; i++)
                    {
                        strJoin += subValue[i];
                    }

                    string[] subParameter = strJoin.Split('}');
                    if (subParameter.Length > 0)
                    {
                        int countSys = subParameter.Length;
                        for (int i = 0; i < countSys; i++)
                        {
                            result.Add(subParameter[i]);
                        }
                    }
                }
            }

            return result;
        }
    }
}
