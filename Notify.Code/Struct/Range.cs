using System;
using System.Runtime.Serialization;

namespace Notify.Code.Struct
{
    /// <summary>
    /// 区间
    /// </summary>
    /// <typeparam name="T">区间类型</typeparam>
    [Serializable]
    [DataContract]
    public struct Range<T>
    {
        /// <summary>
        /// The _lower.
        /// </summary>
        private T lower;

        /// <summary>
        /// The _upper.
        /// </summary>
        private T upper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        /// <param name="lower">
        /// The lower.
        /// </param>
        /// <param name="upper">
        /// The upper.
        /// </param>
        public Range(T lower, T upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        /// <summary>
        /// 下限(时间开始)
        /// </summary>
        [DataMember]
        public T Lower
        {
            get { return this.lower; }
            set { this.lower = value; }
        }

        /// <summary>
        /// 上限(时间结束)
        /// </summary>
        [DataMember]
        public T Upper
        {
            get { return this.upper; }
            set { this.upper = value; }
        }
    }
}
