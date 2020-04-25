using ACE.Common;
using ACE.Database.Models.World;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateSummoningEssence(int tier)
        {
            uint id = 0;

            // Adding a spread of Pet Device levels for each tier - Level 200 pets should only be dropping in T8 Loot - HQ 2/29/2020
            // The spread is from Optim's Data
            // T5-T8 20/35/30/15% split 
            // T8- 200,180,150,125
            // T7- 180,150,125,100
            // T6- 150,125,100,80
            // T5- 125,100,80,50
            // T4- 100,80,50
            // T3- 80,50
            // T2- 50
            // T1- 50

            // Tables are already 1-7, so removing them being Tier dependent

            int petLevel = 0;
            int chance = ThreadSafeRandom.Next(1, 100);
            if (chance > 80)
                petLevel = tier - 1;
            else if (chance > 45)
                petLevel = tier - 2;
            else if (chance > 15)
                petLevel = tier - 3;
            else
                petLevel = tier - 4;
            if (petLevel < 2)
                petLevel = 1;

            int summoningEssenceIndex = ThreadSafeRandom.Next(0, LootTables.SummoningEssencesMatrix.Length - 1);

            id = (uint)LootTables.SummoningEssencesMatrix[summoningEssenceIndex][petLevel - 1];

            var petDevice = WorldObjectFactory.CreateNewWorldObject(id) as PetDevice;

            if (petDevice == null)
                return null;

            var ratingChance = 0.5f;

            // add rng ratings to pet device
            // linear or biased?
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamage = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamageResist = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamage = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamageResist = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCrit = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritResist = GeneratePetDeviceRating(tier);

            petDevice.ItemWorkmanship = GetWorkmanship(tier);

            return petDevice;
        }

        public static int GeneratePetDeviceRating(int tier)
        {
            // thanks to morosity for this formula!
            var baseRating = ThreadSafeRandom.Next(1, 10);
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var tierMod = 0.4f + tier * 0.02f;
            if (rng > tierMod)
                baseRating += ThreadSafeRandom.Next(1, 10);

            return baseRating;
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

            if (tier == 7)
            {
                // According to wiki, Tier 7 has a chance for level 8 spell components or level 7 spell scrolls
                // No indication of weighting in either direction, so assuming a 50/50 split
                int chance = ThreadSafeRandom.Next(1, 100);
                if (chance > 50)
                {
                    int id = CreateLevel8SpellComp();
                    wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                    return wo;
                }
            }

            if (tier < 1) tier = 1;

            int scrollLootMatrixIndex = tier - 1;
            int minSpellLevel = LootTables.ScrollLootMatrix[scrollLootMatrixIndex][0];
            int maxSpellLevel = LootTables.ScrollLootMatrix[scrollLootMatrixIndex][1];

            int scrollLootIndex = ThreadSafeRandom.Next(minSpellLevel, maxSpellLevel);
            uint spellID = 0;

            while (spellID == 0)
                spellID = (uint)LootTables.ScrollSpells[ThreadSafeRandom.Next(0, LootTables.ScrollSpells.Length - 1)][scrollLootIndex];

            var weenie = DatabaseManager.World.GetScrollWeenie(spellID);
            if (weenie == null)
            {
                log.DebugFormat("CreateRandomScroll for tier {0} and spellID of {1} returned null from the database.", tier, spellID);
                return null;
            }

            wo = WorldObjectFactory.CreateNewWorldObject(weenie.WeenieClassId);
            return wo;
        }

        private static int CreateLevel8SpellComp()
        {
            int upperLimit = LootTables.Level8SpellComps.Length - 1;
            int chance = ThreadSafeRandom.Next(0, upperLimit);

            return LootTables.Level8SpellComps[chance];
        }

        /// <summary>
        /// Creates Caster (Wand, Staff, Orb)
        /// </summary>
        public static WorldObject CreateCaster(TreasureDeath profile, bool isMagical, int wield = -1, bool forceWar = false)
        {
            // Refactored 11/20/19  - HarliQ

            int casterWeenie = 0;
            double elementalDamageMod = 0;
            Skill wieldSkillType = Skill.None;
            WieldRequirement wieldRequirement = WieldRequirement.RawSkill;
            int subType = 0;
            if (wield == -1)
                wield = GetWieldDifficulty(profile.Tier, WieldType.Caster);

            // Getting the caster Weenie needed.
            if (wield == 0)
            {
                // Determine plain caster type: 0 - Orb, 1 - Sceptre, 2 - Staff, 3 - Wand
                subType = ThreadSafeRandom.Next(0, 3);
                casterWeenie = LootTables.CasterWeaponsMatrix[wield][subType];

                if (profile.Tier > 6)
                {
                    wieldRequirement = WieldRequirement.Level;
                    wieldSkillType = Skill.Axe;  // Set by examples from PCAP data

                    wield = profile.Tier switch
                    {
                        7 => 150,// In this instance, used for indicating player level, rather than skill level
                        _ => 180,// In this instance, used for indicating player level, rather than skill level
                    };
                }
            }
            else
            {
                // Determine the Elemental Damage Mod amount
                elementalDamageMod = DetermineElementMod(wield);

                // Determine caster type: 1 - Sceptre, 2 - Baton, 3 - Staff
                int casterType = ThreadSafeRandom.Next(1, 3);

                // Determine element type: 0 - Slashing, 1 - Piercing, 2 - Blunt, 3 - Frost, 4 - Fire, 5 - Acid, 6 - Electric, 7 - Nether
                int element = forceWar ? ThreadSafeRandom.Next(0, 6) : ThreadSafeRandom.Next(0, 7);
                casterWeenie = LootTables.CasterWeaponsMatrix[casterType][element];

                // If element is Nether, Void Magic is required, else War Magic is required for all other elements
                if (element == 7)
                    wieldSkillType = Skill.VoidMagic;
                else
                    wieldSkillType = Skill.WarMagic;
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)casterWeenie);

            // Why is this here?  Should not get a null object
            if (wo == null)
                return null;

            // Setting MagicD and MissileD Bonuses to null (some weenies have a value)
            wo.WeaponMagicDefense = null;
            wo.WeaponMissileDefense = null;
            // Not sure why this is here, guessing some wienies have it by default
            wo.ItemSkillLevelLimit = null;

            // Setting general traits of weapon
            wo.ItemWorkmanship = GetWorkmanship(profile.Tier);

            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;
            wo.GemCount = ThreadSafeRandom.Next(1, 5);
            wo.GemType = (MaterialType)ThreadSafeRandom.Next(10, 50);
            wo.Value = GetValue(profile.Tier, wo.ItemWorkmanship.Value, LootTables.getMaterialValueModifier(wo), LootTables.getGemMaterialValueModifier(wo));
            // Is this right??
            wo.LongDesc = wo.Name;

            // Setting Weapon defensive mods 
            wo.WeaponDefense = GetWieldReqMeleeDMod(wield);
            wo.WeaponMagicDefense = GetMagicMissileDMod(profile.Tier);
            wo.WeaponMissileDefense = GetMagicMissileDMod(profile.Tier);

            // Setting weapon Offensive Mods
            if (elementalDamageMod > 1.0f)
                wo.ElementalDamageMod = elementalDamageMod;

            // Setting Wield Reqs for weapon
            if (wield > 0 || wieldRequirement == WieldRequirement.Level)
            {
                wo.WieldRequirements = wieldRequirement;
                wo.WieldSkillType = (int)wieldSkillType;
                wo.WieldDifficulty = wield;
            }
            else
            {
                wo.WieldRequirements = WieldRequirement.Invalid;
                wo.WieldSkillType = null;
                wo.WieldDifficulty = null;
            }

            // Adjusting Properties if weapon has magic (spells)
            double manaConMod = GetManaCMod(profile.Tier);
            if (manaConMod > 0.0f)
                wo.ManaConversionMod = manaConMod;

            if (isMagical)
                wo = AssignMagic(wo, profile);
            else
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
            }

            wo = RandomizeColor(wo);

            return wo;
        }

        private static double DetermineElementMod(int wield)
        {
            double elementBonus = 0;

            int chance = ThreadSafeRandom.Next(1, 100);
            switch (wield)
            {
                case 290:
                    if (chance > 95)
                        elementBonus = 0.03;
                    else if (chance > 65)
                        elementBonus = 0.02;
                    else
                        elementBonus = 0.01;
                    break;
                case 310:
                    if (chance > 95)
                        elementBonus = 0.06;
                    else if (chance > 65)
                        elementBonus = 0.05;
                    else
                        elementBonus = 0.04;
                    break;

                case 330:
                    if (chance > 95)
                        elementBonus = 0.09;
                    else if (chance > 65)
                        elementBonus = 0.08;
                    else
                        elementBonus = 0.07;
                    break;

                case 355:
                    if (chance > 95)
                        elementBonus = 0.13;
                    else if (chance > 80)
                        elementBonus = 0.12;
                    else if (chance > 55)
                        elementBonus = 0.11;
                    else if (chance > 20)
                        elementBonus = 0.10;
                    else
                        elementBonus = 0.09;
                    break;

                case 375:
                    if (chance > 95)
                        elementBonus = 0.16;
                    else if (chance > 85)
                        elementBonus = 0.15;
                    else if (chance > 60)
                        elementBonus = 0.14;
                    else if (chance > 30)
                        elementBonus = 0.13;
                    else if (chance > 10)
                        elementBonus = 0.12;
                    else
                        elementBonus = 0.11;
                    break;

                default:
                    // 385
                    if (chance > 95)
                        elementBonus = 0.18;
                    else if (chance > 65)
                        elementBonus = 0.17;
                    else
                        elementBonus = 0.16;
                    break;
            }

            elementBonus += 1;

            return elementBonus;
        }
    }
}
