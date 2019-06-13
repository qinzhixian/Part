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
            ThreadPool.QueueUserWorkItem((s) =>
            {
                try
                {
                    Work.Start();
                }
                catch (Exception ex)
                {
                    Util.Log.LogUtil.Write(ex.ToString(), Util.Log.LogType.Error);
                }
            });
        }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="log"></param>
        internal static void AddLog(LogModel log)
        {
            try
            {
                LogList.Enqueue(log);
                logAutoEvent.Set();
            }
            catch (Exception ex)
            {
                AddLog(string.Format("添加日志是发生错误！错误原因：{0}", ex.Message), Util.Log.LogType.Error);
            }

        }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="logType"></param>
        internal static void AddLog(string content, Util.Log.LogType logType)
        {
            AddLog(new LogModel { AddTime = DateTime.Now, Content = content, LogType = logType });
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
