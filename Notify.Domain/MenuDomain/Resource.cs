using System;

namespace Notify.Domain.MenuDomain
{
    /// <summary>
    /// 菜单资源
    /// </summary>
    public class Resource
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
        /// 克隆
        /// </summary>
        /// <returns>资源菜单</returns>
        public Resource Clone()
        {
            return new Resource
            {
                Id = this.Id,
                Title = this.Title,
                Url = this.Url,
                ParentId = this.ParentId,
                Sort = this.Sort,
                Description = this.Description,
                Icon = this.Icon
            };
        }

        /// <summary>
        /// 重新ToString
        /// </summary>
        /// <returns>返回资源信息</returns>
        public override string ToString()
        {
            return $"名称：{this.Title} 地址:{this.Url} 序号：{this.Sort} 菜单图标：{this.Icon} 描述：{this.Description}";
        }

        /// <summary>
        /// 地址是否相同
        /// </summary>
        /// <param name="address">菜单地址</param>
        /// <returns>结果</returns>
        internal bool IsSameAddress(string address)
        {
            return string.Compare(this.Url, address, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
