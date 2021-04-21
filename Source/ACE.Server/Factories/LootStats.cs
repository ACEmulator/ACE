using System;
using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

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
                CasterWeapons = new List<string>() { $"-----Caster Weapons----\nWield,Element,ElementBonus,DefenseMod,MagicDBonus,MissileDBonus,Value,MaxMana,Burden" };
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
                CasterWeapons = new List<string>() { $"-----Caster Weapons----\n Wield \t Element \t ElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus \t Value\t Burden\t MaxMana" };
                Armor = new List<string>() { $"-----Armor----\n AL\tArcane\tValue\tBurden\tEpics\tLegend\tEquipment Set\t\t\tType" };
                Pets = new List<string>() { $"-----Pet Devices----\n Level \t Dmg \t DmgR \t Crit \t CritD \t CDR \t CritR \t Total" };
                Aetheria = new List<string>() { $"-----Aetheria----\n Color \t Level" };
                Cloaks = new List<string>() { $"-----Cloaks----\n Level\t Wield\t Proc\t Value\t Set" };
                Jewelry = new List<string>() { $"-----Jewelry----\n Slot\t Arcane\t Value" };
            }
        }

        public void AddItem(WorldObject wo, bool logStats)
        {
            // Weapon Properties 
            double missileDefMod = 0.00f;
            double magicDefMod = 0.00f;
            double wield = 0.00f;
            int value = 0;

            TotalItems++;

            // Loop depending on how many items you are creating
            for (int i = 0; i < 1; i++)
            {
                var testItem = wo;
                if (testItem == null)
                {
                    NullCount++;
                    continue;
                }

                switch (testItem.ItemType)
                {
                    case ItemType.None:
                        break;
                    case ItemType.MeleeWeapon:
                        MeleeWeaponCount++;

                        bool cantrip = false;
                        if (testItem.EpicCantrips.Count > 0)
                        {
                            cantrip = true;
                        }
                        if (testItem.LegendaryCantrips.Count > 0)
                            cantrip = true;

                        string strikeType = "N";
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.WeaponSkill == Skill.TwoHandedCombat)
                        {
                            if (logStats)
                            {
                                MeleeWeapons.Add($"{testItem.WeaponSkill},{wield},{testItem.Damage.Value},{strikeType},{testItem.DamageVariance.Value},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{cantrip},{value},{testItem.EncumbranceVal},{testItem.Name}");
                            }
                            else
                                MeleeWeapons.Add($"{testItem.WeaponSkill}\t {wield}\t {testItem.Damage.Value}\t\t {strikeType} \t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {cantrip}\t{value}\t{testItem.EncumbranceVal} \t {testItem.Name}");
                        }
                        else
                        {
                            if ((testItem.W_AttackType & AttackType.TripleStrike) != 0)
                            {
                                strikeType = "3x";
                            }
                            else if ((testItem.W_AttackType & AttackType.DoubleStrike) != 0)
                            {
                                strikeType = "2x";
                            }
                            if (logStats)
                            {
                                MeleeWeapons.Add($"{testItem.WeaponSkill},{wield},{testItem.Damage.Value},{strikeType},{testItem.DamageVariance.Value},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{cantrip},{value},{testItem.EncumbranceVal},{testItem.Name}");
                            }
                            else
                                MeleeWeapons.Add($" {testItem.WeaponSkill}\t\t {wield}\t {testItem.Damage.Value}\t\t {strikeType}\t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {cantrip}\t{value}\t{testItem.EncumbranceVal} \t {testItem.Name}");
                        }
                        break;
                    case ItemType.Armor:
                        ArmorCount++;
                        string equipmentSet = "None    ";
                        cantrip = false;
                        // float cantripSpells = 0;
                        var epicCantripSpells = testItem.EpicCantrips.Keys;

                        if (testItem.EpicCantrips.Count > 0)
                            cantrip = true;

                        if (testItem.LegendaryCantrips.Count > 0)
                            cantrip = true;

                        if (testItem.EquipmentSetId != null)
                            equipmentSet = testItem.EquipmentSetId.ToString();
                        if (logStats)
                        {
                            Armor.Add($"{testItem.ArmorLevel},{testItem.ItemDifficulty},{testItem.Value.Value},{testItem.EncumbranceVal},{testItem.EpicCantrips.Count},{testItem.LegendaryCantrips.Count},{equipmentSet},{testItem.Name}");
                        }
                        else
                            Armor.Add($" {testItem.ArmorLevel}\t{testItem.ItemDifficulty}\t{testItem.Value.Value}\t{testItem.EncumbranceVal}\t{testItem.EpicCantrips.Count}\t{testItem.LegendaryCantrips.Count}\t{equipmentSet}\t\t\t{testItem.Name}");
                        if (testItem.Name.Contains("Sheild"))   // typo?
                            break;
                        if (testItem.ArmorLevel > MaxAL)
                        {
                            MaxAL = testItem.ArmorLevel.Value;
                            MaxALItem = testItem.Name;
                        }
                        if (testItem.ArmorLevel < MinAL)
                        {
                            MinAL = testItem.ArmorLevel.Value;
                            MinALItem = testItem.Name;
                        }
                        break;
                    case ItemType.Clothing:
                        if (testItem.Name.Contains("Cloak"))
                        {
                            string cloakSet = "None ";
                            if (testItem.EquipmentSetId != null)
                                cloakSet = testItem.EquipmentSetId.ToString();
                            CloakCount++;
                            if (logStats)
                            {
                                Cloaks.Add($"{testItem.ItemMaxLevel},{testItem.WieldDifficulty},{testItem.CloakWeaveProc.Value},{testItem.Value.Value},{cloakSet}");
                            }
                            else
                                Cloaks.Add($" {testItem.ItemMaxLevel}\t {testItem.WieldDifficulty}\t {testItem.CloakWeaveProc.Value}\t {testItem.Value.Value}\t {cloakSet}");
                        }
                        else
                            ClothingCount++;
                        break;
                    case ItemType.Jewelry:
                        JewelryCount++;
                        string jewelrySlot = "";
                        switch (testItem.ValidLocations)
                        {
                            case EquipMask.NeckWear:
                                JewelryNecklaceCount++;
                                jewelrySlot = "Neck";
                                break;
                            case EquipMask.WristWear:
                                JewelryBraceletCount++;
                                jewelrySlot = "Brace";
                                break;
                            case EquipMask.FingerWear:
                                JewelryRingCount++;
                                jewelrySlot = "Ring";
                                break;
                            case EquipMask.TrinketOne:
                                JewelryTrinketCount++;
                                jewelrySlot = "Trink";
                                break;
                            default:
                                // Console.WriteLine(testItem.Name);                            
                                break;
                        }
                        if (logStats)
                        {
                            Jewelry.Add($"{jewelrySlot},{testItem.ItemDifficulty},{testItem.Value}");
                        }
                        else
                            Jewelry.Add($" {jewelrySlot}\t {testItem.ItemDifficulty}\t {testItem.Value}");


                        break;
                    case ItemType.Creature:
                        break;
                    case ItemType.Food:
                        Food++;
                        break;
                    case ItemType.Money:
                        break;
                    case ItemType.Misc:

                        string spirit = "Spirit";
                        string potionA = "Philtre";
                        string potionB = "Elixir";
                        string potionC = "Tonic";
                        string potionD = "Brew";
                        string potionE = "Potion";
                        string potionF = "Draught";
                        string potionG = "Tincture";

                        string healingKits = "Kit";
                        string spellcompGlyph = "Glyph";
                        string spellcompInk = "Ink";
                        string spellcompQuill = "Quill";

                        if (testItem.Name.Contains(spirit))
                        {
                            Spirits++;
                        }
                        else if (testItem is PetDevice petDevice)
                        {
                            PetsCount++;
                            int totalRatings = 0;
                            int damage = 0;
                            int damageResist = 0;
                            int crit = 0;
                            int critDamage = 0;
                            int critDamageResist = 0;
                            int critResist = 0;
                            int petLevel = 0;

                            if (petDevice.UseRequiresSkillLevel == 570)
                                petLevel = 200;
                            else if (petDevice.UseRequiresSkillLevel == 530)
                                petLevel = 180;
                            else if (petDevice.UseRequiresSkillLevel == 475)
                                petLevel = 150;
                            else if (petDevice.UseRequiresSkillLevel == 430)
                                petLevel = 125;
                            else if (petDevice.UseRequiresSkillLevel == 400)
                                petLevel = 100;
                            else if (petDevice.UseRequiresSkillLevel == 370)
                                petLevel = 80;
                            else if (petDevice.UseRequiresSkillLevel == 310)
                                petLevel = 50;

                            if (petDevice.GearDamage != null)
                            {
                                totalRatings += petDevice.GearDamage.Value;
                                damage = petDevice.GearDamage.Value;
                            }
                            if (petDevice.GearDamageResist != null)
                            {
                                totalRatings += petDevice.GearDamageResist.Value;
                                damageResist = petDevice.GearDamageResist.Value;
                            }
                            if (petDevice.GearCrit != null)
                            {
                                totalRatings += petDevice.GearCrit.Value;
                                crit = petDevice.GearCrit.Value;
                            }
                            if (petDevice.GearCritDamage != null)
                            {
                                totalRatings += petDevice.GearCritDamage.Value;
                                critDamage = petDevice.GearCritDamage.Value;
                            }
                            if (petDevice.GearCritDamageResist != null)
                            {
                                totalRatings += petDevice.GearCritDamageResist.Value;
                                critDamageResist = petDevice.GearCritDamageResist.Value;
                            }
                            if (petDevice.GearCritResist != null)
                            {
                                totalRatings += petDevice.GearCritResist.Value;
                                critResist = petDevice.GearCritResist.Value;
                            }
                            if (logStats)
                            {
                                Pets.Add($"{petLevel},{damage},{damageResist},{crit},{critDamage},{critDamageResist},{critResist},{totalRatings}");
                            }
                            else
                                Pets.Add($" {petLevel}\t {damage}\t {damageResist}\t {crit}\t {critDamage}\t {critDamageResist}\t {critResist}\t {totalRatings}");

                            if (totalRatings > 99)
                                PetRatingsOverHundred++;
                            else if (totalRatings > 89)
                                PetRatingsOverNinety++;
                            else if (totalRatings > 79)
                                PetRatingsOverEighty++;
                            else if (totalRatings > 69)
                                PetRatingsOverSeventy++;
                            else if (totalRatings > 59)
                                PetRatingsOverSixty++;
                            else if (totalRatings > 49)
                                PetRatingsOverFifty++;
                            else if (totalRatings > 39)
                                PetRatingsOverForty++;
                            else if (totalRatings > 29)
                                PetRatingsOverThirty++;
                            else if (totalRatings > 19)
                                PetRatingsOverTwenty++;
                            else if (totalRatings > 9)
                                PetRatingsOverTen++;
                            else if (totalRatings > 0)
                                PetRatingsEqualOne++;
                            else if (totalRatings < 1)
                                PetRatingsEqualZero++;
                        }
                        else if (testItem.Name.Contains(potionA) || testItem.Name.Contains(potionB) || testItem.Name.Contains(potionC) || testItem.Name.Contains(potionD) || testItem.Name.Contains(potionE) || testItem.Name.Contains(potionF) || testItem.Name.Contains(potionG))
                            Potions++;
                        else if (testItem.Name.Contains(spellcompGlyph) || testItem.Name.Contains(spellcompInk) || testItem.Name.Contains(spellcompQuill))
                            LevelEightComp++;
                        else if (testItem.Name.Contains(healingKits))
                            HealingKit++;
                        else
                        {
                            // Console.WriteLine($"ItemType.Misc Name={testItem.Name}");
                            Misc++;
                        }
                        break;
                    case ItemType.MissileWeapon:
                        double eleBonus = 0.00f;
                        double damageMod = 0.00f;
                        string missileType = "Other";
                        if (testItem.AmmoType != null)
                        {
                            switch (testItem.AmmoType.Value)
                            {
                                case AmmoType.None:
                                    break;
                                case AmmoType.Arrow:
                                    missileType = "Bow";
                                    MissileWeaponCount++;
                                    break;
                                case AmmoType.Bolt:
                                    missileType = "X Bow";
                                    MissileWeaponCount++;
                                    break;
                                case AmmoType.Atlatl:
                                    missileType = "Thrown";
                                    MissileWeaponCount++;
                                    break;
                                case AmmoType.ArrowCrystal:
                                    break;
                                case AmmoType.BoltCrystal:
                                    break;
                                case AmmoType.AtlatlCrystal:
                                    break;
                                case AmmoType.ArrowChorizite:
                                    break;
                                case AmmoType.BoltChorizite:
                                    break;
                                case AmmoType.AtlatlChorizite:
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.ElementalDamageBonus != null)
                            eleBonus = testItem.ElementalDamageBonus.Value;
                        if (testItem.DamageMod != null)
                            damageMod = testItem.DamageMod.Value;

                        if (missileType == "Other")
                        {
                            DinnerWare++;
                        }
                        else
                        {
                            if (logStats)
                            {
                                MissileWeapons.Add($"{missileType},{wield},{damageMod},{eleBonus},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{value},{testItem.EncumbranceVal}");
                            }
                            else
                                MissileWeapons.Add($"{missileType}\t {wield}\t {damageMod}\t\t{eleBonus}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.EncumbranceVal}");
                        }

                        break;
                    case ItemType.Container:
                        break;
                    case ItemType.Useless:
                        // Console.WriteLine($"ItemType.Useless Name={testItem.Name}");
                        break;
                    case ItemType.Gem:
                        string aetheriaColor = "None";
                        if (Server.Entity.Aetheria.IsAetheria(testItem.WeenieClassId))
                        {
                            AetheriaCount++;
                            if (testItem.WieldDifficulty == 75)
                                aetheriaColor = "Blue  ";
                            else if (testItem.WieldDifficulty == 150)
                                aetheriaColor = "Yellow";
                            else if (testItem.WieldDifficulty == 225)
                                aetheriaColor = "Red   ";
                            if (logStats)
                            {
                                Aetheria.Add($"{aetheriaColor},{testItem.ItemMaxLevel}");
                            }
                            else
                                Aetheria.Add($" {aetheriaColor}\t {testItem.ItemMaxLevel}");
                        }
                        else
                            GemCount++;
                        break;
                    case ItemType.SpellComponents:
                        SpellComponents++;
                        break;
                    case ItemType.Writable:
                        string scrolls = "Scroll";

                        if (testItem.Name.Contains(scrolls))
                            Scrolls++;
                        else
                            Console.WriteLine($"ItemType.Writeable Name={testItem.Name}");
                        break;
                    case ItemType.Key:
                        Key++;
                        break;
                    case ItemType.Caster:
                        CasterCount++;
                        double eleMod = 0.00f;
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.ElementalDamageMod != null)
                            eleMod = testItem.ElementalDamageMod.Value;
                        if (testItem.ItemMaxMana != null)
                            ItemMaxMana = testItem.ItemMaxMana.Value;
                        if (logStats)
                        {
                            CasterWeapons.Add($"{wield},{testItem.Name},{eleMod},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{value},{testItem.EncumbranceVal},{ItemMaxMana}");
                        }
                        else
                            CasterWeapons.Add($" {wield}\t {testItem.Name}\t {eleMod}\t\t {testItem.WeaponDefense.Value}\t\t  {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.EncumbranceVal} \t {ItemMaxMana}");
                        break;
                    case ItemType.Portal:
                        break;
                    case ItemType.Lockable:
                        break;
                    case ItemType.PromissoryNote:
                        break;
                    case ItemType.ManaStone:
                        ManaStone++;
                        break;
                    case ItemType.Service:
                        break;
                    case ItemType.MagicWieldable:
                        break;
                    case ItemType.CraftCookingBase:
                        OtherCount++;
                        break;
                    case ItemType.CraftAlchemyBase:
                        OtherCount++;
                        break;
                    case ItemType.CraftFletchingBase:
                        OtherCount++;
                        break;
                    case ItemType.CraftAlchemyIntermediate:
                        OtherCount++;
                        break;
                    case ItemType.CraftFletchingIntermediate:
                        OtherCount++;
                        break;
                    case ItemType.LifeStone:
                        break;
                    case ItemType.TinkeringTool:
                        OtherCount++;
                        break;
                    case ItemType.TinkeringMaterial:
                        OtherCount++;
                        break;
                    case ItemType.Gameboard:
                        break;
                    case ItemType.PortalMagicTarget:
                        break;
                    case ItemType.LockableMagicTarget:
                        break;
                    case ItemType.Vestements:
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.WeaponOrCaster:
                        break;
                    case ItemType.Item:
                        Console.WriteLine($"ItemType.item Name={testItem.Name}");
                        break;
                    case ItemType.RedirectableItemEnchantmentTarget:
                        break;
                    case ItemType.ItemEnchantableTarget:
                        break;
                    case ItemType.VendorShopKeep:
                        break;
                    case ItemType.VendorGrocer:
                        break;
                    default:
                        OtherCount++;
                        break;
                }
                /*switch (testItem.ItemType)
                {
                    case ItemType.Armor:
                        break;
                    case ItemType.MeleeWeapon:
                        break;
                    case ItemType.Caster:
                        break;
                    case ItemType.MissileWeapon:
                        break;
                    case ItemType.Jewelry:
                        break;
                    case ItemType.Gem:
                        break;
                    case ItemType.Clothing:
                        break;
                    default:
                        break;
                }*/
                if (testItem.ItemMaxMana != null)
                {
                    if (testItem.ItemMaxMana > MaxMana)
                        MaxMana = testItem.ItemMaxMana.Value;
                    if (testItem.ItemMaxMana < MinMana)
                        MinMana = testItem.ItemMaxMana.Value;
                    HasManaCount++;
                    TotalMaxMana += testItem.ItemMaxMana.Value;
                }
                if (testItem.Name == null)
                {
                    Console.WriteLine("*Name is Null*");
                    continue;
                }
                if (testItem.EpicCantrips.Count > 0)
                {
                    EpicCantripCount++;
                }
                if (testItem.LegendaryCantrips.Count > 0)
                {
                    LegendaryCantripCount++;
                }
            }
        }
    }
}
