using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Notify.Code.Net
{
    /// <summary>
    /// Tcp处理
    /// </summary>
    public class TcpProcessor : IDisposable
    {
        /// <summary>
        /// 异步发送数据
        /// </summary>
        private class AsyncSendData
        {
            /// <summary>
            /// 数据
            /// </summary>
            public byte[] Data { get; set; }

            /// <summary>
            /// Offset
            /// </summary>
            public int Offset { get; set; }

            /// <summary>
            /// 大小
            /// </summary>
            public int Size { get; set; }

            /// <summary>
            /// 回调函数
            /// </summary>
            public WaitCallback Callback { get; set; }

            /// <summary>
            /// 参数
            /// </summary>
            public object State { get; set; }
        }

        /// <summary>
        /// 异步接收数据
        /// </summary>
        private class AsyncReceiveData
        {
            /// <summary>
            /// Tcp
            /// </summary>
            public TcpClient Client { get; set; }

            /// <summary>
            /// 回调函数
            /// </summary>
            public ReceiveDataCallback Callback { get; set; }

            /// <summary>
            /// 参数
            /// </summary>
            public object State { get; set; }
        }

        /// <summary>
        /// 最大数据长度
        /// </summary>
        private readonly int m_lengthArrayLength = Utility.ToBytes(2147483647).Length;

        /// <summary>
        /// 心跳连接请求数据
        /// </summary>
        private byte[] m_heartBeatRequestData;

        /// <summary>
        /// 心跳连接频率
        /// </summary>
        private int m_heartBeatInterval;

        /// <summary>
        /// 是否停止心跳连接
        /// </summary>
        private volatile bool m_stopHeartBeat;

        /// <summary>
        /// 是否断开连接
        /// </summary>
        private bool m_reportedDisconnected;

        /// <summary>
        /// TcpClient
        /// </summary>
        private TcpClient m_client;

        /// <summary>
        /// 是否停止数据接收
        /// </summary>
        private volatile bool m_stopReceiveData;

        /// <summary>
        /// 最后接收数据时间
        /// </summary>
        private DateTime m_lastReceiveDataTime;

        /// <summary>
        /// 最后发送数据时间
        /// </summary>
        private DateTime m_lastSendDataTime;

        /// <summary>
        /// 连接断开事件
        /// </summary>
        public event EventHandler<ConnectionDisconnectedEventArgs> ConnectionDisconnected;

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// 发送数据大小
        /// </summary>
        public int SendBufferSize
        {
            get { return this.m_client.SendBufferSize; }
            set { this.m_client.SendBufferSize = value; }
        }

        /// <summary>
        /// 接收数据大小
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
                return this.m_client.ReceiveBufferSize;
            }
            set
            {
                this.m_client.ReceiveBufferSize = value;
            }
        }

        /// <summary>
        /// 发送超时时间
        /// </summary>
        public int SendTimeout
        {
            get
            {
                return this.m_client.SendTimeout;
            }
            set
            {
                this.m_client.SendTimeout = value;
            }
        }

        /// <summary>
        /// 接收超时时间
        /// </summary>
        public int ReceiveTimeout
        {
            get
            {
                return this.m_client.ReceiveTimeout;
            }
            set
            {
                this.m_client.ReceiveTimeout = value;
            }
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        public bool Connected
        {
            get
            {
                return this.m_client.Connected;
            }
        }

        /// <summary>
        /// 是否心跳连接开始
        /// </summary>
        public bool HeartBeatStarted
        {
            get
            {
                return !this.m_stopHeartBeat;
            }
        }

        /// <summary>
        /// 心跳连接数据
        /// </summary>
        public byte[] HeartBeatData
        {
            get
            {
                return this.m_heartBeatRequestData;
            }
            set
            {
                var mHeartBeatRequestData = this.m_heartBeatRequestData;
                if (mHeartBeatRequestData != null)
                {
                    if (mHeartBeatRequestData != value)
                    {
                        this.m_heartBeatRequestData = value;
                    }
                }
                else
                {
                    this.m_heartBeatRequestData = value;
                }
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="hostname">IP地址</param>
        /// <param name="port">端口</param>
        public TcpProcessor(string hostname, int port) : this(new TcpClient(hostname, port))
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        public TcpProcessor(IPAddress address, int port) : this(new IPEndPoint(address, port))
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="remoteEP">地址</param>
        public TcpProcessor(IPEndPoint remoteEP) : this(new TcpClient(remoteEP))
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="client">tcp</param>
        public TcpProcessor(TcpClient client)
        {
            this.m_heartBeatRequestData = new byte[1];
            this.m_heartBeatInterval = 1000;
            this.m_stopHeartBeat = true;
            this.m_reportedDisconnected = false;
            this.m_client = null;
            this.m_stopReceiveData = true;
            this.m_lastReceiveDataTime = DateTime.Now;
            this.m_lastSendDataTime = DateTime.Now;
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            this.m_client = client;
        }

        /// <summary>
        /// 开始心态连接
        /// </summary>
        public void StartHeartBeat()
        {
            this.StartHeartBeat(this.m_heartBeatInterval);
        }

        /// <summary>
        /// 开始心跳连接
        /// </summary>
        /// <param name="interval"></param>
        public void StartHeartBeat(int interval)
        {
            if (interval <= 0)
            {
                throw new ArgumentException("心跳频率必须大于0", nameof(interval));
            }
            if (this.m_stopHeartBeat)
            {
                this.m_heartBeatInterval = interval;
                ThreadPool.QueueUserWorkItem(this.RunHeartBeat, this.m_client);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        public void Send(byte[] data)
        {
            if (data != null)
            {
                this.Send(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="offset">offset</param>
        /// <param name="size">大小</param>
        public void Send(byte[] data, int offset, int size)
        {
            if (data != null && data.Length > 0 && data.Length > offset)
            {
                if (this.m_client == null)
                {
                    throw new InvalidOperationException("client");
                }
                if (this.m_client.Connected)
                {
                    NetworkStream stream = this.m_client.GetStream();
                    if (stream.CanWrite)
                    {
                        byte[] array = this.FormatData(data, offset, size);
                        try
                        {
                            stream.Write(array, 0, array.Length);
                        }
                        catch (SocketException)
                        {
                            this.OnConnectionDisconnected(this.m_client);
                        }
                        catch (IOException)
                        {
                            this.OnConnectionDisconnected(this.m_client);
                        }
                        this.m_lastSendDataTime = DateTime.Now;
                    }
                }
                else
                {
                    this.OnConnectionDisconnected(this.m_client);
                }
            }
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="callback">回调函数</param>
        /// <param name="state">参数</param>
        public void AsyncSend(byte[] data, WaitCallback callback, object state)
        {
            this.AsyncSend(data, callback, 0, data?.Length ?? 0, state);
        }

        /// <summary>
        /// 异步发送请求
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="callback">回调函数</param>
        /// <param name="offset">offset</param>
        /// <param name="size">size</param>
        /// <param name="state">参数</param>
        public void AsyncSend(byte[] data, WaitCallback callback, int offset, int size, object state)
        {
            AsyncSendData state2 = new AsyncSendData
            {
                Callback = callback,
                Data = data,
                Offset = offset,
                Size = size,
                State = state
            };
            ThreadPool.QueueUserWorkItem(this.SendData, state2);
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns>结果</returns>
        public byte[] ReceiveVal()
        {
            while (this.m_client.Connected)
            {
                var array = this.Receive(this.m_client);
                if (array != null)
                {
                    return array;
                }
            }
            this.OnConnectionDisconnected(this.m_client);
            return null;
        }

        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">参数</param>
        public void AsyncReceive(ReceiveDataCallback callback, object state)
        {
            AsyncReceiveData state2 = new AsyncReceiveData
            {
                Client = this.m_client,
                Callback = callback,
                State = state
            };
            ThreadPool.QueueUserWorkItem(this.AsyncReceive, state2);
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        public void StartReceive()
        {
            if (this.m_stopReceiveData)
            {
                ThreadPool.QueueUserWorkItem(this.RunReceive, this.m_client);
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
        /// <param name="disposing">是否是否资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_client != null)
                {
                    this.m_client.Close();
                    this.m_client = null;
                }
            }
            this.m_stopReceiveData = true;
            this.m_stopHeartBeat = true;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="state">参数</param>
        private void SendData(object state)
        {
            AsyncSendData asyncSendData = state as AsyncSendData;
            if (asyncSendData != null)
            {
                this.Send(asyncSendData.Data, asyncSendData.Offset, asyncSendData.Size);
                asyncSendData.Callback?.Invoke(asyncSendData.State);
            }
        }

        /// <summary>
        /// 数据格式化
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="offset">offset</param>
        /// <param name="size">大小</param>
        /// <returns>结果</returns>
        private byte[] FormatData(byte[] data, int offset, int size)
        {
            byte[] array;
            if (data == null)
            {
                array = Utility.ToBytes(0);
            }
            else
            {
                byte[] array2 = Utility.ToBytes(data.Length);
                array = new byte[array2.Length + data.Length];
                int count = (offset + size > data.Length) ? data.Length : size;
                Buffer.BlockCopy(array2, 0, array, 0, array2.Length);
                Buffer.BlockCopy(data, 0, array, array2.Length, count);
            }
            return array;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        public byte[] Receive()
        {
            while (this.m_client.Connected)
            {
                var array = this.Receive(this.m_client);
                if (array != null)
                {
                    return array;
                }
            }
            this.OnConnectionDisconnected(this.m_client);
            return null;
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="client">Tcp</param>
        /// <returns>结果</returns>
        private byte[] Receive(TcpClient client)
        {
            byte[] result = null;
            if (client.Available > 0)
            {
                int contentLength = this.GetContentLength(client);
                byte[] array = this.ReceiveData(client, contentLength);
                this.m_lastReceiveDataTime = DateTime.Now;
                if (array.Length > 0)
                {
                    if (!Utility.Equals(this.m_heartBeatRequestData, array))
                    {
                        result = array;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="state">参数</param>
        private void AsyncReceive(object state)
        {
            AsyncReceiveData asyncReceiveData = (AsyncReceiveData) state;
            if (asyncReceiveData != null)
            {
                while (asyncReceiveData.Client.Connected)
                {
                    byte[] array = this.Receive(asyncReceiveData.Client);
                    if (array != null)
                    {
                        asyncReceiveData.Callback?.Invoke(array, asyncReceiveData.State);
                        goto IL_83;
                    }
                }
                this.OnConnectionDisconnected(asyncReceiveData.Client);
            }
            IL_83:;
        }

        /// <summary>
        /// 运行接收参数
        /// </summary>
        /// <param name="state">参数</param>
        private void RunReceive(object state)
        {
            if (state != null)
            {
                this.m_stopReceiveData = false;
                TcpClient tcpClient = (TcpClient)state;
                while (!this.m_stopReceiveData)
                {
                    if (tcpClient.Connected)
                    {
                        byte[] array = this.Receive(tcpClient);
                        if (array != null)
                        {
                            this.OnDataReceived(tcpClient, array);
                        }
                    }
                    else
                    {
                        this.OnConnectionDisconnected(tcpClient);
                    }
                    Thread.Sleep(5);
                }
            }
        }

        /// <summary>
        /// 获取数据内容长度
        /// </summary>
        /// <param name="client">Tcp</param>
        /// <returns>结果</returns>
        private int GetContentLength(TcpClient client)
        {
            int result = 0;
            byte[] array = this.ReceiveData(client, this.m_lengthArrayLength);
            if (array.Length == this.m_lengthArrayLength)
            {
                result = Utility.ToInt(array);
            }
            return result;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="client">Tcp</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        private byte[] ReceiveData(TcpClient client, int length)
        {
            MemoryStream memoryStream = new MemoryStream();
            NetworkStream stream = client.GetStream();
            while (client.Available > 0)
            {
                if (memoryStream.Length >= length)
                {
                    break;
                }
                long size = client.Available < length - memoryStream.Length ? client.Available : length - memoryStream.Length;
                byte[] array = new byte[size];
                stream.Read(array, 0, array.Length);
                memoryStream.Write(array, 0, array.Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// 运行心跳连接
        /// </summary>
        /// <param name="state">状态</param>
        private void RunHeartBeat(object state)
        {
            this.m_stopHeartBeat = true;
            TcpClient tcpClient = state as TcpClient;
            if (tcpClient != null)
            {
                if (tcpClient.Connected)
                {
                    this.m_stopHeartBeat = false;
                }
                else
                {
                    this.OnConnectionDisconnected(tcpClient);
                }
                while (!this.m_stopHeartBeat)
                {
                    if (this.ConnectionInvalid())
                    {
                        try
                        {
                            this.SendHeartBeatRequest();
                            this.m_lastSendDataTime = DateTime.Now;
                        }
                        catch (SocketException)
                        {
                            this.OnConnectionDisconnected(tcpClient);
                        }
                        catch (IOException)
                        {
                            this.OnConnectionDisconnected(tcpClient);
                        }
                    }
                    Thread.Sleep(this.m_heartBeatInterval);
                }
            }
        }

        /// <summary>
        /// 发送心跳请求
        /// </summary>
        private void SendHeartBeatRequest()
        {
            this.Send(this.m_heartBeatRequestData, 0, this.m_heartBeatRequestData.Length);
        }

        /// <summary>
        /// 连接是否有效
        /// </summary>
        /// <returns>结果</returns>
        private bool ConnectionInvalid()
        {
            return (DateTime.Now - this.m_lastReceiveDataTime).TotalMilliseconds >= this.m_heartBeatInterval && (DateTime.Now - this.m_lastSendDataTime).TotalMilliseconds >= this.m_heartBeatInterval;
        }

        /// <summary>
        /// 连接断开事件
        /// </summary>
        /// <param name="client">Tcp</param>
        private void OnConnectionDisconnected(TcpClient client)
        {
            this.m_stopReceiveData = true;
            this.m_stopHeartBeat = true;
            if (!this.m_reportedDisconnected && this.ConnectionDisconnected != null)
            {
                this.m_reportedDisconnected = true;
                ConnectionDisconnectedEventArgs e = new ConnectionDisconnectedEventArgs
                {
                    Client = client
                };
                this.ConnectionDisconnected(this, e);
            }
        }

        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// <param name="client">tcp</param>
        /// <param name="data">数据</param>
        private void OnDataReceived(TcpClient client, byte[] data)
        {
            if (this.DataReceived != null)
            {
                DataReceivedEventArgs e = new DataReceivedEventArgs
                {
                    Client = client,
                    Data = data
                };
                this.DataReceived(this, e);
            }
        }
    }
}