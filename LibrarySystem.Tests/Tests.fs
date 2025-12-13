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

[<Fact>]
let ``removeBook should remove existing book`` () =
    // Arrange
    let book1 = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let book2 = { Id = 2; Title = "The Hobbit"; Author = "J.R.R. Tolkien"; IsBorrowed = false }
    let initialLib = { Books = [book1; book2]; NextId = 3 }

    // Act
    let result = removeBook 1 initialLib

    // Assert
    result.Books.Length |> should equal 1
    result.Books.Head.Id |> should equal 2

[<Fact>]
let ``removeBook should do nothing if book does not exist`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let initialLib = { Books = [book]; NextId = 2 }

    // Act
    let result = removeBook 99 initialLib

    // Assert
    result.Books.Length |> should equal 1
    result.Books.Head.Id |> should equal 1

[<Fact>]
let ``searchBooks should find book by partial title case insensitive`` () =
    // Arrange
    let book1 = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let book2 = { Id = 2; Title = "Dune Messiah"; Author = "Frank Herbert"; IsBorrowed = false }
    let initialLib = { Books = [book1; book2]; NextId = 3 }

    // Act
    let result = searchBooks "dune" initialLib

    // Assert
    result.Length |> should equal 2

[<Fact>]
let ``searchBooks should find book by author`` () =
    // Arrange
    let book1 = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let book2 = { Id = 2; Title = "The Hobbit"; Author = "J.R.R. Tolkien"; IsBorrowed = false }
    let initialLib = { Books = [book1; book2]; NextId = 3 }

    // Act
    let result = searchBooks "Tolkien" initialLib

    // Assert
    result.Length |> should equal 1
    result.Head.Title |> should equal "The Hobbit"

[<Fact>]
let ``searchBooks should return empty list if no match found`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let initialLib = { Books = [book]; NextId = 2 }

    // Act
    let result = searchBooks "Harry Potter" initialLib

    // Assert
    result |> should be Empty

[<Fact>]
let ``borrowBook should do nothing if book does not exist`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let initialLib = { Books = [book]; NextId = 2 }

    // Act
    let result = borrowBook 99 initialLib

    // Assert
    result.Books.Head.IsBorrowed |> should be False

[<Fact>]
let ``returnBook should do nothing if book is not borrowed`` () =
    // Arrange
    let book = { Id = 1; Title = "Dune"; Author = "Frank Herbert"; IsBorrowed = false }
    let initialLib = { Books = [book]; NextId = 2 }

    // Act
    let result = returnBook 1 initialLib

    // Assert
    result.Books.Head.IsBorrowed |> should be False
