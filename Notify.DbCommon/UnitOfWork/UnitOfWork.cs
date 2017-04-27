using System.Data;
using Notify.Infrastructure.UnitOfWork;

namespace Notify.DbCommon.UnitOfWork
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// 设置事务隔离级别
        /// </summary>
        /// <param name="name"> The connection String.</param>
        public UnitOfWork(string name)
        {
            this.Conn = DbFactories.GetConnection(name);
            this.Command = this.Conn.CreateCommand();
            this.Transaction = this.Conn.BeginTransaction();
            this.Command.Transaction = this.Transaction;
        }

        /// <summary>
        /// 链接
        /// </summary>
        protected IDbConnection Conn { get; set; }

        /// <summary>
        /// 事务
        /// </summary>
        protected IDbTransaction Transaction { get; set; }

        /// <summary>
        /// SQL执行
        /// </summary>
        /// <summary>
        /// IDbCommand
        /// </summary>
        public IDbCommand Command { get; private set; }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void Complete()
        {
            this.Transaction.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void Rollback()
        {
            this.Transaction.Rollback();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
                this.Transaction = null;
            }

            if (this.Command != null)
            {
                this.Command.Dispose();
                this.Command = null;
            }

            if (this.Conn != null)
            {
                this.Conn.Dispose();
                this.Conn.Close();
                this.Conn = null;
            }
        }
    }
}
