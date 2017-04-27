using System;
using System.IO;

namespace Notify.Code.Write
{
    /// <summary>
    /// 写入文本
    /// </summary>
    internal class TextWriter
    {
        /// <summary>
        /// 写入文件路径
        /// </summary>
        private readonly string fileName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriter"/> class.
        /// </summary>
        public TextWriter()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriter"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public TextWriter(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logInfo">内容 </param>
        /// <returns>结果</returns>
        internal bool WriteLog(string logInfo)
        {
            if (string.IsNullOrWhiteSpace(logInfo))
            {
                return false;
            }

            DateTime timeStamp = DateTime.Now;
            string path = this.GetFileMainPath(timeStamp);
            FileInfo lastFile = GetLastAccessFile(path, timeStamp);
            FileStream fileStream = GetFileStream(lastFile, path, timeStamp);
            if (fileStream == null)
            {
                return false;
            }

            try
            {
                StreamWriter sw = new StreamWriter(fileStream);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(logInfo);
                sw.Flush();
                sw.Close();
                return true;
            }
            finally
            {
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>path</returns>
        private string GetFileMainPath(DateTime timeStamp)
        {
            return Path.Combine(this.fileName, timeStamp.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 获取最后写入日志的文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>FileInfo</returns>
        private static FileInfo GetLastAccessFile(string path, DateTime timeStamp)
        {
            FileInfo result = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (timeStamp.Hour == fileInfo.CreationTime.Hour)
                    {
                        result = fileInfo;
                        break;
                    }
                }
            }
            else
            {
                directoryInfo.Create();
            }

            return result;
        }

        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="fileInfo">lastFile</param>
        /// <param name="path">path</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>FileStream</returns>
        private static FileStream GetFileStream(FileInfo fileInfo, string path, DateTime timeStamp)
        {
            FileStream result;
            if (fileInfo == null)
            {
                try
                {
                    result = CreateFile(path, GetFileMainName(timeStamp));
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
            else if (IsOutOfTimeMaxLength(fileInfo.CreationTime, timeStamp))
            {
                result = CreateFile(path, GetFileMainName(timeStamp));
            }
            else
            {
                try
                {
                    result = fileInfo.OpenWrite();
                }
                catch
                {
                    result = CreateFile(path, GetFileMainName(timeStamp));
                }
            }

            return result;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName1">名称</param>
        /// <returns>FileStream</returns>
        private static FileStream CreateFile(string path, string fileName1)
        {
            return File.Create(string.Format(@"{0}\{1}.log", path, fileName1));
        }

        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>HHmmssfff</returns>
        private static string GetFileMainName(DateTime timeStamp)
        {
            return timeStamp.ToString("HH");
        }

        /// <summary>
        /// IsOutOfTimeMaxLength
        /// </summary>
        /// <param name="creationTime">creationTime</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <returns>bool</returns>
        private static bool IsOutOfTimeMaxLength(DateTime creationTime, DateTime timeStamp)
        {
            return Math.Abs((creationTime - timeStamp).TotalHours) >= 1;
        }
    }
}
