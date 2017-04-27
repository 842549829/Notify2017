using System;
using System.IO;
using System.Text;

namespace Notify.Code.Write
{
    /// <summary>
    /// 写入文本日志
    /// </summary>
    public static class LogService
    {
        /// <summary>
        /// The obj.
        /// </summary>
        private static readonly object obj = new object();

        /// <summary>
        /// 记录异常文本日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="remark">备注</param>
        public static void WriteLog(System.Exception ex, string remark)
        {
            WriteLog(ex, null, remark);
        }

        /// <summary>
        /// 记录异常文本日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="path">日志路径</param>
        /// <param name="remark">备注</param>
        public static void WriteLog(System.Exception ex, string path, string remark)
        {
            var errormessage = CreateErrorMessage(ex, remark);
            WriteLog(errormessage.ToString(), path ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/ExceptionLog"));
        }

        /// <summary>
        /// 记录异常文本日志
        /// </summary>
        /// <param name="describe">错误描述</param>
        /// <param name="ex">异常</param>
        /// <param name="path">日志路径</param>
        /// <param name="remark">备注</param>
        public static void WriteLog(string describe, System.Exception ex, string path, string remark)
        {
            var errormessage = CreateErrorMessage(ex, remark);
            WriteLog($"Describe:{describe} Error:{errormessage}", path ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/ExceptionLog"));
        }

        /// <summary>
        /// 创建异常消息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="remark">备注</param>
        /// <returns>结果</returns>
        private static StringBuilder CreateErrorMessage(System.Exception ex, string remark)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("************************Exception Start********************************");
            string newLine = Environment.NewLine;
            System.Exception innerException = ex.InnerException;
            stringBuilder.AppendFormat("Exception Date:{0}{1}", DateTime.Now, Environment.NewLine);
            if (innerException != null)
            {
                stringBuilder.AppendFormat("Inner Exception Type:{0}{1}", innerException.GetType(), newLine);
                stringBuilder.AppendFormat("Inner Exception Message:{0}{1}", innerException.Message, newLine);
                stringBuilder.AppendFormat("Inner Exception Source:{0}{1}", innerException.Source, newLine);
                stringBuilder.AppendFormat("Inner Exception StackTrace:{0}{1}", innerException.StackTrace, newLine);
            }
            stringBuilder.AppendFormat("Exception Type:{0}{1}", ex.GetType(), newLine);
            stringBuilder.AppendFormat("Exception Message:{0}{1}", ex.Message, newLine);
            stringBuilder.AppendFormat("Exception Source:{0}{1}", ex.Source, newLine);
            stringBuilder.AppendFormat("Exception StackTrace:{0}{1}", ex.StackTrace, newLine);
            stringBuilder.AppendFormat("Exception Remark：{0}{1}", remark, newLine);
            stringBuilder.Append("************************Exception End************************************");
            stringBuilder.Append(newLine);
            return stringBuilder;
        }

        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void WriteLog(string content)
        {
            WriteLog(content, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
        }

        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="path">日志路径</param>
        public static void WriteLog(string content, string path)
        {
            Action action = () => Log(content, path);
            action.BeginInvoke(null, null);
        }

        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The 
        /// </returns>
        internal static bool Log(string content, string path)
        {
            lock (obj)
            {
                try
                {
                    TextWriter textWriter = new TextWriter(path);
                    return
                        textWriter.WriteLog(
                            DateTime.Now.ToString("日志时间:yyyy-MM-dd HH:mm:ss") + Environment.NewLine + content
                            + Environment.NewLine);
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }
}
