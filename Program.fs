open System
open Domain
open Storage
open UI

[<EntryPoint>]
let main argv =
    let library = loadLibrary()
    mainLoop library
