using System;
using TcpService.Model;

namespace TcpService.Command
{
    /// <summary>
    /// 登录
    /// </summary>
    internal class Logon : CommandProcessor
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <returns>结果</returns>
        protected override string ExecuteCore()
        {
            var array = this.Request.Split('/');
            if (array.Length == 2)
            {
                var userName = array[0];
                var password = array[1];
                User user;
                string errorCode;
                if (LogonCenter.Instance.Logon(userName, password, this.Connection, out user, out errorCode))
                {
                    var ipEndPoint = this.Connection.Client.RemoteEndPoint as System.Net.IPEndPoint;
                    if (ipEndPoint != null)
                    {
                        Console.WriteLine("{0} 用户[{1}]登录 {2} 地址:{3} 批次号:{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, Environment.NewLine, ipEndPoint.Address, user.Id);
                    }
                    return "0/" + user.Id;
                }
                else
                {
                    var ipEndPoint = this.Connection.Client.RemoteEndPoint as System.Net.IPEndPoint;
                    if (ipEndPoint != null)
                        Console.WriteLine("{0} 用户[{1}]登录失败 {2} 地址:{3} 错误代码:{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, Environment.NewLine, ipEndPoint.Address, errorCode);
                    return errorCode;
                }
            }
            return "3";
        }

        /// <summary>
        /// 是否处理
        /// </summary>
        public override bool DisposeConnection => false;
    }
}