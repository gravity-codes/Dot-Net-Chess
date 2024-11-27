namespace ChessLogic
{
    public class GameManager
    {
        public Board Board { get; }
        public Player Player { get; private set; }
        public Result Result { get; private set; } = null;

        public GameManager(Board board, Player player)
        {
            Player = player;
            Board = board;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if (Board.IsEmpty(pos) || Board[pos].Color != Player)
            {
                return Enumerable.Empty<Move>();
            }

            IEnumerable<Move> potentialMoves = Board[pos].GetMoves(pos, Board);
            return potentialMoves.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            Board.SetPawnSkipPositions(Player, null);
            move.Execute(Board);
            Player = Player.Opponent();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(Player).Any())
            {
                if (Board.IsInCheck(Player))
                {
                    Result = Result.Win(Player.Opponent());
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }
    }
}
