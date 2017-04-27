using System;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class TMenu
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string MenuDescription { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public int MenuSort { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }
    }
}
