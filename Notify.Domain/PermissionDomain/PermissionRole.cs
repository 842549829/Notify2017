using System;
using System.Collections.Generic;
using Notify.Domain.MenuDomain;

namespace Notify.Domain.PermissionDomain
{
    /// <summary>
    /// 权限角色
    /// </summary>
    public class PermissionRole
    {
        /// <summary>
        /// 菜单
        /// </summary>
        private readonly List<Menu> m_menus;

        /// <summary>
        /// 构造函数
        /// </summary>
        internal PermissionRole()
        {
            m_menus = new List<Menu>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            internal set;
        }

        /// <summary>
        /// 可访问菜单集合
        /// </summary>
        public IEnumerable<Menu> Menus
        {
            get
            {
                return m_menus.AsReadOnly();
            }
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="item">菜单</param>
        internal void AppendMenu(Menu item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (this.m_menus.Exists(menu => menu.Id == item.Id))
            {
                throw new Notify.Code.Exception.CustomException("不能重复添加的同一菜单");
            }
            this.m_menus.Add(item);
        }
    }
}