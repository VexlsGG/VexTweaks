using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VexTweaks.Models;
using VexTweaks.Services;

namespace VexTweaks.ViewModels;

public partial class TweaksViewModel : ObservableObject
{
    private readonly ITweakService _tweakService;
    private readonly ILicenseService _licenseService;
    private List<Tweak> _allTweaks = new();
    
    [ObservableProperty]
    private ObservableCollection<Tweak> tweaks = new();
    
    [ObservableProperty]
    private string filterCategory = "All";
    
    [ObservableProperty]
    private bool isProVersion;

    public TweaksViewModel(ITweakService tweakService, ILicenseService licenseService)
    {
        _tweakService = tweakService;
        _licenseService = licenseService;
        LoadTweaksAsync();
    }

    private async void LoadTweaksAsync()
    {
        var license = await _licenseService.GetCurrentLicenseAsync();
        IsProVersion = license.Tier == LicenseTier.Pro && license.IsValid;
        
        _allTweaks = await _tweakService.GetAllTweaksAsync();
        
        foreach (var tweak in _allTweaks)
        {
            tweak.IsApplied = await _tweakService.CheckTweakStatusAsync(tweak);
        }

        FilterTweaks();
    }

    partial void OnFilterCategoryChanged(string value)
    {
        FilterTweaks();
    }

    private void FilterTweaks()
    {
        Tweaks.Clear();

        var filteredTweaks = FilterCategory == "All" 
            ? _allTweaks 
            : _allTweaks.Where(t => t.Category == FilterCategory).ToList();

        foreach (var tweak in filteredTweaks)
        {
            Tweaks.Add(tweak);
        }
    }

    [RelayCommand]
    private async Task ApplyTweakAsync(Tweak tweak)
    {
        if (tweak.RequiresPro && !IsProVersion)
        {
            // Show pro message
            return;
        }

        var success = await _tweakService.ApplyTweakAsync(tweak);
        if (success)
        {
            tweak.IsApplied = true;
        }
    }

    [RelayCommand]
    private async Task RevertTweakAsync(Tweak tweak)
    {
        var success = await _tweakService.RevertTweakAsync(tweak);
        if (success)
        {
            tweak.IsApplied = false;
        }
    }

    [RelayCommand]
    private async Task ToggleTweakAsync(Tweak tweak)
    {
        if (tweak.IsApplied)
        {
            await RevertTweakAsync(tweak);
        }
        else
        {
            await ApplyTweakAsync(tweak);
        }
    }
}

