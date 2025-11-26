# Library Management System

A console-based Library Management System written in F#.

## Features
- **Add Book**: Add new books with title and author.
- **Search Books**: Find books by title or author.
- **Borrow/Return**: Manage book availability.
- **List Books**: View all books and their status.
- **Persistence**: Data is automatically saved to `library.json`.

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)

## How to Run

1.  **Open a terminal** (Command Prompt, PowerShell, or your preferred terminal).
2.  **Navigate to the project directory**:
    ```bash
    cd [path]
    ```
3.  **Run the application**:
    ```bash
    dotnet run
    ```

## How to Build
To compile the project without running it:
```bash
dotnet build
```

## Project Structure
- `Domain.fs`: Core data types (`Book`, `Library`).
- `Storage.fs`: JSON file handling.
- `Operations.fs`: Business logic (Add, Search, Borrow, Return).
- `UI.fs`: User interface and menu loop.
- `Program.fs`: Application entry point.
