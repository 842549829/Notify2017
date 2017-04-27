using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Notify.Code.Extension
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 value 参数为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns> 如果 value 参数为 null 或 System.String.Empty，或者如果 value 仅由空白字符组成，则为 true。</returns>
        public static bool IsEmptySpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        #region 拆分字符串
        /// <summary>
        /// 根据字符串拆分字符串
        /// </summary>
        /// <param name="source">要拆分的字符串</param>
        /// <param name="separator">拆分符</param>
        /// <returns>数组</returns>
        public static string[] Split(this string source, string separator)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }

            string[] strtmp = new string[1];
            // ReSharper disable once StringIndexOfIsCultureSpecific.2
            int index = source.IndexOf(separator, 0);
            if (index < 0)
            {
                strtmp[0] = source;
                return strtmp;
            }

            strtmp[0] = source.Substring(0, index);
            return Split(source.Substring(index + separator.Length), separator, strtmp);
        }

        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="source">要拆分的字符串</param>
        /// <param name="separator">拆分符</param>
        /// <param name="attachArray">attachArray</param>
        /// <returns>string[]</returns>
        private static string[] Split(string source, string separator, string[] attachArray)
        {
            // while循环的方式
            while (true)
            {
                string[] strtmp = new string[attachArray.Length + 1];
                attachArray.CopyTo(strtmp, 0);

                // ReSharper disable once StringIndexOfIsCultureSpecific.2
                int index = source.IndexOf(separator, 0);
                if (index < 0)
                {
                    strtmp[attachArray.Length] = source;
                    return strtmp;
                }

                strtmp[attachArray.Length] = source.Substring(0, index);
                source = source.Substring(index + separator.Length);
                attachArray = strtmp;
            }

            // 递归的方式
            /*
            string[] strtmp = new string[attachArray.Length + 1];
            attachArray.CopyTo(strtmp, 0);

            // ReSharper disable once StringIndexOfIsCultureSpecific.2
            int index = source.IndexOf(separator, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = source;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = source.Substring(0, index);
                return Split(source.Substring(index + separator.Length), separator, strtmp);
            }*/
        }

        #endregion

        /// <summary>
        /// 去末尾的0
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>value</returns>
        private static string TrimClearZeor(string value)
        {
            return value.IndexOf('.') > -1 ? value.TrimEnd('0').TrimEnd('.') : value;
        }

        /// <summary>
        /// 去末尾的0
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>value</returns>
        public static string TrimClearZero(this double value)
        {
            return TrimClearZeor(value.ToString());
        }

        /// <summary>
        /// 去末尾的0
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>value</returns>
        public static string TrimClearZero(this float value)
        {
            return TrimClearZeor(value.ToString());
        }

        /// <summary>
        /// 去末尾的0
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>value</returns>
        public static string TrimClearZero(this decimal value)
        {
            return TrimClearZeor(value.ToString());
        }

        /// <summary>
        /// 手机号码隐藏中间几位
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>结果</returns>
        public static string ToMobile(this string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return mobile;
            }
            if (mobile.Length != 11)
            {
                return mobile;
            }

            return Regex.Replace(mobile, @"(\d{3})\d{6}(\d{2})", "$1******$2");
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static string ToMd5(this string value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var byt = System.Text.Encoding.UTF8.GetBytes(value);
            var bytHash = md5.ComputeHash(byt);
            md5.Clear();
            return bytHash.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0')).ToUpper();
        }

        /// <summary>
        /// 字符串转UniCode
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>UniCode字符串</returns>
        public static string StringToUniCode(this string value)
        {
            char[] charbuffers = value.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format(@"\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// UniCode转字符串
        /// </summary>
        /// <param name="value">UniCode字符串</param>
        /// <returns>字符串</returns>
        public static string UnicodeToString(this string value)
        {
            string dst = string.Empty;
            string src = value;
            int len = value.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }
    }
}