using System;
namespace LogCenter
{
    public static class Global
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="logDirPath">日志目录</param>
        public static void Start(string logDirPath = "")
        {

            Static.Init();

            Util.Log.LogUtil.Write("logApplication is started ！", Util.Log.LogType.Debug);
        }

        public static void AddLog(LogModel log)
        {
            Static.AddLog(log);
        }

        public static void AddLog(string content, Util.Log.LogType logType = Util.Log.LogType.Error)
        {
            Static.AddLog(content, logType);
        }
    }
}