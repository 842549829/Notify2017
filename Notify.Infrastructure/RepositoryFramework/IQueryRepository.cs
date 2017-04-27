using System;

namespace Notify.Infrastructure.RepositoryFramework
{
    /// <summary>
    /// IQueryRepository{根据主键查询接口}.
    /// </summary>
    /// <typeparam name="TValue">实体</typeparam>
    public interface IQueryRepository<out TValue> : IDisposable
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回查询单条数据</returns>
        TValue Query(object id);
    }

    /// <summary>
    /// IQueryRepository{根据主键查询接口}.
    /// </summary>
    /// <typeparam name="TKey">Tkey</typeparam>
    /// <typeparam name="TValue">实体</typeparam>
    public interface IQueryRepository<in TKey,out TValue> : IDisposable
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        TValue Query(TKey key);
    }
}