using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VexTweaks.Services;
using VexTweaks.ViewModels;

namespace VexTweaks;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Services
        services.AddSingleton<ILicenseService, LicenseService>();
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<ITweakService, TweakService>();
        services.AddSingleton<ISystemInfoService, SystemInfoService>();
        services.AddSingleton<IProfileService, ProfileService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<TweaksViewModel>();
        services.AddTransient<LicenseViewModel>();

        // MainWindow
        services.AddSingleton<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}


