# StartScreen

> The project is being left for a Metro version. For a truly Metro UI, and because of my skill issues.
> (actually it's harder to do some stuff with non .NET API it seems)

A Recreation of the Windows 8.x Start Screen

Made with .NET C# using WPF and WinForms

## Differences with ScreenLaunch and StartScreen

### StartScreen
The main app that has the StartScreen Component

This is has the Tile System and other stuff

Made using WPF C# .NET

### ScreenLaunch
The app that launches the StartScreen app

This is also used to check if the User has clicked the start button or not

Made using WinForms C# .NET

## Known Issues
- High Memory Usage when first launch, it stays around ~100mb after a few seconds
- (Atleast for me) All Apps lags in battery mode on laptop, aka when not plugged in<br>
  Probably due to so much items in that Menu
## How-to
To use this as a start menu, use open-shell custom commands
## To-Do
- Use p/invoke or native code to replace the actual start menu instead of using Open-Shell
- Do some optimization for the ram usage issue
## Any help with the code/assets is appreciated
