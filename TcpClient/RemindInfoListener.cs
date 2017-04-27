using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notify.Code.Net;

namespace TcpClient
{
    class RemindInfoListener
    {
        private static RemindInfoListener m_instance = null;
        private static object m_locker = new object();
        public static RemindInfoListener Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_locker)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new RemindInfoListener();
                        }
                    }
                }
                return m_instance;
            }
        }

        TcpProcessor m_processor = null;
        private RemindInfoListener() { }

        public void Start(System.Net.Sockets.TcpClient connection)
        {
            if (m_processor == null)
            {
                m_processor = new TcpProcessor(connection);
                m_processor.DataReceived += m_processor_DataReceived;
                m_processor.ConnectionDisconnected += m_processor_ConnectionDisconnected;
                m_processor.StartReceive();
                m_processor.StartHeartBeat();
            }
        }
        public void Stop()
        {
            if (m_processor != null)
            {
                m_processor.Dispose();
                m_processor = null;
            }
        }

        void m_processor_DataReceived(object sender,DataReceivedEventArgs e)
        {
            var dataContent = Encoding.GetEncoding("gb2312").GetString(e.Data);
            Console.WriteLine(dataContent);
            
        }
        void m_processor_ConnectionDisconnected(object sender, ConnectionDisconnectedEventArgs e)
        {
            Console.WriteLine("与服务器断开连接");
        }
    }
}
