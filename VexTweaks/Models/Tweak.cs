namespace VexTweaks.Models;

/// <summary>
/// Represents a system optimization tweak
/// </summary>
public class Tweak
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public TweakTag Tag { get; set; } = TweakTag.None;
    public bool IsApplied { get; set; }
    public bool RequiresAdmin { get; set; }
    public bool RequiresPro { get; set; }
    public TweakType Type { get; set; }
    
    // Configuration data for the tweak
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    // Backup data for reverting
    public Dictionary<string, object> BackupData { get; set; } = new();
}

public enum TweakTag
{
    None,
    Recommended,
    Advanced,
    MayReduceFeatures
}

public enum TweakType
{
    Registry,
    Service,
    PowerPlan,
    Network,
    Startup,
    Custom
}
