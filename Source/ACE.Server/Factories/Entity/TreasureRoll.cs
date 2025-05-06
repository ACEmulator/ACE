using ACE.Server.Factories.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Entity
{
    public class TreasureRoll
    {
        public TreasureItemType ItemType;
        public TreasureArmorType ArmorType;
        public TreasureWeaponType WeaponType;

        public WeenieClassName Wcid;

        public int BaseArmorLevel;

        /// <summary>
        /// A cumulative addon to the ItemDifficulty / Arcane Lore requirement
        /// </summary>
        public float ItemDifficulty;

        public TreasureRoll() { }

        public TreasureRoll(TreasureItemType itemType)
        {
            ItemType = itemType;
        }

        public string GetItemType()
        {
            switch (ItemType)
            {
                case TreasureItemType.Armor:
                    return ArmorType.ToString();
                case TreasureItemType.Weapon:
                    return WeaponType.ToString();
            }
            return ItemType.ToString();
        }

        /// <summary>
        /// Returns TRUE if this roll is for a MeleeWeapon / MissileWeapon / Caster
        /// </summary>
        public bool IsWeapon => WeaponType != TreasureWeaponType.Undef;

        public bool IsMeleeWeapon => WeaponType.IsMeleeWeapon();

        public bool IsMissileWeapon => WeaponType.IsMissileWeapon();

        public bool IsCaster => WeaponType.IsCaster();

        /// <summary>
        /// Returns TRUE if this roll is for a piece of armor
        /// (clothing w/ armor level)
        /// </summary>
        public bool IsArmor => ArmorType != TreasureArmorType.Undef;

        public bool IsClothing => ItemType == TreasureItemType.Clothing;

        public bool IsCloak => ItemType == TreasureItemType.Cloak;

        /// <summary>
        /// Returns TRUE if wo has an ArmorLevel > 0
        /// </summary>
        public bool HasArmorLevel(WorldObject wo)
        {
            return (wo.ArmorLevel ?? 0) > 0;
        }

        public bool IsGem => ItemType == TreasureItemType.Gem;

        public bool IsJewelry => ItemType == TreasureItemType.Jewelry;

        public bool IsDinnerware => ItemType == TreasureItemType.ArtObject;
    }
}
