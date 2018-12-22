
using System.Diagnostics;
using System.Text;
using System.Threading;
using Ds = System.Diagnostics;

namespace Util
{
    /// <summary>
    /// Process处理类
    /// </summary>
    public static class Process
    {
        /// <summary>  
        /// 运行一个控制台程序并返回其输出参数。  
        /// </summary>  
        /// <param name="filename">程序名</param>  
        /// <param name="arguments">输入参数</param>  
        /// <returns></returns>  
        public static string StartCmd(string filename, string arguments = "")
        {
            try
            {
                Ds.Process proc = new Ds.Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    Thread.Sleep(100);

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    return txt;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        /// <summary>
        /// 打开一个程序
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        public static void StartExe(string fileName, string arguments = "")
        {
            System.Diagnostics.Process.Start(fileName, arguments);
        }


    }
}
