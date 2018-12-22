using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Util.IP
{
    /// <summary>
    /// IP操作类
    /// </summary>
    public static class IpUtil
    {
        /// <summary>
        /// 是否是合法IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip) || ip.Length < 7 || ip.Length > 15) return false;

            string regformat = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(ip);

        }

        /// <summary>
        /// 是否是合法端口
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsIpPort(int port)
        {
            return port > 0 && port <= 65535;
        }

        /// <summary>
        /// 获取本机所有IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetIpList()
        {
            string name = Dns.GetHostName();
            IPAddress[] list = Dns.GetHostAddresses(name);

            return list;
        }

        /// <summary>
        /// 获取IPV4
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetIpV4List()
        {
            List<IPAddress> list = new List<IPAddress>();
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    list.Add(ipa);
            }
            return list.ToArray()
;
        }

        /// <summary>
        /// 获取当前使用的IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            string result = Util.Process.StartCmd("route", "print");
            Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            if (m.Success)
            {
                return m.Groups[2].Value;
            }
            else
            {
                try

                {
                    System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient();
                    c.Connect("www.baidu.com", 80);
                    string ip = ((System.Net.IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
                    c.Close();
                    return ip;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>  
        /// 获取本机主DNS  
        /// </summary>  
        /// <returns></returns>  
        public static string GetPrimaryDNS()
        {
            string result = Util.Process.StartCmd("nslookup", "");
            Match m = Regex.Match(result, @"\d+\.\d+\.\d+\.\d+");
            if (m.Success)
            {
                return m.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Ip详细信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IpInfo GetIpInfo(string ip)
        {
            string postDataAddress = string.Format("http://ip.taobao.com/service/getIpInfo.php?ip={0}", ip);
            JObject jsonData = JObject.Parse(Web.WebUtil.Get(postDataAddress));

            if (int.Parse(jsonData["code"].ToString()) != 0)
            {
                return null;
            }
            string data = jsonData["data"].ToString();

            var IpInfo = Json.Deseriailze<IpInfo>(data);

            return IpInfo;
        }
    }
}
