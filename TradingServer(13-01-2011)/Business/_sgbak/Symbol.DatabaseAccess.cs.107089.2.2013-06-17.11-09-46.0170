using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Symbol
    {
        /// <summary>
        /// 
        /// </summary>
        internal List<Business.Symbol> GetAllSymbol()
        {
            return Symbol.DBWSymbolInstance.GetAllSymbol();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SecurityID"></param>
        /// <returns></returns>
        internal List<Business.Symbol> GetSymbolBySecurityID(int SecurityID)
        {
            return Symbol.DBWSymbolInstance.GetSymbolBySecurityID(SecurityID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        internal bool DeleteSymbol(int SymbolID)
        {
            bool Result = false;
            Result = Symbol.DBWSymbolInstance.DeleteSymbol(SymbolID);

            if (Result == true)
            {
                //Find In Queue Symbol In Class Market And Delete Symbol
                if (Market.SymbolList != null)
                {
                    int count = Market.SymbolList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Market.SymbolList[i].SymbolID == SymbolID)
                        {
                            Market.SymbolList.Remove(Market.SymbolList[i]);
                        }
                        else
                        {
                            if (Market.SymbolList[i].RefSymbol.Count > 0)
                            {
                                this.DeleteSymbolReference(SymbolID, Market.SymbolList[i].RefSymbol);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// delete symbol
        /// if exist tradinfg config, delete trading config
        /// </summary>
        /// <param name="symbolID">symbolid</param>
        /// <returns></returns>
        internal bool DFDeleteSymbol(int symbolID)
        {
            DBW.DBWSymbol dbwSymbol = new DBW.DBWSymbol();
            return dbwSymbol.DFDeleteSymbol(symbolID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SymbolID"></param>
        internal void DeleteSymbolReference(int SymbolID, List<Business.Symbol> objSymbol)
        {
            if (objSymbol != null)
            {
                int count = objSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (objSymbol[i].SymbolID == SymbolID)
                    {
                        objSymbol.Remove(objSymbol[i]);
                    }
                    else
                    {
                        if (objSymbol[i].RefSymbol.Count > 0)
                        {
                            this.DeleteSymbolReference(SymbolID, objSymbol[i].RefSymbol);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountTotalSymbol()
        {
            return Symbol.DBWSymbolInstance.CountSymbol();
        }
    }
}
