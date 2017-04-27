using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using Notify.Code.Extension;

namespace Notify.Code.Utility
{
    public class HttpClientUtility
    {
        private T Assign<T>(Dictionary<string, string> dic) where T : new()
        {
            Type t = typeof(T);
            T entity = new T();
            var fields = t.GetProperties();

            string val = string.Empty;
            object obj = null;
            foreach (var field in fields)
            {
                if (!dic.Keys.Contains(field.Name))
                {
                    continue;
                }
                val = dic[field.Name];
                //非泛型
                if (!field.PropertyType.IsGenericType)
                {
                    obj = string.IsNullOrEmpty(val) ? null : Convert.ChangeType(val, field.PropertyType);
                }
                else //泛型Nullable<>
                {
                    Type genericTypeDefinition = field.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        obj = string.IsNullOrEmpty(val)
                                  ? null
                                  : Convert.ChangeType(val, Nullable.GetUnderlyingType(field.PropertyType));
                    }
                }
                field.SetValue(entity, obj, null);
            }


            return entity;
        }

        /// <summary>
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static Dictionary<string, object> ToMap(object o)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();
                if (mi != null && mi.IsPublic)
                {
                    //map.Add(p.Name, mi.Invoke(o, new object[] { }));

                }

                map.Add(p.Name, p.GetValue(o));
            }

            return map;

        }

        /// <summary>
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ToMap<T>(T obj)
        {
            Type t = typeof(T);
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return pi.ToDictionary(p => p.Name, p => p.GetValue(obj).ToString());

        }

        public static async Task<string> Get(string url, Dictionary<string, string> parameters)
        {
            // 设置HttpClientHandler的AutomaticDecompression
            ////var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };

            url = url + "?v=1.0" + GteParameters(parameters);

            Uri uri;
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);

            // 创建HttpClient（注意传入HttpClientHandler）
            using (var http = new HttpClient())
            {
                // await异步等待回应
                var response = await http.GetAsync(uri);

                // await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                var rel = await response.Content.ReadAsStringAsync();

                // 确保HTTP成功状态值
                response.EnsureSuccessStatusCode();

                return rel;
            }
        }

        public static string GetString<T>(string url, T t)
        {
            var map = ToMap(t);
            Task<string> result = Get(url, map);
            return result.Result;
        }

        public static V GetModel<T, V>(string url, T t)
        {
            var map = ToMap(t);
            var rel = GetString(url, map);
            return rel.DeserializeObject<V>();
        }

        public static async Task<string> Post(string url, Dictionary<string, string> parameters)
        {

            // 设置HttpClientHandler的AutomaticDecompression
            ////var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };

            Uri uri;
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);

            // 创建HttpClient（注意传入HttpClientHandler）
            using (var http = new HttpClient())
            {
                ////设置要的数据格式
                ////http.DefaultRequestHeaders.Add("Accept", "application/xml");
                http.DefaultRequestHeaders.Add("Accept", "application/json");

                // 使用FormUrlEncodedContent做HttpContent
                var content = new FormUrlEncodedContent(parameters);

                // await异步等待回应
                var response = await http.PostAsync(uri, content);

                // await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                var rel = await response.Content.ReadAsStringAsync();

                // 确保HTTP成功状态值
                response.EnsureSuccessStatusCode();

                return rel;
            }
        }

        public static string PostString<T>(string url, T t)
        {
            var map = ToMap(t);
            Task<string> result = Post(url, map);
            return result.Result;
        }

        public static V PostModel<T, V>(string url, T t)
        {
            var map = ToMap(t);
            var rel = PostString(url, map);
            return rel.DeserializeObject<V>();
        }

        /// <summary>
        /// GteParameters
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>string</returns>
        private static string GteParameters(Dictionary<string, string> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return string.Empty;
            }

            return parameters.Join("&", p => p.Key + "=" + p.Value);
        }
    }
}