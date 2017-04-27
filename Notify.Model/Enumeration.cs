using System.ComponentModel;

namespace Notify.Model
{
    public enum Enumeration
    {
    }

    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerificationCodeType
    {
        /// <summary>
        /// 短信
        /// </summary>
        [Description("短信")]
        Mobile = 0,

        /// <summary>
        /// 邮件
        /// </summary>
        [Description("邮件")]
        Mail = 1
    }

    /// <summary>
    /// 帐号状态
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enabled = 1
    }
}