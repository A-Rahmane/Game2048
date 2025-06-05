# 2048 Game - WPF Implementation

A clean, well-architected implementation of the popular 2048 game using WPF and following SOLID principles and design patterns.

## Table of Contents
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Key Components](#key-components)
- [Adding New Features](#adding-new-features)
- [Modifying Existing Code](#modifying-existing-code)
- [Design Patterns Used](#design-patterns-used)
- [Build and Run](#build-and-run)
- [Testing](#testing)
- [Suggestions for Improvement](#suggestions-for-improvement)

## Architecture Overview

This implementation follows the MVVM (Model-View-ViewModel) pattern with a service layer for game logic. The architecture ensures:

- **Separation of Concerns**: UI logic is separated from business logic
- **Testability**: All components can be unit tested independently
- **Extensibility**: Easy to add new features without modifying existing code
- **Maintainability**: Clear structure and single responsibility for each class

```
┌─────────────┐     ┌──────────────┐     ┌──────────────┐
│    Views    │────▶│  ViewModels  │────▶│   Services   │
└─────────────┘     └──────────────┘     └──────────────┘
│                     │
▼                     ▼
┌─────────┐         ┌─────────┐
│Commands │         │ Models  │
└─────────┘         └─────────┘
```

## Project Structure

```
Game2048/
├── Models/
│   ├── Direction.cs          # Enum for movement directions
│   ├── Tile.cs              # Represents a single tile
│   └── GameBoard.cs         # Game board state and logic
├── Services/
│   ├── IGameService.cs      # Game service interface
│   └── GameService.cs       # Core game logic implementation
├── ViewModels/
│   ├── ViewModelBase.cs     # Base class for all ViewModels
│   ├── TileViewModel.cs     # ViewModel for individual tiles
│   └── MainViewModel.cs     # Main game ViewModel
├── Views/
│   ├── MainWindow.xaml      # Main game UI
│   ├── MainWindow.xaml.cs   # Code-behind
│   └── Converters.cs        # XAML value converters
├── Commands/
│   └── RelayCommand.cs      # ICommand implementation
└── App.xaml                 # Application resources
```

## Key Components

### Models

**Tile.cs**

- Represents a single tile on the game board
- Properties: Value, Row, Column, IsNew, IsMerged
- Supports cloning for immutable game state

**GameBoard.cs**

- Maintains the 4x4 grid of tiles
- Tracks score, game over state, and win condition
- Provides cloning for state management

### Services

**IGameService.cs**

- Defines the contract for game operations
- Methods: StartNewGame(), Move(), CanMove()
- Events: GameBoardChanged

**GameService.cs**

- Implements core game logic
- Handles tile movements and merging
- Manages random tile generation
- Checks win/lose conditions

### ViewModels

**MainViewModel.cs**

- Orchestrates the game UI
- Exposes commands for user actions
- Updates UI based on game state changes

**TileViewModel.cs**

- Wraps a Tile model for UI binding
- Provides color and styling based on tile value
- Implements property change notifications

## Design Patterns Used

- **MVVM Pattern**: Separates presentation logic from business logic
- **Command Pattern**: Encapsulates user actions as commands
- **Observer Pattern**: INotifyPropertyChanged for data binding
- **Repository Pattern**: GameService manages game state
- **Dependency Injection**: Services are injected into consumers
- **Factory Pattern**: (Can be added for tile creation)

## Build and Run

1. Clone the repository
2. Open Game2048.sln in Visual Studio 2022 or later
3. Restore NuGet packages
4. Build the solution (Ctrl+Shift+B)
5. Run the application (F5)

**Requirements:**

- .NET 8.0 or later
- Windows OS (for WPF)
