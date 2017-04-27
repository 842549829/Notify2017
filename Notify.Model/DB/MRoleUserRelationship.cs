using System;
using Notify.Infrastructure.DomainBase;

namespace Notify.Model.DB
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    public class MRoleUserRelationship : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public object Key => this.Id;
    }
}