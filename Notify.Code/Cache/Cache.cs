using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Notify.Code.Cache
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class Cache
    {
        /// <summary>
        /// 数据锁，保证线程同步
        /// </summary>
        internal readonly ReaderWriterLock Locker;

        /// <summary>
        /// 锁超时时间
        /// </summary>
        private const int LockApplyTimeOut = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache"/> class.
        /// 实例化读写线程嗦
        /// </summary>
        protected Cache()
        {
            this.Locker = new ReaderWriterLock();
        }

        /// <summary>
        /// 超时时间(缓存时间)
        /// </summary>
        protected double? Timeout { get; set; }

        /// <summary>
        /// 检查超时时间
        /// </summary>
        /// <param name="lastUpdateTime">lastUpdateTime</param>
        /// <returns>rel</returns>
        protected bool IsExpired(DateTime lastUpdateTime)
        {
            return this.Timeout.HasValue && (DateTime.Now - lastUpdateTime).TotalSeconds > this.Timeout.Value;
        }

        /// <summary>
        /// 值获取写线程锁
        /// </summary>
        protected void AcquireWriterLock()
        {
            this.Locker.AcquireWriterLock(LockApplyTimeOut);
        }

        /// <summary>
        /// 减少写线程锁
        /// </summary>
        protected void ReleaseWriterLock()
        {
            this.Locker.ReleaseWriterLock();
        }

        /// <summary>
        /// 获取读线程锁
        /// </summary>
        protected void AcquireReaderLock()
        {
            this.Locker.AcquireReaderLock(LockApplyTimeOut);
        }

        /// <summary>
        /// 减少读线程锁
        /// </summary>
        protected void ReleaseReaderLock()
        {
            this.Locker.ReleaseReaderLock();
        }
    }

    /// <summary>
    /// 简单缓存(不需要维护缓存数据)
    /// </summary>
    /// <typeparam name="TValue">TValue</typeparam>
    public class Cache<TValue> : Cache
    {
        /// <summary>
        /// 缓存项
        /// </summary>
        private CacheItem<TValue> mvalue;

        /// <summary>
        /// 缓存Value
        /// </summary>
        public TValue Value
        {
            get
            {
                TValue result = default(TValue);
                this.AcquireWriterLock();
                if (mvalue != null && !IsExpired(mvalue.Time))
                {
                    result = mvalue.Value;
                }

                this.ReleaseWriterLock();
                return result;
            }

            set
            {
                this.AcquireReaderLock();
                if (mvalue == null)
                {
                    mvalue = new CacheItem<TValue>(value);
                }
                else
                {
                    mvalue.Value = value;
                }

                this.ReleaseReaderLock();
            }
        }
    }

    /// <summary>
    /// 基于数据库的缓存(需要维护缓存数据)
    /// </summary>
    /// <typeparam name="TKey">TKey</typeparam>
    /// <typeparam name="TValue">TValue</typeparam>
    public class Cache<TKey, TValue> : Cache
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        private readonly Dictionary<TKey, CacheItem<TValue>> dataList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache{TKey,TValue}"/> class.
        /// 初始缓存字典
        /// </summary>
        public Cache()
        {
            this.dataList = new Dictionary<TKey, CacheItem<TValue>>();
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Add(TKey key, TValue value)
        {
            this.AcquireWriterLock();
            AddItem(key, value);
            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 添加缓存(批量添加)
        /// </summary>
        /// <param name="range">range</param>
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            if (range == null)
            {
                return;
            }

            this.AcquireWriterLock();
            AddRanges(range);
            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 刷新缓存(全量刷新)
        /// </summary>
        /// <param name="range">range</param>
        public void Refresh(IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            this.AcquireWriterLock();
            this.dataList.Clear();
            if (range != null)
            {
                AddRanges(range);
            }

            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 缓存值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>TValue</returns>
        public TValue this[TKey key]
        {
            get
            {
                return GetValue(key);
            }
        }

        /// <summary>
        /// 取出缓存
        /// </summary>
        /// <param name="key">TValue</param>
        /// <returns>TValue</returns>
        public TValue GetValue(TKey key)
        {
            TValue result = default(TValue);
            this.AcquireReaderLock();
            if (dataList.ContainsKey(key))
            {
                CacheItem<TValue> data = dataList[key];
                if (!this.IsExpired(data.Time))
                {
                    result = data.Value;
                }
            }

            this.ReleaseReaderLock();
            return result;
        }

        /// <summary>
        /// 缓存集合
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                this.AcquireReaderLock();
                Dictionary<TKey, CacheItem<TValue>>.ValueCollection data = dataList.Values;
                this.ReleaseReaderLock();
                return from item in data
                       where !this.IsExpired(item.Time)
                       select item.Value;
            }
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="updateFunc">updateFunc</param>
        public void Update(TKey key, Func<TValue, bool> updateFunc)
        {
            if (updateFunc != null)
            {
                this.AcquireWriterLock();
                if (this.dataList.ContainsKey(key))
                {
                    updateFunc(dataList[key].Value);
                }

                this.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="insertFunc">insertFunc</param>
        /// <param name="updateFunc">updateFunc</param>
        public void Update(TKey key, Func<TValue> insertFunc, Func<TValue, bool> updateFunc)
        {
            this.AcquireWriterLock();
            if (dataList.ContainsKey(key))
            {
                if (updateFunc != null)
                {
                    updateFunc(dataList[key].Value);
                }
            }
            else
            {
                if (insertFunc != null)
                {
                    dataList.Add(key, new CacheItem<TValue>(insertFunc()));
                }
            }

            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 保存(缓存数据存在就修改不存在就添加)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Update(TKey key, TValue value)
        {
            this.AcquireWriterLock();
            if (this.dataList.ContainsKey(key))
            {
                this.dataList[key].Value = value;
            }
            else
            {
                this.dataList.Add(key, new CacheItem<TValue>(value));
            }

            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">key</param>
        public void Remove(TKey key)
        {
            this.AcquireWriterLock();
            if (dataList.ContainsKey(key))
            {
                this.dataList.Remove(key);
            }

            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void Clear()
        {
            this.AcquireWriterLock();
            this.dataList.Clear();
            this.ReleaseWriterLock();
        }

        /// <summary>
        /// 添加(批量添加)
        /// </summary>
        /// <param name="range">range</param>
        private void AddRanges(IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            foreach (var item in range)
            {
                AddItem(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        private void AddItem(TKey key, TValue value)
        {
            if (dataList.ContainsKey(key))
            {
                dataList[key].Value = value;
            }
            else
            {
                dataList.Add(key, new CacheItem<TValue>(value));
            }
        }
    }

    /// <summary>
    /// 缓存项
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    internal class CacheItem<T>
    {
        /// <summary>
        /// T
        /// </summary>
        private T mvalue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItem{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public CacheItem(T value)
        {
            this.Time = DateTime.Now;
            this.mvalue = value;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// T
        /// </summary>
        public T Value
        {
            get
            {
                return this.mvalue;
            }

            set
            {
                this.mvalue = value;
                this.Time = DateTime.Now;
            }
        }
    }
}
