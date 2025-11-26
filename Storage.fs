module Storage

open System.IO
open System.Text.Json
open Domain

let fileName = "library.json"

type LibraryDto = {
    Books: Book array
    NextId: int
}

let saveLibrary (library: Library) =
    let dto = { LibraryDto.Books = List.toArray library.Books; NextId = library.NextId }
    let json = JsonSerializer.Serialize(dto)
    File.WriteAllText(fileName, json)

let loadLibrary () =
    if File.Exists(fileName) then
        let json = File.ReadAllText(fileName)
        try
            let dto = JsonSerializer.Deserialize<LibraryDto>(json)
            { Library.Books = Array.toList dto.Books; NextId = dto.NextId }
        with
        | ex -> 
            printfn "Error loading library: %s" ex.Message
            emptyLibrary
    else
        emptyLibrary
