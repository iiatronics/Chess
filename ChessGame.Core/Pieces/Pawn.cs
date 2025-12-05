using System;
using ChessGame.Core.Common;
using ChessGame.Core.Game;

namespace ChessGame.Core.Pieces
{
    public class Pawn : ChessPiece
    {
        public Pawn(PlayerColor color) : base(color, PieceType.Pawn)
        {
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            // Визначаємо напрямок руху.
            int direction = (Color == PlayerColor.White) ? -1 : 1;

            int dx = toX - fromX;
            int dy = toY - fromY;

            //хід вперед 
            if (dx == 0 && dy == direction)
            {
                if (board.GetPiece(toX, toY) == null)
                {
                    return true;
                }
            }

            //Подвійний хід
            if (dx == 0 && dy == 2 * direction)
            {
                if (!HasMoved)
                {
                    int midY = fromY + direction;
                    if (board.GetPiece(toX, midY) == null && board.GetPiece(toX, toY) == null)
                    {
                        return true;
                    }
                }
            }

            //Атака по діагоналі
            if (Math.Abs(dx) == 1 && dy == direction)
            {
                ChessPiece target = board.GetPiece(toX, toY);
                if (target != null && target.Color != this.Color)
                {
                    return true;
                }
            }


            return false;
        }
    }
}