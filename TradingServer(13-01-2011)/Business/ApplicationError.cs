using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class ApplicationError
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }

        #region Create Instance Class DBW Application Error
        private static DBW.DBWApplicationError dbwApplicationError;
        private static DBW.DBWApplicationError DBWApplicationError
        {
            get
            {
                if (ApplicationError.dbwApplicationError == null)
                {
                    ApplicationError.dbwApplicationError = new DBW.DBWApplicationError();
                }
                return ApplicationError.dbwApplicationError;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ApplicationError> GetAllApplicationError()
        {
            return ApplicationError.DBWApplicationError.GetAllApplicationError();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        internal int AddNewApplicationError(string Name, string Description, DateTime Time)
        {
            return ApplicationError.DBWApplicationError.AddNewApplicationError(Name, Description, Time);
        }
    }
}
