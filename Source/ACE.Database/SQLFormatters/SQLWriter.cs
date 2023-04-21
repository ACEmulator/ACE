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
                case PropertyInt.Faction1Bits:
                case PropertyInt.Faction2Bits:
                case PropertyInt.Faction3Bits:
                case PropertyInt.Hatred1Bits:
                case PropertyInt.Hatred2Bits:
                case PropertyInt.Hatred3Bits:
                    return ((FactionBits)value).ToString();
                case PropertyInt.CharacterTitleId:
                    return ((CharacterTitle)value).ToString();
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
                    if (TreasureWielded != null && TreasureWielded.ContainsKey(value))
                    {
                        return GetValuesForTreasureDID(TreasureWielded[value]);
                    }
                    else if (TreasureDeath != null && TreasureDeath.ContainsKey(value))
                    {
                        return $"Loot Tier: {TreasureDeath[value].Tier}";
                    }
                    break;
                case PropertyDataId.DeathTreasureType:
                    if (TreasureDeath != null && TreasureDeath.ContainsKey(value))
                    {
                        return $"Loot Tier: {TreasureDeath[value].Tier}";
                    }
                    else if (TreasureWielded != null && TreasureWielded.ContainsKey(value))
                    {
                        return GetValuesForTreasureDID(TreasureWielded[value]);
                    }
                    break;
                case PropertyDataId.InventoryTreasureType:
                    if (TreasureWielded != null && TreasureWielded.ContainsKey(value))
                    {
                        return GetValuesForTreasureDID(TreasureWielded[value]);
                    }
                    else if (TreasureDeath != null && TreasureDeath.ContainsKey(value))
                    {
                        return $"Loot Tier: {TreasureDeath[value].Tier}";
                    }
                    break;
                case PropertyDataId.ShopTreasureType:
                    if (TreasureWielded != null && TreasureWielded.ContainsKey(value))
                    {
                        return GetValuesForTreasureDID(TreasureWielded[value]);
                    }
                    else if (TreasureDeath != null && TreasureDeath.ContainsKey(value))
                    {
                        return $"Loot Tier: {TreasureDeath[value].Tier}";
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

        protected string GetValuesForTreasureDID(List<TreasureWielded> treasureWieldedList)
        {
            string treasure = "";

            var setNumber = 0;
            var setTotalProbability = 0.0f;
            var nextItemIsStartOfSubSet = false;
            var nextItemIsPartOfSubSet = false;
            var depth = 0;
            var subSetTotalProbability = new Dictionary<int, float>();

            foreach (var item in treasureWieldedList)
            {
                var treasureItem = "";
                if (item.StackSize > 1 && item.StackSizeVariance == 0)
                    treasureItem += $"{item.StackSize}x ";

                if (item.StackSize > 1 && item.StackSizeVariance > 0)
                {
                    var minStack = Math.Max(1, (int)Math.Round(item.StackSize * (1.0f - item.StackSizeVariance), 0, MidpointRounding.AwayFromZero));
                    var maxStack = item.StackSize;
                    if (minStack != maxStack)
                        treasureItem += $"{minStack}x to {maxStack}x ";
                    else
                        treasureItem += $"{maxStack}x ";
                }

                treasureItem += $"{WeenieNames[item.WeenieClassId]} ({item.WeenieClassId})";

                if (item.PaletteId > 0)
                    treasureItem += $" | Palette: {Enum.GetName(typeof(PaletteTemplate), item.PaletteId)} ({item.PaletteId})";

                if (item.Shade > 0)
                    treasureItem += $" | Shade: {item.Shade}";

                if (item.StackSizeVariance > 0)
                    treasureItem += $" | StackSizeVariance: {item.StackSizeVariance}";

                if (item.SetStart || (setTotalProbability >= 1.0f && !nextItemIsPartOfSubSet))
                {
                    if ((!nextItemIsStartOfSubSet && !item.ContinuesPreviousSet) || (nextItemIsStartOfSubSet && item.ContinuesPreviousSet) || (setTotalProbability >= 1.0f && !nextItemIsStartOfSubSet))
                    {
                        if (setTotalProbability > 0.0f && setTotalProbability < 1.0f)
                        {
                            var setProbabilityLeftover = (1.0f - setTotalProbability) * 100;
                            if (setProbabilityLeftover < 0.01f)
                                treasure += Environment.NewLine + $"                                   | {Math.Ceiling(setProbabilityLeftover),6:#.00}% chance of nothing from this set";
                            else
                                treasure += Environment.NewLine + $"                                   | {(1.0f - setTotalProbability) * 100,6:#.00}% chance of nothing from this set";
                        }

                        if (depth > 0 && subSetTotalProbability[depth] > 0.0f && subSetTotalProbability[depth] < 1.0f)
                        {
                            var setProbabilityLeftover = (1.0f - subSetTotalProbability[depth]) * 100;
                            if (setProbabilityLeftover < 0.01f)
                                treasure += Environment.NewLine + $"                                   |          {new string(' ', 1 * depth)} {Math.Ceiling(setProbabilityLeftover),6:#.00}% chance of nothing from this subset";
                            else
                                treasure += Environment.NewLine + $"                                   |          {new string(' ', 1 * depth)} {(1.0f - subSetTotalProbability[depth]) * 100,6:#.00}% chance of nothing from this subset";
                        }

                        treasure += Environment.NewLine + $"                                   # Set: {++setNumber}";
                        nextItemIsStartOfSubSet = false;
                        nextItemIsPartOfSubSet = false;
                        setTotalProbability = 0.0f;
                        if (depth > 0)
                            depth--;
                    }
                    else if (nextItemIsStartOfSubSet)
                    {
                        treasure += Environment.NewLine + $"                                   |       {new string(' ', 1 * depth)} with";
                        nextItemIsStartOfSubSet = false;
                        nextItemIsPartOfSubSet = true;
                    }
                    else if (nextItemIsPartOfSubSet && item.ContinuesPreviousSet)
                    {
                        nextItemIsStartOfSubSet = false;
                        nextItemIsPartOfSubSet = false;
                        if (depth > 0)
                            depth--;
                    }
                }

                if (!nextItemIsPartOfSubSet)
                {
                    if ((setTotalProbability + item.Probability) > 1.0f)
                    {
                        var realProbability = item.Probability - (setTotalProbability + item.Probability - 1.0f);

                        if ($"{realProbability:#.00}" != $"{item.Probability:#.00}")
                            treasureItem += $" | Chance adjusted down from {item.Probability * 100:#.00}% due to overage for this set";

                        treasure += Environment.NewLine + $"                                   | {realProbability * 100,6:#.00}% chance of {treasureItem}";
                    }
                    else
                        treasure += Environment.NewLine + $"                                   | {item.Probability * 100,6:#.00}% chance of {treasureItem}";

                    setTotalProbability += item.Probability;
                }
                else
                {
                    treasure += Environment.NewLine + $"                                   |          {new string(' ', 1 * depth)} {item.Probability * 100,6:#.00}% chance of {treasureItem}";
                    subSetTotalProbability[depth] += item.Probability;
                }

                if (item.HasSubSet)
                {
                    nextItemIsStartOfSubSet = true;
                    nextItemIsPartOfSubSet = false;
                    depth++;
                    subSetTotalProbability[depth] = 0.0f;
                }
            }

            if (depth > 0 && subSetTotalProbability[depth] > 0.0f && subSetTotalProbability[depth] < 1.0f)
            {
                var setProbabilityLeftover = (1.0f - subSetTotalProbability[depth]) * 100;
                if (setProbabilityLeftover < 0.01f)
                    treasure += Environment.NewLine + $"                                   |          {new string(' ', 1 * depth)} {Math.Ceiling(setProbabilityLeftover),6:#.00}% chance of nothing from this subset";
                else
                    treasure += Environment.NewLine + $"                                   |          {new string(' ', 1 * depth)} {(1.0f - subSetTotalProbability[depth]) * 100,6:#.00}% chance of nothing from this subset";
            }

            if (setTotalProbability > 0.0f && setTotalProbability < 1.0f)
            {
                var setProbabilityLeftover = (1.0f - setTotalProbability) * 100;
                if (setProbabilityLeftover < 0.01f)
                    treasure += Environment.NewLine + $"                                   | {Math.Ceiling(setProbabilityLeftover),6:#.00}% chance of nothing from this set";
                else
                    treasure += Environment.NewLine + $"                                   | {(1.0f - setTotalProbability) * 100,6:#.00}% chance of nothing from this set";
            }

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
                    label = $"something from one or more sets from Wielded Treasure Table id: {wieldedTreasureType}";
                    label += GetValuesForTreasureDID(TreasureWielded[wieldedTreasureType.Value]);
                }
            }
            else
            {
                label = "nothing";
            }

            return label;
        }

        protected static float? TrimNegativeZero(float? input, int places = 6)
        {
            if (input == null)
                return null;

            var str = input.Value.ToString($"0.{new string('#', places)}");

            if (str == $"-0.{new string('#', places)}" || str == "-0")
                str = str.Substring(1, str.Length - 1);

            var ret = float.Parse(str);

            return ret;
        }
    }
}
