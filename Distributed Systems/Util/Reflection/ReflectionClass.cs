using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Reflection
{
    public class ReflectionClass
    {
        private object classObject;

        internal ReflectionClass(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj","参数不能为null。");
            }
            this.classObject = obj;
        }

        /// <summary>
        /// 执行实例方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public object CallInstanceMethod(string methodName)
        {
            return this.CallInstanceMethod(methodName, Type.EmptyTypes, null);
        }

        /// <summary>
        /// 执行实例方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object CallInstanceMethod(string methodName, params object[] param)
        {
            return this.CallInstanceMethod(methodName, ReflectionUtil.GetParamTypes(param), param);
        }

        /// <summary>
        /// 执行实例方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="paramTypes"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object CallInstanceMethod(string methodName, Type[] paramTypes, params object[] param)
        {
            return this.classObject.GetType().GetMethod(methodName, paramTypes)?.Invoke(this.classObject, param);
        }



    }
}
