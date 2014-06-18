using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWVirtualDealerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbVirtualDealerConfig"></param>
        /// <returns></returns>
        private List<Business.ParameterItem> MapVirtualDealerConfig(DS.VirtualDealerConfigDataTable tbVirtualDealerConfig)
        {
            List<Business.ParameterItem> result = new List<Business.ParameterItem>();
            if (tbVirtualDealerConfig != null)
            {
                int count = tbVirtualDealerConfig.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.ParameterItem newParamerter = new Business.ParameterItem();
                    newParamerter.BoolValue = tbVirtualDealerConfig[i].BoolValue;
                    newParamerter.Code = tbVirtualDealerConfig[i].Code;
                    newParamerter.DateValue = tbVirtualDealerConfig[i].DateValue;
                    newParamerter.Name = tbVirtualDealerConfig[i].Name;
                    newParamerter.NumValue = tbVirtualDealerConfig[i].NumValue;
                    newParamerter.ParameterItemID = tbVirtualDealerConfig[i].VirtualDealerConfigID;
                    newParamerter.SecondParameterID = tbVirtualDealerConfig[i].VirtualDealID;
                    newParamerter.StringValue = tbVirtualDealerConfig[i].StringValue;

                    result.Add(newParamerter);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAll()
        {
            List<Business.ParameterItem> result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerConfigTableAdapter adap = new DSTableAdapters.VirtualDealerConfigTableAdapter();

            try
            {
                result = this.MapVirtualDealerConfig(adap.GetData());
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
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetVirtualDealerConfigByVirtualDealerID(int virtualDealerID)
        {
            List<Business.ParameterItem> result = new List<Business.ParameterItem>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerConfigTableAdapter adap = new DSTableAdapters.VirtualDealerConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapVirtualDealerConfig(adap.GetVirtualDealerConfigByVirtualDealerID(virtualDealerID));
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
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal int AddNewVirtualDealerConfig(Business.ParameterItem objParameterItem)
        {
            int result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerConfigTableAdapter adap = new DSTableAdapters.VirtualDealerConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = int.Parse(adap.AddNewVirtualDealerConfig(-1, objParameterItem.Name, objParameterItem.SecondParameterID, objParameterItem.Code, objParameterItem.BoolValue,
                    objParameterItem.StringValue, objParameterItem.NumValue, objParameterItem.DateValue).ToString());
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
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal bool UpdateVirtualDealerConfig(Business.ParameterItem objParameterItem)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerConfigTableAdapter adap = new DSTableAdapters.VirtualDealerConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateVirtualDealerConfig(-1, objParameterItem.Name, objParameterItem.SecondParameterID, objParameterItem.Code, objParameterItem.BoolValue,
                    objParameterItem.StringValue, objParameterItem.NumValue, objParameterItem.DateValue, objParameterItem.ParameterItemID);

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
        /// <param name="virtualDealerConfigID"></param>
        /// <returns></returns>
        internal bool DeleteVirtualDealerConfig(int virtualDealerConfigID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerConfigTableAdapter adap = new DSTableAdapters.VirtualDealerConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteVirtualDealerConfig(virtualDealerConfigID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        internal bool DeleteVirtualConfigByVirtualDealearID(int virtualDealerID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerConfigTableAdapter adap = new DSTableAdapters.VirtualDealerConfigTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultDelete = adap.DeleteVirtualConfigByVirtualDealerID(virtualDealerID);
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
