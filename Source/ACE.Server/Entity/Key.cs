using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Entity
{
    public class Key : WorldObject
    {
        private string KeyCode
        {
            get;
            set;
        }

        public Key(AceObject aceO)
            : base(aceO)
        {
            Attackable = true;
            
            SetObjectDescriptionBools();

            KeyCode = AceObject.KeyCode ?? "";
            Structure = AceObject.Structure ?? AceObject.MaxStructure;
        }

        public void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(player, () =>
            {
                if (player == null)
                {
                    return;
                }

                if (!player.IsWithinUseRadiusOf(target))
                    player.DoMoveTo(target);
                else
                {
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    if (target.WeenieType == WeenieType.Door)
                    {
                        Door door = target as Door;
                        Door.UnlockDoorResults results = door.UnlockDoor(KeyCode);
                        switch (results)
                        {
                            case global::ACE.Server.Entity.Door.UnlockDoorResults.UnlockSuccess:
                                Structure--;
                                if (Structure < 1)
                                    player.HandleActionRemoveItemFromInventory(Guid.Full, player.Guid.Full, 1);

                                player.Session.Network.EnqueueSend(sendUseDoneEvent);
                                player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this.Sequences, Guid, PropertyInt.Structure, (int)Structure));
                                break;
                            case global::ACE.Server.Entity.Door.UnlockDoorResults.DoorOpen:
                                var messageDoorOpen = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.YouCannotLockWhatIsOpen);  // TODO: Messages are not quiet right. Need to find right one.
                                player.Session.Network.EnqueueSend(sendUseDoneEvent, messageDoorOpen);
                                break;
                            case global::ACE.Server.Entity.Door.UnlockDoorResults.AlreadyUnlocked:
                                var messageAlreadyUnlocked = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.KeyDoesntFitThisLock);  // TODO: Messages are not quiet right. Need to find right one.
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
