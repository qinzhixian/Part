using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        public static void Start(string dataAssemblyName, bool dataIsEncrypt)
        {
            if (!string.IsNullOrEmpty(dataAssemblyName))
            {
                DataAssemblyName = dataAssemblyName;
            }

            StaticDao.DataIsEncrypt = dataIsEncrypt;

            var typeList = Util.Reflection.AssemblyUtil.GetTypeListOfImplementedInterface(DataAssemblyName, typeof(Interface.IDbModel)).ToArray();

            if (GlobalData == null)
            {
                GlobalData = new Dictionary<Type, List<object>>();
            }
            if (DataFilePath == null)
            {
                DataFilePath = new Dictionary<Type, string>();
            }

            ReadData(typeList);
        }

        /// <summary>
        /// 全局数据对象
        /// </summary>
        public static Dictionary<Type, List<object>> GlobalData { get; set; }

        /// <summary>
        /// 数据文件地址
        /// </summary>
        public static Dictionary<Type, string> DataFilePath { get; set; }

        /// <summary>
        /// 数据目录
        /// </summary>
        public static string DataDirPath
        {
            get
            {
                var path = Util.IO.Directory.GetCurrentDirectory() + "/Data/";
                Util.IO.Directory.Create(path);

                return path;
            }
        }

        /// <summary>
        /// DbModel所在程序集名
        /// </summary>
        private static string DataAssemblyName = string.Empty;



        #region 私有成员

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="typeList"></param>
        private static void ReadData(params Type[] typeList)
        {
            foreach (var type in typeList)
            {
                if (!DataFilePath.ContainsKey(type))
                {
                    DataFilePath.Add(type, StaticDao.GetDataFilePath(type, DataDirPath));
                }

                if (!GlobalData.ContainsKey(type))
                {
                    var data = StaticDao.ReadGlobalData(type, DataFilePath[type]);
                    GlobalData.Add(type, data);
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="list"></param>
        private static void SaveChanges<T>(List<T> list)
        {
            StaticDao.SaveChanges(list, GlobalData, DataFilePath);
            //ThreadPool.QueueUserWorkItem((s) =>
            //{
            //    StaticDao.SaveChanges(list, GlobalData, DataFilePath);
            //    Thread.Sleep(1000 * 2);
            //});
        }

        private static int ComputePageCount(int pageSize, int rowCount)
        {
            int num = rowCount / pageSize;
            if (rowCount > (num * pageSize))
            {
                return (num + 1);
            }
            return num;
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
        /// <param name="data"></param>
        public static void Add<T>(params T[] data)
        {
            var list = GetList<T>();
            list.AddRange(data);

            SaveChanges(list);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data"></param>
        public static void Remove<T>(params T[] data)
        {
            var list = GetList<T>();
            foreach (var item in data)
            {
                list.Remove(item);
            }

            SaveChanges(list);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        public static void Remove<T>(Func<T, bool> where)
        {
            var list = GetList<T>();
            var entityList = list.Where(where).ToArray();
            if (entityList != null && entityList.Length > 0)
            {
                foreach (var item in entityList)
                {
                    list.Remove(item);
                }
            }
            SaveChanges(list);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="entity"></param>
        public static void Update<T>(Func<T, bool> where, T entity)
        {
            var list = GetList<T>();
            var data = list.Where(where).FirstOrDefault();
            if (data != null)
            {
                list.Remove(data);
            }
            list.Add(entity);
            SaveChanges(list);
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
