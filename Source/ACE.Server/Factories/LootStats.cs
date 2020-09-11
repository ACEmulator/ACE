using System.Collections.Generic;

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
        public float Potions { get; set; }
        public float HealingKit { get; set; }
        public float DinnerWare { get; set; }
        public float LevelEightComp { get; set; }
        public float MinorCantripCount { get; set; }
        public float MajorCantripCount { get; set; }
        public float EpicCantripCount { get; set; }
        public float LegendaryCantripCount { get; set; }


        // Tables
        public List<string> MeleeWeapons { get; set; }
        public List<string> MissileWeapons { get; set; }
        public List<string> CasterWeapons { get; set; }
        public List<string> Armor { get; set; }
        public List<string> Cloaks { get; set; }
        public List<string> Pets { get; set; }
        public List<string> Aetheria { get; set; }

        public List<string> Jewelry { get; set; }

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

        public LootStats(bool logstats)
        {
            // Counters
            MinMana = 50000;
            MinItemsCreated = 100;
            MinAL = 1000;

            // Tables
            if (logstats)
            {
                MeleeWeapons = new List<string>() { $"-----Melee Weapons----\nSkill,Wield,Damage,MStrike,Variance,DefenseMod,MagicDBonus,MissileDBonus,Cantrip,Value,Burden,Type" };
                MissileWeapons = new List<string>() { $"-----Missile Weapons----\nType,Wield,Modifier,ElementBonus,DefenseMod,MagicDBonus,MissileDBonus,Value,Burden" };
                CasterWeapons = new List<string>() { $"-----Caster Weapons----\nWield,ElementBonus,DefenseMod,MagicDBonus,MissileDBonus,Value,MaxMana,Burden" };
                Armor = new List<string>() { $"-----Armor----\nAL,Arcane,Value,Burden,Epic,Legendary,EquipmentSet,Type" };
                Pets = new List<string>() { $"-----Pet Devices----\nLevel,Dmg,DmgR,Crit,CritD,CDR,CritR,Total" };
                Aetheria = new List<string>() { $"-----Aetheria----\nColor,Level" };
                Cloaks = new List<string>() { $"-----Cloaks----\nLevel,Wield,Proc,Value,Set" };
                Jewelry = new List<string>() { $"-----Jewelry----\nSlot,Arcane,Value" };
            }
            else
            {
                MeleeWeapons = new List<string>() { $"-----Melee Weapons----\n Skill \t\t\t Wield \t Damage \t MStrike \t Variance \t DefenseMod \t MagicDBonus \t MissileDBonus\tCantrip\t Value\t Burden\t Type" };
                MissileWeapons = new List<string>() { $"-----Missile Weapons----\n Type \t Wield \t Modifier \tElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus\t Value\t Burden" };
                CasterWeapons = new List<string>() { $"-----Caster Weapons----\n Wield \t ElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus \t Value\t Burden\t MaxMana" };
                Armor = new List<string>() { $"-----Armor----\n AL\tArcane\tValue\tBurden\tEpics\tLegend\tEquipment Set\t\t\tType" };
                Pets = new List<string>() { $"-----Pet Devices----\n Level \t Dmg \t DmgR \t Crit \t CritD \t CDR \t CritR \t Total" };
                Aetheria = new List<string>() { $"-----Aetheria----\n Color \t Level" };
                Cloaks = new List<string>() { $"-----Cloaks----\n Level\t Wield\t Proc\t Value\t Set" };
                Jewelry = new List<string>() { $"-----Jewelry----\n Slot\t Arcane\t Value" };
            }
        }
    }
}
