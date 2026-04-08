@echo off
REM CatCalculate – build single-file Windows executable
REM Double-click this file or run it from a Developer Command Prompt / PowerShell.
REM Output will be placed in build\output\CatCalculate.exe

echo.
echo === CatCalculate Build Script ===
echo.

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
