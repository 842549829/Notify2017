namespace Notify.Model.Transfer
{
    /// <summary>
    /// Ztree框架菜单
    /// </summary>
    public class ZtreeMenu
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 菜单父Id
        /// </summary>
        public string pId { get; set; }

        /// <summary>
        /// 是否打开
        /// </summary>
        public bool open { get; set; } = true;
    }
}