using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Notify.Code.Encrypt
{
    /// <summary>
    /// Des加密解密类
    /// </summary>
    public class Des
    {
        /// <summary>
        /// Key
        /// </summary>
        private const string KEY = "xda14121";

        /// <summary>
        /// IV
        /// </summary>
        private const string IV = "vva14121";

        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <returns>密文</returns>
        public static string Encrypt(string source)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] key = Encoding.ASCII.GetBytes(KEY);
                byte[] iv = Encoding.ASCII.GetBytes(IV);
                byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
                des.Mode = CipherMode.CBC;
                des.Key = key;
                des.IV = iv;
                string encrypt;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return encrypt;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="source">要解密的base64串</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string source)
        {
            try
            {
                byte[] inputByteArray = Convert.FromBase64String(source);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = Encoding.ASCII.GetBytes(KEY);
                    des.IV = Encoding.ASCII.GetBytes(IV);
                    MemoryStream ms = new MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
    }
}