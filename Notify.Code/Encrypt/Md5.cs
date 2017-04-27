using System;
using System.Security.Cryptography;
using System.Text;

namespace Notify.Code.Encrypt
{
    /// <summary>
    /// Md5
    /// </summary>	
    public class Md5
    {
        /// <summary>
        /// 加密32
        /// </summary>
        /// <param name="source">数据</param>
        /// <returns>密文</returns>
        public static string Encrypt32(string source)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder sb = new StringBuilder(32);
            foreach (byte item in t)
            {
                sb.Append(item.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 加密16
        /// </summary>
        /// <param name="source">数据</param>
        /// <returns>密文</returns>
        public static string Encrypt16(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(source)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2.ToUpper();
        }
    }
}