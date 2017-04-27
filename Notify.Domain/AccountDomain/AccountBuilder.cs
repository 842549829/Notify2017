using System;
using System.Collections.Generic;
using System.Linq;
using Notify.Domain.VerificationCodeDomain;
using Notify.Mail.Mdoel;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.Domain.AccountDomain
{
    /// <summary>
    /// 账户服务工厂
    /// </summary>
    public static class AccountBuilder
    {
        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mAccount">MAccount</param>
        /// <returns>Account</returns>
        public static Account ToAccount(this MAccount mAccount)
        {
            var account = new Account(mAccount.Id)
            {
                AccountName = mAccount.AccountName,
                AccountNO = mAccount.AccountNo,
                Mail = mAccount.Mail,
                Mobile = mAccount.Mobile,
                Password = mAccount.Password,
                PayPassword = mAccount.PayPassword,
                CreateTime = mAccount.CreateTime,
                IsAdmin = mAccount.IsAdmin,
                Status = mAccount.Status
            };
            return account;
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mAccount">MAccount</param>
        /// <returns>TAccount</returns>
        public static TAccount ToTAccount(this MAccount mAccount)
        {
            var account = new TAccount
            {
                Id = mAccount.Id,
                AccountName = mAccount.AccountName,
                AccountNo = mAccount.AccountNo,
                Mail = mAccount.Mail,
                Mobile = mAccount.Mobile,
                CreateTime = mAccount.CreateTime,
                IsAdmin = mAccount.IsAdmin,
                Status = mAccount.Status
            };
            return account;
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="account">Account</param>
        /// <returns>TAccount</returns>
        public static TAccount ToTAccount(this Account account)
        {
            var tAccount = new TAccount
            {
                Id = account.Key,
                AccountName = account.AccountName,
                AccountNo = account.AccountNO,
                Mail = account.Mail,
                Mobile = account.Mobile,
                CreateTime = account.CreateTime
            };
            return tAccount;
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="account">Account</param>
        /// <returns>MAccount</returns>
        public static MAccount ToMAccount(this Account account)
        {
            return new MAccount
            {
                Id = account.Key,
                AccountName = account.AccountName,
                AccountNo = account.AccountNO,
                Mail = account.Mail,
                Mobile = account.Mobile,
                Password = account.Password,
                PayPassword = account.PayPassword,
                CreateTime = account.CreateTime,
                IsAdmin = account.IsAdmin,
                Status = account.Status
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <returns>MAccount</returns>
        public static MAccount ToMAccount(this Guid accountId)
        {
            return new MAccount
            {
                Id = accountId
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="account">Account</param>
        /// <returns>Register</returns>
        public static Register ToRegister(this Account account)
        {
            return new Register
            {
                Account = account.AccountNO,
                AccountName = account.AccountName,
                Code = account.VerificationCode.Code,
                SendDateTime = DateTime.Now.ToString("yyyy年MM月dd日")
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mVerificationCode">MVerificationCode</param>
        /// <returns>VerificationCode</returns>
        public static VerificationCode ToVerificationCode(this MVerificationCode mVerificationCode)
        {
            return new VerificationCode(mVerificationCode.Id)
            {
                AccountId = mVerificationCode.AccountId,
                Code = mVerificationCode.Code,
                Contact = mVerificationCode.Code,
                Type = mVerificationCode.Type,
                CreateTime = mVerificationCode.CreateTime
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="verificationCode">VerificationCode</param>
        /// <returns>MVerificationCode</returns>
        public static MVerificationCode ToMVerificationCode(this VerificationCode verificationCode)
        {
            return new MVerificationCode
            {
                Id = verificationCode.Key,
                AccountId = verificationCode.AccountId,
                Code = verificationCode.Code,
                Contact = verificationCode.Code,
                Type = verificationCode.Type,
                CreateTime = verificationCode.CreateTime
            };
        }

        /// <summary>
        /// 对象转化
        /// </summary>
        /// <param name="mAccount">MAccount</param>
        /// <returns>TAccount</returns>
        public static IEnumerable<TAccount> ToTAccounts(this IEnumerable<MAccount> mAccount)
        {
            return mAccount.Select(ToTAccount);
        }
    }
}