using Notify.Infrastructure.DomainBase;

namespace Notify.Infrastructure.RepositoryFramework
{
    /// <summary>
    /// 仓储接口(CRUD)
    /// </summary>
    /// <typeparam name="TValue">实体</typeparam>
    public interface IRepository<TValue> : IAddRepository<TValue>, IRemoveRepository<TValue>, IUpdateRepository<TValue>, IQueryRepository<TValue> where TValue : IEntity
    {
    }

    /// <summary>
    /// 仓储接口(CRUD)
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    /// <typeparam name="TValue">实体</typeparam>
    public interface IRepository<in TKey, TValue> : IAddRepository<TValue>, IRemoveRepository<TValue>, IUpdateRepository<TValue>, IQueryRepository<TKey, TValue> where TValue : IEntity
    {
    }
}