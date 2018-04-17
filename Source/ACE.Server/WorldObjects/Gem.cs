using System;
using System.Diagnostics;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Gem : Stackable
    {
        /// <summary>
        /// This is ace_object_property_int #349.   It links a contract weenie with the quest that it will add to your quest panel.
        /// </summary>
        //public int? UseCreateContractId
        //{
        //    get { return AceObject.UseCreateContractId; }
        //    set { AceObject.UseCreateContractId = value; }
        //}

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
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the player using the item.<para />
        /// The item should be in the players possession.
        /// 
        /// The OnUse method for this class is to use a contract to add a tracked quest to our quest panel.
        /// This gives the player access to information about the quest such as starting and ending NPC locations,
        /// and shows our progress for kill tasks as well as any timing information such as when we can repeat the
        /// quest or how much longer we have to complete it in the case of at timed quest.   Og II
        /// </summary>
        public override void UseItem(Player player, ActionChain actionChain)
        {
            if (UseCreateContractId == null)
            {
                var spellTable = DatManager.PortalDat.SpellTable;
                if (!spellTable.Spells.ContainsKey((uint)SpellDID))
                    return;

                var spellBase = DatManager.PortalDat.SpellTable.Spells[(uint)SpellDID];
                var spell = DatabaseManager.World.GetCachedSpell((uint)SpellDID);

                string castMessage = "The gem casts " + spell.Name + " on you";
                ////These if statements are to catch spells with an apostrophe in the dat file which throws off the client in reading it from the dat.
                if (spell.MetaSpellId == 3810)
                    castMessage = "The gem casts Asheron's Benediction on you";
                if (spell.MetaSpellId == 3811)
                    castMessage = "The gem casts Blackmoor's Favor on you";
                if (spell.MetaSpellId == 3953)
                    castMessage = "The gem casts Carraida's Benediction on you";
                if (spell.MetaSpellId == 4024)
                    castMessage = "The gem casts Asheron's Lesser Benediction on you";
                castMessage += "."; // If not refreshing/surpassing/less than active spell, which I will check for in the very near future when I get the active enchantment list implemented.

                player.Session.Network.EnqueueSend(new GameMessageSystemChat(castMessage, ChatMessageType.Magic));
                player.PlayParticleEffect((PlayScript)spellBase.TargetEffect, player.Guid);
                const ushort layer = 1; // FIXME: This will be tracked soon, once a list is made to track active enchantments
                var gem = new Enchantment(player, SpellDID.Value, layer, spell.Category);
                player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(player.Session, gem));

                // add to enchantment registry
                player.EnchantmentManager.Add(gem);

                ////session.Player.HandleActionRemoveItemFromInventory(Guid.Full, (uint)ContainerId, 1); This is commented out to aid in testing. Will be uncommented later.
                player.SendUseDoneEvent();
                return;
            }

            ContractTracker contractTracker = new ContractTracker((uint)UseCreateContractId, player.Guid.Full)
            {
                Stage = 0,
                TimeWhenDone = 0,
                TimeWhenRepeats = 0,
                DeleteContract = 0,
                SetAsDisplayContract = 1
            };

            if (CooldownId != null && player.LastUseTracker.TryGetValue(CooldownId.Value, out var lastUse))
            {
                var timeRemaining = lastUse.AddSeconds(CooldownDuration ?? 0.00).Subtract(DateTime.Now);
                if (timeRemaining.Seconds > 0)
                {
                    ChatPacket.SendServerMessage(player.Session, "You cannot use another contract for " + timeRemaining.Seconds + " seconds", ChatMessageType.Broadcast);
                    player.SendUseDoneEvent();
                    return;
                }
            }

            // We need to see if we are tracking this quest already.   Also, I cannot be used on world, so I must have a container id
            if (!player.TrackedContracts.ContainsKey((uint)UseCreateContractId) && ContainerId != null)
            {
                player.TrackedContracts.Add((uint)UseCreateContractId, contractTracker);

                // This will track our use for each contract using the shared cooldown server side.
                if (CooldownId != null)
                {
                    // add or update.
                    if (!player.LastUseTracker.ContainsKey(CooldownId.Value))
                        player.LastUseTracker.Add(CooldownId.Value, DateTime.Now);
                    else
                        player.LastUseTracker[CooldownId.Value] = DateTime.Now;
                }

                GameEventSendClientContractTracker contractMsg = new GameEventSendClientContractTracker(player.Session, contractTracker);
                player.Session.Network.EnqueueSend(contractMsg);
                ChatPacket.SendServerMessage(player.Session, "You just added " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);

                // TODO: Add sending the 02C2 message UpdateEnchantment.   They added a second use to this existing system
                // so they could show the delay on the client side - it is not really an enchantment but the they overloaded the use. Og II
                // Thanks Slushnas for letting me know about this as well as an awesome pcap that shows it all in action.

                // TODO: there is a lot of work to do here.   I am stubbing this in for now to send the right message.   Lots of magic numbers at the moment.
                Debug.Assert(CooldownId != null, "CooldownId != null");
                Debug.Assert(CooldownDuration != null, "CooldownDuration != null");
                //const ushort layer = 0x10000; // FIXME: we need to track how many layers of the exact same spell we have in effect.
                const ushort layer = 1;
                //const uint spellCategory = 0x8000; // FIXME: Not sure where we get this from
                var spellBase = new SpellBase(0, CooldownDuration.Value, 0, -666);
                // cooldown not being used in network packet?
                var gem = new Enchantment(player, spellBase, layer, /*CooldownId.Value,*/ (uint)EnchantmentTypeFlags.Cooldown);
                player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(player.Session, gem));

                // Ok this was not known to us, so we used the contract - now remove it from inventory.
                // HandleActionRemoveItemFromInventory is has it's own action chain.
                player.TryRemoveItemFromInventoryWithNetworking(this, 1);
            }
            else
                ChatPacket.SendServerMessage(player.Session, "You already have this quest tracked: " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);

            // No mater any condition we need to send the use done event to clear the hour glass from the client.
            player.SendUseDoneEvent();
        }
    }
}
