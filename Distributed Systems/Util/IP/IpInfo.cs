using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.IP
{
    /// <summary>
    /// Ip详细信息
    /// </summary>
    public class IpInfo
    {
        /// <summary>
        /// IP
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        public string county { get; set; }

        /// <summary>
        /// 网络服务提供商
        /// </summary>
        public string isp { get; set; }

        /// <summary>
        /// 国家ID
        /// </summary>
        public string country_id { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public string area_id { get; set; }

        /// <summary>
        /// 省ID
        /// </summary>
        public string region_id { get; set; }

        /// <summary>
        /// 市ID
        /// </summary>
        public string city_id { get; set; }

        /// <summary>
        /// 县ID
        /// </summary>
        public string county_id { get; set; }

        /// <summary>
        /// 服务商ID
        /// </summary>
        public string isp_id { get; set; }
    }
}
