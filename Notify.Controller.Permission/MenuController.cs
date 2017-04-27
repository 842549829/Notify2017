using System;
using System.Web.Mvc;
using Notify.Controller.Base;
using Notify.Controller.Base.Filters;
using Notify.Model.Transfer;
using Notify.Service;

namespace Notify.Controller.Permission
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    public class MenuController : BaseController
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns>视图</returns>
        [OperatingAuthorize]
        public ActionResult MenuList()
        {
            return this.View();
        }

        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public JsonResult QueryMenuList()
        {
            var data = MenuService.QueryZtreeMenus();
            return this.Json(data);
        }

        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public JsonResult QueryMenuById(Guid menuId)
        {
            var data = MenuService.QueryMenuById(menuId);
            return this.Json(data);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public JsonResult RemoveMenu(Guid menuId)
        {
            var data = MenuService.RemoveMenu(menuId, GetOperational());
            return this.Json(data);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="tMenu">菜单</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public JsonResult AddMenu(TMenu tMenu)
        {
            var data = MenuService.AddMenu(tMenu, GetOperational());
            return this.Json(data);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="tMenu">菜单</param>
        /// <returns>结果</returns>
        [AcceptVerbs("POST")]
        public JsonResult UpdateMenu(TMenu tMenu)
        {
            var data = MenuService.UpdateMenu(tMenu, GetOperational());
            return this.Json(data);
        }
    }
}