using System;

namespace Notify.Code.Code
{
    /// <summary>
    /// 主键工厂类
    /// </summary>
    public class KeyIdFactory
    {
        /// <summary>
        /// Fields
        /// </summary>
        private static long lastIdentity;

        /// <summary>
        /// locker
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 新的主键ID
        /// </summary>
        /// <returns>主键ID</returns>
        public static string NewKeyId()
        {
            return NewKeyId(32);
        }

        /// <summary>
        /// 新的主键ID
        /// </summary>
        /// <param name="length">ID长度(不能小于24)</param>
        /// <returns>主键ID</returns>
        public static string NewKeyId(int length)
        {
            if (length <= 24)
            {
                length = 24;
            }

            string strNum = string.Empty;
            for (int i = 24; i <= length; i++)
            {
                strNum += "9";
            }

            lock (locker)
            {
                string str = lastIdentity.ToString().PadLeft(strNum.Length, '0');
                string str2 = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                lastIdentity++;

                if (lastIdentity > long.Parse(strNum))
                {
                    lastIdentity = 0;
                }
                Random random = new Random(Common.CreateRandomSeed());
                var r = random.Next(1000000, 9999999);
                return string.Format("{0}{1}{2}", str2, r, str);
            }
        }
    }
}