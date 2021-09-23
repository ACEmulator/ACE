using System;
using System.Collections.Generic;
using System.IO;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class LootGenerationFactory_Test
    {
        public static string TestLootGen(int numItems, int tier, bool logStats, string displayTable)
        {
            string displayHeader = $"\n LootFactory Simulator - Items\n ---------------------\n";

            Console.WriteLine($"Creating {numItems} items, that are in tier {tier}");

            var lootStats = new LootStats(logStats);

            // Create a dummy treasure profile for passing in tier value
            TreasureDeath profile = new TreasureDeath
            {
                Tier = tier,
                LootQualityMod = 0
            };

            // Loop depending on how many items you are creating
            for (int i = 0; i < numItems; i++)
            {
                var testItem = LootGenerationFactory.CreateRandomLootObjects(profile, true);
                lootStats.AddItem(testItem, logStats);
            }

            var displayStats = BuildDisplayStats(lootStats, displayTable);

            Console.WriteLine(displayHeader);
            Console.WriteLine(displayStats);

            displayHeader += $" A total of {lootStats.TotalItems} items were generated in Tier {tier}. \n";

            if (logStats)
            {
                string myfilename = string.Format("LootSim-{0:hh-mm-ss-tt_MM-dd-yyyy}.csv", DateTime.Now);
                File.WriteAllText(myfilename, displayHeader + displayStats);
            }

            return displayHeader;
        }

        public static string TestLootGenMonster(uint deathTreasureDID, int numCorpses, bool logStats, string displayTable)
        {
            string displayHeader = $"\n LootFactory Simulator - Corpses\n ---------------------\n";

            var corpseContainer = new List<WorldObject>();
            var ls = new LootStats(logStats);

            Console.WriteLine($"Creating {numCorpses} corpses.");

            var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(deathTreasureDID);
            if (deathTreasure != null)
            {
                displayHeader += $" Loot profile {deathTreasure.Id} (a Tier {deathTreasure.Tier} profile) from DID {deathTreasureDID} was used for creating {numCorpses} corpses. \n";
            }
            else
            {
                displayHeader += $" DID {deathTreasureDID} you specified is invalid. \n";
                return displayHeader;
            }

            for (int i = 0; i < numCorpses; i++)
            {
                if (deathTreasure != null)
                {
                    corpseContainer = LootGenerationFactory.CreateRandomLootObjects(deathTreasure);

                    if (corpseContainer.Count < ls.MinItemsCreated)
                        ls.MinItemsCreated = corpseContainer.Count;
                    if (corpseContainer.Count > ls.MaxItemsCreated)
                        ls.MaxItemsCreated = corpseContainer.Count;

                    foreach (var lootItem in corpseContainer)
                        ls.AddItem(lootItem, logStats);
                }
            }

            var displayStats = BuildDisplayStats(ls, displayTable);

            Console.WriteLine(displayHeader);
            Console.WriteLine(displayStats);

            displayHeader += $" A total of {ls.TotalItems} unique items were generated. \n";

            if (logStats)
            {
                string myfilename = $"LootSim_DeathTreasureDID-{deathTreasureDID}" + string.Format("_{0:hh-mm-ss-tt_MM-dd-yyyy}.csv", DateTime.Now);
                File.WriteAllText(myfilename, displayHeader + displayStats);
            }

            return displayHeader;
        }

        private static string BuildDisplayStats(LootStats ls, string displaytable)
        {
            string displayStats = "";
            // Seeing if a table was specified to display
            // fixme
            switch (displaytable)
            {
                case "melee":
                    displayStats += string.Join("\n", ls.MeleeWeapons) + $"\n";
                    break;
                case "missile":
                    displayStats += string.Join("\n", ls.MissileWeapons) + $"\n";
                    break;
                case "caster":
                    displayStats += string.Join("\n", ls.CasterWeapons) + $"\n";
                    break;
                case "jewelry":
                    displayStats += string.Join("\n", ls.Jewelry) + $"\n";
                    break;
                case "armor":
                    displayStats += string.Join("\n", ls.Armor) + $"\n";
                    break;
                case "cloak":
                    displayStats += string.Join("\n", ls.Cloaks) + $"\n";
                    break;
                case "pet":
                    displayStats += string.Join("\n", ls.Pets) + $"\n";
                    break;
                case "aetheria":
                    displayStats += string.Join("\n", ls.Aetheria) + $"\n";
                    break;
                case "all":
                    displayStats += string.Join("\n", ls.MeleeWeapons) + $"\n";
                    displayStats += string.Join("\n", ls.MissileWeapons) + $"\n";
                    displayStats += string.Join("\n", ls.CasterWeapons) + $"\n";
                    displayStats += string.Join("\n", ls.Jewelry) + $"\n";
                    displayStats += string.Join("\n", ls.Armor) + $"\n";
                    displayStats += string.Join("\n", ls.Cloaks) + $"\n";
                    displayStats += string.Join("\n", ls.Pets) + $"\n";
                    displayStats += string.Join("\n", ls.Aetheria) + $"\n";
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
                    $"Aetheria={ls.AetheriaCount} \n " +
                    $"Clothing={ls.ClothingCount} \n " +
                    $"Cloaks={ls.CloakCount} \n " +
                    $"\n Generic Items \n " +
                    $"---- \n " +
                    $"Food={ls.Food} \n " +
                    $"SpellComps={ls.SpellComponents} \n " +
                    $"Keys={ls.Key} \n " +
                    $"ManaStones={ls.ManaStone} \n " +
                    $"Pets={ls.PetsCount} \n " +
                    $"EncapSpirits={ls.Spirits} \n " +
                    $"Scrolls={ls.Scrolls} \n " +
                    $"Potions={ls.Potions} \n " +
                    $"Healing Kits={ls.HealingKit} \n " +
                    $"Level 8 Comps={ls.LevelEightComp} \n " +
                    $"DinnerWare={ls.DinnerWare} \n " +
                    $"Misc={ls.Misc} \n " +
                    $"Other={ls.OtherCount} \n " +
                    $"NullCount={ls.NullCount} \n " +
                    $"Total Found={ls.ArmorCount + ls.MeleeWeaponCount + ls.CasterCount + ls.MissileWeaponCount + ls.JewelryCount + ls.GemCount + ls.ClothingCount + ls.Food + ls.SpellComponents + ls.Key + ls.ManaStone + ls.PetsCount + ls.Spirits + ls.Scrolls + ls.Potions + ls.LevelEightComp + ls.HealingKit + ls.DinnerWare + ls.Misc + ls.OtherCount + ls.NullCount} \n " +
                    $"TotalGenerated={ls.TotalItems}\n");

            displayStats += $"\n Drop Rates \n ----\n " +
                                $"Armor= {ls.ArmorCount / ls.TotalItems * 100}% \n " +
                                $"MeleeWeapon= {ls.MeleeWeaponCount / ls.TotalItems * 100}% \n " +
                                $"Caster= {ls.CasterCount / ls.TotalItems * 100}% \n " +
                                $"MissileWeapon= {ls.MissileWeaponCount / ls.TotalItems * 100}% \n " +
                                $"Jewelry= {ls.JewelryCount / ls.TotalItems * 100}% \n " +
                                $"Gem= {ls.GemCount / ls.TotalItems * 100}% \n " +
                                $"Aetheria= {ls.AetheriaCount / ls.TotalItems * 100}% \n " +
                                $"Clothing= {ls.ClothingCount / ls.TotalItems * 100}% \n " +
                                $"Cloaks= {ls.CloakCount / ls.TotalItems * 100}% \n " +
                                $"Food= {ls.Food / ls.TotalItems * 100}% \n " +
                                $"SpellComps= {ls.SpellComponents / ls.TotalItems * 100}% \n " +
                                $"Keys= {ls.Key / ls.TotalItems * 100}% \n " +
                                $"Mana Stones= {ls.ManaStone / ls.TotalItems * 100}% \n " +
                                $"Pets= {ls.PetsCount / ls.TotalItems * 100}% \n " +
                                $"Encap. Spirits= {ls.Spirits / ls.TotalItems * 100}% \n " +
                                $"Scrolls= {ls.Scrolls / ls.TotalItems * 100}% \n " +
                                $"Potions= {ls.Potions / ls.TotalItems * 100}% \n " +
                                $"Healing Kits= {ls.HealingKit / ls.TotalItems * 100}% \n " +
                                $"Level 8 Comps= {ls.LevelEightComp / ls.TotalItems * 100}% \n " +
                                $"DinnerWare= {ls.DinnerWare / ls.TotalItems * 100}% \n " +
                                $"Misc= {ls.Misc / ls.TotalItems * 100}% \n " +
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
            // Jewelry
            displayStats += ($"\n Jewelry Counts Stats \n ----\n " +
                                $"Necklace = {ls.JewelryNecklaceCount}\t Droprate = {ls.JewelryNecklaceCount / ls.JewelryCount * 100}%\n" +
                                $" Bracelet = {ls.JewelryBraceletCount}\t Droprate = {ls.JewelryBraceletCount / ls.JewelryCount * 100}%\n" +
                                $"     Ring = {ls.JewelryRingCount}\t Droprate = {ls.JewelryRingCount / ls.JewelryCount * 100}%\n" +
                                $"  Trinket = {ls.JewelryTrinketCount}\t Droprate = {ls.JewelryTrinketCount / ls.JewelryCount * 100}%\n");
            // Cantrip Counts
            displayStats += ($"\n Cantip Counts Stats \n ----\n" +
                    //$"     Minor = {ls.MinorCantripCount}\t Droprate = {ls.MinorCantripCount / ls.TotalItems * 100}%\n" +
                    //$"     Major = {ls.MajorCantripCount}\t Droprate = {ls.MajorCantripCount / ls.TotalItems * 100}%\n" +
                    $"      Epic = {ls.EpicCantripCount}\t Droprate = {ls.EpicCantripCount / ls.TotalItems * 100}%\n" +
                    $" Legendary = {ls.LegendaryCantripCount}\t Droprate = {ls.LegendaryCantripCount / ls.TotalItems * 100}%\n");

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
    }
}
