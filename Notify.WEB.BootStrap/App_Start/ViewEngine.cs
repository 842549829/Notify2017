using System.Web.Mvc;

namespace Notify.WEB.BootStrap.App_Start
{
    /// <summary>
    /// 自定义视图
    /// </summary>
    public class ViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewEngine"/> class.
        /// </summary>
        public ViewEngine()
        {
            var views = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Account/{1}/{0}.cshtml",
                "~/Views/Permission/{1}/{0}.cshtml",
                "~/Views/SwfupLoad/{1}/{0}.cshtml"
            };

            this.ViewLocationFormats = views;

            this.PartialViewLocationFormats = views;
        }

        /// <summary>
        /// 添加视图规则
        /// </summary>
        /// <param name="viewEngineCollection">viewEngineCollection</param>
        internal static void RegisterView(ViewEngineCollection viewEngineCollection)
        {
            viewEngineCollection.Add(new ViewEngine());
        }
    }
}