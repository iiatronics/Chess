using System;
using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public class Knight : ChessPiece
    {
        public Knight(PlayerColor color) : base(color, PieceType.Knight)
        {
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            int dx = Math.Abs(fromX - toX);
            int dy = Math.Abs(fromY - toY);

            if (!((dx == 2 && dy == 1) || (dx == 1 && dy == 2)))
            {
                return false; 
            }

            // Перевірка цільової клітинки (чи не їмо ми своїх)
            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false;
            }

            return true;
        }
    }
}