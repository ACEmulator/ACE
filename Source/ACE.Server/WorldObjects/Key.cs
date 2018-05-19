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
                if (target.WeenieType == WeenieType.Door)
                {
                    var door = (Door)target;

                    UnlockResults results = door.Unlock(keyCode);

                    switch (results)
                    {
                        case UnlockResults.UnlockSuccess:
                            Structure--;
                            if (Structure < 1)
                                player.TryRemoveItemFromInventoryWithNetworking(this, 1);

                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                            player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, (int)Structure));
                            break;
                        case UnlockResults.Open:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockWhatIsOpen));
                            break;
                        case UnlockResults.AlreadyUnlocked:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.LockAlreadyUnlocked));
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
