using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class Advocate
    {
        public const Channel AdvocateChannels = Channel.Help | Channel.Abuse | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3;

        public static bool Bestow(WorldObject playerToBeBestowed, int advocateLevelToBestow)
        {
            if (advocateLevelToBestow < 1 || advocateLevelToBestow > 7)
                return false;

            var player = playerToBeBestowed as Player;

            if (player == null) return false;

            if (!player.AdvocateQuest)
            {
                player.AdvocateQuest = true;

                if (!HasAdvocateTome(player))
                {
                    var useCreateItem = WorldObjectFactory.CreateNewWorldObject("bookadvocatefane");

                    if (useCreateItem != null)
                        player.TryCreateInInventoryWithNetworking(useCreateItem);
                }
            }

            player.IsAdvocate = true;

            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(player, PropertyBool.IsAdvocate, player.IsAdvocate));

            if (player.AdvocateLevel.HasValue)
            {
                var objectsToBeConsumed = new List<WorldObject>();

                objectsToBeConsumed.AddRange(player.GetInventoryItemsOfWeenieClass($"shieldadvocate{player.AdvocateLevel}"));

                foreach (var wo in objectsToBeConsumed)
                    player.TryConsumeFromInventoryWithNetworking(wo);

                var equippedAegis = new List<WorldObject>();
                equippedAegis.AddRange(player.EquippedObjects.Values.Where(a => a.WeenieClassName.Equals($"shieldadvocate{player.AdvocateLevel}", StringComparison.OrdinalIgnoreCase)).ToList());

                foreach (var wo in equippedAegis)
                    player.TryDequipObjectWithNetworking(wo.Guid, out _, Player.DequipObjectAction.ConsumeItem);
            }

            player.AdvocateLevel = advocateLevelToBestow;
            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AdvocateLevel, player.AdvocateLevel ?? 1));

            if (player.AdvocateLevel > 4)
            {
                player.IsPsr = true;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(player, PropertyBool.IsPsr, player.IsPsr));
            }

            if (!HasAdvocateInstructions(player))
            {
                var useCreateItem = WorldObjectFactory.CreateNewWorldObject("bookadvocateinstructions");

                if (useCreateItem != null)
                    player.TryCreateInInventoryWithNetworking(useCreateItem);
            }

            var useCreateAegis = WorldObjectFactory.CreateNewWorldObject($"shieldadvocate{player.AdvocateLevel.Value}");

            if (useCreateAegis != null)
                player.TryCreateInInventoryWithNetworking(useCreateAegis);

            player.ChannelsAllowed = AdvocateChannels | Channel.TownChans;
            player.ChannelsActive  = AdvocateChannels | Channel.TownChans;

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been bestowed as an Advocate, level {player.AdvocateLevel}!", ChatMessageType.Broadcast));

            return true;
        }

        public static bool Remove(WorldObject playerToBeRemoved)
        {
            var player = playerToBeRemoved as Player;

            if (player == null) return false;

            if (!player.AdvocateQuest)
                return false;

            var objectsToBeConsumed = new List<WorldObject>();

            objectsToBeConsumed.AddRange(player.GetInventoryItemsOfWeenieClass("bookadvocatefane"));
            objectsToBeConsumed.AddRange(player.GetInventoryItemsOfWeenieClass("bookadvocateinstructions"));

            for (var i = 1; i < 8; i++)
                objectsToBeConsumed.AddRange(player.GetInventoryItemsOfWeenieClass($"shieldadvocate{i}"));

            foreach (var wo in objectsToBeConsumed)
                player.TryConsumeFromInventoryWithNetworking(wo);

            var equippedAegis = new List<WorldObject>();
            for (var i = 1; i < 8; i++)
                equippedAegis.AddRange(player.EquippedObjects.Values.Where(a => a.WeenieClassName.Equals($"shieldadvocate{i}", StringComparison.OrdinalIgnoreCase)).ToList());

            foreach (var wo in equippedAegis)
                player.TryDequipObjectWithNetworking(wo.Guid, out _, Player.DequipObjectAction.ConsumeItem);

            player.ChannelsActive = null;
            player.ChannelsAllowed = null;

            player.AdvocateLevel = null;
            player.IsAdvocate = false;
            player.IsPsr = false;

            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AdvocateLevel, 0));
            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(player, PropertyBool.IsAdvocate, false));
            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(player, PropertyBool.IsPsr, false));

            player.AdvocateQuest = false;

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been removed from the Advocate ranks!", ChatMessageType.Broadcast));

            return true;
        }

        public static bool IsAdvocateFane(WorldObject wo)
        {
            return wo.WeenieType == WeenieType.AdvocateFane;
        }

        public static bool IsAdvocate(WorldObject wo)
        {
            if (wo is Player player)
                return player.AdvocateQuest;

            return false;
        }

        public static bool HasAdvocateTome(Player player)
        {
            return player.GetNumInventoryItemsOfWeenieClass("bookadvocatefane") > 0;
        }

        public static bool HasAdvocateInstructions(Player player)
        {
            return player.GetNumInventoryItemsOfWeenieClass("bookadvocateinstructions") > 0;
        }

        public static bool HasItem(Player player, uint weenieClassId)
        {
            return player.GetNumInventoryItemsOfWCID(weenieClassId) > 0;
        }

        public static readonly Dictionary<string, uint> AdvocateBooks = new Dictionary<string, uint>()
        {
            { "bookadvocatefane",           3653 },
            { "bookadvocateinstructions",   3941 },
        };

        public static readonly Dictionary<string, uint> AdvocateItems = new Dictionary<string, uint>()
        {
            { "shieldadvocate1",            2628 },
            { "shieldadvocate2",            2629 },
            { "shieldadvocate3",            2630 },
            { "shieldadvocate4",            2631 },
            { "shieldadvocate5",            2632 },
            { "shieldadvocate6",            2633 },
            { "shieldadvocate7",            3594 }
        };

        public static bool CanAcceptAdvocateItems(Player player, int advocateLevel)
        {
            var itemsToReceive = new ItemsToReceive(player);

            foreach (var item in AdvocateBooks)
            {
                if (HasItem(player, item.Value))
                    continue;

                itemsToReceive.Add(item.Value, 1);
            }

            if (player.AdvocateLevel.HasValue)
            {
                var currentShieldWcid = AdvocateItems["shieldadvocate" + player.AdvocateLevel.Value];

                if (HasItem(player, currentShieldWcid))
                {
                    var currentShield = player.GetInventoryItemsOfWCID(currentShieldWcid);

                    foreach (var aegis in currentShield)
                        itemsToReceive.Remove(aegis.WeenieClassId, 1);
                }
            }

            itemsToReceive.Add(AdvocateItems["shieldadvocate" + advocateLevel], 1);

            if (itemsToReceive.PlayerExceedsLimits)
                return false;

            return true;
        }
    }
}
