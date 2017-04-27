using System;
using System.Linq;
using System.Web;
using Notify.Code.Code;
using Notify.Code.Constant;
using Notify.Model.Transfer;
using Notify.Service;

namespace Notify.Controller.Base
{
    /// <summary>
    /// 登录工具类
    /// </summary>
    public class LogonUtility
    {
        /// <summary>
        /// 验证码
        /// </summary>
        private static string ValidateCode
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Const.LoginValidateCodeSessionKey] == null)
                {
                    return null;
                }
                return HttpContext.Current.Session[Const.LoginValidateCodeSessionKey] as string;
            }
            set
            {
                HttpContext.Current.Session[Const.LoginValidateCodeSessionKey] = value;
            }
        }

        /// <summary>
        /// 产生登录验证码
        /// </summary>
        /// <returns>返回验证码</returns>
        public static byte[] GenerateValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            HttpContext.Current.Session[Const.LoginValidateCodeSessionKey] = code;
            ValidateCode = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return bytes;
        }

        /// <summary>
        /// 较验验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns>结果</returns>
        private static bool ValidateValidateCode(string code)
        {
            return string.Equals(ValidateCode, code, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 清除验证码
        /// </summary>
        private static void ClearValidateCode()
        {
            HttpContext.Current.Session[Const.LoginValidateCodeSessionKey] = null;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="password">登录密码</param>
        /// <param name="validateCode">登录验证码</param>
        /// <returns>登录结果</returns>
        public static Result Logon(string userName, string password, string validateCode)
        {
            Result result = new Result();
            if (ValidateValidateCode(validateCode))
            {
                result = Logon(userName, password);
            }
            else
            {
                result.IsSucceed = false;
                result.Message = "验证码错误，请重新输入";
            }
            ClearValidateCode();
            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="password">登录密码</param>
        /// <returns>登录结果</returns>
        private static Result Logon(string userName, string password)
        {
            var ipAddress = HttpContext.Current.Request.UserHostAddress;
            var loginInfo = CreateLoginInfo(userName, password, ipAddress);
            var result = AccountService.Login(loginInfo);
            if (result.IsSucceed)
            {
                if (result.Menu.Any())
                {
                    HttpContext.Current.Session[Const.UserSessionKey] = result.Account;
                    HttpContext.Current.Session[Const.MenuSessionKey] = result.Menu;
                    HttpContext.Current.Session[Const.EsayUIMenuSessionKey] = result.EsayUiMenu;
                }
                else
                {
                    result.IsSucceed = false;
                    result.Message = "无访问权限";
                }
            }

            return result;
        }

        /// <summary>
        /// 创建登录信息
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="password">登录密码</param>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>登录信息</returns>
        private static LoginInfo CreateLoginInfo(string userName, string password, string ipAddress)
        {
            var loginInfo = new LoginInfo
            {
                AccountNo = userName,
                LoginPassword = password,
                ClinetIp = ipAddress
            };

            return loginInfo;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Logoff()
        {
            // 清除保存的登录账号信息
            var cookie = new HttpCookie("userKey")
            {
                Expires = DateTime.Now.AddDays(-1)
            }; //初使化并设置Cookie的名称
            //设置过期时间
            cookie.Values.Add("user", "");
            HttpContext.Current.Response.AppendCookie(cookie);
            HttpContext.Current.Session.Clear();
            //System.Web.Security.FormsAuthentication.SignOut();
        }
    }
}