using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataCenter
{
    /// <summary>
    /// 全局数据对象   
    /// 使用Global数据的DbModel必须继承Util.Data.Interface.IDbModel
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="dataAssemblyName">数据模型所在程序集名   默认当前项目所在程序集</param>
        public static void Start(string dataAssemblyName = "")
        {
            //数据初始化
            Static.Start(dataAssemblyName);
        }

        #region 公共方法

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool Any<T>(Func<T, bool> where)
        {
            return Static.Any(where);
        }

        /// <summary>
        /// 获取第一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(Func<T, bool> where)
        {
            return Static.FirstOrDefault(where);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(Func<T, bool> where)
        {
            return Static.Where(where);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void Add<T>(T data)
        {
            Static.Add(data);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        public static void Remove<T>(Func<T, bool> where)
        {
            Static.Remove(where);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataAry"></param>
        public static void Delete<T>(params T[] dataAry)
        {
            Static.Remove(dataAry);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldEntity"></param>
        /// <param name="newEntity"></param>
        public static void Update<T>(Func<T, bool> where, T newEntity)
        {
            Static.Update(where, newEntity);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetList<T>()
        {
            return Static.GetList<T>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(int pageIndex, int pageSize, out int pageCount)
        {
            return Static.GetList<T>(pageIndex, pageSize, out pageCount);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(Func<T, bool> where, int pageIndex, int pageSize, out int pageCount)
        {
            return Static.GetList(where, pageIndex, pageSize, out pageCount);
        }

        #endregion

    }
}
