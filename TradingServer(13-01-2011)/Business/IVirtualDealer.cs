using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class IVirtualDealer
    {
        public int IVirtualDealerID { get; set; }
        public int InvestorGroupID { get; set; }        
        public int SymbolID { get; set; }
        public int VirtualDealerID { get; set; }

        #region CREATE INSTANCE CLASS DBW IVIRTUALDEALER
        private static DBW.DBWIVirtualDealer dbwIVirtualDealer;
        private static DBW.DBWIVirtualDealer IVirtualDealerInstance
        {
            get
            {
                if (IVirtualDealer.dbwIVirtualDealer == null)
                    IVirtualDealer.dbwIVirtualDealer = new DBW.DBWIVirtualDealer();

                return IVirtualDealer.dbwIVirtualDealer;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IVirtualDealer> GetAllIVirtualDealer()
        {
            return IVirtualDealer.IVirtualDealerInstance.GetAllVirtualDealer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objIVirtualDealer"></param>
        /// <returns></returns>
        internal int AddNewIVirtualDealer(Business.IVirtualDealer objIVirtualDealer)
        {
            return IVirtualDealer.IVirtualDealerInstance.AddNewIVirtualDealer(objIVirtualDealer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objIVirtualDealer"></param>
        /// <returns></returns>
        internal bool UpdateIVirtualDealer(Business.IVirtualDealer objIVirtualDealer)
        {
            return IVirtualDealer.IVirtualDealerInstance.UpdateIVirtualDealer(objIVirtualDealer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iVirtualDealeriID"></param>
        /// <returns></returns>
        internal bool DeleteIVirtualDealer(int iVirtualDealeriID)
        {
            return IVirtualDealer.IVirtualDealerInstance.DeleteIVirtualDealer(iVirtualDealeriID);
        }
    }
}
