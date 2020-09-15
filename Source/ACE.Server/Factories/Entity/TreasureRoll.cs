using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Entity
{
    public class TreasureRoll
    {
        public TreasureItemType_Orig ItemType;
        public TreasureArmorType ArmorType;
        public TreasureWeaponType WeaponType;

        public WeenieClassName Wcid;

        public TreasureRoll() { }

        public TreasureRoll(TreasureItemType_Orig itemType)
        {
            ItemType = itemType;
        }

        public string GetItemType()
        {
            switch (ItemType)
            {
                case TreasureItemType_Orig.Armor:
                    return ArmorType.ToString();
                case TreasureItemType_Orig.Weapon:
                    return WeaponType.ToString();
            }
            return ItemType.ToString();
        }
    }
}
