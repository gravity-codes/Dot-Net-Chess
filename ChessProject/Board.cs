﻿using ChessLogic;
using System.Diagnostics.Metrics;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.White, null },
            { Player.Black, null }
        };

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPositions(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }

        public static Board Initialize()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            /* Board looks like:
             * -01234567 
             * 0 - Black Backrank
             * 1 - Black Pawns
             * 2
             * 3
             * 4
             * 5
             * 6 - White Pawns
             * 7 - White Backrank
             */

            // Create black backrank
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            // Create black pawns
            this[1, 0] = new Pawn(Player.Black);
            this[1, 1] = new Pawn(Player.Black);
            this[1, 2] = new Pawn(Player.Black);
            this[1, 3] = new Pawn(Player.Black);
            this[1, 4] = new Pawn(Player.Black);
            this[1, 5] = new Pawn(Player.Black);
            this[1, 6] = new Pawn(Player.Black);
            this[1, 7] = new Pawn(Player.Black);

            // Create white pawns
            this[6, 0] = new Pawn(Player.White);
            this[6, 1] = new Pawn(Player.White);
            this[6, 2] = new Pawn(Player.White);
            this[6, 3] = new Pawn(Player.White);
            this[6, 4] = new Pawn(Player.White);
            this[6, 5] = new Pawn(Player.White);
            this[6, 6] = new Pawn(Player.White);
            this[6, 7] = new Pawn(Player.White);

            // Create white backrank
            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new King(Player.White);
            this[7, 4] = new Queen(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Column >= 0 && pos.Row < 8 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);

                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];

                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }
            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVsKing(counting) || IsKingBishopVsKing(counting) || IsKingKnightVsKing(counting) || IsKingBishopVsKingBishop(counting);
        }

        private static bool IsKingVsKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.GetWhiteCount(PieceType.Bishop) == 1 || counting.GetBlackCount(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.GetWhiteCount(PieceType.Knight) == 1 || counting.GetBlackCount(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVsKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.GetWhiteCount(PieceType.Bishop) != 1 || counting.GetBlackCount(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position WhiteBishopPos = FindPiece(Player.White, PieceType.Bishop);
            Position BlackBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return WhiteBishopPos.SquareColor() == BlackBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType pieceType)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == pieceType);
        }
    }
}
