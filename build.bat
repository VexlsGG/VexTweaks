@echo off
REM VexTweaks Build Script
REM This script builds VexTweaks in Release mode and creates a distributable package

echo ========================================
echo VexTweaks Build Script
echo ========================================
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo Please install .NET 8.0 SDK from https://dotnet.microsoft.com/
    pause
    exit /b 1
)

echo [1/4] Cleaning previous builds...
dotnet clean VexTweaks/VexTweaks.csproj -c Release
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Clean failed
    pause
    exit /b 1
)

echo.
echo [2/4] Restoring NuGet packages...
dotnet restore VexTweaks/VexTweaks.csproj
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Restore failed
    pause
    exit /b 1
)

echo.
echo [3/4] Building VexTweaks in Release mode...
dotnet build VexTweaks/VexTweaks.csproj -c Release --no-restore
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo [4/4] Publishing self-contained executable...
dotnet publish VexTweaks/VexTweaks.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false --output ./Publish
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Publish failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo Output location: %CD%\Publish
echo Main executable: VexTweaks.exe
echo.
echo The application is now ready for distribution.
echo.
pause
