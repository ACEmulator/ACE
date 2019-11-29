using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;

namespace ACE.Server.Command.Handlers
{
    public class DeveloperFixCommands
    {
        [CommandHandler("fix-biota-emote-delay", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 0, "Fixes biota emotes with incorrect default delays")]
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
    }
}
