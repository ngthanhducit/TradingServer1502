using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TradingServer.Business
{
    public class PriceServer
    {
        public string IpServer { get; set; }
        public bool IsPrimary { get; set; }
        public Business.StatusPriceServer Status { get; set; }
        public DateTime TimeConnect { get; set; }        
        public bool IsRecive { get; set; }

        private static PriceServer _priceServer;
        public static PriceServer Instance
        {
            get
            {
                if (PriceServer._priceServer == null)
                    PriceServer._priceServer = new PriceServer();

                return PriceServer._priceServer;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool ConfigMultipleQuotes(string value)
        {
            List<Business.PriceServer> result = new List<Business.PriceServer>();
            string[] subValue = value.Split('[');

            if (subValue.Length > 0)
            {
                int count = subValue.Length;
                for (int i = 0; i < count; i++)
                {
                    Business.PriceServer newPriceServer = new Business.PriceServer();
                    string[] subParameter = subValue[i].Split(':');

                    bool isIp = this.IsIpAddress(subParameter[0]);

                    if (!isIp)
                        return false;

                    newPriceServer.IpServer = subParameter[0];

                    string[] subValueCheck = subParameter[1].Split('{');
                    int isPrimary, isRecive = 1;
                    bool isPri = int.TryParse(subValueCheck[0], out isPrimary);
                    if (!isPri)
                        return false; 

                    bool isRec = int.TryParse(subValueCheck[1], out isRecive);
                    if (!isRec)
                        return false;

                    if (isPrimary == 1)
                        newPriceServer.IsPrimary = true;

                    if (isRecive == 1)
                        newPriceServer.IsRecive = true;

                    newPriceServer.TimeConnect = DateTime.Now;

                    result.Add(newPriceServer);
                }
            }

            if (result != null)
            {
                if (Business.Market.IsMultipleQuote)
                    Business.Market.IsMultipleQuote = false;

                while (Business.Market.isBlock)
                    System.Threading.Thread.Sleep(200);

                Business.Market.MultiplePriceQuotes = result;

                Business.Market.IsMultipleQuote = true;
            }
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        internal bool UpdateTimeCheckMultipleQuotes(string value)
        {
            if (!Business.Market.IsMultipleQuote)
                Business.Market.IsMultipleQuote = true;

            while (Business.Market.isBlock)
                System.Threading.Thread.Sleep(200);

            int time = -1;
            bool isNum = int.TryParse(value, out time);

            if (!isNum)
            {
                Business.Market.IsMultipleQuote = true;
                return false;
            }
            
            Business.Market.TimeCheckMultiplePrice = time;

            Business.Market.IsMultipleQuote = true;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private bool IsIpAddress(string value)
        {
            IPAddress address;
            return IPAddress.TryParse(value, out address);
        }
    }    
}
