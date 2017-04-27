using System;
using System.Security.Cryptography;
using System.Text;

namespace Notify.Code.Encrypt
{
    /// <summary>
    /// RSA
    /// </summary>	
    public class RSA
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publicKey">publicKey</param>
        /// <param name="content">content</param>
        /// <returns>结果</returns>
        public static string Encrypt(string publicKey, string content)
        {
            /* 将文本转换成byte数组 */
            byte[] source = Encoding.Default.GetBytes(content);

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);    //设置公钥
            var ciphertext = rsa.Encrypt(source, false);

            /* 对字符数组进行转码 */
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ciphertext)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();    //返回结果
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKey">privateKey</param>
        /// <param name="content">content</param>
        /// <returns>结果</returns>
        public static string Decrypt(string privateKey, string content)
        {
            /* 将文本转换成byte数组 */
            byte[] ciphertext = new byte[content.Length / 2];
            for (int x = 0; x < content.Length / 2; x++)
            {
                int i = (Convert.ToInt32(content.Substring(x * 2, 2), 16));
                ciphertext[x] = (byte)i;
            }

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);    //设置私钥
            var source = rsa.Decrypt(ciphertext, false);

            return Encoding.Default.GetString(source);    //返回结果
        }

        /// <summary>
        /// 创建一对 RSA 密钥（公钥adn私钥）。
        /// </summary>
        /// <returns></returns>
        public static RSAKey CreateRSAKey()
        {
            RSAKey rsaKey = new RSAKey();    //声明一个RSAKey对象
            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsaKey.PrivateKey = rsa.ToXmlString(true);    //创建私钥
            rsaKey.PublicKey = rsa.ToXmlString(false);    //创建公钥
            return rsaKey;    //返回结果
        }
    }
}