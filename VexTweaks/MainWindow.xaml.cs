using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using VexTweaks.Services;
using VexTweaks.ViewModels;
using VexTweaks.Views;

namespace VexTweaks;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILicenseService _licenseService;

    public MainWindow(IServiceProvider serviceProvider, ILicenseService licenseService)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _licenseService = licenseService;
        
        LoadLicenseStatus();
        NavigateToDashboard();
    }

    private async void LoadLicenseStatus()
    {
        var license = await _licenseService.GetCurrentLicenseAsync();
        LicenseStatusText.Text = license.Tier == Models.LicenseTier.Pro && license.IsValid 
            ? "Pro License" 
            : "Free Tier";
    }

    private void NavigationButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var page = button.Tag?.ToString();
            NavigateToPage(page);
        }
    }

    private void NavigateToPage(string? page)
    {
        UserControl? view = page switch
        {
            "Dashboard" => new DashboardView { DataContext = _serviceProvider.GetRequiredService<DashboardViewModel>() },
            "Performance" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "Gaming" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "Network" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "Privacy" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "Services" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "Startup" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "Profiles" => new PerformanceView { DataContext = _serviceProvider.GetRequiredService<TweaksViewModel>() },
            "License" => new LicenseView { DataContext = _serviceProvider.GetRequiredService<LicenseViewModel>() },
            "Settings" => new DashboardView { DataContext = _serviceProvider.GetRequiredService<DashboardViewModel>() },
            _ => new DashboardView { DataContext = _serviceProvider.GetRequiredService<DashboardViewModel>() }
        };

        ContentArea.Content = view;
    }

    private void NavigateToDashboard()
    {
        NavigateToPage("Dashboard");
    }
}
