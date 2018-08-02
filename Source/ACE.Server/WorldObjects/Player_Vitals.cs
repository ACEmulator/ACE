using System;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void HandleActionRaiseVital(PropertyAttribute2nd attribute, uint amount)
        {
            var creatureVital = new CreatureVital(this, attribute);

            uint result = SpendVitalXp(creatureVital, amount);

            if (result > 0u)
            {
                GameMessage abilityUpdate = new GameMessagePrivateUpdateVital(this, attribute, creatureVital.Ranks, creatureVital.StartingValue, result, creatureVital.Current);

                // checks if max rank is achieved and plays fireworks w/ special text
                string messageText;

                if (IsVitalMaxRank(creatureVital.Ranks))
                {
                    // fireworks
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {attribute.ToSentence()} is now {creatureVital.Base} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {attribute.ToSentence()} is now {creatureVital.Base}!";
                }

                var soundEvent = new GameMessageSound(Guid, Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);

                // This seems to be needed to keep health up to date properly.
                // Needed when increasing health and endurance.
                //if (attribute == PropertyAttribute2nd.Endurance)
                //{
                //    var healthUpdate = new GameMessagePrivateUpdateVital(Session, Ability.Health, Health.Ranks, Health.StartingValue, Health.ExperienceSpent, Health.Current);
                //    Session.Network.EnqueueSend(abilityUpdate, soundEvent, message, healthUpdate);
                //}
                //else if (attribute == PropertyAttribute2nd.Self)
                //{
                //    var manaUpdate = new GameMessagePrivateUpdateVital(Session, Ability.Mana, Mana.Ranks, Mana.StartingValue, Mana.ExperienceSpent, Mana.Current);
                //    Session.Network.EnqueueSend(abilityUpdate, soundEvent, message, manaUpdate);
                //}
                //else
                //{
                Session.Network.EnqueueSend(abilityUpdate, soundEvent, message);
                //}
            }
            else
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {attribute.ToSentence()} has failed.", ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendVitalXp(CreatureVital ability, uint amount)
        {
            uint result = 0;

            var xpList = DatManager.PortalDat.XpTable.VitalXpList;

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
                // Really AddRank() should probably be a method of CreatureAbility/CreatureVital
                ability.Ranks += rankUps;
                ability.ExperienceSpent += amount;
                SpendXP(amount);
                result = ability.ExperienceSpent;
            }

            return result;
        }

        /// <summary>
        /// Check a rank against the ability charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if ability is max rank; false if ability is below max rank</returns>
        private static bool IsVitalMaxRank(uint rank)
        {
            var xpList = DatManager.PortalDat.XpTable.VitalXpList;

            if (rank == (xpList.Count - 1))
                return true;

            return false;
        }

        /// <summary>
        /// Sets the current vital to a new value
        /// </summary>
        /// <returns>The actual change in the vital, after clamping between 0 and MaxVital</returns>
        public override int UpdateVital(CreatureVital vital, int newVal)
        {
            var change = base.UpdateVital(vital, newVal);

            if (change != 0)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(this, vital.ToEnum(), vital.Current));

            // check for exhaustion
            if (vital.Vital == PropertyAttribute2nd.Stamina || vital.Vital == PropertyAttribute2nd.MaxStamina)
            {
                if (change != 0 && vital.Current == 0)
                    OnExhausted();

            }
            return change;
        }
    }
}
