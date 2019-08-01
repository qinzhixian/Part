using System;
using System.Linq;

namespace Util.Extensions
{
    /// <summary>
    /// 扩展帮助类
    /// </summary>
    public static class StringExpansion
    {
        /// <summary>
        ///     替换系统IF
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func">表达式返回bool类型</param>
        /// <param name="trueAction">表达式为真时执行</param>
        /// <param name="falseAction">表达式为假时执行</param>
        public static object If(this object obj, Func<object, bool> func, Action trueAction = null, Action falseAction = null)
        {
            if (func.Invoke(obj))
                trueAction?.Invoke();
            else
                falseAction?.Invoke();
            return obj;
        }

        /// <summary>
        ///     判断是否为NULL或空对象，执行回调
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <param name="falseAction">判断为假时执行的回调</param>
        /// <param name="trueAction">判断为真时执行的回调</param>
        public static object IfNullOrEmpty(this object obj, Action falseAction = null, Action trueAction = null)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                trueAction?.Invoke();
            else
                falseAction?.Invoke();
            return obj;
        }

        /// <summary>
        ///     判断是否为NULL或空对象，执行回调
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <param name="falseAction">判断为假时执行的回调</param>
        /// <param name="trueAction">判断为真时执行的回调</param>
        public static object IfNullOrEmpty(this object obj, Func<object> falseAction = null, Func<object> trueAction = null)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return trueAction?.Invoke();
            else
                return falseAction?.Invoke();
        }

        /// <summary>
        ///     判断是否为NULL或Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>判断的结果</returns>
        public static bool IfNullOrEmpty(object obj)
        {
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                return true;
            return false;
        }

        /// <summary>
        ///     判断是否为NULL或Empty，为真则返回0
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns>转换后的值</returns>
        public static int IfNullOrEmpty(this string str)
        {
            var result = 0;
            if (str != null && !string.IsNullOrEmpty(str))
                int.TryParse(str, out result);
            return result;
        }

        /// <summary>
        ///     判断是否为NULL或Empty，为真则返回指定值
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <param name="val">为真时返回的值</param>
        public static Int64 IfNullOrEmpty(this string str, Int64 val)
        {
            var result = val;
            if (str != null && !string.IsNullOrEmpty(str))
                Int64.TryParse(str, out result);
            return result;
        }

        /// <summary>
        ///     判断是否为NULL或Empty，为真则返回默认值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="str">要判断的对象</param>
        /// <returns>T类型的默认值</returns>
        public static T IfNullOrEmpty<T>(this T str)
        {
            var result = default(T);
            if (str != null && !string.IsNullOrEmpty(str.ToString()))
                result = str;
            return result;
        }

        /// <summary>
        ///     判断是否为NULL或Empty，为真则返回指定值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="str">要判断的对象</param>
        /// <param name="val">为真时返回的值</param>
        public static T IfNullOrEmpty<T>(this T str, T val)
        {
            var result = val;
            if (str != null && !string.IsNullOrEmpty(str.ToString()))
                result = str;
            return result;
        }

        /// <summary>
        /// 判断是否是有效的DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            bool res = System.DateTime.TryParse(str, out System.DateTime time);
            return res;
        }

        /// <summary>
        /// 判断是否是有效的DateTime，如果是则转换，如果不是返回当前时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static System.DateTime IfDateTime(this string str)
        {
            if (System.DateTime.TryParse(str, out System.DateTime time))
                return time;
            else
                return System.DateTime.Now;
        }

        /// <summary>
        /// 判断是否是有效的DateTime，如果是则转换，如果不是返回传递的默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static System.DateTime IfDateTime(this string str, System.DateTime val)
        {
            System.DateTime time;
            if (!System.DateTime.TryParse(str, out time))
                return val;
            return time;
        }

        /// <summary>
        /// 包含空或NULL
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static bool IsAnyEmptyOrNull(params object[] vals)
        {
            return vals.Any(t => t == null || string.IsNullOrEmpty(t.ToString()));
        }

        /// <summary>
        /// 全部为空或NULL
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static bool IsAllEmptyOrNull(params string[] vals)
        {
            return !vals.Any(t => string.IsNullOrEmpty(t));
        }


    }
}
