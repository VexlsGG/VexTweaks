using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VexTweaks.Models;
using VexTweaks.Services;

namespace VexTweaks.ViewModels;

public partial class LicenseViewModel : ObservableObject
{
    private readonly ILicenseService _licenseService;
    
    [ObservableProperty]
    private string licenseKey = string.Empty;
    
    [ObservableProperty]
    private string email = string.Empty;
    
    [ObservableProperty]
    private License? currentLicense;
    
    [ObservableProperty]
    private string statusMessage = string.Empty;
    
    [ObservableProperty]
    private bool isActivating;

    public LicenseViewModel(ILicenseService licenseService)
    {
        _licenseService = licenseService;
        LoadLicenseAsync();
    }

    private async void LoadLicenseAsync()
    {
        CurrentLicense = await _licenseService.GetCurrentLicenseAsync();
    }

    [RelayCommand]
    private async Task ActivateLicenseAsync()
    {
        if (string.IsNullOrWhiteSpace(LicenseKey) || string.IsNullOrWhiteSpace(Email))
        {
            StatusMessage = "Please enter both license key and email";
            return;
        }

        IsActivating = true;
        StatusMessage = "Activating...";

        var success = await _licenseService.ActivateLicenseAsync(LicenseKey, Email);

        IsActivating = false;

        if (success)
        {
            StatusMessage = "License activated successfully!";
            CurrentLicense = await _licenseService.GetCurrentLicenseAsync();
        }
        else
        {
            StatusMessage = "Invalid license key. Please check and try again.";
        }
    }

    [RelayCommand]
    private async Task DeactivateLicenseAsync()
    {
        await _licenseService.DeactivateLicenseAsync();
        CurrentLicense = await _licenseService.GetCurrentLicenseAsync();
        StatusMessage = "License deactivated";
    }
}
