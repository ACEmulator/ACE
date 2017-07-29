// WeenieType.Generic

using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Entity
{
    public class GenericObject : WorldObject
    {
        public GenericObject(AceObject aceObject)
            : base(aceObject)
        {
        }

        public GenericObject(ObjectGuid guid, AceObject aceObject)
            : base(guid, aceObject)
        {
        }

        ////public override void HandleActionOnCollide(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        public override void HandleActionOnUse(ObjectGuid playerId)
        {
            // TODO: Implement

            if (CurrentLandblock != null)
            {
                ActionChain chain = new ActionChain();
                CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
                {
                    Player player = wo as Player;
                    if (player == null)
                    {
                        return;
                    }

                    var errorMessage = new GameMessageSystemChat($"Generic OnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);

                    player.Session.Network.EnqueueSend(errorMessage, sendUseDoneEvent);
                });
                chain.EnqueueChain();
            }
        }

        public override void OnUse(Session session)
        {
            var errorMessage = new GameMessageSystemChat($"Generic InternalOnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
            var sendUseDoneEvent = new GameEventUseDone(session);

            session.Network.EnqueueSend(errorMessage, sendUseDoneEvent);
        }
    }
}
