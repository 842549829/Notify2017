using System;
using System.Threading;
using TcpService.Command;

namespace TcpService.DataSource
{
    /// <summary>
    /// 数据处理
    /// </summary>
    internal class DataProcessor
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static DataProcessor m_instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object m_locker = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static DataProcessor Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_locker)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new DataProcessor();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// 是否停止
        /// </summary>
        private volatile bool m_stop = true;

        /// <summary>
        ///  线程
        /// </summary>
        private Thread m_thread = null;

        public void Start()
        {
            if (m_stop)
            {
                m_stop = false;
                m_thread = new Thread(Run)
                {
                    IsBackground = true
                };
                m_thread.Start();
                DataCenter.Instance.Start();
                Console.WriteLine("开始数据提醒处理...");
            }
        }
        public void Stop()
        {
            m_stop = true;
            m_thread = null;
            DataCenter.Instance.Stop();
        }

        /// <summary>
        /// 运行
        /// </summary>
        private void Run()
        {
            while (!m_stop)
            {
                var remindDatas = DataCenter.Instance.RemindDatas;
                if (remindDatas != null)
                {
                    foreach (var item in remindDatas)
                    {
                        var user = LogonCenter.Instance.GetUser(item.Key);
                        if (user != null)
                        {
                            user.Remind(item);
                            Thread.Sleep(10);
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}