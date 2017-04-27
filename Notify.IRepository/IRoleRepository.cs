using System;
using System.Collections.Generic;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.IRepository
{
    /// <summary>
    /// 角色仓储接口
    /// </summary>
    public interface IRoleRepository : IRepository<Guid, MRole>
    {
        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        IEnumerable<MRole> QueryRolesByPaging(TRoleCondition condition);
    }
}