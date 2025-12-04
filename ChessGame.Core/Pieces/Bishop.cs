using System;
using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop(PlayerColor color) : base(color, PieceType.Bishop)
        {
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            // Перевірка на діагональ
            int dx = Math.Abs(toX - fromX);
            int dy = Math.Abs(toY - fromY);

            if (dx != dy) return false; // Це не діагональ

            // Перевірка перешкод
            int stepX = Math.Sign(toX - fromX); 
            int stepY = Math.Sign(toY - fromY); 

            int currentX = fromX + stepX;
            int currentY = fromY + stepY;

            while (currentX != toX)
            {
                if (board.GetPiece(currentX, currentY) != null)
                {
                    return false; 
                }
                currentX += stepX;
                currentY += stepY;
            }

            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false;
            }

            return true;
        }
    }
}