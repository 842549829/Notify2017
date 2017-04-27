using System;
using System.Collections.Generic;
using System.Net.Sockets;
using TcpService.DataSource;
using TcpService.Model;

namespace TcpService.Command
{
    /// <summary>
    /// 登录中心
    /// </summary>
    internal class LogonCenter : IDisposable
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static LogonCenter m_instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object m_locker = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static LogonCenter Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_locker)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new LogonCenter();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// 以登录Id作为键
        /// </summary>
        private readonly Dictionary<string, User> m_users;

        /// <summary>
        /// 构造
        /// </summary>
        private LogonCenter()
        {
            m_users = new Dictionary<string, User>();
        }

        /// <summary>
        /// 用户
        /// </summary>
        public IEnumerable<User> Users => m_users.Values;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="connection">连接</param>
        /// <param name="logonUser">用户</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns>结果</returns>
        public bool Logon(string userName, string password, TcpClient connection, out User logonUser, out string errorCode)
        {
            logonUser = new User("001", connection)
            {
                Name = userName,
                Id = "001",
                Time = DateTime.Now
            };
            m_users.Add(logonUser.Id, logonUser);
            errorCode = string.Empty;
            DataProcessor.Instance.Start();
            return true;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>用户</returns>
        public User GetUser(string id)
        {
            User result;
            m_users.TryGetValue(id, out result);
            return result;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="id">Id号</param>
        /// <returns>退出用户</returns>
        public User Logoff(string id)
        {
            User result;
            m_users.TryGetValue(id, out result);
            if (result != null)
            {
                m_users.Remove(id);
                DataProcessor.Instance.Stop();
            }
            return result;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
        }
    }
}