module UI

open System
open Domain
open Operations
open Storage

let rec mainLoop (library: Library) =
    Console.Clear()
    printfn "=== Library Management System ==="
    printfn "1. Add Book"
    printfn "2. Search Books"
    printfn "3. Borrow Book"
    printfn "4. Return Book"
    printfn "5. List All Books"
    printfn "6. Exit"
    printf "Select an option: "

    match Console.ReadLine() with
    | "1" ->
        printf "Enter Title: "
        let title = Console.ReadLine()
        printf "Enter Author: "
        let author = Console.ReadLine()
        let newLibrary = addBook title author library
        saveLibrary newLibrary
        printfn "Book added!"
        System.Threading.Thread.Sleep(1000)
        mainLoop newLibrary
    | "2" ->
        printf "Enter search query: "
        let query = Console.ReadLine()
        let results = searchBooks query library
        results |> List.iter (fun b -> printfn "%d: %s by %s [%s]" b.Id b.Title b.Author (if b.IsBorrowed then "Borrowed" else "Available"))
        printfn "Press any key to continue..."
        Console.ReadKey() |> ignore
        mainLoop library
    | "3" ->
        printfn "Available Books:"
        library.Books |> List.iter (fun b -> printfn "%d: %s by %s [%s]" b.Id b.Title b.Author (if b.IsBorrowed then "Borrowed" else "Available"))
        printfn ""
        printf "Enter Book ID to borrow: "
        let newLibrary =
            match Int32.TryParse(Console.ReadLine()) with
            | true, id ->
                let lib = borrowBook id library
                saveLibrary lib
                printfn "Operation processed."
                lib
            | _ -> 
                printfn "Invalid ID."
                library
        System.Threading.Thread.Sleep(1000)
        mainLoop newLibrary
    | "4" ->
        printfn "Borrowed Books:"
        library.Books 
        |> List.filter (fun b -> b.IsBorrowed)
        |> List.iter (fun b -> printfn "%d: %s by %s" b.Id b.Title b.Author)
        printfn ""
        printf "Enter Book ID to return: "
        let newLibrary = 
            match Int32.TryParse(Console.ReadLine()) with
            | true, id ->
                let lib = returnBook id library
                saveLibrary lib
                printfn "Operation processed."
                lib
            | _ -> 
                printfn "Invalid ID."
                library
        System.Threading.Thread.Sleep(1000)
        mainLoop newLibrary
    | "5" ->
        library.Books |> List.iter (fun b -> printfn "%d: %s by %s [%s]" b.Id b.Title b.Author (if b.IsBorrowed then "Borrowed" else "Available"))
        printfn "Press any key to continue..."
        Console.ReadKey() |> ignore
        mainLoop library
    | "6" ->
        printfn "Goodbye!"
        0
    | _ ->
        mainLoop library
