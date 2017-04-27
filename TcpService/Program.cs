using System;
using System.Reflection;
using Notify.Code.Write;

namespace TcpService
{
    /// <summary>
    /// 服务端
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 服务端入口
        /// </summary>
        /// <param name="args">args</param>
        public static void Main(string[] args)
        {
            // 注册错误事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // 程序引导
            Booting();
            // 启动服务
            string message;
            if (BootServer(out message))
            {
                // 启动成功
                BootSuccessed();
                EnterCommandMode();
            }
            else
            {
                // 启动失败
                BootFailed(message);
            }
        }

        /// <summary>
        /// 程序引导
        /// </summary>
        private static void Booting()
        {
            Console.Title = "服务器端";
            Console.WriteLine("启动中...");
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>结果</returns>
        private static bool BootServer(out string message)
        {
            try
            {
                RequestListner.Instance.Start();
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            message = string.Empty;
            return true;
        }

        /// <summary>
        /// 常用指令
        /// </summary>
        private static void EnterCommandMode()
        {
            Console.WriteLine("目前支持命令:");
            ShowCommands();
            WriteLine();
            while (true)
            {
                var readLine = Console.ReadLine();
                if (readLine != null)
                {
                    var inputString = readLine.ToLower();
                    switch (inputString)
                    {
                        case "cls":
                            Console.Clear();
                            break;
                        case "exit":
                            //RequestListner.Instance.Dispose();
                            //DataProcessor.Instance.Stop();
                            //LogonCenter.Instance.Dispose();
                            //CustomGCCollection.Instance.Dispose();
                            return;
                        case "company":
                            //ShowCompanies();
                            break;
                        case "user":
                            //ShowUsers();
                            break;
                        case "help":
                            ShowCommands();
                            break;
                        default:
                            Console.WriteLine("不存在的命令");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 显示常用指令
        /// </summary>
        private static void ShowCommands()
        {
            Console.WriteLine("    help       帮助");
            Console.WriteLine("    cls        清屏");
            Console.WriteLine("    exit       退出");
            Console.WriteLine("    company    查询当前登录单位");
            Console.WriteLine("    user       查看当前登录用户");
        }

        /// <summary>
        /// 服务启动成功
        /// </summary>
        private static void BootSuccessed()
        {
            Console.WriteLine("启动成功");
            Console.WriteLine("    启动时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("    版本号:   " + GetVersion());
            WriteLine();
        }

        /// <summary>
        /// 服务启动失败
        /// </summary>
        /// <param name="reason">失败信息</param>
        private static void BootFailed(string reason)
        {
            Console.WriteLine("启动失败" + Environment.NewLine + "失败原因:" + reason);
        }

        /// <summary>
        /// 当前程序未处理的异常事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            ApplicationError(ex?.Message ?? "未知错误");
            if (ex != null)
            {
                LogService.WriteLog(ex, "系统错误");
            }
        }

        /// <summary>
        /// 程序错误
        /// </summary>
        /// <param name="message">错误消息</param>
        internal static void ApplicationError(string message)
        {
            Console.WriteLine("程序出错" + Environment.NewLine + "错误信息:" + message);
        }

        /// <summary>
        /// 输出行
        /// </summary>
        private static void WriteLine()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns>结果</returns>
        private static string GetVersion()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var versionAttribute = Attribute.GetCustomAttribute(currentAssembly, typeof(AssemblyFileVersionAttribute)) as AssemblyFileVersionAttribute;
            if (versionAttribute != null)
            {
                return versionAttribute.Version;
            }
            else
            {
                return "0.0.0.1";
            }
        }
    }
}