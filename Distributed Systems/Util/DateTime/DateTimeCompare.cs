using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.DateTime
{
    public class DateTimeCompare
    {
        /// <summary>
        /// 时间段不合理判断
        /// </summary>
        /// <param name="Start_dt">开始时间</param>
        /// <param name="End_dt">结束时间</param>
        /// <returns>True:不合理，False:合理</returns>
        public static Boolean CompareStartAndEnd(System.DateTime Start_dt, System.DateTime End_dt)
        {
            return System.DateTime.Compare(Start_dt, End_dt) >= 0;
        }

        /// <summary>
        /// 两个时间区间是否重叠
        /// </summary>
        /// <param name="basicStart_dt"></param>
        /// <param name="basicEnd_dt"></param>
        /// <param name="Start_dt"></param>
        /// <param name="End_dt"></param>
        /// <returns>True:重叠,False:不重叠</returns>
        public static Boolean IsOverrideTime(System.DateTime basicStart_dt, System.DateTime basicEnd_dt, System.DateTime Start_dt, System.DateTime End_dt)
        {
            return basicStart_dt <= End_dt && basicEnd_dt >= Start_dt;
        }
    }
}
