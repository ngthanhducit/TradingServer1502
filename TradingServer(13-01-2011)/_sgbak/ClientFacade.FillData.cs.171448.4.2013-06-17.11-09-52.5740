using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer
{
    public static partial class ClientFacade
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCommand"></param>
        /// <param name="Command"></param>
        private static void FillInstanceOpenTrade(TradingServer.ClientBusiness.Command objCommand, Business.OpenTrade Command)
        {
            #region Find Symbol In Symbol List Command Type,Symbol
            //Find Symbol In Symbol List Command Type
            if (Business.Market.SymbolList != null)
            {
                bool FlagSymbol = false;
                int countSymbol = Business.Market.SymbolList.Count;
                for (int j = 0; j < countSymbol; j++)
                {
                    if (Business.Market.SymbolList[j].Name == objCommand.Symbol)
                    {
                        #region COMMENT CODE 27/07/2011
                        //if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                        //{
                        //    int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                        //    for (int n = 0; n < countType; n++)
                        //    {
                        //        if (Business.Market.SymbolList[j].MarketAreaRef.IMarketAreaName.Trim() == "FutureCommand")
                        //        {
                        //            if (objCommand.CommandType == "7" || objCommand.CommandType == "8" ||
                        //                objCommand.CommandType == "9" || objCommand.CommandType == "10")
                        //            {
                        //                #region SWITHC COMMAND TYPE
                        //                switch (objCommand.CommandType)
                        //                {
                        //                    case "7":
                        //                        {
                        //                            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 19)
                        //                            {
                        //                                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                            }
                        //                        }
                        //                        break;
                        //                    case "8":
                        //                        {
                        //                            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 20)
                        //                            {
                        //                                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                            }
                        //                        }
                        //                        break;
                        //                    case "9":
                        //                        {
                        //                            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 17)
                        //                            {
                        //                                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                            }
                        //                        }
                        //                        break;
                        //                    case "10":
                        //                        {
                        //                            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 18)
                        //                            {
                        //                                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                            }
                        //                        }
                        //                        break;
                        //                }
                        //                #endregion
                        //            }
                        //            else
                        //            {
                        //                #region COMMAND IS BUY SELL FUTURE
                        //                if (objCommand.IsBuy)
                        //                {
                        //                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 11)
                        //                    {
                        //                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                        break;
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 12)
                        //                    {
                        //                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                        break;
                        //                    }
                        //                }
                        //                #endregion
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == int.Parse(objCommand.CommandType))
                        //            {
                        //                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        if (Business.Market.SymbolList[j].MarketAreaRef.IMarketAreaID == 3)
                        {
                            if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                            {
                                int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                                for (int n = 0; n < countType; n++)
                                {
                                    if (objCommand.CommandType == "7" || objCommand.CommandType == "8" ||
                                        objCommand.CommandType == "9" || objCommand.CommandType == "10")
                                    {
                                        #region SWITHC COMMAND TYPE
                                        switch (objCommand.CommandType)
                                        {
                                            case "7":
                                                {
                                                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 19)
                                                    {
                                                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                                    }
                                                }
                                                break;
                                            case "8":
                                                {
                                                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 20)
                                                    {
                                                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                                    }
                                                }
                                                break;
                                            case "9":
                                                {
                                                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 17)
                                                    {
                                                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                                    }
                                                }
                                                break;
                                            case "10":
                                                {
                                                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 18)
                                                    {
                                                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                                    }
                                                }
                                                break;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region COMMAND IS BUY SELL FUTURE
                                        if (objCommand.IsBuy)
                                        {
                                            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 11)
                                            {
                                                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == 12)
                                            {
                                                Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                                break;
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        else if (Business.Market.SymbolList[j].MarketAreaRef.IMarketAreaID == 1)
                        {
                            if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                            {
                                int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                                for (int n = 0; n < countType; n++)
                                {
                                    if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == int.Parse(objCommand.CommandType))
                                    {
                                        Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];

                                        break;
                                    }
                                }
                            }
                        }

                        Command.Symbol = Business.Market.SymbolList[j];
                        FlagSymbol = true;
                        break;
                    }

                    #region COMMAND CODE 27/07/2011
                    //if (FlagSymbol == false)
                    //{
                    //    if (Business.Market.SymbolList[j].RefSymbol != null && Business.Market.SymbolList[j].RefSymbol.Count > 0)
                    //    {
                    //        Command.Symbol = ClientFacade.ClientFindSymbolReference(Business.Market.SymbolList[j].RefSymbol, objCommand.Symbol);

                    //        if (Command.Symbol != null)
                    //        {
                    //            int countType = Command.Symbol.MarketAreaRef.Type.Count;
                    //            for (int k = 0; k < countType; k++)
                    //            {
                    //                if (Command.Symbol.MarketAreaRef.Type[k].ID == int.Parse(objCommand.CommandType))
                    //                {
                    //                    Command.Type = Command.Symbol.MarketAreaRef.Type[k];
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
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
                    if (Business.Market.InvestorList[n].InvestorID == objCommand.InvestorID)
                    {
                        Command.Investor = Business.Market.InvestorList[n];
                        break;
                    }
                }
            }
            #endregion

            #region Fill IGroupSecurity
            if (Command.Investor != null)
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].SecurityID == Command.Symbol.SecurityID &&
                            Business.Market.IGroupSecurityList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            Command.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Find Spread Difference And Add To Property SpreadDifference In Symbol
            double spreadDifference = 0;
            spreadDifference = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(Command.Symbol.SecurityID, Command.Investor.InvestorGroupInstance.InvestorGroupID);
            Command.SpreaDifferenceInOpenTrade = spreadDifference;

            #region COMMAND CODE
            //if (Command.IGroupSecurity != null)
            //{
            //    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
            //    {
            //        int count = Command.IGroupSecurity.IGroupSecurityConfig.Count;
            //        for (int i = 0; i < count; i++)
            //        {
            //            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B04")
            //            {
            //                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out spreadDifference);
            //                Command.Symbol.SpreadDifference = spreadDifference;                            
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCommand"></param>
        /// <param name="Command"></param>
        public static void FillInstanceOpenTrade(Business.RequestDealer RequestObject, Business.OpenTrade Command)
        {
            #region Find Symbol In Symbol List Command Type,Symbol
            //Find Symbol In Symbol List Command Type
            if (Business.Market.SymbolList != null)
            {
                bool FlagSymbol = false;
                int countSymbol = Business.Market.SymbolList.Count;
                for (int j = 0; j < countSymbol; j++)
                {
                    if (Business.Market.SymbolList[j].Name == RequestObject.Request.Symbol.Name)
                    {
                        if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                        {
                            int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                            for (int n = 0; n < countType; n++)
                            {
                                if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == RequestObject.Request.Type.ID)
                                {
                                    Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                    break;
                                }
                            }
                        }

                        Command.Symbol = Business.Market.SymbolList[j];
                        FlagSymbol = true;
                        break;
                    }

                    if (FlagSymbol == false)
                    {
                        if (Business.Market.SymbolList[j].RefSymbol != null && Business.Market.SymbolList[j].RefSymbol.Count > 0)
                        {
                            Command.Symbol = ClientFacade.ClientFindSymbolReference(Business.Market.SymbolList[j].RefSymbol, RequestObject.Request.Symbol.Name);

                            if (Command.Symbol != null)
                            {
                                int countType = Command.Symbol.MarketAreaRef.Type.Count;
                                for (int k = 0; k < countType; k++)
                                {
                                    if (Command.Symbol.MarketAreaRef.Type[k].ID == RequestObject.Request.Type.ID)
                                    {
                                        Command.Type = Command.Symbol.MarketAreaRef.Type[k];
                                        break;
                                    }
                                }
                            }
                        }
                    }
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
                    if (Business.Market.InvestorList[n].InvestorID == RequestObject.Request.Investor.InvestorID)
                    {
                        Command.Investor = Business.Market.InvestorList[n];
                        break;
                    }
                }
            }
            #endregion

            #region Fill IGroupSecurity
            if (Command.Investor != null)
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].SecurityID == Command.Symbol.SecurityID &&
                            Business.Market.IGroupSecurityList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            Command.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Find Spread Difference And Add To Property SpreadDifference In Symbol
            double spreadDifference = 0;
            spreadDifference = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(Command.Symbol.SecurityID, Command.Investor.InvestorGroupInstance.InvestorGroupID);
            Command.SpreaDifferenceInOpenTrade = spreadDifference;
            //if (Command.IGroupSecurity != null)
            //{
            //    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
            //    {
            //        int count = Command.IGroupSecurity.IGroupSecurityConfig.Count;
            //        for (int i = 0; i < count; i++)
            //        {
            //            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B04")
            //            {
            //                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out spreadDifference);
            //                Command.Symbol.SpreadDifference = spreadDifference;                            
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <param name="SymbolName"></param>
        /// <returns></returns>
        internal static Business.Symbol ClientFindSymbolReference(List<Business.Symbol> ListSymbol, string SymbolName)
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
                            ClientFacade.ClientFindSymbolReference(ListSymbol[i].RefSymbol, SymbolName);
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolID"></param>
        /// <param name="investorID"></param>
        public static Business.OpenTrade FillInstanceOpenTrade(int symbolID, int investorID, int commandTypeID)
        {
            Business.OpenTrade result = new Business.OpenTrade();
            //result.Symbol
            #region Find Symbol In Symbol List Command Type,Symbol
            //Find Symbol In Symbol List Command Type
            if (Business.Market.SymbolList != null)
            {
                int countSymbol = Business.Market.SymbolList.Count;
                for (int j = 0; j < countSymbol; j++)
                {
                    if (Business.Market.SymbolList[j].SymbolID == symbolID)
                    {
                        if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                        {
                            int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                            for (int n = 0; n < countType; n++)
                            {
                                if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == commandTypeID)
                                {
                                    result.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                    break;
                                }
                            }
                        }

                        result.Symbol = Business.Market.SymbolList[j];
                        break;
                    }
                }
            }
            #endregion

            //result.Investor
            #region Find Investor List
            //Find Investor List
            if (Business.Market.InvestorList != null)
            {
                int countInvestor = Business.Market.InvestorList.Count;
                for (int n = 0; n < countInvestor; n++)
                {
                    if (Business.Market.InvestorList[n].InvestorID == investorID)
                    {
                        result.Investor = Business.Market.InvestorList[n];
                        break;
                    }
                }
            }
            #endregion

            #region Fill IGroupSecurity
            if (result.Investor != null && result.Symbol != null)
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].SecurityID == result.Symbol.SecurityID &&
                            Business.Market.IGroupSecurityList[i].InvestorGroupID == result.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            result.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                            break;
                        }
                    }
                }
            }
            #endregion

            //BECAUSE WITH TYPE IS 13,14 THEN CAN NOT GET TYPE(BECAUSE SYMBOL ID = 0)
            if (result.Type == null)
            {
                result.Type = new Business.TradeType();
                result.Type.ID = commandTypeID;
                result.Type.Name = TradingServer.Facade.FacadeGetTypeNameByTypeID(commandTypeID);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="investorID"></param>
        /// <param name="commandTypeID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FillInstanceOpenTrade(string symbolName, int investorID, int commandTypeID)
        {
            Business.OpenTrade result = new Business.OpenTrade();
            //result.Symbol
            #region Find Symbol In Symbol List Command Type,Symbol
            //Find Symbol In Symbol List Command Type
            if (Business.Market.SymbolList != null)
            {
                int countSymbol = Business.Market.SymbolList.Count;
                for (int j = 0; j < countSymbol; j++)
                {
                    if (Business.Market.SymbolList[j].Name == symbolName)
                    {
                        if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                        {
                            int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                            for (int n = 0; n < countType; n++)
                            {
                                if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == commandTypeID)
                                {
                                    result.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                    break;
                                }
                            }
                        }

                        result.Symbol = Business.Market.SymbolList[j];
                        break;
                    }
                }
            }
            #endregion

            //result.Investor
            #region Find Investor List
            //Find Investor List
            if (Business.Market.InvestorList != null)
            {
                int countInvestor = Business.Market.InvestorList.Count;
                for (int n = 0; n < countInvestor; n++)
                {
                    if (Business.Market.InvestorList[n].InvestorID == investorID)
                    {
                        result.Investor = Business.Market.InvestorList[n];
                        break;
                    }
                }
            }
            #endregion

            #region Fill IGroupSecurity
            if (result.Investor != null)
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].SecurityID == result.Symbol.SecurityID &&
                            Business.Market.IGroupSecurityList[i].InvestorGroupID == result.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            result.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                            break;
                        }
                    }
                }
            }
            #endregion

            return result;
        }        
    }
}
