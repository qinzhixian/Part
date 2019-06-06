using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Util.Reflection.Attribute;

namespace Util.Reflection
{
    /// <summary>
    /// 反射缓存类
    /// </summary>
    public static class ReflectionCache
    {
        private static ConcurrentDictionary<string, MethodInfo> mMethodCache = new ConcurrentDictionary<string, MethodInfo>();

        private static ConcurrentDictionary<string, Type> mTypeCache = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// 初始化特性模块
        /// </summary>
        public static void InitAttributeModule()
        {
            Type[] classArray = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (var classItem in classArray)
            {
                ModuleInfoAttribute[] moduleArray = classItem.GetCustomAttributes(typeof(ModuleInfoAttribute), false) as ModuleInfoAttribute[];
                if (moduleArray != null && moduleArray.Length <= 0)
                {
                    continue;
                }

                MethodInfo[] methodArray = classItem.GetMethods(BindingFlags.Public | BindingFlags.Static);

                if (methodArray == null || methodArray.Length <= 0)
                {
                    continue;
                }

                foreach (var method in methodArray)
                {
                    var methods = method.GetCustomAttributes(typeof(MethodInfoAttribute), false);
                    if (methods != null && methods.Length <= 0)
                    {
                        continue;
                    }
                    SetMethod(GetConfigKey(classItem.Name, method.Name), method);
                }
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public static void Clear()
        {
            mMethodCache.Clear();
            mTypeCache.Clear();
        }

        /// <summary>
        /// 获取方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static MethodInfo GetMehtod(string key)
        {
            if (!mMethodCache.ContainsKey(key))
            {
                return null;
            }
            return mMethodCache[key];
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Type GetType(string key)
        {
            if (!mTypeCache.ContainsKey(key))
            {
                return null;
            }
            return mTypeCache[key];
        }

        /// <summary>
        /// 设置方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="method"></param>
        public static void SetMethod(string key, MethodInfo method)
        {
            mMethodCache[key] = method;
        }

        /// <summary>
        /// 设置类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public static void SetType(string key, Type type)
        {
            mTypeCache[key] = type;
        }


        /// <summary>
        /// 拼接Key
        /// </summary>
        /// <param name="className">模块名称</param>
        /// <param name="methedName">子模块名称</param>
        /// <returns>拼接之后的Key</returns>
        private static String GetConfigKey(String className, String methedName)
        {
            return String.Format("{0}_{1}", className, methedName);
        }
    }
}
