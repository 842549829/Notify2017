using System;
using Notify.Infrastructure.DomainBase;

namespace Notify.Model.DB
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MMenu : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public object Key => this.Id;
    }
}