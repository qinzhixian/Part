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
            logDirPath = string.IsNullOrEmpty(logDirPath) ? Util.IO.Directory.GetCurrentDirectory() : logDirPath;

            Util.LogUtil.SetLogConfig(string.Format("{0}/Logs/", logDirPath));

            Static.Init();

            Util.LogUtil.Write("logApplication is started ！", Util.LogType.Info);
        }

        public static void AddLog(LogModel log)
        {
            Static.AddLog(log);
        }

        public static void AddLog(string content, Util.LogType logType = Util.LogType.Error)
        {
            Static.AddLog(content, logType);
        }
    }
}