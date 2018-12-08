using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    /// <summary>
    /// 日志类别枚举 
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error = 0,

        /// <summary>
        /// 调试
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 2,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3

    }
}
