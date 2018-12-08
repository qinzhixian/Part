using System.Web;

namespace Util.Web
{
    /// <summary>
    /// HttpSession操作类
    /// </summary>
    public static class Session
    {
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="name"></param>
        /// <returns>存入的值</returns>
        public static object Get(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        public static void Set(string name, object value)
        {
            Remove(name);

            HttpContext.Current.Session.Add(name, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name">name</param>
        public static void Remove(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
