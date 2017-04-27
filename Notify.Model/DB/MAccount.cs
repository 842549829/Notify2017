using System;
using Notify.Infrastructure.DomainBase;

namespace Notify.Model.DB
{
    /// <summary>
    /// 用户
    /// </summary>
    public class MAccount: IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 账户名(昵称)
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile{ get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }

        /// <summary>
        ///  插入时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 帐号状态
        /// </summary>
        public AccountStatus Status { get; set; }

        /// <summary>
        /// 主键Key
        /// </summary>
        public object Key => this.Id;
    }
}