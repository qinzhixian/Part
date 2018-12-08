using System;

namespace Util.Extensions
{
    /// <summary>
    /// 扩展帮助类
    /// </summary>
    public static class Extensions
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
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
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
            if (str != null && !string.IsNullOrEmpty(str)) int.TryParse(str, out result);
            return result;
        }

        /// <summary>
        ///     判断是否为NULL或Empty，为真则返回指定值
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <param name="val">为真时返回的值</param>
        public static int IfNullOrEmpty(this string str, int val)
        {
            var result = val;
            if (str != null && !string.IsNullOrEmpty(str))
                int.TryParse(str, out result);
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
            if (str != null && !string.IsNullOrEmpty(str.ToString())) result = str;
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
            if (str != null && !string.IsNullOrEmpty(str.ToString())) result = str;
            return result;
        }


    }
}
