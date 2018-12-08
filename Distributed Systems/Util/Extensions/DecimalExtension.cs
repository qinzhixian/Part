using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Extensions
{
    /// <summary>
    /// Decimal扩展类
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 尝试将对象转换为Decimal类型数据。转换失败是返回0
        /// </summary>
        /// <param name="decimalObj">要转换的对象</param>
        /// <returns></returns>
        public static Decimal ParseToDecimalValue(this object decimalObj)
        {
            if (decimalObj == null) return 0;

            if (Decimal.TryParse(decimalObj.ToString(), out Decimal decValue))
            {
                return decValue;
            }
            return 0;
        }

        /// <summary>
        /// 转中文大写数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertNumToZHUpperCase(this decimal value)
        {
            string[] numList = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            string[] unitList = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟" };

            decimal money = value;
            if (money == 0)
            {
                return "零元整";
            }

            StringBuilder strMoney = new StringBuilder();
            //只取小数后2位



            string strNum = decimal.Truncate(money * 100).ToString();

            int len = strNum.Length;
            int zero = 0;
            for (int i = 0; i < len; i++)
            {
                int num = int.Parse(strNum.Substring(i, 1));
                int unitNum = len - i - 1;

                if (num == 0)
                {
                    zero++;
                    if (unitNum == 2 || unitNum == 6 || unitNum == 10)
                    {
                        if (unitNum == 2 || zero < 4)
                            strMoney.Append(unitList[unitNum]);
                        zero = 0;
                    }
                }
                else
                {

                    if (zero > 0)
                    {
                        strMoney.Append(numList[0]);
                        zero = 0;
                    }
                    strMoney.Append(numList[num]);
                    strMoney.Append(unitList[unitNum]);
                }

            }
            if (zero > 0)
                strMoney.Append("整");

            return strMoney.ToString();
        }

        /// <summary>
        /// 截取指定位数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal ToFixed(this decimal value, int length)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, length));
            return Math.Truncate(value) + Math.Floor((value - Math.Truncate(value)) * sp) / sp;
        }

        /// <summary>
        ///  截取指定位数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double ToFixed(this double value, int length)
        {
            double sp = Math.Pow(10, length);
            return Math.Truncate(value) + Math.Floor((value - Math.Truncate(value)) * sp) / sp;
        }
    }
}
