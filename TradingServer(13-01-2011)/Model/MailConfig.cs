using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Model
{
    public class MailConfig
    {
        public bool isEnable { get; set; }
        public string UserCredential { get; set; }
        public string PasswordCredential { get; set; }
        public string MessageFrom { get; set; }
        public string DisplayNameFrom { get; set; }
        public string AttachFile { get; set; }
        public int SmtpPort { get { return 25; } }
        //public int SmtpPort { get { return 587; } }
        public string SmtpHost { get; set; }
        public bool EnableSSL { get { return false; } }
        public bool EnableHTMLBody { get { return true; } }
        public string Signature { get; set; }
    }
}
