using System;

namespace Notify.Code.Extension
{
    /// <summary>
    /// 数字计算
    /// </summary>
    public static class MathExtension
    {
        /// <summary>
        /// 全入到分
        /// </summary>
        /// <param name="decimal">数字</param>
        /// <returns>结果</returns>
        public static decimal AllInPenny(this decimal @decimal)
        {
            return Math.Ceiling(@decimal * 100) / 100;
        }

        /// <summary>
        /// 全舍到分
        /// </summary>
        /// <param name="decimal">数字</param>
        /// <returns>结果</returns>
        public static decimal AllAbandonPenny(this decimal @decimal)
        {
            return Math.Floor(@decimal * 100) / 100;
        }

        /// <summary>
        /// 四舍五入到分(银行家算法)
        /// </summary>
        /// <param name="decimal">数字</param>
        /// <returns>结果</returns>
        public static decimal Rounding(this decimal @decimal)
        {
            return Math.Round(@decimal, 2);
        }

        /// <summary>
        /// 四舍五入到分(中国式四舍五入)
        /// </summary>
        /// <param name="decimal">数字</param>
        /// <returns>结果</returns>
        public static decimal RoundingCn(this decimal @decimal)
        {
            return Math.Round(@decimal, 2, MidpointRounding.AwayFromZero);
        }
    }
}