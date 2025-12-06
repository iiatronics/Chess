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

            if (fromX != toX && fromY != toY)
                return false;

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


            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false; // Не можна їсти своїх
            }

            return true;
        }
    }
}