using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class ManagerAPITick
    {
        //int upDown, string time, double bid, double ask, double high, double low, double spread
        public string Symbol { get; set; }
        private int _upDown;
        public int UpDown
        {
            get { return this._upDown; }
            set
            {
                this._upDown = value;
                if (this._upDown == 1)
                    this.IsUp = "up";
                else
                    this.IsUp = "down";
            }
        }

        private string _isUp;
        public string IsUp { get; set; }
        public string Time { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Spread { get; set; }
    }
}
