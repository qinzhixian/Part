using System.Threading;
using System.Threading.Tasks;

namespace LogCenter
{
    internal static class Work
    {
        /// <summary>
        /// 日志启动
        /// </summary>
        internal static void Start()
        {
            while (true)
            {
                var logList = Static.GetList();

                int len = logList.Count;
                for (int i = 0; i < len; i++)
                {
                    var log = logList.Dequeue();
                    ThreadPool.QueueUserWorkItem((s) =>
                    {
                        WriteLog(s as LogModel);
                    }, log);
                }
                Static.logAutoEvent.WaitOne();
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        private static void WriteLog(LogModel log)
        {
            Util.Log.WriteLog(string.Format("{0}", log.Content), log.LogType);
        }

    }
}