using System;
using System.Web.Mvc;
using Common.Code;
using Notify.Code.Code;
using Notify.Controller.Base;
using Notify.Model.Transfer;
using Notify.Service;
using MvcPager;
using Result = Notify.Code.Code.Result;

namespace Notify.Controller.Account
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns>结果</returns>
        public ActionResult UserList(TAccountCondition condition)
        {
            if (condition != null)
            {
                var data = AccountService.QueryAccountByPagings(condition);
                PagedList<TAccount> pageList = new PagedList<TAccount>(data, condition.PageIndex, condition.PageSize, condition.RowsCount);
                ViewModel<TAccountCondition, PagedList<TAccount>> result = new ViewModel<TAccountCondition, PagedList<TAccount>>
                {
                     Condition = condition,
                     Data  = pageList
                };
                return this.View(result);
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public ActionResult UserListVal(TAccountCondition condition)
        {
            var data = AccountService.QueryAccountByPaging(condition);
            return new MyJsonResult { Data = data };
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="registerAccount">用户信息</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public ActionResult AddUser(RegisterAccount registerAccount)
        {
            registerAccount.Password = "123456";
            registerAccount.PayPassword = "123456";
            var result = AccountService.AddUser(registerAccount, null);
            return new MyJsonResult { Data = result };
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="tAccount">用户信息</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public ActionResult UpdateUser(TAccount tAccount)
        {
            var result = ValidateTAccount(tAccount);
            if (!result.IsSucceed)
            {
                return new MyJsonResult { Data = result };
            }
            result = AccountService.UpdateUser(tAccount, null);
            return new MyJsonResult { Data = result };
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户信息</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public ActionResult RemoveUser(Guid userId)
        {
            var result = AccountService.RemoveUser(userId, null);
            return new MyJsonResult { Data = result };
        }

        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="tAccount">用户信息</param>
        /// <returns>角色</returns>
        private static Result ValidateTAccount(TAccount tAccount)
        {
            throw new NotImplementedException();
        }
    }
}