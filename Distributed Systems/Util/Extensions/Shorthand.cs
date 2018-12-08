using System.Text;

namespace Util.Extensions
{
    /// <summary>
    /// 中文处理类
    /// </summary>
    public class Shorthand
    {
        /// <summary>
        /// 获取首字母(小写)
        /// </summary>
        /// <param name="chinese">中文字符串</param>
        /// <returns></returns>
        public static string Convert(string chinese)
        {
            char[] chArray = new char[chinese.Length];
            for (int i = 0; i < chinese.Length; i++)
            {
                chArray[i] = Convert(chinese[i]);
            }
            return new string(chArray);
        }

        #region 私有方法

        private static char Convert(char chinese)
        {
            Encoding dstEncoding = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;
            byte[] bytes = unicode.GetBytes(new char[] { chinese });
            byte[] buffer2 = Encoding.Convert(unicode, dstEncoding, bytes);
            int num = buffer2[0] << 8;
            num += buffer2[1];
            if (In(0xb0a1, 0xb0c4, num))
            {
                return 'a';
            }
            if (In(0xb0c5, 0xb2c0, num))
            {
                return 'b';
            }
            if (In(0xb2c1, 0xb4ed, num))
            {
                return 'c';
            }
            if (In(0xb4ee, 0xb6e9, num))
            {
                return 'd';
            }
            if (In(0xb6ea, 0xb7a1, num))
            {
                return 'e';
            }
            if (In(0xb7a2, 0xb8c0, num))
            {
                return 'f';
            }
            if (In(0xb8c1, 0xb9fd, num))
            {
                return 'g';
            }
            if (In(0xb9fe, 0xbbf6, num))
            {
                return 'h';
            }
            if (In(0xbbf7, 0xbfa5, num))
            {
                return 'j';
            }
            if (In(0xbfa6, 0xc0ab, num))
            {
                return 'k';
            }
            if (In(0xc0ac, 0xc2e7, num))
            {
                return 'l';
            }
            if (In(0xc2e8, 0xc4c2, num))
            {
                return 'm';
            }
            if (In(0xc4c3, 0xc5b5, num))
            {
                return 'n';
            }
            if (In(0xc5b6, 0xc5bd, num))
            {
                return 'o';
            }
            if (In(0xc5be, 0xc6d9, num))
            {
                return 'p';
            }
            if (In(0xc6da, 0xc8ba, num))
            {
                return 'q';
            }
            if (In(0xc8bb, 0xc8f5, num))
            {
                return 'r';
            }
            if (In(0xc8f6, 0xcbf0, num))
            {
                return 's';
            }
            if (In(0xcbfa, 0xcdd9, num))
            {
                return 't';
            }
            if (In(0xcdda, 0xcef3, num))
            {
                return 'w';
            }
            if (In(0xcef4, 0xd188, num))
            {
                return 'x';
            }
            if (In(0xd1b9, 0xd4d0, num))
            {
                return 'y';
            }
            if (In(0xd4d1, 0xd7f9, num))
            {
                return 'z';
            }
            return '\0';
        }

        private static bool In(int Lp, int Hp, int Value) =>
            ((Value <= Hp) && (Value >= Lp));

        #endregion
    }
}
