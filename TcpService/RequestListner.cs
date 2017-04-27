using System;
using System.Configuration;
using System.Text;
using Notify.Code.Net;
using TcpService.Command;

namespace TcpService
{
    /// <summary>
    /// 请求数据集合
    /// </summary>
    internal class RequestListner : IDisposable
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static RequestListner m_instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object m_locker = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static RequestListner Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_locker)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new RequestListner();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Tcp服务端口侦听
        /// </summary>
        private TcpServer m_server;

        /// <summary>
        /// 编码
        /// </summary>
        private static readonly Encoding m_encoding = Encoding.GetEncoding("gb2312");

        /// <summary>
        /// 端口配置文件
        /// </summary>
        private const string m_listenPortSetting = "listenPort";

        /// <summary>
        /// 开始侦听
        /// </summary>
        /// <returns>结果</returns>
        public bool Start()
        {
            if (m_server == null)
            {
                var portString = ConfigurationManager.AppSettings[m_listenPortSetting];
                if (string.IsNullOrWhiteSpace(portString))
                {
                    Console.WriteLine("缺少侦听端口:" + m_listenPortSetting);
                    return false;
                }
                else
                {
                    int port;
                    if (int.TryParse(portString, out port))
                    {
                        this.m_server = new TcpServer(port);
                        this.m_server.Connected += M_server_Connected;
                        this.m_server.Exception += M_server_Exception;
                        this.m_server.Start();
                        Console.WriteLine("成功开启侦听端口 " + portString);
                    }
                    else
                    {
                        Console.WriteLine("侦听端口格式 " + portString);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 服务异常事件处理
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private static void M_server_Exception(object sender, ExceptionEventArgs e)
        {
            Program.ApplicationError(e.Exception?.Message ?? "未知错误");
        }

        /// <summary>
        /// 服务成功事件处理
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private static void M_server_Connected(object sender, ConnectedEventArgs e)
        {
            var processor = new TcpProcessor(e.Client);
            processor.StartReceive();
            processor.DataReceived += Processor_DataReceived; 
        }

        /// <summary>
        /// 接收数据事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private static void Processor_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var request = m_encoding.GetString(e.Data);
            var cmdProcessor = CommandProcessor.GetCommandProcessor(request, e.Client);
            if (cmdProcessor != null)
            {
                var response = cmdProcessor.Execute();
                var send = sender as TcpProcessor;
                send?.Send(m_encoding.GetBytes(response));
                if (cmdProcessor.DisposeConnection)
                {
                    return;
                }
            }
           //CustomGCCollection.Instance.Register(sender as TcpProcessor);
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (this.m_server != null)
            {
                this.m_server.Dispose();
                this.m_server = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}