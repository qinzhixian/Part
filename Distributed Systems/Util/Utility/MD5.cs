using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Util.Utility
{
    /// <summary>
    /// MD5 操作类
    /// </summary>
    public static class MD5
    {

        /// <summary>
        /// MD5(16位加密)
        /// </summary>
        /// <param name="ConvertString">需要加密的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string GetMD5_16(string ConvertString)
        {
            string md5Pwd = string.Empty;

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            
            md5Pwd = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);

            md5Pwd = md5Pwd.Replace("-", "");

            return md5Pwd;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">要加密的内容</param>
        /// <returns>加密后32位小写的字符串</returns>
        public static string GetMD5_32(string str)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(str));
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
