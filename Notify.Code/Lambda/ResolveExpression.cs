using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Notify.Code.Lambda
{
    public class ResolveExpression
    {
        public Dictionary<string, object> NewMember = new Dictionary<string, object>();

        /// <summary>
        /// SQL语句中的Dapper参数
        /// </summary>
        public Dictionary<string, object> parameters = new Dictionary<string, object>();

        public string SqlWhere = string.Empty;

        public virtual string ProcessExpression(Expression expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }

            //主入口
            if (expression is LambdaExpression)
            {
                return VisitLambdaExpression(expression as LambdaExpression);
            }
            else if (expression is BinaryExpression)
            {
                return VisitBinaryExpression(expression as BinaryExpression);
            }
            else if (expression is BlockExpression)
            {
                return VisitBlockExpression(expression as BlockExpression);
            }
            else if (expression is ConditionalExpression)
            {
                return VisitConditionalExpression(expression as ConditionalExpression);
            }
            else if (expression is ConstantExpression)
            {
                return VisitConstantExpression(expression as ConstantExpression);
            }

            else if (expression is ListInitExpression)
            {
                return VisitListInitExpression(expression as ListInitExpression);
            }

            else if (expression is MemberExpression)
            {
                return VisitMemberExpression(expression as MemberExpression);
            }
            else if (expression is MemberInitExpression)
            {
                return VisitMemberInit(expression as MemberInitExpression);
            }
            else if (expression is MethodCallExpression)
            {
                return VisitMethodCallExpression(expression as MethodCallExpression);
            }
            else if (expression is NewArrayExpression)
            {
                return VisitNewArrayExpression(expression as NewArrayExpression);
            }
            else if (expression is NewExpression)
            {
                return VisitNewExpression(expression as NewExpression);
            }

            else if (expression is UnaryExpression)
            {
                return VisitUnaryExpression(expression as UnaryExpression);
            }


            return null;
        }

        public virtual string VisitLambdaExpression(LambdaExpression exp)
        {
            var expression = exp.Body;
            return ProcessExpression(expression);
        }

        public virtual string VisitBinaryExpression(BinaryExpression exp)
        {
            var left = ProcessExpression(exp.Left);
            var right = ProcessExpression(exp.Right);
            var operate = GetOperator(exp.NodeType);
            // 
            var IsCreateParme =
            exp.NodeType != ExpressionType.And &&
            exp.NodeType != ExpressionType.AndAlso &&
            exp.NodeType != ExpressionType.Or &&
            exp.NodeType != ExpressionType.OrElse;
            if (IsCreateParme)
            {
                //构造参数形式
                var parmeName = AddParameter(left, right);
                string SQL = string.Format("( [{0}] {1} {2})", left, operate, parmeName);
                return SQL;

            }
            else
            {
                //构造参数形式
                string SQL = string.Format(" {0} {1} {2} ", left, operate, right);
                return SQL;
            }
        }

        public virtual string VisitBlockExpression(BlockExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitConditionalExpression(ConditionalExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitConstantExpression(ConstantExpression exp)
        {
            var Value = exp.Value.ToString();
            return Value;
        }

        public virtual string VisitDynamicExpression(DynamicExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitIndexExpression(IndexExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitInvocationExpression(InvocationExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitListInitExpression(ListInitExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitMemberExpression(MemberExpression exp)
        {
            object NameOrValue = "";
            if (exp.Expression == null || exp.Expression.NodeType != ExpressionType.Parameter)
            {
                GetMemberValue(exp, ref NameOrValue);
                if (NameOrValue == null) return null;
                else return NameOrValue.ToString();
            }
            else
            {
                NameOrValue = exp.Member.Name;
            }
            return NameOrValue.ToString();
        }

        private void GetMemberValue(MemberExpression me, ref object value)
        {
            var conExp = me.Expression as ConstantExpression;
            var fieldInfo = me.Member as System.Reflection.FieldInfo;
            //简单变量
            if (conExp != null && fieldInfo != null)
            {
                value = (fieldInfo).GetValue((me.Expression as ConstantExpression).Value);
                if (fieldInfo.FieldType.IsEnum)
                {
                    value = Convert.ToInt64(System.Enum.ToObject(fieldInfo.FieldType, value));
                }
            }
            else
            {
                //类中属性或字段
                var memberInfos = new Stack<MemberInfo>();
                Expression exp = me;
                while (exp is MemberExpression)
                {
                    var memberExpr = exp as MemberExpression;
                    memberInfos.Push(memberExpr.Member);
                    if (memberExpr.Expression != null)
                    {

                    }
                    exp = memberExpr.Expression;
                }
                var constExpr = exp as ConstantExpression;
                if (constExpr != null)
                {
                    var objReference = constExpr.Value;

                    // "ascend" back whence we came from and resolve object references along the way:
                    while (memberInfos.Count > 0)  // or some other break condition
                    {
                        var mi = memberInfos.Pop();
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            var objProp = objReference.GetType().GetProperty(mi.Name);
                            if (objProp == null)
                            {
                                return;
                            }
                            objReference = objProp.GetValue(objReference, null);
                        }
                        else if (mi.MemberType == MemberTypes.Field)
                        {
                            var objField = objReference.GetType().GetField(mi.Name);
                            if (objField == null)
                            {
                                return;
                            }
                            objReference = objField.GetValue(objReference);
                        }
                    }
                    value = objReference;
                }
            }
        }

        private bool UnderNodeTypeIsConstantExpression(MemberExpression exp)
        {
            while (exp.Expression != null)
            {
                if (exp != null && exp.Expression != null)
                {
                    if (exp.Expression is MemberExpression)
                    {
                        exp = exp.Expression as MemberExpression;
                    }
                    else
                    {

                        break;
                    }
                }
            }
            return exp.Expression is ConstantExpression;
        }

        public virtual string VisitMethodCallExpression(MethodCallExpression exp)
        {
            string methodName = exp.Method.Name;
            if (methodName.Equals("Equals"))
            {
                return this.Equals(exp);
            }
            if (methodName.Equals("Contains"))
            {
                return this.Contains(exp);
            }
            if (methodName.Equals("EndsWith"))
            {
                return this.Contains(exp);
            }
            if (methodName.Equals("StartsWith"))
            {
                return this.Contains(exp);
            }
            if (exp.Method.ReflectedType.Name.Equals("Convert"))
            {
                return this.ConvertToObject(exp).ToString();
            }
            else
            {
                return this.ParMethodTo(exp);
            }
        }

        public virtual string VisitNewArrayExpression(NewArrayExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitNewExpression(NewExpression exp)
        {
            string result = "";
            foreach (var member in exp.Members)
            {
                result += member.Name + ",";
            }
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        public virtual string VisitParameterExpression(ParameterExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitTypeBinaryExpression(TypeBinaryExpression exp)
        {
            throw new System.Exception();
        }

        public virtual string VisitUnaryExpression(UnaryExpression exp)
        {
            return ProcessExpression((Expression)exp.Operand);
        }

        /// <summary>
        /// 解析 new T(){ Name="saff"}的形式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual string VisitMemberInit(MemberInitExpression exp)
        {
            var BuidMember = exp.Bindings;
            StringBuilder strBuild = new StringBuilder();
            foreach (var item in BuidMember)
            {
                var MemberAssignment = item as System.Linq.Expressions.MemberAssignment;
                var value = ProcessExpression(MemberAssignment.Expression);
                //构造参数形式
                var parmeName = AddParameter(item.Member.Name, value);
                NewMember.Add(parmeName, item.Member.Name);
                strBuild.AppendFormat("[{0}] = {1} ,", item.Member.Name, parmeName);
            }
            strBuild.Remove(strBuild.Length - 1, 1);
            return strBuild.ToString();
        }

        public virtual Expression VisitNew(NewExpression exp)
        {
            return exp;
        }

        /// <summary>
        /// 根据条件生成对应的sql查询操作符
        /// </summary>
        /// <param name="expressiontype"></param>
        /// <returns></returns>
        private string GetOperator(ExpressionType expressiontype)
        {
            switch (expressiontype)
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
                    throw new System.Exception("未找到对应查询操作符号");
            }
        }

        /// <summary>
        /// 向SQL当中添加参数
        /// </summary>
        /// <param name="parameterValue">参数的值,Value</param>
        /// <returns>参数名</returns>
        private string AddParameter(string Name, object parameterValue)
        {
            string time = DateTime.Now.Ticks.ToString();
            parameters.Add(string.Format("{0}{1}", Name, time), parameterValue);
            return string.Format("@{0}{1}", Name, time);
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="mce"></param>
        /// <returns></returns>
        private string Equals(MethodCallExpression mce)
        {
            var left = ProcessExpression(mce.Object);
            var right = ProcessExpression(mce.Arguments.FirstOrDefault());
            var parmeName = AddParameter(left, right);

            return string.Format("([{0}] = {1} )", left, parmeName);
        }

        private string Contains(MethodCallExpression mce)
        {
            var left = ProcessExpression(mce.Object);
            var right = ProcessExpression(mce.Arguments.FirstOrDefault());
            var parmeName = AddParameter(left, string.Format("%{0}%", right));
            return string.Format("([{0}] LIKE {1} )", left, parmeName);
        }

        private string StartsWith(MethodCallExpression mce)
        {
            var left = ProcessExpression(mce.Object);
            var right = ProcessExpression(mce.Arguments.FirstOrDefault());
            var parmeName = AddParameter(left, string.Format("%{0}", right));
            return string.Format("([{0}] LIKE {1} )", left, parmeName);
        }

        private string EndsWith(MethodCallExpression mce)
        {
            var left = ProcessExpression(mce.Object);
            var right = ProcessExpression(mce.Arguments.FirstOrDefault());
            var parmeName = AddParameter(left, string.Format("{0}%", right));
            return string.Format("([{0}] LIKE {1} )", left, parmeName);
        }

        private object ConvertToObject(MethodCallExpression mce)
        {
            string right = ProcessExpression(mce.Arguments.FirstOrDefault());
            object parameterName = "";

            switch (mce.Method.ReturnType.Name)
            {
                case "DateTime":
                    parameterName = Convert.ToDateTime(right);
                    break;
                case "String":
                    parameterName = right;
                    break;
                case "Int16":
                    parameterName = Convert.ToInt16(right);
                    break;
                case "Int32":
                    parameterName = Convert.ToInt32(right);
                    break;
                case "Int64":
                    parameterName = Convert.ToInt64(right);
                    break;
                case "Single":
                    parameterName = Convert.ToSingle(right);
                    break;
                case "UInt16":
                    parameterName = Convert.ToUInt16(right);
                    break;
                case "UInt32":
                    parameterName = Convert.ToUInt32(right);
                    break;
                case "UInt64":
                    parameterName = Convert.ToUInt64(right);
                    break;
                case "SByte":
                    parameterName = Convert.ToSByte(right);
                    break;
                case "Double":
                    parameterName = Convert.ToDouble(right);
                    break;
                case "Decimal":
                    parameterName = Convert.ToDecimal(right);
                    break;
                case "Char":
                    parameterName = Convert.ToChar(right);
                    break;
                case "Byte":
                    parameterName = Convert.ToByte(right);
                    break;
                case "Boolean":
                    parameterName = Convert.ToBoolean(right);
                    break;
                default:
                    parameterName = right;
                    break;
            }
            return parameterName;
            // var parmeName = AddParameter(left, string.Format("{0}%", right));
            //  return string.Format("( [{0}]  like {1} )", left, parmeName);
        }

        private string ParMethodTo(MethodCallExpression mce)
        {
            object parameterName = "";
            string right = ProcessExpression(mce.Object);

            switch (mce.Method.ReturnType.Name)
            {
                case "DateTime":
                    parameterName = Convert.ToDateTime(right);
                    break;
                case "String":
                    parameterName = right;
                    break;
                case "Int16":
                    parameterName = Convert.ToInt16(right);
                    break;
                case "Int32":
                    parameterName = Convert.ToInt32(right);
                    break;
                case "Int64":
                    parameterName = Convert.ToInt64(right);
                    break;
                case "Single":
                    parameterName = Convert.ToSingle(right);
                    break;
                case "UInt16":
                    parameterName = Convert.ToUInt16(right);
                    break;
                case "UInt32":
                    parameterName = Convert.ToUInt32(right);
                    break;
                case "UInt64":
                    parameterName = Convert.ToUInt64(right);
                    break;
                case "SByte":
                    parameterName = Convert.ToSByte(right);
                    break;
                case "Double":
                    parameterName = Convert.ToDouble(right);
                    break;
                case "Decimal":
                    parameterName = Convert.ToDecimal(right);
                    break;
                case "Char":
                    parameterName = Convert.ToChar(right);
                    break;
                case "Byte":
                    parameterName = Convert.ToByte(right);
                    break;
                case "Boolean":
                    parameterName = Convert.ToBoolean(right);
                    break;
                default:
                    parameterName = right;
                    break;
            }
            return parameterName.ToString();
        }
    }
}
