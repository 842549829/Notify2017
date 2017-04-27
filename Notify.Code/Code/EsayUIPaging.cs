namespace Notify.Code.Code
{
    /// <summary>
    /// EsayUi分页控件分页信息
    /// </summary>
    public class EsayUIPaging : Paging
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        /// 页码
        /// </summary>
        public override int PageIndex
        {
            get
            {
                return this.page;
            }

            set
            {
                this.page = value;
            }
        }

        /// <summary>
        /// 页大小
        /// </summary>
        public int rows { get; set; } = 10;

        /// <summary>
        /// 页大小
        /// </summary>
        public override int PageSize
        {
            get
            {
                return this.rows;
            }

            set
            {
                this.rows = value;
            }
        }
    }
}