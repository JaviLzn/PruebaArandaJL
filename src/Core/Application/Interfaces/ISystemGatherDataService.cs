using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ISystemGatherDataService
    {
        string GetOperatingSystem();
        string GetMachineName();
        IEnumerable<string> GetIPAdress();
        string GetCpuName();
        IEnumerable<string> GetHardDiskCapacity();
        string GetRAMCapacity();
    }
}
