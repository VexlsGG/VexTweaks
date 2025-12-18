using Microsoft.Win32;
using VexTweaks.Models;

namespace VexTweaks.Services;

/// <summary>
/// Service for applying and reverting system tweaks
/// </summary>
public interface ITweakService
{
    Task<bool> ApplyTweakAsync(Tweak tweak);
    Task<bool> RevertTweakAsync(Tweak tweak);
    Task<bool> CheckTweakStatusAsync(Tweak tweak);
    Task<List<Tweak>> GetAllTweaksAsync();
}

public class TweakService : ITweakService
{
    private readonly ILoggingService _loggingService;

    public TweakService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task<bool> ApplyTweakAsync(Tweak tweak)
    {
        try
        {
            switch (tweak.Type)
            {
                case TweakType.Registry:
                    return await ApplyRegistryTweakAsync(tweak);
                case TweakType.Service:
                    return await ApplyServiceTweakAsync(tweak);
                case TweakType.PowerPlan:
                    return await ApplyPowerPlanTweakAsync(tweak);
                default:
                    return false;
            }
        }
        catch (Exception ex)
        {
            await _loggingService.LogTweakActionAsync(tweak, TweakAction.Apply, false, ex.Message);
            return false;
        }
    }

    public async Task<bool> RevertTweakAsync(Tweak tweak)
    {
        try
        {
            switch (tweak.Type)
            {
                case TweakType.Registry:
                    return await RevertRegistryTweakAsync(tweak);
                case TweakType.Service:
                    return await RevertServiceTweakAsync(tweak);
                case TweakType.PowerPlan:
                    return await RevertPowerPlanTweakAsync(tweak);
                default:
                    return false;
            }
        }
        catch (Exception ex)
        {
            await _loggingService.LogTweakActionAsync(tweak, TweakAction.Revert, false, ex.Message);
            return false;
        }
    }

    public async Task<bool> CheckTweakStatusAsync(Tweak tweak)
    {
        try
        {
            switch (tweak.Type)
            {
                case TweakType.Registry:
                    return await CheckRegistryTweakAsync(tweak);
                case TweakType.Service:
                    return await CheckServiceTweakAsync(tweak);
                default:
                    return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Tweak>> GetAllTweaksAsync()
    {
        // Return predefined tweaks
        return await Task.FromResult(GetPredefinedTweaks());
    }

    private async Task<bool> ApplyRegistryTweakAsync(Tweak tweak)
    {
        if (!tweak.Configuration.ContainsKey("RegistryPath") ||
            !tweak.Configuration.ContainsKey("ValueName") ||
            !tweak.Configuration.ContainsKey("Value"))
        {
            return false;
        }

        var path = tweak.Configuration["RegistryPath"].ToString()!;
        var valueName = tweak.Configuration["ValueName"].ToString()!;
        var value = tweak.Configuration["Value"];
        var valueType = tweak.Configuration.ContainsKey("ValueType") 
            ? tweak.Configuration["ValueType"].ToString() 
            : "DWord";

        // Backup current value
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(path, false);
            if (key != null)
            {
                var currentValue = key.GetValue(valueName);
                if (currentValue != null)
                {
                    tweak.BackupData["OriginalValue"] = currentValue;
                }
            }
        }
        catch
        {
            // Key might not exist
        }

        // Apply new value
        using var writeKey = Registry.LocalMachine.CreateSubKey(path, true);
        
        switch (valueType)
        {
            case "DWord":
                writeKey.SetValue(valueName, Convert.ToInt32(value), RegistryValueKind.DWord);
                break;
            case "String":
                writeKey.SetValue(valueName, value.ToString()!, RegistryValueKind.String);
                break;
            case "QWord":
                writeKey.SetValue(valueName, Convert.ToInt64(value), RegistryValueKind.QWord);
                break;
        }

        tweak.IsApplied = true;
        await _loggingService.LogTweakActionAsync(tweak, TweakAction.Apply, true);
        return true;
    }

    private async Task<bool> RevertRegistryTweakAsync(Tweak tweak)
    {
        if (!tweak.Configuration.ContainsKey("RegistryPath") ||
            !tweak.Configuration.ContainsKey("ValueName"))
        {
            return false;
        }

        var path = tweak.Configuration["RegistryPath"].ToString()!;
        var valueName = tweak.Configuration["ValueName"].ToString()!;

        using var key = Registry.LocalMachine.OpenSubKey(path, true);
        if (key == null)
            return false;

        if (tweak.BackupData.ContainsKey("OriginalValue"))
        {
            var originalValue = tweak.BackupData["OriginalValue"];
            var valueType = tweak.Configuration.ContainsKey("ValueType") 
                ? tweak.Configuration["ValueType"].ToString() 
                : "DWord";

            switch (valueType)
            {
                case "DWord":
                    key.SetValue(valueName, Convert.ToInt32(originalValue), RegistryValueKind.DWord);
                    break;
                case "String":
                    key.SetValue(valueName, originalValue.ToString()!, RegistryValueKind.String);
                    break;
                case "QWord":
                    key.SetValue(valueName, Convert.ToInt64(originalValue), RegistryValueKind.QWord);
                    break;
            }
        }
        else
        {
            // No backup, delete the value
            key.DeleteValue(valueName, false);
        }

        tweak.IsApplied = false;
        await _loggingService.LogTweakActionAsync(tweak, TweakAction.Revert, true);
        return true;
    }

    private async Task<bool> CheckRegistryTweakAsync(Tweak tweak)
    {
        if (!tweak.Configuration.ContainsKey("RegistryPath") ||
            !tweak.Configuration.ContainsKey("ValueName") ||
            !tweak.Configuration.ContainsKey("Value"))
        {
            return false;
        }

        var path = tweak.Configuration["RegistryPath"].ToString()!;
        var valueName = tweak.Configuration["ValueName"].ToString()!;
        var expectedValue = tweak.Configuration["Value"];

        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(path, false);
            if (key == null)
                return false;

            var currentValue = key.GetValue(valueName);
            if (currentValue == null)
                return false;

            return currentValue.ToString() == expectedValue.ToString();
        }
        catch
        {
            return false;
        }
    }

    private Task<bool> ApplyServiceTweakAsync(Tweak tweak)
    {
        // Service management would go here
        return Task.FromResult(true);
    }

    private Task<bool> RevertServiceTweakAsync(Tweak tweak)
    {
        // Service management would go here
        return Task.FromResult(true);
    }

    private Task<bool> CheckServiceTweakAsync(Tweak tweak)
    {
        // Service checking would go here
        return Task.FromResult(false);
    }

    private Task<bool> ApplyPowerPlanTweakAsync(Tweak tweak)
    {
        // Power plan management would go here
        return Task.FromResult(true);
    }

    private Task<bool> RevertPowerPlanTweakAsync(Tweak tweak)
    {
        // Power plan management would go here
        return Task.FromResult(true);
    }

    private List<Tweak> GetPredefinedTweaks()
    {
        return new List<Tweak>
        {
            new()
            {
                Id = "disable-visual-effects",
                Name = "Disable Visual Effects",
                Description = "Disables unnecessary visual effects for better performance",
                Category = "Performance",
                Tag = TweakTag.Recommended,
                Type = TweakType.Registry,
                RequiresAdmin = true,
                RequiresPro = false,
                Configuration = new Dictionary<string, object>
                {
                    ["RegistryPath"] = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects",
                    ["ValueName"] = "VisualFXSetting",
                    ["Value"] = 2,
                    ["ValueType"] = "DWord"
                }
            },
            new()
            {
                Id = "disable-game-bar",
                Name = "Disable Game Bar",
                Description = "Disables Windows Game Bar to reduce background processes",
                Category = "Gaming",
                Tag = TweakTag.Recommended,
                Type = TweakType.Registry,
                RequiresAdmin = true,
                RequiresPro = false,
                Configuration = new Dictionary<string, object>
                {
                    ["RegistryPath"] = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR",
                    ["ValueName"] = "AppCaptureEnabled",
                    ["Value"] = 0,
                    ["ValueType"] = "DWord"
                }
            },
            new()
            {
                Id = "disable-game-dvr",
                Name = "Disable Game DVR",
                Description = "Disables Game DVR recording feature",
                Category = "Gaming",
                Tag = TweakTag.Recommended,
                Type = TweakType.Registry,
                RequiresAdmin = true,
                RequiresPro = false,
                Configuration = new Dictionary<string, object>
                {
                    ["RegistryPath"] = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR",
                    ["ValueName"] = "GameDVR_Enabled",
                    ["Value"] = 0,
                    ["ValueType"] = "DWord"
                }
            },
            new()
            {
                Id = "reduce-network-throttling",
                Name = "Reduce Network Throttling",
                Description = "Reduces network throttling for better online gaming",
                Category = "Network",
                Tag = TweakTag.Advanced,
                Type = TweakType.Registry,
                RequiresAdmin = true,
                RequiresPro = true,
                Configuration = new Dictionary<string, object>
                {
                    ["RegistryPath"] = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    ["ValueName"] = "NetworkThrottlingIndex",
                    ["Value"] = 0xffffffff,
                    ["ValueType"] = "DWord"
                }
            },
            new()
            {
                Id = "disable-transparency",
                Name = "Disable Transparency Effects",
                Description = "Disables window transparency for performance",
                Category = "Performance",
                Tag = TweakTag.Recommended,
                Type = TweakType.Registry,
                RequiresAdmin = false,
                RequiresPro = false,
                Configuration = new Dictionary<string, object>
                {
                    ["RegistryPath"] = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    ["ValueName"] = "EnableTransparency",
                    ["Value"] = 0,
                    ["ValueType"] = "DWord"
                }
            }
        };
    }
}
