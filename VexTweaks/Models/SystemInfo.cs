namespace VexTweaks.Models;

/// <summary>
/// System information model
/// </summary>
public class SystemInfo
{
    public string CpuName { get; set; } = string.Empty;
    public int CpuCores { get; set; }
    public double CpuUsage { get; set; }
    
    public string RamTotal { get; set; } = string.Empty;
    public string RamAvailable { get; set; } = string.Empty;
    public double RamUsagePercent { get; set; }
    
    public string GpuName { get; set; } = string.Empty;
    
    public string WindowsVersion { get; set; } = string.Empty;
    public string WindowsBuild { get; set; } = string.Empty;
    
    public int OptimizationScore { get; set; }
}
