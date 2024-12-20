﻿namespace ChessLogic
{
    public class Normal : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get;  }
        public override Position ToPos { get; }

        public Normal(Position from, Position to) 
        {
            FromPos = from;
            ToPos = to;
        }

        public override bool Execute(Board board)
        {
            Piece piece = board[FromPos];
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;

            return !board.IsEmpty(ToPos) || piece.Type == PieceType.Pawn;
        }
    }
}
