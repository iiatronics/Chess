using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public abstract class ChessPiece
    {
        public PlayerColor Color { get; }
        public PieceType Type { get; }
        
        // Властивість для перевірки чи ходила фігура (потрібно для пішаків та рокіровки)
        public bool HasMoved { get; set; } = false;

        protected ChessPiece(PlayerColor color, PieceType type)
        {
            Color = color;
            Type = type;
        }

        // Кожна фігура буде реалізовувати цей метод по-своєму
        public abstract bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board);
    }
}