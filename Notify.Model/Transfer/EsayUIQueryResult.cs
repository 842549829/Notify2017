using System.Collections.Generic;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// EsayUI框架分页查询结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class EsayUIQueryResult<T>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> rows { get; set; }
    }
}