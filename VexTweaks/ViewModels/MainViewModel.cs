using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VexTweaks.Models;
using VexTweaks.Services;

namespace VexTweaks.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ILicenseService _licenseService;
    
    [ObservableProperty]
    private string currentPage = "Dashboard";
    
    [ObservableProperty]
    private License? currentLicense;
    
    [ObservableProperty]
    private bool isProVersion;

    public MainViewModel(ILicenseService licenseService)
    {
        _licenseService = licenseService;
        LoadLicenseAsync();
    }

    private async void LoadLicenseAsync()
    {
        CurrentLicense = await _licenseService.GetCurrentLicenseAsync();
        IsProVersion = CurrentLicense.Tier == LicenseTier.Pro && CurrentLicense.IsValid;
    }

    [RelayCommand]
    private void NavigateTo(string page)
    {
        CurrentPage = page;
    }
}
