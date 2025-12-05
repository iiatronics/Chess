# Chess
OOP project 2nd year 1 semestr

# ChessGame Project
     A WPF-based Chess game with separated logic and UI.

## Features

The program provides a graphical interface for playing chess with the following capabilities:

1.  **Game Board & Custom Assets:** Visual representation of the chess board using custom image assets (PNG) for pieces. Supports visual rendering logic.
2.  **Game State Management:** Ability to **Save** and **Load** the game state using JSON serialization (via `Newtonsoft.Json`).
3.  **Settings System:** Persistent application settings (Dark Theme, Sound, Hints) managed via `AppSettings.cs`. Settings are saved between sessions.
4.  **Multi-Window Interface:** Navigation between 4 distinct windows: Main Menu, Game Board, Settings, and About screen.
5.  **Move History:** visual tracking of moves and game status updates.

## Technologies

* **Language:** C# (.NET 10.0)
* **Framework:** WPF (Windows Presentation Foundation)
* **External Libraries:** Newtonsoft.Json (for data persistence)
* **Architecture:** Layered (UI depends on Core)
* **Testing:** xUnit
* **AI Assistance:** Google Gemini was used for code analysis, debugging, and resolving logic errors during the development process.

## Project structure

├── ChessGame.sln                   # Solution file
│
├── ChessGame.Core/                 # Class Library (Game Logic)
│   ├── Common/
│   │   └── Enums.cs                # Piece types and colors
│   ├── Game/
│   │   ├── Board.cs                # Board logic representation
│   │   ├── GameSession.cs          # Main game loop controller
│   │   └── Move.cs                 # Move logic
│   ├── Pieces/
│   │   ├── ChessPiece.cs           # Base class for pieces
│   │   ├── King.cs
│   │   ├── Queen.cs
│   │   ├── Rook.cs
│   │   ├── Bishop.cs
│   │   ├── Knight.cs
│   │   └── Pawn.cs
│   └── ChessGame.Core.csproj
│
├── ChessGame.UI/                   # WPF Application (User Interface)
│   ├── Images/                     # Graphic assets (PNG)
│   │   ├── b_bishop.png
│   │   ├── w_king.png
│   │   └── ... (other pieces)
│   ├── Services/
│   │   └── AppSettings.cs          # Settings manager (Singleton)
│   ├── AboutWindow.xaml            # "About" screen
│   ├── AboutWindow.xaml.cs
│   ├── App.xaml                    # Application entry point
│   ├── MainMenuWindow.xaml         # Starting menu
│   ├── MainMenuWindow.xaml.cs
│   ├── MainWindow.xaml             # Main game board window
│   ├── MainWindow.xaml.cs
│   ├── SettingsWindow.xaml         # Configuration window
│   ├── SettingsWindow.xaml.cs
│   ├── chess_save.json             # Game save file
│   ├── settings.json               # Config save file
│   └── ChessGame.UI.csproj
│
└── ChessGame.Tests/                # Unit Tests
├── UnitTest1.cs
└── ChessGame.Tests.csproj