using System;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void RaiseAttributeGameAction(PropertyAttribute attribute, uint amount)
        {
            var creatureAttribute = new CreatureAttribute(this, attribute);

            uint result = SpendAttributeXp(creatureAttribute, amount);

            if (result > 0u)
            {
                GameMessage abilityUpdate = new GameMessagePrivateUpdateAttribute(this, attribute, creatureAttribute.Ranks, creatureAttribute.StartingValue, result);

                // checks if max rank is achieved and plays fireworks w/ special text
                string messageText;

                if (IsAttributeMaxRank(creatureAttribute.Ranks))
                {
                    // fireworks
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {attribute} is now {creatureAttribute.Base} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {attribute} is now {creatureAttribute.Base}!";
                }

                var soundEvent = new GameMessageSound(Guid, Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);

                // This seems to be needed to keep health up to date properly.
                // Needed when increasing health and endurance.
                if (attribute == PropertyAttribute.Endurance)
                {
                    var healthUpdate = new GameMessagePrivateUpdateVital(this, PropertyAttribute2nd.MaxHealth, Health.Ranks, Health.StartingValue, Health.ExperienceSpent, Health.Current);
                    Session.Network.EnqueueSend(abilityUpdate, soundEvent, message, healthUpdate);
                }
                else if (attribute == PropertyAttribute.Self)
                {
                    var manaUpdate = new GameMessagePrivateUpdateVital(this, PropertyAttribute2nd.MaxMana, Mana.Ranks, Mana.StartingValue, Mana.ExperienceSpent, Mana.Current);
                    Session.Network.EnqueueSend(abilityUpdate, soundEvent, message, manaUpdate);
                }
                else
                {
                    Session.Network.EnqueueSend(abilityUpdate, soundEvent, message);
                }

                // retail was missing the 'raise attribute' runrate hook here
                if ((attribute == PropertyAttribute.Strength || attribute == PropertyAttribute.Quickness) && PropertyManager.GetBool("runrate_add_hooks").Item)
                    HandleRunRateUpdate();
            }
            else
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {attribute} has failed.", ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendAttributeXp(CreatureAttribute ability, uint amount, bool sendNetworkPropertyUpdate = true)
        {
            uint result = 0;

            var xpList = DatManager.PortalDat.XpTable.AbilityXpList;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (ability.Ranks >= (xpList.Count - 1))
                return result;

            uint rankUps = 0u;
            uint currentRankXp = xpList[Convert.ToInt32(ability.Ranks)];
            uint rank1 = xpList[Convert.ToInt32(ability.Ranks) + 1] - currentRankXp;
            uint rank10;
            int rank10Offset = 0;

            if (ability.Ranks + 10 >= (xpList.Count))
            {
                rank10Offset = 10 - (Convert.ToInt32(ability.Ranks + 10) - (xpList.Count - 1));
                rank10 = xpList[Convert.ToInt32(ability.Ranks) + rank10Offset] - currentRankXp;
            }
            else
            {
                rank10 = xpList[Convert.ToInt32(ability.Ranks) + 10] - currentRankXp;
            }

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
            {
                if (rank10Offset > 0u)
                    rankUps = Convert.ToUInt32(rank10Offset);
                else
                    rankUps = 10u;
            }

            if (rankUps > 0)
            {
                if (SpendXP(amount, sendNetworkPropertyUpdate))
                {
                    ability.Ranks += rankUps;
                    ability.ExperienceSpent += amount;
                    result = ability.ExperienceSpent;
                }
            }

            return result;
        }

        public void SpendAllAvailableAttributeXp(CreatureAttribute ability, bool sendNetworkPropertyUpdate = true)
        {
            var xpList = DatManager.PortalDat.XpTable.AbilityXpList;

            while (true)
            {
                uint currentRankXp = xpList[Convert.ToInt32(ability.Ranks)];
                uint rank10;

                if (ability.Ranks + 10 >= (xpList.Count))
                {
                    var rank10Offset = 10 - (Convert.ToInt32(ability.Ranks + 10) - (xpList.Count - 1));
                    rank10 = xpList[Convert.ToInt32(ability.Ranks) + rank10Offset] - currentRankXp;
                }
                else
                {
                    rank10 = xpList[Convert.ToInt32(ability.Ranks) + 10] - currentRankXp;
                }

                if (SpendAttributeXp(ability, rank10, sendNetworkPropertyUpdate) == 0)
                    break;
            }
        }

        /// <summary>
        /// Check a rank against the ability charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if ability is max rank; false if ability is below max rank</returns>
        private static bool IsAttributeMaxRank(uint rank)
        {
            var xpList = DatManager.PortalDat.XpTable.AbilityXpList;

            if (rank == (xpList.Count - 1))
                return true;

            return false;
        }

        /// <summary>
        /// Returns the maximum rank that can be purchased with an xp amount
        /// </summary>
        /// <param name="xpAmount">The amount of xp used to make the purchase</param>
        public static int CalcAttributeRank(uint xpAmount)
        {
            var rankXpTable = DatManager.PortalDat.XpTable.AbilityXpList;
            for (var i = rankXpTable.Count - 1; i >= 0; i--)
            {
                var rankAmount = rankXpTable[i];
                if (xpAmount >= rankAmount)
                    return i;
            }
            return -1;
        }
    }
}
