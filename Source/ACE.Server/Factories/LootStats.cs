using System;
using System.Collections.Generic;
using System.Text;

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
        public float GemCount { get; set; }
        public float ClothingCount { get; set; }
        public float OtherCount { get; set; }
        public float NullCount { get; set; }
        public int MinItemsCreated { get; set; }
        public int MaxItemsCreated { get; set; }
        public int SpellComponents { get; set; }
        public int Food { get; set; }
        public int Key { get; set; }
        public int ManaStone { get; set; }
        public int Misc { get; set; }
        public int TotalItems { get; set; }
        public int Scrolls { get; set; }
        public int PetsCount { get; set; }
        public int Spirits { get; set; }
        public int Poitions { get; set; }
        public int HealingKit { get; set; }
        public int DinnerWare { get; set; }
        public int LevelEightComp { get; set; }

        // Tables
        public string MeleeWeapons { get; set; }
        public string MissileWeapons { get; set; }
        public string CasterWeapons { get; set; }
        public string Armor { get; set; }
        public string Pets { get; set; }

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
