namespace Game2048.Models
{
    public class GameBoard
    {
        public const int Size = 4;
        public Tile[,] Tiles { get; private set; }
        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool HasWon { get; private set; }

        public GameBoard()
        {
            Tiles = new Tile[Size, Size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Tiles[row, col] = new Tile(row, col);
                }
            }
            Score = 0;
            IsGameOver = false;
            HasWon = false;
        }

        public void Reset()
        {
            InitializeBoard();
        }

        public void UpdateScore(int points)
        {
            Score += points;
        }

        public void SetGameOver(bool isOver)
        {
            IsGameOver = isOver;
        }

        public void SetWon(bool won)
        {
            HasWon = won;
        }

        public GameBoard Clone()
        {
            var clone = new GameBoard()
            {
                Score = this.Score,
                IsGameOver = this.IsGameOver,
                HasWon = this.HasWon
            };

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    clone.Tiles[row, col] = this.Tiles[row, col].Clone();
                }
            }

            return clone;
        }
    }
}
