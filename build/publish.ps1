# CatCalculate – publish single-file Windows executable
#
# Usage:
#   .\build\publish.ps1
#
# Output: build\output\CatCalculate.exe   (self-contained, no .NET install required)

param(
    [string]$Configuration = "Release",
    [string]$Runtime       = "win-x64"
)

$ErrorActionPreference = "Stop"

$repoRoot   = Split-Path -Parent $PSScriptRoot
$outputDir  = Join-Path $PSScriptRoot "output"

Write-Host "==> Restoring NuGet packages..." -ForegroundColor Cyan
dotnet restore "$repoRoot\CatCalculate.csproj"

Write-Host "==> Publishing single-file executable ($Runtime, $Configuration)..." -ForegroundColor Cyan
dotnet publish "$repoRoot\CatCalculate.csproj" `
    --configuration $Configuration `
    --runtime $Runtime `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    --output "$outputDir"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Publish failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "==> Build successful!" -ForegroundColor Green
Write-Host "    Executable: $outputDir\CatCalculate.exe" -ForegroundColor Green
