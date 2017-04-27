using System;
using System.Collections.Generic;
using Notify.Code.Code;
using Notify.Code.Constant;
using Notify.Code.Exception;
using Notify.Code.Write;
using Notify.Domain.PermissionDomain;
using Notify.Infrastructure.UnitOfWork;
using Notify.IRepository;

namespace Notify.Service
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class PermissionService
    {
        /// <summary>
        /// 数据工厂(当前上下文)
        /// </summary>
        public static IDbFactory.IDbFactory DbContext = Factory.DbContext;

        /// <summary>
        /// 查询可用菜单Id
        /// </summary>
        /// <returns>角色Id</returns>
        public static IEnumerable<Guid> QueryMenuIds(Guid roleId)
        {
            using (IRolePermissionsRepository rolePermissionsRepository = DbContext.CreateIRolePermissionsRepository())
            {
                return rolePermissionsRepository.QueryMenuIds(roleId);
            }
        }

        /// <summary>
        /// 保存权限菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuIds">菜单Id集合</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result SavePermissionMenu(Guid roleId, List<Guid> menuIds, Operational operational)
        {
            Result result = new Result();
            try
            {
                PermissionValidate.ValidateMenuIds(menuIds);
                using (IPowerUnitOfWork unit = DbContext.CreateIPowerUnitOfWork())
                {
                    IRolePermissionsRepository rolePermissionsRepository = DbContext.CreateIRolePermissionsRepository(unit);
                    rolePermissionsRepository.RemoveByRoleId(roleId);
                    var content = PermissionBuilder.ToMRolePermissions(roleId, menuIds);
                    rolePermissionsRepository.Add(content);
                    unit.Complete();
                }

                result.IsSucceed = true;
                result.Message = "保存成功";
            }
            catch (CustomException ex)
            {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.ErrorMessage;
                LogService.WriteLog(ex, "保存权限菜单");
            }
            return result;
        }

        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="accountId">用户Id</param>
        /// <param name="roleIds">角色Id集合</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result SavePermissionRole(Guid accountId, List<Guid> roleIds, Operational operational)
        {
            Result result = new Result();
            try
            {
                using (IPowerUnitOfWork unit = DbContext.CreateIPowerUnitOfWork())
                {
                    IRoleUserRelationshipRepository roleUserRelationshipRepository = DbContext.CreateIRoleUserRelationshipRepository(unit);
                    roleUserRelationshipRepository.RemoveByAccountId(accountId);
                    var content = PermissionBuilder.ToMRoleUserRelationship(accountId, roleIds);
                    roleUserRelationshipRepository.Add(content);
                    unit.Complete();
                }

                result.IsSucceed = true;
                result.Message = "保存成功";

            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.ErrorMessage;
                LogService.WriteLog(ex, "保存用户角色关系");
            }
            return result;
        }
    }
}