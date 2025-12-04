using ChessGame.Core.Common;
using ChessGame.Core.Pieces;

namespace ChessGame.Core.Game
{
    public class Move
    {
        public int FromX { get; }
        public int FromY { get; }
        public int ToX { get; }
        public int ToY { get; }
        public ChessPiece MovedPiece { get; }
        public ChessPiece CapturedPiece { get; } 

        public Move(int fromX, int fromY, int toX, int toY, ChessPiece movedPiece, ChessPiece capturedPiece)
        {
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            MovedPiece = movedPiece;
            CapturedPiece = capturedPiece;
        }
    }
}