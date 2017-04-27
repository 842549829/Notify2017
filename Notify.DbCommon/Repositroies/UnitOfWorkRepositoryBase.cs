using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Infrastructure.UnitOfWork;

namespace Notify.DbCommon.Repositroies
{
    /// <summary>
    /// 工作单元仓储基类
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    /// <typeparam name="TValue">实体</typeparam>
    public abstract class UnitOfWorkRepositoryBase<TKey, TValue> : BaseRepository, IRepository<TKey, TValue>, IUnitOfWorkRepository
        where TValue : IEntity
    {
        /// <summary>
        /// 工作单元接口
        /// </summary>
        public new IPowerUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkRepositoryBase{TKey,TValue}"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据连接</param>
        protected UnitOfWorkRepositoryBase(IPowerUnitOfWork unitOfWork, string name)
            : base(unitOfWork, name)
        {
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// 根据主键查询实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>实体</returns>
        public abstract TValue Query(TKey key);

        /// <summary>
        /// 根据主键查询实体(索引)
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>实体</returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.Query(key);
            }

            set
            {
                if (this.Query(key) == null)
                {
                    this.Add(value);
                }
                else
                {
                    this.Update(value);
                }
            }
        }

        /// <summary>
        /// 添加实体(注册到工作单元)
        /// </summary>
        /// <param name="item">实体</param>
        public void Add(TValue item)
        {
            if (this.UnitOfWork != null)
            {
                this.UnitOfWork.RegisterAdded(item, this);
            }
            else
            {
                this.PersistNewItem(item);
            }
        }

        /// <summary>
        /// 删除实体(注册到工作单元)
        /// </summary>
        /// <param name="item">实体</param>
        public void Remove(TValue item)
        {
            if (this.UnitOfWork != null)
            {
                this.UnitOfWork.RegisterRemoved(item, this);
            }
            else
            {
                this.PersistDeletedItem(item);
            }
        }

        /// <summary>
        /// 修改实体(注册到工作单元)
        /// </summary>
        /// <param name="item">实体</param>
        public void Update(TValue item)
        {
            if (this.UnitOfWork != null)
            {
                this.UnitOfWork.RegisterChanged(item, this);
            }
            else
            {
                this.PersistUpdatedItem(item);
            }
        }

        /// <summary>
        /// 添加实体(持久化)
        /// </summary>
        /// <param name="item">实体</param>
        public abstract void PersistNewItem(IEntity item);

        /// <summary>
        /// 修改实体(持久化)
        /// </summary>
        /// <param name="item">实体</param>
        public abstract void PersistUpdatedItem(IEntity item);

        /// <summary>
        /// 删除实体(持久化)
        /// </summary>
        /// <param name="item">实体</param>
        public abstract void PersistDeletedItem(IEntity item);
    }
}