using System;
using System.Net.Sockets;

namespace Notify.Code.Net
{
    /// <summary>
    /// 连接断开事件
    /// </summary>
    public class ConnectionDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Tcp连接
        /// </summary>
        public TcpClient Client
        {
            get;
            internal set;
        }
    }
}