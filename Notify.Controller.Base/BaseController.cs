using System;
using System.Collections.Generic;
using Notify.Code.Code;
using Notify.Code.Constant;
using Notify.Controller.Base.Filters;
using Notify.Model.Transfer;
using Notify.Service;

namespace Notify.Controller.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [UserAuthorize]
    public class BaseController : System.Web.Mvc.Controller
    {
        /// <summary>
        /// 操作信息
        /// </summary>
        public static Operational GetOperational()
        {
            return new Operational
            {
                Operator = LogonUser.AccountNo,
                OperationDateTime = DateTime.Now
            };
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static TAccount LogonUser
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null || System.Web.HttpContext.Current.Session[Const.UserSessionKey] == null)
                {
                    return null;
                }
                return System.Web.HttpContext.Current.Session[Const.UserSessionKey] as TAccount;
            }
        }

        /// <summary>
        /// 权限菜单
        /// </summary>
        public static IEnumerable<TMenu> Menu
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null || System.Web.HttpContext.Current.Session[Const.MenuSessionKey] == null)
                {
                    return null;
                }
                return System.Web.HttpContext.Current.Session[Const.MenuSessionKey] as IEnumerable<TMenu>;
            }
        }

        /// <summary>
        /// 权限菜单(EsayUI)
        /// </summary>
        public static IEnumerable<EsayUIMenu> EsayUIMenu
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null || System.Web.HttpContext.Current.Session[Const.EsayUIMenuSessionKey] == null)
                {
                    return null;
                }
                return System.Web.HttpContext.Current.Session[Const.EsayUIMenuSessionKey] as IEnumerable<EsayUIMenu>;
            }
        }

        /// <summary>
        /// 登录状态
        /// </summary>
        public static bool Logoned => LogonUser != null && LogonUser.Id != Guid.Empty;

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="address">菜单地址</param>
        /// <returns>结果</returns>
        public static bool HasPermission(string address)
        {
            return MenuService.HasPermission(Menu, address);
        }
    }
}