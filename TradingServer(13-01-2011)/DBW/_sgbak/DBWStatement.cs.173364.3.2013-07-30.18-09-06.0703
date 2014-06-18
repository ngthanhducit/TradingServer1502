using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWStatement
    {
        #region CREATE INSTANCE DBWSTATEMENT
        private static DBW.DBWStatement _instance;
        internal static DBW.DBWStatement Instance
        {
            get
            {
                if (DBW.DBWStatement._instance == null)
                    DBW.DBWStatement._instance = new DBW.DBWStatement();

                return DBW.DBWStatement._instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Statement> GetAllStatement()
        {
            List<Business.Statement> result = new List<Business.Statement>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.StatementTableAdapter adap = new DSTableAdapters.StatementTableAdapter();
            DS.StatementDataTable tbStatement = new DS.StatementDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbStatement = adap.GetData();

                if (tbStatement != null)
                {
                    int count = tbStatement.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Statement newStatement = new Business.Statement();
                        newStatement.StatementID = tbStatement[i].ID;
                        newStatement.InvestorCode = tbStatement[i].InvestorCode;
                        newStatement.Content = tbStatement[i].Content;
                        newStatement.Email = tbStatement[i].Email;
                        newStatement.TimeStatement = tbStatement[i].SaveDate;
                        newStatement.StatementType = tbStatement[i].StatementType;

                        result.Add(newStatement);
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int AddStatement(Business.Statement value)
        {
            int result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.StatementTableAdapter adap = new DSTableAdapters.StatementTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = int.Parse(adap.AddNewStatement(value.InvestorCode, value.Content, value.TimeStatement, value.Email, value.StatementType).ToString());
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
        /// <param name="values"></param>
        /// <returns></returns>
        internal int AddStatement(List<Business.Statement> values)
        {
            int result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.StatementTableAdapter adap = new DSTableAdapters.StatementTableAdapter();

            try
            {
                if (values != null && values.Count > 0)
                {
                    conn.Open();
                    adap.Connection = conn;

                    int count = values.Count;
                    for (int i = 0; i < count; i++)
                    {
                        result = int.Parse(adap.AddNewStatement(values[i].InvestorCode, values[i].Content, values[i].TimeStatement,
                            values[i].Email, values[i].StatementType).ToString());
                    }
                }
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
    }
}
