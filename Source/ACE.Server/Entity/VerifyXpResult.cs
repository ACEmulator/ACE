namespace ACE.Server.Entity
{
    public class VerifyXpResult
    {
        public OfflinePlayer Player;
        public long Calculated;
        public long Current;
        public long Diff => Current - Calculated;

        public VerifyXpResult(OfflinePlayer player, long calculated, long current)
        {
            Player = player;
            Calculated = calculated;
            Current = current;
        }
    }
}
