using System;
using System.Linq;
using System.Collections.Generic;

namespace Notify.Domain.MenuDomain
{
    /// <summary>
    /// 菜单子集
    /// </summary>
    public class SubMenu
    {
        /// <summary>
        /// 菜单资源集合
        /// </summary>
        public readonly List<Resource> m_resources;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SubMenu()
        {
            this.m_resources = new List<Resource>();
        }

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
        /// 菜单资源集合
        /// </summary>
        public IEnumerable<Resource> Resources => m_resources.AsReadOnly();

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns>子菜单</returns>
        public SubMenu Clone()
        {
            var result = new SubMenu
            {
                Id = this.Id,
                Title = this.Title,
                Url = this.Url,
                ParentId = this.ParentId,
                Sort = this.Sort,
                Description = this.Description,
                Icon = this.Icon
            };
            foreach (var item in m_resources)
            {
                result.AppendResource(item.Clone());
            }
            return result;
        }

        /// <summary>
        /// 添加资源菜单
        /// </summary>
        /// <param name="item">资源菜单</param>
        internal void AppendResource(Resource item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            //如果有同一资源菜单就不进行添加了
            if (this.m_resources.Any(menu => menu.Id == item.Id))
            {
                return;
            }
            this.m_resources.Add(item);
        }

        /// <summary>
        /// 是否包含菜单
        /// </summary>
        /// <param name="address">菜单地址</param>
        /// <returns>结果</returns>
        internal bool ContainValidResource(string address)
        {
            return string.Compare(this.Url, address, StringComparison.OrdinalIgnoreCase) == 0 || m_resources.Any(item => item.IsSameAddress(address));
        }
    }
}