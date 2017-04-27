namespace TcpService.Model
{
    /// <summary>
    /// 提醒信息
    /// </summary>
    public class RemindInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomNO { get; set; }
    }
}