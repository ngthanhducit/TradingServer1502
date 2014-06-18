using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWInternalMail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbInternalMail"></param>
        /// <returns></returns>
        private List<Business.InternalMail> MapInternalMail(DS.InternalMailDataTable tbInternalMail)
        {
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            if (tbInternalMail != null)
            {
                int count = tbInternalMail.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.InternalMail newInternailMail = new Business.InternalMail();
                    newInternailMail.Content = tbInternalMail[i].Content;
                    newInternailMail.From = tbInternalMail[i].From;
                    newInternailMail.FromName = tbInternalMail[i].FromName;
                    newInternailMail.InternalMailID = tbInternalMail[i].InternalMailID;
                    newInternailMail.Subject = tbInternalMail[i].Subject;
                    newInternailMail.Time = tbInternalMail[i].Time;
                    newInternailMail.To = tbInternalMail[i].To;
                    newInternailMail.ToName = tbInternalMail[i].ToName;
                    newInternailMail.IsNew = tbInternalMail[i].IsNew;
                    result.Add(newInternailMail);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.InternalMail> GetAllInternalMail()
        {
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapInternalMail(adap.GetData());
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        internal List<Business.InternalMail> GetInternalMailToInvestor(string investorCode)
        {
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapInternalMail(adap.GetInternalMailToInvestor(investorCode));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        internal List<Business.InternalMail> GetInternalMailFromInvestor(string investorCode)
        {
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapInternalMail(adap.GetInternalMailFromInvestor(investorCode));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        internal List<Business.InternalMail> GetTopInternalMailToInvestor(string investorCode)
        {
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();            

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapInternalMail(adap.GetTopInternalMailToInvestor(investorCode));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        internal Business.InternalMail GetInternalMailToInvestorByID(int mailID)
        {
            List<Business.InternalMail> result = new List<Business.InternalMail>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();
            DS.InternalMailDataTable tab = new DS.InternalMailDataTable();
            Business.InternalMail newInternailMail = new Business.InternalMail();
            try
            {
                conn.Open();
                adap.Connection = conn;
                tab = adap.GetInternalMailByID(mailID);
                newInternailMail.Content = tab[0].Content;
                newInternailMail.From = tab[0].From;
                newInternailMail.FromName = tab[0].FromName;
                newInternailMail.InternalMailID = tab[0].InternalMailID;
                newInternailMail.Subject = tab[0].Subject;
                newInternailMail.Time = tab[0].Time;
                newInternailMail.To = tab[0].To;
                newInternailMail.IsNew = tab[0].IsNew;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return newInternailMail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailIns"></param>
        /// <returns></returns>
        internal int AddNewInternalMail(Business.InternalMail internalMailIns)
        {
            int result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int.TryParse(adap.AddNewInternalMail(internalMailIns.Subject, internalMailIns.From, internalMailIns.To, internalMailIns.Content, internalMailIns.Time, internalMailIns.IsNew, DateTime.Now, internalMailIns.FromName, internalMailIns.ToName).ToString(), out result);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailID"></param>
        /// <returns></returns>
        internal bool UpdateInternalMail(Business.InternalMail internailMailIns)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateInternalMail(internailMailIns.Subject, internailMailIns.From, internailMailIns.To,
                    internailMailIns.Content, internailMailIns.Time, internailMailIns.IsNew, DateTime.Now, internailMailIns.FromName, internailMailIns.ToName,internailMailIns.InternalMailID);

                if (resultUpdate > 0)
                    result = true;
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

            return result;
        }

        internal void UpdateInternalMailStatus(bool isNew,int mailID)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateStatusMail(isNew,DateTime.Now,mailID);
               
            }
            catch (Exception ex)
            {
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalMailID"></param>
        /// <returns></returns>
        internal bool DeleteInternalMail(int internalMailID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InternalMailTableAdapter adap = new DSTableAdapters.InternalMailTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteInternalMail(internalMailID);
                if (resultDelete > 0)
                    result = true;
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

            return result;
        }
    }
}
