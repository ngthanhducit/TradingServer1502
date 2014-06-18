using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class PendingCommand : IPresenter.IMarketArea
    {
        public IPresenter.AddCommandDelegate AddCommandNotify { get; set; }
        public int IMarketAreaID { get; set; }
        public string IMarketAreaName { get; set; }
        public Market MarketContainer { get; set; }
        List<TradeType> IPresenter.IMarketArea.Type { get; set; }
        public List<Symbol> ListSymbol { get; set; }
        public List<ParameterItem> MarketAreaConfig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void AddCommand(OpenTrade Command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void CloseCommand(OpenTrade Command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public OpenTrade CalculateCommand(OpenTrade Command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public IPresenter.CloseCommandDelegate CloseCommandNotify(OpenTrade Command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        public IPresenter.SendClientCmdDelegate SendClientCmdDelegate(string Cmd)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        public void SetTickValueNotify(Tick Tick, Symbol RefSymbol)
        {
            throw new NotImplementedException();
        }


        public void UpdateCommand(OpenTrade Command)
        {
            throw new NotImplementedException();
        }
    }
}
