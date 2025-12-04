using ChessGame.Core.Pieces; 

namespace ChessGame.Core.Game
{
    public class Board
    {
        // Масив 8 на 8. Якщо null - клітинка пуста.
        private readonly ChessPiece[,] _pieces; 

        public Board()
        {
            _pieces = new ChessPiece[8, 8];
        }

        // Метод, щоб поставити фігуру (використовуватимемо при старті)
        public void SetPiece(int x, int y, ChessPiece piece)
        {
            _pieces[x, y] = piece;
        }

        // Метод, щоб дізнатися, хто стоїть на клітинці (потрібен для перевірки ходів)
        public ChessPiece GetPiece(int x, int y)
        {

            if (x < 0 || x > 7 || y < 0 || y > 7)
                return null;
                
            return _pieces[x, y];
        }
    }
}