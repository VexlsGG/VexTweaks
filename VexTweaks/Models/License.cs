namespace VexTweaks.Models;

/// <summary>
/// Represents a software license
/// </summary>
public class License
{
    public string LicenseKey { get; set; } = string.Empty;
    public LicenseTier Tier { get; set; } = LicenseTier.Free;
    public DateTime? ExpirationDate { get; set; }
    public bool IsValid { get; set; }
    public string HardwareId { get; set; } = string.Empty;
    public DateTime ActivationDate { get; set; }
    public string Email { get; set; } = string.Empty;
}

public enum LicenseTier
{
    Free,
    Pro
}
