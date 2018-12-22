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
        [Description("输入数据为NULL或者空")]
        InputDataIsNullOrEmpty,

        /// <summary>
        /// 数据(输出数据)为NULL或者空
        /// </summary>
        [Description("数输出数据为NULL或者空")]
        OutDataIsNullOrEmpty,

        /// <summary>
        /// 数据转换失败
        /// </summary>
        [Description("数据转换失败")]
        ConvertError,

        /// <summary>
        /// 时间为NULL或者空
        /// </summary>
        [Description("时间为NULL或者空")]
        DateTimeIsNullOrEmpty,


        /// <summary>
        /// 未知的
        /// </summary>
        [Description("未知的")]
        UnKnown
    }
}
