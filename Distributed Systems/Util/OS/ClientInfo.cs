using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Util.OS
{
    /// <summary>
    /// 计算机信息帮助类
    /// </summary>
    public static class ComputerInfo
    {

        /// <summary>  
        /// 获取本机 机器名   
        /// </summary>  
        /// <returns></returns>  
        public static string GetMachineName()
        {
            return Environment.GetEnvironmentVariable("COMPUTERNAME");
        }

        /// <summary>  
        /// 获取本机的MAC地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetLocalMac()
        {
            string mac = null;
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return (mac);
        }

        /// <summary>
        /// 取得设备硬盘的卷标号
        /// </summary>
        /// <returns></returns>
        public static string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        /// <summary>
        /// 获得CPU的序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        /// <summary>
        /// 硬盘ID,大小
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDisk()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                string HDid = (string)mo.Properties["Model"].Value;
                string bytes = mo["Size"].ToString();
                dic.Add(HDid, bytes);
            }
            moc = null;
            mc = null;
            return dic;
        }

        /// <summary>
        /// 获取声卡
        /// </summary>
        /// <returns></returns>
        public static string SoundDevice()
        {
            ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_SoundDevice");
            string ret = null;
            foreach (ManagementObject obj in objvide.Get())
            {
                ret = obj["Name"].ToString();
                //string a = "Name - " + obj["Name"];
                //string a1 = "DeviceID - " + obj["DeviceID"];
                //string a2 = "AdapterRAM - " + obj["AdapterRAM"];
                //string a3 = "AdapterDACType - " + obj["AdapterDACType"] ;
                //string a4 = "Monochrome - " + obj["Monochrome"];
                //string a5 = "InstalledDisplayDrivers - " + obj["InstalledDisplayDrivers"];
                //string a6 = "DriverVersion - " + obj["DriverVersion"];
                //string a7 = "VideoProcessor - " + obj["VideoProcessor"];
                //string a8 = "VideoArchitecture - " + obj["VideoArchitecture"];
                //string a9 = "VideoMemoryType - " + obj["VideoMemoryType"];
            }
            return ret;
        }

        /// <summary>
        /// 获取显卡
        /// </summary>
        public static string GetVGACard()
        {
            ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_VideoController");
            string ret = null;
            foreach (ManagementObject obj in objvide.Get())
            {
                ret = obj["Name"].ToString();
                //string a = "Name - " + obj["Name"];
                //string a1 = "DeviceID - " + obj["DeviceID"];
                //string a2 = "AdapterRAM - " + obj["AdapterRAM"];
                //string a3 = "AdapterDACType - " + obj["AdapterDACType"] ;
                //string a4 = "Monochrome - " + obj["Monochrome"];
                //string a5 = "InstalledDisplayDrivers - " + obj["InstalledDisplayDrivers"];
                //string a6 = "DriverVersion - " + obj["DriverVersion"];
                //string a7 = "VideoProcessor - " + obj["VideoProcessor"];
                //string a8 = "VideoArchitecture - " + obj["VideoArchitecture"];
                //string a9 = "VideoMemoryType - " + obj["VideoMemoryType"];
            }
            return ret;
        }

        /// <summary>
        /// 获取操作系统
        /// </summary>
        /// <returns></returns>
        public static string GetSys()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        /// <summary>
        /// 电脑类型
        /// </summary>
        /// <returns></returns>
        public static ChassisTypes GetComputerType()
        {
            ManagementClass systemEnclosures = new ManagementClass("Win32_SystemEnclosure");
            foreach (ManagementObject obj in systemEnclosures.GetInstances())
            {
                foreach (int i in (UInt16[])(obj["ChassisTypes"]))
                {
                    if (i > 0 && i < 25)
                    {
                        return (ChassisTypes)i;
                    }
                }
            }
            return ChassisTypes.Unknown;
        }

        /// <summary>  
        /// PC位数
        /// </summary>  
        /// <returns></returns>  
        public static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }

        }

        /// <summary>  
        /// 操作系统的登录用户名  
        /// </summary>  
        /// <returns></returns>  
        public static string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["AddressWidth"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

    }

    public enum ChassisTypes
    {
        Other = 1,
        Unknown,
        Desktop,
        LowProfileDesktop,
        PizzaBox,
        MiniTower,
        Tower,
        Portable,
        Laptop,
        Notebook,
        Handheld,
        DockingStation,
        AllInOne,
        SubNotebook,
        SpaceSaving,
        LunchBox,
        MainSystemChassis,
        ExpansionChassis,
        SubChassis,
        BusExpansionChassis,
        PeripheralChassis,
        StorageChassis,
        RackMountChassis,
        SealedCasePC
    }
}
