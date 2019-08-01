using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Util.Json
{
    /// <summary>
    /// Json类
    /// </summary>
    public static class JsonUtil
    {
        static IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

        static JsonUtil()
        {
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data">要序列化的数据</param>
        /// <returns>序列化后的JsonString字符串</returns>
        public static string Serialize(object data, Newtonsoft.Json.Formatting formatting = Formatting.None)
        {
            if (data == null || string.IsNullOrEmpty(data.ToString()))
                return "{}";
            return JsonConvert.SerializeObject(data, formatting, timeFormat);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Deseriailze(this string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return default(Dictionary<string, string>);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr, timeFormat);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T Deseriailze<T>(this string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return default(T);
            return JsonConvert.DeserializeObject<T>(jsonStr, timeFormat);
        }


        public static Dictionary<string, object> JsonParse(this string jsonStr)
        {
            return Deseriailze<Dictionary<string, object>>(jsonStr);
        }

        public static bool IsJson(this object jsonStr)
        {
            try
            {
                JsonParse(jsonStr.ToString());
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }
    }
}
