using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ImageCenter
{
    public enum ImageTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("Default")]
        Default = 1,

        /// <summary>
        /// 头像
        /// </summary>
        [Description("Head")]
        Head = 2,

        /// <summary>
        /// 身份
        /// </summary>
        [Description("Identity")]
        Identity = 3,

        /// <summary>
        /// 广告
        /// </summary>
        [Description("Advertising")]
        Advertising = 4,

        /// <summary>
        /// 活动
        /// </summary>
        [Description("Activity")]
        Activity = 5

    }
}