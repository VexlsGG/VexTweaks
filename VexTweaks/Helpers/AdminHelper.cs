using System.Diagnostics;
using System.Security.Principal;

namespace VexTweaks.Helpers;

/// <summary>
/// Helper class for managing administrator privileges
/// </summary>
public static class AdminHelper
{
    /// <summary>
    /// Check if the application is running with administrator privileges
    /// </summary>
    public static bool IsAdministrator()
    {
        try
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Restart the application with administrator privileges
    /// </summary>
    public static void RestartAsAdmin()
    {
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty,
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(processInfo);
            Environment.Exit(0);
        }
        catch
        {
            // User declined UAC prompt or other error
        }
    }

    /// <summary>
    /// Check if admin is required and prompt if necessary
    /// </summary>
    public static bool EnsureAdministrator(string message = "This application requires administrator privileges to apply system tweaks.")
    {
        if (!IsAdministrator())
        {
            var result = System.Windows.MessageBox.Show(
                message + "\n\nWould you like to restart as administrator?",
                "Administrator Required",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
            );

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                RestartAsAdmin();
                return false;
            }

            return false;
        }

        return true;
    }
}
