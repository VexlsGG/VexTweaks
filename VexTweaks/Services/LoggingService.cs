using System.IO;
using VexTweaks.Models;

namespace VexTweaks.Services;

/// <summary>
/// Service for logging tweak actions
/// </summary>
public interface ILoggingService
{
    Task LogTweakActionAsync(Tweak tweak, TweakAction action, bool success, string? errorMessage = null);
    Task<List<TweakLog>> GetLogsAsync();
    Task ClearLogsAsync();
}

public class LoggingService : ILoggingService
{
    private const string LogFileName = "tweaks.log";
    private readonly List<TweakLog> _logs = new();

    public async Task LogTweakActionAsync(Tweak tweak, TweakAction action, bool success, string? errorMessage = null)
    {
        var log = new TweakLog
        {
            TweakId = tweak.Id,
            TweakName = tweak.Name,
            Action = action,
            Success = success,
            ErrorMessage = errorMessage,
            BackupData = new Dictionary<string, object>(tweak.BackupData)
        };

        _logs.Add(log);

        // Save to file
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks"
        );
        
        Directory.CreateDirectory(appDataPath);
        
        var logPath = Path.Combine(appDataPath, LogFileName);
        var logText = $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] {action} - {tweak.Name} - {(success ? "SUCCESS" : $"FAILED: {errorMessage}")}\n";
        await File.AppendAllTextAsync(logPath, logText);
    }

    public Task<List<TweakLog>> GetLogsAsync()
    {
        return Task.FromResult(_logs.ToList());
    }

    public async Task ClearLogsAsync()
    {
        _logs.Clear();
        
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks",
            LogFileName
        );

        if (File.Exists(logPath))
        {
            File.Delete(logPath);
        }

        await Task.CompletedTask;
    }
}
