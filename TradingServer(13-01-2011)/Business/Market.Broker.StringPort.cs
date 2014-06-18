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
        /// <param name="cmd"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public string ExtractStringCommand(string cmd, string ipAddress)
        {
            string result = string.Empty;
            string Command = string.Empty;
            string Value = string.Empty;
            if (!string.IsNullOrEmpty(cmd))
            {
                string[] subValue = new string[2];

                int Position = -1;
                Position = cmd.IndexOf('$');
                if (Position > 0)
                {
                    Command = cmd.Substring(0, Position);
                    Value = cmd.Substring(Position + 1);

                    subValue[0] = Command;
                    subValue[1] = Value;
                }
                else
                {
                    subValue[0] = cmd;
                }

                if (subValue.Length > 0)
                {
                    switch (subValue[0])
                    {
                        #region BRANCH
                        case "BrA65100100": //Add Branch
                            {

                            }
                            break;

                        case "BrA69100105116":  //Edit Brach
                            {

                            }
                            break;

                        case "BrA68101108":  //Delete Brach
                            {

                            }
                            break;
                        #endregion

                        #region BRANCH ADMIN
                        case "BaD65100100": //Add Branch Admin
                            {

                            }
                            break;

                        case "BaD69100105116":  //Edit Branch Admin
                            {

                            }
                            break;

                        case "BaD68101108": //Delete Branch Admin
                            {

                            }
                            break;
                        #endregion

                        #region BROKER ADMIN
                        case "BkA65100100": //Add Broker Admin
                            {

                            }
                            break;

                        case "BkA69100105116":  //Edit Broker Admin
                            {

                            }
                            break;

                        case "BkA68101108": //Delete Broker Admin
                            {

                            }
                            break;
                        #endregion

                        #region SYMBOL
                        case "SyM65100100": //Add Symbol
                            {

                            }
                            break;

                        case "SyM69100105116":  //Edit Symbol
                            {

                            }
                            break;

                        case "SyM68101108": //Delete Symbol
                            {

                            }
                            break;
                        #endregion

                        #region IPINVESTOR GROUP
                        case "IiG65100100": //Add IP Invetor Group
                            {

                            }
                            break;

                        case "IiG69100105116":  //Edit IP Investor Group
                            {

                            }
                            break;

                        case "IiG68101108": //Delete IP Investor Group
                            {

                            }
                            break;
                        #endregion

                        #region INVESTOR GROUP
                        case "IvG65100100": //Add Investor Group
                            {

                            }
                            break;

                        case "IvG69100105116":  //Edit Investor Group
                            {

                            }
                            break;

                        case "IvG68101108": //Delete Investor Group
                            {

                            }
                            break;
                        #endregion

                        #region IP AGENT
                        case "IaG65100100": //Add IP Agent
                            {

                            }
                            break;

                        case "IaG69100105116":  //Edit IP Agent
                            {

                            }
                            break;

                        case "IaG68101108": //Delete IP Agent
                            {

                            }
                            break;
                        #endregion

                        #region IP AGENT INCOME
                        case "IaI65100100": //Add IP Agent InCome
                            {

                            }
                            break;

                        case "IaI69100105116":  //Edit IP Agent InCome
                            {

                            }
                            break;

                        case "IaI68101108": //Delete IP Agent InCome
                            {

                            }
                            break;
                        #endregion

                        #region IP AGENT INVESTOR
                        case "IaN65100100": //Add IP Agent Investor
                            {

                            }
                            break;

                        case "IaN69100105116":  //Edit IP Agent InCome
                            {

                            }
                            break;

                        case "IaN68101108": //Delete IP Agent InCome
                            {

                            }
                            break;
                        #endregion

                        #region INVESTOR
                        case "IvS65100100": //Add Investor
                            {

                            }
                            break;

                        case "IvS69100105116":  //Edit Investor
                            {

                            }
                            break;

                        case "IvS68101108": //Delete Investor
                            {

                            }
                            break;
                        #endregion

                        #region INCOME
                        case "InC65100100": //Add InCome
                            {

                            }
                            break;

                        case "InC69100105116":  //Edit InCome
                            {

                            }
                            break;

                        case "InC68101108": //Delete InCome
                            {

                            }
                            break;
                        #endregion

                        #region IP COMMISSION LOG
                        case "IcL65100100": //Add IP Commission Log
                            {

                            }
                            break;

                        case "IcL69100105116":  //Edit IP Commission Log
                            {

                            }
                            break;

                        case "IcL68101108": //Delete IP Commission Log
                            {

                            }
                            break;
                        #endregion

                        #region COMMAND HISTORY
                        case "ChS65100100": //Add Command History
                            {

                            }
                            break;

                        case "ChS69100105116":  //Edit Command History
                            {

                            }
                            break;

                        case "ChS68101108": //Delete Command Hisotyr
                            {

                            }
                            break;
                        #endregion
                    }
                }
            }

            return result;
        }

    }
}
