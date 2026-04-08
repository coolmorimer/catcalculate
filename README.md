# 🐱 CatCalculate

A cute, cat-themed desktop calculator built with **WPF (.NET 8)**.

Features a pastel colour palette, cat ears, paw-print decorations, and a borderless rounded window.

---

## Features

- Standard arithmetic: `+`, `−`, `×`, `÷`
- Percent (`%`), sign toggle (`±`), and backspace (`⌫`)
- Chain calculations (e.g. `5 + 3 × 2`)
- Expression history shown above the result
- Error handling for division by zero
- Draggable, borderless rounded window with cat ears 🐾
- Hover and press animations on all buttons
- Single-file, self-contained Windows EXE — no installation needed

---

## Download

Head to the [**Actions**](../../actions/workflows/build.yml) tab, pick the latest successful run, and download the **CatCalculate-win-x64** artifact. Extract and double-click `CatCalculate.exe`.

---

## Running from Source

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (Windows only — WPF)

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
├── src/
│   ├── logic/
│   │   └── Calculator.cs      # Core arithmetic logic
│   └── ui/
│       ├── App.xaml / .cs     # WPF application entry point
│       ├── MainWindow.xaml    # Window layout
│       ├── MainWindow.xaml.cs # Code-behind / event handlers
│       └── Styles.xaml        # Cat-themed style resources
├── build/
│   ├── publish.ps1            # PowerShell publish script
│   └── publish.cmd            # Batch wrapper (double-click)
└── .github/workflows/
    └── build.yml              # CI: build + publish EXE on every push to main
```

## License

MIT
