# 🐱 CatCalculate

A cute, cat-themed desktop calculator built with **WPF (.NET 8)**.

## Features

- Standard arithmetic: `+`, `−`, `×`, `÷`
- Percent (`%`), sign toggle (`±`), and backspace (`⌫`)
- Chain calculations (e.g. `5 + 3 × 2`)
- Expression history shown above the result
- Draggable, borderless rounded window with cat ears 🐾
- Hover and press animations on all buttons
- Single-file, self-contained Windows EXE — no installation needed

## Download

Head to the [**Actions**](../../actions/workflows/build.yml) tab, pick the latest successful run, and download the **CatCalculate-win-x64** artifact. Extract and double-click `CatCalculate.exe`.

## Build from source

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (Windows only — WPF)

### Run in development

```bash
dotnet run --project CatCalculate.csproj
```

### Publish self-contained EXE

```bash
dotnet publish CatCalculate.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -o publish/
```

The resulting `publish/CatCalculate.exe` can be copied and run on any 64-bit Windows machine without installing .NET.

## Project structure

```
CatCalculate.csproj        — .NET 8 WPF project file
src/
  logic/
    Calculator.cs          — Core arithmetic logic (no UI dependency)
  ui/
    App.xaml / .cs         — WPF application entry point
    MainWindow.xaml / .cs  — Main window layout + event handlers
    Styles.xaml            — Cat-themed colour palette and control styles
.github/workflows/
  build.yml                — CI: build + publish EXE on every push to main
```

## License

MIT
