using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DataCenter
{
    internal static class StaticDao
    {
        #region 私有成员

        private static object locker = new object();

        private static string DataAesKv = "3f102d2a096af5aae6cc8666fbc17029";

        public static bool DataIsEncrypt { get; set; }

        #endregion

        #region 公共成员

        /// <summary>
        /// 获取数据文件地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataDirPath">数据目录</param>
        /// <returns></returns>
        public static string GetDataFilePath(Type type, string dataDirPath)
        {
            var dirPath = string.Format("{0}/", dataDirPath);
            Util.IO.Directory.Create(dirPath);

            var fileName = type.Name;

            if (DataIsEncrypt)
            {
                fileName = Util.Utility.MD5.GetMD5(Util.Utility.Base64.Encrypt(fileName));
            }

            string filePath = string.Format("{0}/{1}.data", dirPath, fileName);

            Util.IO.File.Create(filePath);

            return filePath;
        }

        /// <summary>
        /// 读取文件内数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataFilePath"></param>
        /// <returns></returns>
        public static List<object> ReadGlobalData(Type type, string dataFilePath)
        {
            var jsonData = Util.IO.File.ReadFile(dataFilePath, Encoding.UTF8);

            if (!string.IsNullOrEmpty(jsonData))
            {
                if (DataIsEncrypt)
                {
                    jsonData = Util.Utility.AES.AesDecrypt(Util.Utility.Base64.Decrypt(jsonData), DataAesKv);
                }
            }

            var list = Util.Json.Deseriailze<List<object>>(jsonData);
            if (list == null || list.Count <= 0)
            {
                list = new List<object>();
            }

            return list;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="list"></param>
        /// <param name="globalData"></param>
        /// <param name="dataFilePath"></param>
        public static void SaveChanges<T>(List<T> list, Dictionary<Type, List<object>> globalData, Dictionary<Type, string> dataFilePath)
        {
            var type = typeof(T);
            if (!globalData.ContainsKey(type))
            {
                throw new Exception("未找到对应DB！");
            }
            if (!dataFilePath.ContainsKey(type))
            {
                throw new Exception("未找到对应DB的数据文件！");
            }
            globalData[type] = list.ConvertAll(s => (object)s);

            var jsonData = Util.Json.Serialize(list);

            if (!string.IsNullOrEmpty(jsonData))
            {
                if (DataIsEncrypt)
                {
                    jsonData = Util.Utility.Base64.Encrypt(Util.Utility.AES.AesEncrypt(jsonData, DataAesKv));
                }
            }

            Util.IO.File.WriteFile(dataFilePath[type], jsonData, Encoding.UTF8, true);
        }

        #endregion

    }
}
