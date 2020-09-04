namespace ACE.Server.Factories
{
    public class LootStats
    {
        // Counters
        public float ArmorCount { get; set; }
        public float MeleeWeaponCount { get; set; }
        public float CasterCount { get; set; }
        public float MissileWeaponCount { get; set; }
        public float JewelryCount { get; set; }
        public float JewelryNecklaceCount { get; set; }
        public float JewelryBraceletCount { get; set; }
        public float JewelryRingCount { get; set; }
        public float JewelryTrinketCount { get; set; }
        public float GemCount { get; set; }
        public float AetheriaCount { get; set; }        
        public float ClothingCount { get; set; }
        public float CloakCount { get; set; }
        public float OtherCount { get; set; }
        public float NullCount { get; set; }
        public int MinItemsCreated { get; set; }
        public int MaxItemsCreated { get; set; }
        public float SpellComponents { get; set; }
        public float Food { get; set; }
        public float Key { get; set; }
        public float ManaStone { get; set; }
        public float Misc { get; set; }
        public float TotalItems { get; set; }
        public float Scrolls { get; set; }
        public float PetsCount { get; set; }
        public float Spirits { get; set; }
        public float Poitions { get; set; }
        public float HealingKit { get; set; }
        public float DinnerWare { get; set; }
        public float LevelEightComp { get; set; }
        public float MinorCantripCount { get; set; }
        public float MajorCantripCount { get; set; }
        public float EpicCantripCount { get; set; }
        public float LegendaryCantripCount { get; set; }


        // Tables
        public string MeleeWeapons { get; set; }
        public string MissileWeapons { get; set; }
        public string CasterWeapons { get; set; }
        public string Armor { get; set; }
        public string Cloaks { get; set; }
        public string Pets { get; set; }
        public string Aetheria { get; set; }

        public string Jewelry { get; set; }

        // Item Stats
        public int ItemMaxMana { get; set; }
        public int MinMana { get; set; }
        public int MaxMana { get; set; }
        public int HasManaCount { get; set; }
        public int TotalMaxMana { get; set; }
        public int MinAL { get; set; }
        public int MaxAL { get; set; }
        public string MinALItem { get; set; }
        public string MaxALItem { get; set; }
        

        // Pet Stats
        public int PetsTotalRatings { get; set; }
        public int PetRatingsEqualZero { get; set; }
        public int PetRatingsEqualOne { get; set; }
        public int PetRatingsOverTen { get; set; }
        public int PetRatingsOverTwenty { get; set; }
        public int PetRatingsOverThirty { get; set; }
        public int PetRatingsOverForty { get; set; }
        public int PetRatingsOverFifty { get; set; }
        public int PetRatingsOverSixty { get; set; }
        public int PetRatingsOverSeventy { get; set; }
        public int PetRatingsOverEighty { get; set; }
        public int PetRatingsOverNinety { get; set; }
        public int PetRatingsOverHundred { get; set; }
    }
}
