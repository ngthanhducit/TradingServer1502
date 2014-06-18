using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.IO;
using Ionic.Zip;

namespace TradingServer.Model
{
    public class TradingCalculate
    {
        private static bool flagRendCode = false;
        public static Model.TradingCalculate Instance = new TradingCalculate();
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal object StringToEnum(Type T, string Value)
        {
           foreach ( FieldInfo fi in T.GetFields() )
            if ( fi.Name == Value )
              return fi.GetValue( null );    // We use null because
                                             // enumeration values
                                             // are static

           throw new Exception(string.Format("Can't convert {0} to {1}", Value, T.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommandCode"></param>
        /// <returns></returns>
        internal string BuildCommandCode(string CommandCode)
        {
            string Result = string.Empty;
            while (CommandCode.Length < 8)
            {
                CommandCode = "0" + CommandCode;
            }
            return CommandCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        internal string BuildStringWithDigit(string value, int digit)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(value))
                return value;

            string[] subValue = value.Split('.');
            string sub = string.Empty;
            if (subValue.Length == 2)
            {
                sub = subValue[1];
                while (sub.Length < digit)
                {
                    sub += "0";
                }
            }
            else if (subValue.Length == 1)
            {
                sub = "";
                while (sub.Length < digit)
                {
                    sub += "0";
                }
            }

            if (!string.IsNullOrEmpty(sub))
                result = subValue[0] + "." + sub;
            else
                result = subValue[0];

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="tempOpenTrade"></param>
        /// <param name="size"></param>
        /// <param name="openPrice"></param>
        internal string ConvertTypeIDToString(int typeID)
        {
            string result = string.Empty;
            switch (typeID)
            {
                case 1:
                    {
                        result = "buy";
                    }
                    break;

                case 2:
                    {
                        result = "sell";
                    }
                    break;
                case 7:
                    {
                        result = "buy limit";
                    }
                    break;
                case 8:
                    {
                        result = "sell limit";
                    }
                    break;
                case 9:
                    {
                        result = "buy stop";
                    }
                    break;
                case 10:
                    {
                        result = "sell stop";
                    }
                    break;
                case 11:
                    {
                        result = "future buy";
                    }
                    break;
                case 12:
                    {
                        result = "future sell";
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        internal bool IsEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return false;
            return Regex.IsMatch(Email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool SendMail(string to, string subject, string content,Model.MailConfig config)
        {
            return false;
            try
            {
                string messageFrom = config.MessageFrom;
                string displayNameFrom = config.DisplayNameFrom;
                string userCredential = config.UserCredential;
                string passwordCredential = config.PasswordCredential;
                int smtpPort = config.SmtpPort;
                string smtpHost = config.SmtpHost;
                bool smtpEnabelSSL = config.EnableSSL;

                MailAddress ad = new MailAddress(to);
                MailAddress sender = new MailAddress(messageFrom);
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.From = new System.Net.Mail.MailAddress(messageFrom, displayNameFrom, System.Text.Encoding.UTF8);
                msg.To.Add(ad);
                msg.Sender = sender;
                msg.Subject = subject;
                msg.Body = content;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = config.EnableHTMLBody;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.EnableSsl = smtpEnabelSSL;
                client.Port = smtpPort;
                client.Host = smtpHost;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userCredential, passwordCredential);

                client.Send(msg);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="config"></param>
        public void SendMailAsync(string to, string subject, string content, Model.MailConfig config)
        {
            return;
            try
            {
                string messageFrom = config.MessageFrom;
                string displayNameFrom = config.DisplayNameFrom;
                string userCredential = config.UserCredential;
                string passwordCredential = config.PasswordCredential;
                int smtpPort = config.SmtpPort;
                string smtpHost = config.SmtpHost;
                bool smtpEnabelSSL = config.EnableSSL;

                MailAddress ad = new MailAddress(to);
                MailAddress sender = new MailAddress(messageFrom);
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.From = new System.Net.Mail.MailAddress(messageFrom, displayNameFrom, System.Text.Encoding.UTF8);
                msg.To.Add(ad);
                msg.Sender = sender;
                msg.Subject = subject;
                msg.Body = content;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = config.EnableHTMLBody;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.EnableSsl = smtpEnabelSSL;
                client.Port = smtpPort;
                client.Host = smtpHost;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userCredential, passwordCredential);

                client.SendCompleted += new SendCompletedEventHandler(sendMailComplete);

                client.SendAsync(msg, to);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sendMailComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string userState = (string)e.UserState;

            if (e.Error != null || e.Cancelled)
            {
                Business.Market.LogContentSendMail += "+ SEND REPORT TO EMAIL: " + userState + " UNCOMPLETE \r\n";
            }
            else
            {
                Business.Market.LogContentSendMail += "+ SEND REPORT TO EMAIL: " + userState + " COMPLETE \r\n";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="config"></param>
        public void SendMailMonthAsync(string to, string subject, string content, Model.MailConfig config)
        {
            return;
            try
            {
                string messageFrom = config.MessageFrom;
                string displayNameFrom = config.DisplayNameFrom;
                string userCredential = config.UserCredential;
                string passwordCredential = config.PasswordCredential;
                int smtpPort = config.SmtpPort;
                string smtpHost = config.SmtpHost;
                bool smtpEnabelSSL = config.EnableSSL;

                MailAddress ad = new MailAddress(to);
                MailAddress sender = new MailAddress(messageFrom);
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.From = new System.Net.Mail.MailAddress(messageFrom, displayNameFrom, System.Text.Encoding.UTF8);
                msg.To.Add(ad);
                msg.Sender = sender;
                msg.Subject = subject;
                msg.Body = content;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = config.EnableHTMLBody;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.EnableSsl = smtpEnabelSSL;
                client.Port = smtpPort;
                client.Host = smtpHost;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userCredential, passwordCredential);

                client.SendCompleted += new SendCompletedEventHandler(sendMailMonthComplete);

                client.SendAsync(msg, to);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sendMailMonthComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string userState = (string)e.UserState;

            if (e.Error != null || e.Cancelled)
            {
                Business.Market.LogContentSendMailMonth += "+ SEND REPORT TO EMAIL: " + userState + " UNCOMPLETE \r\n";
            }
            else
            {
                Business.Market.LogContentSendMailMonth += "+ SEND REPORT TO EMAIL: " + userState + " COMPLETE \r\n";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool SendMailAgent(string to, string subject, string content)
        {
            return false;
            try
            {
                Model.MailConfigAgent config = new MailConfigAgent();
                string messageFrom = config.MessageFrom;
                string displayNameFrom = config.DisplayNameFrom;
                string userCredential = config.UserCredential;
                string passwordCredential = config.PasswordCredential;
                int smtpPort = config.SmtpPort;
                string smtpHost = config.SmtpHost;
                bool smtpEnabelSSL = config.EnableSSL;

                MailAddress ad = new MailAddress(to);
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.From = new System.Net.Mail.MailAddress(messageFrom, displayNameFrom, System.Text.Encoding.UTF8);
                msg.To.Add(ad);
                msg.Sender = ad;
                msg.Subject = subject;
                msg.Body = content;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = config.EnableHTMLBody;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.EnableSsl = smtpEnabelSSL;
                client.Port = smtpPort;
                client.Host = smtpHost;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userCredential, passwordCredential);

                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asciiString"></param>
        /// <returns></returns>
        public string ConvertStringToHex(string asciiString)
        {
            string hex = string.Empty;
            foreach (char c in asciiString)
            {
                int temp = c;
                hex += string.Format("{0:x2}", (uint)System.Convert.ToUInt32(temp.ToString()));
            }

            return hex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns></returns>
        public string ConvertHexToString(string hexValue)
        {
            string strValue = string.Empty;
            while (hexValue.Length > 0)
            {
                strValue += System.Convert.ToChar(System.Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }

            return strValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetNextRandomCode()
        {
            while (TradingCalculate.flagRendCode)
            {
                System.Threading.Thread.Sleep(100);
            }

            TradingCalculate.flagRendCode = true;

            lock (Business.Market.syncObject)
            {
                string result = string.Empty;
                if (Business.Market.InvestorList != null)
                {
                    int index = 0;
                    int count = Business.Market.InvestorList.Count;

                    #region SORT INVESTOR
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = i + 1; j < count; j++)
                        {
                            int codeI = 0;
                            int codeJ = 0;
                            bool parseI = int.TryParse(Business.Market.InvestorList[i].Code, out codeI);
                            bool parseJ = int.TryParse(Business.Market.InvestorList[j].Code, out codeJ);

                            if (parseI && parseJ)
                            {
                                if (codeJ < codeI)
                                {
                                    Business.Investor temp = new Business.Investor();
                                    temp = Business.Market.InvestorList[i];
                                    Business.Market.InvestorList[i] = Business.Market.InvestorList[j];
                                    Business.Market.InvestorList[j] = temp;
                                }
                            }
                            else
                            {
                                if (parseJ)
                                {
                                    Business.Investor temp = new Business.Investor();
                                    temp = Business.Market.InvestorList[i];
                                    Business.Market.InvestorList[i] = Business.Market.InvestorList[j];
                                    Business.Market.InvestorList[j] = Business.Market.InvestorList[i];
                                }
                            }
                        }
                    }
                    #endregion

                    #region REND NEXT CODE
                    int countInvestor = Business.Market.InvestorList.Count;
                    if (countInvestor > 1)
                    {
                        #region COUNT INVESTOR LIST > 1
                        for (int j = 0; j < countInvestor; j++)
                        {
                            int codeIndexI = 0;
                            int codeIndexJ = 0;
                            bool parseIndexI = int.TryParse(Business.Market.InvestorList[j].Code, out codeIndexI);
                            int n = j + 1;
                            bool parseIndexJ = false;
                            if (n < countInvestor)
                            {
                                parseIndexJ = int.TryParse(Business.Market.InvestorList[j + 1].Code, out codeIndexJ);
                            }

                            if (parseIndexI && parseIndexJ)
                            {
                                int temp = codeIndexJ - codeIndexI;
                                if (temp > 1)
                                {
                                    int tempResult = codeIndexI + 1;
                                    result = tempResult.ToString();
                                    while (result.Length < 8)
                                    {
                                        result = "0" + result;
                                    }

                                    TradingCalculate.flagRendCode = false;

                                    return result;
                                }
                            }
                            else
                            {
                                if (parseIndexI)
                                {
                                    int tempResult = codeIndexI + 1;
                                    result = tempResult.ToString();
                                    while (result.Length < 8)
                                    {
                                        result = "0" + result;
                                    }

                                    TradingCalculate.flagRendCode = false;

                                    return result;
                                }
                            }

                            index++;
                        }
                        #endregion                        
                    }
                    else
                    {
                        if (countInvestor == 0) //count investor list == 0
                        {
                            int tempResult = 1;
                            result = tempResult.ToString();

                            while (result.Length < 8)
                            {
                                result = "0" + result;
                            }

                            TradingCalculate.flagRendCode = false;

                            return result;
                        }
                        else//count investor list == 1
                        {
                            #region COUNT INVESTOR LIST = 1
                            int codeI = 0;
                            int codeJ = 0;
                            bool parseJ = false;
                            parseJ = int.TryParse(Business.Market.InvestorList[0].Code, out codeJ);

                            if (parseJ)
                            {
                                int temp = codeJ - codeI;
                                if (temp > 1)
                                {
                                    int tempResult = codeI + 1;
                                    result = tempResult.ToString();
                                    while (result.Length < 8)
                                    {
                                        result = "0" + result;
                                    }

                                    TradingCalculate.flagRendCode = false;

                                    return result;
                                }
                                else
                                {
                                    int tempResult = codeJ + 1;
                                    result = tempResult.ToString();
                                    while (result.Length < 8)
                                    {
                                        result = "0" + result;
                                    }

                                    TradingCalculate.flagRendCode = false;

                                    return result;
                                }
                            }
                            else
                            {
                                int tempResult = codeI + 1;
                                result = tempResult.ToString();
                                while (result.Length < 8)
                                {
                                    result = "0" + result;
                                }

                                TradingCalculate.flagRendCode = false;

                                return result;
                            }
                            #endregion                            
                        }
                    }
                    #endregion
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="code"></param>
        /// <param name="timeFolder"></param>
        /// <param name="mode"></param>
        internal void StreamFile(string content, string code,DateTime timeFolder,int mode,string pathDirectory)
        {
            return;
            //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try
            {
                string folder = timeFolder.Year.ToString() + timeFolder.Month.ToString() + timeFolder.Day.ToString();
                string pathDic = pathDirectory + @"\" + folder;
                if (!Directory.Exists(pathDic))
                {
                    Directory.CreateDirectory(pathDic);
                }

                string path = string.Empty;

                if (mode == 0)
                {
                    string tempString = @"\" + code + ".html";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                else if (mode == 1)
                {
                    string tempString = @"\" + folder + ".txt";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write);

                StreamWriter fw = new StreamWriter(fs);
                fw.WriteLine(content);

                fw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="code"></param>
        /// <param name="timeFolder"></param>
        /// <param name="mode"></param>
        internal void StreamFile(string content, string code, DateTime timeFolder, int mode, string pathDirectory, bool isSCalper)
        {
            return;
            
            content = content.Replace("[ScalperName]", "");
            content = content.Replace("[Scalper]", "");

            try
            {
                string folder = timeFolder.Year.ToString() + timeFolder.Month.ToString() + timeFolder.Day.ToString();
                string pathDic = pathDirectory + @"\" + folder;
                if (!Directory.Exists(pathDic))
                {
                    Directory.CreateDirectory(pathDic);
                }

                string path = string.Empty;

                if (mode == 0)
                {
                    string tempString = @"\" + code + ".html";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                else if (mode == 1)
                {
                    string tempString = @"\" + folder + ".txt";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write);

                StreamWriter fw = new StreamWriter(fs);
                fw.WriteLine(content);

                fw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        internal void ZipFolder(string path,string fileName,string url)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(path);
                    zip.Save(url);
                    zip.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        internal void StreamManagerNotify(string content)
        {
            try
            {
                StreamWriter log;
                if (!File.Exists(@"D:\\ManagerAPI\mql.txt"))
                    log = new StreamWriter("mql.txt");
                else
                    log = File.AppendText(@"D:\\ManagerAPI\mql.txt");

                log.WriteLine(content);

                log.Close();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="code"></param>
        /// <param name="timeFolder"></param>
        /// <param name="mode"></param>
        internal void StreamMonthFile(string content, string code, DateTime timeFolder, int mode, string pathDirectory, bool isSCalper)
        {
            return;
            try
            {
                if (isSCalper)
                {
                    content = content.Replace("[ScalperName]", "Scalpe");
                    content = content.Replace("[Scalper]", "YES");
                }
                else
                {
                    content = content.Replace("[ScalperName]", "Scalpe");
                    content = content.Replace("[Scalper]", "NO");
                }

                string folder = timeFolder.Year.ToString() + timeFolder.Month.ToString();
                string pathDic = pathDirectory.ToString() + @"\" + folder;
                if (!Directory.Exists(pathDic))
                {
                    Directory.CreateDirectory(pathDic);
                }

                string path = string.Empty;

                if (mode == 0)
                {
                    string tempString = @"\" + code + ".html";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                else if (mode == 1)
                {
                    string tempString = @"\" + folder + ".txt";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write);

                StreamWriter fw = new StreamWriter(fs);
                fw.WriteLine(content);

                fw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="code"></param>
        /// <param name="timeFolder"></param>
        /// <param name="mode"></param>
        internal void StreamMonthFile(string content, string code, DateTime timeFolder, int mode, string pathDirectory)
        {
            return;
            try
            {
                string folder = timeFolder.Year.ToString() + timeFolder.Month.ToString();
                string pathDic = pathDirectory.ToString() + @"\" + folder;
                if (!Directory.Exists(pathDic))
                {
                    Directory.CreateDirectory(pathDic);
                }

                string path = string.Empty;

                if (mode == 0)
                {
                    string tempString = @"\" + code + ".html";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                else if (mode == 1)
                {
                    string tempString = @"\" + folder + ".txt";
                    path = pathDic + tempString;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write);

                StreamWriter fw = new StreamWriter(fs);
                fw.WriteLine(content);

                fw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        internal void StreamFileStatement(string content, string code,DateTime folderTime)
        {
            return;
            
            try
            {
                string folder = folderTime.Year.ToString() + folderTime.Month.ToString() + folderTime.Day.ToString();
                string pathDic = @"D:\" + folder;
                if (!Directory.Exists(pathDic))
                {
                    Directory.CreateDirectory(pathDic);
                }

                string tempString = @"\" + code + ".html";
                string path = pathDic + tempString;

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write);

                StreamWriter fw = new StreamWriter(fs);
                fw.WriteLine(content);

                fw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandTypeID"></param>
        /// <returns></returns>
        internal bool CheckIsPendingPosition(int commandTypeID)
        {
            bool isPendingPosition = false;
            if (commandTypeID == 7 || commandTypeID == 8 || commandTypeID == 9 || commandTypeID == 10 ||
                commandTypeID == 17 || commandTypeID == 18 || commandTypeID == 19 || commandTypeID == 20)
                isPendingPosition = true;

            return isPendingPosition;
        }

        /// <summary>
        /// Caculate Day Of Week
        /// </summary>
        /// <param name="newBarTick">BarTick newBarTick</param>
        /// <returns>DateTime</returns>
        internal DateTime CaculateDayOfWeek(DateTime Time)
        {
            DateTime end = new DateTime();
            try
            {
                DayOfWeek day = Time.DayOfWeek;
                int days = day - DayOfWeek.Monday;
                DateTime start = Time.AddDays(-days);
                end = start.AddDays(7);
                end = new DateTime(end.Year, end.Month, end.Day, 00, 00, 00);
            }   //End Try
            catch (Exception ex)
            {
                QuotesServer.QuotesFacade.AddApplicationError("Calculate Day Of Week", "CalculateDayOfWeek " + ex.ToString() + " " + DateTime.Now.ToString());
            }   //End Catch            

            return end;
        }   //End Function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal string GetFileInfo( DateTime from, DateTime to)
        {
            string path = "";
            if (Business.Market.MarketConfig != null)
            {
                int count = Business.Market.MarketConfig.Count;
                for (int i = 0; i < count; i++)
                {     
                    if (Business.Market.MarketConfig[i].Code == "C35")
                    {
                        path = Business.Market.MarketConfig[i].StringValue;
                        break;
                    }
                }
            }
            else
            {
                return "";
            }

            if (path == "") return "";

            List<Model.FileIn> list = new List<Model.FileIn>();
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo[] FilesList = dInfo.GetFiles();//can filter here with appropriate extentions

                foreach (FileInfo fi in FilesList)
                {
                    if (fi.LastWriteTime >= from && fi.LastWriteTime <= to)
                    {
                        list.Add(new Model.FileIn(fi.Name, fi.LastWriteTime, false, fi.Length, null));
                    }
                }
            }
            catch
            {
                return "";
            }
            return MapFileInfo(list);
        }

        /// <summary>
        //
        /// </summary>
        /// <param name="nameFiles"></param>
        /// <returns></returns>
        internal string GetContenFile(string nameFiles)
        {
            string path = "";
            if (Business.Market.MarketConfig != null)
            {
                int count = Business.Market.MarketConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C35")
                    {
                        path = Business.Market.MarketConfig[i].StringValue;
                        break;
                    }
                }
            }
            else
            {
                return "";
            }
            if (path == "") return "";
            List<Model.FileIn> list = new List<Model.FileIn>();
            try
            {                
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo[] FilesList = dInfo.GetFiles();//can filter here with appropriate extentions
                string[] nameArr = nameFiles.Split('{');
                for (int i = 0; i < nameArr.Length; i++)
                {
                    if (nameArr[i] != "")
                    {
                        string nameFile = nameArr[i];
                        foreach (FileInfo fi in FilesList)
                        {
                            if (fi.Name == nameFile)
                            {
                                list.Add(new Model.FileIn(fi.Name, fi.LastWriteTime, false, fi.Length, fi.OpenRead()));
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }

            return MapFileInfo(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool SaveFile(Model.FileIn file, string path)
        {
            return false;
            try
            {
                byte[] rebin = Convert.FromBase64String(file.FileContent);
                using (FileStream fs2 = new FileStream(path + "/" + file.FileName, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs2))
                    bw.Write(rebin);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listFile"></param>
        /// <returns></returns>
        internal string MapFileInfo(List<Model.FileIn> listFile)
        {
            string result = "";
            for (int i = 0; i < listFile.Count; i++)
            {
                result += listFile[i].FileName + "{" + listFile[i].DateTime.Ticks + "{" + listFile[i].Size + "{" + listFile[i].FileContent + "}";
            }
            return result;
        }
    }
}
