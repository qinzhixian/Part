using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Util.Config
{
    /// <summary>
    /// AppConfig帮助类
    /// </summary>
    public static class AppConfigUtil
    {
        private const string ConfigFileNotSpecified = "请指定需要读取的配置文件";
        private static Configuration mConfiguration;

        static AppConfigUtil()
        {
            List<string> list = new List<string>();
            foreach (string str in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.config"))
            {
                string str2 = Path.GetFileName(str).ToLower();
                if ((!str2.Contains("vshost") && !str2.Contains("debug")) && !str2.Contains("release"))
                {
                    list.Add(str);
                }
            }
            if (list.Count == 1)
            {
                ConfigPath = list[0];
                SetConfigFile(ConfigPath);
            }
        }

        /// <summary>
        /// config文件中appSettings配置节的value项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppConfig(string key)
        {
            if (mConfiguration == null)
            {
                throw new Exception("请指定需要读取的配置文件");
            }
            if (!mConfiguration.AppSettings.Settings.AllKeys.Contains<string>(key))
            {
                throw new Exception($"Key={key}的配置不存在!");
            }
            return mConfiguration.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// 依据连接串名字connectionName返回数据连接字符串
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        public static string GetConnectionStringsConfig(string connectionName)
        {
            return mConfiguration?.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.ToString();
        }

        /// <summary>
        /// 设置配置文件路径
        /// </summary>
        /// <param name="path">config文件路径</param>
        public static void SetConfigFile(string path)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = path
            };
            ConfigPath = path;
            mConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 在config文件中appSettings配置节更新或增加一对键、值对
        /// </summary>
        /// <param name="newKey">新的key</param>
        /// <param name="newValue">新的value</param>
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            if (mConfiguration == null)
            {
                throw new Exception("请指定需要读取的配置文件");
            }
            if (mConfiguration.AppSettings.Settings.AllKeys.Contains<string>(newKey))
            {
                mConfiguration.AppSettings.Settings.Remove(newKey);
            }
            mConfiguration.AppSettings.Settings.Add(newKey, newValue);
            mConfiguration.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 更新连接字符串
        /// </summary>
        /// <param name="newName">连接字符串名称</param>
        /// <param name="newConString">连接字符串内容</param>
        /// <param name="newProviderName">数据提供程序名称</param>
        public static void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
        {
            if (mConfiguration == null)
            {
                throw new Exception("请指定需要读取的配置文件");
            }
            if (mConfiguration.ConnectionStrings.ConnectionStrings[newName] != null)
            {
                mConfiguration.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            ConnectionStringSettings settings = new ConnectionStringSettings(newName, newConString, newProviderName);
            mConfiguration.ConnectionStrings.ConnectionStrings.Add(settings);
            mConfiguration.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 当前读取的配置文件路径
        /// </summary>
        public static string ConfigPath
        {
            get;
            [CompilerGenerated]
            private set;
        }
    }
}
