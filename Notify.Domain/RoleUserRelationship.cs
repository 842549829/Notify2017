using System;

namespace Notify.Domain
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    public class RoleUserRelationship
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid RoleId { get; set; }
    }
}