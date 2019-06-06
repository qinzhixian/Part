using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Util.Reflection
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionUtil
    {
        private static Type AssemblyType(string assemblyName, string className)
        {
            //className = className.StartsWith(assemblyName) ? className : string.Format("{0}.{1}", assemblyName, className);
            return System.Reflection.Assembly.Load(assemblyName).GetTypes().FirstOrDefault(t => t.Name == className);
        }

        /// <summary>
        /// 执行静态方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static object CallStaticMethod(string assemblyName, string className, string methodName)
        {
            return CallStaticMethod(assemblyName, className, methodName, Type.EmptyTypes, null);
        }

        /// <summary>
        /// 执行静态方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object CallStaticMethod(string assemblyName, string className, string methodName, params object[] param)
        {
            return CallStaticMethod(assemblyName, className, methodName, GetParamTypes(param), param);
        }

        /// <summary>
        /// 执行静态方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="types"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object CallStaticMethod(string assemblyName, string className, string methodName, Type[] types, params object[] param)
        {
            var method = GetMethod(assemblyName, className, methodName, types);

            return method?.Invoke(null, param);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ReflectionClass CreateInstance(string assemblyName, string className)
        {
            return CreateInstance(assemblyName, className, null);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ReflectionClass CreateInstance(string assemblyName, string className, params object[] param)
        {
            return new ReflectionClass(Activator.CreateInstance(GetClassType(assemblyName, className), param));
        }

        private static Type GetClassType(string assemblyName, string className)
        {
            string[] textArray1 = new string[] { assemblyName, className };
            string key = string.Join("_", textArray1);
            Type type = ReflectionCache.GetType(key);
            if (type == null)
            {
                type = AssemblyType(assemblyName, className);

                if (type == null)
                {
                    throw new ArgumentNullException(string.Format("{0}.{1}", assemblyName, className), "未找到类型");
                }

                ReflectionCache.SetType(key, type);
            }
            return type;
        }

        private static MethodInfo GetMethod(string assemblyName, string className, string methodName, Type[] types)
        {
            string[] textArray1 = new string[] { assemblyName, className, methodName, string.Join<Type>("_", types) };
            string key = string.Join("_", textArray1);
            MethodInfo mehtod = ReflectionCache.GetMehtod(key);
            if (mehtod == null)
            {
                mehtod = GetClassType(assemblyName, className).GetMethod(methodName, types);

                if (mehtod == null)
                {
                    throw new ArgumentNullException(string.Format("{0}.{1}.{2}", assemblyName, className, methodName), "未找到方法");
                }

                ReflectionCache.SetMethod(key, mehtod);
            }
            return mehtod;
        }

        internal static Type[] GetParamTypes(params object[] param)
        {
            Type[] emptyTypes = Type.EmptyTypes;
            if ((param != null) && (param.Length != 0))
            {
                emptyTypes = (from s in param select s.GetType()).ToArray<Type>();
            }
            return emptyTypes;
        }

        private static string CheckParamExists(string[] paramNames, string paramNameItem)
        {
            foreach (string str in paramNames)
            {
                if (string.Equals(str, paramNameItem, StringComparison.OrdinalIgnoreCase))
                {
                    return str;
                }
            }
            return null;
        }

    }
}
