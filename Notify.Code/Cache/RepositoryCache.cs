using System.Collections.Generic;
using System.Timers;

namespace Notify.Code.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    /// <typeparam name="TKey">
    /// TKey
    /// </typeparam>
    /// <typeparam name="TValue">
    /// TValue
    /// </typeparam>
    public class RepositoryCache<TKey, TValue>
    {
        /// <summary>
        /// 刷新时间
        /// 单位：毫秒
        /// 可用配置代替
        /// </summary>
        private const double DefaultInterval = 2 * 60 * 1000;

        /// <summary>
        /// 仓储接口
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// 缓存
        /// </summary>
        private readonly Cache<TKey, TValue> cache;

        /// <summary>
        /// 刷新Timer控件
        /// 也可用其他方式实现
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCache{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public RepositoryCache(IRepository repository)
            : this(repository, new Cache<TKey, TValue>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCache{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public RepositoryCache(IRepository repository, Cache<TKey, TValue> cache)
            : this(repository, cache, DefaultInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCache{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="refreshInterval">
        /// The refresh interval.
        /// </param>
        public RepositoryCache(IRepository repository, double refreshInterval)
            : this(repository, new Cache<TKey, TValue>(), new Timer(refreshInterval))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCache{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        /// <param name="refreshInterval">
        /// The refresh interval.
        /// </param>
        public RepositoryCache(IRepository repository, Cache<TKey, TValue> cache, double refreshInterval)
            : this(repository, new Cache<TKey, TValue>(), new Timer(refreshInterval))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCache{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        /// <param name="timer">
        /// The timer.
        /// </param>
        private RepositoryCache(IRepository repository, Cache<TKey, TValue> cache, Timer timer)
        {
            this.repository = repository;
            this.cache = cache;
            this.timer = timer;
            this.timer.Elapsed += this.TimerElapsed;
            this.Refresh();
            this.timer.Start();
        }

        #region 读取缓存数据
        /// <summary>
        /// 缓存值
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get { return this.cache.Values; }
        }

        /// <summary>
        /// 获取缓存的索引器
        /// </summary>
        /// <param name="key">
        /// TKey
        /// </param>
        /// <returns>
        /// TValue
        /// </returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.cache[key];
            }
        } 
        #endregion

        #region 刷新缓存数据
        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            IEnumerable<KeyValuePair<TKey, TValue>> data = this.QueryFromRepository();
            this.cache.Refresh(data);
        } 
        #endregion

        #region 维护缓存数据
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">
        /// key
        /// </param>
        /// <param name="value">
        /// value
        /// </param>
        /// <returns>添加结果</returns>
        public object Add(TKey key, TValue value)
        {
            object result = this.AddModelFromRepository(value);
            this.cache.Add(key, value);
            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>添加结果</returns>
        public object Add(TValue value)
        {
            object key = this.AddModelFromRepository(value);
            this.cache.Add((TKey)key, value);
            return key;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key">
        /// key
        /// </param>
        /// <param name="value">
        /// value
        /// </param>
        public void Update(TKey key, TValue value)
        {
            this.UpdateModelFromRepository(value);
            this.cache.Update(key, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">
        /// key
        /// </param>
        /// <returns>
        /// TValue
        /// </returns>
        public TValue Remove(TKey key)
        {
            TValue value = this[key];
            if (value != null)
            {
                this.DeleteModelFromRepository(value);
                this.cache.Remove(key);
            }

            return value;
        } 
        #endregion

        #region 持久化的方法
        /// <summary>
        /// QueryFromRepository
        /// </summary>
        /// <returns>IEnumerable</returns>
        protected virtual IEnumerable<KeyValuePair<TKey, TValue>> QueryFromRepository()
        {
            return null != this.repository ? this.repository.Query() : null;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>添加结果</returns>
        protected virtual object AddModelFromRepository(TValue value)
        {
            return null != this.repository ? this.repository.Insert(value) : null;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="value">
        /// value
        /// </param>
        protected virtual void UpdateModelFromRepository(TValue value)
        {
            if (null != this.repository)
            {
                this.repository.Modify(value);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="value">value</param>
        protected virtual void DeleteModelFromRepository(TValue value)
        {
            if (null != this.repository)
            {
                this.repository.Delete(value);
            }
        } 
        #endregion

        #region 自动刷新缓存数据事件
        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender">
        /// sender
        /// </param>
        /// <param name="e">
        /// e
        /// </param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Refresh();
        } 
        #endregion

        #region  仓储接口(持久化到数据库)
        /// <summary>
        /// 仓储接口(持久化到数据库)
        /// </summary>
        public interface IRepository
        {
            /// <summary>
            /// 查询数据
            /// </summary>
            /// <returns>数据集</returns>
            IEnumerable<KeyValuePair<TKey, TValue>> Query();

            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">value</param>
            /// <returns>返回添加结果</returns>
            object Insert(TValue value);

            /// <summary>
            /// 修改数据
            /// </summary>
            /// <param name="value">value</param>
            /// <returns>返回修改结果</returns>
            void Modify(TValue value);

            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="value">value</param>
            void Delete(TValue value);
        } 
        #endregion
    }
}