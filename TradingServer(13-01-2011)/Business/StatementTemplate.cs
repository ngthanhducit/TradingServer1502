using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    class StatementTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOpenPosition"></param>
        /// <param name="listClosePosition"></param>
        /// <param name="listPendingPosition"></param>
        /// <returns></returns>
        public StringBuilder GetStatementReport(List<Business.OpenTrade> listOpenPosition, List<Business.OpenTrade> listClosePosition, List<Business.OpenTrade> listPendingPosition, Business.Investor account, DateTime timeEndDay)
        {
            StringBuilder result = new StringBuilder();

            try
            {   
                Business.ReportItem openPosition = new ReportItem();
                Business.ReportItem closePosition = new ReportItem();
                StringBuilder pendingPosition = new StringBuilder();

                result = this.GetStatementTemplate();
                openPosition = this.RendOpenPosition(listOpenPosition);
                closePosition = this.RendClosePosition(listClosePosition);
                pendingPosition = this.RendPendingPosition(listPendingPosition);

                result.Replace("<tr><td>[#ClosePosition]</td></tr>", closePosition.Content.ToString());
                result.Replace("<tr><td>[#OpenPosition]</td></tr>", openPosition.Content.ToString());
                result.Replace("<tr><td>[#LimitStopOrder]</td></tr>", pendingPosition.ToString());

                result.Replace("[LoginCode]", account.Code);
                result.Replace("[LoginName]", account.NickName);
                result.Replace("[Currency]", "USD");

                string serverName = string.Empty;
                string serverType = string.Empty;
                if (Business.Market.MarketConfig != null)
                {
                    int countMarketConfig = Business.Market.MarketConfig.Count;
                    for (int j = 0; j < countMarketConfig; j++)
                    {
                        if (Business.Market.MarketConfig[j].Code == "C01")
                        {
                            serverName = Business.Market.MarketConfig[j].StringValue;
                        }

                        if (Business.Market.MarketConfig[j].Code == "C02")
                        {
                            serverType = Business.Market.MarketConfig[j].StringValue;
                        }
                    }
                }

                result.Replace("[HeaderStatement]", "Daily Statement - " + serverName);
                result.Replace("[CompanyName]", serverName + " " + serverType);

                //company name get in admin(Because Need test)
                //result.Replace("[CompanyName]", Business.Market.CompanyName);

                double equity = 0;
                double balance = 0;

                //double previousLedgerBalance = TradingServer.Facade.FacadeGetPreviousLedgerBalance(account.InvestorID);
                double previousLedgerBalance = 0;

                #region NEW CODE(22/05/2012) GET PREVIOUSLEDGERBALANCE = LAST BALANCE IN COMMAND HISTORY WITH TYPE = 21
                TradingServer.Business.OpenTrade LastBalance = null;
                if (timeEndDay.DayOfWeek == DayOfWeek.Monday)
                {
                    DateTime tempTimeStart = timeEndDay.AddDays(-3);
                    DateTime timeStartLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 00, 00, 00);
                    DateTime timeEndLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 23, 59, 59);

                    LastBalance = TradingServer.Facade.FacadeGetLastBalanceByInvestor(account.InvestorID, timeStartLastBalance, 21, timeEndLastBalance);
                }
                else
                {
                    if (timeEndDay.DayOfWeek == DayOfWeek.Sunday)
                    {
                        DateTime tempTimeStart = timeEndDay.AddDays(-2);
                        DateTime timeStartLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 00, 00, 00);
                        DateTime timeEndLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 23, 59, 59);

                        LastBalance = TradingServer.Facade.FacadeGetLastBalanceByInvestor(account.InvestorID, timeStartLastBalance, 21, timeEndLastBalance);
                    }
                    else
                    {
                        DateTime tempTimeStart = timeEndDay.AddDays(-1);
                        DateTime timeStartLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 00, 00, 00);
                        DateTime timeEndLastBalance = new DateTime(tempTimeStart.Year, tempTimeStart.Month, tempTimeStart.Day, 23, 59, 59);

                        LastBalance = TradingServer.Facade.FacadeGetLastBalanceByInvestor(account.InvestorID, timeStartLastBalance, 21, timeEndLastBalance);
                    }
                }

                if (LastBalance != null)
                    previousLedgerBalance = LastBalance.Profit;
                #endregion

                balance = Math.Round(previousLedgerBalance + closePosition.TotalProfit +
                    closePosition.Deposit - closePosition.Withdrawls, 2);

                equity = Math.Round(balance + openPosition.TotalProfit + account.Credit, 2);

                result.Replace("[PreviousLedger]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(previousLedgerBalance.ToString(), 2));
                result.Replace("[Withdrawal]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(closePosition.Withdrawls.ToString(), 2));
                result.Replace("[Deposit]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(closePosition.Deposit.ToString(), 2));
                result.Replace("[Equity]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(equity.ToString(), 2));
                result.Replace("[Unrealized]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(openPosition.TotalProfit, 2).ToString(), 2));
                result.Replace("[Realized]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(closePosition.TotalProfit.ToString(), 2));
                result.Replace("[Margin]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit((account.Margin + account.FreezeMargin).ToString(), 2));
                result.Replace("[CreditIn]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(closePosition.CreditIn, 2).ToString(), 2));
                result.Replace("[CreditOut]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(closePosition.CreditOut, 2).ToString(), 2));
                //result.Replace("[Swap]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(openPosition.Swap, 2).ToString(), 2));
                result.Replace("[Credit]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(account.Credit, 2).ToString(), 2));

                double freeMargin = equity - (account.Margin + account.FreezeMargin);

                result.Replace("[FreeMargin]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(freeMargin, 2).ToString(), 2));

                double tempTotalMargin = (account.Margin + account.FreezeMargin);

                double marginLevel = 0;

                if (tempTotalMargin > 0)
                    marginLevel = (equity * 100) / tempTotalMargin;

                if (marginLevel != 0)
                    result.Replace("[MarginLevel]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(marginLevel, 2).ToString(), 2));
                else
                    result.Replace("[MarginLevel]", "-");

                result.Replace("[Balance]", TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(balance.ToString(), 2));

                account.PreviousLedgerBalance = balance;
            }
            catch (Exception ex)
            {
                
            }
                
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetStatementTemplate()
        {
            StringBuilder result = new StringBuilder();
            
            result.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/1999/REC-html401-19991224'>");
            result.Append("<html xmlns:msxsl='urn:schemas-microsoft-com:xslt' xmlns:ms='urn:schemas-microsoft-com:xslt' xmlns:dt='urn:schemas-microsoft-com:datatypes' xmlns:ext='urn:xstl-ext'>");
            result.Append("<head>");
            result.Append("<META http-equiv='Content-Type' content='text/html; charset=utf-16'>");
            result.Append("<title>[HeaderStatement]</title>");
            result.Append("</head>");
            result.Append("<body style='margin:0px 25px 0px 25px'>");
            result.Append("<table style='font-size:12px;font-family:Arial;' width='100%'>");
            result.Append("<tr>");
            result.Append("<td align='right'>");
            result.Append("<div style='font-size:30px;font-weight:bold;margin-top:30px;font-family:Arial;'>[CompanyName]</div>");
            result.Append("</td>");
            result.Append("</tr>");
            result.Append("<tr align='left'>");
            result.Append("<td>");
            result.Append("<table style='width:100%;border-top:1px black solid;border-bottom:1px black solid;margin-top:20px;padding:0px'>");
            result.Append("<tr style='background-color:#EEEEEE;'>");
            result.Append("<td style='width:auto;font-weight:bolder;'>Account</td>");
            result.Append("<td style='width:auto;font-weight:bolder;'>Name</td>");
            result.Append("<td style='width:auto;font-weight:bolder;'>Currency</td>");
            result.Append("<td style='width:auto;font-weight:bolder;'>Date</td>");
            result.Append("<td style='width:auto;font-weight:bolder;'>[ScalperName]</td>");
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append("<td style='width:auto'>[LoginCode]</td>");
            result.Append("<td style='width:auto'>[LoginName]</td>");
            result.Append("<td style='width:auto'>[Currency]</td>");
            result.Append("<td style='width:auto'>[ReportDay]</td>");
            result.Append("<td style='width:auto'>[Scalper]</td>");
            result.Append("</tr>");
            result.Append("</table>");
            result.Append("</td>");
            result.Append("</tr>");

            result.Append("<tr align='left'>");
            result.Append("<td>");
            result.Append("<table style='width:100%; margin-top:30px;' cellspacing='0' cellpadding='3'>");
            result.Append("<tr>");
            result.Append("<td colspan='13' style='font-size:16px;font-family:Arial;height:20px' valign='top' align='right'><strong>CLOSED POSITIONS </strong></td>");
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:65px;'><strong>#Ticket</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:115px;'><strong>Time</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:90px;'><strong>Symbol</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:60px;'><strong>Volume</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:85px;'><strong>Type</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:95px;'><strong>Open Price</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:75px;'><strong>T/P</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:75px;'><strong>S/L</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:70px;'><strong>Pips</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:95px;'><strong>Close Price</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:115px'><strong>Close Time</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:70px;'><strong>Swap</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:85px;'><strong>Commissions</strong></td>");            
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>USD</strong></td>");
            result.Append("</tr>");

            result.Append("<tr><td>[#ClosePosition]</td></tr>");

            result.Append("</table>");
            result.Append("</td>");
            result.Append("</tr>");

            result.Append("<tr align='left'>");
            result.Append("<td>");
            result.Append("<table style='width:100%; margin-top:30px;' cellspacing='0' cellpadding='3'>");
            result.Append("<tr>");
            result.Append("<td colspan='13' style='font-size:16px;font-family:Arial;height:20px' valign='top' align='right'><strong>OPEN POSITIONS</strong></td>");
            result.Append("</tr>");
            result.Append("<tr>");

            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:65px;'><strong>#Ticket</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:115px;'><strong>Time</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:90px;'><strong>Symbol</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:60px;'><strong>Volume</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:85px;'><strong>Type</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:95px;'><strong>Open Price</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:75px;'><strong>T/P</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:75px;'><strong>S/L</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:70px;'><strong>Pips</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:95px;'><strong>Close Price</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:115px'><strong>Close Time</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:70px;'><strong>Swap</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;width:85px;'><strong>Commissions</strong></td>");            
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>USD</strong></td>");
            result.Append("</tr>");

            result.Append("<tr><td>[#OpenPosition]</td></tr>");

            //result.Append("</table>");
            //result.Append("</td>");
            //result.Append("</tr>");

            result.Append("<tr align='left'>");
            result.Append("<td>");
            result.Append("<table style='width:100%; margin-top:30px;' cellspacing='0' cellpadding='3'>");
            result.Append("<tr>");
            result.Append("<td colspan='13' style='font-size:16px;font-family:Arial;height:20px' valign='top' align='right'><strong> LIMIT/STOP ORDERS </strong></td>");
            result.Append("</tr>");
            result.Append("<tr>");

            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>#Ticket</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Time</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Symbol</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Volume</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Type</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Entry</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Expire</strong></td>");
            result.Append("<td style='border-top:1px solid black;border-bottom:1px solid black;text-align:center;background-color:#EEEEEE;font-size:13px;font-family:Arial;'><strong>Price</strong></td>");
            result.Append("</tr>");

            result.Append("<tr><td>[#LimitStopOrder]</td></tr>");

            result.Append("</table>");
            result.Append("</td>");
            result.Append("</tr>");

            result.Append("<tr align='left'>");
            result.Append("<td>");
            result.Append("<table style='width:100%; margin-top:30px; border:1px black solid' cellspacing='0' cellpadding='3'>");
            result.Append("<tr>");
            result.Append("<td colspan='10' style='height:20px;font-size:15px;font-weight:bold;background-color:#DBDBDB;border-bottom:2px black solid' align='right'> ACCOUNT SUMMARY </td>");
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Previous Ledger:</td>");
            result.Append("<td><strong>[PreviousLedger]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Equity:</td>");
            result.Append("<td><strong>[Equity]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Margin:</td>");
            result.Append("<td><strong>[Margin]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td><strong>Balance:</strong></td>");
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Withdrawal:</td>");
            result.Append("<td><strong>[Withdrawal]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Unrealized P&amp;L:</td>");
            result.Append("<td><strong>[Unrealized]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Free Margin:</td>");
            result.Append("<td><strong>[FreeMargin]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>"); 
            result.Append("<td rowspan='2'><strong><span style='font-size:20px'>[Balance]</span></strong></td>");
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom: 1px black solid'>Deposit:</td>");
            result.Append("<td><strong>[Deposit]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom: 1px black solid'>Realized P&amp;L:</td>");
            result.Append("<td><strong>[Realized]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;border-bottom: 1px black solid'>Margin Level:</td>");
            result.Append("<td><strong>[MarginLevel]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("</tr>");

            result.Append("<tr>");
            result.Append("<td style='background-color:#DBDBDB;'>Credit In:</td>");
            result.Append("<td><strong>[CreditIn]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;'>Credit Out:</td>");
            result.Append("<td><strong>[CreditOut]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("<td style='background-color:#DBDBDB;'>Credit</td>");
            result.Append("<td><strong>[Credit]</strong></td>");
            result.Append("<td style='width:10%'>&nbsp;</td>");
            result.Append("</tr>");

            result.Append("</table>");
            result.Append("</td>");
            result.Append("</tr>");
            result.Append("</table>");
            result.Append("<br /><br />");
            result.Append("</body>");
            result.Append("</html>");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOpenPosition"></param>
        /// <returns></returns>
        public Business.ReportItem RendOpenPosition(List<Business.OpenTrade> listOpenPosition)
        {
            Business.ReportItem reportData = new ReportItem();
            StringBuilder result = new StringBuilder();
            double commission = 0;
            double swap = 0;
            double profitLoss = 0;
            double unrealize = 0;

            if (listOpenPosition != null && listOpenPosition.Count > 0)
            {
                int count = listOpenPosition.Count;
                for (int i = 0; i < count; i++)
                {                   
                    result.Append("<tr>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + "#" + listOpenPosition[i].CommandCode + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        listOpenPosition[i].OpenTime.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + listOpenPosition[i].Symbol.Name + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                         TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].Size.ToString(), 2) + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Facade.FacadeConvertTypeIDToName(listOpenPosition[i].Type.ID) + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].OpenPrice.ToString(), listOpenPosition[i].Symbol.Digit) + "</td>");

                    if (listOpenPosition[i].TakeProfit > 0)
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].TakeProfit.ToString(), listOpenPosition[i].Symbol.Digit) + "</td>");
                    else
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");

                    if (listOpenPosition[i].StopLoss > 0)
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].StopLoss.ToString(), listOpenPosition[i].Symbol.Digit) + "</td>");
                    else
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");

                    double closePrice = listOpenPosition[i].ClosePrice;
                    double spread = closePrice - listOpenPosition[i].OpenPrice;
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(spread, listOpenPosition[i].Symbol.Digit).ToString(), listOpenPosition[i].Symbol.Digit) + "</td>");

                    //ADD NEW TWO COLUMN(CLOSE PRICE AND CLOSE TIME)
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].ClosePrice.ToString(), listOpenPosition[i].Symbol.Digit) +
                        "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                        
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].Swap.ToString(), 2) + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listOpenPosition[i].Commission.ToString(), 2) + "</td>");

                    result.Append("<td style='border-bottom:1px solid Gray;text-align:right;background-color:#EEEEEE;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listOpenPosition[i].Profit,2).ToString(), 2) + "</td>");
                    result.Append("</tr>");

                    commission += listOpenPosition[i].Commission;
                    swap += listOpenPosition[i].Swap;
                    profitLoss += listOpenPosition[i].Profit;
                }

                unrealize = Math.Round(profitLoss + swap + commission, 2);
                //unrealize = Math.Round(profitLoss + commission, 2);

                result.Append("<tr style='text-align:center;background-color:#EEEEEE;font-family:Arial;'>");
                result.Append("<td colspan='11'>&nbsp;</td>");
                result.Append("<td>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(swap, 2).ToString(), 2) + "</td>");
                result.Append("<td>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(commission, 2).ToString(), 2) + "</td>");
                result.Append("<td align='right'>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(profitLoss, 2).ToString(), 2) + "</td>");
                result.Append("</tr>");
                result.Append("<tr>");
                result.Append("<td colspan='14'>");
                result.Append("<table cellspacing='0' cellpadding='3' align='right' style='width:30%;background-color:#DBDBDB'>");
                result.Append("<tr>");
                result.Append("<td><strong>Unrealized P&amp;L:</strong></td>");
                result.Append("<td align='right'><strong>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(unrealize, 2).ToString(), 2) + "</strong></td>");
                result.Append("</tr>");
                result.Append("</table>");
                result.Append("</td>");
                result.Append("</tr>");
            }
            else
            {
                result.Append("<tr><td style='border-bottom:1px solid black;text-align:center;font-family:Arial;' colspan='14'>-No transactions-</td></tr>");
            }

            result.Append("</table>");
            result.Append("</td>");
            result.Append("</tr>");

            reportData.Comission = commission;
            reportData.Content = result;
            reportData.Profit = profitLoss;
            reportData.Swap = swap;
            reportData.TotalProfit = unrealize;

            return reportData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listClosePosition"></param>
        /// <returns></returns>
        public Business.ReportItem RendClosePosition(List<Business.OpenTrade> listClosePosition)
        {
            Business.ReportItem reportData = new ReportItem();

            StringBuilder result = new StringBuilder();
            double commission = 0;
            double swap = 0;
            double totalProfit = 0;
            double realizedPL = 0;
            double deposit = 0;
            double withdrawal = 0;
            double creditIn = 0;
            double creditOut = 0;

            if (listClosePosition != null && listClosePosition.Count > 0)
            {
                int count = listClosePosition.Count;
                for (int i = 0; i < count; i++)
                {
                    if (listClosePosition[i].Type.ID == 21)
                        continue;

                    result.Append("<tr>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + "#" + listClosePosition[i].CommandCode + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + 
                        listClosePosition[i].OpenTime.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");

                    if (listClosePosition[i].Type.ID == 13 || listClosePosition[i].Type.ID == 14 || listClosePosition[i].Type.ID == 15 || listClosePosition[i].Type.ID == 16)
                    {
                        if (listClosePosition[i].Type.ID == 13)
                            deposit += listClosePosition[i].Profit;

                        if (listClosePosition[i].Type.ID == 14)
                            withdrawal += listClosePosition[i].Profit;

                        if (listClosePosition[i].Type.ID == 15)
                            creditIn += listClosePosition[i].Profit;

                        if (listClosePosition[i].Type.ID == 16)
                            creditOut += listClosePosition[i].Profit;

                        #region REND TEMPLATE DEPOSIT
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'></td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'></td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Facade.FacadeConvertTypeIDToName(listClosePosition[i].Type.ID) + "</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'></td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'></td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'></td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'></td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>0.00</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>0.00</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:right;background-color:#EEEEEE;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].Profit, 2).ToString(), 2) + "</td>");
                        result.Append("</tr>");
                        #endregion
                    }
                    else
                    {
                        #region REND CLOSE POSITION
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + listClosePosition[i].Symbol.Name + "</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listClosePosition[i].Size.ToString(), 2) + "</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Facade.FacadeConvertTypeIDToName(listClosePosition[i].Type.ID) + "</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].OpenPrice, listClosePosition[i].Symbol.Digit).ToString(),
                            listClosePosition[i].Symbol.Digit) + "</td>");

                        if (listClosePosition[i].TakeProfit > 0)
                            result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                                TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].TakeProfit, listClosePosition[i].Symbol.Digit).ToString(),
                                listClosePosition[i].Symbol.Digit) + "</td>");
                        else
                            result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");

                        if (listClosePosition[i].StopLoss > 0)
                            result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                                TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].StopLoss, listClosePosition[i].Symbol.Digit).ToString(),
                                listClosePosition[i].Symbol.Digit) + "</td>");
                        else
                            result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");

                        //ADD NEW COLUMN PIP
                        double spread = listClosePosition[i].ClosePrice - listClosePosition[i].OpenPrice;

                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(spread, listClosePosition[i].Symbol.Digit).ToString(), listClosePosition[i].Symbol.Digit) + "</td>");

                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].ClosePrice, listClosePosition[i].Symbol.Digit).ToString(),
                            listClosePosition[i].Symbol.Digit) + "</td>");
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + 
                            listClosePosition[i].CloseTime.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");

                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].Swap, 2).ToString(), 2) + "</td>");

                        result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].Commission, 2).ToString(), 2) + "</td>"); 
                       
                        result.Append("<td style='border-bottom:1px solid Gray;text-align:right;background-color:#EEEEEE;font-family:Arial;'>" +
                            TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listClosePosition[i].Profit, 2).ToString(), 2) + "</td>");
                        result.Append("</tr>");

                        commission += listClosePosition[i].Commission;
                        swap += listClosePosition[i].Swap;
                        totalProfit += listClosePosition[i].Profit;  
                        #endregion                                             
                    }                    
                }

                realizedPL = Math.Round(totalProfit + commission + swap, 2);
                //realizedPL = Math.Round(totalProfit + commission, 2);

                result.Append("<tr style='text-align:center;background-color:#EEEEEE;font-family:Arial;'>");
                result.Append("<td colspan='11'>&nbsp;</td>");                
                result.Append("<td>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(swap, 2).ToString(), 2) + "</td>");
                result.Append("<td>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(commission, 2).ToString(), 2) + "</td>");
                result.Append("<td align='right'>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(totalProfit, 2).ToString(), 2) + "</td>");
                result.Append("</tr>");
                result.Append("<tr>");
                result.Append("<td colspan='14'>");
                result.Append("<table cellspacing='0' cellpadding='3' align='right' style='width:30%;background-color:#DBDBDB'>");
                result.Append("<tr>");
                result.Append("<td><strong>Realized P&amp;L:</strong></td>");
                result.Append("<td align='right'><strong>" + TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(realizedPL.ToString(), 2) + "</strong></td>");
                result.Append("</tr>");
                result.Append("</table>");
                result.Append("</td>");
                result.Append("</tr>");
            }
            else
            {
                result.Append("<tr><td style='border-bottom:1px solid black;text-align:center;font-family:Arial;' colspan='14'>-No transactions-</td></tr>");
            }

            reportData.Comission = commission;
            reportData.Content = result;
            reportData.Profit = totalProfit;
            reportData.Swap = swap;
            reportData.TotalProfit = realizedPL;
            reportData.Deposit = deposit;
            reportData.Withdrawls = withdrawal;
            reportData.CreditIn = creditIn;
            reportData.CreditOut = creditOut;

            return reportData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listPendingPositin"></param>
        /// <returns></returns>
        public StringBuilder RendPendingPosition(List<Business.OpenTrade> listPendingPositin)
        {
            StringBuilder result = new StringBuilder();

            if (listPendingPositin != null && listPendingPositin.Count > 0)
            {
                int count = listPendingPositin.Count;
                for (int i = 0; i < count; i++)
                {
                    result.Append("<tr>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + "#" + listPendingPositin[i].CommandCode + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + listPendingPositin[i].OpenTime.ToString("MM/dd/yyyy HH:mm:ss")  + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + listPendingPositin[i].Symbol.Name + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(listPendingPositin[i].Size.ToString(), 2) + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Facade.FacadeConvertTypeIDToName(listPendingPositin[i].Type.ID) + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>-</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" + listPendingPositin[i].ExpTime + "</td>");
                    result.Append("<td style='border-bottom:1px solid Gray;text-align:center;font-family:Arial;'>" +
                        TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Math.Round(listPendingPositin[i].OpenPrice, listPendingPositin[i].Symbol.Digit).ToString(), 
                        listPendingPositin[i].Symbol.Digit) + "</td>");
                    result.Append("</tr>");
                }
            }
            else
            {
                result.Append("<tr><td style='border-bottom:2px solid black;text-align:center;font-family:Arial;' colspan='8'>-No transactions-</td></tr>");                
            }

            return result;
        }
    }
}
