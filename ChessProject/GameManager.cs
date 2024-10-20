namespace ChessLogic
{
    public class GameManager
    {
        public Board Board { get; }
        public Player Player { get; private set; }

        public GameManager(Board board, Player player) 
        {
            Player = player;
            Board = board;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if (Board.IsEmpty(pos) || Board[pos].Color !=  Player)
            {
                return Enumerable.Empty<Move>();
            }

            IEnumerable<Move> potentialMoves = Board[pos].GetMoves(pos, Board);
            return potentialMoves.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            move.Execute(Board);
            Player = Player.Opponent();
        }
    }
}
