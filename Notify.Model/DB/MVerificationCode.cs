using System;
using Notify.Infrastructure.DomainBase;

namespace Notify.Model.DB
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class MVerificationCode: IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public VerificationCodeType Type { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 账户Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 联系方法(手机Or邮箱)
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 主键Key
        /// </summary>
        public object Key => this.Id;
    }
}