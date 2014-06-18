using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWApplicationError
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ApplicationError> GetAllApplicationError()
        {
            List<Business.ApplicationError> Result = new List<Business.ApplicationError>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.ApplicationErrorTableAdapter adap = new DSTableAdapters.ApplicationErrorTableAdapter();
            DS.ApplicationErrorDataTable tbApplicationError = new DS.ApplicationErrorDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbApplicationError = adap.GetData();
                if (tbApplicationError != null)
                {
                    int count = tbApplicationError.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.ApplicationError newApplicationError = new Business.ApplicationError();
                        newApplicationError.ID = tbApplicationError[i].ID;
                        newApplicationError.Name = tbApplicationError[i].Name;
                        newApplicationError.Description = tbApplicationError[i].Description;
                        newApplicationError.Time = tbApplicationError[i].DateTime;

                        Result.Add(newApplicationError);
                    }
                }
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

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="TimeError"></param>
        /// <returns></returns>
        internal int AddNewApplicationError(string Name, string Description, DateTime TimeError)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.ApplicationErrorTableAdapter adap = new DSTableAdapters.ApplicationErrorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewApplicationError(Name, Description, TimeError).ToString());
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

            return Result;
        }
    }
}
