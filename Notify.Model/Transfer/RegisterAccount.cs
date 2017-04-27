namespace Notify.Model.Transfer
{
    /// <summary>
    /// 账户注册
    /// </summary>
    public class RegisterAccount
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string AccountNO { get; set; }

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
        public string Mobile { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }
    }
}