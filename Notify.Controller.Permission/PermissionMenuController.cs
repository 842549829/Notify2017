using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Notify.Code.Code;
using Notify.Controller.Base;
using Notify.Service;

namespace Notify.Controller.Permission
{
    /// <summary>
    /// 权限菜单
    /// </summary>
    public class PermissionMenuController : BaseController
    {
        /// <summary>
        /// 权限菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>视图</returns>
        public ActionResult PermissionMenuList(Guid roleId, string roleName)
        {
            ViewBag.RoleName = roleName + "(权限设置)";
            return this.View(roleId);
        }

        /// <summary>
        /// 查询可用菜单Id
        /// </summary>
        /// <returns>角色Id</returns>
        public ActionResult QueryMenuIds(Guid roleId)
        {
            var data = PermissionService.QueryMenuIds(roleId);
            return new MyJsonResult { Data = data };
        }

        /// <summary>
        /// 保存权限菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuIds">菜单Id</param>
        /// <returns>结果</returns>
        public ActionResult SavePermissionMenu(Guid roleId, List<Guid> menuIds)
        {
            var operational = GetOperational();
            operational.OperationContent = "保存权限菜单";
            var result = PermissionService.SavePermissionMenu(roleId, menuIds, operational);
            return new MyJsonResult { Data = result };
        }
    }
}