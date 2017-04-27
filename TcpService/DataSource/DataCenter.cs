using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Notify.Code.Write;
using TcpService.Model;

namespace TcpService.DataSource
{
    /// <summary>
    /// 数据中心
    /// </summary>
    internal class DataCenter
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static DataCenter m_instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object m_locker = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static DataCenter Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_locker)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new DataCenter();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// 线程
        /// </summary>
        private Thread m_thread;

        /// <summary>
        /// 是否停止
        /// </summary>
        private volatile bool m_stop = true;

        /// <summary>
        /// 提醒数据
        /// </summary>
        public IEnumerable<IGrouping<string, RemindInfo>> RemindDatas
        {
            get;
            private set;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (m_stop)
            {
                m_thread = new Thread(Run)
                {
                    IsBackground = true
                };
                m_thread.Start();
            }
        }

        /// <summary>
        /// 运行
        /// </summary>
        internal void Run()
        {
            while (m_stop)
            {
                QueryData();
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        internal void QueryData()
        {
            try
            {
                var remindInfos = RemindInfos;
                this.RemindDatas = remindInfos.GroupBy(item => item.Id);
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "获取提醒数据出错");
                Console.WriteLine("获取提醒数据出错");
                Console.WriteLine("错误原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            m_stop = true;
            m_thread = null;
        }

        public void RemoveData(string id)
        {
            for (int i = 0; i < RemindInfos.Count(); i++)
            {
                var item = RemindInfos[i];
                if (item.Id == id)
                {
                    RemindInfos.Remove(item);
                }
            }

            var remindInfos = RemindInfos;
            this.RemindDatas = remindInfos.GroupBy(item => item.Id);
        }

        public static List<RemindInfo> RemindInfos = new List<RemindInfo>
                {
                    new RemindInfo
                    {
                        Id = "001",
                        CustomNO = "201558456555",
                        Status = "等待支付"
                    }
                };
    }
}