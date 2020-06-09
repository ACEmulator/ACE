using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.WorldObjects;

namespace ACE.Server.Command.Handlers
{
    public class DeveloperFixCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [CommandHandler("verify-player-data", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with player data. Runs all of the verify* commands.")]
        public static void HandleVerifyAll(Session session, params string[] parameters)
        {
            HandleVerifyAttributes(session, parameters);
            HandleVerifyVitals(session, parameters);

            HandleVerifySkills(session, parameters);

            HandleVerifySkillCredits(session, parameters);

            HandleVerifyHeritageAugs(session, parameters);
            HandleVerifyMaxAugs(session, parameters);

            HandleVerifyExperience(session, parameters);
        }

        [CommandHandler("verify-attributes", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with player attribute data")]
        public static void HandleVerifyAttributes(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            foreach (var player in players)
            {
                var updated = false;

                foreach (var attr in new Dictionary<PropertyAttribute, PropertiesAttribute>(player.Biota.PropertiesAttribute))
                {
                    // ensure this is a valid attribute
                    if (attr.Key < PropertyAttribute.Strength || attr.Key > PropertyAttribute.Self)
                    {
                        Console.WriteLine($"{player.Name} has unknown attribute {attr.Key}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            // i have found no instances of this situation being run into,
                            // but if it does happen, verify-xp will refund the player xp properly

                            player.Biota.PropertiesAttribute.Remove(attr);
                            updated = true;
                        }
                        continue;
                    }

                    var rank = attr.Value.LevelFromCP;

                    // verify attribute rank
                    var correctRank = Player.CalcAttributeRank(attr.Value.CPSpent);
                    if (rank != correctRank)
                    {
                        Console.WriteLine($"{player.Name}'s {attr.Key} rank is {rank}, should be {correctRank}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            attr.Value.LevelFromCP = (ushort)correctRank;
                            updated = true;
                        }
                    }

                    // verify attribute xp is within bounds
                    var attributeXPTable = DatManager.PortalDat.XpTable.AttributeXpList;
                    var maxAttributeXp = attributeXPTable[attributeXPTable.Count - 1];

                    if (attr.Value.CPSpent > maxAttributeXp)
                    {
                        Console.WriteLine($"{player.Name}'s {attr.Key} attribute total xp is {attr.Value.CPSpent:N0}, should be capped at {maxAttributeXp:N0}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            // again i have found no instances of this situation being run into,
                            // but if it does happen, verify-xp will refund the player xp properly

                            attr.Value.CPSpent = maxAttributeXp;
                            updated = true;
                        }
                    }
                }
                if (fix && updated)
                    player.SaveBiotaToDatabase();
            }

            if (!fix && foundIssues)
                Console.WriteLine($"Dry run completed. Type 'verify-attributes fix' to fix any issues.");

            if (!foundIssues)
                Console.WriteLine($"Verified attributes for {players.Count:N0} players");
        }

        [CommandHandler("verify-vitals", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with player vitals data")]
        public static void HandleVerifyVitals(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            foreach (var player in players)
            {
                var updated = false;

                foreach (var vital in new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>(player.Biota.PropertiesAttribute2nd))
                {
                    // ensure this is a valid MaxVital
                    if (vital.Key != PropertyAttribute2nd.MaxHealth && vital.Key != PropertyAttribute2nd.MaxStamina && vital.Key != PropertyAttribute2nd.MaxMana)
                    {
                        Console.WriteLine($"{player.Name} has unknown vita {vital.Key}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            // i have found no instances of this situation being run into,
                            // but if it does happen, verify-xp will refund the player xp properly

                            player.Biota.PropertiesAttribute2nd.Remove(vital.Key);
                            updated = true;
                        }
                        continue;
                    }

                    var rank = vital.Value.LevelFromCP;

                    // verify vital rank
                    var correctRank = Player.CalcVitalRank(vital.Value.CPSpent);
                    if (rank != correctRank)
                    {
                        Console.WriteLine($"{player.Name}'s {vital.Key} rank is {rank}, should be {correctRank}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            vital.Value.LevelFromCP = (ushort)correctRank;
                            updated = true;
                        }
                    }

                    // verify vital xp is within bounds
                    var vitalXPTable = DatManager.PortalDat.XpTable.VitalXpList;
                    var maxVitalXp = vitalXPTable[vitalXPTable.Count - 1];

                    if (vital.Value.CPSpent > maxVitalXp)
                    {
                        Console.WriteLine($"{player.Name}'s {vital.Key} vital total xp is {vital.Value.CPSpent:N0}, should be capped at {maxVitalXp:N0}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            // again i have found no instances of this situation being run into,
                            // but if it does happen, verify-xp will refund the player xp properly

                            vital.Value.CPSpent = maxVitalXp;
                            updated = true;
                        }
                    }
                }

                if (updated)
                    player.SaveBiotaToDatabase();
            }

            if (!fix && foundIssues)
                Console.WriteLine($"Dry run completed. Type 'verify-vitals fix' to fix any issues.");

            if (!foundIssues)
                Console.WriteLine($"Verified vitals for {players.Count:N0} players");
        }

        [CommandHandler("verify-skills", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with player skill data")]
        public static void HandleVerifySkills(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            foreach (var player in players)
            {
                var updated = false;

                foreach (var skill in new Dictionary<Skill, PropertiesSkill>(player.Biota.PropertiesSkill))
                {
                    // ensure this is a valid player skill
                    if (!Player.PlayerSkills.Contains(skill.Key))
                    {
                        Console.WriteLine($"{player.Name} has unknown skill {skill.Key}{fixStr}");
                        foundIssues = true;
                        if (fix)
                        {
                            // i have found no instances of these skills ever having xp put into them,
                            // but if there were, verify-xp will fix that
                            player.Biota.PropertiesSkill.Remove(skill.Key);
                            updated = true;
                        }
                        continue;
                    }

                    var rank = skill.Value.LevelFromPP;

                    var sac = skill.Value.SAC;
                    if (sac < SkillAdvancementClass.Trained)
                    {
                        if (skill.Value.PP > 0 || skill.Value.LevelFromPP > 0)
                        {
                            Console.WriteLine($"{player.Name} has {sac} skill {skill.Key} with {skill.Value.PP:N0} xp (rank {skill.Value.LevelFromPP}){fixStr}");
                            foundIssues = true;

                            if (fix)
                            {
                                // i have found no instances of this situation being run into,
                                // but if it does happen, verify-xp will refund the player xp properly
                                skill.Value.PP = 0;
                                skill.Value.LevelFromPP = 0;

                                updated = true;
                            }
                        }
                        continue;
                    }

                    if (sac != SkillAdvancementClass.Specialized)
                    {
                        if (skill.Value.InitLevel > 0)
                        {
                            Console.WriteLine($"{player.Name} has {sac} skill {skill.Key} with {skill.Value.InitLevel:N0} InitLevel{fixStr}");
                            foundIssues = true;

                            if (fix)
                            {
                                skill.Value.InitLevel = 0;

                                updated = true;
                            }
                        }
                    }
                    else
                    {
                        var augProp = 0;
                        var augType = AugmentationType.None;
                        switch (skill.Key)
                        {

                            case Skill.ArmorTinkering:
                                augType = AugmentationType.ArmorTinkering;
                                augProp = player.GetProperty(PropertyInt.AugmentationSpecializeArmorTinkering) ?? 0;
                                break;

                            case Skill.ItemTinkering:
                                augType = AugmentationType.ItemTinkering;
                                augProp = player.GetProperty(PropertyInt.AugmentationSpecializeItemTinkering) ?? 0;
                                break;

                            case Skill.MagicItemTinkering:
                                augType = AugmentationType.MagicItemTinkering;
                                augProp = player.GetProperty(PropertyInt.AugmentationSpecializeMagicItemTinkering) ?? 0;
                                break;

                            case Skill.WeaponTinkering:
                                augType = AugmentationType.WeaponTinkering;
                                augProp = player.GetProperty(PropertyInt.AugmentationSpecializeWeaponTinkering) ?? 0;
                                break;

                            case Skill.Salvaging:
                                augType = AugmentationType.Salvage;
                                augProp = player.GetProperty(PropertyInt.AugmentationSpecializeSalvaging) ?? 0;
                                break;
                        }

                        if (skill.Value.InitLevel != 10 && augProp == 0)
                        {
                            Console.WriteLine($"{player.Name} has {sac} skill {skill.Key} with {skill.Value.InitLevel:N0} InitLevel{fixStr}");
                            foundIssues = true;

                            if (fix)
                            {
                                skill.Value.InitLevel = 10;

                                updated = true;
                            }
                        }
                        else if (skill.Value.InitLevel == 10 && augProp == 1)
                        {
                            Console.WriteLine($"{player.Name} has {sac} skill {skill.Key} with {skill.Value.InitLevel:N0} InitLevel as a result of {augType} augmentation{fixStr}");
                            foundIssues = true;

                            if (fix)
                            {
                                skill.Value.InitLevel = 0;

                                updated = true;
                            }
                        }
                    }

                    // verify skill rank
                    var correctRank = Player.CalcSkillRank(sac, skill.Value.PP);
                    if (rank != correctRank)
                    {
                        Console.WriteLine($"{player.Name}'s {skill.Key} rank is {rank}, should be {correctRank}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            skill.Value.LevelFromPP = (ushort)correctRank;
                            updated = true;
                        }
                    }

                    // verify skill xp is within bounds

                    // in retail, if a player had a trained skill maxed out, and then they speced it in spec temple,
                    // they would sort of temporarily 'lose' that ~103m xp, unless they reset the trained skill, and then speced it

                    // so the data can be in a legit situation here where a character has a skill speced,
                    // but their xp is beyond the spec xp cap (4,100,490,438) and <= the trained xp cap (4,203,819,496)

                    //var skillXPTable = Player.GetSkillXPTable(sac);
                    var skillXPTable = Player.GetSkillXPTable(SkillAdvancementClass.Trained);
                    var maxSkillXp = skillXPTable[skillXPTable.Count - 1];

                    if (skill.Value.PP > maxSkillXp)
                    {
                        Console.WriteLine($"{player.Name}'s {sac} {skill.Key} skill total xp is {skill.Value.PP:N0}, should be capped at {maxSkillXp:N0}{fixStr}");
                        foundIssues = true;
                        if (fix)
                        {
                            // again i have found no instances of this situation being run into,
                            // but if it does happen, verify-xp will refund the player xp properly
                            skill.Value.PP = maxSkillXp;
                            updated = true;
                        }
                    }
                }

                if (fix && updated)
                    player.SaveBiotaToDatabase();
            }

            if (!fix && foundIssues)
                Console.WriteLine($"Dry run completed. Type 'verify-skills fix' to fix any issues.");

            if (!foundIssues)
                Console.WriteLine($"Verified skills for {players.Count:N0} players");
        }

        [CommandHandler("verify-skill-credits", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with player skill credits")]
        public static void HandleVerifySkillCredits(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            using (var ctx = new ShardDbContext())
            {
                foreach (var player in players)
                {
                    // skip admins
                    if (player.Account == null || player.Account.AccessLevel == (uint)AccessLevel.Admin)
                        continue;

                    // player starts with 52 skill credits
                    var startCredits = 52;

                    // skills that cannot be untrained: arcane lore, jump, loyalty, magic defense, run, salvaging
                    // all of these have '0' cost to train, except for arcane lore, which has 4 (seems to be an outlier?)
                    startCredits += 4;

                    var levelCredits = GetAdditionalCredits(player.Level ?? 1);

                    var totalCredits = startCredits + levelCredits;

                    var used = 0;

                    foreach (var skill in new Dictionary<Skill, PropertiesSkill>(player.Biota.PropertiesSkill))
                    {
                        var sac = skill.Value.SAC;
                        if (sac < SkillAdvancementClass.Trained)
                            continue;

                        if (!DatManager.PortalDat.SkillTable.SkillBaseHash.TryGetValue((uint)skill.Key, out var skillInfo))
                        {
                            Console.WriteLine($"{player.Name}.HandleVerifySkillCredits({skill.Key}): unknown skill");
                            continue;
                        }

                        //Console.WriteLine($"{(Skill)skill.Type} trained cost: {skillInfo.TrainedCost}, spec cost: {skillInfo.SpecializedCost}");

                        used += skillInfo.TrainedCost;

                        if (sac == SkillAdvancementClass.Specialized)
                        {
                            switch (skill.Key)
                            {
                                // these can only be speced through augs, they have >= 999 in the spec data
                                case Skill.ArmorTinkering:
                                case Skill.ItemTinkering:
                                case Skill.MagicItemTinkering:
                                case Skill.WeaponTinkering:
                                case Skill.Salvaging:
                                    continue;
                            }

                            used += skillInfo.UpgradeCostFromTrainedToSpecialized;
                        }
                    }

                    // 2 possible skill credits from quests
                    // - ChasingOswaldDone
                    // - ArantahKill1 (no 'turned in' stamp, only if given figurine?)
                    var questCredits = ctx.CharacterPropertiesQuestRegistry.Count(i => i.CharacterId == player.Guid.Full && (i.QuestName.Equals("ChasingOswaldDone") || i.QuestName.Equals("ArantahKill1")));

                    totalCredits += questCredits;

                    // TODO: 2 lum augs

                    var targetCredits = totalCredits - used;
                    var targetMsg = $"{player.Name} should have {targetCredits} available skill credits";

                    if (targetCredits < 0)
                    {
                        // if the player has already spent more skill credits than they should have,
                        // unfortunately this situation requires a partial reset..

                        Console.WriteLine($"{targetMsg}. To fix this situation, trained skill reset will need to be applied{fixStr}");
                        foundIssues = true;

                        if (fix)
                            UntrainSkills(player, targetCredits);

                        continue;
                    }

                    var availableCredits = player.GetProperty(PropertyInt.AvailableSkillCredits) ?? 0;

                    if (availableCredits != targetCredits)
                    {
                        Console.WriteLine($"{targetMsg}, but they have {availableCredits}{fixStr}");
                        foundIssues = true;

                        if (fix)
                        {
                            player.SetProperty(PropertyInt.AvailableSkillCredits, targetCredits);
                            player.SaveBiotaToDatabase();
                        }
                    }
                }
            }
            if (!fix && foundIssues)
                Console.WriteLine($"Dry run completed. Type 'verify-skill-credits fix' to fix any issues.");

            if (!foundIssues)
                Console.WriteLine($"Verified skill credits for {players.Count:N0} players");
        }

        public static int GetAdditionalCredits(int level)
        {
            foreach (var kvp in AdditionalCredits.Reverse())
                if (level >= kvp.Key)
                    return kvp.Value;

            return 0;
        }

        /// <summary>
        /// level => total additional credits
        /// </summary>
        public static SortedDictionary<int, int> AdditionalCredits = new SortedDictionary<int, int>()
        {
            { 2, 1 },
            { 3, 2 },
            { 4, 3 },
            { 5, 4 },
            { 6, 5 },
            { 7, 6 },
            { 8, 7 },
            { 9, 8 },
            { 10, 9 },
            { 12, 10 },
            { 14, 11 },
            { 16, 12 },
            { 18, 13 },
            { 20, 14 },
            { 23, 15 },
            { 26, 16 },
            { 29, 17 },
            { 32, 18 },
            { 35, 19 },
            { 40, 20 },
            { 45, 21 },
            { 50, 22 },
            { 55, 23 },
            { 60, 24 },
            { 65, 25 },
            { 70, 26 },
            { 75, 27 },
            { 80, 28 },
            { 85, 29 },
            { 90, 30 },
            { 95, 31 },
            { 100, 32 },
            { 105, 33 },
            { 110, 34 },
            { 115, 35 },
            { 120, 36 },
            { 125, 37 },
            { 130, 38 },
            { 140, 39 },
            { 150, 40 },
            { 160, 41 },
            { 180, 42 },
            { 200, 43 },
            { 225, 44 },
            { 250, 45 },
            { 275, 46 }
        };

        /// <summary>
        /// This method is only required in the rare situation when the amount of available skill credits
        /// a player should have is negative.
        /// </summary>
        private static void UntrainSkills(OfflinePlayer player, int targetCredits)
        {
            long refundXP = 0;

            foreach (var skill in new Dictionary<Skill, PropertiesSkill>(player.Biota.PropertiesSkill))
            {
                if (!DatManager.PortalDat.SkillTable.SkillBaseHash.TryGetValue((uint)skill.Key, out var skillBase))
                {
                    Console.WriteLine($"{player.Name}.UntrainSkills({skill.Key}) - unknown skill");
                    continue;
                }

                var sac = skill.Value.SAC;

                if (sac != SkillAdvancementClass.Trained || !Player.IsSkillUntrainable(skill.Key))
                    continue;

                refundXP += skill.Value.PP;

                skill.Value.SAC = SkillAdvancementClass.Untrained;
                skill.Value.InitLevel = 0;
                skill.Value.PP = 0;
                skill.Value.LevelFromPP = 0;

                targetCredits += skillBase.TrainedCost;
            }

            var availableExperience = player.GetProperty(PropertyInt64.AvailableExperience) ?? 0;

            player.SetProperty(PropertyInt64.AvailableExperience, availableExperience + refundXP);

            player.SetProperty(PropertyInt.AvailableSkillCredits, targetCredits);

            player.SetProperty(PropertyBool.UntrainedSkills, true);

            player.SaveBiotaToDatabase();
        }


        [CommandHandler("verify-heritage-augs", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies all players have their heritage augs.")]
        public static void HandleVerifyHeritageAugs(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            foreach (var player in players)
            {
                var heritage = (HeritageGroup?)player.GetProperty(PropertyInt.HeritageGroup);
                if (heritage == null)
                {
                    Console.WriteLine($"Couldn't find heritage for {player.Name}");
                    continue;
                }

                var heritageAug = GetHeritageAug(heritage.Value);
                if (heritageAug == null)
                {
                    Console.WriteLine($"Couldn't find heritage aug for {heritage} player {player.Name}");
                    continue;
                }

                var numAugs = player.GetProperty(heritageAug.Value) ?? 0;
                if (numAugs < 1)
                {
                    Console.WriteLine($"{heritageAug}={numAugs} for {heritage} player {player.Name}{fixStr}");
                    foundIssues = true;

                    if (fix)
                    {
                        player.SetProperty(heritageAug.Value, 1);
                        player.SaveBiotaToDatabase();
                    }
                }
            }
            if (!fix && foundIssues)
                Console.WriteLine($"Dry run completed. Type 'verify-heritage-augs fix' to fix any issues.");

            if (!foundIssues)
                Console.WriteLine($"Verified heritage augs for {players.Count:N0} players");
        }


        public static PropertyInt? GetHeritageAug(HeritageGroup heritage)
        {
            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                case HeritageGroup.Gharundim:
                case HeritageGroup.Sho:
                case HeritageGroup.Viamontian:
                    return PropertyInt.AugmentationJackOfAllTrades;

                case HeritageGroup.Shadowbound:
                case HeritageGroup.Penumbraen:
                    return PropertyInt.AugmentationCriticalExpertise;

                case HeritageGroup.Gearknight:
                    return PropertyInt.AugmentationDamageReduction;

                case HeritageGroup.Undead:
                    return PropertyInt.AugmentationCriticalDefense;

                case HeritageGroup.Empyrean:
                    return PropertyInt.AugmentationInfusedLifeMagic;

                case HeritageGroup.Tumerok:
                    return PropertyInt.AugmentationCriticalPower;

                case HeritageGroup.Lugian:
                    return PropertyInt.AugmentationIncreasedCarryingCapacity;
            }
            return null;
        }


        [CommandHandler("verify-max-augs", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with the # of augs each player has")]
        public static void HandleVerifyMaxAugs(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            foreach (var player in players)
            {
                foreach (var kvp in AugmentationDevice.AugProps)
                {
                    var type = kvp.Key;
                    var prop = kvp.Value;

                    var max = AugmentationDevice.MaxAugs[type];

                    var augProp = player.GetProperty(prop) ?? 0;

                    if (augProp >= 0 && augProp <= max)
                        continue;

                    var msg = $"{player.Name} has {augProp} {prop}";

                    if (augProp < 0)
                        msg += $", min should be 0{fixStr}";
                    else
                        msg += $", max should be {max}{fixStr}";

                    Console.WriteLine(msg);

                    foundIssues = true;

                    if (fix)
                    {
                        if (augProp < 0)
                            player.SetProperty(prop, 0);
                        else
                            player.SetProperty(prop, max);

                        player.SaveBiotaToDatabase();
                    }
                }
            }
            if (!fix && foundIssues)
                Console.WriteLine($"Dry run completed. Type 'verify-max-augs fix' to fix any issues.");

            if (!foundIssues)
                Console.WriteLine($"Verified max augs for {players.Count:N0} players");
        }


        public static Dictionary<PropertyInt, string> AugmentationDevices = new Dictionary<PropertyInt, string>()
        {
            { PropertyInt.AugmentationBonusImbueChance,             "gemaugmentationluckonimbues" },
            { PropertyInt.AugmentationBonusSalvage,                 "gemaugmentationbonussalvage" },
            { PropertyInt.AugmentationBonusXp,                      "gemaugmentationbonusxp" },
            { PropertyInt.AugmentationCriticalDefense,              "gemaugmentationcriticaldefense" },
            { PropertyInt.AugmentationCriticalExpertise,            "ace41482-eyeoftheremorseless" },
            { PropertyInt.AugmentationCriticalPower,                "ace41481-handoftheremorseless" },
            { PropertyInt.AugmentationDamageBonus,                  "ace41478-frenzyoftheslayer" },
            { PropertyInt.AugmentationDamageReduction,              "ace41480-ironskinoftheinvincible" },
            { PropertyInt.AugmentationExtraPackSlot,                "gemaugmentationpackslot" },
            { PropertyInt.AugmentationFasterRegen,                  "gemaugmentationfastregen" },
            { PropertyInt.AugmentationIncreasedCarryingCapacity,    "gemaugmentationcarryingcapacityi" },
            { PropertyInt.AugmentationIncreasedSpellDuration,       "gemaugmentationspellduration" },
            { PropertyInt.AugmentationInfusedCreatureMagic,         "ace41472-infusedcreaturemagic" },
            { PropertyInt.AugmentationInfusedItemMagic,             "ace41473-infuseditemmagic" },
            { PropertyInt.AugmentationInfusedLifeMagic,             "ace41474-infusedlifemagic" },
            { PropertyInt.AugmentationInfusedVoidMagic,             "ace41479-infusedvoidmagic" },
            { PropertyInt.AugmentationInfusedWarMagic,              "ace41475-infusedwarmagic" },
            { PropertyInt.AugmentationInnateCoordination,           "gemaugmentationattcoordination" },
            { PropertyInt.AugmentationInnateEndurance,              "gemaugmentationattendurance" },
            { PropertyInt.AugmentationInnateFocus,                  "gemaugmentationattfocus" },
            { PropertyInt.AugmentationInnateQuickness,              "gemaugmentationattquickness" },
            { PropertyInt.AugmentationInnateSelf,                   "gemaugmentationattself" },
            { PropertyInt.AugmentationInnateStrength,               "gemaugmentationattstrength" },
            { PropertyInt.AugmentationJackOfAllTrades,              "ace43167-jackofalltrades" },
            { PropertyInt.AugmentationLessDeathItemLoss,            "gemaugmentationdeathreduceditems" },
            { PropertyInt.AugmentationResistanceAcid,               "gemaugmentationnaturalresistanceacid" },
            { PropertyInt.AugmentationResistanceBlunt,              "gemaugmentationnaturalresistancebludg" },
            { PropertyInt.AugmentationResistanceFire,               "gemaugmentationnaturalresistancefire" },
            { PropertyInt.AugmentationResistanceFrost,              "gemaugmentationnaturalresistancefrost" },
            { PropertyInt.AugmentationResistanceLightning,          "gemaugmentationnaturalresistanceelectric" },
            { PropertyInt.AugmentationResistancePierce,             "gemaugmentationnaturalresistancepierc" },
            { PropertyInt.AugmentationResistanceSlash,              "gemaugmentationnaturalresistanceslash" },
            { PropertyInt.AugmentationSkilledMagic,                 "ace41476-masterofthefivefoldpath" },
            { PropertyInt.AugmentationSkilledMelee,                 "ace41477-masterofthesteelcircle" },
            { PropertyInt.AugmentationSkilledMissile,               "ace41490-masterofthefocusedeye" },
            { PropertyInt.AugmentationSpecializeArmorTinkering,     "gemaugmentationtinkeringspecarmor" },
            { PropertyInt.AugmentationSpecializeItemTinkering,      "gemaugmentationtinkeringspecitem" },
            { PropertyInt.AugmentationSpecializeMagicItemTinkering, "gemaugmentationtinkeringspecmagic" },
            { PropertyInt.AugmentationSpecializeSalvaging,          "gemaugmentationtinkeringspecsalv" },
            { PropertyInt.AugmentationSpecializeWeaponTinkering,    "gemaugmentationtinkeringspecweap" },
            { PropertyInt.AugmentationSpellsRemainPastDeath,        "gemaugmentationdeathspellsremain" },
        };


        [CommandHandler("verify-xp", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any bugs with player xp")]
        public static void HandleVerifyExperience(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllOffline();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";
            var foundIssues = false;

            var results = new List<VerifyXpResult>();

            foreach (var player in players)
            {
                var totalXP = player.GetProperty(PropertyInt64.TotalExperience) ?? 0;
                var unassignedXP = player.GetProperty(PropertyInt64.AvailableExperience) ?? 0;

                // loop through all attributes/vitals/skills, add up assigned xp
                long attributeXP = 0;
                long vitalXP = 0;
                long skillXP = 0;
                long augXP = 0;

                long diffXP = Math.Min(0, player.GetProperty(PropertyInt64.VerifyXp) ?? 0);

                foreach (var attribute in player.Biota.PropertiesAttribute)
                    attributeXP += attribute.Value.CPSpent;

                foreach (var vital in player.Biota.PropertiesAttribute2nd)
                    vitalXP += vital.Value.CPSpent;

                foreach (var skill in player.Biota.PropertiesSkill)
                    skillXP += skill.Value.PP;

                // find any xp spent on augs
                var heritage = (HeritageGroup?)player.GetProperty(PropertyInt.HeritageGroup);
                if (heritage == null)
                    continue;       // ignore admins who have morphed into asheron / bael'zharon

                var heritageAug = GetHeritageAug(heritage.Value);

                foreach (var kvp in AugmentationDevices)
                {
                    var augProperty = kvp.Key;

                    var numAugs = player.GetProperty(augProperty) ?? 0;
                    if (augProperty == heritageAug)
                        numAugs--;

                    if (numAugs <= 0)
                        continue;

                    var aug = DatabaseManager.World.GetCachedWeenie(kvp.Value);
                    aug.PropertiesInt64.TryGetValue(PropertyInt64.AugmentationCost, out var costPer);

                    augXP += costPer * numAugs;
                }

                var calculatedSpent = attributeXP + vitalXP + skillXP + augXP + diffXP;

                var currentSpent = totalXP - unassignedXP;

                var bonusXp = (currentSpent - calculatedSpent) % 526;

                if (calculatedSpent != currentSpent && bonusXp != 0)
                {
                    // the results for this data set can be large,
                    // especially due to an earlier ace bug where it wasn't calculating the Proficiency Points correctly

                    // instead of displaying the results in random order,
                    // we going to sort them all by diff

                    foundIssues = true;
                    results.Add(new VerifyXpResult(player, calculatedSpent, currentSpent));
                }
            }

            var xpList = DatManager.PortalDat.XpTable.CharacterLevelXPList;
            var maxTotalXp = (long)xpList[xpList.Count - 1];

            foreach (var result in results.OrderBy(i => i.Player.Name).OrderBy(i => i.Diff))
            {
                var player = result.Player;
                var diff = result.Diff;

                Console.WriteLine($"{player.Name} is calculated to have spent {result.Calculated:N0} experience, which currently differs by {diff:N0}{fixStr}");

                if (!fix)
                    continue;

                if (diff > 0)
                {
                    // add to unassigned xp
                    var unassignedXP = player.GetProperty(PropertyInt64.AvailableExperience) ?? 0;
                    player.SetProperty(PropertyInt64.AvailableExperience, unassignedXP + diff);
                    player.SetProperty(PropertyInt64.VerifyXp, diff);
                }
                else
                {
                    var totalXP = player.GetProperty(PropertyInt64.TotalExperience) ?? 0;
                    if (totalXP - diff > maxTotalXp)
                    {
                        var unassignedXP = player.GetProperty(PropertyInt64.AvailableExperience) ?? 0;
                        if (unassignedXP + diff >= 0)
                        {
                            // this is the only (rare) case where subtracting from AvailableExperience is required
                            player.SetProperty(PropertyInt64.AvailableExperience, unassignedXP + diff);
                        }
                        else
                            Console.WriteLine($"ERROR: couldn't fix, xp exceeds all bounds");
                    }
                    else
                    {
                        // setting the diff property below, which will be handled on player login
                        // to properly handle possibly leveling up / skill credits
                        player.SetProperty(PropertyInt64.VerifyXp, diff);
                    }
                }
                player.SaveBiotaToDatabase();
            }

            if (foundIssues)
            {
                Console.WriteLine($"{(fix ? "Fixed" : "Found")} issues for {results.Count:N0} players");

                if (!fix)
                    Console.WriteLine($"Dry run completed. Type 'verify-xp fix' to fix any issues.");
            }
            else
                Console.WriteLine($"Verified XP for {players.Count:N0} players");
        }


        [CommandHandler("fix-biota-emote-delay", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Fixes biota emotes with incorrect default delays")]
        public static void HandleFixBiotaEmoteDelay(Session session, params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "This command is intended to be run while the world is in offline mode, or there are 0 players connected.");
                CommandHandlerHelper.WriteOutputInfo(session, "To run this fix, type fix-biota-emote-delay fix");
                return;
            }

            var fix = parameters[0].Equals("fix", StringComparison.OrdinalIgnoreCase);

            CommandHandlerHelper.WriteOutputInfo(session, "Building weenie emote cache");

            var weenieEmoteCache = BuildWeenieEmoteCache();

            CommandHandlerHelper.WriteOutputInfo(session, $"Found {weenieEmoteCache.Count:N0} weenie templates w/ emote actions with delay 0");

            using (var ctx = new ShardDbContext())
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Finding biotas for these wcids");

                var biotas = ctx.Biota.Where(i => weenieEmoteCache.ContainsKey(i.WeenieClassId)).ToList();

                var distinct = biotas.Select(i => i.WeenieClassId).Distinct();
                var counts = new Dictionary<uint, uint>();
                foreach (var biota in biotas)
                {
                    if (!counts.ContainsKey(biota.WeenieClassId))
                        counts[biota.WeenieClassId] = 1;
                    else
                        counts[biota.WeenieClassId]++;
                }

                CommandHandlerHelper.WriteOutputInfo(session, $"Found {biotas.Count} biotas matching {distinct.Count()} distinct wcids");

                foreach (var kvp in counts.OrderBy(i => i.Key))
                    CommandHandlerHelper.WriteOutputInfo(session, $"{kvp.Key} - {(WeenieClassName)kvp.Key} ({kvp.Value})");

                if (!fix)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Dry run completed");
                    return;
                }

                var totalUpdated = 0;

                foreach (var biota in biotas)
                {
                    bool updated = false;

                    var query = from emote in ctx.BiotaPropertiesEmote
                                join action in ctx.BiotaPropertiesEmoteAction on emote.Id equals action.EmoteId
                                where emote.ObjectId == biota.Id && action.Delay == 1.0f
                                select new
                                {
                                    Emote = emote,
                                    Action = action
                                };

                    var emoteActions = query.ToList();

                    foreach (var emoteAction in emoteActions)
                    {
                        var emote = emoteAction.Emote;
                        var action = emoteAction.Action;

                        // ensure this delay 1 should be delay 0
                        var hash = CalculateEmoteHash(emote);
                        var weenieEmotes = weenieEmoteCache[biota.WeenieClassId];
                        if (!weenieEmotes.TryGetValue(hash, out var list))
                        {
                            //CommandHandlerHelper.WriteOutputInfo(session, $"Skipping emote for {biota.WeenieClassId} not found in hash list");
                            continue;
                        }
                        if (!list.Contains(action.Order))
                        {
                            //CommandHandlerHelper.WriteOutputInfo(session, $"Skipping emote for {biota.WeenieClassId} not found in action list");
                            continue;
                        }

                        // confirmed match, update delay 1 -> 0
                        action.Delay = 0.0f;
                        updated = true;
                    }

                    if (updated)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Fixed shard object {biota.Id:X8} of type {biota.WeenieClassId} - {(WeenieClassName)biota.WeenieClassId}");
                        totalUpdated++;
                    }
                }
                ctx.SaveChanges();

                CommandHandlerHelper.WriteOutputInfo(session, $"Completed successfully, fixed {totalUpdated} shard items");
            }
        }

        private static Dictionary<uint, Dictionary<int, HashSet<uint>>> BuildWeenieEmoteCache()
        {
            /// wcid => emote hash => list of order ids with delay 0
            var emoteCache = new Dictionary<uint, Dictionary<int, HashSet<uint>>>();

            using (var ctx = new WorldDbContext())
            {
                var query = from emote in ctx.WeeniePropertiesEmote
                            join action in ctx.WeeniePropertiesEmoteAction on emote.Id equals action.EmoteId
                            where action.Delay == 0.0f
                            select new
                            {
                                Emote = emote,
                                Action = action
                            };

                var emoteActions = query.ToList();

                foreach (var emoteAction in emoteActions)
                {
                    var emote = emoteAction.Emote;
                    var action = emoteAction.Action;

                    var wcid = emote.ObjectId;

                    if (!emoteCache.TryGetValue(wcid, out var hashTable))
                    {
                        hashTable = new Dictionary<int, HashSet<uint>>();
                        emoteCache.Add(wcid, hashTable);
                    }

                    var emoteHash = CalculateEmoteHash(emote);

                    if (!hashTable.TryGetValue(emoteHash, out var actionIdx))
                    {
                        actionIdx = new HashSet<uint>();
                        hashTable.Add(emoteHash, actionIdx);
                    }

                    actionIdx.Add(action.Order);
                }
            }

            return emoteCache;
        }

        private static int CalculateEmoteHash(WeeniePropertiesEmote emote)
        {
            return CalculateEmoteHash(emote.Category, emote.Probability, emote.WeenieClassId, emote.Style, emote.Substyle, emote.Quest, emote.VendorType);
        }

        private static int CalculateEmoteHash(BiotaPropertiesEmote emote)
        {
            return CalculateEmoteHash(emote.Category, emote.Probability, emote.WeenieClassId, emote.Style, emote.Substyle, emote.Quest, emote.VendorType);
        }

        private static int CalculateEmoteHash(uint category, float probability, uint? wcid, uint? style, uint? substyle, string quest, int? vendorType)
        {
            // no illegitimate collisions found that require doing exact equality comparison as of 11/23/2019 emote data

            int hash = 0;

            hash = (hash * 397) ^ category.GetHashCode();
            hash = (hash * 397) ^ probability.GetHashCode();

            if (wcid != null)
                hash = (hash * 397) ^ wcid.GetHashCode();

            if (style != null)
                hash = (hash * 397) ^ style.GetHashCode();

            if (substyle != null)
                hash = (hash * 397) ^ substyle.GetHashCode();

            if (quest != null)
                hash = (hash * 397) ^ quest.GetHashCode();

            if (vendorType != null)
                hash = (hash * 397) ^ vendorType.GetHashCode();

            return hash;
        }

        [CommandHandler("verify-armor-levels", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Verifies and optionally fixes any existing armor levels above AL cap")]
        public static void HandleFixArmorLevel(Session session, params string[] parameters)
        {
            Console.WriteLine($"Fetching shard armors (this may take awhile on large servers) ...");

            var resistMagic = GetResistMagic();
            var tinkerLogs = GetTinkerLogs();
            var numTimesTinkered = GetNumTimesTinkered();
            var imbuedEffects = GetImbuedEffect();

            var fix = parameters.Length > 0 && parameters[0].Equals("fix");
            var fixStr = fix ? " -- fixed" : "";

            // get all loot-generated items on server with armor level
            using (var ctx = new ShardDbContext())
            {
                ctx.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));

                var query = from armor in ctx.BiotaPropertiesInt
                            join workmanship in ctx.BiotaPropertiesInt on armor.ObjectId equals workmanship.ObjectId
                            join name in ctx.BiotaPropertiesString on armor.ObjectId equals name.ObjectId
                            join validLocations in ctx.BiotaPropertiesInt on armor.ObjectId equals validLocations.ObjectId
                            where armor.Type == (int)PropertyInt.ArmorLevel && workmanship.Type == (int)PropertyInt.ItemWorkmanship && name.Type == (int)PropertyString.Name && validLocations.Type == (int)PropertyInt.ValidLocations
                            orderby armor.Value descending
                            select new
                            {
                                Guid = armor.ObjectId,
                                ArmorLevel = armor.Value,
                                Name = name.Value,
                                ValidLocs = validLocations.Value,
                                Armor = armor
                            };

                var armorItems = query.ToList();

                var adjusted = 0;

                foreach (var armorItem in armorItems)
                {
                    // ignore unenchantable
                    if (resistMagic.TryGetValue(armorItem.Guid, out var resist) && resist == 9999)
                        continue;

                    TinkerLog tinkerLog = null;
                    if (tinkerLogs.TryGetValue(armorItem.Guid, out var _tinkerLog))
                        tinkerLog = new TinkerLog(_tinkerLog);

                    numTimesTinkered.TryGetValue(armorItem.Guid, out var numTinkers);
                    imbuedEffects.TryGetValue(armorItem.Guid, out var imbuedEffect);

                    var numArmorTinkers = tinkerLog != null ? tinkerLog.NumTinkers(MaterialType.Steel) : 0;

                    var numArmorTinkerStr = numArmorTinkers > 0 ? $" ({numArmorTinkers})" : "";

                    var equipMask = (EquipMask)armorItem.ValidLocs;

                    var newArmorLevel = GetArmorLevel(armorItem.ArmorLevel, equipMask, tinkerLog, numTinkers, imbuedEffect);

                    if (newArmorLevel != armorItem.ArmorLevel)
                    {
                        if (fix)
                            armorItem.Armor.Value = newArmorLevel;

                        Console.WriteLine($"{armorItem.Name}, {armorItem.ArmorLevel}{numArmorTinkerStr} => {newArmorLevel}{fixStr}");

                        adjusted++;
                    }
                }
                if (fix)
                    ctx.SaveChanges();

                var willBe = fix ? " " : " will be ";

                if (adjusted > 0)
                {
                    Console.WriteLine($"Found {armorItems.Count:N0} armors, {adjusted:N0}{willBe}adjusted");

                    if (!fix)
                        Console.WriteLine($"Dry run completed. Type 'verify-armor-levels fix' to fix any issues.");
                }
                else
                    Console.WriteLine($"Verified {armorItems.Count:N0} armors.");
            }
        }

        public static Dictionary<uint, int> GetResistMagic()
        {
            using (var ctx = new ShardDbContext())
            {
                var resistMagic = ctx.BiotaPropertiesInt.Where(i => i.Type == (int)PropertyInt.ResistMagic).ToDictionary(i => i.ObjectId, i => i.Value);

                return resistMagic;
            }
        }

        public static Dictionary<uint, string> GetTinkerLogs()
        {
            using (var ctx = new ShardDbContext())
            {
                var tinkerLogs = ctx.BiotaPropertiesString.Where(i => i.Type == (int)PropertyString.TinkerLog).ToDictionary(i => i.ObjectId, i => i.Value);

                return tinkerLogs;
            }
        }

        public static Dictionary<uint, int> GetNumTimesTinkered()
        {
            using (var ctx = new ShardDbContext())
            {
                var numTimesTinkered = ctx.BiotaPropertiesInt.Where(i => i.Type == (int)PropertyInt.NumTimesTinkered).ToDictionary(i => i.ObjectId, i => i.Value);

                return numTimesTinkered;
            }
        }

        public static Dictionary<uint, int> GetImbuedEffect()
        {
            using (var ctx = new ShardDbContext())
            {
                var imbuedEffect = ctx.BiotaPropertiesInt.Where(i => i.Type == (int)PropertyInt.ImbuedEffect).ToDictionary(i => i.ObjectId, i => i.Value);

                return imbuedEffect;
            }
        }

        // head / hands / feet
        public static readonly int MaxArmorLevel_Extremity = 345;

        // everything else
        public static readonly int MaxArmorLevel_NonExtremity = 315;

        public static int GetArmorLevel(int armorLevel, EquipMask equipMask, TinkerLog tinkerLog, int numTinkers, int imbuedEffect)
        {
            var maxArmorLevel = (equipMask & EquipMask.Extremity) != 0 ? MaxArmorLevel_Extremity : MaxArmorLevel_NonExtremity;

            if (tinkerLog != null && tinkerLog.Tinkers.Count == numTinkers)
            {
                // full tinkering log available
                maxArmorLevel += tinkerLog.NumTinkers(MaterialType.Steel) * 20;
            }
            else if (numTinkers > 0)
            {
                // partial or no tinkering log available
                var rngMax = numTinkers;
                if (imbuedEffect != 0)
                    rngMax--;

                if (rngMax > 0)
                {
                    // prevent further iterations on multiple re-runs
                    if (armorLevel <= maxArmorLevel + rngMax * 20)
                        return armorLevel;

                    var rng = ThreadSafeRandom.Next(0, rngMax);
                    maxArmorLevel += rng * 20;
                }
            }
            return Math.Min(armorLevel, maxArmorLevel);
        }
    }
}
