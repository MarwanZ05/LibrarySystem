
module Operations

open Domain

let addBook (title: string) (author: string) (library: Library) =
    let newBook = {
        Id = library.NextId
        Title = title
        Author = author
        IsBorrowed = false
    }
    { library with 
        Books = library.Books @ [newBook]
        NextId = library.NextId + 1 }

let removeBook (id: BookId) (library: Library) =
    { library with Books = library.Books |> List.filter (fun b -> b.Id <> id) }


let searchBooks (query: string) (library: Library) =
    library.Books 
    |> List.filter (fun b -> 
        b.Title.Contains(query, System.StringComparison.OrdinalIgnoreCase) || 
        b.Author.Contains(query, System.StringComparison.OrdinalIgnoreCase))

let borrowBook (id: BookId) (library: Library) =
    let updateBook b =
        if b.Id = id && not b.IsBorrowed then { b with IsBorrowed = true } else b
    
    let newBooks = library.Books |> List.map updateBook
    // Check if any change happened to know if borrow was successful could be done differently, 
    // but for now just returning the new state.
    { library with Books = newBooks }

let returnBook (id: BookId) (library: Library) =
    let updateBook b =
        if b.Id = id && b.IsBorrowed then { b with IsBorrowed = false } else b
    { library with Books = library.Books |> List.map updateBook }
