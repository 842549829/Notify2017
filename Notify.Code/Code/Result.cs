using System;

namespace Notify.Code.Code
{
    /// <summary>
    /// 结果
    /// </summary>
    [Serializable]
    public class Result
    {
        /// <summary>
        /// 成功/失败
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
