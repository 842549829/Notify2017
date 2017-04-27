using Notify.DbCommon.UnitOfWork;
using Notify.Infrastructure.UnitOfWork;
using Notify.IRepository;

namespace Notify.IDbFactory
{
    /// <summary>
    /// 数据抽象
    /// </summary>
    public abstract class IDbFactory
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public abstract string DbString { get; }

        /// <summary>
        /// 创建工厂单元
        /// </summary>
        /// <returns>工厂单元</returns>
        public IUnitOfWork CreateIUnitOfWork()
        {
            return new UnitOfWork(DbString);
        }

        /// <summary>
        /// 创建委托工作单元
        /// </summary>
        /// <returns>委托工作单元</returns>
        public IPowerUnitOfWork CreateIPowerUnitOfWork()
        {
            return new PowerUnitOfWork(DbString);
        }

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <returns>用户仓储</returns>
        public abstract IAccountesRepository CreateIAccountesRepository();

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>用户仓储</returns>
        public abstract IAccountesRepository CreateIAccountesRepository(IPowerUnitOfWork unit);

        /// <summary>
        /// 创建注册验证码仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>注册验证码仓储</returns>
        public abstract IVerificationCodeRepository CreateIVerificationCodeRepository(IPowerUnitOfWork unit);

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <returns>角色仓储</returns>
        public abstract IRoleRepository CreateIRoleRepository();

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>角色仓储</returns>
        public abstract IRoleRepository CreateIRoleRepository(IPowerUnitOfWork unit);

        /// <summary>
        /// 创建菜单仓储
        /// </summary>
        /// <returns>菜单仓储</returns>
        public abstract IMenuRepository CreateIMenuRepository();

        /// <summary>
        /// 创建菜单仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>菜单仓储</returns>
        public abstract IMenuRepository CreateIMenuRepository(IPowerUnitOfWork unit);

        /// <summary>
        /// 创建角色权限仓储
        /// </summary>
        /// <returns>角色权限仓储</returns>
        public abstract IRolePermissionsRepository CreateIRolePermissionsRepository();

        /// <summary>
        /// 创建角色权限仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>角色权限仓储</returns>
        public abstract IRolePermissionsRepository CreateIRolePermissionsRepository(IPowerUnitOfWork unit);

        /// <summary>
        /// 创建用户角色关系仓储
        /// </summary>
        /// <returns>用户角色关系仓储</returns>
        public abstract IRoleUserRelationshipRepository CreateIRoleUserRelationshipRepository();

        /// <summary>
        /// 创建用户角色关系仓储
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <returns>用户角色关系仓储</returns>
        public abstract IRoleUserRelationshipRepository CreateIRoleUserRelationshipRepository(IPowerUnitOfWork unit);
    }
}