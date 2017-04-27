using System;
using System.Linq;
using System.Collections.Generic;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.Domain.MenuDomain
{
    /// <summary>
    /// 菜单服务工厂
    /// </summary>
    public static class MenuBuilder
    {
        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mMenu">MMenu</param>
        /// <returns>ZtreeMenu</returns>
        public static ZtreeMenu ToZtreeMenu(this MMenu mMenu)
        {
            return new ZtreeMenu
            {
                id = mMenu.Id.ToString(),
                pId = mMenu.ParentId.ToString(),
                name = mMenu.Title
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mMenu">MMenu</param>
        /// <returns>ZtreeMenus</returns>
        public static IEnumerable<ZtreeMenu> ToZtreeMenu(this IEnumerable<MMenu> mMenu)
        {
            return mMenu.Select(ToZtreeMenu);
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mMenu">MMenu</param>
        /// <returns>TMenu</returns>
        public static TMenu ToTMenu(this MMenu mMenu)
        {
            return new TMenu
            {
                Id = mMenu.Id,
                MenuDescription = mMenu.Description,
                MenuSort = mMenu.Sort,
                ParentId = mMenu.ParentId,
                MenuName = mMenu.Title,
                MenuUrl = mMenu.Url,
                MenuIcon = mMenu.Icon
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="tMenu">TMenu</param>
        /// <returns>MMenu</returns>
        public static MMenu ToMMenu(this TMenu tMenu)
        {
            return new MMenu
            {
                Id = tMenu.Id,
                Description = tMenu.MenuDescription,
                Sort = tMenu.MenuSort,
                ParentId = tMenu.ParentId,
                Title = tMenu.MenuName,
                Url = tMenu.MenuUrl,
                Icon = tMenu.MenuIcon
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="tMenu">TMenu</param>
        /// <returns>Menu</returns>
        public static Menu ToMenu(this TMenu tMenu)
        {
            return new Menu
            {
                Id = tMenu.Id,
                Description = tMenu.MenuDescription,
                Sort = tMenu.MenuSort,
                ParentId = tMenu.ParentId,
                Title = tMenu.MenuName,
                Url = tMenu.MenuUrl,
                Icon = tMenu.MenuIcon
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mMenu">TMenu</param>
        /// <returns>MMenu</returns>
        public static Menu ToMenu(this MMenu mMenu)
        {
            return new Menu
            {
                Id = mMenu.Id,
                Description = mMenu.Description,
                Sort = mMenu.Sort,
                ParentId = mMenu.ParentId,
                Title = mMenu.Title,
                Url = mMenu.Url,
                Icon = mMenu.Icon
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="menu">tMenu</param>
        /// <returns>MMenu</returns>
        public static TMenu ToTMenu(this Menu menu)
        {
            return new TMenu
            {
                Id = menu.Id,
                MenuDescription = menu.Description,
                MenuSort = menu.Sort,
                ParentId = menu.ParentId,
                MenuName = menu.Title,
                MenuUrl = menu.Url,
                MenuIcon = menu.Icon
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="menu">Menu</param>
        /// <returns>MMenu</returns>
        public static MMenu ToMMenu(this Menu menu)
        {
            return new MMenu
            {
                Id = menu.Id,
                Description = menu.Description,
                Sort = menu.Sort,
                ParentId = menu.ParentId,
                Title = menu.Title,
                Url = menu.Url,
                Icon = menu.Icon
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="menuId">menuId</param>
        /// <returns>MMenu</returns>
        public static MMenu ToMMenu(this Guid menuId)
        {
            return new MMenu
            {
                Id = menuId
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mMenu">MMenu</param>
        /// <returns>EsayUIMenu</returns>
        public static EsayUIMenu ToEsayUIMenu(this MMenu mMenu)
        {
            return new EsayUIMenu
            {
                menuid = mMenu.Id,
                icon = mMenu.Icon,
                menuname = mMenu.Title,
                url = mMenu.Url
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="menu">tMenu</param>
        /// <returns>MMenu</returns>
        public static IEnumerable<TMenu> ToTMenus(this IEnumerable<Menu> menu)
        {
            return menu.Select(ToTMenu);
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mMenu">MMenu</param>
        /// <returns>Menu</returns>
        public static IEnumerable<Menu> ToMenus(this IEnumerable<MMenu> mMenu)
        {
            return mMenu.Select(ToMenu);
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="tMenu">TMenu</param>
        /// <returns>Menu</returns>
        public static IEnumerable<Menu> ToMenus(this IEnumerable<TMenu> tMenu)
        {
            return tMenu.Select(ToMenu);
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="menu">Menu</param>
        /// <returns>MMenu</returns>
        public static IEnumerable<MMenu> ToMMenus(this IEnumerable<Menu> menu)
        {
            return menu.Select(ToMMenu);
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="menus">MMenu</param>
        /// <returns>EsayUIMenu</returns>
        public static IEnumerable<EsayUIMenu> ToEsayUIMenus(this IEnumerable<MMenu> menus)
        {
            var mMenus = menus as MMenu[] ?? menus.ToArray();
            var drList = mMenus.Where(item => item.ParentId == Guid.Empty).Select(item => item.Id);
            var enumerable = drList as Guid[] ?? drList.ToArray();
            var drData = enumerable.Any() ? mMenus.Where(item => enumerable.Contains(item.ParentId)) : mMenus.Where(item => item.ParentId == MenuService.QueryDefaultParentId());
            List<EsayUIMenu> rootNode = new List<EsayUIMenu>();
            foreach (var item in drData)
            {
                EsayUIMenu esayUIMenu = new EsayUIMenu
                {
                    menuid = item.Id,
                    menuname = item.Title,
                    icon = item.Icon,
                    url = item.Url
                };
                esayUIMenu.menus = CreateChildTree(mMenus, esayUIMenu);
                rootNode.Add(esayUIMenu);
            }
            return rootNode;
        }

        /// <summary>
        /// 递归菜单
        /// </summary>
        /// <param name="menus">菜单集合</param>
        /// <param name="menu">父级菜单</param>
        /// <returns>结果</returns>
        private static IEnumerable<EsayUIMenu> CreateChildTree(IEnumerable<MMenu> menus, EsayUIMenu menu)
        {
            List<EsayUIMenu> nodeList = new List<EsayUIMenu>();
            var mMenus = menus as MMenu[] ?? menus.ToArray();
            var children = mMenus.Where(item => item.ParentId == menu.menuid);
            foreach (var item in children)
            {
                EsayUIMenu node = new EsayUIMenu
                {
                    menuid = item.Id,
                    menuname = item.Title,
                    url = item.Url,
                    icon = item.Icon
                };
                node.menus = CreateChildTree(mMenus, node);
                nodeList.Add(node);
            }
            return nodeList;
        }
    }
}