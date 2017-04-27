using System;
using Notify.Infrastructure.DomainBase;

namespace Notify.Model.DB
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class MRolePermissions: IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public object Key => this.Id;
    }
}