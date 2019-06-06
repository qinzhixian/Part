using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Reflection.Attribute
{
    /// <summary>
    /// 方法注释类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MethodInfoAttribute : System.Attribute
    {
        /// <summary>
        /// 方法功能
        /// </summary>
        public String FunctionRemarks { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public String ParameterRemarks { get; set; }

        /// <summary>
        /// 返回值说明
        /// </summary>
        public String ReturnRemarks { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public String ModifyDate { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="remark">说明</param>
        /// <param name="parame">参数</param>
        /// <param name="returnRemark">返回值</param>
        /// <param name="modifyDate">修改时间</param>
        public MethodInfoAttribute(string remark, string parame = "", string returnRemark = "", string modifyDate = "")
        {
            this.FunctionRemarks = remark;
            this.ParameterRemarks = parame;
            this.ReturnRemarks = returnRemark;
            this.ModifyDate = modifyDate;
        }

    }
}
