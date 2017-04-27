using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using Notify.Code.Code;
using Notify.Code.Extension;


namespace Notify.Code.Utility
{
    using System.Security.Cryptography.X509Certificates;

    using Exception = System.Exception;

    /// <summary>
    /// The http utility.
    /// </summary>
    internal static class HttpRequestUtility
    {
        /// <summary>
        /// GetStream
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>Stream</returns>
        internal static Stream GetStream(Request request)
        {
            HttpWebResponse response = null;
            Stream responseStream = null;
            try
            {
                var httpWebRequest = GetHttpWebRequest(request);
                response = (HttpWebResponse)httpWebRequest.GetResponse();
                responseStream = response.GetResponseStream();
                byte[] buffer = StreamToBytes(responseStream);
                Stream memoryStream = new MemoryStream(buffer);
                request.Cookie = httpWebRequest.CookieContainer;
                return memoryStream;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Dispose();
                    responseStream.Close();
                }

                if (response != null)
                {
                    response.Dispose();
                    response.Close();
                }
            }
        }

        /// <summary>
        /// GteParameters
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>string</returns>
        private static string GteParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return string.Empty;
            }

            return parameters.Join("&", p => p.Key + "=" + p.Value);
        }

        /// <summary>
        /// CreateUri
        /// </summary>
        /// <param name="request">url</param>
        /// <returns>Uri</returns>
        private static Uri CreateUri(Request request)
        {
            if (string.IsNullOrEmpty(request.Url))
            {
                throw new ArgumentNullException("url");
            }

            string url = request.Url;
            if (request.HttpMethod == HttpMethod.Get)
            {
                string parameters = GteParameters(request.Parameters);
                url = url + "?" + parameters;
            }

            Uri uri;
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return uri;
            }
            else
            {
                throw new ArgumentException("请求目标地址格式错误");
            }
        }

        /// <summary>
        /// 设置请求基本信息
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>HttpWebRequest</returns>
        private static HttpWebRequest GetHttpWebRequest(Request request)
        {
            Uri uri = CreateUri(request);
            HttpWebRequest webRequest = WebRequest.CreateHttp(uri);
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            webRequest.CachePolicy = policy;
            webRequest.Timeout = request.Timeout;
            webRequest.KeepAlive = request.KeepAlive;
            webRequest.Method = request.HttpMethod.ToString();
            webRequest.CookieContainer = request.Cookie;
            webRequest.Referer = request.Referer;
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)";
            webRequest.Headers.Add("Cache-Control", "no-cache");
            webRequest.Accept = "*/*";
            webRequest.Credentials = CredentialCache.DefaultCredentials;

            SetProxy(webRequest, request);

            SetCertificate(webRequest, request);

            SetParameters(webRequest, request);

            return webRequest;
        }

        /// <summary>
        /// 设置代理信息
        /// </summary>
        /// <param name="webRequest">httpWebRequest</param>
        /// <param name="request">request</param>
        private static void SetProxy(WebRequest webRequest, Request request)
        {
            if (request.Proxy != null)
            {
                webRequest.Proxy = request.Proxy;
            }
        }

        /// <summary>
        /// 设置请求证书
        /// </summary>
        /// <param name="webRequest">httpWebRequest</param>
        /// <param name="request">request</param>
        private static void SetCertificate(HttpWebRequest webRequest, Request request)
        {
            if (request.Certificate != null)
            {
                try
                {
                    webRequest.ClientCertificates.Add(new X509Certificate2(request.Certificate.CertFile, request.Certificate.CertPasswd, X509KeyStorageFlags.MachineKeySet));
                }
                catch (Exception)
                {
                    X509Store store = new X509Store("My", StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2 cert = store.Certificates.Find(X509FindType.FindBySubjectName, request.Certificate.CertPasswd, false)[0];
                    webRequest.ClientCertificates.Add(cert);
                }
            }
        }

        /// <summary>
        /// 设置请求参数信息
        /// </summary>
        /// <param name="webRequest">httpWebRequest</param>
        /// <param name="request">request</param>
        private static void SetParameters(WebRequest webRequest, Request request)
        {
            if (request.HttpMethod == HttpMethod.Post)
            {
                string parameters = GteParameters(request.Parameters);
                byte[] bytes = request.Encoding.GetBytes(parameters);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = bytes.Length;
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// 数据流转换
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>字节数组</returns>
        private static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }

            return bytes.ToArray();
        }
    }
}