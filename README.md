# Library Management System

A cross-platform Desktop Library Management System written in F# using **Avalonia.FuncUI**. For comprehensive architecture and usage documentation, see [DOCUMENTATION.md](DOCUMENTATION.md).

## Features
- **Library Tab**:
    - Browse all books.
    - Search by title or author.
    - Borrow available books.
    - Return borrowed books.
- **Admin Panel**:
    - Add new books via a dialog.
    - Remove books from the library.
- **Persistence**: Data is automatically saved to `library.json`.

## Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## How to Run

1.  **Open a terminal**.
2.  **Navigate to the project directory**:
    ```bash
    cd "[directory]"
    ```
3.  **Run the application**:
    - **GUI Mode** (Default):
        ```bash
        dotnet run
        ```
    - **CLI Mode**:
        ```bash
        dotnet run -- cli
        ```

## Testing
To run the automated tests:
```bash
dotnet test LibrarySystem.Tests
```

## Project Structure
- `Domain.fs`: Core data types (`Book`, `Library`).
- `Storage.fs`: JSON file handling.
- `Operations.fs`: Pure business logic (Add, Remove, Search, Borrow, Return).
- `Gui.fs`: Avalonia.FuncUI implementation (State, Msg, View).
- `Program.fs`: Application entry point (AppBuilder configuration).
