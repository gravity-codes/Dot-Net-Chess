﻿namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;

        public override Player Color { get; }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        };

        public King(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in directions)
            {
                Position to = from + dir;

                if (!Board.IsInside(to))
                {
                    continue;
                }

                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new Normal(from, to);
            }
        }
    }
}