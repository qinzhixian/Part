using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataCenter
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Static
    {

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataAssemblyName"></param>
        public static void Start(string dataAssemblyName)
        {
            if (!string.IsNullOrEmpty(dataAssemblyName))
            {
                DataAssemblyName = dataAssemblyName;
            }
            
            var typeList = Util.Reflection.AssemblyUtil.GetTypeListOfImplementedInterface(DataAssemblyName, typeof(Interface.IDbModel)).ToArray();

            if (GlobalData == null)
            {
                GlobalData = new Dictionary<Type, List<object>>();
            }

            GlobalData = StaticDao.ReadData(GlobalData, typeList);
        }

        /// <summary>
        /// 全局数据对象
        /// </summary>
        public static Dictionary<Type, List<object>> GlobalData { get; set; }

        #region 私有成员

        private static object locker = new object();

        /// <summary>
        /// DbModel所在程序集名
        /// </summary>
        private static string DataAssemblyName = string.Empty;

        private static int ComputePageCount(int pageSize, int rowCount)
        {
            int num = rowCount / pageSize;
            if (rowCount > (num * pageSize))
            {
                return (num + 1);
            }
            return num;
        }

        private static void Excute(Action func)
        {
            ThreadPool.QueueUserWorkItem(s =>
            {
                func?.Invoke();
            });
        }

        private static void SaveChanges<T>(List<T> list)
        {
            GlobalData[typeof(T)] = list.ConvertAll(s => (object)s);
        }

        #endregion

        #region 公共成员

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool Any<T>(Func<T, bool> where)
        {
            var list = GetList<T>();
            return list.Any(where);
        }

        /// <summary>
        /// 获取第一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(Func<T, bool> where)
        {
            var list = GetList<T>();
            var data = list.FirstOrDefault(where);
            return data;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> Where<T>(Func<T, bool> where)
        {
            var list = GetList<T>();

            var data = list.Where(where);

            return data;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="dataAry"></param>
        public static void Add<T>(params T[] dataAry)
        {
            lock (locker)
            {
                var list = GetList<T>();
                list.AddRange(dataAry);
                SaveChanges(list);
            }
            foreach (var item in dataAry)
            {
                Excute(() =>
                {
                    StaticDao.Add(item);
                });
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="dataAry"></param>
        public static void Remove<T>(params T[] dataAry)
        {
            lock (locker)
            {
                var list = GetList<T>();
                foreach (var item in dataAry)
                {
                    list.Remove(item);
                }
                SaveChanges(list);
            }
            foreach (var item in dataAry)
            {
                Excute(() =>
                {
                    StaticDao.Del(item);
                });
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        public static void Remove<T>(Func<T, bool> where)
        {
            T[] entityList = new T[] { };
            lock (locker)
            {
                var list = GetList<T>();
                entityList = list.Where(where).ToArray();
                foreach (var item in entityList)
                {
                    list.Remove(item);
                }
                SaveChanges(list);
            }
            foreach (var item in entityList)
            {
                Excute(() =>
                {
                    StaticDao.Del(item);
                });
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldEntity"></param>
        /// <param name="newEntity"></param>
        public static void Update<T>(Func<T, bool> where, T newEntity)
        {
            T[] oldEntityList = new T[] { };
            lock (locker)
            {
                var list = GetList<T>();
                oldEntityList = list.Where(where).ToArray();
                foreach (var item in oldEntityList)
                {
                    list.Remove(item);
                    list.Add(newEntity);
                }

                SaveChanges(list);
            }
            foreach (var item in oldEntityList)
            {
                Excute(() =>
                {
                    StaticDao.Update(item, newEntity);
                });
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetList<T>()
        {
            var type = typeof(T);
            List<T> list = new List<T>();
            if (GlobalData == null)
            {
                return list;
            }
            if (!GlobalData.ContainsKey(type))
            {
                return list;
            }

            var dataList = GlobalData[type];
            if (dataList == null || dataList.Count <= 0)
            {

            }
            list = JArray.Parse(Util.Json.Serialize(dataList)).ToObject<List<T>>();
            return list;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(int pageIndex, int pageSize, out int pageCount)
        {
            var type = typeof(T);

            var data = GlobalData[type];

            var list = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            pageCount = ComputePageCount(pageSize, data.Count);

            return JArray.Parse(Util.Json.Serialize(list)).ToObject<List<T>>();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(Func<T, bool> where, int pageIndex, int pageSize, out int pageCount)
        {
            var type = typeof(T);

            var data = GlobalData[type];

            var list = Util.Json.Deseriailze<List<T>>(Util.Json.Serialize(data)).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            pageCount = ComputePageCount(pageSize, data.Count);

            return list;
        }

        #endregion

    }
}
