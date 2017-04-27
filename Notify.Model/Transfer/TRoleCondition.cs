using Notify.Code.Code;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// 角色查询条件
    /// </summary>
    public class TRoleCondition: EsayUIPaging
    {
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