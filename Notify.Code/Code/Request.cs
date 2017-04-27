using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Notify.Code.Code
{
    /// <summary>
    /// The request.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        public Request(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// 请求目标地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Referer HTTP 标头的值
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer Cookie { get; set; }

        /// <summary>
        /// 代理
        /// </summary>
        public WebProxy Proxy { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 参数编码格式
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 是否与 Internet 资源建立持久性连接。
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// 请求超时时间(单位毫秒)
        /// </summary>
        public int Timeout { get; set; } = 30 * 0x3e8;

        /// <summary>
        /// 请求参数
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// 证书
        /// </summary>
        public Certificate Certificate { get; set; }
    }

    /// <summary>
    /// 证书
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// 证书地址
        /// </summary>
        public string CertFile { get; set; }

        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertPasswd { get; set; }
    }
}
