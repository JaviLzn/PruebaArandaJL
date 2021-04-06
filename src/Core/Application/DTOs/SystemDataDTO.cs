using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Application.DTOs
{
    public class SystemDataDTO
    {
        private readonly ISystemGatherDataService service;
        public SystemDataDTO(ISystemGatherDataService service)
        {
            this.service = service;
        }

        public string OperatingSystemName => service.GetOperatingSystem();
        public string MachineName => service.GetMachineName();
        public IEnumerable<string> IpAdresses => service.GetIPAdress();
        public string CpuName => service.GetCpuName();
        public IEnumerable<string> Disks => service.GetHardDiskCapacity();
        public string MemoryRam => service.GetRAMCapacity();
        public string ReportDate => DateTime.Now.ToString("dd-MMMM-yyyy HH:mm:ss");

        public string ToString(bool writeIndented = true)
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = writeIndented });
        }
    }
}
