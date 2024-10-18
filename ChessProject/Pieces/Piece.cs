namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected IEnumerable<Position> MovePositionsInDirection(Position from, Board board, Direction direction)
        {
            for (Position pos = from + direction; Board.IsInside(pos); pos += direction)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                }
                else
                { 
                    if (board[pos].Color != Color)
                    {
                        yield return pos;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        protected IEnumerable<Position> MovePositionsInDirection(Position from, Board board, Direction[] directions)
        {
            return directions.SelectMany(direction => MovePositionsInDirection(from, board, direction));
        }
    }
}
