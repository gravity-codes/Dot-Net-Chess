namespace ChessLogic
{
    public class GameManager
    {
        public Board Board { get; }
        public Player Player { get; private set; }
        public Result Result { get; private set; } = null;

        private int NoCaptureOrPawnMoves = 0;
        private string GameStateString;

        private readonly Dictionary<string, int> GameStateHistory = new Dictionary<string, int>();

        public GameManager(Board board, Player player)
        {
            Player = player;
            Board = board;

            GameStateString = new StateString(player, board).ToString();
            GameStateHistory[GameStateString] = 1;
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
            bool captureOrPawn = move.Execute(Board);
            
            if (captureOrPawn)
            {
                NoCaptureOrPawnMoves = 0;
            }
            else
            {
                NoCaptureOrPawnMoves++;
            }

            Player = Player.Opponent();
            UpdateGameStateHistory();
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
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
            else if (ThreeFoldRepition())
            {
                Result = Result.Draw(EndReason.ThreeFoldRepition);
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            int fullMoves = NoCaptureOrPawnMoves / 2;
            return fullMoves == 50;
        }

        private void UpdateGameStateHistory()
        {
            GameStateString = new StateString(Player, Board).ToString();

            if (!GameStateHistory.ContainsKey(GameStateString))
            {
                GameStateHistory[GameStateString] = 1;
            }
            else
            {
                GameStateHistory[GameStateString]++;
            }
        }

        private bool ThreeFoldRepition()
        {
            return GameStateHistory[GameStateString] == 3;
        }
    }
}
