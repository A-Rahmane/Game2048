using System;
using System.Linq;
using Game2048.Models;

namespace Game2048.Services
{
    public class GameService : IGameService
    {
        private readonly Random _random = new();
        private GameBoard _gameBoard;

        public GameBoard GameBoard => _gameBoard;

        public event EventHandler<GameBoard>? GameBoardChanged;

        public GameService()
        {
            _gameBoard = new GameBoard();
            StartNewGame();
        }
        public void StartNewGame()
        {
            _gameBoard.Reset();
            AddRandomTile();
            AddRandomTile();
            OnGameBoardChanged();
        }

        public bool Move(Direction direction)
        {
            if (_gameBoard.IsGameOver) return false;

            var boardBeforeMove = _gameBoard.Clone();
            bool moved = false;

            switch (direction)
            {
                case Direction.Up:
                    moved = MoveUp();
                    break;
                case Direction.Down:
                    moved = MoveDown();
                    break;
                case Direction.Left:
                    moved = MoveLeft();
                    break;
                case Direction.Right:
                    moved = MoveRight();
                    break;
            }

            if (moved)
            {
                AddRandomTile();
                CheckGameState();
                OnGameBoardChanged();
            }

            return moved;
        }

        public bool CanMove()
        {
            // check for empty cells
            for (int row = 0; row < GameBoard.Size; row++)
            {
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    if (GameBoard.Tiles[row, col].Value == 0)
                        return true;
                }
            }

            // check for possible merges
            for (int row = 0; row < GameBoard.Size; row++)
            {
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    if (col < GameBoard.Size - 1 && GameBoard.Tiles[row, col].Value == GameBoard.Tiles[row, col + 1].Value)
                        return true;
                    if (row < GameBoard.Size - 1 && GameBoard.Tiles[row, col].Value == GameBoard.Tiles[row + 1, col].Value)
                        return true;
                }
            }

            return false;
        }

        private bool MoveUp()
        {
            bool moved = false;
            for (int col = 0; col < GameBoard.Size; col++)
            {
                int[] column = new int[GameBoard.Size];
                for (int row = 0; row < GameBoard.Size; row++)
                {
                    column[row] = _gameBoard.Tiles[row, col].Value;
                }

                int[] newColumn = ProcessLine(column);
                for (int row = 0; row < GameBoard.Size; row++)
                {
                    if (_gameBoard.Tiles[row, col].Value != newColumn[row])
                    {
                        _gameBoard.Tiles[row, col].Value = newColumn[row];
                        moved = true;
                    }
                }
            }
            return moved;
        }

        private bool MoveDown()
        {
            bool moved = false;
            for (int col = 0; col < GameBoard.Size; col++)
            {
                int[] column = new int[GameBoard.Size];
                for (int row = 0; row < GameBoard.Size; row++)
                {
                    column[row] = _gameBoard.Tiles[GameBoard.Size - 1 - row, col].Value;
                }

                int[] newColumn = ProcessLine(column);
                for (int row = 0; row < GameBoard.Size; row++)
                {
                    if (_gameBoard.Tiles[GameBoard.Size - 1 - row, col].Value != newColumn[row])
                    {
                        _gameBoard.Tiles[GameBoard.Size - 1 - row, col].Value = newColumn[row];
                        moved = true;
                    }
                }
            }
            return moved;
        }

        private bool MoveLeft()
        {
            bool moved = false;
            for (int row = 0; row < GameBoard.Size; row++)
            {
                int[] line = new int[GameBoard.Size];
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    line[col] = _gameBoard.Tiles[row, col].Value;
                }

                int[] newLine = ProcessLine(line);
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    if (_gameBoard.Tiles[row, col].Value != newLine[col])
                    {
                        _gameBoard.Tiles[row, col].Value = newLine[col];
                        moved = true;
                    }
                }
            }
            return moved;
        }

        private bool MoveRight()
        {
            bool moved = false;
            for (int row = 0; row < GameBoard.Size; row++)
            {
                int[] line = new int[GameBoard.Size];
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    line[col] = _gameBoard.Tiles[row, GameBoard.Size - 1 - col].Value;
                }

                int[] newLine = ProcessLine(line);
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    if (_gameBoard.Tiles[row, GameBoard.Size - 1 - col].Value != newLine[col])
                    {
                        _gameBoard.Tiles[row, GameBoard.Size - 1 - col].Value = newLine[col];
                        moved = true;
                    }
                }
            }
            return moved;
        }

        private int[] ProcessLine(int[] line)
        {
            // Remove zeros
            var nonZeros = line.Where(x => x != 0).ToList();

            // Merge adjacent equal numbers
            for (int i = 0; i < nonZeros.Count - 1; i++)
            {
                if (nonZeros[i] == nonZeros[i + 1])
                {
                    nonZeros[i] *= 2;
                    _gameBoard.UpdateScore(nonZeros[i]);
                    nonZeros.RemoveAt(i + 1);

                    if (nonZeros[i] == 2048)
                    {
                        _gameBoard.SetWon(true);
                    }
                }
            }

            // Pad with zeros
            while (nonZeros.Count < GameBoard.Size)
            {
                nonZeros.Add(0);
            }

            return nonZeros.ToArray();
        }

        private void AddRandomTile()
        {
            var emptyTiles = new List<(int row, int col)>();

            for (int row = 0; row < GameBoard.Size; row++)
            {
                for (int col = 0; col < GameBoard.Size; col++)
                {
                    if (GameBoard.Tiles[row, col].Value == 0)
                    {
                        emptyTiles.Add((row, col));
                    }
                }
            }

            if (emptyTiles.Count > 0)
            {
                var (row, col) = emptyTiles[_random.Next(emptyTiles.Count)];
                _gameBoard.Tiles[row, col].Value = _random.Next(10) < 9 ? 2 : 4;
                _gameBoard.Tiles[row, col].IsNew = true;
            }
        }

        private void CheckGameState()
        {
            if (!CanMove())
            {
                _gameBoard.SetGameOver(true);
            }
        }

        private void OnGameBoardChanged()
        {
            GameBoardChanged?.Invoke(this, _gameBoard);
        }

    }
}
