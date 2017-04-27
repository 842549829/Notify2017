using System;
using System.Data;

namespace Notify.Infrastructure.UnitOfWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// SQL执行
        /// </summary>
        IDbCommand Command { get; }

        /// <summary>
        /// 提交
        /// </summary>
        void Complete();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();
    }
}