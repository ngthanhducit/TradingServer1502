using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    class ConfirmationTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetConfirmation()
        {
            StringBuilder result = new StringBuilder();

            result.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            result.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
            result.Append("<head>");
            result.Append("<meta content='text/html; charset=utf-8' http-equiv='Content-Type'/>");
            result.Append("<title>Confirmation Mail</title>");
            result.Append("</head>");
            result.Append("<body style='font-family:Arial; font-size:12px;color:#333333;width:500px;margin-left:auto;margin-right:auto;'>");
            result.Append("<div id='header' style='width:100%;height:70px;border-bottom:2px #dddddd solid;border-top:2px #dddddd solid;'>");
            //result.Append("<img alt='MPF Logo' src='[#LinkLogo]' width='164' height='65'>");
            result.Append("<div><h3>EFXGM</h3></div>");
            result.Append("</div>");
            result.Append("<div id='content' style='background-color:white;width:90%;color:#333333;margin:15px;'>");
            result.Append("<b>Dear [#FullName]</b>,</br></br>");
            result.Append("<span style='color:#156a9d;font-weight:bold'>Your [#AccountType] has been activated !</span><br/><br/>");
            result.Append("Please, keep your login credentials for future access :<br/><br/>");
            result.Append("<table>");
            result.Append("<tr><td style='width:100px'>Username :</td><td style='color:#156a9d;font-weight:bold'>[#UserName]</td></tr>");
            result.Append("<tr><td style='width:100px'>Password :</td><td>[#Password]</td></tr>");
            result.Append("<tr><td style='width:100px'>Access :</td><td><a onmouseover='this.style.textDecoration='underline';this.style.color='blue'' onmouseout='this.style.textDecoration='none';this.style.color='#333333'' style='text-decoration:none;color:#333333;' href='[#Website]'>[#AccessLink]</a></td></tr>");
            result.Append("</table>");
            result.Append("<br/>");
            result.Append("<span style='color:#156a9d;font-weight:bold'>Best Regards,</span><br/>");
            result.Append("[#ServiceName]<br/>");
            result.Append("Contact us anytime: <a href='[#MailSupport]' onmouseover='this.style.textDecoration='underline';this.style.color='blue'' onmouseout='this.style.textDecoration='none';this.style.color='#333333'' style='text-decoration:none;color:#333333;'>[#EmailContact]</a><br/><br/>");
            result.Append("</div>");
            //result.Append("<div style='background-color:#f7f7f7;width:480px;border-top:2px #dddddd solid;border-bottom:2px #dddddd solid;padding:10px;'>");
            //result.Append("<span style='color:#156a9d;font-weight:bold'>[#CompanyName] </span>[#CompanyIntroduct]");
            //result.Append("</div>");
            result.Append("<div style='width:490px;padding:5px;font-size:10px;text-align:right'>");
            result.Append("© Copyright 2011. [#CompanyCopyright]");
            result.Append("</div>");
            result.Append("</body>");
            result.Append("</html>");


            return result;
        }
    }
}
