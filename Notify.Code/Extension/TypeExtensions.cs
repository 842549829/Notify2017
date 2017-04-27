using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Notify.Code.Struct;

namespace Notify.Code.Extension
{
    /// <summary>
    /// Type扩展
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 查询集合类型
        /// </summary>
        /// <param name="seqType">类型</param>
        /// <returns>类型</returns>
        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null)
            {
                return null;
            }
            if (seqType == typeof(string))
            {
                return typeof (IEnumerable<char>);
            }
            if (seqType.IsArray)
            {
                return typeof (IEnumerable<>).MakeGenericType(seqType.GetTypeOfElements());
            }
            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof (IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }
            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces.Length > 0)
            {
                foreach (Type ienum in ifaces.Select(FindIEnumerable).Where(ienum => ienum != null))
                {
                    return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }

        /// <summary>
        /// 获取Type
        /// </summary>
        /// <param name="seqType">类型</param>
        /// <returns>类型</returns>
        public static Type GetTypeOfElements(this Type seqType)
        {
            Type ienum = FindIEnumerable(seqType);
            return ienum == null ? seqType : ienum.GetGenericArguments()[0];
        }

        /// <summary>
        /// 获取当前类型对应的序列类型（例如：对于类型 T，GetSequenceType 方法将返回 IEnumerable&lt;T&gt;）
        /// </summary>
        /// <param name="elementType">type</param>
        /// <returns>type</returns>
        public static Type GetSequenceType(this Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        /// <summary>
        /// <para>获取与当前可为 null 类型对应的原始类型</para> <para>如果当前类型为引用类型，返回该类型本身，否则返回 Nullable&lt;T&gt; 的 T 类型</para> >
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>type</returns>
        public static Type GetNonNullableType(this Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }

        /// <summary>
        /// <para>获取当前类型对应的可赋值 null 的类型</para> <para>如果当前类型为引用类型，返回该类型本身，否则返回其对应的 Nullable&lt;T&gt; 类型</para>
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>type</returns>
        public static Type GetNullAssignableType(this Type type)
        {
            return !IsNullAssignable(type) ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

        /// <summary>
        /// 获取当前成员的类型
        /// </summary>
        /// <param name="mi">mi</param>
        /// <returns>Type</returns>
        public static Type GetMemberType(this MemberInfo mi)
        {
            if (mi == null)
            {
                throw new ArgumentNullException(nameof(mi));
            }
            switch (mi.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)mi).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)mi).PropertyType;
                case MemberTypes.Event:
                    return ((EventInfo)mi).EventHandlerType;
                case MemberTypes.Method:
                    return ((MethodInfo)mi).ReturnType;
                case MemberTypes.Constructor:
                    return mi.DeclaringType;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取一个值，用于表示当前类型的默认值
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>obj</returns>
        public static object DefaultValue(this Type type)
        {
            return !type.IsNullAssignable() ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 判断当前类型是否是 Nullable 类型
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>type</returns>
        public static bool IsNullableType(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 判断当前类型是否可以赋值 null
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>type</returns>
        public static bool IsNullAssignable(this Type type)
        {
            return !type.IsValueType || IsNullableType(type);
        }

        /// <summary>
        /// 判断当前成员是否只读
        /// </summary>
        /// <param name="member">MemberInfo</param>
        /// <returns>bool</returns>
        public static bool IsReadOnly(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return (((FieldInfo)member).Attributes & FieldAttributes.InitOnly) != 0;
                case MemberTypes.Property:
                    var pi = (PropertyInfo)member;
                    return !pi.CanWrite || pi.GetSetMethod() == null;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 判断当前类型是否是整数类型
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>bool</returns>
        public static bool IsInteger(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断当前类型是否是数值类型
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>bool</returns>
        public static bool IsNumeric(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断当前类型是否是简单类型
        /// </summary>
        /// <param name="type">被扩展的类型</param>
        /// <returns>如果</returns>
        public static bool IsSimpleType(this Type type)
        {
            if (type.IsEnum || type == typeof (Time) || type == typeof (Guid))
            {
                return true;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Boolean:
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.DBNull:
                    return true;
                default:
                    return false;
            }
        }
    }
}