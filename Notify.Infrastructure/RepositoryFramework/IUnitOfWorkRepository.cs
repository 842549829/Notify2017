using Notify.Infrastructure.DomainBase;

namespace Notify.Infrastructure.RepositoryFramework
{
    /// <summary>
    /// 工作单元仓储接口
    /// </summary>
    public interface IUnitOfWorkRepository
    {
        /// <summary>
        /// 持久化新增实体
        /// </summary>
        /// <param name="item">待新增实体接口</param>
        void PersistNewItem(IEntity item);

        /// <summary>
        /// 持久化更新实体
        /// </summary>
        /// <param name="item">待更新实体接口</param>
        void PersistUpdatedItem(IEntity item);

        /// <summary>
        /// 持久化删除实体
        /// </summary>
        /// <param name="item">待删除实体接口</param>
        void PersistDeletedItem(IEntity item);
    }
}
