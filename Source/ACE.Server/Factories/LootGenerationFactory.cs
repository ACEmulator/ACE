using System.Collections.Generic;
using System;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class LootGenerationFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                for (var i = 0; i < numItems; i++)
                {
                    lootWorldObject = CreateRandomLootObjects(profile.Tier, true, lootBias);
                    if (lootWorldObject != null)
                        loot.Add(lootWorldObject);
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

            // 50/50 chance to drop a scroll
            itemChance = ThreadSafeRandom.Next(0, 2);
            if (itemChance >= 1 || profile.Tier > 3)
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
                lootWorldObject = CreateSummoningEssence(profile.Tier);

                if (lootWorldObject != null)
                    loot.Add(lootWorldObject);
            }

            return loot;
        }

        private static WorldObject CreateSummoningEssence(int tier)
        {
            uint id = 0;

            if (tier < 1) tier = 1;
            if (tier > 8) tier = 8;

            int summoningEssenceIndex = ThreadSafeRandom.Next(0, LootHelper.SummoningEssencesMatrix.Length - 1);

            id = (uint)LootHelper.SummoningEssencesMatrix[summoningEssenceIndex][tier - 1];

            if (id == 0)
                return null;

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(id);
            return wo;
        }

        private static WorldObject CreateRandomLootObjects(int tier, bool isMagical, LootBias lootBias = LootBias.UnBiased)
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
                default:
                    //jewelry
                    wo = CreateJewelry(tier, isMagical);
                    return wo;
            }
        }

        private static WorldObject CreateMundaneObjects(int tier)
        {
            uint id = 0;
            int chance;
            WorldObject wo;

            if (tier < 1) tier = 1;
            if (tier > 8) tier = 8;

            chance = ThreadSafeRandom.Next(0, 1);

            switch (chance)
            {
                case 0:
                    id = (uint)CreateFood();
                    break;
                default:
                    int mundaneLootMatrixIndex = tier - 1;
                    int upperLimit = LootHelper.MundaneLootMatrix[mundaneLootMatrixIndex].Length - 1;

                    chance = ThreadSafeRandom.Next(0, upperLimit);
                    id = (uint)LootHelper.MundaneLootMatrix[mundaneLootMatrixIndex][chance];
                    break;
            }

            if (id == 0)
                return null;

            wo = WorldObjectFactory.CreateNewWorldObject(id);
            return wo;
        }

        private static WorldObject CreateRandomScroll(int tier)
        {
            WorldObject wo;

            if (tier > 7)
            {
                int id = CreateLevel8SpellComp();
                wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                return wo;
            }

            if (tier < 1) tier = 1;

            int scrollLootMatrixIndex = tier - 1;
            int minSpellLevel = LootHelper.ScrollLootMatrix[scrollLootMatrixIndex][0];
            int maxSpellLevel = LootHelper.ScrollLootMatrix[scrollLootMatrixIndex][1];

            int scrollLootIndex = ThreadSafeRandom.Next(minSpellLevel, maxSpellLevel);
            uint spellID = 0;

            while (spellID == 0)
                spellID = (uint)LootHelper.ScrollSpells[ThreadSafeRandom.Next(0, LootHelper.ScrollSpells.Length - 1)][scrollLootIndex];

            var weenie = DatabaseManager.World.GetScrollWeenie(spellID);
            if (weenie == null)
            {
                log.WarnFormat("CreateRandomScroll for tier {0} and spellID of {1} returned null from the database.", tier, spellID);
                return null;
            }

            wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);
            return wo;
        }

        private static int CreateLevel8SpellComp()
        {
            int upperLimit = LootHelper.Level8SpellComps.Length - 1;
            int chance = ThreadSafeRandom.Next(0, upperLimit);

            return LootHelper.Level8SpellComps[chance];
        }

        private static WorldObject CreateJewels(int tier, bool isMagical)
        {
            uint gemType = 0;
            int workmanship = 0;
            int rank = 0;
            int difficulty = 0;
            int mana_cost = 0;
            int spellDID = 0;
            int max_mana = 0;
            int skill_level_limit = 0;
            int spellcraft = 0;

            int gemLootMatrixIndex = tier - 1;

            if (gemLootMatrixIndex > 4) gemLootMatrixIndex = 4;
            int upperLimit = LootHelper.GemsMatrix[gemLootMatrixIndex].Length - 1;

            gemType = (uint)LootHelper.GemsMatrix[gemLootMatrixIndex][ThreadSafeRandom.Next(0, upperLimit)];

            gemLootMatrixIndex = tier - 1;

            if (isMagical)
            {
                int gemSpellIndex;
                int spellChance = 0;

                spellChance = ThreadSafeRandom.Next(0, 3);
                switch (spellChance)
                {
                    case 0:
                        gemSpellIndex = LootHelper.GemSpellIndexMatrix[gemLootMatrixIndex][0];
                        spellDID = LootHelper.GemCreatureSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootHelper.GemCreatureSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    case 1:
                        gemSpellIndex = LootHelper.GemSpellIndexMatrix[gemLootMatrixIndex][0];
                        spellDID = LootHelper.GemLifeSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootHelper.GemLifeSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    case 2:
                        gemSpellIndex = LootHelper.GemSpellIndexMatrix[gemLootMatrixIndex][1];
                        spellDID = LootHelper.GemCreatureSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootHelper.GemCreatureSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    default:
                        gemSpellIndex = LootHelper.GemSpellIndexMatrix[gemLootMatrixIndex][1];
                        spellDID = LootHelper.GemLifeSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootHelper.GemLifeSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                }

                mana_cost = 50 * gemSpellIndex;
                spellcraft = 50 * gemSpellIndex;
                max_mana = ThreadSafeRandom.Next(mana_cost, mana_cost + 50);
            }

            workmanship = GetWorkmanship(tier);

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(gemType) as Gem;
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            if (spellDID > 0)
            {
                wo.SetProperty(PropertyInt.ItemUseable, 8);
                wo.SetProperty(PropertyInt.UiEffects, 1);
            }
            else
            {
                wo.SetProperty(PropertyInt.ItemUseable, 1);
                wo.SetProperty(PropertyInt.UiEffects, 0);
            }

            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship));
            wo.SetProperty(PropertyDataId.Spell, (uint)spellDID);
            wo.SetProperty(PropertyInt.ItemAllegianceRankLimit, rank);
            wo.SetProperty(PropertyInt.ItemDifficulty, difficulty);
            wo.SetProperty(PropertyInt.ItemManaCost, mana_cost);
            wo.SetProperty(PropertyInt.ItemMaxMana, max_mana);
            wo.SetProperty(PropertyInt.ItemSkillLevelLimit, skill_level_limit);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.RemoveProperty(PropertyInt.ItemDifficulty);
            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);

            if (spellDID == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
                wo.RemoveProperty(PropertyDataId.Spell);
            }

            return wo;
        }

        private static int CreateFood()
        {
            int foodType = 0;
            foodType = LootHelper.food[ThreadSafeRandom.Next(0, LootHelper.food.Length - 1)];
            return foodType;
        }

        private static WorldObject CreateJewelry(int tier, bool isMagical)
        {

            int[][] JewelrySpells = LootHelper.JewelrySpells;
            int[][] JewelryCantrips = LootHelper.JewelryCantrips;
            int[] jewelryItems = { 621, 295, 297, 294, 623, 622 };
            int jewelType = jewelryItems[ThreadSafeRandom.Next(0, jewelryItems.Length - 1)];
            int numSpells = 0;
            int numCantrips = 0;
            int lowSpellTier = 0;
            int highSpellTier = 0;
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            int workmanship = 0;
            int value = 100;
            //int rank = 0;
            int difficulty = 0;
            int max_mana = 0;
            //int skill_level_limit = 0;
            int spellcraft = 0;
            lowSpellTier = GetLowSpellTier(tier);
            highSpellTier = GetHighSpellTier(tier);
            if (isMagical)
            {
                numSpells = GetNumSpells(tier);
            }
            numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            if (numCantrips > 10)
            {
                minorCantrips = 0;
            }
            numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)jewelType);
            workmanship = GetWorkmanship(tier);
            value = GetValue(tier, workmanship);
            spellcraft = GetSpellcraft(numSpells, tier);
            max_mana = GetMaxMana(numSpells, tier);
            difficulty = GetDifficulty(tier, spellcraft);
            int mT = GetMaterialType(1, tier);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);
            wo.SetProperty(PropertyInt.MaterialType, mT);
            wo.SetProperty(PropertyInt.Value, value);
            int gemCount = ThreadSafeRandom.Next(1, 5);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.ItemDifficulty, difficulty);
            wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());
            wo.SetProperty(PropertyInt.ItemMaxMana, max_mana);
            wo.SetProperty(PropertyInt.ItemCurMana, max_mana);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));
            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);
            int[] shuffledValues = new int[JewelrySpells.Length];
            for (int i = 0; i < JewelrySpells.Length; i++)
            {
                shuffledValues[i] = i;
            }
            Shuffle(shuffledValues);
            if (numSpells - numCantrips > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = ThreadSafeRandom.Next(lowSpellTier - 1, highSpellTier - 1);
                    int spellID = JewelrySpells[shuffledValues[a]][col];
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (numCantrips > 0)
            {
                shuffledValues = new int[JewelryCantrips.Length];
                for (int i = 0; i < JewelryCantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            return wo;
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

        private static WorldObject CreateWeapon(int tier, bool isMagical)
        {
            int weaponWeenie = 0;
            int numSpells = 0;
            int damage = 0;
            double damageVariance = 0;
            double weaponDefense = 0;
            double weaponOffense = 0;
            int longDescDecoration = 5; 

            ///Properties for weapons
            if (isMagical)
                numSpells = GetNumSpells(tier);

            double magicD = GetMissileDMod(tier);
            double missileD = GetMissileDMod(tier);
            int gemCount = ThreadSafeRandom.Next(1, 5);
            int gemType = ThreadSafeRandom.Next(10, 50);
            int materialType = GetMaterialType(2, tier);
            int workmanship = GetWorkmanship(tier);
            int value = GetValue(tier, workmanship);
            int spellCraft = GetSpellcraft(numSpells, tier);
            int itemDifficulty = GetDifficulty(tier, spellCraft);
            int wieldDiff = GetWield(tier, 3);
            WieldRequirement wieldRequirments = WieldRequirement.RawSkill;
            Skill wieldSkillType = Skill.None;
            int maxMana = GetMaxMana(numSpells, tier);

            int eleType = ThreadSafeRandom.Next(0, 4);
            int weaponType = ThreadSafeRandom.Next(0, 5);
            switch (weaponType)
            {
                case 0:
                    wieldSkillType = Skill.HeavyWeapons;
                    int heavyWeaponsType = ThreadSafeRandom.Next(0, 22);
                    weaponWeenie = LootHelper.HeavyWeaponsMatrix[heavyWeaponsType][eleType];

                    switch (heavyWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMaxDamage(1, tier, wieldDiff, 1);
                            damageVariance = GetVariance(1, 1);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(1, tier, wieldDiff, 2);

                            if (heavyWeaponsType == 3)
                                damageVariance = GetVariance(1, 2);
                            if (heavyWeaponsType == 4 || heavyWeaponsType == 5)
                            {
                                damage = GetMaxDamage(1, tier, wieldDiff, 3);
                                damageVariance = GetVariance(1, 3);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(1, tier, wieldDiff, 4);
                            damageVariance = GetVariance(1, 4);
                            break;
                        case 10:
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(1, tier, wieldDiff, 5);
                            damageVariance = GetVariance(1, 5);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(1, tier, wieldDiff, 8);
                            damageVariance = GetVariance(1, 6);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(1, tier, wieldDiff, 6);
                            damageVariance = GetVariance(1, 7);

                            if (heavyWeaponsType == 20)
                            {
                                damage = GetMaxDamage(1, tier, wieldDiff, 7);
                                damageVariance = GetVariance(1, 8);
                            }
                            break;
                        case 21:
                        default:
                            damage = GetMaxDamage(1, tier, wieldDiff, 9);
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damageVariance = GetVariance(1, 9);
                            break;
                    }
                    break;
                case 1:
                    wieldSkillType = Skill.LightWeapons;
                    int lightWeaponsType = ThreadSafeRandom.Next(0, 19);
                    weaponWeenie = LootHelper.HeavyWeaponsMatrix[lightWeaponsType][eleType];

                    switch (lightWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMaxDamage(2, tier, wieldDiff, 1);
                            damageVariance = GetVariance(2, 1);
                            break;
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            if (lightWeaponsType == 4)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 2);
                                damageVariance = GetVariance(2, 2);
                            }

                            if (lightWeaponsType == 5)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 3);
                                damageVariance = GetVariance(2, 3);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(2, tier, wieldDiff, 4);
                            damageVariance = GetVariance(2, 4);
                            break;
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(2, tier, wieldDiff, 5);
                            damageVariance = GetVariance(2, 6);
                            break;
                        case 11:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(2, tier, wieldDiff, 8);
                            damageVariance = GetVariance(2, 7);
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(2, tier, wieldDiff, 6);
                            damageVariance = GetVariance(2, 8);

                            if (lightWeaponsType == 14)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 7);
                                damageVariance = GetVariance(2, 9);
                            }
                            break;
                        case 18:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 9);
                            damageVariance = GetVariance(2, 10);
                            break;
                    }
                    break;
                case 2:
                    wieldSkillType = Skill.FinesseWeapons;
                    int finesseWeaponsType = ThreadSafeRandom.Next(0, 22);
                    weaponWeenie = LootHelper.HeavyWeaponsMatrix[finesseWeaponsType][eleType];

                    switch (finesseWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMaxDamage(2, tier, wieldDiff, 1);
                            damageVariance = GetVariance(2, 1);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 2);
                            damageVariance = GetVariance(2, 2);

                            if (finesseWeaponsType == 3 || finesseWeaponsType == 4)
                            {
                                damageVariance = GetVariance(2, 3);
                                damage = GetMaxDamage(2, tier, wieldDiff, 3);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(2, tier, wieldDiff, 4);
                            damageVariance = GetVariance(2, 4);

                            if (finesseWeaponsType == 9)
                                damageVariance = GetVariance(2, 5);
                            break;
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(2, tier, wieldDiff, 5);
                            damageVariance = GetVariance(2, 6);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(2, tier, wieldDiff, 8);
                            damageVariance = GetVariance(2, 7);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 6);
                            damageVariance = GetVariance(2, 8);

                            if (finesseWeaponsType == 15)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 7);
                                damageVariance = GetVariance(2, 9);
                            }
                            break;
                        case 21:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 9);
                            damageVariance = GetVariance(2, 10);
                            break;
                    }
                    break;
                case 3:
                    ///Two handed
                    wieldSkillType = Skill.TwoHandedCombat;
                    int twoHandedWeaponsType = ThreadSafeRandom.Next(0, 11);
                    weaponWeenie = LootHelper.HeavyWeaponsMatrix[twoHandedWeaponsType][eleType];

                    damage = GetMaxDamage(3, tier, wieldDiff, 1);
                    damageVariance = GetVariance(3, 1);

                    switch (twoHandedWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damageVariance = GetVariance(2, 1);
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            break;
                        case 7:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            break;
                        case 8:
                        case 9:
                        case 10:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(3, tier, wieldDiff, 2);
                            break;
                    }
                    break;
                case 4:
                    return CreateMissileWeapon(tier);
                default:
                    return CreateCaster(tier);
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(2, tier));
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            wo.SetProperty(PropertyInt.WeaponSkill, (int)wieldSkillType);
            wo.SetProperty(PropertyInt.Damage, damage);
            wo.SetProperty(PropertyFloat.DamageVariance, damageVariance);

            wo.SetProperty(PropertyFloat.WeaponDefense, weaponDefense);
            wo.SetProperty(PropertyFloat.WeaponOffense, weaponOffense);
            wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileD);
            wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicD);

            wo.SetProperty(PropertyInt.WieldDifficulty, wieldDiff);
            wo.SetProperty(PropertyInt.WieldRequirements, (int)wieldRequirments);

            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, longDescDecoration);
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            int lowSpellTier = GetLowSpellTier(tier);
            int highSpellTier = GetHighSpellTier(tier);
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            int[][] spells = LootHelper.MeleeSpells;
            int[][] cantrips = LootHelper.MeleeCantrips;

            if (numSpells > 0)
            {
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellCraft);

                // Override weenie property, as item contains spells
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

                wo.SetProperty(PropertyInt.ItemDifficulty, itemDifficulty);
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, maxMana);
                wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());

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
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
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
                    wo.SetProperty(PropertyInt.UiEffects, 1);
                    //minor cantripps
                    for (int a = 0; a < minorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    //major cantrips
                    for (int a = 0; a < majorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    // epic cantrips
                    for (int a = 0; a < epicCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    //legendary cantrips
                    for (int a = 0; a < legendaryCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                }
            }

            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }
            if (wieldDiff == 0)
            {
                wo.RemoveProperty(PropertyInt.WieldDifficulty);
                wo.RemoveProperty(PropertyInt.WieldRequirements);
                wo.RemoveProperty(PropertyInt.WieldSkillType);
            }

            return wo;
        }

        private static WorldObject CreateArmor(int tier, bool isMagical, LootBias lootBias = LootBias.UnBiased)
        {

            int lowSpellTier = 0;
            int highSpellTier = 0;
            ////Double values needed
            double armorModAcid = 0;
            double armorModBludge = 0;
            double armorModCold = 0;
            double armorModElectric = 0;
            double armorModFire = 0;
            double armorModNether = 0;
            double armorModPierce = 0;
            double armorModSlash = 0;
            int spellArray = 0;
            int cantripArray = 0;
            int equipSetId = 0;
            int wieldDifficulty = 0;
            int maxMana = 0;
            int curMana = 0;
            int spellcraft = 0;
            int materialType = 0;
            int armorType = 0;
            int armorPieceType = 0;
            int[][] spells;
            int[][] cantrips;
            int armorWeenie = 0;
            //int palette = 0;
            switch (tier)
            {
                case 1:
                    lowSpellTier = 1;
                    highSpellTier = 3;
                    armorType = ThreadSafeRandom.Next(0, 3);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 11:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Random Armor Items
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////bandana
                                armorWeenie = 28612;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 1:
                                ////beret
                                armorWeenie = 28605;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 2:
                                ////Cloth Cap
                                armorWeenie = 118;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 3:
                                ////Cloth gloves
                                armorWeenie = 121;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 4:
                                ////Cowl
                                armorWeenie = 119;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 5:
                                ////crown
                                armorWeenie = 296;
                                armorPieceType = 1;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 6:
                                ////Fez
                                armorWeenie = 5894;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 7:
                                ////hood
                                armorWeenie = 5905;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 8:
                                ////Kasa
                                armorWeenie = 5901;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 9:
                                ////Metal cap
                                armorWeenie = 46;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 10:
                                ////Qafiya
                                armorWeenie = 76;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 11:
                                ////turban
                                armorWeenie = 135;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 12:
                                ////loafers
                                armorWeenie = 28610;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 13:
                                ////sandals
                                armorWeenie = 129;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 14:
                                ////shoes
                                armorWeenie = 132;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 15:
                                ////slippers
                                armorWeenie = 133;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 16:
                                ////steel toed boots
                                armorWeenie = 7897;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(6, 1);
                                break;
                            case 17:
                                ////buckler
                                armorWeenie = 44;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 18:
                                ////kite shield
                                armorWeenie = 91;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 19:
                                ////large Kite Shield
                                armorWeenie = 92;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            default:
                                ////Round Shield
                                armorWeenie = 93;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                        }
                    }
                    break;
                case 2:
                    lowSpellTier = 3;
                    highSpellTier = 5;
                    armorType = ThreadSafeRandom.Next(0, 6);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28367;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Random Armor Items
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        switch (armorPiece)
                        {
                            case 0:
                                ////bandana
                                armorWeenie = 28612;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 1:
                                ////beret
                                armorWeenie = 28605;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 2:
                                ////Cloth Cap
                                armorWeenie = 118;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 3:
                                ////Cloth gloves
                                armorWeenie = 121;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 4:
                                ////Cowl
                                armorWeenie = 119;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 5:
                                ////crown
                                armorWeenie = 296;
                                armorPieceType = 1;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 6:
                                ////Fez
                                armorWeenie = 5894;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 7:
                                ////hood
                                armorWeenie = 5905;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 8:
                                ////Kasa
                                armorWeenie = 5901;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 9:
                                ////Metal cap
                                armorWeenie = 46;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 10:
                                ////Qafiya
                                armorWeenie = 76;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 11:
                                ////turban
                                armorWeenie = 135;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 12:
                                ////loafers
                                armorWeenie = 28610;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 13:
                                ////sandals
                                armorWeenie = 129;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 14:
                                ////shoes
                                armorWeenie = 132;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 15:
                                ////slippers
                                armorWeenie = 133;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(8, 1);
                                break;
                            case 16:
                                ////steel toed boots
                                armorWeenie = 7897;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(6, 1);
                                break;
                            case 17:
                                ////buckler
                                armorWeenie = 44;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 18:
                                ////kite shield
                                armorWeenie = 91;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 19:
                                ////large Kite Shield
                                armorWeenie = 92;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 20:
                                ////Circlet
                                armorWeenie = 29528;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 21:
                                ////Armet
                                armorWeenie = 8488;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 22:
                                ////Baigha
                                armorWeenie = 550;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 23:
                                ////Heaume
                                armorWeenie = 74;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 24:
                                ////Helmet
                                armorWeenie = 75;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 25:
                                ////Kabuton
                                armorWeenie = 77;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 26:
                                ////sollerets
                                armorWeenie = 107;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 27:
                                ////viamontian laced boots
                                armorWeenie = 28611;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                materialType = GetMaterialType(7, 1);
                                break;
                            case 28:
                                ////RTower Shield
                                armorWeenie = 95;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                            default:
                                ////Round Shield
                                armorWeenie = 93;
                                armorPieceType = 5;
                                spellArray = 10;
                                cantripArray = 10;
                                materialType = GetMaterialType(7, 1);
                                break;
                        }
                    }
                    break;
                case 3:
                    lowSpellTier = 4;
                    highSpellTier = 6;
                    armorType = ThreadSafeRandom.Next(0, 10);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 11);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28627;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 4:
                    lowSpellTier = 5;
                    highSpellTier = 6;
                    armorType = ThreadSafeRandom.Next(0, 10);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28367;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 5:
                    lowSpellTier = 5;
                    highSpellTier = 7;
                    armorType = ThreadSafeRandom.Next(0, 14);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28367;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 30950;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 28629;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 30951;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 28617;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            default:
                                armorWeenie = 28620;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 34)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 35)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                ///////
                ////

                case 6:
                    lowSpellTier = 6;
                    highSpellTier = 7;
                    armorType = ThreadSafeRandom.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 30950;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 28629;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 30951;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 28617;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            default:
                                armorWeenie = 28620;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    //////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43830;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 7:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    armorType = ThreadSafeRandom.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28367;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 30950;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28629;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 30951;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28617;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else
                        {
                            armorWeenie = 28620;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    //else if (armorPiece == 1)
                    //    //{
                    //    //    armorWeenie = 43830;
                    //    //    armorPieceType = 3;
                    //    //    spellArray = 5;
                    //    //    cantripArray = 5;
                    //    //}
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    wieldDifficulty = 150;
                    break;
                default:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    if (lootBias == LootBias.Armor) // Armor Mana Forge Chests don't include clothing type items
                        armorType = ThreadSafeRandom.Next(0, 14);
                    else
                        armorType = ThreadSafeRandom.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 15);
                        switch (armorPiece)
                        {
                            case 0:
                                ////helm
                                armorWeenie = 25636;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                ////head
                                armorWeenie = 25640;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 2:
                                ////Chest
                                armorWeenie = 25639;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                ////Chest
                                armorWeenie = 25641;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                ////Chest
                                armorWeenie = 25638;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                ////arms
                                armorWeenie = 25651;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                ////Hands
                                armorWeenie = 25642;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 7:
                                ////Lower Arms
                                armorWeenie = 25637;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 8:
                                ////Upper arms
                                armorWeenie = 25648;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 2;
                                break;
                            case 9:
                                ////Abdomen
                                armorWeenie = 25643;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 10:
                                ////Abdomen
                                armorWeenie = 25650;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 11:
                                ////legs
                                armorWeenie = 25647;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 12:
                                ////legs
                                armorWeenie = 25645;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 13:
                                ////Upper legs
                                armorWeenie = 25652;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 14:
                                ////lower legs
                                armorWeenie = 25644;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            default:
                                ////feet
                                armorWeenie = 25661;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 14);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 554;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 116;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 2:
                                armorWeenie = 38;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 3:
                                armorWeenie = 42;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 4:
                                armorWeenie = 48;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 723;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 6:
                                armorWeenie = 53;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 59;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 8:
                                armorWeenie = 63;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 9:
                                armorWeenie = 68;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 10:
                                armorWeenie = 68;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 11:
                                armorWeenie = 89;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 12:
                                armorWeenie = 99;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 13:
                                armorWeenie = 105;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 112;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 35;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 413;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 414;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 85;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 55;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 5:
                                armorWeenie = 415;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 6:
                                armorWeenie = 2605;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 71;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 8:
                                armorWeenie = 80;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 416;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 96;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 11:
                                armorWeenie = 101;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 108;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 10);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 40;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 51;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 57;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 61;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 66;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 5:
                                armorWeenie = 72;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 6:
                                armorWeenie = 82;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 7:
                                armorWeenie = 87;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 8:
                                armorWeenie = 103;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 9:
                                armorWeenie = 110;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 114;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 13);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 552;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 1:
                                armorWeenie = 37;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 2:
                                armorWeenie = 41;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 793;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 52;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 5:
                                armorWeenie = 58;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 6:
                                armorWeenie = 62;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 7:
                                armorWeenie = 67;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 8:
                                armorWeenie = 73;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 9:
                                armorWeenie = 83;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 10:
                                armorWeenie = 88;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 98;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 12:
                                armorWeenie = 104;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 111;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 7);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 43;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 54;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 64;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 3:
                                armorWeenie = 69;
                                armorPieceType = 1;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 4:
                                armorWeenie = 2437;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 5:
                                armorWeenie = 90;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 6:
                                armorWeenie = 102;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            default:
                                armorWeenie = 113;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 12);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 28367;
                                armorPieceType = 1;
                                spellArray = 4;
                                cantripArray = 4;
                                break;
                            case 1:
                                armorWeenie = 28628;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 28630;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 3:
                                armorWeenie = 28632;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 4:
                                armorWeenie = 28633;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 5:
                                armorWeenie = 28634;
                                armorPieceType = 31;
                                spellArray = 8;
                                cantripArray = 8;
                                break;
                            case 6:
                                armorWeenie = 30948;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 7:
                                armorWeenie = 28618;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 8:
                                armorWeenie = 28621;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            case 9:
                                armorWeenie = 28623;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 10:
                                armorWeenie = 30949;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                            case 11:
                                armorWeenie = 28625;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            default:
                                armorWeenie = 28626;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 3);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 6044;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 6043;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 2:
                                armorWeenie = 6045;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 6048;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 1);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 5);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27220;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27221;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27222;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27223;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 4:
                                armorWeenie = 27224;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27225;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 6);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27226;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 27227;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 27228;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 27229;
                                armorPieceType = 1;
                                spellArray = 6;
                                cantripArray = 6;
                                break;
                            case 4:
                                armorWeenie = 27230;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 5:
                                armorWeenie = 27231;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27232;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 27215;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 1:
                                armorWeenie = 27216;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 2:
                                armorWeenie = 27217;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            case 3:
                                armorWeenie = 27218;
                                armorPieceType = 1;
                                spellArray = 7;
                                cantripArray = 7;
                                break;
                            default:
                                armorWeenie = 27219;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 4);
                        switch (armorPiece)
                        {
                            case 0:
                                armorWeenie = 30950;
                                armorPieceType = 4;
                                spellArray = 9;
                                cantripArray = 9;
                                break;
                            case 1:
                                armorWeenie = 28629;
                                armorPieceType = 1;
                                spellArray = 2;
                                cantripArray = 2;
                                break;
                            case 2:
                                armorWeenie = 30951;
                                armorPieceType = 3;
                                spellArray = 5;
                                cantripArray = 5;
                                break;
                            case 3:
                                armorWeenie = 28617;
                                armorPieceType = 2;
                                spellArray = 1;
                                cantripArray = 1;
                                break;
                            default:
                                armorWeenie = 28620;
                                armorPieceType = 1;
                                spellArray = 3;
                                cantripArray = 3;
                                break;
                        }

                        materialType = GetMaterialType(7, 1);
                    }
                    //////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43830;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Alduressa
                    //if (armorType == 18)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37207;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37217;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37187;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 37200;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37195;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Amuli
                    //if (armorType == 19)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37208;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37299;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37188;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37201;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37196;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(6, 1);
                    //}
                    //////Olthoi Celdon
                    //if (armorType == 20)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 7);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37209;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37214;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37189;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37202;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 37192;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 37205;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37197;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Celdon
                    //if (armorType == 21)
                    //{
                    //    int armorPiece = ThreadSafeRandom.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37215;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37190;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37203;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37206;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37198;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = ThreadSafeRandom.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    wieldDifficulty = 180;
                    break;

            }
            ////ArmorModVsSlash, with a value between 0.0-2.0.
            armorModSlash = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsPierce, with a value between 0.0-2.0.
            armorModPierce = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsBludgeon, with a value between 0.0-2.0.
            armorModBludge = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsCold, with a value between 0.0-2.0.
            armorModCold = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsFire, with a value between 0.0-2.0.
            armorModFire = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsAcid, with a value between 0.0-2.0.
            armorModAcid = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsElectric, with a value between 0.0-2.0.
            armorModElectric = .1 * ThreadSafeRandom.Next(1, 20);
            ////ArmorModVsNether, with a value between 0.0-2.0.
            armorModNether = .1 * ThreadSafeRandom.Next(1, 20);

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);
            int gemCount = ThreadSafeRandom.Next(1, 6);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.SetProperty(PropertyInt.MaterialType, materialType);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyFloat.ArmorModVsSlash, armorModSlash);
            wo.SetProperty(PropertyFloat.ArmorModVsPierce, armorModPierce);
            wo.SetProperty(PropertyFloat.ArmorModVsCold, armorModCold);
            wo.SetProperty(PropertyFloat.ArmorModVsBludgeon, armorModBludge);
            wo.SetProperty(PropertyFloat.ArmorModVsFire, armorModFire);
            wo.SetProperty(PropertyFloat.ArmorModVsAcid, armorModAcid);
            wo.SetProperty(PropertyFloat.ArmorModVsElectric, armorModElectric);
            wo.SetProperty(PropertyFloat.ArmorModVsNether, armorModNether);
            wo.SetProperty(PropertyInt.AppraisalItemSkill, 7);
            wo.SetProperty(PropertyInt.EquipmentSetId, equipSetId);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);
            ////Encumberance will be added based on item in the future

            int numSpells = 0;

            if (isMagical)
                numSpells = GetNumSpells(tier);

            maxMana = GetMaxMana(numSpells, tier);
            curMana = maxMana;
            wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
            wo.SetProperty(PropertyInt.ItemCurMana, curMana);
            int numCantrips = GetNumCantrips(numSpells);

            switch (spellArray)
            {
                case 1:
                    spells = LootHelper.HeadSpells;
                    break;
                case 2:
                    spells = LootHelper.ChestSpells;
                    break;
                case 3:
                    spells = LootHelper.UpperArmSpells;
                    break;
                case 4:
                    spells = LootHelper.LowerArmSpells;
                    break;
                case 5:
                    spells = LootHelper.HandSpells;
                    break;
                case 6:
                    spells = LootHelper.AbdomenSpells;
                    break;
                case 7:
                    spells = LootHelper.UpperLegSpells;
                    break;
                case 8:
                    spells = LootHelper.LowerLegSpells;
                    break;
                case 9:
                    spells = LootHelper.FeetSpells;
                    break;
                default:
                    spells = LootHelper.ShieldSpells;
                    break;
            }

            switch (cantripArray)
            {
                case 1:
                    cantrips = LootHelper.HeadCantrips;
                    break;
                case 2:
                    cantrips = LootHelper.ChestCantrips;
                    break;
                case 3:
                    cantrips = LootHelper.UpperArmCantrips;
                    break;
                case 4:
                    cantrips = LootHelper.LowerArmCantrips;
                    break;
                case 5:
                    cantrips = LootHelper.HandCantrips;
                    break;
                case 6:
                    cantrips = LootHelper.AbdomenCantrips;
                    break;
                case 7:
                    cantrips = LootHelper.UpperLegCantrips;
                    break;
                case 8:
                    cantrips = LootHelper.LowerLegCantrips;
                    break;
                case 9:
                    cantrips = LootHelper.FeetCantrips;
                    break;
                default:
                    cantrips = LootHelper.ShieldCantrips;
                    break;
            }

            int[] shuffledValues = new int[spells.Length];
            for (int i = 0; i < spells.Length; i++)
            {
                shuffledValues[i] = i;
            }

            Shuffle(shuffledValues);
            if (numSpells - numCantrips > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = ThreadSafeRandom.Next(lowSpellTier - 1, highSpellTier - 1);
                    int spellID = spells[shuffledValues[a]][col];
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }

            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
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
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }

            if (wieldDifficulty > 0)
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
            else
                wo.RemoveProperty(PropertyInt.WieldDifficulty);

            /////Setting random color
            wo.SetProperty(PropertyInt.PaletteTemplate, ThreadSafeRandom.Next(1, 2047));
            double shade = .1 * ThreadSafeRandom.Next(0, 9);
            wo.SetProperty(PropertyFloat.Shade, shade);
            spellcraft = GetSpellcraft(numSpells, tier);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));
            int workmanship2 = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship2);
            if (numSpells > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
            }
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship2));

            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

            if (baseArmorLevel == 0)
                wo.RemoveProperty(PropertyInt.ArmorLevel);
            else
            {
                int adjustedArmorLevel = baseArmorLevel + GetArmorLevelModifier(tier, armorPieceType);
                wo.SetProperty(PropertyInt.ArmorLevel, adjustedArmorLevel);
            }

            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));
            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            return wo;
        }

        private static int GetArmorLevelModifier(int tier, int armorType)
        {
            switch (tier)
            {
                case 1:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(10, 37);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(10, 33);
                    else
                        return ThreadSafeRandom.Next(10, 50);
                case 2:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(37, 72);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(34, 57);
                    else
                        return ThreadSafeRandom.Next(51, 90);
                case 3:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(73, 109);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(58, 82);
                    else
                        return ThreadSafeRandom.Next(92, 132);
                case 4:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(109, 145);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(82, 106);
                    else
                        return ThreadSafeRandom.Next(133, 173);
                case 5:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(145, 181);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(106, 130);
                    else
                        return ThreadSafeRandom.Next(173, 213);
                case 6:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(181, 217);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(130, 154);
                    else
                        return ThreadSafeRandom.Next(213, 254);
                case 7:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(217, 253);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(154, 178);
                    else
                        return ThreadSafeRandom.Next(254, 294);
                case 8:
                    if (armorType == 1)
                        return ThreadSafeRandom.Next(253, 304);
                    else if (armorType == 5)
                        return ThreadSafeRandom.Next(178, 202);
                    else
                        return ThreadSafeRandom.Next(294, 335);
                default:
                    return 0;
            }
        }

        private static int GetWield(int tier, int type)
        {

            int wield = 0;
            int chance = 0;
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
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else
                            {
                                wield = 250;
                            }
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 270;
                            }
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 270;
                            }
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 270;
                            }
                            else if (chance < 90)
                            {
                                wield = 290;
                            }
                            else
                            {
                                wield = 315;
                            }
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 315;
                            }
                            else if (chance < 90)
                            {
                                wield = 335;
                            }
                            else
                            {
                                wield = 360;
                            }
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 335;
                            }
                            else if (chance < 90)
                            {
                                wield = 360;
                            }
                            else
                            {
                                wield = 375;
                            }
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 360;
                            }
                            else if (chance < 90)
                            {
                                wield = 375;
                            }
                            else
                            {
                                wield = 385;
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (tier)
                    {
                        case 1:
                            wield = 0;
                            break;
                        case 2:
                            wield = 0;
                            break;
                        case 3:
                            wield = 0;
                            break;
                        case 4:
                            wield = 0;
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 290;
                            }
                            else
                            {
                                wield = 310;
                            }
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 40)
                            {
                                wield = 0;
                            }
                            else if (chance < 70)
                            {
                                wield = 310;
                            }
                            else if (chance < 90)
                            {
                                wield = 330;
                            }
                            else
                            {
                                wield = 365;
                            }
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 330;
                            }
                            else if (chance < 90)
                            {
                                wield = 365;
                            }
                            else
                            {
                                wield = 375;
                            }
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 355;
                            }
                            else if (chance < 90)
                            {
                                wield = 375;
                            }
                            else
                            {
                                wield = 385;
                            }
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
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else
                            {
                                wield = 250;
                            }
                            break;
                        case 3:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 300;
                            }
                            break;
                        case 4:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 300;
                            }
                            break;
                        case 5:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 300;
                            }
                            else if (chance < 90)
                            {
                                wield = 325;
                            }
                            else
                            {
                                wield = 350;
                            }
                            break;
                        case 6:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 350;
                            }
                            else if (chance < 90)
                            {
                                wield = 370;
                            }
                            else
                            {
                                wield = 400;
                            }
                            break;
                        case 7:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 370;
                            }
                            else if (chance < 90)
                            {
                                wield = 400;
                            }
                            else
                            {
                                wield = 420;
                            }
                            break;
                        case 8:
                            chance = ThreadSafeRandom.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 400;
                            }
                            else if (chance < 90)
                            {
                                wield = 420;
                            }
                            else
                            {
                                wield = 430;
                            }
                            break;
                    }
                    break;
            }

            return wield;
        }

        private static double GetManaRate()
        {
            double manaRate = 1.0 / (double)(ThreadSafeRandom.Next(10, 30));
            return -manaRate;
        }

        private static int GetNumSpells(int tier)
        {
            int chance = 0;
            int numSpells = 0;
            switch (tier)
            {
                case 1:
                    ////1-3, minor cantrips
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance < 50)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 90)
                    {
                        numSpells = 2;
                    }
                    else
                    {
                        numSpells = 3;
                    }
                    break;
                case 2:
                    ////3-5 minor, and major
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 500)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 900)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 950)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 998)
                    {
                        numSpells = 4;
                    }
                    else
                    {
                        numSpells = 5;
                    }
                    break;
                case 3:
                    //4-6, major/minor
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 500)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 800)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 900)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 950)
                    {
                        numSpells = 4;
                    }
                    else if (chance < 985)
                    {
                        numSpells = 5;
                    }
                    else
                    {
                        numSpells = 6;
                    }
                    break;
                case 4:
                    //5-6, major and minor
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 500)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 800)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 900)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 950)
                    {
                        numSpells = 4;
                    }
                    else if (chance < 985)
                    {
                        numSpells = 5;
                    }
                    else
                    {
                        numSpells = 6;
                    }
                    break;
                case 5:
                    //5-7 major/minor
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 500)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 600)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 700)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 850)
                    {
                        numSpells = 4;
                    }
                    else if (chance < 940)
                    {
                        numSpells = 5;
                    }
                    else if (chance < 980)
                    {
                        numSpells = 6;
                    }
                    else
                    {
                        numSpells = 7;
                    }
                    break;
                case 6:
                    //6-7, minor(4 total) major(2 total)
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 200)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 300)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 400)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 500)
                    {
                        numSpells = 4;
                    }
                    else if (chance < 600)
                    {
                        numSpells = 5;
                    }
                    else if (chance < 700)
                    {
                        numSpells = 6;
                    }
                    else if (chance < 950)
                    {
                        numSpells = 7;
                    }
                    else
                    {
                        numSpells = 8;
                    }
                    break;
                case 7:
                    ///6-8, minor(4), major(5), epic(3)
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 200)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 300)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 400)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 500)
                    {
                        numSpells = 4;
                    }
                    else if (chance < 600)
                    {
                        numSpells = 5;
                    }
                    else if (chance < 700)
                    {
                        numSpells = 6;
                    }
                    else if (chance < 950)
                    {
                        numSpells = 7;
                    }
                    else
                    {
                        numSpells = 8;
                    }
                    break;
                default:
                    //6-8, minor(4), major(5), epic(3), legendary(2)
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance < 200)
                    {
                        numSpells = 1;
                    }
                    else if (chance < 300)
                    {
                        numSpells = 2;
                    }
                    else if (chance < 400)
                    {
                        numSpells = 3;
                    }
                    else if (chance < 500)
                    {
                        numSpells = 4;
                    }
                    else if (chance < 600)
                    {
                        numSpells = 5;
                    }
                    else if (chance < 700)
                    {
                        numSpells = 6;
                    }
                    else if (chance < 950)
                    {
                        numSpells = 7;
                    }
                    else
                    {
                        numSpells = 8;
                    }
                    break;

            }

            return numSpells;
        }

        private static int GetNumCantrips(int spellAmount)
        {
            int chance = 0;
            int numSpells = 0;
            switch (spellAmount)
            {
                case 1:
                    if (ThreadSafeRandom.Next(0, 100) > 90)
                    {
                        return 1;
                    }
                    break;
                case 2:
                    if (ThreadSafeRandom.Next(0, 100) > 90)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 750)
                        {
                            numSpells = 1;
                        }
                        else
                        {
                            numSpells = 2;
                        }
                    }
                    break;
                case 3:
                    if (ThreadSafeRandom.Next(0, 100) > 60)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 750)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 900)
                        {
                            numSpells = 2;
                        }
                        else
                        {
                            numSpells = 3;
                        }
                    }
                    break;
                case 4:
                    if (ThreadSafeRandom.Next(0, 100) > 60)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 800)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 3;
                        }
                        else
                        {
                            numSpells = 4;
                        }
                    }
                    break;
                case 5:
                    if (ThreadSafeRandom.Next(0, 100) > 60)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 850)
                        {
                            numSpells = 4;
                        }
                        else
                        {
                            numSpells = 5;
                        }
                    }
                    break;
                case 6:
                    if (ThreadSafeRandom.Next(0, 100) > 60)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                    }
                    break;
                case 7:
                    if (ThreadSafeRandom.Next(0, 100) > 60)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else
                        {
                            numSpells = 7;
                        }
                    }
                    break;
                case 8:
                    //6-8, minor(4), major(5), epic(3), legendary(2)
                    if (ThreadSafeRandom.Next(0, 100) > 60)
                    {
                        chance = ThreadSafeRandom.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 7;
                        }
                        else
                        {
                            numSpells = 8;
                        }
                    }
                    break;
                default:
                    break;

            }

            return numSpells;
        }

        private static double GetDamageModifier(int wield, int weaponType)
        {
            ///0 bow
            ///1 crossbow
            ///2 atlatl
            double damageMod = 0;
            if (wield == 0)
            {
                if (weaponType == 0)
                    damageMod = 2.10;
                else if (weaponType == 1)
                    damageMod = 2.40;
                else
                    damageMod = 2.3;
            }
            else
            {
                if (weaponType == 0)
                    damageMod = 2.40;
                else if (weaponType == 1)
                    damageMod = 2.65;
                else
                    damageMod = 2.60;
            }

            return damageMod;
        }

        private static WorldObject CreateMissileWeapon(int tier)
        {
            int[][] spells = LootHelper.MissileSpells;
            int[][] cantrips = LootHelper.MissileCantrips;

            ////Double Values
            double manaRate = -.04166667; ///done

            int weaponWeenie;
            int subType;
            int chance;
            int elemenatalBonus = 0;

            int wieldDifficulty = GetWield(tier, 1);

            if (tier < 4)
                GetNonElementalMissileWeapon(out weaponWeenie, out subType);
            else
            {
                chance = ThreadSafeRandom.Next(0, 1);
                switch (chance)
                {
                    case 0:
                        GetNonElementalMissileWeapon(out weaponWeenie, out subType);
                        break;
                    default:
                        elemenatalBonus = GetElementalBonus(wieldDifficulty);
                        GetElementalMissileWeapon(out weaponWeenie, out subType);
                        break;
                }
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship));
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(2, tier));
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            // wo.SetProperty(PropertyFloat.DamageMod, GetDamageModifier(wieldDifficulty, subType));
            wo.SetProperty(PropertyFloat.WeaponDefense, GetMeleeDMod(tier));
            wo.SetProperty(PropertyFloat.WeaponMissileDefense, GetMissileDMod(tier));
            // wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicDefense);
            wo.SetProperty(PropertyFloat.WeaponOffense, 1);

            if (elemenatalBonus > 0)
                wo.SetProperty(PropertyInt.ElementalDamageBonus, elemenatalBonus);

            if (wieldDifficulty > 0)
            {
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.RawSkill);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)Skill.MissileWeapons);
            }

            wo.SetProperty(PropertyFloat.ManaRate, manaRate);

            int numSpells = GetNumSpells(tier);
            int numCantrips = GetNumCantrips(numSpells);
            int spellCraft = GetSpellcraft(numSpells, tier);
            int lowSpellTier = GetLowSpellTier(tier);
            int highSpellTier = GetHighSpellTier(tier);
            int itemSkillLevelLimit = 0;

            wo.SetProperty(PropertyInt.ItemMaxMana, GetMaxMana(numSpells, tier));
            wo.SetProperty(PropertyInt.ItemCurMana, GetMaxMana(numSpells, tier));
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellCraft);
            wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellCraft));
            wo.SetProperty(PropertyInt.ItemSkillLevelLimit, itemSkillLevelLimit);

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
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            if (numCantrips > 0)
            {
                shuffledValues = new int[cantrips.Length];
                for (int i = 0; i < cantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }

            if (numCantrips > 0 || numSpells > 0)
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
                wo.RemoveProperty(PropertyFloat.ManaRate);
            }

            return wo;
        }

        private static void GetElementalMissileWeapon(out int weaponWeenie, out int subType)
        {
            int chance = ThreadSafeRandom.Next(0, 6);

            subType = ThreadSafeRandom.Next(0, 5);
            switch (subType)
            {
                case 0: // Bow
                    switch (chance)
                    {
                        case 0: // Slashing
                            weaponWeenie = 29244;
                            break;
                        case 1: // Blunt
                            weaponWeenie = 29239;
                            break;
                        case 2: // Piercing
                            weaponWeenie = 29243;
                            break;
                        case 3: // Fire
                            weaponWeenie = 29241;
                            break;
                        case 4: // Frost
                            weaponWeenie = 29242;
                            break;
                        case 5: // Acid
                            weaponWeenie = 29238;
                            break;
                        default: // Electric
                            weaponWeenie = 29240;
                            break;
                    }
                    break;
                case 1: // Crossbow
                    switch (chance)
                    {
                        case 0: // Slashing
                            weaponWeenie = 29251;
                            break;
                        case 1: // Blunt
                            weaponWeenie = 29246;
                            break;
                        case 2: // Piercing
                            weaponWeenie = 29250;
                            break;
                        case 3: // Fire
                            weaponWeenie = 29248;
                            break;
                        case 4: // Frost
                            weaponWeenie = 29249;
                            break;
                        case 5: // Acid
                            weaponWeenie = 29245;
                            break;
                        default: // Electric
                            weaponWeenie = 29247;
                            break;
                    }
                    break;
                case 2: // Atlatl
                    switch (chance)
                    {
                        case 0: // Slashing
                            weaponWeenie = 29258;
                            break;
                        case 1: // Blunt
                            weaponWeenie = 29253;
                            break;
                        case 2: // Piercing
                            weaponWeenie = 29257;
                            break;
                        case 3: // Fire
                            weaponWeenie = 29255;
                            break;
                        case 4: // Frost
                            weaponWeenie = 29256;
                            break;
                        case 5: // Acid
                            weaponWeenie = 29252;
                            break;
                        default: // Electric
                            weaponWeenie = 29254;
                            break;
                    }
                    break;
                case 3: // Slingshot
                    switch (chance)
                    {
                        case 0: // Slashing
                            weaponWeenie = 31812;
                            break;
                        case 1: // Blunt
                            weaponWeenie = 31814;
                            break;
                        case 2: // Piercing
                            weaponWeenie = 31818;
                            break;
                        case 3: // Fire
                            weaponWeenie = 31816;
                            break;
                        case 4: // Frost
                            weaponWeenie = 31817;
                            break;
                        case 5: // Acid
                            weaponWeenie = 31813;
                            break;
                        default: // Electric
                            weaponWeenie = 31815;
                            break;
                    }
                    break;
                case 4: // Compound Bow
                    switch (chance)
                    {
                        case 0: // Slashing
                            weaponWeenie = 31798;
                            break;
                        case 1: // Blunt
                            weaponWeenie = 31800;
                            break;
                        case 2: // Piercing
                            weaponWeenie = 31804;
                            break;
                        case 3: // Fire
                            weaponWeenie = 31802;
                            break;
                        case 4: // Frost
                            weaponWeenie = 31803;
                            break;
                        case 5: // Acid
                            weaponWeenie = 31799;
                            break;
                        default: // Electric
                            weaponWeenie = 31801;
                            break;
                    }
                    break;
                default: // Compound Crossbow
                    switch (chance)
                    {
                        case 0: // Slashing
                            weaponWeenie = 31805;
                            break;
                        case 1: // Blunt
                            weaponWeenie = 31807;
                            break;
                        case 2: // Piercing
                            weaponWeenie = 31811;
                            break;
                        case 3: // Fire
                            weaponWeenie = 31809;
                            break;
                        case 4: // Frost
                            weaponWeenie = 31810;
                            break;
                        case 5: // Acid
                            weaponWeenie = 31806;
                            break;
                        default: // Electric
                            weaponWeenie = 31808;
                            break;
                    }
                    break;
            }
        }

        private static void GetNonElementalMissileWeapon(out int weaponWeenie, out int subType)
        {
            subType = ThreadSafeRandom.Next(0, 2);
            if (subType == 0)
            {
                ////There are 8 subtypes of Bows
                int subBowType = ThreadSafeRandom.Next(0, 6);
                switch (subBowType)
                {
                    case 0: // Longbow
                        weaponWeenie = 306;
                        break;
                    case 1: // Yumi
                        weaponWeenie = 363;
                        break;
                    case 2: // Nayin
                        weaponWeenie = 334;
                        break;
                    case 3: // Shortbow
                        weaponWeenie = 307;
                        break;
                    case 4: // Shouyumi
                        weaponWeenie = 341;
                        break;
                    case 5: // War Bow
                        weaponWeenie = 30625;
                        break;
                    default: // Yag
                        weaponWeenie = 360;
                        break;
                }
            }
            else if (subType == 1)
            {
                ////There are 4 subtypes of Crossbows
                int subXbowType = ThreadSafeRandom.Next(0, 2);
                if (subXbowType == 0) // Arbalest
                    weaponWeenie = 30616;
                else if (subXbowType == 1) // Heavy Crossbow
                    weaponWeenie = 311;
                else // Light Crossbow
                    weaponWeenie = 312;
            }
            else
            {
                ////There are 3 subtypes of Atlatl
                int subAtlatlType = ThreadSafeRandom.Next(0, 1);
                if (subAtlatlType == 0) // Atlatl
                    weaponWeenie = 12463;
                else // Royal Atlatl
                    weaponWeenie = 20640;
            }
        }

        private enum CasterType : ushort
        {
            Orb,
            Wand,
            Staff,
            Sceptre,
            Baton
        }

        private static WorldObject CreateCaster(int tier)
        {
            int[][] WandSpells =
            {
                ////Focus
                new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
                ////Willpower
                new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
                ////Sneak Attack
                new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
                ////Arcane Enlight
                new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
                ////Mana C
                new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
                ////Creature Enchant
                new int[] { 557, 558, 559, 560, 561, 562 , 2215, 4530},
                ////Item Enchant
                new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
                ////Life Magic
                new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
                ////War Magic
                new int[] { 629, 630, 631, 632, 633, 634 , 2287, 4602},
                ////Defender
                new int[] { 1599, 1600, 1601, 1602, 1603, 1604, 2101, 4400 },
                ////Hermetic Link
                new int[] { 1475, 1476, 1477, 1478, 1479, 1480, 2117, 4418 },
             };

            int casterWeenie = 0; //done
            double elementalDamageMod = 0;
            int wieldReqs = 2; //done
            Skill wieldSkillType = Skill.None;
            int wield = 0; //done
            int chance = 0; //done

            switch (tier)
            {
                case 1:
                    wield = 0;
                    break;
                case 2:
                    wield = 0;
                    break;
                case 3:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 80)
                        wield = 0;
                    else
                        wield = 290;
                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 60)
                        wield = 0;
                    else if (chance < 95)
                        wield = 290;
                    else
                        wield = 310;
                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 50)
                        wield = 0;
                    else if (chance < 70)
                        wield = 290;
                    else if (chance < 90)
                        wield = 310;
                    else
                        wield = 330;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 40)
                        wield = 0;
                    else if (chance < 60)
                        wield = 290;
                    else if (chance < 80)
                        wield = 310;
                    else if (chance < 90)
                        wield = 330;
                    else
                        wield = 355;
                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 30)
                        wield = 0;
                    else if (chance < 50)
                        wield = 290;
                    else if (chance < 60)
                        wield = 310;
                    else if (chance < 70)
                        wield = 330;
                    else if (chance < 90)
                        wield = 355;
                    else
                        wield = 375;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 25)
                        wield = 0;
                    else if (chance < 50)
                        wield = 290;
                    else if (chance < 60)
                        wield = 310;
                    else if (chance < 70)
                        wield = 330;
                    else if (chance < 80)
                        wield = 355;
                    else if (chance < 90)
                        wield = 375;
                    else
                        wield = 385;
                    break;
            }

            ////Getting the caster Weenie needed.
            if (wield == 0)
            {
                int subType = ThreadSafeRandom.Next(0, 3);
                switch (subType)
                {
                    case 0:
                        casterWeenie = 2366; // Orb
                        break;
                    case 1:
                        casterWeenie = 2548; // Sceptre
                        break;
                    case 2:
                        casterWeenie = 2547; // Staff
                        break;
                    default:
                        casterWeenie = 2472; // Wand
                        break;
                }
            }
            else
            {
                // Determine the Elemental Damage Mod amount
                elementalDamageMod = GetMaxDamageMod(tier, 18);

                // Determine skill of wield requirement
                chance = ThreadSafeRandom.Next(0, 4);
                switch (chance)
                {
                    case 0:
                        wieldSkillType = Skill.VoidMagic;
                        break;
                    case 1:
                        wieldSkillType = Skill.WarMagic;
                        break;
                    case 2:
                        wieldSkillType = Skill.CreatureEnchantment;
                        break;
                    case 3:
                        wieldSkillType = Skill.ItemEnchantment;
                        break;
                    default:
                        wieldSkillType = Skill.LifeMagic;
                        break;
                }

                // Determine caster type
                CasterType casterType;
                chance = ThreadSafeRandom.Next(0, 2);
                switch (chance)
                {
                    case 0:
                        casterType = CasterType.Baton;
                        break;
                    case 1:
                        casterType = CasterType.Staff;
                        break;
                    default:
                        casterType = CasterType.Sceptre;
                        break;
                }

                if (wieldSkillType == Skill.VoidMagic)
                {
                    if (casterType == CasterType.Sceptre)
                        casterWeenie = 43381; // Nether Sceptre
                    else if (casterType == CasterType.Baton)
                        casterWeenie = 43382; // Nether Baton
                    else
                        casterWeenie = 43383; // Nether Staff
                }
                else
                {
                    chance = ThreadSafeRandom.Next(0, 6);
                    switch (chance)
                    {
                        case 0:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29265; // Slashing Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31819; // Slashing Baton
                            else
                                casterWeenie = 37223; // Slashing Staff
                            break;
                        case 1:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29264; // Piercing Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31825; // Piercing Baton
                            else
                                casterWeenie = 37222; // Piercing Staff
                            break;
                        case 2:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29260; // Blunt Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31821; // Blunt Baton
                            else
                                casterWeenie = 37225; // Blunt Staff
                            break;
                        case 3:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29263; // Frost Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31824; // Frost Baton
                            else
                                casterWeenie = 37221; // Frost Staff
                            break;
                        case 4:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29262; // Fire Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31823; // Fire Baton
                            else
                                casterWeenie = 37220; // Fire Staff
                            break;
                        case 5:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29259; // Acid Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31820; // Acid Baton
                            else
                                casterWeenie = 37224; // Acid Staff
                            break;
                        default:
                            if (casterType == CasterType.Sceptre)
                                casterWeenie = 29261; // Electric Sceptre
                            else if (casterType == CasterType.Baton)
                                casterWeenie = 31822; // Electric Baton
                            else
                                casterWeenie = 37219; // Electric Staff
                            break;
                    }
                }
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)casterWeenie);

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship));
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(3, tier));
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            if (ThreadSafeRandom.Next(0, 100) > 95)
                wo.SetProperty(PropertyFloat.WeaponMissileDefense, GetMissileDMod(tier));
            else
            {
                var meleeDMod = GetMeleeDMod(tier);
                if (meleeDMod > 1.0f)
                    wo.SetProperty(PropertyFloat.WeaponDefense, GetMeleeDMod(tier));
            }

            double manaConMod = GetManaCMod(tier);
            wo.SetProperty(PropertyFloat.ManaConversionMod, manaConMod);

            if (elementalDamageMod > 1.0f)
                wo.SetProperty(PropertyFloat.ElementalDamageMod, elementalDamageMod);

            if (wield > 0)
            {
                wo.SetProperty(PropertyInt.WieldRequirements, wieldReqs);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)wieldSkillType);
                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }

            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);

            int lowSpellTier = GetLowSpellTier(tier);
            int highSpellTier = GetHighSpellTier(tier);
            int numSpells = GetNumSpells(tier);
            int spellcraft = GetSpellcraft(numSpells, tier);
            int itemMaxMana = GetMaxMana(numSpells, tier);

            wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));
            wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());

            wo.SetProperty(PropertyInt.ItemMaxMana, itemMaxMana);
            wo.SetProperty(PropertyInt.ItemCurMana, itemMaxMana);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 7);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);

            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

            if (numCantrips > 0 || numSpells > 0)
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

            int[][] spells = LootHelper.WandSpells;
            int[][] cantrips = LootHelper.WandCantrips;
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
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
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
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }

            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            return wo;
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

        private static int GetElementalBonus(int wield)
        {
            int chance = 0;
            int eleMod = 0;
            switch (wield)
            {
                case 0:
                    break;
                case 250:
                    break;
                case 270:
                    break;
                case 315:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 20)
                        eleMod = 1;
                    else if (chance < 40)
                        eleMod = 2;
                    else if (chance < 70)
                        eleMod = 3;
                    else if (chance < 95)
                        eleMod = 4;
                    else
                        eleMod = 5;
                    break;
                case 335:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 20)
                        eleMod = 5;
                    else if (chance < 40)
                        eleMod = 6;
                    else if (chance < 70)
                        eleMod = 7;
                    else if (chance < 95)
                        eleMod = 8;
                    else
                        eleMod = 9;
                    break;
                case 360:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 20)
                        eleMod = 10;
                    else if (chance < 30)
                        eleMod = 11;
                    else if (chance < 45)
                        eleMod = 12;
                    else if (chance < 60)
                        eleMod = 13;
                    else if (chance < 75)
                        eleMod = 14;
                    else if (chance < 95)
                        eleMod = 15;
                    else
                        eleMod = 16;
                    break;
                case 375:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 20)
                        eleMod = 12;
                    else if (chance < 30)
                        eleMod = 13;
                    else if (chance < 45)
                        eleMod = 14;
                    else if (chance < 60)
                        eleMod = 15;
                    else if (chance < 75)
                        eleMod = 16;
                    else if (chance < 95)
                        eleMod = 17;
                    else
                        eleMod = 18;
                    break;
                case 385:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 20)
                        eleMod = 16;
                    else if (chance < 30)
                        eleMod = 17;
                    else if (chance < 45)
                        eleMod = 18;
                    else if (chance < 60)
                        eleMod = 19;
                    else if (chance < 75)
                        eleMod = 20;
                    else if (chance < 95)
                        eleMod = 21;
                    else
                        eleMod = 22;
                    break;
            }

            return eleMod;
        }

        //The percentages for variances need to be fixed
        private static double GetVariance(int category, int type)
        {
            double variance = 0;
            int chance = ThreadSafeRandom.Next(0, 100);

            switch (category)
            {
                case 1:
                    //Heavy Weapons
                    switch (type)
                    {
                        case 1:
                            //Axe
                            if (chance < 10)
                                variance = .90;
                            if (chance < 30)
                                variance = .93;
                            if (chance < 70)
                                variance = .95;
                            if (chance < 90)
                                variance = .97;
                            else
                                variance = .99;
                            break;
                        case 2:
                            //Dagger
                            if (chance < 10)
                                variance = .47;
                            if (chance < 30)
                                variance = .50;
                            if (chance < 70)
                                variance = .53;
                            if (chance < 90)
                                variance = .57;
                            else
                                variance = .62;
                            break;
                        case 3:
                            //Dagger MultiStrike
                            if (chance < 10)
                                variance = .40;
                            if (chance < 30)
                                variance = .43;
                            if (chance < 70)
                                variance = .48;
                            if (chance < 90)
                                variance = .53;
                            else
                                variance = .58;
                            break;
                        case 4:
                            //Mace
                            if (chance < 10)
                                variance = .30;
                            if (chance < 30)
                                variance = .33;
                            if (chance < 70)
                                variance = .37;
                            if (chance < 90)
                                variance = .42;
                            else
                                variance = .46;
                            break;
                        case 5:
                            //Spear
                            if (chance < 10)
                                variance = .59;
                            if (chance < 30)
                                variance = .63;
                            if (chance < 70)
                                variance = .68;
                            if (chance < 90)
                                variance = .72;
                            else
                                variance = .75;
                            break;
                        case 6:
                            //Staff
                            if (chance < 10)
                                variance = .38;
                            if (chance < 30)
                                variance = .42;
                            if (chance < 70)
                                variance = .45;
                            if (chance < 90)
                                variance = .50;
                            else
                                variance = .52;
                            break;
                        case 7:
                            //Sword
                            if (chance < 10)
                                variance = .47;
                            if (chance < 30)
                                variance = .50;
                            if (chance < 70)
                                variance = .53;
                            if (chance < 90)
                                variance = .57;
                            else
                                variance = .62;
                            break;
                        case 8:
                            //Sword Multistrike
                            if (chance < 10)
                                variance = .40;
                            if (chance < 30)
                                variance = .43;
                            if (chance < 70)
                                variance = .48;
                            if (chance < 90)
                                variance = .53;
                            else
                                variance = .60;
                            break;
                        case 9:
                            //UA
                            if (chance < 10)
                                variance = .44;
                            if (chance < 30)
                                variance = .48;
                            if (chance < 70)
                                variance = .53;
                            if (chance < 90)
                                variance = .58;
                            else
                                variance = .60;
                            break;
                    }
                    break;
                case 2:
                    //Finesse/Light Weapons
                    switch (type)
                    {
                        case 1:
                            //Axe
                            if (chance < 10)
                                variance = .80;
                            if (chance < 30)
                                variance = .83;
                            if (chance < 70)
                                variance = .85;
                            if (chance < 90)
                                variance = .90;
                            else
                                variance = .95;
                            break;
                        case 2:
                            //Dagger
                            if (chance < 10)
                                variance = .42;
                            if (chance < 30)
                                variance = .47;
                            if (chance < 70)
                                variance = .52;
                            if (chance < 90)
                                variance = .56;
                            else
                                variance = .60;
                            break;
                        case 3:
                            //Dagger MultiStrike
                            if (chance < 10)
                                variance = .24;
                            if (chance < 30)
                                variance = .28;
                            if (chance < 70)
                                variance = .35;
                            if (chance < 90)
                                variance = .40;
                            else
                                variance = .45;
                            break;
                        case 4:
                            //Mace
                            if (chance < 10)
                                variance = .23;
                            if (chance < 30)
                                variance = .28;
                            if (chance < 70)
                                variance = .32;
                            if (chance < 90)
                                variance = .37;
                            else
                                variance = .43;
                            break;
                        case 5:
                            //Jitte
                            if (chance < 10)
                                variance = .325;
                            if (chance < 30)
                                variance = .35;
                            if (chance < 70)
                                variance = .40;
                            if (chance < 90)
                                variance = .45;
                            else
                                variance = .50;
                            break;
                        case 6:
                            //Spear
                            if (chance < 10)
                                variance = .65;
                            if (chance < 30)
                                variance = .68;
                            if (chance < 70)
                                variance = .71;
                            if (chance < 90)
                                variance = .75;
                            else
                                variance = .80;
                            break;
                        case 7:
                            //Staff
                            if (chance < 10)
                                variance = .325;
                            if (chance < 30)
                                variance = .35;
                            if (chance < 70)
                                variance = .40;
                            if (chance < 90)
                                variance = .45;
                            else
                                variance = .50;
                            break;
                        case 8:
                            //Sword
                            if (chance < 10)
                                variance = .42;
                            if (chance < 30)
                                variance = .47;
                            if (chance < 70)
                                variance = .52;
                            if (chance < 90)
                                variance = .56;
                            else
                                variance = .60;
                            break;
                        case 9:
                            //Sword Multistrike
                            if (chance < 10)
                                variance = .24;
                            if (chance < 30)
                                variance = .28;
                            if (chance < 70)
                                variance = .35;
                            if (chance < 90)
                                variance = .40;
                            else
                                variance = .45;
                            break;
                        case 10:
                            //UA
                            if (chance < 10)
                                variance = .44;
                            if (chance < 30)
                                variance = .48;
                            if (chance < 70)
                                variance = .53;
                            if (chance < 90)
                                variance = .58;
                            else
                                variance = .60;
                            break;
                    }
                    break;
                case 3:
                    /// Two Handed only have one set of variances
                    if (chance < 5)
                        variance = .30;
                    if (chance < 20)
                        variance = .35;
                    if (chance < 50)
                        variance = .40;
                    if (chance < 80)
                        variance = .45;
                    if (chance < 95)
                        variance = .50;
                    else
                        variance = .55;
                    break;
                default:
                    break;
            }

            return variance;
        }

        private static int GetMaxDamage(int weaponType, int tier, int wieldDiff, int baseWeapon)
        {
            ///weaponType: 1 Heavy, 2 Finesse/Light, 3 two-handed
            ///baseWeapon: 1 Axe, 2 Dagger, 3 DaggerMulti, 4 Mace, 5 Spear, 6 Sword, 7 SwordMulti, 8 Staff, 9 UA

            int[,] lightWeaponDamageTable =
            {
                { 22, 28, 33, 39, 44, 50, 55, 57, 61},
                { 18, 24, 29, 35, 40, 46, 51, 54, 58},
                {  7, 10, 13, 16, 18, 21, 24, 27, 28},
                { 19, 24, 29, 35, 40, 45, 50, 52, 57},
                { 21, 26, 32, 37, 42, 48, 53, 56, 60},
                { 20, 25, 31, 36, 41, 47, 52, 55, 58},
                {  7, 10, 13, 16, 18, 21, 24, 25, 28},
                { 19, 24, 30, 35, 40, 46, 51, 54, 57},
                { 17, 22, 26, 31, 35, 40, 44, 46, 48}
            };
            int[,] heavyWeaponDamageTable =
            {
                { 26, 33, 40, 47, 54, 61, 68, 71, 74 },
                { 24, 31, 38, 45, 51, 58, 65, 68, 71 },
                { 13, 16, 20, 23, 26, 30, 33, 36, 38 },
                { 22, 29, 36, 43, 49, 56, 63, 66, 69 },
                { 25, 32, 39, 46, 52, 59, 66, 69, 72 },
                { 24, 31, 38, 45, 51, 58, 65, 68, 71 },
                { 12, 16, 19, 23, 26, 30, 33, 36, 38 },
                { 23, 30, 36, 43, 50, 56, 63, 66, 70 },
                { 20, 26, 31, 37, 43, 48, 54, 56, 59 }
            };
            int[,] twohandedWeaponDamageTable =
            {
                { 13, 17, 22, 26, 30, 35, 39, 42, 45 },
                { 14, 19, 23, 28, 33, 37, 42, 45, 48 }
            };

            int tieredDamage = 0;
            int finalDamage = 0;

            switch (weaponType)
            {
                case 1:
                    tieredDamage = heavyWeaponDamageTable[baseWeapon - 1, tier];
                    break;
                case 2:
                    tieredDamage = lightWeaponDamageTable[baseWeapon - 1, tier];
                    break;
                case 3:
                    tieredDamage = twohandedWeaponDamageTable[baseWeapon - 1, tier];
                    break;
            }

            float chanceOffset = 0f;
            double chance = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (wieldDiff > 0)
                chanceOffset = 0.02f;
            else if (wieldDiff > 250)
                chanceOffset = 0.03f;
            else if (wieldDiff > 300)
                chanceOffset = 0.04f;
            else if (wieldDiff > 325)
                chanceOffset = 0.05f;
            else if (wieldDiff > 350)
                chanceOffset = 0.06f;
            else if (wieldDiff > 370)
                chanceOffset = 0.07f;
            else if (wieldDiff > 400)
                chanceOffset = 0.08f;
            else if (wieldDiff > 420)
                chanceOffset = 0.1f;

            chance = chance + chanceOffset;
            if (tieredDamage < 10)
            {
                if (chance < .026)
                    finalDamage = tieredDamage - 3;
                else if (chance < .5)
                    finalDamage = tieredDamage - 2;
                else if (chance < .977)
                    finalDamage = tieredDamage - 1;
                else
                    finalDamage = tieredDamage;
            }
            else
            {
                if (chance < .005)
                    finalDamage = tieredDamage - 7;
                else if (chance < .026)
                    finalDamage = tieredDamage - 6;
                else if (chance < .162)
                    finalDamage = tieredDamage - 5;
                else if (chance < .5)
                    finalDamage = tieredDamage - 4;
                else if (chance < .841)
                    finalDamage = tieredDamage - 3;
                else if (chance < .977)
                    finalDamage = tieredDamage - 2;
                else if (chance < .995)
                    finalDamage = tieredDamage - 1;
                else
                    finalDamage = tieredDamage;
            }

            return finalDamage;
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
                    lowSpellTier = 5;
                    break;
                default:
                    lowSpellTier = 6;
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
                    highSpellTier = 6;
                    break;
                case 4:
                    highSpellTier = 6;
                    break;
                case 5:
                    highSpellTier = 7;
                    break;
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

        private static int GetValue(int tier, int work)
        {
            ///This is just a placeholder. This doesnt return a final value used retail, just a quick value for now.
            ///Will use, tier, material type, amount of gems set into item, type of gems, spells on item
            int value = ThreadSafeRandom.Next(1, tier) * ThreadSafeRandom.Next(1, tier) * ThreadSafeRandom.Next(1, work) * ThreadSafeRandom.Next(1, 250) + ThreadSafeRandom.Next(1, 50);

            return value;
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
                    maxmana = (ThreadSafeRandom.Next(100, 500) + spellAmount * ThreadSafeRandom.Next(1, 4)) * ThreadSafeRandom.Next(3, 9); //1-50
                    break;
                case 2:
                    maxmana = (ThreadSafeRandom.Next(400, 700) + spellAmount * ThreadSafeRandom.Next(1, 5)) * ThreadSafeRandom.Next(3, 9); //40-90
                    break;
                case 3:
                    maxmana = (ThreadSafeRandom.Next(700, 900) + spellAmount * ThreadSafeRandom.Next(1, 6)) * ThreadSafeRandom.Next(3, 9); //80 - 130
                    break;
                case 4:
                    maxmana = (ThreadSafeRandom.Next(1000, 1200) + spellAmount * ThreadSafeRandom.Next(1, 7)) * ThreadSafeRandom.Next(3, 9); /// 120 - 160
                    break;
                case 5:
                    maxmana = (ThreadSafeRandom.Next(1300, 1500) + spellAmount * ThreadSafeRandom.Next(1, 8)) * ThreadSafeRandom.Next(3, 9); ///150 - 210
                    break;
                case 6:
                    maxmana = (ThreadSafeRandom.Next(1600, 1800) + spellAmount * ThreadSafeRandom.Next(1, 9)) * ThreadSafeRandom.Next(3, 9); /// 200-260
                    break;
                case 7:
                    maxmana = (ThreadSafeRandom.Next(2300, 2600) + spellAmount * ThreadSafeRandom.Next(1, 10)) * ThreadSafeRandom.Next(3, 9); /// 250 - 310
                    break;
                case 8:
                    maxmana = (ThreadSafeRandom.Next(2800, 3000) + spellAmount * ThreadSafeRandom.Next(1, 11)) * ThreadSafeRandom.Next(3, 9); //300-450
                    break;
                default:
                    break;
            }

            return maxmana;
        }

        private static int GetMaterialType(int type, int tier)
        {
            uint materialType = 0;

            ///Type = 1 for gems, 2 for weapons/bows, 3 for armor, 4 jewelry
            uint[] gems5 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                            0x00000014, 0x00000026, 0x00000027, 0x00000015 };

            uint[] gems4 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029};

            uint[] gems3 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021};

            uint[] gems2 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023};

            uint[] gems1 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028};

            uint[] materialTypes5 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                     0x00000014, 0x00000026, 0x00000027, 0x00000015, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F};

            uint[] materialTypes4 = { 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                      0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                      0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                      0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F};

            uint[] materialTypes3 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040,
                                     0x0000003C, 0x0000003F};

            uint[] materialTypes2 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F};

            uint[] materialTypes1 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C,
                                     0x0000003F };

            uint[] bowTypes5 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                     0x00000014, 0x00000026, 0x00000027, 0x00000015, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F,
                                     0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes4 = { 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                      0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                      0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                      0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes3 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040,
                                     0x0000003C, 0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes2 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes1 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C,
                                     0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D, };
            uint[] metalWeaponsAll = { 0x0000003B, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000003E };

            uint[] leatherArmor = { 52, 53, 54, 55 };

            uint[] clothArmor = { 4, 5, 6, 7, 8 };

            uint[] metalArmor = { 0x0000003B, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000003E };

            uint[] metalAndLeatherArmor = { 52, 53, 54, 55, 0x0000003B, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000003E };

            ///Type = 1 for gems, 2 for weapons/bows, 3 for armor, 4 MetalWeapons, 5 jewelry, 
            switch (type)
            {
                case 1:
                    switch (tier)
                    {
                        case 1:
                            materialType = gems1[ThreadSafeRandom.Next(0, gems1.Length - 1)];
                            break;
                        case 2:
                            materialType = gems2[ThreadSafeRandom.Next(0, gems2.Length - 1)];
                            break;
                        case 3:
                            materialType = gems3[ThreadSafeRandom.Next(0, gems3.Length - 1)];
                            break;
                        case 4:
                            materialType = gems4[ThreadSafeRandom.Next(0, gems4.Length - 1)];
                            break;
                        default:
                            materialType = gems5[ThreadSafeRandom.Next(0, gems5.Length - 1)];
                            break;
                    }
                    break;
                case 2:
                    switch (tier)
                    {
                        case 1:
                            materialType = bowTypes1[ThreadSafeRandom.Next(0, bowTypes1.Length - 1)];
                            break;
                        case 2:
                            materialType = bowTypes2[ThreadSafeRandom.Next(0, bowTypes2.Length - 1)];
                            break;
                        case 3:
                            materialType = bowTypes3[ThreadSafeRandom.Next(0, bowTypes3.Length - 1)];
                            break;
                        case 4:
                            materialType = bowTypes4[ThreadSafeRandom.Next(0, bowTypes4.Length - 1)];
                            break;
                        default:
                            materialType = bowTypes5[ThreadSafeRandom.Next(0, bowTypes5.Length - 1)];
                            break;
                    }
                    break;
                case 3:
                    switch (tier)
                    {
                        case 1:
                            materialType = materialTypes1[ThreadSafeRandom.Next(0, materialTypes1.Length - 1)];
                            break;
                        case 2:
                            materialType = materialTypes2[ThreadSafeRandom.Next(0, materialTypes2.Length - 1)];
                            break;
                        case 3:
                            materialType = materialTypes3[ThreadSafeRandom.Next(0, materialTypes3.Length - 1)];
                            break;
                        case 4:
                            materialType = materialTypes4[ThreadSafeRandom.Next(0, materialTypes4.Length - 1)];
                            break;
                        default:
                            materialType = materialTypes5[ThreadSafeRandom.Next(0, materialTypes5.Length - 1)];
                            break;
                    }
                    break;
                case 4:
                    materialType = metalWeaponsAll[ThreadSafeRandom.Next(0, metalWeaponsAll.Length - 1)];
                    break;
                case 5:
                    materialType = leatherArmor[ThreadSafeRandom.Next(0, leatherArmor.Length - 1)];
                    break;
                case 6:
                    materialType = metalAndLeatherArmor[ThreadSafeRandom.Next(0, metalAndLeatherArmor.Length - 1)];
                    break;
                case 7:
                    materialType = metalArmor[ThreadSafeRandom.Next(0, metalArmor.Length - 1)];
                    break;
                case 8:
                    materialType = clothArmor[ThreadSafeRandom.Next(0, clothArmor.Length - 1)];
                    break;
                default:
                    break;

            }

            return (int)materialType;
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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

            int amount = 0;

            if (tier < 8)
                return amount;

            if (ThreadSafeRandom.Next(0, 1000) == 0)
                amount = 1;

            if (ThreadSafeRandom.Next(0, 5000) == 0)
                amount = 2;

            if (ThreadSafeRandom.Next(0, 1000000) == 0)
                amount = 3;

            if (ThreadSafeRandom.Next(0, 10000000) == 0)
                amount = 4;

            return amount;
        }

        private static int GetNumEpicCantrips(int tier)
        {
            int amount = 0;

            if (tier < 7)
                return amount;

            switch (tier)
            {
                case 7:
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 10000) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 100000) == 0)
                        amount = 3;
                    break;
                default:
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 10000) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 100000) == 0)
                        amount = 3;
                    break;

            }

            return amount;
        }

        private static int GetNumMajorCantrips(int tier)
        {

            int amount = 0;
            switch (tier)
            {
                case 1:
                    amount = 0;
                    break;
                case 2:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    break;
                case 3:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 10000) == 0)
                        amount = 2;
                    break;
                case 4:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 2;
                    break;
                case 5:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 2;
                    break;
                case 6:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 2;
                    break;
                case 7:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 15000) == 0)
                        amount = 3;
                    break;
                default:
                    if (ThreadSafeRandom.Next(0, 500) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 15000) == 0)
                        amount = 3;
                    break;
            }

            return amount;
        }

        private static int GetNumMinorCantrips(int tier)
        {

            int amount = 0;
            switch (tier)
            {
                case 1:
                    if (ThreadSafeRandom.Next(0, 100) == 0)
                        amount = 1;
                    break;
                case 2:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    break;
                case 3:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    break;
                case 4:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 3;
                    break;
                case 5:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 3;
                    break;
                case 6:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 3;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 4;
                    break;
                case 7:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 3;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 4;
                    break;
                default:
                    if (ThreadSafeRandom.Next(0, 50) == 0)
                        amount = 1;
                    if (ThreadSafeRandom.Next(0, 250) == 0)
                        amount = 2;
                    if (ThreadSafeRandom.Next(0, 1000) == 0)
                        amount = 3;
                    if (ThreadSafeRandom.Next(0, 5000) == 0)
                        amount = 4;
                    break;
            }

            return amount;
        }
    }
}
