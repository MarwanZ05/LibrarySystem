module Gui

open System
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media
open Domain
open Operations
open Storage

type Tab =
    | LibraryTab
    | AdminTab

type State =
    { CurrentTab: Tab
      Library: Library
      // Library Tab State
      SelectedLibraryBook: Book option
      LibraryMessage: string
      SearchQuery: string
      // Admin Tab State
      SelectedAdminBook: Book option
      ShowAddBookDialog: bool
      AddTitle: string
      AddAuthor: string
      AddMessage: string }

type Msg =
    | SetTab of Tab
    | SetSearchQuery of string
    // Library Actions
    | SelectLibraryBook of Book option
    | BorrowBookAction
    | ReturnBookAction
    // Admin Actions
    | SelectAdminBook of Book option
    | RemoveBookAction
    | ShowAddDialog of bool
    | SetAddTitle of string
    | SetAddAuthor of string
    | AddBookSubmit

let init () =
    { CurrentTab = LibraryTab
      Library = loadLibrary()
      SelectedLibraryBook = None
      LibraryMessage = ""
      SearchQuery = ""
      SelectedAdminBook = None
      ShowAddBookDialog = false
      AddTitle = ""
      AddAuthor = ""
      AddMessage = "" }

let update (msg: Msg) (state: State) =
    match msg with
    | SetTab tab -> { state with CurrentTab = tab; LibraryMessage = ""; AddMessage = "" }
    | SetSearchQuery q -> { state with SearchQuery = q }
    // Library Tab Updates
    | SelectLibraryBook book -> { state with SelectedLibraryBook = book; LibraryMessage = "" }
    | BorrowBookAction ->
        match state.SelectedLibraryBook with
        | Some book ->
            let newLib = borrowBook book.Id state.Library
            saveLibrary newLib
            // Update selected book reference if needed, or just reload logic relies on ID
            { state with Library = newLib; SelectedLibraryBook = Some { book with IsBorrowed = true }; LibraryMessage = "Book borrowed successfully." }
        | None -> { state with LibraryMessage = "Please select a book." }
    | ReturnBookAction ->
        match state.SelectedLibraryBook with
        | Some book ->
            let newLib = returnBook book.Id state.Library
            saveLibrary newLib
            { state with Library = newLib; SelectedLibraryBook = Some { book with IsBorrowed = false }; LibraryMessage = "Book returned successfully." }
        | None -> { state with LibraryMessage = "Please select a book." }
    // Admin Tab Updates
    | SelectAdminBook book -> { state with SelectedAdminBook = book }
    | RemoveBookAction ->
        match state.SelectedAdminBook with
        | Some book ->
            let newLib = removeBook book.Id state.Library
            saveLibrary newLib
            { state with Library = newLib; SelectedAdminBook = None }
        | None -> state
    | ShowAddDialog show -> { state with ShowAddBookDialog = show; AddTitle = ""; AddAuthor = ""; AddMessage = "" }
    | SetAddTitle t -> { state with AddTitle = t }
    | SetAddAuthor a -> { state with AddAuthor = a }
    | AddBookSubmit ->
        if String.IsNullOrWhiteSpace state.AddTitle || String.IsNullOrWhiteSpace state.AddAuthor then
            { state with AddMessage = "Title and Author are required." }
        else
            let newLib = addBook state.AddTitle state.AddAuthor state.Library
            saveLibrary newLib
            { state with Library = newLib; AddTitle = ""; AddAuthor = ""; AddMessage = "Book added successfully!"; ShowAddBookDialog = false }

let view (state: State) (dispatch: Msg -> unit) =
    DockPanel.create [
        DockPanel.children [
            // Modal Overlay for Add Book
            if state.ShowAddBookDialog then
                Border.create [
                    Border.zIndex 100 // Ensure it's on top
                    Border.background "#80000000" // Semi-transparent black
                    Border.child (
                        Border.create [
                            Border.background "White"
                            Border.width 400.0
                            Border.height 300.0
                            Border.cornerRadius 10.0
                            Border.padding 20.0
                            Border.verticalAlignment VerticalAlignment.Center
                            Border.horizontalAlignment HorizontalAlignment.Center
                            Border.child (
                                StackPanel.create [
                                    StackPanel.spacing 10.0
                                    StackPanel.children [
                                        TextBlock.create [ TextBlock.text "Add New Book"; TextBlock.fontSize 20.0; TextBlock.fontWeight FontWeight.Bold; TextBlock.horizontalAlignment HorizontalAlignment.Center ]
                                        TextBlock.create [ TextBlock.text "Title:" ]
                                        TextBox.create [ TextBox.text state.AddTitle; TextBox.onTextChanged (fun t -> dispatch (SetAddTitle t)) ]
                                        TextBlock.create [ TextBlock.text "Author:" ]
                                        TextBox.create [ TextBox.text state.AddAuthor; TextBox.onTextChanged (fun t -> dispatch (SetAddAuthor t)) ]
                                        StackPanel.create [
                                            StackPanel.orientation Orientation.Horizontal
                                            StackPanel.horizontalAlignment HorizontalAlignment.Center
                                            StackPanel.spacing 10.0
                                            StackPanel.margin (0.0, 20.0, 0.0, 0.0)
                                            StackPanel.children [
                                                Button.create [ Button.content "Add Book"; Button.onClick (fun _ -> dispatch AddBookSubmit); Button.classes ["accent"] ]
                                                Button.create [ Button.content "Cancel"; Button.onClick (fun _ -> dispatch (ShowAddDialog false)) ]
                                            ]
                                        ]
                                        TextBlock.create [ TextBlock.text state.AddMessage; TextBlock.foreground "Red"; TextBlock.horizontalAlignment HorizontalAlignment.Center ]
                                    ]
                                ]
                            )
                        ]
                    )
                ]

            // Navigation Sidebar
            StackPanel.create [
                DockPanel.dock Dock.Left
                StackPanel.width 200.0
                StackPanel.background "Gray"
                StackPanel.children [
                    Button.create [
                        Button.content "Library"
                        Button.onClick (fun _ -> dispatch (SetTab LibraryTab))
                        Button.margin 5.0
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                    ]
                    Button.create [
                        Button.content "Admin Panel"
                        Button.onClick (fun _ -> dispatch (SetTab AdminTab))
                        Button.margin 5.0
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                    ]
                ]
            ]

            // Main Content Area
            match state.CurrentTab with
            | LibraryTab ->
                DockPanel.create [
                    DockPanel.children [
                        StackPanel.create [
                            DockPanel.dock Dock.Top
                            StackPanel.margin 10.0
                            StackPanel.children [
                                StackPanel.create [
                                    StackPanel.orientation Orientation.Horizontal
                                    StackPanel.spacing 10.0
                                    StackPanel.children [
                                        TextBlock.create [ TextBlock.text "Search: "; TextBlock.verticalAlignment VerticalAlignment.Center ]
                                        TextBox.create [
                                            TextBox.width 300.0
                                            TextBox.text state.SearchQuery
                                            TextBox.onTextChanged (fun t -> dispatch (SetSearchQuery t))
                                        ]
                                    ]
                                ]
                                StackPanel.create [
                                    StackPanel.orientation Orientation.Horizontal
                                    StackPanel.margin (0.0, 10.0, 0.0, 0.0)
                                    StackPanel.spacing 10.0
                                    StackPanel.children [
                                        Button.create [
                                            Button.content "Borrow Selected"
                                            Button.isEnabled (match state.SelectedLibraryBook with Some b -> not b.IsBorrowed | None -> false)
                                            Button.onClick (fun _ -> dispatch BorrowBookAction)
                                        ]
                                        Button.create [
                                            Button.content "Return Selected"
                                            Button.isEnabled (match state.SelectedLibraryBook with Some b -> b.IsBorrowed | None -> false)
                                            Button.onClick (fun _ -> dispatch ReturnBookAction)
                                        ]
                                        TextBlock.create [ TextBlock.text state.LibraryMessage; TextBlock.foreground "Blue"; TextBlock.verticalAlignment VerticalAlignment.Center ]
                                    ]
                                ]
                            ]
                        ]
                        ListBox.create [
                            ListBox.dataItems (
                                if String.IsNullOrWhiteSpace state.SearchQuery then state.Library.Books
                                else searchBooks state.SearchQuery state.Library
                            )
                            ListBox.selectedItem state.SelectedLibraryBook
                            ListBox.onSelectedItemChanged (fun obj ->
                                match obj with
                                | :? Book as book -> dispatch (SelectLibraryBook (Some book))
                                | _ -> dispatch (SelectLibraryBook None)
                            )
                            ListBox.itemTemplate (DataTemplateView<Book>.create (fun book ->
                                StackPanel.create [
                                    StackPanel.orientation Orientation.Horizontal
                                    StackPanel.children [
                                        TextBlock.create [ TextBlock.text (sprintf "%s by %s" book.Title book.Author); TextBlock.fontWeight FontWeight.Bold ]
                                        TextBlock.create [ TextBlock.text (if book.IsBorrowed then " (Borrowed)" else " (Available)"); TextBlock.foreground (if book.IsBorrowed then "Red" else "Green"); TextBlock.margin (10.0, 0.0, 0.0, 0.0) ]
                                    ]
                                ]
                            ))
                        ]
                    ]
                ]
            | AdminTab ->
                DockPanel.create [
                    DockPanel.children [
                        StackPanel.create [
                            DockPanel.dock Dock.Top
                            StackPanel.margin 10.0
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.spacing 10.0
                            StackPanel.children [
                                Button.create [
                                    Button.content "Add New Book"
                                    Button.onClick (fun _ -> dispatch (ShowAddDialog true))
                                ]
                                Button.create [
                                    Button.content "Remove Selected Book"
                                    Button.background "Red"
                                    Button.foreground "White"
                                    Button.isEnabled (state.SelectedAdminBook.IsSome)
                                    Button.onClick (fun _ -> dispatch RemoveBookAction)
                                ]
                            ]
                        ]
                        ListBox.create [
                            ListBox.dataItems state.Library.Books
                            ListBox.selectedItem state.SelectedAdminBook
                            ListBox.onSelectedItemChanged (fun obj ->
                                match obj with
                                | :? Book as book -> dispatch (SelectAdminBook (Some book))
                                | _ -> dispatch (SelectAdminBook None)
                            )
                            ListBox.itemTemplate (DataTemplateView<Book>.create (fun book ->
                                TextBlock.create [ TextBlock.text (sprintf "%s by %s" book.Title book.Author) ]
                            ))
                        ]
                    ]
                ]
        ]
    ]
