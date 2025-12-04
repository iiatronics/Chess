using System;
using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public class Rook : ChessPiece
    {
        public Rook(PlayerColor color) : base(color, PieceType.Rook)
        {
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            //Тура не може ходити по діагоналі
            if (fromX != toX && fromY != toY)
                return false;

            // Тура не може перестрибувати інші фігури.           
            // Визначаємо напрямок руху (-1, 0, або 1)
            int stepX = Math.Sign(toX - fromX); 
            int stepY = Math.Sign(toY - fromY);

            int currentX = fromX + stepX;
            int currentY = fromY + stepY;

            // Йдемо по клітинках до (але не включаючи) кінцевої точки
            while (currentX != toX || currentY != toY)
            {
                if (board.GetPiece(currentX, currentY) != null)
                {
                    return false; // Шлях заблоковано
                }
                currentX += stepX;
                currentY += stepY;
            }

            // Перевірка кінцевої точки (чи не їмо ми своїх)
            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false; // Не можна їсти своїх
            }

            return true;
        }
    }
}