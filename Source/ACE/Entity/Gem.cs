using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    using System;

    public class Gem : WorldObject
    {
        /// <summary>
        /// This is ace_object_property_int #349.   It links a contract weenie with the quest that it will add to your quest panel.
        /// </summary>
        public uint? UseCreateContractId
        {
            get { return AceObject.UseCreateContractId; }
            set { AceObject.UseCreateContractId = value; }
        }

        public Gem(AceObject aceObject)
            : base(aceObject)
        {
        }

        /// <summary>
        /// The OnUse method for this class is to use a contract to add a tracked quest to our quest panel.
        /// This gives the player access to information about the quest such as starting and ending NPC locations,
        /// and shows our progress for kill tasks as well as any timing information such as when we can repeat the
        /// quest or how much longer we have to complete it in the case of at timed quest.   Og II
        /// </summary>
        /// <param name="session">Pass the session variable so we will have access to player and the correct sequences</param>
        public override void OnUse(Session session)
        {
            if (UseCreateContractId == null) return;
            ContractTracker contractTracker = new ContractTracker((uint)UseCreateContractId, session.Player.Guid.Full)
            {
                Stage = 0,
                TimeWhenDone = 0,
                TimeWhenRepeats = 0,
                DeleteContract = 0,
                SetAsDisplayContract = 1
            };

            DateTime lastUse;
            if (CooldownId != null && session.Player.LastUseTracker.TryGetValue(CooldownId.Value, out lastUse))
            {
                TimeSpan timeRemaining = lastUse.AddSeconds(CooldownDuration + 600.00 ?? 0.00).Subtract(DateTime.Now);
                if (timeRemaining.Seconds > 0)
                {
                    ChatPacket.SendServerMessage(session, "You cannot use another contract for " + timeRemaining.Seconds + " seconds", ChatMessageType.Broadcast);
                    session.Player.SendUseDoneEvent();
                    return;
                }
            }

            // We need to see if we are tracking this quest already.   Also, I cannot be used on world, so I must have a container id
            if (!session.Player.TrackedContracts.ContainsKey((uint)UseCreateContractId) && ContainerId != null)
            {
                session.Player.TrackedContracts.Add((uint)UseCreateContractId, contractTracker);

                // This will track our use for each contract using the shared cooldown server side.
                if (CooldownId != null)
                {
                    // add or update.
                    if (!session.Player.LastUseTracker.ContainsKey(CooldownId.Value))
                        session.Player.LastUseTracker.Add(CooldownId.Value, DateTime.Now);
                    else
                        session.Player.LastUseTracker[CooldownId.Value] = DateTime.Now;
                }

                GameEventSendClientContractTracker contractMsg = new GameEventSendClientContractTracker(session, contractTracker);
                session.Network.EnqueueSend(contractMsg);
                ChatPacket.SendServerMessage(session, "You just added " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);

                // TODO: Add sending the 02C2 message UpdateEnchantment.   They added a second use to this existing system
                // so they could show the delay on the client side - it is not really an enchantment but the they overloaded the use. Og II
                // Thanks Slushnas for letting me know about this as well as an awesome pcap that shows it all in action.

                // TODO: there is a lot of work to do here.   I am stubbing this in for now to send the right message.   Lots of magic numbers at the moment.
                SpellBase spellBase = new SpellBase
                {
                    Category = (uint)(EnchantmentTypeFlags.Cooldown | EnchantmentTypeFlags.Additive),
                    Power = 0,
                    Duration = CooldownId.Value,
                    DegradeModifier = 0,
                    DegradeLimit = -666,
                    MetaSpellId = 32768 + (uint)UseCreateContractId
                    // We will need to create a set of constants for each contract - I may be overloading this.
                };
                // session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(session, spellBase));

                // Ok this was not known to us, so we used the contract - now remove it from inventory.
                // HandleActionRemoveItemFromInventory is has it's own action chain.
                session.Player.HandleActionRemoveItemFromInventory(Guid.Full, (uint)ContainerId, 1);
            }
            else
                ChatPacket.SendServerMessage(session, "You already have this quest tracked: " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);

            // No mater any condition we need to send the use done event to clear the hour glass from the client.
            session.Player.SendUseDoneEvent();
        }
    }
}
