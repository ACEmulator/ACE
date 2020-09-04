using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Entity
{
    public class TreasureItemTypeChance
    {
        public TreasureItemType TreasureItemType;
        public float Chance;

        public TreasureItemTypeChance(TreasureItemType treasureItemType, float chance)
        {
            TreasureItemType = treasureItemType;
            Chance = chance;
        }
    }
}
