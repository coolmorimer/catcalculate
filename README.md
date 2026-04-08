# 🐱 CatCalculate

A cute cat-themed calculator built with **WPF / .NET 8** for Windows.

Features a pastel colour palette, cat ears, paw-print decorations, and a borderless rounded window.

---

## Features

- Basic arithmetic: **+**, **−**, **×**, **÷**
- Percentage (`%`), sign toggle (`±`), and backspace (`⌫`)
- Chained operations (e.g. `3 + 4 × 2`)
- Error handling for division by zero
- Drag-to-move borderless window with rounded corners

---

## Running from Source

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (Windows)

### Run

```powershell
dotnet run --project CatCalculate.csproj
```

---

## Building a Standalone Executable (.exe)

A single self-contained `.exe` can be produced — no .NET runtime needs to be installed on the target machine.

### Option A – PowerShell (recommended)

```powershell
.\build\publish.ps1
```

### Option B – Double-click batch file

Double-click `build\publish.cmd` in Windows Explorer.

### Option C – Manual `dotnet publish`

```powershell
dotnet publish CatCalculate.csproj `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    --output build\output
```

In all cases the executable is written to **`build\output\CatCalculate.exe`**.

---

## Project Structure

```
catcalculate/
├── CatCalculate.csproj        # .NET 8 WPF project file
├── App.xaml / App.xaml.cs     # Application entry point
├── src/
│   ├── logic/
│   │   └── Calculator.cs      # Core arithmetic logic
│   └── ui/
│       ├── MainWindow.xaml    # Window layout
│       ├── MainWindow.xaml.cs # Code-behind / event handlers
│       └── Styles.xaml        # Cat-themed style resources
└── build/
    ├── publish.ps1            # PowerShell publish script
    └── publish.cmd            # Batch wrapper (double-click)
```
