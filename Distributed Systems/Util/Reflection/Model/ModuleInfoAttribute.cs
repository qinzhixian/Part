using System;

namespace Util.Reflection.Model
{
    /// <summary>
    /// 模块描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleInfoAttribute : Attribute
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public String ModuleName { get; set; }

        /// <summary>
        /// 子模块名称
        /// </summary>
        public String SubModuleName { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="subModuleName">子模块名称</param>
        public ModuleInfoAttribute(String moduleName, String subModuleName)
        {
            this.ModuleName = moduleName;
            this.SubModuleName = subModuleName;
        }
    }
}
