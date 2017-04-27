using System;
using System.Collections.Concurrent;
using System.Threading;
using Notify.Code.Write;

namespace Notify.Code.Utility
{
    /// <summary>
    /// 表示一个服务资源
    /// </summary>
    public class ServerResource
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        public ServerResource(string ip, int port)
        {
            this.Ip = ip;
            this.Port = port;
        }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 请求
        /// </summary>
        public RequestInfo Request { get; private set; }

        /// <summary>
        /// 请求队列信息
        /// </summary>
        public RequestQueue RequestQueue { get; private set; }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="queue">请求队列信息</param>
        public void GetRequest(RequestInfo request, RequestQueue queue)
        {
            this.Request = request;
            this.RequestQueue = queue;
            ThreadPool.QueueUserWorkItem(o => this.RequestInBackground());
        }

        /// <summary>
        /// 请求后台线程
        /// </summary>
        private void RequestInBackground()
        {
            try
            {
                // 执行后台请求
                var str = "sss";
                this.Request.Response = str;
            }
            finally
            {
                this.RequestQueue.ResourceWaitHandleSet();
                this.RequestQueue.ResponseWaitHandleSet();
                RequestQueue.AddServerResource(this);
            }
        }
    }

    /// <summary>
    /// 请求
    /// </summary>
    public class RequestInfo
    {
        /// <summary>
        /// 请求构造函数
        /// </summary>
        /// <param name="content">请求内容</param>
        public RequestInfo(string content)
        {
            this.Id = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper();
            this.Content = content;
        }

        /// <summary>
        /// 请求标识
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        public bool Responsed { get; private set; }

        /// <summary>
        /// 请求结果内容
        /// </summary>
        private string response;

        /// <summary>
        /// 请求结果内容
        /// </summary>
        public string Response
        {
            get
            {
                return this.response;
            }
            set
            {
                this.response = value;
                this.Responsed = true;
            }
        }
    }

    /// <summary>
    /// 队列消息
    /// </summary>
    public class RequestQueue
    {
        /// <summary>
        /// 请求队列
        /// </summary>
        private static readonly ConcurrentQueue<RequestInfo> requestQueue = new ConcurrentQueue<RequestInfo>();

        /// <summary>
        /// 资源集合
        /// </summary>
        private static readonly ConcurrentQueue<ServerResource> resourceQueue = new ConcurrentQueue<ServerResource>();

        /// <summary>
        /// 请求阻塞
        /// </summary>
        private readonly AutoResetEvent requestWaitHandle = new AutoResetEvent(false);

        /// <summary>
        /// 响应阻塞
        /// </summary>
        private readonly AutoResetEvent responseWaitHandle = new AutoResetEvent(false);

        /// <summary>
        /// 资源阻塞
        /// </summary>
        private readonly AutoResetEvent resourceWaitHandle = new AutoResetEvent(false);

        /// <summary>
        /// 加载资源
        /// </summary>
        static RequestQueue()
        {
            var one = new ServerResource("192.168.1.1", 8000);
            var two = new ServerResource("192.168.1.2", 8000);
            AddServerResource(one);
            AddServerResource(two);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestQueue()
        {
            ThreadPool.QueueUserWorkItem(o => { this.RequestInBackground(); });
        }

        /// <summary>
        /// 请求申请
        /// </summary>
        /// <param name="content">请求内容</param>
        /// <returns>结果</returns>
        public string Request(string content)
        {
            RequestInfo request = new RequestInfo(content);
            requestQueue.Enqueue(request);
            requestWaitHandle.Set();
            while (true)
            {
                if (request.Responsed)
                {
                    return request.Response;
                }
                responseWaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// 请求线程
        /// </summary>
        private void RequestInBackground()
        {
            try
            {
                RequestInfo request = this.GetRequest();
                ServerResource resource = this.GetReource();
                resource.GetRequest(request, this);
            }
            catch (System.Exception ex)
            {
                LogService.WriteLog(ex, "RequestQueue_RequestInBackground");
            }
        }

        /// <summary>
        /// 获取请求信息
        /// </summary>
        /// <returns>请求信息</returns>
        private RequestInfo GetRequest()
        {
            RequestInfo request;
            if (!requestQueue.TryDequeue(out request))
            {
                requestWaitHandle.WaitOne();
                return this.GetRequest();
            }
            return request;
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <returns>资源信息</returns>
        private ServerResource GetReource()
        {
            ServerResource resource;
            if (!resourceQueue.TryDequeue(out resource))
            {
                resourceWaitHandle.WaitOne();
                return this.GetReource();
            }
            return resource;
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="serverResource">资源</param>
        public static void AddServerResource(ServerResource serverResource)
        {
            resourceQueue.Enqueue(serverResource);
        }

        /// <summary>
        /// 资源Set
        /// </summary>
        public void ResourceWaitHandleSet()
        {
            this.resourceWaitHandle.Set();
        }

        /// <summary>
        /// 响应Set
        /// </summary>
        public void ResponseWaitHandleSet()
        {
            this.responseWaitHandle.Set();
        }
    }
}
