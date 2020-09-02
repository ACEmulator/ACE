using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Entity
{
    public class TreasureItemTypeChance_Orig
    {
        public TreasureItemType_Orig TreasureItemType;
        public float Chance;

        public TreasureItemTypeChance_Orig(TreasureItemType_Orig treasureItemType, float chance)
        {
            TreasureItemType = treasureItemType;
            Chance = chance;
        }
    }
}
