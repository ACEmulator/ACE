using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using log4net;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Entity
{
    public static class MutationCache
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ConcurrentDictionary<string, MutationFilter> tSysMutationFilters = new ConcurrentDictionary<string, MutationFilter>();

        public static MutationFilter GetMutation(string filename)
        {
            if (!tSysMutationFilters.TryGetValue(filename, out var tSysMutationFilter))
            {
                tSysMutationFilter = BuildMutation(filename);

                if (tSysMutationFilter != null)
                    tSysMutationFilters.TryAdd(filename, tSysMutationFilter);
            }
            return tSysMutationFilter;
        }

        private static MutationFilter BuildMutation(string filename)
        {
            var lines = ReadScript(filename);

            if (lines == null)
            {
                log.Error($"MutationCache.BuildMutation({filename}) - embedded resource not found");
                return null;
            }

            string mutationLine = null;

            var mutationFilter = new MutationFilter();
            Mutation mutation = null;
            MutationOutcome outcome = null;
            EffectList effectList = null;

            var totalChance = 0.0M;

            var timer = Stopwatch.StartNew();

            foreach (var line in lines)
            {
                if (line.Contains("Mutation #", StringComparison.OrdinalIgnoreCase))
                {
                    mutationLine = line;
                    continue;
                }

                if (line.Contains("Tier chances", StringComparison.OrdinalIgnoreCase))
                {
                    if (outcome != null && outcome.EffectLists.Last().Chance != 1.0f)
                        log.Error($"MutationCache.BuildMutation({filename}) - {mutationLine} total {outcome.EffectLists.Last().Chance}, expected 1.0");

                    mutation = new Mutation();
                    mutationFilter.Mutations.Add(mutation);

                    var tierPieces = line.Split(',');

                    foreach (var tierPiece in tierPieces)
                    {
                        var match = Regex.Match(tierPiece, @"([\d.]+)");
                        if (match.Success && float.TryParse(match.Groups[1].Value, out var tierChance))
                        {
                            mutation.Chances.Add(tierChance);
                        }
                        else
                        {
                            log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse tier chances for {mutationLine}: {tierPiece}");
                            mutation.Chances.Add(0.0f);
                        }
                    }

                    // verify
                    outcome = new MutationOutcome();
                    mutation.Outcomes.Add(outcome);

                    totalChance = 0.0M;

                    continue;
                }

                if (line.Contains("- Chance", StringComparison.OrdinalIgnoreCase))
                {
                    effectList = new EffectList();
                    outcome.EffectLists.Add(effectList);

                    var match = Regex.Match(line, @"([\d.]+)");
                    if (match.Success && decimal.TryParse(match.Groups[1].Value, out var chance))
                    {
                        totalChance += chance / 100;

                        effectList.Chance = (float)totalChance;
                    }
                    else
                    {
                        log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse {line} for {mutationLine}");
                    }
                    continue;
                }

                if (!line.Contains("="))
                    continue;

                var effect = new Effect();

                effect.Type = GetMutationEffectType(line);

                var firstOperator = GetFirstOperator(effect.Type);

                var pieces = line.Split(firstOperator, 2);

                if (pieces.Length != 2)
                {
                    log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse {line}");
                    continue;
                }

                pieces[0] = pieces[0].Trim();
                pieces[1] = pieces[1].Trim();

                var firstStatType = GetStatType(pieces[0]);

                /*if (firstStatType == StatType.Undef)
                {
                    log.Error($"MutationCache.BuildMutation({filename}) - couldn't determine StatType for {pieces[0]} in {line}");
                    continue;
                }*/

                effect.Quality = ParseEffectArgument(filename, pieces[0]);

                var hasSecondOperator = HasSecondOperator(effect.Type);

                if (!hasSecondOperator)
                {
                    effect.Arg1 = ParseEffectArgument(filename, pieces[1]);
                }
                else
                {
                    var secondOperator = GetSecondOperator(effect.Type);

                    var subpieces = pieces[1].Split(secondOperator, 2);

                    if (subpieces.Length != 2)
                    {
                        log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse {line}");
                        continue;
                    }

                    subpieces[0] = subpieces[0].Trim();
                    subpieces[1] = subpieces[1].Trim();

                    effect.Arg1 = ParseEffectArgument(filename, subpieces[0]);
                    effect.Arg2 = ParseEffectArgument(filename, subpieces[1]);
                }

                effectList.Effects.Add(effect);
            }

            if (outcome != null && outcome.EffectLists.Last().Chance != 1.0f)
                log.Error($"MutationCache.BuildMutation({filename}) - {mutationLine} total {outcome.EffectLists.Last().Chance}, expected 1.0");

            timer.Stop();

            // scripts take about ~2ms to compile
            //Console.WriteLine($"Compiled {filename} in {timer.Elapsed.TotalMilliseconds}ms");

            return mutationFilter;
        }

        private static EffectArgument ParseEffectArgument(string filename, string operand)
        {
            var effectArgument = new EffectArgument();

            effectArgument.Type = GetEffectArgumentType(operand);

            switch (effectArgument.Type)
            {
                case EffectArgumentType.Int:

                    if (!int.TryParse(operand, out effectArgument.IntVal))
                    {
                        if (System.Enum.TryParse(operand, out WieldRequirement wieldRequirement))
                            effectArgument.IntVal = (int)wieldRequirement;
                        else
                            log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse IntVal {operand}");
                    }
                    break;

                case EffectArgumentType.Double:

                    if (!double.TryParse(operand, out effectArgument.DoubleVal))
                    {
                        log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse DoubleVal {operand}");
                    }
                    break;

                case EffectArgumentType.Quality:

                    effectArgument.StatType = GetStatType(operand);

                    switch (effectArgument.StatType)
                    {
                        case StatType.Int:

                            if (System.Enum.TryParse(operand, out PropertyInt propInt))
                                effectArgument.StatIdx = (int)propInt;
                            else
                                log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse PropertyInt.{operand}");
                            break;

                        case StatType.Float:

                            if (System.Enum.TryParse(operand, out PropertyFloat propFloat))
                                effectArgument.StatIdx = (int)propFloat;
                            else
                                log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse PropertyFloat.{operand}");
                            break;

                        case StatType.Bool:

                            if (System.Enum.TryParse(operand, out PropertyBool propBool))
                                effectArgument.StatIdx = (int)propBool;
                            else
                                log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse PropertyBool.{operand}");
                            break;

                        case StatType.DataID:

                            if (System.Enum.TryParse(operand, out PropertyDataId propDID))
                                effectArgument.StatIdx = (int)propDID;
                            else
                                log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse PropertyBool.{operand}");
                            break;

                        default:
                            log.Error($"MutationCache.BuildMutation({filename}) - unknown PropertyType.{operand}");
                            break;
                    }
                    break;

                case EffectArgumentType.Random:

                    var match = Regex.Match(operand, @"Random\(([\d.-]+), ([\d.-]+)\)");

                    if (!match.Success || !float.TryParse(match.Groups[1].Value, out effectArgument.MinVal) || !float.TryParse(match.Groups[2].Value, out effectArgument.MaxVal))
                        log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse {operand}");

                    break;

                case EffectArgumentType.Variable:

                    match = Regex.Match(operand, @"\[(\d+)\]");

                    if (!match.Success || !int.TryParse(match.Groups[1].Value, out effectArgument.IntVal))
                        log.Error($"MutationCache.BuildMutation({filename}) - couldn't parse {operand}");

                    break;
            }
            return effectArgument;
        }

        private static MutationEffectType GetMutationEffectType(string line)
        {
            if (line.Contains("+="))
            {
                if (line.Contains("*"))
                    return MutationEffectType.AddMultiply;
                else if (line.Contains("/"))
                    return MutationEffectType.AddDivide;
                else
                    return MutationEffectType.Add;
            }
            else if (line.Contains("-="))
            {
                if (line.Contains("*"))
                    return MutationEffectType.SubtractMultiply;
                else if (line.Contains("/"))
                    return MutationEffectType.SubtractDivide;
                else
                    return MutationEffectType.Subtract;
            }
            else if (line.Contains("*="))
                return MutationEffectType.Multiply;
            else if (line.Contains("/="))
                return MutationEffectType.Divide;
            else if (line.Contains("+"))
                return MutationEffectType.AssignAdd;
            else if (line.Contains(" - "))
                return MutationEffectType.AssignSubtract;
            else if (line.Contains("*"))
                return MutationEffectType.AssignMultiply;
            else if (line.Contains("/"))
                return MutationEffectType.AssignDivide;
            else if (line.Contains(">="))
                return MutationEffectType.AtLeastAdd;
            else if (line.Contains("<="))
                return MutationEffectType.AtMostSubtract;
            else
                return MutationEffectType.Assign;
        }

        private static string GetFirstOperator(MutationEffectType effectType)
        {
            switch (effectType)
            {
                case MutationEffectType.Assign:
                case MutationEffectType.AssignAdd:
                case MutationEffectType.AssignSubtract:
                case MutationEffectType.AssignMultiply:
                case MutationEffectType.AssignDivide:
                    return "=";
                case MutationEffectType.Add:
                case MutationEffectType.AddMultiply:
                case MutationEffectType.AddDivide:
                    return "+=";
                case MutationEffectType.Subtract:
                case MutationEffectType.SubtractMultiply:
                case MutationEffectType.SubtractDivide:
                    return "-=";
                case MutationEffectType.Multiply:
                    return "*=";
                case MutationEffectType.Divide:
                    return "/=";
                case MutationEffectType.AtLeastAdd:
                    return ">=";
                case MutationEffectType.AtMostSubtract:
                    return "<=";
            }
            return null;
        }

        public static StatType GetStatType(string operand)
        {
            if (System.Enum.TryParse(operand, out PropertyInt propInt))
                return StatType.Int;
            else if (System.Enum.TryParse(operand, out PropertyFloat propFloat))
                return StatType.Float;
            else if (System.Enum.TryParse(operand, out PropertyBool propBool))
                return StatType.Bool;
            else if (System.Enum.TryParse(operand, out PropertyDataId propDID))
                return StatType.DataID;
            else
                return StatType.Undef;
        }

        public static EffectArgumentType GetEffectArgumentType(string operand)
        {
            if (IsNumber(operand) || System.Enum.TryParse(operand, out WieldRequirement wieldRequirement))
            {
                if (operand.Contains('.'))
                    return EffectArgumentType.Double;
                else
                    return EffectArgumentType.Int;
            }
            else if (GetStatType(operand) != StatType.Undef)
            {
                return EffectArgumentType.Quality;
            }
            else if (operand.StartsWith("Random", StringComparison.OrdinalIgnoreCase))
                return EffectArgumentType.Random;
            else if (operand.StartsWith("Variable", StringComparison.OrdinalIgnoreCase))
                return EffectArgumentType.Variable;
            else
                return EffectArgumentType.Invalid;
        }

        public static bool IsNumber(string operand)
        {
            var match = Regex.Match(operand, @"([\d.-]+)");
            return match.Success && match.Groups[1].Value.Equals(operand);
        }

        public static bool HasSecondOperator(MutationEffectType effectType)
        {
            switch (effectType)
            {
                case MutationEffectType.AssignAdd:
                case MutationEffectType.AssignSubtract:
                case MutationEffectType.AssignMultiply:
                case MutationEffectType.AssignDivide:
                case MutationEffectType.AddMultiply:
                case MutationEffectType.AddDivide:
                case MutationEffectType.SubtractMultiply:
                case MutationEffectType.SubtractDivide:
                case MutationEffectType.AtLeastAdd:
                case MutationEffectType.AtMostSubtract:
                    return true;
            }
            return false;
        }

        public static string GetSecondOperator(MutationEffectType effectType)
        {
            switch (effectType)
            {
                case MutationEffectType.AssignAdd:
                    return "+";
                case MutationEffectType.AssignSubtract:
                    return "-";
                case MutationEffectType.AssignMultiply:
                case MutationEffectType.AddMultiply:
                case MutationEffectType.SubtractMultiply:
                    return "*";
                case MutationEffectType.AssignDivide:
                case MutationEffectType.AddDivide:
                case MutationEffectType.SubtractDivide:
                    return "/";
                case MutationEffectType.AtLeastAdd:
                    return ", add";
                case MutationEffectType.AtMostSubtract:
                    return ", sub";
            }
            return null;
        }

        private static readonly string prefix = "ACE.Server.Factories.Entity.Mutations.";

        private static List<string> ReadScript(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = prefix + filename.Replace('/', '.');

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;

                var lines = new List<string>();

                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                        lines.Add(reader.ReadLine());
                }
                return lines;
            }
        }
    }
}
