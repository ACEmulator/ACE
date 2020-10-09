using System.Runtime.CompilerServices;

using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Handles the GameAction 0x44 - RaiseVital network message from client
        /// </summary>
        public bool HandleActionRaiseVital(PropertyAttribute2nd vital, uint amount)
        {
            if (!Vitals.TryGetValue(vital, out var creatureVital))
            {
                log.Error($"{Name}.HandleActionRaiseVital({vital}, {amount}) - invalid vital");
                return false;
            }

            if (amount > AvailableExperience)
            {
                // there is a client bug for vitals only,

                // where the client will enable the button to raise a vital by 10
                // if the player only has enough AvailableExperience to raise it by 1

                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {vital.ToSentence()} has failed.", ChatMessageType.Broadcast);

                log.Error($"{Name}.HandleActionRaiseVital({vital}, {amount}) - amount > AvailableExperience ({AvailableExperience})");
                return false;
            }

            var prevRank = creatureVital.Ranks;

            if (!SpendVitalXp(creatureVital, amount))
                return false;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(this, creatureVital));

            if (prevRank != creatureVital.Ranks)
            {
                // checks if max rank is achieved and plays fireworks w/ special text
                var suffix = "";
                if (creatureVital.IsMaxRank)
                {
                    // fireworks
                    PlayParticleEffect(PlayScript.WeddingBliss, Guid);
                    suffix = $" and has reached its upper limit";
                }

                var sound = new GameMessageSound(Guid, Sound.RaiseTrait);
                var msg = new GameMessageSystemChat($"Your base {vital.ToSentence()} is now {creatureVital.Base}{suffix}!", ChatMessageType.Advancement);

                Session.Network.EnqueueSend(sound, msg);
            }
            return true;
        }

        private bool SpendVitalXp(CreatureVital creatureVital, uint amount, bool sendNetworkUpdate = true)
        {
            // ensure vital is not already max rank
            if (creatureVital.IsMaxRank)
            {
                log.Error($"{Name}.SpendVitalXp({creatureVital.Vital}, {amount}) - player tried to raise vital beyond max rank");
                return false;
            }

            // the client should already handle this naturally,
            // but ensure player can't spent xp beyond the max rank
            var amountToEnd = creatureVital.ExperienceLeft;

            if (amount > amountToEnd)
            {
                log.Error($"{Name}.SpendVitalXp({creatureVital.Vital}, {amount}) - player tried to raise vital beyond {amountToEnd} experience");
                return false;   // returning error here, instead of setting amount to amountToEnd
            }

            // everything looks good at this point,
            // spend xp on vital
            if (!SpendXP(amount, sendNetworkUpdate))
            {
                log.Error($"{Name}.SpendVitalXp({creatureVital.Vital}, {amount}) - SpendXP failed");
                return false;
            }

            creatureVital.ExperienceSpent += amount;

            // calculate new rank
            creatureVital.Ranks = (ushort)CalcVitalRank(creatureVital.ExperienceSpent);

            return true;
        }

        public void SpendAllAvailableVitalXp(CreatureVital creatureVital, bool sendNetworkUpdate = true)
        {
            var amountRemaining = creatureVital.ExperienceLeft;

            if (amountRemaining > AvailableExperience)
                amountRemaining = (uint)AvailableExperience;

            SpendVitalXp(creatureVital, amountRemaining, sendNetworkUpdate);
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
            var target = selectedTarget?.TryGetWorldObject() as Creature;

            if (target == null || target.Health.Current <= 0)
                return;

            var healthPercent = (float)target.Health.Current / target.Health.MaxValue;

            Session.Network.EnqueueSend(new GameEventUpdateHealth(Session, target.Guid.Full, healthPercent));
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

        /// <summary>
        /// Called when a player equips/dequips an item w/ GearMaxHealth
        /// </summary>
        public void HandleMaxHealthUpdate()
        {
            var gearMaxHealth = GetGearMaxHealth();

            if (gearMaxHealth == 0)
                GearMaxHealth = null;
            else
                GearMaxHealth = gearMaxHealth;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.GearMaxHealth, gearMaxHealth));

            if (Health.Current > Health.MaxValue)
                Health.Current = Health.MaxValue;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, Health.Current));
        }
    }
}
