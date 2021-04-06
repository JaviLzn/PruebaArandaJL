using Application.Interfaces;
using Hardware.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Application.Services
{
    public class SystemGatherDataService : ISystemGatherDataService
    {
        private readonly HardwareInfo hardwareInfo;
        public SystemGatherDataService()
        {
            hardwareInfo = new HardwareInfo();
        }

        public string GetOperatingSystem()
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        }

        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        public IEnumerable<string> GetIPAdress()
        {
            var listIP = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    listIP.Add(ip.ToString());
                }
            }
            return listIP;
        }

        public string GetCpuName()
        {
            hardwareInfo.RefreshCPUList();
            return hardwareInfo.CpuList[0].Name.Trim();
        }

        public IEnumerable<string> GetHardDiskCapacity()
        {
            var list = new List<string>();
            foreach (DriveInfo disk in DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Fixed))
            {
                var capacityTotalGB = disk.TotalSize / (double)(1024 * 1024 * 1024);
                var capacityFreeGB = disk.TotalFreeSpace / (double)(1024 * 1024 * 1024);
                var capacityUsageGB = capacityTotalGB - capacityFreeGB;
                var percentageUsage = 100 * (capacityUsageGB) / capacityTotalGB;
                percentageUsage = Math.Round(percentageUsage, 0);
                capacityTotalGB = Math.Round(capacityTotalGB, 0);
                list.Add($"{capacityTotalGB} GB, Porcentaje de uso: {percentageUsage}%");
            }
            return list.Distinct();
        }

        public string GetRAMCapacity()
        {
            hardwareInfo.RefreshMemoryStatus();
            var capacityAvailable = hardwareInfo.MemoryStatus.AvailablePhysical / (double)(1024 * 1024 * 1024);
            var capacityTotal = hardwareInfo.MemoryStatus.TotalPhysical / (double)(1024 * 1024 * 1024);
            var capacityUsage = capacityTotal - capacityAvailable;
            var percentageUsage = 100 * (capacityUsage) / capacityTotal;
            percentageUsage = Math.Round(percentageUsage, 0);
            capacityTotal = Math.Round(capacityTotal, 0);
            return $"{capacityTotal} GB, Porcentaje de uso: {percentageUsage}%";
        }
    }
}
