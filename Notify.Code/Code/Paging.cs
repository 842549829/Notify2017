using System;

namespace Notify.Code.Code
{
    /// <summary>
    /// 分页类
    /// </summary>
    [Serializable]
    public class Paging
    {
        /// <summary>
        /// 页码(默认第一页)
        /// </summary>
        public virtual int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页大小(默认10页)
        /// </summary>
        public virtual int PageSize { get; set; } = 10;

        /// <summary>
        /// 是否获取总条数(默认获取)
        /// </summary>
        public bool GetRowsCount { get; set; } = true;

        /// <summary>
        /// 总条数
        /// </summary>
        public int RowsCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        private int pageCount;

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                this.pageCount = (this.RowsCount % this.PageSize) == 0
                                     ? this.RowsCount / this.PageSize
                                     : (this.RowsCount / this.PageSize) + 1;
                return this.pageCount;
            }

            set
            {
                this.pageCount = value;
            }
        }

        /// <summary>
        /// 开始索引
        /// </summary>
        public int StratRows
        {
            get
            {
                if (this.PageIndex <= 0)
                {
                    return 0;
                }

                return this.PageSize * (this.PageIndex - 1);
            }
        }

        /// <summary>
        /// 分页类型复制
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>结果</returns>
        public static Paging Copy(Paging condition)
        {
            Paging paging = new Paging
            {
                GetRowsCount = condition.GetRowsCount,
                PageCount = condition.PageCount,
                PageIndex = condition.PageIndex,
                PageSize = condition.PageSize,
                RowsCount = condition.RowsCount
            };
            return paging;
        }
    }
}