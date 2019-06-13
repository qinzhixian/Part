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
        static JsonUtil()
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data">要序列化的数据</param>
        /// <returns>序列化后的JsonString字符串</returns>
        public static string Serialize(object data)
        {
            if (data == null || string.IsNullOrEmpty(data.ToString()))
                throw new Util.Exception.ExceptionUtil("要序列化的数据不能为空！");
            return JsonConvert.SerializeObject(data);
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
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
