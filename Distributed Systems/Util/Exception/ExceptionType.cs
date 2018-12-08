using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    /// <summary>
    /// 异常类别
    /// </summary>
    public enum ExceptionType
    {
        /// <summary>
        /// 路径为NULL或者空
        /// </summary>
        [Description("路径为NULL或者空")]
        PathIsNullOrEmpty,

        /// <summary>
        /// 数据(传入数据)为NULL或者空
        /// </summary>
        [Description("数据(传入数据)为NULL或者空")]
        ContentIsNullOrEmpty,

        /// <summary>
        /// 数据(输出数据)为NULL或者空
        /// </summary>
        [Description("数据(输出数据)为NULL或者空")]
        DataIsNullOrEmpty,

        /// <summary>
        /// 未知的
        /// </summary>
        [Description("未知的")]
        UnKnown
    }
}
