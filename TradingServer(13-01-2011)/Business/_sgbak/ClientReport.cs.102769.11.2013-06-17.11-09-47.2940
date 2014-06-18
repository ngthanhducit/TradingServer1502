using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    class ClientReport
    {
        private StringBuilder strBuild=new StringBuilder();
        private double sumProfit, sumDeposite, sumCredit, sumCommission, sumSwap;
        private double onlineProfit;
        private Investor account;

        private StringBuilder strStatement = new StringBuilder();
        double withdrawal, deposit, unrealized, realized, marginRequirement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="historyCommand"></param>
        /// <param name="onlineCommand"></param>
        /// <param name="pendingorder"></param>
        /// <returns></returns>
        public string RendReport(Investor account,List<OpenTrade> historyCommand, List<OpenTrade> onlineCommand, List<OpenTrade> pendingorder)
        {
            this.account = account;
            this.strBuild.Clear();
            this.sumDeposite=0;
            this.sumProfit=0;
            this.sumCredit=0;
            this.sumSwap=0;
            this.sumCommission=0;
            this.onlineProfit=0;

            this.strBuild.Append("<table width='100%' cellpadding='1' cellspacing='1' style=font-size:11px;font-family:Arial;><tr>");
            this.strBuild.Append("<td>Account Code: <b>" + this.account.Code + "</b></td><td>Name: <b>" + this.account.NickName + "</b></td><td>" + DateTime.Now + "</td></tr></table>");            
            if(historyCommand!=null)
            {
                this.strBuild.Append("<b>Closed Transactions:</>");
                this.RendHistoryReport(historyCommand);                
            }
            
            if(onlineCommand!=null)
            {
                this.strBuild.Append("<b>Open Trade</b>");
                this.RendOlineCmdReport(onlineCommand);
            }

            if(pendingorder!=null)
            {
                this.strBuild.Append("<b>Pending Order</b>");
                this.RendPendingOrder(pendingorder);
            }

            this.RendSummaryTb();

            return this.strBuild.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyCommand"></param>
        private void RendHistoryReport(List<OpenTrade> historyCommand)
        {
            
            //begin table
            this.strBuild.Append("<table width='100%' cellpadding='1' cellspacing='1' style='font-size:11px;font-family:Arial;'>");
            //table header
            this.strBuild.Append("<tr style='background-Color:#c0c0c0'><th>Ticket</th><th>Open Time</th><th>Style</th><th>Size</th><th>Symbol</th><th>Open Price</th><th>StopLoss</th><th>TakeProfit</th><th>Close Time</th><th>Close Price</th><th>Commission</th><th>Swap</th><th>Profit</th></tr>");

            #region build report body
            bool isAlter=false;
            int count = historyCommand.Count;
            for (int i = 0; i < count; i++) 
            {
                if(isAlter)
                {
                    this.strBuild.Append("<tr>");
                }
                else
                {
                    this.strBuild.Append("<tr style='background-Color:#e0e0e0'>");
                }
                isAlter=!isAlter;

                int typeID=historyCommand[i].Type.ID;
                string typeName = TradingServer.Facade.FacadeGetTypeNameByTypeID(historyCommand[i].Type.ID);
                if (typeID==13||typeID==14||typeID==15||typeID==16)
                {                    
                    //add balance item
                    this.strBuild.Append("<td>" + historyCommand[i].CommandCode + "</td>"
                        + "<td>" + historyCommand[i].OpenTime + "</td>"
                        + "<td>balance</td>"                        
                        + "<td colspan='9'>" + typeName + "</td>"
                        + "<td>" + historyCommand[i].Profit + "</td>"
                        );
                }
                else
                {
                    //open trade item
                    
                    this.strBuild.Append("<td>"+historyCommand[i].CommandCode+"</td>"
                        +"<td>"+historyCommand[i].OpenTime+"</td>"
                        +"<td>"+ typeName +"</td>"
                        +"<td>"+historyCommand[i].Size+"</td>"
                        +"<td>"+historyCommand[i].Symbol.Name+"</td>"
                        +"<td>"+historyCommand[i].OpenPrice+"</td>"
                        +"<td>"+historyCommand[i].StopLoss+"</td>"
                        +"<td>"+historyCommand[i].TakeProfit+"</td>"
                        +"<td>"+historyCommand[i].CloseTime+"</td>"
                        +"<td>"+historyCommand[i].ClosePrice+"</td>"
                        +"<td>"+historyCommand[i].Commission+"</td>"
                        +"<td>"+historyCommand[i].Swap+"</td>"
                        +"<td>"+historyCommand[i].Profit+"</td>"
                        );
                }

                this.strBuild.Append("</tr>");
                
                this.sumCommission+=historyCommand[i].Commission;
                this.sumSwap+=historyCommand[i].Swap;
                if(typeID==13||typeID==14)
                {
                    this.sumDeposite+=historyCommand[i].Profit;
                    continue;
                }

                if(typeID==15||typeID==16)
                {
                    this.sumCredit+=historyCommand[i].Profit;
                    continue;
                }

                this.sumProfit+=historyCommand[i].Profit;
            }

            #endregion

            //summart row
            this.strBuild.Append("<tr><td colspan=10></td><td>"+this.sumCommission+"</td><td>"+this.sumSwap+"</td><td>"+this.sumProfit+"</td></tr>");

            //end table
            this.strBuild.Append("</table>");
            
            //summary close account
            this.strBuild.Append("<table style='font-weight:bold;margin-top:20px'><tr><td>Deposit/Withdrawal: " + this.sumDeposite + "</td><td>Credit: " + this.sumCredit + "</td><td>Close Trade: " + (this.sumProfit + this.sumSwap + this.sumCommission) + "</td></tr></table>");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlineCommand"></param>
        private void RendOlineCmdReport(List<OpenTrade> onlineCommand)
        {
            double profit=0;
            double commission=0;
            double swap=0;
            //begin table
            this.strBuild.Append("<table width='100%' cellpadding='1' cellspacing='1' style='font-size:11px;font-family:Arial;'>");
            //table header
            this.strBuild.Append("<tr style='background-Color:#c0c0c0'><th>Ticket</th><th>Open Time</th><th>Style</th><th>Size</th><th>Symbol</th><th>Open Price</th><th>StopLoss</th><th>TakeProfit</th><th>Close Time</th><th>Close Price</th><th>Commission</th><th>Swap</th><th>Profit</th></tr>");

            #region build report body
            bool isAlter=false;
            int count = onlineCommand.Count;
            for (int i = 0; i < count; i++)
            {
                if(isAlter)
                {
                    this.strBuild.Append("<tr>");
                }
                else
                {
                    this.strBuild.Append("<tr style='background-Color:#e0e0e0'>");
                }
                isAlter=!isAlter;

                int typeID=onlineCommand[i].Type.ID;
                
                //open trade item
                string typeName = TradingServer.Facade.FacadeGetTypeNameByTypeID(onlineCommand[i].Type.ID);
                this.strBuild.Append("<td>" + onlineCommand[i].CommandCode + "</td>"
                    + "<td>" + onlineCommand[i].OpenTime + "</td>"
                    + "<td>" + typeName + "</td>"
                    + "<td>" + onlineCommand[i].Size + "</td>"
                    + "<td>" + onlineCommand[i].Symbol.Name + "</td>"
                    + "<td>" + onlineCommand[i].OpenPrice + "</td>"
                    + "<td>" + onlineCommand[i].StopLoss + "</td>"
                    + "<td>" + onlineCommand[i].TakeProfit + "</td>"
                    + "<td></td>"
                    + "<td>" + onlineCommand[i].ClosePrice + "</td>"
                    + "<td>" + onlineCommand[i].Commission + "</td>"
                    + "<td>" + onlineCommand[i].Swap + "</td>"
                    + "<td>" + onlineCommand[i].Profit + "</td>"
                    );                

                this.strBuild.Append("</tr>");

                swap+=onlineCommand[i].Swap;
                profit+=onlineCommand[i].Profit;
                commission+=onlineCommand[i].Commission;
            }

            #endregion
            this.onlineProfit=profit+swap+commission;
            //end table
            this.strBuild.Append("</table>");
            this.strBuild.Append("<table style='font-weight:bold;margin-top:20px'><tr><td>Profit: "+this.onlineProfit+"</td></tr></table>");
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pendingOrder"></param>
        private void RendPendingOrder(List<OpenTrade> pendingOrder)
        {
            
            //begin table
            this.strBuild.Append("<table width='100%' cellpadding='1' cellspacing='1' style='font-size:11px;font-family:Arial;'>");
            //table header
            this.strBuild.Append("<tr style='background-Color:#c0c0c0'><th>Ticket</th><th>Open Time</th><th>Style</th><th>Size</th><th>Symbol</th><th>Open Price</th><th>StopLoss</th><th>TakeProfit</th><th>Online Time</th><th>Online Price</th></tr>");

            #region build report body
            bool isAlter=false;
            int count = pendingOrder.Count;
            for (int i = 0; i < count; i++)
            {
                if(isAlter)
                {
                    this.strBuild.Append("<tr>");
                }
                else
                {
                    this.strBuild.Append("<tr style='background-Color:#e0e0e0'>");
                }

                isAlter=!isAlter;

                int typeID=pendingOrder[i].Type.ID;
                
                //open trade item
                string typeName = TradingServer.Facade.FacadeGetTypeNameByTypeID(typeID);
                this.strBuild.Append("<td>" + pendingOrder[i].CommandCode + "</td>"
                    + "<td>" + pendingOrder[i].OpenTime + "</td>"
                    + "<td>" + typeName + "</td>"
                    + "<td>" + pendingOrder[i].Size + "</td>"
                    + "<td>" + pendingOrder[i].Symbol.Name + "</td>"
                    + "<td>" + pendingOrder[i].OpenPrice + "</td>"
                    + "<td>" + pendingOrder[i].StopLoss + "</td>"
                    + "<td>" + pendingOrder[i].TakeProfit + "</td>"
                    + "<td>" + pendingOrder[i].CloseTime + "</td>"
                    + "<td>" + pendingOrder[i].ClosePrice + "</td>"
                    );

                this.strBuild.Append("</tr>");
            }
            #endregion
            //end table
            this.strBuild.Append("</table>");
        }

        /// <summary>
        /// 
        /// </summary>
        private void RendSummaryTb()
        {
            this.account.PreviousLedgerBalance = this.account.Balance - (this.sumProfit + this.sumSwap + this.sumCommission);

            this.strBuild.Append("<table width='100%' cellpadding='1' cellspacing='1' style='font-size:11px;font-family:Arial;'><tr><td colspan=4><td><b>A/C Summary</b></td></td></tr>");
            this.strBuild.Append("<tr><td>Previous Ledger Balance:</td><td>" + this.account.PreviousLedgerBalance + "</td></tr>");
            this.strBuild.Append("<tr><td>Account Balance</td><td>" + this.account.Balance + "</td><td>Online Profit:</td><td>" + this.onlineProfit + "</td></tr>");
            this.strBuild.Append("<tr><td>Close Trade</td><td>" + (this.sumProfit+this.sumSwap+this.sumCommission) + "</td><td>Total Credit:</td><td>" + this.sumCredit + "</td></tr>");
            this.strBuild.Append("<tr><td>Deposit:</td><td>" + this.sumDeposite + "</td><td>Online Equity:</td><td>" + this.account.Equity + "</td></tr>");
            this.strBuild.Append("<tr><td>Online Balance</td><td>" + this.account.Balance + "</td><td>Online Margin Requirement:</td><td>" + this.account.Margin + "</td></tr>");
            this.strBuild.Append("<tr><td></td><td></td><td>Online Free Margin:</td><td>" + this.account.FreeMargin + "</td></tr>");
            this.strBuild.Append("</table>");
        }

        //==================REND STATEMENT OF ACCOUNT=================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="historyCommand"></param>
        /// <param name="onlineCommand"></param>
        /// <param name="pendingOrder"></param>
        /// <returns></returns>
        public string RendStatement(Business.Investor account, List<Business.OpenTrade> historyCommand, List<Business.OpenTrade> onlineCommand, List<Business.OpenTrade> pendingOrder)
        {
            this.strStatement.Clear();
            this.account = account;
            //this.RendStyleStatement();

            if (historyCommand != null)
            {
                this.RendHistoryStatement(historyCommand);
            }

            if (onlineCommand != null)
            {
                this.RendOpenStatement(onlineCommand);
            }

            if (pendingOrder != null)
            {
                this.RendLimitStop(pendingOrder);
            }

            if (account != null)
            {
                this.RendAccount(account);
            }

            return this.strStatement.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string RendStyleStatement()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/1999/REC-html401-19991224'>");
            result.Append("<html xmlns:msxsl='urn:schemas-microsoft-com:xslt' xmlns:ms='urn:schemas-microsoft-com:xslt' xmlns:dt='urn:schemas-microsoft-com:datatypes' xmlns:ext='urn:xstl-ext'>");
            result.Append("<head>");
            result.Append("<META http-equiv='Content-Type' content='text/html; charset=utf-16'>");
            result.Append("<title>Daily Statement - Millenium Penata Futures</title>");
            result.Append("<style type='text/css'>");
            result.Append("body {");
            result.Append("color:#000000;");
            result.Append("background-color:#FFFFFF;");
            result.Append("font-family: Arial;} ");

            result.Append(".statementHeader {");
            result.Append("font-size: 30px;");
            result.Append("font-weight: bold;");
            result.Append("margin-top: 30px; } ");

            result.Append(".textStyle{");
            result.Append("font-size: 12px; } ");

            result.Append(".partHeader {");
            result.Append("font-size: 16px; } ");

            result.Append(".tableHeader {");
            result.Append("border-top: 1px solid black;");
            result.Append("border-bottom: 1px solid black;");
            result.Append("text-align: center;");
            result.Append("background-color:#EEEEEE;");
            result.Append("font-size: 13px; } ");

            result.Append(".rowStyle {");
            result.Append("border-bottom: 1px solid Gray;");
            result.Append("text-align: center; } ");

            result.Append(".emptyRowStyle{");
            result.Append("border-bottom: 1px solid Gray;");
            result.Append("text-align: center; } ");

            result.Append(".rowStyleProfit {");
            result.Append("border-bottom: 1px solid Gray;");
            result.Append("text-align: right;");
            result.Append("background-color:#EEEEEE; } ");

            result.Append(".noTransaction{");
            result.Append("border-bottom: 2px solid black;");
            result.Append("text-align: center; } ");

            result.Append(".subSummary {");
            result.Append("text-align: center;");
            result.Append("background-color:#EEEEEE; } ");

            result.Append("</style>");
            result.Append("</head>");
            result.Append("<body style='margin:0px 25px 0px 25px' onload='init()'>");
            result.Append("<table class='textStyle' width='100%'>");

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listHistory"></param>
        private void RendHistoryStatement(List<Business.OpenTrade> listHistory)
        {
            #region DEFAULT HTML
            this.strStatement.Append("<tr align='left'>");
            this.strStatement.Append("<td>");
            this.strStatement.Append("<table style='width: 100%; margin-top:30px;' cellspacing='0' cellpadding='3'>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td colspan='13' class='partHeader' style='height:20px' valign='top' align='right'><strong>CLOSED POSITIONS </strong></td>");
            this.strStatement.Append("</tr>");

            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td class='tableHeader'><strong>#Ticket</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Open Time</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Symbol</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Volume</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Type</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Open Price</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>T/P</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>S/L</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Close Price</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Close Time</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Comm.</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Swap</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>USD</strong></td>");
            this.strStatement.Append("</tr>");
            #endregion

            if (listHistory != null && listHistory.Count > 0)
            {
                double totalComm = 0;
                double totalSwap = 0;
                double totalProfit = 0;

                int count = listHistory.Count;
                for (int i = 0; i < count; i++)
                {
                    int typeID = listHistory[i].Type.ID;
                    string typeName = TradingServer.Facade.FacadeGetTypeNameByTypeID(typeID);
                    string TP = string.Empty;
                    if (listHistory[i].TakeProfit == 0)
                        TP = "-";
                    else
                        TP = listHistory[i].TakeProfit.ToString();

                    string SL = string.Empty;
                    if (listHistory[i].StopLoss == 0)
                        SL = "-";
                    else
                        SL = listHistory[i].StopLoss.ToString();


                    #region BUILD HISTORY COMMAND
                    if (typeID == 13 || typeID == 14 || typeID == 15 || typeID == 16)
                    {
                        if (typeID == 14)
                            withdrawal += Math.Round(listHistory[i].Profit, 2);

                        if (typeID == 13)
                            deposit += Math.Round(listHistory[i].Profit, 2);

                        this.strStatement.Append("<tr>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].CommandCode + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].OpenTime + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>-</td>");
                        this.strStatement.Append("<td class='rowStyle'>-</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + typeName + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>-</td>");
                        this.strStatement.Append("<td class='emptyRowStyle'>-</td>");
                        this.strStatement.Append("<td class='emptyRowStyle'>-</td>");
                        this.strStatement.Append("<td class='rowStyle'>-</td>");
                        this.strStatement.Append("<td class='rowStyle'>-</td>");
                        this.strStatement.Append("<td class='rowStyle'>0.00</td>");
                        this.strStatement.Append("<td class='rowStyle'>0.00</td>");
                        this.strStatement.Append("<td class='rowStyleProfit'>" + listHistory[i].Profit + "</td>");
                        this.strStatement.Append("</tr>");
                    }
                    else
                    {
                        totalComm += Math.Round(listHistory[i].Commission, 2);
                        totalSwap += Math.Round(listHistory[i].Swap, 2);
                        totalProfit += Math.Round(listHistory[i].Profit, 2);

                        this.strStatement.Append("<tr >");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].CommandCode + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].OpenTime + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].Symbol.Name + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].Size + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + typeName + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].OpenPrice + "</td>");
                        this.strStatement.Append("<td class='emptyRowStyle'>" + TP + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + SL + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].ClosePrice + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].CloseTime + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].Commission + "</td>");
                        this.strStatement.Append("<td class='rowStyle'>" + listHistory[i].Swap + "</td>");
                        this.strStatement.Append("<td class='rowStyleProfit'>" + listHistory[i].Profit + "</td>");
                        this.strStatement.Append("</tr>");
                    }
                    #endregion

                }

                this.realized = Math.Round((totalProfit + totalComm + totalSwap), 2);

                #region BUILD HTML SUMMARY CLOSE POSITION
                this.strStatement.Append("<tr class='subSummary'>");
                this.strStatement.Append("<td colspan='10'>&nbsp;</td>");
                this.strStatement.Append("<td>" + Math.Round(totalComm, 2) + "</td>");
                this.strStatement.Append("<td>" + Math.Round(totalSwap, 2) + "</td>");
                this.strStatement.Append("<td align='right'>" + Math.Round(totalProfit, 2) + "</td>");
                this.strStatement.Append("</tr>");

                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td colspan='13'>");
                this.strStatement.Append("<table cellspacing='0' cellpadding='3' align='right' style='width:30%;background-color:#DBDBDB'>");
                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td><strong>Realized P&amp;L:</strong></td>");
                this.strStatement.Append("<td align='right'><strong>" + Math.Round(this.realized, 2) + "</strong></td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
                #endregion
            }
            else
            {
                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td class='noTransaction' colspan='13'>-No transactions-</td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOpenTrade"></param>
        private void RendOpenStatement(List<Business.OpenTrade> listOpenTrade)
        {
            #region DEFAULT HTML
            this.strStatement.Append("<tr align='left'>");
            this.strStatement.Append("<td>");
            this.strStatement.Append("<table style='width: 100%; margin-top:30px;' cellspacing='0' cellpadding='3'>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td colspan='13' class='partHeader' style='height:20px' valign='top' align='right'><strong>OPEN POSITIONS </strong></td>");
            this.strStatement.Append("</tr>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td class='tableHeader'><strong>#Ticket</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Time</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Symbol</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Volume</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Type</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Open Price</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>T/P</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>S/L</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Pips</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Swap</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Commissions</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>USD</strong></td>");
            this.strStatement.Append("</tr>");
            #endregion

            if (listOpenTrade != null && listOpenTrade.Count > 0)
            {
                double totalSwap = 0;
                double totalCommission = 0;
                double totalProfit = 0;

                #region BUILD OPEN POSITION
                int count = listOpenTrade.Count;
                for (int i = 0; i < count; i++)
                {
                    string typeName = TradingServer.Facade.FacadeGetTypeNameByTypeID(listOpenTrade[i].Type.ID);

                    string TP = string.Empty;
                    if (listOpenTrade[i].TakeProfit == 0)
                        TP = "-";
                    else
                        TP = listOpenTrade[i].TakeProfit.ToString();

                    string SL = string.Empty;
                    if (listOpenTrade[i].StopLoss == 0)
                        SL = "-";
                    else
                        SL = listOpenTrade[i].StopLoss.ToString();

                    totalCommission += listOpenTrade[i].Commission;
                    totalSwap += listOpenTrade[i].Swap;
                    totalProfit += listOpenTrade[i].Profit;                    

                    this.strStatement.Append("<tr>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].CommandCode + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].OpenTime + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].Symbol.Name + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].Size + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + typeName + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].OpenPrice + "</td>");
                    this.strStatement.Append("<td class='emptyRowStyle'>" + TP + "</td>");
                    this.strStatement.Append("<td class='emptyRowStyle'>" + SL + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>-128</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].Swap + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listOpenTrade[i].Commission + "</td>");
                    this.strStatement.Append("<td class='rowStyleProfit'>" + listOpenTrade[i].Profit + "</td>");
                    this.strStatement.Append("</tr>");
                }
                #endregion

                this.unrealized = Math.Round(totalProfit + totalCommission + totalSwap, 2);                

                #region BUILD HTML SUMMARY OPEN POSITION
                this.strStatement.Append("<tr class='subSummary'>");
                this.strStatement.Append("<td colspan='9'>&nbsp;</td>");
                this.strStatement.Append("<td>" + totalSwap + "</td>");
                this.strStatement.Append("<td>" + totalCommission + "</td>");
                this.strStatement.Append("<td align='right'>" + totalProfit + "</td>");
                this.strStatement.Append("</tr>");

                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td colspan='12'>");
                this.strStatement.Append("<table cellspacing='0' cellpadding='3' align='right' style='width:30%;background-color:#DBDBDB'>");
                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td><strong>Unrealized P&amp;L:</strong></td>");
                this.strStatement.Append("<td align='right'><strong>" + this.unrealized + "</strong></td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
                #endregion
            }
            else
            {
                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td class='noTransaction' colspan='12'>-No transactions-</td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listLimitStop"></param>
        private void RendLimitStop(List<Business.OpenTrade> listLimitStop)
        {
            #region BUILD DEFAULT HTML
            this.strStatement.Append("<tr align='left'>");
            this.strStatement.Append("<td>");
            this.strStatement.Append("<table style='width: 100%; margin-top:30px;' cellspacing='0' cellpadding='3'>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td colspan='13' class='partHeader' style='height:20px' valign='top' align='right'><strong> LIMIT/STOP ORDERS </strong></td>");
            this.strStatement.Append("</tr>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td class='tableHeader'><strong>#Ticket</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Time</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Symbol</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Volume</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Type</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Entry</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Expire</strong></td>");
            this.strStatement.Append("<td class='tableHeader'><strong>Price</strong></td>");
            this.strStatement.Append("</tr>");
            #endregion

            if (listLimitStop != null && listLimitStop.Count > 0)
            {
                int count = listLimitStop.Count;
                for (int i = 0; i < count; i++)
                {
                    string typeName = TradingServer.Facade.FacadeGetTypeNameByTypeID(listLimitStop[i].Type.ID);

                    this.strStatement.Append("<tr>");
                    this.strStatement.Append("<td class='rowStyle'>" + listLimitStop[i].CommandCode + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listLimitStop[i].OpenTime + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listLimitStop[i].Symbol.Name + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listLimitStop[i].Size + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + typeName + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>Entry</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listLimitStop[i].ExpTime + "</td>");
                    this.strStatement.Append("<td class='rowStyle'>" + listLimitStop[i].OpenPrice + "</td>");
                    this.strStatement.Append("</tr>");
                }
            }
            else
            {
                this.strStatement.Append("<tr>");
                this.strStatement.Append("<td class='noTransaction' colspan='8'>-No transactions-</td>");
                this.strStatement.Append("</tr>");
                this.strStatement.Append("</table>");
                this.strStatement.Append("</td>");
                this.strStatement.Append("</tr>");
            }

            #region REND END HTML
            this.strStatement.Append("</table>");
            this.strStatement.Append("</td>");
            this.strStatement.Append("</tr>");
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        private void RendAccount(Business.Investor account)
        {
            double displayBalance = account.PreviousLedgerBalance + this.realized + this.deposit - this.withdrawal;
            double equity = displayBalance + this.unrealized + account.Credit;
            
            #region BUILD DEFAULT HTML
            this.strStatement.Append("<tr align='left'>");
            this.strStatement.Append("<td>");
            this.strStatement.Append("<table style='width: 100%; margin-top:30px; border:1px black solid' cellspacing='0' cellpadding='3'>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td colspan='10' style='height:20px;font-size:15px;font-weight:bold;background-color:#DBDBDB;border-bottom:2px black solid' align='right'> ACCOUNT SUMMARY </td>");
            this.strStatement.Append("</tr>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Previous Ledger:</td>");
            this.strStatement.Append("<td><strong>" + account.PreviousLedgerBalance);
            this.strStatement.Append("</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Equity:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(equity) + "</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Margin:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(account.Margin, 2) + "</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td><strong>Balance:</strong></td>");
            this.strStatement.Append("</tr>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Withdrawal:</td>");
            this.strStatement.Append("<td><strong>0.00</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Unrealized P&amp;L:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(this.unrealized, 2) + "</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;border-bottom:1px black solid'>Free Margin:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(account.FreeMargin, 2) + "</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td rowspan='2'><strong><span style='font-size:20px'>" + displayBalance);
            this.strStatement.Append("</span></strong></td>");
            this.strStatement.Append("</tr>");
            this.strStatement.Append("<tr>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;'>Deposit:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(this.deposit,2) + "</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;'>Realized P&amp;L:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(this.realized,2) + "</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("<td style='background-color:#DBDBDB;'>Margin Level:</td>");
            this.strStatement.Append("<td><strong>" + Math.Round(account.MarginLevel, 2) + "%</strong></td>");
            this.strStatement.Append("<td style='width:10%'>&nbsp;</td>");
            this.strStatement.Append("</tr>");
            this.strStatement.Append("</table>");
            this.strStatement.Append("</td>");
            this.strStatement.Append("</tr>");
            #endregion

            //set Previous Ledger Balance
            this.account.PreviousLedgerBalance = displayBalance;
        }
    }
}
