using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using System;

namespace ACE.Server.WorldObjects
{
    public class Lockpick : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Lockpick(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Lockpick(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Lockpick;
        }

        private void Consume(Player player)
        {
            // to-do don't consume "Limitless Lockpick" rare.
            Structure--;
            if (Structure < 1)
                player.TryRemoveItemFromInventoryWithNetworking(this, 1);
            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, (int)Structure));
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your lockpicks have {Structure} uses left.", ChatMessageType.Craft));
        }

        public void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            ActionChain chain = new ActionChain();

            chain.AddAction(player, () =>
            {
                if (player.Skills[Skill.Lockpick].Status != SkillStatus.Trained && player.Skills[Skill.Lockpick].Status != SkillStatus.Specialized)
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouArentTrainedInLockpicking));
                    return;
                }
                if (target is Lock @lock)
                {
                    UnlockResults results = @lock.Unlock(player.Skills[Skill.Lockpick].Current);
                    switch (results)
                    {
                        case UnlockResults.UnlockSuccess:
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have successfully picked the lock! It is now unlocked.", ChatMessageType.Craft));
                            Consume(player);
                            break;
                        case UnlockResults.Open:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockWhatIsOpen));
                            break;
                        case UnlockResults.AlreadyUnlocked:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.LockAlreadyUnlocked));
                            break;
                        case UnlockResults.PickLockFailed:
                            target.CurrentLandblock.EnqueueBroadcastSound(target, Sound.PicklockFail);
                            Consume(player);
                            break;
                        default:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.KeyDoesntFitThisLock));
                            break;
                    }
                }
                else
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockOrUnlockThat));
                }
            });

            chain.EnqueueChain();
        }
    }
}
