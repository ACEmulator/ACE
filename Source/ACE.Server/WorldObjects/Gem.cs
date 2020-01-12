using System;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Gem : Stackable
    {
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
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// 
        /// The OnUse method for this class is to use a contract to add a tracked quest to our quest panel.
        /// This gives the player access to information about the quest such as starting and ending NPC locations,
        /// and shows our progress for kill tasks as well as any timing information such as when we can repeat the
        /// quest or how much longer we have to complete it in the case of at timed quest.   Og II
        /// </summary>
        public override void ActOnUse(WorldObject activator)
        {
            ActOnUse(activator, false);
        }

        public void ActOnUse(WorldObject activator, bool confirmed)
        {
            if (!(activator is Player player))
                return;

            if (player.IsBusy || player.Teleporting)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YoureTooBusy));
                return;
            }

            if (!string.IsNullOrWhiteSpace(UseSendsSignal))
            {
                player.CurrentLandblock?.EmitSignal(player, UseSendsSignal);
                return;
            }

            // handle rare gems
            if (RareId != null && player.GetCharacterOption(CharacterOption.ConfirmUseOfRareGems) && !confirmed)
            {
                var msg = $"Are you sure you want to use {Name}?";
                var confirm = new Confirmation_Custom(player.Guid, () => ActOnUse(activator, true));
                player.ConfirmationManager.EnqueueSend(confirm, msg);
                return;
            }

            if (RareUsesTimer)
            {
                var currentTime = Time.GetUnixTime();

                var timeElapsed = currentTime - player.LastRareUsedTimestamp;

                if (timeElapsed < RareTimer)
                {
                    // TODO: get retail message
                    var remainTime = (int)Math.Ceiling(RareTimer - timeElapsed);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You may use another timed rare in {remainTime}s", ChatMessageType.Broadcast));
                    return;
                }

                player.LastRareUsedTimestamp = currentTime;

                // local broadcast usage
                player.EnqueueBroadcast(new GameMessageSystemChat($"{player.Name} used the rare item {Name}", ChatMessageType.Broadcast));
            }

            if (SpellDID.HasValue)
            {
                var spell = new Server.Entity.Spell((uint)SpellDID);

                TryCastSpell(spell, player, this);
            }

            if (UseCreateContractId > 0)
            {
                if (!player.ContractManager.Add(UseCreateContractId.Value))
                    return;
                else // this wasn't in retail, but the lack of feedback when using a contract gem just seems jarring so...
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} accepted. Click on the quill icon in the lower right corner to open your contract tab to view your active contracts.", ChatMessageType.Broadcast));
            }

            if (UseCreateItem > 0)
            {
                //if (DatabaseManager.World.GetCachedWeenie(UseCreateItem.Value) is null)
                //{
                //    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "Unable to create object, WCID is not in database !")); // custom error
                //    return;
                //}

                var playerFreeInventorySlots = player.GetFreeInventorySlots();
                var playerFreeContainerSlots = player.GetFreeContainerSlots();
                var playerAvailableBurden = player.GetAvailableBurden();

                var playerOutOfInventorySlots = false;
                var playerOutOfContainerSlots = false;
                var playerExceedsAvailableBurden = false;

                var amount = UseCreateQuantity ?? 1;

                var itemStacks = player.PreCheckItem(UseCreateItem.Value, amount, playerFreeContainerSlots, playerFreeInventorySlots, playerAvailableBurden, out var itemEncumberance, out bool itemRequiresBackpackSlot);

                if (itemRequiresBackpackSlot)
                {
                    playerFreeContainerSlots -= itemStacks;
                    playerAvailableBurden -= itemEncumberance;

                    playerOutOfContainerSlots = playerFreeContainerSlots < 0;
                }
                else
                {
                    playerFreeInventorySlots -= itemStacks;
                    playerAvailableBurden -= itemEncumberance;

                    playerOutOfInventorySlots = playerFreeInventorySlots < 0;
                }

                playerExceedsAvailableBurden = playerAvailableBurden < 0;

                if (playerOutOfInventorySlots || playerOutOfContainerSlots || playerExceedsAvailableBurden)
                {
                    if (playerExceedsAvailableBurden)
                        player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You are too encumbered to use that!"));
                    else if (playerOutOfInventorySlots)
                        player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You do not have enough pack space to use that!"));
                    else //if (playerOutOfContainerSlots)
                        player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You do not have enough container slots to use that!"));
                    return;
                }

                if (itemStacks > 0)
                {
                    while (amount > 0)
                    {
                        var item = WorldObjectFactory.CreateNewWorldObject(UseCreateItem.Value);

                        if (item is Stackable)
                        {
                            // amount contains a max stack
                            if (item.MaxStackSize <= amount)
                            {
                                item.SetStackSize(item.MaxStackSize);
                                amount -= item.MaxStackSize.Value;
                            }
                            else // not a full stack
                            {
                                item.SetStackSize(amount);
                                amount -= amount;
                            }
                        }
                        else
                            amount -= 1;

                        player.TryCreateInInventoryWithNetworking(item);
                    }
                }
                else
                {
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"Unable to use {Name} at this time!"));
                    return;
                }
            }

            if ((GetProperty(PropertyBool.UnlimitedUse) ?? false) == false)
                player.TryConsumeFromInventoryWithNetworking(this, 1);
        }

        public int? RareId
        {
            get => GetProperty(PropertyInt.RareId);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.RareId); else SetProperty(PropertyInt.RareId, value.Value); }
        }

        public bool RareUsesTimer
        {
            get => GetProperty(PropertyBool.RareUsesTimer) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.RareUsesTimer); else SetProperty(PropertyBool.RareUsesTimer, value); }
        }

        public override void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            // should tailoring kit / aetheria be subtyped?
            if (Tailoring.IsTailoringKit(WeenieClassId))
            {
                Tailoring.UseObjectOnTarget(player, this, target);
                return;
            }

            // fallback on recipe manager?
            base.HandleActionUseOnTarget(player, target);
        }

        /// <summary>
        /// For Rares that use cooldown timers (RareUsesTimer),
        /// any other rares with RareUsesTimer may not be used for 3 minutes
        /// Note that if the player logs out, this cooldown timer continues to tick/expire (unlike enchantments)
        /// </summary>
        public static int RareTimer = 180;

        public string UseSendsSignal
        {
            get => GetProperty(PropertyString.UseSendsSignal);
            set { if (value == null) RemoveProperty(PropertyString.UseSendsSignal); else SetProperty(PropertyString.UseSendsSignal, value); }
        }
    }
}
