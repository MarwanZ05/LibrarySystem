# Library System Documentation

## Overview
The Library Management System is a functional desktop application built with F# and Avalonia.FuncUI. It adheres to the Elmish architecture (Model-View-Update), ensuring a predictable and immutable state management flow.

## Architecture

The system is modularized into layers:

1.  **Core (`Domain.fs`)**: Defines the fundamental types used throughout the system (`Book`, `Library`). It contains no logic.
2.  **Data Layer (`Storage.fs`)**: Handles persistence. It reads and writes the `Library` state to a `library.json` file.
3.  **Business Logic (`Operations.fs`)**: Contains pure functions that transform the `Library` state. These functions (e.g., `addBook`, `borrowBook`) take a state and parameters and return a new state.
4.  **Presentation (`Gui.fs`)**: Implements the UI logic using Avalonia.FuncUI.
    - **State**: Holds the current `Library`, selected tabs, form inputs, and selected items.
    - **Msg**: Represents all possible user interactions (e.g., `SelectLibraryBook`, `AddBookSubmit`).
    - **Update**: The reducer function that handles `Msg` to produce a new `State`.
    - **View**: Describes the UI layout code-behind-free.
5.  **Entry Point (`Program.fs`)**: Configures the Avalonia application and hosts the Elmish loop.

## Features

### Library Tab
The main interface for general users.
- **Inventory List**: Displays all books with their status (Available in Green, Borrowed in Red).
- **Search**: Real-time filtering of the book list.
- **Borrowing**: Users can select an available book and borrow it.
- **Returning**: Users can select a borrowed book and return it.
- **Validation**: Borrow/Return buttons are context-aware (enabled/disabled based on selection).

### Admin Panel
The interface for library administrators.
- **Management List**: Displays all books for management.
- **Add Book**: Renders a modal overlay dialog to input Title and Author.
- **Remove Book**: Allows deletion of selected books from the system.

## Data Persistence
The application loads data from `library.json` on startup. Every state-changing operation (Add, Remove, Borrow, Return) triggers a save to this file immediately, ensuring data consistency.

## Dependencies
- **Avalonia**: Cross-platform UI framework.
- **Avalonia.FuncUI**: F# wrapper for Avalonia, enabling functional UI patterns.
- **Avalonia.Themes.Fluent**: Modern look and feel.
