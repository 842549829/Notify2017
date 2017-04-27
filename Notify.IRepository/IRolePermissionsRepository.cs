using System;
using System.Collections.Generic;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Model.DB;

namespace Notify.IRepository
{
    /// <summary>
    /// 角色权限仓储接口
    /// </summary>
    public interface IRolePermissionsRepository : IAddsRepository<MRolePermissions>, IRepository<Guid, MRolePermissions>
    {
        /// <summary>
        /// 根据角色Id删除
        /// </summary>
        /// <param name="roleId">用户Id</param>
        void RemoveByRoleId(Guid roleId);

        /// <summary>
        /// 查询可用菜单Id
        /// </summary>
        /// <returns>角色Id</returns>
        IEnumerable<Guid> QueryMenuIds(Guid roleId);
    }
}