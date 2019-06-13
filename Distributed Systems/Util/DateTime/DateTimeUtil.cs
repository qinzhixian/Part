using System;
using SDateTime = System.DateTime;
using Util.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Util.DateTime
{
    /// <summary>
    /// DateTime帮助类
    /// </summary>
    public static class DateTimeUtil
    {
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


        /// <summary>  
        /// 转换为DateTime 
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>DateTime</returns>  
        public static SDateTime ConvertToDateTime(this string timeStamp)
        {
            if (string.IsNullOrEmpty(timeStamp))
                throw new Util.Exception.ExceptionUtil("时间戳不允许为空", Exception.ExceptionType.DateTimeIsNullOrEmpty);

            try
            {
                SDateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new SDateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000000");
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
            catch (System.Exception)
            {
                throw new Util.Exception.ExceptionUtil("时间戳转换为时间中发生异常", Exception.ExceptionType.ConvertError);
            }

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
        /// 获取编号 yyyyMMddHHmmss
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

        /// <summary>
        /// 获取时间点
        /// </summary>
        /// <param name="intervalMinute">分钟间隔</param>
        /// <param name="getOneDayLastTime">获取一天最后一个时间:23:59:59</param>
        /// <returns></returns>
        public static List<SDateTime> GetTimePoint(int intervalMinute, bool getOneDayLastTime = true)
        {
            return GetTimePoint(intervalMinute, SDateTime.Parse("00:00"), SDateTime.Parse("23:59"), getOneDayLastTime);
        }

        /// <summary>
        /// 获取时间点
        /// </summary>
        /// <param name="intervalMinute">分钟间隔</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="getOneDayLastTime">获取一天最后一个时间:23:59:59</param>
        /// <returns></returns>
        public static List<SDateTime> GetTimePoint(int intervalMinute, SDateTime startTime, bool getOneDayLastTime = false)
        {
            return GetTimePoint(intervalMinute, startTime, SDateTime.Parse("23:59"), getOneDayLastTime);
        }

        /// <summary>
        /// 获取时间点
        /// </summary>
        /// <param name="intervalMinute">时间间隔</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="getOneDayLastTime">获取一天最后一个时间:23:59:59</param>
        /// <returns></returns>
        public static List<SDateTime> GetTimePoint(int intervalMinute, SDateTime startTime, SDateTime endTime, bool getOneDayLastTime = false)
        {
            List<SDateTime> list = new List<SDateTime>();

            if (startTime >= endTime)
                return list;

            SDateTime tempTime = startTime;
            list.Add(tempTime);

            while (tempTime < endTime)
            {
                tempTime = tempTime.AddMinutes(intervalMinute);
                list.Add(tempTime);
            }

            list = list.OrderByDescending(t => t).ToList();

            if (getOneDayLastTime)
            {
                list[0] = list[0].AddMinutes(60 * 24).AddSeconds(-1);
            }
            else
            {
                list.RemoveAt(0);
            }

            return list;
        }

        /// <summary>
        /// 时间分钟数能否被整除
        /// </summary>
        /// <param name="minute"></param>
        /// <param name="dateTimes"></param>
        /// <returns></returns>
        public static bool IsCanDivisibleMinute(int minute, params SDateTime[] dateTimes)
        {
            return !dateTimes.Any(t => t.Minute % minute != 0);
        }
    }
}
