using System;

namespace Notify.Code.Code
{
    /// <summary>
    /// 操作信息
    /// </summary>
    public class Operational
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDateTime { get; set; }
    }
}