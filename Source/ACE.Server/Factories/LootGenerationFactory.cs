using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static LootGenerationFactory()
        {
            InitRares();
        }

        public static List<WorldObject> CreateRandomObjectsOfType(WeenieType type, int count)
        {
            var weenies = DatabaseManager.World.GetRandomWeeniesOfType((int)type, count);

            var worldObjects = new List<WorldObject>();

            foreach (var weenie in weenies)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);
                worldObjects.Add(wo);
            }

            return worldObjects;
        }

        // Enum used for adjusting the loot bias for the various types of Mana forge chests
        public enum LootBias : uint
        {
            UnBiased = 0,
            Armor = 1,
            Weapons = 2,
            SpellComps = 4,
            Clothing = 8,
            Jewelry = 16,
            MagicEquipment = 31,
            MixedEquipment = 31,
        }

        public static List<WorldObject> CreateRandomLootObjects(TreasureDeath profile)
        {
            int numItems;
            WorldObject lootWorldObject;

            LootBias lootBias = LootBias.UnBiased;
            var loot = new List<WorldObject>();

            switch (profile.TreasureType)
            {
                case 1001:  // Mana Forge Chest, Advanced Equipment Chest, and Mixed Equipment Chest
                    lootBias = LootBias.MixedEquipment;
                    break;
                case 1002:  // Armor Chest
                    lootBias = LootBias.Armor;
                    break;
                case 1003:  // Magic Chest
                    lootBias = LootBias.MagicEquipment;
                    break;
                case 1004:  // Weapon Chest
                    lootBias = LootBias.Weapons;
                    break;
                default:    // Default to unbiased loot profile
                    break;
            }

            var itemChance = ThreadSafeRandom.Next(1, 100);
            if (itemChance <= profile.ItemChance)
            {
                numItems = ThreadSafeRandom.Next(profile.ItemMinAmount, profile.ItemMaxAmount);
                for (var i = 0; i < numItems; i++)
                {
                    if (lootBias == LootBias.MagicEquipment)
                        lootWorldObject = CreateRandomLootObjects(profile.Tier, false, LootBias.Weapons);
                    else
                        lootWorldObject = CreateRandomLootObjects(profile.Tier, false, lootBias);
                    if (lootWorldObject != null)
                        loot.Add(lootWorldObject);
                }
            }

            if (itemChance <= profile.MagicItemChance)
            {
                numItems = ThreadSafeRandom.Next(profile.MagicItemMinAmount, profile.MagicItemMaxAmount);

                bool aetheriaGenerated = false;
                bool generateAetheria = false;
                double dropRate = PropertyManager.GetDouble("aetheria_drop_rate").Item;
                double dropRateMod = 1.0 / dropRate;

                for (var i = 0; i < numItems; i++)
                {
                    // Coalesced Aetheria doesn't drop in loot tiers less than 5
                    // According to wiki, Weapon Mana Forge chests don't drop Aetheria, also
                    // a loot role will only drop one Coealesced Aetheria per call into loot system, as I don't remember there
                    // being multiples, and I didn't find any written mention of it.
                    if (!aetheriaGenerated && profile.Tier > 4 && lootBias != LootBias.Weapons && dropRate > 0)
                        generateAetheria = ThreadSafeRandom.Next(1, (int)(100 * dropRateMod)) == 1;     // base 1% of all magical items aetheria?
                    else
                        generateAetheria = false;

                    if (generateAetheria)
                    {
                        lootWorldObject = CreateAetheria(profile.Tier);
                        if (lootWorldObject != null)
                        {
                            loot.Add(lootWorldObject);
                            aetheriaGenerated = true;
                        }
                    }
                    else
                    {
                        lootWorldObject = CreateRandomLootObjects(profile.Tier, true, lootBias);
                        if (lootWorldObject != null)
                            loot.Add(lootWorldObject);
                    }
                }
            }

            if (itemChance <= profile.MundaneItemChance)
            {
                numItems = ThreadSafeRandom.Next(profile.MundaneItemMinAmount, profile.MundaneItemMaxAmount);
                for (var i = 0; i < numItems; i++)
                {
                    if (lootBias != LootBias.UnBiased)
                        lootWorldObject = CreateRandomScroll(profile.Tier);
                    else
                        lootWorldObject = CreateMundaneObjects(profile.Tier);

                    if (lootWorldObject != null)
                        loot.Add(lootWorldObject);
                }
            }

            // 25% chance to drop a scroll
            itemChance = ThreadSafeRandom.Next(0, 3);
            if (itemChance == 3)
            {
                if (lootBias == LootBias.UnBiased && profile.MagicItemMinAmount > 0)
                {
                    lootWorldObject = CreateRandomScroll(profile.Tier);

                    if (lootWorldObject != null)
                        loot.Add(lootWorldObject);
                }
            }

            if (lootBias != LootBias.Armor && lootBias != LootBias.Weapons && lootBias != LootBias.MagicEquipment && profile.MagicItemMinAmount > 0)
            {
                // 17% chance to drop a summoning essence
                itemChance = ThreadSafeRandom.Next(1, 6);
                if (itemChance == 6)
                {
                    lootWorldObject = CreateSummoningEssence(profile.Tier);

                    if (lootWorldObject != null)
                        loot.Add(lootWorldObject);
                }

                // Roll for a 1 in 50 chance to drop an Encapsulated Spirit
                itemChance = ThreadSafeRandom.Next(1, 50);
                if (itemChance == 50)
                {
                    var encapSpirit = WorldObjectFactory.CreateNewWorldObject(49485);

                    if (encapSpirit != null)
                        loot.Add(encapSpirit);
                }
            }

            return loot;
        }

        /// <summary>
        /// This is currently just a function to help test some functionality in the loot gen system.
        /// </summary>
        /// <param name="wcid"></param>
        /// <param name="tier"></param>
        /// <returns></returns>
        public static WorldObject CreateLootByWCID(uint wcid, int tier)
        {
            int longDescDecoration = 5;

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(wcid);

            if (wo == null)
                return null;

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));

            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, longDescDecoration);
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            if (wo.TsysMutationData != null)
            {
                int newMaterialType = GetMaterialType(wo, tier);
                if (newMaterialType > 0)
                {
                    wo.MaterialType = (MaterialType)newMaterialType;
                    wo = RandomizeColor(wo);
                }
            }

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            return wo;
        }

        public static WorldObject CreateRandomLootObjects(int tier, bool isMagical, LootBias lootBias = LootBias.UnBiased)
        {
            int type;
            WorldObject wo;

            switch (lootBias)
            {
                case LootBias.Armor:
                    type = 2;
                    break;
                case LootBias.Weapons:
                    type = 3;
                    break;
                case LootBias.Jewelry:
                    type = 4;
                    break;
                default:
                    type = ThreadSafeRandom.Next(1, 4);
                    break;
            }

            switch (type)
            {
                case 1:
                    //jewels
                    wo = CreateJewels(tier, isMagical);
                    return wo;
                case 2:
                    //armor
                    wo = CreateArmor(tier, isMagical, lootBias);
                    return wo;
                case 3:
                    //weapons
                    wo = CreateWeapon(tier, isMagical);
                    return wo;
                case 4:
                default:
                    //jewelry
                    wo = CreateJewelry(tier, isMagical);
                    return wo;
            }
        }

        private static WorldObject CreateWeapon(int tier, bool isMagical)
        {
            int chance = ThreadSafeRandom.Next(1, 3);

            switch (chance)
            {
                case 1:
                    return CreateMeleeWeapon(tier, isMagical);
                case 2:
                    return CreateMissileWeapon(tier, isMagical);
                default:
                    return CreateCaster(tier, isMagical);
            }
        }

        private static void Shuffle<T>(T[] array)
        {
            Random _r = new Random();
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + _r.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        private static int GetWield(int tier, int type)
        {

            int wield = 0;
            int chance = ThreadSafeRandom.Next(1, 100);

            ////Types: 1 Missiles, 2 Casters, 3 melee weapons, 4 covenant armor
            switch (type)
            {
                case 1:
                    switch (tier)
                    {
                        case 1:
                            wield = 0;
                            break;
                        case 2:
                            if (chance < 60)
                                wield = 0;
                            else
                                wield = 250;
                            break;
                        case 3:
                            if (chance < 60)
                                wield = 0;
                            else if (chance < 90)
                                wield = 250;
                            else
                                wield = 270;
                            break;
                        case 4:
                            if (chance < 60)
                                wield = 0;
                            else if (chance < 90)
                                wield = 250;
                            else
                                wield = 270;
                            break;
                        case 5:
                            if (chance < 60)
                                wield = 270;
                            else if (chance < 90)
                                wield = 290;
                            else
                                wield = 315;
                            break;
                        case 6:
                            if (chance < 60)
                                wield = 315;
                            else if (chance < 90)
                                wield = 335;
                            else
                                wield = 360;
                            break;
                        case 7:
                            if (chance < 60)
                                wield = 335;
                            else if (chance < 90)
                                wield = 360;
                            else
                                wield = 375;
                            break;
                        case 8:
                            if (chance < 60)
                                wield = 360;
                            else if (chance < 90)
                                wield = 375;
                            else
                                wield = 385;
                            break;
                    }
                    break;
                case 2:
                    switch (tier)
                    {
                        case 1:
                        case 2:
                        case 3:
                            if (chance < 80)
                                wield = 0;
                            else
                                wield = 290;
                            break;
                        case 4:
                            if (chance < 60)
                                wield = 0;
                            else
                                wield = 290;
                            break;
                        case 5:
                            if (chance < 60)
                                wield = 0;
                            else if (chance < 90)
                                wield = 290;
                            else
                                wield = 310;
                            break;
                        case 6:
                            if (chance < 40)
                                wield = 0;
                            else if (chance < 70)
                                wield = 310;
                            else if (chance < 90)
                                wield = 330;
                            else
                                wield = 355;
                            break;
                        case 7:
                            if (chance < 30)
                                wield = 0;
                            else if (chance < 60)
                                wield = 330;
                            else if (chance < 90)
                                wield = 355;
                            else
                                wield = 375;
                            break;
                        case 8:
                            if (chance < 25)
                                wield = 0;
                            else if (chance < 50)
                                wield = 355;
                            else if (chance < 85)
                                wield = 375;
                            else
                                wield = 385;
                            break;
                    }
                    break;
                case 3:
                    switch (tier)
                    {
                        case 1:
                            wield = 0;
                            break;
                        case 2:
                            if (chance < 60)
                                wield = 0;
                            else
                                wield = 250;
                            break;
                        case 3:
                            if (chance < 60)
                                wield = 0;
                            else if (chance < 90)
                                wield = 250;
                            else
                                wield = 300;
                            break;
                        case 4:
                            if (chance < 60)
                                wield = 0;
                            else if (chance < 90)
                                wield = 250;
                            else
                                wield = 300;
                            break;
                        case 5:
                            if (chance < 60)
                                wield = 300;
                            else if (chance < 90)
                                wield = 325;
                            else
                                wield = 350;
                            break;
                        case 6:
                            if (chance < 60)
                                wield = 350;
                            else if (chance < 90)
                                wield = 370;
                            else
                                wield = 400;
                            break;
                        case 7:
                            if (chance < 60)
                                wield = 370;
                            else if (chance < 90)
                                wield = 400;
                            else
                                wield = 420;
                            break;
                        case 8:
                            if (chance < 60)
                                wield = 400;
                            else if (chance < 90)
                                wield = 420;
                            else
                                wield = 430;
                            break;
                    }
                    break;
            }

            return wield;
        }

        private static double GetMaxDamageMod(int tier, int maxDamageMod)
        {
            double damageMod = 0;
            int chance = 0;
            switch (maxDamageMod)
            {
                case 15:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 80)
                                damageMod = .01;
                            else if (chance < 95)
                                damageMod = .02;
                            else
                                damageMod = .03;
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .03;
                            else if (chance < 80)
                                damageMod = .04;
                            else if (chance < 95)
                                damageMod = .05;
                            else
                                damageMod = .06;
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .06;
                            else if (chance < 80)
                                damageMod = .07;
                            else if (chance < 95)
                                damageMod = .08;
                            else
                                damageMod = .09;
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .09;
                            else if (chance < 80)
                                damageMod = .10;
                            else if (chance < 95)
                                damageMod = .11;
                            else
                                damageMod = .12;
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .09;
                            else if (chance < 80)
                                damageMod = .10;
                            else if (chance < 95)
                                damageMod = .11;
                            else
                                damageMod = .12;
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .11;
                            else if (chance < 80)
                                damageMod = .12;
                            else
                                damageMod = .13;
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .13;
                            else if (chance < 80)
                                damageMod = .14;
                            else
                                damageMod = .15;
                            break;
                    }
                    break;
                case 18:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 80)
                                damageMod = .01;
                            else if (chance < 95)
                                damageMod = .02;
                            else
                                damageMod = .03;
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .03;
                            else if (chance < 80)
                                damageMod = .04;
                            else if (chance < 95)
                                damageMod = .05;
                            else
                                damageMod = .06;
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .06;
                            else if (chance < 80)
                                damageMod = .07;
                            else if (chance < 95)
                                damageMod = .08;
                            else
                                damageMod = .09;
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .09;
                            else if (chance < 80)
                                damageMod = .10;
                            else if (chance < 95)
                                damageMod = .11;
                            else
                                damageMod = .12;
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .12;
                            else if (chance < 80)
                                damageMod = .13;
                            else if (chance < 95)
                                damageMod = .14;
                            else
                                damageMod = .15;
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .15;
                            else if (chance < 80)
                                damageMod = .16;
                            else
                                damageMod = .17;
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .16;
                            else if (chance < 80)
                                damageMod = .17;
                            else
                                damageMod = .18;
                            break;
                    }
                    break;
                case 20:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 80)
                                damageMod = .01;
                            else if (chance < 95)
                                damageMod = .02;
                            else
                                damageMod = .03;
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .02;
                            else if (chance < 80)
                                damageMod = .03;
                            else if (chance < 95)
                                damageMod = .04;
                            else
                                damageMod = .05;
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .05;
                            else if (chance < 80)
                                damageMod = .06;
                            else if (chance < 95)
                                damageMod = .07;
                            else
                                damageMod = .08;
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .07;
                            else if (chance < 80)
                                damageMod = .08;
                            else if (chance < 95)
                                damageMod = .09;
                            else
                                damageMod = .10;
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .11;
                            else if (chance < 80)
                                damageMod = .12;
                            else if (chance < 95)
                                damageMod = .13;
                            else
                                damageMod = .14;
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .13;
                            else if (chance < 80)
                                damageMod = .14;
                            else if (chance < 90)
                                damageMod = .15;
                            else
                                damageMod = .16;
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .17;
                            else if (chance < 80)
                                damageMod = .18;
                            else if (chance < 90)
                                damageMod = .19;
                            else
                                damageMod = .20;
                            break;
                    }
                    break;
                case 22:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 80)
                                damageMod = .01;
                            else if (chance < 95)
                                damageMod = .02;
                            else
                                damageMod = .03;
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .02;
                            else if (chance < 80)
                                damageMod = .03;
                            else if (chance < 95)
                                damageMod = .04;
                            else
                                damageMod = .05;
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .05;
                            else if (chance < 80)
                                damageMod = .06;
                            else if (chance < 95)
                                damageMod = .07;
                            else
                                damageMod = .08;
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .07;
                            else if (chance < 80)
                                damageMod = .08;
                            else if (chance < 95)
                                damageMod = .09;
                            else
                                damageMod = .10;
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .11;
                            else if (chance < 80)
                                damageMod = .12;
                            else if (chance < 95)
                                damageMod = .13;
                            else
                                damageMod = .14;
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .14;
                            else if (chance < 80)
                                damageMod = .15;
                            else if (chance < 90)
                                damageMod = .16;
                            else
                                damageMod = .17;
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .18;
                            else if (chance < 80)
                                damageMod = .19;
                            else if (chance < 90)
                                damageMod = .20;
                            else if (chance < 95)
                                damageMod = .21;
                            else
                                damageMod = .22;
                            break;
                    }
                    break;
                case 25:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 80)
                                damageMod = .01;
                            else if (chance < 70)
                                damageMod = .02;
                            else if (chance < 90)
                                damageMod = .03;
                            else
                                damageMod = .04;
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .04;
                            else if (chance < 80)
                                damageMod = .05;
                            else if (chance < 95)
                                damageMod = .06;
                            else
                                damageMod = .07;
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .07;
                            else if (chance < 70)
                                damageMod = .08;
                            else if (chance < 90)
                                damageMod = .09;
                            else if (chance < 96)
                                damageMod = .10;
                            else
                                damageMod = .11;
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .11;
                            else if (chance < 70)
                                damageMod = .12;
                            else if (chance < 90)
                                damageMod = .13;
                            else if (chance < 96)
                                damageMod = .14;
                            else
                                damageMod = .15;
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .15;
                            else if (chance < 70)
                                damageMod = .16;
                            else if (chance < 90)
                                damageMod = .17;
                            else
                                damageMod = .18;
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .18;
                            else if (chance < 80)
                                damageMod = .19;
                            else if (chance < 90)
                                damageMod = .20;
                            else
                                damageMod = .21;
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 50)
                                damageMod = .21;
                            else if (chance < 80)
                                damageMod = .22;
                            else if (chance < 90)
                                damageMod = .23;
                            else if (chance < 95)
                                damageMod = .24;
                            else
                                damageMod = .25;
                            break;
                    }
                    break;
                default:
                    break;
            }
            double damageMod2 = 1.0 + damageMod;

            return damageMod2;
        }

        public static double GetManaRate(WorldObject wo)
        {
            double manaRate;

            switch (wo.WeenieType)
            {
                case WeenieType.MissileLauncher:
                    manaRate = 0.04166667;
                    break;
                default:
                    manaRate = 1.0 / ThreadSafeRandom.Next(10, 30);
                    break;
            }

            return -manaRate;
        }

        private static WorldObject AssignMagic(WorldObject wo, int tier)
        {
            int[][] spells;
            int[][] cantrips;

            int lowSpellTier = GetLowSpellTier(tier);
            int highSpellTier = GetHighSpellTier(tier);

            double manaRate = GetManaRate(wo);

            switch (wo.WeenieType)
            {
                case WeenieType.Clothing:
                    spells = LootTables.ArmorSpells;
                    cantrips = LootTables.ArmorCantrips;
                    break;
                case WeenieType.Caster:
                    spells = LootTables.WandSpells;
                    cantrips = LootTables.WandCantrips;
                    break;
                case WeenieType.Generic:
                    spells = LootTables.JewelrySpells;
                    cantrips = LootTables.JewelryCantrips;
                    break;
                case WeenieType.MeleeWeapon:
                    spells = LootTables.MeleeSpells;
                    cantrips = LootTables.MeleeCantrips;
                    break;
                case WeenieType.MissileLauncher:
                    spells = LootTables.MissileSpells;
                    cantrips = LootTables.MissileCantrips;
                    break;
                default:
                    spells = null;
                    cantrips = null;
                    break;
            }

            if (spells == null || cantrips == null)
                return wo;

            wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

            wo.SetProperty(PropertyFloat.ManaRate, manaRate);

            int numSpells = GetSpellDistribution(tier, out int minorCantrips, out int majorCantrips, out int epicCantrips, out int legendaryCantrips);
            int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

            int spellcraft = GetSpellcraft(numSpells, tier);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));

            int maxMana = GetMaxMana(numSpells, tier);
            wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
            wo.SetProperty(PropertyInt.ItemCurMana, maxMana);

            int[] shuffledValues = new int[spells.Length];
            for (int i = 0; i < spells.Length; i++)
            {
                shuffledValues[i] = i;
            }

            Shuffle(shuffledValues);

            if (numSpells - numCantrips > 0)
            {
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = ThreadSafeRandom.Next(lowSpellTier - 1, highSpellTier - 1);
                    int spellID = spells[shuffledValues[a]][col];
                    wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                }
            }

            if (numCantrips > 0)
            {
                shuffledValues = new int[cantrips.Length];
                for (int i = 0; i < cantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                }
            }

            return wo;
        }

        private static int GetSpellDistribution(int tier, out int numMinors, out int numMajors, out int numEpics, out int numLegendaries)
        {
            int numNonCantrips = 0;

            numMinors = 0;
            numMajors = 0;
            numEpics = 0;
            numLegendaries = 0;

            int nonCantripChance = ThreadSafeRandom.Next(1, 100);

            numMinors = GetNumMinorCantrips(tier); // All tiers have a chance for at least one minor cantrip

            switch (tier)
            {
                case 1:
                    // 1-3 w/ chance of minor cantrip
                    if (nonCantripChance <= 50)
                        numNonCantrips = 1;
                    else if (nonCantripChance <= 90)
                        numNonCantrips = 2;
                    else
                        numNonCantrips = 3;
                    break;

                case 2:
                    // 3-4 w/ chance of either minor or major
                    if (nonCantripChance <= 50)
                        numNonCantrips = 3;
                    else
                        numNonCantrips = 4;
                    break;

                case 3:
                    // 4-5 w/ chance of either major or minor
                    if (nonCantripChance <= 50)
                        numNonCantrips = 4;
                    else
                        numNonCantrips = 5;

                    numMajors = GetNumMajorCantrips(tier);
                    break;

                case 4:
                    // 5-6, major and minor
                    if (nonCantripChance <= 50)
                        numNonCantrips = 5;
                    else
                        numNonCantrips = 6;

                    numMajors = GetNumMajorCantrips(tier);
                    break;

                case 5:
                    // 5-7 major and minor
                    if (nonCantripChance <= 50)
                        numNonCantrips = 5;
                    else if (nonCantripChance <= 90)
                        numNonCantrips = 6;
                    else
                        numNonCantrips = 7;

                    numMajors = GetNumMajorCantrips(tier);
                    break;

                case 6:
                    // 6-7, minor(4 total) major(2 total)
                    if (nonCantripChance <= 50)
                        numNonCantrips = 6;
                    else
                        numNonCantrips = 7;

                    numMajors = GetNumMajorCantrips(tier);
                    break;

                case 7:
                    /// 6-7, minor(4), major(3), epic(4)
                    if (nonCantripChance <= 50)
                        numNonCantrips = 6;
                    else
                        numNonCantrips = 7;

                    numMajors = GetNumMajorCantrips(tier);
                    numEpics = GetNumEpicCantrips(tier);
                    break;

                default:
                    // 6-7, minor(4), major(3), epic(4), legendary(2)
                    if (nonCantripChance <= 50)
                        numNonCantrips = 6;
                    else
                        numNonCantrips = 7;

                    numMajors = GetNumMajorCantrips(tier);
                    numEpics = GetNumEpicCantrips(tier);
                    numLegendaries = GetNumLegendaryCantrips(tier);
                    break;
            }

            return numNonCantrips + numMinors + numMajors + numEpics + numLegendaries;
        }

        private static int GetLowSpellTier(int tier)
        {
            int lowSpellTier = 0;

            switch (tier)
            {
                case 1:
                    lowSpellTier = 1;
                    break;
                case 2:
                    lowSpellTier = 3;
                    break;
                case 3:
                    lowSpellTier = 4;
                    break;
                case 4:
                    lowSpellTier = 5;
                    break;
                case 5:
                case 6:
                    lowSpellTier = 6;
                    break;
                default:
                    lowSpellTier = 7;
                    break;
            }

            return lowSpellTier;
        }

        private static int GetHighSpellTier(int tier)
        {
            int highSpellTier = 0;

            switch (tier)
            {
                case 1:
                    highSpellTier = 3;
                    break;
                case 2:
                    highSpellTier = 5;
                    break;
                case 3:
                case 4:
                    highSpellTier = 6;
                    break;
                case 5:
                case 6:
                    highSpellTier = 7;
                    break;
                default:
                    highSpellTier = 8;
                    break;
            }

            return highSpellTier;
        }

        private static int GetSkillLevelLimit(int wield)
        {

            double percentage = (double)ThreadSafeRandom.Next(75, 98);
            int skill = (int)(percentage * (double)wield);

            return skill;
        }

        private static double GetManaCMod(int tier)
        {
            int magicMod = 0;

            int chance = 0;
            switch (tier)
            {
                case 1:
                case 2:
                    magicMod = 0;
                    break;
                case 3:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 900)
                        magicMod = 5;
                    else if (chance > 800)
                        magicMod = 4;
                    else if (chance > 700)
                        magicMod = 3;
                    else if (chance > 600)
                        magicMod = 2;
                    else if (chance > 500)
                        magicMod = 1;
                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
            }

            double manaDMod = magicMod / 100.0;

            return manaDMod;
        }

        private static double GetMissileDMod(int tier)
        {
            double missileMod = 0;

            switch (tier)
            {
                case 1:
                case 2:
                    missileMod = 0;
                    break;
                case 3:
                    int chance = ThreadSafeRandom.Next(0, 100);
                    if (chance > 95)
                        missileMod = .005;
                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance > 95)
                        missileMod = .01;
                    else if (chance > 80)
                        missileMod = .005;
                    else
                        missileMod = 0;
                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 950)
                        missileMod = .01;
                    else if (chance > 800)
                        missileMod = .005;
                    else
                        missileMod = 0;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 975)
                        missileMod = .020;
                    else if (chance > 900)
                        missileMod = .015;
                    else if (chance > 800)
                        missileMod = .010;
                    else if (chance > 700)
                        missileMod = .005;
                    else
                        missileMod = 0;
                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 990)
                        missileMod = .030;
                    else if (chance > 985)
                        missileMod = .025;
                    else if (chance > 950)
                        missileMod = .020;
                    else if (chance > 900)
                        missileMod = .015;
                    else if (chance > 850)
                        missileMod = .01;
                    else if (chance > 800)
                        missileMod = .005;
                    else
                        missileMod = 0;
                    break;
                default: // tier 8
                    chance = ThreadSafeRandom.Next(0, 1000);
                    if (chance > 998)
                        missileMod = .04;
                    else if (chance > 994)
                        missileMod = .035;
                    else if (chance > 990)
                        missileMod = .03;
                    else if (chance > 985)
                        missileMod = .025;
                    else if (chance > 950)
                        missileMod = .02;
                    else if (chance > 900)
                        missileMod = .015;
                    else if (chance > 850)
                        missileMod = .01;
                    else if (chance > 800)
                        missileMod = .005;
                    else
                        missileMod = 0;
                    break;
            }

            double m2 = 1.0 + missileMod;

            return m2;
        }

        private static int GetValue(int tier, int work, double gemMod, double matMod)
        {
            ///This is just a placeholder. This doesnt return a final value used retail, just a quick value for now.
            ///Will use, tier, material type, amount of gems set into item, type of gems, spells on item
            //int value = ThreadSafeRandom.Next(1, tier) * ThreadSafeRandom.Next(1, tier) * ThreadSafeRandom.Next(1, work) * ThreadSafeRandom.Next(1, 250) + ThreadSafeRandom.Next(1, 50);
            int value = 0;
            switch (tier)
            {
                case 1:
                    value = (int)(ThreadSafeRandom.Next(50, 1000) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 2:
                    value = (int)(ThreadSafeRandom.Next(200, 1500) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 3:
                    value = (int)(ThreadSafeRandom.Next(200, 2000) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 4:
                    value = (int)(ThreadSafeRandom.Next(400, 2500) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 5:
                    value = (int)(ThreadSafeRandom.Next(400, 3000) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 6:
                    value = (int)(ThreadSafeRandom.Next(400, 3500) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 7:
                    value = (int)(ThreadSafeRandom.Next(600, 4000) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;
                case 8:
                    value = (int)(ThreadSafeRandom.Next(600, 4500) * gemMod * matMod * Math.Ceiling((double)tier / 2));
                    break;

            }
            return value;
        }

        private static WorldObject AssignValue(WorldObject wo)
        {
            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);

            var baseValue = ThreadSafeRandom.Next(300, 600);

            var value = (int)(baseValue * gemMaterialMod * materialMod * Math.Ceiling((double)(wo.GetProperty(PropertyInt.ItemWorkmanship) ?? 1)));
            wo.SetProperty(PropertyInt.Value, value);

            return wo;
        }

        private static int GetWorkmanship(int tier)
        {
            int workmanship = 0;
            int chance = ThreadSafeRandom.Next(0, 100);

            switch (tier)
            {
                case 1:
                    if (chance < 50)
                        workmanship = 1;
                    else if (chance < 80)
                        workmanship = 2;
                    else
                        workmanship = 3;
                    break;
                case 2:
                    if (chance < 30)
                        workmanship = 2;
                    else if (chance < 50)
                        workmanship = 3;
                    else if (chance < 65)
                        workmanship = 4;
                    else if (chance < 85)
                        workmanship = 5;
                    else
                        workmanship = 6;
                    break;
                case 3:
                    if (chance < 30)
                        workmanship = 3;
                    else if (chance < 50)
                        workmanship = 4;
                    else if (chance < 65)
                        workmanship = 5;
                    else if (chance < 85)
                        workmanship = 6;
                    else
                        workmanship = 7;
                    break;
                case 4:
                    if (chance < 30)
                        workmanship = 3;
                    else if (chance < 50)
                        workmanship = 4;
                    else if (chance < 65)
                        workmanship = 5;
                    else if (chance < 80)
                        workmanship = 6;
                    else if (chance < 92)
                        workmanship = 7;
                    else
                        workmanship = 8;
                    break;
                case 5:
                    if (chance < 15)
                        workmanship = 3;
                    else if (chance < 30)
                        workmanship = 4;
                    else if (chance < 50)
                        workmanship = 5;
                    else if (chance < 65)
                        workmanship = 6;
                    else if (chance < 80)
                        workmanship = 7;
                    else if (chance < 92)
                        workmanship = 8;
                    else
                        workmanship = 9;
                    break;
                default: // tier 6 through 8
                    if (chance < 15)
                        workmanship = 4;
                    else if (chance < 30)
                        workmanship = 5;
                    else if (chance < 50)
                        workmanship = 6;
                    else if (chance < 65)
                        workmanship = 7;
                    else if (chance < 80)
                        workmanship = 8;
                    else if (chance < 92)
                        workmanship = 9;
                    else
                        workmanship = 10;
                    break;
            }

            return workmanship;
        }

        private static int GetSpellcraft(int spellAmount, int tier)
        {

            int spellcraft = 0;
            switch (tier)
            {
                case 1:
                    spellcraft = ThreadSafeRandom.Next(1, 20) + spellAmount * ThreadSafeRandom.Next(1, 4); //1-50
                    break;
                case 2:
                    spellcraft = ThreadSafeRandom.Next(40, 70) + spellAmount * ThreadSafeRandom.Next(1, 5); //40-90
                    break;
                case 3:
                    spellcraft = ThreadSafeRandom.Next(70, 90) + spellAmount * ThreadSafeRandom.Next(1, 6); //80 - 130
                    break;
                case 4:
                    spellcraft = ThreadSafeRandom.Next(100, 120) + spellAmount * ThreadSafeRandom.Next(1, 7); /// 120 - 160
                    break;
                case 5:
                    spellcraft = ThreadSafeRandom.Next(130, 150) + spellAmount * ThreadSafeRandom.Next(1, 8); ///150 - 210
                    break;
                case 6:
                    spellcraft = ThreadSafeRandom.Next(160, 180) + spellAmount * ThreadSafeRandom.Next(1, 9); /// 200-260
                    break;
                case 7:
                    spellcraft = ThreadSafeRandom.Next(230, 260) + spellAmount * ThreadSafeRandom.Next(1, 10); /// 250 - 310
                    break;
                case 8:
                    spellcraft = ThreadSafeRandom.Next(280, 300) + spellAmount * ThreadSafeRandom.Next(1, 11); //300-450
                    break;
                default:
                    break;
            }

            return spellcraft;
        }

        private static int GetDifficulty(int tier, int spellcraft)
        {
            int difficulty = 0;
            switch (tier)
            {
                case 1:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 2:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 3:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 4:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 5:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 6:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 7:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                case 8:
                    difficulty = spellcraft + (ThreadSafeRandom.Next(0, 10) * ThreadSafeRandom.Next(1, 3));
                    break;
                default:
                    break;
            }

            return difficulty;
        }

        private static int GetMaxMana(int spellAmount, int tier)
        {
            int maxmana = 0;
            switch (tier)
            {
                case 1:
                    maxmana = ThreadSafeRandom.Next(200, 400) * spellAmount;
                    break;
                case 2:
                    maxmana = ThreadSafeRandom.Next(400, 600) * spellAmount;
                    break;
                case 3:
                    maxmana = ThreadSafeRandom.Next(600, 800) * spellAmount;
                    break;
                case 4:
                    maxmana = ThreadSafeRandom.Next(800, 1000) * spellAmount;
                    break;
                case 5:
                    maxmana = ThreadSafeRandom.Next(1000, 1200) * spellAmount;
                    break;
                case 6:
                    maxmana = ThreadSafeRandom.Next(1200, 1400) * spellAmount;
                    break;
                case 7:
                    maxmana = ThreadSafeRandom.Next(1400, 1600) * spellAmount;
                    break;
                case 8:
                    maxmana = ThreadSafeRandom.Next(1600, 1800) * spellAmount;
                    break;
                default:
                    break;
            }

            return maxmana;
        }

        /// <summary>
        /// Returns an appropriate material type fo the World Object based on its loot tier.
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="tier"></param>
        /// <returns></returns>
        private static int GetMaterialType(WorldObject wo, int tier)
        {
            int defaultMaterialType = (int)SetDefaultMaterialType(wo);

            if(wo.TsysMutationData == null)
            {
                log.Info($"Missing PropertyInt.TsysMutationData on loot item {wo.WeenieClassId} - {wo.Name}");
                return defaultMaterialType;
            }

            int materialCode = ((int)wo.TsysMutationData >> 0) & 0xFF;

            // Enforce some bounds
            if (tier < 1) tier = 1;
            // Data only goes to Tier 6 at the moment... Just in case the loot gem goes above this first, we'll cap it here for now.
            if (tier > 6)
                tier = 6;

            var materialBase = DatabaseManager.World.GetCachedTreasureMaterialBase(materialCode, tier);

            float totalProbability = GetTotalProbability(materialBase);
            // If there's zero chance, no point in continuing...
            if (totalProbability == 0) return defaultMaterialType;

            var rng = ThreadSafeRandom.Next(0.0f, totalProbability);
            float probability = 0.0f;
            foreach (var m in materialBase)
            {
                probability += m.Probability;
                if (rng >= probability || probability == totalProbability)
                {
                    // Ivory is unique... It doesn't have a group
                    if (m.MaterialId == (uint)MaterialType.Ivory)
                        return (int)m.MaterialId;

                    var materialGroup = DatabaseManager.World.GetCachedTreasureMaterialGroup((int)m.MaterialId, tier);
                    float totalGroupProbability = GetTotalProbability(materialGroup);
                    // If there's zero chance, no point in continuing...
                    if (totalGroupProbability == 0) return defaultMaterialType;

                    var groupRng = ThreadSafeRandom.Next(0.0f, totalGroupProbability);
                    float groupProbability = 0.0f;
                    foreach (var g in materialGroup)
                    {
                        groupProbability += g.Probability;
                        if (groupProbability > groupRng || groupProbability == totalGroupProbability)
                            return (int)g.MaterialId;
                    }

                    break;
                }
            }

            return (int)defaultMaterialType;
        }

        /// <summary>
        /// Sets a randomized default material type for when a weenie does not have TsysMutationData 
        /// </summary>
        /// <param name="wo"></param>
        /// <returns></returns>
        private static MaterialType SetDefaultMaterialType(WorldObject wo)
        {
            if (wo == null)
                return MaterialType.Unknown;

            MaterialType material = MaterialType.Unknown;
            int defaultMaterialEntry = ThreadSafeRandom.Next(0, 4);

            WeenieType weenieType = wo.WeenieType;
            switch (weenieType)
            {
                case WeenieType.Caster:
                    material = (MaterialType)LootTables.DefaultMaterial[3][defaultMaterialEntry];
                    break;
                case WeenieType.Clothing:
                    if (wo.ItemType == ItemType.Armor)
                        material = (MaterialType)LootTables.DefaultMaterial[0][defaultMaterialEntry];
                    if (wo.ItemType == ItemType.Clothing)
                        material = (MaterialType)LootTables.DefaultMaterial[5][defaultMaterialEntry];
                    break;
                case WeenieType.MissileLauncher:
                case WeenieType.Missile:
                    material = (MaterialType)LootTables.DefaultMaterial[1][defaultMaterialEntry];
                    break;
                case WeenieType.MeleeWeapon:
                    material = (MaterialType)LootTables.DefaultMaterial[2][defaultMaterialEntry];
                    break;
                case WeenieType.Generic:
                    if (wo.ItemType == ItemType.Jewelry)
                        material = (MaterialType)LootTables.DefaultMaterial[3][defaultMaterialEntry];
                    if (wo.ItemType == ItemType.MissileWeapon)
                        material = (MaterialType)LootTables.DefaultMaterial[4][defaultMaterialEntry];
                    break;
                default:
                    material = MaterialType.Unknown;
                    break;
            }

            return material;
        }

        private static double GetMeleeDMod(int tier)
        {
            double meleeMod = 0;
            int chance = 0;

            switch (tier)
            {
                case 1:
                    meleeMod = 0;
                    break;
                case 2:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = 0;
                    else if (chance < 80)
                        meleeMod = .01;
                    else if (chance < 92)
                        meleeMod = .02;
                    else
                        meleeMod = .03;
                    break;
                case 3:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = .03;
                    else if (chance < 80)
                        meleeMod = .04;
                    else if (chance < 92)
                        meleeMod = .05;
                    else
                        meleeMod = .06;
                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = .06;
                    else if (chance < 80)
                        meleeMod = .07;
                    else if (chance < 92)
                        meleeMod = .08;
                    else
                        meleeMod = .09;
                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = .09;
                    else if (chance < 80)
                        meleeMod = .1;
                    else if (chance < 92)
                        meleeMod = .11;
                    else
                        meleeMod = .12;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = .12;
                    else if (chance < 80)
                        meleeMod = .13;
                    else if (chance < 92)
                        meleeMod = .14;
                    else
                        meleeMod = .15;
                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = .15;
                    else if (chance < 80)
                        meleeMod = .16;
                    else if (chance < 92)
                        meleeMod = .17;
                    else
                        meleeMod = .18;
                    break;
                case 8:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 60)
                        meleeMod = .17;
                    else if (chance < 80)
                        meleeMod = .18;
                    else if (chance < 92)
                        meleeMod = .19;
                    else
                        meleeMod = .20;
                    break;
            }

            meleeMod += 1.0;
            return meleeMod;
        }

        private static int GetNumLegendaryCantrips(int tier)
        {
            int numLegendaries = 0;

            if (tier < 8)
                return 0;

            var dropRate = PropertyManager.GetDouble("legendary_cantrip_drop_rate").Item;
            if (dropRate <= 0)
                return 0;

            var dropRateMod = 1.0 / dropRate;

            // 1% chance for a legendary, 0.02% chance for 2 legendaries
            if (ThreadSafeRandom.Next(1, (int)(100 * dropRateMod)) == 1)
                numLegendaries = 1;
            if (ThreadSafeRandom.Next(1, (int)(500 * dropRateMod)) == 1)
                numLegendaries = 2;

            return numLegendaries;
        }

        private static int GetNumEpicCantrips(int tier)
        {
            int numEpics = 0;

            if (tier < 7)
                return 0;

            var dropRate = PropertyManager.GetDouble("epic_cantrip_drop_rate").Item;
            if (dropRate <= 0)
                return 0;

            var dropRateMod = 1.0 / dropRate;

            // 25% base chance for no epics for tier 7
            if (ThreadSafeRandom.Next(1, 4) > 1)
            {
                // 1% chance for 1 Epic, 0.1% chance for 2 Epics,
                // 0.01% chance for 3 Epics, 0.001% chance for 4 Epics 
                if (ThreadSafeRandom.Next(1, (int)(100 * dropRateMod)) == 1)
                    numEpics = 1;
                if (ThreadSafeRandom.Next(1, (int)(1000 * dropRateMod)) == 1)
                    numEpics = 2;
                if (ThreadSafeRandom.Next(1, (int)(10000 * dropRateMod)) == 1)
                    numEpics = 3;
                if (ThreadSafeRandom.Next(1, (int)(100000 * dropRateMod)) == 1)
                    numEpics = 4;
            }

            return numEpics;
        }

        private static int GetNumMajorCantrips(int tier)
        {
            int numMajors = 0;

            var dropRate = PropertyManager.GetDouble("major_cantrip_drop_rate").Item;
            if (dropRate <= 0)
                return 0;

            var dropRateMod = 1.0 / dropRate;

            switch (tier)
            {
                case 1:
                    numMajors = 0;
                    break;
                case 2:
                    if (ThreadSafeRandom.Next(1, (int)(500 * dropRateMod)) == 1)
                        numMajors = 1;
                    break;
                case 3:
                    if (ThreadSafeRandom.Next(1, (int)(500 * dropRateMod)) == 1)
                        numMajors = 1;
                    if (ThreadSafeRandom.Next(1, (int)(10000 * dropRateMod)) == 1)
                        numMajors = 2;
                    break;
                case 4:
                case 5:
                case 6:
                    if (ThreadSafeRandom.Next(1, (int)(500 * dropRateMod)) == 1)
                        numMajors = 1;
                    if (ThreadSafeRandom.Next(1, (int)(5000 * dropRateMod)) == 1)
                        numMajors = 2;
                    break;
                case 7:
                default:
                    if (ThreadSafeRandom.Next(1, (int)(500 * dropRateMod)) == 1)
                        numMajors = 1;
                    if (ThreadSafeRandom.Next(1, (int)(5000 * dropRateMod)) == 1)
                        numMajors = 2;
                    if (ThreadSafeRandom.Next(1, (int)(15000 * dropRateMod)) == 1)
                        numMajors = 3;
                    break;
            }

            return numMajors;
        }

        private static int GetNumMinorCantrips(int tier)
        {
            int numMinors = 0;

            var dropRate = PropertyManager.GetDouble("minor_cantrip_drop_rate").Item;
            if (dropRate <= 0)
                return 0;

            var dropRateMod = 1.0 / dropRate;

            switch (tier)
            {
                case 1:
                    if (ThreadSafeRandom.Next(1, (int)(100 * dropRateMod)) == 1)
                        numMinors = 1;
                    break;
                case 2:
                case 3:
                    if (ThreadSafeRandom.Next(1, (int)(50 * dropRateMod)) == 1)
                        numMinors = 1;
                    if (ThreadSafeRandom.Next(1, (int)(250 * dropRateMod)) == 1)
                        numMinors = 2;
                    break;
                case 4:
                case 5:
                    if (ThreadSafeRandom.Next(1, (int)(50 * dropRateMod)) == 1)
                        numMinors = 1;
                    if (ThreadSafeRandom.Next(1, (int)(250 * dropRateMod)) == 1)
                        numMinors = 2;
                    if (ThreadSafeRandom.Next(1, (int)(1000 * dropRateMod)) == 1)
                        numMinors = 3;
                    break;
                case 6:
                case 7:
                default:
                    if (ThreadSafeRandom.Next(1, (int)(50 * dropRateMod)) == 1)
                        numMinors = 1;
                    if (ThreadSafeRandom.Next(1, (int)(250 * dropRateMod)) == 1)
                        numMinors = 2;
                    if (ThreadSafeRandom.Next(1, (int)(1000 * dropRateMod)) == 1)
                        numMinors = 3;
                    if (ThreadSafeRandom.Next(1, (int)(5000 * dropRateMod)) == 1)
                        numMinors = 4;
                    break;
            }

            return numMinors;
        }

        /// <summary>
        /// Set the AppraisalLongDescDecoration of the item, which controls the full descriptive text shown in the client on appraisal
        /// </summary>
        /// <param name="wo"></param>
        /// <returns></returns>
        private static WorldObject SetAppraisalLongDescDecoration(WorldObject wo)
        {
            // LDDecoration_PrependWorkmanship = 0x1,
            // LDDecoration_PrependMaterial = 0x2,
            // LDDecoration_AppendGemInfo = 0x4,
            int appraisalLongDescDecoration = 0;
            if (wo.ItemWorkmanship > 0)
                appraisalLongDescDecoration |= 1;
            if (wo.MaterialType > 0)
                appraisalLongDescDecoration |= 2;
            if (wo.GemType > 0 && wo.GemCount > 0)
                appraisalLongDescDecoration |= 4;

            if (appraisalLongDescDecoration > 0)
                wo.AppraisalLongDescDecoration = appraisalLongDescDecoration;
            else
                wo.AppraisalLongDescDecoration = null;

            return wo;
        }

        /// <summary>
        /// This will assign a completely random, valid color to the item in question. It will also randomize the shade and set the appropriate icon.
        ///
        /// This was a temporary function to give some color to loot until further work was put in for "proper" color handling. Leave it here as an option for future potential use (perhaps config option?)
        /// </summary>
        private static WorldObject RandomizeColorTotallyRandom(WorldObject wo)
        {
            // Make sure the item has a ClothingBase...otherwise we can't properly randomize the colors.
            if (wo.ClothingBase != null)
            {
                DatLoader.FileTypes.ClothingTable item = DatLoader.DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.ClothingTable>((uint)wo.ClothingBase);

                // Get a random PaletteTemplate index from the ClothingBase entry
                // But make sure there's some valid ClothingSubPalEffects (in a valid loot/clothingbase item, there always SHOULD be)
                if (item.ClothingSubPalEffects.Count > 0)
                {
                    int randIndex = ThreadSafeRandom.Next(0, item.ClothingSubPalEffects.Count - 1);
                    var cloSubPal = item.ClothingSubPalEffects.ElementAt(randIndex);

                    // Make sure this entry has a valid icon, otherwise there's likely something wrong with the ClothingBase value for this WorldObject (e.g. not supposed to be a loot item)
                    if (cloSubPal.Value.Icon > 0)
                    {
                        // Assign the appropriate Icon and PaletteTemplate
                        wo.IconId = cloSubPal.Value.Icon;
                        wo.PaletteTemplate = (int)cloSubPal.Key;

                        // Throw some shade, at random
                        wo.Shade = ThreadSafeRandom.Next(0.0f, 1.0f);
                    }
                }
            }
            return wo;
        }

        /// <summary>
        /// Assign a random color (Int.PaletteTemplate and Float.Shade) to a World Object based on the material assigned to it.
        /// </summary>
        /// <param name="wo"></param>
        /// <returns>WorldObject with a random applicable PaletteTemplate and Shade applied, if available</returns>
        private static WorldObject RandomizeColor(WorldObject wo)
        {
            if(wo.MaterialType > 0 && wo.TsysMutationData != null && wo.ClothingBase != null)
            {
                byte colorCode = (byte)( ((uint)wo.TsysMutationData >> 16) & 0xFF );

                // BYTE spellCode = (tsysMutationData >> 24) & 0xFF;
                // BYTE colorCode = (tsysMutationData >> 16) & 0xFF;
                // BYTE gemCode = (tsysMutationData >> 8) & 0xFF;
                // BYTE materialCode = (tsysMutationData >> 0) & 0xFF;

                List<TreasureMaterialColor> colors;
                // This is a unique situation that typically applies to Under Clothes.
                // If the Color Code is 0, they can be PaletteTemplate 1-18, assuming there is a MaterialType
                // (gems have ColorCode of 0, but also no MaterialCode as they are defined by the weenie)
                if (colorCode == 0 && (uint)wo.MaterialType > 0)
                {
                    colors = new List<TreasureMaterialColor>();
                    for (uint i = 1; i < 19; i++)
                    {
                        TreasureMaterialColor tmc = new TreasureMaterialColor
                        {
                            PaletteTemplate = i,
                            Probability = 1
                        };
                        colors.Add(tmc);
                    }
                }
                else
                {
                    colors = DatabaseManager.World.GetCachedTreasureMaterialColors((int)wo.MaterialType, colorCode);
                }

                // Load the clothingBase associated with the WorldObject
                DatLoader.FileTypes.ClothingTable clothingBase = DatLoader.DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.ClothingTable>((uint)wo.ClothingBase);

                // TODO : Probably better to use an intersect() function here. I defer to someone who knows how these work better than I - Optim
                // Compare the colors list and the clothingBase PaletteTemplates and remove any invalid items
                var colorsValid = new List<TreasureMaterialColor>();
                foreach (var e in colors)
                    if (clothingBase.ClothingSubPalEffects.ContainsKey((uint)e.PaletteTemplate))
                        colorsValid.Add(e);
                colors = colorsValid;

                float totalProbability = GetTotalProbability(colors);
                // If there's zero chance to get a random color, no point in continuing.
                if (totalProbability == 0) return wo;

                var rng = ThreadSafeRandom.Next(0.0f, totalProbability);

                uint paletteTemplate = 0;
                float probability = 0.0f;
                // Loop through the colors until we've reach our target value
                foreach (var color in colors)
                {
                    probability += color.Probability;
                    if (probability >= rng || probability == totalProbability)
                    {
                        paletteTemplate = color.PaletteTemplate;
                        break;
                    }
                }
                if (paletteTemplate > 0)
                {
                    var cloSubPal = clothingBase.ClothingSubPalEffects[(uint)paletteTemplate];
                    // Make sure this entry has a valid icon, otherwise there's likely something wrong with the ClothingBase value for this WorldObject (e.g. not supposed to be a loot item)
                    if (cloSubPal.Icon > 0)
                    {
                        // Assign the appropriate Icon and PaletteTemplate
                        wo.IconId = cloSubPal.Icon;
                        wo.PaletteTemplate = (int)paletteTemplate;

                        // Throw some shade, at random
                        wo.Shade = ThreadSafeRandom.Next(0.0f, 1.0f);

                        // Some debu ginfo...
                        // log.Info($"Color success for {wo.MaterialType}({(int)wo.MaterialType}) - {wo.WeenieClassId} - {wo.Name}. PaletteTemplate {paletteTemplate} applied.");
                    }
                }
                else
                {
                    log.Info($"Color looked failed for {wo.MaterialType} ({(int)wo.MaterialType}) - {wo.WeenieClassId} - {wo.Name}.");
                }
            }

            return wo;
        }

        /// <summary>
        /// Some helper functions to get Probablity from different list types
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        private static float GetTotalProbability(List<TreasureMaterialColor> colors)
        {
            if (colors == null || colors.Count == 0) return 0.0f;

            var prob = colors.Select(i => i.Probability).ToList();

            var totalSum = prob.Sum();
            return totalSum;
        }
        private static float GetTotalProbability(List<TreasureMaterialBase> list)
        {
            if (list == null || list.Count == 0) return 0.0f;

            var prob = list.Select(i => i.Probability).ToList();

            var totalSum = prob.Sum();
            return totalSum;
        }
        private static float GetTotalProbability(List<TreasureMaterialGroups> list)
        {
            if (list == null || list.Count == 0) return 0.0f;

            var prob = list.Select(i => i.Probability).ToList();

            var totalSum = prob.Sum();
            return totalSum;
        }

    }
}
