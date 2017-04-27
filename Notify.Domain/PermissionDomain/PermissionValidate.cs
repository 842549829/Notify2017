using System;
using System.Collections.Generic;
using System.Linq;
using Notify.Code.Exception;

namespace Notify.Domain.PermissionDomain
{
    /// <summary>
    /// 权限验证
    /// </summary>
    public class PermissionValidate
    {
        /// <summary>
        /// 验证菜单
        /// </summary>
        /// <param name="menuIds">菜单</param>
        public static void ValidateMenuIds(List<Guid> menuIds)
        {
            if (menuIds == null || !menuIds.Any())
            {
                throw new CustomException("请勾选权限");
            }
        }
    }
}