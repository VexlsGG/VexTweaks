#!/bin/bash
# VexTweaks Build Script (Linux/macOS)
# This script builds VexTweaks in Release mode

echo "========================================"
echo "VexTweaks Build Script"
echo "========================================"
echo ""

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK is not installed or not in PATH"
    echo "Please install .NET 8.0 SDK from https://dotnet.microsoft.com/"
    exit 1
fi

echo "[1/4] Cleaning previous builds..."
dotnet clean VexTweaks/VexTweaks.csproj -c Release
if [ $? -ne 0 ]; then
    echo "ERROR: Clean failed"
    exit 1
fi

echo ""
echo "[2/4] Restoring NuGet packages..."
dotnet restore VexTweaks/VexTweaks.csproj
if [ $? -ne 0 ]; then
    echo "ERROR: Restore failed"
    exit 1
fi

echo ""
echo "[3/4] Building VexTweaks in Release mode..."
dotnet build VexTweaks/VexTweaks.csproj -c Release --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed"
    exit 1
fi

echo ""
echo "[4/4] Publishing self-contained executable..."
dotnet publish VexTweaks/VexTweaks.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false --output ./Publish
if [ $? -ne 0 ]; then
    echo "ERROR: Publish failed"
    exit 1
fi

echo ""
echo "========================================"
echo "Build completed successfully!"
echo "========================================"
echo ""
echo "Output location: $(pwd)/Publish"
echo "Main executable: VexTweaks.exe"
echo ""
echo "The application is now ready for distribution."
echo ""
