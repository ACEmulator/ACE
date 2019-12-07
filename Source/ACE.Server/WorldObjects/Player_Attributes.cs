using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool HandleActionRaiseAttribute(PropertyAttribute attribute, uint amount)
        {
            if (!Attributes.TryGetValue(attribute, out var creatureAttribute))
            {
                log.Error($"{Name}.HandleActionRaiseAttribute({attribute}, {amount}) - invalid attribute");
                return false;
            }

            if (amount > AvailableExperience)
            {
                log.Error($"{Name}.HandleActionRaiseAttribute({attribute}, {amount}) - amount > AvaiableExperience ({AvailableExperience})");
                return false;
            }

            var prevRank = creatureAttribute.Ranks;

            if (!SpendAttributeXp(creatureAttribute, amount))
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {attribute} has failed.", ChatMessageType.Broadcast);
                return false;
            }

            Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute(this, creatureAttribute));

            if (prevRank != creatureAttribute.Ranks)
            {
                // checks if max rank is achieved and plays fireworks w/ special text
                var suffix = "";
                if (creatureAttribute.IsMaxRank)
                {
                    // fireworks
                    PlayParticleEffect(PlayScript.WeddingBliss, Guid);
                    suffix = " and has reached its upper limit";
                }

                var sound = new GameMessageSound(Guid, Sound.RaiseTrait);
                var msg = new GameMessageSystemChat($"Your base {attribute} is now {creatureAttribute.Base}{suffix}!", ChatMessageType.Advancement);

                Session.Network.EnqueueSend(sound, msg);

                if (attribute == PropertyAttribute.Endurance)
                {
                    // this packet appears to trigger client to update both health and stamina
                    var updateHealth = new GameMessagePrivateUpdateVital(this, Health);

                    Session.Network.EnqueueSend(updateHealth);
                }
                else if (attribute == PropertyAttribute.Self)
                {
                    var updateMana = new GameMessagePrivateUpdateVital(this, Mana);

                    Session.Network.EnqueueSend(updateMana);
                }

                // retail was missing the 'raise attribute' runrate hook here
                if ((attribute == PropertyAttribute.Strength || attribute == PropertyAttribute.Quickness) && PropertyManager.GetBool("runrate_add_hooks").Item)
                    HandleRunRateUpdate();
            }

            return true;
        }

        private bool SpendAttributeXp(CreatureAttribute creatureAttribute, uint amount, bool sendNetworkUpdate = true)
        {
            // ensure attribute is not already max rank
            if (creatureAttribute.IsMaxRank)
            {
                log.Error($"{Name}.SpendAttributeXp({creatureAttribute.Attribute}, {amount}) - player tried to raise attribute beyond max rank");
                return false;
            }

            // the client should already handle this naturally,
            // but ensure player can't spend xp beyond the max rank
            var amountToEnd = creatureAttribute.ExperienceLeft;

            if (amount > amountToEnd)
            {
                log.Error($"{Name}.SpendAttributeXp({creatureAttribute.Attribute}, {amount}) - player tried to raise attribute beyond {amountToEnd} experience");
                return false;   // returning error here, instead of setting amount to amountToEnd
            }

            // everything looks good at this point,
            // spend xp on attribute
            if (!SpendXP(amount, sendNetworkUpdate))
            {
                log.Error($"{Name}.SpendAttributeXp({creatureAttribute.Attribute}, {amount}) - SpendXP failed");
                return false;
            }

            creatureAttribute.ExperienceSpent += amount;

            // calculate new rank
            creatureAttribute.Ranks = (ushort)CalcAttributeRank(creatureAttribute.ExperienceSpent);

            return true;
        }

        public void SpendAllAvailableAttributeXp(CreatureAttribute creatureAttribute, bool sendNetworkUpdate = true)
        {
            var amountRemaining = creatureAttribute.ExperienceLeft;

            if (amountRemaining > AvailableExperience)
                amountRemaining = (uint)AvailableExperience;

            SpendAttributeXp(creatureAttribute, amountRemaining, sendNetworkUpdate);
        }

        /// <summary>
        /// Returns the maximum rank that can be purchased with an xp amount
        /// </summary>
        /// <param name="xpAmount">The amount of xp used to make the purchase</param>
        public static int CalcAttributeRank(uint xpAmount)
        {
            var rankXpTable = DatManager.PortalDat.XpTable.AttributeXpList;

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
