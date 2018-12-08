using System;
using System.Collections.Generic;

namespace DataCenter
{
    internal static class StaticDao
    {
        #region 私有成员

        private static object locker = new object();

        private static string DataAesKv = "3f102d2a096af5aae6cc8666fbc17029";

        private static string GetDataDirPath(Type type)
        {
            var path = string.Format("{0}/Data/{1}", Util.IO.Directory.GetCurrentDirectory(), type.Name);
            Util.IO.Directory.Create(path);

            return path;
        }

        /// <summary>
        /// 数据目录
        /// </summary>
        private static string GetDataFilePath(Type type, string dataStr)
        {
            var iflePath = string.Format("{0}/{1}.data", GetDataDirPath(type),  dataStr);

            return iflePath;

        }

        private static string GetDataStr(object data)
        {
            var dataStr = Util.Json.Serialize(data);

            dataStr = Util.Utility.Base64.Encrypt(Util.Utility.AES.AesEncrypt(dataStr, DataAesKv));

            return dataStr;
        }

        #endregion

        #region 公共成员

        public static bool DataIsEncrypt { get; set; }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="globalData">全局数据对象</param>
        /// <param name="typeList">DbModelList</param>
        /// <returns></returns>
        public static Dictionary<Type, List<object>> ReadData(Dictionary<Type, List<object>> globalData, params Type[] typeList)
        {
            foreach (var type in typeList)
            {
                if (!globalData.ContainsKey(type))
                {
                    var dataDirPath = GetDataDirPath(type);
                    var data = ReadGlobalData(type, dataDirPath);
                    globalData.Add(type, data);
                }
            }
            return globalData;
        }

        #endregion


        #region CRUD

        /// <summary>
        /// 读取文件内数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataFilePath"></param>
        /// <returns></returns>
        public static List<object> ReadGlobalData(Type type, string dataFilePath)
        {
            List<object> list = new List<object>();
            var files = Util.IO.Directory.GetDirFiles(dataFilePath, "*.data");

            for (int i = 0; i < files.Length; i++)
            {
                var item = files[i];

                var jsonData = Util.IO.File.GetFileName(item, false);
                jsonData = Util.Utility.AES.AesDecrypt(Util.Utility.Base64.Decrypt(jsonData), DataAesKv);

                var data = Util.Json.Deseriailze<object>(jsonData);
                if (data != null)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public static void Add<T>(T data)
        {
            var path = GetDataFilePath(typeof(T), GetDataStr(data));
            Util.IO.File.Create(path);
        }

        public static void Del<T>(T data)
        {
            Util.IO.File.DeleteFile(GetDataFilePath(typeof(T), GetDataStr(data)));
        }

        public static void Update<T>(T oldData, T newData)
        {
            Del(oldData);

            Add(newData);
        }
        
        #endregion

    }
}
