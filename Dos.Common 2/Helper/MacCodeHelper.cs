using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Dos.Common.Helper
{
    public class MacCodeHelper
    {

        public static string getMacAddress8()
        {
            var t = EncryptHelper.MD5Encrypt(getMacAddress(),32);
            return t.Substring(0, 8).ToUpper();
        }

        /// <summary> 
        /// 获取MAC地址(返回第一个物理以太网卡的mac地址) 
        /// </summary> 
        /// <returns>成功返回mac地址，失败返回null</returns> 
        public static string getMacAddress()
        {
            string macAddress = null;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet")) //是以太网卡
                    {
                        string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                        if (rk != null)
                        {
                            // 区分 PnpInstanceID     
                            // 如果前面有 PCI 就是本机的真实网卡    
                            // MediaSubType 为 01 则是常见网卡，02为无线网卡。    
                            string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                            int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                            if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI") //是物理网卡
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                return macAddress;
                            }
                            else if (fMediaSubType == 1) //虚拟网卡
                                continue;
                            else if (fMediaSubType == 2) //无线网卡(上面判断Ethernet时已经排除了)
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                continue;
                            }
                        }
                    }
                }
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType.ToString().StartsWith("Wireless")) //是无线网卡
                    {
                        string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                        if (rk != null)
                        {
                            // 区分 PnpInstanceID     
                            // 如果前面有 PCI 就是本机的真实网卡    
                            // MediaSubType 为 01 则是常见网卡，02为无线网卡。    
                            string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                            int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                            if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI") //是物理网卡
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                return macAddress;
                            }
                            else if (fMediaSubType == 1) //虚拟网卡
                                continue;
                            else if (fMediaSubType == 2) //无线网卡(上面判断Ethernet时已经排除了)
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                return macAddress;
                            }
                        }
                    }
                }
            }
            catch
            {
                macAddress = "";
            }
            return macAddress;
        }
    }
}
