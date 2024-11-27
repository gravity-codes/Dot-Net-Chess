namespace ChessLogic
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant;
        public override Position FromPos { get; }
        public override Position ToPos {  get; }
        private readonly Position CapturePosition;

        public EnPassant(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
            CapturePosition = new Position(fromPos.Row, ToPos.Column);
        }

        public override void Execute(Board board)
        {
            new Normal(FromPos, ToPos).Execute(board);
            board[CapturePosition] = null;
        }
    }
}
