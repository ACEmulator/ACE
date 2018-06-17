using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.WorldObjects;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Entity;
using ACE.Entity.Enum;
using System.Linq;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Allegiance helper methods
    /// </summary>
    public class AllegianceManager
    {
        /// <summary>
        /// A mapping of all Players on the server => their AllegianceNodes
        /// </summary>
        public static Dictionary<Player, AllegianceNode> Players;

        static AllegianceManager()
        {
            Players = new Dictionary<Player, AllegianceNode>();
        }

        /// <summary>
        /// Returns the monarch for a player
        /// </summary>
        public static Player GetMonarch(Player player)
        {
            return WorldManager.AllPlayers.Where(p => p.Guid.Full.Equals(player.Monarch)).FirstOrDefault();

            // find the monarch by walking the allegiance chain
            var currentPlayer = player;
            do
            {
                var patron = currentPlayer.GetProperty(PropertyInstanceId.Patron);
                if (patron != null)
                    currentPlayer = WorldManager.GetPlayerByGuidId(patron.Value);
                else
                    return currentPlayer;
            }
            while (true);
        }

        /// <summary>
        /// Returns the full allegiance structure for any player
        /// </summary>
        /// <param name="player">A player at any level of an allegiance</param>
        public static Allegiance GetAllegiance(Player player)
        {
            var monarch = GetMonarch(player);

            if (monarch == null) return null;

            var allegiance = new Allegiance(monarch);
            if (allegiance.TotalMembers == 1)
                return null;
            else
            {
                AddPlayers(allegiance);
                return allegiance;
            }
        }

        /// <summary>
        /// Returns the AllegianceNode for a Player
        /// </summary>
        public static AllegianceNode GetAllegianceNode(Player player)
        {
            Players.TryGetValue(player, out var allegianceNode);
            return allegianceNode;
        }

        /// <summary>
        /// Returns a list of all players under a monarch
        /// </summary>
        public static List<Player> FindAllPlayers(Player monarch)
        {
            return WorldManager.GetAllegiance(monarch);
        }

        /// <summary>
        /// Loads the Allegiance and AllegianceNode for a Player
        /// </summary>
        public static void LoadPlayer(Player player)
        {
            player.Allegiance = GetAllegiance(player);
            player.AllegianceNode = GetAllegianceNode(player);
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

            var loyalty = Math.Min(vassal.GetCreatureSkill(Skill.Loyalty).Current, SkillCap);
            var leadership = Math.Min(patron.GetCreatureSkill(Skill.Leadership).Current, SkillCap);

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

            var generatedAmount = (ulong)(amount * generated);
            var passupAmount = (ulong)(amount * passup);

            Console.WriteLine("---");
            Console.WriteLine("AllegianceManager.PassXP(" + amount + ")");
            Console.WriteLine("Vassal: " + vassal.Name);
            Console.WriteLine("Patron: " + patron.Name);

            Console.WriteLine("Generated: " + Math.Round(generated * 100, 2) + "%");
            Console.WriteLine("Received: " + Math.Round(received * 100, 2) + "%");
            Console.WriteLine("Passup: " + Math.Round(passup * 100, 2) + "%");

            Console.WriteLine("Generated amount: " + generatedAmount);
            Console.WriteLine("Passup amount: " + passupAmount);

            if (passupAmount > 0)
            {
                vassal.CPTithed += generatedAmount;
                patron.CPCached += passupAmount;
                patron.CPPoolToUnload += passupAmount;

                // call recursively
                PassXP(patronNode, passupAmount, false);
            }
        }
    }
}
