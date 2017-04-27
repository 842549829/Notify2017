using System.Collections.Generic;
using Notify.Code.Code;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// 查询结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class QueryResult<T>
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public Paging Paging { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}