using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Notify.Domain.MenuDomain;
using Notify.Model.Transfer;

namespace Notify.Domain.PermissionDomain
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionCollection
    {
        /// <summary>
        /// 菜单
        /// </summary>
        internal List<Menu> m_menus;

        /// <summary>
        /// 初始化权限
        /// </summary>
        /// <param name="menus">菜单</param>
        internal PermissionCollection(IEnumerable<Menu> menus)
        {
            this.m_menus = new List<Menu>();
            var unitedMenus = Union(menus);
            var sortedMenus = unitedMenus.OrderBy(menu => menu.Sort);
            this.m_menus.AddRange(sortedMenus);
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="address">菜单地址</param>
        /// <returns>结果</returns>
        public bool HasPermission(string address)
        {
            return this.m_menus.Any(item => item.ContainsResource(address));
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="menus">菜单</param>
        /// <param name="address">菜单地址</param>
        /// <returns>结果</returns>
        public static bool HasPermission(IEnumerable<Menu> menus, string address)
        {
            address = Regex.Replace(address, "/[%28|\\(].*?[%29|\\)]/", "/").ToLower();
            if (address == "/home/index")
            {
                return true;
            }
            return menus.Any(item => string.Compare(item.Url, address, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// 菜单组合
        /// </summary>
        /// <param name="menus">菜单</param>
        /// <returns>菜单集</returns>
        internal static IEnumerable<Menu> Union(IEnumerable<Menu> menus)
        {
            var dicMenus = new Dictionary<Guid, Menu>();
            foreach (var item in menus.Where(item => item != null))
            {
                if (dicMenus.ContainsKey(item.Id))
                {
                    dicMenus[item.Id] = Menu.Union(dicMenus[item.Id], item);
                }
                else
                {
                    dicMenus.Add(item.Id, item.Clone());
                }
            }
            foreach (var item in dicMenus)
            {
                item.Value.Sorting();
            }
            return dicMenus.Values;
        }

        /// <summary>
        /// 菜单组合
        /// </summary>
        /// <param name="menus">菜单</param>
        /// <returns>菜单集</returns>
        internal static IEnumerable<Menu> Union(params IEnumerable<Menu>[] menus)
        {
            return Union(from items in menus
                         from item in items
                         where item != null
                         select item);
        }

        /// <summary>
        /// 菜单组合
        /// </summary>
        /// <param name="roles">角色权限</param>
        /// <returns>菜单集</returns>
        internal static IEnumerable<Menu> Union(IEnumerable<PermissionRole> roles)
        {
            IEnumerable<Menu> menus = new List<Menu>();
            if (roles != null)
            {
                menus = from role in roles
                        where role != null
                        from menu in role.Menus
                        where menu != null
                        select menu;
            }
            return Union(menus);
        }

        /// <summary>
        /// 菜单减去
        /// </summary>
        /// <param name="first">菜单1</param>
        /// <param name="second">菜单2</param>
        /// <returns>菜单</returns>
        internal static IEnumerable<Menu> Subtract(IEnumerable<Menu> first, IEnumerable<Menu> second)
        {
            var result = new List<Menu>();
            if (second == null)
            {
                if (first != null)
                {
                    result.AddRange(first.Select(item => item.Clone()));
                }
            }
            else
            {
                if (first == null)
                {
                    return result;
                }
                var dicForbidenMenus = second.ToDictionary(item => item.Id);
                foreach (var item in first)
                {
                    if (dicForbidenMenus.ContainsKey(item.Id))
                    {
                        var menu = Menu.Subtract(item, dicForbidenMenus[item.Id]);
                        if (menu != null && !menu.IsEmpty)
                        {
                            result.Add(menu);
                        }
                    }
                    else
                    {
                        result.Add(item.Clone());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 菜单交叉
        /// </summary>
        /// <param name="first">菜单1</param>
        /// <param name="second">菜单2</param>
        /// <returns>菜单</returns>
        internal static IEnumerable<Menu> Intersact(IEnumerable<Menu> first, IEnumerable<Menu> second)
        {
            var firstDic = Union(first).ToDictionary(item => item.Id);
            var secondDic = Union(second).ToDictionary(item => item.Id);
            return firstDic.Where(item => secondDic.ContainsKey(item.Key)).Select(item => Menu.Intersact(item.Value, secondDic[item.Key])).Where(menu => menu != null && !menu.IsEmpty);
        }

        /// <summary>
        /// 菜单
        /// </summary>
        internal IEnumerable<TMenu> Menus => this.m_menus.ToTMenus();
    }
}