using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Util.Log
{
    /// <summary>
    /// 日志帮助类（默认路径为当前工作目录，如需修改请设置目录）
    /// 线程安全类
    /// 可能引发的异常：Util.Exception
    /// </summary>
    public static class LogUtil
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        static LogUtil()
        {
            logFilePath = string.Format("{0}/{1}/", AppDomain.CurrentDomain.BaseDirectory, "Logs");

            Thread logWork = new Thread(() =>
            {
                while (true)
                {
                    if (LogList.Count <= 0)
                    {
                        logAutoEvent.WaitOne(1000 * 60);
                    }

                    for (int i = 0; i < LogList.Count; i++)
                    {
                        var log = LogList.Dequeue();

                        if (log == null)
                            continue;

                        Save(log);
                    }
                }
            });

            logWork.Start();
        }


        #region 私有成员

        private static string logFilePath = string.Empty;

        private static AutoResetEvent logAutoEvent = new AutoResetEvent(false);

        private static Queue<LogModel> LogList = new Queue<LogModel>();

        private static object locker = new object();

        #endregion

        #region 私有方法

        private static void AddLog(LogModel log)
        {
            LogList.Enqueue(log);

            logAutoEvent.Set();
        }

        /// <summary>
        /// 写文件
        /// </summary>
        private static void Save(LogModel log)
        {
            var now = Util.DateTime.DateTimeUtil.Now;
            string filePath = $"{logFilePath}/{now.Year}/{now.Month}/{now.Day}";

            string fileName = $"{now.ToString("yyyy-MM-dd")}.{System.Enum.GetName(typeof(Util.Log.LogType), log.LogType)}.{"txt"}";

            try
            {
                lock (locker)
                {
                    Util.IO.FileUtil.WriteFile($@"{filePath}\{fileName}", log.Content);
                }
            }
            catch (Util.Exception.ExceptionUtil ex)
            {
                WriteException(ex, "保存日志");
            }

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 设置日志文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        public static void SetLogPath(string dirPath)
        {
            logFilePath = dirPath;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="logType">日志类型</param>
        public static void Write(string msg, Util.Log.LogType logType = Util.Log.LogType.Info)
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("--------------------------------------------------------start--------------------------------------------------------");

            sb.AppendLine($"{Util.DateTime.DateTimeUtil.Now.ToString("yyyy-MM-dd HH:mm:ss")}：{msg}");

            //sb.Append("----------------------------------------------------------------------------------------------------------------end");

            AddLog(new LogModel() { Content = sb.ToString(), LogType = logType });
        }

        /// <summary>
        /// 分隔
        /// </summary>
        /// <param name="logType"></param>
        public static void WriteSeparate(Util.Log.LogType logType = Util.Log.LogType.Info)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-----------------------------------------------------------------------结束-----------------------------------------------------------------------");

            AddLog(new LogModel() { Content = sb.ToString(), LogType = logType });
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="name">异常名</param>
        /// <param name="location">异常位置</param>
        /// <param name="data">发送数据</param>
        public static void WriteException(Util.Exception.ExceptionUtil ex, string name = "", string location = "", object data = null)
        {
            if (ex == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"--------------------------------------------------------------------start--------------------------------------------------------------------");

            sb.AppendLine(string.Format("【异常名称】：{0}", string.IsNullOrEmpty(name) ? "无" : name));

            sb.AppendLine($"【异常时间】：{Util.DateTime.DateTimeUtil.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            sb.AppendLine($"【异常类型】：{ex.GetType().Name}");

            sb.AppendLine(string.Format("【内部信息】：{0}", ex.InnerException == null ? "无" : ex.InnerException.Message));

            sb.AppendLine($"【异常描述】：{ex.Message}");

            sb.AppendLine(string.Format("【异常地址】：{0}", string.IsNullOrEmpty(location) ? "无" : location));

            sb.AppendLine(string.Format("【异常数据】：{0}", data != null ? Util.Json.JsonUtil.Serialize(data) : "无"));

            sb.AppendLine($"【异常对象】：{ex.Source}");

            sb.AppendLine(string.Format("【终止位置】：{0}", ex.TargetSite == null ? "无" : string.Format("{0}/{1}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name)));

            sb.AppendLine($"【堆栈调用】：{ex.StackTrace}");

            sb.AppendLine("--------------------------------------------------------------------end--------------------------------------------------------------------");

            AddLog(new LogModel() { Content = sb.ToString(), LogType = Util.Log.LogType.Error });
        }

        /// <summary>
        /// 记录内存消耗
        /// </summary>
        /// <param name="taskName"></param>
        public static void WriteMemoryComSume(string taskName)
        {
            Write($"{taskName}占用内存：{System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} MB", Util.Log.LogType.Debug);
        }

        /// <summary>
        /// 记录内存消耗
        /// </summary>
        /// <param name="taskName"></param>
        public static void WriteMemoryComSume(string taskName, Action func)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            long runTaskBefore = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            func?.Invoke();

            long runTaskAfter = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            sw.Stop();

            Write($"{taskName}完成，耗时：{sw.ElapsedMilliseconds} ms，占用内存：{(runTaskAfter - runTaskBefore) / 1024 / 1024} MB", Util.Log.LogType.Debug);
        }

        /// <summary>
        /// 记录内存消耗
        /// </summary>
        /// <param name="taskName"></param>
        public static object WriteMemoryComSume(string taskName, Func<object> func)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            long runTaskBefore = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            var res = func?.Invoke();

            long runTaskAfter = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            sw.Stop();

            Write($"{taskName}完成，耗时：{sw.ElapsedMilliseconds} ms，占用内存：{(runTaskAfter - runTaskBefore) / 1024 / 1024} MB", Util.Log.LogType.Debug);

            return res;
        }

        #endregion

    }

    internal class LogModel
    {
        public string Content { get; set; }

        public Util.Log.LogType LogType { get; set; }
    }
}
