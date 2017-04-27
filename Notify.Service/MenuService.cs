using System;
using System.Collections.Generic;
using Notify.Code.Code;
using Notify.Code.Constant;
using Notify.Code.Write;
using Notify.Domain.MenuDomain;
using Notify.Domain.PermissionDomain;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.Service
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public class MenuService
    {
        /// <summary>
        /// 数据工厂(当前上下文)
        /// </summary>
        public static IDbFactory.IDbFactory DbContext = Factory.DbContext;

        /// <summary>
        /// 菜单查询(Ztree)
        /// </summary>
        /// <returns>结果</returns>
        public static IEnumerable<MMenu> QueryMenus()
        {
            using (var menuRepository = DbContext.CreateIMenuRepository())
            {
                var data = menuRepository.QueryMenus();
                return data;
            }
        }

        /// <summary>
        /// 菜单查询(Ztree)
        /// </summary>
        /// <returns>结果</returns>
        public static IEnumerable<ZtreeMenu> QueryZtreeMenus()
        {
            var data = QueryMenus().ToZtreeMenu();
            return data;
        }

        /// <summary>
        /// 菜单查询(EsayUI)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<EsayUIMenu> QueryEsayUIMenus()
        {
            var data = QueryMenus().ToEsayUIMenus();
            return data;
        }

        /// <summary>
        /// 查询菜单(根据菜单Id查询)
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <returns>菜单</returns>
        public static TMenu QueryMenuById(Guid menuId)
        {
            using (var menuRepository = DbContext.CreateIMenuRepository())
            {
                var data = menuRepository.Query(menuId).ToTMenu();
                return data;
            }
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="tMenu">菜单信息</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result AddMenu(TMenu tMenu, Operational operational)
        {
            Result result = new Result();
            try
            {
                using (var menuRepository = DbContext.CreateIMenuRepository())
                {
                    if (tMenu.Id == Guid.Empty)
                    {
                        tMenu.Id = Guid.NewGuid();
                    }
                    var mMenu = tMenu.ToMMenu();
                    menuRepository.Add(mMenu);
                }

                result.IsSucceed = true;
                result.Message = "添加成功";

            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.ErrorMessage;
                LogService.WriteLog(ex, "添加菜单");
            }
            return result;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="tMneu">菜单信息</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result UpdateMenu(TMenu tMneu, Operational operational)
        {
            Result result = new Result();
            try
            {
                using (var menuRepository = DbContext.CreateIMenuRepository())
                {
                    var mMenu = tMneu.ToMMenu();
                    menuRepository.Update(mMenu);
                }

                result.IsSucceed = true;
                result.Message = "修改成功";
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.ErrorMessage;
                LogService.WriteLog(ex, "修改菜单");
            }
            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单信息</param>
        /// <param name="operational">操作信息</param>
        /// <returns>结果</returns>
        public static Result RemoveMenu(Guid menuId, Operational operational)
        {
            Result result = new Result();
            try
            {
                using (var menuRepository = DbContext.CreateIMenuRepository())
                {
                    var mMenu = menuId.ToMMenu();
                    menuRepository.Remove(mMenu);
                }

                result.IsSucceed = true;
                result.Message = "删除成功";
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = Const.ErrorMessage;
                LogService.WriteLog(ex, "修改菜单");
            }
            return result;
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="menus">菜单</param>
        /// <param name="address">菜单地址</param>
        /// <returns>结果</returns>
        public static bool HasPermission(IEnumerable<TMenu> menus, string address)
        {
            return PermissionCollection.HasPermission(menus.ToMenus(), address);
        }
    }
}