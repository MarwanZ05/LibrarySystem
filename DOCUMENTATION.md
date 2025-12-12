# Library System Documentation

## Overview
The **Library Management System** is a robust, cross-platform application designed to manage book inventories, borrowing limits, and library users. Built with **F#**, it leverages functional programming paradigms to ensure reliability and maintainability.

The application features a **Dual-Interface Architecture**, allowing users to interact with the system via a modern **Graphical User Interface (GUI)** or a lightweight **Command Line Interface (CLI)**.

## System Architecture

The project follows a localized **Onion Architecture** (or Clean Architecture) variant, keeping the core domain logic pure and isolated from side effects like UI or Storage.

### 1. Domain Layer (`Domain.fs`)
The heart of the system. It defines the immutable data structures used throughout the application. It contains no external dependencies or logic.

*   **Logic**: None (Type definitions only).
*   **Key Types**:
    *   `Book`: Represents a single book entity with `Id`, `Title`, `Author`, and `IsBorrowed` status.
    *   `Library`: The aggregate root containing a list of `Books` and the `NextId` counter for auto-incrementing IDs.

### 2. Business Logic Layer (`Operations.fs`)
Contains **pure functions** that implement the library's business rules. These functions accept the current state (Library) and parameters, returning a new state.

*   **Key Operations**:
    *   `addBook`: Creates a new book with a unique ID and appends it to the library.
    *   `removeBook`: Filters out a book by its ID.
    *   `borrowBook`: Marks a book as borrowed *only if* it is currently available.
    *   `returnBook`: Marks a book as available *only if* it is currently borrowed.
    *   `searchBooks`: Performs a case-insensitive search on both Title and Author fields.

### 3. Data Persistence Layer (`Storage.fs`)
Handles the serialization and deserialization of the application state.

*   **Format**: JSON (`library.json`).
*   **Mechanism**:
    *   **Loading**: On startup, it attempts to read `library.json`. If the file is missing or corrupt, it initializes an empty library.
    *   **Saving**: Triggered automatically after **every** state-changing operation (Add, Borrow, Return, Remove) to ensure data safety.
*   **DTO Pattern**: Uses an internal `LibraryDto` to handle mapping between F# distinct types and JSON-friendly structures (e.g., converting `List` to `Array`).

### 4. Presentation Layer
The system provides two distinct entry points for user interaction, both consuming the same underlying Core and Storage layers.

#### A. Graphical User Interface (`Gui.fs`)
Built with **Avalonia.FuncUI**, implementing the **Elmish (Model-View-Update)** architecture.
*   **Pattern**: MVU (Unidirectional Data Flow).
*   **Components**:
    *   `State`: Immutably holds the current Library, UI selection state, and Form inputs.
    *   `Msg`: A discriminated union defining all possible UI events (e.g., `AddBookSubmit`, `SearchInputChanged`).
    *   `update`: Pure function reducing `Msg + State -> New State`.
    *   `view`: Declarative UI layout code.

#### B. Command Line Interface (`UI.fs`)
A looped console application for headless interaction.
*   **Structure**: A recursive `mainLoop` function.
*   **Flow**: Displays menu -> Reads Input -> Executes Operation -> Saves State -> Recurses with new State.

### 5. Entry Point (`Program.fs`)
The composition root. It parses command-line arguments to determine which presentation layer to launch.
*   **Default**: Launches GUI (`AppBuilder`).
*   **CLI Mode**: Launches CLI if arguments contain `--cli` or `cli`.

---

## Usage Guide

### Running the Application
Check `README.md` for build instructions.

*   **GUI Mode**: `dotnet run`
*   **CLI Mode**: `dotnet run -- cli`

### CLI Menu Structure
When running in CLI mode, the user is presented with the following options:

1.  **Add Book**: Prompts for Title and Author to create a new entry.
2.  **Search Books**: Interactive search query.
3.  **Borrow Book**: Lists available books and prompts for an ID to borrow.
4.  **Return Book**: Lists borrowed books and prompts for an ID to return.
5.  **List All Books**: Dumps the entire inventory to the console.
6.  **Exit**: Terminates the application.

### GUI Features
*   **Inventory Tab**: View all books, with color-coded status (Green = Available, Red = Borrowed).
*   **Search Bar**: Real-time filtering of the inventory list.
*   **Admin Panel**: Dedicated tab for adding new books or removing existing ones from the collection.

---

## Extensibility Guide

To add a new feature (e.g., "Edit Book"):

1.  **Domain**: Update `Domain.fs` type if new data fields are needed.
2.  **Logic**: Add a pure `editBook` function in `Operations.fs`.
3.  **CLI**: Add a new case "7. Edit Book" in `UI.fs` match expression, calling the operation.
4.  **GUI**:
    *   Add `EditBook` message to `Msg` type in `Gui.fs`.
    *   Handle the message in `update` function.
    *   Add a Button/Form in `view` to trigger the message.

## Diagrams
Refer to the project root for visual representations:
*   `Block Diagram.png`: High-level component interaction.
*   `System Architecture Diagram.png`: Detailed structural breakdown.
