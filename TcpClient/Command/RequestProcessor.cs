using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notify.Code.Net;

namespace TcpClient.Command
{
    abstract class RequestProcessor<TResponse>
    {
        private string m_host;
        private int m_port;
        private Encoding m_encoding = Encoding.GetEncoding("gb2312");

        protected RequestProcessor(string host, int port, Guid logonId)
        {
            this.m_host = host;
            this.m_port = port;
            this.LogonId = logonId;
        }

        protected System.Net.Sockets.TcpClient Connection
        {
            get;
            private set;
        }

        protected virtual bool KeepConnection
        {
            get { return false; }
        }

        public Guid LogonId
        {
            get;
            private set;
        }

        public CommandResult<TResponse> Execute()
        {
            string content = prepareRequest();
            try
            {
                string response = sendRequest(content);
                return parseResponse(response);
            }
            catch (System.Net.Sockets.SocketException)
            {
                return new CommandResult<TResponse>()
                {
                    Success = false,
                    ErrorMessage = "无法连接服务器"
                };
            }
            catch (Exception ex)
            {
                return new CommandResult<TResponse>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        protected abstract string Command
        {
            get;
        }
        protected abstract string PrepareRequestContent();
        protected abstract TResponse ParseResponseCore(string content);

        private string prepareRequest()
        {
            var result = this.Command;
            var content = PrepareRequestContent();
            if (content != null)
            {
                result += "/" + content;
            }
            return result;
        }
        private string sendRequest(string content)
        {
            if (KeepConnection)
            {
                this.Connection = new System.Net.Sockets.TcpClient(this.m_host, this.m_port);
                return sendResultCore(new TcpProcessor(this.Connection), content);
            }
            else
            {
                using (var processor = new TcpProcessor(m_host, m_port))
                {
                    return sendResultCore(processor, content);
                }
            }
        }
        private string sendResultCore(TcpProcessor processor, string content)
        {
            processor.Send(m_encoding.GetBytes(content));
            var datas = processor.Receive();
            return m_encoding.GetString(datas);
        }
        private CommandResult<TResponse> parseResponse(string response)
        {
            var result = new CommandResult<TResponse>();
            var array = response.Split('/');
            if (array.Length >= 2)
            {
                var code = array[1];
                if (code == "0")
                {
                    result.Success = true;
                    result.Response = ParseResponseCore(getResponseContent(response));
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = getErrorMessage(code);
                }
            }
            return result;
        }
        private string getErrorMessage(string errorCode)
        {
            if (errorCode == "1")
            {
                return "用户名不存在";
            }
            else if (errorCode == "2")
            {
                return "无登录信息";
            }
            else if (errorCode == "3")
            {
                return "指令格式错误";
            }
            else if (errorCode == "4")
            {
                return "密码错误";
            }
            else if (errorCode == "5")
            {
                return "账号被禁用";
            }
            else if (errorCode == "6")
            {
                return "加载单位信息失败";
            }
            else if (errorCode == "7")
            {
                return "单位被禁用";
            }
            else if (errorCode == "8")
            {
                return "非法用户";
            }
            else if (errorCode == "99")
            {
                return "系统错误";
            }
            else
            {
                return "未知错误";
            }
        }
        private string getResponseContent(string response)
        {
            var array = response.Split('/');
            if (array.Length > 2)
            {
                return response.Substring(array[0].Length + array[1].Length + 2);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
