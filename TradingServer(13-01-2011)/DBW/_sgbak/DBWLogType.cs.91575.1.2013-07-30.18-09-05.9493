using System.Collections.Generic;
using System.Data.SqlClient;
using System;
namespace TradingServer.DBW
{
    internal class DBWLogType
    {
        internal List<Business.LogType> GetAll()
        {
            DS.LogTypeDataTable tb = null;
            SqlConnection connection = new SqlConnection(DBConnection.DBConnection.Connection);
            try
            {
                DSTableAdapters.LogTypeTableAdapter adap = new DSTableAdapters.LogTypeTableAdapter();
                adap.Connection = connection;
                tb = adap.GetData();
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Dispose();
            }
            return this.MapLogType(tb);
        }

        List<Business.LogType> MapLogType(DS.LogTypeDataTable tb)
        {
            if (tb == null || tb.Rows.Count == 0)
            {
                return null;
            }
            List<Business.LogType> logTypeList = new List<Business.LogType>();
            int count = tb.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                Business.LogType logType = new Business.LogType();
                logType.ID = tb[i].LogTypeID;
                logType.Name = tb[i].Name;
                logTypeList.Add(logType);
            }
            return logTypeList;
        }
    }
}
