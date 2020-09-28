using System;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateArmor(TreasureDeath profile, bool isMagical, bool isArmor, LootBias lootBias = LootBias.UnBiased, bool mutate = true)
        {
            var minType = LootTables.ArmorType.Helms;
            LootTables.ArmorType maxType;

            switch (profile.Tier)
            {
                case 1:
                default:
                    maxType = LootTables.ArmorType.ChainmailArmor;
                    break;
                case 2:
                    maxType = LootTables.ArmorType.DiforsaArmor;
                    break;
                case 3:
                case 4:
                    maxType = LootTables.ArmorType.CovenantArmor;
                    break;
                case 5:
                    maxType = LootTables.ArmorType.AlduressaArmor;
                    break;
                case 6:
                    maxType = LootTables.ArmorType.HaebreanArmor;
                    break;
                case 7:
                case 8:
                    maxType = LootTables.ArmorType.OlthoiAlduressaArmor;
                    break;
            }

            // Added for making clothing drops their own drop, and not involved in armor roll chance
            LootTables.ArmorType armorType;
            if (isArmor)
                armorType = (LootTables.ArmorType)ThreadSafeRandom.Next((int)minType, (int)maxType);
            else
                armorType = LootTables.ArmorType.MiscClothing;

            int[] table = LootTables.GetLootTable(armorType);

            int rng = ThreadSafeRandom.Next(0, table.Length - 1);

            int armorWeenie = table[rng];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);

            if (wo != null && mutate)
                MutateArmor(wo, profile, isMagical, armorType);

            return wo;
        }

        private static WorldObject CreateSocietyArmor(TreasureDeath profile, bool mutate)
        {

            int society = 0;
            int armortype = 0;

            if (profile.TreasureType >= 2971 && profile.TreasureType <= 2980)
                society = 0; // CH
            else if (profile.TreasureType >= 2981 && profile.TreasureType <= 2990)
                society = 1; // EW
            else if (profile.TreasureType >= 2991 && profile.TreasureType <= 3000)
                society = 2; // RB

            switch (profile.TreasureType)
            {
                case 2971:
                case 2981:
                case 2991:
                    armortype = 0; // BP
                    break;
                case 2972:
                case 2982:
                case 2992:
                    armortype = 1; // Gauntlets
                    break;
                case 2973:
                case 2983:
                case 2993:
                    armortype = 2; // Girth
                    break;
                case 2974:
                case 2984:
                case 2994:
                    armortype = 3; // Greaves
                    break;
                case 2975:
                case 2985:
                case 2995:
                    armortype = 4; // Helm
                    break;
                case 2976:
                case 2986:
                case 2996:
                    armortype = 5; // Pauldrons
                    break;
                case 2977:
                case 2987:
                case 2997:
                    armortype = 6; // Tassets
                    break;
                case 2978:
                case 2988:
                case 2998:
                    armortype = 7; // Vambraces
                    break;
                case 2979:
                case 2989:
                case 2999:
                    armortype = 8; // Sollerets
                    break;
                default:
                    break;
            }

            int societyArmorWeenie = LootTables.SocietyArmorMatrix[armortype][society];
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)societyArmorWeenie);

            if (wo != null && mutate)
            MutateSocietyArmor(wo, profile, true);

            return wo;
        }

        private static void MutateArmor(WorldObject wo, TreasureDeath profile, bool isMagical, LootTables.ArmorType armorType)
        {
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;

            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 6);

            wo.GemType = RollGemType(profile.Tier);

            int workmanship = GetWorkmanship(profile.Tier);
            wo.ItemWorkmanship = workmanship;

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            int wield;
            if (profile.Tier > 6 && armorType != LootTables.ArmorType.CovenantArmor && armorType != LootTables.ArmorType.OlthoiArmor)
            {
                wo.WieldRequirements = WieldRequirement.Level;
                wo.WieldSkillType = (int)Skill.Axe;  // Set by examples from PCAP data

                wield = profile.Tier switch
                {
                    7 => 150,// In this instance, used for indicating player level, rather than skill level
                    _ => 180,// In this instance, used for indicating player level, rather than skill level
                };

                wo.WieldDifficulty = wield;
            }

            if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
            {
                int chance = ThreadSafeRandom.Next(1, 3);
                var wieldSkill = chance switch
                {
                    1 => Skill.MagicDefense,
                    2 => Skill.MissileDefense,
                    _ => Skill.MeleeDefense,
                };
                wield = GetCovenantWieldReq(profile.Tier, wieldSkill);

                wo.WieldRequirements = WieldRequirement.RawSkill;
                wo.WieldSkillType = (int)wieldSkill;
                wo.WieldDifficulty = wield;

                // used by tinkering requirements for copper/silver
                wo.ItemSkillLimit = (uint)wieldSkill;
            }

            // Setting random color
            wo.PaletteTemplate = ThreadSafeRandom.Next(1, 2047);
            wo.Shade = .1 * ThreadSafeRandom.Next(0, 9);

            wo = AssignArmorLevel(wo, profile.Tier, armorType);

            wo = AssignEquipmentSetId(wo, profile);

            if (isMagical)
            {
                //bool covenantArmor = false || (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor);
                AssignMagic(wo, profile, true);
            }
            else
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
            }

            wo.LongDesc = GetLongDesc(wo);

            //wo.AppraisalItemSkill = 7;
            //wo.AppraisalLongDescDecoration = AppraisalLongDescDecorations.PrependWorkmanship;

            // try mutate burden, if MutateFilter exists
            if (wo.HasMutateFilter(MutateFilter.EncumbranceVal))
                MutateBurden(wo, profile, false);

            MutateColor(wo);
        }


        private static void MutateSocietyArmor(WorldObject wo, TreasureDeath profile, bool isMagical)
        {
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;

            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 6);

            wo.GemType = RollGemType(profile.Tier);

            int workmanship = GetWorkmanship(profile.Tier);
            wo.ItemWorkmanship = workmanship;

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            // wo.WieldSkillType = (int)Skill.Axe;  // Set by examples from PCAP data

            if (isMagical)
            {
                // looks like society armor always had impen on it
                AssignMagic(wo, profile, true);
            }
            else
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
            }
            wo = AssignArmorLevel(wo, profile.Tier, LootTables.ArmorType.SocietyArmor);

            wo.LongDesc = GetLongDesc(wo);

            // try mutate burden, if MutateFilter exists
            if (wo.HasMutateFilter(MutateFilter.EncumbranceVal))
                MutateBurden(wo, profile, false);
        }

        private static bool GetMutateArmorData(uint wcid, out LootTables.ArmorType? armorType)
        {
            foreach (var kvp in LootTables.armorTypeMap)
            {
                armorType = kvp.Key;
                var table = kvp.Value;

                if (kvp.Value.Contains((int)wcid))
                    return true;
            }
            armorType = null;
            return false;
        }

        private static int GetCovenantWieldReq(int tier, Skill skill)
        {
            var index = tier switch
            {
                3 => ThreadSafeRandom.Next(1, 3),
                4 => ThreadSafeRandom.Next(1, 4),
                5 => ThreadSafeRandom.Next(1, 5),
                6 => ThreadSafeRandom.Next(1, 6),
                7 => ThreadSafeRandom.Next(1, 7),
                _ => ThreadSafeRandom.Next(1, 8),
            };

            var wield = skill switch
            {
                Skill.MagicDefense => index switch
                {
                    1 => 145,
                    2 => 185,
                    3 => 225,
                    4 => 245,
                    5 => 270,
                    6 => 290,
                    7 => 310,
                    _ => 320,
                },
                Skill.MissileDefense => index switch
                {
                    1 => 160,
                    2 => 205,
                    3 => 245,
                    4 => 270,
                    5 => 290,
                    6 => 305,
                    7 => 330,
                    _ => 340,
                },
                _ => index switch
                {
                    1 => 200,
                    2 => 250,
                    3 => 300,
                    4 => 325,
                    5 => 350,
                    6 => 370,
                    7 => 400,
                    _ => 410,
                },
            };
            return wield;
        }

        private static WorldObject AssignEquipmentSetId(WorldObject wo, TreasureDeath profile)
        {
            EquipmentSet equipSetId = EquipmentSet.Invalid;

            // Last condition included to prevent equipment set Ids being added to armor weenies
            // that would be assigned AL via AssignArmorLevelCompat()
            if (PropertyManager.GetBool("equipmentsetid_enabled").Item
                && wo.ClothingPriority != (CoverageMask)CoverageMaskHelper.Underwear && !wo.IsShield && profile.Tier > 6
                && (wo.GetProperty(PropertyInt.Version) ?? 0) >= 3)
            {
                if (wo.WieldRequirements == WieldRequirement.Level || wo.WieldRequirements == WieldRequirement.RawSkill)
                {
                    double dropRate = PropertyManager.GetDouble("equipmentsetid_drop_rate").Item;
                    double dropRateMod = 1.0 / dropRate;

                    double lootQualityMod = 1.0f;
                    if (PropertyManager.GetBool("loot_quality_mod").Item && profile.LootQualityMod > 0 && profile.LootQualityMod < 1)
                        lootQualityMod = 1.0f - profile.LootQualityMod;

                    // Initial base 10% chance to add a random EquipmentSetID, which can be adjusted via property mod
                    int chance = ThreadSafeRandom.Next(1, (int)(100 * dropRateMod * lootQualityMod));
                    if (chance < 11)
                    {
                        equipSetId = (EquipmentSet)ThreadSafeRandom.Next((int)EquipmentSet.Soldiers, (int)EquipmentSet.Lightningproof);

                        wo.EquipmentSetId = equipSetId;

                        if (PropertyManager.GetBool("equipmentsetid_name_decoration").Item)
                        {
                            string name = equipSetId.ToString();

                            if (equipSetId >= EquipmentSet.Soldiers && equipSetId <= EquipmentSet.Crafters)
                                name = name.TrimEnd('s') + "'s";

                            wo.Name = string.Join(" ", name, wo.Name);
                        }
                    }
                }
            }

            return wo;
        }

        /// <summary>
        /// Assign a final AL value based upon tier
        /// Used values given at https://asheron.fandom.com/wiki/Loot#Armor_Levels for setting the AL mod values
        /// so as to not exceed the values listed in that table
        /// </summary>
        private static WorldObject AssignArmorLevel(WorldObject wo, int tier, LootTables.ArmorType armorType)
        {
            if (wo.ArmorType == null)
            {
                log.Warn($"[LOOT] Missing PropertyInt.ArmorType on loot item {wo.WeenieClassId} - {wo.Name}");
                return wo;
            }

            var baseArmorLevel = wo.ArmorLevel ?? 0;

            if (((wo.ClothingPriority & (CoverageMask)CoverageMaskHelper.Underwear) == 0) || wo.IsShield)
            {
                int armorModValue = 0;

                // Account for ACE World Databases that have not yet been updated
                if (wo.ArmorType != (int)ArmorType.Cloth && (wo.GetProperty(PropertyInt.Version) ?? 0) < 3)
                    return AssignArmorLevelCompat(wo, tier, armorType);

                // Sets AL variations based on weenie ArmorType field, such as cloth, leather, metal, etc.
                switch (tier)
                {
                    case 1:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(0, 25);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(0, 29);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(0, 26);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        // Does not drop at this tier level
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = 0;
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(5, 22);
                        break;
                    case 2:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(25, 50);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(29, 52);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(26, 50);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        // Does not drop at this tier level
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = 0;
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(22, 39);
                        break;
                    case 3:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(50, 75);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(52, 75);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(50, 74);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(190, 210);
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(39, 56);
                        break;
                    case 4:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(75, 100);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(75, 98);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(74, 98);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(210, 230);
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(56, 73);
                        break;
                    case 5:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(100, 125);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(98, 121);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(98, 122);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(230, 250);
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(73, 90);
                        break;
                    case 6:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(125, 150);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(121, 144);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(122, 146);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(250, 270);
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(90, 107);
                        break;
                    case 7:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(150, 175);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(144, 167);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(146, 170);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(270, 290);
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(107, 124);
                        break;
                    case 8:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(175, 200);
                        if (wo.ArmorType == (int)ArmorType.Leather
                            || wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(167, 190);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(170, 194);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(290, 310);
                        if (wo.IsShield)
                            armorModValue = ThreadSafeRandom.Next(124, 141);
                        break;
                }

                int adjustedArmorLevel = baseArmorLevel + armorModValue;
                wo.ArmorLevel = adjustedArmorLevel;
            }

            if ((wo.ResistMagic == null || wo.ResistMagic < 9999) && wo.ArmorLevel >= 345)
                log.Warn($"[LOOT] Standard armor item exceeding upper AL threshold {wo.WeenieClassId} - {wo.Name}");

            return wo;
        }

        private static WorldObject AssignArmorLevelCompat(WorldObject wo, int tier, LootTables.ArmorType armorType)
        {
            log.Debug($"[LOOT] Using AL Assignment Compatibility layer for item {wo.WeenieClassId} - {wo.Name}.");

            var baseArmorLevel = wo.ArmorLevel ?? 0;

            if (baseArmorLevel > 0)
            {
                int armorModValue = 0;

                if (armorType > LootTables.ArmorType.HaebreanArmor && armorType <= LootTables.ArmorType.OlthoiAlduressaArmor)
                {
                    // Even if most are not using T8, made a change to that outcome to ensure that Olthoi Alduressa doesn't go way out of spec
                    // Side effect is that Haebrean to Olthoi Celdon may suffer
                    armorModValue = tier switch
                    {
                        7 => ThreadSafeRandom.Next(0, 40),
                        8 => ThreadSafeRandom.Next(91, 115),
                        _ => 0,
                    };
                }
                else
                {
                    switch (tier)
                    {
                        case 1:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(0, 27);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(0, 23);

                            else
                                armorModValue = ThreadSafeRandom.Next(0, 40);
                            break;
                        case 2:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(27, 54);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(23, 46);

                            else
                                armorModValue = ThreadSafeRandom.Next(40, 80);
                            break;
                        case 3:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(54, 81);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(46, 69);

                            else if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
                                armorModValue = ThreadSafeRandom.Next(90, 130);

                            else
                                armorModValue = ThreadSafeRandom.Next(80, 120);
                            break;
                        case 4:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(81, 108);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(69, 92);

                            else if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
                                armorModValue = ThreadSafeRandom.Next(130, 170);

                            else
                                armorModValue = ThreadSafeRandom.Next(120, 160);
                            break;
                        case 5:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(108, 135);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(92, 115);

                            else if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
                                armorModValue = ThreadSafeRandom.Next(170, 210);

                            else
                                armorModValue = ThreadSafeRandom.Next(160, 200);
                            break;
                        case 6:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(135, 162);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(115, 138);

                            else if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
                                armorModValue = ThreadSafeRandom.Next(210, 250);

                            else
                                armorModValue = ThreadSafeRandom.Next(200, 240);
                            break;
                        case 7:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(162, 189);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                  || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(138, 161);

                            else if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
                                armorModValue = ThreadSafeRandom.Next(250, 290);

                            else
                                armorModValue = ThreadSafeRandom.Next(240, 280);
                            break;
                        case 8:
                            if (armorType == LootTables.ArmorType.StuddedLeatherArmor
                             || armorType == LootTables.ArmorType.Helms
                             || armorType == LootTables.ArmorType.Shields)
                                armorModValue = ThreadSafeRandom.Next(189, 216);

                            else if (armorType == LootTables.ArmorType.LeatherArmor
                                || armorType == LootTables.ArmorType.MiscClothing)
                                armorModValue = ThreadSafeRandom.Next(161, 184);

                            else if (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor)
                                armorModValue = ThreadSafeRandom.Next(290, 330);

                            else if (armorType == LootTables.ArmorType.SocietyArmor)
                                armorModValue = ThreadSafeRandom.Next(189, 216);
                            else
                                armorModValue = ThreadSafeRandom.Next(280, 320);
                            break;
                        default:
                            armorModValue = 0;
                            break;
                    }
                }

                int adjustedArmorLevel = baseArmorLevel + armorModValue;
                wo.ArmorLevel = adjustedArmorLevel;
            }

            return wo;
        }
        private static WorldObject CreateCloak(TreasureDeath profile, bool mutate = true)
        {
            int cloakWeenie;
            int cloakType = ThreadSafeRandom.Next(0, 10);  // 11 different types of Cloaks

            cloakWeenie = LootTables.Cloaks[cloakType];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)cloakWeenie);

            if (wo != null && mutate)
                MutateCloak(wo, profile);

            return wo;
        }
        private static void MutateCloak(WorldObject wo, TreasureDeath profile)
        {
            const uint cloakIconOverlayOne = 100690996;
            const uint cloakIconOverlayTwo = 100690997;
            const uint cloakIconOverlayThree = 100690998;
            const uint cloakIconOverlayFour = 100690999;
            const uint cloakIconOverlayFive = 100691000;

            EquipmentSet equipSetId = EquipmentSet.Invalid;

            int cloakSpellId;

            // Workmanship - This really doesn't matter, so not making a big fuss about it.
            wo.Workmanship = ThreadSafeRandom.Next(1, 10);

            // Value
            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            wo.Value = GetValue(profile.Tier, (int)wo.Workmanship, gemMaterialMod, materialMod);

            // Level and Icons
            wo.ItemMaxLevel = GetCloakMaxLevel(profile);

            if (wo.ItemMaxLevel == 1)
                wo.IconOverlayId = cloakIconOverlayOne;
            else if (wo.ItemMaxLevel == 2)
                wo.IconOverlayId = cloakIconOverlayTwo;
            else if (wo.ItemMaxLevel == 3)
                wo.IconOverlayId = cloakIconOverlayThree;
            else if (wo.ItemMaxLevel == 4)
                wo.IconOverlayId = cloakIconOverlayFour;
            else if (wo.ItemMaxLevel == 5)
                wo.IconOverlayId = cloakIconOverlayFive;

            // Wield Diff
            // Notes - There is a lvl 180 cloak, but that cloak req has Damage Ratings and is T8 only.  Damage ratings on items are done at this time.
            if (wo.ItemMaxLevel == 1)
                wo.WieldDifficulty = 30;
            else if (wo.ItemMaxLevel == 2)
                wo.WieldDifficulty = 60;
            else if (wo.ItemMaxLevel == 3)
                wo.WieldDifficulty = 90;
            else if (wo.ItemMaxLevel == 4)
                wo.WieldDifficulty = 120;
            else if (wo.ItemMaxLevel == 5)
                wo.WieldDifficulty = 150;

            // Cloak Set - Doing it this way because the Equipment Set Enum has MoA values duplicates (which would slant the ratios/drops to Light,Missile,Heavy and Finesse Spells
            int cloakSetValue = ThreadSafeRandom.Next(0, 34);  // 35 different types of Cloak Sets
            int cloakSet = LootTables.CloakSets[cloakSetValue];
            equipSetId = (EquipmentSet)cloakSet;
            wo.EquipmentSetId = equipSetId;

            // Surge Spells
            int spellRoll = ThreadSafeRandom.Next(0, 12);
            if (spellRoll == 12)
            {
                // This is for the one "surge" that is not a spell.  Its a property set for the 
                wo.CloakWeaveProc = 2;
            }
            else
            {
                cloakSpellId = LootTables.CloakSpells[spellRoll];
                AssignCloakSpells(wo, cloakSpellId);               
                if (cloakSpellId == 5753)  // Cloaked in Skill is the only Self Targeted Spell
                    wo.ProcSpellSelfTargeted = true;
                else
                    wo.ProcSpellSelfTargeted = false;
                wo.CloakWeaveProc = 1;
            }
        }
        private static int GetCloakMaxLevel(TreasureDeath profile)
        {
            //  These Values are just for starting off.  I haven't gotten the numbers yet to confirm these.

            int cloakLevel = 1;
            int chance = 0;

            chance = ThreadSafeRandom.Next(1, 1000);
            switch (profile.Tier)
            {
                case 1:
                case 2:
                default:                
                    cloakLevel = 1;
                    break;
                case 3:
                case 4:
                    if (chance <= 440)
                        cloakLevel = 1;
                    else
                        cloakLevel = 2;
                    break;
                case 5:
                    if (chance <= 250)
                        cloakLevel = 1;
                    else if (chance <= 700)
                        cloakLevel = 2;
                    else
                        cloakLevel = 3;
                    break;
                case 6:
                    if (chance <= 36)
                        cloakLevel = 1;
                    else if (chance <= 357)
                        cloakLevel = 2;
                    else if (chance <= 990)
                        cloakLevel = 3;
                    else
                        cloakLevel = 4;
                    break;
                case 7:  // From data, no chance to get a lvl 1 cloak
                    if (chance <= 463)
                        cloakLevel = 2;
                    else if (chance <= 945)
                        cloakLevel = 3;
                    else if (chance <= 984)
                        cloakLevel = 4;
                    else
                        cloakLevel = 5;
                    break;
                case 8:  // From data, no chance to get a lvl 1 cloak
                    if (chance <= 451)
                        cloakLevel = 2;
                    else if (chance <= 920)
                        cloakLevel = 3;
                    else if (chance <= 975)
                        cloakLevel = 4;
                    else
                        cloakLevel = 5;
                    break;
            }
            return cloakLevel;
        }
        private static bool GetMutateCloakData(uint wcid)
        {
            return LootTables.Cloaks.Contains((int)wcid);
        }
    }
}
