using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<string> GetCandlesByDate(string value)
        {
            List<string> result = new List<string>();

            string[] subValue = value.Split('|');
            string[] subParameter = subValue[0].Split('{');
            DateTime time = DateTime.Parse(subValue[1]);

            int day = DateTime.Now.Day - time.Day;
            for (int i = 0; i < subParameter.Length; i++)
            {
                switch (day)
                {
                    case 0:
                        if (Business.Market.CandlesByDate.ContainsKey(subParameter[i]))
                            result.Add(Business.Market.CandlesByDate[subParameter[i]]);
                        break;

                    case 1:
                        if(Business.Market.CandlesByDateOneDay.ContainsKey(subParameter[i]))
                            result.Add(Business.Market.CandlesByDateOneDay[subParameter[i]]);
                        break;

                    default:
                        if (Business.Market.CandlesByDateFiveDay.ContainsKey(subParameter[i]))
                            result.Add(Business.Market.CandlesByDateFiveDay[subParameter[i]]);
                        break;
                }
            }
            return result;
        }

    }
}
