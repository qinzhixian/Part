using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Util.Reflection
{
    /// <summary>
    /// 反射操作类
    /// </summary>
    public static class AssemblyUtil
    {
        private static string assemblyString { get; set; }

        static AssemblyUtil()
        {
            assemblyString = System.Reflection.Assembly.GetExecutingAssembly().FullName;
        }

        /// <summary>
        /// 执行初始化(需继承自IRegisterMethods)
        /// </summary>
        public static void InIt(string assemblyName = "")
        {
            List<Type> list = GetTypeListOfImplementedInterface(assemblyName, typeof(IRegisterMethods));
            foreach (Type type in list)
            {
                var model = type.Assembly.CreateInstance(type.FullName) as IRegisterMethods;
                if (model != null)
                {
                    model.InIt();
                }
            }
        }

        /// <summary>
        /// 执行所有方法(method is public and static)
        /// </summary>
        public static void ExcuteAllMethod(string assemblyName)
        {
            List<Type> list = GetTypeListOfImplementedInterface(assemblyName, typeof(IRegisterMethods));
            foreach (Type type in list)
            {
                ExcuteMethodList(type);
            }
        }

        /// <summary>
        /// 执行所有方法(method is public and static)
        /// </summary>
        public static void ExcuteAllMethod(string assemblyName, Type interfaceType)
        {
            List<Type> list = GetTypeListOfImplementedInterface(assemblyName, interfaceType);
            foreach (Type type in list)
            {
                ExcuteMethodList(type);
            }
        }

        /// <summary>
        /// 获取实现了接口的类型列表
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="interfaceTypes">接口类型集合</param>
        /// <returns></returns>
        public static List<Type> GetTypeListOfImplementedInterface(string assemblyName = "", params Type[] interfaceTypes)
        {
            List<Type> typeList = new List<Type>();

            //获取整个应用程序集的类型数组
            assemblyName = string.IsNullOrEmpty(assemblyName) ? assemblyString : assemblyName;
            var assembly = System.Reflection.Assembly.Load(assemblyName);
            Type[] types = assembly.GetExportedTypes();
            if (types == null || types.Length == 0)
            {
                return typeList;
            }

            //遍历每一个类型，看看是否实现了指定的接口
            foreach (Type type in types)
            {
                //获得此类型所实现的所有接口列表
                Type[] allInterfaces = type.GetInterfaces();

                //判断给出的接口类型列表是否存在于实现的所有接口列表中
                if (interfaceTypes.All(item => allInterfaces.Contains(item)))
                {
                    typeList.Add(type);
                }
            }

            return typeList;
        }

        private static void ExcuteMethodList(Type myType)
        {
            var myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var item in myArrayMethodInfo)
            {
                item?.Invoke(null, null);
            }

        }

    }
}
