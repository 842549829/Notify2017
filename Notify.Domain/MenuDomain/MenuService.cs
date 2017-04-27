using System;
using System.Collections.Generic;
using Notify.DbCommon.Repositroies;
using Notify.Model.DB;

namespace Notify.Domain.MenuDomain
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public class MenuService
    {
        /// <summary>
        /// 当前上下文
        /// </summary>
        public static IDbFactory.IDbFactory DbContext { get; } = Factory.GetFactory<IDbFactory.IDbFactory>();

        /// <summary>
        /// 菜单查询(所有)
        /// </summary>
        /// <returns>结果</returns>
        public static IEnumerable<Menu> QueryMenus()
        {
            using (var menuRepository = DbContext.CreateIMenuRepository())
            {
                var data = menuRepository.QueryMenus().ToMenus();
                return data;
            }
        }

        /// <summary>
        /// 菜单查询(根据用户Id查询)
        /// </summary>
        /// <returns>结果</returns>
        public static IEnumerable<Menu> QueryMenus(Guid accountId)
        {
            using (var menuRepository = DbContext.CreateIMenuRepository())
            {
                var data = menuRepository.QueryMenus(accountId).ToMenus();
                return data;
            }
        }

        /// <summary>
        /// 查询父级默认Id
        /// </summary>
        /// <returns>父级默认Id</returns>
        public static Guid QueryDefaultParentId()
        {
            using (var menuRepository = DbContext.CreateIMenuRepository())
            {
                var data = menuRepository.QueryDefaultParentId();
                return data;
            }
        }
    }
}