using System;
using System.Linq.Expressions;
using System.Text;

namespace Notify.Code.Lambda
{
    // 测试
    //static class Program
    //{
    //    public static void Main()
    //    {
    //        UserId uid = new UserId();
    //        uid.Where(userId => (userId.Id == "8" && userId.LoginCount > 5) || userId.Pws != null || userId.Id.Like("%aa") && userId.LoginCount.In(new int?[] { 4, 6, 8, 9 }) && userId.Id.NotIn(new string[] { "a", "b", "c", "d" }));
    //    }
    //}

    public class BaseEntity
    {
        public string WhereStr { get; set; }
    }

    public class UserId : BaseEntity
    {
        public string Id { get; set; }

        public string Pws { get; set; }

        public int? LoginCount { get; set; }
    }

    public static class ExFunc
    {
        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool NotIn<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }
        public static void Where<T>(this T entity, Expression<Func<T, bool>> func) where T : BaseEntity
        {
            if (func.Body is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)func.Body);
                entity.WhereStr = BinarExpressionProvider(be.Left, be.Right, be.NodeType);
            }
            else
                entity.WhereStr = string.Empty;
        }

        static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type)
        {
            string sb = "(";
            //先处理左边
            sb += ExpressionRouter(left);

            sb += ExpressionTypeCast(type);

            //再处理右边
            string tmpStr = ExpressionRouter(right);
            if (tmpStr.ToUpper() == "NULL")
            {
                if (sb.EndsWith(" ="))
                    sb = sb.Substring(0, sb.Length - 2) + " IS NULL";
                else if (sb.EndsWith("<>"))
                    sb = sb.Substring(0, sb.Length - 2) + " IS NOT NULL";
            }
            else
                sb += tmpStr;
            return sb += ")";
        }

        static string ExpressionRouter(Expression exp)
        {
            string sb = string.Empty;
            if (exp is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)exp);
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression me = ((MemberExpression)exp);
                return me.Member.Name;
            }
            else if (exp is NewArrayExpression)
            {
                NewArrayExpression ae = ((NewArrayExpression)exp);
                StringBuilder tmpstr = new StringBuilder();
                foreach (Expression ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(ex));
                    tmpstr.Append(",");
                }
                return tmpstr.ToString(0, tmpstr.Length - 1);
            }
            else if (exp is MethodCallExpression)
            {
                MethodCallExpression mce = (MethodCallExpression)exp;
                if (mce.Method.Name.ToUpper() == "LIKE")
                    return string.Format("({0} LIKE {1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                else if (mce.Method.Name.ToUpper() == "NOTLIKE")
                    return string.Format("({0} NOT LIKE {1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                else if (mce.Method.Name.ToUpper() == "IN")
                    return string.Format("{0} IN ({1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                else if (mce.Method.Name.ToUpper() == "NOTIN")
                    return string.Format("{0} NOT In ({1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));

            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression ce = ((ConstantExpression)exp);
                if (ce.Value == null)
                    return "NULL";
                else if (ce.Value is ValueType)
                    return ce.Value.ToString();
                else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                    return string.Format("'{0}'", ce.Value.ToString());
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);
                return ExpressionRouter(ue.Operand);
            }
            return null;
        }

        static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return " =";
                case ExpressionType.GreaterThan:
                    return " >";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    return null;
            }
        }
    }
}