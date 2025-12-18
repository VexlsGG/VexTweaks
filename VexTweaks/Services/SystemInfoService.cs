using System.Management;
using VexTweaks.Models;

namespace VexTweaks.Services;

/// <summary>
/// Service for gathering system information
/// </summary>
public interface ISystemInfoService
{
    Task<SystemInfo> GetSystemInfoAsync();
    Task<int> CalculateOptimizationScoreAsync();
}

public class SystemInfoService : ISystemInfoService
{
    private readonly ITweakService _tweakService;

    public SystemInfoService(ITweakService tweakService)
    {
        _tweakService = tweakService;
    }

    public async Task<SystemInfo> GetSystemInfoAsync()
    {
        var info = new SystemInfo
        {
            CpuName = GetCpuName(),
            CpuCores = Environment.ProcessorCount,
            RamTotal = GetTotalMemory(),
            GpuName = GetGpuName(),
            WindowsVersion = GetWindowsVersion(),
            WindowsBuild = GetWindowsBuild()
        };

        info.OptimizationScore = await CalculateOptimizationScoreAsync();

        return info;
    }

    public async Task<int> CalculateOptimizationScoreAsync()
    {
        var tweaks = await _tweakService.GetAllTweaksAsync();
        var appliedCount = 0;
        var totalCount = 0;

        foreach (var tweak in tweaks)
        {
            if (!tweak.RequiresPro) // Only count free tweaks for score
            {
                totalCount++;
                if (await _tweakService.CheckTweakStatusAsync(tweak))
                {
                    appliedCount++;
                }
            }
        }

        if (totalCount == 0)
            return 0;

        return (int)((appliedCount / (double)totalCount) * 100);
    }

    private string GetCpuName()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_Processor");
            using var instances = managementClass.GetInstances();

            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var result = obj.Properties["Name"].Value?.ToString()?.Trim() ?? "Unknown CPU";
                    return result;
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }

        return "Unknown CPU";
    }

    private string GetTotalMemory()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_ComputerSystem");
            using var instances = managementClass.GetInstances();

            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var bytes = Convert.ToInt64(obj.Properties["TotalPhysicalMemory"].Value);
                    var gb = bytes / (1024.0 * 1024.0 * 1024.0);
                    return $"{gb:F1} GB";
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }

        return "Unknown";
    }

    private string GetGpuName()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_VideoController");
            using var instances = managementClass.GetInstances();

            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var name = obj.Properties["Name"].Value?.ToString();
                    if (!string.IsNullOrEmpty(name) && !name.Contains("Microsoft Basic"))
                    {
                        return name;
                    }
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }

        return "Unknown GPU";
    }

    private string GetWindowsVersion()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_OperatingSystem");
            using var instances = managementClass.GetInstances();

            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var result = obj.Properties["Caption"].Value?.ToString() ?? "Unknown";
                    return result;
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }

        return "Unknown";
    }

    private string GetWindowsBuild()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_OperatingSystem");
            using var instances = managementClass.GetInstances();

            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var result = obj.Properties["BuildNumber"].Value?.ToString() ?? "Unknown";
                    return result;
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }

        return "Unknown";
    }
}
