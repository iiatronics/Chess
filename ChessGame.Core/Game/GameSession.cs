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

            if (piece == null) return false;
            if (piece.Color != CurrentTurn) return false;


            if (piece.Type == PieceType.King && System.Math.Abs(toX - fromX) == 2 && fromY == toY)
            {

                if (PerformCastling(fromX, fromY, toX))
                {
                    SwapTurn();
                    return true;
                }
                return false;
            }

            if (!piece.IsValidMove(fromX, fromY, toX, toY, Board))
            {
                return false;
            }

            ChessPiece capturedPiece = Board.GetPiece(toX, toY);
            bool originalHasMoved = piece.HasMoved;

            Board.SetPiece(toX, toY, piece);
            Board.SetPiece(fromX, fromY, null);

            bool isSuicide = IsKingInCheck(CurrentTurn);


            Board.SetPiece(fromX, fromY, piece);
            Board.SetPiece(toX, toY, capturedPiece);
            piece.HasMoved = originalHasMoved;

            if (isSuicide)
            {
                return false;
            }

            Board.SetPiece(toX, toY, piece);
            Board.SetPiece(fromX, fromY, null);
            piece.HasMoved = true;


            if (piece.Type == PieceType.Pawn)
            {
                if ((piece.Color == PlayerColor.White && toY == 0) ||
                    (piece.Color == PlayerColor.Black && toY == 7))
                {
                    var queen = new Queen(piece.Color);
                    queen.HasMoved = true;
                    Board.SetPiece(toX, toY, queen);


                }
            }

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

        private bool PerformCastling(int kingX, int kingY, int targetX)
        {
            if (IsKingInCheck(CurrentTurn)) return false;

            ChessPiece king = Board.GetPiece(kingX, kingY);
            bool isKingSide = targetX > kingX; 
            int rookX = isKingSide ? 7 : 0;    
            ChessPiece rook = Board.GetPiece(rookX, kingY);

            if (rook == null || rook.Type != PieceType.Rook || rook.Color != king.Color) return false;
            if (king.HasMoved || rook.HasMoved) return false;

            int start = System.Math.Min(kingX, rookX) + 1;
            int end = System.Math.Max(kingX, rookX);
            for (int i = start; i < end; i++)
            {
                if (Board.GetPiece(i, kingY) != null) return false;
            }

            int direction = isKingSide ? 1 : -1;
            if (IsSquareUnderAttack(kingX + direction, kingY, CurrentTurn)) return false;

            if (IsSquareUnderAttack(targetX, kingY, CurrentTurn)) return false;


            Board.SetPiece(kingX, kingY, null);
            Board.SetPiece(targetX, kingY, king);
            king.HasMoved = true;


            int rookTargetX = isKingSide ? targetX - 1 : targetX + 1;
            Board.SetPiece(rookX, kingY, null);
            Board.SetPiece(rookTargetX, kingY, rook);
            rook.HasMoved = true;

            MoveHistory.Push(new Move(kingX, kingY, targetX, kingY, king, null));

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

        public bool IsSquareUnderAttack(int targetX, int targetY, PlayerColor defenderColor)
        {
            // 1. ВИЗНАЧАЄМО КОЛІР ВОРОГА
            // Якщо захищаються Білі, то нападають Чорні, і навпаки.
            PlayerColor attackerColor = (defenderColor == PlayerColor.White) ? PlayerColor.Black : PlayerColor.White;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = Board.GetPiece(x, y);

                    if (piece != null && piece.Color == attackerColor)
                    {
                        if (piece.Type == PieceType.Pawn)
                        {
                            int direction = (piece.Color == PlayerColor.White) ? -1 : 1;

                            if (targetY == y + direction)
                            {
                                if (targetX == x - 1 || targetX == x + 1)
                                {
                                    return true; 
                                }
                            }
                        }
                        else
                        {
                            if (piece.IsValidMove(x, y, targetX, targetY, Board))
                            {
                                return true;
                            }
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