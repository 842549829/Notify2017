using System;
using System.Net.Sockets;

namespace Notify.Code.Net
{
    /// <summary>
    /// 数据接收事件
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Tcp连接
        /// </summary>
        public TcpClient Client
        {
            get;
            internal set;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data
        {
            get;
            internal set;
        }
    }
}