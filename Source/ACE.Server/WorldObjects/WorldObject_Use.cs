
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        /// <summary>
        /// This is used to determine how close you need to be to use an item.
        /// NOTE: cheat factor (2) added for items with null use radius. Og II
        /// </summary>
        public float UseRadiusSquared => ((UseRadius ?? 2) + CSetup.Radius) * ((UseRadius ?? 2) + CSetup.Radius);

        public bool IsWithinUseRadiusOf(WorldObject wo)
        {
            return Location.SquaredDistanceTo(wo.Location) <= wo.UseRadiusSquared;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// </summary>
        public virtual void UseItem(Player player)
        {
            // Do Nothing by default
#if DEBUG
            var message = $"Default UseItem reached, this object ({Name}) not programmed yet.";
            player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.System));
            log.Error(message);
#endif

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public virtual void ActOnUse(WorldObject worldObject)
        {
            // Do Nothing by default
            if (worldObject is Player player)
            {
#if DEBUG
                var message = $"Default ActOnUse reached, this object ({Name}) not programmed yet.";
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.System));
                log.Error(message);
#endif
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            }
        }
    }
}
