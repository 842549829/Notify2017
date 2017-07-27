using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Notify.Code.Features;

namespace Notify.Code.Extension
{
    public static class IEnumerableExtension
    {
        #region Join
        /// <summary>
        /// 根据字符串拆分数组
        /// </summary>
        /// <param name="source">
        /// 要拆分的数组
        /// </param>
        /// <param name="separator">
        /// 拆分符
        /// </param>
        /// <returns>
        /// 字符串
        /// </returns>
        public static string Join(this IEnumerable<string> source, string separator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            return source.Aggregate((x, y) => x + separator + y);
        }

        /// <summary>
        /// 根据字符串拆分数组
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <param name="soucre"> 要拆分的数组</param>
        /// <param name="separator">拆分符</param>
        /// <param name="map">拆分条件</param>
        /// <returns>字符串 <see cref="string"/></returns>
        public static string Join<TSource>(this IEnumerable<TSource> soucre, string separator, Func<TSource, string> map)
        {
            if (soucre == null)
            {
                throw new ArgumentNullException(nameof(soucre));
            }

            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var enumerable = soucre as TSource[] ?? soucre.ToArray();
            return Join(enumerable.Select(map), separator);
        }
        #endregion

        #region Sort
        /// <summary>
        /// 多条件排序扩展方法
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="sources">sources</param>
        /// <param name="keySelector">keySelector</param>
        /// <returns>排序结果</returns>
        public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> sources, params KeyValuePair<bool, Func<TSource, object>>[] keySelector)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            IOrderedEnumerable<TSource> orderBys = null;
            int i = 0;
            foreach (var func in keySelector)
            {
                if (i == 0)
                {
                    orderBys = func.Key ? sources.OrderBy(func.Value) : sources.OrderByDescending(func.Value);
                }
                else
                {
                    if (orderBys != null)
                    {
                        orderBys = func.Key ? orderBys.ThenBy(func.Value) : orderBys.ThenByDescending(func.Value);
                    }
                }

                i++;
            }

            return orderBys;
        }

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="sources">数据</param>
        /// <param name="predicate">条件</param>
        /// <param name="keySelector">排序条件</param>
        /// <returns>最终数据</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate, Func<TSource, TKey> keySelector)
        {
            return sources.Where(predicate).OrderBy(keySelector);
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="sources">数据</param>
        /// <param name="predicate">条件</param>
        /// <param name="keySelector">排序条件</param>
        /// <returns>最终数据</returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate, Func<TSource, TKey> keySelector)
        {
            return sources.Where(predicate).OrderByDescending(keySelector);
        }

        /// <summary>
        /// 扩展Linq的OrderBy方法，实现根据属性和顺序(倒序)进行排序，调用和linq的方法一致
        /// </summary>
        /// <typeparam name="TEntity">需要排序的实体对象</typeparam>
        /// <param name="source">结果集信息</param>
        /// <param name="propertyStr">动态排序的属性名(从前台获取)</param>
        /// <param name="isDesc">排序方式，不传递表示顺序，默认true，false表示倒序</param>
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyStr,
            bool isDesc = true) where TEntity : class
        {
            //以下四句用来建立c>c.propertyStr的Expression对象，实现Lambda表达式的状态
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "c");
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(propertyStr);
            Expression expression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
            LambdaExpression lambdaExpression = Expression.Lambda(expression, parameterExpression);
            Type type = typeof(TEntity);

            //读取排序的顺序信息，如果传递的参数(isDesc)是true，则为顺序排序，否则为倒序排序
            string ascDesc = isDesc ? "OrderBy" : "OrderByDescending";

            //Expression.Call跟上面的信息一样，这里采用重载的形式，上面的GetCurrentMethod结果也是ascDesc
            //Expression.Call方法会利用typeof(Queryable),ascDesc,new Type[]{type,property,PropertyType}三个参数
            //合成跟MethodInfo等同的消息
            MethodCallExpression methodCallExpression = Expression.Call(typeof(Queryable), ascDesc,
                new[] { type, propertyInfo.PropertyType }, source.Expression, Expression.Quote(lambdaExpression));

            //返回成功
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(methodCallExpression);

        }

        #endregion

        #region MaxElement
        /// <summary>
        ///  获取最大值的当前对象
        /// </summary>
        /// <typeparam name="TElement">TElement</typeparam>
        /// <typeparam name="TData">TData</typeparam>
        /// <param name="source">source</param>
        /// <param name="selector">selector</param>
        /// <returns>MaxValue</returns>
        public static TElement MaxElement<TElement, TData>(this IEnumerable<TElement> source, Func<TElement, TData> selector)
            where TData : IComparable<TData>
        {
            return ComparableElement(source, selector, true);
        }
        #endregion

        #region MinElement
        /// <summary>
        ///  获取最小值的当前对象
        /// </summary>
        /// <typeparam name="TElement">TElement</typeparam>
        /// <typeparam name="TData">TData</typeparam>
        /// <param name="source">source</param>
        /// <param name="selector">selector</param>
        /// <returns>MaxValue</returns>
        public static TElement MinElement<TElement, TData>(this IEnumerable<TElement> source, Func<TElement, TData> selector)
          where TData : IComparable<TData>
        {
            return ComparableElement(source, selector, false);
        }
        #endregion

        #region Max
        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最大值</returns>
        public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Max();
        }
        #endregion

        #region Min
        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>最小值</returns>
        public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Min();
        }
        #endregion

        #region Sum
        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum(result => result.GetValueOrDefault());
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <param name="selector">selector</param>
        /// <returns>和</returns>
        public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(predicate).Select(selector).Sum();
        }

        #endregion

        #region Repeat

        /// <summary>
        /// 是否存在重复
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <returns>结果</returns>
        public static bool Repeat<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Count(predicate) > 1;
        }

        /// <summary>
        /// 根据某个字段获取重复数据
        /// </summary>
        /// <typeparam name="TSource">数据类型</typeparam>
        /// <typeparam name="Tkey">字段类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="predicate">表达式</param>
        /// <returns>结果</returns>
        public static IEnumerable<TSource> Repeat<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> predicate)
        {
            return source.GroupBy(predicate).Where(item => item.Count() > 1).SelectMany(item => item);
        }

        /// <summary>
        /// 所有的数据是否都是重复的
        /// </summary>
        /// <typeparam name="TSource">数据类型</typeparam>
        /// <typeparam name="Tkey">字段类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="predicate">表达式</param>
        /// <returns>结果</returns>
        public static bool AllRepeat<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var enumerable = source as TSource[] ?? source.ToArray();
            var first = enumerable.GroupBy(predicate).FirstOrDefault(item => item.Count() > 1);
            return first != null && enumerable.Length == first.Count();
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <typeparam name="TSource">数据源</typeparam>
        /// <param name="first">第一个</param>
        /// <param name="second">第二个</param>
        /// <param name="comparer">比较器</param>
        /// <returns>结果</returns>
        public static bool Comparer<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            var enumerable = second as TSource[] ?? second.ToArray();
            return (from source in first from source1 in enumerable where comparer.Equals(source, source1) select source).Any();
        }

        #endregion

        #region DynamicExpression
        /// <summary>
        /// 获取动态表达式
        /// </summary>
        /// <typeparam name="TResult">表达式数据类型</typeparam>
        /// <typeparam name="TCondition">表达式条件类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="condtion">条件</param>
        /// <returns>表达式</returns>
        public static Expression<Func<TResult, bool>> GetDynamicExpression<TResult, TCondition>(this IEnumerable<TResult> data, TCondition condtion)
            where TResult : class
            where TCondition : class
        {
            Type tConditionType = typeof(TCondition);
            Type tResultType = typeof(TResult);

            Expression totalExpr = Expression.Constant(true);
            ParameterExpression param = Expression.Parameter(typeof(TResult), "n");
            foreach (PropertyInfo property in tConditionType.GetProperties())
            {
                string key = property.Name;
                object value = property.GetValue(condtion);
                if (value != null && value.ToString() != string.Empty)
                {
                    DynamicExpressionAttribute dynamicExpressionAttribute = CustomAttributeExtension<DynamicExpressionAttribute>.GetCustomAttributeValue(tConditionType, property);

                    //等式左边的值
                    string name = dynamicExpressionAttribute.Name ?? key;
                    Expression left = Expression.Property(param, tResultType.GetProperty(name));
                    //等式右边的值
                    Expression right = Expression.Constant(value);

                    Expression filter;
                    switch (dynamicExpressionAttribute.Operator)
                    {
                        case "!=":
                            filter = Expression.NotEqual(left, right);
                            break;
                        case ">":
                            filter = Expression.GreaterThan(left, right);
                            break;
                        case ">=":
                            filter = Expression.GreaterThanOrEqual(left, right);
                            break;
                        case "<":
                            filter = Expression.LessThan(left, right);
                            break;
                        case "<=":
                            filter = Expression.LessThanOrEqual(left, right);
                            break;
                        case "Contains":
                            filter = Expression.Call(Expression.Property(param, tResultType.GetProperty(name)), typeof(string).GetMethod("Contains", new [] { typeof(string) }), Expression.Constant(value));
                            break;
                        default:
                            filter = Expression.Equal(left, right);
                            break;
                    }
                    totalExpr = Expression.And(filter, totalExpr);
                }
            }
            var predicate = Expression.Lambda(totalExpr, param);
            //var dynamic = (Func<TResult, bool>)predicate.Compile();
            var dynamic = (Expression<Func<TResult, bool>>)predicate;
            return dynamic;
        }

        /// <summary>
        /// 获取动态表达式
        /// </summary>
        /// <typeparam name="TResult">表达式数据类型</typeparam>
        /// <typeparam name="TCondition">表达式条件类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="condtion">条件</param>
        /// <returns>表达式</returns>
        public static Func<TResult, bool> GetDynamicExpressionDelegate<TResult, TCondition>(this IEnumerable<TResult> data, TCondition condtion)
            where TResult : class
            where TCondition : class
        {
            Expression<Func<TResult, bool>> dynamic = GetDynamicExpression(data, condtion);
            return dynamic.Compile();
        }

        #region 测试

        public class TestClass
        {
            public static void Test()
            {
                var data = GetTestData();
                var codition = new Condition { Name = "X" };
                var dynamicExpression = GetDynamicExpression(data, codition);
                //var query1 = data.Where(dynamicExpression);
            }

            public static List<Data> GetTestData()
            {
                Data account = new Data
                {
                    AccountNO = "01",
                    Count = 1
                };

                Data account1 = new Data
                {
                    AccountNO = "02X",
                    Count = 2
                };

                Data account2 = new Data
                {
                    AccountNO = "03",
                    Count = 3
                };

                Data account3 = new Data
                {
                    AccountNO = "04",
                    Count = 4
                };

                Data account4 = new Data
                {
                    AccountNO = "05",
                    Count = 5
                };
                var rel = new List<Data>
                {
                    account,
                    account1,
                    account2,
                    account3,
                    account4
                };
                return rel;
            }
        }

        public class Condition : Notify.Code.Lambda.BaseEntity
        {
            [DynamicExpression(Name = "AccountNO", Operator = "Contains")]
            public string Name { get; set; }

            [DynamicExpression(Name = "Count", Operator = ">")]
            public int? Age { get; set; }
        }

        public class Data : Notify.Code.Lambda.BaseEntity
        {
            public string AccountNO { get; set; }

            public int Count { get; set; }
        }
        #endregion
        #endregion

        #region private
        /// <summary>
        ///  获取最大or最小值的当前对象
        /// </summary>
        /// <typeparam name="TElement">TElement</typeparam>
        /// <typeparam name="TData">TData</typeparam>
        /// <param name="source">source</param>
        /// <param name="selector">selector</param>
        /// <param name="isMax">最大还是最小</param>
        /// <returns>MaxValue</returns>
        private static TElement ComparableElement<TElement, TData>(IEnumerable<TElement> source, Func<TElement, TData> selector, bool isMax)
            where TData : IComparable<TData>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            bool firstElement = true;
            TElement result = default(TElement);
            TData maxValue = default(TData);
            foreach (TElement element in source)
            {
                var candidate = selector(element);
                if (!firstElement)
                {
                    if (isMax && candidate.CompareTo(maxValue) <= 0)
                    {
                        continue;
                    }

                    if (!isMax && candidate.CompareTo(maxValue) > 0)
                    {
                        continue;
                    }
                }

                firstElement = false;
                maxValue = candidate;
                result = element;
            }

            return result;
        }
        #endregion
    }
}
