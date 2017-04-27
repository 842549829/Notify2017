using System;
using System.Collections.Generic;
using System.Linq;
using Notify.Model.DB;

namespace Notify.Domain.PermissionDomain
{
    /// <summary>
    /// 权限集合创建
    /// </summary>
    public class PermissionBuilder
    {
        /// <summary>
        ///  对象转化
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuIds">菜单Id集合</param>
        /// <returns>MRolePermissions</returns>
        public static IEnumerable<MRolePermissions> ToMRolePermissions(Guid roleId, List<Guid> menuIds)
        {
            return menuIds.Select(item => new MRolePermissions
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                MenuId = item
            });
        }

        /// <summary>
        ///  对象转化
        /// </summary>
        /// <param name="accountId">用户Id</param>
        /// <param name="roleIds">角色Id集合</param>
        /// <returns>MRoleUserRelationship</returns>
        public static IEnumerable<MRoleUserRelationship> ToMRoleUserRelationship(Guid accountId, List<Guid> roleIds)
        {
            return roleIds.Select(item => new MRoleUserRelationship
            {
                Id = Guid.NewGuid(),
                RoleId = accountId,
                AccountId = item
            });
        }
    }
}