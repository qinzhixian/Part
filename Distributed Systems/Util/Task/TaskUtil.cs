using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Util.Task
{
    public static class TaskUtil
    {
        /// <summary>
        /// 执行异步任务
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns>异常列表</returns>
        public static List<System.Exception> ExcuteTasks(params System.Threading.Tasks.Task[] tasks)
        {
            List<System.Exception> exceptions = new List<System.Exception>();

            List<System.Threading.Tasks.Task> taskList = new List<System.Threading.Tasks.Task>();

            foreach (var task in tasks)
            {
                if (task == null)
                    continue;

                try
                {
                    if (task.Status == TaskStatus.Created)
                    {
                        task.Start();
                    }

                    if (task.Exception != null)
                    {
                        exceptions.Add(task.Exception.InnerException ?? task.Exception);
                    }
                }
                catch (System.Exception ex)
                {
                    exceptions.Add(ex.InnerException ?? ex);
                }
            }

            try
            {
                System.Threading.Tasks.Task.WaitAll(tasks);
            }
            catch (System.Exception ex)
            {
                exceptions.Add(ex.InnerException ?? ex);
            }

            return exceptions;
        }

        /// <summary>
        /// 记录内存消耗
        /// </summary>
        /// <param name="taskName"></param>
        public static void WriteMemoryComSume(string taskName)
        {
            Log.LogUtil.Write($"{taskName}占用内存：{System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} MB", Log.LogType.Debug);
        }

        /// <summary>
        /// 记录内存消耗
        /// </summary>
        /// <param name="taskName"></param>
        public static void ExcuteTaskWriteMemoryComSume(string taskName, Action func)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            long runTaskBefore = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            try
            {
                func?.Invoke();
            }
            catch (System.Exception ex)
            {
                Log.LogUtil.WriteException(ex, "执行Task出错！");
            }

            long runTaskAfter = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            sw.Stop();

            Log.LogUtil.Write($"{taskName}完成，耗时：{sw.ElapsedMilliseconds} ms，占用内存：{(runTaskAfter - runTaskBefore) / 1024 / 1024} MB", Log.LogType.Debug);
        }

        /// <summary>
        /// 记录内存消耗
        /// </summary>
        /// <param name="taskName"></param>
        public static object ExcuteTaskWriteMemoryComSume(string taskName, Func<object> func)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            long runTaskBefore = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            object res = null;
            try
            {
                res = func?.Invoke();
            }
            catch (System.Exception ex)
            {
                Log.LogUtil.WriteException(ex, "执行Task出错！");
            }

            long runTaskAfter = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            sw.Stop();

            Log.LogUtil.Write($"{taskName}完成，耗时：{sw.ElapsedMilliseconds} ms，占用内存：{(runTaskAfter - runTaskBefore) / 1024 / 1024} MB", Log.LogType.Debug);

            return res;
        }
    }
}
