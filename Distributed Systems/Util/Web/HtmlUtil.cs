using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Util.Web
{
    public static class HtmlUtil
    {
        ///<summary>   
        ///清除HTML标记   
        ///</summary>   
        ///<param name="html">包括HTML的源码</param>   
        ///<returns>已经去除后的文字</returns>   
        public static string NoHTML(string html)
        {
            //删除脚本   
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML   
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            html.Replace("<", "");
            html.Replace(">", "");
            html.Replace("\r\n", "");

            return html;
        }

        /// <summary>
        /// 删除文本中带的HTML标记
        /// </summary>
        /// <param name="html">输入要删除带HTML的字符串</param>    
        /// <returns>返回处理过的字符串</returns>
        public static string DelHtmlCode(string html)
        {
            string strTemp = html;
            int htmlBeginNum = 0;
            int htmlEndNum = 0;
            while (strTemp.Contains("<"))
            {
                if (!strTemp.Contains(">")) { break; }    //当字符串内不包含">"时退出循环
                htmlBeginNum = strTemp.IndexOf("<");
                htmlEndNum = strTemp.IndexOf(">");
                //删除从"<"到">"之间的所有字符串
                strTemp = strTemp.Remove(htmlBeginNum, htmlEndNum - htmlBeginNum + 1);
            }
            strTemp = strTemp.Replace("\n", "");
            strTemp = strTemp.Replace("\r", "");
            strTemp = strTemp.Replace("\n\r", "");
            strTemp = strTemp.Replace("&nbsp;", "");
            strTemp = strTemp.Replace(" ", "");
            strTemp = strTemp.Trim();
            return strTemp;
        }

        /// <summary>
        /// 获取页面的链接正则
        /// </summary>
        /// <param name="html">要匹配的Html</param>
        /// <returns></returns>
        public static List<string> GetHref(string html)
        {
            List<string> result = new List<string>();
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)[\S]*";
            foreach (Match m in Regex.Matches(html, Reg))
            {
                result.Add(m.Value.ToLower().Replace("href=", "").Trim());
            }
            return result;
        }

        /// <summary>
        /// 匹配页面的图片地址
        /// </summary>
        /// <param name="html">要补充的http://路径信息</param>
        public static List<string> GetImgSrc(string html)
        {
            List<string> result = new List<string>();
            string Reg = @"<img.+?>";
            foreach (Match m in Regex.Matches(html.ToLower(), Reg))
            {
                result.Add(m.Value.ToLower().Trim());
            }

            return result;
        }

        /// <summary>
        /// 压缩HTML输出
        /// </summary>
        public static string ZipHtml(string Html)
        {
            Html = Regex.Replace(Html, @">\s+?<", "><");//去除HTML中的空白字符
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }

        /// <summary>
        /// 过滤指定HTML标签
        /// </summary>
        /// <param name="s_TextStr">要过滤的字符</param>
        /// <param name="html_Str">a img p div</param>
        public static string DelHtml(string s_TextStr, string html_Str)
        {
            string rStr = "";
            if (!string.IsNullOrEmpty(s_TextStr))
            {
                rStr = Regex.Replace(s_TextStr, "<" + html_Str + "[^>]*>", "", RegexOptions.IgnoreCase);
                rStr = Regex.Replace(rStr, "</" + html_Str + ">", "", RegexOptions.IgnoreCase);
            }
            return rStr;
        }

    }
}
