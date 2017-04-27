using System;
using System.Net.Sockets;

namespace Notify.Code.Net
{
    /// <summary>
    /// 事件异常
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Tcp连接
        /// </summary>
        public TcpClient Client { get; internal set; }

        /// <summary>
        /// 异常
        /// </summary>
        public System.Exception Exception { get; internal set; }
    }
}