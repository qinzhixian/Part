using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Util.Reflection.Model;

namespace Util.Reflection
{
    public class Assembly
    {
        private static readonly Dictionary<string, List<APIModel>> APIDic = new Dictionary<string, List<APIModel>>();

        private static ConcurrentDictionary<string, MethodInfo> mMethodCache = new ConcurrentDictionary<string, MethodInfo>();


        public void Init()
        {
            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (var classItem in types)
            {
                ModuleInfoAttribute[] moduleArray = classItem.GetCustomAttributes(typeof(ModuleInfoAttribute), false) as ModuleInfoAttribute[];
                if (moduleArray != null && moduleArray.Length <= 0)
                {
                    continue;
                }

                MethodInfo[] methods = classItem.GetMethods(BindingFlags.Public | BindingFlags.Static);

                if (methods == null)
                {
                    continue;
                }

                foreach (var method in methods)
                {
                    var methodArray = method.GetCustomAttributes(typeof(MethodInfoAttribute), false);
                    if (methodArray != null && methodArray.Length <= 0)
                    {
                        continue;
                    }

                    APIModel api = new APIModel()
                    {
                        ModuleName = moduleArray[0].ModuleName,
                        MethodName = method.Name,
                        Link = string.Format("{0}/{1}", classItem.Name, method.Name),
                        Remark = GetMethodRemarks(method),
                        Parames = GetParamTypes(method)
                    };

                    if (!APIDic.ContainsKey(api.ModuleName))
                    {
                        APIDic[api.ModuleName] = new List<APIModel>();
                    }

                    APIDic[api.ModuleName].Add(api);

                    mMethodCache.TryAdd(string.Format("{0}/{1}", classItem.Name, method.Name), method);
                }
            }
        }

        /// <summary>
        /// 执行静态方法
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="methodName"></param>
        /// <param name="parame"></param>
        /// <returns></returns>
        public object CallStaticMethod(string moduleName, string methodName, object[] parame)
        {
            var key = string.Format("{0}/{1}", moduleName, methodName);

            if (mMethodCache.ContainsKey(key))
            {
                var method = mMethodCache[key];

                return method?.Invoke(null, parame);
            }

            return null;
        }

        /// <summary>
        /// 获取方法说明
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string GetMethodRemarks(MethodInfo method)
        {
            MethodInfoAttribute[] mra = method.GetCustomAttributes(typeof(MethodInfoAttribute), false) as MethodInfoAttribute[];
            if (mra.Length > 0 && mra[0] != null)
            {
                return mra[0].FunctionRemarks;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取方法的参数信息
        /// </summary>
        /// <param name="method">方法对象</param>
        /// <returns>方法所对应的参数</returns>
        private static Type[] GetParamTypes(MethodInfo method)
        {
            var parms = method.GetParameters();
            Type[] result = new Type[parms.Length];

            for (Int32 i = 0; i < parms.Length; i++)
                result[i] = parms[i].ParameterType;

            return result;
        }
    }
}
