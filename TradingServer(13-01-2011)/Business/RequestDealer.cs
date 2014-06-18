using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class RequestDealer
    {
        public DateTime TimeClientRequest { get; set; }
        public int AgentID { get; set; }
        public DateTime TimeAgentReceive { get; set; }
        public bool FlagConfirm { get; set; }
        public int InvestorID { get; set; }
        public OpenTrade Request { get; set; }
        public string Notice { get; set; }
        public string Name { get; set; }
        public double MaxDev { get; set; }
        public string Answer { get; set; }
        public string AgentCode { get; set; }
        public DateTime TimeActive { get { return DateTime.Now; } }
        public string RequestString
        {
            get
            {
                return this.AgentID + "}" + this.FlagConfirm + "}" + this.MaxDev + "}" + this.Name + "}" + this.Notice
                                                + "}" + this.TimeAgentReceive + "}" + this.TimeClientRequest + "}" + this.Request.Investor.InvestorID
                                                + "}" + this.Request.Symbol.Name + "}" + this.Request.Size + "}" + this.Request.OpenTime
                                                + "}" + this.Request.OpenPrice + "}" + this.Request.StopLoss + "}" + this.Request.TakeProfit
                                                + "}" + this.Request.ClosePrice + "}" + this.Request.Commission + "}" + this.Request.Swap
                                                + "}" + this.Request.Profit + "}" + this.Request.ID + "}" + this.Request.ExpTime + "}" + this.Request.ClientCode
                                                + "}" + this.Request.IsHedged + "}" + this.Request.Type.ID + "}" + this.Request.Margin + "}" + this.Request.Investor.InvestorGroupInstance.Name
                                                + "}" + this.Request.Symbol.Digit + "}" + this.Request.Symbol.SpreadByDefault.ToString() 
                                                + "}" + this.Request.SpreaDifferenceInOpenTrade + "}" + this.Request.CommandCode 
                                                + "}" + this.Request.IsMultiClose + "}" + this.AgentCode + "}" + this.Answer + "}" + this.TimeActive;
            }
        }

        public string LogRequest
        {
            get
            {
                string result = "";
                Business.Tick onlineTick = new Tick();
                int digits = this.Request.Symbol.Digit;
                double Pip = this.Request.SpreaDifferenceInOpenTrade / Math.Pow(10, digits);
                onlineTick.Ask = Math.Round((this.Request.Symbol.TickValue.Ask + Pip), digits);
                onlineTick.Bid = this.Request.Symbol.TickValue.Bid;
                switch (this.Name.ToLower())
                {
                    case "open":
                        {
                            result = "open " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                            + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice)
                                                            + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss) + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                            + " (" + this.Request.MapPriceForDigit(onlineTick.Bid) + " / " + this.Request.MapPriceForDigit(onlineTick.Ask) + ")";
                            break;
                        }
                    case "closepending":
                        {
                            result = "delete #" + this.Request.CommandCode + " " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                            + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice) 
                                                            + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss)
                                                            + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                            + " (" + this.Request.MapPriceForDigit(onlineTick.Bid) + " / " + this.Request.MapPriceForDigit(onlineTick.Ask) + ")";
                            break;
                        }
                    case "openpending":
                        {
                            result = "open " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                           + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice)
                                                           + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss) + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                           + " (" + this.Request.MapPriceForDigit(onlineTick.Bid) + " / " + this.Request.MapPriceForDigit(onlineTick.Ask) + ")";
                            break;
                        }
                    case "update":
                        {
                            result = "modify #" + this.Request.CommandCode + " " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                             + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice) 
                                                             + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss)
                                                             + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                             + " (" + this.Request.MapPriceForDigit(onlineTick.Bid) + " / " + this.Request.MapPriceForDigit(onlineTick.Ask) + ")";
                            break;
                        }
                    case "updatepending":
                        {
                            result = "modify #" + this.Request.CommandCode + " " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                             + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice)
                                                             + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss)
                                                             + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                             + " (" + this.Request.MapPriceForDigit(onlineTick.Bid) + " / " + this.Request.MapPriceForDigit(onlineTick.Ask) + ")";
                            break;
                        }
                    case "close":
                        {
                            result = "close #" + this.Request.CommandCode + " " +Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " "  + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                             + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice) + " price close:"
                                                             + this.Request.MapPriceForDigit(this.Request.ClosePrice) + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss)
                                                             + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                             + " (" + this.Request.MapPriceForDigit(onlineTick.Bid) + " / " + this.Request.MapPriceForDigit(onlineTick.Ask) + ")";
                            break;
                        }
                }
                return result;
            }
        }
        public string LogRequestSuper(double low, double high)
        {            
                string result = "";
                switch (this.Name.ToLower())
                {
                    case "open":
                        {
                            result = "open " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                            + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice)
                                                            + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss) + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                            + " (" + this.Request.MapPriceForDigit(low) + " - " + this.Request.MapPriceForDigit(high) + ")";
                            break;
                        }
                    case "close":
                        {
                            result = "close #" + this.Request.CommandCode + " " + Facade.FacadeGetTypeCommand(this.Request.Type.ID) + " " + this.Request.FormatDoubleToString(this.Request.Size) + " symbol:"
                                                             + this.Request.Symbol.Name + " price open:" + this.Request.MapPriceForDigit(this.Request.OpenPrice) + " price close:"
                                                             + this.Request.MapPriceForDigit(this.Request.ClosePrice) + " sl:" + this.Request.MapPriceForDigit(this.Request.StopLoss)
                                                             + " tp:" + this.Request.MapPriceForDigit(this.Request.TakeProfit)
                                                             + " (" + this.Request.MapPriceForDigit(low) + " - " + this.Request.MapPriceForDigit(high) + ")";
                            break;
                        }
                }
                return result;          
        }
    }
}
