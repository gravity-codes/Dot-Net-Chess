namespace ChessLogic
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPos { get; }
        public override Position ToPos {  get; }
        private readonly PieceType newType;

        public PawnPromotion(Position pos, Position to, PieceType newType)
        {
            FromPos = pos;
            ToPos = to;
            this.newType = newType;
        }

        private Piece CreatePromotionPiece(Player player)
        {
            return newType switch
            {
                PieceType.Knight => new Knight(player),
                PieceType.Bishop => new Bishop(player),
                PieceType.Rook => new Rook(player),
                _ => new Queen(player)
            };
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPos];
            board[FromPos] = null;

            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true;
            board[ToPos] = promotionPiece;

            return true;
        }
    }
}
