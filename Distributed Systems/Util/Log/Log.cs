using System;
using System.Text;

namespace Util
{
    /// <summary>
    /// 日志帮助类（默认路径为当前工作目录，如需修改请设置目录）
    /// 线程安全类
    /// 可能引发的异常：Util.Exception
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 构造函数设置Log目录为当前工作目录
        /// </summary>
        static Log()
        {
            if (string.IsNullOrEmpty(logPath))
                logPath = Util.IO.Directory.GetCurrentDirectory() + @"\Logs\";
        }

        #region 公共方法

        /// <summary>
        /// 设置Log目录
        /// </summary>
        /// <param name="path">目录地址(不含文件名)</param>
        public static void SetLogPath(string path)
        {
            logPath = path;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">要写的内容</param>
        /// <returns>Log文件地址</returns>
        public static string WriteLog(string content)
        {
            content = string.Format("【Time】：{0}    【Content】：{1}", GetShortTime(), content);

            return SaveLog(content);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">要写的内容</param>
        /// <param name="logType">日志类别</param>
        /// <returns>Log文件地址</returns>
        public static string WriteLog(string content, LogType logType)
        {
            content = string.Format("【Time】：{0}    【Content】：{1}", GetShortTime(), content);

            return SaveLog(content, logType);
        }

        ///// <summary>
        ///// 读取日志
        ///// </summary>
        ///// <param name="logFilePath">log文件地址</param>
        ///// <returns>日志列表</returns>
        //public static string[] ReadLogs(string logFilePath)
        //{
        //    string fiels = Util.IO.File.ReadFile(logFilePath, Encoding.UTF8);
        //    return fiels.Split(new string[] { writeEndAppendStr }, StringSplitOptions.None);
        //}

        #endregion

        #region 私有成员

        /// <summary>
        /// 线程安全锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Log目录
        /// </summary>
        private static string logPath { get; set; }

        /// <summary>
        /// 日志分隔符
        /// </summary>
        private static string writeEndAppendStr = "";

        /// <summary>
        /// 获取格式化后的当前时间
        /// </summary>
        /// <returns></returns>
        private static string GetShortTime()
        {
            return System.DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取LogFile文件地址
        /// </summary>
        /// <returns></returns>
        private static string GetLogFilePath(LogType logType)
        {
            if (string.IsNullOrEmpty(logPath))
            {
                throw new Util.Exception("请先设置Log文件路径！");
            }
            var now = System.DateTime.Now;
            var dir = now.Year + "\\" + now.Month + "\\" + now.Day + "\\";
            string type = logType.ToString(); //Util.EnumUtil.GetMemberName<LogType>(logType);
            return logPath + dir + now.ToString("yyyy年-MM月-dd日-HH时") + "-" + type + ".log";
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="content">要保存的内容</param>
        /// <param name="logType">Log类别</param>
        /// <returns>Log文件地址</returns>
        private static string SaveLog(string content, LogType logType = LogType.Error)
        {
            lock (locker)
            {
                string logFilePath = GetLogFilePath(logType);
                Util.IO.File.WriteFile(logFilePath, content);
                
                return logFilePath;
            }
        }

        #endregion

    }
}
