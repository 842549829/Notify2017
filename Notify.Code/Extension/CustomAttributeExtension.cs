using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Notify.Code.Extension
{
    /// <summary>
    /// 获取实体对象的自定义特性
    /// </summary>
    /// <typeparam name="TAttribute">自定义特性类型</typeparam>
    public static class CustomAttributeExtension<TAttribute>
        where TAttribute : Attribute
    {
        /// <summary>
        /// Cache Data
        /// </summary>
        private static readonly Dictionary<string, TAttribute> Cache = new Dictionary<string, TAttribute>();

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <param name="type">实体对象类型</param>
        /// <param name="propertyInfo">实体对象属性信息</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static TAttribute GetCustomAttributeValue(Type type, PropertyInfo propertyInfo)
        {
            var key = BuildKey(type, propertyInfo);
            if (!Cache.ContainsKey(key))
            {
                CacheAttributeValue(type, propertyInfo);
            }
            return Cache[key];
        }

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <param name="sourceType">实体对象数据类型</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static TAttribute GetCustomAttributeValue(Type sourceType)
        {
            var key = BuildKey(sourceType, null);
            if (!Cache.ContainsKey(key))
            {
                CacheAttributeValue(sourceType, null);
            }
            return Cache[key];
        }

        /// <summary>
        /// 获取实体类上的特性
        /// </summary>
        /// <param name="type">实体对象类型</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        private static TAttribute GetClassAttributes(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
            return (TAttribute)attribute;
        }

        /// <summary>
        /// 获取实体属性上的特性
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        private static TAttribute GetPropertyAttributes(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo?.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
            return (TAttribute)attribute;
        }

        /// <summary>
        /// 缓存Attribute Value
        /// <param name="type">实体对象类型</param>
        /// <param name="propertyInfo">实体对象属性信息</param>
        /// </summary>
        private static void CacheAttributeValue(Type type, PropertyInfo propertyInfo)
        {
            var key = BuildKey(type, propertyInfo);
            TAttribute value;
            if (propertyInfo == null)
            {
                value = GetClassAttributes(type);
            }
            else
            {
                value = GetPropertyAttributes(propertyInfo);
            }

            lock (key + "_attributeValueLockKey")
            {
                if (!Cache.ContainsKey(key))
                {
                    Cache[key] = value;
                }
            }
        }

        /// <summary>
        /// 缓存Collection Name Key
        /// <param name="type">type</param>
        /// <param name="propertyInfo">propertyInfo</param>
        /// </summary>
        private static string BuildKey(Type type, PropertyInfo propertyInfo)
        {
            if (string.IsNullOrEmpty(propertyInfo?.Name))
            {
                return type.FullName;
            }
            return type.FullName + "." + propertyInfo.Name;
        }
    }

    #region 测试代码
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class TestAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    [Test(Name = "刘")]
    public class TestEntity
    {
        [Test(Name = "Name")]
        public string Name { get; set; }

        [Test(Name = "Age")]
        public int Age { get; set; }
    }

    public static class TestClass
    {
        public static TestAttribute Test()
        {
            TestAttribute testAttribute = CustomAttributeExtension<TestAttribute>.GetCustomAttributeValue(typeof(TestEntity));
            return testAttribute;
        }
    }
    #endregion
}