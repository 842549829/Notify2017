using System;
using System.Security.Cryptography;
using System.Text;

namespace Notify.Code.Encrypt
{
    /// <summary>
    /// SHA1
    /// </summary>	
    public class SHA1
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="source">数据</param>
        /// <returns>密文</returns>
        public static string Encrypt(string source)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(source);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            ((IDisposable)sha1).Dispose();
            return Convert.ToBase64String(str2);
        }
    }
}