using System;

namespace Notify.Code.Features
{
    /// <summary>
    /// 动态表达式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class DynamicExpressionAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 运行符号
        /// </summary>
        public string Operator { get; set; }
    }
}