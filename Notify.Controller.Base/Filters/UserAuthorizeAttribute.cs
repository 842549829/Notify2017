using System;
using System.Web.Mvc;
using Notify.Code.Write;
using Notify.Code.Extension;

namespace Notify.Controller.Base.Filters
{
    /// <summary>
    /// 用户是否登录筛选器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 登录筛选器实现
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (!BaseController.Logoned)
                {
                    ProcessingWithoutPermission(filterContext);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "验证登录权限出错");
                ProcessingExceptionPermission(filterContext, "验证登录权限出错");
            }
        }

        /// <summary>
        /// 处理没有登录授权的
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        private static void ProcessingWithoutPermission(AuthorizationContext filterContext)
        {
            string url = CommonFilters.GetLoginUrl();
            var message = new { Type = "RequireLogon", Result = CommonFilters.GetLoginUrl() };
            CommonFilters.ProcessingWithoutPermission(filterContext, url, message.SerializeObject());
        }

        /// <summary>
        /// 处理异常的
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        /// <param name="message">message</param>
        private static void ProcessingExceptionPermission(AuthorizationContext filterContext, string message)
        {
            CommonFilters.ProcessingExceptionPermission(filterContext, message);
        }
    }
}
