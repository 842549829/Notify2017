using System;
using Notify.Model.Transfer;

namespace Notify.Domain.RoleDomain
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// ID
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
        /// 角色修改
        /// </summary>
        /// <param name="tRole">角色信息</param>
        public void Update(TRole tRole)
        {
            this.RoleName = tRole.RoleName;
            this.RoleDescription = tRole.RoleDescription;
        }
    }
}