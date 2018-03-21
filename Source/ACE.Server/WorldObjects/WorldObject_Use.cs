
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.
        /// </summary>
        public virtual void DoActionUseItem(Session session)
        {
            // Do Nothing by default
            #if DEBUG
            var errorMessage = new GameMessageSystemChat($"Default OnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
            session.Network.EnqueueSend(errorMessage);
            #endif

            session.Network.EnqueueSend(new GameEventUseDone(session));
        }

        public virtual void ActOnUse(ObjectGuid playerId)
        {
            // Do Nothing by default
            if (CurrentLandblock != null)
            {
                var player = CurrentLandblock.GetObject(playerId) as Player;

                if (player == null)
                    return;

                #if DEBUG
                var errorMessage = new GameMessageSystemChat($"Default HandleActionOnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
                player.Session.Network.EnqueueSend(errorMessage);
                #endif

                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            }
        }
    }
}
