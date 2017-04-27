using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Notify.Code.Utility
{
    public class FileUtility
    {
        /// <summary>
        /// 检查某个文件是否真的存在
        /// </summary>
        /// <param name="path">需要检查的文件的路径(包括路径的文件全名)</param>
        /// <returns>返回true则表示存在，false为不存在</returns>
        public static bool IsFileExists(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查文件目录是否真的存在
        /// </summary>
        /// <param name="path">需要检查的文件目录</param>
        /// <returns>返回true则表示存在，false为不存在</returns>
        public static bool IsDirectoryExists(string path)
        {
            try
            {
                return Directory.Exists(Path.GetDirectoryName(path));
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 查找文件中是否存在匹配的内容
        /// </summary>
        /// <param name="fileInfo">查找的文件流信息</param>
        /// <param name="lineTxt">在文件中需要查找的行文本</param>
        /// <param name="lowerUpper">是否区分大小写，true为区分，false为不区分</param>
        /// <returns>返回true则表示存在，false为不存在</returns>
        public static bool FindLineTextFromFile(FileInfo fileInfo, string lineTxt, bool lowerUpper = false)
        {
            bool isTrue = false; //表示没有查询到信息
            try
            {
                //首先判断文件是否存在
                if (fileInfo.Exists)
                {
                    var streamReader = new StreamReader(fileInfo.FullName);
                    do
                    {
                        string readLine = streamReader.ReadLine(); //读取的信息
                        if (string.IsNullOrEmpty(readLine))
                        {
                            break;
                        }
                        if (lowerUpper)
                        {
                            if (readLine.Trim() != lineTxt.Trim())
                            {
                                continue;
                            }
                            isTrue = true;
                            break;
                        }
                        if (readLine.Trim().ToLower() != lineTxt.Trim().ToLower())
                        {
                            continue;
                        }
                        isTrue = true;
                        break;
                    } while (streamReader.Peek() != -1);
                    streamReader.Close(); //继承自IDisposable接口，需要手动释放资源
                }
            }
            catch (System.Exception)
            {
                isTrue = false;
            }
            return isTrue;
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>内容</returns>
        public static string GetFileContent(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    StringBuilder builder = new StringBuilder();
                    while (streamReader.Peek() != -1)
                    {
                        builder.Append(streamReader.ReadLine());
                    }

                    return builder.ToString();
                }

            }
        }

        /// <summary>
        /// 获取文件夹下面所有的文件内容字典表
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>内容</returns>
        public static Dictionary<string, string> GetFileContents(string path)
        {
            var result = new Dictionary<string, string>();
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (var item in folder.GetFiles())
            {
                var content = GetFileContent($"{path}{item.Name}");
                result.Add(item.Name, content);
            }

            /*
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                //NextFolder.GetFiles()：获取目录中（不包含子目录）的文件，返回类型为FileInfo[]，支持通配符查找；
                //NextFolder.GetDirectories()：获取目录（不包含子目录）的子目录，返回类型为DirectoryInfo[]，支持通配符查找；
                //NextFolder. GetFileSystemInfos()：获取指定目录下（不包含子目录）的文件和子目录，返回类型为FileSystemInfo[]，支持通配符查找；
            }
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                //NextFile.Exists：获取指定文件是否存在；
                //NextFile.Name，NextFile.Extensioin：获取文件的名称和扩展名；
                //NextFile.FullName：获取文件的全限定名称（完整路径）；
                //NextFile.Directory：获取文件所在目录，返回类型为DirectoryInfo；
                //NextFile.DirectoryName：获取文件所在目录的路径（完整路径）；
                //NextFile.Length：获取文件的大小（字节数）；
                //NextFile.IsReadOnly：获取文件是否只读；
                //NextFile.Attributes：获取或设置指定文件的属性，返回类型为FileAttributes枚举，可以是多个值的组合
                //NextFile.CreationTime、NextFile.LastAccessTime、NextFile.LastWriteTime：分别用于获取文件的创建时间、访问时间、修改时间；
            }
            */
            return result;
        }

        // 加密密钥
        public const string FileKey = "ihlih*0037JOHT*)(PIJY*(()JI^)IO%";

        /// <summary>
        /// 对文件进行加密
        /// 调用:FileEncryptHelper.FileEncryptInfo(Server.MapPath("~" +路径), Server.MapPath("~" +路径), FileHelper.FileEncrityKey)
        /// </summary>
        /// <param name="fileOriginalPath">需要加密的文件路径</param>
        /// <param name="fileFinshPath">加密完成后存放的文件路径</param>
        /// <param name="fileKey">文件密钥</param>
        public static void FileEncryptInfo(string fileOriginalPath, string fileFinshPath, string fileKey)
        {
            // 分组加密算法的实现
            using (var fileStream = new FileStream(fileOriginalPath, FileMode.Open))
            {
                var buffer = new byte[fileStream.Length];
                // 得到需要加密的字节数组
                fileStream.Read(buffer, 0, buffer.Length);
                // 设置密钥，密钥向量，两个一样，都是16个字节byte
                var rDel = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(fileKey),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cryptoTransform = rDel.CreateEncryptor();
                byte[] cipherBytes = cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
                using (var fileSEncrypt = new FileStream(fileFinshPath, FileMode.Create, FileAccess.Write))
                {
                    fileSEncrypt.Write(cipherBytes, 0, cipherBytes.Length);
                }
            }
        }

        /// <summary>
        /// 对文件进行解密
        /// 调用:FileEncryptHelper.FileDecryptInfo(Server.MapPath("~" +路径), Server.MapPath("~" +路径), FileHelper.FileEncrityKey)
        /// </summary>
        /// <param name="fileFinshPath">传递需要解密的文件路径</param>
        /// <param name="fileOriginalPath">解密后文件存放的路径</param>
        /// <param name="fileKey">密钥</param>
        public static void FileDecryptInfo(string fileFinshPath, string fileOriginalPath, string fileKey)
        {
            using (var fileStreamIn = new FileStream(fileFinshPath, FileMode.Open, FileAccess.Read))
            {
                using (var fileStreamOut = new FileStream(fileOriginalPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var rDel = new RijndaelManaged
                    {
                        Key = Encoding.UTF8.GetBytes(fileKey),
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    };
                    using (var cryptoStream = new CryptoStream(fileStreamOut, rDel.CreateDecryptor(),
                        CryptoStreamMode.Write))
                    {
                        var bufferLen = 4096;
                        var buffer = new byte[bufferLen];
                        int bytesRead;
                        do
                        {
                            bytesRead = fileStreamIn.Read(buffer, 0, bufferLen);
                            cryptoStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }
            }
        }
    }
}
