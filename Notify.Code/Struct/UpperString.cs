using System;

namespace Notify.Code.Struct
{
    /// <summary>
    /// The upper string.
    /// </summary>
    [Serializable]
    public struct UpperString
    {
        /// <summary>
        /// m_original
        /// </summary>
        private readonly string m_original;

        /// <summary>
        /// The value.
        /// </summary>
        private readonly string m_value;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpperString"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public UpperString(string value)
        {
            this.m_original = value;
            this.m_value = null == value ? null : value.Trim().ToUpper();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.m_value;
            }
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return this.m_value;
        }

        /// <summary>
        /// The op_ implicit.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator UpperString(string value)
        {
            return new UpperString(value);
        }

        /// <summary>
        /// The is null or empty.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsNullOrEmpty()
        {
            return string.IsNullOrEmpty(this.m_value);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                if (obj is UpperString)
                {
                    return ((UpperString)obj).Value == this.m_value;
                }
                string s = obj as string;
                if (s != null)
                {
                    return string.Compare(s, this.m_value, true) == 1;
                }
            }
            return false;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.m_value.GetHashCode();
        }
    }
}