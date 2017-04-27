using System;
using System.Collections.Generic;
using System.Timers;
using Notify.Code.Net;

namespace TcpService
{
    /// <summary>
    /// 自定义GC回收
    /// </summary>
    internal class CustomGCCollection : IDisposable
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static CustomGCCollection m_instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object m_locker = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static CustomGCCollection Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_locker)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new CustomGCCollection();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// 主资源
        /// </summary>
        private volatile bool m_recycleMainQueue;

        /// <summary>
        /// 主队列
        /// </summary>
        private readonly Queue<TcpProcessor> m_mainQueue;

        /// <summary>
        /// 附队列
        /// </summary>
        private readonly Queue<TcpProcessor> m_assitantQueue;

        /// <summary>
        /// 时间间隔
        /// </summary>
        private Timer m_recycleTimer;

        /// <summary>
        /// 构造
        /// </summary>
        private CustomGCCollection()
        {
            m_mainQueue = new Queue<TcpProcessor>();
            m_assitantQueue = new Queue<TcpProcessor>();
        }

        /// <summary>
        /// 开始处理
        /// </summary>
        private void Start()
        {
            if (m_recycleTimer == null)
            {
                m_recycleTimer = new Timer(5000);
                m_recycleTimer.Elapsed += delegate
                {
                    m_recycleMainQueue = !m_recycleMainQueue;
                    RecycleQueue(m_recycleMainQueue ? m_mainQueue : m_assitantQueue);
                };
                m_recycleTimer.Start();
            }
        }

        /// <summary>
        /// 注册资源
        /// </summary>
        /// <param name="item">资源</param>
        public void Register(TcpProcessor item)
        {
            Start();
            if (item != null)
            {
                var queue = m_recycleMainQueue ? m_assitantQueue : m_mainQueue;
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (m_recycleTimer != null)
            {
                m_recycleTimer.Stop();
                m_recycleTimer.Dispose();
                m_recycleTimer = null;
            }
            RecycleQueue(m_mainQueue);
            RecycleQueue(m_assitantQueue);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 资源清除
        /// </summary>
        /// <param name="queue">队列</param>
        internal void RecycleQueue(Queue<TcpProcessor> queue)
        {
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                item.Dispose();
            }
        }
    }
}