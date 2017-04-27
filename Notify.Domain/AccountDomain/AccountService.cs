using System;
using Notify.DbCommon.Repositroies;
using Notify.Domain.VerificationCodeDomain;
using Notify.Model.DB;

namespace Notify.Domain.AccountDomain
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class AccountService
    {
        /// <summary>
        /// 当前上下文
        /// </summary>
        public static IDbFactory.IDbFactory DbContext { get; } = Factory.GetFactory<IDbFactory.IDbFactory>();

        /// <summary>
        /// 根据帐号查询
        /// </summary>
        /// <param name="accountNo">帐号</param>
        /// <returns>账户信息</returns>
        public static Account QueryAccountByAccountNo(string accountNo)
        {
            return QueryMAccountByAccountNo(accountNo).ToAccount();
        }

        /// <summary>
        /// 根据帐号查询用户信息
        /// </summary>
        /// <param name="accountNo">帐号</param>
        /// <returns>用户信息</returns>
        public static MAccount QueryMAccountByAccountNo(string accountNo)
        {
            using (var accountesRepository = DbContext.CreateIAccountesRepository())
            {
                return accountesRepository.Query(accountNo);
            }
        }

        /// <summary>
        /// 根据用户ID查询
        /// </summary>
        /// <param name="accountId">用户ID</param>
        /// <returns>用户信息</returns>
        public static Account QueryAccountById(Guid accountId)
        {
            using (var accountesRepository = DbContext.CreateIAccountesRepository())
            {
                return accountesRepository.Query(accountId).ToAccount();
            }
        }

        /// <summary>
        /// 根据用户Id查询验证码
        /// </summary>
        /// <param name="accountId">用户ID</param>
        /// <returns>验证码</returns>
        public static VerificationCode QueryVerificationCodeByAccountId(Guid accountId)
        {
            return null;
        }
    }
}