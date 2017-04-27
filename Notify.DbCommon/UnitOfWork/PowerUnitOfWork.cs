using System;
using System.Collections.Generic;
using Notify.Code.Write;
using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Infrastructure.UnitOfWork;

namespace Notify.DbCommon.UnitOfWork
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class PowerUnitOfWork : UnitOfWork, IPowerUnitOfWork
    {
        /// <summary>
        /// 新增实体工作单元
        /// </summary>
        private readonly Dictionary<IEntity, IUnitOfWorkRepository> m_addedEntities;

        /// <summary>
        /// 修改实体工作单元
        /// </summary>
        private readonly Dictionary<IEntity, IUnitOfWorkRepository> m_changedEntities;

        /// <summary>
        /// 删除实体工作单元
        /// </summary>
        private readonly Dictionary<IEntity, IUnitOfWorkRepository> m_deletedEntities;

        /// <summary>
        /// 其他非基础的增删改
        /// </summary>
        private readonly Dictionary<IEntity, InvokeMethod> m_invokeEntities;

        /// <summary>
        /// 非继承聚合根的其他非基础的增删改工作单元
        /// </summary>
        private readonly Dictionary<object, Action<object>> m_action;

        /// <summary>
        /// 非继承聚合根的其他非基础的增删改工作单元
        /// </summary>
        private readonly Dictionary<object, Func<object, object>> m_func;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerUnitOfWork"/> class.
        /// </summary>
        /// <param name="connectionSetting">
        /// The connection setting.
        /// </param>
        public PowerUnitOfWork(string connectionSetting)
            : base(connectionSetting)
        {
            this.m_addedEntities = new Dictionary<IEntity, IUnitOfWorkRepository>();
            this.m_changedEntities = new Dictionary<IEntity, IUnitOfWorkRepository>();
            this.m_deletedEntities = new Dictionary<IEntity, IUnitOfWorkRepository>();
            this.m_invokeEntities = new Dictionary<IEntity, InvokeMethod>();
            this.m_action = new Dictionary<object, Action<object>>();
            this.m_func = new Dictionary<object, Func<object, object>>();
        }

        /// <summary>
        /// 提交工作单元
        /// </summary>
        public override void Complete()
        {
            try
            {
                foreach (IEntity entity in this.m_deletedEntities.Keys)
                {
                    this.m_deletedEntities[entity].PersistDeletedItem(entity);
                }

                foreach (IEntity entity in this.m_addedEntities.Keys)
                {
                    this.m_addedEntities[entity].PersistNewItem(entity);
                }

                foreach (IEntity entity in this.m_changedEntities.Keys)
                {
                    this.m_changedEntities[entity].PersistUpdatedItem(entity);
                }

                foreach (IEntity entity in this.m_invokeEntities.Keys)
                {
                    this.m_invokeEntities[entity](entity);
                }

                foreach (var entity in this.m_action)
                {
                    entity.Value(entity.Key);
                }

                foreach (var entity in this.m_func)
                {
                    entity.Value(entity.Key);
                }

                base.Complete();
            }
            catch (Exception exception)
            {
                this.Rollback();
                LogService.WriteLog(exception, "工作单元提交失败");
                throw;
            }
            finally
            {
                this.Dispose();
                this.Clear();
            }
        }

        /// <summary>
        /// 注册新增实体工作单元仓储接口
        /// </summary>
        /// <param name="entity">待新增实体接口</param>
        /// <param name="repository">工作单元仓储接口</param>
        public void RegisterAdded(IEntity entity, IUnitOfWorkRepository repository)
        {
            this.m_addedEntities.Add(entity, repository);
        }

        /// <summary>
        /// 注册修改实体工作单元仓储接口
        /// </summary>
        /// <param name="entity">待修改实体接口</param>
        /// <param name="repository">工作单元仓储接口</param>
        public void RegisterChanged(IEntity entity, IUnitOfWorkRepository repository)
        {
            this.m_changedEntities.Add(entity, repository);
        }

        /// <summary>
        /// 注册删除实体工作单元仓储接口
        /// </summary>
        /// <param name="entity">待删除实体接口</param>
        /// <param name="repository">工作单元仓储接口</param>
        public void RegisterRemoved(IEntity entity, IUnitOfWorkRepository repository)
        {
            this.m_deletedEntities.Add(entity, repository);
        }

        /// <summary>
        /// 注册一个其他非基础的增删改工作单元仓储接口
        /// </summary>
        /// <param name="entity">待操作实体接口</param>
        /// <param name="methodName">自定义委托</param>
        public void RegisterInvokeMethod(IEntity entity, InvokeMethod methodName)
        {
            this.m_invokeEntities.Add(entity, methodName);
        }

        /// <summary>
        /// 注册一个非继承聚合根的其他非基础的增删改工作单元仓储接口 
        /// </summary>
        /// <param name="entity">待操作实体接口</param>
        /// <param name="methodName">Action委托</param>
        public void RegisterAction(object entity, Action<object> methodName)
        {
            this.m_action.Add(entity, methodName);
        }

        /// <summary>
        /// 注册一个非继承聚合根的其他非基础的增删改工作单元仓储接口 
        /// </summary>
        /// <param name="entity">待操作实体接口</param>
        /// <param name="methodName">Func委托</param>
        public void RegisterFunc(object entity, Func<object, object> methodName)
        {
            this.m_func.Add(entity, methodName);
        }

        /// <summary>
        /// 清除
        /// </summary>
        private void Clear()
        {
            this.m_addedEntities.Clear();
            this.m_changedEntities.Clear();
            this.m_deletedEntities.Clear();
            this.m_invokeEntities.Clear();
            this.m_action.Clear();
            this.m_func.Clear();
        }
    }
}