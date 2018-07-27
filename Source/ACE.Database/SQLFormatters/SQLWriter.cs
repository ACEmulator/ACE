using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters
{
    public abstract class SQLWriter
    {
        protected static readonly Regex IllegalInFileName = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", RegexOptions.Compiled);

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// </summary>
        public Dictionary<uint, string> WeenieClassNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a weenie id is found in the dictionary, the name will be added in the form of a /* Friendly Weenie Name */
        /// </summary>
        public Dictionary<uint, string> WeenieNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a spell id is found in the dictionary, the name will be added in the form of a /* Friendly Spell Name */
        /// </summary>
        public Dictionary<uint, string> SpellNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a opcode is found in the dictionary, the name will be added in the form of a /* Friendly Opcode Name */
        /// </summary>
        public Dictionary<uint, string> PacketOpCodes;

        /// <summary>
        /// lineGenerator should generate the entire line after the first (. It should include the trailing ) and any comments after.<para />
        /// It should consist of only a single line.<para />
        /// This will automatically call FixNullFields on the output created by lineGenerator()
        /// </summary>
        protected static void ValuesWriter(int count, Func<int, string> lineGenerator, StreamWriter writer)
        {
            for (int i = 0; i < count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += lineGenerator(i);

                if (i == count - 1)
                    output += ";";

                output = FixNullFields(output);

                writer.WriteLine(output);
            }
        }

        /// <summary>
        /// If input is null, NULL will be returned.<para />
        /// If input is not null, a string surrounded in ' will be returned, and any ' found within the string will be replaced wtih ''
        /// </summary>
        protected static string GetSQLString(string input)
        {
            if (input == null)
                return null;

            return "'" + input.Replace("'", "''") + "'";
        }

        /// <summary>
        /// This will find values that were not output to a values line, for example, if a property is a (int?), and it has no value, you might see ", ," in the sql.<para />
        /// This function will replace that ", ," with ", NULL,".<para />
        /// It also removes empty comments like the folliwng: " /*  */"
        /// </summary>
        protected static string FixNullFields(string input)
        {
            input = input.Replace(", ,", ", NULL,");
            input = input.Replace(", ,", ", NULL,");

            // Fix cases where the last field might be null
            input = input.Replace(", )", ", NULL)");

            // Remove empty comments
            input = input.Replace(" /*  */", "");

            return input;
        }

        protected string GetIntPropertyDescription(PropertyInt property, int value)
        {
            switch (property)
            {
                case PropertyInt.AmmoType:
                    return Enum.GetName(typeof(AmmoType), value);
                case PropertyInt.BoosterEnum:
                    return Enum.GetName(typeof(PropertyAttribute2nd), value);
                case PropertyInt.ClothingPriority:
                    return ((CoverageMask)value).ToString();
                case PropertyInt.CombatMode:
                    return Enum.GetName(typeof(CombatMode), value);
                case PropertyInt.CombatUse:
                    return Enum.GetName(typeof(CombatUse), value);
                case PropertyInt.CreatureType:
                case PropertyInt.SlayerCreatureType:
                case PropertyInt.FoeType:
                case PropertyInt.FriendType:
                    return Enum.GetName(typeof(CreatureType), value);
                case PropertyInt.CurrentWieldedLocation:
                case PropertyInt.ValidLocations:
                    return ((EquipMask)value).ToString();
                case PropertyInt.DamageType:
                    return ((DamageType)value).ToString();
                case PropertyInt.DefaultCombatStyle:
                case PropertyInt.AiAllowedCombatStyle:
                    return Enum.GetName(typeof(CombatStyle), value);
                case PropertyInt.Gender:
                    return Enum.GetName(typeof(Gender), value);
                case PropertyInt.GeneratorDestructionType:
                case PropertyInt.GeneratorEndDestructionType:
                    return Enum.GetName(typeof(GeneratorDestruct), value);
                case PropertyInt.GeneratorTimeType:
                    return Enum.GetName(typeof(GeneratorTimeType), value);
                case PropertyInt.GeneratorType:
                    return Enum.GetName(typeof(GeneratorType), value);
                case PropertyInt.HeritageGroup:
                    return Enum.GetName(typeof(HeritageGroup), value);
                case PropertyInt.HookItemType:
                case PropertyInt.ItemType:
                case PropertyInt.MerchandiseItemTypes:
                case PropertyInt.TargetType:
                    return Enum.GetName(typeof(ItemType), value);
                case PropertyInt.HookPlacement:
                case PropertyInt.Placement:
                    return Enum.GetName(typeof(Placement), value);
                case PropertyInt.HookType:
                    return ((HookType)value).ToString();
                case PropertyInt.HouseType:
                    return Enum.GetName(typeof(HouseType), value);
                case PropertyInt.ItemUseable:
                    return ((Usable)value).ToString();
                case PropertyInt.ItemXpStyle:
                    return Enum.GetName(typeof(ItemXpStyle), value);
                case PropertyInt.MaterialType:
                    return Enum.GetName(typeof(Material), value);
                case PropertyInt.PaletteTemplate:
                    return Enum.GetName(typeof(PaletteTemplate), value);
                case PropertyInt.PhysicsState:
                    return ((PhysicsState)value).ToString();
                case PropertyInt.PlayerKillerStatus:
                    return Enum.GetName(typeof(PlayerKillerStatus), value);
                case PropertyInt.PortalBitmask:
                    return Enum.GetName(typeof(PortalBitmask), value);
                case PropertyInt.RadarBlipColor:
                    return Enum.GetName(typeof(RadarColor), value);
                case PropertyInt.ShowableOnRadar:
                    return Enum.GetName(typeof(RadarBehavior), value);
                case PropertyInt.SummoningMastery:
                    return Enum.GetName(typeof(SummoningMastery), value);
                case PropertyInt.UiEffects:
                    return Enum.GetName(typeof(UiEffects), value);
                case PropertyInt.WeaponSkill:
                case PropertyInt.WieldSkilltype2:
                case PropertyInt.WieldSkilltype3:
                case PropertyInt.WieldSkilltype4:
                case PropertyInt.WieldSkilltype:
                    return Enum.GetName(typeof(Skill), value);
                case PropertyInt.WeaponType:
                    return Enum.GetName(typeof(WeaponType), value);
                case PropertyInt.ActivationCreateClass:
                    if (WeenieNames != null)
                    {
                        WeenieNames.TryGetValue((uint) value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
                case PropertyInt.ActivationResponse:
                    return Enum.GetName(typeof(ActivationResponse), value);
                case PropertyInt.Attuned:
                    return Enum.GetName(typeof(AttunedStatus), value);
                case PropertyInt.AttackHeight:
                    return Enum.GetName(typeof(AttackHeight), value);
                case PropertyInt.AttackType:
                    return ((AttackType)value).ToString();
                case PropertyInt.Bonded:
                    return Enum.GetName(typeof(BondedStatus), value);
                case PropertyInt.ChannelsActive:
                case PropertyInt.ChannelsAllowed:
                    return ((Channel)value).ToString();
                case PropertyInt.AccountRequirements:
                    return Enum.GetName(typeof(SubscriptionStatus), value);
                case PropertyInt.AetheriaBitfield:
                    return ((AetheriaBitfield)value).ToString();
                case PropertyInt.EquipmentSetId:
                    return Enum.GetName(typeof(EquipmentSet), value);
                case PropertyInt.WieldRequirements2:
                case PropertyInt.WieldRequirements3:
                case PropertyInt.WieldRequirements4:
                case PropertyInt.WieldRequirements:
                    return ((WieldRequirement)value).ToString();
                case PropertyInt.GeneratorStartTime:
                case PropertyInt.GeneratorEndTime:
                    return DateTimeOffset.FromUnixTimeSeconds(value).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
                case PropertyInt.ImbuedEffect:
                case PropertyInt.ImbuedEffect2:
                case PropertyInt.ImbuedEffect3:
                case PropertyInt.ImbuedEffect4:
                case PropertyInt.ImbuedEffect5:
                    return ((ImbuedEffectType)value).ToString();
            }

            return null;
        }

        protected string GetIntPropertyDescription(PropertyDataId property, uint value)
        {
            switch (property)
            {
                case PropertyDataId.ActivationAnimation:
                case PropertyDataId.InitMotion:
                case PropertyDataId.UseTargetAnimation:
                case PropertyDataId.UseTargetFailureAnimation:
                case PropertyDataId.UseTargetSuccessAnimation:
                case PropertyDataId.UseUserAnimation:
                    return ((MotionCommand)value).ToString();
                case PropertyDataId.ActivationSound:
                case PropertyDataId.UseSound:
                    return ((Sound)value).ToString();
                case PropertyDataId.AlternateCurrency:
                case PropertyDataId.AugmentationCreateItem:
                case PropertyDataId.LastPortal:
                case PropertyDataId.LinkedPortalOne:
                case PropertyDataId.LinkedPortalTwo:
                case PropertyDataId.OriginalPortal:
                case PropertyDataId.UseCreateItem:
                case PropertyDataId.VendorsClassId:
                    if (WeenieNames != null)
                    {
                        WeenieNames.TryGetValue(value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
                case PropertyDataId.BlueSurgeSpell:
                case PropertyDataId.DeathSpell:
                case PropertyDataId.ProcSpell:
                case PropertyDataId.RedSurgeSpell:
                case PropertyDataId.Spell:
                case PropertyDataId.YellowSurgeSpell:
                    if (SpellNames != null)
                    {
                        SpellNames.TryGetValue(value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
                case PropertyDataId.PhysicsScript:
                case PropertyDataId.RestrictionEffect:
                    return ((PlayScript)value).ToString();
                case PropertyDataId.WieldedTreasureType:
                case PropertyDataId.DeathTreasureType:
                    // todo
                    break;
            }

            return null;
        }
    }
}
