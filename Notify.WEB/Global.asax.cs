using System.Web.Mvc;
using System.Web.Routing;

namespace Notify.WEB
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// The mvc application.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The application_ start.
        /// </summary>
        protected void Application_Start()
        {
            // 注册MVC区域
            AreaRegistration.RegisterAllAreas();

            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            // 注册过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // 注册路由规则
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 注册视图规则
            ViewEngine.RegisterView(ViewEngines.Engines);
        }
    }
}