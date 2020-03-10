using System;
using System.Collections.Generic;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Allegiance helper methods
    /// </summary>
    public class AllegianceManager
    {
        /// <summary>
        /// A mapping of all loaded Allegiance GUIDs => their Allegiances
        /// </summary>
        public static readonly Dictionary<ObjectGuid, Allegiance> Allegiances = new Dictionary<ObjectGuid, Allegiance>();

        /// <summary>
        /// A mapping of all Players on the server => their AllegianceNodes
        /// </summary>
        public static readonly Dictionary<ObjectGuid, AllegianceNode> Players = new Dictionary<ObjectGuid, AllegianceNode>();

        /// <summary>
        /// Returns the monarch for a player
        /// </summary>
        public static IPlayer GetMonarch(IPlayer player)
        {
            if (player.MonarchId == null)
                return player;

            var monarch = PlayerManager.FindByGuid(player.MonarchId.Value);

            return monarch ?? player;
        }

        /// <summary>
        /// Returns the full allegiance structure for any player
        /// </summary>
        /// <param name="player">A player at any level of an allegiance</param>
        public static Allegiance GetAllegiance(IPlayer player)
        {
            if (player == null) return null;

            var monarch = GetMonarch(player);

            if (monarch == null) return null;

            // is this allegiance already loaded / cached?
            if (Players.ContainsKey(monarch.Guid))
                return Players[monarch.Guid].Allegiance;

            // try to load biota
            var allegianceID = DatabaseManager.Shard.BaseDatabase.GetAllegianceID(monarch.Guid.Full);
            var biota = allegianceID != null ? DatabaseManager.Shard.BaseDatabase.GetBiota(allegianceID.Value) : null;

            Allegiance allegiance;

            if (biota != null)
            {
                var entityBiota = ACE.Database.Adapter.BiotaConverter.ConvertToEntityBiota(biota);

                allegiance = new Allegiance(entityBiota);
            }
            else
                allegiance = new Allegiance(monarch.Guid);

            if (allegiance.TotalMembers == 1)
                return null;

            if (biota == null)
            {
                allegiance = WorldObjectFactory.CreateNewWorldObject("allegiance") as Allegiance;
                allegiance.MonarchId = monarch.Guid.Full;
                allegiance.Init(monarch.Guid);

                allegiance.SaveBiotaToDatabase();
            }

            AddPlayers(allegiance);

            //if (!Allegiances.ContainsKey(allegiance.Guid))
                //Allegiances.Add(allegiance.Guid, allegiance);
            Allegiances[allegiance.Guid] = allegiance;

            return allegiance;
        }

        /// <summary>
        /// Returns the AllegianceNode for a Player
        /// </summary>
        public static AllegianceNode GetAllegianceNode(IPlayer player)
        {
            Players.TryGetValue(player.Guid, out var allegianceNode);
            return allegianceNode;
        }

        /// <summary>
        /// Returns a list of all players under a monarch
        /// </summary>
        public static List<IPlayer> FindAllPlayers(ObjectGuid monarchGuid)
        {
            return PlayerManager.FindAllByMonarch(monarchGuid);
        }

        /// <summary>
        /// Loads the Allegiance and AllegianceNode for a Player
        /// </summary>
        public static void LoadPlayer(IPlayer player)
        {
            if (player == null) return;

            player.Allegiance = GetAllegiance(player);
            player.AllegianceNode = GetAllegianceNode(player);

            // TODO: update chat channels for online players here?
        }

        /// <summary>
        /// Called when a player joins/exits an Allegiance
        /// </summary>
        public static void Rebuild(Allegiance allegiance)
        {
            if (allegiance == null) return;

            RemoveCache(allegiance);

            // rebuild allegiance
            allegiance = GetAllegiance(allegiance.Monarch.Player);

            // relink players
            foreach (var member in allegiance.Members.Keys)
            {
                var player = PlayerManager.FindByGuid(member);
                if (player == null) continue;

                LoadPlayer(player);
            }

            // update dynamic properties
            allegiance.UpdateProperties();
        }

        /// <summary>
        /// Appends the Players lookup table with the members of an Allegiance
        /// </summary>
        public static void AddPlayers(Allegiance allegiance)
        {
            foreach (var member in allegiance.Members)
            {
                var player = member.Key;
                var allegianceNode = member.Value;

                if (!Players.ContainsKey(player))
                    Players.Add(player, allegianceNode);
                else
                    Players[player] = allegianceNode;
            }
        }

        /// <summary>
        /// Removes an Allegiance from the Players lookup table cache
        /// </summary>
        public static void RemoveCache(Allegiance allegiance)
        {
            foreach (var member in allegiance.Members)
                Players.Remove(member.Key);
        }

        /// <summary>
        /// The maximum amount of leadership / loyalty
        /// </summary>
        public static float SkillCap = 291.0f;

        /// <summary>
        /// The maximum amount of realtime hours sworn to patron
        /// </summary>
        public static float RealCap = 730.0f;

        /// <summary>
        /// The maximum amount of in-game hours sworn to patron
        /// </summary>
        public static float GameCap = 720.0f;

        public static void PassXP(AllegianceNode vassalNode, ulong amount, bool direct)
        {
            // http://asheron.wikia.com/wiki/Allegiance_Experience

            // Pre-patch:
            // Vassal-to-patron pass-up has no effective cap, but patron-to-grandpatron pass-up caps with an effective Loyalty of 175.
            // If you're sworn for 10 days to the same patron, the base Loyalty required to maximize the pass-up from your vassal through you to your patron is only 88.
            // Take into account level 7 enchantments, and you can see how there's practically no need to spend XP on the skill, due to the ease in reaching the cap.
            // Leadership is arguably worse off. In theory, you need to train Leadership and spend XP on it in order to get maximum results.
            // However, effective Leadership is multiplied by two factors: how many vassals you have, and how long they have been sworn to you, with the emphasis on the number of vassals you have.
            // The effect of Leadership on pass-up caps at around 165 effective Leadership, or 83 base Leadership before the modifier.
            // The end result of this is that if you have two active vassals and you can get 10 mules sworn underneath you for an average of 5 in-game days,
            // you never need to raise your Leadership beyond 83. Again, take into account level 7 enchantments and you can see why few people even bother training the skill. It's just too easy to reach the cap. 

            // Post-patch:
            // - Leadership and Loyalty are not based on Self attribute
            // - Effective Loyalty is still modified by the time you have spent sworn to your patron
            // - Effective Leadership is still modified by number of vassals and average time that they have been sworn to you,
            //   but the emphasis is on the "time sworn" side and not on the "number of vassals" side. In fact, the vassals
            //   required to achieve the maximum benefit has been decreased from 12 to 4. This is to reduce the incentive of having non-playing vassals.
            // - For both Loyalty and Leadership, the time sworn modifier will now be a factor of both in-game time and real time.
            // - Most importantly, Leadership and Loyalty no longer "cap"

            // XP pass-up:
            // - New minimums and maximums
            // - Vassal-to-patron pass-up will have a minimum of 25% of earned XP, and a maximum of 90% of earned XP.
            //   Under the old system, the minimum was about 9% of earned XP, and the effective maximum was somewhere near 44% of earned XP.
            // - Patron-to-grandpatron pass-up will have a minimum of 0% of XP passed-up by the patron's vassal, and a maximum of 10% of passed-up XP.
            //   Under the old system, the minimum was about 30% and the maximum was about 94%.

            // Original system: up to January 12, 2004
            // Follow-up: all XP instead of just kill XP: October 2009

            // Formulas:
            // http://asheron.wikia.com/wiki/XP_Passup

            // Thanks for Xerxes of Thistledown, who verified accuracy over four months of testing and research!

            // Generated % - Percentage of XP passed to the patron through the vassal's earned XP (hunting and most quests).
            // Received % - Percentage of XP that patron will receive from his vassal's Generated XP.
            // Passup % -  Percentage of XP actually received by patron from vassal's earned XP (hunting and most quests).

            // Generated % = 50.0 + 22.5 * (loyalty / 291) * (1.0 + (RT/730) * (IG/720))
            // Received % = 50.0 + 22.5 * (leadership / 291) * (1.0 + V * (RT2/730) * (IG2/720))
            // Passup % = Generated% * Received% / 100.0

            // Where:
            // Loyalty = Buffed Loyalty (291 max)
            // Leadership = Buffed Leadership (291 max)
            // RT = actual real time sworn to patron in days (730 max)
            // IG = actual in-game time sworn to patron in hours (720 max)
            // RT2 = average real time sworn to patron for all vassals in days (730 max)
            // IG2 = average in-game time sworn to patron for all vassals in hours (720 max)
            // V = vassal factor(1 = 0.25, 2 = 0.50, 3 = 0.75, 4 + = 1.00) (1.0 max)

            var patronNode = vassalNode.Patron;
            if (patronNode == null)
                return;

            var vassal = vassalNode.Player;
            var patron = patronNode.Player;

            if (!vassal.ExistedBeforeAllegianceXpChanges)
                return;

            var loyalty = Math.Min(vassal.GetCurrentLoyalty(), SkillCap);
            var leadership = Math.Min(patron.GetCurrentLeadership(), SkillCap);

            var timeReal = Math.Min(RealCap, RealCap);
            var timeGame = Math.Min(GameCap, GameCap);

            var timeRealAvg = Math.Min(RealCap, RealCap);
            var timeGameAvg = Math.Min(GameCap, GameCap);

            var vassalFactor = Math.Min(0.25f * patronNode.TotalVassals, 1.0f);

            var factor1 = direct ? 50.0f : 16.0f;
            var factor2 = direct ? 22.5f : 8.0f;

            var generated = (factor1 + factor2 * (loyalty / SkillCap) * (1.0f + (timeReal / RealCap) * (timeGame / GameCap))) * 0.01f;
            var received = (factor1 + factor2 * (leadership / SkillCap) * (1.0f + vassalFactor * (timeRealAvg / RealCap) * (timeGameAvg / GameCap))) * 0.01f;
            var passup = generated * received;

            var generatedAmount = (uint)(amount * generated);
            var passupAmount = (uint)(amount * passup);

            /*Console.WriteLine("---");
            Console.WriteLine("AllegianceManager.PassXP(" + amount + ")");
            Console.WriteLine("Vassal: " + vassal.Name);
            Console.WriteLine("Patron: " + patron.Name);

            Console.WriteLine("Generated: " + Math.Round(generated * 100, 2) + "%");
            Console.WriteLine("Received: " + Math.Round(received * 100, 2) + "%");
            Console.WriteLine("Passup: " + Math.Round(passup * 100, 2) + "%");

            Console.WriteLine("Generated amount: " + generatedAmount);
            Console.WriteLine("Passup amount: " + passupAmount);*/

            if (passupAmount > 0)
            {
                //vassal.CPTithed += generatedAmount;
                //patron.CPCached += passupAmount;
                //patron.CPPoolToUnload += passupAmount;

                vassal.AllegianceXPGenerated += generatedAmount;
                patron.AllegianceXPCached += passupAmount;

                var onlinePatron = PlayerManager.GetOnlinePlayer(patron.Guid);
                if (onlinePatron != null)
                    onlinePatron.AddAllegianceXP();

                // call recursively
                PassXP(patronNode, passupAmount, false);
            }
        }

        /// <summary>
        /// Updates the Allegiance tree structure when a new player joins
        /// </summary>
        /// <param name="vassal">The vassal swearing into the Allegiance</param>
        public static void OnSwearAllegiance(Player vassal)
        {
            if (vassal == null) return;

            // was this vassal previously a Monarch?
            if (vassal.Allegiance != null)
                RemoveCache(vassal.Allegiance);

            // rebuild the new combined structure
            var allegiance = GetAllegiance(vassal);
            Rebuild(allegiance);

            LoadPlayer(vassal);

            // maintain approved vassals list
            if (allegiance != null && allegiance.HasApprovedVassal(vassal.Guid.Full))
                allegiance.RemoveApprovedVassal(vassal.Guid.Full);
        }

        /// <summary>
        /// Updates the Allegiance tree structure when a member leaves
        /// </summary>
        /// <param name="self">The player initiating the break request</param>
        /// <param name="target">The patron or vassal of the self player</param>
        public static void OnBreakAllegiance(IPlayer self, IPlayer target)
        {
            // remove the previous allegiance structure
            if (self != null)   // ??
                RemoveCache(self.Allegiance);

            // rebuild for self and target
            var selfAllegiance = GetAllegiance(self);
            var targetAllegiance = GetAllegiance(target);

            Rebuild(selfAllegiance);
            Rebuild(targetAllegiance);

            LoadPlayer(self);
            LoadPlayer(target);

            HandleNoAllegiance(self);
            HandleNoAllegiance(target);
        }

        public static void HandleNoAllegiance(IPlayer player)
        {
            if (player == null || player.Allegiance != null)
                return;

            var onlinePlayer = PlayerManager.GetOnlinePlayer(player.Guid);

            var updated = false;

            if (player.MonarchId != null)
            {
                player.UpdateProperty(PropertyInstanceId.Monarch, null, true);

                updated = true;
            }

            if (player.AllegianceRank != null)
            {
                player.AllegianceRank = null;

                if (onlinePlayer != null)
                    onlinePlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlinePlayer, PropertyInt.AllegianceRank, 0));

                updated = true;
            }

            if (updated)
                player.SaveBiotaToDatabase();

            if (onlinePlayer != null)
                onlinePlayer.Session.Network.EnqueueSend(new GameEventAllegianceUpdate(onlinePlayer.Session, onlinePlayer.Allegiance, onlinePlayer.AllegianceNode), new GameEventAllegianceAllegianceUpdateDone(onlinePlayer.Session));
        }

        public static Allegiance FindAllegiance(uint allegianceID)
        {
            Allegiances.TryGetValue(new ObjectGuid(allegianceID), out var allegiance);
            return allegiance;
        }

        // This function is called from a database callback.
        // We must add thread safety to prevent AllegianceManager corruption
        public static void HandlePlayerDelete(uint playerGuid)
        {
            WorldManager.EnqueueAction(new ActionEventDelegate(() => DoHandlePlayerDelete(playerGuid)));
        }

        private static void DoHandlePlayerDelete(uint playerGuid)
        {
            var player = PlayerManager.FindByGuid(playerGuid);
            if (player == null)
            {
                Console.WriteLine($"AllegianceManager.HandlePlayerDelete({playerGuid:X8}): couldn't find player guid");
                return;
            }
            var allegiance = GetAllegiance(player);

            if (allegiance == null) return;

            allegiance.Members.TryGetValue(player.Guid, out var allegianceNode);

            var players = new List<IPlayer>() { player };

            if (player.PatronId != null)
            {
                var patron = PlayerManager.FindByGuid(player.PatronId.Value);

                if (patron != null)
                    players.Add(patron);
            }

            player.PatronId = null;
            player.UpdateProperty(PropertyInstanceId.Monarch, null, true);

            // vassals now become monarchs...
            foreach (var vassalNode in allegianceNode.Vassals.Values)
            {
                var vassal = PlayerManager.FindByGuid(vassalNode.PlayerGuid);

                if (vassal == null) continue;

                vassal.PatronId = null;
                vassal.UpdateProperty(PropertyInstanceId.Monarch, null, true);

                // walk the allegiance tree from this node, update monarch ids
                vassalNode.Walk((node) =>
                {
                    node.Player.UpdateProperty(PropertyInstanceId.Monarch, vassalNode.PlayerGuid.Full, true);

                    node.Player.SaveBiotaToDatabase();

                }, false);

                players.Add(vassal);
            }

            RemoveCache(allegiance);

            // rebuild for those directly involved
            foreach (var p in players)
                Rebuild(GetAllegiance(p));

            foreach (var p in players)
                LoadPlayer(p);

            foreach (var p in players)
                HandleNoAllegiance(p);

            // save immediately?
            foreach (var p in players)
                p.SaveBiotaToDatabase();

            foreach (var p in players)
            {
                Player.CheckAllegianceHouse(p.Guid);

                var newAllegiance = GetAllegiance(p);
                if (newAllegiance != null)
                    newAllegiance.Monarch.Walk((node) => Player.CheckAllegianceHouse(node.PlayerGuid), false);
            }
        }
    }
}
