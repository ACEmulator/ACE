namespace ACE.Server.Entity
{
    public class ItemProfile
    {
        // original data struct
        public int Amount;      // sent as int, not as uint -- needs to be verified > 0
        public uint ObjectGuid;

        // extended server data
        public uint WeenieClassId;
        public int? Palette;
        public double? Shade;

        /// <summary>
        /// If false, should be rejected as early as possible
        /// </summary>
        public bool IsValidAmount => Amount > 0;

        public ItemProfile(int amount, uint objectGuid)
        {
            Amount = amount;
            ObjectGuid = objectGuid;
        }
    }
}
