using System;
using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.RepositoryFramework;

namespace Notify.Infrastructure.UnitOfWork
{
    /// <summary>
    /// InvokeMethod
    /// </summary>
    /// <param name="entity">
    /// The entity.
    /// </param>
    public delegate void InvokeMethod(IEntity entity);

    public interface IPowerUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 注册新增实体工作单元仓储接口
        /// </summary>
        /// <param name="entity">待新增实体接口</param>
        /// <param name="repository">工作单元仓储接口</param>
        void RegisterAdded(IEntity entity, IUnitOfWorkRepository repository);

        /// <summary>
        /// 注册修改实体工作单元仓储接口
        /// </summary>
        /// <param name="entity">待修改实体接口</param>
        /// <param name="repository">工作单元仓储接口</param>
        void RegisterChanged(IEntity entity, IUnitOfWorkRepository repository);

        /// <summary>
        /// 注册删除实体工作单元仓储接口
        /// </summary>
        /// <param name="entity">待删除实体接口</param>
        /// <param name="repository">工作单元仓储接口</param>
        void RegisterRemoved(IEntity entity, IUnitOfWorkRepository repository);

        /// <summary>
        /// 注册一个其他非基础的增删改工作单元仓储接口
        /// </summary>
        /// <param name="entity">待操作实体接口</param>
        /// <param name="methodName">自定义委托</param>
        void RegisterInvokeMethod(IEntity entity, InvokeMethod methodName);

        /// <summary>
        /// 注册一个非继承聚合根的其他非基础的增删改工作单元仓储接口 
        /// </summary>
        /// <param name="entity">待操作实体接口</param>
        /// <param name="methodName">Action委托</param>
        void RegisterAction(object entity, Action<object> methodName);

        /// <summary>
        /// 注册一个非继承聚合根的其他非基础的增删改工作单元仓储接口 
        /// </summary>
        /// <param name="entity">待操作实体接口</param>
        /// <param name="methodName">Func委托</param>
        void RegisterFunc(object entity, Func<object, object> methodName);
    }
}