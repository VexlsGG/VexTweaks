# VexTweaks

**VexTweaks** is a professional Windows optimization tool designed to enhance system performance, gaming experience, and privacy. Built with modern .NET 8 and WPF, it provides a clean, dark-themed interface with safe, reversible system tweaks.

## 🌟 Features

### Core Functionality
- **System Optimization**: Apply performance tweaks to reduce system overhead
- **Gaming Optimizations**: Specialized tweaks for gaming with reduced latency
- **Network Optimization**: Reduce network throttling and improve latency (Pro)
- **Privacy Controls**: Disable telemetry, tracking, and advertising features
- **Profile System**: Apply preset profiles or create custom configurations
- **Licensing System**: Hardware-bound licensing with Free and Pro tiers

### Safety & Reversibility
- **Automatic Backups**: All tweaks are backed up before application
- **One-Click Revert**: Easily revert any applied tweak
- **Action Logging**: All changes are logged for audit trail
- **User Consent**: Clear descriptions of what each tweak does

### Modern UI/UX
- **Dark Theme**: Professional, modern dark interface
- **Sidebar Navigation**: Easy access to all features
- **Card-Based Layout**: Clean, organized tweak presentation
- **Animated Toggles**: Smooth animations for better UX
- **Status Indicators**: Real-time status for each tweak
- **Tagging System**: Recommended, Advanced, and "May Reduce Features" tags

## 🎯 Optimization Categories

### Performance
- Disable Visual Effects
- Disable Transparency
- Optimize Processor Scheduling
- Remove Startup Delay
- Optimize Memory Management
- Disable Prefetch/Superfetch
- Disable Windows Search Indexing

### Gaming
- Disable Game Bar & Game DVR
- Gaming Priority Mode
- Hardware-Accelerated GPU Scheduling
- Disable Fullscreen Optimizations
- Disable Mouse Smoothing

### Network (Pro Features)
- Reduce Network Throttling
- Disable Nagle's Algorithm
- Optimize Network Adapter Settings

### Privacy
- Disable Telemetry
- Disable Activity History
- Disable Location Tracking
- Disable Advertising ID

## 📋 Built-in Profiles

### ⚡ Performance Mode
Optimizes system for maximum performance by:
- Disabling visual effects
- Removing startup delays
- Optimizing processor scheduling
- Improving memory management

### 🎮 Gaming Mode
Optimizes for gaming with:
- Disabled Game Bar and DVR
- Gaming priority settings
- GPU scheduling optimization
- Reduced network latency (Pro)
- Disabled Nagle's algorithm (Pro)

### 📹 Streaming Mode
Balanced optimization for content creation:
- Processor scheduling optimization
- Memory optimization
- Network optimization (Pro)
- GPU scheduling

## 🔐 License System

### Free Tier
- All basic performance tweaks
- Gaming optimizations (basic)
- Privacy controls
- Built-in profiles

### Pro Tier
- Advanced network optimizations
- All advanced tweaks unlocked
- Custom profile creation
- Profile export/import
- Priority support
- Lifetime updates

**License Format**: `VXT-XXXX-XXXX-XXXX`

The license system includes:
- Hardware-bound activation
- Online validation (API-ready)
- Offline graceful fallback
- Easy activation/deactivation

## 🛠️ Technical Stack

- **Framework**: .NET 8.0
- **UI**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Dependencies**:
  - CommunityToolkit.Mvvm (8.2.2)
  - Microsoft.Extensions.DependencyInjection (8.0.0)
  - Newtonsoft.Json (13.0.3)
  - System.Management (8.0.0)

## 📁 Project Structure

```
VexTweaks/
├── Models/              # Data models (Tweak, License, Profile, etc.)
├── Services/            # Business logic services
│   ├── LicenseService   # License management
│   ├── TweakService     # Tweak application/reversion
│   ├── LoggingService   # Action logging
│   ├── ProfileService   # Profile management
│   └── SystemInfoService # System information gathering
├── ViewModels/          # MVVM ViewModels
├── Views/               # UI Views (XAML)
├── Resources/           # Styles, themes, images
└── Helpers/             # Utility classes and converters
```

## 🚀 Building from Source

### Prerequisites
- .NET 8.0 SDK or later
- Windows 10/11 (for testing)
- Visual Studio 2022 or JetBrains Rider (recommended)

### Build Steps

1. Clone the repository:
```bash
git clone https://github.com/VexlsGG/VexTweaks.git
cd VexTweaks
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build --configuration Release
```

4. Run the application:
```bash
dotnet run --project VexTweaks/VexTweaks.csproj
```

## 📦 Deployment

The application is built as a standalone Windows executable (`.exe`). For production deployment:

1. Build in Release mode
2. Publish as self-contained:
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

3. The executable will be in `bin/Release/net8.0-windows/win-x64/publish/`

## ⚠️ Requirements

- **Operating System**: Windows 10 (1809+) or Windows 11
- **Architecture**: x64
- **Permissions**: Administrator rights required for system tweaks
- **.NET Runtime**: Included in self-contained builds

## 🛡️ Safety Information

VexTweaks is designed with safety in mind:

- ✅ **No Malware**: Clean, auditable source code
- ✅ **Reversible**: All tweaks can be reverted
- ✅ **Transparent**: Clear descriptions of all changes
- ✅ **Logged**: All actions are logged
- ✅ **User Consent**: No hidden changes

**Note**: While all tweaks are designed to be safe, always create a system restore point before making system changes. Some tweaks may reduce certain Windows features to improve performance.

## 🔄 Updating

Future versions will include an auto-update framework. For now:

1. Check releases page for updates
2. Download the latest version
3. Install over existing version (settings are preserved in AppData)

## 📝 License Key Validation

The license system is API-ready and can be integrated with:
- Lemon Squeezy
- Gumroad  
- Stripe
- Custom API endpoints

Hardware IDs are generated using CPU and motherboard information for binding licenses to specific machines.

## 🙏 Credits

Developed by **VexlsGG**

- YouTube: [VexlsGG](https://youtube.com/@VexlsGG)
- Twitter/X: [@VexlsGG](https://twitter.com/VexlsGG)
- Twitch: [VexlsGG](https://twitch.tv/VexlsGG)

## 📄 Legal

Copyright © 2025 VexlsGG. All rights reserved.

This software is provided for legitimate system optimization purposes. Users are responsible for ensuring compliance with their system's terms of service and local regulations.

## 🆘 Support

For support:
1. Check the [Issues](https://github.com/VexlsGG/VexTweaks/issues) page
2. Join the community on Discord (coming soon)
3. Contact via YouTube/Twitter for Pro license holders

## 🚧 Roadmap

- [ ] Windows Installer (MSI/Setup.exe)
- [ ] Auto-update system
- [ ] Service management UI
- [ ] Startup program manager
- [ ] Custom profile creation UI
- [ ] Restore point creation
- [ ] Advanced logging dashboard
- [ ] Multi-language support
- [ ] Portable mode

---

**Made with ❤️ by VexlsGG**

