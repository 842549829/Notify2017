using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Notify.Code.Code;
using Notify.Controller.Base;
using Notify.Service;

namespace Notify.Controller.Permission
{
    public class PermissionRoleController : BaseController
    {
        /// <summary>
        /// 权限角色
        /// </summary>
        /// <param name="accountId">用户Id</param>
        /// <param name="accountName">用户名</param>
        /// <returns>视图</returns>
        public ActionResult PermissionRoleList(Guid accountId, string accountName)
        {
            ViewBag.AccountId = accountId;
            ViewBag.AccountName = accountName + "(角色设置)";
            return this.View();
        }

        /// <summary>
        /// 查询可用角色Id
        /// </summary>
        /// <returns>用户Id</returns>
        public ActionResult QueryRoleIds(Guid accountId)
        {
            var data = PermissionService.QueryMenuIds(accountId);
            return new MyJsonResult { Data = data };
        }

        /// <summary>
        /// 保存权限角色
        /// </summary>
        /// <param name="accountId">用户Id</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>结果</returns>
        public ActionResult SavePermissionRole(Guid accountId, List<Guid> roleIds)
        {
            var operational = GetOperational();
            operational.OperationContent = "保存权限角色";
            var result = PermissionService.SavePermissionMenu(accountId, roleIds, operational);
            return new MyJsonResult { Data = result };
        }
    }
}