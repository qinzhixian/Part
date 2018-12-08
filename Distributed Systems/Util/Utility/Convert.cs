using System;
using System.Text;

namespace Util.Utility
{
    /// <summary>
    /// 类型转换帮助类
    /// </summary>
    public class Convert
    {
        /// <summary>
        /// 将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <returns>返回值</returns>
        public static byte[] GetBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }

        /// <summary>
        /// 使用指定编码将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>返回值</returns>
        public static byte[] GetBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// 将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>返回值</returns>
        public static string GetString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>返回值</returns>
        public static string GetString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

    }
}