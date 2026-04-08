@echo off
REM CatCalculate – build single-file Windows executable
REM Double-click this file or run it from a Developer Command Prompt / PowerShell.
REM Output will be placed in build\output\CatCalculate.exe

echo.
echo === CatCalculate Build Script ===
echo.

REM Validate that the .NET SDK is installed before proceeding.
where dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK not found.
    echo.
    echo The .NET SDK is required to build CatCalculate.
    echo Please download and install it from: https://aka.ms/dotnet-download
    echo.
    echo After installing, open a new terminal window and try again.
    pause
    exit /b 1
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0publish.ps1"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Build FAILED. See errors above.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Done! Executable is at: %~dp0output\CatCalculate.exe
pause
