using System;
using System.Security.Cryptography;

namespace Notify.Code.Code
{
    public class Common
    {
        /// <summary>
        /// 随机种子
        /// </summary>
        /// <returns>结构</returns>
        public static int CreateRandomSeed()
        {
            byte[] bytes = new byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
