using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SEnum = System.Enum;

namespace Util
{
    /// <summary>
    /// 枚举帮助类
    /// 可能引发的异常：Util.Exception
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// 获取枚举成员的值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this SEnum obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 根据值转换为指定类型的枚举成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string val) where T : struct
        {
            if (string.IsNullOrEmpty(val))
            {
                return default(T);
            }
            try
            {
                return (T)SEnum.Parse(typeof(T), val, true);
            }
            catch (System.Exception ex)
            {
                throw new Util.Exception.ExceptionUtil($"转换枚举失败！错误信息：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取指定枚举成员的描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescriptionString(this SEnum obj)
        {
            var attribs = (DescriptionAttribute[])obj.GetType().GetField(obj.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribs.Length > 0 ? attribs[0].Description : obj.ToString();
        }

        /// <summary>
        /// 根据枚举值，获取指定枚举类的成员描述
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="val">成员的值</param>
        /// <returns></returns>
        public static string GetDescriptionString(this Type type, int? val)
        {
            var values = from SEnum e in SEnum.GetValues(type)
                         select new { id = e.ToInt(), name = e.GetDescriptionString() };

            if (!val.HasValue) val = 0;

            try
            {
                return values.ToList().Find(c => c.id == val).name;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取值和描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumValueAndDisription(Type enumType)
        {
            Dictionary<int, string> res = new Dictionary<int, string>();

            var memberInfos = enumType.GetMembers();

            foreach (var member in memberInfos)
            {
                DescriptionAttribute[] attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    res.Add((int)System.Enum.Parse(enumType, member.Name), attrs[0].Description);
                }
            }
            return res;
        }

    }
}
