using System;
using System.Collections.Generic;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// EsayUI框架菜单
    /// </summary>
    public class EsayUIMenu
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid menuid { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string menuname { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public IEnumerable<EsayUIMenu> menus { get; set; }
    }
}