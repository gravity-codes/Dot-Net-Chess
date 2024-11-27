namespace ChessLogic
{
    public class Counting
    {
        private readonly Dictionary<PieceType, int> WhiteCount = new();
        private readonly Dictionary<PieceType, int> BlackCount = new();

        public int TotalCount { get; private set; }

        public Counting()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                WhiteCount[type] = 0;
                BlackCount[type] = 0;
            }
        }

        public void Increment(Player color, PieceType type)
        {
            if (color == Player.White)
            {
                WhiteCount[type]++;
            }
            else if (color == Player.Black) 
            {
                BlackCount[type]++;
            }

            TotalCount++;
        }

        public int GetWhiteCount(PieceType type)
        {
            return WhiteCount[type];
        }

        public int GetBlackCount(PieceType type)
        {
            return BlackCount[type];
        }
    }
}
