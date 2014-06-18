using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.IPresenter
{
    public delegate void AddCommandDelegate();
    public delegate void CloseCommandDelegate();
    public delegate void SendClientCmdDelegate();
    public delegate void InitServerDelegate();

    interface IMarketArea
    {
        #region Property Market Area
        AddCommandDelegate AddCommandNotify { get; set; }
        int IMarketAreaID { get; set; }
        string IMarketAreaName { get; set; }
        Business.Market MarketContainer { get; set; }
        List<Business.TradeType> Type { get; set; }
        List<Business.Symbol> ListSymbol { get; set; }
        List<Business.ParameterItem> MarketAreaConfig { get; set; }
        #endregion

        #region Function Market Area
        void AddCommand(Business.OpenTrade Command);
        void CloseCommand(Business.OpenTrade Command);
        void MultiCloseCommand(Business.OpenTrade Command);
        void MultiUpdateCommand(Business.OpenTrade Command);
        Business.OpenTrade CalculateCommand(Business.OpenTrade Command);
        CloseCommandDelegate CloseCommandNotify(Business.OpenTrade Command);
        SendClientCmdDelegate SendClientCmdDelegate(string Cmd);
    
        void SetTickValueNotify(Business.Tick Tick,Business.Symbol RefSymbol);
        void UpdateCommand(Business.OpenTrade Command);
        #endregion
    }
}
