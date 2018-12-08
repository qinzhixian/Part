using System;
using SDateTime = System.DateTime;
using Util.Extensions;

namespace Util
{
    /// <summary>
    /// DateTime帮助类
    /// </summary>
    public static class DateTime
    {
        /// <summary>  
        /// 转换为DateTime 
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>DateTime</returns>  
        public static SDateTime ConvertToDateTime(this string timeStamp)
        {
            SDateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new SDateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>  
        /// 转换为时间戳  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>时间戳</returns>  
        public static int ConvertToTimestamp(this SDateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 检查指定字符串是否是有效的DateTime
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string dateTimeStr)
        {
            return SDateTime.TryParse(dateTimeStr, out SDateTime result);
        }

        /// <summary>
        /// 将指定字符串转换为有效的DateTime
        /// 失败则返回DateTime的最小值：0001-1-1 0:00:00
        /// </summary>
        /// <param name="datetimeStr">要转换的字符串</param>
        /// <returns>转换之后的值，转换失败则返回DateTime的最小值：0001-1-1 0:00:00</returns>
        public static SDateTime ToDateTime(this string datetimeStr)
        {
            if (SDateTime.TryParse(datetimeStr, out SDateTime result))
                return result;

            return SDateTime.MinValue;
        }

        /// <summary>
        /// 将指定字符串转换为有效的DateTime
        /// 失败则返回传的默认值
        /// </summary>
        /// <param name="datetimeStr">要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认值</param>
        /// <returns>转换之后的值，转换失败则返回DateTime的最小值：0001-1-1 0:00:00</returns>
        public static SDateTime ToDateTime(this string datetimeStr, SDateTime defaultVal)
        {
            if (SDateTime.TryParse(datetimeStr, out SDateTime result))
            {
                return result;
            }
            return defaultVal;
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        public static string GetNo(bool appendRandom = false)
        {
            string code = Now.ToString("yyyyMMddHHmmss");

            if (appendRandom)
            {
                Random random = new Random();
                string strRandom = random.Next(1000, 10000).ToString();
                code = string.Format("{0}{1}", code, strRandom);
            }

            return code;
        }



        #region 属性

        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static SDateTime Now { get { return SDateTime.Now; } }

        /// <summary>
        /// 获取最大值
        /// </summary>
        public static SDateTime MaxValue { get { return SDateTime.MaxValue; } }

        /// <summary>
        /// 获取最小值
        /// </summary>
        public static SDateTime MinValue { get { return SDateTime.MinValue; } }

        #endregion
    }
}
