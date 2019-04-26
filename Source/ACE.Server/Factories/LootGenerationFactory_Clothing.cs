using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateArmor(int tier, bool isMagical, LootBias lootBias = LootBias.UnBiased)
        {
            int lowSpellTier = 0;
            int highSpellTier = 0;

            int spellArray = 0;
            int cantripArray = 0;
            int equipSetId = 0;

            int materialType = 0;

            int armorPiece = 0;
            int armorType = 0;
            int armorPieceType = 0;
            int armorWeenie = 0;

            switch (tier)
            {
                case 1:
                    lowSpellTier = 1;
                    highSpellTier = 3;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.ChainmailArmor);
                    break;
                case 2:
                    lowSpellTier = 3;
                    highSpellTier = 5;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.DiforsaArmor);
                    break;
                case 3:
                    lowSpellTier = 4;
                    highSpellTier = 6;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.CovenantArmor);
                    break;
                case 4:
                    lowSpellTier = 5;
                    highSpellTier = 6;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.CovenantArmor);
                    break;
                case 5:
                    lowSpellTier = 5;
                    highSpellTier = 7;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.AlduressaArmor);
                    break;
                case 6:
                    lowSpellTier = 6;
                    highSpellTier = 7;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.HaebreanArmor);
                    break;
                case 7:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.OlthoiAlduressaArmor);
                    break;
                default:
                    lowSpellTier = 7;
                    highSpellTier = 8;
                    if (lootBias == LootBias.Armor) // Armor Mana Forge Chests don't include clothing type items
                        armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.Helms, (int)LootTables.ArmorType.OlthoiAlduressaArmor);
                    else
                        armorType = ThreadSafeRandom.Next((int)LootTables.ArmorType.MiscClothing, (int)LootTables.ArmorType.OlthoiAlduressaArmor);
                    break;
            }

            switch (armorType)
            {
                case (int)LootTables.ArmorType.MiscClothing:
                    armorPiece = ThreadSafeRandom.Next(0, 47);
                    armorWeenie = LootTables.MiscClothing[armorPiece][0];
                    armorPieceType = LootTables.MiscClothing[armorPiece][1];
                    spellArray = LootTables.MiscClothing[armorPiece][2];
                    cantripArray = LootTables.MiscClothing[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.Helms:
                    armorPiece = ThreadSafeRandom.Next(0, 8);
                    armorWeenie = LootTables.Helms[armorPiece][0];
                    armorPieceType = LootTables.Helms[armorPiece][1];
                    spellArray = LootTables.Helms[armorPiece][2];
                    cantripArray = LootTables.Helms[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.Shields:
                    armorPiece = ThreadSafeRandom.Next(0, 4);
                    armorWeenie = LootTables.Shields[armorPiece][0];
                    armorPieceType = LootTables.Shields[armorPiece][1];
                    spellArray = LootTables.Shields[armorPiece][2];
                    cantripArray = LootTables.Shields[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.LeatherArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 15);
                    armorWeenie = LootTables.LeatherArmor[armorPiece][0];
                    armorPieceType = LootTables.LeatherArmor[armorPiece][1];
                    spellArray = LootTables.LeatherArmor[armorPiece][2];
                    cantripArray = LootTables.LeatherArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.StuddedLeatherArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 14);
                    armorWeenie = LootTables.StuddedLeatherArmor[armorPiece][0];
                    armorPieceType = LootTables.StuddedLeatherArmor[armorPiece][1];
                    spellArray = LootTables.StuddedLeatherArmor[armorPiece][2];
                    cantripArray = LootTables.StuddedLeatherArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.ChainmailArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 12);
                    armorWeenie = LootTables.ChainmailArmor[armorPiece][0];
                    armorPieceType = LootTables.ChainmailArmor[armorPiece][1];
                    spellArray = LootTables.ChainmailArmor[armorPiece][2];
                    cantripArray = LootTables.ChainmailArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.PlatemailArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 10);
                    armorWeenie = LootTables.PlatemailArmor[armorPiece][0];
                    armorPieceType = LootTables.PlatemailArmor[armorPiece][1];
                    spellArray = LootTables.PlatemailArmor[armorPiece][2];
                    cantripArray = LootTables.PlatemailArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.ScalemailArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 13);
                    armorWeenie = LootTables.ScalemailArmor[armorPiece][0];
                    armorPieceType = LootTables.ScalemailArmor[armorPiece][1];
                    spellArray = LootTables.ScalemailArmor[armorPiece][2];
                    cantripArray = LootTables.ScalemailArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.YoroiArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 7);
                    armorWeenie = LootTables.YoroiArmor[armorPiece][0];
                    armorPieceType = LootTables.YoroiArmor[armorPiece][1];
                    spellArray = LootTables.YoroiArmor[armorPiece][2];
                    cantripArray = LootTables.YoroiArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.DiforsaArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 12);
                    armorWeenie = LootTables.DiforsaArmor[armorPiece][0];
                    armorPieceType = LootTables.DiforsaArmor[armorPiece][1];
                    spellArray = LootTables.DiforsaArmor[armorPiece][2];
                    cantripArray = LootTables.DiforsaArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.CeldonArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 3);
                    armorWeenie = LootTables.CeldonArmor[armorPiece][0];
                    armorPieceType = LootTables.CeldonArmor[armorPiece][1];
                    spellArray = LootTables.CeldonArmor[armorPiece][2];
                    cantripArray = LootTables.CeldonArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.AmuliArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 1);
                    armorWeenie = LootTables.AmuliArmor[armorPiece][0];
                    armorPieceType = LootTables.AmuliArmor[armorPiece][1];
                    spellArray = LootTables.AmuliArmor[armorPiece][2];
                    cantripArray = LootTables.AmuliArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.KoujiaArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 2);
                    armorWeenie = LootTables.KoujiaArmor[armorPiece][0];
                    armorPieceType = LootTables.KoujiaArmor[armorPiece][1];
                    spellArray = LootTables.KoujiaArmor[armorPiece][2];
                    cantripArray = LootTables.KoujiaArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.TenassaArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 2);
                    armorWeenie = LootTables.TenassaArmor[armorPiece][0];
                    armorPieceType = LootTables.TenassaArmor[armorPiece][1];
                    spellArray = LootTables.TenassaArmor[armorPiece][2];
                    cantripArray = LootTables.TenassaArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.CovenantArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 9);
                    armorWeenie = LootTables.CovenantArmor[armorPiece][0];
                    armorPieceType = LootTables.CovenantArmor[armorPiece][1];
                    spellArray = LootTables.CovenantArmor[armorPiece][2];
                    cantripArray = LootTables.CovenantArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.LoricaArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 5);
                    armorWeenie = LootTables.LoricaArmor[armorPiece][0];
                    armorPieceType = LootTables.LoricaArmor[armorPiece][1];
                    spellArray = LootTables.LoricaArmor[armorPiece][2];
                    cantripArray = LootTables.LoricaArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.NariyidArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 6);
                    armorWeenie = LootTables.NariyidArmor[armorPiece][0];
                    armorPieceType = LootTables.NariyidArmor[armorPiece][1];
                    spellArray = LootTables.NariyidArmor[armorPiece][2];
                    cantripArray = LootTables.NariyidArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.ChiranArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 4);
                    armorWeenie = LootTables.ChiranArmor[armorPiece][0];
                    armorPieceType = LootTables.ChiranArmor[armorPiece][1];
                    spellArray = LootTables.ChiranArmor[armorPiece][2];
                    cantripArray = LootTables.ChiranArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.AlduressaArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 4);
                    armorWeenie = LootTables.AlduressaArmor[armorPiece][0];
                    armorPieceType = LootTables.AlduressaArmor[armorPiece][1];
                    spellArray = LootTables.AlduressaArmor[armorPiece][2];
                    cantripArray = LootTables.AlduressaArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.KnorrAcademyArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 7);
                    armorWeenie = LootTables.KnorrAcademyArmor[armorPiece][0];
                    armorPieceType = LootTables.KnorrAcademyArmor[armorPiece][1];
                    spellArray = LootTables.KnorrAcademyArmor[armorPiece][2];
                    cantripArray = LootTables.KnorrAcademyArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.SedgemailLeatherArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 5);
                    armorWeenie = LootTables.SedgemailLeatherArmor[armorPiece][0];
                    armorPieceType = LootTables.SedgemailLeatherArmor[armorPiece][1];
                    spellArray = LootTables.SedgemailLeatherArmor[armorPiece][2];
                    cantripArray = LootTables.SedgemailLeatherArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.HaebreanArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 8);
                    armorWeenie = LootTables.HaebreanArmor[armorPiece][0];
                    armorPieceType = LootTables.HaebreanArmor[armorPiece][1];
                    spellArray = LootTables.HaebreanArmor[armorPiece][2];
                    cantripArray = LootTables.HaebreanArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.OlthoiArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 9);
                    armorWeenie = LootTables.OlthoiArmor[armorPiece][0];
                    armorPieceType = LootTables.OlthoiArmor[armorPiece][1];
                    spellArray = LootTables.OlthoiArmor[armorPiece][2];
                    cantripArray = LootTables.OlthoiArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.OlthoiAmuliArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 4);
                    armorWeenie = LootTables.OlthoiAmuliArmor[armorPiece][0];
                    armorPieceType = LootTables.OlthoiAmuliArmor[armorPiece][1];
                    spellArray = LootTables.OlthoiAmuliArmor[armorPiece][2];
                    cantripArray = LootTables.OlthoiAmuliArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.OlthoiCeldonArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 6);
                    armorWeenie = LootTables.OlthoiCeldonArmor[armorPiece][0];
                    armorPieceType = LootTables.OlthoiCeldonArmor[armorPiece][1];
                    spellArray = LootTables.OlthoiCeldonArmor[armorPiece][2];
                    cantripArray = LootTables.OlthoiCeldonArmor[armorPiece][3];
                    break;

                case (int)LootTables.ArmorType.OlthoiKoujiaArmor:
                    armorPiece = ThreadSafeRandom.Next(0, 4);
                    armorWeenie = LootTables.OlthoiKoujiaArmor[armorPiece][0];
                    armorPieceType = LootTables.OlthoiKoujiaArmor[armorPiece][1];
                    spellArray = LootTables.OlthoiKoujiaArmor[armorPiece][2];
                    cantripArray = LootTables.OlthoiKoujiaArmor[armorPiece][3];
                    break;

                default: // Olthoi Alduressa
                    armorPiece = ThreadSafeRandom.Next(0, 4);
                    armorWeenie = LootTables.OlthoiAlduressaArmor[armorPiece][0];
                    armorPieceType = LootTables.OlthoiAlduressaArmor[armorPiece][1];
                    spellArray = LootTables.OlthoiAlduressaArmor[armorPiece][2];
                    cantripArray = LootTables.OlthoiAlduressaArmor[armorPiece][3];
                    break;
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            wo.SetProperty(PropertyInt.AppraisalItemSkill, 7);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);

            materialType = GetMaterialType(wo, tier);
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
            if (tier > 6 && armorType != (int)LootTables.ArmorType.CovenantArmor)
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

            if (armorType == (int)LootTables.ArmorType.CovenantArmor)
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

            var baseArmorLevel = wo.GetProperty(PropertyInt.ArmorLevel) ?? 0;

            if (baseArmorLevel == 0)
                wo.RemoveProperty(PropertyInt.ArmorLevel);
            else
            {
                int adjustedArmorLevel = baseArmorLevel + GetArmorLevelModifier(tier, armorType);
                wo.SetProperty(PropertyInt.ArmorLevel, adjustedArmorLevel);
            }

            wo.SetProperty(PropertyInt.EquipmentSetId, equipSetId);

            if (isMagical)
            {
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);
                int numSpells = GetNumSpells(tier);

                int spellcraft = GetSpellcraft(numSpells, tier);
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
                wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));

                int maxMana = GetMaxMana(numSpells, tier);
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, maxMana);

                int[][] spells;
                int[][] cantrips;
                switch (spellArray)
                {
                    case 1:
                        spells = LootTables.HeadSpells;
                        break;
                    case 2:
                        spells = LootTables.ChestSpells;
                        break;
                    case 3:
                        spells = LootTables.UpperArmSpells;
                        break;
                    case 4:
                        spells = LootTables.LowerArmSpells;
                        break;
                    case 5:
                        spells = LootTables.HandSpells;
                        break;
                    case 6:
                        spells = LootTables.AbdomenSpells;
                        break;
                    case 7:
                        spells = LootTables.UpperLegSpells;
                        break;
                    case 8:
                        spells = LootTables.LowerLegSpells;
                        break;
                    case 9:
                        spells = LootTables.FeetSpells;
                        break;
                    default:
                        spells = LootTables.ShieldSpells;
                        break;
                }

                switch (cantripArray)
                {
                    case 1:
                        cantrips = LootTables.HeadCantrips;
                        break;
                    case 2:
                        cantrips = LootTables.ChestCantrips;
                        break;
                    case 3:
                        cantrips = LootTables.UpperArmCantrips;
                        break;
                    case 4:
                        cantrips = LootTables.LowerArmCantrips;
                        break;
                    case 5:
                        cantrips = LootTables.HandCantrips;
                        break;
                    case 6:
                        cantrips = LootTables.AbdomenCantrips;
                        break;
                    case 7:
                        cantrips = LootTables.UpperLegCantrips;
                        break;
                    case 8:
                        cantrips = LootTables.LowerLegCantrips;
                        break;
                    case 9:
                        cantrips = LootTables.FeetCantrips;
                        break;
                    default:
                        cantrips = LootTables.ShieldCantrips;
                        break;
                }

                int[] shuffledValues = new int[spells.Length];
                for (int i = 0; i < spells.Length; i++)
                {
                    shuffledValues[i] = i;
                }

                Shuffle(shuffledValues);

                int minorCantrips = GetNumMinorCantrips(tier);
                int majorCantrips = GetNumMajorCantrips(tier);
                int epicCantrips = GetNumEpicCantrips(tier);
                int legendaryCantrips = GetNumLegendaryCantrips(tier);
                int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

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

        private static int GetArmorLevelModifier(int tier, int armorType)
        {
            // Olthoi Armor base weenies already have the full amount of AL
            if (armorType > (int)LootTables.ArmorType.OlthoiKoujiaArmor)
                return 0;

            if (armorType > (int)LootTables.ArmorType.HaebreanArmor
                    && armorType < (int)LootTables.ArmorType.OlthoiAlduressaArmor)
            {
                switch (tier)
                {
                    case 7:
                        return ThreadSafeRandom.Next(0, 40);
                    default:
                        return ThreadSafeRandom.Next(160, 200);
                }
            }

            switch (tier)
            {
                case 1:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(0, 27);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(0, 23);
                    else
                        return ThreadSafeRandom.Next(0, 40);
                case 2:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(27, 54);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(23, 46);
                    else
                        return ThreadSafeRandom.Next(40, 80);
                case 3:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(54, 81);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(46, 69);
                    else if (armorType == (int)LootTables.ArmorType.CovenantArmor)
                        return ThreadSafeRandom.Next(90, 130);
                    else
                        return ThreadSafeRandom.Next(80, 120);
                case 4:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(81, 108);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(69, 92);
                    else if (armorType == (int)LootTables.ArmorType.CovenantArmor)
                        return ThreadSafeRandom.Next(130, 170);
                    else
                        return ThreadSafeRandom.Next(120, 160);
                case 5:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(108, 135);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(92, 115);
                    else if (armorType == (int)LootTables.ArmorType.CovenantArmor)
                        return ThreadSafeRandom.Next(170, 210);
                    else
                        return ThreadSafeRandom.Next(160, 200);
                case 6:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(135, 162);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(115, 138);
                    else if (armorType == (int)LootTables.ArmorType.CovenantArmor)
                        return ThreadSafeRandom.Next(210, 250);
                    else
                        return ThreadSafeRandom.Next(200, 240);
                case 7:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(162, 189);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(138, 161);
                    else if (armorType == (int)LootTables.ArmorType.CovenantArmor)
                        return ThreadSafeRandom.Next(250, 290);
                    else
                        return ThreadSafeRandom.Next(240, 280);
                case 8:
                    if (armorType == (int)LootTables.ArmorType.StuddedLeatherArmor
                            || armorType == (int)LootTables.ArmorType.Helms
                            || armorType == (int)LootTables.ArmorType.Shields)
                        return ThreadSafeRandom.Next(189, 216);
                    else if (armorType == (int)LootTables.ArmorType.LeatherArmor
                        || armorType == (int)LootTables.ArmorType.MiscClothing)
                        return ThreadSafeRandom.Next(161, 184);
                    else if (armorType == (int)LootTables.ArmorType.CovenantArmor)
                        return ThreadSafeRandom.Next(290, 330);
                    else
                        return ThreadSafeRandom.Next(280, 320);
                default:
                    return 0;
            }
        }
    }
}
