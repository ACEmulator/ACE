using System.Collections.Generic;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Actions;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Entity
{
    public class Key : WorldObject
    {
        private static List<AceObjectPropertyId> _updateStructure = new List<AceObjectPropertyId>() { new AceObjectPropertyId((uint)PropertyInt.Structure, AceObjectPropertyType.PropertyInt) };

        private string KeyCode
        {
            get;
            set;
        }

        public Key(AceObject aceO)
            : base(aceO)
        {
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
                    Door door = target as Door;
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    Door.UnlockDoorResults results = door.UnlockDoor(KeyCode);
                    switch (results)
                    {
                        case Entity.Door.UnlockDoorResults.UnlockSuccess:
                            Structure--;
                            uint objectGuid = this.Guid.Full;
                            SendPartialUpdates(player.Session, _updateStructure);
                            if (Structure < 1)
                            {
                                // Remove item from players
                                player.DestroyInventoryItem(this);
                                // Clean up the shard database.
                                DatabaseManager.Shard.DeleteObject(SnapShotOfAceObject(), null);
                            }
                            player.Session.Network.EnqueueSend(sendUseDoneEvent);
                            break;
                        case Entity.Door.UnlockDoorResults.AlreadyUnlocked:
                            var messageAlreadyUnlocked = new GameMessageSystemChat($"The {target.Name} is already unlocked.", ChatMessageType.System);
                            player.Session.Network.EnqueueSend(sendUseDoneEvent, messageAlreadyUnlocked);
                            break;
                        default:
                            var message = new GameMessageSystemChat($"The {Name} cannot be used on the {target.Name}.", ChatMessageType.System);
                            player.Session.Network.EnqueueSend(sendUseDoneEvent, message);
                            break;
                    }
                }
            });
            chain.EnqueueChain();
        }
    }
}
