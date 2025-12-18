using System.IO;
using VexTweaks.Models;

namespace VexTweaks.Services;

/// <summary>
/// Service for managing optimization profiles
/// </summary>
public interface IProfileService
{
    Task<List<OptimizationProfile>> GetAllProfilesAsync();
    Task<OptimizationProfile?> GetProfileByIdAsync(string id);
    Task<bool> SaveProfileAsync(OptimizationProfile profile);
    Task<bool> DeleteProfileAsync(string id);
    Task<bool> ApplyProfileAsync(string profileId);
    Task<string> ExportProfileAsync(string profileId);
    Task<OptimizationProfile?> ImportProfileAsync(string json);
}

public class ProfileService : IProfileService
{
    private const string ProfilesFileName = "profiles.json";
    private readonly ITweakService _tweakService;
    private List<OptimizationProfile> _customProfiles = new();

    public ProfileService(ITweakService tweakService)
    {
        _tweakService = tweakService;
        LoadProfilesAsync().Wait();
    }

    public async Task<List<OptimizationProfile>> GetAllProfilesAsync()
    {
        var profiles = new List<OptimizationProfile>
        {
            BuiltInProfiles.Performance,
            BuiltInProfiles.Gaming,
            BuiltInProfiles.Streaming
        };

        profiles.AddRange(_customProfiles);

        return await Task.FromResult(profiles);
    }

    public async Task<OptimizationProfile?> GetProfileByIdAsync(string id)
    {
        var profiles = await GetAllProfilesAsync();
        return profiles.FirstOrDefault(p => p.Id == id);
    }

    public async Task<bool> SaveProfileAsync(OptimizationProfile profile)
    {
        if (profile.IsBuiltIn)
            return false;

        var existing = _customProfiles.FirstOrDefault(p => p.Id == profile.Id);
        if (existing != null)
        {
            _customProfiles.Remove(existing);
        }

        _customProfiles.Add(profile);
        await SaveProfilesAsync();
        return true;
    }

    public async Task<bool> DeleteProfileAsync(string id)
    {
        var profile = _customProfiles.FirstOrDefault(p => p.Id == id);
        if (profile == null)
            return false;

        _customProfiles.Remove(profile);
        await SaveProfilesAsync();
        return true;
    }

    public async Task<bool> ApplyProfileAsync(string profileId)
    {
        var profile = await GetProfileByIdAsync(profileId);
        if (profile == null)
            return false;

        var allTweaks = await _tweakService.GetAllTweaksAsync();
        var success = true;

        foreach (var tweakId in profile.TweakIds)
        {
            var tweak = allTweaks.FirstOrDefault(t => t.Id == tweakId);
            if (tweak != null)
            {
                var result = await _tweakService.ApplyTweakAsync(tweak);
                if (!result)
                    success = false;
            }
        }

        return success;
    }

    public async Task<string> ExportProfileAsync(string profileId)
    {
        var profile = await GetProfileByIdAsync(profileId);
        if (profile == null)
            return string.Empty;

        return Newtonsoft.Json.JsonConvert.SerializeObject(profile, Newtonsoft.Json.Formatting.Indented);
    }

    public async Task<OptimizationProfile?> ImportProfileAsync(string json)
    {
        try
        {
            var profile = Newtonsoft.Json.JsonConvert.DeserializeObject<OptimizationProfile>(json);
            if (profile != null)
            {
                profile.IsBuiltIn = false;
                profile.Id = Guid.NewGuid().ToString();
                await SaveProfileAsync(profile);
                return profile;
            }
        }
        catch
        {
            // Invalid JSON
        }

        return null;
    }

    private async Task LoadProfilesAsync()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks"
        );

        var profilesPath = Path.Combine(appDataPath, ProfilesFileName);

        if (File.Exists(profilesPath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(profilesPath);
                _customProfiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OptimizationProfile>>(json) ?? new();
            }
            catch
            {
                _customProfiles = new();
            }
        }
    }

    private async Task SaveProfilesAsync()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks"
        );

        Directory.CreateDirectory(appDataPath);

        var profilesPath = Path.Combine(appDataPath, ProfilesFileName);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(_customProfiles, Newtonsoft.Json.Formatting.Indented);
        await File.WriteAllTextAsync(profilesPath, json);
    }
}
