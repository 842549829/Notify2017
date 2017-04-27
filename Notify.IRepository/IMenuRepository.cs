using System;
using System.Collections.Generic;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Model.DB;

namespace Notify.IRepository
{
    /// <summary>
    /// 菜单仓储接口
    /// </summary>
    public interface IMenuRepository : IRepository<Guid, MMenu>
    {
        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns>菜单集合</returns>
        IEnumerable<MMenu> QueryMenus();

        /// <summary>
        /// 菜单查询(根据用户Id查询)
        /// </summary>
        /// <returns>结果</returns>
        IEnumerable<MMenu> QueryMenus(Guid accountId);

        /// <summary>
        /// 查询父级默认Id
        /// </summary>
        /// <returns>父级默认Id</returns>
        Guid QueryDefaultParentId();
    }
}