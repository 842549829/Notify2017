using System;
using System.Web;
using System.Web.Caching;

namespace Notify.Code.Cache
{
    using System.Collections.Generic;

    /// <summary>
    /// 文件依赖缓存
    /// </summary>
    public class FileDependentCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDependentCache"/> class.
        /// </summary>
        /// <param name="filePath">
        /// 依赖文件路径
        /// </param>
        /// <param name="value">
        /// 缓存值
        /// </param>
        public FileDependentCache(Func<string> filePath, Func<object> value)
        {
            this.FilePath = filePath;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public Func<string> FilePath { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public Func<object> Value { get; set; }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public object this[string key]
        {
            get
            {
                var cache = HttpRuntime.Cache;
                object value = cache.Get(key);
                if (value != null)
                {
                    return value;
                }

                var filePath = this.FilePath();
                var val = this.Value();
                var cacheDependency = new CacheDependency(filePath);
                cache.Insert(key, val, cacheDependency);
                return val;
            }
        }
    }

    /// <summary>
    /// The config.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 获取GetStudent
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static List<string> GetStudent(string key)
        {
            FileDependentCache cache = new FileDependentCache(
                () => "~/Cache/Student.xml",
                () =>
                    {
                        // 返回缓存的对象
                        List<string> list = new List<string> { };
                        return list;
                    });
            return cache[key] as List<string>;
        }
    }
}