using System;
using System.Security.Cryptography;
using System.Text;

namespace Notify.Code.Encrypt
{
    /// <summary>
    /// AES
    /// </summary>	
    public class AES
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">数据字符</param>
        /// <returns>密文</returns>
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">密文</param>
        /// <returns>结果</returns>
        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}