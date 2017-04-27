using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Notify.Code.Code;
using Notify.Code.Constant;
using Notify.Code.Exception;
using Notify.Code.Mail;
using Notify.Code.Write;
using Notify.Domain.AccountDomain;
using Notify.Mail;
using Notify.Model.Transfer;

namespace Notify.Service
{
    public class AccountService
    {
        /// <summary>
        /// 数据工厂(当前上下文)
        /// </summary>
        public static IDbFactory.IDbFactory DbContext => Factory.DbContext;

        /// <summary>
        /// 事物提交多个
        /// </summary>
        public static void AddAccountes()
        {
            // 通过 工厂方法(反射) 获取数据库抽象工厂
            // var factory = DbFactoryContext;

            /*无事物操作*/
            //Account account = new Account { Id = Guid.NewGuid(), AccountNO = "xxwsd@qq.com", Name = "刘小吉" };
            //IAccountesRepository repository = factory.CreateIAccountesRepository();
            //repository.Add(account);

            /*事物操作*/
            //Account account1 = new Account { Id = Id, AccountNO = "xxwsd@qq.com", Name = "刘小吉" };
            //Address address = new Address { AccountId = account1.Id, Name = "成都" };
            //using (IPowerUnitOfWork unit = factory.CreateIPowerUnitOfWork())
            //{
            //    IAccountesRepository repository1 = factory.CreateIAccountesRepository(unit);
            //    repository1.Add(account1);

            //    IAddressRepository addressRepository = factory.CreateIAddressRepository(unit);
            //    addressRepository.Add(address);
            //    unit.Complete();
            //}
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>登录结果</returns>
        public static LoginResult Login(LoginInfo loginInfo)
        {
            LoginResult result = new LoginResult();
            try
            {
                // 验证登录基本信息
                AccountValidate.ValdateLoginInfo(loginInfo);

                // 加载用户信息
                var mAccount = Domain.AccountDomain.AccountService.QueryMAccountByAccountNo(loginInfo.AccountNo);

                // 验证用户信息
                AccountValidate.ValdateLoginMAccount(mAccount);

                // 加载用户领域对象
                var account = mAccount.ToAccount();

                // 登录
                result = account.Login(loginInfo.LoginPassword);
            }
            catch (CustomException ex)
            {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.LoginFail;

                // 记录异常日志
                LogService.WriteLog(ex, Const.Login);
            }

            return result;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerAccount">注册信息</param>
        /// <returns>注册结果</returns>
        public static Result Register(RegisterAccount registerAccount)
        {
            Result result = new Result();
            try
            {
                // 创建用户对象 执行注册方法和创建验证码方法
                var account = new Account(registerAccount);
                account.Register();

                // 将领域对象转化成数据库实体对象
                var mAccount = account.ToMAccount();
                var mVerificationCode = account.VerificationCode.ToMVerificationCode();

                // 通过工资单元持久化数据
                using (var unit = DbContext.CreateIPowerUnitOfWork())
                {
                    var accountesRepository = DbContext.CreateIAccountesRepository(unit);
                    var verificationCodeRepository = DbContext.CreateIVerificationCodeRepository(unit);

                    accountesRepository.Add(mAccount);
                    verificationCodeRepository.Add(mVerificationCode);

                    unit.Complete();
                }

                result.IsSucceed = true;

                // 异步调用发送邮件的方法
                Task.Factory.StartNew(() =>
                {
                    RegisterSendMail(account);
                });
            }
            catch (CustomException ex)
            {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = "注册失败";

                // 记录异常日志
                LogService.WriteLog(ex, "注册帐号");
            }
            return result;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="account">账户信息</param>
        private static void RegisterSendMail(Account account)
        {
            var register = account.ToRegister();
            MailBase mail = new RegisterMail(register);
            var content = mail.GetTemplateContent();
            Send send = new Send();
            send.SendEmail("842549829@qq.com", "密码", account.Mail, "注册", content, null);
        }

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        public static EsayUIQueryResult<TAccount> QueryAccountByPaging(TAccountCondition condition)
        {
            using (var accountesRepository = DbContext.CreateIAccountesRepository())
            {
                var data = accountesRepository.QueryAccountByPaging(condition).ToTAccounts();
                EsayUIQueryResult<TAccount> roles = new EsayUIQueryResult<TAccount>
                {
                    rows = data,
                    total = condition.RowsCount
                };
                return roles;
            }
        }

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        public static IEnumerable<TAccount> QueryAccountByPagings(TAccountCondition condition)
        {
            using (var accountesRepository = DbContext.CreateIAccountesRepository())
            {
                var data = accountesRepository.QueryAccountByPaging(condition).ToTAccounts();
                return data;
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="registerAccount">用户信息</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result AddUser(RegisterAccount registerAccount, Operational operational)
        {
            Result result = new Result();
            try
            {
                // 创建用户对象
                var account = new Account(registerAccount);
                account.AddUser();

                // 将领域对象转化成数据库实体对象
                var mAccount = account.ToMAccount();

                using (var accountesRepository = DbContext.CreateIAccountesRepository())
                {
                    accountesRepository.Add(mAccount);
                }

                result.IsSucceed = true;
                result.Message = "添加成功";
            }
            catch (CustomException ex)
            {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = "添加失败";

                // 记录异常日志
                LogService.WriteLog(ex, "添加用户");
            }
            return result;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="tAccount">用户信息</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result UpdateUser(TAccount tAccount, Operational operational)
        {
            return null;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户信息</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result RemoveUser(Guid userId, Operational operational)
        {
            Result result = new Result();
            try
            {
                using (var accountesRepository = DbContext.CreateIAccountesRepository())
                {
                    var mAccount = userId.ToMAccount();
                    accountesRepository.Remove(mAccount);
                }

                result.IsSucceed = true;
                result.Message = "删除成功";
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.ErrorMessage;
                LogService.WriteLog(ex, "删除用户");
            }
            return result;
        }
    }
}