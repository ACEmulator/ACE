using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Entity.WorldObjects
{
    public class Key : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Key(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Key(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            DescriptionFlags |= ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Attackable, true);

            // These shoudl come from the weenie. After confirmation, remove these
            //KeyCode = AceObject.KeyCode ?? "";
            //Structure = AceObject.Structure ?? AceObject.MaxStructure;
        }

        public void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            ActionChain chain = new ActionChain();

            var keyCode = GetProperty(PropertyString.KeyCode);

            chain.AddAction(player, () =>
            {
                if (player == null)
                    return;

                if (!player.IsWithinUseRadiusOf(target))
                    player.DoMoveTo(target);
                else
                {
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);

                    if (target.WeenieType == WeenieType.Door)
                    {
                        Door door = target as Door;

                        Door.UnlockDoorResults results = door.UnlockDoor(keyCode);

                        switch (results)
                        {
                            case WorldObjects.Door.UnlockDoorResults.UnlockSuccess:
                                Structure--;
                                if (Structure < 1)
                                    player.HandleActionRemoveItemFromInventory(Guid.Full, player.Guid.Full, 1);

                                player.Session.Network.EnqueueSend(sendUseDoneEvent);
                                player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(Sequences, Guid, PropertyInt.Structure, (int)Structure));
                                break;
                            case WorldObjects.Door.UnlockDoorResults.DoorOpen:
                                var messageDoorOpen = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.YouCannotLockWhatIsOpen); // TODO: Messages are not quiet right. Need to find right one.
                                player.Session.Network.EnqueueSend(sendUseDoneEvent, messageDoorOpen);
                                break;
                            case WorldObjects.Door.UnlockDoorResults.AlreadyUnlocked:
                                var messageAlreadyUnlocked = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.KeyDoesntFitThisLock); // TODO: Messages are not quiet right. Need to find right one.
                                player.Session.Network.EnqueueSend(sendUseDoneEvent, messageAlreadyUnlocked);
                                break;
                            default:
                                var messageIncorrectKey = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.KeyDoesntFitThisLock);
                                player.Session.Network.EnqueueSend(sendUseDoneEvent, messageIncorrectKey);
                                break;
                        }
                    }
                    else if (target.WeenieType == WeenieType.Chest)
                    {
                        var message = new GameMessageSystemChat($"Unlocking {target.Name} has not been implemented, yet!", ChatMessageType.System);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent, message);
                    }
                    else
                    {
                        var message = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.YouCannotLockOrUnlockThat);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent, message);
                    }
                }
            });

            chain.EnqueueChain();
        }
    }
}
