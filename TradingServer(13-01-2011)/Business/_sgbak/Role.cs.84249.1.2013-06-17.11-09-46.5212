using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        #region Create Instance Class DBWRole
        private static DBW.DBWRole dbwRole;
        private static DBW.DBWRole DBWRoleInstance
        {
            get
            {
                if (Role.dbwRole == null)
                {
                    Role.dbwRole = new DBW.DBWRole();
                }

                return Role.dbwRole;
            }
        }
        #endregion

        /// <summary>
        /// Constructure
        /// </summary>
        public Role()
        { 
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Role> GetAllRole()
        {
            return Role.DBWRoleInstance.GetAllRole();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        internal Business.Role GetRoleByID(int RoleID)
        {
            return Role.DBWRoleInstance.GetRoleByRoleID(RoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListRoleID"></param>
        /// <returns></returns>
        internal List<Business.Role> GetListRoleByListID(List<int> ListRoleID)
        {
            return Role.DBWRoleInstance.GetListRoleByListRoleID(ListRoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.Role GetRoleByCode(string Code)
        {
            return Role.DBWRoleInstance.GetRoleByCode(Code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        internal int AddNewRole(string Code, string Name, string Comment)
        {
            return Role.DBWRoleInstance.AddNewRole(Code, Name, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        internal bool UpdateRole(int RoleID, string Code, string Name, string Comment)
        {
            return Role.DBWRoleInstance.UpdateRole(RoleID, Code, Name, Comment);
        }
    }
}
