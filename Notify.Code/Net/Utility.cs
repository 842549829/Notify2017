using System;

namespace Notify.Code.Net
{
    /// <summary>
    /// 实用程序
    /// </summary>
    internal class Utility
    {
        /// <summary>
        /// 检查数据长度
        /// </summary>
        /// <param name="data">数据</param>
        private static void CkeckLength(byte[] data)
        {
            if (data.Length == 0)
            {
                throw new InvalidOperationException("data");
            }
        }

        /// <summary>
        /// 转化成long
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="offset">offset</param>
        /// <param name="width">offset</param>
        /// <returns>结果</returns>
        private static long ToLong(byte[] data, int offset, int width)
        {
            if (width < 0 || width > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "width must between 1 and 8");
            }
            if (offset < 0 || offset > data.Length - 1)
            {
                throw new IndexOutOfRangeException();
            }
            if (offset + width > data.Length)
            {
                width = data.Length - offset;
            }
            long num = 0L;
            for (int i = offset; i < width + offset; i++)
            {
                num += (long)data[i] << 8 * (width - 1 - i + offset);
            }
            return num;
        }

        /// <summary>
        /// 转化成int
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public static int ToInt(byte[] data)
        {
            return ToInt(data, 0);
        }

        /// <summary>
        /// 转化成int
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="offset">offset</param>
        /// <returns>结果</returns>
        public static int ToInt(byte[] data, int offset)
        {
            return ToInt(data, offset, 4);
        }

        /// <summary>
        /// 转化成int
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="offset">offset</param>
        /// <param name="count">长度</param>
        /// <returns>结果</returns>
        public static int ToInt(byte[] data, int offset, int count)
        {
            CkeckLength(data);
            return (int)ToLong(data, offset, count);
        }

        /// <summary>
        /// 转化byte[]
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>结果</returns>
        public static byte[] ToBytes(int value)
        {
            int num = 4;
            byte[] array = new byte[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = (byte)(value >> 8 * (num - 1 - i));
            }
            return array;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="arr1">参数1</param>
        /// <param name="arr2">参数2</param>
        /// <returns>结果</returns>
        public static bool Equals<T>(T[] arr1, T[] arr2)
        {
            bool result = false;
            if (arr1 != null && null != arr2)
            {
                if (arr1.Length == arr2.Length)
                {
                    result = true;
                    for (int i = 0; i < arr1.Length; i++)
                    {
                        if (!arr1[i].Equals(arr2[i]))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}