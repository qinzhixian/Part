using System.Web;
using Util.Extensions;

namespace Util.Web
{
    /// <summary>
    /// HttpCookie操作类
    /// </summary>
    public static class Cookies
    {
        /// <summary>
        /// 删除一个Cookie
        /// </summary>
        /// <param name="name">cookiename</param>
        public static void Remove(string name)
        {
            HttpContext.Current.Response.Cookies.Remove(name);
        }

        /// <summary>
        /// 获取一个Cookie的值
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];

            string str = string.Empty;
            cookie.IfNullOrEmpty(() =>
            {
                str = WebUtil.UrlDeCode(cookie.Value);
            });

            return str;
        }

        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, DateTime.Now.AddHours(24));
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string name, string value, System.DateTime expires)
        {
            Remove(name);

            HttpCookie cookie = new HttpCookie(name)
            {
                Value = value,
                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
