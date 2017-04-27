using System;
using System.Configuration;
using System.IO;

namespace Notify.Code.Code
{
    /// <summary>
    /// 配置文件监听器
    /// </summary>
    public sealed class ConfigMonitor
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string Path { get; set; } = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public static string Filter { get; set; } = "\\Web.config";

        /// <summary>
        /// 配置文件更改委托定义
        /// </summary>
        public delegate void EventHandlerAfterConfigModify();

        /// <summary>
        /// 配置文件更改事件
        /// </summary>
        public static event EventHandlerAfterConfigModify ConfigModifyInfoEvent;

        /// <summary>
        /// 配置对象
        /// </summary>
        public static Configuration Config { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="ConfigMonitor"/> class.
        /// 构造函数
        /// </summary>
        static ConfigMonitor()
        {
            MonitorConfigFile();
            InitConnectionConfig();
        }

        /// <summary>
        /// 创建配置文件发动监听器
        /// </summary>
        private static void MonitorConfigFile()
        {
            FileSystemWatcher fileWatcher = new FileSystemWatcher
                                                {
                                                    Path = Path,
                                                    Filter = Filter,
                                                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName
                                                };

            fileWatcher.Changed += OnChanged;
            fileWatcher.Created += OnChanged;
            fileWatcher.Deleted += OnChanged;
            fileWatcher.Renamed += OnChanged;
            fileWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 更改处理事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            InitConnectionConfig();
            RaiseEvent();
        }

        /// <summary>
        /// 向订阅者发布信息
        /// </summary>
        private static void RaiseEvent()
        {
            ConfigModifyInfoEvent?.Invoke();
        }

        /// <summary>
        /// 初始化所有连接配置
        /// </summary>
        private static void InitConnectionConfig()
        {
            string filePath = Path + Filter;
            var map = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            Config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }
    }
}