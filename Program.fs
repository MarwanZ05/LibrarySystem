module Program

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.Hosts
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Elmish
open Gui
open Storage
open UI

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Library Management System"
        base.Width <- 800.0
        base.Height <- 600.0
        
        // This is where we run the Elmish program
        Elmish.Program.mkSimple init update view
        |> Elmish.Program.withHost this
        |> Elmish.Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Light

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()
        base.OnFrameworkInitializationCompleted()

[<EntryPoint>]
let main argv =
    if Array.contains "cli" argv || Array.contains "--cli" argv then
        let library = Storage.loadLibrary()
        UI.mainLoop library
    else
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .StartWithClassicDesktopLifetime(argv)
