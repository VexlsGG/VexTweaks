using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using VexTweaks.Models;

namespace VexTweaks.Services;

/// <summary>
/// Service for managing software licenses
/// </summary>
public interface ILicenseService
{
    Task<License> GetCurrentLicenseAsync();
    Task<bool> ValidateLicenseAsync(string licenseKey);
    Task<bool> ActivateLicenseAsync(string licenseKey, string email);
    Task<bool> DeactivateLicenseAsync();
    string GetHardwareId();
}

public class LicenseService : ILicenseService
{
    private const string LicenseFileName = "license.dat";
    private License? _currentLicense;

    public async Task<License> GetCurrentLicenseAsync()
    {
        if (_currentLicense != null)
            return _currentLicense;

        var licensePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks",
            LicenseFileName
        );

        if (File.Exists(licensePath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(licensePath);
                _currentLicense = Newtonsoft.Json.JsonConvert.DeserializeObject<License>(json);
                
                if (_currentLicense != null)
                {
                    // Validate hardware ID
                    if (_currentLicense.HardwareId != GetHardwareId())
                    {
                        _currentLicense.IsValid = false;
                    }
                    
                    // Check expiration
                    if (_currentLicense.ExpirationDate.HasValue && 
                        _currentLicense.ExpirationDate.Value < DateTime.Now)
                    {
                        _currentLicense.IsValid = false;
                    }
                    
                    return _currentLicense;
                }
            }
            catch
            {
                // Invalid license file
            }
        }

        // Return free tier license
        _currentLicense = new License
        {
            Tier = LicenseTier.Free,
            IsValid = true,
            HardwareId = GetHardwareId()
        };

        return _currentLicense;
    }

    public async Task<bool> ValidateLicenseAsync(string licenseKey)
    {
        // This is where you would call your license API
        // For now, implement basic validation structure
        
        if (string.IsNullOrWhiteSpace(licenseKey))
            return false;

        // TODO: Call API endpoint like:
        // var response = await httpClient.PostAsync($"{ApiUrl}/validate", ...);
        
        // For development, accept keys matching pattern: VXT-XXXX-XXXX-XXXX
        if (licenseKey.StartsWith("VXT-") && licenseKey.Length == 19)
        {
            return await Task.FromResult(true);
        }

        return false;
    }

    public async Task<bool> ActivateLicenseAsync(string licenseKey, string email)
    {
        var isValid = await ValidateLicenseAsync(licenseKey);
        
        if (!isValid)
            return false;

        var license = new License
        {
            LicenseKey = licenseKey,
            Tier = LicenseTier.Pro,
            IsValid = true,
            HardwareId = GetHardwareId(),
            ActivationDate = DateTime.Now,
            Email = email
        };

        // Save license
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks"
        );
        
        Directory.CreateDirectory(appDataPath);
        
        var licensePath = Path.Combine(appDataPath, LicenseFileName);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(license, Newtonsoft.Json.Formatting.Indented);
        await File.WriteAllTextAsync(licensePath, json);

        _currentLicense = license;
        return true;
    }

    public async Task<bool> DeactivateLicenseAsync()
    {
        var licensePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VexTweaks",
            LicenseFileName
        );

        if (File.Exists(licensePath))
        {
            File.Delete(licensePath);
        }

        _currentLicense = new License
        {
            Tier = LicenseTier.Free,
            IsValid = true,
            HardwareId = GetHardwareId()
        };

        return await Task.FromResult(true);
    }

    public string GetHardwareId()
    {
        try
        {
            // Generate hardware ID from CPU and motherboard info
            var cpuId = GetCpuId();
            var motherboardId = GetMotherboardId();
            
            var combined = $"{cpuId}-{motherboardId}";
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hash)[..16];
        }
        catch
        {
            // Fallback to machine name
            return Environment.MachineName;
        }
    }

    private string GetCpuId()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_Processor");
            using var instances = managementClass.GetInstances();
            
            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var result = obj.Properties["ProcessorId"].Value?.ToString() ?? "UNKNOWN";
                    return result;
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }
        
        return "UNKNOWN";
    }

    private string GetMotherboardId()
    {
        try
        {
            using var managementClass = new ManagementClass("Win32_BaseBoard");
            using var instances = managementClass.GetInstances();
            
            foreach (ManagementObject obj in instances)
            {
                try
                {
                    var result = obj.Properties["SerialNumber"].Value?.ToString() ?? "UNKNOWN";
                    return result;
                }
                finally
                {
                    obj?.Dispose();
                }
            }
        }
        catch
        {
            // Ignore
        }
        
        return "UNKNOWN";
    }
}
