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
                    if (log == null)
                        continue;

                    try
                    {
                        WriteLog(log);
                    }
                    catch (System.Exception ex)
                    {
                        WriteLog(new LogModel
                        {
                            AddTime = System.DateTime.Now,
                            Content = string.Format("记录日志时发生错误！错误原因：{0}", ex.Message),
                            LogType = Util.LogType.Error
                        });
                    }
                }
                Static.logAutoEvent.WaitOne(1000 * 60 * 10);
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