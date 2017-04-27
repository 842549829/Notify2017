using System;
using System.ComponentModel;
using System.Reflection;

namespace Notify.Code.Extension
{
    /// <summary>
    /// The enum extension.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>Description</returns>
        public static string GetDescription(this System.Enum value)
        {
            string strValue = value.ToString();
            Type type = value.GetType();
            FieldInfo fieldinfo = type.GetField(strValue);
            object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0)
            {
                return strValue;
            }

            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }
    }
}
