using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateArmor(TreasureDeath profile, bool isMagical, bool isArmor, LootBias lootBias = LootBias.UnBiased)
        {
            var minType = LootTables.ArmorType.Helms;
            var maxType = new LootTables.ArmorType();

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
            if (isArmor == true)
                armorType = (LootTables.ArmorType)ThreadSafeRandom.Next((int)minType, (int)maxType);
            else
                armorType = LootTables.ArmorType.MiscClothing;

            int[] table = LootTables.GetLootTable(armorType);

            int rng = ThreadSafeRandom.Next(0, table.Length - 1);

            int armorWeenie = table[rng];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            wo.SetProperty(PropertyInt.AppraisalItemSkill, 7);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);

            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;

            int gemCount = ThreadSafeRandom.Next(1, 6);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);

            int workmanship = GetWorkmanship(profile.Tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            int wield;
            if (profile.Tier > 6 && armorType != LootTables.ArmorType.CovenantArmor)
            {
                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.Level);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)Skill.Axe);  // Set by examples from PCAP data

                wield = profile.Tier switch
                {
                    7 => 150,// In this instance, used for indicating player level, rather than skill level
                    _ => 180,// In this instance, used for indicating player level, rather than skill level
                };

                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }

            if (armorType == LootTables.ArmorType.CovenantArmor)
            {
                int chance = ThreadSafeRandom.Next(1, 3);
                var wieldSkill = chance switch
                {
                    // Magic Def
                    1 => Skill.MagicDefense,
                    // Missile Def
                    2 => Skill.MissileDefense,
                    // Melee Def
                    _ => Skill.MeleeDefense,
                };
                wield = GetCovenantWieldReq(profile.Tier, wieldSkill);

                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.RawSkill);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)wieldSkill);
                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }

            // Setting random color
            wo.SetProperty(PropertyInt.PaletteTemplate, ThreadSafeRandom.Next(1, 2047));
            double shade = .1 * ThreadSafeRandom.Next(0, 9);
            wo.SetProperty(PropertyFloat.Shade, shade);

            wo = AssignArmorLevel(wo, profile.Tier, armorType);

            wo = AssignEquipmentSetId(wo, profile);

            if (isMagical)
            {
                bool covenantArmor = false || (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor);
                wo = AssignMagic(wo, profile, covenantArmor);
            }
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            wo = RandomizeColor(wo);

            return wo;
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
        /// <param name="wo"></param>
        /// <param name="tier"></param>
        /// <param name="armorType"></param>
        /// <returns></returns>
        private static WorldObject AssignArmorLevel(WorldObject wo, int tier, LootTables.ArmorType armorType)
        {
            if (wo.ArmorType == null)
            {
                log.Warn($"[LOOT] Missing PropertyInt.ArmorType on loot item {wo.WeenieClassId} - {wo.Name}");
                return wo;
            }

            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

            if (((wo.ClothingPriority & (CoverageMask)CoverageMaskHelper.Underwear) == 0) || wo.IsShield)
            {
                int armorModValue = 0;

                // Account for ACE World Databases that have not yet been updated
                if (wo.ArmorType != (int)ArmorType.Cloth && (wo.GetProperty(PropertyInt.Version) == null || wo.GetProperty(PropertyInt.Version) < 3))
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
                wo.SetProperty(PropertyInt.ArmorLevel, adjustedArmorLevel);
            }

            if ((wo.ResistMagic == null || wo.ResistMagic < 9999) && wo.GetProperty(PropertyInt.ArmorLevel) >= 345)
                log.Warn($"[LOOT] Standard armor item exceeding upper AL threshold {wo.WeenieClassId} - {wo.Name}");

            return wo;
        }

        private static WorldObject AssignArmorLevelCompat(WorldObject wo, int tier, LootTables.ArmorType armorType)
        {
            log.Debug($"[LOOT] Using AL Assignment Compatibility layer for item {wo.WeenieClassId} - {wo.Name}.");

            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

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

                            else if (armorType == LootTables.ArmorType.CovenantArmor)
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

                            else if (armorType == LootTables.ArmorType.CovenantArmor)
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

                            else if (armorType == LootTables.ArmorType.CovenantArmor)
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

                            else if (armorType == LootTables.ArmorType.CovenantArmor)
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

                            else if (armorType == LootTables.ArmorType.CovenantArmor)
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

                            else if (armorType == LootTables.ArmorType.CovenantArmor)
                                armorModValue = ThreadSafeRandom.Next(290, 330);

                            else
                                armorModValue = ThreadSafeRandom.Next(280, 320);
                            break;
                        default:
                            armorModValue = 0;
                            break;
                    }
                }

                int adjustedArmorLevel = baseArmorLevel + armorModValue;
                wo.SetProperty(PropertyInt.ArmorLevel, adjustedArmorLevel);
            }

            return wo;
        }
    }
}
