using System.Web;
using System.Web.Mvc;
using Notify.Code.Code;
using Notify.Code.Extension;

namespace Notify.Controller.Base.Filters
{
    /// <summary>
    /// 过滤器公用方法
    /// </summary>
    public class CommonFilters
    {
        /// <summary>
        /// 处理Request请求
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        /// <param name="url">url</param>
        /// <param name="message">消息</param>
        public static void ProcessingWithoutPermission(AuthorizationContext filterContext, string url, string message)
        {
            var request = filterContext.HttpContext.Request;
            if (request != null && request.IsAjaxRequest())
            {
                ProcessingAjax(filterContext, message);
            }
            else
            {
                filterContext.Result = new RedirectTopResult(url);
            }
        }

        /// <summary>
        /// 处理Request请求
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        /// <param name="message">message</param>
        public static void ProcessingExceptionPermission(AuthorizationContext filterContext, string message)
        {
            var request = filterContext.HttpContext.Request;
            if (request != null && request.IsAjaxRequest())
            {
                var rel = new { Type = "ExceptionPermission", Result = message };
                ProcessingAjax(filterContext, rel.SerializeObject());
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "ExceptionPermission", ViewData = new ViewDataDictionary(message) };
            }
        }

        /// <summary>
        /// 处理Request请求
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        /// <param name="message">message</param>
        public static void ProcessingExceptionPermission(ExceptionContext filterContext, string message)
        {
            var request = filterContext.HttpContext.Request;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            if (request != null && request.IsAjaxRequest())
            {
                var rel = new { Type = "ExceptionPermission", Result = message };
                ProcessingAjax(filterContext, rel.SerializeObject());
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.Result = new ViewResult { ViewName = "ExceptionPermission", ViewData = new ViewDataDictionary("系统错误") };
            }
        }

        /// <summary>
        /// 处理ajax请求
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        /// <param name="message">message</param>
        private static void ProcessingAjax(ExceptionContext filterContext, string message)
        {
            filterContext.Result = new JsonResult();
            HttpResponseBase response = filterContext.HttpContext.Response;
            response.ClearContent();
            response.StatusCode = 300;
            response.Write(message);
            response.End();
        }

        /// <summary>
        /// 处理ajax请求
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        /// <param name="message">message</param>
        private static void ProcessingAjax(AuthorizationContext filterContext, string message)
        {
            filterContext.Result = new JsonResult();
            HttpResponseBase response = filterContext.HttpContext.Response;
            response.ClearContent();
            response.StatusCode = 300;
            response.Write(message);
            response.End();
        }

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <returns>url</returns>
        public static string GetLoginUrl()
        {
            return "/Unauthorized/Login";
        }
    }
}
