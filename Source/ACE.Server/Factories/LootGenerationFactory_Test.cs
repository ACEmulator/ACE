using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using log4net;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Factories
{
      public static class LootGenerationFactory_Test
    {
        public static string TestLootGen(int numItems, int tier, bool logstats, string displaytable)
        {
            string displayHeader = $"\n LootFactory Simulator - Items\n ---------------------\n";

            Console.WriteLine($"Creating {numItems} items, that are in tier {tier}");

             var ls = SetLootStatsDefaults(new LootStats());

            // Loop depending on how many items you are creating
            for (int i = 0; i < numItems; i++)
            {
                var testItem = LootGenerationFactory.CreateRandomLootObjects(tier, true);
                ls = LootStats(testItem, ls);
            }
            Console.WriteLine(displayHeader);
            Console.WriteLine(DisplayStats(ls, displaytable));
            displayHeader += $" A total of {ls.TotalItems} items were generated in Tier {tier}. \n";

            return displayHeader;
        }
        public static string TestLootGenMonster(uint wcid, int numberofcorpses, bool logstats, string displaytable)
        {
            string displayHeader = $"\n LootFactory Simulator - Corpses\n ---------------------\n";

            var corpseContainer = new List<WorldObject>();
            var ls = SetLootStatsDefaults(new LootStats());

            Console.WriteLine($"Creating {numberofcorpses} corpses.");
            
                var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(wcid);
            if (deathTreasure != null)
            {
               displayHeader += $" Loot profile {deathTreasure.Id} (a Tier {deathTreasure.Tier} profile) from DID {wcid} was used for creating {numberofcorpses} corpses. \n";
            }
            else
            {
                displayHeader += $" DID {wcid} you specified is invalid. \n";
                return displayHeader;
            }
            for (int i = 0; i < numberofcorpses; i++)
            {
                if (deathTreasure != null)
                {
                    corpseContainer = LootGenerationFactory.CreateRandomLootObjects(deathTreasure);
                    if (corpseContainer.Count < ls.MinItemsCreated)
                        ls.MinItemsCreated = corpseContainer.Count;
                    if (corpseContainer.Count > ls.MaxItemsCreated)
                        ls.MaxItemsCreated = corpseContainer.Count;

                    foreach (var lootItem in corpseContainer)
                    {
                        ls = LootStats(lootItem, ls);
                    }
                }
                else
                {
                }
            }
            Console.WriteLine(displayHeader);
            Console.WriteLine(DisplayStats(ls, displaytable));
            displayHeader += $" A total of {ls.TotalItems} unique items were generated.";
            return displayHeader;
        }
        public static LootStats LootStats(WorldObject wo, LootStats ls)
        {          
            // Weapon Properties 
            double missileDefMod = 0.00f;
            double magicDefMod = 0.00f;
            double wield = 0.00f;
            int value = 0;

            ls.TotalItems++;

            // Loop depending on how many items you are creating
            for (int i = 0; i < 1; i++)
            {
                var testItem = wo;
                if (testItem is null)
                {
                    ls.NullCount++;
                    continue;
                }
                string itemType = testItem.ItemType.ToString();
                if (itemType == null)
                {
                    ls.NullCount++;

                    continue;
                }
   
                switch (testItem.ItemType)
                {
                    case ItemType.None:
                        break;
                    case ItemType.MeleeWeapon:
                        ls.MeleeWeaponCount++;
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
                            ls.MeleeWeapons = ls.MeleeWeapons + $" {testItem.WeaponSkill}\t {wield}\t {testItem.Damage.Value}\t\t {strikeType} \t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.Name}\n";
                        else
                        {                         
                            AttackType attackType = testItem.W_AttackType;
                            string at = attackType.ToString("F");
                            if (at.Contains("DoubleSlash") || at.Contains("DoubleThrust"))
                            {
                                strikeType = "2x";
                            }
                            else if (at.Contains("TripleSlash") || at.Contains("TripleThrust"))
                            {
                                strikeType = "3x";
                            }
                            ls.MeleeWeapons = ls.MeleeWeapons + $" {testItem.WeaponSkill}\t\t {wield}\t {testItem.Damage.Value}\t\t {strikeType}\t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.Name}\n";
                        }
                        break;
                    case ItemType.Armor:
                        ls.ArmorCount++;
                        ls.Armor += $" {testItem.ArmorLevel}\t {testItem.Value.Value} \t {testItem.Name}\n";
                        if (testItem.Name.Contains("Sheild"))
                            break;
                        if (testItem.ArmorLevel > ls.MaxAL)
                        {
                            ls.MaxAL = testItem.ArmorLevel.Value;
                            ls.MaxALItem = testItem.Name;
                        }
                        if (testItem.ArmorLevel < ls.MinAL)
                        {
                            ls.MinAL = testItem.ArmorLevel.Value;
                            ls.MinALItem = testItem.Name;
                        }
                        break;
                    case ItemType.Clothing:
                        ls.ClothingCount++;
                        break;
                    case ItemType.Jewelry:
                        ls.JewelryCount++;
                        break;
                    case ItemType.Creature:
                        break;
                    case ItemType.Food:
                        ls.Food++;
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
                            ls.Spirits++;
                        }
                        else if (testItem is PetDevice petDevice)
                        {
                            ls.PetsCount++;
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
                            ls.Pets += $" {petLevel}\t {damage}\t {damageResist}\t {crit}\t {critDamage}\t {critDamageResist}\t {critResist}\t {totalRatings}\n";

                            if (totalRatings > 99)
                                ls.PetRatingsOverHundred++;
                            else if (totalRatings > 89)
                                ls.PetRatingsOverNinety++;
                            else if (totalRatings > 79)
                                ls.PetRatingsOverEighty++;
                            else if (totalRatings > 69)
                                ls.PetRatingsOverSeventy++;
                            else if (totalRatings > 59)
                                ls.PetRatingsOverSixty++;
                            else if (totalRatings > 49)
                                ls.PetRatingsOverFifty++;
                            else if (totalRatings > 39)
                                ls.PetRatingsOverForty++;
                            else if (totalRatings > 29)
                                ls.PetRatingsOverThirty++;
                            else if (totalRatings > 19)
                                ls.PetRatingsOverTwenty++;
                            else if (totalRatings > 9)
                                ls.PetRatingsOverTen++;
                            else if (totalRatings > 0)
                                ls.PetRatingsEqualOne++;                           
                            else if (totalRatings < 1)
                                ls.PetRatingsEqualZero++;
                        }
                        else if (testItem.Name.Contains(potionA) || testItem.Name.Contains(potionB) || testItem.Name.Contains(potionC) || testItem.Name.Contains(potionD) || testItem.Name.Contains(potionE) || testItem.Name.Contains(potionF) || testItem.Name.Contains(potionG))
                            ls.Poitions++;
                        else if (testItem.Name.Contains(spellcompGlyph) || testItem.Name.Contains(spellcompInk) || testItem.Name.Contains(spellcompQuill))
                            ls.LevelEightComp++;
                        else if (testItem.Name.Contains(healingKits))
                            ls.HealingKit++;
                        else
                        {
                            Console.WriteLine($"ItemType.Misc Name={testItem.Name}");
                            ls.Misc++;
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
                                case ACE.Entity.Enum.AmmoType.None:
                                    break;
                                case ACE.Entity.Enum.AmmoType.Arrow:
                                    missileType = " Bow";
                                    ls.MissileWeaponCount++;
                                    break;
                                case ACE.Entity.Enum.AmmoType.Bolt:
                                    missileType = " X Bow";
                                    ls.MissileWeaponCount++;
                                    break;
                                case ACE.Entity.Enum.AmmoType.Atlatl:
                                    missileType = " Thrown";
                                    ls.MissileWeaponCount++;
                                    break;
                                case ACE.Entity.Enum.AmmoType.ArrowCrystal:
                                    break;
                                case ACE.Entity.Enum.AmmoType.BoltCrystal:
                                    break;
                                case ACE.Entity.Enum.AmmoType.AtlatlCrystal:
                                    break;
                                case ACE.Entity.Enum.AmmoType.ArrowChorizite:
                                    break;
                                case ACE.Entity.Enum.AmmoType.BoltChorizite:
                                    break;
                                case ACE.Entity.Enum.AmmoType.AtlatlChorizite:
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
                            ls.DinnerWare++;
                        }
                        else
                            ls.MissileWeapons = ls.MissileWeapons + $"{missileType}\t {wield}\t {Math.Round(damageMod, 2)}\t\t{eleBonus}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\n";
                        break;
                    case ItemType.Container:
                        break;
                    case ItemType.Useless:
                        Console.WriteLine($"ItemType.Useless Name={testItem.Name}");
                        break;
                    case ItemType.Gem:
                        ls.GemCount++;
                        break;
                    case ItemType.SpellComponents:
                        ls.SpellComponents++;
                        break;
                    case ItemType.Writable:
                        string scrolls = "Scroll";
                        
                        if (testItem.Name.Contains(scrolls))
                            ls.Scrolls++;
                        else
                            Console.WriteLine($"ItemType.Writeable Name={testItem.Name}");
                        break;
                    case ItemType.Key:
                        ls.Key++;
                        break;
                    case ItemType.Caster:
                        ls.CasterCount++;
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
                            ls.ItemMaxMana = testItem.ItemMaxMana.Value;
                        ls.CasterWeapons = ls.CasterWeapons + $" {wield}\t {eleMod}\t\t {testItem.WeaponDefense.Value}\t\t  {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {ls.ItemMaxMana}\n";
                        break;
                    case ItemType.Portal:
                        break;
                    case ItemType.Lockable:
                        break;
                    case ItemType.PromissoryNote:
                        break;
                    case ItemType.ManaStone:
                        ls.ManaStone++;
                        break;
                    case ItemType.Service:
                        break;
                    case ItemType.MagicWieldable:
                        break;
                    case ItemType.CraftCookingBase:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftAlchemyBase:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftFletchingBase:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftAlchemyIntermediate:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftFletchingIntermediate:
                        ls.OtherCount++;
                        break;
                    case ItemType.LifeStone:
                        break;
                    case ItemType.TinkeringTool:
                        ls.OtherCount++;
                        break;
                    case ItemType.TinkeringMaterial:
                        ls.OtherCount++;
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
                        ls.OtherCount++;
                        break;
                }
                switch (itemType)
                {
                    case "Armor":                        
                        break;
                    case "MeleeWeapon":                      
                        break;
                    case "Caster":                        
                        break;
                    case "MissileWeapon":                     
                        break;
                    case "Jewelry":
                        break;
                    case "Gem":
                        break;
                    case "Clothing":                       
                        break;
                    default:

                        break;
                }
                if (testItem.ItemMaxMana != null)
                {
                    if (testItem.ItemMaxMana > ls.MaxMana)
                        ls.MaxMana = testItem.ItemMaxMana.Value;
                    if (testItem.ItemMaxMana < ls.MinMana)
                        ls.MinMana = testItem.ItemMaxMana.Value;
                    ls.HasManaCount++;
                    ls.TotalMaxMana = ls.TotalMaxMana + testItem.ItemMaxMana.Value;
                }
                if (testItem == null)
                {
                    Console.WriteLine("*Name is Null*");
                    continue;
                }
                else
                {
                }
            }
            return ls;
        }
        private static string DisplayStats(LootStats ls, string displaytable)
        {
            string displayStats = "";
            // Seeing if a table was specified to display
            switch (displaytable)
            {
                case "melee":
                    displayStats += ls.MeleeWeapons + $"\n";
                    break;
                case "missile":
                    displayStats += ls.MissileWeapons + $"\n";
                    break;
                case "caster":
                    displayStats += ls.CasterWeapons + $"\n";
                    break;
                case "armor":
                    displayStats += ls.Armor + $"\n";
                    break;
                case "pet":
                    displayStats += ls.Pets + $"\n";
                    break;
                case "all":
                    displayStats += ls.MeleeWeapons + $"\n";
                    displayStats += ls.MissileWeapons + $"\n";
                    displayStats += ls.CasterWeapons + $"\n";
                    displayStats += ls.Armor + $"\n";
                    displayStats += ls.Pets + $"\n";
                    break;
                default:
                    displayStats += $"\n No Table(s) was selected to display, showing only general statistics";
                    break;
            }

            displayStats += ($" \n Treasure Items \n " +
                    $"---- \n " +
                    $"Armor={ls.ArmorCount} \n " +
                    $"MeleeWeapon={ls.MeleeWeaponCount} \n " +
                    $"Caster={ls.CasterCount} \n " +
                    $"MissileWeapon={ls.MissileWeaponCount} \n " +
                    $"Jewelry={ls.JewelryCount} \n " +
                    $"Gem={ls.GemCount} \n " +
                    $"Clothing={ls.ClothingCount} \n " +
                    $"\n Generic Items \n " +
                    $"---- \n " +
                    $"Food={ls.Food} \n " +
                    $"SpellComps={ls.SpellComponents} \n " +
                    $"Keys={ls.Key} \n " +
                    $"ManaStones={ls.ManaStone} \n " +
                    $"Pets={ls.PetsCount} \n " +
                    $"EncapSpirits={ls.Spirits} \n " +
                    $"Scrolls={ls.Scrolls} \n " +
                    $"Potions={ls.Poitions} \n " +
                    $"Healing Kits={ls.HealingKit} \n " +
                    $"Level 8 Comps={ls.LevelEightComp} \n " +
                    $"DinnerWare={ls.DinnerWare} \n " +
                    $"Misc={ls.Misc} \n " +
                    $"Other={ls.OtherCount} \n " +
                    $"NullCount={ls.NullCount} \n " +
                    $"Total Found={ls.ArmorCount + ls.MeleeWeaponCount + ls.CasterCount + ls.MissileWeaponCount + ls.JewelryCount + ls.GemCount + ls.ClothingCount + ls.Food + ls.SpellComponents + ls.Key + ls.ManaStone + ls.PetsCount + ls.Spirits + ls.Scrolls + ls.Poitions + ls.LevelEightComp + ls.HealingKit + ls.DinnerWare + ls.Misc + ls.OtherCount + ls.NullCount} \n " +
                    $"TotalGenerated={ls.TotalItems}\n");

            displayStats += $"\n Drop Rates \n ----\n " +
                                $"Armor= {ls.ArmorCount / ls.TotalItems * 100}% \n " +
                                $"MeleeWeapon= {ls.MeleeWeaponCount / ls.TotalItems * 100}% \n " +
                                $"Caster= {ls.CasterCount / ls.TotalItems * 100}% \n " +
                                $"MissileWeapon= {ls.MissileWeaponCount / ls.TotalItems * 100}% \n " +
                                $"Jewelry= {ls.JewelryCount / ls.TotalItems * 100}% \n " +
                                $"Gem= {ls.GemCount / ls.TotalItems * 100}% \n " +
                                $"Clothing= {ls.ClothingCount / ls.TotalItems * 100}% \n " +
                                $"Other={ls.OtherCount / ls.TotalItems * 100}% \n";

            // Armor Level Stats
            displayStats += $"\n Armor Levels \n ----\n MinAL = {ls.MinAL}\t {ls.MinALItem}\n MaxAL = {ls.MaxAL}\t {ls.MaxALItem}\n";

            // Pet Summons Stats
            displayStats += ($"\n Pets Ratings Stats \n ----\n " +
                                $"  100+ = {ls.PetRatingsOverHundred} \n" +
                                $"  90-99 = {ls.PetRatingsOverNinety} \n" +
                                $"  80-89 = {ls.PetRatingsOverEighty} \n" +
                                $"  70-79 = {ls.PetRatingsOverSeventy} \n" +
                                $"  60-69 = {ls.PetRatingsOverSixty} \n" +
                                $"  50-59 = {ls.PetRatingsOverFifty} \n" +
                                $"  40-49 = {ls.PetRatingsOverForty} \n" +
                                $"  30-39 = {ls.PetRatingsOverThirty} \n" +
                                $"  20-29 = {ls.PetRatingsOverTwenty} \n" +
                                $"  10-19 = {ls.PetRatingsOverTen} \n" +
                                $"    1-9 = {ls.PetRatingsEqualOne} \n" +
                                $"      0 = {ls.PetRatingsEqualZero} \n" +                                
                                $" Total Pets Generated = {ls.PetsCount} \n");

            if (ls.HasManaCount == 0)
            {
            }
            else
            {
                displayStats += $"\n Mana capacity across all items Min={ls.MinMana}  Max={ls.MaxMana} Avg Mana={ls.TotalMaxMana / ls.HasManaCount}";
            }
            if (ls.MinItemsCreated == 100)
            { }
            else
            {
                displayStats += $"\n Min Items on a corpse = {ls.MinItemsCreated}, Max Items on coprse = {ls.MaxItemsCreated} \n";
            }
            return displayStats;
        }
        public static LootStats SetLootStatsDefaults(LootStats ls)
        {
            // Counters
            ls.ArmorCount = 0;
            ls.MeleeWeaponCount = 0;
            ls.CasterCount = 0;
            ls.MissileWeaponCount = 0;
            ls.JewelryCount = 0;
            ls.GemCount = 0;
            ls.ClothingCount = 0;
            ls.OtherCount = 0;
            ls.NullCount = 0;
            ls.ItemMaxMana = 0;
            ls.MinMana = 50000;
            ls.MaxMana = 0;
            ls.HasManaCount = 0;
            ls.TotalMaxMana = 0;
            ls.MinItemsCreated = 100;
            ls.MaxItemsCreated = 0;
            ls.ManaStone = 0;
            ls.Key = 0;
            ls.Food = 0;
            ls.Misc = 0;
            ls.SpellComponents = 0;
            ls.TotalItems = 0;
            ls.Scrolls = 0;
            ls.PetsCount = 0;
            ls.Spirits = 0;
            ls.Poitions = 0;
            ls.HealingKit = 0;
            ls.DinnerWare = 0;
            ls.LevelEightComp = 0;
            ls.PetsTotalRatings = 0;
            ls.PetRatingsEqualZero = 0;
            ls.PetRatingsEqualOne = 0;
            ls.PetRatingsOverTen = 0;
            ls.PetRatingsOverTwenty = 0;
            ls.PetRatingsOverThirty = 0;
            ls.PetRatingsOverForty = 0;
            ls.PetRatingsOverFifty = 0;
            ls.PetRatingsOverSixty = 0;
            ls.PetRatingsOverSeventy = 0;
            ls.PetRatingsOverEighty = 0;
            ls.PetRatingsOverNinety = 0;
            ls.PetRatingsOverHundred = 0;
            ls.MinAL = 1000;
            ls.MaxAL = 0;

            // Tables
            ls.MeleeWeapons = $"-----Melee Weapons----\n Skill \t\t\t Wield \t Damage \t MStrike \t Variance \t DefenseMod \t MagicDBonus \t MissileDBonus\t Value\t Type \n";
            ls.MissileWeapons = $"-----Missile Weapons----\n Type \t Wield \t Modifier \tElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus\t Value\n";
            ls.CasterWeapons = $"-----Caster Weapons----\n Wield \t ElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus \t Value \t MaxMana\n";
            ls.Armor = $"-----Armor----\n AL \t Value \t Type\n";
            ls.Pets = $"-----Pet Devices----\n Level \t Dmg \t DmgR \t Crit \t CritD \t CDR \t CritR \t Total \n";

            return ls;
        }
        public static string LogStats()
        {
            string test = "";
                return test;
        }
    }
}
