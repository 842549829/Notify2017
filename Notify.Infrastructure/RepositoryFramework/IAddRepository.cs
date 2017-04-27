using System;

namespace Notify.Infrastructure.RepositoryFramework
{
    /// <summary>
    /// IAddRepository{Add单条数据的接口}
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public interface IAddRepository<in T> : IDisposable
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        void Add(T entity);
    }
}