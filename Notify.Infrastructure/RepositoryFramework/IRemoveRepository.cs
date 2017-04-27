using System;

namespace Notify.Infrastructure.RepositoryFramework
{
    /// <summary>
    /// IRepository{删除接口}
    /// </summary>
    /// <typeparam name="T">TKey</typeparam>
    public interface IRemoveRepository<in T> : IDisposable
    {
        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        void Remove(T entity);
    }
}