using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace TradingServer.Model
{
    public class CommandFramework 
    {
        public static Model.CommandFramework CommandFrameworkInstance = new CommandFramework();
        
        /// <summary>
        /// EXTRACT STRING TO OPEN TRADE AND FILL SYMBOL,INVESTOR,TYPE,IGROUPSECRUITY INSTANT
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        internal Business.OpenTrade ExtractCommand(string Cmd)
        {
            Business.OpenTrade Result = new Business.OpenTrade();
            string[] subValue = Cmd.Split(',');
            if (subValue.Length > 1)
            {                
                int InvestorID = 0;
                double OpenPrice = 0;
                double Size = 0;
                double StopLoss = 0;                
                double TakeProfit = 0;
                int TypeID = 0;
                string ClientCode = string.Empty;
                string SymbolName = string.Empty;
                double MaxDev=0;

                int.TryParse(subValue[0], out InvestorID);
                double.TryParse(subValue[1], out OpenPrice);
                double.TryParse(subValue[2], out Size);
                double.TryParse(subValue[3], out StopLoss);
                double.TryParse(subValue[4], out TakeProfit);
                int.TryParse(subValue[5], out TypeID);
                ClientCode = subValue[6];
                SymbolName = subValue[7];
                double.TryParse(subValue[8],out MaxDev);

                #region Find Symbol In Symbol List Command Type And Fill Symbol To Command
                if (Business.Market.SymbolList != null)
                {
                    bool FlagSymbol = false;
                    int countSymbol = Business.Market.SymbolList.Count;
                    for (int j = 0; j < countSymbol; j++)
                    {
                        #region Find In Symbol
                        if (Business.Market.SymbolList[j].Name == SymbolName)
                        {
                            if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                            {
                                int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                                for (int n = 0; n < countType; n++)
                                {
                                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == TypeID)
                                    {
                                        Result.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                        break;
                                    }
                                }
                            }

                            Result.Symbol = Business.Market.SymbolList[j];
                            FlagSymbol = true;
                            break;
                        }
                        #endregion                        

                        #region If Don't Find Symbol Then Find In Reference Symbol
                        if (FlagSymbol == false)
                        {
                            if (Business.Market.SymbolList[j].RefSymbol != null && Business.Market.SymbolList[j].RefSymbol.Count > 0)
                            {
                                Result.Symbol = this.ClientFindSymbolReference(Business.Market.SymbolList[j].RefSymbol, SymbolName);

                                if (Result.Symbol != null)
                                {
                                    int countType = Result.Symbol.MarketAreaRef.Type.Count;
                                    for (int k = 0; k < countType; k++)
                                    {
                                        if (Result.Symbol.MarketAreaRef.Type[k].ID == TypeID)
                                        {
                                            Result.Type = Result.Symbol.MarketAreaRef.Type[k];
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region Find Investor List
                //Find Investor List
                if (Business.Market.InvestorList != null)
                {
                    int countInvestor = Business.Market.InvestorList.Count;
                    for (int n = 0; n < countInvestor; n++)
                    {
                        if (Business.Market.InvestorList[n].InvestorID == InvestorID)
                        {
                            Result.Investor = Business.Market.InvestorList[n];
                            break;
                        }
                    }
                }
                #endregion

                #region Fill IGroupSecurity
                if (Result.Investor != null)
                {
                    if (Business.Market.IGroupSecurityList != null)
                    {
                        int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                        for (int i = 0; i < countIGroupSecurity; i++)
                        {
                            if (Business.Market.IGroupSecurityList[i].SecurityID == Result.Symbol.SecurityID &&
                                Business.Market.IGroupSecurityList[i].InvestorGroupID == Result.Investor.InvestorGroupInstance.InvestorGroupID)
                            {
                                Result.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                                break;
                            }
                        }
                    }
                }
                #endregion

                Result.OpenPrice = OpenPrice;
                Result.Size = Size;
                Result.StopLoss = StopLoss;
                Result.TakeProfit = TakeProfit;
                Result.ClientCode = ClientCode;
                Result.IsHedged = Result.Symbol.IsHedged;
                Result.MaxDev = MaxDev;
                Result.OpenTime = DateTime.Now;
            }

            return Result;
        }

        /// <summary>
        /// FIND SYMBOL IN SYMBOL REFERENCE
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="SymbolName"></param>
        /// <returns></returns>
        internal Business.Symbol ClientFindSymbolReference(List<Business.Symbol> ListSymbol, string SymbolName)
        {
            Business.Symbol Result = null;
            if (ListSymbol != null)
            {
                bool Flag = false;
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].Name == SymbolName)
                    {
                        Result = ListSymbol[i];
                        Flag = true;
                        break;
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            this.ClientFindSymbolReference(ListSymbol[i].RefSymbol, SymbolName);
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// EXTRACT OPEN TRADE TO STRING
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal string ExtractCommandToString(int Action, bool Status, string Code, Business.OpenTrade Command)
        {
            string Result = string.Empty;
            string SymbolName = string.Empty;
            int InvestorID = -1;
            int CommandTypeID = -1;
            string ActionCommand = this.ExtractActionCommand(Action);
            if (Command != null)
            {
                if (Command.Symbol != null)
                {
                    SymbolName = Command.Symbol.Name;
                }

                if (Command.Investor != null)
                {
                    InvestorID = Command.Investor.InvestorID;
                }

                if (Command.Type != null)
                {
                    CommandTypeID = Command.Type.ID;
                }

                Result = ActionCommand + "$" + Status + "," + Code + "," + InvestorID + "," + SymbolName + "," + Command.Size + "," + Command.OpenPrice + "," +
                    Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," +
                    Command.ID + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," +
                    Command.Margin;
            }

            return Result;
        }

        /// <summary>
        /// EXTRACT OPEN TRADE TO STRING
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal string ExtractCommandToString(Business.OpenTrade Command)
        {
            string Result = string.Empty;
            int InvestorID = -1;
            string SymbolName = string.Empty;
            int TypeID = -1;
            if (Command != null)
            {
                if (Command.Symbol != null)
                    SymbolName = Command.Symbol.Name;

                if (Command.Investor != null)
                    InvestorID = Command.Investor.InvestorID;

                if (Command.Type != null)
                    TypeID = Command.Type.ID;

                Result = InvestorID + "," + SymbolName + "," + Command.Size + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," + 
                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + Command.ID + "," + Command.ExpTime + "," + 
                    Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin;
            }

            return Result;
        }

        /// <summary>
        /// EXTRACT ACTION TO STRING
        /// </summary>
        /// <param name="ActionID"></param>
        /// <returns></returns>
        private string ExtractActionCommand(int ActionID)
        {
            string Result = string.Empty;
            switch (ActionID)
            {
                case 1:
                    {
                        Result = "MakeCommand";
                    }
                    break;

                case 2:
                    {
                        Result = "CloseCommand";
                    }
                    break;

                case 3:
                    {
                        Result = "UpdateCommand";
                    }
                    break;

                case 4:
                    {
                        Result = "MakeBinaryCommand";
                    }
                    break;

                case 5:
                    {
                        Result = "CancelBinaryCommand";
                    }
                    break;

                case 6:
                    {
                        Result = "UpdatePendingOrder";
                    }
                    break;

                case 7:
                    {
                        Result = "FillPrices";
                    }
                    break;

                case 8:
                    {
                        Result = "CloseBinary";
                    }
                    break;
            }

            return Result;
        }

        /// <summary>
        /// EXTRACT INVESTOR TO STRING
        /// </summary>
        /// <param name="Investor"></param>
        /// <returns></returns>
        internal string ExtractInvestorToString(Business.Investor Investor)
        {
            string Result = string.Empty;
            if (Investor != null)
            {
                Result = Investor.InvestorID + "," + Investor.InvestorStatusID + "," + Investor.InvestorGroupInstance.InvestorGroupID + "," +
                    Investor.AgentID + "," + Investor.Balance + "," + Investor.Credit + "," + Investor.Code + "," + Investor.IsDisable + "," +
                    Investor.TaxRate + "," + Investor.Leverage + "," + Investor.InvestorProfileID + "," + Investor.Address + "," +
                    Investor.Phone + "," + Investor.City + "," + Investor.Country + "," + Investor.Email + "," + Investor.ZipCode + "," +
                    Investor.RegisterDay + "," + Investor.Comment + "," + Investor.State + "," + Investor.NickName;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        internal string ExtractSymbolToString(Business.Symbol Symbol)
        {
            string Result=string.Empty;
            return Result = Symbol.Name + "," + Symbol.ContractSize + "," + Symbol.Currency + "," + Symbol.Digit + "," + Symbol.IsHedged + "," +
                Symbol.LongOnly + "," + Symbol.ProfitCalculation + "," + Symbol.SecurityID + "," + Symbol.SpreadBalace + "," +
                Symbol.SpreadByDefault + "," + Symbol.SpreadDifference + "," + Symbol.TickPrice + "," + Symbol.TickSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSecurity"></param>
        /// <returns></returns>
        internal string ExtractIGroupSecurityToString(Business.IGroupSecurity IGroupSecurity)
        {
            string Message = string.Empty;

            Message = IGroupSecurity.IGroupSecurityID + "," + IGroupSecurity.InvestorGroupID + "," + IGroupSecurity.SecurityID + ",";

            #region GET CONFIG IN IGROUPSECURITY
            if (IGroupSecurity.IGroupSecurityConfig != null && IGroupSecurity.IGroupSecurityConfig.Count > 0)
            {
                int countIGroupSeucurityConfig = IGroupSecurity.IGroupSecurityConfig.Count;
                for (int j = 0; j < countIGroupSeucurityConfig; j++)
                {
                    if (IGroupSecurity.IGroupSecurityConfig[j].Code == "B04")
                    {
                        Message += IGroupSecurity.IGroupSecurityConfig[j].NumValue + ",";
                    }
                }
            }
            #endregion

            return Message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IGroupSymbol"></param>
        /// <returns></returns>
        internal string ExtractIGroupSymbolToString(Business.IGroupSymbol IGroupSymbol)
        {
            string Result = string.Empty;

            Result = IGroupSymbol.IGroupSymbolID + "," + IGroupSymbol.InvestorGroupID + "," + IGroupSymbol.SymbolID + ",";

            #region GET CONFIG IN IGROUPSYMBOL
            if (IGroupSymbol.IGroupSymbolConfig != null && IGroupSymbol.IGroupSymbolConfig.Count > 0)
            {
                int count = IGroupSymbol.IGroupSymbolConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    if (IGroupSymbol.IGroupSymbolConfig[i].Code == "GS01")
                    {
                        Result += IGroupSymbol.IGroupSymbolConfig[i].NumValue + ",";
                    }

                    if (IGroupSymbol.IGroupSymbolConfig[i].Code == "GS02")
                    {
                        Result += IGroupSymbol.IGroupSymbolConfig[i].NumValue + ",";
                    }

                    if (IGroupSymbol.IGroupSymbolConfig[i].Code == "GS03")
                    {
                        Result += IGroupSymbol.IGroupSymbolConfig[i].NumValue + ",";
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// BUILD COMMAND CODE
        /// </summary>
        /// <param name="CommandCode"></param>
        /// <returns></returns>
        internal string BuildCommandCode(string CommandCode)
        {
            string Result = string.Empty;
            while (CommandCode.Length < 8)
            {
                CommandCode = "0" + CommandCode;
            }
            return CommandCode;
        }

        /// <summary>
        /// GET ENCODED STRING
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal string GetEncodedString(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string strReturn = "";
            byte[] hash;
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(input);
            hash = md5.ComputeHash(buffer);

            foreach (byte b in hash)
            {
                strReturn += b.ToString();
            }
            string password = strReturn.Substring(0, 20);
            return password;
        }

        /// <summary>
        /// GET FIRST DAY OF MONT WITH CURRENT DATETIME
        /// </summary>
        /// <param name="TimeDay"></param>
        /// <returns></returns>
        internal DateTime GetFirstDayOfMonth(DateTime TimeDay)
        {
            DateTime TimeMonth = TimeDay;

            TimeMonth = TimeMonth.AddDays(-(TimeDay.Day - 1));

            return TimeMonth;
        }

        /// <summary>
        /// GET FIRST DAY OF MONT WITH MONTH
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        internal DateTime GetFirstDayOfMonth(int Month)
        {
            DateTime TimeMonth = new DateTime(DateTime.Now.Year, Month, 1);

            TimeMonth = TimeMonth.AddDays(-(TimeMonth.Day - 1));

            return TimeMonth;
        }

        /// <summary>
        /// GET LAST DAY OF MONTH WITH CURRENT DATETIME
        /// </summary>
        /// <param name="TimeDay"></param>
        /// <returns></returns>
        internal DateTime GetLastDayOfMonth(DateTime TimeDay)
        {
            DateTime TimeMonth = TimeDay;

            TimeMonth = TimeMonth.AddMonths(1);

            TimeMonth = TimeMonth.AddDays(-(TimeMonth.Day));

            return TimeMonth;
        }

        /// <summary>
        /// GET LAST DAY OF MONTH WITH MONTH
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        internal DateTime GetLastDayOfMonth(int Month)
        {
            DateTime TimeMonth = new DateTime(DateTime.Now.Year, Month, 1);

            TimeMonth = TimeMonth.AddMonths(1);

            TimeMonth = TimeMonth.AddDays(-(TimeMonth.Day));

            return TimeMonth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        internal string BuildStringWithDigit(string value, int digit)
        {
            string result = string.Empty;
            string[] subValue = value.Split('.');
            if (subValue.Length == 2)
            {
                string sub = subValue[1];
                while (sub.Length == digit)
                {
                    sub += "0";
                }
            }
            else if (subValue.Length == 1)
            {

            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        public double GetSpreadDifference(int SecurityID, int InvestorGroupID)
        {
            double Result = 0;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].SecurityID == SecurityID && 
                        Business.Market.IGroupSecurityList[i].InvestorGroupID == InvestorGroupID)
                    {
                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                        {
                            int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == "B04")
                                {
                                    double.TryParse(Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue, out Result);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityID"></param>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public double GetMinLots(int securityID, int investorGroupID)
        {
            double Result = 0;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].SecurityID == securityID &&
                        Business.Market.IGroupSecurityList[i].InvestorGroupID == investorGroupID)
                    {
                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                        {
                            int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == "B11")
                                {
                                    double.TryParse(Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue, out Result);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityID"></param>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public double GetMaxLots(int securityID, int investorGroupID)
        {
            double Result = 0;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].SecurityID == securityID &&
                        Business.Market.IGroupSecurityList[i].InvestorGroupID == investorGroupID)
                    {
                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                        {
                            int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == "B12")
                                {
                                    double.TryParse(Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue, out Result);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityID"></param>
        /// <param name="investorGroupID"></param>
        /// <returns></returns>
        public double GetStepLots(int securityID, int investorGroupID)
        {
            double Result = 0;
            if (Business.Market.IGroupSecurityList != null)
            {
                int count = Business.Market.IGroupSecurityList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.IGroupSecurityList[i].SecurityID == securityID &&
                        Business.Market.IGroupSecurityList[i].InvestorGroupID == investorGroupID)
                    {
                        if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig != null)
                        {
                            int countParameter = Business.Market.IGroupSecurityList[i].IGroupSecurityConfig.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].Code == "B13")
                                {
                                    double.TryParse(Business.Market.IGroupSecurityList[i].IGroupSecurityConfig[j].NumValue, out Result);
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
    }
}
