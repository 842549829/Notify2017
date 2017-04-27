namespace Notify.Code.Extension
{
    /// <summary>
    /// NumberExtension
    /// </summary>	
    public static class NumberExtension
    {
        #region byte
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool byteerval(this byte number, byte right, byte left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool byteervalByEqual(this byte number, byte right, byte left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool byteervalByRightEqual(this byte number, byte right, byte left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool byteervalByLeftEqual(this byte number, byte right, byte left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region sbyte
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool sbyteerval(this sbyte number, sbyte right, sbyte left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool sbyteervalByEqual(this sbyte number, sbyte right, sbyte left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool sbyteervalByRightEqual(this sbyte number, sbyte right, sbyte left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool sbyteervalByLeftEqual(this sbyte number, sbyte right, sbyte left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region ushort
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ushorterval(this ushort number, ushort right, ushort left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ushortervalByEqual(this ushort number, ushort right, ushort left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ushortervalByRightEqual(this ushort number, ushort right, ushort left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ushortervalByLeftEqual(this ushort number, ushort right, ushort left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region short
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool shorterval(this short number, short right, short left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool shortervalByEqual(this short number, short right, short left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool shortervalByRightEqual(this short number, short right, short left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool shortervalByLeftEqual(this short number, short right, short left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region uint
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool uinterval(this uint number, uint right, uint left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool uintervalByEqual(this uint number, uint right, uint left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool uintervalByRightEqual(this uint number, uint right, uint left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool uintervalByLeftEqual(this uint number, uint right, uint left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region int
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool Interval(this int number, int right, int left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByEqual(this int number, int right, int left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByRightEqual(this int number, int right, int left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByLeftEqual(this int number, int right, int left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region ulong
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ulongerval(this ulong number, ulong right, ulong left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ulongervalByEqual(this ulong number, ulong right, ulong left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ulongervalByRightEqual(this ulong number, ulong right, ulong left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool ulongervalByLeftEqual(this ulong number, ulong right, ulong left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region long
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool longerval(this long number, long right, long left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool longervalByEqual(this long number, long right, long left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool longervalByRightEqual(this long number, long right, long left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool longervalByLeftEqual(this long number, long right, long left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region float
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool Interval(this float number, float right, float left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByEqual(this float number, float right, float left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByRightEqual(this float number, float right, float left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByLeftEqual(this float number, float right, float left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region double
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool Interval(this double number, double right, double left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByEqual(this double number, double right, double left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByRightEqual(this double number, double right, double left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByLeftEqual(this double number, double right, double left)
        {
            return number > right && number <= left;
        }
        #endregion

        #region decimal
        /// <summary>
        /// 大于,小于 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool Interval(this decimal number, decimal right, decimal left)
        {
            return number > right && number < left;
        }

        /// <summary>
        /// 大于等于,小于等于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByEqual(this decimal number, decimal right, decimal left)
        {
            return number >= right && number <= left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByRightEqual(this decimal number, decimal right, decimal left)
        {
            return number >= right && number < left;
        }

        /// <summary>
        /// 大于等于,小于
        /// </summary>
        /// <param name="number"></param>
        /// <param name="right">左区间，意味大于</param>
        /// <param name="left">右区间，意味小于</param>
        /// <returns></returns>
        public static bool IntervalByLeftEqual(this decimal number, decimal right, decimal left)
        {
            return number > right && number <= left;
        } 
        #endregion
    }
}