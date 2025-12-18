using CommunityToolkit.Mvvm.ComponentModel;
using VexTweaks.Models;
using VexTweaks.Services;

namespace VexTweaks.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ISystemInfoService _systemInfoService;
    
    [ObservableProperty]
    private SystemInfo? systemInfo;
    
    [ObservableProperty]
    private string optimizationScoreText = "Calculating...";

    public DashboardViewModel(ISystemInfoService systemInfoService)
    {
        _systemInfoService = systemInfoService;
        LoadSystemInfoAsync();
    }

    private async void LoadSystemInfoAsync()
    {
        SystemInfo = await _systemInfoService.GetSystemInfoAsync();
        OptimizationScoreText = $"{SystemInfo.OptimizationScore}%";
    }
}
