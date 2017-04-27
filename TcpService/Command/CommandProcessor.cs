using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpService.Command
{
    internal abstract class CommandProcessor
    {
        /// <summary>
        /// Tcp
        /// </summary>
        public TcpClient Connection
        {
            get;
            private set;
        }

        /// <summary>
        /// 指令
        /// </summary>
        public string Command
        {
            get;
            private set;
        }

        /// <summary>
        /// 请求信息
        /// </summary>
        public string Request
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取指令对象
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="connection">Tcp</param>
        /// <returns></returns>
        public static CommandProcessor GetCommandProcessor(string request, TcpClient connection)
        {
            CommandProcessor result = null;
            switch (GetCommand(request))
            {
                case "LOGON":
                    result = new Logon();
                    break;
                //case "LOGOFF":
                //    result = new Logoff();
                //    break;
                //case "SAVECARRIER":
                //    result = new SaveCarrier();
                //    break;
                //case "SAVESTATUS":
                //    result = new SaveStatus();
                //    break;
                //case "QUERYCARRIER":
                //    result = new QueryCarrier();
                //    break;
                //case "QUERYSTATUS":
                //    result = new QueryStatus();
                //    break;
                //case "QUERYALLCARRIERS":
                //    result = new QueryAllCarriers();
                //    break;
                //case "QUERYALLSTATUS":
                //    result = new QueryAllStatus();
                    break;
            }
            if (result != null)
            {
                result.Command = GetCommand(request);
                result.Request = GetRequestContent(request);
                result.Connection = connection;
            }
            return result;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>结果</returns>
        public string Execute()
        {
            string response;
            try
            {
                response = ExecuteCore();
            }
            catch
            {
                response = "99";
            }
            return this.Command + "/" + response;
        }

        /// <summary>
        /// 是否处理
        /// </summary>
        public virtual bool DisposeConnection
        {
            get { return true; }
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <returns>结果</returns>
        protected abstract string ExecuteCore();

        /// <summary>
        /// 获取指令对象
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>结果</returns>
        private static string GetCommand(string request)
        {
            return string.IsNullOrWhiteSpace(request) ? string.Empty : request.Substring(0, request.IndexOf('/'));
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>结果</returns>
        private static string GetRequestContent(string request)
        {
            return request.Substring(request.IndexOf('/') + 1);
        }
    }
}