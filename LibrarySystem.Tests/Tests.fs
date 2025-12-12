module Tests

open Xunit
open FsUnit.Xunit
open Domain
open Operations

[<Fact>]
let ``addBook should add a book and increment ID`` () =
    // Arrange
    let initialLib = { Books = []; NextId = 1 }
    
    // Act
    let result = addBook "Dune" "Frank Herbert" initialLib
    
    // Assert
    result.NextId |> should equal 2
    result.Books.Length |> should equal 1
    result.Books.Head.Title |> should equal "Dune"
    result.Books.Head.Author |> should equal "Frank Herbert"
    result.Books.Head.IsBorrowed |> should be False

[<Fact>]
let ``borrowBook should mark book as borrowed`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let initialLib = { Books = [book]; NextId = 2 }
    
    // Act
    let result = borrowBook 1 initialLib
    
    // Assert
    result.Books.Head.IsBorrowed |> should be True

[<Fact>]
let ``borrowBook should not change already borrowed book`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = true }
    let initialLib = { Books = [book]; NextId = 2 }
    
    // Act
    let result = borrowBook 1 initialLib
    
    // Assert
    result.Books.Head.IsBorrowed |> should be True

[<Fact>]
let ``returnBook should mark book as not borrowed`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = true }
    let initialLib = { Books = [book]; NextId = 2 }
    
    // Act
    let result = returnBook 1 initialLib
    
    // Assert
    result.Books.Head.IsBorrowed |> should be False
