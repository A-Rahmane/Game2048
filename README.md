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
┌─────────────┐     ┌──────────────┐     ┌──────────────┐
│    Views    │────▶│  ViewModels  │────▶│   Services   │
└─────────────┘     └──────────────┘     └──────────────┘
│                     │
▼                     ▼
┌─────────┐         ┌─────────┐
│Commands │         │ Models  │
└─────────┘         └─────────┘

## Project Structure
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

## Adding New Features

### 1. Adding Undo/Redo Functionality

Create a history manager:

```csharp
// Services/IGameHistoryService.cs
public interface IGameHistoryService
{
    void SaveState(GameBoard board);
    GameBoard? Undo();
    GameBoard? Redo();
    bool CanUndo { get; }
    bool CanRedo { get; }
}

// Services/GameHistoryService.cs
public class GameHistoryService : IGameHistoryService
{
    private readonly Stack<GameBoard> _undoStack = new();
    private readonly Stack<GameBoard> _redoStack = new();
    
    public void SaveState(GameBoard board)
    {
        _undoStack.Push(board.Clone());
        _redoStack.Clear();
    }
    
    // Implement other methods...
}
Update GameService to use history:
csharppublic class GameService : IGameService
{
    private readonly IGameHistoryService _historyService;
    
    public GameService(IGameHistoryService historyService)
    {
        _historyService = historyService;
    }
    
    public bool Move(Direction direction)
    {
        _historyService.SaveState(_gameBoard);
        // ... existing move logic
    }
}
2. Adding Animation Enhancements
Create animation services:
csharp// Services/IAnimationService.cs
public interface IAnimationService
{
    void AnimateTileMove(TileViewModel tile, double fromX, double fromY, double toX, double toY);
    void AnimateTileMerge(TileViewModel tile);
    void AnimateTileAppear(TileViewModel tile);
}
3. Adding High Score Tracking
csharp// Services/IScoreService.cs
public interface IScoreService
{
    int GetHighScore();
    void SaveHighScore(int score);
    List<ScoreEntry> GetTopScores(int count);
}

// Models/ScoreEntry.cs
public class ScoreEntry
{
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }
}
Modifying Existing Code
Optimizing Game Logic
The current ProcessLine method in GameService can be optimized:
csharp// Current implementation processes line multiple times
// Optimized version:
private (int[] line, int points) ProcessLineOptimized(int[] line)
{
    int points = 0;
    var result = new int[line.Length];
    int writeIndex = 0;
    
    for (int i = 0; i < line.Length; i++)
    {
        if (line[i] == 0) continue;
        
        if (writeIndex > 0 && result[writeIndex - 1] == line[i])
        {
            result[writeIndex - 1] *= 2;
            points += result[writeIndex - 1];
        }
        else
        {
            result[writeIndex++] = line[i];
        }
    }
    
    return (result, points);
}
Customizing Tile Colors
To change tile colors, modify the Background property in TileViewModel:
csharp// Create a theme service
public interface IThemeService
{
    Dictionary<int, Color> GetTileColors();
    Color GetBackgroundColor();
}

// Inject into TileViewModel
public TileViewModel(Tile tile, IThemeService themeService)
{
    _tile = tile;
    _themeService = themeService;
}
Adding Keyboard Shortcuts
Extend the KeyDown handler in MainWindow.xaml.cs:
csharpprivate void Window_KeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
    {
        _viewModel.NewGameCommand.Execute(null);
        e.Handled = true;
    }
    // ... existing arrow key handling
}
Design Patterns Used

MVVM Pattern: Separates presentation logic from business logic
Command Pattern: Encapsulates user actions as commands
Observer Pattern: INotifyPropertyChanged for data binding
Repository Pattern: GameService manages game state
Dependency Injection: Services are injected into consumers
Factory Pattern: (Can be added for tile creation)

Build and Run

Clone the repository
Open Game2048.sln in Visual Studio 2022 or later
Restore NuGet packages
Build the solution (Ctrl+Shift+B)
Run the application (F5)

Requirements:

.NET 8.0 or later
Windows OS (for WPF)

Testing
Unit Testing Structure
csharp// Tests/Services/GameServiceTests.cs
[TestClass]
public class GameServiceTests
{
    private IGameService _gameService;
    
    [TestInitialize]
    public void Setup()
    {
        _gameService = new GameService();
    }
    
    [TestMethod]
    public void Move_ValidDirection_ReturnsTrue()
    {
        // Arrange
        _gameService.StartNewGame();
        
        // Act
        var result = _gameService.Move(Direction.Up);
        
        // Assert
        Assert.IsTrue(result);
    }
}
Integration Testing
csharp// Tests/ViewModels/MainViewModelTests.cs
[TestClass]
public class MainViewModelTests
{
    [TestMethod]
    public void NewGameCommand_ResetsScore()
    {
        // Test ViewModel behavior
    }
}
Suggestions for Improvement
1. Performance Enhancements

Tile Pooling: Implement object pooling for TileViewModel instances to reduce garbage collection
Async Operations: Make tile animations and game state updates asynchronous
Virtualization: For larger board sizes, implement UI virtualization

2. Feature Additions

Multiple Board Sizes: Support 5x5, 6x6 boards with configurable goals
Power-ups: Add special tiles that clear rows/columns or multiply scores
Multiplayer Mode: Implement competitive or cooperative gameplay
AI Solver: Add an AI that can suggest optimal moves
Statistics Tracking: Track games played, win rate, average score, etc.

3. Technical Improvements

Dependency Injection Container: Use a DI container (e.g., Microsoft.Extensions.DependencyInjection)
Logging: Implement structured logging with Serilog or NLog
Settings Management: Add user preferences (theme, sound, animations)
Localization: Support multiple languages using resource files
Save/Load Game State: Persist game state between sessions

4. UI/UX Enhancements

Smooth Animations: Implement fluid tile movements and merging animations
Sound Effects: Add audio feedback for moves, merges, and game events
Themes: Support multiple visual themes (dark mode, high contrast, custom colors)
Touch Support: Add swipe gestures for touch-enabled devices
Responsive Design: Make the UI scalable for different window sizes

5. Code Quality

Code Analysis: Enable and configure StyleCop or Roslyn analyzers
Documentation: Add XML documentation to all public APIs
Performance Profiling: Use BenchmarkDotNet for performance testing
Code Coverage: Aim for >80% test coverage
CI/CD Pipeline: Set up automated builds and tests

6. Architecture Enhancements

Plugin System: Allow third-party extensions
Event Sourcing: Store all game moves for replay functionality
State Machine: Implement game states (Menu, Playing, Paused, GameOver)
Mediator Pattern: Decouple components further using MediatR

7. Distribution

Installer: Create MSI or ClickOnce installer
Portable Version: Create a self-contained executable
Microsoft Store: Package as MSIX for Store distribution
Cross-Platform: Consider migrating to .NET MAUI or Avalonia

These improvements would transform the application from a simple game implementation into a professional, feature-rich gaming experience while maintaining code quality and extensibility.

