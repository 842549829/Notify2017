using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace Notify.Code.Code
{
    /// <summary>
    /// EntityFrameWork使用匿名对象查询，由于匿名对象的可访问性为internal,不能直接返回给View，所以扩展下面的方法实现
    /// </summary>
    public static class MvcDynamic
    {
        /// <summary>
        /// 将对象(主要是匿名对象)转换为View层可以访问的对象(dynamic)
        /// 使用：MVC控制器中构造：
        ///     var data=new{Id=1,Name="Kencery"};
        ///     dynamic result=data.ToDynamicInfo();
        ///     Viewbag.Result=result;
        ///       View端如何解析调用
        ///     dynamic result=Viewbag.Result;
        ///     @result.Id,@result.Name
        /// </summary>  
        /// <param name="value">需要转换的匿名对象</param>
        /// <returns></returns>
        public static dynamic ToDynamicInfo(this object value)
        {
            IDictionary<string, object> expandTo = new ExpandoObject(); //初始化对象
            Type type = value.GetType();

            PropertyDescriptorCollection propertys = TypeDescriptor.GetProperties(type); //获取集合对象

            //循环集合对象
            foreach (PropertyDescriptor property in propertys)
            {
                var val = property.GetValue(value);
                if (property.PropertyType.FullName.StartsWith("<>f__AnonymousType"))
                {
                    dynamic dval = val.ToDynamicInfo();
                    expandTo.Add(property.Name, dval);
                }
                else
                {
                    expandTo.Add(property.Name, val);
                }
            }
            return (ExpandoObject) expandTo;
        }
    }
}
