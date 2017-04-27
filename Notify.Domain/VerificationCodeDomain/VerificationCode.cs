using System;
using Notify.Code.Code;
using Notify.Domain.AccountDomain;
using Notify.Infrastructure.DomainBase;
using Notify.Model;

namespace Notify.Domain.VerificationCodeDomain
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class VerificationCode : EntityBase<Guid>
    {
        public VerificationCode(Guid id)
            : base(id)
        {
        }

        public VerificationCode(Account account)
            : base(Guid.NewGuid())
        {
            this.AccountId = account.Key;
        }

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
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public void CreateMailCode(string mail)
        {
            this.Type = VerificationCodeType.Mail;
            this.Contact = mail;
            this.CreateCode();
        }

        public void CreateMobileCode(string mobile)
        {
            this.Type = VerificationCodeType.Mobile;
            this.Contact = mobile;
            this.CreateCode();
        }

        private void CreateCode()
        {
            Random random = new Random(Common.CreateRandomSeed());
            var result = random.Next(10000, 99999);
            this.Code = result.ToString();
        }
    }
}