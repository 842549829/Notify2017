using System;
using System.Collections.Generic;
using System.Reflection;
using Notify.Code.Utility;

namespace Notify.Mail
{
    public abstract class MailBase
    {
        protected static Dictionary<string, string> template;

        protected MailBase()
        {
            // Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs /ExceptionLog");
            string path = @"E:\MyCode\Notify\trunk\Notify.Mail\Template\";
            if (template == null)
            {
                template = FileUtility.GetFileContents(path);
            }
        }

        public abstract Type TemplateType { get; }

        public abstract object GetValue(PropertyInfo propertyInfo);

        public string GetTemplateContent()
        {
            var key = $"{TemplateType.Name}.html";
            if (template.ContainsKey(key))
            {
                var content = template[key];
                PropertyInfo[] arrpf = this.TemplateType.GetProperties();
                foreach (PropertyInfo p in arrpf)
                {
                    var name = p.Name;
                    var value = this.GetValue(p);
                    content = content.Replace($"[{name}]", value.ToString());
                }
                return content;
            }

            return null;
        }

    }
}
