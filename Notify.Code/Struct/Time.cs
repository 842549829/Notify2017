using System;

namespace Notify.Code.Struct
{
    /// <summary>
    /// 时间
    /// </summary>
    [Serializable]
    public struct Time : IComparable, IComparable<Time>
    {
        /// <summary>
        /// The m_hour.
        /// </summary>
        private readonly int m_hour;

        /// <summary>
        /// The m_minute.
        /// </summary>
        private readonly int m_minute;

        /// <summary>
        /// The max.
        /// </summary>
        private static readonly Time Max = new Time(23, 59);

        /// <summary>
        /// The min.
        /// </summary>
        public static readonly Time Min = new Time(0, 0);

        /// <summary>
        /// The now.
        /// </summary>
        public static readonly Time Now = new Time(DateTime.Now);

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> struct.
        /// </summary>
        /// <param name="hour">
        /// The hour.
        /// </param>
        /// <param name="minute">
        /// The minute.
        /// </param>
        public Time(int hour, int minute)
        {
            if (0 <= hour && hour <= 23)
            {
                this.m_hour = hour;
            }
            else
            {
                throw new FormatException("小时格式必须为0-23");
            }
            if (0 <= minute && minute <= 59)
            {
                this.m_minute = minute;
            }
            else
            {
                throw new FormatException("分钟必须为0-59");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public Time(DateTime value)
        {
            this.m_hour = value.Hour;
            this.m_minute = value.Minute;
        }

        /// <summary>
        /// Gets the hour.
        /// </summary>
        public int Hour
        {
            get { return this.m_hour; }
        }

        /// <summary>
        /// Gets the minute.
        /// </summary>
        public int Minute
        {
            get { return this.m_minute; }
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="day">
        /// The day.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public DateTime ToDateTime(int year, int month, int day)
        {
            return new DateTime(year, month, day, this.m_hour, this.m_minute, 0);
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public DateTime ToDateTime(DateTime date)
        {
            return date.Date.AddHours(this.m_hour).AddMinutes(this.m_minute);
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public DateTime ToDateTime()
        {
            return DateTime.Today.AddHours(this.m_hour).AddMinutes(this.m_minute);
        }

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Time"/>.
        /// </returns>
        public static Time Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !(value.Length == 4 || value.Length == 3))
            {
                throw new FormatException("参数格式必须为HHmm");
            }
            int hour;
            int minute;
            if (!int.TryParse(value.Substring(0, value.Length - 2), out hour))
            {
                throw new FormatException("小时格式必须为0-12");
            }
            if (!int.TryParse(value.Substring(2), out minute))
            {
                throw new FormatException("分钟必须为00-59");
            }
            return new Time(hour, minute);
        }

        /// <summary>
        /// The try parse.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParse(string value, out Time result)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Length == 3 || value.Length == 4)
                {
                    int hour;
                    if (int.TryParse(value.Substring(0, value.Length - 2), out hour) && 0 <= hour && hour <= 12)
                    {
                        int minute;
                        if (int.TryParse(value.Substring(2), out minute) && 0 <= minute && minute <= 59)
                        {
                            result = new Time(hour, minute);
                            return true;
                        }
                    }
                }
            }
            result = new Time(0, 0);
            return false;
        }

        /// <summary>
        /// The ==.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public static bool operator ==(Time left, Time right)
        {
            return left.Hour == right.Hour && left.Minute == right.Minute;
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public static bool operator !=(Time left, Time right)
        {
            return !(left == right);
        }

        /// <summary>
        /// The &gt;.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public static bool operator >(Time left, Time right)
        {
            if (left.Hour > right.Hour)
            {
                return true;
            }
            if (left.Hour == right.Hour && left.Minute > right.Minute)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// The &gt;=.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public static bool operator >=(Time left, Time right)
        {
            return left == right || left > right;
        }

        /// <summary>
        /// The &lt;.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public static bool operator <(Time left, Time right)
        {
            return right > left;
        }

        /// <summary>
        /// The &lt;=.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public static bool operator <=(Time left, Time right)
        {
            return right >= left;
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
            if (obj != null && obj is Time)
            {
                return this == (Time)obj;
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
            return (this.m_hour | this.m_minute).GetHashCode();
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return this.m_hour.ToString("00") + ":" + this.m_minute.ToString("00");
        }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj != null && obj is Time)
            {
                return this.CompareTo((Time)obj);
            }
            return 1;
        }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(Time other)
        {
            return this.m_hour == other.m_hour ? this.m_minute - other.m_minute : this.m_hour - other.m_hour;
        }
    }
}
