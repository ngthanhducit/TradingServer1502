using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer
{
    public static partial class Facade
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.Role> FacadeGetAllRole()
        {
            return Facade.RoleInstance.GetAllRole();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public static Business.Role FacadeGetRoleByRoleID(int RoleID)
        {
            return Facade.RoleInstance.GetRoleByID(RoleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListRoleID"></param>
        /// <returns></returns>
        public static List<Business.Role> FacadeGetListRoleByListRoleID(List<int> ListRoleID)
        {
            return Facade.RoleInstance.GetListRoleByListID(ListRoleID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static Business.Role FacadeGetRoleByCode(string Code)
        {
            return Facade.RoleInstance.GetRoleByCode(Code);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public static int FacadeAddNewRole(string Code, string Name, string Comment)
        {
            return Facade.RoleInstance.AddNewRole(Code, Name, Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public static bool FacadeUpdateRole(int RoleID, string Code, string Name, string Comment)
        {
            return Facade.RoleInstance.UpdateRole(RoleID, Code, Name, Comment);
        }        
    }
}
