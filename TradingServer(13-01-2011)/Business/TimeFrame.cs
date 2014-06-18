using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class TimeFrame
    {
        public string Name { get; set; }
        public int Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //internal void IniTimeFrame()
        //{
        //    if (Business.Market.ListTimeFrame == null)
        //        Business.Market.ListTimeFrame = new List<TimeFrame>();

        //    this.Init();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //private void Init()
        //{            
        //    Business.TimeFrame newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "1M";
        //    newTimeFrame.Value = 1;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);

        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "5M";
        //    newTimeFrame.Value = 5;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);
                        
        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "15M";
        //    newTimeFrame.Value = 15;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);
                        
        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "30M";
        //    newTimeFrame.Value = 30;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);
                        
        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "1H";
        //    newTimeFrame.Value = 60;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);
                        
        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "4H";
        //    newTimeFrame.Value = 240;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);

        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "1D";
        //    newTimeFrame.Value = 1440;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);

        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "1W";
        //    newTimeFrame.Value = 10080;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);

        //    newTimeFrame = new TimeFrame();
        //    newTimeFrame.Name = "1MN";
        //    newTimeFrame.Value = 43200;
        //    Business.Market.ListTimeFrame.Add(newTimeFrame);
        //}
    }
}
