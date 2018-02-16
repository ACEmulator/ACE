using System;
using System.Diagnostics;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Gem : WorldObject
    {
        /// <summary>
        /// This is ace_object_property_int #349.   It links a contract weenie with the quest that it will add to your quest panel.
        /// </summary>
        public int? UseCreateContractId
        {
            get { return AceObject.UseCreateContractId; }
            set { AceObject.UseCreateContractId = value; }
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Gem(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Gem(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Stuck, true);
            SetProperty(PropertyBool.Attackable, true);
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
            if (UseCreateContractId == null)
            {
                var spellTable = DatManager.PortalDat.SpellTable;
                if (!spellTable.Spells.ContainsKey((uint)SpellDID))
                {
                    return;
                }
                SpellBase spell = spellTable.Spells[(uint)SpellDID];
                string castMessage = "The gem casts " + spell.Name + " on you";
                ////These if statements are to catch spells with an apostrophe in the dat file which throws off the client in reading it from the dat.
                if (spell.MetaSpellId == 3810)
                {
                    castMessage = "The gem casts Asheron's Benediction on you";
                }
                if (spell.MetaSpellId == 3811)
                {
                    castMessage = "The gem casts Blackmoor's Favor on you";
                }
                if (spell.MetaSpellId == 3953)
                {
                    castMessage = "The gem casts Carraida's Benediction on you";
                }
                if (spell.MetaSpellId == 4024)
                {
                    castMessage = "The gem casts Asheron's Lesser Benediction on you";
                }
                castMessage += "."; // If not refreshing/surpassing/less than active spell, which I will check for in the very near future when I get the active enchantment list implemented.
                session.Network.EnqueueSend(new GameMessageSystemChat(castMessage, ChatMessageType.Magic));
                session.Player.PlayParticleEffect((PlayScript)spell.TargetEffect, session.Player.Guid);
                const ushort layer = 1; // FIXME: This will be tracked soon, once a list is made to track active enchantments
                session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(session, session.Player, spell, layer, 1, 0x2009010)); ////The values that are hardcoded are not directly available from spell table, but will be available soon.
                ////session.Player.HandleActionRemoveItemFromInventory(Guid.Full, (uint)ContainerId, 1); This is commented out to aid in testing. Will be uncommented later.
                session.Player.SendUseDoneEvent();
                return;
            }

            ContractTracker contractTracker = new ContractTracker((uint)UseCreateContractId, session.Player.Guid.Full)
            {
                Stage = 0,
                TimeWhenDone = 0,
                TimeWhenRepeats = 0,
                DeleteContract = 0,
                SetAsDisplayContract = 1
            };

            if (CooldownId != null && session.Player.LastUseTracker.TryGetValue(CooldownId.Value, out var lastUse))
            {
                TimeSpan timeRemaining = lastUse.AddSeconds(CooldownDuration ?? 0.00).Subtract(DateTime.Now);
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
                Debug.Assert(CooldownId != null, "CooldownId != null");
                Debug.Assert(CooldownDuration != null, "CooldownDuration != null");
                const uint layer = 0x10000; // FIXME: we need to track how many layers of the exact same spell we have in effect.
                //const uint spellCategory = 0x8000; // FIXME: Not sure where we get this from
                SpellBase spellBase = new SpellBase(0, CooldownDuration.Value, 0, -666);
                session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(session, session.Player, spellBase, layer, CooldownId.Value, (uint)EnchantmentTypeFlags.Cooldown));

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
