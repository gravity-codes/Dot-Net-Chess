namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason EndReason { get; }

        public Result(Player player, EndReason endReason)
        {
            Winner = player;
            EndReason = endReason;
        }

        public static Result Win(Player player)
        {
            return new Result(player, EndReason.Checkmate);
        }

        public static Result Draw(EndReason endReason)
        {
            return new Result(Player.None, endReason);
        }
    }
}
