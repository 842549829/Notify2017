using System;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// 角色
    /// </summary>
    public class TRole
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
    }
}