using System;
using System.Runtime.CompilerServices;

using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
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
        private uint SpendVitalXp(CreatureVital ability, uint amount, bool sendNetworkPropertyUpdate = true)
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
                if (SpendXP(amount, sendNetworkPropertyUpdate))
                {
                    ability.Ranks += rankUps;
                    ability.ExperienceSpent += amount;
                    result = ability.ExperienceSpent;
                }
            }

            return result;
        }

        public void SpendAllAvailableVitalXp(CreatureVital ability, bool sendNetworkPropertyUpdate = true)
        {
            var xpList = DatManager.PortalDat.XpTable.VitalXpList;

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

                if (SpendVitalXp(ability, rank10, sendNetworkPropertyUpdate) == 0)
                    break;
            }
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

        public override void SetMaxVitals()
        {
            base.SetMaxVitals();

            var health = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, Health.Current);
            var stamina = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Stamina, Stamina.Current);
            var mana = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Mana, Mana.Current);

            Session.Network.EnqueueSend(health, stamina, mana);

            if (Fellowship != null)
                FellowVitalUpdate = true;
        }

        /// <summary>
        /// Sets the current vital to a new value
        /// </summary>
        /// <returns>The actual change in the vital, after clamping between 0 and MaxVital</returns>
        public override int UpdateVital(CreatureVital vital, int newVal)
        {
            var prevVal = vital.Current;

            var change = base.UpdateVital(vital, newVal);

            if (change == 0)
                return 0;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(this, vital.ToEnum(), vital.Current));

            if (Fellowship != null)
                FellowVitalUpdate = true;

            // check for exhaustion
            if (vital.Vital == PropertyAttribute2nd.Stamina || vital.Vital == PropertyAttribute2nd.MaxStamina)
            {
                if (newVal == 0)
                {
                    OnExhausted();
                }
                // retail was missing the 'exhausted done' automatic hook here
                else if (prevVal == 0 && PropertyManager.GetBool("runrate_add_hooks").Item)
                {
                    HandleRunRateUpdate();
                }
            }
            return change;
        }

        /// <summary>
        /// Called on Player heartbeat
        /// Sends a packet containing the latest health info for the selected target
        /// </summary>
        public void HandleTargetVitals()
        {
            if (selectedTarget == ObjectGuid.Invalid)
                return;

            var target = CurrentLandblock?.GetObject(selectedTarget) as Creature;
            if (target == null)
                return;

            if (target.Health.Current <= 0)
                return;

            var healthPercent = (float)target.Health.Current / target.Health.MaxValue;

            Session.Network.EnqueueSend(new GameEventUpdateHealth(Session, selectedTarget.Full, healthPercent));
        }

        /// <summary>
        /// Returns the maximum rank that can be purchased with an xp amount
        /// </summary>
        /// <param name="xpAmount">The amount of xp used to make the purchase</param>
        public static int CalcVitalRank(uint xpAmount)
        {
            var rankXpTable = DatManager.PortalDat.XpTable.VitalXpList;
            for (var i = rankXpTable.Count - 1; i >= 0; i--)
            {
                var rankAmount = rankXpTable[i];
                if (xpAmount >= rankAmount)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Called every ~5 secs to regenerate player vitals
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool VitalHeartBeat()
        {
            var vitalUpdate = base.VitalHeartBeat();

            if (vitalUpdate && Fellowship != null)
                FellowVitalUpdate = true;

            return vitalUpdate;
        }
    }
}
