# Contributing to VexTweaks

Thank you for your interest in contributing to VexTweaks! This document provides guidelines and information for contributors.

## 🤝 How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:
- Clear description of the bug
- Steps to reproduce
- Expected vs actual behavior
- Windows version and system specs
- Screenshots if applicable

### Suggesting Features

Feature requests are welcome! Please include:
- Clear description of the feature
- Use case and benefits
- Any relevant examples or mockups

### Code Contributions

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/your-feature-name`
3. **Make your changes** following our coding standards
4. **Test thoroughly** on Windows 10 and 11 if possible
5. **Commit with clear messages**: `git commit -m "Add: feature description"`
6. **Push to your fork**: `git push origin feature/your-feature-name`
7. **Create a Pull Request**

## 📝 Coding Standards

### C# Code Style
- Use modern C# features (records, pattern matching, etc.)
- Follow Microsoft's C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and small

### XAML Style
- Use consistent indentation (4 spaces)
- Organize properties logically (layout, appearance, behavior)
- Use resource dictionaries for reusable styles
- Follow WPF best practices

### Architecture
- Follow MVVM pattern strictly
- Keep ViewModels testable (no UI logic)
- Use dependency injection
- Services should be stateless when possible
- Models should be simple DTOs

## 🧪 Testing

- Test all new features on Windows 10 (latest) and Windows 11
- Verify admin elevation works correctly
- Ensure tweaks are fully reversible
- Test with both Free and Pro licenses
- Verify no crashes or exceptions

## 🔒 Security

- Never commit sensitive data (API keys, passwords, etc.)
- Validate all user input
- Use secure coding practices
- Report security issues privately to the maintainers

## 📋 Adding New Tweaks

When adding new system tweaks:

1. **Document thoroughly**: Explain what the tweak does
2. **Make it reversible**: Always store backup data
3. **Test extensively**: Verify on multiple Windows versions
4. **Use appropriate tags**: Recommended, Advanced, or "May Reduce Features"
5. **Add to correct category**: Performance, Gaming, Network, Privacy, etc.
6. **Consider Pro/Free tier**: Decide if it should be a Pro feature

Example tweak structure:
```csharp
new()
{
    Id = "unique-tweak-id",
    Name = "Descriptive Tweak Name",
    Description = "Clear explanation of what this does and why",
    Category = "Performance", // or Gaming, Network, Privacy
    Tag = TweakTag.Recommended, // or Advanced, MayReduceFeatures
    Type = TweakType.Registry,
    RequiresAdmin = true,
    RequiresPro = false,
    Configuration = new Dictionary<string, object>
    {
        ["RegistryPath"] = @"HKLM\Path\To\Key",
        ["ValueName"] = "ValueName",
        ["Value"] = 1,
        ["ValueType"] = "DWord"
    }
}
```

## 🎨 UI Guidelines

- Follow the dark theme design
- Use existing styles from DarkTheme.xaml
- Maintain consistent spacing and padding
- Add animations where appropriate
- Test on different screen resolutions
- Ensure accessibility (keyboard navigation, etc.)

## 📚 Documentation

- Update README.md for major features
- Add XML comments to public APIs
- Update the changelog
- Include screenshots for UI changes

## 🚀 Release Process

1. Update version number in VexTweaks.csproj
2. Update CHANGELOG.md
3. Create a release tag
4. Build release binaries
5. Create GitHub release with notes

## ❓ Questions?

If you have questions about contributing:
- Open a discussion on GitHub
- Check existing issues and pull requests
- Contact the maintainers

## 📜 Code of Conduct

- Be respectful and professional
- Welcome newcomers and help them learn
- Focus on constructive feedback
- No harassment, discrimination, or inappropriate behavior

## 🙏 Recognition

All contributors will be recognized in:
- README.md contributors section
- Release notes
- About dialog in the application

Thank you for helping make VexTweaks better! 🎉
