using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
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
                var sendUseDoneEvent = new GameEventUseDone(player.Session);

                if (target.WeenieType == WeenieType.Door)
                {
                    var door = (Door)target;

                    Door.UnlockDoorResults results = door.UnlockDoor(keyCode);

                    switch (results)
                    {
                        case Door.UnlockDoorResults.UnlockSuccess:
                            Structure--;
                            if (Structure < 1)
                                player.RemoveItemFromInventory(Guid.Full, player.Guid.Full, 1);

                            player.Session.Network.EnqueueSend(sendUseDoneEvent);
                            player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, (int)Structure));
                            break;
                        case Door.UnlockDoorResults.DoorOpen:
                            var messageDoorOpen = new GameEventWeenieError(player.Session, WeenieError.YouCannotLockWhatIsOpen); // TODO: Messages are not quiet right. Need to find right one.
                            player.Session.Network.EnqueueSend(sendUseDoneEvent, messageDoorOpen);
                            break;
                        case Door.UnlockDoorResults.AlreadyUnlocked:
                            var messageAlreadyUnlocked = new GameEventWeenieError(player.Session, WeenieError.KeyDoesntFitThisLock); // TODO: Messages are not quiet right. Need to find right one.
                            player.Session.Network.EnqueueSend(sendUseDoneEvent, messageAlreadyUnlocked);
                            break;
                        default:
                            var messageIncorrectKey = new GameEventWeenieError(player.Session, WeenieError.KeyDoesntFitThisLock);
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
                    var message = new GameEventWeenieError(player.Session, WeenieError.YouCannotLockOrUnlockThat);
                    player.Session.Network.EnqueueSend(sendUseDoneEvent, message);
                }
            });

            chain.EnqueueChain();
        }
    }
}
