using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Common;

namespace ACE.Entity
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
                    if (Structure == 0)
                    {
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent);
                    }
                    else
                    {
                        Door door = target as Door;
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        if (door.UnlockDoor(KeyCode))
                        {
                            Structure--;
                            player.Session.Network.EnqueueSend(sendUseDoneEvent);
                        }
                        else
                        {
                            var message = new GameMessageSystemChat($"The {Name} cannot be used on the {target.Name}.", ChatMessageType.System);
                            player.Session.Network.EnqueueSend(sendUseDoneEvent, message);
                        }
                   }
                }
            });
            chain.EnqueueChain();
        }
    }
}
