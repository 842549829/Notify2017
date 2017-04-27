using Notify.Code.Code;

namespace Notify.Model.Transfer
{
    /// <summary>
    /// 用户列表
    /// </summary>
    public class TAccountCondition : EsayUIPaging
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string AccountName { get; set; }
    }
}