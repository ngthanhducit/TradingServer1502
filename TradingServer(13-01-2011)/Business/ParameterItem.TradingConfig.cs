using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class ParameterItem
    {        
        /// <summary>
        /// GET ALL TRADING CONFIG IN DATABASE
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllTradingConfig()
        {
            return ParameterItem.DBWTradingConfigInstance.GetAllParameterItem();
        }
                
        /// <summary>
        /// ADD NEW SYMBOL CONFIG
        /// </summary>
        /// <param name="ListParameterItem"></param>
        internal int AddNewSymbolConfig(List<Business.ParameterItem> ListParameterItem)
        {
            int Result = -1;

            //Add Symbol Config To Database
            int countSymbol = Business.Market.SymbolList.Count;
            int countParameterItem = ListParameterItem.Count;
            bool Flag = false;
            for (int i = 0; i < countSymbol; i++)
            {
                if (Business.Market.SymbolList[i].SymbolID == ListParameterItem[0].SecondParameterID)
                {
                    if (Business.Market.SymbolList[i].ParameterItems == null)
                        Business.Market.SymbolList[i].ParameterItems = new List<ParameterItem>();

                    for (int j = 0; j < countParameterItem; j++)
                    {
                        Result = DBWTradingConfigInstance.AddNewTradingConfig(ListParameterItem[j].SecondParameterID, -1, ListParameterItem[j].Name, ListParameterItem[j].Code,
                                      ListParameterItem[j].BoolValue, ListParameterItem[j].StringValue, ListParameterItem[j].NumValue, ListParameterItem[j].DateValue);

                        ListParameterItem[j].ParameterItemID = Result;
                        if (Result > 0)
                        {
                            Business.Market.SymbolList[i].ParameterItems.Add(ListParameterItem[j]);
                        }

                        #region Add Value Default Of Symbol
                        //Add Value Default Of Symbol

                        #region comment code
                        #region ADD VALUE TO PROPERTY SPREAD DIFFIRENCE IN CLASS SYMBOL
                        //if (Business.Market.IGroupSecurityList != null)
                        //{
                        //    double SpreadDiffirence = 0;
                        //    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                        //    for (int n = 0; n < countIGroupSecurity; n++)
                        //    {
                        //        if (Business.Market.IGroupSecurityList[n].SecurityID == Business.Market.SymbolList[i].SecurityID)
                        //        {
                        //            if (Business.Market.IGroupSecurityList[n].IGroupSecurityConfig != null)
                        //            {
                        //                int countIGroupSecurityConfig = Business.Market.IGroupSecurityList[n].IGroupSecurityConfig.Count;
                        //                for (int m = 0; m < countIGroupSecurityConfig; m++)
                        //                {
                        //                    if (Business.Market.IGroupSecurityList[n].IGroupSecurityConfig[m].Code == "B04")
                        //                    {
                        //                        SpreadDiffirence = double.Parse(Business.Market.IGroupSecurityList[n].IGroupSecurityConfig[m].NumValue);
                        //                        Market.SymbolList[i].SpreadDifference = SpreadDiffirence;
                        //                        break;
                        //                    }
                        //                }
                        //            }
                        //            break;
                        //        }
                        //    }
                        //}
                        #endregion

                        #endregion

                        #region Add Value To Property Contract Size In Class Symbol
                        if (ListParameterItem[j].Code == "S025")
                        {
                            double ContractSize = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out ContractSize);
                            Market.SymbolList[i].ContractSize = ContractSize;
                        }
                        #endregion

                        #region Add Value To Property Tick Price In Class Symbol
                        if (ListParameterItem[j].Code == "S030")
                        {
                            double TickPrice = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out TickPrice);
                            Market.SymbolList[i].TickPrice = TickPrice;
                        }
                        #endregion

                        #region Add Value To Property Tick Size In Class Symbol
                        if (ListParameterItem[j].Code == "S029")
                        {
                            double TickSize = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out TickSize);
                            Market.SymbolList[i].TickSize = TickSize;
                        }
                        #endregion

                        #region Add Value To Property Currency In Class Symbol
                        if (ListParameterItem[j].Code == "S007")
                        {
                            Market.SymbolList[i].Currency = ListParameterItem[j].StringValue;
                        }
                        #endregion

                        #region Add Value To Property Profit Calculation In CLass Symbol
                        if (ListParameterItem[j].Code == "S033")
                        {
                            Market.SymbolList[i].ProfitCalculation = ListParameterItem[j].StringValue;
                        }
                        #endregion

                        #region Add Value To Property Digit In Class Symbol
                        if (ListParameterItem[j].Code == "S003")
                        {
                            int Digit = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out Digit);
                            Market.SymbolList[i].Digit = Digit;
                        }
                        #endregion

                        #region Add Value To Property Spread By Default In Class Symbol
                        if (ListParameterItem[j].Code == "S013")
                        {
                            double SpreadByDefault = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out SpreadByDefault);
                            Market.SymbolList[i].SpreadByDefault = SpreadByDefault;
                        }
                        #endregion

                        #region Add Value To Property Spread Balance In Class Symbol
                        if (ListParameterItem[j].Code == "S016")
                        {
                            double SpreadBalance = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out SpreadBalance);
                            Market.SymbolList[i].SpreadBalace = SpreadBalance;
                        }
                        #endregion

                        #region Add Value To Property Long Only In Class Symbol
                        if (ListParameterItem[j].Code == "S014")
                        {
                            bool LongOnly = false;
                            if (ListParameterItem[j].BoolValue == 1)
                            {
                                LongOnly = true;
                            }
                            Market.SymbolList[i].LongOnly = LongOnly;
                        }
                        #endregion

                        #region Add Value To Property Limit Stop Level In Class Symbol
                        if (ListParameterItem[j].Code == "S015")
                        {
                            int LimitLevel = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out LimitLevel);
                            Market.SymbolList[i].LimitLevel = LimitLevel;
                        }
                        if (ListParameterItem[j].Code == "S046")
                        {
                            int StopLevel = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out StopLevel);
                            Market.SymbolList[i].StopLevel = StopLevel;
                        }
                        if (ListParameterItem[j].Code == "S047")
                        {
                            int SLTP = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out SLTP);
                            Market.SymbolList[i].StopLossTakeProfitLevel = SLTP;
                        }
                        #endregion

                        #region Add Value To Property Freeze Level In Class Symbol
                        if (ListParameterItem[j].Code == "S017")
                        {
                            int FreezeLevel = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out FreezeLevel);
                            Market.SymbolList[i].FreezeLevel = FreezeLevel;
                        }
                        #endregion

                        #region Add Value To Property Allow Read Time In Class Symbol
                        if (ListParameterItem[j].Code == "S018")
                        {
                            bool AllowReadTime = false;
                            if (ListParameterItem[j].BoolValue == 1)
                            {
                                AllowReadTime = true;
                            }

                            Market.SymbolList[i].AllowReadTime = AllowReadTime;
                        }
                        #endregion

                        #region Add Value To Property Filter In Class Symbol
                        if (ListParameterItem[j].Code == "S022")
                        {
                            int Filter = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out Filter);
                            Market.SymbolList[i].Filter = Filter;
                        }
                        #endregion

                        #region Add Value To Property Filtration Level In Class Symbol
                        if (ListParameterItem[j].Code == "S020")
                        {
                            int FiltrationsLevel = 0;
                            int.TryParse(ListParameterItem[j].NumValue, out FiltrationsLevel);
                            Market.SymbolList[i].FiltrationsLevel = FiltrationsLevel;
                        }
                        #endregion

                        #region Add Value To Property Auto Limit In Class Symbol
                        if (ListParameterItem[j].Code == "S021")
                        {
                            Market.SymbolList[i].AutoLimit = ListParameterItem[j].StringValue;
                        }
                        #endregion

                        #region Add Value To Property Is Hedged In Class Symbol
                        if (ListParameterItem[j].Code == "S034")
                        {
                            bool IsHedged = false;
                            if (ListParameterItem[j].BoolValue == 1)
                                IsHedged = true;
                            Market.SymbolList[i].IsHedged = IsHedged;
                        }
                        #endregion

                        #region Add Value To Property Trade In Class Symbol
                        if (ListParameterItem[j].Code == "S008")
                        {
                            Market.SymbolList[i].Trade = ListParameterItem[j].StringValue;
                        }
                        #endregion

                        #region Add Value To Property Initial Margin In Class Symbll
                        if (ListParameterItem[j].Code == "S026")
                        {
                            double InitialMargin = -1;
                            double.TryParse(ListParameterItem[j].NumValue, out InitialMargin);
                            Market.SymbolList[i].InitialMargin = InitialMargin;
                        }
                        #endregion

                        #region Add Value To Event Of Symbol
                        //if (ListParameterItem[j].Code == "S042")
                        //{
                        //    TradingServer.Facade.FacadeAddSymbolEvent(ListParameterItem[j].StringValue, Business.Market.SymbolList[i].Name, ListParameterItem[j].Code, ListParameterItem[j].ParameterItemID);
                        //}

                        //if (ListParameterItem[j].Code == "S043")
                        //{
                        //    TradingServer.Facade.FacadeAddSymbolEvent(ListParameterItem[j].StringValue, Business.Market.SymbolList[i].Name, ListParameterItem[j].Code, ListParameterItem[j].ParameterItemID);
                        //}
                        #endregion

                        #region ADD VALUE APPLY SPREAD TO NEW SYMBOL
                        if (ListParameterItem[j].Code == "S050")
                        {
                            bool applySpread = false;
                            if (ListParameterItem[j].BoolValue == 1)
                            {
                                applySpread = true;
                            }

                            TradingServer.Business.Market.SymbolList[i].ApplySpread = applySpread;
                        }
                        #endregion

                        #region ADD VALUE USE FREEZE MARGIN
                        if (ListParameterItem[j].Code == "S051")
                        {
                            bool useFreezeMargin = false;
                            if (ListParameterItem[j].BoolValue == 1)
                                useFreezeMargin = true;

                            TradingServer.Business.Market.SymbolList[i].IsEnableFreezeMargin = useFreezeMargin;
                        }
                        #endregion

                        #region ADD VALUE FREEZE MARGIN
                        if (ListParameterItem[j].Code == "S052")
                        {
                            double freezeMargin = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out freezeMargin);
                            TradingServer.Business.Market.SymbolList[i].FreezeMargin = freezeMargin;
                        }
                        #endregion

                        #region ADD VALUE FREEZE MARGIN HEDGED
                        if (ListParameterItem[j].Code == "S053")
                        {
                            double freezeMarginH = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out freezeMarginH);
                            TradingServer.Business.Market.SymbolList[i].FreezeMarginHedged = freezeMarginH;
                        }
                        #endregion

                        #region ADD VALUE MARGIN HEDGED
                        if (ListParameterItem[j].Code == "S028")
                        {
                            double marginH = 0;
                            double.TryParse(ListParameterItem[j].NumValue, out marginH);
                            TradingServer.Business.Market.SymbolList[i].MarginHedged = marginH;
                        }
                        #endregion

                        #endregion
                    }
                    Flag = true;
                    break;
                }

                if (Flag == false)
                {
                    if (Business.Market.SymbolList[i].RefSymbol != null && Business.Market.SymbolList[i].RefSymbol.Count > 0)
                    {
                        this.AddNewSymbolConfigReference(ListParameterItem, Market.SymbolList[i].RefSymbol);
                    }
                }
            }

            //Init Symbol Event
            Business.Market.marketInstance.InitTimeEventInSymbol();

            //Init Time Event
            Business.Market.marketInstance.InitSymbolFuture();

            return Result;
        }

        /// <summary>
        /// ADD NEW SYMBOL CONFIG REFERENCE
        /// </summary>
        /// <param name="ListParameterItem"></param>
        internal void AddNewSymbolConfigReference(List<Business.ParameterItem> ListParameterItem, List<Business.Symbol> ListSymbol)
        {
            return;
            if (ListSymbol != null)
            {
                bool Flag = false;
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].SymbolID == ListParameterItem[0].SecondParameterID)
                    {
                        int coutParameter = ListParameterItem.Count;
                        for (int j = 0; j < coutParameter; j++)
                        {
                            int Result = DBWIGroupSymbolConfig.AddIGroupSymbolConfig(ListParameterItem[j].SecondParameterID, -1, ListParameterItem[j].Name,
                                ListParameterItem[j].Code, ListParameterItem[j].BoolValue, ListParameterItem[j].StringValue, ListParameterItem[j].NumValue, ListParameterItem[j].DateValue);

                            ListParameterItem[j].ParameterItemID = Result;

                            #region Add Value Default Of Symbol
                            //Add Value Default Of Symbol

                            #region ADD VALUE TO PROPERTY SPREAD DIFFIRENCE IN CLASS SYMBOL
                            if (Business.Market.IGroupSecurityList != null)
                            {
                                double SpreadDiffirence = 0;
                                int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                                for (int n = 0; n < countIGroupSecurity; n++)
                                {
                                    if (Business.Market.IGroupSecurityList[n].SecurityID == ListSymbol[i].SecurityID)
                                    {
                                        if (Business.Market.IGroupSecurityList[n].IGroupSecurityConfig != null)
                                        {
                                            int countIGroupSecurityConfig = Business.Market.IGroupSecurityList[n].IGroupSecurityConfig.Count;
                                            for (int m = 0; m < countIGroupSecurityConfig; m++)
                                            {
                                                if (Business.Market.IGroupSecurityList[n].IGroupSecurityConfig[m].Code == "B04")
                                                {
                                                    SpreadDiffirence = double.Parse(Business.Market.IGroupSecurityList[n].IGroupSecurityConfig[m].NumValue);
                                                    ListSymbol[i].SpreadDifference = SpreadDiffirence;
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region Add Value To Property Contract Size In Class Symbol
                            if (ListParameterItem[j].Code == "S025")
                            {
                                double ContractSize = 0;
                                double.TryParse(ListParameterItem[j].NumValue, out ContractSize);
                                ListSymbol[i].ContractSize = ContractSize;
                            }
                            #endregion

                            #region Add Value To Property Tick Price In Class Symbol
                            if (ListParameterItem[j].Code == "S030")
                            {
                                double TickPrice = 0;
                                double.TryParse(ListParameterItem[j].NumValue, out TickPrice);
                                ListSymbol[i].TickPrice = TickPrice;
                            }
                            #endregion

                            #region Add Value To Property Tick Size In Class Symbol
                            if (ListParameterItem[j].Code == "S029")
                            {
                                double TickSize = 0;
                                double.TryParse(ListParameterItem[j].NumValue, out TickSize);
                                ListSymbol[i].TickSize = TickSize;
                            }
                            #endregion

                            #region Add Value To Property Profit Calculation In CLass Symbol
                            if (ListParameterItem[j].Code == "S033")
                            {
                                ListSymbol[i].ProfitCalculation = ListParameterItem[j].StringValue;
                            }
                            #endregion

                            #region Add Value To Property Digit In Class Symbol
                            if (ListParameterItem[j].Code == "S003")
                            {
                                int Digit = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out Digit);
                                ListSymbol[i].Digit = Digit;
                            }
                            #endregion

                            #region Add Value To Property Spread By Default In Class Symbol
                            if (ListParameterItem[j].Code == "S013")
                            {
                                double SpreadByDefault = 0;
                                double.TryParse(ListParameterItem[j].NumValue, out SpreadByDefault);
                                ListSymbol[i].SpreadByDefault = SpreadByDefault;
                            }
                            #endregion

                            #region Add Value To Property Spread Balance In Class Symbol
                            if (ListParameterItem[j].Code == "S016")
                            {
                                double SpreadBalance = 0;
                                double.TryParse(ListParameterItem[j].NumValue, out SpreadBalance);
                                ListSymbol[i].SpreadBalace = SpreadBalance;
                            }
                            #endregion

                            #region Add Value To Property Long Only In Class Symbol
                            if (ListParameterItem[j].Code == "S014")
                            {
                                bool LongOnly = false;
                                if (ListParameterItem[j].BoolValue == 1)
                                {
                                    LongOnly = true;
                                }
                                ListSymbol[i].LongOnly = LongOnly;
                            }
                            #endregion

                            #region Add Value To Property Limit Stop Level In Class Symbol
                            if (ListParameterItem[j].Code == "S015")
                            {
                                int LimitLevel = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out LimitLevel);
                                ListSymbol[i].LimitLevel = LimitLevel;
                            }
                            if (ListParameterItem[j].Code == "S046")
                            {
                                int StopLevel = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out StopLevel);
                                ListSymbol[i].StopLevel = StopLevel;
                            }
                            if (ListParameterItem[j].Code == "S047")
                            {
                                int SLTP = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out SLTP);
                                ListSymbol[i].StopLossTakeProfitLevel = SLTP;
                            }
                            #endregion

                            #region Add Value To Property Freeze Level In Class Symbol
                            if (ListParameterItem[j].Code == "S017")
                            {
                                int FreezeLevel = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out FreezeLevel);
                                ListSymbol[i].FreezeLevel = FreezeLevel;
                            }
                            #endregion

                            #region Add Value To Property Allow Read Time In Class Symbol
                            if (ListParameterItem[j].Code == "S018")
                            {
                                bool AllowReadTime = false;
                                if (ListParameterItem[j].BoolValue == 1)
                                {
                                    AllowReadTime = true;
                                }

                                ListSymbol[i].AllowReadTime = AllowReadTime;
                            }
                            #endregion

                            #region Add Value To Property Filter In Class Symbol
                            if (ListParameterItem[j].Code == "S022")
                            {
                                int Filter = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out Filter);
                                ListSymbol[i].Filter = Filter;
                            }
                            #endregion

                            #region Add Value To Property Filtration Level In Class Symbol
                            if (ListParameterItem[j].Code == "S020")
                            {
                                int FiltrationsLevel = 0;
                                int.TryParse(ListParameterItem[j].NumValue, out FiltrationsLevel);
                                ListSymbol[i].FiltrationsLevel = FiltrationsLevel;
                            }
                            #endregion

                            #region Add Value To Property Auto Limit In Class Symbol
                            if (ListParameterItem[j].Code == "S021")
                            {
                                ListSymbol[i].AutoLimit = ListParameterItem[j].StringValue;
                            }
                            #endregion

                            #region Add Value To Property Is Hedged In Class Symbol
                            if (ListParameterItem[j].Code == "S034")
                            {
                                bool IsHedged = false;
                                if (ListParameterItem[j].BoolValue == 1)
                                    IsHedged = true;
                                ListSymbol[i].IsHedged = IsHedged;
                            }
                            #endregion

                            #region Add Value To Property Trade In Class Symbol
                            if (ListParameterItem[j].Code == "S008")
                            {
                                ListSymbol[i].Trade = ListParameterItem[j].StringValue;
                            }
                            #endregion

                            #region Add Value To Property Currency
                            if (ListParameterItem[j].Code == "S007")
                            {
                                ListSymbol[i].Currency = ListParameterItem[j].StringValue;
                            }
                            #endregion

                            #region Add Value To Property Initial Margin
                            if (ListParameterItem[j].Code == "S026")
                            {
                                double InitialMargin = -1;
                                double.TryParse(ListParameterItem[j].NumValue, out InitialMargin);
                                ListSymbol[i].InitialMargin = InitialMargin;
                            }
                            #endregion

                            #region Add Value To Event Of Symbol
                            //if (ListParameterItem[j].Code == "S042")
                            //{
                            //    TradingServer.Facade.FacadeAddSymbolEvent(ListParameterItem[j].StringValue, ListSymbol[i].Name, ListParameterItem[j].Code, ListParameterItem[j].ParameterItemID);
                            //}

                            //if (ListParameterItem[j].Code == "S043")
                            //{
                            //    TradingServer.Facade.FacadeAddSymbolEvent(ListParameterItem[j].StringValue, ListSymbol[i].Name, ListParameterItem[j].Code, ListParameterItem[j].ParameterItemID);
                            //}
                            #endregion

                            #endregion
                        }

                        Flag = true;
                        break;
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol.Count > 0 && ListSymbol[i].RefSymbol != null)
                        {
                            this.AddNewSymbolConfigReference(ListParameterItem, ListSymbol[i].RefSymbol);
                        }
                    }
                }
            }
        }        
                
        /// <summary>
        /// UPDATE TRADING CONFIG, SETTING PARAMETER DEFAULT
        /// </summary>
        /// <param name="objSymbolConfig"></param>
        internal bool UpdateTradingConfig(Business.ParameterItem objSymbolConfig)
        {
            bool Result = false;
            StringBuilder content = new StringBuilder();

            if (Market.SymbolList != null)
            {
                int count = Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Market.SymbolList[i].SymbolID == objSymbolConfig.SecondParameterID)
                    {
                        if (Market.SymbolList[i].ParameterItems != null)
                        {
                            int countConfig = Market.SymbolList[i].ParameterItems.Count;
                            for (int j = 0; j < countConfig; j++)
                            {
                                if (Market.SymbolList[i].ParameterItems[j].Code == objSymbolConfig.Code)
                                {
                                    #region CHANGE CONFIG SYMBOL AND UPDATE CONFIG DEAFULT
                                    //Set ParameterID For Config
                                    objSymbolConfig.ParameterItemID = Market.SymbolList[i].ParameterItems[j].ParameterItemID;

                                    //Update Value Default Of Symbol
                                    #region Update Property Contract Size Of Symbol
                                    if (objSymbolConfig.Code == "S025")
                                    {
                                        double ContractSize = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out ContractSize);
                                        Market.SymbolList[i].ContractSize = ContractSize;
                                    }
                                    #endregion

                                    #region Update Property Tick Price Of Symbol
                                    if (objSymbolConfig.Code == "S030")
                                    {
                                        double TickPrice = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out TickPrice);
                                        Market.SymbolList[i].TickPrice = TickPrice;
                                    }
                                    #endregion

                                    #region Update Property Tick Size Of Symbol
                                    if (objSymbolConfig.Code == "S029")
                                    {
                                        double TickSize = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out TickSize);
                                        Market.SymbolList[i].TickSize = TickSize;
                                    }
                                    #endregion

                                    #region Update Property Profit Calculation Of Symbol
                                    if (objSymbolConfig.Code == "S033")
                                    {
                                        Market.SymbolList[i].ProfitCalculation = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Property Digit Of Symbol
                                    if (objSymbolConfig.Code == "S003")
                                    {
                                        int Digit = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out Digit);
                                        Market.SymbolList[i].Digit = Digit;
                                    }
                                    #endregion

                                    #region Update Property Currency
                                    if (objSymbolConfig.Code == "S007")
                                    {
                                        Market.SymbolList[i].Currency = objSymbolConfig.StringValue.Trim();
                                    }
                                    #endregion

                                    #region Update Property Initital Margin
                                    if (objSymbolConfig.Code == "S026")
                                    {
                                        double InitialMargin = -1;
                                        double.TryParse(objSymbolConfig.NumValue, out InitialMargin);
                                        Market.SymbolList[i].InitialMargin = InitialMargin;
                                    }
                                    #endregion

                                    #region Update Property Spread By Default Of Symbol
                                    if (objSymbolConfig.Code == "S013")
                                    {
                                        double SpreadByDefault = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out SpreadByDefault);
                                        Market.SymbolList[i].SpreadByDefault = SpreadByDefault;
                                    }
                                    #endregion

                                    #region Update Property Sread Balance Of Symbol
                                    if (objSymbolConfig.Code == "S016")
                                    {
                                        double SpreadBalance = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out SpreadBalance);
                                        Market.SymbolList[i].SpreadBalace = SpreadBalance;
                                    }
                                    #endregion

                                    #region Update Property Long Only Of Symbol
                                    if (objSymbolConfig.Code == "S014")
                                    {
                                        bool LongOnly = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                        {
                                            LongOnly = true;
                                        }
                                        Market.SymbolList[i].LongOnly = LongOnly;
                                    }
                                    #endregion

                                    #region Update Property Limit Stop Level Of Symbol
                                    if (objSymbolConfig.Code == "S015")
                                    {
                                        int LimitLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out LimitLevel);
                                        Market.SymbolList[i].LimitLevel = LimitLevel;
                                    }

                                    if (objSymbolConfig.Code == "S046")
                                    {
                                        int StopLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out StopLevel);
                                        Market.SymbolList[i].StopLevel = StopLevel;
                                    }

                                    if (objSymbolConfig.Code == "S047")
                                    {
                                        int SLTP = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out SLTP);
                                        Market.SymbolList[i].StopLossTakeProfitLevel = SLTP;
                                    }
                                    #endregion

                                    #region Update Property Freeze Level Of Symbol
                                    if (objSymbolConfig.Code == "S017")
                                    {
                                        int FreezeLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out FreezeLevel);
                                        Market.SymbolList[i].FreezeLevel = FreezeLevel;
                                    }
                                    #endregion

                                    #region Update Property Allow Read Time In Symbol
                                    if (objSymbolConfig.Code == "S018")
                                    {
                                        bool AllowReadTime = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                        {
                                            AllowReadTime = true;
                                        }

                                        Market.SymbolList[i].AllowReadTime = AllowReadTime;
                                    }
                                    #endregion

                                    #region Update Property Filter Of Symbol
                                    if (objSymbolConfig.Code == "S022")
                                    {
                                        int Filter = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out Filter);
                                        Market.SymbolList[i].Filter = Filter;
                                    }
                                    #endregion

                                    #region Update Property Filtration Level Of Symbol
                                    if (objSymbolConfig.Code == "S020")
                                    {
                                        int FiltrationsLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out FiltrationsLevel);
                                        Market.SymbolList[i].FiltrationsLevel = FiltrationsLevel;
                                    }
                                    #endregion

                                    #region Update Property Auto Limit Of Symbol
                                    if (objSymbolConfig.Code == "S021")
                                    {
                                        Market.SymbolList[i].AutoLimit = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Property IsHedged Of Symbol
                                    if (objSymbolConfig.Code == "S034")
                                    {
                                        bool IsHedged = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                            IsHedged = true;
                                        Market.SymbolList[i].IsHedged = IsHedged;

                                        //RECALCULATION ALL INVESTOR
                                        Business.Investor.investorInstance.ReCalculationTotalMargin();
                                    }
                                    #endregion

                                    #region Update Property Trade Of Symbol
                                    if (objSymbolConfig.Code == "S008")
                                    {
                                        Market.SymbolList[i].Trade = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Event Symbol
                                    //if (objSymbolConfig.Code == "S042")
                                    //{
                                    //    TradingServer.Facade.FacadeUpdateSymbolEvent(objSymbolConfig.StringValue, Business.Market.SymbolList[i].Name, 
                                    //        objSymbolConfig.Code, Business.Market.SymbolList[i].ParameterItems[j].ParameterItemID);
                                    //}

                                    //if (objSymbolConfig.Code == "S043")
                                    //{
                                    //    TradingServer.Facade.FacadeUpdateSymbolEvent(objSymbolConfig.StringValue, Business.Market.SymbolList[i].Name,
                                    //        objSymbolConfig.Code, Business.Market.SymbolList[i].ParameterItems[j].ParameterItemID);
                                    //}
                                    #endregion

                                    #region UPDATE PROPERTY APPLY SPREAD
                                    if (objSymbolConfig.Code == "S050")
                                    {
                                        bool applySpread = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                        {
                                            applySpread = true;
                                        }

                                        Business.Market.SymbolList[i].ApplySpread = applySpread;
                                    }
                                    #endregion

                                    #region UPDATE PROPERTY USE FREEZE MARGIN
                                    if (objSymbolConfig.Code == "S051")
                                    {
                                        bool useFreezeMargin = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                            useFreezeMargin = true;

                                        Business.Market.SymbolList[i].IsEnableFreezeMargin = useFreezeMargin;
                                    }
                                    #endregion

                                    #region UPDATE PROPERTY FREEZE MARGIN
                                    if (objSymbolConfig.Code == "S052")
                                    {
                                        double freezeMargin = 0;
                                        double.TryParse(objSymbolConfig.NumValue.ToString(), out freezeMargin);
                                        Business.Market.SymbolList[i].FreezeMargin = freezeMargin;
                                    }
                                    #endregion

                                    #region UPDATE PROPERTY FREEZE MARGIN HEDGED
                                    if (objSymbolConfig.Code == "S053")
                                    {
                                        double freezeMarginH = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out freezeMarginH);
                                        Business.Market.SymbolList[i].FreezeMarginHedged = freezeMarginH;
                                    }
                                    #endregion

                                    #region UPDATE PROPERTY MARGIN HEDGED
                                    if (objSymbolConfig.Code == "S028")
                                    {
                                        double marginH = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out marginH);
                                        Business.Market.SymbolList[i].MarginHedged = marginH;
                                    }
                                    #endregion

                                    //Update Value Parameter Item
                                    Market.SymbolList[i].ParameterItems[j].Name = objSymbolConfig.Name;
                                    Market.SymbolList[i].ParameterItems[j].Code = objSymbolConfig.Code;
                                    Market.SymbolList[i].ParameterItems[j].BoolValue = objSymbolConfig.BoolValue;
                                    Market.SymbolList[i].ParameterItems[j].StringValue = objSymbolConfig.StringValue;
                                    Market.SymbolList[i].ParameterItems[j].NumValue = objSymbolConfig.NumValue;
                                    Market.SymbolList[i].ParameterItems[j].DateValue = objSymbolConfig.DateValue;

                                    //Set ParameterID Again
                                    objSymbolConfig.ParameterItemID = Market.SymbolList[i].ParameterItems[j].ParameterItemID;
                                    #endregion

                                    break;
                                }
                            }
                        }

                        #region UPDATE CALCULATION SYMBOL
                        if (objSymbolConfig.Code == "S025" || objSymbolConfig.Code == "S026" ||
                            objSymbolConfig.Code == "S027" || objSymbolConfig.Code == "S028" ||
                            objSymbolConfig.Code == "S029" || objSymbolConfig.Code == "S030" ||
                            objSymbolConfig.Code == "S031" || objSymbolConfig.Code == "S032" ||
                            objSymbolConfig.Code == "S034")
                        {
                            List<Business.Investor> listInvestor = new List<Investor>();
                            if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                            {
                                int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                                for (int j = 0; j < countCommand; j++)
                                {
                                    bool flagInvestor = false;
                                    Business.Market.SymbolList[i].CommandList[j].CalculatorMarginCommand(Business.Market.SymbolList[i].CommandList[j]);

                                    if (listInvestor != null)
                                    {
                                        int countInvestor = listInvestor.Count;
                                        for (int n = 0; n < countInvestor; n++)
                                        {
                                            if (Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID == listInvestor[n].InvestorID)
                                            {
                                                flagInvestor = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!flagInvestor)
                                    {
                                        listInvestor.Add(Business.Market.SymbolList[i].CommandList[j].Investor);
                                    }
                                }
                            }

                            if (listInvestor != null && listInvestor.Count > 0)
                            {
                                int countInvestor = listInvestor.Count;
                                for (int j = 0; j < countInvestor; j++)
                                {
                                    if (listInvestor[j].CommandList != null && listInvestor[j].CommandList.Count > 0)
                                    {
                                        int countCommand = listInvestor[j].CommandList.Count;
                                        for (int n = 0; n < countCommand; n++)
                                        {
                                            if (listInvestor[j].CommandList[n].Symbol.Name == Market.SymbolList[i].Name)
                                            {
                                                listInvestor[j].CommandList[n].CalculatorMarginCommand(listInvestor[j].CommandList[n]);
                                            }
                                        }
                                    }

                                    if (listInvestor[j].CommandList.Count > 0)
                                    {
                                        Business.Margin newMargin = new Margin();
                                        newMargin = listInvestor[j].CommandList[0].Symbol.CalculationTotalMargin(listInvestor[j].CommandList);
                                        listInvestor[j].Margin = newMargin.TotalMargin;
                                        listInvestor[j].FreezeMargin = newMargin.TotalFreezeMargin;
                                    }
                                    else
                                    {
                                        listInvestor[j].Margin = 0;
                                        listInvestor[j].FreezeMargin = 0;
                                    }

                                    //SEND NOTIFY TO MANAGER
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(3, listInvestor[j]);

                                    //SEND NOTIFY TO CLIENT
                                    TradingServer.Business.Market.SendNotifyToClient("IAC04332451", 3, listInvestor[j].InvestorID);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //Find In Reference Symbol
                        this.UpdateTradingConfigReference(objSymbolConfig, Market.SymbolList[i].RefSymbol);
                    }
                }
            }

            Result = ParameterItem.DBWTradingConfigInstance.UpdateTradingConfig(objSymbolConfig.ParameterItemID, objSymbolConfig.SecondParameterID, -1,
                objSymbolConfig.Name, objSymbolConfig.Code, objSymbolConfig.BoolValue, objSymbolConfig.StringValue, objSymbolConfig.NumValue, objSymbolConfig.DateValue);

            #region SEARCH IN INVESTOR ONLINE AND SENT COMMAND UPDATE CONFIG SYMBOL
            //SEARCH IN INVESTOR ONLINE AND SENT COMMAND UPDATE CONFIG SYMBOL 
            //if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
            //{
            //    int count = Business.Market.InvestorList.Count;
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (Business.Market.InvestorList[i].IsOnline)
            //        {
            //            if (Business.Market.InvestorList[i].ClientCommandQueue == null)
            //                Business.Market.InvestorList[i].ClientCommandQueue = new List<string>();

            //            Business.Market.InvestorList[i].ClientCommandQueue.Add("RSC1235789$" + objSymbolConfig.ParameterItemID + "," + objSymbolConfig.SecondParameterID + ",-1" +
            //                objSymbolConfig.Name + "," + objSymbolConfig.Code + "," + objSymbolConfig.BoolValue + "," + objSymbolConfig.StringValue + "," + objSymbolConfig.NumValue +
            //                objSymbolConfig.DateValue);
            //        }
            //    }
            //}
            #endregion

            if (objSymbolConfig.Code == "S042" || objSymbolConfig.Code == "S043")
            {
                //Call Function Init Time Event Symbol
                Business.Market.marketInstance.InitTimeEventInSymbol();

                //CALL FUNCTION INIT TIME MARKET
                Business.Market.marketInstance.InitTimeEventServer();
            }

            if (objSymbolConfig.Code == "S044" || objSymbolConfig.Code == "S045")
            {
                Business.Market.marketInstance.InitSymbolFuture();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listTradingConfig"></param>
        /// <returns></returns>
        internal bool UpdateTradingConfig(List<Business.ParameterItem> listTradingConfig, string code,string ipAddress)
        {
            bool Result = false;
            bool isExits = false;
            StringBuilder content = new StringBuilder();
            StringBuilder beforeContent = new StringBuilder();
            StringBuilder afterContent = new StringBuilder();

            content.Append("'" + code + "': update config symbol ");

            if (listTradingConfig != null)
            {
                int countTradingConfig = listTradingConfig.Count;
                for (int a = 0; a < countTradingConfig; a++)
                {
                    if (Market.SymbolList != null)
                    {
                        int count = Market.SymbolList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Market.SymbolList[i].SymbolID == listTradingConfig[a].SecondParameterID)
                            {
                                #region CHECK FIRST RUN
                                if (!isExits)
                                {
                                    content.Append("'" + Business.Market.SymbolList[i].Name + "': ");
                                    isExits = true;
                                }
                                #endregion

                                #region UPDATE PARAMETER ON TRADING CONFIG
                                if (Market.SymbolList[i].ParameterItems != null)
                                {
                                    int countConfig = Market.SymbolList[i].ParameterItems.Count;
                                    for (int j = 0; j < countConfig; j++)
                                    {
                                        if (Market.SymbolList[i].ParameterItems[j].Code == listTradingConfig[a].Code)
                                        {
                                            #region FILTER CODE GET NAME UPDATE BEFORE
                                            switch (Market.SymbolList[i].ParameterItems[j].Code)
                                            {
                                                case "S001":    //SYMBOL NAME
                                                    beforeContent.Append("symbol name: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S002":    //SOURCE
                                                    break;
                                                case "S003":    //DIGIT
                                                    beforeContent.Append("digit: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S004":    //DESCRIPTION
                                                    beforeContent.Append("description: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S005":    //TYPE
                                                    beforeContent.Append("type: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S006":    //EXECUTION
                                                    beforeContent.Append("execution: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S007":    //CURRENCY
                                                    beforeContent.Append("currency: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S008":    //TRADE
                                                    beforeContent.Append("trade: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S009":    //BACKGROUND
                                                    break;
                                                case "S010": //MARGIN CURRENCY
                                                    beforeContent.Append("margin currency: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S011":    //MAXIMUM LOTS FOR IE
                                                    beforeContent.Append("maximum lots for IE: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S012":    //ORDERS
                                                    beforeContent.Append("orders: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S013":    //SPREAD BY DEFAULT
                                                    beforeContent.Append("spread by default: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S014":    //LONG ONLY
                                                    if (Market.SymbolList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("long only: enable - ");
                                                    else
                                                        beforeContent.Append("long only: disable - ");
                                                    break;
                                                case "S015":
                                                    beforeContent.Append("limit level: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S016":
                                                    beforeContent.Append("spread balance: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S017":
                                                    beforeContent.Append("freeze level: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S018":
                                                    if (Business.Market.SymbolList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("use datafeed time stamp: on - ");
                                                    else
                                                        beforeContent.Append("use datafeed time stamp: off - ");
                                                    break;
                                                case "S019":
                                                    break;
                                                case "S020":
                                                    beforeContent.Append("filtration level: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S021":
                                                    beforeContent.Append("auto limit: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S022":
                                                    beforeContent.Append("filter: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S023":
                                                    break;
                                                case "S024":
                                                    break;
                                                case "S025":
                                                    beforeContent.Append("contract size: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S026":
                                                    beforeContent.Append("initial margin: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S027":
                                                    break;
                                                case "S028":
                                                    beforeContent.Append("hedge: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S029":
                                                    beforeContent.Append("tick size: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S030":
                                                    beforeContent.Append("tick price: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S031":
                                                    beforeContent.Append("percentage: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S032":
                                                    beforeContent.Append("margin calculation: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S033":
                                                    beforeContent.Append("profit calculation: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S034":
                                                    if (Market.SymbolList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("strong hedged margin mode: enable - ");
                                                    else
                                                        beforeContent.Append("string hedged margin mode: disable - ");
                                                    break;
                                                case "S035":
                                                    if (Business.Market.SymbolList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("enable swap: on - ");
                                                    else
                                                        beforeContent.Append("enable swap: off - ");
                                                    break;
                                                case "S036":
                                                    beforeContent.Append("swap calculation: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S037":
                                                    beforeContent.Append("long position: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S038":
                                                    beforeContent.Append("short position: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S039":
                                                    beforeContent.Append("3-day swaps: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S040":
                                                    break;
                                                case "S041":
                                                    break;
                                                case "S042":
                                                    beforeContent.Append("trade session: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S043":
                                                    beforeContent.Append("quote session: " + Market.SymbolList[i].ParameterItems[j].StringValue + " - ");
                                                    break;
                                                case "S044":
                                                    beforeContent.Append("close only: " + Market.SymbolList[i].ParameterItems[j].DateValue + " - ");
                                                    break;
                                                case "S045":
                                                    beforeContent.Append("time expired: " + Market.SymbolList[i].ParameterItems[j].DateValue + " - ");
                                                    break;
                                                case "S046":
                                                    beforeContent.Append("stop level: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S047":
                                                    beforeContent.Append("stoploss and takeprofit level: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S048":
                                                    beforeContent.Append("start focus digit: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S049":
                                                    beforeContent.Append("end focus digit: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S050":
                                                    if (Market.SymbolList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("apply spread: enable - ");
                                                    else
                                                        beforeContent.Append("apply spread: disable - ");
                                                    break;
                                                case "S051":
                                                    if (Market.SymbolList[i].ParameterItems[j].BoolValue == 1)
                                                        beforeContent.Append("use freeze margin: enable - ");
                                                    else
                                                        beforeContent.Append("use freeze margin: disable - ");
                                                    break;
                                                case "S052":
                                                    beforeContent.Append("freeze margin: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S053":
                                                    beforeContent.Append("freeze margin hedged: " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                                case "S054":
                                                    beforeContent.Append("on hold time : " + Market.SymbolList[i].ParameterItems[j].NumValue + " - ");
                                                    break;
                                            }
                                            #endregion

                                            #region FILTER CODE GET NAME UPDATE AFTER
                                            switch (listTradingConfig[a].Code)
                                            {
                                                case "S001":    //SYMBOL NAME
                                                    afterContent.Append("symbol name: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S002":    //SOURCE
                                                    break;
                                                case "S003":    //DIGIT
                                                    afterContent.Append("digit: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S004":    //DESCRIPTION
                                                    afterContent.Append("description: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S005":    //TYPE
                                                    afterContent.Append("type: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S006":    //EXECUTION
                                                    afterContent.Append("execution: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S007":    //CURRENCY
                                                    afterContent.Append("currency: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S008":    //TRADE
                                                    afterContent.Append("trade: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S009":    //BACKGROUND
                                                    break;
                                                case "S010": //MARGIN CURRENCY
                                                    afterContent.Append("margin currency: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S011":    //MAXIMUM LOTS FOR IE
                                                    afterContent.Append("maximum lots for IE: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S012":    //ORDERS
                                                    afterContent.Append("orders: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S013":    //SPREAD BY DEFAULT
                                                    afterContent.Append("spread by default: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S014":    //LONG ONLY
                                                    if (listTradingConfig[a].BoolValue == 1)
                                                        afterContent.Append("long only: enable - ");
                                                    else
                                                        afterContent.Append("long only: disable - ");
                                                    break;
                                                case "S015":
                                                    afterContent.Append("limit level: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S016":
                                                    afterContent.Append("spread balance: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S017":
                                                    afterContent.Append("freeze level: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S018":
                                                    if (listTradingConfig[a].BoolValue == 1)
                                                        afterContent.Append("use datafeed time stamp: on - ");
                                                    else
                                                        afterContent.Append("use datafeed time stamp: off - ");
                                                    break;
                                                case "S019":
                                                    break;
                                                case "S020":
                                                    afterContent.Append("filtration level: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S021":
                                                    afterContent.Append("auto limit: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S022":
                                                    afterContent.Append("filter: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S023":
                                                    break;
                                                case "S024":
                                                    break;
                                                case "S025":
                                                    afterContent.Append("contract size: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S026":
                                                    afterContent.Append("initial margin: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S027":
                                                    break;
                                                case "S028":
                                                    afterContent.Append("hedge: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S029":
                                                    afterContent.Append("tick size: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S030":
                                                    afterContent.Append("tick price: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S031":
                                                    afterContent.Append("percentage: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S032":
                                                    afterContent.Append("margin calculation: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S033":
                                                    afterContent.Append("profit calculation: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S034":
                                                    if (listTradingConfig[a].BoolValue == 1)
                                                        afterContent.Append("strong hedged margin mode: enable - ");
                                                    else
                                                        afterContent.Append("string hedged margin mode: disable - ");
                                                    break;
                                                case "S035":
                                                    if (listTradingConfig[a].BoolValue == 1)
                                                        afterContent.Append("enable swap: on - ");
                                                    else
                                                        afterContent.Append("enable swap: off - ");
                                                    break;
                                                case "S036":
                                                    afterContent.Append("swap calculation: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S037":
                                                    afterContent.Append("long position: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S038":
                                                    afterContent.Append("short position: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S039":
                                                    afterContent.Append("3-day swaps: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S040":
                                                    break;
                                                case "S041":
                                                    break;
                                                case "S042":
                                                    afterContent.Append("trade session: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S043":
                                                    afterContent.Append("quote session: " + listTradingConfig[a].StringValue + " - ");
                                                    break;
                                                case "S044":
                                                    afterContent.Append("close only: " + listTradingConfig[a].DateValue + " - ");
                                                    break;
                                                case "S045":
                                                    afterContent.Append("time expired: " + listTradingConfig[a].DateValue + " - ");
                                                    break;
                                                case "S046":
                                                    afterContent.Append("stop level: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S047":
                                                    afterContent.Append("stoploss and takeprofit level: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S048":
                                                    afterContent.Append("start focus digit: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S049":
                                                    afterContent.Append("end focus digit: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S050":
                                                    if (listTradingConfig[a].BoolValue == 1)
                                                        afterContent.Append("apply spread: enable - ");
                                                    else
                                                        afterContent.Append("apply spread: disable - ");
                                                    break;
                                                case "S051":
                                                    if (listTradingConfig[a].BoolValue == 1)
                                                        afterContent.Append("use freeze margin: enable - ");
                                                    else
                                                        afterContent.Append("use freeze margin: disable - ");
                                                    break;
                                                case "S052":
                                                    afterContent.Append("freeze margin: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S053":
                                                    afterContent.Append("freeze margin hedged: " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                                case "S054":
                                                    afterContent.Append("on hold time : " + listTradingConfig[a].NumValue + " - ");
                                                    break;
                                            }
                                            #endregion

                                            #region CHANGE CONFIG SYMBOL AND UPDATE CONFIG DEAFULT
                                            //Set ParameterID For Config
                                            listTradingConfig[a].ParameterItemID = Market.SymbolList[i].ParameterItems[j].ParameterItemID;

                                            //Update Value Default Of Symbol
                                            #region Update Property Contract Size Of Symbol
                                            if (listTradingConfig[a].Code == "S025")
                                            {
                                                double ContractSize = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out ContractSize);
                                                Market.SymbolList[i].ContractSize = ContractSize;
                                            }
                                            #endregion

                                            #region Update Property Tick Price Of Symbol
                                            if (listTradingConfig[a].Code == "S030")
                                            {
                                                double TickPrice = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out TickPrice);
                                                Market.SymbolList[i].TickPrice = TickPrice;
                                            }
                                            #endregion

                                            #region Update Property Tick Size Of Symbol
                                            if (listTradingConfig[a].Code == "S029")
                                            {
                                                double TickSize = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out TickSize);
                                                Market.SymbolList[i].TickSize = TickSize;
                                            }
                                            #endregion

                                            #region Update Property Profit Calculation Of Symbol
                                            if (listTradingConfig[a].Code == "S033")
                                            {
                                                Market.SymbolList[i].ProfitCalculation = listTradingConfig[a].StringValue;
                                            }
                                            #endregion

                                            #region Update Property Digit Of Symbol
                                            if (listTradingConfig[a].Code == "S003")
                                            {
                                                int Digit = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out Digit);
                                                Market.SymbolList[i].Digit = Digit;
                                            }
                                            #endregion

                                            #region Update Property Currency
                                            if (listTradingConfig[a].Code == "S007")
                                            {
                                                Market.SymbolList[i].Currency = listTradingConfig[a].StringValue.Trim();
                                            }
                                            #endregion

                                            #region Update Property Initital Margin
                                            if (listTradingConfig[a].Code == "S026")
                                            {
                                                double InitialMargin = -1;
                                                double.TryParse(listTradingConfig[a].NumValue, out InitialMargin);
                                                Market.SymbolList[i].InitialMargin = InitialMargin;
                                            }
                                            #endregion

                                            #region Update Property Spread By Default Of Symbol
                                            if (listTradingConfig[a].Code == "S013")
                                            {
                                                double SpreadByDefault = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out SpreadByDefault);
                                                Market.SymbolList[i].SpreadByDefault = SpreadByDefault;
                                            }
                                            #endregion

                                            #region Update Property Sread Balance Of Symbol
                                            if (listTradingConfig[a].Code == "S016")
                                            {
                                                double SpreadBalance = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out SpreadBalance);
                                                Market.SymbolList[i].SpreadBalace = SpreadBalance;
                                            }
                                            #endregion

                                            #region Update Property Long Only Of Symbol
                                            if (listTradingConfig[a].Code == "S014")
                                            {
                                                bool LongOnly = false;
                                                if (listTradingConfig[a].BoolValue == 1)
                                                {
                                                    LongOnly = true;
                                                }
                                                Market.SymbolList[i].LongOnly = LongOnly;
                                            }
                                            #endregion

                                            #region Update Property Limit Stop Level Of Symbol
                                            if (listTradingConfig[a].Code == "S015")
                                            {
                                                int LimitLevel = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out LimitLevel);
                                                Market.SymbolList[i].LimitLevel = LimitLevel;
                                            }

                                            if (listTradingConfig[a].Code == "S046")
                                            {
                                                int StopLevel = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out StopLevel);
                                                Market.SymbolList[i].StopLevel = StopLevel;
                                            }

                                            if (listTradingConfig[a].Code == "S047")
                                            {
                                                int SLTP = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out SLTP);
                                                Market.SymbolList[i].StopLossTakeProfitLevel = SLTP;
                                            }
                                            #endregion

                                            #region Update Property Freeze Level Of Symbol
                                            if (listTradingConfig[a].Code == "S017")
                                            {
                                                int FreezeLevel = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out FreezeLevel);
                                                Market.SymbolList[i].FreezeLevel = FreezeLevel;
                                            }
                                            #endregion

                                            #region Update Property Allow Read Time In Symbol
                                            if (listTradingConfig[a].Code == "S018")
                                            {
                                                bool AllowReadTime = false;
                                                if (listTradingConfig[a].BoolValue == 1)
                                                {
                                                    AllowReadTime = true;
                                                }

                                                Market.SymbolList[i].AllowReadTime = AllowReadTime;
                                            }
                                            #endregion

                                            #region Update Property Filter Of Symbol
                                            if (listTradingConfig[a].Code == "S022")
                                            {
                                                int Filter = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out Filter);
                                                Market.SymbolList[i].Filter = Filter;
                                            }
                                            #endregion

                                            #region Update Property Filtration Level Of Symbol
                                            if (listTradingConfig[a].Code == "S020")
                                            {
                                                int FiltrationsLevel = 0;
                                                int.TryParse(listTradingConfig[a].NumValue, out FiltrationsLevel);
                                                Market.SymbolList[i].FiltrationsLevel = FiltrationsLevel;
                                            }
                                            #endregion

                                            #region Update Property Auto Limit Of Symbol
                                            if (listTradingConfig[a].Code == "S021")
                                            {
                                                Market.SymbolList[i].AutoLimit = listTradingConfig[a].StringValue;
                                            }
                                            #endregion

                                            #region Update Property IsHedged Of Symbol
                                            if (listTradingConfig[a].Code == "S034")
                                            {
                                                bool IsHedged = false;
                                                if (listTradingConfig[a].BoolValue == 1)
                                                    IsHedged = true;
                                                Market.SymbolList[i].IsHedged = IsHedged;

                                                //RECALCULATION ALL INVESTOR
                                                Business.Investor.investorInstance.ReCalculationTotalMargin();
                                            }
                                            #endregion

                                            #region Update Property Trade Of Symbol
                                            if (listTradingConfig[a].Code == "S008")
                                            {
                                                Market.SymbolList[i].Trade = listTradingConfig[a].StringValue;
                                            }
                                            #endregion

                                            #region UPDATE PROPERTY APPLY SPREAD
                                            if (listTradingConfig[a].Code == "S050")
                                            {
                                                bool applySpread = false;
                                                if (listTradingConfig[a].BoolValue == 1)
                                                {
                                                    applySpread = true;
                                                }

                                                Business.Market.SymbolList[i].ApplySpread = applySpread;
                                            }
                                            #endregion

                                            #region UPDATE PROPERTY USE FREEZE MARGIN
                                            if (listTradingConfig[a].Code == "S051")
                                            {
                                                bool useFreezeMargin = false;
                                                if (listTradingConfig[a].BoolValue == 1)
                                                    useFreezeMargin = true;

                                                Business.Market.SymbolList[i].IsEnableFreezeMargin = useFreezeMargin;
                                            }
                                            #endregion

                                            #region UPDATE PROPERTY FREEZE MARGIN
                                            if (listTradingConfig[a].Code == "S052")
                                            {
                                                double freezeMargin = 0;
                                                double.TryParse(listTradingConfig[a].NumValue.ToString(), out freezeMargin);
                                                Business.Market.SymbolList[i].FreezeMargin = freezeMargin;
                                            }
                                            #endregion

                                            #region UPDATE PROPERTY FREEZE MARGIN HEDGED
                                            if (listTradingConfig[a].Code == "S053")
                                            {
                                                double freezeMarginH = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out freezeMarginH);
                                                Business.Market.SymbolList[i].FreezeMarginHedged = freezeMarginH;
                                            }
                                            #endregion

                                            #region UPDATE PROPERTY MARGIN HEDGED
                                            if (listTradingConfig[a].Code == "S028")
                                            {
                                                double marginH = 0;
                                                double.TryParse(listTradingConfig[a].NumValue, out marginH);
                                                Business.Market.SymbolList[i].MarginHedged = marginH;
                                            }
                                            #endregion

                                            //Update Value Parameter Item
                                            Market.SymbolList[i].ParameterItems[j].Name = listTradingConfig[a].Name;
                                            Market.SymbolList[i].ParameterItems[j].Code = listTradingConfig[a].Code;
                                            Market.SymbolList[i].ParameterItems[j].BoolValue = listTradingConfig[a].BoolValue;
                                            Market.SymbolList[i].ParameterItems[j].StringValue = listTradingConfig[a].StringValue;
                                            Market.SymbolList[i].ParameterItems[j].NumValue = listTradingConfig[a].NumValue;
                                            Market.SymbolList[i].ParameterItems[j].DateValue = listTradingConfig[a].DateValue;

                                            //Set ParameterID Again
                                            listTradingConfig[a].ParameterItemID = Market.SymbolList[i].ParameterItems[j].ParameterItemID;
                                            #endregion

                                            break;
                                        }
                                    }
                                } 
                                #endregion

                                #region UPDATE CALCULATION SYMBOL
                                if (listTradingConfig[a].Code == "S025" || listTradingConfig[a].Code == "S026" ||
                                    listTradingConfig[a].Code == "S027" || listTradingConfig[a].Code == "S028" ||
                                    listTradingConfig[a].Code == "S029" || listTradingConfig[a].Code == "S030" ||
                                    listTradingConfig[a].Code == "S031" || listTradingConfig[a].Code == "S032" ||
                                    listTradingConfig[a].Code == "S034")
                                {
                                    List<Business.Investor> listInvestor = new List<Investor>();
                                    if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                    {
                                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                                        for (int j = 0; j < countCommand; j++)
                                        {
                                            bool flagInvestor = false;
                                            Business.Market.SymbolList[i].CommandList[j].CalculatorMarginCommand(Business.Market.SymbolList[i].CommandList[j]);

                                            if (listInvestor != null)
                                            {
                                                int countInvestor = listInvestor.Count;
                                                for (int n = 0; n < countInvestor; n++)
                                                {
                                                    if (Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID == listInvestor[n].InvestorID)
                                                    {
                                                        flagInvestor = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!flagInvestor)
                                            {
                                                listInvestor.Add(Business.Market.SymbolList[i].CommandList[j].Investor);
                                            }
                                        }
                                    }

                                    if (listInvestor != null && listInvestor.Count > 0)
                                    {
                                        int countInvestor = listInvestor.Count;
                                        for (int j = 0; j < countInvestor; j++)
                                        {
                                            if (listInvestor[j].CommandList != null && listInvestor[j].CommandList.Count > 0)
                                            {
                                                int countCommand = listInvestor[j].CommandList.Count;
                                                for (int n = 0; n < countCommand; n++)
                                                {
                                                    if (listInvestor[j].CommandList[n].Symbol.Name == Market.SymbolList[i].Name)
                                                    {
                                                        listInvestor[j].CommandList[n].CalculatorMarginCommand(listInvestor[j].CommandList[n]);
                                                    }
                                                }
                                            }

                                            if (listInvestor[j].CommandList.Count > 0)
                                            {
                                                Business.Margin newMargin = new Margin();
                                                newMargin = listInvestor[j].CommandList[0].Symbol.CalculationTotalMargin(listInvestor[j].CommandList);
                                                listInvestor[j].Margin = newMargin.TotalMargin;
                                                listInvestor[j].FreezeMargin = newMargin.TotalFreezeMargin;
                                            }
                                            else
                                            {
                                                listInvestor[j].Margin = 0;
                                                listInvestor[j].FreezeMargin = 0;
                                            }

                                            //SEND NOTIFY TO MANAGER
                                            TradingServer.Facade.FacadeSendNotifyManagerRequest(3, listInvestor[j]);

                                            //SEND NOTIFY TO CLIENT
                                            TradingServer.Business.Market.SendNotifyToClient("IAC04332451", 3, listInvestor[j].InvestorID);
                                        }
                                    }
                                }
                                #endregion

                                #region UPDATE TRADING CONFIG TO DATABASE AND INIT TIME EVENT IN SYSTEM
                                Result = ParameterItem.DBWTradingConfigInstance.UpdateTradingConfig(listTradingConfig[a].ParameterItemID, listTradingConfig[a].SecondParameterID, -1,
                                    listTradingConfig[a].Name, listTradingConfig[a].Code, listTradingConfig[a].BoolValue, listTradingConfig[a].StringValue, listTradingConfig[a].NumValue,
                                    listTradingConfig[a].DateValue);

                                if (listTradingConfig[a].Code == "S042" || listTradingConfig[a].Code == "S043")
                                {
                                    //Call Function Init Time Event Symbol
                                    Business.Market.marketInstance.InitTimeEventInSymbol();

                                    //CALL FUNCTION INIT TIME MARKET
                                    Business.Market.marketInstance.InitTimeEventServer();
                                }

                                if (listTradingConfig[a].Code == "S044" || listTradingConfig[a].Code == "S045")
                                {
                                    Business.Market.marketInstance.InitSymbolFuture();
                                }
                                #endregion

                                break;
                            }
                            else
                            {
                                //Find In Reference Symbol
                                this.UpdateTradingConfigReference(listTradingConfig[a], Market.SymbolList[i].RefSymbol);
                            }
                        }
                    }
                }
            }

            string tempBeforeContent = beforeContent.ToString();
            if (tempBeforeContent.EndsWith(" - "))
                tempBeforeContent = tempBeforeContent.Remove(tempBeforeContent.Length - 2, 2);

            string tempAfterContent = afterContent.ToString();
            if (tempAfterContent.EndsWith(" - "))
                tempAfterContent = tempAfterContent.Remove(tempAfterContent.Length - 2, 2);

            if (!string.IsNullOrEmpty(tempAfterContent.Trim()) && !string.IsNullOrEmpty(tempBeforeContent.Trim()))
            {
                content.Append(tempBeforeContent + " -> " + tempAfterContent);
                TradingServer.Facade.FacadeAddNewSystemLog(3, content.ToString(), "[update symbol config]", ipAddress, code);
            }

            return Result;
        }

        /// <summary>
        /// UPDATE TRADING CONFIG REFERENCE
        /// </summary>
        /// <param name="objSymbolConfig"></param>
        /// <param name="ListSymbol"></param>
        internal void UpdateTradingConfigReference(Business.ParameterItem objSymbolConfig, List<Business.Symbol> ListSymbol)
        {
            return;
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].SymbolID == objSymbolConfig.SecondParameterID)
                    {
                        if (ListSymbol[i].ParameterItems != null)
                        {
                            int countConfig = ListSymbol[i].ParameterItems.Count;
                            for (int j = 0; j < countConfig; j++)
                            {
                                if (ListSymbol[i].ParameterItems[j].ParameterItemID == objSymbolConfig.ParameterItemID)
                                {
                                    //Set ParameterID For Config
                                    objSymbolConfig.ParameterItemID = ListSymbol[i].ParameterItems[j].ParameterItemID;

                                    //Update Value Default Of Symbol
                                    #region Update Property Contract Size Of Symbol
                                    if (objSymbolConfig.Code == "S025")
                                    {
                                        double ContractSize = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out ContractSize);
                                        ListSymbol[i].ContractSize = ContractSize;
                                    }
                                    #endregion

                                    #region Update Property Tick Price Of Symbol
                                    if (objSymbolConfig.Code == "S030")
                                    {
                                        double TickPrice = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out TickPrice);
                                        ListSymbol[i].TickPrice = TickPrice;
                                    }
                                    #endregion

                                    #region Update Property Tick Size Of Symbol
                                    if (objSymbolConfig.Code == "S029")
                                    {
                                        double TickSize = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out TickSize);
                                        ListSymbol[i].TickSize = TickSize;
                                    }
                                    #endregion

                                    #region Update Property Profit Calculation Of Symbol
                                    if (objSymbolConfig.Code == "S033")
                                    {
                                        ListSymbol[i].ProfitCalculation = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Property Digit Of Symbol
                                    if (objSymbolConfig.Code == "S003")
                                    {
                                        int Digit = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out Digit);
                                        ListSymbol[i].Digit = Digit;
                                    }
                                    #endregion

                                    #region Update Property Spread By Default Of Symbol
                                    if (objSymbolConfig.Code == "S013")
                                    {
                                        double SpreadByDefault = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out SpreadByDefault);
                                        ListSymbol[i].SpreadByDefault = SpreadByDefault;
                                    }
                                    #endregion

                                    #region Update Property Sread Balance Of Symbol
                                    if (objSymbolConfig.Code == "S016")
                                    {
                                        double SpreadBalance = 0;
                                        double.TryParse(objSymbolConfig.NumValue, out SpreadBalance);
                                        ListSymbol[i].SpreadBalace = SpreadBalance;
                                    }
                                    #endregion

                                    #region Update Property Long Only Of Symbol
                                    if (objSymbolConfig.Code == "S014")
                                    {
                                        bool LongOnly = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                        {
                                            LongOnly = true;
                                        }
                                        ListSymbol[i].LongOnly = LongOnly;
                                    }
                                    #endregion

                                    #region Update Property Limit Stop Level Of Symbol
                                    if (objSymbolConfig.Code == "S015")
                                    {
                                        int LimitLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out LimitLevel);
                                        ListSymbol[i].LimitLevel = LimitLevel;
                                    }
                                    if (objSymbolConfig.Code == "S046")
                                    {
                                        int StopLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out StopLevel);
                                        ListSymbol[i].StopLevel = StopLevel;
                                    }
                                    if (objSymbolConfig.Code == "S047")
                                    {
                                        int SLTP = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out SLTP);
                                        ListSymbol[i].StopLossTakeProfitLevel = SLTP;
                                    }
                                    #endregion

                                    #region Update Property Freeze Level Of Symbol
                                    if (objSymbolConfig.Code == "S017")
                                    {
                                        int FreezeLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out FreezeLevel);
                                        ListSymbol[i].FreezeLevel = FreezeLevel;
                                    }
                                    #endregion

                                    #region Update Property Allow Read Time In Symbol
                                    if (objSymbolConfig.Code == "S018")
                                    {
                                        bool AllowReadTime = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                        {
                                            AllowReadTime = true;
                                        }

                                        ListSymbol[i].AllowReadTime = AllowReadTime;
                                    }
                                    #endregion

                                    #region Update Property Filter Of Symbol
                                    if (objSymbolConfig.Code == "S022")
                                    {
                                        int Filter = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out Filter);
                                        ListSymbol[i].Filter = Filter;
                                    }
                                    #endregion

                                    #region Update Property Filtration Level Of Symbl
                                    if (objSymbolConfig.Code == "S020")
                                    {
                                        int FiltrationsLevel = 0;
                                        int.TryParse(objSymbolConfig.NumValue, out FiltrationsLevel);
                                        ListSymbol[i].FiltrationsLevel = FiltrationsLevel;
                                    }
                                    #endregion

                                    #region Update Property Auto Limit Of Symbol
                                    if (objSymbolConfig.Code == "S021")
                                    {
                                        ListSymbol[i].AutoLimit = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Property IsHedged Of Symbol
                                    if (objSymbolConfig.Code == "S034")
                                    {
                                        bool IsHedged = false;
                                        if (objSymbolConfig.BoolValue == 1)
                                            IsHedged = true;
                                        ListSymbol[i].IsHedged = IsHedged;
                                    }
                                    #endregion

                                    #region Update Property Trade Of Symbol
                                    if (objSymbolConfig.Code == "S008")
                                    {
                                        ListSymbol[i].Trade = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Property Currency Of Symbol
                                    if (objSymbolConfig.Code == "S007")
                                    {
                                        ListSymbol[i].Currency = objSymbolConfig.StringValue;
                                    }
                                    #endregion

                                    #region Update Property Initital Margin Of Symbol
                                    if (objSymbolConfig.Code == "S026")
                                    {
                                        double InitialMargin = -1;
                                        double.TryParse(objSymbolConfig.NumValue, out InitialMargin);
                                        ListSymbol[i].InitialMargin = InitialMargin;
                                    }
                                    #endregion

                                    //Update Value Parameter Item
                                    ListSymbol[i].ParameterItems[j].Name = objSymbolConfig.Name;
                                    ListSymbol[i].ParameterItems[j].Code = objSymbolConfig.Code;
                                    ListSymbol[i].ParameterItems[j].BoolValue = objSymbolConfig.BoolValue;
                                    ListSymbol[i].ParameterItems[j].StringValue = objSymbolConfig.StringValue;
                                    ListSymbol[i].ParameterItems[j].NumValue = objSymbolConfig.NumValue;
                                    ListSymbol[i].ParameterItems[j].DateValue = objSymbolConfig.DateValue;

                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        this.UpdateTradingConfigReference(objSymbolConfig, ListSymbol[i].RefSymbol);
                    }
                }
            }
        }        
                
        /// <summary>
        /// DELETE TRADING CONFIG
        /// </summary>
        /// <param name="TradingConfigID"></param>
        /// <returns></returns>
        internal bool DeleteTradingConfig(int TradingConfigID)
        {
            bool Result = false;
            bool Flag = false;
            Result = ParameterItem.DBWTradingConfigInstance.DeleteTradingConfigByID(TradingConfigID);

            if (Result == true)
            {
                if (Market.SymbolList != null)
                {
                    int count = Market.SymbolList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SymbolList[i].ParameterItems != null)
                        {
                            int countRef = Market.SymbolList[i].ParameterItems.Count;
                            for (int j = 0; j < countRef; j++)
                            {
                                if (Market.SymbolList[i].ParameterItems[j].ParameterItemID == TradingConfigID)
                                {
                                    //Remove Trading Config
                                    Market.SymbolList[i].ParameterItems.Remove(Market.SymbolList[i].ParameterItems[j]);

                                    //Set Flag == True;
                                    Flag = true;

                                    return Result;
                                }
                            }
                        }

                        if (Flag == false)
                        {
                            if (Market.SymbolList[i].RefSymbol != null)
                            {
                                //Call Function Find In Reference Symbol
                                this.DeleteTradingConfigReference(TradingConfigID, Market.SymbolList[i].RefSymbol);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// DELETE TRADING CONFIG REFERENCE
        /// </summary>
        /// <param name="TradingConfigID"></param>
        /// <param name="ListSymbol"></param>
        internal void DeleteTradingConfigReference(int TradingConfigID, List<Business.Symbol> ListSymbol)
        {
            return;
            if (ListSymbol != null)
            {
                bool Flag = false;
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].ParameterItems != null)
                    {
                        int countRef = ListSymbol[i].ParameterItems.Count;
                        for (int j = 0; j < countRef; j++)
                        {
                            if (ListSymbol[i].ParameterItems[j].ParameterItemID == TradingConfigID)
                            {
                                //Remove Trading Config
                                ListSymbol[i].ParameterItems.Remove(ListSymbol[i].ParameterItems[j]);

                                Flag = true;
                                return;
                            }
                        }
                    }

                    if (Flag == false)
                    {
                        if (ListSymbol[i].RefSymbol != null)
                        {
                            //Call This
                            this.DeleteTradingConfigReference(TradingConfigID, ListSymbol[i].RefSymbol);
                        }
                    }
                }
            }
        }
    }
}
