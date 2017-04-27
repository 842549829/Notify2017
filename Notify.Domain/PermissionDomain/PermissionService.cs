using System;
using Notify.DbCommon.Repositroies;
using Notify.Domain.AccountDomain;
using Notify.Domain.MenuDomain;

namespace Notify.Domain.PermissionDomain
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class PermissionService
    {
        /// <summary>
        /// 当前上下文
        /// </summary>
        public static IDbFactory.IDbFactory DbContext { get; } = Factory.GetFactory<IDbFactory.IDbFactory>();

        /// <summary>
        /// 查询用户的权限
        /// </summary>
        /// <param name="account">用户</param>
        /// <returns>权限</returns>
        public static PermissionCollection QueryPermissionOfUser(Account account)
        {
            if (account.IsAdmin)
            {
                return QueryPermissionOfCommonUser();
            }
            else
            {
                return QueryPermissionOfCommonUser(account.Key);
            }
        }

        /// <summary>
        /// 查询权限(非管理员)
        /// </summary>
        /// <param name="accountId">用户Id</param>
        /// <returns>权限</returns>
        private static PermissionCollection QueryPermissionOfCommonUser(Guid accountId)
        {
            var userPermissionRoles = MenuService.QueryMenus(accountId);
            var userPermissions = PermissionCollection.Union(userPermissionRoles);
            return new PermissionCollection(userPermissions);
        }

        /// <summary>
        ///  查询权限(管理员) 
        /// </summary>
        private static PermissionCollection QueryPermissionOfCommonUser()
        {
            var userPermissionRoles = MenuService.QueryMenus();
            var userPermissions = PermissionCollection.Union(userPermissionRoles);
            return new PermissionCollection(userPermissions);
        }
    }
}