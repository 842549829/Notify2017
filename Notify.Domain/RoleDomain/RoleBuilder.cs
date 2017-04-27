using System;
using System.Linq;
using System.Collections.Generic;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.Domain.RoleDomain
{
    /// <summary>
    /// 角色服务工厂
    /// </summary>
    public static class RoleBuilder
    {
        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mRole">MRole</param>
        /// <returns>Role</returns>
        public static Role ToRole(this MRole mRole)
        {
            var role = new Role
            {
                Id = mRole.Id,
                RoleName = mRole.RoleName,
                RoleDescription = mRole.RoleDescription
            };
            return role;
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="tRole">TRole</param>
        /// <returns>Role</returns>
        public static Role ToRole(this TRole tRole)
        {
            var role = new Role
            {
                Id = tRole.Id,
                RoleName = tRole.RoleName,
                RoleDescription = tRole.RoleDescription
            };
            return role;
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>MRole</returns>
        public static MRole ToMRole(this Role role)
        {
            return new MRole
            {
                Id = role.Id,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>MRole</returns>
        public static MRole ToMRole(this Guid roleId)
        {
            return new MRole
            {
                Id = roleId,
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="tRole">tRole</param>
        /// <returns>MRole</returns>
        public static MRole ToMRole(this TRole tRole)
        {
            return new MRole
            {
                Id = tRole.Id,
                RoleName = tRole.RoleName,
                RoleDescription = tRole.RoleDescription
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>TRole</returns>
        public static TRole ToTRole(this Role role)
        {
            return new TRole
            {
                Id = role.Id,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mRole">MRole</param>
        /// <returns>TRole</returns>
        public static IEnumerable<TRole> ToTRole(this IEnumerable<MRole> mRole)
        {
            return mRole.Select(ToTRole);
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mRole">MRole</param>
        /// <returns>TRole</returns>
        public static TRole ToTRole(this MRole mRole)
        {
            return new TRole
            {
                Id = mRole.Id,
                RoleName = mRole.RoleName,
                RoleDescription = mRole.RoleDescription
            };
        }
    }
}