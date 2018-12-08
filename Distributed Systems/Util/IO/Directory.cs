using System;
using SIO = System.IO;

namespace Util.IO
{
    /// <summary>
    /// 目录操作类
    /// 线程安全类
    /// 可能引发的异常：Util.Exception
    /// </summary>
    public static class Directory
    {
        #region 私有成员

        private static object locker = new object();

        #endregion

        #region Add

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        public static void Create(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                throw new Util.Exception("目录路径不能为空！");
            }
            if (Exists(dirPath))
                return;
            lock (locker)
            {
                SIO.Directory.CreateDirectory(dirPath);
            }

        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="oldDirPath">要复制的文件夹</param>
        /// <param name="newDirPath">复制到的文件夹</param>
        /// <param name="isSkipExistsDirOrFile">是否跳过存在的目录或文件</param>
        public static void Copy(string oldDirPath, string newDirPath, bool isSkipExistsDirOrFile = true)
        {
            if (Exists(oldDirPath))
                throw new Util.Exception("原始目录不存在！");

            string[] directories = GetDirs(oldDirPath);
            string[] files = GetDirFiles(oldDirPath);

            if (directories.Length > 0 || files.Length > 0)
            {
                Create(newDirPath);

                foreach (string dir in directories)
                {
                    if (Exists(dir))
                        if (isSkipExistsDirOrFile)
                            continue;
                        else
                            Delete(dir);
                    Copy(dir, newDirPath + dir.Substring(dir.LastIndexOf(@"\")));
                }

                foreach (string file in files)
                {
                    Util.IO.File.Copy(file, newDirPath + file.Substring(file.LastIndexOf(@"\")));
                }
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dirPath">要删除文件夹的全路径</param>
        public static void Delete(string dirPath)
        {
            if (Exists(dirPath))
            {
                lock (locker)
                {
                    SIO.Directory.Delete(dirPath, true);
                }
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// 设当前目录
        /// </summary>
        /// <param name="path">目录绝对路径</param>
        public static void SetCurrentDirectory(string path)
        {
            SIO.Directory.SetCurrentDirectory(path);
        }

        #endregion

        #region Select

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>是否存在</returns>
        public static bool Exists(string dirPath)
        {
            lock (locker)
            {
                return SIO.Directory.Exists(dirPath);
            }

        }

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
        /// 获取子目录列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>目录列表</returns>
        public static string[] GetDirs(string dirPath)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");
            return SIO.Directory.GetDirectories(dirPath);
        }

        /// <summary>
        /// 获取目录中文件列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>文件列表</returns>
        public static string[] GetDirFiles(string dirPath)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            return SIO.Directory.GetFiles(dirPath, "*.*", SIO.SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获取目录中文件列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="searchPattern">查找文件的扩展名如“*.*代表所有文件；*.doc代表所有doc文件”</param>
        /// <returns>文件列表</returns>
        public static string[] GetDirFiles(string dirPath, string searchPattern)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            return SIO.Directory.GetFiles(dirPath, searchPattern);
        }

        /// <summary>
        /// 获取目录中文件列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="so">查找文件的选项，是否包含子级文件夹</param>
        /// <returns>s文件列表</returns>
        public static string[] GetDirFiles(string dirPath, SIO.SearchOption so)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            return SIO.Directory.GetFiles(dirPath, "*.*", so);
        }

        /// <summary>
        /// 获取目录中文件列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="searchPattern">查找文件的扩展名如“*.*代表所有文件；*.doc代表所有doc文件”</param>
        /// <param name="so">查找文件的选项，是否包含子级文件夹</param>
        /// <returns>文件列表</returns>
        public static string[] GetDirFiles(string dirPath, string searchPattern, SIO.SearchOption so)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            return SIO.Directory.GetFiles(dirPath, searchPattern, so);
        }

        /// <summary>
        /// 判断目录是否为空目录
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns>是否为空</returns>
        public static bool IsEmptyDirectory(string dirPath)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            //判断是否存在文件
            string[] fileNames = GetFiles(dirPath);
            if (fileNames.Length > 0)
            {
                return false;
            }

            //判断是否存在文件夹
            string[] directoryNames = GetDirs(dirPath);
            if (directoryNames.Length > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取当前工作目录
        /// </summary>
        /// <returns>目录名</returns>
        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 获取逻辑驱动器
        /// </summary>
        /// <returns>逻辑驱动器</returns>
        public static SIO.DriveInfo[] GetAllDrives()
        {
            return SIO.DriveInfo.GetDrives();
        }

        /// <summary>
        /// 获取目录中文件列表
        /// </summary>
        /// <param name="dirPath">指定目录的绝对路径</param>
        /// <returns>文件列表</returns>
        public static string[] GetFiles(string dirPath)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            return SIO.Directory.GetFiles(dirPath);
        }

        /// <summary>
        /// 获取目录所有文件列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>文件列表</returns>
        public static string[] GetFiles(string dirPath, string searchPattern, bool isSearchChild)
        {
            if (!Exists(dirPath))
                throw new Util.Exception("目录不存在！");

            return SIO.Directory.GetFiles(dirPath, searchPattern, isSearchChild ? SIO.SearchOption.AllDirectories : SIO.SearchOption.TopDirectoryOnly);
        }

        #endregion

    }
}
