namespace VexTweaks.Models;

/// <summary>
/// Represents a logged action for undo support
/// </summary>
public class TweakLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TweakId { get; set; } = string.Empty;
    public string TweakName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public TweakAction Action { get; set; }
    public Dictionary<string, object> BackupData { get; set; } = new();
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public enum TweakAction
{
    Apply,
    Revert
}
