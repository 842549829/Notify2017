using Notify.Code.Exception;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.Domain.AccountDomain
{
    /// <summary>
    /// 用户验证
    /// </summary>
    public class AccountValidate
    {
        /// <summary>
        /// 验证登录信息
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        public static void ValdateLoginInfo(LoginInfo loginInfo)
        {
            if (loginInfo == null)
            {
                throw new CustomException("登录信息为空");
            }
            if (string.IsNullOrWhiteSpace(loginInfo.AccountNo))
            {
                throw new CustomException("登录帐号为空");
            }
            if (string.IsNullOrWhiteSpace(loginInfo.LoginPassword))
            {
                throw new CustomException("登录密码为空");
            }
            if (string.IsNullOrWhiteSpace(loginInfo.ClinetIp))
            {
                throw new CustomException("登录IP为空");
            }
        }

        /// <summary>
        /// 验证用户信息(登录)
        /// </summary>
        /// <param name="mAccount">用户信息</param>
        public static void ValdateLoginMAccount(MAccount mAccount)
        {
            if (mAccount == null)
            {
                throw new CustomException("帐号不存在");
            }
            if (mAccount.Status == Model.AccountStatus.Disable)
            {
                throw new CustomException("帐号已被禁用");
            }
        }
    }
}