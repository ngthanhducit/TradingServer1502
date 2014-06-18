using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWCommandType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.TradeType> GetAllTradeType()
        {
            List<Business.TradeType> Result = new List<Business.TradeType>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandTypeTableAdapter adap = new DSTableAdapters.CommandTypeTableAdapter();
            DS.CommandTypeDataTable tbCommandType = new DS.CommandTypeDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbCommandType = adap.GetData();

                if (tbCommandType != null)
                {
                    int count = tbCommandType.Count;
                    for (int i = 0; i < count; i++)
                    {
                        switch (tbCommandType[i].Code)
                        {
                            #region Spot Command
                            case 1 :
                                {
                                    if (Business.TradeType.BuySpot == null)
                                        Business.TradeType.BuySpot = new Business.TradeType();

                                    Business.TradeType.BuySpot.ID = tbCommandType[i].Code;
                                    Business.TradeType.BuySpot.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.BuySpot);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.BuySpot);
                                }
                                break;

                            case 2 :
                                {
                                    if (Business.TradeType.SellSpot == null)
                                        Business.TradeType.SellSpot = new Business.TradeType();

                                    Business.TradeType.SellSpot.ID = tbCommandType[i].Code;
                                    Business.TradeType.SellSpot.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SellSpot);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SellSpot);
                                }
                                break;
                            #endregion

                            #region Binary Command
                            case 3:
                                {
                                    if (Business.TradeType.UpBinary == null)
                                        Business.TradeType.UpBinary = new Business.TradeType();
                                    Business.TradeType.UpBinary.ID = tbCommandType[i].Code;
                                    Business.TradeType.UpBinary.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.UpBinary);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.UpBinary);
                                }
                                break;

                            case 4:
                                {
                                    if (Business.TradeType.DownBinary == null)
                                        Business.TradeType.DownBinary = new Business.TradeType();
                                    Business.TradeType.DownBinary.ID = tbCommandType[i].Code;
                                    Business.TradeType.DownBinary.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.DownBinary);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.DownBinary);
                                }
                                break;
                            #endregion

                            #region Option Command
                            case 5:
                                {
                                    if (Business.TradeType.BuyOption == null)
                                        Business.TradeType.BuyOption = new Business.TradeType();
                                    Business.TradeType.BuyOption.ID = tbCommandType[i].Code;
                                    Business.TradeType.BuyOption.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.BuyOption);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.BuyOption);
                                }
                                break;

                            case 6:
                                {
                                    if (Business.TradeType.SellOption == null)
                                        Business.TradeType.SellOption = new Business.TradeType();
                                    Business.TradeType.SellOption.ID = tbCommandType[i].Code;
                                    Business.TradeType.SellOption.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SellOption);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SellOption);
                                }
                                break;
                            #endregion

                            #region Pending Order Command
                            case 7:
                                {
                                    if (Business.TradeType.SpotBuyLimit == null)
                                        Business.TradeType.SpotBuyLimit = new Business.TradeType();
                                    Business.TradeType.SpotBuyLimit.ID = tbCommandType[i].Code;
                                    Business.TradeType.SpotBuyLimit.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SpotBuyLimit);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SpotBuyLimit);
                                }
                                break;

                            case 8: 
                                {
                                    if (Business.TradeType.SpotSellLimit == null)
                                        Business.TradeType.SpotSellLimit = new Business.TradeType();
                                    Business.TradeType.SpotSellLimit.ID = tbCommandType[i].Code;
                                    Business.TradeType.SpotSellLimit.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SpotSellLimit);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SpotSellLimit);
                                }
                                break;

                            case 9:
                                {
                                    if (Business.TradeType.SpotBuyStop == null)
                                        Business.TradeType.SpotBuyStop = new Business.TradeType();
                                    Business.TradeType.SpotBuyStop.ID = tbCommandType[i].Code;
                                    Business.TradeType.SpotBuyStop.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SpotBuyStop);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SpotBuyStop);
                                }
                                break;

                            case 10:
                                {
                                    if (Business.TradeType.SpotSellStop == null)
                                        Business.TradeType.SpotSellStop = new Business.TradeType();
                                    Business.TradeType.SpotSellStop.ID = tbCommandType[i].Code;
                                    Business.TradeType.SpotSellStop.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SpotSellStop);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SpotSellStop);
                                }
                                break;
                            #endregion

                            #region Future Command
                            case 11:
                                {
                                    if (Business.TradeType.BuyFuture == null)
                                        Business.TradeType.BuyFuture = new Business.TradeType();
                                    Business.TradeType.BuyFuture.ID = tbCommandType[i].Code;
                                    Business.TradeType.BuyFuture.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.BuyFuture);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.BuyFuture);
                                }
                                break;

                            case 12:
                                {
                                    if (Business.TradeType.SellFuture == null)
                                        Business.TradeType.SellFuture = new Business.TradeType();
                                    Business.TradeType.SellFuture.ID = tbCommandType[i].Code;
                                    Business.TradeType.SellFuture.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SellFuture);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SellFuture);
                                }
                                break;
                            #endregion

                            #region PENDING ORDER FUTURE COMMAND
                            case 17:
                                {
                                    if (Business.TradeType.BuyStopFutureCommand == null)
                                        Business.TradeType.BuyStopFutureCommand = new Business.TradeType();
                                    Business.TradeType.BuyStopFutureCommand.ID = tbCommandType[i].Code;
                                    Business.TradeType.BuyStopFutureCommand.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.BuyStopFutureCommand);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.BuyStopFutureCommand);
                                }
                                break;

                            case 18:
                                {
                                    if (Business.TradeType.SellStopFutureCommand == null)
                                        Business.TradeType.SellStopFutureCommand = new Business.TradeType();
                                    Business.TradeType.SellStopFutureCommand.ID = tbCommandType[i].Code;
                                    Business.TradeType.SellStopFutureCommand.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SellStopFutureCommand);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SellStopFutureCommand);
                                }
                                break;

                            case 19:
                                {
                                    if (Business.TradeType.BuyLimitFutureCommand == null)
                                        Business.TradeType.BuyLimitFutureCommand = new Business.TradeType();
                                    Business.TradeType.BuyLimitFutureCommand.ID = tbCommandType[i].Code;
                                    Business.TradeType.BuyLimitFutureCommand.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.BuyLimitFutureCommand);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.BuyLimitFutureCommand);
                                }
                                break;

                            case 20:
                                {
                                    if (Business.TradeType.SellLimitFutureCommand == null)
                                        Business.TradeType.SellLimitFutureCommand = new Business.TradeType();
                                    Business.TradeType.SellLimitFutureCommand.ID = tbCommandType[i].Code;
                                    Business.TradeType.SellLimitFutureCommand.Name = tbCommandType[i].Value;

                                    //Find Market Area
                                    int countMarketArea = Business.Market.MarketArea.Count;
                                    for (int j = 0; j < countMarketArea; j++)
                                    {
                                        if (Business.Market.MarketArea[j].IMarketAreaID == tbCommandType[i].MarketAreaID)
                                        {
                                            if (Business.Market.MarketArea[j].Type == null)
                                                Business.Market.MarketArea[j].Type = new List<Business.TradeType>();

                                            Business.Market.MarketArea[j].Type.Add(Business.TradeType.SellLimitFutureCommand);

                                            break;
                                        }
                                    }

                                    Result.Add(Business.TradeType.SellLimitFutureCommand);
                                }
                                break;
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountCommandType()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.CommandTypeTableAdapter adap = new DSTableAdapters.CommandTypeTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountCommandType();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result.Value;
        }
    }
}
