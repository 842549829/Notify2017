using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Notify.Code.Net
{
    /// <summary>
    /// Tcp服务
    /// </summary>
    public class TcpServer : IDisposable
    {
        /// <summary>
        /// IPEndPoint
        /// </summary>
        private readonly IPEndPoint m_localEP;

        /// <summary>
        /// 是否停止
        /// </summary>
        private volatile bool m_shouldStop = true;

        /// <summary>
        /// TcpListener
        /// </summary>
        private TcpListener m_listener;

        /// <summary>
        /// 连接完成事件
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// 连接异常事件
        /// </summary>
        public event EventHandler<ExceptionEventArgs> Exception;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="port">端口</param>
        public TcpServer(int port) : this(IPAddress.Any, port)
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        public TcpServer(IPAddress address, int port) : this(new IPEndPoint(address, port))
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="localEP">地址</param>
        public TcpServer(IPEndPoint localEP)
        {
            if (localEP == null)
            {
                throw new ArgumentNullException(nameof(localEP));
            }
            this.m_localEP = localEP;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (this.m_listener == null)
            {
                ThreadPool.QueueUserWorkItem(this.RunAcceptTcpClient, this.m_localEP);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this.m_shouldStop = true;
            if (this.m_listener != null)
            {
                this.m_listener.Stop();
                this.m_listener = null;
            }
        }

        /// <summary>
        /// 运行接受Tcp
        /// </summary>
        /// <param name="state">状态参数</param>
        private void RunAcceptTcpClient(object state)
        {
            IPEndPoint localEp = (IPEndPoint)state;
            this.m_shouldStop = false;
            this.m_listener = new TcpListener(localEp);
            this.m_listener.Start();
            while (!this.m_shouldStop)
            {
                TcpClient client = this.m_listener.AcceptTcpClient();
                try
                {
                    this.OnConnect(client);
                }
                catch (System.Exception exception)
                {
                    this.OnException(client, exception);
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// 连接成功事件
        /// </summary>
        /// <param name="client">tcp</param>
        private void OnConnect(TcpClient client)
        {
            if (this.Connected != null)
            {
                ConnectedEventArgs e = new ConnectedEventArgs
                {
                    Client = client
                };
                this.Connected(this, e);
            }
        }

        /// <summary>
        /// 连接异常事件
        /// </summary>
        /// <param name="client">tcp</param>
        /// <param name="exception">异常</param>
        private void OnException(TcpClient client, System.Exception exception)
        {
            if (this.Exception != null)
            {
                ExceptionEventArgs e = new ExceptionEventArgs
                {
                    Client = client,
                    Exception = exception
                };
                this.Exception(this, e);
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Stop();
            }
        }
    }
}