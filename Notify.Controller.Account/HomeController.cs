using System.Web.Mvc;
using Notify.Code.Code;
using Notify.Controller.Base;

namespace Notify.Controller.Account
{
    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// 登录首页
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// 登录首页
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Default()
        {
            return this.View();
        }

        /// <summary>
        /// QueryMenu
        /// </summary>
        /// <returns>ActionResult</returns>
        [AcceptVerbs("POST")]
        public ActionResult QueryMenu()
        {
            var data = EsayUIMenu;
            return new MyJsonResult { Data = data };
        }

        /// <summary>
        /// QueryMenu
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult QueryTMenu()
        {
            return this.PartialView();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Logoff()
        {
            LogonUtility.Logoff();
            var result = new Result { IsSucceed = true };
            return new MyJsonResult { Data = result };
        }
    }
}