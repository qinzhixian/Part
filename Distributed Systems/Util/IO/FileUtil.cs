using System;
using SIO = System.IO;
using System.Text;

namespace Util.IO
{
    /// <summary>
    /// 文件操作类
    /// 线程安全类
    /// 可能引发的异常：Util.Exception
    /// </summary>
    public static class FileUtil
    {
        #region 私有成员

        /// <summary>
        /// 线程安全锁
        /// </summary>
        private static object locker = new object();

        #endregion

        #region Add

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="oldFilePath">源文件</param>
        /// <param name="newFilePath">目标文件</param>
        public static bool Copy(string oldFilePath, string newFilePath)
        {
            lock (locker)
            {
                if (!Exists(oldFilePath) || Exists(newFilePath))
                {
                    return false;
                }

                SIO.File.Copy(oldFilePath, newFilePath);
            }

            return true;
        }

        /// <summary>
        /// 创建一个空文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void Create(string filePath)
        {
            if (Exists(filePath))
            {
                return;
            }
            using (SIO.File.Create(filePath))
            {

            }
        }

        /// <summary>
        /// 创建一个文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="value"></param>
        public static void Create(string filePath, string value)
        {
            if (Exists(filePath))
            {
                return;
            }
            using (SIO.FileStream fs = new SIO.FileStream(filePath, SIO.FileMode.CreateNew))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] data = Encoding.Default.GetBytes(value);
                    fs.Write(data, 0, data.Length);
                }
            }
        }

        #endregion

        #region Del

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        public static void DeleteFile(string filePath)
        {
            if (Exists(filePath))
            {
                lock (locker)
                {
                    SIO.File.Delete(filePath);
                }
            }
        }

        #endregion

        #region UpDate

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldFilePath">源文件</param>
        /// <param name="newFilePath">目标文件</param>
        public static bool Move(string oldFilePath, string newFilePath)
        {
            if (!Exists(oldFilePath) || Exists(newFilePath))
            {
                return false;
            }

            lock (locker)
            {
                SIO.File.Move(oldFilePath, newFilePath);
            }

            return true;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="filePath">文件历经</param>
        /// <param name="content">要写入的内容</param>
        /// <param name="encoding">内容编码</param>
        /// <param name="isOverride">是否覆盖源文件</param>
        /// <param name="isNewLine">是否新起一行</param>
        public static void WriteFile(string filePath, string content, Encoding encoding = null, bool isOverride = false, bool isNewLine = true)
        {
            SaveFile(filePath, content, encoding, isOverride, isNewLine);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">内容编码</param>
        /// <param name="isOverride">是否覆盖源文件</param>
        /// <param name="isNewLine">是否新起一行</param>
        private static void SaveFile(string filePath, string content, Encoding encoding, bool isOverride = false, bool isNewLine = true)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(filePath))
                throw new System.Exception("操作不正确！");
            string dirPath = GetDirPath(filePath);
            SIO.Directory.CreateDirectory(dirPath);

            var fileModel = SIO.FileMode.OpenOrCreate;
            if (isOverride)
            {
                fileModel = SIO.FileMode.Create;
            }
            lock (locker)
            {
                if (!Exists(filePath))
                    Create(filePath);

                using (SIO.FileStream file = new SIO.FileStream(filePath, fileModel))
                {
                    encoding = encoding == null ? Encoding.UTF8 : encoding;

                    if (isNewLine)
                    {
                        using (SIO.StreamWriter writer = new SIO.StreamWriter(file, encoding))
                        {
                            writer.BaseStream.Seek(0, SIO.SeekOrigin.End);
                            writer.Write(content);
                            writer.Write(Environment.NewLine);
                        }
                    }
                    else
                    {
                        file.Position = file.Length;

                        byte[] data = encoding.GetBytes(content);
                        file.Write(data, 0, data.Length);
                    }
                }
            }
        }

        #endregion

        #region Select

        /// <summary>
        /// 获取路径中的目录部分
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetDirPath(string filePath)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(filePath))
                path = filePath.Substring(0, filePath.LastIndexOf(@"\"));
            return path;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string filePath)
        {
            string content = string.Empty;
            if (Exists(filePath))
            {
                using (SIO.StreamReader reader = new SIO.StreamReader(filePath))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoding">编码</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            string content = string.Empty;
            if (Exists(filePath))
            {
                using (SIO.StreamReader reader = new SIO.StreamReader(filePath, encoding))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>true：存在文件 否则不存在</returns>
        public static bool Exists(string filePath)
        {
            lock (locker)
            {
                return SIO.File.Exists(filePath);
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <returns>文件大小 规范文件大小称呼，ＫＢ,ＭＢ,ＧＢ  1KB、10MB、1GB ……</returns>
        public static string GetFileSize(string filePath)
        {
            string size = string.Empty;
            if (Exists(filePath))
            {
                var file = new SIO.FileInfo(filePath);

                long length = file.Length;
                if (length > 1024 * 1024 * 1024)
                {
                    size = Convert.ToString(Math.Round((length + 0.00) / (1024 * 1024 * 1024), 2)) + " GB";
                }
                else if (length > 1024 * 1024)
                {
                    size = Convert.ToString(Math.Round((length + 0.00) / (1024 * 1024), 2)) + " MB";
                }
                size = Convert.ToString(Math.Round((length + 0.00) / 1024, 2)) + " KB";
            }
            return size;
        }

        /// <summary>
        /// 获取文件新名称全路径  file1、file2、file3 ……
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <returns>string 新的文件全路径名称</returns>
        public static string GetNewFileName(string filePath)
        {
            if (Exists(filePath))
            {
                var file = new SIO.FileInfo(filePath);
                string tempFileName = filePath.Replace(file.Extension, "");
                int getFileNameCount = 1;
                while (true)
                {
                    filePath = tempFileName + getFileNameCount.ToString() + file.Extension;

                    if (Exists(filePath) == false)
                    {
                        break;
                    }
                }
            }
            return filePath;
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        ///  /// <param name="isContansExtension">是否包含扩展名 默认包含</param>
        /// <returns>文件名称</returns>
        public static string GetFileName(string filePath, bool isContansExtension = true)
        {
            string fileName = string.Empty;

            fileName = SIO.Path.GetFileName(filePath);
            if (!isContansExtension)
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf("."));
            }
            return fileName;
        }

        #endregion
    }
}
