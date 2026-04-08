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

## Troubleshooting

### "No .NET SDKs were found" / build fails immediately

If the build script prints a message like:

```
No .NET SDKs were found.
Download a .NET SDK: https://aka.ms/dotnet-download
```

it means the .NET SDK is either not installed or not on your `PATH`.

**Step 1 – Check whether the SDK is installed**

Open a new PowerShell or Command Prompt window and run:

```powershell
dotnet --info
```

- If the command is recognised you will see a list of installed SDKs under the **".NET SDKs installed"** heading.  
  Make sure at least one SDK with a version starting with **8.x** is listed.
- If you get `'dotnet' is not recognized as an internal or external command` the SDK is not installed (or not on your `PATH`).

**Step 2 – Install the .NET 8 SDK**

Download and run the official installer for Windows:

👉 [Download .NET 8 SDK for Windows (x64)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

Select the **SDK** installer (not the Runtime) that matches your machine (x64 for most modern PCs).

**Step 3 – Restart your terminal and retry**

After installation, close all open PowerShell / Command Prompt windows, open a fresh one, and run `dotnet --info` again to confirm the SDK appears in the list.  
Then re-run the build script:

```powershell
.\build\publish.ps1
```

---

## License

MIT
