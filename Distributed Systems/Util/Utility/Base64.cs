using System.Text;

namespace Util.Utility
{
    /// <summary>
    /// Base编码类
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(str);
            str = System.Convert.ToBase64String(bytedata, 0, bytedata.Length);

            return str;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str)
        {
            byte[] outputb = System.Convert.FromBase64String(str);
            str = Encoding.Default.GetString(outputb);

            return str;
        }



    }
}
