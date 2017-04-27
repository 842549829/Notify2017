using System;
using System.Web;
using System.Web.Mvc;

namespace Notify.Controller.Base.Filters
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class OperatingAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 控制器执行之前
        /// </summary>
        /// <param name="filterContext">控制器执行上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();

            // 验证权限
            if (!PermissionValidation(controllerName, actionName))
            {
                ProcessingResults(filterContext);
            }
        }

        /// <summary>
        /// 没有权限处理结果
        /// </summary>
        /// <param name="filterContext">当前上下文</param>
        public static void ProcessingResults(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (request != null && request.IsAjaxRequest())
            {
                HttpResponseBase response = filterContext.HttpContext.Response;
                response.ClearContent();
                response.StatusCode = 300;
                filterContext.Result = new JsonResult { Data = new { Type = "Unauthorized" } };
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "WithoutPermission", ViewData = new ViewDataDictionary("无菜单权限,无权访问") };
            }
        }

        /// <summary>
        /// 权限查询
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="action">action</param>
        /// <returns>result</returns>
        public static bool PermissionValidation(string controller, string action)
        {
            var url = GetUrlMenu(controller, action);
            return BaseController.HasPermission(url);
        }

        /// <summary>
        /// 获取菜单URL
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="action">action</param>
        /// <returns>菜单URL</returns>
        public static string GetUrlMenu(string controller, string action)
        {
            return $"/{controller}/{action}";
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="controller">controller</param>
        /// <returns>结果</returns>
        public static bool IsAuthorization(string action, string controller)
        {
            return PermissionValidation(controller, action);
        }
    }
}