using Notify.DbCommon.UnitOfWork;
using Notify.Infrastructure.UnitOfWork;
using Notify.IRepository;
using Notify.Repository.SqlServer;

namespace Notify.SqlServerDbFactory
{
    /// <summary>
    /// Sqlserver
    /// </summary>
    public class SqlServerFactory : IDbFactory.IDbFactory
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public override string DbString => ConnectionString.SqlDb;

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <returns>用户仓储</returns>
        public override IAccountesRepository CreateIAccountesRepository()
        {
            return new AccountesRepository(null, DbString);
        }

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>用户仓储</returns>
        public override IAccountesRepository CreateIAccountesRepository(IPowerUnitOfWork unit)
        {
            return new AccountesRepository(unit, null);
        }

        /// <summary>
        /// 创建注册验证码仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>注册验证码仓储</returns>
        public override IVerificationCodeRepository CreateIVerificationCodeRepository(IPowerUnitOfWork unit)
        {
            return new VerificationCodeRepository(unit, null);
        }

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <returns>角色仓储</returns>
        public override IRoleRepository CreateIRoleRepository()
        {
            return new RoleRepository(null, DbString);
        }

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>角色仓储</returns>
        public override IRoleRepository CreateIRoleRepository(IPowerUnitOfWork unit)
        {
            return new RoleRepository(unit, null);
        }

        /// <summary>
        /// 创建菜单仓储
        /// </summary>
        /// <returns>菜单仓储</returns>
        public override IMenuRepository CreateIMenuRepository()
        {
            return new MenuRepository(null, DbString);
        }

        /// <summary>
        /// 创建菜单仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>菜单仓储</returns>
        public override IMenuRepository CreateIMenuRepository(IPowerUnitOfWork unit)
        {
            return new MenuRepository(unit, null);
        }

        /// <summary>
        /// 创建角色权限仓储
        /// </summary>
        /// <returns>角色权限仓储</returns>
        public override IRolePermissionsRepository CreateIRolePermissionsRepository()
        {
            return new RolePermissionsRepository(null, DbString);
        }

        /// <summary>
        /// 创建角色权限仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>角色权限仓储</returns>
        public override IRolePermissionsRepository CreateIRolePermissionsRepository(IPowerUnitOfWork unit)
        {
            return new RolePermissionsRepository(unit, null);
        }

        /// <summary>
        /// 创建用户角色关系仓储
        /// </summary>
        /// <returns>用户角色关系仓储</returns>
        public override IRoleUserRelationshipRepository CreateIRoleUserRelationshipRepository()
        {
            return new RoleUserRelationshipRepository(null, DbString);
        }

        /// <summary>
        /// 创建用户角色关系仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>用户角色关系仓储</returns>
        public override IRoleUserRelationshipRepository CreateIRoleUserRelationshipRepository(IPowerUnitOfWork unit)
        {
            return new RoleUserRelationshipRepository(unit, null);
        }
    }
}