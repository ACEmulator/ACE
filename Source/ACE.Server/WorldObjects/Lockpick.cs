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

        public void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            ActionChain chain = new ActionChain();


            chain.AddAction(player, () =>
            {
                if (target.WeenieType == WeenieType.Door)
                {
                    var door = (Door)target;

                    if (player.Skills[Skill.Lockpick].Status != SkillStatus.Trained && player.Skills[Skill.Lockpick].Status != SkillStatus.Specialized)
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouArentTrainedInLockpicking));
                        return;
                    }

                    void consume(Player plr, Lockpick lp)
                    {
                        lp.Structure--;
                        if (lp.Structure < 1)
                            plr.TryRemoveItemFromInventoryWithNetworking(this, 1);
                        plr.Session.Network.EnqueueSend(new GameEventUseDone(plr.Session));
                        plr.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(lp, PropertyInt.Structure, (int)lp.Structure));
                        plr.Session.Network.EnqueueSend(new GameMessageSystemChat($"Uses reamaining: {Structure}", ChatMessageType.System));
                    }

                    Door.UnlockDoorResults results = door.UnlockDoor(player.Skills[Skill.Lockpick].Current);
                    switch (results)
                    {
                        case Door.UnlockDoorResults.UnlockSuccess:
                            consume(player, this);
                            break;
                        case Door.UnlockDoorResults.DoorOpen:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockWhatIsOpen));
                            break;
                        case Door.UnlockDoorResults.AlreadyUnlocked:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.LockAlreadyUnlocked));
                            break;
                        case Door.UnlockDoorResults.PickLockFailed:
                            consume(player, this);
                            break;
                        default:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.KeyDoesntFitThisLock));
                            break;
                    }
                }
                else if (target.WeenieType == WeenieType.Chest)
                {
                    var message = new GameMessageSystemChat($"Unlocking {target.Name} has not been implemented, yet!", ChatMessageType.System);
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session), message);
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
