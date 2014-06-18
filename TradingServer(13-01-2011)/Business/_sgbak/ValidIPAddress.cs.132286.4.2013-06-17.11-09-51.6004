using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class ValidIPAddress
    {        
        private static ValidIPAddress validIPAddress;
        public static ValidIPAddress Instance
        {
            get
            {
                if (ValidIPAddress.validIPAddress == null)
                    ValidIPAddress.validIPAddress = new ValidIPAddress();

                return ValidIPAddress.validIPAddress;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>   
        /// <returns></returns>
        internal bool AddIpBlock(string ip, int investorID)
        {   
            int a, b, c, d, e;

            string[] subValue = ip.Split('.');
            if (subValue.Length != 4)
                return false;

            a = investorID;

            if (!int.TryParse(subValue[0], out b) || b > 255)
                return false;

            if (!int.TryParse(subValue[1], out c) || c > 255)
                return false;

            if (!int.TryParse(subValue[2], out d) || d > 255)
                return false;

            if (!int.TryParse(subValue[3], out e) || e > 255)
                return false;

            if (a >= Business.Market.BlockIpAddress.Count)
            {
                for (int i = Business.Market.BlockIpAddress.Count; i < a + 1; i++)
                {
                    Business.Market.BlockIpAddress.Add(null);
                }

                Business.Market.BlockIpAddress[a] = new List<List<List<List<bool>>>>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a] == null)
                    Business.Market.BlockIpAddress[a] = new List<List<List<List<bool>>>>();
            }

            if (b >= Business.Market.BlockIpAddress[a].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a].Count; i < b + 1; i++)
                {
                    Business.Market.BlockIpAddress[a].Add(null);
                }

                Business.Market.BlockIpAddress[a][b] = new List<List<List<bool>>>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a][b] == null)
                    Business.Market.BlockIpAddress[a][b] = new List<List<List<bool>>>();
            }

            if (c >= Business.Market.BlockIpAddress[a][b].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a][b].Count; i < c + 1; i++)
                {
                    Business.Market.BlockIpAddress[a][b].Add(null);
                }

                Business.Market.BlockIpAddress[a][b][c] = new List<List<bool>>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a][b][c] == null)
                    Business.Market.BlockIpAddress[a][b][c] = new List<List<bool>>();
            }

            if (d >= Business.Market.BlockIpAddress[a][b][c].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a][b][c].Count; i < d + 1; i++)
                {
                    Business.Market.BlockIpAddress[a][b][c].Add(null);
                }

                Business.Market.BlockIpAddress[a][b][c][d] = new List<bool>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a][b][c][d] == null)
                    Business.Market.BlockIpAddress[a][b][c][d] = new List<bool>();
            }

            if (e >= Business.Market.BlockIpAddress[a][b][c][d].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a][b][c][d].Count; i < e + 1; i++)
                {
                    Business.Market.BlockIpAddress[a][b][c][d].Add(false);
                }

                Business.Market.BlockIpAddress[a][b][c][d][e] = true;
            }
            else
            {
                Business.Market.BlockIpAddress[a][b][c][d][e] = true;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="investorID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal bool AddIpBlock(string ip, int investorID,string key)
        {
            int a, b, c, d, e;

            string[] subValue = ip.Split('.');
            if (subValue.Length != 4)
                return false;

            a = investorID;

            if (!int.TryParse(subValue[0], out b) || b > 255)
                return false;

            if (!int.TryParse(subValue[1], out c) || c > 255)
                return false;

            if (!int.TryParse(subValue[2], out d) || d > 255)
                return false;

            if (!int.TryParse(subValue[3], out e) || e > 255)
                return false;

            if (a >= Business.Market.BlockIpAddress.Count)
            {
                for (int i = Business.Market.BlockIpAddress.Count; i < a + 1; i++)
                {
                    Business.Market.BlockIpAddress.Add(null);
                }

                Business.Market.BlockIpAddress[a] = new List<List<List<List<bool>>>>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a] == null)
                    Business.Market.BlockIpAddress[a] = new List<List<List<List<bool>>>>();
            }

            if (b >= Business.Market.BlockIpAddress[a].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a].Count; i < b + 1; i++)
                {
                    Business.Market.BlockIpAddress[a].Add(null);
                }

                Business.Market.BlockIpAddress[a][b] = new List<List<List<bool>>>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a][b] == null)
                    Business.Market.BlockIpAddress[a][b] = new List<List<List<bool>>>();
            }

            if (c >= Business.Market.BlockIpAddress[a][b].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a][b].Count; i < c + 1; i++)
                {
                    Business.Market.BlockIpAddress[a][b].Add(null);
                }

                Business.Market.BlockIpAddress[a][b][c] = new List<List<bool>>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a][b][c] == null)
                    Business.Market.BlockIpAddress[a][b][c] = new List<List<bool>>();
            }

            if (d >= Business.Market.BlockIpAddress[a][b][c].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a][b][c].Count; i < d + 1; i++)
                {
                    Business.Market.BlockIpAddress[a][b][c].Add(null);
                }

                Business.Market.BlockIpAddress[a][b][c][d] = new List<bool>();
            }
            else
            {
                if (Business.Market.BlockIpAddress[a][b][c][d] == null)
                    Business.Market.BlockIpAddress[a][b][c][d] = new List<bool>();
            }

            if (e >= Business.Market.BlockIpAddress[a][b][c][d].Count)
            {
                for (int i = Business.Market.BlockIpAddress[a][b][c][d].Count; i < e + 1; i++)
                {
                    Business.Market.BlockIpAddress[a][b][c][d].Add(false);
                }

                Business.Market.BlockIpAddress[a][b][c][d][e] = true;
            }
            else
            {
                Business.Market.BlockIpAddress[a][b][c][d][e] = true;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>-1: Invalid IP | 0: False no Exists | 1:True Exists</returns>
        internal int CheckIp(string ip,int investorID)
        {
            int a, b, c, d, e;
            string[] subValue = ip.Split('.');
            if (subValue.Length != 4)
                return -1;

            a = investorID;
                        
            if (!int.TryParse(subValue[0], out b) || b > 255)
                return -1;

            if (!int.TryParse(subValue[1], out c) || c > 255)
                return -1;

            if (!int.TryParse(subValue[2], out d) || d > 255)
                return -1;

            if (!int.TryParse(subValue[3], out e) || e > 255)
                return -1;

            if (a >= Business.Market.BlockIpAddress.Count || Business.Market.BlockIpAddress[a] == null)
                return 0;

            if (b >= Business.Market.BlockIpAddress[a].Count || Business.Market.BlockIpAddress[a][b] == null)
                return 0;

            if (c >= Business.Market.BlockIpAddress[a][b].Count || Business.Market.BlockIpAddress[a][b][c] == null)
                return 0;

            if (d >= Business.Market.BlockIpAddress[a][b][c].Count || Business.Market.BlockIpAddress[a][b][c][d] == null)
                return 0;

            if(e >= Business.Market.BlockIpAddress[a][b][c][d].Count || Business.Market.BlockIpAddress[a][b][c][d] == null)
                return 0;

            if (Business.Market.BlockIpAddress[a][b][c][d][e])
                return 1;

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal int RemoveIp(string ip,int investorID)
        {
            int a, b, c, d, e;
            string[] subValue = ip.Split('.');
            if (subValue.Length != 4)
                return -1;

            a = investorID;
                        
            if (!int.TryParse(subValue[0], out b) || b > 255)
                return -1;

            if (!int.TryParse(subValue[1], out c) || c > 255)
                return -1;

            if (!int.TryParse(subValue[2], out d) || c > 255)
                return -1;

            if (!int.TryParse(subValue[3], out e) || e > 255)
                return -1;
            
            if (a > Business.Market.BlockIpAddress.Count || Business.Market.BlockIpAddress[a] == null)
                return 0;

            if (b > Business.Market.BlockIpAddress[a].Count || Business.Market.BlockIpAddress[a][b] == null)
                return 0;

            if (c > Business.Market.BlockIpAddress[a][b].Count || Business.Market.BlockIpAddress[a][b][c] == null)
                return 0;

            if (d > Business.Market.BlockIpAddress[a][b][c].Count || Business.Market.BlockIpAddress[a][b][c] == null)
                return 0;

            if (e > Business.Market.BlockIpAddress[a][b][c][d].Count || Business.Market.BlockIpAddress[a][b][c][d] == null)
                return 0;

            if (Business.Market.BlockIpAddress[a][b][c][d][e])
            {
                Business.Market.BlockIpAddress[a][b][c][d].RemoveAt(e);
                Business.Market.BlockIpAddress[a][b][c].RemoveAt(d);
                Business.Market.BlockIpAddress[a][b].RemoveAt(c);
                Business.Market.BlockIpAddress[a].RemoveAt(b);
                Business.Market.BlockIpAddress.RemoveAt(a);
            }

            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool ValidIpAddress(int investorID, string ipAddress)
        {
            //if (Business.Market.InvestorOnline != null)
            //{
            //    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
            //    {
            //        if (Business.Market.InvestorOnline[i].InvestorID == investorID &&
            //            Business.Market.InvestorOnline[i].LoginKey == key)
            //        {
            //            if (string.IsNullOrEmpty(Business.Market.InvestorOnline[i].IpAddress))
            //                return false;

            //            if (Business.Market.InvestorOnline[i].IpAddress.Trim() != ipAddress.Trim())
            //                return false;
            //        }

            //        break;
            //    }
            //}
            return true;

            int result = Business.ValidIPAddress.Instance.CheckIp(ipAddress, investorID);

            if (result == -1 || result == 0)
                return false;

            return true;
        }
    }
}
