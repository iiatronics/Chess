namespace ChessGame.Core.Common
{
    public enum PlayerColor
    {
        White,
        Black
    }

    public enum GameStatus
    {
        InProgress,
        WhiteWon,
        BlackWon,
        Stalemate
    }
    public enum PieceType
    {
        Pawn,   // Пішак
        Rook,   // Тура
        Knight, // Кінь
        Bishop, // Слон
        Queen,  // Королева
        King    // Король
    }
}
