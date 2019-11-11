using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateArmor(int tier, bool isMagical, LootBias lootBias = LootBias.UnBiased)
        {
            var minType = LootTables.ArmorType.MiscClothing;
            var maxType = new LootTables.ArmorType();

            switch (tier)
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

                    // armor Mana Forge Chests don't include clothing type items
                    if (lootBias == LootBias.Armor)
                        minType = LootTables.ArmorType.Helms;
                    break;
            }

            var armorType = (LootTables.ArmorType)ThreadSafeRandom.Next((int)minType, (int)maxType);

            int[] table = LootTables.GetLootTable(armorType);

            int rng = ThreadSafeRandom.Next(0, table.Length - 1);

            int armorWeenie = table[rng];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            wo.SetProperty(PropertyInt.AppraisalItemSkill, 7);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);

            int materialType = GetMaterialType(wo, tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;

            int gemCount = ThreadSafeRandom.Next(1, 6);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            int wield;
            if (tier > 6 && armorType != LootTables.ArmorType.CovenantArmor)
            {
                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.Level);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)Skill.Axe);  // Set by examples from PCAP data

                switch (tier)
                {
                    case 7:
                        wield = 150; // In this instance, used for indicating player level, rather than skill level
                        break;
                    default:
                        wield = 180; // In this instance, used for indicating player level, rather than skill level
                        break;
                }

                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }

            if (armorType == LootTables.ArmorType.CovenantArmor)
            {
                Skill wieldSkill;

                int chance = ThreadSafeRandom.Next(1, 3);
                switch (chance)
                {
                    case 1: // Magic Def
                        wieldSkill = Skill.MagicDefense;
                        break;
                    case 2: // Missile Def
                        wieldSkill = Skill.MissileDefense;
                        break;
                    default: // Melee Def
                        wieldSkill = Skill.MeleeDefense;
                        break;
                }

                wield = GetCovenantWieldReq(tier, wieldSkill);

                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.RawSkill);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)wieldSkill);
                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }

            // Setting random color
            wo.SetProperty(PropertyInt.PaletteTemplate, ThreadSafeRandom.Next(1, 2047));
            double shade = .1 * ThreadSafeRandom.Next(0, 9);
            wo.SetProperty(PropertyFloat.Shade, shade);

            wo = AssignArmorLevel(wo, tier, armorType);

            wo = AssignEquipmentSetId(wo, tier);

            if (isMagical)
            {
                bool covenantArmor = false || (armorType == LootTables.ArmorType.CovenantArmor || armorType == LootTables.ArmorType.OlthoiArmor);
                wo = AssignMagic(wo, tier, covenantArmor);
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

        private static WorldObject AssignEquipmentSetId(WorldObject wo, int tier)
        {
            int equipSetId = 0;

            if (tier > 6)
                wo.SetProperty(PropertyInt.EquipmentSetId, equipSetId);

            return wo;
        }

        private static int GetCovenantWieldReq(int tier, Skill skill)
        {
            int index, wield;

            switch (tier)
            {
                case 3:
                    index = ThreadSafeRandom.Next(1, 3);
                    break;
                case 4:
                    index = ThreadSafeRandom.Next(1, 4);
                    break;
                case 5:
                    index = ThreadSafeRandom.Next(1, 5);
                    break;
                case 6:
                    index = ThreadSafeRandom.Next(1, 6);
                    break;
                case 7:
                    index = ThreadSafeRandom.Next(1, 7);
                    break;
                default:
                    index = ThreadSafeRandom.Next(1, 8);
                    break;
            }

            switch (skill)
            {
                case Skill.MagicDefense:
                    switch (index)
                    {
                        case 1:
                            wield = 145;
                            break;
                        case 2:
                            wield = 185;
                            break;
                        case 3:
                            wield = 225;
                            break;
                        case 4:
                            wield = 245;
                            break;
                        case 5:
                            wield = 270;
                            break;
                        case 6:
                            wield = 290;
                            break;
                        case 7:
                            wield = 310;
                            break;
                        default:
                            wield = 320;
                            break;
                    }
                    break;
                case Skill.MissileDefense:
                    switch (index)
                    {
                        case 1:
                            wield = 160;
                            break;
                        case 2:
                            wield = 205;
                            break;
                        case 3:
                            wield = 245;
                            break;
                        case 4:
                            wield = 270;
                            break;
                        case 5:
                            wield = 290;
                            break;
                        case 6:
                            wield = 305;
                            break;
                        case 7:
                            wield = 330;
                            break;
                        default:
                            wield = 340;
                            break;
                    }
                    break;
                default:
                    switch (index)
                    {
                        case 1:
                            wield = 200;
                            break;
                        case 2:
                            wield = 250;
                            break;
                        case 3:
                            wield = 300;
                            break;
                        case 4:
                            wield = 325;
                            break;
                        case 5:
                            wield = 350;
                            break;
                        case 6:
                            wield = 370;
                            break;
                        case 7:
                            wield = 400;
                            break;
                        default:
                            wield = 410;
                            break;
                    }
                    break;
            }

            return wield;
        }

        /// <summary>
        /// Assign a final AL value based upon tier
        /// Used values given at http://acpedia.org/wiki/Loot/Tier_7_Pre2013#Max_Armor_Levels
        /// and http://acpedia.org/wiki/Loot/Tier_8#Maximum_Armor_Levels for setting the AL mod values
        /// so as to not exceed the values listed on those tables
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="tier"></param>
        /// <param name="armorType"></param>
        /// <returns></returns>
        private static WorldObject AssignArmorLevel(WorldObject wo, int tier, LootTables.ArmorType armorType)
        {
            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

            if (wo.ArmorType != null && wo.ClothingPriority != (CoverageMask)CoverageMaskHelper.Underwear)
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
                            armorModValue = ThreadSafeRandom.Next(20, 50);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(0, 29);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(0, 30);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(0, 10);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = 0;
                        break;
                    case 2:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(50, 80);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(29, 52);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(30, 54);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(10, 30);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = 0;
                        break;
                    case 3:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(80, 110);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(52, 75);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(54, 80);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(30, 60);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(190, 210);
                        break;
                    case 4:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(110, 140);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(75, 98);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(80, 106);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(60, 90);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(210, 230);
                        break;
                    case 5:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(140, 170);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(98, 121);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(106, 132);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(90, 120);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(230, 250);
                        break;
                    case 6:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(170, 200);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(121, 144);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(132, 158);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(120, 150);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(250, 270);
                        break;
                    case 7:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(200, 230);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(144, 167);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(158, 184);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(150, 180);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(270, 290);
                        break;
                    case 8:
                        if (wo.ArmorType == (int)ArmorType.Cloth)
                            armorModValue = ThreadSafeRandom.Next(230, 260);
                        if (wo.ArmorType == (int)ArmorType.Leather)
                            armorModValue = ThreadSafeRandom.Next(167, 190);
                        if (wo.ArmorType == (int)ArmorType.StuddedLeather)
                            armorModValue = ThreadSafeRandom.Next(184, 210);
                        if (wo.ArmorType == (int)ArmorType.Metal
                            || wo.ArmorType == (int)ArmorType.Chainmail
                            || wo.ArmorType == (int)ArmorType.Scalemail)
                            armorModValue = ThreadSafeRandom.Next(180, 204);
                        // Covenant and Olthoi Armor (not Amuli, Celdon, Koujia, or Alduressa types of Olthoi Armor)
                        if (wo.ResistMagic != null && wo.ResistMagic == 9999)
                            armorModValue = ThreadSafeRandom.Next(290, 310);
                        break;
                }

                int adjustedArmorLevel = baseArmorLevel + armorModValue;
                wo.SetProperty(PropertyInt.ArmorLevel, adjustedArmorLevel);
            }

            if (wo.ArmorType == null)
                log.Warn($"[LOOT] Missing PropertyInt.ArmorType on loot item {wo.WeenieClassId} - {wo.Name}");

            return wo;
        }

        private static WorldObject AssignArmorLevelCompat(WorldObject wo, int tier, LootTables.ArmorType armorType)
        {
            log.Warn($"[LOOT] Using AL Assignment Compatibility layer for item {wo.WeenieClassId} - {wo.Name}.");

            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

            if (baseArmorLevel > 0)
            {
                int armorModValue = 0;

                if (armorType > LootTables.ArmorType.HaebreanArmor && armorType <= LootTables.ArmorType.OlthoiAlduressaArmor)
                {
                    // Even if most are not using T8, made a change to that outcome to ensure that Olthoi Alduressa doesn't go way out of spec
                    // Side effect is that Haebrean to Olthoi Celdon may suffer
                    switch (tier)
                    {
                        case 7:
                            armorModValue = ThreadSafeRandom.Next(0, 40);
                            break;
                        case 8:
                            armorModValue = ThreadSafeRandom.Next(91, 115); 
                            break;
                        default:
                            armorModValue = 0;
                            break;
                    }
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
