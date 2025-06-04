using Game2048.Models;

namespace Game2048.Services
{
    public interface IGameService
    {
        GameBoard GameBoard { get; }
        event EventHandler<GameBoard>? GameBoardChanged;

        void StartNewGame();
        bool Move(Direction direction);
        bool CanMove();
    }
}
