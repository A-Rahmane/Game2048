using System.Windows.Input;
using System.Collections.ObjectModel;
using Game2048.Models;
using Game2048.Commands;
using Game2048.Services;

namespace Game2048.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IGameService _gameService;
        private int _score;
        private bool _isGameOver;
        private bool _hasWon;
        private string _gameMessage;

        public ObservableCollection<TileViewModel> Tiles { get; }
        public ICommand NewGameCommand { get; }
        public ICommand MoveCommand { get; }

        public int Score
        {
            get => _score;
            set => SetField(ref _score, value);
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            set => SetField(ref _isGameOver, value);
        }

        public bool HasWon
        {
            get => _hasWon;
            set => SetField(ref _hasWon, value);
        }

        public string GameMessage
        {
            get => _gameMessage;
            set => SetField(ref _gameMessage, value);
        }

        public MainViewModel(IGameService gameService)
        {
            _gameService = gameService;
            _gameMessage = string.Empty;
            Tiles = new ObservableCollection<TileViewModel>();

            NewGameCommand = new RelayCommand(_ => NewGame());
            MoveCommand = new RelayCommand(direction => Move((Direction)direction!));

            _gameService.GameBoardChanged += OnGameBoardChanged;

            InitializeTiles();
            UpdateGameBoard(_gameService.GameBoard);
        }

        private void InitializeTiles()
        {
            for (int row = 0; row < GameBoard.Size; row++)
            {
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    Tiles.Add(new TileViewModel(new Tile(row, col)));
                }
            }
        }

        private void NewGame()
        {
            _gameService.StartNewGame();
        }

        private void Move(Direction direction)
        {
            if (!IsGameOver)
            {
                _gameService.Move(direction);
            }
        }

        private void OnGameBoardChanged(object? sender, GameBoard gameBoard)
        {
            UpdateGameBoard(gameBoard);
        }

        private void UpdateGameBoard(GameBoard gameBoard)
        {
            Score = gameBoard.Score;
            IsGameOver = gameBoard.IsGameOver;
            HasWon = gameBoard.HasWon;

            if (HasWon)
            {
                GameMessage = "You Won!";
            }
            else if (IsGameOver)
            {
                GameMessage = "Game Over!";
            }
            else
            {
                GameMessage = string.Empty;
            }

            // Update tiles
            for (int row = 0; row < GameBoard.Size; row++)
            {
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    var tile = gameBoard.Tiles[row, col];
                    var tileViewModel = Tiles.FirstOrDefault(t => t.Row == row && t.Column == col);
                    tileViewModel?.UpdateTile(tile);
                }
            }
        }
    }
}
