using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Notify.Code.Net;
using TcpService.Command;

namespace TcpService.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// Tcp处理
        /// </summary>
        private TcpProcessor m_processor = null;

        /// <summary>
        /// 地址
        /// </summary>
        private IPAddress m_address = null;

        /// <summary>
        /// 处理时间
        /// </summary>
        private DateTime? m_previousRemindTime = null;

        /// <summary>
        /// 提醒信息
        /// </summary>
        private List<RemindInfo> m_previousRemindInfos = null;

        // 是否需要提醒，用于处理刚登录的用户，还没有得到登录的返回信息，就收到提醒数据了
        private bool _requireRemind = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="connection">Tcp</param>
        public User(string id, TcpClient connection)
        {
            Id = id;
            Time = DateTime.Now;
            m_previousRemindInfos = new List<RemindInfo>();
            InitTcpProcessor(connection);
        }

        /// <summary>
        /// 初始化Tcp
        /// </summary>
        /// <param name="connection">TCP</param>
        private void InitTcpProcessor(TcpClient connection)
        {
            m_address = ((IPEndPoint)connection.Client.RemoteEndPoint).Address;
            m_processor = new TcpProcessor(connection);
            m_processor.ConnectionDisconnected += M_processor_ConnectionDisconnected; ;
        }

        /// <summary>
        /// 连接断开处理事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void M_processor_ConnectionDisconnected(object sender, ConnectionDisconnectedEventArgs e)
        {
            Console.WriteLine("{0} 用户[{1}]已断开连接.{2}  批次号:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Name, Environment.NewLine, Id);
            LogonCenter.Instance.Logoff(Id);
        }

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 提醒
        /// </summary>
        /// <param name="remindInfos">提醒项</param>
        internal void Remind(IEnumerable<RemindInfo> remindInfos)
        {
            if (!_requireRemind)
            {
                _requireRemind = (DateTime.Now - Time).TotalSeconds > 10;
                if (!_requireRemind)
                {
                    return;
                }
            }
            m_previousRemindInfos.Clear();
            m_previousRemindInfos.AddRange(remindInfos);
            m_previousRemindTime = DateTime.Now;
            var processor = new Remind(m_processor, m_previousRemindInfos);
            processor.Execute();
        }
    }
}