using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    internal class LogType
    {
        internal string Name { get; set; }
        internal int ID { get; set; }

        internal List<LogType> GetAll()
        {
            DBW.DBWLogType dbwLogType = new DBW.DBWLogType();
            return dbwLogType.GetAll();
        }
    }
}
