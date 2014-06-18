using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWInvestorProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        internal bool UpdateInvestorProfile(Business.Investor objInvestor)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int ResultUpdate = adap.UpdateInvestorProfile(objInvestor.InvestorID, objInvestor.Address, objInvestor.Phone, objInvestor.City, objInvestor.Country, objInvestor.Email,
                    objInvestor.ZipCode, objInvestor.InvestorComment, objInvestor.State, objInvestor.NickName, objInvestor.IDPassport, objInvestor.InvestorProfileID);

                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }
    }
}
