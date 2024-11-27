namespace ChessLogic
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Direction KingMoveDirection;
        private readonly Position RookFromPosition;
        private readonly Position RookToPosition;

        public Castle(MoveType type, Position KingPosition)
        {
            Type = type;
            FromPos = KingPosition;

            if (type == MoveType.CastleKing)
            {
                KingMoveDirection = Direction.East;
                ToPos = new Position(KingPosition.Row, 6);
                RookFromPosition = new Position(KingPosition.Row, 7);
                RookToPosition = new Position(KingPosition.Row, 5);
            }
            else if (type == MoveType.CastleQueen)
            {
                KingMoveDirection = Direction.West;
                ToPos = new Position(KingPosition.Row, 0);
                RookFromPosition = new Position(KingPosition.Row, 0);
                RookToPosition = new Position(KingPosition.Row, 3);
            }
        }

        public override bool Execute(Board board)
        {
            new Normal(FromPos, ToPos).Execute(board);
            new Normal(RookFromPosition, RookToPosition).Execute(board);

            return false;
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++)
            {
                new Normal(kingPosInCopy, kingPosInCopy + KingMoveDirection).Execute(copy);
                kingPosInCopy += KingMoveDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
