﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class Statement
    {
        public int StatementID { get; set; }
        public string InvestorCode { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public DateTime TimeStatement { get; set; }
        public int StatementType { get; set; }

        #region CREATE INSTANCE STATEMENT
        private static Business.Statement _instance;
        public static Business.Statement Instance
        {
            get
            {
                if (Business.Statement._instance == null)
                    Business.Statement._instance = new Statement();

                return Business.Statement._instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Statement> GetStatement()
        {
            return DBW.DBWStatement.Instance.GetAllStatement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int AddNewStatement(Business.Statement value)
        {
            return DBW.DBWStatement.Instance.AddStatement(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        internal int AddNewStatement(List<Business.Statement> values)
        {
            return DBW.DBWStatement.Instance.AddStatement(values);
        }
    }
}
