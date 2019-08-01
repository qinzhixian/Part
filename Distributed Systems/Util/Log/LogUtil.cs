using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Util.Web;
using Util.Json;
using Util.IO;

namespace Util.Log
{
    public static class LogUtil
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        static LogUtil()
        {
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

                        if (log != null)
                            Save(log);
                    }
                }
            });

            logWork.IsBackground = true;

            logWork.Start();

            Write($"日志任务已启动，线程ID：{logWork.ManagedThreadId}", LogType.Debug);
        }


        #region 私有成员

        private static LogConfig logConfig = new LogConfig();

        private static AutoResetEvent logAutoEvent = new AutoResetEvent(false);

        private static Queue<LogModel> LogList = new Queue<LogModel>();

        private static object locker = new object();

        #endregion

        #region 私有方法

        internal static void AddLog(LogModel log)
        {
            LogList.Enqueue(log);

            logAutoEvent.Set();
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        private static void Save(LogModel log)
        {
            var now = DateTime.DateTimeUtil.Now;

            string filePath = $@"{logConfig.LogPathDir}\{now.ToString(logConfig.FilePathFormat)}\";

            string fileName = $"{now.ToString("yyyy-MM-dd")}.{System.Enum.GetName(typeof(LogType), log.LogType)}.{"txt"}";

            try
            {
                lock (locker)
                {
                    FileUtil.WriteFile($"{filePath}/{fileName}", log.Content);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex, "保存日志");
            }

        }

        #endregion

        #region 公共方法

        ///// <summary>
        ///// 设置日志配置
        ///// </summary>
        ///// <param name="dirPath"></param>
        //public static void SetLogConfig(LogConfig config)
        //{
        //    logConfig.LogPathDir = config.LogPathDir ?? logConfig.LogPathDir;
        //    logConfig.FilePathFormat = config.FilePathFormat ?? logConfig.FilePathFormat;
        //}

        /// <summary>
        /// 设置日志配置
        /// </summary>
        /// <param name="logPathDir">日志目录</param>
        /// <param name="filePathFormat">日志文件格式 默认：yyyy/MM/dd</param>
        public static void SetLogConfig(string logPathDir = "", string filePathFormat = "")
        {
            logConfig.LogPathDir = string.IsNullOrEmpty(logPathDir) ? logConfig.LogPathDir : logPathDir;
            logConfig.FilePathFormat = string.IsNullOrEmpty(filePathFormat) ? logConfig.FilePathFormat : filePathFormat;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="logType">日志类型</param>
        public static void Write(string msg, LogType logType = LogType.Info)
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("--------------------------------------------------------start--------------------------------------------------------");

            sb.AppendLine($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：{msg}");

            //sb.Append("----------------------------------------------------------------------------------------------------------------end");

            AddLog(new LogModel() { Content = sb.ToString(), LogType = logType });
        }

        /// <summary>
        /// 分隔
        /// </summary>
        /// <param name="logType"></param>
        public static void WriteSeparate(LogType logType = LogType.Info)
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
        public static void WriteException(System.Exception ex, string name = "", string location = "", object data = null)
        {
            if (ex == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"--------------------------------------------------------------------start--------------------------------------------------------------------");

            sb.AppendLine($"【异常名称】：{string.Format(string.IsNullOrEmpty(name) ? "无" : name)}");

            sb.AppendLine($"【异常时间】：{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            sb.AppendLine($"【异常类型】：{ex.GetType().Name}");

            sb.AppendLine($"【内部信息】：{string.Format(ex.InnerException == null ? "无" : ex.InnerException.Message)}");

            sb.AppendLine($"【异常描述】：{ex.Message}");

            sb.AppendLine($"【异常地址】： { string.Format(string.IsNullOrEmpty(location) ? "无" : location)}");

            sb.AppendLine($"【异常数据】：{string.Format(string.IsNullOrEmpty(data.ToString()) ? "无" : JsonUtil.Serialize(data))}");

            sb.AppendLine($"【异常对象】：{ex.Source}");

            sb.AppendLine($"【终止位置】：{string.Format(ex.TargetSite == null ? "无" : string.Format("{0}/{1}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name))}");

            sb.AppendLine($"【堆栈调用】：{ex.StackTrace}");

            sb.AppendLine("--------------------------------------------------------------------end--------------------------------------------------------------------");

            AddLog(new LogModel() { Content = sb.ToString(), LogType = LogType.Error });
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="request">request对象</param>
        /// <param name="responseData">返回数据(最终被序列化为Json格式)</param>
        /// <param name="operatorName">操作者</param>
        public static void WriteOperationalLog(HttpRequest request, object responseData, string operatorName = "")
        {
            //只记录上传数据，需保证所有上传接口用Post方式
            if (request.HttpMethod != System.Net.Http.HttpMethod.Post.ToString())
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"------------------------------------------start----------------------------------------------------------------------------------------------");

            sb.AppendLine($"【操作时间】：{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            sb.AppendLine($"【操作地址】：{WebUtil.GetRequestUrl(request)}");

            sb.AppendLine($"【上传数据】：{WebUtil.GetResponseParame(request)}");

            responseData = string.IsNullOrEmpty(responseData.ToString()) ? new object { } : responseData;

            responseData = responseData.IsJson() ? responseData.ToString() : JsonUtil.Serialize(responseData);

            sb.AppendLine($"【输出数据】：{responseData}");

            sb.AppendLine($"【操 作 者】：{operatorName}");

            sb.AppendLine("----------------------------------------------------------------------------------------------end------------------------------------------");

            AddLog(new LogModel() { Content = sb.ToString(), LogType = LogType.Data });
        }

        #endregion

    }
}
