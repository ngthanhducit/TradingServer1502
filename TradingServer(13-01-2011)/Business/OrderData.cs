using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class OrderData
    {
        #region CREATE INSTANCE OF CLASS DBWORDER
        private static DBW.DBWOrders dbwOrder;
        internal static DBW.DBWOrders OrderInstance
        {
            get
            {
                if (OrderData.dbwOrder == null)
                    OrderData.dbwOrder = new DBW.DBWOrders();

                return OrderData.dbwOrder;
            }
        }
        #endregion

        public int ID { get; set; }
        public string OrderCode { get; set; }
        public string Login { get; set; }
        public int InvestorID { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public double OneConvRate { get; set; }
        public double Commission { get; set; }
        public string Comment { get; set; }
        public double Lots { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public double TwoConvRate { get; set; }
        public double AgentCommission { get; set; }
        public string Symbol { get; set; }
        public double StopLoss { get; set; }
        public double TakeProfit { get; set; }
        public double MarginRate { get; set; }
        public double Swaps { get; set; }
        public double Taxes { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Profit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        internal List<Business.OrderData> GetOrderDataStartEnd(int InvestorID, int Start, int Limit)
        {
            return OrderData.OrderInstance.GetOrderByInvestorID(InvestorID, Start, Limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.OrderData GetOrderDataByCode(string Code)
        {
            return OrderData.OrderInstance.GetOrderByCode(Code);
        }
    }
}
