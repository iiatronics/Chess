using System;
using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public class Queen : ChessPiece
    {
        public Queen(PlayerColor color) : base(color, PieceType.Queen)
        {
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            int dx = Math.Abs(toX - fromX);
            int dy = Math.Abs(toY - fromY);

            // пряма (як Тура) || діагональ (як Слон)
            bool isStraight = (fromX == toX || fromY == toY);
            bool isDiagonal = (dx == dy);

            if (!isStraight && !isDiagonal)
            {
                return false;
            }

            int stepX = Math.Sign(toX - fromX); 
            int stepY = Math.Sign(toY - fromY);

            int currentX = fromX + stepX;
            int currentY = fromY + stepY;

            while (currentX != toX || currentY != toY)
            {
                if (board.GetPiece(currentX, currentY) != null)
                {
                    return false; 
                }
                currentX += stepX;
                currentY += stepY;
            }

            // Перевірка цілі
            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false;
            }

            return true;
        }
    }
}