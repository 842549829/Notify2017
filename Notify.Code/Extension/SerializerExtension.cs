using Newtonsoft.Json;

namespace Notify.Code.Extension
{
    /// <summary>
    /// 序列化
    /// </summary>
    public static class SerializerExtension
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeObject(this object obj)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd hh:mm:ss" };
                return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>对象</returns>
        public static T DeserializeObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>对象</returns>
        public static dynamic DeserializeObject(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch
            {
                return null;
            }
        }
    }
}