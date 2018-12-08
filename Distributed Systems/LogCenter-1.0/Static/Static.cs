using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogCenter
{
    internal static class Static
    {
        private static Queue<LogModel> LogList;

        internal static AutoResetEvent logAutoEvent = new AutoResetEvent(false);

        /// <summary>
        /// 初始化
        /// </summary>
        internal static void Init()
        {
            LogList = new Queue<LogModel>();

            try
            {
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    Work.Start();
                });
            }
            catch (Exception ex)
            {
                Util.Log.WriteLog(ex.ToString(), Util.LogType.Error);
            }
        }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="log"></param>
        internal static void AddLog(LogModel log)
        {
            LogList.Enqueue(log);
            logAutoEvent.Set();
        }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="logType"></param>
        internal static void AddLog(string content, Util.LogType logType)
        {
            LogList.Enqueue(new LogModel { AddTime = DateTime.Now, Content = content, LogType = logType });
            logAutoEvent.Set();
        }

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <returns></returns>
        internal static Queue<LogModel> GetList()
        {
            return LogList;
        }
    }
}
