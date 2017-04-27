using System.Web.Mvc;
using System.Web.Routing;

namespace Notify.WEB
{
    /// <summary>
    /// 路由
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// 注册路由.
        /// </summary>
        /// <param name="routes">
        /// 路由集合
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 默认路由规则
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}