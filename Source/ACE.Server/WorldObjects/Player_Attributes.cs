using System;
using System.Collections.Generic;

using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void RaiseAttribute(Ability ability, uint amount)
        {

        }




















        public void SpendXp(Ability ability, uint amount)
        {
            /* todo fix for EF version
            bool isSecondary = false;
            ICreatureXpSpendableStat creatureStat;

            bool success = AceObject.AceObjectPropertiesAttributes.TryGetValue(ability, out var creatureAbility);

            if (success)
                creatureStat = creatureAbility;
            else
            {
                success = AceObject.AceObjectPropertiesAttributes2nd.TryGetValue(ability, out var v);

                // Invalid ability
                if (success)
                    creatureStat = v;
                else
                {
                    log.Error("Invalid ability passed to Player.SpendXp");
                    return;
                }

                isSecondary = true;
            }

            uint baseValue = creatureStat.StartingValue;
            uint result = SpendAbilityXp(creatureStat, amount);
            uint ranks = creatureStat.Ranks;
            uint newValue = creatureStat.Base;

            if (result > 0u)
            {
                GameMessage abilityUpdate;

                if (!isSecondary)
                    abilityUpdate = new GameMessagePrivateUpdateAbility(Session, ability, ranks, baseValue, result);
                else
                    abilityUpdate = new GameMessagePrivateUpdateVital(Session, ability, ranks, baseValue, result, creatureStat.Current);

                // checks if max rank is achieved and plays fireworks w/ special text
                string messageText;

                if (IsAbilityMaxRank(ranks, isSecondary))
                {
                    // fireworks
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {ability} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {ability} is now {newValue}!";
                }

                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, (ulong)AvailableExperience);
                var soundEvent = new GameMessageSound(Guid, Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);

                // This seems to be needed to keep health up to date properly.
                // Needed when increasing health and endurance.
                if (ability == Ability.Endurance)
                {
                    var healthUpdate = new GameMessagePrivateUpdateVital(Session, Ability.Health, Health.Ranks, Health.StartingValue, Health.ExperienceSpent, Health.Current);
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message, healthUpdate);
                }
                else if (ability == Ability.Self)
                {
                    var manaUpdate = new GameMessagePrivateUpdateVital(Session, Ability.Mana, Mana.Ranks, Mana.StartingValue, Mana.ExperienceSpent, Mana.Current);
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message, manaUpdate);
                }
                else
                {
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message);
                }
            }
            else
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {ability} has failed.", ChatMessageType.Broadcast);
            }*/
        }


        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        /*private uint SpendAbilityXp(ICreatureXpSpendableStat ability, uint amount)
        {
            uint result = 0;

            List<uint> xpList;
            var xpTable = DatManager.PortalDat.XpTable;

            switch (ability.Ability)
            {
                case Ability.Health:
                case Ability.Stamina:
                case Ability.Mana:
                    xpList = xpTable.VitalXpList;
                    break;
                default:
                    xpList = xpTable.AbilityXpList;
                    break;
            }

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
                // FIXME(ddevec):
                //      Really AddRank() should probably be a method of CreatureAbility/CreatureVital
                ability.Ranks += rankUps;
                ability.ExperienceSpent += amount;
                Character.SpendXp(amount);
                result = ability.ExperienceSpent;
            }

            return result;
        }*/

        /// <summary>
        /// Check a rank against the ability charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if ability is max rank; false if ability is below max rank</returns>
        private bool IsAbilityMaxRank(uint rank, bool isAbilityVitals)
        {
            List<uint> xpList;
            var xpTable = DatManager.PortalDat.XpTable;

            if (isAbilityVitals)
                xpList = xpTable.VitalXpList;
            else
                xpList = xpTable.AbilityXpList;

            if (rank == (xpList.Count - 1))
                return true;

            return false;
        }


    }
}
