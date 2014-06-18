using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWIVirtualDealer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbIVirtualDealer"></param>
        /// <returns></returns>
        private List<Business.IVirtualDealer> MapIVirtualDealer(DS.IVirtualDealerDataTable tbIVirtualDealer)
        {
            List<Business.IVirtualDealer> result = new List<Business.IVirtualDealer>();
            if (tbIVirtualDealer != null)
            {
                int count = tbIVirtualDealer.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.IVirtualDealer newIVirtualDealer = new Business.IVirtualDealer();
                    newIVirtualDealer.InvestorGroupID = tbIVirtualDealer[i].InvestorGroupID;
                    newIVirtualDealer.IVirtualDealerID = tbIVirtualDealer[i].IVirtualDealerID;                    
                    newIVirtualDealer.SymbolID = tbIVirtualDealer[i].SymbolID;
                    newIVirtualDealer.VirtualDealerID = tbIVirtualDealer[i].VirtualDealerID;

                    result.Add(newIVirtualDealer);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.IVirtualDealer> GetAllVirtualDealer()
        {
            List<Business.IVirtualDealer> result = new List<Business.IVirtualDealer>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IVirtualDealerTableAdapter adap = new DSTableAdapters.IVirtualDealerTableAdapter();
            DS.IVirtualDealerDataTable tbIVirtualDealer = new DS.IVirtualDealerDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapIVirtualDealer(adap.GetData());
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
        /// <param name="objIVirtualDealer"></param>
        /// <returns></returns>
        internal int AddNewIVirtualDealer(Business.IVirtualDealer objIVirtualDealer)
        {
            int result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IVirtualDealerTableAdapter adap = new DSTableAdapters.IVirtualDealerTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = int.Parse(adap.AddNewIVirtualDealer(objIVirtualDealer.InvestorGroupID, objIVirtualDealer.SymbolID, objIVirtualDealer.VirtualDealerID).ToString());
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
        /// <param name="objIVirtualDealer"></param>
        /// <returns></returns>
        internal bool UpdateIVirtualDealer(Business.IVirtualDealer objIVirtualDealer)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IVirtualDealerTableAdapter adap = new DSTableAdapters.IVirtualDealerTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateIVirtualDealer(objIVirtualDealer.InvestorGroupID, objIVirtualDealer.SymbolID, objIVirtualDealer.VirtualDealerID, objIVirtualDealer.IVirtualDealerID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iVirtualDealerID"></param>
        /// <returns></returns>
        internal bool DeleteIVirtualDealer(int iVirtualDealerID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IVirtualDealerTableAdapter adap = new DSTableAdapters.IVirtualDealerTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteIVirtualDealer(iVirtualDealerID);
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
