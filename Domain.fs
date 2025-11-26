module Domain

open System

type BookId = int

type Book = {
    Id: BookId
    Title: string
    Author: string
    IsBorrowed: bool
}

type Library = {
    Books: Book list
    NextId: int
}

let emptyLibrary = { Books = []; NextId = 1 }
