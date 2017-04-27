using System;
using Notify.Infrastructure.DomainBase;

namespace Notify.Model.DB
{
    /// <summary>
    /// 角色
    /// </summary>
    public class MRole: IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string RoleDescription { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public object Key => this.Id;
    }
}