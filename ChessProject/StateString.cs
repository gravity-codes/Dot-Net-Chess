using System.Text;

namespace ChessLogic
{
    public class StateString
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public StateString(Player currentPlayer, Board board) 
        {
            AddPiecePlacement(board);
            stringBuilder.Append(' ');
            AddCurrentPlayer(currentPlayer);
            stringBuilder.Append(' ');
            AddCastlingRights(board);
            stringBuilder.Append(' ');
            AddEnPassant(board, currentPlayer);
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        private static char PieceChar(Piece piece)
        {
            char c = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' '
            };

            if (piece.Color == Player.White)
            {
                return char.ToUpper(c);
            }

            return c;
        }

        private void AddRowData(Board board, int row)
        {
            int empty = 0;

            for (int c  = 0; c < 8; c++)
            {
                if (board[row, c] == null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    stringBuilder.Append(empty);
                    empty = 0;
                }

                stringBuilder.Append(PieceChar(board[row, c]));
            }

            if (empty > 0)
            {
                stringBuilder.Append(empty);
            }
        }

        private void AddPiecePlacement(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                if (r != 0)
                {
                    stringBuilder.Append('/');
                }

                AddRowData(board, r);
            }
        }

        private void AddCurrentPlayer(Player player)
        {
            if (player == Player.White)
            {
                stringBuilder.Append('w');
            }
            else
            {
                stringBuilder.Append('b');
            }
        }

        private void AddCastlingRights(Board board)
        {
            bool castleWKS = board.CastleRightKS(Player.White);
            bool castleWQS = board.CastleRightQS(Player.White);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

            if (!(castleWKS || castleWQS || castleBKS || castleBKS))
            {
                stringBuilder.Append('-');
                return;
            }

            if (castleWKS)
            {
                stringBuilder.Append('K');
            }

            if (castleWQS)
            {
                stringBuilder.Append('Q');
            }

            if(castleBKS)
            {
                stringBuilder.Append('k');
            }

            if (castleBQS)
            {
                stringBuilder.Append('q');
            }
        }

        private void AddEnPassant(Board board, Player player)
        {
            if (!board.CanCaptureEnPassant(player))
            {
                stringBuilder.Append('-');
                return;
            }

            Position pos = board.GetPawnSkipPosition(player.Opponent());
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;
            stringBuilder.Append(file);
            stringBuilder.Append(rank);
        }
    }
}
