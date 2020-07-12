using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters
{
    public abstract class SQLWriter
    {
        protected static readonly Regex IllegalInFileName = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", RegexOptions.Compiled);

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a weenie id is found in the dictionary, the name will be added in the form of a /* Friendly Weenie Name */
        /// </summary>
        public IDictionary<uint, string> WeenieNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a spell id is found in the dictionary, the name will be added in the form of a /* Friendly Spell Name */
        /// </summary>
        public IDictionary<uint, string> SpellNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a opcode is found in the dictionary, the name will be added in the form of a /* Friendly Opcode Name */
        /// </summary>
        public IDictionary<uint, string> PacketOpCodes;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// </summary>
        public IDictionary<uint, List<TreasureWielded>> TreasureWielded;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// </summary>
        public IDictionary<uint, TreasureDeath> TreasureDeath;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// </summary>
        public IDictionary<uint, Weenie> Weenies;

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

        protected string GetValueEnumName(PropertyInt property, int value)
        {
            switch (property)
            {
                case PropertyInt.ActivationCreateClass:
                case PropertyInt.AttackersClass:
                case PropertyInt.PetClass:
                    if (WeenieNames != null)
                    {
                        WeenieNames.TryGetValue((uint) value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
                case PropertyInt.ActivationResponse:
                    return ((ActivationResponse)value).ToString();
                case PropertyInt.AetheriaBitfield:
                    return ((AetheriaBitfield)value).ToString();
                case PropertyInt.AiAllowedCombatStyle:
                    return ((CombatStyle)value).ToString();
                case PropertyInt.AppraisalLongDescDecoration:
                    return ((AppraisalLongDescDecorations)value).ToString();
                case PropertyInt.AttackType:
                    return ((AttackType)value).ToString();
                case PropertyInt.ChannelsActive:
                case PropertyInt.ChannelsAllowed:
                    return ((Channel)value).ToString();
                case PropertyInt.ClothingPriority:
                    return ((CoverageMask)value).ToString();
                case PropertyInt.CurrentWieldedLocation:
                    return ((EquipMask)value).ToString();
                case PropertyInt.DamageType:
                    return ((DamageType)value).ToString();
                case PropertyInt.DefaultCombatStyle:
                    return ((CombatStyle)value).ToString();
                case PropertyInt.HookType:
                    return ((HookType)value).ToString();
                case PropertyInt.ImbuedEffect:
                case PropertyInt.ImbuedEffect2:
                case PropertyInt.ImbuedEffect3:
                case PropertyInt.ImbuedEffect4:
                    return ((ImbuedEffectType)value).ToString();
                case PropertyInt.ItemUseable:
                    return ((Usable)value).ToString();
                case PropertyInt.MerchandiseItemTypes:
                    return ((ItemType)value).ToString();
                case PropertyInt.PhysicsState:
                    return ((PhysicsState)value).ToString();
                case PropertyInt.PortalBitmask:
                    return ((PortalBitmask)value).ToString();
                case PropertyInt.SlayerCreatureType:
                    return ((CreatureType)value).ToString();
                case PropertyInt.TargetType:
                    return ((ItemType)value).ToString();
                case PropertyInt.UiEffects:
                    return ((UiEffects)value).ToString();
                case PropertyInt.ValidLocations:
                    return ((EquipMask)value).ToString();
                case PropertyInt.WieldRequirements:
                case PropertyInt.WieldRequirements2:
                case PropertyInt.WieldRequirements3:
                case PropertyInt.WieldRequirements4:
                    return ((WieldRequirement)value).ToString();
                case PropertyInt.CombatTactic:
                case PropertyInt.HomesickTargetingTactic:
                case PropertyInt.TargetingTactic:
                    return ((TargetingTactic)value).ToString();
                case PropertyInt.Tolerance:
                    return ((Tolerance)value).ToString();
                case PropertyInt.AiOptions:
                    return ((AiOption)value).ToString();
            }

            return property.GetValueEnumName(value);
        }

        protected string GetValueEnumName(PropertyDataId property, uint value)
        {
            switch (property)
            {
                case PropertyDataId.AlternateCurrency:
                case PropertyDataId.AugmentationCreateItem:
                case PropertyDataId.LastPortal:
                case PropertyDataId.LinkedPortalOne:
                case PropertyDataId.LinkedPortalTwo:
                case PropertyDataId.OriginalPortal:
                case PropertyDataId.UseCreateItem:
                case PropertyDataId.VendorsClassId:
                case PropertyDataId.PCAPPhysicsDIDDataTemplatedFrom:
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
                case PropertyDataId.WieldedTreasureType:
                    string treasureW = "";
                    if (TreasureWielded != null)
                    {
                        if (TreasureWielded.ContainsKey(value))
                        {
                            foreach (var item in TreasureWielded[value])
                            {
                                treasureW += GetValueForTreasureDID(item);
                            }
                            return treasureW;
                        }
                    }
                    else if (TreasureDeath != null)
                    {
                        if (TreasureDeath.ContainsKey(value))
                            return $"Loot Tier: {TreasureDeath[value].Tier}";
                    }
                    break;
                case PropertyDataId.DeathTreasureType:
                    string treasureD = "";
                    if (TreasureDeath != null)
                    {
                        if (TreasureDeath.ContainsKey(value))
                            return $"Loot Tier: {TreasureDeath[value].Tier}";
                    }
                    else if  (TreasureWielded != null)
                    {
                        if (TreasureWielded.ContainsKey(value))
                        {
                            foreach (var item in TreasureWielded[value])
                            {
                                treasureD += GetValueForTreasureDID(item);
                            }
                            return treasureD;
                        }
                    }
                    break;
                case PropertyDataId.PCAPRecordedObjectDesc:
                    return ((ObjectDescriptionFlag)value).ToString();
                case PropertyDataId.PCAPRecordedPhysicsDesc:
                    return ((PhysicsDescriptionFlag)value).ToString();
                case PropertyDataId.PCAPRecordedWeenieHeader:
                    return ((WeenieHeaderFlag)value).ToString();
                case PropertyDataId.PCAPRecordedWeenieHeader2:
                    return ((WeenieHeaderFlag2)value).ToString();
            }

            return property.GetValueEnumName(value);
        }

        protected string GetValueForTreasureDID(TreasureWielded item)
        {
            string treasure = "";

            treasure += Environment.NewLine + $"                                   Wield ";
            if (item.StackSize > 1)
                treasure += $"{item.StackSize}x ";
            treasure += $"{WeenieNames[item.WeenieClassId]} ({item.WeenieClassId})";
            if (item.PaletteId > 0)
                treasure += $" | Palette: {Enum.GetName(typeof(PaletteTemplate), item.PaletteId)} ({item.PaletteId})";
            if (item.Shade > 0)
                treasure += $" | Shade: {item.Shade}";
            treasure += $" | Probability: {item.Probability * 100}%";

            return treasure;
        }

        protected string GetValueForTreasureData(uint weenieOrType, bool isWeenieClassID = false)
        {
            string label = "UNKNOWN RANDOMLY GENERATED TREASURE";

            uint? deathTreasureType = null;
            uint? wieldedTreasureType = null;

            if (isWeenieClassID)
            {
                if (Weenies != null && Weenies.ContainsKey(weenieOrType))
                {
                    deathTreasureType = Weenies[weenieOrType].GetProperty(PropertyDataId.DeathTreasureType);
                    wieldedTreasureType = Weenies[weenieOrType].GetProperty(PropertyDataId.WieldedTreasureType);
                }
            }
            else
            {
                deathTreasureType = weenieOrType;
                wieldedTreasureType = weenieOrType;
            }

            if (deathTreasureType.HasValue && wieldedTreasureType.HasValue)
            {
                if (TreasureDeath != null && TreasureDeath.ContainsKey(deathTreasureType.Value))
                {
                    label = $"RANDOMLY GENERATED TREASURE from Loot Tier {TreasureDeath[deathTreasureType.Value].Tier} from Death Treasure Table id: {deathTreasureType}";
                }
                else if (TreasureWielded != null && TreasureWielded.ContainsKey(wieldedTreasureType.Value))
                {
                    label = "";
                    foreach (var item in TreasureWielded[wieldedTreasureType.Value])
                    {
                        var wName = "";
                        if (WeenieNames != null && WeenieNames.ContainsKey(item.WeenieClassId))
                            wName = WeenieNames[item.WeenieClassId];

                        label += $"{(item.StackSize > 0 ? $"{item.StackSize}" : "1")}x {wName} ({item.WeenieClassId}), ";
                    }
                    label = label.Substring(0, label.Length - 2) + $" from Wielded Treasure Table id: {wieldedTreasureType}";
                }
            }
            else
            {
                label = "nothing";
            }

            return label;
        }
    }
}
