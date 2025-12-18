namespace VexTweaks.Models;

/// <summary>
/// Represents a system optimization profile
/// </summary>
public class OptimizationProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsBuiltIn { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public List<string> TweakIds { get; set; } = new();
}

public static class BuiltInProfiles
{
    public static OptimizationProfile Performance => new()
    {
        Id = "performance",
        Name = "Performance Mode",
        Description = "Optimizes system for maximum performance",
        IsBuiltIn = true,
        TweakIds = new List<string>
        {
            "disable-visual-effects",
            "disable-transparency",
            "optimize-processor-scheduling",
            "disable-startup-delay",
            "disable-prefetch",
            "optimize-memory"
        }
    };

    public static OptimizationProfile Gaming => new()
    {
        Id = "gaming",
        Name = "Gaming Mode",
        Description = "Optimizes system for gaming with reduced latency",
        IsBuiltIn = true,
        TweakIds = new List<string>
        {
            "gaming-priority",
            "disable-game-bar",
            "disable-game-dvr",
            "optimize-gpu-scheduling",
            "reduce-network-throttling",
            "disable-nagle-algorithm",
            "optimize-mouse-smoothing",
            "disable-fullscreen-optimizations"
        }
    };

    public static OptimizationProfile Streaming => new()
    {
        Id = "streaming",
        Name = "Streaming Mode",
        Description = "Balanced optimization for streaming and content creation",
        IsBuiltIn = true,
        TweakIds = new List<string>
        {
            "optimize-processor-scheduling",
            "optimize-memory",
            "reduce-network-throttling",
            "optimize-gpu-scheduling",
            "disable-game-bar"
        }
    };
}
