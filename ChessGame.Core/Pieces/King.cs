using System;
using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public class King : ChessPiece
    {
        public King(PlayerColor color) : base(color, PieceType.King)
        {
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            int dx = Math.Abs(toX - fromX);
            int dy = Math.Abs(toY - fromY);

            if (dx > 1 || dy > 1)
            {
                return false;
            }

            // не стояти на місці (це не хід)
            if (dx == 0 && dy == 0)
            {
                return false;
            }

            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false;
            }
            
            // додати перевірку "IsCheck" щоб не стати під шах
            

            return true;
        }
    }
}