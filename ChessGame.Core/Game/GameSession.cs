using System.Collections.Generic;
using ChessGame.Core.Common;
using ChessGame.Core.Pieces;

namespace ChessGame.Core.Game
{
    public class GameSession
    {
        public Board Board { get; private set; }

        public GameStatus GameStatus { get; private set; }
        public PlayerColor CurrentTurn { get; private set; }

        public Stack<Move> MoveHistory { get; private set; }

        public GameSession()
        {
            Board = new Board();
            MoveHistory = new Stack<Move>();
            CurrentTurn = PlayerColor.White;

            GameStatus = GameStatus.InProgress;

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            //black
            Board.SetPiece(0, 0, new Rook(PlayerColor.Black));
            Board.SetPiece(1, 0, new Knight(PlayerColor.Black));
            Board.SetPiece(2, 0, new Bishop(PlayerColor.Black));
            Board.SetPiece(3, 0, new Queen(PlayerColor.Black));
            Board.SetPiece(4, 0, new King(PlayerColor.Black));
            Board.SetPiece(5, 0, new Bishop(PlayerColor.Black));
            Board.SetPiece(6, 0, new Knight(PlayerColor.Black));
            Board.SetPiece(7, 0, new Rook(PlayerColor.Black));

            for (int i = 0; i < 8; i++)
            {
                Board.SetPiece(i, 1, new Pawn(PlayerColor.Black));
            }
            //white
            Board.SetPiece(0, 7, new Rook(PlayerColor.White));
            Board.SetPiece(1, 7, new Knight(PlayerColor.White));
            Board.SetPiece(2, 7, new Bishop(PlayerColor.White));
            Board.SetPiece(3, 7, new Queen(PlayerColor.White));
            Board.SetPiece(4, 7, new King(PlayerColor.White));
            Board.SetPiece(5, 7, new Bishop(PlayerColor.White));
            Board.SetPiece(6, 7, new Knight(PlayerColor.White));
            Board.SetPiece(7, 7, new Rook(PlayerColor.White));

            for (int i = 0; i < 8; i++)
            {
                Board.SetPiece(i, 6, new Pawn(PlayerColor.White));
            }
        }

        public bool MakeMove(int fromX, int fromY, int toX, int toY)
        {
            ChessPiece piece = Board.GetPiece(fromX, fromY);

            if (piece == null)
            {
                return false;
            }

            if (piece.Color != CurrentTurn)
            {
                return false;
            }

            // 2. Запитуємо у фігури, чи може вона так ходити
            if (!piece.IsValidMove(fromX, fromY, toX, toY, Board))
            {
                return false;
            }

            ChessPiece capturedPiece = Board.GetPiece(toX, toY);
            bool originalHasMoved = piece.HasMoved;

            //Віртуальний хід
            Board.SetPiece(toX, toY, piece);
            Board.SetPiece(fromX, fromY, null);

            bool isSuicide = IsKingInCheck(CurrentTurn);

            //ВІДКОЧУЄМО ВСЕ НАЗАД
            Board.SetPiece(fromX, fromY, piece);
            Board.SetPiece(toX, toY, capturedPiece);
            piece.HasMoved = originalHasMoved;

            //Якщо це було самогубство - забороняємо хід
            if (isSuicide)
            {
                System.Diagnostics.Debug.WriteLine("Хід заборонено: Король під ударом!");
                return false;
            }

            Board.SetPiece(toX, toY, piece);
            Board.SetPiece(fromX, fromY, null);
            piece.HasMoved = true;


            MoveHistory.Push(new Move(fromX, fromY, toX, toY, piece, capturedPiece));

            SwapTurn();

            if (IsCheckMate(CurrentTurn))
            {
                if (CurrentTurn == PlayerColor.White)
                    GameStatus = GameStatus.BlackWon;
                else
                    GameStatus = GameStatus.WhiteWon;
            }
            else if (IsStalemate(CurrentTurn))
            {
                GameStatus = GameStatus.Stalemate;
            }

            return true;
        }

        public bool IsCheckMate(PlayerColor playerColor)
        {
            if (!IsKingInCheck(playerColor)) return false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = Board.GetPiece(x, y);

                    if (piece != null && piece.Color == playerColor)
                    {
                        for (int targetX = 0; targetX < 8; targetX++)
                        {
                            for (int targetY = 0; targetY < 8; targetY++)
                            {
                                if (piece.IsValidMove(x, y, targetX, targetY, Board))
                                {
                                    var captured = Board.GetPiece(targetX, targetY);
                                    Board.SetPiece(targetX, targetY, piece);
                                    Board.SetPiece(x, y, null);


                                    bool stillInCheck = IsKingInCheck(playerColor);

                                    Board.SetPiece(x, y, piece);
                                    Board.SetPiece(targetX, targetY, captured);

                                    if (!stillInCheck)
                                    {
                                        return false; 
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true; 
        }

        public bool IsStalemate(PlayerColor playerColor)
        {
            if (IsKingInCheck(playerColor)) return false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = Board.GetPiece(x, y);

                    if (piece != null && piece.Color == playerColor)
                    {
                        for (int targetX = 0; targetX < 8; targetX++)
                        {
                            for (int targetY = 0; targetY < 8; targetY++)
                            {
                                if (piece.IsValidMove(x, y, targetX, targetY, Board))
                                {
                                    var captured = Board.GetPiece(targetX, targetY);
                                    Board.SetPiece(targetX, targetY, piece);
                                    Board.SetPiece(x, y, null);

                                    bool isSelfCheck = IsKingInCheck(playerColor);

                                    Board.SetPiece(x, y, piece);
                                    Board.SetPiece(targetX, targetY, captured);

                                    if (!isSelfCheck)
                                    {
                                        return false; 
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void SwapTurn()
        {
            CurrentTurn = (CurrentTurn == PlayerColor.White) ? PlayerColor.Black : PlayerColor.White;
        }

        public bool IsSquareUnderAttack(int x, int y, PlayerColor attackerColor)
        {
            // Проходимо по всій дошці
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = Board.GetPiece(i, j);
                    // Якщо це фігура ворога
                    if (piece != null && piece.Color == attackerColor)
                    {
                        // І вона може походити на клітинку (x, y)
                        if (piece.IsValidMove(i, j, x, y, Board))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private (int X, int Y) FindKingPosition(PlayerColor color)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = Board.GetPiece(x, y);
                    if (piece != null && piece.Type == PieceType.King && piece.Color == color)
                    {
                        return (x, y);
                    }
                }
            }
            throw new System.Exception("Короля не знайдено на дошці! Щось пішло не так.");
        }

        public bool IsKingInCheck(PlayerColor playerColor)
        {
            var kingPos = FindKingPosition(playerColor);
            return IsSquareUnderAttack(kingPos.X, kingPos.Y, playerColor);
        }


    }
}