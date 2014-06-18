using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class Tick
    {
        private string _strBid, _strAsk, _strHighInDay, _strLowInDay, _strTickTime, _strTimeCurrent, _strHighAsk, _strLowAsk;
        public int ID { get; set; }
        public double Ask { get; set; }
        public string StrAsk
        {
            get { return this._strAsk; }
            set
            {
                this._strAsk = value;
                double num;
                double.TryParse(this._strAsk, out num);
                this.Ask = num;
            }
        }

        public double Bid { get; set; }
        public string StrBid
        {
            get { return this._strBid; }
            set
            {
                this._strBid = value;
                double num;
                double.TryParse(this._strBid, out num);
                this.Bid = num;
            }
        }
        public bool IsUpdate { get; set; }
        public string SymbolName { get; set; }
        public DateTime TickTime { get; set; }
        public string StrTickTime
        {
            get { return this._strTickTime; }
            set
            {
                this._strTickTime = value;
                DateTime time;
                DateTime.TryParse(this._strTickTime, out time);
                this.TickTime = time;
            }
        }
        public DateTime TimeCurrent { get; set; }
        public string StrTimeCurrent
        {
            get { return this._strTimeCurrent; }
            set
            {
                this._strTimeCurrent = value;
                DateTime time;
                DateTime.TryParse(this._strTimeCurrent, out time);
                this.TimeCurrent = time;
            }
        }
        public string Status { get; set; }
        public int SymbolID { get; set; }
        public double HighInDay { get; set; }
        public string StrHighInDay
        {
            get { return this._strHighInDay; }
            set
            {
                this._strHighInDay = value;
                double num;
                double.TryParse(this._strHighInDay, out num);
                this.HighInDay = num;
            }
        }
        public double LowInDay { get; set; }
        public string StrLowInDay
        {
            get { return this._strLowInDay; }
            set
            {
                this._strLowInDay = value;
                double num;
                double.TryParse(this._strLowInDay, out num);
                this.LowInDay = num;
            }
        }
        public double HighAsk { get; set; }
        public string StrHighAsk
        {
            get { return this._strHighAsk; }
            set
            {
                this._strHighAsk = value;
                double num;
                double.TryParse(this._strHighAsk, out num);
                this.HighAsk = num;
            }
        }
        public double LowAsk { get; set; }
        public string StrLowAsk
        {
            get { return this._strLowAsk; }
            set
            {
                this._strLowAsk = value;
                double num;
                double.TryParse(this._strLowAsk, out num);
                this.LowAsk = num;
            }
        }
        public bool IsManager { get; set; }
    }
}
