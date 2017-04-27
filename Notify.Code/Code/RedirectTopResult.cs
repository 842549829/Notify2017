using System;
using System.Web.Mvc;

namespace Notify.Code.Code
{
    /// <summary>
    /// 跳出父框架
    /// </summary>
    public class RedirectTopResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectTopResult"/> class.
        /// </summary>
        /// <param name="url">url</param>
        public RedirectTopResult(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context">context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.IsChildAction)
            {
                throw new InvalidOperationException();
            }
            string url = UrlHelper.GenerateContentUrl(this.Url, context.HttpContext);
            context.Controller.TempData.Keep();
            var result = CreateScript(url);
            context.HttpContext.Response.Write(result);
            context.HttpContext.Response.End();
        }

        /// <summary>
        /// 创建跳转脚本
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>脚本</returns>
        private static string CreateScript(string url)
        {
            return $"<script> window.top.location.href = '{url}';</script>";
        }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; private set; }
    }
}
